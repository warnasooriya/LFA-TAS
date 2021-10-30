using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using TAS.Web.Common;
using TAS.Services;
using TAS.DataTransfer.Responses;
using TAS.Services.Entities;
using TAS.DataTransfer.Requests;
using Newtonsoft.Json;
using NLog;
using System.Reflection;

namespace TAS.Web.Controllers
{
    public class ProductController : ApiController
    {
        //private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public string AddProduct(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                ProductRequestDto product = data.ToObject<ProductRequestDto>(); //JsonConvert.DeserializeObject<List<ProductRequestDto>>(data);
                IProductManagementService productManagementService = ServiceFactory.GetProductManagementService();
                ProductRequestDto result = productManagementService.AddProduct(product, SecurityHelper.Context, AuditHelper.Context);
                logger.Info("Product Added");
                if (result.ProductInsertion)
                {
                    return "OK";
                }
                else
                {
                    return "Add product failed!";
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return "Add product failed!";
            }

        }

        [HttpPost]
        public bool IsExsistingProductName(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                IProductManagementService productManagementService = ServiceFactory.GetProductManagementService();

                var isExsists =
                    productManagementService.IsExsistingProductName(
                        Guid.Parse(data["Id"].ToString()),data["ProductName"].ToString(),
                        SecurityHelper.Context,
                        AuditHelper.Context);
                return isExsists;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return false;
            }

        }

        public string UpdateProduct(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                ProductRequestDto product = data.ToObject<ProductRequestDto>();
                IProductManagementService productManagementService = ServiceFactory.GetProductManagementService();
                ProductRequestDto result = productManagementService.UpdateProduct(product, SecurityHelper.Context, AuditHelper.Context);
                logger.Info("Product Added");
                if (result.ProductInsertion)
                {
                    return "OK";
                }
                else
                {
                    return "Add product failed!";
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return "Add product failed!";
            }

        }

        [HttpPost]
        public object GetProductById(JObject data)
        {

            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            IProductManagementService productManagementService = ServiceFactory.GetProductManagementService();

            ProductResponseDto product = productManagementService.GetProductById(Guid.Parse(data["productId"].ToString()),
                SecurityHelper.Context,
                AuditHelper.Context);

            return product;

        }

        [HttpPost]
        public object GetAllProducts(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            IProductManagementService productManagementService = ServiceFactory.GetProductManagementService();

            ProductsResponseDto productData = productManagementService.GetProducts(
            SecurityHelper.Context,
            AuditHelper.Context);
            if (data == null)
                return productData.Products.ToArray();
            if (productData.Products != null)
            {
                return productData.Products.FindAll(p => p.CommodityTypeId == Guid.Parse(data["Id"].ToString())).ToArray();
            }
            return null;
        }

        [HttpPost]
        public object GetProducts()
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            IProductManagementService productManagementService = ServiceFactory.GetProductManagementService();

            ProductsResponseDto productData = productManagementService.GetProducts(
            SecurityHelper.Context,
            AuditHelper.Context);
            //foreach (var item in productData.Products)
            //{
            //    item.ProductTypeCode = GetProductTypeCodeByProductTypeId(item.ProductTypeId, Request.Headers.Authorization.ToString());
            //}
            return productData.Products.ToArray();
        }

        [HttpPost]
        public object GetAllChildProducts(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            IProductManagementService productManagementService = ServiceFactory.GetProductManagementService();

            ProductsResponseDto productData = productManagementService.GetChildProductsbyParentId(Guid.Parse(data["Id"].ToString()),
            SecurityHelper.Context,
            AuditHelper.Context);
            if (productData.Products != null)
            {
                return productData.Products.ToArray();
            }
            return null;
        }

        [HttpPost]
        public object GetProductTypes()
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            IProductTypeManagementService productTypeManagementService = ServiceFactory.GetProductTypeManagementService();

            ProductTypesResponseDto productData = productTypeManagementService.GetProductTypes(
            SecurityHelper.Context,
            AuditHelper.Context);

            return productData.ProductTypes.ToArray();
        }

        [HttpPost]
        public object GetProductTypesById(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            IProductTypeManagementService productTypeManagementService = ServiceFactory.GetProductTypeManagementService();

            ProductTypesResponseDto productData = productTypeManagementService.GetProductTypes(
            SecurityHelper.Context,
            AuditHelper.Context);
            return productData.ProductTypes.Find(p => p.Id == Guid.Parse(data["Id"].ToString()));
        }

        [HttpPost]
        public object GetAllProductsByCommodityTypeId2(JObject data)
        {
            object Response = null;
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                IProductManagementService productManagementService = ServiceFactory.GetProductManagementService();
                var CommodityTypeId = Guid.Parse(data["Id"].ToString());

                Response = productManagementService.GetAllProductsByCommodityTypeId(CommodityTypeId, SecurityHelper.Context,
                    AuditHelper.Context);
                return Response;

            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }

            //return Response;
        }

        [HttpPost]
        public object GetAllProductsByCommodityTypeId(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                IProductManagementService productManagementService = ServiceFactory.GetProductManagementService();
                ProductsResponseDto result = new ProductsResponseDto();
                result.Products = new List<ProductResponseDto>();

                ProductsResponseDto products = productManagementService.GetProducts(
                    SecurityHelper.Context,
                    AuditHelper.Context);
                if (products == null)
                    return new ProductsResponseDto();
                //foreach (var prod in products.Products.FindAll(p => p.CommodityTypeId == Guid.Parse(data["Id"].ToString()) && p.Isactive).ToArray())
                //{
                //    prod.ProductTypeCode = GetProductTypeCodeByProductTypeId(prod.ProductTypeId, Request.Headers.Authorization.ToString());
                //    result.Products.Add(prod);

                //}
                return products.Products.Where(p => p.CommodityTypeId == Guid.Parse(data["Id"].ToString()) && p.Isactive).ToList();
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }
        }

        public String GetProductTypeCodeByProductTypeId(Guid ProductTypeId,String Auth)
        {
            IProductTypeManagementService productTypeManagementService = ServiceFactory.GetProductTypeManagementService();
            SecurityHelper.Context.setToken(Auth);
            ProductTypesResponseDto productData = productTypeManagementService.GetProductTypes(
            SecurityHelper.Context,
            AuditHelper.Context);
            if (productData.ProductTypes.Find(p => p.Id == ProductTypeId) == null)
                return "";
            return productData.ProductTypes.Find(p => p.Id == ProductTypeId).Code;
        }



        [HttpPost]
        public object GetAllBasicProductsByCommodityTypeId(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                IProductManagementService productManagementService = ServiceFactory.GetProductManagementService();
                ProductsResponseDto result = new ProductsResponseDto();
                result.Products = new List<ProductResponseDto>();

                ProductsResponseDto products = productManagementService.GetChildProductsbyParentId(Guid.Parse("00000000-0000-0000-0000-000000000000"),
                    SecurityHelper.Context,
                    AuditHelper.Context);
                foreach (var prod in products.Products.FindAll(p => p.Isactive && p.CommodityTypeId == Guid.Parse(data["Id"].ToString())))
                {
                    prod.ProductTypeCode = GetProductTypeCodeByProductTypeId(prod.ProductTypeId, Request.Headers.Authorization.ToString());
                    result.Products.Add(prod);

                }
                return result.Products.ToArray();
            }
            catch(Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }
        }

        [HttpPost]
        public object GetAllCommodities()
        {
            ICommodityManagementService commodityManagementService = ServiceFactory.GetCommodityManagementService();
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            CommoditiesResponseDto commodityData = commodityManagementService.GetAllCommodities(
            SecurityHelper.Context,
            AuditHelper.Context);
            return commodityData.Commmodities.ToArray();
        }

        [HttpPost]
        public object GetAllParentProducts()
        {
            try
            {
                IProductManagementService productManagementService = ServiceFactory.GetProductManagementService();
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                ProductsResponseDto productData = productManagementService.GetParentProducts(
                SecurityHelper.Context,
                AuditHelper.Context);
                if (productData.Products != null)
                {
                    return productData.Products.ToArray();
                }
                return null;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }

        }

        [HttpPost]
        public object GetProductsByTPA(JObject data)
        {
            IProductManagementService productManagementService = ServiceFactory.GetProductManagementService();
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            Guid TPAId = new Guid();   // Temp new Guid. should assign the tpa id coming from jObject data
            TPAId = Guid.Parse(data["tpaId"].ToString());

            TPAProductsResponseDto productData = productManagementService.GetProductsByTPA(TPAId,
            SecurityHelper.Context,
            AuditHelper.Context);
            return productData.TPAProducts.ToArray();
        }
    }
}
