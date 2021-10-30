using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.Entities
{
    public class DealerDiscount
    {
        public virtual Guid Id { get; set; }
        public virtual string PartOrLabour { get; set; }
        public virtual Guid DealerId { get; set; }
        public virtual Guid CountryId { get; set; }
        public virtual Guid DiscuntSchemeId { get; set; }
        public virtual Guid? MakeId { get; set; }
        public virtual DateTime StartDate { get; set; }
        public virtual DateTime EndDate { get; set; }
        public virtual decimal DiscountPercentage { get; set; }
        public virtual decimal GoodWillPercentage { get; set; }
        public virtual bool IsApplicable { get; set; }
        public virtual DateTime EntryDate { get; set; }
        public virtual Guid EntryBy { get; set; }

    }

}
