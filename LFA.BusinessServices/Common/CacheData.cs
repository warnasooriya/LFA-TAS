using TAS.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TAS.DataTransfer.Responses;

namespace TAS.Services.Common
{
    internal static class CacheData
    {

        private static ICache Cache
        {
            get
            {
                return CacheFactory.GetCache();
            }
        }

        public static string Text
        {
            get
            {
                return CacheData.Cache["Test_Text"] as string;
            }
            set
            {
                CacheData.Cache["Test_Text"] = value;
            }
        }

        public static List<CityResponseDto> Cities
        {
            get
            {
                return CacheData.Cache["Cities"] as List<CityResponseDto>;
            }
            set
            {
                CacheData.Cache["Cities"] = value;
            }
        }
         
        public static List<CityResponseDto> GetCitiesByCountryId(Guid countryId)
        {
            List<CityResponseDto> cities = null;

            string key = "CitiesForCountry_" + countryId.ToString();

            cities = CacheData.Cache[key] as List<CityResponseDto>;

            return cities;
        }

        public static void SetCitiesByCountryId(Guid countryId, List<CityResponseDto> cities)
        {
            string key = "CitiesForCountry_" + countryId.ToString();

            CacheData.Cache[key] = cities;
        }

    }
}