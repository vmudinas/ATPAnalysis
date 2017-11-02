using System.Collections.Generic;

namespace Infrastructure.ClientEntities
{
    public class CloudSyncReturn
    {
        public int ResultsAdded { get; set; }
        public int ResultsUpdated { get; set; }
        public int ResultsErrors { get; set; }
        public List<string> ErrorList { get; set; }
    }
}