using TAS.DataTransfer.Common;
using TAS.DataTransfer.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services
{
    public interface ICommodityManagementService
    {
        CommoditiesResponseDto GetAllCommodities(SecurityContext securityContext, AuditContext auditContext);

        String GetCommodityTypeIdbyCommodityCategoryId(Guid id, SecurityContext securityContext, AuditContext auditContext);

        CommodityCategoriesRespondDto GetCommodityCategoriesByCommodityTypeId(SecurityContext securityContext, AuditContext auditContext, Guid commodityTypeID);
        object GetCommodityTypeByCommodityCategoryId(Guid commodityCategoryId, SecurityContext securityContext, AuditContext auditContext);
    }
}
