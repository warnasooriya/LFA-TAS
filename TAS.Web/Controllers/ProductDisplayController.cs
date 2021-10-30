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
    public class ProductDisplayController : ApiController
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        //private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [HttpPost]
        public object GetProductsByTPA(JObject data)
        {
            IProductManagementService productManagementService = ServiceFactory.GetProductManagementService();

            Guid TPAId = new Guid();   // Temp new Guid. should assign the tpa id coming from jObject data
            TPAId = Guid.Parse(data["tpaId"].ToString());

            TPAProductsResponseDto productData = productManagementService.GetProductsByTPA(TPAId,
            SecurityHelper.Context,
            AuditHelper.Context);
            return productData.TPAProducts.ToArray();
        }

        [HttpPost]
        public object GetTPADetailsById(JObject data)
        {
            ITASTPAManagementService TPAManagementService = ServiceFactory.GetTASTPAManagementService();
            TPAsResponseDto TPAData = TPAManagementService.GetProductDisplayTPADetailsByTPAId(
            SecurityHelper.Context,
            AuditHelper.Context,
            Guid.Parse(data["tpaId"].ToString()));
            return TPAData.TPAs.ToArray();
        }

        [HttpPost]
        public object GetTPAIdByName(JObject data)
        {
            ITASTPAManagementService TPAManagementService = ServiceFactory.GetTASTPAManagementService();
            TPAsResponseDto TPAData = TPAManagementService.GetProductDisplayTPADetailsByTPAName(
            SecurityHelper.Context,
            AuditHelper.Context,
            data["tpaName"].ToString());
            return TPAData.TPAs.ToArray();
        }

        [HttpPost]
        public string GetTPANameById(JObject data)
        {
            ITASTPAManagementService TPAManagementService = ServiceFactory.GetTASTPAManagementService();
            string tpaName = TPAManagementService.GetTPANameById(
            SecurityHelper.Context,
            AuditHelper.Context,
            Guid.Parse(data["tpaId"].ToString()));
            return tpaName;
        }

        [HttpPost]
        public object GetDocumentTypesByPageName(JObject data)
        {
            String PageName = data["PageName"].ToString();
            Guid TpaId = Guid.Parse(data["tpaId"].ToString());

            IProductManagementService productManagementService = ServiceFactory.GetProductManagementService();
            return productManagementService.GetDocumentTypesByPageName(PageName, TpaId);

        }

        [HttpPost]
        public string GetTPAImageById(JObject data)
        {
            //IImageManagementService ImageManagementService = ServiceFactory.GetImageManagementService();
            ITASTPAManagementService TPAManagementService = ServiceFactory.GetTASTPAManagementService();
            //Guid TPAId = new Guid();   // Temp new Guid. should assign the tpa id coming from jObject data
            Guid TPAId = new Guid();   // Temp new Guid. should assign the tpa id coming from jObject data
            TPAId = Guid.Parse(data["tpaId"].ToString());

            ImageResponseDto ImageData = TPAManagementService.GetTPAImageById(TPAId, Guid.Parse(data["ImageId"].ToString()),
            SecurityHelper.Context,
            AuditHelper.Context);
            return ImageData.DisplayImageSrc;
        }

        [HttpPost]
        public object GetAllCategories(JObject data)
        {
            ITASTPAManagementService TPAManagementService = ServiceFactory.GetTASTPAManagementService();

            CommodityCategoriesRespondDto CommoditiesData = TPAManagementService.GetCommodityCategoriesByCommodityTypeId(
            SecurityHelper.Context,
            AuditHelper.Context, Guid.Parse(data["Id"].ToString()), Guid.Parse(data["tpaId"].ToString()));
            return CommoditiesData.CommodityCategories.ToArray();

        }

        [HttpPost]
        public object GetProductByProdId(JObject data)
        {

            ITASTPAManagementService TPAManagementService = ServiceFactory.GetTASTPAManagementService();

            ProductResponseDto ProductData = TPAManagementService.GetProductByProdId(
            SecurityHelper.Context,
            AuditHelper.Context, Guid.Parse(data["Id"].ToString()), Guid.Parse(data["tpaId"].ToString()));
            return ProductData;

        }

        [HttpPost]
        public object GetAllMakesByComodityTypeId(JObject data)
        {
            ITASTPAManagementService TPAManagementService = ServiceFactory.GetTASTPAManagementService();


            MakesResponseDto MakeData = TPAManagementService.GetAllMakes(
            SecurityHelper.Context,
            AuditHelper.Context, Guid.Parse(data["Id"].ToString()), Guid.Parse(data["tpaId"].ToString()));
            return MakeData;
        }

        [HttpPost]
        public object GetAllItemStatuss(JObject data)
        {
            ITASTPAManagementService TPAManagementService = ServiceFactory.GetTASTPAManagementService();


            ItemStatusesResponseDto ItemStatusData = TPAManagementService.GetItemStatuss(
            SecurityHelper.Context,
            AuditHelper.Context, Guid.Parse(data["tpaId"].ToString()));
            return ItemStatusData.ItemStatuss.ToArray();
        }


        [HttpPost]
        public object GetModelesByMakeId(JObject data)
        {
            ITASTPAManagementService TPAManagementService = ServiceFactory.GetTASTPAManagementService();

            ModelesResponseDto ModelData = TPAManagementService.GetModelesByMakeId(
            SecurityHelper.Context,
            AuditHelper.Context, Guid.Parse(data["Id"].ToString()), Guid.Parse(data["tpaId"].ToString()));
            return ModelData;
        }

        [HttpPost]
        public object GetVariantsByModelId(JObject data)
        {
            ITASTPAManagementService TPAManagementService = ServiceFactory.GetTASTPAManagementService();

            VariantsResponseDto VariantData = TPAManagementService.GetVariantsByModelId(
            SecurityHelper.Context,
            AuditHelper.Context, Guid.Parse(data["Id"].ToString()), Guid.Parse(data["tpaId"].ToString()));
            return VariantData.Variants.ToArray();
        }

        [HttpPost]
        public object GetAllCylinderCounts(JObject data)
        {
            ITASTPAManagementService TPAManagementService = ServiceFactory.GetTASTPAManagementService();

            CylinderCountsResponseDto cylinderCountData = TPAManagementService.GetCylinderCounts(
            SecurityHelper.Context,
            AuditHelper.Context, Guid.Parse(data["tpaId"].ToString()));
            return cylinderCountData.CylinderCounts.ToArray();
        }


        [HttpPost]
        public object GetAllVehicleBodyTypes(JObject data)
        {
            ITASTPAManagementService TPAManagementService = ServiceFactory.GetTASTPAManagementService();

            VehicleBodyTypesResponseDto vehicleBodyTypeData = TPAManagementService.GetVehicleBodyTypes(
            SecurityHelper.Context,
            AuditHelper.Context, Guid.Parse(data["tpaId"].ToString()));
            return vehicleBodyTypeData.VehicleBodyTypes.ToArray();
        }

        [HttpPost]
        public object GetAllFuelTypes(JObject data)
        {
            ITASTPAManagementService TPAManagementService = ServiceFactory.GetTASTPAManagementService();

            FuelTypesResponseDto fuelTypeData = TPAManagementService.GetFuelTypes(
            SecurityHelper.Context,
            AuditHelper.Context, Guid.Parse(data["tpaId"].ToString()));
            return fuelTypeData.FuelTypes.ToArray();
        }

        [HttpPost]
        public object GetAllVehicleAspirationTypes(JObject data)
        {
            ITASTPAManagementService TPAManagementService = ServiceFactory.GetTASTPAManagementService();

            VehicleAspirationTypesResponseDto VehicleAspirationTypeData = TPAManagementService.GetVehicleAspirationTypes(
            SecurityHelper.Context,
            AuditHelper.Context, Guid.Parse(data["tpaId"].ToString()));
            return VehicleAspirationTypeData.VehicleAspirationTypes.ToArray();
        }

        [HttpPost]
        public object GetAllTransmissionTypes(JObject data)
        {
            ITASTPAManagementService TPAManagementService = ServiceFactory.GetTASTPAManagementService();

            TransmissionTypesResponseDto TransmissionTypeData = TPAManagementService.GetTransmissionTypes(
            SecurityHelper.Context,
            AuditHelper.Context, Guid.Parse(data["tpaId"].ToString()));
            return TransmissionTypeData.TransmissionTypes.ToArray();
        }

        [HttpPost]
        public object GetAllEngineCapacities(JObject data)
        {
            ITASTPAManagementService TPAManagementService = ServiceFactory.GetTASTPAManagementService();

            EngineCapacitiesResponseDto engineCapacityData = TPAManagementService.GetEngineCapacities(
            SecurityHelper.Context,
            AuditHelper.Context, Guid.Parse(data["tpaId"].ToString()));
            return engineCapacityData.EngineCapacities.ToArray();
        }


        [HttpPost]
        public object GetAllDriveTypes(JObject data)
        {
            ITASTPAManagementService TPAManagementService = ServiceFactory.GetTASTPAManagementService();

            DriveTypesResponseDto driveTypeData = TPAManagementService.GetDriveTypes(
            SecurityHelper.Context,
            AuditHelper.Context, Guid.Parse(data["tpaId"].ToString()));
            return driveTypeData.DriveTypes.ToArray();
        }


        [HttpPost]
        public object GetAllVariant(JObject data)
        {
            ITASTPAManagementService TPAManagementService = ServiceFactory.GetTASTPAManagementService();

            VariantsResponseDto VariantData = TPAManagementService.GetVariants(
            SecurityHelper.Context,
            AuditHelper.Context, Guid.Parse(data["tpaId"].ToString()));
            return VariantData.Variants.ToArray();
        }

        [HttpPost]
        public object GetAllDealersByCountryId(JObject data)
        {
            ITASTPAManagementService TPAManagementService = ServiceFactory.GetTASTPAManagementService();

            DealersRespondDto DealerData = TPAManagementService.GetAllDealersByCountryId(
            SecurityHelper.Context,
            AuditHelper.Context, Guid.Parse(data["Id"].ToString()), Guid.Parse(data["tpaId"].ToString()));
            return DealerData;
        }


        [HttpPost]
        public object GetAllDealerLocationsByDealerId(JObject data)
        {
            ITASTPAManagementService TPAManagementService = ServiceFactory.GetTASTPAManagementService();

            DealerLocationsRespondDto DealerLocationData = TPAManagementService.GetAllDealerLocationsByDealerId(
            SecurityHelper.Context,
            AuditHelper.Context, Guid.Parse(data["Id"].ToString()), Guid.Parse(data["tpaId"].ToString()));
            return DealerLocationData;
        }

        [HttpPost]
        public object GetAllCountries(JObject data)
        {
            ITASTPAManagementService TPAManagementService = ServiceFactory.GetTASTPAManagementService();

            CountriesResponseDto countryData = TPAManagementService.GetAllCountries(
            SecurityHelper.Context,
            AuditHelper.Context, Guid.Parse(data["tpaId"].ToString()));
            return countryData;
        }

        [HttpPost]
        public object GetAllCountriesThatHaveDealers(JObject data)
        {
            ITASTPAManagementService TPAManagementService = ServiceFactory.GetTASTPAManagementService();

            CountriesResponseDto countryData = TPAManagementService.GetAllCountriesThatHaveDealers(
            SecurityHelper.Context,
            AuditHelper.Context, Guid.Parse(data["tpaId"].ToString()));
            return countryData;
        }



        [HttpPost]
        public object GetExtensionTypesByDealerId(JObject data)
        {
            ITASTPAManagementService TPAManagementService = ServiceFactory.GetTASTPAManagementService();

            ExtensionTypesResponseDto ExtensionTypeData = TPAManagementService.GetExtensionTypesByDealerId(
            SecurityHelper.Context,
            AuditHelper.Context, Guid.Parse(data["tpaId"].ToString()), Guid.Parse(data["dealerId"].ToString()), Guid.Parse(data["modelId"].ToString()));
            return ExtensionTypeData;
        }

        [HttpPost]
        public object GetPrices(JObject data)
        {
            ITASTPAManagementService TPAManagementService = ServiceFactory.GetTASTPAManagementService();

            ContractPricesResponseDto ExtensionTypeData = TPAManagementService.GetPrices(
            SecurityHelper.Context,
            AuditHelper.Context, Guid.Parse(data["tpaId"].ToString()), Guid.Parse(data["dealerId"].ToString()), Guid.Parse(data["modelId"].ToString()), decimal.Parse(data["dealerPrice"].ToString()), decimal.Parse(data["itemPrice"].ToString())); // , Guid.Parse(data["extensionTypeId"].ToString())
            return ExtensionTypeData;
        }
        
        [HttpPost]
        public object GetAllNationalities(JObject data)
        {
            object Response = null;
            try
            {
                Guid tpaId = Guid.Parse(data["tpaId"].ToString());

                IPolicyManagementService policyManagementService = ServiceFactory.GetPolicyManagementService();
                Response = policyManagementService.GetAllCountries(tpaId);
            }
            catch (Exception ex)
            {
                Response = "Error occured";
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                             ex.InnerException);
            }
            return Response;
        }
        
        

        [HttpPost]
        public object GetAllAdditionalFieldDetails(JObject data)
        {
            object Response = null;
            try
            {
                Guid tpaId = Guid.Parse(data["tpaId"].ToString());
                string productCode = data["productCode"].ToString();

                IPolicyManagementService policyManagementService = ServiceFactory.GetPolicyManagementService();
                Response = policyManagementService.GetAllAdditionalFieldDetailsByProductCode(tpaId, productCode);
            }
            catch (Exception ex)
            {
                Response = "Error occured";
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                             ex.InnerException);
            }
            return Response;
        }

        [HttpPost]
        public object GetTireDetailsByInvoiceCode(JObject data)
        {
            object Response = null;
            try
            {
                Guid tpaId = Guid.Parse(data["tpaId"].ToString());
                string invoiceCode = data["invoiceCode"].ToString();

                IPolicyManagementService policyManagementService = ServiceFactory.GetPolicyManagementService();
                Response = policyManagementService.GetTireDetailsByInvoiceCode(tpaId, invoiceCode);
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                             ex.InnerException);
            }
            return Response;
        }


        [HttpPost]
        public object GetAllCitiesByCountryId(JObject data)
        {
            object Response = null;
            try
            {
                Guid tpaId = Guid.Parse(data["tpaId"].ToString());
                Guid countryId = Guid.Parse(data["countryId"].ToString());

                IPolicyManagementService policyManagementService = ServiceFactory.GetPolicyManagementService();
                Response = policyManagementService.GetAllCitiesByCountryId(tpaId, countryId);
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                             ex.InnerException);
            }
            return Response;
        }

        [HttpPost]
        public object GetAllUsageTypes(JObject data)
        {
            object Response = null;
            try
            {
                Guid tpaId = Guid.Parse(data["tpaId"].ToString());

                IPolicyManagementService policyManagementService = ServiceFactory.GetPolicyManagementService();
                Response = policyManagementService.GetAllUsageTypesByTpaId(tpaId);
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                             ex.InnerException);
            }
            return Response;
        }

        [HttpPost]
        public object GetAllCustomerTypes(JObject data)
        {
            object Response = null;
            try
            {
                Guid tpaId = Guid.Parse(data["tpaId"].ToString());

                IPolicyManagementService policyManagementService = ServiceFactory.GetPolicyManagementService();
                Response = policyManagementService.GetAllCustomerTypes(tpaId);
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                             ex.InnerException);
            }
            return Response;
        }

        [HttpPost]
        public object SaveCustomerEnterdPolicy(JObject data)
        {
            object response = null;
            try
            {
                SaveCustomerEnterdPolicyRequestDto saveCustomerPolicyDto = data.ToObject<SaveCustomerEnterdPolicyRequestDto>();
                IPolicyManagementService policyManagementService = ServiceFactory.GetPolicyManagementService();
                response = policyManagementService.SaveCustomerEnterdPolicy(saveCustomerPolicyDto);
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                             ex.InnerException);
            }
            return response;
        }

        [HttpPost]
        public object SaveCustomerInvoiceDetails(JObject data)
        {
            object response = null;
            try
            {
                SaveCustomerPolicyInfoRequestDto saveCustomerPolicyDto = data.ToObject<SaveCustomerPolicyInfoRequestDto>();
                IPolicyManagementService policyManagementService = ServiceFactory.GetPolicyManagementService();
                response = policyManagementService.SaveCustomerEnterdInvoiceDetails(saveCustomerPolicyDto);
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                             ex.InnerException);
            }
            return response;
        }

        [HttpPost]
        public object LoadSavedCustomerInvoiceData(JObject data)
        {
            object Response = null;
            try
            {
                Guid tpaId = Guid.Parse(data["tpaId"].ToString());
                Guid tempInvoiceId = Guid.Parse(data["tempInvoiceId"].ToString());

                IPolicyManagementService policyManagementService = ServiceFactory.GetPolicyManagementService();
                Response = policyManagementService.LoadSavedCustomerInvoiceDataById(tpaId, tempInvoiceId);
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                             ex.InnerException);
            }
            return Response;
        }


        [HttpPost]
        public object SaveOnlinePurchase(JObject data)
        {
            OnlinePurchaseRequestDto onlinePurData = data.ToObject<OnlinePurchaseRequestDto>();

            ITASTPAManagementService TPAManagementService = ServiceFactory.GetTASTPAManagementService();

            IPolicyManagementService PolicyManagementService = ServiceFactory.GetPolicyManagementService();
            OnlinePurchaseRequestDto result = PolicyManagementService.SaveOnlinePurchase(onlinePurData, SecurityHelper.Context, AuditHelper.Context);

            return result.PolicyInsertion;
        }
    }
}
