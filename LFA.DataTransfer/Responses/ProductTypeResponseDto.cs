using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    [Serializable]
    public class ProductTypeResponseDto
    {
        public Guid Id { get; set; }
        public string Type { get; set; }
        public DateTime EntryDateTime { get; set; }
        public Guid EntryUser { get; set; }
        public bool IsProductTypeExists { get; set; }


        public string Code { get; set; }
    }
}
