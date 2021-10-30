
using Newtonsoft.Json.Linq;
using NLog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Web.Http;
using TAS.DataTransfer.Requests;
using TAS.DataTransfer.Responses;
using TAS.Services;
using TAS.Web.Common;

namespace TAS.Web.Controllers
{
    public class BordxController : ApiController
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        //private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        [HttpPost]
        public object AddBordx(JObject data)
        {
            try
            {

                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                data["Month"] = DateTime.ParseExact(data["Month"].ToString(), "MMMM", CultureInfo.InvariantCulture).Month;

                BordxInfo Bordx = data.ToObject<BordxInfo>();
                IBordxManagementService BordxManagementService = ServiceFactory.GetBordxManagementService();
                BordxRequestDto save = new BordxRequestDto()
                {
                    EntryDateTime = Bordx.EntryDateTime,
                    EntryUser = Bordx.EntryUser,
                    IsConformed = false,
                    IsProcessed = true,
                    Month = Bordx.Month,
                    Year = Bordx.Year,
                    Id = Bordx.Id
                };
                BordxRequestDto result = BordxManagementService.AddBordx(save, SecurityHelper.Context, AuditHelper.Context);

                foreach (var item in Bordx.Policy)
                {
                    BordxDetailsRequestDto itm = new BordxDetailsRequestDto()
                    {
                        Id = new Guid(),
                        BordxId = result.Id,
                        PolicyId = item
                    };
                    BordxDetailsRequestDto d = BordxManagementService.AddBordxDetails(itm,
                        SecurityHelper.Context,
                        AuditHelper.Context);
                }
                logger.Info("Bordx Added");
                if (result.BordxInsertion)
                {
                    return result;
                }
                else
                {
                    return "Add Bordx failed!";
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return "Add Bordx failed!";
            }

        }

        [HttpPost]
        public string UpdateBordx(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                data["Month"] = DateTime.ParseExact(data["Month"].ToString(), "MMMM", CultureInfo.InvariantCulture).Month;

                BordxInfo Bordx = data.ToObject<BordxInfo>();
                IBordxManagementService BordxManagementService = ServiceFactory.GetBordxManagementService();
                BordxRequestDto save = new BordxRequestDto()
                {
                    EntryDateTime = Bordx.EntryDateTime,
                    EntryUser = Bordx.EntryUser,
                    IsConformed = false,
                    IsProcessed = true,
                    Month = Bordx.Month,
                    Year = Bordx.Year,
                    Id = Bordx.Id
                };
                BordxRequestDto result = BordxManagementService.UpdateBordx(save, SecurityHelper.Context, AuditHelper.Context);

                foreach (var item in Bordx.Policy)
                {
                    BordxDetailsRequestDto itm = new BordxDetailsRequestDto()
                    {
                        Id = new Guid(),
                        BordxId = result.Id,
                        PolicyId = item
                    };
                    BordxDetailsRequestDto d = BordxManagementService.AddBordxDetails(itm,
                        SecurityHelper.Context,
                        AuditHelper.Context);
                }
                logger.Info("Bordx Added");
                if (result.BordxInsertion)
                {
                    return result.Id.ToString();
                }
                else
                {
                    return "Add Bordx failed!";
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return "Add Bordx failed!";
            }

        }

        [HttpPost]
        public object GetBordxById(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            IBordxManagementService BordxManagementService = ServiceFactory.GetBordxManagementService();
            BordxResponseDto Bordx = BordxManagementService.GetBordxById(Guid.Parse(data["Id"].ToString()),
                SecurityHelper.Context,
                AuditHelper.Context);
            BordxPolicyInfo ret = new BordxPolicyInfo();
            ret.Bordx = Bordx;

            List<BordxDetailsResponseDto> BordxDetailData = BordxManagementService.GetBordxDetails(
            SecurityHelper.Context,
            AuditHelper.Context).BordxDetails.FindAll(b => b.BordxId == Bordx.Id);

            IPolicyManagementService PolicyManagementService = ServiceFactory.GetPolicyManagementService();
            ret.Policies = new List<PolicyResponseDto>();
            foreach (var item in BordxDetailData)
            {
                ret.Policies.Add(PolicyManagementService.GetPolicyById(item.PolicyId, SecurityHelper.Context, AuditHelper.Context));
            }


            return ret;
        }

        [HttpPost]
        public object getAllBordxAllowedYearsMonths(JObject data)
        {
            //Guid CountryId = Guid.Parse(data["countryId"].ToString());
            Guid InsurerId = Guid.Parse(data["insurerId"].ToString());
            Guid ReinsurerId = Guid.Parse(data["reinsurerId"].ToString());
            Guid CommodityTypeId = Guid.Parse(data["commodityTypeId"].ToString());


            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            IBordxManagementService BordxManagementService = ServiceFactory.GetBordxManagementService();
            return BordxManagementService.getAllBordxAllowedYearsMonths(InsurerId, ReinsurerId, CommodityTypeId, SecurityHelper.Context, AuditHelper.Context);
        }

        [HttpPost]
        public object getBordxNumbersYearsAndMonth(JObject data)
        {
            //Guid CountryId = Guid.Parse(data["countryId"].ToString());
            string bordxYear = data["bordxYear"].ToString();
            string bordxMonth = data["bordxMonth"].ToString();

            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            IBordxManagementService BordxManagementService = ServiceFactory.GetBordxManagementService();
            return BordxManagementService.getBordxNumbersYearsAndMonth(bordxYear, bordxMonth, SecurityHelper.Context, AuditHelper.Context);
        }

        [HttpPost]
        public object GetAllBordxs()
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            IBordxManagementService BordxManagementService = ServiceFactory.GetBordxManagementService();
            List<BordxResponseDto> BordxData = BordxManagementService.GetBordxs(SecurityHelper.Context, AuditHelper.Context).Bordxs.FindAll(b => b.IsConformed);
            BordxsDetailsResponseDto BordxDetailData = BordxManagementService.GetBordxDetails(SecurityHelper.Context, AuditHelper.Context);
            List<BordxInfo> ret = new List<BordxInfo>();
            foreach (var item in BordxData)
            {
                List<Guid> p = new List<Guid>();
                Guid x = new Guid();
                List<BordxDetailsResponseDto> detail = BordxDetailData.BordxDetails.FindAll(d => d.BordxId == item.Id);
                if (detail.Count > 0)
                {
                    x = BordxDetailData.BordxDetails.FindAll(d => d.BordxId == item.Id).First().PolicyId;
                    p.Add(x);
                }
                BordxInfo i = new BordxInfo()
                {
                    EntryDateTime = item.EntryDateTime,
                    EntryUser = item.EntryUser,
                    Id = item.Id,
                    IsConformed = item.IsConformed,
                    IsProcessed = item.IsProcessed,
                    Month = item.Month,
                    Policy = p,
                    Year = item.Year
                };
                ret.Add(i);
            }
            return ret;
        }

