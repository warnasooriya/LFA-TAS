using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAS.DataTransfer.Common;
using TAS.DataTransfer.Requests;
using TAS.Services.Entities;

namespace TAS.Services
{
    public interface ITempBulkUploadManagementService
    {
        List<TempBulkUpload> tempBulkUploadInsert(TempBulkHeaderRequestDto bulkData, SecurityContext securityContext, AuditContext auditContext);

        bool saveTempBulkUpload(List<TempBulkUpload> bulkData,string CommodityType , string ProductCode, SecurityContext securityContext, AuditContext auditContext);

        List<TempBulkUpload> getTempBulkValidationFailedData(Guid tempBulkHeaderId,string CommodityType, string ProductCode, SecurityContext securityContext, AuditContext auditContext);

        bool isUploaded(TempBulkHeaderRequestDto bulkData, SecurityContext securityContext, AuditContext auditContext);

        byte[] getBulkUplaodTemplate(string commodityCode,string productCode, SecurityContext securityContext, AuditContext auditContext);
    }
}
