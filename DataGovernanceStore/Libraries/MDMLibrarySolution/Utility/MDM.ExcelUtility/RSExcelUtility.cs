using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Collections;
using System.Threading;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Practices.ServiceLocation;

namespace MDM.ExcelUtility
{
    using MDM.AdminManager.Business;
    using MDM.Core;
    using MDM.Core.Extensions;
    using MDM.ExcelUtility.RSExcelStyling;
    using MDM.ExcelUtility.RSExcelValidation;
    using MDM.Interfaces;
    using BO = MDM.BusinessObjects;
    using MDM.Utility;

    /// <summary>
    /// Class responsible for Excel document generation 
    /// </summary>
    public sealed class RSExcelUtility
    {
        #region Helper classes

        /// <summary>
        /// Class is used to map style with corresponding style index
        /// </summary>
        private class StyleIndexMapData
        {
            public UInt32 Index { get; set; }
            public AttributesTypeStyle Style { get; set; }
        }

        /// <summary>
        /// Class is used to gather Style data
        /// </summary>
        private class CellStyleData
        {
            public List<Font> Fonts { get; set; }
            public List<Fill> Fills { get; set; }
            public List<CellFormat> CellFormats { get; set; }
        }

        #endregion

        #region Private Constants

        internal const UInt32 ColumnOne = 1;
        internal const UInt32 ColumnTwo = 2;

        private static Mutex thisLock = new Mutex(false);
        // Key -> AttributeId, Value = RowIndex
        private Collection<KeyValuePair<Int32, UInt32>> _attributeWiseRowIndex = new Collection<KeyValuePair<Int32, UInt32>>();
        private Collection<KeyValuePair<Int32, ComplexAttributeExcelInfo>> _complexAttributeWiseExcelInfos = new Collection<KeyValuePair<Int32, ComplexAttributeExcelInfo>>();

        private const String CommonRequiredKey = "CommonRequiredKey";
        private const String CommonOptionalKey = "CommonOptionalKey";
        private const String TechnicalRequiredKey = "TechnicalRequiredKey";
        private const String TechnicalOptionalKey = "TechnicalOptionalKey";
        private const String RelationshipRequiredKey = "RelationshipRequiredKey";
        private const String RelationshipOptionalKey = "RelationshipOptionalKey";

        private const String CommonCollectionRequiredKey = "CommonCollectionRequiredKey";
        private const String CommonCollectionOptionalKey = "CommonCollectionOptionalKey";
        private const String TechnicalCollectionRequiredKey = "TechnicalCollectionRequiredKey";
        private const String TechnicalCollectionOptionalKey = "TechnicalCollectionOptionalKey";
        private const String RelationshipCollectionRequiredKey = "RelationshipCollectionRequiredKey";
        private const String RelationshipCollectionOptionalKey = "RelationshipCollectionOptionalKey";

        private const String SystemAttributeKey = "SystemAttributeKey";
        private const String WorkflowAttributeKey = "WorkflowAttributeKey";

        private static Dictionary<String, StyleIndexMapData> _indexes = new Dictionary<String, StyleIndexMapData>
        {
            {CommonRequiredKey, new StyleIndexMapData()},
            {CommonOptionalKey, new StyleIndexMapData()},
            {TechnicalRequiredKey, new StyleIndexMapData()},
            {TechnicalOptionalKey, new StyleIndexMapData()},
            {RelationshipRequiredKey, new StyleIndexMapData()},
            {RelationshipOptionalKey, new StyleIndexMapData()},

            {CommonCollectionRequiredKey, new StyleIndexMapData()},
            {CommonCollectionOptionalKey, new StyleIndexMapData()},
            {TechnicalCollectionRequiredKey, new StyleIndexMapData()},
            {TechnicalCollectionOptionalKey, new StyleIndexMapData()},
            {RelationshipCollectionRequiredKey, new StyleIndexMapData()},
            {RelationshipCollectionOptionalKey, new StyleIndexMapData()},

            {SystemAttributeKey, new StyleIndexMapData()},
            {WorkflowAttributeKey, new StyleIndexMapData()}
        };

        private static UInt32 _headerCellStyleIndex;
        private static UInt32 _fontCount;
        private static UInt32 _fillCount;
        private static UInt32 _cellFormatCount;

        private const String DefaultTemplateFileName = "Default_RSexcel12_Template";
        private const String DefaultStyleTemplateFileName = "Default_RSexcel11_Style_Template"; // TODO - Once template is get modified on TFS needs to change this constant.
        private const String UseStylesTemplateConfigName = "MDM.Exports.RSExcelFormatter.UseStylesTemplate";
        private const String SaxParserEnableConfigName = "MDMCenter.ExcelExports.SAXParser.Enabled";

        private const String DefaultFileType = "xlsx";

        public class AttrIndexes
        {
            public UInt32 WorkflowAttrIndex { get; set; }
            public UInt32 SystemAttrIndex { get; set; }
            public UInt32 HeaderCellStyleIndex { get; set; }
            public UInt32 CommonOptionalIndex { get; set; }
            public UInt32 CommonRequiredIndex { get; set; }
            public UInt32 TechnicalOptionalIndex { get; set; }
            public UInt32 TechnicalRequiredIndex { get; set; }
            public UInt32 CommonCollectionOptionalIndex { get; set; }
            public UInt32 CommonCollectionRequiredIndex { get; set; }
            public UInt32 TechnicalCollectionOptionalIndex { get; set; }
            public UInt32 TechnicalCollectionRequiredIndex { get; set; }
        }

        private Dictionary<String, Int32> _complexSheetDictionary = new Dictionary<String, Int32>();

        private Dictionary<String, String> _complexAttributes = new Dictionary<String, String>();
        private static Dictionary<String, Column> _attributeColumnMappings = new Dictionary<String, Column>();

        private Boolean _isJobServiceRequest = false;
        private Boolean _showHiddenAttributesOnExport = true;

        #endregion

        public RSExcelUtility()
        {
            
        }

        #region Properties

        public Boolean IsJobServiceRequest
        {
            get
            {
                return _isJobServiceRequest;
            }
            set
            {
                _isJobServiceRequest = value;
            }
        }

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Generates Excel document
        /// </summary>
        /// <param name="entityCollection">The entity collection</param>
        /// <param name="dataTemplate">The data Template.</param>
        /// <param name="fullFileName">The full File Name.</param>
        /// <param name="creationOptions">Creation options</param>
        /// <param name="profileName">The profile Name.</param>
        public void GenerateRSExcel(BO.EntityCollection entityCollection, DataTemplate dataTemplate, String fullFileName, RSExcelCreationOptions creationOptions, String profileName = "")
        {
            thisLock.WaitOne(Timeout.Infinite, false);
            SpreadsheetDocument document = null;
            try
            {
                String templateFileName = GetTemplateFileName(profileName, GetTemporaryFileLocation(), creationOptions);

                CopyFile(templateFileName, fullFileName);

                /**********************************************************************************************************
                 While debugging RSExcel Export in local environment and If you are getting Access Denied for fullFileName 
                 at that time make sure default RSExcelTemplate is not set in ReadOnly mode.
                **********************************************************************************************************/
                document = SpreadsheetDocument.Open(fullFileName, true);

                if (document != null)
                {
                    Boolean isSAXParserEnabled = AppConfigurationHelper.GetAppConfig<Boolean>(SaxParserEnableConfigName, false);
                    _showHiddenAttributesOnExport = AppConfigurationHelper.GetAppConfig<Boolean>("MDMCenter.Exports.ShowHiddenAttributes", true);

                    RsExcelDocumentContentVerification.ReviewAndCleanContent(document);

                    WorkbookPart workbookPart = document.WorkbookPart;

                    if (workbookPart != null)
                    {
                        UpdateSeparatorValues();

                        PopulateEntityInfoSheet(workbookPart, entityCollection, dataTemplate, creationOptions);

                        if (creationOptions.PopulateEntitySheet)
                        {
                            if (isSAXParserEnabled)
                            {
                                PopulateEntitySheetWithSAXParser(workbookPart, entityCollection, dataTemplate, creationOptions);
                            }
                            else
                            {
                                PopulateEntitySheet(workbookPart, entityCollection, dataTemplate, creationOptions);
                            }
                        }

                        if (creationOptions.PopulateRelationshipSheet)
                        {
                            if (isSAXParserEnabled)
                            {
                                PopulateRelationshipSheetSAXParser(workbookPart, entityCollection, dataTemplate, creationOptions);
                            }
                            else
                            {
                                PopulateRelationshipSheet(workbookPart, entityCollection, dataTemplate, creationOptions);
                            }
                        }
                        else if (creationOptions.HideRelationshipsSheet)
                        {
                            Sheet sheet = workbookPart.Workbook.Descendants<Sheet>().FirstOrDefault(s => s.Name == RSExcelConstants.RelationshipSheetName);
                            if (sheet != null)
                            {
                                sheet.State = SheetStateValues.Hidden;
                            }
                        }

                        MetadataSheetDecorator.PopulateMetadataSheet(workbookPart, entityCollection, _complexAttributes, dataTemplate.RequestedLocales);
                    }
                }
            }
            finally
            {
                if (document != null)
                {
                    document.Close();
                    document.Dispose();
                }
                thisLock.ReleaseMutex();
            }
        }
        
        /// <summary>
        /// Gets the coordinates for where on the excel spreadsheet to display the VML comment shape
        /// </summary>
        /// <param name="rowIndex">Row index of where the comment is located (ie. 2)</param>
        /// <returns><![CDATA[<see cref="<x:Anchor>"/>]]> coordinates in the form of a comma separated list</returns>
        public static String GetAnchorCoordinatesForVMLCommentShape(String rowIndex)
        {
            String coordinates = String.Empty;
            Int32 startingRow;
            Int32 startingColumn = 12;

            // From (upper right coordinate of a rectangle)
            // [0] Left column
            // [1] Left column offset
            // [2] Left row
            // [3] Left row offset
            // To (bottom right coordinate of a rectangle)
            // [4] Right column
            // [5] Right column offset
            // [6] Right row
            // [7] Right row offset
            List<Int32> coordList = new List<Int32>(8) { 0, 0, 0, 0, 0, 0, 0, 0 };

            if (Int32.TryParse(rowIndex, out startingRow))
            {
                // Make the row be a zero based index
                startingRow -= 1;

                coordList[0] = startingColumn + 1; // If starting column is A, display shape in column B
                coordList[1] = 15;
                coordList[2] = startingRow;
                coordList[4] = startingColumn + 3; // If starting column is A, display shape till column D
                coordList[5] = 31;
                coordList[6] = startingRow + 2; // If starting row is 0, display 3 rows down to row 3

                // The row offsets change if the shape is defined in the first row
                if (startingRow == 0)
                {
                    coordList[3] = 2;
                    coordList[7] = 17;
                }
                else
                {
                    coordList[3] = 10;
                    coordList[7] = 4;
                }

                coordinates = String.Join(",", coordList.ConvertAll(x => x.ToString()).ToArray());
            }

            return coordinates;
        }

