using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.Entities
{
    public class TASTPA
    {
        public virtual Guid Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string TelNumber { get; set; }
        public virtual string Address { get; set; }
        public virtual Guid Banner { get; set; }
        public virtual Guid Logo { get; set; }
        public virtual string DiscountDescription { get; set; }
        public virtual string DBName { get; set; }
        public virtual string DBConnectionString { get; set; }
        public virtual string DBConnectionStringViewOnly { get; set; }
        public virtual string OriginalTPAName { get; set; }

    }
}
