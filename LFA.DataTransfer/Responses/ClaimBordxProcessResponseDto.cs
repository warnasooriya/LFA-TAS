using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    public class ClaimBordxProcessResponseDto
    {
        public Guid Id { get; set; }
        public int BordxYear { get; set; }
        public int Bordxmonth { get; set; }
        public string BordxNumber { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public decimal Amount { get; set; }
        public bool IsConfirmed { get; set; }

    }
}
