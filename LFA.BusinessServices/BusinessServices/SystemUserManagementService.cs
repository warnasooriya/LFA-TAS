using TAS.Services.UnitsOfWork;
using TAS.DataTransfer.Common;
using TAS.DataTransfer.Responses;
using TAS.DataTransfer.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAS.Services.Common.Notification;

namespace TAS.Services.BusinessServices
{
    internal sealed class SystemUserManagementService : ISystemUserManagementService
    {

        public SystemUsersResponseDto GetUsers(
            SecurityContext securityContext,
            AuditContext auditContext)
        {
            SystemUsersResponseDto result = null;

            SystemUsersRetrievalUnitOfWork uow = new SystemUsersRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }

            result = uow.Result;

            return result;
        }

        public SystemUserRequestDto AddUser(SystemUserRequestDto SystemUser,
            SecurityContext securityContext,
            AuditContext auditContext)
        {
            SystemUserRequestDto result = new SystemUserRequestDto();
            SystemUserInsertionUnitOfWork uow = new SystemUserInsertionUnitOfWork(SystemUser);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result.UserInsertion = uow.SystemUser.UserInsertion;
            return result;
        }

        public SystemUserResponseDto GetUserById(string UserId,
            SecurityContext securityContext,
            AuditContext auditContext)
        {
            SystemUserResponseDto result = new SystemUserResponseDto();

            SystemUserRetrievalUnitOfWork uow = new SystemUserRetrievalUnitOfWork();

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

        public SystemUserRequestDto UpdateUser(SystemUserRequestDto User,
            SecurityContext securityContext,
            AuditContext auditContext)
        {
            SystemUserRequestDto result = new SystemUserRequestDto();
            SystemUserUpdationUnitOfWork uow = new SystemUserUpdationUnitOfWork(User);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result.UserInsertion = uow.User.UserInsertion;
            return result;
        }

        public LoginResponseDto AuthUser(LoginRequestDto LoginRequest, SecurityContext securityContext,
            AuditContext auditContext)
        {
            LoginResponseDto result = new LoginResponseDto();
            LoginAuthUnitOfWork uow = new LoginAuthUnitOfWork(LoginRequest);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
                result = uow.Result;
            }
            return result;
        }


        public LoginResponseDto TASAuthUser(LoginRequestDto LoginRequest, SecurityContext securityContext,
    AuditContext auditContext)
        {
            LoginResponseDto result = new LoginResponseDto();
            TASLoginAuthUnitOfWork uow = new TASLoginAuthUnitOfWork(LoginRequest);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
                result = uow.Result;
            }
            return result;
        }


        public MenusResponseDto GetMenu(SecurityContext securityContext, AuditContext auditContext)
        {
            LoadTpaMenuUnitOfWork tmu = new LoadTpaMenuUnitOfWork();
            tmu.SecurityContext = securityContext;
            tmu.AuditContext = auditContext;
            if (tmu.PreExecute())
            {
                tmu.Execute();
                return tmu.Result;
            }
            return null;
        }

        public MenusResponseDto TASGetMenu(SecurityContext securityContext, AuditContext auditContext)
        {
            TASLoadTpaMenuUnitOfWork tmu = new TASLoadTpaMenuUnitOfWork();
            tmu.SecurityContext = securityContext;
            tmu.AuditContext = auditContext;
            if (tmu.PreExecute())
            {
                tmu.Execute();
                return tmu.Result;
            }
            return null;
        }



        public string LoginOut(SecurityContext securityContext,
            AuditContext auditContext)
        {
            string result = string.Empty;
            LogoutUnitOfWork uow = new LogoutUnitOfWork();
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
                result = uow.checkResult;
            }
            return result;
        }

        public bool TASLoginOut(SecurityContext securityContext,
            AuditContext auditContext)
        {
            bool result = false;
            TASLogoutUnitOfWork uow = new TASLogoutUnitOfWork();
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
                result = uow.checkResult;
            }
            return result;
        }


        public SystemUserRequestDto AddTempUserAcc(string TPAID, SystemUserRequestDto SystemUser,
    SecurityContext securityContext,
    AuditContext auditContext)
        {
            SystemUserRequestDto result = new SystemUserRequestDto();
            TempSystemUserInsertionUnitOfWork uow = new TempSystemUserInsertionUnitOfWork(SystemUser);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            uow.tpaid = TPAID;
            if (uow.PreExecute())
            {
                uow.Execute();
            }

            result.UserInsertion = uow.SystemUser.UserInsertion;
            return result;
        }

        public bool KeepAlivePing(SecurityContext securityContext, AuditContext auditContext)
        {
            LoginKeepAliveUnitOfWork uow = new LoginKeepAliveUnitOfWork();
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            bool ret = uow.PreExecute();

            return ret;
        }

        public bool ChangePassword(ChangePasswordRequestDto requestData, SecurityContext securityContext, AuditContext auditContext)
        {

            ChangePasswordUnitOfWork uow = new ChangePasswordUnitOfWork(requestData);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Result;
        }

        public bool RequestNewPassword(ForgotPasswordRequestDto forgotPasswordRequestDto, SecurityContext securityContext, AuditContext auditContext)
        {

            try
            {
                ForgotPasswordUnitOfWork uow = new ForgotPasswordUnitOfWork(forgotPasswordRequestDto);
                if (uow.PreExecute())
                {
                    uow.Execute();
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception)
            {

                return false;
            }
        }

        public string validateChangePassswordLink(ChangePassswordLinkRequestDto requestData, SecurityContext securityContext, AuditContext auditContext)
        {
            try
            {
                ChangePassswordLinkCheckUnitOfWork uow = new ChangePassswordLinkCheckUnitOfWork(requestData);
                if (uow.PreExecute())
                {
                    return uow.Result;
                }
                else
                {
                    return string.Empty;
                }

            }
            catch (Exception)
            {

                return string.Empty;
            }
        }

        public bool ChangeForgotPassword(ChangePasswordForgotRequestDto requestData, SecurityContext securityContext, AuditContext auditContext)
        {
            try
            {
                ChangeForgotPasswordUnotOfWork uow = new ChangeForgotPasswordUnotOfWork(requestData);
                if (uow.PreExecute())
                {
                    uow.Execute();
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception)
            {

                return false;
            }
        }

        public void SendUserRegistrationEmail(string email, string password, string username,string TPAID)
        {
            try
            {
                List<string> toEmailList = new List<string>();
                toEmailList.Add(email);
                new GetMyEmail().UserRegistrationEmail(toEmailList, username, password, Guid.Parse(TPAID));
            }
            catch (Exception)
            {
            }
        }

        public void SendCustomerRegistrationEmail(string email, string password, string TPAID)
        {
            try
            {
                List<string> toEmailList = new List<string>();
                toEmailList.Add(email);
                new GetMyEmail().CustomerRegistrationEmail(toEmailList, email, password, Guid.Parse(TPAID));
            }
            catch (Exception)
            {
            }
        }
    }
}
