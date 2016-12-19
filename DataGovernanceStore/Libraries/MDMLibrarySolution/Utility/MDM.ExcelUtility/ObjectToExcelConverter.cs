using System;
using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using MDM.AdminManager.Business;
using MDM.BusinessObjects;
using MDM.Core;
using MDM.ExcelUtility.RSExcelStyling;
using MDM.ExcelUtility.RSExcelValidation;
using MDM.Interfaces;
using MDM.Utility;
using File = System.IO.File;
using Row = DocumentFormat.OpenXml.Spreadsheet.Row;
using Value = MDM.BusinessObjects.Value;

namespace MDM.ExcelUtility
{
    public class ObjectToExcelConverter
    {
        /// <summary>
        /// Generates the excel file.
        /// </summary>
        /// <param name="objects">The objects.</param>
        /// <param name="models">The models.</param>
        /// <param name="fullFileName">Full name of the file.</param>
        /// <param name="creationOptions">The creation options.</param>
        public void GenerateRSExcel(List<BusinessObject> objects, List<AttributeModel> models, string fullFileName, RSExcelCreationOptions creationOptions)
        {
            String templateFileName = RSExcelUtility.GetTemplateFileName("Dummy", GetTemporaryFileLocation());

            File.Copy(templateFileName, fullFileName, true);

            /**********************************************************************************************************
             While debugging RSExcel Export in local environment and If you are getting Access Denied for fullFileName 
             at that time make sure default RSExcelTemplate is not set in ReadOnly mode.
            **********************************************************************************************************/
            SpreadsheetDocument document = SpreadsheetDocument.Open(fullFileName, true);

            if (document != null)
            {
                RsExcelDocumentContentVerification.ReviewAndCleanContent(document);

                WorkbookPart workbookPart = document.WorkbookPart;

                if (workbookPart != null)
                {
                    PopulateEntitySheet(workbookPart, objects, models, creationOptions);
                }

                document.Close();
            }
        }

        #region Private methods

        private static string GetTemporaryFileLocation()
        {
            //return AppConfigurationHelper.GetAppConfig<String>(this.IsJobServiceRequest ? "Jobs.TemporaryFileRoot" : "WCF.TemporaryFileRoot");
            return AppConfigurationHelper.GetAppConfig<String>("WCF.TemporaryFileRoot");
        }

