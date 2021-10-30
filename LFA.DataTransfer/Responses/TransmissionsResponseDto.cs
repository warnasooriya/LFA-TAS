using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    public class TransmissionTypesResponseDto
    {
        public List<TransmissionTypeResponseDto> TransmissionTypes
        {
            get;
            set;
        }
    }

    public class TransmissionTechnologiesResponseDto
    {
        public List<TransmissionTechnologyResponseDto> TransmissionTechnologies
        {
            get;
            set;
        }
    }
}
