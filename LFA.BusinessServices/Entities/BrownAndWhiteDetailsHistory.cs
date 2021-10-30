using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.Entities
{
    public class BrownAndWhiteDetailsHistory
    {
        public virtual Guid Id { get; set; }
        public virtual Guid BrownAndWhiteDetailsId { get; set; }

        public virtual DateTime ItemPurchasedDate { get; set; }
        public virtual Guid MakeId { get; set; }
        public virtual Guid ModelId { get; set; }
        public virtual string SerialNo { get; set; }
        public virtual decimal ItemPrice { get; set; }
        public virtual Guid CategoryId { get; set; }
        public virtual string ModelYear { get; set; }
        public virtual string AddnSerialNo { get; set; }
        public virtual Guid ItemStatusId { get; set; }
        public virtual string InvoiceNo { get; set; }
        public virtual string ModelCode { get; set; }
        public virtual decimal DealerPrice { get; set; }
        public virtual DateTime EntryDateTime { get; set; }
        public virtual Guid EntryUser { get; set; }
        public virtual Guid CommodityUsageTypeId { get; set; }
        public virtual Guid PolicyTransactionId { get; set; }
    }
    
}
