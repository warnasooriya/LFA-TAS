using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.Entities
{
    public class CurrencyConversions
    {
        public virtual Guid Id { get; set; }
        public virtual decimal Rate { get; set; }
        public virtual Guid CurrencyId { get; set; }
        public virtual Guid CurrencyConversionPeriodId { get; set; }
        public virtual string Comment { get; set; }
    }
}
