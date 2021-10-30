using System;
using TAS.DataTransfer.Common;
using TAS.DataTransfer.Requests;
using TAS.DataTransfer.Responses;
using TAS.Services.UnitsOfWork;

namespace TAS.Services.BusinessServices
{
    internal sealed class FaultManagementService : IFaultManagementService
    {
        #region FaultCategory
        public FaultCategorysResponseDto GetAllCatFaults(
            SecurityContext securityContext,
            AuditContext auditContext)
        {
            FaultCategorysResponseDto result = null;
            FaultCategorysRetrievalUnitOfWork uow = new FaultCategorysRetrievalUnitOfWork();
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;

        }

        public FaultCategoryResponseDto GetFaultCategoryById(Guid FaultCategoryId,
           SecurityContext securityContext,
           AuditContext auditContext)
        {
            FaultCategoryResponseDto result = new FaultCategoryResponseDto();

            FaultCategoryRetrievalUnitOfWork uow = new FaultCategoryRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            uow.FaultCategoryId = FaultCategoryId;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }

        public FaultCategoryRequestDto AddFaultCategory(FaultCategoryRequestDto FaultCategory,
        SecurityContext securityContext,
        AuditContext auditContext)
        {
            FaultCategoryRequestDto result = new FaultCategoryRequestDto();
            FaultCategoryInsertionUnitOfWork uow = new FaultCategoryInsertionUnitOfWork(FaultCategory);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result.FaultCategoryInsertion = uow.FaultCategory.FaultCategoryInsertion;
            return result;
        }

        public FaultCategoryRequestDto UpdateFaultCategory(FaultCategoryRequestDto FaultCategory,
            SecurityContext securityContext,
           AuditContext auditContext)
        {
            FaultCategoryRequestDto result = new FaultCategoryRequestDto();
            FaultCategoryUpdationUnitOfWork uow = new FaultCategoryUpdationUnitOfWork(FaultCategory);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result.FaultCategoryInsertion = uow.FaultCategory.FaultCategoryInsertion;
            return result;
        }
        #endregion

        #region FaultArea
        public FaultAreasResponseDto GetAllFaultAreas(
            SecurityContext securityContext,
            AuditContext auditContext)
        {
            FaultAreasResponseDto result = null;
            FaultAreasRetrievalUnitOfWork uow = new FaultAreasRetrievalUnitOfWork();
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;

        }

        public FaultAreaResponseDto GetFaultAreaById(Guid FaultAreaId,
          SecurityContext securityContext,
          AuditContext auditContext)
        {
            FaultAreaResponseDto result = new FaultAreaResponseDto();

            FaultAreaRetrievalUnitOfWork uow = new FaultAreaRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            uow.FaultAreaId = FaultAreaId;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }

        public FaultAreaRequestDto AddFaultArea(FaultAreaRequestDto FaultArea,
        SecurityContext securityContext,
        AuditContext auditContext)
        {
            FaultAreaRequestDto result = new FaultAreaRequestDto();
            FaultAreaInsertionUnitOfWork uow = new FaultAreaInsertionUnitOfWork(FaultArea);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result.FaultAreaInsertion = uow.FaultArea.FaultAreaInsertion;
            return result;
        }

        public FaultAreaRequestDto UpdateFaultArea(FaultAreaRequestDto FaultArea,
            SecurityContext securityContext,
           AuditContext auditContext)
        {
            FaultAreaRequestDto result = new FaultAreaRequestDto();
            FaultAreaUpdationUnitOfWork uow = new FaultAreaUpdationUnitOfWork(FaultArea);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result.FaultAreaInsertion = uow.FaultArea.FaultAreaInsertion;
            return result;
        }
        #endregion

        #region Fault
        public FaultsResponseDto GetAllFaults(
            SecurityContext securityContext,
            AuditContext auditContext)
        {
            FaultsResponseDto result = null;
            FaultsRetrievalUnitOfWork uow = new FaultsRetrievalUnitOfWork();
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;

        }

        public FaultResponseDto GetFaultById(Guid FaultAreaId,
          SecurityContext securityContext,
          AuditContext auditContext)
        {
            FaultResponseDto result = new FaultResponseDto();

            FaultRetrievalUnitOfWork uow = new FaultRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            uow.FaultId = FaultAreaId;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }

        public FaultRequestDto AddFault(FaultRequestDto Fault,
        SecurityContext securityContext,
        AuditContext auditContext)
        {
            FaultRequestDto result = new FaultRequestDto();
            FaultInsertionUnitOfWork uow = new FaultInsertionUnitOfWork(Fault);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result.FaultInsertion = uow.Fault.FaultInsertion;
            return result;
        }

        public FaultRequestDto UpdateFault(FaultRequestDto Fault,
            SecurityContext securityContext,
           AuditContext auditContext)
        {
            FaultRequestDto result = new FaultRequestDto();
            FaultUpdationUnitOfWork uow = new FaultUpdationUnitOfWork(Fault);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result.FaultInsertion = uow.Fault.FaultInsertion;
            return result;
        }

        public FaultCauseOfFailuresDto GetAllFaultCauseOfFailures(
            SecurityContext securityContext,
            AuditContext auditContext)
        {
            FaultCauseOfFailuresDto result = null;
            FaultCauseOfFailureRetrievalUnitOfWork uow = new FaultCauseOfFailureRetrievalUnitOfWork();
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;

        }

        public object GetAllCauseOfFailuresByFaultId(Guid faultId, SecurityContext securityContext, AuditContext auditContext)
        {
            object response = null;
            GetAllCauseOfFailuresByFaultIdUnitOfWork uow = new GetAllCauseOfFailuresByFaultIdUnitOfWork(faultId);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            response = uow.Result;
            return response;
        }

        public object SearchFaultsByCriterias(FaultSearchRequestDto faultSearchRequestDto, SecurityContext securityContext, AuditContext auditContext)
        {
            object result = null;
            SearchFaultsByCriteriasUnitOfWork uow = new SearchFaultsByCriteriasUnitOfWork(faultSearchRequestDto);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;

        }

        public object GetAllFaultForSearchGrid(FaultSearchRequestDto faultSearchRequestDto, SecurityContext securityContext, AuditContext auditContext)
        {
            object result = null;
            SearchFaultsGridPagineUnitOfWork uow = new SearchFaultsGridPagineUnitOfWork(faultSearchRequestDto);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;

        }

        public object ValidateFaultCode(string faultCode,Guid faultCategoryId,Guid faultAreaId, SecurityContext securityContext, AuditContext auditContext)
        {
            object response = new object();
            ValidateFaultCodeUnitOfWork uow = new ValidateFaultCodeUnitOfWork(faultCode, faultCategoryId, faultAreaId);
            uow.AuditContext = auditContext;
            uow.SecurityContext = securityContext;
            if (uow.PreExecute())
            {
                uow.Execute();
                response = uow.Result;
            }
            return response;
        }        

        #endregion
    }
}