using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    public class PieChartDataResponseDto
    {
        public decimal value { get; set; }
        public string color { get; set; }
        public string highlight { get; set; }
        public string label { get; set; }

    }
}
