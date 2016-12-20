using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Xml;
using System.Text;
using System.Linq;
using System.Transactions;
using System.Collections.ObjectModel;
using Microsoft.SqlServer.Server;
using System.Collections.Generic;
using System.Globalization;
using System.Diagnostics;

namespace MDM.AttributeManager.Data
{
    using MDM.Core;
    using MDM.BusinessObjects;
    using MDM.Utility;

    using MDM.Interfaces;

    /// <summary>
    /// 
    /// </summary>
    public class AttributeDA : SqlClientDataAccessBase
    {
        #region Fields
        #endregion

        #region Constructors
        #endregion

        #region Properties
        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Gives the string representing AttributeName - Value string for Breadcrumb.
        /// Can return datatable representing AttributeName - Value string for multiple UserConfigType like Breadcrumb , Title etc.
        /// </summary>
        /// <param name="breadcrumbConfigXML">
        /// XML representing AttributeName - ID and some additional attributes needed
        /// Sample XML : 
        ///         <UserConfigs>
        ///           <UserConfig UserConfigType="Breadcrumb">
        ///            <Attribute Name="localizable Decimal Apply locale format" ID="110099" SelectedSeqNo="1" Selected="true" LabelSeperator="&gt;&gt;" />
        ///             <Attribute Name="Product ID" ID="109838" SelectedSeqNo="2" Selected="true" LabelSeperator="&gt;&gt;" />
        ///             <Attribute Name="FH_BooleanDropdown Attribute" ID="109778" SelectedSeqNo="3" Selected="true" LabelSeperator="&gt;&gt;" />
        ///           </UserConfig>
        ///           <UserConfig UserConfigType="Title">
        ///             <Attribute Name="localizable Decimal Apply locale format" ID="110099" SelectedSeqNo="1" Selected="true" LabelSeperator="&gt;&gt;" />
        ///             <Attribute Name="Product ID" ID="109838" SelectedSeqNo="2" Selected="true" LabelSeperator="&gt;&gt;" />
        ///             <Attribute Name="FH_BooleanDropdown Attribute" ID="109778" SelectedSeqNo="3" Selected="true" LabelSeperator="&gt;&gt;" />
        ///           </UserConfig>
        ///         </UserConfigs>
        /// </param>
        /// <param name="FK_Org">FK_Org of an item</param>
        /// <param name="FK_Catalog">FK_Catalog of an item</param>
        /// <param name="FK_CNode">CNodeID of an item</param>
        /// <param name="FK_Locale">FK_Locale of an Item</param>
        /// <param name="UserID">ID of currently logged in user</param>
        /// <returns>
        ///     Datatable containing 2 columns 1. UserConfigType 2.AttributeName - value string
        ///     No. of rows in DataTable = No. of UserConfigType in input XML
        /// </returns>
        public DataTable GetBreadcrumbAttributeValueString(String breadcrumbConfigXML, Int32 FK_Org, Int32 FK_Catalog, Int64 FK_CNode, Int32 FK_Locale, Int32 UserID)
        {
            DataTable result = null;

            SqlParameter[] parameters;
            SqlDataReader reader = null;
            String connectionString = String.Empty;
            String storedProcedureName = String.Empty;

            connectionString = AppConfigurationHelper.ConnectionString;

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("AttributeManager_SqlParameters");

                parameters = generator.GetParameters("AttributeManager_Attribute_BreadcrumbAttributeValue_Get_ParametersArray");

                parameters[0].Value = breadcrumbConfigXML;
                parameters[1].Value = FK_Org;
                parameters[2].Value = FK_Catalog;
                parameters[3].Value = FK_CNode;
                parameters[4].Value = FK_Locale;
                parameters[5].Value = UserID;

                storedProcedureName = "usp_AttributeManager_Attribute_BreadcrumbAttributeValue_Get";

                reader = ExecuteProcedureReader(connectionString, parameters, storedProcedureName);

                if (reader != null)
                {
                    result = new DataTable();
                    result.Load(reader);
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fromCnodeId"></param>
        /// <param name="toCnodeId"></param>
        /// <param name="fromCatalogId"></param>
        /// <param name="toCatalogId"></param>
        /// <param name="comAttributes"></param>
        /// <param name="techAttributes"></param>
        /// <param name="relationships"></param>
        /// <param name="userId"></param>
        /// <param name="programName"></param>
        /// <param name="coreUpdateCount"></param>
        /// <param name="techUpdateCount"></param>
        /// <param name="partCount"></param>
        /// <returns></returns>
        public String CopyPasteContent(Int32 fromCnodeId, Int32 toCnodeId, Int32 fromCatalogId, Int32 toCatalogId, String comAttributes, String techAttributes, String relationships, String userId, String programName, ref Int32 coreUpdateCount, ref Int32 techUpdateCount, ref Int32 partCount)
        {
            StringBuilder returnValue = new StringBuilder();

            SqlParameter[] parameters;
            String connectionString = String.Empty;
            String storedProcedureName = String.Empty;
            SqlDataReader reader = null;

            connectionString = AppConfigurationHelper.ConnectionString;

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("AttributeManager_SqlParameters");

                parameters = generator.GetParameters("AttributeManager_Attribute_CopyPasteContent_ParametersArray");

                parameters[0].Value = fromCnodeId ;
                parameters[1].Value = toCnodeId ;
                parameters[2].Value = fromCatalogId ;
                parameters[3].Value = toCatalogId ;
                parameters[4].Value = comAttributes ;
                parameters[5].Value = techAttributes ;
                parameters[6].Value = relationships ;
                parameters[7].Value = userId ;
                parameters[8].Value = programName ;

                storedProcedureName = "usp_AttributeManager_Attribute_CopyPasteContent";

                reader = ExecuteProcedureReader(connectionString, parameters, storedProcedureName);

                while (reader.Read())
                {
                    if (!reader.IsDBNull(0))
                    {
                        returnValue.Append(reader[0]);
                    }
                }

                reader.NextResult();

                while (reader.Read())
                {
                    if (!reader.IsDBNull(0))
                    {
                        Int32.TryParse(reader["CoreUpdateCount"].ToString(), out coreUpdateCount );
                        Int32.TryParse(reader["TechUpdateCount"].ToString(), out techUpdateCount );
                        Int32.TryParse(reader["PartCount"].ToString(), out partCount);
                    }
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
            return returnValue.ToString();
        }

        /// <summary>
        /// Get Complex Data for Complex Attribute's version history based on attribute id, auditRefId and locale provided.
        /// </summary>
        /// <param name="entityId">EntityId for which attribute history is needed</param>
        /// <param name="containerId">Container Id  under which Entity is created</param>
        /// <param name="attributeId">Attribute id for which we needs data</param>
        /// <param name="auditRefId">AuditRefId for which we needs data</param>
        /// <param name="locale">locale details</param>
        /// <param name="command">command object which contains all the db info like connection string</param>
        /// <returns>Attribute object with complex attribute's data</returns>
        public Attribute GetComplexDataByAuditRefId(Int64 entityId, Int32 containerId, Int32 attributeId, Int64 auditRefId, LocaleEnum locale, DBCommandProperties command, AttributeModel attributeModel)
        {
            if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity("AttributeManager.AttributeDA.GetComplexData", MDMTraceSource.AttributeGet, false);
            }

            SqlParameter[] parameters;
            String storedProcedureName = String.Empty;
            SqlDataReader reader = null;
            Attribute attribute = null;
            Dictionary<Int32, AttributeCollection> childAttributes = null;

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("AttributeManager_SqlParameters");

                parameters = generator.GetParameters("AttributeManager_ComplexData_Get_ParametersArray");

                parameters[0].Value = entityId;
                parameters[1].Value = containerId;
                parameters[2].Value = auditRefId;
                parameters[3].Value = attributeId;
                parameters[4].Value = locale;

                storedProcedureName = "usp_AttributeManager_ComplexData_Get";

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                if (reader != null)
                {
                    childAttributes = new Dictionary<Int32, AttributeCollection>();
                    Int32 instanceRefIdForInvalidRecords = -1;

                    while (reader.Read())
                    {
                        #region Construct Child Attribute
                        Int32 attributeParentId = 0;
                        Int32 childAttributeId = 0;
                        Int32 instanceRefId = 0;
                        Int32 uomId = 0;
                        String uom = String.Empty;
                        String attrVal = String.Empty;
                        LocaleEnum attributeLocale = LocaleEnum.UnKnown;
                        AttributeModel childAttributeModel = new AttributeModel();
                        Attribute childAttribute = null;

                        if (reader["FK_AttributeParent"] != null)
                            attributeParentId = ValueTypeHelper.Int32TryParse(reader["FK_AttributeParent"].ToString(), -1);

                        if (reader["FK_Attribute"] != null)
                            childAttributeId = ValueTypeHelper.Int32TryParse(reader["FK_Attribute"].ToString(), -1);

                        if (attributeModel != null)
                        {
                            AttributeModelCollection childAttributeModels = attributeModel.AttributeModels;
                            if (childAttributeModels != null && childAttributeModels.Count > 0)
                            {
                                childAttributeModel = (AttributeModel)childAttributeModels.GetAttributeModel(ValueTypeHelper.ConvertToInt32(childAttributeId), locale);
                                childAttribute = new Attribute(childAttributeModel);
                            }
                            else
                            {
                                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, "Child Attribute Models is not available.", MDMTraceSource.AttributeGet);
                            }
                        }
                        else
                        {
                            MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, "Attribute Model is not available.", MDMTraceSource.AttributeGet);
                        }

                        if (reader["InstanceRefId"] != null)
                            instanceRefId = ValueTypeHelper.Int32TryParse(reader["InstanceRefId"].ToString(), -1);

                        if (reader["FK_UOM"] != null)
                            uomId = ValueTypeHelper.Int32TryParse(reader["FK_UOM"].ToString(), -1);

                        if (reader["UOM"] != null)
                            uom = reader["UOM"].ToString();

                        if (reader["AttrVal"] != null)
                        {
                            attrVal = reader["AttrVal"].ToString();
                            if (childAttribute.AttributeDataType == AttributeDataType.Date)
                            {
                                if (!String.IsNullOrWhiteSpace(attrVal))
                                {
                                    DateTime dateTime = new DateTime();
                                    dateTime = DateTime.ParseExact(attrVal, "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                                    if (childAttribute.AttributeDataType == AttributeDataType.Date)
                                        attrVal = dateTime.ToString("M/dd/yyyy");
                                }
                            }
                        }

                        if (reader["Locale"] != null)
                        {
                            String strLocale = reader["Locale"].ToString();

                            if (!String.IsNullOrWhiteSpace(strLocale))
                                Enum.TryParse<LocaleEnum>(strLocale, out attributeLocale);
                        }

                        Value value = new Value();
                        value.AttrVal = attrVal;
                        value.Locale = attributeLocale;
                        value.Uom = uom;
                        value.UomId = uomId;
                        value.ValueRefId = 0;

                        childAttribute.SetValue(value);

                        //SetValue updates ObjectAction.so set again as Read.
                        childAttribute.Action = ObjectAction.Read;
                        childAttribute.Locale = attributeLocale;

                        childAttribute.InstanceRefId = instanceRefId;
                        childAttribute.AttributeParentId = attributeParentId;
                        childAttribute.Id = childAttributeId;

                        //If instance ref id already available then append child attribute into existing collection.
                        if (childAttributes != null && childAttributes.ContainsKey(instanceRefId))
                        {
                            AttributeCollection attributes = childAttributes[instanceRefId];

                            if (attributes != null)
                            {
                                attributes.Add(childAttribute);
                            }
                        }
                        else
                        {
                            childAttributes.Add(instanceRefId, new AttributeCollection() { childAttribute });
                        }

                        #endregion
                    }

                    #region Read InValid Data

                    reader.NextResult();

                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            String invalidDataXml = String.Empty;

                            if (reader["AttrVal"] != null)
                                invalidDataXml = reader["AttrVal"].ToString();

                            attribute = new Attribute(invalidDataXml);
                            childAttributes.Add(instanceRefIdForInvalidRecords++, attribute.Attributes );
                        }
                    }

                    #endregion

                    if (childAttributes != null && childAttributes.Count > 0)
                    {
                        attribute = new Attribute();
                        var enumerator = childAttributes.GetEnumerator();

                        while (enumerator.MoveNext())
                        {
                            Attribute instanceRecord = new Attribute();
                            instanceRecord.Id = attributeId;
                            instanceRecord.Locale = locale;

                            instanceRecord.InstanceRefId = enumerator.Current.Key;
                            instanceRecord.Attributes = enumerator.Current.Value;

                            attribute.Attributes.Add(instanceRecord, true);
                        }
                    }
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.StopTraceActivity("AttributeManager.AttributeDA.GetComplexData", MDMTraceSource.AttributeGet);
                }
            }

