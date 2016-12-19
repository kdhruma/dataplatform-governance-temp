using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace MDM.DataModelManager.Business
{
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.DataModel;
    using MDM.BusinessObjects.Diagnostics;
    using MDM.Core;
    using MDM.Interfaces;
    using MDM.MessageManager.Business;
    using MDM.Utility;
    /// <summary>
    /// Represents business logic for Container EntityType mapping
    /// </summary>
    public class ContainerLocaleMappingBL : BusinessLogicBase, IDataModelManager
    {
        #region Fields

        /// <summary>
        /// Field denoting the security principal
        /// </summary>
        private SecurityPrincipal _securityPrincipal;

        /// <summary>
        /// Field denoting localeMessageBL
        /// </summary>
        private LocaleMessageBL _localeMessageBL = new LocaleMessageBL();

        /// <summary>
        /// Field denoting the System Data Locale
        /// </summary>
        private LocaleEnum _systemDataLocale = LocaleEnum.UnKnown;

        /// <summary>
        /// Field denoting the reference of ContainerBL. 
        /// </summary>
        private IContainerManager _iContainerManager = null;

        /// <summary>
        /// Indicates current trace settings
        /// </summary>
        private TraceSettings _currentTraceSettings = null;

        /// <summary>
        /// Indicates whether diagnostic tracing is enable or not
        /// </summary>
        private Boolean isTracingEnabled = false;

        #endregion

        #region Constructors

        /// <summary>
        /// Parameter less constructor
        /// </summary>
        public ContainerLocaleMappingBL()
        {
            _systemDataLocale = GlobalizationHelper.GetSystemDataLocale();
            GetSecurityPrincipal();
            _currentTraceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
            isTracingEnabled = _currentTraceSettings.IsBasicTracingEnabled;
        }

        /// <summary>
        /// Initializes a new instance of the ContainerLocaleMappingBL.
        /// </summary>
        /// <param name="iContainerManager">Represents the reference of ContainerBL.</param>
        public ContainerLocaleMappingBL(IContainerManager iContainerManager) : this()
        {
            this._iContainerManager = iContainerManager;
        }

        #endregion

        #region Properties
        #endregion

        #region Methods

        #region Public Methods
        
        #region IDataModelManager Methods

        /// <summary>
        /// Generate and OperationResult Schema for given DataModelObjectCollection
        /// </summary>
        /// <param name="iDataModelObjects"></param>
        /// <param name="operationResults">Collection of DataModel OperationResult</param>
        public void PrepareOperationResultsSchema(IDataModelObjectCollection iDataModelObjects, DataModelOperationResultCollection operationResults)
        {
            ((IDataModelManager)_iContainerManager).PrepareOperationResultsSchema(iDataModelObjects, operationResults);
        }

        /// <summary>
        /// Validates DataModelObjects and populate OperationResults
        /// </summary>
        /// <param name="iDataModelObjects">Collection of data model objects to be filled up.</param>
        /// <param name="operationResults">Collection of DataModel OperationResult</param>
        /// <param name="iCallerContext">Indicates the context of the caller.</param>
        /// <returns>DataModel Operation Result Collection</returns>
        public void Validate(IDataModelObjectCollection iDataModelObjects, DataModelOperationResultCollection operationResults, ICallerContext iCallerContext)
        {
            DiagnosticActivity diagnosticActivity = null;
            try
            {
                if (isTracingEnabled)
                {
                    diagnosticActivity = new DiagnosticActivity();
                    diagnosticActivity.Start();
                }

                ContainerCollection containerCollection = iDataModelObjects as ContainerCollection;
                CallerContext callerContext = iCallerContext as CallerContext;
                
                ValidateInputParameters(containerCollection, operationResults, callerContext);
                ValidateData(containerCollection, operationResults, callerContext);
            }
            catch (Exception ex)
            {
                DataModelHelper.AddOperationResults(operationResults, String.Empty, ex.Message, null, OperationResultType.Error, TraceEventType.Error, iCallerContext as CallerContext);
            }
            finally
            {
                if (isTracingEnabled)
                {
                    diagnosticActivity.Stop();
                }
            }
        }

        /// <summary>
        /// Load original data model objects
        /// </summary>
        /// <param name="iDataModelObjects">Collection of data model objects to load original object.</param>
        /// <param name="operationResult">Collection of DataModel OperationResult</param>
        /// <param name="iCallerContext">Context of caller making call to this API.</param>
        public void LoadOriginal(IDataModelObjectCollection iDataModelObjects, DataModelOperationResultCollection operationResults, ICallerContext iCallerContext)
        {
            DiagnosticActivity diagnosticActivity = null;
            try
            {
                if (isTracingEnabled)
                {
                    diagnosticActivity = new DiagnosticActivity();
                    diagnosticActivity.Start();
                }

                LoadOriginalContainerCollection(iDataModelObjects as ContainerCollection, iCallerContext as CallerContext);
            }
            finally
            {
                if (isTracingEnabled)
                {
                    diagnosticActivity.Stop();
                }
            }
        }

        /// <summary>
        /// Fills missing information to data model objects.
        /// </summary>
        /// <param name="iDataModelObjects">Collection of data model objects to be filled up.</param>
        /// <param name="iCallerContext">Context of caller making call to this API.</param>
        public void FillDataModel(IDataModelObjectCollection iDataModelObjects, ICallerContext iCallerContext)
        {
            DiagnosticActivity diagnosticActivity = null;
            try
            {
                if (isTracingEnabled)
                {
                    diagnosticActivity = new DiagnosticActivity();
                    diagnosticActivity.Start();
                }

                FillContainerCollection(iDataModelObjects as ContainerCollection);
            }
            finally
            {
                if (isTracingEnabled)
                {
                    diagnosticActivity.Stop();
                }
            }
        }

        /// <summary>
        /// Compare and merge data model object collection with current collection
        /// </summary>
        /// <param name="iDataModelObjects">Collection of data model objects to be compare and merge.</param>
        /// <param name="operationResult">Collection of DataModel OperationResult</param>
        /// <param name="iCallerContext">Context of caller making call to this API.</param>
        /// <returns>Returns merged data model objects.</returns>
        public void CompareAndMerge(IDataModelObjectCollection iDataModelObjects, DataModelOperationResultCollection operationResults, ICallerContext iCallerContext)
        {
            //Do Nothing as there is nothing to compare and merge as Container object is always taken from latest and supported locales action given by user deletes or by default set as create which DB changes as update.
        }

        /// <summary>
        /// Process data model collection
        /// </summary>
        /// <param name="iDataModelObjects">Collection of data model objects to be processed.</param>
        /// <param name="operationResult">Collection of DataModel OperationResult</param>
        /// <param name="iCallerContext">Context of caller making call to this API.</param>
        /// <returns>Result of operation</returns>
        public void Process(IDataModelObjectCollection iDataModelObjects, DataModelOperationResultCollection operationResults, ICallerContext iCallerContext)
        {
            _iContainerManager.ProcessLocaleMappings(iDataModelObjects, operationResults, iCallerContext);
        }

        ///<summary>
        /// Processes the Entity cache statuses for data model objects
        /// </summary>
        /// <param name="iDataModelObjects">Collection of data model objects to be process entity cache load.</param>
        /// <param name="operationResult">Collection of DataModel OperationResult</param>
        /// <param name="iCallerContext">Context of caller making call to this API.</param>
        public void InvalidateCache(IDataModelObjectCollection iDataModelObjects, DataModelOperationResultCollection operationResults, ICallerContext iCallerContext)
        {
            ((IDataModelManager)_iContainerManager).InvalidateCache(iDataModelObjects, operationResults, iCallerContext);
        }

        #endregion

        #endregion

        #region Private Methods

        #region Validation Methods
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="containers"></param>
        /// <param name="operationResults"></param>
        /// <param name="callerContext"></param>
        private void ValidateInputParameters(ContainerCollection containers, DataModelOperationResultCollection operationResults, CallerContext callerContext)
        {
            if (containers == null || containers.Count < 1)
            {
                DataModelHelper.AddOperationResults(operationResults, "113590", String.Empty, null, OperationResultType.Error, TraceEventType.Error, callerContext);
            }

            if (callerContext == null)
            {
                DataModelHelper.AddOperationResults(operationResults, "111846", String.Empty, null, OperationResultType.Error, TraceEventType.Error, callerContext);
            }
        }
        
        /// <summary>
        /// Validate the data of specified parameters in containers
        /// </summary>
        /// <param name="containerCollection">Indicates the collection of containers</param>
        /// <param name="operationResults">Indicates the result of operation</param>
        /// <param name="callerContext">Indicates the context of the caller</param>
        private void ValidateData(ContainerCollection containerCollection, DataModelOperationResultCollection operationResults, CallerContext callerContext)
        {
            if (containerCollection != null && containerCollection.Count > 0)
            {
                foreach (Container container in containerCollection)
                {
                    IDataModelOperationResult operationResult = operationResults.GetByReferenceId(container.ReferenceId);

                    if (String.IsNullOrWhiteSpace(container.Name))
                    {
                        DataModelHelper.AddOperationResult(operationResult, "111681", String.Empty, null, OperationResultType.Error, TraceEventType.Error, callerContext);
                    }

                    Boolean hasSystemDataLocale = false;

                    if (container.SupportedLocales != null && container.SupportedLocales.Count > 1)
                    {
                        HashSet<LocaleEnum> locales = new HashSet<LocaleEnum>();
                        String duplicateLocales = String.Empty;

                        foreach (Locale locale in container.SupportedLocales)
                        {
                            if (!locales.Contains(locale.Locale))
                            {
                                locales.Add(locale.Locale);
                            }
                            else
                            {
                                duplicateLocales = String.Concat(duplicateLocales, locale.Locale, ",");
                            }

                            if (locale.Locale == _systemDataLocale)
                            {
                                hasSystemDataLocale = true;
                                if (locale.Action == ObjectAction.Delete)
                                {
                                    Object[] paramaters = new Object[] { locale.Locale, container.Name };
                                    DataModelHelper.AddOperationResult(operationResult, "114270", String.Empty, paramaters, OperationResultType.Error, TraceEventType.Error, callerContext);
                                }
                            }
                        }

                        if (duplicateLocales.Length > 0)
                        {
                            Object[] paramaters = new Object[] { container.Name, duplicateLocales.Substring(0, duplicateLocales.Length - 1) };
                            DataModelHelper.AddOperationResult(operationResult, "114272", String.Empty, paramaters, OperationResultType.Error, TraceEventType.Error, callerContext);
                        }
                    }
                    else if (container.SupportedLocales != null && container.SupportedLocales.Count == 1 && container.SupportedLocales.First().Locale == _systemDataLocale)
                    {
                        hasSystemDataLocale = true;
                    }

                    if (!hasSystemDataLocale)
                    {
                        Object[] paramaters = new Object[] { container.Name };
                        DataModelHelper.AddOperationResult(operationResult, "114273", String.Empty, paramaters, OperationResultType.Warning, TraceEventType.Warning, callerContext);

                        container.SupportedLocales.Add(new Locale()
                        {
                            Id = (Int32)_systemDataLocale,
                            Name = _systemDataLocale.ToString(),
                            LongName = _systemDataLocale.GetDescription(),
                            Locale = _systemDataLocale,
                            Action = ObjectAction.Create
                        });
                    }
                }
            }
        }

        #endregion

        #region Get Methods
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="containerEntityTypeMappingCollection"></param>
        /// <param name="callerContext"></param>
        private void LoadOriginalContainerCollection(ContainerCollection containerCollection, CallerContext callerContext)
        {
            ContainerCollection originalContainerCollection = _iContainerManager.GetAll(callerContext, false);

            if (originalContainerCollection != null && originalContainerCollection.Count > 0)
            {
                foreach (Container container in containerCollection)
                {
                    container.OriginalContainer = (Container)originalContainerCollection.GetContainerByName(container.Name);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="containerEntityTypeMappingCollection"></param>
        /// <param name="callerContext"></param>
        private void FillContainerCollection(ContainerCollection containerCollection)
        {
            foreach (Container container in containerCollection)
            {
                //Container object has to be cloned and used as the only changing property is the supported locales.
                Container originalContainer = container.OriginalContainer;
                
                container.Id = originalContainer.Id;
                container.Name = originalContainer.Name;
                container.LongName = originalContainer.LongName;
                container.Locale = originalContainer.Locale;
                container.Action = originalContainer.Action;
                container.AuditRefId = originalContainer.AuditRefId;
                container.ExtendedProperties = originalContainer.ExtendedProperties;
                
                container.OrganizationId = originalContainer.OrganizationId;
                container.HierarchyId = originalContainer.HierarchyId;
                container.Attributes = new AttributeCollection(originalContainer.Attributes.ToList());
                container.OrganizationShortName = originalContainer.OrganizationShortName;
                container.OrganizationLongName = originalContainer.OrganizationLongName;
                container.HierarchyShortName = originalContainer.HierarchyShortName;
                container.HierarchyLongName = originalContainer.HierarchyLongName;
                container.IsDefault = originalContainer.IsDefault;
                container.SecurityObjectTypeId = originalContainer.SecurityObjectTypeId;
                container.IsStaging = originalContainer.IsStaging;
            }
        }
        
        #endregion
        
        #region Misc. Methods

        /// <summary>
        /// 
        /// </summary>
        private void GetSecurityPrincipal()
        {
            if (_securityPrincipal == null)
            {
                _securityPrincipal = SecurityPrincipalHelper.GetCurrentSecurityPrincipal();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="callerContext"></param>
        private void PopulateProgramName(CallerContext callerContext)
        {
            if (String.IsNullOrWhiteSpace(callerContext.ProgramName))
            {
                callerContext.ProgramName = "ContainerLocaleMappingBL";
            }
        }

        #endregion
        
        #endregion

        #endregion
    }
}