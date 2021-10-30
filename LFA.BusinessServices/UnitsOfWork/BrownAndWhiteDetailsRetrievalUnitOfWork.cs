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

using TAS.Services.Common;

namespace TAS.Services.UnitsOfWork
{
    internal sealed class BrownAndWhiteDetailsRetrievalUnitOfWork : UnitOfWork
    {

        public Guid BrownAndWhiteDetailsId;
        public BrownAndWhiteDetailsResponseDto Result
        {
            get;
            private set;
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
                BrownAndWhiteDetailsEntityManager BrownAndWhiteDetailsEntityManager = new BrownAndWhiteDetailsEntityManager();
                BrownAndWhiteDetailsResponseDto BrownAndWhiteDetailsEntity = BrownAndWhiteDetailsEntityManager.GetBrownAndWhiteDetailsById(BrownAndWhiteDetailsId);
                if (BrownAndWhiteDetailsEntity == null)
                    BrownAndWhiteDetailsEntity = new BrownAndWhiteDetailsResponseDto();
                if (BrownAndWhiteDetailsEntity.IsBrownAndWhiteDetailsExists == null || BrownAndWhiteDetailsEntity.IsBrownAndWhiteDetailsExists== false)
                {
                    BrownAndWhiteDetailsEntity.IsBrownAndWhiteDetailsExists = false;
                }
                else
                {
                    BrownAndWhiteDetailsEntity.IsBrownAndWhiteDetailsExists = true;
                }

                this.Result = BrownAndWhiteDetailsEntity;
            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
        }

    }
}
