using System;
using System.Collections.Generic;
using System.Linq;
using TAS.DataTransfer.Requests;
using TAS.Services.Common;
using TAS.Services.Entities.Management;
using TAS.Services.Entities.Persistence;

namespace TAS.Services.UnitsOfWork
{
    internal sealed class BordxReportTemplateRetrievalForSearchGridUnitOfWork : UnitOfWork
    {
        private readonly BordxReportTemplateSearchGridRequestDto _bordxReportTemplateSearchGridRequestDto;
        public object result;
        public BordxReportTemplateRetrievalForSearchGridUnitOfWork(BordxReportTemplateSearchGridRequestDto bordxReportTemplateSearchGridRequestDto)
        {
            _bordxReportTemplateSearchGridRequestDto = bordxReportTemplateSearchGridRequestDto;
        }

        public override bool PreExecute()
        {
            try
            {
                JWTHelper JWTHelper = new JWTHelper(SecurityContext);
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
                BordxEntityManager bordxEntityManager = new BordxEntityManager();
                result = bordxEntityManager.GetAllBordxReportTemplateForSearchGrid(_bordxReportTemplateSearchGridRequestDto);
            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
        }
    }
}
