using System;
using System.Collections.Generic;
using Infrastructure.ClientEntities;
using Infrastructure.Entities;
using System.Globalization;

namespace Infrastructure.BusinessLogic
{
    public class ReportScheduleLogic
    {
        private string byMonthDay = string.Empty, byMonth = string.Empty, byDay = string.Empty, interval = string.Empty;
        private string recurrenceFrequency = string.Empty, recurUntil = string.Empty, recurCount = string.Empty;

        /// <summary>
        /// need a guaranteed way to determine exact date & time from last sent stamp. iterate days from last sent and return match.
        /// if no match for a particular day, return DateTime.MinValue, else return date
        /// </summary>
        /// <param name="crs"></param>
        /// <param name="dtLastSent"></param>
        /// <returns></returns>
        public DateTime? GetNextSendDateFromSchedule(ClientReportSchedule crs, DateTime? dtLastSent)
        {
            if (string.IsNullOrWhiteSpace(crs.RecurrenceRule)) { return null; } // non-recurring schedules have no NextSend date concept

            DateTime dtLastSentNotNull = dtLastSent.HasValue ? dtLastSent.Value : DateTime.UtcNow;
            DateTime dtOneYearFromLastSent = dtLastSentNotNull.AddDays(397); // 365 + 32 days for an extra 31 day month + 1 day for leap year
            var dtNextSend = DateTime.MinValue;

            // reduce uneccessary CPU usage by fast forwarding for monthly and yearly recurrences (leaving room near edges)
            if (crs.RecurrenceRule.ToUpper().Contains("YEARLY")) { dtLastSentNotNull = dtLastSentNotNull.AddDays(362); }
            else if (crs.RecurrenceRule.ToUpper().Contains("MONTHLY")) { dtLastSentNotNull = dtLastSentNotNull.AddDays(25); }

            do
            {
                dtLastSentNotNull = dtLastSentNotNull.AddHours(24);

                var lstReportSchedulePayload = GetCurrentReportSchedulePayload(crs, dtLastSentNotNull.ToString("s"), false, true);

                if (lstReportSchedulePayload != null && lstReportSchedulePayload.Count > 0)
                {
                    dtNextSend = lstReportSchedulePayload[0].DateStamp;
                    break;
                }
            } while (DateTime.Compare(dtLastSentNotNull, dtOneYearFromLastSent) < 1);

            return dtNextSend;
        }

        private List<ReportSchedulePayload> GetCurrentReportSchedulePayload(ClientReportSchedule crs, string dtLatestOfStartOrLastSent,
                                                                            bool isFromUnitTest = false, bool shouldGetNextSendDate = false)
        {
            List<ClientReportSchedule> lstClientReportSchedule = new List<ClientReportSchedule>();

            if (crs != null) { lstClientReportSchedule.Add(crs); }

            return GetCurrentReportSchedulePayload(lstClientReportSchedule, dtLatestOfStartOrLastSent, isFromUnitTest, shouldGetNextSendDate);
        }

