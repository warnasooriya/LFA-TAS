using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.Entities
{
    public class ClaimComment
    {
        public virtual Guid Id { get; set; }
        public virtual Guid PolicyId { get; set; }
        public virtual Guid ClaimId { get; set; }
        public virtual string Comment { get; set; }
        public virtual Guid SentFrom { get; set; }
        public virtual Guid SentTo { get; set; }
        public virtual bool ByTPA { get; set; }
        public virtual bool Seen { get; set; }
        public virtual DateTime EntryDateTime { get; set; }
        public virtual DateTime SeenDateTime { get; set; }

    }
}
