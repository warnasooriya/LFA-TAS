using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    [Serializable]
    public class TPAResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string TelNumber { get; set; }
        public string Address { get; set; }
        public Guid Banner { get; set; }
        public Guid Banner2 { get; set; }
        public Guid Banner3 { get; set; }
        public Guid Banner4 { get; set; }
        public Guid Banner5 { get; set; }
        public Guid Logo { get; set; }
        public string DiscountDescription { get; set; }
        public string OriginalTPAName { get; set; }
        public String tpaCode { get; set; }
        public string languageCode { get; set; }



    }
}
