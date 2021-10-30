using TAS.DataTransfer.Common;
using TAS.DataTransfer.Responses;
using TAS.DataTransfer.Requests;
using System;
using TAS.Services.Entities;
namespace TAS.Services
{
    public interface IReinsurerContractManagementService
    {
        ReinsurerContractsResponseDto GetReinsurerContracts(
            SecurityContext securityContext, 
            AuditContext auditContext);

        ReinsurerContractRequestDto AddReinsurerContract(ReinsurerContractRequestDto ReinsurerContract,
            SecurityContext securityContext,
            AuditContext auditContext);

        ReinsurerContractResponseDto GetReinsurerContractById(Guid ReinsurerContractId,
            SecurityContext securityContext,
            AuditContext auditContext);

        ReinsurerContractRequestDto UpdateReinsurerContract(ReinsurerContractRequestDto ReinsurerContract,
            SecurityContext securityContext,
            AuditContext auditContext);


    }
}