        /// <summary>
        ///     Returns a payload list of currently scheduled reports.
        /// </summary>
        /// <param name="lstReportSchedule">schedules read from database for now's 30 minute time slot -- OR -- non-null recurrence strings</param>
        /// <param name="latestOfStartLastSentOrNextSend">if from a unit test, uses a set date (May 10th, 2017 as of development)</param>
        /// <param name="isFromUnitTest">overload. unit test send a list. live sends one schedule at a time.</param>
        /// <param name="shouldGetNextSendDate">upon stamping last sent schedules, need to calculate and stamp next send</param>
        /// <returns></returns>
        public List<ReportSchedulePayload> GetCurrentReportSchedulePayload(IEnumerable<ClientReportSchedule> lstClientReportSchedule, 
                                                                           string latestOfStartLastSentOrNextSend, bool isFromUnitTest,
                                                                           bool shouldGetNextSendDate = false)
        {
            var lstReportSchedulePayload = new List<ReportSchedulePayload>();
            int recurCountInt = 0, byMonthInt = 0, byMonthDayInt = 0, intervalInt = 0;
            DateTime dtRecurrenceEnding = DateTime.Now, dtLatestOfStartLastSentOrNextSend = DateTime.UtcNow;
            DateTime dtNowUTC = DateTime.UtcNow, dtNowUTCFixed = DateTime.UtcNow;
            bool doesRecurrenceEnd = false, hasRecurrenceEnded = false;
            string[] arrRecurrenceRule = null, arrByDay = null;
            var subRule = string.Empty;

            if (!string.IsNullOrWhiteSpace(latestOfStartLastSentOrNextSend))
            {
                //dtLatestOfStartLastSentOrNextSend = DateTimeConverter.SafeParseUtc(latestOfStartLastSentOrNextSend);
                dtLatestOfStartLastSentOrNextSend = DateTimeConverter.MomentUtcToDateTime(latestOfStartLastSentOrNextSend);
            }

            if (isFromUnitTest) { dtNowUTC = dtLatestOfStartLastSentOrNextSend; }

            foreach (var rs in lstClientReportSchedule)
            {
                if (string.IsNullOrWhiteSpace(rs.RecurrenceRule))
                {
                    // Falls within timeslot and non-recurring. Add to list of candidates as is.
                    AddToCandidateList(lstReportSchedulePayload, rs, dtLatestOfStartLastSentOrNextSend);

                    if (shouldGetNextSendDate) { return lstReportSchedulePayload; } // should not happen here since non-recurring

                    continue;
                }

                // Calculate if need to run based on parsing recurrence rule
                arrRecurrenceRule = rs.RecurrenceRule.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                if (arrRecurrenceRule == null || arrRecurrenceRule.Length < 1) { continue; } // should never happen

                ResetRecurrenceVars();

                for (var i = 0; i < arrRecurrenceRule.Length; i++) // populates recurrence vars
                {
                    if (string.IsNullOrWhiteSpace(arrRecurrenceRule[i])) { continue; } // should never happen

                    subRule = arrRecurrenceRule[i].ToUpper().Trim();

                    if (subRule.Contains("FREQ=")) { recurrenceFrequency = subRule.Replace("FREQ=", string.Empty); }
                    else if (subRule.Contains("UNTIL=")) { recurUntil = subRule.Replace("UNTIL=", string.Empty); }
                    else if (subRule.Contains("COUNT=")) { recurCount = subRule.Replace("COUNT=", string.Empty); }
                    else if (subRule.Contains("BYMONTHDAY=")) { byMonthDay = subRule.Replace("BYMONTHDAY=", string.Empty); }
                    else if (subRule.Contains("BYMONTH=")) { byMonth = subRule.Replace("BYMONTH=", string.Empty); }
                    else if (subRule.Contains("BYDAY=")) { byDay = subRule.Replace("BYDAY=", string.Empty); }
                    else if (subRule.Contains("INTERVAL=")) { interval = subRule.Replace("INTERVAL=", string.Empty); }
                }

                if (string.IsNullOrWhiteSpace(recurrenceFrequency)) { continue; } // should never happen

                // check recurrence rule vars for schedule candidates
                doesRecurrenceEnd = !(string.IsNullOrWhiteSpace(recurUntil) && string.IsNullOrWhiteSpace(recurCount));
                hasRecurrenceEnded = false;

                if (doesRecurrenceEnd)
                {
                    if (string.IsNullOrWhiteSpace(recurCount)) // recurrence ends on a date ("until" & "count" are mutually exclusive)
                    {
                        recurUntil = GetParsableDateFromDevEx(recurUntil);
                            
                        if (string.IsNullOrWhiteSpace(recurUntil)) { continue; }

                        dtRecurrenceEnding = DateTimeConverter.MomentUtcToDateTime(recurUntil);
                    }
                    else // ends after a certain count
                    {
                        int.TryParse(recurCount, out recurCountInt);
                        dtRecurrenceEnding = DateTimeConverter.MomentUtcToDateTime(rs.StartDate);

                        if (recurrenceFrequency == "DAILY") { dtRecurrenceEnding = dtRecurrenceEnding.AddDays(recurCountInt); }
                        else if (recurrenceFrequency == "WEEKLY") { dtRecurrenceEnding = dtRecurrenceEnding.AddDays(recurCountInt * 7); }
                        else if (recurrenceFrequency == "MONTHLY") { dtRecurrenceEnding = dtRecurrenceEnding.AddMonths(recurCountInt); }
                        else if (recurrenceFrequency == "YEARLY") { dtRecurrenceEnding = dtRecurrenceEnding.AddYears(recurCountInt); }
                    }

                    hasRecurrenceEnded = dtRecurrenceEnding <= dtNowUTC;
                }

                if (hasRecurrenceEnded) { continue; }

                if (recurrenceFrequency == "DAILY")
                {
                    if (string.IsNullOrWhiteSpace(interval)) { intervalInt = 1; }
                    else { int.TryParse(interval, out intervalInt); }

                    if (intervalInt > 0 && (dtLatestOfStartLastSentOrNextSend.Day - DateTimeConverter.MomentUtcToDateTime(rs.StartDate).Day) % intervalInt == 0)
                    {
                        AddToCandidateList(lstReportSchedulePayload, rs, dtLatestOfStartLastSentOrNextSend);
                        if (shouldGetNextSendDate) { return lstReportSchedulePayload; }
                    }
                }
                else if (recurrenceFrequency == "WEEKLY")
                {
                    // split byDay on "," for individual days of week
                    arrByDay = byDay.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                    if (arrByDay != null && arrByDay.Length > 0)
                    {
                        for (var i = 0; i < arrByDay.Length; i++)
                        {
                            if (string.IsNullOrWhiteSpace(arrByDay[i])) { continue; }

                            arrByDay[i] = arrByDay[i].Trim();

                            if ((arrByDay[i] == "SU" && dtLatestOfStartLastSentOrNextSend.DayOfWeek == DayOfWeek.Sunday) ||
                                (arrByDay[i] == "MO" && dtLatestOfStartLastSentOrNextSend.DayOfWeek == DayOfWeek.Monday) ||
                                (arrByDay[i] == "TU" && dtLatestOfStartLastSentOrNextSend.DayOfWeek == DayOfWeek.Tuesday) ||
                                (arrByDay[i] == "WE" && dtLatestOfStartLastSentOrNextSend.DayOfWeek == DayOfWeek.Wednesday) ||
                                (arrByDay[i] == "TH" && dtLatestOfStartLastSentOrNextSend.DayOfWeek == DayOfWeek.Thursday) ||
                                (arrByDay[i] == "FR" && dtLatestOfStartLastSentOrNextSend.DayOfWeek == DayOfWeek.Friday) ||
                                (arrByDay[i] == "SA" && dtLatestOfStartLastSentOrNextSend.DayOfWeek == DayOfWeek.Saturday))
                            {
                                AddToCandidateList(lstReportSchedulePayload, rs, dtLatestOfStartLastSentOrNextSend);

                                if (shouldGetNextSendDate) { return lstReportSchedulePayload; }

                                break; // today is one of the weekdays in the weekly recurrence rule.
                            }
                        }
                    }
                }
                else if (recurrenceFrequency == "MONTHLY" || recurrenceFrequency == "YEARLY")
                {
                    int.TryParse(byMonthDay, out byMonthDayInt);

                    if (string.IsNullOrWhiteSpace(interval)) { intervalInt = 1; }
                    else { int.TryParse(interval, out intervalInt); }

                    if ((dtNowUTCFixed.Day == byMonthDayInt && dtLatestOfStartLastSentOrNextSend <= dtNowUTCFixed)
                        || dtLatestOfStartLastSentOrNextSend.Day == byMonthDayInt) // this day of month assuming start date [or last sent] has occurred
                    {
                        if (recurrenceFrequency == "MONTHLY")
                        {
                            if (intervalInt > 0 &&
                                (dtLatestOfStartLastSentOrNextSend.Month - DateTimeConverter.MomentUtcToDateTime(rs.StartDate).Month) % intervalInt == 0) // check for skipping months
                            {
                                AddToCandidateList(lstReportSchedulePayload, rs, dtNowUTCFixed);

                                if (shouldGetNextSendDate) { return lstReportSchedulePayload; }
                            }
                        }
                        else // YEARLY
                        {
                            int.TryParse(byMonth, out byMonthInt);

                            if (dtNowUTCFixed.Month == byMonthInt || dtLatestOfStartLastSentOrNextSend.Month == byMonthInt)
                            {
                                if (intervalInt > 0 &&
                                    (dtLatestOfStartLastSentOrNextSend.Year - DateTimeConverter.MomentUtcToDateTime(rs.StartDate).Year) % intervalInt == 0) // check for skipping years
                                {
                                    AddToCandidateList(lstReportSchedulePayload, rs, dtNowUTCFixed);

                                    if (shouldGetNextSendDate) { return lstReportSchedulePayload; }
                                }
                            }
                        }
                    }
                }
            }

            return lstReportSchedulePayload;
        }

