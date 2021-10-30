using System;
using System.Collections.Generic;

namespace TAS.DataTransfer.Responses
{
    public class PremiumBreakdownResponseDto
    {
        public decimal Premium { get; set; }
        public List<BreakdownDetails> BreakdownDetails { get; set; }
        public bool isBasedonRP { get; set; }
    }

    public class BreakdownDetails
    {
        public String Name { get; set; }
        public List<BreakdownValue> Breakdowns { get; set; }
    }

    public class BreakdownValue
    {
        public String ItemName { get; set; }
        public decimal Value { get; set; }
        public bool isBasedonRP { get; set; }
        public bool isPercentageValue { get; set; }

    }
}
