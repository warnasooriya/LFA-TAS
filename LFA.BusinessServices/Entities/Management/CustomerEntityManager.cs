using TAS.Services.Entities.Persistence;
using NHibernate;
using TAS.Services.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using System.Text;
using System.Threading.Tasks;
using TAS.DataTransfer.Requests;
using TAS.DataTransfer.Responses;

using NHibernate.Criterion;
using TAS.Services.Common;
using System.Security.Cryptography;
using System.Linq.Expressions;
using System.Web.Script.Serialization;
using NLog;
using System.Reflection;

namespace TAS.Services.Entities.Management
{
    public class CustomerEntityManager
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        #region Customer

        public List<CustomerResponseDto> GetCustomers()
        {
            ISession session = EntitySessionManager.GetSession();
            return session.Query<Customer>().Select(Customer => new CustomerResponseDto
            {
                FirstName = Customer.FirstName,
                LastName = Customer.LastName,
                UserName = Customer.UserName,
                Password = Customer.Password,
                NationalityId = Customer.NationalityId,
                CountryId = Customer.CountryId,
                DateOfBirth = Customer.DateOfBirth,
                MobileNo = Customer.MobileNo,
                OtherTelNo = Customer.OtherTelNo,
                CustomerTypeId = Customer.CustomerTypeId,
                UsageTypeId = Customer.UsageTypeId,
                Gender = Customer.Gender,
                Address1 = Customer.Address1,
                Address2 = Customer.Address2,
                Address3 = Customer.Address3,
                Address4 = Customer.Address4,
                IDNo = Customer.IDNo,
                IDTypeId = Customer.IDTypeId,
                CityId = Customer.CityId,
                DLIssueDate = Customer.DLIssueDate,
                Email = Customer.UserName,
                IsActive = Customer.IsActive,
                BusinessName = Customer.BusinessName,
                BusinessAddress1 = Customer.BusinessAddress1,
                BusinessAddress2 = Customer.BusinessAddress2,
                BusinessAddress3 = Customer.BusinessAddress3,
                BusinessAddress4 = Customer.BusinessAddress4,
                BusinessTelNo = Customer.BusinessTelNo,
                EntryDateTime = Customer.EntryDateTime,
                EntryUserId = Customer.EntryUserId,
                OccupationId = checkIsEmpty(Customer.OccupationId),
                TitleId = checkIsEmpty(Customer.TitleId),
                MaritalStatusId = checkIsEmpty(Customer.MaritalStatusId),
                PostalCode = Customer.PostalCode,
                Id = Customer.Id.ToString().ToLower()
            }).ToList();

        }

