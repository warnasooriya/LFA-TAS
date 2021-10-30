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
    internal sealed class BordxReportColumnsMapsRetrievalUnitOfWork : UnitOfWork
    {
        public BordxReportColumnsMapsResponseDto Result
        {
            get;
            private set;
        }
        public Guid UserId
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
                List<BordxReportColumnsMap> BordxReportColumnsEntities = BordxEntityManager.GetBordxReportColumnsMaps(UserId);

				
                BordxReportColumnsMapsResponseDto result = new BordxReportColumnsMapsResponseDto();
                result.BordxReportColumnsMaps = new List<BordxReportColumnsMapResponseDto>();
                foreach (var BordxReportColumnsMap in BordxReportColumnsEntities)
                {
                    BordxReportColumnsMapResponseDto pr = new BordxReportColumnsMapResponseDto();

                    pr.Id = BordxReportColumnsMap.Id;
                    pr.UserId = BordxReportColumnsMap.UserId;
                    pr.ColumnId = BordxReportColumnsMap.ColumnId;
                    pr.IsActive = BordxReportColumnsMap.IsActive;
					
                    //need to write other fields
                    result.BordxReportColumnsMaps.Add(pr);
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
