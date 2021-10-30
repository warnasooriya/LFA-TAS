using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.Entities
{
    public class ClaimStatusCode
    {
        public virtual Guid Id { get; set; }
        public virtual string StatusCode { get; set; }
        public virtual string Description { get; set; }
        public virtual int DisplayOrder { get; set; }

        
    }
}
