using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    public class InvoiceCodeDetailsResponseDto
    {
        public  Guid Id { get; set; }
        public  Guid InvoiceCodeId { get; set; }
        public  Guid MakeId { get; set; }
        public  Guid ModelId { get; set; }
        public  Guid VariantId { get; set; }
        public  string Position { get; set; }
        public  int TireQuantity { get; set; }

        public  bool IsPolicyCreated { get; set; }
        public  Guid? PolicyId { get; set; }
        public  bool IsPolicyApproved { get; set; }
        public  DateTime PolicyCreatedDate { get; set; }
    }
}
