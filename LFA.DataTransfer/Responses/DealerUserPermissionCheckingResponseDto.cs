using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    [Serializable]
    public class DealerUserPermissionCheckingResponseDto
    {
        public bool availableTyreSalesRole { get; set; }
        public bool availableTyreClaimSubmitRole { get; set; }
    }
}
