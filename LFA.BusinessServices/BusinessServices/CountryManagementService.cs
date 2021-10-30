using TAS.DataTransfer.Common;
using TAS.DataTransfer.Responses;
using TAS.Services.UnitsOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAS.DataTransfer.Requests;

namespace TAS.Services.BusinessServices
{
    internal sealed class CountryManagementService : ICountryManagementService
    {
        public CountriesResponseDto GetAllCountries(
            SecurityContext securityContext,
            AuditContext auditContext)
        {
            CountriesResponseDto result = null;
            CountriesRetrievalUnitOfWork uow = new CountriesRetrievalUnitOfWork();
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;

        }
        public CountriesResponseDto GetAllActiveCountries(
            SecurityContext securityContext,
            AuditContext auditContext)
        {
            CountriesResponseDto result = null;
            CountriesRetrievalUnitOfWork uow = new CountriesRetrievalUnitOfWork();
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;

        }

        
        public CountryRequestDto AddCountry(CountryRequestDto Country,
         SecurityContext securityContext,
         AuditContext auditContext)
        {
            CountryRequestDto result = new CountryRequestDto();
            CountryInsertionUnitOfWork uow = new CountryInsertionUnitOfWork(Country);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result.CountryInsertion = uow.Country.CountryInsertion;
            return result;
        }


        public CountryResponseDto GetCountryById(Guid CountryId,
            SecurityContext securityContext,
            AuditContext auditContext)
        {
            CountryResponseDto result = new CountryResponseDto();

            CountryRetrievalUnitOfWork uow = new CountryRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            uow.CountryId = CountryId;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }

        public CountryRequestDto UpdateCountry(CountryRequestDto Country,
            SecurityContext securityContext,
           AuditContext auditContext)
        {
            CountryRequestDto result = new CountryRequestDto();
            CountryUpdationUnitOfWork uow = new CountryUpdationUnitOfWork(Country);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result.CountryInsertion = uow.Country.CountryInsertion;
            return result;
        }


        public object GetAllCountrysByMakeNModelIdsNew(List<Guid> ModelIdList, Guid MakeId, SecurityContext securityContext, AuditContext auditContext)
        {
            object result = null;
            CountriesRetrievalByMakeNModelsIdsUnitOfWork uow = new CountriesRetrievalByMakeNModelsIdsUnitOfWork(MakeId, ModelIdList);
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
