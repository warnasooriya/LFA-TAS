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
    public class InsurerManagementController : ApiController
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        //private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Insurer
        [HttpPost]
        public string AddInsurer(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                InsurerRequestDto Insurer = data.ToObject<InsurerRequestDto>();
                IInsurerManagementService InsurerManagementService = ServiceFactory.GetInsurerManagementService();
                InsurerRequestDto result = InsurerManagementService.AddInsurer(Insurer, SecurityHelper.Context, AuditHelper.Context);
                logger.Info("Insurer  Added");
                if (result.InsurerInsertion)
                {
                    return "OK";
                }
                else
                {
                    return "Add Insurer  failed!";
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return "Add Insurer  failed!";
            }
            
        }

        [HttpPost]
        public string UpdateInsurer(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                InsurerRequestDto Insurer = data.ToObject<InsurerRequestDto>();
                IInsurerManagementService InsurerManagementService = ServiceFactory.GetInsurerManagementService();
                InsurerRequestDto result = InsurerManagementService.UpdateInsurer(Insurer, SecurityHelper.Context, AuditHelper.Context);
                logger.Info("Insurer  Added");
                if (result.InsurerInsertion)
                {
                    return "OK";
                }
                else
                {
                    return "Add Insurer  failed!";
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return "Add Insurer  failed!";
            }
            
        }

        [HttpPost]
        public object GetInsurerById(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            IInsurerManagementService InsurerManagementService = ServiceFactory.GetInsurerManagementService();
            
            InsurerResponseDto Insurer = InsurerManagementService.GetInsurerById(Guid.Parse(data["Id"].ToString()),
                SecurityHelper.Context,
                AuditHelper.Context);
            return Insurer;
        }
        
        [HttpPost]
        public object GetAllInsurers()
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            IInsurerManagementService InsurerManagementService = ServiceFactory.GetInsurerManagementService();

            InsurersResponseDto InsurerData = InsurerManagementService.GetInsurers(
            SecurityHelper.Context,
            AuditHelper.Context);
            return InsurerData.Insurers.ToArray();
        }

        [HttpPost]
        public object GetAllInsurersByCountryId(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            IInsurerManagementService InsurerManagementService = ServiceFactory.GetInsurerManagementService();

            InsurersResponseDto InsurerData = InsurerManagementService.GetInsurers(
            SecurityHelper.Context,
            AuditHelper.Context);
            List<InsurerResponseDto> temp = new List<InsurerResponseDto>();
            foreach (var item in InsurerData.Insurers)
            {
                if (InsurerManagementService.GetInsurerById(item.Id, SecurityHelper.Context, AuditHelper.Context).Countries.Contains(Guid.Parse(data["Id"].ToString())))
                    temp.Add(item);
            }

            return temp;
        }
        #endregion

        #region Consortium
        [HttpPost]
        public string AddorUpdateInsurerConsortiums(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                InsurerConsortiumData InsurerConsortiums = data.ToObject<InsurerConsortiumData>();
                IInsurerManagementService InsurerConsortiumManagementService = ServiceFactory.GetInsurerManagementService();
                foreach (var item in InsurerConsortiums.Consortiums)
                {
                    if (item.Id2.ToString() == "10000000-0000-0000-0000-000000000001")
                    {
                        InsurerConsortiumRequestDto resultA = InsurerConsortiumManagementService.AddInsurerConsortium(item, SecurityHelper.Context, AuditHelper.Context);
                    }
                    else
                    {
                        InsurerConsortiumRequestDto resultU = InsurerConsortiumManagementService.UpdateInsurerConsortium(item, SecurityHelper.Context, AuditHelper.Context);
                    }
                    logger.Info("Insurer Consortium  Added");
                }
                //if (result.InsurerConsortiumInsertion)
                //{
                return "OK";
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return "";
            }
            
            //}
            //else
            //{
            //    return "Add Insurer Consortium  failed!";
            //}

        }

        //[HttpPost]
        //public string UpdateInsurerConsortiums(JObject data)
        //{
        //    ILog logger = LogManager.GetLogger(typeof(ApiController));
        //    logger.Debug("Add Insurer Consortium  method!");

        //    List<InsurerConsortiumRequestDto> InsurerConsortium = data.ToObject<List<InsurerConsortiumRequestDto>>();
        //    IInsurerManagementService InsurerConsortiumManagementService = ServiceFactory.GetInsurerManagementService();
        //    List<InsurerConsortiumRequestDto> result = InsurerConsortiumManagementService.UpdateInsurerConsortium(InsurerConsortium, SecurityHelper.Context, AuditHelper.Context);
        //    logger.Info("Insurer Consortium  Added");
        //    if (result.InsurerConsortiumInsertion)
        //    {
        //        return "OK";
        //    }
        //    else
        //    {
        //        return "Add Insurer Consortium  failed!";
        //    }
        //}

        [HttpPost]
        public object GetInsurerConsortiumById(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            IInsurerManagementService InsurerConsortiumManagementService = ServiceFactory.GetInsurerManagementService();

            InsurerConsortiumResponseDto InsurerConsortium = InsurerConsortiumManagementService.GetInsurerConsortiumById(Guid.Parse(data["Id"].ToString()),
                SecurityHelper.Context,
                AuditHelper.Context);
            return InsurerConsortium;
        }

        [HttpPost]
        public object GetAllInsurerConsortiums()
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            IInsurerManagementService InsurerConsortiumManagementService = ServiceFactory.GetInsurerManagementService();

            InsurerConsortiumsResponseDto InsurerConsortiumData = InsurerConsortiumManagementService.GetInsurerConsortiums(
            SecurityHelper.Context,
            AuditHelper.Context);
            return InsurerConsortiumData.InsurerConsortiums.ToArray();
        }

        [HttpPost]
        public object GetInsurerConsortiumsByInsurerId(JObject data)
        {

            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            IInsurerManagementService InsurerConsortiumManagementService = ServiceFactory.GetInsurerManagementService();

            InsurerConsortiumsResponseDto InsurerConsortiumData = InsurerConsortiumManagementService.GetInsurerConsortiums(
            SecurityHelper.Context,
            AuditHelper.Context);
            return InsurerConsortiumData.InsurerConsortiums.FindAll(c => c.ParentInsurerId == Guid.Parse(data["Id"].ToString())).ToArray();
        }
        #endregion
    }
    public class InsurerConsortiumData
    {
        public List<InsurerConsortiumRequestDto> Consortiums { get; set; }
    }
}
