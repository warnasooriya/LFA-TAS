using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.Entities
{
    public class Broker
    {
        public virtual Guid Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string Code { get; set; }
        public virtual Guid CountryId { get; set; }
        public virtual string BrokerStatus { get; set; }
        public virtual string TelNumber { get; set; }
        public virtual string Address { get; set; }
    }
}
