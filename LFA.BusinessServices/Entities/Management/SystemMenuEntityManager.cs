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
using NLog;
using System.Reflection;


namespace TAS.Services.Entities.Management
{
    public class SystemMenuEntityManager
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public List<Menu> GetTPAMenu()
        {
            List<Menu> entities = null;
            ISession session = EntitySessionManager.GetSession();
            IQueryable<Menu> userData = session.Query<Menu>();

            // TODO: get skip/take values as params
            entities = userData.ToList();
            return entities;
        }

        public List<SystemUser> GetTASMenu()
        {
            List<SystemUser> entities = null;

            try
            {
                ISession session = TASEntitySessionManager.GetSession();
                IQueryable<SystemUser> userData = session.Query<SystemUser>();

                // TODO: get skip/take values as params
                int recordsTotal = userData.Count();
                userData = userData.Skip(0).Take(10);
                entities = userData.ToList();
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }            
            return entities;
        }

        public List<TASMenu> GetTPATASMenu()
        {
            List<TASMenu> entities = null;
            try
            {
                ISession session = TASEntitySessionManager.GetSession();
                IQueryable<TASMenu> userData = session.Query<TASMenu>();

                // TODO: get skip/take values as params
                entities = userData.ToList();
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            
            return entities;
        }



        //public SystemUserResponseDto MenuById(string UserId)
        //{
        //    ISession session = EntitySessionManager.GetSession();

        //    SystemUserResponseDto pDto = new SystemUserResponseDto();

        //    var query =
        //        from User in session.Query<SystemUser>()
        //        where User.Id == UserId
        //        select new { User = User };

        //    var result = query.ToList();


        //    if (result != null && result.Count > 0)
        //    {
        //        pDto.Id = result.First().User.Id;
        //        pDto.Address1 = result.First().User.Address1;
        //        pDto.Address2 = result.First().User.Address2;
        //        pDto.Address3 = result.First().User.Address3;
        //        pDto.Address4 = result.First().User.Address4;
        //        pDto.CountryId = result.First().User.CountryId;
        //        pDto.DateOfBirth = result.First().User.DateOfBirth;
        //        pDto.DLIssueDate = result.First().User.DLIssueDate;
        //        pDto.Email = result.First().User.Email;
        //        pDto.FirstName = result.First().User.FirstName;
        //        pDto.Gender = result.First().User.Gender;
        //        pDto.IDNo = result.First().User.IDNo;
        //        pDto.IDTypeId = result.First().User.IDTypeId;
        //        pDto.InternalExtension = result.First().User.InternalExtension;
        //        pDto.IsActive = result.First().User.IsActive;
        //        pDto.LastName = result.First().User.LastName;
        //        pDto.MobileNo = result.First().User.MobileNo;
        //        pDto.NationalityId = result.First().User.NationalityId;
        //        pDto.OtherTelNo = result.First().User.OtherTelNo;
        //        pDto.Password = result.First().User.Password;
        //        pDto.ProfilePicture = result.First().User.ProfilePicture;
        //        pDto.UserName = result.First().User.UserName;
        //        pDto.EntryDate = result.First().User.EntryDate;
        //        pDto.EntryBy = result.First().User.EntryBy;

        //        pDto.IsUserExists = true;
        //        return pDto;
        //    }
        //    else
        //    {
        //        pDto.IsUserExists = false;
        //        return null;
        //    }
        //}

        internal List<MenuResponseDto> GetTPAMenuByUserName(string userName)
        {
            //List<Menu> entities = null;
            ISession session = EntitySessionManager.GetSession();

            Guid UserId = session.Query<SystemUser>().Where(a => a.UserName == userName).First().LoginMapId;

            //IQueryable<Menu> userData = session.Query<Menu>();
            var ss = (from RoleMenu in session.Query<RoleMenuMapping>()
                      join RoleUserMapping in session.Query<UserRoleMapping>() on RoleMenu.RoleId equals RoleUserMapping.RoleId
                      join menu in session.Query<Menu>() on RoleMenu.MenuId equals menu.Id
                      where RoleUserMapping.UserId == UserId
                      select menu).ToList();


            MenuResponseDto _mr = new MenuResponseDto();
            List<MenuResponseDto> mrd = new List<MenuResponseDto>();
            foreach (var item in ss.GroupBy(menu => new {menu.Id, menu.MenuName, menu.LinkURL, menu.ParentMenuId, menu.Icon, menu.OrderVal , menu.MenuCode}).Select(s => new {s.Key}).ToList())
            {
                _mr = new MenuResponseDto();

                _mr.Id = item.Key.Id;
                _mr.MenuName = item.Key.MenuName;
                _mr.LinkURL = item.Key.LinkURL;
                _mr.ParentMenuId = item.Key.ParentMenuId;
                _mr.Icon = item.Key.Icon;
                _mr.OrderVal = item.Key.OrderVal;
                _mr.MenuCode = item.Key.MenuCode;

                mrd.Add(_mr);
            }


            //var ss = (from RoleMenu in session.Query<RoleMenuMapping>()
            //          join RoleUserMapping in session.Query<UserRoleMapping>() on RoleMenu.RoleId equals RoleUserMapping.RoleId
            //          join menu in session.Query<Menu>() on RoleMenu.MenuId equals menu.Id
            //          where RoleUserMapping.UserId == Guid.Parse(UserId)
            //          group RoleMenu by RoleMenu.MenuId into grouped
            //          select new { menuid = grouped.Key }).ToList();

            //  //from m in session.Query<Menu>()
            //var query = from m in session.Query<Menu>() where m.MenuName
            //            join sub in ss on m.Id equals sub.menuid 
            //                select new {m.MenuName};

            // TODO: get skip/take values as params

            //var e = query.ToList();
            return mrd;
        }
    }
}
