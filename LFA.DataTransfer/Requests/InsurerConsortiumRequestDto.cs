using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class InsurerConsortiumRequestDto
    {
        public Guid Id { get; set; }
        public Guid Id2 { get; set; }
        public Guid ParentInsurerId { get; set; }
        public Guid InsurerId { get; set; }
        public Decimal NRPPercentage { get; set; }
        public Decimal RiskSharePercentage { get; set; }
        public Decimal ProfitSharePercentage { get; set; }

        public bool InsurerConsortiumInsertion
        {
            get;
            set;
        }

    }
}
