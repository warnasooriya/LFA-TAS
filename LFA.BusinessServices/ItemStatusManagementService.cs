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
    internal sealed class ItemStatusManagementService : IItemStatusManagementService
    {

        public ItemStatusesResponseDto GetItemStatuss(
            SecurityContext securityContext, 
            AuditContext auditContext)
        {
            ItemStatusesResponseDto result = null;

            ItemStatussRetrievalUnitOfWork uow = new ItemStatussRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }

            result = uow.Result;

            return result;
        }

        public ItemStatusRequestDto AddItemStatus(ItemStatusRequestDto ItemStatus, SecurityContext securityContext,
            AuditContext auditContext) {
                ItemStatusRequestDto result = new ItemStatusRequestDto();
                ItemStatusInsertionUnitOfWork uow = new ItemStatusInsertionUnitOfWork(ItemStatus);
                uow.SecurityContext = securityContext;
                uow.AuditContext = auditContext;

                if (uow.PreExecute())
                {
                    uow.Execute();
                }
                result.ItemStatusInsertion = uow.ItemStatus.ItemStatusInsertion;
                return result;
        }


        public ItemStatusResponseDto GetItemStatusById(Guid ItemStatusId,
    SecurityContext securityContext,
    AuditContext auditContext)
        {
            ItemStatusResponseDto result = new ItemStatusResponseDto();

            ItemStatusRetrievalUnitOfWork uow = new ItemStatusRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            uow.ItemStatusId = ItemStatusId;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }

        public ItemStatusRequestDto UpdateItemStatus(ItemStatusRequestDto ItemStatus, SecurityContext securityContext,
           AuditContext auditContext)
        {
            ItemStatusRequestDto result = new ItemStatusRequestDto();
            ItemStatusUpdationUnitOfWork uow = new ItemStatusUpdationUnitOfWork(ItemStatus);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result.ItemStatusInsertion = uow.ItemStatus.ItemStatusInsertion;
            return result;
        }

        public bool IsExsistingItemStatusByStatus(Guid Id, string status,
            SecurityContext securityContext,
            AuditContext auditContext)
        {
            bool Result = false;
            IsExixtingItemStatusByStatusUnitOfWorks uow = new IsExixtingItemStatusByStatusUnitOfWorks();
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            uow.Status = status;
            uow.ItemStatusDescription = "";
            uow.Id = Id;
            if (uow.PreExecute()) { uow.Execute(); }
            Result = uow.Result;
            return Result;
        }
       
    }
}
