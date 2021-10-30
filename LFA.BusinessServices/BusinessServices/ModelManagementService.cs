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
    internal sealed class ModelManagementService : IModelManagementService
    {

        public ModelesResponseDto GetAllModeles(
            SecurityContext securityContext,
            AuditContext auditContext)
        {
            ModelesResponseDto result = null;

            ModelesRetrievalUnitOfWork uow = new ModelesRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }

            result = uow.Result;

            return result;
        }

        public ModelesResponseDto GetParentModeles(
    SecurityContext securityContext,
    AuditContext auditContext)
        {
            ModelesResponseDto result = null;

            ModelesRetrievalUnitOfWork uow = new ModelesRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }

            result = uow.Result;

            return result;
        }

        public ModelRequestDto AddModel(ModelRequestDto Model, SecurityContext securityContext,
            AuditContext auditContext) {
                ModelRequestDto result = new ModelRequestDto();
                ModelInsertionUnitOfWork uow = new ModelInsertionUnitOfWork(Model);
                uow.SecurityContext = securityContext;
                uow.AuditContext = auditContext;

                if (uow.PreExecute())
                {
                    uow.Execute();
                }
                result.ModelInsertion = uow.Model.ModelInsertion;
                return result;
        }


        public ModelResponseDto GetModelById(Guid ModelId,
    SecurityContext securityContext,
    AuditContext auditContext)
        {
            ModelResponseDto result = new ModelResponseDto();

            ModelRetrievalUnitOfWork uow = new ModelRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            uow.ModelId = ModelId;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }

        public ModelRequestDto UpdateModel(ModelRequestDto Model, SecurityContext securityContext,
           AuditContext auditContext)
        {
            ModelRequestDto result = new ModelRequestDto();
            ModelUpdationUnitOfWork uow = new ModelUpdationUnitOfWork(Model);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result.ModelInsertion = uow.Model.ModelInsertion;
            return result;
        }

        public ModelesResponseDto GetModelsByMakeId(Guid MakeId,
    SecurityContext securityContext,
    AuditContext auditContext)
        {
            ModelesResponseDto result = new ModelesResponseDto();
            ModelRetrievalUnitOfWork uow = new ModelRetrievalUnitOfWork();
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            uow.MakeId = MakeId;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Results;
            return result;
        }

        public ModelesResponseDto GetModelesByMakeIdsAndCommodityCategoryId(List<Guid> MakeIdList, Guid CommodityCategoryId, SecurityContext securityContext, AuditContext auditContext)
        {
            ModelesResponseDto result = new ModelesResponseDto();
            ModelRetrievalByMakeIdsAndCommodityCategoryIdUnitOfWork uow = new ModelRetrievalByMakeIdsAndCommodityCategoryIdUnitOfWork(MakeIdList, CommodityCategoryId);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Results;
            return result;
        }

        public object GetModelByMakeIdAndCatogaryId(Guid MakeId, Guid CommodityCategoryId, SecurityContext securityContext, AuditContext auditContext)
        {
            object Response = null;
            ModelsRetriveByMakeIdandCatogaryIdUnitOfWork uow = new ModelsRetriveByMakeIdandCatogaryIdUnitOfWork(MakeId, CommodityCategoryId);
            uow.AuditContext = auditContext;
            uow.SecurityContext = securityContext;
            if (uow.PreExecute())
            {
                uow.Execute();
                Response = uow.Result;
            }
            return Response;
        }

        public object GetModelesByMakeIds(SecurityContext securityContext, AuditContext auditContext, List<Guid> makeIdList)
        {
            object result = null;
            ModelesRetrievalByMakesIdsUnitOfWork uow = new ModelesRetrievalByMakesIdsUnitOfWork(makeIdList);
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
