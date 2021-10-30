using Microsoft.Reporting.WebForms;
using System;
using System.Data;
using System.Text.RegularExpressions;
using TAS.Services.Entities.Management;

namespace TAS.Web
{
    public partial class ReportExplorer : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                return;
            }

            string reportCacheKey = Request.QueryString["key"];
            string jwt = Request.QueryString["jwt"];
            string outputType = "PDF";

            try
            {
                string outFormat = Request.QueryString["output"];
                if (outFormat != null) {
                    outputType = outFormat;
                }
            }
            catch (Exception eex) {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "ErrorAlert", "alert('Output Format Problem.');", true);
                Console.WriteLine(eex.StackTrace);
            }


            if (IsGuid(reportCacheKey))
            {
                //read and delete cache
                ReportEntityManager reportEntityManager = new ReportEntityManager();
                CacheReportData reportData = reportEntityManager.GetReportCacheDataByReportId(reportCacheKey, jwt);
                if (reportData.isValid)
                {
                    ReportViewer1.Visible = true;

                    ReportViewer1.ProcessingMode = ProcessingMode.Local;
                    ReportViewer1.LocalReport.ReportPath = Server.MapPath(reportData.reportPath);




                    if (reportData.reportData.Rows.Count > 0)
                    {
                        ReportDataSource rds = new ReportDataSource("DataSet", reportData.reportData);
                        ReportViewer1.LocalReport.DataSources.Clear();
                        ReportViewer1.LocalReport.DataSources.Add(rds);
                    }

                    // export by default
                    Warning[] warnings;
                    string[] streamids;
                    string mimeType;
                    string encoding;
                    string extension;

                    try
                    {

                        byte[] bytes = ReportViewer1.LocalReport.Render(outputType, null, out mimeType, out encoding, out extension, out streamids, out warnings);


                        Response.Buffer = true;
                        Response.Clear();
                        Response.ContentType = mimeType;
                        Response.AddHeader("content-disposition", "attachment; filename=" + reportData.reportCode + "_" + new Random().Next(100000, 999999) + "." + extension);

                        Response.BinaryWrite(bytes);
                    }
                    catch (Exception ex)
                    {
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "ErrorAlert", "alert('No Data Found.');", true);


                        Console.WriteLine(ex.StackTrace);

                    }

                    Response.Flush();
                    ReportViewer1.LocalReport.Refresh();

                }
                else
                {
                    //show invalid
                }
            }
            else
            {
                //show invalid
            }
        }

        private DataSet GetData()
        {
            throw new NotImplementedException();
        }

        private static bool IsGuid(string candidate)
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

    }
}