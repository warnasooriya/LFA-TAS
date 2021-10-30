using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.Entities
{
    public class FaultArea
    {

        public virtual Guid Id { get; set; }
        public virtual string FaultAreaCode { get; set; }
        public virtual string FaultAreaName { get; set; }
        public virtual bool IsActive { get; set; }
    }
}
