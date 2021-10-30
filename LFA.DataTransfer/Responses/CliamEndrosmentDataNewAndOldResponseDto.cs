using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAS.DataTransfer.Requests;

namespace TAS.DataTransfer.Responses
{
    public class CliamEndrosmentDataNewAndOldResponseDto
    {
        public string Status { get; set; }
        public OldCliamEndrosment oldCliamEndrosment { get; set; }
        public NewCliamEndrosment newCliamEndrosment { get; set; }
        
        
    }

    public class OldCliamEndrosment
    {
        public Guid Id { get; set; }
        public string PolicyNumber { get; set; }
        public string ClaimNumber { get; set; }
        public string customer { get; set; }
        public string Serial { get; set; }
        public string MakeId { get; set; }
        public string ModelId { get; set; }
        public string PolicyDealerId { get; set; }
        public string ClaimDealer { get; set; }
        public decimal TotalClaimAmount { get; set; }
        public decimal TotalGrossAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public Complaint Complaint { get; set; }
        public List<ClaimItemList> ClaimItemList { get; set; }

    }

    public class NewCliamEndrosment
    {
        public Guid Id { get; set; }
        public string PolicyNumber { get; set; }
        public string ClaimNumber { get; set; }
        public string customer { get; set; }
        public string Serial { get; set; }
        public string MakeId { get; set; }
        public string ModelId { get; set; }
        public string PolicyDealerId { get; set; }
        public string ClaimDealer { get; set; }
        public decimal TotalClaimAmount { get; set; }
        public decimal TotalGrossAmount { get; set; }
        public decimal PaidAmount { get; set; }

        public Complaint Complaint { get; set; }
         public List<ClaimItemList> ClaimItemList { get; set; }

        //public decimal AuthorizedClaimAmount { get; set; }


        //public string RequestedUser { get; set; }
        //public string DealerName { get; set; }


        //public string CurrencyCode { get; set; }
        //public string Country { get; set; }
        //public string EntryDate { get; set; }
        //public string ApprovedDate { get; set; }
        //public string ApprovedBy { get; set; }

        //public AttachmentsResponseDto Attachments { get; set; }
        //public object ServiceHistory { get; set; }

        //public Guid CommodityCategoryId { get; set; }



        //public Guid DealerId { get; set; }
        //public Guid PolicyId { get; set; }

        //public Guid PolicyCountryId { get; set; }
        //public Guid ParentId { get; set; }

        //public string InvoiceReceivedDate { get; set; }
        //public string InvoiceDate { get; set; }
        //public string InvoiceNumber { get; set; }
        //public string BatchNumber { get; set; }
        //public string BatchEntryDate { get; set; }
        //public string GroupName { get; set; }

        //public string Insurer { get; set; }
        //public string Reinsurer { get; set; }
        //public int BordxYear { get; set; }
        //public int Bordxmonth { get; set; }
        //public string BordxNumber { get; set; }
    }
   
}