        private List<ClientReportSchedule> GetClientReportSchedule(IEnumerable<ReportSchedule> lstReportSchedule)
        {
            List<ClientReportSchedule> lstClientReportSchedule = new List<ClientReportSchedule>();

            foreach (var rs in lstReportSchedule)
            {
                lstClientReportSchedule.Add(new ClientReportSchedule()
                {
                    AccountId = rs.AccountId,
                    EmailList = rs.EmailList,
                    EndDate = rs.EndDate.ToString("s"),
                    LastSent = rs.LastSent,
                    NextSend = rs.NextSend,
                    RecurrenceRule = rs.RecurrenceRule,
                    ReportId = rs.ReportId,
                    ReportPeriod = rs.ReportPeriod,
                    ScheduleId = rs.ScheduleId,
                    SiteIds = null, // not needed in this context, so set as null
                    StartDate = rs.StartDate.ToString("s"),
                    Text = rs.ScheduleTitle
                });
            }

            return lstClientReportSchedule;
        }

        /// <summary>
        /// Iterates lstReportSchedule appending Report Schedule Payload list with
        /// each Report Schedule latest of either StartDate or LastSent date
        /// </summary>
        /// <param name="lstReportSchedulePayload"></param>
        /// <param name="lstReportSchedule"></param>
        /// <param name="dtNowUTC"></param>
        public void AppendFinalReportSchedulePayloadList(List<ReportSchedulePayload> lstReportSchedulePayload, 
                                                         IEnumerable<ReportSchedule> lstReportSchedule, DateTime dtNowUTC)
        {
            var dtLatestOfStartLastSentOrNextSend = DateTime.UtcNow;
            var dtStart = DateTime.UtcNow;
            var dtNextSendFromRecurrence = DateTime.UtcNow;
            var lstClientReportSchedule = GetClientReportSchedule(lstReportSchedule);

            foreach (var crs in lstClientReportSchedule)
            {
                dtStart = DateTimeConverter.MomentUtcToDateTime(crs.StartDate);
                dtLatestOfStartLastSentOrNextSend = GetLatestOfStartLastSentOrNextSendDate(dtStart, crs);

                while (DateTime.Compare(dtLatestOfStartLastSentOrNextSend, dtNowUTC) < 0) // dtLatestOfStartLastSentOrNextSend < dtNowUTC
                {
                    if (string.IsNullOrWhiteSpace(crs.RecurrenceRule)) // send non-recurring only once - break while (not foreach) loop
                    {
                        lstReportSchedulePayload.AddRange(GetCurrentReportSchedulePayload(crs, dtLatestOfStartLastSentOrNextSend.ToString("s")));
                        break;
                    }

                    lstReportSchedulePayload.AddRange(GetCurrentReportSchedulePayload(crs, dtLatestOfStartLastSentOrNextSend.ToString("s"))); // recurring

                    dtLatestOfStartLastSentOrNextSend = dtLatestOfStartLastSentOrNextSend.AddHours(24);
                }
            }
        }

