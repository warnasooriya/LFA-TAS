using System;
using TAS.DataTransfer.Common;
using TAS.DataTransfer.Requests;
using TAS.DataTransfer.Responses;
using TAS.Services.UnitsOfWork;

namespace TAS.Services.BusinessServices
{
    internal sealed class ReinsurerReceiptManagementService : IReinsurerReceiptManagementService
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

        public ClaimBordxsResponseDto GetBordxsById(Guid ReId, Guid InId, int Year,
            SecurityContext securityContext,
            AuditContext auditContext)
        {
            ClaimBordxsResponseDto result = null;

            ReClaimBordxRetrievalUnitOfWork uow = new ReClaimBordxRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            uow.ReId = ReId;
            uow.InId = InId;
            uow.Year = Year;

            if (uow.PreExecute())
            {
                uow.Execute();
            }

            result = uow.Result;

            return result;
        }

        public BordxPaymentRequestDto AddBordxPayment(BordxPaymentRequestDto BordxPayment,
       SecurityContext securityContext,
       AuditContext auditContext)
        {
            BordxPaymentRequestDto result = new BordxPaymentRequestDto();
            BordxPaymentInsertionUnitOfWork uow = new BordxPaymentInsertionUnitOfWork(BordxPayment);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result.BordxPaymentInsertion = uow.BordxPayment.BordxPaymentInsertion;
            return result;
        }

        public BordxPaymentRequestDto UpdateBordxPayment(BordxPaymentRequestDto BordxPayment,
            SecurityContext securityContext,
           AuditContext auditContext)
        {
            BordxPaymentRequestDto result = new BordxPaymentRequestDto();
            BordxPaymentUpdationUnitOfWork uow = new BordxPaymentUpdationUnitOfWork(BordxPayment);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result.BordxPaymentInsertion = uow.BordxPayment.BordxPaymentInsertion;
            return result;
        }

        public BordxPaymentsResponseDto GetClaimBordxPaymentsById(Guid ClaimBordxId,
              SecurityContext securityContext,
              AuditContext auditContext)
        {
            BordxPaymentsResponseDto result = null;

            BordxPaymentsRetrievalUnitOfWork uow = new BordxPaymentsRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            uow.ClaimBordxId = ClaimBordxId;

            if (uow.PreExecute())
            {
                uow.Execute();
            }

            result = uow.Result;

            return result;
        }
      
    }
}
