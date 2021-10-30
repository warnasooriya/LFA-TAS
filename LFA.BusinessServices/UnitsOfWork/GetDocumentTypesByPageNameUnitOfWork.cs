using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAS.Services.Entities.Management;
using TAS.Services.Entities.Persistence;

namespace TAS.Services.UnitsOfWork
{
    internal sealed class GetDocumentTypesByPageNameUnitOfWork : UnitOfWork
    {
        public object Result { get; set; }
        private readonly Guid _tpaId;
        private readonly string _pageName;

        public GetDocumentTypesByPageNameUnitOfWork(string pageName, Guid tpaId)
        {
            _tpaId = tpaId;
            _pageName = pageName;
        }


        public override bool PreExecute()
        {
            try
            {
                TASEntitySessionManager.OpenSession();
                string dbName = TASTPAEntityManager.GetTPADetailById(_tpaId).FirstOrDefault().DBName;
                if (dbName != "")
                {
                    string tasTpaConnString = TASTPAEntityManager.GetTPADetailById(_tpaId).FirstOrDefault().DBConnectionStringViewOnly;
                    if (tasTpaConnString != "")
                    {
                        dbConnectionString = tasTpaConnString;
                        return true;
                    }
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                TASEntitySessionManager.CloseSession();
            }
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

                this.Result = new AttachmentEntityManager().GetDocumentTypesByPageName(_pageName);
            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
        }
    }
}
