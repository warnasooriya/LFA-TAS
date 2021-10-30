using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    public class ClaimDetailsForClaimViewResponseDto
    {
        public List<AssessmentData> AssessmentData { get; set; }
        public List<ClaimData> ClaimData { get; set; }

    }

    public class AssessmentData
    {
        public string claimNumber { get; set; }
        public DateTime claimDate { get; set; }
        public string s_claimDate { get; set; }
        public string milage { get; set; }
        public string customerComplaint { get; set; }
        public string dealerComment { get; set; }
        public string engineerAssesment { get; set; }
        public string conclution { get; set; }
        public string status { get; set; }
        public DateTime failureDate { get; set; }
        public DateTime approvedDate { get; set; }
        public string fault { get; set; }
        public string perClaimLiability { get; set; }
        public string totalLiability { get; set; }
    }

    public class ClaimData
    {
        public string claimNo { get; set; }
        public DateTime date { get; set; }
        public string s_date { get; set; }

        public string milage { get; set; }
        public string requestedAmount { get; set; }
        public string authorizedAmount { get; set; }
        public string status { get; set; }
        public string s_status { get; set; }

        public string comments { get; set; }
        public Guid claimId { get; set; }
        public string failureDate { get; set; }
        public string approvedDate { get; set; }
        public string fault { get; set; }
        public string perClaimLiability { get; set; }
        public string totalLiability { get; set; }



    }
}
