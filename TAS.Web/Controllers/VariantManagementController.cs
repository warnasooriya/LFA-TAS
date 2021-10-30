using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using TAS.Web.Common;
using TAS.Services;
using TAS.DataTransfer.Responses;
using TAS.Services.Entities;
using TAS.DataTransfer.Requests;
using Newtonsoft.Json;
using NLog;
using System.Reflection;

namespace TAS.Web.Controllers
{
    public class VariantManagementController : ApiController
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        //private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [HttpPost]
        public string AddVariant(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                VariantRequestDto Variant = data.ToObject<VariantRequestDto>();
                IVariantManagementService VariantManagementService = ServiceFactory.GetVariantManagementService();
                VariantRequestDto result = VariantManagementService.AddVariant(Variant, SecurityHelper.Context, AuditHelper.Context);
                logger.Info("Vehicle Details Added");
                if (result.VariantInsertion)
                {
                    return "OK";
                }
                else
                {
                    return "Add Vehicle Details failed!";
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return "Add Vehicle Details failed!";
            }
            
        }

        [HttpPost]
        public string UpdateVariant(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                VariantRequestDto Variant = data.ToObject<VariantRequestDto>();
                IVariantManagementService VariantManagementService = ServiceFactory.GetVariantManagementService();
                VariantRequestDto result = VariantManagementService.UpdateVariant(Variant, SecurityHelper.Context, AuditHelper.Context);
                logger.Info("Vehicle Details Added");
                if (result.VariantInsertion)
                {
                    return "OK";
                }
                else
                {
                    return "Add Vehicle Details failed!";
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return "Add Vehicle Details failed!";
            }
            
        }

        [HttpPost]
        public object GetVariantById(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            IVariantManagementService VariantManagementService = ServiceFactory.GetVariantManagementService();
            
            VariantResponseDto Variant = VariantManagementService.GetVariantById(Guid.Parse(data["Id"].ToString()),
                SecurityHelper.Context,
                AuditHelper.Context);
            return Variant;
        }
        
        [HttpPost]
        public object GetAllVariant()
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            IVariantManagementService VariantManagementService = ServiceFactory.GetVariantManagementService();

            VariantsResponseDto VariantData = VariantManagementService.GetVariants(
            SecurityHelper.Context,
            AuditHelper.Context);
            return VariantData.Variants.ToArray();
        }

        [HttpPost]
        public object GetAllVariantByModelId(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            IVariantManagementService VariantManagementService = ServiceFactory.GetVariantManagementService();

            VariantsResponseDto VariantData = VariantManagementService.GetVariants(
            SecurityHelper.Context,
            AuditHelper.Context);
            return VariantData.Variants.FindAll(v => v.ModelId == Guid.Parse(data["Id"].ToString())).ToArray();
        }
    }
}
