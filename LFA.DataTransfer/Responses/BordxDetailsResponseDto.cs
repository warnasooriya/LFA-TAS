using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    public class BordxDetailsResponseDto
    {
        public Guid Id { get; set; }
        public Guid PolicyId { get; set; }
        public Guid BordxId { get; set; }

        public bool IsBordxDetailsExists { get; set; }

    }
}
