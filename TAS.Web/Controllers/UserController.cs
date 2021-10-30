using Newtonsoft.Json.Linq;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.ServiceModel.Channels;
using System.Web;
using System.Web.Http;
using TAS.DataTransfer.Requests;
using TAS.DataTransfer.Responses;
using TAS.Services;
using TAS.Services.Common;
using TAS.Web.Common;
//using LogManager = log4net.LogManager;

namespace TAS.Web.Controllers
{
    public class UserController : ApiController
    {
      //  private static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Logger nlogger = NLog.LogManager.GetCurrentClassLogger();

        #region Priviledge

        [HttpPost]
        public object GetAllPriviledgeLevels()
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            var PriviledgeLevelManagementService = ServiceFactory.GetUserManagementService();

            var PriviledgeLevelData = PriviledgeLevelManagementService.GetPriviledgeLevels(
                SecurityHelper.Context,
                AuditHelper.Context);
            return PriviledgeLevelData.PriviledgeLevels.ToArray();
        }

        #endregion

        #region User

        [HttpPost]
        public object GetAllUsers(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            var userManagementService = ServiceFactory.GetUserManagementService();

            var users = userManagementService.GetUsers(
                SecurityHelper.Context,
                AuditHelper.Context);

            var Data = users.Users
             .Select(a => new
             {
                 a.UserName,
                 a.IsActive,
                 a.Id,
                 a.FirstName,
                 a.LastName,
                 a.DateOfBirth
             }).ToArray();

            var ab = new
            {
                draw = 1,
                recordsTotal = users.Users.Count,
                recordsFiltered = Data.Length,
                data = Data
            };

          //  logger.Info("Queried All Users");
            return ab;
        }

        [HttpPost]
        public object AddUser(JObject data)
        {
          //  var logger = LogManager.GetLogger(typeof (ApiController));
            //logger.Debug("Add user method!");
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            var User = data.ToObject<UserRequestDto>();

            var userManagementService = ServiceFactory.GetUserManagementService();

            var isExistUserName = userManagementService.CheckAlreadyExistUsernameOrEmail(User, SecurityHelper.Context, AuditHelper.Context);


            if (isExistUserName.AlreadyExistEmail || isExistUserName.AlreadyExistUsername)
            {
                return isExistUserName;
            }
            else {
                var resultU = userManagementService.AddUser(User, SecurityHelper.Context, AuditHelper.Context);

                var systemUser = data.ToObject<SystemUserRequestDto>();

                if (User.UserRoleMappings.Count() == 1)
                {
                    systemUser.RoleId = User.UserRoleMappings[0];
                }

                string userTypeCode = "IU"; ;
                if (User.IsDealerAccount) {
                    userTypeCode = "DU";
                }
                systemUser.UserTypeId =
                    userManagementService.GetUserTypes(SecurityHelper.Context, AuditHelper.Context)
                        .UserTypes.Find(t => t.Code == userTypeCode)
                        .Id;
                systemUser.LoginMapId = Guid.Parse(resultU.Id);
                systemUser.LanguageId = (Guid)User.LanguageId;


                var SystemUserManagementService = ServiceFactory.GetSystemUserManagementService();
                var resultS = SystemUserManagementService.AddUser(systemUser, SecurityHelper.Context, AuditHelper.Context);
                // logger.Info("User Added");
                if (resultU.UserInsertion)
                {
                    SystemUserManagementService.SendUserRegistrationEmail(User.Email, systemUser.Password ,User.UserName ,Guid.NewGuid().ToString());
                    return "OK";
                }

                return "Add User Details failed!";
            }

            }


