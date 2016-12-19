using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;

namespace MDM.JobManager.Business
{
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Jobs;
    using MDM.ConfigurationManager.Business;
    using MDM.Core;
    using MDM.JobManager.Data.SqlClient;
    using MDM.Utility;
    using MDM.BusinessObjects.DataModel;
    using MDM.BusinessObjects.Diagnostics;
    using MDM.MessageManager.Business;

    /// <summary>
    /// Specifies job import results class
    /// </summary>
    public class JobImportResultBL : BusinessLogicBase
    {
        #region Fields

        /// <summary>
        /// Indicates current trace settings
        /// </summary>
        private TraceSettings _currentTraceSettings = new TraceSettings();

        /// <summary>
        /// Indicates locale messages manager
        /// </summary>
        private LocaleMessageBL _localeMessageBL;

        #endregion

        #region Constructors

        /// <summary>
        /// Default Constructor
        /// </summary>
        public JobImportResultBL()
        {
            _currentTraceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
        }

        #endregion

        #region Public Methods

        #region Create method

        /// <summary>
        /// Saves the given set of import job results in to the database.
        /// </summary>
        /// <param name="jobImportResults"></param>
        /// <param name="application"></param>
        /// <param name="module"></param>
        /// <param name="programName"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public Boolean Save(JobImportResultCollection jobImportResults, MDMCenterApplication application, MDMCenterModules module, String programName, String userName)
        {
            if (jobImportResults != null)
            {
                foreach (JobImportResult result in jobImportResults)
                {
                    if (result.Action == ObjectAction.Read || result.Action == ObjectAction.Unknown)
                    {
                        result.Action = ObjectAction.Create;
                    }
                }
            }

            DBCommandProperties command = DBCommandHelper.Get(application, module, MDMCenterModuleAction.Create);
            return new JobImportResultDA().Save(jobImportResults, userName, programName, command);
        }

        ///// <summary>
        ///// Save Job Result Summary
        ///// </summary>
        ///// <param name="importResults"></param>
        ///// <param name="mDMCenterApplication"></param>
        ///// <param name="mDMCenterModules"></param>
        ///// <param name="ProgramName"></param>
        ///// <param name="UserName"></param>
        ///// <returns></returns>
        //public bool Save(JobImportResultSummaryCollection importResults, MDMCenterApplication application, MDMCenterModules module, String programName, String userName)
        //{
        //    if (importResults != null)
        //    {
        //        foreach (JobImportResult result in importResults)
        //        {
        //            if (result.Action == ObjectAction.Read || result.Action == ObjectAction.Unknown)
        //            {
        //                result.Action = ObjectAction.Create;
        //            }
        //        }
        //    }

        //    DBCommandProperties command = DBCommandHelper.Get(application, module, MDMCenterModuleAction.Create);
        //    return new JobImportResultDA().Save(importResults, userName, programName, command);
        //}

        #endregion

        /// <summary>
        /// Gets the import job results for a given job id
        /// </summary>
        /// <param name="jobId"></param>
        /// <param name="application"></param>
        /// <param name="module"></param>
        /// <returns></returns>
        public JobImportResultCollection Get(Int32 jobId, MDMCenterApplication application, MDMCenterModules module)
        {
            DBCommandProperties command = DBCommandHelper.Get(application, module, MDMCenterModuleAction.Read);
            return new JobImportResultDA().Get(jobId, command);
        }

