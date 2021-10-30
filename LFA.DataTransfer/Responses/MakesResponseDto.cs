using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    [Serializable]
    public class MakesResponseDto
    {
        public List<MakeResponseDto> Makes
        {
            get;
            set;
        }
    }
}
