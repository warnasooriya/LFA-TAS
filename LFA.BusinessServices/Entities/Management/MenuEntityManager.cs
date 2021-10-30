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
using System.Security.Cryptography;
using System.Reflection;
using NLog;

namespace TAS.Services.Entities.Management
{
    public class MenuEntityManager
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public List<MenuResponseDto> GetMenus()
        {
            ISession session = EntitySessionManager.GetSession();
            return session.Query<Menu>().Select(Menu => new MenuResponseDto {
                Id = Menu.Id,
                MenuName = Menu.MenuName,
                ParentMenuId = Menu.ParentMenuId,
                LinkURL = Menu.LinkURL,
                Icon = Menu.Icon,
                OrderVal = Menu.OrderVal,
                //need to write other fields
            }).ToList();
        }

        public MenuResponseDto GetMenuById(Guid MenuId)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();

                MenuResponseDto pDto = new MenuResponseDto();

                var query =
                    from Menu in session.Query<Menu>()
                    where Menu.Id == MenuId
                    select new { Menu = Menu };

                var result = query.ToList();


                if (result != null && result.Count > 0)
                {
                    pDto.Id = result.First().Menu.Id;
                    pDto.MenuName = result.First().Menu.MenuName;
                    pDto.Icon = result.First().Menu.Icon;
                    pDto.LinkURL = result.First().Menu.LinkURL;
                    pDto.ParentMenuId = result.First().Menu.ParentMenuId;
                    pDto.OrderVal = result.First().Menu.OrderVal;
                    pDto.MenuCode = result.First().Menu.MenuCode;
                    pDto.IsMenuExists = true;
                    return pDto;
                }
                else
                {
                    pDto.IsMenuExists = false;
                    return null;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }

        }

        internal bool AddMenu(MenuRequestDto Menu)
        {
            try
            {

                ISession session = EntitySessionManager.GetSession();
                Menu pr = new Entities.Menu();

                pr.Id = new Guid();
                pr.MenuName = Menu.MenuName;
                pr.LinkURL = Menu.LinkURL;
                pr.ParentMenuId = Menu.ParentMenuId;
                pr.Icon = Menu.Icon;
                pr.OrderVal = Menu.OrderVal;
                pr.MenuCode = Menu.MenuCode;
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.SaveOrUpdate(pr);
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

        internal bool UpdateMenu(MenuRequestDto Menu)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();
                Menu pr = new Entities.Menu();

                pr.Id = Menu.Id;
                pr.MenuName = Menu.MenuName;
				pr.LinkURL = Menu.LinkURL;
                pr.ParentMenuId = Menu.ParentMenuId;
                pr.Icon = Menu.Icon;
                pr.OrderVal = Menu.OrderVal;
                pr.MenuCode = Menu.MenuCode;

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

        internal bool GetMenuAccessByUserId(string menuLinkUrl, Guid LoggedInUserId)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();
                Guid menuId = session.Query<Menu>().Where(a => a.LinkURL.ToLower() == menuLinkUrl.ToLower()).FirstOrDefault().Id;
                if (menuId == Guid.Empty) return false;

                IQueryable<UserRoleMapping> roleMappings = session.Query<UserRoleMapping>().Where(a => a.UserId == LoggedInUserId);
                IQueryable<RoleMenuMapping> allowedRoles = session.Query<RoleMenuMapping>().Where(a => a.MenuId == menuId);

                int allowedRoleCount = roleMappings.Where(a => allowedRoles.Any(b => b.RoleId == a.RoleId)).Count();
                if (allowedRoleCount > 0)
                    return true;
                return false;

            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                 return false;
            }


        }


    }
}
