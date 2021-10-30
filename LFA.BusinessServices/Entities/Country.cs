using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.Entities
{
    public class Country
    {
        public virtual Guid Id { get; set; }
        public virtual Guid CurrencyId { get; set; }
        public virtual string CountryCode { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual string CountryName { get; set; }
        public virtual string PhoneCode { get; set; }
    }
    [Serializable]
    public class CountryInfo:Country
    {
        public virtual List<Guid> Makes { get; set; }
        public virtual List<Guid> Models { get; set; }
    }
    public class CountryTaxes
    {
        public virtual Guid Id { get; set; }
        public virtual Guid CountryId { get; set; }
        public virtual Guid TaxTypeId { get; set; }
        public virtual Decimal TaxValue { get; set; }
        public virtual bool IsPercentage { get; set; }
        public virtual bool IsOnPreviousTax { get; set; }
        public virtual bool IsOnNRP { get; set; }
        public virtual bool IsOnGross { get; set; }
        public virtual Decimal MinimumValue { get; set; }
        public virtual int IndexVal { get; set; }
        public virtual Guid currencyPeriodId { get; set; }
        public virtual Guid TpaCurrencyId { get; set; }
        public virtual Decimal ConversionRate { get; set; }

    }
    public class CountryMakes
    {
        public virtual Guid Id { get; set; }
        public virtual Guid CountryId { get; set; }
        public virtual Guid MakeId { get; set; }
    }
    public class CountryModeles
    {
        public virtual Guid Id { get; set; }
        public virtual Guid CountryId { get; set; }
        public virtual Guid ModelId { get; set; }
    }
}
