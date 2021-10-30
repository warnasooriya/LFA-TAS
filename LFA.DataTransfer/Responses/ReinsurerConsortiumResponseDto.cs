using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    public class ReinsurerConsortiumResponseDto
    {
        public Guid Id { get; set; }
        public Guid ParentReinsurerId { get; set; }
        public Guid ReinsurerId { get; set; }
        public Decimal NRPPercentage { get; set; }
        public Decimal RiskSharePercentage { get; set; }
        public Decimal ProfitSharePercentage { get; set; }

        public bool IsReinsurerConsortiumExists { get; set; }

    }
}
