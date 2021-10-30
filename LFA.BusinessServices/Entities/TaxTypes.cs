using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.Entities
{
    public class TaxTypes
    {
        public virtual Guid Id { get; set; }
        public virtual string TaxName { get; set; }
        public virtual string TaxCode { get; set; }
        public virtual bool OnNRP { get; set; }
        public virtual bool OnGross { get; set; }
    }
}
