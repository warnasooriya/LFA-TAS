using TAS.DataTransfer.Common;
using TAS.DataTransfer.Requests;
using TAS.DataTransfer.Responses;
using TAS.Services.UnitsOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.BusinessServices
{
    public class TPABranchManagementService : ITPABranchManagementService
    {
        public TPABranchesResponseDto GetTPABranchesByTPAId(
            Guid TPAId,
            SecurityContext securityContext,
            AuditContext auditContext)
        {
            TPABranchesResponseDto result = null;
            TPABranchesRetrievalUnitOfWork uow = new TPABranchesRetrievalUnitOfWork(TPAId);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;

        }

        public TPABranchesResponseDto GetTPABranches(
    SecurityContext securityContext,
    AuditContext auditContext)
        {
            TPABranchesResponseDto result = null;
            TPABranchesRetrievalUnitOfWork uow = new TPABranchesRetrievalUnitOfWork();
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;

        }

        public bool SaveTPABranch(TPABranchRequestDto TPABranchData, SecurityContext securityContext, AuditContext auditContext)
        {
            bool result = false;
            TPABranchInsertionUnitOfWork uow = new TPABranchInsertionUnitOfWork(TPABranchData);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }
        public bool UpdateTPABranch(TPABranchRequestDto TPABranchData, SecurityContext securityContext, AuditContext auditContext)
        {
            bool result = false;
            TPABranchUpdatingUnitOfWork uow = new TPABranchUpdatingUnitOfWork(TPABranchData);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }
        public TPABranchesResponseDto GetTPABranchesBySystemUserId(Guid loggedUserId, SecurityContext securityContext, AuditContext auditContext)
        {
            TPABranchesResponseDto result = null;
            TPABranchesRetrievalBySystemUserIdUnitOfWork uow = new TPABranchesRetrievalBySystemUserIdUnitOfWork(loggedUserId);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }

        public object GetAllTimezones(SecurityContext securityContext,AuditContext auditContext)
        {
            object Response = new object();
            TimeZoneRetrievalUnitOfWork uow = new TimeZoneRetrievalUnitOfWork();
            uow.AuditContext = auditContext;
            uow.SecurityContext = securityContext;
            if (uow.PreExecute())
            {
                uow.Execute();
                Response = uow.Result;
            }
            return Response;
        }


        public object GetTimeZoneById(Guid TimeZoneId, SecurityContext securityContext, AuditContext auditContext)
        {
            object result = null;
            TimeZoneRetrievalUnitOfWork uow = new TimeZoneRetrievalUnitOfWork(TimeZoneId);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;

        }

        public bool IsExsistingTpaByCode(Guid Id, string branchCode,
           SecurityContext securityContext,
           AuditContext auditContext)
        {
            bool Result = false;
            IsExixtingTpaBranchCodeUnitOfWorks uow = new IsExixtingTpaBranchCodeUnitOfWorks();
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            uow.BranchCode = branchCode;
            uow.BranchName = "";
            uow.Id = Id;
            if (uow.PreExecute()) { uow.Execute(); }
            Result = uow.Result;
            return Result;
        }
    }
}
