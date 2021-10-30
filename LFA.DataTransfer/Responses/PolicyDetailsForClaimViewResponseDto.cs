using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    public class PolicyDetailsForClaimViewResponseDto
    {
        public string policyNo { get; set; }
        public string warrentyType { get; set; }
        public string customerName { get; set; }
        public string mobileNumber { get; set; }
        public string insurer { get; set; }
        public string reInsurer { get; set; }
        public string make { get; set; }
        public string model { get; set; }
        public string modelYear { get; set; }
        public string cyllinderCount { get; set; }
        public string engineCapacity { get; set; }
        public string salePrice { get; set; }
        public string status { get; set; }
        public string uwYear { get; set; }
        public string vin { get; set; }


        public DateTime manfWarrentyStartDate { get; set; }
        public DateTime manfWarrentyEndDate { get; set; }
        public DateTime extensionStartDate { get; set; }
        public DateTime extensionEndDate { get; set; }
        public string s_manfWarrentyStartDate { get; set; }
        public string s_manfWarrentyEndDate { get; set; }
        public string s_extensionStartDate { get; set; }
        public string s_extensionEndDate { get; set; }
        public string manfWarrentyMonths { get; set; }
        public string extensionPeriod { get; set; }
        public string extensionMilage { get; set; }
        public string manfWarrentyMilage { get; set; }
        public string cutoff { get; set; }

        public decimal sumOfAuthorizedClaimedAmount { get; set; }
}
}
