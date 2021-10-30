using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAS.DataTransfer.Responses;
using TAS.Services.Common;
using TAS.Services.Entities;
using TAS.Services.Entities.Management;
using TAS.Services.Entities.Persistence;

namespace TAS.Services.UnitsOfWork
{
    internal sealed class PartAreaByMakeIdRetrievalUnitOfWork : UnitOfWork
    {

        public PartsResponseDto Result
        {
            get;
            private set;
        }
        public Guid MakeId
        {
            get;
            set;
        }
        public PartAreaByMakeIdRetrievalUnitOfWork(Guid MakeId)
        {
            this.MakeId = MakeId;
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
                ClaimEntityManager claimEntityManager = new ClaimEntityManager();
                List<Part> PartEntities = claimEntityManager.GetAllPartAreaByMakeId(MakeId);


                PartsResponseDto result = new PartsResponseDto();
                result.Part = new List<PartResponseDto>();
                foreach (var Parts in PartEntities)
                {
                    PartResponseDto pr = new PartResponseDto();

                    pr.Id = Parts.Id;
                    pr.AllocatedHours = Parts.AllocatedHours;
                    pr.ApplicableForAllModels = Parts.ApplicableForAllModels;
                    pr.CommodityId = Parts.CommodityId;
                  //  pr.EntryBy = Parts.EntryBy;
                   // pr.EntryDateTime = Parts.EntryDateTime;
                    pr.IsActive = Parts.IsActive;
                    pr.MakeId = Parts.MakeId;
                    pr.PartAreaId = Parts.PartAreaId;
                    pr.PartCode = Parts.PartCode;
                    pr.PartName = Parts.PartName;
                    pr.PartNumber = Parts.PartNumber;
                    //need to write other fields
                    result.Part.Add(pr);
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
