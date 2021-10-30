using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.Entities
{
    public class TPA
    {
        public virtual Guid Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string TelNumber { get; set; }
        public virtual string Address { get; set; }
        public virtual Guid Banner { get; set; }
        public virtual Guid Banner2 { get; set; }
        public virtual Guid Banner3 { get; set; }
        public virtual Guid Banner4 { get; set; }
        public virtual Guid Banner5 { get; set; }
        public virtual Guid Logo { get; set; }
        public virtual string DiscountDescription { get; set; }
        public virtual string OriginalTPAName { get; set; }
        public virtual string TpaCode { get; set; }
        public virtual string LanguageCode { get; set; }


    }
}
