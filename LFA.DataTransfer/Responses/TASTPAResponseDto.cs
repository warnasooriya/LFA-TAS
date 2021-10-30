using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    public class TASTPAResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string TelNumber { get; set; }
        public string Address { get; set; }
        public Guid Banner { get; set; }
        public Guid Logo { get; set; }
        public string DiscountDescription { get; set; }
        
    }
}
