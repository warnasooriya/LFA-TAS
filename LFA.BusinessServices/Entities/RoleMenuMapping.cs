using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.Entities
{
    public class RoleMenuMapping
    {
        public virtual Guid Id { get; set; }
        public virtual Guid MenuId { get; set; }
        public virtual Guid RoleId { get; set; }
        public virtual Guid LevelId { get; set; }
        public virtual string Description { get; set; }
    }
}
