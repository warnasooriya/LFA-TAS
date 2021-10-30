using NHibernate;
using NHibernate.Linq;
using NHibernate.Transform;
using NLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using TAS.DataTransfer.Requests;
using TAS.DataTransfer.Responses;
using TAS.Services.Common;
using TAS.Services.Common.Transformer;
using TAS.Services.Entities.Persistence;

namespace TAS.Services.Entities.Management
{
    public class ReportEntityManager
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public String SetupPolicyStatement(String HTMLText, Guid policyId)
        {
            ISession session = EntitySessionManager.GetSession();
            ContractEntityManager ContractEntityManager = new ContractEntityManager();
            CommonEntityManager CommonEntityManager = new CommonEntityManager();
            CommonEntityManager cem = new CommonEntityManager();
            //PolicyBundle PolicyBundle = session.Query<PolicyBundle>().Where(a => a.Id == policyId).FirstOrDefault();
            Policy policy = session.Query<Policy>().Where(a => a.PolicyBundleId == policyId).FirstOrDefault();
            if (policy == null)
            {
                return String.Empty;
            }

            Customer customer = session.Query<Customer>().Where(a => a.Id == policy.CustomerId).FirstOrDefault();
            Nationality nationality = session.Query<Nationality>().Where(a => a.Id == customer.NationalityId).FirstOrDefault();
            IdType idType = session.Query<IdType>().Where(a => a.Id == customer.IDTypeId).FirstOrDefault();
            CustomerType customerType = session.Query<CustomerType>().Where(a => a.Id == customer.CustomerTypeId).FirstOrDefault();
            Country country = session.Query<Country>().Where(a => a.Id == customer.CountryId).FirstOrDefault();

            CommodityType CommodityType = session.Query<CommodityType>().Where(a => a.CommodityTypeId == policy.CommodityTypeId).FirstOrDefault();
            Dealer Dealer = session.Query<Dealer>().Where(a => a.Id == policy.DealerId).FirstOrDefault();
            Product Product = session.Query<Product>().Where(a => a.Id == policy.ProductId).FirstOrDefault();
            Country dealerCountry =  session.Query<Country>().Where(a => a.Id == Dealer.CountryId).FirstOrDefault();

            DealerLocation DealerLocation = session.Query<DealerLocation>().Where(a => a.Id == policy.DealerLocationId).FirstOrDefault();
            Contract Contract = session.Query<Contract>().Where(a => a.Id == policy.ContractId).FirstOrDefault();

            UsageType UsageType = session.Query<UsageType>().Where(a => a.Id == customer.UsageTypeId).FirstOrDefault();
            ExtensionType ExtensionType = session.Query<ExtensionType>().Where(a => a.Id == policy.ExtensionTypeId).FirstOrDefault();

            InternalUser InternalUser = session.Query<InternalUser>().Where(a => a.Id == policy.SalesPersonId.ToString()).FirstOrDefault();
            /***/
            ReinsurerContract ReinsurerContract = session.Query<ReinsurerContract>().Where(a => a.Id == Contract.ReinsurerContractId).FirstOrDefault();
            Currency Currency = session.Query<Currency>().Where(a => a.Id == policy.PremiumCurrencyTypeId).FirstOrDefault();
            string paymentModeid = cem.GetPaymentMethodNameById(policy.PaymentModeId);
            ContractInsuaranceLimitation CIL = session.Query<ContractInsuaranceLimitation>().Where(a => a.ContractId == Contract.Id).FirstOrDefault();

            ContractExtensions ContractExtensions = session.Query<ContractExtensions>().Where(a => a.ContractInsuanceLimitationId == CIL.Id).FirstOrDefault();
            ContractExtensionPremium contractExtensionPremium = session.Query<ContractExtensionPremium>().Where(a => a.ContractExtensionId == ContractExtensions.Id).FirstOrDefault();
            WarrantyType WarrantyType = session.Query<WarrantyType>().Where(a => a.Id == contractExtensionPremium.WarrentyTypeId).FirstOrDefault();

            InsuaranceLimitation IL = session.Query<InsuaranceLimitation>().Where(a => a.CommodityTypeId == policy.CommodityTypeId && a.Id == CIL.InsuaranceLimitationId).FirstOrDefault();


            if (Product.Productcode == "TYRE")
            {
                OtherItemPolicy otherItemPolicy = session.Query<OtherItemPolicy>().Where(a => a.PolicyId == policy.Id).FirstOrDefault();
                OtherItemDetails otherItemDetails = session.Query<OtherItemDetails>().Where(a => a.Id == otherItemPolicy.OtherItemId).FirstOrDefault();
                Make Make = session.Query<Make>().Where(a => a.Id == otherItemDetails.MakeId).FirstOrDefault();
                Model Model = session.Query<Model>().Where(a => a.Id == otherItemDetails.ModelId).FirstOrDefault();
                CommodityCategory CommodityCategory = session.Query<CommodityCategory>().Where(a => a.CommodityCategoryId == otherItemDetails.CategoryId).FirstOrDefault();
                InvoiceCodeDetails InvoiceCodeDetails = session.Query<InvoiceCodeDetails>().Where(a => a.PolicyId == policy.Id).FirstOrDefault();
                InvoiceCode InvoiceCode = session.Query<InvoiceCode>().Where(a => a.Id == InvoiceCodeDetails.InvoiceCodeId).FirstOrDefault();
                CustomerEnterdInvoiceDetails CustomerEnterdInvoiceDetails = session.Query<CustomerEnterdInvoiceDetails>()
                    .Where(a => a.InvoiceCodeId == InvoiceCode.Id).FirstOrDefault();
                //ManufacturerWarrantyDetails mwd = session.Query<ManufacturerWarrantyDetails>().Where(a => a.ModelId == Model.Id &&
                //a.CountryId == ReinsurerContract.CountryId).FirstOrDefault();
                //ManufacturerWarranty mw = session.Query<ManufacturerWarranty>().Where(a => a.Id == mwd.ManufacturerWarrantyId && a.MakeId == Make.Id).FirstOrDefault();
                //int mwMonths = mw == null ? 0 : mw.WarrantyMonths;

                //replacing
                HTMLText = HTMLText.Replace("{POLICYNUMBER}", policy == null ? "" : policy.PolicyNo)
               //.Replace("{POLICYPERIOD}", policy.PolicyEndDate.ToString("dd-MMM-yyyy"))
               //.Replace("{POLICYPERIOD}", policy == null ? "" : (policy.PolicySoldDate.AddMonths(mwMonths).ToString("dd-MMM-yyyy") + "-" + policy == null ? "" : policy.PolicyEndDate.AddDays(1).ToString("dd-MMM-yyyy")))
               .Replace("{CUSTOMERNAME}", customer.FirstName + " " + customer.LastName)
               .Replace("{CUSTOMEREMAIL}", customer.Email)
               .Replace("{TELWORK}", customer.MobileNo.Replace(" ",""))
               //.Replace("{TELHOME}", customer.OtherTelNo)
               .Replace("{CUSTOMERNATIONALTY}", nationality == null ? "N/A" : nationality.NationalityName)
               .Replace("{CUSTOMERIDTYPE}", idType == null ? "" : idType.IdTypeName)
               //.Replace("{CUSTOMERIDNUMBER}", customer.IDNo)
               //.Replace("{CUSTOMERGENDER}", customer.Gender == 'M' ? "Male" : "Female")
               .Replace("{ADDRESSLINE1}", customer.Address1)
               .Replace("{ADDRESSLINE2}", customer.Address2)
               .Replace("{ADDRESSLINE3}", customer.Address3)

               .Replace("{BUSINESSADDRESSLINE1}", customer.BusinessAddress1)
               .Replace("{BUSINESSADDRESSLINE2}", customer.BusinessAddress2)
               .Replace("{BUSINESSADDRESSLINE3}", customer.BusinessAddress3)
               .Replace("{BUSINESSNAME}", customer.BusinessName)
               //.Replace("{BUSINESSNAME}", customer.BusinessTelNo)

               //.Replace("{POSTCODE}", customer.PostalCode)
               .Replace("{COUNTRY}", country == null ? "" : country.CountryName)
               .Replace("{CUSTOMERTYPE}", customerType == null ? "" : customerType.CustomerTypeName)
               .Replace("{CUSTOMERDOB}", customer.DateOfBirth == null ? "" : customer.DateOfBirth.Value.ToString("dd-MMM-yyyy"))

               .Replace("{CLAIMLIMITATION}", (Contract.ClaimLimitation * policy.LocalCurrencyConversionRate).ToString("#,##0.00"))
               .Replace("{LIABILITYLIMITATION}", (Contract.LiabilityLimitation * policy.LocalCurrencyConversionRate).ToString("#,##0.00"))
                //.Replace("{GVWCODE}", vehicleWeight)

                .Replace("{COMMODITYTYPE}", CommodityType.CommodityTypeDescription)
                .Replace("{PRODUCTNAME}", Product.Productname)
                .Replace("{MAKE}", Make.MakeName)
                .Replace("{MODEL}", Model.ModelName)
                .Replace("{MODELYEAR}", CustomerEnterdInvoiceDetails.AdditionalDetailsModelYear.ToString())
                .Replace("{VIN}", CustomerEnterdInvoiceDetails.AdditionalDetailsMileage.ToString("#,##0"))
                .Replace("{PLATENO}", InvoiceCode.PlateNumber)
                .Replace("{DEALERNAME}", Dealer.DealerName)
                .Replace("{DEALERBRANCH}", otherItemDetails.InvoiceNo)
                .Replace("{USAGETYPE}", UsageType.UsageTypeName)
                .Replace("{PURCHASEDATE}", otherItemDetails.ItemPurchasedDate.ToString("dd-MMM-yyyy"))
                .Replace("{DEALERPRICE}", (otherItemDetails.DealerPrice * policy.LocalCurrencyConversionRate).ToString("#,##0.00"))
                .Replace("{PRODUCTPRICE}", (otherItemDetails.ItemPrice * policy.LocalCurrencyConversionRate).ToString("#,##0.00"))
                .Replace("{VEHICLECATEGORY}", CommodityCategory.CommodityCategoryDescription)
                .Replace("{POLICYSTARTDATE}", policy.PolicyStartDate.AddDays(1).ToString("dd-MMM-yyyy"))
                .Replace("{POLICYENDDATE}", policy.PolicyEndDate.ToString("dd-MMM-yyyy"))

                //.Replace("{MWENDDATE}", mw == null ? policy.PolicySoldDate.ToString("dd-MMM-yyyy") : policy.MWStartDate.AddMonths(mw.WarrantyMonths).AddDays(-1).ToString("dd-MMM-yyyy"))
                //.Replace("{MWSTARTDATE}", mw == null ? "" : policy.PolicySoldDate.ToString("dd-MMM-yyyy"))
                //.Replace("{MWSTARTDATE}", mw == null ? policy.PolicySoldDate.ToString("dd-MMM-yyyy") : policy.MWStartDate.ToString("dd-MMM-yyyy"))

                //.Replace("{MWENDDATE}",mw == null ? "" : policy.PolicySoldDate.AddMonths(mwMonths).AddDays(-1).ToString("dd-MMM-yyyy"))
                //.Replace("{EXTENSIONSTARTDATE}", ExtensionType == null ? "" : VehicleDetails.ItemPurchasedDate.AddMonths(mwMonths).AddDays(1).ToString("dd-MMM-yyyy"))
                //.Replace("{EXTENSIONENDDATE}", ExtensionType == null ? "" : VehicleDetails.ItemPurchasedDate.AddMonths(mwMonths).AddDays(1).AddMonths(ExtensionType.Month).ToString("dd-MMM-yyyy"))
                //.Replace("{EXTENSIONSTARTDATE}", policy.MWIsAvailable ? policy.MWStartDate.AddMonths(mw.WarrantyMonths).ToString("dd-MMM-yyyy")
                //: policy.PolicySoldDate.ToString("dd-MMM-yyyy"))
                //.Replace("{EXTENSIONENDDATE}", policy.MWIsAvailable ? policy.MWStartDate.AddMonths(mw.WarrantyMonths)
                //    .AddMonths(IL == null ? 0 : IL.Months).AddDays(-1).ToString("dd-MMM-yyyy")
                //: policy.PolicySoldDate.AddMonths(IL == null ? 0
                //: IL.Months).AddDays(-1).ToString("dd-MMM-yyyy"))

                //.Replace("{EXTENSIONSTARTDATE}", policy.PolicyStartDate.AddDays(1).ToString("dd-MMM-yyyy"))


                //.Replace("{EXTENSIONENDDATE}", policy.PolicySoldDate.AddMonths(mwMonths).AddMonths(IL.Months).AddDays(-1).ToString("dd-MMM-yyyy"))


                //.Replace("{EXTENSIONKM}", IL.Km.ToString("0"))
                //.Replace("{ENNUMBER}", EngineCapacity.EngineCapacityNumber.ToString())
                //.Replace("{MESURETYPE}", EngineCapacity.MesureType)
                //.Replace("{MWKM}", mw.IsUnlimited ? "Unlimited" : mw.WarrantyKm.ToString())
                //.Replace("{GROSSWEIGHT}", VehicleDetails.GrossWeight.ToString())
                .Replace("{DEALNAME}", Contract.DealName)
                .Replace("{DEALERCOUNTRY}", dealerCountry.CountryName)
                .Replace("{CONTRACTNO}", ReinsurerContract == null ? "" : ReinsurerContract.ContractNo)
                //.Replace("{EXTENTIONTYPE}", IL.InsuaranceLimitationName)
                .Replace("{COVERTYPE}", WarrantyType.WarrantyTypeDescription)
                .Replace("{POLICYSOLDDATE}", policy == null ? "" : policy.PolicySoldDate.ToString("dd-MMM-yyyy"))
                .Replace("{MILAGE}", policy == null ? "" : policy.HrsUsedAtPolicySale)
                .Replace("{CURRENCY}", Currency.Code)
                //.Replace("{SALESPERSON}", InternalUser == null ? "" : (InternalUser.FirstName + " " + InternalUser.LastName))
                .Replace("{GROSSPREMIUM}", policy == null ? "" : (policy.Premium * policy.LocalCurrencyConversionRate).ToString("#,##0.00"))


                .Replace("{PAYMENTMETHOD}", policy == null ? "" : paymentModeid)
                .Replace("{PAYMENTREFNO}", policy == null ? "" : policy.RefNo)
                .Replace("{PAYMENTCOMMENT}", policy == null ? "" : policy.Comment)
                .Replace("{DISCOUNT}", policy == null ? "" : (policy.Discount * policy.LocalCurrencyConversionRate).ToString("#,##0.00"))
                .Replace("{PAYMENTAMOUNT}", policy == null ? "" : ((policy.CustomerPayment + policy.DealerPayment) * policy.LocalCurrencyConversionRate).ToString("#,##0.00"));

            }
            else
            {
                VehiclePolicy VehiclePolicy = session.Query<VehiclePolicy>().Where(a => a.PolicyId == policy.Id).FirstOrDefault();
                VehicleDetails VehicleDetails = session.Query<VehicleDetails>().Where(a => a.Id == VehiclePolicy.VehicleId).FirstOrDefault();
                Make Make = session.Query<Make>().Where(a => a.Id == VehicleDetails.MakeId).FirstOrDefault();
                Model Model = session.Query<Model>().Where(a => a.Id == VehicleDetails.ModelId).FirstOrDefault();
                CommodityCategory CommodityCategory = session.Query<CommodityCategory>().Where(a => a.CommodityCategoryId == VehicleDetails.CategoryId).FirstOrDefault();

                ManufacturerWarranty mw = null;

                    ManufacturerWarrantyDetails mwd = session.Query<ManufacturerWarrantyDetails>().Where(a => a.ModelId == Model.Id &&
                                                  a.CountryId == ReinsurerContract.CountryId).FirstOrDefault();
                if (mwd != null)
                {
                    mw = session.Query<ManufacturerWarranty>().Where(a => a.Id == mwd.ManufacturerWarrantyId && a.MakeId == Make.Id).FirstOrDefault();
                    //  .Where(a => a.MakeId == Make.Id && a.ModelId == Model.Id && a.CountryId == ReinsurerContract.CountryId).FirstOrDefault();
                }

                int mwMonths = mw == null ? 0 : mw.WarrantyMonths;

                EngineCapacity EngineCapacity = session.Query<EngineCapacity>().Where(a => a.Id == VehicleDetails.EngineCapacityId).FirstOrDefault();
                //int ExtensionTypeMonth = ExtensionType == null ? 0 : ExtensionType.Month;


                IQueryable<VehicleWeight> vehicleWeightData = session.Query<VehicleWeight>();
                var vehicleWeight = "";

                if (vehicleWeightData != null)
                {
                    foreach (var GVW in vehicleWeightData)
                    {

                        if (GVW.WeightFrom < VehicleDetails.GrossWeight && GVW.WeightTo == 0)
                        {
                            vehicleWeight = GVW.VehicleWeightCode;
                        }
                        else if (GVW.WeightFrom < VehicleDetails.GrossWeight && VehicleDetails.GrossWeight > GVW.WeightTo)
                        {
                            vehicleWeight = GVW.VehicleWeightCode;
                        }
                        //else if (GVW.WeightFrom > VehicleDetails.GrossWeight && VehicleDetails.GrossWeight < GVW.WeightTo)
                        //{
                        //    vehicleWeight = GVW.VehicleWeightCode;
                        //}

                    }
                }

                //replacing
                HTMLText = HTMLText.Replace("{POLICYNUMBER}", policy == null ? "" : policy.PolicyNo)
               //.Replace("{POLICYPERIOD}", policy.PolicyEndDate.ToString("dd-MMM-yyyy"))
               .Replace("{POLICYPERIOD}", policy == null ? "" : (policy.PolicySoldDate.AddMonths(mwMonths).ToString("dd-MMM-yyyy") + "-" + policy == null ? "" : policy.PolicyEndDate.AddDays(1).ToString("dd-MMM-yyyy")))
               .Replace("{CUSTOMERNAME}", customer.FirstName + " " + customer.LastName)
               .Replace("{CUSTOMEREMAIL}", customer.Email)
               .Replace("{TELWORK}", customer.MobileNo)
               .Replace("{TELHOME}", customer.OtherTelNo)
               .Replace("{CUSTOMERNATIONALTY}", nationality == null ? "N/A" : nationality.NationalityName)
               .Replace("{CUSTOMERIDTYPE}", idType == null ? "" : idType.IdTypeName)
               .Replace("{CUSTOMERIDNUMBER}", customer.IDNo)
               .Replace("{CUSTOMERGENDER}", customer.Gender == 'M' ? "Male" : "Female")
               .Replace("{ADDRESSLINE1}", customer.Address1)
               .Replace("{ADDRESSLINE2}", customer.Address2)
               .Replace("{ADDRESSLINE3}", customer.Address3)
               .Replace("{ADDRESSLINE4}", customer.Address4)
               .Replace("{BUSINESSADDRESSLINE1}", customer.BusinessAddress1)
               .Replace("{BUSINESSADDRESSLINE2}", customer.BusinessAddress2)
               .Replace("{BUSINESSADDRESSLINE3}", customer.BusinessAddress3)
               .Replace("{BUSINESSNAME}", customer.BusinessName)
                //.Replace("{BUSINESSNAME}", customer.BusinessTelNo)

                // dealer country
                .Replace("{DEALERCOUNTRY}", dealerCountry.CountryName)


               .Replace("{POSTCODE}", customer.PostalCode)
               .Replace("{COUNTRY}", country == null ? "" : country.CountryName)
               .Replace("{CUSTOMERTYPE}", customerType == null ? "" : customerType.CustomerTypeName)
               .Replace("{CUSTOMERDOB}", customer.DateOfBirth == null ? "" : customer.DateOfBirth.Value.ToString("dd-MMM-yyyy"))

               .Replace("{CLAIMLIMITATION}", (Contract.ClaimLimitation * policy.LocalCurrencyConversionRate).ToString("#,##0.00"))
               .Replace("{LIABILITYLIMITATION}", (Contract.LiabilityLimitation * policy.LocalCurrencyConversionRate).ToString("#,##0.00"))
               .Replace("{GVWCODE}", vehicleWeight)

                .Replace("{COMMODITYTYPE}", CommodityType.CommodityTypeDescription)
                .Replace("{PRODUCTNAME}", Product.Productname)
                .Replace("{MAKE}", Make.MakeName)
                .Replace("{MODEL}", Model.ModelName)
                .Replace("{MODELYEAR}", VehicleDetails.ModelYear)
                .Replace("{VIN}", VehicleDetails.VINNo)
                .Replace("{PLATENO}", VehicleDetails.PlateNo)
                .Replace("{DEALERNAME}", Dealer.DealerName)
                .Replace("{DEALERBRANCH}", DealerLocation.Location)
                .Replace("{USAGETYPE}", UsageType.UsageTypeName)
                .Replace("{PURCHASEDATE}", VehicleDetails.ItemPurchasedDate.ToString("dd-MMM-yyyy"))
                .Replace("{DEALERPRICE}", (VehicleDetails.DealerPrice * policy.LocalCurrencyConversionRate).ToString("#,##0.00"))
                .Replace("{PRODUCTPRICE}", (VehicleDetails.VehiclePrice * policy.LocalCurrencyConversionRate).ToString("#,##0.00"))
                .Replace("{VEHICLECATEGORY}", CommodityCategory.CommodityCategoryDescription)
                .Replace("{POLICYSTARTDATE}", policy.PolicyStartDate.AddDays(1).ToString("dd-MMM-yyyy"))
                .Replace("{POLICYENDDATE}", policy.PolicyEndDate.AddDays(1).ToString("dd-MMM-yyyy"))

                .Replace("{MWENDDATE}", mw == null ? "" : policy.MWStartDate.AddMonths(mw.WarrantyMonths).AddDays(-1).ToString("dd-MMM-yyyy"))
                //.Replace("{MWSTARTDATE}", mw == null ? "" : policy.PolicySoldDate.ToString("dd-MMM-yyyy"))
                .Replace("{MWSTARTDATE}", mw == null ? "" : policy.MWStartDate.ToString("dd-MMM-yyyy"))

                  //.Replace("{MWENDDATE}",mw == null ? "" : policy.PolicySoldDate.AddMonths(mwMonths).AddDays(-1).ToString("dd-MMM-yyyy"))
                  //.Replace("{EXTENSIONSTARTDATE}", ExtensionType == null ? "" : VehicleDetails.ItemPurchasedDate.AddMonths(mwMonths).AddDays(1).ToString("dd-MMM-yyyy"))
                  //.Replace("{EXTENSIONENDDATE}", ExtensionType == null ? "" : VehicleDetails.ItemPurchasedDate.AddMonths(mwMonths).AddDays(1).AddMonths(ExtensionType.Month).ToString("dd-MMM-yyyy"))
                  .Replace("{EXTENSIONSTARTDATE}", policy.MWIsAvailable ? policy.MWStartDate.AddMonths(mw.WarrantyMonths).ToString("dd-MMM-yyyy")
                            : policy.PolicySoldDate.ToString("dd-MMM-yyyy"))
                .Replace("{EXTENSIONENDDATE}", policy.MWIsAvailable ? policy.MWStartDate.AddMonths(mw.WarrantyMonths)
                                .AddMonths(IL == null ? 0 : IL.Months).AddDays(-1).ToString("dd-MMM-yyyy")
                            : policy.PolicySoldDate.AddMonths(IL == null ? 0
                            : IL.Months).AddDays(-1).ToString("dd-MMM-yyyy"))

                //.Replace("{EXTENSIONSTARTDATE}", policy.PolicyStartDate.AddDays(1).ToString("dd-MMM-yyyy"))


                //.Replace("{EXTENSIONENDDATE}", policy.PolicySoldDate.AddMonths(mwMonths).AddMonths(IL.Months).AddDays(-1).ToString("dd-MMM-yyyy"))

                .Replace("{EXTENSIONKM}", IL.InsuaranceLimitationName.Split('-').Last())
                .Replace("{ENNUMBER}", EngineCapacity==null ? "" : EngineCapacity.EngineCapacityNumber.ToString())
                .Replace("{MESURETYPE}", EngineCapacity == null ? "" : EngineCapacity.MesureType)
                .Replace("{MWKM}", mw == null ? "" : mw.IsUnlimited ? "Unlimited" : mw.WarrantyKm.ToString())
                .Replace("{GROSSWEIGHT}", VehicleDetails.GrossWeight.ToString())
                .Replace("{DEALNAME}", Contract.DealName)
                .Replace("{CONTRACTNO}", ReinsurerContract == null ? "" : ReinsurerContract.ContractNo)
                .Replace("{EXTENTIONTYPE}", IL.InsuaranceLimitationName)
                .Replace("{COVERTYPE}", WarrantyType.WarrantyTypeDescription)
                .Replace("{POLICYSOLDDATE}", policy == null ? "" : policy.PolicySoldDate.ToString("dd-MMM-yyyy"))
                .Replace("{MILAGE}", policy == null ? "" : policy.HrsUsedAtPolicySale)
                .Replace("{CURRENCY}", Currency.Code)
                .Replace("{SALESPERSON}", InternalUser == null ? "" : (InternalUser.FirstName + " " + InternalUser.LastName))
                .Replace("{GROSSPREMIUM}", policy == null ? "" : (policy.Premium * policy.LocalCurrencyConversionRate).ToString("#,##0.00"))


                .Replace("{PAYMENTMETHOD}", policy == null ? "" : paymentModeid)
                .Replace("{PAYMENTREFNO}", policy == null ? "" : policy.RefNo)
                .Replace("{PAYMENTCOMMENT}", policy == null ? "" : policy.Comment)
                .Replace("{DISCOUNT}", policy == null ? "" : (policy.Discount * policy.LocalCurrencyConversionRate).ToString("#,##0.00"))
                .Replace("{PAYMENTAMOUNT}", policy == null ? "" : ((policy.CustomerPayment + policy.DealerPayment) * policy.LocalCurrencyConversionRate).ToString("#,##0.00"));

            }





            return HTMLText;

        }

