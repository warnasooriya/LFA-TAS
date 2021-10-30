using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TAS.DataTransfer.Requests;

namespace TAS.DataTransfer.Responses
{
    public class PartResponseDto
    {
        public Guid Id { get; set; }
        public Guid PartAreaId { get; set; }
        public Guid MakeId { get; set; }
        public Guid CommodityId { get; set; }
        public string PartCode { get; set; }
        public string PartName { get; set; }
        public string PartNumber { get; set; }
        public bool IsActive { get; set; }
        public bool ApplicableForAllModels { get; set; }
        public decimal AllocatedHours { get; set; }
        public List<PartPriceReq> PartPrices { get; set; }
    }

   
}
