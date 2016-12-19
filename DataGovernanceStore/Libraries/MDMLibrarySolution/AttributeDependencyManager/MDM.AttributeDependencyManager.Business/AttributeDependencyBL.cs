using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Transactions;

namespace MDM.AttributeDependencyManager.Business
{
    using MDM.ApplicationServiceManager.Business;
    using MDM.AttributeDependencyManager.Data;
    using MDM.BufferManager;
    using MDM.BusinessObjects;
    using MDM.ConfigurationManager.Business;
    using MDM.Core;
    using MDM.Core.Exceptions;
    using MDM.Interfaces;
    using MDM.MessageManager.Business;
    using MDM.Utility;

    /// <summary>
    /// Specifies the business operations for Dependent Attribute
    /// </summary>
    public class AttributeDependencyBL : BusinessLogicBase
    {
        #region Fields

        /// <summary>
        /// Field denoting the security principal
        /// </summary>
        private SecurityPrincipal _securityPrincipal;

        /// <summary>
        /// Field denoting locale Message
        /// </summary>
        private LocaleMessage _localeMessage;

        /// <summary>
        /// Field denoting localeMessageBL
        /// </summary>
        private LocaleMessageBL _localeMessageBL = new LocaleMessageBL();

        private AttributeDependencyDA _attributeDependencyDA = new AttributeDependencyDA();

        ApplicationContextBL _applicationContextBL = new ApplicationContextBL();

        #endregion

        #region Constructors

        public AttributeDependencyBL()
        {
            GetSecurityPrincipal();
        }
        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Get dependency mapping details for requested attribute.
        /// This method will return the link table details. 
        /// For example if the the attribute is lookup then will return the WSID of the lookup table which mapped to the requested attribute.
        /// If the attribute is non-lookup attribute then will return the dependent values based on the mapping link.
        /// </summary>
        /// <param name="attributeId">Id of the attribute for which dependency details needs to be fetched</param>
        /// <param name="applicationContext">Current context of the application</param>
        /// <param name="dependentAttributeCollection">Dependent of Attribute mapping details(Parent attribute mapping) for the attribute</param>
        /// <param name="callerContext">Indicates Application and Module name by which action is being performed</param>
        /// <returns>Collection of string values</returns>
        public Collection<String> GetDependencyMappings(Int32 attributeId, ApplicationContext applicationContext, DependentAttributeCollection dependentAttributeCollection, CallerContext callerContext)
        {
            Collection<String> result = new Collection<String>();

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("AttributeDependencyManager.AttributeDependencyBL.GetDependencyMappings", MDMTraceSource.Attribute, false);

            try
            {
                #region Validations

                if (attributeId <= 0)
                {
                    if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Attribute Id is empty", MDMTraceSource.Attribute);

                    return result;
                }

                if (dependentAttributeCollection == null || (dependentAttributeCollection != null && dependentAttributeCollection.Count == 0))   //If there is no parent attribute found for requested attribute why do db has to check in link table?
                {
                    if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Attribute Id {0} there is no dependency found for this attribute. Dependency attribute details is null or empty", attributeId.ToString()), MDMTraceSource.Attribute);

                    return result;
                }

                #endregion

                DBCommandProperties command = DBCommandHelper.Get(callerContext.Application, callerContext.Module, MDMCenterModuleAction.Read);

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Attribute Id {0} - ApplicationContextId {1} ", attributeId.ToString(), applicationContext == null ? 0 : applicationContext.Id), MDMTraceSource.Attribute);

                AttributeDependencyDA attributeDependencyDA = new AttributeDependencyDA();

                Int32 applicationContexId = _applicationContextBL.GetApplicationContextId(applicationContext);

                result = attributeDependencyDA.GetDependencyMappings(attributeId, applicationContexId, this.SplitCollectionAttributeValues(dependentAttributeCollection), command);
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("AttributeDependencyManager.AttributeDependencyBL.GetDependencyMappings", MDMTraceSource.Attribute);
            }

            return result;
        }

        /// <summary>
        /// Check if given attribute's value is valid or not with respect to given dependent parent attribute's value.
        /// This API will validate the values and remove the invalid value from Attribute object and correct actions accordingly.
        /// If any attribute value is removed, respective message will be added in OperationResult.
        /// </summary>
        /// <param name="attribute">Attribute for which dependency details needs to be validated</param>
        /// <param name="applicationContext">Current context of the application</param>
        /// <param name="parentDependentAttributes">Dependent of Attribute mapping details(Parent attribute mapping) for the attribute</param>
        /// <param name="callerContext">Indicates Application and Module name by which action is being performed</param>
        /// <param name="attributeModel">Indicates model of Attribute for which dependency validation is to be done.</param>
        /// <returns>Operation result indicating if any value is deleted from attribute due to some invalid dependency.</returns>
        public IOperationResult AreDependentValuesValid(IAttribute attribute, IAttributeModel attributeModel, ApplicationContext applicationContext, DependentAttributeCollection parentDependentAttributes, CallerContext callerContext)
        {
            IOperationResult operationResult = new OperationResult();

            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity("AttributeDependencyManager.AttributeDependencyBL.AreDependentValuesValid", MDMTraceSource.Attribute, false);
            }

