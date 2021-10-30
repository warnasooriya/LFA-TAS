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
    internal sealed class PremiumBasedOnManagementService : IPremiumBasedOnManagementService
    {

        public PremiumBasedOnesResponseDto GetPremiumBasedOns(
            SecurityContext securityContext, 
            AuditContext auditContext)
        {
            PremiumBasedOnesResponseDto result = null;

            PremiumBasedOnsRetrievalUnitOfWork uow = new PremiumBasedOnsRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }

            result = uow.Result;

            return result;
        }

        public PremiumBasedOnRequestDto AddPremiumBasedOn(PremiumBasedOnRequestDto PremiumBasedOn, SecurityContext securityContext,
            AuditContext auditContext) {
                PremiumBasedOnRequestDto result = new PremiumBasedOnRequestDto();
                PremiumBasedOnInsertionUnitOfWork uow = new PremiumBasedOnInsertionUnitOfWork(PremiumBasedOn);
                uow.SecurityContext = securityContext;
                uow.AuditContext = auditContext;

                if (uow.PreExecute())
                {
                    uow.Execute();
                }
                result.PremiumBasedOnInsertion = uow.PremiumBasedOn.PremiumBasedOnInsertion;
                return result;
        }


        public PremiumBasedOnResponseDto GetPremiumBasedOnById(Guid PremiumBasedOnId,
    SecurityContext securityContext,
    AuditContext auditContext)
        {
            PremiumBasedOnResponseDto result = new PremiumBasedOnResponseDto();

            PremiumBasedOnRetrievalUnitOfWork uow = new PremiumBasedOnRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            uow.PremiumBasedOnId = PremiumBasedOnId;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }

        public PremiumBasedOnRequestDto UpdatePremiumBasedOn(PremiumBasedOnRequestDto PremiumBasedOn, SecurityContext securityContext,
           AuditContext auditContext)
        {
            PremiumBasedOnRequestDto result = new PremiumBasedOnRequestDto();
            PremiumBasedOnUpdationUnitOfWork uow = new PremiumBasedOnUpdationUnitOfWork(PremiumBasedOn);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result.PremiumBasedOnInsertion = uow.PremiumBasedOn.PremiumBasedOnInsertion;
            return result;
        }

       
       
    }
}
