using NHibernate;
using NHibernate.Linq;
using NHibernate.Transform;
using NHibernate.Util;
using NLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using TAS.DataTransfer.Exceptions;
using TAS.DataTransfer.Requests;
using TAS.DataTransfer.Responses;
using TAS.Services.Common;
using TAS.Services.Common.Notification;
using TAS.Services.Common.Transformer;
using TAS.Services.Entities.Persistence;
using TAS.Services.Reports;
using TAS.Services.Reports.PolicyStatement;
using TAS.Caching;

namespace TAS.Services.Entities.Management
{
    public class PolicyEntityManager
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        #region Policy
        public List<PolicyInfo> GetPolicys()
        {
            List<PolicyInfo> entities = new List<PolicyInfo>();

            try
            {
                ISession session = EntitySessionManager.GetSession();
                IQueryable<Policy> PolicyData = session.Query<Policy>();
                IQueryable<VehiclePolicy> VPolicyData = session.Query<VehiclePolicy>();
                IQueryable<BAndWPolicy> BPolicyData = session.Query<BAndWPolicy>();
                IQueryable<OtherItemPolicy> OPolicyData = session.Query<OtherItemPolicy>();
                IQueryable<YellowGoodPolicy> YPolicyData = session.Query<YellowGoodPolicy>();
                if (PolicyData.ToList().Count > 0)
                {
                    if (VPolicyData.ToList().Count > 0)
                    {
                        foreach (var item in VPolicyData)
                        {
                            Policy policy = PolicyData.FirstOrDefault(p => p.Id == item.PolicyId);
                            if (policy != null)
                            {
                                entities.Add(new PolicyInfo()
                                {
                                    Comment = policy.Comment,
                                    CommodityTypeId = policy.CommodityTypeId,
                                    ContractId = policy.ContractId,
                                    CoverTypeId = policy.CoverTypeId,
                                    CurrencyPeriodId = policy.CurrencyPeriodId,
                                    PremiumCurrencyTypeId = policy.PremiumCurrencyTypeId,
                                    DealerPaymentCurrencyTypeId = policy.DealerPaymentCurrencyTypeId,
                                    CustomerPaymentCurrencyTypeId = policy.CustomerPaymentCurrencyTypeId,
                                    ContractInsuaranceLimitationId = policy.ContractInsuaranceLimitationId,
                                    ContractExtensionPremiumId = policy.ContractExtensionPremiumId,
                                    ContractExtensionsId = policy.ContractExtensionsId,
                                    CustomerId = policy.CustomerId,
                                    CustomerPayment = policy.CustomerPayment,
                                    DealerId = policy.DealerId,
                                    DealerLocationId = policy.DealerLocationId,
                                    DealerPayment = policy.DealerPayment,
                                    EntryDateTime = policy.EntryDateTime,
                                    EntryUser = policy.EntryUser,
                                    ExtensionTypeId = policy.ExtensionTypeId,
                                    HrsUsedAtPolicySale = policy.HrsUsedAtPolicySale,
                                    Id = policy.Id,
                                    IsPartialPayment = policy.IsPartialPayment,
                                    IsPreWarrantyCheck = policy.IsPreWarrantyCheck,
                                    IsSpecialDeal = policy.IsSpecialDeal,
                                    ItemId = item.VehicleId,
                                    ProductId = policy.ProductId,
                                    PaymentModeId = policy.PaymentModeId,
                                    PaymentTypeId = policy.PaymentTypeId,
                                    PolicyNo = policy.PolicyNo,
                                    PolicySoldDate = policy.PolicySoldDate,
                                    Premium = policy.Premium,
                                    RefNo = policy.RefNo,
                                    SalesPersonId = policy.SalesPersonId,
                                    Type = "Vehicle",
                                    IsApproved = policy.IsApproved,
                                    IsPolicyCanceled = policy.IsPolicyCanceled,
                                    PolicyStartDate = policy.PolicyStartDate,
                                    PolicyEndDate = policy.PolicyEndDate,
                                    Discount = policy.Discount,
                                    DiscountPercentage = policy.DiscountPercentage,
                                    PolicyBundleId = policy.PolicyBundleId,
                                    TransferFee = policy.TransferFee,
                                    BordxId = policy.BordxId,
                                    Year = policy.Year,
                                    Month = policy.Month,
                                    ForwardComment = policy.ForwardComment,
                                    DealerPolicy = policy.DealerPolicy,
                                    TPABranchId = policy.TPABranchId,
                                    MWStartDate = policy.MWStartDate,
                                    MWIsAvailable = policy.MWIsAvailable,
                                    BookletNumber = policy.BookletNumber,
                                });
                            }
                        }
                    }
                    if (BPolicyData.ToList().Count > 0)
                    {
                        foreach (var item in BPolicyData)
                        {
                            Policy policy = PolicyData.FirstOrDefault(p => p.Id == item.PolicyId);
                            if (policy != null)
                            {
                                entities.Add(new PolicyInfo()
                                {
                                    Comment = policy.Comment,
                                    CommodityTypeId = policy.CommodityTypeId,
                                    ContractId = policy.ContractId,
                                    CoverTypeId = policy.CoverTypeId,
                                    CurrencyPeriodId = policy.CurrencyPeriodId,
                                    PremiumCurrencyTypeId = policy.PremiumCurrencyTypeId,
                                    DealerPaymentCurrencyTypeId = policy.DealerPaymentCurrencyTypeId,
                                    CustomerPaymentCurrencyTypeId = policy.CustomerPaymentCurrencyTypeId,
                                    CustomerId = policy.CustomerId,
                                    CustomerPayment = policy.CustomerPayment,
                                    DealerId = policy.DealerId,
                                    DealerLocationId = policy.DealerLocationId,
                                    DealerPayment = policy.DealerPayment,
                                    EntryDateTime = policy.EntryDateTime,
                                    EntryUser = policy.EntryUser,
                                    ExtensionTypeId = policy.ExtensionTypeId,
                                    HrsUsedAtPolicySale = policy.HrsUsedAtPolicySale,
                                    Id = policy.Id,
                                    IsPartialPayment = policy.IsPartialPayment,
                                    IsPreWarrantyCheck = policy.IsPreWarrantyCheck,
                                    IsSpecialDeal = policy.IsSpecialDeal,
                                    ItemId = item.BAndWId,
                                    ProductId = policy.ProductId,
                                    PaymentModeId = policy.PaymentModeId,
                                    PaymentTypeId = policy.PaymentTypeId,
                                    PolicyNo = policy.PolicyNo,
                                    PolicySoldDate = policy.PolicySoldDate,
                                    Premium = policy.Premium,
                                    RefNo = policy.RefNo,
                                    SalesPersonId = policy.SalesPersonId,
                                    Type = "B&W",
                                    IsApproved = policy.IsApproved,
                                    IsPolicyCanceled = policy.IsPolicyCanceled,
                                    PolicyStartDate = policy.PolicyStartDate,
                                    PolicyEndDate = policy.PolicyEndDate,
                                    Discount = policy.Discount,
                                    PolicyBundleId = policy.PolicyBundleId,
                                    ForwardComment = policy.ForwardComment,
                                    Year = policy.Year,
                                    Month = policy.Month,
                                    BordxId = policy.BordxId,
                                    DealerPolicy = policy.DealerPolicy
                                });
                            }
                        }
                    }
                    if (OPolicyData.ToList().Count > 0)
                    {
                        foreach (var item in OPolicyData)
                        {
                            Policy policy = PolicyData.FirstOrDefault(p => p.Id == item.PolicyId);
                            if (policy != null)
                            {
                                entities.Add(new PolicyInfo()
                                {
                                    Comment = policy.Comment,
                                    CommodityTypeId = policy.CommodityTypeId,
                                    ContractId = policy.ContractId,
                                    CoverTypeId = policy.CoverTypeId,
                                    CurrencyPeriodId = policy.CurrencyPeriodId,
                                    PremiumCurrencyTypeId = policy.PremiumCurrencyTypeId,
                                    DealerPaymentCurrencyTypeId = policy.DealerPaymentCurrencyTypeId,
                                    CustomerPaymentCurrencyTypeId = policy.CustomerPaymentCurrencyTypeId,
                                    CustomerId = policy.CustomerId,
                                    CustomerPayment = policy.CustomerPayment,
                                    DealerId = policy.DealerId,
                                    DealerLocationId = policy.DealerLocationId,
                                    DealerPayment = policy.DealerPayment,
                                    EntryDateTime = policy.EntryDateTime,
                                    EntryUser = policy.EntryUser,
                                    ExtensionTypeId = policy.ExtensionTypeId,
                                    HrsUsedAtPolicySale = policy.HrsUsedAtPolicySale,
                                    Id = policy.Id,
                                    IsPartialPayment = policy.IsPartialPayment,
                                    IsPreWarrantyCheck = policy.IsPreWarrantyCheck,
                                    IsSpecialDeal = policy.IsSpecialDeal,
                                    ItemId = item.OtherItemId,
                                    ProductId = policy.ProductId,
                                    PaymentTypeId = policy.PaymentTypeId,
                                    PaymentModeId = policy.PaymentModeId,
                                    PolicyNo = policy.PolicyNo,
                                    PolicySoldDate = policy.PolicySoldDate,
                                    Premium = policy.Premium,
                                    RefNo = policy.RefNo,
                                    SalesPersonId = policy.SalesPersonId,
                                    Type = "Other",
                                    IsApproved = policy.IsApproved,
                                    IsPolicyCanceled = policy.IsPolicyCanceled,
                                    PolicyStartDate = policy.PolicyStartDate,
                                    PolicyEndDate = policy.PolicyEndDate,
                                    Discount = policy.Discount,
                                    PolicyBundleId = policy.PolicyBundleId,
                                    ForwardComment = policy.ForwardComment,
                                    Year = policy.Year,
                                    Month = policy.Month,
                                    BordxId = policy.BordxId,
                                    DealerPolicy = policy.DealerPolicy,
                                    BookletNumber = policy.BookletNumber,
                                    ContractExtensionPremiumId = policy.ContractExtensionPremiumId,
                                    ContractExtensionsId = policy.ContractExtensionsId,
                                    DiscountPercentage = policy.DiscountPercentage,
                                    ContractInsuaranceLimitationId = policy.ContractInsuaranceLimitationId
                                });
                            }
                        }
                    }

                    if (YPolicyData.ToList().Count > 0)
                    {
                        foreach (var item in YPolicyData)
                        {
                            Policy policy = PolicyData.FirstOrDefault(p => p.Id == item.PolicyId);
                            if (policy != null)
                            {
                                entities.Add(new PolicyInfo()
                                {
                                    Comment = policy.Comment,
                                    CommodityTypeId = policy.CommodityTypeId,
                                    ContractId = policy.ContractId,
                                    CoverTypeId = policy.CoverTypeId,
                                    CurrencyPeriodId = policy.CurrencyPeriodId,
                                    PremiumCurrencyTypeId = policy.PremiumCurrencyTypeId,
                                    DealerPaymentCurrencyTypeId = policy.DealerPaymentCurrencyTypeId,
                                    CustomerPaymentCurrencyTypeId = policy.CustomerPaymentCurrencyTypeId,
                                    ContractInsuaranceLimitationId = policy.ContractInsuaranceLimitationId,
                                    ContractExtensionPremiumId = policy.ContractExtensionPremiumId,
                                    ContractExtensionsId = policy.ContractExtensionsId,
                                    CustomerId = policy.CustomerId,
                                    CustomerPayment = policy.CustomerPayment,
                                    DealerId = policy.DealerId,
                                    DealerLocationId = policy.DealerLocationId,
                                    DealerPayment = policy.DealerPayment,
                                    EntryDateTime = policy.EntryDateTime,
                                    EntryUser = policy.EntryUser,
                                    ExtensionTypeId = policy.ExtensionTypeId,
                                    HrsUsedAtPolicySale = policy.HrsUsedAtPolicySale,
                                    Id = policy.Id,
                                    IsPartialPayment = policy.IsPartialPayment,
                                    IsPreWarrantyCheck = policy.IsPreWarrantyCheck,
                                    IsSpecialDeal = policy.IsSpecialDeal,
                                    ItemId = item.YellowGoodId,
                                    ProductId = policy.ProductId,
                                    PaymentModeId = policy.PaymentModeId,
                                    PaymentTypeId = policy.PaymentTypeId,
                                    PolicyNo = policy.PolicyNo,
                                    PolicySoldDate = policy.PolicySoldDate,
                                    Premium = policy.Premium,
                                    RefNo = policy.RefNo,
                                    SalesPersonId = policy.SalesPersonId,
                                    Type = "Yellow",
                                    IsApproved = policy.IsApproved,
                                    IsPolicyCanceled = policy.IsPolicyCanceled,
                                    PolicyStartDate = policy.PolicyStartDate,
                                    PolicyEndDate = policy.PolicyEndDate,
                                    Discount = policy.Discount,
                                    PolicyBundleId = policy.PolicyBundleId,
                                    ForwardComment = policy.ForwardComment,
                                    Year = policy.Year,
                                    Month = policy.Month,
                                    BordxId = policy.BordxId,
                                    DealerPolicy = policy.DealerPolicy,
                                    MWStartDate = policy.MWStartDate
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ((ex.InnerException != null) ? ex.InnerException.ToString() : ""));
            }

            return entities;
        }

        internal bool AddPolicyAttachments(Guid policyBundleId, List<Guid> attachmentIds)
        {
            bool status = false;
            ISession session = EntitySessionManager.GetSession();
            using (ITransaction transaction = session.BeginTransaction())
            {
                foreach (Guid attachmentId in attachmentIds)
                {
                    PolicyAttachment policyAttachment = new PolicyAttachment
                    {
                        Id =Guid.NewGuid(),
                        PolicyBundleId = policyBundleId,
                        UserAttachmentId = attachmentId
                    };
                    session.Save(policyAttachment, policyAttachment.Id);
                }

                transaction.Commit();
                status = true;
            }
            return status;
        }

        internal List<PolicyResponseDto> GetPoliciesById(Guid itemId, string polityType)
        {
            CurrencyEntityManager cem = new CurrencyEntityManager();
            ISession session = EntitySessionManager.GetSession();
            List<PolicyResponseDto> entities = new List<PolicyResponseDto>();
            entities = session.CreateSQLQuery("exec GetPolicyDetailsByItemIdAndType :itemId,:type ")
                       .SetGuid("itemId", itemId)
                       .SetString("type", polityType)
                       .SetResultTransformer(Transformers.AliasToBean<PolicyInfo>())
                       .List<PolicyInfo>().ToList()
                       .Select(Policy => new PolicyResponseDto {
                            Id = Policy.Id,
                            tpaBranchId = Policy.TPABranchId,
                            Comment = Policy.Comment,
                            CommodityTypeId = Policy.CommodityTypeId,
                            ContractId = Policy.ContractId,
                            CoverTypeId = Policy.CoverTypeId,
                            ContractInsuaranceLimitationId = Policy.ContractInsuaranceLimitationId,
                            ContractExtensionsId = Policy.ContractExtensionsId,
                            ContractExtensionPremiumId = Policy.ContractExtensionPremiumId,
                            PremiumCurrencyTypeId = Policy.PremiumCurrencyTypeId,
                            DealerPaymentCurrencyTypeId = Policy.DealerPaymentCurrencyTypeId,
                            CustomerPaymentCurrencyTypeId = Policy.CustomerPaymentCurrencyTypeId,
                            CustomerId = Policy.CustomerId,
                            CustomerPayment = cem.ConvertFromBaseCurrency(Policy.CustomerPayment, Policy.PremiumCurrencyTypeId, Policy.CurrencyPeriodId),
                            DealerId = Policy.DealerId,
                            DealerLocationId = Policy.DealerLocationId,
                            DealerPayment = cem.ConvertFromBaseCurrency(Policy.DealerPayment, Policy.PremiumCurrencyTypeId, Policy.CurrencyPeriodId),
                            ExtensionTypeId = Policy.ExtensionTypeId,
                            HrsUsedAtPolicySale = Policy.HrsUsedAtPolicySale,
                            IsPartialPayment = Policy.IsPartialPayment,
                            IsPreWarrantyCheck = Policy.IsPreWarrantyCheck,
                            IsSpecialDeal = Policy.IsSpecialDeal,
                            ItemId = Policy.ItemId,
                            ProductId = Policy.ProductId,
                            PaymentTypeId = Policy.PaymentTypeId,
                            PaymentModeId = Policy.PaymentModeId,
                            PolicyNo = Policy.PolicyNo,
                            PolicySoldDate = Policy.PolicySoldDate,
                            Premium = cem.ConvertFromBaseCurrency(Policy.Premium, Policy.PremiumCurrencyTypeId, Policy.CurrencyPeriodId),
                            RefNo = Policy.RefNo,
                            SalesPersonId = Policy.SalesPersonId,
                            EntryDateTime = Policy.EntryDateTime,
                            EntryUser = Policy.EntryUser,
                            Type = Policy.Type,
                            IsApproved = Policy.IsApproved,
                            PolicyStartDate = Policy.PolicyStartDate,
                            PolicyEndDate = Policy.PolicyEndDate,
                            Discount = Convert.ToDecimal(Policy.DiscountPercentage),
                            //DiscountPercentage = Policy.DiscountPercentage,
                            PolicyBundleId = Policy.PolicyBundleId,
                            TransferFee = Policy.TransferFee,
                            ForwardComment = Policy.ForwardComment,
                            BordxId = Policy.BordxId,
                            Month = Policy.Month,
                            Year = Policy.Year,
                            DealerPolicy = Policy.DealerPolicy,
                            MWStartDate = Policy.MWStartDate,
                            MWIsAvailable = Policy.MWIsAvailable,
                            BookletNumber = Policy.BookletNumber,
                            DiscountPercentage = Policy.DiscountPercentage
                       }).ToList();
            return entities;

        }

        internal object GetEMIValue2(decimal loneAmount, Guid contractId)
        {
            throw new NotImplementedException();
        }

        public  object GetEMIValue(decimal loneAmount ,Guid contractId)
        {
            object Response = null;
            try
            {
                ISession session = EntitySessionManager.GetSession();
                Contract contract = session.Query<Contract>().Where(a => a.Id == contractId).FirstOrDefault();
                ContractInsuaranceLimitation contractInsuaranceLimitation = session.Query<ContractInsuaranceLimitation>()
                    .Where(a => a.ContractId == contractId).FirstOrDefault();
                InsuaranceLimitation insuaranceLimitation = session.Query<InsuaranceLimitation>()
                    .Where(a => a.Id == contractInsuaranceLimitation.InsuaranceLimitationId).FirstOrDefault();

                decimal loanamt = loneAmount; // loanAmount
                decimal intrest = contract.AnnualInterestRate;
                decimal repaytrm = insuaranceLimitation.Months;

                //EMI calculation logic
                decimal rate1 = ((intrest) / 100) / 12;
                decimal rate = 1 + rate1;
                Double interestRate = Math.Pow(Convert.ToDouble(rate), Convert.ToDouble(repaytrm));
                decimal E1 = loanamt * rate1 * Convert.ToDecimal(interestRate);
                decimal E2 = Convert.ToDecimal(interestRate) - 1;
                decimal EMI = (E1 / E2);
                var total_payable = EMI * repaytrm;
                var total_interest = (total_payable - loanamt);

                Response = EMI;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        internal static object DownloadPolicyStatementforTYER(Guid policyId, string tpaName, string dbConnectionString)
        {
            object response = null;
            try
            {
                bool isExist = File.Exists(
                    System.Web.HttpContext.Current.Server.MapPath(
                        ConfigurationData.ReportsPath + "\\PolicyStatement\\" + tpaName.ToLower() + "\\PolicyStatementContinental.sql")
                    );
                string ReportLocation = isExist ? tpaName.ToLower() : "Default";

                ISession session = EntitySessionManager.GetSession();

                String Query = File.ReadAllText(
                    System.Web.HttpContext.Current.Server.MapPath(
                        ConfigurationData.ReportsPath +
                        "\\PolicyStatement\\" + ReportLocation + "\\PolicyStatementContinental.sql"));
                Query = Query

                    .Replace("{policyId}", policyId.ToString());

                //putting in the db so report viewer can view and read it

                string reportKey = Guid.NewGuid().ToString();
                ReportDataQuery ReportDataQuery = new ReportDataQuery()
                {
                    Id = Guid.NewGuid(),
                    ReportKey = Guid.Parse(reportKey),
                    ReportCode = "PolicyStatementContinental",
                    ReportDbConnStr = dbConnectionString,
                    ReportQuery = Query,
                    ReportDirectory = ConfigurationData.ReportsPath + "\\PolicyStatement\\" + ReportLocation
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
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ((ex.InnerException != null) ? ex.InnerException.ToString() : ""));
            }
            return response;
        }

        internal static TyreCustomerEnterdPolicySaveResponse SaveCustomerEnterdPolicyByTpaId2(TyrePolicySaveResponse tyrePolicySaveResponse, Customer_data customer_Data, Guid tpaId, Guid dealerBranch , List<ContractDetails> contractDetails)
        {
            TyreCustomerEnterdPolicySaveResponse response = new TyreCustomerEnterdPolicySaveResponse();
            response.code = "ERROR";
            try
            {
                if (tyrePolicySaveResponse == null || customer_Data == null)
                {
                    response.msg = "Request data is invalid.";
                    return response;
                }
                if (!IsGuid(tpaId.ToString()))
                {
                    response.msg = "Invalid TPA selection.";
                    return response;
                }
                if (customer_Data == null)
                {
                    response.msg = "Customer data is invalid.";
                    return response;
                }

                ISession session = EntitySessionManager.GetSession();

                //validate invoice code
                CustomerEnterdInvoiceDetails customerEnterdInvoiceDetails = session.Query<CustomerEnterdInvoiceDetails>()
                .FirstOrDefault(a => a.Id == tyrePolicySaveResponse.tempInvId);

                InvoiceCode invoiceCode = session.Query<InvoiceCode>()
                    .FirstOrDefault(a => a.Id == customerEnterdInvoiceDetails.InvoiceCodeId);

                List<InvoiceCodeDetails> invoiceCodeDetails = session.Query<InvoiceCodeDetails>().Where(a => a.InvoiceCodeId == invoiceCode.Id).ToList();

                if (invoiceCodeDetails.FirstOrDefault().IsPolicyCreated)
                {
                    response.msg = "A policy already associated with invoice code.";
                    return response;
                }

                // customer already exist validation with email removed old codes
                //bool isExistingCustomer = false;
                //if (IsGuid(customer_Data.customerId.ToString()))
                //{
                //    isExistingCustomer = true;
                //}
                //Customer customer = new Customer();
                //if (!isExistingCustomer)
                //{
                //    //email validation
                //    Customer customer_exist = session.Query<Customer>()
                //        .FirstOrDefault(a => a.Email.ToLower() == customer_Data.email.ToLower());
                //    if (customer_exist != null)
                //    {
                //        customer_Data.customerId = customer_exist.Id;
                //        //response.msg = "Customer email already exist. Please login.";
                //        //return response;
                //    }
                //    else
                //    {
                //        customer = DBDTOTransformer.Instance.CustomerEnterdTirePolicyDetailsCustomer(customer_Data);
                //    }

                //    if (customer_Data.dateOfBirth <= SqlDateTime.MinValue.Value)
                //    {
                //        customer_Data.dateOfBirth = SqlDateTime.MinValue.Value;
                //    }


                //}
                // end  customer already exist validation with email removed old codes

                Customer customer = new Customer();
                customer = DBDTOTransformer.Instance.CustomerEnterdTirePolicyDetailsCustomer(customer_Data);
                if (customer != null && IsGuid(customer.Id.ToString()))
                {
                    customer_Data.customerId = customer.Id;
                }

                //policy bundle
                PolicyBundle policyBundle = DBDTOTransformer.Instance.CustomerEnterdTirePolicyDetailsToPolicyBundle2(customer_Data, tyrePolicySaveResponse, tpaId);
                //policy
                List<Policy> policies = DBDTOTransformer.Instance.CustomerEnterdTirePolicyDetailsToPolicyList2(customer_Data, tyrePolicySaveResponse, tpaId, policyBundle.Id, contractDetails);
                //other item
                List<OtherItemDetails> otherItemDetails = DBDTOTransformer.Instance.CustomerEnterdTirePolicyDetailsToOtherItemDetails2(customer_Data, tyrePolicySaveResponse, tpaId, policyBundle.Id , contractDetails);
                //other item policy
                List<OtherItemPolicy> otherItemPolicyDetails = DBDTOTransformer.Instance.CustomerEnterdTirePolicyDetailsToOtherItemPolicyDetails(policies, otherItemDetails);
                //policyAttachment
               // PolicyAttachment policyAttachment = DBDTOTransformer.Instance.CustomerEnterdTirePolicyDetailsToPolicyAttachmentDetails(saveCustomerEnterdPolicyRequest, policyBundle.Id);


                var policyNumbers = string.Empty;
                List<string> policyNumbersList = new List<string>();
                using (ITransaction transaction = session.BeginTransaction())
                {
                    if (customer.IDTypeId == 0)
                    {
                        customer.IDTypeId = 1;
                    }

                    if (customer != null && IsGuid(customer.Id.ToString()))
                    {
                        // customer.CustomerTypeId = Convert.ToInt16("2");
                        session.Save(customer, customer.Id);
                    }

                    policyBundle.MWStartDate = SqlDateTime.MinValue.Value;
                    policyBundle.PolicySoldDate = SqlDateTime.MinValue.Value;

                    session.Save(policyBundle, policyBundle.Id);
                    int counter = 0; string generatedFirstPolicyNumber = string.Empty;
                    foreach (Policy policy in policies)
                    {
                        counter++;
                        var policyNum = GetNextPolicyNumberForTyre(dealerBranch, policy.DealerId,
                           policy.ProductId, new CommonEntityManager().GetDealerCountryByDealerId(policy.DealerId), policyBundle.CommodityTypeId, tpaId);
                        //trick for get next policy number since stored procedures not working properly inside transaction
                        if (counter == 1)
                        {
                            generatedFirstPolicyNumber = policyNum;
                        }

                        if (generatedFirstPolicyNumber == policyNum && counter != 1)
                        {
                            policyNum = GetNextPolicyNumberPlusCounterForTyre(counter, dealerBranch, policy.DealerId,
                                policy.ProductId, new CommonEntityManager().GetDealerCountryByDealerId(policy.DealerId));
                        }


                        policy.PolicyNo = policyNum;
                        policyNumbersList.Add(policyNum);
                        policyNumbers += policyNum + "<br>" + (policyNumbers.Length > 0 ? "," : string.Empty);
                        session.Evict(policy);
                        session.Save(policy, policy.Id);


                    }

                    int count = 0;
                    //update invoiceCodeDetails
                    foreach (var tiredetails in invoiceCodeDetails)
                    {
                        count++;
                        if (count == 1)
                        {
                            tiredetails.IsPolicyCreated = true;
                            tiredetails.PolicyId = policies.FirstOrDefault().Id;

                            session.Evict(tiredetails);
                            session.Update(tiredetails);
                        }
                        else
                        {
                            tiredetails.IsPolicyCreated = true;
                            tiredetails.PolicyId = policies.LastOrDefault().Id;

                            session.Evict(tiredetails);
                            session.Update(tiredetails);
                        }



                    }

                   // session.Save(policyAttachment, policyAttachment.Id);

                    foreach (OtherItemDetails otherItem in otherItemDetails)
                    {
                        session.Evict(otherItem);
                        session.Save(otherItem, otherItem.Id);
                    }

                    foreach (OtherItemPolicy otherItemPolicy in otherItemPolicyDetails)
                    {
                        session.Evict(otherItemPolicy);
                        session.Save(otherItemPolicy, otherItemPolicy.Id);
                    }



                    transaction.Commit();
                }

                response.code = "SUCCESS";
                response.msg = "Your details are submitted for approval,<br> Email is sent for your reference.";
                try
                {

                    //email sending

                    //statement and confrimatim
                    List<string> toEmailList = new List<string>();
                    toEmailList.Add(customer_Data.email);
                    string policyNumberCommaSeparated = string.Join(",", policyNumbersList);
                    new GetMyEmail().TyreSalesSubmitComfirmation(toEmailList, customer_Data.firstName, policyNumberCommaSeparated);
                    int month = DateTime.UtcNow.Month;
                    int year = DateTime.UtcNow.Year;

                    double days = DateTime.DaysInMonth(year, month);
                    double NoofdatesinPolicy = (DateTime.UtcNow - invoiceCode.PurcheasedDate).TotalDays;
                    if (NoofdatesinPolicy > days)
                    {
                        TPA tpa = session.Query<TPA>().FirstOrDefault(a => a.Id == tpaId);
                        var logo = new ImageEntityManager().GetImageBase64ById(tpa.Logo);
                        var bytes = Convert.FromBase64String(logo);
                        Stream stream = new MemoryStream(bytes);
                        new GetMyEmail().RejectionEmail(toEmailList, customer.FirstName, "", customer.Id, stream);
                    }



                }
                catch (Exception)
                { }
            }
            catch (DealNotFoundException)
            {
                response.msg = "No deals found for enterd tire details. Please contact administrator.";
            }
            catch (Exception ex)
            {
                response.msg = "Error occured while saving invoice details.";
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ((ex.InnerException != null) ? ex.InnerException.ToString() : ""));
            }
            return response;
        }

        internal static object SaveCustomerEnterdPolicyByTpaId(SaveCustomerEnterdPolicyRequestDto saveCustomerEnterdPolicyRequest)
        {
            //save
            GenericCodeMsgResponse response = new GenericCodeMsgResponse();
            response.code = "ERROR";
            try
            {
                if (saveCustomerEnterdPolicyRequest == null || saveCustomerEnterdPolicyRequest.data == null)
                {
                    response.msg = "Request data is invalid.";
                    return response;
                }
                if (!IsGuid(saveCustomerEnterdPolicyRequest.tpaId.ToString()))
                {
                    response.msg = "Invalid TPA selection.";
                    return response;
                }
                if (saveCustomerEnterdPolicyRequest.data.customer == null)
                {
                    response.msg = "Customer data is invalid.";
                    return response;
                }

                ISession session = EntitySessionManager.GetSession();

                //validate invoice code
                CustomerEnterdInvoiceDetails customerEnterdInvoiceDetails = session.Query<CustomerEnterdInvoiceDetails>()
                .FirstOrDefault(a => a.Id == saveCustomerEnterdPolicyRequest.data.tempInvId);

                InvoiceCode invoiceCode = session.Query<InvoiceCode>()
                    .FirstOrDefault(a => a.Id == customerEnterdInvoiceDetails.InvoiceCodeId);

                List<InvoiceCodeDetails> invoiceCodeDetails = session.Query<InvoiceCodeDetails>().Where(a => a.InvoiceCodeId == invoiceCode.Id).ToList();

                if (invoiceCodeDetails.FirstOrDefault().IsPolicyCreated)
                {
                    response.msg = "A policy already associated with invoice code.";
                    return response;
                }


                bool isExistingCustomer = false;
                if (IsGuid(saveCustomerEnterdPolicyRequest.data.customer.customerId.ToString()))
                {
                    isExistingCustomer = true;
                }
                Customer customer = new Customer();
                if (!isExistingCustomer)
                {
                    //email validation
                    Customer customer_exist = session.Query<Customer>()
                        .FirstOrDefault(a => a.Email.ToLower() == saveCustomerEnterdPolicyRequest.data.customer.email.ToLower());
                    if (customer_exist != null)
                    {
                        response.msg = "Customer email already exist. Please login.";
                        return response;
                    }

                    if (saveCustomerEnterdPolicyRequest.data.customer.dateOfBirth <= SqlDateTime.MinValue.Value)
                    {
                        saveCustomerEnterdPolicyRequest.data.customer.dateOfBirth = SqlDateTime.MinValue.Value;
                    }
                    customer = DBDTOTransformer.Instance.CustomerEnterdTirePolicyDetailsToCustomer(saveCustomerEnterdPolicyRequest);

                }


                if (customer != null && IsGuid(customer.Id.ToString()))
                {
                    saveCustomerEnterdPolicyRequest.data.customer.customerId = customer.Id;
                }

                //policy bundle
                PolicyBundle policyBundle = DBDTOTransformer.Instance.CustomerEnterdTirePolicyDetailsToPolicyBundle(saveCustomerEnterdPolicyRequest);
                //policy
                List<Policy> policies = DBDTOTransformer.Instance.CustomerEnterdTirePolicyDetailsToPolicyList(saveCustomerEnterdPolicyRequest, policyBundle.Id);
                //other item
                List<OtherItemDetails> otherItemDetails = DBDTOTransformer.Instance.CustomerEnterdTirePolicyDetailsToOtherItemDetails(saveCustomerEnterdPolicyRequest, policyBundle.Id);
                //other item policy
                List<OtherItemPolicy> otherItemPolicyDetails = DBDTOTransformer.Instance.CustomerEnterdTirePolicyDetailsToOtherItemPolicyDetails(policies, otherItemDetails);
                //policyAttachment
                PolicyAttachment policyAttachment = DBDTOTransformer.Instance.CustomerEnterdTirePolicyDetailsToPolicyAttachmentDetails(saveCustomerEnterdPolicyRequest, policyBundle.Id);


                var policyNumbers = string.Empty;

                using (ITransaction transaction = session.BeginTransaction())
                {
                    if (customer.IDTypeId == 0)
                    {
                        customer.IDTypeId = 1;
                    }

                    if (customer != null && IsGuid(customer.Id.ToString()))
                    {
                        session.Save(customer, customer.Id);
                    }

                    policyBundle.MWStartDate = SqlDateTime.MinValue.Value;
                    policyBundle.PolicySoldDate = SqlDateTime.MinValue.Value;

                    session.Save(policyBundle, policyBundle.Id);
                    int counter = 0; string generatedFirstPolicyNumber = string.Empty;
                    foreach (Policy policy in policies)
                    {
                        counter++;
                        var policyNum = GetNextPolicyNumber(policy.TPABranchId, policy.DealerId,
                           policy.ProductId, policyBundle.CommodityTypeId, saveCustomerEnterdPolicyRequest.tpaId, new CommonEntityManager().GetDealerCountryByDealerId(policy.DealerId));
                        //trick for get next policy number since stored procedures not working properly inside transaction
                        if (counter == 1)
                        {
                            generatedFirstPolicyNumber = policyNum;
                        }

                        if (generatedFirstPolicyNumber == policyNum && counter != 1)
                        {
                            policyNum = GetNextPolicyNumberPlusCounter(counter, policy.TPABranchId, policy.DealerId,
                                policy.ProductId, new CommonEntityManager().GetDealerCountryByDealerId(policy.DealerId));
                        }


                        policy.PolicyNo = policyNum;
                        policyNumbers += policyNum + "<br>" + (policyNumbers.Length > 0 ? "," : string.Empty);
                        session.Evict(policy);
                        session.Save(policy, policy.Id);


                    }

                    int count = 0;
                    //update invoiceCodeDetails
                    foreach (var tiredetails in invoiceCodeDetails)
                    {
                        count++;
                        if (count == 1)
                        {
                            tiredetails.IsPolicyCreated = true;
                            tiredetails.PolicyId = policies.FirstOrDefault().Id;

                            session.Evict(tiredetails);
                            session.Update(tiredetails);
                        }
                        else
                        {
                            tiredetails.IsPolicyCreated = true;
                            tiredetails.PolicyId = policies.LastOrDefault().Id;

                            session.Evict(tiredetails);
                            session.Update(tiredetails);
                        }



                    }

                    session.Save(policyAttachment, policyAttachment.Id);

                    foreach (OtherItemDetails otherItem in otherItemDetails)
                    {
                        session.Evict(otherItem);
                        session.Save(otherItem, otherItem.Id);
                    }

                    foreach (OtherItemPolicy otherItemPolicy in otherItemPolicyDetails)
                    {
                        session.Evict(otherItemPolicy);
                        session.Save(otherItemPolicy, otherItemPolicy.Id);
                    }



                    transaction.Commit();
                }

                response.code = "SUCCESS";
                response.msg = "Congratulations!<br> " +
                    "Your policy number(s) is " +
                   policyNumbers
                    + "<br>We will email your policy details once it is approved.<br><br> " +
                    "If you have any further questions please contact us at support@leftfieldassurance.com.";
                try
                {
                    //email sending
                    List<string> toEmailList = new List<string>();
                    toEmailList.Add(customer.Email);
                    TPA tpa = session.Query<TPA>().FirstOrDefault(a => a.Id == saveCustomerEnterdPolicyRequest.tpaId);
                    var logo = new ImageEntityManager().GetImageBase64ById(tpa.Logo);
                    var bytes = Convert.FromBase64String(logo);
                    Stream stream = new MemoryStream(bytes);
                    //new GetMyEmail().CustomerRegistrationConfirmEmail(toEmailList, customer.FirstName, "" ,customer.Id, stream);

                    int month = DateTime.UtcNow.Month;
                    int year = DateTime.UtcNow.Year;

                    double days = DateTime.DaysInMonth(year, month);
                    double NoofdatesinPolicy = (DateTime.UtcNow - invoiceCode.PurcheasedDate).TotalDays;
                    if (NoofdatesinPolicy > days)
                    {
                        new GetMyEmail().RejectionEmail(toEmailList, customer.FirstName, "", customer.Id, stream);
                    }

                    //statement and confrimatim

                }
                catch (Exception)
                { }
            }
            catch (DealNotFoundException)
            {
                response.msg = "No deals found for enterd tire details. Please contact administrator.";
            }
            catch (Exception ex)
            {
                response.msg = "Error occured while saving invoice details.";
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ((ex.InnerException != null) ? ex.InnerException.ToString() : ""));
            }
            return response;
        }


        internal static object LoadSavedCustomerInvoiceDataById(Guid tempInvoiceId)
        {
            SavedCustomerInvoiceDataResponseDto response = new SavedCustomerInvoiceDataResponseDto();
            try
            {
                ISession session = EntitySessionManager.GetSession();

                CustomerEnterdInvoiceDetails customerEnterdInvoiceDetails = session.Query<CustomerEnterdInvoiceDetails>()
                    .FirstOrDefault(a => a.Id == tempInvoiceId);
                if (customerEnterdInvoiceDetails != null)
                {
                    List<CustomerEnterdInvoiceTireDetails> customerEnterdInvoiceTireDetailsList
                        = session.Query<CustomerEnterdInvoiceTireDetails>()
                        .Where(a => a.CustomerEnterdInvoiceId == customerEnterdInvoiceDetails.Id).ToList();

                    var availaboeTireDetails = new List<AvailableTireList>();
                    foreach (CustomerEnterdInvoiceTireDetails tireDtl in customerEnterdInvoiceTireDetailsList)
                    {
                        var availableTire = new AvailableTireList()
                        {
                            id = tireDtl.InvoiceCodeTireDetailId,
                            price = Math.Round(tireDtl.PurchasedPrice * tireDtl.ConversionRate * 100) / 100
                        };
                        availaboeTireDetails.Add(availableTire);
                    }

                    response = new SavedCustomerInvoiceDataResponseDto()
                    {
                        AdditionalMakeId = customerEnterdInvoiceDetails.AdditionalDetailsMakeId,
                        AdditionalMileage = customerEnterdInvoiceDetails.AdditionalDetailsMileage,
                        AdditionalModelId = customerEnterdInvoiceDetails.AdditionalDetailsModelId,
                        AdditionalModelYearId = customerEnterdInvoiceDetails.AdditionalDetailsModelYear,
                        InvoiceAttachmentId = customerEnterdInvoiceDetails.InvoiceAttachmentId,
                        InvoiceCode = customerEnterdInvoiceDetails.InvoiceCode,
                        InvoiceNumber = customerEnterdInvoiceDetails.InvoiceNumber,
                        UsageTypeCode = customerEnterdInvoiceDetails.UsageTypeCode,
                        availableTireList = availaboeTireDetails

                    };
                }

            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ((ex.InnerException != null) ? ex.InnerException.ToString() : ""));
            }
            return response;
        }


        internal static TyrePolicySaveResponse SaveCustomerEnteredInvoiceDetails2(TyreProduct product,Guid tpaId,Guid tempInvId, Guid customerId)
        {
            TyrePolicySaveResponse response = new TyrePolicySaveResponse();
            response.code = "ERROR";

            try
            {
                #region validation
                if (product == null)
                {
                    response.msg = "Request data is empty.";
                    return response;
                }

                if (!IsGuid(tpaId.ToString()) ||  product == null)
                {
                    response.msg = "Request data is invalid.";
                    return response;
                }

                #endregion


                ISession session = EntitySessionManager.GetSession();
                if (IsGuid(customerId.ToString()))
                {
                    //already registerd customer
                }
                else
                {
                    //new customer and need to save temp details until he enter the next step
                    //check invoice code validity
                    InvoiceCode invoiceCode = session.Query<InvoiceCode>()
                        .FirstOrDefault(a => a.Code.ToLower() == product.invoiceCode.ToLower());
                    List<InvoiceCodeDetails> invoiceCodeDetails = session.Query<InvoiceCodeDetails>().Where(a => a.InvoiceCodeId == invoiceCode.Id).ToList();
                    if (invoiceCode == null)
                    {
                        response.msg = "Invoice code is invalid.";
                        return response;
                    }

                    if (invoiceCodeDetails.FirstOrDefault().IsPolicyCreated)
                    {
                        response.msg = "Invoice code already associated with a policy.";
                        return response;
                    }

                    //time period validity
                    var lastDateOfRegistration = invoiceCode.GeneratedDate.AddDays(ConfigurationData.TireInvoiceCodeValidityPeriodDays);
                    if (lastDateOfRegistration.Date < DateTime.UtcNow.Date)
                    {
                        response.msg = "Policy registration allowed time period has elapsed.";
                        return response;
                    }

                    CurrencyEntityManager currencyEntityManager = new CurrencyEntityManager();
                    //currency details validation
                    Dealer dealer = session.Query<Dealer>().FirstOrDefault(a => a.Id == invoiceCode.DealerId);
                    Guid currentPeriodId = currencyEntityManager.GetCurrentCurrencyPeriodId();
                    if (dealer == null)
                    {
                        response.msg = "Dealer is invalid.";
                        return response;
                    }
                    if (currentPeriodId == Guid.Empty)
                    {
                        response.msg = "Currency period is not set.";
                        return response;
                    }

                    decimal conversionRate = decimal.Zero;
                    try
                    {
                        conversionRate = currencyEntityManager.GetConversionRate(dealer.CurrencyId, currentPeriodId, true);
                    }
                    catch (Exception)
                    {
                        response.msg = "Currency conversion is not found in current conversion period.";
                        return response;
                    }
                    //checking reloaded invoice data , duplicate invoice code entry and remove them
                    CustomerEnterdInvoiceDetails oldCustomerEnterdInvoiceDetails = null;
                    List<CustomerEnterdInvoiceTireDetails> oldCustomerEnterdInvoiceTireDetailsList = null;
                    if (IsGuid(tempInvId.ToString()))
                    {
                        oldCustomerEnterdInvoiceDetails = session.Query<CustomerEnterdInvoiceDetails>()
                            .FirstOrDefault(a => a.Id == tempInvId);
                        oldCustomerEnterdInvoiceTireDetailsList = session.Query<CustomerEnterdInvoiceTireDetails>()
                            .Where(a => a.CustomerEnterdInvoiceId == tempInvId).ToList();
                    }
                    else
                    {
                        oldCustomerEnterdInvoiceDetails = session.Query<CustomerEnterdInvoiceDetails>()
                            .FirstOrDefault(a => a.InvoiceCode.ToLower() == product.invoiceCode.ToLower());
                        if (oldCustomerEnterdInvoiceDetails != null && oldCustomerEnterdInvoiceDetails.Id != Guid.Empty)
                        {
                            oldCustomerEnterdInvoiceTireDetailsList = session.Query<CustomerEnterdInvoiceTireDetails>()
                               .Where(a => a.CustomerEnterdInvoiceId == oldCustomerEnterdInvoiceDetails.Id).ToList();
                        }
                    }
                    //good to go
                    Guid customerEnterdInvoiceDetailsId = Guid.NewGuid();
                    CustomerEnterdInvoiceDetails customerEnterdInvoiceDetails = new CustomerEnterdInvoiceDetails()
                    {
                        AdditionalDetailsMakeId = product.addMakeId,
                        AdditionalDetailsModelId = product.addModelId,
                        AdditionalDetailsMileage = product.addMileage,
                        AdditionalDetailsModelYear = product.addModelYear,
                        GeneratedDateTime = DateTime.UtcNow,
                        Id = customerEnterdInvoiceDetailsId,
                        InvoiceAttachmentId = product.invoiceAttachmentId,
                        InvoiceCode = product.invoiceCode,
                        InvoiceCodeId = invoiceCode.Id,
                        InvoiceNumber = product.invoiceNo,
                        UsageTypeCode = product.commodityUsageType
                    };
                    List<CustomerEnterdInvoiceTireDetails> customerEnterdInvoiceTireDetailsList = new List<CustomerEnterdInvoiceTireDetails>();
                    //foreach (var tireData in saveCustomerPolicyInfoRequestDto.availableTireList)
                    foreach (var tireData in invoiceCodeDetails)
                    {
                        List<InvoiceCodeTireDetails> invoiceCodeTireDetailslist = session.Query<InvoiceCodeTireDetails>()
                            .Where(a => a.InvoiceCodeDetailId == tireData.Id).ToList();
                        //InvoiceCodeTireDetails invoiceCodeTire = session.Query<InvoiceCodeTireDetails>()
                        //    .FirstOrDefault(a => a.InvoiceCodeDetailId == tireData.Id);
                        foreach(var invCodeTireDetailslist in invoiceCodeTireDetailslist)
                        {
                            var invTireDetail = new CustomerEnterdInvoiceTireDetails()
                            {
                                Id = Guid.NewGuid(),
                                // PurchasedPrice = Convert.ToDecimal("200")  / conversionRate, // hard coded
                                PurchasedPrice = invCodeTireDetailslist.Price / conversionRate, // Update Price With Frontend Entering Price
                                ConversionRate = conversionRate,
                                CurrencyId = dealer.CurrencyId,
                                CurrencyPeriodId = currentPeriodId,
                                CustomerEnterdInvoiceId = customerEnterdInvoiceDetailsId,
                                InvoiceCodeTireDetailId = invCodeTireDetailslist.Id,
                                TirePositionCode = invCodeTireDetailslist.Position,

                            };
                            customerEnterdInvoiceTireDetailsList.Add(invTireDetail);
                        }


                    }

                    using (ITransaction transaction = session.BeginTransaction(IsolationLevel.ReadCommitted))
                    {
                        session.Save(customerEnterdInvoiceDetails, customerEnterdInvoiceDetails.Id);
                        foreach (CustomerEnterdInvoiceTireDetails customerInvTireDetail in customerEnterdInvoiceTireDetailsList)
                        {
                            session.Evict(customerInvTireDetail);
                            session.Save(customerInvTireDetail, customerInvTireDetail.Id);
                        }

                        //remove previous record for update purpose
                        if (oldCustomerEnterdInvoiceDetails != null)
                        {
                            session.Evict(oldCustomerEnterdInvoiceDetails);
                            session.Delete(oldCustomerEnterdInvoiceDetails);

                            foreach (CustomerEnterdInvoiceTireDetails customerTireDetails in oldCustomerEnterdInvoiceTireDetailsList)
                            {
                                session.Evict(customerTireDetails);
                                session.Delete(customerTireDetails);
                            }
                        }


                        transaction.Commit();
                    }

                    //success
                    response.tempInvId = customerEnterdInvoiceDetailsId ;
                    response.msg = customerEnterdInvoiceDetailsId.ToString();

                }
            }
            catch (Exception ex)
            {
                response.msg = "Error occured while saving invoice details.";
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ((ex.InnerException != null) ? ex.InnerException.ToString() : ""));
            }

            return response;
        }

        internal static object SaveCustomerEnteredInvoiceDetails(SaveCustomerPolicyInfoRequestDto saveCustomerPolicyInfoRequestDto)
        {
            GenericCodeMsgObjResponse response = new GenericCodeMsgObjResponse();
            response.code = "ERROR";
            try
            {
                #region validation
                if (saveCustomerPolicyInfoRequestDto == null)
                {
                    response.msg = "Request data is empty.";
                    return response;
                }

                if (!IsGuid(saveCustomerPolicyInfoRequestDto.tpaId.ToString()) || saveCustomerPolicyInfoRequestDto.availableTireList == null
                    || saveCustomerPolicyInfoRequestDto.product == null)
                {
                    response.msg = "Request data is invalid.";
                    return response;
                }

                #endregion
                ISession session = EntitySessionManager.GetSession();
                if (IsGuid(saveCustomerPolicyInfoRequestDto.customerId.ToString()))
                {
                    //already registerd customer
                }
                else
                {
                    //new customer and need to save temp details until he enter the next step
                    //check invoice code validity
                    InvoiceCode invoiceCode = session.Query<InvoiceCode>()
                        .FirstOrDefault(a => a.Code.ToLower() == saveCustomerPolicyInfoRequestDto.product.invoiceCode.ToLower());
                    List<InvoiceCodeDetails> invoiceCodeDetails = session.Query<InvoiceCodeDetails>().Where(a => a.InvoiceCodeId == invoiceCode.Id).ToList();
                    if (invoiceCode == null)
                    {
                        response.msg = "Invoice code is invalid.";
                        return response;
                    }

                    if (invoiceCodeDetails.FirstOrDefault().IsPolicyCreated)
                    {
                        response.msg = "Invoice code already associated with a policy.";
                        return response;
                    }

                    //time period validity
                    var lastDateOfRegistration = invoiceCode.GeneratedDate.AddDays(ConfigurationData.TireInvoiceCodeValidityPeriodDays);
                    if (lastDateOfRegistration.Date < DateTime.UtcNow.Date)
                    {
                        response.msg = "Policy registration allowed time period has elapsed.";
                        return response;
                    }

                    CurrencyEntityManager currencyEntityManager = new CurrencyEntityManager();
                    //currency details validation
                    Dealer dealer = session.Query<Dealer>().FirstOrDefault(a => a.Id == invoiceCode.DealerId);
                    Guid currentPeriodId = currencyEntityManager.GetCurrentCurrencyPeriodId();
                    if (dealer == null)
                    {
                        response.msg = "Dealer is invalid.";
                        return response;
                    }
                    if (currentPeriodId == Guid.Empty)
                    {
                        response.msg = "Currency period is not set.";
                        return response;
                    }

                    decimal conversionRate = decimal.Zero;
                    try
                    {
                        conversionRate = currencyEntityManager.GetConversionRate(dealer.CurrencyId, currentPeriodId, true);
                    }
                    catch (Exception)
                    {
                        response.msg = "Currency conversion is not found in current conversion period.";
                        return response;
                    }
                    //checking reloaded invoice data , duplicate invoice code entry and remove them
                    CustomerEnterdInvoiceDetails oldCustomerEnterdInvoiceDetails = null;
                    List<CustomerEnterdInvoiceTireDetails> oldCustomerEnterdInvoiceTireDetailsList = null;
                    if (IsGuid(saveCustomerPolicyInfoRequestDto.tempInvId.ToString()))
                    {
                        oldCustomerEnterdInvoiceDetails = session.Query<CustomerEnterdInvoiceDetails>()
                            .FirstOrDefault(a => a.Id == saveCustomerPolicyInfoRequestDto.tempInvId);
                        oldCustomerEnterdInvoiceTireDetailsList = session.Query<CustomerEnterdInvoiceTireDetails>()
                            .Where(a => a.CustomerEnterdInvoiceId == saveCustomerPolicyInfoRequestDto.tempInvId).ToList();
                    }
                    else
                    {
                        oldCustomerEnterdInvoiceDetails = session.Query<CustomerEnterdInvoiceDetails>()
                            .FirstOrDefault(a => a.InvoiceCode.ToLower() == saveCustomerPolicyInfoRequestDto.product.invoiceCode.ToLower());
                        if (oldCustomerEnterdInvoiceDetails != null && oldCustomerEnterdInvoiceDetails.Id != Guid.Empty)
                        {
                            oldCustomerEnterdInvoiceTireDetailsList = session.Query<CustomerEnterdInvoiceTireDetails>()
                               .Where(a => a.CustomerEnterdInvoiceId == oldCustomerEnterdInvoiceDetails.Id).ToList();
                        }
                    }
                    //good to go
                    Guid customerEnterdInvoiceDetailsId = Guid.NewGuid();
                    CustomerEnterdInvoiceDetails customerEnterdInvoiceDetails = new CustomerEnterdInvoiceDetails()
                    {
                        AdditionalDetailsMakeId = saveCustomerPolicyInfoRequestDto.product.addMakeId,
                        AdditionalDetailsModelId = saveCustomerPolicyInfoRequestDto.product.addModelId,
                        AdditionalDetailsMileage = saveCustomerPolicyInfoRequestDto.product.addMileage,
                        AdditionalDetailsModelYear = saveCustomerPolicyInfoRequestDto.product.addModelYear,
                        GeneratedDateTime = DateTime.UtcNow,
                        Id = customerEnterdInvoiceDetailsId,
                        InvoiceAttachmentId = saveCustomerPolicyInfoRequestDto.product.invoiceAttachmentId,
                        InvoiceCode = saveCustomerPolicyInfoRequestDto.product.invoiceCode,
                        InvoiceCodeId = invoiceCode.Id,
                        InvoiceNumber = saveCustomerPolicyInfoRequestDto.product.invoiceNo,
                        UsageTypeCode = saveCustomerPolicyInfoRequestDto.product.commodityUsageType
                    };
                    List<CustomerEnterdInvoiceTireDetails> customerEnterdInvoiceTireDetailsList = new List<CustomerEnterdInvoiceTireDetails>();
                    foreach (var tireData in saveCustomerPolicyInfoRequestDto.availableTireList)
                    {
                        InvoiceCodeTireDetails invoiceCodeTire = session.Query<InvoiceCodeTireDetails>()
                            .FirstOrDefault(a => a.Id == tireData.id);
                        var invTireDetail = new CustomerEnterdInvoiceTireDetails()
                        {
                            Id = Guid.NewGuid(),
                            PurchasedPrice = tireData.price / conversionRate,
                            ConversionRate = conversionRate,
                            CurrencyId = dealer.CurrencyId,
                            CurrencyPeriodId = currentPeriodId,
                            CustomerEnterdInvoiceId = customerEnterdInvoiceDetailsId,
                            InvoiceCodeTireDetailId = tireData.id,
                            TirePositionCode = invoiceCodeTire.Position
                        };
                        customerEnterdInvoiceTireDetailsList.Add(invTireDetail);
                    }

                    using (ITransaction transaction = session.BeginTransaction(IsolationLevel.ReadCommitted))
                    {
                        session.Save(customerEnterdInvoiceDetails, customerEnterdInvoiceDetails.Id);
                        foreach (CustomerEnterdInvoiceTireDetails customerInvTireDetail in customerEnterdInvoiceTireDetailsList)
                        {
                            session.Evict(customerInvTireDetail);
                            session.Save(customerInvTireDetail, customerInvTireDetail.Id);
                        }

                        //remove previous record for update purpose
                        if (oldCustomerEnterdInvoiceDetails != null)
                        {
                            session.Evict(oldCustomerEnterdInvoiceDetails);
                            session.Delete(oldCustomerEnterdInvoiceDetails);

                            foreach (CustomerEnterdInvoiceTireDetails customerTireDetails in oldCustomerEnterdInvoiceTireDetailsList)
                            {
                                session.Evict(customerTireDetails);
                                session.Delete(customerTireDetails);
                            }
                        }


                        transaction.Commit();
                    }

                    //success
                    response.code = "SUCCESS";
                    response.msg = customerEnterdInvoiceDetailsId.ToString();

                }
            }
            catch (Exception ex)
            {
                response.msg = "Error occured while saving invoice details.";
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ((ex.InnerException != null) ? ex.InnerException.ToString() : ""));
            }
            return response;
        }

        internal static object ManufacturerWarrentyAvailabilityCheck(ManufacturerWarrentyAvailabilityCheckDto policyDetails)
        {
            GenericCodeMsgResponse response = new GenericCodeMsgResponse();
            try
            {
                if (policyDetails == null)
                {
                    return response;
                }

                ISession session = EntitySessionManager.GetSession();
                Dealer dealer = session.Query<Dealer>()
                    .FirstOrDefault(a => a.Id == policyDetails.dealerId);
                if (dealer == null)
                {
                    return response;
                }

                var manufWarrentyData = session.Query<ManufacturerWarranty>()
                    .Join(session.Query<ManufacturerWarrantyDetails>(), a => a.Id, b => b.ManufacturerWarrantyId, (a, b) => new { a, b })
                    .Where(a => a.a.MakeId == policyDetails.makeId && a.b.ModelId == policyDetails.modelId && a.b.CountryId == dealer.CountryId
                     && a.a.ApplicableFrom <= policyDetails.mwStartDate)
                     .OrderByDescending(a => a.a.ApplicableFrom)
                     .Select(x => new
                     {
                         WarrantyKm = x.a.IsUnlimited ? "Unlimited" : x.a.WarrantyKm.ToString(),
                         x.a.WarrantyMonths,
                         x.a.ApplicableFrom,
                         x.a.IsUnlimited
                     });
                if (manufWarrentyData == null || manufWarrentyData.FirstOrDefault() == null)
                {
                    response.code = "No";
                    return response;
                }

                var selectedMw = manufWarrentyData.FirstOrDefault();
                var commodityCode = new CommonEntityManager().GetCommodityTypeUniqueCodeById(policyDetails.commodityTypeId);
                //checking if the mw still applicable
                var isMwViolated = false;
                if (commodityCode.ToLower() == "a")
                {
                    //vehicle mw validation
                    isMwViolated = selectedMw.IsUnlimited ? false : policyDetails.usage > decimal.Parse(selectedMw.WarrantyKm);
                    if (!isMwViolated)
                    {
                        isMwViolated = policyDetails.mwStartDate.AddMonths(selectedMw.WarrantyMonths) < policyDetails.policySoldDate;
                    }
                }
                else if (commodityCode.ToLower() == "b")
                {
                    //vehicle mw validation
                    isMwViolated = selectedMw.IsUnlimited ? false : policyDetails.usage > decimal.Parse(selectedMw.WarrantyKm);
                    if (!isMwViolated)
                    {
                        isMwViolated = policyDetails.mwStartDate.AddMonths(selectedMw.WarrantyMonths) < policyDetails.policySoldDate;
                    }
                }
                else if (commodityCode.ToLower() == "b")
                {
                    //vehicle mw validation
                    isMwViolated = selectedMw.IsUnlimited ? false : policyDetails.usage > decimal.Parse(selectedMw.WarrantyKm);
                    if (!isMwViolated)
                    {
                        isMwViolated = policyDetails.mwStartDate.AddMonths(selectedMw.WarrantyMonths) < policyDetails.policySoldDate;
                    }
                }
                else
                {
                    //other items mw validation
                    isMwViolated = policyDetails.usage > selectedMw.WarrantyMonths;
                }

                response.code = isMwViolated ? "Yes" : "No";
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ((ex.InnerException != null) ? ex.InnerException.ToString() : ""));
            }
            return response;
        }

        public List<PolicyInfo> GetPolicysByCustomerId(Guid customerId)
        {
            List<PolicyInfo> entities = new List<PolicyInfo>();

            try
            {
                ISession session = EntitySessionManager.GetSession();
                //IQueryable<Policy> PolicyData = session.Query<Policy>();

                //IQueryable<Policy> PolicyData = from p in session.Query<Policy>()
                //                              where p.CustomerId == customerId
                //                              select p;

                //IQueryable<VehiclePolicy> VPolicyData = session.Query<VehiclePolicy>();

                //IQueryable<BAndWPolicy> BPolicyData = session.Query<BAndWPolicy>();

                IQueryable<Policy> PolicyData = session.Query<Policy>()
                                                .Where(a => a.CustomerId == customerId);

                if (PolicyData.ToList().Count > 0)
                {

                    IQueryable<VehiclePolicy> VPolicyData = session.Query<VehiclePolicy>()
                                                            .Where(a => PolicyData.Any(b => b.Id == a.PolicyId)).Distinct();

                    IQueryable<BAndWPolicy> BPolicyData = session.Query<BAndWPolicy>()
                                                            .Where(a => PolicyData.Any(b => b.Id == a.PolicyId)).Distinct();

                    if (VPolicyData.ToList().Count > 0)
                    {
                        foreach (var item in VPolicyData)
                        {
                            Policy policy = PolicyData.FirstOrDefault(p => p.Id == item.PolicyId);
                            if (policy != null)
                            {
                                entities.Add(new PolicyInfo()
                                {
                                    Comment = policy.Comment,
                                    CommodityTypeId = policy.CommodityTypeId,
                                    ContractId = policy.ContractId,
                                    CoverTypeId = policy.CoverTypeId,
                                    CurrencyPeriodId = policy.CurrencyPeriodId,
                                    PremiumCurrencyTypeId = policy.PremiumCurrencyTypeId,
                                    DealerPaymentCurrencyTypeId = policy.DealerPaymentCurrencyTypeId,
                                    CustomerPaymentCurrencyTypeId = policy.CustomerPaymentCurrencyTypeId,
                                    CustomerId = policy.CustomerId,
                                    CustomerPayment = policy.CustomerPayment,
                                    DealerId = policy.DealerId,
                                    DealerLocationId = policy.DealerLocationId,
                                    DealerPayment = policy.DealerPayment,
                                    EntryDateTime = policy.EntryDateTime,
                                    EntryUser = policy.EntryUser,
                                    ExtensionTypeId = policy.ExtensionTypeId,
                                    HrsUsedAtPolicySale = policy.HrsUsedAtPolicySale,
                                    Id = policy.Id,
                                    IsPartialPayment = policy.IsPartialPayment,
                                    IsPreWarrantyCheck = policy.IsPreWarrantyCheck,
                                    IsSpecialDeal = policy.IsSpecialDeal,
                                    ItemId = item.VehicleId,
                                    ProductId = policy.ProductId,
                                    PaymentModeId = policy.PaymentModeId,
                                    PolicyNo = policy.PolicyNo,
                                    PolicySoldDate = policy.PolicySoldDate,
                                    Premium = policy.Premium,
                                    RefNo = policy.RefNo,
                                    SalesPersonId = policy.SalesPersonId,
                                    Type = "Vehicle",
                                    IsApproved = policy.IsApproved,
                                    IsPolicyCanceled = policy.IsPolicyCanceled,
                                    PolicyStartDate = policy.PolicyStartDate,
                                    PolicyEndDate = policy.PolicyEndDate,
                                    Discount = policy.Discount,
                                    PolicyBundleId = policy.PolicyBundleId,
                                    TransferFee = policy.TransferFee,
                                    BordxId = policy.BordxId,
                                    Year = policy.Year,
                                    Month = policy.Month,
                                    ForwardComment = policy.ForwardComment,
                                    DealerPolicy = policy.DealerPolicy,
                                    TPABranchId = policy.TPABranchId
                                });
                            }
                        }
                    }
                    if (BPolicyData.ToList().Count > 0)
                    {
                        foreach (var item in BPolicyData)
                        {
                            Policy policy = PolicyData.FirstOrDefault(p => p.Id == item.PolicyId);
                            if (policy != null)
                            {
                                entities.Add(new PolicyInfo()
                                {
                                    Comment = policy.Comment,
                                    CommodityTypeId = policy.CommodityTypeId,
                                    ContractId = policy.ContractId,
                                    CoverTypeId = policy.CoverTypeId,
                                    PremiumCurrencyTypeId = policy.PremiumCurrencyTypeId,
                                    DealerPaymentCurrencyTypeId = policy.DealerPaymentCurrencyTypeId,
                                    CustomerPaymentCurrencyTypeId = policy.CustomerPaymentCurrencyTypeId,
                                    CustomerId = policy.CustomerId,
                                    CustomerPayment = policy.CustomerPayment,
                                    DealerId = policy.DealerId,
                                    DealerLocationId = policy.DealerLocationId,
                                    DealerPayment = policy.DealerPayment,
                                    EntryDateTime = policy.EntryDateTime,
                                    EntryUser = policy.EntryUser,
                                    ExtensionTypeId = policy.ExtensionTypeId,
                                    HrsUsedAtPolicySale = policy.HrsUsedAtPolicySale,
                                    Id = policy.Id,
                                    IsPartialPayment = policy.IsPartialPayment,
                                    IsPreWarrantyCheck = policy.IsPreWarrantyCheck,
                                    IsSpecialDeal = policy.IsSpecialDeal,
                                    ItemId = item.BAndWId,
                                    ProductId = policy.ProductId,
                                    PaymentModeId = policy.PaymentModeId,
                                    PolicyNo = policy.PolicyNo,
                                    PolicySoldDate = policy.PolicySoldDate,
                                    Premium = policy.Premium,
                                    RefNo = policy.RefNo,
                                    SalesPersonId = policy.SalesPersonId,
                                    Type = "B&W",
                                    IsApproved = policy.IsApproved,
                                    IsPolicyCanceled = policy.IsPolicyCanceled,
                                    PolicyStartDate = policy.PolicyStartDate,
                                    PolicyEndDate = policy.PolicyEndDate,
                                    Discount = policy.Discount,
                                    PolicyBundleId = policy.PolicyBundleId,
                                    ForwardComment = policy.ForwardComment,
                                    Year = policy.Year,
                                    Month = policy.Month,
                                    BordxId = policy.BordxId,
                                    DealerPolicy = policy.DealerPolicy
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ((ex.InnerException != null) ? ex.InnerException.ToString() : ""));
            }

            //entities = PolicyData.ToList();
            return entities;//.FindAll(p=>p.IsApproved.Equals(false));
        }



        public PolicyTransactionResponseDto GetPolicyTransactionById(Guid PolicyId)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();
                PolicyTransactionResponseDto pDto = new PolicyTransactionResponseDto();
                CurrencyEntityManager currencyEM = new CurrencyEntityManager();
                var query =
                    from PolicyTransaction in session.Query<PolicyTransaction>()
                    where PolicyTransaction.PolicyId == PolicyId
                    select new { PolicyTransaction = PolicyTransaction };
                var result = query.ToList();

                var queryV =
                    from v in session.Query<VehiclePolicy>()
                    where PolicyId == v.PolicyId
                    select new { PolicyV = v };
                var resultV = queryV.ToList();

                var queryB =
                    from b in session.Query<BAndWPolicy>()
                    where PolicyId == b.PolicyId
                    select new { PolicyB = b };
                var resultB = queryB.ToList();
                Guid itemId = Guid.Parse("00000000-0000-0000-0000-000000000000");
                if (resultB.Count > 0)
                {
                    itemId = Guid.Parse(resultB[0].PolicyB.BAndWId.ToString());
                }
                if (resultV.Count > 0)
                {
                    itemId = Guid.Parse(resultV[0].PolicyV.VehicleId.ToString());
                }

                if (result != null && result.Count > 0)
                {

                    pDto.Id = result.First().PolicyTransaction.Id;
                    pDto.Comment = result.First().PolicyTransaction.Comment;
                    pDto.CommodityTypeId = result.First().PolicyTransaction.CommodityTypeId;
                    pDto.ContractId = result.First().PolicyTransaction.ContractId;
                    pDto.CoverTypeId = result.First().PolicyTransaction.CoverTypeId;
                    pDto.PremiumCurrencyTypeId = result.First().PolicyTransaction.PremiumCurrencyTypeId;
                    pDto.DealerPaymentCurrencyTypeId = result.First().PolicyTransaction.DealerPaymentCurrencyTypeId;
                    pDto.CustomerPaymentCurrencyTypeId = result.First().PolicyTransaction.CustomerPaymentCurrencyTypeId;
                    pDto.CustomerId = result.First().PolicyTransaction.CustomerId;
                    pDto.CustomerPayment = result.First().PolicyTransaction.CustomerPayment;
                    //pDto.CustomerPayment = currencyEM.ConvertFromBaseCurrency(result.First().PolicyTransaction.CustomerPayment, result.First().PolicyTransaction.CustomerPaymentCurrencyTypeId, result.First().PolicyTransaction.CurrencyPeriodId);
                    pDto.DealerId = result.First().PolicyTransaction.DealerId;
                    pDto.DealerLocationId = result.First().PolicyTransaction.DealerLocationId;
                    pDto.DealerPayment = result.First().PolicyTransaction.DealerPayment;

                    //pDto.DealerPayment = currencyEM.ConvertFromBaseCurrency(result.First().PolicyTransaction.DealerPayment, result.First().PolicyTransaction.DealerPaymentCurrencyTypeId, result.First().PolicyTransaction.CurrencyPeriodId);
                    pDto.ExtensionTypeId = result.First().PolicyTransaction.ExtensionTypeId;
                    pDto.HrsUsedAtPolicySale = result.First().PolicyTransaction.HrsUsedAtPolicySale;
                    pDto.IsPartialPayment = result.First().PolicyTransaction.IsPartialPayment;
                    pDto.IsPreWarrantyCheck = result.First().PolicyTransaction.IsPreWarrantyCheck;
                    pDto.IsSpecialDeal = result.First().PolicyTransaction.IsSpecialDeal;
                    pDto.ProductId = result.First().PolicyTransaction.ProductId;
                    pDto.PaymentModeId = result.First().PolicyTransaction.PaymentModeId;
                    pDto.PolicyNo = result.First().PolicyTransaction.PolicyNo;
                    pDto.PolicySoldDate = result.First().PolicyTransaction.PolicySoldDate;
                    pDto.Premium = result.First().PolicyTransaction.Premium;
                    //pDto.Premium = currencyEM.ConvertFromBaseCurrency(result.First().PolicyTransaction.Premium, result.First().PolicyTransaction.PremiumCurrencyTypeId, result.First().PolicyTransaction.CurrencyPeriodId);
                    pDto.RefNo = result.First().PolicyTransaction.RefNo;
                    pDto.SalesPersonId = result.First().PolicyTransaction.SalesPersonId;
                    pDto.IsApproved = result.First().PolicyTransaction.IsApproved;
                    pDto.PolicyStartDate = result.First().PolicyTransaction.PolicyStartDate;
                    pDto.PolicyEndDate = result.First().PolicyTransaction.PolicyEndDate;
                    pDto.Discount = result.First().PolicyTransaction.Discount;
                    //pDto.Discount = currencyEM.ConvertFromBaseCurrency(result.First().PolicyTransaction.Discount, result.First().PolicyTransaction.PremiumCurrencyTypeId, result.First().PolicyTransaction.CurrencyPeriodId);
                    pDto.TransferFee = result.First().PolicyTransaction.TransferFee;
                    pDto.PolicyBundleId = result.First().PolicyTransaction.PolicyBundleId;
                    pDto.DealerPolicy = result.First().PolicyTransaction.DealerPolicy;
                    pDto.AddnSerialNo = result.First().PolicyTransaction.AddnSerialNo;
                    pDto.Address1 = result.First().PolicyTransaction.Address1;
                    pDto.Address2 = result.First().PolicyTransaction.Address2;
                    pDto.Address3 = result.First().PolicyTransaction.Address3;
                    pDto.Address4 = result.First().PolicyTransaction.Address4;
                    pDto.AspirationId = result.First().PolicyTransaction.AspirationId;
                    pDto.BAndWId = result.First().PolicyTransaction.BAndWId;
                    pDto.BodyTypeId = result.First().PolicyTransaction.BodyTypeId;
                    pDto.BusinessAddress1 = result.First().PolicyTransaction.BusinessAddress1;
                    pDto.BusinessAddress2 = result.First().PolicyTransaction.BusinessAddress2;
                    pDto.BusinessAddress3 = result.First().PolicyTransaction.BusinessAddress3;
                    pDto.BusinessAddress4 = result.First().PolicyTransaction.BusinessAddress4;
                    pDto.BusinessName = result.First().PolicyTransaction.BusinessName;
                    pDto.BusinessTelNo = result.First().PolicyTransaction.BusinessTelNo;
                    pDto.CancelationComment = result.First().PolicyTransaction.CancelationComment;
                    pDto.CategoryId = result.First().PolicyTransaction.CategoryId;
                    pDto.CityId = result.First().PolicyTransaction.CityId;
                    pDto.EngineCapacityId = result.First().PolicyTransaction.EngineCapacityId;
                    pDto.VINNo = result.First().PolicyTransaction.VINNo;
                    pDto.TransmissionId = result.First().PolicyTransaction.TransmissionId;
                    pDto.SerialNo = result.First().PolicyTransaction.SerialNo;
                    pDto.InvoiceNo = result.First().PolicyTransaction.InvoiceNo;
                    return pDto;
                }
                else
                {

                    return null;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ((ex.InnerException != null) ? ex.InnerException.ToString() : ""));
                return null;
            }
        }



        public PolicyResponseDto GetPolicyById(Guid PolicyId)
        {

            try
            {
                ISession session = EntitySessionManager.GetSession();

                PolicyResponseDto pDto = new PolicyResponseDto();
                CurrencyEntityManager currencyEM = new CurrencyEntityManager();
                var query =
                    from Policy in session.Query<Policy>()
                    where Policy.Id == PolicyId
                    select new { Policy = Policy };
                var result = query.ToList();

                var queryV =
                    from v in session.Query<VehiclePolicy>()
                    where PolicyId == v.PolicyId
                    select new { PolicyV = v };
                var resultV = queryV.ToList();

                var queryB =
                    from b in session.Query<BAndWPolicy>()
                    where PolicyId == b.PolicyId
                    select new { PolicyB = b };
                var resultB = queryB.ToList();
                Guid itemId = Guid.Parse("00000000-0000-0000-0000-000000000000");
                string type = "";
                if (resultB.Count > 0)
                {
                    itemId = Guid.Parse(resultB[0].PolicyB.BAndWId.ToString());
                    type = "B&W";
                }
                if (resultV.Count > 0)
                {
                    itemId = Guid.Parse(resultV[0].PolicyV.VehicleId.ToString());
                    type = "Vehicle";
                }



                if (result != null && result.Count > 0)
                {
                    pDto.Id = result.First().Policy.Id;
                    pDto.Comment = result.First().Policy.Comment;
                    pDto.CommodityTypeId = result.First().Policy.CommodityTypeId;
                    pDto.ContractId = result.First().Policy.ContractId;
                    pDto.CoverTypeId = result.First().Policy.CoverTypeId;
                    pDto.PremiumCurrencyTypeId = result.First().Policy.PremiumCurrencyTypeId;
                    pDto.DealerPaymentCurrencyTypeId = result.First().Policy.DealerPaymentCurrencyTypeId;
                    pDto.CustomerPaymentCurrencyTypeId = result.First().Policy.CustomerPaymentCurrencyTypeId;
                    pDto.CustomerId = result.First().Policy.CustomerId;
                    pDto.CustomerPayment = currencyEM.ConvertFromBaseCurrency(result.First().Policy.CustomerPayment, result.First().Policy.CustomerPaymentCurrencyTypeId, result.First().Policy.CurrencyPeriodId);
                    pDto.DealerId = result.First().Policy.DealerId;
                    pDto.DealerLocationId = result.First().Policy.DealerLocationId;
                    pDto.DealerPayment = currencyEM.ConvertFromBaseCurrency(result.First().Policy.DealerPayment, result.First().Policy.DealerPaymentCurrencyTypeId, result.First().Policy.CurrencyPeriodId);
                    pDto.ExtensionTypeId = result.First().Policy.ExtensionTypeId;
                    pDto.HrsUsedAtPolicySale = result.First().Policy.HrsUsedAtPolicySale;
                    pDto.IsPartialPayment = result.First().Policy.IsPartialPayment;
                    pDto.IsPreWarrantyCheck = result.First().Policy.IsPreWarrantyCheck;
                    pDto.IsSpecialDeal = result.First().Policy.IsSpecialDeal;
                    pDto.ItemId = itemId;
                    pDto.Type = type;
                    pDto.ProductId = result.First().Policy.ProductId;
                    pDto.ProductCode = GetProductCodeById(result.First().Policy.ProductId);
                    pDto.PaymentModeId = result.First().Policy.PaymentModeId;
                    pDto.PolicyNo = result.First().Policy.PolicyNo;
                    pDto.PolicySoldDate = result.First().Policy.PolicySoldDate;
                    pDto.Premium = currencyEM.ConvertFromBaseCurrency(result.First().Policy.Premium, result.First().Policy.PremiumCurrencyTypeId, result.First().Policy.CurrencyPeriodId);
                    pDto.RefNo = result.First().Policy.RefNo;
                    pDto.SalesPersonId = result.First().Policy.SalesPersonId;
                    pDto.EntryDateTime = result.First().Policy.EntryDateTime;
                    pDto.EntryUser = result.First().Policy.EntryUser;
                    pDto.IsApproved = result.First().Policy.IsApproved;
                    pDto.IsRenewed = result.First().Policy.IsPolicyRenewed;
                    pDto.IsPolicyCanceled = result.First().Policy.IsPolicyCanceled;
                    pDto.PolicyStartDate = result.First().Policy.PolicyStartDate;
                    pDto.PolicyEndDate = result.First().Policy.PolicyEndDate;
                    pDto.Discount = currencyEM.ConvertFromBaseCurrency(result.First().Policy.Discount, result.First().Policy.PremiumCurrencyTypeId, result.First().Policy.CurrencyPeriodId);
                    pDto.TransferFee = result.First().Policy.TransferFee;
                    pDto.PolicyBundleId = result.First().Policy.PolicyBundleId;
                    pDto.ForwardComment = result.First().Policy.ForwardComment;
                    pDto.BordxId = result.First().Policy.BordxId;
                    pDto.Month = result.First().Policy.Month;
                    pDto.Year = result.First().Policy.Year;
                    pDto.DealerPolicy = result.First().Policy.DealerPolicy;
                    pDto.MWStartDate = result.First().Policy.MWStartDate;
                    pDto.MWIsAvailable = result.First().Policy.MWIsAvailable;
                    pDto.IsPolicyExists = true;
                    return pDto;
                }
                else
                {
                    pDto.IsPolicyExists = false;
                    return null;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ((ex.InnerException != null) ? ex.InnerException.ToString() : ""));
                return null;
            }

        }

        internal string GetProductCodeById(Guid productId) {
            string productCode = "";
            try
            {
                ISession session = EntitySessionManager.GetSession();

            productCode= session.Query<Product>().Where(p => p.Id == productId).FirstOrDefault().Productcode;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ((ex.InnerException != null) ? ex.InnerException.ToString() : ""));
            }
            return productCode;
        }
        internal bool CheckEndosmentApprovalPending(Guid PolicyId)
        {
            bool res = false;
            try
            {
                ISession session = EntitySessionManager.GetSession();
                int response = (from p in session.Query<Policy>()
                                join pt in session.Query<PolicyTransaction>() on p.Id equals pt.PolicyId
                                join tt in session.Query<PolicyTransactionType>() on pt.TransactionTypeId equals tt.Id
                                where tt.Code == "Endorsement" && pt.IsRecordActive == true && pt.IsApproved == false && pt.IsRejected == false && p.Id == PolicyId
                                select new { p.Id }
                                ).ToList().Count();
                if (response > 0) {
                    res = true;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ((ex.InnerException != null) ? ex.InnerException.ToString() : ""));
                res= false;
            }
            return res;
        }

            internal bool AddPolicy(PolicyRequestDto Policy)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();
                #region "crap"
                //Policy pr = new Entities.Policy();
                //VehiclePolicy v = new Entities.VehiclePolicy();
                //BAndWPolicy b = new Entities.BAndWPolicy();
                //OtherItemPolicy o = new OtherItemPolicy();
                //YellowGoodPolicy y = new YellowGoodPolicy();

                //// List<PolicyContractProduct> p = new List<Entities.PolicyContractProduct>();

                //DateTime Start = new DateTime();
                //DateTime End = new DateTime();

                //var contractData =
                //   from Contract in session.Query<Contract>()
                //   where Contract.Id == Policy.ContractId
                //   select new { Contract = Contract };

                //var contract = session.Query<Contract>().Where(a => a.Id == Policy.ContractId).FirstOrDefault();

                //var ContractExtensionData =
                //  from ContractExtensions in session.Query<ContractExtensions>()
                //  //where ContractExtensions.ExtensionTypeId == Policy.ExtensionTypeId
                //  //&& ContractExtensions.ContractId == Policy.ContractId
                //  //&& ContractExtensions.WarrantyTypeId == Policy.CoverTypeId
                //  select new { ContractExtensions = ContractExtensions };
                //if (ContractExtensionData.ToList().Count == 0)
                //{
                //    ContractExtensionData =
                //            from ContractExtensions in session.Query<ContractExtensions>()
                //           // where ContractExtensions.ExtensionTypeId == Policy.ExtensionTypeId
                //           // && ContractExtensions.ContractId == Policy.ContractId
                //           // && ContractExtensions.RSAProviderId == Policy.CoverTypeId
                //            select new { ContractExtensions = ContractExtensions };
                //}
                //var ContractExtension = ContractExtensionData.First().ContractExtensions;

                //var ExtensionTypeData =
                // from ExtensionType in session.Query<ExtensionType>()
                // where ExtensionType.Id == Policy.ExtensionTypeId
                // select new { ExtensionType = ExtensionType };

                //var extensionType = ExtensionTypeData.First().ExtensionType;


                //if (Policy.Type == "Vehicle")
                //{

                //    Start = Policy.PolicySoldDate;
                //    End = Policy.PolicySoldDate.AddMonths(ExtensionTypeData.First().ExtensionType.Month);

                //    var vehicleData =
                //     from VehicleDetails in session.Query<VehicleDetails>()
                //     where VehicleDetails.Id == Policy.ItemId
                //     select new { VehicleDetails = VehicleDetails };

                //    Guid itemStatus = vehicleData.First().VehicleDetails.ItemStatusId;
                //    Guid ModelId = vehicleData.First().VehicleDetails.ModelId;
                //    var itemStatusData =
                //    from ItemStatus in session.Query<ItemStatus>()
                //    where ItemStatus.Id == itemStatus
                //    select new { ItemStatus = ItemStatus };

                //    var ManufacturerWarrantyDetailsData =
                //        from ManufacturerWarrantyDetails in session.Query<ManufacturerWarrantyDetails>()
                //        where ManufacturerWarrantyDetails.ModelId == ModelId
                //        select new { ManufacturerWarrantyDetails = ManufacturerWarrantyDetails };
                //    if (ManufacturerWarrantyDetailsData.Count() > 0 &&
                //        ManufacturerWarrantyDetailsData.First().ManufacturerWarrantyDetails != null)
                //    {
                //        Guid ManufacturerWarrantyId = ManufacturerWarrantyDetailsData.First().ManufacturerWarrantyDetails.ManufacturerWarrantyId;

                //        var ManufacturerWarrantyData =
                //          from ManufacturerWarranty in session.Query<ManufacturerWarranty>()
                //          where ManufacturerWarranty.Id == ManufacturerWarrantyId
                //          select new { ManufacturerWarranty = ManufacturerWarranty };



                //        if (itemStatusData.First().ItemStatus.Status == "New"
                //            && ManufacturerWarrantyData.Count() > 0)
                //        {
                //            var manufacturerWarranty = ManufacturerWarrantyData.OrderBy(m => m.ManufacturerWarranty.ApplicableFrom).FirstOrDefault().ManufacturerWarranty;
                //            Start = Policy.PolicySoldDate.AddMonths(manufacturerWarranty.WarrantyMonths);
                //            End = Start.AddMonths(extensionType.Month);
                //        }
                //        else
                //        {
                //            Start = Policy.PolicySoldDate;
                //            End = Policy.PolicySoldDate.AddMonths(ExtensionTypeData.First().ExtensionType.Month);
                //        }
                //    }
                //}
                //else if (Policy.Type == "B&W")
                //{
                //    var bAndWData =
                //     from BrownAndWhiteDetails in session.Query<BrownAndWhiteDetails>()
                //     where BrownAndWhiteDetails.Id == Policy.ItemId
                //     select new { BrownAndWhiteDetails = BrownAndWhiteDetails };

                //    Guid itemStatusId = bAndWData.First().BrownAndWhiteDetails.ItemStatusId;
                //    Guid ModelId = bAndWData.First().BrownAndWhiteDetails.ModelId;
                //    var itemStatusData =
                //    from ItemStatus in session.Query<ItemStatus>()
                //    where ItemStatus.Id == itemStatusId
                //    select new { ItemStatus = ItemStatus };

                //    var ManufacturerWarrantyDetailsData =
                //        from ManufacturerWarrantyDetails in session.Query<ManufacturerWarrantyDetails>()
                //        where ManufacturerWarrantyDetails.ModelId == ModelId
                //        select new { ManufacturerWarrantyDetails = ManufacturerWarrantyDetails };

                //    Guid ManufacturerWarrantyId = ManufacturerWarrantyDetailsData.First().ManufacturerWarrantyDetails.ManufacturerWarrantyId;

                //    var ManufacturerWarrantyData =
                //      from ManufacturerWarranty in session.Query<ManufacturerWarranty>()
                //      where ManufacturerWarranty.Id == ManufacturerWarrantyId
                //      select new { ManufacturerWarranty = ManufacturerWarranty };

                //    //  var ManufacturerWarrantyData =
                //    //  from ManufacturerWarranty in session.Query<ManufacturerWarranty>()
                //    ////  where ManufacturerWarranty.ModelId == ModelId
                //    //  select new { ManufacturerWarranty = ManufacturerWarranty };

                //    if (itemStatusData.First().ItemStatus.Status == "New"
                //       && ManufacturerWarrantyData.Count() > 0)
                //    {
                //        var manufacturerWarranty = ManufacturerWarrantyData.OrderBy(m => m.ManufacturerWarranty.ApplicableFrom).FirstOrDefault().ManufacturerWarranty;
                //        Start = Policy.PolicySoldDate.AddHours(manufacturerWarranty.WarrantyMonths);
                //        End = Start.AddHours(extensionType.Hours);
                //    }
                //    else
                //    {
                //        Start = Policy.PolicySoldDate;
                //        End = Policy.PolicySoldDate.AddMonths(ExtensionTypeData.First().ExtensionType.Month);
                //    }
                //}
                //else if (Policy.Type == "YellowGood")
                //{
                //    var YellowGoodData =
                //    from YellowGoodDetails in session.Query<YellowGoodDetails>()
                //    where YellowGoodDetails.Id == Policy.ItemId
                //    select new { YellowGoodDetails = YellowGoodDetails };

                //    Guid itemStatusId = YellowGoodData.First().YellowGoodDetails.ItemStatusId;
                //    Guid ModelId = YellowGoodData.First().YellowGoodDetails.ModelId;
                //    var itemStatusData =
                //    from ItemStatus in session.Query<ItemStatus>()
                //    where ItemStatus.Id == itemStatusId
                //    select new { ItemStatus = ItemStatus };

                //    var ManufacturerWarrantyDetailsData =
                //        from ManufacturerWarrantyDetails in session.Query<ManufacturerWarrantyDetails>()
                //        where ManufacturerWarrantyDetails.ModelId == ModelId
                //        select new { ManufacturerWarrantyDetails = ManufacturerWarrantyDetails };

                //    Guid ManufacturerWarrantyId = ManufacturerWarrantyDetailsData.First().ManufacturerWarrantyDetails.ManufacturerWarrantyId;

                //    var ManufacturerWarrantyData =
                //      from ManufacturerWarranty in session.Query<ManufacturerWarranty>()
                //      where ManufacturerWarranty.Id == ManufacturerWarrantyId
                //      select new { ManufacturerWarranty = ManufacturerWarranty };

                //    //  var ManufacturerWarrantyData =
                //    //  from ManufacturerWarranty in session.Query<ManufacturerWarranty>()
                //    ////  where ManufacturerWarranty.ModelId == ModelId
                //    //  select new { ManufacturerWarranty = ManufacturerWarranty };

                //    if (itemStatusData.First().ItemStatus.Status == "New"
                //        && ManufacturerWarrantyData.Count() > 0)
                //    {
                //        var manufacturerWarranty = ManufacturerWarrantyData.OrderBy(m => m.ManufacturerWarranty.ApplicableFrom).FirstOrDefault().ManufacturerWarranty;
                //        Start = Policy.PolicySoldDate.AddHours(manufacturerWarranty.WarrantyMonths);
                //        End = Start.AddHours(extensionType.Hours);
                //    }
                //    else
                //    {
                //        Start = Policy.PolicySoldDate;
                //        End = Policy.PolicySoldDate.AddMonths(ExtensionTypeData.First().ExtensionType.Month);
                //    }
                //}
                //else if (Policy.Type == "Other")
                //{
                //    var OtherItemData =
                //    from OtherItemDetails in session.Query<OtherItemDetails>()
                //    where OtherItemDetails.Id == Policy.ItemId
                //    select new { OtherItemDetails = OtherItemDetails };

                //    Guid itemStatusId = OtherItemData.First().OtherItemDetails.ItemStatusId;
                //    Guid ModelId = OtherItemData.First().OtherItemDetails.ModelId;

                //    var itemStatusData =
                //    from ItemStatus in session.Query<ItemStatus>()
                //    where ItemStatus.Id == itemStatusId
                //    select new { ItemStatus = ItemStatus };

                //    var ManufacturerWarrantyDetailsData =
                //        from ManufacturerWarrantyDetails in session.Query<ManufacturerWarrantyDetails>()
                //        where ManufacturerWarrantyDetails.ModelId == ModelId
                //        select new { ManufacturerWarrantyDetails = ManufacturerWarrantyDetails };

                //    Guid ManufacturerWarrantyId = ManufacturerWarrantyDetailsData.First().ManufacturerWarrantyDetails.ManufacturerWarrantyId;

                //    var ManufacturerWarrantyData =
                //      from ManufacturerWarranty in session.Query<ManufacturerWarranty>()
                //      where ManufacturerWarranty.Id == ManufacturerWarrantyId
                //      select new { ManufacturerWarranty = ManufacturerWarranty };

                //    // var ManufacturerWarrantyData =
                //    // from ManufacturerWarranty in session.Query<ManufacturerWarranty>()
                //    //// where ManufacturerWarranty.ModelId == ModelId
                //    // select new { ManufacturerWarranty = ManufacturerWarranty };

                //    if (itemStatusData.First().ItemStatus.Status == "New"
                //        && ManufacturerWarrantyData.Count() > 0)
                //    {
                //        var manufacturerWarranty = ManufacturerWarrantyData.OrderBy(m => m.ManufacturerWarranty.ApplicableFrom).FirstOrDefault().ManufacturerWarranty;
                //        Start = Policy.PolicySoldDate.AddHours(manufacturerWarranty.WarrantyMonths);
                //        End = Start.AddHours(extensionType.Hours);
                //    }
                //    else
                //    {
                //        Start = Policy.PolicySoldDate;
                //        End = Policy.PolicySoldDate.AddMonths(ExtensionTypeData.First().ExtensionType.Month);
                //    }
                //}

                //pr.Id = new Guid();
                //pr.Comment = Policy.Comment;
                //pr.CommodityTypeId = Policy.CommodityTypeId;
                //pr.ContractId = Policy.ContractId;
                //pr.CoverTypeId = Policy.CoverTypeId;
                //pr.PremiumCurrencyTypeId = Policy.PremiumCurrencyTypeId;
                //pr.DealerPaymentCurrencyTypeId = Policy.DealerPaymentCurrencyTypeId;
                //pr.CustomerPaymentCurrencyTypeId = Policy.CustomerPaymentCurrencyTypeId;
                //pr.CustomerId = Policy.CustomerId;
                //pr.CustomerPayment = Policy.CustomerPayment;
                //pr.DealerId = Policy.DealerId;
                //pr.DealerLocationId = Policy.DealerLocationId;
                //pr.DealerPayment = Policy.DealerPayment;
                //pr.ExtensionTypeId = Policy.ExtensionTypeId;
                //pr.HrsUsedAtPolicySale = Policy.HrsUsedAtPolicySale;
                //pr.IsPartialPayment = Policy.IsPartialPayment;
                //pr.IsPreWarrantyCheck = Policy.IsPreWarrantyCheck;
                //pr.IsSpecialDeal = Policy.IsSpecialDeal;
                //pr.ProductId = Policy.ProductId;
                //pr.PaymentModeId = Policy.PaymentModeId;
                //pr.PaymentTypeId = Policy.PaymentTypeId;
                //pr.PolicyNo = Policy.PolicyNo;
                //pr.PolicySoldDate = Policy.PolicySoldDate;
                //pr.CurrencyPeriodId = Policy.CurrencyPeriodId;
                //pr.Premium = Policy.Premium;
                //pr.IsApproved = Policy.IsApproved;
                //pr.IsPolicyCanceled = Policy.IsPolicyCanceled;
                //pr.RefNo = Policy.RefNo;
                //pr.SalesPersonId = Policy.SalesPersonId;
                //pr.PolicyStartDate = Start;
                //pr.PolicyEndDate = End;
                //pr.Discount = Policy.Discount;
                //pr.PolicyBundleId = Policy.PolicyBundleId;
                //pr.TransferFee = Policy.TransferFee;
                //pr.ForwardComment = Policy.ForwardComment;
                //pr.BordxId = Policy.BordxId;
                //pr.Month = Policy.Month;
                //pr.Year = Policy.Year;
                //pr.DealerPolicy = Policy.DealerPolicy;
                //pr.EntryDateTime = DateTime.Today.ToUniversalTime();
                //pr.TPABranchId = Policy.TPABranchId;
                //pr.LocalCurrencyConversionRate = Policy.LocalCurrencyConversionRate;
                //pr.EntryUser = Guid.Parse("ba56ec84-1fe0-4385-abe4-182f62caa050");

                //pr.PaymentMethodFee = Policy.PaymentMethodFee;
                //pr.PaymentMethodFeePercentage = Policy.PaymentMethodFeePercentage;
                //pr.GrossPremiumBeforeTax = Policy.GrossPremiumBeforeTax;
                //pr.EligibilityFee = Policy.EligibilityFee;
                //pr.NRP = Policy.NRP;
                //pr.TotalTax = Policy.TotalTax;
                ////pr.Co_Customer = Policy.


                ////setup policy taxes
                //List<PolicyTax> policyTaxes = new List<PolicyTax>();
                //IEnumerable<ContractTaxMapping> contractTaxes = session.Query<ContractTaxMapping>()
                //    .Where(a => a.ContractId == Policy.ContractId);
                //IEnumerable<CountryTaxes> countryTaxes = session.Query<CountryTaxes>()
                //    .Where(a => contractTaxes.Any(c => c.CountryTaxId == a.Id));

                //decimal TotalTaxSumWithGross = Policy.GrossPremiumBeforeTax;
                //foreach (CountryTaxes tax in countryTaxes)
                //{

                //    decimal ValueIncludedInPolicy = CalculateTaxValue(tax, Policy.NRP, Policy.GrossPremiumBeforeTax, TotalTaxSumWithGross);
                //    TotalTaxSumWithGross += ValueIncludedInPolicy;
                //    PolicyTax policyTax = new PolicyTax()
                //    {
                //        Id = Guid.NewGuid(),
                //        IndexVal = tax.IndexVal,
                //        IsOnGross = tax.IsOnGross,
                //        IsOnNRP = tax.IsOnNRP,
                //        IsOnPreviousTax = tax.IsOnPreviousTax,
                //        IsPercentage = tax.IsPercentage,
                //        PolicyId = pr.Id,
                //        TaxTypeId = tax.TaxTypeId,
                //        TaxValue = tax.TaxValue,
                //        ValueIncluededInPolicy = ValueIncludedInPolicy

                //    };
                //    policyTaxes.Add(policyTax);
                //}

                ////end of policy taxes

                //using (ITransaction transaction = session.BeginTransaction())
                //{
                //    session.SaveOrUpdate(pr);
                //    if (Policy.Type == "Vehicle")
                //    {
                //        v.Id = new Guid();
                //        v.VehicleId = Policy.ItemId;
                //        v.PolicyId = pr.Id;
                //        session.SaveOrUpdate(v);
                //    }
                //    else if (Policy.Type == "B&W")
                //    {
                //        b.Id = new Guid();
                //        b.BAndWId = Policy.ItemId;
                //        b.PolicyId = pr.Id;
                //        session.SaveOrUpdate(b);
                //    }
                //    else if (Policy.Type == "YellowGood")
                //    {
                //        y.Id = new Guid();
                //        y.YellowGoodId = Policy.ItemId;
                //        y.PolicyId = pr.Id;
                //        session.SaveOrUpdate(y);
                //    }
                //    else if (Policy.Type == "Other")
                //    {
                //        o.Id = new Guid();
                //        o.OtherItemId = Policy.ItemId;
                //        o.PolicyId = pr.Id;
                //        session.SaveOrUpdate(o);
                //    }


                //    foreach (PolicyTax pTax in policyTaxes)
                //    {
                //        pTax.PolicyId = pr.Id;
                //        session.Save(pTax, pTax.Id);
                //    }
                //    transaction.Commit();
                //}
                #endregion "crap"


                return true;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ((ex.InnerException != null) ? ex.InnerException.ToString() : ""));
                return false;
            }
            // return false;
        }

        private decimal CalculateTaxValue(CountryTaxes tax, decimal NRP, decimal PureGross, decimal TotalTaxSumWithGross)
        {
            decimal Response = decimal.Zero;
            try
            {
                if (tax.IsOnGross)
                {
                    if (tax.IsPercentage)
                    {
                        if (tax.IsOnPreviousTax)
                        {
                            Response = TotalTaxSumWithGross * tax.TaxValue / 100;
                        }
                        else
                        {
                            Response = PureGross * tax.TaxValue / 100;
                        }
                    }
                    else
                    {
                        Response = tax.TaxValue;
                    }
                }
                else
                {
                    if (tax.IsPercentage)
                    {
                        Response = NRP * tax.TaxValue / 100;
                    }
                    else
                    {
                        Response = tax.TaxValue;
                    }

                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ((ex.InnerException != null) ? ex.InnerException.ToString() : ""));
            }
            return Response;
        }

        internal bool UpdateInvoiceCode(InvoiceCodeRequestDto InvoiceCodeDetails, Guid policyid)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();
                InvoiceCode invoice = session.Query<InvoiceCode>().Where(a => a.Id == InvoiceCodeDetails.Id).FirstOrDefault();
                List<InvoiceCodeDetails> invoiceCodeDetails = session.Query<InvoiceCodeDetails>().Where(a => a.InvoiceCodeId == invoice.Id && a.PolicyId == policyid).ToList();

                using (ITransaction transaction = session.BeginTransaction())
                {

                    foreach (var invoiceCodeD in invoiceCodeDetails)
                    {
                        invoiceCodeD.IsPolicyApproved = InvoiceCodeDetails.IsPolicyApproved;
                        invoiceCodeD.PolicyCreatedDate = InvoiceCodeDetails.PolicyCreatedDate;
                        session.Update(invoiceCodeD);
                    }


                    transaction.Commit();

                }

                return true;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ((ex.InnerException != null) ? ex.InnerException.ToString() : ""));
                return false;
            }
        }




        internal bool UpdatePolicy(PolicyRequestDto Policy)
        {
            var expObj = new Entities.Policy();
            bool policyApproveDateWantToLoad = false;
            try
            {
                if (DateTime.MinValue.Equals(Policy.ApprovedDate)) {
                    policyApproveDateWantToLoad = true;
                    Policy.ApprovedDate = DateTime.UtcNow;
                }
                ISession session = EntitySessionManager.GetSession();
                CurrencyEntityManager currencyEm = new CurrencyEntityManager();
                Guid currentCurrencyPeriodId = currencyEm.GetCurrentCurrencyPeriodId();

                Policy pr = new Entities.Policy();
                VehiclePolicy v = new Entities.VehiclePolicy();
                BAndWPolicy b = new Entities.BAndWPolicy();
                OtherItemPolicy o = new Entities.OtherItemPolicy();
                YellowGoodPolicy y = new Entities.YellowGoodPolicy();

                VehiclePolicy vpp = session.Query<VehiclePolicy>().Where(a => a.PolicyId == Policy.Id).FirstOrDefault();
                OtherItemPolicy oip = session.Query<OtherItemPolicy>().Where(a => a.PolicyId == Policy.Id).FirstOrDefault();


                if (vpp != null)
                {

                    VehicleDetails Vd = session.Query<VehicleDetails>().Where(a => a.Id == vpp.VehicleId).FirstOrDefault();

                    session.Load(pr, Policy.Id);
                    pr.Id = Policy.Id;
                    pr.ContractId = Policy.ContractId;
                    pr.CoverTypeId = Policy.CoverTypeId;
                    pr.ExtensionTypeId = Policy.ExtensionTypeId;
                    pr.Premium = currencyEm.ConvertToBaseCurrency(Policy.Premium, Policy.PremiumCurrencyTypeId, currentCurrencyPeriodId);
                    pr.PremiumCurrencyTypeId = Policy.PremiumCurrencyTypeId;
                    pr.ProductId = Policy.ProductId;
                    pr.Comment = Policy.Comment;
                    pr.CommodityTypeId = Policy.CommodityTypeId;
                    pr.ContractId = Policy.ContractId;
                    pr.PremiumCurrencyTypeId = Policy.PremiumCurrencyTypeId;
                    pr.DealerPaymentCurrencyTypeId = Policy.DealerPaymentCurrencyTypeId;
                    pr.CustomerPaymentCurrencyTypeId = Policy.CustomerPaymentCurrencyTypeId;
                    pr.CustomerId = Policy.CustomerId;
                    pr.CustomerPayment = currencyEm.ConvertToBaseCurrency(Policy.CustomerPayment, Policy.PremiumCurrencyTypeId, currentCurrencyPeriodId);
                    pr.DealerId = Policy.DealerId;
                    pr.DealerLocationId = Policy.DealerLocationId;
                    pr.DealerPayment = currencyEm.ConvertToBaseCurrency(Policy.DealerPayment, Policy.PremiumCurrencyTypeId, currentCurrencyPeriodId);
                    pr.LocalCurrencyConversionRate = currencyEm.GetLocalCurrencyConversionRate(Policy.ContractId,
                        currentCurrencyPeriodId);
                    pr.ExtensionTypeId = Policy.ExtensionTypeId;
                    pr.HrsUsedAtPolicySale = Policy.HrsUsedAtPolicySale;
                    pr.IsPartialPayment = Policy.IsPartialPayment;
                    pr.IsPreWarrantyCheck = Policy.IsPreWarrantyCheck;
                    pr.IsSpecialDeal = Policy.IsSpecialDeal;
                    pr.ProductId = Policy.ProductId;
                    pr.ApprovedDate = Policy.ApprovedDate;
                    if (IsGuid(Policy.PaymentModeId.ToString()))
                    {
                        pr.PaymentModeId = Policy.PaymentModeId;
                    }

                    if (IsGuid(Policy.PaymentTypeId.ToString()))
                    {
                        pr.PaymentTypeId = Policy.PaymentTypeId;
                    }

                    pr.PolicyNo = Policy.PolicyNo;
                    pr.PolicySoldDate = Policy.PolicySoldDate;
                    //pr.Premium = Policy.Premium;
                    pr.RefNo = Policy.RefNo;
                    pr.SalesPersonId = Policy.SalesPersonId;
                    if (IsGuid(Policy.CurrencyPeriodId.ToString()))
                    {
                        pr.CurrencyPeriodId = Policy.CurrencyPeriodId;
                    }

                    //  pr.EntryUser = Policy.EntryUser;
                    pr.IsApproved = Policy.IsApproved;
                    pr.IsPolicyCanceled = Policy.IsPolicyCanceled;
                    // pr.PolicyStartDate =  Policy.PolicyStartDate;
                    pr.PolicyStartDate = DBDTOTransformer.Instance.GetPolicyStartDate(Policy.MWStartDate, Policy.PolicySoldDate, Vd.MakeId, Vd.ModelId,
                        Policy.DealerId, Policy.ExtensionTypeId, Policy.MWIsAvailable);
                    //pr.PolicyEndDate = Policy.PolicyEndDate;
                    pr.PolicyEndDate = DBDTOTransformer.Instance.GetPolicyEndDate(Policy.MWStartDate, Policy.PolicySoldDate, Vd.MakeId, Vd.ModelId,
                        Policy.DealerId, Policy.ExtensionTypeId, Policy.MWIsAvailable);
                    pr.Discount = pr.Premium * Policy.Discount / 100;
                    pr.DiscountPercentage = pr.Discount;
                    //premiumDetails.TotalPremium / rate * policyDetails.payment.discount / 100
                    pr.PolicyBundleId = Policy.PolicyBundleId;
                    pr.TransferFee = Policy.TransferFee;
                    pr.ForwardComment = Policy.ForwardComment;
                    pr.BordxId = Policy.BordxId;
                    pr.Month = Policy.Month;
                    pr.Year = Policy.Year;
                    pr.BookletNumber = Policy.BookletNumber;
                    pr.MWStartDate = Policy.MWStartDate;
                    pr.MWIsAvailable = Policy.MWIsAvailable;
                    pr.DealerPolicy = Policy.DealerPolicy;
                    pr.ContractExtensionPremiumId = Policy.CoverTypeId;
                    pr.ContractExtensionsId = Policy.ExtensionTypeId;
                    pr.ContractInsuaranceLimitationId = Policy.ContractInsuaranceLimitationId;
                    if (IsGuid(Policy.TPABranchId.ToString()))
                    {
                        pr.TPABranchId = Policy.TPABranchId;
                    }

                    v.VehicleId = Policy.ItemId;
                    v.PolicyId = pr.Id;

                }
                else if (oip != null)
                {

                    OtherItemDetails Oid = session.Query<OtherItemDetails>().Where(a => a.Id == oip.OtherItemId).FirstOrDefault();
                    session.Load(pr, Policy.Id);
                    pr.Id = Policy.Id;
                    pr.ContractId = Policy.ContractId;
                    pr.CoverTypeId = Policy.CoverTypeId;
                    pr.ExtensionTypeId = Policy.ExtensionTypeId;
                    pr.Premium = currencyEm.ConvertToBaseCurrency(Policy.Premium, Policy.PremiumCurrencyTypeId, currentCurrencyPeriodId);
                    pr.PremiumCurrencyTypeId = Policy.PremiumCurrencyTypeId;
                    pr.ProductId = Policy.ProductId;
                    pr.Comment = Policy.Comment;
                    pr.CommodityTypeId = Policy.CommodityTypeId;
                    pr.ContractId = Policy.ContractId;
                    pr.PremiumCurrencyTypeId = Policy.PremiumCurrencyTypeId;
                    pr.DealerPaymentCurrencyTypeId = Policy.DealerPaymentCurrencyTypeId;
                    pr.CustomerPaymentCurrencyTypeId = Policy.CustomerPaymentCurrencyTypeId;
                    pr.CustomerId = Policy.CustomerId;
                    pr.CustomerPayment = currencyEm.ConvertToBaseCurrency(Policy.CustomerPayment, Policy.PremiumCurrencyTypeId, currentCurrencyPeriodId);
                    pr.DealerId = Policy.DealerId;
                    pr.DealerLocationId = Policy.DealerLocationId;
                    pr.DealerPayment = currencyEm.ConvertToBaseCurrency(Policy.DealerPayment, Policy.PremiumCurrencyTypeId, currentCurrencyPeriodId);
                    pr.LocalCurrencyConversionRate = currencyEm.GetLocalCurrencyConversionRate(Policy.ContractId,
                        currentCurrencyPeriodId);
                    pr.ExtensionTypeId = Policy.ExtensionTypeId;
                    pr.HrsUsedAtPolicySale = Policy.HrsUsedAtPolicySale;
                    pr.IsPartialPayment = Policy.IsPartialPayment;
                    pr.IsPreWarrantyCheck = Policy.IsPreWarrantyCheck;
                    pr.IsSpecialDeal = Policy.IsSpecialDeal;
                    pr.ProductId = Policy.ProductId;
                    pr.ApprovedDate = Policy.ApprovedDate;
                    if (IsGuid(Policy.PaymentModeId.ToString()))
                    {
                        pr.PaymentModeId = Policy.PaymentModeId;
                    }

                    if (IsGuid(Policy.PaymentTypeId.ToString()))
                    {
                        pr.PaymentTypeId = Policy.PaymentTypeId;
                    }

                    pr.PolicyNo = Policy.PolicyNo;
                    pr.PolicySoldDate = Policy.PolicySoldDate;
                    //pr.Premium = Policy.Premium;
                    pr.RefNo = Policy.RefNo;
                    pr.SalesPersonId = Policy.SalesPersonId;
                    if (IsGuid(Policy.CurrencyPeriodId.ToString()))
                    {
                        pr.CurrencyPeriodId = Policy.CurrencyPeriodId;
                    }

                    //  pr.EntryUser = Policy.EntryUser;
                    pr.IsApproved = Policy.IsApproved;
                    pr.IsPolicyCanceled = Policy.IsPolicyCanceled;
                    pr.PolicyStartDate = Policy.PolicyStartDate;
                    //pr.PolicyStartDate = DBDTOTransformer.Instance.GetPolicyStartDate(Policy.PolicyStartDate, Policy.PolicySoldDate, Oid.MakeId, Oid.ModelId,
                    //    Policy.DealerId, Policy.ExtensionTypeId, Policy.MWIsAvailable);
                    pr.PolicyEndDate = Policy.PolicyEndDate;
                    //pr.PolicyEndDate = DBDTOTransformer.Instance.GetPolicyEndDate(Policy.MWStartDate, Policy.PolicySoldDate, Oid.MakeId, Oid.ModelId,
                    //    Policy.DealerId, Policy.ExtensionTypeId, Policy.MWIsAvailable);
                    pr.Discount = pr.Premium * Policy.Discount / 100;
                    pr.DiscountPercentage = pr.Discount;
                    //premiumDetails.TotalPremium / rate * policyDetails.payment.discount / 100
                    pr.PolicyBundleId = Policy.PolicyBundleId;
                    pr.TransferFee = Policy.TransferFee;
                    pr.ForwardComment = Policy.ForwardComment;
                    pr.BordxId = Policy.BordxId;
                    pr.Month = Policy.Month;
                    pr.Year = Policy.Year;
                    pr.BookletNumber = Policy.BookletNumber;
                    pr.MWStartDate = new DateTime(1900, 10, 10);//   Convert.ToDateTime("1900/10/10");

                    pr.MWIsAvailable = Policy.MWIsAvailable;
                    pr.DealerPolicy = Policy.DealerPolicy;
                    pr.ContractExtensionPremiumId = Policy.CoverTypeId;
                    pr.ContractExtensionsId = Policy.ContractExtensionsId;
                    pr.ContractInsuaranceLimitationId = Policy.ContractInsuaranceLimitationId;
                    if (IsGuid(Policy.TPABranchId.ToString()))
                    {
                        pr.TPABranchId = Policy.TPABranchId;
                    }

                    o.OtherItemId = Policy.ItemId;
                    o.PolicyId = pr.Id;
                }



                //v.VehicleId = Policy.ItemId;
                //v.PolicyId = pr.Id;

                //b.BAndWId = Policy.ItemId;
                //b.PolicyId = pr.Id;
                expObj = pr;
                using (ITransaction transaction = session.BeginTransaction())
                {
                    if (policyApproveDateWantToLoad) {
                        Policy existingPolicy = session.Query<Policy>().Where(a => a.Id == pr.Id).FirstOrDefault();
                        if (existingPolicy != null)
                        {
                            pr.ApprovedDate = existingPolicy.ApprovedDate;
                        }
                        else
                        {
                            pr.ApprovedDate = DateTime.UtcNow;
                        }
                    }

                    session.Update(pr);
                    var queryV = from vp in session.Query<VehiclePolicy>()
                                 where vp.PolicyId == Policy.Id
                                 select new { Id = vp };
                    var queryB = from bp in session.Query<BAndWPolicy>()
                                 where bp.PolicyId == Policy.Id
                                 select new { Id = bp };
                    var queryC = from op in session.Query<OtherItemPolicy>()
                                 where op.PolicyId == Policy.Id
                                 select new { Id = op };

                    if (Policy.Type == "Vehicle")
                    {
                        if (queryV.ToList().Count == 0)
                        {
                            v.Id = new Guid();
                            session.SaveOrUpdate(v);
                            if (queryB.ToList().Count > 0)
                            {
                                session.Delete(b);
                            }
                        }
                        else
                        {
                            v = queryV.ToList().First().Id;
                            session.Update(v);
                            if (queryB.ToList().Count > 0)
                            {
                                session.Delete(b);
                            }
                        }
                    }

                    else if (Policy.Type == "B&W")
                    {
                        if (queryC.ToList().Count == 0)
                        {
                            o.Id = new Guid();
                            session.SaveOrUpdate(o);
                            //if (queryV.ToList().Count > 0)
                            //{
                            //    session.Delete(v);
                            //}
                        }
                        else
                        {
                            o = queryC.ToList().First().Id;
                            session.Update(o);
                            //if (queryV.ToList().Count > 0)
                            //{
                            //    session.Delete(v);
                            //}
                        }
                        //if (queryB.ToList().Count == 0)
                        //{
                        //    b.Id = new Guid();
                        //    session.SaveOrUpdate(b);
                        //    if (queryV.ToList().Count > 0)
                        //    {
                        //        session.Delete(v);
                        //    }
                        //}
                        //else
                        //{
                        //    b = queryB.ToList().First().Id;
                        //    session.Update(b);
                        //    if (queryV.ToList().Count > 0)
                        //    {
                        //        session.Delete(v);
                        //    }
                        //}
                    }
                    else if (Policy.Type == "Other")
                    {
                        if (queryC.ToList().Count == 0)
                        {
                            o.Id = new Guid();
                            session.SaveOrUpdate(o);
                            //if (queryV.ToList().Count > 0)
                            //{
                            //    session.Delete(v);
                            //}
                        }
                        else
                        {
                            o = queryC.ToList().First().Id;
                            session.Update(o);
                            //if (queryV.ToList().Count > 0)
                            //{
                            //    session.Delete(v);
                            //}
                        }
                    }
                    transaction.Commit();
                }


                return true;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ((ex.InnerException != null) ? ex.InnerException.ToString() : "") + " , object : " + expObj);
                logger.Error("Inner exception :" + ((ex.InnerException != null) ? ex.InnerException.ToString() : ""));
                logger.Error("object : " + expObj);
                return false;
            }
        }

        internal static bool saveOnlinePurchase(OnlinePurchaseRequestDto Policy)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();
                StagOnlinePurchase sop = new Entities.StagOnlinePurchase();

                using (ITransaction transaction = session.BeginTransaction())
                {
                    sop.StagOnlinePurchaseID = Policy.StagOnlinePurchaseID;
                    sop.PurchaseDate = DateTime.Today.ToUniversalTime();
                    sop.CustomerID = Policy.CustomerID;
                    sop.DealerID = Policy.DealerID;
                    sop.DealerLocationId = Policy.DealerLocationId;
                    sop.VINNo = Policy.VINNo;
                    sop.MakeID = Policy.MakeID;
                    sop.ModelID = Policy.ModelID;
                    sop.Status = Policy.Status;
                    sop.ModelYear = Policy.ModelYear;
                    sop.VarientID = Policy.VarientID;
                    sop.EngineCapacityId = Policy.EngineCapacityId;
                    sop.CeylinderCountID = Policy.CeylinderCountID;
                    sop.FuelTypeID = Policy.FuelTypeID;
                    sop.TransmissionID = Policy.TransmissionID;
                    sop.DriveTypeID = Policy.DriveTypeID;
                    sop.BodyTypeID = Policy.BodyTypeID;
                    sop.AspirationID = Policy.AspirationID;
                    sop.VehiclePurchaseDate = Policy.VehiclePurchaseDate;
                    sop.PlateNo = Policy.PlateNo;
                    sop.VehiclePrice = Policy.VehiclePrice;
                    sop.ExtensionTypeID = Policy.ExtensionTypeID;
                    sop.MilageAtPolicySale = Policy.MilageAtPolicySale;
                    sop.ItemCategoryID = Policy.ItemCategoryID;
                    sop.SerialNo = Policy.SerialNo;
                    sop.InvoiceNo = Policy.InvoiceNo;
                    sop.AddnSerialNo = Policy.AddnSerialNo;
                    sop.ItemPrice = Policy.ItemPrice;
                    sop.HrsUsedAtPolicySale = Policy.HrsUsedAtPolicySale;
                    sop.ContractID = Policy.ContractID;
                    sop.StartDate = DateTime.Today.ToUniversalTime();
                    sop.EndDate = DateTime.Today.ToUniversalTime();
                    sop.SelectedPremium = Policy.SelectedPremium;
                    sop.VehicleCategoryID = Policy.VehicleCategoryID;

                    session.SaveOrUpdate(sop);

                    transaction.Commit();

                    return true;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ((ex.InnerException != null) ? ex.InnerException.ToString() : ""));
                throw;
            }
        }

        internal string SavePolicy(SavePolicyRequestDto savePolicyRequest, String dbName)
        {
            string response = string.Empty;
            #region "crap"

            ////avoid datetime errors
            //if (SavePolicyRequest.policyDetails.customer.idIssueDate == null ||
            //    SavePolicyRequest.policyDetails.customer.idIssueDate < SqlDateTime.MinValue.Value)
            //    SavePolicyRequest.policyDetails.customer.idIssueDate = SqlDateTime.MinValue.Value;
            //if (SavePolicyRequest.policyDetails.customer.dateOfBirth == null ||
            //    SavePolicyRequest.policyDetails.customer.dateOfBirth < SqlDateTime.MinValue.Value)
            //    SavePolicyRequest.policyDetails.customer.dateOfBirth = SqlDateTime.MinValue.Value;
            ////currency check
            //string validateComment = ValidateCurrencyPeriodOnPolicySave(SavePolicyRequest);
            //if (validateComment != "ok")
            //    return validateComment;

            //#region "Customer"
            //var customerEM = new CustomerEntityManager();
            //CustomerRequestDto customer = DBDTOTransformer.Instance.PolicyCustomerToCustomerEntity(SavePolicyRequest.policyDetails.customer);
            //bool customerStatus = false;
            //if (IsGuid(SavePolicyRequest.policyDetails.customer.customerId.ToString()))
            //{
            //    //existing customer
            //    customerStatus = customerEM.UpdateCustomerInPolicy(SavePolicyRequest.policyDetails.customer);
            //    customerStatus = true;
            //}
            //else
            //{
            //    //new customer
            //    customer.Id = Guid.NewGuid().ToString();
            //    //SystemUserEntityManager sue = new SystemUserEntityManager().
            //    customerStatus = customerEM.AddCustomer(customer);
            //}
            //#endregion "Customer"
            //if (!customerStatus)
            //    return "Error occured while saving/updating customer";
            //#region "Product"
            //var commodityCode = new CommodityEntityManager()
            //    .GetCommodityById(SavePolicyRequest.policyDetails.product.commodityTypeId)
            //    .CommodityCode;
            //bool productStatus = false;
            //Guid ItemId = Guid.Empty;
            //if (IsGuid(SavePolicyRequest.policyDetails.product.id.ToString()))
            //{
            //    //existing product
            //    if (commodityCode == "A")
            //    {
            //        var VehicleEM = new VehicleDetailsEntityManager();
            //        VehicleDetailsRequestDto vehicleRequest = DBDTOTransformer.Instance.PolicyVehicleToVehicleEntity(SavePolicyRequest.policyDetails.product, SavePolicyRequest.policyDetails.policy);
            //        productStatus = VehicleEM.UpdateVehicleDetails(vehicleRequest);
            //        ItemId = vehicleRequest.Id;
            //    }
            //    else if (commodityCode == "E")
            //    {
            //        var BandWEM = new BrownAndWhiteDetailsEntityManager();
            //        BrownAndWhiteDetailsRequestDto BnWRequest = DBDTOTransformer.Instance.PolicyBnWToBnWEntity(SavePolicyRequest.policyDetails.product, SavePolicyRequest.policyDetails.policy);
            //        productStatus = BandWEM.UpdateBrownAndWhiteDetails(BnWRequest);
            //        ItemId = BnWRequest.Id;
            //    }
            //    else if (commodityCode == "Y")
            //    {
            //        var YellowGoodEM = new YellowGoodsEntityManager();
            //        YellowGoodRequestDto YellowGoodRequest = DBDTOTransformer.Instance.PolicyYellowGoodToYellowGoodEntity(SavePolicyRequest.policyDetails.product, SavePolicyRequest.policyDetails.policy);
            //        productStatus = YellowGoodEM.UpdateYellowGoodDetails(YellowGoodRequest);
            //        ItemId = YellowGoodRequest.Id;
            //    }
            //    else if (commodityCode == "O")
            //    {
            //        var OtherItemEM = new OtherItemEntityManager();
            //        OtherItemRequestDto OtherItemRequest = DBDTOTransformer.Instance.PolicyOtherItemToOtherItemEntity(SavePolicyRequest.policyDetails.product, SavePolicyRequest.policyDetails.policy);
            //        productStatus = OtherItemEM.UpdateOtherItemDetails(OtherItemRequest);
            //        ItemId = OtherItemRequest.Id;
            //    }

            //}
            //else
            //{
            //    //new product
            //    if (commodityCode == "A")
            //    {
            //        var VehicleEM = new VehicleDetailsEntityManager();
            //        VehicleDetailsRequestDto vehicleRequest = DBDTOTransformer.Instance.PolicyVehicleToVehicleEntity(SavePolicyRequest.policyDetails.product, SavePolicyRequest.policyDetails.policy);
            //        vehicleRequest.EntryDateTime = DateTime.Today.ToUniversalTime();
            //        productStatus = VehicleEM.AddVehicleDetails(vehicleRequest);
            //        ItemId = vehicleRequest.Id;
            //    }
            //    else if (commodityCode == "E")
            //    {
            //        var BandWEM = new BrownAndWhiteDetailsEntityManager();
            //        BrownAndWhiteDetailsRequestDto BnWRequest = DBDTOTransformer.Instance.PolicyBnWToBnWEntity(SavePolicyRequest.policyDetails.product, SavePolicyRequest.policyDetails.policy);
            //        BnWRequest.EntryDateTime = DateTime.Today.ToUniversalTime();
            //        productStatus = BandWEM.AddBrownAndWhiteDetails(BnWRequest);
            //        ItemId = BnWRequest.Id;
            //    }
            //    else if (commodityCode == "Y")
            //    {
            //        var YellowGoodEM = new YellowGoodsEntityManager();
            //        YellowGoodRequestDto YellowGoodRequest = DBDTOTransformer.Instance.PolicyYellowGoodToYellowGoodEntity(SavePolicyRequest.policyDetails.product, SavePolicyRequest.policyDetails.policy);
            //        YellowGoodRequest.EntryDateTime = DateTime.Today.ToUniversalTime();
            //        productStatus = YellowGoodEM.AddYellowGoodDetails(YellowGoodRequest);
            //        ItemId = YellowGoodRequest.Id;
            //    }
            //    else if (commodityCode == "O")
            //    {
            //        var OtherItemEM = new OtherItemEntityManager();
            //        OtherItemRequestDto OtherItemRequest = DBDTOTransformer.Instance.PolicyOtherItemToOtherItemEntity(SavePolicyRequest.policyDetails.product, SavePolicyRequest.policyDetails.policy);
            //        OtherItemRequest.EntryDateTime = DateTime.Today.ToUniversalTime();
            //        productStatus = OtherItemEM.AddOtherItemDetails(OtherItemRequest);
            //        ItemId = OtherItemRequest.Id;
            //    }
            //}
            //#endregion "Product"
            //if (!productStatus)
            //    return "Error occured while saving/updating product";

            //#region "Policy Bundle"
            //bool policyBundleStatus = false;
            //PolicyBundleRequestDto PolicyBundleRequest = DBDTOTransformer.Instance.PolicyDetailsToPolicyBundle(SavePolicyRequest.policyDetails.policy, SavePolicyRequest.policyDetails.payment, SavePolicyRequest.policyDetails.product, Guid.Empty);
            //PolicyBundleRequest.CustomerId = Guid.Parse(customer.Id);
            //PolicyBundleRequest.ItemId = ItemId;
            //PolicyBundleRequest.Id = Guid.NewGuid();
            //policyBundleStatus = AddPolicyBundle(PolicyBundleRequest);

            //#endregion "Policy Bundle"
            //if (!policyBundleStatus)
            //    return "Error occured while saving policy bundle";

            //#region "Policy"
            //bool policyStatus = false;
            //List<PolicyRequestDto> PolicyRequestList = DBDTOTransformer.Instance.PolicyDetailsToPolicy(SavePolicyRequest.policyDetails.policy, SavePolicyRequest.policyDetails.payment, SavePolicyRequest.policyDetails.product);
            //foreach (PolicyRequestDto policy in PolicyRequestList)
            //{
            //    if (commodityCode == "A")
            //    {
            //        policy.Type = "Vehicle";
            //    }
            //    else if (commodityCode == "E")
            //    {
            //        policy.Type = "B&W";
            //    }
            //    else if (commodityCode == "Y")
            //    {
            //        policy.Type = "YellowGood";
            //    }
            //    else if (commodityCode == "O")
            //    {
            //        policy.Type = "Other";
            //    }
            //    policy.CustomerId = Guid.Parse(customer.Id);
            //    policy.ItemId = ItemId;
            //    policy.PolicyBundleId = PolicyBundleRequest.Id;
            //    if (policy.DealerPolicy)
            //    {
            //        policy.TPABranchId = GetTpaBranchIdByDealerLocationId(policy.DealerLocationId);
            //    }
            //    else
            //    {
            //        if (!IsGuid(SavePolicyRequest.policyDetails.policy.tpaBranchId.ToString()))
            //        {
            //            policy.TPABranchId = GetTpaBranchIdByDealerLocationId(policy.DealerLocationId);
            //        }
            //        else
            //        {
            //            policy.TPABranchId = SavePolicyRequest.policyDetails.policy.tpaBranchId;
            //        }
            //    }
            //    policyStatus = AddPolicy(policy);
            //    if (!policyStatus)
            //        return "Error occured while saving policy";
            //}

            //if (PolicyBundleRequest.IsApproved)
            //{
            //    try
            //    {
            //        new ReportsForSend().SendPolicyStatementAndBooklate(PolicyBundleRequest, dbName);
            //    }
            //    catch (Exception ex)
            //    {
            //        logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ((ex.InnerException != null) ? ex.InnerException.ToString() : ""));
            //    }
            //}
            //#endregion "Policy"

            //#region PolicyAttachment
            //ISession session = EntitySessionManager.GetSession();
            //try
            //{
            //    foreach (Guid AttachmentId in SavePolicyRequest.policyDetails.policyDocIds)
            //    {
            //        PolicyAttachment policyAttachment = new PolicyAttachment()
            //        {
            //            Id = Guid.NewGuid(),
            //            PolicyBundleId = PolicyBundleRequest.Id,
            //            UserAttachmentId = AttachmentId
            //        };

            //        using (ITransaction transaction = session.BeginTransaction())
            //        {
            //            session.Save(policyAttachment, policyAttachment.Id);
            //            transaction.Commit();
            //        }

            //    }
            //}
            //catch (Exception ex)
            //{
            //    logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ((ex.InnerException != null) ? ex.InnerException.ToString() : ""));
            //    return "Policy attachment updating failed";
            //}

            //#endregion

            #endregion "crap"
            try
            {
                if (savePolicyRequest == null || savePolicyRequest.policyDetails == null ||
                    savePolicyRequest.policyDetails.product == null
                    || savePolicyRequest.policyDetails.customer == null ||
                    savePolicyRequest.policyDetails.payment == null)
                {
                    return "Input data invalid";
                }

                Guid CommodityTypeId = savePolicyRequest.policyDetails.product.commodityTypeId;
                var commodityCode = new CommonEntityManager().GetCommodityTypeUniqueCodeById(CommodityTypeId);
                //validate usage
                if (decimal.Parse(savePolicyRequest.policyDetails.policy.hrsUsedAtPolicySale) < decimal.Zero)
                {
                    if (commodityCode.ToLower() == "a")
                    {
                        return "KM at Policy Sale cannot be zero.";
                    }
                    else if (commodityCode.ToLower() == "b")
                    {
                        return "KM at Policy Sale cannot be zero.";
                    }
                    else
                    {
                        return "Usage at Policy Sale cannot be zero.";
                    }
                }


                ISession session = EntitySessionManager.GetSession();
                // eligibility validation
                foreach (var contractData in savePolicyRequest.policyDetails.policy.productContracts)
                {
                    var premiumDetails = new ContractEntityManager().GetPremium(contractData.CoverTypeId,
                        decimal.Parse(savePolicyRequest.policyDetails.policy.hrsUsedAtPolicySale), contractData.AttributeSpecificationId,
                        contractData.ExtensionTypeId,
                        contractData.ContractId, savePolicyRequest.policyDetails.product.productId, savePolicyRequest.policyDetails.product.dealerId,
                        savePolicyRequest.policyDetails.policy.policySoldDate,
                        savePolicyRequest.policyDetails.product.cylinderCountId, savePolicyRequest.policyDetails.product.engineCapacityId,
                        savePolicyRequest.policyDetails.product.makeId, savePolicyRequest.policyDetails.product.modelId,
                        savePolicyRequest.policyDetails.product.variantId, savePolicyRequest.policyDetails.product.grossWeight,
                        savePolicyRequest.policyDetails.product.itemStatusId, savePolicyRequest.policyDetails.product.dealerPrice,
                        savePolicyRequest.policyDetails.product.itemPurchasedDate) as GetPremiumResponseDto;

                    if (premiumDetails.Status != "ok")
                    {
                        return premiumDetails.Status;
                    }
                }
                //policy history validation
                Guid productId = savePolicyRequest.policyDetails.product.productId;
                Guid itemId = savePolicyRequest.policyDetails.product.id;

                if (IsGuid(productId.ToString()) && IsGuid(itemId.ToString())
                    && IsGuid(CommodityTypeId.ToString()))
                {

                    //getting all the active policies for this vehicle and product
                    if (commodityCode.ToLower() == "a")
                    {
                        var existingPolicy = session.Query<VehiclePolicy>()
                            .Join(session.Query<Policy>(), m => m.PolicyId, n => n.Id, (m, n) => new { m, n })
                            .Join(session.Query<Customer>(), o => o.n.CustomerId, p => p.Id, (o, p) => new { o, p })
                            .Where(q => q.o.n.CommodityTypeId == CommodityTypeId && q.o.m.VehicleId == itemId
                            && q.o.n.ProductId == productId && q.o.n.IsPolicyCanceled != true
                            && q.o.n.IsPolicyRenewed != true);

                        if (existingPolicy.Count() > 0)
                        {
                            return "Entered vehicle already has an active policy against selected product.";
                        }
                    }
                    else if (commodityCode.ToLower() == "b")
                    {
                        var existingPolicy = session.Query<VehiclePolicy>()
                            .Join(session.Query<Policy>(), m => m.PolicyId, n => n.Id, (m, n) => new { m, n })
                            .Join(session.Query<Customer>(), o => o.n.CustomerId, p => p.Id, (o, p) => new { o, p })
                            .Where(q => q.o.n.CommodityTypeId == CommodityTypeId && q.o.m.VehicleId == itemId
                            && q.o.n.ProductId == productId && q.o.n.IsPolicyCanceled != true
                            && q.o.n.IsPolicyRenewed != true);

                        if (existingPolicy.Count() > 0)
                        {
                            return "Entered vehicle already has an active policy against selected product.";
                        }
                    }
                    else if (commodityCode.ToLower() == "e")
                    {
                        var existingPolicy = session.Query<BAndWPolicy>()
                            .Join(session.Query<Policy>(), m => m.PolicyId, n => n.Id, (m, n) => new { m, n })
                            .Join(session.Query<Customer>(), o => o.n.CustomerId, p => p.Id, (o, p) => new { o, p })
                            .Where(q => q.o.n.CommodityTypeId == CommodityTypeId && q.o.m.BAndWId == itemId
                            && q.o.n.ProductId == productId && q.o.n.IsPolicyCanceled != true
                            && q.o.n.IsPolicyRenewed != true);

                        if (existingPolicy.Count() > 0)
                        {
                            return "Entered item already has an active policy against selected product.";
                        }
                    }
                    else if (commodityCode.ToLower() == "o")
                    {
                        var existingPolicy = session.Query<OtherItemPolicy>()
                            .Join(session.Query<Policy>(), m => m.PolicyId, n => n.Id, (m, n) => new { m, n })
                            .Join(session.Query<Customer>(), o => o.n.CustomerId, p => p.Id, (o, p) => new { o, p })
                            .Where(q => q.o.n.CommodityTypeId == CommodityTypeId && q.o.m.OtherItemId == itemId
                            && q.o.n.ProductId == productId && q.o.n.IsPolicyCanceled != true
                            && q.o.n.IsPolicyRenewed != true);

                        if (existingPolicy.Count() > 0)
                        {
                            return "Entered item already has an active policy against selected product.";
                        }
                    }
                    else if (commodityCode.ToLower() == "y")
                    {
                        var existingPolicy = session.Query<YellowGoodPolicy>()
                            .Join(session.Query<Policy>(), m => m.PolicyId, n => n.Id, (m, n) => new { m, n })
                            .Join(session.Query<Customer>(), o => o.n.CustomerId, p => p.Id, (o, p) => new { o, p })
                            .Where(q => q.o.n.CommodityTypeId == CommodityTypeId && q.o.m.YellowGoodId == itemId
                            && q.o.n.ProductId == productId && q.o.n.IsPolicyCanceled != true
                            && q.o.n.IsPolicyRenewed != true);

                        if (existingPolicy.Count() > 0)
                        {
                            return "Entered item already has an active policy against selected product.";
                        }
                    }
                }


                Customer customerInfo = DBDTOTransformer.Instance.ConvertPolicyCustomerToCustomerEntity(savePolicyRequest.policyDetails.customer, savePolicyRequest.policyDetails.loggedInUserId);
                var product = DBDTOTransformer.Instance.ConvertPolicyProductToProductEntity(savePolicyRequest.policyDetails.product, savePolicyRequest.policyDetails.loggedInUserId);



                PolicyBundle policyBundle = DBDTOTransformer.Instance.ConvertPolicyBundleToPolicyBundleEntity(savePolicyRequest.policyDetails);
                List<Policy> policyList = DBDTOTransformer.Instance.ConvertPolicyListToPolicyListEntity(savePolicyRequest.policyDetails);


                //Product products = session.Query<Product>().FirstOrDefault();
                //ProductType productType = session.Query<ProductType>().Where(a => a.Id == products.ProductTypeId).FirstOrDefault();
                //List<PolicyAttachment> attachments = new List<PolicyAttachment>();
                //if (productType.Code == "ILOE")
                //{
                //    if (customerInfo.Id != Guid.Empty)
                //    {
                //        var query = from customer in session.Query<Customer>()
                //                    join policy in session.Query<Policy>() on customer.Id equals policy.CustomerId
                //                    where customer.Id == customerInfo.Id
                //                    select new { customer = customer, policy = policy };

                //        var result3 = query.OrderByDescending(a => a.policy.PolicyNo);
                //        var result2 = result3.First();

                //        List<PolicyAttachment> policyAttachment = session.Query<PolicyAttachment>()
                //            .Where(a => a.PolicyBundleId == result2.policy.PolicyBundleId).ToList();

                //        attachments = policyAttachment;

                //        //foreach (var policyatt in policyAttachment)
                //        //{
                //        //    var attachment = new PolicyAttachment()
                //        //    {
                //        //        Id = Guid.NewGuid(),
                //        //        PolicyBundleId = policyBundle.Id,
                //        //        UserAttachmentId = policyatt.UserAttachmentId
                //        //    };
                //        //    policyAttachment.Add(attachment);
                //        //}

                //    }
                //}

                policyBundle.CustomerId = customerInfo.Id;

                List<VehiclePolicy> vehiclePolicies = new List<VehiclePolicy>();
                List<OtherItemPolicy> otherItemDetailsPolicies = new List<OtherItemPolicy>();
                List<YellowGoodPolicy> yellowGoodPolicies = new List<YellowGoodPolicy>();
                List<BAndWPolicy> bAndWPolicies = new List<BAndWPolicy>();


                foreach (var policy in policyList)
                {
                    bool exceedDbLength = false;
                    string currencyCode = new CommonEntityManager().GetCurrencyCodeById(policy.DealerPaymentCurrencyTypeId);
                    if (product.code == "A")
                    {
                        var vDetails = product.obj as VehicleDetails;
                        if (Math.Truncate(vDetails.DealerPrice).ToString().Length > 10)
                        {
                            exceedDbLength = true;
                        }

                        var vpolicy = new VehiclePolicy()
                        {
                            Id = Guid.NewGuid(),
                            PolicyId = policy.Id,
                            VehicleId = vDetails.Id
                        };
                        vehiclePolicies.Add(vpolicy);
                    }
                    else if (product.code == "B")
                    {
                        var vDetails = product.obj as VehicleDetails;
                        if (Math.Truncate(vDetails.DealerPrice).ToString().Length > 10)
                        {
                            exceedDbLength = true;
                        }

                        var vpolicy = new VehiclePolicy()
                        {
                            Id = Guid.NewGuid(),
                            PolicyId = policy.Id,
                            VehicleId = vDetails.Id
                        };
                        vehiclePolicies.Add(vpolicy);
                    }
                    else if (product.code == "E")
                    {
                        var bDetails = product.obj as BrownAndWhiteDetails;
                        if (Math.Truncate(bDetails.DealerPrice).ToString().Length > 10)
                        {
                            exceedDbLength = true;
                        }

                        var bPolicy = new BAndWPolicy()
                        {
                            Id = Guid.NewGuid(),
                            PolicyId = policy.Id,
                            BAndWId = bDetails.Id
                        };
                        bAndWPolicies.Add(bPolicy);
                    }
                    else if (product.code == "O")
                    {
                        var oDetails = product.obj as OtherItemDetails;
                        if (Math.Truncate(oDetails.DealerPrice).ToString().Length > 10)
                        {
                            exceedDbLength = true;
                        }

                        var oPolicy = new OtherItemPolicy()
                        {
                            Id = Guid.NewGuid(),
                            PolicyId = policy.Id,
                            OtherItemId = oDetails.Id
                        };
                        otherItemDetailsPolicies.Add(oPolicy);
                    }
                    else
                    {
                        var yDetails = product.obj as YellowGoodDetails;
                        if (Math.Truncate(yDetails.DealerPrice).ToString().Length > 10)
                        {
                            exceedDbLength = true;
                        }

                        var yPolicy = new YellowGoodPolicy()
                        {
                            Id = Guid.NewGuid(),
                            PolicyId = policy.Id,
                            YellowGoodId = yDetails.Id
                        };
                        yellowGoodPolicies.Add(yPolicy);
                    }
                    ////validate vehicle price if exceed 9M USD (we are allowing 10 numbers to db)
                    if (exceedDbLength)
                    {
                        return "Item purchased price cannot exceed " + (9999999999 * policy.LocalCurrencyConversionRate).ToString("N", CultureInfo.InvariantCulture) + " " + currencyCode + ".";
                    }
                }
                string policyNumbers = string.Empty;
                Guid entryUser = Guid.Empty, approvedUser = Guid.Empty;
                using (ITransaction transaction = session.BeginTransaction(IsolationLevel.ReadCommitted))
                {
                    if (customerInfo.IDTypeId == 0)
                    {
                        customerInfo.IDTypeId = 1;//cannot nullable atm
                    }

                    if (customerInfo.Id == Guid.Empty)
                    {
                        session.Save(customerInfo, customerInfo.Id = Guid.NewGuid());
                    }
                    else
                    {
                        session.Clear();
                        session.Update(customerInfo);
                    }

                    policyBundle.CustomerId = customerInfo.Id;

                    session.Save(policyBundle, policyBundle.Id);
                    foreach (var policy in policyList)
                    {
                        policy.CustomerId = customerInfo.Id;
                        policy.PolicyBundleId = policyBundle.Id;

                        if (policy.TPABranchId == Guid.Empty)
                        {
                            policy.TPABranchId = session.Query<TPABranch>().FirstOrDefault(a => a.IsHeadOffice).Id;
                        }

                        TPA tpa = session.Query<TPA>().FirstOrDefault();

                        //add tpa Id
                        var policyNum = "";
                        //if (tpa.Name.ToLower() == "cycleandcarriage")
                        //{
                        //      policyNum = GetNextPolicyNumber(policy.TPABranchId, policy.DealerId,
                        //                   policy.ProductId, new CommonEntityManager().GetDealerCountryByDealerId(policy.DealerId),
                        //                   policyBundle.CommodityTypeId, tpa.Id);
                        //}
                        //else {
                            policyNum = GetNextPolicyNumber(savePolicyRequest.policyDetails.product.dealerLocationId, policy.DealerId,
                                          policy.ProductId, new CommonEntityManager().GetDealerCountryByDealerId(policy.DealerId),
                                          policyBundle.CommodityTypeId, tpa.Id);
                        //}

                        policy.PolicyNo = policyNum;
                        entryUser = policy.EntryUser;
                        approvedUser = policy.PolicyApprovedBy;
                        policyNumbers += policyNum + (policyNumbers.Length > 0 ? "," : string.Empty);

                        var PolicyNUM = session.Query<Policy>()
                        .OrderByDescending(a => a.SequenceNo).FirstOrDefault();
                        int sequenceNum = 0;
                        if (PolicyNUM == null)
                        {
                             sequenceNum = Convert.ToInt32("1");
                        }
                        else
                        {
                            sequenceNum = session.Query<Policy>()
                        .OrderByDescending(a => a.SequenceNo).FirstOrDefault().SequenceNo + 1;
                        }



                        policy.SequenceNo = sequenceNum;
                        policy.ApprovedDate = DateTime.UtcNow;
                        session.Evict(policy);
                        session.Save(policy, policy.Id);
                    }

                    if (product.code == "A")
                    {
                        VehicleDetails vd = product.obj as VehicleDetails;
                        if (vd.Id == Guid.Empty)
                        {
                            session.Save(vd, vd.Id = Guid.NewGuid());
                        }
                        else
                        {
                            session.Update(vd);
                        }

                        foreach (var vPolicy in vehiclePolicies)
                        {
                            vPolicy.VehicleId = vd.Id;
                            session.Evict(vPolicy);
                            session.Save(vPolicy, vPolicy.Id);
                        }

                        //if (productType.Code == "ILOE")
                        //{
                        //    foreach(var policyAttachment in attachments)
                        //    {
                        //        policyAttachment.Id = Guid.NewGuid();
                        //        policyAttachment.PolicyBundleId = policyBundle.Id;
                        //        session.Evict(policyAttachment);
                        //        session.Save(policyAttachment, policyAttachment.Id);
                        //    }
                        //}
                    }
                    else if (product.code == "B")
                    {
                        VehicleDetails vd = product.obj as VehicleDetails;
                        if (vd.Id == Guid.Empty)
                        {
                            session.Save(vd, vd.Id = Guid.NewGuid());
                        }
                        else
                        {
                            session.Update(vd);
                        }

                        foreach (var vPolicy in vehiclePolicies)
                        {
                            vPolicy.VehicleId = vd.Id;
                            session.Evict(vPolicy);
                            session.Save(vPolicy, vPolicy.Id);
                        }

                        //if (productType.Code == "ILOE")
                        //{
                        //    foreach(var policyAttachment in attachments)
                        //    {
                        //        policyAttachment.Id = Guid.NewGuid();
                        //        policyAttachment.PolicyBundleId = policyBundle.Id;
                        //        session.Evict(policyAttachment);
                        //        session.Save(policyAttachment, policyAttachment.Id);
                        //    }
                        //}
                    }
                    else if (product.code == "O")
                    {
                        OtherItemDetails od = product.obj as OtherItemDetails;
                        if (od.Id == Guid.Empty)
                        {
                            session.Save(od, od.Id = Guid.NewGuid());
                        }
                        else
                        {
                            session.Update(od);
                        }

                        foreach (var oPolicy in otherItemDetailsPolicies)
                        {
                            oPolicy.OtherItemId = od.Id;
                            session.Evict(oPolicy);
                            session.Save(oPolicy, oPolicy.Id);
                        }
                    }
                    else if (product.code == "E")
                    {
                        BrownAndWhiteDetails bd = product.obj as BrownAndWhiteDetails;
                        if (bd.Id == Guid.Empty)
                        {
                            session.Save(bd, bd.Id = Guid.NewGuid());
                        }
                        else
                        {
                            session.Update(bd);
                        }

                        foreach (var bPolicy in bAndWPolicies)
                        {
                            bPolicy.BAndWId = bd.Id;
                            session.Evict(bPolicy);
                            session.Save(bPolicy, bPolicy.Id);
                        }
                    }
                    else
                    {
                        YellowGoodDetails yd = product.obj as YellowGoodDetails;
                        if (yd.Id == Guid.Empty)
                        {
                            session.Save(yd, yd.Id = Guid.NewGuid());
                        }
                        else
                        {
                            session.Update(yd);
                        }

                        foreach (var yPolicy in yellowGoodPolicies)
                        {
                            yPolicy.YellowGoodId = yd.Id;
                            session.Evict(yPolicy);
                            session.Save(yPolicy, yPolicy.Id);
                        }
                    }

                    foreach (Guid AttachmentId in savePolicyRequest.policyDetails.policyDocIds)
                    {
                        PolicyAttachment policyAttachment = new PolicyAttachment()
                        {
                            Id = Guid.NewGuid(),
                            PolicyBundleId = policyBundle.Id,
                            UserAttachmentId = AttachmentId
                        };
                        session.Save(policyAttachment, policyAttachment.Id);
                    }

                    transaction.Commit();
                }
                if (policyBundle.IsApproved)
                {
                    try
                    {
                        var policyBundleRequest = new PolicyBundleRequestDto()
                        {
                            IsApproved = policyBundle.IsApproved,
                            Id = policyBundle.Id,
                            ProductId = policyBundle.ProductId,
                            CustomerId = policyBundle.CustomerId
                        };
                        new ReportsForSend().SendPolicyStatementAndBooklate(policyBundleRequest, dbName);
                    }
                    catch (Exception ex)
                    {
                        logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ((ex.InnerException != null) ? ex.InnerException.ToString() : ""));
                    }
                }


                if (savePolicyRequest.policyDetails.policy.dealerPolicy)
                {
                    //set notification
                    try
                    {
                        CommonEntityManager commonEntityManager = new CommonEntityManager();
                        var notificationUser = new List<UserDetail>()
                        {
                            new UserDetail()
                            {
                                tpaId = TASTPAEntityManager.GetTpaIdByName(dbName),
                                userId = entryUser
                            }
                        };
                        var notificationDto = new PushNotificationsRequestDto()
                        {
                            generatedTime = DateTime.UtcNow,
                            message = "Policy Number(s) " + policyNumbers + " got approved.",
                            link = "#",
                            messageFrom = commonEntityManager.GetUserNameById(approvedUser),
                            profilePic = commonEntityManager.GetProfilePictureByUserId(approvedUser),
                            userDetails = notificationUser
                        };
                        Task.Run(async () => await NotificationEntityManager.PushNotificationSender(notificationDto));

                    }
                    catch (Exception ex)
                    {
                        logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ((ex.InnerException != null) ? ex.InnerException.ToString() : ""));
                    }
                }
                response = "success";
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ((ex.InnerException != null) ? ex.InnerException.ToString() : ""));
                response = "Error occured while saving policy.";
            }
            return response;
        }

        private string ValidateCurrencyPeriodOnPolicySave(SavePolicyRequestDto SavePolicyRequest)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();
                foreach (ProductContract_ CurrentContract in SavePolicyRequest.policyDetails.policy.productContracts)
                {
                    // // eligibility validation
                    foreach (var contractData in SavePolicyRequest.policyDetails.policy.productContracts)
                    {
                        var premiumDetails = new ContractEntityManager().GetPremium(contractData.CoverTypeId,
                            decimal.Parse(SavePolicyRequest.policyDetails.policy.hrsUsedAtPolicySale), contractData.AttributeSpecificationId,
                            contractData.ExtensionTypeId,
                            contractData.ContractId, SavePolicyRequest.policyDetails.product.productId, SavePolicyRequest.policyDetails.product.dealerId,
                            SavePolicyRequest.policyDetails.policy.policySoldDate,
                            SavePolicyRequest.policyDetails.product.cylinderCountId, SavePolicyRequest.policyDetails.product.engineCapacityId,
                            SavePolicyRequest.policyDetails.product.makeId, SavePolicyRequest.policyDetails.product.modelId,
                            SavePolicyRequest.policyDetails.product.variantId, SavePolicyRequest.policyDetails.product.grossWeight,
                            SavePolicyRequest.policyDetails.product.itemStatusId, SavePolicyRequest.policyDetails.product.dealerPrice,
                            SavePolicyRequest.policyDetails.product.itemPurchasedDate) as GetPremiumResponseDto;

                        if (premiumDetails.Status != "ok")
                        {
                            return premiumDetails.Status;
                        }
                    }



                    if (CurrentContract == null)
                    {
                        return "Invalid Contract selection";
                    }

                    Contract contract = session.Query<Contract>()
                        .Where(a => a.Id == CurrentContract.ContractId).FirstOrDefault();
                    if (contract == null)
                    {
                        return "Invalid Contract selection";
                    }

                    ReinsurerContract reinsurerContract = session.Query<ReinsurerContract>().FirstOrDefault();
                    // .Where(a => a.Id == contract.ReinsurerId).FirstOrDefault();
                    if (reinsurerContract == null)
                    {
                        return "Reinsurer Contract not found";
                    }

                    Country country = session.Query<Country>()
                        .Where(a => a.Id == reinsurerContract.CountryId).FirstOrDefault();

                    if (country == null)
                    {
                        return "Country not found in reinsurer contract";
                    }

                    if (!IsGuid(country.CurrencyId.ToString()))
                    {
                        return "Currency is not defined for country " + country.CountryName;
                    }

                    //    Currency currency = session.Query<Currency>().Where(a => a.Id == country.CurrencyId).FirstOrDefault();
                    //    Guid currencyPeriodId = new ContractEntityManager().GetCurrencyPeriodByContractId(CurrentContract.ContractId);
                    //    CurrencyConversions conversion = session.Query<CurrencyConversions>()
                    //        .Where(a => a.CurrencyConversionPeriodId == currencyPeriodId && a.CurrencyId == country.CurrencyId).FirstOrDefault();
                    //    if (conversion == null)
                    //        return "Currency " + currency.CurrencyName + " is not found in the current conversion preiod";
                }


                return "ok";
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ((ex.InnerException != null) ? ex.InnerException.ToString() : ""));
                return "Error occured in currency validation";
            }

        }

        internal string SavePolicy(SavePolicyRequestDto savePolicyRequest, string dbName, Guid newPolicyId)
        {
            string response = string.Empty;
            #region "crap"
            //if (SavePolicyRequest.policyDetails.customer.idIssueDate == null ||
            //    SavePolicyRequest.policyDetails.customer.idIssueDate < SqlDateTime.MinValue.Value)
            //    SavePolicyRequest.policyDetails.customer.idIssueDate = SqlDateTime.MinValue.Value;
            //if (SavePolicyRequest.policyDetails.customer.dateOfBirth == null ||
            //    SavePolicyRequest.policyDetails.customer.dateOfBirth < SqlDateTime.MinValue.Value)
            //    SavePolicyRequest.policyDetails.customer.dateOfBirth = SqlDateTime.MinValue.Value;

            //string validateComment = ValidateCurrencyPeriodOnPolicySave(SavePolicyRequest);
            //if (validateComment != "ok")
            //    return validateComment;
            //ISession session = EntitySessionManager.GetSession();

            //#region "Customer"
            //var customerEM = new CustomerEntityManager();
            //CustomerRequestDto customer = DBDTOTransformer.Instance.PolicyCustomerToCustomerEntity(SavePolicyRequest.policyDetails.customer);
            //bool customerStatus = false;
            //if (IsGuid(SavePolicyRequest.policyDetails.customer.customerId.ToString()))
            //{
            //    //existing customer
            //    customerStatus = customerEM.UpdateCustomerInPolicy(SavePolicyRequest.policyDetails.customer);
            //    customerStatus = true;
            //}
            //else
            //{
            //    //new customer
            //    customer.Id = Guid.NewGuid().ToString();
            //    //SystemUserEntityManager sue = new SystemUserEntityManager().
            //    customerStatus = customerEM.AddCustomer(customer);
            //}
            //#endregion "Customer"
            //if (!customerStatus)
            //    return "Error occured while saving/updating customer";
            //#region "Product"
            //var commodityCode = new CommodityEntityManager()
            //    .GetCommodityById(SavePolicyRequest.policyDetails.product.commodityTypeId)
            //    .CommodityCode;
            //bool productStatus = false;
            //Guid ItemId = Guid.Empty;
            //if (IsGuid(SavePolicyRequest.policyDetails.product.id.ToString()))
            //{
            //    //existing product
            //    if (commodityCode == "A")
            //    {
            //        var VehicleEM = new VehicleDetailsEntityManager();
            //        VehicleDetailsRequestDto vehicleRequest = DBDTOTransformer.Instance.PolicyVehicleToVehicleEntity(SavePolicyRequest.policyDetails.product, SavePolicyRequest.policyDetails.policy);
            //        productStatus = VehicleEM.UpdateVehicleDetails(vehicleRequest);
            //        ItemId = vehicleRequest.Id;
            //    }
            //    else if (commodityCode == "E")
            //    {
            //        var BandWEM = new BrownAndWhiteDetailsEntityManager();
            //        BrownAndWhiteDetailsRequestDto BnWRequest = DBDTOTransformer.Instance.PolicyBnWToBnWEntity(SavePolicyRequest.policyDetails.product, SavePolicyRequest.policyDetails.policy);
            //        productStatus = BandWEM.UpdateBrownAndWhiteDetails(BnWRequest);
            //        ItemId = BnWRequest.Id;
            //    }
            //    else if (commodityCode == "Y")
            //    {
            //        var YellowGoodEM = new YellowGoodsEntityManager();
            //        YellowGoodRequestDto YellowGoodRequest = DBDTOTransformer.Instance.PolicyYellowGoodToYellowGoodEntity(SavePolicyRequest.policyDetails.product, SavePolicyRequest.policyDetails.policy);
            //        productStatus = YellowGoodEM.UpdateYellowGoodDetails(YellowGoodRequest);
            //        ItemId = YellowGoodRequest.Id;
            //    }
            //    else if (commodityCode == "O")
            //    {
            //        var OtherItemEM = new OtherItemEntityManager();
            //        OtherItemRequestDto OtherItemRequest = DBDTOTransformer.Instance.PolicyOtherItemToOtherItemEntity(SavePolicyRequest.policyDetails.product, SavePolicyRequest.policyDetails.policy);
            //        productStatus = OtherItemEM.UpdateOtherItemDetails(OtherItemRequest);
            //        ItemId = OtherItemRequest.Id;
            //    }

            //}
            //else
            //{
            //    //new product
            //    if (commodityCode == "A")
            //    {
            //        var VehicleEM = new VehicleDetailsEntityManager();
            //        VehicleDetailsRequestDto vehicleRequest = DBDTOTransformer.Instance.PolicyVehicleToVehicleEntity(SavePolicyRequest.policyDetails.product, SavePolicyRequest.policyDetails.policy);
            //        vehicleRequest.EntryDateTime = DateTime.Today.ToUniversalTime();
            //        productStatus = VehicleEM.AddVehicleDetails(vehicleRequest);
            //        ItemId = vehicleRequest.Id;
            //    }
            //    else if (commodityCode == "E")
            //    {
            //        var BandWEM = new BrownAndWhiteDetailsEntityManager();
            //        BrownAndWhiteDetailsRequestDto BnWRequest = DBDTOTransformer.Instance.PolicyBnWToBnWEntity(SavePolicyRequest.policyDetails.product, SavePolicyRequest.policyDetails.policy);
            //        BnWRequest.EntryDateTime = DateTime.Today.ToUniversalTime();
            //        productStatus = BandWEM.AddBrownAndWhiteDetails(BnWRequest);
            //        ItemId = BnWRequest.Id;
            //    }
            //    else if (commodityCode == "Y")
            //    {
            //        var YellowGoodEM = new YellowGoodsEntityManager();
            //        YellowGoodRequestDto YellowGoodRequest = DBDTOTransformer.Instance.PolicyYellowGoodToYellowGoodEntity(SavePolicyRequest.policyDetails.product, SavePolicyRequest.policyDetails.policy);
            //        YellowGoodRequest.EntryDateTime = DateTime.Today.ToUniversalTime();
            //        productStatus = YellowGoodEM.AddYellowGoodDetails(YellowGoodRequest);
            //        ItemId = YellowGoodRequest.Id;
            //    }
            //    else if (commodityCode == "O")
            //    {
            //        var OtherItemEM = new OtherItemEntityManager();
            //        OtherItemRequestDto OtherItemRequest = DBDTOTransformer.Instance.PolicyOtherItemToOtherItemEntity(SavePolicyRequest.policyDetails.product, SavePolicyRequest.policyDetails.policy);
            //        OtherItemRequest.EntryDateTime = DateTime.Today.ToUniversalTime();
            //        productStatus = OtherItemEM.AddOtherItemDetails(OtherItemRequest);
            //        ItemId = OtherItemRequest.Id;
            //    }
            //}
            //#endregion "Product"
            //if (!productStatus)
            //    return "Error occured while saving/updating product";

            //#region "Policy Bundle"
            //bool policyBundleStatus = false;
            //PolicyBundleRequestDto PolicyBundleRequest = DBDTOTransformer.Instance.PolicyDetailsToPolicyBundle(SavePolicyRequest.policyDetails.policy, SavePolicyRequest.policyDetails.payment, SavePolicyRequest.policyDetails.product, Guid.Empty);
            //PolicyBundleRequest.CustomerId = Guid.Parse(customer.Id);
            //PolicyBundleRequest.ItemId = ItemId;
            //PolicyBundleRequest.Id = newPolicyId;
            //policyBundleStatus = AddPolicyBundle(PolicyBundleRequest);

            //#endregion "Policy Bundle"
            //if (!policyBundleStatus)
            //    return "Error occured while saving policy bundle";

            //#region "Policy"
            //bool policyStatus = false;
            //List<PolicyRequestDto> PolicyRequestList = DBDTOTransformer.Instance.PolicyDetailsToPolicy(SavePolicyRequest.policyDetails.policy, SavePolicyRequest.policyDetails.payment, SavePolicyRequest.policyDetails.product);
            //foreach (PolicyRequestDto policy in PolicyRequestList)
            //{
            //    if (commodityCode == "A")
            //    {
            //        policy.Type = "Vehicle";
            //    }
            //    else if (commodityCode == "E")
            //    {
            //        policy.Type = "B&W";
            //    }
            //    else if (commodityCode == "Y")
            //    {
            //        policy.Type = "YellowGood";
            //    }
            //    else if (commodityCode == "O")
            //    {
            //        policy.Type = "Other";
            //    }
            //    policy.CustomerId = Guid.Parse(customer.Id);
            //    policy.ItemId = ItemId;
            //    policy.PolicyBundleId = PolicyBundleRequest.Id;
            //    if (policy.DealerPolicy)
            //    {
            //        policy.TPABranchId = GetTpaBranchIdByDealerLocationId(policy.DealerLocationId);
            //    }
            //    else
            //    {
            //        if (!IsGuid(SavePolicyRequest.policyDetails.policy.tpaBranchId.ToString()))
            //        {
            //            policy.TPABranchId = GetTpaBranchIdByDealerLocationId(policy.DealerLocationId);
            //        }
            //        else
            //        {
            //            policy.TPABranchId = SavePolicyRequest.policyDetails.policy.tpaBranchId;
            //        }
            //    }
            //    List<Policy> policyList = DBDTOTransformer.Instance.ConvertPolicyListToPolicyListEntity(SavePolicyRequest.policyDetails);

            //    using (ITransaction transaction = session.BeginTransaction(IsolationLevel.ReadCommitted))
            //    {
            //        foreach (var policyp in policyList)
            //        {

            //            if (policyp.TPABranchId == Guid.Empty)
            //            {
            //                policyp.TPABranchId = session.Query<TPABranch>().FirstOrDefault(a => a.IsHeadOffice).Id;
            //            }

            //            var policyNum = GetNextPolicyNumber(policyp.TPABranchId, policyp.DealerId,
            //                policyp.ProductId, new CommonEntityManager().GetDealerCountryByDealerId(policyp.DealerId));
            //            policyp.PolicyNo = policyNum;
            //            session.Evict(policyp);
            //            session.Save(policyp, policyp.Id);
            //            transaction.Commit();
            //            policyStatus = true;
            //        }

            //    }
            //    // policyStatus = AddPolicy(policy);
            //    if (!policyStatus)
            //        return "Error occured while saving policy";
            //}

            //if (PolicyBundleRequest.IsApproved)
            //{
            //    try
            //    {
            //        new ReportsForSend().SendPolicyStatementAndBooklate(PolicyBundleRequest, DBName);
            //    }
            //    catch (Exception ex)
            //    {
            //        logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ((ex.InnerException != null) ? ex.InnerException.ToString() : ""));
            //    }
            //}
            //#endregion "Policy"

            //#region PolicyAttachment
            ////ISession session = EntitySessionManager.GetSession();
            //try
            //{
            //    foreach (Guid AttachmentId in SavePolicyRequest.policyDetails.policyDocIds)
            //    {
            //        PolicyAttachment policyAttachment = new PolicyAttachment()
            //        {
            //            Id = Guid.NewGuid(),
            //            PolicyBundleId = PolicyBundleRequest.Id,
            //            UserAttachmentId = AttachmentId
            //        };

            //        using (ITransaction transaction = session.BeginTransaction())
            //        {
            //            session.Save(policyAttachment, policyAttachment.Id);
            //            transaction.Commit();
            //        }

            //    }
            //}
            //catch (Exception ex)
            //{
            //    logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ((ex.InnerException != null) ? ex.InnerException.ToString() : ""));
            //    return "Policy attachment updating failed";
            //}

            //#endregion
            #endregion "crap"
            //avoid datetime errors
            try
            {
                if (savePolicyRequest == null || savePolicyRequest.policyDetails == null ||
                    savePolicyRequest.policyDetails.product == null
                    || savePolicyRequest.policyDetails.customer == null ||
                    savePolicyRequest.policyDetails.payment == null)
                {
                    return "Input data invalid";
                }

                ISession session = EntitySessionManager.GetSession();

                Customer customerInfo = DBDTOTransformer.Instance.ConvertPolicyCustomerToCustomerEntity(savePolicyRequest.policyDetails.customer, savePolicyRequest.policyDetails.loggedInUserId);
                var product = DBDTOTransformer.Instance.ConvertPolicyProductToProductEntity(savePolicyRequest.policyDetails.product, savePolicyRequest.policyDetails.loggedInUserId);

                PolicyBundle policyBundle = DBDTOTransformer.Instance.ConvertPolicyBundleToPolicyBundleEntityRenewal(savePolicyRequest.policyDetails);
                List<Policy> policyList = DBDTOTransformer.Instance.ConvertPolicyListToPolicyListEntity(savePolicyRequest.policyDetails);

                policyBundle.CustomerId = customerInfo.Id;

                List<VehiclePolicy> vehiclePolicies = new List<VehiclePolicy>();
                List<OtherItemPolicy> otherItemDetailsPolicies = new List<OtherItemPolicy>();
                List<YellowGoodPolicy> yellowGoodPolicies = new List<YellowGoodPolicy>();
                List<BAndWPolicy> bAndWPolicies = new List<BAndWPolicy>();

                foreach (var policy in policyList)
                {


                    if (product.code == "A")
                    {
                        var vDetails = product.obj as VehicleDetails;
                        var vpolicy = new VehiclePolicy()
                        {
                            Id = Guid.NewGuid(),
                            PolicyId = policy.Id,
                            VehicleId = vDetails.Id
                        };
                        vehiclePolicies.Add(vpolicy);
                    }
                    else if (product.code == "B")
                    {
                        var vDetails = product.obj as VehicleDetails;
                        var vpolicy = new VehiclePolicy()
                        {
                            Id = Guid.NewGuid(),
                            PolicyId = policy.Id,
                            VehicleId = vDetails.Id
                        };
                        vehiclePolicies.Add(vpolicy);
                    }
                    else if (product.code == "E")
                    {
                        var bDetails = product.obj as BrownAndWhiteDetails;
                        var bPolicy = new BAndWPolicy()
                        {
                            Id = Guid.NewGuid(),
                            PolicyId = policy.Id,
                            BAndWId = bDetails.Id
                        };
                        bAndWPolicies.Add(bPolicy);
                    }
                    else if (product.code == "O")
                    {
                        var oDetails = product.obj as OtherItemDetails;
                        var oPolicy = new OtherItemPolicy()
                        {
                            Id = Guid.NewGuid(),
                            PolicyId = policy.Id,
                            OtherItemId = oDetails.Id
                        };
                        otherItemDetailsPolicies.Add(oPolicy);
                    }
                    else
                    {
                        var yDetails = product.obj as YellowGoodDetails;
                        var yPolicy = new YellowGoodPolicy()
                        {
                            Id = Guid.NewGuid(),
                            PolicyId = policy.Id,
                            YellowGoodId = yDetails.Id
                        };
                        yellowGoodPolicies.Add(yPolicy);
                    }
                }

                using (ITransaction transaction = session.BeginTransaction(IsolationLevel.ReadCommitted))
                {
                    if (customerInfo.IDTypeId == 0)
                    {
                        customerInfo.IDTypeId = 1;
                    }
                    if (customerInfo.Id == Guid.Empty)
                    {
                        session.Save(customerInfo, customerInfo.Id = Guid.NewGuid());
                    }
                    else
                    {
                        session.Update(customerInfo);
                    }

                    session.Save(policyBundle, policyBundle.Id);


                    foreach (var policy in policyList)
                    {
                        policy.CustomerId = customerInfo.Id;
                        policy.PolicyBundleId = policyBundle.Id;

                        if (policy.TPABranchId == Guid.Empty)
                        {
                            policy.TPABranchId = session.Query<TPABranch>().FirstOrDefault(a => a.IsHeadOffice).Id;
                        }
                        // add TPA ID
                        //var policyNum = GetNextPolicyNumber(policy.TPABranchId, policy.DealerId,
                        //    policy.ProductId,policy.CommodityTypeId, savePolicyRequest.policyDetails.policy.tpaBranchId, new CommonEntityManager().GetDealerCountryByDealerId(policy.DealerId));
                        //policy.PolicyNo = policyNum;

                        session.Evict(policy);
                        session.Save(policy, policy.Id);
                    }

                    if (product.code == "A")
                    {
                        VehicleDetails vd = product.obj as VehicleDetails;
                        if (vd.Id == Guid.Empty)
                        {
                            session.Save(vd, vd.Id = Guid.NewGuid());
                        }
                        else
                        {
                            session.Update(vd);
                        }

                        foreach (var vPolicy in vehiclePolicies)
                        {
                            vPolicy.VehicleId = vd.Id;
                            session.Evict(vPolicy);
                            session.Save(vPolicy, vPolicy.Id);
                        }
                    }
                    else if (product.code == "B")
                    {
                        VehicleDetails vd = product.obj as VehicleDetails;
                        if (vd.Id == Guid.Empty)
                        {
                            session.Save(vd, vd.Id = Guid.NewGuid());
                        }
                        else
                        {
                            session.Update(vd);
                        }

                        foreach (var vPolicy in vehiclePolicies)
                        {
                            vPolicy.VehicleId = vd.Id;
                            session.Evict(vPolicy);
                            session.Save(vPolicy, vPolicy.Id);
                        }
                    }
                    else if (product.code == "O")
                    {
                        //session.SaveOrUpdate(product.obj as OtherItemDetails);

                        OtherItemDetails od = product.obj as OtherItemDetails;
                        if (od.Id == Guid.Empty)
                        {
                            session.Save(od, od.Id = Guid.NewGuid());
                        }
                        else
                        {
                            session.Update(od);
                        }

                        foreach (var oPolicy in otherItemDetailsPolicies)
                        {
                            oPolicy.OtherItemId = od.Id;
                            session.Evict(oPolicy);
                            session.Save(oPolicy, oPolicy.Id);
                        }
                    }
                    else if (product.code == "E")
                    {
                        // session.SaveOrUpdate(product.obj as BrownAndWhiteDetails);
                        BrownAndWhiteDetails bd = product.obj as BrownAndWhiteDetails;
                        if (bd.Id == Guid.Empty)
                        {
                            session.Save(bd, bd.Id = Guid.NewGuid());
                        }
                        else
                        {
                            session.Update(bd);
                        }

                        foreach (var bPolicy in bAndWPolicies)
                        {
                            bPolicy.BAndWId = bd.Id;
                            session.Evict(bPolicy);
                            session.Save(bPolicy, bPolicy.Id);
                        }
                    }
                    else
                    {
                        // session.SaveOrUpdate(product.obj as YellowGoodDetails);
                        YellowGoodDetails yd = product.obj as YellowGoodDetails;
                        if (yd.Id == Guid.Empty)
                        {
                            session.Save(yd, yd.Id = Guid.NewGuid());
                        }
                        else
                        {
                            session.Update(yd);
                        }

                        foreach (var yPolicy in yellowGoodPolicies)
                        {
                            yPolicy.YellowGoodId = yd.Id;
                            session.Evict(yPolicy);
                            session.Save(yPolicy, yPolicy.Id);
                        }
                    }

                    foreach (Guid AttachmentId in savePolicyRequest.policyDetails.policyDocIds)
                    {
                        PolicyAttachment policyAttachment = new PolicyAttachment()
                        {
                            Id = Guid.NewGuid(),
                            PolicyBundleId = policyBundle.Id,
                            UserAttachmentId = AttachmentId
                        };
                        session.Save(policyAttachment, policyAttachment.Id);
                    }

                    transaction.Commit();
                }
                if (policyBundle.IsApproved)
                {

                    try
                    {
                        var policyBundleRequest = new PolicyBundleRequestDto()
                        {
                            IsApproved = policyBundle.IsApproved,
                            Id = policyBundle.Id,
                            ProductId = policyBundle.ProductId,
                            CustomerId = policyBundle.CustomerId
                        };
                        new ReportsForSend().SendPolicyStatementAndBooklate(policyBundleRequest, dbName);
                    }
                    catch (Exception ex)
                    {
                        logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ((ex.InnerException != null) ? ex.InnerException.ToString() : ""));
                    }

                }
                response = "success";
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ((ex.InnerException != null) ? ex.InnerException.ToString() : ""));
                response = "Error occured while saving policy.";
            }
            return response;
        }

        internal string UpdatePolicyV2(SavePolicyRequestDto SavePolicyRequest, string dbName)
        {
            string UniqueDbName = dbName;
            try
            {
                /* remove cache */
                ICache cache = CacheFactory.GetCache();
                cache.Remove(UniqueDbName + "_VehicleDetails");
                cache.Remove(UniqueDbName + "_BrownAndWhiteDetails");


                string validateComment = ValidateCurrencyPeriodOnPolicySave(SavePolicyRequest);
                if (validateComment != "ok")
                {
                    return validateComment;
                }

                #region "Customer"
                var customerEM = new CustomerEntityManager();
                CustomerRequestDto customer = DBDTOTransformer.Instance.PolicyCustomerToCustomerEntity(SavePolicyRequest.policyDetails.customer);
                bool customerStatus = false;
                if (IsGuid(SavePolicyRequest.policyDetails.customer.customerId.ToString()))
                {
                    //existing customer
                    customerStatus = customerEM.UpdateCustomerInPolicy(SavePolicyRequest.policyDetails.customer);
                    customerStatus = true;
                    if (customerStatus) {
                        /* remove cache */
                        cache.Remove(dbName + "_Customers");
                    }
                }
                else
                {
                    //new customer
                    customer.Id = Guid.NewGuid().ToString();
                    //SystemUserEntityManager sue = new SystemUserEntityManager().
                    customerStatus = customerEM.AddCustomer(customer);
                }
                #endregion "Customer"
                if (!customerStatus)
                {
                    return "Error occured while saving/updating customer";
                }
                #region "Product"
                var commodityCode = new CommodityEntityManager()
                    .GetCommodityById(SavePolicyRequest.policyDetails.product.commodityTypeId)
                    .CommodityCode;
                bool productStatus = false;
                Guid ItemId = Guid.Empty;
                if (IsGuid(SavePolicyRequest.policyDetails.product.id.ToString()))
                {
                    //existing product
                    if (commodityCode == "A")
                    {
                        var VehicleEM = new VehicleDetailsEntityManager();
                        VehicleDetailsRequestDto vehicleRequest = DBDTOTransformer.Instance.PolicyVehicleToVehicleEntity(SavePolicyRequest.policyDetails.product, SavePolicyRequest.policyDetails.policy);
                        productStatus = VehicleEM.UpdateVehicleDetails(vehicleRequest);

                        ItemId = vehicleRequest.Id;
                    }
                    else if (commodityCode == "B")
                    {
                        var VehicleEM = new VehicleDetailsEntityManager();
                        VehicleDetailsRequestDto vehicleRequest = DBDTOTransformer.Instance.PolicyVehicleToVehicleEntity(SavePolicyRequest.policyDetails.product, SavePolicyRequest.policyDetails.policy);
                        productStatus = VehicleEM.UpdateVehicleDetails(vehicleRequest);
                        ItemId = vehicleRequest.Id;
                    }
                    else if (commodityCode == "E")
                    {
                        var BandWEM = new BrownAndWhiteDetailsEntityManager();
                        BrownAndWhiteDetailsRequestDto BnWRequest = DBDTOTransformer.Instance.PolicyBnWToBnWEntity(SavePolicyRequest.policyDetails.product, SavePolicyRequest.policyDetails.policy);
                        productStatus = BandWEM.UpdateBrownAndWhiteDetails(BnWRequest);
                        ItemId = BnWRequest.Id;
                    }
                    else if (commodityCode == "Y")
                    {
                        var YellowGoodEM = new YellowGoodsEntityManager();
                        YellowGoodRequestDto YellowGoodRequest = DBDTOTransformer.Instance.PolicyYellowGoodToYellowGoodEntity(SavePolicyRequest.policyDetails.product, SavePolicyRequest.policyDetails.policy);
                        productStatus = YellowGoodEM.UpdateYellowGoodDetails(YellowGoodRequest);
                        ItemId = YellowGoodRequest.Id;
                    }
                    else if (commodityCode == "O")
                    {
                        var OtherItemEM = new OtherItemEntityManager();
                        OtherItemRequestDto OtherItemRequest = DBDTOTransformer.Instance.PolicyOtherItemToOtherItemEntity(SavePolicyRequest.policyDetails.product, SavePolicyRequest.policyDetails.policy);
                        productStatus = OtherItemEM.UpdateOtherItemDetails(OtherItemRequest);
                        ItemId = OtherItemRequest.Id;
                    }

                }
                else
                {
                    //new product
                    if (commodityCode == "A")
                    {
                        var VehicleEM = new VehicleDetailsEntityManager();
                        VehicleDetailsRequestDto vehicleRequest = DBDTOTransformer.Instance.PolicyVehicleToVehicleEntity(SavePolicyRequest.policyDetails.product, SavePolicyRequest.policyDetails.policy);
                        vehicleRequest.EntryDateTime = DateTime.Today.ToUniversalTime();
                        productStatus = VehicleEM.AddVehicleDetails(vehicleRequest);

                        ItemId = vehicleRequest.Id;
                    }
                    else if (commodityCode == "B")
                    {
                        var VehicleEM = new VehicleDetailsEntityManager();
                        VehicleDetailsRequestDto vehicleRequest = DBDTOTransformer.Instance.PolicyVehicleToVehicleEntity(SavePolicyRequest.policyDetails.product, SavePolicyRequest.policyDetails.policy);
                        vehicleRequest.EntryDateTime = DateTime.Today.ToUniversalTime();
                        productStatus = VehicleEM.AddVehicleDetails(vehicleRequest);

                        ItemId = vehicleRequest.Id;
                    }
                    else if (commodityCode == "E")
                    {
                        var BandWEM = new BrownAndWhiteDetailsEntityManager();
                        BrownAndWhiteDetailsRequestDto BnWRequest = DBDTOTransformer.Instance.PolicyBnWToBnWEntity(SavePolicyRequest.policyDetails.product, SavePolicyRequest.policyDetails.policy);
                        BnWRequest.EntryDateTime = DateTime.Today.ToUniversalTime();
                        productStatus = BandWEM.AddBrownAndWhiteDetails(BnWRequest);
                        ItemId = BnWRequest.Id;
                    }
                    else if (commodityCode == "Y")
                    {
                        var YellowGoodEM = new YellowGoodsEntityManager();
                        YellowGoodRequestDto YellowGoodRequest = DBDTOTransformer.Instance.PolicyYellowGoodToYellowGoodEntity(SavePolicyRequest.policyDetails.product, SavePolicyRequest.policyDetails.policy);
                        YellowGoodRequest.EntryDateTime = DateTime.Today.ToUniversalTime();
                        productStatus = YellowGoodEM.AddYellowGoodDetails(YellowGoodRequest);
                        ItemId = YellowGoodRequest.Id;
                    }
                    else if (commodityCode == "O")
                    {
                        var OtherItemEM = new OtherItemEntityManager();
                        OtherItemRequestDto OtherItemRequest = DBDTOTransformer.Instance.PolicyOtherItemToOtherItemEntity(SavePolicyRequest.policyDetails.product, SavePolicyRequest.policyDetails.policy);
                        OtherItemRequest.EntryDateTime = DateTime.Today.ToUniversalTime();
                        productStatus = OtherItemEM.AddOtherItemDetails(OtherItemRequest);
                        ItemId = OtherItemRequest.Id;
                    }
                }
                #endregion "Product"
                if (!productStatus)
                {
                    return "Error occured while saving/updating product";
                }
                #region "Policy Bundle"
                bool policyBundleStatus = false;
                Guid bundleId = SavePolicyRequest.policyDetails.policy.id;
                PolicyBundleRequestDto PolicyBundleRequest = DBDTOTransformer.Instance.PolicyDetailsToPolicyBundle(SavePolicyRequest.policyDetails.policy, SavePolicyRequest.policyDetails.payment, SavePolicyRequest.policyDetails.product, bundleId);
                PolicyBundleRequest.CustomerId = Guid.Parse(customer.Id);
                PolicyBundleRequest.ItemId = ItemId;
                PolicyBundleRequest.Id = bundleId;
                policyBundleStatus = UpdatePolicyBundle(PolicyBundleRequest);

                #endregion "Policy Bundle"
                if (!policyBundleStatus)
                {
                    return "Error occured while updating policy bundle";
                }

                #region "Policy"

                string policyNumber = string.Empty;
                Guid entryUser = Guid.Empty, approvedUser = Guid.Empty;

                bool policyStatus = false;
                List<PolicyRequestDto> PolicyRequestList = DBDTOTransformer.Instance.PolicyDetailsToPolicy(SavePolicyRequest.policyDetails.policy, SavePolicyRequest.policyDetails.payment, SavePolicyRequest.policyDetails.product);
                foreach (PolicyRequestDto policy in PolicyRequestList)
                {
                    if (commodityCode == "A")
                    {
                        policy.Type = "Vehicle";
                    }
                    else if (commodityCode == "B")
                    {
                        policy.Type = "Vehicle";
                    }
                    else if (commodityCode == "E")
                    {
                        policy.Type = "B&W";
                    }
                    else if (commodityCode == "Y")
                    {
                        policy.Type = "YellowGood";
                    }
                    else if (commodityCode == "O")
                    {
                        policy.Type = "Other";
                    }
                    policy.CustomerId = Guid.Parse(customer.Id);
                    policy.ItemId = ItemId;
                    policy.PolicyBundleId = PolicyBundleRequest.Id;
                    //if (policy.DealerPolicy)
                    //{
                    //    policy.TPABranchId = GetTpaBranchIdByDealerLocationId(policy.DealerLocationId);
                    //}

                    if (policy.DealerPolicy)
                    {
                        policy.TPABranchId = GetTpaBranchIdByDealerLocationId(policy.DealerLocationId);
                    }
                    else
                    {
                        if (!IsGuid(SavePolicyRequest.policyDetails.policy.tpaBranchId.ToString()))
                        {
                            policy.TPABranchId = GetTpaBranchIdByDealerLocationId(policy.DealerLocationId);
                        }
                        else
                        {
                            policy.TPABranchId = SavePolicyRequest.policyDetails.policy.tpaBranchId;
                        }
                    }

                    policyStatus = UpdatePolicy(policy);
                    policyNumber = policy.PolicyNo;
                    entryUser = policy.EntryUser;

                    if (!policyStatus)
                    {
                        return "Error occured while saving policy";
                    }
                }

                if (PolicyBundleRequest.IsApproved)
                {
                    try
                    {
                        new ReportsForSend().SendPolicyStatementAndBooklate(PolicyBundleRequest, dbName);
                    }
                    catch (Exception) { }
                }


                if (SavePolicyRequest.policyDetails.policy.dealerPolicy)
                {
                    //set notification
                    try
                    {
                        approvedUser = SavePolicyRequest.policyDetails.loggedInUserId;
                        CommonEntityManager commonEntityManager = new CommonEntityManager();
                        var notificationUser = new List<UserDetail>()
                        {
                            new UserDetail()
                            {
                                tpaId = TASTPAEntityManager.GetTpaIdByName(dbName),
                                userId = entryUser
                            }
                        };
                        var notificationDto = new PushNotificationsRequestDto()
                        {
                            generatedTime = DateTime.UtcNow,
                            message = "Policy Number(s) " + policyNumber + " got approved.",
                            link = "#",
                            messageFrom = commonEntityManager.GetUserNameById(approvedUser),
                            profilePic = commonEntityManager.GetProfilePictureByUserId(approvedUser),
                            userDetails = notificationUser
                        };
                        Task.Run(async () => await NotificationEntityManager.PushNotificationSender(notificationDto));

                    }
                    catch (Exception ex)
                    {
                        logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ((ex.InnerException != null) ? ex.InnerException.ToString() : ""));
                    }
                }
                #endregion "Policy"


                #region PolicyAttachment
                ISession session = EntitySessionManager.GetSession();
                try
                {
                    foreach (Guid AttachmentId in SavePolicyRequest.policyDetails.policyDocIds)
                    {
                        PolicyAttachment policyAttachment = new PolicyAttachment()
                        {
                            Id = Guid.NewGuid(),
                            PolicyBundleId = PolicyBundleRequest.Id,
                            UserAttachmentId = AttachmentId
                        };

                        using (ITransaction transaction = session.BeginTransaction())
                        {
                            session.Save(policyAttachment, policyAttachment.Id);
                            transaction.Commit();
                        }

                    }
                }
                catch (Exception)
                {

                    return "Policy attachment updating failed";
                }

                #endregion
                return "success";
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ((ex.InnerException != null) ? ex.InnerException.ToString() : ""));
                return "Error occured while saving policy";
            }

        }
        public object GetOtherTirePolicyById(Guid PolicyId)
        {
            //PolicyViewData Response = new PolicyViewData();
            try
            {
                ISession session = EntitySessionManager.GetSession();
                CommonEntityManager cem = new CommonEntityManager();

                PolicyBundle policyBundle = session.Query<PolicyBundle>().Where(a => a.Id == PolicyId).FirstOrDefault();
                Policy policy = session.Query<Policy>().Where(a => a.PolicyBundleId == policyBundle.Id).FirstOrDefault();

                if (policy == null)
                {
                    return String.Empty;
                }
                string PlateNumber = String.Empty;
                string cityForPlate = "N/A";
                Nullable<bool> allTyresAreSame = null;
                Currency Currency = session.Query<Currency>().Where(a => a.Id == policy.PremiumCurrencyTypeId).FirstOrDefault();
                CurrencyConversions CurrencyConversions = session.Query<CurrencyConversions>().Where(a => a.CurrencyId == Currency.Id).FirstOrDefault();
                CommodityType CommodityType = session.Query<CommodityType>().Where(a => a.CommodityTypeId == policy.CommodityTypeId).FirstOrDefault();
                CurrencyEntityManager Currencyem = new CurrencyEntityManager();

                List<Policy> Bundle = session.Query<Policy>().Where(a => a.PolicyBundleId == policyBundle.Id).ToList();
                List<PolicyContractTireProductResponseDto> retBundle = new List<PolicyContractTireProductResponseDto>();

                List<InvoiceCodeTireDetailsResponseDto> ListInvoiceCodeTireDetails = new List<InvoiceCodeTireDetailsResponseDto>();

                CustomerEnterdInvoiceDetails customerEnterdInvoiceDetails = null;

                foreach (var item in Bundle)
                {

                    InvoiceCodeDetails invoiceCodeDetails = session.Query<InvoiceCodeDetails>()
                       .Where(a => a.PolicyId == item.Id).FirstOrDefault();

                    InvoiceCode invoiceCode = session.Query<InvoiceCode>().Where(a => a.Id == invoiceCodeDetails.InvoiceCodeId).FirstOrDefault();
                    PlateNumber = invoiceCode.PlateNumber;
                    if (invoiceCode.PlateRelatedCityId != null && invoiceCode.PlateRelatedCityId.ToString().Length > 0 && Guid.Empty!= invoiceCode.PlateRelatedCityId) {
                        City city = session.Query<City>().Where(a => a.Id == invoiceCode.PlateRelatedCityId).FirstOrDefault();
                        cityForPlate = city.CityName;
                    }



                    allTyresAreSame = invoiceCode.AllTyresAreSame;
                     customerEnterdInvoiceDetails = session.Query<CustomerEnterdInvoiceDetails>().Where(a => a.InvoiceCodeId == invoiceCode.Id).FirstOrDefault();
                    InvoiceCodeTireDetails invoiceCodeTireDetails = session.Query<InvoiceCodeTireDetails>()
                        .Where(a => a.InvoiceCodeDetailId == invoiceCodeDetails.Id).FirstOrDefault();

                    List<InvoiceCodeTireDetails> ListInvoiceCodeTire = session.Query<InvoiceCodeTireDetails>()
                        .Where(a => a.InvoiceCodeDetailId == invoiceCodeDetails.Id).ToList();

                    foreach (var IVTD in ListInvoiceCodeTire)
                    {

                        AvailableTireSizesPattern availableTireSizesPattern = session.Query<AvailableTireSizesPattern>().Where(a => a.Id == IVTD.AvailableTireSizesPatternId).FirstOrDefault();
                        string pattern = String.Empty;
                        if (availableTireSizesPattern != null) {
                            pattern = availableTireSizesPattern.Pattern;
                        }
                        InvoiceCodeTireDetailsResponseDto ot = new InvoiceCodeTireDetailsResponseDto()
                        {
                            Id = IVTD.Id,
                            ArticleNumber = IVTD.ArticleNumber,
                            CrossSection = IVTD.CrossSection,
                            Diameter = IVTD.Diameter,
                            InvoiceCodeDetailId = IVTD.InvoiceCodeDetailId,
                            LoadSpeed = IVTD.LoadSpeed,
                            Position = IVTD.Position,
                            SerialNumber = IVTD.SerialNumber,
                            Width = IVTD.Width,
                            DotNumber = IVTD.DotNumber,
                            Pattern = pattern,
                            Price = IVTD.Price
                        };

                        ListInvoiceCodeTireDetails.Add(ot);
                    };

                    PolicyContractTireProductResponseDto p = new PolicyContractTireProductResponseDto()
                    {
                        ContractId = item.ContractId,
                        CoverTypeId = item.ContractExtensionPremiumId,
                        ExtensionTypeId = item.ExtensionTypeId,
                        AttributeSpecificationId = item.ContractInsuaranceLimitationId,
                        ContractExtensionsId = item.ContractExtensionsId,
                        Id = item.Id,
                        Premium = Currencyem.ConvertFromBaseCurrency(item.Premium, item.PremiumCurrencyTypeId, item.CurrencyPeriodId),// item.Premium * CurrencyConversions.Rate,
                        PremiumCurrencyTypeId = item.PremiumCurrencyTypeId,
                        ProductId = item.ProductId,
                        PolicyNo = item.PolicyNo,
                        PolicyEndDate = item.PolicyEndDate,
                        PolicyStartDate = item.PolicyStartDate,
                        BookletNumber = item.BookletNumber,
                        ArticleNumber = invoiceCodeTireDetails.ArticleNumber,
                        SerialNumber = invoiceCodeTireDetails.SerialNumber,
                        Position = invoiceCodeTireDetails.Position,
                        InvoiceCodeId = invoiceCode.Id,
                        NoOfDate = (item.PolicySoldDate - invoiceCode.GeneratedDate).TotalDays,
                        VariantId = invoiceCodeDetails.VariantId
                    };
                    retBundle.Add(p);
                }

                // customerEnterdInvoiceDetails.AdditionalDetailsModelId

                BrownAndWhiteDetailsResponseDto retBrownAndWhiteDetails = new BrownAndWhiteDetailsResponseDto();
                Make Make = new Make();
                Model Model = new Model();
                CommodityCategory CommodityCategory = new CommodityCategory();

                if (CommodityType.CommodityCode == "O")
                {
                    OtherItemPolicy OtherItemPolicy = session.Query<OtherItemPolicy>().Where(a => a.PolicyId == policy.Id).FirstOrDefault();
                    OtherItemDetails OtherItemDetails = session.Query<OtherItemDetails>().Where(a => a.Id == OtherItemPolicy.OtherItemId).FirstOrDefault();
                    Make = session.Query<Make>().Where(a => a.Id == OtherItemDetails.MakeId).FirstOrDefault();
                    Model = session.Query<Model>().Where(a => a.Id == OtherItemDetails.ModelId).FirstOrDefault();
                    AdditionalPolicyMakeData additionalPolicyMakeData = session.Query<AdditionalPolicyMakeData>().Where(a => a.Id == customerEnterdInvoiceDetails.AdditionalDetailsMakeId).FirstOrDefault();
                    AdditionalPolicyModelData additionalPolicyModelData = session.Query<AdditionalPolicyModelData>().Where(a => a.Id == customerEnterdInvoiceDetails.AdditionalDetailsModelId).FirstOrDefault();

                    UserAttachment userAttachment = session.Query<UserAttachment>().Where(ua => ua.Id == customerEnterdInvoiceDetails.InvoiceAttachmentId).FirstOrDefault();
                    string uploadedInvoiceFileName = String.Empty;
                    string uploadedInvoiceFileRef = String.Empty;
                    if (userAttachment!=null) {
                        uploadedInvoiceFileName = userAttachment.AttachmentFileName;
                        uploadedInvoiceFileRef = userAttachment.FileServerReference;
                    }

                    retBrownAndWhiteDetails = new BrownAndWhiteDetailsResponseDto()
                    {
                        AddnSerialNo = OtherItemDetails.AddnSerialNo,
                        CategoryId = OtherItemDetails.CategoryId,
                        CommodityUsageTypeId = OtherItemDetails.CommodityUsageTypeId,
                        ConversionRate = OtherItemDetails.ConversionRate,
                        //CountryId = OtherItemDetails.CountryId,
                        currencyPeriodId = OtherItemDetails.currencyPeriodId,
                        DealerCurrencyId = OtherItemDetails.DealerCurrencyId,
                        //DealerId = OtherItemDetails.DealerId,
                        DealerPrice = OtherItemDetails.DealerPrice * policy.LocalCurrencyConversionRate,
                        EntryDateTime = OtherItemDetails.EntryDateTime,
                        EntryUser = OtherItemDetails.EntryUser,
                        Id = OtherItemDetails.Id,
                        InvoiceNo = OtherItemDetails.InvoiceNo,
                        ItemPrice = OtherItemDetails.ItemPrice * policy.LocalCurrencyConversionRate,
                        ItemPurchasedDate = OtherItemDetails.ItemPurchasedDate,
                        ItemStatusId = OtherItemDetails.ItemStatusId,
                        MakeId = OtherItemDetails.MakeId,
                        ModelCode = OtherItemDetails.ModelCode,
                        ModelId = OtherItemDetails.ModelId,
                        ModelYear = OtherItemDetails.ModelYear,
                        SerialNo = OtherItemDetails.SerialNo,
                        Variant = OtherItemDetails.VariantId,
                        MakeCodeV = additionalPolicyMakeData.MakeName,
                        ModelCodeV = additionalPolicyModelData.ModelName,
                        AdditionalModalYear = customerEnterdInvoiceDetails.AdditionalDetailsModelYear,
                        Milage = customerEnterdInvoiceDetails.AdditionalDetailsMileage,
                        UploadedInvoiceFileName = uploadedInvoiceFileName,
                        UploadedInvoiceFileRef = uploadedInvoiceFileRef
                        //Variant = OtherItemDetails.Variant,
                    };

                }

                decimal PremiumData;

                if (policy.DealerPayment == 0 && policy.CustomerPayment == 0)
                {
                    PremiumData = policy.Premium;
                }
                else
                {
                    PremiumData = policy.DealerPayment + policy.CustomerPayment;
                }

                #region Customer
                Customer customer = session.Query<Customer>().Where(a => a.Id == policy.CustomerId).FirstOrDefault();
                //CustomersResponseDto customerData = new CustomersResponseDto();

                CustomerResponseDto customerD = new CustomerResponseDto()
                {
                    Address1 = customer.Address1,
                    Address2 = customer.Address2,
                    Address3 = customer.Address3,
                    Address4 = customer.Address4,
                    BusinessAddress1 = customer.BusinessAddress1,
                    BusinessAddress2 = customer.BusinessAddress2,
                    BusinessAddress3 = customer.BusinessAddress3,
                    BusinessAddress4 = customer.BusinessAddress4,
                    BusinessName = customer.BusinessName,
                    BusinessTelNo = customer.BusinessTelNo,
                    CityId = customer.CityId,
                    CountryId = customer.CountryId,
                    CustomerTypeId = customer.CustomerTypeId,
                    DateOfBirth = customer.DateOfBirth,
                    DLIssueDate = customer.DLIssueDate,
                    EntryDateTime = customer.EntryDateTime,
                    Email = customer.Email,
                    EntryUserId = customer.EntryUserId,
                    FirstName = customer.FirstName,
                    Gender = customer.Gender,
                    LastName = customer.LastName,
                    IDNo = customer.IDNo,
                    Id = customer.Id.ToString(),
                    IDTypeId = customer.IDTypeId,
                    IsActive = customer.IsActive,
                    MaritalStatusId = customer.MaritalStatusId,
                    MobileNo = customer.MobileNo,
                    NationalityId = customer.NationalityId,
                    OccupationId = customer.OccupationId,
                    OtherTelNo = customer.OtherTelNo,
                    TitleId = customer.TitleId,
                    UsageTypeId = customer.UsageTypeId,
                    UserName = customer.UserName,
                    Password = customer.Password,
                };
                VehicleDetailsResponseDto VehicleD = new VehicleDetailsResponseDto()
                {
                    PlateNo = PlateNumber,
                    AllTyresAreSame = allTyresAreSame,
                    City = cityForPlate
                };

                #endregion

                Contract Contract = session.Query<Contract>().Where(a => a.Id == policy.ContractId).FirstOrDefault();
                ReinsurerContract ReinsurerContract = session.Query<ReinsurerContract>().Where(a => a.Id == Contract.ReinsurerContractId).FirstOrDefault();

                ManufacturerWarrantyDetails mwd = session.Query<ManufacturerWarrantyDetails>().Where(a => a.ModelId == Model.Id &&
                                                    a.CountryId == ReinsurerContract.CountryId).FirstOrDefault();

                ManufacturerWarranty ManufacturerWarranty = null;
                ManufacturerWarrantyResponseDto ManufacturerWarrantyResponseDto = new ManufacturerWarrantyResponseDto();

                //if (CommodityType.CommodityCode == "O")
                //{
                ContractInsuaranceLimitation CIL = session.Query<ContractInsuaranceLimitation>().Where(a => a.ContractId == Contract.Id).FirstOrDefault();
                InsuaranceLimitation InsuaranceLimitation = session.Query<InsuaranceLimitation>().Where(a => a.CommodityTypeId == policy.CommodityTypeId && a.Id == CIL.InsuaranceLimitationId).FirstOrDefault();
                ContractExtensions ContractExtensions = session.Query<ContractExtensions>().Where(a => a.ContractInsuanceLimitationId == CIL.Id).FirstOrDefault();

                InsuaranceLimitationResponseDto InsuaranceLimitationResponseDto = new InsuaranceLimitationResponseDto()
                {

                    CommodityTypeCode = InsuaranceLimitation.CommodityTypeCode,
                    CommodityTypeId = InsuaranceLimitation.CommodityTypeId,
                    Hrs = InsuaranceLimitation.Hrs,
                    Id = InsuaranceLimitation.Id,
                    InsuaranceLimitationName = InsuaranceLimitation.InsuaranceLimitationName,
                    Km = InsuaranceLimitation.Km,
                    Months = InsuaranceLimitation.Months,
                    TopOfMW = InsuaranceLimitation.TopOfMW,
                    IsRsa = InsuaranceLimitation.IsRsa
                };

                PolicyViewData result = new PolicyViewData()
                {
                    Id = policyBundle.Id,
                    CommodityTypeId = policy.CommodityTypeId,
                    tpaBranchId = policy.TPABranchId,

                    ProductId = policy.ProductId,
                    DealerId = policy.DealerId,
                    DealerLocationId = policy.DealerLocationId,
                    ContractId = policy.ContractId,
                    ExtensionTypeId = policy.ExtensionTypeId,
                    //Premium = Bundle[0].DealerPayment + Bundle[0].CustomerPayment,
                    Premium = PremiumData,
                    PremiumCurrencyTypeId = policy.PremiumCurrencyTypeId,
                    DealerPaymentCurrencyTypeId = policy.DealerPaymentCurrencyTypeId,
                    CustomerPaymentCurrencyTypeId = policy.CustomerPaymentCurrencyTypeId,
                    CoverTypeId = policy.CoverTypeId,
                    HrsUsedAtPolicySale = policy.HrsUsedAtPolicySale,
                    IsPreWarrantyCheck = policy.IsPreWarrantyCheck,
                    PolicySoldDate = policy.PolicySoldDate,
                    SalesPersonId = policy.SalesPersonId,
                    PolicyNo = policy.PolicyNo,
                    IsSpecialDeal = policy.IsSpecialDeal,
                    IsPartialPayment = policy.IsPartialPayment,
                    DealerPayment = policy.DealerPayment,
                    CustomerPayment = policy.CustomerPayment,
                    PaymentModeId = policy.PaymentModeId,
                    PaymentTypeId = policy.PaymentTypeId,
                    RefNo = policy.RefNo,
                    Comment = policy.Comment,
                    ItemId = retBrownAndWhiteDetails.Id,
                    IsApproved = policy.IsApproved,
                    Type = "",
                    CustomerId = Bundle[0].CustomerId,
                    // Vehicle = (retVehicleDetails == null || retVehicleDetails.Id == null) ? null : retVehicleDetails,
                    Customer = customerD,
                    Vehicle = VehicleD,
                    // Customer = customerData.Customers.Find(c => c.Id == Bundle[0].CustomerId.ToString()),
                    BAndW = (retBrownAndWhiteDetails == null || retBrownAndWhiteDetails.Id == null) ? null : retBrownAndWhiteDetails,

                    EntryDateTime = Bundle[0].EntryDateTime,
                    EntryUser = Bundle[0].EntryUser,
                    PolicyStartDate = ManufacturerWarranty == null || !Bundle[0].MWIsAvailable ? Bundle[0].PolicySoldDate : Bundle[0].PolicyStartDate.AddDays(+1),
                    PolicyEndDate = Bundle[0].PolicyEndDate,
                    ContractTireProducts = retBundle,
                    Discount = Bundle[0].DiscountPercentage,
                    DiscountPercentage = Bundle[0].DiscountPercentage,
                    DealerPolicy = Bundle[0].DealerPolicy
                };

                foreach (var item in result.ContractTireProducts)
                {
                    Product Product = session.Query<Product>().Where(a => a.Id == policy.ProductId).FirstOrDefault();
                    item.Name = Product.Productcode + " - " + Product.Productname;

                    item.OtherTireDetails = ListInvoiceCodeTireDetails;

                    if (item.PremiumCurrencyTypeId.ToString() != "00000000-0000-0000-0000-000000000000")
                    {

                        item.PremiumCurrencyName = Currency.Code;
                    }
                    List<Guid> policyContractIdLists = new List<Guid>();
                    policyContractIdLists = GetContractIdListForReleventTyre(item.Id,item.VariantId);
                    List<Contract> Contracts = session.Query<Contract>().Where(c => policyContractIdLists.Contains(c.Id)).ToList();
                    List<ContractResponseDto> ContractResponseDto = new List<ContractResponseDto>();

                    foreach (var ConRes in Contracts)
                    {
                        ContractResponseDto c = new ContractResponseDto
                        {
                            ClaimLimitation = ConRes.ClaimLimitation,
                            CommodityCategoryId = ConRes.CommodityCategoryId,
                            CommodityTypeId = ConRes.CommodityTypeId,
                            CommodityUsageTypeId = ConRes.CommodityUsageTypeId,
                            CountryId = ConRes.CountryId,
                            DealerId = ConRes.DealerId,
                            DealName = ConRes.DealName,
                            DealType = ConRes.DealType,
                            DiscountAvailable = ConRes.DiscountAvailable,
                            EndDate = ConRes.EndDate,
                            StartDate = ConRes.StartDate,
                            Id = ConRes.Id,
                            InsurerId = ConRes.InsurerId,
                            IsActive = ConRes.IsActive,
                            IsAutoRenewal = ConRes.IsAutoRenewal,
                            LiabilityLimitation = ConRes.LiabilityLimitation,
                            Remark = ConRes.Remark,
                            ProductId = ConRes.ProductId,
                            EntryUser = ConRes.EntryUser
                        };
                        ContractResponseDto.Add(c);
                    }

                    item.Contracts = ContractResponseDto;
                    if (CommodityType.CommodityCode == "O")
                    {
                        item.ExtensionTypes = ContractEntityManager.GetAllExtensionTypeByContractId(Bundle[0].ContractId,
                                Bundle[0].ProductId, Bundle[0].DealerId, Bundle[0].PolicySoldDate,
                                Guid.Empty, Guid.Empty, retBrownAndWhiteDetails.MakeId, retBrownAndWhiteDetails.ModelId,
                                retBrownAndWhiteDetails.Variant, 0);
                    }


                    List<ContractExtensions> ContractExtension = session.Query<ContractExtensions>().Where(a => a.ContractInsuanceLimitationId == CIL.Id).ToList();
                    List<ContractExtensionResponseDto> ContractExtensionResponseDto = new List<ContractExtensionResponseDto>();
                    foreach (var ContractExtRes in ContractExtension)
                    {
                        ContractExtensionResponseDto ContractExtensionResponse = new ContractExtensionResponseDto()
                        {
                            AttributeSpecification = ContractExtRes.AttributeSpecification,
                            ContractInsuanceLimitationId = ContractExtRes.ContractInsuanceLimitationId,
                            Id = ContractExtRes.Id,
                            RSAProviderId = ContractExtRes.RSAProviderId,
                            EntryDateTime = ContractExtRes.EntryDateTime,
                            EntryUser = ContractExtRes.EntryUser,
                            Rate = ContractExtRes.Rate,
                            RegionId = ContractExtRes.RegionId,
                        };
                        ContractExtensionResponseDto.Add(ContractExtensionResponse);
                    }

                    item.AttributeSpecifications = ContractExtensionResponseDto;

                    WarrantyType Covertypes = session.Query<WarrantyType>().FirstOrDefault();


                    List<Guid> ContractIds = new List<Guid>();
                    foreach (var c in Contracts)
                    {
                        ContractIds.Add(c.Id);
                    }

                    if (Product.Productcode == "RSA")
                    {
                        List<Guid> RSAIds = new List<Guid>();
                        foreach (var r in ContractExtensionResponseDto)
                        {
                            RSAIds.Add(r.RSAProviderId);
                        }
                    }
                    else
                    {
                        List<Guid> Warranty = new List<Guid>();
                        foreach (var r in ContractExtensionResponseDto)
                        {
                            Warranty.Add(r.WarrantyTypeId);
                        }

                        if (CommodityType.CommodityCode == "O")
                        {
                            item.CoverTypess = ContractEntityManager.GetGetCoverTypesByExtensionIdUnitOfWork2(Bundle[0].ContractInsuaranceLimitationId,
                                         Bundle[0].ContractExtensionsId, Bundle[0].ContractId,
                                         policy.ProductId, policy.DealerId, Bundle[0].PolicySoldDate, Guid.Empty,
                                         Guid.Empty,
                                         retBrownAndWhiteDetails.MakeId, retBrownAndWhiteDetails.ModelId, Guid.Empty,
                                         0, retBrownAndWhiteDetails.ItemStatusId);
                        }

                    }
                }
                return result;
                //}
                //return "";
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ((ex.InnerException != null) ? ex.InnerException.ToString() : ""));
                return "Error occured";
            }

            //throw new NotImplementedException();
        }



        public List<Guid> GetContractIdListForReleventTyre(Guid policyId , Guid VariantId) {
            ISession session = EntitySessionManager.GetSession();
            Policy policy = session.Query<Policy>().Where(a => a.Id == policyId).FirstOrDefault();
            Contract contract = session.Query<Contract>().Where(a => a.Id == policy.ContractId).FirstOrDefault();

            List<Guid> response = (from c in session.Query<Contract>()
                                   join cil in session.Query<ContractInsuaranceLimitation>() on c.Id equals cil.ContractId
                                   join ce in session.Query<ContractExtensions>() on cil.Id equals ce.ContractInsuanceLimitationId
                                   join cev in session.Query<ContractExtensionVariant>() on ce.Id equals cev.ContractExtensionId
                                   join cep in session.Query<ContractExtensionPremium>() on ce.Id equals cep.ContractExtensionId
                                   join il in session.Query<InsuaranceLimitation>() on cil.InsuaranceLimitationId equals il.Id
                                   where c.CommodityTypeId == policy.CommodityTypeId &&
                                   c.IsActive == true &&
                                   c.CountryId == contract.CountryId &&
                                   c.ProductId == policy.ProductId &&
                                   c.DealerId == policy.DealerId &&
                                   c.CommodityUsageTypeId == contract.CommodityUsageTypeId &&
                                   c.StartDate <= policy.PolicySoldDate && c.EndDate >= policy.PolicySoldDate &&
                                   cev.VariantId== VariantId
                                   select c.Id).ToList<Guid>();
            return response;
        }

        public object GetPolicyById2(Guid PolicyId)
        {
            PolicyViewData Response = new PolicyViewData();
            try
            {
                ISession session = EntitySessionManager.GetSession();
                CommonEntityManager cem = new CommonEntityManager();
                //SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());


                //Policy policy = session.Query<Policy>().Where(a => a.PolicyBundleId == PolicyId).FirstOrDefault();
                PolicyBundle policyBundle = session.Query<PolicyBundle>().Where(a => a.Id == PolicyId).FirstOrDefault();
                Policy policy = session.Query<Policy>().Where(a => a.PolicyBundleId == policyBundle.Id).FirstOrDefault();
                PolicyAdditionalDetails policyAdditionalDetails =
                    session.Query<PolicyAdditionalDetails>().Where(b => b.PolicyId == policy.Id).FirstOrDefault();

                if (policy == null)
                {
                    return String.Empty;
                }

                Currency Currency = session.Query<Currency>().Where(a => a.Id == policy.PremiumCurrencyTypeId).FirstOrDefault();
                CurrencyConversions CurrencyConversions = session.Query<CurrencyConversions>().Where(a => a.CurrencyId == Currency.Id).FirstOrDefault();
                CommodityType CommodityType = session.Query<CommodityType>().Where(a => a.CommodityTypeId == policy.CommodityTypeId).FirstOrDefault();
                CurrencyEntityManager Currencyem = new CurrencyEntityManager();

                List<Policy> Bundle = session.Query<Policy>().Where(a => a.PolicyBundleId == policyBundle.Id).ToList();
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
                        Premium = Currencyem.ConvertFromBaseCurrency(item.Premium, item.PremiumCurrencyTypeId, item.CurrencyPeriodId),// item.Premium * CurrencyConversions.Rate,
                        PremiumCurrencyTypeId = item.PremiumCurrencyTypeId,
                        ProductId = item.ProductId,
                        PolicyNo = item.PolicyNo,
                        PolicyEndDate = item.PolicyEndDate,
                        PolicyStartDate = item.PolicyStartDate,
                        BookletNumber = item.BookletNumber,
                        FinanceAmount = Currencyem.ConvertFromBaseCurrency(item.FinanceAmount, item.PremiumCurrencyTypeId, item.CurrencyPeriodId)
                    };
                    retBundle.Add(p);
                }

                #region ItemDetails

                VehicleDetailsResponseDto retVehicleDetails = new VehicleDetailsResponseDto();
                BrownAndWhiteDetailsResponseDto retBrownAndWhiteDetails = new BrownAndWhiteDetailsResponseDto();
                Make Make = new Make();
                Model Model = new Model();
                CommodityCategory CommodityCategory = new CommodityCategory();

                if (CommodityType.CommodityCode == "A")
                {
                    VehiclePolicy VehiclePolicy = session.Query<VehiclePolicy>().Where(a => a.PolicyId == policy.Id).FirstOrDefault();
                    VehicleDetails VehicleDetail = session.Query<VehicleDetails>().Where(a => a.Id == VehiclePolicy.VehicleId).FirstOrDefault();

                    retVehicleDetails = new VehicleDetailsResponseDto()
                    {
                        AspirationId = VehicleDetail.AspirationId,
                        BodyTypeId = VehicleDetail.BodyTypeId,
                        CategoryId = VehicleDetail.CategoryId,
                        currencyPeriodId = VehicleDetail.currencyPeriodId,
                        CommodityUsageTypeId = VehicleDetail.CommodityUsageTypeId,
                        ConversionRate = VehicleDetail.ConversionRate,
                        CountryId = VehicleDetail.CountryId,
                        CylinderCountId = VehicleDetail.CylinderCountId,
                        DealerCurrencyId = VehicleDetail.DealerCurrencyId,
                        DealerId = VehicleDetail.DealerId,
                        DealerPrice = Currencyem.ConvertFromBaseCurrency(VehicleDetail.DealerPrice, VehicleDetail.DealerCurrencyId, VehicleDetail.currencyPeriodId), //VehicleDetail.DealerPrice * VehicleDetail.ConversionRate,
                        DriveTypeId = VehicleDetail.DriveTypeId,
                        EngineCapacityId = VehicleDetail.EngineCapacityId,
                        EntryDateTime = VehicleDetail.EntryDateTime,
                        EntryUser = VehicleDetail.EntryUser,
                        FuelTypeId = VehicleDetail.FuelTypeId,
                        GrossWeight = VehicleDetail.GrossWeight,
                        Id = VehicleDetail.Id,
                        ItemPurchasedDate = VehicleDetail.ItemPurchasedDate,
                        ItemStatusId = VehicleDetail.ItemStatusId,
                        MakeId = VehicleDetail.MakeId,
                        ModelId = VehicleDetail.ModelId,
                        ModelYear = VehicleDetail.ModelYear,
                        PlateNo = VehicleDetail.PlateNo,
                        RegistrationDate = VehicleDetail.RegistrationDate,
                        TransmissionId = VehicleDetail.TransmissionId,
                        Variant = VehicleDetail.Variant,
                        VehiclePrice = Currencyem.ConvertFromBaseCurrency(VehicleDetail.VehiclePrice, VehicleDetail.DealerCurrencyId, VehicleDetail.currencyPeriodId),// VehicleDetail.VehiclePrice * VehicleDetail.ConversionRate,
                        VINNo = VehicleDetail.VINNo,
                        EngineNumber = VehicleDetail.EngineNumber

                    };

                    Make = session.Query<Make>().Where(a => a.Id == VehicleDetail.MakeId).FirstOrDefault();
                    Model = session.Query<Model>().Where(a => a.Id == VehicleDetail.ModelId).FirstOrDefault();
                    CommodityCategory = session.Query<CommodityCategory>().Where(a => a.CommodityCategoryId == VehicleDetail.CategoryId).FirstOrDefault();

                }
                else if (CommodityType.CommodityCode == "B")
                {
                    VehiclePolicy VehiclePolicy = session.Query<VehiclePolicy>().Where(a => a.PolicyId == policy.Id).FirstOrDefault();
                    VehicleDetails VehicleDetail = session.Query<VehicleDetails>().Where(a => a.Id == VehiclePolicy.VehicleId).FirstOrDefault();

                    retVehicleDetails = new VehicleDetailsResponseDto()
                    {
                        AspirationId = VehicleDetail.AspirationId,
                        BodyTypeId = VehicleDetail.BodyTypeId,
                        CategoryId = VehicleDetail.CategoryId,
                        currencyPeriodId = VehicleDetail.currencyPeriodId,
                        CommodityUsageTypeId = VehicleDetail.CommodityUsageTypeId,
                        ConversionRate = VehicleDetail.ConversionRate,
                        CountryId = VehicleDetail.CountryId,
                        CylinderCountId = VehicleDetail.CylinderCountId,
                        DealerCurrencyId = VehicleDetail.DealerCurrencyId,
                        DealerId = VehicleDetail.DealerId,
                        DealerPrice = Currencyem.ConvertFromBaseCurrency(VehicleDetail.DealerPrice, VehicleDetail.DealerCurrencyId, VehicleDetail.currencyPeriodId), //VehicleDetail.DealerPrice * VehicleDetail.ConversionRate,
                        DriveTypeId = VehicleDetail.DriveTypeId,
                        EngineCapacityId = VehicleDetail.EngineCapacityId,
                        EntryDateTime = VehicleDetail.EntryDateTime,
                        EntryUser = VehicleDetail.EntryUser,
                        FuelTypeId = VehicleDetail.FuelTypeId,
                        GrossWeight = VehicleDetail.GrossWeight,
                        Id = VehicleDetail.Id,
                        ItemPurchasedDate = VehicleDetail.ItemPurchasedDate,
                        ItemStatusId = VehicleDetail.ItemStatusId,
                        MakeId = VehicleDetail.MakeId,
                        ModelId = VehicleDetail.ModelId,
                        ModelYear = VehicleDetail.ModelYear,
                        PlateNo = VehicleDetail.PlateNo,
                        RegistrationDate = VehicleDetail.RegistrationDate,
                        TransmissionId = VehicleDetail.TransmissionId,
                        Variant = VehicleDetail.Variant,
                        VehiclePrice = Currencyem.ConvertFromBaseCurrency(VehicleDetail.VehiclePrice, VehicleDetail.DealerCurrencyId, VehicleDetail.currencyPeriodId),// VehicleDetail.VehiclePrice * VehicleDetail.ConversionRate,
                        VINNo = VehicleDetail.VINNo,
                        EngineNumber = VehicleDetail.EngineNumber

                    };

                    Make = session.Query<Make>().Where(a => a.Id == VehicleDetail.MakeId).FirstOrDefault();
                    Model = session.Query<Model>().Where(a => a.Id == VehicleDetail.ModelId).FirstOrDefault();
                    CommodityCategory = session.Query<CommodityCategory>().Where(a => a.CommodityCategoryId == VehicleDetail.CategoryId).FirstOrDefault();

                }
                else if (CommodityType.CommodityCode == "E")
                {
                    BAndWPolicy BAndWPolicy = session.Query<BAndWPolicy>().Where(a => a.PolicyId == policy.Id).FirstOrDefault();
                    BrownAndWhiteDetails BrownAndWhiteDetails = session.Query<BrownAndWhiteDetails>().Where(a => a.Id == BAndWPolicy.BAndWId).FirstOrDefault();

                    retBrownAndWhiteDetails = new BrownAndWhiteDetailsResponseDto()
                    {
                        AddnSerialNo = BrownAndWhiteDetails.AddnSerialNo,
                        CategoryId = BrownAndWhiteDetails.CategoryId,
                        CommodityUsageTypeId = BrownAndWhiteDetails.CommodityUsageTypeId,
                        ConversionRate = BrownAndWhiteDetails.ConversionRate,
                        CountryId = BrownAndWhiteDetails.CountryId,
                        currencyPeriodId = BrownAndWhiteDetails.currencyPeriodId,
                        DealerCurrencyId = BrownAndWhiteDetails.DealerCurrencyId,
                        DealerId = BrownAndWhiteDetails.DealerId,
                        DealerPrice = BrownAndWhiteDetails.DealerPrice * BrownAndWhiteDetails.ConversionRate,
                        EntryDateTime = BrownAndWhiteDetails.EntryDateTime,
                        EntryUser = BrownAndWhiteDetails.EntryUser,
                        Id = BrownAndWhiteDetails.Id,
                        InvoiceNo = BrownAndWhiteDetails.InvoiceNo,
                        ItemPrice = BrownAndWhiteDetails.ItemPrice * BrownAndWhiteDetails.ConversionRate,
                        ItemPurchasedDate = BrownAndWhiteDetails.ItemPurchasedDate,
                        ItemStatusId = BrownAndWhiteDetails.ItemStatusId,
                        MakeId = BrownAndWhiteDetails.MakeId,
                        ModelCode = BrownAndWhiteDetails.ModelCode,
                        ModelId = BrownAndWhiteDetails.ModelId,
                        ModelYear = BrownAndWhiteDetails.ModelYear,
                        SerialNo = BrownAndWhiteDetails.SerialNo,
                        Variant = BrownAndWhiteDetails.Variant,
                    };

                    Make = session.Query<Make>().Where(a => a.Id == BrownAndWhiteDetails.MakeId).FirstOrDefault();
                    Model = session.Query<Model>().Where(a => a.Id == BrownAndWhiteDetails.ModelId).FirstOrDefault();
                    CommodityCategory = session.Query<CommodityCategory>().Where(a => a.CommodityCategoryId == BrownAndWhiteDetails.CategoryId).FirstOrDefault();

                }
                else if (CommodityType.CommodityCode == "Y")
                {
                    YellowGoodPolicy YellowGoodPolicy = session.Query<YellowGoodPolicy>().Where(a => a.PolicyId == policy.Id).FirstOrDefault();
                    YellowGoodDetails YellowGoodDetails = session.Query<YellowGoodDetails>().Where(a => a.Id == YellowGoodPolicy.YellowGoodId).FirstOrDefault();

                    retBrownAndWhiteDetails = new BrownAndWhiteDetailsResponseDto()
                    {
                        AddnSerialNo = YellowGoodDetails.AddnSerialNo,
                        CategoryId = YellowGoodDetails.CategoryId,
                        CommodityUsageTypeId = YellowGoodDetails.CommodityUsageTypeId,
                        ConversionRate = YellowGoodDetails.ConversionRate,
                        //CountryId = YellowGoodDetails.CountryId,
                        currencyPeriodId = YellowGoodDetails.currencyPeriodId,
                        DealerCurrencyId = YellowGoodDetails.DealerCurrencyId,
                        // DealerId = YellowGoodDetails.DealerId,
                        DealerPrice = YellowGoodDetails.DealerPrice * YellowGoodDetails.ConversionRate,
                        EntryDateTime = YellowGoodDetails.EntryDateTime,
                        EntryUser = YellowGoodDetails.EntryUser,
                        Id = YellowGoodDetails.Id,
                        InvoiceNo = YellowGoodDetails.InvoiceNo,
                        ItemPrice = YellowGoodDetails.ItemPrice * YellowGoodDetails.ConversionRate,
                        ItemPurchasedDate = YellowGoodDetails.ItemPurchasedDate,
                        ItemStatusId = YellowGoodDetails.ItemStatusId,
                        MakeId = YellowGoodDetails.MakeId,
                        ModelCode = YellowGoodDetails.ModelCode,
                        ModelId = YellowGoodDetails.ModelId,
                        ModelYear = YellowGoodDetails.ModelYear,
                        SerialNo = YellowGoodDetails.SerialNo,
                        // Variant = YellowGoodDetails.Variant,
                    };

                    Make = session.Query<Make>().Where(a => a.Id == YellowGoodDetails.MakeId).FirstOrDefault();
                    Model = session.Query<Model>().Where(a => a.Id == YellowGoodDetails.ModelId).FirstOrDefault();
                    CommodityCategory = session.Query<CommodityCategory>().Where(a => a.CommodityCategoryId == YellowGoodDetails.CategoryId).FirstOrDefault();

                }
                else if (CommodityType.CommodityCode == "O")
                {
                    OtherItemPolicy OtherItemPolicy = session.Query<OtherItemPolicy>().Where(a => a.PolicyId == policy.Id).FirstOrDefault();
                    OtherItemDetails OtherItemDetails = session.Query<OtherItemDetails>().Where(a => a.Id == OtherItemPolicy.OtherItemId).FirstOrDefault();

                    retBrownAndWhiteDetails = new BrownAndWhiteDetailsResponseDto()
                    {
                        AddnSerialNo = OtherItemDetails.AddnSerialNo,
                        CategoryId = OtherItemDetails.CategoryId,
                        CommodityUsageTypeId = OtherItemDetails.CommodityUsageTypeId,
                        ConversionRate = OtherItemDetails.ConversionRate,
                        //CountryId = OtherItemDetails.CountryId,
                        currencyPeriodId = OtherItemDetails.currencyPeriodId,
                        DealerCurrencyId = OtherItemDetails.DealerCurrencyId,
                        //DealerId = OtherItemDetails.DealerId,
                        DealerPrice = OtherItemDetails.DealerPrice * policy.LocalCurrencyConversionRate,
                        EntryDateTime = OtherItemDetails.EntryDateTime,
                        EntryUser = OtherItemDetails.EntryUser,
                        Id = OtherItemDetails.Id,
                        InvoiceNo = OtherItemDetails.InvoiceNo,
                        ItemPrice = OtherItemDetails.ItemPrice * policy.LocalCurrencyConversionRate,
                        ItemPurchasedDate = OtherItemDetails.ItemPurchasedDate,
                        ItemStatusId = OtherItemDetails.ItemStatusId,
                        MakeId = OtherItemDetails.MakeId,
                        ModelCode = OtherItemDetails.ModelCode,
                        ModelId = OtherItemDetails.ModelId,
                        ModelYear = OtherItemDetails.ModelYear,
                        SerialNo = OtherItemDetails.SerialNo,
                        Variant = OtherItemDetails.VariantId,
                        //Variant = OtherItemDetails.Variant,
                    };

                    Make = session.Query<Make>().Where(a => a.Id == OtherItemDetails.MakeId).FirstOrDefault();
                    Model = session.Query<Model>().Where(a => a.Id == OtherItemDetails.ModelId).FirstOrDefault();
                    CommodityCategory = session.Query<CommodityCategory>().Where(a => a.CommodityCategoryId == OtherItemDetails.CategoryId).FirstOrDefault();
                }

                #endregion

                decimal PremiumData;

                if (policy.DealerPayment == 0 && policy.CustomerPayment == 0)
                {
                    PremiumData = policy.Premium;
                }
                else
                {
                    PremiumData = policy.DealerPayment + policy.CustomerPayment;
                }


                #region Customer
                Customer customer = session.Query<Customer>().Where(a => a.Id == policy.CustomerId).FirstOrDefault();
                //CustomersResponseDto customerData = new CustomersResponseDto();

                CustomerResponseDto customerD = new CustomerResponseDto()
                {
                    Address1 = customer.Address1,
                    Address2 = customer.Address2,
                    Address3 = customer.Address3,
                    Address4 = customer.Address4,
                    BusinessAddress1 = customer.BusinessAddress1,
                    BusinessAddress2 = customer.BusinessAddress2,
                    BusinessAddress3 = customer.BusinessAddress3,
                    BusinessAddress4 = customer.BusinessAddress4,
                    BusinessName = customer.BusinessName,
                    BusinessTelNo = customer.BusinessTelNo,
                    CityId = customer.CityId,
                    CountryId = customer.CountryId,
                    CustomerTypeId = customer.CustomerTypeId,
                    DateOfBirth = customer.DateOfBirth,
                    DLIssueDate = customer.DLIssueDate ,
                    EntryDateTime = customer.EntryDateTime,
                    Email = customer.Email,
                    EntryUserId = customer.EntryUserId,
                    FirstName = customer.FirstName,
                    Gender = customer.Gender,
                    LastName = customer.LastName,
                    IDNo = customer.IDNo,
                    Id = customer.Id.ToString(),
                    IDTypeId = customer.IDTypeId,
                    IsActive = customer.IsActive,
                    MaritalStatusId = customer.MaritalStatusId,
                    MobileNo = customer.MobileNo,
                    NationalityId = customer.NationalityId,
                    OccupationId = customer.OccupationId,
                    OtherTelNo = customer.OtherTelNo,
                    TitleId = customer.TitleId,
                    UsageTypeId = customer.UsageTypeId,
                    UserName = customer.UserName,
                };



                #endregion

                Contract Contract = session.Query<Contract>().Where(a => a.Id == policy.ContractId).FirstOrDefault();
                ReinsurerContract ReinsurerContract = session.Query<ReinsurerContract>().Where(a => a.Id == Contract.ReinsurerContractId).FirstOrDefault();

                ManufacturerWarrantyDetails mwd = session.Query<ManufacturerWarrantyDetails>().Where(a => a.ModelId == Model.Id &&
                                                    a.CountryId == ReinsurerContract.CountryId).FirstOrDefault();

                ManufacturerWarranty ManufacturerWarranty = null;
                ManufacturerWarrantyResponseDto ManufacturerWarrantyResponseDto = new ManufacturerWarrantyResponseDto();

                if (CommodityType.CommodityCode != "O" )
                {
                    if (Bundle[0].MWIsAvailable && mwd==null) {
                        return "Manufacture Warranty Not Set For Selected Make & Country";
                    }

                    if (mwd!=null) {
                        ManufacturerWarranty = session.Query<ManufacturerWarranty>().Where(a => a.Id == mwd.ManufacturerWarrantyId && a.MakeId == Make.Id).FirstOrDefault();

                        ManufacturerWarrantyResponseDto = new ManufacturerWarrantyResponseDto()
                        {
                            ApplicableFrom = ManufacturerWarranty.ApplicableFrom,
                            WarrantyKm = ManufacturerWarranty.IsUnlimited ? "Unlimited" : ManufacturerWarranty.WarrantyKm.ToString(),
                            WarrantyMonths = ManufacturerWarranty.WarrantyMonths,
                            WarrantyName = ManufacturerWarranty.WarrantyName,
                            Id = ManufacturerWarranty.Id,
                            IsUnlimited = ManufacturerWarranty.IsUnlimited
                        };
                    }


                    ContractInsuaranceLimitation CIL = session.Query<ContractInsuaranceLimitation>().Where(a => a.ContractId == Contract.Id).FirstOrDefault();
                    InsuaranceLimitation InsuaranceLimitation = session.Query<InsuaranceLimitation>().Where(a => a.CommodityTypeId == policy.CommodityTypeId && a.Id == CIL.InsuaranceLimitationId).FirstOrDefault();
                    ContractExtensions ContractExtensions = session.Query<ContractExtensions>().Where(a => a.ContractInsuanceLimitationId == CIL.Id).FirstOrDefault();

                    InsuaranceLimitationResponseDto InsuaranceLimitationResponseDto = new InsuaranceLimitationResponseDto()
                    {

                        CommodityTypeCode = InsuaranceLimitation.CommodityTypeCode,
                        CommodityTypeId = InsuaranceLimitation.CommodityTypeId,
                        Hrs = InsuaranceLimitation.Hrs,
                        Id = InsuaranceLimitation.Id,
                        InsuaranceLimitationName = InsuaranceLimitation.InsuaranceLimitationName,
                        Km = InsuaranceLimitation.Km,
                        Months = InsuaranceLimitation.Months,
                        TopOfMW = InsuaranceLimitation.TopOfMW,
                        IsRsa = InsuaranceLimitation.IsRsa
                    };

                    PolicyViewData result = new PolicyViewData()
                    {
                        Id = policyBundle.Id,
                        CommodityTypeId = policy.CommodityTypeId,
                        tpaBranchId = policy.TPABranchId,

                        ProductId = policy.ProductId,
                        DealerId = policy.DealerId,
                        DealerLocationId = policy.DealerLocationId,
                        ContractId = policy.ContractId,
                        ExtensionTypeId = policy.ExtensionTypeId,
                        //Premium = Bundle[0].DealerPayment + Bundle[0].CustomerPayment,
                        Premium = PremiumData,
                        PremiumCurrencyTypeId = policy.PremiumCurrencyTypeId,
                        DealerPaymentCurrencyTypeId = policy.DealerPaymentCurrencyTypeId,
                        CustomerPaymentCurrencyTypeId = policy.CustomerPaymentCurrencyTypeId,
                        CoverTypeId = policy.CoverTypeId,
                        HrsUsedAtPolicySale = policy.HrsUsedAtPolicySale,
                        IsPreWarrantyCheck = policy.IsPreWarrantyCheck,
                        PolicySoldDate = policy.PolicySoldDate,
                        SalesPersonId = policy.SalesPersonId,
                        PolicyNo = policy.PolicyNo,
                        IsSpecialDeal = policy.IsSpecialDeal,
                        IsPartialPayment = policy.IsPartialPayment,
                        DealerPayment = policy.DealerPayment,
                        CustomerPayment = policy.CustomerPayment,
                        PaymentModeId = policy.PaymentModeId,
                        PaymentTypeId = policy.PaymentTypeId,
                        RefNo = policy.RefNo,
                        Comment = policy.Comment,
                        ItemId = retVehicleDetails != null ? retVehicleDetails.Id : retBrownAndWhiteDetails.Id,
                        IsApproved = policy.IsApproved,
                        Type = "",
                        CustomerId = Bundle[0].CustomerId,
                        Vehicle = (retVehicleDetails == null || retVehicleDetails.Id == null) ? null : retVehicleDetails,
                        Customer = customerD,
                        // Customer = customerData.Customers.Find(c => c.Id == Bundle[0].CustomerId.ToString()),
                        BAndW = (retBrownAndWhiteDetails == null || retBrownAndWhiteDetails.Id == null) ? null : retBrownAndWhiteDetails,

                        EntryDateTime = Bundle[0].EntryDateTime,
                        EntryUser = Bundle[0].EntryUser,
                        PolicyStartDate = ManufacturerWarranty == null || !Bundle[0].MWIsAvailable ? Bundle[0].PolicySoldDate : Bundle[0].PolicyStartDate.AddDays(+1),
                        PolicyEndDate = Bundle[0].PolicyEndDate.AddDays(+1),
                        ContractProducts = retBundle,
                        Discount = Bundle[0].DiscountPercentage,
                        DiscountPercentage = Bundle[0].DiscountPercentage,
                        DealerPolicy = Bundle[0].DealerPolicy,
                        //chainging details after modification
                        MWEnddate = ManufacturerWarranty == null ? Bundle[0].PolicySoldDate : Bundle[0].MWStartDate.AddMonths(ManufacturerWarranty.WarrantyMonths).AddDays(-1),
                        MWStartDate = ManufacturerWarranty == null ? Bundle[0].PolicySoldDate : Bundle[0].MWStartDate,
                        MWKM = ManufacturerWarrantyResponseDto.WarrantyKm,
                        MWMonths = ManufacturerWarranty == null ? 0 : ManufacturerWarranty.WarrantyMonths,
                        ExtMonths = InsuaranceLimitation == null ? 0 : InsuaranceLimitation.Months,
                        ExtKM = InsuaranceLimitation == null ? 0 : InsuaranceLimitation.Km,
                        ExtStartDate = Bundle[0].MWIsAvailable ? Bundle[0].MWStartDate.AddMonths(ManufacturerWarranty.WarrantyMonths)
                            : Bundle[0].PolicySoldDate,
                        ExtEndDate = Bundle[0].MWIsAvailable ? Bundle[0].MWStartDate.AddMonths(ManufacturerWarranty.WarrantyMonths)
                                .AddMonths(InsuaranceLimitation == null ? 0 : InsuaranceLimitation.Months).AddDays(-1)
                            : Bundle[0].PolicySoldDate.AddMonths(InsuaranceLimitation == null ? 0
                            : InsuaranceLimitation.Months).AddDays(-1),

                        MWIsAvailable = Bundle[0].MWIsAvailable,
                        KMCutOff = GetKMCutOff(InsuaranceLimitationResponseDto, ManufacturerWarrantyResponseDto)

                    };

                    foreach (var item in result.ContractProducts)
                    {
                        Product Product = session.Query<Product>().Where(a => a.Id == policy.ProductId).FirstOrDefault();
                        item.Name = Product.Productcode + " - " + Product.Productname;


                        item.PolicyAdditionalDetails = policyAdditionalDetails;


                        if (item.PremiumCurrencyTypeId.ToString() != "00000000-0000-0000-0000-000000000000")
                        {

                            item.PremiumCurrencyName = Currency.Code;
                        }

                        List<Contract> Contracts = session.Query<Contract>().Where(c => c.CommodityTypeId == policy.CommodityTypeId && c.ProductId == policy.ProductId && c.DealerId==policy.DealerId
                        && c.CommodityUsageTypeId== retVehicleDetails.CommodityUsageTypeId && c.StartDate<=policy.PolicySoldDate && c.EndDate > policy.PolicySoldDate).ToList();
                        List<ContractResponseDto> ContractResponseDto = new List<ContractResponseDto>();

                        foreach (var ConRes in Contracts)
                        {
                            ContractResponseDto c = new ContractResponseDto
                            {
                                ClaimLimitation = ConRes.ClaimLimitation,
                                CommodityCategoryId = ConRes.CommodityCategoryId,
                                CommodityTypeId = ConRes.CommodityTypeId,
                                CommodityUsageTypeId = ConRes.CommodityUsageTypeId,
                                CountryId = ConRes.CountryId,
                                DealerId = ConRes.DealerId,
                                DealName = ConRes.DealName,
                                DealType = ConRes.DealType,
                                DiscountAvailable = ConRes.DiscountAvailable,
                                EndDate = ConRes.EndDate,
                                StartDate = ConRes.StartDate,
                                Id = ConRes.Id,
                                InsurerId = ConRes.InsurerId,
                                IsActive = ConRes.IsActive,
                                IsAutoRenewal = ConRes.IsAutoRenewal,
                                LiabilityLimitation = ConRes.LiabilityLimitation,
                                Remark = ConRes.Remark,
                                ProductId = ConRes.ProductId,
                                EntryUser = ConRes.EntryUser
                            };
                            ContractResponseDto.Add(c);
                        }

                        item.Contracts = ContractResponseDto;
                        if (CommodityType.CommodityCode == "A")
                        {
                            item.ExtensionTypes = ContractEntityManager.GetAllExtensionTypeByContractId(Bundle[0].ContractId,
                                    Bundle[0].ProductId, Bundle[0].DealerId, Bundle[0].PolicySoldDate,
                                    retVehicleDetails.CylinderCountId, retVehicleDetails.EngineCapacityId, retVehicleDetails.MakeId,
                                    retVehicleDetails.ModelId, retVehicleDetails.Variant, retVehicleDetails.GrossWeight);
                        }
                        else if (CommodityType.CommodityCode == "B")
                        {
                            item.ExtensionTypes = ContractEntityManager.GetAllExtensionTypeByContractId(Bundle[0].ContractId,
                                    Bundle[0].ProductId, Bundle[0].DealerId, Bundle[0].PolicySoldDate,
                                    retVehicleDetails.CylinderCountId, retVehicleDetails.EngineCapacityId, retVehicleDetails.MakeId,
                                    retVehicleDetails.ModelId, retVehicleDetails.Variant, retVehicleDetails.GrossWeight);
                        }
                        else
                        {
                            item.ExtensionTypes = ContractEntityManager.GetAllExtensionTypeByContractId(Bundle[0].ContractId,
                                    Bundle[0].ProductId, Bundle[0].DealerId, Bundle[0].PolicySoldDate,
                                    retVehicleDetails.CylinderCountId, retVehicleDetails.EngineCapacityId, retBrownAndWhiteDetails.MakeId, retBrownAndWhiteDetails.ModelId,
                                    retVehicleDetails.Variant, retVehicleDetails.GrossWeight);
                        }

                        List<ContractExtensions> ContractExtension = session.Query<ContractExtensions>().Where(a => a.ContractInsuanceLimitationId == CIL.Id).ToList();
                        List<ContractExtensionResponseDto> ContractExtensionResponseDto = new List<ContractExtensionResponseDto>();
                        foreach (var ContractExtRes in ContractExtension)
                        {
                            ContractExtensionResponseDto ContractExtensionResponse = new ContractExtensionResponseDto()
                            {
                                AttributeSpecification = ContractExtRes.AttributeSpecification,
                                ContractInsuanceLimitationId = ContractExtRes.ContractInsuanceLimitationId,
                                Id = ContractExtRes.Id,
                                RSAProviderId = ContractExtRes.RSAProviderId,
                                EntryDateTime = ContractExtRes.EntryDateTime,
                                EntryUser = ContractExtRes.EntryUser,
                                Rate = ContractExtRes.Rate,
                                RegionId = ContractExtRes.RegionId,
                            };
                            ContractExtensionResponseDto.Add(ContractExtensionResponse);
                        }

                        item.AttributeSpecifications = ContractExtensionResponseDto;

                        WarrantyType Covertypes = session.Query<WarrantyType>().FirstOrDefault();


                        List<Guid> ContractIds = new List<Guid>();
                        foreach (var c in Contracts)
                        {
                            ContractIds.Add(c.Id);
                        }

                        if (Product.Productcode == "RSA")
                        {
                            List<Guid> RSAIds = new List<Guid>();
                            foreach (var r in ContractExtensionResponseDto)
                            {
                                RSAIds.Add(r.RSAProviderId);
                            }
                        }
                        else
                        {
                            List<Guid> Warranty = new List<Guid>();
                            foreach (var r in ContractExtensionResponseDto)
                            {
                                Warranty.Add(r.WarrantyTypeId);
                            }

                            if (CommodityType.CommodityCode == "A")
                            {
                                item.CoverTypess = ContractEntityManager.GetGetCoverTypesByExtensionIdUnitOfWork2(Bundle[0].ContractInsuaranceLimitationId,
                                            Bundle[0].ContractExtensionsId, Bundle[0].ContractId,
                                            policy.ProductId, policy.DealerId, Bundle[0].PolicySoldDate, retVehicleDetails.CylinderCountId,
                                            retVehicleDetails.EngineCapacityId,
                                            retVehicleDetails.MakeId, retVehicleDetails.ModelId, retVehicleDetails.Variant,
                                            retVehicleDetails.GrossWeight, retVehicleDetails.ItemStatusId);
                            }
                            else if (CommodityType.CommodityCode == "B")
                            {
                                item.CoverTypess = ContractEntityManager.GetGetCoverTypesByExtensionIdUnitOfWork2(Bundle[0].ContractInsuaranceLimitationId,
                                            Bundle[0].ContractExtensionsId, Bundle[0].ContractId,
                                            policy.ProductId, policy.DealerId, Bundle[0].PolicySoldDate, retVehicleDetails.CylinderCountId,
                                            retVehicleDetails.EngineCapacityId,
                                            retVehicleDetails.MakeId, retVehicleDetails.ModelId, retVehicleDetails.Variant,
                                            retVehicleDetails.GrossWeight, retVehicleDetails.ItemStatusId);
                            }
                            else
                            {
                                item.CoverTypess = ContractEntityManager.GetGetCoverTypesByExtensionIdUnitOfWork2(Bundle[0].ContractInsuaranceLimitationId,
                                            Bundle[0].ContractExtensionsId, Bundle[0].ContractId,
                                            policy.ProductId, policy.DealerId, Bundle[0].PolicySoldDate, retVehicleDetails.CylinderCountId,
                                            retVehicleDetails.EngineCapacityId,
                                            retBrownAndWhiteDetails.MakeId, retBrownAndWhiteDetails.ModelId, retVehicleDetails.Variant,
                                            retVehicleDetails.GrossWeight, retBrownAndWhiteDetails.ItemStatusId);
                            }
                        }
                    }

                    result.IsEndorsementApprovalPending = CheckEndosmentApprovalPending(policy.Id);
                    return result;
                }
                else
                {
                    ContractInsuaranceLimitation CIL = session.Query<ContractInsuaranceLimitation>().Where(a => a.ContractId == Contract.Id).FirstOrDefault();
                    InsuaranceLimitation InsuaranceLimitation = session.Query<InsuaranceLimitation>().Where(a => a.CommodityTypeId == policy.CommodityTypeId && a.Id == CIL.InsuaranceLimitationId).FirstOrDefault();
                    ContractExtensions ContractExtensions = session.Query<ContractExtensions>().Where(a => a.ContractInsuanceLimitationId == CIL.Id).FirstOrDefault();

                    InsuaranceLimitationResponseDto InsuaranceLimitationResponseDto = new InsuaranceLimitationResponseDto()
                    {

                        CommodityTypeCode = InsuaranceLimitation.CommodityTypeCode,
                        CommodityTypeId = InsuaranceLimitation.CommodityTypeId,
                        Hrs = InsuaranceLimitation.Hrs,
                        Id = InsuaranceLimitation.Id,
                        InsuaranceLimitationName = InsuaranceLimitation.InsuaranceLimitationName,
                        Km = InsuaranceLimitation.Km,
                        Months = InsuaranceLimitation.Months,
                        TopOfMW = InsuaranceLimitation.TopOfMW,
                        IsRsa = InsuaranceLimitation.IsRsa
                    };

                    PolicyViewData result = new PolicyViewData()
                    {
                        Id = policyBundle.Id,
                        CommodityTypeId = policy.CommodityTypeId,
                        tpaBranchId = policy.TPABranchId,

                        ProductId = policy.ProductId,
                        DealerId = policy.DealerId,
                        DealerLocationId = policy.DealerLocationId,
                        ContractId = policy.ContractId,
                        ExtensionTypeId = policy.ExtensionTypeId,
                        //Premium = Bundle[0].DealerPayment + Bundle[0].CustomerPayment,
                        Premium = PremiumData,
                        PremiumCurrencyTypeId = policy.PremiumCurrencyTypeId,
                        DealerPaymentCurrencyTypeId = policy.DealerPaymentCurrencyTypeId,
                        CustomerPaymentCurrencyTypeId = policy.CustomerPaymentCurrencyTypeId,
                        CoverTypeId = policy.CoverTypeId,
                        HrsUsedAtPolicySale = policy.HrsUsedAtPolicySale,
                        IsPreWarrantyCheck = policy.IsPreWarrantyCheck,
                        PolicySoldDate = policy.PolicySoldDate,
                        SalesPersonId = policy.SalesPersonId,
                        PolicyNo = policy.PolicyNo,
                        IsSpecialDeal = policy.IsSpecialDeal,
                        IsPartialPayment = policy.IsPartialPayment,
                        DealerPayment = policy.DealerPayment,
                        CustomerPayment = policy.CustomerPayment,
                        PaymentModeId = policy.PaymentModeId,
                        PaymentTypeId = policy.PaymentTypeId,
                        RefNo = policy.RefNo,
                        Comment = policy.Comment,
                        ItemId = retVehicleDetails != null ? retVehicleDetails.Id : retBrownAndWhiteDetails.Id,
                        IsApproved = policy.IsApproved,
                        Type = "",
                        CustomerId = Bundle[0].CustomerId,
                        Vehicle = (retVehicleDetails == null || retVehicleDetails.Id == null) ? null : retVehicleDetails,
                        Customer = customerD,
                        BAndW = (retBrownAndWhiteDetails == null || retBrownAndWhiteDetails.Id == null) ? null : retBrownAndWhiteDetails,
                        ExtMonths = InsuaranceLimitation == null ? 0 : InsuaranceLimitation.Months,
                        ExtKM = InsuaranceLimitation == null ? 0 : InsuaranceLimitation.Km,
                        EntryDateTime = Bundle[0].EntryDateTime,
                        EntryUser = Bundle[0].EntryUser,
                        PolicyStartDate = ManufacturerWarranty == null || !Bundle[0].MWIsAvailable ? Bundle[0].PolicySoldDate : Bundle[0].PolicyStartDate.AddDays(+1),
                        PolicyEndDate = Bundle[0].PolicyEndDate,
                        ContractProducts = retBundle,
                        Discount = Bundle[0].DiscountPercentage,
                        DiscountPercentage = Bundle[0].DiscountPercentage,
                        DealerPolicy = Bundle[0].DealerPolicy,

                    };

                    foreach (var item in result.ContractProducts)
                    {

                        Product Product = session.Query<Product>().Where(a => a.Id == policy.ProductId).FirstOrDefault();
                        item.Name = Product.Productcode + " - " + Product.Productname;

                        if (item.PremiumCurrencyTypeId.ToString() != "00000000-0000-0000-0000-000000000000")
                        {

                            item.PremiumCurrencyName = Currency.Code;
                        }

                        List<Contract> Contracts = session.Query<Contract>().Where(c => c.CommodityTypeId == policy.CommodityTypeId).ToList();
                        List<ContractResponseDto> ContractResponseDto = new List<ContractResponseDto>();

                        foreach (var ConRes in Contracts)
                        {
                            ContractResponseDto c = new ContractResponseDto
                            {
                                ClaimLimitation = ConRes.ClaimLimitation,
                                CommodityCategoryId = ConRes.CommodityCategoryId,
                                CommodityTypeId = ConRes.CommodityTypeId,
                                CommodityUsageTypeId = ConRes.CommodityUsageTypeId,
                                CountryId = ConRes.CountryId,
                                DealerId = ConRes.DealerId,
                                DealName = ConRes.DealName,
                                DealType = ConRes.DealType,
                                DiscountAvailable = ConRes.DiscountAvailable,
                                EndDate = ConRes.EndDate,
                                StartDate = ConRes.StartDate,
                                Id = ConRes.Id,
                                InsurerId = ConRes.InsurerId,
                                IsActive = ConRes.IsActive,
                                IsAutoRenewal = ConRes.IsAutoRenewal,
                                LiabilityLimitation = ConRes.LiabilityLimitation,
                                Remark = ConRes.Remark,
                                ProductId = ConRes.ProductId,
                                EntryUser = ConRes.EntryUser
                            };
                            ContractResponseDto.Add(c);
                        }

                        item.Contracts = ContractResponseDto;
                        if (CommodityType.CommodityCode == "A")
                        {
                            item.ExtensionTypes = ContractEntityManager.GetAllExtensionTypeByContractId(Bundle[0].ContractId,
                                    Bundle[0].ProductId, Bundle[0].DealerId, Bundle[0].PolicySoldDate,
                                    retVehicleDetails.CylinderCountId, retVehicleDetails.EngineCapacityId, retVehicleDetails.MakeId,
                                    retVehicleDetails.ModelId, retVehicleDetails.Variant, retVehicleDetails.GrossWeight);
                        }
                        else if (CommodityType.CommodityCode == "B")
                        {
                            item.ExtensionTypes = ContractEntityManager.GetAllExtensionTypeByContractId(Bundle[0].ContractId,
                                    Bundle[0].ProductId, Bundle[0].DealerId, Bundle[0].PolicySoldDate,
                                    retVehicleDetails.CylinderCountId, retVehicleDetails.EngineCapacityId, retVehicleDetails.MakeId,
                                    retVehicleDetails.ModelId, retVehicleDetails.Variant, retVehicleDetails.GrossWeight);
                        }
                        else
                        {
                            item.ExtensionTypes = ContractEntityManager.GetAllExtensionTypeByContractId(Bundle[0].ContractId,
                                    Bundle[0].ProductId, Bundle[0].DealerId, Bundle[0].PolicySoldDate,
                                    retVehicleDetails.CylinderCountId, retVehicleDetails.EngineCapacityId, retBrownAndWhiteDetails.MakeId, retBrownAndWhiteDetails.ModelId,
                                    retVehicleDetails.Variant, retVehicleDetails.GrossWeight);
                        }

                        List<ContractExtensions> ContractExtension = session.Query<ContractExtensions>().Where(a => a.ContractInsuanceLimitationId == CIL.Id).ToList();
                        List<ContractExtensionResponseDto> ContractExtensionResponseDto = new List<ContractExtensionResponseDto>();
                        foreach (var ContractExtRes in ContractExtension)
                        {
                            ContractExtensionResponseDto ContractExtensionResponse = new ContractExtensionResponseDto()
                            {
                                AttributeSpecification = ContractExtRes.AttributeSpecification,
                                ContractInsuanceLimitationId = ContractExtRes.ContractInsuanceLimitationId,
                                Id = ContractExtRes.Id,
                                RSAProviderId = ContractExtRes.RSAProviderId,
                                EntryDateTime = ContractExtRes.EntryDateTime,
                                EntryUser = ContractExtRes.EntryUser,
                                Rate = ContractExtRes.Rate,
                                RegionId = ContractExtRes.RegionId,
                            };
                            ContractExtensionResponseDto.Add(ContractExtensionResponse);
                        }

                        item.AttributeSpecifications = ContractExtensionResponseDto;

                        WarrantyType Covertypes = session.Query<WarrantyType>().FirstOrDefault();


                        List<Guid> ContractIds = new List<Guid>();
                        foreach (var c in Contracts)
                        {
                            ContractIds.Add(c.Id);
                        }

                        if (Product.Productcode == "RSA")
                        {
                            List<Guid> RSAIds = new List<Guid>();
                            foreach (var r in ContractExtensionResponseDto)
                            {
                                RSAIds.Add(r.RSAProviderId);
                            }
                        }
                        else
                        {
                            List<Guid> Warranty = new List<Guid>();
                            foreach (var r in ContractExtensionResponseDto)
                            {
                                Warranty.Add(r.WarrantyTypeId);
                            }

                            if (CommodityType.CommodityCode == "A")
                            {
                                item.CoverTypess = ContractEntityManager.GetGetCoverTypesByExtensionIdUnitOfWork2(Bundle[0].ContractInsuaranceLimitationId,
                                            Bundle[0].ContractExtensionsId, Bundle[0].ContractId,
                                            policy.ProductId, policy.DealerId, Bundle[0].PolicySoldDate, retVehicleDetails.CylinderCountId,
                                            retVehicleDetails.EngineCapacityId,
                                            retVehicleDetails.MakeId, retVehicleDetails.ModelId, retVehicleDetails.Variant,
                                            retVehicleDetails.GrossWeight, retVehicleDetails.ItemStatusId);
                            }
                            else if (CommodityType.CommodityCode == "B")
                            {
                                item.CoverTypess = ContractEntityManager.GetGetCoverTypesByExtensionIdUnitOfWork2(Bundle[0].ContractInsuaranceLimitationId,
                                            Bundle[0].ContractExtensionsId, Bundle[0].ContractId,
                                            policy.ProductId, policy.DealerId, Bundle[0].PolicySoldDate, retVehicleDetails.CylinderCountId,
                                            retVehicleDetails.EngineCapacityId,
                                            retVehicleDetails.MakeId, retVehicleDetails.ModelId, retVehicleDetails.Variant,
                                            retVehicleDetails.GrossWeight, retVehicleDetails.ItemStatusId);
                            }
                            else
                            {
                                item.CoverTypess = ContractEntityManager.GetGetCoverTypesByExtensionIdUnitOfWork2(Bundle[0].ContractInsuaranceLimitationId,
                                            Bundle[0].ContractExtensionsId, Bundle[0].ContractId,
                                            policy.ProductId, policy.DealerId, Bundle[0].PolicySoldDate, retVehicleDetails.CylinderCountId,
                                            retVehicleDetails.EngineCapacityId,
                                            retBrownAndWhiteDetails.MakeId, retBrownAndWhiteDetails.ModelId, retVehicleDetails.Variant,
                                            retVehicleDetails.GrossWeight, retBrownAndWhiteDetails.ItemStatusId);
                            }
                        }
                    }

                    result.IsEndorsementApprovalPending = CheckEndosmentApprovalPending(policy.Id);
                    return result;
                }
                //ManufacturerWarranty ManufacturerWarrantyD = session.Query<ManufacturerWarranty>().Where(a => a.Id == mwd.ManufacturerWarrantyId && a.MakeId == Make.Id).FirstOrDefault();

            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ((ex.InnerException != null) ? ex.InnerException.ToString() : ""));

                return "";
                //Response = "Error occurred whilie Loading policy details..";
            }
        }

        private int GetKMCutOff(InsuaranceLimitationResponseDto insuaranceLimitation, ManufacturerWarrantyResponseDto manufacturerWarrantyD)
        {
            int cutoff = 0;
            if (insuaranceLimitation != null)
            {
                if (insuaranceLimitation.TopOfMW)
                {
                    if (manufacturerWarrantyD.IsUnlimited)
                    {
                        cutoff = Decimal.ToInt32(insuaranceLimitation.Km);
                    }
                    else
                    {
                        int ManufactureWarrantyKm = 0;
                        if (manufacturerWarrantyD != null && manufacturerWarrantyD.WarrantyKm != null) {
                            ManufactureWarrantyKm = int.Parse(manufacturerWarrantyD.WarrantyKm);
                        }
                        cutoff = Decimal.ToInt32(insuaranceLimitation.Km) + ManufactureWarrantyKm;
                    }
                }
                else
                {
                    cutoff = Decimal.ToInt32(insuaranceLimitation.Km);
                }
            }
            return cutoff;
        }

        public class PolicyViewData : PolicyResponseDto
        {
            public VehicleDetailsResponseDto Vehicle { get; set; }
            public CustomerResponseDto Customer { get; set; }
            public BrownAndWhiteDetailsResponseDto BAndW { get; set; }

            public Guid ModifiedUser { get; set; }

        }

        #endregion

        #region Policy Bundle
        public List<PolicyBundle> GetPolicyBundles()
        {
            List<PolicyBundle> entities = new List<PolicyBundle>();
            ISession session = EntitySessionManager.GetSession();
            IQueryable<PolicyBundle> PolicyData = session.Query<PolicyBundle>();
            entities = PolicyData.ToList();
            return entities;
        }
        public PolicyBundleResponseDto GetPolicyBundleById(Guid PolicyId)
        {
            ISession session = EntitySessionManager.GetSession();
            CurrencyEntityManager cem = new CurrencyEntityManager();
            PolicyBundleResponseDto pDto = new PolicyBundleResponseDto();

            var query =
                from PolicyBundle in session.Query<PolicyBundle>()
                where PolicyBundle.Id == PolicyId
                select new { Policy = PolicyBundle };
            var result = query.ToList();

            Guid currencyPeriodId = session.Query<Policy>().
                Where(a => a.PolicyBundleId == result.First().Policy.Id).FirstOrDefault().CurrencyPeriodId;
            Guid premiumCurrencyId = session.Query<Policy>().
                Where(a => a.PolicyBundleId == result.First().Policy.Id).FirstOrDefault().PremiumCurrencyTypeId;


            if (result != null && result.Count > 0)
            {
                pDto.Id = result.First().Policy.Id;
                pDto.Comment = result.First().Policy.Comment;
                pDto.CommodityTypeId = result.First().Policy.CommodityTypeId;
                pDto.ContractId = result.First().Policy.ContractId;
                pDto.CoverTypeId = result.First().Policy.CoverTypeId;
                pDto.PremiumCurrencyTypeId = result.First().Policy.PremiumCurrencyTypeId;
                pDto.DealerPaymentCurrencyTypeId = result.First().Policy.DealerPaymentCurrencyTypeId;
                pDto.CustomerPaymentCurrencyTypeId = result.First().Policy.CustomerPaymentCurrencyTypeId;
                pDto.CustomerId = result.First().Policy.CustomerId;
                pDto.CustomerPayment = cem.ConvertFromBaseCurrency(result.First().Policy.CustomerPayment,
                        premiumCurrencyId, currencyPeriodId);
                pDto.DealerId = result.First().Policy.DealerId;
                pDto.DealerLocationId = result.First().Policy.DealerLocationId;
                pDto.DealerPayment = cem.ConvertFromBaseCurrency(result.First().Policy.DealerPayment,
                        premiumCurrencyId, currencyPeriodId);
                pDto.ExtensionTypeId = result.First().Policy.ExtensionTypeId;
                pDto.HrsUsedAtPolicySale = result.First().Policy.HrsUsedAtPolicySale;
                pDto.IsPartialPayment = result.First().Policy.IsPartialPayment;
                pDto.IsPreWarrantyCheck = result.First().Policy.IsPreWarrantyCheck;
                pDto.IsSpecialDeal = result.First().Policy.IsSpecialDeal;
                pDto.ProductId = result.First().Policy.ProductId;
                pDto.PaymentModeId = result.First().Policy.PaymentModeId;
                pDto.PaymentTypeId = result.First().Policy.PaymentTypeId;
                pDto.PolicyNo = result.First().Policy.PolicyNo;
                pDto.PolicySoldDate = result.First().Policy.PolicySoldDate;
                pDto.Premium = cem.ConvertFromBaseCurrency(result.First().Policy.Premium,
                        premiumCurrencyId, currencyPeriodId);
                pDto.RefNo = result.First().Policy.RefNo;
                pDto.SalesPersonId = result.First().Policy.SalesPersonId;
                pDto.EntryDateTime = result.First().Policy.EntryDateTime;
                pDto.EntryUser = result.First().Policy.EntryUser;
                pDto.IsApproved = result.First().Policy.IsApproved;
                pDto.IsPolicyCanceled = result.First().Policy.IsPolicyCanceled;
                // pDto.PolicyStartDate = result.First().Policy.PolicyStartDate;
                // pDto.PolicyEndDate = result.First().Policy.PolicyEndDate;
                pDto.Discount = cem.ConvertFromBaseCurrency(result.First().Policy.Discount,
                        premiumCurrencyId, currencyPeriodId);
                pDto.BookletNumber = result.First().Policy.BookletNumber;
                pDto.ContractExtensionPremiumId = result.First().Policy.ContractExtensionPremiumId;
                pDto.ContractInsuaranceLimitationId = result.First().Policy.ContractInsuaranceLimitationId;
                pDto.ContractExtensionsId = result.First().Policy.ContractExtensionsId;
                pDto.MWStartDate = result.First().Policy.MWStartDate;
                //result.First().Policy.Discount;
                pDto.DealerPolicy = result.First().Policy.DealerPolicy;

                pDto.IsPolicyBundleExists = true;
                return pDto;
            }
            else
            {
                pDto.IsPolicyBundleExists = false;
                return new PolicyBundleResponseDto();
            }
        }
        public bool AddPolicyBundle(PolicyBundleRequestDto Policy)
        {
            try
            {

                PolicyBundle pr = new PolicyBundle();
                if (!IsGuid(Policy.Id.ToString()))
                {
                    pr.Id = new Guid();
                }
                else
                {
                    pr.Id = Policy.Id;
                }

                pr.Comment = Policy.Comment;
                pr.CommodityTypeId = Policy.CommodityTypeId;
                pr.DealerPaymentCurrencyTypeId = Policy.DealerPaymentCurrencyTypeId;
                pr.CustomerPaymentCurrencyTypeId = Policy.CustomerPaymentCurrencyTypeId;
                pr.CustomerId = Policy.CustomerId;
                pr.CustomerPayment = Policy.CustomerPayment;
                pr.DealerId = Policy.DealerId;
                pr.DealerLocationId = Policy.DealerLocationId;
                pr.DealerPayment = Policy.DealerPayment;
                pr.ExtensionTypeId = Policy.ExtensionTypeId;
                pr.HrsUsedAtPolicySale = Policy.HrsUsedAtPolicySale;
                pr.IsPartialPayment = Policy.IsPartialPayment;
                pr.IsPreWarrantyCheck = Policy.IsPreWarrantyCheck;
                pr.IsSpecialDeal = Policy.IsSpecialDeal;
                pr.ProductId = Policy.ProductId;
                pr.PaymentModeId = Policy.PaymentModeId;
                pr.PaymentTypeId = Policy.PaymentTypeId;
                pr.PolicyNo = Policy.PolicyNo;
                pr.PolicySoldDate = Policy.PolicySoldDate;
                pr.Premium = Policy.Premium;
                pr.IsApproved = Policy.IsApproved;
                pr.IsPolicyCanceled = Policy.IsPolicyCanceled;
                pr.RefNo = Policy.RefNo;
                pr.SalesPersonId = Policy.SalesPersonId;
                pr.Discount = Policy.Discount;
                pr.DealerPolicy = Policy.DealerPolicy;
                pr.EntryDateTime = DateTime.UtcNow;
                pr.EntryUser = Guid.Parse("ba56ec84-1fe0-4385-abe4-182f62caa050");
                pr.BookletNumber = Policy.BookletNumber;
                pr.MWStartDate = Policy.MWStartDate;
                pr.MWIsAvailable = Policy.MWIsAvailable;
                pr.ContractExtensionPremiumId = Policy.CoverTypeId;
                pr.ContractExtensionsId = Policy.ExtensionTypeId;
                pr.ContractInsuaranceLimitationId = Policy.AttributeSpecificationId;

                ISession session = EntitySessionManager.GetSession();
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Save(pr, pr.Id);
                    transaction.Commit();
                }
                Policy.Id = pr.Id;
                return true;
            }
            catch (SqlTypeException e)
            {
                foreach (var key in e.Data.Keys)
                {
                    //System.Console.Write("Key is " + key.ToString());
                }
                foreach (var value in e.Data.Values)
                {
                    //Console.WriteLine("Value is " + value.ToString());
                }
                return false;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ((ex.InnerException != null) ? ex.InnerException.ToString() : ""));
                return false;
            }
        }
        internal bool UpdatePolicyBundle(PolicyBundleRequestDto Policy)
        {
            try
            {
                CurrencyEntityManager currencyEm = new CurrencyEntityManager();
                Guid currentCurrencyPeriodId = currencyEm.GetCurrentCurrencyPeriodId();
                ISession session = EntitySessionManager.GetSession();
                PolicyBundle pr = new Entities.PolicyBundle();
                session.Load(pr, Policy.Id);
                pr.Id = Policy.Id;
                pr.Comment = Policy.Comment;
                pr.CommodityTypeId = Policy.CommodityTypeId;
                pr.DealerPaymentCurrencyTypeId = Policy.DealerPaymentCurrencyTypeId;
                pr.CustomerPaymentCurrencyTypeId = Policy.CustomerPaymentCurrencyTypeId;
                pr.CustomerId = Policy.CustomerId;
                pr.CustomerPayment = currencyEm.ConvertToBaseCurrency(Policy.CustomerPayment, Policy.DealerPaymentCurrencyTypeId, currentCurrencyPeriodId);
                pr.DealerId = Policy.DealerId;
                pr.DealerLocationId = Policy.DealerLocationId;
                pr.DealerPayment = currencyEm.ConvertToBaseCurrency(Policy.DealerPayment, Policy.DealerPaymentCurrencyTypeId, currentCurrencyPeriodId);
                pr.HrsUsedAtPolicySale = Policy.HrsUsedAtPolicySale;
                pr.IsPartialPayment = Policy.IsPartialPayment;
                pr.IsPreWarrantyCheck = Policy.IsPreWarrantyCheck;
                pr.IsSpecialDeal = Policy.IsSpecialDeal;
                pr.ProductId = Policy.ProductId;
                pr.PaymentModeId = Policy.PaymentModeId;
                pr.PaymentTypeId = Policy.PaymentTypeId;

                pr.PolicySoldDate = Policy.PolicySoldDate;
                pr.Premium = currencyEm.ConvertToBaseCurrency(Policy.Premium, Policy.DealerPaymentCurrencyTypeId, currentCurrencyPeriodId);
                pr.RefNo = Policy.RefNo;
                pr.SalesPersonId = Policy.SalesPersonId;
                //pr.EntryDateTime = Policy.EntryDateTime;
                //pr.EntryUser = Policy.EntryUser;
                pr.IsApproved = Policy.IsApproved;
                pr.IsPolicyCanceled = Policy.IsPolicyCanceled;
                pr.Discount = Policy.Discount;
                pr.DealerPolicy = Policy.DealerPolicy;

                if (Policy.Type == "B&W")
                {
                    pr.MWStartDate = Convert.ToDateTime("1900/10/10");
                }
                else
                {
                    pr.MWStartDate = Policy.MWStartDate;
                }

                pr.ContractExtensionPremiumId = Policy.CoverTypeId;
                pr.ContractExtensionsId = Policy.ExtensionTypeId;
                pr.ContractInsuaranceLimitationId = Policy.AttributeSpecificationId;

                if (pr.MWStartDate == DateTime.MinValue)
                {
                    pr.MWStartDate = SqlDateTime.MinValue.Value;
                }

                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Evict(pr);
                    session.Update(pr, pr.Id);
                    transaction.Commit();
                }

                return true;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ((ex.InnerException != null) ? ex.InnerException.ToString() : ""));
                return false;
            }
        }
        #endregion

        #region History Bundle
        public List<PolicyBundleHistory> GetPolicyBundleHistories()
        {
            List<PolicyBundleHistory> entities = new List<PolicyBundleHistory>();
            ISession session = EntitySessionManager.GetSession();
            IQueryable<PolicyBundleHistory> PolicyData = session.Query<PolicyBundleHistory>();
            entities = PolicyData.ToList();
            return entities;
        }
        public PolicyBundleHistoryResponseDto GetPolicyBundleHistoryById(Guid PolicyId)
        {

            try
            {
                ISession session = EntitySessionManager.GetSession();

                PolicyBundleHistoryResponseDto pDto = new PolicyBundleHistoryResponseDto();

                var query =
                    from PolicyBundleHistory in session.Query<PolicyBundleHistory>()
                    where PolicyBundleHistory.Id == PolicyId
                    select new { Policy = PolicyBundleHistory };
                var result = query.ToList();

                if (result != null && result.Count > 0)
                {
                    pDto.Id = result.First().Policy.Id;
                    pDto.Comment = result.First().Policy.Comment;
                    pDto.CommodityTypeId = result.First().Policy.CommodityTypeId;
                    pDto.ContractId = result.First().Policy.ContractId;
                    pDto.CoverTypeId = result.First().Policy.CoverTypeId;
                    pDto.PremiumCurrencyTypeId = result.First().Policy.PremiumCurrencyTypeId;
                    pDto.DealerPaymentCurrencyTypeId = result.First().Policy.DealerPaymentCurrencyTypeId;
                    pDto.CustomerPaymentCurrencyTypeId = result.First().Policy.CustomerPaymentCurrencyTypeId;
                    pDto.CustomerId = result.First().Policy.CustomerId;
                    pDto.CustomerPayment = result.First().Policy.CustomerPayment;
                    pDto.DealerId = result.First().Policy.DealerId;
                    pDto.DealerLocationId = result.First().Policy.DealerLocationId;
                    pDto.DealerPayment = result.First().Policy.DealerPayment;
                    pDto.ExtensionTypeId = result.First().Policy.ExtensionTypeId;
                    pDto.HrsUsedAtPolicySale = result.First().Policy.HrsUsedAtPolicySale;
                    pDto.IsPartialPayment = result.First().Policy.IsPartialPayment;
                    pDto.IsPreWarrantyCheck = result.First().Policy.IsPreWarrantyCheck;
                    pDto.IsSpecialDeal = result.First().Policy.IsSpecialDeal;
                    pDto.ProductId = result.First().Policy.ProductId;
                    pDto.PaymentModeId = result.First().Policy.PaymentModeId;
                    pDto.PolicyNo = result.First().Policy.PolicyNo;
                    pDto.PolicySoldDate = result.First().Policy.PolicySoldDate;
                    pDto.Premium = result.First().Policy.Premium;
                    pDto.RefNo = result.First().Policy.RefNo;
                    pDto.SalesPersonId = result.First().Policy.SalesPersonId;
                    pDto.EntryDateTime = result.First().Policy.EntryDateTime;
                    pDto.EntryUser = result.First().Policy.EntryUser;
                    pDto.IsApproved = result.First().Policy.IsApproved;
                    pDto.IsPolicyCanceled = result.First().Policy.IsPolicyCanceled;
                    pDto.Discount = result.First().Policy.Discount;
                    pDto.DealerPolicy = result.First().Policy.DealerPolicy;

                    pDto.IsPolicyBundleExists = true;
                    return pDto;
                }
                else
                {
                    pDto.IsPolicyBundleExists = false;
                    return null;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ((ex.InnerException != null) ? ex.InnerException.ToString() : ""));
                return null;
            }

        }
        internal bool AddPolicyBundleHistory(PolicyBundleHistoryRequestDto Policy)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();

                PolicyBundleHistory pr = new Entities.PolicyBundleHistory();
                pr.Id = new Guid();
                pr.Comment = Policy.Comment;
                pr.CommodityTypeId = Policy.CommodityTypeId;
                pr.DealerPaymentCurrencyTypeId = Policy.DealerPaymentCurrencyTypeId;
                pr.CustomerPaymentCurrencyTypeId = Policy.CustomerPaymentCurrencyTypeId;
                pr.CustomerId = Policy.CustomerId;
                pr.CustomerPayment = Policy.CustomerPayment;
                pr.DealerId = Policy.DealerId;
                pr.DealerLocationId = Policy.DealerLocationId;
                pr.DealerPayment = Policy.DealerPayment;
                pr.ExtensionTypeId = Policy.ExtensionTypeId;
                pr.HrsUsedAtPolicySale = Policy.HrsUsedAtPolicySale;
                pr.IsPartialPayment = Policy.IsPartialPayment;
                pr.IsPreWarrantyCheck = Policy.IsPreWarrantyCheck;
                pr.IsSpecialDeal = Policy.IsSpecialDeal;
                pr.ProductId = Policy.ProductId;
                pr.PaymentModeId = Policy.PaymentModeId;
                pr.PolicyNo = Policy.PolicyNo;
                pr.PolicySoldDate = Policy.PolicySoldDate;
                pr.Premium = Policy.Premium;
                pr.IsApproved = Policy.IsApproved;
                pr.IsPolicyCanceled = Policy.IsPolicyCanceled;
                pr.RefNo = Policy.RefNo;
                pr.SalesPersonId = Policy.SalesPersonId;
                pr.Discount = Policy.Discount;
                pr.DealerPolicy = Policy.DealerPolicy;
                pr.EntryDateTime = DateTime.Today.ToUniversalTime();
                pr.EntryUser = Guid.Parse("ba56ec84-1fe0-4385-abe4-182f62caa050");

                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.SaveOrUpdate(pr);
                    Policy.Id = pr.Id;
                    transaction.Commit();
                }
                return true;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ((ex.InnerException != null) ? ex.InnerException.ToString() : ""));
                return false;
            }
        }
        #endregion

        #region Transaction Bundle
        public List<PolicyBundleTransaction> GetPolicyBundleTransactions()
        {
            List<PolicyBundleTransaction> entities = new List<PolicyBundleTransaction>();
            ISession session = EntitySessionManager.GetSession();
            IQueryable<PolicyBundleTransaction> PolicyData = session.Query<PolicyBundleTransaction>();
            entities = PolicyData.ToList();
            return entities;
        }
        public PolicyBundleTransactionResponseDto GetPolicyBundleTransactionById(Guid PolicyId)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();

                PolicyBundleTransactionResponseDto pDto = new PolicyBundleTransactionResponseDto();

                var query =
                    from PolicyBundleTransaction in session.Query<PolicyBundleTransaction>()
                    where PolicyBundleTransaction.Id == PolicyId
                    select new { Policy = PolicyBundleTransaction };
                var result = query.ToList();

                if (result != null && result.Count > 0)
                {
                    pDto.Id = result.First().Policy.Id;
                    pDto.PolicyId = result.First().Policy.PolicyBundleId;
                    pDto.Comment = result.First().Policy.Comment;
                    pDto.CommodityTypeId = result.First().Policy.CommodityTypeId;
                    pDto.ContractId = result.First().Policy.ContractId;
                    pDto.CoverTypeId = result.First().Policy.CoverTypeId;
                    pDto.PremiumCurrencyTypeId = result.First().Policy.PremiumCurrencyTypeId;
                    pDto.DealerPaymentCurrencyTypeId = result.First().Policy.DealerPaymentCurrencyTypeId;
                    pDto.CustomerPaymentCurrencyTypeId = result.First().Policy.CustomerPaymentCurrencyTypeId;
                    pDto.CustomerId = result.First().Policy.CustomerId;
                    pDto.CustomerPayment = result.First().Policy.CustomerPayment;
                    pDto.DealerId = result.First().Policy.DealerId;
                    pDto.DealerLocationId = result.First().Policy.DealerLocationId;
                    pDto.DealerPayment = result.First().Policy.DealerPayment;
                    pDto.ExtensionTypeId = result.First().Policy.ExtensionTypeId;
                    pDto.HrsUsedAtPolicySale = result.First().Policy.HrsUsedAtPolicySale;
                    pDto.IsPartialPayment = result.First().Policy.IsPartialPayment;
                    pDto.IsPreWarrantyCheck = result.First().Policy.IsPreWarrantyCheck;
                    pDto.IsSpecialDeal = result.First().Policy.IsSpecialDeal;
                    pDto.ProductId = result.First().Policy.ProductId;
                    pDto.PaymentModeId = result.First().Policy.PaymentModeId;
                    pDto.PolicyNo = result.First().Policy.PolicyNo;
                    pDto.PolicySoldDate = result.First().Policy.PolicySoldDate;
                    pDto.Premium = result.First().Policy.Premium;
                    pDto.RefNo = result.First().Policy.RefNo;
                    pDto.SalesPersonId = result.First().Policy.SalesPersonId;
                    pDto.EntryDateTime = result.First().Policy.EntryDateTime;
                    pDto.EntryUser = result.First().Policy.EntryUser;
                    pDto.IsApproved = result.First().Policy.IsApproved;
                    pDto.IsPolicyCanceled = result.First().Policy.IsPolicyCanceled;
                    pDto.Discount = result.First().Policy.Discount;
                    pDto.TransactionTypeId = result.First().Policy.TransactionTypeId;
                    pDto.DealerPolicy = result.First().Policy.DealerPolicy;

                    pDto.IsPolicyBundleExists = true;
                    return pDto;
                }
                else
                {
                    pDto.IsPolicyBundleExists = false;
                    return null;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ((ex.InnerException != null) ? ex.InnerException.ToString() : ""));
                return null;
            }

        }
        internal bool AddPolicyBundleTransaction(PolicyBundleTransactionRequestDto Policy)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();

                PolicyBundleTransaction pr = new Entities.PolicyBundleTransaction();
                pr.Id = new Guid();
                pr.Comment = Policy.Comment;
                pr.CommodityTypeId = Policy.CommodityTypeId;
                pr.DealerPaymentCurrencyTypeId = Policy.DealerPaymentCurrencyTypeId;
                pr.CustomerPaymentCurrencyTypeId = Policy.CustomerPaymentCurrencyTypeId;
                pr.CustomerId = Policy.CustomerId;
                pr.CustomerPayment = Policy.CustomerPayment;
                pr.DealerId = Policy.DealerId;
                pr.DealerLocationId = Policy.DealerLocationId;
                pr.DealerPayment = Policy.DealerPayment;
                pr.ExtensionTypeId = Policy.ExtensionTypeId;
                pr.HrsUsedAtPolicySale = Policy.HrsUsedAtPolicySale;
                pr.IsPartialPayment = Policy.IsPartialPayment;
                pr.IsPreWarrantyCheck = Policy.IsPreWarrantyCheck;
                pr.IsSpecialDeal = Policy.IsSpecialDeal;
                pr.ProductId = Policy.ProductId;
                pr.PaymentModeId = Policy.PaymentModeId;
                pr.PolicyNo = Policy.PolicyNo;
                pr.PolicySoldDate = Policy.PolicySoldDate;
                pr.Premium = Policy.Premium;
                pr.IsApproved = Policy.IsApproved;
                pr.IsPolicyCanceled = Policy.IsPolicyCanceled;
                pr.RefNo = Policy.RefNo;
                pr.SalesPersonId = Policy.SalesPersonId;
                pr.Discount = Policy.Discount;
                pr.TransactionTypeId = Policy.TransactionTypeId;
                pr.DealerPolicy = Policy.DealerPolicy;
                pr.EntryDateTime = DateTime.Today.ToUniversalTime();
                pr.EntryUser = Guid.Parse("ba56ec84-1fe0-4385-abe4-182f62caa050");

                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.SaveOrUpdate(pr);
                    Policy.Id = pr.Id;
                    transaction.Commit();
                }
                return true;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ((ex.InnerException != null) ? ex.InnerException.ToString() : ""));
                return false;
            }
        }
        #endregion

        #region Transaction
        internal static string PolicyEndorsement(SavePolicyRequestDto SavePolicyRequestDto)
        {
            string Response = String.Empty;
            //validate
            if (SavePolicyRequestDto == null || SavePolicyRequestDto.policyDetails == null
                || SavePolicyRequestDto.policyDetails.requestedUser == Guid.Empty
                || SavePolicyRequestDto.policyDetails.policy == null || SavePolicyRequestDto.policyDetails.product == null
                || SavePolicyRequestDto.policyDetails.customer == null || SavePolicyRequestDto.policyDetails.payment == null)
            {
                Response = "Request data validation failed";

            }
            else
            {
                try
                {
                    //avoid datetime errors
                    if (SavePolicyRequestDto.policyDetails.customer.idIssueDate == null ||
                        SavePolicyRequestDto.policyDetails.customer.idIssueDate < SqlDateTime.MinValue.Value)
                    {
                        SavePolicyRequestDto.policyDetails.customer.idIssueDate = SqlDateTime.MinValue.Value;
                    }

                    if (SavePolicyRequestDto.policyDetails.customer.dateOfBirth == null ||
                        SavePolicyRequestDto.policyDetails.customer.dateOfBirth < SqlDateTime.MinValue.Value)
                    {
                        SavePolicyRequestDto.policyDetails.customer.dateOfBirth = SqlDateTime.MinValue.Value;
                    }

                    CommonEntityManager cem = new CommonEntityManager();
                    ISession session = EntitySessionManager.GetSession();
                    //generating new policy bundle transactionId
                    Guid policyBundleTransactionId = Guid.NewGuid();
                    //saving new policy Attachments
                    if (SavePolicyRequestDto.policyDetails.policyDocIds != null
                        && SavePolicyRequestDto.policyDetails.policyDocIds.Count > 0)
                    {
                        foreach (Guid docId in SavePolicyRequestDto.policyDetails.policyDocIds)
                        {
                            PolicyAttachmentTransaction attachmentTransaction = new PolicyAttachmentTransaction()
                            {
                                Id = Guid.NewGuid(),
                                PolicyBundleIdTransaction = policyBundleTransactionId,
                                UserAttachmentId = docId
                            };
                            using (ITransaction transaction = session.BeginTransaction())
                            {
                                session.Save(attachmentTransaction, attachmentTransaction.Id);
                                transaction.Commit();
                            }

                        }
                    }
                    CurrencyEntityManager currencyEntityManager = new CurrencyEntityManager();
                    Guid getCurrencyPeriodId = currencyEntityManager.GetCurrentCurrencyPeriodId();
                    decimal rate = currencyEntityManager.GetConversionRate(SavePolicyRequestDto.policyDetails.product.dealerPaymentCurrencyTypeId,
                        getCurrencyPeriodId, true);

                    //saving policy bundle transaction
                    PolicyBundleTransaction bundleTransaction = new PolicyBundleTransaction()
                    {
                        Comment = SavePolicyRequestDto.policyDetails.payment.comment,
                        CommodityTypeId = SavePolicyRequestDto.policyDetails.product.commodityTypeId,
                        ContractId = Guid.Empty,
                        CoverTypeId = Guid.Empty,
                        CustomerId = SavePolicyRequestDto.policyDetails.customer.customerId,
                        CustomerPayment = SavePolicyRequestDto.policyDetails.payment.customerPayment / rate,
                        CustomerPaymentCurrencyTypeId = SavePolicyRequestDto.policyDetails.product.customerPaymentCurrencyTypeId,
                        DealerId = SavePolicyRequestDto.policyDetails.product.dealerId,
                        DealerLocationId = SavePolicyRequestDto.policyDetails.product.dealerLocationId,
                        DealerPayment = SavePolicyRequestDto.policyDetails.payment.dealerPayment / rate,
                        DealerPaymentCurrencyTypeId = SavePolicyRequestDto.policyDetails.product.dealerPaymentCurrencyTypeId,
                        DealerPolicy = SavePolicyRequestDto.policyDetails.policy.dealerPolicy,
                        Discount = SavePolicyRequestDto.policyDetails.payment.discount,
                        EntryDateTime = DateTime.UtcNow,
                        EntryUser = SavePolicyRequestDto.policyDetails.requestedUser,
                        ExtensionTypeId = Guid.Empty,
                        HrsUsedAtPolicySale = SavePolicyRequestDto.policyDetails.policy.hrsUsedAtPolicySale,
                        Id = policyBundleTransactionId,
                        IsApproved = false,

                        IsPartialPayment = SavePolicyRequestDto.policyDetails.payment.isPartialPayment,
                        IsPolicyCanceled = false,
                        IsPreWarrantyCheck = false,
                        IsSpecialDeal = SavePolicyRequestDto.policyDetails.payment.isSpecialDeal,
                        PaymentModeId = SavePolicyRequestDto.policyDetails.payment.paymentModeId,
                        PolicyBundleId = SavePolicyRequestDto.policyDetails.policy.id,
                        PolicyNo = "",
                        PolicySoldDate = SavePolicyRequestDto.policyDetails.policy.policySoldDate,
                        Premium = SavePolicyRequestDto.policyDetails.policy.premium / rate,
                        PremiumCurrencyTypeId = Guid.Empty,
                        ProductId = SavePolicyRequestDto.policyDetails.product.productId,
                        RefNo = SavePolicyRequestDto.policyDetails.payment.refNo,
                        SalesPersonId = SavePolicyRequestDto.policyDetails.policy.salesPersonId,
                        TransactionTypeId = GetTransactionTypeId("Endorsement"),
                        BookletNumber = SavePolicyRequestDto.policyDetails.policy.productContracts.First().BookletNumber,
                        ContractExtensionPremiumId = SavePolicyRequestDto.policyDetails.policy.productContracts.First().CoverTypeId,
                        ContractInsuaranceLimitationId = SavePolicyRequestDto.policyDetails.policy.productContracts.First().AttributeSpecificationId,
                        ContractExtensionsId = SavePolicyRequestDto.policyDetails.policy.productContracts.First().ExtensionTypeId,
                        MWStartDate = SavePolicyRequestDto.policyDetails.product.MWStartDate,
                        MWIsAvailable = SavePolicyRequestDto.policyDetails.product.MWIsAvailable
                    };
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.Save(bundleTransaction, bundleTransaction.Id);
                        transaction.Commit();
                    }

                    //saving policy transactions

                    CurrencyEntityManager currencyEM = new CurrencyEntityManager();

                    decimal CustomerPayment = decimal.Zero, DealerPayment = decimal.Zero;
                    foreach (ProductContract_ productContract in SavePolicyRequestDto.policyDetails.policy.productContracts)
                    {
                        Guid relevantCurrencyPeriodId = new ContractEntityManager().GetCurrencyPeriodByContractId(productContract.ContractId);
                        PolicyTransaction policyTransaction = new PolicyTransaction()
                        {
                            AddnSerialNo = SavePolicyRequestDto.policyDetails.product.additionalSerial,
                            Address1 = SavePolicyRequestDto.policyDetails.customer.address1,
                            Address2 = SavePolicyRequestDto.policyDetails.customer.address2,
                            Address3 = SavePolicyRequestDto.policyDetails.customer.address3,
                            Address4 = SavePolicyRequestDto.policyDetails.customer.address4,
                            AspirationId = SavePolicyRequestDto.policyDetails.product.aspirationTypeId,
                            BAndWId = SavePolicyRequestDto.policyDetails.product.id,
                            BodyTypeId = SavePolicyRequestDto.policyDetails.product.bodyTypeId,
                            BusinessAddress1 = SavePolicyRequestDto.policyDetails.customer.businessAddress1,
                            BusinessAddress2 = SavePolicyRequestDto.policyDetails.customer.businessAddress2,
                            BusinessAddress3 = SavePolicyRequestDto.policyDetails.customer.businessAddress3,
                            BusinessAddress4 = SavePolicyRequestDto.policyDetails.customer.businessAddress4,
                            BusinessName = SavePolicyRequestDto.policyDetails.customer.businessName,
                            BusinessTelNo = SavePolicyRequestDto.policyDetails.customer.businessTelNo,
                            CancelationComment = "",
                            CategoryId = SavePolicyRequestDto.policyDetails.product.categoryId,
                            CityId = SavePolicyRequestDto.policyDetails.customer.cityId,
                            Comment = SavePolicyRequestDto.policyDetails.payment.comment,
                            CommodityTypeId = SavePolicyRequestDto.policyDetails.product.commodityTypeId,
                            CommodityUsageTypeId = SavePolicyRequestDto.policyDetails.product.commodityUsageTypeId,
                            ContractId = productContract.ContractId,
                            CountryId = SavePolicyRequestDto.policyDetails.customer.countryId,
                            CoverTypeId = productContract.CoverTypeId,
                            CustomerId = SavePolicyRequestDto.policyDetails.customer.customerId,
                            CustomerPayment = SavePolicyRequestDto.policyDetails.payment.customerPayment / rate,
                            CustomerPaymentCurrencyTypeId = SavePolicyRequestDto.policyDetails.product.customerPaymentCurrencyTypeId,
                            CustomerTypeId = SavePolicyRequestDto.policyDetails.customer.customerTypeId,
                            CylinderCountId = SavePolicyRequestDto.policyDetails.product.cylinderCountId,
                            RegistrationDate = SavePolicyRequestDto.policyDetails.product.registrationDate,
                            DateOfBirth = SavePolicyRequestDto.policyDetails.customer.dateOfBirth,
                            DealerId = SavePolicyRequestDto.policyDetails.product.dealerId,
                            DealerLocationId = SavePolicyRequestDto.policyDetails.product.dealerLocationId,
                            DealerPayment = SavePolicyRequestDto.policyDetails.payment.dealerPayment / rate,
                            DealerPaymentCurrencyTypeId = SavePolicyRequestDto.policyDetails.product.dealerPaymentCurrencyTypeId,
                            DealerPolicy = SavePolicyRequestDto.policyDetails.policy.dealerPolicy,
                            DealerPrice = SavePolicyRequestDto.policyDetails.product.dealerPrice / rate,

                            Discount = SavePolicyRequestDto.policyDetails.payment.discount,

                            DLIssueDate = SavePolicyRequestDto.policyDetails.customer.idIssueDate,
                            DriveTypeId = Guid.Empty,
                            Email = SavePolicyRequestDto.policyDetails.customer.email,
                            EngineCapacityId = SavePolicyRequestDto.policyDetails.product.engineCapacityId,
                            ExtensionTypeId = productContract.ExtensionTypeId,
                            FirstName = SavePolicyRequestDto.policyDetails.customer.firstName,
                            FuelTypeId = SavePolicyRequestDto.policyDetails.product.fuelTypeId,
                            Gender = SavePolicyRequestDto.policyDetails.customer.gender,
                            HrsUsedAtPolicySale = SavePolicyRequestDto.policyDetails.policy.hrsUsedAtPolicySale,
                            Id = Guid.NewGuid(),
                            PolicyBundleTransactionId = policyBundleTransactionId,
                            IDNo = SavePolicyRequestDto.policyDetails.customer.idNo,
                            IDTypeId = SavePolicyRequestDto.policyDetails.customer.idTypeId,
                            InvoiceNo = SavePolicyRequestDto.policyDetails.product.invoiceNo,
                            IsActive = true,
                            IsApproved = false,
                            IsRejected = false,
                            ApprovedRejectedBy = Guid.Empty,
                            IsPartialPayment = SavePolicyRequestDto.policyDetails.payment.isPartialPayment,
                            IsPreWarrantyCheck = false,
                            IsRecordActive = true,
                            IsSpecialDeal = SavePolicyRequestDto.policyDetails.payment.isSpecialDeal,
                            //ItemPrice = currencyEM.ConvertToBaseCurrency(SavePolicyRequestDto.policyDetails.product.itemPrice, productContract.PremiumCurrencyTypeId, relevantCurrencyPeriodId),
                            ItemPrice = SavePolicyRequestDto.policyDetails.product.itemPrice / rate,

                            ItemPurchasedDate = SavePolicyRequestDto.policyDetails.product.itemPurchasedDate,
                            ItemStatusId = SavePolicyRequestDto.policyDetails.product.itemStatusId,
                            LastName = SavePolicyRequestDto.policyDetails.customer.lastName,
                            MakeId = SavePolicyRequestDto.policyDetails.product.makeId,
                            MobileNo = SavePolicyRequestDto.policyDetails.customer.mobileNo,
                            ModelCode = "",
                            ModelId = SavePolicyRequestDto.policyDetails.product.modelId,
                            ModelYear = SavePolicyRequestDto.policyDetails.product.modelYear,
                            Password = "",
                            PaymentModeId = SavePolicyRequestDto.policyDetails.payment.paymentModeId,
                            PlateNo = SavePolicyRequestDto.policyDetails.product.additionalSerial,
                            PolicyId = productContract.Id,
                            PolicyNo = productContract.PolicyNo,
                            PolicySoldDate = SavePolicyRequestDto.policyDetails.policy.policySoldDate,

                            PolicyEndDate = DBDTOTransformer.Instance.GetPolicyEndDate(
                                SavePolicyRequestDto.policyDetails.product.MWStartDate,
                                SavePolicyRequestDto.policyDetails.policy.policySoldDate,
                                SavePolicyRequestDto.policyDetails.product.makeId,
                                SavePolicyRequestDto.policyDetails.product.modelId,
                                SavePolicyRequestDto.policyDetails.product.dealerId,
                                productContract.ExtensionTypeId,
                                SavePolicyRequestDto.policyDetails.product.MWIsAvailable),

                            PolicyStartDate = DBDTOTransformer.Instance.GetPolicyStartDate(
                                SavePolicyRequestDto.policyDetails.product.MWStartDate,
                                SavePolicyRequestDto.policyDetails.policy.policySoldDate,
                                SavePolicyRequestDto.policyDetails.product.makeId,
                                SavePolicyRequestDto.policyDetails.product.modelId,
                                SavePolicyRequestDto.policyDetails.product.dealerId,
                                productContract.ExtensionTypeId,
                                SavePolicyRequestDto.policyDetails.product.MWIsAvailable),

                            ModifiedDate = DateTime.UtcNow,
                            NationalityId = SavePolicyRequestDto.policyDetails.customer.nationalityId,
                            OtherTelNo = SavePolicyRequestDto.policyDetails.customer.otherTelNo,
                            Premium = productContract.Premium / rate,

                            PremiumCurrencyTypeId = productContract.PremiumCurrencyTypeId,
                            ProductId = SavePolicyRequestDto.policyDetails.product.productId,
                            RefNo = SavePolicyRequestDto.policyDetails.payment.refNo,
                            SalesPersonId = SavePolicyRequestDto.policyDetails.policy.salesPersonId,
                            SerialNo = SavePolicyRequestDto.policyDetails.product.serialNumber,
                            TransactionTypeId = GetTransactionTypeId("Endorsement"),
                            TransferFee = 0,
                            TransmissionId = SavePolicyRequestDto.policyDetails.product.transmissionTypeId,
                            UsageTypeId = SavePolicyRequestDto.policyDetails.customer.usageTypeId,
                            Variant = SavePolicyRequestDto.policyDetails.product.variantId,
                            VehicleId = SavePolicyRequestDto.policyDetails.product.productId,
                            //VehiclePrice = currencyEM.ConvertToBaseCurrency(SavePolicyRequestDto.policyDetails.product.itemPrice, productContract.PremiumCurrencyTypeId, relevantCurrencyPeriodId),
                            VehiclePrice = SavePolicyRequestDto.policyDetails.product.itemPrice / rate,

                            VINNo = SavePolicyRequestDto.policyDetails.product.serialNumber,
                            PolicyBundleId = SavePolicyRequestDto.policyDetails.policy.id,

                            BookletNumber = SavePolicyRequestDto.policyDetails.policy.productContracts.First().BookletNumber,
                            ContractExtensionPremiumId = SavePolicyRequestDto.policyDetails.policy.productContracts.First().CoverTypeId,
                            ContractInsuaranceLimitationId = SavePolicyRequestDto.policyDetails.policy.productContracts.First().AttributeSpecificationId,
                            ContractExtensionsId = SavePolicyRequestDto.policyDetails.policy.productContracts.First().ExtensionTypeId,
                            MWStartDate = SavePolicyRequestDto.policyDetails.product.MWStartDate,
                            MWIsAvailable = SavePolicyRequestDto.policyDetails.product.MWIsAvailable,
                            EngineNumber = SavePolicyRequestDto.policyDetails.product.engineNumber
                        };
                        CustomerPayment += policyTransaction.CustomerPayment;
                        DealerPayment += policyTransaction.DealerPayment;

                        using (ITransaction transaction = session.BeginTransaction())
                        {
                            session.Save(policyTransaction, policyTransaction.Id);
                            transaction.Commit();
                        }
                    }
                    //update policy bundle transaction
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        bundleTransaction.DealerPayment = DealerPayment;
                        bundleTransaction.CustomerPayment = CustomerPayment;

                        session.Evict(bundleTransaction);
                        session.Update(bundleTransaction, bundleTransaction.Id);
                        transaction.Commit();
                    }
                    Response = "success";

                }
                catch (Exception ex)
                {
                    Response = "Error occurred whilie saving the Endorsment";
                    logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ((ex.InnerException != null) ? ex.InnerException.ToString() : ""));
                }

            }
            return Response;
        }
        private static Guid GetTransactionTypeId(string type)
        {
            Guid Response = Guid.Empty;
            try
            {
                ISession session = EntitySessionManager.GetSession();
                PolicyTransactionType transactionType = session.Query<PolicyTransactionType>()
                    .Where(a => a.Code.ToLower() == type.ToLower()).FirstOrDefault();
                if (transactionType != null)
                {
                    Response = transactionType.Id;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ((ex.InnerException != null) ? ex.InnerException.ToString() : ""));
            }
            return Response;
        }
        public List<PolicyTransaction> GetPolicyEndorsements()
        {
            List<PolicyTransaction> entities = null;
            ISession session = EntitySessionManager.GetSession();
            IQueryable<PolicyTransaction> MakeData = session.Query<PolicyTransaction>();
            entities = MakeData.ToList();
            return entities;
        }
        public PolicyTransactionResponseDto GetPolicyEndorsementById(Guid PolicyEndorsementId)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();

                PolicyTransactionResponseDto pr = new PolicyTransactionResponseDto();

                var query =
                    from PolicyTransaction in session.Query<PolicyTransaction>()
                    where PolicyTransaction.Id == PolicyEndorsementId
                    select new { PolicyTransaction = PolicyTransaction };

                var result = query.ToList();

                var queryP =
                     from PolicyContractProduct in session.Query<PolicyContractProduct>()
                     where PolicyContractProduct.ReferenceId == PolicyEndorsementId && PolicyContractProduct.Type == "Transaction"
                     select new { PolicyContractProduct = PolicyContractProduct };

                var resultP = queryP.ToList();

                List<PolicyContractProductResponseDto> prod = new List<PolicyContractProductResponseDto>();
                foreach (var item in resultP)
                {
                    PolicyContractProductResponseDto p = new PolicyContractProductResponseDto()
                    {
                        ContractId = item.PolicyContractProduct.ContractId,
                        ExtensionTypeId = item.PolicyContractProduct.ExtensionTypeId,
                        CoverTypeId = item.PolicyContractProduct.CoverTypeId,
                        Id = item.PolicyContractProduct.Id,
                        ParentProductId = item.PolicyContractProduct.ParentProductId,
                        Premium = item.PolicyContractProduct.Premium,
                        PremiumCurrencyTypeId = item.PolicyContractProduct.PremiumCurrencyTypeId,
                        ProductId = item.PolicyContractProduct.ProductId,
                        ReferenceId = item.PolicyContractProduct.ReferenceId,
                        Type = item.PolicyContractProduct.Type
                    };
                    prod.Add(p);
                }
                if (result != null && result.Count > 0)
                {
                    pr.Id = result.First().PolicyTransaction.Id;
                    pr.PolicyId = result.First().PolicyTransaction.PolicyId;
                    pr.AddnSerialNo = result.First().PolicyTransaction.AddnSerialNo;
                    pr.CommodityTypeId = result.First().PolicyTransaction.CommodityUsageTypeId;
                    pr.Address1 = result.First().PolicyTransaction.Address1;
                    pr.Address2 = result.First().PolicyTransaction.Address2;
                    pr.Address3 = result.First().PolicyTransaction.Address3;
                    pr.Address4 = result.First().PolicyTransaction.Address4;
                    pr.AspirationId = result.First().PolicyTransaction.AspirationId;
                    pr.BodyTypeId = result.First().PolicyTransaction.BodyTypeId;
                    pr.BusinessAddress1 = result.First().PolicyTransaction.BusinessAddress1;
                    pr.BusinessAddress2 = result.First().PolicyTransaction.BusinessAddress2;
                    pr.BusinessAddress3 = result.First().PolicyTransaction.BusinessAddress3;
                    pr.BusinessAddress4 = result.First().PolicyTransaction.BusinessAddress4;
                    pr.BusinessName = result.First().PolicyTransaction.BusinessName;
                    pr.BusinessTelNo = result.First().PolicyTransaction.BusinessTelNo;
                    pr.CategoryId = result.First().PolicyTransaction.CategoryId;
                    pr.CityId = result.First().PolicyTransaction.CityId;
                    pr.Comment = result.First().PolicyTransaction.Comment;
                    pr.CommodityTypeId = result.First().PolicyTransaction.CommodityTypeId;
                    pr.ContractId = result.First().PolicyTransaction.ContractId;
                    pr.CountryId = result.First().PolicyTransaction.CountryId;
                    pr.CoverTypeId = result.First().PolicyTransaction.CoverTypeId;
                    pr.CustomerPayment = result.First().PolicyTransaction.CustomerPayment;
                    pr.CustomerPaymentCurrencyTypeId = result.First().PolicyTransaction.CustomerPaymentCurrencyTypeId;
                    pr.CustomerTypeId = result.First().PolicyTransaction.CustomerTypeId;
                    pr.CylinderCountId = result.First().PolicyTransaction.CylinderCountId;
                    pr.RegistrationDate = result.First().PolicyTransaction.RegistrationDate;
                    pr.DateOfBirth = result.First().PolicyTransaction.DateOfBirth;
                    pr.DealerId = result.First().PolicyTransaction.DealerId;
                    pr.DealerLocationId = result.First().PolicyTransaction.DealerLocationId;
                    pr.DealerPayment = result.First().PolicyTransaction.DealerPayment;
                    pr.DealerPaymentCurrencyTypeId = result.First().PolicyTransaction.DealerPaymentCurrencyTypeId;
                    pr.DealerPrice = result.First().PolicyTransaction.DealerPrice;
                    pr.DLIssueDate = result.First().PolicyTransaction.DLIssueDate;
                    pr.DriveTypeId = result.First().PolicyTransaction.DriveTypeId;
                    pr.Email = result.First().PolicyTransaction.Email;
                    pr.EngineCapacityId = result.First().PolicyTransaction.EngineCapacityId;
                    pr.ExtensionTypeId = result.First().PolicyTransaction.ExtensionTypeId;
                    pr.FirstName = result.First().PolicyTransaction.FirstName;
                    pr.FuelTypeId = result.First().PolicyTransaction.FuelTypeId;
                    pr.Gender = result.First().PolicyTransaction.Gender;
                    pr.HrsUsedAtPolicySale = result.First().PolicyTransaction.HrsUsedAtPolicySale;
                    pr.IDNo = result.First().PolicyTransaction.IDNo;
                    pr.IDTypeId = result.First().PolicyTransaction.IDTypeId;
                    pr.InvoiceNo = result.First().PolicyTransaction.InvoiceNo;
                    pr.IsActive = result.First().PolicyTransaction.IsActive;
                    pr.IsApproved = result.First().PolicyTransaction.IsApproved;
                    pr.IsPartialPayment = result.First().PolicyTransaction.IsPartialPayment;
                    pr.IsPreWarrantyCheck = result.First().PolicyTransaction.IsPreWarrantyCheck;
                    pr.IsSpecialDeal = result.First().PolicyTransaction.IsSpecialDeal;
                    pr.ItemPrice = result.First().PolicyTransaction.ItemPrice;
                    pr.ItemPurchasedDate = result.First().PolicyTransaction.ItemPurchasedDate;
                    pr.ItemStatusId = result.First().PolicyTransaction.ItemStatusId;
                    pr.LastName = result.First().PolicyTransaction.LastName;
                    pr.MakeId = result.First().PolicyTransaction.MakeId;
                    pr.MobileNo = result.First().PolicyTransaction.MobileNo;
                    pr.ModelCode = result.First().PolicyTransaction.ModelCode;
                    pr.ModelId = result.First().PolicyTransaction.ModelId;
                    pr.ModelYear = result.First().PolicyTransaction.ModelYear;
                    pr.NationalityId = result.First().PolicyTransaction.NationalityId;
                    pr.OtherTelNo = result.First().PolicyTransaction.OtherTelNo;
                    pr.Password = result.First().PolicyTransaction.Password;
                    pr.PaymentModeId = result.First().PolicyTransaction.PaymentModeId;
                    pr.PlateNo = result.First().PolicyTransaction.PlateNo;
                    pr.PolicyNo = result.First().PolicyTransaction.PolicyNo;
                    pr.PolicySoldDate = result.First().PolicyTransaction.PolicySoldDate;
                    pr.Premium = result.First().PolicyTransaction.Premium;
                    pr.PremiumCurrencyTypeId = result.First().PolicyTransaction.PremiumCurrencyTypeId;
                    pr.ProductId = result.First().PolicyTransaction.ProductId;
                    pr.ProfilePicture = result.First().PolicyTransaction.ProfilePicture;
                    pr.RefNo = result.First().PolicyTransaction.RefNo;
                    pr.SalesPersonId = result.First().PolicyTransaction.SalesPersonId;
                    pr.SerialNo = result.First().PolicyTransaction.SerialNo;
                    pr.TransmissionId = result.First().PolicyTransaction.TransmissionId;
                    pr.UsageTypeId = result.First().PolicyTransaction.UsageTypeId;
                    pr.UserName = result.First().PolicyTransaction.UserName;
                    pr.Variant = result.First().PolicyTransaction.Variant;
                    pr.VehiclePrice = result.First().PolicyTransaction.VehiclePrice;
                    pr.VINNo = result.First().PolicyTransaction.VINNo;
                    pr.TransactionTypeId = result.First().PolicyTransaction.TransactionTypeId;
                    pr.IsRecordActive = result.First().PolicyTransaction.IsRecordActive;
                    pr.CancelationComment = result.First().PolicyTransaction.CancelationComment;
                    pr.PolicyStartDate = result.First().PolicyTransaction.PolicyStartDate;
                    pr.PolicyEndDate = result.First().PolicyTransaction.PolicyEndDate;
                    pr.ModifiedDate = result.First().PolicyTransaction.ModifiedDate;
                    pr.ModifiedUser = result.First().PolicyTransaction.ModifiedUser;
                    pr.Discount = result.First().PolicyTransaction.Discount;
                    pr.PolicyBundleId = result.First().PolicyTransaction.PolicyBundleId;
                    pr.TransferFee = result.First().PolicyTransaction.TransferFee;
                    pr.DealerPolicy = result.First().PolicyTransaction.DealerPolicy;


                    pr.IsPolicyExists = true;
                    return pr;
                }
                else
                {
                    pr.IsPolicyExists = false;
                    return null;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ((ex.InnerException != null) ? ex.InnerException.ToString() : ""));
                return null;
            }

        }
        internal bool AddPolicyEndorsement(PolicyTransactionRequestDto PolicyEndorsement)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();
                PolicyTransaction pr = new Entities.PolicyTransaction();
                List<PolicyContractProduct> p = new List<Entities.PolicyContractProduct>();

                pr.Id = new Guid();
                pr.PolicyId = PolicyEndorsement.PolicyId;
                pr.CommodityUsageTypeId = PolicyEndorsement.CommodityUsageTypeId;
                pr.AddnSerialNo = PolicyEndorsement.AddnSerialNo;
                pr.Address1 = PolicyEndorsement.Address1;
                pr.Address2 = PolicyEndorsement.Address2;
                pr.Address3 = PolicyEndorsement.Address3;
                pr.Address4 = PolicyEndorsement.Address4;
                pr.AspirationId = PolicyEndorsement.AspirationId;
                pr.BodyTypeId = PolicyEndorsement.BodyTypeId;
                pr.BusinessAddress1 = PolicyEndorsement.BusinessAddress1;
                pr.BusinessAddress2 = PolicyEndorsement.BusinessAddress2;
                pr.BusinessAddress3 = PolicyEndorsement.BusinessAddress3;
                pr.BusinessAddress4 = PolicyEndorsement.BusinessAddress4;
                pr.BusinessName = PolicyEndorsement.BusinessName;
                pr.BusinessTelNo = PolicyEndorsement.BusinessTelNo;
                pr.CategoryId = PolicyEndorsement.CategoryId;
                pr.CityId = PolicyEndorsement.CityId;
                pr.Comment = PolicyEndorsement.Comment;
                pr.CommodityTypeId = PolicyEndorsement.CommodityTypeId;
                pr.ContractId = PolicyEndorsement.ContractId;
                pr.CountryId = PolicyEndorsement.CountryId;
                pr.CoverTypeId = PolicyEndorsement.CoverTypeId;
                pr.CustomerPayment = PolicyEndorsement.CustomerPayment;
                pr.CustomerPaymentCurrencyTypeId = PolicyEndorsement.CustomerPaymentCurrencyTypeId;
                pr.CustomerTypeId = PolicyEndorsement.CustomerTypeId;
                pr.CylinderCountId = PolicyEndorsement.CylinderCountId;
                pr.RegistrationDate = PolicyEndorsement.RegistrationDate;
                pr.DateOfBirth = PolicyEndorsement.DateOfBirth;
                pr.DealerId = PolicyEndorsement.DealerId;
                pr.DealerLocationId = PolicyEndorsement.DealerLocationId;
                pr.DealerPayment = PolicyEndorsement.DealerPayment;
                pr.DealerPaymentCurrencyTypeId = PolicyEndorsement.DealerPaymentCurrencyTypeId;
                pr.DealerPrice = PolicyEndorsement.DealerPrice;
                pr.DLIssueDate = PolicyEndorsement.DLIssueDate;
                pr.DriveTypeId = PolicyEndorsement.DriveTypeId;
                pr.Email = PolicyEndorsement.Email;
                pr.EngineCapacityId = PolicyEndorsement.EngineCapacityId;
                pr.ExtensionTypeId = PolicyEndorsement.ExtensionTypeId;
                pr.FirstName = PolicyEndorsement.FirstName;
                pr.FuelTypeId = PolicyEndorsement.FuelTypeId;
                pr.Gender = PolicyEndorsement.Gender;
                pr.HrsUsedAtPolicySale = PolicyEndorsement.HrsUsedAtPolicySale;
                pr.IDNo = PolicyEndorsement.IDNo;
                pr.IDTypeId = PolicyEndorsement.IDTypeId;
                pr.InvoiceNo = PolicyEndorsement.InvoiceNo;
                pr.IsActive = PolicyEndorsement.IsActive;
                pr.IsApproved = PolicyEndorsement.IsApproved;
                pr.IsPartialPayment = PolicyEndorsement.IsPartialPayment;
                pr.IsPreWarrantyCheck = PolicyEndorsement.IsPreWarrantyCheck;
                pr.IsSpecialDeal = PolicyEndorsement.IsSpecialDeal;
                pr.ItemPrice = PolicyEndorsement.ItemPrice;
                pr.ItemPurchasedDate = PolicyEndorsement.ItemPurchasedDate;
                pr.ItemStatusId = PolicyEndorsement.ItemStatusId;
                pr.LastName = PolicyEndorsement.LastName;
                pr.MakeId = PolicyEndorsement.MakeId;
                pr.MobileNo = PolicyEndorsement.MobileNo;
                pr.ModelCode = PolicyEndorsement.ModelCode;
                pr.ModelId = PolicyEndorsement.ModelId;
                pr.ModelYear = PolicyEndorsement.ModelYear;
                pr.NationalityId = PolicyEndorsement.NationalityId;
                pr.OtherTelNo = PolicyEndorsement.OtherTelNo;
                pr.Password = PolicyEndorsement.Password;
                pr.PaymentModeId = PolicyEndorsement.PaymentModeId;
                pr.PlateNo = PolicyEndorsement.PlateNo;
                pr.PolicyNo = PolicyEndorsement.PolicyNo;
                pr.PolicySoldDate = PolicyEndorsement.PolicySoldDate;
                pr.Premium = PolicyEndorsement.Premium;
                pr.PremiumCurrencyTypeId = PolicyEndorsement.PremiumCurrencyTypeId;
                pr.ProductId = PolicyEndorsement.ProductId;
                pr.ProfilePicture = PolicyEndorsement.ProfilePicture;
                pr.RefNo = PolicyEndorsement.RefNo;
                pr.SalesPersonId = PolicyEndorsement.SalesPersonId;
                pr.SerialNo = PolicyEndorsement.SerialNo;
                pr.TransmissionId = PolicyEndorsement.TransmissionId;
                pr.UsageTypeId = PolicyEndorsement.UsageTypeId;
                pr.UserName = PolicyEndorsement.UserName;
                pr.Variant = PolicyEndorsement.Variant;
                pr.VehiclePrice = PolicyEndorsement.VehiclePrice;
                pr.VINNo = PolicyEndorsement.VINNo;
                pr.VehicleId = PolicyEndorsement.VehicleId;
                pr.CustomerId = PolicyEndorsement.CustomerId;
                pr.BAndWId = PolicyEndorsement.BAndWId;
                pr.TransactionTypeId = PolicyEndorsement.TransactionTypeId;
                pr.CancelationComment = PolicyEndorsement.CancelationComment;
                pr.IsRecordActive = PolicyEndorsement.IsRecordActive;
                pr.PolicyStartDate = PolicyEndorsement.PolicyStartDate;
                pr.PolicyEndDate = PolicyEndorsement.PolicyEndDate;
                pr.Discount = PolicyEndorsement.Discount;
                pr.ModifiedUser = PolicyEndorsement.ModifiedUser;
                pr.PolicyBundleId = PolicyEndorsement.PolicyBundleId;
                pr.ModifiedDate = DateTime.Today.ToUniversalTime();
                pr.TransferFee = PolicyEndorsement.TransferFee;
                pr.DealerPolicy = PolicyEndorsement.DealerPolicy;

                foreach (var item in PolicyEndorsement.ContractProducts)
                {
                    PolicyContractProduct pp = new Entities.PolicyContractProduct()
                    {
                        ContractId = item.ContractId,
                        CoverTypeId = item.CoverTypeId,
                        ExtensionTypeId = item.ExtensionTypeId,
                        ParentProductId = item.ParentProductId,
                        Premium = item.Premium,
                        PremiumCurrencyTypeId = item.PremiumCurrencyTypeId,
                        ProductId = item.ProductId,
                        Type = "Transaction"
                    };
                    p.Add(pp);

                }

                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.SaveOrUpdate(pr);

                    foreach (var item in p)
                    {
                        item.Id = new Guid();
                        item.ReferenceId = pr.Id;
                        session.SaveOrUpdate(item);
                    }
                    transaction.Commit();
                }

                return true;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ((ex.InnerException != null) ? ex.InnerException.ToString() : ""));
                return false;
            }
        }
        internal object GetAllPolicyInquiryDetails(Guid PolicyBundleId)
        {
            PolicyInquiryDetailsResponseDto response = new PolicyInquiryDetailsResponseDto();

            try
            {
                ISession session = EntitySessionManager.GetSession();
                Policy policy = session.Query<Policy>().Where(a => a.PolicyBundleId == PolicyBundleId).FirstOrDefault();
                PolicyTransaction policytransaction_ = session.Query<PolicyTransaction>().Where(a => a.PolicyId == PolicyBundleId).FirstOrDefault();
                Customer CustomerData = session.Query<Customer>().Where(a => a.Id == policy.CustomerId).FirstOrDefault();
                //policy details
                if (policy != null)
                {
                    PolicyResponseDto policyData = GetPolicyById(policy.Id);
                    response.policyDetails = DBDTOTransformer.Instance.PolicyDtoToInquiryPolicyDetails(policyData, CustomerData);
                }

                //endorsement
                PolicyTransaction policytransactionEnd = session.Query<PolicyTransaction>().Where(a => a.PolicyId == policy.Id && a.TransactionTypeId== GetTransactionTypeId("Endorsement")).FirstOrDefault();
                if (policytransactionEnd != null)
                {
                    PolicyResponseDto policyData = GetPolicyById(policy.Id);
                    response.policyEnrolmentDetails = DBDTOTransformer.Instance.PolicyDtoToInquiryEnrolmentDetails(policytransactionEnd, policyData, CustomerData);
                    response.policyDetails.Status = "Endorsed Policy Not Approved";
                }
                //endorsementApproval
                PolicyTransaction policyTransactionEndApp = session.Query<PolicyTransaction>().Where(a => a.PolicyId == policy.Id &&  a.IsRecordActive == false && a.IsApproved == true && a.IsRejected == false && a.TransactionTypeId == GetTransactionTypeId("EndorsementApproval")).FirstOrDefault();
                if (policyTransactionEndApp != null)
                {
                    PolicyResponseDto policyData = GetPolicyById(policy.Id);
                    response.policyEnrolmentDetails = DBDTOTransformer.Instance.PolicyDtoToInquiryEnrolmentDetails(policyTransactionEndApp, policyData, CustomerData);
                    response.policyEnrolmentDetails.Status = "Endorsed Policy Approved";
                    response.policyDetails.Status = "Endorsed Policy Approved";
                }

                //cancellation
                Policy PolicyCancel = session.Query<Policy>().Where(a => a.PolicyBundleId == PolicyBundleId && a.IsPolicyCanceled == true).FirstOrDefault();
                if (PolicyCancel != null)
                {
                    PolicyResponseDto policyData = GetPolicyById(policy.Id);
                    PolicyTransactionResponseDto PTResponse = GetPolicyTransactionById(policy.Id);
                    response.policyCancelationDetails = DBDTOTransformer.Instance.PolicyDtoToInquiryCancelationDetails(policyData, CustomerData, PTResponse);
                    response.policyDetails.Status = "Policy Canceled";
                }
                else {
                    // cancellation Request
                    PolicyTransaction policytransactionCancelRequest = session.Query<PolicyTransaction>().Where(a => a.PolicyId == policy.Id &&  a.IsRecordActive==true && a.IsApproved == false && a.IsRejected == false && a.TransactionTypeId == GetTransactionTypeId("Cancellation")).FirstOrDefault();
                    if (policytransactionCancelRequest != null)
                    {
                        PolicyResponseDto policyData = GetPolicyById(policy.Id);
                        PolicyTransactionResponseDto PTResponse = GetPolicyTransactionById(policy.Id);
                        response.policyCancelationDetails = DBDTOTransformer.Instance.PolicyDtoToInquiryCancelationDetails(policyData, CustomerData, PTResponse);
                        response.policyDetails.Status = "Policy Cancellation Approve Pending";
                    }

                }

                //renewal
                PolicyRenewal PolicyRene = session.Query<PolicyRenewal>().Where(a => a.OriginalPolicyBundleId == PolicyBundleId).FirstOrDefault();

                if (PolicyRene != null)
                {
                    //PolicyBundle  PolicyBundle = session.Query<PolicyBundle>().Where(a => a.Id == PolicyRene.NewPolicyBundleId)
                    Policy PolicyRenewal = session.Query<Policy>().Where(a => a.PolicyBundleId == PolicyRene.NewPolicyBundleId).FirstOrDefault();
                    if (PolicyRenewal != null)
                    {
                        PolicyResponseDto policyData = GetPolicyById(PolicyRenewal.Id);
                        PolicyTransactionResponseDto PTResponse = GetPolicyTransactionById(policy.Id);
                        response.policyrenewal = DBDTOTransformer.Instance.PolicyDtoToInquiryRenewalDetails(policyData, CustomerData, PTResponse);
                        response.policyDetails.Status = "Policy Renewed";
                    }
                }

                //Policy PolicyRenewal = session.Query<Policy>().Where(a => a.PolicyBundleId == PolicyBundleId && a.IsPolicyRenewed == true).FirstOrDefault();
                //if (PolicyRenewal != null)
                //{
                //    PolicyResponseDto policyData = GetPolicyById(policy.Id);
                //    PolicyTransactionResponseDto PTResponse = GetPolicyTransactionById(policy.Id);
                //    response.policyrenewal = DBDTOTransformer.Instance.PolicyDtoToInquiryRenewalDetails(policyData, CustomerData, PTResponse);
                //}

            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ((ex.InnerException != null) ? ex.InnerException.ToString() : ""));
            }
            return response;

        }
        internal object GetAllPolicyEndorsementDetailsByPolicyBundleId(Guid PolicyBundleId)
        {
            PolicyEndorsementDetailsForApprovalResponseDto response = new PolicyEndorsementDetailsForApprovalResponseDto();

            try
            {
                ISession session = EntitySessionManager.GetSession();
                //reading old policy details
                IEnumerable<PolicyBundle> policyBundleObj = session.Query<PolicyBundle>()
                    .Where(a => a.Id == PolicyBundleId);
                if (policyBundleObj == null || policyBundleObj.FirstOrDefault() == null)
                {
                    return response;
                }

                PolicyBundle policyBundle = policyBundleObj.FirstOrDefault();
                IEnumerable<Policy> policies = session.Query<Policy>()
                    .Where(a => a.PolicyBundleId == PolicyBundleId);
                if (policies == null || policies.Count() == 0)
                {
                    return response;
                }

                policyBundle.PremiumCurrencyTypeId = policies.FirstOrDefault().CustomerPaymentCurrencyTypeId;
                Customer customer = session.Query<Customer>().Where(a => a.Id == policyBundle.CustomerId).FirstOrDefault();

                response.policyDetails = new PolicyEndorsementDetails()
                {
                    policyDetails = new PolicyDetails_E()
                    {

                        customer = DBDTOTransformer.Instance.CustomerEntityToEndorsementCustomer(customer),
                        payment = DBDTOTransformer.Instance.PolicyBundleEntityToEndorsementPayment(policyBundle),
                        product = DBDTOTransformer.Instance.PolicyBundleEntityToEndorsementProduct(policyBundle),
                        policy = DBDTOTransformer.Instance.PolicyListToEndorsementPolicy(policies.ToList())
                    }
                };

                response.endorsementDetails = new PolicyEndorsementDetails()
                {
                    policyDetails = DBDTOTransformer.Instance.FullEndorsmentDetailsByPolicyBundleId(PolicyBundleId)
                };
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ((ex.InnerException != null) ? ex.InnerException.ToString() : ""));
            }

            return response;

        }

        internal string ApproveEndorsement(Guid PolicyBundleId, Guid UserId)
        {
            String Response = "Invalid policy bundle";
            try
            {
                ISession session = EntitySessionManager.GetSession();

                //reading old policy details
                IEnumerable<PolicyBundle> policyBundleObj = session.Query<PolicyBundle>()
                    .Where(a => a.Id == PolicyBundleId);
                if (policyBundleObj == null || policyBundleObj.FirstOrDefault() == null)
                {
                    return Response;
                }

                PolicyBundle policyBundle = policyBundleObj.FirstOrDefault();
                IEnumerable<Policy> policies = session.Query<Policy>()
                    .Where(a => a.PolicyBundleId == PolicyBundleId);
                if (policies == null || policies.Count() == 0)
                {
                    return Response;
                }

                PolicyBundleTransaction bundleTransaction = session.Query<PolicyBundleTransaction>()
                    .Where(a => a.PolicyBundleId == PolicyBundleId && a.IsApproved != true && a.IsPolicyCanceled != true && a.TransactionTypeId == GetTransactionTypeId("Endorsement")).FirstOrDefault();
                if (bundleTransaction == null)
                {
                    return Response;
                }

                List<PolicyTransaction> policyTransactionList = session.Query<PolicyTransaction>()
                    .Where(a => a.PolicyBundleTransactionId == bundleTransaction.Id).ToList();
                if (policyTransactionList == null || policyTransactionList.Count() == 0)
                {
                    return Response;
                }
                #region "Item Information"
                //if everything is there code hits here
                //we are backing up existing data to history tables
                CommodityType commodityType = session.Query<CommodityType>()
                    .Where(a => a.CommodityTypeId == policyBundle.CommodityTypeId).FirstOrDefault();

                if (commodityType.CommodityCode.ToLower().StartsWith("a"))
                {
                    //this is an automobile
                    VehiclePolicy vehiclePolicy = session.Query<VehiclePolicy>()
                        .Where(a => a.PolicyId == policies.FirstOrDefault().Id).FirstOrDefault();

                    VehicleDetails Vehicle = session.Query<VehicleDetails>()
                        .Where(a => a.Id == vehiclePolicy.VehicleId).FirstOrDefault();
                    if (Vehicle.RegistrationDate == DateTime.MinValue)
                    {
                        Vehicle.RegistrationDate = SqlDateTime.MinValue.Value;
                    }
                    //save history
                    VehicleDetailsHistory vehicleHistory = DBDTOTransformer.Instance.DBVehicleDetailsToVehicleDetailsHistory(Vehicle, bundleTransaction);
                    // EntitySessionManager.CloseSession();

                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.Save(vehicleHistory, vehicleHistory.Id);
                        transaction.Commit();

                    }
                    //update live data
                    VehicleDetails VehicleDetails = DBDTOTransformer.Instance.PolicyTransactionToVehicleEntity(Vehicle, bundleTransaction, policyTransactionList.FirstOrDefault());
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.Evict(Vehicle);
                        session.Update(VehicleDetails, VehicleDetails.Id);
                        transaction.Commit();

                    }
                }
                else if (commodityType.CommodityCode.ToLower().StartsWith("b"))
                {
                    //this is an automobile
                    VehiclePolicy vehiclePolicy = session.Query<VehiclePolicy>()
                        .Where(a => a.PolicyId == policies.FirstOrDefault().Id).FirstOrDefault();

                    VehicleDetails Vehicle = session.Query<VehicleDetails>()
                        .Where(a => a.Id == vehiclePolicy.VehicleId).FirstOrDefault();
                    if (Vehicle.RegistrationDate == DateTime.MinValue)
                    {
                        Vehicle.RegistrationDate = SqlDateTime.MinValue.Value;
                    }
                    //save history
                    VehicleDetailsHistory vehicleHistory = DBDTOTransformer.Instance.DBVehicleDetailsToVehicleDetailsHistory(Vehicle, bundleTransaction);
                    // EntitySessionManager.CloseSession();

                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.Save(vehicleHistory, vehicleHistory.Id);
                        transaction.Commit();

                    }
                    //update live data
                    VehicleDetails VehicleDetails = DBDTOTransformer.Instance.PolicyTransactionToVehicleEntity(Vehicle, bundleTransaction, policyTransactionList.FirstOrDefault());
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.Evict(Vehicle);
                        session.Update(VehicleDetails, VehicleDetails.Id);
                        transaction.Commit();

                    }
                }
                else if (commodityType.CommodityCode.ToLower().StartsWith("e"))
                {
                    BAndWPolicy electronicPolicy = session.Query<BAndWPolicy>()
                        .Where(a => a.PolicyId == policies.FirstOrDefault().Id).FirstOrDefault();
                    BrownAndWhiteDetails electronicItem = session.Query<BrownAndWhiteDetails>()
                        .Where(a => a.Id == electronicPolicy.BAndWId).FirstOrDefault();
                    //saving history
                    BrownAndWhiteDetailsHistory electronicHistory = DBDTOTransformer.Instance.DBBrownAndWhiteDetailsToBrownAndWhiteHistory(electronicItem, bundleTransaction);
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.Save(electronicHistory, electronicHistory.Id);
                        transaction.Commit();
                    }
                    //update live data
                    BrownAndWhiteDetails electronicItemDetails = DBDTOTransformer.Instance.PolicyTransactionToBrownAndWhiteEntity(electronicItem, bundleTransaction, policyTransactionList.FirstOrDefault());
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.Evict(electronicItem);
                        session.Update(electronicItemDetails, electronicItemDetails.Id);
                        transaction.Commit();
                    }

                }
                else if (commodityType.CommodityCode.ToLower().StartsWith("o"))
                {
                    OtherItemPolicy otherItemPolicy = session.Query<OtherItemPolicy>()
                        .Where(a => a.PolicyId == policies.FirstOrDefault().Id).FirstOrDefault();
                    OtherItemDetails otherItem = session.Query<OtherItemDetails>()
                        .Where(a => a.Id == otherItemPolicy.OtherItemId).FirstOrDefault();
                    //saving history
                    OtherItemDetailsHistory otherItemHistory = DBDTOTransformer.Instance.DBOtherItemDetailsToOtherItemDetailsHistory(otherItem, bundleTransaction);
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.Save(otherItemHistory, otherItemHistory.Id);
                        transaction.Commit();
                    }

                    //update live data
                    OtherItemDetails otherItemDetails = DBDTOTransformer.Instance.PolicyTransactionToOtherItemEntity(otherItem, bundleTransaction, policyTransactionList.FirstOrDefault());
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.Evict(otherItem);
                        session.Update(otherItemDetails, otherItemDetails.Id);
                        transaction.Commit();
                    }
                }
                else if (commodityType.CommodityCode.ToLower().StartsWith("y"))
                {
                    YellowGoodPolicy yellowGoodPolicy = session.Query<YellowGoodPolicy>()
                        .Where(a => a.PolicyId == policies.FirstOrDefault().Id).FirstOrDefault();
                    YellowGoodDetails yellowGood = session.Query<YellowGoodDetails>()
                        .Where(a => a.Id == yellowGoodPolicy.Id).FirstOrDefault();
                    //saving history
                    YellowGoodDetailsHistory yellowGoodHistory = DBDTOTransformer.Instance.DBYellowGoodDetailsToYellowGoodHistory(yellowGood, bundleTransaction);
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.Save(yellowGoodHistory, yellowGoodHistory.Id);
                        transaction.Commit();
                    }

                    //update live data
                    YellowGoodDetails yellowGoodDetails = DBDTOTransformer.Instance.PolicyTransactionToYellowGoodEntity(yellowGood, bundleTransaction, policyTransactionList.FirstOrDefault());
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.Evict(yellowGood);
                        session.Update(yellowGoodDetails, yellowGoodDetails.Id);
                        transaction.Commit();
                    }

                }

                #endregion "Item Information"

                #region "CustomerInformation"
                Customer customer = session.Query<Customer>()
                    .Where(a => a.Id == policyBundle.CustomerId).FirstOrDefault();
                //saving customer history
                CustomerHistory customerHistory = DBDTOTransformer.Instance.CustomerEntityToCustomerHistory(customer, bundleTransaction);
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Save(customerHistory, customerHistory.Id);
                    transaction.Commit();
                }
                //update live data
                Customer customerDetails = DBDTOTransformer.Instance.PolicyTransactionToCustomer(customer, bundleTransaction, policyTransactionList.FirstOrDefault());
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Evict(customer);
                    session.Update(customerDetails, customerDetails.Id);
                    transaction.Commit();
                }
                #endregion "CustomerInformation"

                #region "Policy Bundle Information"
                //saving policy bundle history
                PolicyBundleHistory policyBundleHistory = DBDTOTransformer.Instance.PolicyBundleToPolicyBundleHistory(policyBundle, bundleTransaction);
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Save(policyBundleHistory, policyBundleHistory.Id);
                    transaction.Commit();
                }
                //update policy bundle
                PolicyBundle policyBundleToSave = DBDTOTransformer.Instance.PolicyTransactionToPolicyBundle(policyBundleObj.FirstOrDefault(), bundleTransaction);
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Evict(policyBundleObj);
                    session.Update(policyBundleToSave, policyBundleToSave.Id);
                    transaction.Commit();
                }
                #endregion "Policy Bundle Information"

                #region "Policy Information"
                //save policy history
                List<PolicyHistory> policyHistoryList = DBDTOTransformer.Instance.PolicyToPolicyHistory(policies.ToList());
                foreach (PolicyHistory policyHistory in policyHistoryList)
                {
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        policyHistory.TransactionTypeId = policyBundleHistory.TransactionTypeId;
                        session.Save(policyHistory, policyHistory.Id);
                        transaction.Commit();
                    }
                }
                //update policy
                List<Policy> policyList = DBDTOTransformer.Instance.PolicyTransactionToPolicyList(policyTransactionList, bundleTransaction, policies.ToList());
                foreach (Policy policy in policyList)
                {
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.Evict(policies);
                        session.Update(policy, policy.Id);
                        transaction.Commit();
                    }
                }

                #endregion "Policy Information"

                #region "Transaction Information"

                using (ITransaction transaction = session.BeginTransaction())
                {
                    bundleTransaction.IsApproved = true;
                    session.Update(bundleTransaction, bundleTransaction.Id);
                    transaction.Commit();
                }

                foreach (PolicyTransaction policyTransaction in policyTransactionList)
                {
                    policyTransaction.IsApproved = true;
                    policyTransaction.IsRecordActive = false;
                    policyTransaction.IsRejected = false;
                    policyTransaction.ApprovedRejectedBy = UserId;
                    policyTransaction.TransactionTypeId = GetTransactionTypeId("EndorsementApproval");

                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.Update(policyTransaction, policyTransaction.Id);
                        transaction.Commit();
                    }

                }
                #endregion "Transaction Information"

                Response = "Success";
            }
            catch (Exception ex)
            {
                Response = "Error Occured. Please Retry.";
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ((ex.InnerException != null) ? ex.InnerException.ToString() : ""));
            }
            return Response;

        }
        internal static string RejectEndorsement(Guid PolicyBundleId, Guid UserId)
        {
            String Response = "Invalid policy bundle";
            try
            {
                ISession session = EntitySessionManager.GetSession();

                PolicyBundleTransaction bundleTransaction = session.Query<PolicyBundleTransaction>()
                 .Where(a => a.PolicyBundleId == PolicyBundleId && a.IsApproved != true && a.IsPolicyCanceled != true).FirstOrDefault();
                if (bundleTransaction == null)
                {
                    return Response;
                }

                List<PolicyTransaction> policyTransactionList = session.Query<PolicyTransaction>()
                    .Where(a => a.PolicyBundleTransactionId == bundleTransaction.Id).ToList();
                if (policyTransactionList == null || policyTransactionList.Count() == 0)
                {
                    return Response;
                }

                //update
                bundleTransaction.IsApproved = false;

                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Update(bundleTransaction, bundleTransaction.Id);
                    transaction.Commit();
                }

                foreach (PolicyTransaction policyTransaction in policyTransactionList)
                {
                    policyTransaction.IsActive = false;
                    policyTransaction.IsApproved = false;
                    policyTransaction.IsRejected = true;
                    policyTransaction.ApprovedRejectedBy = UserId;


                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.Update(policyTransaction, policyTransaction.Id);
                        transaction.Commit();
                    }
                }

                Response = "Success";
            }
            catch (Exception ex)
            {

                Response = "Error Occured. Please Retry.";
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ((ex.InnerException != null) ? ex.InnerException.ToString() : ""));
            }

            return Response;
        }
        internal static string PolicyCancellationRequest(Guid PolicyBundleId, Guid UserId, string CancellationComment)
        {
            string Response = "Invalid Policy Selection";
            try
            {
                //validate
                if (UserId == Guid.Empty || PolicyBundleId == Guid.Empty
                    || String.IsNullOrEmpty(CancellationComment))
                {
                    Response = "Request data validation failed";
                }
                else
                {
                    ISession session = EntitySessionManager.GetSession();
                    Guid CamcellationTransactionTypeId = GetTransactionTypeId("Cancellation");
                    PolicyBundleTransaction bundleTransactionPreCheck = session.Query<PolicyBundleTransaction>()
                        .Where(a => a.PolicyBundleId == PolicyBundleId && a.TransactionTypeId == CamcellationTransactionTypeId).OrderByDescending(a => a.EntryDateTime).FirstOrDefault();
                    if (bundleTransactionPreCheck != null)
                    {
                        List<PolicyTransaction> policyListPreCheck = session.Query<PolicyTransaction>()
                            .Where(a => a.PolicyBundleTransactionId == bundleTransactionPreCheck.Id && a.IsRecordActive == true).ToList();
                        if (policyListPreCheck.Count > 0)
                        {
                            Response = "Another pending cancellation request found for this policy.Resolve it before request again.";
                            return Response;
                        }
                    }


                    PolicyBundle currentBundle = session.Query<PolicyBundle>()
                        .Where(a => a.Id == PolicyBundleId).FirstOrDefault();
                    if (currentBundle == null)
                    {
                        return Response;
                    }

                    IEnumerable<Policy> policies = session.Query<Policy>()
                   .Where(a => a.PolicyBundleId == PolicyBundleId);
                    if (policies == null || policies.Count() == 0)
                    {
                        return Response;
                    }

                    //save policy bundle transaction
                    Guid purposedBundleId = Guid.NewGuid();
                    PolicyBundleTransaction bundleTransaction = new PolicyBundleTransaction()
                    {
                        CancellationComment = CancellationComment,
                        Comment = currentBundle.Comment,
                        CommodityTypeId = currentBundle.CommodityTypeId,
                        ContractId = Guid.Empty,
                        CoverTypeId = Guid.Empty,
                        CustomerId = currentBundle.CustomerId,
                        CustomerPayment = currentBundle.CustomerPayment,
                        CustomerPaymentCurrencyTypeId = currentBundle.CustomerPaymentCurrencyTypeId,
                        DealerId = currentBundle.DealerId,
                        DealerLocationId = currentBundle.DealerLocationId,
                        DealerPayment = currentBundle.DealerPayment,
                        DealerPaymentCurrencyTypeId = currentBundle.DealerPaymentCurrencyTypeId,
                        DealerPolicy = currentBundle.DealerPolicy,
                        Discount = currentBundle.Discount,
                        EntryDateTime = DateTime.UtcNow,
                        EntryUser = UserId,
                        ExtensionTypeId = Guid.Empty,
                        HrsUsedAtPolicySale = currentBundle.HrsUsedAtPolicySale,
                        Id = purposedBundleId,
                        IsApproved = false,
                        IsPartialPayment = currentBundle.IsPartialPayment,
                        IsPolicyCanceled = false,
                        IsPreWarrantyCheck = false,
                        IsSpecialDeal = currentBundle.IsSpecialDeal,
                        PaymentModeId = currentBundle.PaymentModeId,
                        PolicyBundleId = currentBundle.Id,
                        PolicyNo = "",
                        PolicySoldDate = currentBundle.PolicySoldDate,
                        Premium = currentBundle.Premium,
                        PremiumCurrencyTypeId = Guid.Empty,
                        ProductId = currentBundle.ProductId,
                        RefNo = currentBundle.RefNo,
                        SalesPersonId = currentBundle.SalesPersonId,
                        TransactionTypeId = GetTransactionTypeId("Cancellation"),
                        Type = "Cancellation",
                        MWStartDate = currentBundle.MWStartDate,
                        ContractExtensionPremiumId = currentBundle.ContractExtensionPremiumId,
                        ContractExtensionsId = currentBundle.ContractExtensionsId,
                        ContractInsuaranceLimitationId = currentBundle.ContractInsuaranceLimitationId,
                        BookletNumber = currentBundle.BookletNumber
                    };
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.Save(bundleTransaction, bundleTransaction.Id);
                        transaction.Commit();
                    }

                    //save policy transaction
                    //no need to save all details in PolicyTransaction coz cancellation dose not change any data
                    foreach (Policy policy in policies)
                    {
                        PolicyTransaction policyTransaction = new PolicyTransaction()
                        {
                            PolicyBundleTransactionId = purposedBundleId,
                            PolicyBundleId = bundleTransaction.PolicyBundleId,
                            PolicyId = policy.Id,
                            IsApproved = false,
                            IsRecordActive = true,
                            IsRejected = false,
                            Id = Guid.NewGuid(),
                            TransactionTypeId = GetTransactionTypeId("Cancellation"),
                            DateOfBirth = SqlDateTime.MinValue.Value,
                            PolicyEndDate = SqlDateTime.MinValue.Value,
                            PolicySoldDate = SqlDateTime.MinValue.Value,
                            PolicyStartDate = SqlDateTime.MinValue.Value,
                            DLIssueDate = SqlDateTime.MinValue.Value,
                            ItemPurchasedDate = SqlDateTime.MinValue.Value,
                            ModifiedDate = DateTime.UtcNow,
                            PolicyNo = policy.PolicyNo,
                            RegistrationDate = SqlDateTime.MinValue.Value,
                            MWStartDate = policy.MWStartDate,

                        };

                        using (ITransaction transaction = session.BeginTransaction())
                        {
                            session.Save(policyTransaction, policyTransaction.Id);
                            transaction.Commit();
                        }
                    }
                }
                Response = "success";
            }
            catch (Exception ex)
            {
                Response = "Error Occured in Policy Cancellation";
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ((ex.InnerException != null) ? ex.InnerException.ToString() : ""));
            }
            return Response;
        }

        internal static string GetPolicyCancellationCommentByPolicyBundleId(Guid PolicyBundleId)
        {
            String Response = String.Empty;
            try
            {
                ISession session = EntitySessionManager.GetSession();

                PolicyBundleTransaction bundleTransaction = session.Query<PolicyBundleTransaction>()
                 .Where(a => a.PolicyBundleId == PolicyBundleId && a.IsApproved != true && a.IsPolicyCanceled != true).FirstOrDefault();
                if (bundleTransaction == null)
                {
                    return Response;
                }

                Response = bundleTransaction.CancellationComment;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ((ex.InnerException != null) ? ex.InnerException.ToString() : ""));
            }

            return Response;
        }

        internal static string ApprovePolicyCancellation(Guid PolicyBundleId, Guid UserId)
        {

            String Response = "Policy selection invalid";
            try
            {
                ISession session = EntitySessionManager.GetSession();

                PolicyBundle policyBundle = session.Query<PolicyBundle>()
                     .Where(a => a.Id == PolicyBundleId).FirstOrDefault();
                if (policyBundle == null)
                {
                    return Response;
                }

                IEnumerable<Policy> policies = session.Query<Policy>()
                    .Where(a => a.PolicyBundleId == PolicyBundleId);
                if (policies == null || policies.Count() == 0)
                {
                    return Response;
                }

                PolicyBundleTransaction bundleTransaction = session.Query<PolicyBundleTransaction>()
                    .Where(a => a.PolicyBundleId == PolicyBundleId && a.IsApproved != true && a.IsPolicyCanceled != true && a.TransactionTypeId == GetTransactionTypeId("Cancellation")).FirstOrDefault();
                if (bundleTransaction == null)
                {
                    return Response;
                }

                List<PolicyTransaction> policyTransactionList = session.Query<PolicyTransaction>()
                    .Where(a => a.PolicyBundleTransactionId == bundleTransaction.Id).ToList();
                if (policyTransactionList == null || policyTransactionList.Count() == 0)
                {
                    return Response;
                }

                #region "Policy Bundle Information"
                //saving policy bundle history
                PolicyBundleHistory policyBundleHistory = DBDTOTransformer.Instance.PolicyBundleToPolicyBundleHistory(policyBundle, bundleTransaction);
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Save(policyBundleHistory, policyBundleHistory.Id);
                    transaction.Commit();
                }
                //update policy bundle
                using (ITransaction transaction = session.BeginTransaction())
                {
                    policyBundle.IsPolicyCanceled = true;
                    session.Update(policyBundle, policyBundle.Id);
                    transaction.Commit();
                }
                #endregion "Policy Bundle Information"

                #region "Policy Information"
                //save policy history
                List<PolicyHistory> policyHistoryList = DBDTOTransformer.Instance.PolicyToPolicyHistory(policies.ToList());
                foreach (PolicyHistory policyHistory in policyHistoryList)
                {
                    using (ITransaction transaction = session.BeginTransaction())
                    {

                        policyHistory.TransactionTypeId = policyBundleHistory.TransactionTypeId;
                        session.Save(policyHistory, policyHistory.Id);
                        transaction.Commit();
                    }
                }
                //update policy
                foreach (Policy policy in policies.ToList())
                {

                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        Policy existingPolicy = session.Query<Policy>().Where(a => a.Id == policy.Id).FirstOrDefault();

                        policy.IsPolicyCanceled = true;
                        policy.ApprovedDate = existingPolicy.ApprovedDate;
                        policy.BordxNumber = 0;
                        policy.Year = 0;
                        policy.Month= 0;
                        policy.BordxId= Guid.Empty;
                        session.Update(policy, policy.Id);
                        transaction.Commit();
                    }
                }

                #endregion "Policy Information"
                #region "Transaction Information"

                using (ITransaction transaction = session.BeginTransaction())
                {
                    bundleTransaction.IsApproved = true;
                    session.Update(bundleTransaction, bundleTransaction.Id);
                    transaction.Commit();
                }

                foreach (PolicyTransaction policyTransaction in policyTransactionList)
                {
                    policyTransaction.IsApproved = true;
                    policyTransaction.IsRecordActive = false;
                    policyTransaction.IsRejected = false;
                    policyTransaction.ApprovedRejectedBy = UserId;

                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.Update(policyTransaction, policyTransaction.Id);
                        transaction.Commit();
                    }

                }
                #endregion "Transaction Information"
                Response = "Success";
            }
            catch (Exception ex)
            {
                Response = "Error Occured. Please Retry.";
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ((ex.InnerException != null) ? ex.InnerException.ToString() : ""));
            }

            return Response;
        }
        internal static string RejectPolicyCancellation(Guid PolicyBundleId, Guid UserId)
        {
            String Response = "Policy selection invalid";
            try
            {
                ISession session = EntitySessionManager.GetSession();

                PolicyBundle policyBundle = session.Query<PolicyBundle>()
                     .Where(a => a.Id == PolicyBundleId).FirstOrDefault();
                if (policyBundle == null)
                {
                    return Response;
                }

                IEnumerable<Policy> policies = session.Query<Policy>()
                    .Where(a => a.PolicyBundleId == PolicyBundleId);
                if (policies == null || policies.Count() == 0)
                {
                    return Response;
                }

                PolicyBundleTransaction bundleTransaction = session.Query<PolicyBundleTransaction>()
                    .Where(a => a.PolicyBundleId == PolicyBundleId && a.IsApproved != true && a.IsPolicyCanceled != true && a.TransactionTypeId == GetTransactionTypeId("Cancellation")).FirstOrDefault();
                if (bundleTransaction == null)
                {
                    return Response;
                }

                List<PolicyTransaction> policyTransactionList = session.Query<PolicyTransaction>()
                    .Where(a => a.PolicyBundleTransactionId == bundleTransaction.Id).ToList();
                if (policyTransactionList == null || policyTransactionList.Count() == 0)
                {
                    return Response;
                }

                #region "Transaction Information"

                using (ITransaction transaction = session.BeginTransaction())
                {
                    bundleTransaction.IsApproved = false;
                    session.Update(bundleTransaction, bundleTransaction.Id);
                    transaction.Commit();
                }

                foreach (PolicyTransaction policyTransaction in policyTransactionList)
                {
                    policyTransaction.IsApproved = false;
                    policyTransaction.IsRecordActive = false;
                    policyTransaction.IsRejected = true;
                    policyTransaction.ApprovedRejectedBy = UserId;

                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.Update(policyTransaction, policyTransaction.Id);
                        transaction.Commit();
                    }

                }
                #endregion "Transaction Information"
                Response = "Success";
            }
            catch (Exception ex)
            {
                Response = "Error Occured. Please Retry.";
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ((ex.InnerException != null) ? ex.InnerException.ToString() : ""));
            }

            return Response;
        }
        #endregion

        #region History
        internal bool AddPolicyHistory(PolicyHistoryRequestDto Policy)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();
                PolicyHistory pr = new Entities.PolicyHistory();
                List<PolicyContractProduct> p = new List<Entities.PolicyContractProduct>();
                if (!Char.IsLetter(Policy.Gender)) {
                    Policy.Gender = ' ';
                }
                pr.Id = new Guid();
                pr.CommodityUsageTypeId = Policy.CommodityUsageTypeId;
                pr.AddnSerialNo = Policy.AddnSerialNo;
                pr.Address1 = Policy.Address1;
                pr.Address2 = Policy.Address2;
                pr.Address3 = Policy.Address3;
                pr.Address4 = Policy.Address4;
                pr.AspirationId = Policy.AspirationId;
                pr.BodyTypeId = Policy.BodyTypeId;
                pr.BusinessAddress1 = Policy.BusinessAddress1;
                pr.BusinessAddress2 = Policy.BusinessAddress2;
                pr.BusinessAddress3 = Policy.BusinessAddress3;
                pr.BusinessAddress4 = Policy.BusinessAddress4;
                pr.BusinessName = Policy.BusinessName;
                pr.BusinessTelNo = Policy.BusinessTelNo;
                pr.CategoryId = Policy.CategoryId;
                pr.CityId = Policy.CityId;
                pr.Comment = Policy.Comment;
                pr.CommodityTypeId = Policy.CommodityTypeId;
                pr.ContractId = Policy.ContractId;
                pr.CountryId = Policy.CountryId;
                pr.CoverTypeId = Policy.CoverTypeId;
                pr.CustomerPayment = Policy.CustomerPayment;
                pr.CustomerPaymentCurrencyTypeId = Policy.CustomerPaymentCurrencyTypeId;
                pr.CustomerTypeId = Policy.CustomerTypeId;
                pr.CylinderCountId = Policy.CylinderCountId;
                pr.DateOfBirth = Policy.DateOfBirth;
                pr.DealerId = Policy.DealerId;
                pr.DealerLocationId = Policy.DealerLocationId;
                pr.DealerPayment = Policy.DealerPayment;
                pr.DealerPaymentCurrencyTypeId = Policy.DealerPaymentCurrencyTypeId;
                pr.DealerPrice = Policy.DealerPrice;
                pr.DLIssueDate = Policy.DLIssueDate;
                pr.DriveTypeId = Policy.DriveTypeId;
                pr.Email = Policy.Email;
                pr.EngineCapacityId = Policy.EngineCapacityId;
                pr.ExtensionTypeId = Policy.ExtensionTypeId;
                pr.FirstName = Policy.FirstName;
                pr.FuelTypeId = Policy.FuelTypeId;
                pr.Gender = Policy.Gender;
                pr.HrsUsedAtPolicySale = Policy.HrsUsedAtPolicySale;
                pr.IDNo = Policy.IDNo;
                pr.IDTypeId = Policy.IDTypeId;
                pr.InvoiceNo = Policy.InvoiceNo;
                pr.IsActive = Policy.IsActive;
                pr.IsApproved = Policy.IsApproved;
                pr.IsPartialPayment = Policy.IsPartialPayment;
                pr.IsPreWarrantyCheck = Policy.IsPreWarrantyCheck;
                pr.IsSpecialDeal = Policy.IsSpecialDeal;
                pr.ItemPrice = Policy.ItemPrice;
                pr.ItemPurchasedDate = Convert.ToDateTime("1900/10/10");
                pr.ItemStatusId = Policy.ItemStatusId;
                pr.LastName = Policy.LastName;
                pr.MakeId = Policy.MakeId;
                pr.MobileNo = Policy.MobileNo;
                pr.ModelCode = Policy.ModelCode;
                pr.ModelId = Policy.ModelId;
                pr.ModelYear = Policy.ModelYear;
                pr.NationalityId = Policy.NationalityId;
                pr.OtherTelNo = Policy.OtherTelNo;
                pr.Password = Policy.Password;
                pr.PaymentModeId = Policy.PaymentModeId;
                pr.PlateNo = Policy.PlateNo;
                pr.PolicyNo = Policy.PolicyNo;
                pr.PolicySoldDate = Convert.ToDateTime("1900/10/10");
                pr.Premium = Policy.Premium;
                pr.PremiumCurrencyTypeId = Policy.PremiumCurrencyTypeId;
                pr.ProductId = Policy.ProductId;
                pr.ProfilePicture = Policy.ProfilePicture;
                pr.RefNo = Policy.RefNo;
                pr.SalesPersonId = Policy.SalesPersonId;
                pr.SerialNo = Policy.SerialNo;
                pr.TransmissionId = Policy.TransmissionId;
                pr.UsageTypeId = Policy.UsageTypeId;
                pr.UserName = Policy.UserName;
                pr.Variant = Policy.Variant;
                pr.VehiclePrice = Policy.VehiclePrice;
                pr.VINNo = Policy.VINNo;
                pr.CustomerId = Policy.CustomerId;
                pr.VehicleId = Policy.VehicleId;
                pr.BAndWId = Policy.BAndWId;
                pr.PolicyStartDate = Convert.ToDateTime("1900/10/10");
                pr.PolicyEndDate = Convert.ToDateTime("1900/10/10");
                pr.Discount = Policy.Discount;
                pr.ModifiedUser = Policy.ModifiedUser;
                pr.ModifiedDate = DateTime.Today.ToUniversalTime();
                pr.TransactionTypeId = Policy.TransactionTypeId;
                pr.PolicyBundleId = Policy.PolicyBundleId;
                pr.TransferFee = Policy.TransferFee;
                pr.DealerPolicy = Policy.DealerPolicy;

                if(Policy.ContractProducts != null)
                {
                    foreach (var item in Policy.ContractProducts)
                    {
                        PolicyContractProduct pp = new Entities.PolicyContractProduct()
                        {
                            ContractId = item.ContractId,
                            CoverTypeId = item.CoverTypeId,
                            ExtensionTypeId = item.ExtensionTypeId,
                            ParentProductId = item.ParentProductId,
                            Premium = item.Premium,
                            PremiumCurrencyTypeId = item.PremiumCurrencyTypeId,
                            ProductId = item.ProductId,
                            Type = "History"
                        };
                        p.Add(pp);

                    }
                }

                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.SaveOrUpdate(pr);

                    if (Policy.ContractProducts != null)
                    {
                        foreach (var item in p)
                        {
                            item.Id = new Guid();
                            item.ReferenceId = pr.Id;
                            session.SaveOrUpdate(item);
                        }
                    }

                    transaction.Commit();
                }

                return true;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ((ex.InnerException != null) ? ex.InnerException.ToString() : ""));
                return false;
            }
        }
        public List<PolicyHistory> GetPolicyHistories()
        {
            List<PolicyHistory> entities = null;
            ISession session = EntitySessionManager.GetSession();
            IQueryable<PolicyHistory> history = session.Query<PolicyHistory>();
            entities = history.ToList();
            return entities;
        }
        public PolicyHistoryResponseDto GetPolicyHistoryById(Guid PolicyHistoryId)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();
                PolicyHistoryResponseDto pr = new PolicyHistoryResponseDto();
                var query =
                    from PolicyHistory in session.Query<PolicyHistory>()
                    where PolicyHistory.Id == PolicyHistoryId
                    select new { PolicyHistory = PolicyHistory };
                var result = query.ToList();

                var queryP =
                 from PolicyContractProduct in session.Query<PolicyContractProduct>()
                 where PolicyContractProduct.ReferenceId == PolicyHistoryId && PolicyContractProduct.Type == "History"
                 select new { PolicyContractProduct = PolicyContractProduct };
                var resultP = queryP.ToList();

                List<PolicyContractProductResponseDto> prod = new List<PolicyContractProductResponseDto>();
                foreach (var item in resultP)
                {
                    PolicyContractProductResponseDto p = new PolicyContractProductResponseDto()
                    {
                        ContractId = item.PolicyContractProduct.ContractId,
                        ExtensionTypeId = item.PolicyContractProduct.ExtensionTypeId,
                        CoverTypeId = item.PolicyContractProduct.CoverTypeId,
                        Id = item.PolicyContractProduct.Id,
                        ParentProductId = item.PolicyContractProduct.ParentProductId,
                        Premium = item.PolicyContractProduct.Premium,
                        PremiumCurrencyTypeId = item.PolicyContractProduct.PremiumCurrencyTypeId,
                        ProductId = item.PolicyContractProduct.ProductId,
                        ReferenceId = item.PolicyContractProduct.ReferenceId,
                        Type = item.PolicyContractProduct.Type
                    };
                    prod.Add(p);
                }

                if (result != null && result.Count > 0)
                {
                    pr.Id = result.First().PolicyHistory.Id;
                    pr.PolicyId = result.First().PolicyHistory.PolicyId;
                    pr.AddnSerialNo = result.First().PolicyHistory.AddnSerialNo;
                    pr.CommodityUsageTypeId = result.First().PolicyHistory.CommodityUsageTypeId;
                    pr.Address1 = result.First().PolicyHistory.Address1;
                    pr.Address2 = result.First().PolicyHistory.Address2;
                    pr.Address3 = result.First().PolicyHistory.Address3;
                    pr.Address4 = result.First().PolicyHistory.Address4;
                    pr.AspirationId = result.First().PolicyHistory.AspirationId;
                    pr.BodyTypeId = result.First().PolicyHistory.BodyTypeId;
                    pr.BusinessAddress1 = result.First().PolicyHistory.BusinessAddress1;
                    pr.BusinessAddress2 = result.First().PolicyHistory.BusinessAddress2;
                    pr.BusinessAddress3 = result.First().PolicyHistory.BusinessAddress3;
                    pr.BusinessAddress4 = result.First().PolicyHistory.BusinessAddress4;
                    pr.BusinessName = result.First().PolicyHistory.BusinessName;
                    pr.BusinessTelNo = result.First().PolicyHistory.BusinessTelNo;
                    pr.CategoryId = result.First().PolicyHistory.CategoryId;
                    pr.CityId = result.First().PolicyHistory.CityId;
                    pr.Comment = result.First().PolicyHistory.Comment;
                    pr.CommodityTypeId = result.First().PolicyHistory.CommodityTypeId;
                    pr.ContractId = result.First().PolicyHistory.ContractId;
                    pr.CountryId = result.First().PolicyHistory.CountryId;
                    pr.CoverTypeId = result.First().PolicyHistory.CoverTypeId;
                    pr.CustomerPayment = result.First().PolicyHistory.CustomerPayment;
                    pr.CustomerPaymentCurrencyTypeId = result.First().PolicyHistory.CustomerPaymentCurrencyTypeId;
                    pr.CustomerTypeId = result.First().PolicyHistory.CustomerTypeId;
                    pr.CylinderCountId = result.First().PolicyHistory.CylinderCountId;
                    pr.DateOfBirth = result.First().PolicyHistory.DateOfBirth;
                    pr.DealerId = result.First().PolicyHistory.DealerId;
                    pr.DealerLocationId = result.First().PolicyHistory.DealerLocationId;
                    pr.DealerPayment = result.First().PolicyHistory.DealerPayment;
                    pr.DealerPaymentCurrencyTypeId = result.First().PolicyHistory.DealerPaymentCurrencyTypeId;
                    pr.DealerPrice = result.First().PolicyHistory.DealerPrice;
                    pr.DLIssueDate = result.First().PolicyHistory.DLIssueDate;
                    pr.DriveTypeId = result.First().PolicyHistory.DriveTypeId;
                    pr.Email = result.First().PolicyHistory.Email;
                    pr.EngineCapacityId = result.First().PolicyHistory.EngineCapacityId;
                    pr.ExtensionTypeId = result.First().PolicyHistory.ExtensionTypeId;
                    pr.FirstName = result.First().PolicyHistory.FirstName;
                    pr.FuelTypeId = result.First().PolicyHistory.FuelTypeId;
                    pr.Gender = result.First().PolicyHistory.Gender;
                    pr.HrsUsedAtPolicySale = result.First().PolicyHistory.HrsUsedAtPolicySale;
                    pr.IDNo = result.First().PolicyHistory.IDNo;
                    pr.IDTypeId = result.First().PolicyHistory.IDTypeId;
                    pr.InvoiceNo = result.First().PolicyHistory.InvoiceNo;
                    pr.IsActive = result.First().PolicyHistory.IsActive;
                    pr.IsApproved = result.First().PolicyHistory.IsApproved;
                    pr.IsPartialPayment = result.First().PolicyHistory.IsPartialPayment;
                    pr.IsPreWarrantyCheck = result.First().PolicyHistory.IsPreWarrantyCheck;
                    pr.IsSpecialDeal = result.First().PolicyHistory.IsSpecialDeal;
                    pr.ItemPrice = result.First().PolicyHistory.ItemPrice;
                    pr.ItemPurchasedDate = result.First().PolicyHistory.ItemPurchasedDate;
                    pr.ItemStatusId = result.First().PolicyHistory.ItemStatusId;
                    pr.LastName = result.First().PolicyHistory.LastName;
                    pr.MakeId = result.First().PolicyHistory.MakeId;
                    pr.MobileNo = result.First().PolicyHistory.MobileNo;
                    pr.ModelCode = result.First().PolicyHistory.ModelCode;
                    pr.ModelId = result.First().PolicyHistory.ModelId;
                    pr.ModelYear = result.First().PolicyHistory.ModelYear;
                    pr.NationalityId = result.First().PolicyHistory.NationalityId;
                    pr.OtherTelNo = result.First().PolicyHistory.OtherTelNo;
                    pr.Password = result.First().PolicyHistory.Password;
                    pr.PaymentModeId = result.First().PolicyHistory.PaymentModeId;
                    pr.PlateNo = result.First().PolicyHistory.PlateNo;
                    pr.PolicyNo = result.First().PolicyHistory.PolicyNo;
                    pr.PolicySoldDate = result.First().PolicyHistory.PolicySoldDate;
                    pr.Premium = result.First().PolicyHistory.Premium;
                    pr.PremiumCurrencyTypeId = result.First().PolicyHistory.PremiumCurrencyTypeId;
                    pr.ProductId = result.First().PolicyHistory.ProductId;
                    pr.ProfilePicture = result.First().PolicyHistory.ProfilePicture;
                    pr.RefNo = result.First().PolicyHistory.RefNo;
                    pr.SalesPersonId = result.First().PolicyHistory.SalesPersonId;
                    pr.SerialNo = result.First().PolicyHistory.SerialNo;
                    pr.TransmissionId = result.First().PolicyHistory.TransmissionId;
                    pr.UsageTypeId = result.First().PolicyHistory.UsageTypeId;
                    pr.UserName = result.First().PolicyHistory.UserName;
                    pr.Variant = result.First().PolicyHistory.Variant;
                    pr.VehiclePrice = result.First().PolicyHistory.VehiclePrice;
                    pr.VINNo = result.First().PolicyHistory.VINNo;
                    pr.TransactionTypeId = result.First().PolicyHistory.TransactionTypeId;
                    pr.IsRecordActive = result.First().PolicyHistory.IsRecordActive;
                    pr.CancelationComment = result.First().PolicyHistory.CancelationComment;
                    pr.PolicyStartDate = result.First().PolicyHistory.PolicyStartDate;
                    pr.PolicyEndDate = result.First().PolicyHistory.PolicyEndDate;
                    pr.Discount = result.First().PolicyHistory.Discount;
                    pr.ContractProducts = prod;
                    pr.ModifiedUser = pr.ModifiedUser;
                    pr.PolicyBundleId = pr.PolicyBundleId;
                    pr.TransferFee = pr.TransferFee;
                    pr.IsPolicyExists = true;
                    pr.DealerPolicy = pr.DealerPolicy;
                    return pr;
                }
                else
                {
                    pr.IsPolicyExists = false;
                    return null;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ((ex.InnerException != null) ? ex.InnerException.ToString() : ""));
                return null;
            }
        }

        internal object GetPloiciesForSearchGridReneval(PolicySearchGridRequestDto PolicySearchGridRequestDto)
        {

            ISession session = EntitySessionManager.GetSession();
            DateTime DueDate = DateTime.Today.AddDays(30);
            DateTime Today = DateTime.Today;
            //expression builder for policy
            Expression<Func<Policy, bool>> filterPolicy = PredicateBuilder.True<Policy>();
            filterPolicy = filterPolicy.And(a => a.PolicyEndDate < DueDate && a.PolicyEndDate > Today);


            if (PolicySearchGridRequestDto.type == "forapproval")
            {
                filterPolicy = filterPolicy.And(a => a.IsApproved == false);
            }
            else if (PolicySearchGridRequestDto.type == "forpolicyreg")
            {
                filterPolicy = filterPolicy.And(a => a.BordxId == Guid.Empty || a.BordxId == null);
            }
            else if (PolicySearchGridRequestDto.type == "forendorsement")
            {
                filterPolicy = filterPolicy.And(a => a.BordxId != Guid.Empty && a.BordxId != null);
            }
            else if (PolicySearchGridRequestDto.type == "forpolicydealerreg")
            {
                filterPolicy = filterPolicy.And(a => a.DealerPolicy);
            }
            else if (PolicySearchGridRequestDto.type == "forendorsementapproval")
            {
                IEnumerable<PolicyTransaction> policyTransaction = session.Query<PolicyTransaction>()
                    .Where(a => a.IsRecordActive == true && a.IsApproved == false && a.IsRejected == false && a.TransactionTypeId == GetTransactionTypeId("Endorsement"));
                filterPolicy = filterPolicy.And(a => policyTransaction.Any(b => b.PolicyId == a.Id));
            }
            else if (PolicySearchGridRequestDto.type == "forcancellation")
            {
                filterPolicy = filterPolicy.And(a => a.IsApproved == true && a.IsPolicyCanceled == false);
            }
            else if (PolicySearchGridRequestDto.type == "forcancellationapproval")
            {
                IEnumerable<PolicyTransaction> policyTransaction = session.Query<PolicyTransaction>()
                    .Where(a => a.IsRecordActive == true && a.IsApproved == false && a.IsRejected == false && a.TransactionTypeId == GetTransactionTypeId("Cancellation"));
                filterPolicy = filterPolicy.And(a => policyTransaction.Any(b => b.PolicyId == a.Id));
            }
            else if (PolicySearchGridRequestDto.type == "forpolicytransfer")
            {
                filterPolicy = filterPolicy.And(a => a.BordxId != Guid.Empty && a.BordxId != null && a.IsPolicyCanceled == false && a.IsPolicyRenewed == false);
            }
            else if (PolicySearchGridRequestDto.type == "forpolicyrenewal")
            {
                filterPolicy = filterPolicy.And(a => a.BordxId != Guid.Empty && a.BordxId != null && a.IsPolicyCanceled == false && a.IsPolicyRenewed == false);
            }
            else if (PolicySearchGridRequestDto.type == "forclaimsub")
            {
                filterPolicy = filterPolicy.And(a => a.IsPolicyCanceled == false && a.IsApproved == true && a.IsPolicyRenewed == false);
            }

            if (IsGuid(PolicySearchGridRequestDto.policySearchGridSearchCriterias.commodityTypeId.ToString()))
            {
                filterPolicy = filterPolicy.And(a => a.CommodityTypeId == PolicySearchGridRequestDto.policySearchGridSearchCriterias.commodityTypeId);
            }
            if (!String.IsNullOrEmpty(PolicySearchGridRequestDto.policySearchGridSearchCriterias.policyNo))
            {
                filterPolicy = filterPolicy.And(a => a.PolicyNo.ToLower().Contains(PolicySearchGridRequestDto.policySearchGridSearchCriterias.policyNo.ToLower()));
            }
            if (!String.IsNullOrEmpty(PolicySearchGridRequestDto.policySearchGridSearchCriterias.policyStartDate.ToString())
                && PolicySearchGridRequestDto.policySearchGridSearchCriterias.policyStartDate != DateTime.MinValue)
            {
                filterPolicy = filterPolicy.And(a => a.PolicyStartDate >= PolicySearchGridRequestDto.policySearchGridSearchCriterias.policyStartDate);
            }
            if (!String.IsNullOrEmpty(PolicySearchGridRequestDto.policySearchGridSearchCriterias.policyEndDate.ToString())
                && PolicySearchGridRequestDto.policySearchGridSearchCriterias.policyEndDate != DateTime.MinValue)
            {
                filterPolicy = filterPolicy.And(a => a.PolicyEndDate <= PolicySearchGridRequestDto.policySearchGridSearchCriterias.policyEndDate);
            }

            //userid filtering
            Guid requestedUserId = PolicySearchGridRequestDto.userId;
            if (requestedUserId == Guid.Empty)
            {
                return new object();
            }

            SystemUser sysUser = session.Query<SystemUser>().Where(a => a.LoginMapId == requestedUserId).FirstOrDefault();
            if (sysUser == null)
            {
                return new object();
            }

            UserType userType = session.Query<UserType>().Where(a => a.Id == sysUser.UserTypeId).FirstOrDefault();
            if (userType == null)
            {
                return new object();
            }

            if (userType.Code.ToUpper() == "IU")
            {
                if (PolicySearchGridRequestDto.type != "forclaimsub")
                {
                    IEnumerable<UserBranch> userBranches = session.Query<UserBranch>().Where(a => a.InternalUserId == requestedUserId);
                    filterPolicy = filterPolicy.And(a => userBranches.Any(b => b.TPABranchId == a.TPABranchId));
                }


            }


            var policyGridDetails = session.Query<Policy>().Where(filterPolicy);

            IQueryable<VehicleDetails> vehicle;
            IQueryable<BrownAndWhiteDetails> BnW;
            IQueryable<VehiclePolicy> vehiclePolicy;
            IQueryable<BAndWPolicy> BnWPolicy;
            IQueryable<OtherItemDetails> other;
            IQueryable<OtherItemPolicy> otherPolicy;

            if (!String.IsNullOrEmpty(PolicySearchGridRequestDto.policySearchGridSearchCriterias.serialNo))
            {
                vehicle = session.Query<VehicleDetails>().Where(a => a.VINNo.ToLower().Contains(PolicySearchGridRequestDto.policySearchGridSearchCriterias.serialNo.ToLower()));
                BnW = session.Query<BrownAndWhiteDetails>().Where(a => a.SerialNo.ToLower().Contains(PolicySearchGridRequestDto.policySearchGridSearchCriterias.serialNo.ToLower()));
                other = session.Query<OtherItemDetails>().Where(a => a.SerialNo.ToLower().Contains(PolicySearchGridRequestDto.policySearchGridSearchCriterias.serialNo.ToLower()));
                vehiclePolicy = session.Query<VehiclePolicy>().Where(b => vehicle.Any(c => c.Id == b.VehicleId));
                BnWPolicy = session.Query<BAndWPolicy>().Where(b => BnW.Any(c => c.Id == b.Id));
                otherPolicy = session.Query<OtherItemPolicy>().Where(b => other.Any(c => c.Id == b.OtherItemId));
                policyGridDetails = policyGridDetails.Where(d => vehiclePolicy.Any(e => e.PolicyId == d.Id) || BnWPolicy.Any(f => f.PolicyId == d.Id) || otherPolicy.Any(f => f.PolicyId == d.Id));

            }
            IQueryable<Customer> customers;
            List<Customer> customerList = new List<Customer>();
            if (!String.IsNullOrEmpty(PolicySearchGridRequestDto.policySearchGridSearchCriterias.mobileNo))
            {
                customers = session.Query<Customer>().Where(a => a.MobileNo.Contains(PolicySearchGridRequestDto.policySearchGridSearchCriterias.mobileNo));
                policyGridDetails = policyGridDetails.Where(b => customers.Any(c => c.Id == b.CustomerId));

            }

            IEnumerable<CommodityType> commodities = session.Query<CommodityType>();
            IEnumerable<Customer> customerData = session.Query<Customer>();

            long TotalRecords = policyGridDetails.Count();
            var policyGridDetailsFilterd = policyGridDetails
            .OrderByDescending(a => a.EntryDateTime)
            .Skip((PolicySearchGridRequestDto.paginationOptionsPolicySearchGrid.pageNumber - 1) * (PolicySearchGridRequestDto.paginationOptionsPolicySearchGrid.pageSize))
            .Take(PolicySearchGridRequestDto.paginationOptionsPolicySearchGrid.pageSize)
            .Join(commodities, m => m.CommodityTypeId, n => n.CommodityTypeId, (m, n) => new { m, n })
            .Join(customerData, o => o.m.CustomerId, p => p.Id, (o, p) => new { o, p })
            .Select(a => new
            {
                Id = a.o.m.PolicyBundleId,
                CommodityType = a.o.n.CommodityTypeDescription,
                PolicyNo = a.o.m.PolicyNo,
                SerialNo = getSerialNumberByPolicyId(a.o.m.Id),
                MobileNo = a.p.MobileNo,
                PolicySoldDate = a.o.m.PolicySoldDate.ToString("dd-MMM-yyyy"),
                //UniqueRe = a.o.m.UniqueRef
            }).ToArray();
            var response = new CommonGridResponseDto()
            {
                totalRecords = TotalRecords,
                data = policyGridDetailsFilterd
            };
            return new JavaScriptSerializer().Serialize(response);
        }

        internal object GetPloiciesForSearchGrid_old(PolicySearchGridRequestDto PolicySearchGridRequestDto)
        {

            ISession session = EntitySessionManager.GetSession();

            //expression builder for policy
            Expression<Func<Policy, bool>> filterPolicy = PredicateBuilder.True<Policy>();

            if (PolicySearchGridRequestDto.type == "forapproval")
            {
                filterPolicy = filterPolicy.And(a => a.IsApproved == false);
            }
            else if (PolicySearchGridRequestDto.type == "forpolicyreg")
            {
                filterPolicy = filterPolicy.And(a => a.BordxId == Guid.Empty || a.BordxId == null);
            }
            else if (PolicySearchGridRequestDto.type == "forendorsement")
            {
                filterPolicy = filterPolicy.And(a => a.BordxId != Guid.Empty && a.BordxId != null);
            }
            else if (PolicySearchGridRequestDto.type == "forpolicydealerreg")
            {
                filterPolicy = filterPolicy.And(a => a.DealerPolicy);
            }
            else if (PolicySearchGridRequestDto.type == "forendorsementapproval")
            {
                IEnumerable<PolicyTransaction> policyTransaction = session.Query<PolicyTransaction>()
                    .Where(a => a.IsRecordActive == true && a.IsApproved == false && a.IsRejected == false && a.TransactionTypeId == GetTransactionTypeId("Endorsement"));
                filterPolicy = filterPolicy.And(a => policyTransaction.Any(b => b.PolicyId == a.Id));
            }
            else if (PolicySearchGridRequestDto.type == "forcancellation")
            {
                filterPolicy = filterPolicy.And(a => a.IsApproved == true && a.IsPolicyCanceled == false);
            }
            else if (PolicySearchGridRequestDto.type == "forcancellationapproval")
            {
                IEnumerable<PolicyTransaction> policyTransaction = session.Query<PolicyTransaction>()
                    .Where(a => a.IsRecordActive == true && a.IsApproved == false && a.IsRejected == false && a.TransactionTypeId == GetTransactionTypeId("Cancellation"));
                filterPolicy = filterPolicy.And(a => policyTransaction.Any(b => b.PolicyId == a.Id));
            }
            else if (PolicySearchGridRequestDto.type == "forpolicytransfer")
            {
                filterPolicy = filterPolicy.And(a => a.BordxId != Guid.Empty && a.BordxId != null && a.IsPolicyCanceled == false && a.IsPolicyRenewed == false);
            }
            else if (PolicySearchGridRequestDto.type == "forpolicyrenewal")
            {
                filterPolicy = filterPolicy.And(a => a.BordxId != Guid.Empty && a.BordxId != null && a.IsPolicyCanceled == false && a.IsPolicyRenewed == false);
            }
            else if (PolicySearchGridRequestDto.type == "forclaimsub")
            {
                filterPolicy = filterPolicy.And(a => a.IsPolicyCanceled == false && a.IsApproved == true && a.IsPolicyRenewed == false);
            }

            if (IsGuid(PolicySearchGridRequestDto.policySearchGridSearchCriterias.commodityTypeId.ToString()))
            {
                filterPolicy = filterPolicy.And(a => a.CommodityTypeId == PolicySearchGridRequestDto.policySearchGridSearchCriterias.commodityTypeId);
            }
            if (!String.IsNullOrEmpty(PolicySearchGridRequestDto.policySearchGridSearchCriterias.policyNo))
            {
                filterPolicy = filterPolicy.And(a => a.PolicyNo.ToLower().Contains(PolicySearchGridRequestDto.policySearchGridSearchCriterias.policyNo.ToLower()));
            }
            if (!String.IsNullOrEmpty(PolicySearchGridRequestDto.policySearchGridSearchCriterias.policyStartDate.ToString())
                && PolicySearchGridRequestDto.policySearchGridSearchCriterias.policyStartDate != DateTime.MinValue)
            {
                filterPolicy = filterPolicy.And(a => a.PolicyStartDate >= PolicySearchGridRequestDto.policySearchGridSearchCriterias.policyStartDate);
            }
            if (!String.IsNullOrEmpty(PolicySearchGridRequestDto.policySearchGridSearchCriterias.policyEndDate.ToString())
                && PolicySearchGridRequestDto.policySearchGridSearchCriterias.policyEndDate != DateTime.MinValue)
            {
                filterPolicy = filterPolicy.And(a => a.PolicyEndDate <= PolicySearchGridRequestDto.policySearchGridSearchCriterias.policyEndDate);
            }

            //userid filtering
            Guid requestedUserId = PolicySearchGridRequestDto.userId;
            if (requestedUserId == Guid.Empty)
            {
                return new object();
            }

            SystemUser sysUser = session.Query<SystemUser>().Where(a => a.LoginMapId == requestedUserId).FirstOrDefault();
            if (sysUser == null)
            {
                return new object();
            }

            UserType userType = session.Query<UserType>().Where(a => a.Id == sysUser.UserTypeId).FirstOrDefault();
            if (userType == null)
            {
                return new object();
            }

            if (userType.Code.ToUpper() == "IU")
            {
                if (PolicySearchGridRequestDto.type != "forclaimsub")
                {
                    IEnumerable<UserBranch> userBranches = session.Query<UserBranch>().Where(a => a.InternalUserId == requestedUserId);
                    filterPolicy = filterPolicy.And(a => userBranches.Any(b => b.TPABranchId == a.TPABranchId));
                }


            }
            else if (userType.Code.ToUpper() == "DU")
            {
                IEnumerable<DealerStaff> dealerMappings = session.Query<DealerStaff>().Where(a => a.UserId == requestedUserId);
                filterPolicy = filterPolicy.And(a => dealerMappings.Any(b => b.DealerId == a.DealerId) && a.DealerPolicy == true);
            }
            //else if (userType.Code.ToUpper() == "DU")
            //{
            //    IEnumerable<DealerStaff> dealerMappings = session.Query<DealerStaff>().Where(a => a.UserId == requestedUserId);
            //    filterPolicy = filterPolicy.And(a => dealerMappings.Any(b => b.DealerId == a.DealerId) && a.DealerPolicy == true);
            //}
            //else
            //{
            //    //yet to do. if some user is a customer of reinsurer user
            //    return new object();
            //}

            var policyGridDetails = session.Query<Policy>().Where(filterPolicy)
                .Join(session.Query<Customer>(), o => o.CustomerId, p => p.Id, (o, p) => new { o, p })
                .Join(session.Query<CommodityType>(), r => r.o.CommodityTypeId, s => s.CommodityTypeId, (r, s) => new { r, s });

            IQueryable<VehicleDetails> vehicle;
            IQueryable<BrownAndWhiteDetails> BnW;
            IQueryable<VehiclePolicy> vehiclePolicy;
            IQueryable<BAndWPolicy> BnWPolicy;
            IQueryable<OtherItemDetails> other;
            IQueryable<OtherItemPolicy> otherPolicy;

            if (!String.IsNullOrEmpty(PolicySearchGridRequestDto.policySearchGridSearchCriterias.serialNo))
            {
                vehicle = session.Query<VehicleDetails>().Where(a => a.VINNo.ToLower().Contains(PolicySearchGridRequestDto.policySearchGridSearchCriterias.serialNo.ToLower()));
                BnW = session.Query<BrownAndWhiteDetails>().Where(a => a.SerialNo.ToLower().Contains(PolicySearchGridRequestDto.policySearchGridSearchCriterias.serialNo.ToLower()));
                other = session.Query<OtherItemDetails>().Where(a => a.SerialNo.ToLower().Contains(PolicySearchGridRequestDto.policySearchGridSearchCriterias.serialNo.ToLower()));
                vehiclePolicy = session.Query<VehiclePolicy>().Where(b => vehicle.Any(c => c.Id == b.VehicleId));
                BnWPolicy = session.Query<BAndWPolicy>().Where(b => BnW.Any(c => c.Id == b.Id));
                otherPolicy = session.Query<OtherItemPolicy>().Where(b => other.Any(c => c.Id == b.OtherItemId));
                policyGridDetails = policyGridDetails.Where(d => vehiclePolicy.Any(e => e.PolicyId == d.r.o.Id) || BnWPolicy.Any(f => f.PolicyId == d.r.o.Id) || otherPolicy.Any(f => f.PolicyId == d.r.o.Id));

            }
            IQueryable<Customer> customers;
            List<Customer> customerList = new List<Customer>();
            if (!String.IsNullOrEmpty(PolicySearchGridRequestDto.policySearchGridSearchCriterias.mobileNo))
            {
                customers = session.Query<Customer>().Where(a => a.MobileNo.Contains(PolicySearchGridRequestDto.policySearchGridSearchCriterias.mobileNo));
                policyGridDetails = policyGridDetails.Where(b => customers.Any(c => c.Id == b.r.o.CustomerId));

            }

           // IEnumerable<CommodityType> commodities = session.Query<CommodityType>();
            // IEnumerable<Customer> customerData = session.Query<Customer>();

            long TotalRecords = policyGridDetails.Count();
            var policyGridDetailsFilterd = policyGridDetails
            .OrderByDescending(a => a.r.o.EntryDateTime)
            .Skip((PolicySearchGridRequestDto.paginationOptionsPolicySearchGrid.pageNumber - 1) * (PolicySearchGridRequestDto.paginationOptionsPolicySearchGrid.pageSize))
            .Take(PolicySearchGridRequestDto.paginationOptionsPolicySearchGrid.pageSize)
            //.Join(commodities, m => m.CommodityTypeId, n => n.CommodityTypeId, (m, n) => new { m, n })
            //.Join(customerData, o => o.m.CustomerId, p => p.Id, (o, p) => new { o, p })
            .Select(a => new
            {
                Id = a.r.o.PolicyBundleId,
                CommodityType = a.s.CommodityTypeDescription,
                PolicyNo = a.r.o.PolicyNo,
                SerialNo = getSerialNumberByPolicyAndComodityCode(a.r.o.Id, a.s.CommodityCode),
                MobileNo = a.r.p.MobileNo,
                PolicySoldDate = a.r.o.PolicySoldDate.ToString("dd-MMM-yyyy"),
                //UniqueRe = a.o.m.UniqueRef
            }).ToArray();
            var response = new CommonGridResponseDto()
            {
                totalRecords = TotalRecords,
                data = policyGridDetailsFilterd
            };
            return new JavaScriptSerializer().Serialize(response);
        }


        internal object GetPloiciesForSearchGrid(PolicySearchGridRequestDto PolicySearchGridRequestDto)
        {

            logger.Trace("Policy Approval Grid Data Load Request came to Entity Manager");
            ISession session = EntitySessionManager.GetSession();
            //userid filtering
            Guid requestedUserId = PolicySearchGridRequestDto.userId;
            if (requestedUserId == Guid.Empty)
            {
                return new object();
            }

            SystemUser sysUser = session.Query<SystemUser>().Where(a => a.LoginMapId == requestedUserId).FirstOrDefault();
            if (sysUser == null)
            {
                return new object();
            }

            UserType userType = session.Query<UserType>().Where(a => a.Id == sysUser.UserTypeId).FirstOrDefault();
            if (userType == null)
            {
                return new object();
            }

            if(PolicySearchGridRequestDto.policySearchGridSearchCriterias.policyStartDate != DateTime.MinValue)
            {
                PolicySearchGridRequestDto.policySearchGridSearchCriterias.policyStartDate = PolicySearchGridRequestDto.policySearchGridSearchCriterias.policyStartDate.AddDays(-1);
            }
            if (PolicySearchGridRequestDto.policySearchGridSearchCriterias.policyEndDate != DateTime.MinValue)
            {
                PolicySearchGridRequestDto.policySearchGridSearchCriterias.policyEndDate = PolicySearchGridRequestDto.policySearchGridSearchCriterias.policyEndDate.AddDays(-1);
            }

            if (String.IsNullOrEmpty(PolicySearchGridRequestDto.policySearchGridSearchCriterias.policyStartDate.ToString())
                            || PolicySearchGridRequestDto.policySearchGridSearchCriterias.policyStartDate == DateTime.MinValue)
            {
                PolicySearchGridRequestDto.policySearchGridSearchCriterias.policyStartDate = DateTime.Parse("1753-01-02 00:00:00.000");
            }
            if (String.IsNullOrEmpty(PolicySearchGridRequestDto.policySearchGridSearchCriterias.policyEndDate.ToString())
                || PolicySearchGridRequestDto.policySearchGridSearchCriterias.policyEndDate == DateTime.MinValue)
            {
                PolicySearchGridRequestDto.policySearchGridSearchCriterias.policyEndDate = DateTime.MaxValue;
            }
            logger.Trace("Policy Approval Grid Data Execute Query");
            List<PolicySearchResponseDto> policyGridDetailsFilterd = session.CreateSQLQuery("exec ApprovePolicyGridDataLoader :type,:commodityTypeId,:policyNo,:policyStartDate,:policyEndDate,:mobileNo,:serialNo,:userId,:skip,:take ")
                       .SetString("type", PolicySearchGridRequestDto.type)
                       .SetGuid("commodityTypeId", PolicySearchGridRequestDto.policySearchGridSearchCriterias.commodityTypeId)
                       .SetString("policyNo", PolicySearchGridRequestDto.policySearchGridSearchCriterias.policyNo)
                       .SetDateTime("policyStartDate", PolicySearchGridRequestDto.policySearchGridSearchCriterias.policyStartDate)
                       .SetDateTime("policyEndDate", PolicySearchGridRequestDto.policySearchGridSearchCriterias.policyEndDate)
                       .SetString("mobileNo", PolicySearchGridRequestDto.policySearchGridSearchCriterias.mobileNo)
                       .SetString("serialNo", PolicySearchGridRequestDto.policySearchGridSearchCriterias.serialNo)
                       .SetGuid("userId", PolicySearchGridRequestDto.userId)
                       .SetInt32("skip", (PolicySearchGridRequestDto.paginationOptionsPolicySearchGrid.pageNumber - 1) * (PolicySearchGridRequestDto.paginationOptionsPolicySearchGrid.pageSize))
                       .SetInt32("take", PolicySearchGridRequestDto.paginationOptionsPolicySearchGrid.pageSize)
                        .SetResultTransformer(Transformers.AliasToBean<PolicySearchResponseDto>()).List<PolicySearchResponseDto>().ToList();

            int res =  session.CreateSQLQuery("exec ApprovePolicyGridDataLoaderRowCount :type,:commodityTypeId,:policyNo,:policyStartDate,:policyEndDate,:mobileNo,:serialNo,:userId ")
                    .SetString("type", PolicySearchGridRequestDto.type)
                    .SetGuid("commodityTypeId", PolicySearchGridRequestDto.policySearchGridSearchCriterias.commodityTypeId)
                    .SetString("policyNo", PolicySearchGridRequestDto.policySearchGridSearchCriterias.policyNo)
                    .SetDateTime("policyStartDate", PolicySearchGridRequestDto.policySearchGridSearchCriterias.policyStartDate)
                    .SetDateTime("policyEndDate", PolicySearchGridRequestDto.policySearchGridSearchCriterias.policyEndDate)
                    .SetString("mobileNo", PolicySearchGridRequestDto.policySearchGridSearchCriterias.mobileNo)
                    .SetString("serialNo", PolicySearchGridRequestDto.policySearchGridSearchCriterias.serialNo)
                    .SetGuid("userId", PolicySearchGridRequestDto.userId)
                    .UniqueResult<int>();
            logger.Trace("Policy Approval Grid Data Complete Execute Query ");
            var totRecord = long.Parse(res.ToString());
            var response = new CommonGridResponseDto()
            {
                totalRecords = totRecord,
                data = policyGridDetailsFilterd
            };
            logger.Trace("Policy Approval Grid Data Returning");
            return new JavaScriptSerializer().Serialize(response);
        }
        private object getMobileNumberByCustomerId(Guid CustomerId)
        {
            ISession session = EntitySessionManager.GetSession();
            object result= "N/A";
            try
            {
                result = session.Query<Customer>().Where(a => a.Id == CustomerId).FirstOrDefault().MobileNo;
            }
            catch (Exception ex) {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return result;
        }

        private object getSerialNumberByPolicyId(Guid PolicyId)
        {
            ISession session = EntitySessionManager.GetSession();

            Policy policy = session.Query<Policy>().Where(a => a.Id == PolicyId).FirstOrDefault();
            if (policy == null)
            {
                return "Not Found";
            }

            CommodityType commodity = session.Query<CommodityType>().Where(a => a.CommodityTypeId == policy.CommodityTypeId)
                .FirstOrDefault();
            if (commodity == null)
            {
                return "Not Found";
            }

            if (commodity.CommodityCode == "A")
            {
                Guid vehicleId = Guid.Empty;
                if (session.Query<VehiclePolicy>().Where(a => a.PolicyId == PolicyId).FirstOrDefault() != null)
                {
                    vehicleId = session.Query<VehiclePolicy>().Where(a => a.PolicyId == PolicyId).FirstOrDefault().VehicleId;
                }
                else
                {
                    return "VIN Not found";
                }
                if (vehicleId != Guid.Empty &&
                    session.Query<VehicleDetails>().Where(a => a.Id == vehicleId).Count() > 0)
                {
                    return session.Query<VehicleDetails>().Where(a => a.Id == vehicleId).FirstOrDefault().VINNo;
                }
                else
                {
                    return "VIN Not found";
                }
            }
            else if (commodity.CommodityCode == "B")
            {
                Guid vehicleId = Guid.Empty;
                if (session.Query<VehiclePolicy>().Where(a => a.PolicyId == PolicyId).FirstOrDefault() != null)
                {
                    vehicleId = session.Query<VehiclePolicy>().Where(a => a.PolicyId == PolicyId).FirstOrDefault().VehicleId;
                }
                else
                {
                    return "VIN Not found";
                }
                if (vehicleId != Guid.Empty &&
                    session.Query<VehicleDetails>().Where(a => a.Id == vehicleId).Count() > 0)
                {
                    return session.Query<VehicleDetails>().Where(a => a.Id == vehicleId).FirstOrDefault().VINNo;
                }
                else
                {
                    return "VIN Not found";
                }
            }
            else if (commodity.CommodityCode == "E")
            {
                Guid BnWId = session.Query<BAndWPolicy>().Where(a => a.PolicyId == PolicyId).FirstOrDefault().BAndWId;
                if (BnWId != Guid.Empty)
                {
                    return session.Query<BrownAndWhiteDetails>().Where(a => a.Id == BnWId).FirstOrDefault().SerialNo;
                }
                else
                {
                    return "N/A";
                }
            }
            else if (commodity.CommodityCode == "Y")
            {
                Guid YgId = session.Query<YellowGoodPolicy>().Where(a => a.PolicyId == PolicyId).FirstOrDefault().YellowGoodId;
                if (YgId != Guid.Empty)
                {
                    return session.Query<YellowGoodDetails>().Where(a => a.Id == YgId).FirstOrDefault().SerialNo;
                }
                else
                {
                    return "N/A";
                }
            }
            else if (commodity.CommodityCode == "O")
            {
                Guid OId = session.Query<OtherItemPolicy>().Where(a => a.PolicyId == PolicyId).FirstOrDefault().OtherItemId;
                if (OId != Guid.Empty)
                {
                    return session.Query<OtherItemDetails>().Where(a => a.Id == OId).FirstOrDefault().SerialNo;
                }
                else
                {
                    return "N/A";
                }
            }
            else
            {
                return "N/A";
            }
        }



        private object getSerialNumberByPolicyAndComodityCode(Guid PolicyId , string CommodityCode )
        {

            ISession session = EntitySessionManager.GetSession();
            if (CommodityCode == "A")
            {
                Guid vehicleId = Guid.Empty;
                if (session.Query<VehiclePolicy>().Where(a => a.PolicyId == PolicyId).FirstOrDefault() != null)
                {
                    vehicleId = session.Query<VehiclePolicy>().Where(a => a.PolicyId == PolicyId).FirstOrDefault().VehicleId;
                }
                else
                {
                    return "VIN Not found";
                }
                if (vehicleId != Guid.Empty &&
                    session.Query<VehicleDetails>().Where(a => a.Id == vehicleId).Count() > 0)
                {
                    return session.Query<VehicleDetails>().Where(a => a.Id == vehicleId).FirstOrDefault().VINNo;
                }
                else
                {
                    return "VIN Not found";
                }
            }
            else if (CommodityCode == "B")
            {
                Guid vehicleId = Guid.Empty;
                if (session.Query<VehiclePolicy>().Where(a => a.PolicyId == PolicyId).FirstOrDefault() != null)
                {
                    vehicleId = session.Query<VehiclePolicy>().Where(a => a.PolicyId == PolicyId).FirstOrDefault().VehicleId;
                }
                else
                {
                    return "VIN Not found";
                }
                if (vehicleId != Guid.Empty &&
                    session.Query<VehicleDetails>().Where(a => a.Id == vehicleId).Count() > 0)
                {
                    return session.Query<VehicleDetails>().Where(a => a.Id == vehicleId).FirstOrDefault().VINNo;
                }
                else
                {
                    return "VIN Not found";
                }
            }
            else if (CommodityCode == "E")
            {
                Guid BnWId = session.Query<BAndWPolicy>().Where(a => a.PolicyId == PolicyId).FirstOrDefault().BAndWId;
                if (BnWId != Guid.Empty)
                {
                    return session.Query<BrownAndWhiteDetails>().Where(a => a.Id == BnWId).FirstOrDefault().SerialNo;
                }
                else
                {
                    return "N/A";
                }
            }
            else if (CommodityCode == "Y")
            {
                Guid YgId = session.Query<YellowGoodPolicy>().Where(a => a.PolicyId == PolicyId).FirstOrDefault().YellowGoodId;
                if (YgId != Guid.Empty)
                {
                    return session.Query<YellowGoodDetails>().Where(a => a.Id == YgId).FirstOrDefault().SerialNo;
                }
                else
                {
                    return "N/A";
                }
            }
            else if (CommodityCode == "O")
            {
                Guid OId = session.Query<OtherItemPolicy>().Where(a => a.PolicyId == PolicyId).FirstOrDefault().OtherItemId;
                if (OId != Guid.Empty)
                {
                    return session.Query<OtherItemDetails>().Where(a => a.Id == OId).FirstOrDefault().SerialNo;
                }
                else
                {
                    return "N/A";
                }
            }
            else
            {
                return "N/A";
            }
        }

        internal static SerialNumberCheckResponseDto SerialNumberCheck(string SerialNumber, string CommodityCode, Guid LoggedInUserId, Guid DealerId)
        {
            SerialNumberCheckResponseDto SerialNumberCheckResponse = new DataTransfer.Responses.SerialNumberCheckResponseDto();

            try
            {
                ISession session = EntitySessionManager.GetSession();
                #region "Automobile"
                if (CommodityCode == "A")
                {
                    VehicleDetails vehicle = session.Query<VehicleDetails>().Where(a => a.VINNo.ToLower() == SerialNumber.ToLower()).FirstOrDefault();
                    if (vehicle == null)
                    {
                        //this is a new serial
                        SerialNumberCheckResponse = new SerialNumberCheckResponseDto()
                        {
                            IsSerialExist = false,
                            AllowedToApprove = false,
                            IsBordxConfirmed = false,
                            IsPolicyApproved = false,
                            ItemId = Guid.Empty,
                            IsDealerInvalid = false
                        };
                    }
                    else if (vehicle.DealerId != DealerId)
                    {
                        //this is a new serial
                        SerialNumberCheckResponse = new SerialNumberCheckResponseDto()
                        {
                            IsSerialExist = true,
                            AllowedToApprove = false,
                            IsBordxConfirmed = false,
                            IsPolicyApproved = false,
                            ItemId = Guid.Empty,
                            IsDealerInvalid = true
                        };
                    }
                    else
                    {
                        //this is a existing serial
                        IQueryable<VehiclePolicy> VehiclePolicyList = session.Query<VehiclePolicy>().Where(a => a.VehicleId == vehicle.Id);
                        if (VehiclePolicyList.Count() == 0)
                        {
                            //this vehicle dose note have any policy attached
                            SerialNumberCheckResponse = new SerialNumberCheckResponseDto()
                            {
                                IsSerialExist = true,
                                AllowedToApprove = false,
                                IsBordxConfirmed = false,
                                IsPolicyApproved = false,
                                ItemId = vehicle.Id,
                                IsDealerInvalid = false
                            };
                        }
                        else
                        {
                            //checking if any bordx is confiremd against this vehicle
                            IQueryable<Policy> PolicyList = session.Query<Policy>().Where(a => VehiclePolicyList.Any(b => b.PolicyId == a.Id));
                            if (PolicyList.Where(a => a.BordxId != null && a.BordxId != Guid.Empty).Count() > 0)
                            {
                                //there are bordx confirmed against this vehicle
                                SerialNumberCheckResponse = new SerialNumberCheckResponseDto()
                                {
                                    IsSerialExist = true,
                                    AllowedToApprove = false,
                                    IsBordxConfirmed = true,
                                    IsPolicyApproved = true,
                                    ItemId = vehicle.Id,
                                    IsDealerInvalid = false
                                };
                            }
                            else
                            {
                                //check if there is any approved policy for this vehicle
                                if (PolicyList.Where(a => a.IsApproved == true).Count() > 0)
                                {
                                    SerialNumberCheckResponse = new SerialNumberCheckResponseDto()
                                    {
                                        IsSerialExist = true,
                                        AllowedToApprove = new MenuEntityManager().GetMenuAccessByUserId("#/app/policyApproval", LoggedInUserId),//PolicyList.Select(a=>a.PolicyApprovedBy.ToString()).ToList(),
                                        IsBordxConfirmed = false,
                                        IsPolicyApproved = true,
                                        ItemId = vehicle.Id,
                                        IsDealerInvalid = false
                                    };
                                }
                                else
                                {
                                    //no any approved policy for this serial number
                                    SerialNumberCheckResponse = new SerialNumberCheckResponseDto()
                                    {
                                        IsSerialExist = true,
                                        AllowedToApprove = false,
                                        IsBordxConfirmed = false,
                                        IsPolicyApproved = false,
                                        ItemId = vehicle.Id,
                                        IsDealerInvalid = false
                                    };
                                }

                            }

                        }

                    }
                }
                #endregion "Automobile"
                #region "Bank"
                else if (CommodityCode == "B")
                {
                    VehicleDetails vehicle = session.Query<VehicleDetails>().Where(a => a.VINNo.ToLower() == SerialNumber.ToLower()).FirstOrDefault();
                    if (vehicle == null)
                    {
                        //this is a new serial
                        SerialNumberCheckResponse = new SerialNumberCheckResponseDto()
                        {
                            IsSerialExist = false,
                            AllowedToApprove = false,
                            IsBordxConfirmed = false,
                            IsPolicyApproved = false,
                            ItemId = Guid.Empty,
                            IsDealerInvalid = false
                        };
                    }
                    else if (vehicle.DealerId != DealerId)
                    {
                        //this is a new serial
                        SerialNumberCheckResponse = new SerialNumberCheckResponseDto()
                        {
                            IsSerialExist = true,
                            AllowedToApprove = false,
                            IsBordxConfirmed = false,
                            IsPolicyApproved = false,
                            ItemId = Guid.Empty,
                            IsDealerInvalid = true
                        };
                    }
                    else
                    {
                        //this is a existing serial
                        IQueryable<VehiclePolicy> VehiclePolicyList = session.Query<VehiclePolicy>().Where(a => a.VehicleId == vehicle.Id);
                        if (VehiclePolicyList.Count() == 0)
                        {
                            //this vehicle dose note have any policy attached
                            SerialNumberCheckResponse = new SerialNumberCheckResponseDto()
                            {
                                IsSerialExist = true,
                                AllowedToApprove = false,
                                IsBordxConfirmed = false,
                                IsPolicyApproved = false,
                                ItemId = vehicle.Id,
                                IsDealerInvalid = false
                            };
                        }
                        else
                        {
                            //checking if any bordx is confiremd against this vehicle
                            IQueryable<Policy> PolicyList = session.Query<Policy>().Where(a => VehiclePolicyList.Any(b => b.PolicyId == a.Id));
                            if (PolicyList.Where(a => a.BordxId != null && a.BordxId != Guid.Empty).Count() > 0)
                            {
                                //there are bordx confirmed against this vehicle
                                SerialNumberCheckResponse = new SerialNumberCheckResponseDto()
                                {
                                    IsSerialExist = true,
                                    AllowedToApprove = false,
                                    IsBordxConfirmed = true,
                                    IsPolicyApproved = true,
                                    ItemId = vehicle.Id,
                                    IsDealerInvalid = false
                                };
                            }
                            else
                            {
                                //check if there is any approved policy for this vehicle
                                if (PolicyList.Where(a => a.IsApproved == true).Count() > 0)
                                {
                                    SerialNumberCheckResponse = new SerialNumberCheckResponseDto()
                                    {
                                        IsSerialExist = true,
                                        AllowedToApprove = new MenuEntityManager().GetMenuAccessByUserId("#/app/policyApproval", LoggedInUserId),//PolicyList.Select(a=>a.PolicyApprovedBy.ToString()).ToList(),
                                        IsBordxConfirmed = false,
                                        IsPolicyApproved = true,
                                        ItemId = vehicle.Id,
                                        IsDealerInvalid = false
                                    };
                                }
                                else
                                {
                                    //no any approved policy for this serial number
                                    SerialNumberCheckResponse = new SerialNumberCheckResponseDto()
                                    {
                                        IsSerialExist = true,
                                        AllowedToApprove = false,
                                        IsBordxConfirmed = false,
                                        IsPolicyApproved = false,
                                        ItemId = vehicle.Id,
                                        IsDealerInvalid = false
                                    };
                                }

                            }

                        }

                    }
                }
                #endregion
                #region "Electronics"
                else if (CommodityCode == "E")
                {
                    BrownAndWhiteDetails ElectronicItem = session.Query<BrownAndWhiteDetails>().Where(a => a.SerialNo.ToLower() == SerialNumber.ToLower()).FirstOrDefault();
                    if (ElectronicItem == null)
                    {
                        //this is a new serial
                        SerialNumberCheckResponse = new SerialNumberCheckResponseDto()
                        {
                            IsSerialExist = false,
                            AllowedToApprove = false,
                            IsBordxConfirmed = false,
                            IsPolicyApproved = false,
                            ItemId = Guid.Empty,
                            IsDealerInvalid = false
                        };
                    }
                    else if (ElectronicItem.DealerId != DealerId)
                    {
                        //this is a new serial
                        SerialNumberCheckResponse = new SerialNumberCheckResponseDto()
                        {
                            IsSerialExist = true,
                            AllowedToApprove = false,
                            IsBordxConfirmed = false,
                            IsPolicyApproved = false,
                            ItemId = Guid.Empty,
                            IsDealerInvalid = true
                        };
                    }
                    else
                    {
                        IQueryable<BAndWPolicy> ElectronicPolicyList = session.Query<BAndWPolicy>().Where(a => a.BAndWId == ElectronicItem.Id);
                        if (ElectronicPolicyList.Count() == 0)
                        {
                            //no policies for this item
                            SerialNumberCheckResponse = new SerialNumberCheckResponseDto()
                            {
                                IsSerialExist = true,
                                AllowedToApprove = false,
                                IsBordxConfirmed = false,
                                IsPolicyApproved = false,
                                ItemId = ElectronicItem.Id,
                                IsDealerInvalid = false
                            };
                        }
                        else
                        {
                            IQueryable<Policy> PolicyList = session.Query<Policy>().Where(a => ElectronicPolicyList.Any(b => b.PolicyId == a.Id));
                            //checking for bordx confirmed policies
                            if (PolicyList.Where(a => a.BordxId != null && a.BordxId != Guid.Empty).Count() > 0)
                            {
                                //there are bordx confirmed policies
                                SerialNumberCheckResponse = new SerialNumberCheckResponseDto()
                                {
                                    IsSerialExist = true,
                                    AllowedToApprove = false,
                                    IsBordxConfirmed = true,
                                    IsPolicyApproved = true,
                                    ItemId = ElectronicItem.Id,
                                    IsDealerInvalid = false
                                };
                            }
                            else
                            {
                                //checking for approved policies
                                if (PolicyList.Where(a => a.IsApproved == true).Count() > 0)
                                {
                                    SerialNumberCheckResponse = new SerialNumberCheckResponseDto()
                                    {
                                        IsSerialExist = true,
                                        AllowedToApprove = new MenuEntityManager().GetMenuAccessByUserId("#/app/policyApproval", LoggedInUserId),
                                        IsBordxConfirmed = false,
                                        IsPolicyApproved = true,
                                        ItemId = ElectronicItem.Id,
                                        IsDealerInvalid = false
                                    };
                                }
                                else
                                {
                                    //no any approved policy for this serial number
                                    SerialNumberCheckResponse = new SerialNumberCheckResponseDto()
                                    {
                                        IsSerialExist = true,
                                        AllowedToApprove = false,
                                        IsBordxConfirmed = false,
                                        IsPolicyApproved = false,
                                        ItemId = ElectronicItem.Id,
                                        IsDealerInvalid = false
                                    };
                                }
                            }
                        }
                    }
                }
                #endregion "Electronics"
                #region "OtherItems"
                else if (CommodityCode == "O")
                {
                    OtherItemDetails OtherItem = session.Query<OtherItemDetails>()
                        .Where(a => a.SerialNo.ToLower() == SerialNumber.ToLower()).FirstOrDefault();
                    if (OtherItem == null)
                    {
                        //this is a new serial
                        SerialNumberCheckResponse = new SerialNumberCheckResponseDto()
                        {
                            IsSerialExist = false,
                            AllowedToApprove = false,
                            IsBordxConfirmed = false,
                            IsPolicyApproved = false,
                            ItemId = Guid.Empty,
                            IsDealerInvalid = false
                        };
                    }
                    //else if (OtherItem.DealerId != DealerId)
                    //{
                    //    //this is a new serial
                    //    SerialNumberCheckResponse = new SerialNumberCheckResponseDto()
                    //    {
                    //        IsSerialExist = true,
                    //        AllowedToApprove = false,
                    //        IsBordxConfirmed = false,
                    //        IsPolicyApproved = false,
                    //        ItemId = Guid.Empty,
                    //        IsDealerInvalid = true
                    //    };
                    //}
                    else
                    {
                        IQueryable<OtherItemPolicy> OtherPolicyList = session.Query<OtherItemPolicy>()
                            .Where(a => a.OtherItemId == OtherItem.Id);
                        if (OtherPolicyList.Count() == 0)
                        {
                            //no policies for this item
                            SerialNumberCheckResponse = new SerialNumberCheckResponseDto()
                            {
                                IsSerialExist = true,
                                AllowedToApprove = false,
                                IsBordxConfirmed = false,
                                IsPolicyApproved = false,
                                ItemId = OtherItem.Id,
                                IsDealerInvalid = false
                            };
                        }
                        else
                        {
                            IQueryable<Policy> PolicyList = session.Query<Policy>()
                                .Where(a => OtherPolicyList.Any(b => b.PolicyId == a.Id));
                            //checking for bordx confirmed policies
                            if (PolicyList.Where(a => a.BordxId != null && a.BordxId != Guid.Empty).Count() > 0)
                            {
                                //there are bordx confirmed policies
                                SerialNumberCheckResponse = new SerialNumberCheckResponseDto()
                                {
                                    IsSerialExist = true,
                                    AllowedToApprove = false,
                                    IsBordxConfirmed = true,
                                    IsPolicyApproved = true,
                                    ItemId = OtherItem.Id,
                                    IsDealerInvalid = false
                                };
                            }
                            else
                            {
                                //checking for approved policies
                                if (PolicyList.Where(a => a.IsApproved == true).Count() > 0)
                                {
                                    SerialNumberCheckResponse = new SerialNumberCheckResponseDto()
                                    {
                                        IsSerialExist = true,
                                        AllowedToApprove = new MenuEntityManager().GetMenuAccessByUserId("#/app/policyApproval", LoggedInUserId),
                                        IsBordxConfirmed = false,
                                        IsPolicyApproved = true,
                                        ItemId = OtherItem.Id,
                                        IsDealerInvalid = false
                                    };
                                }
                                else
                                {
                                    //no any approved policy for this serial number
                                    SerialNumberCheckResponse = new SerialNumberCheckResponseDto()
                                    {
                                        IsSerialExist = true,
                                        AllowedToApprove = false,
                                        IsBordxConfirmed = false,
                                        IsPolicyApproved = false,
                                        ItemId = OtherItem.Id,
                                        IsDealerInvalid = false
                                    };
                                }
                            }
                        }
                    }
                }
                #endregion "OtherItems"

            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ((ex.InnerException != null) ? ex.InnerException.ToString() : ""));
            }
            return SerialNumberCheckResponse;
        }

        internal EligibilityCheckResponse EligibilityCheck(EligibilityCheckRequest eligibilityCheckRequest)
        {
            var response = new EligibilityCheckResponse()
            {
                isPercentage = false,
                premium = (decimal)0.00,
                status = ""
            };
            ISession session = EntitySessionManager.GetSession();
            Contract contract = session.Query<Contract>().FirstOrDefault(a => a.Id == eligibilityCheckRequest.contractId);
            if (contract == null)
            {
                return response;
            }

            CommodityType commodityType = new CommodityEntityManager().GetCommodityById(contract.CommodityTypeId);
            if (commodityType == null)
            {
                return response;
            }

            var commodityTypeCommodityCode = commodityType.CommodityCode;

            List<Eligibility> eligibilityList = session.Query<Eligibility>()
                .Where(a => a.ContractId == eligibilityCheckRequest.contractId).ToList();

            bool isMandatoryEligibilitySatisfied = false;
            foreach (Eligibility eligibility in eligibilityList)
            {
                double usedMonths = (double)(eligibilityCheckRequest.policySoldDate - eligibilityCheckRequest.itemPurchesedDate).Days / 30;
                if (commodityTypeCommodityCode == "A")
                {
                    if (eligibility.MileageFrom <= eligibilityCheckRequest.usedAmount &&
                        eligibility.MileageTo >= eligibilityCheckRequest.usedAmount &&
                        eligibility.AgeFrom <= usedMonths && eligibility.AgeTo >= usedMonths)
                    {
                        if (eligibility.IsPercentage)
                        {
                            response.premium = eligibility.Premium;
                            response.isPercentage = true;
                        }
                        else
                        {
                            response.premium = eligibility.Premium *
                                                          contract.ConversionRate;
                            response.isPercentage = false;
                        }
                        if (eligibility.PlusMinus.ToLower() != "plus")
                        {
                            response.premium = response.premium * (-1);
                        }

                        if (eligibility.isMandatory)
                        {
                            isMandatoryEligibilitySatisfied = true;
                        }
                    }
                }
                else if (commodityTypeCommodityCode == "B")
                {
                    if (eligibility.MileageFrom <= eligibilityCheckRequest.usedAmount &&
                        eligibility.MileageTo >= eligibilityCheckRequest.usedAmount &&
                        eligibility.AgeFrom <= usedMonths && eligibility.AgeTo >= usedMonths)
                    {
                        if (eligibility.IsPercentage)
                        {
                            response.premium = eligibility.Premium;
                            response.isPercentage = true;
                        }
                        else
                        {
                            response.premium = eligibility.Premium *
                                                          contract.ConversionRate;
                            response.isPercentage = false;
                        }
                        if (eligibility.PlusMinus.ToLower() != "plus")
                        {
                            response.premium = response.premium * (-1);
                        }

                        if (eligibility.isMandatory)
                        {
                            isMandatoryEligibilitySatisfied = true;
                        }
                    }
                }
                else
                {
                    if (eligibility.MonthsFrom <= usedMonths && eligibility.MonthsTo >= usedMonths)
                    {
                        if (eligibility.IsPercentage)
                        {
                            response.premium = response.premium;
                            response.isPercentage = true;
                        }
                        else
                        {
                            response.premium = eligibility.Premium *
                                                          contract.ConversionRate;
                            response.isPercentage = false;
                        }
                        if (eligibility.PlusMinus.ToLower() != "plus")
                        {
                            response.premium = response.premium * (-1);
                        }

                        if (eligibility.isMandatory)
                        {
                            isMandatoryEligibilitySatisfied = true;
                        }
                    }
                }
            }
            response.status = isMandatoryEligibilitySatisfied ? "YES" : "NO";
            return response;
        }
        #endregion

        #region TransactionType
        public List<PolicyTransactionTypeResponseDto> GetPolicyTransactionTypes()
        {
            ISession session = EntitySessionManager.GetSession();
            return  session.Query<PolicyTransactionType>().Select(pt => new PolicyTransactionTypeResponseDto
            {
                Code = pt.Code,
                Description = pt.Description,
                Id =pt.Id
            }).ToList();
        }
        #endregion

        internal static string TransferPolicy(SavePolicyRequestDto SavePolicyRequest , string UniqueDbName)
        {
            String Response = "Invalid customer selection";
            try
            {
                if (SavePolicyRequest.policyDetails.customer == null)
                {
                    return Response;
                }

                //avoid datetime errors
                if (SavePolicyRequest.policyDetails.customer.idIssueDate == null ||
                    SavePolicyRequest.policyDetails.customer.idIssueDate < SqlDateTime.MinValue.Value)
                {
                    SavePolicyRequest.policyDetails.customer.idIssueDate = SqlDateTime.MinValue.Value;
                }

                if (SavePolicyRequest.policyDetails.customer.dateOfBirth == null ||
                    SavePolicyRequest.policyDetails.customer.dateOfBirth < SqlDateTime.MinValue.Value)
                {
                    SavePolicyRequest.policyDetails.customer.dateOfBirth = SqlDateTime.MinValue.Value;
                }

                ISession session = EntitySessionManager.GetSession();
                CustomerEntityManager customerEM = new CustomerEntityManager();

                PolicyBundle policyBundle = session.Query<PolicyBundle>()
                    .Where(a => a.Id == SavePolicyRequest.policyDetails.policy.id).FirstOrDefault();
                if (policyBundle == null)
                {
                    return Response;
                }

                IEnumerable<Policy> policies = session.Query<Policy>()
                    .Where(a => a.PolicyBundleId == policyBundle.Id);
                if (policies == null || policies.Count() == 0)
                {
                    return Response;
                }

                if (policyBundle.CustomerId == SavePolicyRequest.policyDetails.customer.customerId)
                {
                    Response = "Please select a different customer to transfer policy";
                    return Response;
                }

                bool customerStatus = false;
                Guid customerId = Guid.Empty;
                #region "customer"
                if (SavePolicyRequest.policyDetails.customer.customerId == Guid.Empty)
                {
                    //new customer
                    CustomerRequestDto customer = DBDTOTransformer.Instance.PolicyCustomerToCustomerEntity(SavePolicyRequest.policyDetails.customer);
                    customer.Id = Guid.NewGuid().ToString();
                    customerStatus = customerEM.AddCustomer(customer);
                    if (customerStatus) {
                        /* remove cache */
                        ICache cache = CacheFactory.GetCache();
                        cache.Remove(UniqueDbName + "_Customers");
                    }
                    customerId = Guid.Parse(customer.Id);
                }
                else
                {
                    customerStatus = customerEM.UpdateCustomerInPolicy(SavePolicyRequest.policyDetails.customer);
                    customerStatus = true;
                    customerId = SavePolicyRequest.policyDetails.customer.customerId;
                }
                #endregion "customer"
                if (!customerStatus)
                {
                    Response = "Error occured in customer add/update";
                    return Response;
                }

                Response = "Error occured in policy transaction";
                #region "policy bundle transaction"
                //saving policy bundle transaction
                Guid PolicyBundleTransactionId = Guid.NewGuid();

                PolicyBundleTransaction bundleTransaction = new PolicyBundleTransaction()
                {
                    Comment = SavePolicyRequest.policyDetails.payment.comment,
                    CommodityTypeId = SavePolicyRequest.policyDetails.product.commodityTypeId,
                    ContractId = Guid.Empty,
                    CoverTypeId = Guid.Empty,
                    CustomerId = customerId,
                    CustomerPayment = SavePolicyRequest.policyDetails.payment.customerPayment,
                    CustomerPaymentCurrencyTypeId = SavePolicyRequest.policyDetails.product.customerPaymentCurrencyTypeId,
                    DealerId = SavePolicyRequest.policyDetails.product.dealerId,
                    DealerLocationId = SavePolicyRequest.policyDetails.product.dealerLocationId,
                    DealerPayment = SavePolicyRequest.policyDetails.payment.dealerPayment,
                    DealerPaymentCurrencyTypeId = SavePolicyRequest.policyDetails.product.dealerPaymentCurrencyTypeId,
                    DealerPolicy = SavePolicyRequest.policyDetails.policy.dealerPolicy,
                    Discount = SavePolicyRequest.policyDetails.payment.discount,
                    EntryDateTime = DateTime.UtcNow,
                    EntryUser = SavePolicyRequest.policyDetails.requestedUser,
                    ExtensionTypeId = Guid.Empty,
                    HrsUsedAtPolicySale = SavePolicyRequest.policyDetails.policy.hrsUsedAtPolicySale,
                    Id = PolicyBundleTransactionId,
                    IsApproved = true,
                    IsPartialPayment = SavePolicyRequest.policyDetails.payment.isPartialPayment,
                    IsPolicyCanceled = false,
                    IsPreWarrantyCheck = false,
                    IsSpecialDeal = SavePolicyRequest.policyDetails.payment.isSpecialDeal,
                    PaymentModeId = SavePolicyRequest.policyDetails.payment.paymentModeId,
                    PolicyBundleId = SavePolicyRequest.policyDetails.policy.id,
                    PolicyNo = "",
                    PolicySoldDate = SavePolicyRequest.policyDetails.policy.policySoldDate,
                    Premium = SavePolicyRequest.policyDetails.policy.premium,
                    PremiumCurrencyTypeId = Guid.Empty,
                    ProductId = SavePolicyRequest.policyDetails.product.productId,
                    RefNo = SavePolicyRequest.policyDetails.payment.refNo,
                    SalesPersonId = SavePolicyRequest.policyDetails.policy.salesPersonId,
                    TransactionTypeId = GetTransactionTypeId("Transfer"),
                    Type = "Transfer",
                    MWStartDate = SavePolicyRequest.policyDetails.product.MWStartDate
                };
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Save(bundleTransaction, bundleTransaction.Id);
                    transaction.Commit();
                }

                #endregion "policy bundle transaction"

                #region "policy transaction"
                foreach (ProductContract_ productContract in SavePolicyRequest.policyDetails.policy.productContracts)
                {
                    PolicyTransaction policyTransaction = new PolicyTransaction()
                    {
                        AddnSerialNo = SavePolicyRequest.policyDetails.product.additionalSerial,
                        Address1 = SavePolicyRequest.policyDetails.customer.address1,
                        Address2 = SavePolicyRequest.policyDetails.customer.address2,
                        Address3 = SavePolicyRequest.policyDetails.customer.address3,
                        Address4 = SavePolicyRequest.policyDetails.customer.address4,
                        AspirationId = SavePolicyRequest.policyDetails.product.aspirationTypeId,
                        BAndWId = SavePolicyRequest.policyDetails.product.id,
                        BodyTypeId = SavePolicyRequest.policyDetails.product.bodyTypeId,
                        BusinessAddress1 = SavePolicyRequest.policyDetails.customer.businessAddress1,
                        BusinessAddress2 = SavePolicyRequest.policyDetails.customer.businessAddress2,
                        BusinessAddress3 = SavePolicyRequest.policyDetails.customer.businessAddress3,
                        BusinessAddress4 = SavePolicyRequest.policyDetails.customer.businessAddress4,
                        BusinessName = SavePolicyRequest.policyDetails.customer.businessName,
                        BusinessTelNo = SavePolicyRequest.policyDetails.customer.businessTelNo,
                        CancelationComment = "",
                        CategoryId = SavePolicyRequest.policyDetails.product.categoryId,
                        CityId = SavePolicyRequest.policyDetails.customer.cityId,
                        Comment = SavePolicyRequest.policyDetails.payment.comment,
                        CommodityTypeId = SavePolicyRequest.policyDetails.product.commodityTypeId,
                        CommodityUsageTypeId = SavePolicyRequest.policyDetails.product.commodityUsageTypeId,
                        ContractId = productContract.ContractId,
                        CountryId = SavePolicyRequest.policyDetails.customer.countryId,
                        CoverTypeId = productContract.CoverTypeId,
                        CustomerId = customerId,
                        CustomerPayment = SavePolicyRequest.policyDetails.payment.customerPayment,
                        CustomerPaymentCurrencyTypeId = SavePolicyRequest.policyDetails.product.customerPaymentCurrencyTypeId,
                        CustomerTypeId = SavePolicyRequest.policyDetails.customer.customerTypeId,
                        CylinderCountId = SavePolicyRequest.policyDetails.product.cylinderCountId,
                        RegistrationDate = SavePolicyRequest.policyDetails.product.registrationDate,
                        DateOfBirth = SavePolicyRequest.policyDetails.customer.dateOfBirth,
                        DealerId = SavePolicyRequest.policyDetails.product.dealerId,
                        DealerLocationId = SavePolicyRequest.policyDetails.product.dealerLocationId,
                        DealerPayment = SavePolicyRequest.policyDetails.payment.dealerPayment,
                        DealerPaymentCurrencyTypeId = SavePolicyRequest.policyDetails.product.dealerPaymentCurrencyTypeId,
                        DealerPolicy = SavePolicyRequest.policyDetails.policy.dealerPolicy,
                        DealerPrice = SavePolicyRequest.policyDetails.product.dealerPrice,
                        Discount = SavePolicyRequest.policyDetails.payment.discount,
                        DLIssueDate = SavePolicyRequest.policyDetails.customer.idIssueDate,
                        DriveTypeId = Guid.Empty,
                        Email = SavePolicyRequest.policyDetails.customer.email,
                        EngineCapacityId = SavePolicyRequest.policyDetails.product.engineCapacityId,
                        ExtensionTypeId = productContract.ExtensionTypeId,
                        FirstName = SavePolicyRequest.policyDetails.customer.firstName,
                        FuelTypeId = SavePolicyRequest.policyDetails.product.fuelTypeId,
                        Gender = SavePolicyRequest.policyDetails.customer.gender,
                        HrsUsedAtPolicySale = SavePolicyRequest.policyDetails.policy.hrsUsedAtPolicySale,
                        Id = Guid.NewGuid(),
                        PolicyBundleTransactionId = PolicyBundleTransactionId,
                        IDNo = SavePolicyRequest.policyDetails.customer.idNo,
                        IDTypeId = SavePolicyRequest.policyDetails.customer.idTypeId,
                        InvoiceNo = SavePolicyRequest.policyDetails.product.invoiceNo,
                        IsActive = true,
                        IsApproved = true,
                        IsRejected = false,
                        ApprovedRejectedBy = Guid.Empty,
                        IsPartialPayment = SavePolicyRequest.policyDetails.payment.isPartialPayment,
                        IsPreWarrantyCheck = false,
                        IsRecordActive = true,
                        IsSpecialDeal = SavePolicyRequest.policyDetails.payment.isSpecialDeal,
                        ItemPrice = SavePolicyRequest.policyDetails.product.itemPrice,
                        ItemPurchasedDate = SavePolicyRequest.policyDetails.product.itemPurchasedDate,
                        ItemStatusId = SavePolicyRequest.policyDetails.product.itemStatusId,
                        LastName = SavePolicyRequest.policyDetails.customer.lastName,
                        MakeId = SavePolicyRequest.policyDetails.product.makeId,
                        MobileNo = SavePolicyRequest.policyDetails.customer.mobileNo,
                        ModelCode = "",
                        ModelId = SavePolicyRequest.policyDetails.product.modelId,
                        ModelYear = SavePolicyRequest.policyDetails.product.modelYear,
                        Password = "",
                        PaymentModeId = SavePolicyRequest.policyDetails.payment.paymentModeId,
                        PlateNo = SavePolicyRequest.policyDetails.product.additionalSerial,
                        PolicyId = productContract.Id,
                        PolicyNo = productContract.PolicyNo,
                        PolicySoldDate = SavePolicyRequest.policyDetails.policy.policySoldDate,
                        PolicyEndDate = SqlDateTime.MinValue.Value,
                        PolicyStartDate = SqlDateTime.MinValue.Value,
                        ModifiedDate = DateTime.UtcNow,
                        NationalityId = SavePolicyRequest.policyDetails.customer.nationalityId,
                        OtherTelNo = SavePolicyRequest.policyDetails.customer.otherTelNo,
                        Premium = productContract.Premium,
                        PremiumCurrencyTypeId = productContract.PremiumCurrencyTypeId,
                        ProductId = SavePolicyRequest.policyDetails.product.productId,
                        RefNo = SavePolicyRequest.policyDetails.payment.refNo,
                        SalesPersonId = SavePolicyRequest.policyDetails.policy.salesPersonId,
                        SerialNo = SavePolicyRequest.policyDetails.product.serialNumber,
                        TransactionTypeId = GetTransactionTypeId("Transfer"),
                        TransferFee = 0,
                        TransmissionId = SavePolicyRequest.policyDetails.product.transmissionTypeId,
                        UsageTypeId = SavePolicyRequest.policyDetails.customer.usageTypeId,
                        Variant = SavePolicyRequest.policyDetails.product.variantId,
                        VehicleId = SavePolicyRequest.policyDetails.product.productId,
                        VehiclePrice = SavePolicyRequest.policyDetails.product.itemPrice,
                        VINNo = SavePolicyRequest.policyDetails.product.serialNumber,
                        MWStartDate = SavePolicyRequest.policyDetails.product.MWStartDate,
                    };

                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.Save(policyTransaction, policyTransaction.Id);
                        transaction.Commit();
                    }
                }

                #endregion "policy transaction"

                #region "Policy Bundle Information"
                //saving policy bundle history
                PolicyBundleHistory policyBundleHistory = DBDTOTransformer.Instance.PolicyBundleToPolicyBundleHistory(policyBundle, bundleTransaction);
                using (ITransaction transaction = session.BeginTransaction())
                {
                    policyBundleHistory.Type = "Transfer";
                    session.Save(policyBundleHistory, policyBundleHistory.Id);
                    transaction.Commit();
                }
                //update policy bundle
                using (ITransaction transaction = session.BeginTransaction())
                {
                    policyBundle.CustomerId = customerId;
                    session.Update(policyBundle, policyBundle.Id);
                    transaction.Commit();
                }
                #endregion "Policy Bundle Information"

                #region "Policy Information"
                //save policy history
                List<PolicyHistory> policyHistoryList = DBDTOTransformer.Instance.PolicyToPolicyHistory(policies.ToList());
                foreach (PolicyHistory policyHistory in policyHistoryList)
                {
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        policyHistory.TransactionTypeId = policyBundleHistory.TransactionTypeId;
                        policyHistory.ModifiedDate = DateTime.UtcNow;
                        session.Save(policyHistory, policyHistory.Id);
                        transaction.Commit();
                    }
                }
                //update policy
                foreach (Policy policy in policies.ToList())
                {
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        Policy existingPolicy = session.Query<Policy>().Where(a => a.Id == policy.Id).FirstOrDefault();
                        policy.CustomerId = customerId;
                        policy.TransferFee = SavePolicyRequest.policyDetails.transferFee;
                        policy.BordxId = Guid.Empty;
                        policy.BordxNumber = 0;
                        policy.Year = 0;
                        policy.Month = 0;
                        policy.ApprovedDate = existingPolicy.ApprovedDate;
                        session.Update(policy, policy.Id);
                        transaction.Commit();
                    }
                }
                #endregion "Policy Bundle Information"

                #region PolicyAttachment
                try
                {

                    List<PolicyAttachment> attachments = session.Query<PolicyAttachment>()
                        .Where(a => a.PolicyBundleId == policyBundle.Id).ToList();
                    //backup existing Attachments

                    foreach (PolicyAttachment att in attachments)
                    {
                        PolicyAttachmentTransaction attTrans = new PolicyAttachmentTransaction()
                        {
                            Id = Guid.NewGuid(),
                            PolicyBundleIdTransaction = bundleTransaction.Id,
                            UserAttachmentId = att.UserAttachmentId
                        };
                        using (ITransaction transaction = session.BeginTransaction())
                        {
                            session.Save(attTrans, attTrans.Id);
                            transaction.Commit();
                        }
                    }


                    session.Evict(attachments);
                    //remove existing Attachments
                    foreach (PolicyAttachment att in attachments)
                    {
                        using (ITransaction transaction = session.BeginTransaction())
                        {
                            session.Delete(att);
                            transaction.Commit();
                        }

                    }


                    //add new Attachments
                    foreach (Guid AttachmentId in SavePolicyRequest.policyDetails.policyDocIds)
                    {
                        PolicyAttachment policyAttachment = new PolicyAttachment()
                        {
                            Id = Guid.NewGuid(),
                            PolicyBundleId = policyBundle.Id,
                            UserAttachmentId = AttachmentId
                        };

                        using (ITransaction transaction = session.BeginTransaction())
                        {
                            session.Save(policyAttachment, policyAttachment.Id);
                            transaction.Commit();
                        }

                    }
                }
                catch (Exception ex)
                {
                    logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ((ex.InnerException != null) ? ex.InnerException.ToString() : ""));
                    return "Policy attachment updating failed";
                }

                #endregion


                Response = "success";
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ((ex.InnerException != null) ? ex.InnerException.ToString() : ""));
            }
            return Response;
        }

        internal static object GetPolicyTransferHistoryById(Guid policyBundleId)
        {
            object Response = null;
            try
            {
                ISession session = EntitySessionManager.GetSession();
                CommonEntityManager cem = new CommonEntityManager();
                Guid TransactionTypeId = GetTransactionTypeId("Transfer");
                List<PolicyHistory> policyHistoryList = session.Query<PolicyHistory>()
                    .Where(a => a.TransactionTypeId == TransactionTypeId && a.PolicyBundleId == policyBundleId).ToList();

                if (policyHistoryList == null || policyHistoryList.Count() == 0)
                {
                    return Response;
                }

                Response = policyHistoryList
                    .OrderBy(a => a.ModifiedDate)
                    .Select(a => new
                    {
                        a.ModifiedDate,
                        Customer = cem.GetCustomerNameById(a.CustomerId),
                        PolicySoldDate = a.PolicySoldDate.ToString("dd-MMM-yyyy"),
                        ContactNumber = cem.GetCustomerMobileNumberById(a.CustomerId)
                    }).ToArray();


            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ((ex.InnerException != null) ? ex.InnerException.ToString() : ""));
            }
            return Response;
        }

        #region "helper"

        private Guid GetTpaBranchIdByDealerLocationId(Guid dealerLocationId)
        {
            Guid Response = Guid.Empty;
            try
            {
                ISession session = EntitySessionManager.GetSession();
                var dealerLocation = session.Query<DealerLocation>().Where(a => a.Id == dealerLocationId).FirstOrDefault();
                if (dealerLocation != null)
                {
                    Response = dealerLocation.TpaBranchId;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ((ex.InnerException != null) ? ex.InnerException.ToString() : ""));
            }
            return Response;
        }
        internal static object GetPoliciesByBordxIdGrid(GetPoliciesByBordxIdRequestDto _GetPoliciesByBordxIdRequestDto)
        {
            ISession session = EntitySessionManager.GetSession();
            if (IsGuid(_GetPoliciesByBordxIdRequestDto.bordxId.ToString()))
            {

                IQueryable<BordxDetails> bordxDetails = session.Query<BordxDetails>()
                    .Where(a => a.BordxId == _GetPoliciesByBordxIdRequestDto.bordxId);
                IQueryable<Policy> PolicyList = session.Query<Policy>()
                    .Where(a => bordxDetails.Any(b => b.PolicyId == a.Id));
                long TotalRecords = PolicyList.Count();
                var policyGridDetailsFilterd = PolicyList.Skip(_GetPoliciesByBordxIdRequestDto.page - 1)
                .Take(_GetPoliciesByBordxIdRequestDto.pageSize)
                .ToList().OrderByDescending(a => a.PolicyNo);
                var policyGridDetails = policyGridDetailsFilterd.Select(a => new
                {
                    a.PolicyNo,
                    Premium = (a.Premium * a.LocalCurrencyConversionRate),
                    PolicySoldDate = a.PolicySoldDate.ToString("dd-MMM-yyyy"),
                    RefNo = GetVINSerialNumnerByPolicyId(a.CommodityTypeId, a.Id),
                    CommodityType = GetCommodityNameById(a.CommodityTypeId),
                    Dealer = GetDealerNameById(a.DealerId)

                }).ToArray();

                var response = new CommonGridResponseDto()
                {
                    totalRecords = TotalRecords,
                    data = policyGridDetails
                };
                return new JavaScriptSerializer().Serialize(response);
            }
            else
            {
                return new JavaScriptSerializer().Serialize("");
            }
        }

        internal static object GetClaimHistorysByPolicyId(Guid policyBundleId)
        {
            ISession session = EntitySessionManager.GetSession();
            if (IsGuid(policyBundleId.ToString()))
            {
                CurrencyEntityManager currencyEm = new CurrencyEntityManager();
                CommonEntityManager cem = new CommonEntityManager();
                IQueryable<Policy> PolicyDetails = session.Query<Policy>()
                    .Where(a => a.PolicyBundleId == policyBundleId);

                long TotalRecords = PolicyDetails.Count();

                var ClaimHistoryDetails = //new object();
                        PolicyDetails
                        .Join(session.Query<Claim>(), a => a.Id, b => b.PolicyId, (a, b) => new { a, b })
                        .Join(session.Query<ClaimStatusCode>(), c => c.b.StatusId, d => d.Id, (c, d) => new { c, d })
                        .Join(session.Query<Policy>(), e => e.c.b.PolicyId, f => f.Id, (e, f) => new { e, f })
                        .Select(i => new
                        {
                            i.e.c.b.ClaimNumber,
                            ApprovedDate = i.e.c.b.ApprovedDate.ToString("dd-MMM-yyyy"),
                            CustomerPaymentCurrencyTypeId = cem.GetCurrencyTypeByIdCode(i.e.c.a.CustomerPaymentCurrencyTypeId),
                            AuthorizedAmount = currencyEm.ConvertFromBaseCurrency(i.e.c.b.AuthorizedAmount, i.e.c.a.PremiumCurrencyTypeId, i.e.c.a.CurrencyPeriodId),
                            i.e.d.Description
                        }).ToArray();


                var response = ClaimHistoryDetails;


                return response;
            }
            else
            {
                return new JavaScriptSerializer().Serialize("");
            }
        }

        public static string GetVINSerialNumnerByPolicyId(Guid commodityTypeId, Guid PolicyId)
        {
            String Response = string.Empty;
            try
            {
                ISession session = EntitySessionManager.GetSession();
                string commodityTypeCode = session.Query<CommodityType>()
                    .FirstOrDefault(a => a.CommodityTypeId == commodityTypeId).CommodityCode;
                if (commodityTypeCode.ToLower() == "a")
                {
                    VehiclePolicy vp = session
                        .Query<VehiclePolicy>().FirstOrDefault(a => a.PolicyId == PolicyId);
                    if (vp == null)
                    {
                        return Response;
                    }

                    VehicleDetails vd = session
                        .Query<VehicleDetails>().FirstOrDefault(a => a.Id == vp.VehicleId);

                    Response = vd.VINNo;

                }
                else if (commodityTypeCode.ToLower() == "b")
                {
                    VehiclePolicy vp = session
                        .Query<VehiclePolicy>().FirstOrDefault(a => a.PolicyId == PolicyId);
                    if (vp == null)
                    {
                        return Response;
                    }

                    VehicleDetails vd = session
                        .Query<VehicleDetails>().FirstOrDefault(a => a.Id == vp.VehicleId);

                    Response = vd.VINNo;

                }
                else if (commodityTypeCode.ToLower() == "e")
                {
                    BAndWPolicy ep = session
                        .Query<BAndWPolicy>().FirstOrDefault(a => a.PolicyId == PolicyId);
                    if (ep == null)
                    {
                        return Response;
                    }

                    BrownAndWhiteDetails bd = session
                        .Query<BrownAndWhiteDetails>().FirstOrDefault(a => a.Id == ep.BAndWId);

                    Response = bd.SerialNo;

                }
                else if (commodityTypeCode.ToLower() == "o")
                {
                    OtherItemPolicy op = session
                        .Query<OtherItemPolicy>().FirstOrDefault(a => a.PolicyId == PolicyId);
                    if (op == null)
                    {
                        return Response;
                    }

                    OtherItemDetails od = session
                        .Query<OtherItemDetails>().FirstOrDefault(a => a.Id == op.OtherItemId);

                    Response = od.SerialNo;

                }
                else if (commodityTypeCode.ToLower() == "y")
                {
                    YellowGoodPolicy yp = session
                        .Query<YellowGoodPolicy>().FirstOrDefault(a => a.PolicyId == PolicyId);
                    if (yp == null)
                    {
                        return Response;
                    }

                    YellowGoodDetails yd = session
                        .Query<YellowGoodDetails>().FirstOrDefault(a => a.Id == yp.YellowGoodId);

                    Response = yd.SerialNo;

                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ((ex.InnerException != null) ? ex.InnerException.ToString() : ""));
            }
            return Response;

        }
        private static object GetDealerNameById(Guid guid)
        {
            ISession session = EntitySessionManager.GetSession();
            return session.Query<Dealer>().Where(a => a.Id == guid).SingleOrDefault().DealerName;
        }

        private static object GetCommodityNameById(Guid guid)
        {
            ISession session = EntitySessionManager.GetSession();
            return session.Query<CommodityType>().Where(a => a.CommodityTypeId == guid).SingleOrDefault().CommodityTypeDescription;
        }
        private static bool IsGuid(string candidate)
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
        #endregion "helper"

        internal static object GetPremiumBreakdown(PremiumBreakdownRequestDto premiumBreakdownRequest)
        {
            PremiumBreakdownResponseDto Response = new PremiumBreakdownResponseDto();
            Response.BreakdownDetails = new List<BreakdownDetails>();
            try
            {

                ISession session = EntitySessionManager.GetSession();
                var TaxBreakdowns = new List<BreakdownValue>();

                foreach (ProductContract_ contract in premiumBreakdownRequest.premiumBreakdownRequest.productContracts)
                {

                    var commonEntityManeger = new CommonEntityManager();
                    Guid commodityTypeId = commonEntityManeger.GetCommodityTypeIdByContractId(contract.ContractId);
                    ContractExtensionPremium contractExtensionPremium =
                        session.Query<ContractExtensionPremium>()
                            .FirstOrDefault(a => a.Id == contract.CoverTypeId);
                    //  response.Currency = commonEntityManeger.GetCurrencyCodeById(contractExtensionPremium.CurrencyId);
                    //gross
                    IList<ContractExtensionsPremiumAddon> contractExtensionsPremiumAddons = session
                        .QueryOver<ContractExtensionsPremiumAddon>()
                        .WhereRestrictionOn(a => a.ContractExtensionPremiumId)
                        .IsIn(new Guid[] { contract.CoverTypeId })
                        .WhereRestrictionOn(c => c.PremiumType).IsIn(new string[] { "GROSS" })
                        .List<ContractExtensionsPremiumAddon>();

                    IList<ContractExtensionsPremiumAddon> contractExtensionsPremiumAddonsNrp = session
                        .QueryOver<ContractExtensionsPremiumAddon>()
                        .WhereRestrictionOn(a => a.ContractExtensionPremiumId)
                        .IsIn(new Guid[] { contract.CoverTypeId })
                        .WhereRestrictionOn(c => c.PremiumType).IsIn(new string[] { "NETT" })
                        .List<ContractExtensionsPremiumAddon>();

                    IList<Guid> mandatoryAddonIds = session.QueryOver<PremiumAddonType>()
                        .Where(a => a.IsApplicableforVariant != true && a.CommodityTypeId == commodityTypeId)
                        .Select(a => a.Id)
                        .List<Guid>();
                    IList<Guid> mandatoryAddonIdsNrp = session.QueryOver<PremiumAddonType>()
                        .Where(a => a.IsApplicableforVariant != true && a.CommodityTypeId == commodityTypeId)
                        .Select(a => a.Id)
                        .List<Guid>();

                    decimal BasicPremium = decimal.Zero,
                        variantPremium = decimal.Zero,
                        BasicPremiumNRP = decimal.Zero,
                        variantPremiumNrp = decimal.Zero,
                        TotalPremium = decimal.Zero,
                        TotalPremiumNRP = decimal.Zero,
                        EligibilityPremium = decimal.Zero;

                    BasicPremium = contractExtensionsPremiumAddons
                        .Where(a => mandatoryAddonIds.Any(c => c == a.PremiumAddonTypeId))
                        .Sum(a => a.Value);
                    BasicPremiumNRP = contractExtensionsPremiumAddonsNrp
                        .Where(a => mandatoryAddonIdsNrp.Any(c => c == a.PremiumAddonTypeId))
                        .Sum(a => a.Value);


                    //variant based premium addons
                    if (IsGuid(premiumBreakdownRequest.premiumBreakdownRequest.variantId.ToString()))
                    {
                        IList<Guid> variantAddonIds = session.QueryOver<VariantPremiumAddon>()
                            .Where(a => a.VariantId == premiumBreakdownRequest.premiumBreakdownRequest.variantId)
                            .Select(a => a.PremiumAddonTypeId).List<Guid>();

                        variantPremium = contractExtensionsPremiumAddons
                            .Where(a => variantAddonIds.Any(c => c == a.PremiumAddonTypeId))
                            .Sum(a => a.Value);


                        variantPremiumNrp = contractExtensionsPremiumAddonsNrp
                            .Where(a => variantAddonIds.Any(c => c == a.PremiumAddonTypeId))
                            .Sum(a => a.Value);

                    }
                    //setting premium if it is based on retail price
                    if (
                        commonEntityManeger.GetPremiumBasedonCodeById(contractExtensionPremium.PremiumBasedOnGross)
                            .ToLower() == "rp")
                    {
                        var calculatedRate = BasicPremium + variantPremium;
                        var calculatedPremiumUsd = (premiumBreakdownRequest.premiumBreakdownRequest.DealerPrice /
                                                    contractExtensionPremium.ConversionRate) * calculatedRate / 100;
                        if (calculatedPremiumUsd < contractExtensionPremium.MinValueGross)
                        {
                            calculatedPremiumUsd = contractExtensionPremium.MinValueGross;
                        }

                        if (calculatedPremiumUsd > contractExtensionPremium.MaxValueGross)
                        {
                            calculatedPremiumUsd = contractExtensionPremium.MaxValueGross;
                        }

                        TotalPremium = calculatedPremiumUsd * contractExtensionPremium.ConversionRate;
                    }
                    else
                    {
                        TotalPremium = (variantPremium + BasicPremium) * contractExtensionPremium.ConversionRate;
                    }

                    if (
                        commonEntityManeger.GetPremiumBasedonCodeById(contractExtensionPremium.PremiumBasedOnNett)
                            .ToLower() == "rp")
                    {
                        var calculatedRate = BasicPremiumNRP + variantPremiumNrp;
                        var calculatedPremiumUsd = (premiumBreakdownRequest.premiumBreakdownRequest.DealerPrice /
                                                    contractExtensionPremium.ConversionRate) * calculatedRate / 100;
                        if (calculatedPremiumUsd < contractExtensionPremium.MinValueNett)
                        {
                            calculatedPremiumUsd = contractExtensionPremium.MinValueNett;
                        }

                        if (calculatedPremiumUsd > contractExtensionPremium.MaxValueNett)
                        {
                            calculatedPremiumUsd = contractExtensionPremium.MaxValueNett;
                        }

                        TotalPremiumNRP = calculatedPremiumUsd * contractExtensionPremium.ConversionRate;
                    }
                    else
                    {
                        TotalPremiumNRP = (variantPremiumNrp + BasicPremiumNRP) *
                                          contractExtensionPremium.ConversionRate;
                    }
                    //eligibility fee
                    IList<Eligibility> eligibilities = session.QueryOver<Eligibility>()
                        .Where(a => a.ContractId == contract.ContractId)
                        .List<Eligibility>();
                    foreach (Eligibility eligibility in eligibilities)
                    {
                        var commodityCode = new CommonEntityManager().GetCommodityTypeUniqueCodeById(commodityTypeId);

                        if (commodityCode == "A")
                        {
                            var usedMonths =
                                (premiumBreakdownRequest.premiumBreakdownRequest.PolicySoldDate -
                                 premiumBreakdownRequest.premiumBreakdownRequest.ItemPurchasedDate).Days / 30;
                            if (eligibility.MileageFrom <= premiumBreakdownRequest.premiumBreakdownRequest.Usage &&
                                eligibility.MileageTo >= premiumBreakdownRequest.premiumBreakdownRequest.Usage &&
                                eligibility.AgeFrom <= usedMonths && eligibility.AgeTo >= usedMonths)
                            {
                                if (eligibility.IsPercentage)
                                {
                                    EligibilityPremium = TotalPremium * eligibility.Premium / 100;
                                }
                                else
                                {
                                    EligibilityPremium = eligibility.Premium *
                                                         contractExtensionPremium.ConversionRate;
                                }
                                if (eligibility.PlusMinus.ToLower() != "plus")
                                {
                                    EligibilityPremium = EligibilityPremium * (-1);
                                }

                                if (eligibility.isMandatory)
                                {
                                }
                            }
                        }
                        else if (commodityCode == "B")
                        {
                            var usedMonths =
                                (premiumBreakdownRequest.premiumBreakdownRequest.PolicySoldDate -
                                 premiumBreakdownRequest.premiumBreakdownRequest.ItemPurchasedDate).Days / 30;
                            if (eligibility.MileageFrom <= premiumBreakdownRequest.premiumBreakdownRequest.Usage &&
                                eligibility.MileageTo >= premiumBreakdownRequest.premiumBreakdownRequest.Usage &&
                                eligibility.AgeFrom <= usedMonths && eligibility.AgeTo >= usedMonths)
                            {
                                if (eligibility.IsPercentage)
                                {
                                    EligibilityPremium = TotalPremium * eligibility.Premium / 100;
                                }
                                else
                                {
                                    EligibilityPremium = eligibility.Premium *
                                                         contractExtensionPremium.ConversionRate;
                                }
                                if (eligibility.PlusMinus.ToLower() != "plus")
                                {
                                    EligibilityPremium = EligibilityPremium * (-1);
                                }

                                if (eligibility.isMandatory)
                                {
                                }
                            }
                        }
                        else
                        {
                            if (eligibility.MonthsFrom <= premiumBreakdownRequest.premiumBreakdownRequest.Usage &&
                                eligibility.MonthsTo >= premiumBreakdownRequest.premiumBreakdownRequest.Usage)
                            {
                                if (eligibility.IsPercentage)
                                {
                                    EligibilityPremium = TotalPremium * eligibility.Premium / 100;
                                }
                                else
                                {
                                    EligibilityPremium = eligibility.Premium *
                                                         contractExtensionPremium.ConversionRate;
                                }
                                if (eligibility.PlusMinus.ToLower() != "plus")
                                {
                                    EligibilityPremium = EligibilityPremium * (-1);
                                }

                                if (eligibility.isMandatory)
                                {
                                }
                            }
                        }
                    }


                    // TotalPremium += EligibilityPremium;
                    //  TotalPremiumNRP += EligibilityPremium;

                    var eligibilityVal = new BreakdownValue()
                    {
                        Value = EligibilityPremium,
                        ItemName = "Value"
                    };
                    var eligibilityBreakdownDetails = new BreakdownDetails()
                    {
                        Name = "Eligibility",
                        Breakdowns = new List<BreakdownValue>() { eligibilityVal }
                    };
                    Response.BreakdownDetails.Add(eligibilityBreakdownDetails);


                    //tax
                    IList<ContractTaxMapping> contractTaxesList = session.QueryOver<ContractTaxMapping>()
                        .Where(a => a.ContractId == contract.ContractId).List<ContractTaxMapping>();
                    IList<CountryTaxes> countryTaxesList = session.QueryOver<CountryTaxes>()
                        .WhereRestrictionOn(b => b.Id).IsIn(contractTaxesList.Select(a => a.CountryTaxId).ToList())
                        .List<CountryTaxes>();

                    var totalTax = decimal.Zero;

                    foreach (var countryTax in countryTaxesList.OrderBy(a => a.IndexVal))
                    {
                        var currentTax = decimal.Zero;
                        if (countryTax.IsPercentage)
                        {
                            if (countryTax.IsOnGross)
                            {
                                if (countryTax.IsOnPreviousTax)
                                {
                                    currentTax = (TotalPremium + totalTax) * countryTax.TaxValue / 100;
                                }
                                else
                                {
                                    currentTax = (TotalPremium) * countryTax.TaxValue / 100;
                                }
                            }
                            else
                            {
                                if (countryTax.IsOnPreviousTax)
                                {
                                    currentTax = (TotalPremiumNRP + totalTax) * countryTax.TaxValue / 100;
                                }
                                else
                                {
                                    currentTax = (TotalPremiumNRP) * countryTax.TaxValue / 100;
                                }
                            }
                            if (currentTax < countryTax.MinimumValue * countryTax.ConversionRate)
                            {
                                currentTax = countryTax.MinimumValue * countryTax.ConversionRate;
                            }
                        }
                        else
                        {
                            currentTax += countryTax.TaxValue * countryTax.ConversionRate;
                        }
                        TaxBreakdowns.Add(new BreakdownValue()
                        {
                            ItemName = new CommonEntityManager().GetTaxCodeByTaxTypeId(countryTax.TaxTypeId),
                            Value = currentTax
                        });

                        totalTax += currentTax;
                    }
                    //  Tax = totalTax;
                    // TotalPremium += totalTax;

                    TotalPremium = Math.Round(TotalPremium * 100) / 100;
                    //response setup
                    Response.Premium = TotalPremium;

                    //adding tax for main response
                    if (TaxBreakdowns.Count == 0)
                    {
                        BreakdownValue defaultValues = new BreakdownValue()
                        {
                            ItemName = "Value",
                            Value = decimal.Zero
                        };
                        TaxBreakdowns.Add(defaultValues);
                    }

                    var BreakdownDetails = new BreakdownDetails()
                    {
                        Name = "TAX",
                        Breakdowns = TaxBreakdowns
                    };
                    Response.BreakdownDetails.Add(BreakdownDetails);

                    //others if applicable
                }
            }

            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                             ex.InnerException);
            }
            return Response;

        }

        internal static string GetNextPolicyNumber(Guid BranchId, Guid DealerId, Guid ProductId, Guid CountryId,Guid CommodityId,Guid TpaId )
        {
            string response = string.Empty;
            try
            {
                ISession session = EntitySessionManager.GetSession();
                TPA tpa = session.Query<TPA>().FirstOrDefault();
                    response =
                    session.CreateSQLQuery("exec GenerateNextPolicyNumberUpdated :BranchId,:DealerId,:ProductId,:CountryId")
                        .SetGuid("BranchId", BranchId)
                        .SetGuid("DealerId", DealerId)
                        .SetGuid("ProductId", ProductId)
                        .SetGuid("CountryId", CountryId)
                        .UniqueResult<string>();
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                             ex.InnerException);
            }
            return response;
        }

        internal static string GetNextPolicyNumberForTyre(Guid BranchId, Guid DealerId, Guid ProductId, Guid CountryId, Guid CommodityId, Guid TpaId)
        {
            string response = string.Empty;
            try
            {
                ISession session = EntitySessionManager.GetSession();

                TPA tpa = session.Query<TPA>().FirstOrDefault();

                if (tpa.Name == "CycleandCarriage" || tpa.Name == "CNC" || tpa.Name == "CycleandCarriagetest" || tpa.Name == "continental" || tpa.Name == "contidentalTest")
                {
                    response =
                    session.CreateSQLQuery("exec GenerateNextPolicyNumber :BranchId,:DealerId,:ProductId,:CountryId ")
                        .SetGuid("BranchId", BranchId)
                        .SetGuid("DealerId", DealerId)
                        .SetGuid("ProductId", ProductId)
                        .SetGuid("CountryId", CountryId)

                        .UniqueResult<string>();
                }
                else
                {
                    response =
                    session.CreateSQLQuery("exec GenerateNextPolicyNumberNew :BranchId,:DealerId,:ProductId,:CountryId,:CommodityId,:TpaId ")
                        .SetGuid("BranchId", BranchId)
                        .SetGuid("DealerId", DealerId)
                        .SetGuid("ProductId", ProductId)
                        .SetGuid("CountryId", CountryId)
                        .SetGuid("CommodityId", CommodityId)
                        .SetGuid("TpaId", TpaId)

                        .UniqueResult<string>();
                }



            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                             ex.InnerException);
            }
            return response;
        }

        internal static string GetNextPolicyNumberPlusCounter(int counter, Guid tPABranchId, Guid dealerId, Guid productId, Guid countryId)
        {
            string response = string.Empty;
            try
            {
                ISession session = EntitySessionManager.GetSession();

                TPA tpa = session.Query<TPA>().FirstOrDefault();

                if (tpa.Name == "CycleandCarriage")
                {
                    response =
                    session.CreateSQLQuery("exec GenerateNextPolicyNumberWithCounter :BranchId,:DealerId,:ProductId,:CountryId, :Counter ")
                        .SetGuid("BranchId", tPABranchId)
                        .SetGuid("DealerId", dealerId)
                        .SetGuid("ProductId", productId)
                        .SetGuid("CountryId", countryId)
                        .SetInt32("Counter", counter)
                        .UniqueResult<string>();
                }
                else
                {
                    response =
                    session.CreateSQLQuery("exec GenerateNextPolicyNumberWithCounter :BranchId,:DealerId,:ProductId,:CountryId, :Counter ")
                        .SetGuid("BranchId", tPABranchId)
                        .SetGuid("DealerId", dealerId)
                        .SetGuid("ProductId", productId)
                        .SetGuid("CountryId", countryId)
                        .SetInt32("Counter", counter)
                        .UniqueResult<string>();
                }



            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                             ex.InnerException);
            }
            return response;
        }


        internal static string GetNextPolicyNumberPlusCounterForTyre(int counter, Guid dealerBranch, Guid dealerId, Guid productId, Guid countryId)
        {
            string response = string.Empty;
            try
            {
                ISession session = EntitySessionManager.GetSession();

                TPA tpa = session.Query<TPA>().FirstOrDefault();

                if (tpa.Name == "CycleandCarriage")
                {
                    response =
                    session.CreateSQLQuery("exec GenerateNextPolicyNumberWithCounter :BranchId,:DealerId,:ProductId,:CountryId, :Counter ")
                        .SetGuid("BranchId", dealerBranch)
                        .SetGuid("DealerId", dealerId)
                        .SetGuid("ProductId", productId)
                        .SetGuid("CountryId", countryId)
                        .SetInt32("Counter", counter)
                        .UniqueResult<string>();
                }
                else
                {
                    response =
                    session.CreateSQLQuery("exec GenerateNextPolicyNumberWithCounter :BranchId,:DealerId,:ProductId,:CountryId, :Counter ")
                        .SetGuid("BranchId", dealerBranch)
                        .SetGuid("DealerId", dealerId)
                        .SetGuid("ProductId", productId)
                        .SetGuid("CountryId", countryId)
                        .SetInt32("Counter", counter)
                        .UniqueResult<string>();
                }



            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                             ex.InnerException);
            }
            return response;
        }


        private static decimal GetPremiumWOTaxesByContractId(Guid contractId, Guid currencyId, Guid currencyPeriodId, string premiumBasedOn)
        {
            decimal Response = Decimal.Zero;
            try
            {
                ISession session = EntitySessionManager.GetSession();
                CurrencyEntityManager cem = new CurrencyEntityManager();

                Contract contract = session.Query<Contract>()
                    .Where(a => a.Id == contractId).FirstOrDefault();
                if (contract == null)
                {
                    return Response;
                }

                if (premiumBasedOn.ToLower() == "rp")
                {
                    //  Response = contract.GrossPremium;
                }
                else
                {
                    //  Response = cem.ConvertFromBaseCurrency(contract.GrossPremium, currencyId, currencyPeriodId);
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ((ex.InnerException != null) ? ex.InnerException.ToString() : ""));
            }
            return Response;
        }

        internal string RenewPolicy(SavePolicyRequestDto SavePolicyRequest, string DBName)
        {

            string Response = String.Empty;

            if (SavePolicyRequest == null || SavePolicyRequest.policyDetails == null
                 || SavePolicyRequest.policyDetails.requestedUser == Guid.Empty
                || SavePolicyRequest.policyDetails.policy == null || SavePolicyRequest.policyDetails.product == null
                || SavePolicyRequest.policyDetails.customer == null || SavePolicyRequest.policyDetails.payment == null)
            {
                Response = "Request data is invalid";
                return Response;
            }
            //avoid datetime errors
            if (SavePolicyRequest.policyDetails.customer.idIssueDate == null ||
                SavePolicyRequest.policyDetails.customer.idIssueDate < SqlDateTime.MinValue.Value)
            {
                SavePolicyRequest.policyDetails.customer.idIssueDate = SqlDateTime.MinValue.Value;
            }

            if (SavePolicyRequest.policyDetails.customer.dateOfBirth == null ||
                SavePolicyRequest.policyDetails.customer.dateOfBirth < SqlDateTime.MinValue.Value)
            {
                SavePolicyRequest.policyDetails.customer.dateOfBirth = SqlDateTime.MinValue.Value;
            }

            try
            {
                ISession session = EntitySessionManager.GetSession();
                PolicyBundle dbPolicyBundle = session.Query<PolicyBundle>()
                    .Where(a => a.Id == SavePolicyRequest.policyDetails.policy.id).FirstOrDefault();
                if (dbPolicyBundle == null)
                {
                    return Response;
                }

                List<Policy> dbPolicyList = session.Query<Policy>()
                    .Where(a => a.PolicyBundleId == SavePolicyRequest.policyDetails.policy.id).ToList();
                if (dbPolicyList == null || dbPolicyList.Count == 0)
                {
                    return Response;
                }

                //customer validation
                if (SavePolicyRequest.policyDetails.customer.customerId != dbPolicyBundle.CustomerId)
                {
                    Response = "Customer details mismatched.";
                    return Response;
                }

                //product validation
                if (!IsGuid(SavePolicyRequest.policyDetails.product.productId.ToString()))
                {
                    Response = "Product details mismatched.";
                    return Response;
                }

                #region "policy bundle transaction"
                //saving policy bundle transaction
                Guid PolicyBundleTransactionId = Guid.NewGuid();


                PolicyBundleTransaction bundleTransaction = new PolicyBundleTransaction()
                {
                    Comment = SavePolicyRequest.policyDetails.payment.comment,
                    CommodityTypeId = SavePolicyRequest.policyDetails.product.commodityTypeId,
                    ContractId = Guid.Empty,
                    CoverTypeId = Guid.Empty,
                    CustomerId = SavePolicyRequest.policyDetails.customer.customerId,
                    CustomerPayment = SavePolicyRequest.policyDetails.payment.customerPayment,
                    CustomerPaymentCurrencyTypeId = SavePolicyRequest.policyDetails.product.customerPaymentCurrencyTypeId,
                    DealerId = SavePolicyRequest.policyDetails.product.dealerId,
                    DealerLocationId = SavePolicyRequest.policyDetails.product.dealerLocationId,
                    DealerPayment = SavePolicyRequest.policyDetails.payment.dealerPayment,
                    DealerPaymentCurrencyTypeId = SavePolicyRequest.policyDetails.product.dealerPaymentCurrencyTypeId,
                    DealerPolicy = SavePolicyRequest.policyDetails.policy.dealerPolicy,
                    Discount = SavePolicyRequest.policyDetails.payment.discount,
                    EntryDateTime = DateTime.UtcNow,
                    EntryUser = SavePolicyRequest.policyDetails.requestedUser,
                    ExtensionTypeId = Guid.Empty,
                    HrsUsedAtPolicySale = SavePolicyRequest.policyDetails.policy.hrsUsedAtPolicySale,
                    Id = PolicyBundleTransactionId,
                    IsApproved = true,
                    IsPartialPayment = SavePolicyRequest.policyDetails.payment.isPartialPayment,
                    IsPolicyCanceled = false,
                    IsPreWarrantyCheck = false,
                    IsSpecialDeal = SavePolicyRequest.policyDetails.payment.isSpecialDeal,
                    PaymentModeId = SavePolicyRequest.policyDetails.payment.paymentModeId,
                    PolicyBundleId = SavePolicyRequest.policyDetails.policy.id,
                    PolicyNo = "",
                    PolicySoldDate = SavePolicyRequest.policyDetails.policy.policySoldDate,
                    Premium = SavePolicyRequest.policyDetails.policy.premium,
                    PremiumCurrencyTypeId = Guid.Empty,
                    ProductId = SavePolicyRequest.policyDetails.product.productId,
                    RefNo = SavePolicyRequest.policyDetails.payment.refNo,
                    SalesPersonId = SavePolicyRequest.policyDetails.policy.salesPersonId,
                    TransactionTypeId = GetTransactionTypeId("Renewed"),
                    Type = "Renewed",
                    MWStartDate = SavePolicyRequest.policyDetails.product.MWStartDate,
                    MWIsAvailable = SavePolicyRequest.policyDetails.product.MWIsAvailable,

                };
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Save(bundleTransaction, bundleTransaction.Id);
                    transaction.Commit();
                }

                #endregion "policy bundle transaction"

                #region "policy transaction"
                foreach (ProductContract_ productContract in SavePolicyRequest.policyDetails.policy.productContracts)
                {
                    PolicyTransaction policyTransaction = new PolicyTransaction()
                    {
                        AddnSerialNo = SavePolicyRequest.policyDetails.product.additionalSerial,
                        Address1 = SavePolicyRequest.policyDetails.customer.address1,
                        Address2 = SavePolicyRequest.policyDetails.customer.address2,
                        Address3 = SavePolicyRequest.policyDetails.customer.address3,
                        Address4 = SavePolicyRequest.policyDetails.customer.address4,
                        AspirationId = SavePolicyRequest.policyDetails.product.aspirationTypeId,
                        BAndWId = SavePolicyRequest.policyDetails.product.id,
                        BodyTypeId = SavePolicyRequest.policyDetails.product.bodyTypeId,
                        BusinessAddress1 = SavePolicyRequest.policyDetails.customer.businessAddress1,
                        BusinessAddress2 = SavePolicyRequest.policyDetails.customer.businessAddress2,
                        BusinessAddress3 = SavePolicyRequest.policyDetails.customer.businessAddress3,
                        BusinessAddress4 = SavePolicyRequest.policyDetails.customer.businessAddress4,
                        BusinessName = SavePolicyRequest.policyDetails.customer.businessName,
                        BusinessTelNo = SavePolicyRequest.policyDetails.customer.businessTelNo,
                        CancelationComment = "",
                        CategoryId = SavePolicyRequest.policyDetails.product.categoryId,
                        CityId = SavePolicyRequest.policyDetails.customer.cityId,
                        Comment = SavePolicyRequest.policyDetails.payment.comment,
                        CommodityTypeId = SavePolicyRequest.policyDetails.product.commodityTypeId,
                        CommodityUsageTypeId = SavePolicyRequest.policyDetails.product.commodityUsageTypeId,
                        ContractId = productContract.ContractId,
                        CountryId = SavePolicyRequest.policyDetails.customer.countryId,
                        CoverTypeId = productContract.CoverTypeId,
                        CustomerId = SavePolicyRequest.policyDetails.customer.customerId,
                        CustomerPayment = SavePolicyRequest.policyDetails.payment.customerPayment,
                        CustomerPaymentCurrencyTypeId = SavePolicyRequest.policyDetails.product.customerPaymentCurrencyTypeId,
                        CustomerTypeId = SavePolicyRequest.policyDetails.customer.customerTypeId,
                        CylinderCountId = SavePolicyRequest.policyDetails.product.cylinderCountId,
                        RegistrationDate = SavePolicyRequest.policyDetails.product.registrationDate,
                        DateOfBirth = SavePolicyRequest.policyDetails.customer.dateOfBirth,
                        DealerId = SavePolicyRequest.policyDetails.product.dealerId,
                        DealerLocationId = SavePolicyRequest.policyDetails.product.dealerLocationId,
                        DealerPayment = SavePolicyRequest.policyDetails.payment.dealerPayment,
                        DealerPaymentCurrencyTypeId = SavePolicyRequest.policyDetails.product.dealerPaymentCurrencyTypeId,
                        DealerPolicy = SavePolicyRequest.policyDetails.policy.dealerPolicy,
                        DealerPrice = SavePolicyRequest.policyDetails.product.dealerPrice,
                        Discount = SavePolicyRequest.policyDetails.payment.discount,
                        DLIssueDate = SavePolicyRequest.policyDetails.customer.idIssueDate,
                        DriveTypeId = Guid.Empty,
                        Email = SavePolicyRequest.policyDetails.customer.email,
                        EngineCapacityId = SavePolicyRequest.policyDetails.product.engineCapacityId,
                        ExtensionTypeId = productContract.ExtensionTypeId,
                        FirstName = SavePolicyRequest.policyDetails.customer.firstName,
                        FuelTypeId = SavePolicyRequest.policyDetails.product.fuelTypeId,
                        Gender = SavePolicyRequest.policyDetails.customer.gender,
                        HrsUsedAtPolicySale = SavePolicyRequest.policyDetails.policy.hrsUsedAtPolicySale,
                        Id = Guid.NewGuid(),
                        PolicyBundleTransactionId = PolicyBundleTransactionId,
                        IDNo = SavePolicyRequest.policyDetails.customer.idNo,
                        IDTypeId = SavePolicyRequest.policyDetails.customer.idTypeId,
                        InvoiceNo = SavePolicyRequest.policyDetails.product.invoiceNo,
                        IsActive = true,
                        IsApproved = true,
                        IsRejected = false,
                        ApprovedRejectedBy = Guid.Empty,
                        IsPartialPayment = SavePolicyRequest.policyDetails.payment.isPartialPayment,
                        IsPreWarrantyCheck = false,
                        IsRecordActive = true,
                        IsSpecialDeal = SavePolicyRequest.policyDetails.payment.isSpecialDeal,
                        ItemPrice = SavePolicyRequest.policyDetails.product.itemPrice,
                        ItemPurchasedDate = SavePolicyRequest.policyDetails.product.itemPurchasedDate,
                        ItemStatusId = SavePolicyRequest.policyDetails.product.itemStatusId,
                        LastName = SavePolicyRequest.policyDetails.customer.lastName,
                        MakeId = SavePolicyRequest.policyDetails.product.makeId,
                        MobileNo = SavePolicyRequest.policyDetails.customer.mobileNo,
                        ModelCode = "",
                        ModelId = SavePolicyRequest.policyDetails.product.modelId,
                        ModelYear = SavePolicyRequest.policyDetails.product.modelYear,
                        Password = "",
                        PaymentModeId = SavePolicyRequest.policyDetails.payment.paymentModeId,
                        PlateNo = SavePolicyRequest.policyDetails.product.additionalSerial,
                        PolicyId = productContract.Id,
                        PolicyNo = productContract.PolicyNo,
                        PolicySoldDate = SavePolicyRequest.policyDetails.policy.policySoldDate,
                        PolicyEndDate = SqlDateTime.MinValue.Value,
                        PolicyStartDate = SqlDateTime.MinValue.Value,
                        ModifiedDate = DateTime.UtcNow,
                        NationalityId = SavePolicyRequest.policyDetails.customer.nationalityId,
                        OtherTelNo = SavePolicyRequest.policyDetails.customer.otherTelNo,
                        Premium = productContract.Premium,
                        PremiumCurrencyTypeId = productContract.PremiumCurrencyTypeId,
                        ProductId = SavePolicyRequest.policyDetails.product.productId,
                        RefNo = SavePolicyRequest.policyDetails.payment.refNo,
                        SalesPersonId = SavePolicyRequest.policyDetails.policy.salesPersonId,
                        SerialNo = SavePolicyRequest.policyDetails.product.serialNumber,
                        TransactionTypeId = GetTransactionTypeId("Renewed"),
                        TransferFee = 0,
                        TransmissionId = SavePolicyRequest.policyDetails.product.transmissionTypeId,
                        UsageTypeId = SavePolicyRequest.policyDetails.customer.usageTypeId,
                        Variant = SavePolicyRequest.policyDetails.product.variantId,
                        VehicleId = SavePolicyRequest.policyDetails.product.productId,
                        VehiclePrice = SavePolicyRequest.policyDetails.product.itemPrice,
                        VINNo = SavePolicyRequest.policyDetails.product.serialNumber,
                        MWStartDate = SavePolicyRequest.policyDetails.product.MWStartDate,
                        MWIsAvailable = SavePolicyRequest.policyDetails.product.MWIsAvailable,
                        BookletNumber = productContract.BookletNumber,
                        ContractExtensionPremiumId = productContract.CoverTypeId,
                        ContractExtensionsId = productContract.ExtensionTypeId,
                        ContractInsuaranceLimitationId = productContract.AttributeSpecificationId,
                    };

                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.Save(policyTransaction, policyTransaction.Id);
                        transaction.Commit();
                    }
                }

                #endregion "policy transaction"

                #region "Policy/Bundle History"

                PolicyBundleHistory policyBundleHistory = DBDTOTransformer.Instance.PolicyBundleToPolicyBundleHistory(dbPolicyBundle, bundleTransaction);
                using (ITransaction transaction = session.BeginTransaction())
                {
                    policyBundleHistory.Type = "Renewal";
                    session.Save(policyBundleHistory, policyBundleHistory.Id);
                    transaction.Commit();
                }


                List<PolicyHistory> policyHistoryList = DBDTOTransformer.Instance.PolicyToPolicyHistory(dbPolicyList);
                foreach (PolicyHistory policyHistory in policyHistoryList)
                {
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        policyHistory.TransactionTypeId = policyBundleHistory.TransactionTypeId;
                        policyHistory.ModifiedDate = DateTime.UtcNow;
                        session.Save(policyHistory, policyHistory.Id);
                        transaction.Commit();
                    }
                }
                #endregion

                #region "Update Policy"
                //   session.Evict(dbPolicyList);
                foreach (Policy policy in dbPolicyList)
                {
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        Policy existingPolicy = session.Query<Policy>().Where(a => a.Id == policy.Id).FirstOrDefault();
                        policy.IsPolicyRenewed = true;
                        policy.ApprovedDate = existingPolicy.ApprovedDate;
                        session.Update(policy, policy.Id);
                        transaction.Commit();
                    }
                }
                #endregion "Update Policy"

                #region "Insert New Policy"
                Guid newPolicyId = Guid.NewGuid();
                SavePolicyRequest.policyDetails.policy.policyBundleId = newPolicyId;
                string status = SavePolicy(SavePolicyRequest, DBName, newPolicyId);
                #endregion "Insert New Policy"
                if (status != "success")
                {
                    Response = status;
                    return Response;
                }

                #region "Insert to Policy Renewal"
                PolicyRenewal policyRenewalDetails = new PolicyRenewal()
                {
                    Id = Guid.NewGuid(),
                    NewPolicyBundleId = newPolicyId,
                    OriginalPolicyBundleId = dbPolicyBundle.Id,
                    PolicyBundleHistoryId = policyBundleHistory.Id,
                    PolicyTransactionId = PolicyBundleTransactionId,
                    RenewalFee = SavePolicyRequest.policyDetails.renewalFee,
                    RenewedBy = SavePolicyRequest.policyDetails.requestedUser,
                    RenewedDate = DateTime.UtcNow

                };
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Save(policyRenewalDetails, policyRenewalDetails.Id);
                    transaction.Commit();
                }
                #endregion "Insert to Policy Renewal"

                Response = "success";
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ((ex.InnerException != null) ? ex.InnerException.ToString() : ""));
                Response = "Error occured while renewing policy";
            }
            return Response;

        }

        internal object GetPloiciesForSearchInquiryGrid(PolicySearchInquiryGridSearchCriterias policySearchInquiryGridSearchCriterias, PaginationOptionsPolicySearchInquiryGrid paginationOptionsPolicySearchInquiryGrid)
        {
            try
            {
                CommonEntityManager cem = new CommonEntityManager();
                ISession session = EntitySessionManager.GetSession();
                //expression builder for contract
                Expression<Func<Policy, bool>> filterPolicy = PredicateBuilder.True<Policy>();
                //if (IsGuid(policySearchInquiryGridSearchCriterias.Country.ToString()))
                //{
                //    filterPolicy = filterPolicy.And(a => a.BordxCountryId == policySearchInquiryGridSearchCriterias.Country);
                //}

                if (IsGuid(policySearchInquiryGridSearchCriterias.DealerId.ToString()))
                {
                    filterPolicy = filterPolicy.And(a => a.DealerId == policySearchInquiryGridSearchCriterias.DealerId);
                }

                if (!String.IsNullOrEmpty(policySearchInquiryGridSearchCriterias.policyNo))
                {
                    filterPolicy = filterPolicy.And(a => a.PolicyNo.ToLower().Contains(policySearchInquiryGridSearchCriterias.policyNo.ToLower()));
                }
                //if (IsGuid(policySearchInquiryGridSearchCriterias.PolicyNo.ToString()))
                //    filterPolicy = filterPolicy.And(a => a.PolicyNo == policySearchInquiryGridSearchCriterias.PolicyNo);
                if (IsGuid(policySearchInquiryGridSearchCriterias.commodityTypeId.ToString()))
                {
                    filterPolicy = filterPolicy.And(a => a.CommodityTypeId == policySearchInquiryGridSearchCriterias.commodityTypeId);
                }

                if (!String.IsNullOrEmpty(policySearchInquiryGridSearchCriterias.policySoldDateFrom.ToString())
                && policySearchInquiryGridSearchCriterias.policySoldDateFrom != DateTime.MinValue)
                {
                    filterPolicy = filterPolicy.And(a => a.PolicySoldDate >= policySearchInquiryGridSearchCriterias.policySoldDateFrom);
                }
                if (!String.IsNullOrEmpty(policySearchInquiryGridSearchCriterias.policySoldDateTo.ToString())
                    && policySearchInquiryGridSearchCriterias.policySoldDateTo != DateTime.MinValue)
                {
                    filterPolicy = filterPolicy.And(a => a.PolicySoldDate <= policySearchInquiryGridSearchCriterias.policySoldDateTo);
                }
                IQueryable<Guid> customers;
                IQueryable<VehicleDetails> vehicle;
                IQueryable<BrownAndWhiteDetails> BnW;
                IQueryable<VehiclePolicy> vehiclePolicy;
                IQueryable<BAndWPolicy> BnWPolicy;
                IQueryable<OtherItemDetails> other;
                IQueryable<OtherItemPolicy> otherPolicy;
                IQueryable<Guid> contractsIdList;


                List<Customer> customerList = new List<Customer>();

                var policyGridDetails = session.Query<Policy>().Where(filterPolicy);

                if (IsGuid(policySearchInquiryGridSearchCriterias.Country.ToString()))
                {
                    contractsIdList = session.Query<Contract>().Where(a => a.CountryId== policySearchInquiryGridSearchCriterias.Country).Select(s=> s.Id);
                    policyGridDetails = policyGridDetails.Where(b => contractsIdList.Any(c => c == b.ContractId));
                }

                if (!String.IsNullOrEmpty(policySearchInquiryGridSearchCriterias.mobileNo))
                {
                    customers = session.Query<Customer>().Where(a => a.MobileNo.Contains(policySearchInquiryGridSearchCriterias.mobileNo)).Select(s=> s.Id);
                    policyGridDetails = policyGridDetails.Where(b => customers.Any(c => c == b.CustomerId));

                }
                if (!String.IsNullOrEmpty(policySearchInquiryGridSearchCriterias.CustomerName))
                {
                    customers = session.Query<Customer>().Where(a => a.FirstName.Contains(policySearchInquiryGridSearchCriterias.CustomerName)).Select(s => s.Id);
                    policyGridDetails = policyGridDetails.Where(b => customers.Any(c => c == b.CustomerId));

                }
                //if (!String.IsNullOrEmpty(policySearchInquiryGridSearchCriterias.CustomerName)) {

                //    vehiclePolicy = session.Query<VehiclePolicy>()
                //         .Where(session.Query<VehicleDetails>(), a => a.Id, b => b.VehicleId, (a, b) => new { a, b } )

                //    policyGridDetails = policyGridDetails.Where(b => vehiclePolicy.Any(c => c.PolicyId==b.Id));
                //}
                if (!String.IsNullOrEmpty(policySearchInquiryGridSearchCriterias.serialNo))
                {
                    vehicle = session.Query<VehicleDetails>().Where(a => a.VINNo.ToLower().Contains(policySearchInquiryGridSearchCriterias.serialNo.ToLower()));
                    BnW = session.Query<BrownAndWhiteDetails>().Where(a => a.SerialNo.ToLower().Contains(policySearchInquiryGridSearchCriterias.serialNo.ToLower()));
                    other = session.Query<OtherItemDetails>().Where(a => a.SerialNo.ToLower().Contains(policySearchInquiryGridSearchCriterias.serialNo.ToLower()));
                    vehiclePolicy = session.Query<VehiclePolicy>().Where(b => vehicle.Any(c => c.Id == b.VehicleId));
                    BnWPolicy = session.Query<BAndWPolicy>().Where(b => BnW.Any(c => c.Id == b.Id));
                    otherPolicy = session.Query<OtherItemPolicy>().Where(b => other.Any(c => c.Id == b.OtherItemId));
                    policyGridDetails = policyGridDetails.Where(d => vehiclePolicy.Any(e => e.PolicyId == d.Id) || BnWPolicy.Any(f => f.PolicyId == d.Id) || otherPolicy.Any(f => f.PolicyId == d.Id));

                }

                long TotalRecords = policyGridDetails.Count();
                var policyGridDetailsFilterd =
                    policyGridDetails.OrderByDescending(a => a.PolicyNo)
                    .Skip((paginationOptionsPolicySearchInquiryGrid.pageNumber - 1) * (paginationOptionsPolicySearchInquiryGrid.pageSize))
                .Take(paginationOptionsPolicySearchInquiryGrid.pageSize)
                .Select(a => new
                {

                    Id = a.PolicyBundleId,
                    CommodityType = GetCommodityNameById(a.CommodityTypeId),
                    PolicyNo = a.PolicyNo,
                    SerialNo = getSerialNumberByPolicyId(a.Id),
                    MobileNo = getMobileNumberByCustomerId(a.CustomerId),
                    PolicySoldDate = a.PolicySoldDate.ToString("dd-MMM-yyyy"),
                    CustomerName = cem.GetCustomerNameById(a.CustomerId),


                })
               .ToArray();
                var response = new CommonGridResponseDto()
                {
                    totalRecords = TotalRecords,
                    data = policyGridDetailsFilterd
                };
                return new JavaScriptSerializer().Serialize(response);
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ((ex.InnerException != null) ? ex.InnerException.ToString() : ""));
                return null;
            }
        }


        public byte[] GetPolicyAttachmentByPolicyId(Guid PolicyId, string dbName)
        {
            ISession session = EntitySessionManager.GetSession();

            var query = from policy in session.Query<Policy>()
                        where policy.PolicyBundleId == PolicyId
                        join product in session.Query<Product>() on policy.ProductId equals product.Id
                        select new { productcode = product.Productcode, policyId = policy.Id };

            if (query != null && query.ToList().Count() > 0)
            {
                PolicyStatementReport psr = new PolicyStatementReport();
                return psr.Generate(dbName, query.FirstOrDefault().productcode, PolicyId);
            }
            return null;

        }

        public static object PolicySectionDataRetrivalForDashboard(Guid loggedInUserId)
        {
            PolicyDashboardResponseDto response = new PolicyDashboardResponseDto();
            response.data = new List<ChartData>();
            response.status = "ok";
            try
            {
                ISession session = EntitySessionManager.GetSession();
                SystemUser systemUser = session.Query<SystemUser>().FirstOrDefault(a => a.LoginMapId == loggedInUserId);

                #region validation
                if (systemUser == null)
                {
                    response.status = "Logged in user invalid";
                    return response;
                }

                UserRoleMapping userRoleMapping =
                    session.Query<UserRoleMapping>().FirstOrDefault(a => a.UserId == loggedInUserId);

                if (userRoleMapping == null)
                {
                    response.status = "Logged in user not assigned to any user role.";
                    return response;
                }
                #endregion

                List<DashboardChart> dashboardPolicyCharts = session.Query<DashboardChart>()
                    .Where(a => a.Section.ToLower() == "policy")
                    .ToList();

                List<DashboardChartAccess> chartAccessData = session.Query<DashboardChartAccess>()
                    .Where(a => a.UserRoleId == userRoleMapping.RoleId)
                    .ToList();
                if (chartAccessData.Count == 0)
                {
                    response.status = "User role dosen't have access to dashboard charts.Please contact administrator.";
                    return response;
                }

                foreach (DashboardChart chart in dashboardPolicyCharts)
                {
                    DashboardChartAccess accessData = chartAccessData.FirstOrDefault(a => a.DashboardChartId == chart.Id);

                    var chartData = new ChartData()
                    {
                        chartCode = chart.ChartCode,
                        status = accessData == null ? "noaccess" : accessData.IsAllowed ? "ok" : "noaccess",
                        data = GetChartDataForDashboard(accessData, chart.ChartCode, systemUser)
                    };
                    response.data.Add(chartData);
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                             ex.InnerException);
            }
            return response;
        }

        private static object GetChartDataForDashboard(DashboardChartAccess accessData, string chartCode, SystemUser systemUser)
        {
            object response = new object();
            if (accessData == null || accessData.IsAllowed == false)
            {
                return null;
            }
            ISession session = EntitySessionManager.GetSession();
            Expression<Func<Policy, bool>> filterPolicy = PredicateBuilder.True<Policy>();
            filterPolicy = filterPolicy.And(a => a.IsPolicyRenewed == false);
            filterPolicy = filterPolicy.And(a => a.IsApproved == true);

            var userType = session.Query<UserType>().FirstOrDefault(a => a.Id == systemUser.UserTypeId);
            if (userType == null)
            {
                return null;
            }

            var userTypeCode = userType.Code;
            //dynamic policy filtering based on branch data
            if (!accessData.IsAllBranches)
            {
                if (userTypeCode.ToLower() == "iu")
                {
                    List<Guid> allowedBranches = session.Query<UserBranch>()
                        .Where(a => a.InternalUserId == systemUser.LoginMapId)
                        .Select(a => a.TPABranchId).ToList();

                    filterPolicy = filterPolicy.And(a => allowedBranches.Any(c => c == a.TPABranchId));
                }
                else if (userTypeCode.ToLower() == "du")
                {
                    DealerStaff dealerStaff =
                        session.Query<DealerStaff>().FirstOrDefault(a => a.UserId == systemUser.LoginMapId);
                    //getting all dealer data coz not yet delaer branch user assigmnet done
                    filterPolicy = filterPolicy.And(a => a.DealerId == dealerStaff.DealerId);
                }
                else if (userTypeCode.ToLower() == "ri")
                {
                    ReinsurerUser reinsurerUser =
                        session.Query<ReinsurerUser>().FirstOrDefault(a => a.InternalUserId == systemUser.LoginMapId);
                    List<ReinsurerContract> reinsurerContracts = session.Query<ReinsurerContract>()
                        .Where(a => a.ReinsurerId == reinsurerUser.ReinsurerId).ToList();
                    List<Guid> contractIds = new List<Guid>();
                    //session.Query<Contract>()
                    //.Where(a => reinsurerContracts.Any(c => c.Id == a.ReinsurerId)).Select(a => a.Id).ToList();

                    filterPolicy = filterPolicy.And(a => contractIds.Any(d => d == a.ContractId));
                }
            }
            else
            {
                if (userTypeCode.ToLower() == "du")
                {
                    DealerStaff dealerStaff =
                        session.Query<DealerStaff>().FirstOrDefault(a => a.UserId == systemUser.LoginMapId);
                    filterPolicy = filterPolicy.And(a => a.DealerId == dealerStaff.DealerId);
                }
                else if (userTypeCode.ToLower() == "ri")
                {
                    ReinsurerUser reinsurerUser =
                        session.Query<ReinsurerUser>().FirstOrDefault(a => a.InternalUserId == systemUser.LoginMapId);
                    List<ReinsurerContract> reinsurerContracts = session.Query<ReinsurerContract>()
                        .Where(a => a.ReinsurerId == reinsurerUser.ReinsurerId).ToList();
                    List<Guid> contractIds = new List<Guid>();
                    //session.Query<Contract>()
                    //.Where(a => reinsurerContracts.Any(c => c.Id == a.ReinsurerId)).Select(a => a.Id).ToList();

                    filterPolicy = filterPolicy.And(a => contractIds.Any(d => d == a.ContractId));
                }
            }

            switch (chartCode)
            {
                case "ByWarrenty":
                    {
                        var data = //new object();
                         session.Query<Policy>().Where(filterPolicy)
                         .Join(session.Query<Contract>(), a => a.ContractId, b => b.Id, (a, b) => new { a, b })
                         .Join(session.Query<ContractExtensionPremium>(), c => c.a.ContractExtensionPremiumId, d => d.Id, (c, d) => new { c, d })
                         .Join(session.Query<WarrantyType>(), e => e.d.WarrentyTypeId, g => g.Id, (e, g) => new { e, g })
                         .GroupBy(h => h.g.WarrantyTypeDescription)
                         .Select(i => new PieChartDataResponseDto()
                         {
                             value = i.Count(),
                             label = i.Key,
                             color = "",
                             highlight = ""
                         }).ToArray();

                        response = data;
                        break;
                    }
                case "ByProduct":
                    {
                        var data = session.Query<Policy>().Where(filterPolicy)
                             .Join(session.Query<Product>(), a => a.ProductId, b => b.Id, (a, b) => new { a, b })
                              .GroupBy(c => c.b.Productname)
                               .Select(i => new PieChartDataResponseDto()
                               {
                                   value = i.Count(),
                                   label = i.Key,
                                   color = "",
                                   highlight = ""
                               }).ToArray();

                        response = data;
                        break;
                    }
                case "ByMake":
                    {
                        var dataAuto = session.Query<Policy>().Where(filterPolicy)
                             .Join(session.Query<VehiclePolicy>(), a => a.Id, b => b.PolicyId, (a, b) => new { a, b })
                             .Join(session.Query<VehicleDetails>(), c => c.b.VehicleId, d => d.Id, (c, d) => new { c, d })
                             .Join(session.Query<Make>(), e => e.d.MakeId, g => g.Id, (e, g) => new { e, g })
                             .Select(h => new
                             {
                                 h.g.MakeName,
                                 h.e.c.a.Id
                             }).ToList();


                        var dataElect = session.Query<Policy>().Where(filterPolicy)
                            .Join(session.Query<BAndWPolicy>(), i => i.Id, j => j.PolicyId, (i, j) => new { i, j })
                            .Join(session.Query<BrownAndWhiteDetails>(), k => k.j.BAndWId, l => l.Id,
                                (k, l) => new { k, l })
                            .Join(session.Query<Make>(), m => m.l.MakeId, n => n.Id, (m, n) => new { m, n })
                            .Select(o => new
                            {
                                o.n.MakeName,
                                o.m.k.i.Id
                            })
                            .ToList();

                        var dataOther = session.Query<Policy>().Where(filterPolicy)
                           .Join(session.Query<OtherItemPolicy>(), a => a.Id, b => b.PolicyId, (a, b) => new { a, b })
                           .Join(session.Query<OtherItemDetails>(), c => c.b.OtherItemId, d => d.Id, (c, d) => new { c, d })
                           .Join(session.Query<Make>(), e => e.d.MakeId, g => g.Id, (e, g) => new { e, g })
                           .Select(h => new
                           {
                               h.g.MakeName,
                               h.e.c.a.Id
                           }).ToList();

                        var dataYellow = session.Query<Policy>().Where(filterPolicy)
                           .Join(session.Query<YellowGoodPolicy>(), a => a.Id, b => b.PolicyId, (a, b) => new { a, b })
                           .Join(session.Query<YellowGoodDetails>(), c => c.b.YellowGoodId, d => d.Id, (c, d) => new { c, d })
                           .Join(session.Query<Make>(), e => e.d.MakeId, g => g.Id, (e, g) => new { e, g })
                           .Select(h => new
                           {
                               h.g.MakeName,
                               h.e.c.a.Id
                           }).ToList();

                        var data = dataAuto.Concat(dataElect).Concat(dataOther)
                            .Concat(dataYellow).GroupBy(i => i.MakeName)
                        .Select(j => new
                        {
                            Key = j.Key,
                            Count = j.Count()
                        }).ToArray();


                        RadarChartDataResponseDto resp = new RadarChartDataResponseDto()
                        {
                            labels = data.Select(a => a.Key).ToArray(),
                            datasets = new List<RadarChartData>()
                            {
                                new RadarChartData()
                                {
                                    data = data.Select(a=>a.Count).ToArray(),
                                    label = "default",
                                    fillColor ="#ccffcc",
                                    pointColor = "#00b300",
                                    strokeColor = "",
                                    pointHighlightFill =  "",
                                    pointHighlightStroke =  "",
                                    pointStrokeColor =  ""
                                }
                            }
                        };

                        response = resp;
                        break;
                    }
                case "ByCountry":
                    {
                        var data = session.Query<Policy>().Where(filterPolicy)
                            .Join(session.Query<Dealer>(), a => a.DealerId, b => b.Id, (a, b) => new { a, b })
                            .Join(session.Query<Country>(), c => c.b.CountryId, d => d.Id, (c, d) => new { c, d })
                            .GroupBy(h => h.d.CountryName)
                            .Select(i => new GeoChartDataResponseDto()
                            {
                                data = new List<KeyValuePair<string, int>>()
                               {
                                   new KeyValuePair<string, int>(i.Key,i.Count())
                               }
                            }).ToArray();

                        response = data;
                        break;
                    }
                case "ByMonth":
                    {
                        var data = session.Query<Policy>().Where(filterPolicy)
                            .GroupBy(h => new { h.PolicySoldDate.Year, h.PolicySoldDate.Month })
                            .Select(x => new
                            {
                                Key = x.Key,
                                Count = x.Count()
                            }).Take(12).ToArray();

                        RadarChartDataResponseDto resp = new RadarChartDataResponseDto()
                        {
                            labels = data.Select(a => CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(a.Key.Month) + " " + a.Key.Year).ToArray(),
                            datasets = new List<RadarChartData>()
                            {
                                new RadarChartData()
                                {
                                    data = data.Select(a=>a.Count).ToArray(),
                                    label = "default",
                                    fillColor ="#ebccff",
                                    pointColor = "#7a00cc",
                                    strokeColor = "",
                                    pointHighlightFill =  "",
                                    pointHighlightStroke =  "",
                                    pointStrokeColor =  ""
                                }
                            }
                        };

                        response = resp;
                        break;
                    }

            }
            return response;
        }

        public static string GetPolicyNumber(Guid branchId, Guid dealerId, Guid productId, Guid tpaId, Guid commodityTypeId)
        {
            string response = string.Empty;
            try
            {
                ISession session = EntitySessionManager.GetSession();

                if (branchId == Guid.Empty)
                {
                    branchId = session.Query<TPABranch>().FirstOrDefault(a => a.IsHeadOffice == true).Id;
                }
                Guid countryId = session.Query<Dealer>().FirstOrDefault(a => a.Id == dealerId).CountryId;
                response = GetNextPolicyNumber(branchId, dealerId, productId, countryId, commodityTypeId, tpaId);
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                               ex.InnerException);
            }
            return response;
        }

        internal static object GetAllAdditionalFieldDetailsByProductCode(string productCode)
        {
            object response = new object();
            try
            {
                ISession session = EntitySessionManager.GetSession();
                Product product = session.Query<Product>()
                    .FirstOrDefault(a => a.Productcode.ToLower() == productCode.ToLower());
                if (productCode.ToLower() == "tyre")
                {

                    var makes = session.Query<AdditionalPolicyMakeData>()
                        .Where(q => q.IsActive != false)
                        .Select(r => new
                        {
                            r.Id,
                            r.MakeName
                        }).ToArray().OrderBy(a => a.MakeName);

                    var models = session.Query<AdditionalPolicyModelData>()
                      .Where(q => q.IsActive != false)
                      .Select(r => new
                      {
                          r.Id,
                          r.MakeId,
                          r.ModelName
                      }).ToArray().OrderBy(a => a.ModelName);

                    //var  modelYears = new SelectList(Enumerable.Range(2000, (DateTime.Now.Year - 2000) + 2));

                    const int numberOfYears = 21;
                    var startYear = DateTime.Now.Year + 5;
                    var endYear = startYear - numberOfYears;

                    var yearList = new List<SelectListItem>();
                    for (var i = startYear; i > endYear; i--)
                    {
                        yearList.Add(new SelectListItem() { Value = i.ToString(), Text = i.ToString() });
                    }


                    dynamic obj = new ExpandoObject();
                    obj.makes = makes;
                    obj.models = models;
                    obj.modelYears = yearList;

                    response = new JavaScriptSerializer().Serialize(obj);
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                               ex.InnerException);
            }
            return response;
        }

        internal static object GetAllTireDetailsByInvoiceCode(string invocieCode)
        {
            GenericCodeMsgObjResponse response = new GenericCodeMsgObjResponse();
            response.code = "ERROR";
            try
            {
                #region validation
                if (string.IsNullOrEmpty(invocieCode))
                {
                    response.msg = "Invoice code is empty";
                    return response;
                }
                ISession session = EntitySessionManager.GetSession();
                InvoiceCode invoiceCode = session.Query<InvoiceCode>()
                    .FirstOrDefault(a => a.Code.ToUpper() == invocieCode.ToUpper());
                if (invoiceCode == null)
                {
                    response.msg = "Invoice code is not found in the database.";
                    return response;
                }
                List<InvoiceCodeDetails> invoiceCodeDetails = session.Query<InvoiceCodeDetails>()
                  .Where(a => a.InvoiceCodeId == invoiceCode.Id).ToList();
                if (invoiceCodeDetails.FirstOrDefault().IsPolicyCreated == true)
                {
                    response.msg = "Another policy already associated with this invoice code.";
                    return response;
                }
                #endregion Validation

                InvoceCodeDetailsFullResponseDto invoceCodeDetailsResponseDto = new InvoceCodeDetailsFullResponseDto();


                if (invoiceCodeDetails == null)
                {
                    response.msg = "Invoice details not found.";
                    return response;
                }
                IList<InvoiceCodeTireDetails> invoiceCodeTireDetails = session.QueryOver<InvoiceCodeTireDetails>()
                    .WhereRestrictionOn(x => x.InvoiceCodeDetailId)
                    .IsIn(invoiceCodeDetails.Select(y => y.Id).ToList())
                    .List<InvoiceCodeTireDetails>();
                if (invoiceCodeTireDetails == null)
                {
                    response.msg = "Invoice details not found.";
                    return response;
                }

                var frontLeft = invoiceCodeTireDetails.FirstOrDefault(a => a.Position.ToLower() == "fl");
                var frontRight = invoiceCodeTireDetails.FirstOrDefault(a => a.Position.ToLower() == "fr");
                var BackLeft = invoiceCodeTireDetails.FirstOrDefault(a => a.Position.ToLower() == "bl");
                var BackRight = invoiceCodeTireDetails.FirstOrDefault(a => a.Position.ToLower() == "br");


                invoceCodeDetailsResponseDto.PlateNumber = invoiceCode.PlateNumber;
                invoceCodeDetailsResponseDto.Quantity = invoiceCode.TireQuantity;
                invoceCodeDetailsResponseDto.Position = invoiceCodeDetails.FirstOrDefault().Position;
                invoceCodeDetailsResponseDto.MakeId = invoiceCodeDetails.FirstOrDefault().MakeId;
                invoceCodeDetailsResponseDto.ModelId = invoiceCodeDetails.FirstOrDefault().ModelId;
                invoceCodeDetailsResponseDto.CurrencyCode = new CommonEntityManager().GetDealerCurrencyCodeByDealerId(invoiceCode.DealerId);
                invoceCodeDetailsResponseDto.TireBack = new TireDtls()
                {
                    Cross = BackLeft != null ? BackLeft.CrossSection : "0",
                    Diameter = BackLeft != null ? BackLeft.Diameter : 0,
                    LoadSpeed = BackLeft != null ? BackLeft.LoadSpeed : string.Empty,
                    Wide = BackLeft != null ? BackLeft.Width : "0",
                    SerialLeft = BackLeft != null ? BackLeft.SerialNumber : string.Empty,
                    SerialRight = BackRight != null ? BackRight.SerialNumber : string.Empty,
                    IdLeft = BackRight != null ? BackRight.Id : Guid.Empty,
                    IdRight = BackLeft != null ? BackLeft.Id : Guid.Empty

                };
                invoceCodeDetailsResponseDto.TireFront = new TireDtls()
                {
                    Cross = frontLeft != null ? frontLeft.CrossSection : "0",
                    Diameter = frontLeft != null ? frontLeft.Diameter : 0,
                    LoadSpeed = frontLeft != null ? frontLeft.LoadSpeed : string.Empty,
                    Wide = frontLeft != null ? frontLeft.Width : "0",
                    SerialLeft = frontLeft != null ? frontLeft.SerialNumber : string.Empty,
                    SerialRight = frontRight != null ? frontRight.SerialNumber : string.Empty,
                    IdLeft = frontLeft != null ? frontLeft.Id : Guid.Empty,
                    IdRight = frontRight != null ? frontRight.Id : Guid.Empty
                };

                //everythings is scussess
                response.code = "SUCCESS";
                response.obj = invoceCodeDetailsResponseDto;
            }
            catch (Exception ex)
            {
                response.msg = "An error occurred while reading invoice data.";
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                               ex.InnerException);
            }
            return response;
        }

        public object GetPolicesByCustomerId(Guid CustomerId)
        {
            PolicyforCustomerResponseDto Response = new PolicyforCustomerResponseDto();
            try
            {
                ISession session = EntitySessionManager.GetSession();
                CommonEntityManager cem = new CommonEntityManager();

                PolicyforCustomerResponseDto result = new PolicyforCustomerResponseDto();
                List<PolicyDetailsCustomer> PolicyDetailsCustomer = new List<PolicyDetailsCustomer>();
                //ContractInsuaranceLimitation CIL = session.Query<ContractInsuaranceLimitation>().Where(a => a.ContractId == Contract.Id).FirstOrDefault();
                Customer customer = session.Query<Customer>().Where(a => a.Id == CustomerId).FirstOrDefault();

                List<Policy> policy = session.Query<Policy>().Where(a => a.CustomerId == CustomerId).ToList();

                foreach (var policyData in policy)
                {
                    CommodityType CommodityType = session.Query<CommodityType>().Where(a => a.CommodityTypeId == policyData.CommodityTypeId).FirstOrDefault();
                    Product Product = session.Query<Product>().Where(a => a.Id == policyData.ProductId).FirstOrDefault();

                    VehicleDetailsResponseDto retVehicleDetails = new VehicleDetailsResponseDto();
                    BrownAndWhiteDetailsResponseDto retBrownAndWhiteDetails = new BrownAndWhiteDetailsResponseDto();
                    Make Make = new Make();
                    Model Model = new Model();
                    CommodityCategory CommodityCategory = new CommodityCategory();

                    if (CommodityType.CommodityCode == "A")
                    {
                        VehiclePolicy VehiclePolicy = session.Query<VehiclePolicy>().Where(a => a.PolicyId == policyData.Id).FirstOrDefault();
                        VehicleDetails VehicleDetail = session.Query<VehicleDetails>().Where(a => a.Id == VehiclePolicy.VehicleId).FirstOrDefault();

                        Make = session.Query<Make>().Where(a => a.Id == VehicleDetail.MakeId).FirstOrDefault();
                        Model = session.Query<Model>().Where(a => a.Id == VehicleDetail.ModelId).FirstOrDefault();
                        CommodityCategory = session.Query<CommodityCategory>().Where(a => a.CommodityCategoryId == VehicleDetail.CategoryId).FirstOrDefault();

                    }
                    else if (CommodityType.CommodityCode == "B")
                    {
                        VehiclePolicy VehiclePolicy = session.Query<VehiclePolicy>().Where(a => a.PolicyId == policyData.Id).FirstOrDefault();
                        VehicleDetails VehicleDetail = session.Query<VehicleDetails>().Where(a => a.Id == VehiclePolicy.VehicleId).FirstOrDefault();

                        Make = session.Query<Make>().Where(a => a.Id == VehicleDetail.MakeId).FirstOrDefault();
                        Model = session.Query<Model>().Where(a => a.Id == VehicleDetail.ModelId).FirstOrDefault();
                        CommodityCategory = session.Query<CommodityCategory>().Where(a => a.CommodityCategoryId == VehicleDetail.CategoryId).FirstOrDefault();

                    }
                    else if (CommodityType.CommodityCode == "E")
                    {
                        BAndWPolicy BAndWPolicy = session.Query<BAndWPolicy>().Where(a => a.PolicyId == policyData.Id).FirstOrDefault();
                        BrownAndWhiteDetails BrownAndWhiteDetails = session.Query<BrownAndWhiteDetails>().Where(a => a.Id == BAndWPolicy.BAndWId).FirstOrDefault();


                        Make = session.Query<Make>().Where(a => a.Id == BrownAndWhiteDetails.MakeId).FirstOrDefault();
                        Model = session.Query<Model>().Where(a => a.Id == BrownAndWhiteDetails.ModelId).FirstOrDefault();
                        CommodityCategory = session.Query<CommodityCategory>().Where(a => a.CommodityCategoryId == BrownAndWhiteDetails.CategoryId).FirstOrDefault();

                    }
                    else if (CommodityType.CommodityCode == "Y")
                    {
                        YellowGoodPolicy YellowGoodPolicy = session.Query<YellowGoodPolicy>().Where(a => a.PolicyId == policyData.Id).FirstOrDefault();
                        YellowGoodDetails YellowGoodDetails = session.Query<YellowGoodDetails>().Where(a => a.Id == YellowGoodPolicy.YellowGoodId).FirstOrDefault();

                        Make = session.Query<Make>().Where(a => a.Id == YellowGoodDetails.MakeId).FirstOrDefault();
                        Model = session.Query<Model>().Where(a => a.Id == YellowGoodDetails.ModelId).FirstOrDefault();
                        CommodityCategory = session.Query<CommodityCategory>().Where(a => a.CommodityCategoryId == YellowGoodDetails.CategoryId).FirstOrDefault();

                    }
                    else if (CommodityType.CommodityCode == "O")
                    {
                        OtherItemPolicy OtherItemPolicy = session.Query<OtherItemPolicy>().Where(a => a.PolicyId == policyData.Id).FirstOrDefault();
                        OtherItemDetails OtherItemDetails = session.Query<OtherItemDetails>().Where(a => a.Id == OtherItemPolicy.OtherItemId).FirstOrDefault();

                        Make = session.Query<Make>().Where(a => a.Id == OtherItemDetails.MakeId).FirstOrDefault();
                        Model = session.Query<Model>().Where(a => a.Id == OtherItemDetails.ModelId).FirstOrDefault();
                        CommodityCategory = session.Query<CommodityCategory>().Where(a => a.CommodityCategoryId == OtherItemDetails.CategoryId).FirstOrDefault();
                    }

                    Contract Contract = session.Query<Contract>().Where(a => a.Id == policyData.ContractId).FirstOrDefault();
                    ReinsurerContract ReinsurerContract = session.Query<ReinsurerContract>().Where(a => a.Id == Contract.ReinsurerContractId).FirstOrDefault();

                    ManufacturerWarrantyDetails mwd = session.Query<ManufacturerWarrantyDetails>().Where(a => a.ModelId == Model.Id &&
                                                        a.CountryId == ReinsurerContract.CountryId).FirstOrDefault();

                    ManufacturerWarranty ManufacturerWarranty = null;
                    ManufacturerWarrantyResponseDto ManufacturerWarrantyResponseDto = new ManufacturerWarrantyResponseDto();

                    if (CommodityType.CommodityCode != "O")
                    {
                        ManufacturerWarranty = session.Query<ManufacturerWarranty>().Where(a => a.Id == mwd.ManufacturerWarrantyId && a.MakeId == Make.Id).FirstOrDefault();

                        ManufacturerWarrantyResponseDto = new ManufacturerWarrantyResponseDto()
                        {
                            ApplicableFrom = ManufacturerWarranty.ApplicableFrom,
                            WarrantyKm = ManufacturerWarranty.IsUnlimited ? "Unlimited" : ManufacturerWarranty.WarrantyKm.ToString(),
                            WarrantyMonths = ManufacturerWarranty.WarrantyMonths,
                            WarrantyName = ManufacturerWarranty.WarrantyName,
                            Id = ManufacturerWarranty.Id,
                            IsUnlimited = ManufacturerWarranty.IsUnlimited
                        };

                        ContractInsuaranceLimitation CIL = session.Query<ContractInsuaranceLimitation>().Where(a => a.ContractId == Contract.Id).FirstOrDefault();
                        InsuaranceLimitation InsuaranceLimitation = session.Query<InsuaranceLimitation>().Where(a => a.CommodityTypeId == policyData.CommodityTypeId && a.Id == CIL.InsuaranceLimitationId).FirstOrDefault();
                        ContractExtensions ContractExtensions = session.Query<ContractExtensions>().Where(a => a.ContractInsuanceLimitationId == CIL.Id).FirstOrDefault();



                        PolicyDetailsCustomer p = new PolicyDetailsCustomer()
                        {

                            Id = policyData.Id,
                            PolicyNo = policyData.PolicyNo,
                            ProductName = Product.Productname,
                            ProductId = policyData.ProductId,
                            PolicyStartDate = ManufacturerWarranty == null || !policyData.MWIsAvailable ? policyData.PolicySoldDate : policyData.PolicyStartDate.AddDays(+1),
                            PolicyEndDate = policyData.PolicyEndDate,
                            ExtMonths = InsuaranceLimitation == null ? 0 : InsuaranceLimitation.Months,
                            ExtKM = InsuaranceLimitation == null ? 0 : InsuaranceLimitation.Km,
                            ExtStartDate = policyData.MWIsAvailable ? policyData.MWStartDate.AddMonths(ManufacturerWarranty.WarrantyMonths)
                                : policyData.PolicySoldDate,
                            ExtEndDate = policyData.MWIsAvailable ? policyData.MWStartDate.AddMonths(ManufacturerWarranty.WarrantyMonths)
                                    .AddMonths(InsuaranceLimitation == null ? 0 : InsuaranceLimitation.Months).AddDays(-1)
                                : policyData.PolicySoldDate.AddMonths(InsuaranceLimitation == null ? 0
                                : InsuaranceLimitation.Months).AddDays(-1),
                        };
                        PolicyDetailsCustomer.Add(p);

                    }
                    else
                    {
                        ContractInsuaranceLimitation CIL = session.Query<ContractInsuaranceLimitation>().Where(a => a.ContractId == Contract.Id).FirstOrDefault();
                        InsuaranceLimitation InsuaranceLimitation = session.Query<InsuaranceLimitation>().Where(a => a.CommodityTypeId == policyData.CommodityTypeId && a.Id == CIL.InsuaranceLimitationId).FirstOrDefault();
                        ContractExtensions ContractExtensions = session.Query<ContractExtensions>().Where(a => a.ContractInsuanceLimitationId == CIL.Id).FirstOrDefault();

                        InsuaranceLimitationResponseDto InsuaranceLimitationResponseDto = new InsuaranceLimitationResponseDto()
                        {

                            CommodityTypeCode = InsuaranceLimitation.CommodityTypeCode,
                            CommodityTypeId = InsuaranceLimitation.CommodityTypeId,
                            Hrs = InsuaranceLimitation.Hrs,
                            Id = InsuaranceLimitation.Id,
                            InsuaranceLimitationName = InsuaranceLimitation.InsuaranceLimitationName,
                            Km = InsuaranceLimitation.Km,
                            Months = InsuaranceLimitation.Months,
                            TopOfMW = InsuaranceLimitation.TopOfMW,
                            IsRsa = InsuaranceLimitation.IsRsa
                        };

                        PolicyDetailsCustomer p = new PolicyDetailsCustomer()
                        {

                            Id = policyData.Id,
                            PolicyNo = policyData.PolicyNo,
                            ProductName = Product.Productname,
                            ProductId = policyData.ProductId,
                            PolicyStartDate = ManufacturerWarranty == null || !policyData.MWIsAvailable ? policyData.PolicySoldDate : policyData.PolicyStartDate.AddDays(+1),
                            PolicyEndDate = policyData.PolicyEndDate,
                            //ExtMonths = InsuaranceLimitation == null ? 0 : InsuaranceLimitation.Months,
                            //ExtKM = InsuaranceLimitation == null ? 0 : InsuaranceLimitation.Km,
                            //ExtStartDate = policyData.MWIsAvailable ? policyData.MWStartDate.AddMonths(ManufacturerWarranty.WarrantyMonths)
                            //    : policyData.PolicySoldDate,
                            //ExtEndDate = policyData.MWIsAvailable ? policyData.MWStartDate.AddMonths(ManufacturerWarranty.WarrantyMonths)
                            //        .AddMonths(InsuaranceLimitation == null ? 0 : InsuaranceLimitation.Months).AddDays(-1)
                            //    : policyData.PolicySoldDate.AddMonths(InsuaranceLimitation == null ? 0
                            //    : InsuaranceLimitation.Months).AddDays(-1),
                        };
                        PolicyDetailsCustomer.Add(p);
                    }
                }
                return PolicyDetailsCustomer;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ((ex.InnerException != null) ? ex.InnerException.ToString() : ""));
                return "";
            }
        }






    }
}
