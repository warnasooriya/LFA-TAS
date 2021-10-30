using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.Entities
{
    public class DealerLocation
    {
        public virtual Guid Id { get; set; }
        public virtual Guid DealerId { get; set; }
        public virtual Guid CityId { get; set; }
        public virtual Guid TpaBranchId { get; set; }
        public virtual string SalesContactPerson { get; set; }
        public virtual string SalesTelephone { get; set; }
        public virtual string SalesFax { get; set; }
        public virtual string SalesEmail { get; set; }
        public virtual string Location { get; set; }
        public virtual string LocationCode { get; set; }
        public virtual string ServiceContactPerson { get; set; }
        public virtual string ServiceTelephone { get; set; }
        public virtual string ServiceFax { get; set; }
        public virtual string ServiceEmail { get; set; }
        public virtual string DealerAddress { get; set; }
        public virtual bool HeadOfficeBranch { get; set; }
        public virtual DateTime EntryDateTime { get; set; }
        public virtual Guid EntryUser { get; set; }
    }
}
