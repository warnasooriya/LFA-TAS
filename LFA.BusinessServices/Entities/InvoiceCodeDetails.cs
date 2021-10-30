using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.Entities
{
    public class InvoiceCodeDetails
    {
        public virtual Guid Id { get; set; }
        public virtual Guid InvoiceCodeId { get; set; }
        public virtual Guid MakeId { get; set; }
        public virtual Guid ModelId { get; set; }
        public virtual Guid VariantId { get; set; }
        public virtual string Position { get; set; }
        public virtual int TireQuantity { get; set; }

        public virtual bool IsPolicyCreated { get; set; }
        public virtual Guid? PolicyId { get; set; }
        public virtual bool IsPolicyApproved { get; set; }
        public virtual DateTime PolicyCreatedDate { get; set; }
        public virtual decimal Price { get; set; }
    }
}
