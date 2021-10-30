using TAS.DataTransfer.Requests;
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
    internal sealed class ReinsurerInsertionUnitOfWork : UnitOfWork
    {
        public ReinsurerRequestDto Reinsurer;
       
        public ReinsurerInsertionUnitOfWork(ReinsurerRequestDto Reinsurer)
        {
            
            this.Reinsurer = Reinsurer;
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
                ReinsurerEntityManager ReinsurerEntityManager = new ReinsurerEntityManager();
                var ce = ReinsurerEntityManager.GetReinsurerById(Reinsurer.Id);
                if (ce.IsReinsurerExists == false)
                {
                    bool result = ReinsurerEntityManager.AddReinsurer(Reinsurer);
                    this.Reinsurer.ReinsurerInsertion = result;
                }
                else
                {
                    this.Reinsurer.ReinsurerInsertion = false;
                }

            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
        }
    }
}
