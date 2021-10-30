using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class ClaimCommentRequestDto
    {
        public  Guid Id { get; set; }
        public  Guid PolicyId { get; set; }
        public  Guid ClaimId { get; set; }
        public  string Comment { get; set; }
        public  Guid SentFrom { get; set; }
        public  Guid SentTo { get; set; }
        public  bool ByTPA { get; set; }
        public  bool Seen { get; set; }
        public  DateTime EntryDateTime { get; set; }
        public  DateTime SeenDateTime { get; set; }
    }
}
