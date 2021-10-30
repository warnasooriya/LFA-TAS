using System;
using System.Collections.Generic;
using TAS.DataTransfer.Common;
using TAS.DataTransfer.Requests;
using TAS.DataTransfer.Responses;
using TAS.Services.UnitsOfWork;

namespace TAS.Services.BusinessServices
{
    internal sealed class VariantManagementService : IVariantManagementService
    {

        public VariantsResponseDto GetVariants(
            SecurityContext securityContext,
            AuditContext auditContext)
        {
            VariantsResponseDto result = null;

            VariantsRetrievalUnitOfWork uow = new VariantsRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }

            result = uow.Result;

            return result;
        }

        public VariantsResponseDto GetParentVariants(
    SecurityContext securityContext,
    AuditContext auditContext)
        {
            VariantsResponseDto result = null;

            VariantsRetrievalUnitOfWork uow = new VariantsRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }

            result = uow.Result;

            return result;
        }

        public VariantRequestDto AddVariant(VariantRequestDto Variant, SecurityContext securityContext,
            AuditContext auditContext)
        {
            VariantRequestDto result = new VariantRequestDto();
            VariantInsertionUnitOfWork uow = new VariantInsertionUnitOfWork(Variant);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result.VariantInsertion = uow.Variant.VariantInsertion;
            return result;
        }


        public VariantResponseDto GetVariantById(Guid VariantId,
    SecurityContext securityContext,
    AuditContext auditContext)
        {
            VariantResponseDto result = new VariantResponseDto();

            VariantRetrievalUnitOfWork uow = new VariantRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            uow.VariantId = VariantId;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }

        public VariantRequestDto UpdateVariant(VariantRequestDto Variant, SecurityContext securityContext,
           AuditContext auditContext)
        {
            VariantRequestDto result = new VariantRequestDto();
            VariantUpdationUnitOfWork uow = new VariantUpdationUnitOfWork(Variant);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result.VariantInsertion = uow.Variant.VariantInsertion;
            return result;
        }


        public VariantsResponseDto GetAllVariantsByModelIds(List<Guid> VariantIdList, SecurityContext securityContext, AuditContext auditContext)
        {

            VariantRetrivalByModelIdsUnitOfWork uow = new VariantRetrivalByModelIdsUnitOfWork(VariantIdList);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Result;
        }

    }
}
