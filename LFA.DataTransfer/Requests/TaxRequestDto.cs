using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class TaxRequestDto
    {
        public string TaxCode { get; set; }
        public string TaxName { get; set; }
        public Guid Id { get; set; }
        public bool OnNRP { get; set; }
        public bool OnGross { get; set; }
        public bool TaxInsertion
        {
            get;
            set;
        }

    }
}
