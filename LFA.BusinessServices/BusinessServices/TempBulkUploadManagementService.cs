using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAS.DataTransfer.Common;
using TAS.DataTransfer.Requests;
using TAS.Services.Entities;
using TAS.Services.UnitsOfWork;

namespace TAS.Services.BusinessServices
{
    internal sealed class TempBulkUploadManagementService : ITempBulkUploadManagementService
    {
        public List<TempBulkUpload> tempBulkUploadInsert(TempBulkHeaderRequestDto bulkData, SecurityContext securityContext,
    AuditContext auditContext)
        {

            TempBulkUploadInsertionUnitOfWork uow = new TempBulkUploadInsertionUnitOfWork(bulkData);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute()) { uow.Execute(); }
            return uow.Result;
        }

        public bool saveTempBulkUpload(List<TempBulkUpload> bulkData, string CommodityType, string ProductCode, SecurityContext securityContext,
    AuditContext auditContext)
        {
            TempBulkSaveUnitOfWork uow = new TempBulkSaveUnitOfWork(bulkData,  CommodityType,  ProductCode);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute()) { uow.Execute(); }
            return uow.Result;
        }

        public List<TempBulkUpload> getTempBulkValidationFailedData(Guid tempBulkHeaderId, string CommodityType, string ProductCode, SecurityContext securityContext,
   AuditContext auditContext)
        {
            TempBulkGetValidationFailedDataUnitOfWork uow = new TempBulkGetValidationFailedDataUnitOfWork(tempBulkHeaderId, CommodityType, ProductCode);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute()) { uow.Execute(); }
            return uow.Result;
        }

        public bool isUploaded(TempBulkHeaderRequestDto bulkData, SecurityContext securityContext,
    AuditContext auditContext)
        {
            TempBulkUploadIsUploadedUnitOfWork uow = new TempBulkUploadIsUploadedUnitOfWork(bulkData);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute()) { uow.Execute(); }
            return uow.Result;
        }

        public byte[] getBulkUplaodTemplate(string commodityCode, string productCode, SecurityContext securityContext,
    AuditContext auditContext)
        {
            TempBulkUploadGetTemplateByCommodityCodeUnitOfWork uow = new TempBulkUploadGetTemplateByCommodityCodeUnitOfWork(commodityCode, productCode);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute()) { uow.Execute(); }
            return uow.Result;
        }

    }
}
