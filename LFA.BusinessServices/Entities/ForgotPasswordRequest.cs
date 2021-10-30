using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.Entities
{
    public class ForgotPasswordRequest
    {
        public virtual Guid Id { get; set; }
        public virtual string SystemUserId { get; set; }
        public virtual string TempKey { get; set; }
        public virtual DateTime RequestedTime { get; set; }
        public virtual DateTime ExpiryTime { get; set; }
        public virtual bool IsUsed { get; set; }
        public virtual string PreviousPassword { get; set; }
    }
}
