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
    internal sealed class TASTPARetrievalUnitOfWork : UnitOfWork
    {
        public TASTPAsResponseDto Result
        {
            get;
            private set;
        }
        public Guid tpaId
        {
            get;
            set;
        }
      
       

        internal TASTPARetrievalUnitOfWork(Guid tpaId)
        {
            this.tpaId = tpaId;
        }

        public TASTPARetrievalUnitOfWork()
        {
            this.tpaId = Guid.Empty;
        }
        
        public override bool PreExecute() {
            try
            {
                Common.TASJWTHelper JWTHelper = new Common.TASJWTHelper(SecurityContext);
                Dictionary<string, object> str = JWTHelper.DecodeAuthenticationToken();
                string dbName = str.FirstOrDefault(f => f.Key == "dbName").Value.ToString();
                if (dbName == "TAS")
                {
                    TASEntitySessionManager.OpenSession();
                    if (JWTHelper.checkTokenValidity(Convert.ToInt32(ConfigurationData.tasTokenValidTime.ToString())))
                    {
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
                TASEntitySessionManager.OpenSession();
                TASTPAEntityManager TPAEntityManager = new TASTPAEntityManager();
                List<TASTPA> TPAEntities = new List<TASTPA>();
                if (tpaId == Guid.Empty)
                {
                    TPAEntities = TASTPAEntityManager.GetAllTPAs();
                }
                else
                {
                    TPAEntities = TASTPAEntityManager.GetTPADetailById(tpaId);

                }
                TASTPAsResponseDto result = new TASTPAsResponseDto();
                result.TPAs = new List<TASTPAResponseDto>();
                foreach (TASTPA tpa in TPAEntities)
                {
                    TASTPAResponseDto TPAResponseDto = new TASTPAResponseDto();
                    TPAResponseDto.Id = tpa.Id;
                    TPAResponseDto.Address = tpa.Address;
                    TPAResponseDto.Banner = tpa.Banner;
                    TPAResponseDto.DiscountDescription = tpa.DiscountDescription;
                    TPAResponseDto.Logo = tpa.Logo;
                    TPAResponseDto.Name = tpa.Name;
                    TPAResponseDto.TelNumber = tpa.TelNumber;

                    result.TPAs.Add(TPAResponseDto);
                }
                this.Result = result;
            }
            finally
            {
                TASEntitySessionManager.CloseSession();
            }
        }


       
    }
}
