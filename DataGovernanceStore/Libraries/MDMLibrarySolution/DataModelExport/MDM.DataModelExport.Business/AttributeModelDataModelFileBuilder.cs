using System;
using System.Collections.Generic;

using DocumentFormat.OpenXml.Spreadsheet;

namespace MDM.DataModelExport.Business
{
    using MDM.AttributeModelManager.Business;
    using MDM.BusinessObjects.Exports;
    using MDM.Core;
    using MDM.Core.DataModel;
    using MDM.ExcelUtility;
    using MDM.Interfaces;
    using BO = MDM.BusinessObjects;

    /// <summary>
    /// Class provides functionality to export Attribute Model data as an excel file.
    /// </summary>
    internal class AttributeModelDataModelFileBuilder : DataModelFileBuilderBase
    {
        #region Overridden Methods

        protected override IDataModelObjectCollection GetDataModelObjectCollection(BO.CallerContext callerContext)
        {
            AttributeModelBL attributeModelBL = new AttributeModelBL();
            BO.AttributeModelCollection attributeModels = attributeModelBL.GetAllBaseAttributeModels();

            BO.AttributeModelCollection filteredAttributeModels = new BO.AttributeModelCollection();

            foreach(BO.AttributeModel attributeModel in attributeModels)
            {
                if(attributeModel.Id >= 4000)
                {
                    filteredAttributeModels.Add(attributeModel);
                }
            }

            return filteredAttributeModels;
        }

        protected override IDataModelObjectCollection GetDataModelObjectCollection(DataModelExportContext dataModelExportContext, BO.CallerContext callerContext)
        {
            AttributeModelBL attributeModelBL = new AttributeModelBL();
            BO.AttributeModelCollection attributeModels = new BO.AttributeModelCollection();
            BO.AttributeModelCollection filteredAttributeModels = new BO.AttributeModelCollection();

            if (dataModelExportContext.ContainerIdList != null && dataModelExportContext.ContainerIdList.Count > 0)
            {
                attributeModels = attributeModelBL.GetMappedAttributeModelsForContainers(dataModelExportContext.ContainerIdList, callerContext);

                if (attributeModels != null && attributeModels.Count > 0)
                {
                    foreach (BO.AttributeModel attributeModel in attributeModels)
                    {
                        if (attributeModel.Id >= 4000)
                        {
                            filteredAttributeModels.Add(attributeModel);

                            if (dataModelExportContext.SheetNames.Contains(DataModelSheet.LookupModel) &&
                                attributeModel.IsLookup && !dataModelExportContext.LookupTableNames.Contains(attributeModel.LookUpTableName))
                            {
                                dataModelExportContext.LookupTableNames.Add(attributeModel.LookUpTableName);
                            }
                        }
                    }
                }
            }
            else
            {
                return GetDataModelObjectCollection(callerContext);
            }

            return filteredAttributeModels;
        }

