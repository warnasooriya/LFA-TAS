using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class InvoiceCodeRequestDto
    {
        public  Guid Id { get; set; }
        //public  Guid DealerId { get; set; }
        //public  Guid DealerLocation { get; set; }
        //public  Guid CountryId { get; set; }
        //public  Guid CityId { get; set; }
        //public  string Code { get; set; }
        //// public virtual int CodeInt { get; set; }
        //public  DateTime PurcheasedDate { get; set; }
        //public  string PlateNumber { get; set; }
        //public  int TireQuantity { get; set; }
        //public  DateTime GeneratedDate { get; set; }
        //public  Guid GeneratedBy { get; set; }
        //public  bool IsPolicyCreated { get; set; }
        //public  Guid? PolicyId { get; set; }
        public  bool IsPolicyApproved { get; set; }
        public  DateTime PolicyCreatedDate { get; set; }

        public bool InvoiceCodeInsertion
        {
            get;
            set;
        }
    }
}
