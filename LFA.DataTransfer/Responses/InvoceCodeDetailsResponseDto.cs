using System;

namespace TAS.DataTransfer.Responses
{
    public class InvoceCodeDetailsResponseDto
    {
        public string PlateNumber { get; set; }
        public int Quantity { get; set; }

        public TireDtls TireFront { get; set; }
        public TireDtls TireBack { get; set; }
        public string Position { get; set; }

    }

    public class TireDtls {
        public string Wide { get; set; }
        public string Cross { get; set; }
        public int Diameter { get; set; }
        public string LoadSpeed { get; set; }

        public string SerialLeft { get; set; }
        public string SerialRight { get; set; }
        public string PatternRight { get; set; }
        public string PatternLeft { get; set; }

        public Guid IdLeft { get; set; }
        public Guid IdRight { get; set; }
    }
}
