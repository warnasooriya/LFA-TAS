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
    internal sealed class BrownAndWhiteDetailsManagementService : IBrownAndWhiteDetailsManagementService
    {

        public BrownAndWhiteAllDetailsResponseDto GetBrownAndWhiteAllDetails(
            SecurityContext securityContext, 
            AuditContext auditContext)
        {
            BrownAndWhiteAllDetailsResponseDto result = null;

            BrownAndWhiteDetailssRetrievalUnitOfWork uow = new BrownAndWhiteDetailssRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }

            result = uow.Result;

            return result;
        }

        public BrownAndWhiteAllDetailsResponseDto GetParentBrownAndWhiteDetailss(
    SecurityContext securityContext,
    AuditContext auditContext)
        {
            BrownAndWhiteAllDetailsResponseDto result = null;

            BrownAndWhiteDetailssRetrievalUnitOfWork uow = new BrownAndWhiteDetailssRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }

            result = uow.Result;

            return result;
        }

        public BrownAndWhiteDetailsRequestDto AddBrownAndWhiteDetails(BrownAndWhiteDetailsRequestDto BrownAndWhiteDetails, SecurityContext securityContext,
            AuditContext auditContext) {
                BrownAndWhiteDetailsRequestDto result = new BrownAndWhiteDetailsRequestDto();
                BrownAndWhiteDetailsInsertionUnitOfWork uow = new BrownAndWhiteDetailsInsertionUnitOfWork(BrownAndWhiteDetails);
                uow.SecurityContext = securityContext;
                uow.AuditContext = auditContext;

                if (uow.PreExecute())
                {
                    uow.Execute();
                }
                result = uow.BrownAndWhiteDetails;
                return result;
        }


        public BrownAndWhiteDetailsResponseDto GetBrownAndWhiteDetailsById(Guid BrownAndWhiteDetailsId,
    SecurityContext securityContext,
    AuditContext auditContext)
        {
            BrownAndWhiteDetailsResponseDto result = new BrownAndWhiteDetailsResponseDto();

            BrownAndWhiteDetailsRetrievalUnitOfWork uow = new BrownAndWhiteDetailsRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            uow.BrownAndWhiteDetailsId = BrownAndWhiteDetailsId;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }

        public BrownAndWhiteDetailsRequestDto UpdateBrownAndWhiteDetails(BrownAndWhiteDetailsRequestDto BrownAndWhiteDetails, SecurityContext securityContext,
           AuditContext auditContext)
        {
            BrownAndWhiteDetailsRequestDto result = new BrownAndWhiteDetailsRequestDto();
            BrownAndWhiteDetailsUpdationUnitOfWork uow = new BrownAndWhiteDetailsUpdationUnitOfWork(BrownAndWhiteDetails);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.BrownAndWhiteDetails;
            return result;
        }

        public object GetAllItemsForSearchGrid(BnWSearchGridRequestDto BnWSearchGridRequestDto,
            SecurityContext securityContext, 
            AuditContext auditContext)
        {
            BrownAndWhiteItemsRetrievalUnitOfWork uow = new BrownAndWhiteItemsRetrievalUnitOfWork(BnWSearchGridRequestDto);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Result;
        }

        public string ValidateDealerCurrency(Guid dealerId, SecurityContext securityContext, AuditContext auditContext)
        {
            DealerCurrencyValidationUnitOfWork uow = new DealerCurrencyValidationUnitOfWork(dealerId);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Result;
        }
       
    }
}