        private DateTime GetLatestOfStartLastSentOrNextSendDate(DateTime dtStart, ClientReportSchedule crsIn)
        {
            DateTime? dtLastSentNullable = crsIn.LastSent, dtNextSendNullable = crsIn.NextSend;
            DateTime dtLatest = DateTime.UtcNow, dtLastSent = DateTime.UtcNow, dtNextSend = DateTime.UtcNow;

            dtLastSent = dtLastSentNullable.HasValue ? dtLastSentNullable.Value : DateTime.MinValue;
            dtNextSend = dtNextSendNullable.HasValue ? dtNextSendNullable.Value : DateTime.MinValue;

            dtLatest = (DateTime.Compare(dtNextSend, dtLastSent) > 0) ? dtNextSend : dtLastSent;

            if (DateTime.Compare(dtStart, dtLatest) > 0) { dtLatest = dtStart; }

            if (dtLatest == DateTime.MinValue) { dtLatest = DateTime.UtcNow.AddMinutes(-5); } // should never happen - give 5 minute buffer

            return dtLatest;
        }

        private void AddToCandidateList(List<ReportSchedulePayload> lstCandidates, ClientReportSchedule rsIn, DateTime dtLatestOfStartLastSentOrNextSend)
        {
            lstCandidates.Add(new ReportSchedulePayload
            {
                ReportId = rsIn.ReportId,
                DateStamp = dtLatestOfStartLastSentOrNextSend,
                EmailList = rsIn.EmailList,
                EndDate = rsIn.EndDate,
                StartDate = dtLatestOfStartLastSentOrNextSend.ToString("s"),
                SiteIds = rsIn.SiteIds,
                ReportPeriod = rsIn.ReportPeriod,
                ScheduleName = rsIn.Text,
                ScheduleId = rsIn.ScheduleId
            });
        }

