using System;

namespace TAS.DataTransfer.Requests
{
    public class DealerInvoicesGenerateRequestDto
    {
        public Guid dealerId { get; set; }
        public int year { get; set; }
        public int month { get; set; }

    }
}
