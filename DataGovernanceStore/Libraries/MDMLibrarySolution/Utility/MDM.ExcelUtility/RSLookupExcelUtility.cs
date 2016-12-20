using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml;

using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using OpenXmlSpreadSheet = DocumentFormat.OpenXml.Spreadsheet;

namespace MDM.ExcelUtility
{
    using MDM.BusinessObjects;
    using MDM.Core;
    using MDM.Interfaces;
    using BOCell = MDM.BusinessObjects.Cell;
    using BORow = MDM.BusinessObjects.Row;

    /// <summary>
    /// Represents RsLookup Excel Utilities. 
    /// This class will provide the utility method and properties for RS lookup Excel file construction. 
    /// </summary>
    public sealed class RSLookupExcelUtility
    {
        #region Fields

        private const String _lookupHeaderTableName = "LookupTableName";

        private UInt32 _uniqueColumnIndex;
        private UInt32 _idColumnIndex;
        private UInt32 _defaultColumnIndex;
        private const String _idColumnFroregroundColor = "FFA6A6A6";    //Future this values we have to read it from lookup style sheet.
        private const String _uniqueColumnFroregroundColor = "FFFD0002";
        private const String _defaultColumnFroregroundColor = "FF5B9BD5";
        private const String _mandatoryColumnSymbol = " *";
        private Collection<String> _mandatoryUniqueColumns = new Collection<String>(); // This collections are used to append asterisk '*' to column name which marked as not null.
        private Collection<String> _mandatoryNonUniqueColumns = new Collection<String>();

        #endregion

        #region Constructors

        #endregion

        #region Properties

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Generate RSLookupExcel file based on requested lookups
        /// </summary>
        /// <param name="lookups">Indicates the lookup collection</param>
        /// <param name="lookupNamesAndSheetNames">Indicates the lookup table and sheet names as dictionary</param>
        /// <param name="lookupExportContext">Indicates the lookup export context</param>
        /// <param name="filepath">Indicates the file path</param>
        public void GenerateRSExcel(LookupCollection lookups, Dictionary<String, String> lookupNamesAndSheetNames, LookupExportContext lookupExportContext, String filepath)
        {
            #region Logical Flow
            /*
             * Logical Flow
             * 1. Create a RS Lookup Excel file based on the default template
             * 2. Fill the meta data sheet details
             * 3. Create Excel work sheet per lookup
             * 4. Fill the lookup details into the sheet (based on the lookup)
             */

            #endregion

            #region Input Validation

            //if (lookups == null || lookups.Count < 1 || lookupExportContext == null || lookupExportContext.LocaleList == null
            //    || lookupExportContext.LocaleList.Count < 1 || lookupNamesAndSheetNames == null || lookupNamesAndSheetNames.Count < 1)
            //{
            //    //If one of the object do not have proper information then no point of creating excel file.
            //    //So return it back. But this checks already validated at RSLookupExport formatter. Since this as a public method the validation added here.
            //    return;
            //}

            #endregion

            SpreadsheetDocument document = SpreadsheetDocument.Open(filepath, true);

            if (document != null)
            {
                WorkbookPart workbookPart = document.WorkbookPart;

                if (workbookPart != null)
                {
                    Worksheet workSheet = OpenSpreadsheetUtility.GetWorksheet(workbookPart, RSExcelConstants.MetadataSheetName);

                    //Prepare meta data sheet
                    PopulateMetaDataSheet(workbookPart, lookupNamesAndSheetNames);

                    //Populate other lookup sheets
                    PopulateLookupSheets(workbookPart, lookups, lookupNamesAndSheetNames, lookupExportContext);
                }

                document.Close();
            }
        }

        #endregion

        #region Private Methods

        #region Lookup Helper methods

