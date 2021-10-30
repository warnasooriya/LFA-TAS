using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.Entities
{
    public class Part
    {
        public virtual Guid Id { get; set; }
        public virtual Guid PartAreaId { get; set; }
        public virtual Guid MakeId { get; set; }
        public virtual Guid CommodityId { get; set; }
        public virtual string PartCode { get; set; }
        public virtual string PartName { get; set; }  
        public virtual string PartNumber { get; set; } 
        public virtual bool IsActive { get; set; } 
        public virtual bool ApplicableForAllModels { get; set; } 
        public virtual DateTime EntryDateTime { get; set; } 
        public virtual Guid EntryBy { get; set; }
        public virtual decimal AllocatedHours { get; set; }
      
    }
}
