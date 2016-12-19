using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Transactions;
using System.Collections;
using System.Collections.Concurrent;

namespace MDM.AdminManager.Business
{
    using MDM.AdminManager.Data;
    using MDM.BusinessObjects;
    using MDM.Core;
    using MDM.Core.Exceptions;
    using MDM.Utility;

    /// <summary>
    /// Business Logic Layer for AppConfig business object
    /// </summary>
    public class AppConfigBL : BusinessLogicBase
    {
        #region Fields

        private AppConfigDA appConfigDA = new AppConfigDA();
        private String programName = "MDM.AdminManager.Business.AppConfigBL";
        
        #endregion

        #region CUD

        /// <summary>
        /// Create new AppConfig
        /// </summary>
        /// <param name="appConfig">Represent AppConfig Object</param>
        /// <param name="callerContext">Context of application making call for this API</param>
        /// <returns>OperationResult of AppConfig Creation</returns>
        /// <exception cref="ArgumentNullException">If AppConfig Object is Null</exception>
        public OperationResult Create(AppConfig appConfig, CallerContext callerContext)
        {
            this.ValidateAppConfig(appConfig, "Create");

            appConfig.Action = Core.ObjectAction.Create;
            return this.Process(appConfig, callerContext);
        }

        /// <summary>
        /// Update existing appConfig
        /// </summary>
        /// <param name="appConfig">Represent AppConfig Object</param>
        /// <param name="callerContext">Context of application making call for this API</param>
        /// <returns>OperationResult of AppConfig Updating</returns>
        /// <exception cref="ArgumentNullException">If AppConfig Object is Null</exception>
        /// <exception cref="ArgumentNullException">If AppConfig ShortName is Null or having empty String</exception>
        /// <exception cref="ArgumentNullException">If AppConfig LongName is Null or having empty String</exception>
        public OperationResult Update(AppConfig appConfig, CallerContext callerContext)
        {
            this.ValidateAppConfig(appConfig, "Update");

            appConfig.Action = Core.ObjectAction.Update;
            return this.Process(appConfig, callerContext);
        }

        /// <summary>
        /// Delete appConfig
        /// </summary>
        /// <param name="appConfig">Represent AppConfig Object</param>
        /// <param name="callerContext">Context of application making call for this API</param>
        /// <returns>OperationResult of AppConfig Deletion</returns>
        /// <exception cref="ArgumentNullException">If AppConfig Object is Null</exception>
        public OperationResult Delete(AppConfig appConfig, CallerContext callerContext)
        {
            this.ValidateAppConfig(appConfig, "Delete");

            appConfig.Action = Core.ObjectAction.Delete;
            return this.Process(appConfig, callerContext);
        }

        /// <summary>
        /// Process appConfigs
        /// </summary>
        /// <param name="appConfigs">AppConfigs to process</param>
        /// <param name="callerContext">Context of application making call for this API</param>
        /// <returns>OperationResult of AppConfig Creation</returns>
        public OperationResultCollection Process(AppConfigCollection appConfigs, CallerContext callerContext)
        {
            #region Parameter Validation

            if (appConfigs == null)
            {
                throw new MDMOperationException("112114", "AppConfig cannot be null", "AppConfigBL.Process", String.Empty, "Process");
            }

            #endregion Parameter Validation

            OperationResultCollection operationResults = new OperationResultCollection();

            foreach (AppConfig relType in appConfigs)
            {
                OperationResult relOR = this.Process(relType, callerContext);
                if (relOR != null)
                {
                    operationResults.Add(relOR);
                }
            }

            return operationResults;
        }

        #endregion CUD

        #region Get

        /// <summary>
        /// Get All AppConfigs from the system
        /// </summary>
        /// <param name="callerContext">Context which called the application</param>
        /// <returns>Collection of AppConfig business objects</returns>
        public AppConfigCollection GetAll(CallerContext callerContext)
        {
            MDMTraceHelper.StartTraceActivity("MDM.AdminManager.AppConfigBL.Get", false);
            MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "Start : MDM.AdminManager.AppConfigBL.Get");

            AppConfigCollection appConfigs;

            try
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "Start : DB call to get all AppConfigs ");

                appConfigs = appConfigDA.Get(0, String.Empty);

                MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "End : DB call to get all AppConfigs ");
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "End : MDM.AdminManager.AppConfigBL.Get");
            }
            finally
            {
                MDMTraceHelper.StopTraceActivity("MDM.AdminManager.AppConfigBL.Get");
            }

            return appConfigs;
        }

        /// <summary>
        /// Gets the AppConfig by name.
        /// </summary>
        /// <param name="callerContext">The caller context.</param>
        /// <param name="appConfigName">Name of the app config.</param>
        /// <returns>App Config</returns>
        public AppConfig Get(String appConfigName)
        {
            AppConfigCollection appConfigs = appConfigDA.Get(0, appConfigName);
            if (appConfigs.Count > 0)
            {
                return appConfigs.First<AppConfig>();
            }

            return null;
        }

        #endregion Get

        #region Conversion methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="appConfigs"></param>
        /// <param name="htAppConfig"></param>
        /// <returns></returns>
        public Hashtable ToHashtable(Collection<AppConfig> appConfigs, Hashtable htAppConfig)
        {

            if (htAppConfig != null)
            {

                if (appConfigs != null)
                {
                    foreach (AppConfig appConfig in appConfigs)
                    {
                        if (!String.IsNullOrEmpty(appConfig.Name) && appConfig.Value != null
                            && !htAppConfig.Contains(appConfig.Name))
                        {
                            htAppConfig.Add(appConfig.Name, appConfig.Value);
                        }
                    }
                }
            }
            return htAppConfig;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="appConfigs"></param>
        /// <param name="htAppConfig"></param>
        public void ToConcurrentDictionary(IEnumerable<AppConfig> appConfigs, ConcurrentDictionary<object, object> htAppConfig)
        {
            if (htAppConfig != null)
            {

                if (appConfigs != null)
                {
                    foreach (AppConfig appConfig in appConfigs)
                    {
                        if (!String.IsNullOrEmpty(appConfig.Name) && appConfig.Value != null
                            && !htAppConfig.ContainsKey(appConfig.Name))
                        {
                            htAppConfig.TryAdd(appConfig.Name, appConfig.Value);
                        }
                    }
                }
            }
        }

        #endregion Conversion methods

        #region Private methods

        /// <summary>
        /// Create, Update or Delete given AppConfigs
        /// </summary>
        /// <param name="appConfig">Collection of AppConfigs to process</param>
        /// <param name="callerContext">Current call context</param>
        /// <returns>
        /// Result of operation
        /// </returns>
        /// <exception cref="MDMOperationException">
        /// 111846;CallerContext cannot be null.;AppConfigBL.Process;Process
        /// or
        /// 112066;AppConfigs cannot be null.;AppConfigBL.Process;Process
        /// or
        /// </exception>
        private OperationResult Process(AppConfig appConfig, CallerContext callerContext)
        {
            OperationResult operationResult = new OperationResult();

            #region Validations

            if (callerContext == null)
            {
                throw new MDMOperationException("111846", "CallerContext cannot be null.", "AppConfigBL.Process", String.Empty, "Process");
            }

            if (appConfig == null)
            {
                throw new MDMOperationException("112114", "AppConfigs cannot be null.", "AppConfigBL.Process", String.Empty, "Process");
            }

            if (String.IsNullOrWhiteSpace(programName))
            {
                programName = "AppConfigBL.Process";
            }

            #endregion Validations

            string userName = SecurityPrincipalHelper.GetCurrentSecurityPrincipal().CurrentUserName;

            try
            {
                using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required,
                        new TransactionOptions { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
                {
                    operationResult = appConfigDA.Process(appConfig, callerContext.ProgramName, userName);

                    transactionScope.Complete();
                }

                AppConfigurationHelper.ReloadAppConfig(appConfig.Name);
            }
            catch (Exception ex)
            {
                throw new MDMOperationException(ex.Message, ex);
            }
            return operationResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="appConfig"></param>
        /// <param name="methodName"></param>
        private void ValidateAppConfig(AppConfig appConfig, String methodName)
        {
            if (appConfig == null)
            {
                throw new MDMOperationException("112114", "AppConfig cannot be null", "AppConfigBL." + methodName, String.Empty, methodName);
            }
        }

        #endregion Private methods
    }
}
