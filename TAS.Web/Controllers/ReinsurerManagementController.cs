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
    public class ReinsurerManagementController : ApiController
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        //private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Reinsurer
        [HttpPost]
        public string AddReinsurer(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                ReinsurerRequestDto Reinsurer = data.ToObject<ReinsurerRequestDto>();
                IReinsurerManagementService ReinsurerManagementService = ServiceFactory.GetReinsurerManagementService();
                ReinsurerRequestDto result = ReinsurerManagementService.AddReinsurer(Reinsurer, SecurityHelper.Context, AuditHelper.Context);
                logger.Info("Reinsurer  Added");
                if (result.ReinsurerInsertion)
                {
                    return "OK";
                }
                else
                {
                    return "Add Reinsurer  failed!";
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return "Add Reinsurer  failed!";
            }

        }

        [HttpPost]
        public string UpdateReinsurer(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                ReinsurerRequestDto Reinsurer = data.ToObject<ReinsurerRequestDto>();
                IReinsurerManagementService ReinsurerManagementService = ServiceFactory.GetReinsurerManagementService();
                ReinsurerRequestDto result = ReinsurerManagementService.UpdateReinsurer(Reinsurer, SecurityHelper.Context, AuditHelper.Context);
                logger.Info("Reinsurer  Added");
                if (result.ReinsurerInsertion)
                {
                    return "OK";
                }
                else
                {
                    return "Add Reinsurer  failed!";
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return "Add Reinsurer  failed!";
            }

        }

        [HttpPost]
        public object GetReinsurerById(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            IReinsurerManagementService ReinsurerManagementService = ServiceFactory.GetReinsurerManagementService();

            ReinsurerResponseDto Reinsurer = ReinsurerManagementService.GetReinsurerById(Guid.Parse(data["Id"].ToString()),
                SecurityHelper.Context,
                AuditHelper.Context);
            return Reinsurer;
        }

        [HttpPost]
        public object GetAllReinsurers()
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            IReinsurerManagementService ReinsurerManagementService = ServiceFactory.GetReinsurerManagementService();

            ReinsurersResponseDto ReinsurerData = ReinsurerManagementService.GetReinsurers(
            SecurityHelper.Context,
            AuditHelper.Context);
            return ReinsurerData.Reinsurers.ToArray();
        }

        [HttpPost]
        public bool SaveReinsurerStaff(ReinsurerStaffAddRequestDto data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            IReinsurerManagementService ReinsurerManagementService = ServiceFactory.GetReinsurerManagementService();

            return ReinsurerManagementService.SaveReinsurerStaff(data,
            SecurityHelper.Context,
            AuditHelper.Context);

        }
        #endregion

        #region Contract
        [HttpPost]
        public string AddReinsurerContract(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                ReinsurerContractRequestDto ReinsurerContract = data.ToObject<ReinsurerContractRequestDto>();
                IReinsurerContractManagementService ReinsurerContractManagementService = ServiceFactory.GetReinsurerContractManagementService();
                ReinsurerContractRequestDto result = ReinsurerContractManagementService.AddReinsurerContract(ReinsurerContract, SecurityHelper.Context, AuditHelper.Context);
                logger.Info("ReinsurerContract  Added");
                if (result.ReinsurerContractInsertion)
                {
                    return "OK";
                }
                else
                {
                    return "Contract No Exist!";
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return "Add ReinsurerContract  failed!";
            }

        }

        [HttpPost]
        public string UpdateReinsurerContract(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                ReinsurerContractRequestDto ReinsurerContract = data.ToObject<ReinsurerContractRequestDto>();
                IReinsurerContractManagementService ReinsurerContractManagementService = ServiceFactory.GetReinsurerContractManagementService();
                ReinsurerContractRequestDto result = ReinsurerContractManagementService.UpdateReinsurerContract(ReinsurerContract, SecurityHelper.Context, AuditHelper.Context);
                logger.Info("ReinsurerContract  Added");
                if (result.ReinsurerContractInsertion)
                {
                    return "OK";
                }
                else
                {
                    return "Add ReinsurerContract  failed!";
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return "Add ReinsurerContract  failed!";
            }

        }

        [HttpPost]
        public object GetReinsurerContrctById(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            IReinsurerContractManagementService ReinsurerContractManagementService = ServiceFactory.GetReinsurerContractManagementService();

            ReinsurerContractResponseDto ReinsurerContract = ReinsurerContractManagementService.GetReinsurerContractById(Guid.Parse(data["Id"].ToString()),
                SecurityHelper.Context,
                AuditHelper.Context);
            return ReinsurerContract;
        }

        [HttpPost]
        public object GetAllReinsurerContracts()
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            IReinsurerContractManagementService ReinsurerContractManagementService = ServiceFactory.GetReinsurerContractManagementService();
            ReinsurerContractsResponseDto ReinsurerContractData = ReinsurerContractManagementService.GetReinsurerContracts(
            SecurityHelper.Context,
            AuditHelper.Context);
            return ReinsurerContractData.ReinsurerContracts.ToArray();
        }

        [HttpPost]
        public object GetAllReinsurerContractsByInsurerId(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            IReinsurerContractManagementService ReinsurerContractManagementService = ServiceFactory.GetReinsurerContractManagementService();
            ReinsurerContractsResponseDto ReinsurerContractData = ReinsurerContractManagementService.GetReinsurerContracts(
            SecurityHelper.Context,
            AuditHelper.Context);
            return ReinsurerContractData.ReinsurerContracts.FindAll(r => r.InsurerId == Guid.Parse(data["Id"].ToString()) && r.IsActive==true).ToArray();
        }

        [HttpPost]
        public object GetAllReinsurerContractsByReinsurerId(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            IReinsurerContractManagementService ReinsurerContractManagementService = ServiceFactory.GetReinsurerContractManagementService();
            ReinsurerContractsResponseDto ReinsurerContractData = ReinsurerContractManagementService.GetReinsurerContracts(
            SecurityHelper.Context,
            AuditHelper.Context);
            return ReinsurerContractData.ReinsurerContracts.FindAll(m => m.ReinsurerId == Guid.Parse(data["Id"].ToString())).ToArray();
        }
        #endregion

        #region Consortium
        [HttpPost]
        public string AddorUpdateReinsurerConsortiums(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                ReinsurerConsortiumData ReinsurerConsortiums = data.ToObject<ReinsurerConsortiumData>();
                IReinsurerManagementService ReinsurerConsortiumManagementService = ServiceFactory.GetReinsurerManagementService();
                foreach (var item in ReinsurerConsortiums.Consortiums)
                {
                    if (item.Id2.ToString() == "10000000-0000-0000-0000-000000000001")
                    {
                        ReinsurerConsortiumRequestDto resultA = ReinsurerConsortiumManagementService.AddReinsurerConsortium(item, SecurityHelper.Context, AuditHelper.Context);
                    }
                    else
                    {
                        ReinsurerConsortiumRequestDto resultU = ReinsurerConsortiumManagementService.UpdateReinsurerConsortium(item, SecurityHelper.Context, AuditHelper.Context);
                    }
                    logger.Info("Reinsurer Consortium  Added");
                }
                //if (result.ReinsurerConsortiumInsertion)
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
            //    return "Add Reinsurer Consortium  failed!";
            //}

        }


        [HttpPost]
        public string AddorUpdateReinsurerConsortiumsNew(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                ReinsurerConsortiumData ReinsurerConsortiums = data.ToObject<ReinsurerConsortiumData>();
                IReinsurerManagementService ReinsurerConsortiumManagementService = ServiceFactory.GetReinsurerManagementService();
                ReinsurerConsortiumManagementService.AddorUpdateReinsurerConsortiums(ReinsurerConsortiums.Consortiums, SecurityHelper.Context, AuditHelper.Context);
                logger.Info("Reinsurer Consortium  Added");
                //foreach (var item in ReinsurerConsortiums.Consortiums)
                //{
                //    if (item.Id2.ToString() == "10000000-0000-0000-0000-000000000001")
                //    {
                //        ReinsurerConsortiumRequestDto resultA = ReinsurerConsortiumManagementService.AddReinsurerConsortium(item, SecurityHelper.Context, AuditHelper.Context);
                //    }
                //    else
                //    {
                //        ReinsurerConsortiumRequestDto resultU = ReinsurerConsortiumManagementService.UpdateReinsurerConsortium(item, SecurityHelper.Context, AuditHelper.Context);
                //    }
                //    logger.Info("Reinsurer Consortium  Added");
                //}
                //if (result.ReinsurerConsortiumInsertion)
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
            //    return "Add Reinsurer Consortium  failed!";
            //}

        }

        //[HttpPost]
        //public string UpdateReinsurerConsortiums(JObject data)
        //{
        //    ILog logger = LogManager.GetLogger(typeof(ApiController));
        //    logger.Debug("Add Reinsurer Consortium  method!");

        //    List<ReinsurerConsortiumRequestDto> ReinsurerConsortium = data.ToObject<List<ReinsurerConsortiumRequestDto>>();
        //    IReinsurerManagementService ReinsurerConsortiumManagementService = ServiceFactory.GetReinsurerManagementService();
        //    List<ReinsurerConsortiumRequestDto> result = ReinsurerConsortiumManagementService.UpdateReinsurerConsortium(ReinsurerConsortium, SecurityHelper.Context, AuditHelper.Context);
        //    logger.Info("Reinsurer Consortium  Added");
        //    if (result.ReinsurerConsortiumInsertion)
        //    {
        //        return "OK";
        //    }
        //    else
        //    {
        //        return "Add Reinsurer Consortium  failed!";
        //    }
        //}

        [HttpPost]
        public object GetReinsurerConsortiumById(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            IReinsurerManagementService ReinsurerConsortiumManagementService = ServiceFactory.GetReinsurerManagementService();

            ReinsurerConsortiumResponseDto ReinsurerConsortium = ReinsurerConsortiumManagementService.GetReinsurerConsortiumById(Guid.Parse(data["Id"].ToString()),
                SecurityHelper.Context,
                AuditHelper.Context);
            return ReinsurerConsortium;
        }

        [HttpPost]
        public object GetAllReinsurerConsortiums()
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            IReinsurerManagementService ReinsurerConsortiumManagementService = ServiceFactory.GetReinsurerManagementService();

            ReinsurerConsortiumsResponseDto ReinsurerConsortiumData = ReinsurerConsortiumManagementService.GetReinsurerConsortiums(
            SecurityHelper.Context,
            AuditHelper.Context);
            return ReinsurerConsortiumData.ReinsurerConsortiums.ToArray();
        }

        [HttpPost]
        public object GetReinsurerConsortiumsByReinsurerId(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            IReinsurerManagementService ReinsurerConsortiumManagementService = ServiceFactory.GetReinsurerManagementService();

            ReinsurerConsortiumsResponseDto ReinsurerConsortiumData = ReinsurerConsortiumManagementService.GetReinsurerConsortiums(
            SecurityHelper.Context,
            AuditHelper.Context);
            return ReinsurerConsortiumData.ReinsurerConsortiums.FindAll(c => c.ParentReinsurerId == Guid.Parse(data["Id"].ToString())).ToArray();
        }
        #endregion

        #region Other
        [HttpPost]
        public object GetAllCommodities()
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            ICommodityManagementService CommodityManagementService = ServiceFactory.GetCommodityManagementService();

            CommoditiesResponseDto CommoditiesData = CommodityManagementService.GetAllCommodities(
            SecurityHelper.Context,
            AuditHelper.Context);
            return CommoditiesData.Commmodities.ToArray();
        }

        #endregion

        #region ReinsurerUser
        [HttpPost]
        public object GetUsersById(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            IReinsurerManagementService ReinsurerManagementService = ServiceFactory.GetReinsurerManagementService();

            UserResponseDto Users = ReinsurerManagementService.GetUserById(Guid.Parse(data["Id"].ToString()),
                SecurityHelper.Context,
                AuditHelper.Context);
            return Users;
        }
        [HttpPost]
        public object GetAllStaffByReinsurerId(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            IReinsurerManagementService ReinsurerManagementService = ServiceFactory.GetReinsurerManagementService();

            object staff = ReinsurerManagementService.GetAllStaffByReinsurerId(Guid.Parse(data["Id"].ToString()),
                SecurityHelper.Context,
                AuditHelper.Context);
            return staff;
        }
        #endregion

        #region ReinsurerBordx

        [HttpPost]
        public object GetBordxReportColumnsByReinsureId(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            IReinsurerManagementService ReinsurerManagementService = ServiceFactory.GetReinsurerManagementService();

            var ReinsureId = Guid.Parse(data["ReinsureId"].ToString());

            var BordxReportColumnsData = ReinsurerManagementService.GetBordxReportColumnsByReinsureId(ReinsureId,
            SecurityHelper.Context,
            AuditHelper.Context);


            return BordxReportColumnsData;
        }

        [HttpPost]
        public string AddOrUpdateReinsureBordxReportColumns(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                var RMMResponseDto = data["JObject"].ToObject<List<ReinsureBordxReportColumnsMappingResponseDto>>();
                IReinsurerManagementService ReinsurerManagementService = ServiceFactory.GetReinsurerManagementService();

                var saved = ReinsurerManagementService.AddOrUpdateReinsureBordxReportColumns(RMMResponseDto,
                    SecurityHelper.Context,
                    AuditHelper.Context);

                if (saved)
                {
                    return "OK";
                }
                return "Error";
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return "Error";
            }


        }

        [HttpPost]
        public object GetAllReinsurerBordxByYearandReinsurerIdForGrid(JObject data)
        {

            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            IReinsurerManagementService ReinsurerManagementService = ServiceFactory.GetReinsurerManagementService();

            var ReinsureId = Guid.Parse(data["ReinsureId"].ToString());
            var BordxYear = int.Parse(data["Year"].ToString());

            var BordxReportColumnsData = ReinsurerManagementService.GetAllReinsurerBordxByYearandReinsurerIdForGrid(ReinsureId,BordxYear,
            SecurityHelper.Context,
            AuditHelper.Context);


            return BordxReportColumnsData;
        }

        [HttpPost]
        public object UserValidationReinsureBordxSubmission(JObject data)
        {
            object Response = null;
            try
            {
                var loggedInUserId = Guid.Parse(data["loggedInUserId"].ToString());
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                IReinsurerManagementService ReinsurerManagementService = ServiceFactory.GetReinsurerManagementService();
                Response = ReinsurerManagementService.UserValidationReinsureBordxSubmission(loggedInUserId,
                    SecurityHelper.Context, AuditHelper.Context);
                return Response;
            }
            catch (Exception ex)
            {
                Response = "Error occured";
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                             ex.InnerException);
            }
            return Response;
        }

        #endregion

    }
    public class ReinsurerConsortiumData
    {
        public List<ReinsurerConsortiumRequestDto> Consortiums { get; set; }
    }
}
