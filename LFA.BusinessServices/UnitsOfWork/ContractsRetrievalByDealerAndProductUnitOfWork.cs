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
    internal sealed class ContractsRetrievalByDealerAndProductUnitOfWork : UnitOfWork
    {


        public ContractsResponseDto Result
        {
            get;
            private set;
        }

        public Guid dealerId { get; set; }
        public Guid productId { get; set; }
        public ContractsRetrievalByDealerAndProductUnitOfWork(Guid dealerId , Guid productId) {
            this.dealerId = dealerId;
            this.productId = productId;
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
                ContractEntityManager ContractEntityManager = new ContractEntityManager();
                List<Contract> ContractEntities = ContractEntityManager.GetContractsByDealerAndProduct(this.dealerId , this.productId);

                CurrencyEntityManager currencyEntityManager = new CurrencyEntityManager();
                ContractsResponseDto result = new ContractsResponseDto();
                result.Contracts = new List<ContractResponseDto>();

                ContractExtensions contractExtension = ContractEntityManager.GetFirstContractExtensionByContractId(ContractEntities[0].Id);
                foreach (var Contract in ContractEntities)
                {

                    if(contractExtension==null)
                        continue;
                    ContractResponseDto contract = new ContractResponseDto();

                    contract.Id = Contract.Id;
                    contract.IsAutoRenewal = Contract.IsAutoRenewal;
                    contract.DiscountAvailable = Contract.DiscountAvailable;
                    contract.IsPromotional = Contract.IsPromotional;
                    contract.CommodityTypeId = Contract.CommodityTypeId;
                    contract.CountryId = Contract.CountryId;
                    contract.DealName = Contract.DealName;
                    contract.DealerId = Contract.DealerId;
                    contract.DealType = Contract.DealType;
                    contract.EndDate = Contract.EndDate;
                 //   contract.ItemStatusId = Contract.ItemStatusId;
                    contract.LinkDealId = Contract.LinkDealId;
                    contract.ProductId = Contract.ProductId;
                    contract.Remark = Contract.Remark;
                    contract.StartDate = Contract.StartDate;
                    contract.InsurerId = Contract.InsurerId;
                   // contract.ReinsurerId = Contract.ReinsurerId;
                    contract.IsActive = Contract.IsActive;
                    contract.EntryDateTime = Contract.EntryDateTime;
                    contract.EntryUser = Contract.EntryUser;
                   // contract.PremiumTotal = Contract.PremiumTotal;
                  //  contract.GrossPremium = Contract.GrossPremium;
                  //  contract.ClaimLimitation = currencyEntityManager.ConvertFromBaseCurrency(Contract.ClaimLimitation, contractExtension.PremiumCurrencyId, contractExtension.CurrencyPeriodId);
                  //  contract.LiabilityLimitation = currencyEntityManager.ConvertFromBaseCurrency(Contract.LiabilityLimitation, contractExtension.PremiumCurrencyId, contractExtension.CurrencyPeriodId);
                    contract.CommodityUsageTypeId = Contract.CommodityUsageTypeId;

                    //need to write other fields
                    result.Contracts.Add(contract);
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
