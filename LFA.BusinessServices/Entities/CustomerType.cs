using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.Entities
{
    public class CustomerType
    {
        public virtual int Id { get; set; }
        public virtual string CustomerTypeName { get; set; }
        public virtual string CustomerTypeDescription { get; set; }
    }
}
