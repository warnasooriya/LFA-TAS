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
    internal sealed class ClaimBordxsRetrievalUnitOfWork : UnitOfWork
    {


        public ClaimBordxsResponseDto Result
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
                ClaimBordxEntityManager BordxEntityManager = new ClaimBordxEntityManager();
                ReinsurerEntityManager ReinsurerEntity = new ReinsurerEntityManager();
                InsurerEntityManager InsurerEntity = new InsurerEntityManager();
                List<ClaimBordx> BordxEntities = BordxEntityManager.GetClaimBordxs();


                ClaimBordxsResponseDto result = new ClaimBordxsResponseDto();
                result.ClaimBordxs = new List<ClaimBordxResponseDto>();
                foreach (var Bordx in BordxEntities)
                {
                    ClaimBordxResponseDto pr = new ClaimBordxResponseDto();

                    pr.Id = Bordx.Id;
                    pr.IsConfirmed = Bordx.IsConfirmed;
                    pr.Bordxmonth = Bordx.Bordxmonth;
                    pr.BordxYear = Bordx.BordxYear;
                    pr.BordxNumber = Bordx.BordxNumber;
                    pr.EntryDateTime = Bordx.EntryDateTime;
                    pr.UserId = Bordx.UserId;
                    pr.Reinsurer = Bordx.Reinsurer;
                    pr.Insurer = Bordx.Insurer;
                    pr.Fromdate = Bordx.Fromdate;
                    pr.Todate = Bordx.Todate;
                    pr.ReinsurerName = ReinsurerEntity.GetReinsurerById(Bordx.Reinsurer).ReinsurerName;
                    pr.InsurerShortName = InsurerEntity.GetInsurerById(Bordx.Insurer).InsurerShortName;
					
                    //need to write other fields
                    result.ClaimBordxs.Add(pr);
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
