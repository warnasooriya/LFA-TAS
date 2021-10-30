using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    public class CustomerEnterdInvoiceTireDetailsResponsesDto
    {
        public  Guid Id { get; set; }
        public  Guid CustomerEnterdInvoiceId { get; set; }
        public  Guid InvoiceCodeTireDetailId { get; set; }
        public  string TirePositionCode { get; set; }
        public  decimal PurchasedPrice { get; set; }
        public  Guid CurrencyId { get; set; }
        public  Guid CurrencyPeriodId { get; set; }
        public  decimal ConversionRate { get; set; }
    }
}
