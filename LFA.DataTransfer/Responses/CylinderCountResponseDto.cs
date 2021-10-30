using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    [Serializable]
    public class CylinderCountResponseDto
    {
        public Guid Id { get; set; }
        public string Count { get; set; }
        public DateTime EntryDateTime { get; set; }
        public Guid EntryUser { get; set; }

        public bool IsCylinderCountExists { get; set; }

    }
}
