using MDM.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DocumentFormat.OpenXml.Spreadsheet;

namespace MDM.DataModelExport.Business
{
    using MDM.Core;
    using MDM.Core.DataModel;
    using MDM.DataModelManager.Business;
    using MDM.ExcelUtility;
    using BO = MDM.BusinessObjects;
    using MDM.ContainerManager.Business;
    using MDM.CategoryManager.Business;

    /// <summary>
    /// Class provides functionality to export entity variant definition mapping data model as an excel file.
    /// </summary>
    internal class EntityVariantDefinitionMappingFileBuilder : DataModelFileBuilderBase
    {
        protected override IDataModelObjectCollection GetDataModelObjectCollection(BO.CallerContext callerContext)
        {
            EntityVariantDefinitionMappingBL entityVariantDefinitionMappingManager = new EntityVariantDefinitionMappingBL(new ContainerBL(), new CategoryBL());

            return entityVariantDefinitionMappingManager.GetAll(callerContext);
        }

        protected override IDataModelObjectCollection GetDataModelObjectCollection(BO.Exports.DataModelExportContext dataModelExportContext, BO.CallerContext callerContext)
        {
            return GetDataModelObjectCollection(callerContext);
        }

        protected override void PopulateDataModelObjectInDataRow(IDataModelObject dataModelObject, List<string> headerList, Row dataRow)
        {
            UInt32 columnIndex = 1;

            PopulateDataModelBasePropertiesInDataRow(dataModelObject, headerList, dataRow, ref columnIndex);

            BO.EntityVariantDefinitionMapping mappingObject = (BO.EntityVariantDefinitionMapping)dataModelObject;

            if (headerList.Contains(DataModelDictionary.EntityVariantDefinitionMappingDictionary[DataModelEntityVariantDefinitionMapping.EntityVariantDefinitionName]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, mappingObject.EntityVariantDefinitionName);
            }

            if (headerList.Contains(DataModelDictionary.EntityVariantDefinitionMappingDictionary[DataModelEntityVariantDefinitionMapping.ContainerName]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, mappingObject.ContainerName);
            }

            if (headerList.Contains(DataModelDictionary.EntityVariantDefinitionMappingDictionary[DataModelEntityVariantDefinitionMapping.CategoryPath]))
            {
                OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, mappingObject.CategoryPath);
            }
        }

        protected override ObjectType ObjectType
        {
            get { return MDM.Core.ObjectType.EntityVariantDefinitionMapping; }
        }
    }
}