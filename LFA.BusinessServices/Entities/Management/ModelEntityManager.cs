using TAS.Services.Entities.Persistence;
using NHibernate;
using TAS.Services.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using System.Text;
using System.Threading.Tasks;
using TAS.DataTransfer.Requests;
using TAS.DataTransfer.Responses;

using NHibernate.Criterion;
using TAS.Services.Common;
using System.Security.Cryptography;
using System.Linq.Expressions;
using NLog;
using System.Reflection;

namespace TAS.Services.Entities.Management
{
    public class ModelEntityManager
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public List<Model> GetModeles()
        {
            List<Model> entities = null;
            ISession session = EntitySessionManager.GetSession();
            IQueryable<Model> ModelData = session.Query<Model>();
            entities = ModelData.ToList();
            return entities;
        }


        public ModelResponseDto GetModelById(Guid ModelId)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();

                ModelResponseDto pDto = new ModelResponseDto();

                var query =
                    from Model in session.Query<Model>()
                    where Model.Id == ModelId
                    select new { Model = Model };

                var result = query.ToList();


                if (result != null && result.Count > 0)
                {
                    pDto.Id = result.First().Model.Id;
                    pDto.ModelCode = result.First().Model.ModelCode;
                    pDto.ModelName = result.First().Model.ModelName;
                    pDto.MakeId = result.First().Model.MakeId;
                    pDto.IsActive = result.First().Model.IsActive;
                    pDto.NoOfDaysToRiskStart = result.First().Model.NoOfDaysToRiskStart;
                    pDto.WarantyGiven = result.First().Model.WarantyGiven;
                    pDto.EntryDateTime = result.First().Model.EntryDateTime;
                    pDto.EntryUser = result.First().Model.EntryUser;
                    pDto.ContryOfOrigineId = result.First().Model.ContryOfOrigineId;
                    pDto.CategoryId = result.First().Model.CategoryId;
                    pDto.AdditionalPremium = result.First().Model.AdditionalPremium;
                    pDto.IsModelExists = true;
                    return pDto;
                }
                else
                {
                    pDto.IsModelExists = false;
                    return null;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }
        }


        internal bool AddModel(ModelRequestDto Model)
        {
            try
            {

                ISession session = EntitySessionManager.GetSession();
                Model pr = new Entities.Model();

                pr.Id = new Guid();
                pr.ModelCode = Model.ModelCode;
                pr.ModelName= Model.ModelName;
                pr.NoOfDaysToRiskStart = Model.NoOfDaysToRiskStart;
                pr.WarantyGiven = Model.WarantyGiven;
                pr.IsActive = Model.IsActive;
                pr.CategoryId = Model.CategoryId;
                pr.ContryOfOrigineId = Model.ContryOfOrigineId;
                pr.MakeId = Model.MakeId;
                pr.AdditionalPremium = Model.AdditionalPremium;
                pr.EntryDateTime = DateTime.Today.ToUniversalTime();
                pr.EntryUser = Guid.Parse("ba56ec84-1fe0-4385-abe4-182f62caa050");

                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.SaveOrUpdate(pr);
                    transaction.Commit();
                }

                return true;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return false;
            }
        }

        internal bool UpdateModel(ModelRequestDto Model)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();
                Model pr = new Entities.Model();

                pr.Id = Model.Id;
				pr.ContryOfOrigineId = Model.ContryOfOrigineId;
				pr.IsActive = Model.IsActive;
				pr.MakeId = Model.MakeId;
				pr.NoOfDaysToRiskStart = Model.NoOfDaysToRiskStart;
                pr.CategoryId = Model.CategoryId;
                pr.MakeId = Model.MakeId;
                pr.ModelCode = Model.ModelCode;
                pr.ModelName = Model.ModelName;
                pr.WarantyGiven = Model.WarantyGiven;
                pr.AdditionalPremium = Model.AdditionalPremium;
                pr.EntryDateTime = Model.EntryDateTime;
                pr.EntryUser = Model.EntryUser;

                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Update(pr);
                    transaction.Commit();
                }

                return true;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return false;
            }
        }

        internal static ModelesResponseDto GetModelsByMakeId(Guid MakeId)
        {
            ISession session = EntitySessionManager.GetSession();

            ModelesResponseDto ModelsDto = new ModelesResponseDto();

            try
            {
                ModelsDto.Modeles = new List<ModelResponseDto>();
                var query = session.Query<Model>().Where(a => a.MakeId == MakeId);

                var result = query.ToList();
                foreach (Model model in result)
                {
                    ModelResponseDto mrd = new ModelResponseDto();
                    mrd.Id = model.Id;
                    mrd.ModelCode = model.ModelCode;
                    mrd.ModelName = model.ModelName;
                    mrd.MakeId = model.MakeId;
                    mrd.IsActive = model.IsActive;
                    mrd.NoOfDaysToRiskStart = model.NoOfDaysToRiskStart;
                    mrd.WarantyGiven = model.WarantyGiven;
                    mrd.EntryDateTime = model.EntryDateTime;
                    mrd.EntryUser = model.EntryUser;
                    mrd.ContryOfOrigineId = model.ContryOfOrigineId;
                    mrd.CategoryId = model.CategoryId;
                    mrd.AdditionalPremium = model.AdditionalPremium;
                    ModelsDto.Modeles.Add(mrd);
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }

            return ModelsDto;

        }

        internal static ModelesResponseDto ModelsByMakeIdsAndCommodityCategoryId(List<Guid> MakeIds,Guid CommodityCategoryId)
        {
            ISession session = EntitySessionManager.GetSession();
            ModelesResponseDto ModelsDto = new ModelesResponseDto();

            try
            {
                ModelsDto.Modeles = new List<ModelResponseDto>();
                Expression<Func<Model, bool>> filterModel = PredicateBuilder.True<Model>();
                if (MakeIds.Count == 0)
                {
                    return ModelsDto;
                }
                //filterModel = filterModel.And(a => a.IsActive);
                int count = 0;
                foreach (Guid uuid in MakeIds)
                {
                    if (count == 0)
                    {
                        filterModel = filterModel.And(a => a.MakeId == uuid);
                    }
                    else
                    {
                        filterModel = filterModel.Or(a => a.MakeId == uuid);
                    }
                    count++;
                }

                filterModel = filterModel.And(a => a.CategoryId == CommodityCategoryId);

                IQueryable<Model> result = session.Query<Model>().Where(filterModel);
                foreach (Model model in result)
                {
                    ModelResponseDto mrd = new ModelResponseDto();
                    mrd.Id = model.Id;
                    mrd.ModelCode = model.ModelCode;
                    mrd.ModelName = model.ModelName;
                    mrd.MakeId = model.MakeId;
                    mrd.IsActive = model.IsActive;
                    mrd.NoOfDaysToRiskStart = model.NoOfDaysToRiskStart;
                    mrd.WarantyGiven = model.WarantyGiven;
                    mrd.EntryDateTime = model.EntryDateTime;
                    mrd.EntryUser = model.EntryUser;
                    mrd.ContryOfOrigineId = model.ContryOfOrigineId;
                    mrd.CategoryId = model.CategoryId;
                    mrd.AdditionalPremium = model.AdditionalPremium;
                    ModelsDto.Modeles.Add(mrd);
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return ModelsDto;
        }

        internal static object GetModelesByMakeIds(List<Guid> MakeIdList) {
            ISession session = EntitySessionManager.GetSession();
            object modelLists = session.Query<Model>().Where(a => MakeIdList.Contains(a.MakeId)).ToList();
            return modelLists;
        }
        internal static object GetModelByMakeIdAndCatogaryId(Guid MakeId, Guid CommodityCategoryId)
        {
            object Response = null;
            try
            {
                ISession session = EntitySessionManager.GetSession();
                CommonEntityManager cem = new CommonEntityManager();
                List<Model> partList = session.Query<Model>()
                    .Where(a => a.MakeId == MakeId && a.CategoryId == CommodityCategoryId).ToList();
                if (partList != null && partList.Count() > 0)
                {
                    Response = partList
                        .OrderBy(a => a.ModelName)
                        .Select(a => new
                        {
                            a.Id,
                            a.AdditionalPremium,
                            a.ContryOfOrigineId,
                            a.IsActive,
                            a.ModelCode,
                            a.ModelName,
                            a.NoOfDaysToRiskStart,
                            a.WarantyGiven


                        }).ToArray();
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }
    }
}
