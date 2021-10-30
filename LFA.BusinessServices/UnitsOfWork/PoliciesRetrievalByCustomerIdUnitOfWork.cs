using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAS.DataTransfer.Responses;
using TAS.Services.Common;
using TAS.Services.Entities;
using TAS.Services.Entities.Management;
using TAS.Services.Entities.Persistence;

namespace TAS.Services.UnitsOfWork
{
    internal sealed class PoliciesRetrievalByCustomerIdUnitOfWork : UnitOfWork
    {
        public Guid customerid;
        public PoliciesResponseDto Result
        {
            get;
            private set;
        }
        public override bool PreExecute()
        {
            try
            {
                Common.JWTHelper JWTHelper = new Common.JWTHelper(SecurityContext);
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
                        if (JWTHelper.checkTokenValidity(Convert.ToInt32(ConfigurationData.tasTokenValidTime.ToString())))
                        {
                            return true;
                        }
                        EntitySessionManager.CloseSession();
                    }
                }
            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
            return false;

        }
        public override void Execute()
        {

            try
            {
                //temp section can remove  //**  lines once preexecute jwt and db name working fine
                if (dbConnectionString != null)   //**
                {     //**
                    EntitySessionManager.OpenSession(dbConnectionString);
                }     //**
                else     //**
                {     //**
                    EntitySessionManager.OpenSession();     //**
                }     //**
                PolicyEntityManager PolicyEntityManager = new PolicyEntityManager();
                List<PolicyInfo> PolicyEntities = PolicyEntityManager.GetPolicysByCustomerId(customerid);
                CurrencyEntityManager cem = new CurrencyEntityManager();
                PoliciesResponseDto result = new PoliciesResponseDto();
                result.Policies = new List<PolicyResponseDto>();
                foreach (var Policy in PolicyEntities)
                {
                    PolicyResponseDto pr = new PolicyResponseDto();

                    pr.Id = Policy.Id;
                    pr.tpaBranchId = Policy.TPABranchId;
                    pr.Comment = Policy.Comment;
                    pr.CommodityTypeId = Policy.CommodityTypeId;
                    pr.ContractId = Policy.ContractId;
                    pr.CoverTypeId = Policy.CoverTypeId;
                    pr.PremiumCurrencyTypeId = Policy.PremiumCurrencyTypeId;
                    pr.DealerPaymentCurrencyTypeId = Policy.DealerPaymentCurrencyTypeId;
                    pr.CustomerPaymentCurrencyTypeId = Policy.CustomerPaymentCurrencyTypeId;
                    pr.CustomerId = Policy.CustomerId;
                    pr.CustomerPayment = cem.ConvertFromBaseCurrency(Policy.CustomerPayment, Policy.PremiumCurrencyTypeId, Policy.CurrencyPeriodId);
                    pr.DealerId = Policy.DealerId;
                    pr.DealerLocationId = Policy.DealerLocationId;
                    pr.DealerPayment = cem.ConvertFromBaseCurrency(Policy.DealerPayment, Policy.PremiumCurrencyTypeId, Policy.CurrencyPeriodId);
                    pr.ExtensionTypeId = Policy.ExtensionTypeId;
                    pr.HrsUsedAtPolicySale = Policy.HrsUsedAtPolicySale;
                    pr.IsPartialPayment = Policy.IsPartialPayment;
                    pr.IsPreWarrantyCheck = Policy.IsPreWarrantyCheck;
                    pr.IsSpecialDeal = Policy.IsSpecialDeal;
                    pr.ItemId = Policy.ItemId;
                    pr.ProductId = Policy.ProductId;
                    pr.PaymentModeId = Policy.PaymentModeId;
                    pr.PolicyNo = Policy.PolicyNo;
                    pr.PolicySoldDate = Policy.PolicySoldDate;
                    pr.Premium = cem.ConvertFromBaseCurrency(Policy.Premium, Policy.PremiumCurrencyTypeId, Policy.CurrencyPeriodId);
                    pr.RefNo = Policy.RefNo;
                    pr.SalesPersonId = Policy.SalesPersonId;
                    pr.EntryDateTime = Policy.EntryDateTime;
                    pr.EntryUser = Policy.EntryUser;
                    pr.Type = Policy.Type;
                    pr.IsApproved = Policy.IsApproved;
                    pr.PolicyStartDate = Policy.PolicyStartDate;
                    pr.PolicyEndDate = Policy.PolicyEndDate;
                    pr.Discount = Policy.Discount;
                    pr.PolicyBundleId = Policy.PolicyBundleId;
                    pr.TransferFee = Policy.TransferFee;
                    pr.ForwardComment = Policy.ForwardComment;
                    pr.BordxId = Policy.BordxId;
                    pr.Month = Policy.Month;
                    pr.Year = Policy.Year;
                    pr.DealerPolicy = Policy.DealerPolicy;

                    result.Policies.Add(pr);
                }
                this.Result = result;
            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
        }
    }
}