        /// <summary>
        /// Gets Integration job errors collection in form of Excel file
        /// </summary>
        /// <param name="jobId">Indicates the job Id</param>
        /// <param name="locale">Indicates the locale</param>
        /// <param name="callerContext">Indicates the caller context of application</param>
        /// <returns></returns>
        public File GetAsExcelFile(Int32 jobId, LocaleEnum locale, CallerContext callerContext)
        {
            File result = null;
            JobImportResultCollection imporResults = Get(jobId, callerContext.Application, callerContext.Module);

            if (imporResults != null && imporResults.Count > 0)
            {
                MemoryStream ms = new MemoryStream();
                using (SpreadsheetDocument document = SpreadsheetDocument.Create(ms, SpreadsheetDocumentType.Workbook, true))
                {
                    WorkbookPart workbookPart = document.AddWorkbookPart();
                    workbookPart.Workbook = new DocumentFormat.OpenXml.Spreadsheet.Workbook();
                    workbookPart.Workbook.Sheets = new DocumentFormat.OpenXml.Spreadsheet.Sheets();

                    UInt32 sheetId = 1;

                    List<Tuple<String,Double>> columns = new List<Tuple<String,Double>>
                    {
                        new Tuple<String, Double>(GetLocaleMessage(locale, "113434", "Worksheet Name", callerContext), 20.0D),
                        new Tuple<String, Double>(GetLocaleMessage(locale, "111972", "Message Type", callerContext), 15.0D),
                        new Tuple<String, Double>(GetLocaleMessage(locale, "113435", "Record Identifier", callerContext), 30.0D),
                        new Tuple<String, Double>(GetLocaleMessage(locale, "113436", "Row Number", callerContext), 15.0D),
                        new Tuple<String, Double>(GetLocaleMessage(locale, "111973", "Error Level", callerContext), 12.0D),
                        new Tuple<String, Double>(GetLocaleMessage(locale, "113437", "Action Taken", callerContext), 15.0D),
                        new Tuple<String, Double>(GetLocaleMessage(locale, "100579", "Locale", callerContext), 10.0D),
                        new Tuple<String, Double>(GetLocaleMessage(locale, "111974", "Message Code", callerContext), 15.0D),
                        new Tuple<String, Double>(GetLocaleMessage(locale, "111975", "Message Description", callerContext), 100.0D)
                    };

                    String rowString = GetLocaleMessage(locale, "100906", "Row", callerContext);
                    String errorString = GetLocaleMessage(locale, "110544", "Error", callerContext);
                    String warningString = GetLocaleMessage(locale, "111720", "Warning", callerContext);
                    String informationString = GetLocaleMessage(locale, "111707", "Information", callerContext);

                    Regex externalIdRegex = new Regex("^[[](\\d*)[]] - (.*)$");


                    WorksheetPart sheetPart = workbookPart.AddNewPart<WorksheetPart>();
                    DocumentFormat.OpenXml.Spreadsheet.SheetData sheetData = new DocumentFormat.OpenXml.Spreadsheet.SheetData();
                    sheetPart.Worksheet = new DocumentFormat.OpenXml.Spreadsheet.Worksheet();

                    #region Columns configuration

                    DocumentFormat.OpenXml.Spreadsheet.Columns cls = new DocumentFormat.OpenXml.Spreadsheet.Columns();
                    for (Int32 colIndex = 0; colIndex < columns.Count; colIndex++)
                    {
                        DocumentFormat.OpenXml.Spreadsheet.Column col = new DocumentFormat.OpenXml.Spreadsheet.Column()
                        {
                            Min = (UInt32Value)((UInt32)colIndex + 1),
                            Max = (UInt32Value)((UInt32)colIndex + 1),
                            Width = columns[colIndex].Item2,
                            CustomWidth = true
                        };
                        cls.Append(col);
                    }
                    sheetPart.Worksheet.Append(cls);

                    #endregion

                    sheetPart.Worksheet.Append(sheetData);

                    DocumentFormat.OpenXml.Spreadsheet.Sheets sheets = workbookPart.Workbook.GetFirstChild<DocumentFormat.OpenXml.Spreadsheet.Sheets>();
                    String relationshipId = workbookPart.GetIdOfPart(sheetPart);

                    if (sheets.Elements<DocumentFormat.OpenXml.Spreadsheet.Sheet>().Count() > 0)
                    {
                        sheetId = sheets.Elements<DocumentFormat.OpenXml.Spreadsheet.Sheet>().Select(s => s.SheetId.Value).Max() + 1;
                    }

                    DocumentFormat.OpenXml.Spreadsheet.Sheet sheet =
                        new DocumentFormat.OpenXml.Spreadsheet.Sheet()
                        {
                            Id = relationshipId,
                            SheetId = sheetId,
                            Name = "Job results"
                        };
                    sheets.Append(sheet);

                    #region Header processing

                    DocumentFormat.OpenXml.Spreadsheet.Row headerRow = new DocumentFormat.OpenXml.Spreadsheet.Row();

                    for(Int32 colIndex = 0; colIndex < columns.Count; colIndex++)
                    {
                        String column = columns[colIndex].Item1;

                        DocumentFormat.OpenXml.Spreadsheet.Cell cell = new DocumentFormat.OpenXml.Spreadsheet.Cell();
                        cell.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String;
                        cell.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(column);
                        headerRow.AppendChild(cell);
                    }

                    sheetData.AppendChild(headerRow);

                    #endregion

                    List<String[]> rows = new List<String[]>();

                    #region Import results processing

                    foreach (JobImportResult imporResult in imporResults)
                    {
                        if (imporResult.OperationResult != null)
                        {
                            String rowNumber = String.Empty;
                            String recordIdentifier = String.Empty;
                            Match match = externalIdRegex.Match(imporResult.ExternalId);
                            if (match.Success)
                            {
                                rowNumber = match.Groups[1].Value;
                                recordIdentifier = match.Groups[2].Value;
                            }
                            String action = imporResult.PerformedAction.ToString();
                            String resultLocale = imporResult.Locale.ToString();

                            #region Different message types processing

                            if (imporResult.OperationResult.HasError)
                            {
                                foreach (Error error in imporResult.OperationResult.Errors)
                                {
                                    String[] row = new String[]
                                    {
                                        DataModelOperationResult.GetReferenceNameByObjectType(imporResult.ObjectType),
                                        // worksheet name
                                        errorString, // message type
                                        recordIdentifier, // record identifier
                                        rowNumber, // row number
                                        rowString, // error level
                                        action, // action taken
                                        resultLocale, // locale
                                        error.ErrorCode, // error message code
                                        error.ErrorMessage // error message
                                    };

                                    rows.Add(row);
                                }
                            }

                            if (imporResult.OperationResult.HasWarnings)
                            {
                                foreach (Warning warning in imporResult.OperationResult.Warnings)
                                {
                                    String[] row = new String[]
                                    {
                                        DataModelOperationResult.GetReferenceNameByObjectType(imporResult.ObjectType),
                                        // worksheet name
                                        warningString, // message type
                                        recordIdentifier, // record identifier
                                        rowNumber, // row number
                                        rowString, // error level
                                        action, // action taken
                                        resultLocale, // locale
                                        warning.WarningCode, // warning message code
                                        warning.WarningMessage // warning message
                                    };

                                    rows.Add(row);
                                }
                            }

                            if (imporResult.OperationResult.HasInformation)
                            {
                                foreach (Information information in imporResult.OperationResult.Informations)
                                {
                                    String[] row = new String[]
                                    {
                                        DataModelOperationResult.GetReferenceNameByObjectType(imporResult.ObjectType),
                                        // worksheet name
                                        informationString, // message type
                                        recordIdentifier, // record identifier
                                        rowNumber, // row number
                                        rowString, // error level
                                        action, // action taken
                                        resultLocale, // locale
                                        information.InformationCode, // information message code
                                        information.InformationMessage // information message
                                    };

                                    rows.Add(row);
                                }
                            }

                            #endregion
                        }
                    }

                    #endregion

                    #region Saving data to Excel table

                    foreach (String[] row in rows)
                    {
                        DocumentFormat.OpenXml.Spreadsheet.Row newRow = new DocumentFormat.OpenXml.Spreadsheet.Row();

                        foreach (String col in row)
                        {
                            DocumentFormat.OpenXml.Spreadsheet.Cell cell = new DocumentFormat.OpenXml.Spreadsheet.Cell();
                            cell.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String;
                            cell.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(col);

                            newRow.AppendChild(cell);
                        }

                        sheetData.AppendChild(newRow);
                    }

                    #endregion

                    workbookPart.Workbook.Save();
                }

                result = new File(String.Format("{0}_ImportJobId_{1}_JobResults.xlsx", DateTime.Now.ToString("yyyyMMdd HHmmss"), jobId), "application/vnd.ms-excel", false, ms.ToArray());
            }

            return result;
        }

