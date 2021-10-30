using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAS.DataTransfer.Requests;
using TAS.DataTransfer.Responses;
using TAS.Services.Common;
using TAS.Services.Entities;
using TAS.Services.Entities.Management;
using TAS.Services.Entities.Persistence;

namespace TAS.Services.UnitsOfWork
{
    internal sealed class ReinsurerBordxRetrievalByYearandReinsurerIdyGirdUnitOfWork : UnitOfWork
    {
        public List<ReinsureBordxByReinsureIdandYearResponseDto> Result
        {
            get;
            private set;
        }

        public Guid ReinsureId
        {
            get;
            set;
        }

        public int BordxYear
        {
            get;
            set;
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
                //ReinsurerEntityManager ReinsurerEntityManager = new ReinsurerEntityManager();
                //Result = ReinsurerEntityManager.GetAllReinsurerBordxByYearandReinsurerIdForGrid(_GetAllReinsurerBordxByYearandReinsureIdRequestDto);

                ReinsurerEntityManager ReinsurerEntityManager = new ReinsurerEntityManager();
                List<ReinsureBordxByReinsureIdandYearResponseDto> userEntities = ReinsurerEntityManager.GetAllReinsurerBordxByYearandReinsurerIdForGrid(this.ReinsureId,this.BordxYear);

                this.Result = userEntities;

            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
        }
    }
}
