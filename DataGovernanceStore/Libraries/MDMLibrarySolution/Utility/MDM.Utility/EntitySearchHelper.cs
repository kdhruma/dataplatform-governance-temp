using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDM.Utility
{
    using MDM.BusinessObjects;
    using MDM.Core;

    /// <summary>
    /// Represents the helper method of entity search
    /// </summary>
    public static class EntitySearchHelper
    {

        /// <summary>
        /// Converts entity collection into data table using search context
        /// </summary>
        /// <param name="entities">Indicates collection of entity</param>
        /// <param name="searchContext">Indicates context of search</param>
        /// <returns>Returns data table of given entities</returns>
        public static DataTable ConvertFromEntityCollectionToDataTable(EntityCollection entities, SearchContext searchContext)
        {
            DataTable dataTable = new DataTable();
            dataTable.TableName = "SearchResult";

            Boolean isCustomSortEnabled = AppConfigurationHelper.GetAppConfig<Boolean>("MDM.EntityExplorerGrid.CustomSort.Enabled", false);
            Boolean isRetrunAttributeListConfigured = searchContext.IsRetrunAttributeListConfigured;

            Collection<Attribute> attributes = searchContext.ReturnAttributeList;

            #region Add Columns to data table

            dataTable.Columns.Add("CNodeID");
            dataTable.Columns.Add("ShortName");
            dataTable.Columns.Add("LongName");
            dataTable.Columns.Add("ParentID");
            dataTable.Columns.Add("ParentName");
            dataTable.Columns.Add("CategoryId");
            dataTable.Columns.Add("CategoryName");
            dataTable.Columns.Add("CatalogId");
            dataTable.Columns.Add("CatalogName");
            dataTable.Columns.Add("OrgID");
            dataTable.Columns.Add("FK_NodeType");
            dataTable.Columns.Add("FK_NodeTypeDesc");
            dataTable.Columns.Add("SearchOrder");
            dataTable.Columns.Add("ALTTaxonomy");
            dataTable.Columns.Add("WorkflowInstanceGroupID");
            dataTable.Columns.Add("PermissionSet");
            dataTable.Columns.Add("WorkflowDetails");
            dataTable.Columns.Add("EntityFamilyLongName");
            dataTable.Columns.Add("EntityGlobalFamilyLongName");

            if (isRetrunAttributeListConfigured)
            {
                foreach (BusinessObjects.Attribute attribute in attributes)
                {
                    String columnName = attribute.Name.Replace(" ", "");

                    dataTable.Columns.Add(columnName);

                    if (isCustomSortEnabled)
                    {
                        dataTable.ExtendedProperties.Add(columnName, attribute.AttributeDataType);
                    }
                }
            }

            #endregion Add Columns to data table

            #region Add Rows to data table

            foreach (Entity entity in entities)
            {
                DataRow dr = dataTable.NewRow();

                dr["CNodeID"] = entity.Id;
                dr["ShortName"] = entity.Name;
                dr["LongName"] = entity.LongName;
                dr["ParentID"] = entity.CategoryId;
                dr["ParentName"] = entity.CategoryLongNamePath;
                dr["CategoryId"] = entity.CategoryId;
                dr["CategoryName"] = entity.CategoryLongName;
                dr["CatalogId"] = entity.ContainerId;
                dr["CatalogName"] = entity.ContainerLongName;
                dr["OrgID"] = entity.OrganizationId;
                dr["FK_NodeType"] = entity.EntityTypeId;
                dr["FK_NodeTypeDesc"] = entity.EntityTypeLongName;
                dr["SearchOrder"] = 0;
                dr["ALTTaxonomy"] = String.Empty;
                dr["WorkflowInstanceGroupID"] = String.Empty;
                dr["PermissionSet"] = ValueTypeHelper.JoinCollection<UserAction>(entity.PermissionSet, ",");
                dr["EntityFamilyLongName"] = entity.EntityFamilyLongName;
                dr["EntityGlobalFamilyLongName"] = entity.EntityGlobalFamilyLongName;

                if (isRetrunAttributeListConfigured)
                {
                    foreach (Attribute attribute in attributes)
                    {
                        String columnName = attribute.Name.Replace(" ", "");

                        Attribute filteredAttribute = (Attribute)entity.GetAttribute(attribute.Id, attribute.Locale);

                        if (filteredAttribute != null)
                        {
                            if (attribute.IsComplex || attribute.IsHierarchical)
                            {
                                continue;
                            }

                            ValueCollection values = (ValueCollection)filteredAttribute.GetCurrentValues(attribute.Locale);

                            if (values != null && values.Count > 0)
                            {
                                String concatenatedValue = String.Empty;

                                foreach (Value value in values)
                                {
                                    if (filteredAttribute.IsLookup)
                                    {
                                        concatenatedValue = String.Concat(concatenatedValue, ",", value.GetDisplayValue());
                                    }
                                    else
                                    {
                                        concatenatedValue = String.Concat(concatenatedValue, ",", value.AttrVal);
                                    }
                                }

                                dr[columnName] = concatenatedValue.Substring(1);
                            }
                        }
                    }
                }

                dataTable.Rows.Add(dr);
            }

            #endregion Add Rows to data table

            return dataTable;
        }

    }
}
