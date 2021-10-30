using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class DealerRequestDto
    {
        public Guid Id { get; set; }
        public string DealerCode { get; set; }
        public string DealerName { get; set; }
        public string DealerAliase { get; set; }
        public Guid CountryId { get; set; }
        public Guid CurrencyId { get; set; }
        public string Type { get; set; }
        public Guid CommodityTypeId { get; set; }
        public Guid InsurerId { get; set; }
        public List<Guid> Makes { get; set; }
        public Guid CityId { get; set; }
        public string Location { get; set; }
        public decimal ManHourRate { get; set; }
        public bool IsActive { get; set; }
        public bool IsAutoApproval { get; set; }
        public DateTime EntryDateTime { get; set; }
        public Guid EntryUser { get; set; }

        public bool DealerInsertion
        {
            get;
            set;
        }

    }
}
