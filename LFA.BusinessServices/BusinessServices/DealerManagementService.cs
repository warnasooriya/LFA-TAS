using System;
using System.Collections.Generic;
using TAS.DataTransfer.Common;
using TAS.DataTransfer.Requests;
using TAS.DataTransfer.Responses;
using TAS.Services.UnitsOfWork;

namespace TAS.Services.BusinessServices
{
    internal sealed class DealerManagementService : IDealerManagementService
    {
        public DealersRespondDto GetAllDealers(SecurityContext securityContext, AuditContext auditContext)
        {
            DealersRespondDto result = null;
            DealersRetrievalUnitOfWork uow = new DealersRetrievalUnitOfWork();
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }
        public DealersRespondDto GetAllDealersByCountry(Guid countryId , SecurityContext securityContext, AuditContext auditContext)
        {
            DealersRespondDto result = null;
            DealersRetrievalByCountryUnitOfWork uow = new DealersRetrievalByCountryUnitOfWork(countryId);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }



        public DealersRespondDto GetDealers(SecurityContext securityContext, AuditContext auditContext)
        {
            DealersRespondDto result = null;
            DealersRetrievalUnitOfWork uow = new DealersRetrievalUnitOfWork();
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }

        public DealersRespondDto GetParentDealers(SecurityContext securityContext, AuditContext auditContext)
        {
            DealersRespondDto result = null;
            DealersRetrievalUnitOfWork uow = new DealersRetrievalUnitOfWork();
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }

        public string AddDealer(DealerRequestDto Dealer, SecurityContext securityContext, AuditContext auditContext)
        {
            DealerInsertionUnitOfWork uow = new DealerInsertionUnitOfWork(Dealer);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }

            return uow.Result;
        }

        public string AddDealerComment(AddDealerCommentRequestDto DealerComment, SecurityContext securityContext, AuditContext auditContext)
        {
            DealerCommentAddUnitOfWork uow = new DealerCommentAddUnitOfWork(DealerComment);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }

            return uow.Result;
        }

        public DealerRespondDto GetDealerById(Guid DealerId, SecurityContext securityContext, AuditContext auditContext)
        {
            DealerRespondDto result = new DealerRespondDto();
            DealerRetrievalUnitOfWork uow = new DealerRetrievalUnitOfWork();
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            uow.DealerId = DealerId;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }
        public DealersRespondDto GetAllDealersByUserId(Guid UserId, SecurityContext securityContext, AuditContext auditContext)
        {
            DealersRespondDto result = new DealersRespondDto();
            DealerRetrievalByUserIdUnitOfWork uow = new DealerRetrievalByUserIdUnitOfWork(UserId);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }

        public object GetDealerStaffByDealerIdandBranchId(Guid DealerId, Guid BranchId, SecurityContext securityContext, AuditContext auditContext)
        {
            DealerStaffResponseDto result = new DealerStaffResponseDto();
            DealerStaffRetrievalByDealerIdandBranchIdUnitOfWork uow = new DealerStaffRetrievalByDealerIdandBranchIdUnitOfWork(DealerId, BranchId);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Result;
        }
        public string UpdateDealer(DealerRequestDto Dealer, SecurityContext securityContext, AuditContext auditContext)
        {
            DealerRequestDto result = new DealerRequestDto();
            DealerUpdationUnitOfWork uow = new DealerUpdationUnitOfWork(Dealer);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Result;
        }

        public DealerStaffsResponseDto GetDealerStaffs(SecurityContext securityContext, AuditContext auditContext)
        {
            DealerStaffsResponseDto result = null;

            DealerStaffsRetrievalUnitOfWork uow = new DealerStaffsRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }

            result = uow.Result;

            return result;
        }

        public DealerStaffAddResponse AddDealerStaff(List<DealerStaffRequestDto> DealerStaff,List<DealerBranchRequestDto> DealerBranch, SecurityContext securityContext, AuditContext auditContext)
        {
            DealerStaffAddResponse result = new DealerStaffAddResponse();
            DealerStaffInsertionUnitOfWork uow = new DealerStaffInsertionUnitOfWork(DealerStaff, DealerBranch);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            //  result.DealerStaffInsertion = uow.DealerStaff.DealerStaffInsertion;
            return result;
        }

        public DealerStaffResponseDto GetDealerStaffById(Guid DealerStaffId, SecurityContext securityContext, AuditContext auditContext)
        {
            DealerStaffResponseDto result = new DealerStaffResponseDto();

            DealerStaffRetrievalUnitOfWork uow = new DealerStaffRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            uow.DealerStaffId = DealerStaffId;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }

        public DealerStaffRequestDto UpdateDealerStaff(DealerStaffRequestDto DealerStaff, bool Enable, SecurityContext securityContext, AuditContext auditContext)
        {
            DealerStaffRequestDto result = new DealerStaffRequestDto();
            DealerStaffUpdationUnitOfWork uow = new DealerStaffUpdationUnitOfWork(DealerStaff, Enable);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result.DealerStaffInsertion = uow.DealerStaff.DealerStaffInsertion;
            return result;
        }

        public object GetYearsMonthsForDealerInvoices(SecurityContext securityContext, AuditContext auditContext)
        {
            object result = new object();
            DealerInvoicesYearsMonthsRetrivalUnitOfWork uow = new DealerInvoicesYearsMonthsRetrivalUnitOfWork();
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Result;

        }

        public object GetDealersByBordx(Guid bordxId, SecurityContext securityContext, AuditContext auditContext)
        {
            DealersRespondDto result = new DealersRespondDto();
            DealerRetrievalByBordxUnitOfWork uow = new DealerRetrievalByBordxUnitOfWork(bordxId);
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
