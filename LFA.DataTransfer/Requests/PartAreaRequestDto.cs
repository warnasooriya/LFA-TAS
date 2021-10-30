using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class PartAreaRequestDto
    {
        public Guid Id { get; set; }
        public Guid CommodityTypeId { get; set; }
        public Guid CommodityCategoryId { get; set; }
        public string PartAreaCode { get; set; }
        public string PartAreaName { get; set; }

        public bool PartAreaInsertion
        {
            get;
            set;
        }

    }
}
