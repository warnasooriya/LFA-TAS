using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.Entities
{
    public class CurrencyEmail
    {
        public virtual Guid Id { get; set; }
        public virtual string TPAEmail { get; set; }
        public virtual string AdminEmail { get; set; }
        public virtual int FirstEmailDuration { get; set; }
        public virtual string FirstDurationType { get; set; }
        public virtual string FirstMailSubject { get; set; }
        public virtual string FirstMailBody { get; set; }
        public virtual int SecoundEmailDuration { get; set; }
        public virtual string SecoundDurationType { get; set; }
        public virtual string SecoundMailSubject { get; set; }
        public virtual string SecoundMailBody { get; set; }
        public virtual int LastEmailDuration { get; set; }
        public virtual string LastDurationType { get; set; }
        public virtual string LastMailSubject { get; set; }
        public virtual string LastMailBody { get; set; }
    }
}
