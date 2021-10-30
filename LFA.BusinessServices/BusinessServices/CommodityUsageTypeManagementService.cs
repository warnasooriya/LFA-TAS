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
    internal sealed class CommodityUsageTypeManagementService : ICommodityUsageTypeManagementService
    {

        public CommodityUsageTypesResponseDto GetCommodityUsageTypes(
            SecurityContext securityContext, 
            AuditContext auditContext)
        {
            CommodityUsageTypesResponseDto result = null;

            CommodityUsageTypesRetrievalUnitOfWork uow = new CommodityUsageTypesRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }

            result = uow.Result;

            return result;
        }

        public CommodityUsageTypesResponseDto GetParentCommodityUsageTypes(
    SecurityContext securityContext,
    AuditContext auditContext)
        {
            CommodityUsageTypesResponseDto result = null;

            CommodityUsageTypesRetrievalUnitOfWork uow = new CommodityUsageTypesRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }

            result = uow.Result;

            return result;
        }

        public CommodityUsageTypeRequestDto AddCommodityUsageType(CommodityUsageTypeRequestDto CommodityUsageType, SecurityContext securityContext,
            AuditContext auditContext) {
                CommodityUsageTypeRequestDto result = new CommodityUsageTypeRequestDto();
                CommodityUsageTypeInsertionUnitOfWork uow = new CommodityUsageTypeInsertionUnitOfWork(CommodityUsageType);
                uow.SecurityContext = securityContext;
                uow.AuditContext = auditContext;

                if (uow.PreExecute())
                {
                    uow.Execute();
                }
                result.CommodityUsageTypeInsertion = uow.CommodityUsageType.CommodityUsageTypeInsertion;
                return result;
        }


        public CommodityUsageTypeResponseDto GetCommodityUsageTypeById(Guid CommodityUsageTypeId,
    SecurityContext securityContext,
    AuditContext auditContext)
        {
            CommodityUsageTypeResponseDto result = new CommodityUsageTypeResponseDto();

            CommodityUsageTypeRetrievalUnitOfWork uow = new CommodityUsageTypeRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            uow.CommodityUsageTypeId = CommodityUsageTypeId;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }

        public CommodityUsageTypeRequestDto UpdateCommodityUsageType(CommodityUsageTypeRequestDto CommodityUsageType, SecurityContext securityContext,
           AuditContext auditContext)
        {
            CommodityUsageTypeRequestDto result = new CommodityUsageTypeRequestDto();
            CommodityUsageTypeUpdationUnitOfWork uow = new CommodityUsageTypeUpdationUnitOfWork(CommodityUsageType);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result.CommodityUsageTypeInsertion = uow.CommodityUsageType.CommodityUsageTypeInsertion;
            return result;
        }

        public bool IsExsistingCommodityUsageTypeByName(Guid Id, string Name,
             SecurityContext securityContext,
             AuditContext auditContext)
        {
            bool Result = false;
            IsExsistingCommodityUsageTypeByName uow = new IsExsistingCommodityUsageTypeByName();
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            uow.Name = Name;
            uow.Description = "";
            uow.Id = Id;
            if (uow.PreExecute()) { uow.Execute(); }
            Result = uow.Result;
            return Result;
        }
       
    }
}
