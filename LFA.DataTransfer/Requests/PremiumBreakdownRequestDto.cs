using System;
using System.Collections.Generic;

namespace TAS.DataTransfer.Requests
{
    public class PremiumBreakdownRequestDto
    {

        public PremiumBreakdownRequest premiumBreakdownRequest { get; set; }

    }
    public class PremiumBreakdownRequest
    {
        public List<ProductContract_> productContracts { get; set; }
        public DateTime ItemPurchasedDate { get; set; }
        public decimal Usage { get; set; }
        public Guid commodityTypeId { get; set; }
        public decimal DealerPrice { get; set; }
        public DateTime PolicySoldDate { get; set; }
     
        public Guid variantId { get; set; }
    }
}
