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
using NLog;
using System.Reflection;
using System.Text.RegularExpressions;

namespace TAS.Services.Entities.Management
{
    public class ProductEntityManager
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public List<Product> GetProducts()
        {
            List<Product> entities = null;
            ISession session = EntitySessionManager.GetSession();
            IQueryable<Product> productData = session.Query<Product>();
            entities = productData.ToList();
            return entities;
        }

        public List<ProductResponseDto> GetAllProducts()
        {
            ISession session = EntitySessionManager.GetSession();
            return (from prod in session.Query<Product>()
                    join commodityType in session.Query<CommodityType>() on prod.CommodityTypeId equals commodityType.CommodityTypeId
                    join prodType in session.Query<ProductType>() on prod.ProductTypeId equals prodType.Id
                    select new ProductResponseDto
                    {
                        Id = prod.Id,
                        CommodityTypeId = prod.CommodityTypeId,
                        CommodityType = commodityType.CommodityCode,
                        Productname = prod.Productname,
                        Productcode = prod.Productcode,
                        ProductDisplayCode = prod.ProductDisplayCode,
                        Productdescription = prod.Productdescription,
                        Productshortdescription = prod.Productshortdescription,
                        Displayimage = prod.Displayimage,
                        Isbundledproduct = prod.Isbundledproduct,
                        Isactive = prod.Isactive,
                        Ismandatoryproduct = prod.Ismandatoryproduct,
                        Entrydatetime = prod.Entrydatetime,
                        Entryuser = prod.Entryuser,
                        Lastupdatedatetime = prod.Lastupdatedatetime,
                        Lastupdateuser = prod.Lastupdateuser,
                        ProductTypeId = prod.ProductTypeId,
                        isChecked = false,
                        Premium = "0",
                        CurrencyCode = "",
                        ProductTypeCode = prodType.Code
                    }).ToList();
        }


