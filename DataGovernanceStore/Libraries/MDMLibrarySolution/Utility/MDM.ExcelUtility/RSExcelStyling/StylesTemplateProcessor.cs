using System;
using System.IO;
using System.Linq;

using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace MDM.ExcelUtility.RSExcelStyling
{
    using MDM.Utility;

    internal static class StylesTemplateProcessor
    {
        private const UInt32 MetadataColumnIndex = 2;
        private const UInt32 RequiredCommonColumnIndex = 3;
        private const UInt32 RequiredCategorySpecificColumnIndex = 4;
        private const UInt32 RequiredRelationshipColumnIndex = 5;
        private const UInt32 OptionalCommonColumnIndex = 6;
        private const UInt32 OptionalCategorySpecificColumnIndex = 7;
        private const UInt32 OptionalRelationshipColumnIndex = 8;
		private const UInt32 RequiredCommonCollectionColumnIndex = 9;
		private const UInt32 RequiredCategorySpecificCollectionColumnIndex = 10;
		private const UInt32 RequiredRelationshipCollectionColumnIndex = 11;
		private const UInt32 OptionalCommonCollectionColumnIndex = 12;
		private const UInt32 OptionalCategorySpecificCollectionColumnIndex = 13;
		private const UInt32 OptionalRelationshipCollectionColumnIndex = 14;
        private const UInt32 SystemAttributeColumnIndex = 15;
        private const UInt32 WorkflowAttributeColumnIndex = 16;

        private const UInt32 BackgroundColorRowIndex = 6;
        private const UInt32 FontColorRowIndex = 7;
        private const UInt32 FontSizeRowIndex = 8;
        private const UInt32 FontStyleRowIndex = 9;
        private const UInt32 FontNameRowIndex = 10;

        private const String BoldStyleText = "Bold";
        private const String ItalicStyleText = "Italic";

        /// <summary>
        /// Processes Styles Template document
        /// </summary>
        /// <param name="templateFile">The full path to the styles template</param>
        /// <returns>The WorksheetStyles object</returns>
        public static WorksheetStyles ProcessStylesTemplateFile(String templateFile)
        {
            string tempFileName = AppConfigurationHelper.GetAppConfig<String>("Jobs.TemporaryFileRoot") + "\\" + Guid.NewGuid();

            File.Copy(templateFile, tempFileName, true);

            WorksheetStyles worksheetStyles = new WorksheetStyles();
            using (SpreadsheetDocument document = SpreadsheetDocument.Open(tempFileName, true))
            {
                WorkbookPart workbookPart = document.WorkbookPart;
                if (workbookPart != null)
                {
                    Sheet sheet = workbookPart.Workbook.Descendants<Sheet>().FirstOrDefault();
                    if (sheet != null)
                    {
                        Stylesheet stylesheet = workbookPart.WorkbookStylesPart.Stylesheet;
                        Worksheet worksheet = ((WorksheetPart)workbookPart.GetPartById(sheet.Id)).Worksheet;
                        SheetData sheetData = worksheet.GetFirstChild<SheetData>();
                        SharedStringTable sharedStringTable = workbookPart.SharedStringTablePart.SharedStringTable;
                        
                        // Processing metadata attributes column
                        worksheetStyles.MetadataAttributeStyle = ProcessColumn(sheetData, stylesheet, sharedStringTable, MetadataColumnIndex);

                        // Processing required common attributes column
                        worksheetStyles.RequiredCommonAttributeStyle = ProcessColumn(sheetData, stylesheet, sharedStringTable, RequiredCommonColumnIndex);

                        // Processing required category specific attributes column
                        worksheetStyles.RequiredCategorySpecificAttributeStyle = ProcessColumn(sheetData, stylesheet, sharedStringTable, RequiredCategorySpecificColumnIndex);                        
                        
                        // Processing required relationship attributes column
                        worksheetStyles.RequiredRelationshipAttributeStyle = ProcessColumn(sheetData, stylesheet, sharedStringTable, RequiredRelationshipColumnIndex);

                        // Processing optional common attributes column
                        worksheetStyles.OptionalCommonAttributeStyle = ProcessColumn(sheetData, stylesheet, sharedStringTable, OptionalCommonColumnIndex);

                        // Processing optional category specific attributes column
                        worksheetStyles.OptionalCategorySpecificAttributeStyle = ProcessColumn(sheetData, stylesheet, sharedStringTable, OptionalCategorySpecificColumnIndex);

                        // Processing optional relationship attributes column
                        worksheetStyles.OptionalRelationshipAttributeStyle = ProcessColumn(sheetData, stylesheet, sharedStringTable, OptionalRelationshipColumnIndex);

						// Processing required common Collection attributes column
						worksheetStyles.RequiredCommonAttributeCollectionStyle = ProcessColumn(sheetData, stylesheet, sharedStringTable, RequiredCommonCollectionColumnIndex);

						// Processing required category specific Collection attributes column
						worksheetStyles.RequiredCategorySpecificAttributeCollectionStyle = ProcessColumn(sheetData, stylesheet, sharedStringTable, RequiredCategorySpecificCollectionColumnIndex);

                        // Processing required relationship Collection attributes column
                        worksheetStyles.RequiredRelationshipAttributeCollectionStyle = ProcessColumn(sheetData, stylesheet, sharedStringTable, RequiredRelationshipCollectionColumnIndex);

						// Processing optional common Collection attributes column
						worksheetStyles.OptionalCommonAttributeCollectionStyle = ProcessColumn(sheetData, stylesheet, sharedStringTable, OptionalCommonCollectionColumnIndex);

						// Processing optional category specific Collection attributes column
						worksheetStyles.OptionalCategorySpecificAttributeCollectionStyle = ProcessColumn(sheetData, stylesheet, sharedStringTable, OptionalCategorySpecificCollectionColumnIndex);

                        // Processing optional relationship Collection attributes column
                        worksheetStyles.OptionalRelationshipAttributeCollectionStyle = ProcessColumn(sheetData, stylesheet, sharedStringTable, OptionalRelationshipCollectionColumnIndex);

                        // Processing system attributes column
                        worksheetStyles.SystemAttributeStyle = ProcessColumn(sheetData, stylesheet, sharedStringTable, SystemAttributeColumnIndex);

                        // Processing workflow attributes column
                        worksheetStyles.WorkflowAttributeStyle = ProcessColumn(sheetData, stylesheet, sharedStringTable, WorkflowAttributeColumnIndex);
                    }
                }

                document.Close();
            }

            return worksheetStyles;
        }

        private static AttributesTypeStyle ProcessColumn(SheetData sheetData, Stylesheet stylesheet, SharedStringTable sharedStringTable, UInt32 columnIndex)
        {
            AttributesTypeStyle attributesTypeStyle = new AttributesTypeStyle();

            // Background Cell
            Cell backgroundCell = OpenSpreadsheetUtility.GetDataCell(sheetData, columnIndex, BackgroundColorRowIndex);
            PatternFill backgroundCellPatternFill = GetCellPatternFill(stylesheet, backgroundCell);
            ForegroundColor backgroundCellForegroundColor = backgroundCellPatternFill.ForegroundColor;
            attributesTypeStyle.BackgroundRgb = backgroundCellForegroundColor.Rgb;
            attributesTypeStyle.BackgroundIndexed = backgroundCellForegroundColor.Indexed;

            // Font color cell
            Cell fontColorCell = OpenSpreadsheetUtility.GetDataCell(sheetData, columnIndex, FontColorRowIndex);
            PatternFill fontColorCellPatternFill = GetCellPatternFill(stylesheet, fontColorCell);
            ForegroundColor fontColorCellForegroundColor = fontColorCellPatternFill.ForegroundColor;
            attributesTypeStyle.FontRgb = fontColorCellForegroundColor.Rgb;
            attributesTypeStyle.FontIndexed = fontColorCellForegroundColor.Indexed;

            // Font size cell
            Cell fontSizeCell = OpenSpreadsheetUtility.GetDataCell(sheetData, columnIndex, FontSizeRowIndex);
            Double size = Double.Parse(fontSizeCell.InnerText);
            attributesTypeStyle.FontSizeVal = DoubleValue.FromDouble(size);

            // Font name cell
            Cell fontNameCell = OpenSpreadsheetUtility.GetDataCell(sheetData, columnIndex, FontNameRowIndex);
            String fontName = fontNameCell.InnerText;
            attributesTypeStyle.FontName = fontName;

            // Font style cell
            Cell fontStyleCell = OpenSpreadsheetUtility.GetDataCell(sheetData, columnIndex, FontStyleRowIndex);
            Int32 id = Int32.Parse(fontStyleCell.CellValue.Text);
            String styleText = OpenSpreadsheetUtility.GetSharedStringItemById(sharedStringTable, id).InnerText;
            attributesTypeStyle.IsBold = styleText.Contains(BoldStyleText);
            attributesTypeStyle.IsItalic = styleText.Contains(ItalicStyleText);

            return attributesTypeStyle;
        }

        private static PatternFill GetCellPatternFill(Stylesheet stylesheet, Cell cell)
        {
            Int32 cellStyleIndex = (Int32)cell.StyleIndex.Value;
            CellFormat cellFormat = (CellFormat)stylesheet.CellFormats.ChildElements[cellStyleIndex];
            Fill fill = (Fill)stylesheet.Fills.ChildElements[(int)cellFormat.FillId.Value];
            return fill.PatternFill;
        }
    }
}
