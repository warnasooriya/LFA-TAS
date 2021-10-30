using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.Entities
{
    public class Currency
    {
        public virtual Guid Id { get; set; }
        public virtual string Country { get; set; }
        public virtual string CurrencyName { get; set; }
        public virtual string Code { get; set; }
        public virtual string Symbol { get; set; }
    }
}
