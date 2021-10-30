using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    public class NotesResponseDto
    {
        public List<NoteResponseDto> Notes { get; set; }
    }

    public class NoteResponseDto
    {
        public Guid Id { get; set; }
        public string PolicyNo { get; set; }
        public string ClaimNo { get; set; }
        public string SubmittedUser { get; set; }
        public string Note { get; set; }
        public string EntryDateTime { get; set; }


    }
}