        private void PopulateLookupSheets(WorkbookPart workbookPart, LookupCollection lookups, Dictionary<String, String> lookupNamesAndSheetNames, LookupExportContext lookupExportContext)
        {
            #region Logical Flow
            /*
             * 1. Create excel sheet per lookup
             * 2. Prepare lookup hearder row (column header mapping)
             * -- Map created as Tuple object. Tuple items are ColumnIndex ,actual columnName, Id,unique/locale,headerName
             * --Reason here for the map is, in a single work sheet we have to populate differnt locale's lookup records.
             * --And fill the header accordingly.
             * 3. Fill the lookup records 
             * 
             */

            #endregion

            if (workbookPart != null)
            {
                WorksheetPart newWorksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                newWorksheetPart.Worksheet = new Worksheet(new SheetData());

                XmlDocument vmlDrawing = RSExcelUtility.ReadVMLDrawing(newWorksheetPart);

                //create worksheet for each lookup
                foreach (KeyValuePair<String, String> lookupNameMap in lookupNamesAndSheetNames)
                {
                    String lookupName = lookupNameMap.Key;
                    String lookupSheetName = lookupNameMap.Value;

                    //Add new work sheet into work book part
                    OpenSpreadsheetUtility.AddWorksheet(workbookPart, lookupSheetName);

                    Lookup lookup = lookups.GetLookup(lookupName, lookupExportContext.LocaleList.FirstOrDefault());

                    Collection<Tuple<UInt32, String, String, String>> lookupHeaderMap = BuildLookupColumnTuple(lookup, lookupExportContext);
                    //Sample Tuple object values are <ColumnIndex,actual columnName,Id/Isunique/locale,headerName>

                    if (workbookPart != null)
                    {
                        Worksheet workSheet = OpenSpreadsheetUtility.GetWorksheet(workbookPart, lookupSheetName);

                        if (workSheet != null)
                        {
                            SheetData sheetData = workSheet.GetFirstChild<SheetData>();

                            if (sheetData != null)
                            {
                                //Populate style Sheet
                                this.PrepareLookupStyleSheet(workbookPart, workSheet, sheetData);

                                //Fill the header column
                                this.PopulateLookupSheetHeader(workbookPart, vmlDrawing, sheetData, lookupHeaderMap, lookupSheetName);
                                //Fill Lookup values
                                FillLookupData(sheetData, lookupName, lookups, lookupExportContext, lookupHeaderMap);
                            }
                        }
                    }
                }
                RSExcelUtility.WriteVMLDrawing(newWorksheetPart, vmlDrawing);
            }
        }

        private Collection<Tuple<UInt32, String, String, String>> BuildLookupColumnTuple(Lookup lookup, LookupExportContext lookupExportContext)
        {
            #region Logical Flow

            /*
             * 1. Create the lookup Header row columns
             * 2. If 'Id' column present that will be the first columns
             * 3. Following to 'Id' column unique columns will be formed.
             * 4. Then the other columns will be created based on the LookupExport Group by order.
             * 
             */

            #endregion

            Collection<Tuple<UInt32, String, String, String>> lookupColumnMap = new Collection<Tuple<UInt32, String, String, String>>();
            //Tuple sample model <ColumnIndex,actual columnName,id/unique/locale,headerName>

            if (lookup != null)
            {
                Collection<String> lookupRelationshipsColumnNames = new Collection<String>();
                LookupRelationshipCollection lookupRelationships = lookup.LookupRelationships;

                //Prepare collection of relationship lookup column names.
                //The lookup relationship columns should not be sent to exported file.
                if (lookupRelationships != null && lookupRelationships.Count > 0)
                {
                    String columnName = String.Empty;
                    foreach (LookupRelationship lookupRelationship in lookupRelationships)
                    {
                        columnName = String.Format("{0}_{1}_DisplayFormat", lookupRelationship.RefTableName,lookupRelationship.ColumnName);
                        lookupRelationshipsColumnNames.Add(columnName);
                    }
                }

                Collection<String> uniqueColumns = new Collection<String>();


                Collection<String> nonUniqueColumns = new Collection<String>();
                UInt32 counter = 1;

                //Get all the columns which are available in the lookup
                IColumnCollection columns = lookup.GetColumns();

                #region Identify and Prepare Columns based on the uniqueness

                foreach (IColumn column in columns)
                {
                    //Skip sending lookup relationships column names to the exported file.
                    if (lookupRelationshipsColumnNames.Count > 0 && lookupRelationshipsColumnNames.Contains(column.Name))
                    {
                        continue;
                    }
                    if (column.IsUnique)
                    {
                        uniqueColumns.Add(column.Name);

                        if (!column.Nullable)
                        {
                            _mandatoryUniqueColumns.Add(column.Name);
                        }
                    }
                    else if (column.Name.ToLower() == "id") //Id column always be in first so add it as first column
                    {
                        lookupColumnMap.Add(new Tuple<UInt32, String, String, String>(counter, column.Name, "id", column.Name));
                        counter++;
                    }
                    else
                    {
                        nonUniqueColumns.Add(column.Name);

                        if (!column.Nullable)
                        {
                            _mandatoryNonUniqueColumns.Add(column.Name);
                        }
                    }
                }

                foreach (String columnName in uniqueColumns)
                {
                    lookupColumnMap.Add(new Tuple<UInt32, String, String, String>(counter, columnName, "unique", _mandatoryUniqueColumns.Contains(columnName) ? String.Format("{0}{1}", columnName, _mandatoryColumnSymbol) : columnName));
                    counter++;
                }

                #endregion

                #region Group By Column

                if (lookupExportContext.GroupBy == LookupExportGroupOrder.Locale)
                {
                    foreach (LocaleEnum locale in lookupExportContext.LocaleList)
                    {
                        foreach (String columnName in nonUniqueColumns)
                        {
                            // Gets column name with '*' as suffix, if the column is marked as not null.
                            String columnNameWithSymbol = GetLookupColumnNameWithSymbol(columnName, locale);
                            
                            lookupColumnMap.Add(new Tuple<UInt32, String, String, String>(counter, columnName, locale.ToString(), columnNameWithSymbol));
                            counter++;
                        }
                    }
                }
                else 	//if there is no order specified still we have to prepare the lookup columns
                {
                    foreach (String columnName in nonUniqueColumns)
                    {
                        foreach (LocaleEnum locale in lookupExportContext.LocaleList)
                        {
                            // Gets column name with '*' as suffix, if the column is marked as not null.
                            String columnNameWithSymbol = GetLookupColumnNameWithSymbol(columnName, locale);

                            lookupColumnMap.Add(new Tuple<UInt32, String, String, String>(counter, columnName, locale.ToString(), columnNameWithSymbol));
                            counter++;
                        }
                    }
                }
            }

                #endregion

            return lookupColumnMap;
        }

