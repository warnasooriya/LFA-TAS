
using Newtonsoft.Json.Linq;
using NLog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;
using TAS.DataTransfer.Requests;
using TAS.DataTransfer.Responses;
using TAS.Services;
using TAS.Services.Common.Transformer;
using TAS.Web.Common;

namespace TAS.Web.Controllers
{
    public class PolicyRegController : ApiController
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        #region Policy
        [HttpPost]
        public string AddPolicy(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

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
                            DealerPrice = NewPolicy.Vehicle.DealerPrice,
                            VINNo = NewPolicy.Vehicle.VINNo,
                            Variant = NewPolicy.Vehicle.Variant,
                            TransmissionId = NewPolicy.Vehicle.TransmissionId,
                            GrossWeight = NewPolicy.Vehicle.GrossWeight
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
                            DealerPrice = NewPolicy.Vehicle.DealerPrice,
                            VINNo = NewPolicy.Vehicle.VINNo,
                            Variant = NewPolicy.Vehicle.Variant,
                            TransmissionId = NewPolicy.Vehicle.TransmissionId,
                            EntryDateTime = NewPolicy.Vehicle.EntryDateTime,
                            EntryUser = NewPolicy.Vehicle.EntryUser,
                            GrossWeight = NewPolicy.Vehicle.GrossWeight
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
                            UserName = NewPolicy.Customer.UserName,

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
                if (resultC.CustomerInsertion && (resultV.VehicleDetailsInsertion || resultB.BrownAndWhiteDetailsInsertion))
                {
                    PolicyBundleRequestDto Policy = new PolicyBundleRequestDto()
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
                        EntryUser = NewPolicy.EntryUser,
                        PolicyStartDate = NewPolicy.PolicyStartDate,
                        PolicyEndDate = NewPolicy.PolicyEndDate,
                        Discount = NewPolicy.Discount,
                        BookletNumber = NewPolicy.BookletNumber,
                        AttributeSpecificationId = NewPolicy.ContractProducts.First().AttributeSpecificationId,
                        MWStartDate = NewPolicy.MWStartDate,
                        MWIsAvailable = NewPolicy.MWIsAvailable,
                    };

                    IPolicyManagementService PolicyManagementService = ServiceFactory.GetPolicyManagementService();
                    PolicyBundleRequestDto result = PolicyManagementService.AddPolicyBundle(Policy, SecurityHelper.Context, AuditHelper.Context);

                    foreach (var item in NewPolicy.ContractProducts)
                    {
                        PolicyRequestDto P = new PolicyRequestDto()
                        {
                            Id = NewPolicy.Id,
                            CommodityTypeId = NewPolicy.CommodityTypeId,
                            ProductId = item.ProductId,
                            DealerId = NewPolicy.DealerId,
                            DealerLocationId = NewPolicy.DealerLocationId,
                            ContractId = item.ContractId,
                            ExtensionTypeId = item.ExtensionTypeId,
                            Premium = item.Premium,
                            PremiumCurrencyTypeId = item.PremiumCurrencyTypeId,
                            DealerPaymentCurrencyTypeId = NewPolicy.DealerPaymentCurrencyTypeId,
                            CustomerPaymentCurrencyTypeId = NewPolicy.CustomerPaymentCurrencyTypeId,
                            CoverTypeId = item.CoverTypeId,
                            HrsUsedAtPolicySale = NewPolicy.HrsUsedAtPolicySale,
                            IsPreWarrantyCheck = NewPolicy.IsPreWarrantyCheck,
                            PolicySoldDate = NewPolicy.PolicySoldDate,
                            SalesPersonId = NewPolicy.SalesPersonId,
                            PolicyNo = item.PolicyNo,
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
                            EntryUser = NewPolicy.EntryUser,
                            PolicyStartDate = NewPolicy.PolicyStartDate,
                            PolicyEndDate = NewPolicy.PolicyEndDate,
                            Discount = NewPolicy.Discount,
                            PolicyBundleId = result.Id,
                            BookletNumber = NewPolicy.BookletNumber,
                            MWStartDate = NewPolicy.MWStartDate,
                            MWIsAvailable = NewPolicy.MWIsAvailable,
                            AttributeSpecificationId = NewPolicy.ContractProducts.First().AttributeSpecificationId

                            // Co_coustomerID = NewPolicy.CoCustomerID
                        };
                        PolicyRequestDto res = PolicyManagementService.AddPolicy(P, SecurityHelper.Context, AuditHelper.Context);

                    }
                    IDealerManagementService dealer = ServiceFactory.GetDealerManagementService();
                    if (dealer.GetDealerById(NewPolicy.DealerId, SecurityHelper.Context, AuditHelper.Context).IsAutoApproval)
                    {
                        ApprovePolicy(data);
                    }
                    logger.Info("Policy Added");
                    if (result.PolicyBundleInsertion)
                    {
                        return "OK";
                    }
                    else
                    {
                        return "Add Policy failed!";
                    }
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
        [HttpPost]
        public string SavePolicy(JObject obj)
        {
            SavePolicyRequestDto SavePolicyRequest = obj.ToObject<SavePolicyRequestDto>();
            IPolicyManagementService PolicyManagementService = ServiceFactory.GetPolicyManagementService();
            return PolicyManagementService.SavePolicy(SavePolicyRequest, SecurityHelper.Context, AuditHelper.Context);
        }

        [HttpPost]
        public object ManufacturerWarrentyAvailabilityCheckOnPolicySave(JObject obj)
        {
            ManufacturerWarrentyAvailabilityCheckDto PolicyDetails = obj.ToObject<ManufacturerWarrentyAvailabilityCheckDto>();
            IPolicyManagementService PolicyManagementService = ServiceFactory.GetPolicyManagementService();
            return PolicyManagementService.ManufacturerWarrentyAvailabilityCheckOnPolicySave(PolicyDetails, SecurityHelper.Context, AuditHelper.Context);
        }

        [HttpPost]
        public object GetDocumentTypesByPageName(JObject data)
        {
            String PageName = data["PageName"].ToString();
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            IPolicyManagementService PolicyManagementService = ServiceFactory.GetPolicyManagementService();
            return PolicyManagementService.GetDocumentTypesByPageName(PageName, SecurityHelper.Context, AuditHelper.Context);

        }

        [HttpPost]
        public EligibilityCheckResponse EligibilityCheckRequest(EligibilityCheckRequestDto EligibilityCheckRequestDto)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            IPolicyManagementService PolicyManagementService = ServiceFactory.GetPolicyManagementService();
            return PolicyManagementService.EligibilityCheckRequest(EligibilityCheckRequestDto.eligibilityCheckRequest, SecurityHelper.Context, AuditHelper.Context);

        }

        [HttpPost]
        public string UpdatePolicyV2(JObject obj)
        {
            SavePolicyRequestDto SavePolicyRequest = obj.ToObject<SavePolicyRequestDto>();
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            IPolicyManagementService PolicyManagementService = ServiceFactory.GetPolicyManagementService();
            return PolicyManagementService.UpdatePolicyV2(SavePolicyRequest, SecurityHelper.Context, AuditHelper.Context);

        }

        [HttpPost]
        public SerialNumberCheckResponseDto SerialNumberCheck(JObject data)
        {
            String SerialNumber = data["SerialNumber"].ToString();
            String CommodityCode = data["CommodityCode"].ToString();
            Guid LoggedInUserId = Guid.Parse(data["LoggedInUserId"].ToString());
            Guid DealerId = Guid.Parse(data["DealerId"].ToString());


            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            IPolicyManagementService PolicyManagementService = ServiceFactory.GetPolicyManagementService();
            return PolicyManagementService.SerialNumberCheck(SerialNumber, CommodityCode, LoggedInUserId, DealerId, SecurityHelper.Context, AuditHelper.Context);

        }

        [HttpPost]
        public CustomerCheckResponseDto CheckCustomerById(JObject data)
        {
            Guid CustomerId = Guid.Parse(data["CustomerId"].ToString());
            Guid LoggedInUserId = Guid.Parse(data["LoggedInUserId"].ToString());
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            IPolicyManagementService PolicyManagementService = ServiceFactory.GetPolicyManagementService();
            return PolicyManagementService.CheckCustomerById(CustomerId, LoggedInUserId, SecurityHelper.Context, AuditHelper.Context);

        }
        [HttpPost]
        public object GetDealerAccessByUserId(JObject data)
        {
            Guid LoggedInUserId = Guid.Parse(data["Id"].ToString());
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            IPolicyManagementService PolicyManagementService = ServiceFactory.GetPolicyManagementService();
            return PolicyManagementService.GetDealerAccessByUserId(LoggedInUserId, SecurityHelper.Context, AuditHelper.Context);

        }

