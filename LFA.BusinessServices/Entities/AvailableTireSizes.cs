using System;

namespace TAS.Services.Entities
{
    public class AvailableTireSizes
    {
        public virtual Guid Id { get; set; }
        public virtual String Width { get; set; }
        public virtual String CrossSection { get; set; }
        public virtual int Diameter { get; set; }
        public virtual string LoadSpeed { get; set; }
        public virtual Decimal TirePrice { get; set; }
        public virtual Decimal CurrencyRate { get; set; }
        public virtual Guid CurrencyId { get; set; }
        public virtual Guid CurrencyPeriodId { get; set; }
        public virtual Decimal OriginalTireDepth { get; set; }

    }
}
