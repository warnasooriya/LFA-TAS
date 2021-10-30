using System;

namespace TAS.DataTransfer.Requests
{
    public class FaultSearchRequestDto
    {
        public int page { get; set; }
        public int pageSize { get; set; }
        public string sort { get; set; }
        public SearchFaultDetails searchDetails { get; set; }
    }

    public class SearchFaultDetails
    {
        public Guid Id { get; set; }
        public Guid claimId { get; set; }
        public Guid policyId { get; set; }
        public Guid categoryCodeId { get; set; }
        public Guid faultAreaId { get; set; }
        public string faultCategoryCode { get; set; }
        public string faultAreaCode { get; set; }
        public string faultCode { get; set; }
        public string faultName { get; set; }
        public string faultDescription { get; set; }
    }

  
}
