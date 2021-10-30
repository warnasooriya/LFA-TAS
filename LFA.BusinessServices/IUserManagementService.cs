using TAS.DataTransfer.Common;
using TAS.DataTransfer.Responses;
using TAS.DataTransfer.Requests;
using System;
using TAS.Services.Entities;
namespace TAS.Services
{
    public interface IUserManagementService
    {
        #region User
        UsersResponseDto GetUsers(
            SecurityContext securityContext,
            AuditContext auditContext);

        UserRequestDto AddUser(UserRequestDto User,
            SecurityContext securityContext,
            AuditContext auditContext);

        UserAlreadyExistResponseDto CheckAlreadyExistUsernameOrEmail(UserRequestDto User,
    SecurityContext securityContext,
    AuditContext auditContext);

        UserResponseDto GetUserById(string UserId,
           SecurityContext securityContext,
           AuditContext auditContext);

        UserRequestDto UpdateUser(UserRequestDto User,
            SecurityContext securityContext,
            AuditContext auditContext);

        UserRequestDto AddTempUserAcc(string TPAID,UserRequestDto User,
            SecurityContext securityContext,
            AuditContext auditContext);

        ImageResponseDto UpdateUserProfilePicture(Guid userId, Guid imageId,
            SecurityContext securityContext,
            AuditContext auditContext);
        #endregion

        #region Role
        UserRolesResponseDto GetUserRoles(
            SecurityContext securityContext,
            AuditContext auditContext);

        UserRoleResponseDto GetUserRoleById(Guid UserRoleId,
            SecurityContext securityContext,
            AuditContext auditContext);

        UserRoleRequestDto UpdateUserRole(UserRoleRequestDto User,
            SecurityContext securityContext,
            AuditContext auditContext);

        UserRoleRequestDto AddUserRole(UserRoleRequestDto User,
          SecurityContext securityContext,
          AuditContext auditContext);
        #endregion

        #region Menu
        RoleMenuMappingsResponseDto GetRoleMenuMappings(
            SecurityContext securityContext,
            AuditContext auditContext);
        RoleMenuMappingRequestDto AddRoleMenuMapping(RoleMenuMappingRequestDto User,
          SecurityContext securityContext,
          AuditContext auditContext);
        RoleMenuMappingResponseDto GetRoleMenuMappingById(Guid RoleMenuMappingId,
            SecurityContext securityContext,
            AuditContext auditContext);
        RoleMenuMappingRequestDto UpdateRoleMenuMapping(RoleMenuMappingRequestDto User,
            SecurityContext securityContext,
            AuditContext auditContext);
        #endregion

        #region Types
        UserTypesResponseDto GetUserTypes(
            SecurityContext securityContext,
            AuditContext auditContext);

        UserTypesResponseDto GetUserTypesForTempUser(
    SecurityContext securityContext,
    AuditContext auditContext, string tpaId);
        #endregion

        #region Priviledge Levels
        PriviledgeLevelsResponseDto GetPriviledgeLevels(
           SecurityContext securityContext,
           AuditContext auditContext);
        PriviledgeLevelRequestDto AddPriviledgeLevel(PriviledgeLevelRequestDto PriviledgeLevel, SecurityContext securityContext,
            AuditContext auditContext);
        PriviledgeLevelResponseDto GetPriviledgeLevelById(Guid PriviledgeLevelId,
    SecurityContext securityContext,
    AuditContext auditContext);
        PriviledgeLevelRequestDto UpdatePriviledgeLevel(PriviledgeLevelRequestDto PriviledgeLevel, SecurityContext securityContext,
           AuditContext auditContext);
        #endregion




        System.Collections.Generic.List<RoleMenuLevelMappingResponseDto> GetRoleMenuPrevilagesByRoleId(Guid RoleId, SecurityContext securityContext, AuditContext auditContext);

        bool AddOrUpdateRoleMenuMapping(System.Collections.Generic.List<RoleMenuLevelMappingResponseDto> RMMResponseDto, SecurityContext securityContext, AuditContext auditContext);

        object GetAllUserForSearchGrid(UserSearchGridRequestDto UserSearchGridRequestDto, SecurityContext securityContext, AuditContext auditContext);

        System.Collections.Generic.List<DashboardChartMappingResponseDto> GetDashBoardMenuLevelsByRoleId(Guid RoleId, SecurityContext securityContext, AuditContext auditContext);

        bool AddOrUpdateDashBoardChartMapping(System.Collections.Generic.List<DashboardChartMappingResponseDto> RMMResponseDto, SecurityContext securityContext, AuditContext auditContext);
        DealerUserPermissionCheckingResponseDto CheckTyreDealerPermisionsByUserId(string UserId, SecurityContext context1, AuditContext context2);
    }
}