        private void PopulateMetaDataSheet(WorkbookPart workbookPart, Dictionary<String, String> lookupNamesAndSheetNames)
        {
            if (workbookPart != null)
            {
                Worksheet workSheet = OpenSpreadsheetUtility.GetWorksheet(workbookPart, RSExcelConstants.MetadataSheetName);

                SheetData sheetData = workSheet.GetFirstChild<SheetData>();

                UInt32 startingIndex = GetStartingIndexByText(workbookPart, sheetData, 1, _lookupHeaderTableName);

                if (startingIndex > 0)
                {
                    startingIndex++;

                    foreach (KeyValuePair<String, String> keyValue in lookupNamesAndSheetNames)
                    {
                        UpdateCellWithValue(sheetData, 1, startingIndex, keyValue.Key);
                        UpdateCellWithValue(sheetData, 2, startingIndex, keyValue.Value);
                        UpdateCellWithValue(sheetData, 3, startingIndex, "Yes");
                        startingIndex++;
                    }
                }
            }
        }

        private void PopulateLookupSheetHeader(WorkbookPart workbookPart, XmlDocument vmlDrawing, SheetData sheetData, Collection<Tuple<UInt32, String, String, String>> lookupHeaderMap, String sheetName)
        {
            if (sheetData != null)
            {
                //Tuple object sample <ColumnIndex,actual columnName,Isunique/locale,headerName>
                foreach (Tuple<UInt32, String, String, String> map in lookupHeaderMap)
                {
                    UInt32? styleIndex = null;
                    String comments = String.Empty;

                    if (map.Item3 == "unique" && _uniqueColumnIndex != 0)
                    {
                        styleIndex = _uniqueColumnIndex;
                        comments = "Unique Key";
                    }
                    else if (map.Item3 == "id" && _idColumnIndex != 0)
                    {
                        styleIndex = _idColumnIndex;
                    }
                    else if (_defaultColumnIndex != 0)
                    {
                        styleIndex = _defaultColumnIndex;
                    }

                    UpdateCellWithValue(workbookPart, vmlDrawing, sheetData, map.Item1, 1, map.Item4, styleIndex, comments);
                }
            }
        }

