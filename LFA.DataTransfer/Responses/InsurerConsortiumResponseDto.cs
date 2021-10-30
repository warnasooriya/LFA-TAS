using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    public class InsurerConsortiumResponseDto
    {
        public Guid Id { get; set; }
        public Guid ParentInsurerId { get; set; }
        public Guid InsurerId { get; set; }
        public Decimal NRPPercentage { get; set; }
        public Decimal RiskSharePercentage { get; set; }
        public Decimal ProfitSharePercentage { get; set; }

        public bool IsInsurerConsortiumExists { get; set; }

    }
}
