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
    internal sealed class PartRejectionTypeUpdationUnitOfWork : UnitOfWork
    {
        public PartRejectionTypeRequestDto PartRejectionType;
        public PartRejectionTypesResponseDto ExPr;
        private string UniqueDbName = string.Empty;

        public PartRejectionTypeUpdationUnitOfWork(PartRejectionTypeRequestDto PartRejectionType)
        {

            this.PartRejectionType = PartRejectionType;
        }
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
                            ClaimEntityManager claimEntityManager = new ClaimEntityManager();
                            var ce = claimEntityManager.GetPartRejectionTypeById(PartRejectionType.Id);
                            if (ce.IsPartRejectionTypesExists == true)
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
                ClaimEntityManager claimEntityManager = new ClaimEntityManager();

                bool result = claimEntityManager.UpdatePartRejectioDescription(PartRejectionType);
                this.PartRejectionType.PartRejectionTypeInsertion = result;
                if (result)
                {
                    /* remove cache */
                    ICache cache = CacheFactory.GetCache();
                    cache.Remove(UniqueDbName + "_PartRejectionType");
                }

            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
        }
    }
}