        internal Guid checkIsEmpty(Guid id) {
            Guid gid = Guid.Empty;
            if (!Guid.TryParse(id.ToString(), out gid))
            {
                gid = Guid.Empty;
                return gid;
            }
            else {
                return id;
            }
        }
        public CustomerResponseDto GetCustomerDataById(Guid customerid)
        {
            CustomerResponseDto result = null;

            try
            {
                ISession session = EntitySessionManager.GetSession();
                CommonEntityManager cem = new CommonEntityManager();
                Customer customerData = session.Query<Customer>().FirstOrDefault(a => a.Id == customerid);


                if (customerData != null)
                {


                    result = new CustomerResponseDto();
                    result.FirstName = customerData.FirstName;
                    result.LastName = customerData.LastName;
                    result.UserName = customerData.UserName;
                    result.Password = customerData.Password;
                    result.NationalityId = customerData.NationalityId;
                    result.CountryId = customerData.CountryId;
                    result.DateOfBirth = customerData.DateOfBirth;
                    result.MobileNo = customerData.MobileNo;
                    result.OtherTelNo = customerData.OtherTelNo;
                    result.CustomerTypeId = customerData.CustomerTypeId;
                    result.UsageTypeId = customerData.UsageTypeId;
                    result.Gender = customerData.Gender;
                    result.Address1 = customerData.Address1;
                    result.Address2 = customerData.Address2;
                    result.Address3 = customerData.Address3;
                    result.Address4 = customerData.Address4;
                    result.IDNo = customerData.IDNo;
                    result.IDTypeId = customerData.IDTypeId;
                    result.CityId = customerData.CityId;
                    result.DLIssueDate = customerData.DLIssueDate;
                    result.Email = customerData.UserName;
                    result.IsActive = customerData.IsActive;
                    result.BusinessName = customerData.BusinessName;
                    result.BusinessAddress1 = customerData.BusinessAddress1;
                    result.BusinessAddress2 = customerData.BusinessAddress2;
                    result.BusinessAddress3 = customerData.BusinessAddress3;
                    result.BusinessAddress4 = customerData.BusinessAddress4;
                    result.BusinessTelNo = customerData.BusinessTelNo;
                    result.EntryDateTime = customerData.EntryDateTime;
                    result.EntryUserId = customerData.EntryUserId;
                    result.OccupationId = customerData.OccupationId;
                    result.TitleId = customerData.TitleId;
                    result.MaritalStatusId = customerData.MaritalStatusId;
                    result.PostalCode = customerData.PostalCode;
                    result.Nationality = cem.GetNationaltyNameById(customerData.NationalityId);
                    result.CustomerType = cem.GetCustomerTypeNameById(customerData.CustomerTypeId);
                    result.Occupation = cem.GetOccupationById(customerData.OccupationId);
                    result.Title = cem.getTitleById(customerData.TitleId);
                    result.TitleId = customerData.TitleId;
                    result.OccupationId = customerData.OccupationId;
                    result.Country = cem.GetCountryNameById(customerData.CountryId);
                    result.Id = customerData.Id.ToString();

                    result.status = "Policy";

                    var query =
                      from customer in session.Query<Customer>()
                      join policy in session.Query<Policy>() on customer.Id equals policy.CustomerId
                      join bordix in session.Query<BordxDetails>() on policy.Id equals bordix.PolicyId
                      where customer.Id == customerid
                      select new { bordixid = bordix.Id };

                    if (query.Count() > 0)
                    {
                        result.status = "Bordx";
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }


            return result;
        }

        internal CustomerAddPolicyResponseDto GetCustomerDataForIloeById(Guid customerid)
        {
            CustomerAddPolicyResponseDto result = null;

            try
            {
                ISession session = EntitySessionManager.GetSession();
                CommonEntityManager cem = new CommonEntityManager();
                Customer customerData = session.Query<Customer>().FirstOrDefault(a => a.Id == customerid);

                var query = from customer in session.Query<Customer>()
                            join policy in session.Query<Policy>() on customer.Id equals policy.CustomerId
                            where customer.Id == customerid
                            select new  { customer = customer, policy = policy };

                var result3 = query.OrderByDescending(a => a.policy.PolicyNo);

                var result2 = result3.First();

                if (customerData != null)
                {


                    result = new CustomerAddPolicyResponseDto();
                    result.FirstName = result2.customer.FirstName;
                    result.LastName = result2.customer.LastName;
                    result.UserName = result2.customer.UserName;
                    result.Password = result2.customer.Password;
                    result.NationalityId = result2.customer.NationalityId;
                    result.CountryId = result2.customer.CountryId;
                    result.DateOfBirth = result2.customer.DateOfBirth;
                    result.MobileNo = result2.customer.MobileNo;
                    result.OtherTelNo = result2.customer.OtherTelNo;
                    result.CustomerTypeId = result2.customer.CustomerTypeId;
                    result.UsageTypeId = result2.customer.UsageTypeId;
                    result.Gender = result2.customer.Gender;
                    result.Address1 = result2.customer.Address1;
                    result.Address2 = result2.customer.Address2;
                    result.Address3 = result2.customer.Address3;
                    result.Address4 = result2.customer.Address4;
                    result.IDNo = result2.customer.IDNo;
                    result.IDTypeId = result2.customer.IDTypeId;
                    result.CityId = result2.customer.CityId;
                    result.DLIssueDate = result2.customer.DLIssueDate;
                    result.Email = result2.customer.UserName;
                    result.IsActive = result2.customer.IsActive;
                    result.BusinessName = result2.customer.BusinessName;
                    result.BusinessAddress1 = result2.customer.BusinessAddress1;
                    result.BusinessAddress2 = result2.customer.BusinessAddress2;
                    result.BusinessAddress3 = result2.customer.BusinessAddress3;
                    result.BusinessAddress4 = result2.customer.BusinessAddress4;
                    result.BusinessTelNo = result2.customer.BusinessTelNo;
                    result.EntryDateTime = result2.customer.EntryDateTime;
                    result.EntryUserId = result2.customer.EntryUserId;
                    result.OccupationId = result2.customer.OccupationId;
                    result.TitleId = result2.customer.TitleId;
                    result.MaritalStatusId = result2.customer.MaritalStatusId;
                    result.PostalCode = result2.customer.PostalCode;
                    result.Nationality = cem.GetNationaltyNameById(result2.customer.NationalityId);
                    result.CustomerType = cem.GetCustomerTypeNameById(result2.customer.CustomerTypeId);
                    result.Occupation = cem.GetOccupationById(result2.customer.OccupationId);
                    result.Title = cem.getTitleById(result2.customer.TitleId);
                    result.TitleId = result2.customer.TitleId;
                    result.OccupationId = result2.customer.OccupationId;
                    result.Country = cem.GetCountryNameById(result2.customer.CountryId);
                    result.Id = result2.customer.Id.ToString();
                    result.PolicyId = result2.policy.PolicyBundleId.ToString();

                    result.status = "Policy";

                    var query2 =
                      from customer in session.Query<Customer>()
                      join policy in session.Query<Policy>() on customer.Id equals policy.CustomerId
                      join bordix in session.Query<BordxDetails>() on policy.Id equals bordix.PolicyId
                      where customer.Id == customerid
                      select new { bordixid = bordix.Id };

                    if (query2.Count() > 0)
                    {
                        result.status = "Bordx";
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }


            return result;
        }

        public CustomerResponseDto GetCustomerByUserName(string username)
        {
            ISession session = EntitySessionManager.GetSession();
            CustomerResponseDto cDto = new CustomerResponseDto();

            if (session.Query<Customer>().Where(a => a.UserName == username).FirstOrDefault() != null)
            {
                var aaa = session.Query<Customer>().Max(a => a.EntryDateTime).Value;
                var query =
                   from customer in session.Query<Customer>()
                   join nationality in session.Query<Nationality>() on customer.NationalityId equals nationality.Id
                   join country in session.Query<Country>() on customer.CountryId equals country.Id
                   join customerType in session.Query<CustomerType>() on customer.CustomerTypeId equals customerType.Id
                   join usageType in session.Query<UsageType>() on customer.UsageTypeId equals usageType.Id
                   join idType in session.Query<IdType>() on customer.IDTypeId equals idType.Id
                   join city in session.Query<City>() on customer.CityId equals city.Id
                   join occupation in session.Query<Occupation>() on customer.OccupationId equals occupation.Id
                   join title in session.Query<Title>() on customer.TitleId equals title.Id
                   join maritalstatus in session.Query<MaritalStatus>() on customer.MaritalStatusId equals maritalstatus.Id
                   where customer.UserName == username
                   select new { customer = customer, nationality = nationality, customerType = customerType, country = country, usageType = usageType, idType = idType, city = city, occupation = occupation, title = title, maritalstatus = maritalstatus };

                var result = query.FirstOrDefault();
                cDto.Id = result.customer.Id.ToString();
                cDto.FirstName = result.customer.FirstName;
                cDto.LastName = result.customer.LastName;
                cDto.UserName = result.customer.UserName;
                cDto.Password = result.customer.Password;
                cDto.NationalityId = result.customer.NationalityId;
                cDto.Nationality = result.nationality.NationalityName;
                cDto.CountryId = result.customer.CountryId;
                cDto.Country = result.country.CountryName;
                cDto.DateOfBirth = result.customer.DateOfBirth;
                cDto.MobileNo = result.customer.MobileNo;
                cDto.OtherTelNo = result.customer.OtherTelNo;
                cDto.CustomerTypeId = result.customer.CustomerTypeId;
                cDto.CustomerType = result.customerType.CustomerTypeName;
                cDto.UsageTypeId = result.customer.UsageTypeId;
                cDto.UsageType = result.usageType.UsageTypeName;
                cDto.Gender = result.customer.Gender;
                cDto.Address1 = result.customer.Address1;
                cDto.Address2 = result.customer.Address2;
                cDto.Address3 = result.customer.Address3;
                cDto.Address4 = result.customer.Address4;
                cDto.IDNo = result.customer.IDNo;
                cDto.IDTypeId = result.customer.IDTypeId;
                cDto.IDType = result.idType.IdTypeName;
                cDto.CityId = result.customer.CityId;
                cDto.City = result.city.CityName;
                cDto.DLIssueDate = result.customer.DLIssueDate;
                cDto.Email = result.customer.Email;
                cDto.IsActive = result.customer.IsActive;
                cDto.ProfilePictureSrc = Convert.ToBase64String(result.customer.ProfilePicture == null ? new byte[0] : result.customer.ProfilePicture);
                cDto.BusinessName = result.customer.BusinessName;
                cDto.BusinessAddress1 = result.customer.BusinessAddress1;
                cDto.BusinessAddress2 = result.customer.BusinessAddress2;
                cDto.BusinessAddress3 = result.customer.BusinessAddress3;
                cDto.BusinessAddress4 = result.customer.BusinessAddress4;
                cDto.BusinessTelNo = result.customer.BusinessTelNo;
                cDto.EntryDateTime = result.customer.EntryDateTime;
                cDto.EntryUserId = result.customer.EntryUserId;
                cDto.OccupationId = result.customer.OccupationId;
                cDto.TitleId = result.customer.TitleId;
                cDto.MaritalStatusId = result.customer.MaritalStatusId;
                cDto.Occupation = result.occupation.Name;
                cDto.Title = result.title.Name;
                cDto.MaritalStatus = result.maritalstatus.Name;
                cDto.PostalCode = result.customer.PostalCode;
                cDto.IsCustomerExists = true;

                return cDto;
            }
            else
            {
                cDto.IsCustomerExists = false;
                return cDto;
            }
        }

        public CustomerResponseDto GetCustomerById(Guid CustomerId)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();

                CustomerResponseDto pDto = new CustomerResponseDto();

                var query =
                    from Customer in session.Query<Customer>()
                    where Customer.Id == CustomerId
                    select new { Customer = Customer };

                var result = query.ToList();


                if (result != null && result.Count > 0)
                {
                    pDto.Id = result.First().Customer.Id.ToString();
                    pDto.Email = result.First().Customer.Email;
                    pDto.FirstName = result.First().Customer.FirstName;

                    pDto.EntryDateTime = result.First().Customer.EntryDateTime;
                    pDto.EntryUserId = result.First().Customer.EntryUserId;

                    pDto.IsCustomerExists = true;
                    return pDto;
                }
                else
                {
                    pDto.IsCustomerExists = false;
                    return null;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }
        }

        internal bool AddCustomer(CustomerRequestDto Customer)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();
                Customer cu = new Entities.Customer();

                cu.FirstName = Customer.FirstName;
                cu.LastName = Customer.LastName;
                cu.UserName = Customer.Email;
                cu.Password = Customer.Password;
                cu.NationalityId = Customer.NationalityId;
                cu.CountryId = Customer.CountryId;
                cu.DateOfBirth = Customer.DateOfBirth;
                cu.MobileNo = Customer.MobileNo;
                cu.OtherTelNo = Customer.OtherTelNo;
                cu.CustomerTypeId = Customer.CustomerTypeId;
                cu.UsageTypeId = Customer.UsageTypeId;
                cu.Gender = Customer.Gender;
                cu.Address1 = Customer.Address1;
                cu.Address2 = Customer.Address2;
                cu.Address3 = Customer.Address3;
                cu.Address4 = Customer.Address4;
                cu.IDNo = Customer.IDNo;
                cu.IDTypeId = Customer.IDTypeId;
                cu.CityId = Customer.CityId;
                cu.DLIssueDate = Customer.DLIssueDate;
                cu.Email = Customer.Email;
                cu.IsActive = Customer.IsActive;
                cu.BusinessName = Customer.BusinessName;
                cu.BusinessAddress1 = Customer.BusinessAddress1;
                cu.BusinessAddress2 = Customer.BusinessAddress2;
                cu.BusinessAddress3 = Customer.BusinessAddress3;
                cu.BusinessAddress4 = Customer.BusinessAddress4;
                cu.BusinessTelNo = Customer.BusinessTelNo;
                cu.EntryDateTime = DateTime.Today.ToUniversalTime();
                cu.OccupationId = Customer.OccupationId;
                cu.TitleId = Customer.TitleId;
                cu.MaritalStatusId = Customer.MaritalStatusId;
                cu.PostalCode = Customer.PostalCode;

                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.SaveOrUpdate(cu);
                    Customer.Id = cu.Id.ToString();
                    transaction.Commit();
                }

                return true;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return false;
            }

        }