        public static void PopulateAttributeHeaderCells(WorksheetPart workSheetPart, Row headerRow, IEnumerable<BO.AttributeModel> attributeModels,
            RSExcelCreationOptions creationOptions, UInt32 styleIndex, IDictionary<String, String> messages = null, HashSet<Int32> lookupAttributeModelsContextPresence = null)
        {
            // Here, all the fixed fields would be already present into the header row..just create header cells for the attribute names..
            if (headerRow != null && attributeModels != null)
            {
                UInt32 cellIndex = (UInt32)(headerRow.Elements<Cell>().Count() + 1);
                UInt32 index = (UInt32)headerRow.Elements<Cell>().Count();

                XmlDocument vmlDrawing = ReadVMLDrawing(workSheetPart);

                Columns columns = null;

                if (workSheetPart.Worksheet != null && workSheetPart.Worksheet.Descendants<Columns>().SingleOrDefault() != null)
                {
                    columns = workSheetPart.Worksheet.Descendants<Columns>().Single();
                }

                foreach (IAttributeModel attributeModel in attributeModels)
                {
                    // Get attribute column header text based on configured option in RSExcelCreationOptions.AttributeColumnHeaderType
                    String attrHeaderText = GetAttributeColumnHeaderText(attributeModel, creationOptions);
                    LocaleEnum locale = attributeModel.IsLookup || attributeModel.IsLocalizable ? attributeModel.Locale : GlobalizationHelper.GetSystemDataLocale();
                    String attrKey = String.Concat(attributeModel.AttributeParentName, "//", attributeModel.Name + "//" + locale);

                    String definition = attributeModel.Definition;
                    UInt32 columnWidth = (UInt32)(attrHeaderText.Length < 10 ? 16 : 32);

                    Column column = new Column
                    {
                        Min = cellIndex,
                        Max = cellIndex,
                        Width = columnWidth,
                        CustomWidth = true,
                    };

                    if (columns != null)
                    {
                        columns.Append(column);
                    }

                    if (!_attributeColumnMappings.ContainsKey(attrKey))
                    {
                        _attributeColumnMappings.Add(attrKey, column);
                    }

                    // add styles  based on the Attribute type
                    if (attributeModel.AttributeModelType == AttributeModelType.Category)
                    {
                        if (attributeModel.IsCollection)
                        {
                            styleIndex = attributeModel.Required
                                ? UInt32Value.FromUInt32(_indexes[TechnicalCollectionRequiredKey].Index)
                                : UInt32Value.FromUInt32(_indexes[TechnicalCollectionOptionalKey].Index);
                        }
                        else
                        {
                            styleIndex = attributeModel.Required
                                ? UInt32Value.FromUInt32(_indexes[TechnicalRequiredKey].Index)
                                : UInt32Value.FromUInt32(_indexes[TechnicalOptionalKey].Index);
                        }
                    }
                    else if (attributeModel.AttributeModelType == AttributeModelType.Common)
                    {
                        if (attributeModel.IsCollection)
                        {
                            styleIndex = attributeModel.Required
                                ? UInt32Value.FromUInt32(_indexes[CommonCollectionRequiredKey].Index)
                                : UInt32Value.FromUInt32(_indexes[CommonCollectionOptionalKey].Index);
                        }
                        else
                        {
                            styleIndex = attributeModel.Required
                                ? UInt32Value.FromUInt32(_indexes[CommonRequiredKey].Index)
                                : UInt32Value.FromUInt32(_indexes[CommonOptionalKey].Index);
                        }
                    }
                    else if (attributeModel.AttributeModelType == AttributeModelType.Relationship)
                    {
                        if (attributeModel.IsCollection)
                        {
                            styleIndex = attributeModel.Required
                                ? UInt32Value.FromUInt32(_indexes[RelationshipCollectionRequiredKey].Index)
                                : UInt32Value.FromUInt32(_indexes[RelationshipCollectionOptionalKey].Index);
                        }
                        else
                        {
                            styleIndex = attributeModel.Required
                                ? UInt32Value.FromUInt32(_indexes[RelationshipRequiredKey].Index)
                                : UInt32Value.FromUInt32(_indexes[RelationshipOptionalKey].Index);
                        }
                    }

                    Cell headerCell = OpenSpreadsheetUtility.CreateTextCell(headerRow, cellIndex++, attrHeaderText, styleIndex);

                    if (!String.IsNullOrWhiteSpace(definition))
                    {
                        AddComment(workSheetPart, definition, vmlDrawing, headerCell.CellReference.Value, index, headerRow.RowIndex.Value - 1);
                    }
                    else
                    {
                        if (messages != null)
                        {
                            String caption = String.Empty;
                            if (messages[RSExcelConstants.ProvideValueMessageCode] != String.Empty)
                            {
                                caption = String.Format(messages[RSExcelConstants.ProvideValueMessageCode] + Environment.NewLine, attrHeaderText);
                            }

                            if (attributeModel.IsLookup && lookupAttributeModelsContextPresence != null && lookupAttributeModelsContextPresence.Count > 0 &&
                                lookupAttributeModelsContextPresence.Contains(attributeModel.Id) && messages[RSExcelConstants.ContextLookupValueWarningMessageCode] != String.Empty)
                            {
                                caption = String.Format("{0}{1}{2}", caption, messages[RSExcelConstants.ContextLookupValueWarningMessageCode], Environment.NewLine);
                            }

                            AddComment(workSheetPart, caption, vmlDrawing, headerCell.CellReference.Value, index, headerRow.RowIndex.Value - 1);
                        }
                    }

                    index++;
                }

                WriteVMLDrawing(workSheetPart, vmlDrawing);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stylesheet"></param>
        /// <param name="worksheetStyles"></param>
        public static void ApplyStyleSheet(Stylesheet stylesheet, WorksheetStyles worksheetStyles)
        {
            CountStyles(stylesheet);

            MapStylesToAttributes(worksheetStyles);

            AppendStyles(stylesheet, _fontCount, _fillCount, _cellFormatCount);
        }

        #endregion

        #region Private Methods

        #region EntitySheet Methods

        private void PopulateEntitySheet(WorkbookPart workbookPart, BO.EntityCollection entityCollection, DataTemplate dataTemplate, RSExcelCreationOptions creationOptions)
        {
            _complexAttributes = new Dictionary<String, String>();
            Worksheet workSheet = OpenSpreadsheetUtility.GetWorksheet(workbookPart, RSExcelConstants.EntityDataSheetName);

            if (workSheet != null)
            {
                SheetData sheetData = workSheet.GetFirstChild<SheetData>();

                // Get style index for header row
                Cell entitySheetHeaderCell = OpenSpreadsheetUtility.GetDataCell(sheetData, ColumnOne, 1);
                _headerCellStyleIndex = OpenSpreadsheetUtility.GetCellStyleIndex(entitySheetHeaderCell);

                UInt32 rowIndex = 1; // Header row is always at first row..means index = 0;

                Row headerRow = OpenSpreadsheetUtility.GetRow(sheetData, rowIndex);
                List<Int32> sharedStringIds = headerRow.Elements<Cell>().Select(cell => Int32.Parse(cell.InnerText)).ToList();
                List<String> metadataAttributes = sharedStringIds.Select(i => OpenSpreadsheetUtility.GetSharedStringItemById(workbookPart, i).InnerText).ToList();

                // Get the style index from the first column
                Cell headerFirstCell = OpenSpreadsheetUtility.GetDataCell(sheetData, ColumnOne, rowIndex);
                rowIndex++;

                Stylesheet stylesheet = workbookPart.WorkbookStylesPart.Stylesheet;

                ApplyStyleSheet(stylesheet, headerRow);

                // Localized Validation messages from DB
                IDictionary<String, String> messages = new ValidationMessagesProvider().GetAllValidationMessagesDictionary();
                List<BO.AttributeModel> nonComplexAttributeModels = null;

                // Populate attribute header cells only for non complex attributes
                if (!_showHiddenAttributesOnExport)
                {
                    nonComplexAttributeModels = dataTemplate.EntityAttributeModels.Where(model => model.IsComplex == false && model.AttributeModelType != AttributeModelType.Relationship && !model.IsHidden).ToList();
                }
                else
                {
                    nonComplexAttributeModels = dataTemplate.EntityAttributeModels.Where(model => model.IsComplex == false && model.AttributeModelType != AttributeModelType.Relationship).ToList();
                }

                if (nonComplexAttributeModels.Count > 0)
                {
                    PopulateAttributeHeaderCells(workSheet.WorksheetPart, headerRow, nonComplexAttributeModels, creationOptions, OpenSpreadsheetUtility.GetCellStyleIndex(headerFirstCell), messages, dataTemplate.LookupAttributeModelsContextPresence);
                }

                IList<IAttributeModel> complexAttributeModels = dataTemplate.EntityAttributeModels.Where(model => model.IsComplex).ToList<IAttributeModel>();
                if (complexAttributeModels.Count > 0)
                {
                    PopulateComplexAttributeHeaderCells(workbookPart, MDMObjectFactory.GetIAttributeModelCollection(complexAttributeModels), creationOptions, OpenSpreadsheetUtility.GetCellStyleIndex(headerFirstCell), dataTemplate);
                }

                // Populate sheet with validation restrictions
                Int32 skipColumns = metadataAttributes.Count;
                Boolean includeValidations = AppConfigurationHelper.GetAppConfig<Boolean>("MDM.Exports.RSExcelFormatter.IncludeValidations");
                if (includeValidations)
                {
                    ValidationProvider.Provider.AppendWorksheetWithValidation(workbookPart, workSheet, nonComplexAttributeModels, messages, dataTemplate.ExcelValidationInfo, skipColumns);
                    AddNumberFormatingsToSheet(workSheet, stylesheet, nonComplexAttributeModels, skipColumns);

                    if (dataTemplate.ExcelValidationInfo.Count > 0)
                    {
                        List<IAttributeModel> validationAttrModels = new List<IAttributeModel>();
                        validationAttrModels.AddRange(nonComplexAttributeModels);
                        List<IAttributeModel> filteredComplexAttributeModels = new List<IAttributeModel>();

                        foreach (IAttributeModel cplxModel in dataTemplate.EntityAttributeModels)
                        {
                            if (cplxModel.IsComplex && !IsAttributeInCollection(filteredComplexAttributeModels, cplxModel))
                            {
                                filteredComplexAttributeModels.Add(cplxModel);
                            }
                        }

                        validationAttrModels.AddRange(filteredComplexAttributeModels);
                        ValidationProvider.Provider.PopulateValidationLookupsHiddenSheet(workbookPart, workbookPart.Workbook, validationAttrModels, dataTemplate.ExcelValidationInfo);
                    }
                }

                // Creating collection of column numbers / styles
                Int32 columnIndex = 1;
                List<Column> columns = workSheet.WorksheetPart.Worksheet.Descendants<Columns>().Single().Elements<Column>().ToList();
                Dictionary<Int32, UInt32Value> columnStyles = new Dictionary<Int32, UInt32Value>();
                foreach (Column column in columns)
                {
                    if (column.Style != null && columnIndex > skipColumns)
                    {
                        columnStyles.Add(columnIndex, column.Style);
                    }

                    columnIndex++;
                }

                // loop through each entity and create entity row..
                foreach (IEntity entity in entityCollection)
                {
                    Row entityRow = OpenSpreadsheetUtility.GetRow(sheetData, rowIndex++);
                    PopulateMetadataValues(entityRow, metadataAttributes, entity, creationOptions);
                    PopulateEntityRow(entityRow, entity, dataTemplate, creationOptions, workbookPart, includeValidations, columnStyles);
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
        
        private void PopulateEntitySheetCommon(WorkbookPart workbookPart, DataTemplate dataTemplate, RSExcelCreationOptions creationOptions, ref UInt32 rowIndex, ref List<String> metadataAttributes, ref Dictionary<Int32, UInt32Value> columnStyles, BO.EntityCollection entityCollection, Boolean isValidationsRequired, Boolean isCommentsRequired)
        {
            Worksheet workSheet = OpenSpreadsheetUtility.GetWorksheet(workbookPart, RSExcelConstants.EntityDataSheetName);

            if (workSheet != null)
            {
                SheetData sheetData = workSheet.GetFirstChild<SheetData>();

                // Get style index for header row
                Cell entitySheetHeaderCell = OpenSpreadsheetUtility.GetDataCell(sheetData, ColumnOne, 1);
                _headerCellStyleIndex = OpenSpreadsheetUtility.GetCellStyleIndex(entitySheetHeaderCell);
                rowIndex = 1; // Header row is always at first row..means index = 0;

                Row headerRow = OpenSpreadsheetUtility.GetRow(sheetData, rowIndex);
                List<Int32> sharedStringIds = headerRow.Elements<Cell>().Select(cell => Int32.Parse(cell.InnerText)).ToList();
                metadataAttributes = sharedStringIds.Select(i => OpenSpreadsheetUtility.GetSharedStringItemById(workbookPart, i).InnerText).ToList();

                // Get the style index from the first column
                Cell headerFirstCell = OpenSpreadsheetUtility.GetDataCell(sheetData, ColumnOne, rowIndex);
                rowIndex++;

                Stylesheet stylesheet = workbookPart.WorkbookStylesPart.Stylesheet;

                // Applying styles from styles template in case it is found
                Boolean useStylesTemplate = AppConfigurationHelper.GetAppConfig<Boolean>("MDM.Exports.RSExcelFormatter.UseStylesTemplate", false);

                if (useStylesTemplate)
                {
                    BO.Template template = new ExportTemplateBL().GetExportTemplateByName("Default_RSexcel11_Style_Template", new BO.CallerContext(MDMCenterApplication.PIM, MDMCenterModules.Export));

                    if (template != null)
                    {
                        String stylesTemplateFileName = GetTemplateFileName("Default_RSexcel11_Style_Template", GetTemporaryFileLocation());
                        WorksheetStyles worksheetStyles = StylesTemplateProcessor.ProcessStylesTemplateFile(stylesTemplateFileName);

                        UpdateMetadataAttributesStyle(headerRow, stylesheet, worksheetStyles);

                        ApplyStyleSheet(stylesheet, worksheetStyles);
                    }
                    else
                    {
                        ApplyStyleSheet(stylesheet, headerRow);
                    }
                }
                else
                {
                    ApplyStyleSheet(stylesheet, headerRow);
                }

                // Localized Validation messages from DB
                IDictionary<String, String> messages = new ValidationMessagesProvider().GetAllValidationMessagesDictionary();

                // Populate attribute header cells only for non complex attributes
                List<BO.AttributeModel> nonComplexAttributeModels = dataTemplate.EntityAttributeModels.Where(model => model.IsComplex == false).ToList();
                if (nonComplexAttributeModels.Count > 0)
                {
                    PopulateAttributeHeaderCells(workSheet.WorksheetPart, headerRow, nonComplexAttributeModels, creationOptions, OpenSpreadsheetUtility.GetCellStyleIndex(headerFirstCell), messages);
                }

                IList<IAttributeModel> complexAttributeModels = dataTemplate.EntityAttributeModels.Where(model => model.IsComplex).ToList<IAttributeModel>();
                if (complexAttributeModels.Count > 0)
                {
                    PopulateComplexAttributeHeaderCells(workbookPart, MDMObjectFactory.GetIAttributeModelCollection(complexAttributeModels), creationOptions, OpenSpreadsheetUtility.GetCellStyleIndex(headerFirstCell), dataTemplate);
                }

                // Populate sheet with validation restrictions
                Int32 skipColumns = metadataAttributes.Count;
                if (isValidationsRequired)
                {
                    Boolean includeValidations = AppConfigurationHelper.GetAppConfig<Boolean>("MDM.Exports.RSExcelFormatter.IncludeValidations", false);
                    if (includeValidations)
                    {
                        ValidationProvider.Provider.AppendWorksheetWithValidation(workbookPart, workSheet, nonComplexAttributeModels, messages, dataTemplate.ExcelValidationInfo, skipColumns);
                        AddNumberFormatingsToSheet(workSheet, stylesheet, nonComplexAttributeModels, skipColumns);
                    }
                }

                // Creating collection of column numbers / styles
                Int32 columnIndex = 1;
                List<Column> columns = workSheet.WorksheetPart.Worksheet.Descendants<Columns>().Single().Elements<Column>().ToList();
                columnStyles = new Dictionary<int, UInt32Value>();
                foreach (Column column in columns)
                {
                    if (column.Style != null && columnIndex > skipColumns)
                    {
                        columnStyles.Add(columnIndex, column.Style);
                    }

                    columnIndex++;
                }
            }
        }

        private void PopulateEntityInfoSheet(WorkbookPart workbookPart, BO.EntityCollection entityCollection, DataTemplate dataTemplate, RSExcelCreationOptions creationOptions)
        {
            Worksheet workSheet = OpenSpreadsheetUtility.GetWorksheet(workbookPart, RSExcelConstants.EntityInfoSheetName);

            UInt32Value rowIndex = 1;

            if (workSheet != null)
            {
                List<String> headerList = GetHeaderList(workbookPart, workSheet);

                WorksheetPart replacementPart = workbookPart.AddNewPart<WorksheetPart>();
                String replacementId = workbookPart.GetIdOfPart(replacementPart);

                OpenXmlReader reader = OpenXmlReader.Create(workSheet.WorksheetPart);
                OpenXmlWriter writer = OpenXmlWriter.Create(replacementPart);

                while (reader.Read())
                {
                    if (reader.ElementType == typeof(SheetData))
                    {
                        if (reader.IsEndElement)
                        {
                            continue;
                        }
                        SheetData baseSheetData = workSheet.WorksheetPart.Worksheet.Elements<SheetData>().First();

                        writer.WriteStartElement(new SheetData());

                        foreach (Row r in baseSheetData.Elements<Row>())
                        {
                            WriteRow(writer, r);
                        }

                        PopulateEntityInfo(entityCollection, writer, headerList, rowIndex, creationOptions);

                        writer.WriteEndElement();
                    }
                    else if (reader.ElementType == typeof(Drawing) || reader.ElementType == typeof(LegacyDrawing))
                    {
                        //Skipping Drawing and LegacyDrawing as they contain comments which are corrupting the workSheet
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
                reader.Close();
                writer.Close();

                workSheet.Save();
                Sheet entityInfoSheet = workbookPart.Workbook.Descendants<Sheet>().FirstOrDefault(s => s.Name == "Entity Info");
                entityInfoSheet.Id = replacementId;
                workbookPart.DeletePart(workSheet.WorksheetPart);
                workbookPart.Workbook.Save();
            }
        }

        private List<String> GetHeaderList(WorkbookPart workbookPart, Worksheet workSheet)
        {
            UInt32Value rowIndex = 1;

            SheetData sheetData = workSheet.GetFirstChild<SheetData>();
            Row headerRow = OpenSpreadsheetUtility.GetRow(sheetData, rowIndex);

            List<Int32> sharedStringIds = headerRow.Elements<Cell>().Select(cell => Int32.Parse(cell.InnerText)).ToList();
            List<String> headerlist = sharedStringIds.Select(i => OpenSpreadsheetUtility.GetSharedStringItemById(workbookPart, i).InnerText).ToList();
            return headerlist;
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

        private void PopulateEntityInfo(BO.EntityCollection entities, OpenXmlWriter writer, List<String> headerList, UInt32Value rowIndex, RSExcelCreationOptions creationOptions)
        {
            if (entities.Count > 0)
            {
                BO.EntityStateValidationCollection entityStateValidations = null;

                PopulateEntityInfoRow(writer, headerList, rowIndex, null, new EntityInfo() { InfoType = "Promote Info", Name = "Is Promoted Copy", Value = creationOptions.IsApprovedCopy.ToString() });
                ++rowIndex;

                PopulateEntityInfoRow(writer, headerList, rowIndex, null, new EntityInfo() { InfoType = "Export Info", Name = "Export Profile Name", Value = creationOptions.ProfileName });
                ++rowIndex;

                PopulateEntityInfoRow(writer, headerList, rowIndex, null, new EntityInfo() { InfoType = "Export Info", Name = "Export Timestamp", Value = DateTime.Now.ToString() });
                ++rowIndex;


                if (creationOptions.PopulateEntityStateInfo)
                {
                    Collection<Int64> entityIds = entities.GetEntityIdList();
                    BO.CallerContext callerContext = new BO.CallerContext() { Application = MDMCenterApplication.MDMCenter, Module = MDMCenterModules.Export };

                    var entityStateValidationManager = ServiceLocator.Current.GetInstance(typeof(IEntityStateValidationManager)) as IEntityStateValidationManager;

                    entityStateValidations = entityStateValidationManager.Get(entityIds, callerContext);
                }

                foreach (BO.Entity entity in entities)
                {
                    if (entityStateValidations != null)
                    {
                        BO.EntityStateValidationCollection stateValidations = entityStateValidations.Get(SV => SV.EntityId == entity.Id);
                        BO.ValidationStates validationStates = entity.ValidationStates;

                        EntityInfo entityInfoForSelfValidation = new EntityInfo() { InfoType = "State Info", Name = "IsSelfValid", Value = validationStates.IsSelfValid.ToString() };
                        PopulateEntityInfoRow(writer, headerList, rowIndex, entity, entityInfoForSelfValidation);
                        ++rowIndex;

                        EntityInfo entityInfoForMetadataValidation = new EntityInfo() { InfoType = "State Info", Name = "IsMetaDataValid", Value = validationStates.IsMetaDataValid.ToString() };
                        entityInfoForMetadataValidation.MessageCodes = GetEntityStateValidationMessageCodes(stateValidations, SystemAttributes.EntityMetaDataValid);

                        PopulateEntityInfoRow(writer, headerList, rowIndex, entity, entityInfoForMetadataValidation);
                        ++rowIndex;

                        EntityInfo entityInfoForCommonAttributesValidation = new EntityInfo() { InfoType = "State Info", Name = "IsCommonAttributesValid", Value = validationStates.IsCommonAttributesValid.ToString() };
                        entityInfoForCommonAttributesValidation.MessageCodes = GetEntityStateValidationMessageCodes(stateValidations, SystemAttributes.EntityCommonAttributesValid);

                        PopulateEntityInfoRow(writer, headerList, rowIndex, entity, entityInfoForCommonAttributesValidation);
                        ++rowIndex;

                        EntityInfo entityInfoForCategoryAttributesValidation = new EntityInfo() { InfoType = "State Info", Name = "IsCategoryAttributesValid", Value = validationStates.IsCategoryAttributesValid.ToString() };
                        entityInfoForCategoryAttributesValidation.MessageCodes = GetEntityStateValidationMessageCodes(stateValidations, SystemAttributes.EntityCategoryAttributesValid);

                        PopulateEntityInfoRow(writer, headerList, rowIndex, entity, entityInfoForCategoryAttributesValidation);
                        ++rowIndex;

                        EntityInfo entityInfoForRelationshipsValidation = new EntityInfo() { InfoType = "State Info", Name = "IsRelationshipsValid", Value = validationStates.IsRelationshipsValid.ToString() };
                        entityInfoForRelationshipsValidation.MessageCodes = GetEntityStateValidationMessageCodes(stateValidations, SystemAttributes.EntityRelationshipsValid);

                        PopulateEntityInfoRow(writer, headerList, rowIndex, entity, entityInfoForRelationshipsValidation);
                        ++rowIndex;

                        EntityInfo entityInfoForEntityVariantValidation = new EntityInfo() { InfoType = "State Info", Name = "IsEntityVariantValid", Value = validationStates.IsEntityVariantValid.ToString() };
                        entityInfoForEntityVariantValidation.MessageCodes = GetEntityStateValidationMessageCodes(stateValidations, SystemAttributes.EntityVariantValid);

                        PopulateEntityInfoRow(writer, headerList, rowIndex, entity, entityInfoForEntityVariantValidation);
                        ++rowIndex;

                        EntityInfo entityInfoForExtensionsValidation = new EntityInfo() { InfoType = "State Info", Name = "IsExtensionsValid", Value = validationStates.IsExtensionsValid.ToString() };
                        entityInfoForExtensionsValidation.MessageCodes = GetEntityStateValidationMessageCodes(stateValidations, SystemAttributes.EntityExtensionsValid);

                        PopulateEntityInfoRow(writer, headerList, rowIndex, entity, entityInfoForExtensionsValidation);
                        ++rowIndex;
                    }

                    if (creationOptions.PopulateBusinessConditionInfo)
                    {
                        foreach (BO.BusinessConditionStatus businessConditionStatus in entity.BusinessConditions)
                        {
                            EntityInfo entityInfoForBizCondition = new EntityInfo() { InfoType = "Business Condition", Name = businessConditionStatus.Name, Value = businessConditionStatus.Status.ToString() };
                            PopulateEntityInfoRow(writer, headerList, rowIndex, entity, entityInfoForBizCondition);
                            ++rowIndex;
                        }
                    }
                }
            }
        }

        private static Collection<String> GetEntityStateValidationMessageCodes(BO.EntityStateValidationCollection stateValidations, SystemAttributes systemAttributes)
        {
            Collection<String> messageCodes = new Collection<String>();

            if (stateValidations != null && stateValidations.Count > 0)
            {
                BO.EntityStateValidationCollection systemAttributeStateValidation = stateValidations.Get(MV => MV.SystemValidationStateAttribute == systemAttributes);

                if (systemAttributeStateValidation != null && systemAttributeStateValidation.Count > 0)
                {
                    foreach (BO.EntityStateValidation entityValidationState in systemAttributeStateValidation)
                    {
                        messageCodes.Add(entityValidationState.MessageCode);
                    }
                }
            }

            return messageCodes;
        }

        private void PopulateEntityInfoRow(OpenXmlWriter writer, List<String> headerList, UInt32Value rowIndex, BO.Entity entity, EntityInfo entityInfo)
        {
            Row entityInfoRow = new Row();
            entityInfoRow.RowIndex = ++rowIndex;

            UInt32 columnIndex = 1;

            if (entity != null)
            {
                if (headerList.Contains(RSExcelConstants.EntityStateInfoTemplateColumns[EntityInfoTemplateFieldEnum.Id]))
                {
                    OpenSpreadsheetUtility.AppendRowWithNumberCell(entityInfoRow, columnIndex++, entity.Id);
                }
                if (headerList.Contains(RSExcelConstants.EntityStateInfoTemplateColumns[EntityInfoTemplateFieldEnum.ExtenalId]))
                {
                    OpenSpreadsheetUtility.AppendRowWithTextCell(entityInfoRow, columnIndex++, entity.ExternalId);
                }
                if (headerList.Contains(RSExcelConstants.EntityStateInfoTemplateColumns[EntityInfoTemplateFieldEnum.EntityType]))
                {
                    OpenSpreadsheetUtility.AppendRowWithTextCell(entityInfoRow, columnIndex++, entity.EntityTypeName);
                }
                if (headerList.Contains(RSExcelConstants.EntityStateInfoTemplateColumns[EntityInfoTemplateFieldEnum.CategoryPath]))
                {
                    OpenSpreadsheetUtility.AppendRowWithTextCell(entityInfoRow, columnIndex++, entity.CategoryPath);
                }
                if (headerList.Contains(RSExcelConstants.EntityStateInfoTemplateColumns[EntityInfoTemplateFieldEnum.Container]))
                {
                    OpenSpreadsheetUtility.AppendRowWithTextCell(entityInfoRow, columnIndex++, entity.ContainerName);
                }
            }
            else
            {
                columnIndex = 6;
            }

            if (headerList.Contains(RSExcelConstants.EntityStateInfoTemplateColumns[EntityInfoTemplateFieldEnum.InfoType]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(entityInfoRow, columnIndex++, entityInfo.InfoType);
            }
            if (headerList.Contains(RSExcelConstants.EntityStateInfoTemplateColumns[EntityInfoTemplateFieldEnum.Name]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(entityInfoRow, columnIndex++, entityInfo.Name);
            }
            if (headerList.Contains(RSExcelConstants.EntityStateInfoTemplateColumns[EntityInfoTemplateFieldEnum.Value]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(entityInfoRow, columnIndex++, entityInfo.Value);
            }
            if (headerList.Contains(RSExcelConstants.EntityStateInfoTemplateColumns[EntityInfoTemplateFieldEnum.MessageCodes]))
            {
                String messageCodes = ValueTypeHelper.JoinCollection(entityInfo.MessageCodes, Constants.STRING_PATH_SEPARATOR);
                OpenSpreadsheetUtility.AppendRowWithTextCell(entityInfoRow, columnIndex++, messageCodes);
            }

            WriteRow(writer, entityInfoRow);
        }

        private void PopulateEntitySheetWithSAXParser(WorkbookPart workbookPart, BO.EntityCollection entityCollection, DataTemplate dataTemplate, RSExcelCreationOptions creationOptions)
        {
            Worksheet workSheet = OpenSpreadsheetUtility.GetWorksheet(workbookPart, RSExcelConstants.EntityDataSheetName);
            //IDictionary<String, Column> hierarchialAttributeColumnMappings = new Dictionary<String, Column>();
            IDictionary<Int32, IDictionary<String, Column>> hierarchialAttributeColumnMappings = new Dictionary<Int32, IDictionary<String, Column>>();

            if (workSheet != null)
            {
                SheetData sheetData = workSheet.GetFirstChild<SheetData>();

                // Get style index for header row
                Cell entitySheetHeaderCell = OpenSpreadsheetUtility.GetDataCell(sheetData, ColumnOne, 1);
                _headerCellStyleIndex = OpenSpreadsheetUtility.GetCellStyleIndex(entitySheetHeaderCell);

                UInt32 rowIndex = 1; // Header row is always at first row..means index = 0;

                Row headerRow = OpenSpreadsheetUtility.GetRow(sheetData, rowIndex);
                List<Int32> sharedStringIds = headerRow.Elements<Cell>().Select(cell => Int32.Parse(cell.InnerText)).ToList();
                List<String> metadataAttributes = sharedStringIds.Select(i => OpenSpreadsheetUtility.GetSharedStringItemById(workbookPart, i).InnerText).ToList();

                // Get the style index from the first column
                Cell headerFirstCell = OpenSpreadsheetUtility.GetDataCell(sheetData, ColumnOne, rowIndex);
                rowIndex++;

                Stylesheet stylesheet = workbookPart.WorkbookStylesPart.Stylesheet;

                ApplyStyleSheet(stylesheet, headerRow);

                // Localized Validation messages from DB
                IDictionary<String, String> messages = new ValidationMessagesProvider().GetAllValidationMessagesDictionary();
                List<BO.AttributeModel> nonComplexAttributeModels = null;

                // Populate attribute header cells only for non complex attributes
                if (!_showHiddenAttributesOnExport)
                {
                    nonComplexAttributeModels = dataTemplate.EntityAttributeModels.Where(model => model.IsComplex == false && model.AttributeModelType != AttributeModelType.Relationship && !model.IsHidden).ToList();
                }
                else
                {
                    nonComplexAttributeModels = dataTemplate.EntityAttributeModels.Where(model => model.IsComplex == false && model.AttributeModelType != AttributeModelType.Relationship).ToList();
                }

                List<IAttributeModel> filteredComplexAttributeModelsById = new List<IAttributeModel>();
                List<IAttributeModel> filteredComplexAttributeModels = new List<IAttributeModel>();

                #region Remove duplicate Complex Attribute Models

                foreach (IAttributeModel cplxModel in dataTemplate.EntityAttributeModels)
                {
                    if (cplxModel.IsComplex)
                    {
                        if (!IsAttributeInCollection(filteredComplexAttributeModelsById, cplxModel))
                        {
                            filteredComplexAttributeModelsById.Add(cplxModel);
                        }
                        filteredComplexAttributeModels.Add(cplxModel);
                    }
                }

                #endregion

                if (filteredComplexAttributeModelsById.Count > 0)
                {
                    PopulateComplexAttributeHeaderCells(workbookPart, MDMObjectFactory.GetIAttributeModelCollection(filteredComplexAttributeModelsById), creationOptions, OpenSpreadsheetUtility.GetCellStyleIndex(headerFirstCell), dataTemplate, hierarchialAttributeColumnMappings);
                }

                // Populate sheet with validation restrictions
                Int32 skipColumns = metadataAttributes.Count;
                Boolean includeValidations = AppConfigurationHelper.GetAppConfig<Boolean>("MDM.Exports.RSExcelFormatter.IncludeValidations");
                if (includeValidations)
                {
                    ValidationProvider.Provider.AppendWorksheetWithValidation(workbookPart, workSheet, nonComplexAttributeModels, messages, dataTemplate.ExcelValidationInfo, skipColumns);
                    AddNumberFormatingsToSheet(workSheet, stylesheet, nonComplexAttributeModels, skipColumns);

                    if (dataTemplate.ExcelValidationInfo.Count > 0)
                    {
                        List<IAttributeModel> validationAttrModels = new List<IAttributeModel>();
                        validationAttrModels.AddRange(nonComplexAttributeModels);
                        validationAttrModels.AddRange(filteredComplexAttributeModels);
                        ValidationProvider.Provider.PopulateValidationLookupsHiddenSheet(workbookPart, workbookPart.Workbook, validationAttrModels, dataTemplate.ExcelValidationInfo);
                    }
                }

                // Creating collection of column numbers / styles
                Int32 columnIndex = 1;
                List<Column> columns = workSheet.WorksheetPart.Worksheet.Descendants<Columns>().Single().Elements<Column>().ToList();
                Dictionary<Int32, UInt32Value> columnStyles = new Dictionary<Int32, UInt32Value>();
                foreach (Column column in columns)
                {
                    if (column.Style != null && columnIndex > skipColumns)
                    {
                        columnStyles.Add(columnIndex, column.Style);
                    }

                    columnIndex++;
                }

                WorksheetPart replacementPart = workbookPart.AddNewPart<WorksheetPart>();
                String replacementId = workbookPart.GetIdOfPart(replacementPart);

                OpenXmlReader reader = OpenXmlReader.Create(workSheet.WorksheetPart);
                OpenXmlWriter writer = OpenXmlWriter.Create(replacementPart);

                while (reader.Read())
                {
                    if (reader.ElementType == typeof(SheetData))
                    {
                        if (reader.IsEndElement)
                        {
                            continue;
                        }
                        SheetData baseSheetData = workSheet.WorksheetPart.Worksheet.Elements<SheetData>().First();

                        writer.WriteStartElement(new SheetData());

                        foreach (Row r in baseSheetData.Elements<Row>())
                        {
                            writer.WriteStartElement(r);
                            foreach (Cell c in r.Elements<Cell>())
                            {
                                writer.WriteElement(c);
                            }
                            writer.WriteEndElement();
                        }

                        InitializeReportComplexAttributeSheets(filteredComplexAttributeModels, creationOptions, workbookPart);

                        foreach (IEntity entity in entityCollection)
                        {
                            Row entityRow = new Row();
                            entityRow.RowIndex = rowIndex++;
                            PopulateMetadataValues(entityRow, metadataAttributes, entity, creationOptions);

                            List<Cell> cells = PopulateEntityRow(entityRow, entity, dataTemplate, filteredComplexAttributeModels, creationOptions, workbookPart, columnStyles, workSheet, reader, writer, hierarchialAttributeColumnMappings);
                            writer.WriteStartElement(entityRow);
                            foreach (Cell cell in entityRow.Elements<Cell>())
                            {
                                writer.WriteElement(cell);
                            }

                            if (cells != null && cells.Count > 0)
                            {

                                foreach (Cell cell in cells)
                                {
                                    writer.WriteElement(cell);
                                }

                            }
                            writer.WriteEndElement();
                        }

                        CloseReportComplexAttributeSheets(filteredComplexAttributeModelsById, creationOptions, workbookPart);
                        // SheetData
                        writer.WriteEndElement();
                    }
                    else if (reader.ElementType == typeof(DataValidations))
                    {
                        if (reader.IsEndElement)
                            continue;

                        writer.WriteStartElement(new DataValidations());
                        reader.Read();

                        while (reader.IsStartElement && reader.ElementType == typeof(DataValidation))
                        {
                            writer.WriteStartElement(reader);
                            reader.Read();

                            while (reader.ElementType == typeof(Formula1) || reader.ElementType == typeof(Formula2))
                            {
                                if (reader.IsStartElement)
                                {
                                    writer.WriteStartElement(reader);
                                    writer.WriteString(reader.GetText());
                                    writer.WriteEndElement();
                                }
                                reader.Read();
                            }

                            writer.WriteEndElement();
                            reader.Read();
                        }

                        writer.WriteEndElement();
                    }
                    else if (reader.ElementType == typeof(Drawing) || reader.ElementType == typeof(LegacyDrawing))
                    {
                        //Skipping Drawing and LegacyDrawing as they contain comments which are corrupting the workSheet
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
                reader.Close();
                writer.Close();

                rowIndex = 1;
                if (nonComplexAttributeModels.Count > 0)
                {
                    headerRow = OpenSpreadsheetUtility.GetRow(replacementPart.Worksheet.GetFirstChild<SheetData>(), rowIndex);
                    headerFirstCell = OpenSpreadsheetUtility.GetDataCell(replacementPart.Worksheet.GetFirstChild<SheetData>(), 1, rowIndex);
                    PopulateAttributeHeaderCells(replacementPart, headerRow, nonComplexAttributeModels, creationOptions, OpenSpreadsheetUtility.GetCellStyleIndex(headerFirstCell), messages, dataTemplate.LookupAttributeModelsContextPresence);
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

                Sheet entitiesSheet = workbookPart.Workbook.Descendants<Sheet>().FirstOrDefault(s => s.Name == "Entities");
                if (entitiesSheet != null)
                {
                    entitiesSheet.Id = replacementId;
                }
                workbookPart.DeletePart(workSheet.WorksheetPart);
                workbookPart.Workbook.Save();
            }
        }

        private List<Cell> PopulateEntityRow(Row entityRow, IEntity entity, DataTemplate dataTemplate, List<IAttributeModel> filteredComplexAttributeModels, RSExcelCreationOptions creationOptions, WorkbookPart workbookPart, Dictionary<Int32, UInt32Value> columnStyles = null, Worksheet workSheet = null, OpenXmlReader reader = null, OpenXmlWriter writer = null, IDictionary<Int32, IDictionary<String, Column>> hierarchialAttributeColumnMappings = null)
        {
          UInt32 columnIndex = (UInt32)entityRow.Elements<Cell>().Count() + 1;
            IAttributeCollection attributes = entity.GetAttributes();

          // First write non complex attributes
            IList<IAttribute> nonComplexAttributes = attributes != null ? attributes.Where(attr => !attr.IsComplex).ToList<IAttribute>() : null;
            IList<IAttributeModel> nonComplexAttributeModels = null;

            if (dataTemplate.EntityAttributeModels != null && dataTemplate.EntityAttributeModels.Count > 0)
            {
            if (!_showHiddenAttributesOnExport)
            {
                nonComplexAttributeModels = dataTemplate.EntityAttributeModels.Where(model => model.IsComplex == false && model.AttributeModelType != AttributeModelType.Relationship && !model.IsHidden).ToList<IAttributeModel>();
            }
            else
            {
                nonComplexAttributeModels = dataTemplate.EntityAttributeModels.Where(model => model.IsComplex == false && model.AttributeModelType != AttributeModelType.Relationship).ToList<IAttributeModel>();
            }
            }

            List<Cell> cells = PopulateSAXAttributeCells(entityRow, ref columnIndex, MDMObjectFactory.GetIAttributeCollection(nonComplexAttributes), MDMObjectFactory.GetIAttributeModelCollection(nonComplexAttributeModels), creationOptions, columnStyles);

            IList<IAttribute> complexAttributes = attributes != null ? attributes.Where(attr => attr.IsComplex && !attr.IsHierarchical).ToList<IAttribute>() : null;

          PopulateComplexAttributeCellsSAXParser(entity, dataTemplate, MDMObjectFactory.GetIAttributeCollection(complexAttributes), MDMObjectFactory.GetIAttributeModelCollection(filteredComplexAttributeModels), creationOptions, workbookPart);

            IList<IAttribute> hierarchialAttributes = attributes != null ? attributes.Where(attr => attr.IsComplex && attr.IsHierarchical).ToList<IAttribute>() : null;

            if (hierarchialAttributes != null && hierarchialAttributes.Count > 0)
          {
            PopulateHierarchialAttributeCellsSAXParser(entity, dataTemplate, MDMObjectFactory.GetIAttributeCollection(hierarchialAttributes), MDMObjectFactory.GetIAttributeModelCollection(filteredComplexAttributeModels.Where(am => ((BO.AttributeModel)am).IsHierarchical).ToList()), creationOptions, hierarchialAttributeColumnMappings);
          }

          return cells;
        }
        
        private void PopulateEntityRow(Row entityRow, IEntity entity, DataTemplate dataTemplate, RSExcelCreationOptions creationOptions, WorkbookPart workbookPart, Boolean includeValidations, Dictionary<Int32, UInt32Value> columnStyles = null)
        {
            UInt32 columnIndex = (UInt32)entityRow.Elements<Cell>().Count() + 1;
            IAttributeCollection attributes = entity.GetAttributes();
            IAttributeModelCollection attributeModels = dataTemplate.EntityAttributeModels;

            // First write non complex attributes
            IList<IAttribute> nonComplexAttributes = attributes != null ? attributes.Where(attr => !attr.IsComplex).ToList<IAttribute>() : null;
            IList<IAttributeModel> nonComplexAttributeModels = attributeModels != null ? attributeModels.Where(model => model.IsComplex == false && model.AttributeModelType != AttributeModelType.Relationship).ToList<IAttributeModel>() : null;

            PopulateAttributeCells(entityRow, ref columnIndex, MDMObjectFactory.GetIAttributeCollection(nonComplexAttributes), MDMObjectFactory.GetIAttributeModelCollection(nonComplexAttributeModels), creationOptions, includeValidations, columnStyles);

            // Write complex attributes
            IList<IAttribute> complexAttributes = attributes != null ? attributes.Where(attr => attr.IsComplex).ToList<IAttribute>() : null;
            IList<IAttributeModel> complexAttributeModels = attributeModels != null ? attributeModels.Where(model => model.IsComplex).ToList<IAttributeModel>() : null;

            PopulateComplexAttributeCells(entity, MDMObjectFactory.GetIAttributeCollection(complexAttributes), MDMObjectFactory.GetIAttributeModelCollection(complexAttributeModels), creationOptions, workbookPart);
        }

        private void PopulateMetadataValues(Row entityRow, IList<String> metadataAttributes, IEntity entity, RSExcelCreationOptions creationOptions)
        {
            UInt32 columnIndex = 1;

            if (metadataAttributes.Contains(RSExcelConstants.EntityDataTemplateColumns[EntityDataTemplateFieldEnum.Id]))
            {
                OpenSpreadsheetUtility.AppendRowWithNumberCell(entityRow, columnIndex++, entity.Id); // Id
            }

            if (metadataAttributes.Contains(RSExcelConstants.EntityDataTemplateColumns[EntityDataTemplateFieldEnum.ExtenalId]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(entityRow, columnIndex++, entity.Name); // ExternalId
            }

            if (metadataAttributes.Contains(RSExcelConstants.EntityDataTemplateColumns[EntityDataTemplateFieldEnum.LongName]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(entityRow, columnIndex++, entity.LongName); // LongName
            }

            if (metadataAttributes.Contains(RSExcelConstants.EntityDataTemplateColumns[EntityDataTemplateFieldEnum.EntityType]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(entityRow, columnIndex++, entity.EntityTypeName); // EntityType
            }

            if (metadataAttributes.Contains(RSExcelConstants.EntityDataTemplateColumns[EntityDataTemplateFieldEnum.CategoryPath]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(entityRow, columnIndex++, entity.CategoryPath); // CateogryPath
            }

            if (metadataAttributes.Contains(RSExcelConstants.EntityDataTemplateColumns[EntityDataTemplateFieldEnum.CategoryLongNamePath]))
            {
                String categoryLongNamePath = String.Empty;

                if (creationOptions.PopulateCategoryLongNamePath)
                {
                    categoryLongNamePath = entity.CategoryLongNamePath;
                }

                OpenSpreadsheetUtility.AppendRowWithTextCell(entityRow, columnIndex++, categoryLongNamePath);

            }

            if (metadataAttributes.Contains(RSExcelConstants.EntityDataTemplateColumns[EntityDataTemplateFieldEnum.Container]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(entityRow, columnIndex++, entity.ContainerName); // ContainerName
            }

            if (metadataAttributes.Contains(RSExcelConstants.EntityDataTemplateColumns[EntityDataTemplateFieldEnum.Organization]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(entityRow, columnIndex++, entity.OrganizationName); // OrganizationName
            }

            if (metadataAttributes.Contains(RSExcelConstants.EntityDataTemplateColumns[EntityDataTemplateFieldEnum.ParentExternalId]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(entityRow, columnIndex++, entity.ParentExternalId); // ParentExternalId
            }

            if (metadataAttributes.Contains(RSExcelConstants.EntityDataTemplateColumns[EntityDataTemplateFieldEnum.ParentExtensionExternalId]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(entityRow, columnIndex++, entity.ParentExtensionEntityExternalId); // ParentExtensionEntityExternalId
            }

            if (metadataAttributes.Contains(RSExcelConstants.EntityDataTemplateColumns[EntityDataTemplateFieldEnum.ParentExtensionCategoryPath]))
            {
                // Get parent extension category path value based on RSExcelCreationOptions.CategoryPathType
                String parentExtensionCategoryPath = GetParentExtensionCategoryPath(entity, creationOptions);
                OpenSpreadsheetUtility.AppendRowWithTextCell(entityRow, columnIndex++, parentExtensionCategoryPath); // ParentExtensionEntityCategoryPath
            }

            if (metadataAttributes.Contains(RSExcelConstants.EntityDataTemplateColumns[EntityDataTemplateFieldEnum.ParentExtensionContainer]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(entityRow, columnIndex++, entity.ParentExtensionEntityContainerName); // ParentExtensionEntityContainerName
            }

            if (metadataAttributes.Contains(RSExcelConstants.EntityDataTemplateColumns[EntityDataTemplateFieldEnum.ParentExtensionOrganization]))
            {
                // TODO:: We don't have entity.ParentExtensionEntityOrganization property..What to do??
                OpenSpreadsheetUtility.AppendRowWithTextCell(entityRow, columnIndex, String.Empty); // ParentExtensionEntityOrganization..
            }
        }
        
        private void AddNumberFormatingsToSheet(Worksheet workSheet, Stylesheet stylesheet, List<BO.AttributeModel> nonComplexAttributeModels, Int32 skipColumns)
        {
            // Adding formats restrictions
            Dictionary<String, UInt32Value> numberFormatMappings;
            NumberingFormats numberingFormats = NumberingFormatsProvider.Provider.GetNumberingFormats(nonComplexAttributeModels, out numberFormatMappings, skipColumns);
            if (numberingFormats != null && numberFormatMappings != null && numberFormatMappings.Any())
            {
                if (numberingFormats.Any())
                {
                    if (stylesheet.NumberingFormats.IsNullOrEmpty())
                    {
                        stylesheet.InsertAt(numberingFormats, 0);
                    }
                    else
                    {
                        stylesheet.NumberingFormats.Append(numberingFormats);
                    }
                }

                CellFormats cellFormats = stylesheet.CellFormats;
                Dictionary<String, UInt32Value> columnsStyleFormatMappings = new Dictionary<String, UInt32Value>();

                foreach (KeyValuePair<String, UInt32Value> numberFormatMapping in numberFormatMappings)
                {
                    CellFormat cellFormat = new CellFormat
                    {
                        FillId = 0,
                        BorderId = 0,
                        FormatId = 0,
                        NumberFormatId = numberFormatMapping.Value,
                        ApplyNumberFormat = BooleanValue.FromBoolean(true)
                    };

                    // append cell format for cells of header row
                    cellFormats.AppendChild(cellFormat);

                    // update font count 
                    cellFormats.Count = UInt32Value.FromUInt32((UInt32)cellFormats.ChildElements.Count);

                    columnsStyleFormatMappings.Add(numberFormatMapping.Key, cellFormats.Count - 1);
                }

                foreach (KeyValuePair<String, UInt32Value> columnsStyleFormatMapping in columnsStyleFormatMappings)
                {
                    if (_attributeColumnMappings.ContainsKey(columnsStyleFormatMapping.Key))
                    {
                        Column column = _attributeColumnMappings[columnsStyleFormatMapping.Key];
                        if (column != null)
                        {
                            column.Style = columnsStyleFormatMapping.Value;
                        }
                    }
                }
            }

            workSheet.Save();
        }

        private void AddHeirarchialParentCellToEntityRow(Row entityRow, Dictionary<Row, IDictionary<String, Cell>> hierachialRowCellsValues, IDictionary<String, Column> hierarchialAttributeColumnMappings, String parentHeirarchialColumnName, UInt32 parentHierarchialRowIndex)
        {
            if (!String.IsNullOrEmpty(parentHeirarchialColumnName))
            {
                if (hierarchialAttributeColumnMappings.ContainsKey(parentHeirarchialColumnName))
                {
                    Column col = hierarchialAttributeColumnMappings[parentHeirarchialColumnName];
                    var cell = OpenSpreadsheetUtility.CreateSAXNumberCell(entityRow, col.Min, parentHierarchialRowIndex);
                    if ((cell != null) && hierachialRowCellsValues.ContainsKey(entityRow))
                    {
                        var cellReference = cell.CellReference.Value;

                        if (!hierachialRowCellsValues[entityRow].ContainsKey(cellReference))
                        {
                            hierachialRowCellsValues[entityRow].Add(cellReference, cell);
                        }
                        else
                        {
                            hierachialRowCellsValues[entityRow][cellReference] = cell;
                        }
                    }
                }
            }
        }

        private void WriteHeirarchialAttributeDataToCells(DataTemplate dataTemplate, Row entityRow, OpenXmlWriter writer, IEntity entity, IAttributeCollection attributeCollection, IAttributeModelCollection childModelsCollection, RSExcelCreationOptions creationOptions, Dictionary<Row, IDictionary<String, Cell>> hierachialRowCellsValues, IDictionary<String, Column> hierarchialAttributeColumnMappings, ref UInt32 rowIndex, String parentHeirarchialColumnName = "", UInt32 parentHierarchialRowIndex = 0)
        {
            //Populate the parent Hierarchial Index
            AddHeirarchialParentCellToEntityRow(entityRow, hierachialRowCellsValues, hierarchialAttributeColumnMappings, parentHeirarchialColumnName, parentHierarchialRowIndex);

            #region Non-Hierarchial Models

            foreach (BO.AttributeModel attributeModel in childModelsCollection.Where(model => !model.IsHierarchical))
            {

                foreach (LocaleEnum locale in dataTemplate.RequestedLocales)
                {
                    if (!attributeModel.IsLocalizable)
                    {
                        if (locale != attributeModel.Locale)
                        {
                            continue;
                        }
                    }
                    IAttribute attribute = attributeCollection.GetAttribute(attributeModel.Id, locale);

                    if (attribute == null)
                    {
                        continue;
                    }

                    String value = GetAttributeValue(attribute, attributeModel, creationOptions, attributeModel.Locale);

                    SpaceProcessingModeValues spaceProcessingMode =
                        String.Equals(attributeModel.AttributeDataTypeName,
                            AttributeDataType.String.ToString(), StringComparison.InvariantCulture)
                            ? SpaceProcessingModeValues.Preserve
                            : SpaceProcessingModeValues.Default;

                    Boolean isNumberAttribute = attribute.AttributeDataType ==
                                                AttributeDataType.Decimal ||
                                                attribute.AttributeDataType ==
                                                AttributeDataType.Integer;

                    // If the number attribute has a UOM, write it as text in excel
                    if (isNumberAttribute &&
                        !String.IsNullOrWhiteSpace(attributeModel.AllowableUOM) ||
                        !String.IsNullOrWhiteSpace(attributeModel.DefaultUOM))
                    {
                        isNumberAttribute = false;
                    }

                    String attrHeaderText = String.Concat(attributeModel.AttributeParentName, "//",
                    GetComplexChildColumnHeaderText(attributeModel, creationOptions), "//", locale);

                    if (!hierarchialAttributeColumnMappings.ContainsKey(attrHeaderText))
                    {
                        continue;
                    }

                    var col = hierarchialAttributeColumnMappings[attrHeaderText];
                    Cell cell = null;

                    if (isNumberAttribute && !attribute.HasInvalidValues)
                    {
                        cell = OpenSpreadsheetUtility.CreateSAXNumberCell(entityRow, col.Min, value);
                    }
                    else
                    {
                        cell = OpenSpreadsheetUtility.CreateSAXTextCell(entityRow, col.Min, value, SpaceProcessingModeValues.Preserve);
                    }

                    if ((cell != null) && hierachialRowCellsValues.ContainsKey(entityRow))
                    {
                        var cellReference = cell.CellReference.Value;

                        if (!hierachialRowCellsValues[entityRow].ContainsKey(cellReference))
                        {
                            hierachialRowCellsValues[entityRow].Add(cellReference, cell);
                        }
                        else
                        {
                            hierachialRowCellsValues[entityRow][cellReference] = cell;
                        }

                    }


                }
            }
            #endregion

            #region Heirarchial Attributes

            foreach (BO.AttributeModel attributeModel in childModelsCollection.Where(model => model.IsHierarchical))
            {
                IAttributeModelCollection childAttributeModels = attributeModel.GetChildAttributeModels();
                var hierarchialAttribute =
                        (BO.Attribute)attributeCollection.GetAttribute(attributeModel.Name);

                if (attributeModel.IsCollection)
                {
                    String attrHeaderText = String.Concat(attributeModel.AttributeParentName, "//",
                    GetComplexChildColumnHeaderText(attributeModel, creationOptions));

                    if (!hierarchialAttributeColumnMappings.ContainsKey(attrHeaderText))
                    {
                        continue;
                    }

                    var values = GetAllCurrentValues(attributeCollection, attributeModel.Name).ToList();

                    Int32 i = -1;
                    foreach (var value in values)
                    {
                        i++;
                        if (i > 0)
                        {
                            entityRow = CreateEntityHierarchialRowWithFixedColumns(entity, creationOptions, hierachialRowCellsValues, ref rowIndex);

                            AddHeirarchialParentCellToEntityRow(entityRow, hierachialRowCellsValues, hierarchialAttributeColumnMappings, parentHeirarchialColumnName, parentHierarchialRowIndex);
                        }

                        WriteHeirarchialAttributeDataToCells(dataTemplate, entityRow, writer, entity, GetHierarchyChildAttributes(attributeCollection, value.ValueRefId), childAttributeModels, creationOptions, hierachialRowCellsValues, hierarchialAttributeColumnMappings, ref rowIndex, attrHeaderText, (UInt32)i);
                    }
                }
                else
                {
                    WriteHeirarchialAttributeDataToCells(dataTemplate, entityRow, writer, entity, GetHierarchyChildAttributes(hierarchialAttribute), attributeModel.GetChildAttributeModels(), creationOptions, hierachialRowCellsValues, hierarchialAttributeColumnMappings, ref rowIndex);
                }
            }

            #endregion
        }
        
        #endregion

        #region RelationshipSheet Methods

        private void PopulateRelationshipSheet(WorkbookPart workbookPart, IEntityCollection entityCollection, DataTemplate dataTemplate, RSExcelCreationOptions creationOptions)
        {
            Worksheet workSheet = OpenSpreadsheetUtility.GetWorksheet(workbookPart, RSExcelConstants.RelationshipSheetName);

            if (workSheet != null)
            {
                SheetData sheetData = workSheet.GetFirstChild<SheetData>();

                UInt32 rowIndex = 1; // Header row is always at first row..means index = 1;

                Row headerRow = OpenSpreadsheetUtility.GetRow(sheetData, rowIndex);

                // Get the style index from the first column
                Cell headerFirstCell = OpenSpreadsheetUtility.GetDataCell(sheetData, ColumnOne, rowIndex);
                rowIndex++;

                ApplyStyleSheet(workbookPart.WorkbookStylesPart.Stylesheet, headerRow);
                IEnumerable<BO.AttributeModel> relationshipAttributeModels = null;

                if (!_showHiddenAttributesOnExport)
                {
                    relationshipAttributeModels = dataTemplate.RelationshipAttributeModels.Where(model => !model.IsHidden);
                }
                else
                {
                    relationshipAttributeModels = dataTemplate.RelationshipAttributeModels;
                }

                PopulateAttributeHeaderCells(workSheet.WorksheetPart, headerRow, dataTemplate.RelationshipAttributeModels, creationOptions, OpenSpreadsheetUtility.GetCellStyleIndex(headerFirstCell), null, dataTemplate.LookupAttributeModelsContextPresence);

                // loop through each entity and create entity row..
                foreach (IEntity entity in entityCollection)
                {
                    foreach (IRelationship relationship in entity.GetRelationships())
                    {
                        Row relationshipRow = OpenSpreadsheetUtility.GetRow(sheetData, rowIndex++);
                        PopulateRelationshipRow(relationshipRow, relationship, entity, dataTemplate, creationOptions);
                    }
                }

                workSheet.Save();
            }
        }

        private void PopulateRelationshipSheetSAXParser(WorkbookPart workbookPart, IEntityCollection entityCollection, DataTemplate dataTemplate, RSExcelCreationOptions creationOptions)
        {
            Worksheet workSheet = OpenSpreadsheetUtility.GetWorksheet(workbookPart, RSExcelConstants.RelationshipSheetName);

            if (workSheet != null)
            {
                SheetData sheetData = workSheet.GetFirstChild<SheetData>();

                UInt32 rowIndex = 1; // Header row is always at first row..means index = 1;

                Row headerRow = OpenSpreadsheetUtility.GetRow(sheetData, rowIndex);

                Stylesheet stylesheet = workbookPart.WorkbookStylesPart.Stylesheet;
                // Applying styles from styles template in case it is found
                ApplyStyleSheet(stylesheet, headerRow);

                WorksheetPart replacementPart = workbookPart.AddNewPart<WorksheetPart>();
                String replacementId = workbookPart.GetIdOfPart(replacementPart);

                OpenXmlReader reader = OpenXmlReader.Create(workSheet.WorksheetPart);
                OpenXmlWriter writer = OpenXmlWriter.Create(replacementPart);

                while (reader.Read())
                {
                    if (reader.ElementType == typeof(SheetData))
                    {
                        if (reader.IsEndElement)
                        {
                            continue;
                        }
                        SheetData baseSheetData = workSheet.WorksheetPart.Worksheet.Elements<SheetData>().First();

                        writer.WriteStartElement(new SheetData());

                        foreach (Row r in baseSheetData.Elements<Row>())
                        {
                            writer.WriteStartElement(r);
                            foreach (Cell c in r.Elements<Cell>())
                            {
                                writer.WriteElement(c);
                            }
                            writer.WriteEndElement();
                        }

                        foreach (IEntity entity in entityCollection)
                        {
                            foreach (IRelationship relationship in entity.GetRelationships())
                            {
                                Row relationshipRow = new Row();
                                relationshipRow.RowIndex = ++rowIndex;
                                PopulateRelationshipRow(relationshipRow, relationship, entity, dataTemplate, creationOptions);
                                writer.WriteStartElement(relationshipRow);
                                foreach (Cell cell in relationshipRow.Elements<Cell>())
                                {
                                    writer.WriteElement(cell);
                                }
                                writer.WriteEndElement();
                            }
                        }

                        writer.WriteEndElement();
                    }
                    else if (reader.ElementType == typeof(Drawing) || reader.ElementType == typeof(LegacyDrawing))
                    {
                        //Skipping Drawing and LegacyDrawing as they contain comments which are corrupting the workSheet
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
                reader.Close();
                writer.Close();

                rowIndex = 1;

                headerRow = OpenSpreadsheetUtility.GetRow(replacementPart.Worksheet.GetFirstChild<SheetData>(), rowIndex);

                // Get the style index from the first column
                Cell headerFirstCell = OpenSpreadsheetUtility.GetDataCell(replacementPart.Worksheet.GetFirstChild<SheetData>(), ColumnOne, rowIndex);

                IEnumerable<BO.AttributeModel> relationshipAttributeModels = null;

                if (!_showHiddenAttributesOnExport)
                {
                    relationshipAttributeModels = dataTemplate.RelationshipAttributeModels.Where(model => !model.IsHidden);
                }
                else
                {
                    relationshipAttributeModels = dataTemplate.RelationshipAttributeModels;
                }

                PopulateAttributeHeaderCells(replacementPart, headerRow, dataTemplate.RelationshipAttributeModels, creationOptions, OpenSpreadsheetUtility.GetCellStyleIndex(headerFirstCell), null, dataTemplate.LookupAttributeModelsContextPresence);

                workSheet.Save();

                Sheet relationshipSheet = workbookPart.Workbook.Descendants<Sheet>().FirstOrDefault(s => s.Name == RSExcelConstants.RelationshipSheetName);
                if (relationshipSheet != null)
                {
                    relationshipSheet.Id = replacementId;
                }
                workbookPart.DeletePart(workSheet.WorksheetPart);
                workbookPart.Workbook.Save();
            }
        }

        private void PopulateRelationshipRow(Row relationshipRow, IRelationship iRelationship, IEntity entity, DataTemplate dataTemplate, RSExcelCreationOptions creationOptions)
        {
            UInt32 columnIndex = 1;

            // Create Fixed field cells
            #region Create Fixed field cells

            BO.Relationship relationship = (BO.Relationship)iRelationship;

            //Get category path value based on RSExcelCreationOptions.CategoryPathType
            String categoryPath = GetCategoryPath(entity, creationOptions);

            OpenSpreadsheetUtility.CreateNumberCell(relationshipRow, columnIndex++, relationship.Id); //Id
            OpenSpreadsheetUtility.CreateTextCell(relationshipRow, columnIndex++, relationship.RelationshipExternalId); //ExternalId
            OpenSpreadsheetUtility.CreateTextCell(relationshipRow, columnIndex++, relationship.RelationshipTypeName); //Relationship Type
            OpenSpreadsheetUtility.CreateTextCell(relationshipRow, columnIndex++, entity.ExternalId); // FromExternalId
            OpenSpreadsheetUtility.CreateTextCell(relationshipRow, columnIndex++, entity.EntityTypeName); // FromEntityType
            OpenSpreadsheetUtility.CreateTextCell(relationshipRow, columnIndex++, categoryPath); // FromCateogryPath
            OpenSpreadsheetUtility.CreateTextCell(relationshipRow, columnIndex++, entity.ContainerName); // FromContainerName
            OpenSpreadsheetUtility.CreateTextCell(relationshipRow, columnIndex++, entity.OrganizationName); // FromOrganizationName

            OpenSpreadsheetUtility.CreateTextCell(relationshipRow, columnIndex++, relationship.ToExternalId); // FromExternalId
            OpenSpreadsheetUtility.CreateTextCell(relationshipRow, columnIndex++, relationship.ToEntityTypeName); // FromEntityType

            // TODO :: check if we need to support category path option for relationship also.
            OpenSpreadsheetUtility.CreateTextCell(relationshipRow, columnIndex++, relationship.ToCategoryPath); // FromCateogryPath
            OpenSpreadsheetUtility.CreateTextCell(relationshipRow, columnIndex++, relationship.ToContainerName); // FromContainerName
            OpenSpreadsheetUtility.CreateTextCell(relationshipRow, columnIndex++, String.Empty); // FromOrganizationName

            #endregion

            #region Create Attribute cells

            PopulateAttributeCells(relationshipRow, ref columnIndex, relationship.GetRelationshipAttributes(), dataTemplate.RelationshipAttributeModels, creationOptions);

            #endregion
        }

        #endregion

        #region Attribute Management Methods

        private void PopulateComplexAttributeHeaderCells(WorkbookPart workbookPart, IAttributeModelCollection attributeModels, RSExcelCreationOptions creationOptions, UInt32 styleIndex, DataTemplate dataTemplate, IDictionary<Int32, IDictionary<String, Column>> hierarchialAttributeColumnMappings = null)
        {
            if (attributeModels != null)
            {
                foreach (BO.AttributeModel model in attributeModels)
                {
                    // Find or create complex sheet
                  CreateComplexAttributeSheetAndAttributeHeaders(workbookPart, model, creationOptions, styleIndex, dataTemplate, hierarchialAttributeColumnMappings);
                }
            }
        }

        private List<Cell> PopulateSAXAttributeCells(Row dataRow, ref UInt32 columnIndex, IAttributeCollection attributeCollection, IAttributeModelCollection attributeModelCollection, RSExcelCreationOptions creationOptions, Dictionary<Int32, UInt32Value> columnStyles = null)
        {
            Boolean areStylesProvided = columnStyles != null && columnStyles.Any();
            String stringDataType = AttributeDataType.String.ToString();
            List<Cell> cells = new List<Cell>();
            
            foreach (IAttributeModel attributeModel in attributeModelCollection)
            {
                String value = String.Empty;
                Boolean hasInvalidValues = false;
                IAttribute attribute = attributeCollection.GetAttribute(attributeModel.Id, attributeModel.Locale);

                if (attribute != null)
                {
                    value = GetAttributeValue(attribute, attributeModel as BO.AttributeModel, creationOptions, attributeModel.Locale);
                    hasInvalidValues = attribute.HasInvalidValues;
                }

                Int32 index = (Int32)columnIndex - 1;

                SpaceProcessingModeValues spaceProcessingMode = String.Equals(attributeModel.AttributeDataTypeName, stringDataType, StringComparison.InvariantCulture)
                                                                    ? SpaceProcessingModeValues.Preserve
                                                                    : SpaceProcessingModeValues.Default;

                String attributeDataType = attributeModel.AttributeDataTypeName;
                Boolean isNumberAttribute = (!attributeModel.IsCollection) && (attributeDataType.Equals(AttributeDataType.Decimal.ToString(), StringComparison.InvariantCultureIgnoreCase) ||
                    attributeDataType.Equals(AttributeDataType.Integer.ToString(), StringComparison.InvariantCultureIgnoreCase));

                UInt32? columnStyle = null;
                if (areStylesProvided && columnStyles.ContainsKey(index))
                {
                    columnStyle = columnStyles[index];
                }

                // If the number attribute has a UOM, write it as text in excel
                if (isNumberAttribute && !String.IsNullOrWhiteSpace(attributeModel.AllowableUOM) || !String.IsNullOrWhiteSpace(attributeModel.DefaultUOM))
                {
                    isNumberAttribute = false;
                }

                if (!hasInvalidValues && isNumberAttribute)
                {
                    Cell cell = OpenSpreadsheetUtility.CreateSAXNumberCell(dataRow, columnIndex++, value, columnStyle);
                    cells.Add(cell);
                }
                else
                {
                    Cell cell = OpenSpreadsheetUtility.CreateSAXTextCell(dataRow, columnIndex++, value, spaceProcessingMode, columnStyle);
                    cells.Add(cell);
                }
            }
            return cells;
        }

        private void PopulateAttributeCells(Row dataRow, ref UInt32 columnIndex, IAttributeCollection attributeCollection, IAttributeModelCollection attributeModelCollection,
            RSExcelCreationOptions creationOptions, Boolean includeValidations = false, Dictionary<Int32, UInt32Value> columnStyles = null)
        {
            Boolean areStylesProvided = (_attributeColumnMappings != null && _attributeColumnMappings.Count > 0);
            String stringDataType = AttributeDataType.String.ToString();

            foreach (IAttributeModel attributeModel in attributeModelCollection)
            {
                if (IsAttributeQualifiedForExport(attributeModel.IsHidden))
                {
                String value = String.Empty;
                    IAttribute attribute = attributeCollection.GetAttribute(attributeModel.Id, attributeModel.Locale);
                    String attrKey = String.Concat(attributeModel.AttributeParentName, "//", attributeModel.Name + "//" + attributeModel.Locale);
                Boolean hasInvalidValue = false;

                if (attribute != null)
                {
                        value = GetAttributeValue(attribute, attributeModel as BO.AttributeModel, creationOptions, attributeModel.Locale);
                    hasInvalidValue = attribute.HasInvalidValues;
                }

                Int32 index = (Int32)columnIndex - 1;

                SpaceProcessingModeValues spaceProcessingMode = String.Equals(attributeModel.AttributeDataTypeName, stringDataType, StringComparison.InvariantCulture)
                                                                    ? SpaceProcessingModeValues.Preserve
                                                                    : SpaceProcessingModeValues.Default;

                Boolean isNumberAttribute = attributeModel.AttributeDataTypeName.Equals(AttributeDataType.Decimal.ToString(), StringComparison.InvariantCultureIgnoreCase) ||
                    attributeModel.AttributeDataTypeName.Equals(AttributeDataType.Integer.ToString(), StringComparison.InvariantCultureIgnoreCase);

                UInt32? columnStyle = null;
                    if (areStylesProvided && _attributeColumnMappings.ContainsKey(attrKey))
                {
                        Column column = _attributeColumnMappings[attrKey];

                        if (column != null && column.Style != null)
                        {
                            columnStyle = column.Style;
                }
                    }

                if (includeValidations)
                {
                    // If the number attribute has a UOM, write it as text in excel
                    if (isNumberAttribute && !String.IsNullOrWhiteSpace(attributeModel.AllowableUOM) || !String.IsNullOrWhiteSpace(attributeModel.DefaultUOM))
                    {
                        isNumberAttribute = false;
                    }

                    if (!hasInvalidValue && isNumberAttribute)
                    {
                        OpenSpreadsheetUtility.AppendRowWithNumberCell(dataRow, columnIndex++, value, columnStyle);
                    }
                    else
                    {
                        OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, value, spaceProcessingMode, columnStyle);
                    }
                }
                else
                {
                    OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, value, spaceProcessingMode, columnStyle);
                }
            }
        }
        }
        
        private void PopulateComplexAttributeCells(IEntity entity, IAttributeCollection attributeCollection, IAttributeModelCollection attributeModelCollection,
            RSExcelCreationOptions creationOptions, WorkbookPart workbookPart)
        {
            foreach (IAttributeModel attributeModel in attributeModelCollection)
            {
                if (IsAttributeQualifiedForExport(attributeModel.IsHidden))
                {
                String complexSheetKey = attributeModel.AttributeParentName + "//" + attributeModel.Name;

                String complexSheetName = String.Empty;

                if (_complexAttributes.ContainsKey(complexSheetKey))
                {
                    complexSheetName = _complexAttributes[complexSheetKey];
                }

                Worksheet workSheet = OpenSpreadsheetUtility.GetWorksheet(workbookPart, complexSheetName);

                if (workSheet != null)
                {
                    SheetData sheetData = workSheet.GetFirstChild<SheetData>();

                    IAttribute attribute = attributeCollection.GetAttribute(attributeModel.Id, attributeModel.Locale);

                    if (attribute != null)
                    {
                        UInt32 rowIndex = 2;

                        IEnumerable<KeyValuePair<Int32, UInt32>> matchedIndex = _attributeWiseRowIndex.Where(pair => pair.Key == attribute.Id);

                        if (matchedIndex.Any())
                        {
                            rowIndex = matchedIndex.FirstOrDefault().Value;
                        }

                        if (attribute.HasValue())
                        {
                            foreach (IAttribute instanceAttribute in attribute.GetChildAttributes(attribute.SourceFlag))
                            {
                                Row entityRow = OpenSpreadsheetUtility.GetRow(sheetData, rowIndex++);

                                UInt32 columnIndex = 1;

                                #region Create Fixed field cells

                                //Get category path value based on RSExcelCreationOptions.CategoryPathType
                                String categoryPath = GetCategoryPath(entity, creationOptions);

                                OpenSpreadsheetUtility.CreateNumberCell(entityRow, columnIndex++, entity.Id); //Id
                                OpenSpreadsheetUtility.CreateTextCell(entityRow, columnIndex++, entity.Name); //ExternalId
                                OpenSpreadsheetUtility.CreateTextCell(entityRow, columnIndex++, entity.LongName); //LongName
                                OpenSpreadsheetUtility.CreateTextCell(entityRow, columnIndex++, entity.EntityTypeName); // EntityType
                                OpenSpreadsheetUtility.CreateTextCell(entityRow, columnIndex++, categoryPath); // CateogryPath
                                OpenSpreadsheetUtility.CreateTextCell(entityRow, columnIndex++, entity.ContainerName); // ContainerName
                                OpenSpreadsheetUtility.CreateTextCell(entityRow, columnIndex++, entity.OrganizationName); // OrganizationName
                                OpenSpreadsheetUtility.CreateTextCell(entityRow, columnIndex++, attributeModel.Locale); // ParentExtensionEntityOrganization..

                                #endregion

                                //The complex child attribute to be shown in the excel in the same order as db returns it.
                                //DB returns based on value specified in sequence field and also in the order of creation in case of same sequence value.
                                foreach (IAttributeModel instanceAttributeModel in attributeModel.GetChildAttributeModels())
                                {
                                    IAttribute complexChildAttribute = null;
                                    IAttributeCollection childAttributes = instanceAttribute.GetChildAttributes();

                                    if (childAttributes != null && childAttributes.Count > 0)
                                    {
                                        complexChildAttribute = childAttributes.GetAttribute(instanceAttributeModel.Id, instanceAttributeModel.Locale);
                                    }

                                        if (IsAttributeQualifiedForExport(instanceAttributeModel.IsHidden))
                                        {
                                    String value = GetAttributeValue(complexChildAttribute, instanceAttributeModel as BO.AttributeModel, creationOptions, attributeModel.Locale);

                                    //TODO:: Right now all data are stored as text....Need to handle data type of the cell based on attribute data type..
                                    OpenSpreadsheetUtility.CreateTextCell(entityRow, columnIndex++, value);
                                }
                            }
                        }
                            }

                        //Update the current row index back into dictionary
                        _attributeWiseRowIndex.Remove(matchedIndex.FirstOrDefault());
                        _attributeWiseRowIndex.Add(new KeyValuePair<Int32, UInt32>(attribute.Id, rowIndex));
                    }
                }
            }
        }
        }

        private void PopulateComplexAttributeCellsSAXParser(IEntity entity, DataTemplate dataTemplate, IAttributeCollection attributeCollection, IAttributeModelCollection attributeModelCollection, RSExcelCreationOptions creationOptions, WorkbookPart workbookPart)
        {
            foreach (BO.AttributeModel attributeModel in attributeModelCollection)
            {
                if (IsAttributeQualifiedForExport(attributeModel.IsHidden))
                {
                    ComplexAttributeExcelInfo excelInfo = null;
                    IEnumerable<KeyValuePair<Int32, ComplexAttributeExcelInfo>> matchedPair = from pair in _complexAttributeWiseExcelInfos
                                                                                              where pair.Key == attributeModel.Id
                                                                                              select pair;
                    IAttributeModelCollection childAttributeModels = attributeModel.GetChildAttributeModels();

                    if (matchedPair.Any())
                    {
                        excelInfo = matchedPair.FirstOrDefault().Value;
                    }

                    if (excelInfo != null)
                    {
                        IAttribute attribute = attributeCollection.GetAttribute(attributeModel.Id, attributeModel.Locale);

                            if (attribute != null)
                            {
                                UInt32 rowIndex = excelInfo.RowIndex;

                                if (attribute.HasValue())
                                {
                                    foreach (IAttribute instanceAttribute in attribute.GetChildAttributes(attribute.SourceFlag))
                                    {
                                        Row entityRow = new Row();
                                        entityRow.RowIndex = rowIndex++;

                                        // Get the style index from the first column
                                        String stringDataType = AttributeDataType.String.ToString();

                                        UInt32 columnIndex = 1;

                                        #region Create Fixed field cells

                                        //Get category path value based on RSExcelCreationOptions.CategoryPathType
                                        String categoryPath = GetCategoryPath(entity, creationOptions);

                                        List<Cell> cells = new List<Cell>();

                                        cells.Add(OpenSpreadsheetUtility.CreateSAXNumberCell(entityRow, columnIndex++, entity.Id)); //Id
                                        cells.Add(OpenSpreadsheetUtility.CreateSAXTextCell(entityRow, columnIndex++, entity.Name, SpaceProcessingModeValues.Preserve)); //ExternalId
                                        cells.Add(OpenSpreadsheetUtility.CreateSAXTextCell(entityRow, columnIndex++, entity.LongName, SpaceProcessingModeValues.Preserve)); //LongName
                                        cells.Add(OpenSpreadsheetUtility.CreateSAXTextCell(entityRow, columnIndex++, entity.EntityTypeName, SpaceProcessingModeValues.Preserve)); // EntityType
                                        cells.Add(OpenSpreadsheetUtility.CreateSAXTextCell(entityRow, columnIndex++, categoryPath, SpaceProcessingModeValues.Preserve)); // CateogryPath
                                        cells.Add(OpenSpreadsheetUtility.CreateSAXTextCell(entityRow, columnIndex++, entity.ContainerName, SpaceProcessingModeValues.Preserve)); // ContainerName
                                        cells.Add(OpenSpreadsheetUtility.CreateSAXTextCell(entityRow, columnIndex++, entity.OrganizationName, SpaceProcessingModeValues.Preserve)); // OrganizationName
                                    cells.Add(OpenSpreadsheetUtility.CreateSAXTextCell(entityRow, columnIndex++, attribute.Locale, SpaceProcessingModeValues.Preserve)); // ParentExtensionEntityOrganization.. 

                                        #endregion

                                        OpenXmlWriter writer = excelInfo.Writer;
                                        writer.WriteStartElement(entityRow);

                                        foreach (Cell entitycell in cells)
                                        {
                                            writer.WriteElement(entitycell);
                                        }

                                    #region Write data for Non hierarchal complex child attributes

                                        //The complex child attribute to be shown in the excel in the same order as db returns it.
                                        //DB returns based on value specified in sequence field and also in the order of creation in case of same sequence value.
                                        foreach (IAttributeModel instanceAttributeModel in childAttributeModels)
                                        {
                                            IAttribute complexChildAttribute = null;

                                            IAttributeCollection childAttributes = instanceAttribute.GetChildAttributes();

                                            if (childAttributes != null && childAttributes.Count > 0)
                                            {
                                            complexChildAttribute = childAttributes.GetAttribute(instanceAttributeModel.Id, instanceAttributeModel.Locale);
                                            }

                                            BO.AttributeModel childAttributeModel = childAttributeModels.GetAttributeModel(complexChildAttribute.Id, attributeModel.Locale) as BO.AttributeModel;

                                            if (IsAttributeQualifiedForExport(childAttributeModel.IsHidden))
                                            {
                                            String value = GetAttributeValue(complexChildAttribute, instanceAttributeModel as BO.AttributeModel, creationOptions, attribute.Locale);

                                                if (complexChildAttribute != null)
                                                {
                                                    SpaceProcessingModeValues spaceProcessingMode = String.Equals(instanceAttributeModel.AttributeDataTypeName, stringDataType, StringComparison.InvariantCulture) ? SpaceProcessingModeValues.Preserve : SpaceProcessingModeValues.Default;

                                                    Boolean isNumberAttribute = complexChildAttribute.AttributeDataType == AttributeDataType.Decimal || complexChildAttribute.AttributeDataType == AttributeDataType.Integer;

                                                    // If the number attribute has a UOM, write it as text in excel
                                                    if (isNumberAttribute && !String.IsNullOrWhiteSpace(instanceAttributeModel.AllowableUOM) || !String.IsNullOrWhiteSpace(instanceAttributeModel.DefaultUOM))
                                                    {
                                                        isNumberAttribute = false;
                                                    }

                                                    if (isNumberAttribute && !complexChildAttribute.HasInvalidValues)
                                                    {
                                                        writer.WriteElement(OpenSpreadsheetUtility.CreateSAXNumberCell(entityRow, columnIndex++, value));
                                                    }
                                                    else
                                                    {
                                                        writer.WriteElement(OpenSpreadsheetUtility.CreateSAXTextCell(entityRow, columnIndex++, value, spaceProcessingMode));
                                                    }
                                                }
                                                else
                                                {
                                                    writer.WriteElement(OpenSpreadsheetUtility.CreateSAXTextCell(entityRow, columnIndex++, value, SpaceProcessingModeValues.Preserve));
                                                }
                                            }
                                        }

                                        writer.WriteEndElement();

                                        #endregion
                                    }

                                    //Update the current row index back into dictionary
                                    _complexAttributeWiseExcelInfos.Remove(new KeyValuePair<Int32, ComplexAttributeExcelInfo>(attributeModel.Id, excelInfo));
                                    excelInfo.RowIndex = rowIndex;
                                    _complexAttributeWiseExcelInfos.Add(new KeyValuePair<Int32, ComplexAttributeExcelInfo>(attributeModel.Id, excelInfo));
                                }
                            }
                        }
                    }
                }
            }

        private void PopulateHierarchialAttributeCellsSAXParser(IEntity entity, DataTemplate dataTemplate, IAttributeCollection attributeCollection, IAttributeModelCollection attributeModelCollection, RSExcelCreationOptions creationOptions, IDictionary<Int32, IDictionary<String, Column>> hierarchialAttributeColumnMappings)
        {
         
          //For each Hierarchial Attribute model write the data to corresponding data sheets.
          foreach (BO.AttributeModel attributeModel in attributeModelCollection)
          {
            ComplexAttributeExcelInfo excelInfo = null;
            Int32 id = attributeModel.Id;
            IEnumerable<KeyValuePair<Int32, ComplexAttributeExcelInfo>> matchedPair =
                _complexAttributeWiseExcelInfos.Where(pair => pair.Key == id);
            IAttributeModelCollection childAttributeModels = attributeModel.GetChildAttributeModels();
            if (matchedPair.Any())
            {
              excelInfo = matchedPair.FirstOrDefault().Value;
            }

            //Continue if the sheet is not found for the complex attribute
            if (excelInfo == null)
            {
              continue;
            }

            UInt32 rowIndex = excelInfo.RowIndex;
            OpenXmlWriter writer = excelInfo.Writer;
            Dictionary<Row, IDictionary<String, Cell>> hierachialRowCellsValues = new Dictionary<Row, IDictionary<String, Cell>>();

            if (attributeModel.IsCollection)
            {
              IValueCollection values = GetAllCurrentValues(attributeCollection, attributeModel.Name);

              UInt32 attributeHierarcyLevel = 0;
              foreach (var value in values)
              {
                var entityRow = CreateEntityHierarchialRowWithFixedColumns(entity, creationOptions, hierachialRowCellsValues, ref rowIndex);

                String attrHeaderText = String.Concat(attributeModel.AttributeParentName, "//",
                GetComplexChildColumnHeaderText(attributeModel, creationOptions));

                WriteHeirarchialAttributeDataToCells(dataTemplate, entityRow, writer, entity, GetHierarchyChildAttributes(attributeCollection, value.ValueRefId), childAttributeModels, creationOptions, hierachialRowCellsValues, hierarchialAttributeColumnMappings[attributeModel.Id], ref rowIndex, attrHeaderText, attributeHierarcyLevel);
                attributeHierarcyLevel++;
              }
            }
            else
            { 
                    var entityRow = CreateEntityHierarchialRowWithFixedColumns(entity, creationOptions, hierachialRowCellsValues, ref rowIndex);

              WriteHeirarchialAttributeDataToCells(dataTemplate, entityRow, writer, entity, GetHierarchyChildAttributes(attributeCollection), childAttributeModels, creationOptions, hierachialRowCellsValues, hierarchialAttributeColumnMappings[attributeModel.Id], ref rowIndex);
            } 

            //Write the elements int the order of the cell reference otherwise the Excel gets corrupted.
            foreach (Row entityRow in hierachialRowCellsValues.Keys)
            {
                List<String> rowCellReferences = new List<String>();

                //Build the Meta data references 1 to 7 i.e A to G
                for (UInt32 i = 1; i < 8; i++)
                {
                    rowCellReferences.Add(OpenSpreadsheetUtility.GetCellReferenceAddress(entityRow, i));
                }

                rowCellReferences.AddRange(hierarchialAttributeColumnMappings[attributeModel.Id].Values.Select(column => OpenSpreadsheetUtility.GetCellReferenceAddress(entityRow, column.Min)).ToList());
                writer.WriteStartElement(entityRow);

                IDictionary<String, Cell> cells = hierachialRowCellsValues[entityRow];

                foreach (String cellReference in rowCellReferences) 
                {
                    if (cells.ContainsKey(cellReference))
                    {
                        writer.WriteElement(cells[cellReference]);
                    }
                }

                writer.WriteEndElement();
            }

            //Update the current row index back into dictionary
            _complexAttributeWiseExcelInfos.Remove(
                new KeyValuePair<Int32, ComplexAttributeExcelInfo>(attributeModel.Id, excelInfo));
            excelInfo.RowIndex = rowIndex;
            _complexAttributeWiseExcelInfos.Add(new KeyValuePair<Int32, ComplexAttributeExcelInfo>(
                attributeModel.Id, excelInfo));

            //Clean the records.
            foreach (Row row in hierachialRowCellsValues.Keys)
            {
              hierachialRowCellsValues[row].Clear();
            }
            hierachialRowCellsValues.Clear();
          }
        }

        private Row CreateEntityHierarchialRowWithFixedColumns(IEntity entity, RSExcelCreationOptions creationOptions, IDictionary<Row, IDictionary<String, Cell>> hierachialRowCellsValues, ref uint rowIndex)
        {
            Row entityRow = new Row { RowIndex = rowIndex++ };

          if (hierachialRowCellsValues == null)
          {
            hierachialRowCellsValues = new Dictionary<Row, IDictionary<string, Cell>>();
          }

          UInt32 columnIndex = 1;

          #region Create HierachialRow field cells

          //Get category path value based on RSExcelCreationOptions.CategoryPathType
          String categoryPath = GetCategoryPath(entity, creationOptions);

          //Id
          //ExternalId
          //LongName
          // EntityType
          // CateogryPath
          // ContainerName
          // OrganizationName
          IDictionary<String, Cell> cells = new Dictionary<String, Cell>();

          hierachialRowCellsValues.Add(entityRow, cells);

          var cell = OpenSpreadsheetUtility.CreateSAXNumberCell(entityRow, columnIndex++, entity.Id);
            cells.Add(cell.CellReference.Value, cell);
          cell = OpenSpreadsheetUtility.CreateSAXTextCell(entityRow, columnIndex++, entity.Name, SpaceProcessingModeValues.Preserve);
          cells.Add(cell.CellReference.Value, cell);
          cell = OpenSpreadsheetUtility.CreateSAXTextCell(entityRow, columnIndex++, entity.LongName, SpaceProcessingModeValues.Preserve);
          cells.Add(cell.CellReference.Value, cell);
          cell = OpenSpreadsheetUtility.CreateSAXTextCell(entityRow, columnIndex++, entity.EntityTypeName, SpaceProcessingModeValues.Preserve);
          cells.Add(cell.CellReference.Value, cell);
          cell = OpenSpreadsheetUtility.CreateSAXTextCell(entityRow, columnIndex++, categoryPath, SpaceProcessingModeValues.Preserve);
          cells.Add(cell.CellReference.Value, cell);
          cell = OpenSpreadsheetUtility.CreateSAXTextCell(entityRow, columnIndex++, entity.ContainerName, SpaceProcessingModeValues.Preserve);
          cells.Add(cell.CellReference.Value, cell);
          cell = OpenSpreadsheetUtility.CreateSAXTextCell(entityRow, columnIndex++, entity.OrganizationName, SpaceProcessingModeValues.Preserve);
          cells.Add(cell.CellReference.Value, cell);
          
        #endregion

          return entityRow;
        }

        private IAttributeCollection GetHierarchyChildAttributes(IAttributeCollection hierarchicalAttributeCollection, int instanceReferenceId)
        {
            IAttributeCollection childAttributeCollection = MDMObjectFactory.GetIAttributeCollection();

            foreach (var attribute in hierarchicalAttributeCollection)
            {
                if (!attribute.IsHierarchical) continue;
                var childItems = GetHierarchyChildAttributes(attribute, instanceReferenceId);
                if ((childItems != null) && childItems.Any())
                {
                    childAttributeCollection.AddRange(childItems);
                    }
                }

            return childAttributeCollection;
            }

        private IValueCollection GetAllCurrentValues(IAttributeCollection attributeCollection, String attributeModelName)
        {
            IValueCollection values = new BO.ValueCollection();
            foreach (var attribute in attributeCollection.GetAttributes(attributeModelName))
            {
            foreach (var value in attribute.GetCurrentValues())
                {
                values.Add(value);
                }

            }

            return values;
        }

        private IAttributeCollection GetHierarchyChildAttributes(IAttributeCollection hierarchicalAttributeCollection)
        {
            IAttributeCollection childAttributeCollection = MDMObjectFactory.GetIAttributeCollection();

            foreach (var attribute in hierarchicalAttributeCollection)
                {
                if (!attribute.IsHierarchical) continue;
                var childItems = GetHierarchyChildAttributes(attribute);
                if ((childItems != null) && childItems.Any())
                    {
                    childAttributeCollection.AddRange(childItems);
                    }
                }

            return childAttributeCollection;
        }

        private IAttributeCollection GetHierarchyChildAttributes(BO.Attribute hierarcyAttribute)
        {
            if (!hierarcyAttribute.IsHierarchical)
            {
            throw new ArgumentException("Attribute is not a Hierarchial Attribute.");
            }

            IAttributeCollection childAttributes = MDMObjectFactory.GetIAttributeCollection();

            var childInstanceAttributes = hierarcyAttribute.GetChildAttributes();

            foreach (var childInstanceAttribute in childInstanceAttributes)
            {
                childAttributes.AddRange(childInstanceAttribute.GetChildAttributes());
            } 
 
            return childAttributes;

        }

        private IAttributeCollection GetHierarchyChildAttributes(BO.Attribute hierarcyAttribute, int instanceReferenceId)
        {
            if (!hierarcyAttribute.IsHierarchical)
            {
                throw new ArgumentException("Attribute is not a Hierarchial Attribute.");
            }

            IAttributeCollection childAttributes = MDMObjectFactory.GetIAttributeCollection();

            var childInstanceAttributes = hierarcyAttribute.GetChildAttributes().Where(cia => cia.InstanceRefId == instanceReferenceId);

            foreach (var childInstanceAttribute in childInstanceAttributes)
                {
                childAttributes.AddRange(childInstanceAttribute.GetChildAttributes());
                }

            return childAttributes;

        }
        
        private String GetAttributeValue(IAttribute attribute, MDM.BusinessObjects.AttributeModel attributeModel, RSExcelCreationOptions creationOptions, LocaleEnum locale = LocaleEnum.UnKnown)
        {
            String stringVal = String.Empty;

            if (locale == LocaleEnum.UnKnown)
            {
                locale = GlobalizationHelper.GetSystemDataLocale();
            }

            if (attribute != null)
            {
                if (attribute.HasAnyValue())
                {
                    ILookup lookup = null;

                    if (!attribute.IsCollection)
                    {
                        IValue val;

                        if (attribute.AttributeDataType == AttributeDataType.Date || attribute.AttributeDataType == AttributeDataType.Decimal || attribute.AttributeDataType == AttributeDataType.DateTime)
                        {
                            val = attribute.GetCurrentValueInstanceInvariant();

                            if (attributeModel.IsComplexChild)
                            {
                                if (attribute.AttributeDataType == AttributeDataType.Date)
                                {
                                    DateTime dateTime = Convert.ToDateTime(val.GetStringValue());
                                    stringVal = dateTime.ToShortDateString();
                                }
                                else if (attribute.AttributeDataType == AttributeDataType.DateTime)
                                {
                                    DateTime dateTime = Convert.ToDateTime(val.GetStringValue());
                                    stringVal = FormatHelper.StoreDateTimeUtc(dateTime);
                                }
                            }

                            if (!String.IsNullOrWhiteSpace(val.Uom))
                            {
                                val = attribute.GetCurrentValueInstance();
                            }
                        }
                        else
                        {
                            val = attribute.GetCurrentValueInstance();
                        }

                        if (val != null)
                        {
                            if (String.IsNullOrWhiteSpace(stringVal))
                            {
                                    stringVal = val.GetStringValue();
                                }

                            if (!String.IsNullOrWhiteSpace(val.Uom))
                            {
                                stringVal = String.Concat(stringVal, creationOptions.UomSeparator, val.Uom);
                            }
                        }
                    }
                    else
                    {
                        Boolean isFirstRecord = true;

                        foreach (IValue val in attribute.GetCurrentValues())
                        {
                            String currentVal = val.GetStringValue();

                            if (attributeModel.IsLookup && lookup != null)
                            {
                                currentVal = val.ValueRefId > 0 ? lookup.GetDisplayFormatById(val.ValueRefId) : String.Empty; 
                            }

                            if (!String.IsNullOrWhiteSpace(val.Uom))
                            {
                                currentVal = String.Concat(currentVal, creationOptions.UomSeparator, val.Uom);
                            }

                            if (isFirstRecord)
                            {
                                stringVal = currentVal;
                                isFirstRecord = false;
                            }
                            else
                            {
                                stringVal = String.Concat(stringVal, RSExcelConstants.CollectionSeparator, currentVal);
                            }
                        }
                    }
                }
            }

            return stringVal;
        }

        #endregion

        #region Helper Methods
        
        private void UpdateSeparatorValues()
        {
            String collectionSeparator = MDM.Utility.AppConfigurationHelper.GetAppConfig<String>("MDM.Exports.RSExcelFormatter.CollectionSeparator", string.Empty);
            if (!String.Equals(RSExcelConstants.CollectionSeparator, collectionSeparator))
            {
                RSExcelConstants.CollectionSeparator = collectionSeparator;
            }

            String uomSeparator = MDM.Utility.AppConfigurationHelper.GetAppConfig<String>("MDM.Exports.RSExcelFormatter.UomSeparator", string.Empty);
            if (!String.Equals(RSExcelConstants.UomSeparator, uomSeparator))
            {
                RSExcelConstants.UomSeparator = uomSeparator;
            }
        }

        private void InitializeReportComplexAttributeSheets(IList<IAttributeModel> attributeModelCollection, RSExcelCreationOptions creationOptions, WorkbookPart workbookPart)
        {
            foreach (IAttributeModel attributeModel in attributeModelCollection)
            {
                String complexSheetKey = attributeModel.AttributeParentName + "//" + attributeModel.Name;

                String complexSheetName = String.Empty;

                if (_complexAttributes.ContainsKey(complexSheetKey))
                {
                    complexSheetName = _complexAttributes[complexSheetKey];
                }

                Worksheet workSheet = OpenSpreadsheetUtility.GetWorksheet(workbookPart, complexSheetName);

                if (workSheet != null)
                {
                    SheetData sheetData = workSheet.GetFirstChild<SheetData>();

                    WorksheetPart replacementPart = workbookPart.AddNewPart<WorksheetPart>();
                    String replacementId = workbookPart.GetIdOfPart(replacementPart);

                    OpenXmlReader reader = OpenXmlReader.Create(workSheet.WorksheetPart);
                    OpenXmlWriter writer = OpenXmlWriter.Create(replacementPart);

                    while (reader.Read())
                    {
                        if (reader.ElementType == typeof(SheetData))
                        {
                            if (reader.IsEndElement)
                            {
                                continue;
                            }
                            SheetData baseSheetData = workSheet.WorksheetPart.Worksheet.Elements<SheetData>().First();

                            writer.WriteStartElement(new SheetData());

                            foreach (Row row in baseSheetData.Elements<Row>())
                            {
                                writer.WriteStartElement(row);
                                foreach (Cell cell in row.Elements<Cell>())
                                {
                                    writer.WriteElement(cell);
                                }
                                writer.WriteEndElement();
                            }

                            ComplexAttributeExcelInfo excelInfo = new ComplexAttributeExcelInfo()
                            {
                                SheetData = sheetData,
                                Writer = writer,
                                Reader = reader,
                                RowIndex = 2,
                                ReplacementId = replacementId,
                                WorkSheet = workSheet,
                                SheetName = complexSheetName
                            };

                            _complexAttributeWiseExcelInfos.Add(new KeyValuePair<Int32, ComplexAttributeExcelInfo>(attributeModel.Id, excelInfo));
                            break;
                        }

                        if (reader.ElementType == typeof(DataValidations))
                        {
                            if (reader.IsEndElement)
                                continue;

                            writer.WriteStartElement(new DataValidations());
                            reader.Read();

                            while (reader.IsStartElement && reader.ElementType == typeof(DataValidation))
                            {
                                writer.WriteStartElement(reader);
                                reader.Read();

                                while (reader.ElementType == typeof(Formula1) || reader.ElementType == typeof(Formula2))
                                {
                                    if (reader.IsStartElement)
                                    {
                                        writer.WriteStartElement(reader);
                                        writer.WriteString(reader.GetText());
                                        writer.WriteEndElement();
                                    }
                                    reader.Read();
                                }

                                writer.WriteEndElement();
                                reader.Read();
                            }

                            writer.WriteEndElement();
                        }
                        else if (reader.ElementType == typeof(Drawing) || reader.ElementType == typeof(LegacyDrawing) || reader.ElementType == typeof(VmlDrawingPart))
                        {
                            //Skipping Drawing and LegacyDrawing as they contain comments which are corrupting the workSheet
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

        private void CloseReportComplexAttributeSheets(IList<IAttributeModel> attributeModelCollection, RSExcelCreationOptions creationOptions, WorkbookPart workbookPart)
        {
            foreach (IAttributeModel attributeModel in attributeModelCollection)
            {
                ComplexAttributeExcelInfo excelInfo = null;
                Int32 id = attributeModel.Id;
                IEnumerable<KeyValuePair<Int32, ComplexAttributeExcelInfo>> matchedPair = _complexAttributeWiseExcelInfos.Where(pair => pair.Key == id);

                if (matchedPair.Any())
                {
                    excelInfo = matchedPair.FirstOrDefault().Value;
                }

                if (excelInfo != null)
                {
                    OpenXmlReader reader = excelInfo.Reader;
                    OpenXmlWriter writer = excelInfo.Writer;

                    while (reader.Read())
                    {
                        if (reader.ElementType == typeof(SheetData))
                        {
                            if (reader.IsEndElement)
                            {
                                writer.WriteEndElement();
                                continue;
                            }
                        }
                        else if (reader.ElementType == typeof(DataValidations))
                        {
                            if (reader.IsEndElement)
                                continue;

                            writer.WriteStartElement(new DataValidations());
                            reader.Read();

                            while (reader.IsStartElement && reader.ElementType == typeof(DataValidation))
                            {
                                writer.WriteStartElement(reader);
                                reader.Read();

                                while (reader.ElementType == typeof(Formula1) || reader.ElementType == typeof(Formula2))
                                {
                                    if (reader.IsStartElement)
                                    {
                                        writer.WriteStartElement(reader);
                                        writer.WriteString(reader.GetText());
                                        writer.WriteEndElement();
                                    }
                                    reader.Read();
                                }

                                writer.WriteEndElement();
                                reader.Read();
                            }

                            writer.WriteEndElement();
                        }
                        else if (reader.ElementType == typeof(Drawing) || reader.ElementType == typeof(LegacyDrawing) || reader.ElementType == typeof(VmlDrawingPart))
                        {
                            //Skipping Drawing and LegacyDrawing as they contain comments which are corrupting the workSheet
                        }
                        else if (reader.ElementType == typeof(Row) || reader.ElementType == typeof(Cell) || reader.ElementType == typeof(Value))
                        {
                            //Skipping header row as reader was still pointing to it
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

                    reader.Close();
                    writer.Close();
                    excelInfo.WorkSheet.Save();

                    Sheet originalComplexSheet = workbookPart.Workbook.Descendants<Sheet>().FirstOrDefault(s => s.Name == excelInfo.SheetName);
                    if (originalComplexSheet != null)
                    {
                        originalComplexSheet.Id = excelInfo.ReplacementId;
                    }
                    workbookPart.DeletePart(excelInfo.WorkSheet.WorksheetPart);
                    workbookPart.Workbook.Save();
                }
            }
        }

        private String GetTemporaryFileLocation()
        {
            return AppConfigurationHelper.GetAppConfig<String>(IsJobServiceRequest ? "Jobs.TemporaryFileRoot" : "WCF.TemporaryFileRoot");
        }

        /// <summary>
        /// CopyFile
        /// </summary>
        /// <param name="source">The source location</param>
        /// <param name="dest">The destination path</param>
        private void CopyFile(String source, String dest)
        {
            // Overwrites existing files
            File.Copy(source, dest, true);
        }

        private static void CountStyles(Stylesheet stylesheet)
        {
            _fontCount = (UInt32)stylesheet.Fonts.ChildElements.Count;
            _fillCount = (UInt32)stylesheet.Fills.ChildElements.Count;
            _cellFormatCount = (UInt32)stylesheet.CellFormats.ChildElements.Count;
        }

        private static void AppendStyles(Stylesheet stylesheet, UInt32 fontsCount, UInt32 fillsCount, UInt32 cellsCount)
        {
            CellStyleData cellStyles = BuildStyles(_indexes, fontsCount, fillsCount, cellsCount);

            stylesheet.Fonts.Append(cellStyles.Fonts);
            stylesheet.Fonts.Count = UInt32Value.FromUInt32((UInt32)stylesheet.Fonts.ChildElements.Count);

            stylesheet.Fills.Append(cellStyles.Fills);
            stylesheet.Fills.Count = UInt32Value.FromUInt32((UInt32)stylesheet.Fills.ChildElements.Count);

            stylesheet.CellFormats.Append(cellStyles.CellFormats);
            stylesheet.CellFormats.Count = UInt32Value.FromUInt32((UInt32)stylesheet.CellFormats.ChildElements.Count);
        }

        private static void MapStylesToAttributes(WorksheetStyles worksheetStyles)
        {
            if (worksheetStyles == null)
            {
                MapDefaultStylesToAttributes();
            }
            else
            {
                MapTemplateStylesToAttributes(worksheetStyles);
            }
        }

        private static void MapDefaultStylesToAttributes()
        {
            _indexes[CommonRequiredKey].Style = new AttributesTypeStyle
            {
                FontRgb = new HexBinaryValue(RSExcelConstants.DefaultRequiredFontRgb),
                BackgroundRgb = new HexBinaryValue(RSExcelConstants.DefaultCommonAttributeBackGroundColorFillRgb)
            };
            _indexes[TechnicalRequiredKey].Style = new AttributesTypeStyle
            {
                FontRgb = new HexBinaryValue(RSExcelConstants.DefaultRequiredFontRgb),
                BackgroundRgb = new HexBinaryValue(RSExcelConstants.DefaultTechnicalAttributeBackGroundColorFillRgb)
            };
            _indexes[RelationshipRequiredKey].Style = new AttributesTypeStyle
            {
                FontRgb = new HexBinaryValue(RSExcelConstants.DefaultRequiredFontRgb),
                BackgroundRgb = new HexBinaryValue(RSExcelConstants.DefaultRelationshipAttributeBackGroundColorFillRgb)
            };

            _indexes[CommonOptionalKey].Style = new AttributesTypeStyle
            {
                FontRgb = new HexBinaryValue(RSExcelConstants.DefaultCommonAttributeFontRgb),
                BackgroundRgb = new HexBinaryValue(RSExcelConstants.DefaultCommonAttributeBackGroundColorFillRgb)
            };
            _indexes[TechnicalOptionalKey].Style = new AttributesTypeStyle
            {
                FontRgb = new HexBinaryValue(RSExcelConstants.DefaultCommonAttributeFontRgb),
                BackgroundRgb = new HexBinaryValue(RSExcelConstants.DefaultTechnicalAttributeBackGroundColorFillRgb)
            };
            _indexes[RelationshipOptionalKey].Style = new AttributesTypeStyle
            {
                FontRgb = new HexBinaryValue(RSExcelConstants.DefaultCommonAttributeFontRgb),
                BackgroundRgb = new HexBinaryValue(RSExcelConstants.DefaultRelationshipAttributeBackGroundColorFillRgb)
            };

            _indexes[CommonCollectionRequiredKey].Style = _indexes[CommonRequiredKey].Style;
            _indexes[TechnicalCollectionRequiredKey].Style = _indexes[TechnicalRequiredKey].Style;
            _indexes[RelationshipCollectionRequiredKey].Style = _indexes[RelationshipRequiredKey].Style;

            _indexes[CommonCollectionOptionalKey].Style = _indexes[CommonOptionalKey].Style;
            _indexes[TechnicalCollectionOptionalKey].Style = _indexes[TechnicalOptionalKey].Style;
            _indexes[RelationshipCollectionOptionalKey].Style = _indexes[RelationshipOptionalKey].Style;

            _indexes[SystemAttributeKey].Style = new AttributesTypeStyle
            {
                FontRgb = new HexBinaryValue(RSExcelConstants.DefaultCommonAttributeFontRgb),
                BackgroundRgb = new HexBinaryValue(RSExcelConstants.DefaultSystemAttrFontRgb)
            };
            _indexes[WorkflowAttributeKey].Style = new AttributesTypeStyle
            {
                FontRgb = new HexBinaryValue(RSExcelConstants.DefaultCommonAttributeFontRgb),
                BackgroundRgb = new HexBinaryValue(RSExcelConstants.DefaultWorkflowAttrFontRgb)
            };

            MapDefaultFontStyleProperties();
        }

        private static void MapTemplateStylesToAttributes(WorksheetStyles worksheetStyles)
        {
            _indexes[CommonRequiredKey].Style = worksheetStyles.RequiredCommonAttributeStyle;
            _indexes[TechnicalRequiredKey].Style = worksheetStyles.RequiredCategorySpecificAttributeStyle;
            _indexes[RelationshipRequiredKey].Style = worksheetStyles.RequiredRelationshipAttributeStyle;

            _indexes[CommonOptionalKey].Style = worksheetStyles.OptionalCommonAttributeStyle;
            _indexes[TechnicalOptionalKey].Style = worksheetStyles.OptionalCategorySpecificAttributeStyle;
            _indexes[RelationshipOptionalKey].Style = worksheetStyles.OptionalRelationshipAttributeStyle;

            _indexes[CommonCollectionRequiredKey].Style = worksheetStyles.RequiredCommonAttributeCollectionStyle;
            _indexes[TechnicalCollectionRequiredKey].Style = worksheetStyles.RequiredCategorySpecificAttributeCollectionStyle;
            _indexes[RelationshipCollectionRequiredKey].Style = worksheetStyles.RequiredRelationshipAttributeCollectionStyle;

            _indexes[CommonCollectionOptionalKey].Style = worksheetStyles.OptionalCommonAttributeCollectionStyle;
            _indexes[TechnicalCollectionOptionalKey].Style = worksheetStyles.OptionalCategorySpecificAttributeCollectionStyle;
            _indexes[RelationshipCollectionOptionalKey].Style = worksheetStyles.OptionalRelationshipAttributeCollectionStyle;

            _indexes[SystemAttributeKey].Style = worksheetStyles.SystemAttributeStyle;
            _indexes[WorkflowAttributeKey].Style = worksheetStyles.WorkflowAttributeStyle;
        }

        private static void MapDefaultFontStyleProperties()
        {
            foreach (var style in _indexes.Values)
            {
                style.Style.FontName = "Calibri";
                style.Style.FontSizeVal = DoubleValue.FromDouble(10);
                style.Style.IsBold = true;
            }
        }

        private void ApplyStyleSheet(Stylesheet stylesheet, Row headerRow)
        {
            // Applying styles from styles template in case it is found
            Boolean useStylesTemplate = AppConfigurationHelper.GetAppConfig<Boolean>(UseStylesTemplateConfigName);

            WorksheetStyles worksheetStyles = null;

            // AttrIndexes indexes;
            if (useStylesTemplate)
            {
                BO.Template template = new ExportTemplateBL().GetExportTemplateByName(DefaultStyleTemplateFileName, new BO.CallerContext(MDMCenterApplication.PIM, MDMCenterModules.Export));

                if (template != null)
                {
                    String stylesTemplateFileName = GetTemplateFileName(DefaultStyleTemplateFileName, GetTemporaryFileLocation());
                    worksheetStyles = StylesTemplateProcessor.ProcessStylesTemplateFile(stylesTemplateFileName);

                    UpdateMetadataAttributesStyle(headerRow, stylesheet, worksheetStyles);
                }
            }

            ApplyStyleSheet(stylesheet, worksheetStyles);
        }

        private static CellStyleData BuildStyles(Dictionary<String, StyleIndexMapData> attributesStyles, UInt32 fontsCount, UInt32 fillsCount, UInt32 cellsCount)
        {
            CellStyleData styles = new CellStyleData
                {
                    Fonts = new List<Font>(),
                    Fills = new List<Fill>(),
                    CellFormats = new List<CellFormat>()
                };

            if (!attributesStyles.IsNullOrEmpty())
            {
                UInt32Value counter = 0;

                foreach (KeyValuePair<String, StyleIndexMapData> styleIndex in attributesStyles)
                {
                    styles.Fonts.Add(BuildFontStyle(styleIndex.Value.Style));
                    styles.Fills.Add(BuildFillStyle(styleIndex.Value.Style));
                    styles.CellFormats.Add(BuildCellFormatStyle(fontsCount, fillsCount, counter));

                    styleIndex.Value.Index = cellsCount + counter++;
                }
            }

            return styles;
        }

        private static Font BuildFontStyle(AttributesTypeStyle attributeTypeStyle)
        {
            return new Font
            {
                FontSize = new FontSize { Val = attributeTypeStyle.FontSizeVal },
                Color = new Color { Rgb = attributeTypeStyle.FontRgb, Indexed = attributeTypeStyle.FontIndexed },
                FontName = new FontName { Val = attributeTypeStyle.FontName },
                FontScheme = new FontScheme { Val = FontSchemeValues.Minor },
                Bold = attributeTypeStyle.IsBold ? new Bold() : null,
                Italic = attributeTypeStyle.IsItalic ? new Italic() : null
            };
        }

        private static Fill BuildFillStyle(AttributesTypeStyle attributeTypeStyle)
        {
            Fill fill = new Fill();

            PatternFill patternFill = new PatternFill
            {
                PatternType = PatternValues.Solid,
                ForegroundColor = new ForegroundColor
            {
                Rgb = attributeTypeStyle.BackgroundRgb,
                Indexed = attributeTypeStyle.BackgroundIndexed
            },
                BackgroundColor = new BackgroundColor { Indexed = 64U }
            };

            fill.Append(patternFill);

            return fill;
        }

        private static CellFormat BuildCellFormatStyle(UInt32 fontsCount, UInt32 fillsCount, UInt32Value counter)
        {
            return new CellFormat
            {
                NumberFormatId = 49U,
                FontId = fontsCount + counter,
                FillId = UInt32Value.FromUInt32(fillsCount + counter),
                BorderId = 1U,
                ApplyNumberFormat = true,
                ApplyFont = true,
                ApplyFill = true,
                ApplyBorder = true,
                ApplyAlignment = true,
                Alignment = new Alignment
                {
                    Horizontal = HorizontalAlignmentValues.Center,
                    Vertical = VerticalAlignmentValues.Center,
                    WrapText = true
                }
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isBold"></param>
        /// <returns></returns>
        private static Run GetDefaultTextRunElement(Boolean isBold = true)
        {
            Run result = new Run();

            RunProperties runProperties = new RunProperties();
            Bold bold = new Bold();
            FontSize fontSize = new FontSize { Val = 8D };
            Color color = new Color { Indexed = 81U };
            RunFont runFont = new RunFont { Val = "Tahoma" };
            FontFamily fontFamily = new FontFamily { Val = Int32Value.FromInt32(2) };

            if (isBold)
            {
                runProperties.Append(bold);
            }

            runProperties.Append(fontSize);
            runProperties.Append(color);
            runProperties.Append(runFont);
            runProperties.Append(fontFamily);

            result.Append(runProperties);

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmlDocument"></param>
        /// <param name="columnIndex"></param>
        /// <param name="rowIndex"></param>
        private static void CreateShapeAsXmlNode(XmlDocument xmlDocument, UInt32 columnIndex, UInt32 rowIndex)
        {
            #region Sample Xml
            /*
             <xml xmlns:v="urn:schemas-microsoft-com:vml" xmlns:o="urn:schemas-microsoft-com:office:office" xmlns:x="urn:schemas-microsoft-com:office:excel">
                <o:shapelayout v:ext="edit">
                    <o:idmap v:ext="edit" data="4" />
                </o:shapelayout>
                <v:shapetype id="_x0000_t202" coordsize="21600,21600" o:spt="202" path="m,l,21600r21600,l21600,xe">
                    <v:stroke joinstyle="miter" />
                    <v:path gradientshapeok="t" o:connecttype="rect" />
                </v:shapetype>
                <v:shape id="36d942814a3f4d6dbc06cd4c2ae59f00" type="#_x0000_t202" 
                        style="position:absolute;margin-left:40.5pt;margin-top:1.5pt;width:153.75pt;height:28.5pt;z-index:1;visibility:hidden;mso-wrap-style:tight" 
                        fillcolor="#ffffe1" o:insetmode="auto">
                    <v:fill color2="#ffffe1" />
                    <v:shadow on="t" color="black" obscured="t" />
                    <v:path o:connecttype="none" />
                    <v:textbox style="mso-direction-alt:auto">
                        <div style="text-align:left" />
                    </v:textbox>
                    <x:ClientData ObjectType="">
                        <x:MoveWithCells />
                        <x:SizeWithCells />
                        <x:Anchor>13,15,0,2,15,31,2,17</x:Anchor>
                        <x:AutoFill>False</x:AutoFill>
                        <x:Row>0</x:Row>
                        <x:Column>12</x:Column>
                    </x:ClientData>
                </v:shape>
            */
            #endregion Sample Xml

            #region Intial Setup

            const String prefixV = "v";
            const String nameSpaceForV = "urn:schemas-microsoft-com:vml";

            const String prefixO = "o";
            const String nameSpaceForO = "urn:schemas-microsoft-com:office:office";

            const String prefixX = "x";
            const String nameSpaceForX = "urn:schemas-microsoft-com:office:excel";

            #endregion

            #region Create Root Node

            if (String.IsNullOrWhiteSpace(xmlDocument.OuterXml) || xmlDocument.ChildNodes.Count < 1)
            {
                XmlNode rootNode = xmlDocument.CreateElement("xml");

                #region RootNode Attributes

                XmlAttribute xmlnsV = xmlDocument.CreateAttribute("xmlns:v");
                xmlnsV.Value = "urn:schemas-microsoft-com:vml";
                rootNode.Attributes.Append(xmlnsV);

                XmlAttribute xmlnsO = xmlDocument.CreateAttribute("xmlns:o");
                xmlnsO.Value = "urn:schemas-microsoft-com:office:office";
                rootNode.Attributes.Append(xmlnsO);

                XmlAttribute xmlnsX = xmlDocument.CreateAttribute("xmlns:x");
                xmlnsX.Value = "urn:schemas-microsoft-com:office:excel";
                rootNode.Attributes.Append(xmlnsX);

                #endregion

                #region Append ShapeLayOut Node into Root Node

                XmlNode shapeLayout = xmlDocument.CreateElement(prefixO, "shapelayout", nameSpaceForO);

                #region ShapeLayOut Attributes

                XmlAttribute ext = xmlDocument.CreateAttribute(prefixV, "ext", nameSpaceForV);
                ext.Value = "edit";
                shapeLayout.Attributes.Append(ext);

                #endregion

                #region Append IdMap Node into ShapeLayOut Node

                XmlNode idMap = xmlDocument.CreateElement(prefixO, "idmap", nameSpaceForO);

                #region IdMap Attributes

                XmlAttribute idMapExt = xmlDocument.CreateAttribute(prefixV, "ext", nameSpaceForV);
                idMapExt.Value = "edit";
                idMap.Attributes.Append(idMapExt);

                XmlAttribute data = xmlDocument.CreateAttribute("data");
                data.Value = "4";
                idMap.Attributes.Append(data);

                #endregion

                shapeLayout.AppendChild(idMap);

                #endregion

                rootNode.AppendChild(shapeLayout);

                #endregion

                #region Append ShapeType Node into Root Node

                XmlNode shapetype = xmlDocument.CreateElement(prefixV, "shapetype", nameSpaceForV);

                #region ShapeType Attributes

                XmlAttribute shapeTypeId = xmlDocument.CreateAttribute("id");
                ext.Value = "_x0000_t202";
                shapetype.Attributes.Append(shapeTypeId);

                XmlAttribute coordSize = xmlDocument.CreateAttribute("coordsize");
                coordSize.Value = "21600,21600";
                shapetype.Attributes.Append(coordSize);

                XmlAttribute spt = xmlDocument.CreateAttribute(prefixO, "spt", nameSpaceForO);
                spt.Value = "202";
                shapetype.Attributes.Append(spt);

                XmlAttribute path = xmlDocument.CreateAttribute("path");
                path.Value = "m,l,21600r21600,l21600,xe";
                shapetype.Attributes.Append(path);

                #endregion

                #region Append Stroke Node into ShapeType Node

                XmlNode stroke = xmlDocument.CreateElement(prefixV, "stroke", nameSpaceForV);

                #region Stroke Attributes

                XmlAttribute joinStyle = xmlDocument.CreateAttribute("joinstyle");
                joinStyle.Value = "miter";
                stroke.Attributes.Append(joinStyle);

                #endregion

                shapetype.AppendChild(stroke);

                #endregion

                #region Append Path Node into ShapeType Node

                XmlNode shapeTypePath = xmlDocument.CreateElement(prefixV, "path", nameSpaceForV);

                #region Path Attributes

                XmlAttribute gradientshapeok = xmlDocument.CreateAttribute("gradientshapeok");
                gradientshapeok.Value = "t";
                shapeTypePath.Attributes.Append(gradientshapeok);

                XmlAttribute pathConnectType = xmlDocument.CreateAttribute(prefixO, "connecttype", nameSpaceForO);
                pathConnectType.Value = "rect";
                shapeTypePath.Attributes.Append(pathConnectType);

                #endregion

                shapetype.AppendChild(shapeTypePath);

                #endregion

                rootNode.AppendChild(shapetype);

                #endregion

                xmlDocument.AppendChild(rootNode);
            }

            #endregion

            #region Create Shape Node

            XmlNode shapeNode = xmlDocument.CreateElement(prefixV, "shape", nameSpaceForV);

            #region ShapeNode Attributes

            XmlAttribute id = xmlDocument.CreateAttribute("id");
            id.Value = Guid.NewGuid().ToString().Replace("-", "");
            shapeNode.Attributes.Append(id);

            XmlAttribute type = xmlDocument.CreateAttribute("type");
            type.Value = "#_x0000_t202";
            shapeNode.Attributes.Append(type);

            XmlAttribute style = xmlDocument.CreateAttribute("style");
            style.Value = "position:absolute;margin-left:40.5pt;margin-top:1.5pt;width:153.75pt;height:28.5pt;z-index:1;visibility:hidden;mso-wrap-style:tight";
            shapeNode.Attributes.Append(style);

            XmlAttribute fillColor = xmlDocument.CreateAttribute("fillcolor");
            fillColor.Value = "#ffffe1";
            shapeNode.Attributes.Append(fillColor);

            XmlAttribute insetMode = xmlDocument.CreateAttribute(prefixO, "insetmode", nameSpaceForO);
            insetMode.Value = "auto";
            shapeNode.Attributes.Append(insetMode);

            #endregion

            #region Append Fill Node into Shape Node

            XmlNode fillNode = xmlDocument.CreateElement(prefixV, "fill", nameSpaceForV);

            #region FillNode Attributes

            XmlAttribute color2 = xmlDocument.CreateAttribute("color2");
            color2.Value = "#ffffe1";
            fillNode.Attributes.Append(color2);

            shapeNode.AppendChild(fillNode);

            #endregion

            #endregion

            #region Append Shadow Node into Shape Node

            XmlNode shadowNode = xmlDocument.CreateElement(prefixV, "shadow", nameSpaceForV);

            #region ShadowNode Attributes

            XmlAttribute on = xmlDocument.CreateAttribute("on");
            on.Value = "t";
            shadowNode.Attributes.Append(on);

            XmlAttribute color = xmlDocument.CreateAttribute("color");
            color.Value = "black";
            shadowNode.Attributes.Append(color);

            XmlAttribute obscured = xmlDocument.CreateAttribute("obscured");
            obscured.Value = "t";
            shadowNode.Attributes.Append(obscured);

            #endregion

            shapeNode.AppendChild(shadowNode);

            #endregion

            #region Append Path Node into Shape Node

            XmlNode pathNode = xmlDocument.CreateElement(prefixV, "path", nameSpaceForV);

            #region PathNode Attributes

            XmlAttribute connectType = xmlDocument.CreateAttribute(prefixO, "connecttype", nameSpaceForO);
            connectType.Value = "none";
            pathNode.Attributes.Append(connectType);

            #endregion

            shapeNode.AppendChild(pathNode);

            #endregion

            #region Append Textbox Node into Shape Node

            XmlNode textboxNode = xmlDocument.CreateElement(prefixV, "textbox", nameSpaceForV);

            #region TextBoxNode Attributes

            XmlAttribute textboxStyle = xmlDocument.CreateAttribute("style");
            textboxStyle.Value = "mso-direction-alt:auto";
            textboxNode.Attributes.Append(textboxStyle);

            #endregion

            #region Append Div Node into Textbox Node

            XmlNode divNode = xmlDocument.CreateElement("div");

            #region DivNode Attributes

            XmlAttribute divStyle = xmlDocument.CreateAttribute("style");
            divStyle.Value = "text-align:left";
            divNode.Attributes.Append(divStyle);

            #endregion

            textboxNode.AppendChild(divNode);

            #endregion

            shapeNode.AppendChild(textboxNode);

            #endregion

            #region Append ClientData Node into Shape Node

            XmlNode clientData = xmlDocument.CreateElement(prefixX, "ClientData", nameSpaceForX);

            #region ClientDataNode Attributes

            XmlAttribute objectType = xmlDocument.CreateAttribute("ObjectType");
            objectType.Value = "Note";
            clientData.Attributes.Append(objectType);

            #endregion

            #region Append MoveWithCells Node into ClientData Node

            XmlNode moveWithCellsNode = xmlDocument.CreateElement(prefixX, "MoveWithCells", nameSpaceForX);
            clientData.AppendChild(moveWithCellsNode);

            #endregion

            #region Append SizeWithCells Node into ClientData Node

            XmlNode sizeWithCellsNode = xmlDocument.CreateElement(prefixX, "SizeWithCells", nameSpaceForX);
            clientData.AppendChild(sizeWithCellsNode);

            #endregion

            #region Append Anchor Node into ClientData Node

            XmlNode anchorNode = xmlDocument.CreateElement(prefixX, "Anchor", nameSpaceForX);
            anchorNode.InnerText = GetAnchorCoordinatesForVMLCommentShape("1");
            clientData.AppendChild(anchorNode);

            #endregion

            #region Append AutoFill Node into ClientData Node

            XmlNode autoFillNode = xmlDocument.CreateElement(prefixX, "AutoFill", nameSpaceForX);
            autoFillNode.InnerText = "False";
            clientData.AppendChild(autoFillNode);

            #endregion

            #region Append Row Node into ClientData Node

            XmlNode rowNode = xmlDocument.CreateElement(prefixX, "Row", nameSpaceForX);
            rowNode.InnerText = rowIndex.ToString();
            clientData.AppendChild(rowNode);

            #endregion

            #region Append Column Node into ClientData Node

            XmlNode columnNode = xmlDocument.CreateElement(prefixX, "Column", nameSpaceForX);
            columnNode.InnerText = columnIndex.ToString();
            clientData.AppendChild(columnNode);

            #endregion

            shapeNode.AppendChild(clientData);

            #endregion

            xmlDocument.DocumentElement.AppendChild(shapeNode);

            #endregion
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="workbookPart"></param>
        /// <param name="model"></param>
        /// <param name="creationOptions"></param>
        /// <param name="styleIndex"></param>
        /// <param name="dataTemplate"></param>
        /// <param name="hierarchialAttributeColumnMappings"></param>
        private void CreateComplexAttributeSheetAndAttributeHeaders(WorkbookPart workbookPart, BO.AttributeModel model, RSExcelCreationOptions creationOptions, UInt32 styleIndex, DataTemplate dataTemplate, IDictionary<Int32, IDictionary<String, Column>> hierarchialAttributeColumnMappings)
        {
            if (_complexAttributes.Keys.Contains(String.Format("{0}//{1}", model.AttributeParentName, model.Name)) ||
                (!_showHiddenAttributesOnExport && model.IsHidden) ||
                AreAllComplexChildAttributesHidden(model))
            {
                return;
            }

            List<BO.AttributeModel> complexChildAttrModels;

            if (!_showHiddenAttributesOnExport)
            {
                complexChildAttrModels = model.GetChildAttributeModels().Where(childModel => !childModel.IsHidden).ToList();
            }
            else
            {
                complexChildAttrModels = model.GetChildAttributeModels().ToList();
            }

            String complexSheetName = model.AttributeParentName + "_" + model.Name;
            complexSheetName = complexSheetName.Length > 30 ? (model.Name.Length > 30 ? model.Name.Substring(0, 26) : model.Name) : complexSheetName;

            //Remove invalid characters from complex sheet name.
            complexSheetName = Regex.Replace(complexSheetName, @"[/\\*?[\]:]+", "");

            if (_complexSheetDictionary.ContainsKey(complexSheetName))
            {
                _complexSheetDictionary[complexSheetName]++;
                complexSheetName = complexSheetName + "(" + _complexSheetDictionary[complexSheetName] + ")";
            }
            else
            {
                _complexSheetDictionary[complexSheetName] = 0;
            }

            Worksheet workSheet = OpenSpreadsheetUtility.GetWorksheet(workbookPart, complexSheetName);

            if (workSheet == null)
            {
                //Adding to dictionary for making unique Sheet names

                String dictionaryKey = String.Format("{0}//{1}", model.AttributeParentName, model.Name);
                String dictionaryValue = complexSheetName;

                if (!_complexAttributes.Keys.Contains(dictionaryKey))
                {
                    _complexAttributes.Add(dictionaryKey, dictionaryValue);
                }

                WorksheetPart newWorksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                newWorksheetPart.Worksheet = new Worksheet(new SheetData());

                Sheets sheets = workbookPart.Workbook.GetFirstChild<Sheets>();
                String relationshipId = workbookPart.GetIdOfPart(newWorksheetPart);

                UInt32 sheetId = 1;
                if (sheets.Elements<Sheet>().Any())
                {
                    sheetId = sheets.Elements<Sheet>().Select(s => s.SheetId.Value).Max() + 1;
                }

                UInt32 columnWidth = AppConfigurationHelper.GetAppConfig<UInt32>("MDMCenter.Exports.RSExcelFormatter.ComplexAttributeSheet.ColumnWidth", 20);
                // Append the new worksheet and associate it with the workbook.
                Sheet sheet = new Sheet { Id = relationshipId, SheetId = sheetId, Name = complexSheetName };
                sheets.Append(sheet);

                #region Add Meta Data columns

                const UInt32 RowIndex = 1; //Header row is always at first row..means index = 1;
                UInt32 columnIndex = 1;

                Row headerRow = OpenSpreadsheetUtility.GetRow(newWorksheetPart.Worksheet.GetFirstChild<SheetData>(), RowIndex);
                Columns columns = new Columns();

                //this order should be same as in method CreateEntityHierarchialRowWithFixedColumns

                Cell idCell = OpenSpreadsheetUtility.CreateTextCell(headerRow, columnIndex++, "Id", _headerCellStyleIndex); //Id
                Cell externalIdCell = OpenSpreadsheetUtility.CreateTextCell(headerRow, columnIndex++, "External Id", _headerCellStyleIndex); //External Id
                Cell longNameCell = OpenSpreadsheetUtility.CreateTextCell(headerRow, columnIndex++, "Long Name", _headerCellStyleIndex); //Long Name
                Cell entityTypeCell = OpenSpreadsheetUtility.CreateTextCell(headerRow, columnIndex++, "Entity Type", _headerCellStyleIndex); // Entity Type
                Cell categoryPathCell = OpenSpreadsheetUtility.CreateTextCell(headerRow, columnIndex++, "Category Path", _headerCellStyleIndex); // Category Path
                Cell containerCell = OpenSpreadsheetUtility.CreateTextCell(headerRow, columnIndex++, "Container", _headerCellStyleIndex); // Container
                Cell organizationNameCell = OpenSpreadsheetUtility.CreateTextCell(headerRow, columnIndex++, "Organization", _headerCellStyleIndex); // Organization

                //Separate Column only for Non-hierachial complex attributes.
                if (!model.IsHierarchical) 
                {
                    Cell localeCell = OpenSpreadsheetUtility.CreateTextCell(headerRow, columnIndex++, "Locale",
                        _headerCellStyleIndex); // Locale
                }

                UInt32 index = columnIndex - 1;

                for (Int32 i = 0; i < index; i++)
                {
                    Column col = new Column { BestFit = true, Min = (UInt32)i + 1, Max = (UInt32)i + 1, CustomWidth = true, Width = columnWidth };
                    columns.Append(col);
                }

                #endregion Add metadata columns

                //Non-hierachial Complex attribute has only single level of nested attributes.
                if (!model.IsHierarchical)
                {
                    // XmlDocument vmlDrawing = ReadVMLDrawing(newWorksheetPart);
                    foreach (BO.AttributeModel childAttributeModel in complexChildAttrModels)
                    {
                        if (IsAttributeQualifiedForExport(childAttributeModel.IsHidden))
                        {
                            // Get attribute column header text based on configured option in RSExcelCreationOptions.AttributeColumnHeaderType
                            String attrHeaderText = GetComplexChildColumnHeaderText(childAttributeModel, creationOptions);

                            // add styles  based on the Attribute type
                            if (childAttributeModel.AttributeModelType == AttributeModelType.Category)
                            {
                                styleIndex = childAttributeModel.Required
                                    ? UInt32Value.FromUInt32(_indexes[TechnicalRequiredKey].Index)
                                    : UInt32Value.FromUInt32(_indexes[TechnicalOptionalKey].Index);
                            }
                            else if (childAttributeModel.AttributeModelType == AttributeModelType.Common)
                            {
                                styleIndex = childAttributeModel.Required
                                    ? UInt32Value.FromUInt32(_indexes[CommonRequiredKey].Index)
                                    : UInt32Value.FromUInt32(_indexes[CommonOptionalKey].Index);
                            }
                            else if (childAttributeModel.AttributeModelType == AttributeModelType.Relationship)
                            {
                                styleIndex = childAttributeModel.Required
                                    ? UInt32Value.FromUInt32(_indexes[RelationshipRequiredKey].Index)
                                    : UInt32Value.FromUInt32(_indexes[RelationshipOptionalKey].Index);
                            }

                            Column col = new Column
                            {
                                BestFit = true,
                                Min = columnIndex,
                                Max = columnIndex,
                                CustomWidth = true,
                                Width = columnWidth
                            };
                            columns.Append(col);

                            OpenSpreadsheetUtility.CreateTextCell(headerRow, columnIndex++, attrHeaderText, styleIndex);

                            index++;
                        }
                    }
                }
                else //Hierachial complex attributes have mutli level nested attribute models.
                {
                    if (hierarchialAttributeColumnMappings == null)
                    {
                        hierarchialAttributeColumnMappings = new Dictionary<Int32, IDictionary<String, Column>>();
                    }

                    if (!hierarchialAttributeColumnMappings.ContainsKey(model.Id))
                    {
                        hierarchialAttributeColumnMappings.Add(model.Id, new Dictionary<String, Column>());
                    }

                    // Get attribute column header text based on configured option in RSExcelCreationOptions.AttributeColumnHeaderType
                    String attrHeaderText = String.Concat(model.AttributeParentName, "//", GetComplexChildColumnHeaderText(model, creationOptions));

                    CreateTextCell(model, headerRow, attrHeaderText, columnWidth, ref columns,
                        ref styleIndex, ref index, ref columnIndex, hierarchialAttributeColumnMappings[model.Id]);

                    CreateHierarchialAttributeChildColumnHeaders(dataTemplate, complexChildAttrModels, creationOptions, headerRow,
                        columnWidth, hierarchialAttributeColumnMappings[model.Id], ref columns, ref styleIndex, ref index, ref columnIndex);
                }

                SheetData sheetData = newWorksheetPart.Worksheet.Descendants<SheetData>().FirstOrDefault();
                newWorksheetPart.Worksheet.InsertBefore(columns, sheetData);
                newWorksheetPart.Worksheet.Save();
            }

            #region Append Validation Info

            Boolean includeValidations = AppConfigurationHelper.GetAppConfig<Boolean>("MDM.Exports.RSExcelFormatter.IncludeValidations");
            if (includeValidations && dataTemplate.RequestedLocales.Count == 1)
            {
                Worksheet savedWorkSheet = OpenSpreadsheetUtility.GetWorksheet(workbookPart, complexSheetName);

                if (workSheet == null)
                {
                    //Adding to dictionary for making unique Sheet names

                    // Populate sheet with validation restrictions
                    const Int32 skipColumns = 8;

                    IDictionary<String, String> messages = new ValidationMessagesProvider().GetAllValidationMessagesDictionary();
                    Stylesheet stylesheet = workbookPart.WorkbookStylesPart.Stylesheet;

                    var complexChildAttrModelList = complexChildAttrModels.ToList();

                    ValidationProvider.Provider.AppendWorksheetWithValidation(workbookPart, savedWorkSheet, complexChildAttrModelList, messages, dataTemplate.ExcelValidationInfo, skipColumns);
                    AddNumberFormatingsToSheet(savedWorkSheet, stylesheet, complexChildAttrModelList, skipColumns);
                }
            }

            #endregion
        }


        private void CreateHierarchialAttributeChildColumnHeaders(DataTemplate dataTemplate, List<BO.AttributeModel> complexChildAttrModels, RSExcelCreationOptions creationOptions, Row headerRow, UInt32 columnWidth, IDictionary<String, Column> hierarchialAttributeColumnMappings, ref Columns columns, ref UInt32 styleIndex, ref UInt32 index, ref UInt32 columnIndex)
        {
            // XmlDocument vmlDrawing = ReadVMLDrawing(newWorksheetPart);
            foreach (BO.AttributeModel childAttributeModel in complexChildAttrModels)
            {
                // Get attribute column header text based on configured option in RSExcelCreationOptions.AttributeColumnHeaderType
                String attrHeaderText = String.Concat(childAttributeModel.AttributeParentName, "//", GetComplexChildColumnHeaderText(childAttributeModel, creationOptions));

                if (childAttributeModel.IsHierarchical)
                {
                    
                    CreateTextCell(childAttributeModel, headerRow, attrHeaderText, columnWidth, ref columns,
                            ref styleIndex, ref index, ref columnIndex, hierarchialAttributeColumnMappings);                    

                    CreateHierarchialAttributeChildColumnHeaders(dataTemplate,
                        childAttributeModel.GetChildAttributeModels().ToList<BO.AttributeModel>(), creationOptions, headerRow, columnWidth, hierarchialAttributeColumnMappings,
                        ref columns, ref styleIndex, ref index, ref columnIndex);
                }
                else
                {
                    foreach (var locale in dataTemplate.RequestedLocales)
                    {
                        if (!childAttributeModel.IsLocalizable && childAttributeModel.Locale != locale)
                        {
                            continue;
                        }

                        CreateTextCell(childAttributeModel, headerRow, String.Concat(attrHeaderText, "//", locale), columnWidth, ref columns, ref styleIndex, ref index, ref columnIndex, hierarchialAttributeColumnMappings);
                    }
                }
            }
        }

        private void CreateTextCell(BO.AttributeModel childAttributeModel, Row headerRow, String attrHeaderText, UInt32 columnWidth, ref Columns columns, ref UInt32 styleIndex, ref UInt32 index, ref UInt32 columnIndex, IDictionary<String, Column> hierarchialAttributeColumnMappings = null)
        { 
            // add styles  based on the Attribute type
            switch (childAttributeModel.AttributeModelType)
            {
                case AttributeModelType.Category:
                    styleIndex = childAttributeModel.Required
                        ? UInt32Value.FromUInt32(_indexes[TechnicalRequiredKey].Index)
                        : UInt32Value.FromUInt32(_indexes[TechnicalOptionalKey].Index);
                    break;
                case AttributeModelType.Common:
                    styleIndex = childAttributeModel.Required
                        ? UInt32Value.FromUInt32(_indexes[CommonRequiredKey].Index)
                        : UInt32Value.FromUInt32(_indexes[CommonOptionalKey].Index);
                    break;
                case AttributeModelType.Relationship:
                    styleIndex = childAttributeModel.Required
                        ? UInt32Value.FromUInt32(_indexes[RelationshipRequiredKey].Index)
                        : UInt32Value.FromUInt32(_indexes[RelationshipOptionalKey].Index);
                    break;
            }

            Column col = new Column
            {
                BestFit = true,
                Min = columnIndex,
                Max = columnIndex,
                CustomWidth = true,
                Width = columnWidth
            };
            columns.Append(col);

          if (hierarchialAttributeColumnMappings != null)
          {
            if (!hierarchialAttributeColumnMappings.ContainsKey(attrHeaderText))
            {
              hierarchialAttributeColumnMappings.Add(attrHeaderText, col);
            }
          }

          OpenSpreadsheetUtility.CreateTextCell(headerRow, columnIndex++, attrHeaderText, styleIndex);

            index++;
         }

        /// <summary>
        /// Get attribute column header text based on configuration in profile (formatter)
        /// </summary>
        /// <param name="attributeModel">The attribute model</param>
        /// <param name="creationOptions">The creation option</param>
        /// <returns>Column Header Text</returns>
        private static String GetAttributeColumnHeaderText(IAttributeModel attributeModel, RSExcelCreationOptions creationOptions)
        {
            String attributeColumnHeaderText = String.Empty;
            if (attributeModel != null)
            {
                LocaleEnum locale = attributeModel.IsLookup || attributeModel.IsLocalizable ? attributeModel.Locale : GlobalizationHelper.GetSystemDataLocale();

                switch (creationOptions.AttributeColumnHeaderType)
                {
                    case RSExcelAttributeColumnHeaderType.AttributeAndAttributeParentLongNameWithLocale:
                        attributeColumnHeaderText = String.Concat(attributeModel.AttributeParentLongName, "//", attributeModel.LongName, "//" + locale);
                        break;
                    case RSExcelAttributeColumnHeaderType.AttributeAndAttributeParentShortName:
                        attributeColumnHeaderText = String.Concat(attributeModel.AttributeParentName, "//", attributeModel.Name);
                        break;
                    case RSExcelAttributeColumnHeaderType.AttributeAndAttributeParentShortNameWithLocale:
                        attributeColumnHeaderText = String.Concat(attributeModel.AttributeParentName, "//", attributeModel.Name + "//" + locale);
                        break;
                    case RSExcelAttributeColumnHeaderType.AttributeLongName:
                        attributeColumnHeaderText = attributeModel.LongName;
                        break;
                    case RSExcelAttributeColumnHeaderType.AttributeShortName:
                        attributeColumnHeaderText = attributeModel.Name;
                        break;
                    default:
                        attributeColumnHeaderText = String.Concat(attributeModel.AttributeParentLongName, "//", attributeModel.LongName);
                        break;
                }
            }

            return attributeColumnHeaderText;
        }

        /// <summary>
        /// Get complex child attribute header - only complex child attribute short name or long name
        /// </summary>
        /// <param name="complexChildAttributeModel"></param>
        /// <param name="creationOptions"></param>
        /// <returns></returns>
        private static String GetComplexChildColumnHeaderText(IAttributeModel complexChildAttributeModel, RSExcelCreationOptions creationOptions)
        {
            String complexChildAttributeColumnHeaderText = String.Empty;
            if (complexChildAttributeModel != null)
            {
                switch (creationOptions.AttributeColumnHeaderType)
                {
                    case RSExcelAttributeColumnHeaderType.AttributeAndAttributeParentShortName:
                    case RSExcelAttributeColumnHeaderType.AttributeAndAttributeParentShortNameWithLocale:
                    case RSExcelAttributeColumnHeaderType.AttributeShortName:
                        complexChildAttributeColumnHeaderText = complexChildAttributeModel.Name;
                        break;
                    default:
                        complexChildAttributeColumnHeaderText = complexChildAttributeModel.LongName;
                        break;
                }
            }
            return complexChildAttributeColumnHeaderText;
        }

        /// <summary>
        /// Get category path based on option - Long name path or short name path
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="creationOptions"></param>
        /// <returns></returns>
        private static String GetCategoryPath(IEntity entity, RSExcelCreationOptions creationOptions)
        {
            String categoryPath = String.Empty;
            if (entity != null)
            {
                switch (creationOptions.CategoryPathType)
                {
                    case RSExcelCategoryPathType.LongNamePath:
                        categoryPath = entity.CategoryLongNamePath;
                        break;
                    default:
                        categoryPath = entity.CategoryPath;
                        break;
                }
            }
            return categoryPath;
        }
        
        /// <summary>
        /// Get category path based on option - Long name path or short name path
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="creationOptions"></param>
        /// <returns></returns>
        private static String GetParentExtensionCategoryPath(IEntity entity, RSExcelCreationOptions creationOptions)
        {
            String categoryPath = String.Empty;
            if (entity != null)
            {
                switch (creationOptions.CategoryPathType)
                {
                    case RSExcelCategoryPathType.LongNamePath:
                        categoryPath = entity.ParentExtensionEntityCategoryLongNamePath;
                        break;
                    default:
                        categoryPath = entity.ParentExtensionEntityCategoryPath;
                        break;
                }
            }
            return categoryPath;
        }
        
        private Boolean IsAttributeInCollection(List<IAttributeModel> sourceList, IAttributeModel searchModel)
        {
            return sourceList.Any(model => model.Id == searchModel.Id);
        }

        #endregion

        #endregion Private Methods
      
        #region Internal methods

        /// <summary>
        /// Get Template File Name
        /// </summary>
        /// <param name="profileName">The profile Name.</param>
        /// <param name="tempFileLocation"></param>
        /// <param name="creationOptions"></param>
        /// <returns>The file path.</returns>
        internal static String GetTemplateFileName(String profileName, String tempFileLocation, RSExcelCreationOptions creationOptions = null)
        {
            String filePath = String.Empty;

            if (!String.IsNullOrEmpty(profileName))
            {
                ExportTemplateBL exportTemplateBL = new ExportTemplateBL();

                // Get a template based on the profile.
                BO.Template template = exportTemplateBL.GetExportTemplateByName(profileName, new BO.CallerContext(MDMCenterApplication.PIM, MDMCenterModules.Export));

                // If there is no template available for current profile, request for default profile template.
                if (template == null)
                {
                    template = exportTemplateBL.GetExportTemplateByName(DefaultTemplateFileName, new BO.CallerContext(MDMCenterApplication.PIM, MDMCenterModules.Export));
                }

                if (template != null)
                {
                    BO.File file = new BO.File(template.Name, template.FileType, false, template.FileData);
                    filePath = String.Format("{0}\\{1}.{2}", tempFileLocation, file.Name, file.FileType);

                    if (creationOptions != null && String.IsNullOrEmpty(creationOptions.FileType))
                    {
                        creationOptions.FileType = !String.IsNullOrEmpty(template.FileType) ? file.FileType : DefaultFileType;
                    }

                    FileInfo fileInfo = new FileInfo(filePath);

                    if (fileInfo.Exists)
                    {
                        fileInfo.Delete();
                    }

                    // Create a file to write to.
                    using (FileStream fileStream = fileInfo.Create())
                    {
                        fileStream.Write(file.FileData, 0, file.FileData.Length);
                    }
                }
            }

            return filePath;
        }

        /// <summary>
        /// This method is used for to add a comment.
        /// </summary>
        /// <param name="workSheetPart">Indicates workSheetPart</param>
        /// <param name="definition">Text to display in comment box.</param>
        /// <param name="vmlDrawing">The vmlDrawing element.</param>
        /// <param name="cellReference">Indicates for Cell Reference Ex: A1 ,B1 etc</param>
        /// <param name="columnIndex">Indicates column index.</param>
        /// <param name="rowIndex">Indicates row index.</param>
        /// <param name="notes">The notes.</param>
        internal static void AddComment(WorksheetPart workSheetPart, String definition, XmlDocument vmlDrawing, String cellReference, UInt32 columnIndex, UInt32 rowIndex, String notes = null)
        {
            CreateShapeAsXmlNode(vmlDrawing, columnIndex, rowIndex);

            #region Append Comment to Existing Comments Collection

            Comments comments;
            Boolean appendComments = true;

            if (workSheetPart.WorksheetCommentsPart != null && workSheetPart.WorksheetCommentsPart.Comments != null)
            {
                comments = workSheetPart.WorksheetCommentsPart.Comments;
            }
            else
            {
                comments = new Comments();
                appendComments = false;
            }

            // We only want one Author element per Comments element
            if (workSheetPart.WorksheetCommentsPart == null || workSheetPart.WorksheetCommentsPart.Comments == null)
            {
                Authors authors = new Authors();
                Author author = new Author();
                author.Text = "Riversand";
                authors.Append(author);
                comments.Append(authors);
            }

            CommentList commentList;
            Boolean appendCommentList = true;

            if (workSheetPart.WorksheetCommentsPart != null && workSheetPart.WorksheetCommentsPart.Comments != null &&
                workSheetPart.WorksheetCommentsPart.Comments.Descendants<CommentList>().SingleOrDefault() != null)
            {
                commentList = workSheetPart.WorksheetCommentsPart.Comments.Descendants<CommentList>().Single();
            }
            else
            {
                commentList = new CommentList();
                appendCommentList = false;
            }

            Comment comment = new Comment
            {
                Reference = cellReference,
                AuthorId = 0U
            };

            CommentText commentTextElement = new CommentText();

            Run run = GetDefaultTextRunElement();

            Text text = new Text { Text = definition, Space = SpaceProcessingModeValues.Preserve };

            run.Append(text);

            commentTextElement.Append(run);

            if (!String.IsNullOrWhiteSpace(notes))
            {
                Run notesRun = GetDefaultTextRunElement(false);
                Text notesText = new Text { Text = notes, Space = SpaceProcessingModeValues.Preserve };
                notesRun.Append(notesText);
                commentTextElement.Append(notesRun);
            }

            comment.Append(commentTextElement);
            commentList.Append(comment);

            // Only append the Comment List if this is the first time adding a comment
            if (!appendCommentList)
            {
                comments.Append(commentList);
            }

            // Only append the Comments if this is the first time adding Comments
            if (!appendComments)
            {
                workSheetPart.AddNewPart<WorksheetCommentsPart>();
                workSheetPart.WorksheetCommentsPart.Comments = comments;
            }

            #endregion
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="headerRow"></param>
        /// <param name="stylesheet"></param>
        /// <param name="worksheetStyles"></param>
        internal static void UpdateMetadataAttributesStyle(Row headerRow, Stylesheet stylesheet, WorksheetStyles worksheetStyles)
        {
            Cell headerFirstCell = headerRow.Elements<Cell>().First();
            UInt32 cellFormatIndex = UInt32Value.ToUInt32(headerFirstCell.StyleIndex);

            CellFormat targetCellFormat = stylesheet.CellFormats.ChildElements.GetItem((Int32)cellFormatIndex) as CellFormat;
            if (targetCellFormat != null)
            {
                Font metadataAttributeFont = new Font
                {
                    FontSize = new FontSize { Val = worksheetStyles.MetadataAttributeStyle.FontSizeVal },
                    Color = new Color { Rgb = worksheetStyles.MetadataAttributeStyle.FontRgb },
                    FontName = new FontName { Val = "Calibri" },
                    FontScheme = new FontScheme { Val = FontSchemeValues.Minor }
                };

                if (worksheetStyles.MetadataAttributeStyle.IsBold)
                {
                    metadataAttributeFont.Bold = new Bold();
                }

                if (worksheetStyles.MetadataAttributeStyle.IsItalic)
                {
                    metadataAttributeFont.Italic = new Italic();
                }

                stylesheet.Fonts.Append(metadataAttributeFont);
                stylesheet.Fonts.Count = UInt32Value.FromUInt32((UInt32)stylesheet.Fonts.ChildElements.Count);

                Fill metadataAttributeBackGroundColorFill = new Fill();
                PatternFill patternFill = new PatternFill
                {
                    PatternType = PatternValues.Solid,
                    ForegroundColor = new ForegroundColor { Rgb = worksheetStyles.MetadataAttributeStyle.BackgroundRgb },
                    BackgroundColor = new BackgroundColor { Indexed = 64U }
                };
                metadataAttributeBackGroundColorFill.Append(patternFill);
                stylesheet.Fills.Append(metadataAttributeBackGroundColorFill);
                stylesheet.Fills.Count = UInt32Value.FromUInt32((UInt32)stylesheet.Fills.ChildElements.Count);

                UInt32 fontsCount = (UInt32)stylesheet.Fonts.ChildElements.Count;
                UInt32 fillsCount = (UInt32)stylesheet.Fills.ChildElements.Count;

                targetCellFormat.FillId = fillsCount - 1;
                targetCellFormat.FontId = fontsCount - 1;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="workSheetPart"></param>
        /// <returns></returns>
        internal static XmlDocument ReadVMLDrawing(WorksheetPart workSheetPart)
        {
            VmlDrawingPart vmlDrawingPart = workSheetPart.VmlDrawingParts.FirstOrDefault();
            XmlDocument xmlDocument = new XmlDocument();

            if (vmlDrawingPart != null)
            {
                Byte[] bytes;
                using (Stream stream = vmlDrawingPart.GetStream(FileMode.Open, FileAccess.Read))
                {
                    bytes = new Byte[stream.Length];

                    stream.Read(bytes, 0, bytes.Length);
                }

                String vmlDrawingAsXml = Encoding.UTF8.GetString(bytes, 0, bytes.Length);

                if (!String.IsNullOrWhiteSpace(vmlDrawingAsXml))
                {
                    xmlDocument.LoadXml(vmlDrawingAsXml);
                }
            }

            return xmlDocument;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="workSheetPart"></param>
        /// <param name="vmlDrawing"></param>
        internal static void WriteVMLDrawing(WorksheetPart workSheetPart, XmlDocument vmlDrawing)
        {
            VmlDrawingPart vmlDrawingPart = workSheetPart.VmlDrawingParts.FirstOrDefault();

            if (vmlDrawingPart == null)
            {
                workSheetPart.AddNewPart<VmlDrawingPart>();
                vmlDrawingPart = workSheetPart.VmlDrawingParts.FirstOrDefault();
            }

            using (XmlTextWriter writer = new XmlTextWriter(vmlDrawingPart.GetStream(FileMode.Create), Encoding.UTF8))
            {
                writer.WriteRaw(vmlDrawing.OuterXml);
            }

            // We only want one legacy drawing element per worksheet for comments
            if (workSheetPart.Worksheet.Descendants<LegacyDrawing>().SingleOrDefault() == null)
            {
                String vmlPartId = workSheetPart.GetIdOfPart(vmlDrawingPart);
                LegacyDrawing legacyDrawing = new LegacyDrawing { Id = vmlPartId };
                workSheetPart.Worksheet.Append(legacyDrawing);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isHidden"></param>
        /// <returns></returns>
        private Boolean IsAttributeQualifiedForExport(Boolean isHidden)
        {
            Boolean isAttributeQualifiedForExport = true;

            if (!_showHiddenAttributesOnExport && isHidden)
            {
                isAttributeQualifiedForExport = false;
            }

            return isAttributeQualifiedForExport;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeModel"></param>
        /// <returns></returns>
        private Boolean AreAllComplexChildAttributesHidden(IAttributeModel iAttributeModel)
        {
            Boolean areAllComplexChildAttributesHidden = true;

            if (iAttributeModel != null && iAttributeModel.IsComplex && iAttributeModel.GetChildAttributeModels() != null)
            {
                foreach (BO.AttributeModel childAttributeModel in iAttributeModel.GetChildAttributeModels())
                {
                    if (!childAttributeModel.IsHidden)
                    {
                        areAllComplexChildAttributesHidden = false;
                        break;
                    }
                }
            }

            return areAllComplexChildAttributesHidden;
        }

        #endregion

        #endregion Methods
    }

    /// <summary>
    /// 
    /// </summary>
    public class EntityInfo
    {
        public String InfoType = String.Empty;

        public String Name = String.Empty;

        public String Value = String.Empty;

        public Collection<String> MessageCodes = new Collection<String>();
    }
}