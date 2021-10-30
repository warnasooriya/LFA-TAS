using TAS.DataTransfer.Requests;
using TAS.DataTransfer.Responses;
using TAS.Services.Entities.Management;
using TAS.Services.Entities.Persistence;
using TAS.Services.UnitsOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAS.Services.Common;
using TAS.Caching;

namespace TAS.Services.UnitsOfWork
{
    internal sealed class TransmissionTypeUpdationUnitOfWork : UnitOfWork
    {
        public TransmissionTypeRequestDto TransmissionType;
        public TransmissionTypeResponseDto ExPr;
        private string UniqueDbName = string.Empty;
        public TransmissionTypeUpdationUnitOfWork(TransmissionTypeRequestDto Transmission)
        {

            this.TransmissionType = Transmission;
        }
        public override bool PreExecute()
        {
            try
            {
                Common.JWTHelper JWTHelper = new Common.JWTHelper(SecurityContext);
                Dictionary<string, object> str = JWTHelper.DecodeAuthenticationToken();
                string dbName = UniqueDbName = str.FirstOrDefault(f => f.Key == "dbName").Value.ToString();
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
                            TransmissionTypeEntityManager TransmissionEntityManager = new TransmissionTypeEntityManager();
                            var ce = TransmissionEntityManager.GetTransmissionTypeById(TransmissionType.Id);
                            if (ce.IsTransmissionTypeExists == true)
                            {
                                TransmissionType.EntryDateTime = ce.EntryDateTime;
                                TransmissionType.EntryUser = ce.EntryUser;
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

        //public override bool PreExecute()
        //{
        //    try
        //    {
        //        EntitySessionManager.OpenSession();
        //        TransmissionTypeEntityManager TransmissionEntityManager = new TransmissionTypeEntityManager();
        //        var ce = TransmissionEntityManager.GetTransmissionTypeById(TransmissionType.Id);
        //        if (ce.IsTransmissionTypeExists == true)
        //        {
        //            TransmissionType.EntryDateTime = ce.EntryDateTime;
        //            TransmissionType.EntryUser = ce.EntryUser;
        //            return true;
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //    finally
        //    {
        //        EntitySessionManager.CloseSession();

        //    }

        //}

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
                TransmissionTypeEntityManager TransmissionTypeEntityManager = new TransmissionTypeEntityManager();
                //var ce = TransmissionEntityManager.GetTransmissionById(Transmission.Id);
                //if (ce.IsTransmissionExists == true)
                //{
                    bool result = TransmissionTypeEntityManager.UpdateTransmissionType(TransmissionType);
                    this.TransmissionType.TransmissionTypeInsertion = result;
                    if (result)
                    {
                        /* remove cache */
                        ICache cache = CacheFactory.GetCache();
                        cache.Remove(UniqueDbName + "_TransmissionTypes");
                    }
                //}
                //else
                //{
                //    this.Transmission.TransmissionInsertion = false;
                //}

            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
        }
    }
}
