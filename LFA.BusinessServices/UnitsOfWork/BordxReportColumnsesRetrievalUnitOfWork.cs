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
using TAS.DataTransfer.Requests;

namespace TAS.Services.UnitsOfWork
{
    internal sealed class BordxReportColumnsesRetrievalUnitOfWork : UnitOfWork
    {
        public object Result
        {
            get;
            private set;
        }
        private readonly BordxColumnRequestDto bordxColumnRequestDto;

        public BordxReportColumnsesRetrievalUnitOfWork(BordxColumnRequestDto _bordxColumnRequestDto) {
            bordxColumnRequestDto = _bordxColumnRequestDto;
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
                BordxEntityManager BordxEntityManager = new BordxEntityManager();
                List<TAS.Services.Entities.BordxReportColumns> BordxReportColumnsEntities = BordxEntityManager.GetBordxReportColumnses(bordxColumnRequestDto);



                this.Result = BordxReportColumnsEntities
                    .OrderBy(a => a.Sequance)
                    .Select(a => new
                    {
                        a.DisplayName,
                        a.HeaderId,
                        a.Id,
                        a.IsActive,
                        Status = a.IsActive==true?"Active":"Inactive"
                    });
            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
        }

    }
}