        protected override void PopulateDataModelObjectInDataRow(IDataModelObject dataModelObject, List<String> headerList, Row dataRow)
        {
            UInt32 columnIndex = 1;
            Boolean isAttributeGroup = false;

            PopulateDataModelBasePropertiesInDataRow(dataModelObject, headerList, dataRow, ref columnIndex);

            BO.AttributeModel attributeModel = (BO.AttributeModel)dataModelObject;

            if (attributeModel.AttributeModelType == AttributeModelType.AttributeGroup)
            {
                isAttributeGroup = true;
            }

            if (headerList.Contains(DataModelDictionary.AttributeModelDictionary[DataModelAttributeModel.AttributeType]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, attributeModel.GetAttributeModelTypeForDataModel());
            }

            if (headerList.Contains(DataModelDictionary.AttributeModelDictionary[DataModelAttributeModel.AttributeName]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, attributeModel.Name);
            }

            if (headerList.Contains(DataModelDictionary.AttributeModelDictionary[DataModelAttributeModel.AttributeLongName]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, attributeModel.LongName);
            }
            
            if (headerList.Contains(DataModelDictionary.AttributeModelDictionary[DataModelAttributeModel.AttributeParentName]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, attributeModel.AttributeParentName);
            }

            if (headerList.Contains(DataModelDictionary.AttributeModelDictionary[DataModelAttributeModel.DataType]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, attributeModel.AttributeDataTypeName);
            }

            if (headerList.Contains(DataModelDictionary.AttributeModelDictionary[DataModelAttributeModel.DisplayType]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, attributeModel.AttributeDisplayTypeName);
            }

            if (headerList.Contains(DataModelDictionary.AttributeModelDictionary[DataModelAttributeModel.IsCollection]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, ConvertBooleanValuesToString(attributeModel.IsCollection));
            }

            if (headerList.Contains(DataModelDictionary.AttributeModelDictionary[DataModelAttributeModel.IsInheritable]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, ConvertBooleanValuesToString(attributeModel.Inheritable));
            }

            if (headerList.Contains(DataModelDictionary.AttributeModelDictionary[DataModelAttributeModel.IsLocalizable]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, ConvertBooleanValuesToString(attributeModel.IsLocalizable));
            }

            if (headerList.Contains(DataModelDictionary.AttributeModelDictionary[DataModelAttributeModel.IsComplex]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, ConvertBooleanValuesToString(attributeModel.IsComplex));
            }

            if (headerList.Contains(DataModelDictionary.AttributeModelDictionary[DataModelAttributeModel.IsLookup]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, ConvertBooleanValuesToString(attributeModel.IsLookup));
            }

            if (headerList.Contains(DataModelDictionary.AttributeModelDictionary[DataModelAttributeModel.IsRequired]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, ConvertBooleanValuesToString(attributeModel.Required));
            }

            if (headerList.Contains(DataModelDictionary.AttributeModelDictionary[DataModelAttributeModel.IsReadOnly]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, ConvertBooleanValuesToString(attributeModel.ReadOnly));
            }

            if (headerList.Contains(DataModelDictionary.AttributeModelDictionary[DataModelAttributeModel.IsHidden]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, ConvertBooleanValuesToString(attributeModel.IsHidden));
            }

            if (headerList.Contains(DataModelDictionary.AttributeModelDictionary[DataModelAttributeModel.ShowAtEntityCreation]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, ConvertBooleanValuesToString(attributeModel.ShowAtCreation));
            }

            if (headerList.Contains(DataModelDictionary.AttributeModelDictionary[DataModelAttributeModel.IsSearchable]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, ConvertBooleanValuesToString(attributeModel.Searchable));
            }

            if (headerList.Contains(DataModelDictionary.AttributeModelDictionary[DataModelAttributeModel.IsNullValueSearchRequired]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, ConvertBooleanValuesToString(attributeModel.AllowNullSearch));
            }

           // TODO: This property is not available.
            if (headerList.Contains(DataModelDictionary.AttributeModelDictionary[DataModelAttributeModel.GenerateReportTableColumn]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, String.Empty);
            }

             if (headerList.Contains(DataModelDictionary.AttributeModelDictionary[DataModelAttributeModel.DefaultValue]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, attributeModel.DefaultValue);
            }

            if (headerList.Contains(DataModelDictionary.AttributeModelDictionary[DataModelAttributeModel.MinimumLength]))
            {
                OpenSpreadsheetUtility.AppendRowWithNumberCell(dataRow, columnIndex++, attributeModel.MinLength);
            }

            if (headerList.Contains(DataModelDictionary.AttributeModelDictionary[DataModelAttributeModel.MaximumLength]))
            {
                OpenSpreadsheetUtility.AppendRowWithNumberCell(dataRow, columnIndex++, attributeModel.MaxLength);
            }

            Boolean isInclusive;
            String rangeFrom = GetRange(attributeModel.MinInclusive, attributeModel.MinExclusive, out isInclusive);
            if (headerList.Contains(DataModelDictionary.AttributeModelDictionary[DataModelAttributeModel.RangeFrom]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, rangeFrom);
            }

            if (headerList.Contains(DataModelDictionary.AttributeModelDictionary[DataModelAttributeModel.IsRangeFromInclusive]))
            {
                String isRangeFromInclusive = String.Empty;

                if (!String.IsNullOrWhiteSpace(rangeFrom))
                {
                    isRangeFromInclusive = ConvertBooleanValuesToString(isInclusive);
                }

                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, isRangeFromInclusive);
            }

            String rangeTo = GetRange(attributeModel.MaxInclusive, attributeModel.MaxExclusive, out isInclusive);
            if (headerList.Contains(DataModelDictionary.AttributeModelDictionary[DataModelAttributeModel.RangeTo]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, rangeTo);
            }

            if (headerList.Contains(DataModelDictionary.AttributeModelDictionary[DataModelAttributeModel.IsRangeToInclusive]))
            {
                String isRangeToInclusive = String.Empty;

                if (!String.IsNullOrWhiteSpace(rangeTo))
                {
                    isRangeToInclusive = ConvertBooleanValuesToString(isInclusive);
                }

                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, isRangeToInclusive);
            }

            Boolean isNotDecimalAttribute = (String.Compare(attributeModel.AttributeDataTypeName, AttributeDataType.Decimal.ToString(), StringComparison.InvariantCultureIgnoreCase) != 0);

            if (headerList.Contains(DataModelDictionary.AttributeModelDictionary[DataModelAttributeModel.Precision]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, (isNotDecimalAttribute) ? String.Empty : attributeModel.Precision.ToString());
            }

