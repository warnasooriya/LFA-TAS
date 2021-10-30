using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.Entities
{
    public class DriveType
    {
        public virtual Guid Id { get; set; }
        public virtual string Type { get; set; }
        public virtual string DriveTypeDescription { get; set; }
        public virtual DateTime EntryDateTime { get; set; }
        public virtual Guid EntryUser { get; set; }
    }
}