        internal bool UpdateCustomer(CustomerRequestDto Customer)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();
                Customer cu = new Entities.Customer();

                cu.Id = Guid.Parse(Customer.Id.ToString());
                cu.FirstName = Customer.FirstName;
                cu.LastName = Customer.LastName;
                cu.UserName = Customer.Email;
                cu.Password = Customer.Password;
                cu.NationalityId = Customer.NationalityId;
                cu.CountryId = Customer.CountryId;
                cu.DateOfBirth = Customer.DateOfBirth;
                cu.MobileNo = Customer.MobileNo;
                cu.OtherTelNo = Customer.OtherTelNo;
                cu.CustomerTypeId = Customer.CustomerTypeId;
                cu.UsageTypeId = Customer.UsageTypeId;
                cu.Gender = Customer.Gender;
                cu.Address1 = Customer.Address1;
                cu.Address2 = Customer.Address2;
                cu.Address3 = Customer.Address3;
                cu.Address4 = Customer.Address4;
                cu.IDNo = Customer.IDNo;
                cu.IDTypeId = Customer.IDTypeId;
                cu.CityId = Customer.CityId;
                cu.DLIssueDate = Customer.DLIssueDate;
                cu.Email = Customer.Email;
                cu.IsActive = Customer.IsActive;
                cu.BusinessName = Customer.BusinessName;
                cu.BusinessAddress1 = Customer.BusinessAddress1;
                cu.BusinessAddress2 = Customer.BusinessAddress2;
                cu.BusinessAddress3 = Customer.BusinessAddress3;
                cu.BusinessAddress4 = Customer.BusinessAddress4;
                cu.BusinessTelNo = Customer.BusinessTelNo;
                cu.LastModifiedDateTime = DateTime.Today.ToUniversalTime();
                cu.EntryDateTime = Customer.EntryDateTime;
                cu.EntryUserId = Customer.EntryUserId;
                cu.OccupationId = Customer.OccupationId;
                cu.TitleId = Customer.TitleId;
                cu.MaritalStatusId = Customer.MaritalStatusId;
                cu.PostalCode = Customer.PostalCode;

                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Update(cu);
                    transaction.Commit();
                }
                return true;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return false;
            }
        }
        #endregion

        #region Occupation
        public List<OccupationResponseDto> GetOccupations()
        {
            List<Occupation> entities = null;
            ISession session = EntitySessionManager.GetSession();
            return session.Query<Occupation>().Select(Occupation => new OccupationResponseDto {
                Id = Occupation.Id,
                Name=Occupation.Name
            }).ToList();
        }
        internal bool AddOccupation(OccupationRequestDto Occupation)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();
                Occupation cu = new Entities.Occupation();

                cu.Id = Guid.NewGuid();
                cu.Name = Occupation.Name;

                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.SaveOrUpdate(cu);
                    Occupation.Id = cu.Id;
                    transaction.Commit();
                }

                return true;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return false;
            }

        }
        public OccupationResponseDto GetOccupationById(Guid OccupationId)
        {

            try
            {
                ISession session = EntitySessionManager.GetSession();

                OccupationResponseDto pDto = new OccupationResponseDto();

                var query =
                    from Occupation in session.Query<Occupation>()
                    where Occupation.Id == OccupationId
                    select new { Occupation = Occupation };

                var result = query.ToList();


                if (result != null && result.Count > 0)
                {
                    pDto.Id = result.First().Occupation.Id;
                    pDto.Name = result.First().Occupation.Name;
                    pDto.IsOccupationExists = true;
                    return pDto;
                }
                else
                {
                    pDto.IsOccupationExists = false;
                    return null;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }

        }
        #endregion

        #region Title
        public List<TitleResponseDto> GetTitles()
        {
            ISession session = EntitySessionManager.GetSession();
            return session.Query<Title>().Select(Title => new TitleResponseDto {
                Id = Title.Id,
                Name = Title.Name
        }).ToList();
        }
        #endregion

        #region MarritalStatus
        public List<MarritalStatusResponseDto> GetMarritalStatuses()
        {

            ISession session = EntitySessionManager.GetSession();
            return session.Query<MaritalStatus>().Select(MarritalStatus => new MarritalStatusResponseDto {
                Id = MarritalStatus.Id,
                Name = MarritalStatus.Name,
                //need to write other fields
            }).ToList();

        }
        #endregion

        internal static object GetAllCustomersForSearchGrid(CustomerSearchGridRequestDto CustomerSearchGridRequestDto)
        {

            try
            {
                if (CustomerSearchGridRequestDto != null && CustomerSearchGridRequestDto.paginationOptionsCustomerSearchGrid != null)
                {
                    Expression<Func<Customer, bool>> filterCustomer = PredicateBuilder.True<Customer>();
                    filterCustomer = filterCustomer.And(a => a.IsActive == true);
                    if (!String.IsNullOrEmpty(CustomerSearchGridRequestDto.customerSearchGridSearchCriterias.eMail))
                    {
                        filterCustomer = filterCustomer.And(a => a.Email.ToLower().Contains(CustomerSearchGridRequestDto.customerSearchGridSearchCriterias.eMail.ToLower()));
                    }
                    if (!String.IsNullOrEmpty(CustomerSearchGridRequestDto.customerSearchGridSearchCriterias.firstName))
                    {
                        filterCustomer = filterCustomer.And(a => a.FirstName.ToLower().Contains(CustomerSearchGridRequestDto.customerSearchGridSearchCriterias.firstName.ToLower()));
                    }
                    if (!String.IsNullOrEmpty(CustomerSearchGridRequestDto.customerSearchGridSearchCriterias.lastName))
                    {
                        filterCustomer = filterCustomer.And(a => a.LastName.ToLower().Contains(CustomerSearchGridRequestDto.customerSearchGridSearchCriterias.lastName.ToLower()));
                    }
                    if (!String.IsNullOrEmpty(CustomerSearchGridRequestDto.customerSearchGridSearchCriterias.mobileNo))
                    {
                        filterCustomer = filterCustomer.And(a => a.MobileNo.ToLower().Contains(CustomerSearchGridRequestDto.customerSearchGridSearchCriterias.mobileNo.ToLower()));
                    }
                    if (!String.IsNullOrEmpty(CustomerSearchGridRequestDto.customerSearchGridSearchCriterias.businessName))
                    {
                        filterCustomer = filterCustomer.And(a => a.BusinessName.ToLower().Contains(CustomerSearchGridRequestDto.customerSearchGridSearchCriterias.businessName.ToLower()));
                    }
                    ISession session = EntitySessionManager.GetSession();
                    var filteredCustomers = session.Query<Customer>().Where(filterCustomer);

                    long TotalRecords = filteredCustomers.Count();
                    var customerGridDetailsFilterd = filteredCustomers.Skip((CustomerSearchGridRequestDto.paginationOptionsCustomerSearchGrid.pageNumber - 1) * CustomerSearchGridRequestDto.paginationOptionsCustomerSearchGrid.pageSize)
                    .Take(CustomerSearchGridRequestDto.paginationOptionsCustomerSearchGrid.pageSize)
                    .Select(a => new
                    {
                        Id = a.Id,
                        FirstName = a.FirstName,
                        LastName = a.LastName,
                        MobileNo = a.MobileNo,
                        Email = a.Email,
                        BusinessName = a.BusinessName
                    })
                    .ToArray();
                    var response = new CommonGridResponseDto()
                    {
                        totalRecords = TotalRecords,
                        data = customerGridDetailsFilterd
                    };
                    return new JavaScriptSerializer().Serialize(response);

                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }
        }
        internal bool CheckCustomerExist(String Mobile) {
            bool result = false;
            ISession session = EntitySessionManager.GetSession();
            int count = session.Query<Customer>().Where(a => a.MobileNo == Mobile).Count();
            if (count > 0) {
                result = true;
            }
            return result;
        }

        internal bool UpdateCustomerInPolicy(Customer_ Customer)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();
                Customer cu = new Entities.Customer();
                session.Load(cu, Customer.customerId);
                cu.Id = Guid.Parse(Customer.customerId.ToString());
                cu.FirstName = Customer.firstName;
                cu.LastName = Customer.lastName;
                cu.UserName = Customer.email;
                cu.NationalityId = Customer.nationalityId;
                cu.CountryId = Customer.countryId;
                cu.DateOfBirth = Customer.dateOfBirth;
                cu.MobileNo = Customer.mobileNo;
                cu.OtherTelNo = Customer.otherTelNo;
                cu.CustomerTypeId = Customer.customerTypeId;
                cu.UsageTypeId = Customer.usageTypeId;
                cu.Gender = Customer.gender;
                cu.Address1 = Customer.address1;
                cu.Address2 = Customer.address2;
                cu.Address3 = Customer.address3;
                cu.Address4 = Customer.address4;
                cu.IDNo = Customer.idNo;
                cu.IDTypeId = Customer.idTypeId;
                cu.CityId = Customer.cityId;
                cu.DLIssueDate = Customer.idIssueDate;
                cu.Email = Customer.email;
                cu.BusinessName = Customer.businessName;
                cu.BusinessAddress1 = Customer.businessAddress1;
                cu.BusinessAddress2 = Customer.businessAddress2;
                cu.BusinessAddress3 = Customer.businessAddress3;
                cu.BusinessAddress4 = Customer.businessAddress4;
                cu.BusinessTelNo = Customer.businessTelNo;
                cu.LastModifiedDateTime = DateTime.Today.ToUniversalTime();

                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Update(cu, cu.Id);
                    transaction.Commit();
                }
                session.Flush();
                return true;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return false;
            }
        }



        internal static CustomerCheckResponseDto CustomerCheck(Guid CustomerId, Guid LoggedInUserId)
        {
            CustomerCheckResponseDto CustomerCheckResponse = new CustomerCheckResponseDto();

            try
            {
                ISession session = EntitySessionManager.GetSession();
                //IQueryable<Customer> customerData = session.Query<Customer>();
                IQueryable<Policy> customerPolicies = session.Query<Policy>().Where(a => a.CustomerId == CustomerId && a.IsApproved == true);

                if (customerPolicies.Count() == 0)
                {
                    CustomerCheckResponse = new CustomerCheckResponseDto()
                    {
                        CustomerId = CustomerId,
                        HasAccesstoPolicyApproval = false,
                        IsBordxConfirmed = false,
                        IsPolicyApproved = false
                    };
                }
                else
                {
                    if (customerPolicies.Where(a => a.BordxId != null && a.BordxId != Guid.Empty).Count() > 0)
                    {
                        CustomerCheckResponse = new CustomerCheckResponseDto()
                        {
                            CustomerId = CustomerId,
                            HasAccesstoPolicyApproval = false,
                            IsBordxConfirmed = true,
                            IsPolicyApproved = true
                        };
                    }
                    else
                    {
                        CustomerCheckResponse = new CustomerCheckResponseDto()
                        {
                            CustomerId = CustomerId,
                            HasAccesstoPolicyApproval = new MenuEntityManager().GetMenuAccessByUserId("#/app/policyApproval", LoggedInUserId),
                            IsBordxConfirmed = false,
                            IsPolicyApproved = true
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }


            return CustomerCheckResponse;
        }

        internal static List<CustomerResponseDto> GetCustomersByUserName(string tpaName)
        {
            List<CustomerResponseDto> entities = null;
            ISession session = EntitySessionManager.GetSession();
            return session.Query<Customer>().Where(a => a.UserName == tpaName).Select(Customer=> new CustomerResponseDto {
                FirstName = Customer.FirstName,
                LastName = Customer.LastName,
                UserName = Customer.UserName,
                Password = Customer.Password,
                NationalityId = Customer.NationalityId,
                CountryId = Customer.CountryId,
                DateOfBirth = Customer.DateOfBirth,
                MobileNo = Customer.MobileNo,
                OtherTelNo = Customer.OtherTelNo,
                CustomerTypeId = Customer.CustomerTypeId,
                UsageTypeId = Customer.UsageTypeId,
                Gender = Customer.Gender,
                Address1 = Customer.Address1,
                Address2 = Customer.Address2,
                Address3 = Customer.Address3,
                Address4 = Customer.Address4,
                IDNo = Customer.IDNo,
                IDTypeId = Customer.IDTypeId,
                CityId = Customer.CityId,
                DLIssueDate = Customer.DLIssueDate,
                Email = Customer.UserName,
                IsActive = Customer.IsActive,
                BusinessName = Customer.BusinessName,
                BusinessAddress1 = Customer.BusinessAddress1,
                BusinessAddress2 = Customer.BusinessAddress2,
                BusinessAddress3 = Customer.BusinessAddress3,
                BusinessAddress4 = Customer.BusinessAddress4,
                BusinessTelNo = Customer.BusinessTelNo,
                EntryDateTime = Customer.EntryDateTime,
                EntryUserId = Customer.EntryUserId,
                OccupationId = Customer.OccupationId,
                TitleId = Customer.TitleId,
                MaritalStatusId = Customer.MaritalStatusId,
                PostalCode = Customer.PostalCode,
                Id = Customer.Id.ToString()
            }).ToList();

        }

        internal static List<CustomerResponseDto> GetCustomerlistById(Guid customerID)
        {
            ISession session = EntitySessionManager.GetSession();
            return session.Query<Customer>().Where(a => a.Id == customerID).Select(Customer=> new CustomerResponseDto {
                FirstName = Customer.FirstName,
                LastName = Customer.LastName,
                UserName = Customer.UserName,
                Password = Customer.Password,
                NationalityId = Customer.NationalityId,
                CountryId = Customer.CountryId,
                DateOfBirth = Customer.DateOfBirth,
                MobileNo = Customer.MobileNo,
                OtherTelNo = Customer.OtherTelNo,
                CustomerTypeId = Customer.CustomerTypeId,
                UsageTypeId = Customer.UsageTypeId,
                Gender = Customer.Gender,
                Address1 = Customer.Address1,
                Address2 = Customer.Address2,
                Address3 = Customer.Address3,
                Address4 = Customer.Address4,
                IDNo = Customer.IDNo,
                IDTypeId = Customer.IDTypeId,
                CityId = Customer.CityId,
                DLIssueDate = Customer.DLIssueDate,
                Email = Customer.UserName,
                IsActive = Customer.IsActive,
                BusinessName = Customer.BusinessName,
                BusinessAddress1 = Customer.BusinessAddress1,
                BusinessAddress2 = Customer.BusinessAddress2,
                BusinessAddress3 = Customer.BusinessAddress3,
                BusinessAddress4 = Customer.BusinessAddress4,
                BusinessTelNo = Customer.BusinessTelNo,
                EntryDateTime = Customer.EntryDateTime,
                EntryUserId = Customer.EntryUserId,
                OccupationId = Customer.OccupationId,
                TitleId = Customer.TitleId,
                MaritalStatusId = Customer.MaritalStatusId,
                PostalCode = Customer.PostalCode,
                Id = Customer.Id.ToString(),
            }).ToList();
        }

        internal bool ChangePassword(string OldPassword, string NewPassword, Guid CustomerId)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();
                Customer SysCustomer = new Customer();
                SysCustomer = session.QueryOver<Customer>()
                    .Where(a => a.Id == CustomerId).SingleOrDefault();
                if (SysCustomer == null)
                {
                    return false;
                }
                if (SysCustomer.Password != OldPassword)
                {
                    return false;
                }

                SysCustomer.Password = NewPassword;
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Update(SysCustomer);
                    transaction.Commit();
                }
                return true;

            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return false;
            }
        }

        internal String ValidateEmail(String email)
        {
            String Response = String.Empty;
            try
            {
                ISession session = EntitySessionManager.GetSession();
                Customer Icustomer = new Entities.Customer();
                var IDbcustomer = from ICustomer in session.Query<Customer>()
                                  where ICustomer.Email == email
                                  select new { ICustomer = ICustomer };

                if (IDbcustomer != null && IDbcustomer.First().ICustomer != null && !String.IsNullOrEmpty(Convert.ToString(IDbcustomer.First().ICustomer.Id)))
                {
                    Response = Convert.ToString(IDbcustomer.First().ICustomer.Id);
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        internal string ValidateChangePasswordRequestId(Guid requestId)
        {
            string _Response = String.Empty;
            try
            {
                ISession session = EntitySessionManager.GetSession();
                ForgotPasswordRequest currentRequest = session.QueryOver<ForgotPasswordRequest>()
                    .Where(a => a.Id == requestId).SingleOrDefault();
                if (currentRequest != null && currentRequest.IsUsed == false && currentRequest.ExpiryTime >= DateTime.Now)
                {
                    _Response = currentRequest.SystemUserId;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return _Response;
        }

        internal void ChangeForgetPasswordCustomer(Guid requestId, Guid systemUserId, Guid currentCustomerId, string password)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();
                ForgotPasswordRequest currentRequest = session.QueryOver<ForgotPasswordRequest>()
                    .Where(a => a.Id == requestId).SingleOrDefault();
                Customer cus = session.Query<Customer>().Where(a => a.Id == currentCustomerId).FirstOrDefault();


                if (currentRequest != null && currentRequest.IsUsed == false && currentRequest.ExpiryTime >= DateTime.Now)
                {
                    //update request table
                    currentRequest.IsUsed = true;
                    currentRequest.PreviousPassword = cus.Password;
                    //update customer
                    cus.Password = password;


                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.Update(currentRequest);
                        session.Update(cus);
                        transaction.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                throw;
            }
        }
    }
}
