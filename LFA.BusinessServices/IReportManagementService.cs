using System;
using System.Collections.Generic;
using TAS.DataTransfer.Common;
using TAS.DataTransfer.Requests;
using TAS.DataTransfer.Responses;

namespace TAS.Services
{
    public interface IReportManagementService
    {
        object GetAllReportInformationByUserId(Guid loggedInUserId,
                    SecurityContext securityContext, AuditContext auditContext);
        object GetAllReportParamInformationByReportId(Guid reportId, SecurityContext securityContext, AuditContext auditContext);

        object GetAllDataForReportDropdownElement(Guid reportParameterId, SecurityContext securityContext, AuditContext auditContext);
        object GetAllDataForReportFromParentDropdown(Guid reportParamId, Guid reportParamParentValue,Guid parentParamId, SecurityContext securityContext, AuditContext auditContext);
        object ViewReport(Guid reportId, List<ReportParameterDataRequestDto> paramList, SecurityContext securityContext, AuditContext auditContext);
        object ExcelReport(Guid reportId, List<ReportParameterDataRequestDto> paramList, SecurityContext securityContext, AuditContext auditContext);

       // BordxExportResponseDto ExportPoliciesToExcelByBordxId(ExportPoliciesToExcelByBordxIdRequestDto ExportPoliciesToExcelByBordxIdRequestDto, SecurityContext securityContext, AuditContext auditContext);

    }
}
