using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure.BusinessLogic;
using Infrastructure.ClientEntities;
using Infrastructure.Entities;
using Infrastructure.Services.Abstraction;
using Infrastructure.UserModel.Models;

namespace Infrastructure.Services
{
    public partial class DataService : IDataService
    {
        public IEnumerable<ClientReportSchedule> GetReportSchedules(string userName)
        {
            var accountId = GetDefaultAccountId(userName);
            var lstRptScheds = _repo.Get<ReportSchedule>(x => x.AccountId == accountId).ToList();
            var lstClientRptScheds = new List<ClientReportSchedule>();
            var lstClientRptSchedSite = new List<ClientReportScheduleSite>();
            var lstSitesPerAcct = _repo.Get<Site>(x => x.AccountId == accountId).ToList(); // this is where we get site name

            foreach (var rs in lstRptScheds)
            {
                lstClientRptSchedSite.Clear();

                lstClientRptSchedSite.AddRange(_repo.Get<ReportScheduleSite>(x => x.ScheduleId == rs.ScheduleId)
                    .ToList()
                    .Select(rss => new ClientReportScheduleSite
                    {
                        Id = rss.Id,
                        ScheduleId = rss.ScheduleId,
                        SiteId = rss.SiteId,
                        Name = lstSitesPerAcct.Where(x => x.SiteId == rss.SiteId).Select(x => x.Name).FirstOrDefault()
                    }));

                lstClientRptScheds.Add(new ClientReportSchedule
                {
                    Text = rs.ScheduleTitle,
                    StartDate = rs.StartDate.ToString("s"),
                    AccountId = rs.AccountId,
                    EmailList = rs.EmailList,
                    EndDate = rs.EndDate.ToString("s"),
                    RecurrenceRule = rs.RecurrenceRule,
                    ReportId = rs.ReportId,
                    ReportPeriod = rs.ReportPeriod,
                    ScheduleId = rs.ScheduleId,
                    LastSent = rs.LastSent,
                    NextSend = rs.NextSend,
                    SiteIds = new List<ClientReportScheduleSite>(lstClientRptSchedSite)
                });
            }

            return lstClientRptScheds;
        }

        public void UpdateReportSchedule(ClientReportSchedule crs, string userName)
        {
            var lstRss = _repo.Get<ReportScheduleSite>(x => x.ScheduleId == crs.ScheduleId).ToList();

            // if ReportScheduleSite table has sites not in crs.SiteIds, remove them
            _repo.RemoveRange(
                _repo.Get<ReportScheduleSite>(
                    x =>
                        x.ScheduleId == crs.ScheduleId &&
                        !crs.SiteIds.Any(c => c.ScheduleId == x.ScheduleId && c.SiteId == x.SiteId)
                ).ToList()
            );

            // add crs.SiteIds to ReportScheduleSite table that do not already exist there
            foreach (var crss in crs.SiteIds)
            {
                if (!lstRss.Any(x => x.ScheduleId == crss.ScheduleId && x.SiteId == crss.SiteId))
                {
                    _repo.Add(new ReportScheduleSite
                    {
                        Id = Guid.NewGuid(),
                        SiteId = crss.SiteId,
                        ScheduleId = crss.ScheduleId
                    });
                }
            }

            var startDate = DateTimeConverter.MomentUtcToDateTime(crs.StartDate);
            var adjustedLastSent = GetAdjustedLastSent(crs.LastSent, startDate);

            DateTime? adjustedNextSend = null;

            if (adjustedLastSent != null)
            {
                var rsl = new ReportScheduleLogic();
                adjustedNextSend = rsl.GetNextSendDateFromSchedule(crs, adjustedLastSent);
            }

            var rs = new ReportSchedule
            {
                AccountId = GetDefaultAccountId(userName),
                EmailList = crs.EmailList,
                EndDate = DateTimeConverter.MomentUtcToDateTime(crs.EndDate),
                RecurrenceRule = crs.RecurrenceRule,
                ReportId = crs.ReportId,
                ReportPeriod = crs.ReportPeriod,
                ScheduleId = crs.ScheduleId,
                ScheduleTitle = crs.Text,
                LastSent = adjustedLastSent,
                NextSend = adjustedNextSend,
                StartDate = startDate,
                UserId = GetUserId(userName)
            };

            _repo.UpdateSave(rs);
        }

