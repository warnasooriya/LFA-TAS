using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAS.DataTransfer.Requests;
using TAS.Services.Common;
using TAS.Services.Entities.Management;
using TAS.Services.Entities.Persistence;

namespace TAS.Services.UnitsOfWork
{
    internal sealed class ContractsForSearchGridRetrievalUnotOfWork : UnitOfWork
    {
        private readonly ContractSearchGridRequestDto ContractSearchGridRequest;
        public Object Result;
        public ContractsForSearchGridRetrievalUnotOfWork(ContractSearchGridRequestDto ContractSearchGridRequestDto)
        {
            ContractSearchGridRequest = ContractSearchGridRequestDto;
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
                            if (ContractSearchGridRequest.contractSearchGridSearchCriterias != null &&
                                ContractSearchGridRequest.paginationOptionsContractSearchGrid != null)
                            {
                                return true;
                            }
                        }
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
                Result = ContractEntityManager.GetContractsForSearchGrid(ContractSearchGridRequest.contractSearchGridSearchCriterias, ContractSearchGridRequest.paginationOptionsContractSearchGrid);

            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
        }
    }
}
