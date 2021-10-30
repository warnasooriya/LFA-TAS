using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class RSAProviderRequestDto
    {
        public Guid Id { get; set; }
        public string ProviderName { get; set; }
        public string ProviderCode { get; set; }

        public bool RSAProviderInsertion
        {
            get;
            set;
        }

    }
}