            try
            {
                #region Validations

                if (attribute == null )
                {
                    operationResult.AddOperationResult("", "Attribute is null. Cannot check dependent data.", OperationResultType.Error);
                    return operationResult;
                }

                if (parentDependentAttributes == null || parentDependentAttributes.Count == 0)
                {
                    if (Constants.TRACING_ENABLED)
                    {
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Attribute Id {0} there is no dependency found for this attribute. Dependency attribute details is null or empty", attribute.Id.ToString()), MDMTraceSource.Attribute);
                    }

                    return operationResult;
                }

                if (!attribute.HasValue(false))
                {
                    //Since attribute has no value, no point of validating it any further. Just say All is well.
                    return operationResult;
                }

                #endregion

                DBCommandProperties command = DBCommandHelper.Get(callerContext.Application, callerContext.Module, MDMCenterModuleAction.Read);

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Attribute Id {0} - ApplicationContextId {1} ", attribute.Id.ToString(), applicationContext == null ? 0 : applicationContext.Id), MDMTraceSource.Attribute);

                AttributeDependencyDA attributeDependencyDA = new AttributeDependencyDA();

                Int32 applicationContextId = _applicationContextBL.GetApplicationContextId(applicationContext);

                Collection<KeyValuePair<String,Boolean>> validationStatus = attributeDependencyDA.AreDependentValuesValid(attribute, applicationContextId, this.SplitCollectionAttributeValues(parentDependentAttributes), command);

                if (validationStatus != null)
                {
                    Boolean atleastOneInvalidValue = false;

                    var attrGetOverriddenValues = attribute.GetOverriddenValues();

                    if (attrGetOverriddenValues != null && attrGetOverriddenValues.Count > 0)
                    {
                        foreach (Value value in attrGetOverriddenValues)
                        {
                            if (value.AttrVal != null)
                            {
                                KeyValuePair<String, Boolean> result = default(KeyValuePair<String, Boolean>);
                             
                                foreach (var kvp in validationStatus)
                                {
                                    if (String.Compare(kvp.Key,value.AttrVal.ToString(), true) == 0)
                                    { 
                                           result = kvp;
                                           break;
                                    }
                                }

                                if (!result.Equals(default(KeyValuePair<String, Boolean>)) && !String.IsNullOrWhiteSpace(result.Key) && result.Value == false)
                                {
                                    value.Action = ObjectAction.Delete;
                                    atleastOneInvalidValue = true;
                                }
                            }
                        }
                    }
                    else
                    {
                         MDMTraceHelper.EmitTraceEvent(TraceEventType.Warning, String.Format("GetOverriddenValues: Attribute Value is null: Id - {0}", attribute.Id.ToString(), MDMTraceSource.Attribute));
                    }

                    #region Update Attribute Source Flag and Action

                    String errorCode = String.Empty;

                    if (atleastOneInvalidValue == true)
                    {
                        if (attribute.IsCollection && attribute.HasValue(false))
                        {
                            attribute.Action = ObjectAction.Update;
                            errorCode = "112250"; //'{0}' is cleared due to modified dependent parent attribute ({1}) values.
                        }
                        else
                        {
                            if (attribute.Action != ObjectAction.Ignore)
                            {
                                attribute.Action = ObjectAction.Delete;
                                errorCode = "112392"; //'{0}' values are partially cleared due to modified dependent parent attribute ({1}) values.
                            }
                        }

                        LocaleMessageBL localeMessageBL = new LocaleMessageBL();
                        String errorMessage = localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), errorCode, false, callerContext).Message; //'{0}' values are partially cleared due to modified dependent parent attribute ({1}) values.

                        attribute.SourceFlag = AttributeValueSource.Overridden;


                        UpdateDependencyAttributeErrorMessage(attributeModel, parentDependentAttributes, operationResult, errorMessage);
                    }

