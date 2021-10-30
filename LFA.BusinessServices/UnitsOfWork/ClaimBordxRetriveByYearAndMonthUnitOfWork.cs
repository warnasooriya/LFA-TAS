﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAS.Services.Common;
using TAS.Services.Entities.Management;
using TAS.Services.Entities.Persistence;

namespace TAS.Services.UnitsOfWork
{
    internal sealed class ClaimBordxRetriveByYearAndMonthUnitOfWork : UnitOfWork
    {
        public int year;
        public int month;
        public Guid insurerid;
        public Guid reinsurerid;
        public object result;

        public ClaimBordxRetriveByYearAndMonthUnitOfWork(int Year, int Month, Guid Insurerid, Guid Reinsurerid)
        {
            this.year = Year;
            this.month = Month;
            this.reinsurerid = Reinsurerid;
            this.insurerid = Insurerid;
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
                ClaimBordxProcessEntityManager ClaimBordxProcessEntityManager = new ClaimBordxProcessEntityManager();
                this.result = ClaimBordxProcessEntityManager.GetClaimBordxByYearAndMonth(this.year, this.month, this.insurerid, this.reinsurerid);

            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
        }
    }
}