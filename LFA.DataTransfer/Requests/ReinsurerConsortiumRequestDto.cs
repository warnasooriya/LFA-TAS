using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class ReinsurerConsortiumRequestDto
    {
        public Guid Id { get; set; }

        public Guid Id2 { get; set; }
        public Guid ParentReinsurerId { get; set; }
        public Guid ReinsurerId { get; set; }
        public Decimal NRPPercentage { get; set; }
        public Decimal RiskSharePercentage { get; set; }
        public Decimal ProfitSharePercentage { get; set; }

        public bool ReinsurerConsortiumInsertion
        {
            get;
            set;
        }

    }
}
