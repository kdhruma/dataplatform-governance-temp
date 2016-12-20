using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace MDM.ExcelUtility
{
    using Utility;

    /// <summary>
    /// Open Spreadsheet Utility class
    /// </summary>
    public sealed class OpenSpreadsheetUtility
    {
        #region Public Static Methods

        /// <summary>
        /// Appends Row with number cell
        /// </summary>
        /// <param name="row">The row</param>
        /// <param name="columnIndex">The column index</param>
        /// <param name="value">The value of the cell</param>
        /// <param name="styleIndex">The style Index.</param>
        public static void AppendRowWithNumberCell(Row row, uint columnIndex, object value, uint? styleIndex = null)
        {
            Cell cell = CreateNumberCell(columnIndex, row.RowIndex, value, styleIndex);
            row.AppendChild(cell);
        }

        /// <summary>
        /// Appends Row with number cell
        /// </summary>
        /// <param name="row">The row</param>
        /// <param name="columnIndex">The column index</param>
        /// <param name="value">The value of the cell</param>
        /// <param name="styleIndex">The style Index.</param>
        public static Cell CreateSAXNumberCell(Row row, uint columnIndex, object value, uint? styleIndex = null)
        {
            Cell cell = CreateNumberCell(columnIndex, row.RowIndex, value, styleIndex);
            return cell;
        }


        /// <summary>
        /// Appends Row with text cell
        /// </summary>
        /// <param name="row">The row</param>
        /// <param name="columnIndex">The column index</param>
        /// <param name="value">The value of the cell</param>
        /// <param name="styleIndex">The style Index.</param>
        public static void AppendRowWithTextCell(Row row, uint columnIndex, object value, uint? styleIndex = null)
        {
            AppendRowWithTextCell(row, columnIndex, value, SpaceProcessingModeValues.Default, styleIndex);
        }

        /// <summary>
        /// Append row with formulated text cell
        /// </summary>
        /// <param name="row">Indicates: index of row</param>
        /// <param name="columnIndex">Indicates: index of column</param>
        /// <param name="value">Indicates: value of cell</param>
        /// <param name="formula">Indicates: formula for cell</param>
        /// <param name="styleIndex">Indicates: index of style</param>
        public static void AppendRowWithFormulatedTextCell(Row row, uint columnIndex, object value, String formula, uint? styleIndex = null)
        {
            Cell cell = CreateTextCellWithFormula(columnIndex, row.RowIndex, value, formula);
            row.AppendChild(cell);
        }

        /// <summary>
        /// Appends Row with text cell
        /// </summary>
        /// <param name="row">The row</param>
        /// <param name="columnIndex">The column index</param>
        /// <param name="value">The value of the cell</param>
        /// <param name="preserveSpaces">The preserve Spaces value.</param>
        /// <param name="styleIndex">The style Index.</param>
        public static Cell CreateSAXTextCell(Row row, uint columnIndex, object value, SpaceProcessingModeValues preserveSpaces, uint? styleIndex = null)
        {
            Cell cell = CreateTextCell(columnIndex, row.RowIndex, value, preserveSpaces, styleIndex);
            return cell;
        }

        /// <summary>
        /// Appends Row with text cell
        /// </summary>
        /// <param name="row">The row</param>
        /// <param name="columnIndex">The column index</param>
        /// <param name="value">The value of the cell</param>
        /// <param name="preserveSpaces">The preserve Spaces value.</param>
        /// <param name="styleIndex">The style Index.</param>
        public static void AppendRowWithTextCell(Row row, uint columnIndex, object value, SpaceProcessingModeValues preserveSpaces, uint? styleIndex = null)
        {
            Cell cell = CreateTextCell(columnIndex, row.RowIndex, value, preserveSpaces, styleIndex);
            row.AppendChild(cell);
        }

        /// <summary>
        /// Adds inline string text to the cell
        /// </summary>
        /// <param name="cell">The target cell</param>
        /// <param name="value">The value to add</param>
        public static void AddInlineStringToCell(Cell cell, object value)
        {
            InlineString inlineString = new InlineString();
            Text t = new Text { Text = value.ToString() };
            inlineString.AppendChild(t);
            cell.AppendChild(inlineString);
        }

        /// <summary>
        /// Get Shared String ItemBy Id
        /// </summary>
        /// <param name="sharedStringTable">The shared string table</param>
        /// <param name="id">The id parameter</param>
        /// <returns>The Shared String Item</returns>
        public static SharedStringItem GetSharedStringItemById(SharedStringTable sharedStringTable, int id)
        {
            return sharedStringTable.Elements<SharedStringItem>().ElementAt(id);
        }

        /// <summary>
        /// Get Shared String ItemBy Id
        /// </summary>
        /// <param name="workbookPart">The workbook Part.</param>
        /// <param name="id">The id parameter</param>
        /// <returns>The Shared String Item</returns>
        public static SharedStringItem GetSharedStringItemById(WorkbookPart workbookPart, int id)
        {
            SharedStringTable sharedStringTable = workbookPart.SharedStringTablePart.SharedStringTable;
            return GetSharedStringItemById(sharedStringTable, id);
        }

        /// <summary>
        /// Add Worksheet
        /// </summary>
        /// <param name="workbookPart">The workbook part </param>
        /// <param name="worksheetName">The worksheet name. </param>
        /// <param name="state">The state.</param>
        public static void AddWorksheet(WorkbookPart workbookPart, String worksheetName, SheetStateValues state = SheetStateValues.Visible)
        {
            Worksheet workSheet = GetWorksheet(workbookPart, worksheetName);

            if (workSheet == null)
            {
                WorksheetPart newWorksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                newWorksheetPart.Worksheet = new Worksheet(new SheetData());

                Sheets sheets = workbookPart.Workbook.GetFirstChild<Sheets>();
                String relationshipId = workbookPart.GetIdOfPart(newWorksheetPart);

                uint sheetId = 1;
                if (sheets.Elements<Sheet>().Any())
                {
                    sheetId = sheets.Elements<Sheet>().Select(s => s.SheetId.Value).Max() + 1;
                }

                // Append the new worksheet and associate it with the workbook.
                Sheet sheet = new Sheet { Id = relationshipId, SheetId = sheetId, Name = worksheetName, State = state };

                sheets.Append(sheet);
            }
        }

        /// <summary>
        /// Translate ARGB Into HexBinaryValue
        /// </summary>
        /// <param name="alpha">The alpha.</param>
        /// <param name="red">The red.</param>
        /// <param name="green">The green.</param>
        /// <param name="blue">The blue.</param>
        /// <returns>The HexBinaryValue object</returns>
        public static HexBinaryValue TranslateARGBIntoHexBinaryValue(byte alpha, byte red, byte green, byte blue)
        {
            return new HexBinaryValue
            {
                Value = ColorTranslator.ToHtml(System.Drawing.Color.FromArgb(alpha, red, green, blue)).Replace("#", string.Empty)
            };
        }

        /// <summary>
        /// Translate Color Into HexBinaryValue
        /// </summary>
        /// <param name="fillColor">The fill color.</param>
        /// <returns>The HexBinaryValue object</returns>
        public static HexBinaryValue TranslateColorIntoHexBinaryValue(System.Drawing.Color fillColor)
        {
            return new HexBinaryValue
            {
                Value = ColorTranslator.ToHtml(System.Drawing.Color.FromArgb(fillColor.A, fillColor.R, fillColor.G, fillColor.B)).Replace("#", string.Empty)
            };
        }

        /// <summary>
        /// Return the row at the specified rowIndex located within
        /// the sheet data passed in via sheetData. If the row does not
        /// exist, create it.
        /// </summary>
        /// <param name="sheetData">The SheetData object</param>
        /// <param name="rowIndex">The row Index</param>
        /// <returns>The result Row</returns>
        public static Row GetRow(SheetData sheetData, UInt32 rowIndex)
        {
            var row = sheetData.Elements<Row>().FirstOrDefault(r => r.RowIndex.Value == rowIndex);
            if (row == null)
            {
                row = new Row { RowIndex = rowIndex };
                sheetData.Append(row);
            }

            return row;
        }

        /// <summary>
        /// Always adds a new row to the sheet and returns it, create it.
        /// </summary>
        /// <param name="sheetData">The SheetData object</param>
        /// <param name="rowIndex">The row Index</param>
        /// <returns>The result Row</returns>
        public static Row AddRow(SheetData sheetData, UInt32 rowIndex)
        {
            Row row = new Row { RowIndex = rowIndex };
            sheetData.Append(row);
            return row;
        }

        /// <summary>
        /// Given an Excel address such as E5 or AB128, GetRowIndex
        /// parses the address and returns the row index. 
        /// </summary>
        /// <param name="address">The address</param>
        /// <returns>The row index</returns>
        public static UInt32 GetRowIndex(string address)
        {
            UInt32 result = 0;

            for (int i = 0; i < address.Length; i++)
            {
                UInt32 l;
                if (UInt32.TryParse(address.Substring(i, 1), out l))
                {
                    string rowPart = address.Substring(i, address.Length - i);
                    if (UInt32.TryParse(rowPart, out l))
                    {
                        result = l;
                        break;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Create text cell
        /// </summary>
        /// <param name="columnIndex">Indicates: index of column</param>
        /// <param name="rowIndex">Indicates: index of row</param>
        /// <param name="value">Indicates: value for cell</param>
        /// <param name="preserveSpaces">Indicates: preserve space</param>
        /// <param name="styleIndex">Indicates: index of style</param>
        /// <param name="isFormulaRequired">Indicates: any formula is require for cell</param>
        /// <param name="formula">Indicates: formula for cell</param>
        /// <returns>creates text cell</returns>
        public static Cell CreateTextCell(uint columnIndex, uint rowIndex, object value, SpaceProcessingModeValues preserveSpaces = SpaceProcessingModeValues.Default, uint? styleIndex = null, Boolean isFormulaRequired = false, String formula = "")
        {
            Cell cell = new Cell
            {
                CellReference = GetColumnName(columnIndex) + rowIndex,
                DataType = CellValues.InlineString
            };

            if (value == null)
            {
                value = String.Empty;
            }

            if(isFormulaRequired)
            {
                CellFormula cellFormula = new CellFormula();
                cellFormula.Text = formula;
                cell.AppendChild(cellFormula);
            }

            InlineString inlineString = new InlineString();
            Text t = new Text { Space = preserveSpaces, Text = XmlSerializationHelper.CleanInvalidXmlChars(value.ToString()) };
            inlineString.AppendChild(t);
            cell.AppendChild(inlineString);
            if (styleIndex != null)
            {
                cell.StyleIndex = styleIndex;
            }

            return cell;
        }

        /// <summary>
        /// Creates text cell with formula to fill value based on formula
        /// </summary>
        /// <param name="columnIndex">Indicates: index of column</param>
        /// <param name="rowIndex">Indicates: index of row</param>
        /// <param name="value">Indicates: value for cell</param>
        /// <param name="formula">Indicates: formula to be calculated</param>
        /// <param name="preserveSpaces">Indicates: preserve space</param>
        /// <param name="styleIndex">Indicates: index of style</param>
        /// <returns>cell with formula</returns>
        public static Cell CreateTextCellWithFormula(uint columnIndex, uint rowIndex, object value, String formula, SpaceProcessingModeValues preserveSpaces = SpaceProcessingModeValues.Default, uint? styleIndex = null)
        {
            return CreateTextCell(columnIndex, rowIndex, value, preserveSpaces, styleIndex, true, formula);
        }

        public static Cell CreateNumberCell(uint columnIndex, uint rowIndex, object value, uint? styleIndex = null)
        {
            Cell cell = new Cell
            {
                CellReference = GetColumnName(columnIndex) + rowIndex,
                DataType = CellValues.Number
            };

            if (value == null)
            {
                value = String.Empty;
            }

            cell.CellValue = new CellValue(value.ToString());
            if (styleIndex != null)
            {
                cell.StyleIndex = styleIndex;
            }

            return cell;
        }

        /// <summary>
        /// Gets Worksheet
        /// </summary>
        /// <param name="workbookPart">The workbookPart</param>
        /// <param name="sheetName">The sheet name</param>
        /// <returns>The worksheet object</returns>
        public static Worksheet GetWorksheet(WorkbookPart workbookPart, String sheetName)
        {
            Worksheet worksheet = null;
            Sheet sheet = workbookPart.Workbook.Descendants<Sheet>().FirstOrDefault(s => s.Name == sheetName);

            if (sheet != null)
            {
                worksheet = ((WorksheetPart)workbookPart.GetPartById(sheet.Id)).Worksheet;
            }

            return worksheet;
        }

        /// <summary>
        /// Remove cell by provided address
        /// </summary>
        /// <param name="sheetData">The Sheet Data</param>
        /// <param name="columnIndex">Column index</param>
        /// <param name="rowIndex">Row index</param>
        public static void RemoveDataCell(SheetData sheetData, uint columnIndex, uint rowIndex)
        {
            Cell dataCell = GetDataCell(sheetData, columnIndex, rowIndex);
            if (dataCell != null)
            {
                dataCell.Remove();
            }
        }

        /// <summary>
        /// Remove rows from excel file
        /// </summary>
        /// <param name="sheetData">Document SheetData</param>
        /// <param name="rowsIndexes">Row indexes for removing</param>
        public static void RemoveRows(SheetData sheetData, List<uint> rowsIndexes)
        {
            var rows = sheetData.Elements<Row>().ToList();
            var elementsToRemove = sheetData.Elements<Row>().Where(row => rowsIndexes.Contains(row.RowIndex)).ToList();

            foreach (var row in elementsToRemove)
            {
                row.Remove();
            }

            rows = sheetData.Elements<Row>().ToList();
            UInt32Value i = 1;
            foreach (var row in rows)
            {
                foreach (Cell cell in row.Elements<Cell>())
                {
                    // Update the cells references
                    cell.CellReference = new StringValue(cell.CellReference.Value.Replace(row.RowIndex, i));
                }
                row.RowIndex = i;
                i++;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="columnIndex"></param>
        /// <returns></returns>
        public static string GetColumnName(uint columnIndex)
        {
            int dividend = (Int32)columnIndex;
            string columnName = String.Empty;

            while (dividend > 0)
            {
                int modifier = (dividend - 1) % 26;
                columnName = Convert.ToChar(65 + modifier).ToString() + columnName;
                dividend = (dividend - modifier) / 26;
            }

            return columnName;
        }

        /// <summary>
        /// Add a cell with the specified address to a row. 
        /// </summary>
        /// <param name="row"></param>
        /// <param name="address"></param>
        /// <returns></returns>
        public static Cell CreateCell(Row row, String address)
        {
            Cell refCell = null;

            // Cells must be in sequential order according to CellReference. 
            // Determine where to insert the new cell.
            foreach (Cell cell in row.Elements<Cell>())
            {
                if (string.Compare(cell.CellReference.Value, address, true) > 0)
                {
                    refCell = cell;
                    break;
                }
            }

            Cell cellResult = new Cell { CellReference = address };

            row.InsertBefore(cellResult, refCell);
            return cellResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="row"></param>
        /// <param name="columnIndex"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Cell CreateTextCell(Row row, uint columnIndex, object value, uint? styleIndex = null)
        {
            // Check if we can use SharedString..
            return CreateDataCell(row, columnIndex, CellValues.InlineString, value, styleIndex);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="row"></param>
        /// <param name="columnIndex"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Cell CreateNumberCell(Row row, uint columnIndex, object value, uint? styleIndex = null)
        {
            return CreateDataCell(row, columnIndex, CellValues.Number, value, styleIndex);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="row"></param>
        /// <param name="columnIndex"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Cell CreateDateCell(Row row, uint columnIndex, object value, uint? styleIndex = null)
        {
            return CreateDataCell(row, columnIndex, CellValues.Date, value, styleIndex);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="row"></param>
        /// <param name="columnIndex"></param>
        /// <param name="cellDataType"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Cell CreateDataCell(Row row, uint columnIndex, CellValues cellDataType, object value, uint? styleIndex = null)
        {
            UInt32 rowIndex = row.RowIndex;
            Cell cell = CreateDataCell(columnIndex, rowIndex, cellDataType, value, styleIndex);
            row.AppendChild<Cell>(cell);

            return cell;
        }
      
        /// <summary>
        /// 
        /// </summary>
        /// <param name="columnIndex"></param>
        /// <param name="rowIndex"></param>
        /// <param name="cellValue"></param>
        /// <returns></returns>
        public static Cell CreateDataCell(uint columnIndex, uint rowIndex, CellValues cellDataType, object value, uint? styleIndex = null)
        {
            Cell cell = new Cell
            {
                CellReference = GetColumnName(columnIndex) + rowIndex,
                DataType = cellDataType
            };

            if (value == null)
            {
                value = String.Empty;
            }

            switch (cellDataType)
            {
                case CellValues.InlineString:
                    InlineString inlineString = new InlineString();
                    Text t = new Text();
                    t.Space = SpaceProcessingModeValues.Preserve;
                    t.Text = value.ToString();
                    inlineString.AppendChild(t);
                    cell.AppendChild(inlineString);
                    if (styleIndex != null)
                    {
                        cell.StyleIndex = styleIndex;
                    }

                    break;
                case CellValues.Number:
                    cell.CellValue = new CellValue(value.ToString());
                    if (styleIndex != null)
                    {
                        cell.StyleIndex = styleIndex;
                    }

                    break;
                case CellValues.Date:

                    DateTime dateVal = (DateTime)value;
                    string strDateVal = dateVal.ToOADate().ToString();

                    cell.CellValue = new CellValue(strDateVal);
                    break;
            }

            return cell;
        }

        /// <summary>
        /// Get the cell object for a given row and column in a given sheet. If the cell object is not found, it returns null
        /// </summary>
        /// <param name="sheetData">Indicates the SheetData from which the cell is to be fetched</param>
        /// <param name="columnIndex">Indicates the column index for fetching the cell</param>
        /// <param name="rowIndex">Indicates the row index for fetching the cell</param>
        /// <returns>A Cell object based on column and row index</returns>
        public static Cell GetDataCell(SheetData sheetData, UInt32 columnIndex, UInt32 rowIndex)
        {
            Cell cell = null;

            Row row = GetRowByIndex(sheetData, rowIndex);
            if (row != null)
            {
                String cellAddress = GetColumnName(columnIndex) + rowIndex;
                cell = GetCellByReferenceValue(row, cellAddress);
            }

            return cell;
        }

        /// <summary>
        /// Get the cell object for a given row and column in a given sheet based on the reference cell for optimized search. 
        /// If the cell object is not found, it returns null
        /// </summary>
        /// <param name="sheetData">Indicates the SheetData from which the cell is to be fetched</param>
        /// <param name="referenceCell">Indicates the reference cell which is used to locate the requested cells</param>
        /// <param name="columnIndex">Indicates the column index for fetching the cell</param>
        /// <param name="rowIndex">Indicates the row index for fetching the cell</param>
        /// <returns>A Cell object based on column and row index</returns>
        public static Cell GetDataCell(SheetData sheetData, Cell referenceCell, UInt32 columnIndex, UInt32 rowIndex)
        {
            Row row = null;
            Cell cell = null;

            String cellAddress = GetColumnName(columnIndex) + rowIndex;

            if (referenceCell != null)
            {
                OpenXmlElement parentElement = referenceCell.Parent;
                if (parentElement != null && parentElement is Row)
                {
                    Row currentRow = (Row)parentElement;
                    if (currentRow.RowIndex == rowIndex)
                    {
                        row = currentRow;
                        Cell nextCell = referenceCell.NextSibling<Cell>();
                        if (nextCell != null && String.Compare(nextCell.CellReference.Value, cellAddress, StringComparison.InvariantCultureIgnoreCase) == 0)
                        {
                            cell = nextCell;
                        }
                    }
                    else
                    {
                        Row nextRow = currentRow.NextSibling<Row>();
                        if (nextRow != null && nextRow.RowIndex == rowIndex)
                        {
                            row = nextRow;
                        }
                    }
                }
            }

            // If row is not found by reference cell, find by looping the sheet data
            if (row == null)
            {
                row = GetRowByIndex(sheetData, rowIndex);
            }

            // If cell is not found by reference cell, find by looping the row
            if (row != null && cell == null)
            {
                cell = GetCellByReferenceValue(row, cellAddress);
            }

            return cell;
        }

        /// <summary>
        /// For a given cell, return the style index. If the cell is empty, returns 1
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        public static uint GetCellStyleIndex(Cell cell)
        {
            uint styleIndex = 1;

            if (cell != null)
            {
                styleIndex = cell.StyleIndex.Value;
            }

            return styleIndex;
        }

        /// <summary>
        /// Append new fill into the stylesheet
        /// </summary>
        /// <param name="stylesheet">Document stylesheet</param>
        /// <param name="attributes">Fill attributes (Foreground...)</param>
        /// <returns></returns>
        public static void AppendFill(Stylesheet stylesheet, params object[] attributes)
        {
            if (attributes == null)
                throw new ArgumentNullException("attributes");

            ForegroundColor foregroundColor = null;
            /* ... other attributes ... */

            if (attributes.Any(a => a is ForegroundColor))
            {
                foregroundColor = attributes.Single(a => a is ForegroundColor) as ForegroundColor;
            }
            /*... other attributes ...*/

            var patternFill = new PatternFill()
                {
                    PatternType = PatternValues.Solid,
                    ForegroundColor = foregroundColor
                    /*... other attributes ...*/
                };
            var fill = new Fill(patternFill);

            stylesheet.Fills.Append(fill);
        }

        /// <summary>
        /// Append new cell format into the stylesheet
        /// </summary>
        /// <param name="stylesheet">Document stylesheet</param>
        /// <returns>Id of appropriate format</returns>
        public static uint AppendErrorCellFormat(Stylesheet stylesheet)
        {
            CellFormat errorsCellFormat = new CellFormat
            {
                NumberFormatId = 49U,
                FontId = 11U,
                BorderId = 1U,
                ApplyNumberFormat = true,
                ApplyFont = true,
                ApplyFill = true,
                ApplyBorder = true,
                ApplyAlignment = true,
                Alignment = new Alignment
                {
                    Horizontal = HorizontalAlignmentValues.Left,
                    Vertical = VerticalAlignmentValues.Top,
                    WrapText = true
                }
            };

            stylesheet.CellFormats.Append(errorsCellFormat);

            var result = stylesheet.CellFormats.Count;
            return result;
        }

        /// <summary>
        /// Gets the Cell Reference Address
        /// </summary>
        /// <param name="entityRow"></param>
        /// <param name="columnIndex"></param>
        /// <returns></returns>
        public static String GetCellReferenceAddress(Row entityRow, UInt32 columnIndex)
        {
          return String.Format("{0}{1}", GetColumnName(columnIndex), entityRow.RowIndex);
        }

        #endregion

        #region Private Static Methods

        private static Row GetRowByIndex(SheetData sheetData, UInt32 rowIndex)
        {
            Row rowForIndex = null;
            foreach (Row row in sheetData.Elements<Row>())
            {
                if (row.RowIndex == rowIndex)
                {
                    rowForIndex = row;
                    break;
                }
            }
            return rowForIndex;
        }

        private static Cell GetCellByReferenceValue(Row row, String cellAddress)
        {
            Cell cellForRefValue = null;

            foreach (Cell cell in row.Elements<Cell>())
            {
                if (String.Compare(cell.CellReference.Value, cellAddress, StringComparison.InvariantCultureIgnoreCase) == 0)
                {
                    cellForRefValue = cell;
                    break;
                }
            }

            return cellForRefValue;
        }

        #endregion Private Static Methods
    }
}
