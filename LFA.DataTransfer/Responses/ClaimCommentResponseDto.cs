using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{

    public class ClaimCommentsResponseDto
    {
        public List<ClaimCommentResponseDto> Comments { get; set; }
    }

    public class ClaimCommentResponseDto
    {
        public  Guid Id { get; set; }
        public string PolicyNo { get; set; }
        public string ClaimNo { get; set; }
        public  string Comment { get; set; }
        public string SentFrom { get; set; }
        public  Guid SentTo { get; set; }
        public  bool ByTPA { get; set; }
        public  bool Seen { get; set; }
        public  string EntryDateTime { get; set; }
        public  DateTime SeenDateTime { get; set; }
    }
}
