using TAS.Services.UnitsOfWork;
using TAS.DataTransfer.Common;
using TAS.DataTransfer.Responses;
using TAS.DataTransfer.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.BusinessServices
{
    internal sealed class EligibilityManagementService : IEligibilityManagementService
    {

        public EligibilitiesResponseDto GetEligibilitys(
            SecurityContext securityContext, 
            AuditContext auditContext)
        {
            EligibilitiesResponseDto result = null;

            EligibilitysRetrievalUnitOfWork uow = new EligibilitysRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }

            result = uow.Result;

            return result;
        }

        public EligibilityRequestDto AddEligibility(EligibilityRequestDto Eligibility, SecurityContext securityContext,
            AuditContext auditContext) {
                EligibilityRequestDto result = new EligibilityRequestDto();
                EligibilityInsertionUnitOfWork uow = new EligibilityInsertionUnitOfWork(Eligibility);
                uow.SecurityContext = securityContext;
                uow.AuditContext = auditContext;

                if (uow.PreExecute())
                {
                    uow.Execute();
                }
                result.EligibilityInsertion = uow.Eligibility.EligibilityInsertion;
                return result;
        }


        public EligibilityResponseDto GetEligibilityById(Guid EligibilityId,
    SecurityContext securityContext,
    AuditContext auditContext)
        {
            EligibilityResponseDto result = new EligibilityResponseDto();

            EligibilityRetrievalUnitOfWork uow = new EligibilityRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            uow.EligibilityId = EligibilityId;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }

        public EligibilityRequestDto UpdateEligibility(EligibilityRequestDto Eligibility, SecurityContext securityContext,
           AuditContext auditContext)
        {
            EligibilityRequestDto result = new EligibilityRequestDto();
            EligibilityUpdationUnitOfWork uow = new EligibilityUpdationUnitOfWork(Eligibility);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result.EligibilityInsertion = uow.Eligibility.EligibilityInsertion;
            return result;
        }

       
       
    }
}
