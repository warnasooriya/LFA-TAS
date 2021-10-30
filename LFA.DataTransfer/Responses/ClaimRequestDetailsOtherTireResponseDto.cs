using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAS.DataTransfer.Requests;

namespace TAS.DataTransfer.Responses
{
    public class ClaimRequestDetailsOtherTireResponseDto
    {
        public List<RequestDetailsOtherTire> requestDetailsOtherTires { get; set; }
    }

    public class RequestDetailsOtherTire
    {
        public Guid InvoiceCodeId { get; set; }
        public Guid InvoiceCodeDetailsId { get; set; }
        public Guid InvoiceCodeTireDetailsId { get; set; }
        public decimal UnUsedTireDepth { get; set; }
        public string Position { get; set; }
        public string RemarkPosition { get; set; }
        public string SerialNumber { get; set; }
        public Guid claimSubmissionItemId { get; set; }
        public Guid claimSubmissionId { get; set; }
        

    }
}