        /// <summary>
        /// converts DevExtreme scheduler date time string (arriving as: "YYYYMMDDThhmmssZ") to a parsable format.
        /// </summary>
        /// <param name="devExDateString"></param>
        /// <returns></returns>
        private string GetParsableDateFromDevEx(string devExDateString)
        {
            if (string.IsNullOrWhiteSpace(devExDateString)) { return string.Empty; }

            string strip = devExDateString.Replace("T", string.Empty).Replace("Z", string.Empty);

            if (strip.Length < 14) { return string.Empty; }

            string parsable = $"{strip.Substring(0,4)}-{strip.Substring(4, 2)}-{strip.Substring(6, 2)} {strip.Substring(8, 2)}:{strip.Substring(10, 2)}:{strip.Substring(12, 2)}";

            return parsable;
        }

        private void ResetRecurrenceVars()
        {
            recurrenceFrequency = recurUntil = recurCount = byMonthDay = byMonth = byDay = interval = string.Empty;
        }

        /// <summary>
        /// populates a generic string list from a comma seperated string of emails
        /// </summary>
        /// <param name="emailList"></param>
        /// <param name="emailCSV"></param>
        public void PopulateEmailList(List<string> emailList, string emailCSV)
        {
            if (emailList == null || string.IsNullOrWhiteSpace(emailCSV))
            {
                emailList = new List<string>();
                return;
            }

            string[] arrEmailCSV = emailCSV.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            if (arrEmailCSV == null || arrEmailCSV.Length < 1)
            {
                emailList = new List<string>();
            }
            else
            {
                emailList.Clear();

                for (int i = 0; i < arrEmailCSV.Length; i++)
                {
                    if (string.IsNullOrWhiteSpace(arrEmailCSV[i])) { continue; }

                    emailList.Add(arrEmailCSV[i].Trim());
                }
            }
        }

        public static bool PopulateStartEndDatesFromPeriod(byte reportPeriod, ref DateTime dtStart, ref DateTime dtEnd)
        {
            DateTime dtNowUTC = DateTime.UtcNow;
            int calcStartDay = 0, calcEndDay = 0; // calculated days

            try
            {
                if (reportPeriod == (byte)RptPeriod.today)
                {
                    dtStart = new DateTime(dtNowUTC.Year, dtNowUTC.Month, dtNowUTC.Day, 0, 0, 0);
                    dtEnd = new DateTime(dtNowUTC.Year, dtNowUTC.Month, dtNowUTC.Day, 11, 59, 59);
                }
                else if (reportPeriod == (byte)RptPeriod.yesterday)
                {
                    DateTime dtYesterdayUTC = dtNowUTC.AddDays(-1);
                    dtStart = new DateTime(dtYesterdayUTC.Year, dtYesterdayUTC.Month, dtYesterdayUTC.Day, 0, 0, 0);
                    dtEnd = new DateTime(dtYesterdayUTC.Year, dtYesterdayUTC.Month, dtYesterdayUTC.Day, 11, 59, 59);
                }
                else if (reportPeriod == (byte)RptPeriod.thisWeek)
                {
                    calcStartDay = dtNowUTC.Day - (int)dtNowUTC.DayOfWeek;
                    dtStart = new DateTime(dtNowUTC.Year, dtNowUTC.Month, calcStartDay, 0, 0, 0);
                    dtEnd = new DateTime(dtNowUTC.Year, dtNowUTC.Month, dtNowUTC.Day, 11, 59, 59);
                }
                else if (reportPeriod == (byte)RptPeriod.lastWeek)
                {
                    DateTime dt7DaysAgoUTC = dtNowUTC.AddDays(-7);
                    calcStartDay = dt7DaysAgoUTC.Day - (int)dt7DaysAgoUTC.DayOfWeek;
                    calcEndDay = dt7DaysAgoUTC.Day + ((int)DayOfWeek.Saturday - (int)dt7DaysAgoUTC.DayOfWeek);
                    dtStart = new DateTime(dt7DaysAgoUTC.Year, dt7DaysAgoUTC.Month, calcStartDay, 0, 0, 0);
                    dtEnd = new DateTime(dt7DaysAgoUTC.Year, dt7DaysAgoUTC.Month, calcEndDay, 11, 59, 59);
                }
                else if (reportPeriod == (byte)RptPeriod.thisMonth)
                {
                    dtStart = new DateTime(dtNowUTC.Year, dtNowUTC.Month, 1, 0, 0, 0);
                    dtEnd = new DateTime(dtNowUTC.Year, dtNowUTC.Month, dtNowUTC.Day, 11, 59, 59);
                }
                else if (reportPeriod == (byte)RptPeriod.lastMonth)
                {
                    calcEndDay = new DateTime(dtNowUTC.Year, dtNowUTC.Month, 1, 0, 0, 0).AddHours(-8).Day;
                    DateTime dtLastMonthUTC = dtNowUTC.AddMonths(-1);
                    dtStart = new DateTime(dtLastMonthUTC.Year, dtLastMonthUTC.Month, 1, 0, 0, 0);
                    dtEnd = new DateTime(dtLastMonthUTC.Year, dtLastMonthUTC.Month, calcEndDay, 11, 59, 59);
                }
                else if (reportPeriod == (byte)RptPeriod.thisQuarter)
                {
                    dtStart = GetQuarterDate(dtNowUTC, true, false);
                    dtEnd = GetQuarterDate(dtNowUTC, false, false);
                }
                else if (reportPeriod == (byte)RptPeriod.lastQuarter)
                {
                    dtStart = GetQuarterDate(dtNowUTC, true, true);
                    dtEnd = GetQuarterDate(dtNowUTC, false, true);
                }
                else if (reportPeriod == (byte)RptPeriod.thisYear)
                {
                    dtStart = new DateTime(dtNowUTC.Year, 1, 1, 0, 0, 0);
                    dtEnd = new DateTime(dtNowUTC.Year, dtNowUTC.Month, dtNowUTC.Day, 11, 59, 59);
                }
                else if (reportPeriod == (byte)RptPeriod.lastYear)
                {
                    dtStart = new DateTime((dtNowUTC.Year - 1), 1, 1, 0, 0, 0);
                    dtEnd = new DateTime((dtNowUTC.Year - 1), 12, 31, 11, 59, 59);
                }

                return true;
            }
            catch (Exception) { return false; } // static method needs a return type.
        }

