using System;
using System.Collections.Generic;

namespace TAS.DataTransfer.Requests
{
    public class ClaimBatchGroupSaveRequestDto
    {
        public Data data { get; set; }
        public Guid requestedUserId { get; set; }

    }
    public class Data
    {
        public Guid ClaimBatchId { get; set; }
        public Guid GroupId { get; set; }
        public List<SelectedClaim> SelectedClaims { get; set; }
        public string GroupName { get; set; }
        public string Comment { get; set; }
        public decimal TotalAmount { get; set; }
        public bool IsGoodwill { get; set; }
    }
    public class SelectedClaim
    {
        public Guid Id { get; set; }
        public decimal TotalClaimAmount { get; set; }
        public string ClaimNumber { get; set; }
        public decimal Amount { get; set; }
        public string Comment { get; set; }
    }
}