            if (headerList.Contains(DataModelDictionary.AttributeModelDictionary[DataModelAttributeModel.IsPrecisionArbitrary]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, (isNotDecimalAttribute) ? String.Empty : ConvertBooleanValuesToString(attributeModel.IsPrecisionArbitrary));
            }

            if (headerList.Contains(DataModelDictionary.AttributeModelDictionary[DataModelAttributeModel.UOMType]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, attributeModel.UomType);
            }

            if (headerList.Contains(DataModelDictionary.AttributeModelDictionary[DataModelAttributeModel.AllowedUOMs]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, attributeModel.AllowableUOM);
            }

            if (headerList.Contains(DataModelDictionary.AttributeModelDictionary[DataModelAttributeModel.DefaultUOM]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, attributeModel.DefaultUOM);
            }

            if (headerList.Contains(DataModelDictionary.AttributeModelDictionary[DataModelAttributeModel.AllowableValues]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, attributeModel.AllowableValues);
            }


            if (headerList.Contains(DataModelDictionary.AttributeModelDictionary[DataModelAttributeModel.LookUpTableName]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, attributeModel.LookUpTableName);
            }

            if (headerList.Contains(DataModelDictionary.AttributeModelDictionary[DataModelAttributeModel.LookupDisplayColumns]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, attributeModel.LkDisplayColumns);
            }

            if (headerList.Contains(DataModelDictionary.AttributeModelDictionary[DataModelAttributeModel.LookupSearchColumns]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, attributeModel.LkSearchColumns);
            }

            if (headerList.Contains(DataModelDictionary.AttributeModelDictionary[DataModelAttributeModel.LookupDisplayFormat]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, attributeModel.LkDisplayFormat);
            }

            if (headerList.Contains(DataModelDictionary.AttributeModelDictionary[DataModelAttributeModel.LookupSortOrder]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, attributeModel.LkSortOrder);
            }

            if (headerList.Contains(DataModelDictionary.AttributeModelDictionary[DataModelAttributeModel.ExportFormat]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, attributeModel.ExportMask);
            }

            if (headerList.Contains(DataModelDictionary.AttributeModelDictionary[DataModelAttributeModel.SortOrder]))
            {
                OpenSpreadsheetUtility.AppendRowWithNumberCell(dataRow, columnIndex++, attributeModel.SortOrder);
            }

            if (headerList.Contains(DataModelDictionary.AttributeModelDictionary[DataModelAttributeModel.Definition]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, attributeModel.Definition);
            }

            if (headerList.Contains(DataModelDictionary.AttributeModelDictionary[DataModelAttributeModel.Example]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, attributeModel.AttributeExample);
            }

            if (headerList.Contains(DataModelDictionary.AttributeModelDictionary[DataModelAttributeModel.BusinessRule]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, attributeModel.BusinessRule);
            }

            if (headerList.Contains(DataModelDictionary.AttributeModelDictionary[DataModelAttributeModel.Label]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, attributeModel.Label);
            }

            if (headerList.Contains(DataModelDictionary.AttributeModelDictionary[DataModelAttributeModel.Extension]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, attributeModel.Extension);
            }

            if (headerList.Contains(DataModelDictionary.AttributeModelDictionary[DataModelAttributeModel.WebURI]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, attributeModel.WebUri);
            }

            if (headerList.Contains(DataModelDictionary.AttributeModelDictionary[DataModelAttributeModel.EnableHistory]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, (isAttributeGroup) ? String.Empty : ConvertBooleanValuesToString(attributeModel.EnableHistory));
            }
            
            if (headerList.Contains(DataModelDictionary.AttributeModelDictionary[DataModelAttributeModel.ApplyTimeZoneConversion]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, (isAttributeGroup) ? String.Empty : ConvertBooleanValuesToString(attributeModel.ApplyTimeZoneConversion));
            }

            if (headerList.Contains(DataModelDictionary.AttributeModelDictionary[DataModelAttributeModel.AttributeRegExp]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, attributeModel.AttributeRegEx);
            }
        }

        protected override ObjectType ObjectType
        {
            get { return ObjectType.AttributeModel; }
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Returns appropriate range for provided inclusive and exclusive ranges
        /// </summary>
        /// <param name="inclusiveRange">Specifies inclusive range</param>
        /// <param name="exclusiveRange">Specifies exclusive range</param>
        /// <param name="isInclusive">Specifies whether the range is inclusive</param>
        /// <returns>Returns range (inclusive or exclusive)</returns>
        private static String GetRange(String inclusiveRange, String exclusiveRange, out Boolean isInclusive)
        {
            if (!String.IsNullOrWhiteSpace(inclusiveRange))
            {
                isInclusive = true;
                return inclusiveRange;
            }

            isInclusive = false;
            return exclusiveRange;
        }

        #endregion
    }
}
