using System;
using TAS.DataTransfer.Common;
using TAS.DataTransfer.Responses;
using TAS.Services.UnitsOfWork;

namespace TAS.Services.BusinessServices
{
    internal sealed class CommodityManagementService : ICommodityManagementService
    {
        public CommoditiesResponseDto GetAllCommodities(SecurityContext securityContext,
            AuditContext auditContext)
        {
            CommoditiesResponseDto result = null;
            CommodityRetrievalUnitOfWork uow = new CommodityRetrievalUnitOfWork();
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;

        }

        public CommodityCategoriesRespondDto GetCommodityCategoriesByCommodityTypeId(SecurityContext securityContext,
            AuditContext auditContext, Guid commodityTypeID)
        {
            CommodityCategoriesRespondDto result = null;
            CommodityCategoriesRetrievalUnitOfWork uow = new CommodityCategoriesRetrievalUnitOfWork(commodityTypeID);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }


        public String GetCommodityTypeIdbyCommodityCategoryId(Guid id, SecurityContext securityContext, AuditContext auditContext)
        {
            String result = String.Empty;
            CommodityTypeIdbyCommodityCategoryIdUnitOfWork uow = new CommodityTypeIdbyCommodityCategoryIdUnitOfWork(id);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }

        public object GetCommodityTypeByCommodityCategoryId(Guid commodityCategoryId, SecurityContext securityContext, AuditContext auditContext)
        {
            object result = null;
            CommodityTypebByCommodityCategoryIdUnitOfWork uow = new CommodityTypebByCommodityCategoryIdUnitOfWork(commodityCategoryId);
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