        public ProductResponseDto GetProductById(Guid ProductId)
        {
            ISession session = EntitySessionManager.GetSession();

            ProductResponseDto pDto = new ProductResponseDto();

            var query =
                from product in session.Query<Product>()
                where product.Id == ProductId
                join commodityType in session.Query<CommodityType>() on product.CommodityTypeId equals commodityType.CommodityTypeId
                join image in session.Query<Image>() on product.Displayimage equals image.Id
                select new { product = product, commodityType = commodityType, image = image };
            //from product in session.Query<Product>()
            //join commodityType in session.Query<CommodityType>() on product.CommodityTypeId equals commodityType.CommodityTypeId
            //join image in session.Query<Image>() on product.Displayimage equals image.Id
            ////join bundledProduct in session.Query<BundledProduct>() on product.Id equals bundledProduct.ProductId
            ////join parentProduct in session.Query<Product>() on bundledProduct.ParentProductId equals parentProduct.Id
            //where product.Id == ProductId
            //select new { product = product, commodityType = commodityType, image = i };

            var result = query.ToList();

            if (result.Count == 0)
            {
                query =
                from product in session.Query<Product>()
                where product.Id == ProductId
                join commodityType in session.Query<CommodityType>() on product.CommodityTypeId equals commodityType.CommodityTypeId
                select new { product = product, commodityType = commodityType, image = new Image() };
                result = query.ToList();

            }

            var subQuery = from bundledProduct in session.Query<BundledProduct>()
                           join parentProduct in session.Query<Product>() on bundledProduct.ParentProductId equals parentProduct.Id
                           where bundledProduct.ProductId == ProductId && bundledProduct.IsCurrentProduct == true
                           select new { parentProduct = parentProduct, bundledProduct = bundledProduct };



            if (result != null && result.Count > 0)
            {
                var subResult = subQuery.ToList();

                pDto.Id = result.First().product.Id;
                pDto.CommodityTypeId = result.First().commodityType.CommodityTypeId;
                pDto.CommodityType = result.First().commodityType.CommodityTypeDescription;
                pDto.Productname = result.First().product.Productname;
                pDto.Productcode = result.First().product.Productcode;
                pDto.ProductDisplayCode = result.First().product.ProductDisplayCode;
                pDto.Productdescription = result.First().product.Productdescription;
                pDto.Productshortdescription = result.First().product.Productshortdescription;
                pDto.Displayimage = result.First().product.Displayimage;
                pDto.ProductTypeId = result.First().product.ProductTypeId;
                if (result.First().image.ImageByte != null)
                {
                    pDto.DisplayImageSrc = Convert.ToBase64String(result.First().image.ImageByte);
                }
                pDto.Isbundledproduct = result.First().product.Isbundledproduct;
                pDto.Isactive = result.First().product.Isactive;
                pDto.Ismandatoryproduct = result.First().product.Ismandatoryproduct;
                pDto.Entrydatetime = result.First().product.Entrydatetime;
                pDto.Entryuser = result.First().product.Entryuser;
                pDto.Lastupdatedatetime = result.First().product.Lastupdatedatetime;
                pDto.Lastupdateuser = result.First().product.Lastupdateuser;
                pDto.ProductTypeCode =getProductTypeCode(pDto.ProductTypeId);
                pDto.BundledProducts = new List<BundledProductResponseDto>();

                foreach (var parentProduct in subResult)
                {
                    BundledProductResponseDto bp = new BundledProductResponseDto();
                    bp.Id = parentProduct.bundledProduct.Id;
                    bp.IsCurrentProduct = parentProduct.bundledProduct.IsCurrentProduct;
                    bp.ParentProductId = parentProduct.bundledProduct.ParentProductId;
                    bp.ProductId = parentProduct.bundledProduct.ProductId;
                    bp.parentProduct = new ProductResponseDto();
                    bp.parentProduct.Productname = parentProduct.parentProduct.Productname;
                    bp.parentProduct.Id = parentProduct.parentProduct.Id;
                    bp.parentProduct.Productname = parentProduct.parentProduct.Productname;
                    bp.parentProduct.Productcode = parentProduct.parentProduct.Productcode;
                    bp.parentProduct.ProductDisplayCode = parentProduct.parentProduct.ProductDisplayCode;
                    bp.parentProduct.ProductTypeId = parentProduct.parentProduct.ProductTypeId;
                    bp.parentProduct.Productdescription = parentProduct.parentProduct.Productdescription;
                    bp.parentProduct.Productshortdescription = parentProduct.parentProduct.Productshortdescription;
                    bp.parentProduct.Isactive = parentProduct.parentProduct.Isactive;
                    bp.parentProduct.Ismandatoryproduct = parentProduct.parentProduct.Ismandatoryproduct;
                    bp.parentProduct.ProductTypeId = parentProduct.parentProduct.ProductTypeId;

                    pDto.BundledProducts.Add(bp);
                }

                pDto.IsProductExists = true;

                return pDto;
            }
            else
            {
                pDto.IsProductExists = false;

                return pDto;
            }
        }
        private bool IsGuid(string candidate)
        {
            if (candidate == "00000000-0000-0000-0000-000000000000")
            {
                return false;
            }

            Regex isGuid = new Regex(@"^(\{){0,1}[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}(\}){0,1}$", RegexOptions.Compiled);

            if (candidate != null)
            {
                if (isGuid.IsMatch(candidate))
                {
                    return true;
                }
            }

            return false;
        }

