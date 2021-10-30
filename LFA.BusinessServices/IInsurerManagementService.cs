using TAS.DataTransfer.Common;
using TAS.DataTransfer.Responses;
using TAS.DataTransfer.Requests;
using System;
using TAS.Services.Entities;
namespace TAS.Services
{
    public interface IInsurerManagementService
    {
        InsurersResponseDto GetInsurers(
            SecurityContext securityContext, 
            AuditContext auditContext);

        InsurerRequestDto AddInsurer(InsurerRequestDto Insurer,
            SecurityContext securityContext,
            AuditContext auditContext);


        InsurerResponseDto GetInsurerById(Guid InsurerId,
            SecurityContext securityContext,
            AuditContext auditContext);

        InsurerRequestDto UpdateInsurer(InsurerRequestDto Insurer,
            SecurityContext securityContext,
            AuditContext auditContext);


        InsurerConsortiumsResponseDto GetInsurerConsortiums(
               SecurityContext securityContext,
               AuditContext auditContext);

        InsurerConsortiumRequestDto AddInsurerConsortium(
            InsurerConsortiumRequestDto InsurerConsortium,
            SecurityContext securityContext,
            AuditContext auditContext);

        InsurerConsortiumResponseDto GetInsurerConsortiumById(
            Guid InsurerConsortiumId,
            SecurityContext securityContext,
            AuditContext auditContext);

        InsurerConsortiumRequestDto UpdateInsurerConsortium(
            InsurerConsortiumRequestDto InsurerConsortium,
            SecurityContext securityContext,
            AuditContext auditContext);
    }
}
