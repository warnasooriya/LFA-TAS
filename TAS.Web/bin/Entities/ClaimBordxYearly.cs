using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.Entities
{
    public class ClaimBordxYearly
    {
        public virtual Guid Id { get; set; }
        public virtual int Year { get; set; }
        public virtual Guid CountryId  { get; set; }
        public virtual Decimal BordxAmount { get; set; }
        public virtual bool IsConformed { get; set; }
        //public virtual Guid Reinsurer { get; set; }
        //public virtual Guid Insurer { get; set; }
    }
}
