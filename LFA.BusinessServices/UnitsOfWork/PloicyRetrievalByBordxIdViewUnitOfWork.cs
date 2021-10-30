using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAS.DataTransfer.Common;
using TAS.DataTransfer.Requests;
using TAS.Services.Common;
using TAS.Services.Entities.Management;
using TAS.Services.Entities.Persistence;

namespace TAS.Services.UnitsOfWork
{
    internal sealed class PloicyRetrievalByBordxIdViewUnitOfWork:UnitOfWork
    {
        private readonly BordxViewGridRequestDto _BordxViewGridRequestDto = null;
        public object Result;
        public PloicyRetrievalByBordxIdViewUnitOfWork(BordxViewGridRequestDto BordxViewGridRequestDto)
        {
            _BordxViewGridRequestDto = BordxViewGridRequestDto;
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
                            if (_BordxViewGridRequestDto.BordxViewGridSearchCriterias != null && _BordxViewGridRequestDto.BordxViewGridSearchCriterias.bordxId!=Guid.Empty)
                            {
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

                Result = BordxEntityManager.GetConfirmedBordxForView(_BordxViewGridRequestDto.BordxViewGridSearchCriterias, _BordxViewGridRequestDto.PaginationOptionsbordxSearchGrid);
            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
        }
    }
}
