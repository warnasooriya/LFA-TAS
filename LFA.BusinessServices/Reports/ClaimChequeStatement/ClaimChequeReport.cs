using System;
using System.IO;
using System.Threading.Tasks;
using TAS.Services.Common;
using TAS.Services.Entities.Management;
using TAS.Services.Reports.Common;

namespace TAS.Services.Reports.ClaimChequeStatement
{
    public class ClaimChequeReport : HtmlPdfConverter
    {
        private static readonly Lazy<ClaimChequeReport> lazy = new Lazy<ClaimChequeReport>(() => new ClaimChequeReport());
        public static ClaimChequeReport Instance { get { return lazy.Value; } }
        private readonly String ReportPath, PdfType = String.Empty;

        public ClaimChequeReport()
        {
            ReportPath = ConfigurationData.ReportsPath;
            PdfType = ConfigurationData.PdfType;
        }

        public Byte[] Generate(String DBName, String ProductCode, Guid chequePaymentId)
        {
            String FormattedHTML, ReportHTML = String.Empty;
            String ExactHTMLPath = System.Web.HttpContext.Current.Server.MapPath(ReportPath + "\\ClaimChequeStatement\\" + DBName + "\\" + ProductCode + "\\ClaimChequeStatement_automobile.html");
            if (!File.Exists(ExactHTMLPath))
            {
                String DefaultReportPath = System.Web.HttpContext.Current.Server.MapPath(ReportPath + "\\ClaimChequeStatement\\Default\\" + ProductCode + "\\ClaimChequeStatement_automobile.html");
                ReportHTML = File.ReadAllText(DefaultReportPath);
            }
            else
            {
                ReportHTML = File.ReadAllText(ExactHTMLPath);
            }
            FormattedHTML = new ReportEntityManager().SetupClaimChequeStatement(ReportHTML, chequePaymentId);
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
            return bytes;
        }

      
    }
}
