using TAS.DataTransfer.Requests;
using TAS.DataTransfer.Responses;
using TAS.Services;
using TAS.Web.Common;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NLog;

namespace TAS.Web.Controllers
{
    public class TPABranchController : ApiController
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        //private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
       
        [HttpPost]
        public object GetTPABranchesByTpaId(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            Guid TPAId = Guid.Parse(data["tpaId"].ToString());
            ITPABranchManagementService ITPABranchManagementService = ServiceFactory.GetTPABranchManagementService();
            TPABranchesResponseDto TPABranchesData = ITPABranchManagementService.GetTPABranchesByTPAId(
            TPAId,
            SecurityHelper.Context,
            AuditHelper.Context);

            return TPABranchesData.TPABranches.ToArray();
        }

        [HttpPost]

        public object GetTimeZoneById(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            Guid TimeZoneId = Guid.Parse(data["TimeZone"].ToString());
            ITPABranchManagementService ITPABranchManagementService = ServiceFactory.GetTPABranchManagementService();
            object TimezoneData = ITPABranchManagementService.GetTimeZoneById(
            TimeZoneId,
            SecurityHelper.Context,
            AuditHelper.Context);

            return TimezoneData;
        }

        [HttpPost]
        public object GetTPABranches(JObject data)
        {
            //Guid TPAId = Guid.Parse(data["tpaId"].ToString());
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            ITPABranchManagementService ITPABranchManagementService = ServiceFactory.GetTPABranchManagementService();
            TPABranchesResponseDto TPABranchesData = ITPABranchManagementService.GetTPABranches(
            SecurityHelper.Context,
            AuditHelper.Context);

            return TPABranchesData.TPABranches.ToArray();
        }

        [HttpPost]
        public object GetTpaBranchesByUserId(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            Guid loggedUserId = Guid.Parse(data["Id"].ToString());
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            ITPABranchManagementService TPABranchManagementService = ServiceFactory.GetTPABranchManagementService();
            return TPABranchManagementService.GetTPABranchesBySystemUserId(loggedUserId, SecurityHelper.Context, AuditHelper.Context).TPABranches
                .Select(a => new { 
                    a.Id,
                    a.BranchCode,
                    a.BranchName
                }).ToArray();
        }

        [HttpPost]
        public bool SaveTPABranch(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            TPABranchRequestDto TPAData = new TPABranchRequestDto();
            TPAData.Address = data["Address"].ToString();
            TPAData.BranchCode = data["BranchCode"].ToString();
            TPAData.BranchName = data["BranchName"].ToString();
            TPAData.CityId = Guid.Parse(data["CityId"].ToString());
            TPAData.ContryId =Guid.Parse(data["ContryId"].ToString());
            TPAData.Id = Guid.NewGuid();
            TPAData.IsHeadOffice = data["IsHeadOffice"].ToString() == "" ? false : Convert.ToBoolean(data["IsHeadOffice"]);
            TPAData.State = data["State"].ToString();
           // var format = "g";
          //  var culture = CultureInfo.CurrentCulture;
            TPAData.TimeZone = Guid.Parse(data["TimeZone"].ToString());
            TPAData.TpaId = Guid.Parse(data["TpaId"].ToString());

            ITPABranchManagementService ITPABranchManagementService = ServiceFactory.GetTPABranchManagementService();
            bool status = ITPABranchManagementService.SaveTPABranch(
            TPAData,
            SecurityHelper.Context,
            AuditHelper.Context);
            return status;
        }

        public object UpdateTPABranch(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            TPABranchRequestDto TPAData = new TPABranchRequestDto();
            TPAData.Address = data["Address"].ToString();
            TPAData.BranchCode = data["BranchCode"].ToString();
            TPAData.BranchName = data["BranchName"].ToString();
            TPAData.CityId = Guid.Parse(data["CityId"].ToString());
            TPAData.ContryId = Guid.Parse(data["ContryId"].ToString());
            TPAData.Id = Guid.Parse(data["Id"].ToString());
            TPAData.IsHeadOffice = Convert.ToBoolean(data["IsHeadOffice"]);
            TPAData.State = data["State"].ToString();
            //var format = "g";
            //var culture = CultureInfo.CurrentCulture;
            TPAData.TimeZone = Guid.Parse(data["TimeZone"].ToString());
            TPAData.TpaId = Guid.Parse(data["TpaId"].ToString());

            ITPABranchManagementService ITPABranchManagementService = ServiceFactory.GetTPABranchManagementService();
            bool status = ITPABranchManagementService.UpdateTPABranch(
            TPAData,
            SecurityHelper.Context,
            AuditHelper.Context);
            return status;
           
        }


        [HttpPost]
        public object GetAllTimezones()
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            ITPABranchManagementService TPABranchManagementService = ServiceFactory.GetTPABranchManagementService();
            return TPABranchManagementService.GetAllTimezones(SecurityHelper.Context, AuditHelper.Context);
        }

        [HttpPost]
        public bool IsExsistingTpaByCode(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            ITPABranchManagementService tpaBranchManagementService = ServiceFactory.GetTPABranchManagementService();
            bool isExsists = tpaBranchManagementService.IsExsistingTpaByCode(Guid.Parse(data["Id"].ToString()), data["BranchCode"].ToString(),
                SecurityHelper.Context,
                AuditHelper.Context);
            return isExsists;
        }

       

    }
}
