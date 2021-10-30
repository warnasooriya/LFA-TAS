using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    public class ClaimEnteredItemDetailsResponseDto
    {

        public decimal LabourAmount { get; set; }
        public decimal SundryAmount { get; set; }
        public decimal PartAmount { get; set; }
        public decimal ClaimApprovedAmount { get; set; }
        public decimal InvoiceAmount { get; set; }
        public Guid ClaimId { get; set; }
        public string error { get; set; }
        public string ClaimNo { get; set; }
    }
}
