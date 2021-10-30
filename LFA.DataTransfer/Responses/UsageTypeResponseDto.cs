using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    [Serializable]
    public class UsageTypeResponseDto
    {
        public int Id { get; set; }
        public string UsageTypeName { get; set; }
        public string UsageTypeCode { get; set; }

    }
}