        public enum RptPeriod { today = 0, yesterday, thisWeek, lastWeek, thisMonth, lastMonth, thisQuarter, lastQuarter, thisYear, lastYear }

        private static DateTime GetQuarterDate(DateTime dtNowUTC, bool shouldGetStartDate, bool isPreviousQtr)
        {
            DateTime dtAdjustedNowUTC = dtNowUTC, dtQtrReturn = dtNowUTC;

            if (isPreviousQtr)
            {
                int currMonthModulus = ((dtNowUTC.Month - 1) % 3) + 1; // ensures we don't subtract too many months
                dtAdjustedNowUTC = dtNowUTC.AddMonths(-currMonthModulus);
            }

            int monthStart = 1, yearStart = dtAdjustedNowUTC.Year, currMonth = dtAdjustedNowUTC.Month;

            if (currMonth > 0 && currMonth <= 3)
            {
                monthStart = isPreviousQtr ? 10 : 1;
                if (isPreviousQtr) { yearStart--; }
            }
            else if (currMonth > 3 && currMonth <= 6) { monthStart = isPreviousQtr ? 1 : 4; }
            else if (currMonth > 6 && currMonth <= 9) { monthStart = isPreviousQtr ? 4 : 7; }
            else if (currMonth > 9 && currMonth <= 12) { monthStart = isPreviousQtr ? 7 : 10; }

            if (shouldGetStartDate) { dtQtrReturn = new DateTime(yearStart, monthStart, 1, 0, 0, 0); }
            else
            {
                int lastDayOfMonth = new DateTime(yearStart, (monthStart+1), 1, 0, 0, 0).AddHours(-8).Day;
                dtQtrReturn = new DateTime(yearStart, monthStart, lastDayOfMonth, 11, 59, 59);
            }

            return dtQtrReturn;
        }

        public string GetReportMessage(string reportName, string scheduleTitle, DateTime dtStart, DateTime dtEnd, bool isSubject)
        {
            // TODO: complete - be careful to format dates per culture
            string retMsg = string.Empty;
            string startDate = dtStart.ToString("yyyy-MM-ddTHH:mm", CultureInfo.InvariantCulture);
            string endDate = dtEnd.ToString("yyyy-MM-ddTHH:mm", CultureInfo.InvariantCulture);

            if (isSubject) { retMsg = $"{scheduleTitle} starting {startDate}"; }
            else { retMsg = $"{reportName} from {startDate} to {endDate} \n\nActual Report To Go Here...\n\n"; }

            return retMsg;
        }

    }
}
 
 