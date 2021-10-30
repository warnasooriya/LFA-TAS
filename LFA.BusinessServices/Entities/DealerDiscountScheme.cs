using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.Entities
{
    public class DealerDiscountScheme
    {
        public virtual Guid Id { get; set; }
        public virtual string SchemeName { get; set; }
        public virtual string SchemeCode { get; set; }

        public virtual bool IsActive { get; set; }
        
    }
}
