using TAS.DataTransfer.Common;
using TAS.DataTransfer.Responses;
using TAS.DataTransfer.Requests;
using System;
using TAS.Services.Entities;
using System.Collections.Generic;

namespace TAS.Services
{
    public interface IReinsurerManagementService
    {
        ReinsurersResponseDto GetReinsurers(
            SecurityContext securityContext,
            AuditContext auditContext);

        ReinsurerRequestDto AddReinsurer(ReinsurerRequestDto Reinsurer,
            SecurityContext securityContext,
            AuditContext auditContext);

        ReinsurerResponseDto GetReinsurerById(Guid ReinsurerId,
            SecurityContext securityContext,
            AuditContext auditContext);

        ReinsurerRequestDto UpdateReinsurer(ReinsurerRequestDto Reinsurer,
            SecurityContext securityContext,
            AuditContext auditContext);

        ReinsurerConsortiumsResponseDto GetReinsurerConsortiums(
               SecurityContext securityContext,
               AuditContext auditContext);

        ReinsurerConsortiumRequestDto AddReinsurerConsortium(
            ReinsurerConsortiumRequestDto ReinsurerConsortium,
            SecurityContext securityContext,
            AuditContext auditContext);

        ReinsurerConsortiumResponseDto GetReinsurerConsortiumById(
            Guid ReinsurerConsortiumId,
            SecurityContext securityContext,
            AuditContext auditContext);

        ReinsurerConsortiumRequestDto UpdateReinsurerConsortium(
            ReinsurerConsortiumRequestDto ReinsurerConsortium,
            SecurityContext securityContext,
            AuditContext auditContext);


        bool AddorUpdateReinsurerConsortiums(List<ReinsurerConsortiumRequestDto> Reinsurers,
            SecurityContext securityContext,
            AuditContext auditContext);

        UserResponseDto GetUserById(Guid userId, SecurityContext securityContext, AuditContext auditContext);

        object GetAllStaffByReinsurerId(Guid guid, SecurityContext securityContext, AuditContext auditContext);

        bool SaveReinsurerStaff(ReinsurerStaffAddRequestDto data, SecurityContext securityContext, AuditContext auditContext);

        System.Collections.Generic.List<BordxReportColumnsByReinsureIdResponseDto> GetBordxReportColumnsByReinsureId(Guid ReinsureId, SecurityContext securityContext, AuditContext auditContext);

        bool AddOrUpdateReinsureBordxReportColumns(System.Collections.Generic.List<ReinsureBordxReportColumnsMappingResponseDto> RMMResponseDto, SecurityContext securityContext, AuditContext auditContext);

        //object GetAllReinsurerBordxByYearandReinsurerIdForGrid(GetAllReinsurerBordxByYearandReinsureIdRequestDto GetAllReinsurerBordxByYearandReinsureIdRequestDto, SecurityContext securityContext, AuditContext auditContext);

        System.Collections.Generic.List<ReinsureBordxByReinsureIdandYearResponseDto> GetAllReinsurerBordxByYearandReinsurerIdForGrid(Guid ReinsureId, int BordxYear, SecurityContext securityContext, AuditContext auditContext);

        object UserValidationReinsureBordxSubmission(Guid loggedInUserId, SecurityContext securityContext, AuditContext auditContext);
    }
}