        /// <summary>
        /// This method compares Start Date and Last Sent, nulling Last Sent if it is less than or equal to Start Date. 
        /// It also adjusts the Last Sent time window to the Start Date's window if they differ by 30 minutes or more.
        /// The DevExtreme scheduling widget allows formerly run schedules' start date to be increased beyond Last Sent date, 
        /// which can cause logic errors because the Last Sent date does not get reset to null in that case. Logically, 
        /// Start Date should always be less than Last Sent (if Last Sent not null).
        /// </summary>
        /// <param name="lastSentIn"></param>
        /// <param name="startDateIn"></param>
        /// <returns></returns>
        private DateTime? GetAdjustedLastSent(DateTime? lastSentIn, DateTime startDateIn)
        {
            if (lastSentIn == null) { return null; }

            DateTime? adjLastSent;

            if (DateTime.Compare(lastSentIn.Value, startDateIn) < 1)  { adjLastSent = null; } // LastSent <= StartDate
            else
            {
                // schedule edited such that time window was changed. need to adjust last sent window, lest out of phase
                if (GetMinutesFromTimespan(lastSentIn.Value.TimeOfDay - startDateIn.TimeOfDay) > 30)
                {
                    adjLastSent = new DateTime(lastSentIn.Value.Year, lastSentIn.Value.Month, lastSentIn.Value.Day, 
                                               startDateIn.Hour, startDateIn.Minute, startDateIn.Second);
                }
                else { adjLastSent = lastSentIn.Value; }
            }

            return adjLastSent;
        }

        private int GetMinutesFromTimespan(TimeSpan tsIn)
        {
            var minutes = 0;

            if (tsIn.Days > 0) { minutes += (tsIn.Days * 1440); } // 1440 minutes in a day
            if (tsIn.Hours > 0) { minutes += (tsIn.Hours * 60); }

            minutes += tsIn.Minutes;

            return Math.Abs(minutes);
        }

        public void AddReportSchedule(ClientReportSchedule crs, string userName)
        {
            var userId = _repo.Get<ApplicationUser>(x => x.UserName == userName).FirstOrDefault().Id ?? string.Empty;
            var newScheduleId = Guid.NewGuid();
            var guidUserId = Guid.Parse(userId);
            var accountId = _repo.Get<UserAccount>(x => x.UserId == guidUserId).FirstOrDefault().AccountId;

            foreach (var crss in crs.SiteIds)
            {
                var rss = new ReportScheduleSite {Id = Guid.NewGuid(), ScheduleId = newScheduleId, SiteId = crss.SiteId};
                _repo.Add(rss);
            }

            var rs = new ReportSchedule
            {
                AccountId = accountId,
                ScheduleId = newScheduleId,
                EmailList = crs.EmailList,
                EndDate = DateTimeConverter.MomentUtcToDateTime(crs.EndDate),
                RecurrenceRule = crs.RecurrenceRule,
                ReportId = crs.ReportId,
                ReportPeriod = crs.ReportPeriod,
                ScheduleTitle = crs.Text,
                StartDate = DateTimeConverter.MomentUtcToDateTime(crs.StartDate),
                UserId = guidUserId
            };

            _repo.Add(rs);
            _repo.Save();
        }

        public void DeleteReportSchedule(string accountId, string scheduleId)
        {
            _repo.Remove(
                _repo.Get<ReportSchedule>(
                    x => x.AccountId.ToString() == accountId && x.ScheduleId.ToString() == scheduleId).FirstOrDefault());

            _repo.RemoveRange(_repo.Get<ReportScheduleSite>(x => x.ScheduleId.ToString() == scheduleId).ToList());

            _repo.Save();
        }

