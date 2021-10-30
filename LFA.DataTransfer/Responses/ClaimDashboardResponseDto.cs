using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    public class ClaimDashboardResponseDto
    {
        public string status { get; set; }
        public List<ClaimDashboardData> data { get; set; }
    }

    public class ClaimDashboardData
    {
        public string label { get; set; }
        //public string status { get; set; }
        public object value { get; set; }

        public string highlight { get; set; }
        public string color { get; set; }
    }
}