        private void FillLookupData(SheetData sheetData, String lookupName, LookupCollection lookups, LookupExportContext lookupExportContext, Collection<Tuple<UInt32, String, String, String>> lookupHeaderMap)
        {
            #region Logical Flow

            /*
             * 1. Each lookup we have to fill the values in all requested locale
             * 2. So loop all locale, each local fill the cell value
             * 3. Fill unique column and id column only once
             * 4. If other than SDL column value is empty for a record do not populate
             * 5. Identifiying column index and column name use the lookup header map Tuple object
             * 
             */

            #endregion

            Boolean isUniqueColumnPopulated = false;
            //Tuple object <ColumnIndex,actual columnName,Isunique/locale,headerName>

            foreach (LocaleEnum locale in lookupExportContext.LocaleList)
            {
                Lookup lookup = lookups.GetLookup(lookupName, locale);

                if (lookup != null && lookup.Rows != null && lookup.Rows.Count > 0)
                {
                    var lookupRows = lookup.Rows.OrderBy(row => row.Id);

                    UInt32 rowIndex = 2; //Header already populated so start with 2nd row
                    foreach (BORow row in lookupRows)
                    {
                        foreach (Tuple<UInt32, String, String, String> map in lookupHeaderMap)
                        {
                            Object cellValue = null;

                            if ((map.Item3 == "unique" || map.Item3 == "id") && isUniqueColumnPopulated)
                            {
                                //Already unique column values and Id column populated so no need to populate again.
                                continue;
                            }

                            if (map.Item3 == "unique" || map.Item3 == "id")
                            {
                                BOCell cell = row.GetCell(map.Item2);
                                if (cell != null && cell.Value != null)
                                {
                                    cellValue = cell.Value;
                                }
                            }
                            else if (map.Item3.ToString() == locale.ToString())
                            {
                                if (row.IsSystemLocaleRow == false) //Other than SDL records the cell value is empty then no need to fill the value.
                                {
                                    BOCell cell = row.GetCell(map.Item2);
                                    if (cell != null && cell.Value != null)
                                    {
                                        cellValue = cell.Value;
                                    }
                                }
                            }

                            if (cellValue != null)
                            {
                                UpdateCellWithValue(sheetData, map.Item1, rowIndex, cellValue, lookupHeaderMap.Count);
                            }
                        }
                        //Increment the row counter
                        rowIndex++;
                    }
                    isUniqueColumnPopulated = true;
                }
            }
        }

        private void PrepareLookupStyleSheet(WorkbookPart workbookPart, Worksheet workSheet, SheetData sheetData)
        {
            Columns columns = null;

            if (workSheet.WorksheetPart.Worksheet.Descendants<Columns>().SingleOrDefault() != null)
            {
                columns = workSheet.WorksheetPart.Worksheet.Descendants<Columns>().Single();
            }

            DocumentFormat.OpenXml.Spreadsheet.Column column = new DocumentFormat.OpenXml.Spreadsheet.Column
            {
                Min = 1,
                Max = 1,
                Width = 32,
                CustomWidth = true,
                BestFit = true
            };

            if (columns != null)
            {
                columns.Append(column);
            }

            // Get style index for header row
            OpenXmlSpreadSheet.Cell entitySheetHeaderCell = OpenSpreadsheetUtility.GetDataCell(sheetData, 1, 1);
            UInt32 _headerCellStyleIndex = OpenSpreadsheetUtility.GetCellStyleIndex(entitySheetHeaderCell);
            UInt32 rowIndex = 1;

            OpenXmlSpreadSheet.Row headerRow = OpenSpreadsheetUtility.GetRow(sheetData, rowIndex);
            List<Int32> sharedStringIds = headerRow.Elements<OpenXmlSpreadSheet.Cell>().Select(cell => ValueTypeHelper.Int32TryParse(cell.InnerText,0)).ToList();
            List<String> metadataAttributes = sharedStringIds.Select(i => OpenSpreadsheetUtility.GetSharedStringItemById(workbookPart, i).InnerText).ToList();

            Stylesheet stylesheet = workbookPart.WorkbookStylesPart.Stylesheet;

            this.ApplyStyleSheet(stylesheet);
        }

        private String GetLookupColumnNameWithSymbol(String columnName, LocaleEnum locale)
        {
            String columnNameWithSymbol = String.Format("{0}//{1}", columnName, locale.ToString());

            if (_mandatoryNonUniqueColumns.Contains(columnName))
            {
                columnNameWithSymbol = String.Format("{0}//{1}{2}", columnName, locale.ToString(), _mandatoryColumnSymbol);
            }

            return columnNameWithSymbol;
        }

        #endregion

        #region Open Xml Helper Methods

