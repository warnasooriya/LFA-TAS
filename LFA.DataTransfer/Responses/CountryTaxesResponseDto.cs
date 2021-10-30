using System;

namespace TAS.DataTransfer.Responses
{
    [Serializable]
    public class CountryTaxesResponseDto
    {
        public Guid Id { get; set; }
        public Guid CountryId { get; set; }
        public Guid TaxTypeId { get; set; }
        public Decimal TaxValue { get; set; }
        public bool IsPercentage { get; set; }
        public bool IsOnPreviousTax { get; set; }
        public bool IsOnNRP { get; set; }
        public bool IsOnGross { get; set; }
        public Decimal MinimumValue { get; set; }
        public int IndexVal { get; set; }
        public Guid currencyPeriodId { get; set; }
        public Guid TpaCurrencyId { get; set; }
        public decimal ConversionRate { get; set; }
        public decimal ValueIncluededInPolicy { get; set; }
        public bool IsCountryTaxesExists { get; set; }
    }

    public class CountryTaxesDe
    {
        public Guid TaxTypesId { get; set; }
        public string TaxName { get; set; }
        public Decimal TaxValue { get; set; }
        public Decimal MinimumValue { get; set; }
        public bool IsPercentage { get; set; }
    }
}
