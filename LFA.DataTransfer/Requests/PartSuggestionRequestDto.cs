using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class PartSuggestionRequestDto
    {
        public PartSuggestionReq partSuggestion { get; set; }
        public List<RelatedPartReq> relatedParts { get; set; }
    }


    public class PartSuggestionReq
    {
        public Guid PartId { get; set; }
    }

    public class RelatedPartReq
    {
        public Guid Id { get; set; }
        public bool isSelected { get; set; }
        public int quantity { get; set; }
    }
}
