using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    public class TPAProductResponseDto
    {
        public Guid Id { get; set; }
        public Guid CommodityTypeId { get; set; }
        public string CommodityType { get; set; }
        public string CommodityTypeDisplayDescription { get; set; }
        public string Productname { get; set; }
        public string Productcode { get; set; }
        public Guid ProductTypeId { get; set; }
        public string Productdescription { get; set; }
        public string Productshortdescription { get; set; }
        public Guid Displayimage { get; set; }
        public bool Isbundledproduct { get; set; }
        public bool Isactive { get; set; }
        public bool Ismandatoryproduct { get; set; }
        public DateTime Entrydatetime { get; set; }
        public Guid Entryuser { get; set; }
        public DateTime? Lastupdatedatetime { get; set; }
        public Nullable<Guid> Lastupdateuser { get; set; }
        public string DisplayImageSrc { get; set; }
        public List<BundledProductResponseDto> BundledProducts { get; set; }

        
    }
}
