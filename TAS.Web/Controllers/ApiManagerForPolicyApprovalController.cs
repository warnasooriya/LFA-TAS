using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Http;
using TAS.DataTransfer.Requests;
using TAS.DataTransfer.Responses;
using TAS.Services;
using TAS.Services.Common;
using TAS.Web.Common;

namespace TAS.Web.Controllers
{
    public class ApiManagerForPolicyApprovalController : ApiController
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        [HttpPost]
        public object GetInizializationData(JObject data)
        {
            string baseUrl = ConfigurationData.BackendEndPointUrl;
            string token = Request.Headers.Authorization.ToString();
            SecurityHelper.Context.setToken(token);
            PolicyApprovalInizializeResponseDto policyApprovalInizializeResponseDto = new PolicyApprovalInizializeResponseDto();
            ThreadPool.SetMaxThreads(5, 0);

            string url = baseUrl + "CommodityItemAttributes/GetAllCommodityUsageTypes";
            Thread myNewThread = new Thread(() => policyApprovalInizializeResponseDto.GetAllCommodityUsageTypes = executeApis(url, data.ToString(), token));

            string url1 = baseUrl + "ContractManagement/GetPremiumBasedOns";
            Thread myNewThread1 = new Thread(() => policyApprovalInizializeResponseDto.GetPremiumBasedOns = executeApis(url1, data.ToString(), token));

            string url2 = baseUrl + "DealerManagement/GetCurrencies";
            Thread myNewThread2 = new Thread(() => policyApprovalInizializeResponseDto.GetCurrencies = executeApis(url2, data.ToString(), token));

            string url4 = baseUrl + "DealerManagement/GetAllDealers";
            Thread myNewThread4 = new Thread(() => policyApprovalInizializeResponseDto.GetAllDealers = executeApis(url4, data.ToString(), token));

            string url5 = baseUrl + "Payment/GetAllPaymentModes";
            Thread myNewThread5 = new Thread(() => policyApprovalInizializeResponseDto.GetAllPaymentModes = executeApis(url5, data.ToString(), token));

            string url6 = baseUrl + "Customer/GetAllCountries";
            Thread myNewThread6 = new Thread(() => policyApprovalInizializeResponseDto.GetAllCountries = executeApis(url6, data.ToString(), token));

            string url7 = baseUrl + "Customer/GetAllNationalities";
            Thread myNewThread7 = new Thread(() => policyApprovalInizializeResponseDto.GetAllNationalities = executeApis(url7, data.ToString(), token));

            string url8 = baseUrl + "Customer/GetAllCustomerTypes";
            Thread myNewThread8 = new Thread(() => policyApprovalInizializeResponseDto.GetAllCustomerTypes = executeApis(url8, data.ToString(), token));

            string url9 = baseUrl + "Customer/GetAllUsageTypes";
            Thread myNewThread9 = new Thread(() => policyApprovalInizializeResponseDto.GetAllUsageTypes = executeApis(url9, data.ToString(), token));

            string url10 = baseUrl + "Customer/GetAllIdTypes";
            Thread myNewThread10 = new Thread(() => policyApprovalInizializeResponseDto.GetAllIdTypes = executeApis(url10, data.ToString(), token));

            string url11 = baseUrl + "Customer/GetAllCustomers";
            Thread myNewThread11 = new Thread(() => policyApprovalInizializeResponseDto.GetAllCustomers = executeApis(url11, data.ToString(), token));

            string url12 = baseUrl + "VehicleDetails/GetAllVehicleDetails";
            Thread myNewThread12 = new Thread(() => policyApprovalInizializeResponseDto.GetAllVehicleDetails = executeApis(url12, data.ToString(), token));

            string url13 = baseUrl + "BrownAndWhiteDetails/GetAllBrownAndWhiteDetails";
            Thread myNewThread13 = new Thread(() => policyApprovalInizializeResponseDto.GetAllBrownAndWhiteDetails = executeApis(url13, data.ToString(), token));

            string url14 = baseUrl + "CommodityItemAttributes/GetAllItemStatuss";
            Thread myNewThread14 = new Thread(() => policyApprovalInizializeResponseDto.GetAllItemStatuss = executeApis(url14, data.ToString(), token));

            string url15 = baseUrl + "AutomobileAttributes/GetAllCylinderCounts";
            Thread myNewThread15 = new Thread(() => policyApprovalInizializeResponseDto.GetAllCylinderCounts = executeApis(url15, data.ToString(), token));


            string url16 = baseUrl + "AutomobileAttributes/GetAllDriveTypes";
            Thread myNewThread16 = new Thread(() => policyApprovalInizializeResponseDto.GetAllDriveTypes = executeApis(url16, data.ToString(), token));

            string url17 = baseUrl + "AutomobileAttributes/GetAllEngineCapacities";
            Thread myNewThread17 = new Thread(() => policyApprovalInizializeResponseDto.GetAllEngineCapacities = executeApis(url17, data.ToString(), token));

            string url18 = baseUrl + "AutomobileAttributes/GetAllFuelTypes";
            Thread myNewThread18 = new Thread(() => policyApprovalInizializeResponseDto.GetAllFuelTypes = executeApis(url18, data.ToString(), token));

