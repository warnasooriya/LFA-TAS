using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAS.DataTransfer.Common;
using TAS.DataTransfer.Responses;
using TAS.Services.UnitsOfWork;

namespace TAS.Services.BusinessServices
{
    internal sealed class PaymentManagementService:IPaymentManagementService
    {
        public PaymentModesResponseDto GetAllPaymentModes(SecurityContext securityContext,
           AuditContext auditContext)
        {
            PaymentModesResponseDto PaymentModesResponseDto = new PaymentModesResponseDto();
            PaymentModesRetrievalUnitOfWork uow = new PaymentModesRetrievalUnitOfWork();
            uow.AuditContext = auditContext;
            uow.SecurityContext = securityContext;
            if (uow.PreExecute())
            {
                uow.Execute();
                PaymentModesResponseDto = uow.Result;
            }
            return PaymentModesResponseDto;
        }

        public PaymentTypesResponseDto GetAllPaymentTypesByPaymentModeId(Guid PaymentModeId,SecurityContext securityContext, AuditContext auditContext)
        {
            PaymentTypesResponseDto PaymentTypesResponseDto = new PaymentTypesResponseDto();
            PaymentTypesByPaymentModeIdRetrievalUnitOfWork uow = new PaymentTypesByPaymentModeIdRetrievalUnitOfWork(PaymentModeId);
            uow.AuditContext = auditContext;
            uow.SecurityContext = securityContext;
            if (uow.PreExecute())
            {
                uow.Execute();
                PaymentTypesResponseDto = uow.Result;
            }
            return PaymentTypesResponseDto;
        }
    }
}
