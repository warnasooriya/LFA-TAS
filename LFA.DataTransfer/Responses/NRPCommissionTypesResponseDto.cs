using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    [Serializable]
    public class NRPCommissionTypesResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public bool IsForTPA { get; set; }

        public bool IsNRPCommissionTypesExists { get; set; }

    }
}
