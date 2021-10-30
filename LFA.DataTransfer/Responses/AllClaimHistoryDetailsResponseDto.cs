using System.Collections.Generic;

namespace TAS.DataTransfer.Responses
{
    public class AllClaimHistoryDetailsResponseDto
    {
        public ClaimCommentsResponseDto claimComments { get; set; }
        public NotesResponseDto claimNotes { get; set; }
        public object assessmentAndClaimHistory { get; set; }

    }

    public class ClaimNote
    {
        public string note { get; set; }
        public string dateAdded { get; set; }
        public string policyNumber { get; set; }
        public string claimNumber { get; set; }
        public string userName { get; set; }
    }

    public class ClaimComment
    {
        public string messege { get; set; }
        public string sentBy { get; set; }
        public string sentTo { get; set; }
        public string sentTime { get; set; }
        public bool seen { get; set; }
        public string seenTime { get; set; }
        public bool self { get; set; }

    }
}
