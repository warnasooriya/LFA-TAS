using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Criterion;
using NHibernate.Linq;
using TAS.Services.Entities.Persistence;
using TAS.DataTransfer.Responses;

namespace TAS.Services.Entities.Management
{
    public class CityEntityManager
    {
        public List<CityResponseDto> GetAllCities()
        {

            ISession session = EntitySessionManager.GetSession();
            return session.Query<City>().Select(city => new CityResponseDto
            {
                Id = city.Id,
                CityName = city.CityName,
                CountryId = city.CountryId,
                ZipCode = city.ZipCode
            }).OrderBy(a=>a.CityName).ToList();

        }

        public List<CityResponseDto> GetAllCitiesByCountryId(Guid countryId)
        {
            ISession session = EntitySessionManager.GetSession();
            return session.Query<City>().Where(a => a.CountryId == countryId)
                .Select(city => new CityResponseDto
            {
                Id = city.Id,
                CityName = city.CityName,
                CountryId = city.CountryId,
                ZipCode = city.ZipCode
            }).OrderBy(a=>a.CityName).ToList();
        }

        public CityResponseDto GetCityById(Guid CityId)
        {
            ISession session = EntitySessionManager.GetSession();

            CityResponseDto pDto = new CityResponseDto();

            var query =
                from City in session.Query<City>()
                where City.Id == CityId
                select new { City = City };

            var result = query.ToList();


            if (result != null && result.Count > 0)
            {

                pDto.CityName = result.First().City.CityName;
                pDto.CountryId = result.First().City.CountryId;
                pDto.Id = result.First().City.Id;
                pDto.ZipCode = result.First().City.ZipCode;

                pDto.IsCityExists = true;

                return pDto;
            }
            else
            {
                pDto.IsCityExists = false;

                return null;
            }
        }
    }
}
