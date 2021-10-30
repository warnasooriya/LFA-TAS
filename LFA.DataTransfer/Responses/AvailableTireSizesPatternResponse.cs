using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    public class AvailableTireSizesPatternResponse
    {
        public  Guid Id { get; set; }
        public  Guid AvailableTireSizesId { get; set; }
        public  string Pattern { get; set; }
    }
}
