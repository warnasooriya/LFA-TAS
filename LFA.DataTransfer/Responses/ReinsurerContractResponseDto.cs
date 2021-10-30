using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    public class ReinsurerContractResponseDto
    {
        public Guid Id { get; set; }
        public Guid LinkContractId { get; set; }
        public Guid ReinsurerId { get; set; }
        public string UWYear { get; set; }
        public Guid CountryId { get; set; }
        public Guid CommodityTypeId { get; set; }
        public Guid InsurerId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string ContractNo { get; set; }
        public bool IsActive { get; set; }
        public Guid BrokerId { get; set; }  
        public DateTime EntryDateTime { get; set; }
        public Guid EntryUser { get; set; }

        public bool IsReinsurerContractExists { get; set; }

    }
}
