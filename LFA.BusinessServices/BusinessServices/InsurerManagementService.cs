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
    internal sealed class InsurerManagementService : IInsurerManagementService
    {

        public InsurersResponseDto GetInsurers(
            SecurityContext securityContext, 
            AuditContext auditContext)
        {
            InsurersResponseDto result = null;

            InsurersRetrievalUnitOfWork uow = new InsurersRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }

            result = uow.Result;

            return result;
        }

        public InsurersResponseDto GetParentInsurers(
    SecurityContext securityContext,
    AuditContext auditContext)
        {
            InsurersResponseDto result = null;

            InsurersRetrievalUnitOfWork uow = new InsurersRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }

            result = uow.Result;

            return result;
        }

        public InsurerRequestDto AddInsurer(InsurerRequestDto Insurer, SecurityContext securityContext,
            AuditContext auditContext) {
                InsurerRequestDto result = new InsurerRequestDto();
                InsurerInsertionUnitOfWork uow = new InsurerInsertionUnitOfWork(Insurer);
                uow.SecurityContext = securityContext;
                uow.AuditContext = auditContext;

                if (uow.PreExecute())
                {
                    uow.Execute();
                }
                result.InsurerInsertion = uow.Insurer.InsurerInsertion;
                return result;
        }


        public InsurerResponseDto GetInsurerById(Guid InsurerId,
    SecurityContext securityContext,
    AuditContext auditContext)
        {
            InsurerResponseDto result = new InsurerResponseDto();

            InsurerRetrievalUnitOfWork uow = new InsurerRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            uow.InsurerId = InsurerId;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }

        public InsurerRequestDto UpdateInsurer(InsurerRequestDto Insurer, SecurityContext securityContext,
           AuditContext auditContext)
        {
            InsurerRequestDto result = new InsurerRequestDto();
            InsurerUpdationUnitOfWork uow = new InsurerUpdationUnitOfWork(Insurer);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result.InsurerInsertion = uow.Insurer.InsurerInsertion;
            return result;
        }


        public InsurerConsortiumsResponseDto GetInsurerConsortiums(
           SecurityContext securityContext,
           AuditContext auditContext)
        {
            InsurerConsortiumsResponseDto result = null;

            InsurerConsortiumsRetrievalUnitOfWork uow = new InsurerConsortiumsRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }

            result = uow.Result;

            return result;
        }

        public InsurerConsortiumRequestDto AddInsurerConsortium(
            InsurerConsortiumRequestDto InsurerConsortium,
            SecurityContext securityContext,
            AuditContext auditContext)
        {
            InsurerConsortiumRequestDto result = new InsurerConsortiumRequestDto();
            InsurerConsortiumInsertionUnitOfWork uow = new InsurerConsortiumInsertionUnitOfWork(InsurerConsortium);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result.InsurerConsortiumInsertion = uow.InsurerConsortium.InsurerConsortiumInsertion;
            return result;
        }

        public InsurerConsortiumResponseDto GetInsurerConsortiumById(
            Guid InsurerConsortiumId,
            SecurityContext securityContext,
            AuditContext auditContext)
        {
            InsurerConsortiumResponseDto result = new InsurerConsortiumResponseDto();

            InsurerConsortiumRetrievalUnitOfWork uow = new InsurerConsortiumRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            uow.InsurerConsortiumId = InsurerConsortiumId;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }

        public InsurerConsortiumRequestDto UpdateInsurerConsortium(
            InsurerConsortiumRequestDto InsurerConsortium,
            SecurityContext securityContext,
            AuditContext auditContext)
        {
            InsurerConsortiumRequestDto result = new InsurerConsortiumRequestDto();
            InsurerConsortiumUpdationUnitOfWork uow = new InsurerConsortiumUpdationUnitOfWork(InsurerConsortium);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result.InsurerConsortiumInsertion = uow.InsurerConsortium.InsurerConsortiumInsertion;
            return result;
        }
       
    }
}