        [HttpPost]
        public string UpdateUser(JObject data)
        {
           // var logger = LogManager.GetLogger(typeof (ApiController));
           // logger.Debug("Add Vehicle Details method!");
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            var User = data.ToObject<UserRequestDto>();

            var UsersManagementService = ServiceFactory.GetUserManagementService();
            var result = UsersManagementService.UpdateUser(User, SecurityHelper.Context, AuditHelper.Context);

            var systemUser = data.ToObject<SystemUserRequestDto>();

            string userTypeCode = "IU"; ;
            if (User.IsDealerAccount)
            {
                userTypeCode = "DU";
            }

            if (User.UserRoleMappings.Count() == 1)
            {
                systemUser.RoleId = User.UserRoleMappings[0];
            }

            systemUser.UserTypeId =
                UsersManagementService.GetUserTypes(SecurityHelper.Context, AuditHelper.Context)
                    .UserTypes.Find(t => t.Code == userTypeCode)
                    .Id;
            systemUser.LoginMapId = Guid.Parse(User.Id);

            var SystemUserManagementService = ServiceFactory.GetSystemUserManagementService();
            var resultS = SystemUserManagementService.UpdateUser(systemUser, SecurityHelper.Context, AuditHelper.Context);

            //logger.Info("Vehicle Details Added");
            if (result.UserInsertion)
            {
                return "OK";
            }
                return "Update User failed!";
            }

        [HttpPost]
        public string UpdateUserProfilePic(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            var userManagementService = ServiceFactory.GetUserManagementService();
            var userId = Guid.Parse(data["Id"].ToString());
            var imageId = Guid.Parse(data["ImageId"].ToString());
            ImageResponseDto Return = null;
            if (userId != null && imageId != null)
            {
                Return = userManagementService.UpdateUserProfilePicture(userId, imageId, SecurityHelper.Context,
                    AuditHelper.Context);
            }
            return Return.DisplayImageSrc;
        }

        [HttpPost]
        public object AddTempUserAcc(JObject data)
        {
            UserRequestDto resultU;
            UserRequestDto User;
            string TPAID;
            var userManagementService = ServiceFactory.GetUserManagementService();

            try
            {
               // var logger = LogManager.GetLogger(typeof (ApiController));
               // logger.Debug("Add user method!");
                //SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                User = data.ToObject<UserRequestDto>();
                TPAID = data["TPAID"].ToString();

                resultU = userManagementService.AddTempUserAcc(TPAID, User, SecurityHelper.Context, AuditHelper.Context);
            }
            catch (Exception ex)
            {
                return "1" + ex.Message;
            }

            try
            {
                if (resultU.Id != Guid.Empty.ToString() && resultU.UserInsertion)
                {
                    var systemUser = new SystemUserRequestDto();
                    systemUser.UserName = User.Email;
                    systemUser.Password = User.Password;
                    systemUser.UserTypeId =
                        userManagementService.GetUserTypesForTempUser(SecurityHelper.Context, AuditHelper.Context, TPAID)
                            .UserTypes.Find(t => t.Code == "CU")
                            .Id;
                    systemUser.LoginMapId = Guid.Parse(resultU.Id);

                    var SystemUserManagementService = ServiceFactory.GetSystemUserManagementService();
                    var resultS = SystemUserManagementService.AddTempUserAcc(TPAID, systemUser, SecurityHelper.Context,
                        AuditHelper.Context);
                    //logger.Info("User Added" + data["Email"]);


                    if (resultU.UserInsertion)
                    {
                        var sysManagementService = ServiceFactory.GetSystemUserManagementService();
                        var loginRequest = new LoginRequestDto();
                        loginRequest.UserName = data["Email"].ToString();
                        loginRequest.Password = data["Password"].ToString();
                        loginRequest.tpaID = TPAID;

                        var result = sysManagementService.AuthUser(loginRequest, SecurityHelper.Context,
                            AuditHelper.Context);
                        if (result.JsonWebToken == null)
                        {
                            if (!result.IsValid)
                            {
                                //logger.Info("Invalid user credentials entered!");
                                return "Invalid";
                            }
                                //logger.Info("Error occured in User Logged in");
                                return "Error";
                            }
                        sysManagementService.SendUserRegistrationEmail(loginRequest.UserName, loginRequest.Password, loginRequest.UserName,
                            TPAID);

                            //string smtpAddress = "smtp.gmail.com";
                            //int portNumber = 587;
                            //bool enableSSL = true;

                            //string emailFrom = "noreply.trivow@gmail.com";
                            //string password = "chatrivow";
                            //string emailTo = data["Email"].ToString();
                            //string subject = "Congratulations! Your registration is successfull!!";
                            //string body = ;

                            //using (MailMessage mail = new MailMessage())
                            //{
                            //    mail.From = new MailAddress(emailFrom);
                            //    mail.To.Add(emailTo);
                            //    mail.Subject = subject;
                            //    mail.Body = body;
                            //    mail.IsBodyHtml = true;
                            //    // Can set to false, if you are sending pure text.

                            //    //mail.Attachments.Add(new Attachment("C:\\SomeFile.txt"));
                            //    //mail.Attachments.Add(new Attachment("C:\\SomeZip.zip"));

                            //    using (SmtpClient smtp = new SmtpClient(smtpAddress, portNumber))
                            //    {
                            //        smtp.Credentials = new NetworkCredential(emailFrom, password);
                            //        smtp.EnableSsl = enableSSL;
                            //        smtp.Send(mail);
                            //    }
                            //}

                            //logger.Info("User Logged in");
                            return result;
                        }
                        return "";
                    }
                    return "EmailExists";
                }
            catch (Exception ex)
            {
                return "2" + ex.Message;
            }
        }

