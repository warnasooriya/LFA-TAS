using TAS.Services.Entities.Persistence;
using NHibernate;
using TAS.Services.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using System.Text;
using System.Threading.Tasks;
using TAS.DataTransfer.Requests;
using TAS.DataTransfer.Responses;

using NHibernate.Criterion;
using TAS.Services.Common;
using TAS.Services.Common.Transformer;
using System.Web.Script.Serialization;
using System.Linq.Expressions;
using NLog;
using System.Reflection;


namespace TAS.Services.Entities.Management
{
    public class UserEntityManager
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public Guid tmpCustomerId;

        #region User
        public List<UserResponseDto> GetUsers()
        {

            ISession session = EntitySessionManager.GetSession();
            return session.Query<InternalUser>().Select(s => new UserResponseDto
            {
                LastName = s.LastName,
                IsActive = s.IsActive,
                Id = s.Id,
                DateOfBirth = s.DateOfBirth,
                FirstName = s.FirstName,
                Password = s.Password,
                NationalityId = s.NationalityId,
                CountryId = s.CountryId,
                MobileNo = s.MobileNo,
                OtherTelNo = s.OtherTelNo,
                Gender = s.Gender,
                Address1 = s.Address1,
                Address2 = s.Address2,
                Address3 = s.Address3,
                Address4 = s.Address4,
                IDNo = s.IDNo,
                IDTypeId = s.IDTypeId,
                DLIssueDate = s.DLIssueDate,
                EntryDate = s.EntryDate,
                EntryBy = s.EntryBy,
                Email = s.Email,
                ReinsurerId = UserEntityManager.GetReinsuereIdByUserIdStatic(s.Id.ToString())
            }).ToList();

        }

        public UserResponseDto GetUserByUserName(string UserName)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();

                UserResponseDto pDto = new UserResponseDto();

                var query =
                    from User in session.Query<InternalUser>()
                    where User.Email == UserName
                    select new { User = User };

                var result = query.ToList();

