using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.Entities
{
    public class PriviledgeLevel
    {
        public virtual Guid Id { get; set; }
        public virtual string LevelName { get; set; }
        public virtual string LevelDescription { get; set; }
    }
}
