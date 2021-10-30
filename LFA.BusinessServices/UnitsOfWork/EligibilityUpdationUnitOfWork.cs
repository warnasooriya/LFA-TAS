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
    internal sealed class EligibilityUpdationUnitOfWork : UnitOfWork
    {
        public EligibilityRequestDto Eligibility;
        public EligibilityResponseDto ExPr;
       
        public EligibilityUpdationUnitOfWork(EligibilityRequestDto Eligibility)
        {
            
            this.Eligibility = Eligibility;
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
                            EligibilityEntityManager EligibilityEntityManager = new EligibilityEntityManager();
                            var ce = EligibilityEntityManager.GetEligibilityById(Eligibility.Id);
                            if (ce.IsEligibilityExists == true)
                            {
                                Eligibility.EntryDateTime = ce.EntryDateTime;
                                Eligibility.EntryUser = ce.EntryUser;
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
        //        EligibilityEntityManager EligibilityEntityManager = new EligibilityEntityManager();
        //        var ce = EligibilityEntityManager.GetEligibilityById(Eligibility.Id);
        //        if (ce.IsEligibilityExists == true)
        //        {
        //            Eligibility.EntryDateTime = ce.EntryDateTime;
        //            Eligibility.EntryUser = ce.EntryUser;
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
                EligibilityEntityManager EligibilityEntityManager = new EligibilityEntityManager();
                //var ce = EligibilityEntityManager.GetEligibilityById(Eligibility.Id);
                //if (ce.IsEligibilityExists == true)
                //{
                    bool result = EligibilityEntityManager.UpdateEligibility(Eligibility);
                    this.Eligibility.EligibilityInsertion = result;
                //}
                //else
                //{
                //    this.Eligibility.EligibilityInsertion = false;
                //}

            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
        }
    }
}
