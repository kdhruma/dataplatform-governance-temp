using System;
using System.Linq;
using System.Collections.Generic;

using DocumentFormat.OpenXml.Spreadsheet;

namespace MDM.DataModelExport.Business
{
    using MDM.Core;
    using MDM.Core.DataModel;
    using MDM.ExcelUtility;
    using MDM.Interfaces;
    using MDM.DataModelManager.Business;
    using BO = MDM.BusinessObjects;
    using MDM.ContainerManager.Business;
    using MDM.AttributeModelManager.Business;

    /// <summary>
    /// Class provides functionality to export EntityVariantDefinition data as an excel file.
    /// </summary>
    internal class EntityVariantDefinitionDataModelFileBuilder : DataModelFileBuilderBase
    {
        protected override IDataModelObjectCollection GetDataModelObjectCollection(BO.CallerContext callerContext)
        {
            BO.EntityVariantDefinitionCollection entityVariantDefinitionCollection = new EntityVariantDefinitionBL(new AttributeModelBL()).GetAll(callerContext);

            BO.EntityVariantDefinitionCollection flattenedCollection = new BO.EntityVariantDefinitionCollection();

            //Prepare the flattened collection of entity variant definition as the export is having one rule attribute per row. 
            //So each entity variant definition will contain only one variant level and one attribute rule. This is done only for data model export.
            foreach (BO.EntityVariantDefinition definition in entityVariantDefinitionCollection)
            {
                foreach (BO.EntityVariantLevel variantLevel in definition.EntityVariantLevels)
                {
                    BO.EntityVariantDefinition clonedDefinition = new BO.EntityVariantDefinition
                    {
                        Id = definition.Id,
                        Name = definition.Name,
                        RootEntityTypeName = definition.RootEntityTypeName,
                        HasDimensionAttributes = definition.HasDimensionAttributes
                    };

                    BO.EntityVariantLevel clonedVariantLevel = new BO.EntityVariantLevel
                    {
                        Rank = variantLevel.Rank,
                        EntityTypeName = variantLevel.EntityTypeName
                    };

                    foreach (BO.EntityVariantRuleAttribute ruleAttribute in variantLevel.RuleAttributes)
                    {
                        BO.EntityVariantRuleAttribute clonedRuleAttribute = new BO.EntityVariantRuleAttribute
                        {
                            SourceAttributeName = ruleAttribute.SourceAttributeName,
                            TargetAttributeName = ruleAttribute.TargetAttributeName,
                            IsOptional = ruleAttribute.IsOptional
                        };

                        clonedVariantLevel.RuleAttributes = new BO.EntityVariantRuleAttributeCollection();
                        clonedVariantLevel.RuleAttributes.Add(clonedRuleAttribute);
                    }

                    clonedDefinition.EntityVariantLevels = new BO.EntityVariantLevelCollection();
                    clonedDefinition.EntityVariantLevels.Add(clonedVariantLevel);

                    flattenedCollection.Add(clonedDefinition);
                }
            }

            return flattenedCollection;
        }

        protected override IDataModelObjectCollection GetDataModelObjectCollection(BO.Exports.DataModelExportContext dataModelExportContext, BO.CallerContext callerContext)
        {
            return GetDataModelObjectCollection(callerContext);
        }

        protected override void PopulateDataModelObjectInDataRow(IDataModelObject dataModelObject, List<String> headerList, Row dataRow)
        {
            UInt32 columnIndex = 1;

            BO.EntityVariantDefinition entityVariantDefinitionObject = (BO.EntityVariantDefinition)dataModelObject;
            BO.EntityVariantLevel entityVariantLevelObject = entityVariantDefinitionObject.EntityVariantLevels.FirstOrDefault();
            BO.EntityVariantRuleAttribute entityVariantRuleAttributeObject = entityVariantLevelObject.RuleAttributes.FirstOrDefault();

            PopulateDataModelBasePropertiesInDataRow(dataModelObject, headerList, dataRow, ref columnIndex);

            if (headerList.Contains(DataModelDictionary.EntityVariantDefinitionDictionary[DataModelEntityVariantDefinition.EntityVariantDefinitionName]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, entityVariantDefinitionObject.Name);
            }

            if (headerList.Contains(DataModelDictionary.EntityVariantDefinitionDictionary[DataModelEntityVariantDefinition.HasDimensionAttributes]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, ConvertBooleanValuesToString(entityVariantDefinitionObject.HasDimensionAttributes));
            }

            if (headerList.Contains(DataModelDictionary.EntityVariantDefinitionDictionary[DataModelEntityVariantDefinition.RootEntityType]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, entityVariantDefinitionObject.RootEntityTypeName);
            }

            if (headerList.Contains(DataModelDictionary.EntityVariantDefinitionDictionary[DataModelEntityVariantDefinition.ChildLevel]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, entityVariantLevelObject.Rank);
            }

            if (headerList.Contains(DataModelDictionary.EntityVariantDefinitionDictionary[DataModelEntityVariantDefinition.ChildEntityType]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, entityVariantLevelObject.EntityTypeName);
            }

            if (headerList.Contains(DataModelDictionary.EntityVariantDefinitionDictionary[DataModelEntityVariantDefinition.SourceAttribute]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, (entityVariantRuleAttributeObject != null) ? entityVariantRuleAttributeObject.SourceAttributeName : String.Empty);
            }

            if (headerList.Contains(DataModelDictionary.EntityVariantDefinitionDictionary[DataModelEntityVariantDefinition.TargetAttribute]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, (entityVariantRuleAttributeObject != null) ? entityVariantRuleAttributeObject.TargetAttributeName : String.Empty);
            }

            if (headerList.Contains(DataModelDictionary.EntityVariantDefinitionDictionary[DataModelEntityVariantDefinition.IsOptional]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, (entityVariantRuleAttributeObject != null) ? ConvertBooleanValuesToString(entityVariantRuleAttributeObject.IsOptional) : String.Empty);
            }

        }

        protected override ObjectType ObjectType
        {
            get { return ObjectType.EntityVariantDefinition; }
        }
    }
}
