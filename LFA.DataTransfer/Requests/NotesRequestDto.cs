using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class NotesRequestDto
    {
        public  Guid Id { get; set; }
        public  Guid PolicyId { get; set; }
        public  Guid ClaimId { get; set; }
        public  Guid SubmittedUserId { get; set; }
        public  string Note { get; set; }
        public  DateTime EntryDateTime { get; set; }
    }
}
