using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.Entities
{
    public class ClaimBordxValueDetail
    {
        public virtual Guid Id { get; set; }
        public virtual Guid ClaimBordxId { get; set; }
        public virtual Guid CountryId { get; set; }
        public virtual decimal USDValue { get; set; }
        public virtual decimal Rate { get; set; }
        public virtual decimal Value { get; set; }
    }
}
