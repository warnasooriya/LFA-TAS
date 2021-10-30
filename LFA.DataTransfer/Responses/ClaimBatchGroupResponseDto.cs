using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    public class ClaimBatchGroupResponseDto
    {
        public Guid Id { get; set; }
        public Guid ClaimBatchId { get; set; }
        public string GroupName { get; set; }
        public bool IsAllocatedForCheque { get; set; }
        public DateTime EntryDate { get; set; }
        public Guid EntryBy { get; set; }
        public bool IsClaimBatchGroupExists { get; set; }
    }
}
