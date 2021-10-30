using System;

namespace TAS.DataTransfer.Requests
{
    public class ClaimBatchNumberRequestDto
    {
        public Guid reinsurerId { get; set; }
        public Guid insurerId { get; set; }
        public Guid dealerId { get; set; }
        public Guid countryId { get; set; }
    }
}
