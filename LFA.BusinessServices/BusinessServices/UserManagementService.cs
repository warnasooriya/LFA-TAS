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
    internal sealed class UserManagementService : IUserManagementService
    {
        #region User
        public UsersResponseDto GetUsers(
            SecurityContext securityContext,
            AuditContext auditContext)
        {
            UsersResponseDto result = null;

            UsersRetrievalUnitOfWork uow = new UsersRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }

            result = uow.Result;

            return result;
        }

        public UserRequestDto AddUser(UserRequestDto User,
            SecurityContext securityContext,
            AuditContext auditContext)
        {
            UserRequestDto result = new UserRequestDto();
            UserInsertionUnitOfWork uow = new UserInsertionUnitOfWork(User);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.User;
            return result;
        }

        public UserRequestDto AddTempUserAcc(string TPAID,UserRequestDto User,
            SecurityContext securityContext,
            AuditContext auditContext)
        {
            UserRequestDto result = new UserRequestDto();
            TempUserInsertionUnitOfWork uow = new TempUserInsertionUnitOfWork(User);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            uow.tpaID = TPAID;
            if (uow.PreExecute())
            {
                uow.Execute();

            }
            result.UserInsertion = uow.User.UserInsertion;
            result.Id = uow.User.Id;
            return result;
        }

        public UserAlreadyExistResponseDto CheckAlreadyExistUsernameOrEmail(UserRequestDto User,
            SecurityContext securityContext,
            AuditContext auditContext){
            UserAlreadyExistResponseDto result = new UserAlreadyExistResponseDto();

            UserAlreadyExistCheckUnitOfWork uow = new UserAlreadyExistCheckUnitOfWork(User);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            uow.User = User;

            if (uow.PreExecute())
            {
                uow.Execute();

            }
            result = uow.Result;
            return result;

        }
        public UserResponseDto GetUserById(string UserId,
            SecurityContext securityContext,
            AuditContext auditContext)
        {
            UserResponseDto result = new UserResponseDto();

            UserRetrievalUnitOfWork uow = new UserRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            uow.UserId = UserId;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }

        public UserRequestDto UpdateUser(UserRequestDto User,
            SecurityContext securityContext,
            AuditContext auditContext)
        {
            UserRequestDto result = new UserRequestDto();
            UserUpdationUnitOfWork uow = new UserUpdationUnitOfWork(User);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result.UserInsertion = uow.User.UserInsertion;
            return result;
        }


        public ImageResponseDto UpdateUserProfilePicture(Guid userId, Guid imageId,
            SecurityContext securityContext,
            AuditContext auditContext)
        {
            ImageResponseDto result = new ImageResponseDto();
            UserProfilePicUpdateUnitOfWork uow = new UserProfilePicUpdateUnitOfWork(userId, imageId);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
                result = uow.Result;
            }
            return result;
        }


        public object GetAllUserForSearchGrid(
            UserSearchGridRequestDto UserSearchGridRequestDto,
            SecurityContext securityContext,
            AuditContext auditContext)
        {
            UserRetrievalForSearchGridUnitOfWork uow = new UserRetrievalForSearchGridUnitOfWork(UserSearchGridRequestDto);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }

            return uow.Result;
        }

        #endregion

        #region User Role
        public UserRolesResponseDto GetUserRoles(SecurityContext securityContext, AuditContext auditContext)
        {
            UserRolesResponseDto result = null;
            UserRolesRetrievalUnitOfWork uow = new UserRolesRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }

            result = uow.Result;

            return result;
        }

        public UserRoleRequestDto AddUserRole(UserRoleRequestDto User,
          SecurityContext securityContext,
          AuditContext auditContext)
        {
            UserRoleRequestDto result = new UserRoleRequestDto();
            UserRoleInsertionUnitOfWork uow = new UserRoleInsertionUnitOfWork(User);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result.UserRoleInsertion = uow.UserRole.UserRoleInsertion;
            return result;
        }

        public UserRoleResponseDto GetUserRoleById(Guid UserRoleId,
            SecurityContext securityContext,
            AuditContext auditContext)
        {
            UserRoleResponseDto result = new UserRoleResponseDto();

            UserRoleRetrievalUnitOfWork uow = new UserRoleRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            uow.UserRoleId = UserRoleId;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }

        public UserRoleRequestDto UpdateUserRole(UserRoleRequestDto User,
            SecurityContext securityContext,
            AuditContext auditContext)
        {
            UserRoleRequestDto result = new UserRoleRequestDto();
            UserRoleUpdationUnitOfWork uow = new UserRoleUpdationUnitOfWork(User);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result.UserRoleInsertion = uow.UserRole.UserRoleInsertion;
            return result;
        }
        #endregion

        #region Role Menu
        public RoleMenuMappingsResponseDto GetRoleMenuMappings(
            SecurityContext securityContext,
            AuditContext auditContext)
        {
            RoleMenuMappingsResponseDto result = null;

            RoleMenuMappingsRetrievalUnitOfWork uow = new RoleMenuMappingsRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }

            result = uow.Result;

            return result;
        }

        public RoleMenuMappingRequestDto AddRoleMenuMapping(RoleMenuMappingRequestDto User,
          SecurityContext securityContext,
          AuditContext auditContext)
        {
            RoleMenuMappingRequestDto result = new RoleMenuMappingRequestDto();
            RoleMenuMappingInsertionUnitOfWork uow = new RoleMenuMappingInsertionUnitOfWork(User);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result.RoleMenuMappingInsertion = uow.RoleMenuMapping.RoleMenuMappingInsertion;
            return result;
        }

        public RoleMenuMappingResponseDto GetRoleMenuMappingById(Guid RoleMenuMappingId,
            SecurityContext securityContext,
            AuditContext auditContext)
        {
            RoleMenuMappingResponseDto result = new RoleMenuMappingResponseDto();

            RoleMenuMappingRetrievalUnitOfWork uow = new RoleMenuMappingRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            uow.RoleMenuMappingId = RoleMenuMappingId;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }

        public RoleMenuMappingRequestDto UpdateRoleMenuMapping(RoleMenuMappingRequestDto User,
            SecurityContext securityContext,
            AuditContext auditContext)
        {
            RoleMenuMappingRequestDto result = new RoleMenuMappingRequestDto();
            RoleMenuMappingUpdationUnitOfWork uow = new RoleMenuMappingUpdationUnitOfWork(User);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result.RoleMenuMappingInsertion = uow.RoleMenuMapping.RoleMenuMappingInsertion;
            return result;
        }
        #endregion

        #region Priviledge Levels
        public PriviledgeLevelsResponseDto GetPriviledgeLevels(
           SecurityContext securityContext,
           AuditContext auditContext)
        {
            PriviledgeLevelsResponseDto result = null;

            PriviledgeLevelsRetrievalUnitOfWork uow = new PriviledgeLevelsRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }

            result = uow.Result;

            return result;
        }

        public PriviledgeLevelRequestDto AddPriviledgeLevel(PriviledgeLevelRequestDto PriviledgeLevel, SecurityContext securityContext,
            AuditContext auditContext)
        {
            PriviledgeLevelRequestDto result = new PriviledgeLevelRequestDto();
            PriviledgeLevelInsertionUnitOfWork uow = new PriviledgeLevelInsertionUnitOfWork(PriviledgeLevel);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result.PriviledgeLevelInsertion = uow.PriviledgeLevel.PriviledgeLevelInsertion;
            return result;
        }


        public PriviledgeLevelResponseDto GetPriviledgeLevelById(Guid PriviledgeLevelId,
    SecurityContext securityContext,
    AuditContext auditContext)
        {
            PriviledgeLevelResponseDto result = new PriviledgeLevelResponseDto();

            PriviledgeLevelRetrievalUnitOfWork uow = new PriviledgeLevelRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            uow.PriviledgeLevelId = PriviledgeLevelId;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }

        public PriviledgeLevelRequestDto UpdatePriviledgeLevel(PriviledgeLevelRequestDto PriviledgeLevel, SecurityContext securityContext,
           AuditContext auditContext)
        {
            PriviledgeLevelRequestDto result = new PriviledgeLevelRequestDto();
            PriviledgeLevelUpdationUnitOfWork uow = new PriviledgeLevelUpdationUnitOfWork(PriviledgeLevel);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result.PriviledgeLevelInsertion = uow.PriviledgeLevel.PriviledgeLevelInsertion;
            return result;
        }
        #endregion

        #region User Type
        public UserTypesResponseDto GetUserTypes(
          SecurityContext securityContext,
          AuditContext auditContext)
        {
            UserTypesResponseDto result = null;

            UserTypesRetrievalUnitOfWork uow = new UserTypesRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }

            result = uow.Result;

            return result;
        }


        public UserTypesResponseDto GetUserTypesForTempUser(
  SecurityContext securityContext,
  AuditContext auditContext,string tpaId)
        {
            UserTypesResponseDto result = null;

            TempUserTypesRetrievalUnitOfWork uow = new TempUserTypesRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            uow.tpaID = tpaId;
            if (uow.PreExecute())
            {
                uow.Execute();
            }

            result = uow.Result;

            return result;
        }
        #endregion



        public System.Collections.Generic.List<RoleMenuLevelMappingResponseDto> GetRoleMenuPrevilagesByRoleId(Guid RoleId, SecurityContext securityContext, AuditContext auditContext)
        {
            RoleMenuLevelMappingResponseDto result = null;

            RoleMenuLevelMappingRetrivalUniteOfWork uow = new RoleMenuLevelMappingRetrivalUniteOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            uow.Roleid = RoleId;
            if (uow.PreExecute())
            {
                uow.Execute();
            }


            return uow.Result;
        }

        public System.Collections.Generic.List<DashboardChartMappingResponseDto> GetDashBoardMenuLevelsByRoleId(Guid RoleId, SecurityContext securityContext, AuditContext auditContext)
        {
            DashboardChartMappingResponseDto result = null;

            DashBoardChartMappingRetrivalUniteOfWork uow = new DashBoardChartMappingRetrivalUniteOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            uow.Roleid = RoleId;
            if (uow.PreExecute())
            {
                uow.Execute();
            }


            return uow.Result;
        }

        public bool AddOrUpdateRoleMenuMapping(System.Collections.Generic.List<RoleMenuLevelMappingResponseDto> RMMResponseDto, SecurityContext securityContext, AuditContext auditContext)
        {


            RoleMenuLevelMappingInsertionUnitOfWork uow = new RoleMenuLevelMappingInsertionUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            uow.insertion = RMMResponseDto;
            if (uow.PreExecute())
            {
                uow.Execute();
            }


            return uow.Result;
        }

        public bool AddOrUpdateDashBoardChartMapping(System.Collections.Generic.List<DashboardChartMappingResponseDto> RMMResponseDto, SecurityContext securityContext, AuditContext auditContext)
        {


            DashBoardChartMappingInsertionUnitOfWork uow = new DashBoardChartMappingInsertionUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            uow.insertion = RMMResponseDto;
            if (uow.PreExecute())
            {
                uow.Execute();
            }


            return uow.Result;
        }


        public DealerUserPermissionCheckingResponseDto CheckTyreDealerPermisionsByUserId(string UserId,
           SecurityContext securityContext,
           AuditContext auditContext)
        {
            DealerUserPermissionCheckingResponseDto result = new DealerUserPermissionCheckingResponseDto();

            DealerUserPermissionCheckingUnitOfWork uow = new DealerUserPermissionCheckingUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            uow.UserId = UserId;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }

    }
}