        internal String getProductTypeCode(Guid productTypeId)
        {
            String Response = null;

            try
            {
                if (IsGuid(productTypeId.ToString()))
                {
                    ISession session = EntitySessionManager.GetSession();
                    ProductType productType = session.Query<ProductType>().FirstOrDefault(a => a.Id == productTypeId);
                    if (productType != null)
                    {
                        Response = productType.Code;
                    }

                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }

            return Response;
        }

        internal List<ProductResponseDto> GetAllProductsByCommodityTypeId(Guid commodityTypeId)
        {
            List<ProductResponseDto> Response = null;
            try
            {
                // ---Sachith slowness issue fixed

                /*

                ISession session = EntitySessionManager.GetSession();
                List<ProductResponseDto> ProductTypesResponseDto = new List<ProductResponseDto>();
                List<Product> products = session.Query<Product>().Where(a => a.CommodityTypeId == commodityTypeId).ToList();

                foreach (var prod in products)
                {
                    //List<BundledProduct> bundledProducts = session.Query<BundledProduct>().Where(a => a.ProductId == prod.Id).ToList();
                    ProductType productTypes = session.Query<ProductType>().Where(a => a.Id == prod.ProductTypeId).FirstOrDefault();

                    ProductResponseDto productTypesResponseDtos = new ProductResponseDto()
                    {
                        BundledProducts = null,
                        CommodityType = new CommonEntityManager().GetCommodityTypeUniqueCodeById(prod.CommodityTypeId),
                        CommodityTypeId = prod.CommodityTypeId,
                        CurrencyCode = "",
                        Displayimage = prod.Displayimage,
                        DisplayImageSrc = null,
                        Entrydatetime = prod.Entrydatetime,
                        Entryuser = prod.Entryuser,
                        Id = prod.Id,
                        Isactive = prod.Isactive,
                        Isbundledproduct = prod.Isbundledproduct,
                        isChecked = false,
                        Ismandatoryproduct = prod.Ismandatoryproduct,
                        Lastupdatedatetime = prod.Lastupdatedatetime,
                        Lastupdateuser = prod.Lastupdateuser,
                        Premium = "0",
                        Productcode = new CommonEntityManager().GetProductCodeById(prod.Id),
                        Productdescription = prod.Productdescription,
                        Productname = prod.Productname,
                        Productshortdescription = prod.Productshortdescription,
                        ProductTypeCode = productTypes.Code,
                        ProductTypeId = productTypes.Id
                };
                    ProductTypesResponseDto.Add(productTypesResponseDtos);
                }

                */

                ISession session = EntitySessionManager.GetSession();
                return (from prod in session.Query<Product>().Where(a => a.CommodityTypeId == commodityTypeId)
                        join commodityType in session.Query<CommodityType>() on prod.CommodityTypeId equals commodityType.CommodityTypeId
                        join prodType in session.Query<ProductType>() on prod.ProductTypeId equals prodType.Id
                        select new ProductResponseDto
                        {
                            BundledProducts = null,
                            CommodityType = commodityType.CommodityCode,
                            CommodityTypeId = prod.CommodityTypeId,
                            CurrencyCode = "",
                            Displayimage = prod.Displayimage,
                            DisplayImageSrc = null,
                            Entrydatetime = prod.Entrydatetime,
                            Entryuser = prod.Entryuser,
                            Id = prod.Id,
                            Isactive = prod.Isactive,
                            Isbundledproduct = prod.Isbundledproduct,
                            isChecked = false,
                            Ismandatoryproduct = prod.Ismandatoryproduct,
                            Lastupdatedatetime = prod.Lastupdatedatetime,
                            Lastupdateuser = prod.Lastupdateuser,
                            Premium = "0",
                            Productcode = prod.Productcode,
                            Productdescription = prod.Productdescription,
                            Productname = prod.Productname,
                            Productshortdescription = prod.Productshortdescription,
                            ProductTypeCode = prodType.Code,
                            ProductTypeId = prodType.Id
                        }).ToList();

            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }

            return Response;
        }



        public ProductsResponseDto GetProductWithBundleData()
        {
            ISession session = EntitySessionManager.GetSession();
            ProductsResponseDto pDto = new ProductsResponseDto();
            var query =
                from product in session.Query<Product>()
                join commodityType in session.Query<CommodityType>() on product.CommodityTypeId equals commodityType.CommodityTypeId
                join image in session.Query<Image>() on product.Displayimage equals image.Id
                select new { product = product, commodityType = commodityType, image = image };
            var result = query.ToList();

            var subQuery = from bundledProduct in session.Query<BundledProduct>()
                           join parentProduct in session.Query<Product>() on bundledProduct.ParentProductId equals parentProduct.Id
                           where bundledProduct.IsCurrentProduct == true
                           select new { parentProduct = parentProduct, bundledProduct = bundledProduct };

            if (result != null && result.Count > 0)
            {
                var subResult = subQuery.ToList();
                foreach (var prod in result)
                {
                    List<BundledProductResponseDto> bpList = new List<BundledProductResponseDto>();
                    foreach (var parentProduct in subResult)
                    {
                        BundledProductResponseDto bp = new BundledProductResponseDto();
                        bp.Id = parentProduct.bundledProduct.Id;
                        bp.IsCurrentProduct = parentProduct.bundledProduct.IsCurrentProduct;
                        bp.ParentProductId = parentProduct.bundledProduct.ParentProductId;
                        bp.ProductId = parentProduct.bundledProduct.ProductId;
                        bp.parentProduct = new ProductResponseDto();
                        bp.parentProduct.Productname = parentProduct.parentProduct.Productname;
                        bp.parentProduct.Id = parentProduct.parentProduct.Id;
                        bp.parentProduct.Productname = parentProduct.parentProduct.Productname;
                        bp.parentProduct.Productcode = parentProduct.parentProduct.Productcode;
                        bp.parentProduct.ProductTypeId = parentProduct.parentProduct.ProductTypeId;
                        bp.parentProduct.Productdescription = parentProduct.parentProduct.Productdescription;
                        bp.parentProduct.Productshortdescription = parentProduct.parentProduct.Productshortdescription;
                        bp.parentProduct.Isactive = parentProduct.parentProduct.Isactive;
                        bp.parentProduct.Ismandatoryproduct = parentProduct.parentProduct.Ismandatoryproduct;
                        bp.parentProduct.ProductTypeId = parentProduct.parentProduct.ProductTypeId;
                        bpList.Add(bp);
                    }
                    pDto.Products.Add(new ProductResponseDto()
                    {
                        Id = prod.product.Id,
                        CommodityTypeId = prod.commodityType.CommodityTypeId,
                        CommodityType = prod.commodityType.CommodityTypeDescription,
                        Productname = prod.product.Productname,
                        Productcode = prod.product.Productcode,
                        Productdescription = prod.product.Productdescription,
                        Productshortdescription = prod.product.Productshortdescription,
                        Displayimage = prod.product.Displayimage,
                        ProductTypeId = prod.product.ProductTypeId,
                        DisplayImageSrc = Convert.ToBase64String(prod.image.ImageByte),
                        Isbundledproduct = prod.product.Isbundledproduct,
                        Isactive = prod.product.Isactive,
                        Ismandatoryproduct = prod.product.Ismandatoryproduct,
                        Entrydatetime = prod.product.Entrydatetime,
                        Entryuser = prod.product.Entryuser,
                        Lastupdatedatetime = prod.product.Lastupdatedatetime,
                        Lastupdateuser = prod.product.Lastupdateuser,
                        BundledProducts = bpList,
                        IsProductExists = true
                    });
                }
                return pDto;
            }
            else
            {
                return pDto;
            }
        }

        internal bool AddProduct(ProductRequestDto Product)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();
                Product pr = new Entities.Product();
                BundledProduct bpr = new Entities.BundledProduct();

                pr.Id = new Guid();
                pr.CommodityTypeId = Product.CommodityTypeId;
                pr.Productname = Product.Productname;
                pr.Productcode = GetProductCodeByProductTypeId(Product.ProductTypeId);
                pr.ProductDisplayCode = Product.ProductDisplayCode;
                pr.Productdescription = Product.Productdescription;
                pr.Productshortdescription = Product.Productshortdescription;
                pr.Displayimage = Product.Displayimage;
                pr.ProductTypeId = Product.ProductTypeId;
                pr.Isbundledproduct = Product.Isbundledproduct;
                pr.Isactive = Product.Isactive;
                pr.Ismandatoryproduct = Product.Ismandatoryproduct;
                pr.Entrydatetime = DateTime.Today.ToUniversalTime();
                pr.Entryuser = Guid.Parse("ba56ec84-1fe0-4385-abe4-182f62caa050");
                pr.Isbundledproduct = Product.selectedpp.Count() > 0 ? true : false;

                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.SaveOrUpdate(pr);

                    foreach (var item in Product.selectedpp)
                    {
                        BundledProduct BP = new BundledProduct();

                        BP.Id = new Guid();
                        BP.ProductId = pr.Id;
                        BP.ParentProductId = Guid.Parse(item);
                        BP.IsCurrentProduct = true;

                        session.SaveOrUpdate(BP);
                    }

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


        internal bool UpdateProduct(ProductRequestDto Product)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();
                Product pr = new Entities.Product();

                CommodityType CommodityType = session.Query<CommodityType>()
                    .Where(a => a.CommodityTypeId == Product.CommodityTypeId).FirstOrDefault();



                pr.Id = Product.Id;
                pr.CommodityTypeId = Product.CommodityTypeId;
                pr.Productname = Product.Productname;
                pr.ProductDisplayCode = Product.ProductDisplayCode;
                //pr.Productcode = GetProductCodeByProductTypeId(Product.ProductTypeId);
                pr.Productdescription = Product.Productdescription;
                pr.Productshortdescription = Product.Productshortdescription;
                pr.Displayimage = Product.Displayimage;
                pr.ProductTypeId = Product.ProductTypeId;
                pr.Isbundledproduct = Product.Isbundledproduct;
                pr.Isactive = Product.Isactive;
                pr.Ismandatoryproduct = Product.Ismandatoryproduct;
                pr.Lastupdatedatetime = DateTime.Today.ToUniversalTime();
                pr.Lastupdateuser = Guid.Parse("ba56ec84-1fe0-4385-abe4-182f62caa050");
                pr.Entrydatetime = Product.Entrydatetime;
                pr.Entryuser = Product.Entryuser;
                pr.Isbundledproduct = Product.selectedpp.Count() > 0 ? true : false;

                if (CommodityType.CommodityTypeDescription == "Tire")
                {
                    pr.Productcode = "TYRE";
                }
                else
                {
                    pr.Productcode = GetProductCodeByProductTypeId(Product.ProductTypeId);
                }

                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Update(pr);

                    foreach (var item in session.Query<BundledProduct>().Where(a => a.ProductId == Product.Id).ToList())
                    {
                        item.IsCurrentProduct = false;
                        session.Update(item);
                    }
                    foreach (var item in Product.selectedpp)
                    {
                        BundledProduct BP = new BundledProduct();
                        BP.Id = new Guid();
                        BP.ProductId = Product.Id;
                        BP.ParentProductId = Guid.Parse(item);
                        BP.IsCurrentProduct = true;
                        session.Save(BP);
                    }
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

        internal List<TPAProductResponseDto> GetProductsByTPAId(Guid TPAId)
        {
            ISession session = EntitySessionManager.GetSession();
            List<TPAProductResponseDto> tpaProducts = new List<TPAProductResponseDto>();


            var data = session.Query<Product>()
                .Join(session.Query<CommodityType>(), a => a.CommodityTypeId, b => b.CommodityTypeId, (a, b) => new { a, b })
                // .Join(session.Query<Image>(), c => c.a.Displayimage, d => d.Id, (c, d) => new { c, d })
                .Select(x => new
                {
                    x.a.Id,
                    x.b.CommodityTypeId,
                    x.b.CommodityTypeDescription,
                    x.b.DisplayDescription,
                    x.a.Productname,
                    x.a.Productcode,
                    x.a.ProductTypeId,
                    x.a.Productdescription,
                    x.a.Productshortdescription,
                    x.a.Displayimage,
                    x.a.Isbundledproduct,
                    x.a.Ismandatoryproduct,
                    x.a.Isactive,
                    x.a.Entrydatetime,
                    x.a.Entryuser,
                    x.a.Lastupdatedatetime,
                    x.a.Lastupdateuser
                })
                .OrderBy(z => z.Productname)
                .ToArray();
            foreach (var tpaProduct in data)
            {
                TPAProductResponseDto pDto = new TPAProductResponseDto();
                pDto.Id = tpaProduct.Id;
                pDto.CommodityTypeId = tpaProduct.CommodityTypeId;
                pDto.CommodityType = tpaProduct.CommodityTypeDescription;
                pDto.CommodityTypeDisplayDescription = tpaProduct.DisplayDescription;
                pDto.Productname = tpaProduct.Productname;
                pDto.Productcode = tpaProduct.Productcode;
                pDto.ProductTypeId = tpaProduct.ProductTypeId;
                pDto.Productdescription = tpaProduct.Productdescription;
                pDto.Productshortdescription = tpaProduct.Productshortdescription;
                pDto.DisplayImageSrc = new ImageEntityManager().GetImageBase64ById(tpaProduct.Displayimage);
                pDto.Isbundledproduct = tpaProduct.Isbundledproduct;
                pDto.Isactive = tpaProduct.Isactive;
                pDto.Ismandatoryproduct = tpaProduct.Ismandatoryproduct;
                pDto.Entrydatetime = tpaProduct.Entrydatetime;
                pDto.Entryuser = tpaProduct.Entryuser;
                pDto.Lastupdatedatetime = tpaProduct.Lastupdatedatetime;
                pDto.Lastupdateuser = tpaProduct.Lastupdateuser;
                pDto.BundledProducts = new List<BundledProductResponseDto>();

                tpaProducts.Add(pDto);
            }
            //  var p = data.ToList();

            //var query =
            //    from product in session.Query<Product>()
            //    join commodityType in session.Query<CommodityType>()
            //            on product.CommodityTypeId equals commodityType.CommodityTypeId
            //    join image in session.Query<Image>() on product.Displayimage equals image.Id into img
            //    from image in img.DefaultIfEmpty()
            //    select new { product = product, commodityType = commodityType, image = image };

            //var result = query.ToList().OrderBy(a => a.commodityType.CommodityTypeDescription);

            //if (result != null)
            //{
            //    foreach (var tpaProduct in result)
            //    {
            //        TPAProductResponseDto pDto = new TPAProductResponseDto();
            //        pDto.Id = tpaProduct.product.Id;
            //        pDto.CommodityTypeId = tpaProduct.commodityType.CommodityTypeId;
            //        pDto.CommodityType = tpaProduct.commodityType.CommodityTypeDescription;
            //        pDto.CommodityTypeDisplayDescription = tpaProduct.commodityType.DisplayDescription;
            //        pDto.Productname = tpaProduct.product.Productname;
            //        pDto.Productcode = tpaProduct.product.Productcode;
            //        pDto.ProductTypeId = tpaProduct.product.ProductTypeId;
            //        pDto.Productdescription = tpaProduct.product.Productdescription;
            //        pDto.Productshortdescription = tpaProduct.product.Productshortdescription;
            //        pDto.DisplayImageSrc = Convert.ToBase64String(tpaProduct.image.ImageByte);
            //        pDto.Isbundledproduct = tpaProduct.product.Isbundledproduct;
            //        pDto.Isactive = tpaProduct.product.Isactive;
            //        pDto.Ismandatoryproduct = tpaProduct.product.Ismandatoryproduct;
            //        pDto.Entrydatetime = tpaProduct.product.Entrydatetime;
            //        pDto.Entryuser = tpaProduct.product.Entryuser;
            //        pDto.Lastupdatedatetime = tpaProduct.product.Lastupdatedatetime;
            //        pDto.Lastupdateuser = tpaProduct.product.Lastupdateuser;
            //        pDto.BundledProducts = new List<BundledProductResponseDto>();

            //        tpaProducts.Add(pDto);
            //    }
            //    return tpaProducts;
            //}
            //    else
            //    {
            //        return tpaProducts;
            //    }
            return tpaProducts;
        }

        public List<BundledProduct> GetBundleProducts()
        {
            List<BundledProduct> entities = null;
            ISession session = EntitySessionManager.GetSession();
            IQueryable<BundledProduct> productData = session.Query<BundledProduct>();
            entities = productData.ToList();
            return entities;
        }

        private string GetProductCodeByProductTypeId(Guid productTypeId)
        {

            if (productTypeId != null && productTypeId != Guid.Empty)
            {
                ISession session = EntitySessionManager.GetSession();
                if (session.Query<ProductType>().Where(a => a.Id == productTypeId).Count() == 0)
                    return String.Empty;
                return session.Query<ProductType>().Where(a => a.Id == productTypeId).FirstOrDefault().Code;
            }
            else
            {
                return String.Empty;
            }
        }


        internal int IsExsistingProductName(Guid Id, string ProductName)
        {
            ISession session = EntitySessionManager.GetSession();

            if (Id == Guid.Empty)
            {
                var query =
                    from Product in session.Query<Product>()
                    where Product.Productname == ProductName
                    select new { Product = Product };

                return query.Count();
            }
            else
            {
                var query =
                    from Product in session.Query<Product>()
                    where Product.Id != Id && Product.Productname == ProductName
                    select new { Product = Product };

                return query.Count();
            }
        }

        internal int IsExsistingProductDescription(Guid Id, string Discription)
        {
            throw new NotImplementedException();
        }
    }
}
