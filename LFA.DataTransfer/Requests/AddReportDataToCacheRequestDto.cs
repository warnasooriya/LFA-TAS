using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class AddReportDataToCacheRequestDto
    {
        public string ReportCode { get; set; }
        public string ReportQuery { get; set; }
        public string ReportDBConnectionString { get; set; }
        public string ReportDirectory { get; set; }
    }
}