                    #endregion Update Attribute Source Flag and Action

                }
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.StopTraceActivity("AttributeDependencyManager.AttributeDependencyBL.AreDependentValuesValid", MDMTraceSource.Attribute);
                }
            }

            return operationResult;
        }


        /// <summary>
        /// Get the dependency Details for the requested attribute.
        /// </summary>
        /// <param name="attributeId">Id of the attribute for which dependency details needs to be fetched</param>
        /// <param name="applicationContext">Current context of the application</param>
        /// <param name="includeChildDependency">Indicates whether load children dependency details or not</param>
        /// <param name="callerContext">Indicates Application and Module name by which action is being performed</param>
        /// <returns>DependentAttributeCollection</returns>
        public DependentAttributeCollection GetDependencyDetails(Int32 attributeId, ApplicationContext applicationContext, Boolean includeChildDependency, CallerContext callerContext)
        {
            DependentAttributeCollection dependentAttributes = new DependentAttributeCollection();

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("AttributeDependencyManager.AttributeDependencyBL.GetDependencyDetails", MDMTraceSource.Attribute, false);

            try
            {
                #region Validation

                if (attributeId <= 0)
                {
                    if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Attribute Id is empty", MDMTraceSource.Attribute);

                    return dependentAttributes;
                }

                #endregion

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Attribute Id {0} - ApplicationContextId {1} ", attributeId.ToString(), applicationContext.Id), MDMTraceSource.Attribute);

                applicationContext.AttributeId = attributeId;
                Int32 applicationContexId = _applicationContextBL.GetApplicationContextId(applicationContext);

                dependentAttributes = this.GetDependencies(attributeId, applicationContexId, includeChildDependency, callerContext);
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("AttributeDependencyManager.AttributeDependencyBL.GetDependencyDetails", MDMTraceSource.Attribute);
            }

            return dependentAttributes;
        }

        /// <summary>
        /// Get the dependent attribute data for the requested link table
        /// </summary>
        /// <param name="linkTableName">Indicates the link table name</param>
        /// <param name="locale">Indicates the locale</param>
        /// <param name="callerContext">Indicates the caller context information regarding application and module.</param>
        /// <returns>Returns the dependent attribute data mapping collection for the respective link table.</returns>
        public DependentAttributeDataMapCollection GetDependentData(String linkTableName, LocaleEnum locale, IAttributeModelManager attributeModelManager, ILookupManager lookupManager, CallerContext callerContext)
        {
            DependentAttributeDataMapCollection dependentAttributeDataMaps = null;

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("AttributeDependencyManager.Get", MDMTraceSource.AttributeModel, false);

            try
            {
                #region Validation

                if (String.IsNullOrWhiteSpace(linkTableName))
                {
                    //Get localized message for system default UI locale
                    LocaleMessageBL localeMessageBL = new LocaleMessageBL();
                    LocaleMessage localeMessage = localeMessageBL.Get(MDM.Utility.GlobalizationHelper.GetSystemUILocale(), "", false, callerContext);
                    throw new MDMOperationException("112399", localeMessage.Message, "AttributeDependencyManager", String.Empty, "Get");//Failed to get dependency data. LinkTable name is not provided.
                }

                if (locale == LocaleEnum.Neutral || locale == LocaleEnum.UnKnown)
                {
                    throw new MDMOperationException("112408", "Failed to get dependent data. Locale is not available.", "LookupManager", String.Empty, "Get");
                }

                #endregion

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Requested dependent link table:{0}, locale:{1}, Application:{2} and Service:{3}", linkTableName, locale.ToString(), callerContext.Application, callerContext.Module), MDMTraceSource.AttributeModel);

                DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Read);

                dependentAttributeDataMaps = _attributeDependencyDA.GetDependentData(linkTableName, false, 0, locale, command);

                AssignDisplayValue(dependentAttributeDataMaps, attributeModelManager, lookupManager, locale, callerContext);
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("AttributeDependencyManager.Get", MDMTraceSource.AttributeModel);
            }

            return dependentAttributeDataMaps;
        }

        #region Get

        /// <summary>
        /// Get all dependencies for given attribute.
        /// </summary>
        /// <param name="attributeId">Attribute Id for which dependencies are to be selected</param>
        /// <param name="callerContext">Context indicating application making this API call.</param>
        /// <returns>Attribute Dependencies having collection of parent attribute and context for given attribute id</returns>
        public DependentAttributeCollection GetAttributeDependencies(Int32 attributeId, CallerContext callerContext)
        {
            DependentAttributeCollection dependentAttributes = new DependentAttributeCollection();

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("AttributeDependencyManager.AttributeDependencyBL.GetAttributeDependencies", MDMTraceSource.Attribute, false);

            try
            {
                #region Validation

                if (attributeId <= 0)
                {
                    throw new MDMOperationException("111646", "Attribute Id must be greater than 0", "AttributeDependencyManager.AttributeDependencyBL", String.Empty, "GetAttributeDependencies");
                }

                if (callerContext == null)
                {
                    throw new MDMOperationException("111846", "CallerContext cannot be null.", "AttributeDependencyManager.AttributeDependencyBL", String.Empty, "GetAttributeDependencies");
                }

                #endregion

                //Get the dependencies
                dependentAttributes = this.GetDependencies(attributeId, 0, false, callerContext);

                //Populate Attribute models
                //GetAttributeModels(attributeDependencies);

                //Populate Full ApplicationContext
                PopulateApplicationContext(dependentAttributes, callerContext);

            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("AttributeDependencyManager.AttributeDependencyBL.GetDependencyDetails", MDMTraceSource.Attribute);
            }

            return dependentAttributes;
        }

        #endregion Get

        #region CUD

        /// <summary>
        /// Create attribute dependency
        /// </summary>
        /// <param name="attributeId">AttributeId for which dependency is to be created</param>
        /// <param name="dependentAttribute">Attribute dependency to create</param>
        /// <param name="callerContext">Context which called the application</param>
        /// <returns>Result of the operation</returns>
        public OperationResult Create(Int32 attributeId, DependentAttribute dependentAttribute, IAttributeModelManager iAttributeModelManager, CallerContext callerContext)
        {
            ValidateInputParameters(dependentAttribute, callerContext);
            dependentAttribute.Action = ObjectAction.Create;

            return this.Process(attributeId, dependentAttribute, iAttributeModelManager, callerContext);
        }

        /// <summary>
        /// Update application context
        /// </summary>
        /// <param name="attributeId">AttributeId for which dependency is to be Updated</param>
        /// <param name="dependentAttribute">Application context to Update</param>
        /// <param name="callerContext">Context which called the application</param>
        /// <returns>Result of the operation</returns>
        public OperationResult Update(Int32 attributeId, DependentAttribute dependentAttribute, IAttributeModelManager iAttributeModelManager, CallerContext callerContext)
        {
            ValidateInputParameters(dependentAttribute, callerContext);
            dependentAttribute.Action = ObjectAction.Update;

            return this.Process(attributeId, dependentAttribute, iAttributeModelManager, callerContext);
        }

        /// <summary>
        /// Delete application context
        /// </summary>
        /// <param name="attributeId">AttributeId for which dependency is to be deleted</param>
        /// <param name="dependentAttribute">Application context to Delete</param>
        /// <param name="callerContext">Context which called the application</param>
        /// <returns>Result of the operation</returns>
        public OperationResult Delete(Int32 attributeId, DependentAttribute dependentAttribute, IAttributeModelManager iAttributeModelManager, CallerContext callerContext)
        {
            ValidateInputParameters(dependentAttribute, callerContext);
            dependentAttribute.Action = ObjectAction.Delete;

            return this.Process(attributeId, dependentAttribute, iAttributeModelManager, callerContext);
        }

        /// <summary>
        /// Create - Update or Delete given application context
        /// </summary>
        /// <param name="attributeId">AttributeId for which dependency is to be processed</param>
        /// <param name="dependentAttributes">DependentAttribute collection to process</param>
        /// <param name="callerContext">Context which called the application</param>
        /// <returns>Result of the operation</returns>
        public OperationResultCollection Process(Int32 attributeId, DependentAttributeCollection dependentAttributes, IAttributeModelManager iAttributeModelManager, CallerContext callerContext)
        {
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "CallerContext : Application - " + callerContext.Application + " Module - " + callerContext.Module, MDMTraceSource.DataModel);

            OperationResultCollection applicationContextsProcessOperationResult = new OperationResultCollection();
            OperationResultCollection dependencyProcessOperationResult = new OperationResultCollection();

            #region Validation

            if (dependentAttributes == null)
            {
                throw new MDMOperationException("112383", "Dependent Attribute cannot be null.", "AttributeDependencyManager.AttributeDependencyBL", String.Empty, "Process");
            }

            if (callerContext == null)
            {
                throw new MDMOperationException("111846", "CallerContext cannot be null.", "DependentAttributeBL.JobBL", String.Empty, "Process");
            }

            #endregion Validation

            String userName = String.Empty;
            LocaleEnum systemDataLocale = GlobalizationHelper.GetSystemDataLocale();

            userName = PopulateUserName();

            PopulateProgramName(callerContext, "AttributeDependency.ProcessDependency");

            try
            {
                using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
                {
                    CreateMissingApplicationContext(ref dependentAttributes, callerContext, applicationContextsProcessOperationResult);

                    DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Execute);

                    if (dependentAttributes.Count > 0)
                    {
                        dependencyProcessOperationResult = _attributeDependencyDA.Process(attributeId, dependentAttributes, callerContext.ProgramName, userName, systemDataLocale, command);
                        LocalizeErrors(callerContext, dependencyProcessOperationResult);
                        applicationContextsProcessOperationResult.AddRange(dependencyProcessOperationResult);

                        if (dependencyProcessOperationResult.OperationResultStatus == OperationResultStatusEnum.None || dependencyProcessOperationResult.OperationResultStatus == OperationResultStatusEnum.Successful)
                        {
                            transactionScope.Complete();
                        }
                    }
                }

                UpdateCache(attributeId, dependentAttributes, iAttributeModelManager, callerContext, dependencyProcessOperationResult);

                //_dependentAttributeBufferManager.RemoveDependentAttributes(true);

                //_localeBufferManager.RemoveAvailableLocales(true);

                //_localeBufferManager.RemoveLocalesByOrg(dependentAttribute.Id, true);
            }
            catch (Exception ex)
            {
                throw new MDMOperationException(ex.Message, ex);
            }

            return applicationContextsProcessOperationResult;
        }

        /// <summary>
        /// Create Dependent data for 1 dependency (1 dependent link table)
        /// </summary>
        /// <param name="dependentAttributeDataMaps">Dependent Attribute Data Mapping which needs to be processed</param>
        /// <param name="callerContext">Indicates Application and Module name by which action is being performed</param>
        /// <returns>Result of the process</returns>
        /// <exception cref="MDMOperationException">Thrown when no dependent map found for process</exception>
        /// <exception cref="MDMOperationException">Thrown when link Table Name not found.</exception>
        /// <exception cref="MDMOperationException">Thrown when dependent map locale not found.</exception>
        public OperationResult CreateDependentData(DependentAttributeDataMap dependentAttributeDataMap, CallerContext callerContext)
        {
            OperationResult operationResult = new OperationResult();

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("AttributeDependencyManager.AttributeDependencyBL.Create", MDMTraceSource.AttributeModelProcess, false);

            OperationResultCollection operationResults = new OperationResultCollection();

            #region Parameter Validations

            if (dependentAttributeDataMap == null)
            {
                throw new MDMOperationException("112400", "Failed to process dependent data. No dependentAttribute mapping data are there to process.", "AttributeDependencyManager", String.Empty, "CreateDependentData");
            }

            #endregion

            foreach (Row row in dependentAttributeDataMap.Rows)
            {
                row.Action = ObjectAction.Create;
            }

            operationResults = this.ProcessDependentData(new DependentAttributeDataMapCollection() { dependentAttributeDataMap }, callerContext);

            if (operationResults != null && operationResults.Count > 0)
            {
                operationResult = operationResults.FirstOrDefault();
            }

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StopTraceActivity("AttributeDependencyManager.AttributeDependencyBL.Create", MDMTraceSource.AttributeModelProcess);

            return operationResult;
        }

        /// <summary>
        /// Update  Dependent data for 1 dependency (1 dependent link table)
        /// </summary>
        /// <param name="dependentAttributeDataMaps">Dependent Attribute Data Mapping which needs to be processed</param>
        /// <param name="callerContext">Indicates Application and Module name by which action is being performed</param>
        /// <returns>Result of the process</returns>
        /// <exception cref="MDMOperationException">Thrown when no dependent map found for process</exception>
        /// <exception cref="MDMOperationException">Thrown when link Table Name not found.</exception>
        /// <exception cref="MDMOperationException">Thrown when dependent map locale not found.</exception>
        public OperationResult UpdateDependentData(DependentAttributeDataMap dependentAttributeDataMap, CallerContext callerContext)
        {
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("AttributeDependencyManager.AttributeDependencyBL.Update", MDMTraceSource.AttributeModelProcess, false);

            OperationResult operationResult = new OperationResult();

            OperationResultCollection operationResults = new OperationResultCollection();

            #region Parameter Validations

            if (dependentAttributeDataMap == null)
            {
                throw new MDMOperationException("112400", "Failed to process dependent data. No dependentAttribute mapping data are there to process.", "AttributeDependencyManager", String.Empty, "CreateDependentData");
            }

            #endregion

            foreach (Row row in dependentAttributeDataMap.Rows)
            {
                row.Action = ObjectAction.Update;
            }

            operationResults = this.ProcessDependentData(new DependentAttributeDataMapCollection() { dependentAttributeDataMap }, callerContext);

            if (operationResults != null && operationResults.Count > 0)
            {
                operationResult = operationResults.FirstOrDefault();
            }

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StopTraceActivity("AttributeDependencyManager.AttributeDependencyBL.Update", MDMTraceSource.AttributeModelProcess);

            return operationResult;
        }

        /// <summary>
        /// Delete  Dependent data for 1 dependency (1 dependent link table)
        /// </summary>
        /// <param name="dependentAttributeDataMaps">Dependent Attribute Data Mapping which needs to be processed</param>
        /// <param name="callerContext">Indicates Application and Module name by which action is being performed</param>
        /// <returns>Result of the process</returns>
        /// <exception cref="MDMOperationException">Thrown when no dependent map found for process</exception>
        /// <exception cref="MDMOperationException">Thrown when link Table Name not found.</exception>
        /// <exception cref="MDMOperationException">Thrown when dependent map locale not found.</exception>
        public OperationResult DeleteDependentData(DependentAttributeDataMap dependentAttributeDataMap, CallerContext callerContext)
        {
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("AttributeDependencyManager.AttributeDependencyBL.Delete", MDMTraceSource.AttributeModelProcess, false);

            OperationResult operationResult = new OperationResult();

            OperationResultCollection operationResults = new OperationResultCollection();

            #region Parameter Validations

            if (dependentAttributeDataMap == null)
            {
                throw new MDMOperationException("112400", "Failed to process dependent data. No dependentAttribute mapping data are there to process.", "AttributeDependencyManager", String.Empty, "CreateDependentData");
            }

            #endregion

            foreach (Row row in dependentAttributeDataMap.Rows)
            {
                row.Action = ObjectAction.Delete;
            }

            operationResults = this.ProcessDependentData(new DependentAttributeDataMapCollection() { dependentAttributeDataMap }, callerContext);

            if (operationResults != null && operationResults.Count > 0)
            {
                operationResult = operationResults.FirstOrDefault();
            }

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StopTraceActivity("AttributeDependencyManager.AttributeDependencyBL.Delete", MDMTraceSource.AttributeModelProcess);

            return operationResult;
        }

        /// <summary>
        /// Process  Dependent data for 1 dependency (1 dependent link table)
        /// </summary>
        /// <param name="dependentAttributeDataMaps">Dependent Attribute Data Mapping which needs to be processed</param>
        /// <param name="callerContext">Indicates Application and Module name by which action is being performed</param>
        /// <returns>Result of the process</returns>
        /// <exception cref="MDMOperationException">Thrown when no dependent map found for process</exception>
        /// <exception cref="MDMOperationException">Thrown when link Table Name not found.</exception>
        /// <exception cref="MDMOperationException">Thrown when dependent map locale not found.</exception>
        public OperationResult ProcessDependentData(DependentAttributeDataMap dependentAttributeDataMap, CallerContext callerContext)
        {
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("AttributeDependencyManager.AttributeDependencyBL.Process", MDMTraceSource.AttributeModelProcess, false);

            OperationResult operationResult = new OperationResult();

            OperationResultCollection operationResults = new OperationResultCollection();

            operationResults = this.ProcessDependentData(new DependentAttributeDataMapCollection() { dependentAttributeDataMap }, callerContext);

            if (operationResults != null && operationResults.Count > 0)
            {
                operationResult = operationResults.FirstOrDefault();
            }

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StopTraceActivity("AttributeDependencyManager.AttributeDependencyBL.Delete", MDMTraceSource.AttributeModelProcess);

            return operationResult;
        }

        /// <summary>
        /// Processes Multiple Dependent data
        /// </summary>
        /// <param name="dependentAttributeDataMaps">Dependent Attribute Data Mapping which needs to be processed</param>
        /// <param name="callerContext">Indicates Application and Module name by which action is being performed</param>
        /// <returns>Result of the process</returns>
        /// <exception cref="MDMOperationException">Thrown when no dependent map found for process</exception>
        /// <exception cref="MDMOperationException">Thrown when link Table Name not found.</exception>
        /// <exception cref="MDMOperationException">Thrown when dependent map locale not found.</exception>
        public OperationResultCollection ProcessDependentData(DependentAttributeDataMapCollection dependentAttributeDataMaps, CallerContext callerContext)
        {
            OperationResultCollection operationResults = new OperationResultCollection();

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("AttributeDependencyBL.ProcessDependentData", MDMTraceSource.AttributeModelProcess, false);

            #region Parameter Validations

            if (dependentAttributeDataMaps == null || dependentAttributeDataMaps.Count == 0)
            {
                throw new MDMOperationException("112400", "Failed to process dependent data. No dependentAttribute mapping data are there to process.", "AttributeDependencyManager", String.Empty, "ProcessDependentData");
            }

            StringBuilder listOfLinkTablesToProcess = new StringBuilder();
            foreach (DependentAttributeDataMap dependentAttributeDataMap in dependentAttributeDataMaps)
            {
                if (String.IsNullOrEmpty(dependentAttributeDataMap.Name))
                {
                    throw new MDMOperationException("112401", "Failed to process dependent data. LinkTableName is not available.", "AttributeDependencyManager", String.Empty, "Process");
                }

                if (dependentAttributeDataMap.Locale == LocaleEnum.UnKnown || dependentAttributeDataMap.Locale == LocaleEnum.Neutral)
                {
                    throw new MDMOperationException("112402", "Failed to process dependent data. Locale is not available.", "AttributeDependencyManager", String.Empty, "Process");
                }

                listOfLinkTablesToProcess.Append(dependentAttributeDataMap.Name);
                listOfLinkTablesToProcess.Append(", ");
            }

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("List of dependent link table names to process:{0}", listOfLinkTablesToProcess.ToString()), MDMTraceSource.AttributeModelProcess);

            #endregion

            #region Populate Ids for each Column

            if (dependentAttributeDataMaps != null)
            {
                Int32 dependentMapId = -1;

                foreach (DependentAttributeDataMap dependentAttributeDataMap in dependentAttributeDataMaps)
                {
                    //if dependent Attribute Table Id is not there put Id as -1,-2,-3..
                    if (dependentAttributeDataMap.Id < 1)
                        dependentAttributeDataMap.Id = dependentMapId--;

                    #region populate Ids for each row of link Data

                    if (dependentAttributeDataMap.Rows != null && dependentAttributeDataMap.Rows.Count > 0)
                    {
                        Int32 rowId = -1;
                        foreach (Row row in dependentAttributeDataMap.Rows)
                        {
                            //if any lookup row Id is not there put Id as -1,-2,-3..
                            if (row.Id < 1)
                                row.Id = rowId--;
                        }
                    }

                    #endregion
                }
            }
            #endregion

            #region DB Processing

            String userName = String.Empty;
            String programName = String.Empty;
            userName = PopulateUserName();

            PopulateProgramName(callerContext, "AttributeDependencyManager.AttributeDependencyBL.ProcessDependentData");
            DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Update);

            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Processing dependent link tables in database...", MDMTraceSource.AttributeModelProcess);

                operationResults = _attributeDependencyDA.ProcessDependentData(dependentAttributeDataMaps, userName, callerContext.ProgramName, command);

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Processed dependent link tables in database.", MDMTraceSource.AttributeModelProcess);

                if (operationResults.OperationResultStatus == OperationResultStatusEnum.None || operationResults.OperationResultStatus == OperationResultStatusEnum.Successful)
                    transactionScope.Complete();
            }
            #endregion

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StopTraceActivity("AttributeDependencyManager.ProcessDependentData", MDMTraceSource.AttributeModelProcess);

            return operationResults;
        }

        #endregion CUD

        #endregion

        #region Private methods

        private DependentAttributeCollection GetDependencies(Int32 attributeId, Int32 applicationContextId, Boolean includeChildDependency, CallerContext callerContext)
        {
            DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Read);
            DependentAttributeCollection dependentAttribute = null;

            AttributeDependencyDA attributeDependencyDA = new AttributeDependencyDA();
            dependentAttribute = attributeDependencyDA.GetDependencyDetails(attributeId, applicationContextId, includeChildDependency, command);
            return dependentAttribute;
        }

        private void PopulateApplicationContext(DependentAttributeCollection dependentAttributes, CallerContext callerContext)
        {

            foreach (DependentAttribute dependency in dependentAttributes)
            {
                ApplicationContextCollection updatedContexts = new ApplicationContextCollection();

                foreach (ApplicationContext context in dependency.GetApplicationContexts())
                {
                    if (context.Id > 0)
                    {
                        ApplicationContext fullContext = _applicationContextBL.GetById(context.Id, callerContext);

                        if (fullContext != null)
                        {
                            updatedContexts.Add(fullContext);
                        }
                    }
                }
                dependency.ApplicationContexts.Clear();
                dependency.ApplicationContexts = updatedContexts;
            }
        }

        private OperationResult Process(Int32 attributeId, DependentAttribute dependentAttribute, IAttributeModelManager iAttributeModelManager, CallerContext callerContext)
        {
            OperationResult or = new OperationResult();
            if (dependentAttribute != null)
            {
                OperationResultCollection orc = this.Process(attributeId, new DependentAttributeCollection { dependentAttribute }, iAttributeModelManager, callerContext);
                if (orc != null)
                {
                    or = orc.FirstOrDefault();
                }
            }
            return or;
        }

        private void PopulateProgramName(CallerContext callerContext, String defaultProgramName)
        {
            if (String.IsNullOrWhiteSpace(callerContext.ProgramName))
            {
                callerContext.ProgramName = defaultProgramName;
            }
        }

        private void PopulateOperationResult(DependentAttribute dependentAttribute, OperationResult dependentAttributeProcessOperationResult)
        {
            if (dependentAttribute.Action == ObjectAction.Create)
            {
                if (dependentAttributeProcessOperationResult.ReturnValues.Any())
                {
                    dependentAttributeProcessOperationResult.Id =
                        ValueTypeHelper.Int32TryParse(dependentAttributeProcessOperationResult.ReturnValues[0].ToString(), -1);
                }
            }
            else
            {
                dependentAttributeProcessOperationResult.Id = dependentAttribute.Id;
            }

            dependentAttributeProcessOperationResult.ReferenceId = String.IsNullOrEmpty(dependentAttribute.ReferenceId)
                ? dependentAttribute.Name
                : dependentAttribute.ReferenceId;
        }

        private static void ValidateInputParameters(DependentAttribute dependentAttribute, CallerContext callerContext)
        {
            if (callerContext == null)
            {
                throw new MDMOperationException("111846", "CallerContext cannot be null.", "AttributeDependencyManager.AttributeDependencyBL", String.Empty, "Process");
            }

            if (dependentAttribute == null)
            {
                throw new MDMOperationException("112383", "Dependent Attribute cannot be null.", "AttributeDependencyManager.AttributeDependencyBL", String.Empty, "Process");
            }
        }

        private String PopulateUserName()
        {
            String userName = String.Empty;

            if (_securityPrincipal != null)
            {
                userName = _securityPrincipal.CurrentUserName;
            }

            return userName;
        }

        private void GetSecurityPrincipal()
        {
            if (_securityPrincipal == null)
            {
                _securityPrincipal = SecurityPrincipalHelper.GetCurrentSecurityPrincipal();
            }
        }

        /// <summary>
        /// //if there are any error codes coming in OR from db, populate error messages for them
        /// </summary>
        /// <param name="callerContext">The caller context</param>
        /// <param name="contextProcessOperationResults">Operation result to be modified</param>
        private void LocalizeErrors(CallerContext callerContext, OperationResultCollection contextProcessOperationResults)
        {
            foreach (OperationResult or in contextProcessOperationResults)
            {
                foreach (Error error in or.Errors)
                {
                    if (!String.IsNullOrEmpty(error.ErrorCode))
                    {
                        _localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), error.ErrorCode, false, callerContext);

                        if (_localeMessage != null)
                        {
                            error.ErrorMessage = _localeMessage.Message;
                        }
                    }
                }
            }
        }

        private void CreateMissingApplicationContext(ref DependentAttributeCollection dependentAttributes, CallerContext callerContext, OperationResultCollection dependencyOperationResultCollection)
        {
            DependentAttributeCollection updatedDependentAttributes = new DependentAttributeCollection();
            ApplicationContextBL contextBL = new ApplicationContextBL();
            Int32 contextId = 0;

            foreach (DependentAttribute dependentAttr in dependentAttributes)
            {
                if (dependentAttr.Action == ObjectAction.Create || dependentAttr.Action == ObjectAction.Update)
                {
                    if (dependentAttr.ApplicationContexts.Count() > 0)
                    {
                        Boolean continueProcess = true;
                        ApplicationContextCollection applicationContextCollection = new ApplicationContextCollection();
                        //Try to get context Id
                        foreach (ApplicationContext context in dependentAttr.ApplicationContexts)
                        {
                            if (context.Action == ObjectAction.Create)
                            {
                                contextId = contextBL.GetApplicationContextId(context, true);

                                //If context not found, create the context and update the Id.
                                if (contextId < 1)
                                {
                                    OperationResult contextOperationResult = null;

                                    contextOperationResult = contextBL.Create(context, callerContext); // Create will update the Id.

                                    if (contextOperationResult != null && contextOperationResult.OperationResultStatus == OperationResultStatusEnum.Failed)
                                    {
                                        OperationResult dependencyOperationResult = new OperationResult();
                                        dependencyOperationResult.Id = dependentAttr.Id;
                                        continueProcess = false;
                                        dependencyOperationResult.CopyErrorAndInfo(contextOperationResult);
                                        dependencyOperationResultCollection.Add(dependencyOperationResult);
                                    }
                                }
                                else
                                {
                                    context.Id = contextId;
                                }
                            }
                            if (context.Action != ObjectAction.Read)
                            {
                                applicationContextCollection.Add(context);
                                dependentAttr.ApplicationContexts = applicationContextCollection;
                            }
                        }

                        if (continueProcess == true)
                        {
                            updatedDependentAttributes.Add(dependentAttr);
                        }
                    }
                    else
                    {
                        ApplicationContext defaultApplicationContext = new ApplicationContext(0, 0, 0, 0, 0, 0, 0, LocaleEnum.UnKnown.ToString(), 0, 0, ApplicationContextType.DA);
                        contextId = contextBL.GetApplicationContextId(defaultApplicationContext, true);
                        defaultApplicationContext.Id = contextId;
                        dependentAttr.AddApplicationContext(defaultApplicationContext);
                        updatedDependentAttributes.Add(dependentAttr);
                    }
                }
                else if (dependentAttr.Action == ObjectAction.Delete)
                {
                    updatedDependentAttributes.Add(dependentAttr);

                }
            }

            dependentAttributes = updatedDependentAttributes;
        }

        private DependentAttributeCollection SplitCollectionAttributeValues(DependentAttributeCollection dependentAttributes)
        {
            DependentAttributeCollection result = new DependentAttributeCollection();

            if (dependentAttributes != null)
            {
                foreach (DependentAttribute dAttr in dependentAttributes)
                {
                    if (dAttr.IsCollection || dAttr.AttributeValue.Contains("$@$"))
                    {
                        String attributeValue = dAttr.AttributeValue;

                        if (!String.IsNullOrWhiteSpace(attributeValue))
                        {
                            String[] values = attributeValue.Split(new String[] { "$@$" }, StringSplitOptions.None);

                            foreach (String val in values)
                            {
                                DependentAttribute tempAttr = new DependentAttribute();
                                tempAttr.AttributeId = dAttr.AttributeId;
                                tempAttr.SetAttributeValue(val);

                                result.Add(tempAttr);
                            }

                        }
                        else
                        {
                            result.Add(dAttr);  //If the attribute is empty also add. Because procedure will handle this due to the reverse dependency support in future.
                        }

                    }
                    else
                    {
                        result.Add(dAttr);
                    }
                }
            }

            return result;
        }

        private void AssignDisplayValue(DependentAttributeDataMapCollection dependentAttributeDataMaps, IAttributeModelManager attributeModelManager, ILookupManager lookupModelManager, LocaleEnum locale, CallerContext callerContext)
        {
            if (dependentAttributeDataMaps != null && dependentAttributeDataMaps.Count > 0)
            {
                Collection<Int32> attributeIds = new Collection<Int32>();

                foreach (DependentAttributeDataMap dependentAttributeDataMap in dependentAttributeDataMaps)
                {
                    foreach (Column column in dependentAttributeDataMap.Columns)
                    {
                        if (column.Id > 0)
                            attributeIds.Add(column.Id);

                    }
                }

                AttributeModelContext attributeModelContext = new AttributeModelContext();
                attributeModelContext.AttributeModelType = AttributeModelType.AttributeMaster;
                attributeModelContext.Locales.Add(locale);

                AttributeModelCollection attributeModels = attributeModelManager.GetByIds(attributeIds, attributeModelContext);
                Lookup lookupData = null;

                foreach (AttributeModel attributeModel in attributeModels)
                {
                    if (attributeModel.AttributeDisplayTypeName == AttributeDisplayType.LookupTable.ToString())
                    {
                        lookupData = (Lookup)lookupModelManager.Get(attributeModel.Id, locale, -1, callerContext);

                        foreach (DependentAttributeDataMap dataMap in dependentAttributeDataMaps)
                        {
                            foreach (Row row in dataMap.GetRows())
                            {
                                Cell cell = row.GetCell(attributeModel.Id);
                                if (cell != null)
                                {
                                    Object val = cell.Value;
                                    if (val != null)
                                    {
                                        Int32 wsId = ValueTypeHelper.Int32TryParse(val.ToString(), 0);
                                        if (wsId > 0)
                                        {
                                            String displayFormat = lookupData.GetDisplayFormatById(wsId);
                                            if (cell.ExtendedProperties == null)
                                            {
                                                cell.ExtendedProperties = new System.Collections.Hashtable();
                                            }
                                            cell.ExtendedProperties.Add(wsId, displayFormat);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void UpdateCache(Int32 attributeId, DependentAttributeCollection dependentAttributes, IAttributeModelManager iAttributeModelManager, CallerContext callerContext, OperationResultCollection dependencyProcessOperationResult)
        {
            #region Cache update

            if (dependencyProcessOperationResult.OperationResultStatus == OperationResultStatusEnum.None || dependencyProcessOperationResult.OperationResultStatus == OperationResultStatusEnum.Successful)
            {
                Collection<int> attributeIds = new Collection<int>();

                foreach (DependentAttribute dependentAttribute in dependentAttributes)
                {
                    if (!attributeIds.Contains(dependentAttribute.AttributeId))
                    {
                        attributeIds.Add(dependentAttribute.AttributeId);
                    }

                    if (!attributeIds.Contains(dependentAttribute.ParentAttributeId))
                    {
                        attributeIds.Add(dependentAttribute.ParentAttributeId);
                    }
                }

                if (!attributeIds.Contains(attributeId))
                {
                    attributeIds.Add(attributeId);
                }

                if (attributeIds.Count > 0)
                {
                    var attributeModelBufferManager = new AttributeModelBufferManager();
                    attributeModelBufferManager.RemoveBaseAttributeModels();
                    attributeModelBufferManager.InvalidateImpactedAttributeModels(attributeIds, iAttributeModelManager, false, true);
                }
            }

            #endregion
        }

        private void UpdateDependencyAttributeErrorMessage(IAttributeModel model, DependentAttributeCollection parentDependentAttrs, IOperationResult operationResult, String errorMessage)
        {
            if (!String.IsNullOrWhiteSpace(errorMessage))
            {
                //This will be the additional over head but for parent attribute name is required in the warning message so...
                if (model != null && model.IsDependentAttribute)
                {
                    StringBuilder sb = new StringBuilder();
                    Boolean addSeperator = false;

                    foreach (DependentAttribute dAttr in model.DependentParentAttributes)
                    {
                        IDependentAttribute dependentAttribute = parentDependentAttrs.GetDependentAttribute(dAttr.AttributeId);

                        if (!String.IsNullOrWhiteSpace(dependentAttribute.AttributeValue))
                        {
                            if (addSeperator)
                            {
                                sb.Append(",");
                            }

                            //sb.Append(dependentAttribute.AttributeName + "[ " + dependentAttribute.ParentAttributeName + " ]");
                            sb.Append(dependentAttribute.AttributeName);

                            addSeperator = true;
                        }
                    }

                    operationResult.AddOperationResult(String.Empty, String.Format(errorMessage, model.LongName, sb.ToString()), OperationResultType.Warning);   ////'{0}' is cleared due to modified dependent parent attribute ({1}) values.
                    if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Warning, String.Format(errorMessage, model.LongName, sb.ToString()));
                }
            }
        }

        #endregion Private methods

        #endregion
    }
}