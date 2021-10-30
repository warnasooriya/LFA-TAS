using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    public class DashboardChartMappingResponseDto
    {
        public Guid DashboardChartId { get; set; }
        public Guid UserRoleId { get; set; }
        public string Section { get; set; }
        public string ChartDisplayName { get; set; }

        public bool IsAllowed { get; set; }
        public bool IsAllBranches { get; set; }
       
    }
}
