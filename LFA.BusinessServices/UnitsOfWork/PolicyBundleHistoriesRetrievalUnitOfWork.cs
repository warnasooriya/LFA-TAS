using TAS.Services.Entities.Management;
using TAS.DataTransfer.Common;
using TAS.DataTransfer.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAS.Services.Entities.Persistence;
using TAS.Services.Entities;

using TAS.Services.Common;

namespace TAS.Services.UnitsOfWork
{
	internal sealed class PolicyBundleHistoriesRetrievalUnitOfWork : UnitOfWork
	{
	   

		public PolicyBundleHistoriesResponseDto Result
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
				List<PolicyBundleHistory> PolicyEntities = PolicyEntityManager.GetPolicyBundleHistories();

				
				PolicyBundleHistoriesResponseDto result = new PolicyBundleHistoriesResponseDto();
				result.Policies = new List<PolicyBundleHistoryResponseDto>();
				foreach (var Policy in PolicyEntities)
				{
					PolicyBundleHistoryResponseDto pr = new PolicyBundleHistoryResponseDto();

					pr.Id = Policy.Id;
					pr.Comment = Policy.Comment;
					pr.CommodityTypeId = Policy.CommodityTypeId;
					pr.ContractId = Policy.ContractId;
					pr.CoverTypeId = Policy.CoverTypeId;
					pr.PremiumCurrencyTypeId = Policy.PremiumCurrencyTypeId;
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
					pr.RefNo = Policy.RefNo;
					pr.SalesPersonId = Policy.SalesPersonId ;
					pr.EntryDateTime = Policy.EntryDateTime;
					pr.EntryUser = Policy.EntryUser;
					pr.IsApproved = Policy.IsApproved;
					pr.Discount = Policy.Discount;
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
