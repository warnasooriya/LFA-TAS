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
    public class ReinsurerReceiptManagementController : ApiController
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        //private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region ClaimBordx
        
        [HttpPost]
        public object GetBordxsById(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            IReinsurerReceiptManagementService ReinsurerReceiptManagementService = ServiceFactory.GetReinsurerReceiptManagementService();

            ClaimBordxsResponseDto Reinsurer = ReinsurerReceiptManagementService.GetBordxsById(Guid.Parse(data["ReId"].ToString()), Guid.Parse(data["InId"].ToString()), Convert.ToInt32(data["Year"].ToString()),
                SecurityHelper.Context,
                AuditHelper.Context);
            return Reinsurer.ClaimBordxs.ToArray();
        }

        [HttpPost]
        public object GetAllBordxs()
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            IReinsurerReceiptManagementService ReinsurerReceiptManagementService = ServiceFactory.GetReinsurerReceiptManagementService();
            
            ClaimBordxsResponseDto FaultsData = ReinsurerReceiptManagementService.GetBordxs(SecurityHelper.Context, AuditHelper.Context);
            return FaultsData.ClaimBordxs.ToArray();
        }

        #endregion
        
        #region BordxPayment
        [HttpPost]
        public string AddBordxPayment(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                BordxPaymentRequestDto BordxPayment = data["BordxPayment"].ToObject<BordxPaymentRequestDto>();
                BordxPayment.ClaimBordxID = Guid.Parse(data["ClaimBordxId"].ToString());
                IReinsurerReceiptManagementService ReinsurerReceiptManagementService = ServiceFactory.GetReinsurerReceiptManagementService();
                BordxPaymentRequestDto result = ReinsurerReceiptManagementService.AddBordxPayment(BordxPayment, SecurityHelper.Context, AuditHelper.Context);
                logger.Info("BordxPayment  Added");
                if (result.BordxPaymentInsertion)
                {
                    return "OK";
                }
                else
                {
                    return "Add BordxPayment  failed!";
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return "Add BordxPayment  failed!";
            }
            
        }

        [HttpPost]
        public string UpdateBordxPayment(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                BordxPaymentRequestDto BordxPayment = data["BordxPayment"].ToObject<BordxPaymentRequestDto>();
                BordxPayment.ClaimBordxID = Guid.Parse(data["ClaimBordxId"].ToString());
                IReinsurerReceiptManagementService ReinsurerReceiptManagementService = ServiceFactory.GetReinsurerReceiptManagementService();
                BordxPaymentRequestDto result = ReinsurerReceiptManagementService.UpdateBordxPayment(BordxPayment, SecurityHelper.Context, AuditHelper.Context);
                logger.Info("BordxPayment  Added");
                if (result.BordxPaymentInsertion)
                {
                    return "OK";
                }
                else
                {
                    return "Add BordxPayment  failed!";
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return "Add BordxPayment  failed!";
            }
            
        }

        [HttpPost]
        public object GetBordxPaymmentById(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            IReinsurerReceiptManagementService ReinsurerReceiptManagementService = ServiceFactory.GetReinsurerReceiptManagementService();

            BordxPaymentsResponseDto BordxPayment = ReinsurerReceiptManagementService.GetClaimBordxPaymentsById(Guid.Parse(data["ClaimBordxId"].ToString()),
                SecurityHelper.Context,
                AuditHelper.Context);
            return BordxPayment.BordxPayments.ToArray();
        }
        #endregion

    }
    //public class ReinsurerConsortiumData
    //{
    //    public List<ReinsurerConsortiumRequestDto> Consortiums { get; set; }
    //}
}
