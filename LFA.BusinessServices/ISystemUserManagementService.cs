using TAS.DataTransfer.Common;
using TAS.DataTransfer.Responses;
using TAS.DataTransfer.Requests;
using System;
using TAS.Services.Entities;
namespace TAS.Services
{
    public interface ISystemUserManagementService
    {
        SystemUsersResponseDto GetUsers(
            SecurityContext securityContext,
            AuditContext auditContext);

        SystemUserRequestDto AddUser(SystemUserRequestDto systemUser,
            SecurityContext securityContext,
            AuditContext auditContext);

        SystemUserResponseDto GetUserById(string UserId,
           SecurityContext securityContext,
           AuditContext auditContext);

        SystemUserRequestDto UpdateUser(SystemUserRequestDto User,
            SecurityContext securityContext,
            AuditContext auditContext);

        LoginResponseDto AuthUser(LoginRequestDto loginRequest,
            SecurityContext securityContext,
            AuditContext auditContext);

        LoginResponseDto TASAuthUser(LoginRequestDto loginRequest,
            SecurityContext securityContext,
            AuditContext auditContext);

        MenusResponseDto GetMenu(SecurityContext securityContext, AuditContext auditContext);

        MenusResponseDto TASGetMenu(SecurityContext securityContext, AuditContext auditContext);

        string LoginOut(SecurityContext securityContext, AuditContext auditContext);

        bool TASLoginOut(SecurityContext securityContext, AuditContext auditContext);

        SystemUserRequestDto AddTempUserAcc(string TPAID, SystemUserRequestDto systemUser, SecurityContext securityContext, AuditContext auditContext);

        bool KeepAlivePing(SecurityContext securityContext, AuditContext auditContext);

        bool ChangePassword(ChangePasswordRequestDto requestData, SecurityContext securityContext, AuditContext auditContext);

        bool RequestNewPassword(ForgotPasswordRequestDto forgotPasswordRequestDto, SecurityContext securityContext, AuditContext auditContext);

        string validateChangePassswordLink(ChangePassswordLinkRequestDto requestData, SecurityContext securityContext, AuditContext auditContext);

        bool ChangeForgotPassword(ChangePasswordForgotRequestDto requestData, SecurityContext securityContext, AuditContext auditContext);

        void SendUserRegistrationEmail(string email, string password, string userName , string TPAID);

        void SendCustomerRegistrationEmail(string email, string password, string TPAID);

    }
}
