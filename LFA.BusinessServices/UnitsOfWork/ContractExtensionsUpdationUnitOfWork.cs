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
    internal sealed class ContractExtensionsUpdationUnitOfWork : UnitOfWork
    {
        public ContractExtensionsRequestDto ContractExtensions;
        public ContractExtensionsResponseDto ExPr;

        public ContractExtensionsUpdationUnitOfWork(ContractExtensionsRequestDto ContractExtensions)
        {

            this.ContractExtensions = ContractExtensions;
        }
        //public override bool PreExecute()
        //{
        //    try
        //    {
        //        EntitySessionManager.OpenSession();
        //        ContractExtensionsEntityManager ContractExtensionsEntityManager = new ContractExtensionsEntityManager();
        //        var ce = ContractExtensionsEntityManager.GetContractExtensionsById(ContractExtensions.Id);
        //        if (ce.IsContractExtensionsExists == true)
        //        {
        //            ContractExtensions.EntryDateTime = ce.EntryDateTime;
        //            ContractExtensions.EntryUser = ce.EntryUser;
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
                            ContractExtensionsEntityManager ContractExtensionsEntityManager = new ContractExtensionsEntityManager();
                            var ce = ContractExtensionsEntityManager.GetContractExtensionsById(ContractExtensions.Id);
                            if (ce.IsContractExtensionsExists == true)
                            {
                                ContractExtensions.EntryDateTime = ce.EntryDateTime;
                                ContractExtensions.EntryUser = ce.EntryUser;
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
                ContractExtensionsEntityManager ContractExtensionsEntityManager = new ContractExtensionsEntityManager();
                //var ce = ContractExtensionsEntityManager.GetPremiumSetupById(PremiumSetup.Id);
                //if (ce.IsPremiumSetupExists == true)
                //{
               // bool result = ContractExtensionsEntityManager.UpdateContractExtensions(ContractExtensions);
                //    this.ContractExtensions.ContractExtensionsInsertion = result;
                //}
                //else
                //{
                //    this.PremiumSetup.ContractExtensionsInsertion = false;
                //}

            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
        }
    }
}
