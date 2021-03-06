using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    [Serializable]
    public class DriveTypeResponseDto
    {
        public Guid Id { get; set; }
        public string Type { get; set; }
        public string DriveTypeDescription { get; set; }
        public DateTime EntryDateTime { get; set; }
        public Guid EntryUser { get; set; }

        public bool IsDriveTypeExists { get; set; }

    }
}
