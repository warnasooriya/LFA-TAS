using NHibernate;
using NHibernate.Linq;
using NHibernate.Transform;
using System.Linq;
using System;
using System.IO;
using System.Threading.Tasks;
using TAS.Services.Common;
using TAS.Services.Entities;
using TAS.Services.Entities.Management;
using TAS.Services.Entities.Persistence;
using TAS.Services.Reports.Common;

namespace TAS.Services.Reports.ClaimAuthorization
{
    public class ClaimAuthorizationReport : HtmlPdfConverter
    {
        private static readonly Lazy<ClaimAuthorizationReport> lazy = new Lazy<ClaimAuthorizationReport>(() => new ClaimAuthorizationReport());
        public static ClaimAuthorizationReport Instance { get { return lazy.Value; } }
        private readonly String ReportPath, PdfType = String.Empty;

        public ClaimAuthorizationReport()
        {
            ReportPath = ConfigurationData.ReportsPath;
            PdfType = ConfigurationData.PdfType;
        }


        internal byte[] Generate(Guid claimId, Guid userId, string UserTypeCode, string tpaName)
        {
            string ReportHTMLBody = string.Empty, ReportHTMLDtl = string.Empty;
            string formattedHtml = string.Empty;

            ISession session = EntitySessionManager.GetSession();

            CommonEntityManager commEn = new CommonEntityManager();
            
            Claim claim = session.Query<Claim>().Where(a => a.Id == claimId).FirstOrDefault();
            string CommodityType = commEn.GetCommodityTypeNameById(claim.CommodityTypeId);

            if (UserTypeCode.ToLower().Trim() == "iu")
            {
                if (CommodityType == "Tire")
                {
                    string ExactHTMLBodyPath = System.Web.HttpContext.Current.Server.MapPath(ReportPath + "\\ClaimAuthorization\\" + tpaName + "\\Tpa" + "\\ClaimsAuthorizationBodyContinental.html");
                    string ExactHTMLDtlPath = System.Web.HttpContext.Current.Server.MapPath(ReportPath + "\\ClaimAuthorization\\" + tpaName + "\\Tpa" + "\\ClaimsAuthorizationTireDtl.html");

                    if (!File.Exists(ExactHTMLBodyPath) || !File.Exists(ExactHTMLDtlPath))
                    {
                        String DefaultReportBodyPath = System.Web.HttpContext.Current.Server.MapPath(ReportPath + "\\ClaimAuthorization\\Default\\Tpa" + "\\ClaimsAuthorizationBodyContinental.html");
                        String DefaultReportDtlPath = System.Web.HttpContext.Current.Server.MapPath(ReportPath + "\\ClaimAuthorization\\Default\\Tpa" + "\\ClaimsAuthorizationTireDtl.html");

                        ReportHTMLBody = File.ReadAllText(DefaultReportBodyPath);
                        ReportHTMLDtl = File.ReadAllText(DefaultReportDtlPath);
                    }
                    else
                    {
                        ReportHTMLBody = File.ReadAllText(ExactHTMLBodyPath);
                        ReportHTMLDtl = File.ReadAllText(ExactHTMLDtlPath);
                    }

                    formattedHtml = new ReportEntityManager().SetupClaimAuthorizationTireReport(ReportHTMLBody, ReportHTMLDtl, claimId, userId, tpaName);
                }
                else
                {
                    string ExactHTMLBodyPath = System.Web.HttpContext.Current.Server.MapPath(ReportPath + "\\ClaimAuthorization\\" + tpaName + "\\Tpa" + "\\ClaimsAuthorizationBody.html");
                    string ExactHTMLDtlPath = System.Web.HttpContext.Current.Server.MapPath(ReportPath + "\\ClaimAuthorization\\" + tpaName + "\\Tpa" + "\\ClaimsAuthorizationDtl.html");

                    if (!File.Exists(ExactHTMLBodyPath) || !File.Exists(ExactHTMLDtlPath))
                    {
                        String DefaultReportBodyPath = System.Web.HttpContext.Current.Server.MapPath(ReportPath + "\\ClaimAuthorization\\Default\\Tpa" + "\\ClaimsAuthorizationBody.html");
                        String DefaultReportDtlPath = System.Web.HttpContext.Current.Server.MapPath(ReportPath + "\\ClaimAuthorization\\Default\\Tpa" + "\\ClaimsAuthorizationDtl.html");

                        ReportHTMLBody = File.ReadAllText(DefaultReportBodyPath);
                        ReportHTMLDtl = File.ReadAllText(DefaultReportDtlPath);
                    }
                    else
                    {
                        ReportHTMLBody = File.ReadAllText(ExactHTMLBodyPath);
                        ReportHTMLDtl = File.ReadAllText(ExactHTMLDtlPath);
                    }
                    formattedHtml = new ReportEntityManager().SetupClaimAuthorizationReport(ReportHTMLBody, ReportHTMLDtl, claimId, userId, tpaName);
                }

                
            }
            else if (UserTypeCode.ToLower().Trim() == "du")
            {
                if (CommodityType == "Tire")
                {
                    string ExactHTMLBodyPath = System.Web.HttpContext.Current.Server.MapPath(ReportPath + "\\ClaimAuthorization\\" + tpaName + "\\Dealer" + "\\ClaimsAuthorizationBodyContinental.html");
                    string ExactHTMLDtlPath = System.Web.HttpContext.Current.Server.MapPath(ReportPath + "\\ClaimAuthorization\\" + tpaName + "\\Dealer" + "\\ClaimsAuthorizationTireDtl.html");

                    if (!File.Exists(ExactHTMLBodyPath) || !File.Exists(ExactHTMLDtlPath))
                    {
                        String DefaultReportBodyPath = System.Web.HttpContext.Current.Server.MapPath(ReportPath + "\\ClaimAuthorization\\Default\\Dealer" + "\\ClaimsAuthorizationBodyContinental.html");
                        String DefaultReportDtlPath = System.Web.HttpContext.Current.Server.MapPath(ReportPath + "\\ClaimAuthorization\\Default\\Dealer" + "\\ClaimsAuthorizationTireDtl.html");

                        ReportHTMLBody = File.ReadAllText(DefaultReportBodyPath);
                        ReportHTMLDtl = File.ReadAllText(DefaultReportDtlPath);
                    }
                    else
                    {
                        ReportHTMLBody = File.ReadAllText(ExactHTMLBodyPath);
                        ReportHTMLDtl = File.ReadAllText(ExactHTMLDtlPath);
                    }
                    formattedHtml = new ReportEntityManager().SetupClaimAuthorizationTireReport(ReportHTMLBody, ReportHTMLDtl, claimId, userId, tpaName);
                }
                else
                {
                    string ExactHTMLBodyPath = System.Web.HttpContext.Current.Server.MapPath(ReportPath + "\\ClaimAuthorization\\" + tpaName + "\\Dealer" + "\\ClaimsAuthorizationBody.html");
                    string ExactHTMLDtlPath = System.Web.HttpContext.Current.Server.MapPath(ReportPath + "\\ClaimAuthorization\\" + tpaName + "\\Dealer" + "\\ClaimsAuthorizationDtl.html");

                    if (!File.Exists(ExactHTMLBodyPath) || !File.Exists(ExactHTMLDtlPath))
                    {
                        String DefaultReportBodyPath = System.Web.HttpContext.Current.Server.MapPath(ReportPath + "\\ClaimAuthorization\\Default\\Dealer" + "\\ClaimsAuthorizationBody.html");
                        String DefaultReportDtlPath = System.Web.HttpContext.Current.Server.MapPath(ReportPath + "\\ClaimAuthorization\\Default\\Dealer" + "\\ClaimsAuthorizationDtl.html");

                        ReportHTMLBody = File.ReadAllText(DefaultReportBodyPath);
                        ReportHTMLDtl = File.ReadAllText(DefaultReportDtlPath);
                    }
                    else
                    {
                        ReportHTMLBody = File.ReadAllText(ExactHTMLBodyPath);
                        ReportHTMLDtl = File.ReadAllText(ExactHTMLDtlPath);
                    }

                     formattedHtml = new ReportEntityManager().SetupClaimAuthorizationReport(ReportHTMLBody, ReportHTMLDtl, claimId, userId, tpaName);

                }
                
            }

            
            Byte[] bytes;
            if (PdfType.ToLower() == "crowd")
            {
                bytes = ConvertToPDFNew(formattedHtml);
            }
            else if (PdfType.ToLower() == "nreco")
            {
                bytes = ConvertToPDFNReco(formattedHtml);

            }
            else if (PdfType.ToLower() == "api")
            {

                bytes = ConvertToPDFByExternalAPI(formattedHtml);
            }
            else
            {
                bytes = ConvertToPDF(formattedHtml);
            }
            return cropbox(bytes);
        }
    }
}
