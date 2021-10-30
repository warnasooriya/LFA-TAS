using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.Entities
{
    public class PartArea
    {
        public virtual Guid Id { get; set; }
        public virtual Guid CommodityTypeId { get; set; }
        public virtual Guid CommodityCategoryId { get; set; }
        public virtual string PartAreaCode { get; set; }
        public virtual string PartAreaName { get; set; }

    }
}
