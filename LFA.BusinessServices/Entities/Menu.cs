using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.Entities
{
    public class Menu
    {
        public virtual Guid Id { get; set; }
        public virtual string MenuName { get; set; }
        public virtual string MenuCode { get; set; }
        public virtual string LinkURL { get; set; }
        public virtual Guid ParentMenuId { get; set; }
        public virtual string Icon { get; set; }
        public virtual int OrderVal { get; set; }
    }
}
