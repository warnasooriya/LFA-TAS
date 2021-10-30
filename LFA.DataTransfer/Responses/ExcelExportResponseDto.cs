using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using TAS.DataTransfer.Common;

namespace TAS.DataTransfer.Responses
{
    public class ExcelExportResponseDto
    {
        public List<DataTable> InvoiceCodedetails { get; set; }
        public List<InvoiceCodeColumnHeaders> InvoiceCodeColumnHeaders { get; set; }
        public List<BordxReportColumns> BordxReportColumns { get; set; }
        public string TpaName { get; set; }
        public string BordxYear { get; set; }
        public string BordxMonth { get; set; }
        public List<BordxTaxInfo> CountryTaxInfo { get; set; }
        public decimal currentUSDConversionRate { get; set; }
        public string currencyCode { get; set; }
        public string tpaLogo { get; set; }
        public string reportName { get; set; }
        public string BordxReportTemplateName { get; set; }
        public string BordxStartDate { get; set; }
        public string BordxEndDate { get; set; }
    }

    public class InvoiceCodeColumnHeaders
    {
        public virtual Guid Id { get; set; }
        public virtual string HeaderName { get; set; }
        public virtual int Sequance { get; set; }
    }

    //  public class BordxReportColumns
    //  {
    //      public virtual Guid Id { get; set; }
    //      public virtual string DisplayName { get; set; }
    //      public virtual Guid HeaderId { get; set; }
    //      public virtual string KeyName { get; set; }
    //      public virtual bool IsActive { get; set; }
    //      public virtual int Sequance { get; set; }


    //  }
}
