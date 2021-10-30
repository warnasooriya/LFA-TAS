using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    [Serializable]
    public class TransmissionTypeResponseDto
    {
        public Guid Id { get; set; }
        public string TransmissionTypeCode { get; set; }
        public List<string> Technology { get; set; }
        public DateTime EntryDateTime { get; set; }
        public Guid EntryUser { get; set; }

        public bool IsTransmissionTypeExists { get; set; }

    }
    public class TransmissionTechnologyResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public bool IsTransmissionTechnologyExists { get; set; }

    }
}
