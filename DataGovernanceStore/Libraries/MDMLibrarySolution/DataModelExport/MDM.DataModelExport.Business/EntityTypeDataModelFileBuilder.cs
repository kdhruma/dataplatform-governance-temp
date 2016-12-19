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
    using MDM.DataModelManager.Business;
    using BO = MDM.BusinessObjects;
    using MDM.BusinessObjects.Exports;

    /// <summary>
    /// Class provides functionality to export EntityType data as an excel file.
    /// </summary>
    internal class EntityTypeDataModelFileBuilder : DataModelFileBuilderBase
    {
        #region Overridden Methods

        protected override IDataModelObjectCollection GetDataModelObjectCollection(BO.CallerContext callerContext)
        {
            EntityTypeBL entityTypeBL = new EntityTypeBL();

            BO.EntityTypeCollection entityTypes = entityTypeBL.GetAll(callerContext, false);

            Collection<String> internalentityTypeNames =  MDM.Core.DataModel.InternalObjectCollection.EntityTypeNames;
            BO.EntityTypeCollection filteredEntityTypes = new BO.EntityTypeCollection();

            foreach (BO.EntityType entityType in entityTypes)
            {
                if (!internalentityTypeNames.Contains(entityType.Name.ToLowerInvariant()))
                {
                    filteredEntityTypes.Add(entityType);
                }
            }

            return filteredEntityTypes;
        }

        protected override IDataModelObjectCollection GetDataModelObjectCollection(DataModelExportContext dataModelExportContext, BO.CallerContext callerContext)
        {
            EntityTypeBL entityTypeBL = new EntityTypeBL();
            BO.EntityTypeCollection filteredEntityTypes = new BO.EntityTypeCollection();

            if (dataModelExportContext.EntityTypeIdList != null && dataModelExportContext.EntityTypeIdList.Count > 0)
            {
                BO.EntityTypeCollection entityTypes = entityTypeBL.GetEntityTypesByIds(dataModelExportContext.EntityTypeIdList);

                Collection<String> internalentityTypeNames = MDM.Core.DataModel.InternalObjectCollection.EntityTypeNames;

                if (entityTypes != null)
                {
                    foreach (BO.EntityType entityType in entityTypes)
                    {
                        if (!internalentityTypeNames.Contains(entityType.Name.ToLowerInvariant()) && !filteredEntityTypes.Contains(entityType.Id))
                        {
                            filteredEntityTypes.Add(entityType);
                        }
                    }
                }
            }
            else
            {
                return GetDataModelObjectCollection(callerContext);
            }

            return filteredEntityTypes;
        }

        protected override void PopulateDataModelObjectInDataRow(IDataModelObject dataModelObject, List<String> headerList, Row dataRow)
        {
            UInt32 columnIndex = 1;

            PopulateDataModelBasePropertiesInDataRow(dataModelObject, headerList, dataRow, ref columnIndex);
            
            BO.EntityType entityType = (BO.EntityType)dataModelObject;
            
            if (headerList.Contains(DataModelDictionary.EntityTypeDictionary[DataModelEntityType.EntityTypeName]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, entityType.Name);
            }

            if (headerList.Contains(DataModelDictionary.EntityTypeDictionary[DataModelEntityType.EntityTypeLongName]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, entityType.LongName);
            }
        }

        protected override ObjectType ObjectType
        {
            get { return ObjectType.EntityType; }
        }

        #endregion
    }
}