        [HttpPost]
        public object GetAllNotConformedBordxs()
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            IBordxManagementService BordxManagementService = ServiceFactory.GetBordxManagementService();
            List<BordxResponseDto> BordxData = BordxManagementService.GetBordxs(SecurityHelper.Context, AuditHelper.Context).Bordxs.FindAll(b => b.IsConformed == false);
            BordxsDetailsResponseDto BordxDetailData = BordxManagementService.GetBordxDetails(SecurityHelper.Context, AuditHelper.Context);
            List<BordxInfo> ret = new List<BordxInfo>();
            foreach (var item in BordxData)
            {
                List<Guid> p = new List<Guid>();
                Guid x = new Guid();
                List<BordxDetailsResponseDto> detail = BordxDetailData.BordxDetails.FindAll(d => d.BordxId == item.Id);
                if (detail.Count > 0)
                {
                    x = BordxDetailData.BordxDetails.FindAll(d => d.BordxId == item.Id).First().PolicyId;
                    p.Add(x);
                }
                BordxInfo i = new BordxInfo()
                {
                    EntryDateTime = item.EntryDateTime,
                    EntryUser = item.EntryUser,
                    Id = item.Id,
                    IsConformed = item.IsConformed,
                    IsProcessed = item.IsProcessed,
                    Month = item.Month,
                    Policy = p,
                    Year = item.Year
                };
                ret.Add(i);
            }
            return ret;
        }

        [HttpPost]
        public object ViewBordx(JObject data)
        {
            //SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            //#region Management Service
            //ICountryManagementService CountryManagementService = ServiceFactory.GetCountryManagementService();
            //ITPAManagementService TPAManagementService = ServiceFactory.GetTPAManagementService();
            //ITPABranchManagementService ITPABranchManagementService = ServiceFactory.GetTPABranchManagementService();
            //IBordxManagementService BordxManagementService = ServiceFactory.GetBordxManagementService();
            //IPolicyManagementService PolicyManagementService = ServiceFactory.GetPolicyManagementService();
            //ICustomerManagementService CustomerManagementService = ServiceFactory.GetCustomerManagementService();
            //IContractManagementService ContractManagementService = ServiceFactory.GetContractManagementService();
            //INRPCommissionTypesManagementService NRPCommissionTypesManagementService = ServiceFactory.GetNRPCommissionTypesManagementService();
            //IVehicleDetailsManagementService VehicleDetailsManagementService = ServiceFactory.GetVehicleDetailsManagementService();
            //IBrownAndWhiteDetailsManagementService BrownAndWhiteDetailsManagementService = ServiceFactory.GetBrownAndWhiteDetailsManagementService();
            //ICommodityCategoryManagementService CommodityCategoryManagementService = ServiceFactory.GetCommodityCategoryManagementService();
            //ICityManagementService CityManagementService = ServiceFactory.GetCityManagementService();
            //IItemStatusManagementService ItemStatusManagementService = ServiceFactory.GetItemStatusManagementService();
            //IManufacturerManagementService ManufacturerManagementService = ServiceFactory.GetManufacturerManagementService();
            //IManufacturerWarrantyManagementService ManufacturerWarrantyManagementService = ServiceFactory.GetManufacturerWarrantyManagementService();
            //IInsurerManagementService InsurerManagementService = ServiceFactory.GetInsurerManagementService();
            //IReinsurerManagementService ReinsurerManagementService = ServiceFactory.GetReinsurerManagementService();
            //IReinsurerContractManagementService ReinsurerContractManagementService = ServiceFactory.GetReinsurerContractManagementService();
            //IModelManagementService ModelManagementService = ServiceFactory.GetModelManagementService();
            //IWarrantyTypeManagementService WarrantyTypeManagementService = ServiceFactory.GetWarrantyTypeManagementService();
            //ICylinderCountManagementService CylinderCountManagementService = ServiceFactory.GetCylinderCountManagementService();
            //IEngineCapacityManagementService EngineCapacityManagementService = ServiceFactory.GetEngineCapacityManagementService();
            //IExtensionTypeManagementService ExtensionTypeManagementService = ServiceFactory.GetExtensionTypeManagementService();
            //IMakeManagementService MakeManagementService = ServiceFactory.GetMakeManagementService();
            //IUserManagementService UserManagementService = ServiceFactory.GetUserManagementService();
            //ITaxManagementService TaxManagementService = ServiceFactory.GetTaxManagementService();
            //IDealerManagementService DealerManagementService = ServiceFactory.GetDealerManagementService();
            //IDealerLocationManagementService DealerLocationManagementService = ServiceFactory.GetDealerLocationManagementService();
            //#endregion
            //TPAResponseDto TPAData = TPAManagementService.GetAllTPAs(SecurityHelper.Context, AuditHelper.Context).TPAs.First();
            //TPABranchResponseDto TPABranchesData = ITPABranchManagementService.GetTPABranchesByTPAId(TPAData.Id, SecurityHelper.Context, AuditHelper.Context).TPABranches.First();
            //BordxResponseDto Bordx = BordxManagementService.GetBordxById(Guid.Parse(data["Id"].ToString()), SecurityHelper.Context, AuditHelper.Context);
            //List<BordxDetailsResponseDto> BordxDetailData = BordxManagementService.GetBordxDetails(SecurityHelper.Context, AuditHelper.Context).BordxDetails.FindAll(b => b.BordxId == Bordx.Id);
            //List<BordxViewResponseDto> ViewList = new List<BordxViewResponseDto>();
            //int index = 0;
            //Dictionary<string, decimal> com = new Dictionary<string, decimal>();
            //Dictionary<string, decimal> tax = new Dictionary<string, decimal>();
            //NRPCommissionTypessResponseDto commissionType = NRPCommissionTypesManagementService.GetNRPCommissionTypess(SecurityHelper.Context, AuditHelper.Context);
            //foreach (var ct in commissionType.NRPCommissionTypess)
            //{
            //    com.Add(ct.Name, 0);
            //}
            //TaxesResponseDto taxType = TaxManagementService.GetAllTaxes(SecurityHelper.Context, AuditHelper.Context);
            //foreach (var tt in taxType.Taxes)
            //{
            //    tax.Add(tt.TaxCode, 0);
            //}
            //BordxViewMapResponseDto Map = UserMappings(Guid.Parse(data["UserId"].ToString()));
            //List<BordxReportColumnsResponseDto> cols = BordxManagementService.GetBordxReportColumnses(SecurityHelper.Context, AuditHelper.Context).BordxReportColumnses.FindAll(b => b.IsActive);
            //foreach (var item in BordxDetailData)
            //{
            //    PolicyResponseDto p = PolicyManagementService.GetPolicyById(item.PolicyId, SecurityHelper.Context, AuditHelper.Context);
            //    CustomerResponseDto customer = CustomerManagementService.GetCustomerById(p.CustomerId, SecurityHelper.Context, AuditHelper.Context);
            //    VehicleDetailsResponseDto vehicle = new VehicleDetailsResponseDto();
            //    BrownAndWhiteDetailsResponseDto bNw = new BrownAndWhiteDetailsResponseDto();
            //    if (p.Type == "Vehicle")
            //    {
            //        vehicle = VehicleDetailsManagementService.GetVehicleDetailsById(p.ItemId, SecurityHelper.Context, AuditHelper.Context);
            //    }
            //    else
            //    {
            //        bNw = BrownAndWhiteDetailsManagementService.GetBrownAndWhiteDetailsById(p.ItemId, SecurityHelper.Context, AuditHelper.Context);
            //    }
            //    ModelResponseDto Model = ModelManagementService.GetModelById(vehicle.ModelId, SecurityHelper.Context, AuditHelper.Context);
            //    MakeResponseDto Make = MakeManagementService.GetMakeById(vehicle.MakeId, SecurityHelper.Context, AuditHelper.Context);
            //    UserResponseDto User = UserManagementService.GetUserById(p.SalesPersonId.ToString(), SecurityHelper.Context, AuditHelper.Context);
            //    ManufacturerWarrantyResponseDto ManWarranty = ManufacturerWarrantyManagementService.GetManufacturerWarranties(SecurityHelper.Context, AuditHelper.Context).ManufacturerWarranties.FindAll(m => m.CountryId == customer.CountryId && m.MakeId == Make.Id && m.ModelId == Model.Id).OrderBy(m => m.ApplicableFrom).Last();
            //    ContractResponseDto contract = ContractManagementService.GetContractById(p.ContractId, SecurityHelper.Context, AuditHelper.Context);
            //    ReinsurerContractResponseDto reinsurer = ReinsurerContractManagementService.GetReinsurerContractById(contract.ReinsurerId, SecurityHelper.Context, AuditHelper.Context);
            //    if (reinsurer == null)
            //        reinsurer = new ReinsurerContractResponseDto();
            //    List<NRPCommissionContractMappingResponseDto> commission = NRPCommissionTypesManagementService.GetNRPCommissionContractMappings(SecurityHelper.Context, AuditHelper.Context).NRPCommissionContractMappings.FindAll(c => c.ContractId == contract.Id);
            //    List<ContractTaxesResponseDto> taxContract = ContractManagementService.GetContractTaxess(SecurityHelper.Context, AuditHelper.Context).ContractTaxess.FindAll(c => c.ContractId == contract.Id);
            //    decimal commissionSum = 0;
            //    decimal TaxSum = 0;
            //    decimal salesCommission = 0;
            //    foreach (var c in commission)
            //    {
            //        string name = commissionType.NRPCommissionTypess.Find(n => n.Id == c.NRPCommissionId).Name;
            //        com[name] = c.Commission;
            //        commissionSum = commissionSum + c.Commission;
            //        if (name == "SC")
            //            salesCommission = c.Commission;
            //    }
            //    foreach (var t in taxContract)
            //    {
            //        CountryTaxesResponseDto taxCountry = TaxManagementService.GetCountryTaxesById(t.CountryTaxesId, SecurityHelper.Context, AuditHelper.Context);
            //        string name = taxType.Taxes.Find(n => n.Id == taxCountry.TaxTypeId).TaxCode;
            //        tax[name] = taxCountry.TaxValue;
            //        TaxSum = TaxSum + taxCountry.TaxValue;
            //    }

            //    BordxViewResponseDto v = new BordxViewResponseDto()
            //    {
            //        //SNo = index++,
            //        //Address = Map.Address ? customer.Address1 + " " + customer.Address2 + " " + customer.Address3 + " " + customer.Address4 : "Nil",
            //        //AdministrationFee = Map.AdministrationFee ? CurrencyConversion(com["Admin Fee"], "USD").ToString() : "Nil",
            //        //Brokerage = Map.Brokerage ? CurrencyConversion(com["Broker Fee"], "USD").ToString() : "Nil",
            //        //Category = Map.Category ? p.ItemId.ToString() == "00000000-0000-0000-0000-000000000000" ? "" : p.Type == "Vehicle" ? CommodityCategoryManagementService.GetCommodityCategoryById(vehicle.CategoryId, p.CommodityTypeId, SecurityHelper.Context, AuditHelper.Context).CommodityCategoryDescription : CommodityCategoryManagementService.GetCommodityCategoryById(bNw.CategoryId, p.CommodityTypeId, SecurityHelper.Context, AuditHelper.Context).CommodityCategoryDescription : "Nil",
            //        //City = Map.City ? customer.CityId.ToString() == "00000000-0000-0000-0000-000000000000" ? "" : CityManagementService.GetCityById(customer.CityId, SecurityHelper.Context, AuditHelper.Context).CityName : "Nil",
            //        //Comment = Map.Comment ? p.Comment : "Nil",
            //        //ContractYear = Map.ContractYear ? reinsurer.ReinsurerId.ToString() == "00000000-0000-0000-0000-000000000000" ? "" : reinsurer.UWYear.ToString() : "Nil",
            //        //CoverPeriodMonths = Map.CoverPeriodMonths ? p.ExtensionTypeId.ToString() == "00000000-0000-0000-0000-000000000000" ? "" : ExtensionTypeManagementService.GetExtensionTypeById(p.ExtensionTypeId, SecurityHelper.Context, AuditHelper.Context).Month.ToString() : "Nil",
            //        ////CoverType =Map.CoverType? p.CoverTypeId.ToString() == "00000000-0000-0000-0000-000000000000" ? "" : WarrantyTypeManagementService.GetWarrantyTypeById(p.CoverTypeId, SecurityHelper.Context, AuditHelper.Context).WarrantyTypeDescription:"Nil",
            //        //CylinderCount = Map.CylinderCount ? vehicle.CylinderCountId.ToString() == "00000000-0000-0000-0000-000000000000" ? "" : CylinderCountManagementService.GetCylinderCountById(vehicle.CylinderCountId, SecurityHelper.Context, AuditHelper.Context).Count.ToString() : "Nil",
            //        //DatePurchaseRegistration = Map.DatePurchaseRegistration ? vehicle.ItemPurchasedDate.ToShortDateString() : "Nil",
            //        //DocumentFee = Map.DocumentFee ? CurrencyConversion(tax["DF"], "USD").ToString() : "Nil",
            //        //EngineCapacity = Map.EngineCapacity ? vehicle.EngineCapacityId.ToString() == "00000000-0000-0000-0000-000000000000" ? "" : EngineCapacityManagementService.GetEngineCapacityById(vehicle.EngineCapacityId, SecurityHelper.Context, AuditHelper.Context).EngineCapacityNumber.ToString() : "Nil",
            //        //ExtensonPeriodHoursMileage = Map.ExtensonPeriodHoursMileage ? p.ExtensionTypeId.ToString() == "00000000-0000-0000-0000-000000000000" ? "" : ExtensionTypeManagementService.GetExtensionTypeById(p.ExtensionTypeId, SecurityHelper.Context, AuditHelper.Context).Km.ToString() : "Nil",
            //        //FirstName = Map.FirstName ? customer.FirstName : "Nil",
            //        //GrossPremium = Map.GrossPremium ? CurrencyConversion(p.Premium, "USD").ToString() : "Nil",
            //        //GrossPremiumLessCommission = Map.GrossPremiumLessCommission ? CurrencyConversion((p.Premium - commissionSum), "USD").ToString() : "Nil",
            //        //GrossPremiumLessSalesCommission = Map.GrossPremiumLessSalesCommission ? CurrencyConversion((p.Premium - salesCommission), "USD").ToString() : "Nil",
            //        //Insurer = Map.Insurer ? contract.InsurerId.ToString() == "00000000-0000-0000-0000-000000000000" ? "" : InsurerManagementService.GetInsurerById(contract.InsurerId, SecurityHelper.Context, AuditHelper.Context).InsurerFullName : "Nil",
            //        //IPT = Map.IPT ? CurrencyConversion(tax["IPT"], "USD").ToString() : "Nil",
            //        //ItemStatus = Map.ItemStatus ? vehicle.ItemStatusId.ToString() == "00000000-0000-0000-0000-000000000000" ? "" : ItemStatusManagementService.GetItemStatusById(vehicle.ItemStatusId, SecurityHelper.Context, AuditHelper.Context).Status : "Nil",
            //        //LimitationsHoursMileagePeriod = Map.LimitationsHoursMileagePeriod ? ManWarranty.WarrantyKm.ToString() : "Nil",
            //        //InsurerFee = Map.InsurerFee ? CurrencyConversion(tax["IF"], "USD").ToString() : "Nil",
            //        //LocalTax = Map.LocalTax ? CurrencyConversion(tax["LT"], "USD").ToString() : "Nil",
            //        //MakeNModel = Map.MakeNModel ? Make.MakeName + " " + Model.ModelName : "Nil",
            //        //ManufacturerCoverInMonth = Map.ManufacturerCoverInMonth ? ManWarranty.WarrantyMonths.ToString() : "Nil",
            //        //ManufacturersExpirey = Map.ManufacturersExpirey ? p.PolicySoldDate == p.PolicyStartDate ? "" : p.PolicyStartDate.ToShortDateString() : "Nil",
            //        //Mileage = Map.Mileage ? p.HrsUsedAtPolicySale : "Nil",
            //        //ModelYear = Map.ModelYear ? vehicle.ModelYear : "Nil",
            //        //PointOfSale = Map.PointOfSale ? p.DealerLocationId.ToString() == "00000000-0000-0000-0000-000000000000" ? "" : DealerLocationManagementService.GetDealerLocationById(p.DealerLocationId, SecurityHelper.Context, AuditHelper.Context).Location : "Nil",
            //        //Program = Map.Program ? contract.DealName : "Nil",
            //        //Registration = Map.Registration ? vehicle.PlateNo : "Nil",
            //        //Reinsurer = Map.Reinsurer ? contract.ReinsurerId.ToString() == "00000000-0000-0000-0000-000000000000" ? "" : ReinsurerManagementService.GetReinsurerById(contract.ReinsurerId, SecurityHelper.Context, AuditHelper.Context).ReinsurerName : "Nil",
            //        //ReportingMonth = Map.ReportingMonth ? CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Bordx.Month) : "Nil",
            //        //RIPremiumType = Map.RIPremiumType ? contract.DealType : "Nil",
            //        //RiskStartDate = Map.RiskStartDate ? p.PolicyStartDate.ToShortDateString() : "Nil",
            //        //RiskTerminationDate = Map.RiskTerminationDate ? p.PolicyEndDate.ToShortDateString() : "Nil",
            //        //SalesmanCommission = Map.SalesmanCommission ? CurrencyConversion(com["Sales Commission"], "USD").ToString() : "Nil",
            //        //SalesTax = Map.SalesTax ? CurrencyConversion(tax["ST"], "USD").ToString() : "Nil",
            //        //Seller = Map.Seller ? p.DealerId.ToString() == "00000000-0000-0000-0000-000000000000" ? "" : DealerManagementService.GetDealerById(p.Id, SecurityHelper.Context, AuditHelper.Context).DealerName : "Nil",
            //        //SumInsuredInLocalCurrency = Map.SumInsuredInLocalCurrency ? CurrencyConversion(vehicle.DealerPrice, "USD").ToString() : "Nil",
            //        //Surname = Map.Surname ? customer.LastName : "Nil",
            //        //SystemGeneratedIdentification = Map.SystemGeneratedIdentification ? p.Id.ToString() : "Nil",
            //        //Salesman = Map.Salesman ? User.FirstName + " " + User.LastName : "Nil",
            //        //Tel = Map.Tel ? customer.MobileNo : "Nil",
            //        //Total = Map.Total ? CurrencyConversion(TaxSum, "USD").ToString() : "Nil",
            //        //VAT = Map.VAT ? CurrencyConversion(tax["VAT"], "USD").ToString() : "Nil",
            //        //VehicleManufacturer = Map.VehicleManufacturer ? Make.ManufacturerId.ToString() == "00000000-0000-0000-0000-000000000000" ? "" : ManufacturerManagementService.GetManufacturerById(Make.ManufacturerId, SecurityHelper.Context, AuditHelper.Context).ManufacturerName : "Nil",
            //        //VINNo = Map.VINNo ? vehicle.VINNo : "Nil",
            //        //InsurencePolicyNo = Map.InsurencePolicyNo ? p.PolicyNo : "Nil",
            //        //ManufactureCommission = Map.ManufactureCommission ? "" : "Nil",
            //        //ClientBrokerage = Map.ClientBrokerage ? "" : "Nil",
            //        //CountryOrignCode = Map.CountryOrignCode ? "" : "Nil",
            //        //NetAbsoluteRiskPremium = Map.NetAbsoluteRiskPremium ? "0.0" : "Nil",
            //        //NRPIncludingBrokerage = Map.NRPIncludingBrokerage ? "0.0" : "Nil",
            //        //NetAbsoluteRiskPremiumUSD = Map.NetAbsoluteRiskPremiumUSD ? "0.0" : "Nil",
            //        //NRPIncludingBrokerageUSD = Map.NRPIncludingBrokerageUSD ? "0.0" : "Nil",
            //        //BrokerageUSD = Map.BrokerageUSD ? "0.0" : "Nil",
            //        //Retention = Map.Retention ? "0.0" : "Nil",
            //        //Status = Map.Status ? "" : "Nil",
            //        // AccidentandHealthMedicalExpensesCountryofTreatment = Map.AccidentandHealthMedicalExpensesCountryofTreatment ? "" : "Nil",
            //        // AccidentandHealthMedicalExpensesDateofTreatment = Map.AccidentandHealthMedicalExpensesDateofTreatment ? "" : "Nil",
            //        // AccidentandHealthMedicalExpensesPatientName = Map.AccidentandHealthMedicalExpensesPatientName ? "" : "Nil",
            //        // AccidentandHealthMedicalExpensesPlan = Map.AccidentandHealthMedicalExpensesPlan ? "" : "Nil",
            //        // AccidentandHealthMedicalExpensesTreatmentType = Map.AccidentandHealthMedicalExpensesTreatmentType ? "" : "Nil",
            //        // ClaimantAddressClaimantAddress = Map.ClaimantAddressClaimantAddress ? "" : "Nil",
            //        // ClaimantAddressClaimantCountry = Map.ClaimantAddressClaimantCountry ? "" : "Nil",
            //        // ClaimantAddressClaimantPostcode = Map.ClaimantAddressClaimantPostcode ? "" : "Nil",
            //        //// ClaimDetailsCatastropheName = Map.ClaimDetailsCatastropheName ? "" : "Nil",
            //        // ClaimDetailsCauseofLossCode = Map.ClaimDetailsCauseofLossCode ? "" : "Nil",
            //        //// ClaimDetailsClaimantName = Map.ClaimDetailsClaimantName ? "" : "Nil",
            //        // ClaimDetailsClaimStatus = Map.ClaimDetailsClaimStatus ? "" : "Nil",
            //        // ClaimDetailsDateClaimMade = Map.ClaimDetailsDateClaimMade ? "" : "Nil",
            //        // //ClaimDetailsDateClosed = Map.ClaimDetailsDateClosed ? "" : "Nil",
            //        // //ClaimDetailsDateFirstAdvisedNotificationDate = Map.ClaimDetailsDateFirstAdvisedNotificationDate ? "" : "Nil",
            //        // ClaimDetailsDateofLossFrom = Map.ClaimDetailsDateofLossFrom ? "" : "Nil",
            //        // ClaimDetailsDateofLossto = Map.ClaimDetailsDateofLossto ? "" : "Nil",
            //        //// ClaimDetailsDenial = Map.ClaimDetailsDenial ? "" : "Nil",
            //        //// ClaimDetailsLloydsCatCode = Map.ClaimDetailsLloydsCatCode ? "" : "Nil",
            //        // ClaimDetailsLossDescription = Map.ClaimDetailsLossDescription ? "" : "Nil",
            //        // ClaimDetailsRefertoUnderwriters = Map.ClaimDetailsRefertoUnderwriters ? "" : "Nil",
            //        // ClaimNotesAmountClaimed = Map.ClaimNotesAmountClaimed ? "" : "Nil",
            //        // ClaimNotesClaimnotpaidaswithinexcess = Map.ClaimNotesClaimnotpaidaswithinexcess ? "" : "Nil",
            //        // ClaimNotesComplaintReason = Map.ClaimNotesComplaintReason ? "" : "Nil",
            //        // ClaimNotesDateclaimclosed = Map.ClaimNotesDateclaimclosed ? "" : "Nil",
            //        // ClaimNotesDateClaimDenied = Map.ClaimNotesDateClaimDenied ? "" : "Nil",
            //        // ClaimNotesDateclaimwithdrawn = Map.ClaimNotesDateclaimwithdrawn ? "" : "Nil",
            //        // ClaimNotesDateofComplaint = Map.ClaimNotesDateofComplaint ? "" : "Nil",
            //        // ClaimNotesExgratiapayment = Map.ClaimNotesExgratiapayment ? "" : "Nil",
            //        // ClaimNotesInLitigation = Map.ClaimNotesInLitigation ? "" : "Nil",
            //        // ClaimNotesReasonforDenial = Map.ClaimNotesReasonforDenial ? "" : "Nil",
            //        // ClaimStatusChangesDateClaimAmountAgreed = Map.ClaimStatusChangesDateClaimAmountAgreed ? "" : "Nil",
            //        // ClaimStatusChangesDateClaimOpened = Map.ClaimStatusChangesDateClaimOpened ? "" : "Nil",
            //        // ClaimStatusChangesDateClaimsPaid = Map.ClaimStatusChangesDateClaimsPaid ? "" : "Nil",
            //        // ClaimStatusChangesDateCoverageConfirmed = Map.ClaimStatusChangesDateCoverageConfirmed ? "" : "Nil",
            //        // ClaimStatusChangesDateFeesPaid = Map.ClaimStatusChangesDateFeesPaid ? "" : "Nil",
            //        // ClaimStatusChangesDateofSubrogation = Map.ClaimStatusChangesDateofSubrogation ? "" : "Nil",
            //        // ClaimStatusChangesDateReopened = Map.ClaimStatusChangesDateReopened ? "" : "Nil",
            //        // ClassofBusinessSpecificNameorRegNoofAircraftVehicleVesseletc = Map.ClassofBusinessSpecificNameorRegNoofAircraftVehicleVesseletc ? "" : "Nil",
            //        // ClassofBusinessSpecificPercentCededReinsurance = Map.ClassofBusinessSpecificPercentCededReinsurance ? "" : "Nil",
            //        // ContractDetailsAgreementNo = Map.ContractDetailsAgreementNo ? reinsurer.ContractNo : "Nil",
            //        // ContractDetailsClassofBusiness = Map.ContractDetailsClassofBusiness ? "" : "Nil",
            //        // ContractDetailsContractExpiry = Map.ContractDetailsContractExpiry ? contract.EndDate.ToShortDateString() : "Nil",
            //        // ContractDetailsContractInception = Map.ContractDetailsContractInception ? contract.StartDate.ToShortDateString() : "Nil",
            //        // ContractDetailsCoverholderName = Map.ContractDetailsCoverholderName ? customer.FirstName + " " + customer.LastName : "Nil",
            //        // ContractDetailsCoverholderPIN = Map.ContractDetailsCoverholderPIN ? "" : "Nil",
            //        // ContractDetailsLloydsRiskCode = Map.ContractDetailsLloydsRiskCode ? "" : "Nil",
            //        // ContractDetailsOriginalCurrency = Map.ContractDetailsOriginalCurrency ? "" : "Nil",//
            //        // ContractDetailsRateofExchange = Map.ContractDetailsRateofExchange ? "" : "Nil",//
            //        // ContractDetailsReportingPeriodEndDate = Map.ContractDetailsReportingPeriodEndDate ? "" : "Nil",//
            //        // ContractDetailsReportingPeriodStartDate = Map.ContractDetailsReportingPeriodStartDate ? "" : "Nil",
            //        // ContractDetailsSectionNo = Map.ContractDetailsSectionNo ? "" : "Nil",
            //        // ContractDetailsSettlementCurrency = Map.ContractDetailsSettlementCurrency ? "" : "Nil",//
            //        // ContractDetailsTPAName = Map.ContractDetailsTPAName ? TPAData.Name : "Nil",
            //        // ContractDetailsTypeofInsuranceDirectorTypeofRI = Map.ContractDetailsTypeofInsuranceDirectorTypeofRI ? "" : "Nil",
            //        // ContractDetailsUniqueMarketReferenceUMR = Map.ContractDetailsUniqueMarketReferenceUMR ? reinsurer.ContractNo.ToString() : "Nil",
            //        // ExpertsLawyerAdjusterAttorneyetcAddress = Map.ExpertsLawyerAdjusterAttorneyetcAddress ? "" : "Nil",
            //        // ExpertsLawyerAdjusterAttorneyetcCountry = Map.ExpertsLawyerAdjusterAttorneyetcCountry ? "" : "Nil",
            //        // ExpertsLawyerAdjusterAttorneyetcFirmCompanyName = Map.ExpertsLawyerAdjusterAttorneyetcFirmCompanyName ? "" : "Nil",
            //        // ExpertsLawyerAdjusterAttorneyetcNotes = Map.ExpertsLawyerAdjusterAttorneyetcNotes ? "" : "Nil",
            //        // ExpertsLawyerAdjusterAttorneyetcPostcodeZipCodeorsimilar = Map.ExpertsLawyerAdjusterAttorneyetcPostcodeZipCodeorsimilar ? "" : "Nil",
            //        // ExpertsLawyerAdjusterAttorneyetcReferenceNoetc = Map.ExpertsLawyerAdjusterAttorneyetcReferenceNoetc ? "" : "Nil",
            //        //// ExpertsLawyerAdjusterAttorneyetcRole = Map.ExpertsLawyerAdjusterAttorneyetcRole ? "" : "Nil",
            //        // ExpertsLawyerAdjusterAttorneyetcStateProvinceTerritoryCantonetc = Map.ExpertsLawyerAdjusterAttorneyetcStateProvinceTerritoryCantonetc ? "" : "Nil",
            //        // InsuredDetailsAddress = Map.InsuredDetailsAddress ? "" : "Nil",
            //        // InsuredDetailsCountry = Map.InsuredDetailsCountry ? customer.Country : "Nil",
            //        // InsuredDetailsFullNameorCompanyName = Map.InsuredDetailsFullNameorCompanyName ? customer.FirstName + " " + customer.LastName : "Nil",
            //        // InsuredDetailsPostcodeZipCodeorsimilar = Map.InsuredDetailsPostcodeZipCodeorsimilar ? "" : "Nil",
            //        // InsuredDetailsStateProvinceTerritoryCantonetc = Map.InsuredDetailsStateProvinceTerritoryCantonetc ? "" : "Nil",
            //        // LocationofLossAddress = Map.LocationofLossAddress ? "" : "Nil",
            //        // LocationofLossCountry = Map.LocationofLossCountry ? "" : "Nil",
            //        //// LocationofLossPostcodeZipCodeorsimilar = Map.LocationofLossPostcodeZipCodeorsimilar ? "" : "Nil",
            //        //// LocationofLossStateProvinceTerritoryCantonetc = Map.LocationofLossStateProvinceTerritoryCantonetc ? "" : "Nil",
            //        // LocationofRiskAddress = Map.LocationofRiskAddress ? TPABranchesData.Address : "Nil",
            //        // LocationofRiskCountry = Map.LocationofRiskCountry ? CountryManagementService.GetCountryById(TPABranchesData.ContryId, SecurityHelper.Context, AuditHelper.Context).CountryName : "Nil",
            //        // LocationofRiskLocationID = Map.LocationofRiskLocationID ? "" : "Nil",
            //        // LocationofRiskPostcodeZipCodeorsimilar = Map.LocationofRiskPostcodeZipCodeorsimilar ? "" : "Nil",
            //        // LocationofRiskStateProvinceTerritoryCantonetc = Map.LocationofRiskStateProvinceTerritoryCantonetc ? TPABranchesData.State : "Nil",
            //        // ReferencesCertificateReference = Map.ReferencesCertificateReference ? p.PolicyNo : "Nil",
            //        // ReferencesClaimReference = Map.ReferencesClaimReference ? "" : "Nil",
            //        // RefsPolicyorGroupRef = Map.RefsPolicyorGroupRef ? "" : "Nil",
            //        // RiskDetailsDeductibleAmount = Map.RiskDetailsDeductibleAmount ? "" : "Nil",
            //        // RiskDetailsDeductibleBasis = Map.RiskDetailsDeductibleBasis ? "" : "Nil",
            //        // RiskDetailsPeriodofCoverNarrative = Map.RiskDetailsPeriodofCoverNarrative ? "" : "Nil",
            //        // RiskDetailsRiskExpiryDate = Map.RiskDetailsRiskExpiryDate ? p.PolicyEndDate.ToShortDateString() : "Nil",
            //        // RiskDetailsRiskInceptionDate = Map.RiskDetailsRiskInceptionDate ? p.PolicyStartDate.ToShortDateString() : "Nil",
            //        // RiskDetailsSumsInsuredAmount = Map.RiskDetailsSumsInsuredAmount ? "" : "Nil",
            //        // //RowNo = Map.RowNo ? index++.ToString() : "Nil",
            //        // TransactionDetailsChangethismonthFees = Map.TransactionDetailsChangethismonthFees ? "" : "Nil",
            //        // TransactionDetailsChangethismonthIndemnity = Map.TransactionDetailsChangethismonthIndemnity ? "" : "Nil",
            //        // TransactionDetailsPaidthismonthAdjustersFees = Map.TransactionDetailsPaidthismonthAdjustersFees ? "" : "Nil",
            //        // TransactionDetailsPaidthismonthAttorneyCoverageFees = Map.TransactionDetailsPaidthismonthAttorneyCoverageFees ? "" : "Nil",
            //        // TransactionDetailsPaidthismonthDefenceFees = Map.TransactionDetailsPaidthismonthDefenceFees ? "" : "Nil",
            //        // TransactionDetailsPaidthismonthExpenses = Map.TransactionDetailsPaidthismonthExpenses ? "" : "Nil",
            //        // TransactionDetailsPaidthismonthFees = Map.TransactionDetailsPaidthismonthFees ? "" : "Nil",
            //        // TransactionDetailsPaidthismonthIndemnity = Map.TransactionDetailsPaidthismonthIndemnity ? "" : "Nil",
            //        // TransactionDetailsPaidthismonthTPAFees = Map.TransactionDetailsPaidthismonthTPAFees ? "" : "Nil",
            //        // TransactionDetailsPreviouslyPaidAdjustersFees = Map.TransactionDetailsPreviouslyPaidAdjustersFees ? "" : "Nil",
            //        // TransactionDetailsPreviouslyPaidAttorneyCoverageFees = Map.TransactionDetailsPreviouslyPaidAttorneyCoverageFees ? "" : "Nil",
            //        // TransactionDetailsPreviouslyPaidDefenceFees = Map.TransactionDetailsPreviouslyPaidDefenceFees ? "" : "Nil",
            //        // TransactionDetailsPreviouslyPaidExpenses = Map.TransactionDetailsPreviouslyPaidExpenses ? "" : "Nil",
            //        // TransactionDetailsPreviouslyPaidFees = Map.TransactionDetailsPreviouslyPaidFees ? "" : "Nil",
            //        // TransactionDetailsPreviouslyPaidIndemnity = Map.TransactionDetailsPreviouslyPaidIndemnity ? "" : "Nil",
            //        // TransactionDetailsPreviouslyPaidTPAFees = Map.TransactionDetailsPreviouslyPaidTPAFees ? "" : "Nil",
            //        // TransactionDetailsReserveAdjustersFees = Map.TransactionDetailsReserveAdjustersFees ? "" : "Nil",
            //        // TransactionDetailsReserveAttorneyCoverageFees = Map.TransactionDetailsReserveAttorneyCoverageFees ? "" : "Nil",
            //        // TransactionDetailsReserveDefenceFees = Map.TransactionDetailsReserveDefenceFees ? "" : "Nil",
            //        // TransactionDetailsReserveExpenses = Map.TransactionDetailsReserveExpenses ? "" : "Nil",
            //        // TransactionDetailsReserveFees = Map.TransactionDetailsReserveFees ? "" : "Nil",
            //        // TransactionDetailsReserveIndemnity = Map.TransactionDetailsReserveIndemnity ? "" : "Nil",
            //        // TransactionDetailsReserveTPAFees = Map.TransactionDetailsReserveTPAFees ? "" : "Nil",
            //        // TransactionDetailsTotalIncurred = Map.TransactionDetailsTotalIncurred ? "" : "Nil",
            //        // TransactionDetailsTotalIncurredFees = Map.TransactionDetailsTotalIncurredFees ? "" : "Nil",
            //        // TransactionDetailsTotalIncurredIndemnity = Map.TransactionDetailsTotalIncurredIndemnity ? "" : "Nil",
            //        // USDetailsLossCounty = Map.USDetailsLossCounty ? "" : "Nil",
            //        // USDetailsMedicareConditionalPayments = Map.USDetailsMedicareConditionalPayments ? "" : "Nil",
            //        // USDetailsMedicareEligibilityCheckPerformance = Map.USDetailsMedicareEligibilityCheckPerformance ? "" : "Nil",
            //        // USDetailsMedicareMSPComplianceServices = Map.USDetailsMedicareMSPComplianceServices ? "" : "Nil",
            //        // USDetailsMedicareOutcomeofEligilibilityStatusCheck = Map.USDetailsMedicareOutcomeofEligilibilityStatusCheck ? "" : "Nil",
            //        // USDetailsMedicareUnitedStatesBodilyInjury = Map.USDetailsMedicareUnitedStatesBodilyInjury ? "" : "Nil",
            //        // USDetailsPCSCode = Map.USDetailsPCSCode ? "" : "Nil",
            //        // USDetailsStateofFiling = Map.USDetailsStateofFiling ? "" : "Nil"
            //    };
            //    ViewList.Add(v);
            //}
            //return ViewList;
            return null;
        }

        decimal CurrencyConversion(decimal value, string CurrencyCode)
        {
            try
            {
                decimal ret = new decimal();
                ICurrencyManagementService CurrencyManagementService = ServiceFactory.GetCurrencyManagementService();
                ICountryManagementService CountryManagementService = ServiceFactory.GetCountryManagementService();
                CurrencyResponseDto currency = CurrencyManagementService.GetAllCurrencies(SecurityHelper.Context, AuditHelper.Context).Currencies.Find(c => c.Code == CurrencyCode);
                CurrencyConversionPeriodResponseDto period = CurrencyManagementService.GetCurrencyConversionPeriods(SecurityHelper.Context, AuditHelper.Context).CurrencyConversionPeriods.Find(p => p.FromDate <= DateTime.Now && DateTime.Now < p.ToDate);
                CurrencyConversionResponseDto conversion = CurrencyManagementService.GetCurrencyConversions(SecurityHelper.Context, AuditHelper.Context).CurrencyConversions.Find(c => c.CurrencyId == currency.Id && c.CurrencyConversionPeriodId == period.Id);
                ret = value * conversion.Rate;
                return ret;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return value;
            }
        }

        [HttpPost]
        public object UserMappings(JObject data)
        {
            return UserMappings(Guid.Parse(data["UserId"].ToString()));
        }

        BordxViewMapResponseDto UserMappings(Guid UserId)
        {
            //BordxViewMapResponseDto ret = new BordxViewMapResponseDto();
            //IBordxManagementService BordxManagementService = ServiceFactory.GetBordxManagementService();
            //List<BordxReportColumnsResponseDto> cols = BordxManagementService.GetBordxReportColumnses(SecurityHelper.Context, AuditHelper.Context).BordxReportColumnses.FindAll(b => b.IsActive);
            //BordxReportColumnsMapsResponseDto Maps = BordxManagementService.GetBordxReportColumnsMaps(UserId, SecurityHelper.Context, AuditHelper.Context);
            //foreach (var item in Maps.BordxReportColumnsMaps)
            //{
            //    BordxReportColumnsResponseDto Col = cols.Find(c => c.Id == item.ColumnId);
            //    string ColName = Col.HeaderName + " " + Col.DisplayName;

            //    if (ColName == "Accident and Health / Medical Expenses Date of Treatment") ret.AccidentandHealthMedicalExpensesDateofTreatment = item.IsActive;
            //    if (ColName == "Accident and Health / Medical Expenses Patient Name") ret.AccidentandHealthMedicalExpensesPatientName = item.IsActive;
            //    if (ColName == "Accident and Health / Medical Expenses Plan") ret.AccidentandHealthMedicalExpensesPlan = item.IsActive;
            //    if (ColName == "Accident and Health / Medical Expenses Treatment Type") ret.AccidentandHealthMedicalExpensesTreatmentType = item.IsActive;
            //    if (ColName == "Accident and Health / Medical Expenses Country of Treatment") ret.AccidentandHealthMedicalExpensesCountryofTreatment = item.IsActive;
            //    if (ColName == "Claim Details Catastrophe Name") ret.ClaimDetailsCatastropheName = item.IsActive;
            //    if (ColName == "Claim Details Cause of Loss Code") ret.ClaimDetailsCauseofLossCode = item.IsActive;
            //    if (ColName == "Claim Details Claim Status") ret.ClaimDetailsClaimStatus = item.IsActive;
            //    if (ColName == "Claim Details Claimant Name") ret.ClaimDetailsDateClaimMade = item.IsActive;
            //    if (ColName == "Claim Details Date Claim Made") ret.ClaimDetailsDateClaimMade = item.IsActive;
            //    if (ColName == "Claim Details Date Closed") ret.ClaimDetailsDateClosed = item.IsActive;
            //    if (ColName == "Claim Details Date First Advised / Notification Date") ret.ClaimDetailsDateFirstAdvisedNotificationDate = item.IsActive;
            //    if (ColName == "Claim Details Date of Loss (From)") ret.ClaimDetailsDateofLossFrom = item.IsActive;
            //    if (ColName == "Claim Details Date of Loss to") ret.ClaimDetailsDateofLossto = item.IsActive;
            //    if (ColName == "Claim Details Denial") ret.ClaimDetailsDenial = item.IsActive;
            //    if (ColName == "Claim Details Lloyd's Cat Code") ret.ClaimDetailsLloydsCatCode = item.IsActive;
            //    if (ColName == "Claim Details Loss Description") ret.ClaimDetailsLossDescription = item.IsActive;
            //    if (ColName == "Claim Details Refer to Underwriters") ret.ClaimDetailsRefertoUnderwriters = item.IsActive;
            //    if (ColName == "Claim Notes Amount Claimed") ret.ClaimNotesAmountClaimed = item.IsActive;
            //    if (ColName == "Claim Notes Claim not paid as within excess") ret.ClaimNotesClaimnotpaidaswithinexcess = item.IsActive;
            //    if (ColName == "Claim Notes Complaint Reason") ret.ClaimNotesComplaintReason = item.IsActive;
            //    if (ColName == "Claim Notes Date claim closed") ret.ClaimNotesDateclaimclosed = item.IsActive;
            //    if (ColName == "Claim Notes Date Claim Denied") ret.ClaimNotesDateClaimDenied = item.IsActive;
            //    if (ColName == "Claim Notes Date claim withdrawn") ret.ClaimNotesDateclaimwithdrawn = item.IsActive;
            //    if (ColName == "Claim Notes Date of Complaint") ret.ClaimNotesDateofComplaint = item.IsActive;
            //    if (ColName == "Claim Notes Ex gratia payment") ret.ClaimNotesExgratiapayment = item.IsActive;
            //    if (ColName == "Claim Notes In Litigation") ret.ClaimNotesInLitigation = item.IsActive;
            //    if (ColName == "Claim Notes Reason for Denial") ret.ClaimNotesReasonforDenial = item.IsActive;
            //    if (ColName == "Claim Status Changes Date Claim Amount Agreed") ret.ClaimStatusChangesDateClaimAmountAgreed = item.IsActive;
            //    if (ColName == "Claim Status Changes Date Claim Opened") ret.ClaimStatusChangesDateClaimOpened = item.IsActive;
            //    if (ColName == "Claim Status Changes Date Claims Paid") ret.ClaimStatusChangesDateClaimsPaid = item.IsActive;
            //    if (ColName == "Claim Status Changes Date Coverage Confirmed") ret.ClaimStatusChangesDateCoverageConfirmed = item.IsActive;
            //    if (ColName == "Claim Status Changes Date Fees Paid") ret.ClaimStatusChangesDateFeesPaid = item.IsActive;
            //    if (ColName == "Claim Status Changes Date of Subrogation") ret.ClaimStatusChangesDateofSubrogation = item.IsActive;
            //    if (ColName == "Claim Status Changes Date Reopened") ret.ClaimStatusChangesDateReopened = item.IsActive;
            //    if (ColName == "Claimant Address Claimant Address") ret.ClaimantAddressClaimantAddress = item.IsActive;
            //    if (ColName == "Claimant Address Claimant Country") ret.ClaimantAddressClaimantCountry = item.IsActive;
            //    if (ColName == "Claimant Address Claimant Postcode") ret.ClaimantAddressClaimantPostcode = item.IsActive;
            //    if (ColName == "Class of Business Specific % Ceded (Reinsurance)") ret.ClassofBusinessSpecificPercentCededReinsurance = item.IsActive;
            //    if (ColName == "Class of Business Specific Name or Reg No of Aircraft Vehicle, Vessel etc") ret.ClassofBusinessSpecificNameorRegNoofAircraftVehicleVesseletc = item.IsActive;
            //    if (ColName == "Contract Details Agreement No") ret.ContractDetailsAgreementNo = item.IsActive;
            //    if (ColName == "Contract Details Class of Business") ret.ContractDetailsClassofBusiness = item.IsActive;
            //    if (ColName == "Contract Details Contract Expiry") ret.ContractDetailsContractExpiry = item.IsActive;
            //    if (ColName == "Contract Details Contract Inception") ret.ContractDetailsContractInception = item.IsActive;
            //    if (ColName == "Contract Details Coverholder Name") ret.ContractDetailsCoverholderName = item.IsActive;
            //    if (ColName == "Contract Details Coverholder PIN") ret.ContractDetailsCoverholderPIN = item.IsActive;
            //    if (ColName == "Contract Details Lloyd's Risk Code") ret.ContractDetailsLloydsRiskCode = item.IsActive;
            //    if (ColName == "Contract Details Original Currency") ret.ContractDetailsOriginalCurrency = item.IsActive;
            //    if (ColName == "Contract Details Rate of Exchange") ret.ContractDetailsRateofExchange = item.IsActive;
            //    if (ColName == "Contract Details Reporting Period (End Date)") ret.ContractDetailsReportingPeriodEndDate = item.IsActive;
            //    if (ColName == "Contract Details Reporting Period Start Date") ret.ContractDetailsReportingPeriodStartDate = item.IsActive;
            //    if (ColName == "Contract Details Section No") ret.ContractDetailsSectionNo = item.IsActive;
            //    if (ColName == "Contract Details Settlement Currency") ret.ContractDetailsSettlementCurrency = item.IsActive;
            //    if (ColName == "Contract Details TPA Name") ret.ContractDetailsTPAName = item.IsActive;
            //    if (ColName == "Contract Details Type of Insurance (Direct, or Type of RI)") ret.ContractDetailsTypeofInsuranceDirectorTypeofRI = item.IsActive;
            //    if (ColName == "Contract Details Unique Market Reference (UMR)") ret.ContractDetailsUniqueMarketReferenceUMR = item.IsActive;
            //    if (ColName == "Experts (Lawyer/Adjuster/Attorney etc Role") ret.ExpertsLawyerAdjusterAttorneyetcRole = item.IsActive;
            //    if (ColName == "Experts (Lawyer/Adjuster/Attorney etc) Address") ret.ExpertsLawyerAdjusterAttorneyetcAddress = item.IsActive;
            //    if (ColName == "Experts (Lawyer/Adjuster/Attorney etc) Country") ret.ExpertsLawyerAdjusterAttorneyetcCountry = item.IsActive;
            //    if (ColName == "Experts (Lawyer/Adjuster/Attorney etc) Firm / Company Name") ret.ExpertsLawyerAdjusterAttorneyetcFirmCompanyName = item.IsActive;
            //    if (ColName == "Experts (Lawyer/Adjuster/Attorney etc) Notes") ret.ExpertsLawyerAdjusterAttorneyetcNotes = item.IsActive;
            //    if (ColName == "Experts (Lawyer/Adjuster/Attorney etc) Postcode / Zip Code or similar") ret.ExpertsLawyerAdjusterAttorneyetcPostcodeZipCodeorsimilar = item.IsActive;
            //    if (ColName == "Experts (Lawyer/Adjuster/Attorney etc) Reference No etc") ret.ExpertsLawyerAdjusterAttorneyetcReferenceNoetc = item.IsActive;
            //    if (ColName == "Experts (Lawyer/Adjuster/Attorney etc) State, Province, Territory, Canton etc") ret.ExpertsLawyerAdjusterAttorneyetcStateProvinceTerritoryCantonetc = item.IsActive;
            //    if (ColName == "Insured Details Address") ret.InsuredDetailsAddress = item.IsActive;
            //    if (ColName == "Insured Details Country") ret.InsuredDetailsCountry = item.IsActive;
            //    if (ColName == "Insured Details Full Name or Company Name") ret.InsuredDetailsFullNameorCompanyName = item.IsActive;
            //    if (ColName == "Insured Details Postcode / Zip Code or similar") ret.InsuredDetailsPostcodeZipCodeorsimilar = item.IsActive;
            //    if (ColName == "Insured Details State, Province, Territory, Canton etc") ret.InsuredDetailsStateProvinceTerritoryCantonetc = item.IsActive;
            //    if (ColName == "Location of Loss Address") ret.LocationofLossAddress = item.IsActive;
            //    if (ColName == "Location of Loss Country") ret.LocationofLossCountry = item.IsActive;
            //    if (ColName == "Location of Loss Postcode / Zip Code or similar") ret.LocationofLossPostcodeZipCodeorsimilar = item.IsActive;
            //    if (ColName == "Location of Loss State, Province, Territory, Canton etc") ret.LocationofLossStateProvinceTerritoryCantonetc = item.IsActive;
            //    if (ColName == "Location of Risk Address") ret.LocationofRiskAddress = item.IsActive;
            //    if (ColName == "Location of Risk Country") ret.LocationofRiskCountry = item.IsActive;
            //    if (ColName == "Location of Risk Location ID") ret.LocationofRiskLocationID = item.IsActive;
            //    if (ColName == "Location of Risk Postcode / Zip Code or similar") ret.LocationofRiskPostcodeZipCodeorsimilar = item.IsActive;
            //    if (ColName == "Location of Risk State, Province, Territory, Canton etc") ret.LocationofRiskStateProvinceTerritoryCantonetc = item.IsActive;
            //    if (ColName == "References Certificate Reference") ret.ReferencesCertificateReference = item.IsActive;
            //    if (ColName == "References Claim Reference") ret.ReferencesClaimReference = item.IsActive;
            //    if (ColName == "Refs Policy or Group Ref") ret.RefsPolicyorGroupRef = item.IsActive;
            //    if (ColName == "Risk Details Deductible Amount") ret.RiskDetailsDeductibleAmount = item.IsActive;
            //    if (ColName == "Risk Details Deductible Basis") ret.RiskDetailsDeductibleBasis = item.IsActive;
            //    if (ColName == "Risk Details Period of Cover - Narrative") ret.RiskDetailsPeriodofCoverNarrative = item.IsActive;
            //    if (ColName == "Risk Details Risk Expiry Date") ret.RiskDetailsRiskExpiryDate = item.IsActive;
            //    if (ColName == "Risk Details Risk Inception Date") ret.RiskDetailsRiskInceptionDate = item.IsActive;
            //    if (ColName == "Risk Details Sums Insured Amount") ret.RiskDetailsSumsInsuredAmount = item.IsActive;
            //    if (ColName == "Row No Row No") ret.RowNo = item.IsActive;
            //    if (ColName == "Transaction Details Change this month - Fees") ret.TransactionDetailsChangethismonthFees = item.IsActive;
            //    if (ColName == "Transaction Details Change this month - Indemnity") ret.TransactionDetailsChangethismonthIndemnity = item.IsActive;
            //    if (ColName == "Transaction Details Paid this month - Adjusters Fees") ret.TransactionDetailsPaidthismonthAdjustersFees = item.IsActive;
            //    if (ColName == "Transaction Details Paid this month - Attorney Coverage Fees") ret.TransactionDetailsPaidthismonthAttorneyCoverageFees = item.IsActive;
            //    if (ColName == "Transaction Details Paid this month - Defence Fees") ret.TransactionDetailsPaidthismonthDefenceFees = item.IsActive;
            //    if (ColName == "Transaction Details Paid this month - Expenses") ret.TransactionDetailsPaidthismonthExpenses = item.IsActive;
            //    if (ColName == "Transaction Details Paid this month - Fees") ret.TransactionDetailsPaidthismonthFees = item.IsActive;
            //    if (ColName == "Transaction Details Paid this month - Indemnity") ret.TransactionDetailsPaidthismonthIndemnity = item.IsActive;
            //    if (ColName == "Transaction Details Paid this month - TPA Fees") ret.TransactionDetailsPaidthismonthTPAFees = item.IsActive;
            //    if (ColName == "Transaction Details Previously Paid - Adjusters Fees") ret.TransactionDetailsPreviouslyPaidAdjustersFees = item.IsActive;
            //    if (ColName == "Transaction Details Previously Paid - Attorney Coverage Fees") ret.TransactionDetailsPreviouslyPaidAttorneyCoverageFees = item.IsActive;
            //    if (ColName == "Transaction Details Previously Paid - Defence Fees") ret.TransactionDetailsPreviouslyPaidDefenceFees = item.IsActive;
            //    if (ColName == "Transaction Details Previously Paid - Expenses") ret.TransactionDetailsPreviouslyPaidExpenses = item.IsActive;
            //    if (ColName == "Transaction Details Previously Paid - Fees") ret.TransactionDetailsPreviouslyPaidFees = item.IsActive;
            //    if (ColName == "Transaction Details Previously Paid - Indemnity") ret.TransactionDetailsPreviouslyPaidIndemnity = item.IsActive;
            //    if (ColName == "Transaction Details Previously Paid - TPA Fees") ret.TransactionDetailsPreviouslyPaidTPAFees = item.IsActive;
            //    if (ColName == "Transaction Details Reserve - Adjusters Fees") ret.TransactionDetailsReserveAdjustersFees = item.IsActive;
            //    if (ColName == "Transaction Details Reserve - Attorney Coverage Fees") ret.TransactionDetailsReserveAttorneyCoverageFees = item.IsActive;
            //    if (ColName == "Transaction Details Reserve - Defence Fees") ret.TransactionDetailsReserveDefenceFees = item.IsActive;
            //    if (ColName == "Transaction Details Reserve - Expenses") ret.TransactionDetailsReserveExpenses = item.IsActive;
            //    if (ColName == "Transaction Details Reserve - Fees") ret.TransactionDetailsReserveFees = item.IsActive;
            //    if (ColName == "Transaction Details Reserve - Indemnity") ret.TransactionDetailsReserveIndemnity = item.IsActive;
            //    if (ColName == "Transaction Details Reserve - TPA Fees") ret.TransactionDetailsReserveTPAFees = item.IsActive;
            //    if (ColName == "Transaction Details Total Incurred") ret.TransactionDetailsTotalIncurred = item.IsActive;
            //    if (ColName == "Transaction Details Total Incurred - Fees") ret.TransactionDetailsTotalIncurredFees = item.IsActive;
            //    if (ColName == "Transaction Details Total Incurred - Indemnity") ret.TransactionDetailsTotalIncurredIndemnity = item.IsActive;
            //    if (ColName == "US Details Loss County") ret.USDetailsLossCounty = item.IsActive;
            //    if (ColName == "US Details Medicare Conditional Payments") ret.USDetailsMedicareConditionalPayments = item.IsActive;
            //    if (ColName == "US Details Medicare Eligibility Check Performance") ret.USDetailsMedicareEligibilityCheckPerformance = item.IsActive;
            //    if (ColName == "US Details Medicare MSP Compliance Services") ret.USDetailsMedicareMSPComplianceServices = item.IsActive;
            //    if (ColName == "US Details Medicare Outcome of Eligilibility Status Check") ret.USDetailsMedicareOutcomeofEligilibilityStatusCheck = item.IsActive;
            //    if (ColName == "US Details Medicare United States Bodily Injury") ret.USDetailsMedicareUnitedStatesBodilyInjury = item.IsActive;
            //    if (ColName == "US Details PCS Code") ret.USDetailsPCSCode = item.IsActive;
            //    if (ColName == "US Details State of Filing") ret.USDetailsStateofFiling = item.IsActive;
            //}
            //return ret;

            return null;
        }

        [HttpPost]
        public object GetBordxColumns(JObject data)
        {
            IBordxManagementService BordxManagementService = ServiceFactory.GetBordxManagementService();
            BordxColumnRequestDto bordxColumnRequestDto = data.ToObject<BordxColumnRequestDto>();
            // List<BordxReportColumnsMapResponseDto> Maps = BordxManagementService.GetBordxReportColumnsMaps(Guid.Parse(data["Id"].ToString()), SecurityHelper.Context, AuditHelper.Context).BordxReportColumnsMaps;
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            return BordxManagementService.GetBordxReportColumns(bordxColumnRequestDto,SecurityHelper.Context, AuditHelper.Context);
            //if (Maps.Count > 0)
            //{
            //    List<MapCols> MapList = new List<MapCols>();
            //    foreach (var item in cols)
            //    {
            //        foreach (var itemIn in Maps)
            //        {
            //            if (item.Id == itemIn.ColumnId)
            //            {
            //                MapCols m = new MapCols()
            //                {
            //                    ColumnId = item.Id,
            //                    UserId = Guid.Parse(data["Id"].ToString()),
            //                    IsActive = itemIn.IsActive,
            //                    ColumnName = item.Description,
            //                    HeaderName = item.HeaderName,
            //                    DisplayName = item.DisplayName,
            //                    Status = itemIn.IsActive ? "Active" : "Inactive",
            //                    Id = itemIn.Id
            //                };
            //                MapList.Add(m);
            //            }
            //        }
            //    }
            //    return MapList.ToArray();
            //}
            //else
            //{
            //    Maps = new List<BordxReportColumnsMapResponseDto>();
            //    foreach (var item in cols)
            //    {
            //        MapCols m = new MapCols()
            //        {
            //            ColumnId = item.Id,
            //            UserId = Guid.Parse(data["Id"].ToString()),
            //            IsActive = false,
            //            ColumnName = item.Description,
            //            HeaderName = item.HeaderName,
            //            DisplayName = item.DisplayName,
            //            Status = "Inactive",
            //            Id = Guid.Parse("00000000-0000-0000-0000-000000000000")
            //        };
            //        Maps.Add(m);
            //    }
            //    return Maps.ToArray();
            //}
        }

        [HttpPost]
        public string AddBordxColumns(JObject data)
        {

            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            List<BordxReportColumnsMapRequestDto> Col = data["data"].ToObject<List<BordxReportColumnsMapRequestDto>>();

            IBordxManagementService BordxManagementService = ServiceFactory.GetBordxManagementService();
            foreach (var item in Col)
            {
                if (item.Id.ToString() == "00000000-0000-0000-0000-000000000000")
                {
                    BordxReportColumnsMapRequestDto result = BordxManagementService.AddBordxReportColumnsMap(item, SecurityHelper.Context, AuditHelper.Context);
                }
                else
                {
                    BordxReportColumnsMapRequestDto result = BordxManagementService.UpdateBordxReportColumnsMap(item, SecurityHelper.Context, AuditHelper.Context);
                }
            }
            return "ok";
        }

        [HttpPost]
        public string ConformBordx(JObject data)
        {

            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            IPolicyManagementService PolicyManagementService = ServiceFactory.GetPolicyManagementService();
            try
            {
                Convert.ToInt32(data["Month"].ToString());
            }
            catch
            {
                data["Month"] = DateTime.ParseExact(data["Month"].ToString(), "MMMM", CultureInfo.InvariantCulture).Month;
            }
            BordxRequestDto Bordx = data.ToObject<BordxRequestDto>();
            IBordxManagementService BordxManagementService = ServiceFactory.GetBordxManagementService();
            Bordx.IsConformed = true;
            BordxRequestDto result = BordxManagementService.UpdateBordx(Bordx, SecurityHelper.Context, AuditHelper.Context);

            List<PolicyRequestDto> policies = new List<PolicyRequestDto>();
            try
            {
                policies = data["Policies"].ToObject<List<PolicyRequestDto>>();
                foreach (var item in policies)
                {
                    item.BordxId = Bordx.Id;
                    item.Year = 0;
                    item.Month = 0;
                    PolicyRequestDto resultPol = PolicyManagementService.UpdatePolicy(item, SecurityHelper.Context, AuditHelper.Context);
                }
            }
            catch
            {
                List<Guid> pList = data["Policy"].ToObject<List<Guid>>();
                foreach (var pol in PolicyManagementService.GetPolicys(SecurityHelper.Context, AuditHelper.Context).Policies.FindAll(p => pList.Contains(p.Id)))
                {
                    PolicyRequestDto p = new PolicyRequestDto()
                    {
                        BordxId = Bordx.Id,
                        Year = 0,
                        Month = 0,
                        Comment = pol.Comment,
                        CommodityTypeId = pol.CommodityTypeId,
                        ContractId = pol.ContractId,
                        CoverTypeId = pol.CoverTypeId,
                        CustomerId = pol.CustomerId,
                        CustomerPayment = pol.CustomerPayment,
                        CustomerPaymentCurrencyTypeId = pol.CustomerPaymentCurrencyTypeId,
                        DealerId = pol.DealerId,
                        DealerLocationId = pol.DealerLocationId,
                        DealerPayment = pol.DealerPayment,
                        DealerPaymentCurrencyTypeId = pol.DealerPaymentCurrencyTypeId,
                        Discount = pol.Discount,
                        EntryDateTime = pol.EntryDateTime,
                        EntryUser = pol.EntryUser,
                        ExtensionTypeId = pol.ExtensionTypeId,
                        ForwardComment = pol.ForwardComment,
                        HrsUsedAtPolicySale = pol.HrsUsedAtPolicySale,
                        Id = pol.Id,
                        IsApproved = pol.IsApproved,
                        IsPartialPayment = pol.IsPartialPayment,
                        IsPolicyCanceled = pol.IsPolicyCanceled,
                        IsPreWarrantyCheck = pol.IsPreWarrantyCheck,
                        IsSpecialDeal = pol.IsSpecialDeal,
                        ItemId = pol.ItemId,
                        PaymentModeId = pol.PaymentModeId,
                        PolicyBundleId = pol.PolicyBundleId,
                        PolicyEndDate = pol.PolicyEndDate,
                        PolicyNo = pol.PolicyNo,
                        PolicySoldDate = pol.PolicySoldDate,
                        PolicyStartDate = pol.PolicyStartDate,
                        Premium = pol.Premium,
                        PremiumCurrencyTypeId = pol.PremiumCurrencyTypeId,
                        ProductId = pol.ProductId,
                        RefNo = pol.RefNo,
                        SalesPersonId = pol.SalesPersonId,
                        TransferFee = pol.TransferFee,
                        Type = pol.Type,
                    };
                    PolicyRequestDto resultPol = PolicyManagementService.UpdatePolicy(p, SecurityHelper.Context, AuditHelper.Context);
                }
            }

            logger.Info("Bordx Added");
            if (result.BordxInsertion)
            {
                return result.Id.ToString();
            }
            else
            {
                return "Add Bordx failed!";
            }
        }

        [HttpPost]
        public string ReOpenBordx(JObject data)
        {

            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            IPolicyManagementService PolicyManagementService = ServiceFactory.GetPolicyManagementService();
            try
            {
                Convert.ToInt32(data["Month"].ToString());
            }
            catch
            {
                data["Month"] = DateTime.ParseExact(data["Month"].ToString(), "MMMM", CultureInfo.InvariantCulture).Month;
            }
            BordxRequestDto Bordx = data.ToObject<BordxRequestDto>();
            IBordxManagementService BordxManagementService = ServiceFactory.GetBordxManagementService();
            Bordx.IsConformed = false;
            BordxRequestDto result = BordxManagementService.UpdateBordx(Bordx, SecurityHelper.Context, AuditHelper.Context);

            List<PolicyRequestDto> policies = new List<PolicyRequestDto>();
            try
            {
                policies = data["Policies"].ToObject<List<PolicyRequestDto>>();
                foreach (var item in policies)
                {
                    item.BordxId = Guid.Parse("00000000-0000-0000-0000-000000000000");
                    item.Year = 0;
                    item.Month = 0;
                    PolicyRequestDto resultPol = PolicyManagementService.UpdatePolicy(item, SecurityHelper.Context, AuditHelper.Context);
                }
            }
            catch
            {
                List<Guid> pList = data["Policies"].ToObject<List<Guid>>();
                foreach (var pol in PolicyManagementService.GetPolicys(SecurityHelper.Context, AuditHelper.Context).Policies.FindAll(p => pList.Contains(p.Id)))
                {
                    PolicyRequestDto p = new PolicyRequestDto()
                    {
                        BordxId = Guid.Parse("00000000-0000-0000-0000-000000000000"),
                        Year = 0,
                        Month = 0,
                        Comment = pol.Comment,
                        CommodityTypeId = pol.CommodityTypeId,
                        ContractId = pol.ContractId,
                        CoverTypeId = pol.CoverTypeId,
                        CustomerId = pol.CustomerId,
                        CustomerPayment = pol.CustomerPayment,
                        CustomerPaymentCurrencyTypeId = pol.CustomerPaymentCurrencyTypeId,
                        DealerId = pol.DealerId,
                        DealerLocationId = pol.DealerLocationId,
                        DealerPayment = pol.DealerPayment,
                        DealerPaymentCurrencyTypeId = pol.DealerPaymentCurrencyTypeId,
                        Discount = pol.Discount,
                        EntryDateTime = pol.EntryDateTime,
                        EntryUser = pol.EntryUser,
                        ExtensionTypeId = pol.ExtensionTypeId,
                        ForwardComment = pol.ForwardComment,
                        HrsUsedAtPolicySale = pol.HrsUsedAtPolicySale,
                        Id = pol.Id,
                        IsApproved = pol.IsApproved,
                        IsPartialPayment = pol.IsPartialPayment,
                        IsPolicyCanceled = pol.IsPolicyCanceled,
                        IsPreWarrantyCheck = pol.IsPreWarrantyCheck,
                        IsSpecialDeal = pol.IsSpecialDeal,
                        ItemId = pol.ItemId,
                        PaymentModeId = pol.PaymentModeId,
                        PolicyBundleId = pol.PolicyBundleId,
                        PolicyEndDate = pol.PolicyEndDate,
                        PolicyNo = pol.PolicyNo,
                        PolicySoldDate = pol.PolicySoldDate,
                        PolicyStartDate = pol.PolicyStartDate,
                        Premium = pol.Premium,
                        PremiumCurrencyTypeId = pol.PremiumCurrencyTypeId,
                        ProductId = pol.ProductId,
                        RefNo = pol.RefNo,
                        SalesPersonId = pol.SalesPersonId,
                        TransferFee = pol.TransferFee,
                        Type = pol.Type,
                    };
                    PolicyRequestDto resultPol = PolicyManagementService.UpdatePolicy(p, SecurityHelper.Context, AuditHelper.Context);
                }
            }

            //logger.Info("Bordx Added");
            if (result.BordxInsertion)
            {
                return result.Id.ToString();
            }
            else
            {
                return "Add Bordx failed!";
            }
        }

        [HttpPost]
        public object GetBordxReportColumnses(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            BordxColumnRequestDto bordxColumnRequestDto = data.ToObject<BordxColumnRequestDto>();
            IBordxManagementService BordxManagementService = ServiceFactory.GetBordxManagementService();
            return BordxManagementService.GetBordxReportColumns(bordxColumnRequestDto,
            SecurityHelper.Context,
            AuditHelper.Context);
            //  return BordxReportColumnsData.BordxReportColumnses.ToArray();
        }

        [HttpPost]
        public object GetConfirmedBordxForGrid(ConfirmedBordxForGridRequestDto ConfirmedBordxForGridRequest)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            IBordxManagementService BordxManagementService = ServiceFactory.GetBordxManagementService();
            object ConfirmedBordxForGridResponse = BordxManagementService.GetConfirmedBordxForGrid
             (
             ConfirmedBordxForGridRequest,
             SecurityHelper.Context,
             AuditHelper.Context
             );
            return ConfirmedBordxForGridResponse;
        }
        [HttpPost]
        public object GetConfirmedBordxYears()
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            IBordxManagementService BordxManagementService = ServiceFactory.GetBordxManagementService();
            object ConfirmedBordxYears = BordxManagementService.ConfirmedBordxYears
             (
             SecurityHelper.Context,
             AuditHelper.Context
             );
            return ConfirmedBordxYears;
        }


        [HttpPost]
        public object GetPoliciesByBordxIdForGrid(GetPoliciesByBordxIdRequestDto GetPoliciesByBordxIdRequestDto)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            IPolicyManagementService PolicyManagementService = ServiceFactory.GetPolicyManagementService();

            object PolicyList = PolicyManagementService.GetPoliciesByBordxIdForGrid(GetPoliciesByBordxIdRequestDto,
                SecurityHelper.Context,
                AuditHelper.Context);

            return PolicyList;
        }
        [HttpPost]
        public object GetConfirmedBordxForViewGrid(BordxViewGridRequestDto BordxViewGridRequestDto)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            IPolicyManagementService PolicyManagementService = ServiceFactory.GetPolicyManagementService();

            object PolicyList = PolicyManagementService.GetPoliciesByBordxIdForViewGrid(BordxViewGridRequestDto,
                SecurityHelper.Context,
                AuditHelper.Context);

            return PolicyList;
        }
        [HttpPost]
        public HttpResponseMessage ExportPoliciesToExcelByBordxId(ExportPoliciesToExcelByBordxIdRequestDto ExportPoliciesToExcelByBordxIdRequestDto)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            IPolicyManagementService PolicyManagementService = ServiceFactory.GetPolicyManagementService();

            BordxExportResponseDto BordxResponse = PolicyManagementService.ExportPoliciesToExcelByBordxId(ExportPoliciesToExcelByBordxIdRequestDto,
                SecurityHelper.Context,
                AuditHelper.Context);

            if (BordxResponse.BordxData != null && BordxResponse.BordxData.Count > 0)
            {
                var stream = new ExcelUtlity(SecurityHelper.Context, AuditHelper.Context).GenerateBordxExcel(BordxResponse);
                var fileName = ((string.IsNullOrEmpty(BordxResponse.reportName)) ?
                    "Bordx_" + BordxResponse.BordxYear + "_" + BordxResponse.BordxMonth + "_" + BordxResponse.TpaName
                    : BordxResponse.reportName)
                    + ".xlsx";

                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
                result.Content = new StreamContent(stream);
                result.Content.Headers.ContentType =
                    new MediaTypeHeaderValue("application/octet-stream");
                result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment") { FileName = fileName };
                return result;
            }
            else
            {
                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
                return result;
            }

        }
        [HttpPost]
        public object GetAllBordxDetailsByYearMonth(BordxDetailsByYearMonthRequestDto bordxRequestDetails)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            IBordxManagementService bordxManagementService = ServiceFactory.GetBordxManagementService();
            return bordxManagementService.GetAllBordxDetailsByYearMonth(bordxRequestDetails, SecurityHelper.Context, AuditHelper.Context);
        }
        [HttpPost]
        public string ProcessBordx(BordxProcessRequestDto bordxProcessRequest)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            IBordxManagementService bordxManagementService = ServiceFactory.GetBordxManagementService();
            return bordxManagementService.ProcessBordx(bordxProcessRequest, SecurityHelper.Context, AuditHelper.Context);
        }

        [HttpPost]
        public string ConfirmBordx(BordxProcessRequestDto bordxProcessRequest)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            IBordxManagementService bordxManagementService = ServiceFactory.GetBordxManagementService();
            return bordxManagementService.ConfirmBordx(bordxProcessRequest, SecurityHelper.Context, AuditHelper.Context);
        }


        [HttpPost]
        public string CreateBordx(BordxCreateRequestDto bordxCreateRequest)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            IBordxManagementService bordxManagementService = ServiceFactory.GetBordxManagementService();
            return bordxManagementService.CreateBordx(bordxCreateRequest, SecurityHelper.Context, AuditHelper.Context);
        }

        [HttpPost]
        public object GetLast10Bordx(JObject data)
        {
            BordxListRequestDto bordxListRequestDto = new BordxListRequestDto();
            try {
                bordxListRequestDto = data.ToObject<BordxListRequestDto>();
            } catch (Exception ex) { }
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            IBordxManagementService bordxManagementService = ServiceFactory.GetBordxManagementService();
            return bordxManagementService.GetLast10Bordx(bordxListRequestDto,SecurityHelper.Context, AuditHelper.Context);
        }

        [HttpPost]
        public string GetNextBordxNumber(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            int year = Convert.ToInt32(data["year"].ToString());
            int month = Convert.ToInt32(data["month"].ToString());

            Guid insurerId = Guid.Empty;
            Guid reinsurerId = Guid.Empty;
            Guid CommodityTypeId = Guid.Empty;
            Guid productId = Guid.Empty;

            bool insured = Guid.TryParse(data["insurerId"].ToString(), out insurerId);
            bool reinsured = Guid.TryParse(data["reinsurerId"].ToString() , out reinsurerId);
            //Guid CountryId = Guid.Parse(data["countryId"].ToString());
            bool CommodityType = Guid.TryParse(data["commodityTypeId"].ToString(),out CommodityTypeId);
            bool product = Guid.TryParse(data["productId"].ToString() , out productId);
            if (insured == false || reinsured == false || CommodityType == false || product == false) {
                return "";
            }


            IBordxManagementService bordxManagementService = ServiceFactory.GetBordxManagementService();
            return bordxManagementService.GetNextBordxNumber(year, month, reinsurerId, insurerId , productId, CommodityTypeId, SecurityHelper.Context, AuditHelper.Context);
        }


        [HttpPost]
        public string DeleteBordx(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            Guid BordxId = Guid.Parse(data["bordxId"].ToString());

            IBordxManagementService bordxManagementService = ServiceFactory.GetBordxManagementService();
            return bordxManagementService.DeleteBordx(BordxId, SecurityHelper.Context, AuditHelper.Context);
        }

        [HttpPost]
        public object GetNextBordxNumbers(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            BordxNumbersRequestDto bordxNumberDetails = data.ToObject<BordxNumbersRequestDto>();


            if (bordxNumberDetails.CommodityTypeId == Guid.Empty || bordxNumberDetails.ReinsurerId == Guid.Empty || bordxNumberDetails.InsurerId == Guid.Empty || bordxNumberDetails.ProductId == Guid.Empty)
            {
                return new List<String>();
            }

            IBordxManagementService bordxManagementService = ServiceFactory.GetBordxManagementService();
            return bordxManagementService.GetNextBordxNumbers(bordxNumberDetails.CommodityTypeId, bordxNumberDetails.ReinsurerId, bordxNumberDetails.InsurerId, bordxNumberDetails.ProductId, bordxNumberDetails.year, bordxNumberDetails.month, SecurityHelper.Context, AuditHelper.Context);
        }


        [HttpPost]
        public string TransferPolicyToBordx(BordxTransferRequestDto BordxTransferRequestDto)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            IBordxManagementService bordxManagementService = ServiceFactory.GetBordxManagementService();
            return bordxManagementService.TransferPolicyToBordx(BordxTransferRequestDto, SecurityHelper.Context, AuditHelper.Context);
        }


        [HttpPost]
        public string BordxReopen(BordxReopenRequestDto BordxReopenRequestDto)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            IBordxManagementService bordxManagementService = ServiceFactory.GetBordxManagementService();
            return bordxManagementService.BordxReopen(BordxReopenRequestDto, SecurityHelper.Context, AuditHelper.Context);
        }


        #region Bordx Report Template

        [HttpPost]
        public object GetBordxReportTemplates(JObject data)
        {
            BordxTemplateRequestDto bordxTemplateRequestDto = data.ToObject<BordxTemplateRequestDto>();
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            IBordxManagementService BordxManagementService = ServiceFactory.GetBordxManagementService();
            return BordxManagementService.GetBordxReportTemplates(bordxTemplateRequestDto,SecurityHelper.Context, AuditHelper.Context);
        }

        [HttpPost]
        public object GetBordxReportTemplateColumns(JObject data)
        {
            BordxColumnRequestDto bordxColumnRequestDto = data.ToObject<BordxColumnRequestDto>();
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            IBordxManagementService BordxManagementService = ServiceFactory.GetBordxManagementService();
            return BordxManagementService.GetBordxReportTemplateColumns(bordxColumnRequestDto, SecurityHelper.Context, AuditHelper.Context);
        }

        [HttpPost]
        public object GetAllBordxReportTemplateForSearchGrid(BordxReportTemplateSearchGridRequestDto bordxReportTemlpateSearchGridRequestDto)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            IBordxManagementService bordxManagementService = ServiceFactory.GetBordxManagementService();
            return bordxManagementService.GetAllBordxReportTemplateForSearchGrid(bordxReportTemlpateSearchGridRequestDto, SecurityHelper.Context, AuditHelper.Context);
        }

        [HttpPost]
        public object GetBordxReportTemplateById(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                IBordxManagementService bordxManagementService = ServiceFactory.GetBordxManagementService();
                return bordxManagementService.GetBordxReportTemplateById(Guid.Parse(data["Id"].ToString()), SecurityHelper.Context, AuditHelper.Context);

            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }

        }
        [HttpPost]
        public object SaveBordxReportTemplate(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                BordxReportTemplateRequestDto bordxReportTemplate = data.ToObject<BordxReportTemplateRequestDto>();
                IBordxManagementService BordxManagementService = ServiceFactory.GetBordxManagementService();

                if (!BordxManagementService.BordxReportTemplateNameIsExists(bordxReportTemplate, SecurityHelper.Context, AuditHelper.Context))
                {

                    bool result = BordxManagementService.SaveBordxReportTemplate(bordxReportTemplate, SecurityHelper.Context, AuditHelper.Context);
                    logger.Info("Bordx Report Template Added");
                    if (result) { return result; }
                    else { return "Save failed!"; }
                }
                else
                {
                    return "Name Already Exists!";
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return "Save failed!";
            }

        }

        #endregion
    }

    public class BordxInfo : BordxResponseDto
    {
        public List<Guid> Policy { get; set; }
    }
    public class BordxPolicyInfo
    {
        public BordxResponseDto Bordx { get; set; }
        public List<PolicyResponseDto> Policies { get; set; }
    }
    public class MapCols : BordxReportColumnsMapResponseDto
    {
        public string HeaderName { get; set; }
        public string DisplayName { get; set; }
        public string ColumnName { get; set; }
        public string Status { get; set; }
    }

}