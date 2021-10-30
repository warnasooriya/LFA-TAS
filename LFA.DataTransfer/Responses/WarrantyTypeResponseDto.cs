using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    [Serializable]
    public class WarrantyTypeResponseDto
    {
        public Guid Id { get; set; }
        public string WarrantyTypeDescription { get; set; }
        public DateTime EntryDateTime { get; set; }
        public Guid EntryUser { get; set; }

        public bool IsWarrantyTypeExists { get; set; }

    }
}
