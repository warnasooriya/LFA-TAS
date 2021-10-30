using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAS.DataTransfer.Common;
using TAS.DataTransfer.Requests;
using TAS.DataTransfer.Responses;

namespace TAS.Services
{
    public interface IClaimBatchingManagementService
    {

        string AddClaimBatching(ClaimBatchingRequestDto ClaimBatch, SecurityContext securityContext, AuditContext auditContext);

        ClaimBatchResponseDto GetAllClaimBatching(SecurityContext securityContext, AuditContext auditContext);

        object GetAllClaimBatchingForSearchGrid(ClaimBatchingSearchGridRequestDto ClaimBatchingSearchGridRequestDto, SecurityContext securityContext, AuditContext auditContext);



        ClaimBatchGroupsRespondDto GetAllGroupsById(SecurityContext securityContext, AuditContext auditContext);

        ClaimBatchGroupRequestDto AddClaimBatchGroup(ClaimBatchGroupRequestDto ClaimBatchGroup, SecurityContext securityContext, AuditContext auditContext);

        ClaimsResponseDto GetAllClaimDetailsIsBachingFalse(SecurityContext securityContext, AuditContext auditContext);
        ClaimsResponseDto GetAllClaimDetailsByGroupID(Guid GroupId, SecurityContext securityContext, AuditContext auditContext);
        ClaimsResponseDto GetAllClaimDetails(SecurityContext securityContext, AuditContext auditContext);

        ClaimBatchGroupRequestDto UpdateClaimBatchGroup(ClaimBatchGroupRequestDto ClaimBatchGroup, SecurityContext securityContext, AuditContext auditContext);

        ClaimBatchingResponseDto GetClaimBatchingById(Guid claimBatchId, SecurityContext securityContext, AuditContext auditContext);

        object GetAllClaimGroupForSearchGrid(ClaimBatchGroupSearchGridRequestDto ClaimBatchGroupSearchGridRequestDto, SecurityContext securityContext, AuditContext auditContext);
        string GetNextBatchNumber(ClaimBatchNumberRequestDto claimBatchNumberDetails, SecurityContext securityContext, AuditContext auditContext);
        object GetClaimBatchList(ClaimBatchNumberRequestDto claimBatchNumberDetails, SecurityContext securityContext, AuditContext auditContext);
        object GetClaimBatchDetails(ClaimBatchNumberRequestDto claimBatchNumberDetails, SecurityContext securityContext, AuditContext auditContext);
        object GetClaimGroupsByBatchId(Guid claimBatchId, SecurityContext securityContext, AuditContext auditContext);
        int GetNextClaimGroupNumberById(Guid claimBatchId, SecurityContext securityContext, AuditContext auditContext);
        object GetAllEligibleClaimsByBatchId(Guid claimBatchId, SecurityContext securityContext, AuditContext auditContext);
        object GetAllAllocatedClaimsByGroupId(Guid claimBatchId, Guid claimBatchGroupId, SecurityContext securityContext, AuditContext auditContext);
        object SaveClaimBatchGroup(ClaimBatchGroupSaveRequestDto claimBatchSaveRequest, SecurityContext securityContext, AuditContext auditContext);
    }
}
