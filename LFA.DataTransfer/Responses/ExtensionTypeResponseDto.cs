using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    [Serializable]
    public class ExtensionTypeResponseDto
    {
        public Guid Id { get; set; }
        public string ExtensionName { get; set; }
        public int Km { get; set; }
        public int Month { get; set; }
        public Guid CommodityTypeId { get; set; }
        public Guid ProductId { get; set; }
        public int Hours { get; set; }
        public DateTime EntryDateTime { get; set; }
        public Guid EntryUser { get; set; }

        public bool IsExtensionTypeExists { get; set; }

    }
}
