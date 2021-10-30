using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.Entities
{
    public class DashboardChart
    {
        public virtual Guid Id { get; set; }
        public virtual string Section { get; set; }
        public virtual string ChartDisplayName { get; set; }
        public virtual string ChartCode { get; set; }        
    }
}
