using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.Entities
{
    public class OtherItemServiceHistory
    {
        public virtual Guid Id { get; set; }
        public virtual Guid OtherItemId { get; set; }
        public virtual int ServiceNumber { get; set; }
        public virtual decimal Milage { get; set; }
        public virtual DateTime ServiceDate { get; set; }
        public virtual string Remarks { get; set; }

    }
}
