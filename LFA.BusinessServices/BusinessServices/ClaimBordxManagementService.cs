using System;
using TAS.DataTransfer.Common;
using TAS.DataTransfer.Requests;
using TAS.DataTransfer.Responses;
using TAS.Services.UnitsOfWork;

namespace TAS.Services.BusinessServices
{
    internal sealed class ClaimBordxManagementService : IClaimBordxManagementService
    {
        public ClaimBordxsResponseDto GetBordxs(
            SecurityContext securityContext,
            AuditContext auditContext)
        {
            ClaimBordxsResponseDto result = null;

            ClaimBordxsRetrievalUnitOfWork uow = new ClaimBordxsRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }

            result = uow.Result;

            return result;
        }

        public string AddClaimBordx(ClaimBordxRequestDto ClaimBordx,
            SecurityContext securityContext,
            AuditContext auditContext)
        {
            ClaimBordxRequestDto result = new ClaimBordxRequestDto();
            ClaimBordxInsertionUnitOfWork uow = new ClaimBordxInsertionUnitOfWork(ClaimBordx);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            //result = ClaimBordx;
            //result.ClaimBordxInsertion = uow.ClaimBordx.ClaimBordxInsertion;
            return uow.Result;
        }

        public string GetNextBordxNumber(int year, int month, Guid insurerId, Guid ReinsurerId, SecurityContext securityContext, AuditContext auditContext)
        {
            GetNextClaimBordxNumberUnitOfWork uow = new GetNextClaimBordxNumberUnitOfWork(year, month, insurerId, ReinsurerId);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Result;
        }

        public string DeleteClaimBordx(Guid ClaimBordxId, SecurityContext securityContext, AuditContext auditContext)
        {
            DeleteClaimBordxUnitOfWork uow = new DeleteClaimBordxUnitOfWork(ClaimBordxId);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Result;
        }

        public object ConfirmedClaimBordxYears(SecurityContext securityContext, AuditContext auditContext)
        {
            ConfirmedClaimBordxYearsUnitOfWork uow = new ConfirmedClaimBordxYearsUnitOfWork();
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Results;

        }

        public object GetConfirmedClaimBordxForGrid(ConfirmedClaimBordxForGridRequestDto ConfirmedClaimBordxForGridRequestDto, SecurityContext securityContext, AuditContext auditContext)
        {
            ConfirmedClaimBordxForGridResponseDto ConfirmedClaimBordxForGridResponseDto = new ConfirmedClaimBordxForGridResponseDto();
            ConfirmedClaimBordxForGridRetrievalUnitOfWork uow = new ConfirmedClaimBordxForGridRetrievalUnitOfWork(ConfirmedClaimBordxForGridRequestDto);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Result;
        }

        public object GetAllClaimBordxByDateForGrid(GetAllClaimBordxByDateRequestDto GetAllClaimBordxByDateRequestDto, SecurityContext securityContext, AuditContext auditContext)
        {
            ClaimBordxRetrievalByDateGirdUnitOfWork uow = new ClaimBordxRetrievalByDateGirdUnitOfWork(GetAllClaimBordxByDateRequestDto);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Result;

        }


        public ClaimBordxsYearsResponseDto GetClaimBordxYears(
            SecurityContext securityContext,
            AuditContext auditContext)
        {
            ClaimBordxsYearsResponseDto result = null;

            ClaimBordxsYearsRetrievalUnitOfWork uow = new ClaimBordxsYearsRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }

            result = uow.Result;

            return result;
        }

        public string ClaimBordxReopen(ClaimBordxReopenRequestDto ClaimBordxReopenRequestDto, SecurityContext securityContext, AuditContext auditContext)
        {
            ClaimBordxReopenUnitOfWork uow = new ClaimBordxReopenUnitOfWork(ClaimBordxReopenRequestDto);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Result;
        }

        public object GetConfirmedClaimBordxYearlybyYear(SecurityContext securityContext, AuditContext auditContext)
        {
            ConfirmedClaimBordxYearlybyYearUnitOfWork uow = new ConfirmedClaimBordxYearlybyYearUnitOfWork();
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Results;

        }

        public object GetAllClaimBordxByYearandCountryForGrid(GetAllClaimBordxByYearandCountryRequestDto GetAllClaimBordxByYearandCountryRequestDto, SecurityContext securityContext, AuditContext auditContext)
        {
            ClaimBordxYearlyRetrievalByYearandCountryGirdUnitOfWork uow = new ClaimBordxYearlyRetrievalByYearandCountryGirdUnitOfWork(GetAllClaimBordxByYearandCountryRequestDto);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Result;

        }

        public string ClaimBordxYealyReopen(ClaimBordxYearlyReopenRequestDto ClaimBordxYearlyReopenRequestDto, SecurityContext securityContext, AuditContext auditContext)
        {
            ClaimBordxYearlyReopenUnitOfWork uow = new ClaimBordxYearlyReopenUnitOfWork(ClaimBordxYearlyReopenRequestDto);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Result;
        }

        public object GetClaimBordxBordxNumbers(SecurityContext securityContext, AuditContext auditContext)
        {
            ClaimBordxBordxNumbersUnitOfWork uow = new ClaimBordxBordxNumbersUnitOfWork();
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Results;

        }

        public ClaimBordxsYearsResponseDto GetClaimBordxYearsForProcess(
            SecurityContext securityContext,
            AuditContext auditContext)
        {
            ClaimBordxsYearsResponseDto result = null;

            ClaimBordxsYearsForProcessRetrievalUnitOfWork uow = new ClaimBordxsYearsForProcessRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }

            result = uow.Result;

            return result;
        }

        public string ClaimBordxYearProcess(int year,Guid Reinsurer,Guid Insurer,  SecurityContext securityContext, AuditContext auditContext)
        {
            ClaimBordxYearProcessUnitOfWork uow = new ClaimBordxYearProcessUnitOfWork(year,Reinsurer,Insurer);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Result;
        }

        public string ClaimBordxYearProcessConfirm(int year, Guid Reinsurer, Guid Insurer, SecurityContext securityContext, AuditContext auditContext)
        {
            ClaimBordxYearProcessConfirmUnitOfWork uow = new ClaimBordxYearProcessConfirmUnitOfWork(year, Reinsurer, Insurer);
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
