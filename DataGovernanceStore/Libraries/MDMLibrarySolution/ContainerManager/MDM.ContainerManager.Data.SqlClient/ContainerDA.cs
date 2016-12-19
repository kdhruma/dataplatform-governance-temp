using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Collections.ObjectModel;
using Microsoft.SqlServer.Server;

namespace MDM.ContainerManager.Data
{
    using MDM.Core;
    using MDM.BusinessObjects;
    using MDM.Utility;
    using MDM.Core.Exceptions;
    using MDM.BusinessObjects.DataModel;
    using MDM.MessageManager.Business;
    using MDM.BusinessObjects.Diagnostics;

    /// <summary>
    /// 
    /// </summary>
    public class ContainerDA : SqlClientDataAccessBase
    {
        #region Fields
        
        /// <summary>
        /// 
        /// </summary>
        private LocaleEnum _systemUILocale = LocaleEnum.UnKnown;

        /// <summary>
        /// Indicates current trace settings
        /// </summary>
        private TraceSettings _currentTraceSettings = new TraceSettings();

        /// <summary>
        /// Indicates whether diagnostic tracing is enable or not
        /// </summary>
        private Boolean isTracingEnabled = false;

        #endregion

        #region Constructors

        public ContainerDA()
        {
            _systemUILocale = GlobalizationHelper.GetSystemUILocale();
            _currentTraceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
            isTracingEnabled = _currentTraceSettings.IsBasicTracingEnabled;
        }

        #endregion

        #region Properties
        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Gets all containers
        /// </summary>
        /// <param name="containerContext">Context to indicate if container attributes are to be loaded or not</param>
        /// <param name="command">sql command</param>
        /// <returns>returns container collection</returns>
        public Dictionary<Int32, Container> GetAll(ContainerContext containerContext, DBCommandProperties command)
        {
            SqlDataReader reader = null;
            Dictionary<Int32, Container> containers = null;

            #region Step : Diagnostic Activity initialization

            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();

            if (isTracingEnabled)
            {
                diagnosticActivity.Start();
                diagnosticActivity.LogInformation("ContainerManager.Data.ContainerDA.GetAll started.");
            }

            #endregion

            try
            {
                SqlParametersGenerator generator = new SqlParametersGenerator("ContainerManager_SqlParameters");
                SqlParameter[] parameters = generator.GetParameters("ContainerManager_Container_Get_ParametersArray");

                parameters[0].Value = 0;
                parameters[1].Value = containerContext.LoadAttributes;

                const String storedProcedureName = "usp_ContainerManager_Container_Get";
                reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                if (isTracingEnabled)
                {
                    diagnosticActivity.LogDurationInfo("Completed executing reader for procedure : " + storedProcedureName);
                }

                if (reader != null)
                {
                    containers = ReadContainers(containerContext.LoadAttributes, reader);
                }
            }
            finally
            {
                if (isTracingEnabled)
                {
                    diagnosticActivity.LogInformation("ContainerManager.Data.ContainerDA.GetAll completed.");
                    diagnosticActivity.Stop();
                }

                if (reader != null)
                {
                    reader.Close();
                }
            }

            return containers;
        }

        /// <summary>
        /// Processes the list of containers (i.e All CRUD operations goes through same method
        /// </summary>
        /// <param name="containerCollection">list of containers to processed</param>
        /// <param name="containerOperationResult">operation result for container process</param>
        /// <param name="command">contains all the db related properties such as connection string etc.</param>
        /// <param name="systemDataLocale">systems data locale</param>
        /// <param name="loginUser">name of the logon user</param>
        /// <param name="programName">name of the program who called this operation</param>
        public void Process(ContainerCollection containerCollection, OperationResult containerOperationResult, DBCommandProperties command, LocaleEnum systemDataLocale, String loginUser, String programName)
        {
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("ContainerDA.Process", false);

            SqlDataReader reader = null;

            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    List<SqlDataRecord> containerTable;
                    List<SqlDataRecord> attributeTable;
                    List<SqlDataRecord> containerSecondaryQualifiersTable;
                    SqlParametersGenerator generator = new SqlParametersGenerator("ContainerManager_SqlParameters");
                    SqlParameter[] parameters = generator.GetParameters("ContainerManager_Container_Process_ParametersArray");
                    SqlMetaData[] containerMetaData = generator.GetTableValueMetadata("ContainerManager_Container_Process_ParametersArray", parameters[0].ParameterName);
                    SqlMetaData[] attributeMetaData = generator.GetTableValueMetadata("ContainerManager_Container_Process_ParametersArray", parameters[1].ParameterName);
                    SqlMetaData[] containerSecondaryQualifiers = generator.GetTableValueMetadata("ContainerManager_Container_Process_ParametersArray", parameters[2].ParameterName);

                    CreateContainerTable(containerCollection, containerMetaData, attributeMetaData, containerSecondaryQualifiers, out containerTable, out attributeTable, out containerSecondaryQualifiersTable);

                    parameters[0].Value = containerTable;
                    parameters[1].Value = attributeTable;
                    parameters[2].Value = containerSecondaryQualifiersTable;
                    parameters[3].Value = loginUser;
                    parameters[4].Value = (Int32)systemDataLocale;
                    parameters[5].Value = programName;
                    const String storedProcedureName = "usp_ContainerManager_Container_Process";

                    reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                    PopulateOperationResult(reader, containerOperationResult, containerCollection);

                }
                catch (Exception exception)
                {
                    throw new MDMOperationException(exception.Message, exception);
                }
                finally
                {
                    if (reader != null)
                        reader.Close();
                }

