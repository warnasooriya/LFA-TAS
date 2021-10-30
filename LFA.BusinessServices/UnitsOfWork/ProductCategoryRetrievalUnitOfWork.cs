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
    //internal sealed class CommodityCategoryRetrievalUnitOfWork : UnitOfWork
    //{

    //    public Guid CommodityCategoryId;
    //    public CommodityCategoryResponseDto Result
    //    {
    //        get;
    //        private set;
    //    }
    //    public override bool PreExecute()
    //    {
    //        return true;
    //    }
    //    public override void Execute()
    //    {

    //        try
    //        {
    //            EntitySessionManager.OpenSession();
    //            CommodityCategoryEntityManager CommodityCategoryEntityManager = new CommodityCategoryEntityManager();
    //            CommodityCategoryResponseDto CommodityCategoryEntity = CommodityCategoryEntityManager.GetCommodityCategoryById(CommodityCategoryId);
    //            if (CommodityCategoryEntity.IsCommodityCategoryExists == null || CommodityCategoryEntity.IsCommodityCategoryExists== false)
    //            {
    //                CommodityCategoryEntity.IsCommodityCategoryExists = false;
    //            }
    //            else
    //            {
    //                CommodityCategoryEntity.IsCommodityCategoryExists = true;
    //            }

    //            this.Result = CommodityCategoryEntity;
    //        }
    //        finally
    //        {
    //            EntitySessionManager.CloseSession();
    //        }
    //    }

    //}

}
