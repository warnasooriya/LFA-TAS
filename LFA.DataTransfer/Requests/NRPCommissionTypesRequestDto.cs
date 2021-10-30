using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class NRPCommissionTypesRequestDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public bool IsForTPA { get; set; }

        public bool NRPCommissionTypesInsertion
        {
            get;
            set;
        }

    }
}
