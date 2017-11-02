using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Infrastructure.Entities;


namespace Infrastructure.ClientEntities
{
 
    public class ClientAccount
    {

        public virtual Guid AccountId { get; set; }

        public virtual string UserName { get; set; }

        public List<SiteUser> Sites { get; set; }
    }
}