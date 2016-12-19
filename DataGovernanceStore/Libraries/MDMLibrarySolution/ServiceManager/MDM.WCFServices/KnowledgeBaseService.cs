using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.ServiceModel;

namespace MDM.WCFServices
{
    using ContainerManager.Business;
    using MDM.BusinessObjects;
    using MDM.Core;
    using MDM.KnowledgeManager.Business;
    using MDM.LookupManager.Business;
    using MDM.Utility;
    using MDM.WCFServiceInterfaces;

    [DiagnosticActivity]
    [ServiceBehavior(Namespace = "http://wcfservices.riversand.com", InstanceContextMode = InstanceContextMode.PerCall)]
    public class KnowledgeBaseService : MDMWCFBase, IKnowledgeBaseService
    {
        /// <summary>
        /// Returns locale object by provided id
        /// </summary>
        /// <param name="localeId">The id of locale</param>
        /// <returns>The resulting locale object</returns>
        public Locale GetLocale(Int32 localeId)
        {
            Locale locale;

            try
            {
                LocaleBL localeManager = new LocaleBL();
                locale = localeManager.Get(localeId);
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw WrapException(ex);
            }

            return locale;
        }

        /// <summary>
        /// Returns all available locales
        /// </summary>
        /// <returns>The collection of locales</returns>
        public LocaleCollection GetAvailableLocales()
        {
            LocaleCollection result;

            try
            {
                LocaleBL localeManager = new LocaleBL();
                result = localeManager.GetAvailableLocales();
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw WrapException(ex);
            }

            return result;
        }

        /// <summary>
        /// Returns all locales
        /// </summary>
        /// <returns>The collection of locales</returns>
        public LocaleCollection GetAllLocales()
        {
            LocaleCollection result = MakeBusinessLogicCall<LocaleBL, LocaleCollection>(bl=>bl.GetAll());
            return result;
        }

        /// <summary>
        /// Get all locales by container id.
        /// </summary>
        /// <param name="containerId">This parameter is specifying container id.</param>
        /// <returns>Collection of locales</returns>
        public LocaleCollection GetLocalesByContainer(Int32 containerId)
        {
            Container container = MakeBusinessLogicCall<ContainerBL, Container>(
                bl => bl.GetById(containerId),
                context =>
                {
                    context.CallDataContext.ContainerIdList.Add(containerId);
                });
            return container == null ? null : container.SupportedLocales;
        }
        

        /// <summary>
        /// Get all the lookup table names from system.
        /// </summary>
        /// <param name="callerContext">Indicates the name of application and the module that are performing the action</param>
        /// <returns>Returns the collection of lookup table names</returns>
        public Collection<String> GetAllLookupTableNames(CallerContext callerContext, LookupContext lookupContext = null)
        {
            return MakeBusinessLogicCall<LookupBL, Collection<String>>("GetAllLookupTableNames",
                                        businessLogic => businessLogic.GetAllLookupTableNames(callerContext, lookupContext));
        }

        #region Private Methods

        /// <summary>
        /// Makes calls of KnowledgeBase Business logic.
        /// Also emits traces when necessary
        /// </summary>
        /// <typeparam name="TResult">The type of the result of method of business logic</typeparam>
        /// <typeparam name="TBusinessLogic">Type of business logic</typeparam>
        /// <param name="methodName">Name of the method for tracing</param>
        /// <param name="call">The call delegate to be executed against business logic instance</param>
        /// <returns>The value returned by business logic or default</returns>
        private TResult MakeBusinessLogicCall<TBusinessLogic, TResult>(String methodName, Func<TBusinessLogic, TResult> call, MDMTraceSource traceSource = MDMTraceSource.APIFramework) where TBusinessLogic : BusinessLogicBase, new()
        {
            MDMTraceHelper.InitializeTraceSource();
            MDMTraceHelper.StartTraceActivity("KnowledgeBaseService." + methodName, traceSource, false);

            TResult operationResult;

            try
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("KnowledgeBaseService receives {0} request message.", methodName), traceSource);

                operationResult = call(new TBusinessLogic());

                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("KnowledgeBaseService receives {0} response message.", methodName), traceSource);
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw base.WrapException(ex);
            }

            MDMTraceHelper.StopTraceActivity("KnowledgeBaseService." + methodName, traceSource);

            return operationResult;
        }

        #endregion #region Private Methods
    }
}
