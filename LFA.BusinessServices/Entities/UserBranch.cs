using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.Entities
{
    public class UserBranch
    {
        public virtual Guid Id { get; set; }
        public virtual Guid InternalUserId { get; set; }
        public virtual Guid TPABranchId { get; set; }
    }
}
