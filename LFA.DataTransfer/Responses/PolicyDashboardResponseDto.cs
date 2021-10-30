using System.Collections.Generic;

namespace TAS.DataTransfer.Responses
{
    public class PolicyDashboardResponseDto
    {
        public string status { get; set; }
        public List<ChartData> data { get; set; }
    }

    public class ChartData
    {
        public string chartCode { get; set; }
        public string status { get; set; }
        public object data { get; set; }
    }
}