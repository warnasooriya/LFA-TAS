using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    [Serializable]
    public class FuelTypeResponseDto
    {
        public Guid FuelTypeId { get; set; }
        public string FuelTypeCode { get; set; }
        public string FuelTypeDescription { get; set; }
        public DateTime EntryDateTime { get; set; }
        public Guid EntryUser { get; set; }

        public bool IsFuelTypeExists { get; set; }

    }
}