        private void UpdateCellWithValue(SheetData sheetData, UInt32 columnIndex, UInt32 rowIndex, Object value, Int32 noOfCells, UInt32? styleIndex = null)
        {
            OpenXmlSpreadSheet.Row row = OpenSpreadsheetUtility.GetRow(sheetData, rowIndex);

            if (!row.HasChildren)    
            {
                //Check any cells are there in the row. If not create cells.
                //Because we have to insert/append the cells as sequential order.
                for (UInt32 index = 1; noOfCells >= index; index++)
                {
                    CreateTextCell(row, index, CellValues.InlineString, styleIndex);
                }
            }

            //Find the cell based on the index and fill the cell value.
            OpenXmlSpreadSheet.Cell cell = row.Elements<OpenXmlSpreadSheet.Cell>().ElementAt((Int32)columnIndex - 1);
            FillCellValue(cell, value);
        }

        private void UpdateCellWithValue(SheetData sheetData, UInt32 columnIndex, UInt32 rowIndex, object value, UInt32? styleIndex = null)
        {
            OpenXmlSpreadSheet.Row row = OpenSpreadsheetUtility.GetRow(sheetData, rowIndex);
            OpenXmlSpreadSheet.Cell cell = OpenSpreadsheetUtility.CreateDataCell(columnIndex, rowIndex, CellValues.InlineString, value, styleIndex);
            Int32 cellsCount = row.Elements<OpenXmlSpreadSheet.Cell>().Count();

            row.InsertAt(cell, cellsCount);
        }

        private void UpdateCellWithValue(WorkbookPart workbookPart, XmlDocument vmlDrawing, SheetData sheetData, UInt32 columnIndex, UInt32 rowIndex, object value, UInt32? styleIndex = null, String comments = "")
        {
            OpenXmlSpreadSheet.Row row = OpenSpreadsheetUtility.GetRow(sheetData, rowIndex);
            OpenXmlSpreadSheet.Cell cell = OpenSpreadsheetUtility.CreateDataCell(columnIndex, rowIndex, CellValues.InlineString, value, styleIndex);
            Int32 cellsCount = row.Elements<OpenXmlSpreadSheet.Cell>().Count();

            row.InsertAt(cell, cellsCount);

            if (!String.IsNullOrWhiteSpace(comments))
            {
                this.AddComments(workbookPart, vmlDrawing, cell, columnIndex, row, comments);
            }
        }

        private void AddComments(WorkbookPart workbookPart, XmlDocument vmlDrawing, OpenXmlSpreadSheet.Cell cell, UInt32 columnIndex, OpenXmlSpreadSheet.Row row, String comments)
        {
            WorksheetPart newWorksheetPart = workbookPart.AddNewPart<WorksheetPart>();
            newWorksheetPart.Worksheet = new Worksheet(new SheetData());

            vmlDrawing = RSExcelUtility.ReadVMLDrawing(newWorksheetPart);

            RSExcelUtility.AddComment(newWorksheetPart, comments, vmlDrawing, cell.CellReference.Value, columnIndex, row.RowIndex.Value - 1);
        }

        private static UInt32 GetStartingIndexByText(WorkbookPart workbookPart, SheetData sheetData, UInt32 columnIndex, String headerText)
        {
            UInt32 startingIndex = 0;
            Int32 sharedStringId = 0;
            UInt32 maxRowToSeach = 30; //First 30 rows. We can not go and search the whole sheet to get the header. Search the first 30 rows.

            for (UInt32 i = 1; i < maxRowToSeach; i++)
            {
                OpenXmlSpreadSheet.Cell captionCell = OpenSpreadsheetUtility.GetDataCell(sheetData, columnIndex, i);

                if (captionCell != null && Int32.TryParse(captionCell.InnerText, out sharedStringId))
                {
                    SharedStringItem sharedStringItem = OpenSpreadsheetUtility.GetSharedStringItemById(workbookPart, sharedStringId);

                    if (sharedStringItem.InnerText == headerText)
                    {
                        startingIndex = i;
                        break;
                    }
                }
            }

            return startingIndex;
        }

        #region Apply Styles

        private void ApplyStyleSheet(Stylesheet styleSheet)
        {
            _idColumnIndex = GetCellFormatCount(styleSheet);
            this.ApplyStyleSheet(styleSheet, _idColumnFroregroundColor);

            _uniqueColumnIndex = GetCellFormatCount(styleSheet);
            this.ApplyStyleSheet(styleSheet, _uniqueColumnFroregroundColor);

            _defaultColumnIndex = GetCellFormatCount(styleSheet);
            this.ApplyStyleSheet(styleSheet, _defaultColumnFroregroundColor);
        }

