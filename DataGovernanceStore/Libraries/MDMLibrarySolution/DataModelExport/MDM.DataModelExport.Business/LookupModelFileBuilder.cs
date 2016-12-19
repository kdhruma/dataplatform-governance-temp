using System;
using System.Collections.Generic;
using DocumentFormat.OpenXml.Spreadsheet;

namespace MDM.DataModelExport.Business
{
    using MDM.Core;
    using MDM.Core.DataModel;
    using MDM.ExcelUtility;
    using MDM.Interfaces;
    using MDM.DataModelManager.Business;
    using MDM.BusinessObjects.DynamicTableSchema;
    using BO = MDM.BusinessObjects;
    using MDM.BusinessObjects.Exports;

    /// <summary>
    /// Class provides functionality to export lookup model data as an excel file.
    /// </summary>
    internal class LookupModelFileBuilder : DataModelFileBuilderBase
    {
        #region Overridden Methods

        protected override IDataModelObjectCollection GetDataModelObjectCollection(BO.CallerContext callerContext)
        {
            DBTableCollection dbTables = new DBTableCollection();
            DBTableCollection alldbTables = new DBTableCollection();
            DynamicTableSchemaBL dynamicTableSchemaBL = new DynamicTableSchemaBL();

            //Since internal columns should not loaded in exported file, so passing includeInternalColumn parameter as false;
            dbTables = dynamicTableSchemaBL.GetAll(DynamicTableType.Lookup, callerContext, false);

            if (dbTables != null && dbTables.Count > 0)
            {
                foreach (DBTable dbTable in dbTables)
                {
                    foreach (DBColumn dbColumn in dbTable.Columns)
                    {
                        DBTable clonedLookupModel = (DBTable)dbTable.Clone();

                        clonedLookupModel.Columns = new DBColumnCollection() { dbColumn };
                        alldbTables.Add(clonedLookupModel);
                    }
                }
            }

            return alldbTables;
        }

        protected override IDataModelObjectCollection GetDataModelObjectCollection(DataModelExportContext dataModelExportContext, BO.CallerContext callerContext)
        {
            DBTableCollection dbTables = new DBTableCollection();
            DBTableCollection alldbTables = new DBTableCollection();
            DynamicTableSchemaBL dynamicTableSchemaBL = new DynamicTableSchemaBL();

            //Since internal columns should not loaded in exported file, so passing includeInternalColumn parameter as false;
            if (dataModelExportContext.LookupTableNames != null && dataModelExportContext.LookupTableNames.Count > 0)
            {
                dbTables = dynamicTableSchemaBL.GetLookupsByNames(dataModelExportContext.LookupTableNames, callerContext, false);

                if (dbTables != null && dbTables.Count > 0)
                {
                    foreach (DBTable dbTable in dbTables)
                    {
                        foreach (DBColumn dbColumn in dbTable.Columns)
                        {
                            DBTable clonedLookupModel = (DBTable)dbTable.Clone();

                            clonedLookupModel.Columns = new DBColumnCollection() { dbColumn };
                            alldbTables.Add(clonedLookupModel);
                        }
                    }
                }
            }
            else
            {
                return GetDataModelObjectCollection(callerContext);
            }

            return alldbTables;
        }

        protected override void PopulateDataModelObjectInDataRow(IDataModelObject dataModelObject, List<String> headerList, Row dataRow)
        {
            UInt32 columnIndex = 1;

            DBTable lookupModel = (DBTable)dataModelObject;

            if(lookupModel != null)
            {
                foreach(DBColumn column in lookupModel.Columns)
                {
                    PopulateDataModelBasePropertiesInDataRow(dataModelObject, headerList, dataRow, ref columnIndex);

                    if (headerList.Contains(DataModelDictionary.LookupModelDictionary[DataModelLookupModel.TableName]))
                    {
                        OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, lookupModel.Name);
                    }

                    if (headerList.Contains(DataModelDictionary.LookupModelDictionary[DataModelLookupModel.Sequence]))
                    {
                        OpenSpreadsheetUtility.AppendRowWithNumberCell(dataRow, columnIndex++, column.Sequence);
                    }

                    if (headerList.Contains(DataModelDictionary.LookupModelDictionary[DataModelLookupModel.ColumnName]))
                    {
                        OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, column.Name);
                    }

                    if (headerList.Contains(DataModelDictionary.LookupModelDictionary[DataModelLookupModel.DataType]))
                    {
                        OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, column.DataType);
                    }

                    if (headerList.Contains(DataModelDictionary.LookupModelDictionary[DataModelLookupModel.Width]))
                    {
                        OpenSpreadsheetUtility.AppendRowWithNumberCell(dataRow, columnIndex++, column.Length);
                    }

                    if (headerList.Contains(DataModelDictionary.LookupModelDictionary[DataModelLookupModel.Precision]))
                    {
                        OpenSpreadsheetUtility.AppendRowWithNumberCell(dataRow, columnIndex++, column.Precision);
                    }

                    if (headerList.Contains(DataModelDictionary.LookupModelDictionary[DataModelLookupModel.Nullable]))
                    {
                        OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, ConvertBooleanValuesToString(column.Nullable));
                    }

                    if (headerList.Contains(DataModelDictionary.LookupModelDictionary[DataModelLookupModel.IsUnique]))
                    {
                        OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, ConvertBooleanValuesToString(column.IsUnique));
                    }

                    if (headerList.Contains(DataModelDictionary.LookupModelDictionary[DataModelLookupModel.DefaultValue]))
                    {
                        OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, column.DefaultValue);
                    }

                    if (headerList.Contains(DataModelDictionary.LookupModelDictionary[DataModelLookupModel.CheckConstraint]))
                    {
                        if (lookupModel.Constraints != null && lookupModel.Constraints.Count > 0)
                        {
                            foreach (DBConstraint constraint in lookupModel.Constraints)
                            {
                                if (constraint.ColumnName == column.Name && constraint.ConstraintType == ConstraintType.Check)
                                {
                                    OpenSpreadsheetUtility.AppendRowWithTextCell(dataRow, columnIndex++, constraint.Value);
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }

        protected override ObjectType ObjectType
        {
            get { return ObjectType.LookupModel; }
        }

        #endregion
    }
}