                transactionScope.Complete();

            }

        }

        /// <summary>
        /// Copies Container specific mappings from source container to targetContainer
        /// </summary>
        /// <param name="sourceContainerId">source container Id</param>
        /// <param name="targetContainerId">target Container Id</param>
        /// <param name="containerTemplateCopyContext">Context specifies which all mappings needs to be copied from source to target</param>
        /// <param name="command">contains all the db related properties such as connection string etc.</param>
        /// <param name="loginUser">logged in user</param>
        /// <param name="programName">name of the method which called this API</param>
        /// <returns></returns>
        public OperationResult CopyMappings(Int32 sourceContainerId, Int32 targetContainerId, ContainerTemplateCopyContext containerTemplateCopyContext, DBCommandProperties command, String loginUser, String programName)
        {
            OperationResult operationResult = new OperationResult();

            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required))
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StartTraceActivity("ContainerDA.CopyMappings", false);
                SqlDataReader reader = null;

                try
                {
                    SqlParametersGenerator generator = new SqlParametersGenerator("ContainerManager_SqlParameters");
                    SqlParameter[] parameters = generator.GetParameters("ContainerManager_Container_CopyMappings_ParametersArray");
                    parameters[0].Value = sourceContainerId;
                    parameters[1].Value = targetContainerId;
                    parameters[2].Value = containerTemplateCopyContext.CopyContainerEntityTypeAttributeMappings;
                    parameters[3].Value = containerTemplateCopyContext.CopyContainerRelationshipTypeMappings;
                    parameters[4].Value = containerTemplateCopyContext.CopyContainerRelationshipTypeAttributeMappings;
                    parameters[5].Value = containerTemplateCopyContext.FlushAndFillTargetContainer;
                    parameters[6].Value = containerTemplateCopyContext.CopyContainerBranchLevelOne;
                    parameters[7].Value = loginUser;
                    parameters[8].Value = programName;
                    const String storedProcedureName = "usp_ContainerManager_MappingTemplate_Copy";

                    reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                    while (reader.Read())
                    {
                        Boolean hasError = false;
                        String errorCode = String.Empty;
                        String errorMessage = String.Empty;

                        if (reader["HasError"] != null)
                            Boolean.TryParse(reader["HasError"].ToString(), out hasError);
                        if (reader["ErrorCode"] != null)
                            errorCode = reader["ErrorCode"].ToString();
                        if (reader["ErrorMessage"] != null)
                            errorMessage = reader["ErrorMessage"].ToString();

                        if (hasError)
                        {
                            operationResult.AddOperationResult(errorCode, errorMessage, OperationResultType.Error);
                            operationResult.OperationResultStatus = OperationResultStatusEnum.Failed;
                        }
                        else
                        {
                            operationResult.OperationResultStatus = OperationResultStatusEnum.Successful;
                        }
                    }
                }
                finally
                {
                    if (reader != null)
                        reader.Close();
                }

                transactionScope.Complete();

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("ContainerDA.CopyMappings");
            }

            return operationResult;

        }

        /// <summary>
        /// Processes the list of containers (i.e All CRUD operations goes through same method
        /// </summary>
        /// <param name="containers">list of containers to processed</param>
        /// <param name="operationResults">list of operation results</param>
        /// <param name="command">contains all the db related properties such as connection string etc.</param>
        /// <param name="systemDataLocale">systems data locale</param>
        /// <param name="loginUser">name of the logon user</param>
        /// <param name="programName">name of the program who called this operation</param>
        public void Process(ContainerCollection containers, DataModelOperationResultCollection operationResults, DBCommandProperties command, LocaleEnum systemDataLocale, String loginUser, String programName)
        {
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity("ContainerDA.Process", MDMTraceSource.DataModel, false);
            }

            SqlDataReader reader = null;
            const String storedProcedureName = "usp_ContainerManager_Container_Process";
            SqlParametersGenerator generator = new SqlParametersGenerator("ContainerManager_SqlParameters");

            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    List<SqlDataRecord> containerTable;
                    List<SqlDataRecord> attributeTable;
                    List<SqlDataRecord> containerSecondaryQualifiersTable;
                    SqlParameter[] parameters = generator.GetParameters("ContainerManager_Container_Process_ParametersArray");
                    SqlMetaData[] containerMetaData = generator.GetTableValueMetadata("ContainerManager_Container_Process_ParametersArray", parameters[0].ParameterName);
                    SqlMetaData[] attributeMetaData = generator.GetTableValueMetadata("ContainerManager_Container_Process_ParametersArray", parameters[1].ParameterName);
                    SqlMetaData[] containerSecondaryQualifierMetaData = generator.GetTableValueMetadata("ContainerManager_Container_Process_ParametersArray", parameters[2].ParameterName);

                    CreateContainerTable(containers, containerMetaData, attributeMetaData, containerSecondaryQualifierMetaData, out containerTable, out attributeTable, out containerSecondaryQualifiersTable);

                    parameters[0].Value = containerTable;
                    parameters[1].Value = attributeTable;
                    parameters[2].Value = containerSecondaryQualifiersTable;
                    parameters[3].Value = loginUser;
                    parameters[4].Value = (Int32)systemDataLocale;
                    parameters[5].Value = programName;

                    reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);

                    PopulateOperationResult(reader, operationResults, containers);

                    operationResults.RefreshOperationResultStatus();
                }
                catch (Exception exception)
                {
                    throw new MDMOperationException(exception.Message, exception);
                }
                finally
                {
                    if (reader != null)
                        reader.Close();
                }

                transactionScope.Complete();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="containers"></param>
        /// <param name="operationResults"></param>
        /// <param name="command"></param>
        /// <param name="_systemDataLocale"></param>
        /// <param name="loginUser"></param>
        /// <param name="programName"></param>
        public void ProcessLocaleMappings(ContainerCollection containers, DataModelOperationResultCollection operationResults, DBCommandProperties command, LocaleEnum _systemDataLocale, string loginUser, string programName)
        {
            DiagnosticActivity diagnosticActivity = null;
            if (isTracingEnabled)
            {
                ExecutionContext executionContext = new ExecutionContext(MDMTraceSource.DataModel);
                diagnosticActivity = new DiagnosticActivity(executionContext);
                diagnosticActivity.Start();
            }

            SqlDataReader reader = null;

            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    reader = ProcessContainerLocaleMappings(containers, command, loginUser, programName);

                    PopulateLocalesOperationResult(reader, operationResults, containers);

                    operationResults.RefreshOperationResultStatus();
                }
                catch (Exception exception)
                {
                    throw new MDMOperationException(exception.Message, exception);
                }
                finally
                {
                    if (reader != null)
                    {
                        reader.Close();
                    }

                    if (isTracingEnabled)
                    {
                        diagnosticActivity.Stop();
                    }
                }

                transactionScope.Complete();
            }
        }

        /// <summary>
        /// Processes the list of containers (i.e All CRUD operations goes through same method
        /// </summary>
        /// <param name="containerCollection">list of containers to processed</param>
        /// <param name="containerOperationResult">operation result for container process</param>
        /// <param name="command">contains all the db related properties such as connection string etc.</param>
        /// <param name="systemDataLocale">systems data locale</param>
        /// <param name="loginUser">name of the logon user</param>
        /// <param name="programName">name of the program who called this operation</param>
        public void ProcessLocaleMappings(ContainerCollection containerCollection, OperationResult containerOperationResult, DBCommandProperties command, LocaleEnum systemDataLocale, String loginUser, String programName)
        {
            DiagnosticActivity diagnosticActivity = null;
            if (isTracingEnabled)
            {
                ExecutionContext executionContext = new ExecutionContext(MDMTraceSource.DataModel);
                diagnosticActivity = new DiagnosticActivity(executionContext);
                diagnosticActivity.Start();
            }

            SqlDataReader reader = null;

            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    reader = ProcessContainerLocaleMappings(containerCollection, command, loginUser, programName);

                    PopulateLocalesOperationResult(reader, containerOperationResult, containerCollection);

                    containerOperationResult.RefreshOperationResultStatus();
                }
                catch (Exception exception)
                {
                    throw new MDMOperationException(exception.Message, exception);
                }
                finally
                {
                    if (reader != null)
                    {
                        reader.Close();
                    }

                    if (isTracingEnabled)
                    {
                        diagnosticActivity.Stop();
                    }
                }

                transactionScope.Complete();
            }

        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Reads Containers - Wrapper method to call other read methods
        /// </summary>
        /// <param name="loadAttributes">flag to indicate - read attributes or not</param>
        /// <param name="reader">sql reader</param>
        /// <returns>Returns collection of containers</returns>
        private Dictionary<Int32, Container> ReadContainers(Boolean loadAttributes, SqlDataReader reader)
        {
            var containers = new Dictionary<Int32, Container>();

            ReadContainerProperties(containers, reader);

            if (loadAttributes)
            {
                ReadAttributes(containers, reader);
            }
            else
            {
                reader.NextResult();
            }

            ReadContainerSecondaryQualifiers(containers, reader);

            ReadContainerLocales(containers, reader);

            return containers;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="containers"></param>
        /// <param name="reader"></param>
        private void ReadContainerLocales(Dictionary<Int32, Container> containers, SqlDataReader reader)
        {
            //Move reader to attribute  resultset
            reader.NextResult();

            Dictionary<Int32, LocaleCollection> containerLocales = new Dictionary<Int32, LocaleCollection>();

            while (reader.Read())
            {
                Int32 containerId = 0;

                if (reader["FK_Catalog"] != null)
                {
                    LocaleCollection locales = null;

                    containerId = ValueTypeHelper.Int32TryParse(reader["FK_Catalog"].ToString(), 0);

                    if (!containerLocales.TryGetValue(containerId, out locales))
                    {
                        locales = new LocaleCollection();
                        containerLocales.Add(containerId, locales);
                    }

                    Locale locale = new Locale();

                    if (reader["FK_Locale"] != null)
                    {
                        locale.Id = ValueTypeHelper.Int32TryParse(reader["FK_Locale"].ToString(), locale.Id);
                    }

                    if (reader["LocaleShortName"] != null)
                    {
                        locale.Name = reader["LocaleShortName"].ToString();
                    }

                    if (reader["LocaleLongName"] != null)
                    {
                        locale.LongName = reader["LocaleLongName"].ToString();
                    }

                    if (reader["FK_Region"] != null)
                    {
                        locale.RegionId = ValueTypeHelper.Int32TryParse(reader["FK_Region"].ToString(), locale.RegionId);
                    }

                    if (reader["RegionLongName"] != null)
                    {
                        locale.RegionName = reader["RegionLongName"].ToString();
                    }

                    if (reader["FK_Culture"] != null)
                    {
                        locale.CultureId = ValueTypeHelper.Int32TryParse(reader["FK_Culture"].ToString(), locale.CultureId);
                    }

                    if (reader["CultureLongName"] != null)
                    {
                        locale.CultureName = reader["CultureLongName"].ToString();
                    }

                    if (reader["FK_Lang"] != null)
                    {
                        locale.LanguageId = ValueTypeHelper.Int32TryParse(reader["FK_Lang"].ToString(), locale.LanguageId);
                    }

                    if (reader["LanguageShortName"] != null)
                    {
                        locale.LanguageName = reader["LanguageShortName"].ToString();
                    }

                    locale.Locale = (LocaleEnum)locale.Id;
                    
                    locales.Add(locale);
                }
            }

            if (containerLocales.Count > 0)
            {
                foreach (KeyValuePair<Int32, LocaleCollection> item in containerLocales)
                {
                    Container container = null;
                    if (containers.TryGetValue(item.Key, out container))
                    {
                        container.SupportedLocales = item.Value;
                    }
                }
            }
        }

        /// <summary>
        /// Reads Containers Metadata
        /// </summary>
        /// <param name="containers"></param>
        /// <param name="reader">Dql reader</param>
        /// <returns></returns>
        private void ReadContainerProperties(Dictionary<Int32, Container> containers, SqlDataReader reader)
        {
            Container container = null;

            while (reader.Read())
            {
                Int32 id = 0;
                String name = String.Empty;
                String longName = String.Empty;
                LocaleEnum locale = LocaleEnum.UnKnown;
                Int32 organizationId = 0;
                String organizationShortName = String.Empty;
                String organizationLongName = String.Empty;
                Int32 hierarchyId = 0;
                String hierarchyShortName = String.Empty;
                String hierarchyLongName = String.Empty;
                Boolean isDefault = false;
                Int32 securityObjectTypeId = 0;
                Boolean isStaging = false;
                ContainerType containerType = ContainerType.Unknown;
                Int32 containerQualifierId = 0;
                String containerQualifierName = String.Empty;
                Collection<String> containerSecondaryQualifierNames = new Collection<String>();
                Int32 parentContainerId = -1;
                String parentContainerName = String.Empty;
                Boolean needsApprovedCopy = true;
                WorkflowType workflowType = WorkflowType.Unknown;
                Int32 crossReferenceId = -1;
                Int32 level = -1;
                Boolean autoExtensionEnabled = true;

                if (reader["Id"] != null)
                {
                    id = ValueTypeHelper.Int32TryParse(reader["Id"].ToString(), 0);
                }

                if (reader["Name"] != null)
                {
                    name = reader["Name"].ToString();
                }

                if (reader["LongName"] != null)
                {
                    longName = reader["LongName"].ToString();
                }

                if (reader["Locale"] != null)
                {
                    Enum.TryParse(reader["Locale"].ToString(), out locale);
                }

                if (reader["OrganizationId"] != null)
                {
                    organizationId = ValueTypeHelper.Int32TryParse(reader["OrganizationId"].ToString(), 0);
                }

                if (reader["OrganizationShortName"] != null)
                {
                    organizationShortName = reader["OrganizationShortName"].ToString();
                }

                if (reader["OrganizationLongName"] != null)
                {
                    organizationLongName = reader["OrganizationLongName"].ToString();
                }

                if (reader["HierarchyId"] != null)
                {
                    hierarchyId = ValueTypeHelper.Int32TryParse(reader["HierarchyId"].ToString(), 0);
                }

                if (reader["HierarchyShortName"] != null)
                {
                    hierarchyShortName = reader["HierarchyShortName"].ToString();
                }

                if (reader["HierarchyLongName"] != null)
                {
                    hierarchyLongName = reader["HierarchyLongName"].ToString();
                }

                if (reader["IsDefault"] != null)
                {
                    isDefault = ValueTypeHelper.ConvertToBoolean(reader["IsDefault"].ToString());
                }

                if (reader["SecurityObjectTypeId"] != null)
                {
                    securityObjectTypeId = ValueTypeHelper.Int32TryParse(reader["SecurityObjectTypeId"].ToString(), 0);
                }

                if (reader["IsStaging"] != null)
                {
                    isStaging = ValueTypeHelper.ConvertToBoolean(reader["IsStaging"].ToString());
                }

                if (reader["FK_ContainerType"] != null)
                {
                    Enum.TryParse(reader["FK_ContainerType"].ToString(), out containerType);
                }

                if (reader["FK_ContainerQualifier"] != null)
                {
                    containerQualifierId = ValueTypeHelper.Int32TryParse(reader["FK_ContainerQualifier"].ToString(), 0);
                }

                if (reader["ContainerQualifierName"] != null)
                {
                    containerQualifierName = reader["ContainerQualifierName"].ToString();
                }

                if (reader["FK_CatalogParent"] != null)
                {
                    parentContainerId = ValueTypeHelper.Int32TryParse(reader["FK_CatalogParent"].ToString(), parentContainerId);
                }

                if (reader["ContainerParentShortName"] != null)
                {
                    parentContainerName = reader["ContainerParentShortName"].ToString();
                }

                if (reader["NeedsApprovedCopy"] != null)
                {
                    needsApprovedCopy = ValueTypeHelper.BooleanTryParse(reader["NeedsApprovedCopy"].ToString(), true);
                }

                if (reader["WorkflowType"] != null)
                {
                    Enum.TryParse(reader["WorkflowType"].ToString(), out workflowType);
                }

                if (reader["ContainerReferenceId"] != null)
                {
                    crossReferenceId = ValueTypeHelper.Int32TryParse(reader["ContainerReferenceId"].ToString(), crossReferenceId);
                }

                if (reader["ContainerLevel"] != null)
                {
                    level = ValueTypeHelper.Int32TryParse(reader["ContainerLevel"].ToString(), level);
                }

                if (reader["AutoExtensionEnabled"] != null)
                {
                    autoExtensionEnabled = ValueTypeHelper.BooleanTryParse(reader["AutoExtensionEnabled"].ToString(), autoExtensionEnabled);
                }

                container = new Container()
                {
                    Id = id,
                    Name = name,
                    LongName = longName,
                    Locale = locale,
                    OrganizationId = organizationId,
                    OrganizationShortName = organizationShortName,
                    OrganizationLongName = organizationLongName,
                    HierarchyId = hierarchyId,
                    HierarchyShortName = hierarchyShortName,
                    HierarchyLongName = hierarchyLongName,
                    IsDefault = isDefault,
                    SecurityObjectTypeId = securityObjectTypeId,
                    IsStaging = isStaging,
                    ContainerType = containerType,
                    ContainerQualifierId = containerQualifierId,
                    ContainerQualifierName = containerQualifierName,
                    ContainerSecondaryQualifiers = containerSecondaryQualifierNames,
                    ParentContainerId = parentContainerId,
                    ParentContainerName = parentContainerName,
                    NeedsApprovedCopy = needsApprovedCopy,
                    WorkflowType = workflowType,
                    CrossReferenceId = crossReferenceId,
                    Level = level,
                    AutoExtensionEnabled = autoExtensionEnabled
                };

                containers.Add(container.Id, container);
            }
        }

        /// <summary>
        /// Read Container Attributes
        /// </summary>
        /// <param name="containers"></param>
        /// <param name="reader">sql reader</param>
        private void ReadAttributes(Dictionary<Int32, Container> containers, SqlDataReader reader)
        {
            //Move reader to attribute  resultset
            reader.NextResult();

            while (reader.Read())
            {
                #region Declare Local variables

                Int32 containerId = 0;
                Int32 attributeId = 0;
                String attributeValue = String.Empty;
                Int32 uomId = -1;
                String uom = String.Empty;
                LocaleEnum locale = MDM.Utility.GlobalizationHelper.GetSystemDataLocale();
                Int32 localeId = (Int32)locale;

                #endregion Declare Local variables

                #region Read Container Details from Attirbute Row

                if (reader["FK_Attribute"] != null)
                    attributeId = ValueTypeHelper.Int32TryParse(reader["FK_Attribute"].ToString(), 0);

                if (reader["FK_Catalog"] != null)
                    containerId = ValueTypeHelper.Int32TryParse(reader["FK_Catalog"].ToString(), 0);

                //Get the container
                Container container = containers[containerId];

                #endregion Read Container Details from Attirbute Row

                #region Read Attribute values and create attribute and value object

                /*
                    Read Attribute value Properties
                    create attribute and value object
                    Add this attribute to container
                */

                if (container != null)
                {
                    //Read other parameters
                    if (reader["AttrVal"] != null)
                        attributeValue = reader["AttrVal"].ToString();

                    if (reader["FK_UOM"] != null)
                        uomId = ValueTypeHelper.Int32TryParse(reader["FK_UOM"].ToString(), -1);

                    if (reader["UOM"] != null)
                        uom = reader["UOM"].ToString();


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

                    container.Attributes.Add(attribute);
                }

                #endregion Read Attribute values and create attribute and value object
            }
        }

        /// <summary>
        /// Read container secondary qualifiers
        /// </summary>
        /// <param name="containers"></param>
        /// <param name="reader">sql reader</param>
        private void ReadContainerSecondaryQualifiers(Dictionary<Int32, Container> containers, SqlDataReader reader)
        {
            //Move reader to attribute  resultset
            reader.NextResult();
            
            Dictionary<Int32, Collection<String>> containerSecondaryQualifiers = new Dictionary<Int32, Collection<String>>();

            while (reader.Read())
            {
                Int32 containerId = 0;

                if (reader["QualifierName"] != null && reader["FK_Catalog"] != null)
                {
                    Collection<String> secondaryQualifiers = new Collection<String>();

                    containerId = ValueTypeHelper.Int32TryParse(reader["FK_Catalog"].ToString(), 0);

                    if(containerSecondaryQualifiers.ContainsKey(containerId))
                    {
                        secondaryQualifiers = containerSecondaryQualifiers[containerId];
                        secondaryQualifiers.Add(reader["QualifierName"].ToString());
                    }
                    else
                    {
                        secondaryQualifiers.Add(reader["QualifierName"].ToString());
                        containerSecondaryQualifiers.Add(containerId, secondaryQualifiers);
                    }
                }
            }

            if (containerSecondaryQualifiers.Count > 0)
            {
                foreach (KeyValuePair<Int32, Collection<String>> item in containerSecondaryQualifiers)
                {
                    if (containers.ContainsKey(item.Key))
                    {
                        Container container = containers[item.Key];
                        container.ContainerSecondaryQualifiers = item.Value;
                    }
                }
            }
        }

        /// <summary>
        /// Create Container TVP for process
        /// </summary>
        /// <param name="containerCollection">Collection of Containers</param>
        /// <param name="containerMetaData">Metadata of Container TVP</param>
        /// <param name="attributeMetaData">Metadata of Container Attribute TVP</param>
        /// <param name="containerSecondaryQualifierMetaData">Metadata of Container Secondary Qualifiers TVP</param>
        /// <param name="containerTable">Container Table which is used as Input for SP</param>
        /// <param name="attributeTable">Attribute Table which is used as Input for SP</param>
        /// <param name="containerSecondaryQualifiersTable">Container Secondary Qualifier Table which is used as Input for SP</param>
        private void CreateContainerTable(ContainerCollection containerCollection, SqlMetaData[] containerMetaData, SqlMetaData[] attributeMetaData, SqlMetaData[] containerSecondaryQualifierMetaData, 
                                        out List<SqlDataRecord> containerTable, out List<SqlDataRecord> attributeTable, out List<SqlDataRecord> containerSecondaryQualifiersTable)
        {
            containerTable = new List<SqlDataRecord>();
            //TODO:Currently we are not making use of attribute table. If required, we need to fill this table.So setting it to null
            attributeTable = new List<SqlDataRecord>();
            containerSecondaryQualifiersTable = new List<SqlDataRecord>();

            foreach (Container container in containerCollection)
            {
                SqlDataRecord containerRecord = new SqlDataRecord(containerMetaData);
                containerRecord.SetValue(0, ValueTypeHelper.Int32TryParse(container.ReferenceId, 0));
                containerRecord.SetValue(1, container.Id);
                containerRecord.SetValue(2, container.OrganizationId);
                containerRecord.SetValue(3, container.Name);
                containerRecord.SetValue(4, container.LongName);
                containerRecord.SetValue(5, ValueTypeHelper.ConvertBooleanToString(container.IsDefault));
                containerRecord.SetValue(6, container.HierarchyId);
                containerRecord.SetValue(7, ObjectType.Catalog);
                containerRecord.SetValue(8, container.IsStaging);
                containerRecord.SetValue(9, container.ProcessorWeightage);
                containerRecord.SetValue(10, container.Locale);
                containerRecord.SetValue(11, container.Action.ToString());
                containerRecord.SetValue(12, container.ContainerType);
                containerRecord.SetValue(13, container.ParentContainerId);
                containerRecord.SetValue(14, container.ContainerQualifierName);
                containerRecord.SetValue(15, container.NeedsApprovedCopy);
                containerRecord.SetValue(16, container.WorkflowType);
                containerRecord.SetValue(17, false);
                containerRecord.SetValue(18, container.AutoExtensionEnabled);
                containerRecord.SetValue(19, true); //Setting default as true for IsActive column. 

                containerTable.Add(containerRecord);

                foreach (String containerSecondaryQualifier in container.ContainerSecondaryQualifiers)
                {
                    SqlDataRecord containerSecondaryQualifierRecord = new SqlDataRecord(containerSecondaryQualifierMetaData);
                    containerSecondaryQualifierRecord.SetValue(0, ValueTypeHelper.Int32TryParse(container.ReferenceId, 0));
                    containerSecondaryQualifierRecord.SetValue(1, containerSecondaryQualifier);
                    containerSecondaryQualifiersTable.Add(containerSecondaryQualifierRecord);
                }

                if (container.Attributes.Count > 0)
                {
                    foreach (Attribute attribute in container.Attributes)
                    {
                        var values = attribute.GetOverriddenValuesInvariant();

                        if(values != null && values.Count > 0)
                        {
                            foreach(Value value in values)
                            {
                                SqlDataRecord containerAttribute = new SqlDataRecord(attributeMetaData);
                                containerAttribute.SetValue(0, ValueTypeHelper.Int32TryParse(container.ReferenceId, 0));
                                containerAttribute.SetValue(1, attribute.Id);
                                containerAttribute.SetValue(2, ObjectType.Catalog);
                                containerAttribute.SetValue(3, container.Id);
                                containerAttribute.SetValue(4, value.UomId);
                                containerAttribute.SetValue(5, attribute.Locale);
                                containerAttribute.SetValue(6, value.AttrVal.ToString());

                                attributeTable.Add(containerAttribute);
                            }
                        }
                    }
                }

            }

            //we cannot pass no records in the SqlDataRecord enumeration. To send a table-valued parameter with no rows, use a null reference for the value instead.
            if (containerTable.Count == 0)
            {
                containerTable = null;
            }

            if (attributeTable.Count == 0)
            {
                attributeTable = null;
            }

            if (containerSecondaryQualifiersTable.Count == 0)
            {
                containerSecondaryQualifiersTable = null;
            }
        }

        /// <summary>
        /// populates OperationResult
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="containerOperationResult"></param>
        /// <param name="containers"></param>
        private void PopulateOperationResult(SqlDataReader reader, OperationResult containerOperationResult, ContainerCollection containers)
        {
            while (reader.Read())
            {
                Boolean hasError = false;
                String errorCode = String.Empty;
                String containerId = String.Empty;
                String referenceId = String.Empty;

                if (reader["Id"] != null)
                {
                    referenceId = reader["Id"].ToString();
                }

                if (reader["ContainerId"] != null)
                {
                    containerId = reader["ContainerId"].ToString();
                }
                if (reader["HasError"] != null)
                {
                    Boolean.TryParse(reader["HasError"].ToString(), out hasError);
                }
                if (reader["ErrorCode"] != null)
                {
                    errorCode = reader["ErrorCode"].ToString();
                }

                if (hasError)
                {
                    if (!String.IsNullOrEmpty(errorCode))
                    {
                        if (errorCode == "-99") // -99 is an error code which is return by DB when container's properties trying to update if container has entities inside. 
                        {
                            AddOperationResult(containerOperationResult, containers, containerId, String.Empty);
                        }
                        else
                        {
                            containerOperationResult.AddOperationResult(errorCode, String.Empty, OperationResultType.Error);
                        }
                    }
                    containerOperationResult.OperationResultStatus = OperationResultStatusEnum.Failed;
                }
                else
                {
                    Container container = containers.Where(a => a.ReferenceId == referenceId).FirstOrDefault();
                    if (container != null)
                    {
                        container.Id = ValueTypeHelper.Int32TryParse(containerId, 0);
                    }
                    containerOperationResult.OperationResultStatus = OperationResultStatusEnum.Successful;
                }
            }
        }

        /// <summary>
        /// populates OperationResult
        /// </summary>
        /// <param name="reader">Sql Data reader which is used to read data</param>
        /// <param name="operationResults">Data Model Operation Results collection which is used for result</param>
        /// <param name="containers">Containers to be processed</param>
        private void PopulateOperationResult(SqlDataReader reader, DataModelOperationResultCollection operationResults, ContainerCollection containers)
        {
            while (reader.Read())
            {
                Boolean hasError = false;
                String errorCode = String.Empty;
                String containerId = String.Empty;
                String referenceId = String.Empty;

                if (reader["Id"] != null)
                {
                    referenceId = reader["Id"].ToString();
                }

                OperationResult containerOperationResult = (OperationResult)operationResults.GetByReferenceId(referenceId);

                if (reader["ContainerId"] != null)
                {
                    containerId = reader["ContainerId"].ToString();
                }

                if (reader["HasError"] != null)
                {
                    Boolean.TryParse(reader["HasError"].ToString(), out hasError);
                }

                if (reader["ErrorCode"] != null)
                {
                    errorCode = reader["ErrorCode"].ToString();
                }

                if (hasError)
                {
                    if (!String.IsNullOrEmpty(errorCode))
                    {
                        if (errorCode == "-99") // -99 is an error code which is return by DB when container's properties trying to update if container has entities inside. 
                        {
                            AddOperationResult(containerOperationResult, containers, containerId, referenceId);
                        }
                        else
                        {
                            containerOperationResult.AddOperationResult(errorCode, String.Empty, OperationResultType.Error);
                        }
                    }

                    containerOperationResult.OperationResultStatus = OperationResultStatusEnum.Failed;
                }
                else
                {
                    Container container = containers.Where(a => a.ReferenceId == referenceId).FirstOrDefault();
                    if (container != null)
                    {
                        container.Id = ValueTypeHelper.Int32TryParse(containerId, 0);
                    }

                    containerOperationResult.OperationResultStatus = OperationResultStatusEnum.Successful;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="containerOperationResult"></param>
        /// <param name="containers"></param>
        /// <param name="containerId"></param>
        /// <param name="referenceId"></param>
        private void AddOperationResult(OperationResult containerOperationResult, ContainerCollection containers, String containerId, String referenceId)
        {
            Container container = containers.GetContainer(ValueTypeHelper.Int32TryParse(containerId, 0));
            if (container != null)
            {
                Container originalContainer = container.OriginalContainer;
                CallerContext callerContext = new CallerContext(MDMCenterApplication.MDMCenter, MDMCenterModules.DataModelImport);
                LocaleMessageBL localeMessageManager = new LocaleMessageBL();

                if (originalContainer != null)
                {
                    String errorMessage = String.Empty;

                    LocaleMessage localeMessage;

                    if (originalContainer.ContainerType != container.ContainerType)
                    {
                        localeMessage = localeMessageManager.Get(_systemUILocale, "114150", false, callerContext);
                        errorMessage += localeMessage.Message + ": " + container.ContainerType + ", ";
                    }

                    if (originalContainer.ParentContainerId != container.ParentContainerId)
                    {
                        localeMessage = localeMessageManager.Get(_systemUILocale, "114152", false, callerContext);
                        errorMessage += localeMessage.Message + ": " + container.ParentContainerName + ", ";
                    }

                    if (originalContainer.HierarchyId != container.HierarchyId)
                    {
                        localeMessage = localeMessageManager.Get(_systemUILocale, "100494", false, callerContext);
                        errorMessage += localeMessage.Message + ": " + container.HierarchyShortName + ", ";
                    }

                    if (!String.IsNullOrWhiteSpace(errorMessage))
                    {
                        containerOperationResult.AddOperationResult("114151", String.Empty, referenceId, new Collection<Object> { errorMessage, container.Name }, OperationResultType.Error);
                    }
                }
            }
        }

        /// <summary>
        /// populates OperationResult
        /// </summary>
        /// <param name="reader">Sql Data reader which is used to read data</param>
        /// <param name="operationResults">Data Model Operation Results collection which is used for result</param>
        /// <param name="containers">Containers to be processed</param>
        private void PopulateLocalesOperationResult(SqlDataReader reader, DataModelOperationResultCollection operationResults, ContainerCollection containers)
        {
            while (reader.Read())
            {
                Boolean hasError = false;
                String errorCode = String.Empty;
                String localeId = String.Empty;
                String referenceId = String.Empty;

                if (reader["Id"] != null)
                {
                    referenceId = reader["Id"].ToString();
                }

                OperationResult containerOperationResult = (OperationResult)operationResults.GetByReferenceId(referenceId);

                if (reader["LocaleId"] != null)
                {
                    localeId = reader["LocaleId"].ToString();
                }

                if (reader["HasError"] != null)
                {
                    Boolean.TryParse(reader["HasError"].ToString(), out hasError);
                }

                if (reader["ErrorCode"] != null)
                {
                    errorCode = reader["ErrorCode"].ToString();
                }

                if (hasError)
                {
                    if (!String.IsNullOrEmpty(errorCode))
                    {
                        containerOperationResult.AddOperationResult(errorCode, String.Empty, OperationResultType.Error);
                    }

                    containerOperationResult.OperationResultStatus = OperationResultStatusEnum.Failed;
                }
                else if (containerOperationResult.OperationResultStatus != OperationResultStatusEnum.Failed)
                {
                    containerOperationResult.OperationResultStatus = OperationResultStatusEnum.Successful;
                }
            }
        }

        /// <summary>
        /// populates OperationResult
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="containerOperationResult"></param>
        /// <param name="containers"></param>
        private void PopulateLocalesOperationResult(SqlDataReader reader, OperationResult containerOperationResult, ContainerCollection containers)
        {
            while (reader.Read())
            {
                Boolean hasError = false;
                String errorCode = String.Empty;
                String localeId = String.Empty;
                String referenceId = String.Empty;

                if (reader["Id"] != null)
                {
                    referenceId = reader["Id"].ToString();
                }

                if (reader["LocaleId"] != null)
                {
                    localeId = reader["LocaleId"].ToString();
                }

                if (reader["HasError"] != null)
                {
                    Boolean.TryParse(reader["HasError"].ToString(), out hasError);
                }

                if (reader["ErrorCode"] != null)
                {
                    errorCode = reader["ErrorCode"].ToString();
                }

                if (hasError)
                {
                    if (!String.IsNullOrEmpty(errorCode))
                    {
                        containerOperationResult.AddOperationResult(errorCode, String.Empty, OperationResultType.Error);
                    }
                    containerOperationResult.OperationResultStatus = OperationResultStatusEnum.Failed;
                }
                else if (containerOperationResult.OperationResultStatus != OperationResultStatusEnum.Failed)
                {
                    containerOperationResult.OperationResultStatus = OperationResultStatusEnum.Successful;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="containerCollection"></param>
        /// <param name="command"></param>
        /// <param name="loginUser"></param>
        /// <param name="programName"></param>
        /// <returns></returns>
        private SqlDataReader ProcessContainerLocaleMappings(ContainerCollection containerCollection, DBCommandProperties command, string loginUser, string programName)
        {
            SqlDataReader reader;
            const String storedProcedureName = "usp_ContainerManager_ContainerLocale_Process";
            SqlParametersGenerator generator = new SqlParametersGenerator("ContainerManager_SqlParameters");
            SqlParameter[] parameters = generator.GetParameters("ContainerManager_ContainerLocale_Process_ParametersArray");
            SqlMetaData[] containerLocaleMetaData = generator.GetTableValueMetadata("ContainerManager_ContainerLocale_Process_ParametersArray", parameters[0].ParameterName);

            List<SqlDataRecord> containerLocaleTable = new List<SqlDataRecord>();

            foreach (Container container in containerCollection)
            {
                foreach (Locale locale in container.SupportedLocales)
                {
                    SqlDataRecord containerLocaleRecord = new SqlDataRecord(containerLocaleMetaData);
                    containerLocaleRecord.SetValue(0, ValueTypeHelper.Int32TryParse(container.ReferenceId, 0));
                    containerLocaleRecord.SetValue(1, ValueTypeHelper.Int32TryParse(container.ReferenceId, 0));
                    containerLocaleRecord.SetValue(2, container.Id);
                    containerLocaleRecord.SetValue(3, locale.Id);
                    containerLocaleRecord.SetValue(4, locale.Action.ToString());
                    containerLocaleTable.Add(containerLocaleRecord);
                }
            }

            if (containerLocaleTable.Count == 0)
            {
                containerLocaleTable = null;
            }

            parameters[0].Value = containerLocaleTable;
            parameters[1].Value = loginUser;
            parameters[2].Value = programName;
            parameters[3].Value = true;

            reader = ExecuteProcedureReader(command.ConnectionString, parameters, storedProcedureName);
            return reader;
        }

        #endregion Private Methods

        #endregion
    }
}