using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Data;

using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using OpenXmlCell = DocumentFormat.OpenXml.Spreadsheet.Cell;
using OpenXmlRow = DocumentFormat.OpenXml.Spreadsheet.Row;

namespace MDM.ImportSources.Lookup
{
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Imports;
    using MDM.BusinessObjects.Jobs;
    using MDM.Core;
    using MDM.ExcelUtility;
    using MDM.Imports.Interfaces;
    using MDM.Utility;
    using MDM.Interfaces;
    using MDM.LookupManager.Business;

    /// <summary>
    /// This class implements the source data from a RS Lookup Excel source.
    /// It reads the data in to a data set and process them as per the request from the Lookup import engine.
    /// </summary>
    public class RSLookupExcel10 : BaseLookupSource, ILookupImportSourceData
    {
        #region Fields

        private String _sourceFile = String.Empty;

        private List<String> _columnList = new List<String>();

        private LocaleEnum _systemDataLocale = LocaleEnum.UnKnown;

        private LookupCollection _lookupsToBeProcessed = new LookupCollection();

        private OperationResult _operationResult = null;

        private CallerContext _callerContext = new CallerContext(MDMCenterApplication.JobService, MDMCenterModules.Import);

        private Char[] _mandatoryColumnSymbol = " *".ToCharArray(); 

        #endregion

        #region Constructors

        /// <summary>
        /// Construct the RSLookupExcel10 with file path as parameter.
        /// Initialize the RSLookupExcel10 and update the source file path and system data locale.
        /// </summary>
        /// <param name="filePath">Indicates the file path</param>
        /// <exception cref="ArgumentNullException">When the File path is empty</exception>
        public RSLookupExcel10(String filePath)
        {
            if (Constants.TRACING_ENABLED || Constants.PERFORMANCE_TRACING_ENABLED)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Initializing RSLookupExcel10 provider", MDMTraceSource.LookupImport);
            }

            if (String.IsNullOrEmpty(filePath))
            {
                String errorMessage = "RSLookupExcel10 file is not available";
                throw new ArgumentNullException(errorMessage);
            }

            _sourceFile = filePath;

            _systemDataLocale = GlobalizationHelper.GetSystemDataLocale();

