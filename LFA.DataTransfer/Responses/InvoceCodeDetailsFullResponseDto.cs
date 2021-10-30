using System;

namespace TAS.DataTransfer.Responses
{
    public class InvoceCodeDetailsFullResponseDto : InvoceCodeDetailsResponseDto
    {
        public Guid MakeId { get; set; }
        public Guid  ModelId { get; set; }
        public string CurrencyCode { get; set; }

    }
}
