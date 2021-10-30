using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.Entities
{
    public class PartPrice
    {
        public virtual Guid Id { get; set; }
        public virtual Guid CountryId { get; set; }
        public virtual Guid DealerId { get; set; }
        public virtual Guid CurrencyId { get; set; }
        public virtual Guid CurrencyPeriodId { get; set; }
        public virtual Guid PartId { get; set; }
        
        public virtual decimal Price { get; set; }
        public virtual decimal ConversionRate { get; set; }
     
    }
}
