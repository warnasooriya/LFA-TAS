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
    internal sealed class EligibilitysRetrievalUnitOfWork : UnitOfWork
    {
       

        public EligibilitiesResponseDto Result
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
                EligibilityEntityManager EligibilityEntityManager = new EligibilityEntityManager();
                List<Eligibility> EligibilityEntities = EligibilityEntityManager.GetEligibilitys();

				
                EligibilitiesResponseDto result = new EligibilitiesResponseDto();
                result.Eligibilities = new List<EligibilityResponseDto>();
                foreach (var Eligibility in EligibilityEntities)
                {
                    EligibilityResponseDto pr = new EligibilityResponseDto();

                    pr.Id = Eligibility.Id;
                    pr.ContractId = Eligibility.ContractId;
                    pr.MonthsFrom = Eligibility.MonthsFrom;
                    pr.MonthsTo = Eligibility.MonthsTo;
                    pr.AgeFrom = Eligibility.AgeFrom;
                    pr.MileageTo = Eligibility.MileageTo;
                    pr.AgeTo = Eligibility.AgeTo;
                    pr.MileageFrom = Eligibility.MileageFrom;
                    pr.PlusMinus = Eligibility.PlusMinus;
                    pr.Premium = Eligibility.Premium;
                    pr.IsPercentage = Eligibility.IsPercentage;
                    pr.EntryDateTime = Eligibility.EntryDateTime;
                    pr.EntryUser = Eligibility.EntryUser;
					
                    //need to write other fields
                    result.Eligibilities.Add(pr);
                }
                this.Result = result;
            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
        }

    }
}
