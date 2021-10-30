using TAS.DataTransfer.Common;
using TAS.DataTransfer.Responses;
using TAS.DataTransfer.Requests;
using System;
using TAS.Services.Entities;
using System.Collections.Generic;
namespace TAS.Services
{
    public interface IModelManagementService
    {
        ModelesResponseDto GetAllModeles(
            SecurityContext securityContext,
            AuditContext auditContext);
        object GetModelesByMakeIds(
      SecurityContext securityContext,
      AuditContext auditContext,List<Guid> makeIdList);

        ModelRequestDto AddModel(ModelRequestDto Model,
            SecurityContext securityContext,
            AuditContext auditContext);


        ModelResponseDto GetModelById(Guid ModelId,
            SecurityContext securityContext,
            AuditContext auditContext);

        ModelRequestDto UpdateModel(ModelRequestDto Model,
            SecurityContext securityContext,
            AuditContext auditContext);


        ModelesResponseDto GetModelsByMakeId(Guid MakeId,
   SecurityContext securityContext,
   AuditContext auditContext);


        ModelesResponseDto GetModelesByMakeIdsAndCommodityCategoryId(List<Guid> MakeIdList, Guid CommodityCategoryId, SecurityContext securityContext, AuditContext auditContext);

        object GetModelByMakeIdAndCatogaryId(Guid MakeId, Guid CommodityCategoryId, SecurityContext securityContext, AuditContext auditContext);
    }
}
