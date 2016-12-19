using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace MDM.ExcelUtility.RSExcelValidation
{
    using DocumentFormat.OpenXml;
    using MDM.BusinessObjects;
    using MDM.Core;
    using MDM.ExcelUtility.RSExcelValidation.AttributeValidationProviders;
    using MDM.Interfaces;
    using System.Collections.ObjectModel;
    using Column = Column;
    using Row = Row;

    /// <summary>
    /// 
    /// </summary>
    internal class ValidationProvider
    {
        #region Fields

        private static ValidationProvider _validationProviderInstance;
        private readonly IDictionary<AttributeDataType, IAttributeValidationProviderBase> _attrValidationProviders;

        #endregion

        #region Constructors

        private ValidationProvider()
        {
            _attrValidationProviders = new Dictionary<AttributeDataType, IAttributeValidationProviderBase>
            {
                { AttributeDataType.Date, new DateAttributeValidationProvider() },
                { AttributeDataType.String, new StringAttributeValidationProvider() },
                { AttributeDataType.Decimal, new DecimalAttributeValidationProvider() },
                { AttributeDataType.Integer, new IntegerAttributeValidationProvider() },
                { AttributeDataType.DateTime, new DateTimeAttributeValidationProvider() }
            };
        }

        /// <summary>
        /// Validation Provider instance
        /// </summary>
        public static ValidationProvider Provider
        {
            get
            {
                return _validationProviderInstance ?? (_validationProviderInstance = new ValidationProvider());
            }
        }

        #endregion

        #region Properties
        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Append Worksheet With Validation
        /// </summary>
        /// <param name="workbookPart">The workbook part</param>
        /// <param name="workSheet">The worksheet</param>
        /// <param name="attributeModels">The attributes collection</param>
        /// <param name="entityCollection">The entity Collection.</param>
        /// <param name="messages">The messages.</param>
        /// <param name="skipColumns">Number of skipped columns (metadata attribute count)</param>
        public void AppendWorksheetWithValidation(WorkbookPart workbookPart, Worksheet workSheet, List<AttributeModel> attributeModels, IDictionary<String, String> messages, Dictionary<Int32, String> attrExcelValidationInfoDictionary, int skipColumns = 0)
        {
            Provider.AppendDataValidations(workSheet, attributeModels, attrExcelValidationInfoDictionary, messages, skipColumns);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="workbookPart"></param>
        /// <param name="workbook"></param>
        /// <param name="attributesCollection"></param>
        /// <param name="attributeAllowedValuesMappings"></param>
        public void PopulateValidationLookupsHiddenSheet(WorkbookPart workbookPart, Workbook workbook, List<IAttributeModel> attributesCollection, Dictionary<Int32, String> attributeAllowedValuesMappings)
        {
            OpenSpreadsheetUtility.AddWorksheet(workbookPart, RSExcelConstants.LookupDictionarySheetName, SheetStateValues.Hidden);
            Worksheet lookupTableSheet = OpenSpreadsheetUtility.GetWorksheet(workbookPart, RSExcelConstants.LookupDictionarySheetName);

            IEnumerable<Columns> collection = lookupTableSheet.Descendants<Columns>();
            if (!collection.Any())
            {
                lookupTableSheet.InsertAt(new Columns(), 0);
            }

            Columns columns = lookupTableSheet.Descendants<Columns>().Single();
            SheetData sheetData = lookupTableSheet.GetFirstChild<SheetData>();
            UInt32 columnIndex = 1;

            DefinedNames definedNames = new DefinedNames();

            foreach (IAttributeModel attributeModel in attributesCollection)
            {
                if (attributeModel.IsComplex)
                {
                    foreach (IAttributeModel childAttributeModel in attributeModel.GetChildAttributeModels())
                    {
                        FillValidationLookupSheetData(childAttributeModel, attributeAllowedValuesMappings, ref columnIndex, sheetData, columns, definedNames);
                    }
                }
                else
                {
                    FillValidationLookupSheetData(attributeModel, attributeAllowedValuesMappings, ref columnIndex, sheetData, columns, definedNames);
                }
            }

            if (definedNames.Any())
            {
                CalculationProperties calculationProperties = workbook.Descendants<CalculationProperties>().First();
                workbook.InsertBefore(definedNames, calculationProperties);
            }

            lookupTableSheet.Save();
            workbookPart.Workbook.Save();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Provides DataValidations object containing DataValidation objects created depending upon attributes
        /// </summary>
        /// <param name="workSheet">The worksheet</param>
        /// <param name="attributes">The IAttributeModel collection</param>
        /// <param name="attributeExternalAllowedValuesMappings">The mapping for lookup validation</param>
        /// <param name="messages">The messages.</param>
        /// <param name="skipColumnsCount">The skip Columns Count.</param>
        private void AppendDataValidations(Worksheet workSheet, ICollection<AttributeModel> attributes, Dictionary<Int32, String> attributeExternalAllowedValuesMappings, IDictionary<String, String> messages, int skipColumnsCount = 0)
        {
            DataValidations result = new DataValidations();

            Boolean isExternalValidationSource = attributeExternalAllowedValuesMappings.Any();

            ExcelColumnNameCollection columnNameCollection = ExcelColumnNameCollection.Instance;
            columnNameCollection.DefineColumnsCount(skipColumnsCount + attributes.Count);

            int index = skipColumnsCount;

            foreach (IAttributeModel attributeModel in attributes)
            {
                String attrDataTypeName = attributeModel.AttributeDataTypeName;

                String columnName = columnNameCollection.GetColumnName(index);
                AttributeDataType attributeDataType;

                // Lookup, Boolean & Attributes with allowed values
                if (isExternalValidationSource && attributeExternalAllowedValuesMappings.ContainsKey(attributeModel.Id))
                {
                    // Validation with source on Lookup Table sheet
                    DataValidation externalDataValidation = new DataValidation();
                    externalDataValidation.AddPredefinedLookupValidation(columnName, GetValidationFormula(attributeModel.Name, attributeModel.Id), messages);
                    result.Append(externalDataValidation);
                }
                else if (Enum.TryParse(attrDataTypeName, true, out attributeDataType) && _attrValidationProviders.ContainsKey(attributeDataType) && !attributeModel.IsCollection)
                {
                    // Getting validation provider from collection
                    IAttributeValidationProviderBase validationProvider = _attrValidationProviders[attributeDataType];

                    // Getting resulting DataValidation object.
                    DataValidation dataValidation;
                    if (validationProvider.HasAttributeValidation(attributeModel, columnName, messages, out dataValidation))
                    {
                        // Adding received DataValidation object to the result
                        result.Append(dataValidation);
                    }
                }

                index++;
            }

            if (result.Any())
            {
                var pageMargins = workSheet.Descendants<PageMargins>().FirstOrDefault();
                if (pageMargins != null)
                {
                    workSheet.InsertBefore(result, pageMargins);
                }
                else
                {
                    workSheet.Append(result);
                }
            }
        }
        
        private void FillValidationLookupSheetData(IAttributeModel attributeModel, Dictionary<Int32, String> attributeAllowedValuesMappings, ref UInt32 columnIndex, SheetData sheetData, Columns columns, DefinedNames definedNames)
        {
            if (attributeAllowedValuesMappings.Keys.Contains(attributeModel.Id))
            {
                // Adding cell with table name
                String attrHeaderText = attributeModel.LongName;
                Column column = new Column
                {
                    Min = columnIndex,
                    Max = columnIndex,
                    Width = 25,
                    CustomWidth = true,
                    Style = 1
                };
                UInt32 rowIndex = 1;
                Row row = OpenSpreadsheetUtility.GetRow(sheetData, rowIndex);
                OpenSpreadsheetUtility.AppendRowWithTextCell(row, columnIndex, attrHeaderText);
                rowIndex++;

                String excelValidationInfo = attributeAllowedValuesMappings[attributeModel.Id];
                // Populating Lookup table values
                String[] values = excelValidationInfo.Split(new[] { RSExcelConstants.LookupValuesSeparator }, StringSplitOptions.RemoveEmptyEntries);

                SpaceProcessingModeValues spaceProcessingMode = String.Equals(attributeModel.AttributeDataTypeName, AttributeDataType.String.ToString(), StringComparison.InvariantCulture)
                                                                    ? SpaceProcessingModeValues.Preserve
                                                                    : SpaceProcessingModeValues.Default;
                foreach (string value in values)
                {
                    row = OpenSpreadsheetUtility.GetRow(sheetData, rowIndex);                    
                    OpenSpreadsheetUtility.AppendRowWithTextCell(row, columnIndex, value, spaceProcessingMode);
                    rowIndex++;
                }

                Collection<String> formulas = new Collection<string>();
                formulas.Add("IF(Metadata!H5=\"\",\"\",Metadata!H5)"); //Key word to delete value from Metadata sheet
                formulas.Add("IF(Metadata!H6=\"\",\"\",Metadata!H6)"); //Key word to clear value from Metadata sheet

                foreach (String formula in formulas)
                {
                    row = OpenSpreadsheetUtility.GetRow(sheetData, rowIndex);
                    OpenSpreadsheetUtility.AppendRowWithFormulatedTextCell(row, columnIndex, null, formula);
                    rowIndex++;
                }
                

                columns.AppendChild(column);

                // Filling the mappings collection
                String columnName = OpenSpreadsheetUtility.GetColumnName(columnIndex);
                const Int32 FirstValueRowNumber = 2; // We always starts from the second row
                Int32 lastValueRowNumber = FirstValueRowNumber + formulas.Count() + values.Count() - 1;
                String listDescription = String.Format(RSExcelConstants.ExternalValidationFormulaFormat, columnName, FirstValueRowNumber, lastValueRowNumber);
                String collectionName = GetValidationFormula(attributeModel.Name, attributeModel.Id);
                if (collectionName.Length > 30)
                {
                    collectionName = collectionName.Substring(0, 29);
                }

                DefinedName definedName = new DefinedName { Name = collectionName, Text = listDescription };
                definedNames.Append(definedName);

                columnIndex++;
            }
        }

        private Dictionary<Int32, String> GetExcelValidationDictionary(IEnumerable<AttributeModel> attributeModels)
        {
            Dictionary<Int32, String> populatedAttrAllowedValMappings = new Dictionary<Int32, String>();

            foreach (AttributeModel attributeModel in attributeModels)
            {
                String collectionName = GetValidationFormula(attributeModel.Name, attributeModel.Id);
                populatedAttrAllowedValMappings.Add(attributeModel.Id, collectionName);
            }

            return populatedAttrAllowedValMappings;
        }

        private String GetValidationFormula(String attributeName, Int32 attributeId)
        {
            attributeName = Regex.Replace(attributeName.Trim(), "[^a-zA-Z0-9_]+", "");

            //Adding a separator to make defined name unique
            String collectionName = "_" + attributeName + attributeId.ToString();

            if (collectionName.Length > 30)
            {
                collectionName = collectionName.Substring(0, 29);
            }

            return collectionName;
        }

        #endregion

        #endregion
    }
}