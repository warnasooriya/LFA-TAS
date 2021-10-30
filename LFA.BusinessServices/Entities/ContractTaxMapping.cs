using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.Entities
{
    public class ContractTaxMapping
    {
        public virtual Guid Id { get; set; }
        public virtual Guid ContractId { get; set; }
        public virtual Guid CountryTaxId { get; set; }
    }
}
