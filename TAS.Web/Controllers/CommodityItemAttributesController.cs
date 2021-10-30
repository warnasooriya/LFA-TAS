using Newtonsoft.Json.Linq;
using NLog;
using System;
using System.Reflection;
using System.Web.Http;
using TAS.DataTransfer.Requests;
using TAS.DataTransfer.Responses;
using TAS.Services;
using TAS.Web.Common;

namespace TAS.Web.Controllers
{
    public class CommodityItemAttributesController : ApiController
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        // private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Item Status
        [HttpPost]
        public string AddItemStatus(JObject data)
        {
            try
            {

                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                ItemStatusRequestDto ItemStatus = data.ToObject<ItemStatusRequestDto>();
                IItemStatusManagementService ItemStatusManagementService = ServiceFactory.GetItemStatusManagementService();
                ItemStatusRequestDto result = ItemStatusManagementService.AddItemStatus(ItemStatus, SecurityHelper.Context, AuditHelper.Context);
                logger.Info("ItemStatus Added");
                if (result.ItemStatusInsertion)
                {
                    return "OK";
                }
                else
                {
                    return "Add ItemStatus failed!";
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return "Add ItemStatus failed!";
            }

        }

        [HttpPost]
        public string UpdateItemStatus(JObject data)
        {
            try
            {

                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                ItemStatusRequestDto ItemStatus = data.ToObject<ItemStatusRequestDto>();
                IItemStatusManagementService ItemStatusManagementService = ServiceFactory.GetItemStatusManagementService();
                ItemStatusRequestDto result = ItemStatusManagementService.UpdateItemStatus(ItemStatus, SecurityHelper.Context, AuditHelper.Context);
                logger.Info("ItemStatus Added");
                if (result.ItemStatusInsertion)
                {
                    return "OK";
                }
                else
                {
                    return "Add ItemStatus failed!";
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return "Add ItemStatus failed!";
            }

        }

        [HttpPost]
        public object GetItemStatusById(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            IItemStatusManagementService ItemStatusManagementService = ServiceFactory.GetItemStatusManagementService();

            ItemStatusResponseDto ItemStatus = ItemStatusManagementService.GetItemStatusById(Guid.Parse(data["Id"].ToString()),
                SecurityHelper.Context,
                AuditHelper.Context);
            return ItemStatus;
        }

        [HttpPost]
        public object GetAllItemStatuss()
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            IItemStatusManagementService ItemStatusManagementService = ServiceFactory.GetItemStatusManagementService();

            ItemStatusesResponseDto ItemStatusData = ItemStatusManagementService.GetItemStatuss(
            SecurityHelper.Context,
            AuditHelper.Context);
            return ItemStatusData.ItemStatuss.ToArray();
        }

        [HttpPost]
        public bool IsExsistingItemStatusByStatus(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            IItemStatusManagementService ItemStatusManagementService = ServiceFactory.GetItemStatusManagementService();
            bool isExsists = ItemStatusManagementService.IsExsistingItemStatusByStatus(Guid.Parse(data["Id"].ToString()), data["Status"].ToString(),
                SecurityHelper.Context,
                AuditHelper.Context);
            return isExsists;
        }
        #endregion

        #region Category
        [HttpPost]
        public string AddCommodityCategory(JObject data)
        {
            try
            {


                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                CommodityCategoryRequestDto CommodityCategory = data.ToObject<CommodityCategoryRequestDto>();
                CommodityCategory.CommodityTypeId = Guid.Parse(data["CommodityTypeId"].ToString());
                ICommodityCategoryManagementService CommodityCategoryManagementService = ServiceFactory.GetCommodityCategoryManagementService();
                CommodityCategoryRequestDto result = CommodityCategoryManagementService.AddCommodityCategory(CommodityCategory, SecurityHelper.Context, AuditHelper.Context);
                logger.Info("CommodityCategory Added");
                if (result.CommodityCategoryInsertion)
                {
                    return "OK";
                }
                else
                {
                    return "Add CommodityCategory failed!";
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return "Add CommodityCategory failed!";
            }

        }

        [HttpPost]
        public string UpdateCommodityCategory(JObject data)
        {
            try
            {

                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                CommodityCategoryRequestDto CommodityCategory = data.ToObject<CommodityCategoryRequestDto>();
                ICommodityCategoryManagementService CommodityCategoryManagementService = ServiceFactory.GetCommodityCategoryManagementService();
                CommodityCategoryRequestDto result = CommodityCategoryManagementService.UpdateCommodityCategory(CommodityCategory, SecurityHelper.Context, AuditHelper.Context);
                logger.Info("CommodityCategory Added");
                if (result.CommodityCategoryInsertion)
                {
                    return "OK";
                }
                else
                {
                    return "Add CommodityCategory failed!";
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return "Add CommodityCategory failed!";
            }

        }

        [HttpPost]
        public object GetCommodityCategoryById(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            ICommodityCategoryManagementService CommodityCategoryManagementService = ServiceFactory.GetCommodityCategoryManagementService();

            CommodityCategoryResponseDto CommodityCategory = CommodityCategoryManagementService.GetCommodityCategoryById(
                Guid.Parse(data["CommodityCategoryId"].ToString()), Guid.Parse(data["CommodityTypeId"].ToString()),
                SecurityHelper.Context,
                AuditHelper.Context);
            return CommodityCategory;
        }



        [HttpPost]
        public object GetAllCommodityCategories(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            ICommodityCategoryManagementService CommodityCategoryManagementService = ServiceFactory.GetCommodityCategoryManagementService();

            CommodityCategoriesRespondDto CommodityCategoryData = CommodityCategoryManagementService.GetCommodityCategories(Guid.Parse(data["CommodityTypeId"].ToString()),
            SecurityHelper.Context,
            AuditHelper.Context);
            return CommodityCategoryData.CommodityCategories.ToArray();
        }

        [HttpPost]
        public bool IsExsistingCommodityCategoryByDescription(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            ICommodityCategoryManagementService CommodityCategoryManagementService = ServiceFactory.GetCommodityCategoryManagementService();

            bool isExsists = CommodityCategoryManagementService.IsExsistingCommodityCategoryByDescription(Guid.Parse(data["CommodityCategoryId"].ToString()), data["CommodityCategoryDescription"].ToString(),
                SecurityHelper.Context,
                AuditHelper.Context);
            return isExsists;
        }

        #endregion

        #region Commodity Usage Type
        [HttpPost]
        public string AddCommodityUsageType(JObject data)
        {
            try
            {

                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                CommodityUsageTypeRequestDto CommodityUsageType = data.ToObject<CommodityUsageTypeRequestDto>();
                ICommodityUsageTypeManagementService CommodityUsageTypeManagementService = ServiceFactory.GetCommodityUsageTypeManagementService();
                CommodityUsageTypeRequestDto result = CommodityUsageTypeManagementService.AddCommodityUsageType(CommodityUsageType, SecurityHelper.Context, AuditHelper.Context);
                logger.Info("CommodityUsageType Added");
                if (result.CommodityUsageTypeInsertion)
                {
                    return "OK";
                }
                else
                {
                    return "Add CommodityUsageType failed!";
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return "Add CommodityUsageType failed!";
            }

        }

        [HttpPost]
        public string UpdateCommodityUsageType(JObject data)
        {
            try
            {

                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                CommodityUsageTypeRequestDto CommodityUsageType = data.ToObject<CommodityUsageTypeRequestDto>();
                ICommodityUsageTypeManagementService CommodityUsageTypeManagementService = ServiceFactory.GetCommodityUsageTypeManagementService();
                CommodityUsageTypeRequestDto result = CommodityUsageTypeManagementService.UpdateCommodityUsageType(CommodityUsageType, SecurityHelper.Context, AuditHelper.Context);
                logger.Info("CommodityUsageType Added");
                if (result.CommodityUsageTypeInsertion)
                {
                    return "OK";
                }
                else
                {
                    return "Add CommodityUsageType failed!";
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return "Add CommodityUsageType failed!";
            }

        }

        [HttpPost]
        public object GetCommodityUsageTypeById(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            ICommodityUsageTypeManagementService CommodityUsageTypeManagementService = ServiceFactory.GetCommodityUsageTypeManagementService();

            CommodityUsageTypeResponseDto CommodityUsageType = CommodityUsageTypeManagementService.GetCommodityUsageTypeById(Guid.Parse(data["Id"].ToString()),
                SecurityHelper.Context,
                AuditHelper.Context);
            return CommodityUsageType;
        }

        [HttpPost]
        public object GetAllCommodityUsageTypes()
        {
            try
            {

                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                ICommodityUsageTypeManagementService CommodityUsageTypeManagementService = ServiceFactory.GetCommodityUsageTypeManagementService();
                CommodityUsageTypesResponseDto CommodityUsageTypeData = CommodityUsageTypeManagementService.GetCommodityUsageTypes(
                SecurityHelper.Context,
                AuditHelper.Context);
                return CommodityUsageTypeData.CommodityUsageTypes.ToArray();
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
              //  System.IO.File.WriteAllText(@"C:\LFAErrors.txt", ex.Message);
                return ex.Message;
            }

        }

        [HttpPost]
        public bool IsExsistingCommodityUsageTypeByName(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            ICommodityUsageTypeManagementService CommodityUsageTypeManagementService = ServiceFactory.GetCommodityUsageTypeManagementService();

            bool isExsists = CommodityUsageTypeManagementService.IsExsistingCommodityUsageTypeByName(Guid.Parse(data["Id"].ToString()), data["Name"].ToString(),
                SecurityHelper.Context,
                AuditHelper.Context);
            return isExsists;
        }

        #endregion

        [HttpPost]
        public object GetCommodityTypeIdbyCommodityCategoryId(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            ICommodityManagementService commodityManagementService = ServiceFactory.GetCommodityManagementService();
            String result = commodityManagementService.GetCommodityTypeIdbyCommodityCategoryId(Guid.Parse(data["Id"].ToString()), SecurityHelper.Context, AuditHelper.Context);
            return result;
        }

        [HttpPost]
        public object GetCommodityTypeByCommodityCategoryId(JObject data)
        {
            object response = null;
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                ICommodityManagementService commodityManagementService = ServiceFactory.GetCommodityManagementService();
                response = commodityManagementService.GetCommodityTypeByCommodityCategoryId(Guid.Parse(data["Id"].ToString()), SecurityHelper.Context, AuditHelper.Context);

            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return response;
        }
    }
}
