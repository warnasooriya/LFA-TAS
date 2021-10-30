using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.Entities
{
    public class Fault
    {
        public virtual Guid Id { get; set; }
        public virtual Guid FaultCategoryId { get; set; }
        public virtual Guid FaultAreaId { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual string FaultCode { get; set; }
        public virtual string FaultName { get; set; }
    }
}
