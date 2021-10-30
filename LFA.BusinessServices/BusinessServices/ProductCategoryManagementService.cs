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
    internal sealed class CommodityCategoryManagementService : ICommodityCategoryManagementService
    {

        public CommodityCategoriesRespondDto GetCommodityCategories(Guid commodityTypeId,
            SecurityContext securityContext, 
            AuditContext auditContext)
        {
            CommodityCategoriesRespondDto result = null;

            CommodityCategoriesRetrievalUnitOfWork uow = new CommodityCategoriesRetrievalUnitOfWork(commodityTypeId);

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }

            result = uow.Result;

            return result;
        }

      
    //    public CommodityCategoriesRespondDto GetParentCommodityCategories(
    //SecurityContext securityContext,
    //AuditContext auditContext)
    //    {
    //        CommodityCategoriesResponseDto result = null;

    //        CommodityCategoriesRetrievalUnitOfWork uow = new CommodityCategoriesRetrievalUnitOfWork();

    //        uow.SecurityContext = securityContext;
    //        uow.AuditContext = auditContext;

    //        uow.Execute();

    //        result = uow.Result;

    //        return result;
    //    }

        public CommodityCategoryRequestDto AddCommodityCategory(CommodityCategoryRequestDto CommodityCategory, 
            SecurityContext securityContext,
            AuditContext auditContext) {
                CommodityCategoryRequestDto result = new CommodityCategoryRequestDto();
                CommodityCategoryInsertionUnitOfWork uow = new CommodityCategoryInsertionUnitOfWork(CommodityCategory);
                uow.SecurityContext = securityContext;
                uow.AuditContext = auditContext;

                if (uow.PreExecute())
                {
                    uow.Execute();
                }
                result.CommodityCategoryInsertion = uow.CommodityCategory.CommodityCategoryInsertion;
                return result;
        }


        public CommodityCategoryResponseDto GetCommodityCategoryById(Guid CommodityCategoryId, Guid CommodityTypeId,
    SecurityContext securityContext,
    AuditContext auditContext)
        {
            CommodityCategoryResponseDto result = new CommodityCategoryResponseDto();

            CommodityCategoryRetrievalUnitOfWork uow = new CommodityCategoryRetrievalUnitOfWork(CommodityTypeId);

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            uow.CommodityCategoryId = CommodityCategoryId;
            uow.commodityTypeId = CommodityTypeId;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }

        public CommodityCategoryRequestDto UpdateCommodityCategory(CommodityCategoryRequestDto CommodityCategory, SecurityContext securityContext,
           AuditContext auditContext)
        {
            CommodityCategoryRequestDto result = new CommodityCategoryRequestDto();
            CommodityCategoryUpdationUnitOfWork uow = new CommodityCategoryUpdationUnitOfWork(CommodityCategory);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result.CommodityCategoryInsertion = uow.CommodityCategory.CommodityCategoryInsertion;
            return result;
        }

        public bool IsExsistingCommodityCategoryByDescription(Guid CommodityCategoryId, string commodityCategoryDescription,
             SecurityContext securityContext,
             AuditContext auditContext)
        {
            bool Result = false;
            IsExsistingCommodityCategoryByDescriptionandCode uow = new IsExsistingCommodityCategoryByDescriptionandCode();
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            uow.CommodityCategoryDescription = commodityCategoryDescription;
            uow.CommodityCategoryCode = "";
            uow.CommodityCategoryId = CommodityCategoryId;
            if (uow.PreExecute()) { uow.Execute(); }
            Result = uow.Result;
            return Result;
        }
       
    }
}
