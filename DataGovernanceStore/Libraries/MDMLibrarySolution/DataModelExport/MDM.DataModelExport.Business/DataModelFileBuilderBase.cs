using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Collections.ObjectModel; 
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace MDM.DataModelExport.Business
{
    using MDM.Core;
    using MDM.Core.DataModel;
    using MDM.ExcelUtility;
    using MDM.Interfaces;
    using MDM.Utility;
    using BO = MDM.BusinessObjects;
    using MDM.BusinessObjects.Exports;   

    /// <summary>
    /// Class provides base functionality for writing Data model objects into excel file.
    /// </summary>
    internal abstract class DataModelFileBuilderBase
    {
        #region Fields

        private String PATH_SEPERATOR = AppConfigurationHelper.GetAppConfig<String>("MDMCenter.RSDataModelExcel.Separator", "//").Trim();

        private readonly String[] CATEGORY_PATH_SEPERATOR = { AppConfigurationHelper.GetAppConfig<String>("Catalog.Category.PathSeparator", " » ") };

        protected LocaleEnum systemDataLocale = GlobalizationHelper.GetSystemDataLocale();

        protected const String ALL = "All";

        #endregion

        #region Public Methods

        /// <summary>
        /// Export the Data Model  excel file
        /// </summary>
        /// <param name="dataModelExportContext">Indicates data model export context</param>
        /// <param name="workbookPart">Indicates work book part where to poulate data model</param>
        /// <param name="callerContext">Caller context to denote application and module information</param>
        /// <returns>Returns template name as requested by the user. Also it populate the requested data model in the file and save it.</returns>
        public String PopulateDataModelInFile(DataModelExportContext dataModelExportContext, WorkbookPart workbookPart, BO.CallerContext callerContext)
        {
            String sheetName = String.Empty;

            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity(String.Format("DataModelFileBuilderBase.PopulateDataModelInFile for {0}", this.ObjectType),
                    MDMTraceSource.DataModelExport, false);
            }

            var durationHelper = new DurationHelper(DateTime.Now);
            var overallDurationHelper = new DurationHelper(DateTime.Now);
            IDataModelObjectCollection dataModelObjectCollection = null;

            dataModelObjectCollection = GetDataModelObjectCollection(dataModelExportContext, callerContext);

            if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - Loaded data model collection for {1}",
                    durationHelper.GetDurationInMilliseconds(DateTime.Now), this.ObjectType), MDMTraceSource.DataModelExport);
            }

            Worksheet workSheet = OpenSpreadsheetUtility.GetWorksheet(workbookPart, DataModelDictionary.ObjectsDictionary[this.ObjectType]);
            IEnumerable<DataValidations> dataValidations = workSheet.Descendants<DataValidations>();

            if (workSheet != null && dataModelObjectCollection != null)
            {
                List<String> headerlist = GetHeaderList(workbookPart, workSheet);

                WorksheetPart replacementPart = workbookPart.AddNewPart<WorksheetPart>();
                String replacementId = workbookPart.GetIdOfPart(replacementPart);

                WriteDataModelInExcel(workSheet, replacementPart, dataModelObjectCollection, headerlist);
                workbookPart.DeletePart(workSheet.WorksheetPart);

                sheetName = DataModelDictionary.ObjectsDictionary[this.ObjectType].ToString();

                Sheet dataSheet = workbookPart.Workbook.Descendants<Sheet>().FirstOrDefault(s => s.Name == sheetName);
                dataSheet.Id = replacementId;
                AppendDataValidations(workbookPart, dataValidations);
                workbookPart.Workbook.Save();
            }

            if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - Overall time for population of {1} data model in excel",
                    overallDurationHelper.GetDurationInMilliseconds(DateTime.Now), this.ObjectType), MDMTraceSource.DataModelExport);
            }

            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StopTraceActivity(String.Format("DataModelFileBuilderBase.PopulateDataModelInFile for {0}", this.ObjectType), MDMTraceSource.DataModelExport);
            }

            return sheetName;
        }

        #endregion

        #region Abstract Methods & Properties

        /// <summary>
        /// Returns the data model object collection.
        /// </summary>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        protected abstract IDataModelObjectCollection GetDataModelObjectCollection(BO.CallerContext callerContext);

        /// <summary>
        /// Returns the data model object collection.
        /// </summary>
        /// <param name="dataModelExportContext">Indicates data model export context based on which needs to export data model</param>
        /// <param name="callerContext">Indicates caller context to denote application and module information</param>
        /// <returns></returns>
        protected abstract IDataModelObjectCollection GetDataModelObjectCollection(DataModelExportContext dataModelExportContext, BO.CallerContext callerContext);

        /// <summary>
        /// Populates the data model object in the specified row.
        /// </summary>
        /// <param name="dataModelObject"></param>
        /// <param name="headerList"></param>
        /// <param name="dataRow"></param>
        protected abstract void PopulateDataModelObjectInDataRow(IDataModelObject dataModelObject, List<String> headerList, Row dataRow);

        /// <summary>
        /// Represents the object type for which the class instance is created.
        /// </summary>
        protected abstract ObjectType ObjectType { get; }

        #endregion

        #region Protected Methods

        protected void PopulateDataModelBasePropertiesInDataRow(IDataModelObject dataModelObject, List<String> headerList, Row dataRow, ref UInt32 columnIndex)
        {
            if (headerList.Contains(DataModelDictionary.CommonHeadersDictionary[DataModelCommonHeaders.Id]))
            {
                OpenSpreadsheetUtility.AppendRowWithNumberCell(dataRow, columnIndex++, dataModelObject.Id);
            }

            if (headerList.Contains(DataModelDictionary.CommonHeadersDictionary[DataModelCommonHeaders.Action]))
            {
                //Action should not get displyed in the exported file, that's why changed it to empty.
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, String.Empty);
            }

            if (headerList.Contains(DataModelDictionary.CommonHeadersDictionary[DataModelCommonHeaders.UniqueIdentifier]))
            {
                //TODO : As of now UniqueIdentifier is set as empty.
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, String.Empty);
            }
        }

        protected String GetAttributePath(String attributeParentName, String attributeName)
        {
            return String.Format("{0}{1}{2}", attributeParentName, PATH_SEPERATOR, attributeName);
        }

        //protected String GetAttributePath(String attributeType, String attributeParentName, String attributeName)
        //{
        //    return String.Format("{0}{1}{2}{3}{4}", attributeType, PATH_SEPERATOR, attributeParentName, PATH_SEPERATOR, attributeName);
        //}

        protected String ConvertBooleanValuesToString(Boolean value)
        {
            String returnValue = String.Empty;

            if (value)
            {
                returnValue = "YES";
            }
            else
            {
                returnValue = "NO";
            }

            return returnValue;
        }

        protected String GetParentCategoryPath(String path)
        {
            String parentCategoryPath = String.Empty;

            if (!String.IsNullOrWhiteSpace(path))
            {
                String[] values = path.Trim().Split(CATEGORY_PATH_SEPERATOR, StringSplitOptions.RemoveEmptyEntries);

                if (values != null && (values.Length - 1) > 0)
                {
                    
                    String pathSepartor = PATH_SEPERATOR;
 
                    for (Int32 index = 0; index < (values.Length - 1); index++)
                    {
                        parentCategoryPath = String.Concat(parentCategoryPath, pathSepartor, values[index]);
                    }

                    parentCategoryPath = parentCategoryPath.Substring(pathSepartor.Length);
                }
            }

            return parentCategoryPath;
        }

        #endregion

        #region Private Methods

        private List<String> GetHeaderList(WorkbookPart workbookPart, Worksheet workSheet)
        {
            UInt32Value rowIndex = 1;

            SheetData sheetData = workSheet.GetFirstChild<SheetData>();
            Row headerRow = OpenSpreadsheetUtility.GetRow(sheetData, rowIndex);

            List<Int32> sharedStringIds = headerRow.Elements<Cell>().Select(cell => Int32.Parse(cell.InnerText)).ToList();
            List<String> headerlist = sharedStringIds.Select(i => OpenSpreadsheetUtility.GetSharedStringItemById(workbookPart, i).InnerText).ToList();
            return headerlist;
        }

        private void WriteDataModelInExcel(Worksheet workSheet, WorksheetPart replacementPart, IDataModelObjectCollection dataModelObjectCollection, List<String> headerList)
        {
            using (OpenXmlReader reader = OpenXmlReader.Create(workSheet.WorksheetPart))
            {
                using (OpenXmlWriter writer = OpenXmlWriter.Create(replacementPart))
                {
                    while (reader.Read())
                    {
                        if (reader.ElementType == typeof(SheetData))
                        {
                            if (reader.IsEndElement)
                            {
                                continue;
                            }

                            WriteDataModelUsingOpenXmlWriter(writer, workSheet, dataModelObjectCollection, headerList);
                        }
                        else if (reader.ElementType == typeof(Drawing) || reader.ElementType == typeof(LegacyDrawing))
                        {
                        }
                        else
                        {
                            if (reader.IsStartElement)
                            {
                                writer.WriteStartElement(reader);
                            }
                            else if (reader.IsEndElement)
                            {
                                writer.WriteEndElement();
                            }
                        }
                    }
                }
            }
        }

        private void WriteDataModelUsingOpenXmlWriter(OpenXmlWriter writer, Worksheet workSheet, IDataModelObjectCollection dataModelObjectCollection, List<String> headerList)
        {
            DurationHelper durationHelper = new DurationHelper(DateTime.Now);

            SheetData headerSheetData = workSheet.WorksheetPart.Worksheet.Elements<SheetData>().First();
            writer.WriteStartElement(new SheetData());

            foreach (Row row in headerSheetData.Elements<Row>())
            {
                WriteRow(writer, row);
            }

            PopulateDataModelObjectCollectionInExcel(writer, dataModelObjectCollection, headerList);

            if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - Populated {1} rows of {2} data model in excel",
                    durationHelper.GetDurationInMilliseconds(DateTime.Now), dataModelObjectCollection.Count, this.ObjectType), MDMTraceSource.DataModelExport);
            }

            writer.WriteEndElement();
        }

        private void PopulateDataModelObjectCollectionInExcel(OpenXmlWriter writer, IDataModelObjectCollection dataModelObjectCollection, List<String> headerList)
        {
            UInt32Value rowIndex = 1;
            foreach (IDataModelObject dataModelObject in dataModelObjectCollection)
            {
                Row dataRow = new Row();
                dataRow.RowIndex = ++rowIndex;

                PopulateDataModelObjectInDataRow(dataModelObject, headerList, dataRow);
             
                WriteRow(writer, dataRow);
            }
        }

        private void WriteRow(OpenXmlWriter writer, Row row)
        {
            writer.WriteStartElement(row);
            foreach (Cell cell in row.Elements<Cell>())
            {
                writer.WriteElement(cell);
            }
            writer.WriteEndElement();
        }

        private void AppendDataValidations(WorkbookPart workbookPart, IEnumerable<DataValidations> dataValidations)
        {
            Worksheet workSheet = OpenSpreadsheetUtility.GetWorksheet(workbookPart, DataModelDictionary.ObjectsDictionary[this.ObjectType]);

            if (workSheet == null)
            {
                return;
            }

            List<List<DataValidation>> dataValidationsList = new List<List<DataValidation>>();

            foreach (DataValidations validations in workSheet.Descendants<DataValidations>())
            {
                List<DataValidation> dataValidationList = new  List<DataValidation>();

                dataValidationsList.Add(dataValidationList);

                foreach (DataValidation validation in validations)
                {
                    dataValidationList.Add(validation);
                }

            }

            int validationsIndex = 0, validationIndex = 0;

            foreach (DataValidations sourceDataValidations in dataValidations)
            {
                validationIndex = 0;
                
                foreach (DataValidation sourceDataValidation in sourceDataValidations)
                {
                    dataValidationsList[validationsIndex][validationIndex].InnerXml = sourceDataValidation.InnerXml;
                    validationIndex++;
                }

                validationsIndex++;
            }
        }

        #endregion
    }
}