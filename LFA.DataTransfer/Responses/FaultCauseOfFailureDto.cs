using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    [Serializable]
     public class FaultCauseOfFailureDto
    {
        public  Guid Id { get; set; }
        public  Guid FaultId { get; set; }
        public  string CauseOfFailure { get; set; }
    }
}
