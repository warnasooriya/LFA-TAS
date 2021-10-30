using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
     public class AllAvailableCrossSizeResponseDto
    {
        public List<int> DiameterList { get; set; }
    }

    public class AllAvailablePatternSizeResponseDto
    {
        public List<string> PatternList { get; set; }
    }
}
