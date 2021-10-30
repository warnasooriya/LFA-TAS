
using Newtonsoft.Json.Linq;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using TAS.DataTransfer.Requests;
using TAS.DataTransfer.Responses;
using TAS.Services;
using TAS.Web.Common;

namespace TAS.Web.Controllers
{
    public class MakeAndModelManagementController : ApiController
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

       // private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Make
        [HttpPost]
        public object AddMake(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                MakeRequestDto Make = data.ToObject<MakeRequestDto>();
                IMakeManagementService MakeManagementService = ServiceFactory.GetMakeManagementService();
                MakeRequestDto result = MakeManagementService.AddMake(Make, SecurityHelper.Context, AuditHelper.Context);
                return result;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return "Add Make failed!";
            }

        }

        [HttpPost]
        public string UpdateMake(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                MakeRequestDto Make = data.ToObject<MakeRequestDto>();
                IMakeManagementService MakeManagementService = ServiceFactory.GetMakeManagementService();
                MakeRequestDto result = MakeManagementService.UpdateMake(Make, SecurityHelper.Context, AuditHelper.Context);
                logger.Info("Make Added");
                if (result.MakeInsertion)
                {
                    return "OK";
                }
                else
                {
                    return "Add Make failed!";
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return "Add Make failed!";
            }

        }

        [HttpPost]
        public object GetMakeById(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            IMakeManagementService MakeManagementService = ServiceFactory.GetMakeManagementService();

            MakeResponseDto Make = MakeManagementService.GetMakeById(Guid.Parse(data["Id"].ToString()),
                SecurityHelper.Context,
                AuditHelper.Context);
            return Make;
        }

        [HttpPost]
        public object GetMakesByIdList(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            MakeList MakeList = data.ToObject<MakeList>();
            var Makes = MakeList.Makes.Distinct();
            if (Makes != null && Makes.Count() > 0)
            {
                IMakeManagementService MakeManagementService = ServiceFactory.GetMakeManagementService();
                MakesResponseDto result = new MakesResponseDto();
                result.Makes = new List<MakeResponseDto>();
                foreach (var item in Makes)
                {
                    MakeResponseDto Make = MakeManagementService.GetMakeById(item,
                        SecurityHelper.Context,
                        AuditHelper.Context);
                    result.Makes.Add(Make);
                }

                return result.Makes.FindAll(m => m.CommodityTypeId == Guid.Parse(data["CommodityTypeId"].ToString())).ToArray();
            }
            else
            {
                return new MakesResponseDto();
            }
        }

        [HttpPost]
        public object GetAllMakes()
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            IMakeManagementService MakeManagementService = ServiceFactory.GetMakeManagementService();

            MakesResponseDto MakeData = MakeManagementService.GetAllMakes(
            SecurityHelper.Context,
            AuditHelper.Context);
            return MakeData.Makes.ToArray();
        }

        [HttpPost]
        public object GetAllMakesByComodityTypeId(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            IMakeManagementService MakeManagementService = ServiceFactory.GetMakeManagementService();

            MakesResponseDto MakeData = MakeManagementService.GetAllMakes(
            SecurityHelper.Context,
            AuditHelper.Context);
            return MakeData.Makes.FindAll(m => m.CommodityTypeId == Guid.Parse(data["Id"].ToString())).ToArray();
        }

        [HttpPost]
        public object GetAllMakesByManufacturerId(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            IMakeManagementService MakeManagementService = ServiceFactory.GetMakeManagementService();

            MakesResponseDto MakeData = MakeManagementService.GetAllMakes(
            SecurityHelper.Context,
            AuditHelper.Context);
            return MakeData.Makes.FindAll(m => m.ManufacturerId == Guid.Parse(data["Id"].ToString())).ToArray();
        }

        #endregion

        #region Model
        [HttpPost]
        public object GetModelesByMakeIdsAndCommodityCategoryId(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            List<Guid> MakeIdList = data["data"].ToObject<List<Guid>>();
            Guid CommodityCategoryId = Guid.Parse(data["commodityCategoryId"].ToString());
            IModelManagementService ModelManagementService = ServiceFactory.GetModelManagementService();

            ModelesResponseDto ModelData = ModelManagementService.GetModelesByMakeIdsAndCommodityCategoryId(
                MakeIdList, CommodityCategoryId,
            SecurityHelper.Context,
            AuditHelper.Context);

            return ModelData.Modeles.ToArray();
        }

        [HttpPost]
        public object GetModelByMakeIdAndCatogaryId(JObject data)
        {
            object Response = null;
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                Guid MakeId = Guid.Parse(data["MakeId"].ToString());
                Guid CommodityCategoryId = Guid.Parse(data["CommodityCategoryId"].ToString());

                IModelManagementService ModelManagementService = ServiceFactory.GetModelManagementService();
                Response = ModelManagementService.GetModelByMakeIdAndCatogaryId(MakeId, CommodityCategoryId,
                    SecurityHelper.Context, AuditHelper.Context);
                return Response;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }


        [HttpPost]
        public string AddModel(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                ModelRequestDto Model = data.ToObject<ModelRequestDto>();
                IModelManagementService ModelManagementService = ServiceFactory.GetModelManagementService();
                ModelRequestDto result = ModelManagementService.AddModel(Model, SecurityHelper.Context, AuditHelper.Context);
                logger.Info("Model Added");
                if (result.ModelInsertion)
                {
                    return "OK";
                }
                else
                {
                    return "Add Model failed!";
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return "Add Model failed!";
            }

        }

        [HttpPost]
        public string UpdateModel(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                ModelRequestDto Model = data.ToObject<ModelRequestDto>();
                IModelManagementService ModelManagementService = ServiceFactory.GetModelManagementService();
                ModelRequestDto result = ModelManagementService.UpdateModel(Model, SecurityHelper.Context, AuditHelper.Context);
                logger.Info("Model Added");
                if (result.ModelInsertion)
                {
                    return "OK";
                }
                else
                {
                    return "Add Model failed!";
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return "Add Model failed!";
            }

        }

        [HttpPost]
        public object GetModelById(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            IModelManagementService ModelManagementService = ServiceFactory.GetModelManagementService();

            ModelResponseDto Model = ModelManagementService.GetModelById(Guid.Parse(data["Id"].ToString()),
                SecurityHelper.Context,
                AuditHelper.Context);
            return Model;
        }

        [HttpPost]
        public object GetModelesByMakeId(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            IModelManagementService ModelManagementService = ServiceFactory.GetModelManagementService();

            ModelesResponseDto ModelData = ModelManagementService.GetAllModeles(
            SecurityHelper.Context,
            AuditHelper.Context);
            return ModelData.Modeles.FindAll(m => m.MakeId == Guid.Parse(data["Id"].ToString())).ToArray();
        }

        [HttpPost]
        public object GetModelesByMakeIds(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            MakeModelIdsRequestDto makeIds = data.ToObject<MakeModelIdsRequestDto>();
            IModelManagementService ModelManagementService = ServiceFactory.GetModelManagementService();

            object ModelData = ModelManagementService.GetModelesByMakeIds(
            SecurityHelper.Context,
            AuditHelper.Context, makeIds.makeIdList);
            return ModelData;
        }

        [HttpPost]
        public object GetCylinderCountEnginCapacityByVariantId(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            IVariantManagementService VariantManagementService = ServiceFactory.GetVariantManagementService();

            VariantResponseDto Variant = VariantManagementService.GetVariantById(Guid.Parse(data["Id"].ToString()),
              SecurityHelper.Context,
              AuditHelper.Context);
            return Variant;
        }

        [HttpPost]
        public object GetAllModels()
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            IModelManagementService ModelManagementService = ServiceFactory.GetModelManagementService();

            ModelesResponseDto ModelData = ModelManagementService.GetAllModeles(
            SecurityHelper.Context,
            AuditHelper.Context);
            return ModelData.Modeles.ToArray();
        }

        [HttpPost]
        public object GetMakeIdByModelId(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            try
            {
                Guid id = Guid.Parse(data["Id"].ToString());

                IModelManagementService ModelManagementService = ServiceFactory.GetModelManagementService();

                ModelesResponseDto ModelData = ModelManagementService.GetAllModeles(
                SecurityHelper.Context,
                AuditHelper.Context);
                return ModelData.Modeles.Find(m => m.Id == id).MakeId;
            }
            catch(Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }
        }
        [HttpPost]
        public object GetModelesByCategoryAndMakeId(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            IModelManagementService ModelManagementService = ServiceFactory.GetModelManagementService();

            ModelesResponseDto ModelData = ModelManagementService.GetAllModeles(
            SecurityHelper.Context,
            AuditHelper.Context);
            return ModelData.Modeles.FindAll(m => m.MakeId == Guid.Parse(data["makeId"].ToString()) && m.CategoryId == Guid.Parse(data["categoryId"].ToString())).ToArray();
        }

        [HttpPost]
        public object GetMakesByCommodityCategoryId(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            try
            {
                Guid id = Guid.Parse(data["Id"].ToString());

                IMakeManagementService IMakeManagementService = ServiceFactory.GetMakeManagementService();

                MakesResponseDto MakeData = IMakeManagementService.GetMakesByCommodityCategoryId(
                id,
                SecurityHelper.Context,
                AuditHelper.Context);
                return MakeData.Makes.ToArray();
            }
            catch(Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }
        }

        #endregion

        #region Other
        [HttpPost]
        public object GetAllCommodities()
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                ICommodityManagementService CommodityManagementService = ServiceFactory.GetCommodityManagementService();

                CommoditiesResponseDto CommoditiesData = CommodityManagementService.GetAllCommodities(
                SecurityHelper.Context,
                AuditHelper.Context);
                return CommoditiesData.Commmodities.ToList();

            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                // System.IO.File.WriteAllText(@"C:\LFAErrors.txt", e.Message);
                return ex.Message;
            }

        }

        [HttpPost]
        public object GetAllManufacturers()
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            IManufacturerManagementService ManufacturerManagementService = ServiceFactory.GetManufacturerManagementService();

            ManufacturesResponseDto ManufacturerData = ManufacturerManagementService.GetAllManufatures(
            SecurityHelper.Context,
            AuditHelper.Context);
            return ManufacturerData.Manufactures.ToArray();
        }

        [HttpPost]
        public object GetAllCategories(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            ICommodityManagementService CommodityCategoryManagementService = ServiceFactory.GetCommodityManagementService();

            CommodityCategoriesRespondDto CommoditiesData = CommodityCategoryManagementService.GetCommodityCategoriesByCommodityTypeId(
            SecurityHelper.Context,
            AuditHelper.Context, Guid.Parse(data["Id"].ToString()));
            return CommoditiesData.CommodityCategories.ToArray();

        }

        //[HttpPost]
        //public object GetCategoryById(JObject data)
        //{
        //    ICommodityManagementService CommodityCategoryManagementService = ServiceFactory.GetCommodityManagementService();

        //    CommodityCategoriesRespondDto CommoditiesData = CommodityCategoryManagementService.get(
        //    SecurityHelper.Context,
        //    AuditHelper.Context, Guid.Parse(data["Id"].ToString()));
        //    return CommoditiesData.CommodityCategories.ToArray();

        //}


        [HttpPost]
        public object GetAllVariantsByModelIds(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            List<Guid> VariantIdList = data["data"].ToObject<List<Guid>>();
            IVariantManagementService VariantManagementService = ServiceFactory.GetVariantManagementService();

            VariantsResponseDto VariantData = VariantManagementService.GetAllVariantsByModelIds(
            VariantIdList,
            SecurityHelper.Context,
            AuditHelper.Context);

            return VariantData.Variants.ToArray();
        }


        [HttpPost]
        public object GetAllPremiumAddonType()
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                IPremiumAddonTypeManagementService premiumAddonTypeManagementService = ServiceFactory.GetPremiumAddonTypeManagementService();
                PremiumAddonTypesResponseDto premiumAddonTypeData = premiumAddonTypeManagementService.GetAllPremiumAddonType(SecurityHelper.Context,
                AuditHelper.Context);

                return premiumAddonTypeData.PremiumAddonTypes.ToArray();
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                // System.IO.File.WriteAllText(@"C:\LFAErrors.txt", e.Message);
                return ex.Message;
            }

        }

        #endregion

        public class MakeList
        {
            public List<Guid> Makes { get; set; }
        }
    }
}
