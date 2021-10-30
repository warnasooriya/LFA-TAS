using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Common
{
    public class BordxTaxInfo
    {
        public virtual Guid CountryId { get; set; }
        public virtual List<CountryTaxInfo> CountryTaxes { get; set; }
    }

    public class CountryTaxInfo
    {
        public virtual Guid TaxId { get; set; }
        public virtual string TaxName { get; set; }
        public virtual string TaxCode { get; set; }
        public virtual bool OnNRP { get; set; }
        public virtual bool OnGross { get; set; }
        public virtual Guid TaxTypeId { get; set; }
        public virtual Decimal TaxValue { get; set; }
        public virtual bool IsPercentage { get; set; }
        public virtual bool IsOnPreviousTax { get; set; }
        public virtual bool IsOnNRP { get; set; }
        public virtual bool IsOnGross { get; set; }
        public virtual Decimal MinimumValue { get; set; }
        public virtual int IndexVal { get; set; }
    }

    public class TaxColumnInfo
    {
        public virtual int Row { get; set; }
        public virtual int Column { get; set; }
        public virtual CountryTaxInfo taxInformation { get; set; }
    }
}
