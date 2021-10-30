
using Newtonsoft.Json.Linq;
using NLog;
using System;
using System.Reflection;
using System.Web.Http;
using TAS.DataTransfer.Requests;
using TAS.Services;
using TAS.Web.Common;

namespace TAS.Web.Controllers
{
    public class VehicleController : ApiController
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        public string GetAllVehicleTypes(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                FuelTypeRequestDto fuelType = data.ToObject<FuelTypeRequestDto>(); //JsonConvert.DeserializeObject<List<FuelTypeRequestDto>>(data);
                IFuelTypeManagementService fuelTypeManagementService = ServiceFactory.GetFuelTypeManagementService();
                FuelTypeRequestDto result = fuelTypeManagementService.AddFuelType(fuelType, SecurityHelper.Context, AuditHelper.Context);
                logger.Info("FuelType Added");
                if (result.FuelTypeInsertion)
                {
                    return "OK";
                }
                else
                {
                    return "Add fuelType failed!";
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return "Add fuelType failed!";
            }
            
        }

        [HttpPost]
        public object GetAllVehiclesForSearchGrid(VehicleSearchGridRequestDto VehicleSearchGridRequestDto)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            IVehicleDetailsManagementService VehicleDetailsManagementService = ServiceFactory.GetVehicleDetailsManagementService();
            return VehicleDetailsManagementService.GetAllVehiclesForSearchGrid(VehicleSearchGridRequestDto, SecurityHelper.Context, AuditHelper.Context);

        }

        [HttpPost]
        public object GetAllVehiclesForSearchGridByDealerId(VehicleSearchGridRequestDto VehicleSearchGridRequestDto)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            IVehicleDetailsManagementService VehicleDetailsManagementService = ServiceFactory.GetVehicleDetailsManagementService();
            return VehicleDetailsManagementService.GetAllVehiclesForSearchGridByDealerId(VehicleSearchGridRequestDto, SecurityHelper.Context, AuditHelper.Context);

        }
    }
}
