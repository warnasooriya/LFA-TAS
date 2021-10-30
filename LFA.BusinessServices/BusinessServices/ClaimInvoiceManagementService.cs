using System;
using TAS.DataTransfer.Common;
using TAS.DataTransfer.Requests;
using TAS.DataTransfer.Responses;
using TAS.Services.UnitsOfWork;

namespace TAS.Services.BusinessServices
{
    internal sealed class ClaimInvoiceManagementService : IClaimInvoiceManagementService
    {

        public ClaimInvoiceEntryRequestDto AddClaimInvoice(ClaimInvoiceEntryRequestDto ClaimInvoiceEntry,
         SecurityContext securityContext,
         AuditContext auditContext)
        {
            ClaimInvoiceEntryRequestDto result = new ClaimInvoiceEntryRequestDto();
            ClaimInvoiceInsertionUnitOfWork uow = new ClaimInvoiceInsertionUnitOfWork(ClaimInvoiceEntry);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result.ClaimInvoiceEntryInsertion = uow.ClaimInvoiceEntry.ClaimInvoiceEntryInsertion;
            return result;
        }

        public object AddAjusments(Guid claimId, decimal adjustPartAmount, decimal adjustLabourAmount, decimal adjustSundryAmount,
         SecurityContext securityContext,
         AuditContext auditContext)
        {
            object Response = null;
            ClaimAjusmentsInsertionUnitOfWork uow = new ClaimAjusmentsInsertionUnitOfWork(claimId,adjustPartAmount,adjustLabourAmount,adjustSundryAmount);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
                Response = uow.Response;
            }

            return Response;
        }

        public object RetriveInvoiceEntryData(Guid ClaimSubmittedDealerId, SecurityContext securityContext, AuditContext auditContext)
        {
            object response = null;
            ClaimInvoiceDataRetrievalForDashboardUnitOfWork uow = new ClaimInvoiceDataRetrievalForDashboardUnitOfWork(ClaimSubmittedDealerId);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            response = uow.Result;
            return response;
        }
        public object GetAllClaimByDealerId(Guid ClaimSubmittedDealerId, string claimNumber,SecurityContext securityContext, AuditContext auditContext)
        {
            object Response = null;
            ClaimRetriveByClaimSubmittedDealerIdUnitOfWork uow = new ClaimRetriveByClaimSubmittedDealerIdUnitOfWork(ClaimSubmittedDealerId, claimNumber);
            uow.AuditContext = auditContext;
            uow.SecurityContext = securityContext;
            if (uow.PreExecute())
            {
                uow.Execute();
                Response = uow.Result;
            }
            return Response;
        }

        public object GetAllSubmittedInvoiceClaimByDealerId(Guid ClaimSubmittedDealerId, string invoiceNumber, string claimNumber, SecurityContext securityContext, AuditContext auditContext)
        {
            object Response = null;
            ClaimRetriveByInvoiceClaimSubmittedDealerIdUnitOfWork uow = new ClaimRetriveByInvoiceClaimSubmittedDealerIdUnitOfWork(ClaimSubmittedDealerId,invoiceNumber,claimNumber);
            uow.AuditContext = auditContext;
            uow.SecurityContext = securityContext;
            if (uow.PreExecute())
            {
                uow.Execute();
                Response = uow.Result;
            }
            return Response;
        }

        public object GetAllClaimPartDetailsByClaimId(Guid claimId, SecurityContext securityContext, AuditContext auditContext)
        {
            object Response = null;
            ClaimItemsRetriveByClaimIdUnitOfWork uow = new ClaimItemsRetriveByClaimIdUnitOfWork(claimId);
            uow.AuditContext = auditContext;
            uow.SecurityContext = securityContext;
            if (uow.PreExecute())
            {
                uow.Execute();
                Response = uow.Result;
            }
            return Response;
        }

        public object GetAllClaimInvoiceEntryForSearchGrid(
            ClaimInvoiceEntrySearchGridRequestDto ClaimInvoiceEntrySearchGridRequestDto,
            SecurityContext securityContext,
            AuditContext auditContext)
        {
            ClaimInvoiceEntryRetrievalForSearchGridUnitOfWork uow = new ClaimInvoiceEntryRetrievalForSearchGridUnitOfWork(ClaimInvoiceEntrySearchGridRequestDto);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }

            return uow.Result;
        }


        public object GetAllClaimInvoiceEntryClaimForSearchGrid(
            ClaimInvoiceEntryClaimSearchGridRequestDto ClaimInvoiceEntryClaimSearchGridRequestDto,
            SecurityContext securityContext,
            AuditContext auditContext)
        {
            ClaimInvoiceEntryClaimRetrievalForSearchGridUnitOfWork uow = new ClaimInvoiceEntryClaimRetrievalForSearchGridUnitOfWork(ClaimInvoiceEntryClaimSearchGridRequestDto);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }

            return uow.Result;
        }

        public ClaimInvoiceResponseDto GetClaimInvoiceEntryById(Guid ClaimInvoiceEntryId,
            SecurityContext securityContext,
            AuditContext auditContext)
        {
            ClaimInvoiceResponseDto result = new ClaimInvoiceResponseDto();

            ClaimInvoiceEntryRetrievalUnitOfWork uow = new ClaimInvoiceEntryRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            uow.ClaimInvoiceEntryId = ClaimInvoiceEntryId;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }

        public ClaimInvoiceEntryRequestDto UpdateClaimInvoice(ClaimInvoiceEntryRequestDto ClaimInvoice, SecurityContext securityContext, AuditContext auditContext)
        {
            //string result = string.Empty;
            //ClaimInvoiceEntryUpdationUnitOfWork uow = new ClaimInvoiceEntryUpdationUnitOfWork(ClaimInvoice);
            //uow.SecurityContext = securityContext;
            //uow.AuditContext = auditContext;
            //if (uow.PreExecute())
            //{
            //    uow.Execute();
            //    result = uow.Result;
            //}

            //return result;


            ClaimInvoiceEntryRequestDto result = new ClaimInvoiceEntryRequestDto();
            ClaimInvoiceEntryUpdationUnitOfWork uow = new ClaimInvoiceEntryUpdationUnitOfWork(ClaimInvoice);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result.ClaimInvoiceEntryInsertion = uow.ClaimInvoice.ClaimInvoiceEntryInsertion;
            return result;
        }

        public object GetAllClaimBatchGroupById(Guid ClaimBatchGroupId, SecurityContext securityContext, AuditContext auditContext)
        {
            object Response = null;
            ClaimRetriveByClaimBatchGroupIdUnitOfWork uow = new ClaimRetriveByClaimBatchGroupIdUnitOfWork(ClaimBatchGroupId);
            uow.AuditContext = auditContext;
            uow.SecurityContext = securityContext;
            if (uow.PreExecute())
            {
                uow.Execute();
                Response = uow.Result;
            }
            return Response;
        }

        public ClaimInvoiceEntryRequestDto ConfirmClaimInvoice(ClaimInvoiceEntryRequestDto ClaimInvoice, SecurityContext securityContext, AuditContext auditContext)
        {

            ClaimInvoiceEntryRequestDto result = new ClaimInvoiceEntryRequestDto();
            ConfirmClaimInvoiceEntryUnitOfWork uow = new ConfirmClaimInvoiceEntryUnitOfWork(ClaimInvoice);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result.ClaimInvoiceEntryInsertion = uow.ClaimInvoice.ClaimInvoiceEntryInsertion;
            return result;
        }

    }
}
