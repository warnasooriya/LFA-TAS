using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.Entities
{
    public class InvoiceCodeTireDetails
    {
        public virtual Guid Id { get; set; }
        public virtual Guid InvoiceCodeDetailId { get; set; }
        public virtual string Position { get; set; }
        public virtual string ArticleNumber { get; set; }
        public virtual string SerialNumber { get; set; }
        public virtual string Width { get; set; }
        public virtual string CrossSection { get; set; }
        public virtual int Diameter { get; set; }
        public virtual string LoadSpeed { get; set; }
        public virtual Guid AvailableTireSizesPatternId { get; set; }
        public virtual string DotNumber { get; set; }
        public virtual decimal Price { get; set; }

    }
}
