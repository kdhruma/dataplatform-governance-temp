using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Data.SqlClient;
using System.Linq;
using System.Transactions;
using MDM.Interfaces;

namespace MDM.OrganizationManager.Data
{
    using MDM.Core;
    using MDM.Utility;
    using MDM.BusinessObjects;
    using Microsoft.SqlServer.Server;
    using Attribute = MDM.BusinessObjects.Attribute;
    using MDM.BusinessObjects.DataModel;

    public class OrganizationDA : SqlClientDataAccessBase
    {
        #region Fields

        #endregion

        #region Constructors

        #endregion

        #region Properties

        #endregion

        #region Methods

        #region Get

        public OrganizationCollection GetAll(OrganizationContext organizationContext, DBCommandProperties command)
        {
            OrganizationCollection organizations = null;

            SqlParameter[] parameters;
            String storedProcedureName;
            SqlDataReader reader = null;

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("OrganizationManager_SqlParameters");

                parameters = generator.GetParameters("OrganizationManager_Organization_GetAll_ParametersArray");

                parameters[0].Value = 0;

                storedProcedureName = "usp_OrganizationManager_Organization_Get";

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                if (reader != null)
                {
                    organizations = ReadOrganizations(organizationContext.LoadAttributes, reader);
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
            return organizations;
        }

        public MDMObjectInfoCollection GetAllOrganizationDependencies(Int32 parentOrganizationId, DBCommandProperties command)
        {
            MDMObjectInfoCollection childs = new MDMObjectInfoCollection();

            SqlParameter[] parameters;
            const String storedProcedureName = "usp_Sec_Object_ChkDependency";
            SqlDataReader reader = null;
            const String objectName = "ORGANIZATION";
            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("OrganizationManager_SqlParameters");

                parameters = generator.GetParameters("OrganizationManager_Organization_GetAllOrganizationDependencies_ParametersArray");

                parameters[0].Value = objectName;
                parameters[1].Value = parentOrganizationId;

                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                if (reader != null)
                {
                    childs = ReadOrganizationChilds(reader);
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
            return childs;
        }
        #endregion

        #region Process

        /// <summary>
        /// Processes organization in accordance to operation
        /// </summary>
        /// <param name="organization">Organization</param>
        /// <param name="programName">Name of the program</param>
        /// <param name="userName">Name of the User</param>
        /// <param name="systemDataLocale">System Data Locale</param>
        /// <returns>The result of the operation </returns>
        public OperationResult Process(Organization organization, String programName, String userName, LocaleEnum systemDataLocale, DBCommandProperties command)
        {
            MDMTraceHelper.StartTraceActivity("OrganizationDA.Process", false);

            SqlDataReader reader = null;
            OperationResult organizationProcessOperationResult = new OperationResult();

            SqlParametersGenerator generator = new SqlParametersGenerator("OrganizationManager_SqlParameters");

            const String storedProcedureName = "usp_OrganizationManager_Organization_Process";

            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
            {
                try
                {
                    List<SqlDataRecord> attributeTable;

                    SqlParameter[] parameters = generator.GetParameters("OrganizationManager_Organization_Process_ParametersArray");
                    
                    SqlMetaData[] attributeMetaData = generator.GetTableValueMetadata("OrganizationManager_Organization_Process_ParametersArray", parameters[10].ParameterName);

                    CreateOrganizationTables(organization, attributeMetaData, out attributeTable);

                    parameters[0].Value = organization.Id;
                    parameters[1].Value = organization.OrganizationTypeId;
                    parameters[2].Value = organization.OrganizationClassification;
                    parameters[3].Value = organization.Name;
                    parameters[4].Value = organization.LongName;
                    parameters[5].Value = organization.OrganizationParent;
                    parameters[6].Value = organization.GLN;
                    parameters[7].Value = userName;
                    parameters[8].Value = programName;
                    parameters[9].Value = organization.ProcessorWeightage;
                    parameters[10].Value = attributeTable;
                    parameters[11].Value = (Int32)systemDataLocale;
                    parameters[12].Value = organization.Action;
                    parameters[13].Value = 0;

                    reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                    //Need empty information to make sure correct operation result status is calculated.
                    organizationProcessOperationResult.AddOperationResult(String.Empty, String.Empty, OperationResultType.Information);

                    PopulateOperationResult(reader, organization, organizationProcessOperationResult);
                    transactionScope.Complete();
                }
                catch (Exception exception)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, "Organization Process Failed." + exception.Message, MDMTraceSource.DataModel);
                    organizationProcessOperationResult.AddOperationResult(exception.Message, String.Empty, OperationResultType.Error);
                }
                finally
                {
                    if (reader != null)
                        reader.Close();
                    MDMTraceHelper.StopTraceActivity("OrganizationDA.Process");
                }
            }

            return organizationProcessOperationResult;
        }