        internal static object GetAllReportParamInformationByReportId(Guid reportId)
        {
            object response = null;
            try
            {
                ISession session = EntitySessionManager.GetSession();

                response = session.Query<ReportParameter>()
                      .Where(a => a.ReportId == reportId)
                      .Select(z => new
                      {
                          z.Id,
                          z.ClienSideRegex,
                          z.IsDependOnPreviousParam,
                          z.IsFromField,
                          z.IsIncluedeInDateRange,
                          z.IsServerSideQuery,
                          z.OnSelectNextParam,
                          z.ParamCode,
                          z.ParamName,
                          z.ParamType,
                          z.PreviousParamSequance,
                          z.ReportId,
                          z.Sequance,
                          z.IsShowAll
                      })
                      .OrderBy(b => b.Sequance)
                      .ToArray();
            }

            catch (Exception ex)
            { logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException); }

            return response;
        }

        internal static object GetAllDataForReportFromParentDropdown(Guid reportParamId, Guid reportParamParentValue, Guid parentParamId)
        {
            object response = null;
            try
            {
                ISession session = EntitySessionManager.GetSession();

                ReportParameter reportParameter = session.Query<ReportParameter>()
                    .FirstOrDefault(z => z.Id == reportParamId);

                ReportParameter ParentParameter = session.Query<ReportParameter>()
                   .FirstOrDefault(z => z.Id == parentParamId);

                if (reportParameter != null && ParentParameter != null)
                {
                    var selectQuery = ParentParameter.ServerSideQuery;
                    var replacedQuery = selectQuery
                        .Replace("{" + reportParameter.ParamCode + "}", reportParamParentValue.ToString())
                        .Replace("\r", string.Empty).Replace("\n", string.Empty).Replace("\t", string.Empty);

                    var dropdownData = session.CreateSQLQuery(replacedQuery).SetResultTransformer(Transformers.AliasToBean<ReportDropdownData>())
                    .List<ReportDropdownData>();
                    response = dropdownData;
                }
            }
            catch (Exception ex)
            { logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException); }

            return response;
        }

        internal static object ViewReport(Guid reportId, List<ReportParameterDataRequestDto> reportParameter, string tpaName, string dbConnectionString)
        {
            object response = null;
            bool isAllselected = false;
            String Query = null;
            try
            {

                ISession session = EntitySessionManager.GetSession();
                Report report = session.Query<Report>().FirstOrDefault(a => a.Id == reportId);
                List<ReportParameter> reportParameters = session.Query<ReportParameter>()
                    .Where(a => a.ReportId == reportId).ToList();
                if (report == null)
                {
                    response = "Selected report is invalid";
                    return response;
                }

                ;


                bool isExist = File.Exists(
                  System.Web.HttpContext.Current.Server.MapPath(
                      ConfigurationData.ReportsPath + "\\" + report.ReportCode +
                      "\\" + tpaName.ToLower() + "\\" +
                            report.ReportCode + ".sql")
                  );

                string ReportLocation = isExist ? tpaName.ToLower() : "Default";


                if (GetReportNameById(reportId).ReportName == "DealerInvoiceCode")
                {

                    foreach (ReportParameterDataRequestDto reportParamall in reportParameter)
                    {

                        if (reportParamall.value == "All")
                        {

                            isAllselected = true;
                        }

                    }

                    if (isAllselected)
                    {
                        Query = File.ReadAllText(
                       System.Web.HttpContext.Current.Server.MapPath(
                           ConfigurationData.ReportsPath + "\\" +
                           report.ReportCode +
                           "\\" + ReportLocation + "\\DealerInvoiceCodeAll.sql"));
                        // Query = File.ReadAllText(System.Web.HttpContext.Current.Server.MapPath(ConfigurationData.QueryPath + "\\DealerInvoiceCodeAll.sql"));
                        Query = Query.Replace("\r", string.Empty).Replace("\n", string.Empty);
                        foreach (ReportParameter reportParam in reportParameters)
                        {
                            if (reportParam.ParamType != "DROPDOWN")
                            {
                                Query = Query.Replace("{" + reportParam.ParamCode + "}", reportParameter.FirstOrDefault(z => z.id == reportParam.Id).value.ToString());
                            }
                        }

                    }
                    else
                    {
                        Query = File.ReadAllText(
                        System.Web.HttpContext.Current.Server.MapPath(
                            ConfigurationData.ReportsPath + "\\" +
                            report.ReportCode +
                            "\\" + ReportLocation + "\\" +
                            report.ReportCode + ".sql"));
                        Query = Query.Replace("\r", string.Empty).Replace("\n", string.Empty).Replace("\t", string.Empty);

                        foreach (ReportParameter reportParam in reportParameters)
                        {
                            Query = Query.Replace("{" + reportParam.ParamCode + "}", reportParameter.FirstOrDefault(z => z.id == reportParam.Id).value.ToString());
                        }
                    }


                    //putting in the db so report viewer can view and read it

                    string reportKey = Guid.NewGuid().ToString();
                    ReportDataQuery ReportDataQuery = new ReportDataQuery()
                    {
                        Id = Guid.NewGuid(),
                        ReportKey = Guid.Parse(reportKey),
                        ReportCode = report.ReportCode,
                        ReportDbConnStr = dbConnectionString,
                        ReportQuery = Query,
                        ReportDirectory = ConfigurationData.ReportsPath + "\\" + report.ReportCode + "\\" + ReportLocation
                    };
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.Save(ReportDataQuery, ReportDataQuery.Id);
                        transaction.Commit();
                    }

                    response = reportKey;
                }
                else if (GetReportNameById(reportId).ReportName == "ClaimAuthorization")
                {

                    {
                        Query = File.ReadAllText(
                        System.Web.HttpContext.Current.Server.MapPath(
                            ConfigurationData.ReportsPath + "\\" +
                            report.ReportCode +
                            "\\" + ReportLocation + "\\" +
                            report.ReportCode + ".sql"));
                        //Query = Query.Replace("\r", string.Empty).Replace("\n", string.Empty).Replace("\t", string.Empty);

                        foreach (ReportParameterDataRequestDto reportParamall in reportParameter)
                        {
                            Query = Query

                    .Replace("{claimId}", reportParamall.value);
                        }
                    }


                    //putting in the db so report viewer can view and read it

                    string reportKey = Guid.NewGuid().ToString();
                    ReportDataQuery ReportDataQuery = new ReportDataQuery()
                    {
                        Id = Guid.NewGuid(),
                        ReportKey = Guid.Parse(reportKey),
                        ReportCode = report.ReportCode,
                        ReportDbConnStr = dbConnectionString,
                        ReportQuery = Query,
                        ReportDirectory = ConfigurationData.ReportsPath + "\\" + report.ReportCode + "\\" + ReportLocation
                    };
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.Save(ReportDataQuery, ReportDataQuery.Id);
                        transaction.Commit();
                    }

                    response = reportKey;


                }
                else
                {
                    //--comment by Sachith
                    /*
                    foreach (ReportParameterDataRequestDto reportParamall in reportParameter)
                    {

                        if (reportParamall.value == "All")
                        {

                            isAllselected = true;
                        }

                    }

                    if (isAllselected )
                    {
                        Query = File.ReadAllText(
                       System.Web.HttpContext.Current.Server.MapPath(
                           ConfigurationData.ReportsPath + "\\" +
                           report.ReportCode +
                           "\\" + ReportLocation + "\\DealerInvoiceCodeAll.sql"));
                        // Query = File.ReadAllText(System.Web.HttpContext.Current.Server.MapPath(ConfigurationData.QueryPath + "\\DealerInvoiceCodeAll.sql"));
                        Query = Query.Replace("\r", string.Empty).Replace("\n", string.Empty);
                        foreach (ReportParameter reportParam in reportParameters)
                        {
                            if (reportParam.ParamType != "DROPDOWN")
                            {
                                Query = Query.Replace("{" + reportParam.ParamCode + "}", reportParameter.FirstOrDefault(z => z.id == reportParam.Id).value.ToString());
                            }
                        }

                    }
                    else
                    {
                        Query = File.ReadAllText(
                        System.Web.HttpContext.Current.Server.MapPath(
                            ConfigurationData.ReportsPath + "\\" +
                            report.ReportCode +
                            "\\" + ReportLocation + "\\" +
                            report.ReportCode + ".sql"));
                        Query = Query.Replace("\r", string.Empty).Replace("\n", string.Empty).Replace("\t", string.Empty);

                        foreach (ReportParameter reportParam in reportParameters)
                        {
                            Query = Query.Replace("{" + reportParam.ParamCode + "}", reportParameter.FirstOrDefault(z => z.id == reportParam.Id).value.ToString());
                        }
                    }
                    */

                    Query = File.ReadAllText(
                            System.Web.HttpContext.Current.Server.MapPath(
                                ConfigurationData.ReportsPath + "\\" +
                                report.ReportCode +
                                "\\" + ReportLocation + "\\" +
                                report.ReportCode + ".sql"));
                    Query = Query.Replace("\r", string.Empty).Replace("\n", string.Empty).Replace("\t", string.Empty);

                    foreach (ReportParameter reportParam in reportParameters)
                    {
                        Query = Query.Replace("{" + reportParam.ParamCode + "}", reportParameter.FirstOrDefault(z => z.id == reportParam.Id).value.ToString());
                    }


                    //putting in the db so report viewer can view and read it

                    string reportKey = Guid.NewGuid().ToString();
                    ReportDataQuery ReportDataQuery = new ReportDataQuery()
                    {
                        Id = Guid.NewGuid(),
                        ReportKey = Guid.Parse(reportKey),
                        ReportCode = report.ReportCode,
                        ReportDbConnStr = dbConnectionString,
                        ReportQuery = Query,
                        ReportDirectory = ConfigurationData.ReportsPath + "\\" + report.ReportCode + "\\" + ReportLocation
                    };
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.Save(ReportDataQuery, ReportDataQuery.Id);
                        transaction.Commit();
                    }

                    response = reportKey;

                }
            }

            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return response;
        }


        internal static object ExcelDownloadReport(Guid reportId, List<ReportParameterDataRequestDto> reportParameter, string tpaName, string dbConnectionString)
        {

            ExcelExportResponseDto excelExportResponse = new ExcelExportResponseDto();

            object response = null;
            bool isAllselected = false;
            String Query = null;
            try
            {

                ISession session = EntitySessionManager.GetSession();
                Report report = session.Query<Report>().FirstOrDefault(a => a.Id == reportId);
                List<ReportParameter> reportParameters = session.Query<ReportParameter>()
                    .Where(a => a.ReportId == reportId).ToList();
                if (report == null)
                {
                    response = "Selected report is invalid";
                    return response;
                }

                bool isExist = File.Exists(
                    System.Web.HttpContext.Current.Server.MapPath(
                        ConfigurationData.ReportsPath + "\\" + report.ReportCode +
                        "\\" + tpaName.ToLower() + "\\" +
                              report.ReportCode + ".sql")
                    );

                string ReportLocation = isExist ? tpaName.ToLower() : "Default";

                foreach (ReportParameterDataRequestDto reportParamall in reportParameter)
                {

                    if (reportParamall.value == "All")
                    {

                        isAllselected = true;
                    }

                }

                if (isAllselected)
                {
                    Query = File.ReadAllText(
                   System.Web.HttpContext.Current.Server.MapPath(
                       ConfigurationData.ReportsPath + "\\" +
                       report.ReportCode +
                       "\\" + ReportLocation + "\\DealerInvoiceCodeAll.sql"));
                    // Query = File.ReadAllText(System.Web.HttpContext.Current.Server.MapPath(ConfigurationData.QueryPath + "\\DealerInvoiceCodeAll.sql"));
                    Query = Query.Replace("\r", string.Empty).Replace("\n", string.Empty);
                    foreach (ReportParameter reportParam in reportParameters)
                    {
                        if (reportParam.ParamType != "DROPDOWN")
                        {
                            Query = Query.Replace("{" + reportParam.ParamCode + "}", reportParameter.FirstOrDefault(z => z.id == reportParam.Id).value.ToString());
                        }
                    }

                }
                else
                {
                    Query = File.ReadAllText(
                    System.Web.HttpContext.Current.Server.MapPath(
                        ConfigurationData.ReportsPath + "\\" +
                        report.ReportCode +
                        "\\" + ReportLocation + "\\" +
                        report.ReportCode + ".sql"));
                    Query = Query.Replace("\r", string.Empty).Replace("\n", string.Empty).Replace("\t", string.Empty);

                    foreach (ReportParameter reportParam in reportParameters)
                    {
                        Query = Query.Replace("{" + reportParam.ParamCode + "}", reportParameter.FirstOrDefault(z => z.id == reportParam.Id).value.ToString());
                    }
                }

                //IList<DealerInvoiceCodeReportColumnHeader> bb = session.Query<DealerInvoiceCodeReportColumnHeader>().OrderBy(a => a.Sequance).ToList();

                List<DealerInvoicecodeReportColumnHeaders> columnHdrlist = session.Query<DealerInvoicecodeReportColumnHeaders>().OrderBy(a => a.Sequance).ToList();

                //var columnHdrlist = session.Query<DealerInvoiceCodeReportColumnHeader>().OrderBy(a => a.Sequance).ToList();


                // columnHdrlist = columnHdrlist.OrderBy(a => a.Sequance).ToList();

                excelExportResponse = new ExcelExportResponseDto()
                {
                    //BordxData = FinalPolicyTableList,
                    InvoiceCodeColumnHeaders = new DBDTOTransformer().GetDealerInvoiceReportColumnHeadersToDto(columnHdrlist),
                    //BordxReportColumns = new DBDTOTransformer().GetBordxReportColumnsToDto(columns),
                    //TpaName = TpaName,
                    //BordxYear = bordx.Year.ToString(),
                    //BordxMonth = bordx.Month.ToString(),
                    //CountryTaxInfo = MasterTaxInformationListByCountry,
                    //currentUSDConversionRate = decimal.Parse(currencyDetails.Split('-')[0]),
                    //currencyCode = currencyDetails.Split('-')[1],
                    //tpaLogo = image.DisplayImageSrc,
                    //reportName = ReportName.First().ReportName,
                    //BordxReportTemplateName = bordxReportTemplate.Name,
                    //BordxStartDate = bordx.StartDate.ToString("dd-MMM-yyyy"),
                    //BordxEndDate = bordx.EndDate.ToString("dd-MMM-yyyy")
                };

                //putting in the db so report viewer can view and read it

                string reportKey = Guid.NewGuid().ToString();
                ReportDataQuery ReportDataQuery = new ReportDataQuery()
                {
                    Id = Guid.NewGuid(),
                    ReportKey = Guid.Parse(reportKey),
                    ReportCode = report.ReportCode,
                    ReportDbConnStr = dbConnectionString,
                    ReportQuery = Query,
                    ReportDirectory = ConfigurationData.ReportsPath + "\\" + report.ReportCode + "\\" + ReportLocation
                };
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Save(ReportDataQuery, ReportDataQuery.Id);
                    transaction.Commit();
                }

                response = reportKey;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return response;
        }


        //internal static BordxExportResponseDto GetConfirmedBordxForExport(ExportPoliciesToExcelByBordxIdRequestDto ExportPoliciesToExcelByBordxIdRequestDto)
        //{
        //    BordxExportResponseDto Response = new BordxExportResponseDto();

        //    try
        //    {
        //        if (IsGuid(ExportPoliciesToExcelByBordxIdRequestDto.bordxId.ToString())
        //        && IsGuid(ExportPoliciesToExcelByBordxIdRequestDto.userId.ToString()))
        //        {
        //            ISession session = EntitySessionManager.GetSession();
        //            List<BordxTaxInfo> MasterTaxInformationListByCountry = new List<BordxTaxInfo>();

        //            //bordx
        //            Bordx bordx = session.Query<Bordx>().Where(a => a.Id == ExportPoliciesToExcelByBordxIdRequestDto.bordxId).FirstOrDefault();


        //            String reportNameQuery = File.ReadAllText(System.Web.HttpContext.Current.Server.MapPath(ConfigurationData.QueryPath + "\\BordxExportReportName.sql"));
        //            reportNameQuery = reportNameQuery.Replace("{BordxReportTemplateId}", ExportPoliciesToExcelByBordxIdRequestDto.bordxReportTemplateId.ToString());
        //            reportNameQuery = reportNameQuery.Replace("{bordexId}", ExportPoliciesToExcelByBordxIdRequestDto.bordxId.ToString());
        //            var ReportName = session.CreateSQLQuery(reportNameQuery).SetResultTransformer(Transformers.AliasToBean<ExcelBordxReportNameRequestDto>()).List<ExcelBordxReportNameRequestDto>();


        //            String Query = File.ReadAllText(System.Web.HttpContext.Current.Server.MapPath(ConfigurationData.QueryPath + "\\BordxExport.sql"));
        //            Query = Query.Replace("{bordexId}", ExportPoliciesToExcelByBordxIdRequestDto.bordxId.ToString());
        //            var Policies = session.CreateSQLQuery(Query).SetResultTransformer(Transformers.AliasToBean<AddBordxSummeryRequestDto>())
        //            .List<AddBordxSummeryRequestDto>();

        //            List<Guid> AllCountryList = Policies.Select(a => a.BaseCountryId).Distinct().ToList();
        //            List<DataTable> FinalPolicyTableList = new List<DataTable>();
        //            foreach (Guid countryId in AllCountryList)
        //            {

        //                MasterTaxInformationListByCountry.Add(DBDTOTransformer.Instance.GetBordxTaxDetailsByCountryId(countryId));
        //                DataTable ConvertedTable = CreateDataTable(Policies.Where(a => a.BaseCountryId == countryId).ToList());
        //                DataTable FinalPolicyTable = new DataTable();
        //                FinalPolicyTable = ConvertedTable.Copy();
        //                FinalPolicyTableList.Add(FinalPolicyTable);
        //            }
        //            //get usd currency rate @ now
        //            string currencyDetails = new CurrencyEntityManager().GetCurrentUSDRateByCountryId(AllCountryList.FirstOrDefault());

        //            //get headers and columns data

        //            List<BordxReportColumns> bordxReportcolumnslist = session.Query<BordxReportColumns>().Where(a => a.IsActive).ToList();
        //            List<BordxReportTemplateDetails> bordxReportTemplateDetailsList = session.Query<BordxReportTemplateDetails>().Where(a => a.IsActive && a.BordxReportTemplateId == ExportPoliciesToExcelByBordxIdRequestDto.bordxReportTemplateId).ToList();
        //            BordxReportTemplate bordxReportTemplate = session.Query<BordxReportTemplate>().Where(a => a.Id == bordxReportTemplateDetailsList.FirstOrDefault().BordxReportTemplateId).FirstOrDefault();

        //            List<BordxReportColumns> columns = (from bordxReportcolumns in bordxReportcolumnslist
        //                                                join template in bordxReportTemplateDetailsList
        //                                                on bordxReportcolumns.Id equals template.BordxReportColumnsId
        //                                                select new BordxReportColumns
        //                                                {
        //                                                    Id = bordxReportcolumns.Id,
        //                                                    DisplayName = bordxReportcolumns.DisplayName,
        //                                                    HeaderId = bordxReportcolumns.HeaderId,
        //                                                    IsActive = bordxReportcolumns.IsActive,
        //                                                    KeyName = bordxReportcolumns.KeyName,
        //                                                    Sequance = bordxReportcolumns.Sequance
        //                                                }).ToList();

        //            var columnHdrlist = session.Query<BordxReportColumnHeaders>().OrderBy(a => a.Sequance).ToList();

        //            var colHeaders = (from columnHeader in columnHdrlist
        //                              join column in columns
        //                              on columnHeader.Id equals column.HeaderId
        //                              group columnHeader by new { columnHeader.Id, columnHeader.HeaderName, columnHeader.Sequance } into grpHdr
        //                              select new BordxReportColumnHeaders
        //                              {
        //                                  Id = grpHdr.Key.Id,
        //                                  HeaderName = grpHdr.Key.HeaderName,
        //                                  Sequance = grpHdr.Key.Sequance
        //                              }).ToList();

        //            List<BordxReportColumnHeaders> columnHeaders = colHeaders.OrderBy(a => a.Sequance).ToList();

        //            //List<BordxReportColumns> columns = session.Query<BordxReportColumns>().Where(a => a.IsActive).ToList();


        //            var TpaName = session.Query<TPA>().FirstOrDefault().Name;
        //            var TpaLogo = session.Query<TPA>().FirstOrDefault().Logo;

        //            var image = new ImageEntityManager().GetImageById(TpaLogo);

        //            Response = new BordxExportResponseDto()
        //            {
        //                BordxData = FinalPolicyTableList,
        //                BordxReportColumnHeaders = new DBDTOTransformer().GetBordxReportColumnHeadersToDto(columnHeaders),
        //                BordxReportColumns = new DBDTOTransformer().GetBordxReportColumnsToDto(columns),
        //                TpaName = TpaName,
        //                BordxYear = bordx.Year.ToString(),
        //                BordxMonth = bordx.Month.ToString(),
        //                CountryTaxInfo = MasterTaxInformationListByCountry,
        //                currentUSDConversionRate = decimal.Parse(currencyDetails.Split('-')[0]),
        //                currencyCode = currencyDetails.Split('-')[1],
        //                tpaLogo = image.DisplayImageSrc,
        //                reportName = ReportName.First().ReportName,
        //                BordxReportTemplateName = bordxReportTemplate.Name,
        //                BordxStartDate = bordx.StartDate.ToString("dd-MMM-yyyy"),
        //                BordxEndDate = bordx.EndDate.ToString("dd-MMM-yyyy")
        //            };

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
        //    }


        //    return Response;
        //}

        internal static Report GetReportNameById(Guid reportId)
        {
            Report report = null;
            try
            {
                ISession session = EntitySessionManager.GetSession();

                report = session.Query<Report>()
                      .Where(a => a.Id == reportId)
                      .FirstOrDefault();

                return report;
            }



            catch (Exception ex)
            { logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException); }


            return report;

        }


        internal static object GetAllDataForReportDropdownElement(Guid reportParameterId)
        {
            object response = null;
            try
            {
                ISession session = EntitySessionManager.GetSession();
                ReportParameter reportParameter = session.Query<ReportParameter>()
                    .FirstOrDefault(z => z.Id == reportParameterId);
                if (reportParameter != null)
                {
                    var dropdownData = session.CreateSQLQuery(reportParameter.SelectQuery).SetResultTransformer(Transformers.AliasToBean<ReportDropdownData>())
                    .List<ReportDropdownData>();
                    response = dropdownData;
                }
            }
            catch (Exception ex)
            { logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException); }

            return response;
        }

        internal string SetupClaimAuthorizationReport(string reportHTMLBody, string reportHTMLDtl, Guid claimId, Guid userId, string tpaName)
        {
            string response = string.Empty;
            try
            {
                ISession session = EntitySessionManager.GetSession();

                CommonEntityManager commEn = new CommonEntityManager();
                String QueryMain = "";
                String QuerySub = "";
                Claim claim = session.Query<Claim>().Where(a => a.Id == claimId).FirstOrDefault();
                string CommodityType = commEn.GetCommodityTypeNameById(claim.CommodityTypeId);

                if (CommodityType == "Tire")
                {
                    QueryMain = File.ReadAllText(System.Web.HttpContext.Current.Server.MapPath(ConfigurationData.QueryPath + "\\ClaimAuthorizationReportMainOther.sql"));
                    QueryMain = QueryMain.Replace("{claimId}", claimId.ToString());

                    QuerySub = File.ReadAllText(System.Web.HttpContext.Current.Server.MapPath(ConfigurationData.QueryPath + "\\ClaimAuthorizationReportSub.sql"));
                    QuerySub = QuerySub.Replace("{claimId}", claimId.ToString());
                }
                else
                {
                    QueryMain = File.ReadAllText(System.Web.HttpContext.Current.Server.MapPath(ConfigurationData.QueryPath + "\\ClaimAuthorizationReportMain.sql"));
                    QueryMain = QueryMain.Replace("{claimId}", claimId.ToString());

                    QuerySub = File.ReadAllText(System.Web.HttpContext.Current.Server.MapPath(ConfigurationData.QueryPath + "\\ClaimAuthorizationReportSub.sql"));
                    QuerySub = QuerySub.Replace("{claimId}", claimId.ToString());
                }



                ClaimAuthorizationReportData claimAuthorizationReportData = session.CreateSQLQuery(QueryMain)
                    .SetResultTransformer(Transformers.AliasToBean<ClaimAuthorizationReportData>()).UniqueResult<ClaimAuthorizationReportData>();

                List<ClaimAuthorizationReportDetailData> claimAuthorizationReportDataDetail = session.CreateSQLQuery(QuerySub)
                   .SetResultTransformer(Transformers.AliasToBean<ClaimAuthorizationReportDetailData>()).List<ClaimAuthorizationReportDetailData>().ToList();

                string formattedReportDtl = string.Empty;
                int counter = 1;
                decimal TotalPartPrice = decimal.Zero, TotalLabourCost = decimal.Zero;
                foreach (ClaimAuthorizationReportDetailData claimItmDtl in claimAuthorizationReportDataDetail)
                {
                    if (claimItmDtl.ItemCode.Trim().ToUpper() == "P")
                    {


                        ClaimAuthorizationReportDetailData relavantLabour = claimAuthorizationReportDataDetail.FirstOrDefault(a => a.Remark.EndsWith(claimItmDtl.PartNumber));
                        if (relavantLabour == null)
                        {
                            relavantLabour = new ClaimAuthorizationReportDetailData();
                        }
                        if (claimItmDtl.IsApproved)
                        {
                            TotalPartPrice += claimItmDtl.TotalPrice;
                            TotalLabourCost += relavantLabour.TotalPrice;
                        }
                        formattedReportDtl += reportHTMLDtl
                            .Replace("{d_sn}", counter.ToString())
                            .Replace("{d_PCost}", (Math.Round(claimItmDtl.TotalGrossPrice * claimItmDtl.ConversionRate * 100) / 100).ToString("N", CultureInfo.InvariantCulture))
                            .Replace("{d_PDisc}", (Math.Round(claimItmDtl.DiscountAmount * claimItmDtl.ConversionRate * 100) / 100).ToString("N", CultureInfo.InvariantCulture))
                            .Replace("{d_PGW}", (Math.Round(claimItmDtl.GoodWillAmount * claimItmDtl.ConversionRate * 100) / 100).ToString("N", CultureInfo.InvariantCulture))
                            .Replace("{d_PTotal}", (Math.Round(claimItmDtl.TotalPrice * claimItmDtl.ConversionRate * 100) / 100).ToString("N", CultureInfo.InvariantCulture))
                            .Replace("{d_PartNo}", claimItmDtl.PartNumber)
                            .Replace("{d_Descr}", claimItmDtl.PartName)
                            .Replace("{d_PLttl}", (Math.Round(relavantLabour.TotalPrice * claimItmDtl.ConversionRate * 100) / 100).ToString("N", CultureInfo.InvariantCulture))
                            .Replace("{d_Status}", claimItmDtl.IsApproved ? "Approved" : "Rejected")
                            .Replace("{d_LCost}", (Math.Round(relavantLabour.TotalGrossPrice * claimItmDtl.ConversionRate * 100) / 100).ToString("N", CultureInfo.InvariantCulture))
                            .Replace("{d_LGW}", (Math.Round(relavantLabour.GoodWillAmount * claimItmDtl.ConversionRate * 100) / 100).ToString("N", CultureInfo.InvariantCulture))
                            .Replace("{d_LDisc}", (Math.Round(relavantLabour.DiscountAmount * claimItmDtl.ConversionRate * 100) / 100).ToString("N", CultureInfo.InvariantCulture))
                            .Replace("{d_PLTotal}", (Math.Round((claimItmDtl.TotalPrice * claimItmDtl.ConversionRate * 100))).ToString("N", CultureInfo.InvariantCulture))
                            .Replace("{d_Notes}", claimItmDtl.Remark);

                        counter++;
                    }
                    else if (claimItmDtl.ItemCode.Trim().ToUpper() == "S")
                    {
                        if (claimItmDtl.IsApproved)
                        {
                            TotalPartPrice += claimItmDtl.TotalPrice;
                        }

                        formattedReportDtl += reportHTMLDtl
                           .Replace("{d_sn}", counter.ToString())
                           .Replace("{d_PCost}", (Math.Round(claimItmDtl.TotalGrossPrice * claimItmDtl.ConversionRate * 100) / 100).ToString("N", CultureInfo.InvariantCulture))
                           .Replace("{d_PDisc}", (Math.Round(claimItmDtl.DiscountAmount * claimItmDtl.ConversionRate * 100) / 100).ToString("N", CultureInfo.InvariantCulture))
                           .Replace("{d_PGW}", (Math.Round(claimItmDtl.GoodWillAmount * claimItmDtl.ConversionRate * 100) / 100).ToString("N", CultureInfo.InvariantCulture))
                           .Replace("{d_PTotal}", (Math.Round(claimItmDtl.TotalPrice * claimItmDtl.ConversionRate * 100) / 100).ToString("N", CultureInfo.InvariantCulture))
                           .Replace("{d_PartNo}", "Sundry")
                           .Replace("{d_Descr}", claimItmDtl.ItemCode)
                           .Replace("{d_PLttl}", "0.00")
                           .Replace("{d_Status}", claimItmDtl.IsApproved ? "Approved" : "Rejected")
                           .Replace("{d_LCost}", "0.00")
                           .Replace("{d_LGW}", "0.00")
                           .Replace("{d_LDisc}", "0.00")
                           .Replace("{d_PLTotal}", (Math.Round(claimItmDtl.TotalPrice * claimItmDtl.ConversionRate * 100) / 100).ToString("N", CultureInfo.InvariantCulture))
                           .Replace("{d_Notes}", claimItmDtl.Remark);
                        counter++;
                    }
                }

                string formattedReportBody = reportHTMLBody
                    .Replace("{d_Dealer}", claimAuthorizationReportData.DealerName)
                    .Replace("{d_ClaimNo}", claimAuthorizationReportData.ClaimNumber)
                    .Replace("{d_CountryRegion}", claimAuthorizationReportData.CountryCity)
                    .Replace("{d_CustomerName}", claimAuthorizationReportData.CustomerName)
                    .Replace("{d_BooklateNo}", claimAuthorizationReportData.BookletNumber)
                    .Replace("{d_PolicyNo}", claimAuthorizationReportData.PolicyNo)
                    .Replace("{d_NewUsed}", claimAuthorizationReportData.Status)
                    .Replace("{d_Vin}", claimAuthorizationReportData.VINNo)
                    .Replace("{d_PlateNo}", claimAuthorizationReportData.PlateNo)
                    .Replace("{d_Make}", claimAuthorizationReportData.MakeName)
                    .Replace("{d_Model}", claimAuthorizationReportData.ModelName)
                    .Replace("{d_FailureDate}", claimAuthorizationReportData.FailureDate.ToString("dd-MMM-yyyy"))
                    .Replace("{d_Mileage}", string.Format("{0:0}", claimAuthorizationReportData.ClaimMileageKm) + " km")

                    .Replace("{d_CustomerComplaint}", claimAuthorizationReportData.CustomerComplaint)
                    .Replace("{d_DefectAnalysis}", claimAuthorizationReportData.DealerComment)
                    .Replace("{d_ClaimSubmittedBy}", claimAuthorizationReportData.ClaimSubmittedUser)
                    .Replace("{d_ClaimSubmittedDate}", claimAuthorizationReportData.ClaimDate.ToString("dd-MMM-yyyy"))
                    .Replace("{d_ClaimProcessededDate}", claimAuthorizationReportData.ApprovedDate.ToString("dd-MMM-yyyy"))
                    .Replace("{d_ClaimProcessedBy}", claimAuthorizationReportData.ClaimApprovedUser)

                    .Replace("{d_Remarks}", claimAuthorizationReportData.EngineerComment)


                    .Replace("{d_ttlGrand}", (Math.Round((TotalLabourCost + TotalPartPrice) * claimAuthorizationReportData.ConversionRate * 100) / 100).ToString("N", CultureInfo.InvariantCulture))
                    .Replace("{d_ttlPayable}", (Math.Round((TotalLabourCost + TotalPartPrice) * claimAuthorizationReportData.ConversionRate * 100) / 100).ToString("N", CultureInfo.InvariantCulture))
                    .Replace("{d_ttlParts}", (Math.Round(TotalPartPrice * claimAuthorizationReportData.ConversionRate * 100) / 100).ToString("N", CultureInfo.InvariantCulture))
                    .Replace("{d_ttlLabour}", (Math.Round(TotalLabourCost * claimAuthorizationReportData.ConversionRate * 100) / 100).ToString("N", CultureInfo.InvariantCulture))

                    .Replace("{d_ClaimItem}", formattedReportDtl);

                response = formattedReportBody;
            }
            catch (Exception ex)
            { logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException); }

            return response;
        }

        public String SetupClaimChequeStatement(String HTMLText, Guid chequePaymentId)
        {
            ISession session = EntitySessionManager.GetSession();
            CommonEntityManager cem = new CommonEntityManager();

            ClaimChequePayment chequeData = session.Query<ClaimChequePayment>().FirstOrDefault(a => a.Id == chequePaymentId);

            if (chequeData == null)
            {
                return String.Empty;
            }

            var query = from claimchequedetail in session.Query<ClaimChequePaymentDetail>()
                        where claimchequedetail.ClaimChequePaymentId == chequePaymentId
                        join claimbatchgroup in session.Query<ClaimBatchGroup>() on claimchequedetail.ClaimBatchGroupId equals claimbatchgroup.Id
                        where claimbatchgroup.IsAllocatedForCheque == true
                        join claimGroupClaim in session.Query<ClaimGroupClaim>() on claimbatchgroup.Id equals claimGroupClaim.ClaimGroupId
                        join claim in session.Query<Claim>() on claimGroupClaim.ClaimId equals claim.Id
                        select new { claim.ClaimNumber, claim.DealerComment, claim.TotalClaimAmount };

            var result = query.ToList();

            string dyanmicContent = "";
            string content = "";

            foreach (var item in result)
            {
                content = "<tr style='height: 1pt'><td /><td rowspan='3' colspan='15' valign='top' class='style-7'>CLAIM NUMBER : " + item.ClaimNumber + "</td><td /><td /><td /><td /><td /><td /><td /><td /><td /><td /><td /><td /><td /><td /><td /><td /></tr>"
                            + "<tr style='height: 18pt'><td /><td /><td /><td /><td colspan='9' valign='top' class='style-8'>CLAIM AMOUNT : " + item.TotalClaimAmount.ToString("N", CultureInfo.InvariantCulture) + "</td><td /><td /><td /><td /></tr>"
                            + "<tr style='height: 3pt'><td /><td /><td /><td /><td /><td /><td /><td /><td /><td /><td /><td /><td /><td /><td /><td /><td /> </tr>"
                            + "<tr style='height: 1pt'><td /><td rowspan='3' colspan='15' valign='top' class='style-7'>COMMENT : " + item.DealerComment + "</td><td /><td /><td /><td /><td /><td /><td /><td /><td /><td /><td /><td /><td /><td /><td /><td /></tr>"
                            + "<tr style='height: 18pt'><td /><td /><td /><td /><td colspan='9' valign='top' class='style-8'></td><td /><td /><td /><td /></tr>"
                            + "<tr style='height: 3pt'><td /><td /><td /><td /><td /><td /><td /><td /><td /><td /><td /><td /><td /><td /><td /><td /><td /> </tr>"
                            + "<tr style='height: 1pt'><td /><td /><td class='style-15' /><td class='style-16' /><td class='style-16' /><td class='style-16' /><td class='style-16' /><td class='style-16' /><td class='style-16' /><td class='style-16' /><td class='style-16' /><td class='style-16' /><td class='style-16' /><td class='style-16' /><td class='style-16' /><td class='style-16' /><td class='style-16' /><td class='style-16' /><td class='style-16' /><td class='style-16' /><td class='style-16' /><td class='style-16' /><td class='style-16' /><td class='style-16' /><td class='style-16' /><td class='style-16' /><td class='style-16' /><td class='style-16' /><td class='style-17' /><td class='style-18' /><td /><td /> </tr>"
                            + "<tr style='height: 2pt'><td /><td /><td /><td /><td /><td /><td /><td /><td /><td /><td /><td /><td /><td /><td /><td /><td /><td /><td /><td /><td /><td /><td /><td /><td /><td /><td /><td /><td /><td /><td /><td /></tr>";

                dyanmicContent = dyanmicContent + content;
            }

            HTMLText = HTMLText.Replace("{CHEQUENUMBER}", chequeData.ChequeNo)
                                 .Replace("{CHEQUEDATE}", chequeData.ChequeDate.ToString("dd-MMM-yyyy"))
                                 .Replace("{CHEQUEAMOUNT}", chequeData.ChequeAmount.ToString("N", CultureInfo.InvariantCulture))
                                 .Replace("{CHEQUECOMMENT}", chequeData.Comment)
                                 .Replace("{dynamic}", dyanmicContent);

            return HTMLText;
            //replacing
            // HTMLText = HTMLText.Replace("{POLICYNUMBER}", policy == null ? "" : policy.PolicyNo)
            //.Replace("{POLICYPERIOD}", policy == null ? "" : (policy.PolicySoldDate.AddMonths(mwMonths).ToString("dd-MMM-yyyy") + "-" + policy == null ? "" : policy.PolicyEndDate.ToString("dd-MMM-yyyy")))


            //.Replace("{PAYMENTMETHOD}", policy == null ? "" : paymentModeid)
            //.Replace("{PAYMENTREFNO}", policy == null ? "" : policy.RefNo)
            //.Replace("{PAYMENTCOMMENT}", policy == null ? "" : policy.Comment)
            //.Replace("{DISCOUNT}", policy == null ? "" : (policy.Discount * policy.LocalCurrencyConversionRate).ToString("N", CultureInfo.InvariantCulture))
            //.Replace("{PAYMENTAMOUNT}", policy == null ? "" : ((policy.CustomerPayment + policy.DealerPayment) * policy.LocalCurrencyConversionRate).ToString("N", CultureInfo.InvariantCulture));




            //return HTMLText;

        }

        //unauthorized report data access via cache key
        public CacheReportData GetReportCacheDataByReportId(string reportCacheKey, string jwt)
        {
            CacheReportData response = new CacheReportData();
            response.reportData = new DataTable();
            try
            {
                string dbConnectionString = string.Empty;
                try
                {
                    Common.JWTHelper JWTHelper = new Common.JWTHelper(jwt);
                    Dictionary<string, object> str = JWTHelper.DecodeAuthenticationToken();
                    string dbName = str.FirstOrDefault(f => f.Key == "dbName").Value.ToString();
                    if (dbName != "")
                    {
                        TASEntitySessionManager.OpenSession();
                        string tasTpaConnString = TASTPAEntityManager.GetTPAConnStringBydbName(dbName);
                        TASEntitySessionManager.CloseSession();
                        if (tasTpaConnString != "")
                        {
                            dbConnectionString = tasTpaConnString;
                            EntitySessionManager.OpenSession(dbConnectionString);
                            if (!JWTHelper.checkTokenValidity(Convert.ToInt32(ConfigurationData.tasTokenValidTime.ToString())))
                            {
                                return response;
                            }
                            EntitySessionManager.CloseSession();
                        }
                    }
                }
                finally
                {
                    EntitySessionManager.CloseSession();
                }

                EntitySessionManager.OpenSession(dbConnectionString);

                Guid reportKey = Guid.Parse(reportCacheKey);
                ISession session = EntitySessionManager.GetSession();
                ReportDataQuery data = session.Query<ReportDataQuery>()
                    .FirstOrDefault(a => a.ReportKey == reportKey);
                if (data != null)
                {

                    bool isExist = File.Exists(
                   System.Web.HttpContext.Current.Server.MapPath(
                       data.ReportDirectory + "//" + data.ReportCode + ".rdlc"));
                    if (isExist)
                    {
                        //using sql :(
                        using (SqlConnection conn = new SqlConnection())
                        {
                            conn.ConnectionString = data.ReportDbConnStr;
                            conn.Open();

                            // use the connection here
                            SqlCommand command = new SqlCommand(data.ReportQuery, conn);
                            SqlDataAdapter da = new SqlDataAdapter(command);
                            // this will query your database and return the result to your datatable
                            da.Fill(response.reportData);
                            conn.Close();
                            da.Dispose();
                        }

                        response.reportPath = data.ReportDirectory + "//" + data.ReportCode + ".rdlc";
                        response.isValid = true;
                        response.reportCode = data.ReportCode;
                    }
                }
            }
            catch (Exception ex)
            { logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException); }
            finally
            {
                EntitySessionManager.CloseSession();
            }
            return response;
        }

        #region "Explorer"

        internal static object GetAllReportInformationByUserId(Guid loggedInUserId)
        {
            object response = null;
            try
            {
                ISession session = EntitySessionManager.GetSession();
                //user id report filtering
                //todo:
                response = session.Query<Report>()
                  .Select(z => new
                  {
                      z.Id,
                      z.ReportCode,
                      z.ReportName,

                  }).ToArray();
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);

            }
            return response;
        }


        internal string SetupClaimAuthorizationTireReport(string reportHTMLBody, string reportHTMLDtl, Guid claimId, Guid userId, string tpaName)
        {
            string response = string.Empty;
            try
            {
                ISession session = EntitySessionManager.GetSession();

                CommonEntityManager commEn = new CommonEntityManager();
                String QueryMain = "";
                String QuerySub = "";
                Claim claim = session.Query<Claim>().Where(a => a.Id == claimId).FirstOrDefault();
                string CommodityType = commEn.GetCommodityTypeNameById(claim.CommodityTypeId);
                InvoiceCodeDetails InvoiceCodeDetails = session.Query<InvoiceCodeDetails>().Where(a => a.PolicyId == claim.PolicyId).FirstOrDefault();
                InvoiceCode InvoiceCode = session.Query<InvoiceCode>().Where(a => a.Id == InvoiceCodeDetails.InvoiceCodeId).FirstOrDefault();


                if (CommodityType == "Tire")
                {
                    QueryMain = File.ReadAllText(System.Web.HttpContext.Current.Server.MapPath(ConfigurationData.QueryPath + "\\ClaimAuthorizationReportMainOther.sql"));
                    QueryMain = QueryMain.Replace("{claimId}", claimId.ToString());

                    QuerySub = File.ReadAllText(System.Web.HttpContext.Current.Server.MapPath(ConfigurationData.QueryPath + "\\ClaimAuthorizationReportSub.sql"));
                    QuerySub = QuerySub.Replace("{claimId}", claimId.ToString());
                }
                else
                {
                    QueryMain = File.ReadAllText(System.Web.HttpContext.Current.Server.MapPath(ConfigurationData.QueryPath + "\\ClaimAuthorizationReportMain.sql"));
                    QueryMain = QueryMain.Replace("{claimId}", claimId.ToString());

                    QuerySub = File.ReadAllText(System.Web.HttpContext.Current.Server.MapPath(ConfigurationData.QueryPath + "\\ClaimAuthorizationReportSub.sql"));
                    QuerySub = QuerySub.Replace("{claimId}", claimId.ToString());
                }



                ClaimAuthorizationReportData claimAuthorizationReportData = session.CreateSQLQuery(QueryMain)
                    .SetResultTransformer(Transformers.AliasToBean<ClaimAuthorizationReportData>()).UniqueResult<ClaimAuthorizationReportData>();

                List<ClaimAuthorizationReportDetailData> claimAuthorizationReportDataDetail = session.CreateSQLQuery(QuerySub)
                   .SetResultTransformer(Transformers.AliasToBean<ClaimAuthorizationReportDetailData>()).List<ClaimAuthorizationReportDetailData>().ToList();

                string formattedReportDtl = string.Empty;
                int counter = 1;
                decimal TotalPartPrice = decimal.Zero, TotalLabourCost = decimal.Zero;
                foreach (ClaimAuthorizationReportDetailData claimItmDtl in claimAuthorizationReportDataDetail)
                {
                    if (claimItmDtl.ItemCode.Trim().ToUpper() == "P")
                    {


                        ClaimAuthorizationReportDetailData relavantLabour = claimAuthorizationReportDataDetail.FirstOrDefault(a => a.PartNumber.EndsWith(claimItmDtl.PartNumber));
                        if (relavantLabour == null)
                        {
                            relavantLabour = new ClaimAuthorizationReportDetailData();
                        }
                        if (claimItmDtl.IsApproved)
                        {
                            TotalPartPrice += claim.AuthorizedAmount;
                            TotalLabourCost += relavantLabour.TotalPrice;
                        }
                        formattedReportDtl += reportHTMLDtl
                            .Replace("{d_sn}", counter.ToString())
                            .Replace("{d_PCost}", (Math.Round(claimItmDtl.TotalGrossPrice * claimItmDtl.ConversionRate * 100) / 100).ToString("N", CultureInfo.InvariantCulture))
                            //.Replace("{d_PDisc}", (Math.Round(claimItmDtl.DiscountAmount * claimItmDtl.ConversionRate * 100) / 100).ToString("N", CultureInfo.InvariantCulture))
                            //.Replace("{d_PGW}", (Math.Round(claimItmDtl.GoodWillAmount * claimItmDtl.ConversionRate * 100) / 100).ToString("N", CultureInfo.InvariantCulture))
                            //.Replace("{d_PTotal}", (Math.Round(claimItmDtl.TotalPrice * claimItmDtl.ConversionRate * 100) / 100).ToString("N", CultureInfo.InvariantCulture))
                            .Replace("{d_PartNo}", claimItmDtl.PartNumber)
                            .Replace("{d_Descr}", claimItmDtl.PartName)
                            .Replace("{d_PLttl}", (Math.Round(relavantLabour.TotalPrice * claimItmDtl.ConversionRate * 100) / 100).ToString("N", CultureInfo.InvariantCulture))
                            .Replace("{d_Status}", claimItmDtl.IsApproved ? "Approved" : "Rejected")
                            .Replace("{d_LCost}", (Math.Round(relavantLabour.TotalGrossPrice * claimItmDtl.ConversionRate * 100) / 100).ToString("N", CultureInfo.InvariantCulture))
                            .Replace("{d_LGW}", (Math.Round(relavantLabour.GoodWillAmount * claimItmDtl.ConversionRate * 100) / 100).ToString("N", CultureInfo.InvariantCulture))
                            .Replace("{d_LDisc}", (Math.Round(claimItmDtl.AuthorizedAmount * claimItmDtl.ConversionRate)).ToString("N", CultureInfo.InvariantCulture))
                            .Replace("{d_PLTotal}", (Math.Round(claimItmDtl.TotalPrice * claimItmDtl.ConversionRate)).ToString("N", CultureInfo.InvariantCulture))
                            .Replace("{d_Notes}", claimItmDtl.Remark);

                        counter++;
                    }
                    else if (claimItmDtl.ItemCode.Trim().ToUpper() == "S")
                    {
                        if (claimItmDtl.IsApproved)
                        {
                            TotalPartPrice += claimItmDtl.TotalPrice;
                        }

                        formattedReportDtl += reportHTMLDtl
                           .Replace("{d_sn}", counter.ToString())
                           .Replace("{d_PCost}", (Math.Round(claimItmDtl.TotalGrossPrice * claimItmDtl.ConversionRate * 100) / 100).ToString("N", CultureInfo.InvariantCulture))
                           //.Replace("{d_PDisc}", (Math.Round(claimItmDtl.DiscountAmount * claimItmDtl.ConversionRate * 100) / 100).ToString("N", CultureInfo.InvariantCulture))
                           //.Replace("{d_PGW}", (Math.Round(claimItmDtl.GoodWillAmount * claimItmDtl.ConversionRate * 100) / 100).ToString("N", CultureInfo.InvariantCulture))
                           //.Replace("{d_PTotal}", (Math.Round(claimItmDtl.TotalPrice * claimItmDtl.ConversionRate * 100) / 100).ToString("N", CultureInfo.InvariantCulture))
                           .Replace("{d_PartNo}", "Sundry")
                           .Replace("{d_Descr}", claimItmDtl.ItemCode)
                           .Replace("{d_PLttl}", "0.00")
                           .Replace("{d_Status}", claimItmDtl.IsApproved ? "Approved" : "Rejected")
                           .Replace("{d_LCost}", "0.00")
                           .Replace("{d_LGW}", "0.00")
                           .Replace("{d_LDisc}", "0.00")
                           .Replace("{d_PLTotal}", (Math.Round(claimItmDtl.TotalPrice * claimItmDtl.ConversionRate * 100) / 100).ToString("N", CultureInfo.InvariantCulture))
                           .Replace("{d_Notes}", claimItmDtl.Remark);
                        counter++;
                    }
                }

                string formattedReportBody = reportHTMLBody
                    .Replace("{d_Dealer}", claimAuthorizationReportData.DealerName)
                    .Replace("{d_ClaimNo}", claimAuthorizationReportData.ClaimNumber)
                    .Replace("{d_CountryRegion}", claimAuthorizationReportData.CountryCity)
                    .Replace("{d_CustomerName}", claimAuthorizationReportData.CustomerName)
                    .Replace("{d_BooklateNo}", claimAuthorizationReportData.BookletNumber)
                    .Replace("{d_PolicyNo}", claimAuthorizationReportData.PolicyNo)
                    .Replace("{d_NewUsed}", claimAuthorizationReportData.Status)
                    .Replace("{d_Vin}", InvoiceCode.Code)
                    .Replace("{d_PlateNo}", claimAuthorizationReportData.PlateNo)
                    .Replace("{d_Make}", claimAuthorizationReportData.MakeName)
                    .Replace("{d_Model}", claimAuthorizationReportData.ModelName)
                    .Replace("{d_FailureDate}", claimAuthorizationReportData.FailureDate.ToString("dd-MMM-yyyy"))
                    .Replace("{d_Mileage}", string.Format("{0:0}", claimAuthorizationReportData.ClaimMileageKm) + " km")

                    .Replace("{d_CustomerComplaint}", claimAuthorizationReportData.CustomerComplaint)
                    .Replace("{d_DefectAnalysis}", claimAuthorizationReportData.DealerComment)
                    .Replace("{d_ClaimSubmittedBy}", claimAuthorizationReportData.ClaimSubmittedUser)
                    .Replace("{d_ClaimSubmittedDate}", claimAuthorizationReportData.ClaimDate.ToString("dd-MMM-yyyy"))
                    .Replace("{d_ClaimProcessededDate}", claimAuthorizationReportData.ApprovedDate.ToString("dd-MMM-yyyy"))
                    .Replace("{d_ClaimProcessedBy}", claimAuthorizationReportData.ClaimApprovedUser)

                    .Replace("{d_Remarks}", claimAuthorizationReportData.EngineerComment)


                    .Replace("{d_ttlGrand}", (Math.Round((TotalLabourCost + TotalPartPrice) * claimAuthorizationReportData.ConversionRate * 100) / 100).ToString("N", CultureInfo.InvariantCulture))
                    .Replace("{d_ttlPayable}", (Math.Round((claimAuthorizationReportData.TotalPayable * claimAuthorizationReportData.ConversionRate))).ToString("N", CultureInfo.InvariantCulture))
                    .Replace("{d_ttlParts}", (Math.Round(TotalPartPrice * claimAuthorizationReportData.ConversionRate * 100) / 100).ToString("N", CultureInfo.InvariantCulture))
                    .Replace("{d_ttlLabour}", (Math.Round(TotalLabourCost * claimAuthorizationReportData.ConversionRate * 100) / 100).ToString("N", CultureInfo.InvariantCulture))

                    .Replace("{d_ClaimItem}", formattedReportDtl);

                response = formattedReportBody;
            }
            catch (Exception ex)
            { logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException); }

            return response;
        }
    }

    //internal static object GetAllReportParamInformationByReportId(Guid reportId)
    //{
    //    object response = null;
    //    try
    //    {
    //        ISession session = EntitySessionManager.GetSession();

    //        response = session.Query<ReportParameter>()
    //              .Where(a => a.ReportId == reportId)
    //              .Select(z => new
    //              {
    //                  z.Id,
    //                  z.ClienSideRegex,
    //                  z.IsDependOnPreviousParam,
    //                  z.IsFromField,
    //                  z.IsIncluedeInDateRange,
    //                  z.IsServerSideQuery,
    //                  z.OnSelectNextParam,
    //                  z.ParamCode,
    //                  z.ParamName,
    //                  z.ParamType,
    //                  z.PreviousParamSequance,
    //                  z.ReportId,
    //                  z.Sequance
    //              })
    //              .OrderBy(b => b.Sequance)
    //              .ToArray();
    //    }

    //    catch (Exception ex)
    //    { logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException); }

    //    return response;
    //}



    //internal static object GetAllDataForReportFromParentDropdown(Guid reportParamId, Guid reportParamParentValue, Guid parentParamId)
    //{
    //    object response = null;
    //    try
    //    {
    //        ISession session = EntitySessionManager.GetSession();

    //        ReportParameter reportParameter = session.Query<ReportParameter>()
    //            .FirstOrDefault(z => z.Id == reportParamId);

    //        ReportParameter ParentParameter = session.Query<ReportParameter>()
    //           .FirstOrDefault(z => z.Id == parentParamId);

    //        if (reportParameter != null && ParentParameter != null)
    //        {
    //            var selectQuery = ParentParameter.ServerSideQuery;
    //            var replacedQuery = selectQuery
    //                .Replace("{" + reportParameter.ParamCode + "}", reportParamParentValue.ToString())
    //                .Replace("\r", string.Empty).Replace("\n", string.Empty).Replace("\t", string.Empty);

    //            var dropdownData = session.CreateSQLQuery(replacedQuery).SetResultTransformer(Transformers.AliasToBean<ReportDropdownData>())
    //            .List<ReportDropdownData>();
    //            response = dropdownData;
    //        }
    //    }
    //    catch (Exception ex)
    //    { logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException); }

    //    return response;
    //}

    //internal static object ViewReport(Guid reportId, List<ReportParameterDataRequestDto> reportParameter,
    //    string tpaName, string dbConnectionString)
    //{
    //    object response = null;
    //    try
    //    {

    //        ISession session = EntitySessionManager.GetSession();
    //        Report report = session.Query<Report>().FirstOrDefault(a => a.Id == reportId);
    //        List<ReportParameter> reportParameters = session.Query<ReportParameter>()
    //            .Where(a => a.ReportId == reportId).ToList();
    //        if (report == null)
    //        {
    //            response = "Selected report is invalid";
    //            return response;
    //        }

    //        bool isExist = File.Exists(
    //            System.Web.HttpContext.Current.Server.MapPath(
    //                ConfigurationData.ReportsPath + "\\" + report.ReportCode +
    //                "\\" + tpaName.ToLower() + "\\" +
    //                      report.ReportCode + ".sql")
    //            );

    //        string ReportLocation = isExist ? tpaName.ToLower() : "Default";

    //        String Query = File.ReadAllText(
    //            System.Web.HttpContext.Current.Server.MapPath(
    //                ConfigurationData.ReportsPath + "\\" +
    //                report.ReportCode +
    //                "\\" + ReportLocation + "\\" +
    //                report.ReportCode + ".sql"));
    //        Query = Query.Replace("\r", string.Empty).Replace("\n", string.Empty).Replace("\t", string.Empty);

    //        foreach (ReportParameter reportParam in reportParameters)
    //        {
    //            Query = Query.Replace("{" + reportParam.ParamCode + "}", reportParameter.FirstOrDefault(z => z.id == reportParam.Id).value.ToString());
    //        }

    //        //putting in the db so report viewer can view and read it

    //        string reportKey = Guid.NewGuid().ToString();
    //        ReportDataQuery ReportDataQuery = new ReportDataQuery()
    //        {
    //            Id = Guid.NewGuid(),
    //            ReportKey = Guid.Parse(reportKey),
    //            ReportCode = report.ReportCode,
    //            ReportDbConnStr = dbConnectionString,
    //            ReportQuery = Query,
    //            ReportDirectory = ConfigurationData.ReportsPath + "\\" + report.ReportCode + "\\" + ReportLocation
    //        };
    //        using (ITransaction transaction = session.BeginTransaction())
    //        {
    //            session.Save(ReportDataQuery, ReportDataQuery.Id);
    //            transaction.Commit();
    //        }

    //        response = reportKey;
    //    }
    //    catch (Exception ex)
    //    {
    //        logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
    //    }
    //    return response;

    //}
    #endregion

}

//temporory
public class CacheReportData
{
    public bool isValid { get; set; }
    public string reportCode { get; set; }
    public string reportPath { get; set; }
    public DataTable reportData { get; set; }

}

public class ReportDropdownData
{
    public Guid value { get; set; }
    public string text { get; set; }

}

