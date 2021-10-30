using System;
using System.Collections.Generic;
using System.Linq;
using TAS.DataTransfer.Requests;
using TAS.Services.Common;
using TAS.Services.Entities.Management;
using TAS.Services.Entities.Persistence;
using TAS.Services.UnitsOfWork;

namespace TAS.Services.BusinessServices
{
    internal sealed class ExcelDownloadReportUnitOfWork : UnitOfWork
    {
        public object Result { get; set; }
        private readonly Guid _reportId;
        private string _tpaName { get; set; }
        private string _connStr { get; set; }

        private readonly List<ReportParameterDataRequestDto> _reportParameter;


        public ExcelDownloadReportUnitOfWork(Guid reportId, List<ReportParameterDataRequestDto> paramList)
        {
            _reportId = reportId;
            _reportParameter = paramList;
        }

        public override bool PreExecute()
        {
            try
            {
                Common.JWTHelper JWTHelper = new Common.JWTHelper(SecurityContext);
                Dictionary<string, object> str = JWTHelper.DecodeAuthenticationToken();
                string dbName = _tpaName = str.FirstOrDefault(f => f.Key == "dbName").Value.ToString();
                if (dbName != "")
                {
                    TASEntitySessionManager.OpenSession();
                    string tasTpaConnString = TASTPAEntityManager.GetTPAConnStringBydbName(dbName);
                    TASEntitySessionManager.CloseSession();
                    if (tasTpaConnString != "")
                    {
                        dbConnectionString = _connStr = tasTpaConnString;
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

               this.Result = ReportEntityManager.ExcelDownloadReport(_reportId, _reportParameter, _tpaName, _connStr);
            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
        }
    }
}