using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.Entities
{
    public class ItemStatus
    {
        public virtual Guid Id { get; set; }
        public virtual string Status { get; set; }
        public virtual string ItemStatusDescription { get; set; }
        public virtual DateTime EntryDateTime { get; set; }
        public virtual Guid EntryUser { get; set; }
    }
}