            string url19 = baseUrl + "AutomobileAttributes/GetAllVehicleBodyTypes";
            Thread myNewThread19 = new Thread(() => policyApprovalInizializeResponseDto.GetAllVehicleBodyTypes = executeApis(url19, data.ToString(), token));

            string url20 = baseUrl + "AutomobileAttributes/GetAllTransmissionTypes";
            Thread myNewThread20 = new Thread(() => policyApprovalInizializeResponseDto.GetAllTransmissionTypes = executeApis(url20, data.ToString(), token));

            string url21 = baseUrl + "AutomobileAttributes/GetAllVehicleAspirationTypes";
            Thread myNewThread21 = new Thread(() => policyApprovalInizializeResponseDto.GetAllVehicleAspirationTypes = executeApis(url21, data.ToString(), token));



            myNewThread.Start();
            myNewThread1.Start();
            myNewThread2.Start();
            myNewThread4.Start();
            myNewThread5.Start();
            myNewThread6.Start();
            myNewThread7.Start();
            myNewThread8.Start();
            myNewThread9.Start();
            myNewThread10.Start();
            myNewThread11.Start();
            myNewThread12.Start();
            myNewThread13.Start();
            myNewThread14.Start();
            myNewThread15.Start();
            myNewThread16.Start();
            myNewThread17.Start();
            myNewThread18.Start();
            myNewThread19.Start();
            myNewThread20.Start();
            myNewThread21.Start();

            myNewThread.Join();
            myNewThread1.Join();
            myNewThread2.Join();
            myNewThread4.Join();
            myNewThread5.Join();
            myNewThread6.Join();
            myNewThread7.Join();
            myNewThread8.Join();
            myNewThread9.Join();
            myNewThread10.Join();
            myNewThread11.Join();
            myNewThread12.Join();
            myNewThread13.Join();
            myNewThread14.Join();
            myNewThread15.Join();
            myNewThread16.Join();
            myNewThread17.Join();
            myNewThread18.Join();
            myNewThread19.Join();
            myNewThread20.Join();
            myNewThread21.Join();
            myNewThread21.Join();

            PolicyApprovalInizializeResponseDto obj = GetDataInSerailProcesss(data);
            policyApprovalInizializeResponseDto.getDocumentTypesByPageName = obj.getDocumentTypesByPageName;
            policyApprovalInizializeResponseDto.GetUsers = obj.GetUsers;

            return policyApprovalInizializeResponseDto;
        }


