using TAS.DataTransfer.Responses;
using TAS.Services.Entities;
using TAS.Services.Entities.Management;
using TAS.Services.Entities.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAS.Services.Common;

namespace TAS.Services.UnitsOfWork
{
    internal sealed class CheckTPAIpRetrievalUnitOfWork : UnitOfWork
    {
        public bool Result
        {
            get;
            set;
        }
        public Guid tpaId
        {
            get;
            set;
        }
        public string userIp
        {
            get;
            set;
        }


        internal CheckTPAIpRetrievalUnitOfWork(Guid tpaId,string userIp)
        {
            this.tpaId = tpaId;
            this.userIp = userIp;
        }

        public CheckTPAIpRetrievalUnitOfWork()
        {
            this.tpaId = Guid.Empty;
        }
        
        public override bool PreExecute() {
            try
            {
                TASEntitySessionManager.OpenSession();
                string dbName = TASTPAEntityManager.GetTPADetailById(tpaId).FirstOrDefault().DBName;
                if (dbName != "")
                {

                    //string tasTpaConnString = TASTPAEntityManager.GetTPAViewOnlyConnStringBydbName(dbName);
                    string tasTpaConnString = TASTPAEntityManager.GetTPADetailById(tpaId).FirstOrDefault().DBConnectionStringViewOnly;

                    if (tasTpaConnString != "")
                    {
                        dbConnectionString = tasTpaConnString;
                        return true;
                    }
                }
                return false;
            }
            catch (Exception e)
            {
                return false;
            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
            //return false;
        }
        public override void Execute()
        {
            try
            {
                if (dbConnectionString != null)   //**
                {     //**
                    EntitySessionManager.OpenSession(dbConnectionString);
                }     //**
                else     //**
                {     //**
                    EntitySessionManager.OpenSession();     //**
                }  
                string res = TPAEntityManager.GetTPAipConfigDataById(tpaId).Where(a => a.Name == "IsIPrestricted").FirstOrDefault().Value;
                bool isIPrestricted = Convert.ToBoolean(res == null ? "False" : res);

                if (isIPrestricted)
                {

                    if (TPAEntityManager.GetAllTPAallowedIPstpaId(tpaId).Where(a => a.IP == userIp).Count() > 0)
                    {
                        this.Result = true;
                    }
                    else
                    {
                        this.Result = false;
                    }

                }
                else
                {
                    this.Result = true;
                }
                //return Result;
                
            }
            finally
            {
                //TASEntitySessionManager.CloseSession();
                EntitySessionManager.CloseSession();

            }

          
        }

    }
}
