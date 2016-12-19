using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace MDM.Utility
{
    using MDM.BusinessObjects;
    using MDM.Core;

    /// <summary>
    /// 
    /// </summary>
    public class LookupUtility
    {
        private const String maskSpecialChars = "{}[]";

        /// <summary>
        /// This regex will find out all string between '[' and ']' characters. Example : [Code],[Value] 
        /// (?-\[) - is preceded by a '[' that is not captured (look behind).
        /// .*? - is preceded by a '[' that is not captured (look behind).
        /// (?=\]) - is followed by a ']' that is not captured
        /// </summary>
        private readonly static Regex _regEx = new Regex(@"(?<=\[).*?(?=\])", RegexOptions.Compiled);

        private static readonly Regex LiteralPattern = new Regex(@"(?<={).*?(?=})", RegexOptions.Compiled);

        /// <summary>
        /// Constructs Attribute Lookup Data based on attribute Model and lookup meta data
        /// </summary>
        /// <param name="attributeModel">Indicates attributeModel based on what lookup data should have to be constructed</param>
        /// <param name="lookupMetadata">Indicates lookup meta data </param>
        /// <param name="returnOnlyDisplayColumns">Indicates either display all columns from meta data or only display columns as given in attribute model</param>
        /// <returns>Returns Lookup data with display columns, display format, search columns, export format columns</returns>
        public static Lookup ConstructAttributeLookupData(AttributeModel attributeModel, Lookup lookupMetadata, Boolean returnOnlyDisplayColumns = false)
        {
            Lookup attrLookup = new Lookup();

            if (attributeModel != null && lookupMetadata != null)
            {
                #region Initial Setup

                //Get attribute lookup details..
                List<String> returnColumns = new List<String>();
                List<String> searchColumns = new List<String>();

                String exportMask = attributeModel.ExportMask;

                ColumnCollection dataColumns = lookupMetadata.Columns;

                List<String> displayColumnList = GetColumnsFromFormattedString(attributeModel.LkDisplayColumns, false);

                if (!returnOnlyDisplayColumns && dataColumns != null && dataColumns.Count > 0)
                {
                    foreach (Column column in dataColumns)
                    {
                        if (column.Name.ToLowerInvariant() != "id")
                        {
                            returnColumns.Add(column.Name);
                            searchColumns.Add(column.Name);
                        }
                    }
                }
                else
                {
                    returnColumns = displayColumnList;
                    searchColumns = GetColumnsFromFormattedString(attributeModel.LkSearchColumns, false);
                }

                List<String> displayFormatColumns = GetColumnsFromFormattedString(attributeModel.LkDisplayFormat, true);
                List<String> sortColumns = GetColumnsFromFormattedString(attributeModel.LkSortOrder, false);
                List<String> exportFormatColumns = GetExportMaskColumnsFromFormattedString(exportMask);
                Dictionary<String, String> refLookupColumns = new Dictionary<String, String>();
                LookupRelationshipCollection lookupRelationships = lookupMetadata.LookupRelationships;
                Boolean loadFromLkpRelationship = false;    //Added this for to avoid the performance hit.

                if (lookupRelationships != null && lookupRelationships.Count > 0)
                {
                    foreach (LookupRelationship relationship in lookupRelationships)
                    {
                        String relationshipColumnName = relationship.ColumnName;
                        if (returnColumns != null && returnColumns.Contains(relationshipColumnName) ||
                            exportFormatColumns != null && exportFormatColumns.Contains(relationshipColumnName) ||
                            searchColumns != null && searchColumns.Contains(relationshipColumnName))
                        {
                            var refColumnName = String.Format(Constants.LOOKUP_RELATIONSHIP_COLUMN_NAME_FORMAT, relationship.RefTableName, relationshipColumnName);
                            refLookupColumns.Add(relationshipColumnName, refColumnName);
                            loadFromLkpRelationship = true;
                        }
                    }
                }

                #endregion

                //Start constructing lookup attribute data..
                attrLookup = new Lookup();
                attrLookup.Name = lookupMetadata.Name;
                attrLookup.LongName = lookupMetadata.LongName;
                attrLookup.AttributeId = attributeModel.Id;
                attrLookup.Locale = lookupMetadata.Locale;
                attrLookup.Id = lookupMetadata.Id;
                attrLookup.DisplayColumnList = displayColumnList;

                //Create model
                if (returnColumns != null && returnColumns.Count > 0)
                {
                    #region Adding Id Column

                    Column metadataIdColumn = lookupMetadata.Columns["Id"];

                    if (metadataIdColumn != null)
                    {
                        attrLookup.Columns.Add(new Column(metadataIdColumn.ToXml()));
                    }

                    #endregion

                    #region Adding meta data columns

                    foreach (String column in returnColumns)
                    {
                        //Find this column in lookup meta data
                        Column metadataColumn = lookupMetadata.Columns[column];

                        if (metadataColumn != null)
                            attrLookup.Columns.Add(new Column(metadataColumn.ToXml()));
                    }

                    #endregion

                    if (attrLookup.Columns.Count > 0)
                    {
                        #region Adding format columns

                        //Add display format
                        Column displayFormatColumn = new Column(0, Lookup.DisplayFormatColumnName, Lookup.DisplayFormatColumnName, null);

                        //Add search column
                        Column searchColumn = new Column(0, Lookup.SearchDataColumnName, Lookup.SearchDataColumnName, null);

                        //Add export format
                        Column exportFormatColumn = new Column(0, Lookup.ExportFormatColumnName, Lookup.ExportFormatColumnName, null);

                        String containerIdList = Constants.LOOKUP_CONTEXT_CONTAINER_ID_LIST_COLUMN_NAME;
                        String organizationIdList = Constants.LOOKUP_CONTEXT_ORGANIZATION_ID_LIST_COLUMN_NAME;
                        String categoryPathList = Constants.LOOKUP_CONTEXT_CATEGORY_PATH_LIST_COLUMN_NAME;

                        if (!attrLookup.Columns.Contains(containerIdList) && lookupMetadata.Columns.Contains(containerIdList))
                        {
                            Column containerIdListColumn = new Column(0, containerIdList, containerIdList, null);
                            attrLookup.Columns.Add(containerIdListColumn);
                        }

                        if (!attrLookup.Columns.Contains(organizationIdList) && lookupMetadata.Columns.Contains(organizationIdList))
                        {
                            Column organizationIdListColumn = new Column(0, organizationIdList, organizationIdList, null);
                            attrLookup.Columns.Add(organizationIdListColumn);
                        }

                        if (!attrLookup.Columns.Contains(categoryPathList) && lookupMetadata.Columns.Contains(categoryPathList))
                        {
                            Column categoryPathListColumn = new Column(0, categoryPathList, categoryPathList, null);
                            attrLookup.Columns.Add(categoryPathListColumn);
                        }

                        attrLookup.Columns.Add(displayFormatColumn);
                        attrLookup.Columns.Add(searchColumn);
                        attrLookup.Columns.Add(exportFormatColumn);

                        #endregion

                        if (lookupMetadata.Rows != null && lookupMetadata.Rows.Count > 0)
                        {
                            #region Sorting of data

                            if (sortColumns != null && sortColumns.Count > 0)
                            {
                                List<Row> rows = lookupMetadata.Rows.ToList<Row>();
                                IOrderedEnumerable<Row> orderedQuery = null;
                                Int32 i = 0;

                                foreach (String column in sortColumns)
                                {
                                    Func<Row, Object> expression = row => row[column];
                                    orderedQuery = (i == 0) ? rows.OrderBy(expression) : orderedQuery.ThenBy(expression);
                                    i++;
                                }

                                if (orderedQuery != null)
                                {
                                    rows = orderedQuery.ToList<Row>();
                                }

                                lookupMetadata.Rows = new RowCollection(rows);
                            }

                            #endregion

                            #region Adding row data

                            foreach (Row metadataRow in lookupMetadata.Rows)
                            {
                                Row row = attrLookup.NewRow(metadataRow.Id);

                                //Add Id column
                                row.SetValue("Id", metadataRow.GetValue("Id"));

                                #region Adding metadata columns data

                                foreach (String column in returnColumns)
                                {
                                    String columnName = column;
                                    if (loadFromLkpRelationship && refLookupColumns.TryGetValue(column, out columnName))
                                    {
                                        row.SetValue(column, metadataRow.GetValue(columnName));
                                    }
                                    else
                                    {
                                        row.SetValue(column, metadataRow.GetValue(column));
                                    }

                                    //Set whether the value is system locale value or not
                                    Cell metadataCell = metadataRow.GetCell(column);

                                    if (metadataCell != null)
                                    {
                                        //Get current cell
                                        Cell curCell = row.GetCell(column);

                                        if (curCell != null)
                                            curCell.IsSystemLocaleValue = metadataCell.IsSystemLocaleValue;
                                    }
                                }

                                #endregion

                                #region Adding display format

                                String displayFormat = String.Empty;

                                if (displayFormatColumns != null && displayFormatColumns.Count > 0)
                                {
                                    String columnSeparator = displayFormatColumns[displayFormatColumns.Count - 1]; //Last element is always column separator

                                    foreach (String column in displayFormatColumns)
                                    {
                                        if (column != columnSeparator)
                                        {
                                            String columnName = String.Empty;
                                            Object value = null;

                                            if (loadFromLkpRelationship && refLookupColumns.TryGetValue(column, out columnName))
                                            {
                                                value = metadataRow.GetValue(columnName);
                                            }
                                            else
                                            {
                                                value = metadataRow.GetValue(column);
                                            }

                                            if (value != null && !String.IsNullOrWhiteSpace(value.ToString()))
                                            {
                                                if (displayFormat != String.Empty)
                                                    displayFormat += columnSeparator;

                                                displayFormat += value.ToString();
                                            }
                                        }
                                    }
                                }

                                row.SetValue(Lookup.DisplayFormatColumnName, displayFormat);

                                #endregion

                                #region Adding search columns data

                                String searchColumnData = String.Empty;

                                if (searchColumns != null && searchColumns.Count > 0)
                                {
                                    foreach (String column in searchColumns)
                                    {
                                        String columnName = String.Empty;
                                        Object value = null;

                                        if (loadFromLkpRelationship && refLookupColumns.TryGetValue(column, out columnName))
                                        {
                                            value = metadataRow.GetValue(columnName);
                                        }
                                        else
                                        {
                                            value = metadataRow.GetValue(column);
                                        }

                                        if (value != null)
                                        {
                                            if (searchColumnData != String.Empty)
                                                searchColumnData += "'";

                                            searchColumnData = String.Format("{0}@#@{1}@#@", searchColumnData, value.ToString());
                                        }
                                    }
                                }

                                row.SetValue(Lookup.SearchDataColumnName, searchColumnData);

                                #endregion

                                #region Adding export format

                                //Replace '{' and '}' characters from export mask if model has configured any static text.
                                exportMask = exportMask.Replace("{", "").Replace("}", "");

                                row.SetValue(Lookup.ExportFormatColumnName, LookupUtility.Get(lookupMetadata, exportFormatColumns, metadataRow, exportMask, refLookupColumns, loadFromLkpRelationship));

                                #endregion

                                #region Adding Context for Container,Category,Organization

                                if (lookupMetadata.Columns.Contains("ContainerIdList"))
                                {
                                    row.SetValue("ContainerIdList", metadataRow.GetValue("ContainerIdList"));
                                }

                                if (lookupMetadata.Columns.Contains("CategoryPathList"))
                                {
                                    row.SetValue("CategoryPathList", metadataRow.GetValue("CategoryPathList"));
                                }

                                if (lookupMetadata.Columns.Contains("OrganizationIdList"))
                                {
                                    row.SetValue("OrganizationIdList", metadataRow.GetValue("OrganizationIdList"));
                                }

                                #endregion
                            }

                            #endregion
                        }
                    }
                }
            }

            return attrLookup;
        }

        /// <summary>
        /// Validates export mask formated string
        /// </summary>
        /// <param name="exportMask">Export mask formated string</param>
        /// <returns>Returns <b>true</b> if export mask format is invalid</returns>
        public static Boolean IsExportMaskInvalid(String exportMask)
        {
            if (String.IsNullOrWhiteSpace(exportMask))
            {
                throw new ArgumentNullException("exportMask");
            }

            List<String> columnList = GetMatchedValues(_regEx, exportMask);
            List<String> literalList= GetMatchedValues(LiteralPattern, exportMask);
            if (columnList.Count == 0)
            {
                return true;
            }

            String collectionSeparator = AppConfigurationHelper.GetAppConfig<String>("MDM.Exports.RSExcelFormatter.CollectionSeparator", String.Empty).Trim();
            return HasInvalidChars(columnList, maskSpecialChars) || HasInvalidChars(literalList, String.Concat(maskSpecialChars, collectionSeparator));
        }

        private static List<String> GetMatchedValues(Regex regex, String input)
        {
            List<String> values = new List<String>();
            MatchCollection matches = regex.Matches(input);
            foreach (Match match in matches)
            {
                if (match.Success && match.Groups.Count > 0)
                {
                    foreach (Group group in match.Groups)
                    {
                        values.Add(group.Value.Trim());
                    }
                }
            }
            return values;
        }

        private static Boolean HasInvalidChars(List<String> inputList, String invalidChars)
        {
            foreach (String value in inputList)
            {
                foreach (Char c in value)
                {
                    if (invalidChars.Contains(c))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private static List<String> GetColumnsFromFormattedString(String formattedString, Boolean addSeparator)
        {
            List<String> columnList = null;

            if (!String.IsNullOrWhiteSpace(formattedString))
            {
                String columnSeparator = String.Empty;

                //Remove start and end wrappers..
                formattedString = formattedString.Remove(0, 1);
                formattedString = formattedString.Remove(formattedString.Count() - 1, 1);

                Int32 firstColumnEndIndex = formattedString.IndexOf("]");

                if (firstColumnEndIndex > 0)
                {
                    Int32 secondColumnStartIndex = formattedString.IndexOf('[', firstColumnEndIndex);

                    if (secondColumnStartIndex > firstColumnEndIndex)
                    {
                        Int32 columnSeparatorStartPosition = firstColumnEndIndex + 1;
                        Int32 columnSeparatorEndPosition = secondColumnStartIndex - columnSeparatorStartPosition;

                        columnSeparator = formattedString.Substring(columnSeparatorStartPosition, columnSeparatorEndPosition);
                    }
                }

                String splitString = "]" + columnSeparator + "[";

                String[] columns = formattedString.Split(splitString.ToArray(), StringSplitOptions.RemoveEmptyEntries);

                if (columns != null && columns.Count() > 0)
                {
                    columnList = columns.ToList();

                    if (addSeparator)
                        columnList.Add(columnSeparator); //The last record is always column separator
                }
            }

            return columnList;
        }

        /// <summary>
        /// Returns Export Mask Columns from the Formated String
        /// </summary>
        /// <param name="formattedString">Formatted String(Export Mask) provided by the user</param>
        /// <returns>Export Mask Columns</returns>
        public static List<String> GetExportMaskColumnsFromFormattedString(String formattedString)
        {
            List<String> columnList = null;

            if (!String.IsNullOrWhiteSpace(formattedString))
            {
                columnList = GetMatchedValues(_regEx, formattedString.Trim());
            }

            return columnList;
        }

        private static String Get(Lookup lookupMetadata, List<String> columns, Row metaDataRow, String formattedString, Dictionary<String, String> refLookupColumns, Boolean loadFromLkpRelationship)
        {
            String formattedStringWithData = formattedString;

            if (columns != null && columns.Count > 0)
            {
                foreach (String column in columns)
                {
                    if (!String.IsNullOrWhiteSpace(column))
                    {
                        Object value = null;
                        String columnName = column;

                        if (loadFromLkpRelationship && !refLookupColumns.TryGetValue(column, out columnName))
                        {
                            columnName = column;
                        }

                        //If the lookup metadata contains the column mentioned in the export mask, then fetch the value.
                        if (lookupMetadata.Columns.Contains(columnName))
                        {
                            value = metaDataRow.GetValue(columnName);
                        }
                        else
                        {
                            if (Constants.TRACING_ENABLED)
                            {
                                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Not able to get ExportMask Value. No Column found with column Name : {0} for lookup {1}", column, lookupMetadata.Name), MDMTraceSource.LookupGet);
                            }
                        }

                        //If value is null, then formatting of string is not required.
                        if (value != null)
                        {
                            String val = value as String;
                            formattedStringWithData = formattedStringWithData.Replace(LookupUtility.AppendSquareBrackets(column), val);
                        }
                    }
                }
            }

            return formattedStringWithData;
        }

        /// <summary>
        /// Wraps column name with square brackets 
        /// </summary>
        /// <param name="columnName">Indicates the name of the column to be wrapped</param>
        /// <returns>Column name Wrapped with square brackets </returns>
        public static String AppendSquareBrackets(String columnName)
        {
            return String.Concat("[", columnName, "]");
        }
    }
}