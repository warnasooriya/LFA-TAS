using System;

namespace TAS.DataTransfer.Requests
{
    public class LoadInvoceCodeByIdRequestDto
    {
        public Guid invoiceId { get; set; }
        public Guid dealerId { get; set; }
        public Guid dealerBranchId { get; set; }
        public Guid countryId { get; set; }
        public Guid cityId { get; set; }

    }
}