        [HttpPost]
        public string UpdateUserProfiile(JObject data)
        {
            //var logger = LogManager.GetLogger(typeof (ApiController));
            //logger.Debug("Add Vehicle Details method!");
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            var User = data.ToObject<UserRequestDto>();

            var UsersManagementService = ServiceFactory.GetUserManagementService();
            if (User.UserRoleMappings != null && User.UserRoleMappings.Count == 0)
            {
                var temp = UsersManagementService.GetUserById(data["Id"].ToString(),
               SecurityHelper.Context,
               AuditHelper.Context);
                User.UserRoleMappings = temp.UserRoleMappings;
            }

            var result = UsersManagementService.UpdateUser(User, SecurityHelper.Context, AuditHelper.Context);

            var systemUser = data.ToObject<SystemUserRequestDto>();

            systemUser.UserTypeId =
                UsersManagementService.GetUserTypes(SecurityHelper.Context, AuditHelper.Context)
                    .UserTypes.Find(t => t.Code == "IU")
                    .Id;
            systemUser.LoginMapId = Guid.Parse(User.Id);

            var SystemUserManagementService = ServiceFactory.GetSystemUserManagementService();
            var resultS = SystemUserManagementService.AddUser(systemUser, SecurityHelper.Context, AuditHelper.Context);

            //logger.Info("Vehicle Details Added");
            if (result.UserInsertion)
            {
                return "OK";
            }
                return "Add User failed!";
            }

        [HttpPost]
        public object GetUsersById(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            var UserManagementService = ServiceFactory.GetUserManagementService();

            var Users = UserManagementService.GetUserById(data["Id"].ToString(),
                SecurityHelper.Context,
                AuditHelper.Context);
            return Users;
        }

        [HttpPost]
        public object GetUsers()
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            var mn = (string[]) Request.Headers.GetValues("RequestPage");
            var uId = (string[]) Request.Headers.GetValues("RequestUserId");
            if (mn.Length > 0)
            {
                SecurityHelper.Context.MenuName = mn[0];
                if (uId.Length > 0)
                {
                    SecurityHelper.Context.UserId = uId[0];
                }
            }

            var UserManagementService = ServiceFactory.GetUserManagementService();

