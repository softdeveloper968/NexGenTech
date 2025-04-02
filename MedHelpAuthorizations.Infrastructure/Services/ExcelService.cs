using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.ExportReport;
using MedHelpAuthorizations.Application.Features.Reports.CurrentSummary;
using MedHelpAuthorizations.Application.Helpers;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Shared.Helpers;
using Microsoft.Extensions.Localization;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using OfficeOpenXml.Table.PivotTable;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Infrastructure.Services
{
    public class ExcelService : IExcelService
    {
        private readonly IStringLocalizer<ExcelService> _localizer;

        public ExcelService(IStringLocalizer<ExcelService> localizer)
        {
            _localizer = localizer;
        }

        public async Task<string> ExportAsync(IEnumerable<ExportQueryResponse> data, Dictionary<string, Func<ExportQueryResponse, object>> mappers, string sheetName = "Sheet1", string workSheetName = "Worksheet1", string passwordString = null, bool isFirstColDenailReason = false, Func<ExportQueryResponse, object> groupByKeySelector = null, bool hasGroupByKeySelector = default)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using var package = new ExcelPackage();
            package.Workbook.Properties.Author = "Automated Integration Technologies";

            // Add worksheet and initialize
            var worksheet = package.Workbook.Worksheets.Add(_localizer[workSheetName]);
            worksheet.Name = sheetName;
            worksheet.Cells.Style.Font.Size = 11;
            worksheet.Cells.Style.Font.Name = "Calibri";

            // Add headers
            var headers = mappers.Keys.ToList();
            for (int colIndex = 1; colIndex <= headers.Count; colIndex++)
            {
                var cell = worksheet.Cells[1, colIndex];
                cell.Value = headers[colIndex - 1];
                cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
                cell.Style.Border.BorderAround(ExcelBorderStyle.Thin);
            }

            // Add data rows
            int rowIndex = 1;
            foreach (var item in data)
            {
                rowIndex++;
                int colIndex = 1;
                foreach (var header in headers)
                {
                    var cell = worksheet.Cells[rowIndex, colIndex++];
                    var value = mappers[header](item) ?? string.Empty;

                    worksheet.Column(colIndex).AutoFit(10, 25);

                    ExportHelpers.SetCellFormat(cell, value);
                }
            }

            // Group data by key and create additional sheets for each group
            if (hasGroupByKeySelector && groupByKeySelector != null)
            {
                var distinctGroupingKeyDetails = data.GroupBy(groupByKeySelector).ToList();
                foreach (var groupData in distinctGroupingKeyDetails)
                {
                    var index = distinctGroupingKeyDetails.IndexOf(groupData) + 1;
                    AddPayerSheetAsync(headers, index, groupData, mappers, package, sheetName: "");
                }
            }

            // worksheet.Cells[1, 1, worksheet.Dimension.End.Row, worksheet.Dimension.End.Column].AutoFitColumns();

            // Optionally, adjust the "Exception Reason" column to be between min and max width constraints
            var exceptionReasonColumnIndex = headers.IndexOf("Exception Reason") + 1;
            if (exceptionReasonColumnIndex > 0)
            {
                worksheet.Column(exceptionReasonColumnIndex).AutoFit(10, 75); // Min width = 10, Max width = 75 characters
            }

            HideEmptyColumns(worksheet);

            // Convert to base64 string and return
            var byteArray = await package.GetAsByteArrayAsync(passwordString);
            return Convert.ToBase64String(byteArray);
        }

        public async Task<string> ExportMultipleSheetsAsync(List<IEnumerable<ExportQueryResponse>> exportDetails, List<Dictionary<string, Func<ExportQueryResponse, object>>> mapperList, List<string> sheetNames, bool boldLastRow = false, bool applyBoldRowInFirstDataModel = true, bool applyBoldHeader = false, string passwordString = null, Func<ExportQueryResponse, object> groupByKeySelector = null, bool hasGroupByKeySelector = default(bool))
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage p = new();
            p.Workbook.Properties.Author = "Automated Integration Technologies";

            for (int exportIndex = 0; exportIndex < exportDetails.Count(); exportIndex++)
            {
                try
                {
                    var exportData = exportDetails[exportIndex];
                    var mappers = mapperList[exportIndex];
                    string sheetName = sheetNames[exportIndex] ?? $"Sheet{exportIndex + 1}";

                    AddDataInWorkSheet(exportData, mappers, sheetName, applyBoldRowInFirstDataModel, applyBoldHeader, boldLastRow, ref p, groupByKeySelector, hasGroupByKeySelector);

                }
                catch (Exception e)
                {
                    throw;
                }
            }

            byte[] byteArray = await p.GetAsByteArrayAsync(passwordString);
            return Convert.ToBase64String(byteArray);
        }

        protected void AddDataInWorkSheet(IEnumerable<ExportQueryResponse> data, Dictionary<string, Func<ExportQueryResponse, object>> mappers, string sheetName, bool applyBoldRowInFirstDataModel, bool applyBoldHeader, bool boldLastRow,
                                        ref ExcelPackage p, Func<ExportQueryResponse, object> groupByKeySelector = null, bool hasGroupByKeySelector = default(bool))
        {
            try
            {
                p.Workbook.Worksheets.Add(sheetName);
                ExcelWorksheet worksheet = p.Workbook.Worksheets[p.Workbook.Worksheets.Count - 1];
                worksheet.Name = sheetName;
                worksheet.Cells.Style.Font.Size = 11;
                worksheet.Cells.Style.Font.Name = "Calibri";
                int colIndex = 1;
                int rowIndex = 1;
                var headers = mappers.Keys.ToList();

                // Add header row
                foreach (var header in headers)
                {
                    var cell = worksheet.Cells[rowIndex, colIndex];
                    var fill = cell.Style.Fill;
                    fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);

                    if (applyBoldHeader)
                        cell.Style.Font.Bold = true;

                    var border = cell.Style.Border;
                    border.Bottom.Style =
                        border.Top.Style =
                            border.Left.Style =
                                border.Right.Style = ExcelBorderStyle.Thin;

                    cell.Value = header;
                    ExportHelpers.SetCellFormat(cell, cell.Value);

                    colIndex++;
                }

                // Add data rows
                foreach (var item in data)
                {
                    colIndex = 1;
                    rowIndex++;

                    var result = headers.Select(header => mappers[header](item) ?? string.Empty).ToArray();
                    worksheet.Cells[rowIndex, 1, rowIndex, headers.Count].LoadFromArrays(new[] { result });

                    // Set cell format for each cell in the row
                    for (int i = 1; i <= headers.Count; i++)
                    {
                        var cell = worksheet.Cells[rowIndex, i];
                        ExportHelpers.SetCellFormat(cell, cell.Value);
                    }

                }

                // Apply AutoFit after writing all data
                worksheet.Cells[1, 1, rowIndex, headers.Count].AutoFitColumns(10, 25);

                // Group data by key and create additional sheets for each group
                if (hasGroupByKeySelector && groupByKeySelector != null)
                {
                    var distinctGroupingKeyDetails = data.GroupBy(groupByKeySelector).ToList();
                    foreach (var payerData in distinctGroupingKeyDetails)
                    {
                        try
                        {
                            AddPayerSheetAsync(headers, p.Workbook.Worksheets.Count, payerData, mappers, p, sheetName: sheetName);
                        }
                        catch (Exception e)
                        {
                            throw;
                        }
                    }
                }

                // Apply bold formatting to the last row if needed
                if (boldLastRow)
                {
                    int lastRow = worksheet.Dimension.End.Row;
                    worksheet.Cells[lastRow, 1, lastRow, worksheet.Dimension.End.Column].Style.Font.Bold = true;
                    boldLastRow = false; // Reset flag after applying
                }
                else
                {
                    int lastRow = worksheet.Dimension.End.Row;
                    worksheet.Cells[lastRow, 1, lastRow, worksheet.Dimension.End.Column].Style.Font.Bold = false;
                }

                // Auto-fit columns and hide empty columns
                // worksheet.Cells[1, 1, worksheet.Dimension.End.Row, worksheet.Dimension.End.Column].AutoFitColumns();
                var exceptionReasonColumnIndex = headers.IndexOf("Exception Reason") + 1;
                if (exceptionReasonColumnIndex > 0)
                {
                    worksheet.Column(exceptionReasonColumnIndex).AutoFit(10, 75); // Min width = 10, Max width = 75 characters
                }

                HideEmptyColumns(worksheet);

            }
            catch (Exception e)
            {
                throw;
            }
        }

        private void HideEmptyColumns(ExcelWorksheet worksheet)
        {
            int colCount = worksheet.Dimension.End.Column;
            int rowCount = worksheet.Dimension.End.Row;

            for (int colIndex = colCount; colIndex >= 1; colIndex--)
            {
                bool isColumnEmpty = true;

                // Check if any cell in the column has data
                for (int rowIndex = 2; rowIndex <= rowCount; rowIndex++) // Start from row 2 to skip header
                {
                    var cellValue = worksheet.Cells[rowIndex, colIndex].Text;
                    if (!string.IsNullOrEmpty(cellValue))
                    {
                        isColumnEmpty = false;
                        break;
                    }
                }

                // Hide the column if it is empty
                if (isColumnEmpty)
                {
                    worksheet.Column(colIndex).Hidden = true;
                }
            }
        }

        #region Export Custom Report
        ///<summary>
        /// Export Multiple CustomReport Tabs In Single Worksheet.
        /// </summary>
        /// <param name="exportReportData"> Report Details as String Format.</param>
        /// <param name="headerList">Custom Report Header Details</param>
        /// <param name="sheetNames">Custom Report Sheet</param>
        /// <returns>Return Byte Array.</returns>
        public async Task<string> ExportMultipleCustomReportTabsInWorksheet(List<string> exportReportData, List<List<string>> headerList, List<string> sheetNames)
        {
            try
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                ExcelPackage package = new();
                package.Workbook.Properties.Author = "Automated Integration Technologies";

                for (int exportIndex = 0; exportIndex < exportReportData.Count; exportIndex++)
                {
                    string exportData = exportReportData[exportIndex];
                    List<string> headers = headerList[exportIndex];
                    string sheetName = sheetNames[exportIndex] ?? $"Custom Report Sheet{exportIndex + 1}";

                    AddWorksheetWithData(sheetName, exportData, headers, ref package);
                }

                byte[] byteArray = await package.GetAsByteArrayAsync();
                return Convert.ToBase64String(byteArray);
            }
            catch (Exception e)
            {
                throw;
            }
        }
        private static void AddWorksheetWithData(string worksheetName, string data, List<string> headers, ref ExcelPackage package)
        {
            var worksheet = package.Workbook.Worksheets.Add(worksheetName);

            // Write headers to the worksheet
            var headerRow = worksheet.Cells["A1"].LoadFromArrays(new List<string[]>() { headers.ToArray() });
            headerRow.Style.Font.Bold = true;

            // Split the data into rows and columns
            var rows = data.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            int startDataRow = 2; // Starting row for data (after the headers)
            int startDataColumn = 1; // Starting column for data

            // Populate the data into the worksheet
            for (int row = 0; row < rows.Length; row++)
            {
                var columns = rows[row].Split(',');
                for (int col = 0; col < columns.Length; col++)
                {
                    worksheet.Cells[startDataRow + row, startDataColumn + col].Value = columns[col];
                }
            }

            // Auto-fit columns for better visibility
            worksheet.Cells.AutoFitColumns();
        }

        #endregion

        public async Task<string> ExportWithPivotTableAsync(IEnumerable<ExportQueryResponse> data,
                                                                            Dictionary<string, Func<ExportQueryResponse, object>> mappers,
                                                                            string sheetName = "Sheet1",
                                                                            string workSheetName = "Worksheet1",
                                                                            string passwordString = null,
                                                                            Func<ExportQueryResponse, object> groupByKeySelector = null,
                                                                            bool hasGroupByKeySelector = default(bool))
        {
            try
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using var p = new ExcelPackage();
                p.Workbook.Properties.Author = "Automated Integration Technologies";

                // Extract column headers from the mappers dictionary
                var headers = mappers.Keys.Select(x => x).ToList();

                // Add a worksheet to the Excel package
                var ws = p.Workbook.Worksheets.Add(_localizer[workSheetName]);
                ws.Name = sheetName;
                ws.Cells.Style.Font.Size = 11;
                ws.Cells.Style.Font.Name = "Calibri";

                // Populate main data in the worksheet
                PopulateData(ws, data, mappers, headers);

                // Apply AutoFilter to the header row
                ws.Cells[1, 1, 1, mappers.Count].AutoFilter = true;

                // Define the range of cells for the main data
                var startCell = ws.Cells["A1"];
                var endCell = ws.Cells[ws.Dimension.End.Row, ws.Dimension.End.Column];
                var dataRange = ws.Cells[startCell.Start.Row, startCell.Start.Column, endCell.End.Row, endCell.End.Column];

                // AutoFit columns for the main data range
                dataRange.AutoFitColumns();
                // Apply visual styles to column headers
                SetHeaderStyles(ws, headers);


                // Group data by key and create additional sheets for each group
                if (hasGroupByKeySelector && groupByKeySelector != null)
                {
                    var distinctGroupingKeyDetails = data.GroupBy(groupByKeySelector).ToList();
                    foreach (var payerData in distinctGroupingKeyDetails)
                    {
                        try
                        {
                            AddPayerSheetAsync(headers, p.Workbook.Worksheets.Count, payerData, mappers, p, sheetName: sheetName);
                        }
                        catch (Exception e)
                        {
                            throw;
                        }
                    }
                }

                var tableSource = GetDataRange(p.Workbook.Worksheets[0]);
                // Create pivot tables
                await CreateDenialSummaryPivotTable(p, tableSource);


                //// Create pivot tables
                await CreateFinancialSummaryPivotTable(p, dataRange);
                await CreateDenialSummaryPivotTable(p, dataRange);

                var exceptionReasonColumnIndex = headers.IndexOf("Exception Reason") + 1;
                if (exceptionReasonColumnIndex > 0)
                {
                    ws.Column(exceptionReasonColumnIndex).AutoFit(10, 75); // Min width = 10, Max width = 75 characters
                }

                // Hide empty columns
                HideEmptyColumns(ws);
                // Convert the Excel package to a byte array with optional password protection
                var byteArray = await p.GetAsByteArrayAsync(passwordString);
                return Convert.ToBase64String(byteArray);
            }
            catch (Exception ex)
            {
                // Log or handle the exception as needed
                throw;
            }
        }


        #region functions used to create a detailed sheet with payers data sheet and pivot table
        /// <summary>
        /// Set headers and style them for each sheet
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="headers"></param>
        private static void SetHeaderStyles(ExcelWorksheet ws, List<string> headers)
        {
            try
            {
                // Initialize column and row indices for cell placement
                var colIndex = 1;
                var rowIndex = 1;

                // Loop through each header in the headers list
                foreach (var header in headers)
                {
                    // Get the reference to the current cell in the worksheet
                    var cell = ws.Cells[rowIndex, colIndex];

                    // Apply a solid blue fill to the cell
                    var fill = cell.Style.Fill;
                    fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(System.Drawing.Color.Blue);

                    // Apply thin borders to the cell on all sides
                    var border = cell.Style.Border;
                    border.Bottom.Style =
                        border.Top.Style =
                            border.Left.Style =
                                border.Right.Style = ExcelBorderStyle.Thin;

                    ws.Column(colIndex).AutoFit(10, 25);

                    // Set the cell value to the current header
                    cell.Value = header;

                    ExportHelpers.SetCellFormat(cell, cell.Value);

                    // Move to the next column
                    colIndex++;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private static async Task<ExcelPivotTable> CreateFinancialSummaryPivotTable(ExcelPackage pck, ExcelRange dataRange, bool DataOnRows = false)
        {
            try
            {

                // Check if the data range contains at least two rows
                if ((dataRange.End.Row - dataRange.Start.Row) > 1)
                {
                    // Create a new worksheet for the pivot table summary
                    var wsPivot = pck.Workbook.Worksheets.Add(ReportHelper.Summary);

                    // Add a pivot table to the worksheet, starting from cell A1, using the specified data range, and naming it "PerPayer"
                    var pivotTable = wsPivot.PivotTables.Add(wsPivot.Cells["A1"], dataRange, "PerPayer");

                    // Add the "Payer Name" field to the row fields of the pivot table
                     pivotTable.RowFields.Add(pivotTable.Fields[StoredProcedureColumnsHelper.Payer_Name]);
                     pivotTable.RowFields.Add(pivotTable.Fields[StoredProcedureColumnsHelper.CPT_Code]);

                    // Get a reference to the "Payer Name" field in the pivot table
                    var payerNameField = pivotTable.Fields[StoredProcedureColumnsHelper.Payer_Name];

                    // Add the "Quantity" field as a data field to the pivot table
                    var dataField = pivotTable.DataFields.Add(pivotTable.Fields[StoredProcedureColumnsHelper.Quantity]);

                    // Add the "Allowed Amount" field as a data field to the pivot table
                    var allowedAmtField = pivotTable.DataFields.Add(pivotTable.Fields[StoredProcedureColumnsHelper.Allowed_Amt]);

                    // Add the "Amount" field as a data field to the pivot table
                    var amtField = pivotTable.DataFields.Add(pivotTable.Fields[StoredProcedureColumnsHelper.Billed_Amt]);

                    // Specify the format for the "Quantity" field in the pivot table
                    dataField.Format = "#,##0";

                    // Specify the format for the "Allowed Amount" field in the pivot table
                    allowedAmtField.Format = "$#,##0.00";

                    // Specify the format for the "Amount" field in the pivot table
                    amtField.Format = "$#,##0.00";

                    // Set the data layout to display values on rows in the pivot table
                    pivotTable.DataOnRows = DataOnRows;

                    // Collapse all items in each row field:
                    foreach (var rowField in pivotTable.RowFields)
                    {
                        rowField.Items.Refresh();          // Load the pivot items from the source data
                        rowField.Items.ShowDetails(false); // Collapse all items (no details shown)
                    }
                    // Return the created pivot table
                    return pivotTable;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        private static async Task<ExcelPivotTable> CreateDenialSummaryPivotTable(ExcelPackage pck, ExcelRangeBase dataRange)
        {
            try
            {
                // Check if the data range contains at least two rows
                if ((dataRange.End.Row - dataRange.Start.Row) > 1)
                {
                    // Create a new worksheet for the pivot table summary
                    var wsPivot = pck.Workbook.Worksheets.Add(StoredProcedureColumnsHelper.DenialSummary);

                    // Add a pivot table to the worksheet, starting from cell A1, using the specified data range, and naming it "PerPayer"
                    var pivotTable = wsPivot.PivotTables.Add(wsPivot.Cells["A1"], dataRange, "PerDenialReason");

                    // Add the "Payer Name" field to the row fields of the pivot table
                    var rowField = pivotTable.RowFields.Add(pivotTable.Fields[StoredProcedureColumnsHelper.ExceptionReasonCategory]);

                    // Get a reference to the "Payer Name" field in the pivot table
                    var payerNameField = pivotTable.Fields[StoredProcedureColumnsHelper.ExceptionReasonCategory];

                    // Add the "Quantity" field as a data field to the pivot table
                    var dataField = pivotTable.DataFields.Add(pivotTable.Fields[StoredProcedureColumnsHelper.Quantity]);

                    // Add the "Amount" field as a data field to the pivot table
                    var amtField = pivotTable.DataFields.Add(pivotTable.Fields[StoredProcedureColumnsHelper.Billed_Amt]);

                    // Specify the format for the "Quantity" field in the pivot table
                    dataField.Format = "#,##0";

                    // Specify the format for the "Amount" field in the pivot table
                    amtField.Format = "$#,##0.00";

                    // Set the data layout to display values on rows in the pivot table
                    pivotTable.DataOnRows = false;

                    // Ensure all row fields are collapsed by default
                    rowField.Items.Refresh();          // Load the pivot items from the source data
                    rowField.Items.ShowDetails(false); // Collapse all items

                    // Return the created pivot table
                    return pivotTable;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion

        public string AddTypePrefix(string type, string value)
        {
            string result = $"{type}Col";

            switch (type)
            {
                case ExportHelper.CurrencyType:
                    if (!string.IsNullOrEmpty(value))
                    {
                        result += $"{value}";
                        return result;
                    }
                    break;

                case ExportHelper.TimeType:
                case ExportHelper.DateType:
                    if (!string.IsNullOrEmpty(value))
                    {
                        result += $"{value}";
                        return result;
                    }
                    break;
            }

            return value;
        }

        public static void AddPayerSheetAsync(List<string> headers, int index, IGrouping<object, ExportQueryResponse> payerData, Dictionary<string, Func<ExportQueryResponse, object>> mappers, ExcelPackage p, string sheetName, string sheetPrefix = "")
        {
            try
            {
                // Extract the payer name from the first item in the payerData list
                string payerName = string.Concat(sheetPrefix, payerData.Key);

                if (!string.IsNullOrEmpty(sheetName))
                    payerName = string.Concat(sheetName, "_", payerName);

                //var currentIndex = p.Workbook.Worksheets.Count;

                // Create a new worksheet for payer details using the payer's name as the sheet name
                var ws = p.Workbook.Worksheets.Add($"{index}.{payerName} Details");

                // Set the worksheet name to the payer's name
                ws.Name = $"{index}.{payerName}";

                // Set font size and name for consistent styling
                ws.Cells.Style.Font.Size = 11;
                ws.Cells.Style.Font.Name = "Calibri";

                // Apply custom styles to the header cells in the worksheet
                SetHeaderStyles(ws, headers);

                // Populate the worksheet with payer-specific data
                PopulateData(ws, payerData.ToList(), mappers, headers);

                // Enable autofilter on the first row for easy data filtering
                ws.Cells[1, 1, 1, mappers.Count].AutoFilter = true;

                // Auto-fit columns based on the content in the worksheet
                ws.Cells[ws.Dimension.Address].AutoFitColumns();
                var exceptionReasonColumnIndex = headers.IndexOf("Exception Reason") + 1;
                if (exceptionReasonColumnIndex > 0)
                {
                    ws.Column(exceptionReasonColumnIndex).AutoFit(10, 75); // Min width = 10, Max width = 75 characters
                }

                // Save the changes to the Excel package
                p.Save();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private static void PopulateData(ExcelWorksheet ws, IEnumerable<ExportQueryResponse> data, Dictionary<string, Func<ExportQueryResponse, object>> mappers, List<string> headers)
        {
            var colIndex = 1;
            var rowIndex = 1;
            var alternateColor = true;

            foreach (var item in data)
            {
                colIndex = 1;
                rowIndex++;

                var result = headers.Select(header => mappers[header](item) ?? string.Empty);

                foreach (var value in result)
                {
                    var cell = ws.Cells[rowIndex, colIndex++];

                    var fill = cell.Style.Fill;
                    fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(alternateColor ? System.Drawing.Color.LightBlue : System.Drawing.Color.White);
                    ExportHelpers.SetCellFormat(cell, value);
                }

                alternateColor = !alternateColor;
            }
        }

        //EN-587
        public async Task<string> CreateInsuranceReimbursementAndPivotTableSheet(IEnumerable<ExportQueryResponse> data,
                                                                            Dictionary<string, Func<ExportQueryResponse, object>> mappers,
                                                                            string sheetName = "Sheet1",
                                                                            string workSheetName = "Worksheet1",
                                                                            string passwordString = null)
        {
            try
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using var p = new ExcelPackage();
                p.Workbook.Properties.Author = "Automated Integration Technologies";

                // Extract column headers from the mappers dictionary
                var headers = mappers.Keys.Select(x => x).ToList();

                // Add a worksheet to the Excel package
                var ws = p.Workbook.Worksheets.Add(_localizer[workSheetName]);
                ws.Name = sheetName;
                ws.Cells.Style.Font.Size = 11;
                ws.Cells.Style.Font.Name = "Calibri";

                // Apply visual styles to column headers
                SetHeaderStyles(ws, headers);

                // Populate main data in the worksheet
                PopulateData(ws, data, mappers, headers);

                // Apply AutoFilter to the header row
                ws.Cells[1, 1, 1, mappers.Count].AutoFilter = true;

                // Define the range of cells for the main data
                var startCell = ws.Cells["A1"];
                var endCell = ws.Cells[ws.Dimension.End.Row, ws.Dimension.End.Column];
                var dataRange = ws.Cells[startCell.Start.Row, startCell.Start.Column, endCell.End.Row, endCell.End.Column];

                // AutoFit columns for the main data range
                dataRange.AutoFitColumns();

                // Hide empty columns
                HideEmptyColumns(ws);

                // Create pivot tables


                // Adjust specific column width and wrap text as needed
                //ws.Column(1).Width = 100;
                //ws.Column(1).Style.WrapText = true;

                if ((dataRange.End.Row - dataRange.Start.Row) > 1)
                {
                    var pt1 = await CreatePaymentSummaryPivotTable(p, dataRange);

                    var pt2 = await CreateProcedureCodePivotTable(p, dataRange);

                }

                var exceptionReasonColumnIndex = headers.IndexOf("Exception Reason") + 1;
                if (exceptionReasonColumnIndex > 0)
                {
                    ws.Column(exceptionReasonColumnIndex).AutoFit(10, 75); // Min width = 10, Max width = 75 characters
                }

                // Convert the Excel package to a byte array with optional password protection
                var byteArray = await p.GetAsByteArrayAsync(passwordString);
                return Convert.ToBase64String(byteArray);
            }
            catch (Exception ex)
            {
                // Log or handle the exception as needed
                throw;
            }
        }

        private ExcelWorksheet CreateAndFormatWorksheet(ExcelPackage p, Dictionary<string, Func<ExportQueryResponse, object>> mappers, string sheetName, string workSheetName)
        {
            // Add a worksheet to the Excel package
            var ws = p.Workbook.Worksheets.Add(_localizer[workSheetName]);
            ws.Name = sheetName;
            ws.Cells.Style.Font.Size = 11;
            ws.Cells.Style.Font.Name = "Calibri";

            // Apply visual styles to column headers
            SetHeaderStyles(ws, mappers.Keys.ToList());

            return ws;
        }


        private static async Task<ExcelPivotTable> CreatePaymentSummaryPivotTable(ExcelPackage p, ExcelRangeBase dataRange)
        {
            try
            {
                // Create a new worksheet for the pivot table summary
                var wsPivot = p.Workbook.Worksheets.Add(StoredProcedureColumnsHelper.Payment_Summary);
                // Add a pivot table to the worksheet, starting from cell A1, using the specified data range, and naming it "PerPayer"
                var pivotTable = wsPivot.PivotTables.Add(wsPivot.Cells["A1"], dataRange, "PerPayer");
                // Add the "Payer Name" field to the row fields of the pivot table
                var rowField = pivotTable.RowFields.Add(pivotTable.Fields[StoredProcedureColumnsHelper.Payer_Name]);
                // Get a reference to the "Payer Name" field in the pivot table
                var payerNameField = pivotTable.Fields[StoredProcedureColumnsHelper.Payer_Name];
                // Add the "Quantity" field as a data field to the pivot table
                var countField = pivotTable.DataFields.Add(pivotTable.Fields[StoredProcedureColumnsHelper.Quantity]);
                // Add the "Billed Amount" field as a data field to the pivot table
                var billedAmtField = pivotTable.DataFields.Add(pivotTable.Fields[StoredProcedureColumnsHelper.Billed_Amt]);
                // Add the "Deductible Amount" field as a data field to the pivot table
                var deductibleAmtField = pivotTable.DataFields.Add(pivotTable.Fields[StoredProcedureColumnsHelper.Deductible_Amt]);
                var copayAmtField = pivotTable.DataFields.Add(pivotTable.Fields[StoredProcedureColumnsHelper.Copay_Amt]);
                var coinsuranceAmtField = pivotTable.DataFields.Add(pivotTable.Fields[StoredProcedureColumnsHelper.Coinsurance_Amt]);
                var penalityAmtField = pivotTable.DataFields.Add(pivotTable.Fields[StoredProcedureColumnsHelper.Penality_Amt]);
                var lineItemPaidAmtField = pivotTable.DataFields.Add(pivotTable.Fields[StoredProcedureColumnsHelper.Lineitem_Paid_Amt]);
                var allowedAmtField = pivotTable.DataFields.Add(pivotTable.Fields[StoredProcedureColumnsHelper.Allowed_Amt]);
                var avgAllowedAmtField = pivotTable.DataFields.Add(pivotTable.Fields[StoredProcedureColumnsHelper.Allowed_Amt]);
                avgAllowedAmtField.Name = "Avg Allowed Amt";
                avgAllowedAmtField.Function = DataFieldFunctions.Average;
                var avgLineItemPaidAmtField = pivotTable.DataFields.Add(pivotTable.Fields[StoredProcedureColumnsHelper.Lineitem_Paid_Amt]);
                avgLineItemPaidAmtField.Name = "Avg Lineitem Paid Amt";
                avgLineItemPaidAmtField.Function = DataFieldFunctions.Average;
                // Specify the format for the "Quantity" field in the pivot table
                countField.Format = "#,##0";
                countField.Name = "Count of Line Item Status";
                // Specify the format for the "Allowed Amount" field in the pivot table
                billedAmtField.Format = "$#,##0.00";
                billedAmtField.Name = "Sum of Billed Amt";
                // Specify the format for the "Amount" field in the pivot table
                deductibleAmtField.Format = "$#,##0.00";
                deductibleAmtField.Name = "Sum of LineItem Paid Amt";
                avgAllowedAmtField.Format = "$#,##0.00";
                avgLineItemPaidAmtField.Format = "$#,##0.00";

                // Set the data layout to display values on rows in the pivot table
                pivotTable.DataOnRows = false;

                // Ensure all row fields are collapsed by default
                rowField.Items.Refresh();          // Load the pivot items from the source data
                rowField.Items.ShowDetails(false); // Collapse all items

                return pivotTable;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private static async Task<ExcelPivotTable> CreateProcedureCodePivotTable(ExcelPackage p, ExcelRangeBase dataRange)
        {
            try
            {
                var psPivot = p.Workbook.Worksheets.Add(StoredProcedureColumnsHelper.Procedure_Code_Summary);
                var pivotTable2 = psPivot.PivotTables.Add(psPivot.Cells["A1"], dataRange, "PerProcedure");

                // Add the "CPT Code" field to the row fields of the pivot table
                var rowField =  pivotTable2.RowFields.Add(pivotTable2.Fields[StoredProcedureColumnsHelper.CPT_Code]);

                // Add the "CPT Code" field as a data field to count the number of times each CPT code was billed
                var cptCodeCountField = pivotTable2.DataFields.Add(pivotTable2.Fields[StoredProcedureColumnsHelper.CPT_Code]);
                cptCodeCountField.Name = "CPT Code Count";
                cptCodeCountField.Function = DataFieldFunctions.Count;
                cptCodeCountField.Format = "#,##0";

                // Add the "Quantity" field as a data field to the pivot table
                var quantityField = pivotTable2.DataFields.Add(pivotTable2.Fields[StoredProcedureColumnsHelper.Quantity]);
                quantityField.Name = "Quantity";
                quantityField.Function = DataFieldFunctions.Sum;
                quantityField.Format = "#,##0";

                // Add the "Allowed Amount" field for total sum
                var allowedAmtField = pivotTable2.DataFields.Add(pivotTable2.Fields[StoredProcedureColumnsHelper.Allowed_Amt]);
                allowedAmtField.Name = "Sum of Allowed Amt";
                allowedAmtField.Function = DataFieldFunctions.Sum;
                allowedAmtField.Format = "$#,##0.00";

                var avgAllowedAmtField = pivotTable2.Fields.AddCalculatedField("Avg Allowed Amt", $"={StoredProcedureColumnsHelper.Allowed_Amt}/{StoredProcedureColumnsHelper.Quantity}");
                avgAllowedAmtField.Name = "Allowed Amt";

                // Automatically add the calculated field to DataFields
                var avgDataField = pivotTable2.DataFields.Add(avgAllowedAmtField);
                avgDataField.Function = DataFieldFunctions.Average;
                avgDataField.Format = "#,##0.00";


                // Set the data layout to display values on rows in the pivot table
                pivotTable2.DataOnRows = false;

                // Ensure all row fields are collapsed by default
                rowField.Items.Refresh();          // Load the pivot items from the source data
                rowField.Items.ShowDetails(false); // Collapse all items

                return pivotTable2;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public async Task<string> CreateChargesByPayerAndSummarySheets(List<IEnumerable<ExportQueryResponse>> exportDetails, List<Dictionary<string, Func<ExportQueryResponse, object>>> mapperList, List<string> sheetNames, bool boldLastRow = false, bool applyBoldRowInFirstDataModel = true, bool applyBoldHeader = false, string passwordString = null, Func<ExportQueryResponse, object> groupByKeySelector = null, bool hasGroupByKeySelector = default(bool))
        {
            try
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                ExcelPackage p = new();
                p.Workbook.Properties.Author = ReportHelper.AITAuthor;

                for (int exportIndex = 0; exportIndex < exportDetails.Count(); exportIndex++)
                {
                    try
                    {

                        var exportData = exportDetails[exportIndex];
                        var mappers = mapperList[exportIndex];
                        string sheetName = sheetNames[exportIndex] ?? $"Sheet{exportIndex + 1}";

                        AddDataInWorkSheet(exportData, mappers, sheetName, applyBoldRowInFirstDataModel, applyBoldHeader, boldLastRow, ref p, groupByKeySelector, hasGroupByKeySelector);

                    }
                    catch (Exception e)
                    {
                        throw;
                    }
                }

                // Generate pivot tables if data is present
                if (p.Workbook.Worksheets.Count > 0)
                {
                    var dataRange = GetDataRange(p.Workbook.Worksheets[0]);

                    if ((dataRange.End.Row - dataRange.Start.Row) > 1)
                    {
                        await GeneratePivotTablesForChargesByPayerReport(p, dataRange);
                        p.Workbook.Worksheets.MoveAfter(StoredProcedureColumnsHelper.Summary_Of_Status, p.Workbook.Worksheets[0].Name);
                        p.Workbook.Worksheets.MoveAfter(StoredProcedureColumnsHelper.Summary_Of_Payer, StoredProcedureColumnsHelper.Summary_Of_Status);
                    }
                }

                byte[] byteArray = await p.GetAsByteArrayAsync(passwordString);
                return Convert.ToBase64String(byteArray);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        /// <summary>
        /// Generates pivot tables for the Charges By Payer report.
        /// </summary>
        /// <param name="package">The Excel package to which the pivot tables will be added.</param>
        /// <param name="dataRange">The range of data to be used in the pivot tables.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        private static async Task GeneratePivotTablesForChargesByPayerReport(ExcelPackage package, ExcelRangeBase dataRange)
        {
            // Define configurations for the pivot tables
            List<ExcelPivotTableConfiguration> configs = new List<ExcelPivotTableConfiguration>()
            {
                new ExcelPivotTableConfiguration(
                    StoredProcedureColumnsHelper.Summary_Of_Status,
                    StoredProcedureColumnsHelper.Summary_Of_Status,
                    ReportHelper.PerStatus,
                    ConfigurePivotTableForStatus),
                new ExcelPivotTableConfiguration(
                    StoredProcedureColumnsHelper.Summary_Of_Payer,
                    StoredProcedureColumnsHelper.Summary_Of_Payer,
                    ReportHelper.PerPayer,
                    ConfigurePivotTableForPayer)
            };

            // Generate the pivot tables based on the configurations
            await GeneratePivotTables(package, dataRange, configs);
        }

        /// <summary>
        /// Configures the pivot table for the status summary.
        /// </summary>
        /// <param name="pivotTable">The pivot table to configure.</param>
        private static void ConfigurePivotTableForStatus(ExcelPivotTable pivotTable)
        {
            // Define data fields for the status pivot table
            List<PivotTableDataField> dataFields = new List<PivotTableDataField>()
            {
                new PivotTableDataField(StoredProcedureColumnsHelper.Quantity, "#,##0", ReportHelper.Count_of_Lineitem_Status, PivotTableFieldType.DataField),
                new PivotTableDataField(StoredProcedureColumnsHelper.Billed_Amt, "$#,##0.00", ReportHelper.Sum_of_Billed_Amt, PivotTableFieldType.DataField),
            };

            // Configure the pivot table with the status-specific settings
            ConfigurePivotTable(
                pivotTable,
                StoredProcedureColumnsHelper.Lineitem_Status,
                dataFields);
        }

        /// <summary>
        /// Configures the pivot table for the payer summary.
        /// </summary>
        /// <param name="pivotTable">The pivot table to configure.</param>
        private static void ConfigurePivotTableForPayer(ExcelPivotTable pivotTable)
        {
            // Define data fields for the payer pivot table
            List<PivotTableDataField> dataFields = new List<PivotTableDataField>()
            {
                new PivotTableDataField(StoredProcedureColumnsHelper.Quantity, "#,##0", ReportHelper.Count_of_Billed_Amt, PivotTableFieldType.DataField),
                new PivotTableDataField(StoredProcedureColumnsHelper.Billed_Amt, "$#,##0.00", ReportHelper.Sum_of_Billed_Amt, PivotTableFieldType.DataField),
                new PivotTableDataField(StoredProcedureColumnsHelper.Allowed_Amt, "$#,##0.00", ReportHelper.Sum_of_Allowed_Amt, PivotTableFieldType.DataField)
            };

            // Configure the pivot table with the payer-specific settings
            ConfigurePivotTable(
                pivotTable,
                StoredProcedureColumnsHelper.Payer_Name,
                dataFields);
        }

        #region claim status report
        public async Task<string> CreateClaimStatusReport(
                                    IEnumerable<ExportQueryResponse> data,
                                    Dictionary<string, Func<ExportQueryResponse, object>> mapperFunc,
                                    string sheetName,
                                    string passwordString = null,
                                    Func<ExportQueryResponse, object> groupByKeySelector = null,
                                    bool hasGroupByKeySelector = default(bool))
        {
            try
            {
                // Set the license context for the ExcelPackage
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                // Create a new Excel package
                ExcelPackage p = new();

                // Set the author property for the workbook
                p.Workbook.Properties.Author = ReportHelper.AITAuthor;

                // Flag to determine if the last row should be bolded
                bool boldLastRow = false;

                // Add data to the worksheet
                // `AddDataInWorkSheet` populates the sheet with data based on the provided mapper functions and other parameters
                AddDataInWorkSheet(data, mapperFunc, sheetName, true, true, boldLastRow, ref p, groupByKeySelector, hasGroupByKeySelector);

                // Check if there are any worksheets in the workbook
                if (p.Workbook.Worksheets.Count > 0)
                {
                    // Get the data range of the first worksheet
                    var dataRange = GetDataRange(p.Workbook.Worksheets[0]);

                    // If the data range has more than one row, generate pivot tables
                    if ((dataRange.End.Row - dataRange.Start.Row) > 1)
                    {
                        // Asynchronously generate pivot tables based on the data
                        await GeneratePivotTablesForClaimStatusReport(p, dataRange);

                        // Move the worksheet with the pivot tables to the start
                        p.Workbook.Worksheets.MoveToStart(ReportHelper.Payer_Summary);
                    }
                }

                // Convert the Excel package to a byte array, applying a password if provided
                byte[] byteArray = await p.GetAsByteArrayAsync(passwordString);

                // Return the byte array as a base64-encoded string
                return Convert.ToBase64String(byteArray);
            }
            catch (Exception ex)
            {
                // Log or handle the exception as necessary
                // For now, just rethrow it
                throw;
            }
        }

        /// <summary>
        /// Generates pivot tables for the claim status report based on the provided Excel package and data range.
        /// </summary>
        /// <param name="package">The Excel package containing the workbook where the pivot tables will be added.</param>
        /// <param name="dataRange">The range of data in the worksheet to be used for creating the pivot tables.</param>
        /// <returns>A task representing the asynchronous operation of generating pivot tables.</returns>
        private static async Task GeneratePivotTablesForClaimStatusReport(ExcelPackage package, ExcelRangeBase dataRange)
        {
            // Define configurations for the pivot tables
            // Each configuration specifies the name of the sheet, the pivot table's name, and a configuration method
            List<ExcelPivotTableConfiguration> configs = new List<ExcelPivotTableConfiguration>()
            {
                new ExcelPivotTableConfiguration(
                    ReportHelper.Payer_Summary, // Sheet name where the pivot table will be created
                    ReportHelper.Payer_Summary, // Pivot table name
                    ReportHelper.PerPayer, // Data source name (can be used for specific data range settings)
                    ConfigurePivotTableForClaimStatusPayerSummary) // Method to configure the pivot table
            };

            // Generate the pivot tables based on the configurations
            // This is an asynchronous operation
            await GeneratePivotTables(package, dataRange, configs);
        }

        /// <summary>
        /// Configures the pivot table for the payer summary in the claim status report.
        /// </summary>
        /// <param name="pivotTable">The pivot table object to be configured.</param>
        private static void ConfigurePivotTableForClaimStatusPayerSummary(ExcelPivotTable pivotTable)
        {
            // Define data fields for the payer pivot table
            // Each field represents a column to be included in the pivot table, including its formatting and aggregation function
            List<PivotTableDataField> dataFields = new List<PivotTableDataField>()
            {
                new PivotTableDataField(StoredProcedureColumnsHelper.Billed_Amt, "$#,##0.00", ReportHelper.Sum_of_Billed_Amt, PivotTableFieldType.DataField),
                new PivotTableDataField(StoredProcedureColumnsHelper.Allowed_Amt, "$#,##0.00", ReportHelper.Sum_of_Allowed_Amt, PivotTableFieldType.DataField),
                new PivotTableDataField(StoredProcedureColumnsHelper.Line_Item_Paid_Amount, "$#,##0.00", ReportHelper.Sum_of_Lineitem_Paid_Amt, PivotTableFieldType.DataField),
                new PivotTableDataField(StoredProcedureColumnsHelper.Non_Allowed_Paid_Amt, "$#,##0.00", ReportHelper.Sum_of_NonAllowed_Paid_Amt, PivotTableFieldType.DataField),
                new PivotTableDataField(StoredProcedureColumnsHelper.Deductible_Amt, "$#,##0.00", ReportHelper.Sum_of_Deductible_Amt, PivotTableFieldType.DataField),
                new PivotTableDataField(StoredProcedureColumnsHelper.Copay_Amt, "$#,##0.00", ReportHelper.Sum_of_Copay_Amt, PivotTableFieldType.DataField),
                new PivotTableDataField(StoredProcedureColumnsHelper.Penality_Amt, "$#,##0.00", ReportHelper.Sum_of_Penality_Amt, PivotTableFieldType.DataField),
            };

            // Configure the pivot table with the payer-specific settings
            // This includes setting the row fields and the data fields to be summarized
            ConfigurePivotTable(
                pivotTable,
                StoredProcedureColumnsHelper.Payer_Name, // Field to group data by (rows in the pivot table)
                dataFields); // List of fields to be summarized and formatted in the pivot table
        }

        public async Task<string> CreateClaimStatusDenialsReport(
                                    IEnumerable<ExportQueryResponse> data,
                                    Dictionary<string, Func<ExportQueryResponse, object>> mapperFunc,
                                    string sheetName,
                                    string passwordString = null,
                                    Func<ExportQueryResponse, object> groupByKeySelector = null,
                                    bool hasGroupByKeySelector = default(bool))
        {
            try
            {
                // Set the license context for the ExcelPackage to non-commercial use
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                // Create a new Excel package
                ExcelPackage p = new();

                // Set the author property for the workbook
                p.Workbook.Properties.Author = ReportHelper.AITAuthor;

                // Flag to determine if the last row should be bolded
                bool boldLastRow = false;

                // Populate the worksheet with data based on the provided mapper functions and parameters
                AddDataInWorkSheet(data, mapperFunc, sheetName, true, true, boldLastRow, ref p, groupByKeySelector, hasGroupByKeySelector);

                // Check if any worksheets have been created
                if (p.Workbook.Worksheets.Count > 0)
                {
                    // Get the data range of the first worksheet
                    var dataRange = GetDataRange(p.Workbook.Worksheets[0]);

                    // Generate pivot tables if the data range contains more than one row
                    if ((dataRange.End.Row - dataRange.Start.Row) > 1)
                    {
                        // Asynchronously generate pivot tables based on the defined configurations
                        await GeneratePivotTablesForClaimStatusDenialsReport(p, dataRange);

                        // Move the worksheet with the pivot tables to the start and position it after other relevant worksheets
                        p.Workbook.Worksheets.MoveToStart(ReportHelper.Denial_Summary);
                        p.Workbook.Worksheets.MoveAfter(ReportHelper.Payer_Denial_Summary, ReportHelper.Denial_Summary);
                    }
                }

                // Convert the Excel package to a byte array and apply a password if provided
                byte[] byteArray = await p.GetAsByteArrayAsync(passwordString);

                // Return the byte array as a base64-encoded string
                return Convert.ToBase64String(byteArray);
            }
            catch (Exception ex)
            {
                // Log or handle the exception as necessary
                // For now, just rethrow the exception
                throw;
            }
        }

        /// <summary>
        /// Generates pivot tables for the claim status denials report based on the provided Excel package and data range.
        /// </summary>
        /// <param name="package">The Excel package containing the workbook where the pivot tables will be added.</param>
        /// <param name="dataRange">The range of data in the worksheet to be used for creating the pivot tables.</param>
        /// <returns>A task representing the asynchronous operation of generating pivot tables.</returns>
        private static async Task GeneratePivotTablesForClaimStatusDenialsReport(ExcelPackage package, ExcelRangeBase dataRange)
        {
            // Define configurations for the pivot tables
            // Each configuration specifies the sheet name, pivot table name, data source name, and configuration method
            List<ExcelPivotTableConfiguration> configs = new List<ExcelPivotTableConfiguration>()
            {
                new ExcelPivotTableConfiguration(
                    ReportHelper.Denial_Summary, // Sheet name where the pivot table will be created
                    ReportHelper.Denial_Summary, // Pivot table name
                    ReportHelper.Exception_Category, // Data source name for specific settings
                    ConfigurePivotTableForClaimStatusDenialSummary), // Method to configure the pivot table
                new ExcelPivotTableConfiguration(
                    ReportHelper.Payer_Denial_Summary, // Sheet name for the second pivot table
                    ReportHelper.Payer_Denial_Summary, // Pivot table name for the second pivot table
                    ReportHelper.PerPayer, // Data source name for the second pivot table
                    ConfigurePivotTableForClaimStatusPayerDenialSummary) // Method to configure the second pivot table
            };

            // Generate the pivot tables based on the provided configurations
            // This is an asynchronous operation
            await GeneratePivotTables(package, dataRange, configs);
        }

        /// <summary>
        /// Configures the pivot table for the denial summary in the claim status report.
        /// </summary>
        /// <param name="pivotTable">The pivot table object to be configured.</param>
        private static void ConfigurePivotTableForClaimStatusDenialSummary(ExcelPivotTable pivotTable)
        {
            // Define data fields for the denial summary pivot table
            // Each field represents a column to be included in the pivot table, with formatting and aggregation function
            List<PivotTableDataField> dataFields = new List<PivotTableDataField>()
            {
                new PivotTableDataField(StoredProcedureColumnsHelper.Quantity, "$#,##0.00", ReportHelper.Count_of_Exception_Category, PivotTableFieldType.DataField),
                new PivotTableDataField(StoredProcedureColumnsHelper.Billed_Amt, "$#,##0.00", ReportHelper.Sum_of_Billed_Amt, PivotTableFieldType.DataField)
            };

            // Configure the pivot table with the specified row field and data fields
            ConfigurePivotTable(
                pivotTable,
                StoredProcedureColumnsHelper.Exception_Category, // Field to group data by (rows in the pivot table)
                dataFields); // List of fields to be summarized and formatted in the pivot table
        }

        /// <summary>
        /// Configures the pivot table for the payer denial summary in the claim status report.
        /// </summary>
        /// <param name="pivotTable">The pivot table object to be configured.</param>
        private static void ConfigurePivotTableForClaimStatusPayerDenialSummary(ExcelPivotTable pivotTable)
        {
            // Define data fields for the payer denial summary pivot table
            // Each field represents a column to be included in the pivot table, with formatting and aggregation function
            List<PivotTableDataField> dataFields = new List<PivotTableDataField>()
            {
                new PivotTableDataField(StoredProcedureColumnsHelper.Quantity, "$#,##0.00", ReportHelper.Count_of_Payer_Name, PivotTableFieldType.DataField),
                new PivotTableDataField(StoredProcedureColumnsHelper.Billed_Amt, "$#,##0.00", ReportHelper.Sum_of_Billed_Amt, PivotTableFieldType.DataField)
            };

            // Configure the pivot table with the specified row field and data fields
            ConfigurePivotTable(
                pivotTable,
                StoredProcedureColumnsHelper.Payer_Name, // Field to group data by (rows in the pivot table)
                dataFields); // List of fields to be summarized and formatted in the pivot table
        }

        #endregion

        #region generic method
        public async Task<string> CreateReport(ReportCreationOptions options)
        {
            try
            {
                // Set the license context for the ExcelPackage to non-commercial use
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                // Create a new Excel package
                ExcelPackage p = new();

                // Set the author property for the workbook
                p.Workbook.Properties.Author = ReportHelper.AITAuthor;

                // Add data to the worksheet
                AddDataInWorkSheet(options.Data, options.MapperFunc, options.SheetName, options.ApplyBoldRowInFirstDataModel, options.ApplyBoldHeader, options.BoldLastRow, ref p, options.GroupByKeySelector, options.HasGroupByKeySelector);

                // Check if there are any worksheets in the workbook
                if (p.Workbook.Worksheets.Count > 0)
                {
                    // Get the data range of the first worksheet
                    var dataRange = GetDataRange(p.Workbook.Worksheets[0]);

                    // Generate pivot tables if configurations are provided and data range contains more than one row
                    if (options.PivotTableConfigurations != null && (dataRange.End.Row - dataRange.Start.Row) > 1)
                    {
                        // Asynchronously generate pivot tables based on the provided configurations
                        await GeneratePivotTables(p, dataRange, options.PivotTableConfigurations);

                        // Optionally reorder the sheets if ordering is specified
                        if (options.PivotTableSheetOrdering != null)
                        {
                            foreach (var ordering in options.PivotTableSheetOrdering)
                            {
                                p.Workbook.Worksheets.MoveToStart(ordering.Key);
                                if (ordering.Value != null)
                                {
                                    p.Workbook.Worksheets.MoveAfter(ordering.Value, ordering.Key);
                                }
                            }
                        }
                    }
                }

                // Convert the Excel package to a byte array and apply a password if provided
                byte[] byteArray = await p.GetAsByteArrayAsync(options.PasswordString);

                // Return the byte array as a base64-encoded string
                return Convert.ToBase64String(byteArray);
            }
            catch (Exception ex)
            {
                // Log or handle the exception as necessary
                // For now, just rethrow the exception
                throw;
            }
        }

        #endregion

        #region Helper methods

        /// <summary>
        /// Generates pivot tables based on the provided configurations.
        /// </summary>
        /// <param name="package">The Excel package to which the pivot tables will be added.</param>
        /// <param name="dataRange">The range of data to be used in the pivot tables.</param>
        /// <param name="configs">The list of pivot table configurations.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        private static async Task GeneratePivotTables(ExcelPackage package, ExcelRangeBase dataRange, List<ExcelPivotTableConfiguration> configs)
        {
            foreach (var config in configs)
            {
                await CreatePivotTableAsync(package, config, dataRange);
            }
        }

        /// <summary>
        /// Creates a pivot table asynchronously based on the provided configuration.
        /// </summary>
        /// <param name="package">The Excel package to which the pivot table will be added.</param>
        /// <param name="configuration">The configuration for the pivot table.</param>
        /// <param name="dataRange">The range of data to be used in the pivot table.</param>
        /// <returns>A task that represents the asynchronous operation, with the pivot table as its result.</returns>
        private static async Task<ExcelPivotTable> CreatePivotTableAsync(
            ExcelPackage package,
            ExcelPivotTableConfiguration configuration,
            ExcelRangeBase dataRange)
        {
            try
            {
                // Add a new worksheet and create the pivot table
                var worksheet = package.Workbook.Worksheets.Add(configuration.SheetTitle);
                var pivotTable = worksheet.PivotTables.Add(worksheet.Cells["A1"], dataRange, configuration.PivotTableTitle);
                configuration.ConfigurePivotTable(pivotTable);
                return pivotTable;
            }
            catch (Exception ex)
            {
                // Handle exceptions and throw a specific application exception
                throw new ApplicationException(ReportHelper.PivotTableCreationError, ex);
            }
        }

        /// <summary>
        /// Configures the pivot table with the specified row field and data fields.
        /// </summary>
        /// <param name="pivotTable">The pivot table to configure.</param>
        /// <param name="rowFieldName">The name of the field to use as the row field.</param>
        /// <param name="dataFields">The list of data fields to add to the pivot table.</param>
        /// <param name="dataOnRows">Whether the data fields should be placed on rows (default is false).</param>
        private static void ConfigurePivotTable(
            ExcelPivotTable pivotTable,
            string rowFieldName,
            List<PivotTableDataField> dataFields,
            bool dataOnRows = false)
        {
            // Add the primary row field to the pivot table
            var rowField = pivotTable.RowFields.Add(pivotTable.Fields[rowFieldName]);
            var cptField = pivotTable.RowFields.Add(pivotTable.Fields[StoredProcedureColumnsHelper.CPT_Code]);

            // Add and configure each data field
            foreach (var df in dataFields)
            {
                var field = pivotTable.DataFields.Add(pivotTable.Fields[df.FieldName]);
                field.Name = df.FieldHeader;
                field.Format = df.Format;
            }

            // Set whether the data fields should be displayed on rows
            pivotTable.DataOnRows = dataOnRows;

            // Ensure all row fields are collapsed by default
            rowField.Items.Refresh();  // Load the pivot items from the source data
            rowField.Items.ShowDetails(false); // Collapse all items

            cptField.Items.Refresh();
            cptField.Items.ShowDetails(false);
        }

        /// <summary>
        /// This returns the date range from the worksheet to be filled into the additional info sheets or the summary sheets
        /// </summary>
        /// <param name="worksheet"></param>
        /// <returns></returns>
        private static ExcelRangeBase GetDataRange(ExcelWorksheet worksheet)
        {
            var startCell = worksheet.Cells["A1"];
            var endCell = worksheet.Cells[worksheet.Dimension.End.Row, worksheet.Dimension.End.Column];
            return worksheet.Cells[startCell.Start.Row, startCell.Start.Column, endCell.End.Row, endCell.End.Column];
        }
        #endregion

        public async Task<string> ExportReportAsync<T>( IEnumerable<T> data,Dictionary<string, Func<T, object>> mappers,  string sheetName = "Sheet1",string passwordString = null)
        {
            if (data == null || !data.Any() || mappers == null || !mappers.Any())
                throw new ArgumentException("Invalid data or mappers provided.");

            // Set the license context for EPPlus
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add(sheetName);

            // Add headers with formatting
            int colIndex = 1;
            foreach (var header in mappers.Keys)
            {
                var cell = worksheet.Cells[1, colIndex];
                cell.Value = header;
                cell.Style.Font.Bold = true;
                cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
                cell.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                colIndex++;
            }

            // Add data rows
            int rowIndex = 2;
            foreach (var item in data)
            {
                colIndex = 1;
                foreach (var map in mappers.Values)
                {
                    var value = map(item);
                    var cell = worksheet.Cells[rowIndex, colIndex];

                    // Format the cell based on value
                    ExportHelpers.SetCellFormat(cell, value);
                    colIndex++;
                }
                rowIndex++;
            }

            // AutoFit columns with minimum and maximum widths
            worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns(10, 25);

            // Use the reusable HideEmptyColumns method to hide empty columns
            HideEmptyColumns(worksheet);

            // Protect the worksheet if a password is provided
            if (!string.IsNullOrEmpty(passwordString))
            {
                byte[] bytes = await package.GetAsByteArrayAsync(passwordString);
                return Convert.ToBase64String(bytes);
            }

            // Convert the Excel package to a Base64 string
            byte[] byteArray = await package.GetAsByteArrayAsync();
            return Convert.ToBase64String(byteArray);
        }
    }
}