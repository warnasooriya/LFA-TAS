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
using NLog;
using System.Reflection;


namespace TAS.Web.Controllers
{
    public class CustomerController : ApiController
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        // private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Customer
        [HttpPost]
        public string AddCustomer(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                CustomerRequestDto customer = data.ToObject<CustomerRequestDto>();
                ICustomerManagementService customerManagementService = ServiceFactory.GetCustomerManagementService();
                CustomerRequestDto result = customerManagementService.AddCustomer(customer, SecurityHelper.Context, AuditHelper.Context);

                SystemUserRequestDto systemUser = data.ToObject<SystemUserRequestDto>();
                IUserManagementService userManagementService = ServiceFactory.GetUserManagementService();

                systemUser.UserTypeId = userManagementService.GetUserTypes(SecurityHelper.Context, AuditHelper.Context).UserTypes.Find(t => t.Code == "CU").Id;
                systemUser.LoginMapId = Guid.Parse(result.Id);

                ISystemUserManagementService SystemUserManagementService = ServiceFactory.GetSystemUserManagementService();
                SystemUserRequestDto resultS = SystemUserManagementService.AddUser(systemUser, SecurityHelper.Context, AuditHelper.Context);

                logger.Info("User Added");
                if (result.CustomerInsertion)
                {
                    SystemUserManagementService.SendUserRegistrationEmail(customer.Email, systemUser.Password, customer.Email, Guid.NewGuid().ToString());
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
        public string UpdateCustomer(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                CustomerRequestDto Customer = data.ToObject<CustomerRequestDto>();
                ICustomerManagementService CustomerManagementService = ServiceFactory.GetCustomerManagementService();

                CustomerRequestDto result = CustomerManagementService.UpdateCustomer(Customer, SecurityHelper.Context, AuditHelper.Context);

                SystemUserRequestDto systemUser = data.ToObject<SystemUserRequestDto>();
                IUserManagementService userManagementService = ServiceFactory.GetUserManagementService();

                systemUser.UserTypeId = userManagementService.GetUserTypes(SecurityHelper.Context, AuditHelper.Context).UserTypes.Find(t => t.Code == "CU").Id;
                systemUser.LoginMapId = Guid.Parse(result.Id);

                ISystemUserManagementService SystemUserManagementService = ServiceFactory.GetSystemUserManagementService();
                SystemUserRequestDto resultS = SystemUserManagementService.AddUser(systemUser, SecurityHelper.Context, AuditHelper.Context);

                logger.Info("CylinderCount Added");
                if (result.CustomerInsertion)
                {
                    return "OK";
                }
                else
                {
                    return "Add cylinderCount failed!";
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return "Add cylinderCount failed!";
            }

        }

        [HttpPost]
        public string UpdateCustomerProfile(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                CustomerRequestDto Customer = data.ToObject<CustomerRequestDto>();
                ICustomerManagementService CustomerManagementService = ServiceFactory.GetCustomerManagementService();

                if (Customer.UserRoleMappings != null && Customer.UserRoleMappings.Count == 0)
                {
                    CustomerResponseDto cus = CustomerManagementService.GetCustomerByUserName(Customer.UserName,
                    SecurityHelper.Context,
                    AuditHelper.Context);
                    Customer.UserRoleMappings = cus.UserRoleMappings;
                }

                CustomerRequestDto result = CustomerManagementService.UpdateCustomer(Customer, SecurityHelper.Context, AuditHelper.Context);

                SystemUserRequestDto systemUser = data.ToObject<SystemUserRequestDto>();
                IUserManagementService userManagementService = ServiceFactory.GetUserManagementService();

                systemUser.UserTypeId = userManagementService.GetUserTypes(SecurityHelper.Context, AuditHelper.Context).UserTypes.Find(t => t.Code == "CU").Id;
                systemUser.LoginMapId = Guid.Parse(result.Id);

                ISystemUserManagementService SystemUserManagementService = ServiceFactory.GetSystemUserManagementService();
                SystemUserRequestDto resultS = SystemUserManagementService.AddUser(systemUser, SecurityHelper.Context, AuditHelper.Context);

                logger.Info("CylinderCount Added");
                if (result.CustomerInsertion)
                {
                    return "OK";
                }
                else
                {
                    return "Add cylinderCount failed!";
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return "Add cylinderCount failed!";
            }

        }

        [HttpPost]
        public object GetCustomerByUserName(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            ICustomerManagementService customerManagementService = ServiceFactory.GetCustomerManagementService();

            CustomerResponseDto customer = customerManagementService.GetCustomerById(Guid.Parse(data["username"].ToString()),
                SecurityHelper.Context,
                AuditHelper.Context);


            return customer;

        }

        [HttpPost]
        public object GetAllCustomers()
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            ICustomerManagementService customerManagementService = ServiceFactory.GetCustomerManagementService();

            CustomersResponseDto customerData = customerManagementService.GetCustomers(
            SecurityHelper.Context,
            AuditHelper.Context);
            return customerData.Customers.ToArray();
        }

        [HttpPost]
        public object GetCustomerById(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            ICustomerManagementService customerManagementService = ServiceFactory.GetCustomerManagementService();

            CustomerResponseDto customer = customerManagementService.GetCustomerDataById(Guid.Parse(data["Id"].ToString()),
            SecurityHelper.Context,
            AuditHelper.Context);
            //CustomerResponseDto customer = customerData.Customers.FirstOrDefault();//customerData.Customers.Find(c => c.Id == data["Id"].ToString());
            //IPolicyManagementService PolicyManagementService = ServiceFactory.GetPolicyManagementService();
            //List<PolicyResponseDto> policies = PolicyManagementService.GetPolicysByCustomerId(Guid.Parse(data["Id"].ToString()), SecurityHelper.Context, AuditHelper.Context).Policies; //.Policies.FindAll(v => v.CustomerId == Guid.Parse(data["Id"].ToString()));
            //if (policies.Count > 0)
            //{
            //    foreach (var item in policies)
            //    {
            //        IBordxManagementService BordxManagementService = ServiceFactory.GetBordxManagementService();
            //        if (BordxManagementService.GetBordxDetails(SecurityHelper.Context, AuditHelper.Context).BordxDetails.FindAll(b => b.PolicyId == item.Id).Count > 0)
            //        {
            //            customer.status = "Bordx";
            //        }
            //        else
            //        {
            //            customer.status = "Policy";
            //        }
            //    }
            //}
            return customer;
        }

        [HttpPost]
        public object GetCustomerByIdforIloe(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            ICustomerManagementService customerManagementService = ServiceFactory.GetCustomerManagementService();

            CustomerAddPolicyResponseDto customer = customerManagementService.GetCustomerByIdforIloe(Guid.Parse(data["Id"].ToString()),
            SecurityHelper.Context,
            AuditHelper.Context);

            return customer;
        }

        [HttpPost]
        public object GetAllCustomersForSearchGrid(CustomerSearchGridRequestDto CustomerSearchGridRequestDto)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            ICustomerManagementService customerManagementService = ServiceFactory.GetCustomerManagementService();

            return customerManagementService.GetAllCustomersForSearchGrid(
            CustomerSearchGridRequestDto,
            SecurityHelper.Context,
            AuditHelper.Context);
        }

