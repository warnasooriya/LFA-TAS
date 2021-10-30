using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.Entities
{
    public class AvailableTireSizesPattern
    {
        public virtual Guid Id { get; set; }
        public virtual Guid AvailableTireSizesId { get; set; }
        public virtual string Pattern { get; set; }
    }
}
