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
using TAS.Caching;

namespace TAS.Services.UnitsOfWork
{
    internal sealed class BrownAndWhiteDetailsUpdationUnitOfWork : UnitOfWork
    {
        public BrownAndWhiteDetailsRequestDto BrownAndWhiteDetails;
        public BrownAndWhiteDetailsResponseDto ExPr;
        private string UniqueDbName = string.Empty;

        public BrownAndWhiteDetailsUpdationUnitOfWork(BrownAndWhiteDetailsRequestDto BrownAndWhiteDetails)
        {

            this.BrownAndWhiteDetails = BrownAndWhiteDetails;
        }
        //public override bool PreExecute()
        //{
        //    try
        //    {
        //        EntitySessionManager.OpenSession();
        //        BrownAndWhiteDetailsEntityManager brownAndWhiteDetailsEntityManager = new BrownAndWhiteDetailsEntityManager();
        //        var ce = brownAndWhiteDetailsEntityManager.GetBrownAndWhiteDetailsById(BrownAndWhiteDetails.Id);
        //        if (ce.IsBrownAndWhiteDetailsExists == true)
        //        {
        //            BrownAndWhiteDetails.EntryDateTime = ce.EntryDateTime;
        //            BrownAndWhiteDetails.EntryUser = ce.EntryUser;
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
                string dbName = UniqueDbName = str.FirstOrDefault(f => f.Key == "dbName").Value.ToString();
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
                }
                BrownAndWhiteDetailsEntityManager brownAndWhiteDetailsEntityManager = new BrownAndWhiteDetailsEntityManager();
                //var ce = brownAndWhiteDetailsEntityManager.GetBrownAndWhiteDetailsById(BrownAndWhiteDetails.Id);
                //if (ce.IsBrownAndWhiteDetailsExists == true)
                //{
                bool result = brownAndWhiteDetailsEntityManager.UpdateBrownAndWhiteDetails(BrownAndWhiteDetails);
                if (result) {
                    /* remove cache */
                    ICache cache = CacheFactory.GetCache();
                    cache.Remove(UniqueDbName + "_BrownAndWhiteDetails");
                }
                    this.BrownAndWhiteDetails.BrownAndWhiteDetailsInsertion = result;
                //}
                //else
                //{
                //    this.BrownAndWhiteDetails.BrownAndWhiteDetailsInsertion = false;
                //}

            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
        }
    }
}
