using TAS.DataTransfer.Common;
using TAS.DataTransfer.Requests;
using TAS.DataTransfer.Responses;
using TAS.Services.UnitsOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.BusinessServices
{

    public class BrokerManagementService : IBrokerManagementService
    {



        public BrokerSResponseDto GetAllBrokers(SecurityContext securityContext, AuditContext auditContext)
        {
            BrokerSResponseDto result = null;
            BrokerRetriveUnitOfWork uow = new BrokerRetriveUnitOfWork();
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }

        public BrokerSResponseDto GetBrokerDetailsByBrokerId(SecurityContext securityContext, AuditContext auditContext, Guid BrokerId)
        {
            BrokerSResponseDto result = null;
            BrokerRetriveUnitOfWork uow = new BrokerRetriveUnitOfWork(BrokerId);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }

        public bool SaveBroker(BrokerRequestDto BrokerData, SecurityContext securityContext, AuditContext auditContext)
        {
              
                bool result = false;
                BrokerInsertionUnitOfWork uow = new BrokerInsertionUnitOfWork(BrokerData);
                uow.SecurityContext = securityContext;
                uow.AuditContext = auditContext;
                if (uow.PreExecute())
                {
                    uow.Execute();
                }
                result = uow.Result;
                return result;
      
        }

        public bool UpdateBroker(BrokerRequestDto BrokerData, SecurityContext securityContext, AuditContext auditContext)
        {
            bool result = false;
            BrokerUpdateUnitOfWork uow = new BrokerUpdateUnitOfWork(BrokerData);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }


        public BrokerSResponseDto GetAllBrokersByCountry(SecurityContext securityContext, AuditContext auditContext , Guid countryId)
        {
            BrokerSResponseDto result = null;
            BrokerRetriveByCountryUnitOfWork uow = new BrokerRetriveByCountryUnitOfWork(countryId);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }


        public bool IsExsistsBrokerCode(Guid guid,string Name, string Code, SecurityContext securityContext, AuditContext auditContext)
        {
            bool result = false;
            BrokerIsExsistsBrokerCodeUnitOfWork uow = new BrokerIsExsistsBrokerCodeUnitOfWork(guid, Name, Code);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }
    }
}
