using System;
using System.Collections.Generic;

namespace TAS.DataTransfer.Requests
{
    public class AddDealerCommentRequestDto
    {
        public Guid id { get; set; }
        public string CommentCode { get; set; }
        public string Comment { get; set; }
        public bool isrejectiontype { get; set; }
        
    }

}
