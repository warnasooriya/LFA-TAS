using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAS.DataTransfer.Requests;
using TAS.DataTransfer.Responses;
using TAS.Services.Common;
using TAS.Services.Entities.Management;
using TAS.Services.Entities.Persistence;

namespace TAS.Services.UnitsOfWork
{
    internal sealed class IncurredErningProcessUnitOfWork : UnitOfWork
    {

        public Guid CountryId;
        public Guid Dealer;
        public string UNWyear;
        public Guid Reinsurer;
        public Guid Insurer;
        public Guid MakeId;
        public Guid ModelId;
        public Guid CylindercountId;
        public Guid EngineCapacityId;
        public DateTime BordxStartDate;
        public DateTime BordxEndDate;
        public DateTime ErnedDate;
        public DateTime ClaimedDate; 

        public IncurredErningProcessResponseDto Result
        {
            get;
            private set;
        }
        public IncurredErningProcessUnitOfWork(string UNWyear, Guid CountryId, Guid Dealer, Guid Reinsurer, Guid Insurer, Guid MakeId, Guid ModelId, Guid CylindercountId, Guid EngineCapacityId, string BordxStartDate, string BordxEndDate, string ErnedDate, string ClaimedDate)
        {

            this.CountryId = CountryId;
            this.Dealer = Dealer;
            this.UNWyear = UNWyear;
            this.Reinsurer = Reinsurer;
            this.Insurer= Insurer;
            this.MakeId = MakeId;
            this.ModelId = ModelId;
            this.CylindercountId= CylindercountId;
            this.EngineCapacityId = EngineCapacityId;

            if (BordxStartDate!=null) {
                this.BordxStartDate = DateTime.Parse(BordxStartDate);
            }
            if (BordxEndDate != null)
            {
                this.BordxEndDate = DateTime.Parse(BordxEndDate);
            }
            if (ErnedDate != null)
            {
                this.ErnedDate = DateTime.Parse(ErnedDate);
            }
                //this.ClaimedDate = DateTime.Parse(ClaimedDate);

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
                IncurredErningProcessEntityManager IncurredErningProcessEntityManager = new IncurredErningProcessEntityManager();
                IncurredErningProcessResponseDto IncurredErningProcessEntity = IncurredErningProcessEntityManager.IncurredErningProcess(this.UNWyear,this.CountryId, this.Dealer, this.Reinsurer, this.Insurer, this.MakeId, this.ModelId, this.CylindercountId, this.EngineCapacityId, this.BordxStartDate, this.BordxEndDate, this.ErnedDate, this.ErnedDate, this.ErnedDate);


                this.Result = IncurredErningProcessEntity;
            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
        }
    }
}