                if (result.Count() > 0)
                {

                    var queryR =
                       from User in session.Query<UserRoleMapping>()
                       where User.UserId == Guid.Parse(result.FirstOrDefault().User.Id)
                       select new { User = User };

                    var resultR = queryR.ToList();
                    List<Guid> map = new List<Guid>();
                    foreach (var item in resultR)
                    {
                        map.Add(item.User.RoleId);
                    }


                    if (result != null && result.Count > 0)
                    {
                        pDto.Id = result.First().User.Id;
                        pDto.Address1 = result.First().User.Address1;
                        pDto.Address2 = result.First().User.Address2;
                        pDto.Address3 = result.First().User.Address3;
                        pDto.Address4 = result.First().User.Address4;
                        pDto.CountryId = result.First().User.CountryId;
                        pDto.DateOfBirth = result.First().User.DateOfBirth;
                        pDto.DLIssueDate = result.First().User.DLIssueDate;
                        pDto.Email = result.First().User.Email;
                        pDto.FirstName = result.First().User.FirstName;
                        pDto.Gender = result.First().User.Gender;
                        pDto.IDNo = result.First().User.IDNo;
                        pDto.IDTypeId = result.First().User.IDTypeId;
                        pDto.InternalExtension = result.First().User.InternalExtension;
                        pDto.IsActive = result.First().User.IsActive;
                        pDto.LastName = result.First().User.LastName;
                        pDto.MobileNo = result.First().User.MobileNo;
                        pDto.NationalityId = result.First().User.NationalityId;
                        pDto.OtherTelNo = result.First().User.OtherTelNo;
                        pDto.Password = result.First().User.Password;
                        pDto.ProfilePicture = result.First().User.ProfilePicture;
                        pDto.EntryDate = result.First().User.EntryDate;
                        pDto.EntryBy = result.First().User.EntryBy;
                        pDto.UserRoleMappings = map;

                        pDto.IsUserExists = true;
                        return pDto;
                    }
                    else
                    {
                        pDto.IsUserExists = false;
                        return null;
                    }
                }
                else
                {
                    pDto.IsUserExists = false;
                    return null;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }

        }

        internal DealerUserPermissionCheckingResponseDto CheckTyreDealerPermisionsByUserId(string userId)
        {
            DealerUserPermissionCheckingResponseDto dealerUser = new DealerUserPermissionCheckingResponseDto();
            dealerUser.availableTyreClaimSubmitRole = false;
            dealerUser.availableTyreSalesRole = false;
            ISession session = EntitySessionManager.GetSession();

            List<Menu> menuList = session.Query<Menu>()
                     .Where(a => a.MenuCode == "DealerInvoiceCode" || a.MenuCode == "ClaimSubmission")
                     .Join(session.Query<RoleMenuMapping>(), b => b.Id, c => c.MenuId, (b, c) => new { b, c })
                     .Join(session.Query<UserRoleMapping>(), d => d.c.RoleId, e => e.RoleId, (d, e) => new { d, e })
                     .Where(k=> k.e.UserId.ToString().ToLower() == userId.ToLower() )
                     .Select(g => new Menu
                     {
                         Id=g.d.b.Id,
                         MenuName=g.d.b.MenuName,
                         LinkURL= g.d.b.LinkURL,
                         ParentMenuId= g.d.b.ParentMenuId,
                         Icon=g.d.b.Icon,
                         OrderVal=g.d.b.OrderVal,
                         MenuCode= g.d.b.MenuCode
                     }).Distinct().ToList();

            if (menuList.Count > 0) {
                foreach (Menu menue in menuList)
                {
                    if (menue.MenuCode == "DealerInvoiceCode") {
                        dealerUser.availableTyreSalesRole = true;
                    }
                    if (menue.MenuCode == "ClaimSubmission")
                    {
                        dealerUser.availableTyreClaimSubmitRole = true;
                    }
                }
            }
            return dealerUser;

        }

        internal UserAlreadyExistResponseDto CheckAlreadyExistUsernameOrEmail(UserRequestDto user)
        {
            UserAlreadyExistResponseDto res = new UserAlreadyExistResponseDto();
            res.AlreadyExistUsername = false;
            res.AlreadyExistEmail = false;

            ISession session = EntitySessionManager.GetSession();
            int userNameOccurences = session.Query<SystemUser>()
                               .Where(a => a.UserName.ToLower() == user.UserName.ToLower()).Count();

            int emailOccurences = session.Query<InternalUser>()
                               .Where(a => a.Email.ToLower() == user.Email.ToLower()).Count();

            if (userNameOccurences > 0 )
            {
                res.AlreadyExistUsername = true;
            }

            if (emailOccurences > 0)
            {
                res.AlreadyExistEmail = true;
            }

            return res;
        }

        public UserResponseDto GetUserById(string UserId)
        {
            ISession session = EntitySessionManager.GetSession();
            UserResponseDto _UserResponseDto = new UserResponseDto();

            try
            {
                _UserResponseDto.IsUserExists = false;
                SystemUser SysUser = new SystemUser();
                Guid exactUserId = Guid.Empty;
                var result = Guid.TryParse(UserId, out exactUserId);
                if (!result) return _UserResponseDto;
                try
                {
                    SysUser = session.Query<SystemUser>()
                    .Where(a => a.LoginMapId == exactUserId).FirstOrDefault();
                }
                catch (Exception)
                {

                    SysUser = null;
                }
                if (SysUser == null)
                    return _UserResponseDto;
                UserType UserType = new Entities.UserType();
                session.Load(UserType, SysUser.UserTypeId);

                var userObj = new object();
                if (UserType.Code == "IU" || UserType.Code == "DU" || UserType.Code == "RI")
                {
                    userObj = session.Query<InternalUser>()
                    .Where(a => a.Id == UserId).FirstOrDefault();
                }
                else if (UserType.Code == "CU")
                {
                    userObj = session.Query<Customer>()
                   .Where(a => a.Id == Guid.Parse(UserId)).FirstOrDefault();
                }
                else
                {
                    return _UserResponseDto;
                }
                _UserResponseDto = new UserTransformer().Transform(userObj);
                _UserResponseDto.UserName = SysUser != null ? SysUser.UserName : null;
                _UserResponseDto.UserRoleMappings = session.Query<UserRoleMapping>()
                    .Where(a => a.UserId == Guid.Parse(UserId)).Select(a => a.RoleId).ToList();
                _UserResponseDto.LanguageId = SysUser.LanguageId;
                _UserResponseDto.UserBranches = session.Query<UserBranch>()
                    .Where(a => a.InternalUserId == Guid.Parse(UserId)).Select(a => a.TPABranchId).ToList();
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return _UserResponseDto;
            #region oldcode
            //var queryR =
            //   from User in session.Query<UserRoleMapping>()
            //   where User.UserId == Guid.Parse(UserId)
            //   select new { User = User };

            //var resultR = queryR.ToList();
            //List<Guid> map = new List<Guid>();
            //foreach (var item in resultR)
            //{
            //    map.Add(item.User.RoleId);
            //}


            //if (userObj != null)
            //{
            //    pDto.Id = userObj.Id;
            //    pDto.Address1 = result.First().User.Address1;
            //    pDto.Address2 = result.First().User.Address2;
            //    pDto.Address3 = result.First().User.Address3;
            //    pDto.Address4 = result.First().User.Address4;
            //    pDto.CountryId = result.First().User.CountryId;
            //    pDto.DateOfBirth = result.First().User.DateOfBirth;
            //    pDto.DLIssueDate = result.First().User.DLIssueDate;
            //    pDto.Email = result.First().User.Email;
            //    pDto.FirstName = result.First().User.FirstName;
            //    pDto.Gender = result.First().User.Gender;
            //    pDto.IDNo = result.First().User.IDNo;
            //    pDto.IDTypeId = result.First().User.IDTypeId;
            //    pDto.InternalExtension = result.First().User.InternalExtension;
            //    pDto.IsActive = result.First().User.IsActive;
            //    pDto.LastName = result.First().User.LastName;
            //    pDto.MobileNo = result.First().User.MobileNo;
            //    pDto.NationalityId = result.First().User.NationalityId;
            //    pDto.OtherTelNo = result.First().User.OtherTelNo;
            //    pDto.Password = result.First().User.Password;
            //    pDto.ProfilePicture = result.First().User.ProfilePicture;
            //    pDto.EntryDate = result.First().User.EntryDate;
            //    pDto.EntryBy = result.First().User.EntryBy;
            //    pDto.UserRoleMappings = map;

            //    pDto.IsUserExists = true;
            //    return pDto;
            //}
            //else
            //{
            //    pDto.IsUserExists = false;
            //    return null;
            //}
            #endregion
        }

        internal bool AddUser(UserRequestDto User)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();

                InternalUser su = new Entities.InternalUser();
                su.DateOfBirth = User.DateOfBirth;
                su.EntryBy = "ranga";
                su.EntryDate = DateTime.Now;
                su.FirstName = User.FirstName;
                su.Id = Guid.NewGuid().ToString();
                su.IsActive = User.IsActive;
                su.LastName = User.LastName;
                su.Password = User.Password;
                su.Email = User.Email;
                su.NationalityId = User.NationalityId;
                su.CountryId = User.CountryId;
                su.MobileNo = User.MobileNo;
                su.OtherTelNo = User.OtherTelNo;
                su.InternalExtension = User.InternalExtension;
                su.Gender = User.Gender;
                su.Address1 = User.Address1;
                su.Address2 = User.Address2;
                su.Address3 = User.Address3;
                su.Address4 = User.Address4;
                su.IDNo = User.IDNo;
                su.IDTypeId = User.IDTypeId;
                su.DLIssueDate = User.DLIssueDate;
                su.ProfilePicture = User.ProfilePicture;
                su.IsDealerAccount = User.IsDealerAccount;


                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Save(su, su.Id);
                    User.Id = su.Id;
                    if (User.UserRoleMappings != null)
                    {
                        foreach (var item in User.UserRoleMappings)
                        {
                            UserRoleMapping map = new UserRoleMapping();
                            map.Id = Guid.NewGuid();
                            map.RoleId = item;
                            map.UserId = Guid.Parse(su.Id);
                            session.Save(map, map.Id);
                        }
                    }

                    if (User.UserBranches != null)
                    {
                        foreach (var item in User.UserBranches)
                        {
                            UserBranch map = new UserBranch();
                            map.Id = Guid.NewGuid();
                            map.InternalUserId = Guid.Parse(su.Id);
                            map.TPABranchId = item;
                            session.Save(map, map.Id);
                        }
                    }

                    transaction.Commit();
                }

                return true;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return false;
            }
        }

        internal bool UpdateUser(UserRequestDto User)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();
                InternalUser pr = new Entities.InternalUser();
                session.Load(pr, User.Id);
                pr.Id = User.Id;
                pr.FirstName = User.FirstName;
                pr.LastName = User.LastName;
                pr.Password = User.Password;
                pr.NationalityId = User.NationalityId;
                pr.CountryId = User.CountryId;
                pr.DateOfBirth = User.DateOfBirth;
                pr.MobileNo = User.MobileNo;
                pr.OtherTelNo = User.OtherTelNo;
                pr.InternalExtension = User.InternalExtension;
                pr.Gender = User.Gender;
                pr.Address1 = User.Address1;
                pr.Address2 = User.Address2;
                pr.Address3 = User.Address3;
                pr.Address4 = User.Address4;
                pr.IDNo = User.IDNo;
                pr.IDTypeId = User.IDTypeId;
                pr.DLIssueDate = User.DLIssueDate;
                pr.Email = User.Email;
                pr.IsActive = User.IsActive;
                pr.IsDealerAccount = User.IsDealerAccount;
                // pr.EntryDate = User.EntryDate;
                // pr.EntryBy = User.EntryBy;

                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Update(pr);
                    if (User.UserRoleMappings != null)
                    {
                        var UserRoleMapping = from RoleMenuMappingMapping in session.Query<UserRoleMapping>()
                                              where RoleMenuMappingMapping.UserId == Guid.Parse(pr.Id)
                                              select RoleMenuMappingMapping;

                        if (UserRoleMapping.Count() > 0)
                        {
                            var RoleMenuMappingMappinglist = UserRoleMapping.ToList();
                            foreach (var item in RoleMenuMappingMappinglist)
                            {
                                if (User.UserRoleMappings.Contains(item.RoleId) == false)
                                {
                                    session.Delete(item);
                                }
                            }
                        }

                        foreach (var item in User.UserRoleMappings)
                        {
                            //var query = from RoleMenuMappingMapping in session.Query<UserRoleMapping>()
                            //            where RoleMenuMappingMapping.RoleId == item
                            //            select new { Id = RoleMenuMappingMapping.Id };
                            var query = UserRoleMapping.Where(a => a.RoleId == item).Select(c => new { c.Id });

                            if (query.Count() == 0)
                            {
                                UserRoleMapping cc = new Entities.UserRoleMapping();
                                cc.Id = Guid.NewGuid();
                                cc.RoleId = item;
                                cc.UserId = Guid.Parse(pr.Id);
                                session.Save(cc);
                            }
                        }



                        var UserBranchMap = from UserBranchMapping in session.Query<UserBranch>()
                                            where UserBranchMapping.InternalUserId == Guid.Parse(pr.Id)
                                            select UserBranchMapping;

                        if (UserBranchMap.Count() > 0)
                        {
                            var UserBranchMappinglist = UserBranchMap.ToList();
                            foreach (var item in UserBranchMappinglist)
                            {
                                if (User.UserBranches.Contains(item.TPABranchId) == false)
                                {
                                    session.Delete(item);
                                }
                            }
                        }

                        foreach (var item in User.UserBranches)
                        {
                            var query = UserBranchMap.Where(a => a.TPABranchId == item);

                            if (query.Count() == 0)
                            {
                                UserBranch ub = new Entities.UserBranch();
                                ub.Id = Guid.NewGuid();
                                ub.TPABranchId = item;
                                ub.InternalUserId = Guid.Parse(pr.Id);
                                session.Save(ub);
                            }
                        }

                    }
                    transaction.Commit();
                }

                return true;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return false;
            }
        }

        internal ImageResponseDto UpdateUserProfilePic(Guid userId, Guid imageId)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();
                SystemUser SysUser = new SystemUser();
                SysUser = session.QueryOver<SystemUser>()
                    .Where(a => a.LoginMapId == userId).SingleOrDefault();
                UserType UserType = new Entities.UserType();
                session.Load(UserType, SysUser.UserTypeId);

                if (UserType.Code == "IU" || UserType.Code == "RI")
                {
                    InternalUser user = new Entities.InternalUser();
                    session.Load(user, userId.ToString());
                    user.ProfilePicture = imageId.ToString();
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.Update(user);
                        transaction.Commit();
                    }
                }
                else if (UserType.Code == "CU")
                {
                    Customer user = new Customer();
                    session.Load(user, userId.ToString());
                    //user.ProfilePicture = imageId.ToString();
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.Update(user);
                        transaction.Commit();
                    }
                }
                ImageEntityManager img = new ImageEntityManager();
                return img.GetImageById(imageId);

            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }
        }

        internal bool AddTempUser(UserRequestDto User)
        {
            //try
            //{
            //    ISession session = EntitySessionManager.GetSession();

            //    InternalUser su = new Entities.InternalUser();
            //    su.DateOfBirth = User.DateOfBirth;
            //    su.EntryBy = "ranga";
            //    su.EntryDate = DateTime.Now;
            //    su.FirstName = User.FirstName;
            //    su.UserName = User.UserName;
            //    su.Id = Guid.NewGuid().ToString();
            //    su.IsActive = User.IsActive;
            //    su.LastName = User.LastName;
            //    su.Password = User.Password;
            //    su.Email = User.Email;
            //    su.NationalityId = User.NationalityId;
            //    su.CountryId = User.CountryId;
            //    su.MobileNo = User.MobileNo;
            //    su.OtherTelNo = User.OtherTelNo;
            //    su.InternalExtension = User.InternalExtension;
            //    su.Gender = User.Gender;
            //    su.Address1 = User.Address1;
            //    su.Address2 = User.Address2;
            //    su.Address3 = User.Address3;
            //    su.Address4 = User.Address4;
            //    su.IDNo = User.IDNo;
            //    su.IDTypeId = User.IDTypeId;
            //    su.DLIssueDate = User.DLIssueDate;
            //    using (ITransaction transaction = session.BeginTransaction())
            //    {
            //        session.Save(su);
            //        transaction.Commit();
            //    }

            //    return true;
            //}
            //catch (Exception)
            //{
            //    return false;
            //}

            try
            {
                ISession session = EntitySessionManager.GetSession();
                Customer cu = new Entities.Customer();

                //cu.Id = new Guid().ToString();
                cu.FirstName = "";
                cu.UserName = User.Email;
                cu.Password = User.Password;
                cu.CustomerTypeId = session.Query<CustomerType>().FirstOrDefault(a => a.CustomerTypeName == "Individual").Id;
                cu.UsageTypeId = session.Query<UsageType>().FirstOrDefault(a => a.UsageTypeCode == "PVT").Id;
                cu.Gender = 'M';
                cu.IDNo = "";
                cu.IDTypeId = session.Query<IdType>().FirstOrDefault(a => a.IdTypeName == "NIC").Id;
                cu.EntryDateTime = DateTime.Today.ToUniversalTime();
                cu.EntryUserId = "ba56ec84-1fe0-4385-abe4-182f62caa050";
                cu.IsActive = true;
                //cu.SequanceNumber = session.QueryOver<Customer>().Select(Projections.Max<Customer>(x => x.SequanceNumber))
                //    .SingleOrDefault<int>();

                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Save(cu);
                    transaction.Commit();
                    tmpCustomerId = cu.Id;
                }


                return true;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return false;
            }
        }

        internal bool ChangePassword(string OldPassword, string NewPassword, Guid UserId)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();
                SystemUser SysUser = new SystemUser();
                SysUser = session.QueryOver<SystemUser>()
                    .Where(a => a.LoginMapId == UserId).SingleOrDefault();
                if (SysUser == null)
                {
                    return false;
                }
                if (SysUser.Password != OldPassword)
                {
                    return false;
                }

                SysUser.Password = NewPassword;
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Update(SysUser);
                    transaction.Commit();
                }
                return true;

            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return false;
            }
        }

        internal String ValidateEmail(String email)
        {
            String Response = String.Empty;
            try
            {
                ISession session = EntitySessionManager.GetSession();
                InternalUser Iuser = new Entities.InternalUser();
                var IDbUser = from IUser in session.Query<InternalUser>()
                              where IUser.Email == email
                              select new { IUser = IUser };

                if (IDbUser != null && IDbUser.First().IUser != null && !String.IsNullOrEmpty(IDbUser.First().IUser.Id))
                {
                    Response = IDbUser.First().IUser.Id;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }


        #endregion

        #region User Role
        public List<UserRole> GetUserRoles()
        {
            List<UserRole> entities = null;
            ISession session = EntitySessionManager.GetSession();
            IQueryable<UserRole> userData = session.Query<UserRole>();

            // TODO: get skip/take values as params
            int recordsTotal = userData.Count();
            userData = userData.Skip(0).Take(10);
            entities = userData.ToList();
            return entities;
        }

        public UserRoleResponseDto GetUserRoleById(Guid UserRoleId)
        {
            ISession session = EntitySessionManager.GetSession();
            UserRoleResponseDto pDto = new UserRoleResponseDto();
            UserRole userRole = session.Query<UserRole>().FirstOrDefault(a => a.RoleId == UserRoleId);
            if (userRole != null)
            {
                pDto.RoleId = userRole.RoleId;
                pDto.RoleCode = userRole.RoleCode;
                pDto.RoleName = userRole.RoleName;
                pDto.IsClaimAuthorized = userRole.IsClaimAuthorized;
                pDto.IsGoodWillAuthorized = userRole.IsGoodWillAuthorized;

                pDto.IsUserRoleExists = true;
            }
            else
            {
                pDto.IsUserRoleExists = false;
            }
            return pDto;
        }

        internal bool AddUserRole(UserRoleRequestDto User)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();
                UserRole su = new Entities.UserRole();
                su.RoleId = User.RoleId;
                su.RoleCode = User.RoleCode;
                su.RoleName = User.RoleName;
                su.IsClaimAuthorized = User.IsClaimAuthorized;
                su.IsGoodWillAuthorized = User.IsGoodWillAuthorized;

                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.SaveOrUpdate(su);
                    transaction.Commit();
                }

                return true;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return false;
            }
        }

        internal bool UpdateUserRole(UserRoleRequestDto User)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();
                UserRole pr = new Entities.UserRole();

                pr.RoleId = User.RoleId;
                pr.RoleCode = User.RoleCode;
                pr.RoleName = User.RoleName;
                pr.IsClaimAuthorized = User.IsClaimAuthorized;
                pr.IsGoodWillAuthorized = User.IsGoodWillAuthorized;

                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Update(pr);
                    transaction.Commit();
                }

                return true;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return false;
            }
        }


        #endregion

        #region User Menu
        public List<RoleMenuMapping> GetRoleMenuMappings()
        {
            List<RoleMenuMapping> entities = null;
            ISession session = EntitySessionManager.GetSession();
            IQueryable<RoleMenuMapping> userData = session.Query<RoleMenuMapping>();

            // TODO: get skip/take values as params
            int recordsTotal = userData.Count();
            userData = userData.Skip(0).Take(10);
            entities = userData.ToList();
            return entities;
        }

        public RoleMenuMappingResponseDto GetRoleMenuMappingById(Guid RoleMenuMappingId)
        {
            ISession session = EntitySessionManager.GetSession();

            RoleMenuMappingResponseDto pDto = new RoleMenuMappingResponseDto();

            var query =
                from User in session.Query<RoleMenuMapping>()
                where User.RoleId == RoleMenuMappingId
                select new { User = User };

            var result = query.ToList();


            if (result != null && result.Count > 0)
            {
                pDto.RoleId = result.First().User.RoleId;
                pDto.LevelId = result.First().User.LevelId;
                pDto.MenuId = result.First().User.MenuId;
                pDto.Description = result.First().User.Description;
                pDto.Id = result.First().User.Id;


                pDto.IsRoleMenuMappingExists = true;
                return pDto;
            }
            else
            {
                pDto.IsRoleMenuMappingExists = false;
                return null;
            }
        }

        internal bool AddRoleMenuMapping(RoleMenuMappingRequestDto User)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();
                RoleMenuMapping su = new Entities.RoleMenuMapping();
                //su.Id = Guid.NewGuid();
                su.RoleId = User.RoleId;
                su.MenuId = User.MenuId;
                su.LevelId = User.LevelId;
                su.Description = User.Description;
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.SaveOrUpdate(su);
                    transaction.Commit();
                }

                return true;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return false;
            }
        }

        internal bool UpdateRoleMenuMapping(RoleMenuMappingRequestDto User)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();

                if (User.Id.ToString() == "00000000-0000-0000-0000-000000000000")
                {
                    var query =
                            from x in session.Query<RoleMenuMapping>()
                            where x.LevelId == User.LevelId
                            select new { x = x };

                    var result = query.ToList();
                    RoleMenuMapping pr = result.First().x;
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.Delete(pr);
                        transaction.Commit();
                    }
                }
                else
                {
                    RoleMenuMapping pr = new Entities.RoleMenuMapping();

                    pr.RoleId = User.RoleId;
                    pr.LevelId = User.LevelId;
                    pr.MenuId = User.MenuId;
                    pr.Description = User.Description;
                    pr.Id = User.Id;
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.Update(pr);
                        transaction.Commit();
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return false;
            }
        }
        #endregion

        #region PriviledgeLevel
        public List<PriviledgeLevel> GetPriviledgeLevels()
        {
            List<PriviledgeLevel> entities = null;
            ISession session = EntitySessionManager.GetSession();
            IQueryable<PriviledgeLevel> PriviledgeLevelData = session.Query<PriviledgeLevel>();
            entities = PriviledgeLevelData.ToList();
            return entities;
        }

        public PriviledgeLevelResponseDto GetPriviledgeLevelById(Guid PriviledgeLevelId)
        {
            ISession session = EntitySessionManager.GetSession();

            PriviledgeLevelResponseDto pDto = new PriviledgeLevelResponseDto();

            var query =
                from PriviledgeLevel in session.Query<PriviledgeLevel>()
                where PriviledgeLevel.Id == PriviledgeLevelId
                select new { PriviledgeLevel = PriviledgeLevel };

            var result = query.ToList();


            if (result != null && result.Count > 0)
            {
                pDto.Id = result.First().PriviledgeLevel.Id;
                pDto.LevelDescription = result.First().PriviledgeLevel.LevelDescription;
                pDto.LevelName = result.First().PriviledgeLevel.LevelName;

                pDto.IsPriviledgeLevelExists = true;
                return pDto;
            }
            else
            {
                pDto.IsPriviledgeLevelExists = false;
                return null;
            }
        }

        internal bool AddPriviledgeLevel(PriviledgeLevelRequestDto PriviledgeLevel)
        {
            try
            {

                ISession session = EntitySessionManager.GetSession();
                PriviledgeLevel pr = new Entities.PriviledgeLevel();

                pr.Id = new Guid();
                pr.LevelName = PriviledgeLevel.LevelName;
                pr.LevelDescription = PriviledgeLevel.LevelDescription;

                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.SaveOrUpdate(pr);
                    transaction.Commit();
                }

                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        internal bool UpdatePriviledgeLevel(PriviledgeLevelRequestDto PriviledgeLevel)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();
                PriviledgeLevel pr = new Entities.PriviledgeLevel();

                pr.Id = PriviledgeLevel.Id;
                pr.LevelName = PriviledgeLevel.LevelName;
                pr.LevelDescription = PriviledgeLevel.LevelDescription;

                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Update(pr);

                    transaction.Commit();
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion

        #region User Type
        public List<UserTypeResponseDto> GetUserTypes()
        {
            ISession session = EntitySessionManager.GetSession();
            return session.Query<UserType>().Select(User=> new UserTypeResponseDto {
                Code = User.Code,
                Description = User.Description,
                Id = User.Id
            }).ToList();
        }
        #endregion



        internal static List<RoleMenuLevelMappingResponseDto> GetRoleMenuLevelMappingResponseDto(Guid Roleid)
        {
            ISession session = EntitySessionManager.GetSession();

            Guid ReadLevelId = session.Query<PriviledgeLevel>().FirstOrDefault(a => a.LevelName == "Read").Id;
            Guid CreateLevelId = session.Query<PriviledgeLevel>().FirstOrDefault(a => a.LevelName == "Create").Id;
            Guid UpdateLevelId = session.Query<PriviledgeLevel>().FirstOrDefault(a => a.LevelName == "Update").Id;
            Guid DeleteLevelId = session.Query<PriviledgeLevel>().FirstOrDefault(a => a.LevelName == "Delete").Id;


            var arr = from r in session.Query<UserRole>()
                      from m in session.Query<Menu>()
                      where r.RoleId == Roleid
                      select new
                      {
                          r.RoleId,
                          r.RoleCode,
                          MenuId = m.Id,
                          Menu = m.MenuName,
                          Read = session.Query<RoleMenuMapping>().Count(rmm => rmm.RoleId == r.RoleId && rmm.MenuId == m.Id && rmm.LevelId == ReadLevelId) > 0 ? true : false,
                          //Read = (from rmm in session.Query<RoleMenuMapping>() where rmm.RoleId == r.RoleId && rmm.MenuId == m.Id && rmm.LevelId == ReadLevelId).Count() > 0 ? true : false,
                          Create = session.Query<RoleMenuMapping>().Count(rmm => rmm.RoleId == r.RoleId && rmm.MenuId == m.Id && rmm.LevelId == CreateLevelId) > 0 ? true : false,
                          //Create = (from rmm in session.Query<RoleMenuMapping>() where rmm.RoleId == r.RoleId && rmm.MenuId == m.Id && rmm.LevelId == CreateLevelId).Count() > 0 ? true : false,
                          Update = session.Query<RoleMenuMapping>().Count(rmm => rmm.RoleId == r.RoleId && rmm.MenuId == m.Id && rmm.LevelId == UpdateLevelId) > 0 ? true : false,
                          //Update = (from rmm in session.Query<RoleMenuMapping>() where rmm.RoleId == r.RoleId && rmm.MenuId == m.Id && rmm.LevelId == UpdateLevelId).Count() > 0 ? true : false,
                          Delete = session.Query<RoleMenuMapping>().Count(rmm => rmm.RoleId == r.RoleId && rmm.MenuId == m.Id && rmm.LevelId == DeleteLevelId) > 0 ? true : false,
                          //Delete = (from rmm in session.Query<RoleMenuMapping>() where rmm.RoleId == r.RoleId && rmm.MenuId == m.Id && rmm.LevelId == DeleteLevelId).Count() > 0 ? true : false
                      };

            List<RoleMenuLevelMappingResponseDto> rmlmrList = new List<RoleMenuLevelMappingResponseDto>();

            RoleMenuLevelMappingResponseDto rmlmr;
            foreach (var item in arr)
            {
                rmlmr = new RoleMenuLevelMappingResponseDto();

                rmlmr.MenuId = item.MenuId;
                rmlmr.RoleId = item.RoleId;
                rmlmr.Menu = item.Menu;
                rmlmr.RoleCode = item.RoleCode;
                rmlmr.Read = item.Read;
                rmlmr.Create = item.Create;
                rmlmr.Update = item.Update;
                rmlmr.Delete = item.Delete;

                rmlmrList.Add(rmlmr);
            }

            return rmlmrList;
        }

        internal static bool AddOrUpdateRoleMenuMapping(List<RoleMenuLevelMappingResponseDto> insertion)
        {
            ISession session = EntitySessionManager.GetSession();

            using (ITransaction transaction = session.BeginTransaction())
            {

                try
                {

                    Guid ReadLevelId = session.Query<PriviledgeLevel>().FirstOrDefault(a => a.LevelName == "Read").Id;
                    Guid CreateLevelId = session.Query<PriviledgeLevel>().FirstOrDefault(a => a.LevelName == "Create").Id;
                    Guid UpdateLevelId = session.Query<PriviledgeLevel>().FirstOrDefault(a => a.LevelName == "Update").Id;
                    Guid DeleteLevelId = session.Query<PriviledgeLevel>().FirstOrDefault(a => a.LevelName == "Delete").Id;

                    var levels = session.Query<RoleMenuMapping>().Where(a => a.RoleId == insertion.First().RoleId);

                    foreach (var level in levels)
                    {
                        session.Delete(level);
                    }


                    RoleMenuMapping rmm;
                    foreach (var item in insertion)
                    {
                        if (item.Read)
                        {
                            rmm = new Entities.RoleMenuMapping();
                            rmm.Id = Guid.NewGuid();
                            rmm.RoleId = item.RoleId;
                            rmm.MenuId = item.MenuId;
                            rmm.LevelId = ReadLevelId;
                            session.Save(rmm);
                        }

                        if (item.Create)
                        {
                            rmm = new Entities.RoleMenuMapping();
                            rmm.Id = Guid.NewGuid();
                            rmm.RoleId = item.RoleId;
                            rmm.MenuId = item.MenuId;
                            rmm.LevelId = CreateLevelId;
                            session.Save(rmm);
                        }

                        if (item.Update)
                        {
                            rmm = new Entities.RoleMenuMapping();
                            rmm.Id = Guid.NewGuid();
                            rmm.RoleId = item.RoleId;
                            rmm.MenuId = item.MenuId;
                            rmm.LevelId = UpdateLevelId;
                            session.Save(rmm);
                        }

                        if (item.Delete)
                        {
                            rmm = new Entities.RoleMenuMapping();
                            rmm.Id = Guid.NewGuid();
                            rmm.RoleId = item.RoleId;
                            rmm.MenuId = item.MenuId;
                            rmm.LevelId = DeleteLevelId;
                            session.Save(rmm);
                        }

                    }

                    transaction.Commit();
                    return true;

                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                    return false;
                }
            }

        }



        internal static DealerTypeResponseDto DealerAccessRetrivalByUserId(Guid UserId)
        {
            DealerTypeResponseDto dealerTypeResponseDto = new DealerTypeResponseDto();
           ISession session = EntitySessionManager.GetSession();
            SystemUser su = session.Query<SystemUser>().Where(a => a.LoginMapId == UserId).FirstOrDefault();
            if (su == null) return null;
            UserType ut = session.Query<UserType>().Where(a => a.Id == su.UserTypeId).FirstOrDefault();
            if (ut == null) return null;

            String userTypeCode = ut.Code;
            if (userTypeCode.ToUpper() != "IU" && userTypeCode.ToUpper() != "DU") return null;

            if (userTypeCode.ToUpper() == "DU")
            {
                //checking for staff mapping
                List<DealerStaff> DealerStaffList = session.Query<DealerStaff>().Where(a => a.UserId == UserId).ToList();
                if (DealerStaffList.Count() < 1)
                {

                    dealerTypeResponseDto.dealerType = "NoMapping";
                }
                else
                {
                    dealerTypeResponseDto.dealerType = "Dealer";
                }
            }
            else
            {
                dealerTypeResponseDto.dealerType = "Internal";

            }
            return dealerTypeResponseDto;

        }

            public static Guid GetReinsuereIdByUserIdStatic(string userId)
        {
            Guid response = Guid.Empty;
            try
            {
                Guid UserId = Guid.Parse(userId);
                ISession session = EntitySessionManager.GetSession();
                SystemUser su = session.Query<SystemUser>().Where(a => a.LoginMapId == UserId).FirstOrDefault();
                if (su == null) return Guid.Empty;
                UserType ut = session.Query<UserType>().Where(a => a.Id == su.UserTypeId).FirstOrDefault();
                if (ut == null) return Guid.Empty;

                if (ut.Code.ToUpper() == "RI")
                {
                    var reinsuereUser = session.Query<ReinsurerUser>().Where(a => a.InternalUserId == UserId).FirstOrDefault();
                    if (reinsuereUser != null)
                    {
                        response = reinsuereUser.ReinsurerId;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }

            return response;
        }
        public Guid GetReinsuereIdByUserId(string userId)
        {
            Guid response = Guid.Empty;
            try
            {
                Guid UserId = Guid.Parse(userId);
                ISession session = EntitySessionManager.GetSession();
                SystemUser su = session.Query<SystemUser>().Where(a => a.LoginMapId == UserId).FirstOrDefault();
                if (su == null) return Guid.Empty;
                UserType ut = session.Query<UserType>().Where(a => a.Id == su.UserTypeId).FirstOrDefault();
                if (ut == null) return Guid.Empty;

                if (ut.Code.ToUpper() == "RI")
                {
                    var reinsuereUser = session.Query<ReinsurerUser>().Where(a => a.InternalUserId == UserId).FirstOrDefault();
                    if (reinsuereUser != null)
                    {
                        response = reinsuereUser.ReinsurerId;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }

            return response;
        }

        internal static object GetAllUserForSearchGrid(UserSearchGridRequestDto UserSearchGridRequestDto)
        {
            try
            {
                if (UserSearchGridRequestDto != null && UserSearchGridRequestDto.paginationOptionsUserSearchGrid != null)
                {
                    Expression<Func<InternalUser, bool>> filterUser = PredicateBuilder.True<InternalUser>();
                    // filterUser = filterUser.And(a => a.IsActive == true);
                    //if (!String.IsNullOrEmpty(  UserSearchGridRequestDto.UserSearchGridSearchCriterias.eMail))
                    if (!String.IsNullOrEmpty(UserSearchGridRequestDto.UserSearchGridSearchCriterias.eMail))
                    {
                        filterUser = filterUser.And(a => a.Email.ToLower().Contains(UserSearchGridRequestDto.UserSearchGridSearchCriterias.eMail.ToLower()));
                    }
                    if (!String.IsNullOrEmpty(UserSearchGridRequestDto.UserSearchGridSearchCriterias.firstName))
                    {
                        filterUser = filterUser.And(a => a.FirstName.ToLower().Contains(UserSearchGridRequestDto.UserSearchGridSearchCriterias.firstName.ToLower()));
                    }
                    if (!String.IsNullOrEmpty(UserSearchGridRequestDto.UserSearchGridSearchCriterias.lastName))
                    {
                        filterUser = filterUser.And(a => a.LastName.ToLower().Contains(UserSearchGridRequestDto.UserSearchGridSearchCriterias.lastName.ToLower()));
                    }
                    if (!String.IsNullOrEmpty(UserSearchGridRequestDto.UserSearchGridSearchCriterias.mobileNo))
                    {
                        filterUser = filterUser.And(a => a.MobileNo.ToLower().Contains(UserSearchGridRequestDto.UserSearchGridSearchCriterias.mobileNo.ToLower()));
                    }
                    ISession session = EntitySessionManager.GetSession();
                    var filteredUser = session.Query<InternalUser>().Where(filterUser);

                    long TotalRecords = filteredUser.Count();
                    var customerGridDetailsFilterd = filteredUser.Skip((UserSearchGridRequestDto.paginationOptionsUserSearchGrid.pageNumber - 1) * UserSearchGridRequestDto.paginationOptionsUserSearchGrid.pageSize)
                    .Take(UserSearchGridRequestDto.paginationOptionsUserSearchGrid.pageSize)
                    .Select(a => new
                    {
                        Id = a.Id,
                        FirstName = a.FirstName,
                        LastName = a.LastName,
                        MobileNo = a.MobileNo,
                        Email = a.Email,
                    })
                    .ToArray();
                    var response = new CommonGridResponseDto()
                    {
                        totalRecords = TotalRecords,
                        data = customerGridDetailsFilterd
                    };
                    return new JavaScriptSerializer().Serialize(response);

                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }


        }

        internal static List<DashboardChartMappingResponseDto> GetDashBoardChartMapping(Guid Roleid)
        {
            ISession session = EntitySessionManager.GetSession();

            //Guid ReadLevelId = session.Query<PriviledgeLevel>().FirstOrDefault(a => a.LevelName == "Read").Id;
            //Guid CreateLevelId = session.Query<PriviledgeLevel>().FirstOrDefault(a => a.LevelName == "Create").Id;
            //Guid UpdateLevelId = session.Query<PriviledgeLevel>().FirstOrDefault(a => a.LevelName == "Update").Id;
            //Guid DeleteLevelId = session.Query<PriviledgeLevel>().FirstOrDefault(a => a.LevelName == "Delete").Id;


            var arr = from r in session.Query<UserRole>()
                      from dc in session.Query<DashboardChart>()
                      //from dca in session.Query<DashboardChartAccess>()
                      where r.RoleId == Roleid
                      orderby dc.Section
                      select new
                      {
                          r.RoleId,
                          r.RoleCode,
                          MenuId = dc.Id,
                          DashboardChartId = dc.Id,
                          Section = dc.Section,
                          ChartDisplayName = dc.ChartDisplayName,
                          //IsAllBranches = dca.IsAllBranches,
                          IsAllBranches = session.Query<DashboardChartAccess>().Count(dca => dca.UserRoleId == r.RoleId && dca.DashboardChartId == dc.Id && dca.IsAllBranches == true) > 0 ? true : false,
                          IsAllowed = session.Query<DashboardChartAccess>().Count(dca => dca.UserRoleId == r.RoleId && dca.DashboardChartId == dc.Id && dca.IsAllowed == true) > 0 ? true : false,
                          //Read = session.Query<RoleMenuMapping>().Count(rmm => rmm.RoleId == r.RoleId && rmm.MenuId == m.Id && rmm.LevelId == ReadLevelId) > 0 ? true : false,
                          ////Read = (from rmm in session.Query<RoleMenuMapping>() where rmm.RoleId == r.RoleId && rmm.MenuId == m.Id && rmm.LevelId == ReadLevelId).Count() > 0 ? true : false,
                          //Create = session.Query<RoleMenuMapping>().Count(rmm => rmm.RoleId == r.RoleId && rmm.MenuId == m.Id && rmm.LevelId == CreateLevelId) > 0 ? true : false,
                          ////Create = (from rmm in session.Query<RoleMenuMapping>() where rmm.RoleId == r.RoleId && rmm.MenuId == m.Id && rmm.LevelId == CreateLevelId).Count() > 0 ? true : false,
                          //Update = session.Query<RoleMenuMapping>().Count(rmm => rmm.RoleId == r.RoleId && rmm.MenuId == m.Id && rmm.LevelId == UpdateLevelId) > 0 ? true : false,
                          ////Update = (from rmm in session.Query<RoleMenuMapping>() where rmm.RoleId == r.RoleId && rmm.MenuId == m.Id && rmm.LevelId == UpdateLevelId).Count() > 0 ? true : false,
                          //Delete = session.Query<RoleMenuMapping>().Count(rmm => rmm.RoleId == r.RoleId && rmm.MenuId == m.Id && rmm.LevelId == DeleteLevelId) > 0 ? true : false,
                          ////Delete = (from rmm in session.Query<RoleMenuMapping>() where rmm.RoleId == r.RoleId && rmm.MenuId == m.Id && rmm.LevelId == DeleteLevelId).Count() > 0 ? true : false
                      };

            List<DashboardChartMappingResponseDto> rmlmrList = new List<DashboardChartMappingResponseDto>();

            DashboardChartMappingResponseDto rmlmr;
            foreach (var item in arr)
            {
                rmlmr = new DashboardChartMappingResponseDto();
                rmlmr.UserRoleId = item.RoleId;
                rmlmr.ChartDisplayName = item.ChartDisplayName;
                rmlmr.IsAllBranches = item.IsAllBranches;
                rmlmr.IsAllowed = item.IsAllowed;
                rmlmr.Section = item.Section;
                rmlmr.DashboardChartId = item.DashboardChartId;
                //rmlmr.UserRoleId = item.RoleId;

                rmlmrList.Add(rmlmr);
            }

            return rmlmrList;
        }

        internal static bool AddOrUpdateDashBoardChartMapping(List<DashboardChartMappingResponseDto> insertion)
        {
            ISession session = EntitySessionManager.GetSession();

            using (ITransaction transaction = session.BeginTransaction())
            {

                try
                {


                    var charts = session.Query<DashboardChartAccess>().Where(a => a.UserRoleId == insertion.First().UserRoleId);

                    foreach (var chartAcc in charts)
                    {
                        session.Delete(chartAcc);
                    }

                    DashboardChartAccess dcaccess;
                    //RoleMenuMapping rmm;
                    foreach (var item in insertion)
                    {
                        if (item.IsAllowed && item.IsAllBranches)
                        {
                            dcaccess = new Entities.DashboardChartAccess();
                            dcaccess.Id = Guid.NewGuid();
                            dcaccess.DashboardChartId = item.DashboardChartId;
                            dcaccess.IsAllBranches = item.IsAllBranches;
                            dcaccess.IsAllowed = item.IsAllowed;
                            dcaccess.UserRoleId = item.UserRoleId;
                            session.Save(dcaccess);
                        }

                            if (item.IsAllowed && !item.IsAllBranches)
                            {
                                dcaccess = new Entities.DashboardChartAccess();
                                dcaccess.Id = Guid.NewGuid();
                                dcaccess.DashboardChartId = item.DashboardChartId;
                                dcaccess.IsAllBranches = item.IsAllBranches;
                                dcaccess.IsAllowed = item.IsAllowed;
                                dcaccess.UserRoleId = item.UserRoleId;
                                session.Save(dcaccess);

                            }


                            if (!item.IsAllowed && item.IsAllBranches)
                        {
                            dcaccess = new Entities.DashboardChartAccess();
                            dcaccess.Id = Guid.NewGuid();
                            dcaccess.DashboardChartId = item.DashboardChartId;
                            dcaccess.IsAllBranches = item.IsAllBranches;
                            dcaccess.IsAllowed = item.IsAllowed;
                            dcaccess.UserRoleId = item.UserRoleId;
                            session.Save(dcaccess);
                        }

                    }

                    transaction.Commit();
                    return true;

                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                    return false;
                }
            }
        }
    }
}
