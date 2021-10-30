using TAS.DataTransfer.Common;
using TAS.DataTransfer.Responses;
using TAS.DataTransfer.Requests;
using System;
using TAS.Services.Entities;
namespace TAS.Services
{
    public interface ICommodityCategoryManagementService
    {
        CommodityCategoriesRespondDto GetCommodityCategories(Guid commodityTypeId,
            SecurityContext securityContext, 
            AuditContext auditContext);
       
        CommodityCategoryRequestDto AddCommodityCategory(CommodityCategoryRequestDto CommodityCategory,
            SecurityContext securityContext,
            AuditContext auditContext);


        CommodityCategoryResponseDto GetCommodityCategoryById(Guid CommodityCategoryId, Guid CommodityTypeId,
            SecurityContext securityContext,
            AuditContext auditContext);

        CommodityCategoryRequestDto UpdateCommodityCategory(CommodityCategoryRequestDto CommodityCategory,
            SecurityContext securityContext,
            AuditContext auditContext);





        bool IsExsistingCommodityCategoryByDescription(Guid CommodityCategoryId, string commodityCategoryDescription, SecurityContext securityContext, AuditContext auditContext);
    }
}
