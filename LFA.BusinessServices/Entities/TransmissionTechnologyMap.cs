using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.Entities
{
    public class TransmissionTechnologyMap
    {
        public virtual Guid Id { get; set; }
        public virtual Guid TransmissionTechnologyId { get; set; }
        public virtual Guid TransmissionId { get; set; }
    }
  
}
