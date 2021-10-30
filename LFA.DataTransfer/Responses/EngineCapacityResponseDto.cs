using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    [Serializable]
    public class EngineCapacityResponseDto
    {
        public Guid Id { get; set; }
        public decimal EngineCapacityNumber { get; set; }
        public string MesureType { get; set; }
        public DateTime EntryDateTime { get; set; }
        public Guid EntryUser { get; set; }

        public bool IsEngineCapacityExists { get; set; }

    }
}
