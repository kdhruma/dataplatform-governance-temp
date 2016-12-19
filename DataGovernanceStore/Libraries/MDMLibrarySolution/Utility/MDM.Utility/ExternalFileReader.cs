using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using MDM.BusinessObjects.Diagnostics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using DFOS = DocumentFormat.OpenXml.Spreadsheet;

namespace MDM.Utility
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ExternalFileReader
    {
        #region Fields
        /// <summary>
        /// MetadataSheetName
        /// </summary>
        public const String MetadataSheetName = "Metadata"; //Moved the value form excel constants to compare the sheetnames
        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="FileType"></param>
        /// <param name="CompleteFileName"></param>
        /// <param name="SheetName"></param>
        /// <param name="Query"></param>
        /// <returns></returns>
        public static DataSet ReadExternalFile(String FileType, String CompleteFileName, String SheetName, String Query)
        {
            //Note: Must pass $ on SheetName or in the Query if required.  This method no longer appends the $ to the Query
            return ReadExternalFile(FileType, CompleteFileName, SheetName, Query, false, null, null, null, 0, "ANSI");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="FileType"></param>
        /// <param name="CompleteFileName"></param>
        /// <param name="SheetName"></param>
        /// <param name="Query"></param>
        /// <param name="HeaderOnly"></param>
        /// <param name="FixedColumnList"></param>
        /// <returns></returns>
        public static DataSet ReadExternalFile(String FileType, String CompleteFileName, String SheetName, String Query, Boolean HeaderOnly, String[,] FixedColumnList)
        {
            return ReadExternalFile(FileType, CompleteFileName, SheetName, Query, HeaderOnly, null, FixedColumnList, null, 0, "ANSI");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="FileType"></param>
        /// <param name="CompleteFileName"></param>
        /// <param name="HeaderOnly"></param>
        /// <param name="formatDelimiter"></param>
        /// <param name="FixedColumnList"></param>
        /// <returns></returns>
        public static DataSet ReadExternalFile(String FileType, String CompleteFileName, Boolean HeaderOnly, String formatDelimiter, String[,] FixedColumnList)
        {
            return ReadExternalFile(FileType, CompleteFileName, null, null, false, formatDelimiter, FixedColumnList, null, 0, "ANSI");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="FileType"></param>
        /// <param name="CompleteFileName"></param>
        /// <param name="SheetName"></param>
        /// <param name="Query"></param>
        /// <param name="HeaderOnly"></param>
        /// <param name="formatDelimiter"></param>
        /// <param name="FixedColumnList"></param>
        /// <param name="TextDelimiter"></param>
        /// <returns></returns>
        public static DataSet ReadExternalFile(String FileType, String CompleteFileName, String SheetName, String Query, Boolean HeaderOnly, String formatDelimiter, String[,] FixedColumnList, String TextDelimiter)
        {
            return ReadExternalFile(FileType, CompleteFileName, SheetName, Query, HeaderOnly, formatDelimiter, FixedColumnList, TextDelimiter, 0, "ANSI");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="FileType"></param>
        /// <param name="CompleteFileName"></param>
        /// <param name="SheetName"></param>
        /// <param name="Query"></param>
        /// <param name="HeaderOnly"></param>
        /// <param name="formatDelimiter"></param>
        /// <param name="FixedColumnList"></param>
        /// <param name="TextDelimiter"></param>
        /// <param name="MaxScanRows"></param>
        /// <param name="CharacterSet"></param>
        /// <returns></returns>
        public static DataSet ReadExternalFile(String FileType, String CompleteFileName, String SheetName, String Query, Boolean HeaderOnly, String formatDelimiter, String[,] FixedColumnList, String TextDelimiter, Int32 MaxScanRows, String CharacterSet)
        {
            String message = String.Empty;
            var traceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();

            DiagnosticActivity activity = new DiagnosticActivity();
            if (traceSettings.IsBasicTracingEnabled) activity.Start();

            //TODO move all oledb connection string to a configuration file
            String strFilePath = null;
            String strFileName = null;
            String strQry = null;
            System.Data.OleDb.OleDbDataAdapter myAdapter = null;
            System.Data.OleDb.OleDbConnection dbConnection = null;
            DataSet myDataSet = new DataSet();

            try
            {
                //  Always use the BackSlash (\)
                CompleteFileName = CompleteFileName.Replace("/", @"\");
                //  Split the File Name and Path
                strFilePath = CompleteFileName.Substring(0, CompleteFileName.LastIndexOf(@"\"));
                strFileName = CompleteFileName.Substring(strFilePath.Length + 1);

                String fileProvider = "Microsoft.ACE.OLEDB.12.0";
                String excelConnectionFormat = "Provider={0};Data Source={1};Extended Properties='{2}'";
                String extendedProperties = String.Empty;
                String excelConnectionString = String.Empty;

                if (FileType.ToLower() == "xml")
                    myDataSet = LoadRsXmlIntoDataSet(CompleteFileName);
                else
                {
                    switch (FileType.ToLower())
                    {
                        case "excel":
                            //Create the connection
                            String validQuery = "";
                            //dbConnection = new System.Data.OleDb.OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + CompleteFileName + @";Extended Properties=""Excel 8.0;IMEX=1""");

                            extendedProperties = "Excel 12.0;HDR=Yes;IMEX=2";

                            excelConnectionString = String.Format(excelConnectionFormat, fileProvider, CompleteFileName, extendedProperties);

                            dbConnection = new System.Data.OleDb.OleDbConnection(excelConnectionString);

                            String validSheetName = ValidateExcelWorkSheet(CompleteFileName, SheetName);
                            if (validSheetName[validSheetName.Length - 1] == '$')
                            {
                                validSheetName = validSheetName.Substring(0, validSheetName.Length - 1);
                                //validQuery = "SELECT * FROM [" + validSheetName + "$]";

                                validQuery = String.Format("SELECT * FROM [{0}$]", validSheetName);
                            }
                            else
                            {
                                //validQuery = "SELECT * FROM [" + validSheetName + "]";
                                validQuery = String.Format("SELECT * FROM [{0}]", validSheetName);
                            }
                            /*
                            validQuery = "SELECT * FROM [" + validSheetName + "]";
                            */
                            if (SheetName[SheetName.Length - 1] == '$')
                            {
                                SheetName = SheetName.Substring(0, SheetName.Length - 1);
                            }

                            if (SheetName.ToLower() != validSheetName.ToLower())
                            {
                                message = string.Format("File does not contain the Sheet Name : '{0}' specified in the Profile.", SheetName);

                                activity.LogError(message);
                                throw new Exception(message);
                            }

                            if (Query.Length > 0)
                            {
                                strQry = Query;
                            }
                            else
                            {
                                //Use the $ sheet if it exists
                                dbConnection.Open();
                                DataTable ExcelSheets = dbConnection.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables, new Object[] { null, null, null, "TABLE" });
                                DataRow[] rows = ExcelSheets.Select("TABLE_NAME = '" + SheetName + "'");
                                if (rows.Length > 0)
                                    SheetName = SheetName + "$";
                                strQry = validQuery;
                                dbConnection.Close();
                            }
                            break;
                        case "csv":
                            CreateSchemaIni(strFilePath, strFileName, "CSVDelimited", HeaderOnly, MaxScanRows, CharacterSet, FixedColumnList, TextDelimiter);

                            extendedProperties = "text;HDR=YES;FMT=CSVDelimited";

                            excelConnectionString = String.Format(excelConnectionFormat, fileProvider, strFilePath, extendedProperties);

                            //dbConnection = new System.Data.OleDb.OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + strFilePath + ";Extended Properties='text;HDR=YES;FMT=CSVDelimited';");


                            dbConnection = new System.Data.OleDb.OleDbConnection(excelConnectionString);
                            strQry = "SELECT * FROM [" + strFileName + "]";

                            break;
                        case "tab":
                            CreateSchemaIni(strFilePath, strFileName, "TabDelimited", HeaderOnly, MaxScanRows, CharacterSet, FixedColumnList, TextDelimiter);

                            extendedProperties = "text;HDR=YES;FMT=TabDelimited";

                            excelConnectionString = String.Format(excelConnectionFormat, fileProvider, strFilePath, extendedProperties);


                            //dbConnection = new System.Data.OleDb.OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + strFilePath + ";Extended Properties='text;HDR=YES;FMT=TAbDelimited';");

                            dbConnection = new System.Data.OleDb.OleDbConnection(excelConnectionString);

                            strQry = "SELECT * FROM [" + strFileName + "]";
                            break;
                        case "custom":
                            CreateSchemaIni(strFilePath, strFileName, formatDelimiter, HeaderOnly, MaxScanRows, CharacterSet, FixedColumnList, TextDelimiter);

                            extendedProperties = "text;FMT=Delimited;HDR=YES";

                            excelConnectionString = String.Format(excelConnectionFormat, fileProvider, strFilePath, extendedProperties);


                            //dbConnection = new System.Data.OleDb.OleDbConnection(
                            //    "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + strFilePath + ";" +
                            //    "Extended Properties='text;FMT=Delimited;HDR=YES'");

                            dbConnection = new System.Data.OleDb.OleDbConnection(excelConnectionString);

                            strQry = "SELECT * FROM [" + strFileName + "]";
                            break;
                        default:
                            {
                                message = "File Type: '" + FileType + "' Not Implemented yet";

                                activity.LogError(message);
                                throw new Exception(message);
                    }
                    }

                    myAdapter = new System.Data.OleDb.OleDbDataAdapter(strQry, dbConnection);
                    myAdapter.Fill(myDataSet);
                }
            }
            catch (Exception ex)
            {
                message = string.Format("Error reading external file - {0}", ex.Message);
                activity.LogError(message);
            }
            finally
            {
                if (File.Exists(strFilePath + @"\" + "schema.ini"))
                    File.Delete(strFilePath + @"\" + "schema.ini");
                /*if (Directory.Exists(strFilePath))
                    Directory.Delete(strFilePath,true);
                */
                if (myAdapter != null)
                {
                    myAdapter.Dispose();
                    myAdapter = null;
                }

                if (traceSettings.IsBasicTracingEnabled) activity.Stop();
            }

            return myDataSet;
        }

        /// <summary>
        /// Read the Excel Sheet from given file using SAX Parsing Reader
        /// </summary>
        /// <param name="CompleteFileName">Complete FileName</param>
        /// <param name="SheetName">Sheet Name</param>
        /// <param name="HeaderOnly">True if only Header needs to be read, false for the whole sheet.</param>
        /// <returns>DataSet representing the excel sheet</returns>
        public static DataSet ReadExternalFileSAXParsing(String CompleteFileName, String SheetName, Boolean HeaderOnly)
        {
            string message = string.Empty;
            var traceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();

            DiagnosticActivity activity = new DiagnosticActivity();
            if (traceSettings.IsBasicTracingEnabled) activity.Start();

            DataSet dsDataSet = new DataSet();
            DataTable dtImportDataTable = new DataTable();

            CompleteFileName = CompleteFileName.Replace("/", @"\");
            String validSheetName = ValidateExcelWorkSheetSAXParser(CompleteFileName, SheetName);
            if (!String.IsNullOrWhiteSpace(SheetName) && SheetName[SheetName.Length - 1] == '$')
            {
                message = string.Format("File does not contain the Sheet Name : '{0}' specified in the Profile.", SheetName);

                activity.LogError(message);
                throw new Exception(message);
            }

            if (SheetName[SheetName.Length - 1] == '$')
            {
                SheetName = SheetName.Substring(0, SheetName.Length - 1);
            }

            if (SheetName.ToLower() != validSheetName.ToLower())
            {
                message = string.Format("File does not contain the Sheet Name : '{0}' specified in the Profile.", SheetName);

                activity.LogError(message);
                throw new Exception(message);
            }
            using (SpreadsheetDocument objSpreadSheetDoc = SpreadsheetDocument.Open(CompleteFileName, false))
            {
                dtImportDataTable = GetExcelFileData(SheetName, objSpreadSheetDoc);
            }
            dsDataSet.Tables.Add(dtImportDataTable);

            if (traceSettings.IsBasicTracingEnabled) activity.Stop();

            return dsDataSet;
        }

        /// <summary>
        /// Read the Excel Sheet from given file using SAX Parsing Reader
        /// </summary>
        /// <param name="file">Indicates name of file</param>
        /// <param name="SheetName">Indicates name of sheet</param>
        /// <param name="HeaderOnly">Indicates true if only Header needs to be read, false for the whole sheet.</param>
        /// <returns>DataSet representing the excel sheet</returns>
        public static DataSet ReadExternalFileSAXParsing(MDM.BusinessObjects.File file, String SheetName, Boolean HeaderOnly)
        {
            string message = string.Empty;
            var traceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();

            DiagnosticActivity activity = new DiagnosticActivity();
            if (traceSettings.IsBasicTracingEnabled) activity.Start();

            DataSet dsDataSet = new DataSet();
            DataTable dtImportDataTable = new DataTable();
            byte[] bytes = file.FileData;

            using (Stream stream = new MemoryStream(bytes))
            {
                using (SpreadsheetDocument objSpreadSheetDoc = SpreadsheetDocument.Open(stream, false))
                {
                    dtImportDataTable = GetExcelFileData(SheetName, objSpreadSheetDoc);
                }
                dsDataSet.Tables.Add(dtImportDataTable);
            }

            if (traceSettings.IsBasicTracingEnabled) activity.Stop();

            return dsDataSet;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filepath"></param>
        /// <param name="flatfileName"></param>
        /// <param name="format"></param>
        /// <param name="columnHeader"></param>
        /// <param name="MaxScanRows"></param>
        /// <param name="CharacterSet"></param>
        /// <param name="FixedColumnList"></param>
        /// <param name="TextDelimiter"></param>
        public static void CreateSchemaIni(String filepath, String flatfileName, String format, Boolean columnHeader, Int32 MaxScanRows, String CharacterSet, String[,] FixedColumnList, String TextDelimiter)
        {
            #region sample_schema.ini
            //this has been attempted with delimiters longer than 1 character but doesnt seem to work.
            //[rebates.txt]
            //Format=Delimited(|)
            //ColNameHeader=True
            //MaxScanRows=0
            //CharacterSet=ANSI
            //
            //[customers.txt]
            //Format=TabDelimited
            //ColNameHeader=True
            //MaxScanRows=0
            //CharacterSet=ANSI
            //
            //[orders.txt]
            //Format=Delimited(;)
            //ColNameHeader=True
            //MaxScanRows=0
            //CharacterSet=OEM
            //
            //[invoices.txt]
            //Format=FixedLength
            //ColNameHeader=False
            //Col1=FieldName1 Integer Width 15
            //Col2=FieldName2 Date Width 15
            //Col3=FieldName3 Char Width 40
            //Col4=FieldName4 Float Width 20
            //CharacterSet=ANSI
            #endregion

            String message = String.Empty;
            var traceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();

            DiagnosticActivity activity = new DiagnosticActivity();
            if (traceSettings.IsBasicTracingEnabled) activity.Start();

            String outputFile = filepath + @"\" + "schema.ini";
            FileStream fs = File.Create(outputFile);
            StreamWriter writer = new StreamWriter(fs);
            writer.WriteLine("[" + flatfileName + "]");
            writer.WriteLine("Format=" + format);
            writer.WriteLine("ColNameHeader=" + columnHeader.ToString());
            writer.WriteLine("MaxScanRows=" + MaxScanRows.ToString());

            // Now setting character set to unicode to support chinese/ hindi etc. lang.
            // writer.WriteLine("CharacterSet=Unicode");
            if (!String.IsNullOrEmpty(CharacterSet))
                writer.WriteLine("CharacterSet=" + CharacterSet);
            if (!String.IsNullOrEmpty(TextDelimiter))
                writer.WriteLine("TextDelimiter=" + TextDelimiter);
            Int32 i = 0;
            if (FixedColumnList != null)
            {
                #region "syntax of fixedcolumnlist"
                //Col1=CustomerNumber Text Width 10
                //Col2=CustomerName Text Width 30
                //The syntax of Coln is: (width and # are option on delimited files)
                //Coln=ColumnName type [Width #]
                #endregion "syntax of fixedcolumnlist"
                try
                {
                    while ((FixedColumnList[i, 0] != null) &&
                            (FixedColumnList[i, 1] != null) &&
                            (FixedColumnList[i, 2] != null) &&
                            (FixedColumnList[i, 3] != null))
                    {
                        writer.WriteLine("Col" + (i + 1) + "=" + FixedColumnList[i, 0] + " " + FixedColumnList[i, 1] + " " + FixedColumnList[i, 2] + " " + FixedColumnList[i, 3]);
                        i++;
                    }
                }
                catch (Exception ex)
                {
                    message = string.Format("Error reading schema ini file {0} - {1}", outputFile, ex.Message);
                    activity.LogError(message);
                }
            }

            writer.Close();
            fs.Close();

            if (traceSettings.IsBasicTracingEnabled) activity.Stop();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="completeFileName"></param>
        /// <param name="workSheetName"></param>
        /// <returns></returns>
        public static String ValidateExcelWorkSheet(String completeFileName, String workSheetName)
        {
            String validateExcelWorkSheetReturn = null;

            const String FileProvider = "Microsoft.ACE.OLEDB.12.0";
            const String ExcelConnectionFormat = "Provider={0};Data Source={1};Extended Properties='{2}'";
            const String ExtendedProperties = "Excel 12.0;HDR=Yes;IMEX=2";


            String excelConnectionString = String.Format(ExcelConnectionFormat, FileProvider, completeFileName, ExtendedProperties);

            // System.Data.OleDb.OleDbConnection ExcelConnection = new System.Data.OleDb.OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + CompleteFileName + @";Extended Properties=""Excel 8.0;IMEX=1""");
            System.Data.OleDb.OleDbConnection excelConnection = new System.Data.OleDb.OleDbConnection(excelConnectionString);

            excelConnection.Open();
            DataTable excelSheets = excelConnection.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables, new Object[] { null, null, null, "TABLE" });

            // Default to First sheet
            validateExcelWorkSheetReturn = excelSheets.Rows[0]["TABLE_NAME"].ToString();
            String workSheetNameExt = workSheetName + "$";

            for (Int32 i = 0; i < excelSheets.Rows.Count; i++)
            {
                if (excelSheets.Rows[i]["TABLE_NAME"].ToString() == workSheetName ||
                    excelSheets.Rows[i]["TABLE_NAME"].ToString() == "'" + workSheetName + "'" ||
                    excelSheets.Rows[i]["TABLE_NAME"].ToString() == workSheetNameExt ||
                    excelSheets.Rows[i]["TABLE_NAME"].ToString() == "'" + workSheetNameExt + "'")
                {
                    validateExcelWorkSheetReturn = excelSheets.Rows[i]["TABLE_NAME"].ToString();
                    break;
                }
            }
            excelConnection.Close();

            if (validateExcelWorkSheetReturn.IndexOf("'") == 0 && validateExcelWorkSheetReturn.LastIndexOf("'") == validateExcelWorkSheetReturn.Length - 1)
                validateExcelWorkSheetReturn = validateExcelWorkSheetReturn.Substring(1, validateExcelWorkSheetReturn.Length - 2);
            return validateExcelWorkSheetReturn;
        }

        /// <summary>
        ///  Validates excel worksheet using SAX parser
        /// </summary>
        /// <param name="fImportFile">Indicates import file name which needs to validate</param>
        /// <param name="workSheetName">Indicates name of worksheet</param>
        /// <returns>Returns string which contains sheet name</returns>
        public static String ValidateExcelWorkSheetSAXParser(String fImportFile, String workSheetName)
        {
            String message = String.Empty;
            var traceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();

            var activity = new DiagnosticActivity();
            if (traceSettings.IsBasicTracingEnabled) activity.Start();

            String validateExcelWorkSheetReturn = null;

            try
            {
            using (SpreadsheetDocument objSpreadSheetDoc = SpreadsheetDocument.Open(fImportFile, false))
            {
                if (objSpreadSheetDoc != null)
                {
                    WorkbookPart objWorkBookPart = objSpreadSheetDoc.WorkbookPart;
                    DFOS.Sheets objSheetNames = objWorkBookPart.Workbook.Sheets;
                    foreach (DFOS.Sheet objSheetName in objSheetNames)
                    {
                        if (objSheetName.Name == workSheetName)
                        {
                            validateExcelWorkSheetReturn = objSheetName.Name;
                        }
                    }
                }
            }
            }
            catch (FileFormatException ex)
            {
                message = "Unable to process input excel file, either the file is not of supported format (.xlsx) or the file is corrupt.";
                activity.LogError(message);

                throw new Exception(message, ex);
            }
            finally
            {
                if (traceSettings.IsBasicTracingEnabled) activity.Stop();
            }

            return validateExcelWorkSheetReturn;
        }

        /// <summary>
        /// Gets Worksheet
        /// </summary>
        /// <param name="workbookPart">The workbookPart</param>
        /// <param name="sheetName">The sheet name</param>
        /// <returns>The worksheet object</returns>
        public static DFOS.Worksheet GetWorksheet(WorkbookPart workbookPart, String sheetName)
        {
            DFOS.Worksheet worksheet = null;
            DFOS.Sheet sheet = workbookPart.Workbook.Descendants<DFOS.Sheet>().FirstOrDefault(s => s.Name == sheetName);
            if (sheet != null)
            {
                worksheet = ((WorksheetPart)workbookPart.GetPartById(sheet.Id)).Worksheet;
            }
            return worksheet;
        }

        /// <summary>
        /// Gets the Cell Values
        /// </summary>
        /// <param name="objCell">Indicates cell object</param>
        /// <param name="objWorkBookPart">Indicates work book part for which needs to get cell value</param>
        /// <param name="sharedStringItemArray">The shared string item array.</param>
        private static String GetCellValue(DFOS.Cell objCell, WorkbookPart objWorkBookPart, DFOS.SharedStringItem[] sharedStringItemArray)
        {
            String strCellValue = null;
            Double dateCellVlaue = 0;

            if (objCell == null)
            {
                strCellValue = null;
            }
            else
            {
                var objCellValue = objCell.CellValue;
                if (objCell.DataType != null && objCell.DataType == DFOS.CellValues.SharedString)
                {
                    DFOS.SharedStringItem objSharedStrItem = sharedStringItemArray[Int32.Parse(objCell.CellValue.InnerText)];
                    if (objSharedStrItem.Text != null)
                    {
                        strCellValue = objSharedStrItem.Text.Text;
                    }
                    else if (objSharedStrItem.Elements() != null && objSharedStrItem.Elements().Any())
                    {
                        //When two-byte characters (Chinese) are present or cell text has varying styles in the cell text partially,
                        //Excel saves it as rich text content in the open XML and hence the text will be empty.
                        //Hence we have to check if there are elements inside the shared string, get the concatenated string value of the elements which is provided by the InnerText property
                        strCellValue = objSharedStrItem.InnerText;
                    }
                }
                else
                {
                    strCellValue = (objCellValue == null) ? objCell.InnerText : objCellValue.Text;

                    if (objCell.StyleIndex != null)
                    {
                        DFOS.CellFormat cf = (DFOS.CellFormat)objWorkBookPart.WorkbookStylesPart.Stylesheet.CellFormats.ChildElements[Int32.Parse(objCell.StyleIndex.InnerText)];
                        if ((cf.NumberFormatId >= 14 && cf.NumberFormatId <= 22) || (cf.NumberFormatId >= 164 && cf.NumberFormatId <= 180))
                        {
                            if (Double.TryParse(strCellValue, out dateCellVlaue))
                                strCellValue = MDM.Core.FormatHelper.StoreDateTimeUtc(DateTime.FromOADate(dateCellVlaue));
                        }
                    }
                }
            }
            return strCellValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objCell"></param>
        /// <param name="currentCount"></param>
        /// <returns></returns>
        public static IEnumerable<DFOS.Cell> GetCellsFromRowIncludingEmptyCells(DFOS.Cell objCell, Int32 currentCount)
        {
            String strColumnName = GetColumnName(objCell.CellReference);
            Int32 icurrentColumnIndex = ConvertColumnNameToNumber(strColumnName);
            //Return null for empty cells
            for (; currentCount < icurrentColumnIndex; currentCount++)
            {
                yield return null;
            }
            yield return objCell;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public static Int32 ConvertColumnNameToNumber(String columnName)
        {
            Regex alpha = new Regex("^[A-Z]+$");
            if (!alpha.IsMatch(columnName)) throw new ArgumentException();

            char[] colLetters = columnName.ToCharArray();
            Array.Reverse(colLetters);

            Int32 convertedValue = 0;
            for (Int32 i = 0; i < colLetters.Length; i++)
            {
                char letter = colLetters[i];
                Int32 current = i == 0 ? letter - 65 : letter - 64; // ASCII 'A' = 65
                convertedValue += current * (Int32)Math.Pow(26, i);
            }

            return convertedValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cellReference"></param>
        /// <returns></returns>
        public static String GetColumnName(String cellReference)
        {
            var regex = new Regex("[A-Za-z]+");
            var match = regex.Match(cellReference);
            return match.Value;

        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Read the Excel Sheet from given file using SAX Parsing Reader
        /// </summary>
        /// <param name="SheetName">Indicates sheet Name</param>
        /// <param name="objSpreadSheetDoc">Indicates spread sheet document for which needs to get excel file data</param>
        /// <returns>DataSet representing the excel sheet</returns>
        private static DataTable GetExcelFileData(String SheetName, SpreadsheetDocument objSpreadSheetDoc)
        {
            String message = String.Empty;
            var traceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();

            DiagnosticActivity activity = new DiagnosticActivity();
            if (traceSettings.IsBasicTracingEnabled) activity.Start();

            DataTable dtImportDataTable = new DataTable();
            if (objSpreadSheetDoc != null)
            {
                WorkbookPart objWorkBookPart = objSpreadSheetDoc.WorkbookPart;
                
                try
                {
                    IEnumerable<DFOS.SharedStringItem> sharedStringItems = objWorkBookPart.SharedStringTablePart.SharedStringTable.Elements<DFOS.SharedStringItem>();
                    DFOS.SharedStringItem[] sharedStringItemArray = new DFOS.SharedStringItem[0];

                    if (sharedStringItems != null && sharedStringItems.Any())
                    {
                        sharedStringItemArray = sharedStringItems.ToArray();
                    }

                    DFOS.Worksheet objWorkSheet = GetWorksheet(objWorkBookPart, SheetName);
                    OpenXmlReader objXMLReader = OpenXmlReader.Create(objWorkSheet.WorksheetPart);

                    while (objXMLReader.Read())
                    {
                        if (objXMLReader.ElementType == typeof(DFOS.SheetData))
                        {
                            do
                            {
                                if (objXMLReader.ElementType == typeof(DFOS.Row))
                                {
                                    DataRow newDataRow = dtImportDataTable.NewRow();
                                    DFOS.Row objRow = objXMLReader.LoadCurrentElement() as DFOS.Row;
                                    Int32 currentCount = 0;
                                    Int32 columnCount = 0;
                                    Boolean rowHasValue = false;

                                    foreach (OpenXmlElement objChildElement in objRow.ChildElements)
                                    {
                                        if (objRow.RowIndex == 1)
                                        {
                                            if (objChildElement.GetType() == typeof(DFOS.Cell))
                                            {
                                                String strColumnValue = null;
                                                DFOS.Cell objCell = (DFOS.Cell)objChildElement;
                                                // If cell doesn't have value OpenXML API is not returning Cell thus adding null cell explicitly as a workaround.
                                                IEnumerable<DFOS.Cell> cells = GetCellsFromRowIncludingEmptyCells(objCell, currentCount);
                                                currentCount += cells.Count() - 1;
                                                foreach (DFOS.Cell objCellValue in cells)
                                                {
                                                    strColumnValue = GetCellValue(objCellValue, objWorkBookPart, sharedStringItemArray);

                                                    if (!String.IsNullOrWhiteSpace(strColumnValue))
                                                    {
                                                        if (dtImportDataTable.Columns.Contains(strColumnValue))
                                                        {
                                                            message = String.Format("Load failed due to duplicate column name ('{0}'). Please correct the duplicate column issue and reprocess the file.", strColumnValue);
                                                            activity.LogError(message);
                                                            throw new DuplicateNameException(message);
                                                        }
                                                    }

                                                    if (SheetName == MetadataSheetName || !String.IsNullOrWhiteSpace(strColumnValue))
                                                    {
                                                        dtImportDataTable.Columns.Add(strColumnValue);
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (objChildElement.GetType() == typeof(DFOS.Cell))
                                            {
                                                String strColumnValue = null;
                                                DFOS.Cell objCell = (DFOS.Cell)objChildElement;
                                                // If cell don't have value OpenXML API is not notifying thus adding null cell explicitly as a workaround.
                                                IEnumerable<DFOS.Cell> cells = GetCellsFromRowIncludingEmptyCells(objCell, currentCount);
                                                currentCount += cells.Count() - 1;
                                                foreach (DFOS.Cell objCellValue in cells)
                                                {
                                                    strColumnValue = GetCellValue(objCellValue, objWorkBookPart, sharedStringItemArray);
                                                    if (!rowHasValue && !String.IsNullOrEmpty(strColumnValue))
                                                    {
                                                        rowHasValue = true;
                                                    }
                                                    if (columnCount < dtImportDataTable.Columns.Count)
                                                    {
                                                        newDataRow[columnCount++] = strColumnValue;
                                                    }
                                                }
                                            }
                                        }
                                        currentCount++;
                                    }
                                    if (SheetName == MetadataSheetName || (objRow.RowIndex == 1 || (rowHasValue && objRow.RowIndex > 1)))
                                    {
                                        dtImportDataTable.Rows.Add(newDataRow);
                                    }
                                }
                            } while (objXMLReader.Read());
                        }
                    }
                    objXMLReader.Close();
                    dtImportDataTable.Rows.RemoveAt(0);
                }
                catch (Exception ex)
                {
                    message = String.Format("Error reading Excel data : {0}", ex.Message);
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, message, Core.MDMTraceSource.Imports);
                    throw ex;
                }
            }

            if (traceSettings.IsBasicTracingEnabled) activity.Stop();

            return dtImportDataTable;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CompleteFileName"></param>
        private static DataSet LoadRsXmlIntoDataSet(String CompleteFileName)
        {
            string message = string.Empty;
            var traceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();

            DiagnosticActivity activity = new DiagnosticActivity();
            if (traceSettings.IsBasicTracingEnabled) activity.Start();

            DataTable dtXml = new DataTable();
            Hashtable drHash = new Hashtable();
            #region "build the dataset headers"
            try
            {
                DataSet newDataSet = new DataSet("New DataSet");
                // Read the XML document into the DataSet.
                newDataSet.ReadXml(CompleteFileName);

                for (Int32 x = 0; x <= newDataSet.Tables.Count - 1; x++)
                {
                    //set the column headers
                    String colName = "";
                    switch (newDataSet.Tables[x].TableName.ToLower())
                    {
                        case "attr":
                            ArrayList columnNames = new ArrayList();
                            columnNames.Add("name"); //use the value in column data
                            DataTable newTable = newDataSet.Tables[x].DefaultView.ToTable("UniqueData", true, (String[])columnNames.ToArray(typeof(String)));
                            for (Int32 i = 0; i <= newTable.Rows.Count - 1; i++)
                            {
                                for (Int32 hj = 0; hj <= columnNames.Count - 1; hj++)
                                {
                                    switch (columnNames[hj].ToString().ToLower())
                                    {
                                        case "name":
                                            colName = "[" + "attr" + "].[" + newTable.Rows[i][columnNames[hj].ToString()].ToString() + ".path]";
                                            DataColumn newAttrColumns = new DataColumn(colName.ToLower(), typeof(System.String));
                                            dtXml.Columns.Add(newAttrColumns);

                                            colName = "[" + "attr" + "].[" + newTable.Rows[i][columnNames[hj].ToString()].ToString() + ".value]";
                                            newAttrColumns = new DataColumn(colName.ToLower(), typeof(System.String));
                                            dtXml.Columns.Add(newAttrColumns);
                                            break;
                                        default: break;
                                    }
                                }
                            }
                            break;
                        default:
                            for (Int32 z = 0; z <= newDataSet.Tables[x].Columns.Count - 1; z++)
                            {
                                colName = "[" + newDataSet.Tables[x].TableName + "].[" + newDataSet.Tables[x].Columns[z].ColumnName + "]";
                                if (colName.Contains("_id") == false)
                                {
                                    DataColumn newColumns = new DataColumn(colName.ToLower(), typeof(System.String));
                                    dtXml.Columns.Add(newColumns);
                                }
                            }
                            break;
                    }
                }
                newDataSet = null;
            }
            catch (Exception ex)
            {
                activity.LogError(ex.Message);
                throw ex;
            }

            #endregion "build the dataset headers"

            XmlDocument Xmldoc = new XmlDocument();

            try
            {
                String tableName = "";
                String brac = "";
                Xmldoc.Load(CompleteFileName);
                XmlNodeList NodeList = Xmldoc.SelectNodes("//Data/Organization/Catalog/Items/Item");
                foreach (XmlNode xmlNode in NodeList)
                {
                    tableName = "[item].[";
                    brac = "]";
                    //Data/Organization/Catalog/Items/Item
                    ArrayList itemAttrNames = new ArrayList();
                    itemAttrNames.Add("id");
                    itemAttrNames.Add("shortname");
                    itemAttrNames.Add("longname");
                    itemAttrNames.Add("action");
                    itemAttrNames.Add("categorypath");
                    itemAttrNames.Add("type");
                    for (Int32 x = 0; x <= itemAttrNames.Count - 1; x++)
                    {
                            AddKeytoHashTable(drHash, tableName + itemAttrNames[x].ToString() + brac, xmlNode.Attributes.GetNamedItem(itemAttrNames[x].ToString()).Value);
                            //dr[tableName + itemAttrNames[x].ToString() + brac] = xmlNode.Attributes.GetNamedItem(itemAttrNames[x].ToString()).Value;
                        }

                    #region parentNodes
                    /*
                    //Data/Organization/Catalog/Items
                    ArrayList itemsAttrNames = new ArrayList();
                    itemsAttrNames.Add("id");
                    for (Int32 x=0; itemsAttrNames.Count; x++)
                    {       
                        try
                        {
                            dr[itemAttrNames[x].ToString()] = xmlNode.ParentNode.Attributes.GetNamedItem(itemsAttrNames[x].ToString());                            
                        }
                        catch (Exception ex) {}
                    }
                    */

                    //Data/Organization/Catalog
                    tableName = "[catalog].[";
                    ArrayList catalogAttrNames = new ArrayList();
                    catalogAttrNames.Add("name");
                    catalogAttrNames.Add("id");
                    catalogAttrNames.Add("locator");
                    for (Int32 x = 0; x < catalogAttrNames.Count; x++)
                    {
                            AddKeytoHashTable(drHash, tableName + catalogAttrNames[x].ToString() + brac, xmlNode.ParentNode.ParentNode.Attributes.GetNamedItem(catalogAttrNames[x].ToString()).Value);
                            //dr[tableName + catalogAttrNames[x].ToString() + brac] = xmlNode.ParentNode.ParentNode.Attributes.GetNamedItem(catalogAttrNames[x].ToString()).Value;
                        }

                    //Data/Organization
                    tableName = "[organization].[";
                    ArrayList orgAttrNames = new ArrayList();
                    orgAttrNames.Add("name");
                    orgAttrNames.Add("id");
                    orgAttrNames.Add("locator");
                    for (Int32 x = 0; x < orgAttrNames.Count; x++)
                    {
                            AddKeytoHashTable(drHash, tableName + orgAttrNames[x].ToString() + brac, xmlNode.ParentNode.ParentNode.ParentNode.Attributes.GetNamedItem(orgAttrNames[x].ToString()).Value);
                            //dr[tableName + orgAttrNames[x].ToString() + brac] = xmlNode.ParentNode.ParentNode.ParentNode.Attributes.GetNamedItem(orgAttrNames[x].ToString()).Value;
                        }

                    #endregion parentnodes

                    #region childnodes
                    XmlNodeList AttributesNodeList = xmlNode.ChildNodes;
                    foreach (XmlNode xmlAttributeNode in AttributesNodeList)
                    {
                        switch (xmlAttributeNode.Name)
                        {
                            case "Attributes":
                                #region Attributes

                                tableName = "[attributes].[";
                                ArrayList AttributesNames = new ArrayList();
                                AttributesNames.Add("type");
                                //Data/Organization/Catalog/Items/Item/Attributes
                                for (Int32 x = 0; x < AttributesNames.Count; x++)
                                {
                                        AddKeytoHashTable(drHash, tableName + AttributesNames[x].ToString() + brac, xmlAttributeNode.Attributes.GetNamedItem(AttributesNames[x].ToString()).Value);
                                        //dr[tableName + AttributesNames[x].ToString() + brac] = xmlAttributeNode.Attributes.GetNamedItem(AttributesNames[x].ToString()).Value;
                                    }

                                //Data/Organization/Catalog/Items/Item/Attr                                
                                XmlNodeList AttrNodeList = xmlAttributeNode.ChildNodes;
                                foreach (XmlNode xmlAttrNode in AttrNodeList)
                                {
                                    tableName = "[attr].[";
                                    ArrayList AttrNames = new ArrayList();
                                    AttrNames.Add("name");
                                    AttrNames.Add("path");
                                    AttrNames.Add("value");

                                    //Data/Organization/Catalog/Items/Item/Attributes
                                    String colName = "";
                                    String colPath = "";
                                    String colValue = "";
                                    for (Int32 x = 0; x < AttrNames.Count; x++)
                                    {
                                            switch (AttrNames[x].ToString())
                                            {
                                                case "name":
                                                    colName = xmlAttrNode.Attributes.GetNamedItem(AttrNames[x].ToString()).Value;
                                                    break;
                                                case "path":
                                                    colPath = xmlAttrNode.Attributes.GetNamedItem(AttrNames[x].ToString()).Value;
                                                    break;
                                                case "value":
                                                    colValue = xmlAttrNode.Attributes.GetNamedItem(AttrNames[x].ToString()).Value;
                                                    break;
                                            }
                                    }

                                        AddKeytoHashTable(drHash, tableName + colName + ".path]", colPath);
                                        //dr[tableName + colName + ".path]"] = colPath;
                                        AddKeytoHashTable(drHash, tableName + colName + ".value]", colValue);
                                        //dr[tableName + colName + ".value]"] = colValue;
                                    }

                                //attributes go out on columns
                                DataRow dr = dtXml.NewRow();
                                dr = CreateDataRow(dtXml, drHash);
                                dtXml.Rows.Add(dr);
                                drHash.Clear();
                                #endregion
                                break;
                            case "Relationships":
                                #region Relationships

                                //the parent is relationships OR attributes... dont get confused
                                //Data/Organization/Catalog/Items/Item/Relationships/Relationship
                                XmlNodeList RelationshipNodeList = xmlAttributeNode.ChildNodes;
                                foreach (XmlNode RelationshipNode in RelationshipNodeList)
                                {
                                    tableName = "[Relationship].[";
                                    ArrayList RelationshipsNames = new ArrayList();
                                    RelationshipsNames.Add("type");
                                    RelationshipsNames.Add("action");
                                    RelationshipsNames.Add("shortname");
                                    RelationshipsNames.Add("longname");
                                    RelationshipsNames.Add("sku");
                                    RelationshipsNames.Add("categorypath");
                                    RelationshipsNames.Add("id");
                                    RelationshipsNames.Add("fromitemshortname");
                                    RelationshipsNames.Add("fromitemsku");
                                    RelationshipsNames.Add("fromitemcategorypath");
                                    RelationshipsNames.Add("toitemshortname");
                                    RelationshipsNames.Add("toitemsku");
                                    RelationshipsNames.Add("toitemcategorypath");

                                    for (Int32 x = 0; x < RelationshipsNames.Count; x++)
                                    {
                                            AddKeytoHashTable(drHash, tableName + RelationshipsNames[x].ToString() + brac, RelationshipNode.Attributes.GetNamedItem(RelationshipsNames[x].ToString()).Value);
                                            //dr[tableName + RelationshipsNames[x].ToString() + brac] = RelationshipNode.Attributes.GetNamedItem(RelationshipsNames[x].ToString()).Value;
                                        }
                                    //Data/Organization/Catalog/Items/Item/Relationships/Relationship/Attributes
                                    XmlNodeList RelatedItemList = RelationshipNode.ChildNodes;
                                    foreach (XmlNode RelatedItemNode in RelatedItemList)
                                    {
                                        tableName = "[Attributes].[";
                                        ArrayList RelatedItemNames = new ArrayList();
                                        RelatedItemNames.Add("shortname");
                                        RelatedItemNames.Add("longname");
                                        RelatedItemNames.Add("categorypath");
                                        RelatedItemNames.Add("type");
                                        RelatedItemNames.Add("id");
                                        RelatedItemNames.Add("action");
                                        for (Int32 x = 0; x < RelatedItemNames.Count; x++)
                                        {
                                                AddKeytoHashTable(drHash, tableName + RelatedItemNames[x].ToString() + brac, RelatedItemNode.Attributes.GetNamedItem(RelatedItemNames[x].ToString()).Value);
                                                //dr[tableName + RelatedItemNames[x].ToString() + brac] = RelatedItemNode.Attributes.GetNamedItem(RelatedItemNames[x].ToString()).Value;
                                            }

                                        //Data/Organization/Catalog/Items/Item/Relationships/Relationship/Attributes/Attr
                                        XmlNodeList RelationshipAttributesList = RelatedItemNode.ChildNodes;
                                        foreach (XmlNode RelationshipAttributesNode in RelationshipAttributesList)
                                        {
                                            #region RelationshipAttributes

                                            tableName = "[Attr].[";
                                            ArrayList RelationshipAttributesNames = new ArrayList();
                                            RelationshipAttributesNames.Add("type");
                                            RelationshipAttributesNames.Add("name");
                                            RelationshipAttributesNames.Add("value");
                                            //Data/Organization/Catalog/Items/Item/Attributes
                                            String colName = "";
                                            String colPath = "";
                                            String colValue = "";
                                            for (Int32 x = 0; x < RelationshipAttributesNames.Count; x++)
                                            {
                                                    switch (RelationshipAttributesNames[x].ToString())
                                                    {
                                                        case "name":
                                                            colName = RelationshipAttributesNode.Attributes.GetNamedItem(RelationshipAttributesNames[x].ToString()).Value;
                                                            break;
                                                        case "path":
                                                            colPath = RelationshipAttributesNode.Attributes.GetNamedItem(RelationshipAttributesNames[x].ToString()).Value;
                                                            break;
                                                        case "value":
                                                            colValue = RelationshipAttributesNode.Attributes.GetNamedItem(RelationshipAttributesNames[x].ToString()).Value;
                                                            break;
                                                    }
                                            }

                                                AddKeytoHashTable(drHash, tableName + colName + ".path]", colPath);
                                                //dr[tableName + colName + ".path]"] = colPath;
                                                AddKeytoHashTable(drHash, tableName + colName + ".value]", colValue);
                                                //dr[tableName + colName + ".value]"] = colValue;

                                            //attributes go out on columns
                                            DataRow drRelationships = dtXml.NewRow();
                                            drRelationships = CreateDataRow(dtXml, drHash);
                                            dtXml.Rows.Add(drRelationships);
                                            //drHash.Clear();
                                            #endregion
                                        }
                                    }
                                }
                                #endregion
                                break;
                            default:
                                break;
                        }
                    }
                    #endregion childnodes
                }

                DataSet newDataSet = new DataSet();
                newDataSet.Tables.Add(dtXml);
                return newDataSet;
            }
            catch (Exception ex)
            {
                message = string.Format("Exception converting XML to dataset - {0}", ex.Message);
                activity.LogWarning(message);

                try
            {
                DataSet newDataSet = new DataSet();
                newDataSet.ReadXml(CompleteFileName);
                return newDataSet;
            }
                catch (Exception innerEx)
                {
                    message = string.Format("Error converting XML to dataset - {0}", innerEx.Message);
                    activity.LogError(message);
                }
            }
            finally
            {
                if (traceSettings.IsBasicTracingEnabled) activity.Stop();
            }

            return new DataSet();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ht"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        private static void AddKeytoHashTable(Hashtable ht, String key, String value)
        {
            if (ht.ContainsKey(key))
                ht.Remove(key);

            ht.Add(key, value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="drHash"></param>
        /// <returns></returns>
        private static DataRow CreateDataRow(DataTable dt, Hashtable drHash)
        {
            string message = string.Empty;
            var traceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();

            DiagnosticActivity activity = new DiagnosticActivity();
            if (traceSettings.IsBasicTracingEnabled) activity.Start();

            DataRow dr = dt.NewRow();
            object[] keys = new object[drHash.Count];
            drHash.Keys.CopyTo(keys, 0);
            for (Int32 i = 0; i < keys.Length; i++)
            {
                try
                {
                    if (dt.Columns.Contains(keys[i].ToString()))
                        dr[keys[i].ToString()] = drHash[keys[i].ToString()];
                }
                catch (Exception ex)
                {
                    message = string.Format("Unable to create data row for key {0} - {1}", keys[i].ToString(), ex.Message);
                    activity.LogWarning(message);
                }
            }

            if (traceSettings.IsBasicTracingEnabled) activity.Stop();

            return dr;
        }

        #endregion

        #endregion
    }
}
