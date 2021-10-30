using TAS.DataTransfer.Common;
using TAS.DataTransfer.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAS.DataTransfer.Requests;

namespace TAS.Services
{
    public interface ICountryManagementService
    {

        CountriesResponseDto GetAllCountries(SecurityContext securityContext,AuditContext auditContext);

        CountriesResponseDto GetAllActiveCountries(SecurityContext securityContext, AuditContext auditContext);

        CountryRequestDto AddCountry(CountryRequestDto Country,
         SecurityContext securityContext,
         AuditContext auditContext);


        CountryResponseDto GetCountryById(Guid CountryId,
            SecurityContext securityContext,
            AuditContext auditContext);

        CountryRequestDto UpdateCountry(CountryRequestDto Country,
            SecurityContext securityContext,
            AuditContext auditContext);

        object GetAllCountrysByMakeNModelIdsNew(List<Guid> ModelIdList, Guid MakeId, SecurityContext securityContext, AuditContext auditContext);
    }
}
