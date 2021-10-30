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
    internal sealed class YellowGoodManagementService:IYellowGoodManagementService
    {
        public object GetAllItemsForSearchGrid(YellowGoodSearchGridRequestDto YellowGoodSearchGridRequestDto, 
            SecurityContext securityContext, AuditContext auditContext)
        {
            object Response = null;
            YellowGoodRetrievalUnitOfWork uow = new YellowGoodRetrievalUnitOfWork(YellowGoodSearchGridRequestDto);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
                Response = uow.Result;
            }
            return Response;
        }

        public BrownAndWhiteDetailsResponseDto GetYellowGoodDetaisById(Guid ItemId, SecurityContext securityContext, AuditContext auditContext)
        {
            BrownAndWhiteDetailsResponseDto Response = null;
            YellowGoodDetailRetrievalByIdUnitOfWork uow = new YellowGoodDetailRetrievalByIdUnitOfWork(ItemId);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
                Response = uow.Result;
            }
            return Response;
        }
    }
}
