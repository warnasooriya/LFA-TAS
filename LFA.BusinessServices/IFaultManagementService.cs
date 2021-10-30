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
    public interface IFaultManagementService
    {
        #region FaultArea
        FaultCategorysResponseDto GetAllCatFaults(
            SecurityContext securityContext,
            AuditContext auditContext);

        FaultCategoryResponseDto GetFaultCategoryById(Guid FaultCategoryId,
            SecurityContext securityContext,
            AuditContext auditContext);

        FaultCategoryRequestDto AddFaultCategory(FaultCategoryRequestDto FaultCategory,
            SecurityContext securityContext,
            AuditContext auditContext);

        FaultCategoryRequestDto UpdateFaultCategory(FaultCategoryRequestDto FaultCategory,
           SecurityContext securityContext,
           AuditContext auditContext);

        #endregion

        #region FaultArea
        FaultAreasResponseDto GetAllFaultAreas(
            SecurityContext securityContext,
            AuditContext auditContext);
        
        FaultAreaResponseDto GetFaultAreaById(Guid FaultAreaId,
            SecurityContext securityContext,
            AuditContext auditContext);

        FaultAreaRequestDto AddFaultArea(FaultAreaRequestDto FaultArea,
           SecurityContext securityContext,
           AuditContext auditContext);

        FaultAreaRequestDto UpdateFaultArea(FaultAreaRequestDto FaultArea,
           SecurityContext securityContext,
           AuditContext auditContext);
        #endregion

        #region Fault
        FaultsResponseDto GetAllFaults(
            SecurityContext securityContext,
            AuditContext auditContext);

        FaultResponseDto GetFaultById(Guid FaultId,
            SecurityContext securityContext,
            AuditContext auditContext);

        FaultRequestDto AddFault(FaultRequestDto Fault,
           SecurityContext securityContext,
           AuditContext auditContext);

        FaultRequestDto UpdateFault(FaultRequestDto Fault,
           SecurityContext securityContext,
           AuditContext auditContext);

        FaultCauseOfFailuresDto GetAllFaultCauseOfFailures(SecurityContext securityContext, AuditContext auditContext);
        object GetAllCauseOfFailuresByFaultId(Guid faultId, SecurityContext securityContext, AuditContext auditContext);
        object SearchFaultsByCriterias(FaultSearchRequestDto faultSearchRequestDto, SecurityContext securityContext, AuditContext auditContext);
        object GetAllFaultForSearchGrid(FaultSearchRequestDto faultSearchRequestDto, SecurityContext securityContext, AuditContext auditContext);
        #endregion

        object ValidateFaultCode(string faultCode,Guid faultCategoryId,Guid faultAreaId, SecurityContext context, AuditContext auditContext);        

    }
}
