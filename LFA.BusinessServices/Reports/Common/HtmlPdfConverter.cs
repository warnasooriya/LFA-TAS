using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using Newtonsoft.Json;
using NLog;
using NReco.PdfGenerator;
using Spire.Pdf;
using Spire.Pdf.HtmlConverter;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using TAS.Services.Common;

namespace TAS.Services.Reports.Common
{
    public class HtmlPdfConverter
    {
        private readonly string CrowdPdfUserName, CrowdPdfKey = string.Empty;
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private static readonly HttpClient client = new HttpClient();

        public HtmlPdfConverter()
        {
            CrowdPdfUserName = ConfigurationData.CrowdPdfUserName;
            CrowdPdfKey = ConfigurationData.CrowdPdfKey;
        }

        public byte[] ConvertToPDFNew(string FormattedHTML)
        {
            try
            {

                // create an API client instance
                pdfcrowd.Client client = new pdfcrowd.Client(CrowdPdfUserName, CrowdPdfKey);

                // convert a web page and save the PDF to a file
                // fileStream = new FileStream("google_com.pdf", FileMode.CreateNew);
                // client.convertURI("http://www.google.com", fileStream);
                // fileStream.Close();

                // convert an HTML string and store the PDF into a memory stream
                MemoryStream memStream = new MemoryStream();
                client.convertHtml(FormattedHTML, memStream);

                return memStream.ToArray();
                // convert an HTML file
                //  fileStream = new FileStream("file.pdf", FileMode.CreateNew);
                // client.convertFile("c:/local/file.html", fileStream);
                // fileStream.Close();

                // retrieve the number of credits in your account
                // int ncredits = client.numTokens();
            }
            catch (pdfcrowd.Error)
            {
                return null;
                // System.Console.WriteLine(why.ToString());
            }
        }

        public byte[] ConvertToPDF(string formattedHTML)
        {
            Spire.Pdf.PdfDocument pdf = new Spire.Pdf.PdfDocument();

            PdfHtmlLayoutFormat htmlLayoutFormat = new PdfHtmlLayoutFormat();

            htmlLayoutFormat.IsWaiting = false;

            PdfPageSettings setting = new PdfPageSettings();

            setting.Size = PdfPageSize.A4;
            Spire.Pdf.Graphics.PdfMargins margin = new Spire.Pdf.Graphics.PdfMargins();
            margin.All = 2;
            setting.Margins = margin;

            Thread thread = new Thread(() =>

            { pdf.LoadFromHTML(formattedHTML, false, setting, htmlLayoutFormat); });

            thread.SetApartmentState(ApartmentState.STA);

            thread.Start();

            thread.Join();

            MemoryStream ms = new MemoryStream();
            pdf.SaveToStream(ms);
            return ms.ToArray();

        }

        public byte[] ConvertToPDFNReco(string formattedHTML)
        {

            //return (new NReco.PdfGenerator.HtmlToPdfConverter()).GeneratePdf(formattedHTML);
            HtmlToPdfConverter pdfConverter = new HtmlToPdfConverter();
            pdfConverter.Size = NReco.PdfGenerator.PageSize.A4;
            pdfConverter.Orientation = PageOrientation.Portrait;
            pdfConverter.Margins = new PageMargins { Top = 1, Bottom = 1, Left = 1, Right = 1 };
            return pdfConverter.GeneratePdf(formattedHTML);

        }

        public byte[] ConvertToPdfV2(string formattedHTML)
        {

            byte[] bPDF = null;
            MemoryStream ms = new MemoryStream();
            Document doc = new Document(iTextSharp.text.PageSize.A4, 10, 10, 1, 1);
            PdfWriter oPdfWriter = PdfWriter.GetInstance(doc, ms);
            StringReader sr = new StringReader(formattedHTML);
            doc.Open();
            XMLWorkerHelper.GetInstance().ParseXHtml(oPdfWriter, doc, sr);

            doc.Close();
            bPDF = ms.ToArray();
            return bPDF;

        }

        public byte[] cropbox(byte[] src)
        {
            PdfReader reader = new PdfReader(src);
            int n = reader.NumberOfPages;
            PdfDictionary pageDict;

            for (int i = 1; i <= n; i++)
            {
                Rectangle pagesize = reader.GetPageSizeWithRotation(i);
                PdfRectangle rect = new PdfRectangle(0, 0, pagesize.Width, pagesize.Height - 15);
                pageDict = reader.GetPageN(i);
                pageDict.Put(PdfName.CROPBOX, rect);
            }
            using (MemoryStream ms = new MemoryStream())
            {
                using (PdfStamper stamper = new PdfStamper(reader, ms))
                {
                }
                return ms.ToArray();
            }
        }


        public byte[] ConvertToPDFByExternalAPI(string formattedHTML)
        {
            var randomFileName = Guid.NewGuid().ToString() + ".html";
            bool creationSuccess = false;
            byte[] responseByteArr = null;
            try
            {
               // randomFileName = "37f15d35-639c-44c7-846b-dcc81fbdde63.html";
                File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"\" + randomFileName,
                  formattedHTML);
                creationSuccess = true;
            }
            catch (Exception ex)
            { logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException); }

            if (creationSuccess)
            {
                using (var client = new WebClient())
                {
                    // Build the conversion options
                    NameValueCollection options = new NameValueCollection();
                    options.Add("apikey", ConfigurationData.PrintFriendlyApiKey);
                    options.Add("value", ConfigurationData.PrintFriendlyHtmlBaseUrl + randomFileName);

                    // Call the API convert to a PDF
                    MemoryStream ms = new MemoryStream(client.UploadValues(ConfigurationData.PrintFriendlyBaseUrl, options));

                    // Make the file a downloadable attachment - comment this out to show it directly inside
                    //HttpContext.Response.AddHeader("content-disposition", "attachment; filename=myfilename.pdf");

                    // Return the file as a PDF
                    //  return new FileStreamResult(ms, "application/pdf");
                    responseByteArr = ms.ToArray();

                }

                //delete html
                try
                {
                    File.Delete(AppDomain.CurrentDomain.BaseDirectory + @"\" + randomFileName);
                }
                catch (Exception ex)
                {
                    logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                }

            }


                return responseByteArr;
        }

    }

    public class PrintFriendlyResponseDto
    {
        public string status { get; set; }
        public string file_url { get; set; }
    }
}


