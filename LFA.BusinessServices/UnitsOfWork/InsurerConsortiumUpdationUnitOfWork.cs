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
    internal sealed class InsurerConsortiumUpdationUnitOfWork : UnitOfWork
    {
        public InsurerConsortiumRequestDto InsurerConsortium;
        public InsurerConsortiumResponseDto ExPr;
       
        public InsurerConsortiumUpdationUnitOfWork(InsurerConsortiumRequestDto InsurerConsortium)
        {
            
            this.InsurerConsortium = InsurerConsortium;
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
                            InsurerEntityManager InsurerEntityManager = new InsurerEntityManager();
                            var ce = InsurerEntityManager.GetInsurerConsortiumById(InsurerConsortium.Id);
                            if (ce.IsInsurerConsortiumExists == true)
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
        //        InsurerEntityManager InsurerEntityManager = new InsurerEntityManager();
        //        var ce = InsurerEntityManager.GetInsurerConsortiumById(InsurerConsortium.Id);
        //        if (ce.IsInsurerConsortiumExists == true)
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
                InsurerEntityManager InsurerEntityManager = new InsurerEntityManager();
                //var ce = InsurerEntityManager.GetInsurerConsortiumById(InsurerConsortium.Id);
                //if (ce.IsInsurerConsortiumExists == true)
                //{
                    bool result = InsurerEntityManager.UpdateInsurerConsortium(InsurerConsortium);
                    this.InsurerConsortium.InsurerConsortiumInsertion = result;
                //}
                //else
                //{
                //    this.InsurerConsortium.InsurerConsortiumInsertion = false;
                //}

            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
        }
    }
}
