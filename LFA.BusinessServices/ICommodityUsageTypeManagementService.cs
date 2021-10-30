using TAS.DataTransfer.Common;
using TAS.DataTransfer.Responses;
using TAS.DataTransfer.Requests;
using System;
using TAS.Services.Entities;
namespace TAS.Services
{
    public interface ICommodityUsageTypeManagementService
    {
          CommodityUsageTypesResponseDto GetCommodityUsageTypes(
            SecurityContext securityContext, 
            AuditContext auditContext);

        CommodityUsageTypesResponseDto GetParentCommodityUsageTypes(
            SecurityContext securityContext, 
            AuditContext auditContext);

        CommodityUsageTypeRequestDto AddCommodityUsageType(CommodityUsageTypeRequestDto CommodityUsageType,
            SecurityContext securityContext,
            AuditContext auditContext);

        CommodityUsageTypeResponseDto GetCommodityUsageTypeById(Guid CommodityUsageTypeId,
            SecurityContext securityContext,
            AuditContext auditContext);

        CommodityUsageTypeRequestDto UpdateCommodityUsageType(CommodityUsageTypeRequestDto CommodityUsageType,
            SecurityContext securityContext,
            AuditContext auditContext);


        bool IsExsistingCommodityUsageTypeByName(Guid Id, string name, SecurityContext securityContext, AuditContext auditContext);
    }
}
