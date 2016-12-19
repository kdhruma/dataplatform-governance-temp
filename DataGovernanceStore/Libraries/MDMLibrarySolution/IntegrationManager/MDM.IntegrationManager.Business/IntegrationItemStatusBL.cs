using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Transactions;

namespace MDM.IntegrationManager.Business
{
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Integration;
    using MDM.ConfigurationManager.Business;
    using MDM.Core;
    using MDM.Core.Exceptions;
    using MDM.IntegrationManager.Data;
    using MDM.Interfaces;
    using MDM.Utility;

    public class IntegrationItemStatusBL : BusinessLogicBase
    {
        #region Fields

        private SecurityPrincipal _securityPrincipal = null;
        private IntegrationItemStatusDA _integrationItemStatusDA = new IntegrationItemStatusDA();
        private const String _processMethodName = "MDM.IntegrationManager.Business.IntegrationItemStatusBL.Process";
        private const String _updateStatusMethodName = "MDM.IntegrationManager.Business.IntegrationItemStatusBL.UpdateStatus";
        private const String _searchStatusMethodName = "MDM.IntegrationManager.Business.IntegrationItemStatusBL.SearchIntegrationItemStatus";
        private IntegrationItemDimensionTypeBL itemDimensionTypeBL = new IntegrationItemDimensionTypeBL();
        private ConnectorProfileBL connectorProfileBL = new ConnectorProfileBL();
        private MDMObjectTypeBL mdmObjectTypeBL = new MDMObjectTypeBL();

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public IntegrationItemStatusBL()
        {
            GetSecurityPrincipal();
        }

        #endregion Constructor

        #region Properties
        #endregion

        #region Methods

        #region Public Methods

        #region CUD Methods

        /// <summary>
        /// Update integration item status. Update item and dimension status for given Id/Type
        /// </summary>
        /// <param name="integrationItemStatus">Status for an item. It contains item information and status information</param>
        /// <param name="callerContext">Context of API making call to this API.</param>
        /// <returns>Result of operation.</returns>
        public OperationResult UpdateStatus(IntegrationItemStatus integrationItemStatus, CallerContext callerContext)
        {
            #region Validation

            if (integrationItemStatus == null)
            {
                String message = "IntegrationItemStatus(s) must not be null";
                throw new MDMOperationException("112961", message, _updateStatusMethodName, String.Empty, _updateStatusMethodName);
            }

            #endregion Validation

            return UpdateStatus(new IntegrationItemStatusCollection { integrationItemStatus }, callerContext).FirstOrDefault();
        }

        /// <summary>
        /// Update integration item status. Update item and dimension status for given Id/Type
        /// </summary>
        /// <param name="integrationItemStatusCollection">Collection of status for an item. It contains item information and status information</param>
        /// <param name="callerContext">Context of API making call to this API.</param>
        /// <returns>Result of operation.</returns>
        public OperationResultCollection UpdateStatus(IntegrationItemStatusCollection integrationItemStatusCollection, CallerContext callerContext)
        {
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity(_updateStatusMethodName, MDMTraceSource.Integration, false);
            }

            #region Validation

            if (integrationItemStatusCollection == null || integrationItemStatusCollection.Count < 1)
            {
                String message = "IntegrationItemStatus(s) must not be null";
                throw new MDMOperationException("112961", message, _updateStatusMethodName, String.Empty, _updateStatusMethodName);
            }

            ValidateCallerContext(callerContext);

            #endregion Validation

            if (String.IsNullOrWhiteSpace(callerContext.ProgramName))
            {
                callerContext.ProgramName = _updateStatusMethodName;
            }

            OperationResultCollection results = new OperationResultCollection();
            try
            {
                #region Fetch Data

                MDMObjectTypeCollection allMDMObjectTypes = mdmObjectTypeBL.GetAll(callerContext);
                ConnectorProfileCollection allConnectorProfiles = connectorProfileBL.GetAll(callerContext);
                IntegrationItemDimensionTypeCollection integrationItemDimensionTypes = itemDimensionTypeBL.GetAll(callerContext);

                #region Log error if any of above collection is empty

                if (allMDMObjectTypes == null || allMDMObjectTypes.Count < 1)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, "No MDMObjectTypes are configured in system.", MDMTraceSource.Integration);
                }

