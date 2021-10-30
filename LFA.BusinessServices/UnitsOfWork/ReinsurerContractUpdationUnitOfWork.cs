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
    internal sealed class ReinsurerContractUpdationUnitOfWork : UnitOfWork
    {
        public ReinsurerContractRequestDto ReinsurerContract;
        public ReinsurerContractResponseDto ExPr;
       
        public ReinsurerContractUpdationUnitOfWork(ReinsurerContractRequestDto ReinsurerContract)
        {
            
            this.ReinsurerContract = ReinsurerContract;
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
                            ReinsurerContractEntityManager ReinsurerContractEntityManager = new ReinsurerContractEntityManager();
                            var ce = ReinsurerContractEntityManager.GetReinsurerContractById(ReinsurerContract.Id);
                            if (ce.IsReinsurerContractExists == true)
                            {
                                ReinsurerContract.EntryDateTime = ce.EntryDateTime;
                                ReinsurerContract.EntryUser = ce.EntryUser;
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
        //        ReinsurerContractEntityManager ReinsurerContractEntityManager = new ReinsurerContractEntityManager();
        //        var ce = ReinsurerContractEntityManager.GetReinsurerContractById(ReinsurerContract.Id);
        //        if (ce.IsReinsurerContractExists == true)
        //        {
        //            ReinsurerContract.EntryDateTime = ce.EntryDateTime;
        //            ReinsurerContract.EntryUser = ce.EntryUser;
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
                ReinsurerContractEntityManager ReinsurerContractEntityManager = new ReinsurerContractEntityManager();
                //var ce = ReinsurerContractEntityManager.GetReinsurerContractById(ReinsurerContract.Id);
                //if (ce.IsReinsurerContractExists == true)
                //{
                    bool result = ReinsurerContractEntityManager.UpdateReinsurerContract(ReinsurerContract);
                    this.ReinsurerContract.ReinsurerContractInsertion = result;
                //}
                //else
                //{
                //    this.ReinsurerContract.ReinsurerContractInsertion = false;
                //}

            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
        }
    }
}