        #endregion

        #region Country
        [HttpPost]
        public object GetAllCountries()
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            ICountryManagementService countryManagementService = ServiceFactory.GetCountryManagementService();

            CountriesResponseDto countryData = countryManagementService.GetAllCountries(SecurityHelper.Context, AuditHelper.Context);
            return countryData.Countries;
            //return countryData.Countries.FindAll(c => c.IsActive).OrderBy(c => c.CountryName).ToArray();
        }
        #endregion

        #region Nationality
        [HttpPost]
        public object GetAllNationalities()
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            INationalityManagementService nationalityManagementService = ServiceFactory.GetNationalityManagementService();

            NationalitiesResponseDto nationalityData = nationalityManagementService.GetAllNationalities(
            SecurityHelper.Context,
            AuditHelper.Context);
            return nationalityData.Nationalities.ToArray();
        }

        [HttpPost]
        public object GetNationalityById(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            INationalityManagementService nationalityManagementService = ServiceFactory.GetNationalityManagementService();

            NationalitiesResponseDto nationalityData = nationalityManagementService.GetAllNationalities(
            SecurityHelper.Context,
            AuditHelper.Context);
            return nationalityData.Nationalities.Find(n => n.Id.ToString() == data["Id"].ToString());
        }
        #endregion

        #region CustomerTypes
        [HttpPost]
        public object GetAllCustomerTypes()
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            ICustomerTypeManagementService customertypeManagementService = ServiceFactory.GetCustomerTypeManagementService();

            CustomerTypesResponseDto customertypeData = customertypeManagementService.GetAllCustomerTypes(
            SecurityHelper.Context,
            AuditHelper.Context);
            return customertypeData.CustomerTypes.ToArray();
        }

        [HttpPost]
        public object GetCustomerTypeById(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            ICustomerTypeManagementService customertypeManagementService = ServiceFactory.GetCustomerTypeManagementService();

            CustomerTypesResponseDto customertypeData = customertypeManagementService.GetAllCustomerTypes(
            SecurityHelper.Context,
            AuditHelper.Context);
            return customertypeData.CustomerTypes.Find(c => c.Id.ToString() == data["Id"].ToString());
        }
        #endregion

        #region Usage Types
        [HttpPost]
        public object GetAllUsageTypes()
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            IUsageTypeManagementService usageTypeManagementService = ServiceFactory.GetUsageTypeManagementService();

            UsageTypesResponseDto usageTypeData = usageTypeManagementService.GetAllUsageTypes(
            SecurityHelper.Context,
            AuditHelper.Context);
            return usageTypeData.UsageTypes.ToArray();
        }

        [HttpPost]
        public object GetUsageTypeById(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            IUsageTypeManagementService usageTypeManagementService = ServiceFactory.GetUsageTypeManagementService();

            UsageTypesResponseDto usageTypeData = usageTypeManagementService.GetAllUsageTypes(
            SecurityHelper.Context,
            AuditHelper.Context);
            return usageTypeData.UsageTypes.Find(u => u.Id.ToString() == data["Id"].ToString());
        }
        #endregion

        #region Id Type
        [HttpPost]
        public object GetAllIdTypes()
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            IIdTypeManagementService IdTypeManagementService = ServiceFactory.GetIdTypeManagementService();

            IdTypesResponseDto idTypeData = IdTypeManagementService.GetAllIdTypes(
            SecurityHelper.Context,
            AuditHelper.Context);
            return idTypeData.IdTypes.ToArray();
        }

        [HttpPost]
        public object GetIdTypeById(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            IIdTypeManagementService IdTypeManagementService = ServiceFactory.GetIdTypeManagementService();

            IdTypesResponseDto idTypeData = IdTypeManagementService.GetAllIdTypes(
            SecurityHelper.Context,
            AuditHelper.Context);
            return idTypeData.IdTypes.Find(i => i.Id.ToString() == data["Id"].ToString());
        }
        #endregion

        #region City
        [HttpPost]
        public object GetAllCities()
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            ICityManagementService cityManagementService = ServiceFactory.GetCityManagementService();

            CitiesResponseDto cityData = cityManagementService.GetAllCities(
            SecurityHelper.Context,
            AuditHelper.Context);
            return cityData.Cities.ToArray();
        }

        [HttpPost]
        public object GetCityById(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            ICityManagementService cityManagementService = ServiceFactory.GetCityManagementService();

            CitiesResponseDto cityData = cityManagementService.GetAllCities(
            SecurityHelper.Context,
            AuditHelper.Context);
            return cityData.Cities.Find(c => c.Id == Guid.Parse(data["Id"].ToString()));
        }

        [HttpPost]
        public object GetAllCitiesByCountry(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            ICityManagementService cityManagementService = ServiceFactory.GetCityManagementService();

            CitiesResponseDto cityData = cityManagementService.GetAllCitiesByCountry(Guid.Parse(data["countryId"].ToString()),
            SecurityHelper.Context,
            AuditHelper.Context);
            return cityData.Cities.ToArray();
        }
        #endregion

        #region Occupation
        [HttpPost]
        public object GetOccupations()
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            ICustomerManagementService customerManagementService = ServiceFactory.GetCustomerManagementService();

            OccupationsResponseDto Occupation = customerManagementService.GetOccupations(
            SecurityHelper.Context,
            AuditHelper.Context);
            return Occupation.Occupations.ToArray();
        }
        #endregion

        #region MaritalStatus
        [HttpPost]
        public object GetMarritalStatuses()
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            ICustomerManagementService customerManagementService = ServiceFactory.GetCustomerManagementService();

            MarritalStatusesResponseDto MaritalStatus = customerManagementService.GetMarritalStatuses(
            SecurityHelper.Context,
            AuditHelper.Context);
            return MaritalStatus.MarritalStatuses.ToArray();
        }
        #endregion

        #region Title
        [HttpPost]
        public object GetTitles()
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            ICustomerManagementService customerManagementService = ServiceFactory.GetCustomerManagementService();

            TitlesResponseDto Titles = customerManagementService.GetTitles(
            SecurityHelper.Context,
            AuditHelper.Context);
            return Titles.Titles.ToArray();
        }
        #endregion

        #region Customer Login
        [HttpPost]
        public object CustomerLoginAuth(JObject data)
        {
            var loginRequest = data.ToObject<CustomerLoginRequestDto>();
            //ip check
            string requestIp = string.Empty, customerName = string.Empty;
            bool isIprestricted = true;

            try
            {
                var customerManagementService = ServiceFactory.GetCustomerManagementService();

                CustomerResponseDto customer = customerManagementService.GetCustomerByUserName(loginRequest.UserName, SecurityHelper.Context, AuditHelper.Context);

                var tpaManagementService = ServiceFactory.GetTASTPAManagementService();
                //tpaName = tpaManagementService.GetTPANameById(
                //    SecurityHelper.Context,
                //    AuditHelper.Context, Guid.Parse(loginRequest.tpaID));

                ITPAManagementService tpaManageServ = ServiceFactory.GetTPAManagementService();
                //requestIp = GetClientIp(Request); //"192.168.8.100";//
                //isIprestricted = tpaManageServ.checkIsIprestrcted(
                //   SecurityHelper.Context,
                //   AuditHelper.Context, Guid.Parse(loginRequest.tpaID), requestIp);
                return true;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return "Error";
            }
        }
        #endregion

        [HttpPost]
        public object ChangePassword(ChangePasswordCustomerRequestDto requestData)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            var customerManagementService = ServiceFactory.GetCustomerManagementService();
            var result = customerManagementService.ChangePassword(requestData, SecurityHelper.Context, AuditHelper.Context);
            return result;
        }

        [HttpPost]
        public object ForgotPasswordCustomer(ForgotPasswordCustomerRequestDto forgotPasswordRequestDto)
        {
            //ForgotPasswordRequestDto forgotPasswordRequestDto = data.ToObject<ForgotPasswordRequestDto>();
            var customerManagementService = ServiceFactory.GetCustomerManagementService();
            var result = customerManagementService.RequestNewPasswordCustomer(forgotPasswordRequestDto, SecurityHelper.Context,
                AuditHelper.Context);
            return result;
        }

        [HttpPost]
        public object ChangeForgotPasssword(ChangePasswordForgotCustomerRequestDto requestData)
        {
            var customerManagementService = ServiceFactory.GetCustomerManagementService();
            var result = customerManagementService.ChangeForgotPasswordCustomer(requestData, SecurityHelper.Context,
                AuditHelper.Context);
            return result;
        }
    }
}
