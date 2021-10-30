using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.Entities
{
    public class ClaimBordxPayment
    {
        public virtual Guid Id { get; set; }
        public virtual DateTime ReceiptDate { get; set; }
        public virtual decimal BordxAmount { get; set; }
        public virtual string RefNo { get; set; }
        public virtual decimal BalanceAmount { get; set; }
        public virtual decimal PaidAmount { get; set; }
        public virtual Guid ClaimBordxID { get; set; }
    }
}
