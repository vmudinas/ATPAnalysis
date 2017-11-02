using System.Collections.Generic;

namespace Infrastructure.ClientEntities
{
    public class UserManagement
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string RoleName { get; set; }

        public List<ClientSites> Sites { get; set; }
    }
}