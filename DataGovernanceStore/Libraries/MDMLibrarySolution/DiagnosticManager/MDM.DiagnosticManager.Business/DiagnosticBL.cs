using System;
using SYSIO = System.IO;
using SYSREFLECTION = System.Reflection;
using System.Data;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Collections.Specialized;

namespace MDM.DiagnosticManager.Business
{
    using Data;
    using BusinessObjects;
    using BusinessObjects.Diagnostics;
    using ConfigurationManager.Business;
    using Core;
    using Utility;
    using Core.Exceptions;
    using File = BusinessObjects.File;
    using System.IO;

    /// <summary>
    /// Class to get application/system diagnostic
    /// </summary>
    public class DiagnosticBL : BusinessLogicBase
    {
        #region Fields

        /// <summary>
        /// lock excel report file when perform writing dataset to file
        /// </summary>
        private static Mutex thisLock = new Mutex(false);

        /// <summary>
        /// indicates current trace settings
        /// </summary>
        private TraceSettings _traceSettings = new TraceSettings();


        #endregion

        #region Constructors
        #endregion

        #region Properties
        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Retrieves the application related diagnostic information
        /// </summary>
        /// <param name="applicationDiagnosticType">Indicates the application diagnostic type</param>
        /// <param name="startDateTime">Indicates start date time to get information from datetime</param>
        /// <param name="entityId">Indicates the Id of an entity</param>
        /// <param name="count">Indicates how many rows to return</param>
        /// <param name="callerContext">Indicates the context of the caller</param>
        /// <returns>Returns JSON String</returns>
        public String GetApplicationDiagnostic(ApplicationDiagnosticType applicationDiagnosticType, DateTime startDateTime, Int64 entityId, Int64 count, CallerContext callerContext)
        {
            if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("DiagnosticBL.GetApplicationDiagnostic", MDMTraceSource.Application, false);

            JObject applicationDiagnostic = new JObject();

            try
            {
                DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Read);
                DiagnosticDA mdmCenterDiagnosticDA = new DiagnosticDA();

                applicationDiagnostic = mdmCenterDiagnosticDA.GetApplicationDiagnostic(applicationDiagnosticType, startDateTime, entityId, count, command);
            }
            finally
            {
                if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("DiagnosticBL.GetApplicationDiagnostic", MDMTraceSource.Application);
            }

            return (applicationDiagnostic != null) ? applicationDiagnostic.ToString() : String.Empty;
        }

        /// <summary>
        /// Retrieves the system related diagnostic information
        /// </summary>
        /// <param name="systemDiagnosticType">Indicates the system diagnostic type</param>
        /// <param name="systemDiagnosticSubType">Indicates the system diagnostic sub type</param>
        /// <param name="count">Indicates how many rows to return</param>
        /// <param name="callerContext">Indicates the context of the caller</param>
        /// <returns>Returns JSON String</returns>
        public String GetSystemDiagnostic(SystemDiagnosticType systemDiagnosticType, SystemDiagnosticSubType systemDiagnosticSubType, Int64 count, CallerContext callerContext)
        {
            if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("DiagnosticBL.GetSystemDiagnostic", MDMTraceSource.Application, false);

            JObject systemDiagnostic = new JObject();

            try
            {
                DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Read);
                DiagnosticDA mdmCenterDiagnosticDA = new DiagnosticDA();

                systemDiagnostic = mdmCenterDiagnosticDA.GetSystemDiagnostic(systemDiagnosticType, systemDiagnosticSubType, count, command);
            }
            finally
            {
                if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("DiagnosticBL.GetSystemDiagnostic", MDMTraceSource.Application);
            }

