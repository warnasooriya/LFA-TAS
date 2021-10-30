using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class ClaimInvoiceEntryRequestDto
    {
        public  Guid Id { get; set; }
        public  Guid DealerId { get; set; }
        public  DateTime InvoiceReceivedDate { get; set; }
        public  DateTime InvoiceDate { get; set; }
        public  string InvoiceNumber { get; set; }
        public  decimal InvoiceAmount { get; set; }
        public  Guid CurrencyId { get; set; }
        public  Guid CurrencyPeriodId { get; set; }
        public  decimal ConversionRate { get; set; }
        public  Guid UserAttachmentId { get; set; }
        public  DateTime EntryDateTime { get; set; }
        public  string EntryBy { get; set; }
        public bool IsConfirm { get; set; }


        //public  Guid Id { get; set; }
        public  Guid ClaimInvoiceEntryId { get; set; }
        public  Guid ClaimId { get; set; }

       // public List<ClaimInvoiceEntryClaimRequestDto> ClaimInvoiceEntryClaims { get; set; }

        public List<SelectedClaims> claims { get; set; }
        public bool ClaimInvoiceEntryInsertion
        {
            get;
            set;
        }
    }

    public class SelectedClaims
    {      

        public Guid Id { get; set; }
        public decimal TotalClaimAmount { get; set; }
        public string ClaimNumber { get; set; }
        public decimal Amount { get; set; }
       
    }
}
