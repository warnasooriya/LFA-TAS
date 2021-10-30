using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.Entities
{
    public class InvoiceCode
    {
        public virtual Guid Id { get; set; }
        public virtual Guid DealerId { get; set; }
        public virtual Guid DealerLocation { get; set; }
        public virtual Guid CountryId { get; set; }
        public virtual Guid CityId { get; set; }
        public virtual string Code { get; set; }
       // public virtual int CodeInt { get; set; }
        public virtual DateTime PurcheasedDate { get; set; }
        public virtual string PlateNumber { get; set; }
        public virtual int TireQuantity { get; set; }
        public virtual DateTime GeneratedDate { get; set; }
        public virtual Guid GeneratedBy { get; set; }
        public virtual Nullable<bool> AllTyresAreSame { get; set; }
        public virtual Guid PlateRelatedCityId { get; set; }

    }
}