            var UsersData = UserManagementService.GetUsers(
            SecurityHelper.Context,
            AuditHelper.Context);
            foreach (var item in UsersData.Users)
            {
                item.Id = item.Id.ToLower();
                item.Password = "";
            }
            return UsersData.Users.ToArray();
        }

        [HttpPost]
        public object GetUserByUserName(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            var UserManagementService = ServiceFactory.GetUserManagementService();

            var UsersData = UserManagementService.GetUsers(
            SecurityHelper.Context,
            AuditHelper.Context);
            var user = UsersData.Users.Find(u => u.UserName == data["UserName"].ToString());
            return user;
        }


        [HttpPost]
        public object GetCookieToken(JObject data)
        {
            var loginRequest = data.ToObject<LoginRequestDto>();
            var jwtHelper = new TASJWTHelper(SecurityHelper.Context);
            return jwtHelper.CreateCookieToken(loginRequest.UserName, loginRequest.Password, loginRequest.tpaID);
        }


        [HttpPost]
        public object CookieLogin(JObject data)
        {
            var cookieToken = data["cookieToken"].ToString();
            var jwtHelper = new TASJWTHelper(SecurityHelper.Context);
            var payload = jwtHelper.DecodeCookieToken(cookieToken);
            //JsonSerializer s = new JsonSerializer()
            //{
            //    NullValueHandling = NullValueHandling.Ignore
            //};
            //JObject j = JObject.FromObject(new
            //{
            //    LoginRequestDto =  new
            //    {
            //        name = "mike",
            //        age = 48
            //    } : null
            //}, s);
            var json = @"{UserName: '" + payload["userName"] + "',Password:'" + payload["password"] + "', tpaID:'" +
                       payload["tpaID"] + "'}";
            var a = JObject.Parse(json);
            return LoginAuth(a);
        }

        [HttpPost]
        public object ChangePassword(ChangePasswordRequestDto requestData)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            var userManagementService = ServiceFactory.GetSystemUserManagementService();
            var result = userManagementService.ChangePassword(requestData, SecurityHelper.Context, AuditHelper.Context);
            return result;
        }

        [HttpPost]
        public object ChangeForgotPasssword(ChangePasswordForgotRequestDto requestData)
        {
            var userManagementService = ServiceFactory.GetSystemUserManagementService();
            var result = userManagementService.ChangeForgotPassword(requestData, SecurityHelper.Context,
                AuditHelper.Context);
            return result;
        }

        [HttpPost]
        public object ForgotPassword(ForgotPasswordRequestDto forgotPasswordRequestDto)
        {
            //ForgotPasswordRequestDto forgotPasswordRequestDto = data.ToObject<ForgotPasswordRequestDto>();
            var userManagementService = ServiceFactory.GetSystemUserManagementService();
            var result = userManagementService.RequestNewPassword(forgotPasswordRequestDto, SecurityHelper.Context,
                AuditHelper.Context);
            return result;
        }

        [HttpPost]
        public object validateChangePassswordLink(ChangePassswordLinkRequestDto requestData)
        {
            var userManagementService = ServiceFactory.GetSystemUserManagementService();
            var result = userManagementService.validateChangePassswordLink(requestData, SecurityHelper.Context,
                AuditHelper.Context);
            return result;
        }

        [HttpPost]
        public object GetAllUserForSearchGrid(UserSearchGridRequestDto UserSearchGridRequestDto)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            var userManagementService = ServiceFactory.GetUserManagementService();

            return userManagementService.GetAllUserForSearchGrid(
            UserSearchGridRequestDto,
            SecurityHelper.Context,
            AuditHelper.Context);
        }

        #endregion

        #region User Role

        [HttpPost]
        public object GetUserRoles()
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            var mn = (string[]) Request.Headers.GetValues("RequestPage");
            var uId = (string[]) Request.Headers.GetValues("RequestUserId");
            if (mn.Length > 0)
            {
                SecurityHelper.Context.MenuName = mn[0];
                if (uId.Length > 0)
                {
                    SecurityHelper.Context.UserId = uId[0];
                }
            }


            var UserManagementService = ServiceFactory.GetUserManagementService();

            var UsersData = UserManagementService.GetUserRoles(
            SecurityHelper.Context,
            AuditHelper.Context);
            return UsersData.UserRoles.ToArray();
        }

        [HttpPost]
        public string AddUserRole(JObject data)
        {
            //var logger = LogManager.GetLogger(typeof (ApiController));
            //logger.Debug("Add User Role Details method!");
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            var UserRole = data.ToObject<UserRoleRequestDto>();
            var UserRoleManagementService = ServiceFactory.GetUserManagementService();
            var result = UserRoleManagementService.AddUserRole(UserRole, SecurityHelper.Context, AuditHelper.Context);
            //logger.Info("User Role Details Added");
            if (result.UserRoleInsertion)
            {
                return "OK";
            }
                return "Add User Role Details failed!";
            }

        [HttpPost]
        public string UpdateUserRole(JObject data)
        {
            //var logger = LogManager.GetLogger(typeof (ApiController));
            //logger.Debug("Add Vehicle Details method!");
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            var UserRole = data.ToObject<UserRoleRequestDto>();
            var UserRoleManagementService = ServiceFactory.GetUserManagementService();
            var result = UserRoleManagementService.UpdateUserRole(UserRole, SecurityHelper.Context, AuditHelper.Context);
            //logger.Info("User Role Details Added");
            if (result.UserRoleInsertion)
            {
                return "OK";
            }
                return "Add Vehicle Details failed!";
            }

        [HttpPost]
        public object GetUserRoleById(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            var UserRoleManagementService = ServiceFactory.GetUserManagementService();

            var UserRole = UserRoleManagementService.GetUserRoleById(Guid.Parse(data["Id"].ToString()),
                SecurityHelper.Context,
                AuditHelper.Context);
            return UserRole;
        }

        #endregion

        #region Login

        [HttpPost]
        public object LoginAuth(JObject data)
        {
            var loginRequest = data.ToObject<LoginRequestDto>();
            //ip check
            string requestIp = string.Empty, tpaName = string.Empty;
            bool isIprestricted=true;
            try
            {
                var tpaManagementService = ServiceFactory.GetTASTPAManagementService();
                tpaName = tpaManagementService.GetTPANameById(
                    SecurityHelper.Context,
                    AuditHelper.Context, Guid.Parse(loginRequest.tpaID));

                ITPAManagementService tpaManageServ = ServiceFactory.GetTPAManagementService();
                requestIp = GetClientIp(Request); //"192.168.8.100";//
                isIprestricted = tpaManageServ.checkIsIprestrcted(
                   SecurityHelper.Context,
                   AuditHelper.Context, Guid.Parse(loginRequest.tpaID), requestIp);
            }
            catch (Exception ex)
            {
                nlogger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return "Error";
            }

            nlogger.Trace("Login request recived from ip-" + requestIp +
                " for user - " + loginRequest.UserName +
                " for TPA - " + tpaName);
            //end ip check

            if (isIprestricted)
            {

            var userManagementService = ServiceFactory.GetSystemUserManagementService();
            var result = userManagementService.AuthUser(loginRequest, SecurityHelper.Context, AuditHelper.Context);
            if (result.JsonWebToken == null)
            {
                nlogger.Trace("Login failed for ip-" + requestIp +
                             " for user - " + loginRequest.UserName +
                             " for TPA - " + tpaName);

                if (!result.IsValid)
                {
                    return "Invalid";
                }
                    return "Error";
                }
                nlogger.Trace("Login success for ip-" + requestIp +
                              " for user - " + loginRequest.UserName +
                              " for TPA - " + tpaName +
                              " token - " + result.JsonWebToken);

            return result;
        }
            else {
              return "RestrictedIP";
            }
        }

        private string GetClientIp(HttpRequestMessage request)
        {
            if (request.Properties.ContainsKey("MS_HttpContext"))
            {
                return ((HttpContextWrapper) request.Properties["MS_HttpContext"]).Request.UserHostAddress;
            }

            if (request.Properties.ContainsKey(RemoteEndpointMessageProperty.Name))
            {
                RemoteEndpointMessageProperty prop;
                prop = (RemoteEndpointMessageProperty) request.Properties[RemoteEndpointMessageProperty.Name];
                return prop.Address;
            }

            return null;
        }

        [HttpPost]
        public object TASLoginAuth(JObject data)
        {
            var loginRequest = data.ToObject<LoginRequestDto>();

            var userManagementService = ServiceFactory.GetSystemUserManagementService();
            var result = userManagementService.TASAuthUser(loginRequest, SecurityHelper.Context, AuditHelper.Context);
            if (result.JsonWebToken == null)
            {
                if (!result.IsValid)
                {
                    //logger.Info("Invalid user credentials entered!");
                    return "Invalid";
                }
                    //logger.Info("Error occured in User Logged in");
                    return "Error";
                }
                //logger.Info("User Logged in");
            return result.JsonWebToken;
        }


        [HttpPost]
        public object LoginOut(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            var userManagementService = ServiceFactory.GetSystemUserManagementService();
            var result = userManagementService.LoginOut(SecurityHelper.Context, AuditHelper.Context);
            if (result == "error")
            {
                nlogger.Error("Error Logout!!   jwt :" + Request.Headers.Authorization);
                return result;
            }
            nlogger.Trace("Logout Success!  jwt :" + Request.Headers.Authorization);

            return result;
        }

        [HttpPost]
        public object TASLoginOut(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            var userManagementService = ServiceFactory.GetSystemUserManagementService();
            var result = userManagementService.TASLoginOut(SecurityHelper.Context, AuditHelper.Context);
            if (result == false)
            {
                //logger.Info("Error Logout!!   jwt :" + Request.Headers.Authorization);
                return "Error";
            }
            //logger.Info("Logout Success!  jwt :" + Request.Headers.Authorization);


            return "OK";
        }


        [HttpPost]
        public object KeepAlivePing()
        {
            if (Request.Headers.Authorization != null)
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                var userManagementService = ServiceFactory.GetSystemUserManagementService();

                var result = userManagementService.KeepAlivePing(SecurityHelper.Context, AuditHelper.Context);

                return result;
            }
            return false;
        }

        #endregion

        #region Menu

        [HttpPost]
        public object GetMenu()
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            var Menu = new List<Dictionary<string, object>>();
            try
            {
                var userManagementService = ServiceFactory.GetSystemUserManagementService();
                var Ms = userManagementService.GetMenu(SecurityHelper.Context, AuditHelper.Context);
                var parentlist = Ms.Menus.Where(a => a.ParentMenuId == Guid.Empty).OrderBy(o => o.OrderVal).ToList();
                foreach (var parent in parentlist)
                {
                    Menu.Add(buildMenu(parent, Ms, 0));
                }
                return Menu;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        [HttpPost]
        public object TASGetMenu()
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            var Menu = new List<Dictionary<string, object>>();

            try
            {
                //LoginRequestDto loginRequest = data.ToObject<LoginRequestDto>();

                var userManagementService = ServiceFactory.GetSystemUserManagementService();
                var Ms = userManagementService.TASGetMenu(SecurityHelper.Context, AuditHelper.Context);

                var parentlist = Ms.Menus.Where(a => a.ParentMenuId == Guid.Empty).ToList();
                foreach (var parent in parentlist)
                {
                    Menu.Add(buildMenu(parent, Ms, 0));
                }

                return Menu;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public Dictionary<string, object> buildMenu(MenuResponseDto mrd, MenusResponseDto msrd, int level)
        {
            var Mnuitem = new Dictionary<string, object>();
            var FirstmenuList =
                msrd.Menus.Where(w => w.ParentMenuId != null && w.ParentMenuId == mrd.Id)
                    .OrderBy(m => m.OrderVal)
                    .ToList();
            level = level + 1;
            Mnuitem.Add("MenuName", mrd.MenuName);
            Mnuitem.Add("LinkURL", mrd.LinkURL == "" ? "" : mrd.LinkURL);
            Mnuitem.Add("Icon", mrd.Icon);
            Mnuitem.Add("level", level);
            Mnuitem.Add("MenuCode", mrd.MenuCode);


            if (FirstmenuList.Count > 0)
            {
                var ListSubMenu = new List<Dictionary<string, object>>();
                foreach (var drsub in FirstmenuList)
                {
                    ListSubMenu.Add(buildMenu(drsub, msrd, level));
                }
                Mnuitem.Add("Lastlevel", false);
                Mnuitem.Add("submenu", ListSubMenu);
            }
            else
            {
                Mnuitem.Add("Lastlevel", true);
            }

            return Mnuitem;
        }

        [HttpPost]
        public object GetAllMenus()
        {
            var MenuManagementService = ServiceFactory.GetMenuManagementService();

            var Menu = MenuManagementService.GetMenus(
            SecurityHelper.Context,
            AuditHelper.Context);
            return Menu.Menus.ToArray();
        }

        [HttpPost]
        public string AddOrUpdateDashBoardChartMapping(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            var RMMResponseDto = data["JObject"].ToObject<List<DashboardChartMappingResponseDto>>();
            var UserManagementService = ServiceFactory.GetUserManagementService();

            var saved = UserManagementService.AddOrUpdateDashBoardChartMapping(RMMResponseDto,
                SecurityHelper.Context,
                AuditHelper.Context);

            if (saved)
            {
                return "OK";
            }
            return "Error";
        }


        [HttpPost]
        public string AddOrUpdateRoleMenuMapping(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            var RMMResponseDto = data["JObject"].ToObject<List<RoleMenuLevelMappingResponseDto>>();
            var UserManagementService = ServiceFactory.GetUserManagementService();

            var saved = UserManagementService.AddOrUpdateRoleMenuMapping(RMMResponseDto,
                SecurityHelper.Context,
                AuditHelper.Context);

            if (saved)
            {
                return "OK";
            }
                return "Error";


            //ILog logger = LogManager.GetLogger(typeof(ApiController));
            //logger.Debug("Add RoleMenuMapping method!");
            //IUserManagementService UserManagementService = ServiceFactory.GetUserManagementService();

            //RoleMenuMap RoleMenuMapping = data.ToObject<RoleMenuMap>();
            //RoleMenuMappingsResponseDto InDB = UserManagementService.GetRoleMenuMappings(
            //    SecurityHelper.Context,
            //    AuditHelper.Context);
            //foreach (var item in InDB.RoleMenuMappings)
            //{
            //    if (RoleMenuMapping.RoleMenus.Find(m => m.MenuId == item.MenuId && m.RoleId == item.RoleId && m.LevelId == item.LevelId) == null)
            //    {
            //        item.Id = Guid.Parse("00000000-0000-0000-0000-000000000000");
            //        RoleMenuMappingRequestDto temp = new RoleMenuMappingRequestDto()
            //        {
            //            Id = Guid.Parse("00000000-0000-0000-0000-000000000000"),
            //            LevelId = item.LevelId,
            //            MenuId = item.MenuId,
            //            RoleId = item.RoleId,
            //            Description = item.Description
            //        };
            //        RoleMenuMappingRequestDto result = UserManagementService.UpdateRoleMenuMapping(temp, SecurityHelper.Context, AuditHelper.Context);
            //    }
            //}
            //foreach (var item in RoleMenuMapping.RoleMenus)
            //{
            //    RoleMenuMappingResponseDto exists = InDB.RoleMenuMappings.Find(m => m.MenuId == item.MenuId && m.RoleId == item.RoleId && m.LevelId == item.LevelId);
            //    if (exists != null)
            //    {
            //        item.Id = exists.Id;
            //        RoleMenuMappingRequestDto result = UserManagementService.UpdateRoleMenuMapping(item, SecurityHelper.Context, AuditHelper.Context);
            //    }
            //    else
            //    {
            //        RoleMenuMappingRequestDto result = UserManagementService.AddRoleMenuMapping(item, SecurityHelper.Context, AuditHelper.Context);
            //    }
            //}
            //logger.Info("RoleMenuMapping Added");
            //return "OK";
        }

        [HttpPost]
        public object GetRoleMenuLevelsById(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            var UserManagementService = ServiceFactory.GetUserManagementService();

            var RoleMenuMapping = UserManagementService.GetRoleMenuMappings(
                SecurityHelper.Context,
                AuditHelper.Context).RoleMenuMappings.FindAll(m => m.MenuId == Guid.Parse(data["MenuId"].ToString())
                    && m.RoleId == Guid.Parse(data["RoleId"].ToString()));
            var Levels = new List<Guid>();
            foreach (var item in RoleMenuMapping)
            {
                Levels.Add(item.LevelId);
            }
            return Levels;
        }


        [HttpPost]
        public object GetRoleMenuLevelsByRoleId(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            var UserManagementService = ServiceFactory.GetUserManagementService();

            var RoleId = Guid.Parse(data["RoleId"].ToString());

            var RoleMenuMapping = UserManagementService.GetRoleMenuPrevilagesByRoleId(RoleId,
                SecurityHelper.Context,
                AuditHelper.Context);


            //List<Guid> Levels = new List<Guid>();

            //foreach (var item in RoleMenuMapping)
            //{
            //    Levels.Add(item.LevelId);
            //}
            return RoleMenuMapping;
        }

        [HttpPost]
        public object GetDashBoardMenuLevelsByRoleId(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            var UserManagementService = ServiceFactory.GetUserManagementService();

            var RoleId = Guid.Parse(data["RoleId"].ToString());

            var DashboardChartMapping = UserManagementService.GetDashBoardMenuLevelsByRoleId(RoleId,
            SecurityHelper.Context,
            AuditHelper.Context);


            return DashboardChartMapping;
        }


        public object CheckTyreDealerPermisionsByUserId(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            var UserManagementService = ServiceFactory.GetUserManagementService();

            var Users = UserManagementService.CheckTyreDealerPermisionsByUserId(data["Id"].ToString(),
                SecurityHelper.Context,
                AuditHelper.Context);
            return Users;
        }


        #endregion
    }

    public class RoleMenuMap
    {
        public List<RoleMenuMappingRequestDto> RoleMenus { get; set; }
    }
}