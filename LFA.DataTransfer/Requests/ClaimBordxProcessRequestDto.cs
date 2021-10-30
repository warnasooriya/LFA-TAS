using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class ClaimBordxProcessRequestDto
    {
        public Guid Id { get; set; }
        public Guid ClaimBordxId { get; set; }
        public Guid InsurerId { get; set; }
        public Guid ReinsurerId { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public bool IsProcessed { get; set; }
        public bool IsConfirmed { get; set; }
        public Guid UserId { get; set; }
        public DateTime EntryDateTime { get; set; }
        public Guid ProcessedUserId { get; set; }
        public DateTime ProcessedDateTime { get; set; }
        public Guid ConfirmedUserId { get; set; }
        public DateTime ConfirmedDateTime { get; set; }
    }
}