        //todo[mv]: clear that out
        private static void PopulateEntitySheet(WorkbookPart workbookPart, IEnumerable<BusinessObject> objects, List<AttributeModel> models, RSExcelCreationOptions creationOptions)
        {
            Worksheet workSheet = OpenSpreadsheetUtility.GetWorksheet(workbookPart, RSExcelConstants.EntityDataSheetName);

            if (workSheet != null)
            {
                SheetData sheetData = workSheet.GetFirstChild<SheetData>();

                UInt32 rowIndex = 1; // Header row is always at first row..means index = 0;

                Row headerRow = OpenSpreadsheetUtility.GetRow(sheetData, rowIndex);
                List<Int32> sharedStringIds = headerRow.Elements<DocumentFormat.OpenXml.Spreadsheet.Cell>().Select(cell => Int32.Parse(cell.InnerText)).ToList();
                List<String> metadataAttributes = sharedStringIds.Select(i => OpenSpreadsheetUtility.GetSharedStringItemById(workbookPart, i).InnerText).ToList();

                // Get the style index from the first column
                DocumentFormat.OpenXml.Spreadsheet.Cell headerFirstCell = OpenSpreadsheetUtility.GetDataCell(sheetData, RSExcelUtility.ColumnOne, rowIndex);
                rowIndex++;

                #region Stylesheet

                Stylesheet stylesheet = workbookPart.WorkbookStylesPart.Stylesheet;

                // Applying styles from styles template in case it is found
                Boolean useStylesTemplate = AppConfigurationHelper.GetAppConfig("MDM.Exports.RSExcelFormatter.UseStylesTemplate", false);

                WorksheetStyles worksheetStyles = null;

                if (useStylesTemplate)
                {
                    Template template = new ExportTemplateBL().GetExportTemplateByName("Default_RSexcel11_Style_Template", 
                        new CallerContext(MDMCenterApplication.PIM, MDMCenterModules.Export));

                    if (template != null)
                    {
                        String stylesTemplateFileName = RSExcelUtility.GetTemplateFileName("Default_RSexcel11_Style_Template", GetTemporaryFileLocation());
                        worksheetStyles = StylesTemplateProcessor.ProcessStylesTemplateFile(stylesTemplateFileName);

                        RSExcelUtility.UpdateMetadataAttributesStyle(headerRow, stylesheet, worksheetStyles);
                    }
                }

                RSExcelUtility.ApplyStyleSheet(stylesheet, worksheetStyles);

                #endregion

                // Localized Validation messages from DB
                IDictionary<String, String> messages = new ValidationMessagesProvider().GetAllValidationMessagesDictionary();

                // Populate attribute header cells
                 RSExcelUtility.PopulateAttributeHeaderCells(workSheet.WorksheetPart, headerRow, models, creationOptions, OpenSpreadsheetUtility.GetCellStyleIndex(headerFirstCell), messages);

                // Populate sheet with validation restrictions
                Int32 skipColumns = metadataAttributes.Count;

                // Creating collection of column numbers / styles
                Int32 columnIndex = 1;
                List<DocumentFormat.OpenXml.Spreadsheet.Column> columns = workSheet.WorksheetPart.Worksheet.Descendants<Columns>().Single().Elements<DocumentFormat.OpenXml.Spreadsheet.Column>().ToList();
                Dictionary<Int32, UInt32Value> columnStyles = new Dictionary<int, UInt32Value>();
                foreach (DocumentFormat.OpenXml.Spreadsheet.Column column in columns)
                {
                    if (column.Style != null && columnIndex > skipColumns)
                    {
                        columnStyles.Add(columnIndex, column.Style);
                    }

                    columnIndex++;
                }

                // loop through each entity and create entity row..
                foreach (var bo in objects)
                {
                    Row entityRow = OpenSpreadsheetUtility.GetRow(sheetData, rowIndex++);
                    PopulateMetadataValues(entityRow, metadataAttributes, bo, creationOptions);
                    PopulateEntityRow(entityRow, bo, models, creationOptions, columnStyles);
                }

                // Removing empty redundant rows
                Int32 rowsNumber = sheetData.Elements<Row>().Count();
                if (rowsNumber > rowIndex)
                {
                    for (Int32 i = rowsNumber - 1; i >= (Int32)rowIndex - 1; i--)
                    {
                        sheetData.Elements<Row>().ElementAt(i).Remove();
                    }
                }

                workSheet.Save();
            }
        }

        private static void PopulateMetadataValues(Row entityRow, IList<string> metadataAttributes, BusinessObject obj, RSExcelCreationOptions creationOptions)
        {
            UInt32 columnIndex = 1;
            foreach (EntityDataTemplateFieldEnum metadataField in Enum.GetValues(typeof(EntityDataTemplateFieldEnum)))
            {
                //first condition is a protection from future modifications of EntityDataTemplateFieldEnum
                if (RSExcelConstants.EntityDataTemplateColumns.ContainsKey(metadataField) &&
                    metadataAttributes.Contains(RSExcelConstants.EntityDataTemplateColumns[metadataField]))
                {
                    OpenSpreadsheetUtility.AppendRowWithTextCell(entityRow, columnIndex++, GetMetadataFromObject(obj, metadataField));
                }
            }
        }

        private static string GetMetadataFromObject(BusinessObject obj, EntityDataTemplateFieldEnum metadataField)
        {
            switch (metadataField)
            {
                case EntityDataTemplateFieldEnum.Id:
                    return obj.Id;
                case EntityDataTemplateFieldEnum.ExtenalId:
                    return obj.Name;
                case EntityDataTemplateFieldEnum.LongName:
                    return obj.Name;
                case EntityDataTemplateFieldEnum.EntityType:
                    return obj.EntityTypeName;
                default:
                    return string.Empty;
            }
        }

