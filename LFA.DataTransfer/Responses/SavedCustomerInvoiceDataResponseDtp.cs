using System;
using System.Collections.Generic;
using TAS.DataTransfer.Requests;

namespace TAS.DataTransfer.Responses
{
    public class SavedCustomerInvoiceDataResponseDto
    {
        public string InvoiceCode { get; set; }
        public string InvoiceNumber { get; set; }
        public Guid InvoiceAttachmentId { get; set; }
        public string UsageTypeCode { get; set; }

        public decimal AdditionalMileage { get; set; }

        public Guid AdditionalMakeId { get; set; }
        public Guid AdditionalModelId { get; set; }
        public int AdditionalModelYearId { get; set; }

        public List<AvailableTireList> availableTireList { get; set; }
    }
}
