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
    internal sealed class MenuManagementService : IMenuManagementService
    {

        public MenusResponseDto GetMenus(
            SecurityContext securityContext, 
            AuditContext auditContext)
        {
            MenusResponseDto result = null;

            MenusRetrievalUnitOfWork uow = new MenusRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }

            result = uow.Result;

            return result;
        }

        public MenuRequestDto AddMenu(MenuRequestDto Menu, SecurityContext securityContext,
            AuditContext auditContext) {
                MenuRequestDto result = new MenuRequestDto();
                MenuInsertionUnitOfWork uow = new MenuInsertionUnitOfWork(Menu);
                uow.SecurityContext = securityContext;
                uow.AuditContext = auditContext;

                if (uow.PreExecute())
                {
                    uow.Execute();
                }
                result.MenuInsertion = uow.Menu.MenuInsertion;
                return result;
        }


        public MenuResponseDto GetMenuById(Guid MenuId,
    SecurityContext securityContext,
    AuditContext auditContext)
        {
            MenuResponseDto result = new MenuResponseDto();

            MenuRetrievalUnitOfWork uow = new MenuRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            uow.MenuId = MenuId;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }

        public MenuRequestDto UpdateMenu(MenuRequestDto Menu, SecurityContext securityContext,
           AuditContext auditContext)
        {
            MenuRequestDto result = new MenuRequestDto();
            MenuUpdationUnitOfWork uow = new MenuUpdationUnitOfWork(Menu);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result.MenuInsertion = uow.Menu.MenuInsertion;
            return result;
        }

       
       
    }
}
