using Newtonsoft.Json.Linq;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TAS.DataTransfer.Responses;
using TAS.Services;
using TAS.Web.Common;

namespace TAS.Web.Controllers
{
    public class PaymentController : ApiController
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        [HttpPost]
        public PaymentModesResponseDto GetAllPaymentModes()
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            IPaymentManagementService PaymentManagementService = ServiceFactory.GetPaymentManagementService();
            return PaymentManagementService.GetAllPaymentModes(SecurityHelper.Context,AuditHelper.Context);
        }

        [HttpPost]
        public PaymentTypesResponseDto GetAllPaymentTypesByPaymentModeId(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            IPaymentManagementService PaymentManagementService = ServiceFactory.GetPaymentManagementService();
            return PaymentManagementService.GetAllPaymentTypesByPaymentModeId(Guid.Parse(data["PaymentModeId"].ToString()), SecurityHelper.Context, AuditHelper.Context);
        }
    }
}
