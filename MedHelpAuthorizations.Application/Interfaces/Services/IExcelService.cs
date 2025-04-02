using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base;
using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.ExportReport;
using MedHelpAuthorizations.Application.Features.Reports.CurrentSummary;
using System.Collections.Generic;

namespace MedHelpAuthorizations.Application.Interfaces.Services
{
    public interface IExcelService
    {
        /// <summary>
        /// Exports data to a single worksheet with customizable columns, optional grouping, and password protection.
        /// Allows for flexible column mapping through a provided dictionary of mappers, and handles denial reasons and grouping if specified.
        /// </summary>
        /// <param name="data">The data to export.</param>
        /// <param name="mappers">A dictionary mapping column names to functions that extract values from the data.</param>
        /// <param name="sheetName">The name of the sheet to export data to.</param>
        /// <param name="workSheetName">The name of the worksheet.</param>
        /// <param name="passwordString">An optional password for protecting the worksheet.</param>
        /// <param name="isFirstColDenailReason">Indicates if the first column should handle denial reasons.</param>
        /// <param name="groupByKeySelector">An optional function to group data by a specific key.</param>
        /// <param name="hasGroupByKeySelector">Indicates if the grouping by key selector is used.</param>
        /// <returns>A task representing the asynchronous operation, with a string result indicating the export status.</returns>
        Task<string> ExportAsync(
            IEnumerable<ExportQueryResponse> data,
            Dictionary<string, Func<ExportQueryResponse, object>> mappers,
            string sheetName = "Sheet1",
            string workSheetName = "Worksheet1",
            string passwordString = null,
            bool isFirstColDenailReason = false,
            Func<ExportQueryResponse, object> groupByKeySelector = null,
            bool hasGroupByKeySelector = default(bool));

        /// <summary>
        /// Exports multiple datasets to separate sheets within a single workbook.
        /// Provides options for bold formatting of headers and the last row, and supports column mapping and grouping for each dataset.
        /// </summary>
        /// <param name="exportDetails">A list of datasets to export.</param>
        /// <param name="mapperList">A list of dictionaries mapping column names to functions that extract values from the data.</param>
        /// <param name="sheetNames">A list of names for the sheets in the workbook.</param>
        /// <param name="boldLastRow">Indicates if the last row of each sheet should be bolded.</param>
        /// <param name="applyBoldRowInFirstDataModel">Indicates if bold formatting should be applied to the first data model.</param>
        /// <param name="applyBoldHeader">Indicates if the headers should be bolded.</param>
        /// <param name="passwordString">An optional password for protecting the workbook.</param>
        /// <param name="groupByKeySelector">An optional function to group data by a specific key.</param>
        /// <param name="hasGroupByKeySelector">Indicates if the grouping by key selector is used.</param>
        /// <returns>A task representing the asynchronous operation, with a string result indicating the export status.</returns>
        Task<string> ExportMultipleSheetsAsync(
            List<IEnumerable<ExportQueryResponse>> exportDetails,
            List<Dictionary<string, Func<ExportQueryResponse, object>>> mapperList,
            List<string> sheetNames,
            bool boldLastRow = false,
            bool applyBoldRowInFirstDataModel = true,
            bool applyBoldHeader = false,
            string passwordString = null,
            Func<ExportQueryResponse, object> groupByKeySelector = null,
            bool hasGroupByKeySelector = default(bool));

        /// <summary>
        /// Exports custom report data into multiple tabs within a single worksheet, based on string data and custom headers.
        /// Returns the result as a byte array for further use.
        /// </summary>
        /// <param name="exportReportData">Report details in string format.</param>
        /// <param name="headerList">A list of custom headers for each report tab.</param>
        /// <param name="sheetNames">A list of names for the report sheets.</param>
        /// <returns>A task representing the asynchronous operation, with a string result indicating the export status.</returns>
        Task<string> ExportMultipleCustomReportTabsInWorksheet(
            List<string> exportReportData,
            List<List<string>> headerList,
            List<string> sheetNames);

