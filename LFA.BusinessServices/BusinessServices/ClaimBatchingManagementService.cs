using System;
using TAS.DataTransfer.Common;
using TAS.DataTransfer.Requests;
using TAS.DataTransfer.Responses;
using TAS.Services.UnitsOfWork;

namespace TAS.Services.BusinessServices
{
    internal sealed class ClaimBatchingManagementService : IClaimBatchingManagementService
    {
        public string AddClaimBatching(ClaimBatchingRequestDto ClaimBatch,
             SecurityContext securityContext,
             AuditContext auditContext)
        {
            ClaimBatchingInsertionUnitOfWork uow = new ClaimBatchingInsertionUnitOfWork(ClaimBatch);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
           
            return uow.result;
        }
        public string GetNextBatchNumber(ClaimBatchNumberRequestDto claimBatchNumberDetails,
             SecurityContext securityContext,
             AuditContext auditContext)
        {
            GetNextBatchNumberUnitOfWork uow = new GetNextBatchNumberUnitOfWork(claimBatchNumberDetails);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Result;
        }

        public object GetClaimBatchList(ClaimBatchNumberRequestDto claimBatchNumberDetails, 
            SecurityContext securityContext,
             AuditContext auditContext)
        {
            GetBatchNumbersListUnitOfWork uow = new GetBatchNumbersListUnitOfWork(claimBatchNumberDetails);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Result;
        }

        public object SaveClaimBatchGroup(ClaimBatchGroupSaveRequestDto claimBatchSaveRequest, SecurityContext securityContext, AuditContext auditContext)
        {
            SaveClaimBatchGroupUnitOfWork uow = new SaveClaimBatchGroupUnitOfWork(claimBatchSaveRequest);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Result;
        }

        public object GetClaimBatchDetails(ClaimBatchNumberRequestDto claimBatchNumberDetails, SecurityContext securityContext,
             AuditContext auditContext)
        {
            GetLast10BatchesBySearchCriteraUnitOfWork uow = new GetLast10BatchesBySearchCriteraUnitOfWork(claimBatchNumberDetails);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Result;
        }

        public object GetAllAllocatedClaimsByGroupId(Guid claimBatchId, Guid claimBatchGroupId, SecurityContext securityContext, AuditContext auditContext)
        {
            GetAllAllocatedClaimsByGroupIdUnitOfWork uow = new GetAllAllocatedClaimsByGroupIdUnitOfWork(claimBatchId, claimBatchGroupId);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Result;
        }

        public object GetClaimGroupsByBatchId(Guid claimBatchId, SecurityContext securityContext, AuditContext auditContext)
        {
            GetClaimGroupsByBatchIdUnitOfWork uow = new GetClaimGroupsByBatchIdUnitOfWork(claimBatchId);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Result;
        }

     
        public int GetNextClaimGroupNumberById(Guid claimBatchId, SecurityContext securityContext, AuditContext auditContext)
        {
            GetNextClaimGroupNumberByIdUnitOfWork uow = new GetNextClaimGroupNumberByIdUnitOfWork(claimBatchId);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Result;
        }

        public ClaimBatchResponseDto GetAllClaimBatching(SecurityContext securityContext,
            AuditContext auditContext)
        {
            ClaimBatchResponseDto result = null;
            ClaimBatchRetrievalUnitOfWork uow = new ClaimBatchRetrievalUnitOfWork();
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;

        }

        public object GetAllEligibleClaimsByBatchId(Guid claimBatchId, SecurityContext securityContext, AuditContext auditContext)
        {
            GetAllEligibleClaimsByBatchIdUnitOfWork uow = new GetAllEligibleClaimsByBatchIdUnitOfWork(claimBatchId);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }

            return uow.Result;
        }

        public object GetAllClaimBatchingForSearchGrid(
            ClaimBatchingSearchGridRequestDto ClaimBatchingSearchGridRequestDto,
            SecurityContext securityContext,
            AuditContext auditContext)
        {
            ClaimBatchingRetrievalForSearchGridUnitOfWork uow = new ClaimBatchingRetrievalForSearchGridUnitOfWork(ClaimBatchingSearchGridRequestDto);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }

            return uow.Result;
        }

        public ClaimBatchGroupsRespondDto GetAllGroupsById(SecurityContext securityContext, AuditContext auditContext)
        {
            ClaimBatchGroupsRespondDto result = null;

            ClaimBatchGroupRetrievalByGroupIdUnitOfWork uow = new ClaimBatchGroupRetrievalByGroupIdUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }

            result = uow.Result;

            return result;
        }

        public ClaimBatchGroupRequestDto AddClaimBatchGroup(ClaimBatchGroupRequestDto ClaimBatchGroup,
             SecurityContext securityContext,
             AuditContext auditContext)
        {
            ClaimBatchGroupRequestDto result = new ClaimBatchGroupRequestDto();
            ClaimBatchGroupInsertionUnitOfWork uow = new ClaimBatchGroupInsertionUnitOfWork(ClaimBatchGroup);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result.ClaimBatchGroupEntryInsertion = uow.ClaimBatchGroup.ClaimBatchGroupEntryInsertion;
            return result;
        }

        public ClaimsResponseDto GetAllClaimDetailsIsBachingFalse(SecurityContext securityContext,
            AuditContext auditContext)
        {
            ClaimsResponseDto result = null;
            ClaimRetrievalUnitOfWork uow = new ClaimRetrievalUnitOfWork();
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;

        }

        public object GetAllClaimGroupForSearchGrid(
            ClaimBatchGroupSearchGridRequestDto ClaimBatchGroupSearchGridRequestDto,
            SecurityContext securityContext,
            AuditContext auditContext)
        {
            ClaimBatchGroupRetrievalForSearchGridUnitOfWork uow = new ClaimBatchGroupRetrievalForSearchGridUnitOfWork(ClaimBatchGroupSearchGridRequestDto);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }

            return uow.Result;
        }



        public ClaimsResponseDto GetAllClaimDetails(SecurityContext securityContext, AuditContext auditContext)
        {
            ClaimBatchGroupRetrievalForSearchGridV2UnitOfWork uow = new ClaimBatchGroupRetrievalForSearchGridV2UnitOfWork();
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }

            return uow.Result;
        }


        public ClaimsResponseDto GetAllClaimDetailsByGroupID(Guid GroupId, SecurityContext securityContext, AuditContext auditContext)
        {
            ClaimBatchGroupRetrievalForSearchGridV2UnitOfWork uow = new ClaimBatchGroupRetrievalForSearchGridV2UnitOfWork(GroupId);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }

            return uow.Result;
        }


        public ClaimBatchGroupRequestDto UpdateClaimBatchGroup(ClaimBatchGroupRequestDto ClaimBatchGroup, SecurityContext securityContext, AuditContext auditContext)
        {
            ClaimBatchGroupRequestDto result = new ClaimBatchGroupRequestDto();
            ClaimBatchGroupInsertionUnitOfWork uow = new ClaimBatchGroupInsertionUnitOfWork(ClaimBatchGroup);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }

            result.ClaimBatchGroupEntryInsertion = uow.ClaimBatchGroup.ClaimBatchGroupEntryInsertion;
            return result;
        }




        public ClaimBatchingResponseDto GetClaimBatchingById(Guid claimBatchId, SecurityContext securityContext, AuditContext auditContext)
        {
            ClaimBatchGroupRequestDto result = new ClaimBatchGroupRequestDto();
            ClaimBatchByIDRetrievalUnitOfWork uow = new ClaimBatchByIDRetrievalUnitOfWork(claimBatchId);
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