        /// <summary>
        /// Processes organization in accordance to operation
        /// </summary>
        /// <param name="organizations">Organization Collection</param>
        /// <param name="operationResultCollection">OperationResult Collection</param>
        /// <param name="programName">Name of the program</param>
        /// <param name="userName">Name of the User</param>
        /// <param name="systemDataLocale">System Data Locale</param>
        public void Process(OrganizationCollection organizations, DataModelOperationResultCollection operationResultCollection, String programName, String userName, LocaleEnum systemDataLocale, DBCommandProperties command)
        {
            MDMTraceHelper.StartTraceActivity("OrganizationDA.Process", MDMTraceSource.DataModel, false);

            SqlDataReader reader = null;

            SqlParametersGenerator generator = new SqlParametersGenerator("OrganizationManager_SqlParameters");

            const String storedProcedureName = "usp_OrganizationManager_Organization_Process";

            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
            {

                foreach (Organization organization in organizations)
                {
                    IDataModelOperationResult organizationOperationResult = operationResultCollection.GetByReferenceId(organization.ReferenceId);
                    //TODO What if we get null here? throw?

                    if (organizationOperationResult.HasError || organization.Action == ObjectAction.Read || organization.Action == ObjectAction.Ignore)
                    {
                        continue;
                    }

                    try
                    {
                        #region Execute SQL Procedue
                        
                        List<SqlDataRecord> attributeTable;

                        SqlParameter[] parameters = generator.GetParameters("OrganizationManager_Organization_Process_ParametersArray");
                        
                        SqlMetaData[] attributeMetaData = generator.GetTableValueMetadata("OrganizationManager_Organization_Process_ParametersArray", parameters[10].ParameterName);

                        CreateOrganizationTables(organization, attributeMetaData, out attributeTable);

                        parameters[0].Value = organization.Id;
                        parameters[1].Value = organization.OrganizationTypeId;
                        parameters[2].Value = organization.OrganizationClassification;
                        parameters[3].Value = organization.Name;
                        parameters[4].Value = organization.LongName;
                        parameters[5].Value = organization.OrganizationParent;
                        parameters[6].Value = organization.GLN;
                        parameters[7].Value = organization.UserName;
                        parameters[8].Value = programName;
                        parameters[9].Value = organization.ProcessorWeightage;
                        parameters[10].Value = attributeTable;
                        parameters[11].Value = (Int32)systemDataLocale;
                        parameters[12].Value = organization.Action;
                        parameters[13].Value = 0;

                        reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                        #endregion Execute SQL Procedue

                        #region Update OperationResult

                        //Need empty information to make sure correct operation result status is calculated.
                        PopulateOperationResult(reader, organization, (OperationResult)organizationOperationResult);

                        operationResultCollection.RefreshOperationResultStatus();
                        #endregion

                    }
                    catch (Exception exception)
                    {
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, "Organization Process Failed." + exception.Message, MDMTraceSource.DataModel);
                        organizationOperationResult.AddOperationResult(exception.Message, String.Empty, OperationResultType.Error);
                    }
                    finally
                    {
                        if (reader != null)
                            reader.Close();
                        MDMTraceHelper.StopTraceActivity("OrganizationDA.Process");
                    }
                } // for Organizations

                transactionScope.Complete();
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Reads Organizations Metadata
        /// </summary>
        /// <param name="reader">Sql reader</param>
        /// <returns></returns>
        private MDMObjectInfoCollection ReadOrganizationChilds(SqlDataReader reader)
        {
            MDMObjectInfoCollection childsList = new MDMObjectInfoCollection();
            while (reader.Read())
            {
                String objectType = String.Empty;
                String objectname = String.Empty;

                if (reader["111454"] != null)
                {
                    objectType = reader["111454"].ToString();
                }

                if (reader["100974"] != null)
                {
                    objectname = reader["100974"].ToString();
                }

                childsList.Add(new MDMObjectInfo() { ObjectType = objectType, Name = objectname });
            }
            return childsList;
        }

        private OrganizationCollection ReadOrganizations(Boolean loadAttributes, SqlDataReader reader)
        {
            OrganizationCollection organizations = new OrganizationCollection();

            ReadOrganizationProperties(organizations, reader);

            if (loadAttributes)
            {
                ReadOrganizationAttributes(organizations, reader);
            }

            return organizations;
        }

        /// <summary>
        /// Reads Organizations Metadata
        /// </summary>
        /// <param name="organizations">Organizations to fill</param>
        /// <param name="reader">Sql reader</param>
        /// <returns></returns>
        private void ReadOrganizationProperties(OrganizationCollection organizations, SqlDataReader reader)
        {
            while (reader.Read())
            {
                Int32 orgId = 0;
                String name = String.Empty;
                String longName = String.Empty;
                Int32 organizationTypeId = 0;
                String gln = String.Empty;
                Int32 organizationClassification = 0;
                Int32 organizationParent = 0;
                String parentOrganizationName = String.Empty;
                Int32 processorWeightage = 0;

                if (reader["Id"] != null)
                {
                    orgId = ValueTypeHelper.Int32TryParse(reader["Id"].ToString(), 0);
                }

                if (reader["OrganizationType"] != null)
                {
                    organizationTypeId = ValueTypeHelper.Int32TryParse(reader["OrganizationType"].ToString(), 0);
                }

                if (reader["OrganizationClassification"] != null)
                {
                    organizationClassification = ValueTypeHelper.Int32TryParse(reader["OrganizationClassification"].ToString(), 0);
                }

                if (reader["Name"] != null)
                {
                    name = reader["Name"].ToString();
                }

                if (reader["OrganizationLongName"] != null)
                {
                    longName = reader["OrganizationLongName"].ToString();
                }

                if (reader["OrganizationParent"] != null)
                {
                    organizationParent = ValueTypeHelper.Int32TryParse(reader["OrganizationParent"].ToString(), 0);
                }

                if (reader["ParentOrganizationName"] != null)
                {
                    parentOrganizationName = reader["ParentOrganizationName"].ToString();
                }

                if (reader["GLN"] != null)
                {
                    gln = reader["GLN"].ToString();
                }

                if (reader["Processor_Weightage"] != null)
                {
                    processorWeightage = ValueTypeHelper.Int32TryParse(reader["Processor_Weightage"].ToString(), 0);
                }

                Organization organization = new Organization(orgId, name, longName, organizationTypeId, gln, organizationClassification, organizationParent, processorWeightage);
                organization.ParentOrganizationName = parentOrganizationName;

                organizations.Add(organization);
            }
        }
        
        /// <summary>
        /// Read Organization Attributes
        /// </summary>
        /// <param name="organizations">Collection of Organizations</param>
        /// <param name="reader">sql reader</param>
        private void ReadOrganizationAttributes(OrganizationCollection organizations, SqlDataReader reader)
        {
            //Move reader to attribute  resultset
            reader.NextResult();

            while (reader.Read())
            {
                #region Declare Local variables

                Int32 organizationId = 0;
                Int32 attributeId = 0;
                String attributeValue = String.Empty;
                Int32 uomId = -1;
                String uom = String.Empty;
                LocaleEnum locale = GlobalizationHelper.GetSystemDataLocale();

                #endregion Declare Local variables

                #region Read Organization Details from Attirbute Row

                if (reader["AttributeId"] != null)
                    attributeId = ValueTypeHelper.Int32TryParse(reader["AttributeId"].ToString(), 0);

                if (reader["OrgId"] != null)
                    organizationId = ValueTypeHelper.Int32TryParse(reader["OrgId"].ToString(), 0);

                //Get the organization
                Organization organization = organizations.SingleOrDefault(o => o.Id == organizationId);

                #endregion Read Organization Details from Attirbute Row

                #region Read Attribute values and create attribute and value object

                if (organization != null)
                {
                    //Read other parameters
                    if (reader["AttributeValue"] != null)
                        attributeValue = reader["AttributeValue"].ToString();

                    if (reader["UOM"] != null)
                        uom = reader["UOM"].ToString();

                    if (reader["Locale"] != null)
                        locale = (LocaleEnum)Enum.Parse(typeof(LocaleEnum), reader["Locale"].ToString());
                }

                //Create the value object
                Value value = new Value
                {
                    AttrVal = attributeValue,
                    Locale = locale,
                    Uom = uom,
                    UomId = uomId
                };

                Attribute attribute = new Attribute
                {
                    Id = attributeId,
                    SourceFlag = AttributeValueSource.Overridden,
                    AttributeModelType = AttributeModelType.System,
                    Locale = locale,
                    Action = ObjectAction.Read
                };

                attribute.SetValueInvariant(value);

                if (organization != null)
                    organization.Attributes.Add(attribute);

                #endregion Read Attribute values and create attribute and value object
            }
        }

        /// <summary>
        /// populates OperationResult
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="organization"></param>
        /// <param name="organizationProcessOperationResult"></param>
        private void PopulateOperationResult(SqlDataReader reader, Organization organization, OperationResult organizationProcessOperationResult)
        {
            while (reader.Read())
            {
                Boolean hasError = false;
                String errorCode = String.Empty;
                String organizationId = String.Empty;

                if (reader["Id"] != null)
                {
                    organizationId = reader["Id"].ToString();
                }

                if (reader["HasError"] != null)
                {
                    hasError = ValueTypeHelper.BooleanTryParse(reader["HasError"].ToString(), false);
                }

                if (reader["ErrorCode"] != null)
                {
                    errorCode = reader["ErrorCode"].ToString();
                }

                if (hasError & !String.IsNullOrEmpty(errorCode))
                {
                    organizationProcessOperationResult.AddOperationResult(errorCode, String.Empty, OperationResultType.Error);
                }
                else
                {
                    organizationProcessOperationResult.OperationResultStatus = OperationResultStatusEnum.Successful;
                    //organizationProcessOperationResult.AddOperationResult("", "Organization ID: " + organizationId, OperationResultType.Information);
                    organization.Id = ValueTypeHelper.Int32TryParse(organizationId, -1);
                    organizationProcessOperationResult.ReturnValues.Add(organizationId);
                }
            }
        }

        /// <summary>
        /// Create Container TVP for process
        /// </summary>
        /// <param name="organization">Organization</param>
        /// <param name="attributeMetaData">Metadata of Organization Attribute TVP</param>
        /// <param name="attributesTable"></param>
        private void CreateOrganizationTables(Organization organization, SqlMetaData[] attributeMetaData, out List<SqlDataRecord> attributesTable)
        {
            #region Attributes Table

            attributesTable = new List<SqlDataRecord>();

            foreach (var attribute in organization.Attributes)
            {
                IValueCollection values = attribute.GetCurrentValuesInvariant();

                if (values != null)
                {
                    foreach (Value value in values)
                    {
                        if (value.Action == ObjectAction.Ignore)
                            continue;

                        ObjectAction valueAction = attribute.Action;

                        if (attribute.IsCollection && !attribute.IsComplex)
                            valueAction = value.Action;

                        Int32 valueRefId = attribute.IsComplex ? organization.OrganizationParent : value.ValueRefId;

                        Decimal sequence = attribute.IsComplex ? attribute.Sequence : value.Sequence;

                        SqlDataRecord attributeRecord = new SqlDataRecord(attributeMetaData);
                        attributeRecord.SetInt64(0, value.Id);
                        attributeRecord.SetInt64(1, organization.Id);
                        attributeRecord.SetInt32(2, attribute.Id);
                        attributeRecord.SetInt32(3, attribute.AttributeParentId);
                        attributeRecord.SetInt32(4, attribute.InstanceRefId);

                        if (attribute.IsComplex)
                        {
                            attributeRecord.SetString(5, value.ValueRefId.ToString());
                        }
                        else
                        {
                            if (value.AttrVal != null)
                            {
                                attributeRecord.SetString(5, value.AttrVal.ToString());
                            }
                        }

                        if (attribute.IsLookup && !String.IsNullOrEmpty(value.GetExportValue()))
                        {
                            String maskVal = value.GetExportValue();
                            attributeRecord.SetString(6, maskVal);
                        }
                        else
                        {
                            attributeRecord.SetDBNull(6);
                        }

                        if (attribute.AttributeDataType == AttributeDataType.Integer
                            || attribute.AttributeDataType == AttributeDataType.Decimal
                            || attribute.AttributeDataType == AttributeDataType.Fraction)
                        {
                            if (value.NumericVal == null)
                                attributeRecord.SetDBNull(7);
                            else
                                attributeRecord.SetDecimal(7, (Decimal)value.NumericVal);
                        }
                        else
                        {
                            attributeRecord.SetDBNull(7);
                        }

                        if (attribute.AttributeDataType == AttributeDataType.Date || attribute.AttributeDataType == AttributeDataType.DateTime)
                        {
                            if (value.DateVal == null)
                                attributeRecord.SetDBNull(8);
                            else
                                attributeRecord.SetDateTime(8, (DateTime)value.DateVal);
                        }
                        else
                        {
                            attributeRecord.SetDBNull(8);
                        }

                        attributeRecord.SetInt32(9, valueRefId);
                        attributeRecord.SetInt32(10, (int)attribute.Locale);
                        attributeRecord.SetInt32(11, value.UomId);
                        attributeRecord.SetString(12, value.Uom);
                        attributeRecord.SetDecimal(13, sequence);
                        attributeRecord.SetBoolean(14, attribute.IsCollection);
                        attributeRecord.SetBoolean(15, attribute.IsComplex);
                        attributeRecord.SetString(16, valueAction.ToString());
                        attributeRecord.SetValue(17, attribute.AttributeType);
                        attributeRecord.SetInt64(18, attribute.SourceEntityId);
                        attributeRecord.SetInt32(19, attribute.SourceClass);
                        attributeRecord.SetString(20, Utility.GetSourceFlagString(attribute.SourceFlag));
                        attributeRecord.SetInt64(21, attribute.AuditRefId);
                        attributeRecord.SetValue(22, attribute.ReferenceId);

                        attributesTable.Add(attributeRecord);
                    }
                }
            }


            //we cannot pass no records in the SqlDataRecord enumeration. To send a table-valued parameter with no rows, use a null reference for the value instead.
            if (attributesTable.Count == 0)
                attributesTable = null;

            #endregion
        }

        #endregion Private Methods

        #endregion
    }
}