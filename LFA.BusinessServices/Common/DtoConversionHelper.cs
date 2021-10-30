using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAS.DataTransfer.Responses;
using TAS.Services.Entities;

namespace TAS.Services.Common
{
    internal static class DtoConversionHelper
    {

        public static List<CityResponseDto> GetCityResponseDtos(List<City> cityEntities)
        {
            List<CityResponseDto> cityResponseDtos = new List<CityResponseDto>();

            if (cityEntities != null)
            {
                foreach (City cityEntity in cityEntities)
                {
                    CityResponseDto cityResponseDto = DtoConversionHelper.GetCityResponseDto(cityEntity);

                    cityResponseDtos.Add(cityResponseDto);
                }
            }

            return cityResponseDtos;
        }

        public static CityResponseDto GetCityResponseDto(City city)
        {
            CityResponseDto cityResponseDto = null;

            if (city != null)
            {
                cityResponseDto = new CityResponseDto();
                cityResponseDto.Id = city.Id;
                cityResponseDto.CityName = city.CityName;
                cityResponseDto.CountryId = city.CountryId;
                cityResponseDto.ZipCode = city.ZipCode;
            }

            return cityResponseDto;
        }

    }
}
