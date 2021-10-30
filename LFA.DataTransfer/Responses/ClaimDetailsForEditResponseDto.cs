using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    public class ClaimDetailsForEditResponseDto
    {
        public Guid ClaimId { get; set; }
        public string Status { get; set; }
        public PolicyDetails PolicyDetails { get; set; }
        public ClaimRequestDetailsResponseDto ClaimDetails { get; set; }
        //public ClaimSubmissionDetails ClaimSubmissionDetails { get; set; }
         
    }

    public class PolicyDetails
    {
        public Guid PolicyId { get; set; }
        public Guid CustomerId { get; set; }
        public Guid CommodityCategoryId { get; set; }
        public Guid MakeId { get; set; }
        public Guid ModelId { get; set; }

        public Guid CommodityTypeId { get; set; }
        public Guid DealerId { get; set; }
        public string CustomerComplaint { get; set; }
        public string DealerComment { get; set; }
        public decimal ClaimMileage { get; set; }
    }
    //public class ClaimSubmissionDetails
    //{
    //    public  Guid Id { get; set; }
    //    public  DateTime ClaimDate { get; set; }
    //    public  decimal ClaimMileage { get; set; }
    //    public  string CustomerName { get; set; }
    //    public  decimal LastServiceMileage { get; set; }
    //    public  DateTime LastServiceDate { get; set; }
    //}
}
