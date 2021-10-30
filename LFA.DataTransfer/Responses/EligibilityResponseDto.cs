using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    public class EligibilityResponseDto
    {
        public Guid Id { get; set; }
        public Guid ContractId { get; set; }
        public int AgeFrom { get; set; }
        public int AgeTo { get; set; }
        public int MonthsFrom { get; set; }
        public int MonthsTo { get; set; }
        public int MileageFrom { get; set; }
        public int MileageTo { get; set; }
        public Decimal Premium { get; set; }
        public bool IsPercentage { get; set; }
        public string PlusMinus { get; set; }
        public DateTime EntryDateTime { get; set; }
        public Guid EntryUser { get; set; }

        public bool IsEligibilityExists { get; set; }

    }
}
