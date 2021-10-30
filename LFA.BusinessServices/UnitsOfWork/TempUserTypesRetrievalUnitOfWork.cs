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
    internal sealed class TempUserTypesRetrievalUnitOfWork : UnitOfWork
    {

        public UserTypesResponseDto Result
        {
            get;
            private set;
        }

        public string tpaID;

        private string dbName
        {
            get;
            set;
        }
        public override bool PreExecute()
        {
            try
            {
                TASEntitySessionManager.OpenSession();
                List<TASTPA> tasTpa = new List<TASTPA>();
                tasTpa = TASTPAEntityManager.GetTPADetailById(Guid.Parse(tpaID));
                TASEntitySessionManager.CloseSession();
                if (tasTpa.FirstOrDefault() != null)
                {
                    if (tasTpa.First().DBConnectionString != null && tasTpa.First().DBName != null && tasTpa.First().DBConnectionString != "" && tasTpa.First().DBName != "")
                    {
                        dbConnectionString = tasTpa.First().DBConnectionString;
                        dbName = tasTpa.First().DBName;
                        return true;
                    }
                    else
                    {
                        return false;
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
                UserEntityManager UserEntityManager = new UserEntityManager();
                List<UserTypeResponseDto> userEntities = UserEntityManager.GetUserTypes();
                UserTypesResponseDto result = new UserTypesResponseDto();
                result.UserTypes = userEntities;
                this.Result = result;
            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
        }

    }
}