        /// <summary>
        /// Get the lookup job operation result based on the jobid and lookup table name
        /// </summary>
        /// <param name="jobId">Indicates the job Id</param>
        /// <param name="lookupTableName">Indicates the lookup table name</param>
        /// <param name="callerContext">Indicates the caller context object</param>
        /// <returns>Return the operation result</returns>
        public OperationResult GetLookupJobOperationResult(Int32 jobId,String lookupTableName, CallerContext callerContext)
        {
            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();

            if (_currentTraceSettings.IsBasicTracingEnabled)
            {
                diagnosticActivity.Start();
            }

            try
            {
                OperationResult result = null;

                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.LogInformation("Start : MDM.JobManager.Business.JobImportResultBL.GetLookupJobOperationResult");
                }

                DBCommandProperties command = DBCommandHelper.Get(callerContext.Application, callerContext.Module, MDMCenterModuleAction.Read);

                JobImportResultDA jobImportResultDA = new JobImportResultDA();
                JobImportResultCollection results = jobImportResultDA.Get(jobId, command, lookupTableName);

                if (results != null)
                {
                    var importResult = results.FirstOrDefault();

                    if (importResult != null && importResult.OperationResult != null)
                    {
                        result = importResult.OperationResult;
                    }
                }

                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.LogInformation("End : MDM.JobManager.Business.JobImportResultBL.GetLookupJobOperationResult");
                }

