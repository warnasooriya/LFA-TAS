using System;
using System.Collections.Generic;

namespace TAS.Services.Entities
{
    public class Product
    {
        //public Product() { }
        public virtual Guid Id { get; set; }
        public virtual Guid CommodityTypeId { get; set; }
        public virtual string Productname { get; set; }
        public virtual string Productcode { get; set; }
        public virtual string ProductDisplayCode { get; set; }
        public virtual string Productdescription { get; set; }
        public virtual string Productshortdescription { get; set; }
        public virtual Guid Displayimage { get; set; }
        public virtual Guid ProductTypeId { get; set; }
        public virtual bool Isbundledproduct { get; set; }
        public virtual bool Isactive { get; set; }
        public virtual bool Ismandatoryproduct { get; set; }
        public virtual DateTime Entrydatetime { get; set; }
        public virtual Guid Entryuser { get; set; }
        public virtual DateTime? Lastupdatedatetime { get; set; }
        public virtual Nullable<Guid> Lastupdateuser { get; set; }
        //public virtual List<BundledProduct> BundledProducts { get; set; }

    }
}