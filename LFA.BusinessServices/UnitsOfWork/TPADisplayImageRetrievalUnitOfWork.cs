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


namespace TAS.Services.UnitsOfWork
{
    internal sealed class TPADisplayImageRetrievalUnitOfWork : UnitOfWork
    {

        public Guid imageId;
        public Guid TPAId;
        public ImageResponseDto Result
        {
            get;
            private set;
        }
        internal TPADisplayImageRetrievalUnitOfWork(Guid tpaId)
        {
            this.TPAId = tpaId;
        }

        public override bool PreExecute()
        {
            //try
            //{

            //    Common.JWTHelper JWTHelper = new Common.JWTHelper(SecurityContext);
            //    Dictionary<string, object> str = JWTHelper.DecodeAuthenticationToken();
            //    string dbName = str.FirstOrDefault(f => f.Key == "dbName").Value.ToString();
            //    if (dbName != "")
            //    {
            //        TASEntitySessionManager.OpenSession();
            //        string tasTpaConnString = TASTPAEntityManager.GetTPAConnStringBydbName(dbName);
            //        TASEntitySessionManager.CloseSession();
            //        if (tasTpaConnString != "")
            //        {
            //            dbConnectionString = tasTpaConnString;
            //            return true;
            //        }
            //    }
            //    return false;

            //}
            //catch (Exception e)
            //{
            //    return false;
            //}

            try
            {

                TASEntitySessionManager.OpenSession();
                string dbName = TASTPAEntityManager.GetTPADetailById(TPAId).FirstOrDefault().DBName;
                if (dbName != "")
                {

                    //string tasTpaConnString = TASTPAEntityManager.GetTPAViewOnlyConnStringBydbName(dbName);
                    string tasTpaConnString = TASTPAEntityManager.GetTPADetailById(TPAId).FirstOrDefault().DBConnectionStringViewOnly;

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
                TASEntitySessionManager.CloseSession();
            }
        }
        public override void Execute()
        {
            string dbName = "";
            if (SecurityContext.Token == null)
            {
                
            }
            else
            {
                //get db by token decrypting  TPAId=
            }

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


                //EntitySessionManager.OpenSession();
                ImageEntityManager imageEntityManager = new ImageEntityManager();
                ImageResponseDto imageEntity = imageEntityManager.GetImageById(imageId);
                this.Result = imageEntity;
            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
        }

    }
}
