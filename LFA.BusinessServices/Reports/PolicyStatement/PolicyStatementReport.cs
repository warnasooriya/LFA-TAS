using NHibernate;
using NHibernate.Linq;
using System;
using System.IO;
using System.Linq;
using TAS.Services.Common;
using TAS.Services.Entities;
using TAS.Services.Entities.Management;
using TAS.Services.Entities.Persistence;
using TAS.Services.Reports.Common;

namespace TAS.Services.Reports.PolicyStatement
{
    public class PolicyStatementReport : HtmlPdfConverter
    {
        private static readonly Lazy<PolicyStatementReport> lazy = new Lazy<PolicyStatementReport>(() => new PolicyStatementReport());
        public static PolicyStatementReport Instance { get { return lazy.Value; } }
        private readonly String ReportPath, PdfType = String.Empty;

        public PolicyStatementReport()
        {
            ReportPath = ConfigurationData.ReportsPath;
            PdfType = ConfigurationData.PdfType;
        }

        public Byte[] Generate(String DBName, String ProductCode, Guid PolicyId)
        {
            String FormattedHTML, ReportHTML = String.Empty;

            ISession session = EntitySessionManager.GetSession();
            CommonEntityManager cem = new CommonEntityManager();
            //PolicyBundle PolicyBundle = session.Query<PolicyBundle>().Where(a => a.Id == policyId).FirstOrDefault();
            Policy policy = session.Query<Policy>().Where(a => a.PolicyBundleId == PolicyId).FirstOrDefault();

            Customer customer = session.Query<Customer>().Where(a => a.Id == policy.CustomerId).FirstOrDefault();
            CustomerType customerType = session.Query<CustomerType>().Where(a => a.Id == customer.CustomerTypeId).FirstOrDefault();
            Product Product = session.Query<Product>().Where(a => a.Id == policy.ProductId).FirstOrDefault();

            CommodityType commodityType = session.Query<CommodityType>().Where(a => a.CommodityTypeId ==policy.CommodityTypeId).FirstOrDefault();
            string Commodity = commodityType.CommodityTypeDescription;

            if (customerType != null && customerType.CustomerTypeName == "Corporate")
            {
                String ExactHTMLPath = System.Web.HttpContext.Current.Server.MapPath(ReportPath + "\\PolicyStatement\\" + Commodity + "\\" + ProductCode + "\\PolicyStatement_automobile_Coperate.html");
                if (!File.Exists(ExactHTMLPath))
                {
                    String DefaultReportPath = System.Web.HttpContext.Current.Server.MapPath(ReportPath + "\\PolicyStatement\\Default\\" + ProductCode + "\\PolicyStatement_automobile_Coperate.html");
                    ReportHTML = File.ReadAllText(DefaultReportPath);
                }
                else
                {
                    ReportHTML = File.ReadAllText(ExactHTMLPath);
                }
            }
            else
            {
                String ExactHTMLPath = System.Web.HttpContext.Current.Server.MapPath(ReportPath + "\\PolicyStatement\\" + Commodity + "\\" + ProductCode + "\\PolicyStatement_automobile.html");
                if (!File.Exists(ExactHTMLPath))
                {
                    String DefaultReportPath = System.Web.HttpContext.Current.Server.MapPath(ReportPath + "\\PolicyStatement\\Default\\" + ProductCode + "\\PolicyStatement_automobile.html");
                    ReportHTML = File.ReadAllText(DefaultReportPath);
                }
                else
                {
                    ReportHTML = File.ReadAllText(ExactHTMLPath);
                }
            }


            FormattedHTML = new ReportEntityManager().SetupPolicyStatement(ReportHTML, PolicyId);
            if (String.IsNullOrEmpty(FormattedHTML))
            {
                FormattedHTML = "<head></head><body><h3>Reqiured Document Not Found.</h3></body>";
            }

            Byte[] bytes;
            if (PdfType.ToLower() == "crowd")
            {
                bytes = ConvertToPDFNew(FormattedHTML);
            }
            else if (PdfType.ToLower() == "nreco")
            {
                bytes = ConvertToPDFNReco(FormattedHTML);

            }
            else if (PdfType.ToLower() == "api")
            {

                bytes = ConvertToPDFByExternalAPI(FormattedHTML);
            }
            else
            {
                bytes = ConvertToPDF(FormattedHTML);
            }
            return cropbox(bytes);
        }


    }
}
