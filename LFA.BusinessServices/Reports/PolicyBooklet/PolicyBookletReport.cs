using System;
using System.IO;
using TAS.Services.Common;

namespace TAS.Services.Reports.PolicyBooklet
{
    public class PolicyBookletReport
    {
        private static readonly Lazy<PolicyBookletReport> lazy = new Lazy<PolicyBookletReport>(() => new PolicyBookletReport());
        public static PolicyBookletReport Instance { get { return lazy.Value; } }
        private readonly String ReportPath = String.Empty;
        public PolicyBookletReport()
        {
            ReportPath = ConfigurationData.ReportsPath;
        }

        public byte[] Generate(String DBName, String ProductCode, string dealerCode, string makeCode, string commodityCategoryCode, string usage)
        {
            String ExactFilePath = System.Web.HttpContext.Current.Server.MapPath(ReportPath + "\\PolicyBooklet\\" + DBName + "\\" + ProductCode + "\\" +
                dealerCode.Replace("&","and") + "\\" + commodityCategoryCode + "\\" + makeCode + "\\" + usage + "\\LFA-Policy-Booklet-Template-V.1.0.pdf");

            if (!File.Exists(ExactFilePath))
            {
                String DefaultFilePath = System.Web.HttpContext.Current.Server.MapPath(ReportPath + "\\PolicyBooklet\\Default\\" + ProductCode + "\\LFA-Policy-Booklet-Template-V.1.0.pdf");
                ExactFilePath = DefaultFilePath;
            }
            return File.ReadAllBytes(ExactFilePath);
        }

    }
}