                return result;
            }
            finally
            {
                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.Stop();
                }
            }
            
        }

        /// <summary>
        /// Get the dataModel job operation result summary collection based on the jobid, object name and externalid
        /// </summary>
        /// <param name="jobId">Job Id</param>
        /// <param name="objectType">Object Type, can be empty</param>
        /// <param name="externaId">ExternalId, can be empty</param>
        /// <param name="callerContext">callerContext</param>
        /// <returns>DataModelOperationResultSummaryCollection</returns>
        public DataModelOperationResultSummaryCollection GetDataModelOperationResultSummaryCollection(Int32 jobId, ObjectType objectType, string externaId, CallerContext callerContext)
        {
            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();

            if (_currentTraceSettings.IsBasicTracingEnabled)
            {
                diagnosticActivity.Start();
            }

            try
            {
                DataModelOperationResultSummaryCollection summaryResults = new DataModelOperationResultSummaryCollection();
                
                DBCommandProperties command = DBCommandHelper.Get(callerContext.Application, callerContext.Module, MDMCenterModuleAction.Read);

                JobImportResultDA jobImportResultDA = new JobImportResultDA();
                JobImportResultCollection results = jobImportResultDA.GetImportResults(jobId, command, JobResultsReturnType.ObjectTypeSummary, objectType, externaId);

                if (results != null)
                {
                    foreach (var result in results)
                    {
                        if (result.OperationResultXML != null)
                        {
                            summaryResults.Add(new DataModelOperationResultSummary(result.OperationResultXML));
                        }
                    }
                }

                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.LogInformation("End : MDM.JobManager.Business.JobImportResultBL.GetDataModelJobOperationResult");
                }

                return summaryResults;
            }
            finally
            {
                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.Stop();
                }
            }
        }

        /// <summary>
        /// Get the dataModel job operation result based on the jobid and dataModel table name
        /// </summary>
        /// <param name="jobId">Indicates the job Id</param>
        /// <param name="objectType">Indicates object type</param>
        /// <param name="externaId"></param>
        /// <param name="callerContext">Indicates the caller context object</param>
        /// <returns>DataModel OperationResult Collection</returns>
        public DataModelOperationResultCollection GetDataModelOperationResultCollection(Int32 jobId, ObjectType objectType, String externaId, CallerContext callerContext)
        {
            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();

            if (_currentTraceSettings.IsBasicTracingEnabled)
            {
                diagnosticActivity.Start();
            }

            try
            {
                DataModelOperationResultCollection operationResults = new DataModelOperationResultCollection();
                
                DBCommandProperties command = DBCommandHelper.Get(callerContext.Application, callerContext.Module, MDMCenterModuleAction.Read);

                JobImportResultDA jobImportResultDA = new JobImportResultDA();
                // TODO Prasad
                JobImportResultCollection results = jobImportResultDA.GetImportResults(jobId, command, JobResultsReturnType.ObjectTypeDetail, objectType, externaId);

                if (results != null)
                {
                    foreach (var result in results)
                    {
                        if (result.ExternalId != "DataModelImportResult" && result.OperationResultXML != null)
                        {
                            operationResults.Add(new DataModelOperationResult(result.OperationResultXML));
                        }
                    }
                }

                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.LogInformation("End : MDM.JobManager.Business.JobImportResultBL.DataModelOperationResultCollection");
                }

                return operationResults;
            }
            finally
            {
                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.Stop();
                }
            }
            
        }
        /// <summary>
        /// Update the lookup job's operation result based on the lookup table name and job id
        /// </summary>
        /// <param name="jobId">Indicates the job id</param>
        /// <param name="lookupTableName">Indicates the lookup table name</param>
        /// <param name="operationResult">Indicates the operation result</param>
        /// <param name="callerContext">Indicates the caller context object</param>
        /// <returns>Boolean result be return based on the operation status </returns>
        public Boolean UpdateLookupOperationResult(Int32 jobId, String lookupTableName, OperationResult operationResult, CallerContext callerContext)
        {
            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();

            if (_currentTraceSettings.IsBasicTracingEnabled)
            {
                diagnosticActivity.Start();
            }

            try
            {
                Boolean result = false;

                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.LogInformation("Start : MDM.JobManager.Business.JobImportResultBL.UpdateLookupOperationResult");
                }

                DBCommandProperties command = DBCommandHelper.Get(callerContext.Application, callerContext.Module, MDMCenterModuleAction.Read);

                JobImportResult jobResult = new JobImportResult()
                {
                    Action = ObjectAction.Update,
                    JobId = jobId,
                    ExternalId = lookupTableName,
                    ObjectType = ObjectType.Lookup
                };

                if (operationResult != null)
                {
                    jobResult.OperationResultXML = operationResult.ToXml();

                    if (operationResult.ExtendedProperties != null)
                    {
                        if (operationResult.ExtendedProperties.ContainsKey("LookupTableId"))
                        {
                            //Internal id is nothing but the lookup table id
                            jobResult.InternalId = ValueTypeHelper.Int64TryParse(operationResult.ExtendedProperties["LookupTableId"].ToString(), -1);
                        }

                        if (operationResult.ExtendedProperties.ContainsKey("Status"))
                        {
                            jobResult.Status = operationResult.ExtendedProperties["Status"].ToString();
                        }

                        if (operationResult.ExtendedProperties.ContainsKey("Description"))
                        {
                            jobResult.Description = operationResult.ExtendedProperties["Description"].ToString();
                        }
                    }
                }

                JobImportResultDA jobImportResultDA = new JobImportResultDA();
                result = jobImportResultDA.Save(new JobImportResultCollection() { jobResult }, String.Empty, callerContext.ProgramName, command);
                
                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.LogInformation("End : MDM.JobManager.Business.JobImportResultBL.UpdateLookupOperationResult");
                }

                return result;
            }
            finally
            {
                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.Stop();
                }
            }
            
        }

        #endregion

        #region Private Methods

        private LocaleMessageBL GetLocaleMessageBL()
        {
            if (_localeMessageBL == null)
            {
                _localeMessageBL = new LocaleMessageBL();
            }
            return _localeMessageBL;
        }

        /// <summary>
        /// Gets locale message
        /// </summary>
        /// <param name="locale">Locale</param>
        /// <param name="messageCode">Message code for which message needs to be get</param>
        /// <param name="defaultMessage">Default message to be displayed when locale message is not available.</param>
        /// <param name="callerContext">Indicates the caller context of application</param>
        /// <param name="paramArray">Parameters which needs to be formatted to string</param>
        /// <param name="escapeSpecialCharacters">Escape special characters ['],["],[\]. Set as True whenever message get is required for JavaScript</param>
        /// <returns>Locale message</returns>
        public String GetLocaleMessage(LocaleEnum locale, String messageCode, String defaultMessage, CallerContext callerContext, Object[] paramArray = null, Boolean escapeSpecialCharacters = false)
        {
            if (paramArray == null)
            {
                paramArray = new Object[0];
            }
            String localeMessage = this.GetLocaleMessageBL().TryGet(locale, messageCode, defaultMessage, paramArray, false, callerContext, escapeSpecialCharacters).Message;

            if (String.IsNullOrEmpty(localeMessage) || localeMessage == "Message Code is not Available")
            {
                localeMessage = String.Format(defaultMessage, paramArray);
            }

            return localeMessage;
        }

        #endregion
    }
}
