using System;

namespace TAS.DataTransfer.Responses
{
    public class AddClaimItemResponseDto
    {
        public string Status { get; set; }
        public string ClaimNo { get; set; }
        public decimal RequestedAmount { get; set; }
        public decimal AuthorizedAmount { get; set; }
        public Guid ClaimId { get; set; }
        public Guid ServerId { get; set; }
        public bool IsReload { get; set; }



    }
}