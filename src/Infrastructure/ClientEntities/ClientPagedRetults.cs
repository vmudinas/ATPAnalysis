using System.Collections.Generic;

//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.ClientEntities
{
    public class ClientPagedResults
    {
        public IEnumerable<ClientResult> Items { get; set; }
    }
}