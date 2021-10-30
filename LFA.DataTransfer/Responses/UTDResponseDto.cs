using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    public class UTDResponseDto
    {
        public Decimal OriginalTirePrice { get; set; }
        public Decimal CalculateUDT { get; set; }
        public Decimal UnsedTireDepth { get; set; }
        public Decimal OriginalTireDepth { get; set; }
        public Decimal PercentageValue { get; set; }
        public Decimal CustomerValue { get; set; }
        public Decimal OriginalPercentageValue { get; set; }
        public DateTime PolicyStartDate { get; set; }
        public DateTime FailureDate { get; set; }
        public int NoofDatetoFaliarDate { get; set; }
        public Decimal KMatPolicySale { get; set; }
        public Decimal PurchasedPrice { get; set; }
        public Decimal KMatFailureDate { get; set; }
        public Decimal DrivanKM { get; set; }
        public bool WithinMonth { get; set; }
    }

   
       
}
