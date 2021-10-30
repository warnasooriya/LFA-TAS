using System.Collections.Generic;

namespace TAS.DataTransfer.Responses
{
    public class AllAvailableTireSizeResponseDto
    {
        public List<string> WidthList { get; set; }
        public List<int> DiameterList { get; set; }
        public List<string> CrossSectionList { get; set; }
        public List<string> LoadSpeedList { get; set; }
        public List<string> PatternList { get; set; }

    }
}
