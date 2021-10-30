using Newtonsoft.Json.Linq;
using NLog;
using System;
using System.Net.Http;
using System.Reflection;
using System.ServiceModel.Channels;
using System.Web;
using System.Web.Http;
using TAS.DataTransfer.Requests;
using TAS.DataTransfer.Responses;
using TAS.Services;
using TAS.Web.Common;

namespace TAS.Web.Controllers
{
    public class BuyingWizardController : ApiController
    {

       // private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public string AddCustomer(JObject data)
        {
            try
            {             

                CustomerRequestDto customer = data.ToObject<CustomerRequestDto>();
                ICustomerManagementService customerManagementService = ServiceFactory.GetCustomerManagementService();
                CustomerRequestDto result = customerManagementService.AddCustomer(customer, SecurityHelper.Context, AuditHelper.Context);
                logger.Info("User Added");
                if (result.CustomerInsertion)
                {
                    return "OK";
                }
                else
                {
                    return "User creation failed!";
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return "User creation failed!";
            }
            
        }

        [HttpPost]
        public object GetCustomerByUserName(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            ICustomerManagementService customerManagementService = ServiceFactory.GetCustomerManagementService();


            CustomerResponseDto customer = customerManagementService.GetCustomerByUserName(data["username"].ToString(),
                SecurityHelper.Context,
                AuditHelper.Context);


            return customer;

        }


        [HttpPost]
        public object GetAllCustomers()
        {
            ICustomerManagementService customerManagementService = ServiceFactory.GetCustomerManagementService();

            CustomersResponseDto customerData = customerManagementService.GetCustomers(
            SecurityHelper.Context,
            AuditHelper.Context);
            return customerData.Customers.ToArray();
        }

        [HttpPost]
        public object GetAllCountries()
        {
            ICountryManagementService countryManagementService = ServiceFactory.GetCountryManagementService();

            CountriesResponseDto countryData = countryManagementService.GetAllCountries(
            SecurityHelper.Context,
            AuditHelper.Context);
            return countryData.Countries.FindAll(c => c.IsActive).ToArray();
        }

        [HttpPost]
        public object GetAllNationalities()
        {
            INationalityManagementService nationalityManagementService = ServiceFactory.GetNationalityManagementService();

            NationalitiesResponseDto nationalityData = nationalityManagementService.GetAllNationalities(
            SecurityHelper.Context,
            AuditHelper.Context);
            return nationalityData.Nationalities.ToArray();
        }

        [HttpPost]
        public object GetAllCustomerTypes()
        {
            ICustomerTypeManagementService customertypeManagementService = ServiceFactory.GetCustomerTypeManagementService();

            CustomerTypesResponseDto customertypeData = customertypeManagementService.GetAllCustomerTypes(
            SecurityHelper.Context,
            AuditHelper.Context);
            return customertypeData.CustomerTypes.ToArray();
        }

        [HttpPost]
        public object GetAllUsageTypes()
        {
            IUsageTypeManagementService usageTypeManagementService = ServiceFactory.GetUsageTypeManagementService();

            UsageTypesResponseDto usageTypeData = usageTypeManagementService.GetAllUsageTypes(
            SecurityHelper.Context,
            AuditHelper.Context);
            return usageTypeData.UsageTypes.ToArray();
        }

        [HttpPost]
        public object GetAllIdTypes()
        {
            IIdTypeManagementService IdTypeManagementService = ServiceFactory.GetIdTypeManagementService();

            IdTypesResponseDto idTypeData = IdTypeManagementService.GetAllIdTypes(
            SecurityHelper.Context,
            AuditHelper.Context);
            return idTypeData.IdTypes.ToArray();
        }

        [HttpPost]
        public object GetAllCities()
        {
            ICityManagementService cityManagementService = ServiceFactory.GetCityManagementService();

            CitiesResponseDto cityData = cityManagementService.GetAllCities(
            SecurityHelper.Context,
            AuditHelper.Context);
            return cityData.Cities.ToArray();
        }

        [HttpPost]
        public object GetAllCitiesByCountry(JObject data)
        {
            ICityManagementService cityManagementService = ServiceFactory.GetCityManagementService();

            CitiesResponseDto cityData = cityManagementService.GetAllCitiesByCountry(Guid.Parse(data["countryId"].ToString()),
            SecurityHelper.Context,
            AuditHelper.Context);
            return cityData.Cities.ToArray();
        }


        [HttpPost]
        public object GetCustomerById(JObject data)
        {
            ICustomerManagementService customerManagementService = ServiceFactory.GetCustomerManagementService();

            CustomersResponseDto customerData = customerManagementService.GetCustomers(
            SecurityHelper.Context,
            AuditHelper.Context);
            return customerData.Customers.Find(c => c.Id == data["Id"].ToString());
        }

        [HttpPost]
        public string SubmitPolicyDetails(JObject data)
        {
            try
            {
                
                PolicyViewData NewPolicy = data.ToObject<PolicyViewData>();

                IVehicleDetailsManagementService VehicleDetailsManagementService = ServiceFactory.GetVehicleDetailsManagementService();
                VehicleDetailsRequestDto VehicleDetails = new VehicleDetailsRequestDto();
                VehicleDetailsRequestDto resultV = new VehicleDetailsRequestDto();

                IBrownAndWhiteDetailsManagementService BrownAndWhiteDetailsManagementService = ServiceFactory.GetBrownAndWhiteDetailsManagementService();
                BrownAndWhiteDetailsRequestDto BrownAndWhiteDetails = new BrownAndWhiteDetailsRequestDto();
                BrownAndWhiteDetailsRequestDto resultB = new BrownAndWhiteDetailsRequestDto();

                ICustomerManagementService CustomerManagementService = ServiceFactory.GetCustomerManagementService();
                CustomerRequestDto Customer = new CustomerRequestDto();
                CustomerRequestDto resultC = new CustomerRequestDto();

                if (NewPolicy.Vehicle.VINNo != "")
                {
                    if (NewPolicy.Vehicle.Id.ToString() == "00000000-0000-0000-0000-000000000000")
                    {
                        VehicleDetails = new VehicleDetailsRequestDto()
                        {
                            BodyTypeId = NewPolicy.Vehicle.BodyTypeId,
                            AspirationId = NewPolicy.Vehicle.AspirationId,
                            CylinderCountId = NewPolicy.Vehicle.CylinderCountId,
                            CategoryId = NewPolicy.Vehicle.CategoryId,
                            EngineCapacityId = NewPolicy.Vehicle.EngineCapacityId,
                            DriveTypeId = NewPolicy.Vehicle.DriveTypeId,
                            MakeId = NewPolicy.Vehicle.MakeId,
                            FuelTypeId = NewPolicy.Vehicle.FuelTypeId,
                            ModelId = NewPolicy.Vehicle.ModelId,
                            ModelYear = NewPolicy.Vehicle.ModelYear,
                            PlateNo = NewPolicy.Vehicle.PlateNo,
                            ItemPurchasedDate = NewPolicy.Vehicle.ItemPurchasedDate,
                            ItemStatusId = NewPolicy.Vehicle.ItemStatusId,
                            VehiclePrice = NewPolicy.Vehicle.VehiclePrice,
                            VINNo = NewPolicy.Vehicle.VINNo,
                            Variant = NewPolicy.Vehicle.Variant,
                            TransmissionId = NewPolicy.Vehicle.TransmissionId
                        };
                        resultV = VehicleDetailsManagementService.AddVehicleDetails(VehicleDetails, SecurityHelper.Context, AuditHelper.Context);
                        if (!resultV.VehicleDetailsInsertion)
                        {
                            return "Add Policy failed!";
                        }
                    }
                    else
                    {
                        VehicleDetails = new VehicleDetailsRequestDto()
                        {
                            Id = NewPolicy.Vehicle.Id,
                            BodyTypeId = NewPolicy.Vehicle.BodyTypeId,
                            AspirationId = NewPolicy.Vehicle.AspirationId,
                            CylinderCountId = NewPolicy.Vehicle.CylinderCountId,
                            CategoryId = NewPolicy.Vehicle.CategoryId,
                            EngineCapacityId = NewPolicy.Vehicle.EngineCapacityId,
                            DriveTypeId = NewPolicy.Vehicle.DriveTypeId,
                            MakeId = NewPolicy.Vehicle.MakeId,
                            FuelTypeId = NewPolicy.Vehicle.FuelTypeId,
                            ModelId = NewPolicy.Vehicle.ModelId,
                            ModelYear = NewPolicy.Vehicle.ModelYear,
                            PlateNo = NewPolicy.Vehicle.PlateNo,
                            ItemPurchasedDate = NewPolicy.Vehicle.ItemPurchasedDate,
                            ItemStatusId = NewPolicy.Vehicle.ItemStatusId,
                            VehiclePrice = NewPolicy.Vehicle.VehiclePrice,
                            VINNo = NewPolicy.Vehicle.VINNo,
                            Variant = NewPolicy.Vehicle.Variant,
                            TransmissionId = NewPolicy.Vehicle.TransmissionId,
                            EntryDateTime = NewPolicy.Vehicle.EntryDateTime,
                            EntryUser = NewPolicy.Vehicle.EntryUser
                        };
                        resultV = VehicleDetailsManagementService.UpdateVehicleDetails(VehicleDetails, SecurityHelper.Context, AuditHelper.Context);
                        if (!resultV.VehicleDetailsInsertion)
                        {
                            return "Add Policy failed!";
                        }
                    }
                }

                if (NewPolicy.BAndW.SerialNo != "")
                {
                    if (NewPolicy.BAndW.Id.ToString() == "00000000-0000-0000-0000-000000000000")
                    {
                        BrownAndWhiteDetails = new BrownAndWhiteDetailsRequestDto()
                        {
                            AddnSerialNo = NewPolicy.BAndW.AddnSerialNo,
                            CategoryId = NewPolicy.BAndW.CategoryId,
                            DealerPrice = NewPolicy.BAndW.DealerPrice,
                            InvoiceNo = NewPolicy.BAndW.InvoiceNo,
                            ItemPrice = NewPolicy.BAndW.ItemPrice,
                            ItemPurchasedDate = NewPolicy.BAndW.ItemPurchasedDate,
                            ItemStatusId = NewPolicy.BAndW.ItemStatusId,
                            MakeId = NewPolicy.BAndW.MakeId,
                            ModelCode = NewPolicy.BAndW.ModelCode,
                            ModelId = NewPolicy.BAndW.ModelId,
                            ModelYear = NewPolicy.BAndW.ModelYear,
                            SerialNo = NewPolicy.BAndW.SerialNo
                        };
                        resultB = BrownAndWhiteDetailsManagementService.AddBrownAndWhiteDetails(BrownAndWhiteDetails, SecurityHelper.Context, AuditHelper.Context);
                        if (!resultB.BrownAndWhiteDetailsInsertion)
                        {
                            return "Add Policy failed!";
                        }
                    }
                    else
                    {
                        BrownAndWhiteDetails = new BrownAndWhiteDetailsRequestDto()
                        {
                            Id = NewPolicy.BAndW.Id,
                            AddnSerialNo = NewPolicy.BAndW.AddnSerialNo,
                            CategoryId = NewPolicy.BAndW.CategoryId,
                            DealerPrice = NewPolicy.BAndW.DealerPrice,
                            InvoiceNo = NewPolicy.BAndW.InvoiceNo,
                            ItemPrice = NewPolicy.BAndW.ItemPrice,
                            ItemPurchasedDate = NewPolicy.BAndW.ItemPurchasedDate,
                            ItemStatusId = NewPolicy.BAndW.ItemStatusId,
                            MakeId = NewPolicy.BAndW.MakeId,
                            ModelCode = NewPolicy.BAndW.ModelCode,
                            ModelId = NewPolicy.BAndW.ModelId,
                            ModelYear = NewPolicy.BAndW.ModelYear,
                            SerialNo = NewPolicy.BAndW.SerialNo
                        };
                        resultB = BrownAndWhiteDetailsManagementService.UpdateBrownAndWhiteDetails(BrownAndWhiteDetails, SecurityHelper.Context, AuditHelper.Context);
                        if (!resultB.BrownAndWhiteDetailsInsertion)
                        {
                            return "Add Policy failed!";
                        }
                    }
                }
                if (NewPolicy.Customer.FirstName != "")
                {
                    if (NewPolicy.Customer.Id.ToString() == "00000000-0000-0000-0000-000000000000")
                    {
                        Customer = new CustomerRequestDto()
                        {
                            Address1 = NewPolicy.Customer.Address1,
                            Address2 = NewPolicy.Customer.Address2,
                            Address3 = NewPolicy.Customer.Address3,
                            Address4 = NewPolicy.Customer.Address4,
                            BusinessAddress1 = NewPolicy.Customer.BusinessAddress1,
                            BusinessAddress2 = NewPolicy.Customer.BusinessAddress2,
                            BusinessAddress3 = NewPolicy.Customer.BusinessAddress3,
                            BusinessAddress4 = NewPolicy.Customer.BusinessAddress4,
                            BusinessName = NewPolicy.Customer.BusinessName,
                            BusinessTelNo = NewPolicy.Customer.BusinessTelNo,
                            CityId = NewPolicy.Customer.CityId,
                            CountryId = NewPolicy.Customer.CountryId,
                            CustomerTypeId = NewPolicy.Customer.CustomerTypeId,
                            DateOfBirth = NewPolicy.Customer.DateOfBirth,
                            DLIssueDate = NewPolicy.Customer.DLIssueDate,
                            Email = NewPolicy.Customer.Email,
                            EntryDateTime = NewPolicy.Customer.EntryDateTime,
                            EntryUserId = NewPolicy.Customer.EntryUserId,
                            FirstName = NewPolicy.Customer.FirstName,
                            Gender = NewPolicy.Customer.Gender,
                            Id = NewPolicy.Customer.Id,
                            IDNo = NewPolicy.Customer.IDNo,
                            IDTypeId = NewPolicy.Customer.IDTypeId,
                            IsActive = NewPolicy.Customer.IsActive,
                            LastModifiedDateTime = NewPolicy.Customer.LastModifiedDateTime,
                            LastName = NewPolicy.Customer.LastName,
                            MobileNo = NewPolicy.Customer.MobileNo,
                            NationalityId = NewPolicy.Customer.NationalityId,
                            OtherTelNo = NewPolicy.Customer.OtherTelNo,
                            Password = NewPolicy.Customer.Password,
                            UsageTypeId = NewPolicy.Customer.UsageTypeId,
                            UserName = NewPolicy.Customer.UserName
                        };
                        resultC = CustomerManagementService.AddCustomer(Customer, SecurityHelper.Context, AuditHelper.Context);
                        if (!resultC.CustomerInsertion)
                        {
                            return "Add Policy failed!";
                        }
                    }
                    else
                    {
                        Customer = new CustomerRequestDto()
                        {
                            Address1 = NewPolicy.Customer.Address1,
                            Address2 = NewPolicy.Customer.Address2,
                            Address3 = NewPolicy.Customer.Address3,
                            Address4 = NewPolicy.Customer.Address4,
                            BusinessAddress1 = NewPolicy.Customer.BusinessAddress1,
                            BusinessAddress2 = NewPolicy.Customer.BusinessAddress2,
                            BusinessAddress3 = NewPolicy.Customer.BusinessAddress3,
                            BusinessAddress4 = NewPolicy.Customer.BusinessAddress4,
                            BusinessName = NewPolicy.Customer.BusinessName,
                            BusinessTelNo = NewPolicy.Customer.BusinessTelNo,
                            CityId = NewPolicy.Customer.CityId,
                            CountryId = NewPolicy.Customer.CountryId,
                            CustomerTypeId = NewPolicy.Customer.CustomerTypeId,
                            DateOfBirth = NewPolicy.Customer.DateOfBirth,
                            DLIssueDate = NewPolicy.Customer.DLIssueDate,
                            Email = NewPolicy.Customer.Email,
                            EntryDateTime = NewPolicy.Customer.EntryDateTime,
                            EntryUserId = NewPolicy.Customer.EntryUserId,
                            FirstName = NewPolicy.Customer.FirstName,
                            Gender = NewPolicy.Customer.Gender,
                            Id = NewPolicy.Customer.Id,
                            IDNo = NewPolicy.Customer.IDNo,
                            IDTypeId = NewPolicy.Customer.IDTypeId,
                            IsActive = NewPolicy.Customer.IsActive,
                            LastModifiedDateTime = NewPolicy.Customer.LastModifiedDateTime,
                            LastName = NewPolicy.Customer.LastName,
                            MobileNo = NewPolicy.Customer.MobileNo,
                            NationalityId = NewPolicy.Customer.NationalityId,
                            OtherTelNo = NewPolicy.Customer.OtherTelNo,
                            Password = NewPolicy.Customer.Password,
                            UsageTypeId = NewPolicy.Customer.UsageTypeId,
                            UserName = NewPolicy.Customer.UserName
                        };
                        resultC = CustomerManagementService.UpdateCustomer(Customer, SecurityHelper.Context, AuditHelper.Context);
                        if (!resultC.CustomerInsertion)
                        {
                            return "Add Policy failed!";
                        }
                    }
                }
                PolicyRequestDto Policy = new PolicyRequestDto()
                {
                    Id = NewPolicy.Id,
                    CommodityTypeId = NewPolicy.CommodityTypeId,
                    ProductId = NewPolicy.ProductId,
                    DealerId = NewPolicy.DealerId,
                    DealerLocationId = NewPolicy.DealerLocationId,
                    ContractId = NewPolicy.ContractId,
                    ExtensionTypeId = NewPolicy.ExtensionTypeId,
                    Premium = NewPolicy.Premium,
                    PremiumCurrencyTypeId = NewPolicy.PremiumCurrencyTypeId,
                    DealerPaymentCurrencyTypeId = NewPolicy.DealerPaymentCurrencyTypeId,
                    CustomerPaymentCurrencyTypeId = NewPolicy.CustomerPaymentCurrencyTypeId,
                    CoverTypeId = NewPolicy.CoverTypeId,
                    HrsUsedAtPolicySale = NewPolicy.HrsUsedAtPolicySale,
                    IsPreWarrantyCheck = NewPolicy.IsPreWarrantyCheck,
                    PolicySoldDate = NewPolicy.PolicySoldDate,
                    SalesPersonId = NewPolicy.SalesPersonId,
                    PolicyNo = NewPolicy.PolicyNo,
                    IsSpecialDeal = NewPolicy.IsSpecialDeal,
                    IsPartialPayment = NewPolicy.IsPartialPayment,
                    DealerPayment = NewPolicy.DealerPayment,
                    CustomerPayment = NewPolicy.CustomerPayment,
                    PaymentModeId = NewPolicy.PaymentModeId,
                    RefNo = NewPolicy.RefNo,
                    Comment = NewPolicy.Comment,
                    ItemId = (NewPolicy.Type == "Vehicle") ? resultV.Id : resultB.Id,
                    Type = NewPolicy.Type,
                    CustomerId = Guid.Parse(resultC.Id),
                    EntryDateTime = NewPolicy.EntryDateTime,
                    EntryUser = NewPolicy.EntryUser
                };

                IPolicyManagementService PolicyManagementService = ServiceFactory.GetPolicyManagementService();
                PolicyRequestDto result = PolicyManagementService.AddPolicy(Policy, SecurityHelper.Context, AuditHelper.Context);
                logger.Info("Policy Added");
                if (result.PolicyInsertion)
                {
                    return "OK";
                }
                else
                {
                    return "Add Policy failed!";
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return "Add Policy failed!";
            }

            
        }

        public class PolicyViewData
        {
            public Guid Id { get; set; }
            public Guid CommodityTypeId { get; set; }
            public Guid ProductId { get; set; }
            public Guid DealerId { get; set; }
            public Guid DealerLocationId { get; set; }
            public Guid ContractId { get; set; }
            public Guid ExtensionTypeId { get; set; }
            public Decimal Premium { get; set; }
            public Guid PremiumCurrencyTypeId { get; set; }
            public Guid DealerPaymentCurrencyTypeId { get; set; }
            public Guid CustomerPaymentCurrencyTypeId { get; set; }
            public Guid CoverTypeId { get; set; }
            public string HrsUsedAtPolicySale { get; set; }
            public bool IsPreWarrantyCheck { get; set; }
            public DateTime PolicySoldDate { get; set; }
            public Guid SalesPersonId { get; set; }
            public string PolicyNo { get; set; }
            public bool IsSpecialDeal { get; set; }
            public bool IsPartialPayment { get; set; }
            public Decimal DealerPayment { get; set; }
            public Decimal CustomerPayment { get; set; }
            public Guid PaymentModeId { get; set; }
            public string RefNo { get; set; }
            public string Comment { get; set; }
            public Guid ItemId { get; set; }
            public string Type { get; set; }
            public Guid CustomerId { get; set; }
            public VehicleDetailsResponseDto Vehicle { get; set; }
            public CustomerResponseDto Customer { get; set; }
            public BrownAndWhiteDetailsResponseDto BAndW { get; set; }
            public DateTime EntryDateTime { get; set; }
            public Guid EntryUser { get; set; }
        }

        #region Customer Login
        [HttpPost]
        public object GetCustomerIdByName(JObject data)
        {
            ICustomerManagementService CustomerManagementService = ServiceFactory.GetCustomerManagementService();
            CustomersResponseDto CustomerData = CustomerManagementService.GetCustomerIdByUserName(
            SecurityHelper.Context,
            AuditHelper.Context,
            data["customerUserName"].ToString(), data["tpaName"].ToString());
            return CustomerData.Customers.ToArray();
        }

        [HttpPost]
        public object CustomerLoginAuth(JObject data)
        {
            var loginRequest = data.ToObject<CustomerLoginRequestDto>();
            //ip check
            string requestIp = string.Empty, customerName = string.Empty;
            bool isIprestricted = true;
            var customerManagementService = ServiceFactory.GetCustomerManagementService();
                CustomersResponseDto CustomerData = customerManagementService.GetCustomerIdByUserName(SecurityHelper.Context, AuditHelper.Context, loginRequest.UserName, loginRequest.tpaName);

                if (CustomerData.Customers.Count == 0)
                {
                    return "Invalid";
                }
                else
                {
                    try
                    {

                        ITPAManagementService tpaManageServ = ServiceFactory.GetTPAManagementService();
                        requestIp = GetClientIp(Request); //"192.168.8.100";//
                        isIprestricted = tpaManageServ.checkIsIprestrcted(
                           SecurityHelper.Context,
                           AuditHelper.Context, Guid.Parse(loginRequest.tpaID), requestIp);
                    }
                    catch (Exception ex)
                    {
                        logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                        return "Error";
                    }

                    logger.Trace("Login request recived from ip-" + requestIp +
                        " for user - " + loginRequest.UserName +
                        " for TPA - " + customerName);
                    //end ip check

                    if (isIprestricted)
                    {

                        var buyingWizardManagementService = ServiceFactory.GetBuyingWizardManagementService();
                        var result = buyingWizardManagementService.AuthUser(loginRequest, SecurityHelper.Context, AuditHelper.Context);
                        if (result.JsonWebToken == null)
                        {
                            logger.Trace("Login failed for ip-" + requestIp +
                                         " for user - " + loginRequest.UserName +
                                         " for TPA - " + customerName);

                            if (!result.IsValid)
                            {
                                return "Invalid";
                            }
                            return "Error";
                        }
                        logger.Trace("Login success for ip-" + requestIp +
                                      " for user - " + loginRequest.UserName +
                                      " for TPA - " + customerName +
                                      " token - " + result.JsonWebToken);

                        return result;
                    }
                    else
                    {
                        return "RestrictedIP";
                    }
                }
        }

        private string GetClientIp(HttpRequestMessage request)
        {
            if (request.Properties.ContainsKey("MS_HttpContext"))
            {
                return ((HttpContextWrapper)request.Properties["MS_HttpContext"]).Request.UserHostAddress;
            }

            if (request.Properties.ContainsKey(RemoteEndpointMessageProperty.Name))
            {
                RemoteEndpointMessageProperty prop;
                prop = (RemoteEndpointMessageProperty)request.Properties[RemoteEndpointMessageProperty.Name];
                return prop.Address;
            }

            return null;
        }
        #endregion
    }
}
