using System;
using System.Collections.Generic;
using TAS.DataTransfer.Common;
using TAS.DataTransfer.Requests;
using TAS.DataTransfer.Responses;
using TAS.Services.UnitsOfWork;

namespace TAS.Services.BusinessServices
{
    internal sealed class ReportManagementService : IReportManagementService
    {
        public object GetAllReportInformationByUserId(Guid loggedInUserId, SecurityContext securityContext, AuditContext auditContext)
        {
            GetAllReportInformationByUserIdUnitOfWork uow = new GetAllReportInformationByUserIdUnitOfWork(loggedInUserId);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Result;
        }

        public object GetAllReportParamInformationByReportId(Guid reportId, SecurityContext securityContext, AuditContext auditContext)
        {
            GetAllReportParamInformationByReportIdUnitOfWork uow = new GetAllReportParamInformationByReportIdUnitOfWork(reportId);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Result;
        }

        public object GetAllDataForReportDropdownElement(Guid reportParameterId, SecurityContext securityContext, AuditContext auditContext)
        {
            GetAllDataForReportDropdownElementUnitOfWork uow = new GetAllDataForReportDropdownElementUnitOfWork(reportParameterId);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Result;
        }

        public object GetAllDataForReportFromParentDropdown(Guid reportParamId, Guid reportParamParentValue, Guid parentParamId, SecurityContext securityContext, AuditContext auditContext)
        {
            GetAllDataForReportFromParentDropdownUnitOfWork uow = new GetAllDataForReportFromParentDropdownUnitOfWork(reportParamId, reportParamParentValue, parentParamId);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Result;
        }

        public object ViewReport(Guid reportId, List<ReportParameterDataRequestDto> paramList, SecurityContext securityContext, AuditContext auditContext)
        {
            ViewReportUnitOfWork uow = new ViewReportUnitOfWork(reportId, paramList);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Result;
        }

        public object ExcelReport(Guid reportId, List<ReportParameterDataRequestDto> paramList, SecurityContext securityContext, AuditContext auditContext)
        {
            ExcelDownloadReportUnitOfWork uow = new ExcelDownloadReportUnitOfWork(reportId, paramList);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Result;
        }

        //ExcelExportResponseDto IReportManagementService.ExcelReport(Guid reportId, List<ReportParameterDataRequestDto> paramList, SecurityContext securityContext, AuditContext auditContext)
        //{
        //    throw new NotImplementedException();
        //}
    }
}