            return (systemDiagnostic != null) ? systemDiagnostic.ToString() : String.Empty;
        }

        /// <summary>
        /// Retrieves the related diagnostic record data based on context.
        /// </summary>
        /// <param name="relativeDataReferanceId">Indicates relative data reference id for diagnostic record.</param>
        /// <param name="diagnosticRelativeDataType">Indicates relative data type for diagnostic record.</param>
        /// <param name="callerContext">Indicates the context of the caller</param>
        /// <returns>returns related diagnostic record data as string</returns>
        public String GetRelatedDiagnosticRecordData(Int64 relativeDataReferanceId, DiagnosticRelativeDataType diagnosticRelativeDataType, CallerContext callerContext)
        {
            String relatedDiagnosticRecordData = String.Empty;

            try
            {
                ValidateCallerContext(callerContext);

                DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Read);
                DiagnosticDA diagnosticDA = new DiagnosticDA();

                relatedDiagnosticRecordData = diagnosticDA.GetRelatedDiagnosticRecordData(relativeDataReferanceId, diagnosticRelativeDataType, command);
            }
            finally
            {
            }

            return relatedDiagnosticRecordData;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reportType"></param>
        /// <param name="reportSubtype"></param>
        /// <param name="inputXml"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        public DiagnosticToolsReportResultWrapper ProcessDiagnosticToolsReport(DiagnosticToolsReportType reportType, DiagnosticToolsReportSubType reportSubtype, String inputXml, CallerContext callerContext)
        {
            #region tracing initialization

            _traceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
            DiagnosticActivity activity = new DiagnosticActivity();
            if (_traceSettings.IsBasicTracingEnabled)
            {
                activity.Start();
            }

        #endregion

            #region initialization

            string userName = SecurityPrincipalHelper.GetCurrentSecurityPrincipal().CurrentUserName;
            DataSet resultSet = new DataSet();
            File excelFile = new File();
            OperationResult apiOperationResult = new OperationResult();
            //TODO LOG INFO DURING EXECUTION TO apiOperationResult
            Stream excelStream;
            String fileLocation = GetTemplateFileName(true);
            String fileName = String.Format(@"{0}\{1}_{2}_{3}.xlsx", fileLocation, reportType, "cfadmin", DateTime.Now.ToString("MMddyyyy_HHmmss"));
            #endregion initialization

            try
            {

                #region Step: make call to DA

                ValidateCallerContext(callerContext);

                if (_traceSettings.IsBasicTracingEnabled)
                {
                    activity.LogVerbose("-1", String.Format("{0} making DA call with report Type: {1}", SYSREFLECTION.MethodBase.GetCurrentMethod().Name, reportType.ToString()));
                }

                DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Execute);
                DiagnosticDA mdmCenterDiagnosticDA = new DiagnosticDA();
                resultSet = mdmCenterDiagnosticDA.ProcessDiagnosticToolsReport(reportType, reportSubtype, inputXml, userName, callerContext, command);

                #endregion

                #region Step: read dataset and write excel to stream

                if (resultSet != null && resultSet.Tables.Count > 0)
                {
                    if (_traceSettings.IsBasicTracingEnabled)
                    {
                        activity.LogInformation(String.Format("Report {0} contains {1} sheets", reportSubtype.ToString(), (resultSet.Tables[0].Rows.Count - 1).ToString()));
                    }

                    excelStream = CreateExcelDocument(resultSet, fileName, true);
                    //ApplyColumnAutoFit(fileName);

                    if (_traceSettings.IsBasicTracingEnabled)
                    {
                        activity.LogInformation(String.Format("Excel Report file is written to {0}", fileName));
                    }
                }
                else
                {
                    //apiOperationResult.AddOperationResult("", String.Format( "DiagnosticBL.ProcessDiagnosticToolsReport failed to get report based on report type {0}", reportType.ToString()), OperationResultType.Error);
                    throw new Exception(String.Format("DiagnosticBL.ProcessDiagnosticToolsReport failed to get report based on {0} report type", reportType.ToString()));
                }

                #endregion read dataset and write excel

                #region Step: capture excel file from stream

                byte[] binary = null;

                if (excelStream == null)
                {
                    apiOperationResult.AddOperationResult("-1", "DiagnosticBL.CreateExcelDocument(resultSet, fileName, true) failed to generate Excel File", OperationResultType.Error);
                }
                else
                {
                    MemoryStream memoryStream = new MemoryStream();
                    using (FileStream fileStream = SYSIO.File.OpenRead(fileName))
                    {
                        fileStream.CopyTo(memoryStream);
                        binary = memoryStream.ToArray();
                    }
                }

                #endregion

                #region Step: wrap excel file and operation result then return

                apiOperationResult.RefreshOperationResultStatus();

                if (!apiOperationResult.HasError)
                {
                    apiOperationResult.OperationResultStatus  = OperationResultStatusEnum.Successful;
                }
                
                DiagnosticToolsReportResultWrapper resultWrapper =
                    new DiagnosticToolsReportResultWrapper(new File(fileName, "Excel", false, binary),
                        apiOperationResult, -1);
                return resultWrapper;

                #endregion

            }
            finally
            {
                if (_traceSettings.IsBasicTracingEnabled)
                {
                    activity.Stop();
                }
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reportType"></param>
        /// <param name="reportSubtype"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        public String GetReportTemplate(DiagnosticToolsReportType reportType, DiagnosticToolsReportSubType reportSubtype, CallerContext callerContext)
        {
            #region tracing initialization

            DiagnosticActivity activity = new DiagnosticActivity();

            _traceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();

            if (_traceSettings.IsBasicTracingEnabled)
            {
                activity.Start();
            }

            #endregion tracing initialization

            try
            {

                #region Step: Initialization

                DataSet reportTemplateDataSet = null;
                string userName = SecurityPrincipalHelper.GetCurrentSecurityPrincipal().CurrentUserName, reportTemplate = String.Empty;

                #endregion initialization

                #region Step: Make DA call

                ValidateCallerContext(callerContext);

                if (_traceSettings.IsBasicTracingEnabled)
                {
                    activity.LogVerbose("-1", String.Format("{0} making DA call with report Type: {1}", SYSREFLECTION.MethodBase.GetCurrentMethod().Name, reportType.ToString()));
                }

                DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Execute);
                DiagnosticDA mdmCenterDiagnosticDA = new DiagnosticDA();
                reportTemplateDataSet = mdmCenterDiagnosticDA.GetReportTemplate(reportType, reportSubtype, callerContext, command);

                #endregion make DA call

                #region Step: Get and return xml template from dataSet

                var reportTemplateAsEnum = from p in reportTemplateDataSet.Tables[0].AsEnumerable()
                                               select p["InputdataTemplate"];

                if (reportTemplateAsEnum.FirstOrDefault() != null)
                {
                    reportTemplate = reportTemplateAsEnum.FirstOrDefault().ToString();
                }

                return reportTemplate;

                #endregion

            }
            catch (MDMOperationException e)
            {
                activity.LogError(String.Format("Template for Report type {0} not found, Additional information: {1}", reportType.ToString(), e.Message));
                throw new Exception(e.Message);
            }

            finally
            {
                if (_traceSettings.IsBasicTracingEnabled)
                {
                    activity.Stop();
                }
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isJobServiceRequest"></param>
        /// <returns></returns>
        private static string GetTemplateFileName(bool isJobServiceRequest)
        {
            string tempFileLocation = String.Empty;
            if (isJobServiceRequest)
                tempFileLocation = AppConfigurationHelper.GetAppConfig<String>("Jobs.TemporaryFileRoot");
            else
                tempFileLocation = AppConfigurationHelper.GetAppConfig<String>("WCF.TemporaryFileRoot");

            return tempFileLocation;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="callerContext"></param>
        private void ValidateCallerContext(CallerContext callerContext)
        {
            DiagnosticActivity activity = new DiagnosticActivity();
            if (callerContext == null)
            {
                activity.LogError("CallerContext cannot be null");
                throw new MDMOperationException("111846", "CallerContext cannot be null", "MDM.DiagnosticDataBL.Business.DiagnosticBL", String.Empty, String.Empty);
            }
        }

        /// <summary>
        /// utility method for writing report data to excel
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="excelFilename"></param>
        /// <param name="includeMetadataSheet"></param>
        /// <returns></returns>
        private static Stream CreateExcelDocument(DataSet ds, string excelFilename, Boolean includeMetadataSheet)
        {
            MemoryStream outStream = new MemoryStream();
            try
            {

                #region parse dataset to excel

                thisLock.WaitOne(Timeout.Infinite, false);

                using (SpreadsheetDocument document = SpreadsheetDocument.Create(excelFilename, SpreadsheetDocumentType.Workbook))
                {
                    WriteExcelFile(ds, document, includeMetadataSheet);
                }

                #endregion parse dataset to excel

                #region attach content file to outstream

                Stream instream = SYSIO.File.OpenRead(excelFilename);
                const int BufferSize = 1024 * 10;
                byte[] buffer = new byte[BufferSize];
                int bytesRead;
                do
                {
                    bytesRead = outStream.Read(buffer, 0, buffer.Length);
                    outStream.Write(buffer, 0, bytesRead);
                }
                while (bytesRead == BufferSize);
                

                return outStream;

                #endregion attach content file to outstream

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                //release file for read write
                thisLock.ReleaseMutex();
                outStream.Close();
            }
        }

        /// <summary>
        /// utility method for writing report data to excel
        /// </summary>
        /// <param name="resultDataSet"></param>
        /// <param name="spreadsheet"></param>
        /// <param name="includeMetaDataSheet"></param>
        private static void WriteExcelFile(DataSet resultDataSet, SpreadsheetDocument spreadsheet, Boolean includeMetaDataSheet)
        {
            try
            {

                #region initialization

                spreadsheet.AddWorkbookPart();
                spreadsheet.WorkbookPart.Workbook = new Workbook();
                spreadsheet.WorkbookPart.Workbook.Append(new BookViews(new WorkbookView()));

                WorkbookStylesPart workbookStylesPart = spreadsheet.WorkbookPart.AddNewPart<WorkbookStylesPart>("rIdStyles");
                Stylesheet stylesheet = new Stylesheet();
                workbookStylesPart.Stylesheet = stylesheet;

                #endregion

                #region write datatables to excel tables

                //  Loop thru tables in ds and create 1 sheet for each table with correct name
                uint worksheetNumber = 1;
                foreach (DataTable dt in resultDataSet.Tables)
                {
                    string workSheetID = "rId" + worksheetNumber.ToString();

                    WorksheetPart newWorksheetPart = spreadsheet.WorkbookPart.AddNewPart<WorksheetPart>();
                    newWorksheetPart.Worksheet = new Worksheet();

                    // create
                    newWorksheetPart.Worksheet.AppendChild(new SheetData());

                    // save
                    WriteDataTableToExcelWorksheet(dt, newWorksheetPart);
                    newWorksheetPart.Worksheet.Save();
                    // attach sheet to workbook
                    if (worksheetNumber == 1)
                    {
                        spreadsheet.WorkbookPart.Workbook.AppendChild(new Sheets());
                    }

                    if (worksheetNumber == 1 && !includeMetaDataSheet) //skip the metadatasheet if we dont want
                    {
                        
                    }
                    else
                    {
                        spreadsheet.WorkbookPart.Workbook.GetFirstChild<Sheets>().AppendChild(new Sheet()
                        {
                            Id = spreadsheet.WorkbookPart.GetIdOfPart(newWorksheetPart),
                            SheetId = (uint)worksheetNumber,
                            Name = AssignSheetName(resultDataSet, (int)worksheetNumber)
                        });
                    }
                    worksheetNumber++;

                }

                #endregion

            }
            finally
            {
                if (spreadsheet != null)
                {
                    spreadsheet.Close();
                }
            }
        }

        /// <summary>
        /// first table "metadata" contains sheet names
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="worksheetIndex"></param>
        /// <returns></returns>
        private static String AssignSheetName(DataSet ds, int worksheetIndex)
        {
            StringCollection sheetNameListAsString = new StringCollection();
            if (ds != null)
            {
                var sheetNameListAsEnum = from p in ds.Tables[0].AsEnumerable()
                                          select p["SheetName"];
                sheetNameListAsString.Add("Metadata"); //first sheet is always metadata
                foreach (var sheetNameAsEnum in sheetNameListAsEnum)
                {
                    sheetNameListAsString.Add(sheetNameAsEnum.ToString());
                }
            }

            return sheetNameListAsString[worksheetIndex - 1]; //because sheet index always starts @ 1

        }

        /// <summary>
        /// utility method for writing report data to excel
        /// </summary>
        /// <param name="resultDataTable"></param>
        /// <param name="worksheetPart"></param>
        private static void WriteDataTableToExcelWorksheet(DataTable resultDataTable, WorksheetPart worksheetPart)
        {
            #region initialization

            var workSheet = worksheetPart.Worksheet;
            var sheetData = workSheet.GetFirstChild<SheetData>();
            string cellValue = "";
            int numberOfColumns = resultDataTable.Columns.Count;
            bool[] IsNumericColumn = new bool[numberOfColumns];
            string[] excelColumnNames = new string[numberOfColumns];

            #endregion initialization

            for (int n = 0; n < numberOfColumns; n++)
            {
                excelColumnNames[n] = GetExcelColumnName(n);
            }

            //write header 
            uint rowIndex = 1;

            var headerRow = new DocumentFormat.OpenXml.Spreadsheet.Row { RowIndex = rowIndex };
            sheetData.Append(headerRow);

            for (int colInx = 0; colInx < numberOfColumns; colInx++)
            {
                DataColumn col = resultDataTable.Columns[colInx];
                AppendTextCell(excelColumnNames[colInx] + "1", col.ColumnName, headerRow);
                IsNumericColumn[colInx] = (col.DataType.FullName == "System.Decimal") || (col.DataType.FullName == "System.Int32");
            }

            //step thru rows in datatset
            double cellNumericValue = 0;
            foreach (DataRow dr in resultDataTable.Rows)
            {
                ++rowIndex;
                var newExcelRow = new DocumentFormat.OpenXml.Spreadsheet.Row { RowIndex = rowIndex };  // add a row at the top of spreadsheet
                sheetData.Append(newExcelRow);

                if (rowIndex == 100000) break;
                //number of errors topped at 100k
                for (int colInx = 0; colInx < numberOfColumns; colInx++)
                {
                    cellValue = dr.ItemArray[colInx].ToString();

                    if (String.IsNullOrEmpty(cellValue) || String.IsNullOrWhiteSpace(cellValue)) // empty cell
                    {
                        AppendTextCell(excelColumnNames[colInx] + rowIndex.ToString(), "N/A", newExcelRow);
                    }
                    else if (IsNumericColumn[colInx]) // format numeric cell
                    {
                        cellNumericValue = 0;
                        if (double.TryParse(cellValue, out cellNumericValue))
                        {
                            cellValue = cellNumericValue.ToString();
                            AppendNumericCell(excelColumnNames[colInx] + rowIndex.ToString(), cellValue, newExcelRow);
                        }
                    }
                    else //format text cell
                    {
                        AppendTextCell(excelColumnNames[colInx] + rowIndex.ToString(), cellValue, newExcelRow);
                    }
                }
            }
        }

        /// <summary>
        /// utility method for writing report data to excel
        /// </summary>
        /// <param name="cellReference"></param>
        /// <param name="cellStringValue"></param>
        /// <param name="excelRow"></param>
        private static void AppendTextCell(string cellReference, string cellStringValue, DocumentFormat.OpenXml.Spreadsheet.Row excelRow)
        {
            //  Add cell to row
            DocumentFormat.OpenXml.Spreadsheet.Cell cell = new DocumentFormat.OpenXml.Spreadsheet.Cell() { CellReference = cellReference, DataType = CellValues.String };
            CellValue cellValue = new CellValue();
            cellValue.Text = cellStringValue;
            cell.Append(cellValue);
            excelRow.Append(cell);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cellReference"></param>
        /// <param name="cellStringValue"></param>
        /// <param name="excelRow"></param>
        private static void AppendNumericCell(string cellReference, string cellStringValue, DocumentFormat.OpenXml.Spreadsheet.Row excelRow)
        {
            //  add cell to row
            DocumentFormat.OpenXml.Spreadsheet.Cell cell = new DocumentFormat.OpenXml.Spreadsheet.Cell { CellReference = cellReference };
            CellValue cellValue = new CellValue();
            cellValue.Text = cellStringValue;
            cell.Append(cellValue);
            excelRow.Append(cell);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="columnIndex"></param>
        /// <returns></returns>
        private static string GetExcelColumnName(int columnIndex)
        {
            if (columnIndex < 26)
                return ((char)('A' + columnIndex)).ToString();

            char firstChar = (char)('A' + (columnIndex / 26) - 1);
            char secondChar = (char)('A' + (columnIndex % 26));

            return string.Format("{0}{1}", firstChar, secondChar);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sheet"></param>
        private static void SetColumnWidth(Sheet sheet)
        {
            var sheetData = sheet.GetFirstChild<SheetData>();
            var maxColWidth = GetMaxCharacterWidth(sheetData);
            Columns columns = sheet.GetFirstChild<Columns>();
            double maxWidth = 7; //depends on current fonts of excel file

            foreach (var item in maxColWidth)
            {
                double width = Math.Truncate((item.Value * maxWidth + 5) / maxWidth * 256) / 256;

                double pixels = Math.Truncate(((256 * width + Math.Truncate(128 / maxWidth)) / 256) * maxWidth);

                double charWidth = Math.Truncate((pixels - 5) / maxWidth * 100 + 0.5) / 100;

                foreach (var openXmlElement in columns)
                {
                    var column = (DocumentFormat.OpenXml.Spreadsheet.Column) openXmlElement;
                    column.CustomWidth = true;
                    column.BestFit = true;
                    column.Width = width;
                }
            }
        }

        /// <summary>
        /// apply proper column width based on text size
        /// </summary>
        /// <param name="sheetData"></param>
        /// <returns></returns>
        private static Dictionary<int, int> GetMaxCharacterWidth(SheetData sheetData)
        {
            //iterate over all cells getting a max char value for each column
            Dictionary<int, int> maxColWidth = new Dictionary<int, int>();
            var rows = sheetData.Elements<DocumentFormat.OpenXml.Spreadsheet.Row>();
            UInt32[] numberStyles = new UInt32[] { 5, 6, 7, 8 }; //styles that will add extra chars
            UInt32[] boldStyles = new UInt32[] { 1, 2, 3, 4, 6, 7, 8 }; //styles that will bold
            foreach (var r in rows)
            {
                var cells = r.Elements<DocumentFormat.OpenXml.Spreadsheet.Cell>().ToArray();

                //using cell index as my column
                for (int i = 0; i < cells.Length; i++)
                {
                    var cell = cells[i];
                    var cellValue = cell.CellValue == null ? string.Empty : cell.CellValue.InnerText;
                    var cellTextLength = cellValue.Length;

                    if (cell.StyleIndex != null && numberStyles.Contains(cell.StyleIndex))
                    {
                        int thousandCount = (int)Math.Truncate((double)cellTextLength / 4);

                        //add 3 for '.00' 
                        cellTextLength += (3 + thousandCount);
                    }

                    if (cell.StyleIndex != null && boldStyles.Contains(cell.StyleIndex))
                    {
                        cellTextLength += 1;
                    }

                    if (maxColWidth.ContainsKey(i))
                    {
                        var current = maxColWidth[i];
                        if (cellTextLength > current)
                        {
                            maxColWidth[i] = cellTextLength;
                        }
                    }
                    else
                    {
                        maxColWidth.Add(i, cellTextLength);
                    }
                }
            }
            return maxColWidth;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        static void ApplyColumnAutoFit(string fileName)
        {
            thisLock.WaitOne(Timeout.Infinite, false);

            try
            {
                using (SpreadsheetDocument spreadSheet = SpreadsheetDocument.Open(fileName, true))
                {
                    WorkbookPart workbookPart = spreadSheet.WorkbookPart;

                    foreach (Sheet sheet in workbookPart.Workbook.Sheets)
                    {
                        if (sheet != null)
                        {
                            SetColumnWidth(sheet);
                        }
                    }
                    workbookPart.Workbook.Save();
                    spreadSheet.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                thisLock.ReleaseMutex();
            }
        }

        #endregion

        #endregion
    }
}