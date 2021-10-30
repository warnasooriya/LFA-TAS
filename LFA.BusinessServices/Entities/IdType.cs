using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.Entities
{
    public class IdType
    {
        public virtual int Id { get; set; }
        public virtual string IdTypeName { get; set; }
        public virtual string IdTypeDescription { get; set; }
    }
}
