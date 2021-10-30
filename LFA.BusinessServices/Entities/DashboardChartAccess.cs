using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.Entities
{
    public class DashboardChartAccess
    {
        public virtual Guid Id { get; set; }
        public virtual Guid DashboardChartId { get; set; }
        public virtual Guid UserRoleId { get; set; }
        public virtual bool IsAllowed { get; set; }
        public virtual bool IsAllBranches { get; set; }
    }
}
