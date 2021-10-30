using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.Entities
{
    public class ClaimNotes
    {
        public virtual Guid Id { get; set; }
        public virtual Guid PolicyId { get; set; }
        public virtual Guid ClaimId { get; set; }
        public virtual Guid SubmittedUserId { get; set; }
        public virtual string Note { get; set; }
        public virtual DateTime EntryDateTime { get; set; }
    }
}
