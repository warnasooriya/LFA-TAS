using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    [Serializable]
     public class ClaimBatchTableResponseDto
    {
        public Guid Id { get; set; }
        public string CountryId { get; set; }
        public string DealerId { get; set; }
        public string InsurerId { get; set; }
        public string ReinsurerId { get; set; }
        public string BatchNumber { get; set; }
        public DateTime EntryDate { get; set; }
        public Guid EntryBy { get; set; }

        public bool IsClaimBatchingExists { get; set; }
    }
}
