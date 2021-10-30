using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    public class PaymentModesResponseDto
    {
        public List<PaymetModeResponseDto> PaymetModes { get; set; }
    }

    public class PaymentTypesResponseDto
    {
        public List<PaymetTypeResponseDto> PaymetTypes { get; set; }
    }

    [Serializable]
    public class PaymetModeResponseDto
    {
        public Guid Id { get; set; }
        public String PaymentMode { get; set; }
        public String Code { get; set; }
    }

    public class PaymetTypeResponseDto
    {
        public Guid Id { get; set; }
        public String PaymentType { get; set; }
        public decimal PaymentCharge { get; set; }
    }
}
