using System;

namespace TAS.DataTransfer.Requests
{
    public class ClaimListRequestDto
    {
        public int page { get; set; }
        public int pageSize { get; set; }
        public Guid loggedInUserId { get; set; }
        public string userType { get; set; }
        public ClaimListSearchDto claimSearch { get; set; }

    }

    public class ClaimListSearchDto
    {
        public Guid claimDealerId { get; set; }
        public Guid commodityTypeId { get; set; }
        public Guid makeId { get; set; }
        public Guid statusId { get; set; }
        public string claimNo { get; set; }
        public string policyNo { get; set; }

        public string vinNo { get; set; }
    }
}