                if (allConnectorProfiles == null || allConnectorProfiles.Count < 1)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, "No ConnectorProfiles found in system.", MDMTraceSource.Integration);
                }

                #endregion Log error if any of above collection is empty

                #endregion Fetch data

                IntegrationItemStatusInternalCollection internalItemStatusCollection = new IntegrationItemStatusInternalCollection();
                Int32 itemStatusReferenceCounter = 0;

                foreach (IntegrationItemStatus itemStatus in integrationItemStatusCollection)
                {
                    OperationResult orItemStatus = new OperationResult();
                    results.Add(orItemStatus);

                    itemStatus.ReferenceId = itemStatusReferenceCounter++;
                    orItemStatus.ReferenceId = itemStatus.ReferenceId.ToString();

                    IntegrationItemStatusInternal internalItemStatus = new IntegrationItemStatusInternal();

                    #region Fill Id's

                    //MDMObjectType is optional. But if provided it should be correct one.
                    #region Fill MDMObjectTypeId

                    if (!String.IsNullOrWhiteSpace(itemStatus.MDMObjectTypeName))
                    {
                        IMDMObjectType mdmObjectType = allMDMObjectTypes.Get(itemStatus.MDMObjectTypeName);
                        if (mdmObjectType != null)
                        {
                            internalItemStatus.MDMObjectTypeId = mdmObjectType.Id;
                            internalItemStatus.MDMObjectTypeName = mdmObjectType.Name;

                        }
                        else
                        {
                            String message = String.Format("Cannot find MDMObjectType with MDMObjectTypeName = '{0}'. Cannot update status for IntegrationItemStatus with MDMObjectId = '{1}'", itemStatus.MDMObjectTypeName, itemStatus.MDMObjectId);
                            Collection<Object> param = new Collection<Object> { itemStatus.MDMObjectTypeName, itemStatus.MDMObjectId };
                            orItemStatus.AddOperationResult("112962", message, param, OperationResultType.Error);
                            MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, message, MDMTraceSource.Integration);

                            continue;
                        }
                    }

                    #endregion Fill MDMObjectTypeId

                    #region fill ExternalObjectTypeId

                    if (!String.IsNullOrWhiteSpace(itemStatus.ExternalObjectTypeName))
                    {
                        IMDMObjectType mdmObjectType = allMDMObjectTypes.Get(itemStatus.ExternalObjectTypeName);
                        if (mdmObjectType != null)
                        {
                            internalItemStatus.ExternalObjectTypeId = mdmObjectType.Id;
                            internalItemStatus.ExternalObjectTypeName = mdmObjectType.Name;
                        }
                        else
                        {
                            String message = String.Format("Unable to find ExternalObjectType with ExternalObjectTypeName = '{0}'.Cannot update the status for IntegrationItemStatus with MDMObjectId = '{1}'.", itemStatus.ExternalObjectTypeName, itemStatus.MDMObjectId);
                            Collection<Object> param = new Collection<Object> { itemStatus.ExternalObjectTypeName, itemStatus.MDMObjectId };
                            orItemStatus.AddOperationResult("112982", message, param, OperationResultType.Error);
                            MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, message, MDMTraceSource.Integration);

                            continue;
                        }
                    }

                    #endregion

                    //Connector info is necessary. If ConnectorShortName is not provided or not found in system, it will error out this particular record.
                    #region Fill ConnectorId

                    if (!String.IsNullOrWhiteSpace(itemStatus.ConnectorName))
                    {
                        IConnectorProfile connectorProfile = allConnectorProfiles.Get(itemStatus.ConnectorName);
                        if (connectorProfile != null)
                        {
                            internalItemStatus.ConnectorId = connectorProfile.Id;
                            internalItemStatus.ConnectorName = connectorProfile.Name;
                            internalItemStatus.ConnectorLongName = connectorProfile.LongName;
                        }
                        else
                        {
                            String message = String.Format("Cannot find ConnectorProfile with ShortName = '{0}'. Cannot update status for IntegrationItemStatus with MDMObjectId = '{1}'", itemStatus.ConnectorName, itemStatus.MDMObjectId);
                            Collection<Object> param = new Collection<Object> { itemStatus.ConnectorName, itemStatus.MDMObjectId };
                            orItemStatus.AddOperationResult("112964", message, param, OperationResultType.Error);
                            MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, message, MDMTraceSource.Integration);

                            continue;
                        }
                    }
                    else
                    {
                        String message = String.Format("ConnectorShortName is not provided. Cannot update status for IntegrationItemStatus with MDMObjectId = '{0}'.", itemStatus.MDMObjectId);
                        Collection<Object> param = new Collection<Object> { itemStatus.MDMObjectId };
                        orItemStatus.AddOperationResult("112963", message, param, OperationResultType.Error);
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, message, MDMTraceSource.Integration);

                        continue;
                    }

                    #endregion Fill ConnectorId

                    //Dimension values are optional. But if provided, it should not be empty, and respective short names with connector short name should match.
                    #region Fill DimensionTypeId
                    
                    if (itemStatus.StatusDimensionCollection != null && itemStatus.StatusDimensionCollection.Count > 0)
                    {
                        internalItemStatus.StatusDimensionInternalCollection = new IntegrationItemStatusDimensionInternalCollection();

                        foreach (IntegrationItemStatusDimension integrationItemStatusDimension in itemStatus.StatusDimensionCollection)
                        {
                            IntegrationItemStatusDimensionInternal integrationItemStatusDimensionInternal = new IntegrationItemStatusDimensionInternal();
                            integrationItemStatusDimensionInternal.IntegrationItemDimensionValue = integrationItemStatusDimension.IntegrationItemDimensionValue;

                            if (!String.IsNullOrEmpty(integrationItemStatusDimension.IntegrationItemDimensionTypeName))
                            {
                                integrationItemStatusDimensionInternal.IntegrationItemDimensionTypeName = integrationItemStatusDimension.IntegrationItemDimensionTypeName;
                                IntegrationItemDimensionType integrationItemDimensionType = null;

                                if (String.IsNullOrWhiteSpace(itemStatus.ConnectorName))
                                {
                                    String message = String.Format("ConnectorShortName is not provided. Cannot update status for IntegrationItemStatus with MDMObjectId = '{0}'.", itemStatus.MDMObjectId);
                                    Collection<Object> param = new Collection<Object> { itemStatus.MDMObjectId };
                                    orItemStatus.AddOperationResult("112963", message, param, OperationResultType.Error);
                                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, message, MDMTraceSource.Integration);
                                }
                                else
                                {
                                    integrationItemDimensionType = (IntegrationItemDimensionType)integrationItemDimensionTypes.Get(integrationItemStatusDimension.IntegrationItemDimensionTypeName, internalItemStatus.ConnectorId);
                                }
                                
                                if (integrationItemDimensionType != null)
                                {
                                    integrationItemStatusDimensionInternal.IntegrationItemDimensionTypeId = integrationItemDimensionType.Id;
                                    integrationItemStatusDimensionInternal.IntegrationItemDimensionTypeLongName = integrationItemDimensionType.LongName;
                                }
                                else
                                {
                                    String message = String.Format("Could not find Dimension Type for DimensionTypeName = '{0}'.", integrationItemStatusDimension.IntegrationItemDimensionTypeName);
                                    Collection<Object> param = new Collection<Object> { integrationItemStatusDimension.IntegrationItemDimensionTypeName };
                                    orItemStatus.AddOperationResult("112966", message, param, OperationResultType.Error);
                                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, message, MDMTraceSource.Integration);

                                    continue;
                                }
                            }
                            internalItemStatus.StatusDimensionInternalCollection.Add(integrationItemStatusDimensionInternal);
                        }
                    }

                    #endregion Fill DimensionTypeId

                    #endregion Fill Id's

                    //Copy rest of the info,
                    internalItemStatus.MDMObjectId = itemStatus.MDMObjectId;
                    internalItemStatus.ExternalId = itemStatus.ExternalId;
                    internalItemStatus.Status = itemStatus.Status;
                    internalItemStatus.Comments = itemStatus.Comments;
                    internalItemStatus.IsExternalStatus = itemStatus.IsExternalStatus;
                    internalItemStatus.StatusType = itemStatus.StatusType;

                    internalItemStatusCollection.Add(internalItemStatus);
                }

                this.Process(internalItemStatusCollection, results, callerContext);
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.StopTraceActivity(_updateStatusMethodName, MDMTraceSource.Integration);
                }
            }

            return results;
        }

        /// <summary>
        /// Update integration item status. Update item and dimension status for given Id/Type. For all inputs proper Ids are required to fetch correct result.
        /// </summary>
        /// <param name="integrationItemStatusInternalCollection">Collection of status for an item. It contains item information and status information</param>
        /// <param name="callerContext">Context of API making call to this API.</param>
        /// <returns>Result of operation.</returns>
        public OperationResultCollection Process(IntegrationItemStatusInternalCollection integrationItemStatusInternalCollection, CallerContext callerContext)
        {
            OperationResultCollection operationResults = new OperationResultCollection();

            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity(_processMethodName, MDMTraceSource.Integration, false);
            }

            #region Parameter Validation

            if (integrationItemStatusInternalCollection == null || integrationItemStatusInternalCollection.Count < 1)
            {
                String message = "IntegrationItemStatusInternal(s) must not be null";
                throw new MDMOperationException("112972", message, _processMethodName, String.Empty, _processMethodName);
            }

            ValidateCallerContext(callerContext);

            #endregion Parameter Validation

            try
            {
                operationResults = CreateOperationResults(integrationItemStatusInternalCollection);

                this.Process(integrationItemStatusInternalCollection, operationResults, callerContext);
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.StopTraceActivity(_processMethodName, MDMTraceSource.Integration);
                }
            }

            return operationResults;
        }

        #endregion CUD Methods

        #region Get/Search

        /// <summary>
        /// Search for Integration item status based on given criteria
        /// </summary>
        /// <param name="integrationItemStatusSearchCriteria">Contains search criteria for IntegrationItemStatus</param>
        /// <param name="callerContext">Indicates context of caller making call to this API</param>
        /// <returns><see cref="IntegrationItemStatusInternalCollection"/> which are matching given search criteria</returns>
        public IntegrationItemStatusInternalCollection SearchIntegrationItemStatus(IntegrationItemStatusSearchCriteria integrationItemStatusSearchCriteria, CallerContext callerContext)
        {
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity(_searchStatusMethodName, MDMTraceSource.Integration, false);
            }

            IntegrationItemStatusInternalCollection result = null;

            #region Validations

            if (integrationItemStatusSearchCriteria == null)
            {
                String message = "IntegrationItemStatusSearchCriteria cannot be null.";
                throw new MDMOperationException("112984", message, _searchStatusMethodName, String.Empty, _searchStatusMethodName);
            }

            ValidateCallerContext(callerContext);

            #endregion Validations

            DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Read);
            result = _integrationItemStatusDA.SearchIntegrationItemStatus(integrationItemStatusSearchCriteria, command);

            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StopTraceActivity(_searchStatusMethodName, MDMTraceSource.Integration);
            }

            return result;
        }

        #endregion Get/Search

        #endregion

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="integrationItemStatusInternalCollection"></param>
        /// <param name="operationResults"></param>
        /// <param name="callerContext"></param>
        private void Process(IntegrationItemStatusInternalCollection integrationItemStatusInternalCollection, OperationResultCollection operationResults, CallerContext callerContext)
        {
            if (integrationItemStatusInternalCollection != null && integrationItemStatusInternalCollection.Count > 0)
            {
                String userName = String.Empty;
                if (_securityPrincipal != null)
                {
                    userName = _securityPrincipal.CurrentUserName;
                }

                if (String.IsNullOrWhiteSpace(callerContext.ProgramName))
                {
                    callerContext.ProgramName = _processMethodName;
                }

                //Get command
                DBCommandProperties command = DBCommandHelper.Get(callerContext, Core.MDMCenterModuleAction.Update);

                using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
                {
                    _integrationItemStatusDA.Process(integrationItemStatusInternalCollection, callerContext.ProgramName, userName, operationResults, command);

                    if (operationResults.OperationResultStatus == OperationResultStatusEnum.Successful || operationResults.OperationResultStatus == OperationResultStatusEnum.None)
                    {
                        transactionScope.Complete();
                    }
                }
            }
        }

        /// <summary>
        /// Get security principal for currently logged in user
        /// </summary>
        private void GetSecurityPrincipal()
        {
            if (_securityPrincipal == null)
            {
                _securityPrincipal = SecurityPrincipalHelper.GetCurrentSecurityPrincipal();
            }
        }

        /// <summary>
        /// Validate input parameters
        /// </summary>
        private void Validate(IntegrationItemStatus integrationItemStatus, String methodName)
        {
            if (integrationItemStatus == null)
            {
                //TODO - Ira :: Localize
                throw new MDMOperationException("XXX", "IntegrationItemStatus cannot be null", "IntegrationItemStatusBL." + methodName, String.Empty, methodName);
            }
        }

        /// <summary>
        /// Validate caller context if it is null or not.
        /// </summary>
        /// <param name="callerContext">Caller context to validate</param>
        private void ValidateCallerContext(CallerContext callerContext)
        {
            if (callerContext == null)
            {
                throw new MDMOperationException("111846", "CallerContext cannot be null.", "IntegrationItemStatusBL.Process", String.Empty, "Process");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="integrationItemStatusInternalCollection"></param>
        /// <returns></returns>
        private OperationResultCollection CreateOperationResults(IntegrationItemStatusInternalCollection integrationItemStatusInternalCollection)
        {
            OperationResultCollection operationResults = new OperationResultCollection();
            if (integrationItemStatusInternalCollection != null && integrationItemStatusInternalCollection.Count > 0)
            {
                foreach (IntegrationItemStatusInternal itemStatus in integrationItemStatusInternalCollection)
                {
                    OperationResult or = new OperationResult();
                    or.OperationResultStatus = OperationResultStatusEnum.None;
                    or.ReferenceId = itemStatus.ReferenceId.ToString();

                    operationResults.Add(or);
                }
            }
            return operationResults;
        }

        #endregion Private Methods

        #endregion
    }
}