        /// <summary>
        /// Exports data to a worksheet and optionally includes a pivot table.
        /// Supports customizable column mapping and grouping, with optional password protection for the worksheet.
        /// </summary>
        /// <param name="data">The data to export.</param>
        /// <param name="mappers">A dictionary mapping column names to functions that extract values from the data.</param>
        /// <param name="sheetName">The name of the sheet to export data to.</param>
        /// <param name="workSheetName">The name of the worksheet.</param>
        /// <param name="passwordString">An optional password for protecting the worksheet.</param>
        /// <param name="groupByKeySelector">An optional function to group data by a specific key.</param>
        /// <param name="hasGroupByKeySelector">Indicates if the grouping by key selector is used.</param>
        /// <returns>A task representing the asynchronous operation, with a string result indicating the export status.</returns>
        Task<string> ExportWithPivotTableAsync(
            IEnumerable<ExportQueryResponse> data,
            Dictionary<string, Func<ExportQueryResponse, object>> mappers,
            string sheetName = "Sheet1",
            string workSheetName = "Worksheet1",
            string passwordString = null,
            Func<ExportQueryResponse, object> groupByKeySelector = null,
            bool hasGroupByKeySelector = default(bool));

        /// <summary>
        /// Adds a type prefix to a given value, useful for formatting or categorizing values in data export scenarios.
        /// </summary>
        /// <param name="type">The type prefix to add.</param>
        /// <param name="value">The value to which the prefix will be added.</param>
        /// <returns>The value with the type prefix added.</returns>
        string AddTypePrefix(string type, string value);

        /// <summary>
        /// Creates a sheet for insurance reimbursement data and includes a pivot table.
        /// Supports customizable column mapping and optional password protection for the worksheet.
        /// </summary>
        /// <param name="data">The insurance reimbursement data to export.</param>
        /// <param name="mappers">A dictionary mapping column names to functions that extract values from the data.</param>
        /// <param name="sheetName">The name of the sheet to export data to.</param>
        /// <param name="workSheetName">The name of the worksheet.</param>
        /// <param name="passwordString">An optional password for protecting the worksheet.</param>
        /// <returns>A task representing the asynchronous operation, with a string result indicating the export status.</returns>
        Task<string> CreateInsuranceReimbursementAndPivotTableSheet(
            IEnumerable<ExportQueryResponse> data,
            Dictionary<string, Func<ExportQueryResponse, object>> mappers,
            string sheetName = "Sheet1",
            string workSheetName = "Worksheet1",
            string passwordString = null);

        /// <summary>
        /// Creates multiple sheets for charges by payer and summary data within a single workbook.
        /// Allows for detailed customization of formatting and grouping, with options for bold headers and rows, and optional password protection.
        /// </summary>
        /// <param name="exportDetails">A list of datasets to export.</param>
        /// <param name="mapperList">A list of dictionaries mapping column names to functions that extract values from the data.</param>
        /// <param name="sheetNames">A list of names for the sheets in the workbook.</param>
        /// <param name="boldLastRow">Indicates if the last row of each sheet should be bolded.</param>
        /// <param name="applyBoldRowInFirstDataModel">Indicates if bold formatting should be applied to the first data model.</param>
        /// <param name="applyBoldHeader">Indicates if the headers should be bolded.</param>
        /// <param name="passwordString">An optional password for protecting the workbook.</param>
        /// <param name="groupByKeySelector">An optional function to group data by a specific key.</param>
        /// <param name="hasGroupByKeySelector">Indicates if the grouping by key selector is used.</param>
        /// <returns>A task representing the asynchronous operation, with a string result indicating the export status.</returns>
        Task<string> CreateChargesByPayerAndSummarySheets(
            List<IEnumerable<ExportQueryResponse>> exportDetails,
            List<Dictionary<string, Func<ExportQueryResponse, object>>> mapperList,
            List<string> sheetNames,
            bool boldLastRow = false,
            bool applyBoldRowInFirstDataModel = true,
            bool applyBoldHeader = false,
            string passwordString = null,
            Func<ExportQueryResponse, object> groupByKeySelector = null,
            bool hasGroupByKeySelector = default(bool));