        private static void PopulateEntityRow(Row entityRow, BusinessObject obj, IEnumerable<AttributeModel> models, RSExcelCreationOptions creationOptions, Dictionary<int, UInt32Value> columnStyles = null)
        {
            UInt32 columnIndex = (UInt32)entityRow.Elements<DocumentFormat.OpenXml.Spreadsheet.Cell>().Count() + 1;

            uint columnIndex1 = columnIndex;
            Boolean areStylesProvided = columnStyles != null && columnStyles.Any();
            String stringDataType = AttributeDataType.String.ToString();

            foreach (IAttributeModel attributeModel in models)
            {
                String value = GetAttributeValue(obj.GetAttribute(attributeModel.Id, attributeModel.Locale), attributeModel, creationOptions);

                SpaceProcessingModeValues spaceProcessingMode = String.Equals(attributeModel.AttributeDataTypeName, stringDataType, StringComparison.InvariantCulture)
                    ? SpaceProcessingModeValues.Preserve
                    : SpaceProcessingModeValues.Default;

                Boolean isNumberAttribute = attributeModel.AttributeDataTypeName.Equals(AttributeDataType.Decimal.ToString(), StringComparison.InvariantCultureIgnoreCase) ||
                                            attributeModel.AttributeDataTypeName.Equals(AttributeDataType.Integer.ToString(), StringComparison.InvariantCultureIgnoreCase);

                Int32 index = (Int32)columnIndex1 - 1;
                UInt32? columnStyle = null;
                if (areStylesProvided && columnStyles.ContainsKey(index))
                {
                    columnStyle = columnStyles[index];
                }

                if (isNumberAttribute)
                {
                    OpenSpreadsheetUtility.AppendRowWithNumberCell(entityRow, columnIndex1++, value, columnStyle);
                }
                else
                {
                    OpenSpreadsheetUtility.AppendRowWithTextCell(entityRow, columnIndex1++, value, spaceProcessingMode, columnStyle);
                }
            }
        }

        private static String GetAttributeValue(BusinessObjectAttribute attr, IAttributeModel model, RSExcelCreationOptions creationOptions)
        {
            //todo[mv] : locale?
            var locale = System.Threading.Thread.CurrentThread.CurrentUICulture.ToString();

            if (attr != null && attr.HasAnyValue())
            {
                Value[] values = attr.GetCurrentValues();
                if (!model.IsCollection && values.Length > 1)
                {
                    throw new NotSupportedException("Value property cannot be accessible when attribute is collection.");
                }
                return string.Join(RSExcelConstants.CollectionSeparator,
                    values.Select(v => ValueToString((AttributeDataType)model.AttributeDataTypeId, v, creationOptions.UomSeparator, locale)));
            }

            return string.Empty;
        }

        private static string ValueToString(AttributeDataType attributeDataType, Value val, String uomSeparator, String locale)
        {
            String result = GetStringValueForLocale(attributeDataType, val, locale);

            if (!String.IsNullOrWhiteSpace(val.Uom))
            {
                result = String.Concat(result, uomSeparator, val.Uom);
            }
            return result;
        }

        private static String GetStringValueForLocale(AttributeDataType attributeDataType, Value value, String locale)
        {
            switch (attributeDataType)
            {
                case AttributeDataType.Integer:
                case AttributeDataType.Decimal:
                    return (value.NumericVal != null) ? FormatHelper.FormatNumber((double)value.NumericVal, locale) : value.GetStringValue();

                case AttributeDataType.Date:
                    return (value.DateVal != null) ? FormatHelper.FormatDateOnly(Convert.ToString(value.DateVal), locale) : value.GetStringValue();
                case AttributeDataType.DateTime:
                    return (value.DateVal != null) ? FormatHelper.FormatDate((DateTime)value.DateVal, locale) : value.GetStringValue();

                default:
                    return value.GetStringValue();
            }
        }

        #endregion
    }
}
