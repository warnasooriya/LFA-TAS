using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    public class ClaimBordxDetailResponseDto
    {
        public Guid Id { get; set; }
        public Guid ClaimBordxId { get; set; }
        public Guid ClaimId { get; set; }
        public Guid UserId { get; set; }
        public DateTime EntryDateTime { get; set; }
        public bool IsApproved { get; set; }
        public bool IsBatching { get; set; }
    }
}
