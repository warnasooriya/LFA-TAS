using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.Entities
{
    public class Region
    {
        public virtual Guid Id { get; set; }
        public virtual string RegionName { get; set; }
        public virtual string RegionCode { get; set; }
    }
}
