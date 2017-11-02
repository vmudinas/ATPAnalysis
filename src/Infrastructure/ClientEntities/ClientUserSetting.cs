using System;

namespace Infrastructure.ClientEntities
{
    // ClientUserSetting is a stripped down version of UserSetting to exclude unnecessary items like schemas and identifiers
    public class ClientUserSetting
    {
        public virtual byte? ResultsPeriod { get; set; }
        public virtual DateTime? ResultsDateFrom { get; set; }
        public virtual DateTime? ResultsDateTo { get; set; }
        public virtual byte? DashType { get; set; }
        public virtual byte? DashPeriod { get; set; }
        public virtual DateTime? DashDateFrom { get; set; }
        public virtual DateTime? DashDateTo { get; set; }
        public virtual string ReportScheduleCurrentView { get; set; }
    }
}