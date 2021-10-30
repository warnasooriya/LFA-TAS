using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAS.DataTransfer.Common;
using TAS.DataTransfer.Requests;
using TAS.DataTransfer.Responses;

namespace TAS.Services
{
    public interface IBuyingWizardManagementService
    {
        CustomerLoginResponseDto AuthUser(CustomerLoginRequestDto loginRequest, SecurityContext securityContext, AuditContext auditContext);
    }
}
