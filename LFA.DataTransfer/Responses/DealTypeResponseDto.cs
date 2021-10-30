using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    [Serializable]
    public class DealTypeResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public bool IsDealTypeExists { get; set; }

    }
}
