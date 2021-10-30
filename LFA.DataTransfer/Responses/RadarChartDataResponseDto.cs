using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    public class RadarChartDataResponseDto
    {
        public string[] labels { get; set; }
        public List<RadarChartData> datasets { get; set; }
    }

    public class RadarChartData
    {
        public string label { get; set; }
        public string fillColor { get; set; }
        public string strokeColor { get; set; }
        public string pointColor { get; set; }
        public string pointStrokeColor { get; set; }
        public string pointHighlightFill { get; set; }
        public string pointHighlightStroke { get; set; }
        public int[] data { get; set; }
    }
}
