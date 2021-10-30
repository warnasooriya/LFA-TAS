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
    internal sealed class ModelUpdationUnitOfWork : UnitOfWork
    {
        public ModelRequestDto Model;
        public ModelResponseDto ExPr;

        public ModelUpdationUnitOfWork(ModelRequestDto Model)
        {
            this.Model = Model;
        }
        private string UniqueDbName = string.Empty;
        public override bool PreExecute()
        {
            try
            {
                Common.JWTHelper JWTHelper = new Common.JWTHelper(SecurityContext);
                Dictionary<string, object> str = JWTHelper.DecodeAuthenticationToken();
                string dbName = UniqueDbName= str.FirstOrDefault(f => f.Key == "dbName").Value.ToString();
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
                            ModelEntityManager modelEntityManager = new ModelEntityManager();
                            var ce = modelEntityManager.GetModelById(Model.Id);
                            if (ce.IsModelExists == true)
                            {
                                Model.EntryDateTime = ce.EntryDateTime;
                                Model.EntryUser = ce.EntryUser;
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
        //        ModelEntityManager modelEntityManager = new ModelEntityManager();
        //        var ce = modelEntityManager.GetModelById(Model.Id);
        //        if (ce.IsModelExists == true)
        //        {
        //            Model.EntryDateTime = ce.EntryDateTime;
        //            Model.EntryUser = ce.EntryUser;
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
                ModelEntityManager modelEntityManager = new ModelEntityManager();
                //var ce = modelEntityManager.GetModelById(Model.Id);
                //if (ce.IsModelExists == true)
                //{
                    bool result = modelEntityManager.UpdateModel(Model);
                    this.Model.ModelInsertion = result;
                ICache cache = CacheFactory.GetCache();
                string uniqueCacheKey = UniqueDbName + "_MakesByCommodityCategoryId_" + Model.CategoryId.ToString().ToLower();
                cache.Remove(uniqueCacheKey);
                //}
                //else
                //{
                //    this.Model.ModelInsertion = false;
                //}
            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
        }
    }
}
