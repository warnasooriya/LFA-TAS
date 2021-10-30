using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class ProductRequestDto
    {


        public Guid Id { get; set; }
        public Guid CommodityTypeId { get; set; }
        public string Productname { get; set; }
        public string Productcode { get; set; }
        public string ProductDisplayCode { get; set; }
        public string Productdescription { get; set; }
        public string Productshortdescription { get; set; }
        public Guid Displayimage { get; set; }
        public Guid ProductTypeId { get; set; }
        public bool Isbundledproduct { get; set; }
        public bool Isactive { get; set; }
        public bool Ismandatoryproduct { get; set; }
        public DateTime Entrydatetime { get; set; }
        public Guid Entryuser { get; set; }
        public DateTime? Lastupdatedatetime { get; set; }
        public Nullable<Guid> Lastupdateuser { get; set; }
        //public Guid selectedpp { get; set; }
        public string[] selectedpp { get; set; }


        public bool ProductInsertion
        {
            get;
            set;
        }

        //public class Value
        //{

        //    public string value { get; set; }
        //    public List<string> Values { get; set; }
        //}

        public class Attribute
        {
            public string key { get; set; }
            public bool value { get; set; }
        }

    }
}
