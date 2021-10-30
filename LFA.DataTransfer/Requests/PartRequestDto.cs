using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class PartRequestDto
    {
        public PartReq Part { get; set; }
        public List<PartPriceReq> PartPrice { get; set; }
        public Guid UserId { get; set; }
    }

    public class PartReq
    {
        public Guid Id { get; set; }
        public Guid PartAreaId { get; set; }
        public Guid MakeId { get; set; }
        public Guid CommodityId { get; set; }
        public string PartCode { get; set; }
        public string PartName { get; set; }
        public string PartNumber { get; set; }
        public decimal AllocatedHours { get; set; }
        public bool IsActive { get; set; }
        public bool ApplicableForAllModels { get; set; }
    }

    public class PartPriceReq
    {
        public int Id { get; set; }
        public Guid CountryId { get; set; }
        public Guid DealerId { get; set; }
        public string CountryName { get; set; }
        public string CurrencyName { get; set; }
        public string DealerName { get; set; }
        public decimal Price { get; set; }
    }
}
