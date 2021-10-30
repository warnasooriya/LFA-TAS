using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    [Serializable]
    public class ItemStatusResponseDto
    {
        public Guid Id { get; set; }
        public string Status { get; set; }
        public string ItemStatusDescription { get; set; }
        public DateTime EntryDateTime { get; set; }
        public Guid EntryUser { get; set; }

        public bool IsItemStatusExists { get; set; }

    }
}
