using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace TAS.DataTransfer.Responses
{
     public class TyrePolicySaveResponse
    {
        public string code { get; set; }
        public string msg { get; set; }
        public Guid tempInvId { get; set; }
        //public List<CustomerEnterdInvoiceTireDetailsResponsesDto> customerEnterdInvoiceTireDetails { get; set; }
        //public CustomerEnterdInvoiceDetailsResponses customerEnterdInvoiceDetails { get; set; }
    }


    public class CustomerEnterdInvoiceDetailsResponses
    {
        public  Guid Id { get; set; }
        public  Guid InvoiceCodeId { get; set; }
        public  Guid CustomerId { get; set; }
        public  string InvoiceCode { get; set; }
        public  string InvoiceNumber { get; set; }
        public  Guid InvoiceAttachmentId { get; set; }
        public  string UsageTypeCode { get; set; }
        public  Guid AdditionalDetailsMakeId { get; set; }
        public  Guid AdditionalDetailsModelId { get; set; }
        public  int AdditionalDetailsModelYear { get; set; }
        public  decimal AdditionalDetailsMileage { get; set; }
        public  DateTime GeneratedDateTime { get; set; }

    }

}
