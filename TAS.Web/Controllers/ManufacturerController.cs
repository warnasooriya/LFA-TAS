
using Newtonsoft.Json.Linq;
using NLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Description;
using TAS.DataTransfer.Requests;
using TAS.DataTransfer.Responses;
using TAS.Services;
using TAS.Web.Common;

namespace TAS.Web.Controllers
{
    public class ManufacturerController : ApiController
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        [HttpPost]
        public string AddManufacturer(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                ManufacturerRequestDto Manufacturer = data.ToObject<ManufacturerRequestDto>();
                IManufacturerManagementService ManufacturerManagementService = ServiceFactory.GetManufacturerManagementService();
                ManufacturerRequestDto result = ManufacturerManagementService.AddManufacturer(Manufacturer, SecurityHelper.Context, AuditHelper.Context);
                logger.Info("Manufacturer Added");
                if (result.ManufacturerInsertion)
                {
                    return "OK";
                }
                else
                {
                    return "Add Manufacturer failed!";
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return "Add Manufacturer failed!";
            }
           
        }

        [HttpPost]
        public string UpdateManufacturer(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                ManufacturerRequestDto Manufacturer = data.ToObject<ManufacturerRequestDto>();
                IManufacturerManagementService ManufacturerManagementService = ServiceFactory.GetManufacturerManagementService();
                ManufacturerRequestDto result = ManufacturerManagementService.UpdateManufacturer(Manufacturer, SecurityHelper.Context, AuditHelper.Context);
                logger.Info("Manufacturer Added");
                if (result.ManufacturerInsertion)
                {
                    return "OK";
                }
                else
                {
                    return "Add Manufacturer failed!";
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return "Add Manufacturer failed!";
            }
            
        }

        [HttpPost]
        public object GetManufacturerById(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            IManufacturerManagementService ManufacturerManagementService = ServiceFactory.GetManufacturerManagementService();

            ManufacturerResponseDto Manufacturer = ManufacturerManagementService.GetManufacturerById(Guid.Parse(data["Id"].ToString()),
                SecurityHelper.Context,
                AuditHelper.Context);
            return Manufacturer;
        }

        [HttpPost]
        public object GetAllManufacturers(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            IManufacturerManagementService ManufacturerManagementService = ServiceFactory.GetManufacturerManagementService();

            ManufacturesResponseDto ManufacturerData = ManufacturerManagementService.GetAllManufatures(
            SecurityHelper.Context,
            AuditHelper.Context);
            return ManufacturerData.Manufactures.FindAll(m => m.ComodityTypes.Contains(Guid.Parse(data["Id"].ToString()))).OrderBy(m=>m.ManufacturerName).ToArray();
        }

        [HttpPost]
        public object GetAllManufacturersByCommodityTypeId(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            IManufacturerManagementService ManufacturerManagementService = ServiceFactory.GetManufacturerManagementService();

            ManufacturesResponseDto ManufacturerData = ManufacturerManagementService.GetAllManufatures(
            SecurityHelper.Context,
            AuditHelper.Context);
            return ManufacturerData.Manufactures.FindAll(m => m.ComodityTypes.Contains(Guid.Parse(data["Id"].ToString()))).OrderBy(o=>o.ManufacturerName).ToArray();
        }

    }
}