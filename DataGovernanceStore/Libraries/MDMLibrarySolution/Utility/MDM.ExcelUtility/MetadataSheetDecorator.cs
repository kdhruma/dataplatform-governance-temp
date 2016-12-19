using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

using MDM.BusinessObjects;
using MDM.Core;
using MDM.Interfaces;
using MDM.Utility;
using Cell = DocumentFormat.OpenXml.Spreadsheet.Cell;
using Row = DocumentFormat.OpenXml.Spreadsheet.Row;

namespace MDM.ExcelUtility
{
    internal static class MetadataSheetDecorator
    {
        private const UInt32 ColumnOne = 1;
        private const UInt32 ColumnTwo = 2;
        private const UInt32 ColumnThree = 3;
        private const UInt32 ColumnFour = 4;
        private const UInt32 ColumnFive = 5;
        private const UInt32 ColumnSeven = 7;
        private const UInt32 ColumnEight = 8;

        /// <summary>
        /// Populates data into Meta-data sheet
        /// </summary>
        /// <param name="workbookPart">The workbook part</param>
        /// <param name="entityCollection">The entities collection</param>
        /// <param name="complexAttributes"></param>
        /// <param name="requestedLocales"></param>
        public static void PopulateMetadataSheet(WorkbookPart workbookPart, EntityCollection entityCollection, Dictionary<string, string> complexAttributes, Collection<LocaleEnum> requestedLocales)
        {
            Worksheet workSheet = OpenSpreadsheetUtility.GetWorksheet(workbookPart, RSExcelConstants.MetadataSheetName);

            if (workSheet != null)
            {
                if (entityCollection.Any())
                {
                    SheetData sheetData = workSheet.GetFirstChild<SheetData>();
                    PopulateEntityMetadata(workbookPart, sheetData, entityCollection, requestedLocales);
                    PopulateRelationshipMetadata(workbookPart, sheetData, entityCollection);
                    PopulateProcessingOptions(workbookPart, sheetData);
                }

                PopulateComplexAttribute(workbookPart, workSheet, complexAttributes);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="workbookPart"></param>
        /// <param name="entityCollection"></param>
        /// <param name="complexAttributes"></param>
        public static void PopulateReportMetadataSheet(WorkbookPart workbookPart, Collection<StronglyTypedEntityBase> entityCollection, Dictionary<String, String> complexAttributes)
        {
            Worksheet workSheet = OpenSpreadsheetUtility.GetWorksheet(workbookPart, RSExcelConstants.MetadataSheetName);

            if (workSheet != null)
            {
                if (entityCollection.Any())
                {
                    SheetData sheetData = workSheet.GetFirstChild<SheetData>();
                    PopulateReportEntityMetadata(workbookPart, sheetData, entityCollection);
                    PopulateProcessingOptions(workbookPart, sheetData);
                }

                PopulateComplexAttribute(workbookPart, workSheet, complexAttributes);
            }
        }

        private static void PopulateComplexAttribute(WorkbookPart workbookPart, Worksheet workSheet, Dictionary<String, String> complexAttributes)
        {
            if (complexAttributes == null)
            {
                return;
            }

            SheetData sheetData = workSheet.GetFirstChild<SheetData>();

            // Get style rowIndex for header row
            Cell headerCell = OpenSpreadsheetUtility.GetDataCell(sheetData, ColumnOne, 1);
            UInt32 headerCellStyleInd = OpenSpreadsheetUtility.GetCellStyleIndex(headerCell);

            // Get style rowIndex for sub header row
            Cell subHeaderCell = OpenSpreadsheetUtility.GetDataCell(sheetData, ColumnOne, 2);
            UInt32 subHeaderStyleIndex = OpenSpreadsheetUtility.GetCellStyleIndex(subHeaderCell);

            UInt32 complexAttributeTableRowIndex = RSExcelConstants.DefaultComplexAttributeTableRowIndex;

            // Try to get existing complex Attribute Table Row Index
            Int32 rowsCount = sheetData.Elements<Row>().Count();
            for (int i = 1; i < rowsCount; i++)
            {
                Cell cell = OpenSpreadsheetUtility.GetDataCell(sheetData, 1, (UInt32)i);
                Int32 sharedStringId;
                if (cell != null && Int32.TryParse(cell.InnerText, out sharedStringId))
                {
                    SharedStringItem sharedStringItem = OpenSpreadsheetUtility.GetSharedStringItemById(workbookPart, sharedStringId);
                    if (sharedStringItem.InnerText == RSExcelConstants.ComplexAttributeMetadataTableCaption)
                    {
                        complexAttributeTableRowIndex = (UInt32)i;
                        break;
                    }
                }
            }

            UInt32 cellInsertRowIndex;

            // Adding rows and cells while appropriate one were not found
            if (complexAttributeTableRowIndex == RSExcelConstants.DefaultComplexAttributeTableRowIndex)
            {
                // Complex attribute table header
                Row complexAttributeHeaderRow = OpenSpreadsheetUtility.GetRow(sheetData, complexAttributeTableRowIndex++);
                Cell complexAttributeHeaderCell = OpenSpreadsheetUtility.CreateTextCell(complexAttributeHeaderRow, 1, "Complex Attribute Metadata", headerCellStyleInd);
                Cell blankCell = OpenSpreadsheetUtility.CreateTextCell(complexAttributeHeaderRow, 2, String.Empty, headerCellStyleInd);

                if (workSheet.Elements<MergeCells>().Any())
                {
                    MergeCells mergeCells = workSheet.Elements<MergeCells>().First();

                    MergeCell mergeCell = new MergeCell
                    {
                        Reference = new StringValue(String.Concat(complexAttributeHeaderCell.CellReference, ":", blankCell.CellReference))
                    };

                    // OpenXml won't update Merge Cells Count whenever we are adding any new child.So, manually updating child elements count to parent i.e MergeCells.
                    mergeCells.Append(mergeCell);
                    mergeCells.Count = UInt32Value.FromUInt32((uint)mergeCells.ChildElements.Count);
                }

                // Complex attribute - Attribute identifier column
                Row attributeIdentifierHeaderRow = OpenSpreadsheetUtility.GetRow(sheetData, complexAttributeTableRowIndex++);
                OpenSpreadsheetUtility.CreateTextCell(attributeIdentifierHeaderRow, 1, "Attribute Identifier", subHeaderStyleIndex);
                OpenSpreadsheetUtility.CreateTextCell(attributeIdentifierHeaderRow, 2, "Sheet", subHeaderStyleIndex);

                cellInsertRowIndex = complexAttributeTableRowIndex;

                // Complex attribute detail
                foreach (KeyValuePair<String, String> complexAttrSheetDetail in complexAttributes)
                {
                    Row complexAttrRow = OpenSpreadsheetUtility.GetRow(sheetData, cellInsertRowIndex++);

                    OpenSpreadsheetUtility.CreateTextCell(complexAttrRow, 1, complexAttrSheetDetail.Key);
                    OpenSpreadsheetUtility.CreateTextCell(complexAttrRow, 2, complexAttrSheetDetail.Value);
                }
            }
            else
            {
                cellInsertRowIndex = complexAttributeTableRowIndex + 2;



                // Complex attribute detail
                foreach (KeyValuePair<String, String> complexAttrSheetDetail in complexAttributes)
                {
                    Row complexAttrRow = OpenSpreadsheetUtility.GetRow(sheetData, cellInsertRowIndex++);
                    String keyCellAddress = OpenSpreadsheetUtility.GetColumnName(ColumnOne) + complexAttrRow.RowIndex;
                    UInt32 rowIndex = complexAttrRow.RowIndex;

                    if (complexAttrRow.Elements<Cell>().Any(item => item.CellReference.Value == keyCellAddress))
                    {
                        Cell attrIdentifierCell = complexAttrRow.Elements<Cell>().First(item => item.CellReference.Value == keyCellAddress);
                        OpenSpreadsheetUtility.AddInlineStringToCell(attrIdentifierCell, complexAttrSheetDetail.Key);
                    }
                    else
                    {
                        Cell cell = OpenSpreadsheetUtility.CreateDataCell(ColumnOne, rowIndex, CellValues.InlineString, complexAttrSheetDetail.Key);
                        if (complexAttrRow.Elements<Cell>().Any())
                        {
                            complexAttrRow.InsertAt(cell, 0);
                        }
                        else
                        {
                            complexAttrRow.AppendChild(cell);
                        }
                    }

                    String valueCellAddress = OpenSpreadsheetUtility.GetColumnName(ColumnTwo) + complexAttrRow.RowIndex;
                    if (complexAttrRow.Elements<Cell>().Any(item => item.CellReference.Value == valueCellAddress))
                    {
                        Cell sheetCell = complexAttrRow.Elements<Cell>().First(item => item.CellReference.Value == valueCellAddress);
                        sheetCell.Remove();
                        Cell cell = OpenSpreadsheetUtility.CreateDataCell(ColumnTwo, rowIndex, CellValues.InlineString, complexAttrSheetDetail.Value);
                        complexAttrRow.InsertAt(cell, 1);
                    }
                    else
                    {
                        Cell cell = OpenSpreadsheetUtility.CreateDataCell(ColumnTwo, rowIndex, CellValues.InlineString, complexAttrSheetDetail.Value);
                        if (complexAttrRow.Elements<Cell>().Any())
                        {
                            complexAttrRow.InsertAt(cell, 1);
                        }
                        else
                        {
                            complexAttrRow.AppendChild(cell);
                        }
                    }
                }
            }
        }

        private static void PopulateEntityMetadata(WorkbookPart workbookPart, SheetData sheetData, IEnumerable<Entity> entityCollection, Collection<LocaleEnum> requestedLocales)
        {
            UInt32 startingIndex = GetStartingIndexByText(workbookPart, sheetData, ColumnOne, "Default Organization");

            if (startingIndex > 0)
            {
                IEntity firstEntity = entityCollection.First();

                UpdateCellWithValue(sheetData, ColumnTwo, startingIndex++, firstEntity.OrganizationName);
                UpdateCellWithValue(sheetData, ColumnTwo, startingIndex++, firstEntity.ContainerName);
                UpdateCellWithValue(sheetData, ColumnTwo, startingIndex++, firstEntity.EntityTypeName);
                UpdateCellWithValue(sheetData, ColumnTwo, startingIndex++, firstEntity.CategoryPath);

                // If requestedLocales is null or has multiple values, then SDL is considered
                LocaleEnum localeEnum = GlobalizationHelper.GetSystemDataLocale();
                if (requestedLocales != null && requestedLocales.Count == 1)
                {
                    localeEnum = requestedLocales.First();
                }

                UpdateCellWithValue(sheetData, ColumnTwo, startingIndex++, localeEnum);

                // TODO: Entity doesn't contain Default Parent Extension Organization
                UpdateCellWithValue(sheetData, ColumnTwo, startingIndex++, string.Empty);

                UpdateCellWithValue(sheetData, ColumnTwo, startingIndex++, firstEntity.ParentExtensionEntityContainerLongName);
                UpdateCellWithValue(sheetData, ColumnTwo, startingIndex, firstEntity.ParentExtensionEntityCategoryPath);
            }
        }

        private static void PopulateReportEntityMetadata(WorkbookPart workbookPart, SheetData sheetData, IEnumerable<StronglyTypedEntityBase> entityCollection)
        {
            UInt32 startingIndex = GetStartingIndexByText(workbookPart, sheetData, ColumnOne, "Default Organization");

            if (startingIndex > 0)
            {
                StronglyTypedEntityBase firstEntity = entityCollection.First();

                UpdateCellWithValue(sheetData, ColumnTwo, startingIndex++, firstEntity.OrganizationName);
                UpdateCellWithValue(sheetData, ColumnTwo, startingIndex++, firstEntity.ContainerName);
                UpdateCellWithValue(sheetData, ColumnTwo, startingIndex++, firstEntity.EntityTypeName);
                UpdateCellWithValue(sheetData, ColumnTwo, startingIndex++, firstEntity.CategoryPath);
                //UpdateCellWithValue(sheetData, ColumnTwo, startingIndex++, firstEntity.Locale);

                // TODO: Entity doesn't contain Default Parent Extension Organization
                UpdateCellWithValue(sheetData, ColumnTwo, startingIndex++, string.Empty);

                UpdateCellWithValue(sheetData, ColumnTwo, startingIndex++, firstEntity.ParentExtensionEntityContainerLongName);
                UpdateCellWithValue(sheetData, ColumnTwo, startingIndex, firstEntity.ParentExtensionEntityCategoryPath);
            }
        }

        private static void PopulateProcessingOptions(WorkbookPart workbookPart, SheetData sheetData)
        {
            UInt32 startingIndex = GetStartingIndexByText(workbookPart, sheetData, ColumnSeven, "Collection Separator");

            if (startingIndex > 0)
            {
                UpdateCellWithValue(sheetData, ColumnEight, startingIndex++, RSExcelConstants.CollectionSeparator);
                UpdateCellWithValue(sheetData, ColumnEight, startingIndex, RSExcelConstants.UomSeparator);
            }
        }

        private static void PopulateRelationshipMetadata(WorkbookPart workbookPart, SheetData sheetData, IEnumerable<Entity> entityCollection)
        {
            UInt32 startingIndex = GetStartingIndexByText(workbookPart, sheetData, ColumnFour, "Default From Entity Organization");
            List<Entity> entities = entityCollection.ToList();

            if (startingIndex > 0)
            {
                IEntity firstEntity = entities.First();

                if (entities.Any(x => x.GetRelationships().Any()))
                {
                    Relationship relationship = entities.First(x => x.GetRelationships().Any()).GetRelationships().First();

                    if (relationship != null)
                    {
                        UpdateCellWithValue(sheetData, ColumnFive, startingIndex++, firstEntity.OrganizationName);
                        UpdateCellWithValue(sheetData, ColumnFive, startingIndex++, firstEntity.ContainerName);
                        UpdateCellWithValue(sheetData, ColumnFive, startingIndex++, firstEntity.EntityTypeName);
                        UpdateCellWithValue(sheetData, ColumnFive, startingIndex++, firstEntity.CategoryPath);

                        // TODO: Relationship doesn't contain Default To Entity Organization
                        UpdateCellWithValue(sheetData, ColumnFive, startingIndex++, String.Empty);

                        UpdateCellWithValue(sheetData, ColumnFive, startingIndex++, relationship.ToContainerName);
                        UpdateCellWithValue(sheetData, ColumnFive, startingIndex++, relationship.ToEntityTypeName);
                        UpdateCellWithValue(sheetData, ColumnFive, startingIndex, relationship.ToCategoryPath);
                    }
                }
            }
        }

        private static void UpdateCellWithValue(SheetData sheetData, UInt32 columnIndex, UInt32 rowIndex, object value)
        {
            Cell organizationCell = OpenSpreadsheetUtility.GetDataCell(sheetData, columnIndex, rowIndex);
            if (organizationCell != null)
            {
                organizationCell.Remove();
            }

            Row row = OpenSpreadsheetUtility.GetRow(sheetData, rowIndex);
            Cell cell = OpenSpreadsheetUtility.CreateDataCell(columnIndex, rowIndex, CellValues.InlineString, value);
            Int32 cellsCount = row.Elements<Cell>().Count();

            Int32 shiftIndex = 0;
            Boolean isThirdColumnCellMissed = OpenSpreadsheetUtility.GetDataCell(sheetData, ColumnThree, rowIndex) == null;
            if (isThirdColumnCellMissed && columnIndex > ColumnThree)
            {
                shiftIndex = 1;
            }

            Int32 indexToInsert = cellsCount >= columnIndex - 1 ? (Int32)columnIndex - 1 - shiftIndex : cellsCount;
            row.InsertAt(cell, indexToInsert);
        }

        private static UInt32 GetStartingIndexByText(WorkbookPart workbookPart, SheetData sheetData, UInt32 columnIndex, String headerText)
        {
            UInt32 startingIndex = 0;
            for (UInt32 i = 1; i < RSExcelConstants.DefaultComplexAttributeTableRowIndex; i++)
            {
                Cell captionCell = OpenSpreadsheetUtility.GetDataCell(sheetData, columnIndex, i);
                Int32 sharedStringId;
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
    }
}
