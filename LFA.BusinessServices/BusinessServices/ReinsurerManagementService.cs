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
    internal sealed class ReinsurerManagementService : IReinsurerManagementService
    {

        public ReinsurersResponseDto GetReinsurers(
            SecurityContext securityContext,
            AuditContext auditContext)
        {
            ReinsurersResponseDto result = null;

            ReinsurersRetrievalUnitOfWork uow = new ReinsurersRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }

            result = uow.Result;

            return result;
        }

        public ReinsurersResponseDto GetParentReinsurers(
            SecurityContext securityContext,
            AuditContext auditContext)
        {
            ReinsurersResponseDto result = null;

            ReinsurersRetrievalUnitOfWork uow = new ReinsurersRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }

            result = uow.Result;

            return result;
        }

        public ReinsurerRequestDto AddReinsurer(
            ReinsurerRequestDto Reinsurer,
            SecurityContext securityContext,
            AuditContext auditContext) {
                ReinsurerRequestDto result = new ReinsurerRequestDto();
                ReinsurerInsertionUnitOfWork uow = new ReinsurerInsertionUnitOfWork(Reinsurer);
                uow.SecurityContext = securityContext;
                uow.AuditContext = auditContext;

                if (uow.PreExecute())
                {
                    uow.Execute();
                }
                result.ReinsurerInsertion = uow.Reinsurer.ReinsurerInsertion;
                return result;
        }

        public ReinsurerResponseDto GetReinsurerById(
            Guid ReinsurerId,
            SecurityContext securityContext,
            AuditContext auditContext)
        {
            ReinsurerResponseDto result = new ReinsurerResponseDto();

            ReinsurerRetrievalUnitOfWork uow = new ReinsurerRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            uow.ReinsurerId = ReinsurerId;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }

        public ReinsurerRequestDto UpdateReinsurer(
            ReinsurerRequestDto Reinsurer,
            SecurityContext securityContext,
            AuditContext auditContext)
        {
            ReinsurerRequestDto result = new ReinsurerRequestDto();
            ReinsurerUpdationUnitOfWork uow = new ReinsurerUpdationUnitOfWork(Reinsurer);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result.ReinsurerInsertion = uow.Reinsurer.ReinsurerInsertion;
            return result;
        }

        public ReinsurerConsortiumsResponseDto GetReinsurerConsortiums(
           SecurityContext securityContext,
           AuditContext auditContext)
        {
            ReinsurerConsortiumsResponseDto result = null;

            ReinsurerConsortiumsRetrievalUnitOfWork uow = new ReinsurerConsortiumsRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }

            result = uow.Result;

            return result;
        }

        public ReinsurerConsortiumRequestDto AddReinsurerConsortium(
            ReinsurerConsortiumRequestDto ReinsurerConsortium,
            SecurityContext securityContext,
            AuditContext auditContext)
        {
            ReinsurerConsortiumRequestDto result = new ReinsurerConsortiumRequestDto();
            ReinsurerConsortiumInsertionUnitOfWork uow = new ReinsurerConsortiumInsertionUnitOfWork(ReinsurerConsortium);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result.ReinsurerConsortiumInsertion = uow.ReinsurerConsortium.ReinsurerConsortiumInsertion;
            return result;
        }

        public ReinsurerConsortiumResponseDto GetReinsurerConsortiumById(
            Guid ReinsurerConsortiumId,
            SecurityContext securityContext,
            AuditContext auditContext)
        {
            ReinsurerConsortiumResponseDto result = new ReinsurerConsortiumResponseDto();

            ReinsurerConsortiumRetrievalUnitOfWork uow = new ReinsurerConsortiumRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            uow.ReinsurerConsortiumId = ReinsurerConsortiumId;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }

        public ReinsurerConsortiumRequestDto UpdateReinsurerConsortium(
            ReinsurerConsortiumRequestDto ReinsurerConsortium,
            SecurityContext securityContext,
            AuditContext auditContext)
        {
            ReinsurerConsortiumRequestDto result = new ReinsurerConsortiumRequestDto();
            ReinsurerConsortiumUpdationUnitOfWork uow = new ReinsurerConsortiumUpdationUnitOfWork(ReinsurerConsortium);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result.ReinsurerConsortiumInsertion = uow.ReinsurerConsortium.ReinsurerConsortiumInsertion;
            return result;
        }


        public UserResponseDto GetUserById(Guid userId, SecurityContext securityContext, AuditContext auditContext)
        {
            UserResponseDto _Response = new UserResponseDto();
            ReinsurerUserRetervalUnitOfWork uow = new ReinsurerUserRetervalUnitOfWork(userId);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            _Response = uow.Result;
            return _Response;

        }

        public object GetAllStaffByReinsurerId(Guid reinsurerId, SecurityContext securityContext, AuditContext auditContext)
        {
            List<Guid> Response = new List<Guid>();
            ReinsurerStaffRetervalByReinsurerIdUnitOfWork uow = new ReinsurerStaffRetervalByReinsurerIdUnitOfWork(reinsurerId);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            Response = uow.Result;
            return Response;
        }

        public bool SaveReinsurerStaff(ReinsurerStaffAddRequestDto data, SecurityContext securityContext, AuditContext auditContext)
        {
            bool Response = false;
            ReinsurerStaffInsertionUnitOfWork uow = new ReinsurerStaffInsertionUnitOfWork(data);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            Response = uow.Result;
            return Response;
        }

        public System.Collections.Generic.List<BordxReportColumnsByReinsureIdResponseDto> GetBordxReportColumnsByReinsureId(Guid ReinsureId, SecurityContext securityContext, AuditContext auditContext)
        {
            BordxReportColumnsResponseDto result = null;

            BordxReportColumnsesRetrievalByReinsureIdUnitOfWork uow = new BordxReportColumnsesRetrievalByReinsureIdUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            uow.ReinsureId = ReinsureId;
            if (uow.PreExecute())
            {
                uow.Execute();
            }


            return uow.Result;
        }

        public bool AddOrUpdateReinsureBordxReportColumns(System.Collections.Generic.List<ReinsureBordxReportColumnsMappingResponseDto> RMMResponseDto, SecurityContext securityContext, AuditContext auditContext)
        {


            ReinsureBordxReportColumnsInsertionUnitOfWork uow = new ReinsureBordxReportColumnsInsertionUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            uow.insertion = RMMResponseDto;
            if (uow.PreExecute())
            {
                uow.Execute();
            }


            return uow.Result;
        }

        //public object GetAllReinsurerBordxByYearandReinsurerIdForGrid(Guid ReinsureId, int BordxYear, SecurityContext securityContext, AuditContext auditContext)
        //{
        //    ReinsurerBordxRetrievalByYearandReinsurerIdyGirdUnitOfWork uow = new ReinsurerBordxRetrievalByYearandReinsurerIdyGirdUnitOfWork(ReinsureId, BordxYear);
        //    uow.SecurityContext = securityContext;
        //    uow.AuditContext = auditContext;
        //    if (uow.PreExecute())
        //    {
        //        uow.Execute();
        //    }
        //    return uow.Result;

        //}

        public System.Collections.Generic.List<ReinsureBordxByReinsureIdandYearResponseDto> GetAllReinsurerBordxByYearandReinsurerIdForGrid(Guid ReinsureId, int BordxYear, SecurityContext securityContext, AuditContext auditContext)
        {
            //BordxReportColumnsResponseDto result = null;

            ReinsurerBordxRetrievalByYearandReinsurerIdyGirdUnitOfWork uow = new ReinsurerBordxRetrievalByYearandReinsurerIdyGirdUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            uow.ReinsureId = ReinsureId;
            uow.BordxYear = BordxYear;
            if (uow.PreExecute())
            {
                uow.Execute();
            }


            return uow.Result;
        }

        public object UserValidationReinsureBordxSubmission(Guid loggedInUserId, SecurityContext securityContext, AuditContext auditContext)
        {
            object Response = null;
            UserValidationReinsurerBordxSubmissionUnitOfWork uow = new UserValidationReinsurerBordxSubmissionUnitOfWork(loggedInUserId);
            uow.AuditContext = auditContext;
            uow.SecurityContext = securityContext;
            if (uow.PreExecute())
            {
                uow.Execute();
                Response = uow.Result;
            }
            return Response;
        }

        bool IReinsurerManagementService.AddorUpdateReinsurerConsortiums(List<ReinsurerConsortiumRequestDto> Reinsurers, SecurityContext securityContext, AuditContext auditContext)
        {
            bool Response = false;
            ReinsurerInsertionOrUpdateUnitOfWork uow = new ReinsurerInsertionOrUpdateUnitOfWork(Reinsurers);
            uow.AuditContext = auditContext;
            uow.SecurityContext = securityContext;
            if (uow.PreExecute())
            {
                uow.Execute();
                Response = uow.Result;
            }
            return Response;
        }
    }
}
