using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using DocumentFormat.OpenXml.Spreadsheet;

namespace MDM.DataModelExport.Business
{
    using MDM.Core;
    using MDM.Core.DataModel;
    using MDM.ExcelUtility;
    using MDM.Interfaces;
    using MDM.AttributeModelManager.Business;
    using BO = MDM.BusinessObjects;
    using MDM.KnowledgeManager.Business;
    using MDM.BusinessObjects.Exports;

    /// <summary>
    /// Class provides functionality to export Attribute Model localization data as an excel file.
    /// </summary>
    internal class AttributeModelLocaleMapFileBuilder : DataModelFileBuilderBase
    {
        #region Overridden Methods

        protected override IDataModelObjectCollection GetDataModelObjectCollection(BO.CallerContext callerContext)
        {
            AttributeModelBL attributeModelBL = new AttributeModelBL();
            LocaleBL localeBL = new LocaleBL();

            Collection<LocaleEnum> dataLocales = localeBL.GetAvailableLocaleValues();
            dataLocales.Remove(systemDataLocale);

            BO.AttributeModelContext attributeModelContext = new BO.AttributeModelContext();
            attributeModelContext.AttributeModelType = AttributeModelType.AttributeMaster;
            attributeModelContext.ApplySecurity = false;
            attributeModelContext.ApplySorting = false;
            attributeModelContext.Locales = dataLocales;

            BO.AttributeModelCollection attributeModels = attributeModelBL.Get(attributeModelContext);

            BO.AttributeModelCollection filteredAttributeModels = new BO.AttributeModelCollection();

            foreach (BO.AttributeModel attributeModel in attributeModels)
            {
                if (attributeModel.Id >= 4000)
                {
                    filteredAttributeModels.Add(attributeModel);
                }
            }

            return filteredAttributeModels;
        }

        protected override IDataModelObjectCollection GetDataModelObjectCollection(DataModelExportContext dataModelExportContext, BO.CallerContext callerContext)
        {
            BO.AttributeModelCollection filteredAttributeModels = new BO.AttributeModelCollection();
            Collection<LocaleEnum> dataLocales = dataModelExportContext.Locales;
            if (dataLocales.Contains(systemDataLocale))
            {
                dataLocales.Remove(systemDataLocale);
            }

            if (dataLocales != null && dataLocales.Count > 0)
            {
                AttributeModelBL attributeModelBL = new AttributeModelBL();

                BO.AttributeModelCollection attributeModels;

                BO.AttributeModelContext attributeModelContext = new BO.AttributeModelContext();
                attributeModelContext.ApplySecurity = false;
                attributeModelContext.ApplySorting = false;
                attributeModelContext.Locales = dataLocales;
                attributeModelContext.AttributeModelType = AttributeModelType.AttributeMaster;

                if (dataModelExportContext.ContainerIdList != null && dataModelExportContext.ContainerIdList.Count > 0)
                {
                    foreach (Int32 containerId in dataModelExportContext.ContainerIdList)
                    {
                        attributeModelContext.ContainerId = containerId;
                        Collection<Int32> attributeIdList = attributeModelBL.GetMappedAttributesIdsForContainers(dataModelExportContext.ContainerIdList, callerContext);
                        attributeModels = attributeModelBL.Get(attributeIdList, null, null, attributeModelContext);

                        foreach (BO.AttributeModel attributeModel in attributeModels)
                        {
                            if (attributeModel.Id >= 4000 && !filteredAttributeModels.Contains(attributeModel))
                            {
                                if (dataModelExportContext.ExcludeNonTranslatedModelData && !attributeModel.HasLocaleProperties)
                                {
                                    continue;
                                }
                                filteredAttributeModels.Add(attributeModel);
                            }
                        }
                    }
                }
                else
                {
                    attributeModels = attributeModelBL.Get(attributeModelContext);

                    foreach (BO.AttributeModel attributeModel in attributeModels)
                    {
                        if (attributeModel.Id >= 4000 && !filteredAttributeModels.Contains(attributeModel))
                        {
                            if (dataModelExportContext.ExcludeNonTranslatedModelData && !attributeModel.HasLocaleProperties)
                            {
                                continue;
                            }
                            filteredAttributeModels.Add(attributeModel);
                        }
                    }
                }
            }

            return filteredAttributeModels;
        }

        protected override void PopulateDataModelObjectInDataRow(IDataModelObject dataModelObject, List<String> headerList, Row dataRow)
        {
            UInt32 columnIndex = 1;

            PopulateDataModelBasePropertiesInDataRow(dataModelObject, headerList, dataRow, ref columnIndex);

            BO.AttributeModel attributeModel = (BO.AttributeModel)dataModelObject;

            if (headerList.Contains(DataModelDictionary.AttributeModelLocalizationDictionary[DataModelAttributeModelLocalization.AttributePath]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, GetAttributePath(attributeModel.AttributeParentName, attributeModel.Name));
            }

            if (headerList.Contains(DataModelDictionary.AttributeModelLocalizationDictionary[DataModelAttributeModelLocalization.LocaleName]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, attributeModel.Locale);
            }

            if (headerList.Contains(DataModelDictionary.AttributeModelLocalizationDictionary[DataModelAttributeModelLocalization.AttributeLongName]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, (attributeModel.HasLocaleProperties) ? attributeModel.LongName : RSExcelConstants.BlankText);
            }

            if (headerList.Contains(DataModelDictionary.AttributeModelLocalizationDictionary[DataModelAttributeModelLocalization.MinimumLength]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, (attributeModel.HasLocaleProperties) ? attributeModel.MinLength.ToString() : String.Empty);
            }

            if (headerList.Contains(DataModelDictionary.AttributeModelLocalizationDictionary[DataModelAttributeModelLocalization.MaximumLength]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, (attributeModel.HasLocaleProperties) ? attributeModel.MaxLength.ToString() : String.Empty);
            }

            if (headerList.Contains(DataModelDictionary.AttributeModelLocalizationDictionary[DataModelAttributeModelLocalization.Definition]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, (attributeModel.HasLocaleProperties) ? attributeModel.Definition : String.Empty);
            }

            if (headerList.Contains(DataModelDictionary.AttributeModelLocalizationDictionary[DataModelAttributeModelLocalization.AttributeExample]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, (attributeModel.HasLocaleProperties) ? attributeModel.AttributeExample : String.Empty);
            }

            if (headerList.Contains(DataModelDictionary.AttributeModelLocalizationDictionary[DataModelAttributeModelLocalization.BusinessRule]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, (attributeModel.HasLocaleProperties) ? attributeModel.BusinessRule : String.Empty);
            }
        }

        protected override ObjectType ObjectType
        {
            get { return ObjectType.AttributeModelLocalization; }
        }

        #endregion
    }
}