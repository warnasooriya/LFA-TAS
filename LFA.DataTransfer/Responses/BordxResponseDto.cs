using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    public class BordxResponseDto
    {
        public Guid Id { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public bool IsConformed { get; set; }
        public bool IsProcessed { get; set; }
        public DateTime EntryDateTime { get; set; }
        public Guid EntryUser { get; set; }

        public bool IsBordxExists { get; set; }

    }
}
