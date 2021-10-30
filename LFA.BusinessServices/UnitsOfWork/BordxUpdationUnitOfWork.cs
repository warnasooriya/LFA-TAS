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
    internal sealed class BordxUpdationUnitOfWork : UnitOfWork
    {
        public BordxRequestDto Bordx;
        public BordxResponseDto ExPr;
       
        public BordxUpdationUnitOfWork(BordxRequestDto Bordx)
        {
            
            this.Bordx = Bordx;
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
                            BordxEntityManager BordxEntityManager = new BordxEntityManager();
                            var ce = BordxEntityManager.GetBordxById(Bordx.Id);
                            if (ce.IsBordxExists == true)
                            {
                                Bordx.EntryDateTime = ce.EntryDateTime;
                                Bordx.EntryUser = ce.EntryUser;
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
        //        BordxEntityManager BordxEntityManager = new BordxEntityManager();
        //        var ce = BordxEntityManager.GetBordxById(Bordx.Id);
        //        if (ce.IsBordxExists == true)
        //        {
        //            Bordx.EntryDateTime = ce.EntryDateTime;
        //            Bordx.EntryUser = ce.EntryUser;
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
                BordxEntityManager BordxEntityManager = new BordxEntityManager();
                //var ce = BordxEntityManager.GetBordxById(Bordx.Id);
                //if (ce.IsBordxExists == true)
                //{
                    bool result = BordxEntityManager.UpdateBordx(Bordx);
                    this.Bordx.BordxInsertion = result;
                //}
                //else
                //{
                //    this.Bordx.BordxInsertion = false;
                //}

            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
        }
    }
}
