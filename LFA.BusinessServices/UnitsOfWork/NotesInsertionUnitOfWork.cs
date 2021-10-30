using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAS.DataTransfer.Requests;
using TAS.Services.Common;
using TAS.Services.Entities.Management;
using TAS.Services.Entities.Persistence;

namespace TAS.Services.UnitsOfWork
{
    internal sealed class NotesInsertionUnitOfWork : UnitOfWork
    {
        private readonly NotesRequestDto Note;
        public string Result { get; set; }
        public NotesInsertionUnitOfWork(NotesRequestDto _note)
        {

            Note = _note;
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
                if (dbConnectionString != null)
                {
                    EntitySessionManager.OpenSession(dbConnectionString);
                }
                else
                {
                    EntitySessionManager.OpenSession();
                }
                ClaimEntityManager ClaimEntityManager = new ClaimEntityManager();
                Result = ClaimEntityManager.AddNotes(Note);
            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
        }
    }
}
