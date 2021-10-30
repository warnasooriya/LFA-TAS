using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    [Serializable]
    public class InsuaranceLimitationResponseDto
    {
        public  Guid Id { get; set; }
        public  Guid CommodityTypeId { get; set; }
        public  string CommodityTypeCode { get; set; }
        public  string InsuaranceLimitationName { get; set; }
        public  decimal Km { get; set; }
        public  int Months { get; set; }
        public  decimal Hrs { get; set; }


        public  bool TopOfMW { get; set; }
        public  bool IsRsa { get; set; }
        public  DateTime EntryDateTime { get; set; }
        public  Guid EntryBy { get; set; }

        public bool IsInsuaranceLimitationyExists { get; set; }
    }
}
