using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    public class PolicyforCustomerResponseDto : PolicyDetailsCustomer
    {
        public List<PolicyDetailsCustomer> PolicyDetails { get; set; }
    }

    public class PolicyDetailsCustomer
    {
        public Guid Id { get; set; }
        public string PolicyNo { get; set; }
        public string ProductName { get; set; }
        public Guid ProductId { get; set; }
        public DateTime PolicyStartDate { get; set; }
        public DateTime PolicyEndDate { get; set; }
        public int ExtMonths { get; set; }
         public decimal ExtKM { get; set; }
         public DateTime ExtStartDate { get; set; }
         public DateTime ExtEndDate { get; set; }

    }
}
