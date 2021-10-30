using TAS.DataTransfer.Common;
using TAS.DataTransfer.Responses;
using TAS.DataTransfer.Requests;
using System;
using TAS.Services.Entities;
namespace TAS.Services
{
    public interface IMenuManagementService
    {
        MenusResponseDto GetMenus(
            SecurityContext securityContext, 
            AuditContext auditContext);

        MenuRequestDto AddMenu(MenuRequestDto Menu,
            SecurityContext securityContext,
            AuditContext auditContext);


        MenuResponseDto GetMenuById(Guid MenuId,
            SecurityContext securityContext,
            AuditContext auditContext);

        MenuRequestDto UpdateMenu(MenuRequestDto Menu,
            SecurityContext securityContext,
            AuditContext auditContext);


    }
}
