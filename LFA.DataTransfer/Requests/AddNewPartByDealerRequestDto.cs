using System;

namespace TAS.DataTransfer.Requests
{
    public class AddNewPartByDealerRequestDto
    {
        public Guid CommodityId { get; set; }
        public Guid PartAreaId { get; set; }
        public Guid MakeId { get; set; }
        public string PartName { get; set; }
        public string PartCode { get; set; }
        public string PartNumber { get; set; }
        public decimal AllocatedHours { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; }
        public bool ApplicableForAllModels { get; set; }
        public Guid DealerId { get; set; }
        public Guid EntryBy { get; set; }
        
    }
}
