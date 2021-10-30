using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.Entities
{
    public class Reinsurer
    {
        public virtual Guid Id { get; set; }
        public virtual string ReinsurerName { get; set; }
        public virtual string ReinsurerCode { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual DateTime EntryDateTime { get; set; }
        public virtual Guid EntryUser { get; set; }
        public virtual Guid CurrencyId { get; set; }
    }
}
