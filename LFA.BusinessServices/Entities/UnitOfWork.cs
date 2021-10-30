using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.Entities
{
    public class UnitOfWorks
    {
        public virtual Guid Id { get; set; }
        public virtual string UnitOfWorkName { get; set; }
        public virtual Guid PriviledgeLevelId { get; set; }
    }
}
