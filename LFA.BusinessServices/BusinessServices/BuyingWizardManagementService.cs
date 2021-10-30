using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAS.DataTransfer.Common;
using TAS.DataTransfer.Requests;
using TAS.DataTransfer.Responses;
using TAS.Services.UnitsOfWork;

namespace TAS.Services.BusinessServices
{
    internal sealed class BuyingWizardManagementService : IBuyingWizardManagementService
    {
        public CustomerLoginResponseDto AuthUser(CustomerLoginRequestDto LoginRequest, SecurityContext securityContext,
            AuditContext auditContext)
        {
            CustomerLoginResponseDto result = new CustomerLoginResponseDto();
            ReturnigCustomerLoginAuthUnitOfWork uow = new ReturnigCustomerLoginAuthUnitOfWork(LoginRequest);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
                result = uow.Result;
            }
            return result;
        }
    }
}