        [HttpPost]
        public string UpdatePolicy(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

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

                if (NewPolicy.ApprovedDate == null) {
                    NewPolicy.ApprovedDate = DateTime.UtcNow;
                }

                if (NewPolicy.Vehicle.VINNo != null)
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
                            DealerPrice = NewPolicy.Vehicle.DealerPrice,
                            VINNo = NewPolicy.Vehicle.VINNo,
                            Variant = NewPolicy.Vehicle.Variant,
                            TransmissionId = NewPolicy.Vehicle.TransmissionId,
                            DealerCurrencyId = NewPolicy.DealerPaymentCurrencyTypeId,
                            DealerId = NewPolicy.DealerId,
                            RegistrationDate = NewPolicy.Vehicle.RegistrationDate,
                            GrossWeight = NewPolicy.Vehicle.GrossWeight,
                            EngineNumber = NewPolicy.Vehicle.EngineNumber
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
                            DealerPrice = NewPolicy.Vehicle.DealerPrice,
                            VINNo = NewPolicy.Vehicle.VINNo,
                            Variant = NewPolicy.Vehicle.Variant,
                            TransmissionId = NewPolicy.Vehicle.TransmissionId,
                            EntryDateTime = NewPolicy.Vehicle.EntryDateTime,
                            EntryUser = NewPolicy.Vehicle.EntryUser,
                            DealerCurrencyId = NewPolicy.DealerPaymentCurrencyTypeId,
                            DealerId = NewPolicy.DealerId,
                            RegistrationDate = NewPolicy.Vehicle.RegistrationDate,
                            GrossWeight = NewPolicy.Vehicle.GrossWeight,
                            EngineNumber = NewPolicy.Vehicle.EngineNumber
                        };
                        resultV = VehicleDetailsManagementService.UpdateVehicleDetails(VehicleDetails, SecurityHelper.Context, AuditHelper.Context);
                        if (!resultV.VehicleDetailsInsertion)
                        {
                            return "Add Policy failed!";
                        }
                    }
                }
                if (NewPolicy.BAndW.SerialNo != null)
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
                            SerialNo = NewPolicy.BAndW.SerialNo,
                            DealerCurrencyId = NewPolicy.BAndW.DealerCurrencyId,
                            DealerId = NewPolicy.BAndW.DealerId,
                            CommodityUsageTypeId = NewPolicy.BAndW.CommodityUsageTypeId,
                            CountryId = NewPolicy.BAndW.CountryId,
                            currencyPeriodId = NewPolicy.BAndW.currencyPeriodId,


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
                            SerialNo = NewPolicy.BAndW.SerialNo,
                            DealerCurrencyId = NewPolicy.BAndW.DealerCurrencyId,
                            DealerId = NewPolicy.DealerId,
                            CommodityUsageTypeId = NewPolicy.BAndW.CommodityUsageTypeId,
                            CountryId = NewPolicy.BAndW.CountryId,
                            currencyPeriodId = NewPolicy.BAndW.currencyPeriodId,
                            CommodityTypeId = NewPolicy.CommodityTypeId,


                        };
                        resultB = BrownAndWhiteDetailsManagementService.UpdateBrownAndWhiteDetails(BrownAndWhiteDetails, SecurityHelper.Context, AuditHelper.Context);
                        if (!resultB.BrownAndWhiteDetailsInsertion)
                        {
                            return "Add Policy failed!";
                        }
                    }
                }
                if (NewPolicy.Customer.IDNo != null)
                {
                    if (NewPolicy.Customer.Id.ToString() == "00000000-0000-0000-0000-000000000000")
                    {
                        Customer = new CustomerRequestDto()
                        {
                            Address2 = NewPolicy.Customer.Address2,
                            Address1 = NewPolicy.Customer.Address1,
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

                PolicyBundleRequestDto Policy = new PolicyBundleRequestDto()
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
                    EntryUser = NewPolicy.EntryUser,
                    Discount = NewPolicy.Discount,
                    IsApproved = NewPolicy.IsApproved,
                    AttributeSpecificationId = NewPolicy.ContractProducts.First().AttributeSpecificationId,
                    BookletNumber = NewPolicy.BookletNumber,
                    MWStartDate = NewPolicy.MWStartDate,
                };

                IPolicyManagementService PolicyManagementService = ServiceFactory.GetPolicyManagementService();
                PolicyBundleRequestDto result = PolicyManagementService.UpdatePolicyBundle(Policy, SecurityHelper.Context, AuditHelper.Context);
                if (NewPolicy.UploadAttachments != null && NewPolicy.UploadAttachments.Count > 0)
                {
                    bool uploaded = PolicyManagementService.addPolicyAttachement(Policy.Id, NewPolicy.UploadAttachments, SecurityHelper.Context, AuditHelper.Context);
                    if (!uploaded)
                    {
                        return "Policy Attachment Fail !";
                    }
                }


                foreach (var item in NewPolicy.ContractProducts)
                {
                    PolicyRequestDto P = new PolicyRequestDto()
                    {
                        Id = item.Id,
                        CommodityTypeId = NewPolicy.CommodityTypeId,
                        ProductId = item.ProductId,
                        DealerId = NewPolicy.DealerId,
                        DealerLocationId = NewPolicy.DealerLocationId,
                        ContractId = item.ContractId,
                        ExtensionTypeId = item.ExtensionTypeId,
                        Premium = item.Premium,
                        PremiumCurrencyTypeId = item.PremiumCurrencyTypeId,
                        DealerPaymentCurrencyTypeId = NewPolicy.DealerPaymentCurrencyTypeId,
                        CustomerPaymentCurrencyTypeId = NewPolicy.CustomerPaymentCurrencyTypeId,
                        CoverTypeId = item.CoverTypeId,
                        HrsUsedAtPolicySale = NewPolicy.HrsUsedAtPolicySale,
                        IsPreWarrantyCheck = NewPolicy.IsPreWarrantyCheck,
                        PolicySoldDate = NewPolicy.PolicySoldDate,
                        SalesPersonId = NewPolicy.SalesPersonId,
                        PolicyNo = item.PolicyNo,
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
                        EntryUser = NewPolicy.EntryUser,
                        PolicyStartDate = item.PolicyStartDate,
                        PolicyEndDate = item.PolicyEndDate,
                        //PolicyEndDate = DBDTOTransformer.Instance.GetPolicyEndDate(NewPolicy.MWStartDate,
                        //                  NewPolicy.PolicySoldDate, NewPolicy.Vehicle.MakeId, NewPolicy.Vehicle.ModelId,
                        //                  NewPolicy.Vehicle.DealerId, NewPolicy.ContractProducts.First().ExtensionTypeId,
                        //                  NewPolicy.MWIsAvailable),
                        //PolicyStartDate = DBDTOTransformer.Instance.GetPolicyStartDate(NewPolicy.MWStartDate,
                        //                  NewPolicy.PolicySoldDate, NewPolicy.Vehicle.MakeId, NewPolicy.Vehicle.ModelId,
                        //                  NewPolicy.Vehicle.DealerId, NewPolicy.ContractProducts.First().ExtensionTypeId,
                        //                  NewPolicy.MWIsAvailable),
                        Discount = NewPolicy.Discount,
                        PolicyBundleId = result.Id,
                        IsApproved = true,
                        ApprovedDate = NewPolicy.ApprovedDate,
                        DealerPolicy = NewPolicy.DealerPolicy,
                        ContractInsuaranceLimitationId = NewPolicy.ContractProducts.First().AttributeSpecificationId,
                        MWStartDate = NewPolicy.MWStartDate,
                        BookletNumber = NewPolicy.BookletNumber,
                        MWIsAvailable = NewPolicy.MWIsAvailable
                    };
                    PolicyRequestDto res = PolicyManagementService.UpdatePolicy(P, SecurityHelper.Context, AuditHelper.Context);
                }

                logger.Info("Policy Added");
                if (result.PolicyBundleInsertion)
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

        [HttpPost]
        public object GetPoliciesForSearchGridInquiry(PolicySearchInquiryGridRequestDto PolicySearchInquiryGridRequestDto)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            IPolicyManagementService PolicyManagementService = ServiceFactory.GetPolicyManagementService();

            return PolicyManagementService.GetPoliciesForSearchGridInquiry(
                PolicySearchInquiryGridRequestDto,
                SecurityHelper.Context,
            AuditHelper.Context);
        }


        [HttpPost]
        public object GetPolicyById2(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            IVehicleDetailsManagementService VehicleDetailsManagementService = ServiceFactory.GetVehicleDetailsManagementService();
            VehicleDetailsResponseDto VehicleDetails = new VehicleDetailsResponseDto();

            IManufacturerWarrantyManagementService ManufacturerWarrantyManagementService = ServiceFactory.GetManufacturerWarrantyManagementService();
            IContractManagementService ContractManagementService = ServiceFactory.GetContractManagementService();
            ManufacturerWarrantyResponseDto ManufacturerWarrantyD = new ManufacturerWarrantyResponseDto();
            InsuaranceLimitationResponseDto InsuaranceLimitation = new InsuaranceLimitationResponseDto();
            IBrownAndWhiteDetailsManagementService BrownAndWhiteDetailsManagementService = ServiceFactory.GetBrownAndWhiteDetailsManagementService();
            IOtherItemManagementService otherItemManagementService = ServiceFactory.GetOtherItemManagementService();
            IYellowGoodManagementService yellowGoodManagementService = ServiceFactory.GetYellowGoodManagementService();

            BrownAndWhiteDetailsResponseDto BrownAndWhiteDetails = new BrownAndWhiteDetailsResponseDto();

            ICustomerManagementService customerManagementService = ServiceFactory.GetCustomerManagementService();
            CustomersResponseDto customerData = customerManagementService.GetCustomers(
            SecurityHelper.Context,
            AuditHelper.Context);

            IPolicyManagementService PolicyManagementService = ServiceFactory.GetPolicyManagementService();

            PolicyBundleResponseDto Policy = PolicyManagementService.GetPolicyBundleById(Guid.Parse(data["Id"].ToString()),
                SecurityHelper.Context,
                AuditHelper.Context);
            List<PolicyResponseDto> Bundle = PolicyManagementService.GetPolicys(SecurityHelper.Context,
                AuditHelper.Context).Policies.FindAll(p => p.PolicyBundleId == Policy.Id);
            List<PolicyContractProductResponseDto> retBundle = new List<PolicyContractProductResponseDto>();
            foreach (var item in Bundle)
            {
                PolicyContractProductResponseDto p = new PolicyContractProductResponseDto()
                {
                    ContractId = item.ContractId,
                    CoverTypeId = item.ContractExtensionPremiumId,
                    ExtensionTypeId = item.ExtensionTypeId,
                    AttributeSpecificationId = item.ContractInsuaranceLimitationId,
                    ContractExtensionsId = item.ContractExtensionsId,
                    Id = item.Id,
                    Premium = item.Premium,
                    PremiumCurrencyTypeId = item.PremiumCurrencyTypeId,
                    ProductId = item.ProductId,
                    PolicyNo = item.PolicyNo,
                    PolicyEndDate = item.PolicyEndDate,
                    PolicyStartDate = item.PolicyStartDate,
                    BookletNumber = item.BookletNumber,
                };
                retBundle.Add(p);
            }
            if (Bundle[0].Type == "Vehicle")
            {
                VehicleDetails = VehicleDetailsManagementService.GetVehicleDetailsById(Bundle[0].ItemId,
            SecurityHelper.Context,
            AuditHelper.Context);
            }
            else if (Bundle[0].Type == "B&W")
                BrownAndWhiteDetails = BrownAndWhiteDetailsManagementService.GetBrownAndWhiteDetailsById(Bundle[0].ItemId,
                    SecurityHelper.Context,
                    AuditHelper.Context);
            else if (Bundle[0].Type == "Yellow")
                BrownAndWhiteDetails = yellowGoodManagementService.GetYellowGoodDetaisById(Bundle[0].ItemId,
                   SecurityHelper.Context,
                   AuditHelper.Context);
            else
                BrownAndWhiteDetails = otherItemManagementService.GetOtherItemDetailsById(Bundle[0].ItemId,
                  SecurityHelper.Context,
                  AuditHelper.Context);


            ManufacturerWarrantyD = ManufacturerWarrantyManagementService.GetManufacturerDetailsByContractId(retBundle.FirstOrDefault().ContractId,
                VehicleDetails.ModelId, VehicleDetails.MakeId, SecurityHelper.Context,
                    AuditHelper.Context);
            InsuaranceLimitation = ContractManagementService.GetInsuaranceLimitationByContractInsuaranceLimitationId(Bundle[0].ContractExtensionsId,
                SecurityHelper.Context,
                    AuditHelper.Context);

            decimal PremiumData;

            if (Bundle[0].DealerPayment == 0 && Bundle[0].CustomerPayment == 0)
            {
                PremiumData = Bundle[0].Premium;
            }
            else
            {
                PremiumData = Bundle[0].DealerPayment + Bundle[0].CustomerPayment;
            }

            PolicyViewData result = new PolicyViewData()
            {
                //Id = Bundle[0].Id,
                Id = Policy.Id,
                CommodityTypeId = Bundle[0].CommodityTypeId,
                tpaBranchId = Bundle[0].tpaBranchId,

                ProductId = Bundle[0].ProductId,
                DealerId = Policy.DealerId,
                DealerLocationId = Bundle[0].DealerLocationId,
                ContractId = Bundle[0].ContractId,
                ExtensionTypeId = Bundle[0].ExtensionTypeId,
                //Premium = Bundle[0].DealerPayment + Bundle[0].CustomerPayment,
                Premium = PremiumData,
                PremiumCurrencyTypeId = Bundle[0].PremiumCurrencyTypeId,
                DealerPaymentCurrencyTypeId = Bundle[0].DealerPaymentCurrencyTypeId,
                CustomerPaymentCurrencyTypeId = Bundle[0].CustomerPaymentCurrencyTypeId,
                CoverTypeId = Bundle[0].CoverTypeId,
                HrsUsedAtPolicySale = Bundle[0].HrsUsedAtPolicySale,
                IsPreWarrantyCheck = Bundle[0].IsPreWarrantyCheck,
                PolicySoldDate = Bundle[0].PolicySoldDate,
                SalesPersonId = Bundle[0].SalesPersonId,
                PolicyNo = Bundle[0].PolicyNo,
                IsSpecialDeal = Bundle[0].IsSpecialDeal,
                IsPartialPayment = Bundle[0].IsPartialPayment,
                DealerPayment = Bundle[0].DealerPayment,
                CustomerPayment = Bundle[0].CustomerPayment,
                PaymentModeId = Bundle[0].PaymentModeId,
                PaymentTypeId = Bundle[0].PaymentTypeId,
                RefNo = Bundle[0].RefNo,
                Comment = Bundle[0].Comment,
                ItemId = Bundle[0].ItemId,
                IsApproved = Bundle[0].IsApproved,
                Type = Bundle[0].Type,
                CustomerId = Bundle[0].CustomerId,
                Vehicle = (VehicleDetails == null || VehicleDetails.Id == null) ? null : VehicleDetails,
                Customer = customerData.Customers.Find(c => c.Id == Bundle[0].CustomerId.ToString()),
                BAndW = (BrownAndWhiteDetails == null || BrownAndWhiteDetails.Id == null) ? null : BrownAndWhiteDetails,

                EntryDateTime = Bundle[0].EntryDateTime,
                EntryUser = Bundle[0].EntryUser,
                PolicyStartDate = ManufacturerWarrantyD == null ? Bundle[0].PolicyStartDate : Bundle[0].PolicyStartDate.AddDays(+1),
                PolicyEndDate = Bundle[0].PolicyEndDate,
                ContractProducts = retBundle,
                Discount = Bundle[0].Discount,
                DiscountPercentage = Bundle[0].DiscountPercentage,
                DealerPolicy = Bundle[0].DealerPolicy,
                MWEnddate = Bundle[0].PolicySoldDate.AddMonths(ManufacturerWarrantyD.WarrantyMonths).AddDays(-1),
                MWStartDate = Bundle[0].PolicySoldDate,
                MWKM = ManufacturerWarrantyD.WarrantyKm,
                MWMonths = ManufacturerWarrantyD.WarrantyMonths,
                ExtMonths = InsuaranceLimitation == null ? 0 : InsuaranceLimitation.Months,
                ExtKM = InsuaranceLimitation == null ? 0 : InsuaranceLimitation.Km,
                ExtStartDate = Bundle[0].PolicySoldDate.AddMonths(ManufacturerWarrantyD.WarrantyMonths),
                ExtEndDate = Bundle[0].PolicySoldDate.AddMonths(ManufacturerWarrantyD.WarrantyMonths)
                .AddMonths(InsuaranceLimitation == null ? 0 : InsuaranceLimitation.Months).AddDays(-1),
                MWIsAvailable = Bundle[0].MWIsAvailable,
                KMCutOff = GetKMCutOff(InsuaranceLimitation, ManufacturerWarrantyD)
            };
            foreach (var item in result.ContractProducts)
            {
                IProductManagementService ProductManagementService = ServiceFactory.GetProductManagementService();
                ProductResponseDto Product = ProductManagementService.GetProductById(item.ProductId,
                    SecurityHelper.Context,
                    AuditHelper.Context);
                item.Name = Product.Productcode + " - " + Product.Productname;

                if (item.PremiumCurrencyTypeId.ToString() != "00000000-0000-0000-0000-000000000000")
                {
                    ICurrencyManagementService CurrencyManagementService = ServiceFactory.GetCurrencyManagementService();
                    CurrencyResponseDto currency = CurrencyManagementService.GetAllCurrencies(SecurityHelper.Context, AuditHelper.Context).Currencies.Find(c => c.Id == item.PremiumCurrencyTypeId);
                    item.PremiumCurrencyName = currency.Code;
                }


                //IContractManagementService ContractManagementService = ServiceFactory.GetContractManagementService();

                ContractsResponseDto Contracts = ContractManagementService.GetContracts(
                    SecurityHelper.Context,
                    AuditHelper.Context);
                item.Contracts = Contracts.Contracts.FindAll(c => c.CommodityTypeId == Policy.CommodityTypeId);

                ContractInsuaranceLimitationResponseDto ContractInsuaranceLimitation = ContractManagementService.GetContractInsuaranceLimitations(item.ContractId,
                    SecurityHelper.Context,
                    AuditHelper.Context);

                ContractExtensionsResponseDto ContractExtensionss = ContractManagementService.GetContractExtensions(
                    SecurityHelper.Context,
                    AuditHelper.Context);

                IExtensionTypeManagementService ExtensionTypeManagementService = ServiceFactory.GetExtensionTypeManagementService();


                ExtensionTypesResponseDto ExtensionTypes = ExtensionTypeManagementService.GetExtensionTypes(
                    SecurityHelper.Context,
                    AuditHelper.Context);
                if (Bundle[0].Type == "Vehicle")
                {
                    item.ExtensionTypes = ContractManagementService.GetAllExtensionTypeByContractId(Bundle[0].ContractId,
                                Bundle[0].ProductId, Bundle[0].DealerId, Bundle[0].PolicySoldDate,
                                VehicleDetails.CylinderCountId, VehicleDetails.EngineCapacityId, VehicleDetails.MakeId, VehicleDetails.ModelId,
                                VehicleDetails.Variant, VehicleDetails.GrossWeight,
                                SecurityHelper.Context, AuditHelper.Context);
                }
                else
                {
                    item.ExtensionTypes = ContractManagementService.GetAllExtensionTypeByContractId(Bundle[0].ContractId,
                                Bundle[0].ProductId, Bundle[0].DealerId, Bundle[0].PolicySoldDate,
                                VehicleDetails.CylinderCountId, VehicleDetails.EngineCapacityId, BrownAndWhiteDetails.MakeId, BrownAndWhiteDetails.ModelId,
                                VehicleDetails.Variant, VehicleDetails.GrossWeight,
                                SecurityHelper.Context, AuditHelper.Context);
                }


                //item.ExtensionTypes = ExtensionTypes.ExtensionTypes.FindAll(e => e.CommodityTypeId == Policy.CommodityTypeId);

                InsuaranceLimitationsResponseDto InsuaranceLimitations = ContractManagementService.GetInsuaranceLimitations(
                    SecurityHelper.Context,
                    AuditHelper.Context);
                //item.ExtensionTypesLimi = InsuaranceLimitations.InsuaranceLimitation.FindAll(e => e.Id == ContractInsuaranceLimitation.InsuaranceLimitationId);

                item.AttributeSpecifications = ContractExtensionss.ContractExtensions.FindAll(c => c.ContractInsuanceLimitationId == ContractInsuaranceLimitation.Id);

                List<Guid> ContractIds = new List<Guid>();
                foreach (var c in Contracts.Contracts)
                {
                    ContractIds.Add(c.Id);
                }

                List<ContractExtensionResponseDto> ContractExtensions = ContractManagementService.GetContractExtensions(SecurityHelper.Context, AuditHelper.Context).ContractExtensions.FindAll(c => ContractIds.Contains(c.ContractId));

                if (Product.Productcode == "RSA")
                {
                    List<Guid> RSAIds = new List<Guid>();
                    foreach (var r in ContractExtensions)
                    {
                        RSAIds.Add(r.RSAProviderId);
                    }
                    IRSAProviderManagementService RSAProviderManagementService = ServiceFactory.GetRSAProviderManagementService();
                    RSAProvideresResponseDto Providers = RSAProviderManagementService.GetRSAProviders(
                        SecurityHelper.Context,
                        AuditHelper.Context);
                    item.RSAProviders = Providers.RSAProviders.FindAll(r => RSAIds.Contains(r.Id));
                    item.RSA = true;
                }
                else
                {
                    List<Guid> Warranty = new List<Guid>();
                    foreach (var r in ContractExtensions)
                    {
                        Warranty.Add(r.WarrantyTypeId);
                    }
                    IWarrantyTypeManagementService WarrantyTypeManagementService = ServiceFactory.GetWarrantyTypeManagementService();
                    WarrantyTypesResponseDto Covertypes = WarrantyTypeManagementService.GetWarrantyTypes(
                        SecurityHelper.Context,
                        AuditHelper.Context);

                    if (Bundle[0].Type == "Vehicle")
                    {
                        item.CoverTypess = ContractManagementService.GetCoverTypesByExtensionId(Bundle[0].ContractInsuaranceLimitationId,
                        Bundle[0].ContractExtensionsId, Bundle[0].ContractId,
                        Policy.ProductId, Policy.DealerId, Bundle[0].PolicySoldDate, VehicleDetails.CylinderCountId,
                        VehicleDetails.EngineCapacityId,
                        VehicleDetails.MakeId, VehicleDetails.ModelId, VehicleDetails.Variant,
                        VehicleDetails.GrossWeight, VehicleDetails.ItemStatusId
                        , SecurityHelper.Context,
                        AuditHelper.Context);
                    }
                    else
                    {
                        item.CoverTypess = ContractManagementService.GetCoverTypesByExtensionId(Bundle[0].ContractInsuaranceLimitationId,
                        Bundle[0].ContractExtensionsId, Bundle[0].ContractId,
                        Policy.ProductId, Policy.DealerId, Bundle[0].PolicySoldDate, VehicleDetails.CylinderCountId,
                        VehicleDetails.EngineCapacityId,
                        BrownAndWhiteDetails.MakeId, BrownAndWhiteDetails.ModelId, VehicleDetails.Variant,
                        VehicleDetails.GrossWeight, BrownAndWhiteDetails.ItemStatusId
                        , SecurityHelper.Context,
                        AuditHelper.Context);
                    }

                    ////item.CoverTypes = Covertypes.WarrantyTypes;
                }

            }
            return result;
        }

        private int GetKMCutOff(InsuaranceLimitationResponseDto insuaranceLimitation, ManufacturerWarrantyResponseDto manufacturerWarrantyD)
        {
            int cutoff = 0;
            if (manufacturerWarrantyD != null) {
                if (manufacturerWarrantyD.IsUnlimited) {
                    cutoff = Decimal.ToInt32(insuaranceLimitation.Km);
                }
                else
                {
                    cutoff = Decimal.ToInt32(insuaranceLimitation.Km) + Convert.ToInt32(manufacturerWarrantyD.WarrantyKm);
                }
            }
            else
            {
                cutoff = Decimal.ToInt32(insuaranceLimitation.Km);
            }
            return cutoff;
            //int mwKm = 0;
            //bool res = false;
            //if (manufacturerWarrantyD != null)
            //    res = int.TryParse(manufacturerWarrantyD.WarrantyKm, out mwKm);

            //if (insuaranceLimitation != null)
            //{
            //    if (insuaranceLimitation.TopOfMW)
            //        cutoff = Decimal.ToInt32(insuaranceLimitation.Km) +
            //            res? mwKm : 0 ;
            //    else
            //        cutoff = Decimal.ToInt32(insuaranceLimitation.Km);
            //}
            //return cutoff;
        }

        [HttpPost]
        public AttachmentsResponseDto GetAttachmentsByPolicyId(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            Guid policyId = Guid.Parse(data["Id"].ToString());
            IPolicyManagementService PolicyManagementService = ServiceFactory.GetPolicyManagementService();
            return PolicyManagementService.GetAttachmentsByPolicyId(policyId, SecurityHelper.Context, AuditHelper.Context);

        }

        [HttpPost]
        public AttachmentsByUsersResponseDto GetAttachmentsByPolicyIdByUserType(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            Guid policyId = Guid.Parse(data["Id"].ToString());
            IPolicyManagementService PolicyManagementService = ServiceFactory.GetPolicyManagementService();
            return PolicyManagementService.GetAttachmentsByPolicyIdByUserType(policyId, SecurityHelper.Context, AuditHelper.Context);

        }

        [HttpPost]
        public object GetClaimHistorysByPolicyId(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            Guid policyId = Guid.Parse(data["Id"].ToString());
            IPolicyManagementService PolicyManagementService = ServiceFactory.GetPolicyManagementService();
            return PolicyManagementService.GetClaimHistorysByPolicyId(policyId, SecurityHelper.Context, AuditHelper.Context);

        }

        [HttpPost]
        public object GetPolicies()
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            IVehicleDetailsManagementService VehicleDetailsManagementService = ServiceFactory.GetVehicleDetailsManagementService();
            VehicleDetailsResponseDto VehicleDetails = new VehicleDetailsResponseDto();
            IBrownAndWhiteDetailsManagementService BrownAndWhiteDetailsManagementService = ServiceFactory.GetBrownAndWhiteDetailsManagementService();
            BrownAndWhiteDetailsResponseDto BrownAndWhiteDetails = new BrownAndWhiteDetailsResponseDto();

            ICustomerManagementService customerManagementService = ServiceFactory.GetCustomerManagementService();
            CustomersResponseDto customerData = customerManagementService.GetCustomers(
            SecurityHelper.Context,
            AuditHelper.Context);

            IPolicyManagementService PolicyManagementService = ServiceFactory.GetPolicyManagementService();

            PolicyBundlesResponseDto PolicyData = PolicyManagementService.GetPolicyBundles(
            SecurityHelper.Context,
            AuditHelper.Context);
            List<PolicyResponseDto> Bundle = PolicyManagementService.GetPolicys(
                  SecurityHelper.Context,
                  AuditHelper.Context).Policies;
            List<PolicyViewData> result = new List<PolicyViewData>();

            if (PolicyData.Policies.Count == 0)
            {
                return result;
            }
            foreach (PolicyBundleResponseDto item in PolicyData.Policies.FindAll(p => p.IsApproved.Equals(false)))
            {
                string Type = "";
                Guid ItemId = new Guid();
                string PolicyNo = "";
                List<PolicyResponseDto> bundle = Bundle.FindAll(p => p.PolicyBundleId == item.Id);
                if (bundle.Count > 0)
                {
                    Type = bundle[0].Type;
                    ItemId = bundle[0].ItemId;
                    if (bundle.Count > 1)
                    {
                        PolicyNo = "Bundled";
                    }
                    else
                        PolicyNo = bundle[0].PolicyNo;
                }
                else
                    continue;
                if (Type == "Vehicle")
                {
                    VehicleDetails = VehicleDetailsManagementService.GetVehicleDetailsById(ItemId,
                SecurityHelper.Context,
                AuditHelper.Context);
                }
                else if (Type == "B&W")
                    BrownAndWhiteDetails = BrownAndWhiteDetailsManagementService.GetBrownAndWhiteDetailsById(ItemId,
                        SecurityHelper.Context,
                        AuditHelper.Context);
                result.Add(new PolicyViewData()
                {
                    Id = item.Id,
                    CommodityTypeId = item.CommodityTypeId,
                    ProductId = item.ProductId,
                    DealerId = item.DealerId,
                    DealerLocationId = item.DealerLocationId,
                    ContractId = item.ContractId,
                    ExtensionTypeId = item.ExtensionTypeId,
                    Premium = item.Premium,
                    PremiumCurrencyTypeId = item.PremiumCurrencyTypeId,
                    DealerPaymentCurrencyTypeId = item.DealerPaymentCurrencyTypeId,
                    CustomerPaymentCurrencyTypeId = item.CustomerPaymentCurrencyTypeId,
                    CoverTypeId = item.CoverTypeId,
                    HrsUsedAtPolicySale = item.HrsUsedAtPolicySale,
                    IsPreWarrantyCheck = item.IsPreWarrantyCheck,
                    PolicySoldDate = item.PolicySoldDate,
                    SalesPersonId = item.SalesPersonId,
                    PolicyNo = PolicyNo,
                    IsSpecialDeal = item.IsSpecialDeal,
                    IsPartialPayment = item.IsPartialPayment,
                    DealerPayment = item.DealerPayment,
                    CustomerPayment = item.CustomerPayment,
                    PaymentModeId = item.PaymentModeId,
                    RefNo = item.RefNo,
                    Comment = item.Comment,
                    ItemId = ItemId,
                    Type = Type,
                    CustomerId = item.CustomerId,
                    Vehicle = VehicleDetails,
                    Customer = customerData.Customers.Find(c => c.Id == item.CustomerId.ToString()),
                    BAndW = BrownAndWhiteDetails,
                    EntryDateTime = item.EntryDateTime,
                    EntryUser = item.EntryUser,
                    IsApproved = item.IsApproved,
                    PolicyStartDate = item.PolicyStartDate,
                    PolicyEndDate = item.PolicyEndDate
                });

            }
            return result;
        }

        [HttpPost]
        public object GetAllPolicies()
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            IVehicleDetailsManagementService VehicleDetailsManagementService = ServiceFactory.GetVehicleDetailsManagementService();
            VehicleDetailsResponseDto VehicleDetails = new VehicleDetailsResponseDto();
            IBrownAndWhiteDetailsManagementService BrownAndWhiteDetailsManagementService = ServiceFactory.GetBrownAndWhiteDetailsManagementService();
            BrownAndWhiteDetailsResponseDto BrownAndWhiteDetails = new BrownAndWhiteDetailsResponseDto();

            ICustomerManagementService customerManagementService = ServiceFactory.GetCustomerManagementService();
            CustomersResponseDto customerData = customerManagementService.GetCustomers(
            SecurityHelper.Context,
            AuditHelper.Context);

            IPolicyManagementService PolicyManagementService = ServiceFactory.GetPolicyManagementService();

            PolicyBundlesResponseDto PolicyData = PolicyManagementService.GetPolicyBundles(
            SecurityHelper.Context,
            AuditHelper.Context);
            List<PolicyResponseDto> Bundle = PolicyManagementService.GetPolicys(
                  SecurityHelper.Context,
                  AuditHelper.Context).Policies;
            List<PolicyViewData> result = new List<PolicyViewData>();

            if (PolicyData.Policies.Count == 0)
            {
                return result;
            }
            foreach (PolicyBundleResponseDto item in PolicyData.Policies)
            {
                string Type = "";
                Guid ItemId = new Guid();
                string PolicyNo = "";
                List<PolicyResponseDto> bundle = Bundle.FindAll(p => p.PolicyBundleId == item.Id);
                if (bundle.Count > 0)
                {
                    Type = bundle[0].Type;
                    ItemId = bundle[0].ItemId;
                    if (bundle.Count > 1)
                    {
                        PolicyNo = "Bundled";
                    }
                    else
                        PolicyNo = bundle[0].PolicyNo;
                }
                else
                    continue;
                if (Type == "Vehicle")
                {
                    VehicleDetails = VehicleDetailsManagementService.GetVehicleDetailsById(ItemId,
                SecurityHelper.Context,
                AuditHelper.Context);
                }
                else if (Type == "B&W")
                    BrownAndWhiteDetails = BrownAndWhiteDetailsManagementService.GetBrownAndWhiteDetailsById(ItemId,
                        SecurityHelper.Context,
                        AuditHelper.Context);
                result.Add(new PolicyViewData()
                {
                    Id = item.Id,
                    CommodityTypeId = item.CommodityTypeId,
                    ProductId = item.ProductId,
                    DealerId = item.DealerId,
                    DealerLocationId = item.DealerLocationId,
                    ContractId = item.ContractId,
                    ExtensionTypeId = item.ExtensionTypeId,
                    Premium = item.Premium,
                    PremiumCurrencyTypeId = item.PremiumCurrencyTypeId,
                    DealerPaymentCurrencyTypeId = item.DealerPaymentCurrencyTypeId,
                    CustomerPaymentCurrencyTypeId = item.CustomerPaymentCurrencyTypeId,
                    CoverTypeId = item.CoverTypeId,
                    HrsUsedAtPolicySale = item.HrsUsedAtPolicySale,
                    IsPreWarrantyCheck = item.IsPreWarrantyCheck,
                    PolicySoldDate = item.PolicySoldDate,
                    SalesPersonId = item.SalesPersonId,
                    PolicyNo = PolicyNo,
                    IsSpecialDeal = item.IsSpecialDeal,
                    IsPartialPayment = item.IsPartialPayment,
                    DealerPayment = item.DealerPayment,
                    CustomerPayment = item.CustomerPayment,
                    PaymentModeId = item.PaymentModeId,
                    RefNo = item.RefNo,
                    Comment = item.Comment,
                    ItemId = ItemId,
                    Type = Type,
                    CustomerId = item.CustomerId,
                    Vehicle = VehicleDetails,
                    Customer = customerData.Customers.Find(c => c.Id == item.CustomerId.ToString()),
                    BAndW = BrownAndWhiteDetails,
                    EntryDateTime = item.EntryDateTime,
                    EntryUser = item.EntryUser,
                    IsApproved = item.IsApproved,
                    PolicyStartDate = item.PolicyStartDate,
                    PolicyEndDate = item.PolicyEndDate
                });

            }
            return result;
        }

        [HttpPost]

        public object GetPoliciesForSearchGrid(PolicySearchGridRequestDto PolicySearchGridRequestDto)
        {
            logger.Trace("Policy Approval Grid Data Load Request came to controller");
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            IPolicyManagementService PolicyManagementService = ServiceFactory.GetPolicyManagementService();
            return PolicyManagementService.GetPoliciesForSearchGrid(
                PolicySearchGridRequestDto,
                SecurityHelper.Context,
            AuditHelper.Context);
        }
        [HttpPost]

        public object GetPoliciesForSearchGridReneval(PolicySearchGridRequestDto PolicySearchGridRequestDto)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            IPolicyManagementService PolicyManagementService = ServiceFactory.GetPolicyManagementService();
            return PolicyManagementService.GetPoliciesForSearchGridReneval(
                PolicySearchGridRequestDto,
                SecurityHelper.Context,
            AuditHelper.Context);
        }

        [HttpPost]
        public object GetDealerPolicies(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            IDealerManagementService DealerManagementService = ServiceFactory.GetDealerManagementService();
            List<Guid> Dealers = (List<Guid>)DealerManagementService.GetDealerStaffs(
                SecurityHelper.Context,
                AuditHelper.Context).DealerStaffs.FindAll(s => s.UserId == Guid.Parse(data["Id"].ToString())).Select(d => d.DealerId).ToList();

            IVehicleDetailsManagementService VehicleDetailsManagementService = ServiceFactory.GetVehicleDetailsManagementService();
            VehicleDetailsResponseDto VehicleDetails = new VehicleDetailsResponseDto();
            IBrownAndWhiteDetailsManagementService BrownAndWhiteDetailsManagementService = ServiceFactory.GetBrownAndWhiteDetailsManagementService();
            BrownAndWhiteDetailsResponseDto BrownAndWhiteDetails = new BrownAndWhiteDetailsResponseDto();

            ICustomerManagementService customerManagementService = ServiceFactory.GetCustomerManagementService();
            CustomersResponseDto customerData = customerManagementService.GetCustomers(
            SecurityHelper.Context,
            AuditHelper.Context);

            IPolicyManagementService PolicyManagementService = ServiceFactory.GetPolicyManagementService();

            List<PolicyBundleResponseDto> PolicyData = PolicyManagementService.GetPolicyBundles(
            SecurityHelper.Context,
            AuditHelper.Context).Policies.FindAll(p => p.DealerPolicy && Dealers.Contains(p.DealerId));
            List<PolicyResponseDto> Bundle = PolicyManagementService.GetPolicys(
                  SecurityHelper.Context,
                  AuditHelper.Context).Policies.FindAll(p => p.DealerPolicy && Dealers.Contains(p.DealerId));
            List<PolicyViewData> result = new List<PolicyViewData>();

            if (PolicyData.Count == 0)
            {
                return result;
            }
            foreach (PolicyBundleResponseDto item in PolicyData)
            {
                string Type = "";
                Guid ItemId = new Guid();
                string PolicyNo = "";
                List<PolicyResponseDto> bundle = Bundle.FindAll(p => p.PolicyBundleId == item.Id);
                if (bundle.Count > 0)
                {
                    Type = bundle[0].Type;
                    ItemId = bundle[0].ItemId;
                    if (bundle.Count > 1)
                    {
                        PolicyNo = "Bundled";
                    }
                    else
                        PolicyNo = bundle[0].PolicyNo;
                }
                else
                    continue;
                if (Type == "Vehicle")
                {
                    VehicleDetails = VehicleDetailsManagementService.GetVehicleDetailsById(ItemId,
                SecurityHelper.Context,
                AuditHelper.Context);
                }
                else if (Type == "B&W")
                    BrownAndWhiteDetails = BrownAndWhiteDetailsManagementService.GetBrownAndWhiteDetailsById(ItemId,
                        SecurityHelper.Context,
                        AuditHelper.Context);
                result.Add(new PolicyViewData()
                {
                    Id = item.Id,
                    CommodityTypeId = item.CommodityTypeId,
                    ProductId = item.ProductId,
                    DealerId = item.DealerId,
                    DealerLocationId = item.DealerLocationId,
                    ContractId = item.ContractId,
                    ExtensionTypeId = item.ExtensionTypeId,
                    Premium = item.Premium,
                    PremiumCurrencyTypeId = item.PremiumCurrencyTypeId,
                    DealerPaymentCurrencyTypeId = item.DealerPaymentCurrencyTypeId,
                    CustomerPaymentCurrencyTypeId = item.CustomerPaymentCurrencyTypeId,
                    CoverTypeId = item.CoverTypeId,
                    HrsUsedAtPolicySale = item.HrsUsedAtPolicySale,
                    IsPreWarrantyCheck = item.IsPreWarrantyCheck,
                    PolicySoldDate = item.PolicySoldDate,
                    SalesPersonId = item.SalesPersonId,
                    PolicyNo = PolicyNo,
                    IsSpecialDeal = item.IsSpecialDeal,
                    IsPartialPayment = item.IsPartialPayment,
                    DealerPayment = item.DealerPayment,
                    CustomerPayment = item.CustomerPayment,
                    PaymentModeId = item.PaymentModeId,
                    RefNo = item.RefNo,
                    Comment = item.Comment,
                    ItemId = ItemId,
                    Type = Type,
                    CustomerId = item.CustomerId,
                    Vehicle = VehicleDetails,
                    Customer = customerData.Customers.Find(c => c.Id == item.CustomerId.ToString()),
                    BAndW = BrownAndWhiteDetails,
                    EntryDateTime = item.EntryDateTime,
                    EntryUser = item.EntryUser,
                    IsApproved = item.IsApproved,
                    PolicyStartDate = item.PolicyStartDate,
                    PolicyEndDate = item.PolicyEndDate
                });

            }
            return result;
        }

        [HttpPost]
        public object GetPoliciesFromCustomerId(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            IPolicyManagementService PolicyManagementService = ServiceFactory.GetPolicyManagementService();

            PoliciesResponseDto PolicyData = PolicyManagementService.GetPolicys(
            SecurityHelper.Context,
            AuditHelper.Context);

            return PolicyData.Policies.FindAll(p => p.CustomerId == Guid.Parse(data["Id"].ToString()));

        }

        [HttpPost]
        public object GetPoliciesForBordx(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            IPolicyManagementService PolicyManagementService = ServiceFactory.GetPolicyManagementService();

            List<PolicyResponseDto> policies = PolicyManagementService.GetPolicys(
            SecurityHelper.Context,
            AuditHelper.Context).Policies;

            // List<PolicyResponseDto> transfered = policies.FindAll(t => t.Year == DateTime.Now.Year && t.Month == DateTime.Now.Month);

            policies = policies.FindAll(p => p.IsApproved.Equals(true)
                   && p.PolicySoldDate.Year.ToString() == data["Year"].ToString()
                 && DateTimeFormatInfo.CurrentInfo.GetMonthName(p.PolicySoldDate.Month).ToLower() == data["Month"].ToString().ToLower()
                   && p.CommodityTypeId == Guid.Parse(data["CommodityType"].ToString())
                   && p.DealerId == Guid.Parse(data["Dealer"].ToString())
                   && (p.Year == 0 || p.Year == p.PolicySoldDate.Year)
                   && (p.Month == 0 || p.Month == p.PolicySoldDate.Month)
                   );
            //foreach (var item in transfered)
            //{
            //    policies.Add(item);
            //}
            return policies;
        }

        [HttpPost]
        public string UpdateBordxPolicy(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                data["Month"] = DateTime.ParseExact(data["Month"].ToString(), "MMMM", CultureInfo.InvariantCulture).Month;
                PolicyRequestDto Policy = data.ToObject<PolicyRequestDto>();

                IPolicyManagementService PolicyManagementService = ServiceFactory.GetPolicyManagementService();
                PolicyRequestDto result = PolicyManagementService.UpdatePolicy(Policy, SecurityHelper.Context, AuditHelper.Context);

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

        [HttpPost]
        public object GetPremiumBreakdown(PremiumBreakdownRequestDto premiumBreakdownRequest)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            IPolicyManagementService PolicyManagementService = ServiceFactory.GetPolicyManagementService();
            return PolicyManagementService.GetPremiumBreakdown(premiumBreakdownRequest, SecurityHelper.Context,
            AuditHelper.Context);
        }


        [HttpPost]
        public string GetPolicyNumber(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            Guid branchId = Guid.Parse(data["branchId"].ToString());
            Guid dealerId = Guid.Parse(data["dealerId"].ToString());
            Guid productId = Guid.Parse(data["productId"].ToString());
            Guid tpaId = Guid.Parse(data["tpaId"].ToString());
            Guid commodityTypeId = Guid.Parse(data["commodityTypeId"].ToString());

            IPolicyManagementService PolicyManagementService = ServiceFactory.GetPolicyManagementService();
            return PolicyManagementService.GetPolicyNumber(branchId, dealerId, productId, tpaId, commodityTypeId, SecurityHelper.Context,
            AuditHelper.Context);
        }



        #endregion

        #region Approve Policy
        [HttpPost]
        public object GetApprovedPoliciesForEndorsement()
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            IVehicleDetailsManagementService VehicleDetailsManagementService = ServiceFactory.GetVehicleDetailsManagementService();
            VehicleDetailsResponseDto VehicleDetails = new VehicleDetailsResponseDto();
            IBrownAndWhiteDetailsManagementService BrownAndWhiteDetailsManagementService = ServiceFactory.GetBrownAndWhiteDetailsManagementService();
            BrownAndWhiteDetailsResponseDto BrownAndWhiteDetails = new BrownAndWhiteDetailsResponseDto();

            ICustomerManagementService customerManagementService = ServiceFactory.GetCustomerManagementService();
            CustomersResponseDto customerData = customerManagementService.GetCustomers(
            SecurityHelper.Context,
            AuditHelper.Context);

            IPolicyManagementService PolicyManagementService = ServiceFactory.GetPolicyManagementService();

            PolicyBundlesResponseDto PolicyBundleData = PolicyManagementService.GetPolicyBundles(
            SecurityHelper.Context,
            AuditHelper.Context);

            List<PolicyResponseDto> PolicyData = PolicyManagementService.GetPolicys(
            SecurityHelper.Context,
            AuditHelper.Context).Policies.FindAll(b => b.BordxId.ToString() == "00000000-0000-0000-0000-000000000000");

            Guid trans = PolicyManagementService.GetPolicyTransactionTypes(SecurityHelper.Context, AuditHelper.Context).PolicyTransactionTypes.Find(t => t.Code == "Endorsement").Id;

            List<PolicyBundleTransactionResponseDto> PolicyBundleEndorsementData = PolicyManagementService.GetPolicyBundleTransactions(
            SecurityHelper.Context,
            AuditHelper.Context).Policies.FindAll(c => c.TransactionTypeId == trans);

            PolicyTransactionsResponseDto PolicyEndorsement = PolicyManagementService.GetPolicyEndorsements(
            SecurityHelper.Context,
            AuditHelper.Context);

            List<PolicyTransactionResponseData> result = new List<PolicyTransactionResponseData>();
            foreach (var item in PolicyBundleData.Policies.FindAll(p => p.IsApproved))
            {
                PolicyTransactionResponseData endose = new PolicyTransactionResponseData();
                endose.PolicyId = item.Id;
                endose.CommodityTypeId = item.CommodityTypeId;
                endose.MobileNo = customerData.Customers.Find(c => c.Id == item.CustomerId.ToString()).MobileNo;
                endose.PolicySoldDate = item.PolicySoldDate;
                List<PolicyResponseDto> poly = PolicyData.FindAll(p => p.PolicyBundleId == item.Id);
                if (poly.Count == 0)
                    continue;
                if (poly.Count > 1)
                    endose.PolicyNo = "Bundle";
                else
                    endose.PolicyNo = poly[0].PolicyNo;
                item.Type = poly[0].Type;
                if (poly[0].Type == "Vehicle")
                {
                    VehicleDetails = VehicleDetailsManagementService.GetVehicleDetailsById(poly[0].ItemId,
                        SecurityHelper.Context,
                        AuditHelper.Context);
                    endose.VINNo = VehicleDetails.VINNo;
                    endose.VehicleId = VehicleDetails.Id;
                }
                else
                {
                    BrownAndWhiteDetails = BrownAndWhiteDetailsManagementService.GetBrownAndWhiteDetailsById(poly[0].ItemId,
                       SecurityHelper.Context,
                       AuditHelper.Context);
                    endose.SerialNo = BrownAndWhiteDetails.SerialNo;
                    endose.BAndWId = BrownAndWhiteDetails.Id;
                }
                endose.Endorsed = "No";
                result.Add(endose);
            }
            foreach (var item in PolicyBundleEndorsementData)
            {
                List<PolicyTransactionResponseDto> poly = PolicyEndorsement.Policies.FindAll(p => p.PolicyBundleId == item.Id);
                if (poly.Count == 0)
                    continue;
                if (poly.Count > 1)
                    item.PolicyNo = "Bundle";
                else
                    item.PolicyNo = poly[0].PolicyNo;
                PolicyTransactionResponseData en = new PolicyTransactionResponseData()
                {
                    Id = item.Id,
                    CommodityTypeId = item.CommodityTypeId,
                    MobileNo = poly[0].MobileNo,
                    PolicySoldDate = item.PolicySoldDate,
                    PolicyNo = item.PolicyNo,
                    VINNo = poly[0].VINNo,
                    VehicleId = poly[0].VehicleId,
                    SerialNo = poly[0].SerialNo,
                    BAndWId = poly[0].BAndWId
                };
                en.Endorsed = "Yes";
                result.Add(en);

            }
            return result;
        }


          [HttpPost]
        public string ApproveOtherTirePolicy(JObject data)
        {
            if (!AprovalRequirements())
            {
                return "Approve Policy failed!";
            }

            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            PolicyViewData NewPolicy = data.ToObject<PolicyViewData>();

            IBrownAndWhiteDetailsManagementService BrownAndWhiteDetailsManagementService = ServiceFactory.GetBrownAndWhiteDetailsManagementService();
            BrownAndWhiteDetailsRequestDto BrownAndWhiteDetails = new BrownAndWhiteDetailsRequestDto();
            BrownAndWhiteDetailsRequestDto resultB = new BrownAndWhiteDetailsRequestDto();

            ICustomerManagementService CustomerManagementService = ServiceFactory.GetCustomerManagementService();
            CustomerRequestDto Customer = new CustomerRequestDto();
            CustomerRequestDto resultC = new CustomerRequestDto();

            PolicyHistoryRequestDto history = new PolicyHistoryRequestDto();

            #region Customer
            if (NewPolicy.Customer.IDNo != null)
            {
                if (NewPolicy.Customer.Id.ToString() == "00000000-0000-0000-0000-000000000000")
                {
                    Customer = new CustomerRequestDto()
                    {
                        Address1 = NewPolicy.Customer.Address1,
                        Address2 = NewPolicy.Customer.Address2,
                        Address3 = NewPolicy.Customer.Address3,
                        Address4 = NewPolicy.Customer.Address4,
                        BusinessAddress1 = NewPolicy.Customer.Address1,
                        BusinessAddress2 = NewPolicy.Customer.Address2,
                        BusinessAddress3 = NewPolicy.Customer.Address3,
                        BusinessAddress4 = NewPolicy.Customer.Address4,
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
                        Password = NewPolicy.Customer.Password,
                        OtherTelNo = NewPolicy.Customer.OtherTelNo,
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
                        BusinessAddress1 = NewPolicy.Customer.Address1,
                        BusinessAddress2 = NewPolicy.Customer.Address2,
                        BusinessAddress3 = NewPolicy.Customer.Address3,
                        BusinessAddress4 = NewPolicy.Customer.Address4,
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
            #endregion

            #region Policy
            PolicyBundleRequestDto Policy = new PolicyBundleRequestDto()
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
                ItemId = resultB.Id,
                Type = NewPolicy.Type,
                CustomerId = Guid.Parse(resultC.Id),
                EntryDateTime = NewPolicy.EntryDateTime,
                EntryUser = NewPolicy.EntryUser,
                Discount = NewPolicy.Discount,
                IsApproved = true,
                jwt = NewPolicy.jwt
            };
            #endregion

            #region BHistory
            PolicyBundleHistoryRequestDto BHistory = new PolicyBundleHistoryRequestDto()
            {
                Id = new Guid(),
                PolicyId = NewPolicy.Id,
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
                ItemId = resultB.Id,
                Type = NewPolicy.Type,
                CustomerId = Guid.Parse(resultC.Id),
                EntryDateTime = NewPolicy.EntryDateTime,
                EntryUser = NewPolicy.EntryUser,
                Discount = NewPolicy.Discount
            };
            #endregion

            IPolicyManagementService PolicyManagementService = ServiceFactory.GetPolicyManagementService();
            PolicyBundleRequestDto result = PolicyManagementService.UpdatePolicyBundle(Policy, SecurityHelper.Context, AuditHelper.Context);
            PolicyBundleHistoryRequestDto resultH = PolicyManagementService.AddPolicyBundleHistory(BHistory, SecurityHelper.Context, AuditHelper.Context);
            if (NewPolicy.UploadAttachments !=null &&  NewPolicy.UploadAttachments.Count > 0) {
               bool uploaded = PolicyManagementService.addPolicyAttachement(Policy.Id , NewPolicy.UploadAttachments , SecurityHelper.Context, AuditHelper.Context);
                if (!uploaded)
                {
                    return "Policy Attachment Fail !";
                }
            }

            foreach (var item in NewPolicy.ContractProducts)
            {
                PolicyRequestDto P = new PolicyRequestDto()
                {
                    Id = item.Id,
                    CommodityTypeId = NewPolicy.CommodityTypeId,
                    ProductId = item.ProductId,
                    DealerId = NewPolicy.DealerId,
                    DealerLocationId = NewPolicy.DealerLocationId,
                    ContractId = item.ContractId,
                    ExtensionTypeId = item.ExtensionTypeId,
                    Premium = item.Premium,
                    PremiumCurrencyTypeId = item.PremiumCurrencyTypeId,
                    DealerPaymentCurrencyTypeId = NewPolicy.DealerPaymentCurrencyTypeId,
                    CustomerPaymentCurrencyTypeId = NewPolicy.CustomerPaymentCurrencyTypeId,
                    CoverTypeId = item.CoverTypeId,
                    HrsUsedAtPolicySale = NewPolicy.HrsUsedAtPolicySale,
                    IsPreWarrantyCheck = NewPolicy.IsPreWarrantyCheck,
                    PolicySoldDate = NewPolicy.PolicySoldDate,
                    SalesPersonId = NewPolicy.SalesPersonId,
                    PolicyNo = item.PolicyNo,
                    IsSpecialDeal = NewPolicy.IsSpecialDeal,
                    IsPartialPayment = NewPolicy.IsPartialPayment,
                    DealerPayment = NewPolicy.DealerPayment,
                    CustomerPayment = NewPolicy.CustomerPayment,
                    PaymentModeId = NewPolicy.PaymentModeId,
                    RefNo = NewPolicy.RefNo,
                    Comment = NewPolicy.Comment,
                    ItemId =  resultB.Id,
                    Type = NewPolicy.Type,
                    CustomerId = Guid.Parse(resultC.Id),
                    EntryDateTime = NewPolicy.EntryDateTime,
                    EntryUser = NewPolicy.EntryUser,
                    PolicyStartDate = item.PolicyStartDate,
                    PolicyEndDate = item.PolicyEndDate,
                    Discount = NewPolicy.Discount,
                    PolicyBundleId = result.Id,
                    IsApproved = true,
                    ApprovedDate = NewPolicy.ApprovedDate,
                    ContractInsuaranceLimitationId = item.ExtensionTypeId,
                    ContractExtensionsId = item.ContractExtensionsId
                };
                PolicyRequestDto res = PolicyManagementService.UpdatePolicy(P, SecurityHelper.Context, AuditHelper.Context);

                PolicyHistoryRequestDto Ph = new PolicyHistoryRequestDto()
                {
                    Id = new Guid(),
                    PolicyId = item.Id,
                    CommodityTypeId = NewPolicy.CommodityTypeId,
                    ProductId = item.ProductId,
                    DealerId = NewPolicy.DealerId,
                    DealerLocationId = NewPolicy.DealerLocationId,
                    ContractId = item.ContractId,
                    ExtensionTypeId = item.ExtensionTypeId,
                    Premium = item.Premium,
                    PremiumCurrencyTypeId = item.PremiumCurrencyTypeId,
                    DealerPaymentCurrencyTypeId = NewPolicy.DealerPaymentCurrencyTypeId,
                    CustomerPaymentCurrencyTypeId = NewPolicy.CustomerPaymentCurrencyTypeId,
                    CoverTypeId = item.CoverTypeId,
                    HrsUsedAtPolicySale = NewPolicy.HrsUsedAtPolicySale,
                    IsPreWarrantyCheck = NewPolicy.IsPreWarrantyCheck,
                    PolicySoldDate = NewPolicy.PolicySoldDate,
                    SalesPersonId = NewPolicy.SalesPersonId,
                    PolicyNo = item.PolicyNo,
                    IsSpecialDeal = NewPolicy.IsSpecialDeal,
                    IsPartialPayment = NewPolicy.IsPartialPayment,
                    DealerPayment = NewPolicy.DealerPayment,
                    CustomerPayment = NewPolicy.CustomerPayment,
                    PaymentModeId = NewPolicy.PaymentModeId,
                    RefNo = NewPolicy.RefNo,
                    Comment = NewPolicy.Comment,
                    CustomerId = Guid.Parse(resultC.Id),
                    PolicyStartDate = item.PolicyStartDate,
                    PolicyEndDate = item.PolicyEndDate,
                    Discount = NewPolicy.Discount,
                    PolicyBundleId = resultH.Id,
                    VehicleId = Guid.Empty,
                    BodyTypeId = Guid.Empty,
                    AspirationId = Guid.Empty,
                    CylinderCountId = Guid.Empty,
                    CategoryId = Guid.Empty,
                    EngineCapacityId = Guid.Empty,
                    DriveTypeId = Guid.Empty,
                    MakeId = NewPolicy.BAndW.MakeId,
                    FuelTypeId = Guid.Empty,
                    ModelId = NewPolicy.BAndW.ModelId,
                    ModelYear = NewPolicy.BAndW.ModelYear,
                    PlateNo = "",
                    ItemPurchasedDate = NewPolicy.BAndW.ItemPurchasedDate,
                    ItemStatusId = NewPolicy.BAndW.ItemStatusId,
                    VehiclePrice = NewPolicy.BAndW.ItemPrice,
                    DealerPrice = NewPolicy.BAndW.DealerPrice,
                    VINNo = NewPolicy.BAndW.InvoiceNo,
                    Variant = NewPolicy.BAndW.Variant,
                    TransmissionId = Guid.Empty,

                    AddnSerialNo = NewPolicy.BAndW.AddnSerialNo,
                    BAndWId = resultB.Id,
                    InvoiceNo = NewPolicy.BAndW.InvoiceNo,
                    ItemPrice = NewPolicy.BAndW.ItemPrice,
                    ModelCode = NewPolicy.BAndW.ModelCode,
                    SerialNo = NewPolicy.BAndW.SerialNo,
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
                    FirstName = NewPolicy.Customer.FirstName,
                    Gender = NewPolicy.Customer.Gender,
                    IDNo = NewPolicy.Customer.IDNo,
                    IDTypeId = NewPolicy.Customer.IDTypeId,
                    IsActive = NewPolicy.Customer.IsActive,
                    LastName = NewPolicy.Customer.LastName,
                    MobileNo = NewPolicy.Customer.MobileNo,
                    NationalityId = NewPolicy.Customer.NationalityId,
                    OtherTelNo = NewPolicy.Customer.OtherTelNo,
                    Password = NewPolicy.Customer.Password,
                    UsageTypeId = NewPolicy.Customer.UsageTypeId,
                    UserName = NewPolicy.Customer.UserName
                };
                PolicyHistoryRequestDto resH = PolicyManagementService.AddPolicyHistory(Ph, SecurityHelper.Context, AuditHelper.Context);

                foreach (var items in NewPolicy.ContractTireProducts)
                {
                    InvoiceCodeRequestDto IC = new InvoiceCodeRequestDto()
                    {
                        Id = items.InvoiceCodeId,
                        IsPolicyApproved = true,
                        PolicyCreatedDate = DateTime.UtcNow
                    };

                    InvoiceCodeRequestDto reIcode = PolicyManagementService.UpdateInvoiceCode(IC,P, SecurityHelper.Context, AuditHelper.Context);
                }
            }
            if (result.PolicyBundleInsertion)
            {

                return "OK";
            }
            else
            {
                return "Add Policy failed!";
            }
        }

        [HttpPost]
        public string ApprovePolicy(JObject data)
        {
            if (!AprovalRequirements())
            {
                return "Approve Policy failed!";
            }

            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

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

            PolicyHistoryRequestDto history = new PolicyHistoryRequestDto();


            #region Vehicle
            if (NewPolicy.Vehicle.VINNo != null)
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
                        DealerPrice = NewPolicy.Vehicle.DealerPrice,
                        VINNo = NewPolicy.Vehicle.VINNo,
                        Variant = NewPolicy.Vehicle.Variant,
                        TransmissionId = NewPolicy.Vehicle.TransmissionId,
                        DealerId = NewPolicy.Vehicle.DealerId,
                        CountryId = NewPolicy.Vehicle.CountryId,
                        currencyPeriodId = NewPolicy.Vehicle.currencyPeriodId,
                        DealerCurrencyId = NewPolicy.Vehicle.DealerCurrencyId,
                        ConversionRate = NewPolicy.Vehicle.ConversionRate,
                        RegistrationDate = NewPolicy.Vehicle.RegistrationDate,
                        GrossWeight = NewPolicy.Vehicle.GrossWeight,
                        EngineNumber = NewPolicy.Vehicle.EngineNumber

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
                        DealerPrice = NewPolicy.Vehicle.DealerPrice,
                        VINNo = NewPolicy.Vehicle.VINNo,
                        Variant = NewPolicy.Vehicle.Variant,
                        TransmissionId = NewPolicy.Vehicle.TransmissionId,
                        EntryDateTime = NewPolicy.Vehicle.EntryDateTime,
                        DealerId = NewPolicy.Vehicle.DealerId,
                        CountryId = NewPolicy.Vehicle.CountryId,
                        currencyPeriodId = NewPolicy.Vehicle.currencyPeriodId,
                        DealerCurrencyId = NewPolicy.Vehicle.DealerCurrencyId,
                        ConversionRate = NewPolicy.Vehicle.ConversionRate,
                        EntryUser = NewPolicy.Vehicle.EntryUser,
                        RegistrationDate = NewPolicy.Vehicle.RegistrationDate,
                        GrossWeight = NewPolicy.Vehicle.GrossWeight,
                        EngineNumber = NewPolicy.Vehicle.EngineNumber
                    };
                    resultV = VehicleDetailsManagementService.UpdateVehicleDetails(VehicleDetails, SecurityHelper.Context, AuditHelper.Context);
                    if (!resultV.VehicleDetailsInsertion)
                    {
                        return "Add Policy failed!";
                    }
                }
            }
            #endregion

            #region BAndW
            if (NewPolicy.BAndW.SerialNo != null)
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
            #endregion

            #region Customer
            if (NewPolicy.Customer.IDNo != null)
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
            #endregion

            #region Policy
            PolicyBundleRequestDto Policy = new PolicyBundleRequestDto()
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
                EntryUser = NewPolicy.EntryUser,
                Discount = NewPolicy.Discount,
                IsApproved = true
            };
            #endregion

            #region BHistory
            PolicyBundleHistoryRequestDto BHistory = new PolicyBundleHistoryRequestDto()
            {
                Id = new Guid(),
                PolicyId = NewPolicy.Id,
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
                EntryUser = NewPolicy.EntryUser,
                Discount = NewPolicy.Discount
            };
            #endregion

            IPolicyManagementService PolicyManagementService = ServiceFactory.GetPolicyManagementService();
            PolicyBundleRequestDto result = PolicyManagementService.UpdatePolicyBundle(Policy, SecurityHelper.Context, AuditHelper.Context);
            PolicyBundleHistoryRequestDto resultH = PolicyManagementService.AddPolicyBundleHistory(BHistory, SecurityHelper.Context, AuditHelper.Context);

            foreach (var item in NewPolicy.ContractProducts)
            {
                PolicyRequestDto P = new PolicyRequestDto()
                {
                    Id = item.Id,
                    CommodityTypeId = NewPolicy.CommodityTypeId,
                    ProductId = item.ProductId,
                    DealerId = NewPolicy.DealerId,
                    DealerLocationId = NewPolicy.DealerLocationId,
                    ContractId = item.ContractId,
                    ExtensionTypeId = item.ExtensionTypeId,
                    Premium = item.Premium,
                    PremiumCurrencyTypeId = item.PremiumCurrencyTypeId,
                    DealerPaymentCurrencyTypeId = NewPolicy.DealerPaymentCurrencyTypeId,
                    CustomerPaymentCurrencyTypeId = NewPolicy.CustomerPaymentCurrencyTypeId,
                    CoverTypeId = item.CoverTypeId,
                    HrsUsedAtPolicySale = NewPolicy.HrsUsedAtPolicySale,
                    IsPreWarrantyCheck = NewPolicy.IsPreWarrantyCheck,
                    PolicySoldDate = NewPolicy.PolicySoldDate,
                    SalesPersonId = NewPolicy.SalesPersonId,
                    PolicyNo = item.PolicyNo,
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
                    EntryUser = NewPolicy.EntryUser,
                    PolicyStartDate = item.PolicyStartDate,
                    PolicyEndDate = item.PolicyEndDate,
                    Discount = NewPolicy.Discount,
                    PolicyBundleId = result.Id,
                    IsApproved = true
                };
                PolicyRequestDto res = PolicyManagementService.UpdatePolicy(P, SecurityHelper.Context, AuditHelper.Context);

                PolicyHistoryRequestDto Ph = new PolicyHistoryRequestDto()
                {
                    Id = new Guid(),
                    PolicyId = item.Id,
                    CommodityTypeId = NewPolicy.CommodityTypeId,
                    ProductId = item.ProductId,
                    DealerId = NewPolicy.DealerId,
                    DealerLocationId = NewPolicy.DealerLocationId,
                    ContractId = item.ContractId,
                    ExtensionTypeId = item.ExtensionTypeId,
                    Premium = item.Premium,
                    PremiumCurrencyTypeId = item.PremiumCurrencyTypeId,
                    DealerPaymentCurrencyTypeId = NewPolicy.DealerPaymentCurrencyTypeId,
                    CustomerPaymentCurrencyTypeId = NewPolicy.CustomerPaymentCurrencyTypeId,
                    CoverTypeId = item.CoverTypeId,
                    HrsUsedAtPolicySale = NewPolicy.HrsUsedAtPolicySale,
                    IsPreWarrantyCheck = NewPolicy.IsPreWarrantyCheck,
                    PolicySoldDate = NewPolicy.PolicySoldDate,
                    SalesPersonId = NewPolicy.SalesPersonId,
                    PolicyNo = item.PolicyNo,
                    IsSpecialDeal = NewPolicy.IsSpecialDeal,
                    IsPartialPayment = NewPolicy.IsPartialPayment,
                    DealerPayment = NewPolicy.DealerPayment,
                    CustomerPayment = NewPolicy.CustomerPayment,
                    PaymentModeId = NewPolicy.PaymentModeId,
                    RefNo = NewPolicy.RefNo,
                    Comment = NewPolicy.Comment,
                    CustomerId = Guid.Parse(resultC.Id),
                    PolicyStartDate = item.PolicyStartDate,
                    PolicyEndDate = item.PolicyEndDate,
                    Discount = NewPolicy.Discount,
                    PolicyBundleId = resultH.Id,
                    VehicleId = resultV.Id,
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
                    DealerPrice = NewPolicy.Vehicle.DealerPrice,
                    VINNo = NewPolicy.Vehicle.VINNo,
                    Variant = NewPolicy.Vehicle.Variant,
                    TransmissionId = NewPolicy.Vehicle.TransmissionId,

                    AddnSerialNo = NewPolicy.BAndW.AddnSerialNo,
                    BAndWId = resultB.Id,
                    InvoiceNo = NewPolicy.BAndW.InvoiceNo,
                    ItemPrice = NewPolicy.BAndW.ItemPrice,
                    ModelCode = NewPolicy.BAndW.ModelCode,
                    SerialNo = NewPolicy.BAndW.SerialNo,
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
                    FirstName = NewPolicy.Customer.FirstName,
                    Gender = NewPolicy.Customer.Gender,
                    IDNo = NewPolicy.Customer.IDNo,
                    IDTypeId = NewPolicy.Customer.IDTypeId,
                    IsActive = NewPolicy.Customer.IsActive,
                    LastName = NewPolicy.Customer.LastName,
                    MobileNo = NewPolicy.Customer.MobileNo,
                    NationalityId = NewPolicy.Customer.NationalityId,
                    OtherTelNo = NewPolicy.Customer.OtherTelNo,
                    Password = NewPolicy.Customer.Password,
                    UsageTypeId = NewPolicy.Customer.UsageTypeId,
                    UserName = NewPolicy.Customer.UserName
                };
                PolicyHistoryRequestDto resH = PolicyManagementService.AddPolicyHistory(Ph, SecurityHelper.Context, AuditHelper.Context);
            }
            if (result.PolicyBundleInsertion)
            {

                return "OK";
            }
            else
            {
                return "Add Policy failed!";
            }
        }

        bool AprovalRequirements()
        {
            return true;
        }
        #endregion

        #region Transfer
        [HttpPost]
        public string TransferPolicy(JObject obj)
        {
            SavePolicyRequestDto SavePolicyRequest = obj.ToObject<SavePolicyRequestDto>();
            IPolicyManagementService PolicyManagementService = ServiceFactory.GetPolicyManagementService();
            return PolicyManagementService.TransferPolicy(SavePolicyRequest, SecurityHelper.Context, AuditHelper.Context);

        }

        [HttpPost]
        public object GetPolicyTransferHistory(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            IPolicyManagementService PolicyManagementService = ServiceFactory.GetPolicyManagementService();
            Guid policyBundleId = Guid.Parse(data["Id"].ToString());
            return PolicyManagementService.GetPolicyTransferHistoryById(policyBundleId, SecurityHelper.Context, AuditHelper.Context);
        }
        #endregion

        #region Endorsement
        [HttpPost]
        public string EndorsePolicy(JObject obj)
        {
            SavePolicyRequestDto SavePolicyRequest = obj.ToObject<SavePolicyRequestDto>();
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            IPolicyManagementService PolicyManagementService = ServiceFactory.GetPolicyManagementService();
            return PolicyManagementService.EndorsePolicy(SavePolicyRequest,
            SecurityHelper.Context,
            AuditHelper.Context);
        }

        [HttpPost]
        public object GetAllPolicyEndorsementDetailsForApproval(JObject data)
        {
            Guid BundlePolicyId = Guid.Parse(data["Id"].ToString());
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            IPolicyManagementService PolicyManagementService = ServiceFactory.GetPolicyManagementService();

            return PolicyManagementService.GetAllPolicyEndorsementDetailsForApproval(BundlePolicyId,
            SecurityHelper.Context,
            AuditHelper.Context);
        }

        [HttpPost]
        public object GetAllPolicyInquiryDetails(JObject data)
        {
            Guid BundlePolicyId = Guid.Parse(data["Id"].ToString());
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            IPolicyManagementService PolicyManagementService = ServiceFactory.GetPolicyManagementService();

            return PolicyManagementService.GetAllPolicyInquiryDetails(BundlePolicyId,
            SecurityHelper.Context,
            AuditHelper.Context);
        }

        [HttpPost]
        public object GetPolicyById(JObject data)
        {
            Guid PolicyById = Guid.Parse(data["Id"].ToString());
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            IPolicyManagementService PolicyManagementService = ServiceFactory.GetPolicyManagementService();

            return PolicyManagementService.GetPolicyById2(PolicyById,
            SecurityHelper.Context,
            AuditHelper.Context);
        }

        [HttpPost]
        public object GetOtherTirePolicyById(JObject data)
        {
            Guid PolicyById = Guid.Parse(data["Id"].ToString());
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            IPolicyManagementService PolicyManagementService = ServiceFactory.GetPolicyManagementService();

            return PolicyManagementService.GetOtherTirePolicyById(PolicyById,
            SecurityHelper.Context,
            AuditHelper.Context);
        }

        [HttpPost]
        public object GetPolicesByCustomerId(JObject data)
        {
            Guid CustomerId = Guid.Parse(data["Id"].ToString());
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            IPolicyManagementService PolicyManagementService = ServiceFactory.GetPolicyManagementService();

            return PolicyManagementService.GetPolicesByCustomerId(CustomerId,
            SecurityHelper.Context,
            AuditHelper.Context);
        }

        [HttpPost]
        public string AddPolicyEndorsement(JObject data)
        {

            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            PolicyTransactionRequestDto PolicyEndorsement = data.ToObject<PolicyTransactionRequestDto>();
            PolicyBundleTransactionRequestDto Policy = new PolicyBundleTransactionRequestDto()
            {
                Id = PolicyEndorsement.Id,
                PolicyId = PolicyEndorsement.PolicyId,
                CommodityTypeId = PolicyEndorsement.CommodityTypeId,
                ProductId = PolicyEndorsement.ProductId,
                DealerId = PolicyEndorsement.DealerId,
                DealerLocationId = PolicyEndorsement.DealerLocationId,
                ContractId = PolicyEndorsement.ContractId,
                ExtensionTypeId = PolicyEndorsement.ExtensionTypeId,
                Premium = PolicyEndorsement.Premium,
                PremiumCurrencyTypeId = PolicyEndorsement.PremiumCurrencyTypeId,
                DealerPaymentCurrencyTypeId = PolicyEndorsement.DealerPaymentCurrencyTypeId,
                CustomerPaymentCurrencyTypeId = PolicyEndorsement.CustomerPaymentCurrencyTypeId,
                CoverTypeId = PolicyEndorsement.CoverTypeId,
                HrsUsedAtPolicySale = PolicyEndorsement.HrsUsedAtPolicySale,
                IsPreWarrantyCheck = PolicyEndorsement.IsPreWarrantyCheck,
                PolicySoldDate = PolicyEndorsement.PolicySoldDate,
                SalesPersonId = PolicyEndorsement.SalesPersonId,
                PolicyNo = PolicyEndorsement.PolicyNo,
                IsSpecialDeal = PolicyEndorsement.IsSpecialDeal,
                IsPartialPayment = PolicyEndorsement.IsPartialPayment,
                DealerPayment = PolicyEndorsement.DealerPayment,
                CustomerPayment = PolicyEndorsement.CustomerPayment,
                PaymentModeId = PolicyEndorsement.PaymentModeId,
                RefNo = PolicyEndorsement.RefNo,
                Comment = PolicyEndorsement.Comment,
                ItemId = (PolicyEndorsement.VehicleId.ToString() != "00000000-0000-0000-0000-000000000000") ? PolicyEndorsement.VehicleId : PolicyEndorsement.BAndWId,
                Type = (PolicyEndorsement.VehicleId.ToString() != "00000000-0000-0000-0000-000000000000") ? "Vehicle" : "BAndW",
                CustomerId = PolicyEndorsement.CustomerId,
                Discount = PolicyEndorsement.Discount
            };

            IPolicyManagementService PolicyManagementService = ServiceFactory.GetPolicyManagementService();
            PolicyBundleTransactionRequestDto result = PolicyManagementService.AddPolicyBundleTransaction(Policy, SecurityHelper.Context, AuditHelper.Context);

            foreach (var item in PolicyEndorsement.ContractProducts)
            {
                PolicyTransactionRequestDto P = PolicyEndorsement;
                P.ProductId = item.ProductId;
                P.ContractId = item.ContractId;
                P.ExtensionTypeId = item.ExtensionTypeId;
                P.Premium = item.Premium;
                P.PremiumCurrencyTypeId = item.PremiumCurrencyTypeId;
                P.CoverTypeId = item.CoverTypeId;
                P.PolicyNo = item.PolicyNo;
                P.PolicyBundleId = result.Id;
                P.PolicyStartDate = item.PolicyStartDate;
                P.PolicyEndDate = item.PolicyEndDate;
                PolicyTransactionRequestDto res = PolicyManagementService.AddPolicyEndorsement(P, SecurityHelper.Context, AuditHelper.Context);
            }
            logger.Info("PolicyEndorsement Added");
            if (result.PolicyBundleInsertion)
            {
                return "OK";
            }
            else
            {
                return "Add PolicyEndorsement  failed!";
            }
        }

        [HttpPost]
        public object GetPolicyEndorsementById(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            IPolicyManagementService PolicyManagementService = ServiceFactory.GetPolicyManagementService();

            PolicyBundleTransactionResponseDto Policy = PolicyManagementService.GetPolicyBundleTransactionById(Guid.Parse(data["Id"].ToString()),
                SecurityHelper.Context,
                AuditHelper.Context);
            PolicyTransactionResponseDto result = new PolicyTransactionResponseDto();

            List<PolicyTransactionResponseDto> Bundle = PolicyManagementService.GetPolicyEndorsements(SecurityHelper.Context,
                AuditHelper.Context).Policies.FindAll(p => p.PolicyBundleId == Policy.Id);
            result = Bundle[0];
            result.PolicyId = Policy.PolicyId;
            result.Id = Policy.Id;
            result.ProductId = Policy.ProductId;
            List<PolicyContractProductResponseDto> retBundle = new List<PolicyContractProductResponseDto>();

            foreach (var item in Bundle)
            {
                PolicyContractProductResponseDto p = new PolicyContractProductResponseDto()
                {
                    ContractId = item.ContractId,
                    CoverTypeId = item.CoverTypeId,
                    ExtensionTypeId = item.ExtensionTypeId,
                    Id = item.Id,
                    Premium = item.Premium,
                    PremiumCurrencyTypeId = item.PremiumCurrencyTypeId,
                    ProductId = item.ProductId,
                    PolicyNo = item.PolicyNo,
                    PolicyEndDate = item.PolicyEndDate,
                    PolicyStartDate = item.PolicyStartDate
                };
                retBundle.Add(p);
            }
            result.ContractProducts = retBundle;
            foreach (var item in Bundle[0].ContractProducts)
            {
                IProductManagementService ProductManagementService = ServiceFactory.GetProductManagementService();
                ProductResponseDto Product = ProductManagementService.GetProductById(item.ProductId,
                    SecurityHelper.Context,
                    AuditHelper.Context);
                item.Name = Product.Productcode + " - " + Product.Productname;

                if (item.PremiumCurrencyTypeId.ToString() != "00000000-0000-0000-0000-000000000000")
                {
                    ICurrencyManagementService CurrencyManagementService = ServiceFactory.GetCurrencyManagementService();
                    CurrencyResponseDto currency = CurrencyManagementService.GetAllCurrencies(SecurityHelper.Context, AuditHelper.Context).Currencies.Find(c => c.Id == item.PremiumCurrencyTypeId);
                    item.PremiumCurrencyName = currency.Code;
                }

                IExtensionTypeManagementService ExtensionTypeManagementService = ServiceFactory.GetExtensionTypeManagementService();
                ExtensionTypesResponseDto ExtensionTypes = ExtensionTypeManagementService.GetExtensionTypes(
                    SecurityHelper.Context,
                    AuditHelper.Context);
                item.ExtensionTypes = ExtensionTypes.ExtensionTypes.FindAll(e => e.CommodityTypeId == Policy.CommodityTypeId);

                IContractManagementService ContractManagementService = ServiceFactory.GetContractManagementService();
                ContractsResponseDto Contracts = ContractManagementService.GetContracts(
                    SecurityHelper.Context,
                    AuditHelper.Context);
                item.Contracts = Contracts.Contracts.FindAll(c => c.CommodityTypeId == Policy.CommodityTypeId);

                List<Guid> ContractIds = new List<Guid>();
                foreach (var c in Contracts.Contracts)
                {
                    ContractIds.Add(c.Id);
                }
                List<ContractExtensionResponseDto> ContractExtensions = ContractManagementService.GetContractExtensions(SecurityHelper.Context, AuditHelper.Context).ContractExtensions.FindAll(c => ContractIds.Contains(c.ContractId));

                if (Product.Productcode == "RSA")
                {
                    List<Guid> RSAIds = new List<Guid>();
                    foreach (var r in ContractExtensions)
                    {
                        RSAIds.Add(r.RSAProviderId);
                    }
                    IRSAProviderManagementService RSAProviderManagementService = ServiceFactory.GetRSAProviderManagementService();
                    RSAProvideresResponseDto Providers = RSAProviderManagementService.GetRSAProviders(
                        SecurityHelper.Context,
                        AuditHelper.Context);
                    item.RSAProviders = Providers.RSAProviders.FindAll(r => RSAIds.Contains(r.Id));
                    item.RSA = true;
                }
                else
                {
                    List<Guid> Warranty = new List<Guid>();
                    foreach (var r in ContractExtensions)
                    {
                        Warranty.Add(r.WarrantyTypeId);
                    }
                    IWarrantyTypeManagementService WarrantyTypeManagementService = ServiceFactory.GetWarrantyTypeManagementService();
                    WarrantyTypesResponseDto Covertypes = WarrantyTypeManagementService.GetWarrantyTypes(
                        SecurityHelper.Context,
                        AuditHelper.Context);
                    item.CoverTypes = Covertypes.WarrantyTypes;
                }

            }
            return result;
        }

        [HttpPost]
        public object GetAllPolicyEndorsements()
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            IPolicyManagementService PolicyEndorsementManagementService = ServiceFactory.GetPolicyManagementService();

            Guid trans = PolicyEndorsementManagementService.GetPolicyTransactionTypes(SecurityHelper.Context, AuditHelper.Context).PolicyTransactionTypes.Find(t => t.Code == "Endorsement").Id;

            List<PolicyBundleTransactionResponseDto> PolicyEndorsementData = PolicyEndorsementManagementService.GetPolicyBundleTransactions(
            SecurityHelper.Context,
            AuditHelper.Context).Policies.FindAll(c => c.TransactionTypeId == trans);

            PolicyTransactionsResponseDto PolicyEndorsement = PolicyEndorsementManagementService.GetPolicyEndorsements(
            SecurityHelper.Context,
            AuditHelper.Context);

            List<PolicyTransactionResponseData> result = new List<PolicyTransactionResponseData>();

            foreach (var item in PolicyEndorsementData)
            {
                List<PolicyTransactionResponseDto> poly = PolicyEndorsement.Policies.FindAll(p => p.PolicyBundleId == item.Id);
                if (poly.Count == 0)
                    continue;
                if (poly.Count > 1)
                    item.PolicyNo = "Bundle";
                else
                    item.PolicyNo = poly[0].PolicyNo;
                PolicyTransactionResponseData en = new PolicyTransactionResponseData()
                {
                    Id = item.Id,
                    CommodityTypeId = item.CommodityTypeId,
                    MobileNo = poly[0].MobileNo,
                    PolicySoldDate = item.PolicySoldDate,
                    PolicyNo = item.PolicyNo,
                    VINNo = poly[0].VINNo,
                    VehicleId = poly[0].VehicleId,
                    SerialNo = poly[0].SerialNo,
                    BAndWId = poly[0].BAndWId
                };
                en.Endorsed = "Yes";
                result.Add(en);

            }
            return result;
        }

        [HttpPost]
        public string ApproveEndorsement(JObject data)
        {
            Guid PolicyBundleId = Guid.Parse(data["Id"].ToString());
            Guid UserId = Guid.Parse(data["userId"].ToString());

            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            IPolicyManagementService PolicyEndorsementManagementService = ServiceFactory.GetPolicyManagementService();

            string response = PolicyEndorsementManagementService.ApproveEndorsement(PolicyBundleId, UserId, SecurityHelper.Context, AuditHelper.Context);
            return response;
        }

        [HttpPost]
        public string RejectEndorsement(JObject data)
        {
            Guid PolicyBundleId = Guid.Parse(data["Id"].ToString());
            Guid UserId = Guid.Parse(data["userId"].ToString());
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            IPolicyManagementService PolicyEndorsementManagementService = ServiceFactory.GetPolicyManagementService();

            string response = PolicyEndorsementManagementService.RejectEndorsement(PolicyBundleId, UserId, SecurityHelper.Context, AuditHelper.Context);
            return response;
        }
        #endregion

        #region Endorsement Approval
        [HttpPost]
        public string ApprovePolicyEndorsement(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                PolicyTransactionRequestDto PolicyEndorsement = data.ToObject<PolicyTransactionRequestDto>();
                IPolicyManagementService PolicyEndorsementManagementService = ServiceFactory.GetPolicyManagementService();
                bool result = PolicyEndorsementManagementService.ApprovePolicyEndorsement(PolicyEndorsement.Id, true, SecurityHelper.Context, AuditHelper.Context);
                logger.Info("PolicyEndorsement Added");
                if (result)
                {
                    return "OK";
                }
                else
                {
                    return "Add PolicyEndorsement  failed!";
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return "Add PolicyEndorsement  failed!";
            }

        }
        [HttpPost]
        public string RejectPolicyEndorsement(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                PolicyTransactionRequestDto PolicyEndorsement = data.ToObject<PolicyTransactionRequestDto>();
                IPolicyManagementService PolicyEndorsementManagementService = ServiceFactory.GetPolicyManagementService();
                bool result = PolicyEndorsementManagementService.ApprovePolicyEndorsement(PolicyEndorsement.Id, false, SecurityHelper.Context, AuditHelper.Context);
                logger.Info("PolicyEndorsement Added");
                if (result)
                {
                    return "OK";
                }
                else
                {
                    return "Add PolicyEndorsement  failed!";
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return "Add PolicyEndorsement  failed!";
            }

        }
        #endregion

        #region Cancelation

        [HttpPost]
        public string PolicyCancellationRequest(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            Guid PolicyBundleId = Guid.Parse(data["policyBundleId"].ToString());
            string CancellationComment = data["comment"].ToString();
            Guid UserId = Guid.Parse(data["userId"].ToString());
            IPolicyManagementService PolicyEndorsementManagementService = ServiceFactory.GetPolicyManagementService();
            string response = PolicyEndorsementManagementService.PolicyCancellation(PolicyBundleId, CancellationComment, UserId, SecurityHelper.Context, AuditHelper.Context);
            return response;
        }

        [HttpPost]
        public string GetAllPolicyCancellationCommentForApproval(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            Guid PolicyBundleId = Guid.Parse(data["policyBundleId"].ToString());
            IPolicyManagementService PolicyEndorsementManagementService = ServiceFactory.GetPolicyManagementService();
            string response = PolicyEndorsementManagementService.GetPolicyCancellationCommentByPolicyBundleId(PolicyBundleId, SecurityHelper.Context, AuditHelper.Context);
            return response;

        }

        [HttpPost]
        public string PolicyCancellationApproval(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            Guid PolicyBundleId = Guid.Parse(data["policyBundleId"].ToString());
            Guid UserId = Guid.Parse(data["userId"].ToString());
            IPolicyManagementService PolicyEndorsementManagementService = ServiceFactory.GetPolicyManagementService();
            string response = PolicyEndorsementManagementService.PolicyCancellationApproval(PolicyBundleId, UserId, SecurityHelper.Context, AuditHelper.Context);
            return response;
        }

        [HttpPost]
        public string PolicyCancellationReject(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            Guid PolicyBundleId = Guid.Parse(data["policyBundleId"].ToString());
            Guid UserId = Guid.Parse(data["userId"].ToString());
            IPolicyManagementService PolicyEndorsementManagementService = ServiceFactory.GetPolicyManagementService();
            string response = PolicyEndorsementManagementService.PolicyCancellationReject(PolicyBundleId, UserId, SecurityHelper.Context, AuditHelper.Context);
            return response;
        }

        [HttpPost]
        public string PolicyCancelation(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                PolicyTransactionRequestDto PolicyEndorsement = data.ToObject<PolicyTransactionRequestDto>();
                PolicyBundleTransactionRequestDto Policy = new PolicyBundleTransactionRequestDto()
                {
                    Id = PolicyEndorsement.Id,
                    PolicyId = PolicyEndorsement.PolicyId,
                    CommodityTypeId = PolicyEndorsement.CommodityTypeId,
                    ProductId = PolicyEndorsement.ProductId,
                    DealerId = PolicyEndorsement.DealerId,
                    DealerLocationId = PolicyEndorsement.DealerLocationId,
                    ContractId = PolicyEndorsement.ContractId,
                    ExtensionTypeId = PolicyEndorsement.ExtensionTypeId,
                    Premium = PolicyEndorsement.Premium,
                    PremiumCurrencyTypeId = PolicyEndorsement.PremiumCurrencyTypeId,
                    DealerPaymentCurrencyTypeId = PolicyEndorsement.DealerPaymentCurrencyTypeId,
                    CustomerPaymentCurrencyTypeId = PolicyEndorsement.CustomerPaymentCurrencyTypeId,
                    CoverTypeId = PolicyEndorsement.CoverTypeId,
                    HrsUsedAtPolicySale = PolicyEndorsement.HrsUsedAtPolicySale,
                    IsPreWarrantyCheck = PolicyEndorsement.IsPreWarrantyCheck,
                    PolicySoldDate = PolicyEndorsement.PolicySoldDate,
                    SalesPersonId = PolicyEndorsement.SalesPersonId,
                    PolicyNo = PolicyEndorsement.PolicyNo,
                    IsSpecialDeal = PolicyEndorsement.IsSpecialDeal,
                    IsPartialPayment = PolicyEndorsement.IsPartialPayment,
                    DealerPayment = PolicyEndorsement.DealerPayment,
                    CustomerPayment = PolicyEndorsement.CustomerPayment,
                    PaymentModeId = PolicyEndorsement.PaymentModeId,
                    RefNo = PolicyEndorsement.RefNo,
                    Comment = PolicyEndorsement.Comment,
                    ItemId = (PolicyEndorsement.VehicleId.ToString() != "00000000-0000-0000-0000-000000000000") ? PolicyEndorsement.VehicleId : PolicyEndorsement.BAndWId,
                    Type = (PolicyEndorsement.VehicleId.ToString() != "00000000-0000-0000-0000-000000000000") ? "Vehicle" : "BAndW",
                    CustomerId = PolicyEndorsement.CustomerId,
                    Discount = PolicyEndorsement.Discount
                };

                IPolicyManagementService PolicyManagementService = ServiceFactory.GetPolicyManagementService();
                PolicyBundleTransactionRequestDto result = PolicyManagementService.AddPolicyBundleCancellation(Policy, SecurityHelper.Context, AuditHelper.Context);

                foreach (var item in PolicyEndorsement.ContractProducts)
                {
                    PolicyTransactionRequestDto P = PolicyEndorsement;
                    P.ProductId = item.ProductId;
                    P.ContractId = item.ContractId;
                    P.ExtensionTypeId = item.ExtensionTypeId;
                    P.Premium = item.Premium;
                    P.PremiumCurrencyTypeId = item.PremiumCurrencyTypeId;
                    P.CoverTypeId = item.CoverTypeId;
                    P.PolicyNo = item.PolicyNo;
                    P.PolicyBundleId = result.Id;
                    P.PolicyStartDate = item.PolicyStartDate;
                    P.PolicyEndDate = item.PolicyEndDate;
                    PolicyTransactionRequestDto res = PolicyManagementService.AddPolicyCancellation(P, SecurityHelper.Context, AuditHelper.Context);
                }
                logger.Info("PolicyEndorsement Added");
                if (result.PolicyBundleInsertion)
                {
                    return "OK";
                }
                else
                {
                    return "Add PolicyEndorsement  failed!";
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return "Add PolicyEndorsement  failed!";
            }


        }

        [HttpPost]
        public object GetCanceledPolicies()
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            IPolicyManagementService PolicyEndorsementManagementService = ServiceFactory.GetPolicyManagementService();

            Guid trans = PolicyEndorsementManagementService.GetPolicyTransactionTypes(SecurityHelper.Context, AuditHelper.Context).PolicyTransactionTypes.Find(t => t.Code == "Cancellation").Id;

            List<PolicyBundleTransactionResponseDto> PolicyEndorsementData = PolicyEndorsementManagementService.GetPolicyBundleTransactions(
            SecurityHelper.Context,
            AuditHelper.Context).Policies.FindAll(c => c.TransactionTypeId == trans);

            PolicyTransactionsResponseDto PolicyEndorsement = PolicyEndorsementManagementService.GetPolicyEndorsements(
            SecurityHelper.Context,
            AuditHelper.Context);

            List<PolicyTransactionResponseData> result = new List<PolicyTransactionResponseData>();

            foreach (var item in PolicyEndorsementData)
            {
                List<PolicyTransactionResponseDto> poly = PolicyEndorsement.Policies.FindAll(p => p.PolicyBundleId == item.Id);
                if (poly.Count == 0)
                    continue;
                if (poly.Count > 1)
                    item.PolicyNo = "Bundle";
                else
                    item.PolicyNo = poly[0].PolicyNo;
                PolicyTransactionResponseData en = new PolicyTransactionResponseData()
                {
                    Id = item.Id,
                    CommodityTypeId = item.CommodityTypeId,
                    MobileNo = poly[0].MobileNo,
                    PolicySoldDate = item.PolicySoldDate,
                    PolicyNo = item.PolicyNo,
                    VINNo = poly[0].VINNo,
                    VehicleId = poly[0].VehicleId,
                    SerialNo = poly[0].SerialNo,
                    BAndWId = poly[0].BAndWId
                };
                en.Endorsed = "Yes";
                result.Add(en);

            }
            return result;
        }

        [HttpPost]
        public string PolicyCancelationApproval(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                IPolicyManagementService PolicyManagementService = ServiceFactory.GetPolicyManagementService();

                PolicyTransactionRequestDto CanceledPolicy = data.ToObject<PolicyTransactionRequestDto>();
                PolicyResponseDto NewPolicy = PolicyManagementService.GetPolicyById(CanceledPolicy.PolicyId,
                 SecurityHelper.Context,
                 AuditHelper.Context);

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
                    ItemId = NewPolicy.ItemId,
                    Type = NewPolicy.Type,
                    CustomerId = NewPolicy.CustomerId,
                    EntryDateTime = NewPolicy.EntryDateTime,
                    EntryUser = NewPolicy.EntryUser,
                    IsPolicyCanceled = true,
                    PolicyStartDate = NewPolicy.PolicyStartDate,
                    PolicyEndDate = NewPolicy.PolicyEndDate
                };
                PolicyRequestDto result = PolicyManagementService.UpdatePolicy(Policy, SecurityHelper.Context, AuditHelper.Context);
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

        [HttpPost]
        public object GetPoliciesForCancelation()
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            IVehicleDetailsManagementService VehicleDetailsManagementService = ServiceFactory.GetVehicleDetailsManagementService();
            VehicleDetailsResponseDto VehicleDetails = new VehicleDetailsResponseDto();
            IBrownAndWhiteDetailsManagementService BrownAndWhiteDetailsManagementService = ServiceFactory.GetBrownAndWhiteDetailsManagementService();
            BrownAndWhiteDetailsResponseDto BrownAndWhiteDetails = new BrownAndWhiteDetailsResponseDto();

            ICustomerManagementService customerManagementService = ServiceFactory.GetCustomerManagementService();
            CustomersResponseDto customerData = customerManagementService.GetCustomers(
            SecurityHelper.Context,
            AuditHelper.Context);

            IPolicyManagementService PolicyManagementService = ServiceFactory.GetPolicyManagementService();

            PolicyBundleTransactionsResponseDto PolicyEndorsementData = PolicyManagementService.GetPolicyBundleTransactions(
            SecurityHelper.Context,
            AuditHelper.Context);
            Guid trans = PolicyManagementService.GetPolicyTransactionTypes(SecurityHelper.Context, AuditHelper.Context).PolicyTransactionTypes.Find(t => t.Code == "Cancellation").Id;
            List<Guid> policies = PolicyEndorsementData.Policies.FindAll(p => p.TransactionTypeId == trans).Select(p => p.PolicyId).ToList();

            List<PolicyBundleResponseDto> PolicyData = PolicyManagementService.GetPolicyBundles(
            SecurityHelper.Context,
            AuditHelper.Context).Policies.FindAll(c => !policies.Contains(c.Id));

            List<PolicyResponseDto> Bundle = PolicyManagementService.GetPolicys(
                  SecurityHelper.Context,
                  AuditHelper.Context).Policies;
            List<PolicyViewData> result = new List<PolicyViewData>();

            if (PolicyData.Count == 0)
            {
                return result;
            }
            foreach (PolicyBundleResponseDto item in PolicyData.FindAll(p => p.IsApproved.Equals(true)))
            {
                string Type = "";
                bool Mandatory = false;
                Guid ItemId = new Guid();
                string PolicyNo = "";
                List<PolicyResponseDto> bundle = Bundle.FindAll(p => p.PolicyBundleId == item.Id);
                foreach (var m in bundle)
                {
                    IContractManagementService ContractManagementService = ServiceFactory.GetContractManagementService();

                    ContractResponseDto Contract = ContractManagementService.GetContractById(m.ContractId,
                     SecurityHelper.Context,
                     AuditHelper.Context);
                    if (Contract.DealType == "Mandatory")
                    {
                        Mandatory = true;
                    }

                }
                if (Mandatory)
                {
                    continue;
                }
                if (bundle.Count > 0)
                {
                    Type = bundle[0].Type;
                    ItemId = bundle[0].ItemId;
                    if (bundle.Count > 1)
                    {
                        PolicyNo = "Bundled";
                    }
                    else
                        PolicyNo = bundle[0].PolicyNo;
                }
                else
                    continue;
                if (Type == "Vehicle")
                {
                    VehicleDetails = VehicleDetailsManagementService.GetVehicleDetailsById(ItemId,
                SecurityHelper.Context,
                AuditHelper.Context);
                }
                else if (Type == "B&W")
                    BrownAndWhiteDetails = BrownAndWhiteDetailsManagementService.GetBrownAndWhiteDetailsById(ItemId,
                        SecurityHelper.Context,
                        AuditHelper.Context);
                result.Add(new PolicyViewData()
                {
                    Id = item.Id,
                    CommodityTypeId = item.CommodityTypeId,
                    ProductId = item.ProductId,
                    DealerId = item.DealerId,
                    DealerLocationId = item.DealerLocationId,
                    ContractId = item.ContractId,
                    ExtensionTypeId = item.ExtensionTypeId,
                    Premium = item.Premium,
                    PremiumCurrencyTypeId = item.PremiumCurrencyTypeId,
                    DealerPaymentCurrencyTypeId = item.DealerPaymentCurrencyTypeId,
                    CustomerPaymentCurrencyTypeId = item.CustomerPaymentCurrencyTypeId,
                    CoverTypeId = item.CoverTypeId,
                    HrsUsedAtPolicySale = item.HrsUsedAtPolicySale,
                    IsPreWarrantyCheck = item.IsPreWarrantyCheck,
                    PolicySoldDate = item.PolicySoldDate,
                    SalesPersonId = item.SalesPersonId,
                    PolicyNo = PolicyNo,
                    IsSpecialDeal = item.IsSpecialDeal,
                    IsPartialPayment = item.IsPartialPayment,
                    DealerPayment = item.DealerPayment,
                    CustomerPayment = item.CustomerPayment,
                    PaymentModeId = item.PaymentModeId,
                    RefNo = item.RefNo,
                    Comment = item.Comment,
                    ItemId = ItemId,
                    Type = Type,
                    CustomerId = item.CustomerId,
                    Vehicle = VehicleDetails,
                    Customer = customerData.Customers.Find(c => c.Id == item.CustomerId.ToString()),
                    BAndW = BrownAndWhiteDetails,
                    EntryDateTime = item.EntryDateTime,
                    EntryUser = item.EntryUser,
                    IsApproved = item.IsApproved,
                    PolicyStartDate = item.PolicyStartDate,
                    PolicyEndDate = item.PolicyEndDate
                });

            }
            return result;
        }

        #endregion

        #region Renewal
        [HttpPost]
        public string RenewPolicy(JObject obj)
        {
            SavePolicyRequestDto SavePolicyRequest = obj.ToObject<SavePolicyRequestDto>();
            IPolicyManagementService PolicyManagementService = ServiceFactory.GetPolicyManagementService();
            return PolicyManagementService.RenewPolicy(SavePolicyRequest, SecurityHelper.Context, AuditHelper.Context);
        }

        [HttpPost]
        public object GetPoliciesForRenewal()
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            IVehicleDetailsManagementService VehicleDetailsManagementService = ServiceFactory.GetVehicleDetailsManagementService();
            VehicleDetailsResponseDto VehicleDetails = new VehicleDetailsResponseDto();
            IBrownAndWhiteDetailsManagementService BrownAndWhiteDetailsManagementService = ServiceFactory.GetBrownAndWhiteDetailsManagementService();
            BrownAndWhiteDetailsResponseDto BrownAndWhiteDetails = new BrownAndWhiteDetailsResponseDto();

            ICustomerManagementService customerManagementService = ServiceFactory.GetCustomerManagementService();
            CustomersResponseDto customerData = customerManagementService.GetCustomers(
            SecurityHelper.Context,
            AuditHelper.Context);

            IPolicyManagementService PolicyManagementService = ServiceFactory.GetPolicyManagementService();

            PoliciesResponseDto PolicyData = PolicyManagementService.GetPolicys(
            SecurityHelper.Context,
            AuditHelper.Context);

            PolicyData.Policies = PolicyData.Policies.FindAll(p => p.PolicyEndDate <= DateTime.Now.Date);
            List<PolicyViewData> result = new List<PolicyViewData>();

            if (PolicyData.Policies.Count == 0)
            {
                return result;
            }
            foreach (PolicyResponseDto item in PolicyData.Policies.FindAll(p => p.IsApproved.Equals(false)))
            {
                if (item.Type == "Vehicle")
                {
                    VehicleDetails = VehicleDetailsManagementService.GetVehicleDetailsById(item.ItemId,
                SecurityHelper.Context,
                AuditHelper.Context);
                }
                else if (item.Type == "B&W")
                    BrownAndWhiteDetails = BrownAndWhiteDetailsManagementService.GetBrownAndWhiteDetailsById(item.ItemId,
                        SecurityHelper.Context,
                        AuditHelper.Context);
                result.Add(new PolicyViewData()
                {
                    Id = item.Id,
                    CommodityTypeId = item.CommodityTypeId,
                    ProductId = item.ProductId,
                    DealerId = item.DealerId,
                    DealerLocationId = item.DealerLocationId,
                    ContractId = item.ContractId,
                    ExtensionTypeId = item.ExtensionTypeId,
                    Premium = item.Premium,
                    PremiumCurrencyTypeId = item.PremiumCurrencyTypeId,
                    DealerPaymentCurrencyTypeId = item.DealerPaymentCurrencyTypeId,
                    CustomerPaymentCurrencyTypeId = item.CustomerPaymentCurrencyTypeId,
                    CoverTypeId = item.CoverTypeId,
                    HrsUsedAtPolicySale = item.HrsUsedAtPolicySale,
                    IsPreWarrantyCheck = item.IsPreWarrantyCheck,
                    PolicySoldDate = item.PolicySoldDate,
                    SalesPersonId = item.SalesPersonId,
                    PolicyNo = item.PolicyNo,
                    IsSpecialDeal = item.IsSpecialDeal,
                    IsPartialPayment = item.IsPartialPayment,
                    DealerPayment = item.DealerPayment,
                    CustomerPayment = item.CustomerPayment,
                    PaymentModeId = item.PaymentModeId,
                    RefNo = item.RefNo,
                    Comment = item.Comment,
                    ItemId = item.ItemId,
                    Type = item.Type,
                    CustomerId = item.CustomerId,
                    Vehicle = VehicleDetails,
                    Customer = customerData.Customers.Find(c => c.Id == item.CustomerId.ToString()),
                    BAndW = BrownAndWhiteDetails,
                    EntryDateTime = item.EntryDateTime,
                    EntryUser = item.EntryUser,
                    IsApproved = item.IsApproved,
                    PolicyStartDate = item.PolicyStartDate,
                    PolicyEndDate = item.PolicyEndDate
                });

            }
            return result;
        }
        //[HttpPost]
        //public string RenewPolicy(JObject data)
        //{
        //    ILog logger = LogManager.GetLogger(typeof(ApiController));
        //    logger.Debug("Add Policy method!");
        //    SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

        //    PolicyHistoryRequestDto PolicyHistory = data.ToObject<PolicyHistoryRequestDto>();
        //    IPolicyManagementService PolicyManagementService = ServiceFactory.GetPolicyManagementService();
        //    PolicyHistoryRequestDto result = PolicyManagementService.AddPolicyHistory(PolicyHistory, SecurityHelper.Context, AuditHelper.Context);

        //    PolicyViewData NewPolicy = data.ToObject<PolicyViewData>();
        //    NewPolicy.Id = PolicyHistory.PolicyId;
        //    Guid Item = new Guid();
        //    if (PolicyHistory.VehicleId.ToString() == "00000000-0000-0000-0000-000000000000")
        //        Item = PolicyHistory.VehicleId;
        //    else
        //        Item = PolicyHistory.BAndWId;

        //    IExtensionTypeManagementService ExtensionType = ServiceFactory.GetExtensionTypeManagementService();
        //    ExtensionTypeResponseDto ext = ExtensionType.GetExtensionTypeById(NewPolicy.ExtensionTypeId, SecurityHelper.Context, AuditHelper.Context);

        //    DateTime Start = new DateTime();
        //    DateTime End = new DateTime();

        //    if (NewPolicy.Type == "Vehicle")
        //    {
        //        Start = NewPolicy.PolicyEndDate.AddDays(1);
        //        End = NewPolicy.PolicyEndDate.AddMonths(ext.Month);
        //    }
        //    else
        //    {
        //        Start = NewPolicy.PolicyEndDate.AddDays(1);
        //        End = NewPolicy.PolicyEndDate.AddHours(ext.Hours);
        //    }

        //    PolicyRequestDto Policy = new PolicyRequestDto()
        //    {
        //        Id = NewPolicy.Id,
        //        CommodityTypeId = NewPolicy.CommodityTypeId,
        //        ProductId = NewPolicy.ProductId,
        //        DealerId = NewPolicy.DealerId,
        //        DealerLocationId = NewPolicy.DealerLocationId,
        //        ContractId = NewPolicy.ContractId,
        //        ExtensionTypeId = NewPolicy.ExtensionTypeId,
        //        Premium = NewPolicy.Premium,
        //        PremiumCurrencyTypeId = NewPolicy.PremiumCurrencyTypeId,
        //        DealerPaymentCurrencyTypeId = NewPolicy.DealerPaymentCurrencyTypeId,
        //        CustomerPaymentCurrencyTypeId = NewPolicy.CustomerPaymentCurrencyTypeId,
        //        CoverTypeId = NewPolicy.CoverTypeId,
        //        HrsUsedAtPolicySale = NewPolicy.HrsUsedAtPolicySale,
        //        IsPreWarrantyCheck = NewPolicy.IsPreWarrantyCheck,
        //        PolicySoldDate = NewPolicy.PolicySoldDate,
        //        SalesPersonId = NewPolicy.SalesPersonId,
        //        PolicyNo = NewPolicy.PolicyNo,
        //        IsSpecialDeal = NewPolicy.IsSpecialDeal,
        //        IsPartialPayment = NewPolicy.IsPartialPayment,
        //        DealerPayment = NewPolicy.DealerPayment,
        //        CustomerPayment = NewPolicy.CustomerPayment,
        //        PaymentModeId = NewPolicy.PaymentModeId,
        //        RefNo = NewPolicy.RefNo,
        //        Comment = NewPolicy.Comment,
        //        ItemId = Item,
        //        Type = NewPolicy.Type,
        //        CustomerId = PolicyHistory.CustomerId,
        //        EntryDateTime = NewPolicy.EntryDateTime,
        //        EntryUser = NewPolicy.EntryUser,
        //        PolicyStartDate = Start,
        //        PolicyEndDate = End
        //    };

        //    PolicyRequestDto resultP = PolicyManagementService.UpdatePolicy(Policy, SecurityHelper.Context, AuditHelper.Context);
        //    logger.Info("Policy Added");
        //    if (result.PolicyInsertion)
        //    {
        //        return "OK";
        //    }
        //    else
        //    {
        //        return "Add Policy failed!";
        //    }
        //}
        [HttpPost]
        public object GetPolicyRenewalById(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            IPolicyManagementService PolicyEndorsementManagementService = ServiceFactory.GetPolicyManagementService();

            PolicyHistoryResponseDto PolicyEndorsement = PolicyEndorsementManagementService.GetPolicyHistories(
                SecurityHelper.Context,
                AuditHelper.Context).Policies.Find(h => h.Id == Guid.Parse(data["Id"].ToString()));
            if (PolicyEndorsement.ContractProducts.Count > 0)
            {
                foreach (var item in PolicyEndorsement.ContractProducts)
                {
                    IProductManagementService ProductManagementService = ServiceFactory.GetProductManagementService();
                    ProductResponseDto Product = ProductManagementService.GetProductById(item.ProductId,
                        SecurityHelper.Context,
                        AuditHelper.Context);
                    item.Name = Product.Productcode + " - " + Product.Productname;

                    if (item.PremiumCurrencyTypeId.ToString() != "00000000-0000-0000-0000-000000000000")
                    {
                        ICurrencyManagementService CurrencyManagementService = ServiceFactory.GetCurrencyManagementService();
                        CurrencyResponseDto currency = CurrencyManagementService.GetAllCurrencies(SecurityHelper.Context, AuditHelper.Context).Currencies.Find(c => c.Id == item.PremiumCurrencyTypeId);
                        item.PremiumCurrencyName = currency.Code;
                    }

                    IExtensionTypeManagementService ExtensionTypeManagementService = ServiceFactory.GetExtensionTypeManagementService();
                    ExtensionTypesResponseDto ExtensionTypes = ExtensionTypeManagementService.GetExtensionTypes(
                        SecurityHelper.Context,
                        AuditHelper.Context);
                    item.ExtensionTypes = ExtensionTypes.ExtensionTypes.FindAll(e => e.CommodityTypeId == PolicyEndorsement.CommodityTypeId);

                    IContractManagementService ContractManagementService = ServiceFactory.GetContractManagementService();
                    ContractsResponseDto Contracts = ContractManagementService.GetContracts(
                        SecurityHelper.Context,
                        AuditHelper.Context);
                    item.Contracts = Contracts.Contracts.FindAll(c => c.CommodityTypeId == PolicyEndorsement.CommodityTypeId);

                    List<Guid> ContractIds = new List<Guid>();
                    foreach (var c in Contracts.Contracts)
                    {
                        ContractIds.Add(c.Id);
                    }
                    List<ContractExtensionResponseDto> ContractExtensions = ContractManagementService.GetContractExtensions(SecurityHelper.Context, AuditHelper.Context).ContractExtensions.FindAll(c => ContractIds.Contains(c.ContractId));

                    if (Product.Productcode == "RSA")
                    {
                        List<Guid> RSAIds = new List<Guid>();
                        foreach (var r in ContractExtensions)
                        {
                            RSAIds.Add(r.RSAProviderId);
                        }
                        IRSAProviderManagementService RSAProviderManagementService = ServiceFactory.GetRSAProviderManagementService();
                        RSAProvideresResponseDto Providers = RSAProviderManagementService.GetRSAProviders(
                            SecurityHelper.Context,
                            AuditHelper.Context);
                        item.RSAProviders = Providers.RSAProviders.FindAll(r => RSAIds.Contains(r.Id));
                        item.RSA = true;
                    }
                    else
                    {
                        List<Guid> Warranty = new List<Guid>();
                        foreach (var r in ContractExtensions)
                        {
                            Warranty.Add(r.WarrantyTypeId);
                        }
                        IWarrantyTypeManagementService WarrantyTypeManagementService = ServiceFactory.GetWarrantyTypeManagementService();
                        WarrantyTypesResponseDto Covertypes = WarrantyTypeManagementService.GetWarrantyTypes(
                            SecurityHelper.Context,
                            AuditHelper.Context);
                        item.CoverTypes = Covertypes.WarrantyTypes;
                    }

                }
            }
            return PolicyEndorsement;
        }
        #endregion

        #region Inquiry
        [HttpPost]
        public object GetPoliciesForInquiry()
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            IPolicyManagementService PolicyManagementService = ServiceFactory.GetPolicyManagementService();

            PoliciesResponseDto PolicyData = PolicyManagementService.GetPolicys(
            SecurityHelper.Context,
            AuditHelper.Context);

            return PolicyData.Policies;
        }
        [HttpPost]
        public object GetPolicyTransactionById(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            PolicyTransactions result = new PolicyTransactions();
            IPolicyManagementService PolicyManagementService = ServiceFactory.GetPolicyManagementService();

            PolicyTransactionTypesResponseDto tranType = PolicyManagementService.GetPolicyTransactionTypes(SecurityHelper.Context, AuditHelper.Context);
            PolicyViewData policy = (PolicyViewData)GetPolicyById(data);

            if (policy.Type == "Vehicle")
            {
                #region Vehicle
                result.Policy = new PolicyHistoryResponseDto()
                {
                    Address1 = policy.Customer.Address1,
                    Address2 = policy.Customer.Address2,
                    Address3 = policy.Customer.Address3,
                    Address4 = policy.Customer.Address4,
                    AspirationId = policy.Vehicle.AspirationId,
                    BodyTypeId = policy.Vehicle.BodyTypeId,
                    BusinessAddress1 = policy.Customer.BusinessAddress1,
                    BusinessAddress2 = policy.Customer.BusinessAddress2,
                    BusinessAddress3 = policy.Customer.BusinessAddress3,
                    BusinessAddress4 = policy.Customer.BusinessAddress4,
                    BusinessName = policy.Customer.BusinessName,
                    BusinessTelNo = policy.Customer.BusinessTelNo,
                    CategoryId = policy.Vehicle.CategoryId,
                    CityId = policy.Customer.CityId,
                    Comment = policy.Comment,
                    CommodityTypeId = policy.CommodityTypeId,
                    ContractId = policy.ContractId,
                    CountryId = policy.Customer.CountryId,
                    CoverTypeId = policy.CoverTypeId,
                    CustomerId = policy.CustomerId,
                    CustomerPayment = policy.CustomerPayment,
                    CustomerPaymentCurrencyTypeId = policy.CustomerPaymentCurrencyTypeId,
                    CustomerTypeId = policy.Customer.CustomerTypeId,
                    CylinderCountId = policy.Vehicle.CylinderCountId,
                    DateOfBirth = policy.Customer.DateOfBirth,
                    DealerId = policy.DealerId,
                    DealerLocationId = policy.DealerLocationId,
                    DealerPayment = policy.DealerPayment,
                    DealerPaymentCurrencyTypeId = policy.DealerPaymentCurrencyTypeId,
                    DealerPrice = policy.Vehicle.DealerPrice,
                    DLIssueDate = policy.Customer.DLIssueDate,
                    DriveTypeId = policy.Vehicle.DriveTypeId,
                    Email = policy.Customer.Email,
                    EngineCapacityId = policy.Vehicle.EngineCapacityId,
                    ExtensionTypeId = policy.ExtensionTypeId,
                    FirstName = policy.Customer.FirstName,
                    FuelTypeId = policy.Vehicle.FuelTypeId,
                    Gender = policy.Customer.Gender,
                    HrsUsedAtPolicySale = policy.HrsUsedAtPolicySale,
                    Id = policy.Id,
                    IDNo = policy.Customer.IDNo,
                    IDTypeId = policy.Customer.IDTypeId,
                    IsApproved = policy.IsApproved,
                    IsPartialPayment = policy.IsPartialPayment,
                    IsPolicyExists = policy.IsPolicyExists,
                    IsPreWarrantyCheck = policy.IsPreWarrantyCheck,
                    IsSpecialDeal = policy.IsSpecialDeal,
                    ItemPurchasedDate = policy.Vehicle.ItemPurchasedDate,
                    ItemStatusId = policy.Vehicle.ItemStatusId,
                    LastName = policy.Customer.LastName,
                    MakeId = policy.Vehicle.MakeId,
                    MobileNo = policy.Customer.MobileNo,
                    ModelId = policy.Vehicle.ModelId,
                    ModelYear = policy.Vehicle.ModelYear,
                    NationalityId = policy.Customer.NationalityId,
                    OtherTelNo = policy.Customer.OtherTelNo,
                    Password = policy.Customer.Password,
                    PaymentModeId = policy.PaymentModeId,
                    PlateNo = policy.PolicyNo,
                    PolicyEndDate = policy.PolicyEndDate,
                    PolicyId = policy.Id,
                    PolicySoldDate = policy.PolicySoldDate,
                    PolicyStartDate = policy.PolicyStartDate,
                    Premium = policy.Premium,
                    PremiumCurrencyTypeId = policy.PremiumCurrencyTypeId,
                    ProductId = policy.ProductId,
                    ProfilePicture = policy.Customer.ProfilePicture,
                    RefNo = policy.RefNo,
                    SalesPersonId = policy.SalesPersonId,
                    TransmissionId = policy.Vehicle.TransmissionId,
                    UsageTypeId = policy.Customer.UsageTypeId,
                    UserName = policy.Customer.UserName,
                    Variant = policy.Vehicle.Variant,
                    VehicleId = policy.Vehicle.Id,
                    VehiclePrice = policy.Vehicle.VehiclePrice,
                    VINNo = policy.Vehicle.VINNo,
                    ContractProducts = policy.ContractProducts,
                    Discount = policy.Discount
                };
                #endregion
            }
            else
            {
                #region BAndW
                result.Policy = new PolicyHistoryResponseDto()
                {
                    Address1 = policy.Customer.Address1,
                    Address2 = policy.Customer.Address2,
                    Address3 = policy.Customer.Address3,
                    Address4 = policy.Customer.Address4,
                    BusinessAddress1 = policy.Customer.BusinessAddress1,
                    BusinessAddress2 = policy.Customer.BusinessAddress2,
                    BusinessAddress3 = policy.Customer.BusinessAddress3,
                    BusinessAddress4 = policy.Customer.BusinessAddress4,
                    BusinessName = policy.Customer.BusinessName,
                    BusinessTelNo = policy.Customer.BusinessTelNo,
                    CityId = policy.Customer.CityId,
                    Comment = policy.Comment,
                    CommodityTypeId = policy.CommodityTypeId,
                    ContractId = policy.ContractId,
                    CountryId = policy.Customer.CountryId,
                    CoverTypeId = policy.CoverTypeId,
                    CustomerId = policy.CustomerId,
                    CustomerPayment = policy.CustomerPayment,
                    CustomerPaymentCurrencyTypeId = policy.CustomerPaymentCurrencyTypeId,
                    CustomerTypeId = policy.Customer.CustomerTypeId,
                    DateOfBirth = policy.Customer.DateOfBirth,
                    DealerId = policy.DealerId,
                    DealerLocationId = policy.DealerLocationId,
                    DealerPayment = policy.DealerPayment,
                    DealerPaymentCurrencyTypeId = policy.DealerPaymentCurrencyTypeId,
                    DLIssueDate = policy.Customer.DLIssueDate,
                    Email = policy.Customer.Email,
                    ExtensionTypeId = policy.ExtensionTypeId,
                    FirstName = policy.Customer.FirstName,
                    Gender = policy.Customer.Gender,
                    HrsUsedAtPolicySale = policy.HrsUsedAtPolicySale,
                    Id = policy.Id,
                    IDNo = policy.Customer.IDNo,
                    IDTypeId = policy.Customer.IDTypeId,
                    IsApproved = policy.IsApproved,
                    IsPartialPayment = policy.IsPartialPayment,
                    IsPolicyExists = policy.IsPolicyExists,
                    IsPreWarrantyCheck = policy.IsPreWarrantyCheck,
                    IsSpecialDeal = policy.IsSpecialDeal,
                    LastName = policy.Customer.LastName,
                    MobileNo = policy.Customer.MobileNo,
                    NationalityId = policy.Customer.NationalityId,
                    OtherTelNo = policy.Customer.OtherTelNo,
                    Password = policy.Customer.Password,
                    PaymentModeId = policy.PaymentModeId,
                    PlateNo = policy.PolicyNo,
                    PolicyEndDate = policy.PolicyEndDate,
                    PolicyId = policy.Id,
                    PolicySoldDate = policy.PolicySoldDate,
                    PolicyStartDate = policy.PolicyStartDate,
                    Premium = policy.Premium,
                    PremiumCurrencyTypeId = policy.PremiumCurrencyTypeId,
                    ProductId = policy.ProductId,
                    ProfilePicture = policy.Customer.ProfilePicture,
                    RefNo = policy.RefNo,
                    SalesPersonId = policy.SalesPersonId,
                    //TransactionTypeId=policy.Vehicle.
                    UsageTypeId = policy.Customer.UsageTypeId,
                    UserName = policy.Customer.UserName,
                    AddnSerialNo = policy.BAndW.AddnSerialNo,
                    CategoryId = policy.BAndW.CategoryId,
                    DealerPrice = policy.BAndW.DealerPrice,
                    InvoiceNo = policy.BAndW.InvoiceNo,
                    ItemPrice = policy.BAndW.ItemPrice,
                    ItemPurchasedDate = policy.BAndW.ItemPurchasedDate,
                    ItemStatusId = policy.BAndW.ItemStatusId,
                    MakeId = policy.BAndW.MakeId,
                    ModelCode = policy.BAndW.ModelCode,
                    ModelId = policy.BAndW.ModelId,
                    ModelYear = policy.BAndW.ModelYear,
                    SerialNo = policy.BAndW.SerialNo,
                    BAndWId = policy.BAndW.Id
                };
                #endregion
            }
            List<PolicyTransactionResponseDto> Endorsements = PolicyManagementService.GetPolicyEndorsements(SecurityHelper.Context, AuditHelper.Context).Policies.FindAll(p => p.PolicyId == Guid.Parse(data["Id"].ToString()) && p.TransactionTypeId == tranType.PolicyTransactionTypes.Find(t => t.Code == "Endorsement").Id);
            List<PolicyTransactionResponseDto> CancelationRequests = PolicyManagementService.GetPolicyEndorsements(SecurityHelper.Context, AuditHelper.Context).Policies.FindAll(p => p.PolicyId == Guid.Parse(data["Id"].ToString()) && p.TransactionTypeId == tranType.PolicyTransactionTypes.Find(t => t.Code == "Cancelation").Id);
            List<PolicyHistoryResponseDto> Renewals = PolicyManagementService.GetPolicyHistories(SecurityHelper.Context, AuditHelper.Context).Policies.FindAll(p => p.PolicyId == Guid.Parse(data["Id"].ToString()) && p.TransactionTypeId == tranType.PolicyTransactionTypes.Find(t => t.Code == "Renewals").Id);
            PolicyHistoryResponseDto Approved = PolicyManagementService.GetPolicyHistories(SecurityHelper.Context, AuditHelper.Context).Policies.Find(p => p.PolicyId == Guid.Parse(data["Id"].ToString()) && p.TransactionTypeId == tranType.PolicyTransactionTypes.Find(t => t.Code == "Approval").Id);
            if (Approved != null)
            {
                Endorsements.Add(Approved);
            }
            result.Endorsements = Endorsements;
            result.Renewals = Renewals;
            result.Cancelations = CancelationRequests;

            return result;
        }
        #endregion

        #region TransactionType
        [HttpPost]
        public object GetPolicyTransactionTypes()
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            IPolicyManagementService PolicyManagementService = ServiceFactory.GetPolicyManagementService();

            PolicyTransactionTypesResponseDto PolicyData = PolicyManagementService.GetPolicyTransactionTypes(
            SecurityHelper.Context,
            AuditHelper.Context);

            return PolicyData.PolicyTransactionTypes;
        }
        #endregion

        #region Bulk Policy
        public Random r = new Random(1000);
        public static IList<PolicyBulk> dataList = null;
        [HttpPost]
        public string Upload()
        {
            if (HttpContext.Current.Request.Files.AllKeys.Any())
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                HttpPostedFile File = HttpContext.Current.Request.Files["file"];
                Byte[] fileByte = null;
                fileByte = new Byte[File.ContentLength];
                File.InputStream.Read(fileByte, 0, File.ContentLength);
                string excelPath = AppDomain.CurrentDomain.BaseDirectory;
                string filePath = excelPath + r.Next().ToString() + ".xlsx";
                File.SaveAs(filePath);
                dataList = ExcelReader.GetDataToList(filePath, AddPolicyBulkData);
            }
            return "Ok";
        }

        public object ConvertData()
        {
            if (dataList == null)
                return null;
            return dataList.ToList();
        }

        public string SavePolicyBulkUpload(JObject data)
        {
            List<PolicyBulk> PolicyList = data["Policies"].ToObject<List<PolicyBulk>>();

            #region Service Factory
            ICountryManagementService CountryManagementService = ServiceFactory.GetCountryManagementService();
            ICityManagementService cityManagementService = ServiceFactory.GetCityManagementService();
            ICommodityManagementService CommodityManagementService = ServiceFactory.GetCommodityManagementService();
            IItemStatusManagementService ItemStatusManagementService = ServiceFactory.GetItemStatusManagementService();
            ICommodityCategoryManagementService CommodityCategoryManagementService = ServiceFactory.GetCommodityCategoryManagementService();
            IManufacturerManagementService ManufacturerManagementService = ServiceFactory.GetManufacturerManagementService();
            IManufacturerWarrantyManagementService ManufacturerWarrantyManagementService = ServiceFactory.GetManufacturerWarrantyManagementService();
            IMakeManagementService MakeManagementService = ServiceFactory.GetMakeManagementService();
            IModelManagementService ModelManagementService = ServiceFactory.GetModelManagementService();
            IProductManagementService productManagementService = ServiceFactory.GetProductManagementService();
            IInsurerManagementService InsurerManagementService = ServiceFactory.GetInsurerManagementService();
            IDealerManagementService DealerManagementService = ServiceFactory.GetDealerManagementService();
            IDealerLocationManagementService DealerLocationManagementService = ServiceFactory.GetDealerLocationManagementService();
            ICylinderCountManagementService cylinderCountManagementService = ServiceFactory.GetCylinderCountManagementService();
            IVehicleBodyTypeManagementService vehicleBodyTypeManagementService = ServiceFactory.GetVehicleBodyTypeManagementService();
            IDriveTypeManagementService driveTypeManagementService = ServiceFactory.GetDriveTypeManagementService();
            IEngineCapacityManagementService engineCapacityManagementService = ServiceFactory.GetEngineCapacityManagementService();
            IFuelTypeManagementService fuelTypeManagementService = ServiceFactory.GetFuelTypeManagementService();
            IVehicleColorManagementService VehicleColorManagementService = ServiceFactory.GetVehicleColorManagementService();
            ITransmissionTypeManagementService TransmissionTypeManagementService = ServiceFactory.GetTransmissionTypeManagementService();
            TransmissionTechnologiesResponseDto TransmissionTechData = TransmissionTypeManagementService.GetTransmissionTechnologies(SecurityHelper.Context, AuditHelper.Context);
            IVehicleAspirationTypeManagementService VehicleAspirationTypeManagementService = ServiceFactory.GetVehicleAspirationTypeManagementService();
            IVehicleDetailsManagementService VehicleDetailsManagementService = ServiceFactory.GetVehicleDetailsManagementService();
            ICommodityUsageTypeManagementService CommodityUsageTypeManagementService = ServiceFactory.GetCommodityUsageTypeManagementService();
            IVariantManagementService VariantManagementService = ServiceFactory.GetVariantManagementService();
            ICustomerManagementService customerManagementService = ServiceFactory.GetCustomerManagementService();
            IIdTypeManagementService IdTypeManagementService = ServiceFactory.GetIdTypeManagementService();
            ICustomerTypeManagementService customertypeManagementService = ServiceFactory.GetCustomerTypeManagementService();
            INationalityManagementService nationalityManagementService = ServiceFactory.GetNationalityManagementService();
            IUsageTypeManagementService usageTypeManagementService = ServiceFactory.GetUsageTypeManagementService();
            IExtensionTypeManagementService ExtensionTypeManagementService = ServiceFactory.GetExtensionTypeManagementService();
            IContractManagementService ContractManagementService = ServiceFactory.GetContractManagementService();
            IReinsurerManagementService ReinsurerManagementService = ServiceFactory.GetReinsurerManagementService();
            IWarrantyTypeManagementService WarrantyTypeManagementService = ServiceFactory.GetWarrantyTypeManagementService();
            IPremiumBasedOnManagementService PremiumBasedOnManagementService = ServiceFactory.GetPremiumBasedOnManagementService();
            IPremiumAddonTypeManagementService PremiumAddonTypeManagementService = ServiceFactory.GetPremiumAddonTypeManagementService();
            ICurrencyManagementService CurrencyManagementService = ServiceFactory.GetCurrencyManagementService();
            IRegionManagementService RegionManagementService = ServiceFactory.GetRegionManagementService();
            IRSAProviderManagementService RSAProviderManagementService = ServiceFactory.GetRSAProviderManagementService();
            IPolicyManagementService PolicyManagementService = ServiceFactory.GetPolicyManagementService();
            IUserManagementService userManagementService = ServiceFactory.GetUserManagementService();
            IBrownAndWhiteDetailsManagementService BrownAndWhiteDetailsManagementService = ServiceFactory.GetBrownAndWhiteDetailsManagementService();
            #endregion

            #region Search Data
            CountriesResponseDto CountryData = CountryManagementService.GetAllCountries(SecurityHelper.Context, AuditHelper.Context);
            CitiesResponseDto cityData = cityManagementService.GetAllCities(SecurityHelper.Context, AuditHelper.Context);
            CurrenciesResponseDto CurrencyData = CurrencyManagementService.GetAllCurrencies(SecurityHelper.Context, AuditHelper.Context);
            CommoditiesResponseDto CommoditiesData = CommodityManagementService.GetAllCommodities(SecurityHelper.Context, AuditHelper.Context);
            ItemStatusesResponseDto ItemStatusData = ItemStatusManagementService.GetItemStatuss(SecurityHelper.Context, AuditHelper.Context);
            UsersResponseDto users = userManagementService.GetUsers(SecurityHelper.Context, AuditHelper.Context);
            RSAProvideresResponseDto RSAProvider = RSAProviderManagementService.GetRSAProviders(SecurityHelper.Context, AuditHelper.Context);
            RegionesResponseDto region = RegionManagementService.GetRegions(SecurityHelper.Context, AuditHelper.Context);
            PremiumAddonTypesResponseDto PremiumAddonTypes = PremiumAddonTypeManagementService.GetPremiumAddonTypes(SecurityHelper.Context, AuditHelper.Context);
            PremiumBasedOnesResponseDto PremiumBasedOns = PremiumBasedOnManagementService.GetPremiumBasedOns(SecurityHelper.Context, AuditHelper.Context);
            IdTypesResponseDto idTypeData = IdTypeManagementService.GetAllIdTypes(SecurityHelper.Context, AuditHelper.Context);
            CustomerTypesResponseDto customertypeData = customertypeManagementService.GetAllCustomerTypes(SecurityHelper.Context, AuditHelper.Context);
            MarritalStatusesResponseDto MaritalStatus = customerManagementService.GetMarritalStatuses(SecurityHelper.Context, AuditHelper.Context);
            NationalitiesResponseDto nationalityData = nationalityManagementService.GetAllNationalities(SecurityHelper.Context, AuditHelper.Context);
            OccupationsResponseDto Occupation = customerManagementService.GetOccupations(SecurityHelper.Context, AuditHelper.Context);
            TitlesResponseDto Titles = customerManagementService.GetTitles(SecurityHelper.Context, AuditHelper.Context);
            UsageTypesResponseDto usageTypeData = usageTypeManagementService.GetAllUsageTypes(SecurityHelper.Context, AuditHelper.Context);
            CommodityUsageTypesResponseDto CommodityUsageTypeData = CommodityUsageTypeManagementService.GetCommodityUsageTypes(SecurityHelper.Context, AuditHelper.Context);
            #endregion

            #region Validate Existing data
            ManufacturesResponseDto ManufacturerData = ManufacturerManagementService.GetAllManufatures(SecurityHelper.Context, AuditHelper.Context);
            MakesResponseDto MakeData = MakeManagementService.GetAllMakes(SecurityHelper.Context, AuditHelper.Context);
            ModelesResponseDto ModelData = ModelManagementService.GetAllModeles(SecurityHelper.Context, AuditHelper.Context);
            ProductsResponseDto productData = productManagementService.GetProducts(SecurityHelper.Context, AuditHelper.Context);
            InsurersResponseDto InsurerData = InsurerManagementService.GetInsurers(SecurityHelper.Context, AuditHelper.Context);
            DealersRespondDto DealerData = DealerManagementService.GetAllDealers(SecurityHelper.Context, AuditHelper.Context);
            DealerLocationsRespondDto DealerLocationData = DealerLocationManagementService.GetAllDealerLocations(SecurityHelper.Context, AuditHelper.Context);
            ManufacturerWarrantiesResponseDto ManufacturerWarrantyData = ManufacturerWarrantyManagementService.GetManufacturerWarranties(SecurityHelper.Context, AuditHelper.Context);
            CustomersResponseDto customerData = customerManagementService.GetCustomers(SecurityHelper.Context, AuditHelper.Context);
            ReinsurersResponseDto ReinsurerData = ReinsurerManagementService.GetReinsurers(SecurityHelper.Context, AuditHelper.Context);
            WarrantyTypesResponseDto WarrantyTypeData = WarrantyTypeManagementService.GetWarrantyTypes(SecurityHelper.Context, AuditHelper.Context);
            ContractsResponseDto ContractData = ContractManagementService.GetContracts(SecurityHelper.Context, AuditHelper.Context);
            CylinderCountsResponseDto cylinderCountData = cylinderCountManagementService.GetCylinderCounts(SecurityHelper.Context, AuditHelper.Context);
            VehicleBodyTypesResponseDto vehicleBodyTypeData = vehicleBodyTypeManagementService.GetVehicleBodyTypes(SecurityHelper.Context, AuditHelper.Context);
            DriveTypesResponseDto driveTypeData = driveTypeManagementService.GetDriveTypes(SecurityHelper.Context, AuditHelper.Context);
            EngineCapacitiesResponseDto engineCapacityData = engineCapacityManagementService.GetEngineCapacities(SecurityHelper.Context, AuditHelper.Context);
            FuelTypesResponseDto fuelTypeData = fuelTypeManagementService.GetFuelTypes(SecurityHelper.Context, AuditHelper.Context);
            TransmissionTypesResponseDto TransmissionTypeData = TransmissionTypeManagementService.GetTransmissionTypes(SecurityHelper.Context, AuditHelper.Context);
            VehicleAspirationTypesResponseDto VehicleAspirationTypeData = VehicleAspirationTypeManagementService.GetVehicleAspirationTypes(SecurityHelper.Context, AuditHelper.Context);
            VariantsResponseDto VariantData = VariantManagementService.GetVariants(SecurityHelper.Context, AuditHelper.Context);
            VehicleAllDetailsResponseDto VehicleDetailsData = VehicleDetailsManagementService.GetVehicleAllDetails(SecurityHelper.Context, AuditHelper.Context);
            ExtensionTypesResponseDto ExtensionTypes = ExtensionTypeManagementService.GetExtensionTypes(SecurityHelper.Context, AuditHelper.Context);
            ContractExtensionsResponseDto ContractExtensionData = ContractManagementService.GetContractExtensions(SecurityHelper.Context, AuditHelper.Context);
            BrownAndWhiteAllDetailsResponseDto BrownAndWhiteDetailsData = BrownAndWhiteDetailsManagementService.GetBrownAndWhiteAllDetails(SecurityHelper.Context, AuditHelper.Context);
            #endregion

            foreach (var item in PolicyList)
            {
                try
                {
                    DateTime d = DateTime.Parse(item.DateOfBirth);
                    d = DateTime.Parse(item.DealEndDate);
                    d = DateTime.Parse(item.DealStartDate);
                    d = DateTime.Parse(item.ManufacturerWarantyStartDate);
                    d = DateTime.Parse(item.ModelRiskStartDate);
                    d = DateTime.Parse(item.PolicyEndDate);
                    d = DateTime.Parse(item.PolicySoldDate);
                    d = DateTime.Parse(item.PolicyStartDate);
                    d = DateTime.Parse(item.PurchaseDate);
                    if (item.CommodityType == "Äutomobile")
                        d = DateTime.Parse(item.VehicleRegistrationDate);
                    d = DateTime.Parse(item.CustomerIdIssueDate);
                }
                catch
                {
                    return "Date fields should be valid";
                }
                try
                {
                    Decimal val = Convert.ToDecimal(item.BasicPremium);
                    val = Convert.ToDecimal(item.DealerPayment);
                    val = Convert.ToDecimal(item.DealerPrice);
                    val = Convert.ToDecimal(item.Discount);
                    if (item.CommodityType == "Äutomobile")
                    {
                        val = Convert.ToDecimal(item.FourByFourPremium);
                        val = Convert.ToDecimal(item.SportsPremium);
                    }
                    val = Convert.ToDecimal(item.GrossPremium);
                    val = Convert.ToDecimal(item.ItemPrice);
                    val = Convert.ToDecimal(item.MaximumPremium);
                    val = Convert.ToDecimal(item.MinimumPremium);
                    val = Convert.ToDecimal(item.OtherPremium);
                    val = Convert.ToDecimal(item.Premium);
                    val = Convert.ToDecimal(item.PremiumTotal);
                    val = Convert.ToDecimal(item.ClaimLimitation);
                    val = Convert.ToDecimal(item.LiabilityLimitation);
                    val = Convert.ToDecimal(item.AdditionalPremium);
                    val = Convert.ToDecimal(item.CustomerPayment);
                }
                catch
                {
                    return "Currency fields should be valid";

                }
                try
                {
                    int val = Convert.ToInt32(item.CategoryLength);
                    if (item.CommodityType == "Automobile")
                    {
                        val = Convert.ToInt32(item.ManufacturerWarantyKm);
                        val = Convert.ToInt32(item.ExtensionKm);
                        val = Convert.ToInt32(item.VehicleCylinderCount);
                        val = Convert.ToInt32(item.VehicleModelYear);
                    }
                    else
                    {
                        val = Convert.ToInt32(item.ElectronicModelYear);
                        val = Convert.ToInt32(item.ExtensionHours);
                    }
                    val = Convert.ToInt32(item.ExtensionMonth);
                    val = Convert.ToInt32(item.ManufacturerWarantyMonth);
                    val = Convert.ToInt32(item.HrsUsedAtPolicySale);
                }
                catch
                {
                    return "Please enter Integer for the following fields Electronic Model Year / Extension Hours / Extension Km / Extension Month / Manufacturer Waranty Km / Manufacturer Waranty Measure Type / Manufacturer Waranty Month / Vehicle Cylinder Count / Vehicle Model Year / Category Length / Hrs Used At Policy Sale";

                }
                if (!EmailValidator(item.CustomerEmail) || !EmailValidator(item.DealerBranchSalesEmail) || !EmailValidator(item.DealerBranchServiceEmail))
                {
                    return "Please enter valid emails";
                }
                if (cityData.Cities.FindAll(c => c.CityName == item.CustomerCity || c.CityName == item.DealerBranchCity).Count == 0)
                {
                    return "Please Enter a Valid City";
                }
                if (CountryData.Countries.FindAll(c => c.CountryName == item.CustomerCountry ||
                    c.CountryName == item.DealerCountry ||
                    c.CountryName == item.InsurerCountry ||
                    c.CountryName == item.ManufacturerWarantyCountry
                ).Count == 0)
                {
                    return "Please Enter a Valid Country";
                }
                if (item.CommodityType == "Automobile" && CountryData.Countries.FindAll(c => c.CountryName == item.VehiclelCountryOfOrigine).Count == 0)
                {
                    return "Please Enter a Valid Country";
                }
                if (item.CommodityType == "Electronic" && CountryData.Countries.FindAll(c => c.CountryName == item.ElectronicCountryOfOrigine).Count == 0)
                {
                    return "Please Enter a Valid Country";
                }
                if (CurrencyData.Currencies.FindAll(c => c.Code == item.DealerCurrency ||
                    c.Code == item.DealerPaymentCurrencyType ||
                    c.Code == item.PremiumCurrencyType).Count == 0)
                {
                    return "Please Enter a Valid Currency";
                }
                if (MaritalStatus.MarritalStatuses.FindAll(c => c.Name == item.CustomerMaritalStatus || c.Name == "").Count == 0)
                {
                    return "Please Enter a Valid marital status or enter blank";
                }
                if (Occupation.Occupations.FindAll(c => c.Name == item.CustomerOccupation || c.Name == "").Count == 0)
                {
                    return "Please Enter a Valid Occupation or enter blank";
                }
                if (Titles.Titles.FindAll(c => c.Name == item.CustomerTitle || c.Name == "").Count == 0)
                {
                    return "Please Enter a Valid Title or enter blank";
                }
                if (customertypeData.CustomerTypes.FindAll(c => c.CustomerTypeName == item.CustomerType).Count == 0)
                {
                    return "Please Enter a Valid Customer Type: Corporate / Individual";
                }
                if (usageTypeData.UsageTypes.FindAll(c => c.UsageTypeName == item.UsageType).Count == 0)
                {
                    return "Please Enter a Valid Customer Type: Private / Commercial";
                }
                if (nationalityData.Nationalities.FindAll(c => c.NationalityName == item.Nationality).Count == 0)
                {
                    return "Please Enter a Valid Nationality";
                }
                if (CommodityUsageTypeData.CommodityUsageTypes.FindAll(c => c.Name == item.CommodityUsageType).Count == 0)
                {
                    return "Please Enter a Valid Commodity Usage Types: Residence / Commercial";
                }
                if (item.CustomerGender.ToLower() != "f" && item.CustomerGender.ToLower() != "m")
                {
                    return "Please Enter F / M for Gender";
                }
                if (!(
                    (item.AutoApproval.ToLower() == "yes" || item.AutoApproval.ToLower() == "no") &&
                     (item.AutoRenewal.ToLower() == "yes" || item.AutoRenewal.ToLower() == "no") &&
                     (item.PromotionalDeal.ToLower() == "yes" || item.PromotionalDeal.ToLower() == "no") &&
                     (item.DiscountAvailable.ToLower() == "yes" || item.DiscountAvailable.ToLower() == "no") &&
                     (item.SpecialDeal.ToLower() == "yes" || item.SpecialDeal.ToLower() == "no") &&
                     (item.PartialPayment.ToLower() == "yes" || item.PartialPayment.ToLower() == "no") &&
                     (item.DealerPolicy.ToLower() == "yes" || item.DealerPolicy.ToLower() == "no")
                    ))
                {
                    return "Please Enter Yes / No for fields 'Auto Approval / Auto Renewal / Customer Payment / Promotional Deal / Discount Available / Special Deal / Partial Payment / Dealer Policy' ";
                }
                if (item.Status.ToLower() != "new" && item.Status.ToLower() != "used")
                {
                    return "Status should be New / Used";
                }
                if (item.CommodityType.ToLower() != "automobile" && item.CommodityType.ToLower() != "electronic")
                {
                    return "Commodity Type should be Automobile / Electronic";
                }
                if (users.Users.FindAll(c => c.FirstName + ' ' + c.LastName == item.SalesPerson).Count == 0)
                {
                    return "Please Enter a Valid sales person";
                }
                if (idTypeData.IdTypes.FindAll(c => c.IdTypeName == item.CustomerIdType).Count == 0)
                {
                    return "Please Enter a Valid Id Type";
                }

            }//Validation
            foreach (var item in PolicyList)
            {
                Guid CommodityTypeId = CommoditiesData.Commmodities.Find(c => c.CommodityTypeDescription == item.CommodityType).CommodityTypeId;
                List<Guid> CommodityTypes = new List<Guid>();
                CommodityTypes.Add(CommodityTypeId);

                Guid ItemStatus = ItemStatusData.ItemStatuss.Find(i => i.Status == item.Status).Id;

                #region Validate Existing data
                CommodityCategoriesRespondDto CommodityCategoryData = CommodityCategoryManagementService.GetCommodityCategories(CommodityTypeId, SecurityHelper.Context, AuditHelper.Context);
                #endregion

                #region Common Data Save

                var Category = new Guid();
                if (CommodityCategoryData.CommodityCategories.FindAll(c => c.CommodityCategoryDescription == item.CategoryName).Count == 0)
                {
                    Category = CommodityCategoryManagementService.AddCommodityCategory(new CommodityCategoryRequestDto()
                    {
                        CommodityCategoryCode = "",
                        CommodityCategoryDescription = item.CategoryName,
                        CommodityTypeId = CommodityTypeId,
                        Length = Convert.ToInt32(item.CategoryLength)
                    }, SecurityHelper.Context, AuditHelper.Context).CommodityCategoryId;

                }
                else
                {
                    Category = CommodityCategoryData.CommodityCategories.Find(c => c.CommodityCategoryDescription == item.CategoryName).CommodityCategoryId;
                }

                var Manufacturer = new Guid();
                if (ManufacturerData.Manufactures.FindAll(m => m.ManufacturerName == item.Manufacturer).Count == 0)
                {
                    Manufacturer = ManufacturerManagementService.AddManufacturer(new ManufacturerRequestDto()
                    {
                        ComodityTypes = CommodityTypes,
                        ManufacturerName = item.Manufacturer,
                        ManufacturerCode = item.ManufacturerCode,
                        IsWarrentyGiven = false,
                        IsActive = true
                    }, SecurityHelper.Context, AuditHelper.Context).Id;

                }
                else
                {
                    Manufacturer = ManufacturerData.Manufactures.Find(m => m.ManufacturerName == item.Manufacturer).Id;
                }

                List<Guid> Makes = new List<Guid>();
                var Make = new Guid();
                if (MakeData.Makes.FindAll(m => m.MakeName == item.MakeName).Count == 0)
                {
                    Make = MakeManagementService.AddMake(new MakeRequestDto()
                    {
                        CommodityTypeId = CommodityTypeId,
                        IsActive = true,
                        MakeCode = item.MakeCode,
                        MakeName = item.MakeName,
                        ManufacturerId = Manufacturer
                    }, SecurityHelper.Context, AuditHelper.Context).Id;
                    Makes.Add(Make);
                }
                else
                {
                    Make = MakeData.Makes.Find(m => m.MakeName == item.MakeName).Id;
                    Makes.Add(Make);
                }

                var Model = new Guid();
                if (ModelData.Modeles.FindAll(m => m.ModelName == item.ModelName).Count == 0)
                {
                    Model = ModelManagementService.AddModel(new ModelRequestDto()
                    {
                        AdditionalPremium = item.AdditionalPremium != "" ? true : false,
                        CategoryId = Category,
                        ContryOfOrigineId = CountryData.Countries.Find(c => c.CountryName == item.VehiclelCountryOfOrigine).Id,
                        IsActive = true,
                        MakeId = Make,
                        ModelCode = item.ModelCode,
                        ModelName = item.ModelName,
                        WarantyGiven = false,
                        NoOfDaysToRiskStart = Convert.ToInt32(item.ModelRiskStartDate)
                    }, SecurityHelper.Context, AuditHelper.Context).Id;
                }
                else
                {
                    Model = ModelData.Modeles.Find(m => m.ModelName == item.ModelName).Id;
                }

                List<Guid> Products = new List<Guid>();
                var Product = new Guid();
                if (productData.Products.FindAll(m => m.Productname == item.ProductName).Count == 0)
                {
                    Product = productManagementService.AddProduct(new ProductRequestDto()
                    {
                        CommodityTypeId = CommodityTypeId,
                        Isactive = true,
                        Productcode = item.ProductCode,
                        Productname = item.ProductName
                    }, SecurityHelper.Context, AuditHelper.Context).Id;
                    Products.Add(Product);
                }
                else
                {
                    Product = productData.Products.Find(m => m.Productname == item.ProductName).Id;
                    Products.Add(Product);
                }

                List<Guid> Countries = new List<Guid>();
                try
                {
                    Countries.Add(CountryData.Countries.Find(c => c.CountryCode == item.InsurerCountry).Id);
                }
                catch
                {
                }
                var Insurer = new Guid();
                if (InsurerData.Insurers.FindAll(m => m.InsurerFullName == item.InsurerName || m.InsurerShortName == item.InsurerName).Count == 0)
                {
                    Insurer = InsurerManagementService.AddInsurer(new InsurerRequestDto()
                    {
                        CommodityTypes = CommodityTypes,
                        InsurerCode = item.InsurerCode,
                        InsurerFullName = item.InsurerName,
                        Products = Products,
                        Countries = Countries
                    }, SecurityHelper.Context, AuditHelper.Context).Id;
                }
                else
                {
                    Insurer = InsurerData.Insurers.Find(m => m.InsurerFullName == item.InsurerName || m.InsurerShortName == item.InsurerName).Id;
                }

                var Dealer = new Guid();
                Guid DealerCountry = new Guid();
                try
                {
                    DealerCountry = CountryData.Countries.Find(c => c.CountryCode == item.DealerCountry).Id;
                }
                catch { }
                Guid DealerCurrency = new Guid();
                try
                {
                    DealerCurrency = CurrencyData.Currencies.Find(c => c.Code == item.DealerCurrency).Id;
                }
                catch { }
                if (DealerData.Dealers.FindAll(m => m.DealerName == item.DealerName).Count == 0)
                {
                    DealerManagementService.AddDealer(new DealerRequestDto()
                    {
                        CommodityTypeId = CommodityTypeId,
                        CountryId = DealerCountry,
                        CurrencyId = DealerCurrency,
                        DealerName = item.DealerName,
                        InsurerId = Insurer,
                        IsActive = true,
                        Makes = Makes,
                        Type = "Dealer"
                    }, SecurityHelper.Context, AuditHelper.Context);
                }
                else
                {
                    Dealer = DealerData.Dealers.Find(m => m.DealerName == item.DealerName).Id;
                }

                var Location = new Guid();
                if (DealerLocationData.DealerLocations.FindAll(m => m.Location == item.DealerBranch).Count == 0)
                {
                    Location = DealerLocationManagementService.AddDealerLocation(new DealerLocationRequestDto()
                    {
                        DealerId = Dealer,
                        HeadOfficeBranch = true,
                        Location = item.DealerBranch,
                        CityId = cityData.Cities.Find(c => c.CityName == item.DealerBranchCity).Id,
                        SalesContactPerson = item.DealerBranchSalesContractPerson,
                        SalesEmail = item.DealerBranchSalesEmail,
                        SalesFax = item.DealerBranchSalesFax,
                        SalesTelephone = item.DealerBranchSalesTelephoneNo,
                        ServiceContactPerson = item.DealerBranchServiceContractPerson,
                        ServiceEmail = item.DealerBranchServiceEmail,
                        ServiceFax = item.DealerBranchServiceFax,
                        ServiceTelephone = item.DealerBranchServiceTelephoneNo,
                    }, SecurityHelper.Context, AuditHelper.Context).Id;
                }
                else
                {
                    Location = DealerLocationData.DealerLocations.Find(m => m.Location == item.DealerBranch).Id;
                }

                var ManWarranty = new Guid();
                //if (ManufacturerWarrantyData.ManufacturerWarranties.FindAll(m => m.WarrantyName == item.ManufacturerWaranty && m.ModelId == Model).Count == 0)
                //{
                //    ManWarranty = ManufacturerWarrantyManagementService.AddManufacturerWarranty(new ManufacturerWarrantyRequestDto()
                //    {
                //        ApplicableFrom = DateTime.Parse(item.ManufacturerWarantyStartDate),
                //        MakeId = Make,
                //        //CountryId = CountryData.Countries.Find(c => c.CountryName == item.ManufacturerWarantyCountry).Id,
                //        //ModelId = Model,
                //        WarrantyKm = Convert.ToInt32(item.ManufacturerWarantyKm),
                //        WarrantyMonths = Convert.ToInt32(item.ManufacturerWarantyMonth),
                //        WarrantyName = item.ManufacturerWaranty
                //    }, SecurityHelper.Context, AuditHelper.Context).Id;
                //}
                //else
                //{
                //    ManWarranty = ManufacturerWarrantyData.ManufacturerWarranties.Find(m => m.WarrantyName == item.ManufacturerWaranty && m.ModelId == Model).Id;
                //}

                var Customer = new Guid();
                if (customerData.Customers.FindAll(m => m.FirstName + " " + m.LastName == item.CustomerFirstName + " " + item.CustomerLastName).Count == 0)
                {
                    Customer = Guid.Parse(customerManagementService.AddCustomer(new CustomerRequestDto()
                    {
                        Id = "00000000-0000-0000-0000-000000000000",
                        Address1 = item.CustomerAddress,
                        BusinessAddress1 = item.BusinessAddress,
                        BusinessName = item.BusinessName,
                        BusinessTelNo = item.BusinessPhoneNo,
                        CityId = cityData.Cities.Find(c => c.CityName == item.CustomerCity).Id,
                        CountryId = CountryData.Countries.Find(c => c.CountryName == item.CustomerCountry).Id,
                        CustomerTypeId = customertypeData.CustomerTypes.Find(c => c.CustomerTypeName == item.CustomerType).Id,
                        DateOfBirth = DateTime.Parse(item.DateOfBirth),
                        DLIssueDate = DateTime.Parse(item.CustomerIdIssueDate),
                        Email = item.CustomerEmail,
                        FirstName = item.CustomerFirstName,
                        Gender = item.CustomerGender == "Female" && item.CustomerGender == "F" ? 'F' : 'M',
                        IDNo = item.CustomerIdNo,
                        IDTypeId = idTypeData.IdTypes.Find(i => i.IdTypeName == item.CustomerIdType).Id,
                        IsActive = true,
                        LastName = item.CustomerLastName,
                        MaritalStatusId = MaritalStatus.MarritalStatuses.Find(m => m.Name == item.CustomerMaritalStatus).Id,
                        MobileNo = item.CustomerMobileNo,
                        NationalityId = nationalityData.Nationalities.Find(n => n.NationalityName == item.Nationality).Id,
                        OccupationId = Occupation.Occupations.Find(o => o.Name == item.CustomerOccupation).Id,
                        OtherTelNo = "",
                        TitleId = Titles.Titles.Find(t => t.Name == item.CustomerTitle).Id,
                        UsageTypeId = usageTypeData.UsageTypes.Find(u => u.UsageTypeName == item.UsageType).Id
                    }, SecurityHelper.Context, AuditHelper.Context).Id);
                }
                else
                {
                    Customer = Guid.Parse(customerData.Customers.Find(m => m.FirstName + " " + m.LastName == item.CustomerFirstName + " " + item.CustomerLastName).Id);
                }

                var Reinsurer = new Guid();
                if (ReinsurerData.Reinsurers.FindAll(m => m.ReinsurerName == item.ReinsurerName).Count == 0)
                {
                    Reinsurer = ReinsurerManagementService.AddReinsurer(new ReinsurerRequestDto()
                    {
                        ReinsurerName = item.ReinsurerName,
                        ReinsurerCode = item.ReinsurerCode
                    }, SecurityHelper.Context, AuditHelper.Context).Id;
                }
                else
                {
                    Reinsurer = ReinsurerData.Reinsurers.Find(m => m.ReinsurerName == item.ReinsurerName).Id;
                }

                var WarrantyType = new Guid();
                if (WarrantyTypeData.WarrantyTypes.FindAll(m => m.WarrantyTypeDescription == item.CoverType).Count == 0)
                {
                    WarrantyType = WarrantyTypeManagementService.AddWarrantyType(new WarrantyTypeRequestDto()
                    {
                        WarrantyTypeDescription = item.CoverType,
                    }, SecurityHelper.Context, AuditHelper.Context).Id;
                }
                else
                {
                    WarrantyType = WarrantyTypeData.WarrantyTypes.Find(m => m.WarrantyTypeDescription == item.CoverType).Id;
                }

                var Contract = new Guid();
                if (ContractData.Contracts.FindAll(m => m.DealName == item.ContractDealName).Count == 0)
                {
                    Contract = ContractManagementService.AddContract(new ContractRequestDto()
                    {
                        ClaimLimitation = Convert.ToInt32(item.ClaimLimitation),
                        CommodityTypeId = CommodityTypeId,
                        CommodityUsageTypeId = CommodityUsageTypeData.CommodityUsageTypes.Find(c => c.Name == item.CommodityUsageType).Id,
                        CountryId = CountryData.Countries.Find(c => c.CountryName == item.CustomerCountry).Id,
                        DealerId = Dealer,
                        DealName = item.DealerName,
                        DealType = item.ContractDealType,
                        DiscountAvailable = item.DiscountAvailable == "Yes" ? true : false,
                        EndDate = DateTime.Parse(item.DealEndDate),
                        GrossPremium = Convert.ToDecimal(item.GrossPremium),
                        InsurerId = Insurer,
                        IsActive = true,
                        IsAutoRenewal = item.AutoRenewal == "Yes" ? true : false,
                        IsPromotional = item.PromotionalDeal == "Yes" ? true : false,
                        ItemStatusId = ItemStatus,
                        LiabilityLimitation = Convert.ToDecimal(item.LiabilityLimitation),
                        PremiumTotal = Convert.ToDecimal(item.PremiumTotal),
                        ProductId = Product,
                        ReinsurerId = Reinsurer,
                        StartDate = DateTime.Parse(item.DealEndDate),
                        WarrantyTypeId = WarrantyType
                    }, SecurityHelper.Context, AuditHelper.Context).Id;
                }
                else
                {
                    Contract = ContractData.Contracts.Find(m => m.DealName == item.ContractDealName).Id;
                }

                #endregion

                if (item.CommodityType == "Automobile")
                {
                    var CylinderCount = new Guid();
                    if (cylinderCountData.CylinderCounts.FindAll(m => m.Count == item.VehicleCylinderCount).Count == 0)
                    {
                        CylinderCount = cylinderCountManagementService.AddCylinderCount(new CylinderCountRequestDto()
                        {
                            Count = item.VehicleCylinderCount
                        }, SecurityHelper.Context, AuditHelper.Context).Id;
                    }
                    else
                    {
                        CylinderCount = cylinderCountData.CylinderCounts.Find(m => m.Count == item.VehicleCylinderCount).Id;
                    }

                    var BodyType = new Guid();
                    if (vehicleBodyTypeData.VehicleBodyTypes.FindAll(m => m.VehicleBodyTypeDescription == item.VehicleBodyType).Count == 0)
                    {
                        BodyType = vehicleBodyTypeManagementService.AddVehicleBodyType(new VehicleBodyTypeRequestDto()
                        {
                            VehicleBodyTypeDescription = item.VehicleBodyType
                        }, SecurityHelper.Context, AuditHelper.Context).Id;
                    }
                    else
                    {
                        BodyType = vehicleBodyTypeData.VehicleBodyTypes.Find(m => m.VehicleBodyTypeDescription == item.VehicleBodyType).Id;
                    }

                    var DriveType = new Guid();
                    if (driveTypeData.DriveTypes.FindAll(m => m.Type == item.VehicleDriveType).Count == 0)
                    {
                        DriveType = driveTypeManagementService.AddDriveType(new DriveTypeRequestDto()
                        {
                            DriveTypeDescription = item.VehicleDriveType
                        }, SecurityHelper.Context, AuditHelper.Context).Id;
                    }
                    else
                    {
                        DriveType = driveTypeData.DriveTypes.Find(m => m.Type == item.VehicleDriveType).Id;
                    }

                    var EngineCapacity = new Guid();
                    if (engineCapacityData.EngineCapacities.FindAll(m => m.EngineCapacityNumber.ToString() == item.VehicleEngineCapacity).Count == 0)
                    {
                        EngineCapacity = engineCapacityManagementService.AddEngineCapacity(new EngineCapacityRequestDto()
                        {
                            EngineCapacityNumber = Convert.ToDecimal(item.VehicleEngineCapacity),
                            MesureType = item.EngineCapacityMeasureType
                        }, SecurityHelper.Context, AuditHelper.Context).Id;
                    }
                    else
                    {
                        EngineCapacity = engineCapacityData.EngineCapacities.Find(m => m.EngineCapacityNumber.ToString() == item.VehicleEngineCapacity).Id;
                    }

                    var FuelType = new Guid();
                    if (fuelTypeData.FuelTypes.FindAll(m => m.FuelTypeDescription == item.VehicleFuelType).Count == 0)
                    {
                        FuelType = fuelTypeManagementService.AddFuelType(new FuelTypeRequestDto()
                        {
                            FuelTypeDescription = item.VehicleFuelType
                        }, SecurityHelper.Context, AuditHelper.Context).FuelTypeId;
                    }
                    else
                    {
                        FuelType = fuelTypeData.FuelTypes.Find(m => m.FuelTypeDescription == item.VehicleFuelType).FuelTypeId;
                    }

                    List<string> Tech = new List<string>();
                    Tech.Add(TransmissionTechData.TransmissionTechnologies.Find(t => t.Name == item.VehicleTransmissionTechnology).Name);
                    var Transmission = new Guid();
                    if (TransmissionTypeData.TransmissionTypes.FindAll(m => m.TransmissionTypeCode == item.VehicleTransmission).Count == 0)
                    {
                        Transmission = TransmissionTypeManagementService.AddTransmissionType(new TransmissionTypeRequestDto()
                        {
                            TransmissionTypeCode = item.VehicleTransmission,
                            Technology = Tech,
                        }, SecurityHelper.Context, AuditHelper.Context).Id;
                    }
                    else
                    {
                        Transmission = TransmissionTypeData.TransmissionTypes.Find(m => m.TransmissionTypeCode == item.VehicleTransmission).Id;
                    }

                    var Aspiration = new Guid();
                    if (VehicleAspirationTypeData.VehicleAspirationTypes.FindAll(m => m.AspirationTypeCode == item.VehicleAspiration).Count == 0)
                    {
                        Aspiration = VehicleAspirationTypeManagementService.AddVehicleAspirationType(new VehicleAspirationTypeRequestDto()
                        {
                            AspirationTypeCode = item.VehicleAspiration
                        }, SecurityHelper.Context, AuditHelper.Context).Id;
                    }
                    else
                    {
                        Aspiration = VehicleAspirationTypeData.VehicleAspirationTypes.Find(m => m.AspirationTypeCode == item.VehicleAspiration).Id;
                    }

                    List<Guid> Aspirations = new List<Guid>();
                    Aspirations.Add(Aspiration);
                    List<Guid> BodyTypes = new List<Guid>();
                    BodyTypes.Add(BodyType);
                    List<Guid> Countrys = new List<Guid>();
                    Countrys.Add(CountryData.Countries.Find(c => c.CountryName == item.VehiclelCountryOfOrigine).Id);
                    List<Guid> DriveTypes = new List<Guid>();
                    DriveTypes.Add(DriveType);
                    List<Guid> FuelTypes = new List<Guid>();
                    FuelTypes.Add(FuelType);
                    List<Guid> Transmissions = new List<Guid>();
                    Transmissions.Add(Transmission);

                    var Variant = new Guid();
                    if (VariantData.Variants.FindAll(m => m.VariantName == item.VehicleVariant).Count == 0)
                    {
                        Variant = VariantManagementService.AddVariant(new VariantRequestDto()
                        {
                            Aspirations = Aspirations,
                            BodyCode = "",
                            BodyTypes = BodyTypes,
                            CommodityTypeId = CommodityTypeId,
                            Countrys = Countrys,
                            CylinderCountId = CylinderCount,
                            DriveTypes = DriveTypes,
                            EngineCapacityId = EngineCapacity,
                            FromModelYear = 0,
                            FuelTypes = FuelTypes,
                            IsActive = true,
                            ModelId = Model,
                            ToModelYear = 0,
                            Transmissions = Transmissions,
                            VariantName = item.VehicleVariant
                        }, SecurityHelper.Context, AuditHelper.Context).Id;
                    }
                    else
                    {
                        Variant = VariantData.Variants.Find(m => m.VariantName == item.VehicleVariant).Id;
                    }

                    var Vehicle = new Guid();
                    if (VehicleDetailsData.VehicleAllDetails.FindAll(m => m.VINNo == item.VehicleVinNo).Count == 0)
                    {
                        Vehicle = VehicleDetailsManagementService.AddVehicleDetails(new VehicleDetailsRequestDto()
                        {
                            AspirationId = Aspiration,
                            BodyTypeId = BodyType,
                            CategoryId = Category,
                            CommodityUsageTypeId = CommodityUsageTypeData.CommodityUsageTypes.Find(c => c.Name == item.CommodityUsageType).Id,
                            CylinderCountId = CylinderCount,
                            DealerPrice = Convert.ToDecimal(item.DealerPrice),
                            DriveTypeId = DriveType,
                            EngineCapacityId = EngineCapacity,
                            FuelTypeId = FuelType,
                            ItemPurchasedDate = DateTime.Parse(item.PurchaseDate),
                            ItemStatusId = ItemStatus,
                            MakeId = Make,
                            ModelId = Model,
                            ModelYear = item.VehicleModelYear,
                            PlateNo = item.VehiclePlateNo,
                            TransmissionId = Transmission,
                            VehiclePrice = Convert.ToDecimal(item.ItemPrice),
                            VINNo = item.VehicleVinNo,
                            Variant = Variant
                        }, SecurityHelper.Context, AuditHelper.Context).Id;
                    }
                    else
                    {
                        Vehicle = VehicleDetailsData.VehicleAllDetails.Find(m => m.VINNo == item.VehicleVinNo).Id;
                    }

                    var ExtensionType = new Guid();
                    if (ExtensionTypes.ExtensionTypes.FindAll(m => m.ExtensionName == item.ExtensionName).Count == 0)
                    {
                        ExtensionType = ExtensionTypeManagementService.AddExtensionType(new ExtensionTypeRequestDto()
                        {
                            CommodityTypeId = CommodityTypeId,
                            ExtensionName = item.ExtensionName,
                            Hours = Convert.ToInt32(item.ExtensionHours),
                            Km = Convert.ToInt32(item.ExtensionKm),
                            Month = Convert.ToInt32(item.ExtensionMonth),
                            ProductId = Product
                        }, SecurityHelper.Context, AuditHelper.Context).Id;
                    }
                    else
                    {
                        ExtensionType = ExtensionTypes.ExtensionTypes.Find(m => m.ExtensionName == item.ExtensionName).Id;
                    }

                    List<Guid> CylinderCounts = new List<Guid>();
                    CylinderCounts.Add(CylinderCount);
                    List<Guid> EngineCapacities = new List<Guid>();
                    EngineCapacities.Add(EngineCapacity);
                    List<Guid> Models = new List<Guid>();
                    Models.Add(Model);

                    List<ContractExtensionsPremiumAddonRequestDto> addons = new List<ContractExtensionsPremiumAddonRequestDto>();
                    addons.Add(new ContractExtensionsPremiumAddonRequestDto()
                    {
                        PremiumAddonTypeId = PremiumAddonTypes.PremiumAddonTypes.Find(p => p.Description == "Basic Premium").Id,
                        Value = Convert.ToInt32(item.BasicPremium)
                    });
                    addons.Add(new ContractExtensionsPremiumAddonRequestDto()
                    {
                        PremiumAddonTypeId = PremiumAddonTypes.PremiumAddonTypes.Find(p => p.Description == "Other Premium").Id,
                        Value = Convert.ToInt32(item.OtherPremium)
                    });
                    addons.Add(new ContractExtensionsPremiumAddonRequestDto()
                    {
                        PremiumAddonTypeId = PremiumAddonTypes.PremiumAddonTypes.Find(p => p.Description == "4x4 premium").Id,
                        Value = Convert.ToInt32(item.FourByFourPremium)
                    });
                    addons.Add(new ContractExtensionsPremiumAddonRequestDto()
                    {
                        PremiumAddonTypeId = PremiumAddonTypes.PremiumAddonTypes.Find(p => p.Description == "Sport Premium").Id,
                        Value = Convert.ToInt32(item.SportsPremium)
                    });
                    addons.Add(new ContractExtensionsPremiumAddonRequestDto()
                    {
                        PremiumAddonTypeId = PremiumAddonTypes.PremiumAddonTypes.Find(p => p.Description == "Additional Premium").Id,
                        Value = Convert.ToInt32(item.AdditionalPremium)
                    });

                    var ContractExtension = new Guid();
                    Guid PremiumBasedOnsId = new Guid();
                    try
                    {
                        PremiumBasedOnsId = PremiumBasedOns.PremiumBasedOns.Find(p => p.Description == item.PremiumBasedOnDescription).Id;
                    }
                    catch { }
                    Guid CurrencyDataId = new Guid();
                    try
                    {
                        CurrencyDataId = CurrencyData.Currencies.Find(c => c.Code == item.PremiumCurrencyType).Id;
                    }
                    catch { }
                    Guid RegionId = new Guid();
                    try
                    {
                        RegionId = region.Regions.Find(r => r.RegionName == item.Region).Id;
                    }
                    catch { }
                    Guid RSAProviderId = new Guid();
                    try
                    {
                        RSAProviderId = RSAProvider.RSAProviders.Find(r => r.ProviderName == item.RSAProviderName).Id;
                    }
                    catch { }
                    if (ContractExtensionData.ContractExtensions.FindAll(m => m.ExtensionTypeId == ExtensionType && m.ContractId == Contract).Count == 0)
                    {
                        ContractExtension = ContractManagementService.AddContractExtensions(new ContractExtensionsRequestDto()
                        {
                            ContractId = Contract,
                            CylinderCounts = CylinderCounts,
                            EngineCapacities = EngineCapacities,
                            ExtensionTypeId = ExtensionType,
                            GrossPremium = Convert.ToDecimal(item.GrossPremium),
                            Makes = Makes,
                            ManufacturerWarranty = item.ManufacturerWaranty == "Yes" ? true : false,
                            MaxNett = Convert.ToDecimal(item.MaximumPremium),
                            MinNett = Convert.ToDecimal(item.MinimumPremium),
                            Modeles = Models,
                            PremiumAddones = addons,
                            PremiumBasedOnIdNett = PremiumBasedOnsId,
                            PremiumCurrencyId = CurrencyDataId,
                            PremiumTotal = Convert.ToDecimal(item.PremiumTotal),
                            WarrantyTypeId = WarrantyType,
                            RegionId = RegionId,
                            RSAProviderId = RSAProviderId//,
                            // Rate = Convert.ToDecimal(item.RateperAnum)
                        }, SecurityHelper.Context, AuditHelper.Context).Id;
                    }
                    else
                    {
                        ContractExtension = ContractExtensionData.ContractExtensions.Find(m => m.ExtensionTypeId == ExtensionType && m.ContractId == Contract).Id;
                    }

                    PolicyBundleRequestDto Bundle = PolicyManagementService.AddPolicyBundle(new PolicyBundleRequestDto()
                    {
                        Comment = item.Comment,
                        CommodityTypeId = CommodityTypeId,
                        ContractId = Contract,
                        CoverTypeId = WarrantyType,
                        CustomerId = Customer,
                        CustomerPayment = Convert.ToDecimal(item.CustomerPayment),
                        CustomerPaymentCurrencyTypeId = CurrencyData.Currencies.Find(c => c.Code == item.CustomerPaymentCurrencyType).Id,
                        DealerId = Dealer,
                        DealerLocationId = Location,
                        DealerPayment = Convert.ToDecimal(item.DealerPayment),
                        DealerPaymentCurrencyTypeId = CurrencyData.Currencies.Find(c => c.Code == item.DealerPaymentCurrencyType).Id,
                        DealerPolicy = item.DealerPolicy == "Yes" ? true : false,
                        Discount = Convert.ToDecimal(item.Discount),
                        ExtensionTypeId = ExtensionType,
                        HrsUsedAtPolicySale = item.HrsUsedAtPolicySale,
                        IsPartialPayment = item.PartialPayment == "Yes" ? true : false,
                        IsSpecialDeal = item.SpecialDeal == "Yes" ? true : false,
                        ItemId = Vehicle,
                        PaymentModeId = Guid.Parse(item.PaymentMode),
                        PolicyEndDate = DateTime.Parse(item.PolicyEndDate),
                        PolicyNo = item.PolicyNo,
                        PolicySoldDate = DateTime.Parse(item.PolicySoldDate),
                        PolicyStartDate = DateTime.Parse(item.PolicyStartDate),
                        Premium = Convert.ToDecimal(item.Premium),
                        PremiumCurrencyTypeId = CurrencyData.Currencies.Find(c => c.Code == item.PremiumCurrencyType).Id,
                        ProductId = Product,
                        RefNo = item.RefNo,
                        SalesPersonId = Guid.Parse(users.Users.Find(u => u.FirstName + " " + u.LastName == item.SalesPerson).Id),
                    }, SecurityHelper.Context, AuditHelper.Context);
                    PolicyRequestDto Policy = PolicyManagementService.AddPolicy(new PolicyRequestDto()
                    {
                        Comment = item.Comment,
                        CommodityTypeId = CommodityTypeId,
                        ContractId = Contract,
                        CoverTypeId = WarrantyType,
                        CustomerId = Customer,
                        CustomerPayment = Convert.ToDecimal(item.CustomerPayment),
                        CustomerPaymentCurrencyTypeId = CurrencyData.Currencies.Find(c => c.Code == item.CustomerPaymentCurrencyType).Id,
                        DealerId = Dealer,
                        DealerLocationId = Location,
                        DealerPayment = Convert.ToDecimal(item.DealerPayment),
                        DealerPaymentCurrencyTypeId = CurrencyData.Currencies.Find(c => c.Code == item.DealerPaymentCurrencyType).Id,
                        DealerPolicy = item.DealerPolicy == "Yes" ? true : false,
                        Discount = Convert.ToDecimal(item.Discount),
                        ExtensionTypeId = ExtensionType,
                        HrsUsedAtPolicySale = item.HrsUsedAtPolicySale,
                        IsPartialPayment = item.PartialPayment == "Yes" ? true : false,
                        IsSpecialDeal = item.SpecialDeal == "Yes" ? true : false,
                        ItemId = Vehicle,
                        PaymentModeId = Guid.Parse(item.PaymentMode),
                        PolicyEndDate = DateTime.Parse(item.PolicyEndDate),
                        PolicyNo = item.PolicyNo,
                        PolicySoldDate = DateTime.Parse(item.PolicySoldDate),
                        PolicyStartDate = DateTime.Parse(item.PolicyStartDate),
                        Premium = Convert.ToDecimal(item.Premium),
                        PremiumCurrencyTypeId = CurrencyData.Currencies.Find(c => c.Code == item.PremiumCurrencyType).Id,
                        ProductId = Product,
                        RefNo = item.RefNo,
                        SalesPersonId = Guid.Parse(users.Users.Find(u => u.FirstName + " " + u.LastName == item.SalesPerson).Id),
                        PolicyBundleId = Bundle.Id
                    }, SecurityHelper.Context, AuditHelper.Context);
                }
                else
                {
                    var ExtensionType = new Guid();
                    if (ExtensionTypes.ExtensionTypes.FindAll(m => m.ExtensionName == item.ExtensionName).Count == 0)
                    {
                        ExtensionType = ExtensionTypeManagementService.AddExtensionType(new ExtensionTypeRequestDto()
                        {
                            CommodityTypeId = CommodityTypeId,
                            ExtensionName = item.ExtensionName,
                            Hours = Convert.ToInt32(item.ExtensionHours),
                            Month = Convert.ToInt32(item.ExtensionMonth),
                            ProductId = Product
                        }, SecurityHelper.Context, AuditHelper.Context).Id;
                    }
                    else
                    {
                        ExtensionType = ExtensionTypes.ExtensionTypes.Find(m => m.ExtensionName == item.ExtensionName).Id;
                    }

                    List<Guid> Models = new List<Guid>();
                    Models.Add(Model);

                    List<ContractExtensionsPremiumAddonRequestDto> addons = new List<ContractExtensionsPremiumAddonRequestDto>();
                    addons.Add(new ContractExtensionsPremiumAddonRequestDto()
                    {
                        PremiumAddonTypeId = PremiumAddonTypes.PremiumAddonTypes.Find(p => p.Description == "Basic Premium").Id,
                        Value = Convert.ToInt32(item.BasicPremium)
                    });
                    addons.Add(new ContractExtensionsPremiumAddonRequestDto()
                    {
                        PremiumAddonTypeId = PremiumAddonTypes.PremiumAddonTypes.Find(p => p.Description == "Other Premium").Id,
                        Value = Convert.ToInt32(item.OtherPremium)
                    });
                    addons.Add(new ContractExtensionsPremiumAddonRequestDto()
                    {
                        PremiumAddonTypeId = PremiumAddonTypes.PremiumAddonTypes.Find(p => p.Description == "Additional Premium").Id,
                        Value = Convert.ToInt32(item.AdditionalPremium)
                    });

                    var ContractExtension = new Guid();
                    if (ContractExtensionData.ContractExtensions.FindAll(m => m.ExtensionTypeId == ExtensionType && m.ContractId == Contract).Count == 0)
                    {
                        ContractExtension = ContractManagementService.AddContractExtensions(new ContractExtensionsRequestDto()
                        {
                            ContractId = Contract,
                            ExtensionTypeId = ExtensionType,
                            GrossPremium = Convert.ToDecimal(item.GrossPremium),
                            Makes = Makes,
                            ManufacturerWarranty = item.ManufacturerWaranty == "Yes" ? true : false,
                            MaxNett = Convert.ToDecimal(item.MaximumPremium),
                            MinNett = Convert.ToDecimal(item.MinimumPremium),
                            Modeles = Models,
                            PremiumAddones = addons,
                            PremiumBasedOnIdNett = PremiumBasedOns.PremiumBasedOns.Find(p => p.Description == item.PremiumBasedOnDescription).Id,
                            PremiumCurrencyId = CurrencyData.Currencies.Find(c => c.Code == item.PremiumCurrencyType).Id,
                            PremiumTotal = Convert.ToDecimal(item.PremiumTotal),
                            WarrantyTypeId = WarrantyType,
                        }, SecurityHelper.Context, AuditHelper.Context).Id;
                    }
                    else
                    {
                        ContractExtension = ContractExtensionData.ContractExtensions.Find(m => m.ExtensionTypeId == ExtensionType && m.ContractId == Contract).Id;
                    }

                    var BrownAndWhiteDetails = new Guid();
                    if (BrownAndWhiteDetailsData.BrownAndWhiteAllDetails.FindAll(m => m.SerialNo == item.ElectronicSerialNo).Count == 0)
                    {
                        BrownAndWhiteDetails = BrownAndWhiteDetailsManagementService.AddBrownAndWhiteDetails(new BrownAndWhiteDetailsRequestDto()
                        {
                            CategoryId = Category,
                            CommodityUsageTypeId = CommodityUsageTypeData.CommodityUsageTypes.Find(c => c.Name == item.CommodityUsageType).Id,
                            DealerPrice = Convert.ToDecimal(item.DealerPrice),
                            ItemPurchasedDate = DateTime.Parse(item.PurchaseDate),
                            ItemStatusId = ItemStatus,
                            MakeId = Make,
                            ModelId = Model,
                            ModelYear = item.VehicleModelYear,
                            SerialNo = item.ElectronicSerialNo,
                            ItemPrice = Convert.ToDecimal(item.ItemPrice),
                            InvoiceNo = item.ElectronicInvoiceNo,
                            ModelCode = item.ModelCode,
                        }, SecurityHelper.Context, AuditHelper.Context).Id;
                    }
                    else
                    {
                        BrownAndWhiteDetails = BrownAndWhiteDetailsData.BrownAndWhiteAllDetails.Find(m => m.SerialNo == item.ElectronicSerialNo).Id;
                    }

                    PolicyBundleRequestDto Bundle = PolicyManagementService.AddPolicyBundle(new PolicyBundleRequestDto()
                    {
                        Comment = item.Comment,
                        CommodityTypeId = CommodityTypeId,
                        ContractId = Contract,
                        CoverTypeId = WarrantyType,
                        CustomerId = Customer,
                        CustomerPayment = Convert.ToDecimal(item.CustomerPayment),
                        CustomerPaymentCurrencyTypeId = CurrencyData.Currencies.Find(c => c.Code == item.CustomerPaymentCurrencyType).Id,
                        DealerId = Dealer,
                        DealerLocationId = Location,
                        DealerPayment = Convert.ToDecimal(item.DealerPayment),
                        DealerPaymentCurrencyTypeId = CurrencyData.Currencies.Find(c => c.Code == item.DealerPaymentCurrencyType).Id,
                        DealerPolicy = item.DealerPolicy == "Yes" ? true : false,
                        Discount = Convert.ToDecimal(item.Discount),
                        ExtensionTypeId = ExtensionType,
                        HrsUsedAtPolicySale = item.HrsUsedAtPolicySale,
                        IsPartialPayment = item.PartialPayment == "Yes" ? true : false,
                        IsSpecialDeal = item.SpecialDeal == "Yes" ? true : false,
                        ItemId = BrownAndWhiteDetails,
                        PaymentModeId = Guid.Parse(item.PaymentMode),
                        PolicyEndDate = DateTime.Parse(item.PolicyEndDate),
                        PolicyNo = item.PolicyNo,
                        PolicySoldDate = DateTime.Parse(item.PolicySoldDate),
                        PolicyStartDate = DateTime.Parse(item.PolicyStartDate),
                        Premium = Convert.ToDecimal(item.Premium),
                        PremiumCurrencyTypeId = CurrencyData.Currencies.Find(c => c.Code == item.PremiumCurrencyType).Id,
                        ProductId = Product,
                        RefNo = item.RefNo,
                        SalesPersonId = Guid.Parse(users.Users.Find(u => u.FirstName + " " + u.LastName == item.SalesPerson).Id),
                    }, SecurityHelper.Context, AuditHelper.Context);
                    PolicyRequestDto Policy = PolicyManagementService.AddPolicy(new PolicyRequestDto()
                    {
                        Comment = item.Comment,
                        CommodityTypeId = CommodityTypeId,
                        ContractId = Contract,
                        CoverTypeId = WarrantyType,
                        CustomerId = Customer,
                        CustomerPayment = Convert.ToDecimal(item.CustomerPayment),
                        CustomerPaymentCurrencyTypeId = CurrencyData.Currencies.Find(c => c.Code == item.CustomerPaymentCurrencyType).Id,
                        DealerId = Dealer,
                        DealerLocationId = Location,
                        DealerPayment = Convert.ToDecimal(item.DealerPayment),
                        DealerPaymentCurrencyTypeId = CurrencyData.Currencies.Find(c => c.Code == item.DealerPaymentCurrencyType).Id,
                        DealerPolicy = item.DealerPolicy == "Yes" ? true : false,
                        Discount = Convert.ToDecimal(item.Discount),
                        ExtensionTypeId = ExtensionType,
                        HrsUsedAtPolicySale = item.HrsUsedAtPolicySale,
                        IsPartialPayment = item.PartialPayment == "Yes" ? true : false,
                        IsSpecialDeal = item.SpecialDeal == "Yes" ? true : false,
                        ItemId = BrownAndWhiteDetails,
                        PaymentModeId = Guid.Parse(item.PaymentMode),
                        PolicyEndDate = DateTime.Parse(item.PolicyEndDate),
                        PolicyNo = item.PolicyNo,
                        PolicySoldDate = DateTime.Parse(item.PolicySoldDate),
                        PolicyStartDate = DateTime.Parse(item.PolicyStartDate),
                        Premium = Convert.ToDecimal(item.Premium),
                        PremiumCurrencyTypeId = CurrencyData.Currencies.Find(c => c.Code == item.PremiumCurrencyType).Id,
                        ProductId = Product,
                        RefNo = item.RefNo,
                        SalesPersonId = Guid.Parse(users.Users.Find(u => u.FirstName + " " + u.LastName == item.SalesPerson).Id),
                        PolicyBundleId = Bundle.Id
                    }, SecurityHelper.Context, AuditHelper.Context);
                }
            }

            return "Ok";
        }

        private bool EmailValidator(string email)
        {
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(email);
            if (match.Success)
                return true;
            else
                return false;
        }

        private static PolicyBulk AddPolicyBulkData(IList<string> rowData, IList<string> columnNames)
        {
            var PolicyBulk = new PolicyBulk()
            {
                CommodityType = rowData[columnNames.IndexFor("Commodity Type")],
                Status = rowData[columnNames.IndexFor("Status")],
                ProductName = rowData[columnNames.IndexFor("Product Name")],
                ProductCode = rowData[columnNames.IndexFor("Product Code")],
                DealerName = rowData[columnNames.IndexFor("Dealer Name")],
                DealerCountry = rowData[columnNames.IndexFor("Dealer Country")],
                DealerCurrency = rowData[columnNames.IndexFor("Dealer Currency")],
                DealerBranch = rowData[columnNames.IndexFor("Dealer Branch")],
                DealerBranchCity = rowData[columnNames.IndexFor("Dealer Branch City")],
                DealerBranchServiceContractPerson = rowData[columnNames.IndexFor("Dealer Branch Service Contract Person")],
                DealerBranchServiceTelephoneNo = rowData[columnNames.IndexFor("Dealer Branch Service Telephone No")],
                DealerBranchServiceFax = rowData[columnNames.IndexFor("Dealer Branch Service Fax")],
                DealerBranchServiceEmail = rowData[columnNames.IndexFor("Dealer Branch Service Email")],
                DealerBranchSalesContractPerson = rowData[columnNames.IndexFor("Dealer Branch Sales Contract Person")],
                DealerBranchSalesTelephoneNo = rowData[columnNames.IndexFor("Dealer Branch Sales Telephone No")],
                DealerBranchSalesFax = rowData[columnNames.IndexFor("Dealer Branch Sales Fax")],
                DealerBranchSalesEmail = rowData[columnNames.IndexFor("Dealer Branch Sales Email")],
                InsurerName = rowData[columnNames.IndexFor("Insurer Name")],
                InsurerCode = rowData[columnNames.IndexFor("Insurer Code")],
                InsurerCountry = rowData[columnNames.IndexFor("Insurer Country")],
                VehicleVinNo = rowData[columnNames.IndexFor("Vehicle Vin No")],
                Manufacturer = rowData[columnNames.IndexFor("Manufacturer")],
                ManufacturerCode = rowData[columnNames.IndexFor("Manufacturer Code")],
                ManufacturerWaranty = rowData[columnNames.IndexFor("Manufacturer Waranty")],
                EngineCapacityMeasureType = rowData[columnNames.IndexFor("Engine Capacity Measure Type")],
                ManufacturerWarantyCountry = rowData[columnNames.IndexFor("Manufacturer Waranty Country")],
                ManufacturerWarantyMonth = rowData[columnNames.IndexFor("Manufacturer Waranty Month")],
                ManufacturerWarantyStartDate = rowData[columnNames.IndexFor("Manufacturer Waranty Start Date")],
                ManufacturerWarantyKm = rowData[columnNames.IndexFor("Manufacturer Waranty Km")],
                MakeName = rowData[columnNames.IndexFor("Make Name")],
                MakeCode = rowData[columnNames.IndexFor("Make Code")],
                ModelName = rowData[columnNames.IndexFor("Model Name")],
                ModelCode = rowData[columnNames.IndexFor("Model Code")],
                ModelRiskStartDate = rowData[columnNames.IndexFor("Model Risk Start Date")],
                CategoryName = rowData[columnNames.IndexFor("Category Name")],
                CategoryLength = rowData[columnNames.IndexFor("Category Length")],
                VehiclelCountryOfOrigine = rowData[columnNames.IndexFor("Vehiclel Country Of Origine")],
                VehicleCylinderCount = rowData[columnNames.IndexFor("Vehicle Cylinder Count")],
                VehicleBodyType = rowData[columnNames.IndexFor("Vehicle Body Type")],
                VehiclePlateNo = rowData[columnNames.IndexFor("Vehicle Plate No")],
                VehicleModelYear = rowData[columnNames.IndexFor("Vehicle Model Year")],
                VehicleFuelType = rowData[columnNames.IndexFor("Vehicle Fuel Type")],
                VehicleAspiration = rowData[columnNames.IndexFor("Vehicle Aspiration")],
                VehicleVariant = rowData[columnNames.IndexFor("Vehicle Variant")],
                VehicleTransmission = rowData[columnNames.IndexFor("Vehicle Transmission")],
                VehicleTransmissionTechnology = rowData[columnNames.IndexFor("Vehicle Transmission Technology")],
                PurchaseDate = rowData[columnNames.IndexFor("Purchase Date")],
                VehicleRegistrationDate = rowData[columnNames.IndexFor("Vehicle Registration Date")],
                VehicleEngineCapacity = rowData[columnNames.IndexFor("Vehicle Engine Capacity")],
                VehicleDriveType = rowData[columnNames.IndexFor("Vehicle Drive Type")],
                CommodityUsageType = rowData[columnNames.IndexFor("Commodity Usage Type")],
                ElectronicCountryOfOrigine = rowData[columnNames.IndexFor("Electronic Country Of Origine")],
                ElectronicSerialNo = rowData[columnNames.IndexFor("Electronic Serial No")],
                ElectronicModelYear = rowData[columnNames.IndexFor("Electronic Model Year")],
                ElectronicInvoiceNo = rowData[columnNames.IndexFor("Electronic Invoice No")],
                ItemPrice = rowData[columnNames.IndexFor("Item Price")],
                DealerPrice = rowData[columnNames.IndexFor("Dealer Price")],
                ContractDealName = rowData[columnNames.IndexFor("Contract Deal Name")],
                ContractDealType = rowData[columnNames.IndexFor("Contract Deal Type")],
                ReinsurerName = rowData[columnNames.IndexFor("Reinsurer Name")],
                ReinsurerCode = rowData[columnNames.IndexFor("Reinsurer Code")],
                PromotionalDeal = rowData[columnNames.IndexFor("Promotional Deal")],
                AutoRenewal = rowData[columnNames.IndexFor("Auto Renewal")],
                DiscountAvailable = rowData[columnNames.IndexFor("Discount Available")],
                DealStartDate = rowData[columnNames.IndexFor("Deal Start Date")],
                DealEndDate = rowData[columnNames.IndexFor("Deal End Date")],
                PremiumTotal = rowData[columnNames.IndexFor("Premium Total")],
                ClaimLimitation = rowData[columnNames.IndexFor("Claim Limitation")],
                LiabilityLimitation = rowData[columnNames.IndexFor("Liability Limitation")],
                ExtensionName = rowData[columnNames.IndexFor("Extension Name")],
                ExtensionKm = rowData[columnNames.IndexFor("Extension Km")],
                ExtensionMonth = rowData[columnNames.IndexFor("Extension Month")],
                ExtensionHours = rowData[columnNames.IndexFor("Extension Hours")],
                PremiumBasedOnDescription = rowData[columnNames.IndexFor("Premium Based On Description")],
                PremiumBasedonCode = rowData[columnNames.IndexFor("Premium Based on Code")],
                BasicPremium = rowData[columnNames.IndexFor("Basic Premium")],
                OtherPremium = rowData[columnNames.IndexFor("Other Premium")],
                AdditionalPremium = rowData[columnNames.IndexFor("Additional Premium")],
                SportsPremium = rowData[columnNames.IndexFor("Sports Premium")],
                FourByFourPremium = rowData[columnNames.IndexFor("Four By Four Premium")],
                MinimumPremium = rowData[columnNames.IndexFor("Minimum Premium")],
                MaximumPremium = rowData[columnNames.IndexFor("Maximum Premium")],
                RSAProviderName = rowData[columnNames.IndexFor("RSAProviderName")],
                Region = rowData[columnNames.IndexFor("Region")],
                RateperAnum = rowData[columnNames.IndexFor("Rateper Anum")],
                GrossPremium = rowData[columnNames.IndexFor("Gross Premium")],
                PremiumCurrencyType = rowData[columnNames.IndexFor("Premium Currency Type")],
                CoverType = rowData[columnNames.IndexFor("Cover Type")],
                SalesPerson = rowData[columnNames.IndexFor("Sales Person")],
                DealerPaymentCurrencyType = rowData[columnNames.IndexFor("Dealer Payment Currency Type")],
                CustomerPaymentCurrencyType = rowData[columnNames.IndexFor("Customer Payment Currency Type")],
                PaymentMode = rowData[columnNames.IndexFor("Payment Mode")],
                CustomerFirstName = rowData[columnNames.IndexFor("Customer First Name")],
                CustomerLastName = rowData[columnNames.IndexFor("Customer Last Name")],
                Nationality = rowData[columnNames.IndexFor("Nationality")],
                DateOfBirth = rowData[columnNames.IndexFor("Date Of Birth")],
                CustomerCountry = rowData[columnNames.IndexFor("Customer Country")],
                CustomerOccupation = rowData[columnNames.IndexFor("Customer Occupation")],
                CustomerTitle = rowData[columnNames.IndexFor("Customer Title")],
                CustomerGender = rowData[columnNames.IndexFor("Customer Gender")],
                CustomerMaritalStatus = rowData[columnNames.IndexFor("Customer Marital Status")],
                CustomerMobileNo = rowData[columnNames.IndexFor("Customer Mobile No")],
                CustomerPostalCode = rowData[columnNames.IndexFor("Customer Postal Code")],
                CustomerIdType = rowData[columnNames.IndexFor("Customer Id Type")],
                CustomerIdNo = rowData[columnNames.IndexFor("Customer Id No")],
                CustomerIdIssueDate = rowData[columnNames.IndexFor("Customer Id Issue Date")],
                CustomerEmail = rowData[columnNames.IndexFor("Customer Email")],
                CustomerType = rowData[columnNames.IndexFor("Customer Type")],
                UsageType = rowData[columnNames.IndexFor("Usage Type")],
                BusinessName = rowData[columnNames.IndexFor("Business Name")],
                BusinessAddress = rowData[columnNames.IndexFor("Business Address")],
                BusinessPhoneNo = rowData[columnNames.IndexFor("Business Phone No")],
                CustomerAddress = rowData[columnNames.IndexFor("Customer Address")],
                CustomerCity = rowData[columnNames.IndexFor("Customer City")],
                HrsUsedAtPolicySale = rowData[columnNames.IndexFor("Hrs Used At Policy Sale")],
                PolicyNo = rowData[columnNames.IndexFor("Policy No")],
                RefNo = rowData[columnNames.IndexFor("Ref No")],
                Comment = rowData[columnNames.IndexFor("Comment")],
                Premium = rowData[columnNames.IndexFor("Premium")],
                DealerPayment = rowData[columnNames.IndexFor("Dealer Payment")],
                CustomerPayment = rowData[columnNames.IndexFor("Customer Payment")],
                SpecialDeal = rowData[columnNames.IndexFor("Special Deal")],
                PartialPayment = rowData[columnNames.IndexFor("Partial Payment")],
                PolicySoldDate = rowData[columnNames.IndexFor("Policy Sold Date")],
                PolicyStartDate = rowData[columnNames.IndexFor("Policy Start Date")],
                PolicyEndDate = rowData[columnNames.IndexFor("Policy End Date")],
                Discount = rowData[columnNames.IndexFor("Discount")],
                DealerPolicy = rowData[columnNames.IndexFor("Dealer Policy")],
                AutoApproval = rowData[columnNames.IndexFor("Auto Approval")]
            };
            return PolicyBulk;
        }

        private static System.Reflection.Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            string dllName = args.Name.Contains(',') ? args.Name.Substring(0, args.Name.IndexOf(',')) : args.Name.Replace(".dll", "");
            dllName = dllName.Replace(".", "_");
            if (dllName.EndsWith("_resources")) return null;
            var obj = new Object();
            System.Resources.ResourceManager rm = new System.Resources.ResourceManager(obj.GetType().Namespace + ".Properties.Resources", System.Reflection.Assembly.GetExecutingAssembly());
            byte[] bytes = (byte[])rm.GetObject(dllName);
            return System.Reflection.Assembly.Load(bytes);
        }

        #endregion

        [HttpPost]
        public HttpResponseMessage GetPolicyAttachmentById(JObject data)//Guid policyid)
        {
            Guid policyid = Guid.Parse(data["policyid"].ToString());
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            IPolicyManagementService PolicyManagementService = ServiceFactory.GetPolicyManagementService();
            var array = PolicyManagementService.GetPolicyAttachmentById(policyid, SecurityHelper.Context, AuditHelper.Context);
            Stream stream = new MemoryStream(array);
            //string path = "E://pdf-sample.PDF";
            //byte[] pdf = System.IO.File.ReadAllBytes(@path);

            //HttpResponseMessage result = Request.CreateResponse(HttpStatusCode.OK);
            //result.Content = new ByteArrayContent(array);
            //result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("inline");
            //result.Content.Headers.ContentDisposition.FileName = "Policy.pdf";
            //result.Content.Headers.Add("x-filename", "Policy.pdf");
            //result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            //return result;


            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            result.Content = new StreamContent(stream);
            result.Content.Headers.ContentType =
                 new MediaTypeHeaderValue("application/octet-stream");
            Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            result.Content.Headers.Add("x-filename", "PolicyStatement_" + unixTimestamp + ".pdf");
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = "PolicyStatement_" + unixTimestamp + ".pdf"
            };

            return result;


        }

        [HttpPost]
        public object GetEMIValue(JObject data)
        {
            object Response = null;
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                Decimal LoneAmount = Decimal.Parse(data["LoneAmount"].ToString());
                //int LoanTenure =  Int32.Parse(data["LoanTenure"].ToString());
                Guid ContractId = Guid.Parse(data["ContractId"].ToString());
                IPolicyManagementService PolicyManagementService = ServiceFactory.GetPolicyManagementService();
                Response = PolicyManagementService.GetEMIValue(LoneAmount,  ContractId,
                    SecurityHelper.Context, AuditHelper.Context);
                return Response;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                             ex.InnerException);
            }
            return Response;
        }
        [HttpPost]
        public object checkCustomerExist(JObject data)
        {
            object Response = null;
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                String Mobile = data["mobileNo"].ToString();
                IPolicyManagementService PolicyManagementService = ServiceFactory.GetPolicyManagementService();
                Response = PolicyManagementService.checkCustomerExist(Mobile,
                    SecurityHelper.Context, AuditHelper.Context);
                return Response;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                             ex.InnerException);
            }
            return Response;
        }

        [HttpPost]
        public object DownloadPolicyStatementforTYER(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                var policyid = Guid.Parse(data["policyid"].ToString());

                IPolicyManagementService PolicyManagementService = ServiceFactory.GetPolicyManagementService();
                var result = PolicyManagementService.DownloadPolicyStatementforTYER(policyid,
                SecurityHelper.Context,
                AuditHelper.Context);
                return result;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }
        }

    }

    #region Policy Data Defs
    public class PolicyViewData : PolicyResponseDto
    {
        public VehicleDetailsResponseDto Vehicle { get; set; }
        public CustomerResponseDto Customer { get; set; }
        public BrownAndWhiteDetailsResponseDto BAndW { get; set; }

        public Guid ModifiedUser { get; set; }

    }
    public class PolicyTransactions
    {
        public PolicyHistoryResponseDto Policy { get; set; }
        public List<PolicyTransactionResponseDto> Endorsements { get; set; }
        public List<PolicyTransactionResponseDto> Cancelations { get; set; }
        public List<PolicyHistoryResponseDto> Renewals { get; set; }
    }

    public class PolicyTransactionResponseData : PolicyTransactionResponseDto
    {
        public string Endorsed { get; set; }
    }
    #endregion




}