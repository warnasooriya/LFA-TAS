using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAS.DataTransfer.Responses;
using TAS.Services.Common;
using TAS.Services.Entities.Management;
using TAS.Services.Entities.Persistence;

namespace TAS.Services.UnitsOfWork
{
    internal sealed class IncurredErningToExcelUnitOfWork : UnitOfWork
    {
        public Guid CountryId;
        public Guid Dealer;
        public string UNWyear;
        public Guid Reinsurer;
        public Guid Insurer;
        public Guid MakeId;
        public Guid ModelId;
        public Guid CylindercountId;
        public Guid InsuaranceLimitationId;
        public Guid EngineCapacityId;
        public DateTime BordxStartDate;
        public DateTime BordxEndDate;
        public DateTime ErnedDate;
        public DateTime ClaimedDate;
        public IncurredErningExportResponseDto Result;
        public CultureInfo ci = CultureInfo.InvariantCulture;
        public IncurredErningToExcelUnitOfWork(string UNWyear, Guid CountryId, Guid Dealer, Guid Reinsurer, Guid Insurer, Guid MakeId, Guid ModelId, Guid CylindercountId, Guid EngineCapacityId,Guid InsuaranceLimitationId, string BordxStartDate, string BordxEndDate, string ErnedDate, string ClaimedDate)
        {
            this.CountryId = CountryId;
            this.Dealer = Dealer;
            this.UNWyear = UNWyear;
            this.Reinsurer = Reinsurer;
            this.Insurer = Insurer;
            this.MakeId = MakeId;
            this.ModelId = ModelId;
            this.CylindercountId = CylindercountId;
            this.EngineCapacityId = EngineCapacityId;
            this.InsuaranceLimitationId = InsuaranceLimitationId;
            if (BordxStartDate != null)
            {
                this.BordxStartDate = DateTime.Parse(BordxStartDate);
            }
            else {
                this.BordxStartDate = Convert.ToDateTime("9999-01-15");
            }
            if (BordxEndDate != null)
            {
                this.BordxEndDate = DateTime.Parse(BordxEndDate);
            }
            else {
                this.BordxEndDate = Convert.ToDateTime("9999-01-15");
            }
            if (ErnedDate !=null)
            {
                this.ErnedDate = DateTime.Parse(ErnedDate);
            }
            else {

                this.ErnedDate = DateTime.UtcNow;

            }

            if (ClaimedDate != null)
            {
                this.ClaimedDate = DateTime.Parse(ClaimedDate);
            }
            else
            {
                this.ClaimedDate = DateTime.UtcNow;
            }
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
                            if (UNWyear == ""  && Dealer == Guid.Empty && Reinsurer == Guid.Empty && Insurer == Guid.Empty && MakeId == Guid.Empty && ModelId == Guid.Empty && CylindercountId == Guid.Empty && EngineCapacityId == Guid.Empty ) { return false; }
                            else { return true; }
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


                IncurredErningProcessEntityManager IncurredErningEntityManager = new IncurredErningProcessEntityManager();
                Result = IncurredErningEntityManager.GetIncurredErningExport(this.UNWyear, this.CountryId, this.Dealer, this.Reinsurer, this.Insurer, this.MakeId, this.ModelId, this.CylindercountId, this.EngineCapacityId,this.InsuaranceLimitationId, this.BordxStartDate, this.BordxEndDate, this.ErnedDate, this.ErnedDate);
            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
        }
    }
}
