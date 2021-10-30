using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.Entities
{
    public class BordxReportColumns
    {
        public virtual Guid Id { get; set; }
        public virtual string DisplayName { get; set; }
        public virtual Guid HeaderId { get; set; }
        public virtual string KeyName { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual int Sequance { get; set; }
        public virtual string ColumnWidth { get; set; }
        public virtual string Alignment { get; set; }
        public virtual Guid ProductType { get; set; }
    }
}
