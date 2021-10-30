using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.Entities
{
    public class CommodityType
    {
        public virtual Guid CommodityTypeId { get; set; }
        public virtual string CommodityTypeDescription { get; set; }
        public virtual string DisplayDescription { get; set; }
        public virtual string CommonCode { get; set; }
        public virtual string CommodityCode { get; set; }

        
        
    }
}
