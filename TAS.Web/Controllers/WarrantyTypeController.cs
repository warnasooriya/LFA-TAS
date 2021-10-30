
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
    public class WarrantyTypeController : ApiController
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        [HttpPost]
        public string AddWarrantyType(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                WarrantyTypeRequestDto WarrantyType = data.ToObject<WarrantyTypeRequestDto>();
                IWarrantyTypeManagementService WarrantyTypeManagementService = ServiceFactory.GetWarrantyTypeManagementService();
                WarrantyTypeRequestDto result = WarrantyTypeManagementService.AddWarrantyType(WarrantyType, SecurityHelper.Context, AuditHelper.Context);
                logger.Info("WarrantyType Added");
                if (result.WarrantyTypeInsertion)
                {
                    return "OK";
                }
                else
                {
                    return "Add WarrantyType failed!";
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return "Add WarrantyType failed!";
            }
            
        }

        [HttpPost]
        public string UpdateWarrantyType(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                WarrantyTypeRequestDto WarrantyType = data.ToObject<WarrantyTypeRequestDto>();
                IWarrantyTypeManagementService WarrantyTypeManagementService = ServiceFactory.GetWarrantyTypeManagementService();
                WarrantyTypeRequestDto result = WarrantyTypeManagementService.UpdateWarrantyType(WarrantyType, SecurityHelper.Context, AuditHelper.Context);
                logger.Info("WarrantyType Added");
                if (result.WarrantyTypeInsertion)
                {
                    return "OK";
                }
                else
                {
                    return "Add WarrantyType failed!";
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return "Add WarrantyType failed!";
            }
            
        }

        [HttpPost]
        public object GetWarrantyTypeById(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            IWarrantyTypeManagementService WarrantyTypeManagementService = ServiceFactory.GetWarrantyTypeManagementService();

            WarrantyTypeResponseDto WarrantyType = WarrantyTypeManagementService.GetWarrantyTypeById(Guid.Parse(data["Id"].ToString()),
                SecurityHelper.Context,
                AuditHelper.Context);
            return WarrantyType;
        }

        [HttpPost]
        public object GetAllWarrantyTypes()
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            IWarrantyTypeManagementService WarrantyTypeManagementService = ServiceFactory.GetWarrantyTypeManagementService();

            WarrantyTypesResponseDto WarrantyTypeData = WarrantyTypeManagementService.GetWarrantyTypes(
            SecurityHelper.Context,
            AuditHelper.Context);
            return WarrantyTypeData.WarrantyTypes.ToArray();
        }

      
    }
}