        private void ApplyStyleSheet(Stylesheet stylesheet, String fourGroundColor)
        {
            UInt32 fontCount = (uint)stylesheet.Fonts.ChildElements.Count;
            UInt32 fillCount = (uint)stylesheet.Fills.ChildElements.Count;

            #region Prepare Font

            Font columnFont = new Font
            {
                FontSize = new FontSize() { Val = 11 },
                Color = new Color()
                {
                    Theme = (UInt32Value)0U,
                },

                FontName = new FontName() { Val = "Calibri" },
                FontScheme = new FontScheme { Val = FontSchemeValues.Minor },
                Bold = new Bold()
            };

            stylesheet.Fonts.Append(columnFont);

            stylesheet.Fonts.Count = UInt32Value.FromUInt32((UInt32)stylesheet.Fonts.ChildElements.Count);

            #endregion Prepare Font

            #region Prepare PatternFill

            Fill columnBackGroundColorFill = new Fill();
            PatternFill columnpatternFill = new PatternFill
            {
                PatternType = PatternValues.Solid,
                ForegroundColor = new ForegroundColor()
                {
                    Rgb = new HexBinaryValue(fourGroundColor)
                    // Theme = (UInt32Value)4U 
                },
                BackgroundColor = new BackgroundColor { Indexed = 64U }
            };
            columnBackGroundColorFill.Append(columnpatternFill);

            stylesheet.Fills.Append(columnBackGroundColorFill);

            stylesheet.Fills.Count = UInt32Value.FromUInt32((uint)stylesheet.Fills.ChildElements.Count);

            #endregion

            #region Prepare Cell Format

            CellFormat columnCellFormat = new CellFormat
            {
                NumberFormatId = 49U,
                FontId = fontCount,
                FillId = UInt32Value.FromUInt32(fillCount),
                ApplyNumberFormat = true,
                ApplyFont = true,
                ApplyFill = true,
                ApplyBorder = false,
                ApplyAlignment = true,
                Alignment = new Alignment
                {
                    Horizontal = HorizontalAlignmentValues.Center,
                    Vertical = VerticalAlignmentValues.Center,
                    WrapText = true
                },
            };
            stylesheet.CellFormats.Append(columnCellFormat);

            #endregion Prepare Cell Format

            stylesheet.CellFormats.Count = UInt32Value.FromUInt32((UInt32)stylesheet.CellFormats.ChildElements.Count);
        }

        private UInt32 GetCellFormatCount(Stylesheet styleSheet)
        {
            UInt32 cellFormatCount = 0;

            if (styleSheet != null)
            {
                cellFormatCount = (UInt32)styleSheet.CellFormats.ChildElements.Count;
            }

            return cellFormatCount;
        }

        #endregion Apply Styles

        private OpenXmlSpreadSheet.Cell CreateTextCell(OpenXmlSpreadSheet.Row row, UInt32 columnIndex, CellValues cellDataType, UInt32? styleIndex = null)
        {
            UInt32 rowIndex = row.RowIndex;
            OpenXmlSpreadSheet.Cell cell = CreateTextCell(columnIndex, rowIndex, cellDataType,  styleIndex);
            row.AppendChild<OpenXmlSpreadSheet.Cell>(cell);

            return cell;
        }

        private OpenXmlSpreadSheet.Cell CreateTextCell(UInt32 columnIndex, UInt32 rowIndex, CellValues cellDataType, UInt32? styleIndex = null)
        {
            OpenXmlSpreadSheet.Cell cell = new OpenXmlSpreadSheet.Cell
            {
                CellReference = OpenSpreadsheetUtility.GetColumnName(columnIndex) + rowIndex,
                DataType = cellDataType
            };

            switch (cellDataType)   
            {
                case CellValues.InlineString:
                    InlineString inlineString = new InlineString();
                    Text text = new Text();

                    text.Space = SpaceProcessingModeValues.Preserve;
                    inlineString.AppendChild(text);
                    cell.AppendChild(inlineString);

                    if (styleIndex != null)
                    {
                        cell.StyleIndex = styleIndex;
                    }

                    break;
            }

            return cell;
        }

        private void FillCellValue(OpenXmlSpreadSheet.Cell cell, Object value)
        {
            Text text = new Text();
            text.Text = value != null ? value.ToString() : String.Empty;
            cell.InlineString.Text = text;
        }

        #endregion  Open Xml Helper Methods

        #endregion

        #endregion
    }
}
