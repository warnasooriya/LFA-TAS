using TAS.DataTransfer.Requests;
using TAS.DataTransfer.Responses;
using TAS.Services.Entities.Management;
using TAS.Services.Entities.Persistence;
using TAS.Services.UnitsOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAS.Services.Common;

namespace TAS.Services.UnitsOfWork
{
    internal sealed class ContractTaxesUpdationUnitOfWork : UnitOfWork
    {
        public ContractTaxesRequestDto ContractTaxes;
        public ContractTaxesResponseDto ExPr;
       
        public ContractTaxesUpdationUnitOfWork(ContractTaxesRequestDto ContractTaxes)
        {
            
            this.ContractTaxes = ContractTaxes;
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
                            ContractEntityManager ContractEntityManager = new ContractEntityManager();
                            var ce = ContractEntityManager.GetContractTaxesById(ContractTaxes.Id);
                            if (ce.IsContractTaxesExists == true)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
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
       
        //public override bool PreExecute()
        //{
        //    try
        //    {
        //        EntitySessionManager.OpenSession();
        //        ContractEntityManager ContractEntityManager = new ContractEntityManager();
        //        var ce = ContractEntityManager.GetContractTaxesById(ContractTaxes.Id);
        //        if (ce.IsContractTaxesExists == true)
        //        {
        //            return true;
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //    finally
        //    {
        //        EntitySessionManager.CloseSession();
                
        //    }
            
        //}

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
                //var ce = ContractEntityManager.GetContractTaxesById(ContractTaxes.Id);
                //if (ce.IsContractTaxesExists == true)
                //{
                    bool result = ContractEntityManager.UpdateContractTaxes(ContractTaxes);
                    this.ContractTaxes.ContractTaxesInsertion = result;
                //}
                //else
                //{
                //    this.ContractTaxes.ContractTaxesInsertion = false;
                //}

            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
        }
    }
}
