using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.Entities
{
    public class FaultCategory
    {
        public virtual Guid Id { get; set; }
        public virtual string FaultCategoryCode { get; set; }
        public virtual string FaultCategoryName { get; set; }
        public virtual bool IsActive { get; set; }
    }
}
