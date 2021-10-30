using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.Entities
{
    public class MasterTableMapping
    {
        public virtual Guid MasterTableMappingId { get; set; }
        public virtual string TableName { get; set; }
        public virtual string TableDescription { get; set; }
        public virtual string ColumnName { get; set; }
        public virtual string ColumnDisplayName { get; set; }

    }
}
