using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.Entities
{
    public class CustomerComplaint
    {
        public virtual Guid Id { get; set; }
        public virtual string ComplaintCode { get; set; }
        public virtual string Complaint { get; set; }
    }
}
