using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAS.DataTransfer.Common;
using TAS.DataTransfer.Requests;
using TAS.DataTransfer.Responses;
using TAS.Services.UnitsOfWork;

namespace TAS.Services.BusinessServices
{
    internal sealed class ClaimBordxProcessManagementService : IClaimBordxProcessManagementService
    {
        public List<int> GetClaimBordxYears(SecurityContext securityContext, AuditContext auditContext)
        {
            List<int> Response = null;
            ClaimBordxRetriveYearsUnitOfWork uow = new ClaimBordxRetriveYearsUnitOfWork();
            uow.AuditContext = auditContext;
            uow.SecurityContext = securityContext;
            if (uow.PreExecute())
            {
                uow.Execute();
                Response = uow.result;
            }
            return Response;
        }

        public List<int> GetBordxYearsByClaim(SecurityContext securityContext, AuditContext auditContext)
        {
            List<int> Response = null;
            ClaimYearsRetriveUnitOfWork uow = new ClaimYearsRetriveUnitOfWork();
            uow.AuditContext = auditContext;
            uow.SecurityContext = securityContext;
            if (uow.PreExecute())
            {
                uow.Execute();
                Response = uow.result;
            }
            return Response;
        }
        public object ProcessClaimBordxForSearchGrid(
           ClaimSearchGridRequestDto ClaimSearchGridRequestDto,
           SecurityContext securityContext,
           AuditContext auditContext)
        {
            ProcessClaimBordxForSearchGridUnitOfWork uow = new ProcessClaimBordxForSearchGridUnitOfWork(ClaimSearchGridRequestDto);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }

            return uow.Result;
        }

        public object GetClaimBordxByYearAndMonth(int Year, int Month, Guid Insurerid, Guid Reinsurerid, SecurityContext securityContext, AuditContext auditContext)
        {
            object Response = null;
            ClaimBordxRetriveByYearAndMonthUnitOfWork uow = new ClaimBordxRetriveByYearAndMonthUnitOfWork(Year, Month, Insurerid, Reinsurerid);
            uow.AuditContext = auditContext;
            uow.SecurityContext = securityContext;
            if (uow.PreExecute())
            {
                uow.Execute();
                Response = uow.result;
            }
            return Response;
        }

        public bool ClaimBordxProcess(Guid ClaimbordxId, Guid UserId, bool IsProcess, SecurityContext securityContext, AuditContext auditContext)
        {
            bool Response = false;
            ClaimBordxProcessUnitOfWork uow = new ClaimBordxProcessUnitOfWork(ClaimbordxId, UserId, IsProcess);
            uow.AuditContext = auditContext;
            uow.SecurityContext = securityContext;
            if (uow.PreExecute())
            {
                uow.Execute();
                Response = uow.result;
            }
            return Response;
        }

        public bool ClaimBordxProcessUpdate(Guid ClaimbordxId, Guid UserId, bool IsConfirm, SecurityContext securityContext, AuditContext auditContext)
        {
            bool Response = false;
            ClaimBordxConfirmUnitOfWork uow = new ClaimBordxConfirmUnitOfWork(ClaimbordxId, UserId, IsConfirm);
            uow.AuditContext = auditContext;
            uow.SecurityContext = securityContext;
            if (uow.PreExecute())
            {
                uow.Execute();
                Response = uow.result;
            }
            return Response;
        }

        public object ClaimBordxProcessedDetailsByYear(int Year, Guid Insurerid, Guid Reinsurerid, SecurityContext securityContext, AuditContext auditContext)
        {
            object Response = null;
            ClaimBordxProcessedDetailsByYearUnitOfWork uow = new ClaimBordxProcessedDetailsByYearUnitOfWork(Year, Insurerid, Reinsurerid);
            uow.AuditContext = auditContext;
            uow.SecurityContext = securityContext;
            if (uow.PreExecute())
            {
                uow.Execute();
                Response = uow.result;
            }
            return Response;
        }

        public bool ClaimBordxCanConfirmedByYear(int year, Guid insurerid, Guid reinsurerid, SecurityContext securityContext, AuditContext auditContext)
        {
            bool Response = false;
            ClaimBordexCanConfirmedByYearUnitOfWork uow = new ClaimBordexCanConfirmedByYearUnitOfWork(year, insurerid, reinsurerid);
            uow.AuditContext = auditContext;
            uow.SecurityContext = securityContext;
            if (uow.PreExecute())
            {
                uow.Execute();
                Response = uow.result;
            }
            return Response;
        }

        public ClaimBordxExportResponseDto GetProcessClaimBordxExport(Guid bordxId, SecurityContext securityContext, AuditContext auditContext)
        {
            ClaimBordxToExcelUnitOfWork uow = new ClaimBordxToExcelUnitOfWork(bordxId);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Result;
        }

    }
}
