using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.Entities
{
    public class CommodityCategory
    {
        public virtual Guid CommodityCategoryId { get; set; }
        public virtual string CommodityCategoryCode { get; set; }
        public virtual string CommodityCategoryDescription { get; set; }
        public virtual Guid CommodityTypeId { get; set; }
        public virtual int Length { get; set; }
    }
}
