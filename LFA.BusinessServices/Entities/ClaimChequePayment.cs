using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.Entities
{
    public class ClaimChequePayment
    {
        public virtual Guid Id { get; set; }
        public virtual string ChequeNo { get; set; }
        public virtual DateTime ChequeDate { get; set; }
        public virtual decimal ChequeAmount { get; set; }
        public virtual DateTime EntryDate { get; set; }
        public virtual Guid EntryBy { get; set; }
        public virtual Guid CountryId { get; set; }
        public virtual Guid DealerId { get; set; }
        public virtual DateTime IssuedDate { get; set; }
        public virtual string Comment { get; set; }
    }
}
