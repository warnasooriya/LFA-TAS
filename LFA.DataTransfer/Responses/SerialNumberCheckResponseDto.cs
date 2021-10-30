using System;

namespace TAS.DataTransfer.Responses
{
    public class SerialNumberCheckResponseDto
    {
        public bool IsDealerInvalid { get; set; }
        public bool IsSerialExist { get; set; }
        public Guid ItemId { get; set; }
        public bool IsBordxConfirmed { get; set; }
        public bool IsPolicyApproved { get; set; }
        public bool AllowedToApprove { get; set; }

    }
}
