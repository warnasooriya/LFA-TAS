using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class ClaimBatchGroupRequestDto
    {
        public Guid Id { get; set; }
        public Guid ClaimBatchId { get; set; }
        public string GroupName { get; set; }
        public bool IsAllocatedForCheque { get; set; }
        public DateTime EntryDate { get; set; }
        public Guid EntryBy { get; set; }
        public Decimal TotalAmount { get; set; }
        public String Comment { get; set; }
        public  bool IsGoodwill { get; set; } 
        //public List<Guid> ClaimId { get; set; }

        public List<ClaimGroupClaimRequestDto> ClaimGroupClaims { get; set; }


        public bool ClaimBatchGroupEntryInsertion
        {
            get;
            set;
        }
    }
}