            return attribute;
        }


        /// <summary>
        /// Get Hierarchical attribute data for specific point of version history based on attribute id, auditRefId and locale provided.
        /// Note that as of now, hierarchical attribute is stored using DataTransfer serialization, so Get uses the same.
        /// </summary>
        /// <param name="entityId">EntityId for which attribute history is needed</param>
        /// <param name="containerId">Container Id  under which Entity is created</param>
        /// <param name="attributeId">Attribute id for which we needs data</param>
        /// <param name="auditRefId">AuditRefId for which we needs data</param>
        /// <param name="locale">locale details</param>
        /// <param name="command">command object which contains all the db info like connection string</param>
        /// <param name="attributeModel">Hierarchical attribute model</param>
        /// <returns>Specific version of hierarchical attribute</returns>
        public Attribute GetHierarchicalAttributeByAuditRefId(Int64 entityId, Int32 containerId, Int32 attributeId, Int64 auditRefId, LocaleEnum locale, DBCommandProperties command, AttributeModel attributeModel)
        {
            if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity("AttributeManager.AttributeDA.GetHierarchicalAttributeByAuditRefId", MDMTraceSource.AttributeGet, false);
            }

            SqlParameter[] parameters;
            String storedProcedureName = String.Empty;
            SqlDataReader reader = null;
            Attribute hierarchicalAttribute = null;

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("AttributeManager_SqlParameters");

                parameters = generator.GetParameters("AttributeManager_HierarchyAttribute_Get_ParametersArray");

                parameters[0].Value = entityId;
                parameters[1].Value = containerId;
                parameters[2].Value = auditRefId;
                parameters[3].Value = attributeId;
                parameters[4].Value = locale;

                storedProcedureName = "usp_EntityManager_HierarchyAttribute_Get"; 

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                // creating hierarchical based on model
                hierarchicalAttribute = new Attribute(attributeModel, locale);
                
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        #region Construct first level instance records 

                        Int32 instanceRefId = 0;
                        LocaleEnum attributeLocale = LocaleEnum.UnKnown;

                        if (reader["InstanceRefId"] != null)
                        {
                            instanceRefId = ValueTypeHelper.Int32TryParse(reader["InstanceRefId"].ToString(), -1);
                        }

                        if (reader["Locale"] != null)
                        {
                            String strLocale = reader["Locale"].ToString();

                            if (!String.IsNullOrWhiteSpace(strLocale))
                                Enum.TryParse<LocaleEnum>(strLocale, out attributeLocale);
                        }

                        if (reader["FK_Attribute"] != null)
                        {
                            attributeId = ValueTypeHelper.Int32TryParse(reader["FK_Attribute"].ToString(), -1);
                        }

                        if (reader["AttrVal"] != null)
                        {
                            String attrVal = reader["AttrVal"].ToString();

                            if (!String.IsNullOrWhiteSpace(attrVal))
                            {
                                Int32 valueRefId = instanceRefId;
                                AttributeValueSource sourceFlag = AttributeValueSource.Overridden;

                                Attribute instanceAttribute = new Attribute(attrVal, Constants.HIERARCHICAL_ATTRIBUTE_SERIALIZATION);
                                hierarchicalAttribute.AddComplexChildRecord(instanceAttribute.Attributes, valueRefId, sourceFlag, locale, attributeId, false);
                            }
                        }

                        #endregion
                    }
                }

                hierarchicalAttribute.ValidateHierarchicalAttribute(attributeModel);
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.StopTraceActivity("AttributeManager.AttributeDA.GetHierarchicalAttributeByAuditRefId", MDMTraceSource.AttributeGet);
                }
            }

            return hierarchicalAttribute;
        }

        #endregion

        #region Private Methods
        #endregion

        #endregion
    }
}
