using Newtonsoft.Json.Linq;
using System;
using System.Web.Http;
using TAS.DataTransfer.Responses;
using TAS.Services;
using TAS.Web.Common;

namespace TAS.Web.Controllers
{
    public class HomeController : ApiController
    {
       // private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [HttpPost]
        public object GetAllCountries()
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            ICountryManagementService countryManagementService = ServiceFactory.GetCountryManagementService();

            CountriesResponseDto countryData = countryManagementService.GetAllCountries(
            SecurityHelper.Context,
            AuditHelper.Context);
            return countryData.Countries.FindAll(c=>c.IsActive).ToArray();
        }

        [HttpPost]
        public object GetAllMakes()
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            IMakeManagementService makeManagementService = ServiceFactory.GetMakeManagementService();

            MakesResponseDto makesData = makeManagementService.GetAllMakes(
            SecurityHelper.Context,
            AuditHelper.Context);
            return makesData.Makes.ToArray();
        }

        [HttpPost]
        public object GetAllDealers()
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            IDealerManagementService dealerManagementService = ServiceFactory.GetDealerManagementService();

            DealersRespondDto dealerData = dealerManagementService.GetAllDealers(
            SecurityHelper.Context,
            AuditHelper.Context);
            return dealerData.Dealers.ToArray();
        }

        [HttpPost]
        public object GetAllFuelTypes()
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            
            IFuelTypeManagementService fuelTypeManagementService = ServiceFactory.GetFuelTypeManagementService();

            FuelTypesResponseDto fueltypeData = fuelTypeManagementService.GetFuelTypes(
            SecurityHelper.Context,
            AuditHelper.Context);
            return fueltypeData.FuelTypes.ToArray();
        }

        [HttpPost]
        public object GetModelsByMakeId([FromBody]JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            Guid MakeId = Guid.Parse(data["MakeId"].ToString());
            IModelManagementService modelManagementService = ServiceFactory.GetModelManagementService();
          
            ModelesResponseDto modelData = modelManagementService.GetModelsByMakeId(MakeId,
            SecurityHelper.Context,
            AuditHelper.Context);
            return modelData.Modeles.ToArray();
         }
        //[HttpPost]
        //public object GetAllMakes()
        //{
        //    IMakeManagementService makeManagementService = ServiceFactory.GetMakeManagementService();

        //    ManufacturesResponseDto makeData = makeManagementService.GetAllManufatures(
        //    SecurityHelper.Context,
        //    AuditHelper.Context);
        //    return makeData.Manufactures.ToArray();
        //}

        [HttpPost]
        public object GetAllCommodityTypes()
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            ICommodityManagementService commodityManagementService = ServiceFactory.GetCommodityManagementService();

            CommoditiesResponseDto commodityData = commodityManagementService.GetAllCommodities(
            SecurityHelper.Context,
            AuditHelper.Context);
            return commodityData.Commmodities.ToArray();
        }

        [HttpPost]
        public object GetCommodityCategoriesByCommodityType([FromBody]JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            Guid commodityTypeID = Guid.Parse(data["ID"].ToString());

            ICommodityManagementService commodityManagementService = ServiceFactory.GetCommodityManagementService();

            CommodityCategoriesRespondDto commodityCategories = commodityManagementService.GetCommodityCategoriesByCommodityTypeId(
            SecurityHelper.Context,
            AuditHelper.Context,
            commodityTypeID
            );
            return commodityCategories.CommodityCategories.ToArray();
           
        }

        
    }
}
