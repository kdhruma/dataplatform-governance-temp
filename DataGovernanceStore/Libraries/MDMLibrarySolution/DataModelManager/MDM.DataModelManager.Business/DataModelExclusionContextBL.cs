using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Diagnostics;

namespace MDM.DataModelManager.Business
{
    using MDM.BusinessObjects;
    using MDM.ConfigurationManager.Business;
    using MDM.Core;
    using MDM.DataModelManager.Data;
    using MDM.Utility;
    using MDM.BufferManager;

    /// <summary>
    /// Specifies the business operations for data model exclusion context
    /// </summary>
    public class DataModelExclusionContextBL : BusinessLogicBase
    {
        #region Fields

        /// <summary>
        /// Specifies data model exclusion context data access object
        /// </summary>
        private DataModelExclusionContextDA _dataModelContextDA = null;

        /// <summary>
        /// Specifies data model exclusion context buffer manager
        /// </summary>
        MappingBufferManager _mappingBufferManager = null;

        #endregion Fields

        #region Constructors
        #endregion Constructors

        #region Properties
        #endregion Properties

        #region Public Methods

        /// <summary>
        /// Gets data model exclusion context filtered by given context
        /// </summary>
        /// <param name="organizationId">Specifies Id of an organization</param>
        /// <param name="containerId">Specifies Id of container</param>
        /// <param name="entityTypeId">Specifies Id of an entity type</param>
        /// <param name="locales">Specifies data locales</param>
        /// <param name="callerContext">Specifies context of an caller</param>
        /// <returns>Data Model Exclusion context</returns>
        public DataModelExclusionContextCollection Get(Int32 organizationId, Int32 containerId, Int32 entityTypeId, Collection<LocaleEnum> locales, CallerContext callerContext)
        {
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity("DataModelExclusionContextBL.Get", MDMTraceSource.DataModel, false);
            }

            DataModelExclusionContextCollection dataModelExclusionContext = null;
            DataModelExclusionContextCollection filteredDataModelExclusionContext = null;

            try
            {
                dataModelExclusionContext = GetDataModelExclusionContexts(callerContext);

                if (locales != null && locales.Count > 0)
                {
                    filteredDataModelExclusionContext = new DataModelExclusionContextCollection();

                    foreach (LocaleEnum locale in locales)
                    {
                        filteredDataModelExclusionContext.AddRange(FilterDataModelExclusionContextByContext(dataModelExclusionContext, organizationId, containerId, entityTypeId, locale));
                    }

                    if (filteredDataModelExclusionContext != null)
                    {
                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("No. of DataModelExclusionContext loaded is {0}", filteredDataModelExclusionContext.ToXml()), MDMTraceSource.DataModel);
                        }
                    }
                    else
                    {
                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "No DataModelExclusionContext loaded", MDMTraceSource.DataModel);
                        }
                    }
                }
                else
                {
                    if (Constants.TRACING_ENABLED)
                    {
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "DataModelExclusion feature is disabled", MDMTraceSource.DataModel);
                    }
                }
            }
            catch (Exception ex)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("Exception occurred while getting dataModelExclusionContext is {0}", ex.Message), MDMTraceSource.DataModel);
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.StopTraceActivity("DataModelExclusionContextBL.Get", MDMTraceSource.DataModel);
                }
            }

            return filteredDataModelExclusionContext;
        }

        /// <summary>
        /// Gets All Data Model exclusion contexts
        /// </summary>
        /// <param name="callerContext">Specifies context of an caller</param>
        /// <returns>All Data Model Exclusion context</returns>
        public DataModelExclusionContextCollection GetAll(CallerContext callerContext)
        {
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity("DataModelExclusionContextBL.GetAll", MDMTraceSource.DataModel, false);
            }

            DataModelExclusionContextCollection dataModelExclusionContext = null;

            try
            {
                dataModelExclusionContext = GetDataModelExclusionContexts(callerContext);
            }
            catch (Exception ex)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, String.Format("Exception occurred while getting dataModelExclusionContext is {0}", ex.Message), MDMTraceSource.DataModel);
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.StopTraceActivity("DataModelExclusionContextBL.GetAll", MDMTraceSource.DataModel);
                }
            }

            return dataModelExclusionContext;
        }

        #endregion Public Methods

        #region Private Methods

        private DataModelExclusionContextCollection FilterDataModelExclusionContextByContext(DataModelExclusionContextCollection masterDataModelExclusionContext, Int32 organizationId, Int32 containerId, Int32 entityTypeId, LocaleEnum locale)
        {
            DataModelExclusionContextCollection filteredDataModelExclusionContext = null;

            if (masterDataModelExclusionContext != null)
            {
                filteredDataModelExclusionContext = new DataModelExclusionContextCollection();

                foreach (DataModelExclusionContext dataModelExclusionContext in masterDataModelExclusionContext)
                {
                    if ((dataModelExclusionContext.OrganizationId == organizationId || dataModelExclusionContext.OrganizationId == 0) &&
                        (dataModelExclusionContext.ContainerId == containerId || dataModelExclusionContext.ContainerId == 0) &&
                        (dataModelExclusionContext.EntityTypeId == entityTypeId || dataModelExclusionContext.EntityTypeId == 0) &&
                        (dataModelExclusionContext.Locale == locale || dataModelExclusionContext.Locale == LocaleEnum.UnKnown))
                    {
                        filteredDataModelExclusionContext.Add(dataModelExclusionContext);
                    }
                }
            }

            return filteredDataModelExclusionContext;
        }

        private DataModelExclusionContextCollection GetDataModelExclusionContexts(CallerContext callerContext)
        {
            Boolean isFeatureEnabled = MDMFeatureConfigHelper.IsMDMFeatureEnabled(MDMCenterApplication.MDMCenter, "ContextBasedExclusion", "1");
            DataModelExclusionContextCollection dataModelExclusionContext = null;

            if (isFeatureEnabled)
            {
                _mappingBufferManager = new MappingBufferManager();
                dataModelExclusionContext = _mappingBufferManager.FindAllDataModelExclusionContext();

                if (dataModelExclusionContext == null || dataModelExclusionContext.Count < 1)
                {
                    DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Read);
                    _dataModelContextDA = new DataModelExclusionContextDA();
                    dataModelExclusionContext = _dataModelContextDA.Get(command);

                    if (dataModelExclusionContext != null && dataModelExclusionContext.Count > 0)
                    {
                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("No. of DataModelExclusionContext loaded from database is {0}", dataModelExclusionContext.Count()), MDMTraceSource.DataModel);
                        }

                        _mappingBufferManager.UpdateDataModelExclusionContext(dataModelExclusionContext, 3);
                    }
                    else
                    {
                        if (Constants.TRACING_ENABLED)
                        {
                            MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "No DataModelExclusionContext loaded", MDMTraceSource.DataModel);
                        }
                    }
                }
            }

            return dataModelExclusionContext;
        }

        #endregion Private Methods
    }
}
