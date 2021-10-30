using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAS.DataTransfer.Common;
using TAS.DataTransfer.Requests;
using TAS.DataTransfer.Responses;
using TAS.Services.UnitsOfWork;

namespace TAS.Services.BusinessServices
{
    internal sealed class OtherItemManagementService:IOtherItemManagementService
    {
        public object GetAllItemsForSearchGrid(OtherItemSearchGridRequestDto OtherItemSearchGridRequestDto,
           SecurityContext securityContext, AuditContext auditContext)
        {
            object Response = null;
            OtherItemRetrievalUnitOfWork uow = new OtherItemRetrievalUnitOfWork(OtherItemSearchGridRequestDto);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
                Response = uow.Result;
            }
            return Response;
        }

        public BrownAndWhiteDetailsResponseDto GetOtherItemDetailsById(Guid itemId, 
            SecurityContext securityContext, AuditContext auditContext)
        {
            BrownAndWhiteDetailsResponseDto Response = null;
            OtherItemDetailsRetrievalByIdUnitOfWork uow = new OtherItemDetailsRetrievalByIdUnitOfWork(itemId);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
                Response = uow.Result;
            }
            return Response;
        }

        public OtherItemDetailsResponseDto GetOtherDetailsById(Guid OtherItemId,SecurityContext securityContext,
            AuditContext auditContext)
        {
            OtherItemDetailsResponseDto result = new OtherItemDetailsResponseDto();

            OtherDetailsRetrievalByIdUnitOfWork uow = new OtherDetailsRetrievalByIdUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            uow.BrownAndWhiteDetailsId = OtherItemId;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }
    }
}
