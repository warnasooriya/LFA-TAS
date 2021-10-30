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
    internal sealed class ReinsurerContractManagementService : IReinsurerContractManagementService
    {

        public ReinsurerContractsResponseDto GetReinsurerContracts(
            SecurityContext securityContext, 
            AuditContext auditContext)
        {
            ReinsurerContractsResponseDto result = null;

            ReinsurerContractsRetrievalUnitOfWork uow = new ReinsurerContractsRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }

            result = uow.Result;

            return result;
        }

        public ReinsurerContractsResponseDto GetParentReinsurerContracts(
    SecurityContext securityContext,
    AuditContext auditContext)
        {
            ReinsurerContractsResponseDto result = null;

            ReinsurerContractsRetrievalUnitOfWork uow = new ReinsurerContractsRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }

            result = uow.Result;

            return result;
        }

        public ReinsurerContractRequestDto AddReinsurerContract(ReinsurerContractRequestDto ReinsurerContract, SecurityContext securityContext,
            AuditContext auditContext) {
                ReinsurerContractRequestDto result = new ReinsurerContractRequestDto();
                ReinsurerContractInsertionUnitOfWork uow = new ReinsurerContractInsertionUnitOfWork(ReinsurerContract);
                uow.SecurityContext = securityContext;
                uow.AuditContext = auditContext;

                if (uow.PreExecute())
                {
                    uow.Execute();
                }
                result.ReinsurerContractInsertion = uow.ReinsurerContract.ReinsurerContractInsertion;
                return result;
        }


        public ReinsurerContractResponseDto GetReinsurerContractById(Guid ReinsurerContractId,
    SecurityContext securityContext,
    AuditContext auditContext)
        {
            ReinsurerContractResponseDto result = new ReinsurerContractResponseDto();

            ReinsurerContractRetrievalUnitOfWork uow = new ReinsurerContractRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            uow.ReinsurerContractId = ReinsurerContractId;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }

        public ReinsurerContractRequestDto UpdateReinsurerContract(ReinsurerContractRequestDto ReinsurerContract, SecurityContext securityContext,
           AuditContext auditContext)
        {
            ReinsurerContractRequestDto result = new ReinsurerContractRequestDto();
            ReinsurerContractUpdationUnitOfWork uow = new ReinsurerContractUpdationUnitOfWork(ReinsurerContract);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result.ReinsurerContractInsertion = uow.ReinsurerContract.ReinsurerContractInsertion;
            return result;
        }

       
       
    }
}
