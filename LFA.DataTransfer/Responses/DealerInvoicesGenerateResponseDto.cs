using System;
using System.Collections.Generic;
using System.Data;

namespace TAS.DataTransfer.Responses
{
    public class DealerInvoicesGenerateResponseDto
    {
        public DataTable  DealerInvoiceData { get; set; }
        public String DealerName { get; set; }
        public String DealerSalesPersonName { get; set; }

        public String DealerCurrencyName { get; set; }
        public List<DealerInvoiceReportColumnsResponseDto> DealerInvoiceReportColumns { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public string TpaLogo { get; set; }

    }
}
