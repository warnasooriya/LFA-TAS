using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    [Serializable]
    public class TaxResponseDto
    {
        public string TaxCode { get; set; }
        public string TaxName { get; set; }
        public bool OnNRP { get; set; }
        public bool OnGross { get; set; }
        public Guid Id { get; set; }

        public bool IsTaxExists { get; set; }
    }
}
