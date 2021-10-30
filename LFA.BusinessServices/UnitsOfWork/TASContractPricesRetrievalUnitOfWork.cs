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


namespace TAS.Services.UnitsOfWork
{
    internal sealed class TASContractPricesRetrievalUnitOfWork : UnitOfWork
    {


        public ContractPricesResponseDto Result
        {
            get;
            private set;
        }
        public Guid tpaId
        {
            get;
            set;
        }
        public Guid dealerId
        {
            get;
            set;
        }
        public Guid modelId
        {
            get;
            set;
        }
        public Guid extensionTypeId
        {
            get;
            set;
        }
        public double dealerPrice
        {
            get;
            set;
        }
        public double itemPrice
        {
            get;
            set;
        }

        public TASContractPricesRetrievalUnitOfWork(Guid modelId, Guid dealerId, Guid tpaId, decimal dealerPrice, decimal itemPrice) //, Guid extensionTypeId
        {
            this.tpaId = tpaId;
            this.dealerId = dealerId;
            this.modelId = modelId;
            //this.extensionTypeId = extensionTypeId;
            this.dealerPrice = Convert.ToDouble( dealerPrice);
            this.itemPrice = Convert.ToDouble( itemPrice);
        }

        public override bool PreExecute()
        {
            try
            {

                TASEntitySessionManager.OpenSession();
                string dbName = TASTPAEntityManager.GetTPADetailById(tpaId).FirstOrDefault().DBName;
                if (dbName != "")
                {

                    //string tasTpaConnString = TASTPAEntityManager.GetTPAViewOnlyConnStringBydbName(dbName);
                    string tasTpaConnString = TASTPAEntityManager.GetTPADetailById(tpaId).FirstOrDefault().DBConnectionStringViewOnly;

                    if (tasTpaConnString != "")
                    {
                        dbConnectionString = tasTpaConnString;
                        return true;
                    }
                }
                return false;

            }
            catch (Exception e)
            {
                return false;
            }
            finally
            {
                TASEntitySessionManager.CloseSession();
            }
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
                List<ContractPriceResponseDto> ContractPrices = ContractEntityManager.GetPrices(this.modelId, this.dealerId); //, this.extensionTypeId


                ContractPricesResponseDto result = new ContractPricesResponseDto();
                result.ContractPrices = new List<ContractPriceResponseDto>();
                foreach (var ContractPrice in ContractPrices)
                {
                   if (ContractPrice.PremiumBasedon == "UE") {
                       ContractPrice.price = Math.Round((ContractPrice.PremiumTotal),2);
                            }
                            else if (ContractPrice.PremiumBasedon == "FV") {
                                ContractPrice.price = Math.Round((ContractPrice.PremiumTotal),2);
                            }
                            else if (ContractPrice.PremiumBasedon == "RP") {
                               var premiumVal = Math.Round( (ContractPrice.PremiumTotal * this.itemPrice / 100.0),2);
                                if (premiumVal < ContractPrice.Min)
                                {
                                    ContractPrice.price = ContractPrice.Min;
                                }
                                else if (premiumVal > ContractPrice.Max)
                                {
                                    ContractPrice.price = ContractPrice.Max;
                                }
                            }
                            else if (ContractPrice.PremiumBasedon == "DP") {
                                var premiumVal = Math.Round((ContractPrice.PremiumTotal * this.dealerPrice / 100.0),2);
                                if (premiumVal < ContractPrice.Min)
                                {
                                    ContractPrice.price = ContractPrice.Min;
                                }
                                else if (premiumVal > ContractPrice.Max)
                                {
                                    ContractPrice.price = ContractPrice.Max;
                                }
                            }
					
                    //need to write other fields
                    result.ContractPrices.Add(ContractPrice);
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
