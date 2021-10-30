using Amazon.ElastiCacheCluster;
using Enyim.Caching;
using Enyim.Caching.Memcached;
using NLog;
using System;
using System.Collections.Generic;
using System.Reflection;
using TAS.Caching;
using TAS.DataTransfer.Responses;
using TAS.Services.Entities.Persistence;

namespace TAS.Services.Entities.Management
{
    internal static class EntityCacheData
    {
        private static Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public static List<CityResponseDto> GetCities(string UniqueDbName)
        {
            List<CityResponseDto> entities = null;

            try
            {
                ICache cache = CacheFactory.GetCache();

                string key = UniqueDbName + "_Cities";

                entities = cache[key] as List<CityResponseDto>;

                if (entities == null)
                {
                    CityEntityManager entityManager = new CityEntityManager();

                    entities = entityManager.GetAllCities();

                    cache[key] = entities;
                }

            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }

            return entities;
        }

        public static List<CityResponseDto> GetCities(Guid countryId, string UniqueDbName)
        {
            List<CityResponseDto> entities = null;

            try
            {
                ICache cache = CacheFactory.GetCache();

                string key = UniqueDbName + "_CitiesForCountry_" + countryId.ToString();

                entities = cache[key] as List<CityResponseDto>;

                if (entities == null)
                {
                    CityEntityManager entityManager = new CityEntityManager();

                    entities = entityManager.GetAllCitiesByCountryId(countryId);

                    cache[key] = entities;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }

            return entities;
        }


        /*sample*/

        internal static List<CommodityRespondDto> GetAllCommodities(string UniqueDbName)
        {
            //create response object
            List<CommodityRespondDto> Response = null;

            try
            {
                //check object in cache
                ICache cache = CacheFactory.GetCache();
                string uniqueCacheKey = UniqueDbName + "_Commodities";
                Response = cache[uniqueCacheKey] as List<CommodityRespondDto>;
                if (Response == null)
                {
                    //not available in cache
                    CommodityEntityManager commodityEntityManager = new CommodityEntityManager();
                    Response = commodityEntityManager.GetAllCommodities();
                    //add to cache
                    cache[uniqueCacheKey] = Response;
                }

            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        internal static MakesResponseDto GetMakesByCommodityCategoryId(string UniqueDbName, Guid commodityCategoryId)
        {
            MakesResponseDto Response = null;
            try
            {
                //check object in cache
                ICache cache = CacheFactory.GetCache();
                string uniqueCacheKey = UniqueDbName + "_MakesByCommodityCategoryId_" + commodityCategoryId.ToString().ToLower();
                Response = cache[uniqueCacheKey] as MakesResponseDto;
                if (Response == null)
                {
                    //not available in cache
                    MakeEntityManager MakeEntityManager = new MakeEntityManager();
                    Response = MakeEntityManager.GetMakesByCommodityCategoryId(commodityCategoryId);
                    //add to cache
                    cache[uniqueCacheKey] = Response;
                }

            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        internal static ProductResponseDto GetProductById(string UniqueDbName, Guid productId)
        {
            ProductResponseDto Response = null;
            try
            {
                //check object in cache
                ICache cache = CacheFactory.GetCache();
                string uniqueCacheKey = UniqueDbName + "_ProductById_"+ productId.ToString().ToLower();
                Response = cache[uniqueCacheKey] as ProductResponseDto;
                if (Response == null)
                {
                    //not available in cache
                    ProductEntityManager productEntityManager = new ProductEntityManager();
                    Response = productEntityManager.GetProductById(productId);
                    //add to cache
                    cache[uniqueCacheKey] = Response;
                }

            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        internal static List<DealerLocationRespondDto> GetAllDealerLocations(string UniqueDbName)
        {
            List<DealerLocationRespondDto> Response = null;
            try
            {
                //check object in cache
                ICache cache = CacheFactory.GetCache();
                string uniqueCacheKey = UniqueDbName + "_DealerLocations";
                Response = cache[uniqueCacheKey] as List<DealerLocationRespondDto>;
                if (Response == null)
                {
                    //not available in cache
                    DealerLocationEntityManager DealerLocationEntityManager = new DealerLocationEntityManager();
                    Response = DealerLocationEntityManager.GetAllDealerLocations();
                    //add to cache
                    cache[uniqueCacheKey] = Response;
                }

            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        internal static object GetAllProductsByCommodityTypeId(string UniqueDbName, Guid commodityTypeId)
        {
            List<ProductResponseDto> Response = null;
            try
            {
                //check object in cache
                ICache cache = CacheFactory.GetCache();
                string uniqueCacheKey = UniqueDbName + "_ProductsByCommodityType_" + commodityTypeId.ToString().ToLower();
                Response = cache[uniqueCacheKey] as List<ProductResponseDto>;
                if (Response == null)
                {
                    //not available in cache
                    ProductEntityManager ProductEntityManager = new ProductEntityManager();
                    Response = ProductEntityManager.GetAllProductsByCommodityTypeId(commodityTypeId);
                    //add to cache
                    cache[uniqueCacheKey] = Response;
                }

            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        internal static DealerTypeResponseDto DealerAccessRetrivalByUserId(string UniqueDbName, Guid userId)
        {
            DealerTypeResponseDto Response = null;
            try
            {
                //check object in cache
                ICache cache = CacheFactory.GetCache();
                string uniqueCacheKey = UniqueDbName + "_DealerUserType_" + userId.ToString().ToLower();
                Response = cache[uniqueCacheKey] as DealerTypeResponseDto;
                if (Response == null)
                {
                    //not available in cache
                    UserEntityManager UserEntityManager = new UserEntityManager();
                    Response = UserEntityManager.DealerAccessRetrivalByUserId(userId);
                    //add to cache
                    cache[uniqueCacheKey] = Response;
                }

            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        internal static List<ContractResponseDto> GetContractsByCommodityType(string UniqueDbName, Guid commodityTypeId)
        {
            List<ContractResponseDto> Response = null;
            try
            {
                //check object in cache
                ICache cache = CacheFactory.GetCache();
                string uniqueCacheKey = UniqueDbName + "_Contract_"+ commodityTypeId.ToString().ToLower();
                Response = cache[uniqueCacheKey] as List<ContractResponseDto>;
                if (Response == null)
                {
                    //not available in cache
                    ContractEntityManager ContractEntityManager = new ContractEntityManager();
                    Response = ContractEntityManager.GetContractsByCommodityType(commodityTypeId);
                    //add to cache
                    cache[uniqueCacheKey] = Response;
                }

            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        internal static List<VariantResponseDto> GetVariants(string UniqueDbName)
        {
            List<VariantResponseDto> Response = null;
            try
            {
                //check object in cache
                ICache cache = CacheFactory.GetCache();
                string uniqueCacheKey = UniqueDbName + "_Variants";
                Response = cache[uniqueCacheKey] as List<VariantResponseDto>;
                if (Response == null)
                {
                    //not available in cache
                    VariantEntityManager VariantEntityManager = new VariantEntityManager();
                    Response = VariantEntityManager.GetVariants();
                    //add to cache
                    cache[uniqueCacheKey] = Response;
                }

            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        internal static List<ProductResponseDto> GetAllProducts(string UniqueDbName)
        {
            List<ProductResponseDto> Response = null;
            try
            {
                //check object in cache
                ICache cache = CacheFactory.GetCache();
                string uniqueCacheKey = UniqueDbName + "_Products";
                Response = cache[uniqueCacheKey] as List<ProductResponseDto>;
                if (Response == null)
                {
                    //not available in cache
                    ProductEntityManager productEntityManager = new ProductEntityManager();
                    Response = productEntityManager.GetAllProducts();
                    //add to cache
                    cache[uniqueCacheKey] = Response;
                }

            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        internal static GetContractDetailsByContractIdDto GetContractDetailsByContractId(string UniqueDbName, Guid contractId)
        {
            GetContractDetailsByContractIdDto Response = null;
            try
            {
                //check object in cache
                ICache cache = CacheFactory.GetCache();
                string uniqueCacheKey = UniqueDbName + "_ContractId_" + contractId.ToString().ToLower();
                Response = cache[uniqueCacheKey] as GetContractDetailsByContractIdDto;
                if (Response == null)
                {
                    //not available in cache
                    Response = ContractEntityManager.GetContractDetailsByContractId(contractId);
                    //add to cache
                    cache[uniqueCacheKey] = Response;
                }

            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        internal static List<CountryTaxesResponseDto> GetCountryTaxesByCountryId(string UniqueDbName, Guid countryId)
        {
            List<CountryTaxesResponseDto> Response = null;
            try
            {
                //check object in cache
                ICache cache = CacheFactory.GetCache();
                string uniqueCacheKey = UniqueDbName + "_CountryTax_" + countryId.ToString().ToLower();
                Response = cache[uniqueCacheKey] as List<CountryTaxesResponseDto>;
                if (Response == null)
                {
                    //not available in cache
                    TaxEntityManager taxEntityManager = new TaxEntityManager();
                    Response = taxEntityManager.GetCountryTaxesByCountryId(countryId);
                    //add to cache
                    cache[uniqueCacheKey] = Response;
                }

            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        internal static List<DealerRespondDto> GetAllDealersByCountry(string UniqueDbName, Guid countryId)
        {
            List<DealerRespondDto> Response = null;
            try
            {
                //check object in cache
                ICache cache = CacheFactory.GetCache();
                string uniqueCacheKey = UniqueDbName + "_Dealers_"+ countryId.ToString().ToLower();
                Response = cache[uniqueCacheKey] as List<DealerRespondDto>;
                if (Response == null)
                {
                    //not available in cache
                    DealerEntityManager dealerEntityManager = new DealerEntityManager();
                    Response = dealerEntityManager.GetAllDealersByCountry(countryId);
                    //add to cache
                    cache[uniqueCacheKey] = Response;
                }

            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        internal static List<RegionResponseDto> GetRegions(string UniqueDbName)
        {
            List<RegionResponseDto> Response = null;
            try
            {
                //check object in cache
                ICache cache = CacheFactory.GetCache();
                string uniqueCacheKey = UniqueDbName + "_Regions";
                Response = cache[uniqueCacheKey] as List<RegionResponseDto>;
                if (Response == null)
                {
                    //not available in cache
                    RegionEntityManager regionEntityManager = new RegionEntityManager();
                    Response = regionEntityManager.GetRegions();
                    //add to cache
                    cache[uniqueCacheKey] = Response;
                }

            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        internal static List<RSAProviderResponseDto> GetRSAProviders(string UniqueDbName)
        {
            List<RSAProviderResponseDto> Response = null;
            try
            {
                //check object in cache
                ICache cache = CacheFactory.GetCache();
                string uniqueCacheKey = UniqueDbName + "_RSAProviders";
                Response = cache[uniqueCacheKey] as List<RSAProviderResponseDto>;
                if (Response == null)
                {
                    //not available in cache
                    RSAProviderEntityManager rSAProviderEntityManager = new RSAProviderEntityManager();
                    Response = rSAProviderEntityManager.GetRSAProviders();
                    //add to cache
                    cache[uniqueCacheKey] = Response;
                }

            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        internal static List<InsurerResponseDto> GetInsurers(string UniqueDbName)
        {
            List<InsurerResponseDto> Response = null;
            try
            {
                //check object in cache
                ICache cache = CacheFactory.GetCache();
                string uniqueCacheKey = UniqueDbName + "_Insurers";
                Response = cache[uniqueCacheKey] as List<InsurerResponseDto>;
                if (Response == null)
                {
                    //not available in cache
                    InsurerEntityManager insurerEntityManager = new InsurerEntityManager();
                    Response = insurerEntityManager.GetInsurers();
                    //add to cache
                    cache[uniqueCacheKey] = Response;
                }

            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        internal static object GetInsuaranceLimitationsByCommodityType(string uniqueDbName, Guid commodityTypeId, string productType)
        {
            List<InsuaranceLimitResponseDto> Response = null;
            try
            {
                bool isRsa = productType.ToLower() == "rsa" ? true : false;
                ICache cache = CacheFactory.GetCache();
                string uniqueCacheKey = uniqueDbName + "_InsLimit_" + commodityTypeId.ToString().ToLower()+"_"+ isRsa.ToString().ToLower();
                Response = cache[uniqueCacheKey] as List<InsuaranceLimitResponseDto>;
                if (Response == null)
                {
                    //not available in cache
                    Response = ContractEntityManager.GetInsuaranceLimitationsByCommodityType(commodityTypeId, productType);
                    //add to cache
                    cache[uniqueCacheKey] = Response;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        internal static List<TPAResponseDto> GetTPADetailById(string uniqueDbName, Guid tpaId)
        {
            List<TPAResponseDto> Response = null;
            try
            {
                ICache cache = CacheFactory.GetCache();
                string uniqueCacheKey = uniqueDbName + "_TPA_" + tpaId.ToString().ToLower();
                Response = cache[uniqueCacheKey] as List<TPAResponseDto>;
                if (Response == null)
                {
                    //not available in cache
                    TPAEntityManager TPAEntityManager = new TPAEntityManager();
                    Response = TPAEntityManager.GetTPADetailById(tpaId);
                    //add to cache
                    cache[uniqueCacheKey] = Response;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        internal static List<UserResponseDto> GetUsers(string uniqueDbName)
        {
            List<UserResponseDto> Response = null;
            try
            {
                ICache cache = CacheFactory.GetCache();
                string uniqueCacheKey = uniqueDbName + "_Users";
                Response = cache[uniqueCacheKey] as List<UserResponseDto>;
                if (Response == null)
                {
                    //not available in cache
                    UserEntityManager userEntityManager = new UserEntityManager();
                    Response = userEntityManager.GetUsers();
                    //add to cache
                    cache[uniqueCacheKey] = Response;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        internal static List<TransmissionTypeResponseDto> GetTransmissionTypes(string uniqueDbName)
        {
            List<TransmissionTypeResponseDto> Response = null;
            try
            {
                ICache cache = CacheFactory.GetCache();
                string uniqueCacheKey = uniqueDbName + "_TransmissionTypes";
                Response = cache[uniqueCacheKey] as List<TransmissionTypeResponseDto>;
                if (Response == null)
                {
                    //not available in cache
                    TransmissionTypeEntityManager TransmissionTypeEntityManager = new TransmissionTypeEntityManager();
                    Response = TransmissionTypeEntityManager.GetTransmissionTypes();
                    //add to cache
                    cache[uniqueCacheKey] = Response;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        internal static List<BrownAndWhiteDetailsResponseDto> GetBrownAndWhiteDetailss(string uniqueDbName)
        {
            List<BrownAndWhiteDetailsResponseDto> Response = null;
            try
            {
                ICache cache = CacheFactory.GetCache();
                string uniqueCacheKey = uniqueDbName + "_BrownAndWhiteDetails";
                Response = cache[uniqueCacheKey] as List<BrownAndWhiteDetailsResponseDto>;
                if (Response == null)
                {
                    //not available in cache
                    BrownAndWhiteDetailsEntityManager BrownAndWhiteDetailsEntityManager = new BrownAndWhiteDetailsEntityManager();
                    Response = BrownAndWhiteDetailsEntityManager.GetBrownAndWhiteDetailss();
                    //add to cache
                    cache[uniqueCacheKey] = Response;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        internal static List<VehicleDetailsResponseDto> GetVehicleDetails(string uniqueDbName)
        {
            List<VehicleDetailsResponseDto> Response = null;
            try
            {
                ICache cache = CacheFactory.GetCache();
                string uniqueCacheKey = uniqueDbName + "_VehicleDetails";
                Response = cache[uniqueCacheKey] as List<VehicleDetailsResponseDto>;
                if (Response == null)
                {
                    //not available in cache
                    VehicleDetailsEntityManager VehicleDetailsEntityManager = new VehicleDetailsEntityManager();

                    Response = VehicleDetailsEntityManager.GetVehicleDetailsOptimized();
                    //add to cache
                    cache[uniqueCacheKey] = Response;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        internal static List<CustomerResponseDto> GetCustomers(string UniqueDbName)
        {
            List<CustomerResponseDto> Response = null;
            try
            {
                ICache cache = CacheFactory.GetCache();
                string uniqueCacheKey = UniqueDbName + "_Customers";
                Response = cache[uniqueCacheKey] as List<CustomerResponseDto>;
                if (Response == null)
                {
                    //not available in cache
                    CustomerEntityManager customerEntityManager = new CustomerEntityManager();
                    Response = customerEntityManager.GetCustomers();
                    //add to cache
                    cache[uniqueCacheKey] = Response;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }


        /*end of sample*/

        internal static List<PartAreaResponseDto> GetAllPartArea(string UniqueDbName)
        {
            List<PartAreaResponseDto> Response = null;

            try
            {
                ICache cache = CacheFactory.GetCache();
                string uniqueCacheKey = UniqueDbName + "_PartAreas";
                Response = cache[uniqueCacheKey] as List<PartAreaResponseDto>;
                if (Response == null)
                {
                    //not available in cache
                    ClaimEntityManager claimEntityManager = new ClaimEntityManager();
                    Response = claimEntityManager.GetAllPartArea();
                    //add to cache
                    cache[uniqueCacheKey] = Response;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        internal static List<PartRejectionTypesResponseDto> GetAllPartRejectioDescription(string UniqueDbName)
        {
            List<PartRejectionTypesResponseDto> Response = null;

            try
            {
                ICache cache = CacheFactory.GetCache();
                string uniqueCacheKey = UniqueDbName + "_PartRejectionType";
                Response = cache[uniqueCacheKey] as List<PartRejectionTypesResponseDto>;
                if (Response == null)
                {
                    //not available in cache
                    ClaimEntityManager claimEntityManager = new ClaimEntityManager();
                    Response = claimEntityManager.GetAllPartRejectioDescription();
                    //add to cache
                    cache[uniqueCacheKey] = Response;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        internal static List<CountryInfo> GetAllCountries(string UniqueDbName)
        {
            List<CountryInfo> Response = null;

            try
            {
                ICache cache = CacheFactory.GetCache();
                string uniqueCacheKey = UniqueDbName + "_Countries";
                Response = cache[uniqueCacheKey] as List<CountryInfo>;
                if (Response == null)
                {
                    CountryEntityManager countryEntityManager = new CountryEntityManager();
                    Response = countryEntityManager.GetAllCountries();
                    cache[uniqueCacheKey] = Response;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }



        internal static List<CountryResponseDto> GetAllActiveCountries(string UniqueDbName)
        {
            List<CountryResponseDto> Response = null;

            try
            {
                ICache cache = CacheFactory.GetCache();
                string uniqueCacheKey = UniqueDbName + "_ActiveCountries";
                Response = cache[uniqueCacheKey] as List<CountryResponseDto>;
                if (Response == null)
                {
                    CountryEntityManager countryEntityManager = new CountryEntityManager();
                    Response = countryEntityManager.GetAllActiveCountries();
                    cache[uniqueCacheKey] = Response;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }


        internal static List<CommodityCategoryResponseDto> GetCommodityCategories(string UniqueDbName, Guid commodityTypeId)
        {
            List<CommodityCategoryResponseDto> Response = null;

            try
            {
                ICache cache = CacheFactory.GetCache();
                string uniqueCacheKey = UniqueDbName + "_CommodityCategories_"+ commodityTypeId.ToString().ToLower();
                Response = cache[uniqueCacheKey] as List<CommodityCategoryResponseDto>;
                if (Response == null)
                {
                    CommodityCategoryEntityManager CommodityCategoryEntityManager = new CommodityCategoryEntityManager();
                    Response = CommodityCategoryEntityManager.GetCommodityCategories(commodityTypeId);
                    cache[uniqueCacheKey] = Response;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        internal static List<CommodityUsageTypeResponseDto> GetAllCommodityUsageTypes(string UniqueDbName)
        {
            List<CommodityUsageTypeResponseDto> Response = null;

            try
            {
                ICache cache = CacheFactory.GetCache();

                string uniqueCacheKey = UniqueDbName + "_CommodityUsageTypes";
                Response = cache[uniqueCacheKey] as List<CommodityUsageTypeResponseDto>;
                if (Response == null)
                {
                    CommodityUsageTypeEntityManager CommodityUsageTypeEntityManager = new CommodityUsageTypeEntityManager();
                    Response = CommodityUsageTypeEntityManager.GetAllCommodityUsageTypes();
                    cache[uniqueCacheKey] = Response;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        internal static List<CurrencyResponseDto> GetAllCurrencies(string UniqueDbName)
        {
            List<CurrencyResponseDto> Response = null;
            try
            {
               ICache cache = CacheFactory.GetCache();
                string uniqueCacheKey = UniqueDbName + "_Currency";
                Response = cache[uniqueCacheKey] as List<CurrencyResponseDto>;
                if (Response == null)
                {
                    logger.Trace(MethodBase.GetCurrentMethod().Name + ": information: " + "Cache Retrieval Fail then access to database GetAllCurrencies");
                    CurrencyEntityManager CurrencyEntityManager = new CurrencyEntityManager();
                    Response = CurrencyEntityManager.GetAllCurrencies();
                    cache[uniqueCacheKey] = Response;
                    logger.Trace(MethodBase.GetCurrentMethod().Name + ": information: " + "Saved Currency to Cache");
                }
                else {
                    logger.Trace(MethodBase.GetCurrentMethod().Name + ": information: " + "Cache Retrieval Success and Returning Response GetAllCurrencies");
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        internal static List<CustomerTypeResponseDto> GetAllCustomerTypes(string UniqueDbName)
        {
            List<CustomerTypeResponseDto> Response = null;

            try
            {
                ICache cache = CacheFactory.GetCache();
                string uniqueCacheKey = UniqueDbName + "_CustomerTypes";

                Response = cache[uniqueCacheKey] as List<CustomerTypeResponseDto>;
                if (Response == null)
                {
                    CustomerTypeEntityManager customerTypeEntityManager = new CustomerTypeEntityManager();
                    Response = customerTypeEntityManager.GetAllCustomerTypes();
                    cache[uniqueCacheKey] = Response;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }

            return Response;
        }

        internal static List<CylinderCountResponseDto> GetCylinderCounts(string UniqueDbName)
        {
            List<CylinderCountResponseDto> Response = null;

            try
            {
                ICache cache = CacheFactory.GetCache();
                string uniqueCacheKey = UniqueDbName + "_CylinderCount";
                Response = cache[uniqueCacheKey] as List<CylinderCountResponseDto>;
                if (Response == null)
                {
                    CylinderCountEntityManager cylinderCountEntityManager = new CylinderCountEntityManager();
                    Response = cylinderCountEntityManager.GetCylinderCounts();
                    cache[uniqueCacheKey] = Response;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        internal static List<DealTypeResponseDto> GetDealTypes(string UniqueDbName)
        {
            List<DealTypeResponseDto> Response = null;

            try
            {
                ICache cache = CacheFactory.GetCache();
                string uniqueCacheKey = UniqueDbName + "_DealTypes";
                Response = cache[uniqueCacheKey] as List<DealTypeResponseDto>;
                if (Response == null)
                {
                    DealTypeEntityManager DealTypeEntityManager = new DealTypeEntityManager();
                    Response = DealTypeEntityManager.GetDealTypes();
                    cache[uniqueCacheKey] = Response;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }

            return Response;
        }

        internal static List<DriveTypeResponseDto> GetDriveTypes(string UniqueDbName)
        {
            List<DriveTypeResponseDto> Response = null;

            try
            {
                ICache cache = CacheFactory.GetCache();
                string uniqueCacheKey = UniqueDbName + "_DriveType";
                Response = cache[uniqueCacheKey] as List<DriveTypeResponseDto>;
                if (Response == null)
                {
                    DriveTypeEntityManager driveTypeEntityManager = new DriveTypeEntityManager();
                    Response = driveTypeEntityManager.GetDriveTypes();
                    cache[uniqueCacheKey] = Response;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }

            return Response;
        }

        internal static List<EngineCapacityResponseDto> GetEngineCapacities(string UniqueDbName)
        {
            List<EngineCapacityResponseDto> Response = null;

            try
            {
                ICache cache = CacheFactory.GetCache();
                string uniqueCacheKey = UniqueDbName + "_EngineCapacity";
                Response = cache[uniqueCacheKey] as List<EngineCapacityResponseDto>;
                if (Response == null)
                {
                    EngineCapacityEntityManager engineCapacityEntityManager = new EngineCapacityEntityManager();
                    Response = engineCapacityEntityManager.GetEngineCapacities();
                    cache[uniqueCacheKey] = Response;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }

            return Response;
        }

        internal static List<ExtensionTypeResponseDto> GetExtensionTypes(string UniqueDbName)
        {
            List<ExtensionTypeResponseDto> Response = null;

            try
            {
                ICache cache = CacheFactory.GetCache();
                string uniqueCacheKey = UniqueDbName + "_ExtensionType";
                Response = cache[uniqueCacheKey] as List<ExtensionTypeResponseDto>;
                if (Response == null)
                {
                    ExtensionTypeEntityManager ExtensionTypeEntityManager = new ExtensionTypeEntityManager();
                    Response = ExtensionTypeEntityManager.GetExtensionTypes();
                    cache[uniqueCacheKey] = Response;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }

            return Response;
        }

        internal static List<FuelTypeResponseDto> GetFuelTypes(string UniqueDbName)
        {
            List<FuelTypeResponseDto> Response = null;

            try
            {
                ICache cache = CacheFactory.GetCache();
                string uniqueCacheKey = UniqueDbName + "_FuelType";
                Response = cache[uniqueCacheKey] as List<FuelTypeResponseDto>;
                if (Response == null)
                {
                    FuelTypeEntityManager fuelTypeEntityManager = new FuelTypeEntityManager();
                    Response = fuelTypeEntityManager.GetFuelTypes();
                    cache[uniqueCacheKey] = Response;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }


            return Response;
        }

        internal static List<IdTypeResponseDto> GetAllIdTypes(string UniqueDbName)
        {
            List<IdTypeResponseDto> Response = null;

            try
            {
                ICache cache = CacheFactory.GetCache();
                string uniqueCacheKey = UniqueDbName + "_IdType";
                Response = cache[uniqueCacheKey] as List<IdTypeResponseDto>;
                if (Response == null)
                {
                    IdTypeEntityManager idTypeEntityManager = new IdTypeEntityManager();
                    Response = idTypeEntityManager.GetAllIdTypes();
                    cache[uniqueCacheKey] = Response;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }

            return Response;
        }

        internal static List<ItemStatusResponseDto> GetItemStatuss(string UniqueDbName)
        {
            List<ItemStatusResponseDto> Response = null;

            try
            {
                ICache cache = CacheFactory.GetCache();
                string uniqueCacheKey = UniqueDbName + "_ItemStatus";
                Response = cache[uniqueCacheKey] as List<ItemStatusResponseDto>;
                if (Response == null)
                {
                    ItemStatusEntityManager ItemStatusEntityManager = new ItemStatusEntityManager();
                    Response = ItemStatusEntityManager.GetItemStatuss();
                    cache[uniqueCacheKey] = Response;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        internal static List<MakeResponseDto> GetMakes(string UniqueDbName)
        {
            List<MakeResponseDto> Response = null;
            try
            {
                ICache cache = CacheFactory.GetCache();
                string uniqueCacheKey = UniqueDbName + "_Make";
                Response = cache[uniqueCacheKey] as List<MakeResponseDto>;
                if (Response == null)
                {
                    MakeEntityManager MakeEntityManager = new MakeEntityManager();
                    Response = MakeEntityManager.GetMakes();
                    cache[uniqueCacheKey] = Response;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        internal static List<ManufacturerResponseDto> GetAllManufacturers(string UniqueDbName)
        {
            List<ManufacturerResponseDto> Response = null;

            try
            {
                ICache cache = CacheFactory.GetCache();
                string uniqueCacheKey = UniqueDbName + "_ManufacturerWithCommodityTypes";
                Response = cache[uniqueCacheKey] as List<ManufacturerResponseDto>;
                if (Response == null)
                {
                    ManufacturerEntityManager ManufacturerEntityManager = new ManufacturerEntityManager();
                    Response = ManufacturerEntityManager.GetAllManufacturers();
                    cache[uniqueCacheKey] = Response;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        internal static List<MarritalStatusResponseDto> GetMarritalStatuses(string UniqueDbName)
        {
            List<MarritalStatusResponseDto> Response = null;

            try
            {
                ICache cache = CacheFactory.GetCache();
                string uniqueCacheKey = UniqueDbName + "_MaritalStatus";
                Response = cache[uniqueCacheKey] as List<MarritalStatusResponseDto>;
                if (Response == null)
                {
                    CustomerEntityManager MarritalStatusEntityManager = new CustomerEntityManager();
                    Response = MarritalStatusEntityManager.GetMarritalStatuses();
                    cache[uniqueCacheKey] = Response;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }

            return Response;
        }

        internal static List<MenuResponseDto> GetMenus(string UniqueDbName)
        {
            List<MenuResponseDto> Response = null;
            try
            {
                ICache cache = CacheFactory.GetCache();
                string uniqueCacheKey = UniqueDbName + "_Menu";
                Response = cache[uniqueCacheKey] as List<MenuResponseDto>;
                if (Response == null)
                {
                    MenuEntityManager MenuEntityManager = new MenuEntityManager();
                    Response = MenuEntityManager.GetMenus();
                    cache[uniqueCacheKey] = Response;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }

            return Response;
        }

        internal static List<NationalityResponseDto> GetAllNationalities(string UniqueDbName)
        {
            List<NationalityResponseDto> Response = null;

            try
            {
                ICache cache = CacheFactory.GetCache();
                string uniqueCacheKey = UniqueDbName + "_Nationalies";
                Response = cache[uniqueCacheKey] as List<NationalityResponseDto>;
                if (Response == null)
                {
                    NationalityEntityManager nationalityEntityManager = new NationalityEntityManager();
                    Response = nationalityEntityManager.GetAllNationalities();
                    cache[uniqueCacheKey] = Response;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }

            return Response;
        }

        internal static List<OccupationResponseDto> GetOccupations(string UniqueDbName)
        {
            List<OccupationResponseDto> Response = null;

            try
            {

            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }

            ICache cache = CacheFactory.GetCache();
            string uniqueCacheKey = UniqueDbName + "_Occupation";
            Response = cache[uniqueCacheKey] as List<OccupationResponseDto>;
            if (Response == null)
            {
                CustomerEntityManager OccupationEntityManager = new CustomerEntityManager();
                Response = OccupationEntityManager.GetOccupations();
                cache[uniqueCacheKey] = Response;
            }
            return Response;
        }

        internal static List<NRPCommissionTypesResponseDto> GetNRPCommissionTypess(string UniqueDbName)
        {
            List<NRPCommissionTypesResponseDto> Response = null;

            try
            {
                ICache cache = CacheFactory.GetCache();
                string uniqueCacheKey = UniqueDbName + "_NRPCommissionType";
                Response = cache[uniqueCacheKey] as List<NRPCommissionTypesResponseDto>;
                if (Response == null)
                {
                    NRPCommissionTypesEntityManager NRPCommissionTypesEntityManager = new NRPCommissionTypesEntityManager();
                    Response = NRPCommissionTypesEntityManager.GetNRPCommissionTypess();
                    cache[uniqueCacheKey] = Response;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }

            return Response;
        }

        internal static List<PaymetModeResponseDto> GetPaymentModes(string UniqueDbName)
        {
            List<PaymetModeResponseDto> Response = null;

            try
            {
                ICache cache = CacheFactory.GetCache();
                string uniqueCacheKey = UniqueDbName + "_PaymentMode";
                Response = cache[uniqueCacheKey] as List<PaymetModeResponseDto>;
                if (Response == null)
                {
                    PaymentEntityManager PaymentEntityManager = new PaymentEntityManager();
                    Response = PaymentEntityManager.GetPaymentModes();
                    cache[uniqueCacheKey] = Response;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }

            return Response;
        }

        internal static List<PolicyTransactionTypeResponseDto> GetPolicyTransactionTypes(string UniqueDbName)
        {
            List<PolicyTransactionTypeResponseDto> Response = null;

            try
            {
                ICache cache = CacheFactory.GetCache();
                string uniqueCacheKey = UniqueDbName + "_PolicyTransactionType";
                Response = cache[uniqueCacheKey] as List<PolicyTransactionTypeResponseDto>;
                if (Response == null)
                {
                    PolicyEntityManager PolicyEntityManager = new PolicyEntityManager();
                    Response = PolicyEntityManager.GetPolicyTransactionTypes();
                    cache[uniqueCacheKey] = Response;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }

            return Response;
        }

        internal static List<PremiumAddonTypeResponseDto> GetPremiumAddonTypes(string UniqueDbName)
        {
            List<PremiumAddonTypeResponseDto> Response = null;

            try
            {
                ICache cache = CacheFactory.GetCache();
                string uniqueCacheKey = UniqueDbName + "_PremiumAddonType";
                Response = cache[uniqueCacheKey] as List<PremiumAddonTypeResponseDto>;
                if (Response == null)
                {
                    PremiumAddonTypeEntityManager PremiumAddonTypeEntityManager = new PremiumAddonTypeEntityManager();
                    Response = PremiumAddonTypeEntityManager.GetPremiumAddonTypes();
                    cache[uniqueCacheKey] = Response;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }

            return Response;
        }

        internal static List<PremiumBasedOnResponseDto> GetPremiumBasedOns(string UniqueDbName)
        {
            List<PremiumBasedOnResponseDto> Response = null;

            try
            {
                ICache cache = CacheFactory.GetCache();
                string uniqueCacheKey = UniqueDbName + "_PremiumBasedOns";
                Response = cache[uniqueCacheKey] as List<PremiumBasedOnResponseDto>;
                if (Response == null)
                {
                    PremiumBasedOnEntityManager PremiumBasedOnEntityManager = new PremiumBasedOnEntityManager();
                    Response = PremiumBasedOnEntityManager.GetPremiumBasedOns();
                    cache[uniqueCacheKey] = Response;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }

            return Response;
        }

        internal static List<ProductTypeResponseDto> GetProductTypes(string UniqueDbName)
        {
            List<ProductTypeResponseDto> Response = null;

            try
            {
                ICache cache = CacheFactory.GetCache();
                string uniqueCacheKey = UniqueDbName + "_ProductType";
                Response = cache[uniqueCacheKey] as List<ProductTypeResponseDto>;
                if (Response == null)
                {
                    ProductTypeEntityManager ProductTypeEntityManager = new ProductTypeEntityManager();
                    Response = ProductTypeEntityManager.GetProductTypes();
                    cache[uniqueCacheKey] = Response;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }

            return Response;
        }

        internal static List<TaxResponseDto> GetAllTaxes(string UniqueDbName)
        {
            List<TaxResponseDto> Response = null;

            try
            {
                ICache cache = CacheFactory.GetCache();
                string uniqueCacheKey = UniqueDbName + "_TaxTypes";
                Response = cache[uniqueCacheKey] as List<TaxResponseDto>;
                if (Response == null)
                {
                    TaxEntityManager TaxEntityManager = new TaxEntityManager();
                    Response = TaxEntityManager.GetAllTaxes();
                    cache[uniqueCacheKey] = Response;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        internal static List<FaultCategoryResponseDto> GetAllFaultCategorys(string UniqueDbName)
        {
            List<FaultCategoryResponseDto> Response = null;

            try
            {
                ICache cache = CacheFactory.GetCache();
                string uniqueCacheKey = UniqueDbName + "_FaultCategory";
                Response = cache[uniqueCacheKey] as List<FaultCategoryResponseDto>;
                if (Response == null)
                {
                    FaultEntityManager FaultEntityManager = new FaultEntityManager();
                    Response = FaultEntityManager.GetAllFaultCategorys();
                    cache[uniqueCacheKey] = Response;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }

            return Response;
        }

        internal static List<FaultAreaResponseDto> GetAllFaultAreas(string UniqueDbName)
        {
            List<FaultAreaResponseDto> Response = null;

            try
            {
                ICache cache = CacheFactory.GetCache();
                string uniqueCacheKey = UniqueDbName + "_FaultArea";
                Response = cache[uniqueCacheKey] as List<FaultAreaResponseDto>;
                if (Response == null)
                {
                    FaultEntityManager FaultEntityManager = new FaultEntityManager();
                    Response = FaultEntityManager.GetAllFaultAreas();
                    cache[uniqueCacheKey] = Response;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }

            return Response;
        }

        internal static List<FaultResponseDto> GetAllFaults(string UniqueDbName)
        {
            List<FaultResponseDto> Response = null;

            try
            {
                ICache cache = CacheFactory.GetCache();
                string uniqueCacheKey = UniqueDbName + "_Fault";
                Response = cache[uniqueCacheKey] as List<FaultResponseDto>;
                if (Response == null)
                {
                    FaultEntityManager FaultEntityManager = new FaultEntityManager();
                    Response = FaultEntityManager.GetAllFaults();
                    cache[uniqueCacheKey] = Response;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        internal static List<TimeZoneResponseDtos> GetAllTimeZones(string UniqueDbName)
        {
            List<TimeZoneResponseDtos> Response = null;
            try
            {
                ICache cache = CacheFactory.GetCache();
                string uniqueCacheKey = UniqueDbName + "_TimeZone";
                Response = cache[uniqueCacheKey] as List<TimeZoneResponseDtos>;
                if (Response == null)
                {
                    TPABranchEntityManager TPABranchEntityManager = new TPABranchEntityManager();
                    Response = TPABranchEntityManager.GetAllTimeZones();
                    cache[uniqueCacheKey] = Response;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        internal static List<TitleResponseDto> GetTitles(string UniqueDbName)
        {
            List<TitleResponseDto> Response = null;

            try
            {
                ICache cache = CacheFactory.GetCache();
                string uniqueCacheKey = UniqueDbName + "_Title";
                Response = cache[uniqueCacheKey] as List<TitleResponseDto>;
                if (Response == null)
                {
                    CustomerEntityManager TitleEntityManager = new CustomerEntityManager();
                    Response = TitleEntityManager.GetTitles();
                    cache[uniqueCacheKey] = Response;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        internal static List<UsageTypeResponseDto> GetAllUsageTypes(string UniqueDbName)
        {
            List<UsageTypeResponseDto> Response = null;

            try
            {
                ICache cache = CacheFactory.GetCache();
                string uniqueCacheKey = UniqueDbName + "_UsageType";
                Response = cache[uniqueCacheKey] as List<UsageTypeResponseDto>;
                if (Response == null)
                {
                    UsageTypeEntityManager usageTypeEntityManager = new UsageTypeEntityManager();
                    Response = usageTypeEntityManager.GetAllUsageTypes();
                    cache[uniqueCacheKey] = Response;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }

            return Response;
        }

        internal static List<UserTypeResponseDto> GetUserTypes(string UniqueDbName)
        {
            List<UserTypeResponseDto> Response = null;

            try
            {
                ICache cache = CacheFactory.GetCache();
                string uniqueCacheKey = UniqueDbName + "_UserType";
                Response = cache[uniqueCacheKey] as List<UserTypeResponseDto>;
                if (Response == null)
                {
                    UserEntityManager UserEntityManager = new UserEntityManager();
                    Response = UserEntityManager.GetUserTypes();
                    cache[uniqueCacheKey] = Response;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }

            return Response;
        }

        internal static List<VehicleColorResponseDto> GetVehicleColors(string UniqueDbName)
        {
            List<VehicleColorResponseDto> Response = null;

            try
            {
                ICache cache = CacheFactory.GetCache();
                string uniqueCacheKey = UniqueDbName + "_VehicleColor";
                Response = cache[uniqueCacheKey] as List<VehicleColorResponseDto>;
                if (Response == null)
                {
                    VehicleColorEntityManager vehicleColorEntityManager = new VehicleColorEntityManager();
                    Response = vehicleColorEntityManager.GetVehicleColors();
                    cache[uniqueCacheKey] = Response;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }

            return Response;
        }

        internal static List<VehicleBodyTypeResponseDto> GetVehicleBodyTypes(string UniqueDbName)
        {
            List<VehicleBodyTypeResponseDto> Response = null;

            try
            {
                ICache cache = CacheFactory.GetCache();
                string uniqueCacheKey = UniqueDbName + "_VehicleBodyType";
                Response = cache[uniqueCacheKey] as List<VehicleBodyTypeResponseDto>;
                if (Response == null)
                {
                    VehicleBodyTypeEntityManager vehicleBodyTypeEntityManager = new VehicleBodyTypeEntityManager();
                    Response = vehicleBodyTypeEntityManager.GetVehicleBodyTypes();
                    cache[uniqueCacheKey] = Response;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        internal static List<VehicleAspirationTypeResponseDto> GetVehicleAspirationTypes(string UniqueDbName)
        {
            List<VehicleAspirationTypeResponseDto> Response = null;

            try
            {
                ICache cache = CacheFactory.GetCache();
                string uniqueCacheKey = UniqueDbName + "_VehicleAspirationType";
                Response = cache[uniqueCacheKey] as List<VehicleAspirationTypeResponseDto>;
                if (Response == null)
                {
                    VehicleAspirationTypeEntityManager VehicleAspirationTypeEntityManager = new VehicleAspirationTypeEntityManager();
                    Response = VehicleAspirationTypeEntityManager.GetVehicleAspirationTypes();
                    cache[uniqueCacheKey] = Response;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }

            return Response;
        }

        internal static List<WarrantyTypeResponseDto> GetWarrantyTypes(string UniqueDbName)
        {
            List<WarrantyTypeResponseDto> Response = null;

            try
            {
                ICache cache = CacheFactory.GetCache();
                string uniqueCacheKey = UniqueDbName + "_WarrantyType";
                Response = cache[uniqueCacheKey] as List<WarrantyTypeResponseDto>;
                if (Response == null)
                {
                    WarrantyTypeEntityManager WarrantyTypeEntityManager = new WarrantyTypeEntityManager();
                    Response = WarrantyTypeEntityManager.GetWarrantyTypes();
                    cache[uniqueCacheKey] = Response;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }

            return Response;
        }

        internal static List<TPAResponseDto> GetAllTPAs(string UniqueDbName)
        {
            List<TPAResponseDto> Response = null;

            try
            {
                ICache cache = CacheFactory.GetCache();
                string uniqueCacheKey = UniqueDbName + "_TPA";
                Response = cache[uniqueCacheKey] as List<TPAResponseDto>;
                if (Response == null)
                {
                    TPAEntityManager TPAEntityManager = new TPAEntityManager();
                    Response = TPAEntityManager.GetAllTPAs();
                    cache[uniqueCacheKey] = Response;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        internal static List<ClaimBatchTableResponseDto> GetAllClaimBatching(string UniqueDbName)
        {
            List<ClaimBatchTableResponseDto> Response = null;

            try
            {
                ICache cache = CacheFactory.GetCache();
                string uniqueCacheKey = UniqueDbName + "_ClaimBatch";
                Response = cache[uniqueCacheKey] as List<ClaimBatchTableResponseDto>;
                if (Response == null)
                {
                    //not available in cache
                    ClaimBatchingEntityManager claimBatchingEntityManager = new ClaimBatchingEntityManager();
                    Response = claimBatchingEntityManager.GetAllClaimBatching();
                    //add to cache
                    cache[uniqueCacheKey] = Response;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }

            return Response;
        }

        internal static List<ClaimResponseDto> GetAllClaimDetailsIsBachingFalse(string UniqueDbName)
        {
            List<ClaimResponseDto> Response = null;

            try
            {
                ICache cache = CacheFactory.GetCache();
                string uniqueCacheKey = UniqueDbName + "_Claim";
                Response = cache[uniqueCacheKey] as List<ClaimResponseDto>;
                if (Response == null)
                {
                    //not available in cache
                    ClaimBatchingEntityManager claimBatchingEntityManager = new ClaimBatchingEntityManager();
                    Response = claimBatchingEntityManager.GetAllClaimDetailsIsBachingFalse();
                    //add to cache
                    cache[uniqueCacheKey] = Response;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        internal static List<ContractTaxesResponseDto> GetAllContactTaxes(string UniqueDbName)
        {
            List<ContractTaxesResponseDto> Response = null;
            try
            {
                ICache cache = CacheFactory.GetCache();
                string uniqueCacheKey = UniqueDbName + "_ContractTaxes";
                Response = cache[uniqueCacheKey] as List<ContractTaxesResponseDto>;
                if (Response == null)
                {
                    TaxEntityManager TaxEntityManager = new TaxEntityManager();
                    Response = TaxEntityManager.GetAllContactTaxes();
                    cache[uniqueCacheKey] = Response;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }

            return Response;
        }

        internal static List<VehicleWeightResponseDto> GetVehicleWeights(string UniqueDbName)
        {
            List<VehicleWeightResponseDto> Response = null;

            try
            {
                ICache cache = CacheFactory.GetCache();
                string uniqueCacheKey = UniqueDbName + "_VehicleWeight";
                Response = cache[uniqueCacheKey] as List<VehicleWeightResponseDto>;
                if (Response == null)
                {
                    VehicleWeightEntityManager vehicleWeightEntityManager = new VehicleWeightEntityManager();
                    Response = vehicleWeightEntityManager.GetVehicleWeights();
                    cache[uniqueCacheKey] = Response;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }


            return Response;
        }

        internal static List<FaultCauseOfFailureDto> GetAllFaultCauseOfFailures(string UniqueDbName)
        {
            List<FaultCauseOfFailureDto> Response = null;

            try
            {
                ICache cache = CacheFactory.GetCache();
                string uniqueCacheKey = UniqueDbName + "_FaultCauseOfFailure";
                Response = cache[uniqueCacheKey] as List<FaultCauseOfFailureDto>;
                if (Response == null)
                {
                    FaultEntityManager FaultEntityManager = new FaultEntityManager();
                    Response = FaultEntityManager.GetAllFaultCauseOfFailures();
                    cache[uniqueCacheKey] = Response;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        internal static List<InsuaranceLimitationResponseDto> GetInsuararceLimitation(string UniqueDbName)
        {
            List<InsuaranceLimitationResponseDto> Response = null;
            try
            {
                ICache cache = CacheFactory.GetCache();
                string uniqueCacheKey = UniqueDbName + "_InsuaranceLimitation";
                Response = cache[uniqueCacheKey] as List<InsuaranceLimitationResponseDto>;
                if (Response == null)
                {
                    ContractEntityManager ContractEntityManager = new ContractEntityManager();
                    Response = ContractEntityManager.GetInsuararceLimitation();
                    cache[uniqueCacheKey] = Response;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }

            return Response;
        }

        internal static List<DealerRespondDto> GetAllDealers(string UniqueDbName) {
            List<DealerRespondDto> Response = null;
            try
            {
                ICache cache = CacheFactory.GetCache();
                string uniqueCacheKey = UniqueDbName + "_Dealers";
                Response = cache[uniqueCacheKey] as List<DealerRespondDto>;
                if (Response == null)
                {
                    DealerEntityManager dealerEntityManager = new DealerEntityManager();
                    Response = dealerEntityManager.GetAllDealers();
                    cache[uniqueCacheKey] = Response;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }

            return Response;
        }

        internal static List<CurrencyConversionResponseDto> GetCurrencyConversions(string uniqueDbName)
        {
            List<CurrencyConversionResponseDto> Response = null;
            try
            {
                ICache cache = CacheFactory.GetCache();
                string uniqueCacheKey = uniqueDbName + "_CurrencyConversion";
                Response = cache[uniqueCacheKey] as List<CurrencyConversionResponseDto>;
                if (Response == null)
                {
                    //not available in cache
                    CurrencyEntityManager CurrencyEntityManager = new CurrencyEntityManager();
                    Response = CurrencyEntityManager.GetCurrencyConversions();

                    //add to cache
                    cache[uniqueCacheKey] = Response;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }


    }

}

