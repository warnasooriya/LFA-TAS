using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.Entities
{
    public class UserType
    {
        public virtual Guid Id { get; set; }
        public virtual string Code { get; set; }
        public virtual string Description { get; set; }
    }
}
