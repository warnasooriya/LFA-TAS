using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class DealerLocationRequestDto
    {
        public Guid Id { get; set; }
        public Guid DealerId { get; set; }
        public Guid CityId { get; set; }
        public Guid TpaBranchId { get; set; }
        public string SalesContactPerson { get; set; }
        public string SalesTelephone { get; set; }
        public string SalesFax { get; set; }
        public string SalesEmail { get; set; }
        public string Location { get; set; }
        public string LocationCode { get; set; }
        public string ServiceContactPerson { get; set; }
        public string ServiceTelephone { get; set; }
        public string ServiceFax { get; set; }
        public string DealerAddress { get; set; }
        public string ServiceEmail { get; set; }
        public bool HeadOfficeBranch { get; set; }
        public DateTime EntryDateTime { get; set; }
        public Guid EntryUser { get; set; }

        public bool DealerLocationInsertion
        {
            get;
            set;
        }

    }
}
