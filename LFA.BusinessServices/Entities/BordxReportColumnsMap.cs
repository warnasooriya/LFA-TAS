using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.Entities
{
    public class BordxReportColumnsMap
    {
        public virtual Guid Id { get; set; }
        public virtual Guid UserId { get; set; }
        public virtual Guid ColumnId { get; set; }
        public virtual bool IsActive { get; set; }
    }
}
