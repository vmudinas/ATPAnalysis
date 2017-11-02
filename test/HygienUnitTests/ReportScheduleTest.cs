using Infrastructure.BusinessLogic;
using Infrastructure.ClientEntities;
using System;
using System.Collections.Generic;
using Xunit;

namespace HygienUnitTests
{
    public class ReportScheduleTest
    {
        [Theory]
        [InlineData(1)]
        public void TestReportSchedules(int i)
        {
            // using proper "Now" as date is proving too difficult because the recurrence string must by dynamic.
            // Wednesday, May 10, 2017 at 8:30 am (zero seconds) - to UTC
            DateTime dtNowUTC = new DateTime(2017, 5, 10, 8, 30, 0).ToUniversalTime();
            
            ReportScheduleLogic rsl = new ReportScheduleLogic();
            List<ClientReportSchedule> lstClientReportSchedule = GetClientReportSchedules(dtNowUTC, out int schedulesExpectedToRunNow);
            List<ReportSchedulePayload> lstReportSchedulePayload = rsl.GetCurrentReportSchedulePayload(lstClientReportSchedule, dtNowUTC.ToString("s"), true);

            Assert.Equal(schedulesExpectedToRunNow, lstReportSchedulePayload.Count);
        }

        /// <summary>
        /// returns a list of client report schedules such that some will yield a payload and some will not
        /// </summary>
        /// <param name="dtNow">hard-coded to Wednesday, May, 10th, 2017 at 8:30am - zero seconds</param>
        /// <param name="expectedSchedules">so that more "good" tests can be added dynamically</param>
        /// <returns></returns>
        private List<ClientReportSchedule> GetClientReportSchedules(DateTime dtNow, out int expectedSchedules)
        {
            List<ClientReportSchedule> lstCRS = new List<ClientReportSchedule>();
            Guid tstGuid = Guid.NewGuid(); // not validating actual ids in this test.

            // Note: the terms "Good" and "Bad" refer only to whether the value should return a report schedule payload
            string tstGoodEndDate = dtNow.AddMinutes(8).ToString("s");
            string tstGoodStartDate = dtNow.AddMinutes(10).ToString("s");

            // *** recurring - good and bad ***

            // daily recurring - every day, ending after 10 ocurrences, ending on July 22nd, 2500 & every other day ending on June 22nd, 2500
            List<string> lstGoodDayRecur = new List<string>() { "FREQ=DAILY", "FREQ=DAILY;COUNT=10",
                "FREQ=DAILY;UNTIL=25000722T065959Z", "FREQ=DAILY;UNTIL=25000622T065959Z;INTERVAL=2" };
            // weekly recurring - Wed forever, Wed ending after 2 occurences, Wed ending after June 17th, 2500
            List<string> lstGoodWeekRecur = new List<string>() { "FREQ=WEEKLY;BYDAY=MO,WE,FR",
                "FREQ=WEEKLY;BYDAY=MO,WE;COUNT=2", "FREQ=WEEKLY;BYDAY=WE,FR;UNTIL=25000617T065959Z" };
            // monthly recurring - on 10th, on 10th ending after 4 occurrences, every 2 months on 10th ending June 13th, 2500
            List<string> lstGoodMonthRecur = new List<string>() { "FREQ=MONTHLY;BYMONTHDAY=10", "FREQ=MONTHLY;BYMONTHDAY=10;COUNT=4",
                "FREQ=MONTHLY;BYMONTHDAY=10;INTERVAL=2;UNTIL=25000613T065959Z" };
            // yearly recurring - May 10th every two years ending after 5 occurrences, every year on May 10th - ending July 26th, 2500, every year on May 10th forver
            List<string> lstGoodYearRecur = new List<string>() { "FREQ=YEARLY;BYMONTHDAY=10;BYMONTH=5;INTERVAL=2;COUNT=5",
                "FREQ=YEARLY;BYMONTHDAY=10;BYMONTH=5;UNTIL=25000727T065959Z", "FREQ=YEARLY;BYMONTHDAY=10;BYMONTH=5" };

            // daily recurring - ending in the past
            List<string> lstBadDayRecur = new List<string>() { "FREQ=DAILY;UNTIL=20120722T065959Z" };
            // weekly recurring - non-Wed forever, non-Wed ending after 2 occurences, Wed ending in the past
            List<string> lstBadWeekRecur = new List<string>() { "FREQ=WEEKLY;BYDAY=MO,FR",
                "FREQ=WEEKLY;BYDAY=MO;COUNT=2", "FREQ=WEEKLY;BYDAY=WE,FR;UNTIL=20140617T065959Z"};
            // monthly recurring - on 7th, on 2nd ending after 4 occurrences, every 2 months on 10th ending in the past
            List<string> lstBadMonthRecur = new List<string>() { "FREQ=MONTHLY;BYMONTHDAY=7", "FREQ=MONTHLY;BYMONTHDAY=2;COUNT=4",
                "FREQ=MONTHLY;BYMONTHDAY=10;INTERVAL=2;UNTIL=20160613T065959Z"};
            // yearly recurring - May 21st every two years ending after 5 occurrences, every year on May 10th - ending in past
            List<string> lstBadYearRecur = new List<string>() { "FREQ=YEARLY;BYMONTHDAY=21;BYMONTH=5;INTERVAL=2;COUNT=5",
                "FREQ=YEARLY;BYMONTHDAY=10;BYMONTH=5;UNTIL=20110727T065959Z" };

            foreach (string recur in lstGoodDayRecur) { AddToScheduleList(lstCRS, recur, tstGuid, tstGoodStartDate, tstGoodEndDate); }
            foreach (string recur in lstGoodWeekRecur) { AddToScheduleList(lstCRS, recur, tstGuid, tstGoodStartDate, tstGoodEndDate); }
            foreach (string recur in lstGoodMonthRecur) { AddToScheduleList(lstCRS, recur, tstGuid, tstGoodStartDate, tstGoodEndDate); }
            foreach (string recur in lstGoodYearRecur) { AddToScheduleList(lstCRS, recur, tstGuid, tstGoodStartDate, tstGoodEndDate); }

            expectedSchedules = (lstCRS != null && lstCRS.Count > 0) ? lstCRS.Count : 0; // needs to be here, before "bad" assignments

            foreach (string recur in lstBadDayRecur) { AddToScheduleList(lstCRS, recur, tstGuid, tstGoodStartDate, tstGoodEndDate); }
            foreach (string recur in lstBadWeekRecur) { AddToScheduleList(lstCRS, recur, tstGuid, tstGoodStartDate, tstGoodEndDate); }
            foreach (string recur in lstBadMonthRecur) { AddToScheduleList(lstCRS, recur, tstGuid, tstGoodStartDate, tstGoodEndDate); }
            foreach (string recur in lstBadYearRecur) { AddToScheduleList(lstCRS, recur, tstGuid, tstGoodStartDate, tstGoodEndDate); }

            // *** non-recurring ***

            AddToScheduleList(lstCRS, string.Empty, tstGuid, tstGoodStartDate, tstGoodEndDate); // should be good
            expectedSchedules++;

            return lstCRS;
        }

        private void AddToScheduleList(List<ClientReportSchedule> lstCRS, string recur, Guid tstGuid, string tstStartDate, string tstEndDate)
        {
            lstCRS.Add(new ClientReportSchedule()
            {
                // hard-coded below exist because don't matter for this test
                AccountId = tstGuid,
                EmailList = "test@test.com",
                EndDate = tstEndDate,
                StartDate = tstStartDate,
                RecurrenceRule = recur,
                ReportId = tstGuid,
                ReportPeriod = 1,
                ScheduleId = tstGuid,
                SiteIds = null,
                Text = "Unit Test Title"
            });
        }
    }
}
