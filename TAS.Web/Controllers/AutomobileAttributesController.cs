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
    public class AutomobileAttributesController : ApiController
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        #region CylinderCount

        public string AddCylinderCount(JObject data)
        {
            try
            {
               
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                var cylinderCount = data.ToObject<CylinderCountRequestDto>();
                var cylinderCountManagementService = ServiceFactory.GetCylinderCountManagementService();
                var result = cylinderCountManagementService.AddCylinderCount(cylinderCount, SecurityHelper.Context,
                    AuditHelper.Context);
                logger.Info("CylinderCount Added");
                if (result.CylinderCountInsertion)
                {
                    return "OK";
                }
                return "Add cylinderCount failed!";
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                             ex.InnerException);
                return "Add cylinderCount failed!";
            }
        }

        public string UpdateCylinderCount(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                var cylinderCount = data.ToObject<CylinderCountRequestDto>();
                var cylinderCountManagementService = ServiceFactory.GetCylinderCountManagementService();
                var result = cylinderCountManagementService.UpdateCylinderCount(cylinderCount, SecurityHelper.Context,
                    AuditHelper.Context);
                logger.Info("CylinderCount Added");
                if (result.CylinderCountInsertion)
                {
                    return "OK";
                }
                return "Add cylinderCount failed!";
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                             ex.InnerException);
                return "Add cylinderCount failed!";
            }
        }

        [HttpPost]
        public object GetCylinderCountById(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                var cylinderCountManagementService = ServiceFactory.GetCylinderCountManagementService();


                var cylinderCount =
                    cylinderCountManagementService.GetCylinderCountById(Guid.Parse(data["cylinderCountId"].ToString()),
                        SecurityHelper.Context,
                        AuditHelper.Context);


                return cylinderCount;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }
            
        }

        [HttpPost]
        public object GetAllCylinderCounts()
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                var cylinderCountManagementService = ServiceFactory.GetCylinderCountManagementService();

                var cylinderCountData = cylinderCountManagementService.GetCylinderCounts(
                    SecurityHelper.Context,
                    AuditHelper.Context);
                return cylinderCountData.CylinderCounts.ToArray();
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }
            
        }

        #endregion

        #region DriveType

        public string AddDriveType(JObject data)
        {
            try
            {
               
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                var driveType = data.ToObject<DriveTypeRequestDto>();
                var driveTypeManagementService = ServiceFactory.GetDriveTypeManagementService();
                var result = driveTypeManagementService.AddDriveType(driveType, SecurityHelper.Context,
                    AuditHelper.Context);
                logger.Info("DriveType Added");
                if (result.DriveTypeInsertion)
                {
                    return "OK";
                }
                return "Add driveType failed!";
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                             ex.InnerException);
                return "Add driveType failed!";
            }
        }

        public string UpdateDriveType(JObject data)
        {
            try
            {
             
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                var driveType = data.ToObject<DriveTypeRequestDto>();
                var driveTypeManagementService = ServiceFactory.GetDriveTypeManagementService();
                var result = driveTypeManagementService.UpdateDriveType(driveType, SecurityHelper.Context,
                    AuditHelper.Context);
                logger.Info("DriveType Added");
                if (result.DriveTypeInsertion)
                {
                    return "OK";
                }
                return "Add driveType failed!";
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                             ex.InnerException);
                return "Add driveType failed!";
            }
        }

        [HttpPost]
        public object GetDriveTypeById(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                var driveTypeManagementService = ServiceFactory.GetDriveTypeManagementService();


                var driveType = driveTypeManagementService.GetDriveTypeById(Guid.Parse(data["driveTypeId"].ToString()),
                    SecurityHelper.Context,
                    AuditHelper.Context);


                return driveType;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }
            
        }

        [HttpPost]
        public object GetAllDriveTypes()
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                var driveTypeManagementService = ServiceFactory.GetDriveTypeManagementService();

                var driveTypeData = driveTypeManagementService.GetDriveTypes(
                    SecurityHelper.Context,
                    AuditHelper.Context);
                return driveTypeData.DriveTypes.ToArray();
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }
            
        }

        #endregion

        #region EngineCapacity

        public string AddEngineCapacity(JObject data)
        {
            try
            {
               
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                var engineCapacity = data.ToObject<EngineCapacityRequestDto>();
                   
                var engineCapacityManagementService = ServiceFactory.GetEngineCapacityManagementService();
                var result = engineCapacityManagementService.AddEngineCapacity(engineCapacity, SecurityHelper.Context,
                    AuditHelper.Context);
                logger.Info("EngineCapacity Added");
                if (result.EngineCapacityInsertion)
                {
                    return "OK";
                }
                return "Add engineCapacity failed!";
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                             ex.InnerException);
                return "Add engineCapacity failed!";
            }
        }

        public string UpdateEngineCapacity(JObject data)
        {
            try
            {
               
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                var engineCapacity = data.ToObject<EngineCapacityRequestDto>();
                var engineCapacityManagementService = ServiceFactory.GetEngineCapacityManagementService();
                var result = engineCapacityManagementService.UpdateEngineCapacity(engineCapacity, SecurityHelper.Context,
                    AuditHelper.Context);
                logger.Info("EngineCapacity Added");
                if (result.EngineCapacityInsertion)
                {
                    return "OK";
                }
                return "Add engineCapacity failed!";
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                             ex.InnerException);
                return "Add engineCapacity failed!";
            }
        }

        [HttpPost]
        public object GetEngineCapacityById(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                var engineCapacityManagementService = ServiceFactory.GetEngineCapacityManagementService();


                var engineCapacity =
                    engineCapacityManagementService.GetEngineCapacityById(Guid.Parse(data["engineCapacityId"].ToString()),
                        SecurityHelper.Context,
                        AuditHelper.Context);


                return engineCapacity;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }
            
        }

        [HttpPost]
        public bool IsExsistingEngineCapacityByEngineCapacityNumber(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                var engineCapacityManagementService = ServiceFactory.GetEngineCapacityManagementService();

                var isExsists =
                    engineCapacityManagementService.IsExsistingEngineCapacityByEngineCapacityNumber(
                        Guid.Parse(data["Id"].ToString()), Convert.ToDecimal(data["EngineCapacityNumber"]),
                        SecurityHelper.Context,
                        AuditHelper.Context);
                return isExsists;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return false;
            }
            
        }

        [HttpPost]
        public object GetAllEngineCapacities()
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                var engineCapacityManagementService = ServiceFactory.GetEngineCapacityManagementService();

                var engineCapacityData = engineCapacityManagementService.GetEngineCapacities(
                    SecurityHelper.Context,
                    AuditHelper.Context);
                return engineCapacityData.EngineCapacities.ToArray();
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }
            
        }

      
        

        #endregion

        #region FuelType

        public string AddFuelType(JObject data)
        {
            try
            {
                //ILog logger = LogManager.GetLogger(typeof(ApiController));
                //logger.Debug("Add fuelType method!");
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                var fuelType = data.ToObject<FuelTypeRequestDto>();
                    //JsonConvert.DeserializeObject<List<FuelTypeRequestDto>>(data);
                var fuelTypeManagementService = ServiceFactory.GetFuelTypeManagementService();
                var result = fuelTypeManagementService.AddFuelType(fuelType, SecurityHelper.Context, AuditHelper.Context);
                logger.Info("FuelType Added");
                if (result.FuelTypeInsertion)
                {
                    return "OK";
                }
                return "Add fuelType failed!";
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                             ex.InnerException);
                return "Add fuelType failed!";
            }
        }

        public string UpdateFuelType(JObject data)
        {
            try
            {
                //ILog logger = LogManager.GetLogger(typeof(ApiController));
                //logger.Debug("Add fuelType method!");
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                var fuelType = data.ToObject<FuelTypeRequestDto>();
                var fuelTypeManagementService = ServiceFactory.GetFuelTypeManagementService();
                var result = fuelTypeManagementService.UpdateFuelType(fuelType, SecurityHelper.Context,
                    AuditHelper.Context);
                logger.Info("FuelType Added");
                if (result.FuelTypeInsertion)
                {
                    return "OK";
                }
                return "Add fuelType failed!";
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                             ex.InnerException);
                return "Add fuelType failed!";
            }
        }

        [HttpPost]
        public object GetFuelTypeById(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                var fuelTypeManagementService = ServiceFactory.GetFuelTypeManagementService();


                var fuelType = fuelTypeManagementService.GetFuelTypeById(Guid.Parse(data["fuelTypeId"].ToString()),
                    SecurityHelper.Context,
                    AuditHelper.Context);


                return fuelType;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }
            
        }

        [HttpPost]
        public object GetAllFuelTypes()
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                var fuelTypeManagementService = ServiceFactory.GetFuelTypeManagementService();

                var fuelTypeData = fuelTypeManagementService.GetFuelTypes(
                    SecurityHelper.Context,
                    AuditHelper.Context);
                return fuelTypeData.FuelTypes.ToArray();
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }
           
        }

        #endregion

        #region BodyType

        public string AddVehicleBodyType(JObject data)
        {
            try
            {
                //ILog logger = LogManager.GetLogger(typeof(ApiController));
                //logger.Debug("Add vehicleBodyType method!");
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                var vehicleBodyType = data.ToObject<VehicleBodyTypeRequestDto>();
                    //JsonConvert.DeserializeObject<List<VehicleBodyTypeRequestDto>>(data);
                var vehicleBodyTypeManagementService = ServiceFactory.GetVehicleBodyTypeManagementService();
                var result = vehicleBodyTypeManagementService.AddVehicleBodyType(vehicleBodyType, SecurityHelper.Context,
                    AuditHelper.Context);
                logger.Info("VehicleBodyType Added");
                if (result.VehicleBodyTypeInsertion)
                {
                    return "OK";
                }
                return "Add vehicleBodyType failed!";
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                             ex.InnerException);
                return "Add vehicleBodyType failed!";
            }
        }

        public string UpdateVehicleBodyType(JObject data)
        {
            try
            {
                //ILog logger = LogManager.GetLogger(typeof(ApiController));
                //logger.Debug("Add vehicleBodyType method!");
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                var vehicleBodyType = data.ToObject<VehicleBodyTypeRequestDto>();
                var vehicleBodyTypeManagementService = ServiceFactory.GetVehicleBodyTypeManagementService();
                var result = vehicleBodyTypeManagementService.UpdateVehicleBodyType(vehicleBodyType,
                    SecurityHelper.Context, AuditHelper.Context);
                logger.Info("VehicleBodyType Added");
                if (result.VehicleBodyTypeInsertion)
                {
                    return "OK";
                }
                return "Add vehicleBodyType failed!";
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                             ex.InnerException);
                return "Add vehicleBodyType failed!";
            }
        }

        [HttpPost]
        public object GetVehicleBodyTypeById(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                var vehicleBodyTypeManagementService = ServiceFactory.GetVehicleBodyTypeManagementService();


                var vehicleBodyType =
                    vehicleBodyTypeManagementService.GetVehicleBodyTypeById(
                        Guid.Parse(data["vehicleBodyTypeId"].ToString()),
                        SecurityHelper.Context,
                        AuditHelper.Context);


                return vehicleBodyType;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }
            
        }

        [HttpPost]
        public object GetAllVehicleBodyTypes()
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                var vehicleBodyTypeManagementService = ServiceFactory.GetVehicleBodyTypeManagementService();

                var vehicleBodyTypeData = vehicleBodyTypeManagementService.GetVehicleBodyTypes(
                    SecurityHelper.Context,
                    AuditHelper.Context);
                return vehicleBodyTypeData.VehicleBodyTypes.ToArray();
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }
            
        }

        #endregion

        #region VehicleColor

        public string AddVehicleColor(JObject data)
        {
            try
            {
                //ILog logger = LogManager.GetLogger(typeof(ApiController));
                //logger.Debug("Add VehicleColor method!");
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                var VehicleColor = data.ToObject<VehicleColorRequestDto>();
                    //JsonConvert.DeserializeObject<List<VehicleColorRequestDto>>(data);
                var VehicleColorManagementService = ServiceFactory.GetVehicleColorManagementService();
                var result = VehicleColorManagementService.AddVehicleColor(VehicleColor, SecurityHelper.Context,
                    AuditHelper.Context);
                logger.Info("VehicleColor Added");
                if (result.VehicleColorInsertion)
                {
                    return "OK";
                }
                return "Add VehicleColor failed!";
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                             ex.InnerException);
                return "Add VehicleColor failed!";
            }
        }

        public string UpdateVehicleColor(JObject data)
        {
            try
            {
                //ILog logger = LogManager.GetLogger(typeof(ApiController));
                //logger.Debug("Add vehicleColor method!");
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                var vehicleColor = data.ToObject<VehicleColorRequestDto>();
                var vehicleColorManagementService = ServiceFactory.GetVehicleColorManagementService();
                var result = vehicleColorManagementService.UpdateVehicleColor(vehicleColor, SecurityHelper.Context,
                    AuditHelper.Context);
                logger.Info("VehicleColor Added");
                if (result.VehicleColorInsertion)
                {
                    return "OK";
                }
                return "Add vehicleColor failed!";
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                             ex.InnerException);
                return "Add vehicleColor failed!";
            }
        }

        [HttpPost]
        public object GetVehicleColorById(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                var vehicleColorManagementService = ServiceFactory.GetVehicleColorManagementService();


                var vehicleColor =
                    vehicleColorManagementService.GetVehicleColorById(Guid.Parse(data["vehicleColorId"].ToString()),
                        SecurityHelper.Context,
                        AuditHelper.Context);


                return vehicleColor;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }
            
        }

        [HttpPost]
        public object GetAllVehicleColors()
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                var vehicleColorManagementService = ServiceFactory.GetVehicleColorManagementService();

                var vehicleColorData = vehicleColorManagementService.GetVehicleColors(
                    SecurityHelper.Context,
                    AuditHelper.Context);
                return vehicleColorData.VehicleColors.ToArray();
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }
            
        }

        #endregion

        #region TransmissionType

        public string AddTransmissionType(JObject data)
        {
            try
            {
                //ILog logger = LogManager.GetLogger(typeof(ApiController));
                //logger.Debug("Add TransmissionType method!");
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                var TransmissionType = data.ToObject<TransmissionTypeRequestDto>();
                    //JsonConvert.DeserializeObject<List<TransmissionTypeRequestDto>>(data);
                var TransmissionTypeManagementService = ServiceFactory.GetTransmissionTypeManagementService();
                var result = TransmissionTypeManagementService.AddTransmissionType(TransmissionType,
                    SecurityHelper.Context, AuditHelper.Context);
                logger.Info("TransmissionType Added");
                if (result.TransmissionTypeInsertion)
                {
                    return "OK";
                }
                return "Add TransmissionType failed!";
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                             ex.InnerException);
                return "Add TransmissionType failed!";
            }
        }

        public string UpdateTransmissionType(JObject data)
        {
            try
            {
                //ILog logger = LogManager.GetLogger(typeof(ApiController));
                //logger.Debug("Add TransmissionType method!");
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                var TransmissionType = data.ToObject<TransmissionTypeRequestDto>();
                var TransmissionTypeManagementService = ServiceFactory.GetTransmissionTypeManagementService();
                var result = TransmissionTypeManagementService.UpdateTransmissionType(TransmissionType,
                    SecurityHelper.Context, AuditHelper.Context);
                logger.Info("TransmissionType Added");
                if (result.TransmissionTypeInsertion)
                {
                    return "OK";
                }
                return "Add TransmissionType failed!";
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                             ex.InnerException);
                return "Add TransmissionType failed!";
            }
        }

        [HttpPost]
        public object GetTransmissionTypeById(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                var TransmissionTypeManagementService = ServiceFactory.GetTransmissionTypeManagementService();


                var TransmissionType =
                    TransmissionTypeManagementService.GetTransmissionTypeById(
                        Guid.Parse(data["TransmissionTypeId"].ToString()),
                        SecurityHelper.Context,
                        AuditHelper.Context);


                return TransmissionType;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }
            
        }

        [HttpPost]
        public object GetAllTransmissionTypes()
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                var TransmissionTypeManagementService = ServiceFactory.GetTransmissionTypeManagementService();

                var TransmissionTypeData = TransmissionTypeManagementService.GetTransmissionTypes(
                    SecurityHelper.Context,
                    AuditHelper.Context);
                return TransmissionTypeData.TransmissionTypes.ToArray();
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }
            
        }

        [HttpPost]
        public object GetTransmissionTechnology()
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                var TransmissionTypeManagementService = ServiceFactory.GetTransmissionTypeManagementService();
                var TransmissionTypeData =
                    TransmissionTypeManagementService.GetTransmissionTechnologies(SecurityHelper.Context,
                        AuditHelper.Context);
                return TransmissionTypeData.TransmissionTechnologies.ToArray();
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }
            
        }

        #endregion

        #region AspirationType

        public string AddVehicleAspirationType(JObject data)
        {
            try
            {
                //ILog logger = LogManager.GetLogger(typeof(ApiController));
                //logger.Debug("Add VehicleAspirationType method!");
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                var VehicleAspirationType = data.ToObject<VehicleAspirationTypeRequestDto>();
                    //JsonConvert.DeserializeObject<List<VehicleAspirationTypeRequestDto>>(data);
                var VehicleAspirationTypeManagementService = ServiceFactory.GetVehicleAspirationTypeManagementService();
                var result = VehicleAspirationTypeManagementService.AddVehicleAspirationType(VehicleAspirationType,
                    SecurityHelper.Context, AuditHelper.Context);
                logger.Info("VehicleAspirationType Added");
                if (result.VehicleAspirationTypeInsertion)
                {
                    return "OK";
                }
                return "Add VehicleAspirationType failed!";
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                             ex.InnerException);
                return "Add VehicleAspirationType failed!";
            }
        }

        public string UpdateVehicleAspirationType(JObject data)
        {
            try
            {
                //ILog logger = LogManager.GetLogger(typeof(ApiController));
                //logger.Debug("Add VehicleAspirationType method!");
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                var VehicleAspirationType = data.ToObject<VehicleAspirationTypeRequestDto>();
                var VehicleAspirationTypeManagementService = ServiceFactory.GetVehicleAspirationTypeManagementService();
                var result = VehicleAspirationTypeManagementService.UpdateVehicleAspirationType(VehicleAspirationType,
                    SecurityHelper.Context, AuditHelper.Context);
                logger.Info("VehicleAspirationType Added");
                if (result.VehicleAspirationTypeInsertion)
                {
                    return "OK";
                }
                return "Add VehicleAspirationType failed!";
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                             ex.InnerException);
                return "Add VehicleAspirationType failed!";
            }
        }

        [HttpPost]
        public object GetVehicleAspirationTypeById(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                var VehicleAspirationTypeManagementService = ServiceFactory.GetVehicleAspirationTypeManagementService();


                var VehicleAspirationType =
                    VehicleAspirationTypeManagementService.GetVehicleAspirationTypeById(
                        Guid.Parse(data["VehicleAspirationTypeId"].ToString()),
                        SecurityHelper.Context,
                        AuditHelper.Context);


                return VehicleAspirationType;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }
            
        }

        [HttpPost]
        public object GetAllVehicleAspirationTypes()
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                var VehicleAspirationTypeManagementService = ServiceFactory.GetVehicleAspirationTypeManagementService();

                var VehicleAspirationTypeData = VehicleAspirationTypeManagementService.GetVehicleAspirationTypes(
                    SecurityHelper.Context,
                    AuditHelper.Context);
                return VehicleAspirationTypeData.VehicleAspirationTypes.ToArray();
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }
            
        }

        [HttpPost]
        public bool IsExsistingAspirationTypesByCode(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                var VehicleAspirationTypeManagementService = ServiceFactory.GetVehicleAspirationTypeManagementService();

                var isExsists =
                    VehicleAspirationTypeManagementService.IsExsistingAspirationTypesByCode(
                        Guid.Parse(data["Id"].ToString()), data["AspirationTypeCode"].ToString(),
                        SecurityHelper.Context,
                        AuditHelper.Context);
                return isExsists;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return false;
            }
            
        }

        #endregion

        #region Kilo Watt

        public string AddVehicleHorsePower(JObject data)
        {
            try
            {
                //ILog logger = LogManager.GetLogger(typeof(ApiController));
                //logger.Debug("Add VehicleHorsePower method!");
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                var VehicleHorsePower = data.ToObject<VehicleHorsePowerRequestDto>();
                    //JsonConvert.DeserializeObject<List<VehicleHorsePowerRequestDto>>(data);
                var VehicleHorsePowerManagementService = ServiceFactory.GetVehicleHorsePowerManagementService();
                var result = VehicleHorsePowerManagementService.AddVehicleHorsePower(VehicleHorsePower,
                    SecurityHelper.Context, AuditHelper.Context);
                logger.Info("VehicleHorsePower Added");
                if (result.VehicleHorsePowerInsertion)
                {
                    return "OK";
                }
                return "Add VehicleHorsePower failed!";
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                             ex.InnerException);
                return "Add VehicleHorsePower failed!";
            }
        }

        public string UpdateVehicleHorsePower(JObject data)
        {
            try
            {
                //ILog logger = LogManager.GetLogger(typeof(ApiController));
                //logger.Debug("Add VehicleHorsePower method!");
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                var VehicleHorsePower = data.ToObject<VehicleHorsePowerRequestDto>();
                var VehicleHorsePowerManagementService = ServiceFactory.GetVehicleHorsePowerManagementService();
                var result = VehicleHorsePowerManagementService.UpdateVehicleHorsePower(VehicleHorsePower,
                    SecurityHelper.Context, AuditHelper.Context);
                logger.Info("VehicleHorsePower Added");
                if (result.VehicleHorsePowerInsertion)
                {
                    return "OK";
                }
                return "Add VehicleHorsePower failed!";
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                             ex.InnerException);
                return "Add VehicleHorsePower failed!";
            }
        }

        [HttpPost]
        public object GetVehicleHorsePowerById(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                var VehicleHorsePowerManagementService = ServiceFactory.GetVehicleHorsePowerManagementService();
                var VehicleHorsePower =
                    VehicleHorsePowerManagementService.GetVehicleHorsePowerById(
                        Guid.Parse(data["VehicleHorsePowerId"].ToString()), SecurityHelper.Context, AuditHelper.Context);
                return VehicleHorsePower;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }
            
        }

        [HttpPost]
        public object GetAllVehicleHorsePowers()
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                var VehicleHorsePowerManagementService = ServiceFactory.GetVehicleHorsePowerManagementService();
                var VehicleHorsePowerData = VehicleHorsePowerManagementService.GetVehicleHorsePowers(
                    SecurityHelper.Context, AuditHelper.Context);
                return VehicleHorsePowerData.VehicleHorsePowers.ToArray();
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }
            
        }

        [HttpPost]
        public bool IsExsistingVehicleKiloWattByKiloWatt(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                var VehicleAspirationTypeManagementService = ServiceFactory.GetVehicleAspirationTypeManagementService();

                var isExsists =
                    VehicleAspirationTypeManagementService.IsExsistingVehicleKiloWattByKiloWatt(
                        Guid.Parse(data["Id"].ToString()), data["KiloWatt"].ToString(),
                        SecurityHelper.Context,
                        AuditHelper.Context);
                return isExsists;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return false;
            }
            
        }

        #endregion

        #region Horse Power

        public string AddVehicleKiloWatt(JObject data)
        {
            try
            {
                //ILog logger = LogManager.GetLogger(typeof(ApiController));
                //logger.Debug("Add VehicleKiloWatt method!");
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                var VehicleKiloWatt = data.ToObject<VehicleKiloWattRequestDto>();
                    //JsonConvert.DeserializeObject<List<VehicleKiloWattRequestDto>>(data);
                var VehicleKiloWattManagementService = ServiceFactory.GetVehicleKiloWattManagementService();
                var result = VehicleKiloWattManagementService.AddVehicleKiloWatt(VehicleKiloWatt, SecurityHelper.Context,
                    AuditHelper.Context);
                logger.Info("VehicleKiloWatt Added");
                if (result.VehicleKiloWattInsertion)
                {
                    return "OK";
                }
                return "Add VehicleKiloWatt failed!";
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                             ex.InnerException);
                return "Add VehicleKiloWatt failed!";
            }
        }

        public string UpdateVehicleKiloWatt(JObject data)
        {
            try
            {
                //ILog logger = LogManager.GetLogger(typeof(ApiController));
                //logger.Debug("Add VehicleKiloWatt method!");
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                var VehicleKiloWatt = data.ToObject<VehicleKiloWattRequestDto>();
                var VehicleKiloWattManagementService = ServiceFactory.GetVehicleKiloWattManagementService();
                var result = VehicleKiloWattManagementService.UpdateVehicleKiloWatt(VehicleKiloWatt,
                    SecurityHelper.Context, AuditHelper.Context);
                logger.Info("VehicleKiloWatt Added");
                if (result.VehicleKiloWattInsertion)
                {
                    return "OK";
                }
                return "Add VehicleKiloWatt failed!";
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                             ex.InnerException);
                return "Add VehicleKiloWatt failed!";
            }
        }

        [HttpPost]
        public object GetVehicleKiloWattById(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                var VehicleKiloWattManagementService = ServiceFactory.GetVehicleKiloWattManagementService();
                var VehicleKiloWatt =
                    VehicleKiloWattManagementService.GetVehicleKiloWattById(
                        Guid.Parse(data["VehicleKiloWattId"].ToString()), SecurityHelper.Context, AuditHelper.Context);
                return VehicleKiloWatt;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }
            
        }

        [HttpPost]
        public object GetAllVehicleKiloWatts()
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                var VehicleKiloWattManagementService = ServiceFactory.GetVehicleKiloWattManagementService();
                var VehicleKiloWattData = VehicleKiloWattManagementService.GetVehicleKiloWatts(SecurityHelper.Context,
                    AuditHelper.Context);
                return VehicleKiloWattData.VehicleKiloWatts.ToArray();
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }
            
        }

        [HttpPost]
        public bool IsExsistingHorsePowerByHorsePower(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                var VehicleAspirationTypeManagementService = ServiceFactory.GetVehicleAspirationTypeManagementService();

                var isExsists =
                    VehicleAspirationTypeManagementService.IsExsistingHorsePowerByHorsePower(
                        Guid.Parse(data["Id"].ToString()), data["HorsePower"].ToString(),
                        SecurityHelper.Context,
                        AuditHelper.Context);
                return isExsists;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return false;
            }
            
        }

        #endregion

        #region Vehicle Weight 

        [HttpPost]
        public object GetAllVehicleWeight()
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                var vehicleWeightManagementService = ServiceFactory.GetVehicleWeightManagementService();

                var VehicleWeightsData = vehicleWeightManagementService.GetAllVehicleWeight(
                    SecurityHelper.Context,
                    AuditHelper.Context);
                return VehicleWeightsData.VehicleWeights.ToArray();
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }

        }      


        public object SubmitVehicleWeight(JObject data)
        {
            object response = null;
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                var VehicleWeightData = data.ToObject<VehicleWeightRequestDto>();
                var vehicleWeightManagementService = ServiceFactory.GetVehicleWeightManagementService();
                response = vehicleWeightManagementService.SubmitVehicleWeight(VehicleWeightData,
                    SecurityHelper.Context, AuditHelper.Context);
                
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                             ex.InnerException);
            }
            return response;
        }


        [HttpPost]
        public object GetVehicleWeightById(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                var vehicleWeightManagementService = ServiceFactory.GetVehicleWeightManagementService();
                var vehicleWeight = vehicleWeightManagementService.GetVehicleWeightById(Guid.Parse(data["Id"].ToString()),
                    SecurityHelper.Context,
                    AuditHelper.Context);


                return vehicleWeight;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }

        }

        #endregion
    }
}