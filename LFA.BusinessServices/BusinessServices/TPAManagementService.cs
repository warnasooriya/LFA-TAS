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
    public class TPAManagementService:ITPAManagementService
    {
        public TPAsResponseDto GetAllTPAs(
           SecurityContext securityContext,
           AuditContext auditContext)
        {
            TPAsResponseDto result = null;
            TPARetrievalUnitOfWork uow = new TPARetrievalUnitOfWork();
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;

        }

        public TPAsResponseDto GetTPADetailsByTPAId(
          SecurityContext securityContext,
          AuditContext auditContext, Guid tpaId)
        {
            TPAsResponseDto result = null;
            TPARetrievalUnitOfWork uow = new TPARetrievalUnitOfWork(tpaId);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;

        }

        public bool SaveTPA(TPARequestDto TPAData,
         SecurityContext securityContext,
         AuditContext auditContext)
        {
            bool result = false;
            TPAInsertionUnitOfWork uow = new TPAInsertionUnitOfWork(TPAData);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }

        public bool UpdateTPA(TPARequestDto TPAData,
         SecurityContext securityContext,
         AuditContext auditContext)
        {
            bool result = false;
            TPAUpdatingUnitOfWork uow = new TPAUpdatingUnitOfWork(TPAData);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }

        public bool checkIsIprestrcted(SecurityContext securityContext, AuditContext auditContext, Guid tpaId, string userIp)
        {
            bool result = false;
            CheckTPAIpRetrievalUnitOfWork uow = new CheckTPAIpRetrievalUnitOfWork(tpaId, userIp);
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
