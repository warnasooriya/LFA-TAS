using TAS.DataTransfer.Common;
using TAS.DataTransfer.Responses;
using TAS.Services.UnitsOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.BusinessServices
{
    internal sealed class CityManagementService : ICityManagementService
    {
        public CitiesResponseDto GetAllCities(
            SecurityContext securityContext, 
            AuditContext auditContext) 
        {
            CitiesResponseDto result = null;
            CitiesRetrievalUnitOfWork uow = new CitiesRetrievalUnitOfWork();
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        
        }


        public CitiesResponseDto GetAllCitiesByCountry(Guid CountryId, SecurityContext securityContext, AuditContext auditContext)
        {
            CitiesResponseDto result = null;
            CitiesRetrievalUnitOfWork uow = new CitiesRetrievalUnitOfWork();
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            uow.countryId = CountryId;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }

        public CityResponseDto GetCityById(Guid Id,
    SecurityContext securityContext,
    AuditContext auditContext)
        {
            CityResponseDto result = new CityResponseDto();

            CityRetrievalUnitOfWork uow = new CityRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            uow.CityId = Id;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }
    }
}
