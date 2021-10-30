using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    public class ClaimBatchResponseDto
    {
        public List<ClaimBatchingResponseDto> ClaimBatch
        {
            get;
            set;
        }

        public List<ClaimBatchTableResponseDto> ClaimBatchTable
        {
            get;
            set;
        }


    }
}
