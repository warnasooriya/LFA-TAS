using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.Entities
{
    public class ClaimProcessingStat
    {
        public virtual Guid Id { get; set; }
        public virtual Guid ClaimId { get; set; }
        public virtual Guid UserId { get; set; }
        public virtual string UserName { get; set; }
        public virtual DateTime TimeOpened { get; set; }
        public virtual DateTime TimeStarted { get; set; }
        public virtual DateTime TimeCompleted { get; set; }
        public virtual bool IsAssignedToOther { get; set; }
        public virtual string AssignedUserName { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual DateTime EntryDate { get; set; }


    }
}
