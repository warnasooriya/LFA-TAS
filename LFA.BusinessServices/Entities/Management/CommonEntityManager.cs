using NHibernate;
using NHibernate.Linq;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using TAS.DataTransfer.Responses;
using TAS.Services.Entities.Persistence;

namespace TAS.Services.Entities.Management
{
    public class CommonEntityManager
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        internal string GetUserNameById(Guid Id)
        {
            String Response = String.Empty;
            try
            {
                if (IsGuid(Id.ToString()))
                {
                    ISession session = EntitySessionManager.GetSession();
                    InternalUser user = session.Query<InternalUser>()
                        .Where(a => a.Id == Id.ToString()).FirstOrDefault();
                    if (user != null)
                    {
                        Response = user.FirstName + " " + user.LastName;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }

            return Response;
        }

        internal string getInvoiceCodeByPolicyId(Guid Id)
        {
            String Response = String.Empty;
            try
            {
                if (IsGuid(Id.ToString()))
                {
                    ISession session = EntitySessionManager.GetSession();
                    var invoiceCode = (from ci in session.Query<CustomerEnterdInvoiceDetails>()
                                       join ic in session.Query<InvoiceCode>() on ci.InvoiceCodeId equals ic.Id
                                       join id in session.Query<InvoiceCodeDetails>() on ic.Id equals id.InvoiceCodeId
                                       where id.PolicyId == Id
                                       select new
                                       {
                                           invoiceCode = ci.InvoiceCode
                                       }).FirstOrDefault().invoiceCode;
                    if (invoiceCode != null)
                    {
                        Response = invoiceCode;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }

            return Response;
        }
        internal string GetItemStatusByPolicyId(Guid policyBundleId)
        {
            string Response = String.Empty;

            try
            {
                if (IsGuid(policyBundleId.ToString()))
                {
                    ISession session = EntitySessionManager.GetSession();
                    Policy policy = session.Query<Policy>().FirstOrDefault(a => a.PolicyBundleId == policyBundleId);
                    if (policy != null)
                    {
                        var contractExtPremium = session.Query<ContractExtensionPremium>()
                            .FirstOrDefault(a => a.Id == policy.ContractExtensionPremiumId);
                        if (contractExtPremium != null)
                        {
                            Response = GetItemStatusNameById(contractExtPremium.ItemStatusId);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }

            return Response;
        }
        internal string GetDealerCurrencyCodeByDealerId(Guid dealerId)
        {
            string Response = string.Empty;

            try
            {
                if (IsGuid(dealerId.ToString()))
                {
                    ISession session = EntitySessionManager.GetSession();
                    Dealer Dealer = session.Query<Dealer>()
                        .FirstOrDefault(a => a.Id == dealerId);

                    if (Dealer != null)
                    {
                        Currency currency = session.Query<Currency>()
                       .FirstOrDefault(a => a.Id == Dealer.CurrencyId);
                        if (currency != null)
                        {
                            Response = currency.Code;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        internal string GetMakeCodeByPolicyId(Guid policyBundleId)
        {
            string Response = String.Empty;

            try
            {
                if (IsGuid(policyBundleId.ToString()))
                {
                    ISession session = EntitySessionManager.GetSession();
                    Policy policy = session.Query<Policy>().FirstOrDefault(a => a.PolicyBundleId == policyBundleId);
                    if (policy != null)
                    {
                        var commodityCode = GetCommodityTypeUniqueCodeById(policy.CommodityTypeId);
                        if (commodityCode.ToLower() == "a")
                        {
                            var vehicleDetails = session.Query<VehiclePolicy>()
                                .Join(session.Query<VehicleDetails>(), m => m.VehicleId, n => n.Id, (m, n) => new { m, n })
                                .Join(session.Query<Make>(), o => o.n.MakeId, p => p.Id, (o, p) => new { o, p })
                                .Where(q => q.o.m.PolicyId == policy.Id);
                            if (vehicleDetails != null && vehicleDetails.FirstOrDefault() != null
                                && vehicleDetails.FirstOrDefault().p != null)
                            {
                                Response = vehicleDetails.FirstOrDefault().p.MakeCode;
                            }
                        }
                        else if (commodityCode.ToLower() == "b")
                        {
                            var vehicleDetails = session.Query<VehiclePolicy>()
                                .Join(session.Query<VehicleDetails>(), m => m.VehicleId, n => n.Id, (m, n) => new { m, n })
                                .Join(session.Query<Make>(), o => o.n.MakeId, p => p.Id, (o, p) => new { o, p })
                                .Where(q => q.o.m.PolicyId == policy.Id);
                            if (vehicleDetails != null && vehicleDetails.FirstOrDefault() != null
                                && vehicleDetails.FirstOrDefault().p != null)
                            {
                                Response = vehicleDetails.FirstOrDefault().p.MakeCode;
                            }
                        }
                        else if (commodityCode.ToLower() == "e")
                        {
                            var vehicleDetails = session.Query<BAndWPolicy>()
                                .Join(session.Query<BrownAndWhiteDetails>(), m => m.BAndWId, n => n.Id, (m, n) => new { m, n })
                                .Join(session.Query<Make>(), o => o.n.MakeId, p => p.Id, (o, p) => new { o, p })
                                .Where(q => q.o.m.PolicyId == policy.Id);
                            if (vehicleDetails != null && vehicleDetails.FirstOrDefault() != null
                                && vehicleDetails.FirstOrDefault().p != null)
                            {
                                Response = vehicleDetails.FirstOrDefault().p.MakeCode;
                            }
                        }
                        else if (commodityCode.ToLower() == "y")
                        {
                            var vehicleDetails = session.Query<YellowGoodPolicy>()
                                .Join(session.Query<YellowGoodDetails>(), m => m.YellowGoodId, n => n.Id, (m, n) => new { m, n })
                                .Join(session.Query<Make>(), o => o.n.MakeId, p => p.Id, (o, p) => new { o, p })
                                .Where(q => q.o.m.PolicyId == policy.Id);
                            if (vehicleDetails != null && vehicleDetails.FirstOrDefault() != null
                                && vehicleDetails.FirstOrDefault().p != null)
                            {
                                Response = vehicleDetails.FirstOrDefault().p.MakeCode;
                            }
                        }
                        else if (commodityCode.ToLower() == "o")
                        {
                            var vehicleDetails = session.Query<OtherItemPolicy>()
                                .Join(session.Query<OtherItemDetails>(), m => m.OtherItemId, n => n.Id, (m, n) => new { m, n })
                                .Join(session.Query<Make>(), o => o.n.MakeId, p => p.Id, (o, p) => new { o, p })
                                .Where(q => q.o.m.PolicyId == policy.Id);
                            if (vehicleDetails != null && vehicleDetails.FirstOrDefault() != null
                                && vehicleDetails.FirstOrDefault().p != null)
                            {
                                Response = vehicleDetails.FirstOrDefault().p.MakeCode;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }

            return Response;
        }


        internal string GetCommodityCategoryNameByPolicyId(Guid policyBundleId)
        {
            string Response = String.Empty;

            try
            {
                if (IsGuid(policyBundleId.ToString()))
                {
                    ISession session = EntitySessionManager.GetSession();
                    Policy policy = session.Query<Policy>().FirstOrDefault(a => a.PolicyBundleId == policyBundleId);
                    if (policy != null)
                    {
                        var contract = session.Query<Contract>()
                            .FirstOrDefault(a => a.Id == policy.ContractId);
                        if (contract != null)
                        {
                            Response = GetCommodityCategoryNameById(contract.CommodityCategoryId);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }

            return Response;
        }

        internal string GetCustomerNameById(Guid Id)
        {
            String Response = String.Empty;

            try
            {
                if (IsGuid(Id.ToString()))
                {
                    ISession session = EntitySessionManager.GetSession();
                    Customer customer = session.Query<Customer>()
                        .Where(a => a.Id == Id).FirstOrDefault();
                    if (customer != null)
                    {
                        Response = customer.FirstName + " " + customer.LastName;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }

            return Response;
        }


        internal Customer getCustomerById(Guid Id)
        {
            Customer Response = null;

            try
            {
                if (IsGuid(Id.ToString()))
                {
                    ISession session = EntitySessionManager.GetSession();
                    Response = session.Query<Customer>()
                        .Where(a => a.Id == Id).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }

            return Response;
        }

        internal string getPlateNumberByPolicyId(Guid Id)
        {
            string Response = null;
            try
            {
                if (IsGuid(Id.ToString()))
                {
                    ISession session = EntitySessionManager.GetSession();
                    Response = (from icd in session.Query<InvoiceCodeDetails>()
                               join ic in session.Query<InvoiceCode>() on icd.InvoiceCodeId equals ic.Id
                               where icd.PolicyId == Id
                               select new
                               {
                                   PlateNumber = ic.PlateNumber
                               }).FirstOrDefault().PlateNumber;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }

            return Response;
        }




        internal string GetCustomerMobileNumberById(Guid Id)
        {
            String Response = String.Empty;

            try
            {
                if (IsGuid(Id.ToString()))
                {
                    ISession session = EntitySessionManager.GetSession();
                    Customer customer = session.Query<Customer>()
                        .Where(a => a.Id == Id).FirstOrDefault();
                    if (customer != null)
                    {
                        Response = customer.MobileNo;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }

            return Response;
        }

        internal string GetDealerCodeById(Guid Id)
        {
            String Response = String.Empty;

            try
            {
                if (IsGuid(Id.ToString()))
                {
                    ISession session = EntitySessionManager.GetSession();
                    Dealer dealer = session.Query<Dealer>()
                        .Where(a => a.Id == Id).FirstOrDefault();
                    if (dealer != null)
                    {
                        Response = dealer.DealerCode;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }

            return Response;
        }

        internal string GetCityNameById(Guid Id)
        {
            String Response = String.Empty;

            try
            {
                if (IsGuid(Id.ToString()))
                {
                    ISession session = EntitySessionManager.GetSession();
                    City city = session.Query<City>()
                        .Where(a => a.Id == Id).FirstOrDefault();
                    if (city != null)
                    {
                        Response = city.CityName;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }

            return Response;
        }

        internal string GetCountryNameById(Guid Id)
        {
            String Response = String.Empty;

            try
            {
                if (IsGuid(Id.ToString()))
                {
                    ISession session = EntitySessionManager.GetSession();
                    Country country = session.Query<Country>()
                        .Where(a => a.Id == Id).FirstOrDefault();
                    if (country != null)
                    {
                        Response = country.CountryName;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }

            return Response;
        }

        internal string GetCustomerTypeNameById(int Id)
        {
            String Response = String.Empty;

            try
            {
                if (Id > 0)
                {
                    ISession session = EntitySessionManager.GetSession();
                    CustomerType customerType = session.Query<CustomerType>()
                        .Where(a => a.Id == Id).FirstOrDefault();
                    if (customerType != null)
                    {
                        Response = customerType.CustomerTypeName;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }

            return Response;
        }

        internal string GetIdTypeNameById(int Id)
        {
            String Response = String.Empty;

            try
            {
                if (Id > 0)
                {
                    ISession session = EntitySessionManager.GetSession();
                    IdType idType = session.Query<IdType>()
                        .Where(a => a.Id == Id).FirstOrDefault();
                    if (idType != null)
                    {
                        Response = idType.IdTypeName;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }

            return Response;
        }

        internal string GetNationaltyNameById(int Id)
        {
            String Response = String.Empty;

            try
            {
                if (Id > 0)
                {
                    ISession session = EntitySessionManager.GetSession();
                    Nationality nationality = session.Query<Nationality>()
                        .Where(a => a.Id == Id).FirstOrDefault();
                    if (nationality != null)
                    {
                        Response = nationality.NationalityName;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }

            return Response;
        }

        //internal string GetUsageTypeNameById(int Id)
        //{
        //    String Response = String.Empty;
        //    if (Id > 0)
        //    {
        //        ISession session = EntitySessionManager.GetSession();
        //        UsageType usageType = session.Query<UsageType>()
        //            .Where(a => a.Id == Id).FirstOrDefault();
        //        if (usageType != null)
        //            Response = usageType.UsageTypeName;
        //    }
        //    return Response;
        //}

        internal string GetPaymentMethodNameById(Guid Id)
        {
            String Response = String.Empty;

            try
            {
                if (IsGuid(Id.ToString()))
                {
                    ISession session = EntitySessionManager.GetSession();
                    PaymentMode paymentMode = session.Query<PaymentMode>()
                        .Where(a => a.Id == Id).FirstOrDefault();
                    if (paymentMode != null)
                    {
                        Response = paymentMode.PaymentModeName;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        internal string GetAspirationTypeNameById(Guid Id)
        {
            String Response = String.Empty;

            try
            {
                if (IsGuid(Id.ToString()))
                {
                    ISession session = EntitySessionManager.GetSession();
                    VehicleAspirationType aspirationType = session.Query<VehicleAspirationType>()
                        .Where(a => a.Id == Id).FirstOrDefault();
                    if (aspirationType != null)
                    {
                        Response = aspirationType.AspirationTypeCode;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        internal string GetBodyTypeNameById(Guid Id)
        {
            String Response = String.Empty;

            try
            {
                if (IsGuid(Id.ToString()))
                {
                    ISession session = EntitySessionManager.GetSession();
                    VehicleBodyType bodyType = session.Query<VehicleBodyType>()
                        .Where(a => a.Id == Id).FirstOrDefault();
                    if (bodyType != null)
                    {
                        Response = bodyType.VehicleBodyTypeDescription;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        internal string GetCategoryNameById(Guid Id)
        {
            String Response = String.Empty;

            try
            {
                if (IsGuid(Id.ToString()))
                {
                    ISession session = EntitySessionManager.GetSession();
                    CommodityCategory category = session.Query<CommodityCategory>()
                        .Where(a => a.CommodityCategoryId == Id).FirstOrDefault();
                    if (category != null)
                    {
                        Response = category.CommodityCategoryDescription;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        internal string GetCommodityUsageTypeById(Guid Id)
        {
            String Response = String.Empty;

            try
            {
                if (IsGuid(Id.ToString()))
                {
                    ISession session = EntitySessionManager.GetSession();
                    CommodityUsageType usageType = session.Query<CommodityUsageType>()
                        .Where(a => a.Id == Id).FirstOrDefault();
                    if (usageType != null)
                    {
                        Response = usageType.Name;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        internal string GetCurrencyTypeById(Guid Id)
        {
            String Response = String.Empty;

            try
            {
                if (IsGuid(Id.ToString()))
                {
                    ISession session = EntitySessionManager.GetSession();
                    Currency currency = session.Query<Currency>()
                        .Where(a => a.Id == Id).FirstOrDefault();
                    if (currency != null)
                    {
                        Response = currency.CurrencyName;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }

            return Response;
        }

        internal string GetCurrencyTypeByIdCode(Guid Id)
        {
            String Response = String.Empty;

            try
            {
                if (IsGuid(Id.ToString()))
                {
                    ISession session = EntitySessionManager.GetSession();
                    Currency currency = session.Query<Currency>()
                        .Where(a => a.Id == Id).FirstOrDefault();
                    if (currency != null)
                    {
                        Response = currency.Code;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }

            return Response;
        }

        internal string GetCyllinderCountValueById(Guid Id)
        {
            String Response = String.Empty;

            try
            {
                if (IsGuid(Id.ToString()))
                {
                    ISession session = EntitySessionManager.GetSession();
                    CylinderCount cylinderCount = session.Query<CylinderCount>()
                        .Where(a => a.Id == Id).FirstOrDefault();
                    if (cylinderCount != null)
                    {
                        Response = cylinderCount.Count;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }

            return Response;
        }

        internal string GetDealerNameById(Guid Id)
        {
            String Response = String.Empty;

            try
            {
                if (IsGuid(Id.ToString()))
                {
                    ISession session = EntitySessionManager.GetSession();
                    Dealer dealer = session.Query<Dealer>()
                        .Where(a => a.Id == Id).FirstOrDefault();
                    if (dealer != null)
                    {
                        Response = dealer.DealerName;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }

            return Response;
        }

        internal string GetDriverTypeByName(Guid Id)
        {
            String Response = String.Empty;

            try
            {
                if (IsGuid(Id.ToString()))
                {
                    ISession session = EntitySessionManager.GetSession();
                    DriveType drivetype = session.Query<DriveType>().Where(a => a.Id == Id).FirstOrDefault();
                    if (drivetype != null)
                    {
                        Response = drivetype.DriveTypeDescription;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        internal string GetDealerLocationNameById(Guid Id)
        {
            String Response = String.Empty;

            try
            {
                if (IsGuid(Id.ToString()))
                {
                    ISession session = EntitySessionManager.GetSession();
                    DealerLocation dealerLocation = session.Query<DealerLocation>()
                        .Where(a => a.Id == Id).FirstOrDefault();
                    if (dealerLocation != null)
                    {
                        Response = dealerLocation.Location;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        internal string GetEngineCapacityNameById(Guid Id)
        {
            String Response = String.Empty;

            try
            {
                if (IsGuid(Id.ToString()))
                {
                    ISession session = EntitySessionManager.GetSession();
                    EngineCapacity engineCapacity = session.Query<EngineCapacity>()
                        .Where(a => a.Id == Id).FirstOrDefault();
                    if (engineCapacity != null)
                    {
                        Response = engineCapacity.EngineCapacityNumber.ToString() + engineCapacity.MesureType;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }

            return Response;
        }

        internal string GetFuelTypeNameById(Guid Id)
        {
            String Response = String.Empty;

            try
            {
                if (IsGuid(Id.ToString()))
                {
                    ISession session = EntitySessionManager.GetSession();
                    FuelType fuelType = session.Query<FuelType>()
                        .Where(a => a.FuelTypeId == Id).FirstOrDefault();
                    if (fuelType != null)
                    {
                        Response = fuelType.FuelTypeDescription;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        internal string GetItemStatusNameById(Guid Id)
        {
            String Response = String.Empty;

            try
            {
                if (IsGuid(Id.ToString()))
                {
                    ISession session = EntitySessionManager.GetSession();
                    ItemStatus itemStatus = session.Query<ItemStatus>()
                        .Where(a => a.Id == Id).FirstOrDefault();
                    if (itemStatus != null)
                    {
                        Response = itemStatus.Status;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }

            return Response;
        }

        internal string GetMakeNameById(Guid Id)
        {
            String Response = String.Empty;

            try
            {
                if (IsGuid(Id.ToString()))
                {
                    ISession session = EntitySessionManager.GetSession();
                    Make make = session.Query<Make>()
                        .Where(a => a.Id == Id).FirstOrDefault();
                    if (make != null)
                    {
                        Response = make.MakeName;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }

            return Response;
        }

        internal string GetModelNameById(Guid Id)
        {
            String Response = String.Empty;

            try
            {
                if (IsGuid(Id.ToString()))
                {
                    ISession session = EntitySessionManager.GetSession();
                    Model model = session.Query<Model>()
                        .Where(a => a.Id == Id).FirstOrDefault();
                    if (model != null)
                    {
                        Response = model.ModelName;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        internal String getProductTypeCodebYproductId(Guid productId)
        {
            String Response = null;

            try
            {
                if (IsGuid(productId.ToString()))
                {
                    ISession session = EntitySessionManager.GetSession();
                    Product product = session.Query<Product>().FirstOrDefault(a => a.Id == productId);
                    ProductType productType = session.Query<ProductType>().FirstOrDefault(a => a.Id == product.ProductTypeId);
                    if (productType != null)
                    {
                        Response = productType.Code;
                    }

                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }

            return Response;
        }


        internal string GetProductNameById(Guid Id)
        {
            String Response = String.Empty;

            try
            {
                if (IsGuid(Id.ToString()))
                {
                    ISession session = EntitySessionManager.GetSession();
                    Product product = session.Query<Product>()
                        .Where(a => a.Id == Id).FirstOrDefault();
                    if (product != null)
                    {
                        Response = product.Productname;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        internal string GetTransmissionTypeNameById(Guid Id)
        {
            String Response = String.Empty;

            try
            {
                if (IsGuid(Id.ToString()))
                {
                    ISession session = EntitySessionManager.GetSession();
                    TransmissionType transmissionType = session.Query<TransmissionType>()
                        .Where(a => a.Id == Id).FirstOrDefault();
                    if (transmissionType != null)
                    {
                        Response = transmissionType.TransmissionTypeCode;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        internal string GetVariantNameById(Guid Id)
        {
            String Response = String.Empty;

            try
            {
                if (IsGuid(Id.ToString()))
                {
                    ISession session = EntitySessionManager.GetSession();
                    Variant variant = session.Query<Variant>()
                        .Where(a => a.Id == Id).FirstOrDefault();
                    if (variant != null)
                    {
                        Response = variant.VariantName;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        internal string GetUsageTypeNameById(int Id)
        {
            String Response = String.Empty;

            try
            {
                //if (IsGuid(Id.ToString()))
                //{
                ISession session = EntitySessionManager.GetSession();
                UsageType usageType = session.Query<UsageType>()
                    .Where(a => a.Id == Id).FirstOrDefault();
                if (usageType != null)
                {
                    Response = usageType.UsageTypeName;
                }
                //}
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        internal string GetBranchNameById(Guid Id)
        {
            String Response = String.Empty;

            try
            {
                if (IsGuid(Id.ToString()))
                {
                    ISession session = EntitySessionManager.GetSession();
                    TPABranch tpaBranch = session.Query<TPABranch>()
                        .Where(a => a.Id == Id).FirstOrDefault();
                    if (tpaBranch != null)
                    {
                        Response = tpaBranch.BranchName;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        internal string GetContractNameById(Guid Id)
        {
            String Response = String.Empty;

            try
            {
                if (IsGuid(Id.ToString()))
                {
                    ISession session = EntitySessionManager.GetSession();
                    Contract contract = session.Query<Contract>()
                        .Where(a => a.Id == Id).FirstOrDefault();
                    if (contract != null)
                    {
                        Response = contract.DealName;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }

            return Response;
        }

        internal string GetCoverTypeNameById(Guid Id)
        {
            String Response = String.Empty;

            try
            {
                if (IsGuid(Id.ToString()))
                {
                    ISession session = EntitySessionManager.GetSession();
                    WarrantyType warrentyType = session.Query<WarrantyType>()
                        .Where(a => a.Id == Id).FirstOrDefault();
                    if (warrentyType != null)
                    {
                        Response = warrentyType.WarrantyTypeDescription;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        internal string GetExtentionTypeNameById(Guid Id)
        {
            String Response = String.Empty;

            try
            {
                if (IsGuid(Id.ToString()))
                {
                    ISession session = EntitySessionManager.GetSession();
                    ExtensionType extentionType = session.Query<ExtensionType>()
                        .Where(a => a.Id == Id).FirstOrDefault();
                    if (extentionType != null)
                    {
                        Response = extentionType.ExtensionName;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        internal bool GetRSAByProductId(Guid Id)
        {
            bool Response = false;

            try
            {
                if (IsGuid(Id.ToString()))
                {
                    ISession session = EntitySessionManager.GetSession();
                    Product product = session.Query<Product>()
                        .Where(a => a.Id == Id).FirstOrDefault();
                    if (product != null)
                    {
                        Guid productTypeId = product.ProductTypeId;
                        ProductType productType = session.Query<ProductType>()
                            .Where(a => a.Id == productTypeId).FirstOrDefault();
                        if (productType != null && productType.Code.ToLower().StartsWith("ras"))
                        {
                            Response = true;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        private bool IsGuid(string candidate)
        {
            if (candidate == "00000000-0000-0000-0000-000000000000")
            {
                return false;
            }

            Regex isGuid = new Regex(@"^(\{){0,1}[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}(\}){0,1}$", RegexOptions.Compiled);

            if (candidate != null)
            {
                if (isGuid.IsMatch(candidate))
                {
                    return true;
                }
            }

            return false;
        }


        internal string GetCommodityTypeNameById(Guid Id)
        {
            String Response = String.Empty;

            try
            {
                if (IsGuid(Id.ToString()))
                {
                    ISession session = EntitySessionManager.GetSession();
                    CommodityType commodityType = session.Query<CommodityType>()
                        .Where(a => a.CommodityTypeId == Id).FirstOrDefault();
                    if (commodityType != null)
                    {
                        Response = commodityType.CommodityTypeDescription;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }

            return Response;
        }

        internal string GetCommodityTypeUniqueCodeById(Guid Id)
        {
            String Response = String.Empty;

            try
            {
                if (IsGuid(Id.ToString()))
                {
                    ISession session = EntitySessionManager.GetSession();
                    CommodityType commodityType = session
                        .Query<CommodityType>().FirstOrDefault(a => a.CommodityTypeId == Id);
                    if (commodityType != null)
                    {
                        Response = commodityType.CommodityCode;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }


        internal string GetParentProductNameByProductId(Guid Id)
        {
            String Response = String.Empty;

            try
            {
                if (IsGuid(Id.ToString()))
                {
                    ISession session = EntitySessionManager.GetSession();
                    Product product = session.Query<Product>()
                        .Where(a => a.Id == Id).FirstOrDefault();
                    if (product != null)
                    {
                        Response = product.Productname;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }

            return Response;
        }

        internal string GetPremiumAddonTypeNameById(Guid Id)
        {
            String Response = String.Empty;

            try
            {
                if (IsGuid(Id.ToString()))
                {
                    ISession session = EntitySessionManager.GetSession();
                    PremiumAddonType addonType = session.Query<PremiumAddonType>()
                        .Where(a => a.Id == Id).FirstOrDefault();
                    if (addonType != null)
                    {
                        Response = addonType.Description;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        internal string GetTaxNameByTaxTypeId(Guid taxTypeId)
        {
            String Response = String.Empty;

            try
            {
                if (IsGuid(taxTypeId.ToString()))
                {
                    ISession session = EntitySessionManager.GetSession();
                    TaxTypes TaxTypes = session.Query<TaxTypes>()
                        .Where(a => a.Id == taxTypeId).FirstOrDefault();
                    if (TaxTypes != null)
                    {
                        Response = TaxTypes.TaxName;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        internal string GetTaxCodeByTaxTypeId(Guid taxTypeId)
        {
            String Response = String.Empty;

            try
            {
                if (IsGuid(taxTypeId.ToString()))
                {
                    ISession session = EntitySessionManager.GetSession();
                    TaxTypes TaxTypes = session.Query<TaxTypes>()
                        .Where(a => a.Id == taxTypeId).FirstOrDefault();
                    if (TaxTypes != null)
                    {
                        Response = TaxTypes.TaxCode;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        internal string GetPremiumBasedonCodeById(Guid premiumBasedonId)
        {
            String Response = String.Empty;

            try
            {
                if (IsGuid(premiumBasedonId.ToString()))
                {
                    ISession session = EntitySessionManager.GetSession();
                    PremiumBasedOn PremiumBasedOn = session
                        .Query<PremiumBasedOn>().Where(a => a.Id == premiumBasedonId).FirstOrDefault();
                    if (PremiumBasedOn != null)
                    {
                        Response = PremiumBasedOn.Code;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }

            return Response;
        }


        internal string getAddonTypeCodeById(Guid ContractExtensionId, IEnumerable<PremiumBasedOn> premiumBasedOn)
        {
            String Response = String.Empty;
            try
            {
                if (IsGuid(ContractExtensionId.ToString()))
                {
                    PremiumBasedOn basedOn = premiumBasedOn.Where(a => a.Id == ContractExtensionId).FirstOrDefault();
                    if (basedOn != null)
                    {
                        Response = basedOn.Code;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }


        internal decimal GetPaymentMethodPercentageByPaymentMethodId(Guid paymentMethodId)
        {
            decimal Response = decimal.Zero;

            try
            {
                if (IsGuid(paymentMethodId.ToString()))
                {
                    ISession session = EntitySessionManager.GetSession();
                    PaymentOptions PaymentOptions = session
                        .Query<PaymentOptions>().FirstOrDefault(a => a.Id == paymentMethodId);
                    if (PaymentOptions != null)
                    {
                        Response = PaymentOptions.PaymentCharge;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }



        internal string GetHeadOfficeContactPersonNameByDealerId(Guid dealerId)
        {
            string Response = String.Empty;

            try
            {
                if (IsGuid(dealerId.ToString()))
                {
                    ISession session = EntitySessionManager.GetSession();
                    DealerLocation DealerLocation = session.Query<DealerLocation>()
                        .Where(a => a.HeadOfficeBranch == true && a.DealerId == dealerId).FirstOrDefault();
                    if (DealerLocation != null)
                    {
                        Response = DealerLocation.SalesContactPerson;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        internal Guid GetDealerCurrencyIdByDealerId(Guid dealerId)
        {
            Guid Response = Guid.Empty;

            try
            {
                if (IsGuid(dealerId.ToString()))
                {
                    ISession session = EntitySessionManager.GetSession();
                    Dealer Dealer = session.Query<Dealer>()
                        .Where(a => a.Id == dealerId).FirstOrDefault();
                    if (Dealer != null)
                    {
                        Response = Dealer.CurrencyId;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        internal Guid GetCommodityUsageTypeByName(string usageTypeCode)
        {
            Guid Response = Guid.Empty;

            try
            {
                if (!string.IsNullOrEmpty(usageTypeCode))
                {
                    ISession session = EntitySessionManager.GetSession();
                    CommodityUsageType usageType = session.Query<CommodityUsageType>()
                        .FirstOrDefault(a => a.Name.Trim().ToLower() == usageTypeCode.Trim().ToLower());
                    if (usageType != null)
                    {
                        Response = usageType.Id;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        internal string GetSerialNumberMakeIdModelIdByPolicyIdCommodityTypeId(Guid policyId, Guid commodityTypeId, string type)
        {
            string Response = string.Empty;

            try
            {
                if (IsGuid(policyId.ToString()) && IsGuid(commodityTypeId.ToString()))
                {
                    ISession session = EntitySessionManager.GetSession();
                    CommodityType commodityType = session.Query<CommodityType>()
                        .Where(a => a.CommodityTypeId == commodityTypeId).FirstOrDefault();
                    if (commodityType == null)
                    {
                        return "vin/serial not found";
                    }

                    if (commodityType.CommodityCode == "A")
                    {
                        VehiclePolicy vehiclePolicy = session.Query<VehiclePolicy>()
                            .Where(a => a.PolicyId == policyId).FirstOrDefault();
                        if (vehiclePolicy != null)
                        {
                            VehicleDetails vehicleDetails = session.Query<VehicleDetails>()
                                .Where(a => a.Id == vehiclePolicy.VehicleId).FirstOrDefault();
                            if (vehicleDetails != null)
                            {
                                if (type == "serial")
                                {
                                    Response = vehicleDetails.VINNo;
                                }
                                else if (type == "make")
                                {
                                    Response = vehicleDetails.MakeId.ToString();
                                }
                                else if (type == "model")
                                {
                                    Response = vehicleDetails.ModelId.ToString();
                                }
                                else
                                {
                                    Response = "";
                                }
                            }
                        }

                    }
                    else if (commodityType.CommodityCode == "B")
                    {
                        VehiclePolicy vehiclePolicy = session.Query<VehiclePolicy>()
                            .Where(a => a.PolicyId == policyId).FirstOrDefault();
                        if (vehiclePolicy != null)
                        {
                            VehicleDetails vehicleDetails = session.Query<VehicleDetails>()
                                .Where(a => a.Id == vehiclePolicy.VehicleId).FirstOrDefault();
                            if (vehicleDetails != null)
                            {
                                if (type == "serial")
                                {
                                    Response = vehicleDetails.VINNo;
                                }
                                else if (type == "make")
                                {
                                    Response = vehicleDetails.MakeId.ToString();
                                }
                                else if (type == "model")
                                {
                                    Response = vehicleDetails.ModelId.ToString();
                                }
                                else
                                {
                                    Response = "";
                                }
                            }
                        }

                    }
                    else if (commodityType.CommodityCode == "E")
                    {
                        BAndWPolicy electronicPolicy = session.Query<BAndWPolicy>()
                           .Where(a => a.PolicyId == policyId).FirstOrDefault();
                        if (electronicPolicy != null)
                        {
                            BrownAndWhiteDetails electronicDetails = session.Query<BrownAndWhiteDetails>()
                                .Where(a => a.Id == electronicPolicy.BAndWId).FirstOrDefault();
                            if (electronicDetails != null)
                            {
                                if (type == "serial")
                                {
                                    Response = electronicDetails.SerialNo;
                                }
                                else if (type == "make")
                                {
                                    Response = electronicDetails.MakeId.ToString();
                                }
                                else if (type == "model")
                                {
                                    Response = electronicDetails.ModelId.ToString();
                                }
                                else
                                {
                                    Response = "";
                                }
                            }
                        }
                    }
                    else if (commodityType.CommodityCode == "O")
                    {
                        OtherItemPolicy otherPolicy = session.Query<OtherItemPolicy>()
                         .Where(a => a.PolicyId == policyId).FirstOrDefault();
                        if (otherPolicy != null)
                        {
                            OtherItemDetails otherDetails = session.Query<OtherItemDetails>()
                                .Where(a => a.Id == otherPolicy.OtherItemId).FirstOrDefault();
                            if (otherDetails != null)
                            {
                                if (type == "serial")
                                {
                                    Response = otherDetails.SerialNo;
                                }
                                else if (type == "make")
                                {
                                    Response = otherDetails.MakeId.ToString();
                                }
                                else if (type == "model")
                                {
                                    Response = otherDetails.ModelId.ToString();
                                }
                                else
                                {
                                    Response = "";
                                }
                            }
                        }
                    }
                    else
                    {
                        YellowGoodPolicy ygPolicy = session.Query<YellowGoodPolicy>()
                         .Where(a => a.PolicyId == policyId).FirstOrDefault();
                        if (ygPolicy != null)
                        {
                            YellowGoodDetails ygDetails = session.Query<YellowGoodDetails>()
                                .Where(a => a.Id == ygPolicy.YellowGoodId).FirstOrDefault();
                            if (ygDetails != null)
                            {
                                if (type == "serial")
                                {
                                    Response = ygDetails.SerialNo;
                                }
                                else if (type == "make")
                                {
                                    Response = ygDetails.MakeId.ToString();
                                }
                                else if (type == "model")
                                {
                                    Response = ygDetails.ModelId.ToString();
                                }
                                else
                                {
                                    Response = "";
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }


            return Response;
        }

        internal Guid GetCommodityCategoryIdByContractId(Guid contractId)
        {
            Guid Response = Guid.Empty;

            try
            {
                if (IsGuid(contractId.ToString()))
                {
                    ISession session = EntitySessionManager.GetSession();
                    Contract contract = session.Query<Contract>()
                        .Where(a => a.Id == contractId).FirstOrDefault();
                    if (contract != null)
                    {
                        Response = contract.CommodityCategoryId;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }

            return Response;
        }

        internal Guid GetClaimStatusIdByCode(string code)
        {
            Guid Response = Guid.Empty;

            try
            {
                if (!String.IsNullOrEmpty(code))
                {
                    ISession session = EntitySessionManager.GetSession();
                    ClaimStatusCode claimStatus = session.Query<ClaimStatusCode>()
                        .Where(a => a.StatusCode.ToLower() == code.ToLower()).FirstOrDefault();
                    if (claimStatus != null)
                    {
                        Response = claimStatus.Id;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }

            return Response;
        }

        internal Guid getClaimRejectTypeByCode(string code)
        {
            Guid Response = Guid.Empty;

            try
            {
                if (!String.IsNullOrEmpty(code))
                {
                    ISession session = EntitySessionManager.GetSession();
                    ClaimRejectionType claimRejectType = session.Query<ClaimRejectionType>()
                        .Where(a => a.Code.ToLower() == code.ToLower()).FirstOrDefault();
                    if (claimRejectType != null)
                    {
                        Response = claimRejectType.Id;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }

            return Response;
        }



        internal Guid GetClaimTypeIdByClaimTypeCode(string code)
        {
            Guid Response = Guid.Empty;

            try
            {
                if (!String.IsNullOrEmpty(code))
                {
                    ISession session = EntitySessionManager.GetSession();
                    ClaimItemType claimItemType = session.Query<ClaimItemType>()
                        .Where(a => a.ItemCode.ToLower() == code.ToLower()).FirstOrDefault();
                    if (claimItemType != null)
                    {
                        Response = claimItemType.Id;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }

            return Response;
        }

        internal Guid GetDealerCuntryByDealerId(Guid dealerId)
        {
            Guid Response = Guid.Empty;

            try
            {
                if (IsGuid(dealerId.ToString()))
                {
                    ISession session = EntitySessionManager.GetSession();
                    Dealer dealer = session.Query<Dealer>()
                        .Where(a => a.Id == dealerId).FirstOrDefault();
                    if (dealer != null)
                    {
                        Response = dealer.CountryId;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        internal String GetCommodityCategoryNameById(Guid commodityCategoryId)
        {
            String Response = String.Empty;

            try
            {
                if (IsGuid(commodityCategoryId.ToString()))
                {
                    ISession session = EntitySessionManager.GetSession();
                    CommodityCategory commodityCategory = session.Query<CommodityCategory>()
                        .Where(a => a.CommodityCategoryId == commodityCategoryId).FirstOrDefault();
                    if (commodityCategory != null)
                    {
                        Response = commodityCategory.CommodityCategoryDescription;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }

            return Response;
        }

        internal string GetClaimStatusCodeById(Guid claimStatisId)
        {
            string Response = "NA";

            try
            {
                if (IsGuid(claimStatisId.ToString()))
                {
                    ISession session = EntitySessionManager.GetSession();
                    ClaimStatusCode claimStatus = session.Query<ClaimStatusCode>()
                        .Where(a => a.Id == claimStatisId).FirstOrDefault();
                    if (claimStatus != null)
                    {
                        Response = claimStatus.StatusCode;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }
        internal string GetClaimStatusCodeByIdAdmin(Guid claimStatisId)
        {
            string Response = "NA";

            try
            {
                if (IsGuid(claimStatisId.ToString()))
                {
                    ISession session = EntitySessionManager.GetSession();
                    ClaimStatusCode claimStatus = session.Query<ClaimStatusCode>()
                        .Where(a => a.Id == claimStatisId).FirstOrDefault();
                    if (claimStatus != null)
                    {
                        Response = claimStatus.StatusCode;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        internal int GetClaimStatusSeqById(Guid claimStatisId)
        {
            int Response = 0;

            try
            {
                if (IsGuid(claimStatisId.ToString()))
                {
                    ISession session = EntitySessionManager.GetSession();
                    ClaimStatusCode claimStatus = session.Query<ClaimStatusCode>()
                        .Where(a => a.Id == claimStatisId).FirstOrDefault();
                    if (claimStatus != null)
                    {
                        Response = claimStatus.DisplayOrder;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        internal string GetClaimTypeCodeById(Guid claimTypeId)
        {
            string Response = string.Empty;

            try
            {
                if (IsGuid(claimTypeId.ToString()))
                {
                    ISession session = EntitySessionManager.GetSession();
                    ClaimItemType claimItemType = session.Query<ClaimItemType>()
                        .Where(a => a.Id == claimTypeId).FirstOrDefault();
                    if (claimItemType != null)
                    {
                        Response = claimItemType.ItemCode;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }

            return Response;
        }

        internal Guid GetPartAreaIdByPartId(Guid? partId)
        {
            Guid Response = Guid.Empty;

            try
            {
                if (IsGuid(partId.ToString()))
                {
                    ISession session = EntitySessionManager.GetSession();
                    Part part = session.Query<Part>()
                        .Where(a => a.Id == partId).FirstOrDefault();
                    if (part != null)
                    {
                        Response = part.PartAreaId;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        internal decimal getUnusedTyreDepthByClaimItemId(Guid claimItemId, string CommodityTypeDescription)
        {
            decimal Response = 0;
            try
            {
                if (IsGuid(claimItemId.ToString()))
                {
                    if(CommodityTypeDescription == "Tyre")
                    {
                        ISession session = EntitySessionManager.GetSession();
                        Response = session.Query<ClaimItemTireDetails>()
                            .Where(a => a.ClaimItemId == claimItemId).FirstOrDefault().UnUsedTireDepth;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }


        internal Dealer GetDealerById(Guid dealerId)
        {
            Dealer Response = new Dealer();
            try
            {
                if (IsGuid(dealerId.ToString()))
                {
                    ISession session = EntitySessionManager.GetSession();
                    Dealer dealer = session
                        .Query<Dealer>().FirstOrDefault(a => a.Id == dealerId);
                    if (dealer != null)
                    {
                        Response = dealer;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        internal string GetCountryCodeById(Guid countryId)
        {
            string Response = string.Empty;

            try
            {
                if (IsGuid(countryId.ToString()))
                {
                    ISession session = EntitySessionManager.GetSession();
                    Country country = session
                        .Query<Country>().FirstOrDefault(a => a.Id == countryId);
                    if (country != null)
                    {
                        Response = country.CountryCode;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        internal string GetFaultNameById(Guid? faultId)
        {
            string Response = string.Empty;

            try
            {
                if (IsGuid(faultId.ToString()))
                {
                    ISession session = EntitySessionManager.GetSession();
                    Fault fault = session
                        .Query<Fault>().FirstOrDefault(a => a.Id == faultId);
                    if (fault != null)
                    {
                        Response = fault.FaultName;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        internal Guid GetDealerIdByClaimId(Guid claimId)
        {
            Guid Response = Guid.Empty;
            try
            {
                if (IsGuid(claimId.ToString()))
                {
                    ISession session = EntitySessionManager.GetSession();
                    ClaimSubmission claim = session
                        .Query<ClaimSubmission>().FirstOrDefault(a => a.Id == claimId);
                    if (claim != null)
                    {
                        Response = claim.ClaimSubmittedDealerId;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        internal Guid GetPolicyIdByClaimId(Guid claimId)
        {
            Guid Response = Guid.Empty;

            try
            {
                if (IsGuid(claimId.ToString()))
                {
                    ISession session = EntitySessionManager.GetSession();
                    ClaimSubmission claim = session
                        .Query<ClaimSubmission>().FirstOrDefault(a => a.Id == claimId);
                    if (claim != null)
                    {
                        Response = claim.PolicyId;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        internal string GetInsurerNameById(Guid Id)
        {
            String Response = String.Empty;

            try
            {
                if (IsGuid(Id.ToString()))
                {
                    ISession session = EntitySessionManager.GetSession();
                    Insurer insurer = session.Query<Insurer>()
                        .Where(a => a.Id == Id).FirstOrDefault();
                    if (insurer != null)
                    {
                        Response = insurer.InsurerShortName;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        internal string GetReinsurerNameById(Guid Id)
        {
            String Response = String.Empty;

            try
            {
                if (IsGuid(Id.ToString()))
                {
                    ISession session = EntitySessionManager.GetSession();
                    Reinsurer reinsurer = session.Query<Reinsurer>()
                        .Where(a => a.Id == Id).FirstOrDefault();
                    if (reinsurer != null)
                    {
                        Response = reinsurer.ReinsurerName;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        internal List<DealerData> GetAllDealersByMakeId(Guid makeId)
        {
            List<DealerData> resposne = new List<DealerData>();

            try
            {
                if (IsGuid(makeId.ToString()))
                {
                    ISession session = EntitySessionManager.GetSession();
                    List<Guid> dealerMakeIds = session.Query<DealerMakes>().Where(a => a.MakeId == makeId).Select(a => a.DealerId).ToList();
                    var dealers = session.Query<Dealer>().Where(a => dealerMakeIds.Contains(a.Id)).ToList();


                    resposne.AddRange(dealers.Select(dealer => new DealerData()
                    {
                        Id = dealer.Id,
                        Code = dealer.DealerCode,
                        Name = dealer.DealerName,
                        Country = GetCountryNameById(dealer.CountryId),
                        CurrencyCode = GetCurrencyTypeByIdCode(dealer.CurrencyId)
                    }));
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return resposne;
        }

        internal Guid GetDealerByPolicyBundleId(Guid policyBundleId)
        {
            Guid Response = Guid.Empty;

            try
            {
                if (IsGuid(policyBundleId.ToString()))
                {
                    ISession session = EntitySessionManager.GetSession();
                    PolicyBundle policyBundle = session
                        .Query<PolicyBundle>().FirstOrDefault(a => a.Id == policyBundleId);
                    if (policyBundle != null)
                    {
                        Response = policyBundle.DealerId;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        internal object GetDeakerDiscountSchemeById(Guid schemeId)
        {
            string Response = string.Empty;

            try
            {
                if (IsGuid(schemeId.ToString()))
                {
                    ISession session = EntitySessionManager.GetSession();
                    DealerDiscountScheme discountScheme = session
                        .Query<DealerDiscountScheme>().FirstOrDefault(a => a.Id == schemeId);
                    if (discountScheme != null)
                    {
                        Response = discountScheme.SchemeName;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        internal string GetDiscountSchemeCodeById(Guid? schemeId)
        {
            string Response = string.Empty;

            try
            {
                if (IsGuid(schemeId.ToString()))
                {
                    ISession session = EntitySessionManager.GetSession();
                    DealerDiscountScheme discountScheme = session
                        .Query<DealerDiscountScheme>().FirstOrDefault(a => a.Id == schemeId);
                    if (discountScheme != null)
                    {
                        Response = discountScheme.SchemeCode;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }



        internal Guid? GetDealerDiscountSchemeIdByCode(string DiscountSchemeCode)
        {
            Guid? Response = null;

            try
            {
                if (!string.IsNullOrEmpty(DiscountSchemeCode))
                {
                    ISession session = EntitySessionManager.GetSession();
                    DealerDiscountScheme discountScheme = session
                        .Query<DealerDiscountScheme>().FirstOrDefault(a => a.SchemeCode.ToLower() == DiscountSchemeCode.ToLower());
                    if (discountScheme != null)
                    {
                        Response = discountScheme.Id;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }

            return Response;
        }

        public string GetDealerLocationNameByUserIdId(Guid requestedUserId)
        {
            string Response = string.Empty;

            try
            {
                if (IsGuid(requestedUserId.ToString()))
                {
                    ISession session = EntitySessionManager.GetSession();
                    //  DealerStaff ds = session.Query<DealerBranchStaff>() yet to do once dealer staff maping with branch done

                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }

            return Response;
        }
        internal object GetInsuranceLimitationMonthsById(Guid insuaranceLimitationId)
        {
            string Response = string.Empty;

            try
            {
                if (IsGuid(insuaranceLimitationId.ToString()))
                {

                    ISession session = EntitySessionManager.GetSession();
                    InsuaranceLimitation insuaranceLimitation = session
                        .Query<InsuaranceLimitation>().FirstOrDefault(a => a.Id == insuaranceLimitationId);
                    if (insuaranceLimitation != null)
                    {
                        Response = Convert.ToString(insuaranceLimitation.Months) + " M ";
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }
        public string GetInsuranceLimitationNameById(Guid insuaranceLimitationId)
        {
            string Response = string.Empty;

            try
            {
                if (IsGuid(insuaranceLimitationId.ToString()))
                {

                    ISession session = EntitySessionManager.GetSession();
                    InsuaranceLimitation insuaranceLimitation = session
                        .Query<InsuaranceLimitation>().FirstOrDefault(a => a.Id == insuaranceLimitationId);
                    if (insuaranceLimitation != null)
                    {
                        Response = insuaranceLimitation.InsuaranceLimitationName;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        public string GetWarrentyTypeNameById(Guid warrentyTypeId)
        {
            string Response = string.Empty;

            try
            {
                if (IsGuid(warrentyTypeId.ToString()))
                {
                    ISession session = EntitySessionManager.GetSession();
                    WarrantyType warrantyType = session
                        .Query<WarrantyType>().FirstOrDefault(a => a.Id == warrentyTypeId);
                    if (warrantyType != null)
                    {
                        Response = warrantyType.WarrantyTypeDescription;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        public Guid GetCommodityTypeIdByContractId(Guid contractId)
        {
            Guid Response = Guid.Empty;

            try
            {
                if (IsGuid(contractId.ToString()))
                {
                    ISession session = EntitySessionManager.GetSession();
                    Contract contract = session
                        .Query<Contract>().FirstOrDefault(a => a.Id == contractId);


                    if (contract != null) { }
                    Response = contract.CommodityTypeId;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        public string GetCurrencyCodeById(Guid currencyId)
        {
            String Response = String.Empty;

            try
            {
                if (IsGuid(currencyId.ToString()))
                {
                    ISession session = EntitySessionManager.GetSession();
                    Currency currency = session.Query<Currency>()
                        .Where(a => a.Id == currencyId).FirstOrDefault();
                    if (currency != null)
                    {
                        Response = currency.Code;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }

            return Response;
        }

        public Guid GetDealerCountryByDealerId(Guid dealerId)
        {
            Guid Response = Guid.Empty;

            try
            {
                if (IsGuid(dealerId.ToString()))
                {
                    ISession session = EntitySessionManager.GetSession();
                    Dealer dealer = session
                        .Query<Dealer>().FirstOrDefault(a => a.Id == dealerId);


                    if (dealer != null) { }
                    Response = dealer.CountryId;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        internal string GetCommodityTypeIdbyCommodityCategoryId(Guid Id)
        {
            String Response = String.Empty;

            try
            {
                if (IsGuid(Id.ToString()))
                {
                    ISession session = EntitySessionManager.GetSession();
                    CommodityCategory commodityCategory = session
                        .Query<CommodityCategory>().FirstOrDefault(a => a.CommodityCategoryId == Id);
                    if (commodityCategory != null)
                    {
                        Response = commodityCategory.CommodityTypeId.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        internal Guid GetCommodityCategoryIdByPolicyId(Guid policyId)
        {
            Guid Response = Guid.Empty;

            try
            {
                if (IsGuid(policyId.ToString()))
                {
                    ISession session = EntitySessionManager.GetSession();

                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        internal string GetClaimNoByClaimId(Guid ClaimId)
        {
            String Response = String.Empty;

            try
            {
                if (IsGuid(ClaimId.ToString()))
                {
                    ISession session = EntitySessionManager.GetSession();
                    Claim claim = session.Query<Claim>()
                        .Where(a => a.Id == ClaimId).FirstOrDefault();
                    if (claim != null)
                    {
                        Response = claim.ClaimNumber;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }

            return Response;
        }

        internal string GetPolicyNoByPolicyId(Guid PolicyId)
        {
            String Response = String.Empty;

            try
            {
                if (IsGuid(PolicyId.ToString()))
                {
                    ISession session = EntitySessionManager.GetSession();
                    Policy policy = session.Query<Policy>()
                        .Where(a => a.Id == PolicyId).FirstOrDefault();
                    if (policy != null)
                    {
                        Response = policy.PolicyNo;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }

            return Response;
        }

        internal string GetProfilePictureByUserId(Guid loggedInUserId)
        {
            //default image set
            String Response = "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AENybkNib29sAAAAAABDbnRDYm9vbAAAAAAATGJsc2Jvb2wAAAAAAE5ndHZib29sAAAAAABFbWxEYm9vbAAAAAAASW50cmJvb2wAAAAAAEJja2dPYmpjAAAAAQAAAAAAAFJHQkMAAAADAAAAAFJkICBkb3ViQG/gAAAAAAAAAAAAR3JuIGRvdWJAb+AAAAAAAAAAAABCbCAgZG91YkBv4AAAAAAAAAAAAEJyZFRVbnRGI1JsdAAAAAAAAAAAAAAAAEJsZCBVbnRGI1JsdAAAAAAAAAAAAAAAAFJzbHRVbnRGI1B4bEBSAAAAAAAAAAAACnZlY3RvckRhdGFib29sAQAAAABQZ1BzZW51bQAAAABQZ1BzAAAAAFBnUEMAAAAATGVmdFVudEYjUmx0AAAAAAAAAAAAAAAAVG9wIFVudEYjUmx0AAAAAAAAAAAAAAAAU2NsIFVudEYjUHJjQFkAAAAAAAAAAAAQY3JvcFdoZW5QcmludGluZ2Jvb2wAAAAADmNyb3BSZWN0Qm90dG9tbG9uZwAAAAAAAAAMY3JvcFJlY3RMZWZ0bG9uZwAAAAAAAAANY3JvcFJlY3RSaWdodGxvbmcAAAAAAAAAC2Nyb3BSZWN0VG9wbG9uZwAAAAAAOEJJTQPtAAAAAAAQAEgAAAABAAIASAAAAAEAAjhCSU0EJgAAAAAADgAAAAAAAAAAAAA/gAAAOEJJTQQNAAAAAAAEAAAAHjhCSU0EGQAAAAAABAAAAB44QklNA/MAAAAAAAkAAAAAAAAAAAEAOEJJTScQAAAAAAAKAAEAAAAAAAAAAjhCSU0D9QAAAAAASAAvZmYAAQBsZmYABgAAAAAAAQAvZmYAAQChmZoABgAAAAAAAQAyAAAAAQBaAAAABgAAAAAAAQA1AAAAAQAtAAAABgAAAAAAAThCSU0D+AAAAAAAcAAA/////////////////////////////wPoAAAAAP////////////////////////////8D6AAAAAD/////////////////////////////A+gAAAAA/////////////////////////////wPoAAA4QklNBAgAAAAAABAAAAABAAACQAAAAkAAAAAAOEJJTQQeAAAAAAAEAAAAADhCSU0EGgAAAAADWwAAAAYAAAAAAAAAAAAAAMgAAADIAAAAEwBk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JJJTm/8AN3pfhb/24Uv+bvS/C3/twrSSSU5v/N3pfhb/ANuFL/m70vwtP/XCtJJJTXxum4GI7dj0ta8cWGXO/wA96sJJJKUkkkkpSSSSSn//0PSUkkklKSSSSUpJJJJSkkkklKSSSSUpJJJJSkkkklKSSSSUpJJJJSkkkklP/9k4QklNBCEAAAAAAFUAAAABAQAAAA8AQQBkAG8AYgBlACAAUABoAG8AdABvAHMAaABvAHAAAAATAEEAZABvAGIAZQAgAFAAaABvAHQAbwBzAGgAbwBwACAAQwBTADYAAAABADhCSU0EBgAAAAAABwAIAAAAAQEA/+EMt2h0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC8APD94cGFja2V0IGJlZ2luPSLvu78iIGlkPSJXNU0wTXBDZWhpSHpyZVN6TlRjemtjOWQiPz4gPHg6eG1wbWV0YSB4bWxuczp4PSJhZG9iZTpuczptZXRhLyIgeDp4bXB0az0iQWRvYmUgWE1QIENvcmUgNS4zLWMwMTEgNjYuMTQ1NjYxLCAyMDEyLzAyLzA2LTE0OjU2OjI3ICAgICAgICAiPiA8cmRmOlJERiB4bWxuczpyZGY9Imh0dHA6Ly93d3cudzMub3JnLzE5OTkvMDIvMjItcmRmLXN5bnRheC1ucyMiPiA8cmRmOkRlc2NyaXB0aW9uIHJkZjphYm91dD0iIiB4bWxuczp4bXBNTT0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wL21tLyIgeG1sbnM6c3RFdnQ9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9zVHlwZS9SZXNvdXJjZUV2ZW50IyIgeG1sbnM6ZGM9Imh0dHA6Ly9wdXJsLm9yZy9kYy9lbGVtZW50cy8xLjEvIiB4bWxuczpwaG90b3Nob3A9Imh0dHA6Ly9ucy5hZG9iZS5jb20vcGhvdG9zaG9wLzEuMC8iIHhtbG5zOnhtcD0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wLyIgeG1wTU06RG9jdW1lbnRJRD0iMkRFMjIzMzhFMDVENzhCRkY4RDk0REM3MjM4REIxN0IiIHhtcE1NOkluc3RhbmNlSUQ9InhtcC5paWQ6Rjc3RjExNzQwNzIwNjgxMTgyMkFFRjA1QjdFMTM0RDUiIHhtcE1NOk9yaWdpbmFsRG9jdW1lbnRJRD0iMkRFMjIzMzhFMDVENzhCRkY4RDk0REM3MjM4REIxN0IiIGRjOmZvcm1hdD0iaW1hZ2UvanBlZyIgcGhvdG9zaG9wOkNvbG9yTW9kZT0iMyIgeG1wOkNyZWF0ZURhdGU9IjIwMTMtMDctMjZUMTE6MzE6MDItMDM6MDAiIHhtcDpNb2RpZnlEYXRlPSIyMDEzLTA3LTI2VDEyOjMwOjU5LTAzOjAwIiB4bXA6TWV0YWRhdGFEYXRlPSIyMDEzLTA3LTI2VDEyOjMwOjU5LTAzOjAwIj4gPHhtcE1NOkhpc3Rvcnk+IDxyZGY6U2VxPiA8cmRmOmxpIHN0RXZ0OmFjdGlvbj0ic2F2ZWQiIHN0RXZ0Omluc3RhbmNlSUQ9InhtcC5paWQ6Rjc3RjExNzQwNzIwNjgxMTgyMkFFRjA1QjdFMTM0RDUiIHN0RXZ0OndoZW49IjIwMTMtMDctMjZUMTI6MzA6NTktMDM6MDAiIHN0RXZ0OnNvZnR3YXJlQWdlbnQ9IkFkb2JlIFBob3Rvc2hvcCBDUzYgKE1hY2ludG9zaCkiIHN0RXZ0OmNoYW5nZWQ9Ii8iLz4gPC9yZGY6U2VxPiA8L3htcE1NOkhpc3Rvcnk+IDwvcmRmOkRlc2NyaXB0aW9uPiA8L3JkZjpSREY+IDwveDp4bXBtZXRhPiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDw/eHBhY2tldCBlbmQ9InciPz7/7gAOQWRvYmUAZEAAAAAB/9sAhAABAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAgICAgICAgICAgIDAwMDAwMDAwMDAQEBAQEBAQEBAQECAgECAgMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwP/wAARCADIAMgDAREAAhEBAxEB/90ABAAZ/8QBogAAAAYCAwEAAAAAAAAAAAAABwgGBQQJAwoCAQALAQAABgMBAQEAAAAAAAAAAAAGBQQDBwIIAQkACgsQAAIBAwQBAwMCAwMDAgYJdQECAwQRBRIGIQcTIgAIMRRBMiMVCVFCFmEkMxdScYEYYpElQ6Gx8CY0cgoZwdE1J+FTNoLxkqJEVHNFRjdHYyhVVlcassLS4vJkg3SThGWjs8PT4yk4ZvN1Kjk6SElKWFlaZ2hpanZ3eHl6hYaHiImKlJWWl5iZmqSlpqeoqaq0tba3uLm6xMXGx8jJytTV1tfY2drk5ebn6Onq9PX29/j5+hEAAgEDAgQEAwUEBAQGBgVtAQIDEQQhEgUxBgAiE0FRBzJhFHEIQoEjkRVSoWIWMwmxJMHRQ3LwF+GCNCWSUxhjRPGisiY1GVQ2RWQnCnODk0Z0wtLi8lVldVY3hIWjs8PT4/MpGpSktMTU5PSVpbXF1eX1KEdXZjh2hpamtsbW5vZnd4eXp7fH1+f3SFhoeIiYqLjI2Oj4OUlZaXmJmam5ydnp+So6SlpqeoqaqrrK2ur6/9oADAMBAAIRAxEAPwDdw9+691737r3Xvfuvde9+691737r3Xvfuvde9+691737r3Xvfuvde9+691737r3Xvfuvde9+691737r3Xvfuvde9+691737r3Xvfuvde9+691737r3Xvfuvde9+691737r3Xvfuvde9+691//0N3D37r3Xvfuvde9+691737r3Xvfuvde9+691737r3Xvfuvde9+691737r3Xvfuvde9+691737r3Xvfuvde9+691737r3Xvfuvde9+691737r3Xvfuvde9+691737r3Xvfuvde9+691737r3X//R3cPfuvde9+691737r3Xvfuvde9+691737r3Xvfuvde9+691737r3Xvfuvde9+691737r3Xvfuvde9+691737r3Xvfuvde9+691737r3Xvfuvde9+691737r3Xvfuvde9+691737r3Xvfuvdf/9Ldw9+691737r3Xvfuvde9+691737r3Xvfuvde9+691737r3Xvfuvde9+691737r3Xvfuvde9+691737r3Xvfuvde9+691737r3Xvfuvde9+691737r3Xvfuvde9+691737r3Xvfuvde9+691//093D37r3Xvfuvde9+691737r3Xvfuvde9+691737r3Xvfuvde9+691737r3UasraPHUlRX5CrpqChpInnqqysnipaSmgjGqSaoqJ3SGGJFFyzEAD6+/de6KrvP5tfHvZ08tIm66rdtbAxWWDZuNly8Fx9DFl53oMDUq1vrFVuP629+690FH/AA5B0/5tP9zeyvt7/wCd+x2v5rX/AOOH96dF7f8ANz37r3Qr7M+bXx73jPFSPuup2lWzsFjg3ljZcPBc2uZcvA9fgaYKTyZatB/S/v3XujVUdbR5Glp67H1dNXUNXEk9LWUc8VVS1MEg1RzU9RA7wzROpuGUkEfT37r3Un37r3Xvfuvde9+691737r3Xvfuvde9+691737r3Xvfuvde9+691737r3X//1N3D37r3Xvfuvde9+691737r3Xvfuvde9+691737r3Xvfuvde9+690Dvdnduz+jNoybm3RK1RV1LSU239vUkka5PcGSRA329MHuIKSnDq1TUsDHAhHDO0cb+691RZ3N8huye78pLPunLSUmAjnMmL2hi5Jafb+NRT+yzU4bVkq5B9ampMktyQmhLIPde6Az37r3Xvfuvde9+690OfTPyG7I6QykU+1stJV4CScSZTaGUllqNv5JGP7zJTlr42vdfpU05jluAH1pdD7r3V6fSfdmz+89ox7m2vK1PV0zR0u4NvVciNk9v5J0LfbVISwnpJwrNTVKgJOgPCuskae690MXv3Xuve/de697917r3v3Xuve/de697917r3v3Xuve/de697917r//V3cPfuvde9+691737r3Xvfuvde9+691737r3Xvfuvde9+691ByeSocNjchmMpUxUWMxVDV5LI1kx0w0lDQwSVVXUysASI4IImZj/Qe/de610O/e5Mx3f2Llt21zzw4eKSTHbUxEjejD7eglc0kJjUlBW1dzPVOCdU7kA6FRV917oFPfuvde9+691737r3Xvfuvde9+690NfQXcmY6Q7FxO7aF55sPLJHjt14iNvRmNvTyoauERsQhraSwnpXJGmdACdDOre691sX4zJUOZxuPzGLqYq3GZWhpMljqyE6oauhroI6qkqYmIBMc8Eqsp/offuvdTvfuvde9+691737r3Xvfuvde9+691737r3Xvfuvde9+691//1t3D37r3Xvfuvde9+691737r3Xvfuvde9+691737r3XvfuvdEz+d+9Z9pdBZPHUcrQ1e985itpB4zaRaGVarMZS3/NqoocO1PJ/tM9vz7917qiP37r3Xvfuvde9+691737r3Xvfuvde9+691737r3V7nwQ3rPu3oLGY6slaar2RnMrtIvIbyNQxLS5jF3/5tU9DmFp4/9pgt+PfuvdHM9+691737r3Xvfuvde9+691737r3Xvfuvde9+691737r3X//X3cPfuvde9+691737r3Xvfuvde9+691737r3Xvfuvde9+691Wn/MpaYbL6yRSfA26My0gubeZMTCIDb6EhHk9+691UH7917r3v3Xuve/de697917r3v3Xuve/de697917q3z+Ws0x2X2ajE+Bd0YZoxc28z4mYTm30BKJH7917qyz37r3Xvfuvde9+691737r3Xvfuvde9+691737r3Xvfuvdf//Q3cPfuvde9+691737r3Xvfuvde9+691737r3Xvfuvde9+690SL5+7Pn3J0RJmaSJpJ9kbnxG4JvGuqT+G1KVWArQBy3jjky8UzkfpWEk8A+/de6o09+691737r3Xvfuvde9+691737r3Xvfuvde9+691eX8Atnz7b6IjzNXE0c+99z5fcEPkXTJ/DaZKXAUQI4bxySYiWZCf1LMCOCPfuvdHd9+691737r3Xvfuvde9+691737r3Xvfuvde9+691737r3X//R3cPfuvde9+691737r3Xvfuvde9+691737r3Xvfuvde9+690zbiwGL3VgcztrN0y1mHz+LrsPk6ZuPNRZCmkpalFaxKOYpTpYcq1iOR7917rW97g6uzvT2/8AO7Gzsbs+OqGlxWRMZjhzWDqHdsXl6b6oUqoFtIqlvFOskROpD7917oMvfuvde9+691737r3Xvfuvde9+690JvT/V2d7h3/gtjYKN1fI1Cy5XIiMyQ4XB07o2Uy9T9ECUsDWjVivlnaOIHU49+691shbdwGL2rgcNtrCUy0eHwGLocPjKZefDRY+mjpaZGawLuIohqY8s1yeT7917p59+691737r3Xvfuvde9+691737r3Xvfuvde9+691737r3X/0t3D37r3Xvfuvde9+691737r3Xvfuvde9+691737r3WKaeGmhkqKiaKnghRpJp5pEihijQXaSSRyqIigXJJAHv3Xugrre+ek8dVihrO2uu4asyLE0H978FI8UjMECT+Kudacgtz5CthyePfuvdIz5BfH/afyG2jTwyVNPj9y46B6vaG7qZUqVhFTGsv2dWYW/wAvwORsrMqtdDaSM3BV/de6oq7N6n351DuCXbu+sFU4up1yfY16q82HzNPGQPvcNkwiwV1OwZSQLSxFtMqI91HuvdBx7917r3v3Xuve/de6EfrLqffnb24Itu7FwVTlKnXH99Xsrw4fDU8hI+9zOTKNBQ06hWIBvLKV0xI72U+691er8ffj/tP487RqIY6mnyG5cjAlXu/d1SqUyzCmjaX7OkMzf5BgcddmVWa7m8khuQqe690s6LvnpPI1ZoaPtrruarEjRLB/e/BRvLIrFCkHlrkWoJK8eMtccjj37r3QqQzw1MMdRTzRVEEyLJDPDIksMsbi6yRyIWR0YG4IJB9+691l9+691737r3Xvfuvde9+691737r3Xvfuvde9+691//9Pdw9+691737r3Xvfuvde9+691737r3XvfuvdAL8gO/9rdB7UXM5df4puHKman2vtiCdYarL1cKqZZ55Csho8TQ+RTUT6W06lRQzuqn3XuqM+1u+Ozu5clNWbz3HVTY4zGSi2zj5JaHbOMXVqjSkxMcrRSyRCw885mqWAGqQ2Hv3Xugd9+690Z3or5W9k9ISwY2kqf70bJ8uqo2fmaiU09OjNqlfAV9pqjBVDEk2RZKZmYs8LNZh7r3VrWy/kD8d/khg12zmZMGa2vVBU7E7ApqGCrNUVKqcU9Yz4/J1CMWMUlHM1Sg9RWM8D3Xug63n/Lz6az08tXtfKbp2PLIxZaKkrIc5hor8/t02Yjkyg9X4++0gcAfS3uvdBR/w2bT+W/+mefwX/zf+j6Py2v/AMdv76aL2/5t+/de6FfZn8vPprAzxVe6MpunfEsbBmoqushweGltz+5TYeOPKH1fj77SRwR9b+690Iu9PkD8d/jfg22zhpMGK2gVxTbE6/pqGerFUFCscq9GyY/GVDsFMslZMtS49QWQ8H3Xuqpe9flb2T3fLPjaup/uvsny6qfZ+GqJRT1CK2qJ8/X2hqM7UKQDZ1jplZQyQq12PuvdFi9+690MXVPfHZ3TWShrNmbjq4scJlkrds5CSWu2zk11apEqsTJIIopJRceeAw1KgnTILn37r3V5nx/7+2t35tRsziF/he4MWYafdG2J51mqsRVyqxingl0xmsxNd42NPPpXVpZGCujKPde6Hr37r3Xvfuvde9+691737r3Xvfuvde9+691//9Tdw9+691737r3Xvfuvde9+691737r3UHJ5Khw2NyGXydTHR43FUNXkshWTHTDS0NDBJVVdTKbG0cFPEzMf6D37r3WuJ3p21lu6eyM7vXIvNHQzTNQ7bxsrXXD7cpJZBjKFVBZFmZHM1QV4eplkYWBAHuvdBB7917r3v3Xuve/de697917oXdpd+dz7Ghiptr9lbsx1FAAsGOlykuTxcCgWAgxWV++x0It/qYhf37r3Qof7O38mvD4v9I6X+nm/ufsXzWta2r+7Om/+NtX+Pv3Xugv3b353PvqGWm3R2VuzI0U6lZ8dHlJcbip1YWKz4rFfY46YW/1URt7917oIvfuvde9+691737r3XvfuvdC/0X21luluyMFvXHPNJQwzLQ7kxsTWXMbcq5Ixk6FlJCNMqIJqctwlTFGx4BB917rY7xmSoczjcfl8ZUx1mNytDSZLH1kJ1Q1VDXQR1VJUxGwvHPTyqyn+h9+691O9+691737r3Xvfuvde9+691737r3X/1d3D37r3Xvfuvde9+691737r3XvfuvdEv+d2/pNm9FV+Ho5zDkt/Zai2shjNpVxemXJ5tx+DDNR0IpJPr6ar/Yj3XuqJ/fuvde9+691737r3Xvfuvde9+691737r3Xvfuvde9+691737r3Xvfuvde9+691737r3XvfuvdXsfBHf0m8uiqDD1k5myWwctW7WcyG8rYvTFk8I5/Ahho640kf09NL/sT7r3R0Pfuvde9+691737r3Xvfuvde9+691//1t3D37r3Xvfuvde9+691737r3XvfuvdVD/zKNyPPu/rXaKyER4rbeW3HLEDYM+fya4yGRx/aKLttwt/pqa31Pv3Xuqz/AH7r3Xvfuvde9+691737r3Xvfuvde9+691737r3Xvfuvde9+691737r3Xvfuvde9+691737r3VmH8tfcjQbv7K2i0hKZXbeJ3HFETwr4DJvjJnQH6F13JGGt9dK3+g9+691bx7917r3v3Xuve/de697917r3v3Xuv//X3cPfuvde9+691737r3Xvfuvde9+691RX8+8k1d8hsnSliww21dr41Af7Cy0s2XKj/AtlSf8AY+/de6JX7917r3v3Xuve/de697917r3v3Xuve/de697917r3v3Xuve/de697917r3v3Xuve/de697917o6nwEyTUPyGxlKGKjM7V3RjXA/trFSw5cKf8A2KB/wBh7917q9T37r3Xvfuvde9+691737r3Xvfuvdf/0N3D37r3Xvfuvde9+691737r3XvfuvdUEfN9mb5M9hhr2SDZypf6BTsfbjnT/hrY/wCx9+690U737r3Xvfuvde9+691737r3Xvfuvde9+691737r3Xvfuvde9+691737r3Xvfuvde9+691737r3RsfhAzL8mevAt7PBvFXt9Co2PuNxq/wANaj/Y+/de6v39+691737r3Xvfuvde9+691737r3X/0d3D37r3Xvfuvde9+691737r3XvfuvdUlfzC9n1OE7poN1eJv4fvXa+PmjqdNkfKYC+Hr6YG1menoI6Jz/hMPfuvdEM9+691737r3Xvfuvde9+691737r3Xvfuvde9+691737r3Xvfuvde9+691737r3Xvfuvde9+690fP8Al6bPqc33TX7q8Tfw/ZW18hNJU6bomUz9sPQUxNrK9RQSVrj/AAhPv3Xurtffuvde9+691737r3Xvfuvde9+691//0t3D37r3Xvfuvde9+691737r3XvfuvdAB8j+jcf3x17UbcaWCg3Ji5my20sxMpMdFlkiaN6SsaNWm/heUhPinChip0ShWaJVPuvda/8AvPZW6evtw121t44WtwWcx0hSejrY9Pkj1MsdVSTrqgraGo0kxTxM8Ui8qxHv3Xukt7917r3v3Xuve/de697917r3v3Xuve/de697917r3v3Xuve/de697917r3v3XulTszZW6ewdw0O1tnYWtzucyMgSCjoo9Xjj1KslVVztpgoqGn1AyzyskUa8swHv3XutgD44dG4/ofr2n24ssFfuTKTLlt25iFSI63LPEsaUlG0irN/C8XCPFAGCljrlKq0rKPde6H/37r3Xvfuvde9+691737r3Xvfuvdf/093D37r3Xvfuvde9+691737r3Xvfuvde9+690hd99ZbB7Nxq4nfm1MRuWjj1/bmvpyKyiaQASPjsnTtBksbI4FmaCWNiOCffuvdFGzf8u/o7JTvPi8rv3bqsSVo6HM4uuoowTfSgy+Er682/Baobj639+690nv8Aht3qv/nuuwP+S9uf/WT37r3Xv+G3eq/+e67A/wCS9uf/AFk9+6917/ht3qv/AJ7rsD/kvbn/ANZPfuvde/4bd6r/AOe67A/5L25/9ZPfuvde/wCG3eq/+e67A/5L25/9ZPfuvde/4bd6r/57rsD/AJL25/8AWT37r3Xv+G3eq/8AnuuwP+S9uf8A1k9+6917/ht3qv8A57rsD/kvbn/1k9+6917/AIbd6r/57rsD/kvbn/1k9+690ocJ/Lv6Oxs6T5TK793EqsC1HXZrGUNDIAQdLjEYSgrxf6ErULx9LfX37r3RudidZbB6yxrYnYe1MRtqjk0fcGgpyaytaMERvkcnUNPkslIgNlaeWRgOAffuvdLr37r3Xvfuvde9+691737r3Xvfuvde9+691//U3cPfuvde9+691737r3Xvfuvde9+691737r3Xvfuvde9+691737r3Xvfuvde9+691737r3Xvfuvde9+691737r3Xvfuvde9+691737r3Xvfuvde9+691737r3Xvfuvde9+691737r3Xvfuvdf/9Xdw9+691737r3Xvfuvde9+691737r3Xvfuvde9+691737r3Xvfuvde9+691737r3Xvfuvde9+691737r3Xvfuvde9+691737r3Xvfuvde9+691737r3Xvfuvde9+691737r3Xvfuvde9+691//1t3D37r3Xvfuvde9+691737r3Xvfuvde9+691737r3Xvfuvde9+691737r3Xvfuvde9+691737r3Xvfuvde9+691737r3Xvfuvde9+691737r3Xvfuvde9+691737r3Xvfuvde9+691737r3X//X3cPfuvde9+691737r3Xvfuvde9+691737r3Xvfuvde9+691737r3Xvfuvde9+691737r3Xvfuvde9+691737r3Xvfuvde9+691737r3Xvfuvde9+691737r3Xvfuvde9+691737r3Xvfuvdf/9k=";
            try
            {
                if (IsGuid(loggedInUserId.ToString()))
                {
                    ISession session = EntitySessionManager.GetSession();
                    InternalUser internalUser = session.Query<InternalUser>()
                        .FirstOrDefault(a => a.Id == loggedInUserId.ToString());
                    if (internalUser != null)
                    {
                        var imageId = internalUser.ProfilePicture;
                        if (IsGuid(imageId))
                        {
                            var imageIdGuid = Guid.Parse(imageId);
                            Image image = session.Query<Image>()
                                .FirstOrDefault(a => a.Id == imageIdGuid);
                            if (image != null)
                            {
                                Response = Convert.ToBase64String(image.ImageByte);
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }

            return Response;
        }

        internal string getProductTypeById(Guid productType)
        {
            string Response = "NA";

            try
            {
                if (IsGuid(productType.ToString()))
                {
                    ISession session = EntitySessionManager.GetSession();
                    ProductType productTypes = session.Query<ProductType>()
                        .Where(a => a.Id == productType).FirstOrDefault();
                    if (productTypes != null)
                    {
                        Response = productTypes.Type;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        internal string GetClaimStatusNameById(Guid statusId)
        {
            string Response = "NA";

            try
            {
                if (IsGuid(statusId.ToString()))
                {
                    ISession session = EntitySessionManager.GetSession();
                    ClaimStatusCode claimStatus = session.Query<ClaimStatusCode>()
                        .Where(a => a.Id == statusId).FirstOrDefault();
                    if (claimStatus != null)
                    {
                        Response = claimStatus.Description;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        internal string GetProductCodeById(Guid productId)
        {
            String Response = String.Empty;

            try
            {
                if (IsGuid(productId.ToString()))
                {
                    ISession session = EntitySessionManager.GetSession();
                    Product product = session.Query<Product>()
                        .FirstOrDefault(a => a.Id == productId);
                    if (product != null)
                    {
                        Response = product.Productcode;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        internal string GetOccupationById(Guid OccupationId)
        {
            String Response = String.Empty;

            try
            {
                if (IsGuid(OccupationId.ToString()))
                {
                    ISession session = EntitySessionManager.GetSession();
                    Occupation occupation = session.Query<Occupation>()
                        .FirstOrDefault(a => a.Id == OccupationId);
                    if (occupation != null)
                    {
                        Response = occupation.Name;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        internal string getTitleById(Guid TitleId)
        {
            String Response = String.Empty;

            try
            {
                if (IsGuid(TitleId.ToString()))
                {
                    ISession session = EntitySessionManager.GetSession();
                    Title occupation = session.Query<Title>()
                        .FirstOrDefault(a => a.Id == TitleId);
                    if (occupation != null)
                    {
                        Response = occupation.Name;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        public List<InvoiceCodeTireDetails> GetAllInvoiceCodeTireDetails()
        {
            List<InvoiceCodeTireDetails> entities = null;
            ISession session = EntitySessionManager.GetSession();
            IQueryable<InvoiceCodeTireDetails> invoiceCodeTireDetailsData = session.Query<InvoiceCodeTireDetails>();
            entities = invoiceCodeTireDetailsData.ToList();
            return entities;
        }

        public List<AvailableTireSizes> GetAllAvailableTireSizes()
        {
            List<AvailableTireSizes> entities = null;
            ISession session = EntitySessionManager.GetSession();
            IQueryable<AvailableTireSizes> AvailableTireSizesData = session.Query<AvailableTireSizes>();
            entities = AvailableTireSizesData.ToList();
            return entities;
        }

        public List<InvoiceCodeDetails> GetAllInvoiceCodeDetails()
        {
            List<InvoiceCodeDetails> entities = null;
            ISession session = EntitySessionManager.GetSession();
            IQueryable<InvoiceCodeDetails> InvoiceCodeDetailsData = session.Query<InvoiceCodeDetails>();
            entities = InvoiceCodeDetailsData.ToList();
            return entities;
        }

        internal string GetAdditionalMakeNameById(Guid Id)
        {
            String Response = String.Empty;

            try
            {
                if (IsGuid(Id.ToString()))
                {
                    ISession session = EntitySessionManager.GetSession();
                    AdditionalPolicyMakeData model = session.Query<AdditionalPolicyMakeData>()
                        .Where(a => a.Id == Id).FirstOrDefault();
                    if (model != null)
                    {
                        Response = model.MakeName;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        internal string GetAdditionalModelNameById(Guid Id)
        {
            String Response = String.Empty;

            try
            {
                if (IsGuid(Id.ToString()))
                {
                    ISession session = EntitySessionManager.GetSession();
                    AdditionalPolicyModelData model = session.Query<AdditionalPolicyModelData>()
                        .Where(a => a.Id == Id).FirstOrDefault();
                    if (model != null)
                    {
                        Response = model.ModelName;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        internal ClaimInforResponseDto GetClaimInformation(Guid policyId, string serialNo, string position)
        {
            ClaimInforResponseDto claimInforResponseDto = new ClaimInforResponseDto();
            try
            {

                    ISession session = EntitySessionManager.GetSession();
                claimInforResponseDto = session.Query<ClaimSubmission>()
                .Join(session.Query<ClaimSubmissionItem>(), b => b.Id, c => c.ClaimSubmissionId, (b, c) => new { b, c })
                .Where(w => w.b.PolicyId == policyId && w.c.ItemCode.ToLower() == serialNo.ToLower() && w.c.ItemName.Substring(0, 2) == position)
                .OrderByDescending(ob=> ob.b.EntryDate)
                .Select(xy => new ClaimInforResponseDto
                {
                    LastClaimSubmit = xy.b.EntryDate,
                    NoOfSubmissions = 1,
                    LastClaimApproved = GetClaimIsApprovedByClaimSubmitId(xy.b.Id, xy.b.PolicyId),
                    SubmittedUser = GetClaimSubmitedUserBySubmitionId(xy.b.Id)

                }).FirstOrDefault();

                    }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return claimInforResponseDto;
        }

        private bool GetClaimIsApprovedByClaimSubmitId(Guid ClaimSubmissionId, Guid policyId)
        {
            bool approved = false;
            try
            {

                ISession session = EntitySessionManager.GetSession();
                approved = session.Query<Claim>().Where(a => a.PolicyId == policyId && a.Id == ClaimSubmissionId).Select(a => a.IsApproved).FirstOrDefault();
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return approved;
        }

        private string GetClaimSubmitedUserBySubmitionId(Guid ClaimSubmissionId)
        {
            String submittedUser = String.Empty;
            try
            {

                ISession session = EntitySessionManager.GetSession();
                submittedUser = session.Query<ClaimSubmission>()
                    .Where(a => a.Id == ClaimSubmissionId)
                    .Join(session.Query<InternalUser>(), b => b.ClaimSubmittedBy.ToString().ToLower(), c => c.Id.ToLower(), (b, c) => new { b, c })
                    .Select(s => s.c.FirstName + ' ' + s.c.LastName)
                    .FirstOrDefault();
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return submittedUser;

        }
    }
}
