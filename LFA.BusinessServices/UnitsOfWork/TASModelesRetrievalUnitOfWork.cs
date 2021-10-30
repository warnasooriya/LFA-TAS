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
    internal sealed class TASModelesRetrievalUnitOfWork : UnitOfWork
    {
       

        public ModelesResponseDto Result
        {
            get;
            private set;
        }

                public Guid makeId
        {
            get;
            set;
        }
        public Guid tpaId
        {
            get;
            set;
        }
        public TASModelesRetrievalUnitOfWork(Guid makeId, Guid tpaId)
        {
            this.makeId = makeId;
            this.tpaId = tpaId;
        }
        public override bool PreExecute()
        {
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
                ModelEntityManager ModelEntityManager = new ModelEntityManager();
                List<Model> ModelEntities = ModelEntityManager.GetModeles();

				
                ModelesResponseDto result = new ModelesResponseDto();
                result.Modeles = new List<ModelResponseDto>();
                foreach (var Model in ModelEntities.Where(a => a.MakeId== this.makeId))
                {
                    ModelResponseDto pr = new ModelResponseDto();

                    pr.Id = Model.Id;
                   // pr.CommodityTypeId = Model.CommodityTypeId;
                    pr.CategoryId = Model.CategoryId;
					pr.ModelName = Model.ModelName;
                    pr.ModelCode = Model.ModelCode;
                    pr.NoOfDaysToRiskStart = Model.NoOfDaysToRiskStart;
                    pr.MakeId = Model.MakeId;
                    pr.IsActive = Model.IsActive;
                   // pr.ManufacturerId = Model.ManufacturerId;
                    pr.WarantyGiven = Model.WarantyGiven;

                    pr.EntryDateTime = Model.EntryDateTime;
                    pr.EntryUser = Model.EntryUser;
					
                    //need to write other fields
                    result.Modeles.Add(pr);
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
