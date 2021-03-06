using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class EngineCapacityRequestDto
    {
        public Guid Id { get; set; }
        public decimal EngineCapacityNumber { get; set; }
        public string MesureType { get; set; }
        public DateTime EntryDateTime { get; set; }
        public Guid EntryUser { get; set; }

        public bool EngineCapacityInsertion
        {
            get;
            set;
        }
    }
}
