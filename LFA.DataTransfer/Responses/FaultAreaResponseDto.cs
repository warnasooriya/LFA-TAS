using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    [Serializable]
   public class FaultAreaResponseDto
    {
        public string FaultAreaCode { get; set; }
        public string FaultAreaName { get; set; }
        public Guid Id { get; set; }

        public bool IsActive { get; set; }
    }
}
