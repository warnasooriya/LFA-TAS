using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class ClaimBordxValueDetailRequestDto
    {
        public Guid Id { get; set; }
        public Guid ClaimBordxId { get; set; }
        public Guid CountryId { get; set; }
        public decimal USDValue { get; set; }
        public decimal Rate { get; set; }
        public decimal Value { get; set; }
    }
}
