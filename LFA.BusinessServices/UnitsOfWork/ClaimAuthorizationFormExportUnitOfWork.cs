﻿using System;
using System.Collections.Generic;
using System.Linq;
using TAS.Services.Common;
using TAS.Services.Entities.Management;
using TAS.Services.Entities.Persistence;

namespace TAS.Services.UnitsOfWork
{
    internal sealed class ClaimAuthorizationFormExportUnitOfWork : UnitOfWork
    {
        public object Result { get; set; }
        private readonly Guid _loggedInUserId, _claimId;
        private string tpaName;
        public ClaimAuthorizationFormExportUnitOfWork(Guid loggedInUserId, Guid claimId)
        {
            _loggedInUserId = loggedInUserId;
            _claimId = claimId;

        }

        public override bool PreExecute()
        {
            try
            {
                Common.JWTHelper JWTHelper = new Common.JWTHelper(SecurityContext);
                Dictionary<string, object> str = JWTHelper.DecodeAuthenticationToken();
                string dbName = tpaName = str.FirstOrDefault(f => f.Key == "dbName").Value.ToString();
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

                this.Result = ClaimEntityManager.getClaimAuthorizationFormforCycle(_loggedInUserId, _claimId, tpaName, dbConnectionString);
            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
        }
    }
}

 
