using TAS.DataTransfer.Common;
using TAS.DataTransfer.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services
{
    public interface ICityManagementService
    {

        CitiesResponseDto GetAllCities(SecurityContext securityContext, AuditContext auditContext);

        CitiesResponseDto GetAllCitiesByCountry(Guid CountryId, SecurityContext securityContext, AuditContext auditContext);

        CityResponseDto GetCityById(Guid Id, SecurityContext securityContext, AuditContext auditContext);
    }
}
