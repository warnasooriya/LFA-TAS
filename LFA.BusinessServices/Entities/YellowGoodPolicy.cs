using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.Entities
{
    public class YellowGoodPolicy
    {
        public virtual Guid Id { get; set; }
        public virtual Guid YellowGoodId { get; set; }
        public virtual Guid PolicyId { get; set; }
    }
}
