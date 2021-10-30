using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    public class ExcelDownloadReportResponceUnitOfWork
    {

        public String Code { get; set; }

        public int CodeInt { get; set; }
        public String DealerName { get; set; }
        public String InvoiceCode { get; set; }

        public String PlateNumber { get; set; }

        public String TireQuantity { get; set; }

        public DateTime PurcheasedDate { get; set; }

        public String GeneratedDate { get; set; }

        public int Diameter { get; set; }

        public String Width { get; set; }



    }

   
}
