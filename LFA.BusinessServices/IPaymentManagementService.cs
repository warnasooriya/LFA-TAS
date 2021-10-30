using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAS.DataTransfer.Common;
using TAS.DataTransfer.Responses;

namespace TAS.Services
{
    public interface IPaymentManagementService
    {
        PaymentModesResponseDto GetAllPaymentModes(SecurityContext securityContext,
            AuditContext auditContext);

        PaymentTypesResponseDto GetAllPaymentTypesByPaymentModeId(Guid PaymentModeId,SecurityContext securityContext, AuditContext auditContext);
    }
}
