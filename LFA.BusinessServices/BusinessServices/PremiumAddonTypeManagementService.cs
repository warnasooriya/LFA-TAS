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
    internal sealed class PremiumAddonTypeManagementService : IPremiumAddonTypeManagementService
    {

        public PremiumAddonTypesResponseDto GetPremiumAddonTypes(
            SecurityContext securityContext, 
            AuditContext auditContext)
        {
            PremiumAddonTypesResponseDto result = null;

            PremiumAddonTypesRetrievalUnitOfWork uow = new PremiumAddonTypesRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }

            result = uow.Result;

            return result;
        }
        public PremiumAddonTypesResponseDto GetPremiumAddonTypes(Guid CommodityTypeId,
            SecurityContext securityContext,
            AuditContext auditContext)
        {
            PremiumAddonTypesResponseDto result = null;

            PremiumAddonTypesRetrievalUnitOfWorkV2 uow = new PremiumAddonTypesRetrievalUnitOfWorkV2(CommodityTypeId);

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }

            result = uow.Result;

            return result;
        }

        public PremiumAddonTypeRequestDto AddPremiumAddonType(PremiumAddonTypeRequestDto PremiumAddonType, 
            SecurityContext securityContext,
            AuditContext auditContext) {
                PremiumAddonTypeRequestDto result = new PremiumAddonTypeRequestDto();
                PremiumAddonTypeInsertionUnitOfWork uow = new PremiumAddonTypeInsertionUnitOfWork(PremiumAddonType);
                uow.SecurityContext = securityContext;
                uow.AuditContext = auditContext;

                if (uow.PreExecute())
                {
                    uow.Execute();
                }
                result.PremiumAddonTypeInsertion = uow.PremiumAddonType.PremiumAddonTypeInsertion;
                return result;
        }

        public PremiumAddonTypeResponseDto GetPremiumAddonTypeById(Guid PremiumAddonTypeId,
            SecurityContext securityContext,
            AuditContext auditContext)
        {
            PremiumAddonTypeResponseDto result = new PremiumAddonTypeResponseDto();

            PremiumAddonTypeRetrievalUnitOfWork uow = new PremiumAddonTypeRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            uow.PremiumAddonTypeId = PremiumAddonTypeId;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }

        public PremiumAddonTypeRequestDto UpdatePremiumAddonType(PremiumAddonTypeRequestDto PremiumAddonType, 
            SecurityContext securityContext,
           AuditContext auditContext)
        {
            PremiumAddonTypeRequestDto result = new PremiumAddonTypeRequestDto();
            PremiumAddonTypeUpdationUnitOfWork uow = new PremiumAddonTypeUpdationUnitOfWork(PremiumAddonType);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result.PremiumAddonTypeInsertion = uow.PremiumAddonType.PremiumAddonTypeInsertion;
            return result;
        }


        public PremiumAddonTypesResponseDto GetAllPremiumAddonType(SecurityContext securityContext,
            AuditContext auditContext)
        {
            PremiumAddonTypesResponseDto result = null;
            PremiumAddonForVariantRetrievalUnitOfWork uow = new PremiumAddonForVariantRetrievalUnitOfWork();
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;


        }
    }
}
