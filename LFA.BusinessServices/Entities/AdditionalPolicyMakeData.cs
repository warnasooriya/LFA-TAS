using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.Entities
{
    public class AdditionalPolicyMakeData
    {
        public virtual Guid Id { get; set; }
        public virtual string MakeName { get; set; }
        public virtual string MakeCode { get; set; }
        public virtual bool IsActive { get; set; }
    }
}