        /// <summary>
        /// Creates a claim status report based on the provided data, mapping functions, and configuration options.
        /// </summary>
        /// <param name="data">An enumerable collection of <see cref="ExportQueryResponse"/> objects representing the data to be included in the report.</param>
        /// <param name="mapperFunc">
        /// A dictionary where the key is a string representing the name of the column in the report, and the value is a function
        /// that maps each <see cref="ExportQueryResponse"/> object to the value for that column.
        /// </param>
        /// <param name="sheetName">The name of the sheet in the report where the data will be written.</param>
        /// <param name="passwordString">
        /// Optional. A password to protect the report file. If null, the report will not be password-protected.
        /// </param>
        /// <param name="groupByKeySelector">
        /// Optional. A function that selects the key to group the data by. If null, the data will not be grouped.
        /// </param>
        /// <param name="hasGroupByKeySelector">
        /// Optional. A boolean indicating whether the <paramref name="groupByKeySelector"/> is provided and should be used.
        /// Defaults to false.
        /// </param>
        /// <returns>A task that represents the asynchronous operation. The task result is a string containing the path to the generated report.</returns>
        /// <exception cref="ArgumentNullException">Thrown when required arguments are null.</exception>
        /// <exception cref="InvalidOperationException">Thrown if there is an error creating the report.</exception>
        Task<string> CreateClaimStatusReport(
            IEnumerable<ExportQueryResponse> data,
            Dictionary<string, Func<ExportQueryResponse, object>> mapperFunc,
            string sheetName,
            string passwordString = null,
            Func<ExportQueryResponse, object> groupByKeySelector = null,
            bool hasGroupByKeySelector = default(bool));

        /// <summary>
        /// Creates a claim status denials report in an Excel file based on the provided data, mapping functions, and configuration options.
        /// </summary>
        /// <param name="data">An enumerable collection of <see cref="ExportQueryResponse"/> objects representing the data to be included in the report.</param>
        /// <param name="mapperFunc">
        /// A dictionary where each key is a string representing the column name in the report, and each value is a function
        /// that maps an <see cref="ExportQueryResponse"/> object to the value for that column.
        /// </param>
        /// <param name="sheetName">The name of the sheet in the report where the data will be written.</param>
        /// <param name="passwordString">
        /// Optional. A password to protect the report file. If null, the report will not be password-protected.
        /// </param>
        /// <param name="groupByKeySelector">
        /// Optional. A function that selects the key to group the data by. If null, the data will not be grouped.
        /// </param>
        /// <param name="hasGroupByKeySelector">
        /// Optional. A boolean indicating whether the <paramref name="groupByKeySelector"/> is provided and should be used.
        /// Defaults to false.
        /// </param>
        /// <returns>A task that represents the asynchronous operation. The task result is a base64-encoded string containing the Excel report.</returns>
        /// <exception cref="Exception">Thrown if there is an error during report generation.</exception>
        Task<string> CreateClaimStatusDenialsReport(
            IEnumerable<ExportQueryResponse> data,
            Dictionary<string, Func<ExportQueryResponse, object>> mapperFunc,
            string sheetName,
            string passwordString = null,
            Func<ExportQueryResponse, object> groupByKeySelector = null,
            bool hasGroupByKeySelector = default(bool));

        /// <summary>
        /// Generates a report based on the provided options and returns it as a Base64-encoded string.
        /// </summary>
        /// <param name="options">An instance of <see cref="ReportCreationOptions"/> containing the configuration and data needed to generate the report.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is a Base64-encoded string representing the generated report in Excel format.</returns>
        Task<string> CreateReport(ReportCreationOptions options);

        Task<string> ExportReportAsync<T>(IEnumerable<T> data, Dictionary<string, Func<T, object>> mappers, string sheetName = "Sheet1", string passwordString = null);
    }
}