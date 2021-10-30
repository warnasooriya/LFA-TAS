using System;

namespace TAS.DataTransfer.Requests
{
    public class EligibilityCheckRequestDto
    {
        public EligibilityCheckRequest eligibilityCheckRequest { get; set; }
    }

    public class EligibilityCheckRequest
    {
        public Decimal usedAmount { get; set; }
        public Guid contractId { get; set; }
        public DateTime itemPurchesedDate { get; set; }
        public DateTime policySoldDate { get; set; }
}


}