        private object executeApis(string url, string data, string token)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(token);
            HttpContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            object json = null;
            if (response.IsSuccessStatusCode)
            {
                string jsonString = response.Content.ReadAsStringAsync().Result;
                json = JsonConvert.DeserializeObject<object>(jsonString);

            }
            return json;
        }

        private PolicyApprovalInizializeResponseDto GetDataInSerailProcesss(JObject data)
        {

            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            PolicyApprovalInizializeResponseDto policyApprovalInizializeResponseDto = new PolicyApprovalInizializeResponseDto();
            #region GetDocumentTypesByPageName
            // calling GetDocumentTypesByPageName
            String PageName = data["PageName"].ToString();
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            IPolicyManagementService PolicyManagementService = ServiceFactory.GetPolicyManagementService();
            policyApprovalInizializeResponseDto.getDocumentTypesByPageName = PolicyManagementService.GetDocumentTypesByPageName(PageName, SecurityHelper.Context, AuditHelper.Context);

            #endregion


            #region GetUsers
            // calling User/GetUsers
            var mn = (string[])Request.Headers.GetValues("RequestPage");
            var uId = (string[])Request.Headers.GetValues("RequestUserId");
            if (mn.Length > 0)
            {
                SecurityHelper.Context.MenuName = mn[0];
                if (uId.Length > 0)
                {
                    SecurityHelper.Context.UserId = uId[0];
                }
            }

            var UserManagementService = ServiceFactory.GetUserManagementService();
            var UsersData = UserManagementService.GetUsers(
            SecurityHelper.Context,
            AuditHelper.Context);
            foreach (var item in UsersData.Users)
            {
                item.Id = item.Id.ToLower();
                item.Password = "";
            }
            policyApprovalInizializeResponseDto.GetUsers = UsersData;
            #endregion

            return policyApprovalInizializeResponseDto;

        }


        [HttpPost]
        public object GetInizializationDataSerial(JObject data)
        {
            string baseUrl = ConfigurationData.BackendEndPointUrl;
            string token = Request.Headers.Authorization.ToString();
            SecurityHelper.Context.setToken(token);
            PolicyApprovalInizializeResponseDto policyApprovalInizializeResponseDto = new PolicyApprovalInizializeResponseDto();


            ICommodityUsageTypeManagementService CommodityUsageTypeManagementService = ServiceFactory.GetCommodityUsageTypeManagementService();
            CommodityUsageTypesResponseDto CommodityUsageTypeData = CommodityUsageTypeManagementService.GetCommodityUsageTypes(
            SecurityHelper.Context,
            AuditHelper.Context);
            policyApprovalInizializeResponseDto.GetAllCommodityUsageTypes = CommodityUsageTypeData.CommodityUsageTypes.ToArray();


            IPremiumBasedOnManagementService PremiumBasedOnManagementService = ServiceFactory.GetPremiumBasedOnManagementService();
            PremiumBasedOnesResponseDto PremiumBasedOns = PremiumBasedOnManagementService.GetPremiumBasedOns(
            SecurityHelper.Context,
            AuditHelper.Context);
            policyApprovalInizializeResponseDto.GetPremiumBasedOns = PremiumBasedOns.PremiumBasedOns.ToArray();


            ICurrencyManagementService CurrencyManagementService = ServiceFactory.GetCurrencyManagementService();
            CurrenciesResponseDto CurrencyData = CurrencyManagementService.GetAllCurrencies(
            SecurityHelper.Context,
            AuditHelper.Context);
            policyApprovalInizializeResponseDto.GetCurrencies = CurrencyData.Currencies.ToArray();


            IDealerManagementService DealerManagementService = ServiceFactory.GetDealerManagementService();
            DealersRespondDto DealerData = DealerManagementService.GetAllDealers(
            SecurityHelper.Context,
            AuditHelper.Context);
            policyApprovalInizializeResponseDto.GetAllDealers = DealerData.Dealers.ToArray();


            IPaymentManagementService PaymentManagementService = ServiceFactory.GetPaymentManagementService();
            policyApprovalInizializeResponseDto.GetAllPaymentModes = PaymentManagementService.GetAllPaymentModes(SecurityHelper.Context, AuditHelper.Context);


            ICountryManagementService countryManagementService = ServiceFactory.GetCountryManagementService();
            CountriesResponseDto countryData = countryManagementService.GetAllCountries(SecurityHelper.Context, AuditHelper.Context);
            policyApprovalInizializeResponseDto.GetAllCountries = countryData.Countries;


            INationalityManagementService nationalityManagementService = ServiceFactory.GetNationalityManagementService();
            NationalitiesResponseDto nationalityData = nationalityManagementService.GetAllNationalities(
            SecurityHelper.Context,
            AuditHelper.Context);
            policyApprovalInizializeResponseDto.GetAllNationalities = nationalityData.Nationalities.ToArray();


            ICustomerTypeManagementService customertypeManagementService = ServiceFactory.GetCustomerTypeManagementService();
            CustomerTypesResponseDto customertypeData = customertypeManagementService.GetAllCustomerTypes(
            SecurityHelper.Context,
            AuditHelper.Context);
            policyApprovalInizializeResponseDto.GetAllCustomerTypes = customertypeData.CustomerTypes.ToArray();


            IUsageTypeManagementService usageTypeManagementService = ServiceFactory.GetUsageTypeManagementService();
            UsageTypesResponseDto usageTypeData = usageTypeManagementService.GetAllUsageTypes(
            SecurityHelper.Context,
            AuditHelper.Context);
            policyApprovalInizializeResponseDto.GetAllUsageTypes = usageTypeData.UsageTypes.ToArray();


            IIdTypeManagementService IdTypeManagementService = ServiceFactory.GetIdTypeManagementService();
            IdTypesResponseDto idTypeData = IdTypeManagementService.GetAllIdTypes(
            SecurityHelper.Context,
            AuditHelper.Context);
            policyApprovalInizializeResponseDto.GetAllIdTypes = idTypeData.IdTypes.ToArray();


            ICustomerManagementService customerManagementService = ServiceFactory.GetCustomerManagementService();
            CustomersResponseDto customerData = customerManagementService.GetCustomers(
            SecurityHelper.Context,
            AuditHelper.Context);
            policyApprovalInizializeResponseDto.GetAllCustomers = customerData.Customers.ToArray();


            IVehicleDetailsManagementService VehicleDetailsManagementService = ServiceFactory.GetVehicleDetailsManagementService();
            VehicleAllDetailsResponseDto VehicleDetailsData = VehicleDetailsManagementService.GetVehicleAllDetails(
            SecurityHelper.Context,
            AuditHelper.Context);
            policyApprovalInizializeResponseDto.GetAllVehicleDetails = VehicleDetailsData.VehicleAllDetails.ToArray();


            IBrownAndWhiteDetailsManagementService BrownAndWhiteDetailsManagementService = ServiceFactory.GetBrownAndWhiteDetailsManagementService();
            BrownAndWhiteAllDetailsResponseDto BrownAndWhiteDetailsData = BrownAndWhiteDetailsManagementService.GetBrownAndWhiteAllDetails(
            SecurityHelper.Context,
            AuditHelper.Context);
            policyApprovalInizializeResponseDto.GetAllBrownAndWhiteDetails = BrownAndWhiteDetailsData.BrownAndWhiteAllDetails.ToArray();


            IItemStatusManagementService ItemStatusManagementService = ServiceFactory.GetItemStatusManagementService();
            ItemStatusesResponseDto ItemStatusData = ItemStatusManagementService.GetItemStatuss(
            SecurityHelper.Context,
            AuditHelper.Context);
            policyApprovalInizializeResponseDto.GetAllItemStatuss = ItemStatusData.ItemStatuss.ToArray();


            var cylinderCountManagementService = ServiceFactory.GetCylinderCountManagementService();
            var cylinderCountData = cylinderCountManagementService.GetCylinderCounts(
                SecurityHelper.Context,
                AuditHelper.Context);
            policyApprovalInizializeResponseDto.GetAllCylinderCounts = cylinderCountData.CylinderCounts.ToArray();


            var driveTypeManagementService = ServiceFactory.GetDriveTypeManagementService();
            var driveTypeData = driveTypeManagementService.GetDriveTypes(
                SecurityHelper.Context,
                AuditHelper.Context);
            policyApprovalInizializeResponseDto.GetAllDriveTypes = driveTypeData.DriveTypes.ToArray();


            var engineCapacityManagementService = ServiceFactory.GetEngineCapacityManagementService();
            var engineCapacityData = engineCapacityManagementService.GetEngineCapacities(
                SecurityHelper.Context,
                AuditHelper.Context);
            policyApprovalInizializeResponseDto.GetAllEngineCapacities = engineCapacityData.EngineCapacities.ToArray();


            var fuelTypeManagementService = ServiceFactory.GetFuelTypeManagementService();
            var fuelTypeData = fuelTypeManagementService.GetFuelTypes(
                SecurityHelper.Context,
                AuditHelper.Context);
            policyApprovalInizializeResponseDto.GetAllFuelTypes = fuelTypeData.FuelTypes.ToArray();


            var vehicleBodyTypeManagementService = ServiceFactory.GetVehicleBodyTypeManagementService();
            var vehicleBodyTypeData = vehicleBodyTypeManagementService.GetVehicleBodyTypes(
                SecurityHelper.Context,
                AuditHelper.Context);
            policyApprovalInizializeResponseDto.GetAllVehicleBodyTypes = vehicleBodyTypeData.VehicleBodyTypes.ToArray();


            var TransmissionTypeManagementService = ServiceFactory.GetTransmissionTypeManagementService();
            var TransmissionTypeData = TransmissionTypeManagementService.GetTransmissionTypes(
                SecurityHelper.Context,
                AuditHelper.Context);
            policyApprovalInizializeResponseDto.GetAllTransmissionTypes = TransmissionTypeData.TransmissionTypes.ToArray();


            var VehicleAspirationTypeManagementService = ServiceFactory.GetVehicleAspirationTypeManagementService();
            var VehicleAspirationTypeData = VehicleAspirationTypeManagementService.GetVehicleAspirationTypes(
                SecurityHelper.Context,
                AuditHelper.Context);
            policyApprovalInizializeResponseDto.GetAllVehicleAspirationTypes = VehicleAspirationTypeData.VehicleAspirationTypes.ToArray();


            PolicyApprovalInizializeResponseDto obj = GetDataInSerailProcesss(data);
            policyApprovalInizializeResponseDto.getDocumentTypesByPageName = obj.getDocumentTypesByPageName;
            policyApprovalInizializeResponseDto.GetUsers = obj.GetUsers;

            return policyApprovalInizializeResponseDto;
        }

        [HttpPost]
        public object GetInizializationDataSerialPart01(JObject data)
        {
            string token = Request.Headers.Authorization.ToString();
            SecurityHelper.Context.setToken(token);
            PolicyApprovalInizializeResponseDto policyApprovalInizializeResponseDto = new PolicyApprovalInizializeResponseDto();


            ICommodityUsageTypeManagementService CommodityUsageTypeManagementService = ServiceFactory.GetCommodityUsageTypeManagementService();
            CommodityUsageTypesResponseDto CommodityUsageTypeData = CommodityUsageTypeManagementService.GetCommodityUsageTypes(
            SecurityHelper.Context,
            AuditHelper.Context);
            policyApprovalInizializeResponseDto.GetAllCommodityUsageTypes = CommodityUsageTypeData.CommodityUsageTypes.ToArray();


            IPremiumBasedOnManagementService PremiumBasedOnManagementService = ServiceFactory.GetPremiumBasedOnManagementService();
            PremiumBasedOnesResponseDto PremiumBasedOns = PremiumBasedOnManagementService.GetPremiumBasedOns(
            SecurityHelper.Context,
            AuditHelper.Context);
            policyApprovalInizializeResponseDto.GetPremiumBasedOns = PremiumBasedOns.PremiumBasedOns.ToArray();


            ICurrencyManagementService CurrencyManagementService = ServiceFactory.GetCurrencyManagementService();
            CurrenciesResponseDto CurrencyData = CurrencyManagementService.GetAllCurrencies(
            SecurityHelper.Context,
            AuditHelper.Context);
            policyApprovalInizializeResponseDto.GetCurrencies = CurrencyData.Currencies.ToArray();


            IDealerManagementService DealerManagementService = ServiceFactory.GetDealerManagementService();
            DealersRespondDto DealerData = DealerManagementService.GetAllDealers(
            SecurityHelper.Context,
            AuditHelper.Context);
            policyApprovalInizializeResponseDto.GetAllDealers = DealerData.Dealers.ToArray();


            IPaymentManagementService PaymentManagementService = ServiceFactory.GetPaymentManagementService();
            policyApprovalInizializeResponseDto.GetAllPaymentModes = PaymentManagementService.GetAllPaymentModes(SecurityHelper.Context, AuditHelper.Context);

            ICountryManagementService countryManagementService = ServiceFactory.GetCountryManagementService();
            CountriesResponseDto countryData = countryManagementService.GetAllCountries(SecurityHelper.Context, AuditHelper.Context);
            policyApprovalInizializeResponseDto.GetAllCountries = countryData.Countries;



            return policyApprovalInizializeResponseDto;
        }

        [HttpPost]
        public object GetInizializationDataSerialPart02(JObject data)
        {
            string token = Request.Headers.Authorization.ToString();
            SecurityHelper.Context.setToken(token);
            PolicyApprovalInizializeResponseDto policyApprovalInizializeResponseDto = new PolicyApprovalInizializeResponseDto();



            INationalityManagementService nationalityManagementService = ServiceFactory.GetNationalityManagementService();
            NationalitiesResponseDto nationalityData = nationalityManagementService.GetAllNationalities(
            SecurityHelper.Context,
            AuditHelper.Context);
            policyApprovalInizializeResponseDto.GetAllNationalities = nationalityData.Nationalities.ToArray();


            ICustomerTypeManagementService customertypeManagementService = ServiceFactory.GetCustomerTypeManagementService();
            CustomerTypesResponseDto customertypeData = customertypeManagementService.GetAllCustomerTypes(
            SecurityHelper.Context,
            AuditHelper.Context);
            policyApprovalInizializeResponseDto.GetAllCustomerTypes = customertypeData.CustomerTypes.ToArray();


            IUsageTypeManagementService usageTypeManagementService = ServiceFactory.GetUsageTypeManagementService();
            UsageTypesResponseDto usageTypeData = usageTypeManagementService.GetAllUsageTypes(
            SecurityHelper.Context,
            AuditHelper.Context);
            policyApprovalInizializeResponseDto.GetAllUsageTypes = usageTypeData.UsageTypes.ToArray();


            IIdTypeManagementService IdTypeManagementService = ServiceFactory.GetIdTypeManagementService();
            IdTypesResponseDto idTypeData = IdTypeManagementService.GetAllIdTypes(
            SecurityHelper.Context,
            AuditHelper.Context);
            policyApprovalInizializeResponseDto.GetAllIdTypes = idTypeData.IdTypes.ToArray();


            ICustomerManagementService customerManagementService = ServiceFactory.GetCustomerManagementService();
            CustomersResponseDto customerData = customerManagementService.GetCustomers(
            SecurityHelper.Context,
            AuditHelper.Context);
            policyApprovalInizializeResponseDto.GetAllCustomers = customerData.Customers.ToArray();

            IVehicleDetailsManagementService VehicleDetailsManagementService = ServiceFactory.GetVehicleDetailsManagementService();
            VehicleAllDetailsResponseDto VehicleDetailsData = VehicleDetailsManagementService.GetVehicleAllDetails(
            SecurityHelper.Context,
            AuditHelper.Context);
            policyApprovalInizializeResponseDto.GetAllVehicleDetails = VehicleDetailsData.VehicleAllDetails.ToArray();


            return policyApprovalInizializeResponseDto;
        }

        [HttpPost]
        public object GetInizializationDataSerialPart03(JObject data)
        {
            string token = Request.Headers.Authorization.ToString();
            SecurityHelper.Context.setToken(token);
            PolicyApprovalInizializeResponseDto policyApprovalInizializeResponseDto = new PolicyApprovalInizializeResponseDto();


            IBrownAndWhiteDetailsManagementService BrownAndWhiteDetailsManagementService = ServiceFactory.GetBrownAndWhiteDetailsManagementService();
            BrownAndWhiteAllDetailsResponseDto BrownAndWhiteDetailsData = BrownAndWhiteDetailsManagementService.GetBrownAndWhiteAllDetails(
            SecurityHelper.Context,
            AuditHelper.Context);
            policyApprovalInizializeResponseDto.GetAllBrownAndWhiteDetails = BrownAndWhiteDetailsData.BrownAndWhiteAllDetails.ToArray();


            IItemStatusManagementService ItemStatusManagementService = ServiceFactory.GetItemStatusManagementService();
            ItemStatusesResponseDto ItemStatusData = ItemStatusManagementService.GetItemStatuss(
            SecurityHelper.Context,
            AuditHelper.Context);
            policyApprovalInizializeResponseDto.GetAllItemStatuss = ItemStatusData.ItemStatuss.ToArray();


            var cylinderCountManagementService = ServiceFactory.GetCylinderCountManagementService();
            var cylinderCountData = cylinderCountManagementService.GetCylinderCounts(
                SecurityHelper.Context,
                AuditHelper.Context);
            policyApprovalInizializeResponseDto.GetAllCylinderCounts = cylinderCountData.CylinderCounts.ToArray();


            var driveTypeManagementService = ServiceFactory.GetDriveTypeManagementService();
            var driveTypeData = driveTypeManagementService.GetDriveTypes(
                SecurityHelper.Context,
                AuditHelper.Context);
            policyApprovalInizializeResponseDto.GetAllDriveTypes = driveTypeData.DriveTypes.ToArray();


            var engineCapacityManagementService = ServiceFactory.GetEngineCapacityManagementService();
            var engineCapacityData = engineCapacityManagementService.GetEngineCapacities(
                SecurityHelper.Context,
                AuditHelper.Context);
            policyApprovalInizializeResponseDto.GetAllEngineCapacities = engineCapacityData.EngineCapacities.ToArray();


            var fuelTypeManagementService = ServiceFactory.GetFuelTypeManagementService();
            var fuelTypeData = fuelTypeManagementService.GetFuelTypes(
                SecurityHelper.Context,
                AuditHelper.Context);
            policyApprovalInizializeResponseDto.GetAllFuelTypes = fuelTypeData.FuelTypes.ToArray();



            return policyApprovalInizializeResponseDto;
        }


        [HttpPost]
        public object GetInizializationDataSerialPart04(JObject data)
        {
            string token = Request.Headers.Authorization.ToString();
            SecurityHelper.Context.setToken(token);
            PolicyApprovalInizializeResponseDto policyApprovalInizializeResponseDto = new PolicyApprovalInizializeResponseDto();


            var vehicleBodyTypeManagementService = ServiceFactory.GetVehicleBodyTypeManagementService();
            var vehicleBodyTypeData = vehicleBodyTypeManagementService.GetVehicleBodyTypes(
                SecurityHelper.Context,
                AuditHelper.Context);
            policyApprovalInizializeResponseDto.GetAllVehicleBodyTypes = vehicleBodyTypeData.VehicleBodyTypes.ToArray();


            var TransmissionTypeManagementService = ServiceFactory.GetTransmissionTypeManagementService();
            var TransmissionTypeData = TransmissionTypeManagementService.GetTransmissionTypes(
                SecurityHelper.Context,
                AuditHelper.Context);
            policyApprovalInizializeResponseDto.GetAllTransmissionTypes = TransmissionTypeData.TransmissionTypes.ToArray();


            var VehicleAspirationTypeManagementService = ServiceFactory.GetVehicleAspirationTypeManagementService();
            var VehicleAspirationTypeData = VehicleAspirationTypeManagementService.GetVehicleAspirationTypes(
                SecurityHelper.Context,
                AuditHelper.Context);
            policyApprovalInizializeResponseDto.GetAllVehicleAspirationTypes = VehicleAspirationTypeData.VehicleAspirationTypes.ToArray();


            PolicyApprovalInizializeResponseDto obj = GetDataInSerailProcesss(data);
            policyApprovalInizializeResponseDto.getDocumentTypesByPageName = obj.getDocumentTypesByPageName;
            policyApprovalInizializeResponseDto.GetUsers = obj.GetUsers;


            return policyApprovalInizializeResponseDto;
        }



        [HttpPost]
        public object GetCountryDealerProductCommoditiesInsures()
        {
            CountryDealerProductCommoditiesInsuresResponseDto response = new CountryDealerProductCommoditiesInsuresResponseDto();
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                ICountryManagementService countryManagementService = ServiceFactory.GetCountryManagementService();
                CountriesResponseDto countryData = countryManagementService.GetAllActiveCountries(SecurityHelper.Context, AuditHelper.Context);

                IDealerManagementService DealerManagementService = ServiceFactory.GetDealerManagementService();
                DealersRespondDto DealerData = DealerManagementService.GetAllDealers(SecurityHelper.Context, AuditHelper.Context);

                IProductManagementService productManagementService = ServiceFactory.GetProductManagementService();
                ProductsResponseDto productData = productManagementService.GetProducts(SecurityHelper.Context, AuditHelper.Context);

                ICommodityManagementService CommodityManagementService = ServiceFactory.GetCommodityManagementService();
                CommoditiesResponseDto CommoditiesData = CommodityManagementService.GetAllCommodities(SecurityHelper.Context, AuditHelper.Context);

                IInsurerManagementService InsurerManagementService = ServiceFactory.GetInsurerManagementService();
                InsurersResponseDto InsurerData = InsurerManagementService.GetInsurers(SecurityHelper.Context, AuditHelper.Context);

                response.countries = countryData.Countries.ToArray();
                response.dealers = DealerData.Dealers.ToArray();
                response.products = productData.Products.ToArray();
                response.commodityTypes = CommoditiesData.Commmodities.ToArray();
                response.insurers = InsurerData.Insurers.ToArray();

                return response;

            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return response;
            }

        }


        [HttpPost]
        public object GetAllItemStatussCylinderCountEngineCapacityVehicleWeightBasedOns()
        {
            ItemStatussCylinderCountEngineCapacityVehicleWeightBasedOnsResponseDto response = new ItemStatussCylinderCountEngineCapacityVehicleWeightBasedOnsResponseDto();
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                IItemStatusManagementService ItemStatusManagementService = ServiceFactory.GetItemStatusManagementService();
                ItemStatusesResponseDto ItemStatusData = ItemStatusManagementService.GetItemStatuss(SecurityHelper.Context, AuditHelper.Context);

                var cylinderCountManagementService = ServiceFactory.GetCylinderCountManagementService();
                var cylinderCountData = cylinderCountManagementService.GetCylinderCounts(SecurityHelper.Context, AuditHelper.Context);

                var engineCapacityManagementService = ServiceFactory.GetEngineCapacityManagementService();
                var engineCapacityData = engineCapacityManagementService.GetEngineCapacities(SecurityHelper.Context, AuditHelper.Context);

                var vehicleWeightManagementService = ServiceFactory.GetVehicleWeightManagementService();
                var VehicleWeightsData = vehicleWeightManagementService.GetAllVehicleWeight(SecurityHelper.Context, AuditHelper.Context);

                IPremiumBasedOnManagementService PremiumBasedOnManagementService = ServiceFactory.GetPremiumBasedOnManagementService();
                PremiumBasedOnesResponseDto PremiumBasedOns = PremiumBasedOnManagementService.GetPremiumBasedOns(SecurityHelper.Context, AuditHelper.Context);

                response.itemStatuses = ItemStatusData.ItemStatuss.ToArray();
                response.clinderCounts = cylinderCountData.CylinderCounts.ToArray();
                response.engineCapacities = engineCapacityData.EngineCapacities.ToArray();
                response.grossVehicleWeights = VehicleWeightsData.VehicleWeights.ToArray();
                response.premiumBasedOnValues = PremiumBasedOns.PremiumBasedOns.ToArray();

                return response;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return response;
            }

        }


        [HttpPost]
        public object GetWarrantyTypesRSAProviderRegionsCommissionTypesCommodityUsageTypesDealTypes()
        {
            WartyTpsRSAProviRegCommiTpsCmodityUsageTpsDealTpsResponseDto response = new WartyTpsRSAProviRegCommiTpsCmodityUsageTpsDealTpsResponseDto();
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                IWarrantyTypeManagementService WarrantyTypeManagementService = ServiceFactory.GetWarrantyTypeManagementService();
                WarrantyTypesResponseDto WarrantyType = WarrantyTypeManagementService.GetWarrantyTypes(SecurityHelper.Context, AuditHelper.Context);

                IRSAProviderManagementService RSAProviderManagementService = ServiceFactory.GetRSAProviderManagementService();
                RSAProvideresResponseDto Commition = RSAProviderManagementService.GetRSAProviders(SecurityHelper.Context, AuditHelper.Context);

                IRegionManagementService RegionManagementService = ServiceFactory.GetRegionManagementService();
                RegionesResponseDto region = RegionManagementService.GetRegions(SecurityHelper.Context, AuditHelper.Context);

                INRPCommissionTypesManagementService NRPCommissionTypesManagementService = ServiceFactory.GetNRPCommissionTypesManagementService();
                NRPCommissionTypessResponseDto CommitionT = NRPCommissionTypesManagementService.GetNRPCommissionTypess(SecurityHelper.Context, AuditHelper.Context);

                ICommodityUsageTypeManagementService CommodityUsageTypeManagementService = ServiceFactory.GetCommodityUsageTypeManagementService();
                CommodityUsageTypesResponseDto CommodityUsageTypeData = CommodityUsageTypeManagementService.GetCommodityUsageTypes(SecurityHelper.Context, AuditHelper.Context);

                IDealTypeManagementService DealTypeManagementService = ServiceFactory.GetDealTypeManagementService();
                DealTypesResponseDto DealTypesData = DealTypeManagementService.GetDealTypes(SecurityHelper.Context, AuditHelper.Context);

                response.warrantyTypes = WarrantyType.WarrantyTypes.ToArray();
                response.rsaProviders = Commition.RSAProviders.ToArray();
                response.regions = region.Regions.ToArray();
                response.commissionTypes = CommitionT.NRPCommissionTypess.ToArray();
                response.commodityUsageTypes = CommodityUsageTypeData.CommodityUsageTypes.ToArray();
                response.dealTypes = DealTypesData.DealTypes.ToArray();

                return response;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return response;
            }

        }

        [HttpPost]
        public object GetCountryNationalitiesCustomerTypesUsageTypesIdTypesCommoditiesItemStatus()
        {
            NationalitiesCustomerTypesUsageTypesIdTypesCommoditiesItemStatusResponseDto response = new NationalitiesCustomerTypesUsageTypesIdTypesCommoditiesItemStatusResponseDto();
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                ICountryManagementService countryManagementService = ServiceFactory.GetCountryManagementService();
                CountriesResponseDto countryData = countryManagementService.GetAllCountries(SecurityHelper.Context, AuditHelper.Context);

                INationalityManagementService nationalityManagementService = ServiceFactory.GetNationalityManagementService();
                NationalitiesResponseDto nationalityData = nationalityManagementService.GetAllNationalities(SecurityHelper.Context, AuditHelper.Context);

                ICustomerTypeManagementService customertypeManagementService = ServiceFactory.GetCustomerTypeManagementService();
                CustomerTypesResponseDto customertypeData = customertypeManagementService.GetAllCustomerTypes(SecurityHelper.Context, AuditHelper.Context);

                IUsageTypeManagementService usageTypeManagementService = ServiceFactory.GetUsageTypeManagementService();
                UsageTypesResponseDto usageTypeData = usageTypeManagementService.GetAllUsageTypes(SecurityHelper.Context, AuditHelper.Context);

                IIdTypeManagementService IdTypeManagementService = ServiceFactory.GetIdTypeManagementService();
                IdTypesResponseDto idTypeData = IdTypeManagementService.GetAllIdTypes(SecurityHelper.Context, AuditHelper.Context);

                ICommodityManagementService CommodityManagementService = ServiceFactory.GetCommodityManagementService();
                CommoditiesResponseDto CommoditiesData = CommodityManagementService.GetAllCommodities(SecurityHelper.Context, AuditHelper.Context);

                IItemStatusManagementService ItemStatusManagementService = ServiceFactory.GetItemStatusManagementService();
                ItemStatusesResponseDto ItemStatusData = ItemStatusManagementService.GetItemStatuss(SecurityHelper.Context, AuditHelper.Context);


                response.countries = countryData.Countries;
                response.nationalities = nationalityData.Nationalities.ToArray();
                response.customerTypes = customertypeData.CustomerTypes.ToArray();
                response.usageTypes = usageTypeData.UsageTypes.ToArray();
                response.idTypes = idTypeData.IdTypes.ToArray();
                response.commodityTypes = CommoditiesData.Commmodities.ToList();
                response.ItemStatuss = ItemStatusData.ItemStatuss.ToArray();

                return response;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return response;
            }
        }


        [HttpPost]
        public object GetAutomibileAttribRelatedData()
        {
            AutomobileAttributesResponseDto response = new AutomobileAttributesResponseDto();
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                var driveTypeManagementService = ServiceFactory.GetDriveTypeManagementService();
                var driveTypeData = driveTypeManagementService.GetDriveTypes(SecurityHelper.Context, AuditHelper.Context);

                ICommodityUsageTypeManagementService CommodityUsageTypeManagementService = ServiceFactory.GetCommodityUsageTypeManagementService();
                CommodityUsageTypesResponseDto CommodityUsageTypeData = CommodityUsageTypeManagementService.GetCommodityUsageTypes(SecurityHelper.Context, AuditHelper.Context);

                var engineCapacityManagementService = ServiceFactory.GetEngineCapacityManagementService();
                var engineCapacityData = engineCapacityManagementService.GetEngineCapacities(SecurityHelper.Context, AuditHelper.Context);

                var cylinderCountManagementService = ServiceFactory.GetCylinderCountManagementService();
                var cylinderCountData = cylinderCountManagementService.GetCylinderCounts(SecurityHelper.Context, AuditHelper.Context);

                var fuelTypeManagementService = ServiceFactory.GetFuelTypeManagementService();
                var fuelTypeData = fuelTypeManagementService.GetFuelTypes(SecurityHelper.Context, AuditHelper.Context);

                var TransmissionTypeManagementService = ServiceFactory.GetTransmissionTypeManagementService();
                var TransmissionTypeData = TransmissionTypeManagementService.GetTransmissionTypes(SecurityHelper.Context, AuditHelper.Context);

                var vehicleBodyTypeManagementService = ServiceFactory.GetVehicleBodyTypeManagementService();
                var vehicleBodyTypeData = vehicleBodyTypeManagementService.GetVehicleBodyTypes(SecurityHelper.Context, AuditHelper.Context);

                var VehicleAspirationTypeManagementService = ServiceFactory.GetVehicleAspirationTypeManagementService();
                var VehicleAspirationTypeData = VehicleAspirationTypeManagementService.GetVehicleAspirationTypes(SecurityHelper.Context, AuditHelper.Context);

                response.DriveTypes = driveTypeData.DriveTypes.ToArray();
                response.commodityUsageTypes = CommodityUsageTypeData.CommodityUsageTypes.ToArray();
                response.engineCapacities = engineCapacityData.EngineCapacities.ToArray();
                response.cylinderCounts = cylinderCountData.CylinderCounts.ToArray();
                response.fuelTypes = fuelTypeData.FuelTypes.ToArray();
                response.transmissionTypes = TransmissionTypeData.TransmissionTypes.ToArray();
                response.bodyTypes = vehicleBodyTypeData.VehicleBodyTypes.ToArray();
                response.aspirationTypes = VehicleAspirationTypeData.VehicleAspirationTypes.ToArray();

                return response;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return response;
            }
        }

        [HttpPost]
        public object GetUsersPaymentModeCurrencies()
        {
            UsersPaymentModeCurrenciesResponseDto response = new UsersPaymentModeCurrenciesResponseDto();

            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                var userManagementService = ServiceFactory.GetUserManagementService();
                var users = userManagementService.GetUsers(SecurityHelper.Context,AuditHelper.Context);

                var Data = users.Users.Select(a => new
                 {
                     a.UserName,
                     a.IsActive,
                     a.Id,
                     a.FirstName,
                     a.LastName,
                     a.DateOfBirth
                 }).ToArray();

                var ab = new
                {
                    draw = 1,
                    recordsTotal = users.Users.Count,
                    recordsFiltered = Data.Length,
                    data = Data
                };

                response.users=ab;

                IPaymentManagementService PaymentManagementService = ServiceFactory.GetPaymentManagementService();
                response.paymentModes = PaymentManagementService.GetAllPaymentModes(SecurityHelper.Context, AuditHelper.Context).PaymetModes;

                ICurrencyManagementService CurrencyManagementService = ServiceFactory.GetCurrencyManagementService();
                CurrenciesResponseDto CurrencyData = CurrencyManagementService.GetAllCurrencies(SecurityHelper.Context,AuditHelper.Context);
                response.currencies= CurrencyData.Currencies.ToArray();


                return response;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return response;
            }
        }
    }
}
