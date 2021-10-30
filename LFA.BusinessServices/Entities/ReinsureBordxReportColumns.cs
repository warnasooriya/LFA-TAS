using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.Entities
{
    public class ReinsureBordxReportColumns
    {
        public virtual Guid Id { get; set; }
        public virtual Guid ColumnId { get; set; }
        public virtual Guid ReinsureId { get; set; }
        public virtual bool IsAllowed { get; set; }
        public virtual Guid HeaderId { get; set; }
        
        
    }
}
