using Humanizer;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using TAS.DataTransfer.Responses;
using TAS.Services;

namespace TAS.Web.Common
{
    public class ExcelUtlity
    {
        private DataTransfer.Common.SecurityContext securityContext;
        private DataTransfer.Common.AuditContext auditContext;

        public ExcelUtlity(DataTransfer.Common.SecurityContext securityContext, DataTransfer.Common.AuditContext auditContext)
        {
            this.securityContext = securityContext;
            this.auditContext = auditContext;
        }



        internal Stream GenerateBordxExcel(BordxExportResponseDto BordxResponse)
        {
            using (ExcelPackage p = new ExcelPackage())
            {

                List<string> ReferanceRows = new List<string>(){
                    "currencyconversionrate","transactiontypecode","transactiontypeid",
                    "contractid","grosspremiumbeforetax","policyid"
                };


                //Here setting some document properties
                p.Workbook.Properties.Author = "Trivow Business Solutions";
                p.Workbook.Properties.Title = "TAS Bordereaux Report";
                int worksheet = 0;
                if (BordxResponse.BordxData != null && BordxResponse.BordxData.Count > 0)
                {
                    foreach (DataTable dt in BordxResponse.BordxData)
                    {
                        worksheet++;
                        //Create a sheet
                        p.Workbook.Worksheets.Add(dt.Rows[0]["BaseCountry"].ToString());
                        ExcelWorksheet ws = p.Workbook.Worksheets[worksheet];
                        ws.Name = dt.Rows[0]["BaseCountry"].ToString(); //Setting Sheet's name
                        ws.Cells.Style.Font.Size = 10; //Default font size for whole sheet
                        ws.Cells.Style.Font.Name = "Tahoma"; //Default Font name for whole sheet


                        if(BordxResponse.TpaName == "continental")
                        {
                            //ws.Cells[1, 1, 1, 2]

                            ws.Cells[1, 4].Value = BordxResponse.BordxReportTemplateName + " " + "Continental" + " " + BordxResponse.BordxStartDate + " to " + BordxResponse.BordxEndDate;
                        }
                        else
                        {
                            //Merging cells and create a center heading for out table
                            Image image;
                            try
                            {
                                byte[] bytes = Convert.FromBase64String(BordxResponse.tpaLogo);
                                using (MemoryStream ms = new MemoryStream(bytes))
                                {
                                    image = Image.FromStream(ms);
                                    ws.Row(1).Height = image.Height * .75;

                                    var picture = ws.Drawings.AddPicture("logo", image);
                                    picture.SetPosition(0, 0);
                                    ws.Cells[1, 1, 1, 2].Merge = true;
                                }
                            }
                            catch (Exception)
                            {
                                image = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"STANDARD\\assets\\images\\bg_2.png"));
                                ws.Row(1).Height = image.Height * .75;
                                var picture = ws.Drawings.AddPicture("logo", image);
                                picture.SetPosition(0, 0);
                                ws.Cells[1, 1, 1, 2].Merge = true;
                            }

                            //ws.Cells[1, 1, 1, 2]

                            ws.Cells[1, 4].Value = BordxResponse.BordxReportTemplateName + " " + BordxResponse.TpaName + " " + BordxResponse.BordxStartDate + " to " + BordxResponse.BordxEndDate;
                        }



                        // ws.Cells[1, 1, 1, dt.Columns.Count].Merge = true;
                        ws.Cells[1, 1, 1, dt.Columns.Count].Style.Font.Bold = true;
                        ws.Cells[1, 1, 1, dt.Columns.Count].Style.Font.Size = 14;
                        ws.Cells[1, 1, 1, dt.Columns.Count].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        ws.Cells[1, 1, 1, dt.Columns.Count].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        ws.Row(2).Height = 28.5;
                        ws.Row(3).Height = 62.5;

                        //create merged top column heding
                        int StartingColumn = 1;
                        int HeaderRow = 2;
                        int SubHeaderRow = 3;
                        int SubHeaderColumn = 1;
                        List<string> DynamicTaxColumns = new List<string>();
                        List<int> LocalCurrencyColumns = new List<int>();
                        List<int> CostingUSColumns = new List<int>();
                        List<BordxSumFormulaDto> FormulasToBeCreated = new List<BordxSumFormulaDto>();

                        List<TAS.DataTransfer.Common.TaxColumnInfo> TaxAppearsIn = new List<TAS.DataTransfer.Common.TaxColumnInfo>();
                        Guid countryId = Guid.Parse(dt.Rows[0]["BaseCountryId"].ToString());
                        //s Guid BaseCurrencyPeriodId = Guid.Parse(dt.Rows[0]["BaseCurrencyPeriodId"].ToString());
                        //Guid BaseCurrencyId = Guid.Parse(dt.Rows[0]["BaseCurrencyId"].ToString());


                        TAS.DataTransfer.Common.BordxTaxInfo RelevantTaxes = new TAS.DataTransfer.Common.BordxTaxInfo();
                        RelevantTaxes = BordxResponse.CountryTaxInfo.Where(a => a.CountryId == countryId).FirstOrDefault();

                        int alignmentIndex = 1;
                        List<BordxAlignmentDto> bordxAlignment = new List<BordxAlignmentDto>();

                        #region header looping
                        foreach (BordxReportColumnHeaders header in BordxResponse.BordxReportColumnHeaders.OrderBy(a => a.Sequance).ToList())
                        {
                            //sub header
                            List<BordxReportColumns> subHeaders = BordxResponse.BordxReportColumns.Where(a => a.HeaderId == header.Id).OrderBy(a => a.Sequance).ToList();
                            DataColumnCollection columns = dt.Columns;

                            foreach (BordxReportColumns subHeader in subHeaders)
                            {
                                if (BordxResponse.TpaName == "continental")
                                {

                                    BordxAlignmentDto bordxAlignmentDto = new BordxAlignmentDto()
                                    {
                                        Index = alignmentIndex,
                                        Alignment = subHeader.Alignment,
                                        keyName = subHeader.KeyName,
                                        IsActive = subHeader.IsActive,
                                        Sequance= subHeader.Sequance,
                                        ColumnWidth = Convert.ToDouble(subHeader.ColumnWidth)
                                    };
                                    bordxAlignment.Add(bordxAlignmentDto);
                                    alignmentIndex++;
                                }


                                if (columns.Contains(subHeader.KeyName))
                                {

                                    //this is a valid column
                                    ws.Cells[SubHeaderRow, SubHeaderColumn].Value = subHeader.DisplayName;
                                    ws.Cells[SubHeaderRow, SubHeaderColumn].Style.Font.Bold = true;
                                    ws.Cells[SubHeaderRow, SubHeaderColumn].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    ws.Cells[SubHeaderRow, SubHeaderColumn].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    ws.Cells[SubHeaderRow, SubHeaderColumn].Style.WrapText = true;

                                    var border = ws.Cells[SubHeaderRow, SubHeaderColumn].Style.Border;
                                    border.Left.Style = border.Top.Style = border.Bottom.Style =
                                        border.Right.Style = ExcelBorderStyle.Thin;
                                    //ws.Cells[SubHeaderRow, SubHeaderColumn].AutoFitColumns();

                                    if(Convert.ToDouble(subHeader.ColumnWidth) == 0)
                                    {
                                        ws.Cells[SubHeaderRow, SubHeaderColumn].AutoFitColumns();
                                    }
                                    else
                                    {
                                        //ws.Column(SubHeaderColumn).Width = Convert.ToDouble(subHeader.ColumnWidth);
                                        ws.Column(SubHeaderColumn).Width = Convert.ToDouble(subHeader.ColumnWidth) + 0.80;
                                        // ws.Column(SubHeaderColumn).Width = Convert.ToDouble(subHeader.ColumnWidth);
                                    }

                                    //ws.Column(35).Width = 45;
                                    if (header.GenarateSum == true)
                                    {
                                        LocalCurrencyColumns.Add(SubHeaderColumn);
                                    }
                                    else if (header.HeaderName.ToLower().Contains("costing us $"))
                                    {
                                        CostingUSColumns.Add(SubHeaderColumn);
                                    }

                                    SubHeaderColumn++;
                                }
                                else
                                {
                                    if (subHeader.KeyName.ToLower() == "tax")
                                    {
                                        foreach (var taxInfo in RelevantTaxes.CountryTaxes.OrderBy(a => a.IndexVal))
                                        {
                                            ws.Cells[SubHeaderRow, SubHeaderColumn].Value = taxInfo.TaxName;
                                            ws.Cells[SubHeaderRow, SubHeaderColumn].Style.Font.Bold = true;
                                            var border = ws.Cells[SubHeaderRow, SubHeaderColumn].Style.Border;
                                            border.Left.Style = border.Top.Style = border.Bottom.Style =
                                                border.Right.Style = ExcelBorderStyle.Thin;
                                            ws.Cells[SubHeaderRow, SubHeaderColumn].AutoFitColumns();
                                            TaxAppearsIn.Add(
                                                new TAS.DataTransfer.Common.TaxColumnInfo()
                                                {
                                                    Column = SubHeaderColumn,
                                                    Row = SubHeaderRow,
                                                    taxInformation = taxInfo
                                                }
                                             );
                                            DynamicTaxColumns.Add(taxInfo.TaxName);
                                            if (header.HeaderName.ToLower().Contains("local currency"))
                                            {
                                                LocalCurrencyColumns.Add(SubHeaderColumn);
                                            }
                                            SubHeaderColumn++;
                                        }
                                    }

                                }
                            }
                            //main header
                                ws.Cells[HeaderRow, StartingColumn == 1 ? StartingColumn : (StartingColumn - 1)].Value = header.HeaderName;
                                ws.Cells[HeaderRow, StartingColumn == 1 ? StartingColumn : (StartingColumn - 1), HeaderRow, (SubHeaderColumn - 1)].Merge = true;
                                ws.Cells[HeaderRow, StartingColumn == 1 ? StartingColumn : (StartingColumn - 1), HeaderRow, (SubHeaderColumn - 1)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                ws.Cells[HeaderRow, StartingColumn == 1 ? StartingColumn : (StartingColumn - 1), HeaderRow, (SubHeaderColumn - 1)].Style.Font.Bold = true;
                                var borderSub = ws.Cells[HeaderRow, StartingColumn == 1 ? StartingColumn : (StartingColumn - 1), HeaderRow, (SubHeaderColumn - 1)].Style.Border;
                                borderSub.Left.Style = borderSub.Top.Style = borderSub.Bottom.Style = borderSub.Right.Style = ExcelBorderStyle.Thin;

                            if (header.HeaderName == "Customer Details")
                            {
                                ws.Cells[HeaderRow, StartingColumn == 1 ? StartingColumn : (StartingColumn - 1)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                ws.Cells[HeaderRow, StartingColumn == 1 ? StartingColumn : (StartingColumn - 1)].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 255, 255));
                            }
                            StartingColumn = SubHeaderColumn + 1;

                        }
                        #endregion header looping
                        //getting unwanted columns
                        List<string> ColumnsToRemove = new List<string>();
                        foreach (DataColumn column in dt.Columns)
                        {
                            if (!DynamicTaxColumns.Contains(column.ColumnName))
                            {
                                int rowAvailable = BordxResponse.BordxReportColumns.Where(a => a.KeyName.ToLower() == column.ColumnName.ToLower()).Count();
                                if (rowAvailable == 0)
                                {
                                    ColumnsToRemove.Add(column.ColumnName);
                                }
                            }
                        }

                        //removing unwanted columns
                        foreach (string columnName in ColumnsToRemove)
                        {
                            if (!ReferanceRows.Contains(columnName.ToLower()))
                            {
                                dt.Columns.Remove(columnName);

                            }
                        }

                        //add tax columns
                        int index = 0;

                        foreach (var tax in TaxAppearsIn)
                        {
                            dt.Columns.Add("tax_" + index.ToString()).SetOrdinal(tax.Column - 1);
                            index++;
                        }

                        //bordx sections
                        List<DataRow> endorsementNew = new List<DataRow>();
                        List<DataRow> endorsementOld = new List<DataRow>();
                        List<DataRow> cancellation = new List<DataRow>();
                        List<DataRow> transfer = new List<DataRow>();
                        List<DataRow> inactive = new List<DataRow>();



                        int colIndex = 1;
                        int rowIndex = 4;
                        bool taxDone = false;
                        var cell = ws.Cells[rowIndex, colIndex];

                        //cell.Value = "Active";
                        //ws.Cells[rowIndex, colIndex, rowIndex, colIndex + 2].Merge = true;
                        //ws.Cells[rowIndex, colIndex, rowIndex, colIndex + 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        //ws.Cells[rowIndex, colIndex, rowIndex, colIndex + 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
                        //ws.Cells[rowIndex, colIndex, rowIndex, colIndex + 2].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 102, 102));
                        cell.AutoFitColumns();


                        //saving active policies
                        foreach (DataRow dr in dt.Rows) // Adding Data into rows
                        {
                            colIndex = 1;
                            rowIndex++;


                            decimal conversionRate = Decimal.Parse(dr["currencyconversionrate"].ToString());  //GetConversionRate(BaseCurrencyId,BaseCurrencyPeriodId);
                            string transactionType = dr["TransactionTypeCode"].ToString();

                            if (transactionType.ToLower() == "cancellation")
                                cancellation.Add(dr);

                            if (transactionType.ToLower() == "endorsement")
                                endorsementNew.Add(dr);

                            if (transactionType.ToLower() == "transfer")
                                transfer.Add(dr);

                            if (transactionType.ToLower() == "endorsementold")
                                endorsementOld.Add(dr);

                            if (!String.IsNullOrEmpty(transactionType))
                            {
                                rowIndex--;
                                continue;
                            }

                            if (dr.Table.Columns.Contains("Status") && dr["Status"].ToString().ToLower() == "inactive")
                            {
                                inactive.Add(dr);
                                rowIndex--;
                                continue;
                            }


                            #region active
                            foreach (DataColumn dc in dt.Columns)
                            {


                                if (ReferanceRows.Contains(dc.ColumnName.ToLower()))
                                    continue;

                                bool shouldConvert = false;
                                bool haveSum = false;
                                if (LocalCurrencyColumns.Contains(colIndex))
                                {
                                    shouldConvert = haveSum = true;
                                }
                                if (CostingUSColumns.Contains(colIndex))
                                {
                                    haveSum = true;
                                }


                                cell = ws.Cells[rowIndex, colIndex];
                                object cellValue = GetCellValue(DynamicTaxColumns, shouldConvert, colIndex, rowIndex,
                                       TaxAppearsIn, dc, dr, conversionRate, out taxDone);



                                // custom column values set for continental
                                if (dc.ColumnName.ToLower() == "salesmancommision" && cellValue.ToString()=="0") {
                                    cellValue = "0.00";
                                }
                                if (dc.ColumnName.ToLower() == "mileagelimitationinkms" || dc.ColumnName.ToLower() == "manfcoverhours")
                                {
                                    if (cellValue.ToString() == "0") {
                                        cellValue = "00";
                                    }
                                }
                                // end custom value format

                                if (taxDone)
                                {
                                    cell.Style.Numberformat.Format = "#,##0.00";
                                    cell = ws.Cells[rowIndex, colIndex];
                                    cell.Value = cellValue;
                                    if (FormulasToBeCreated.Where(a => a.section == "active" && a.column == colIndex).Count() == 0)
                                    {
                                        FormulasToBeCreated.Add(new BordxSumFormulaDto()
                                        {
                                            section = "active",
                                            start = rowIndex,
                                            column = colIndex
                                        });
                                    }
                                    var cellborder = cell.Style.Border;
                                    //cellborder.Left.Style =
                                    cellborder.Bottom.Style =
                                    cellborder.Top.Style =
                                    cellborder.Right.Style = ExcelBorderStyle.Thin;
                                    colIndex++;


                                    continue;
                                }

                                if (haveSum)
                                {
                                    cell.Style.Numberformat.Format = "#,##0.00";
                                    if (FormulasToBeCreated.Where(a => a.section == "active" && a.column == colIndex).Count() == 0)
                                    {
                                        FormulasToBeCreated.Add(new BordxSumFormulaDto()
                                        {
                                            section = "active",
                                            start = rowIndex,
                                            column = colIndex
                                        });
                                    }

                                }
                                else if (dc.ColumnName.EndsWith("US") && dc.ColumnName.ToLower() != "conversionrateus")
                                {
                                    cell.Style.Numberformat.Format = "#,##0.00";
                                }
                                else if (dc.ColumnName.ToLower() == "conversionrateus")
                                {
                                    cell.Style.Numberformat.Format = "#,##0.0000";
                                }
                                else
                                {
                                    //cell.Style.Numberformat.Format = "#,##0.00";
                                    cell.Style.Numberformat.Format = "@";
                                }



                                if (cellValue.ToString().StartsWith("=DATE("))
                                {
                                    cell.Style.Numberformat.Format = "dd-mmm-yyyy";
                                    cell.Formula = cellValue.ToString();
                                }
                                else
                                {

                                    cell.Value = cellValue;
                                }


                                if (BordxResponse.TpaName == "continental")
                                {
                                    var col = bordxAlignment.Where(a => a.keyName == dc.ColumnName).FirstOrDefault();
                                    if (col == null)
                                    {

                                    }
                                    else if (col.Alignment == "C")
                                    {
                                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    }
                                    else if (col.Alignment == "L")
                                    {
                                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                    }
                                    else
                                    {
                                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                    }
                                }

                                //Setting borders of cell
                                var border = cell.Style.Border;
                                //border.Left.Style =
                                border.Bottom.Style =
                                border.Top.Style =
                                border.Right.Style = ExcelBorderStyle.Thin;

                                colIndex++;


                            }
                            foreach (var item in FormulasToBeCreated.Where(a => a.section == "active"))
                            {
                                item.end = rowIndex;
                            }
                            #endregion active
                        }
                        //fill other sections
                        //inactive
                        if (inactive.Count > 0)
                        {
                            rowIndex += 3;
                            colIndex = 1;

                            cell = ws.Cells[rowIndex, colIndex];
                            cell.Value = "Inctive";
                            ws.Cells[rowIndex, colIndex, rowIndex, colIndex + 2].Merge = true;
                            ws.Cells[rowIndex, colIndex, rowIndex, colIndex + 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            ws.Cells[rowIndex, colIndex, rowIndex, colIndex + 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
                            ws.Cells[rowIndex, colIndex, rowIndex, colIndex + 2].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 102, 102));
                            cell.AutoFitColumns();
                            rowIndex++;
                            foreach (DataRow dr in inactive)
                            {
                                colIndex = 1;
                                decimal conversionRate = Decimal.Parse(dr["currencyconversionrate"].ToString());
                                foreach (DataColumn dc in dt.Columns)
                                {
                                    if (ReferanceRows.Contains(dc.ColumnName.ToLower()))
                                        continue;

                                    bool shouldConvert = false;
                                    bool haveSum = false;
                                    if (LocalCurrencyColumns.Contains(colIndex))
                                    {
                                        shouldConvert = haveSum = true;
                                    }
                                    if (CostingUSColumns.Contains(colIndex))
                                    {
                                        haveSum = true;
                                    }
                                    cell = ws.Cells[rowIndex, colIndex];
                                    object cellValue = GetCellValue(DynamicTaxColumns, shouldConvert, colIndex, rowIndex,
                                      TaxAppearsIn, dc, dr, conversionRate, out taxDone);

                                    if (taxDone)
                                    {
                                        cell.Style.Numberformat.Format = "#,##0.00";
                                        cell = ws.Cells[rowIndex, colIndex];
                                        cell.Value = cellValue;
                                        if (FormulasToBeCreated.Where(a => a.section == "inactive" && a.column == colIndex).Count() == 0)
                                        {
                                            FormulasToBeCreated.Add(new BordxSumFormulaDto()
                                            {
                                                section = "inactive",
                                                start = rowIndex,
                                                column = colIndex
                                            });
                                        }
                                        var cellborder = cell.Style.Border;
                                        cellborder.Left.Style =
                                        cellborder.Bottom.Style =
                                        cellborder.Top.Style =
                                        cellborder.Right.Style = ExcelBorderStyle.Thin;
                                        colIndex++;
                                        //for (int i = 1; i < DynamicTaxColumns.Count; i++)
                                        //{
                                        //    cell = ws.Cells[rowIndex, colIndex];
                                        //    cellValue = GetCellValue(DynamicTaxColumns, shouldConvert, colIndex, rowIndex,
                                        //    TaxAppearsIn, dc, dr, conversionRate, out taxDone);
                                        //    cell.Value = cellValue; haveSum = false;
                                        //    if (FormulasToBeCreated.Where(a => a.section == "inactive" && a.column == colIndex).Count() == 0)
                                        //    {
                                        //        FormulasToBeCreated.Add(new BordxSumFormulaDto()
                                        //        {
                                        //            section = "inactive",
                                        //            start = rowIndex,
                                        //            column = colIndex
                                        //        });
                                        //    }

                                        //    colIndex++;
                                        //}
                                        continue;
                                    }
                                    if (haveSum)
                                    {
                                        cell.Style.Numberformat.Format = "#,##0.00";
                                        if (FormulasToBeCreated.Where(a => a.section == "inactive" && a.column == colIndex).Count() == 0)
                                        {
                                            FormulasToBeCreated.Add(new BordxSumFormulaDto()
                                            {
                                                section = "inactive",
                                                start = rowIndex,
                                                column = colIndex
                                            });
                                        }

                                    }
                                    else if (dc.ColumnName.EndsWith("US") && dc.ColumnName.ToLower() != "conversionrateus")
                                    {
                                        cell.Style.Numberformat.Format = "#,##0.00";
                                    }
                                    else if (dc.ColumnName.ToLower() == "conversionrateus")
                                    {
                                        cell.Style.Numberformat.Format = "#,##0.0000";
                                    }

                                    if (cellValue.ToString().StartsWith("=DATE("))
                                    {
                                        cell.Style.Numberformat.Format = "dd-mmm-yyyy";
                                        cell.Formula = cellValue.ToString();
                                    }
                                    else
                                    {
                                        cell.Value = cellValue;
                                    }

                                    //Setting borders of cell
                                    var border = cell.Style.Border;
                                    border.Left.Style =
                                    border.Bottom.Style =
                                    border.Top.Style =
                                    border.Right.Style = ExcelBorderStyle.Thin;
                                    if (!taxDone)
                                    {
                                        colIndex++;
                                    }
                                    else
                                    {
                                        if (FormulasToBeCreated.Where(a => a.section == "inactive" && a.column == colIndex).Count() == 0)
                                        {
                                            for (int i = 1; i < DynamicTaxColumns.Count(); i++)
                                            {
                                                FormulasToBeCreated.Add(new BordxSumFormulaDto()
                                                {
                                                    section = "inactive",
                                                    start = rowIndex + i,
                                                    column = colIndex
                                                });
                                            }

                                        }
                                        colIndex += DynamicTaxColumns.Count();
                                    }
                                }
                                foreach (var item in FormulasToBeCreated.Where(a => a.section == "inactive"))
                                {
                                    item.end = rowIndex;
                                }
                                rowIndex++;
                            }
                        }
                        //endorsement new
                        if (endorsementNew.Count > 0)
                        {
                            rowIndex += 3;
                            colIndex = 1;

                            cell = ws.Cells[rowIndex, colIndex];
                            cell.Value = "Endorsement Active";
                            ws.Cells[rowIndex, colIndex, rowIndex, colIndex + 2].Merge = true;
                            ws.Cells[rowIndex, colIndex, rowIndex, colIndex + 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            ws.Cells[rowIndex, colIndex, rowIndex, colIndex + 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
                            ws.Cells[rowIndex, colIndex, rowIndex, colIndex + 2].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 102, 102));
                            cell.AutoFitColumns();
                            rowIndex++;
                            foreach (DataRow dr in endorsementNew)
                            {
                                colIndex = 1;
                                decimal conversionRate = Decimal.Parse(dr["currencyconversionrate"].ToString());
                                foreach (DataColumn dc in dt.Columns)
                                {
                                    if (ReferanceRows.Contains(dc.ColumnName.ToLower()))
                                        continue;

                                    bool shouldConvert = false;
                                    bool haveSum = false;
                                    if (LocalCurrencyColumns.Contains(colIndex))
                                    {
                                        shouldConvert = haveSum = true;
                                    }
                                    if (CostingUSColumns.Contains(colIndex))
                                    {
                                        haveSum = true;
                                    }
                                    cell = ws.Cells[rowIndex, colIndex];
                                    object cellValue = GetCellValue(DynamicTaxColumns, shouldConvert, colIndex, rowIndex,
                                      TaxAppearsIn, dc, dr, conversionRate, out taxDone);

                                    if (taxDone)
                                    {
                                        cell.Style.Numberformat.Format = "#,##0.00";
                                        cell = ws.Cells[rowIndex, colIndex];
                                        cell.Value = cellValue;
                                        if (FormulasToBeCreated.Where(a => a.section == "endorsementactive" && a.column == colIndex).Count() == 0)
                                        {
                                            FormulasToBeCreated.Add(new BordxSumFormulaDto()
                                            {
                                                section = "endorsementactive",
                                                start = rowIndex,
                                                column = colIndex
                                            });
                                        }
                                        var cellborder = cell.Style.Border;
                                        cellborder.Left.Style =
                                        cellborder.Bottom.Style =
                                        cellborder.Top.Style =
                                        cellborder.Right.Style = ExcelBorderStyle.Thin;
                                        colIndex++;
                                        //for (int i = 1; i < DynamicTaxColumns.Count; i++)
                                        //{
                                        //    cell = ws.Cells[rowIndex, colIndex];
                                        //    cellValue = GetCellValue(DynamicTaxColumns, shouldConvert, colIndex, rowIndex,
                                        //   TaxAppearsIn, dc, dr, conversionRate, out taxDone);
                                        //    cell.Value = cellValue; haveSum = false;
                                        //    if (FormulasToBeCreated.Where(a => a.section == "endorsementactive" && a.column == colIndex).Count() == 0)
                                        //    {
                                        //        FormulasToBeCreated.Add(new BordxSumFormulaDto()
                                        //        {
                                        //            section = "endorsementactive",
                                        //            start = rowIndex,
                                        //            column = colIndex
                                        //        });
                                        //    }

                                        //    colIndex++;
                                        //}
                                        continue;
                                    }
                                    if (haveSum)
                                    {
                                        cell.Style.Numberformat.Format = "#,##0.00";
                                        if (FormulasToBeCreated.Where(a => a.section == "endorsementactive" && a.column == colIndex).Count() == 0)
                                        {
                                            FormulasToBeCreated.Add(new BordxSumFormulaDto()
                                            {
                                                section = "endorsementactive",
                                                start = rowIndex,
                                                column = colIndex
                                            });
                                        }

                                    }
                                    else if (dc.ColumnName.EndsWith("US") && dc.ColumnName.ToLower() != "conversionrateus")
                                    {
                                        cell.Style.Numberformat.Format = "#,##0.00";
                                    }
                                    else if (dc.ColumnName.ToLower() == "conversionrateus")
                                    {
                                        cell.Style.Numberformat.Format = "#,##0.0000";
                                    }

                                    else
                                    {
                                        cell.Style.Numberformat.Format = "#####";
                                    }


                                    if (cellValue.ToString().StartsWith("=DATE("))
                                    {
                                        cell.Style.Numberformat.Format = "dd-mmm-yyyy";
                                        cell.Formula = cellValue.ToString();
                                    }
                                    else
                                    {
                                        cell.Value = cellValue;
                                    }

                                    //Setting borders of cell
                                    var border = cell.Style.Border;
                                    border.Left.Style =
                                    border.Bottom.Style =
                                    border.Top.Style =
                                    border.Right.Style = ExcelBorderStyle.Thin;
                                    if (!taxDone)
                                    {
                                        colIndex++;
                                    }
                                    else
                                    {
                                        if (FormulasToBeCreated.Where(a => a.section == "endorsementactive" && a.column == colIndex).Count() == 0)
                                        {
                                            for (int i = 1; i < DynamicTaxColumns.Count(); i++)
                                            {
                                                FormulasToBeCreated.Add(new BordxSumFormulaDto()
                                                {
                                                    section = "endorsementactive",
                                                    start = rowIndex + i,
                                                    column = colIndex
                                                });
                                            }

                                        }
                                        colIndex += DynamicTaxColumns.Count();
                                    }
                                }
                                foreach (var item in FormulasToBeCreated.Where(a => a.section == "endorsementactive"))
                                {
                                    item.end = rowIndex;
                                }
                                rowIndex++;
                            }
                        }

                        //endorsement old
                        if (endorsementOld.Count > 0)
                        {
                            rowIndex += 3;
                            colIndex = 1;

                            cell = ws.Cells[rowIndex, colIndex];
                            cell.Value = "Endorsement Cancelled";
                            ws.Cells[rowIndex, colIndex, rowIndex, colIndex + 2].Merge = true;
                            ws.Cells[rowIndex, colIndex, rowIndex, colIndex + 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            ws.Cells[rowIndex, colIndex, rowIndex, colIndex + 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
                            ws.Cells[rowIndex, colIndex, rowIndex, colIndex + 2].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 102, 102));
                            cell.AutoFitColumns();
                            rowIndex++;
                            foreach (DataRow dr in endorsementOld)
                            {
                                colIndex = 1;
                                decimal conversionRate = Decimal.Parse(dr["currencyconversionrate"].ToString());
                                foreach (DataColumn dc in dt.Columns)
                                {
                                    if (ReferanceRows.Contains(dc.ColumnName.ToLower()))
                                        continue;

                                    bool shouldConvert = false;
                                    bool haveSum = false;
                                    if (LocalCurrencyColumns.Contains(colIndex))
                                    {
                                        shouldConvert = haveSum = true;
                                    }
                                    if (CostingUSColumns.Contains(colIndex))
                                    {
                                        haveSum = true;
                                    }
                                    cell = ws.Cells[rowIndex, colIndex];
                                    object cellValue = GetCellValue(DynamicTaxColumns, shouldConvert, colIndex, rowIndex,
                                     TaxAppearsIn, dc, dr, conversionRate, out taxDone);

                                    if (taxDone)
                                    {
                                        cell.Style.Numberformat.Format = "#,##0.00";
                                        cell = ws.Cells[rowIndex, colIndex];
                                        cell.Value = decimal.Parse(cellValue.ToString()) * -1;
                                        if (FormulasToBeCreated.Where(a => a.section == "endorsementcancelled" && a.column == colIndex).Count() == 0)
                                        {
                                            FormulasToBeCreated.Add(new BordxSumFormulaDto()
                                            {
                                                section = "endorsementcancelled",
                                                start = rowIndex,
                                                column = colIndex
                                            });
                                        }
                                        var cellborder = cell.Style.Border;
                                        cellborder.Left.Style =
                                        cellborder.Bottom.Style =
                                        cellborder.Top.Style =
                                        cellborder.Right.Style = ExcelBorderStyle.Thin;
                                        colIndex++;
                                        //for (int i = 1; i < DynamicTaxColumns.Count; i++)
                                        //{
                                        //    cell = ws.Cells[rowIndex, colIndex];
                                        //    cellValue = GetCellValue(DynamicTaxColumns, shouldConvert, colIndex, rowIndex,
                                        //   TaxAppearsIn, dc, dr, conversionRate, out taxDone);
                                        //    cell.Value = decimal.Parse(cellValue.ToString()) * -1; haveSum = false;
                                        //    if (FormulasToBeCreated.Where(a => a.section == "endorsementcancelled" && a.column == colIndex).Count() == 0)
                                        //    {
                                        //        FormulasToBeCreated.Add(new BordxSumFormulaDto()
                                        //        {
                                        //            section = "endorsementcancelled",
                                        //            start = rowIndex,
                                        //            column = colIndex
                                        //        });
                                        //    }

                                        //    colIndex++;
                                        //}
                                        continue;
                                    }
                                    if (haveSum)
                                    {
                                        cell.Style.Numberformat.Format = "#,##0.00";
                                        if (FormulasToBeCreated.Where(a => a.section == "endorsementcancelled" && a.column == colIndex).Count() == 0)
                                        {
                                            FormulasToBeCreated.Add(new BordxSumFormulaDto()
                                            {
                                                section = "endorsementcancelled",
                                                start = rowIndex,
                                                column = colIndex
                                            });
                                        }

                                    }
                                    else if (dc.ColumnName.EndsWith("US") && dc.ColumnName.ToLower() != "conversionrateus")
                                    {
                                        cell.Style.Numberformat.Format = "#,##0.00";
                                    }
                                    else if (dc.ColumnName.ToLower() == "conversionrateus")
                                    {
                                        cell.Style.Numberformat.Format = "#,##0.0000";
                                    }
                                    else
                                    {
                                        cell.Style.Numberformat.Format = "#####";
                                    }


                                    if (cellValue.ToString().StartsWith("=DATE("))
                                    {
                                        cell.Style.Numberformat.Format = "dd-mmm-yyyy";
                                        cell.Formula = cellValue.ToString();
                                    }
                                    else
                                    {
                                        if (haveSum)
                                        {
                                            decimal parsedValue;
                                            var result = decimal.TryParse(cellValue.ToString(), out parsedValue);
                                            if (result)
                                            {
                                                cell.Value = parsedValue * -1;
                                            }
                                            else
                                            {
                                                cell.Value = 0.00;
                                            }

                                        }
                                        else
                                        {
                                            cell.Value = cellValue;
                                        }

                                    }

                                    //Setting borders of cell
                                    var border = cell.Style.Border;
                                    border.Left.Style =
                                    border.Bottom.Style =
                                    border.Top.Style =
                                    border.Right.Style = ExcelBorderStyle.Thin;
                                    if (!taxDone)
                                    {
                                        colIndex++;
                                    }
                                    else
                                    {
                                        if (FormulasToBeCreated.Where(a => a.section == "endorsementcancelled" && a.column == colIndex).Count() == 0)
                                        {
                                            for (int i = 0; i < DynamicTaxColumns.Count(); i++)
                                            {
                                                FormulasToBeCreated.Add(new BordxSumFormulaDto()
                                                {
                                                    section = "endorsementcancelled",
                                                    start = rowIndex + i,
                                                    column = colIndex
                                                });
                                            }

                                        }
                                        colIndex += DynamicTaxColumns.Count();
                                    }

                                }
                                foreach (var item in FormulasToBeCreated.Where(a => a.section == "endorsementcancelled"))
                                {
                                    item.end = rowIndex;
                                }

                                rowIndex++;
                            }
                        }


                        //cancellation
                        if (cancellation.Count > 0)
                        {
                            rowIndex += 3;
                            colIndex = 1;

                            cell = ws.Cells[rowIndex, colIndex];
                            cell.Value = "Cancelled";
                            ws.Cells[rowIndex, colIndex, rowIndex, colIndex + 2].Merge = true;
                            ws.Cells[rowIndex, colIndex, rowIndex, colIndex + 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            ws.Cells[rowIndex, colIndex, rowIndex, colIndex + 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
                            ws.Cells[rowIndex, colIndex, rowIndex, colIndex + 2].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 102, 102));
                            cell.AutoFitColumns();
                            rowIndex++;
                            foreach (DataRow dr in cancellation)
                            {
                                colIndex = 1;
                                decimal conversionRate = Decimal.Parse(dr["currencyconversionrate"].ToString());
                                foreach (DataColumn dc in dt.Columns)
                                {
                                    if (ReferanceRows.Contains(dc.ColumnName.ToLower()))
                                        continue;

                                    bool shouldConvert = false;
                                    bool haveSum = false;
                                    if (LocalCurrencyColumns.Contains(colIndex))
                                    {
                                        shouldConvert = haveSum = true;
                                    }
                                    if (CostingUSColumns.Contains(colIndex))
                                    {
                                        haveSum = true;
                                    }
                                    cell = ws.Cells[rowIndex, colIndex];
                                    object cellValue = GetCellValue(DynamicTaxColumns, shouldConvert, colIndex, rowIndex,
                                     TaxAppearsIn, dc, dr, conversionRate, out taxDone);

                                    if (taxDone)
                                    {
                                        cell.Style.Numberformat.Format = "#,##0.00";
                                        cell = ws.Cells[rowIndex, colIndex];
                                        cell.Value = cellValue;
                                        if (FormulasToBeCreated.Where(a => a.section == "cancelled" && a.column == colIndex).Count() == 0)
                                        {
                                            FormulasToBeCreated.Add(new BordxSumFormulaDto()
                                            {
                                                section = "cancelled",
                                                start = rowIndex,
                                                column = colIndex
                                            });
                                        }
                                        var cellborder = cell.Style.Border;
                                        cellborder.Left.Style =
                                        cellborder.Bottom.Style =
                                        cellborder.Top.Style =
                                        cellborder.Right.Style = ExcelBorderStyle.Thin;
                                        colIndex++;
                                        //for (int i = 0; i < DynamicTaxColumns.Count; i++)
                                        //{
                                        //    cell = ws.Cells[rowIndex, colIndex];
                                        //    cellValue = GetCellValue(DynamicTaxColumns, shouldConvert, colIndex, rowIndex,
                                        //   TaxAppearsIn, dc, dr, conversionRate, out taxDone);
                                        //    cell.Value = cellValue; haveSum = false;
                                        //    if (FormulasToBeCreated.Where(a => a.section == "cancelled" && a.column == colIndex).Count() == 0)
                                        //    {
                                        //        FormulasToBeCreated.Add(new BordxSumFormulaDto()
                                        //        {
                                        //            section = "cancelled",
                                        //            start = rowIndex,
                                        //            column = colIndex
                                        //        });
                                        //    }

                                        //    colIndex++;
                                        //}
                                        continue;
                                    }
                                    if (haveSum)
                                    {
                                        cell.Style.Numberformat.Format = "#,##0.00";
                                        if (FormulasToBeCreated.Where(a => a.section == "cancelled" && a.column == colIndex).Count() == 0)
                                        {
                                            FormulasToBeCreated.Add(new BordxSumFormulaDto()
                                            {
                                                section = "cancelled",
                                                start = rowIndex,
                                                column = colIndex
                                            });
                                        }

                                    }
                                    else if (dc.ColumnName.EndsWith("US") && dc.ColumnName.ToLower() != "conversionrateus")
                                    {
                                        cell.Style.Numberformat.Format = "#,##0.00";
                                    }
                                    else if (dc.ColumnName.ToLower() == "conversionrateus")
                                    {
                                        cell.Style.Numberformat.Format = "#,##0.0000";
                                    }
                                    else
                                    {
                                        cell.Style.Numberformat.Format = "#####";
                                    }

                                    if (cellValue.ToString().StartsWith("=DATE("))
                                    {
                                        cell.Style.Numberformat.Format = "dd-mmm-yyyy";
                                        cell.Formula = cellValue.ToString();
                                    }
                                    else
                                    {
                                        cell.Value = cellValue;
                                    }

                                    //Setting borders of cell
                                    var border = cell.Style.Border;
                                    border.Left.Style =
                                    border.Bottom.Style =
                                    border.Top.Style =
                                    border.Right.Style = ExcelBorderStyle.Thin;
                                    if (!taxDone)
                                    {
                                        colIndex++;
                                    }
                                    else
                                    {
                                        if (FormulasToBeCreated.Where(a => a.section == "cancelled" && a.column == colIndex).Count() == 0)
                                        {
                                            for (int i = 0; i < DynamicTaxColumns.Count(); i++)
                                            {
                                                FormulasToBeCreated.Add(new BordxSumFormulaDto()
                                                {
                                                    section = "cancelled",
                                                    start = rowIndex + i,
                                                    column = colIndex
                                                });
                                            }

                                        }
                                        colIndex += DynamicTaxColumns.Count();
                                    }

                                }

                                foreach (var item in FormulasToBeCreated.Where(a => a.section == "cancelled"))
                                {
                                    item.end = rowIndex;
                                }
                                rowIndex++;
                            }
                        }


                        //transfer
                        if (transfer.Count > 0)
                        {
                            rowIndex += 3;
                            colIndex = 1;

                            cell = ws.Cells[rowIndex, colIndex];
                            cell.Value = "Transfers";
                            ws.Cells[rowIndex, colIndex, rowIndex, colIndex + 2].Merge = true;
                            ws.Cells[rowIndex, colIndex, rowIndex, colIndex + 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            ws.Cells[rowIndex, colIndex, rowIndex, colIndex + 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
                            ws.Cells[rowIndex, colIndex, rowIndex, colIndex + 2].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 102, 102));
                            cell.AutoFitColumns();
                            rowIndex++;
                            foreach (DataRow dr in transfer)
                            {
                                colIndex = 1;
                                decimal conversionRate = Decimal.Parse(dr["currencyconversionrate"].ToString());
                                foreach (DataColumn dc in dt.Columns)
                                {
                                    if (ReferanceRows.Contains(dc.ColumnName.ToLower()))
                                        continue;

                                    bool shouldConvert = false;
                                    bool haveSum = false;
                                    if (LocalCurrencyColumns.Contains(colIndex))
                                    {
                                        shouldConvert = haveSum = true;
                                    }
                                    if (CostingUSColumns.Contains(colIndex))
                                    {
                                        haveSum = true;
                                    }
                                    cell = ws.Cells[rowIndex, colIndex];
                                    object cellValue = GetCellValue(DynamicTaxColumns, shouldConvert, colIndex, rowIndex,
                                     TaxAppearsIn, dc, dr, conversionRate, out taxDone);

                                    if (taxDone)
                                    {
                                        cell.Style.Numberformat.Format = "#,##0.00";
                                        cell = ws.Cells[rowIndex, colIndex];
                                        cell.Value = cellValue;
                                        if (FormulasToBeCreated.Where(a => a.section == "transfers" && a.column == colIndex).Count() == 0)
                                        {
                                            FormulasToBeCreated.Add(new BordxSumFormulaDto()
                                            {
                                                section = "transfers",
                                                start = rowIndex,
                                                column = colIndex
                                            });
                                        }
                                        var cellborder = cell.Style.Border;
                                        cellborder.Left.Style =
                                        cellborder.Bottom.Style =
                                        cellborder.Top.Style =
                                        cellborder.Right.Style = ExcelBorderStyle.Thin;
                                        colIndex++;
                                        //for (int i = 0; i < DynamicTaxColumns.Count; i++)
                                        //{
                                        //    cell = ws.Cells[rowIndex, colIndex];
                                        //    cellValue = GetCellValue(DynamicTaxColumns, shouldConvert, colIndex, rowIndex,
                                        //   TaxAppearsIn, dc, dr, conversionRate, out taxDone);
                                        //    cell.Value = cellValue; haveSum = false;
                                        //    if (FormulasToBeCreated.Where(a => a.section == "transfers" && a.column == colIndex).Count() == 0)
                                        //    {
                                        //        FormulasToBeCreated.Add(new BordxSumFormulaDto()
                                        //        {
                                        //            section = "transfers",
                                        //            start = rowIndex,
                                        //            column = colIndex
                                        //        });
                                        //    }

                                        //    colIndex++;
                                        //}
                                        continue;
                                    }
                                    if (haveSum)
                                    {
                                        cell.Style.Numberformat.Format = "#,##0.00";
                                        if (FormulasToBeCreated.Where(a => a.section == "transfers" && a.column == colIndex).Count() == 0)
                                        {
                                            FormulasToBeCreated.Add(new BordxSumFormulaDto()
                                            {
                                                section = "transfers",
                                                start = rowIndex,
                                                column = colIndex
                                            });
                                        }

                                    }
                                    else if (dc.ColumnName.EndsWith("US") && dc.ColumnName.ToLower() != "conversionrateus")
                                    {
                                        cell.Style.Numberformat.Format = "#,##0.00";
                                    }
                                    else if (dc.ColumnName.ToLower() == "conversionrateus")
                                    {
                                        cell.Style.Numberformat.Format = "#,##0.0000";
                                    }
                                    else
                                    {
                                        cell.Style.Numberformat.Format = "#####";
                                    }

                                    if (cellValue.ToString().StartsWith("=DATE("))
                                    {
                                        cell.Style.Numberformat.Format = "dd-mmm-yyyy";
                                        cell.Formula = cellValue.ToString();
                                    }
                                    else
                                    {
                                        cell.Value = cellValue;
                                    }

                                    //Setting borders of cell
                                    var border = cell.Style.Border;
                                    border.Left.Style =
                                    border.Bottom.Style =
                                    border.Top.Style =
                                    border.Right.Style = ExcelBorderStyle.Thin;
                                    if (!taxDone)
                                    {
                                        colIndex++;
                                    }
                                    else
                                    {
                                        if (FormulasToBeCreated.Where(a => a.section == "transfers" && a.column == colIndex).Count() == 0)
                                        {
                                            for (int i = 0; i < DynamicTaxColumns.Count(); i++)
                                            {
                                                FormulasToBeCreated.Add(new BordxSumFormulaDto()
                                                {
                                                    section = "transfers",
                                                    start = rowIndex + i,
                                                    column = colIndex
                                                });
                                            }

                                        }
                                        colIndex += DynamicTaxColumns.Count();
                                    }

                                }
                                foreach (var item in FormulasToBeCreated.Where(a => a.section == "transfers"))
                                {
                                    item.end = rowIndex;
                                }
                                rowIndex++;
                            }
                        }
                        #region show conversion rate
                        var activeRate = FormulasToBeCreated.Where(a => a.section == "active").OrderBy(a => a.column).FirstOrDefault();
                        if (activeRate != null)
                        {

                            cell = ws.Cells[activeRate.end + 1, activeRate.column - 1];
                            var beforeborder = cell.Style.Border;
                            beforeborder.Left.Style =
                            beforeborder.Bottom.Style =
                            beforeborder.Top.Style =
                            beforeborder.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.Font.Bold = true;
                            cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            cell.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 255, 255));
                            cell.Value = BordxResponse.currencyCode;


                            cell = ws.Cells[activeRate.end + 2, activeRate.column - 1];
                            var border = cell.Style.Border;
                            border.Left.Style =
                            border.Bottom.Style =
                            border.Top.Style =
                            border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.Font.Bold = true;
                            cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            cell.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 255, 255));
                            cell.Value = "USD Equalant @ " + BordxResponse.currencyCode + " " + BordxResponse.currentUSDConversionRate.ToString("#,##0.00");

                        }

                        var inActiveRate = FormulasToBeCreated.Where(a => a.section == "inactive").OrderBy(a => a.column).FirstOrDefault();
                        if (inActiveRate != null)
                        {

                            cell = ws.Cells[inActiveRate.end + 1, inActiveRate.column - 1];
                            var beforeborder = cell.Style.Border;
                            beforeborder.Left.Style =
                            beforeborder.Bottom.Style =
                            beforeborder.Top.Style =
                            beforeborder.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.Font.Bold = true;
                            cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            cell.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 255, 255));
                            cell.Value = BordxResponse.currencyCode;

                            cell = ws.Cells[inActiveRate.end + 2, inActiveRate.column - 1];
                            var border = cell.Style.Border;
                            border.Left.Style =
                            border.Bottom.Style =
                            border.Top.Style =
                            border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.Font.Bold = true;
                            cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            cell.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 255, 255));
                            cell.Value = "USD Equalant @ " + BordxResponse.currencyCode + " " + BordxResponse.currentUSDConversionRate;

                        }

                        var endorsementactiveRate = FormulasToBeCreated.Where(a => a.section == "endorsementactive").OrderBy(a => a.column).FirstOrDefault();
                        if (endorsementactiveRate != null)
                        {
                            cell = ws.Cells[endorsementactiveRate.end + 1, endorsementactiveRate.column - 1];
                            var beforeborder = cell.Style.Border;
                            beforeborder.Left.Style =
                            beforeborder.Bottom.Style =
                            beforeborder.Top.Style =
                            beforeborder.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.Font.Bold = true;
                            cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            cell.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 255, 255));
                            cell.Value = BordxResponse.currencyCode;


                            cell = ws.Cells[endorsementactiveRate.end + 2, endorsementactiveRate.column - 1];
                            var border = cell.Style.Border;
                            border.Left.Style =
                            border.Bottom.Style =
                            border.Top.Style =
                            border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.Font.Bold = true;
                            cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            cell.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 255, 255));
                            cell.Value = "USD Equalant @ " + BordxResponse.currencyCode + " " + BordxResponse.currentUSDConversionRate.ToString("#,##0.00");

                        }

                        var endorsementcancelledRate = FormulasToBeCreated.Where(a => a.section == "endorsementcancelled").OrderBy(a => a.column).FirstOrDefault();
                        if (endorsementcancelledRate != null)
                        {
                            cell = ws.Cells[endorsementcancelledRate.end + 1, endorsementcancelledRate.column - 1];
                            var beforeborder = cell.Style.Border;
                            beforeborder.Left.Style =
                            beforeborder.Bottom.Style =
                            beforeborder.Top.Style =
                            beforeborder.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.Font.Bold = true;
                            cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            cell.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 255, 255));
                            cell.Value = BordxResponse.currencyCode;

                            cell = ws.Cells[endorsementcancelledRate.end + 2, endorsementcancelledRate.column - 1];
                            var border = cell.Style.Border;
                            border.Left.Style =
                            border.Bottom.Style =
                            border.Top.Style =
                            border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.Font.Bold = true;
                            cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            cell.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 255, 255));
                            cell.Value = "USD Equalant @ " + BordxResponse.currencyCode + " " + BordxResponse.currentUSDConversionRate.ToString("#,##0.00");

                        }

                        var cancelledRate = FormulasToBeCreated.Where(a => a.section == "cancelled").OrderBy(a => a.column).FirstOrDefault();
                        if (cancelledRate != null)
                        {
                            cell = ws.Cells[cancelledRate.end + 1, cancelledRate.column - 1];
                            var beforeborder = cell.Style.Border;
                            beforeborder.Left.Style =
                            beforeborder.Bottom.Style =
                            beforeborder.Top.Style =
                            beforeborder.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.Font.Bold = true;
                            cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            cell.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 255, 255));
                            cell.Value = BordxResponse.currencyCode;

                            cell = ws.Cells[cancelledRate.end + 2, cancelledRate.column - 1];
                            var border = cell.Style.Border;
                            border.Left.Style =
                            border.Bottom.Style =
                            border.Top.Style =
                            border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.Font.Bold = true;
                            cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            cell.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 255, 255));
                            cell.Value = "USD Equalant @ " + BordxResponse.currencyCode + " " + BordxResponse.currentUSDConversionRate.ToString("#,##0.00");

                        }

                        var transfersRate = FormulasToBeCreated.Where(a => a.section == "transfers").OrderBy(a => a.column).FirstOrDefault();
                        if (transfersRate != null)
                        {
                            cell = ws.Cells[transfersRate.end + 1, transfersRate.column - 1];
                            var beforeborder = cell.Style.Border;
                            beforeborder.Left.Style =
                            beforeborder.Bottom.Style =
                            beforeborder.Top.Style =
                            beforeborder.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.Font.Bold = true;
                            cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            cell.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 255, 255));
                            cell.Value = BordxResponse.currencyCode;

                            cell = ws.Cells[transfersRate.end + 2, transfersRate.column - 1];
                            var border = cell.Style.Border;
                            border.Left.Style =
                            border.Bottom.Style =
                            border.Top.Style =
                            border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.Font.Bold = true;
                            cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            cell.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 255, 255));
                            cell.Value = "USD Equalant @ " + BordxResponse.currencyCode + " " + BordxResponse.currentUSDConversionRate.ToString("#,##0.00");

                        }

                        #endregion show conversion rate
                        //updating sum
                        foreach (BordxSumFormulaDto formula in FormulasToBeCreated)
                        {
                            cell = ws.Cells[formula.end + 1, formula.column];
                            var columnName = GetExcelColumnName(formula.column);
                            var formulaForCell = "=SUM(" + columnName + formula.start.ToString() + ":" +
                                columnName + formula.end.ToString() + ")";
                            cell.Formula = formulaForCell;
                            var border = cell.Style.Border;
                            border.Left.Style =
                            border.Bottom.Style =
                            border.Top.Style =
                            border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.Font.Bold = true;
                            cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            cell.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 255, 255));
                            cell.Style.Numberformat.Format = "#,##0.00";

                            if (LocalCurrencyColumns.Contains(formula.column))
                            {
                                cell = ws.Cells[formula.end + 2, formula.column];
                                var formulaForCellConversionRate = "=(" + columnName + (formula.end + 1).ToString() + "/"
                                 + BordxResponse.currentUSDConversionRate.ToString() + ")";
                                cell.Formula = formulaForCellConversionRate;
                                border = cell.Style.Border;
                                border.Left.Style =
                                border.Bottom.Style =
                                border.Top.Style =
                                border.Right.Style = ExcelBorderStyle.Thin;
                                cell.Style.Font.Bold = true;
                                cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                cell.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 255, 255));
                                cell.Style.Numberformat.Format = "#,##0.00";
                            }

                        }

                        //ws.Column(SubHeaderColumn).Width = Convert.ToDouble(subHeader.ColumnWidth);
                    }
                }

                Byte[] bin = p.GetAsByteArray();
                return new MemoryStream(bin);
            }
        }

        private object GetCellValue(List<string> DynamicTaxColumns, bool shouldConvert, int colIndex, int rowIndex, List<DataTransfer.Common.TaxColumnInfo> TaxAppearsIn, DataColumn dc, DataRow dr, decimal conversionRate, out bool taxDone)
        {
            object response = new object();
            taxDone = false;
            int taxColumnCount = TaxAppearsIn.Where(a => a.Column == (colIndex)).Count();

            if (taxColumnCount > 0)
            {
                taxDone = true;
                TAS.DataTransfer.Common.CountryTaxInfo TaxInfo = TaxAppearsIn
                    .Where(a => a.Column == (colIndex)).FirstOrDefault().taxInformation;

                var TaxValidationResponse = ValidateContractWithTax(TaxInfo.TaxId, Guid.Parse(dr["ContractId"].ToString()), Guid.Parse(dr["PolicyId"].ToString()));
                if (!TaxValidationResponse.Status)
                {
                    response = (decimal)0.00;
                    return response;
                }
                if (shouldConvert)
                {
                    response = Math.Round(ConvertFromBaseCurrency(
                        TaxValidationResponse.CountryTax.ValueIncluededInPolicy,
                        conversionRate
                        ) * 100) / 100;
                }
                else
                {
                    response = Math.Round(TaxValidationResponse.CountryTax.ValueIncluededInPolicy * 100) / 100;
                }
                colIndex++;

            }
            else
            {

                //Setting Value in cell
                if (dc.DataType == typeof(DateTime))
                {
                    DateTime validDate;
                    if (DateTime.TryParse(dr[dc.ColumnName].ToString(), out validDate))
                    {

                        //cell.Style.Numberformat.Format = "dd-mmm-yyyy";
                        if (validDate == DateTime.MinValue || SqlDateTime.MinValue == validDate)
                        {
                            response = "-";
                        }
                        else
                        {
                            response = "=DATE(" + validDate.Year + "," + validDate.Month + "," + validDate.Day + ")";
                        }
                    }
                    else
                    {
                        response = dr[dc.ColumnName];
                    }
                }
                else
                {

                    if (shouldConvert)
                    {
                        if (isDecimal(dr[dc.ColumnName]))
                        {
                            response = ConvertFromBaseCurrency(
                                (decimal.Parse(dr[dc.ColumnName].ToString())),
                                conversionRate);
                        }
                        else
                        {
                            response = dr[dc.ColumnName];
                        }
                    }
                    else
                    {
                        if (isDecimal(dr[dc.ColumnName]) && dc.ColumnName.ToLower() != "vinno" && dc.ColumnName.ToLower() != "conversionrateus" && dc.ColumnName.ToLower() != "mobilenumber")
                        {
                            response = Math.Round(decimal.Parse(dr[dc.ColumnName].ToString()) * 100) / 100;
                        }
                        else if (isDecimal(dr[dc.ColumnName]) && dc.ColumnName.ToLower() == "conversionrateus") {
                            response = Math.Round(decimal.Parse(dr[dc.ColumnName].ToString()) ,4);
                        }
                        else
                        {
                            response = dr[dc.ColumnName];
                        }
                    }
                }
            }
            return response;
        }

        private ValidateContractWithTaxResponseDto ValidateContractWithTax(Guid countryTaxId, Guid ContractId, Guid PolicyId)
        {
            ValidateContractWithTaxResponseDto Response = new ValidateContractWithTaxResponseDto();
            try
            {
                IBordxManagementService BordxManagementService = ServiceFactory.GetBordxManagementService();
                Response = BordxManagementService.ValidateContractWithTax(countryTaxId, ContractId, PolicyId, securityContext, auditContext);
            }
            catch (Exception)
            { }
            return Response;
        }

        private decimal ConvertFromBaseCurrency(decimal value, decimal conversionRate)
        {
            return Math.Round(value * conversionRate * 100) / 100;
        }

        private decimal GetConversionRate(Guid BaseCurrencyId, Guid BaseCurrencyPeriodId)
        {
            decimal Response = (decimal)1;
            try
            {
                ICurrencyManagementService CurrencyManagementService = ServiceFactory.GetCurrencyManagementService();
                Response = CurrencyManagementService.GetConversionRate(BaseCurrencyId, BaseCurrencyPeriodId, securityContext, auditContext);
                return Response;
            }
            catch (Exception)
            { }
            return Response;
        }

        private bool isDecimal(object value)
        {
            decimal outputDec;
            if (Decimal.TryParse(value.ToString(), out outputDec))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private string GetExcelColumnName(int columnNumber)
        {
            int dividend = columnNumber;
            string columnName = String.Empty;
            int modulo;

            while (dividend > 0)
            {
                modulo = (dividend - 1) % 26;
                columnName = Convert.ToChar(65 + modulo).ToString() + columnName;
                dividend = (int)((dividend - modulo) / 26);
            }

            return columnName;
        }

        private static bool IsGuid(string candidate)
        {
            if (candidate == "00000000-0000-0000-0000-000000000000")
                return false;
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



        internal Stream GenerateDealerInvoiceExcel(DealerInvoicesGenerateResponseDto Response, out string docName)
        {
            using (ExcelPackage p = new ExcelPackage())
            {


                List<string> ReferanceRows = new List<string>(){
                    "currencyconversionrate"};

                //Here setting some document properties
                p.Workbook.Properties.Author = "Trivow Business Solutions";
                p.Workbook.Properties.Title = "TAS Dealer Invoice Report";
                int worksheet = 1;
                DataTable dt = Response.DealerInvoiceData;
                //Create a sheet
                p.Workbook.Worksheets.Add(Response.DealerName);
                ExcelWorksheet ws = p.Workbook.Worksheets[worksheet];
                ws.Cells.Style.Font.Size = 10; //Default font size for whole sheet
                ws.Cells.Style.Font.Name = "Calibri"; //Default Font name for whole sheet

                //Merging cells and create a center heading for out table
                //Merging cells and create a center heading for out table
                Image image;
                try
                {
                    byte[] bytes = Convert.FromBase64String(Response.TpaLogo);
                    using (MemoryStream ms = new MemoryStream(bytes))
                    {
                        image = Image.FromStream(ms);
                    }
                }
                catch (Exception)
                {
                    image = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"STANDARD\\assets\\images\\bg_2.png"));

                }
                ws.Row(1).Height = image.Height * .75;
                var picture = ws.Drawings.AddPicture("logo", image);
                picture.SetPosition(0, 0);
                ws.Cells[1, 1, 1, 2].Merge = true;

                //ws.Cells[1, 3].Value = "Invoices of " + Response.DealerName + " for " + Response.Year + "/" + CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Int16.Parse(Response.Month.ToString()));
                ws.Cells[1, 1, 1, dt.Columns.Count].Merge = true;

                ws.Cells[2, 1].Value = " ATTN: " + Response.DealerSalesPersonName;
                ws.Cells[2, 1, 2, 4].Merge = true;

                ws.Cells[2, 5].Value = "Invoice Date:" + DateTime.Now.ToString("dd-MMM-yyyy");
                ws.Cells[2, 5, 2, 10].Merge = true;


                ws.Cells[3, 1].Value = Response.DealerName;
                ws.Cells[3, 1, 3, 10].Merge = true;

                ws.Cells[4, 1].Value = " This invoice is for the billing period " + GetYearMonthFormatted(Response.Year, Response.Month);
                ws.Cells[4, 1, 4, 10].Merge = true;

                //create top column heading
                int StartingColumn = 1;
                int HeaderRow = 6;

                #region header looping
                foreach (DealerInvoiceReportColumnsResponseDto header in Response.DealerInvoiceReportColumns.OrderBy(a => a.Sequance).ToList())
                {
                    DataColumnCollection columns = dt.Columns;
                    if (columns.Contains(header.KeyName))
                    {
                        //this is a valid column
                        if (header.KeyName.ToLower() == "grosspremium")
                        {
                            ws.Cells[HeaderRow, StartingColumn].Value = header.DisplayName + " (" + Response.DealerCurrencyName + ")";

                        }
                        else
                        {
                            ws.Cells[HeaderRow, StartingColumn].Value = header.DisplayName;

                        }

                        ws.Cells[HeaderRow, StartingColumn].Style.Font.Bold = true;
                        var border = ws.Cells[HeaderRow, StartingColumn].Style.Border;
                        border.Left.Style = border.Top.Style = border.Bottom.Style =
                            border.Right.Style = ExcelBorderStyle.Thin;
                        ws.Cells[HeaderRow, StartingColumn].AutoFitColumns();

                        StartingColumn++;
                    }
                    else
                    {
                    }
                }
                #endregion header looping

                //getting unwanted columns
                List<string> ColumnsToRemove = new List<string>();
                foreach (DataColumn column in dt.Columns)
                {
                    if (!ReferanceRows.Contains(column.ColumnName))
                    {
                        int rowAvailable = Response.DealerInvoiceReportColumns.Where(a => a.KeyName.ToLower() == column.ColumnName.ToLower()).Count();
                        if (rowAvailable == 0)
                        {
                            ColumnsToRemove.Add(column.ColumnName);
                        }
                    }
                }

                //removing unwanted columns
                foreach (string columnName in ColumnsToRemove)
                {
                    if (!ReferanceRows.Contains(columnName.ToLower()))
                    {
                        dt.Columns.Remove(columnName);

                    }
                }

                int colIndex = 1, rowIndex = 6, rowGrossSum = 0, colGrossSum = 0;

                var cell = ws.Cells[rowIndex, colIndex];

                //saving active policies
                foreach (DataRow dr in dt.Rows) // Adding Data into rows
                {
                    colIndex = 1;
                    rowIndex++;

                    foreach (DataColumn dc in dt.Columns)
                    {

                        if (ReferanceRows.Contains(dc.ColumnName.ToLower()))
                            continue;
                        cell = ws.Cells[rowIndex, colIndex];
                        if (dc.ColumnName.ToLower() == "grosspremium")
                        {
                            decimal cellValue = Math.Round(decimal.Parse(dr[dc].ToString())
                                * decimal.Parse(dr["CurrencyConversionRate"].ToString()) * 100) / 100;
                            cell.Style.Numberformat.Format = "#,##0.00";
                            cell.Value = cellValue;
                            colGrossSum = colIndex;
                            rowGrossSum = rowIndex;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        }
                        else
                        {
                            if (dc.DataType == typeof(DateTime))
                            {
                                DateTime validDate;
                                if (DateTime.TryParse(dr[dc.ColumnName].ToString(), out validDate))
                                {

                                    if (SqlDateTime.MinValue == validDate)
                                    {
                                        cell.Value = "-";
                                    }
                                    else
                                    {
                                        cell.Value = "=DATE(" + validDate.Year + "," + validDate.Month + "," + validDate.Day + ")";
                                        cell.Style.Numberformat.Format = "dd-mmm-yyyy";
                                        cell.Formula = cell.Value.ToString();
                                    }
                                }
                                else
                                {
                                    cell.Value = dr[dc.ColumnName];
                                }
                            }
                            else
                            {
                                cell.Value = dr[dc].ToString();
                            }
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        }
                        var border = cell.Style.Border;
                        border.Left.Style =
                        border.Bottom.Style =
                        border.Top.Style =
                        border.Right.Style = ExcelBorderStyle.Thin;

                        // cell.AutoFitColumns();
                        colIndex++;
                    }

                }
                //formula setup
                cell = ws.Cells[rowGrossSum + 1, colGrossSum];
                var formulaForCell = "=SUM(" + GetExcelColumnName(colGrossSum) + 3 + ":" +
                            GetExcelColumnName(colGrossSum) + rowGrossSum.ToString() + ")";
                cell.Formula = formulaForCell;
                var borderSum = cell.Style.Border;
                borderSum.Left.Style =
                borderSum.Bottom.Style =
                borderSum.Top.Style =
                borderSum.Right.Style = ExcelBorderStyle.Thin;
                cell.Style.Font.Bold = true;
                cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                cell.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(153, 204, 255));
                cell.Style.Numberformat.Format = "#,##0.00";
                //labeling
                cell = ws.Cells[rowGrossSum + 1, colGrossSum - 1];
                borderSum = cell.Style.Border;
                borderSum.Left.Style =
                borderSum.Bottom.Style =
                borderSum.Top.Style =
                borderSum.Right.Style = ExcelBorderStyle.Thin;
                cell.Style.Font.Bold = true;
                cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                cell.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(153, 204, 255));
                cell.Value = "Total";
                //name setup
                docName = Response.DealerName + "-Invoice-" + GetYearMonthFormatted(Response.Year, Response.Month);
                //conversion
                Byte[] bin = p.GetAsByteArray();
                return new MemoryStream(bin);

            }
        }

        private string GetYearMonthFormatted(int year, int month)
        {
            DateTime date = new DateTime(year, month, 1);
            string stratDate = 1.Ordinalize() + " " + date.ToString("MMMM");
            string endDate = date.AddMonths(1).AddDays(-1).Day.Ordinalize() + " " + date.ToString("MMMM");

            return stratDate + " - " + endDate + " , " + year;
        }


        internal Stream GenarateClaimBordxExcel(ClaimBordxExportResponseDto response)
        {
            using (ExcelPackage p = new ExcelPackage())
            {
                List<string> ReferanceRows = new List<string>() { "currencyconversionrate" };

                //Here setting some document properties
                p.Workbook.Properties.Author = "Trivow Business Solutions";
                p.Workbook.Properties.Title = "TAS Dealer Invoice Report";

                int worksheet = 1;

                if (response.ClaimSummaries.Count() > 0)
                {

                    //Create a sheet
                    p.Workbook.Worksheets.Add("Claim Summary");
                    ExcelWorksheet ws = p.Workbook.Worksheets[worksheet];

                    ws.Cells.Style.Font.Size = 11; //Default font size for whole sheet
                    ws.Cells.Style.Font.Name = "Calibri"; //Default Font name for whole sheet

                    ws.Column(1).Width = 25;
                    ws.Column(2).Width = 25;
                    ws.Column(3).Width = 25;
                    ws.Column(4).Width = 25;
                    ws.Column(5).Width = 15;
                    ws.Column(6).Width = 15;
                    ws.Column(7).Width = 15;
                    ws.Column(8).Width = 30;

                    int mergeColumns = 8;
                    int startRow = 1;

                    decimal totalAmount = 0;
                    decimal toatalpaid = 0;
                    decimal outstanding = 0;

                    ws.Cells[startRow, 1].Value = "Reinsurer";
                    ws.Cells[startRow, 2].Value = response.ClaimBordxHeader.Reinsurer;
                    ws.Cells[startRow, 2, startRow, mergeColumns].Merge = true;

                    startRow++;

                    ws.Cells[startRow, 1].Value = "Country";// + response.ClaimBordxHeader.Country;
                    ws.Cells[startRow, 2].Value = response.ClaimBordxHeader.Country;
                    ws.Cells[startRow, 2, startRow, mergeColumns].Merge = true;

                    startRow++;

                    ws.Cells[startRow, 1].Value = "Cedent Name";// + response.ClaimBordxHeader.CedentName;
                    ws.Cells[startRow, 2].Value = response.ClaimBordxHeader.CedentName;
                    ws.Cells[startRow, 2, startRow, mergeColumns].Merge = true;

                    startRow++;

                    ws.Cells[4, 1].Value = "Date From";// + response.ClaimBordxHeader.CedentName;
                    ws.Cells[4, 2].Value = response.ClaimBordxHeader.FromDate.ToString("dd-MMM-yyyy");
                    ws.Cells[4, 2, 4, mergeColumns].Merge = true;

                    startRow++;

                    ws.Cells[startRow, 1].Value = "Date To";// + response.ClaimBordxHeader.CedentName;
                    ws.Cells[startRow, 2].Value = response.ClaimBordxHeader.ToDate.ToString("dd-MMM-yyyy");
                    ws.Cells[startRow, 2, startRow, mergeColumns].Merge = true;

                    startRow++;

                    ws.Cells[startRow, 1, startRow, mergeColumns].Merge = true;

                    ws.Cells[1, 1, startRow, mergeColumns].Style.Border.BorderAround(ExcelBorderStyle.None);

                    startRow++;


                    ws.Cells[startRow, 1].Value = "Paid Claim Summary";
                    ws.Cells[startRow, 1, startRow, mergeColumns].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells[startRow, 1, startRow, mergeColumns].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells[startRow, 1, startRow, mergeColumns].Style.Fill.BackgroundColor.SetColor(Color.Gray);
                    ws.Cells[startRow, 1, startRow, mergeColumns].Style.Border.Top.Style = ExcelBorderStyle.Medium;
                    ws.Cells[startRow, 1, startRow, mergeColumns].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                    ws.Cells[startRow, 1, startRow, mergeColumns].Merge = true;

                    ws.Cells[1, 1, startRow, 1].Style.Font.Bold = true;
                    ws.Cells[1, 1, startRow, mergeColumns].Style.Border.BorderAround(ExcelBorderStyle.None);

                    startRow++;

                    //headers
                    ws.Cells[startRow, 1].Value = "Transaction Number";
                    ws.Cells[startRow, 2].Value = "Claim Bordereaux Year";
                    ws.Cells[startRow, 3].Value = "Claim Bordereaux Month";
                    ws.Cells[startRow, 4].Value = "Bordereaux Reference Number";
                    ws.Cells[startRow, 5].Value = "Claim Amount";
                    ws.Cells[startRow, 6].Value = "Paid Amount";
                    ws.Cells[startRow, 7].Value = "Outstanding";
                    ws.Cells[startRow, 8].Value = "Remarks";
                    ws.Cells[startRow, 1, startRow, mergeColumns].Style.Font.Bold = true;

                    for (int c = 1; c <= mergeColumns; c++) { ws.Cells[startRow, 1, startRow, c].Style.Border.BorderAround(ExcelBorderStyle.Thin); }

                    startRow++;

                    foreach (var detail in response.ClaimSummaries)
                    {

                        ws.Cells[startRow, 1].Value = detail.TransactionNumber;
                        ws.Cells[startRow, 2].Value = detail.ClaimBordxYear;
                        ws.Cells[startRow, 3].Value = detail.ClaimBordxMonth;
                        ws.Cells[startRow, 4].Value = detail.ClaimBordxReferneceNumber;
                        ws.Cells[startRow, 5].Style.Numberformat.Format = "#,##0.00";
                        ws.Cells[startRow, 5].Value = detail.ClaimAmount;
                        ws.Cells[startRow, 6].Style.Numberformat.Format = "#,##0.00";
                        ws.Cells[startRow, 6].Value = detail.PaidAmount;
                        ws.Cells[startRow, 7].Style.Numberformat.Format = "#,##0.00";
                        ws.Cells[startRow, 7].Value = detail.Outstanding;
                        ws.Cells[startRow, 8].Value = detail.Remarks;

                        toatalpaid = toatalpaid + detail.PaidAmount;
                        totalAmount = totalAmount + detail.ClaimAmount;
                        outstanding = outstanding + detail.Outstanding;

                        for (int i = 1; i <= mergeColumns; i++) ws.Cells[startRow, i].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                        startRow++;
                    }

                    ws.Cells[startRow, 1, startRow, mergeColumns].Merge = true;
                    ws.Cells[startRow, 1, startRow, mergeColumns].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    startRow++;

                    ws.Cells[startRow, 1].Value = "Yearly Total";
                    ws.Cells[startRow, 5].Style.Numberformat.Format = "#,##0.00";
                    ws.Cells[startRow, 5].Value = totalAmount;
                    ws.Cells[startRow, 6].Style.Numberformat.Format = "#,##0.00";
                    ws.Cells[startRow, 6].Value = toatalpaid;
                    ws.Cells[startRow, 7].Style.Numberformat.Format = "#,##0.00";
                    ws.Cells[startRow, 7].Value = outstanding;

                    ws.Cells[startRow, 1, startRow, mergeColumns].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    ws.Cells[startRow, 1, startRow, mergeColumns].Style.Font.Bold = true;
                    ws.Cells[startRow, 1, startRow, 3].Merge = true;
                    ws.Cells[startRow, 1, startRow, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[startRow, 4].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[startRow, 5].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[startRow, 6].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[startRow, 7].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[startRow, 8].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    startRow++;

                    ws.Cells[startRow, 1, startRow, mergeColumns].Merge = true;
                    ws.Cells[startRow, 1, startRow, mergeColumns].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    startRow++;

                    ws.Cells[startRow, 1].Value = "Grand Total From Inception";
                    ws.Cells[startRow, 5].Style.Numberformat.Format = "#,##0.00";
                    ws.Cells[startRow, 5].Value = totalAmount;
                    ws.Cells[startRow, 6].Style.Numberformat.Format = "#,##0.00";
                    ws.Cells[startRow, 6].Value = toatalpaid;
                    ws.Cells[startRow, 7].Style.Numberformat.Format = "#,##0.00";
                    ws.Cells[startRow, 7].Value = outstanding;

                    ws.Cells[startRow, 1, startRow, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    ws.Cells[startRow, 1, startRow, mergeColumns].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells[startRow, 1, startRow, mergeColumns].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                    ws.Cells[startRow, 1, startRow, mergeColumns].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    ws.Cells[startRow, 1, startRow, mergeColumns].Style.Border.Bottom.Style = ExcelBorderStyle.Double;
                    ws.Cells[startRow, 1, startRow, 4].Merge = true;
                    ws.Cells[startRow, 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    ws.Cells[startRow, 1].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    ws.Cells[startRow, 5].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    ws.Cells[startRow, 6].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    ws.Cells[startRow, 7].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    ws.Cells[startRow, 8].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                    ws.Cells[startRow, 1, startRow, mergeColumns].Style.Font.Bold = true;

                    startRow++;

                    ws.Cells[startRow, 1, startRow, mergeColumns].Merge = true;
                    ws.Cells[startRow, 1, startRow, mergeColumns].Style.Border.BorderAround(ExcelBorderStyle.None);

                    startRow++;

                    ws.Cells[startRow, 1].Value = "All amounts are in USD";
                    ws.Cells[startRow, 1, startRow, mergeColumns].Merge = true;
                    ws.Cells[startRow, 1, startRow, mergeColumns].Style.Border.BorderAround(ExcelBorderStyle.None);

                    startRow++;

                    ws.Cells[startRow, 1, startRow, mergeColumns].Merge = true;
                    ws.Cells[startRow, 1, startRow, mergeColumns].Style.Border.BorderAround(ExcelBorderStyle.None);

                    startRow++;

                    ws.Cells[startRow, 4].Value = "Claim Float Calculation";
                    ws.Cells[startRow, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    ws.Cells[startRow, 1, startRow, 4].Style.Font.Bold = true;
                    ws.Cells[startRow, 1, startRow, mergeColumns].Style.Border.BorderAround(ExcelBorderStyle.None);

                    startRow++;

                    ws.Cells[startRow, 4].Value = "Claim Float Amount";
                    ws.Cells[startRow, 5].Style.Numberformat.Format = "#,##0.00";
                    ws.Cells[startRow, 5].Value = totalAmount;
                    ws.Cells[startRow, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    ws.Cells[startRow, 1, startRow, mergeColumns].Style.Border.BorderAround(ExcelBorderStyle.None);

                    startRow++;

                    ws.Cells[startRow, 4].Value = "Claim Outstanding Amount";
                    ws.Cells[startRow, 5].Style.Numberformat.Format = "#,##0.00";
                    ws.Cells[startRow, 5].Value = outstanding;
                    ws.Cells[startRow, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    ws.Cells[startRow, 1, startRow, mergeColumns].Style.Border.BorderAround(ExcelBorderStyle.None);

                    startRow++;

                    ws.Cells[startRow, 4].Value = "Balance Float Amount";
                    ws.Cells[startRow, 5].Style.Numberformat.Format = "#,##0.00";
                    ws.Cells[startRow, 5].Value = toatalpaid;
                    ws.Cells[startRow, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    ws.Cells[startRow, 1, startRow, mergeColumns].Style.Border.BorderAround(ExcelBorderStyle.None);

                    startRow++;

                    ws.Cells[startRow, 1, startRow, mergeColumns].Merge = true;
                    ws.Cells[startRow, 1, startRow, mergeColumns].Style.Border.BorderAround(ExcelBorderStyle.None);

                    startRow++;

                    ws.Cells[startRow, 1, startRow, mergeColumns].Merge = true;
                    ws.Cells[startRow, 1, startRow, mergeColumns].Style.Border.BorderAround(ExcelBorderStyle.None);

                    startRow++;

                    ws.Cells[startRow, 1, startRow, mergeColumns].Merge = true;
                    ws.Cells[startRow, 1, startRow, mergeColumns].Style.Border.BorderAround(ExcelBorderStyle.None);

                    startRow++;

                    ws.Cells[startRow, 1].Value = " Authorized Signatory";
                    ws.Cells[startRow, mergeColumns].Value = "Date";
                    ws.Cells[startRow, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    ws.Cells[startRow, mergeColumns].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    ws.Cells[startRow, 1, startRow, mergeColumns].Style.Border.BorderAround(ExcelBorderStyle.None);
                    ws.Cells[startRow, 1, startRow, 2].Merge = true;


                    startRow++;

                }

                if (response.ClaimMonthAndYears.Count() > 0)
                {
                    worksheet++;
                    //Create a sheet
                    p.Workbook.Worksheets.Add("Contract Month & Year");
                    ExcelWorksheet ws = p.Workbook.Worksheets[worksheet];

                    ws.Cells.Style.Font.Size = 11; //Default font size for whole sheet
                    ws.Cells.Style.Font.Name = "Calibri"; //Default Font name for whole sheet

                    ws.Column(1).Width = 15;
                    ws.Column(2).Width = 15;
                    ws.Column(3).Width = 30;
                    ws.Column(4).Width = 25;

                    int mergeColumns = 4;
                    int startRow = 1;

                    decimal totalCount = 0;
                    decimal toatalAmount = 0;
                    decimal toatalAmountInLocal = 0;

                    ws.Cells[startRow, 1].Value = "Reinsurer";
                    ws.Cells[startRow, 2].Value = response.ClaimBordxHeader.Reinsurer;
                    ws.Cells[startRow, 2, startRow, mergeColumns].Merge = true;

                    startRow++;

                    ws.Cells[startRow, 1].Value = "Country";// + response.ClaimBordxHeader.Country;
                    ws.Cells[startRow, 2].Value = response.ClaimBordxHeader.Country;
                    ws.Cells[startRow, 2, startRow, mergeColumns].Merge = true;

                    startRow++;

                    ws.Cells[startRow, 1].Value = "Cedent Name";// + response.ClaimBordxHeader.CedentName;
                    ws.Cells[startRow, 2].Value = response.ClaimBordxHeader.CedentName;
                    ws.Cells[startRow, 2, startRow, mergeColumns].Merge = true;

                    startRow++;

                    ws.Cells[4, 1].Value = "Date From";// + response.ClaimBordxHeader.CedentName;
                    ws.Cells[4, 2].Value = response.ClaimBordxHeader.FromDate.ToString("dd-MMM-yyyy");
                    ws.Cells[4, 2, 4, mergeColumns].Merge = true;

                    startRow++;

                    ws.Cells[startRow, 1].Value = "Date To";// + response.ClaimBordxHeader.CedentName;
                    ws.Cells[startRow, 2].Value = response.ClaimBordxHeader.ToDate.ToString("dd-MMM-yyyy");
                    ws.Cells[startRow, 2, startRow, mergeColumns].Merge = true;

                    startRow++;

                    ws.Cells[startRow, 1, startRow, mergeColumns].Merge = true;

                    ws.Cells[1, 1, startRow, mergeColumns].Style.Border.BorderAround(ExcelBorderStyle.None);

                    startRow++;

                    ws.Cells[startRow, 1].Value = "Claim Summary";
                    ws.Cells[startRow, 1, startRow, mergeColumns].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells[startRow, 1, startRow, mergeColumns].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells[startRow, 1, startRow, mergeColumns].Style.Fill.BackgroundColor.SetColor(Color.Gray);
                    ws.Cells[startRow, 1, startRow, mergeColumns].Style.Border.Top.Style = ExcelBorderStyle.Medium;
                    ws.Cells[startRow, 1, startRow, mergeColumns].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                    ws.Cells[startRow, 1, startRow, mergeColumns].Merge = true;

                    ws.Cells[1, 1, startRow, 1].Style.Font.Bold = true;
                    ws.Cells[1, 1, startRow, mergeColumns].Style.Border.BorderAround(ExcelBorderStyle.None);

                    startRow++;

                    //headers
                    ws.Cells[startRow, 1].Value = "UW Year";
                    ws.Cells[startRow, 2].Value = "Paid Claim Count";
                    ws.Cells[startRow, 3].Value = "Paid Amount In Local Currency";
                    ws.Cells[startRow, 4].Value = "Paid Amount In Dollars";
                    ws.Cells[startRow, 1, startRow, mergeColumns].Style.Font.Bold = true;

                    for (int c = 1; c <= mergeColumns; c++) { ws.Cells[startRow, 1, startRow, c].Style.Border.BorderAround(ExcelBorderStyle.Thin); }

                    startRow++;

                    foreach (var detail in response.ClaimMonthAndYears)
                    {
                        ws.Cells[startRow, 1].Value = detail.UWYear;
                        ws.Cells[startRow, 2].Value = detail.PaidClaimCount;
                        ws.Cells[startRow, 3].Style.Numberformat.Format = "#,##0.00";
                        ws.Cells[startRow, 3].Value = detail.PaiClaimAmountInLocal;
                        ws.Cells[startRow, 4].Style.Numberformat.Format = "#,##0.00";
                        ws.Cells[startRow, 4].Value = detail.PaiClaimAmount;

                        totalCount = totalCount + detail.PaidClaimCount;
                        toatalAmount = toatalAmount + detail.PaiClaimAmount;
                        toatalAmountInLocal = toatalAmountInLocal + detail.PaiClaimAmountInLocal;

                        for (int i = 1; i <= mergeColumns; i++) ws.Cells[startRow, i].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                        startRow++;
                    }

                    ws.Cells[startRow, 1, startRow, mergeColumns].Merge = true;
                    ws.Cells[startRow, 1, startRow, mergeColumns].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    startRow++;

                    ws.Cells[startRow, 1].Value = "Total";
                    ws.Cells[startRow, 2].Value = totalCount;
                    ws.Cells[startRow, 3].Style.Numberformat.Format = "#,##0.00";
                    ws.Cells[startRow, 3].Value = toatalAmountInLocal;
                    ws.Cells[startRow, 4].Style.Numberformat.Format = "#,##0.00";
                    ws.Cells[startRow, 4].Value = toatalAmount;

                    ws.Cells[startRow, 1, startRow, mergeColumns].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    ws.Cells[startRow, 1, startRow, mergeColumns].Style.Font.Bold = true;
                    ws.Cells[startRow, 1, startRow, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[startRow, 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[startRow, 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[startRow, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[startRow, 4].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    startRow++;

                    ws.Cells[startRow, 1, startRow, mergeColumns].Merge = true;
                    ws.Cells[startRow, 1, startRow, mergeColumns].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    startRow++;

                    ws.Cells[startRow, 1].Value = "Grand Total $";
                    ws.Cells[startRow, 2].Value = totalCount;
                    ws.Cells[startRow, 3].Style.Numberformat.Format = "#,##0.00";
                    ws.Cells[startRow, 3].Value = toatalAmountInLocal;
                    ws.Cells[startRow, 4].Style.Numberformat.Format = "#,##0.00";
                    ws.Cells[startRow, 4].Value = toatalAmount;

                    ws.Cells[startRow, 1, startRow, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    ws.Cells[startRow, 1, startRow, mergeColumns].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells[startRow, 1, startRow, mergeColumns].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                    ws.Cells[startRow, 1, startRow, mergeColumns].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    ws.Cells[startRow, 1, startRow, mergeColumns].Style.Border.Bottom.Style = ExcelBorderStyle.Double;
                    ws.Cells[startRow, 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    ws.Cells[startRow, 1].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    ws.Cells[startRow, 2].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    ws.Cells[startRow, 3].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    ws.Cells[startRow, 4].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                    ws.Cells[startRow, 1, startRow, mergeColumns].Style.Font.Bold = true;

                    startRow++;

                    ws.Cells[startRow, 1, startRow, mergeColumns].Merge = true;
                    ws.Cells[startRow, 1, startRow, mergeColumns].Style.Border.BorderAround(ExcelBorderStyle.None);

                    startRow++;

                }

                if (response.PaymentSchedule.Count() > 0)
                {
                    worksheet++;
                    //Create a sheet
                    p.Workbook.Worksheets.Add("Payment Schedule");
                    ExcelWorksheet ws = p.Workbook.Worksheets[worksheet];

                    ws.Cells.Style.Font.Size = 11; //Default font size for whole sheet
                    ws.Cells.Style.Font.Name = "Calibri"; //Default Font name for whole sheet

                    int mergeColumns = 18;
                    int startRow = 1;

                    for (var c = 1; c <= mergeColumns; c++) { ws.Column(c).Width = 18; }

                    ws.Cells[startRow, 1].Value = "Reinsurer";
                    ws.Cells[startRow, 2].Value = response.ClaimBordxHeader.Reinsurer;
                    ws.Cells[startRow, 2, startRow, mergeColumns].Merge = true;

                    startRow++;

                    ws.Cells[startRow, 1].Value = "Country";// + response.ClaimBordxHeader.Country;
                    ws.Cells[startRow, 2].Value = response.ClaimBordxHeader.Country;
                    ws.Cells[startRow, 2, startRow, mergeColumns].Merge = true;

                    startRow++;

                    ws.Cells[startRow, 1].Value = "Cedent Name";// + response.ClaimBordxHeader.CedentName;
                    ws.Cells[startRow, 2].Value = response.ClaimBordxHeader.CedentName;
                    ws.Cells[startRow, 2, startRow, mergeColumns].Merge = true;

                    startRow++;

                    ws.Cells[4, 1].Value = "Date From";// + response.ClaimBordxHeader.CedentName;
                    ws.Cells[4, 2].Value = response.ClaimBordxHeader.FromDate.ToString("dd-MMM-yyyy");
                    ws.Cells[4, 2, 4, mergeColumns].Merge = true;

                    startRow++;

                    ws.Cells[startRow, 1].Value = "Date To";// + response.ClaimBordxHeader.CedentName;
                    ws.Cells[startRow, 2].Value = response.ClaimBordxHeader.ToDate.ToString("dd-MMM-yyyy");
                    ws.Cells[startRow, 2, startRow, mergeColumns].Merge = true;


                    var yearlist = response.PaymentSchedule.GroupBy(a => a.YearOfAccount).Select(a => new { a.Key });

                    int count = 0;

                    foreach (var year in yearlist)
                    {
                        count++;

                        decimal toatalAmount = 0;
                        decimal toatalAmountInLocal = 0;

                        var dataList = response.PaymentSchedule.Where(a => a.YearOfAccount == year.Key).ToList();

                        var bordxMonthRef = response.PaymentSchedule.First().BordxMonth + ((count.ToString().Length > 1) ? count.ToString() : ("0" + count.ToString()));


                        startRow++;

                        ws.Cells[startRow, 1].Value = "Payment Scehdule";
                        ws.Cells[startRow, 1, startRow, mergeColumns].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells[startRow, 1, startRow, mergeColumns].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells[startRow, 1, startRow, mergeColumns].Style.Fill.BackgroundColor.SetColor(Color.Gray);
                        ws.Cells[startRow, 1, startRow, mergeColumns].Style.Border.Top.Style = ExcelBorderStyle.Medium;
                        ws.Cells[startRow, 1, startRow, mergeColumns].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                        ws.Cells[startRow, 1, startRow, mergeColumns].Merge = true;

                        ws.Cells[1, 1, startRow, 1].Style.Font.Bold = true;
                        ws.Cells[1, 1, startRow, mergeColumns].Style.Border.BorderAround(ExcelBorderStyle.None);

                        startRow++;

                        ws.Cells[startRow, 1, startRow + 1, mergeColumns].Merge = true;
                        ws.Cells[1, 1, startRow + 1, mergeColumns].Style.Border.BorderAround(ExcelBorderStyle.None);

                        startRow++;
                        startRow++;

                        //headers
                        ws.Cells[startRow, 1].Value = "Cover Holder";
                        ws.Cells[startRow, 2].Value = "Month Of Account";
                        ws.Cells[startRow, 3].Value = "Year Of Account";
                        ws.Cells[startRow, 4].Value = "Contract Year";
                        ws.Cells[startRow, 5].Value = "Payee";
                        ws.Cells[startRow, 6].Value = "Date Of Claim";
                        ws.Cells[startRow, 7].Value = "Date Of Loss";
                        ws.Cells[startRow, 8].Value = "Manf.Warranty Expiry Date";
                        ws.Cells[startRow, 9].Value = "Km at Date of Loss";
                        ws.Cells[startRow, 10].Value = "Manufacturer Warranty Cut off Kms";
                        ws.Cells[startRow, 11].Value = "Ext. Warranty Expiry Km";
                        ws.Cells[startRow, 12].Value = "Claim Ref";
                        ws.Cells[startRow, 13].Value = "Certchas #";
                        ws.Cells[startRow, 14].Value = "Insured";
                        ws.Cells[startRow, 15].Value = "Payment Amount Local Currency";
                        ws.Cells[startRow, 16].Value = "Payment Amount US";
                        ws.Cells[startRow, 17].Value = "Date Premium Was Paid";

                        ws.Cells[startRow, 1, startRow, mergeColumns].Style.Font.Bold = true;

                        for (int c = 1; c <= mergeColumns; c++) { ws.Cells[startRow, 1, startRow, c].Style.Border.BorderAround(ExcelBorderStyle.Thin); }

                        startRow++;

                        foreach (var detail in dataList)
                        {
                            ws.Cells[startRow, 1].Value = detail.CoverHolder;
                            ws.Cells[startRow, 2].Value = detail.MonthOfAccount;
                            ws.Cells[startRow, 3].Value = detail.YearOfAccount;
                            ws.Cells[startRow, 4].Value = detail.ContractYear;
                            ws.Cells[startRow, 5].Value = detail.Payee;
                            ws.Cells[startRow, 6].Value = detail.DateOfClaim;
                            ws.Cells[startRow, 7].Value = detail.DateOfLoss;
                            ws.Cells[startRow, 8].Value = detail.ManfWarrantyExpiryDate.ToString("dd-MMM-yyyy");
                            ws.Cells[startRow, 9].Value = detail.KmatDateofLoss;
                            ws.Cells[startRow, 10].Value = detail.ManufacturerWarrantyCutoffKms;
                            ws.Cells[startRow, 11].Value = detail.ExtWarrantyExpiryKm;
                            ws.Cells[startRow, 12].Value = detail.ClaimRef;
                            ws.Cells[startRow, 13].Value = detail.Certchas;
                            ws.Cells[startRow, 14].Value = detail.Insured;
                            ws.Cells[startRow, 15].Style.Numberformat.Format = "#,##0.00";
                            ws.Cells[startRow, 15].Value = detail.PaiClaimAmountInLocal;
                            ws.Cells[startRow, 16].Style.Numberformat.Format = "#,##0.00";
                            ws.Cells[startRow, 16].Value = detail.PaiClaimAmount;
                            ws.Cells[startRow, 16].Style.Numberformat.Format = "#,##0.00";
                            ws.Cells[startRow, 17].Value = detail.DatePremiumWasPaid;
                            //ws.Cells[startRow, 2].Value = detail.PaidClaimCount;
                            //ws.Cells[startRow, 3].Value = detail.PaiClaimAmountInLocal;
                            //ws.Cells[startRow, 4].Value = detail.PaiClaimAmount;

                            toatalAmount = toatalAmount + detail.PaiClaimAmount;
                            toatalAmountInLocal = toatalAmountInLocal + detail.PaiClaimAmountInLocal;

                            for (int i = 1; i <= mergeColumns; i++) ws.Cells[startRow, i].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                            startRow++;
                        }

                        ws.Cells[startRow, 15].Value = "Total $";
                        ws.Cells[startRow, 16].Style.Numberformat.Format = "#,##0.00";
                        ws.Cells[startRow, 16].Value = toatalAmount;

                        ws.Cells[startRow, 1, startRow, 14].Merge = true;
                        ws.Cells[startRow, 1, startRow, 14].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                        ws.Cells[startRow, 1, startRow, 15].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        ws.Cells[startRow, 1, startRow, 16].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        ws.Cells[startRow, 1, startRow, 17].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                        ws.Cells[startRow, 1, startRow, mergeColumns].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells[startRow, 1, startRow, mergeColumns].Style.Fill.BackgroundColor.SetColor(Color.LightGray);

                        startRow++;

                        ws.Cells[startRow, 1, startRow, mergeColumns].Merge = true;
                        ws.Cells[1, 1, startRow, mergeColumns].Style.Border.BorderAround(ExcelBorderStyle.None);

                        startRow++;

                        //ws.Cells[startRow, 1].Value = "Unique Bordereau Reference tied to binder reference";
                        //ws.Cells[startRow, 5].Value = "B0386ER457630V";

                        //ws.Cells[startRow, 1, startRow, mergeColumns].Merge = true;
                        ws.Cells[1, 1, startRow, mergeColumns].Style.Border.BorderAround(ExcelBorderStyle.None);

                        startRow++;

                        ws.Cells[startRow, 1, startRow, mergeColumns].Merge = true;
                        ws.Cells[1, 1, startRow, mergeColumns].Style.Border.BorderAround(ExcelBorderStyle.None);

                        startRow++;

                        ws.Cells[startRow, 15].Value = "Grand Total";
                        ws.Cells[startRow, 16].Style.Numberformat.Format = "#,##0.00";
                        ws.Cells[startRow, 16].Value = toatalAmount;

                        ws.Cells[startRow, 1, startRow, 14].Merge = true;
                        ws.Cells[startRow, 1, startRow, 14].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                        ws.Cells[startRow, 1, startRow, mergeColumns].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells[startRow, 1, startRow, mergeColumns].Style.Fill.BackgroundColor.SetColor(Color.SlateGray);
                        ws.Cells[startRow, 1, startRow, mergeColumns].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        ws.Cells[startRow, 1, startRow, mergeColumns].Style.Border.Bottom.Style = ExcelBorderStyle.Double;
                        ws.Cells[startRow, 1].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        ws.Cells[startRow, 14].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        ws.Cells[startRow, 15].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        ws.Cells[startRow, 16].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        ws.Cells[startRow, 17].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                        ws.Cells[startRow, 1, startRow, mergeColumns].Style.Font.Bold = true;
                        ws.Cells[startRow, 1, startRow, mergeColumns].Style.Font.Size = 12;

                        startRow++;

                        ws.Cells[startRow, 1, startRow, mergeColumns].Merge = true;
                        ws.Cells[1, 1, startRow, mergeColumns].Style.Border.BorderAround(ExcelBorderStyle.None);

                        startRow++;

                        ws.Cells[startRow, 1, startRow, mergeColumns].Merge = true;
                        ws.Cells[1, 1, startRow, mergeColumns].Style.Border.BorderAround(ExcelBorderStyle.None);

                        startRow++;

                        ws.Cells[startRow, 1].Value = "Authorised Signature________________________";

                        ws.Cells[startRow, 1, startRow, mergeColumns].Merge = true;
                        ws.Cells[1, 1, startRow, mergeColumns].Style.Border.BorderAround(ExcelBorderStyle.None);

                        startRow++;

                        ws.Cells[startRow, 1, startRow, mergeColumns].Merge = true;
                        ws.Cells[1, 1, startRow, mergeColumns].Style.Border.BorderAround(ExcelBorderStyle.None);

                        startRow++;

                        ws.Cells[startRow, 1, startRow, mergeColumns].Merge = true;
                        ws.Cells[1, 1, startRow, mergeColumns].Style.Border.BorderAround(ExcelBorderStyle.None);

                        startRow++;

                        ws.Cells[startRow, 1].Value = "Date____________________________________";

                        ws.Cells[startRow, 1, startRow, mergeColumns].Merge = true;
                        ws.Cells[1, 1, startRow, mergeColumns].Style.Border.BorderAround(ExcelBorderStyle.None);

                        startRow++;

                        ws.Cells[startRow, 1, startRow, mergeColumns].Merge = true;
                        ws.Cells[1, 1, startRow, mergeColumns].Style.Border.BorderAround(ExcelBorderStyle.None);

                        startRow++;

                        ws.Cells[startRow, 1, startRow, mergeColumns].Merge = true;
                        ws.Cells[1, 1, startRow, mergeColumns].Style.Border.BorderAround(ExcelBorderStyle.None);

                        startRow++;

                        ws.Cells[startRow, 1, startRow, mergeColumns].Merge = true;
                        ws.Cells[1, 1, startRow, mergeColumns].Style.Border.BorderAround(ExcelBorderStyle.None);

                        startRow++;

                        ws.Cells[startRow, 1].Value = "Payment Schedule Reference";
                        ws.Cells[startRow, 2].Value = bordxMonthRef;//"0701";
                        ws.Cells[startRow, 3].Value = "(made up of four numbered digit first is for the month and second dictates more than one in a month. Ie " + bordxMonthRef + " - PRMonth Payment Schedule Number PRNumber) ";

                        //ws.Cells[startRow, 3, startRow, mergeColumns].Merge = true;
                        ws.Cells[1, 1, startRow, mergeColumns].Style.Border.BorderAround(ExcelBorderStyle.None);

                        startRow++;

                        ws.Cells[startRow, 1, startRow, mergeColumns].Merge = true;
                        ws.Cells[1, 1, startRow, mergeColumns].Style.Border.BorderAround(ExcelBorderStyle.None);

                        startRow++;
                    }
                }


                if (response.CountryWise.Count() > 0)
                {
                    var contrylist = response.CountryWise.GroupBy(a => a.CountrId).Select(a => new { a.Key });

                    foreach (var country in contrylist)
                    {
                        var dataList = response.CountryWise.Where(a => a.CountrId == country.Key).ToList();

                        worksheet++;
                        //Create a sheet
                        p.Workbook.Worksheets.Add(dataList[0].CountryName);
                        ExcelWorksheet ws = p.Workbook.Worksheets[worksheet];

                        ws.Cells.Style.Font.Size = 11; //Default font size for whole sheet
                        ws.Cells.Style.Font.Name = "Calibri"; //Default Font name for whole sheet

                        int mergeColumns = 41;
                        int startRow = 1;

                        for (var c = 1; c <= mergeColumns; c++) { ws.Column(c).Width = 18; }

                        ws.Cells[startRow, 1].Value = "Reinsurer";
                        ws.Cells[startRow, 2].Value = response.ClaimBordxHeader.Reinsurer;
                        ws.Cells[startRow, 2, startRow, mergeColumns].Merge = true;

                        startRow++;

                        ws.Cells[startRow, 1].Value = "Country";// + response.ClaimBordxHeader.Country;
                        ws.Cells[startRow, 2].Value = response.ClaimBordxHeader.Country;
                        ws.Cells[startRow, 2, startRow, mergeColumns].Merge = true;

                        startRow++;

                        ws.Cells[startRow, 1].Value = "Cedent Name";// + response.ClaimBordxHeader.CedentName;
                        ws.Cells[startRow, 2].Value = response.ClaimBordxHeader.CedentName;
                        ws.Cells[startRow, 2, startRow, mergeColumns].Merge = true;

                        startRow++;

                        ws.Cells[4, 1].Value = "Date From";// + response.ClaimBordxHeader.CedentName;
                        ws.Cells[4, 2].Value = response.ClaimBordxHeader.FromDate.ToString("dd-MMM-yyyy");
                        ws.Cells[4, 2, 4, mergeColumns].Merge = true;

                        startRow++;

                        ws.Cells[startRow, 1].Value = "Date To";// + response.ClaimBordxHeader.CedentName;
                        ws.Cells[startRow, 2].Value = response.ClaimBordxHeader.ToDate.ToString("dd-MMM-yyyy");
                        ws.Cells[startRow, 2, startRow, mergeColumns].Merge = true;

                        startRow++;

                        ws.Cells[startRow, 1].Value = "Claim Details";
                        ws.Cells[startRow, 1, startRow, mergeColumns].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells[startRow, 1, startRow, mergeColumns].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells[startRow, 1, startRow, mergeColumns].Style.Fill.BackgroundColor.SetColor(Color.Gray);
                        ws.Cells[startRow, 1, startRow, mergeColumns].Style.Border.Top.Style = ExcelBorderStyle.Medium;
                        ws.Cells[startRow, 1, startRow, mergeColumns].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                        ws.Cells[startRow, 1, startRow, mergeColumns].Merge = true;

                        ws.Cells[1, 1, startRow, 1].Style.Font.Bold = true;
                        ws.Cells[1, 1, startRow, mergeColumns].Style.Border.BorderAround(ExcelBorderStyle.None);

                        startRow++;

                        ws.Cells[startRow, 1].Value = "S. No";
                        ws.Cells[startRow, 1, (startRow + 1), 1].Merge = true;
                        ws.Cells[startRow, 1, (startRow + 1), 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells[startRow, 1, (startRow + 1), 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);


                        ws.Cells[startRow, 2].Value = "Dealer Information";
                        ws.Cells[startRow, 2, startRow, 3].Merge = true;
                        ws.Cells[startRow, 2, startRow, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        ws.Cells[startRow, 4].Value = "Vehicle Information";
                        ws.Cells[startRow, 4, startRow, 9].Merge = true;
                        ws.Cells[startRow, 4, startRow, 9].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        //ws.Cells[startRow, 11].Value = "Dealer Information";
                        ws.Cells[startRow, 10, startRow, 12].Merge = true;

                        ws.Cells[startRow, 13].Value = "Fault Information";
                        ws.Cells[startRow, 13, startRow, 25].Merge = true;
                        ws.Cells[startRow, 13, startRow, 25].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        ws.Cells[startRow, 26].Value = "Amount";
                        ws.Cells[startRow, 26, startRow, 29].Merge = true;
                        ws.Cells[startRow, 26, startRow, 29].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        //ws.Cells[startRow, 31].Value = "Vehicle Information";
                        ws.Cells[startRow, 31, startRow, 34].Merge = true;

                        //ws.Cells[startRow, 11].Value = "Dealer Information";
                        ws.Cells[startRow, 35, startRow, 41].Merge = true;

                        ws.Cells[startRow, 1, startRow, mergeColumns].Style.Font.Bold = true;
                        ws.Cells[startRow, 1, startRow, mergeColumns].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                        startRow++;

                        //headers
                        ws.Cells[startRow, 2].Value = "Dealer";
                        ws.Cells[startRow, 3].Value = "Bordx Month & Year";
                        ws.Cells[startRow, 4].Value = "UWYear";
                        ws.Cells[startRow, 5].Value = "Type Policy";
                        ws.Cells[startRow, 6].Value = "Policy No";
                        ws.Cells[startRow, 7].Value = "Make";
                        ws.Cells[startRow, 8].Value = "Model";
                        ws.Cells[startRow, 9].Value = " Engine Size";
                        ws.Cells[startRow, 10].Value = "Chassis No";
                        ws.Cells[startRow, 11].Value = "Claim No";
                        ws.Cells[startRow, 12].Value = "Service Center";
                        ws.Cells[startRow, 13].Value = "Location";
                        ws.Cells[startRow, 14].Value = "Mileage";
                        ws.Cells[startRow, 15].Value = "Letter";
                        ws.Cells[startRow, 16].Value = "Number";
                        ws.Cells[startRow, 17].Value = "Fault Code";
                        ws.Cells[startRow, 18].Value = "Failed Component";
                        ws.Cells[startRow, 19].Value = "Cause Of Failure";
                        ws.Cells[startRow, 20].Value = "Claim Status";
                        ws.Cells[startRow, 21].Value = "Sorting";
                        ws.Cells[startRow, 22].Value = "Labour";
                        ws.Cells[startRow, 23].Value = "Part";
                        ws.Cells[startRow, 24].Value = "Auth.Amt";
                        ws.Cells[startRow, 25].Value = "Variance";
                        ws.Cells[startRow, 26].Value = "GoodWill";
                        ws.Cells[startRow, 27].Value = "Authorized";
                        ws.Cells[startRow, 28].Value = "Inprogress";
                        ws.Cells[startRow, 29].Value = "Paid";
                        ws.Cells[startRow, 30].Value = "Over 180";
                        ws.Cells[startRow, 31].Value = "Invoice Date";
                        ws.Cells[startRow, 32].Value = "Date Claim Paid";
                        ws.Cells[startRow, 33].Value = "Total Claim";
                        ws.Cells[startRow, 34].Value = "Total Claim To Reinsurance";
                        ws.Cells[startRow, 35].Value = "Under Writer";
                        ws.Cells[startRow, 36].Value = "Failure Date";
                        ws.Cells[startRow, 37].Value = "Claim Date";
                        ws.Cells[startRow, 38].Value = "Manufacturer Warranty Cut off Kms";
                        ws.Cells[startRow, 39].Value = "ExtWarranty Expiry Km";
                        ws.Cells[startRow, 40].Value = "Policy Start Date";
                        ws.Cells[startRow, 41].Value = "Policy Expiry Date";


                        ws.Cells[startRow, 1, startRow, mergeColumns].Style.Font.Bold = true;

                        for (int c = 2; c <= mergeColumns; c++) { ws.Cells[startRow, 1, startRow, c].Style.Border.BorderAround(ExcelBorderStyle.Thin); }

                        startRow++;

                        ws.View.FreezePanes(startRow, 1);

                        var normalClaims = dataList.Where(a => a.endosed ==0);
                        var endosedClaims = dataList.Where(a => a.endosed == 1);

                        var policyList = normalClaims.GroupBy(a => a.PolicyNo).Select(a => new { a.Key });

                        var endosedClaimList = endosedClaims.GroupBy(a => a.PolicyNo).Select(a => new { a.Key });

                        int serialNo = 0;
                        int subSerilNo = 0;


                        foreach (var policyNo in policyList)
                        {
                            serialNo++;

                            var data = normalClaims.Where(a => a.PolicyNo == policyNo.Key).ToList();

                            ws.Cells[startRow, 1].Value = serialNo.ToString();
                            ws.Cells[startRow, 2].Value = data[0].Dealer;
                            ws.Cells[startRow, 3].Value = data[0].BordxMonthYear;
                            ws.Cells[startRow, 4].Value = data[0].UWYear;
                            ws.Cells[startRow, 5].Value = data[0].TypePolicy;
                            ws.Cells[startRow, 6].Value = data[0].PolicyNo;
                            ws.Cells[startRow, 7].Value = data[0].Make;
                            ws.Cells[startRow, 8].Value = data[0].Model;
                            ws.Cells[startRow, 9].Value = data[0].EngineSize;
                            ws.Cells[startRow, 10].Value = data[0].ChassisNo;
                            ws.Cells[startRow, 11].Value = data[0].ClaimNo;
                            ws.Cells[startRow, 12].Value = data[0].ServiceCenter;
                            ws.Cells[startRow, 13].Value = data[0].Location;
                            ws.Cells[startRow, 14].Value = data[0].Mileage;
                            ws.Cells[startRow, 15].Value = "-";//data[0].Letter;
                            ws.Cells[startRow, 16].Value = "-";// data[0].Number;
                            ws.Cells[startRow, 17].Value = "-";//data[0].FaultCode;
                            ws.Cells[startRow, 18].Value = "-";//data[0].FailedComponent;
                            ws.Cells[startRow, 19].Value = "-";//data[0].CauseOfFailure;
                            ws.Cells[startRow, 20].Value = data[0].ClaimStatus;
                            ws.Cells[startRow, 21].Value = "Claim Plus";
                            ws.Cells[startRow, 22].Style.Numberformat.Format = "#,##0.00";
                            ws.Cells[startRow, 22].Value = data.Sum(a => a.LocalLabour).Value;
                            ws.Cells[startRow, 23].Style.Numberformat.Format = "#,##0.00";
                            ws.Cells[startRow, 23].Value = data.Sum(a => a.LocalPart).Value;
                            ws.Cells[startRow, 24].Style.Numberformat.Format = "#,##0.00";
                            ws.Cells[startRow, 24].Value = data.Sum(a => a.LocalAuthAmt).Value;//data[0].AuthAmt;
                            ws.Cells[startRow, 25].Style.Numberformat.Format = "#,##0.00";
                            ws.Cells[startRow, 25].Value = data.Sum(a => a.LocalVariance).Value;//data[0].Variance;
                            ws.Cells[startRow, 26].Style.Numberformat.Format = "#,##0.00";
                            ws.Cells[startRow, 26].Value = data.Sum(a => a.LocalGapGoodWill).Value;//data[0].GapGoodWill;
                            ws.Cells[startRow, 27].Style.Numberformat.Format = "#,##0.00";
                            ws.Cells[startRow, 27].Value = data.Sum(a => a.LocalAuthorized).Value;//data[0].Authorized;
                            ws.Cells[startRow, 28].Style.Numberformat.Format = "#,##0.00";
                            ws.Cells[startRow, 28].Value = data.Sum(a => a.LocalInprogress).Value;//data[0].Inprogress;
                            ws.Cells[startRow, 29].Style.Numberformat.Format = "#,##0.00";
                            ws.Cells[startRow, 29].Value = data.Sum(a => a.LocalPaid).Value;// data[0].Paid;
                            ws.Cells[startRow, 30].Style.Numberformat.Format = "#,##0.00";
                            ws.Cells[startRow, 30].Value = data[0].LocalOver180;
                            ws.Cells[startRow, 31].Value = (data[0].InvoiceDate != null) ? ((DateTime)data[0].InvoiceDate).ToString("dd-MMM-yyyy") : ""; //data[0].InvoiceDate;// (data[0].InvoiceDate != null) ? ((DateTime)data[0].InvoiceDate).ToString("dd-MMM-yyyy") : ""; //data[0].InvoiceDate;
                            ws.Cells[startRow, 32].Value = data[0].DateClaimPaid;
                            ws.Cells[startRow, 33].Style.Numberformat.Format = "#,##0.00";
                            ws.Cells[startRow, 33].Value = data.Sum(a => a.LocalTotalClaim).Value;//data[0].TotalClaim;
                            ws.Cells[startRow, 34].Style.Numberformat.Format = "#,##0.00";
                            ws.Cells[startRow, 34].Value = data.Sum(a => a.LocalTotalClaimToReinsurance).Value;//data[0].TotalClaimToReinsurance;
                            ws.Cells[startRow, 35].Value = data[0].UnderWriter;
                            ws.Cells[startRow, 36].Value = (data[0].FailureDate != null) ? ((DateTime)data[0].FailureDate).ToString("dd-MMM-yyyy") : "";//data[0].FailureDate;
                            ws.Cells[startRow, 37].Value = (data[0].ClaimDate != null) ? ((DateTime)data[0].ClaimDate).ToString("dd-MMM-yyyy") : "";
                            ws.Cells[startRow, 38].Value = data[0].ManufacturerWarrantyCutoffKms;
                            ws.Cells[startRow, 39].Value = data[0].ExtWarrantyExpiryKm;
                            ws.Cells[startRow, 40].Value = data[0].PolicyStartDate.ToString("dd-MMM-yyyy");
                            ws.Cells[startRow, 41].Value = data[0].PolicyExpiryDate.ToString("dd-MMM-yyyy");


                            ws.Cells[startRow, 41].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                            startRow++;
                            subSerilNo = 0;



                            foreach (var detail in data)
                            {
                                subSerilNo++;
                                ws.Cells[startRow, 1].Value = serialNo.ToString() + '-' + subSerilNo.ToString();
                                //ws.Cells[startRow, 2].Value = detail.Dealer;
                                //ws.Cells[startRow, 3].Value = detail.BordxMonthYear;
                                //ws.Cells[startRow, 4].Value = detail.UWYear;
                                //ws.Cells[startRow, 5].Value = detail.TypePolicy;
                                //ws.Cells[startRow, 6].Value = detail.PolicyNo;
                                //ws.Cells[startRow, 7].Value = detail.Make;
                                //ws.Cells[startRow, 8].Value = detail.Model;
                                //ws.Cells[startRow, 9].Value = detail.EngineSize;
                                //ws.Cells[startRow, 10].Value = detail.ChassisNo;
                                //ws.Cells[startRow, 11].Value = detail.ClaimNo;
                                //ws.Cells[startRow, 12].Value = detail.ServiceCenter;
                                //ws.Cells[startRow, 13].Value = detail.Location;
                                //ws.Cells[startRow, 14].Value = detail.Mileage;
                                ws.Cells[startRow, 15].Value = detail.Letter;
                                ws.Cells[startRow, 16].Value = detail.Number;
                                ws.Cells[startRow, 17].Value = detail.FaultCode;
                                ws.Cells[startRow, 18].Value = detail.FailedComponent;
                                ws.Cells[startRow, 19].Value = detail.CauseOfFailure;
                                //ws.Cells[startRow, 20].Value = detail.ClaimStatus;
                                ws.Cells[startRow, 21].Value = detail.Sorting;
                                ws.Cells[startRow, 22].Style.Numberformat.Format = "#,##0.00";
                                ws.Cells[startRow, 22].Value = detail.LocalLabour;
                                ws.Cells[startRow, 23].Style.Numberformat.Format = "#,##0.00";
                                ws.Cells[startRow, 23].Value = detail.LocalPart;
                                ws.Cells[startRow, 24].Style.Numberformat.Format = "#,##0.00";
                                ws.Cells[startRow, 24].Value = detail.LocalAuthAmt;
                                ws.Cells[startRow, 25].Style.Numberformat.Format = "#,##0.00";
                                ws.Cells[startRow, 25].Value = detail.LocalVariance;
                                ws.Cells[startRow, 26].Style.Numberformat.Format = "#,##0.00";
                                ws.Cells[startRow, 26].Value = detail.LocalGapGoodWill;
                                ws.Cells[startRow, 27].Style.Numberformat.Format = "#,##0.00";
                                ws.Cells[startRow, 27].Value = detail.LocalAuthorized;
                                ws.Cells[startRow, 28].Style.Numberformat.Format = "#,##0.00";
                                ws.Cells[startRow, 28].Value = detail.LocalInprogress;
                                ws.Cells[startRow, 29].Style.Numberformat.Format = "#,##0.00";
                                ws.Cells[startRow, 29].Value = detail.LocalPaid;
                                ws.Cells[startRow, 30].Style.Numberformat.Format = "#,##0.00";
                                ws.Cells[startRow, 30].Value = detail.LocalOver180;
                                ws.Cells[startRow, 31].Value = "-";// detail.InvoiceDate;
                                ws.Cells[startRow, 32].Value = "-";//detail.DateClaimPaid;
                                ws.Cells[startRow, 33].Style.Numberformat.Format = "#,##0.00";
                                ws.Cells[startRow, 33].Value = detail.LocalTotalClaim;
                                ws.Cells[startRow, 34].Style.Numberformat.Format = "#,##0.00";
                                ws.Cells[startRow, 34].Value = detail.LocalTotalClaimToReinsurance;
                                //ws.Cells[startRow, 35].Value = detail.UnderWriter;
                                //ws.Cells[startRow, 36].Value = detail.FailureDate;
                                //ws.Cells[startRow, 37].Value = detail.ClaimDate;
                                //ws.Cells[startRow, 38].Value = detail.ManufacturerWarrantyCutoffKms;
                                //ws.Cells[startRow, 39].Value = detail.ExtWarrantyExpiryKm;
                                ws.Cells[startRow, 40].Value = "-";//detail.PolicyStartDate;
                                ws.Cells[startRow, 41].Value = "-";//detail.PolicyExpiryDate;

                                ws.Cells[startRow, 1, startRow, mergeColumns].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                ws.Cells[startRow, 1, startRow, mergeColumns].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(242, 242, 242));

                                ws.Cells[startRow, 41].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                                startRow++;
                            }
                        }

                        ws.Cells[startRow, 1, startRow, mergeColumns].Style.Border.Top.Style = ExcelBorderStyle.Thin;

                        startRow++;

                        ws.Cells[startRow, 21].Value = "TOTAL";
                        ws.Cells[startRow, 21, startRow, 21].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        ws.Cells[startRow, 21, startRow, 21].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        ws.Cells[startRow, 22].Style.Numberformat.Format = "#,##0.00";
                        ws.Cells[startRow, 22].Value = normalClaims.Sum(a => a.LocalLabour);
                        ws.Cells[startRow, 22, startRow, 22].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                        ws.Cells[startRow, 23].Style.Numberformat.Format = "#,##0.00";
                        ws.Cells[startRow, 23].Value = normalClaims.Sum(a => a.LocalPart);
                        ws.Cells[startRow, 23, startRow, 23].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                        ws.Cells[startRow, 24].Style.Numberformat.Format = "#,##0.00";
                        ws.Cells[startRow, 24].Value = normalClaims.Sum(a => a.LocalAuthAmt);
                        ws.Cells[startRow, 24, startRow, 24].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                        ws.Cells[startRow, 25].Style.Numberformat.Format = "#,##0.00";
                        ws.Cells[startRow, 25].Value = normalClaims.Sum(a => a.LocalVariance);
                        ws.Cells[startRow, 25, startRow, 25].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                        ws.Cells[startRow, 27].Style.Numberformat.Format = "#,##0.00";
                        ws.Cells[startRow, 27].Value = normalClaims.Sum(a => a.LocalAuthorized);
                        ws.Cells[startRow, 27, startRow, 27].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                        ws.Cells[startRow, 28].Style.Numberformat.Format = "#,##0.00";
                        ws.Cells[startRow, 28].Value = normalClaims.Sum(a => a.LocalInprogress);
                        ws.Cells[startRow, 28, startRow, 28].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                        ws.Cells[startRow, 29].Style.Numberformat.Format = "#,##0.00";
                        ws.Cells[startRow, 29].Value = normalClaims.Sum(a => a.LocalPaid);
                        ws.Cells[startRow, 29, startRow, 29].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                        ws.Cells[startRow, 30, startRow, 30].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                        ws.Cells[startRow, 1, startRow, mergeColumns].Style.Font.Bold = true;

                        ws.Cells[startRow, 21, startRow, 30].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells[startRow, 21, startRow, 30].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(191, 191, 191));//(Color.SlateGray);

                        startRow++;

                        startRow++;

                        ws.Cells[startRow, 21].Value = "Claim Count";
                        ws.Cells[startRow, 21, startRow, 21].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        ws.Cells[startRow, 21, startRow, 21].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;


                        var countList = normalClaims.GroupBy(a => a.PolicyNo).Select(a => new { Authorized = a.Sum(s => s.Authorized), Inprogress = a.Sum(s => s.Inprogress), Paid = a.Sum(s => s.Paid) }).ToList();

                        ws.Cells[startRow, 27].Value = countList.Count(a => a.Authorized > 0);
                        ws.Cells[startRow, 27, startRow, 27].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                        ws.Cells[startRow, 28].Value = countList.Count(a => a.Inprogress > 0);//dataList.Count(a => a.Inprogress > 0);
                        ws.Cells[startRow, 28, startRow, 28].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                        ws.Cells[startRow, 29].Value = countList.Count(a => a.Paid > 0);//dataList.Count(a => a.Paid > 0);
                        ws.Cells[startRow, 29, startRow, 29].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                        ws.Cells[startRow, 30, startRow, 30].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                        ws.Cells[startRow, 1, startRow, mergeColumns].Style.Font.Bold = true;

                        ws.Cells[startRow, 21, startRow, 30].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells[startRow, 21, startRow, 30].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(191, 191, 191));//(Color.SlateGray);

                        startRow++;

                        startRow++;

                        ws.Cells[startRow, 21].Value = "US$ Value";
                        ws.Cells[startRow, 21, startRow, 21].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        ws.Cells[startRow, 21, startRow, 21].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        ws.Cells[startRow, 27].Style.Numberformat.Format = "#,##0.00";
                        ws.Cells[startRow, 27].Value = normalClaims.Sum(a => a.Authorized);
                        ws.Cells[startRow, 27, startRow, 27].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                        ws.Cells[startRow, 28].Value = "-";
                        ws.Cells[startRow, 28, startRow, 28].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                        ws.Cells[startRow, 29].Style.Numberformat.Format = "#,##0.00";
                        ws.Cells[startRow, 29].Value = normalClaims.Sum(a => a.Paid);
                        ws.Cells[startRow, 29, startRow, 29].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                        ws.Cells[startRow, 30].Value = "-";
                        ws.Cells[startRow, 30, startRow, 30].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                        ws.Cells[startRow, 1, startRow, mergeColumns].Style.Font.Bold = true;

                        ws.Cells[startRow, 21, startRow, 30].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells[startRow, 21, startRow, 30].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(191, 191, 191));//(Color.SlateGray);



                        if (endosedClaimList.Count() > 0) {

                        startRow++;
                        var cell = ws.Cells[startRow, 1];
                        cell.Value = "Endorsement Claims";

                      //  ws.Cells[startRow, 1].Style.Fill.BackgroundColor.SetColor(Color.Red);
                        ws.Cells[startRow, 1, startRow, 1 + 2].Merge = true;
                        ws.Cells[startRow, 1, startRow, 1 + 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells[startRow, 1, startRow, 1 + 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
                        ws.Cells[startRow, 1, startRow, 1 + 2].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 102, 102));
                        cell.AutoFitColumns();
                        // endosement
                        startRow++;

                        foreach (var policyNo in endosedClaimList)
                        {
                            serialNo++;

                            var data = endosedClaims.Where(a => a.PolicyNo == policyNo.Key).ToList();

                            ws.Cells[startRow, 1].Value = serialNo.ToString();
                            ws.Cells[startRow, 2].Value = data[0].Dealer;
                            ws.Cells[startRow, 3].Value = data[0].BordxMonthYear;
                            ws.Cells[startRow, 4].Value = data[0].UWYear;
                            ws.Cells[startRow, 5].Value = data[0].TypePolicy;
                            ws.Cells[startRow, 6].Value = data[0].PolicyNo;
                            ws.Cells[startRow, 7].Value = data[0].Make;
                            ws.Cells[startRow, 8].Value = data[0].Model;
                            ws.Cells[startRow, 9].Value = data[0].EngineSize;
                            ws.Cells[startRow, 10].Value = data[0].ChassisNo;
                            ws.Cells[startRow, 11].Value = data[0].ClaimNo;
                            ws.Cells[startRow, 12].Value = data[0].ServiceCenter;
                            ws.Cells[startRow, 13].Value = data[0].Location;
                            ws.Cells[startRow, 14].Value = data[0].Mileage;
                            ws.Cells[startRow, 15].Value = "-";//data[0].Letter;
                            ws.Cells[startRow, 16].Value = "-";// data[0].Number;
                            ws.Cells[startRow, 17].Value = "-";//data[0].FaultCode;
                            ws.Cells[startRow, 18].Value = "-";//data[0].FailedComponent;
                            ws.Cells[startRow, 19].Value = "-";//data[0].CauseOfFailure;
                            ws.Cells[startRow, 20].Value = data[0].ClaimStatus;
                            ws.Cells[startRow, 21].Value = "Claim Plus";
                            ws.Cells[startRow, 22].Style.Numberformat.Format = "#,##0.00";
                            ws.Cells[startRow, 22].Value = data.Sum(a => a.LocalLabour).Value;
                            ws.Cells[startRow, 23].Style.Numberformat.Format = "#,##0.00";
                            ws.Cells[startRow, 23].Value = data.Sum(a => a.LocalPart).Value;
                            ws.Cells[startRow, 24].Style.Numberformat.Format = "#,##0.00";
                            ws.Cells[startRow, 24].Value = data.Sum(a => a.LocalAuthAmt).Value;//data[0].AuthAmt;
                            ws.Cells[startRow, 25].Style.Numberformat.Format = "#,##0.00";
                            ws.Cells[startRow, 25].Value = data.Sum(a => a.LocalVariance).Value;//data[0].Variance;
                            ws.Cells[startRow, 26].Style.Numberformat.Format = "#,##0.00";
                            ws.Cells[startRow, 26].Value = data.Sum(a => a.LocalGapGoodWill).Value;//data[0].GapGoodWill;
                            ws.Cells[startRow, 27].Style.Numberformat.Format = "#,##0.00";
                            ws.Cells[startRow, 27].Value = data.Sum(a => a.LocalAuthorized).Value;//data[0].Authorized;
                            ws.Cells[startRow, 28].Style.Numberformat.Format = "#,##0.00";
                            ws.Cells[startRow, 28].Value = data.Sum(a => a.LocalInprogress).Value;//data[0].Inprogress;
                            ws.Cells[startRow, 29].Style.Numberformat.Format = "#,##0.00";
                            ws.Cells[startRow, 29].Value = data.Sum(a => a.LocalPaid).Value;// data[0].Paid;
                            ws.Cells[startRow, 30].Style.Numberformat.Format = "#,##0.00";
                            ws.Cells[startRow, 30].Value = data[0].LocalOver180;
                            ws.Cells[startRow, 31].Value = (data[0].InvoiceDate != null) ? ((DateTime)data[0].InvoiceDate).ToString("dd-MMM-yyyy") : ""; //data[0].InvoiceDate;// (data[0].InvoiceDate != null) ? ((DateTime)data[0].InvoiceDate).ToString("dd-MMM-yyyy") : ""; //data[0].InvoiceDate;
                            ws.Cells[startRow, 32].Value = data[0].DateClaimPaid;
                            ws.Cells[startRow, 33].Style.Numberformat.Format = "#,##0.00";
                            ws.Cells[startRow, 33].Value = data.Sum(a => a.LocalTotalClaim).Value;//data[0].TotalClaim;
                            ws.Cells[startRow, 34].Style.Numberformat.Format = "#,##0.00";
                            ws.Cells[startRow, 34].Value = data.Sum(a => a.LocalTotalClaimToReinsurance).Value;//data[0].TotalClaimToReinsurance;
                            ws.Cells[startRow, 35].Value = data[0].UnderWriter;
                            ws.Cells[startRow, 36].Value = (data[0].FailureDate != null) ? ((DateTime)data[0].FailureDate).ToString("dd-MMM-yyyy") : "";//data[0].FailureDate;
                            ws.Cells[startRow, 37].Value = (data[0].ClaimDate != null) ? ((DateTime)data[0].ClaimDate).ToString("dd-MMM-yyyy") : "";
                            ws.Cells[startRow, 38].Value = data[0].ManufacturerWarrantyCutoffKms;
                            ws.Cells[startRow, 39].Value = data[0].ExtWarrantyExpiryKm;
                            ws.Cells[startRow, 40].Value = data[0].PolicyStartDate.ToString("dd-MMM-yyyy");
                            ws.Cells[startRow, 41].Value = data[0].PolicyExpiryDate.ToString("dd-MMM-yyyy");


                            ws.Cells[startRow, 41].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                            startRow++;
                            subSerilNo = 0;



                            foreach (var detail in data)
                            {
                                subSerilNo++;
                                ws.Cells[startRow, 1].Value = serialNo.ToString() + '-' + subSerilNo.ToString();
                                //ws.Cells[startRow, 2].Value = detail.Dealer;
                                //ws.Cells[startRow, 3].Value = detail.BordxMonthYear;
                                //ws.Cells[startRow, 4].Value = detail.UWYear;
                                //ws.Cells[startRow, 5].Value = detail.TypePolicy;
                                //ws.Cells[startRow, 6].Value = detail.PolicyNo;
                                //ws.Cells[startRow, 7].Value = detail.Make;
                                //ws.Cells[startRow, 8].Value = detail.Model;
                                //ws.Cells[startRow, 9].Value = detail.EngineSize;
                                //ws.Cells[startRow, 10].Value = detail.ChassisNo;
                                //ws.Cells[startRow, 11].Value = detail.ClaimNo;
                                //ws.Cells[startRow, 12].Value = detail.ServiceCenter;
                                //ws.Cells[startRow, 13].Value = detail.Location;
                                //ws.Cells[startRow, 14].Value = detail.Mileage;
                                ws.Cells[startRow, 15].Value = detail.Letter;
                                ws.Cells[startRow, 16].Value = detail.Number;
                                ws.Cells[startRow, 17].Value = detail.FaultCode;
                                ws.Cells[startRow, 18].Value = detail.FailedComponent;
                                ws.Cells[startRow, 19].Value = detail.CauseOfFailure;
                                //ws.Cells[startRow, 20].Value = detail.ClaimStatus;
                                ws.Cells[startRow, 21].Value = detail.Sorting;
                                ws.Cells[startRow, 22].Style.Numberformat.Format = "#,##0.00";
                                ws.Cells[startRow, 22].Value = detail.LocalLabour;
                                ws.Cells[startRow, 23].Style.Numberformat.Format = "#,##0.00";
                                ws.Cells[startRow, 23].Value = detail.LocalPart;
                                ws.Cells[startRow, 24].Style.Numberformat.Format = "#,##0.00";
                                ws.Cells[startRow, 24].Value = detail.LocalAuthAmt;
                                ws.Cells[startRow, 25].Style.Numberformat.Format = "#,##0.00";
                                ws.Cells[startRow, 25].Value = detail.LocalVariance;
                                ws.Cells[startRow, 26].Style.Numberformat.Format = "#,##0.00";
                                ws.Cells[startRow, 26].Value = detail.LocalGapGoodWill;
                                ws.Cells[startRow, 27].Style.Numberformat.Format = "#,##0.00";
                                ws.Cells[startRow, 27].Value = detail.LocalAuthorized;
                                ws.Cells[startRow, 28].Style.Numberformat.Format = "#,##0.00";
                                ws.Cells[startRow, 28].Value = detail.LocalInprogress;
                                ws.Cells[startRow, 29].Style.Numberformat.Format = "#,##0.00";
                                ws.Cells[startRow, 29].Value = detail.LocalPaid;
                                ws.Cells[startRow, 30].Style.Numberformat.Format = "#,##0.00";
                                ws.Cells[startRow, 30].Value = detail.LocalOver180;
                                ws.Cells[startRow, 31].Value = "-";// detail.InvoiceDate;
                                ws.Cells[startRow, 32].Value = "-";//detail.DateClaimPaid;
                                ws.Cells[startRow, 33].Style.Numberformat.Format = "#,##0.00";
                                ws.Cells[startRow, 33].Value = detail.LocalTotalClaim;
                                ws.Cells[startRow, 34].Style.Numberformat.Format = "#,##0.00";
                                ws.Cells[startRow, 34].Value = detail.LocalTotalClaimToReinsurance;
                                //ws.Cells[startRow, 35].Value = detail.UnderWriter;
                                //ws.Cells[startRow, 36].Value = detail.FailureDate;
                                //ws.Cells[startRow, 37].Value = detail.ClaimDate;
                                //ws.Cells[startRow, 38].Value = detail.ManufacturerWarrantyCutoffKms;
                                //ws.Cells[startRow, 39].Value = detail.ExtWarrantyExpiryKm;
                                ws.Cells[startRow, 40].Value = "-";//detail.PolicyStartDate;
                                ws.Cells[startRow, 41].Value = "-";//detail.PolicyExpiryDate;

                                ws.Cells[startRow, 1, startRow, mergeColumns].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                ws.Cells[startRow, 1, startRow, mergeColumns].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(242, 242, 242));

                                ws.Cells[startRow, 41].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                                startRow++;
                            }
                        }

                        ws.Cells[startRow, 1, startRow, mergeColumns].Style.Border.Top.Style = ExcelBorderStyle.Thin;

                        startRow++;

                        ws.Cells[startRow, 21].Value = "TOTAL";
                        ws.Cells[startRow, 21, startRow, 21].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        ws.Cells[startRow, 21, startRow, 21].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        ws.Cells[startRow, 22].Style.Numberformat.Format = "#,##0.00";
                        ws.Cells[startRow, 22].Value = endosedClaims.Sum(a => a.LocalLabour);
                        ws.Cells[startRow, 22, startRow, 22].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                        ws.Cells[startRow, 23].Style.Numberformat.Format = "#,##0.00";
                        ws.Cells[startRow, 23].Value = endosedClaims.Sum(a => a.LocalPart);
                        ws.Cells[startRow, 23, startRow, 23].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                        ws.Cells[startRow, 24].Style.Numberformat.Format = "#,##0.00";
                        ws.Cells[startRow, 24].Value = endosedClaims.Sum(a => a.LocalAuthAmt);
                        ws.Cells[startRow, 24, startRow, 24].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                        ws.Cells[startRow, 25].Style.Numberformat.Format = "#,##0.00";
                        ws.Cells[startRow, 25].Value = endosedClaims.Sum(a => a.LocalVariance);
                        ws.Cells[startRow, 25, startRow, 25].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                        ws.Cells[startRow, 27].Style.Numberformat.Format = "#,##0.00";
                        ws.Cells[startRow, 27].Value = endosedClaims.Sum(a => a.LocalAuthorized);
                        ws.Cells[startRow, 27, startRow, 27].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                        ws.Cells[startRow, 28].Style.Numberformat.Format = "#,##0.00";
                        ws.Cells[startRow, 28].Value = endosedClaims.Sum(a => a.LocalInprogress);
                        ws.Cells[startRow, 28, startRow, 28].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                        ws.Cells[startRow, 29].Style.Numberformat.Format = "#,##0.00";
                        ws.Cells[startRow, 29].Value = endosedClaims.Sum(a => a.LocalPaid);
                        ws.Cells[startRow, 29, startRow, 29].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                        ws.Cells[startRow, 30, startRow, 30].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                        ws.Cells[startRow, 1, startRow, mergeColumns].Style.Font.Bold = true;

                        ws.Cells[startRow, 21, startRow, 30].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells[startRow, 21, startRow, 30].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(191, 191, 191));//(Color.SlateGray);

                        startRow++;

                        startRow++;

                        ws.Cells[startRow, 21].Value = "Claim Count";
                        ws.Cells[startRow, 21, startRow, 21].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        ws.Cells[startRow, 21, startRow, 21].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;


                        var countLists = endosedClaims.GroupBy(a => a.PolicyNo).Select(a => new { Authorized = a.Sum(s => s.Authorized), Inprogress = a.Sum(s => s.Inprogress), Paid = a.Sum(s => s.Paid) }).ToList();

                        ws.Cells[startRow, 27].Value = countLists.Count(a => a.Authorized > 0);
                        ws.Cells[startRow, 27, startRow, 27].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                        ws.Cells[startRow, 28].Value = countLists.Count(a => a.Inprogress > 0);//dataList.Count(a => a.Inprogress > 0);
                        ws.Cells[startRow, 28, startRow, 28].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                        ws.Cells[startRow, 29].Value = countLists.Count(a => a.Paid > 0);//dataList.Count(a => a.Paid > 0);
                        ws.Cells[startRow, 29, startRow, 29].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                        ws.Cells[startRow, 30, startRow, 30].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                        ws.Cells[startRow, 1, startRow, mergeColumns].Style.Font.Bold = true;

                        ws.Cells[startRow, 21, startRow, 30].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells[startRow, 21, startRow, 30].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(191, 191, 191));//(Color.SlateGray);

                        startRow++;

                        startRow++;

                        ws.Cells[startRow, 21].Value = "US$ Value";
                        ws.Cells[startRow, 21, startRow, 21].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        ws.Cells[startRow, 21, startRow, 21].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        ws.Cells[startRow, 27].Style.Numberformat.Format = "#,##0.00";
                        ws.Cells[startRow, 27].Value = endosedClaims.Sum(a => a.Authorized);
                        ws.Cells[startRow, 27, startRow, 27].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                        ws.Cells[startRow, 28].Value = "-";
                        ws.Cells[startRow, 28, startRow, 28].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                        ws.Cells[startRow, 29].Style.Numberformat.Format = "#,##0.00";
                        ws.Cells[startRow, 29].Value = endosedClaims.Sum(a => a.Paid);
                        ws.Cells[startRow, 29, startRow, 29].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                        ws.Cells[startRow, 30].Value = "-";
                        ws.Cells[startRow, 30, startRow, 30].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                        ws.Cells[startRow, 1, startRow, mergeColumns].Style.Font.Bold = true;

                        ws.Cells[startRow, 21, startRow, 30].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells[startRow, 21, startRow, 30].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(191, 191, 191));//(Color.SlateGray);

                        }
                    }
                }


                Byte[] bin = p.GetAsByteArray(); ;
                return new MemoryStream(bin);
            }
        }

        internal Stream GenarateIncurdErningExcel(IncurredErningExportResponseDto response , String earnDate)
        {
            using (ExcelPackage p = new ExcelPackage())
            {
                List<string> ReferanceRows = new List<string>() { "currencyconversionrate" };

                //Here setting some document properties
                p.Workbook.Properties.Author = "Trivow Business Solutions";
                p.Workbook.Properties.Title = "TAS Lost Ratio Report";

                int worksheet = 1;
                if (response.LostRatioSummary != null)
                {
                    if (response.LostRatioSummary.Count() > 0)
                    {

                        //Create a sheet
                        p.Workbook.Worksheets.Add("Loss Ratio");
                        ExcelWorksheet ws = p.Workbook.Worksheets[worksheet];
                        ws.Cells.Style.Font.Size = 11; //Default font size for whole sheet
                        ws.Cells.Style.Font.Name = "Calibri"; //Default Font name for whole sheet
                        int mergeColumns = 8;
                        int startRow = 1;

                        //    decimal totalAmount = 0;
                        //    decimal toatalpaid = 0;
                        //    decimal outstanding = 0;
                        Image image;
                        try
                        {
                            byte[] bytes = Convert.FromBase64String(response.tpaLogo);
                            using (MemoryStream ms = new MemoryStream(bytes))
                            {
                                image = Image.FromStream(ms);
                                ws.Row(1).Height = image.Height * .75;
                                var picture = ws.Drawings.AddPicture("logo", image);
                                picture.SetPosition(0, 0);
                                ws.Cells[1, 1, 1, 2].Merge = true;
                            }
                        }
                        catch (Exception)
                        {
                            image = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"STANDARD\\assets\\images\\bg_2.png"));
                            ws.Row(1).Height = image.Height * .75;
                            var picture = ws.Drawings.AddPicture("logo", image);
                            picture.SetPosition(0, 0);
                            ws.Cells[1, 1, 1, 2].Merge = true;
                        }



                        ws.Cells[startRow, 3].Value = "Incurred To Earned (Loss Ratio)";
                        ws.Cells[startRow, 3, startRow, 6].Merge = true;
                        ws.Cells[startRow, 3, startRow, 6].Style.Font.Bold = true;
                        ws.Cells[startRow, 3, startRow, 6].Style.Font.Size = 13;
                        ws.Cells[startRow, 3, startRow, 6].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        ws.Cells[startRow, 3, startRow, 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        startRow++;
                        startRow++;
                        ws.Cells[startRow, 1].Value = "* All prices are in USD   ,  Earned Date : " + earnDate;
                        ws.Cells[startRow, 1, startRow, 5].Merge = true;
                        startRow++;
                        //Headers

                        ws.Cells[startRow, 1].Value = "Contract Info";
                        ws.Cells[startRow, 1, startRow, 5].Merge = true;
                        ws.Cells[startRow, 1, startRow, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells[startRow, 1, startRow, 5].Style.Font.Bold = true;
                        ws.Cells[startRow, 1, startRow, 5].Style.Fill.PatternType = ExcelFillStyle.Solid; ;
                        ws.Cells[startRow, 1, startRow, 5].Style.Fill.BackgroundColor.SetColor(Color.LightGray);


                        ws.Cells[startRow, 6].Value = "Policy & Premium";
                        ws.Cells[startRow, 6, startRow, 11].Merge = true;
                        ws.Cells[startRow, 6, startRow, 11].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells[startRow, 6, startRow, 11].Style.Font.Bold = true;
                        ws.Cells[startRow, 6, startRow, 11].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells[startRow, 6, startRow, 11].Style.Fill.BackgroundColor.SetColor(Color.LightPink);

                        ws.Cells[startRow, 12].Value = "Claims";
                        ws.Cells[startRow, 12, startRow, 23].Merge = true;
                        ws.Cells[startRow, 12, startRow, 23].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells[startRow, 12, startRow, 23].Style.Font.Bold = true;
                        ws.Cells[startRow, 12, startRow, 23].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells[startRow, 12, startRow, 23].Style.Fill.BackgroundColor.SetColor(Color.LightGreen);

                        ws.Cells[startRow, 24].Value = "";
                        ws.Cells[startRow, 24, startRow, 35].Merge = true;
                        ws.Cells[startRow, 24, startRow, 35].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells[startRow, 24, startRow, 35].Style.Font.Bold = true;
                        ws.Cells[startRow, 24, startRow, 35].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells[startRow, 24, startRow, 35].Style.Fill.BackgroundColor.SetColor(Color.LightBlue);

                        startRow++;

                        ws.Cells[startRow, 12].Value = "Reinsurer";
                        ws.Cells[startRow, 12, startRow, 15].Merge = true;
                        ws.Cells[startRow, 12, startRow, 15].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells[startRow, 12, startRow, 15].Style.Font.Bold = true;

                        ws.Cells[startRow, 16].Value = "Special Approvals";
                        ws.Cells[startRow, 16, startRow, 19].Merge = true;
                        ws.Cells[startRow, 16, startRow, 19].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells[startRow, 16, startRow, 19].Style.Font.Bold = true;

                        ws.Cells[startRow, 20].Value = "Total";
                        ws.Cells[startRow, 20, startRow, 23].Merge = true;
                        ws.Cells[startRow, 20, startRow, 23].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells[startRow, 20, startRow, 23].Style.Font.Bold = true;

                        ws.Cells[startRow, 24].Value = "Loss Ratio";
                        ws.Cells[startRow, 24, startRow, 29].Merge = true;
                        ws.Cells[startRow, 24, startRow, 29].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells[startRow, 24, startRow, 29].Style.Font.Bold = true;

                        startRow++;

                        ws.Row(startRow).Height = 34;
                        ws.Cells[startRow, 1].Value = "Country";
                        ws.Cells[startRow, 2].Value = "Dealer/ contract";
                        ws.Cells[startRow, 3].Value = "Contract Year";
                        ws.Cells[startRow, 4].Value = "Warranty Type";
                        ws.Cells[startRow, 5].Value = "Cover Type";
                        ws.Cells[startRow, 6].Value = "Policy Count";
                        ws.Column(7).Width = 15;
                        ws.Cells[startRow, 7].Value = "Gross Premium";
                        ws.Column(8).Width = 12.5;
                        ws.Cells[startRow, 8].Value = "Earned Gross Premium";
                        ws.Cells[startRow, 9].Value = "Net Premium";
                        ws.Column(10).Width = 11;
                        ws.Cells[startRow, 10].Value = "Earned Net Premium";
                        ws.Column(11).Width = 13.5;
                        ws.Cells[startRow, 11].Value = "Risk Completed ";
                        ws.Cells[startRow, 12].Value = "Paid Claims";
                        ws.Cells[startRow, 12, startRow, 13].Merge = true;
                        ws.Cells[startRow, 14].Value = "Paid + Reserved Claims";
                        ws.Cells[startRow, 14, startRow, 15].Merge = true;
                        ws.Cells[startRow, 16].Value = "Paid Claims";
                        ws.Cells[startRow, 16, startRow, 17].Merge = true;
                        ws.Cells[startRow, 18].Value = "Paid + Reserved Claims";
                        ws.Cells[startRow, 18, startRow, 19].Merge = true;
                        ws.Cells[startRow, 20].Value = "Paid Claims";
                        ws.Cells[startRow, 20, startRow, 21].Merge = true;
                        ws.Cells[startRow, 22].Value = "Paid + Reserved Claims";
                        ws.Cells[startRow, 22, startRow, 23].Merge = true;
                        ws.Cells[startRow, 22, startRow, 23].Style.Font.Bold = true;
                        ws.Cells[startRow, 24].Value = "RI Claims To NRP %";
                        ws.Column(25).Width = 14;
                        ws.Cells[startRow, 25].Value = "RI Claims To Earned NRP %";
                        ws.Column(26).Width = 11.5;
                        ws.Cells[startRow, 26].Value = "Total Claims To NRP %";
                        ws.Column(27).Width = 13.7;
                        ws.Cells[startRow, 27].Value = "Total Claims To Earned NRP %";
                        ws.Column(28).Width = 11.5;
                        ws.Cells[startRow, 28].Value = "Total Claims To GP %";
                        ws.Column(29).Width = 13.7;
                        ws.Cells[startRow, 29].Value = "Total Claims To Earned GP %";
                        ws.Cells[startRow, 30].Value = "Average NRP";
                        ws.Cells[startRow, 31].Value = "Average GP";
                        ws.Column(32).Width = 15;
                        ws.Cells[startRow, 32].Value = "Weighted Burn Cost To RI Claims";
                        ws.Column(33).Width = 17.5;
                        ws.Cells[startRow, 33].Value = "Weighted Burn Cost To Total Claims";
                        ws.Column(34).Width = 16.5;
                        ws.Cells[startRow, 34].Value = "Weighted Claim Frequency To RI %";
                        ws.Column(35).Width = 17.5;
                        ws.Cells[startRow, 35].Value = "Weighted Claim Frequency-Total %";
                        ws.Cells[startRow, 1, startRow, 35].Style.WrapText = true;
                        ws.Cells[startRow, 1, startRow, 35].Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        ws.Cells[startRow, 1, startRow, 35].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        startRow++;

                        ws.Cells[startRow, 12].Value = "Count";
                        ws.Cells[startRow, 13].Value = "Value";
                        ws.Cells[startRow, 14].Value = "Count";
                        ws.Cells[startRow, 15].Value = "Value";
                        ws.Cells[startRow, 16].Value = "Count";
                        ws.Cells[startRow, 17].Value = "Value";
                        ws.Cells[startRow, 18].Value = "Count";
                        ws.Cells[startRow, 19].Value = "Value";
                        ws.Cells[startRow, 20].Value = "Count";
                        ws.Cells[startRow, 21].Value = "Value";
                        ws.Cells[startRow, 22].Value = "Count";
                        ws.Cells[startRow, 23].Value = "Value";
                        ws.Cells[startRow, 22, startRow, 23].Style.Font.Bold = true;

                        startRow++;

                        foreach (var detail in response.LostRatioSummary)
                        {


                            decimal TotPaidReservedClaimsValue = (detail.ReInPaidReservedClaimsValue + detail.SAPaidReservedClaimsValue);
                            decimal EarnedGrossPremium = detail.GrossPremium * detail.RiskCompleted / 100M;
                            decimal EarnedNetPremium = detail.NetPremium * detail.RiskCompleted / 100M;

                            ws.Cells[startRow, 1].Value = detail.Country;
                            ws.Cells[startRow, 2].Value = detail.DealerContract;
                            ws.Cells[startRow, 3].Value = detail.ContractYear;
                            ws.Cells[startRow, 4].Value = detail.WarrantyType;
                            ws.Cells[startRow, 5].Value = detail.CoverType;
                            ws.Cells[startRow, 6].Value = detail.PolicyCount;//F7//
                            ws.Cells[startRow, 7].Style.Numberformat.Format = "#,##0.00";
                            ws.Cells[startRow, 7].Value = detail.GrossPremium;//G7//
                            ws.Cells[startRow, 8].Style.Numberformat.Format = "#,##0.00";
                            ws.Cells[startRow, 8].Value = detail.ErnedGrossPremium;//H7//
                            ws.Cells[startRow, 9].Style.Numberformat.Format = "#,##0.00";
                            ws.Cells[startRow, 9].Value = detail.NetPremium;//I7//
                            ws.Cells[startRow, 10].Style.Numberformat.Format = "#,##0.00";
                            ws.Cells[startRow, 10].Value = detail.ErnedNetPremium;//J7//
                            ws.Cells[startRow, 11].Style.Numberformat.Format = "##0.00%";
                            ws.Cells[startRow, 11].Value = detail.RiskCompleted;//K7//
                            ws.Cells[startRow, 12].Value = detail.ReInPaidClaimsCount;
                            ws.Cells[startRow, 13].Style.Numberformat.Format = "#,##0.00";
                            ws.Cells[startRow, 13].Value = detail.ReInPaidClaimsValue;
                            ws.Cells[startRow, 14].Value = detail.ReInPaidReservedClaimsCount;//N7//
                            ws.Cells[startRow, 15].Style.Numberformat.Format = "#,##0.00";
                            ws.Cells[startRow, 15].Value = detail.ReInPaidReservedClaimsValue;//O7//
                            ws.Cells[startRow, 16].Value = detail.SAPaidClaimsCount;
                            ws.Cells[startRow, 17].Style.Numberformat.Format = "#,##0.00";
                            ws.Cells[startRow, 17].Value = detail.SAPaidClaimsValue;
                            ws.Cells[startRow, 18].Value = detail.SAPaidReservedClaimsCount;
                            ws.Cells[startRow, 19].Style.Numberformat.Format = "#,##0.00";
                            ws.Cells[startRow, 19].Value = detail.SAPaidReservedClaimsValue;
                            ws.Cells[startRow, 20].Value = (detail.ReInPaidClaimsCount + detail.SAPaidClaimsCount);//detail.TotPaidClaimsCount;
                            ws.Cells[startRow, 21].Style.Numberformat.Format = "#,##0.00";
                            ws.Cells[startRow, 21].Value = (detail.ReInPaidClaimsValue + detail.SAPaidClaimsValue);//detail.TotPaidClaimsValue;
                            ws.Cells[startRow, 22].Value = (detail.ReInPaidReservedClaimsCount + detail.SAPaidReservedClaimsCount);//detail.TotPaidReservedClaimsCount;  //V7//
                            ws.Cells[startRow, 23, startRow, 35].Style.Numberformat.Format = "#,##0.00";
                            ws.Cells[startRow, 23].Value = (detail.ReInPaidReservedClaimsValue + detail.SAPaidReservedClaimsValue);//detail.TotPaidReservedClaimsValue; //W7//

                            ws.Cells[startRow, 24].Style.Numberformat.Format = "##0.00%";
                            ws.Cells[startRow, 24].Value =  detail.NetPremium != 0m ?  detail.ReInPaidReservedClaimsValue  /detail.NetPremium : 0;//detail.RIClaimsToNRP + "%";
                            ws.Cells[startRow, 25].Style.Numberformat.Format = "##0.00%";
                            ws.Cells[startRow, 25].Value = EarnedNetPremium != 0m ?  (detail.ReInPaidReservedClaimsValue /EarnedNetPremium)/100 : 0;//detail.RIClaimsToEarnedNRP + "%";
                            ws.Cells[startRow, 26].Style.Numberformat.Format = "##0.00%";
                            ws.Cells[startRow, 26].Value = detail.NetPremium != 0m ? TotPaidReservedClaimsValue / detail.NetPremium : 0;//detail.TotalClaimsToNRP + "%";
                            ws.Cells[startRow, 27].Style.Numberformat.Format = "##0.00%";
                            ws.Cells[startRow, 27].Value = EarnedNetPremium != 0m ? (TotPaidReservedClaimsValue / EarnedNetPremium)/100 : 0;//detail.TotalClaimsToEarnedNRP + "%";
                            ws.Cells[startRow, 28].Style.Numberformat.Format = "##0.00%";
                            ws.Cells[startRow, 28].Value = detail.GrossPremium != 0m ? TotPaidReservedClaimsValue / detail.GrossPremium : 0;//detail.TotalClaimsToGP + "%";
                            ws.Cells[startRow, 29].Style.Numberformat.Format = "##0.00%";
                            ws.Cells[startRow, 29].Value =EarnedGrossPremium != 0m ? (TotPaidReservedClaimsValue / EarnedGrossPremium) /100 : 0;//detail.TotalClaimsToEarnedGP + "%";
                            ws.Cells[startRow, 30].Value = detail.PolicyCount != 0 ? detail.NetPremium / detail.PolicyCount : 0;//detail.AverageNRP;
                            ws.Cells[startRow, 31].Value = detail.PolicyCount != 0 ? detail.GrossPremium / detail.PolicyCount : 0;//detail.AverageGP;
                            ws.Cells[startRow, 32].Value = (detail.PolicyCount == 0 || detail.RiskCompleted == 0m) ? 0m : detail.ReInPaidReservedClaimsValue / (detail.PolicyCount * (detail.RiskCompleted/100)) / 100;//detail.WeightedBurnCostToRIClaims;
                            ws.Cells[startRow, 33].Value = (detail.PolicyCount == 0 || detail.RiskCompleted == 0m) ? 0m : TotPaidReservedClaimsValue / (detail.PolicyCount * (detail.RiskCompleted  / 100))/100;//detail.WeightedBurnCosttoTotalClaims;
                            ws.Cells[startRow, 34].Style.Numberformat.Format = "##0.00%";
                            ws.Cells[startRow, 34].Value = (detail.PolicyCount == 0 || detail.RiskCompleted == 0m) ? 0m : detail.ReInPaidReservedClaimsCount / (detail.PolicyCount * (detail.RiskCompleted  / 100)) /100;//detail.WeightedClaimFrequencyToRI + "%";.ToString("0")
                            ws.Cells[startRow, 35].Style.Numberformat.Format = "##0.00%";
                            ws.Cells[startRow, 35].Value = (detail.PolicyCount == 0 || detail.RiskCompleted == 0m) ? 0m : (detail.ReInPaidReservedClaimsCount + detail.SAPaidReservedClaimsCount) / ((detail.PolicyCount * detail.RiskCompleted/100)) / 100;//detail.WeightedClaimFrequencyTotal + "%";

                            //toatalpaid = toatalpaid + detail.PaidAmount;
                            //totalAmount = totalAmount + detail.ClaimAmount;
                            //outstanding = outstanding + detail.Outstanding;

                            //for (int i = 1; i <= mergeColumns; i++) ws.Cells[startRow, i].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                            startRow++;

                        }

                    }



                    else
                    {
                        //Create a sheet
                        p.Workbook.Worksheets.Add("Loss Ratio");
                        ExcelWorksheet ws = p.Workbook.Worksheets[worksheet];
                        ws.Cells.Style.Font.Size = 11; //Default font size for whole sheet
                        ws.Cells.Style.Font.Name = "Calibri"; //Default Font name for whole sheet

                        int mergeColumns = 8;
                        int startRow = 1;

                        //    decimal totalAmount = 0;
                        //    decimal toatalpaid = 0;
                        //    decimal outstanding = 0;
                        Image image;
                        try
                        {
                            byte[] bytes = Convert.FromBase64String(response.tpaLogo);
                            using (MemoryStream ms = new MemoryStream(bytes))
                            {
                                image = Image.FromStream(ms);
                                ws.Row(1).Height = image.Height * .75;
                                var picture = ws.Drawings.AddPicture("logo", image);
                                picture.SetPosition(0, 0);
                                ws.Cells[1, 1, 1, 2].Merge = true;
                            }
                        }
                        catch (Exception)
                        {
                            image = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"STANDARD\\assets\\images\\bg_2.png"));
                            ws.Row(1).Height = image.Height * .75;
                            var picture = ws.Drawings.AddPicture("logo", image);
                            picture.SetPosition(0, 0);
                            ws.Cells[1, 1, 1, 2].Merge = true;
                        }



                        ws.Cells[startRow, 3].Value = "Incurred To Earned (Loss Ratio)";
                        ws.Cells[startRow, 3, startRow, 6].Merge = true;
                        ws.Cells[startRow, 3, startRow, 6].Style.Font.Bold = true;
                        ws.Cells[startRow, 3, startRow, 6].Style.Font.Size = 13;
                        ws.Cells[startRow, 3, startRow, 6].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        ws.Cells[startRow, 3, startRow, 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        startRow++;
                        startRow++;
                        ws.Cells[startRow, 1].Value = "* All prices are in USD";
                        startRow++;
                        //Headers


                        ws.Cells[startRow, 1].Value = "Contract Info";
                        ws.Cells[startRow, 1, startRow, 5].Merge = true;
                        ws.Cells[startRow, 1, startRow, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells[startRow, 1, startRow, 5].Style.Font.Bold = true;
                        ws.Cells[startRow, 1, startRow, 5].Style.Fill.PatternType = ExcelFillStyle.Solid; ;
                        ws.Cells[startRow, 1, startRow, 5].Style.Fill.BackgroundColor.SetColor(Color.LightGray);


                        ws.Cells[startRow, 6].Value = "Policy & Premium";
                        ws.Cells[startRow, 6, startRow, 11].Merge = true;
                        ws.Cells[startRow, 6, startRow, 11].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells[startRow, 6, startRow, 11].Style.Font.Bold = true;
                        ws.Cells[startRow, 6, startRow, 11].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells[startRow, 6, startRow, 11].Style.Fill.BackgroundColor.SetColor(Color.LightPink);

                        ws.Cells[startRow, 12].Value = "Claims";
                        ws.Cells[startRow, 12, startRow, 23].Merge = true;
                        ws.Cells[startRow, 12, startRow, 23].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells[startRow, 12, startRow, 23].Style.Font.Bold = true;
                        ws.Cells[startRow, 12, startRow, 23].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells[startRow, 12, startRow, 23].Style.Fill.BackgroundColor.SetColor(Color.LightGreen);

                        ws.Cells[startRow, 24].Value = "Calculations";
                        ws.Cells[startRow, 24, startRow, 35].Merge = true;
                        ws.Cells[startRow, 24, startRow, 35].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells[startRow, 24, startRow, 35].Style.Font.Bold = true;
                        ws.Cells[startRow, 24, startRow, 35].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells[startRow, 24, startRow, 35].Style.Fill.BackgroundColor.SetColor(Color.LightBlue);

                        startRow++;

                        ws.Cells[startRow, 12].Value = "Reinsurer";
                        ws.Cells[startRow, 12, startRow, 15].Merge = true;
                        ws.Cells[startRow, 12, startRow, 15].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells[startRow, 12, startRow, 15].Style.Font.Bold = true;

                        ws.Cells[startRow, 16].Value = "Special Approvals";
                        ws.Cells[startRow, 16, startRow, 19].Merge = true;
                        ws.Cells[startRow, 16, startRow, 19].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells[startRow, 16, startRow, 19].Style.Font.Bold = true;

                        ws.Cells[startRow, 20].Value = "Total";
                        ws.Cells[startRow, 20, startRow, 23].Merge = true;
                        ws.Cells[startRow, 20, startRow, 23].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells[startRow, 20, startRow, 23].Style.Font.Bold = true;

                        ws.Cells[startRow, 24].Value = "Loss Ratio";
                        ws.Cells[startRow, 24, startRow, 29].Merge = true;
                        ws.Cells[startRow, 24, startRow, 29].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells[startRow, 24, startRow, 29].Style.Font.Bold = true;

                        startRow++;

                        ws.Cells[startRow, 1].Value = "Country";
                        ws.Cells[startRow, 2].Value = "Dealer/ contract";
                        ws.Cells[startRow, 3].Value = "Contract Year";
                        ws.Cells[startRow, 4].Value = "Warranty Type";
                        ws.Cells[startRow, 5].Value = "Cover Type";
                        ws.Cells[startRow, 6].Value = "Policy Count";
                        ws.Cells[startRow, 7].Value = "Gross Premium";
                        ws.Cells[startRow, 8].Value = "Earned Gross Premium";
                        ws.Cells[startRow, 9].Value = "Net Premium";
                        ws.Cells[startRow, 10].Value = "Earned Net Premium";
                        ws.Cells[startRow, 11].Value = "Risk Completed ";
                        ws.Cells[startRow, 12].Value = "Paid Claims";
                        ws.Cells[startRow, 12, startRow, 13].Merge = true;
                        ws.Cells[startRow, 14].Value = "Paid + Reserved Claims";
                        ws.Cells[startRow, 14, startRow, 15].Merge = true;
                        ws.Cells[startRow, 16].Value = "Paid Claims";
                        ws.Cells[startRow, 16, startRow, 17].Merge = true;
                        ws.Cells[startRow, 18].Value = "Paid + Reserved Claims";
                        ws.Cells[startRow, 18, startRow, 19].Merge = true;
                        ws.Cells[startRow, 20].Value = "Paid Claims";
                        ws.Cells[startRow, 20, startRow, 21].Merge = true;
                        ws.Cells[startRow, 22].Value = "Paid + Reserved Claims";
                        ws.Cells[startRow, 22, startRow, 23].Merge = true;
                        ws.Cells[startRow, 22, startRow, 23].Style.Font.Bold = true;
                        ws.Cells[startRow, 24].Value = "RI Claims To NRP %";
                        ws.Cells[startRow, 25].Value = "RI Claims To Earned  NRP %";
                        ws.Cells[startRow, 26].Value = "Total Claims To NRP %";
                        ws.Cells[startRow, 27].Value = "Total Claims To Earned NRP %";
                        ws.Cells[startRow, 28].Value = "Total Tlaims To GP %";
                        ws.Cells[startRow, 29].Value = "Total Claims To Earned GP %";
                        ws.Cells[startRow, 30].Value = "Average NRP";
                        ws.Cells[startRow, 31].Value = "Average GP";
                        ws.Cells[startRow, 32].Value = "Weighted Burn Cost To RI Claims";
                        ws.Cells[startRow, 33].Value = "Weighted Burn Cost to Total Claims";
                        ws.Cells[startRow, 34].Value = "Weighted Claim Frequency To RI %";
                        ws.Cells[startRow, 35].Value = "Weighted Claim Frequency-Total %";
                        ws.Cells[startRow, 1, startRow, 35].Style.WrapText = true;
                        ws.Cells[startRow, 1, startRow, 35].Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        ws.Cells[startRow, 1, startRow, 35].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        startRow++;

                        ws.Cells[startRow, 12].Value = "Count";
                        ws.Cells[startRow, 13].Value = "Value";
                        ws.Cells[startRow, 14].Value = "Count";
                        ws.Cells[startRow, 15].Value = "Value";
                        ws.Cells[startRow, 16].Value = "Count";
                        ws.Cells[startRow, 17].Value = "Value";
                        ws.Cells[startRow, 18].Value = "Count";
                        ws.Cells[startRow, 19].Value = "Value";
                        ws.Cells[startRow, 20].Value = "Count";
                        ws.Cells[startRow, 21].Value = "Value";
                        ws.Cells[startRow, 22].Value = "Count";
                        ws.Cells[startRow, 23].Value = "Value";
                        ws.Cells[startRow, 22, startRow, 23].Style.Font.Bold = true;

                        startRow++;
                    }


                }
                //LostRatioSummaryOther
                else
                {

                    if (response.LostRatioSummaryOther.Count > 0)
                    {

                        //Create a sheet
                        p.Workbook.Worksheets.Add("Loss Ratio");
                        ExcelWorksheet ws = p.Workbook.Worksheets[worksheet];
                        ws.Cells.Style.Font.Size = 11; //Default font size for whole sheet
                        ws.Cells.Style.Font.Name = "Calibri"; //Default Font name for whole sheet
                        int mergeColumns = 8;
                        int startRow = 1;

                        //    decimal totalAmount = 0;
                        //    decimal toatalpaid = 0;
                        //    decimal outstanding = 0;
                        Image image;
                        try
                        {
                            byte[] bytes = Convert.FromBase64String(response.tpaLogo);
                            using (MemoryStream ms = new MemoryStream(bytes))
                            {
                                image = Image.FromStream(ms);
                                ws.Row(1).Height = image.Height * .75;
                                var picture = ws.Drawings.AddPicture("logo", image);
                                picture.SetPosition(0, 0);
                                ws.Cells[1, 1, 1, 2].Merge = true;
                            }
                        }
                        catch (Exception)
                        {
                            image = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"STANDARD\\assets\\images\\bg_2.png"));
                            ws.Row(1).Height = image.Height * .75;
                            var picture = ws.Drawings.AddPicture("logo", image);
                            picture.SetPosition(0, 0);
                            ws.Cells[1, 1, 1, 2].Merge = true;
                        }



                        ws.Cells[startRow, 3].Value = "Incurred To Earned (Loss Ratio)";
                        ws.Cells[startRow, 3, startRow, 6].Merge = true;
                        ws.Cells[startRow, 3, startRow, 6].Style.Font.Bold = true;
                        ws.Cells[startRow, 3, startRow, 6].Style.Font.Size = 13;
                        ws.Cells[startRow, 3, startRow, 6].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        ws.Cells[startRow, 3, startRow, 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        startRow++;
                        startRow++;
                        ws.Cells[startRow, 1].Value = "* All prices are in USD";
                        startRow++;
                        //Headers

                        ws.Cells[startRow, 1].Value = "Contract Info";
                        ws.Cells[startRow, 1, startRow, 11].Merge = true;
                        ws.Cells[startRow, 1, startRow, 11].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells[startRow, 1, startRow, 11].Style.Font.Bold = true;
                        ws.Cells[startRow, 1, startRow, 11].Style.Fill.PatternType = ExcelFillStyle.Solid; ;
                        ws.Cells[startRow, 1, startRow, 11].Style.Fill.BackgroundColor.SetColor(Color.LightGray);


                        ws.Cells[startRow, 12].Value = "Policy & Premium";
                        ws.Cells[startRow, 12, startRow, 17].Merge = true;
                        ws.Cells[startRow, 12, startRow, 17].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells[startRow, 12, startRow, 17].Style.Font.Bold = true;
                        ws.Cells[startRow, 12, startRow, 17].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells[startRow, 12, startRow, 17].Style.Fill.BackgroundColor.SetColor(Color.LightPink);

                        ws.Cells[startRow, 18].Value = "Claims";
                        ws.Cells[startRow, 18, startRow, 29].Merge = true;
                        ws.Cells[startRow, 18, startRow, 29].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells[startRow, 18, startRow, 29].Style.Font.Bold = true;
                        ws.Cells[startRow, 18, startRow, 29].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells[startRow, 18, startRow, 29].Style.Fill.BackgroundColor.SetColor(Color.LightGreen);

                        ws.Cells[startRow, 30].Value = "";
                        ws.Cells[startRow, 30, startRow, 41].Merge = true;
                        ws.Cells[startRow, 30, startRow, 41].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells[startRow, 30, startRow, 41].Style.Font.Bold = true;
                        ws.Cells[startRow, 30, startRow, 41].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells[startRow, 30, startRow, 41].Style.Fill.BackgroundColor.SetColor(Color.LightBlue);

                        startRow++;

                        ws.Cells[startRow, 18].Value = "Reinsurer";
                        ws.Cells[startRow, 18, startRow, 21].Merge = true;
                        ws.Cells[startRow, 18, startRow, 21].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells[startRow, 18, startRow, 21].Style.Font.Bold = true;

                        ws.Cells[startRow, 22].Value = "Special Approvals";
                        ws.Cells[startRow, 22, startRow, 25].Merge = true;
                        ws.Cells[startRow, 22, startRow, 25].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells[startRow, 22, startRow, 25].Style.Font.Bold = true;

                        ws.Cells[startRow, 26].Value = "Total";
                        ws.Cells[startRow, 26, startRow, 29].Merge = true;
                        ws.Cells[startRow, 26, startRow, 29].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells[startRow, 26, startRow, 29].Style.Font.Bold = true;

                        ws.Cells[startRow, 30].Value = "Loss Ratio";
                        ws.Cells[startRow, 30, startRow, 35].Merge = true;
                        ws.Cells[startRow, 30, startRow, 35].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells[startRow, 30, startRow, 35].Style.Font.Bold = true;

                        startRow++;

                        ws.Row(startRow).Height = 34;
                        ws.Cells[startRow, 1].Value = "Earned Date";
                        ws.Cells[startRow, 2].Value = "Claim Date";
                        ws.Column(3).Width = 15;
                        ws.Cells[startRow, 3].Value = "Make";
                        ws.Column(4).Width = 15;
                        ws.Cells[startRow, 4].Value = "Model";
                        ws.Column(5).Width = 15;
                        ws.Cells[startRow, 5].Value = "Cylinder Count";
                        ws.Column(6).Width = 15;
                        ws.Cells[startRow, 6].Value = "EngineCapacity Number";
                        ws.Cells[startRow, 7].Value = "Bordx Start Date";
                        ws.Cells[startRow, 8].Value = "Bordx End Date";
                        ws.Column(9).Width = 15;
                        ws.Cells[startRow, 9].Value = "EXTmonth";
                        ws.Cells[startRow, 10].Value = "Warranty Type";
                        ws.Cells[startRow, 11].Value = "Cover Type";
                        ws.Cells[startRow, 12].Value = "Policy Count";
                        ws.Column(13).Width = 15;
                        ws.Cells[startRow, 13].Value = "Gross Premium";
                        ws.Column(14).Width = 12.5;
                        ws.Cells[startRow, 14].Value = "Earned Gross  Premium";
                        ws.Cells[startRow, 15].Value = "Net Premium";
                        ws.Column(16).Width = 11;
                        ws.Cells[startRow, 16].Value = "Earned Net Premium";
                        ws.Column(17).Width = 13.5;
                        ws.Cells[startRow, 17].Value = "Risk Completed %";
                        ws.Cells[startRow, 18].Value = "Paid Claims";
                        ws.Cells[startRow, 18, startRow, 19].Merge = true;
                        ws.Cells[startRow, 20].Value = "Paid + Reserved claims";
                        ws.Cells[startRow, 20, startRow, 21].Merge = true;
                        ws.Cells[startRow, 22].Value = "Paid Claims";
                        ws.Cells[startRow, 22, startRow, 23].Merge = true;
                        ws.Cells[startRow, 24].Value = "Paid + Reserved Claims";
                        ws.Cells[startRow, 24, startRow, 25].Merge = true;
                        ws.Cells[startRow, 26].Value = "Paid Claims";
                        ws.Cells[startRow, 26, startRow, 27].Merge = true;
                        ws.Cells[startRow, 28].Value = "Paid + Reserved Claims";
                        ws.Cells[startRow, 28, startRow, 29].Merge = true;
                        ws.Cells[startRow, 28, startRow, 29].Style.Font.Bold = true;
                        ws.Cells[startRow, 30].Value = "RI Claims to NRP %";
                        ws.Column(31).Width = 14;
                        ws.Cells[startRow, 31].Value = "RI Claims to earned  NRP %";
                        ws.Column(32).Width = 11.5;
                        ws.Cells[startRow, 32].Value = "Total claims to NRP %";
                        ws.Column(33).Width = 13.7;
                        ws.Cells[startRow, 33].Value = "Total Claims to Earned NRP %";
                        ws.Column(34).Width = 11.5;
                        ws.Cells[startRow, 34].Value = "Total Claims to GP %";
                        ws.Column(35).Width = 13.7;
                        ws.Cells[startRow, 35].Value = "Total Claims to Earned GP %";
                        ws.Cells[startRow, 36].Value = "Average NRP";
                        ws.Cells[startRow, 37].Value = "Average GP";
                        ws.Column(38).Width = 15;
                        ws.Cells[startRow, 38].Value = "Weighted Burn Cost to RI Claims";
                        ws.Column(39).Width = 17.5;
                        ws.Cells[startRow, 39].Value = "Weighted Burn Cost to Total Claims";
                        ws.Column(40).Width = 16.5;
                        ws.Cells[startRow, 40].Value = "Weighted Claim Frequency to RI %";
                        ws.Column(41).Width = 17.5;
                        ws.Cells[startRow, 41].Value = "Weighted Claim Frequency-Total %";
                        ws.Cells[startRow, 1, startRow, 41].Style.WrapText = true;
                        ws.Cells[startRow, 1, startRow, 41].Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        ws.Cells[startRow, 1, startRow, 41].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        startRow++;

                        ws.Cells[startRow, 18].Value = "Count";
                        ws.Cells[startRow, 19].Value = "Value";
                        ws.Cells[startRow, 20].Value = "Count";
                        ws.Cells[startRow, 21].Value = "Value";
                        ws.Cells[startRow, 22].Value = "Count";
                        ws.Cells[startRow, 23].Value = "Value";
                        ws.Cells[startRow, 24].Value = "Count";
                        ws.Cells[startRow, 25].Value = "Value";
                        ws.Cells[startRow, 26].Value = "Count";
                        ws.Cells[startRow, 27].Value = "Value";
                        ws.Cells[startRow, 28].Value = "Count";
                        ws.Cells[startRow, 29].Value = "Value";
                        ws.Cells[startRow, 28, startRow, 29].Style.Font.Bold = true;

                        startRow++;

                        foreach (var detail in response.LostRatioSummaryOther)
                        {


                            decimal TotPaidReservedClaimsValue = (detail.ReInPaidReservedClaimsValue + detail.SAPaidReservedClaimsValue);
                            decimal EarnedGrossPremium = detail.GrossPremium * detail.RiskCompleted / 100M;
                            decimal EarnedNetPremium = detail.NetPremium * detail.RiskCompleted / 100M;

                            ws.Cells[startRow, 1].Value = detail.EarnedDate.ToString("dd-MMM-yyyy");
                            ws.Cells[startRow, 2].Value = detail.ClaimDate.ToString("dd-MMM-yyyy");
                            ws.Cells[startRow, 3].Value = detail.MakeName;
                            ws.Cells[startRow, 4].Value = detail.ModelName;
                            ws.Cells[startRow, 5].Value = detail.CylinderCount;
                            ws.Cells[startRow, 6].Value = detail.EngineCapacityNumber;
                            ws.Cells[startRow, 7].Value = detail.BordxStartDate.ToString("dd-MMM-yyyy");
                            ws.Cells[startRow, 8].Value = detail.BordxEndDate.ToString("dd-MMM-yyyy");
                            ws.Cells[startRow, 9].Value = detail.EXTmonth;
                            ws.Cells[startRow, 10].Value = detail.WarrantyType;
                            ws.Cells[startRow, 11].Value = detail.CoverType;
                            ws.Cells[startRow, 12].Value = detail.PolicyCount;//F7//
                            ws.Cells[startRow, 13].Style.Numberformat.Format = "#,##0.00";
                            ws.Cells[startRow, 13].Value = detail.GrossPremium;//G7//
                            ws.Cells[startRow, 14].Style.Numberformat.Format = "#,##0.00";
                            ws.Cells[startRow, 14].Value = detail.ErnedGrossPremium;//H7//
                            ws.Cells[startRow, 15].Style.Numberformat.Format = "#,##0.00";
                            ws.Cells[startRow, 15].Value = detail.NetPremium;//I7//
                            ws.Cells[startRow, 16].Style.Numberformat.Format = "#,##0.00";
                            ws.Cells[startRow, 16].Value = detail.ErnedNetPremium;//J7//
                            ws.Cells[startRow, 17].Style.Numberformat.Format = "#,##0.00%";
                            ws.Cells[startRow, 17].Value = detail.RiskCompleted;//K7//
                            ws.Cells[startRow, 18].Value = detail.ReInPaidClaimsCount;
                            ws.Cells[startRow, 19].Style.Numberformat.Format = "#,##0.00";
                            ws.Cells[startRow, 19].Value = detail.ReInPaidClaimsValue;
                            ws.Cells[startRow, 20].Value = detail.ReInPaidReservedClaimsCount;//N7//
                            ws.Cells[startRow, 21].Style.Numberformat.Format = "#,##0.00";
                            ws.Cells[startRow, 21].Value = detail.ReInPaidReservedClaimsValue;//O7//
                            ws.Cells[startRow, 22].Value = detail.SAPaidClaimsCount;
                            ws.Cells[startRow, 23].Style.Numberformat.Format = "#,##0.00";
                            ws.Cells[startRow, 23].Value = detail.SAPaidClaimsValue;
                            ws.Cells[startRow, 24].Value = detail.SAPaidReservedClaimsCount;
                            ws.Cells[startRow, 25].Style.Numberformat.Format = "#,##0.00";
                            ws.Cells[startRow, 25].Value = detail.SAPaidReservedClaimsValue;
                            ws.Cells[startRow, 26].Value = (detail.ReInPaidClaimsCount + detail.SAPaidClaimsCount);//detail.TotPaidClaimsCount;
                            ws.Cells[startRow, 27].Style.Numberformat.Format = "#,##0.00";
                            ws.Cells[startRow, 27].Value = (detail.ReInPaidClaimsValue + detail.SAPaidClaimsValue);//detail.TotPaidClaimsValue;
                            ws.Cells[startRow, 28].Value = (detail.ReInPaidReservedClaimsCount + detail.SAPaidReservedClaimsCount);//detail.TotPaidReservedClaimsCount;  //V7//
                            ws.Cells[startRow, 29, startRow, 42].Style.Numberformat.Format = "#,##0.00";
                            ws.Cells[startRow, 29].Value = (detail.ReInPaidReservedClaimsValue + detail.SAPaidReservedClaimsValue);//detail.TotPaidReservedClaimsValue; //W7//
                            //ws.Cells[startRow, 30].Style.Numberformat.Format = "##0%";
                            //ws.Cells[startRow, 30].Value = Convert.ToDecimal(detail.NetPremium) != 0m ? Convert.ToInt32((Convert.ToDecimal(detail.ReInPaidReservedClaimsValue) / Convert.ToDecimal(detail.NetPremium))) : 0;//detail.RIClaimsToNRP + "%";
                            //ws.Cells[startRow, 31].Style.Numberformat.Format = "##0%";
                            //ws.Cells[startRow, 31].Value = Convert.ToDecimal(EarnedNetPremium) != 0m ? Convert.ToInt32((Convert.ToDecimal(detail.ReInPaidReservedClaimsValue) / Convert.ToDecimal(detail.ErnedNetPremium))) : 0;//detail.RIClaimsToEarnedNRP + "%";
                            //ws.Cells[startRow, 32].Style.Numberformat.Format = "##0%";
                            //ws.Cells[startRow, 32].Value = Convert.ToDecimal(detail.NetPremium) != 0m ? Convert.ToInt32((TotPaidReservedClaimsValue / Convert.ToDecimal(detail.NetPremium))) : 0;//detail.TotalClaimsToNRP + "%";
                            //ws.Cells[startRow, 33].Style.Numberformat.Format = "##0%";
                            //ws.Cells[startRow, 33].Value = Convert.ToDecimal(EarnedNetPremium) != 0m ? Convert.ToInt32((TotPaidReservedClaimsValue / Convert.ToDecimal(detail.ErnedNetPremium))) : 0;//detail.TotalClaimsToEarnedNRP + "%";
                            //ws.Cells[startRow, 34].Style.Numberformat.Format = "##0%";
                            //ws.Cells[startRow, 34].Value = Convert.ToDecimal(detail.GrossPremium) != 0m ? Convert.ToInt32((TotPaidReservedClaimsValue / Convert.ToDecimal(detail.GrossPremium))) : 0;//detail.TotalClaimsToGP + "%";
                            //ws.Cells[startRow, 35].Style.Numberformat.Format = "##0%";
                            //ws.Cells[startRow, 35].Value = Convert.ToDecimal(EarnedGrossPremium) != 0m ? Convert.ToInt32((TotPaidReservedClaimsValue / Convert.ToDecimal(detail.ErnedGrossPremium))) : 0;//detail.TotalClaimsToEarnedGP + "%";
                            //ws.Cells[startRow, 36].Value = detail.PolicyCount != 0 ? (Convert.ToDecimal(detail.NetPremium) / detail.PolicyCount) : 0;//detail.AverageNRP;
                            //ws.Cells[startRow, 37].Value = detail.PolicyCount != 0 ? (Convert.ToDecimal(detail.GrossPremium) / detail.PolicyCount) : 0;//detail.AverageGP;
                            //ws.Cells[startRow, 38].Value = (detail.PolicyCount == 0 || Convert.ToDecimal(detail.RiskCompleted) == 0m) ? 0m : (Convert.ToDecimal(detail.ReInPaidReservedClaimsValue) / (detail.PolicyCount * Convert.ToDecimal(detail.RiskCompleted) / 100));//detail.WeightedBurnCostToRIClaims;
                            //ws.Cells[startRow, 39].Value = (detail.PolicyCount == 0 || Convert.ToDecimal(detail.RiskCompleted) == 0m) ? 0m : (TotPaidReservedClaimsValue / (detail.PolicyCount * Convert.ToDecimal(detail.RiskCompleted) / 100));//detail.WeightedBurnCosttoTotalClaims;
                            //ws.Cells[startRow, 40].Style.Numberformat.Format = "##0%";
                            //ws.Cells[startRow, 40].Value = (detail.PolicyCount == 0 || Convert.ToDecimal(detail.RiskCompleted) == 0m) ? 0m : Convert.ToInt32((detail.ReInPaidReservedClaimsCount / (detail.PolicyCount * Convert.ToDecimal(detail.RiskCompleted) / 100)));//detail.WeightedClaimFrequencyToRI + "%";.ToString("0")
                            //ws.Cells[startRow, 41].Style.Numberformat.Format = "##0%";
                            //ws.Cells[startRow, 41].Value = (detail.PolicyCount == 0 || Convert.ToDecimal(detail.RiskCompleted) == 0m) ? 0m : Convert.ToInt32(((detail.ReInPaidReservedClaimsCount + detail.SAPaidReservedClaimsCount) / (detail.PolicyCount * Convert.ToDecimal(detail.RiskCompleted) / 100)));//detail.WeightedClaimFrequencyTotal + "%";


                            ws.Cells[startRow, 30].Style.Numberformat.Format = "##0.00%";
                            ws.Cells[startRow, 30].Value = detail.NetPremium != 0m ? detail.ReInPaidReservedClaimsValue / detail.NetPremium : 0;//detail.RIClaimsToNRP + "%";
                            ws.Cells[startRow, 31].Style.Numberformat.Format = "##0.00%";
                            ws.Cells[startRow, 31].Value = EarnedNetPremium != 0m ? (detail.ReInPaidReservedClaimsValue / EarnedNetPremium) / 100 : 0;//detail.RIClaimsToEarnedNRP + "%";
                            ws.Cells[startRow, 32].Style.Numberformat.Format = "##0.00%";
                            ws.Cells[startRow, 32].Value = detail.NetPremium != 0m ? TotPaidReservedClaimsValue / detail.NetPremium : 0;//detail.TotalClaimsToNRP + "%";
                            ws.Cells[startRow, 33].Style.Numberformat.Format = "##0.00%";
                            ws.Cells[startRow, 33].Value = EarnedNetPremium != 0m ? (TotPaidReservedClaimsValue / EarnedNetPremium) / 100 : 0;//detail.TotalClaimsToEarnedNRP + "%";
                            ws.Cells[startRow, 34].Style.Numberformat.Format = "##0.00%";
                            ws.Cells[startRow, 34].Value = detail.GrossPremium != 0m ? TotPaidReservedClaimsValue / detail.GrossPremium : 0;//detail.TotalClaimsToGP + "%";
                            ws.Cells[startRow, 35].Style.Numberformat.Format = "##0.00%";
                            ws.Cells[startRow, 35].Value = EarnedGrossPremium != 0m ? (TotPaidReservedClaimsValue / EarnedGrossPremium) / 100 : 0;//detail.TotalClaimsToEarnedGP + "%";
                            ws.Cells[startRow, 36].Value = detail.PolicyCount != 0 ? detail.NetPremium / detail.PolicyCount : 0;//detail.AverageNRP;
                            ws.Cells[startRow, 37].Value = detail.PolicyCount != 0 ? detail.GrossPremium / detail.PolicyCount : 0;//detail.AverageGP;
                            ws.Cells[startRow, 38].Value = (detail.PolicyCount == 0 || detail.RiskCompleted == 0m) ? 0m : detail.ReInPaidReservedClaimsValue / (detail.PolicyCount * (detail.RiskCompleted / 100)) / 100;//detail.WeightedBurnCostToRIClaims;
                            ws.Cells[startRow, 39].Value = (detail.PolicyCount == 0 || detail.RiskCompleted == 0m) ? 0m : TotPaidReservedClaimsValue / (detail.PolicyCount * (detail.RiskCompleted / 100)) / 100;//detail.WeightedBurnCosttoTotalClaims;
                            ws.Cells[startRow, 40].Style.Numberformat.Format = "##0.00%";
                            ws.Cells[startRow, 40].Value = (detail.PolicyCount == 0 || detail.RiskCompleted == 0m) ? 0m : detail.ReInPaidReservedClaimsCount / (detail.PolicyCount * (detail.RiskCompleted / 100)) / 100;//detail.WeightedClaimFrequencyToRI + "%";.ToString("0")
                            ws.Cells[startRow, 41].Style.Numberformat.Format = "##0.00%";
                            ws.Cells[startRow, 41].Value = (detail.PolicyCount == 0 || detail.RiskCompleted == 0m) ? 0m : (detail.ReInPaidReservedClaimsCount + detail.SAPaidReservedClaimsCount) / ((detail.PolicyCount * detail.RiskCompleted / 100)) / 100;//detail.WeightedClaimFrequencyTotal + "%";





                            //toatalpaid = toatalpaid + detail.PaidAmount;
                            //totalAmount = totalAmount + detail.ClaimAmount;
                            //outstanding = outstanding + detail.Outstanding;

                            //for (int i = 1; i <= mergeColumns; i++) ws.Cells[startRow, i].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                            startRow++;

                        }

                    }



                    else
                    {
                        //Create a sheet
                        p.Workbook.Worksheets.Add("Lost Ratio");
                        ExcelWorksheet ws = p.Workbook.Worksheets[worksheet];
                        ws.Cells.Style.Font.Size = 11; //Default font size for whole sheet
                        ws.Cells.Style.Font.Name = "Calibri"; //Default Font name for whole sheet
                        int mergeColumns = 8;
                        int startRow = 1;

                        //    decimal totalAmount = 0;
                        //    decimal toatalpaid = 0;
                        //    decimal outstanding = 0;
                        Image image;
                        try
                        {
                            byte[] bytes = Convert.FromBase64String(response.tpaLogo);
                            using (MemoryStream ms = new MemoryStream(bytes))
                            {
                                image = Image.FromStream(ms);
                                ws.Row(1).Height = image.Height * .75;
                                var picture = ws.Drawings.AddPicture("logo", image);
                                picture.SetPosition(0, 0);
                                ws.Cells[1, 1, 1, 2].Merge = true;
                            }
                        }
                        catch (Exception)
                        {
                            image = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"STANDARD\\assets\\images\\bg_2.png"));
                            ws.Row(1).Height = image.Height * .75;
                            var picture = ws.Drawings.AddPicture("logo", image);
                            picture.SetPosition(0, 0);
                            ws.Cells[1, 1, 1, 2].Merge = true;
                        }



                        ws.Cells[startRow, 3].Value = "Incurred To Earned (Loss Ratio)";
                        ws.Cells[startRow, 3, startRow, 6].Merge = true;
                        ws.Cells[startRow, 3, startRow, 6].Style.Font.Bold = true;
                        ws.Cells[startRow, 3, startRow, 6].Style.Font.Size = 13;
                        ws.Cells[startRow, 3, startRow, 6].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        ws.Cells[startRow, 3, startRow, 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        startRow++;
                        startRow++;
                        ws.Cells[startRow, 1].Value = "* All prices are in USD";
                        startRow++;
                        //Headers

                        ws.Cells[startRow, 1].Value = "Contract Info";
                        ws.Cells[startRow, 1, startRow, 5].Merge = true;
                        ws.Cells[startRow, 1, startRow, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells[startRow, 1, startRow, 5].Style.Font.Bold = true;
                        ws.Cells[startRow, 1, startRow, 5].Style.Fill.PatternType = ExcelFillStyle.Solid; ;
                        ws.Cells[startRow, 1, startRow, 5].Style.Fill.BackgroundColor.SetColor(Color.LightGray);


                        ws.Cells[startRow, 12].Value = "Policy & Premium";
                        ws.Cells[startRow, 12, startRow, 17].Merge = true;
                        ws.Cells[startRow, 12, startRow, 17].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells[startRow, 12, startRow, 17].Style.Font.Bold = true;
                        ws.Cells[startRow, 12, startRow, 17].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells[startRow, 12, startRow, 17].Style.Fill.BackgroundColor.SetColor(Color.LightPink);

                        ws.Cells[startRow, 18].Value = "Claims";
                        ws.Cells[startRow, 18, startRow, 29].Merge = true;
                        ws.Cells[startRow, 18, startRow, 29].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells[startRow, 18, startRow, 29].Style.Font.Bold = true;
                        ws.Cells[startRow, 18, startRow, 29].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells[startRow, 18, startRow, 29].Style.Fill.BackgroundColor.SetColor(Color.LightGreen);

                        ws.Cells[startRow, 30].Value = "Calculations";
                        ws.Cells[startRow, 30, startRow, 41].Merge = true;
                        ws.Cells[startRow, 30, startRow, 41].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells[startRow, 30, startRow, 41].Style.Font.Bold = true;
                        ws.Cells[startRow, 30, startRow, 41].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells[startRow, 30, startRow, 41].Style.Fill.BackgroundColor.SetColor(Color.LightBlue);

                        startRow++;

                        ws.Cells[startRow, 18].Value = "Reinsurer";
                        ws.Cells[startRow, 18, startRow, 21].Merge = true;
                        ws.Cells[startRow, 18, startRow, 21].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells[startRow, 18, startRow, 21].Style.Font.Bold = true;

                        ws.Cells[startRow, 22].Value = "Special Approvals";
                        ws.Cells[startRow, 22, startRow, 25].Merge = true;
                        ws.Cells[startRow, 22, startRow, 25].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells[startRow, 22, startRow, 25].Style.Font.Bold = true;

                        ws.Cells[startRow, 26].Value = "Total";
                        ws.Cells[startRow, 26, startRow, 29].Merge = true;
                        ws.Cells[startRow, 26, startRow, 29].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells[startRow, 26, startRow, 29].Style.Font.Bold = true;

                        ws.Cells[startRow, 30].Value = "Loss ratio";
                        ws.Cells[startRow, 30, startRow, 35].Merge = true;
                        ws.Cells[startRow, 30, startRow, 35].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells[startRow, 30, startRow, 35].Style.Font.Bold = true;

                        startRow++;

                        ws.Row(startRow).Height = 34;
                        ws.Cells[startRow, 1].Value = "Earned Date";
                        ws.Cells[startRow, 2].Value = "Claim Date";
                        ws.Cells[startRow, 3].Value = "Make";
                        ws.Cells[startRow, 4].Value = "Model";
                        ws.Cells[startRow, 5].Value = "Cylinder Count";
                        ws.Cells[startRow, 6].Value = "EngineCapacity Number";
                        ws.Cells[startRow, 7].Value = "Bordx Start Date";
                        ws.Cells[startRow, 8].Value = "Bordx End Date";
                        ws.Cells[startRow, 9].Value = "EXT Month";
                        ws.Cells[startRow, 10].Value = "Warranty Type";
                        ws.Cells[startRow, 11].Value = "Cover Type";
                        ws.Cells[startRow, 12].Value = "Policy Count";
                        ws.Column(13).Width = 15;
                        ws.Cells[startRow, 13].Value = "Gross Premium";
                        ws.Column(14).Width = 12.5;
                        ws.Cells[startRow, 14].Value = "Earned Gross Premium";
                        ws.Cells[startRow, 15].Value = "Net Premium";
                        ws.Column(16).Width = 11;
                        ws.Cells[startRow, 16].Value = "Earned Net Premium";
                        ws.Column(17).Width = 13.5;
                        ws.Cells[startRow, 17].Value = "Risk completed %";
                        ws.Cells[startRow, 18].Value = "Paid Claims";
                        ws.Cells[startRow, 18, startRow, 19].Merge = true;
                        ws.Cells[startRow, 20].Value = "Paid + Reserved Claims";
                        ws.Cells[startRow, 20, startRow, 21].Merge = true;
                        ws.Cells[startRow, 22].Value = "Paid Claims";
                        ws.Cells[startRow, 22, startRow, 23].Merge = true;
                        ws.Cells[startRow, 24].Value = "Paid + Reserved Claims";
                        ws.Cells[startRow, 24, startRow, 25].Merge = true;
                        ws.Cells[startRow, 26].Value = "Paid Claims";
                        ws.Cells[startRow, 26, startRow, 27].Merge = true;
                        ws.Cells[startRow, 28].Value = "Paid + Reserved Claims";
                        ws.Cells[startRow, 28, startRow, 29].Merge = true;
                        ws.Cells[startRow, 28, startRow, 29].Style.Font.Bold = true;
                        ws.Cells[startRow, 30].Value = "RI Claims to NRP %";
                        ws.Column(31).Width = 14;
                        ws.Cells[startRow, 31].Value = "RI Claims to Earned  NRP %";
                        ws.Column(32).Width = 11.5;
                        ws.Cells[startRow, 32].Value = "Total Claims to NRP %";
                        ws.Column(33).Width = 13.7;
                        ws.Cells[startRow, 33].Value = "Total Claims to Earned NRP %";
                        ws.Column(34).Width = 11.5;
                        ws.Cells[startRow, 34].Value = "Total Claims to GP %";
                        ws.Column(35).Width = 13.7;
                        ws.Cells[startRow, 35].Value = "Total Claims to Earned GP %";
                        ws.Cells[startRow, 36].Value = "Average NRP";
                        ws.Cells[startRow, 37].Value = "Average GP";
                        ws.Column(38).Width = 15;
                        ws.Cells[startRow, 38].Value = "Weighted Burn Cost to RI Claims";
                        ws.Column(39).Width = 17.5;
                        ws.Cells[startRow, 39].Value = "Weighted Burn Cost to Total Claims";
                        ws.Column(40).Width = 16.5;
                        ws.Cells[startRow, 40].Value = "Weighted Claim Frequency to RI %";
                        ws.Column(41).Width = 17.5;
                        ws.Cells[startRow, 41].Value = "Weighted Claim Frequency-Total %";
                        ws.Cells[startRow, 1, startRow, 41].Style.WrapText = true;
                        ws.Cells[startRow, 1, startRow, 41].Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        ws.Cells[startRow, 1, startRow, 41].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        startRow++;

                        ws.Cells[startRow, 18].Value = "Count";
                        ws.Cells[startRow, 19].Value = "Value";
                        ws.Cells[startRow, 20].Value = "Count";
                        ws.Cells[startRow, 21].Value = "Value";
                        ws.Cells[startRow, 22].Value = "Count";
                        ws.Cells[startRow, 23].Value = "Value";
                        ws.Cells[startRow, 24].Value = "Count";
                        ws.Cells[startRow, 25].Value = "Value";
                        ws.Cells[startRow, 26].Value = "Count";
                        ws.Cells[startRow, 27].Value = "Value";
                        ws.Cells[startRow, 28].Value = "Count";
                        ws.Cells[startRow, 29].Value = "Value";
                        ws.Cells[startRow, 28, startRow, 29].Style.Font.Bold = true;

                        startRow++;
                    }

                }

                Byte[] bin = p.GetAsByteArray(); ;
                return new MemoryStream(bin);
            }
        }
    }

    public class BordxSumFormulaDto
    {
        public string section { get; set; }
        public int column { get; set; }
        public int start { get; set; }
        public int end { get; set; }

    }
}