            if (Constants.TRACING_ENABLED || Constants.PERFORMANCE_TRACING_ENABLED)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Initialized RSLookupExcel10 provider.Imported RSLookupExcel10 file path is {0} ", _sourceFile), MDMTraceSource.LookupImport);
            }
        }

        #endregion

        #region Properties
        #endregion

        #region Methods

        #region ILookupImportSourceData Methods

        /// <summary>
        /// Get the batching mode.
        /// </summary>
        /// <returns>Get the type of batch used for the provider</returns>
        ImportProviderBatchingType ILookupImportSourceData.GetBatchingType()
        {
            return base.GetBatchingType();
        }

        /// <summary>
        /// Get all the lookup data which are waiting to be processed.
        /// </summary>
        /// <param name="application">Indicates the MDM application</param>
        /// <param name="module">Indicates the MDM module</param>
        /// <returns>Return the lookup collection object</returns>
        LookupCollection ILookupImportSourceData.GetAllLookupTables(MDMCenterApplication application, MDMCenterModules module)
        {
            return _lookupsToBeProcessed;
        }

        /// <summary>
        /// Get the operation result if there are any error occur during Source data reader operation
        /// </summary>
        /// <returns>Returns the operation result</returns>
        IOperationResult ILookupImportSourceData.GetOperationResult()
        {
            return (IOperationResult)_operationResult;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Provide an opportunity for the source data to initializes itself with configuration from the job.
        /// </summary>
        /// <param name="job">Indicates the Job</param>
        /// <param name="lookupImportProfile">Indicates the Import profile</param>
        /// <returns>RSLookupExcel10 is initialized then will return true else false</returns>
        public Boolean Initialize(Job job, LookupImportProfile lookupImportProfile)
        {
            _operationResult = new OperationResult();

            ReadExcelSheets();

            this.Job = job;

            return true;    //Revisit
        }

        #endregion

        #region Private Methods

        #region Excel Reader Helper Methods

        /// <summary>
        /// Read the Excel sheets using open XML reader
        /// </summary>
        private void ReadExcelSheets()
        {
            Collection<String> sheetList = new Collection<String>();
            Boolean readMetaDataSheet = false;
            Dictionary<String, String> lookupTables = new Dictionary<String, String>();

            using (SpreadsheetDocument document = SpreadsheetDocument.Open(_sourceFile, true))
            {
                WorkbookPart workbookPart = document.WorkbookPart;
                sheetList = GetAllSheetNames(workbookPart);
                readMetaDataSheet = IsMetaDataSheetPresent(sheetList);

                #region Read Meta data sheet If it available

                if (readMetaDataSheet)
                {
                    Worksheet metaDataWorkSheet = OpenSpreadsheetUtility.GetWorksheet(workbookPart, RSExcelConstants.MetadataSheetName);

                    if (metaDataWorkSheet != null)
                    {
                        SheetData metaDataSheet = metaDataWorkSheet.GetFirstChild<SheetData>();

                        var totalRows = metaDataSheet.Elements<OpenXmlRow>().Count();
                        SharedStringTable sharedStringTable = workbookPart.SharedStringTablePart.SharedStringTable;
                        lookupTables = GetMetaDataSheetDetails(metaDataSheet, sharedStringTable, totalRows);
                    }
                    else
                    {
                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(TraceEventType.Warning, String.Format("Metadata sheet not able to read from {0} file. Job Id {1}", _sourceFile, Job.Id), MDMTraceSource.LookupImport);
                        }
                    }
                }

                if (!readMetaDataSheet || lookupTables.Count == 0)
                {
                    //In the case of meta data sheet is not present sheet name is lookup table name (as a assumption)
                    lookupTables = GetLookupTableAndSheetNames(sheetList);
                }

                if (lookupTables != null && lookupTables.Count > 0)
                {
                    LookupModelCollection lookupModels = GetLookupModels(lookupTables, _callerContext);

                    foreach (KeyValuePair<String, String> lookupTable in lookupTables)
                    {
                        Worksheet workSheet = OpenSpreadsheetUtility.GetWorksheet(workbookPart, lookupTable.Value);

                        if (workSheet == null)
                        {
                            String message = String.Format("'{0}' sheet name does not exist for the '{1}' Lookup Table in the MetaData sheet", lookupTable.Value, _sourceFile);
                            _operationResult.AddOperationResult("113009", message, new Collection<Object>() { lookupTable.Value, _sourceFile }, OperationResultType.Error);

                            MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("Sheet name '{0}' not found in {1} file . Job Id is {2}", lookupTable.Value, _sourceFile, Job.Id), MDMTraceSource.LookupImport);
                            //If the work sheet is empty continue reading...
                            continue;
                        }
                        else if (lookupModels.IsViewBasedLookup(lookupTable.Key))
                        {
                            String message = String.Format("'{0}' is a view-based lookup, so it cannot be modified.", lookupTable.Key);
                            _operationResult.AddOperationResult("113934", message, new Collection<Object>() { lookupTable.Key }, OperationResultType.Error);

                            MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("'{0}' is a view-based lookup, so it cannot be modified. Job Id is {1}", lookupTable.Key, Job.Id), MDMTraceSource.LookupImport);
                            continue;
                        }

                        SheetData dataSheet = workSheet.GetFirstChild<SheetData>();

                        var totalRows = dataSheet.Elements<OpenXmlRow>().Count();
                        SharedStringTable sharedStringTable = workbookPart.SharedStringTablePart.SharedStringTable;

                        Dictionary<Int32, SharedStringItem> sharedStringItemDictionary = GetSharedStringItemAsDictionary(sharedStringTable);

                        //Get the columns
                        _columnList = new List<String>();
                        _columnList = GetHeaderColumns(workbookPart, dataSheet);
                        LookupCollection lookupsInSheet = BuildLookupsWithColumns(_columnList, lookupTable.Key, lookupTable.Value);

                        //fill lookup object with rows and cell values using reader
                        FillLookupRecords(lookupsInSheet, dataSheet, sharedStringItemDictionary);

                        if (lookupsInSheet != null && lookupsInSheet.Count > 0)
                        {
                            foreach (Lookup lookup in lookupsInSheet)
                            {
                                //No need to add empty lookup for process
                                if (lookup != null && lookup.Rows != null && lookup.Rows.Count > 0)
                                {
                                    
                                    _lookupsToBeProcessed.Add(lookup);
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (Constants.TRACING_ENABLED)
                    {
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Warning, String.Format("There is no lookup table available to read from {0} file. Job Id {1}", _sourceFile, Job.Id), MDMTraceSource.LookupImport);
                    }
                }

                #endregion

                document.Close();
            }
        }

        /// <summary>
        /// Get all the sheet names from the Excel file
        /// </summary>
        /// <param name="workbookPart">Indicates the workbookpart</param>
        /// <returns>List of sheet names there in the excel file</returns>
        private Collection<String> GetAllSheetNames(WorkbookPart workbookPart)
        {
            Collection<String> sheetList = new Collection<String>();
            if (workbookPart != null)
            {
                var sheets = workbookPart.Workbook.Descendants<Sheet>();
                if (sheets != null && sheets.Count() > 0)
                {
                    foreach (Sheet sheet in sheets)
                    {
                        sheetList.Add(sheet.Name.Value);
                    }
                }
            }

            return sheetList;
        }
        
        /// <summary>
        /// Get the Header column Names which are there in the excel sheet.
        /// </summary>
        /// <param name="workbookPart">Indicates the work book part</param>
        /// <param name="sheetData">Indicates the sheet data</param>
        /// <returns>List of columns which are present in the excel sheet</returns>
        private List<String> GetHeaderColumns(WorkbookPart workbookPart, SheetData sheetData)
        {
            OpenXmlRow headerRow = OpenSpreadsheetUtility.GetRow(sheetData, 1);
            SharedStringTable sharedStringTable = workbookPart.SharedStringTablePart.SharedStringTable;
            List<String> result = new List<String>();

            foreach (OpenXmlCell cell in headerRow.Elements<OpenXmlCell>())
            {
                if (cell != null)
                {
                    if (cell.CellValue != null)
                    {
                        if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString)
                        {
                            result.Add(OpenSpreadsheetUtility.GetSharedStringItemById(sharedStringTable, ValueTypeHelper.Int32TryParse(cell.CellValue.Text, 0)).InnerText);
                        }
                        else
                        {
                            result.Add(cell.CellValue.Text);
                        }
                    }
                    else
                    {
                        result.Add(cell.InnerText);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Get the Meta data sheet details as Dictionary. 
        /// Dictionary key is lookup name.
        /// Value is sheet name.
        /// This dictionary contains the details only which are the tables has to be processed.
        /// [Means: 'LoadLookup' column value is yes then and then only the lookup will be processed]
        /// </summary>
        /// <param name="metaDataSheet">Indicates the sheet Data</param>
        /// <param name="sharedStringTable">Indicates the sharedStringTable</param>
        /// <param name="noOfRows">Indicates number of rows available in the current sheet</param>
        /// <returns>Get the Meta data sheet details as Dictionary. </returns>
        private Dictionary<String, String> GetMetaDataSheetDetails(SheetData metaDataSheet, SharedStringTable sharedStringTable, Int32 noOfRows)
        {
            Dictionary<String, String> lookupTables = new Dictionary<String, String>();

            UInt32 lkpColumnIndex = 1;
            UInt32 sheetNameIndex = 2;
            UInt32 loadLookupColumnIndex = 3;
            OpenXmlCell cell = null;

            Dictionary<Int32, SharedStringItem> sharedStringDictionary = GetSharedStringItemAsDictionary(sharedStringTable);                        

            for (UInt32 rowIndex = 1; rowIndex <= noOfRows; rowIndex++)
            {
                // The cell object for a given row and column in a given sheet is found based on the reference cell for optimized search.
                // If the required cell is not found by reference cell, the code uses the old way of looping around the sheet to find it.
                cell = OpenSpreadsheetUtility.GetDataCell(metaDataSheet, cell, loadLookupColumnIndex, rowIndex);
                String loadLookup = GetCellValue(sharedStringDictionary, cell);

                if (!String.IsNullOrWhiteSpace(loadLookup) && loadLookup.ToLowerInvariant().Equals("yes"))
                {
                    cell = OpenSpreadsheetUtility.GetDataCell(metaDataSheet, cell, lkpColumnIndex, rowIndex);
                    String lookupName = GetCellValue(sharedStringDictionary, cell);

                    cell = OpenSpreadsheetUtility.GetDataCell(metaDataSheet, cell, sheetNameIndex, rowIndex);
                    String sheetName = GetCellValue(sharedStringDictionary, cell);

                    if (!lookupTables.ContainsKey(lookupName))  //Avoid duplicates
                    {
                        lookupTables.Add(lookupName, sheetName);
                    }
                }
            }

            return lookupTables;
        }

        /// <summary>
        /// Get the excel sheet cell value.
        /// </summary>
        /// <param name="sharedStringItemDictionary">Indicates the shared string items as an index based dictionary</param>
        /// <param name="currentCell">Indicates the current cell for which value has to be retrieved</param>
        /// <returns>Cell value as string</returns>
        private String GetCellValue(Dictionary<Int32, SharedStringItem> sharedStringItemDictionary, OpenXmlCell currentCell)
        {
            String cellValue = String.Empty;

            if (currentCell != null)
            {
                if (currentCell.CellValue != null)
                {
                    if (currentCell.DataType != null && currentCell.DataType.Value == CellValues.SharedString)
                    {
                        Int32 keyValue = ValueTypeHelper.Int32TryParse(currentCell.CellValue.Text, 0);

                        SharedStringItem sharedStringItem = null;
                        Boolean doesDataExist = sharedStringItemDictionary.TryGetValue(keyValue, out sharedStringItem);
                        if (doesDataExist && sharedStringItem != null)
                        {
                            cellValue = sharedStringItem.InnerText;
                        }
                    }
                    else
                {
                        cellValue = currentCell.CellValue.Text;
                    }
                }
                else
                {
                    cellValue = currentCell.InnerText;
                }
            }

            return cellValue;
        }
    
        private Dictionary<Int32, SharedStringItem> GetSharedStringItemAsDictionary(SharedStringTable sharedStringTable)
        {
            var indexBasedSharedStringItems = new Dictionary<Int32, SharedStringItem>();
            IEnumerable<SharedStringItem> sharedStringItems = sharedStringTable.Elements<SharedStringItem>();
            
            Int32 index = 0;
            foreach (SharedStringItem sharedStringItem in sharedStringItems)
            {
                indexBasedSharedStringItems.Add(index++, sharedStringItem);
            }
            return indexBasedSharedStringItems;
        }
    
        /// <summary>
        /// Read the excel sheet and Fill the lookup object.
        /// Populate the row values
        /// Populate the cell values.
        /// </summary>
        /// <param name="lookups">Indicates lookup collection</param>
        /// <param name="sheetData">Indicates the sheet data</param>
        /// <param name="sharedStringItemDictionary">Indicates the shared string item as a index based dictionary</param>
        private void FillLookupRecords(LookupCollection lookups, SheetData sheetData, Dictionary<Int32, SharedStringItem> sharedStringItemDictionary)
        {
            #region Logical flow
            /*
             *  Identity the column based on the locale for each row.
             *  Prepare Row based on the locale.
             *  Unique columns values populated to all lookup rows irrespecive of locale.
             *  Add row to lookup based on the locale.
             * 
             *  Let say in a lookup (excel sheet) there are 2 locale values available,
             *  then there will be two lookup tables will be prepared. 
             *  So according to locale the rows will be add to the lookup table.
             * 
             */
            #endregion

            Int32 totalRows = sheetData.Elements<OpenXmlRow>().Count();
            UInt32 columnIndex = 1;
            Int32 rowId = -1;

            Collection<Tuple<UInt32, String, String>> columnTuples = new Collection<Tuple<UInt32, String, String>>();   //This helps to do the mapping
            // column tuples item list
            // Item1 : column index
            // Item2 : locale name / unique column
            // Item3 : lookup column name (with out locale)

            #region  Prepare Actual column , locale and formated lookup column name map list

            HashSet<String> columnNameHashSet = new HashSet<String>();

            foreach (String actualColumnName in _columnList)
            {
                String[] columnSeprators = new String[] { "//" };
                String[] fieldComponents = actualColumnName.Split(columnSeprators, StringSplitOptions.None);
                String mappingValue = String.Empty;
                String columnName = String.Empty;

                if (fieldComponents != null && fieldComponents.Length > 0)
                {
                    // The below code will remove asterisk '*' from the column name which is appended by export formatter to identify unique columns marked as not null.
                    columnName = fieldComponents[0].TrimEnd(_mandatoryColumnSymbol);

                    if (fieldComponents.Length == 1)
                    {
                        mappingValue = "uniquecolumn";
                    }
                    else if (fieldComponents.Length == 2)
                    {
                        // The below code will remove asterisk '*' from the column name which is appended by export formatter to identify non-unique columns marked as not null.
                        mappingValue = fieldComponents[1].Trim(_mandatoryColumnSymbol);
                    }
                }
                // Item1 : column Index
                // Item2 : locale name / uniquecolumn
                // Item3 : lookup column name (with out locale)

                if (columnNameHashSet.Add(actualColumnName))
                {
                    columnTuples.Add(new Tuple<UInt32, String, String>(columnIndex, mappingValue, columnName));
                    columnIndex++;
                }
            }

            #endregion
            OpenXmlCell openXmlCell = null;

            for (UInt32 rowIndex = 2; rowIndex <= totalRows; rowIndex++)
            {
                Row lookupRow = new Row();
                lookupRow.Id = rowId;
                lookupRow.Action = ObjectAction.Unknown;

                // actual cell values with locale map bucket.
                Dictionary<String, CellCollection> cellsWithLocaleMap = new Dictionary<String, CellCollection>();

                //since the same row may have multiple locale values.
                //so we have to create a separate row for each locale and add the row based on the locale to the lookup object.
                foreach (Tuple<UInt32, String, String> columnMap in columnTuples)
                {
                    #region Mapping Variable values and actual cell value

                    String value = String.Empty;
                    UInt32 clmnIndex = columnMap.Item1;         //column index   
                    String localeName = columnMap.Item2;        //Locale (If locale value is 'uniquecolumn' then that will be the unique identifier
                    String columnName = columnMap.Item3;        //Lookup column name as per the standard

                    // The cell object for a given row and column in a given sheet is found based on the reference cell for optimized search.
                    // If the required cell is not found by reference cell, the code uses the old way of looping around the sheet to find it.
                    openXmlCell = OpenSpreadsheetUtility.GetDataCell(sheetData, openXmlCell, clmnIndex, rowIndex);
                    
                    // get the row cell value
                    value = GetCellValue(sharedStringItemDictionary, openXmlCell);

                    #endregion

                    #region Prepare cells for SDL cells and non SDL cells

                    if (localeName.Equals("uniquecolumn"))
                    {
                        #region Unique columns value population

                        if (columnName.ToLowerInvariant() == "action")
                        {
                            //In this case no need to add cell.
                            ObjectAction action = ObjectAction.Unknown;
                            Enum.TryParse<ObjectAction>(value, out action);

                            lookupRow.Action = action;
                        }
                        else    //Unique columns
                        {
                            Cell cell = new Cell();
                           
                            cell.ColumnName = columnName;
                            cell.Value = value;
                            lookupRow.Cells.Add(cell);
                        }

                        #endregion
                    }
                    else
                    {
                        #region Non Unique Columns Value population

                        CellCollection cells = new CellCollection();
                        Boolean isNewEntry = false;

                        if (cellsWithLocaleMap != null && cellsWithLocaleMap.Count > 0 && cellsWithLocaleMap.Keys.Contains(localeName))
                        {
                            cells = cellsWithLocaleMap[localeName];

                            if (cells == null)
                            {
                                cells = new CellCollection();
                                isNewEntry = true;
                            }
                        }
                        else
                        {
                            isNewEntry = true;
                        }

                        Cell cell = new Cell();
                        cell.ColumnName = columnName;
                        cell.Value = value;
                        cells.Add(cell);

                        //If it it is a new entry then it not present in the locale cell map dictionary so add it. else update the bucket.
                        if (isNewEntry)
                        {
                            cellsWithLocaleMap.Add(localeName, cells);
                        }
                        else
                        {
                            if (cellsWithLocaleMap.Keys.Contains(localeName))
                            {
                                cellsWithLocaleMap[localeName] = cells;
                            }
                        }

                        #endregion
                    }
                    #endregion
                }

                #region Add/ Update Lookup collection
                // Here add the cells to rows and update each lookup (each locale has separate lookup object)

                foreach (Lookup lkp in lookups)
                {
                    String lookupLocaleName = lkp.Locale.ToString();
                    Row row = new Row(lookupRow.ToXml());

                    if (_systemDataLocale == lkp.Locale)
                    {
                        row.IsSystemLocaleRow = true;
                    }

                    //find the lookup based on the locale and update the cells
                    if (cellsWithLocaleMap != null && cellsWithLocaleMap.Count > 0 && cellsWithLocaleMap.Keys.Contains(lookupLocaleName))
                    {
                        CellCollection cells = cellsWithLocaleMap[lookupLocaleName];

                        if (cells != null && cells.Count > 0)
                        {
                            foreach (Cell cell in cells)
                            {
                                row.Cells.Add(cell);
                            }
                        }
                    }

                    lkp.Rows.Add(row);
                }

                #endregion

                rowId--;
            }
        }
        
        #endregion Excel Reader Helper Methods

        #region Helper Methods

        /// <summary>
        /// Get all sheet names which are available in the excel sheet.
        /// In the case when the  meta data sheet is not present in input file, 
        /// Try to read all the sheet name and process. 
        /// Here is the assumption is sheet name is nothing but the lookup table name.
        /// </summary>
        /// <param name="sheetNames">Indicates the sheet names</param>
        /// <returns>Dictionary object contains the sheet name and lookup table name</returns>
        private Dictionary<String, String> GetLookupTableAndSheetNames(Collection<String> sheetNames)
        {
            Dictionary<String, String> lookupTables = new Dictionary<String, String>();

            foreach (String name in sheetNames)
            {
                if (!lookupTables.ContainsKey(name))
                {
                    //Since meta data sheet is not present sheet name is nothing but the lookup table name as the assumption.
                    lookupTables.Add(name, name);
                }
            }

            return lookupTables;
        }

        /// <summary>
        /// Whether the meta data sheet is available in the input file or not.
        /// If yes then return true else false
        /// </summary>
        /// <param name="sheets">Indicates the sheet name</param>
        /// <returns>True in the case meta data sheet is present else false</returns>
        private Boolean IsMetaDataSheetPresent(Collection<String> sheets)
        {
            Boolean readMetaDataSheet = false;

            if (sheets != null && sheets.Count() > 0)
            {
                foreach (String sheetName in sheets)
                {
                    if (sheetName.ToLowerInvariant().Equals("metadata"))
                    {
                        readMetaDataSheet = true;
                        break;
                    }
                }
            }
            return readMetaDataSheet;
        }

        /// <summary>
        /// Prepare Lookup object and populate the column details
        /// </summary>
        /// <param name="lookupColumnNames">Indicates the lookup column names</param>
        /// <param name="lookupName">Indicates the lookup name</param>
        /// <returns>LookupCollection object with columns</returns>
        private LookupCollection BuildLookupsWithColumns(List<String> lookupColumnNames, String lookupName, String sheetName)
        {
            LookupCollection lookups = new LookupCollection();
            Lookup lookup = null;
            ColumnCollection uniqueColumns = new ColumnCollection();

            foreach (String lookupColumnName in lookupColumnNames)
            {
                String[] lkpSeprators = new String[] { "//" };

                String[] lookupColumnHeader = lookupColumnName.Split(lkpSeprators, StringSplitOptions.None);

                if (lookupColumnHeader != null)
                {
                    #region Prepare Columns

                    Column column = new Column();
                    column.Locale = _systemDataLocale;

                    if (lookupColumnHeader.Length == 2)     // If the column contains the details about locale then
                    {
                        column.Name = lookupColumnHeader[0];

                        // The below code will remove asterisk '*' from the column name which is appended by export formatter to identify non-unique columns marked as not null.
                        String strlocale = lookupColumnHeader[1].TrimEnd(_mandatoryColumnSymbol);
                        LocaleEnum locale = LocaleEnum.UnKnown;
                        //try to parse only when this value is valid, otherwise TryParse will assign default value
                        if (Enum.IsDefined(typeof(LocaleEnum), strlocale))
                        {
                            Enum.TryParse<LocaleEnum>(strlocale, out locale);
                        }
                        else
                        {
                            String errorMsg = String.Format("'{0}' is not a valid locale. Please check '{1}' column in '{2}' sheet", strlocale, lookupColumnName, sheetName);
                            _operationResult.AddOperationResult("113012", errorMsg, new Collection<Object>() { strlocale, lookupColumnName, sheetName }, OperationResultType.Error);

                            if (Constants.TRACING_ENABLED)
                            {
                                MDMTraceHelper.EmitTraceEvent(TraceEventType.Warning, String.Format("Not able to identify the locale value '{0}' from the column header '{1}'. Lookup Table Name is  {2}, Job Id is {3} ", strlocale, lookupColumnName, lookupName, Job.Id), MDMTraceSource.LookupImport);
                            }
                            continue;   //If not able to identify the locale then ignore the column for building the lookup object.
                        }

                        column.Locale = locale;
                    }
                    else if (lookupColumnHeader.Length == 1)    // 'Id', 'Action' column and unique column comes here.
                    {
                        // The below code will remove asterisk '*' from the column name which is appended by export formatter to identify unique columns marked as not null.
                        column.Name = lookupColumnHeader[0].TrimEnd(_mandatoryColumnSymbol);
                        String columnName = column.Name.ToLowerInvariant();

                        if (columnName.Equals("action"))    
                        {
                            continue;
                        }
                        else if (!columnName.Equals("id"))
                        {
                            //Id column is not a unique column as per the assumption. so it is not a unique column. But is it common for all lookup object.
                            //so can add into sdl lookup bucket.
                            column.IsUnique = true;
                        }

                        uniqueColumns.Add(column);
                    }

                    #endregion

                    #region Prepare Lookup

                    //Find the lookup from the collection.
                    Lookup filterLookup = lookups.Where(x => x.Locale == column.Locale).FirstOrDefault();

                    if (filterLookup != null)
                    {
                        if (!filterLookup.Columns.Contains(column))
                        {
                            //If the lookup object already created update to the existing lookup object.
                            filterLookup.Columns.Add(column);
                        }
                        else
                        {
                            String errorMessage = String.Format("Duplicate column '{0}' exists in the '{1}' lookup table for '{2}' locale", column.Name, lookup.Name, lookup.Locale.ToString());
                            _operationResult.AddOperationResult("113011", errorMessage, new Collection<Object>() { column.Name, lookup.Name, lookup.Locale.ToString() }, OperationResultType.Error); ////Duplicate column '{0}' exists in the '{1}' lookup table for '{2}' locale
                            continue;
                        }
                        if (column.Locale != LocaleEnum.UnKnown)
                        {
                            filterLookup.Locale = column.Locale;
                        }
                    }
                    else
                    {
                        lookup = new Lookup();
                       
                        //In the case of new lookup object.
                        lookup.Columns.Add(column);
                        
                        if (column.Locale != LocaleEnum.UnKnown)
                        {
                            lookup.Locale = column.Locale;
                        }

                        //Since it it is a new lookup add to 'process lookup' bucket
                        lookups.Add(lookup);
                    }

                    #endregion
                }
            }

            #region Add unique columns to Lookup object

            if (uniqueColumns != null && uniqueColumns.Count > 0)
            {
                foreach (Lookup lkup in lookups)
                {
                    StringBuilder uniqueKeyValues = new StringBuilder();

                    foreach (Column uniqueColumn in uniqueColumns)
                    {
                        if (!lkup.Columns.Contains(uniqueColumn))
                        {
                            lkup.Columns.Add(uniqueColumn);

                            if (!uniqueColumn.Name.ToLowerInvariant().Equals("id"))  //other than id column
                            {
                                if (uniqueKeyValues.Length > 0)
                                {
                                    uniqueKeyValues.Append(",");    //Separator for multiple unique columns
                                    uniqueKeyValues.Append(uniqueColumn.Name);
                                }
                                else
                                {
                                    uniqueKeyValues.Append(uniqueColumn.Name);
                                }
                            }
                        }
                    }

                    if (uniqueKeyValues.Length > 0 && !lookup.ExtendedProperties.ContainsKey("UniqueKey"))
                    {
                        lookup.ExtendedProperties.Add("UniqueKey", uniqueKeyValues.ToString());
                    }

                    if (!lookup.ExtendedProperties.ContainsKey("SheetName"))
                    {
                        lookup.ExtendedProperties.Add("SheetName", sheetName);
                    }

                    //Update the lookup table name.
                    lkup.Name = lkup.LongName = lookupName;
                }
            }
            else
            {
                
                //There is no unique column found for the lookup throw error
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("Job Id is {0},There is no unique column found for '{1}' Lookup.", Job.Id, lookupName), MDMTraceSource.LookupImport);
            }
            //else :: Basically there is no unique columns mention in the excel file. lookup will fail.

            #endregion


            return lookups;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lookupTables"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        private LookupModelCollection GetLookupModels(Dictionary<String, String> lookupTables, CallerContext callerContext)
        {
            LookupBL lookupManager = new LookupBL();
            LookupModelCollection lookupModels = null;

            Collection<String> filteredLookupTableNames = new Collection<String>(lookupTables.Keys.ToList());

            if (filteredLookupTableNames != null && filteredLookupTableNames.Count > 0)
            {
                lookupModels = lookupManager.GetLookupModels(DynamicTableType.Lookup, filteredLookupTableNames, callerContext);
            }

            return lookupModels;
        }

        #endregion Helper Methods

        #endregion Private Methods

        #endregion
    }
}