        /// <summary>
        /// Main method called by the Azure service
        /// </summary>
        public async void SendScheduledReports()
        {
            // get scheduled reports where start date's time within TIME_SLOT_MINUTES and recurrence rule exists and not sent recently - OR -
            // start date is before now, last sent date is null and recurrence rule does not exist (i.e. non-recurring)
            const int timeSlotMinutes = 31;
            var dtNowUtc = DateTime.UtcNow;
            var dtStart = DateTime.UtcNow;
            var dtEnd = DateTime.UtcNow;
            var rsl = new ReportScheduleLogic();
            var lstReportSchedulePayload = new List<ReportSchedulePayload>();
            var dtNowUtcStart = dtNowUtc.AddMinutes(-timeSlotMinutes);
            TimeSpan tsUtcNowStart = dtNowUtcStart.TimeOfDay, tsUtcNowEnd = dtNowUtc.TimeOfDay;

            var lstReportScheduleRecurring = _repo.Get<ReportSchedule>(
                x => !string.IsNullOrWhiteSpace(x.RecurrenceRule) && 
                     (
                        (x.NextSend == null && x.StartDate.TimeOfDay >= tsUtcNowStart && x.StartDate.TimeOfDay <= tsUtcNowEnd) ||
                        (x.NextSend != null && x.NextSend >= dtNowUtcStart && x.NextSend <= dtNowUtc)
                     )
                ).OrderBy(x => x.StartDate).ThenBy(x => x.NextSend ?? DateTime.MinValue).ToList();

            rsl.AppendFinalReportSchedulePayloadList(lstReportSchedulePayload, lstReportScheduleRecurring, dtNowUtc);

            var lstReportScheduleNonRecurring = _repo.Get<ReportSchedule>(
                x => (x.StartDate <= dtNowUtc && x.LastSent == null && string.IsNullOrWhiteSpace(x.RecurrenceRule))).OrderBy(x => x.StartDate).ToList();

            rsl.AppendFinalReportSchedulePayloadList(lstReportSchedulePayload, lstReportScheduleNonRecurring, dtNowUtc);

            // TODO: once report tables exist, need a client entity that represents ReportDetails (?) or ReportAttributes (?).
            //       these would be, ReportId, ReportName and various filter lists (User, Plan, Location, etc.) linked to ReportId.
            //       Lists would probably be mostly blank (except for ReportName) for non-customized reports.
            
            if (lstReportSchedulePayload.Count < 1) { return; }

            lstReportSchedulePayload = lstReportSchedulePayload.OrderBy(x => x.DateStamp).ToList(); // crucial for ReportSchedule.LastSent logic
            var lstEmail = new List<string>();

            // now we have the complete list of current schedules and (possibly) past failed-to-send schedules
            foreach (var rsp in lstReportSchedulePayload)
            {
                rsl.PopulateEmailList(lstEmail, rsp.EmailList);

                // 1. LINQ select details (i.e. ReportName, filter lists, etc.) about rsp.ReportId from param list - cannot do until tables exist.
                // 2. Calculate start and end dates from either rsp.ReportPeriod - or - rsp.StartDate and rsp.EndDate
                //    (if they are both > DateTime.MinValue). Note: rsp.StartDate and rsp.EndDate would be sent for past failed reports.
                // 3. Format report subject & message [above graph] based on info from steps 1 & 2
                // 4. Get report from generator
                // 5. Attach report to email and send

                // 1. 
                const string rptName = "Work in progress (WIP)";

                // 2.
                // will rsp.StartDate & rsp.EndDate be empty here?
                // if (string.IsNullOrWhiteSpace(rsp.StartDate) && string.IsNullOrWhiteSpace(rsp.EndDate)) { }
                // else { }
                ReportScheduleLogic.PopulateStartEndDatesFromPeriod(rsp.ReportPeriod, ref dtStart, ref dtEnd);

                // 3. 
                var rptSubject = rsl.GetReportMessage(rptName, rsp.ScheduleName, dtStart, dtEnd, true);
                var rptMessage = rsl.GetReportMessage(rptName, rsp.ScheduleName, dtStart, dtEnd, false);

                // 4. Get report from generator

                // 5. 
                try
                {
                    await EmailSender.SendEmailAsync(lstEmail, $"Subject: {rptSubject}",
                    $"Message: {rptMessage}",
                    Config.GetSection("EmailUserName").Value,
                    Config.GetSection("EmailAddress").Value,
                    Config.GetSection("EmailPassword").Value,
                    Config.GetSection("SmtpServer").Value,
                    int.Parse(Config.GetSection("SmtpServerPort").Value));

                    var rs = _repo.Get<ReportSchedule>(x => x.ScheduleId == rsp.ScheduleId).FirstOrDefault();

                    DateTime? dtLastSentStamp = DateTime.UtcNow;
                    rs.LastSent = dtLastSentStamp;

                    // TODO: Need to calculate NextSend date and stamp it along with LastSent
                    var clientRptSched = new ClientReportSchedule()
                    {
                        RecurrenceRule = rs.RecurrenceRule,
                        NextSend = rs.NextSend,
                        AccountId = rs.AccountId,
                        EmailList = rs.EmailList,
                        EndDate = string.Empty,
                        LastSent = rs.LastSent,
                        ReportId = rs.ReportId,
                        ReportPeriod = rs.ReportPeriod,
                        ScheduleId = rs.ScheduleId,
                        SiteIds = null,
                        StartDate = rs.StartDate.ToString("s"),
                        Text = rs.ScheduleTitle
                    };

                    rs.NextSend = rsl.GetNextSendDateFromSchedule(clientRptSched, dtLastSentStamp);

                    _repo.UpdateSave(rs);
                }
                catch (Exception ex)
                {
                    var errorMsg = string.IsNullOrWhiteSpace(ex.Message) ? ex.InnerException.ToString() : ex.Message;
                    //catch (Exception ex) ==> TODO: log error ex?
                    return;
                }
            }
        }

    }
}