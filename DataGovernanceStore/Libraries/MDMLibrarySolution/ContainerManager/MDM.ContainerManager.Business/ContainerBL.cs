using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Transactions;
using System.Xml;

namespace MDM.ContainerManager.Business
{
    using Core;
    using Utility;
    using AdminManager.Business;
    using AttributeModelManager.Business;
    using BufferManager;
    using BusinessObjects;
    using BusinessObjects.Diagnostics;
    using SP = Riversand.StoredProcedures;
    using Core.Exceptions;
    using Data;
    using Interfaces;
    using LookupManager.Business;
    using MDM.ActivityLogManager.Business;
    using MDM.BusinessObjects.DataModel;
    using MDM.Core.Extensions;
    using MDM.HierarchyManager.Business;
    using MDM.OrganizationManager.Business;
    using MDM.MessageManager.Business;
    using MDM.ConfigurationManager.Business;

    /// <summary>
    ///  Specifies business logic operations for Container.
    /// </summary>
    public class ContainerBL : BusinessLogicBase, IContainerManager, IDataModelManager
    {
        #region Fields

        /// <summary>
        /// 
        /// </summary>
        private LocaleMessageBL _localeMessageBL = null;

        /// <summary>
        /// 
        /// </summary>
        private LocaleMessage _localeMessage = null;

        /// <summary>
        /// 
        /// </summary>
        private LocaleEnum _systemDataLocale = LocaleEnum.UnKnown;

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

        #endregion Fields

        #region Constructors

        public ContainerBL()
        {
            _localeMessageBL = new LocaleMessageBL();
            _systemDataLocale = GlobalizationHelper.GetSystemDataLocale();
            _systemUILocale = GlobalizationHelper.GetSystemUILocale();
            _currentTraceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
            isTracingEnabled = _currentTraceSettings.IsBasicTracingEnabled;
        }

        #endregion Constructors

        #region Properties
        #endregion

        #region Methods

        #region Public Methods

        #region Get Methods

        #region Legacy Get Methods

        /// <summary>
        /// Get container by id
        /// </summary>
        /// <param name="id">Id of container to fetch</param>
        /// <returns>Container having given Id</returns>
        public Container GetById(Int32 id)
        {
            if (id < 1)
            {
                throw new MDMOperationException("111821", "ContainerId must be greater than 0.", "ContainerManager.ContainerBL", String.Empty, "GetById");
            }

            var callerContext = new CallerContext(MDMCenterApplication.MDMCenter, MDMCenterModules.Modeling);
            var containerContext = new ContainerContext(false, false, true); //This is legacy method and would not apply security for get request

            return Get(id, containerContext, callerContext);
        }

        /// <summary>
        /// Get all entity types 
        /// </summary>
        /// <returns>All containers</returns>
        public Collection<Container> GetAllContainers()
        {
            string userName = SecurityPrincipalHelper.GetCurrentSecurityPrincipal().CurrentUserName;
            string strXMLData = SP.Catalog.GetCatalogPermissionsByOrg(userName,
                                                                userName,
                                                                0,
                                                                1, //Change the localeID
                                                                0,
                                                                9999,
                                                                "LongName",
                                                                "LongName",
                                                                "",
                                                                0,
                                                                false,
                                                                false,
                                                                true,
                                                                false,
                                                                true,
                                                                true);

            Collection<Container> containers = new Collection<Container>();
            if (!string.IsNullOrEmpty(strXMLData))
            {
                containers = this.Convert(strXMLData);
            }
            return containers;
        }

        #endregion

        #region Current Get methods

        /// <summary>
        /// Gets all containers from all organizations
        /// </summary>
        /// <param name="callerContext"></param>
        /// <param name="applySecurity"></param>
        /// <returns>ContainerCollection</returns>
        public ContainerCollection GetAll(CallerContext callerContext, Boolean applySecurity = true)
        {
            ContainerContext containerContext = new ContainerContext(false, applySecurity);
            return GetAll(containerContext, callerContext);
        }

        /// <summary>
        /// Gets all containers from all organizations
        /// </summary>
        /// <param name="callerContext"></param>
        /// <param name="containerContext">context indicates the properties like load attributes or not</param>        
        /// <param name="applySecurity"></param>
        /// <returns>ContainerCollection</returns>
        public ContainerCollection GetAll(ContainerContext containerContext, CallerContext callerContext)
        {
            var containerCollection = new ContainerCollection();

            #region Step : Diagnostic Activity initialization

            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();

            String traceMessage = String.Empty;

            if (isTracingEnabled)
            {
                diagnosticActivity.Start();
                traceMessage = String.Format("Starting get all containers internal execution flow for container context:{0} and caller context:{1}", containerContext.ToXml(), callerContext.ToXml());
                diagnosticActivity.LogInformation(traceMessage);
            }

            #endregion

            try
            {
                String errorMessage;

                #region Validation

                if (callerContext == null)
                {
                    errorMessage = "CallerContext cannot be null.";
                    throw new MDMOperationException("111846", errorMessage, "ContainerManager.ContainerBL", String.Empty, "Get");
                }

                if (containerContext == null)
                {
                    errorMessage = "ContainerContext cannot be null.";
                    throw new MDMOperationException("112120", errorMessage, "ContainerManager.ContainerBL", String.Empty, "Get");
                }

                if (isTracingEnabled)
                {
                    diagnosticActivity.LogDurationInfo("Validation is completed.");
                }

                #endregion

                Collection<UserAction> containerObjectPermissionSet;
                Dictionary<Int32, Container> allcontainers = GetAllContainers(containerContext, callerContext);
                Dictionary<Int32, Container> containers = null;

                if (containerContext.ApplySecurity)
                {
                    containers = ApplyContainerSecurity(allcontainers, containerContext, callerContext, out containerObjectPermissionSet);
                    containerCollection.PermissionSet = containerObjectPermissionSet;
                }
                else
                {
                    containers = allcontainers;
                }

                if (containers != null && containers.Count > 0)
                {
                    foreach (var container in containers.Values)
                    {
                        containerCollection.Add(container);
                    }
                }
            }
            catch (Exception ex)
            {
                if (isTracingEnabled)
                {
                    diagnosticActivity.LogError(ex.Message);
                }

                throw;
            }
            finally
            {
                #region Step: Final Diagnostics and tracing

                if (isTracingEnabled)
                {
                    diagnosticActivity.LogMessageWithData("See 'View Data' for loaded all containers.", containerCollection.ToXml());
                    diagnosticActivity.Stop();
                }

                #endregion
            }

            return containerCollection;
        }

        /// <summary>
        /// Gets container by Id
        /// </summary>
        /// <param name="containerId">Indicates containerId</param>
        /// <param name="callerContext">context indicates the properties of the caller who called the API like program name,Module</param>
        /// <param name="applySecurity"></param>
        /// <returns>Container</returns>
        public Container Get(Int32 containerId, CallerContext callerContext, Boolean applySecurity = true)
        {
            ContainerContext containerContext = new ContainerContext(false, applySecurity);
            return this.Get(containerId, containerContext, callerContext);
        }

        /// <summary>
        /// Gets container by Id
        /// </summary>
        /// <param name="containerId">Indicates containerId</param>
        /// <param name="containerContext">context indicates the properties like load attributes or not</param>
        /// <param name="callerContext">context indicates the properties of the caller who called the API like program name,Module</param>
        /// <returns>Container</returns>
        public Container Get(Int32 containerId, ContainerContext containerContext, CallerContext callerContext)
        {
            Container container = null;

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("ContainerBL.Get", MDMTraceSource.General, false);

            try
            {
                #region Validation

                String errorMessage = String.Empty;

                if (containerId <= 0)
                {
                    errorMessage = this.GetSystemLocaleMessage("111821", callerContext).Message;
                    throw new MDMOperationException("111821", errorMessage, "ContainerManager.ContainerBL", String.Empty, "Get");
                }

                if (containerContext == null)
                {
                    errorMessage = "ContainerContext cannot be null.";
                    throw new MDMOperationException("112120", errorMessage, "ContainerManager.ContainerBL", String.Empty, "Get");
                }

                if (callerContext == null)
                {
                    errorMessage = "CallerContext cannot be null.";
                    throw new MDMOperationException("111846", errorMessage, "ContainerManager.ContainerBL", String.Empty, "Get");
                }

                #endregion Validaton

                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Container Id : " + containerId.ToString());
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "CallerContext : Application - " + callerContext.Application + " Module - " + callerContext.Module);
                }

                Collection<UserAction> containerObjectPermissionSet;
                Dictionary<Int32, Container> containers = GetAllContainers(containerContext, callerContext);

                if (containers != null && containers.Count > 0 && containers.ContainsKey(containerId))
                {
                    container = containers[containerId];
                }

                if (containerContext.ApplySecurity)
                {
                    container = ApplyContainerSecurity(container, containerContext, callerContext, out containerObjectPermissionSet);
                }
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("ContainerBL.Get", MDMTraceSource.General);
            }

            return container;
        }

        /// <summary>
        /// Gets all containers from all organizations
        /// </summary>
        /// <param name="containerIdList">Indicates list of container ids which needs to get</param>
        /// <param name="callerContext">Indicates the context properties of the caller who called the API like program name,Module</param>n
        /// <param name="applySecurity">Indicates whether to apply security or not</param>
        /// <returns>ContainerCollection</returns>
        public ContainerCollection GetByIds(Collection<Int32> containerIdList, CallerContext callerContext, Boolean applySecurity = true)
        {
            var diagnosticActivity = new DiagnosticActivity();

            ContainerCollection containers = new ContainerCollection();

            try
            {
                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.Start();
                }

                if (containerIdList == null || containerIdList.Count < 1)
                {
                    _localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), "113960", new Object[] { "ContainerIdList" }, false, callerContext);
                    diagnosticActivity.LogError("113960", _localeMessage.Message);
                    throw new MDMOperationException("113960", _localeMessage.Message, "ContainerBL.GetByIds", String.Empty, "GetByIds");
                }

                ContainerContext containerContext = new ContainerContext(false, applySecurity);
                var containerCollection = GetAll(containerContext, callerContext);

                if (containerCollection != null && containerCollection.Any())
                {
                    foreach (Container container in containerCollection)
                    {
                        if (containerIdList.Contains(container.Id))
                        {
                            if (_currentTraceSettings.IsBasicTracingEnabled)
                            {
                                diagnosticActivity.LogInformation(String.Format("Requested container id :{0} ", container.Id));
                            }

                            containers.Add(container);
                        }
                    }
                }
            }
            finally
            {
                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.Stop();
                }
            }

            return containers;
        }


        /// <summary>
        /// Gets container by ShortName
        /// </summary>
        /// <param name="containerName">container Short Name</param>
        /// <param name="callerContext">context indicates the properties of the caller who called the API like program name,Module</param>
        /// <param name="applySecurity"></param>
        /// <returns>Container</returns>
        public Container Get(String containerName, CallerContext callerContext, Boolean applySecurity = true)
        {
            ContainerContext containerContext = new ContainerContext(false, applySecurity);
            return this.Get(containerName, containerContext, callerContext);
        }

        /// <summary>
        /// Gets container by ShortName
        /// </summary>
        /// <param name="containerName">container Short Name</param>
        /// <param name="containerContext">context indicates the properties like load attributes or not</param>        
        /// <param name="callerContext">context indicates the properties of the caller who called the API like program name,Module</param>
        /// <returns>Container</returns>
        public Container Get(String containerName, ContainerContext containerContext, CallerContext callerContext)
        {
            Container container = null;

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("ContainerBL.Get", MDMTraceSource.General, false);

            try
            {
                #region Validation

                String errorMessage = String.Empty;

                if (String.IsNullOrWhiteSpace(containerName))
                {
                    errorMessage = this.GetSystemLocaleMessage("111681", callerContext).Message;
                    throw new MDMOperationException("111681", errorMessage, "ContainerManager.ContainerBL", String.Empty, "Get");
                }

                if (callerContext == null)
                {
                    errorMessage = "CallerContext cannot be null.";
                    throw new MDMOperationException("111846", errorMessage, "ContainerManager.ContainerBL", String.Empty, "Get");
                }

                if (containerContext == null)
                {
                    errorMessage = "ContainerContext cannot be null.";
                    throw new MDMOperationException("112120", errorMessage, "ContainerManager.ContainerBL", String.Empty, "Get");
                }

                #endregion Validation

                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Container ShortName : " + containerName);
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "CallerContext : Application - " + callerContext.Application + " Module - " + callerContext.Module);
                }

                Collection<UserAction> containerObjectPermissionSet;
                Dictionary<Int32, Container> containers = GetAllContainers(containerContext, callerContext);

                if (containers != null && containers.Count > 0)
                {
                    foreach (var currentContainer in containers.Values)
                    {
                        if (String.Compare(currentContainer.Name, containerName, true) == 0)
                        {
                            container = currentContainer;
                            break;
                        }
                    }
                }

                if (container != null && containerContext.ApplySecurity)
                {
                    container = ApplyContainerSecurity(container, containerContext, callerContext, out containerObjectPermissionSet);
                }
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("ContainerBL.Get", MDMTraceSource.General);
            }

            return container;
        }

        /// <summary>
        /// Gets container based on container shortname and organization name
        /// </summary>
        /// <param name="containerName"></param>
        /// <param name="organizationId"></param>     
        /// <param name="callerContext">context indicates the properties of the caller who called the API like program name,Module</param>
        /// <param name="applySecurity"></param>
        /// <returns>Container</returns>
        public Container Get(String containerName, Int32 organizationId, CallerContext callerContext, Boolean applySecurity = true)
        {
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("ContainerBL.Get", MDMTraceSource.General, false);
            Container returnContainer = null;

            try
            {
                #region Validations

                String errorMessage;

                if (String.IsNullOrWhiteSpace(containerName))
                {
                    errorMessage = this.GetSystemLocaleMessage("111681", callerContext).Message;
                    throw new MDMOperationException("111681", errorMessage, "ContainerManager.ContainerBL", String.Empty, "Get");
                }

                if (callerContext == null)
                {
                    errorMessage = "CallerContext cannot be null.";
                    throw new MDMOperationException("111846", errorMessage, "ContainerManager.ContainerBL", String.Empty, "Get");
                }

                #endregion Validations

                Collection<UserAction> containerObjectPermissionSet;
                ContainerContext containerContext = new ContainerContext(false, applySecurity);

                Dictionary<Int32, Container> containers = GetAllContainers(containerContext, callerContext);

                if (containers != null && containers.Count > 0)
                {
                    Func<Container, Boolean> compareMethod;

                    if (organizationId > 0)
                    {
                        compareMethod = container => ((String.Compare(container.Name, containerName, StringComparison.InvariantCultureIgnoreCase) == 0 &&
                                         container.OrganizationId == organizationId) ||
                                         (String.Compare(container.Name, containerName, StringComparison.InvariantCultureIgnoreCase) == 0));
                    }
                    else
                    {
                        compareMethod = (container => String.Compare(container.Name, containerName, StringComparison.InvariantCultureIgnoreCase) == 0);
                    }

                    foreach (Container container in containers.Values)
                    {
                        if (compareMethod(container))
                        {
                            returnContainer = container;
                        }
                    }

                    if (returnContainer != null && applySecurity)
                    {
                        returnContainer = ApplyContainerSecurity(returnContainer, containerContext, callerContext, out containerObjectPermissionSet);
                    }
                }
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("ContainerBL.Get", MDMTraceSource.General);
            }

            return returnContainer;
        }

        /// <summary>
        /// Gets container based on container shortname and organization name
        /// </summary>
        /// <param name="containerName"></param>
        /// <param name="organizationName"></param>     
        /// <param name="callerContext">context indicates the properties of the caller who called the API like program name,Module</param>
        /// <param name="applySecurity"></param>
        /// <returns>Container</returns>
        public Container Get(String containerName, String organizationName, CallerContext callerContext, Boolean applySecurity = true)
        {
            ContainerContext containerContext = new ContainerContext(false, applySecurity);
            return this.Get(containerName, organizationName, containerContext, callerContext);
        }

        /// <summary>
        /// Gets container based on container shortname and organization name
        /// </summary>
        /// <param name="containerName"></param>
        /// <param name="organizationName"></param>
        /// <param name="containerContext">context indicates the properties like load attributes or not</param>        
        /// <param name="callerContext">context indicates the properties of the caller who called the API like program name,Module</param>
        /// <returns>Container</returns>
        public Container Get(String containerName, String organizationName, ContainerContext containerContext, CallerContext callerContext)
        {
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("ContainerBL.Get", MDMTraceSource.General, false);

            Container returnContainer = null;

            try
            {
                #region Validations

                String errorMessage = String.Empty;

                if (String.IsNullOrWhiteSpace(containerName))
                {
                    errorMessage = this.GetSystemLocaleMessage("111681", callerContext).Message;
                    throw new MDMOperationException("111681", errorMessage, "ContainerManager.ContainerBL", String.Empty, "Get");
                }

                if (String.IsNullOrWhiteSpace(organizationName))
                {
                    errorMessage = this.GetSystemLocaleMessage("111865", callerContext).Message;
                    throw new MDMOperationException("111865", errorMessage, "ContainerManager.ContainerBL", String.Empty, "Get");
                }

                if (callerContext == null)
                {
                    errorMessage = "CallerContext cannot be null.";
                    throw new MDMOperationException("111846", errorMessage, "ContainerManager.ContainerBL", String.Empty, "Get");
                }

                if (containerContext == null)
                {
                    errorMessage = "ContainerContext cannot be null.";
                    throw new MDMOperationException("112120", errorMessage, "ContainerManager.ContainerBL", String.Empty, "Get");
                }

                #endregion Validations

                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "container Name : " + organizationName);
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "organization Name : " + organizationName);
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "CallerContext : Application - " + callerContext.Application + " Module - " + callerContext.Module);
                }

                Collection<UserAction> containerObjectPermissionSet;
                Dictionary<Int32, Container> containers = GetAllContainers(containerContext, callerContext);

                if (containers != null && containers.Count > 0)
                {
                    Func<Container, Boolean> compareMethod;

                    if (String.IsNullOrWhiteSpace(organizationName))
                    {
                        compareMethod = (container => String.Compare(container.Name, containerName, StringComparison.InvariantCultureIgnoreCase) == 0);
                    }
                    else
                    {
                        compareMethod = container => ((String.Compare(container.Name, containerName, StringComparison.InvariantCultureIgnoreCase) == 0 &&
                                         String.Compare(container.OrganizationShortName, organizationName, StringComparison.InvariantCultureIgnoreCase) == 0) ||
                                         (String.Compare(container.Name, containerName, StringComparison.InvariantCultureIgnoreCase) == 0));
                    }

                    foreach (Container container in containers.Values)
                    {
                        if (compareMethod(container))
                        {
                            returnContainer = container;
                        }
                    }

                    if (returnContainer != null && containerContext.ApplySecurity)
                    {
                        returnContainer = ApplyContainerSecurity(returnContainer, containerContext, callerContext, out containerObjectPermissionSet);
                    }
                }
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("ContainerBL.Get", MDMTraceSource.General);
            }

            return returnContainer;
        }

        /// <summary>
        /// Gets all containers in a given organization id
        /// </summary>
        /// <param name="orgainzationId">Indicates orgainzation Id</param>
        /// <param name="containerContext">context indicates the properties like load attributes or not</param>        
        /// <param name="callerContext">context indicates the properties of the caller who called the API like program name,Module</param>
        /// <returns>Container Collection</returns>
        public ContainerCollection GetByOrgId(Int32 orgainzationId, ContainerContext containerContext, CallerContext callerContext)
        {
            ContainerCollection containerCollection = new ContainerCollection();

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("ContainerBL.GetByOrgId", MDMTraceSource.General, false);
            try
            {
                #region Validations

                String errorMessage = String.Empty;

                if (orgainzationId < 0)
                {
                    errorMessage = this.GetSystemLocaleMessage("111864", callerContext).Message;
                    throw new MDMOperationException("111864", errorMessage, "ContainerManager.ContainerBL", String.Empty, "GetByOrgId");
                }

                if (callerContext == null)
                {
                    errorMessage = "CallerContext cannot be null.";
                    throw new MDMOperationException("111846", errorMessage, "ContainerManager.ContainerBL", String.Empty, "GetByOrganizationId");
                }

                if (containerContext == null)
                {
                    errorMessage = "ContainerContext cannot be null.";
                    throw new MDMOperationException("112120", errorMessage, "ContainerManager.ContainerBL", String.Empty, "Get");
                }
                #endregion Validations

                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Orgainzation Id : " + orgainzationId.ToString());
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "CallerContext : Application - " + callerContext.Application + " Module - " + callerContext.Module);
                }

                Collection<UserAction> containerObjectPermissionSet;
                Dictionary<Int32, Container> allcontainers = GetAllContainers(containerContext, callerContext);

                Dictionary<Int32, Container> containers = null;

                if (containerContext.ApplySecurity)
                {
                    containers = ApplyContainerSecurity(allcontainers, containerContext, callerContext, out containerObjectPermissionSet);
                    containerCollection.PermissionSet = containerObjectPermissionSet;
                }
                else
                {
                    containers = allcontainers;
                }

                if (containers != null && containers.Count > 0)
                {
                    foreach (var currentContainer in containers.Values)
                    {
                        if (currentContainer.OrganizationId == orgainzationId)
                        {
                            containerCollection.Add(currentContainer);
                        }
                    }
                }
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("ContainerBL.GetByOrgId", MDMTraceSource.General);
            }

            return containerCollection;
        }

        /// <summary>
        /// Gets all containers in given organization
        /// </summary>
        /// <param name="organizationName">organization short Name</param>
        /// <param name="containerContext">context indicates the properties like load attributes or not</param>
        /// <param name="callerContext">context indicates the properties of the caller who called the API like program name,Module</param>
        /// <returns>Container Collection</returns>
        public ContainerCollection GetByOrgName(String organizationName, ContainerContext containerContext, CallerContext callerContext)
        {
            ContainerCollection containerCollection = new ContainerCollection();

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("ContainerBL.GetByOrgName", MDMTraceSource.General, false);

            try
            {
                #region Validations

                String errorMessage = String.Empty;

                if (String.IsNullOrWhiteSpace(organizationName))
                {
                    errorMessage = this.GetSystemLocaleMessage("111865", callerContext).Message;
                    throw new MDMOperationException("111865", errorMessage, "ContainerManager.ContainerBL", String.Empty, "GetByOrgName");
                }

                if (callerContext == null)
                {
                    errorMessage = "CallerContext cannot be null.";
                    throw new MDMOperationException("111846", errorMessage, "ContainerManager.ContainerBL", String.Empty, "GetByOrganizationName");
                }

                if (containerContext == null)
                {
                    errorMessage = "ContainerContext cannot be null.";
                    throw new MDMOperationException("112120", errorMessage, "ContainerManager.ContainerBL", String.Empty, "Get");
                }

                #endregion Validations

                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "organization Name : " + organizationName);
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "CallerContext : Application - " + callerContext.Application + " Module - " + callerContext.Module);
                }

                Collection<UserAction> containerObjectPermissionSet;
                Dictionary<Int32, Container> allcontainers = GetAllContainers(containerContext, callerContext);
                Dictionary<Int32, Container> containers = null;

                if (containerContext.ApplySecurity)
                {
                    containers = ApplyContainerSecurity(allcontainers, containerContext, callerContext, out containerObjectPermissionSet);
                    containerCollection.PermissionSet = containerObjectPermissionSet;
                }
                else
                {
                    containers = allcontainers;
                }

                if (containers != null && containers.Count > 0)
                {
                    foreach (var currentContainer in containers.Values)
                    {
                        if (currentContainer.OrganizationShortName.Equals(organizationName))
                        {
                            containerCollection.Add(currentContainer);
                        }
                    }
                }
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("ContainerBL.GetByOrgName", MDMTraceSource.General);
            }

            return containerCollection;
        }

        #endregion

        #region Get Child Methods

        /// <summary>
        /// Get hierarchy of child container based on given container identifier and container context
        /// </summary>
        /// <param name="containerId">Indicates container identifier</param>
        /// <param name="containerContext">Indicates context of the container specifying properties like load attributes</param>
        /// <param name="callerContext">Indicates caller of the API specifying application and module name.</param>
        /// <param name="loadRecursive">Indicates whether load only immediate child containers or complete hierarchy of child containers.</param>
        /// <returns>Returns collection of child container based on given container identifier and container context</returns>
        public ContainerCollection GetChildContainers(Int32 containerId, ContainerContext containerContext, CallerContext callerContext, Boolean loadRecursive)
        {
            ContainerCollection childContainers = new ContainerCollection();

            #region Step : Diagnostic Activity initialization

            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();

            String traceMessage = String.Empty;

            if (isTracingEnabled)
            {
                diagnosticActivity.Start();
                traceMessage = String.Format("Starting get child containers internal execution flow for container id:{0}, container context:{1}, Application:{2} and Service:{3}", containerId, containerContext.ToXml(), callerContext.Application, callerContext.Module);
                diagnosticActivity.LogInformation(traceMessage);
            }

            #endregion

            try
            {
                #region Validation

                String errorMessage = String.Empty;

                if (containerId < 0)
                {
                    errorMessage = this.GetSystemLocaleMessage("111821", callerContext).Message;
                    throw new MDMOperationException("111821", errorMessage, "ContainerManager.ContainerBL", String.Empty, "GetChildContainers");
                }

                if (isTracingEnabled)
                {
                    diagnosticActivity.LogInformation("Validation is completed.");
                }

                #endregion Validation

                ContainerCollection containerCollection = GetAll(containerContext, callerContext);

                childContainers = (ContainerCollection)containerCollection.GetChildContainers(containerId, loadRecursive);

                if (isTracingEnabled)
                {
                    diagnosticActivity.LogDurationInfo("Child containers get completed.");
                }

            }
            catch (Exception ex)
            {
                if (isTracingEnabled)
                {
                    diagnosticActivity.LogError(ex.Message);
                }

                throw new MDMOperationException(ex.Message, ex);
            }
            finally
            {
                #region Step: Final Diagnostics and tracing

                if (isTracingEnabled)
                {
                    diagnosticActivity.LogMessageWithData("See 'View Data' for loaded containers.", childContainers.ToXml());
                    diagnosticActivity.Stop();
                }

                #endregion
            }

            return childContainers;
        }

        /// <summary>
        /// Get hierarchy of child container with requested container identifier's container itself based on given container identifier and container context
        /// </summary>
        /// <param name="containerId">Indicates container identifier</param>
        /// <param name="containerContext">Indicates context of the container specifying properties like load attributes</param>
        /// <param name="callerContext">Indicates caller of the API specifying application and module name.</param>
        /// <returns>Returns collection of child containers with requested container identifier's container itself based on given container identifier and container context</returns>
        public ContainerCollection GetContainerHierarchy(Int32 containerId, ContainerContext containerContext, CallerContext callerContext)
        {
            ContainerCollection containerHierarchy = new ContainerCollection();

            #region Step : Diagnostic Activity initialization

            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();

            String traceMessage = String.Empty;

            if (_currentTraceSettings.IsBasicTracingEnabled)
            {
                diagnosticActivity.Start();
                traceMessage = String.Format("Starting get container hierarchy internal execution flow for container id:{0}, container context:{1}, Application:{2} and Service:{3}", containerId, containerContext.ToXml(), callerContext.Application, callerContext.Module);
                diagnosticActivity.LogInformation(traceMessage);
            }

            #endregion

            try
            {
                ContainerCollection containerCollection = GetAll(containerContext, callerContext);

                #region Validation

                String errorMessage = String.Empty;

                if (containerId < 0)
                {
                    errorMessage = this.GetSystemLocaleMessage("111821", callerContext).Message;
                    throw new MDMOperationException("111821", errorMessage, "ContainerManager.ContainerBL", String.Empty, "GetChildContainers");
                }

                if (callerContext == null)
                {
                    errorMessage = "CallerContext cannot be null.";
                    throw new MDMOperationException("111846", errorMessage, "ContainerManager.ContainerBL", String.Empty, "GetChildContainers");
                }

                if (containerContext == null)
                {
                    errorMessage = this.GetSystemLocaleMessage("112120", callerContext).Message;
                    throw new MDMOperationException("112120", errorMessage, "ContainerManager.ContainerBL", String.Empty, "GetChildContainers");
                }

                #endregion Validation

                containerHierarchy = (ContainerCollection)containerCollection.GetContainerHierarchy(containerId);

            }
            catch (Exception ex)
            {
                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.LogError(ex.Message);
                }

                throw new MDMOperationException(ex.Message, ex);
            }
            finally
            {
                #region Step: Final Diagnostics and tracing

                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.LogMessageWithData("See 'View Data' for loaded containers.", containerHierarchy.ToXml());
                    diagnosticActivity.Stop();
                }

                #endregion
            }

            return containerHierarchy;
        }

        #endregion Get Child Methods

        #endregion Get Methods

        #region CUD Methods

        /// <summary>
        /// creates the specified container
        /// </summary>
        /// <param name="container">container to be created</param>
        /// <param name="programName">name of the method which called this API</param>
        /// <param name="callerContext">caller context indicating who called the API</param>
        public OperationResult Create(Container container, String programName, CallerContext callerContext)
        {
            OperationResult containerOperationResult = new OperationResult();

            #region Validations

            if (callerContext == null)
            {
                throw new MDMOperationException("111846", "CallerContext cannot be null.", "ContainerManager.ContainerBL", String.Empty, "Create");
            }

            if (container == null)
            {
                throw new MDMOperationException("111867", "Container cannot be null.", "ContainerManager.ContainerBL", String.Empty, "Create");
            }

            #endregion Validations

            container.Action = ObjectAction.Create;

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "CallerContext : Application - " + callerContext.Application + " Module - " + callerContext.Module);

            ContainerCollection containerCollection = new ContainerCollection();
            containerCollection.Add(container);

            containerOperationResult = Create(containerCollection, containerOperationResult, programName, callerContext);

            return containerOperationResult;
        }

        /// <summary>
        /// creates the specified list of containers
        /// </summary>
        /// <param name="containerCollection">containers to be created</param>
        /// <param name="containerOperationResult">Indicates operation result for container</param>
        /// <param name="programName">name of the method which called this API</param>
        /// <param name="callerContext">caller context indicating who called the API</param>
        public OperationResult Create(ContainerCollection containerCollection, OperationResult containerOperationResult, String programName, CallerContext callerContext)
        {
            #region Validations

            if (callerContext == null)
            {
                throw new MDMOperationException("111846", "CallerContext cannot be null.", "ContainerManager.ContainerBL", String.Empty, "Update");
            }

            if (containerCollection == null)
            {
                throw new MDMOperationException("111866", "ContainerCollection cannot be null.", "ContainerManager.ContainerBL", String.Empty, "Update");
            }

            #endregion Validations

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "CallerContext : Application - " + callerContext.Application + " Module - " + callerContext.Module);

            Process(containerCollection, containerOperationResult, programName, callerContext);

            return containerOperationResult;
        }

        /// <summary>
        /// Updated the container
        /// </summary>
        /// <param name="container">container to be updated</param>
        /// <param name="programName">name of the method which called this API</param>
        /// <param name="callerContext">caller context indicating who called the API</param>
        public OperationResult Update(Container container, String programName, CallerContext callerContext)
        {
            #region Validations

            if (callerContext == null)
            {
                throw new MDMOperationException("111846", "CallerContext cannot be null.", "ContainerManager.ContainerBL", String.Empty, "Update");
            }

            if (container == null)
            {
                throw new MDMOperationException("111867", "Container cannot be null.", "ContainerManager.ContainerBL", String.Empty, "Update");
            }

            #endregion Validations

            //set action to update
            container.Action = ObjectAction.Update;

            OperationResult containerOperationResult = new OperationResult();

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "CallerContext : Application - " + callerContext.Application + " Module - " + callerContext.Module);
            ContainerCollection containerCollection = new ContainerCollection();
            containerCollection.Add(container);

            Update(containerCollection, containerOperationResult, programName, callerContext);

            return containerOperationResult;
        }

        /// <summary>
        /// Updates the specified list of containers
        /// </summary>
        /// <param name="containerCollection">containers to be updated</param>
        /// <param name="containerOperationResult">Indicates operation result for container</param>
        /// <param name="programName">name of the method which called this API</param>
        /// <param name="callerContext">caller context indicating who called the API</param>
        public OperationResult Update(ContainerCollection containerCollection, OperationResult containerOperationResult, String programName, CallerContext callerContext)
        {
            #region Validations

            if (callerContext == null)
            {
                throw new MDMOperationException("111846", "CallerContext cannot be null.", "ContainerManager.ContainerBL", String.Empty, "Update");
            }

            if (containerCollection == null)
            {
                throw new MDMOperationException("111866", "ContainerCollection cannot be null.", "ContainerManager.ContainerBL", String.Empty, "Update");
            }

            #endregion Validations

            Process(containerCollection, containerOperationResult, programName, callerContext);

            return containerOperationResult;
        }

        /// <summary>
        /// Deletes the specified container
        /// </summary>
        /// <param name="container">container to be deleted</param>
        /// <param name="programName">name of the method which called this API</param>
        /// <param name="callerContext">caller context indicating who called the API</param>
        public OperationResult Delete(Container container, String programName, CallerContext callerContext)
        {
            #region Validations

            if (callerContext == null)
            {
                throw new MDMOperationException("111846", "CallerContext cannot be null.", "ContainerManager.ContainerBL", String.Empty, "Delete");
            }

            if (container == null)
            {
                throw new MDMOperationException("111867", "Container cannot be null.", "ContainerManager.ContainerBL", String.Empty, "Delete");
            }

            #endregion Validations

            //set action to update
            container.Action = ObjectAction.Delete;

            OperationResult containerOperationResult = new OperationResult();

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "CallerContext : Application - " + callerContext.Application + " Module - " + callerContext.Module);

            ContainerCollection containerCollection = new ContainerCollection();
            containerCollection.Add(container);

            Delete(containerCollection, containerOperationResult, programName, callerContext);

            return containerOperationResult;
        }

        /// <summary>
        /// Deletes the specified list of containers
        /// </summary>
        /// <param name="containerCollection">containers to be deleted</param>
        /// <param name="containerOperationResult">Indicates operation result for container</param>
        /// <param name="programName">name of the method which called this API</param>
        /// <param name="callerContext">caller context indicating who called the API</param>
        public OperationResult Delete(ContainerCollection containerCollection, OperationResult containerOperationResult, String programName, CallerContext callerContext)
        {
            #region Validations

            if (callerContext == null)
            {
                throw new MDMOperationException("111846", "CallerContext cannot be null.", "ContainerManager.ContainerBL", String.Empty, "Delete");
            }

            if (containerCollection == null)
            {
                throw new MDMOperationException("111866", "ContainerCollection cannot be null.", "ContainerManager.ContainerBL", String.Empty, "Delete");
            }

            #endregion Validations

            Process(containerCollection, containerOperationResult, programName, callerContext);

            return containerOperationResult;
        }

        /// <summary>
        /// Processes the list of container(All CRUD operations goes through same process)
        /// </summary>
        /// <param name="container">Indicates container which needs to be processed</param>
        /// <param name="containerOperationResult">Indicates operation result for container</param>
        /// <param name="programName">name of the method which called this API</param>
        /// <param name="callerContext">caller context indicating who called the API</param>
        public void Process(Container container, OperationResult containerOperationResult, String programName, CallerContext callerContext)
        {
            #region Validations

            if (callerContext == null)
            {
                throw new MDMOperationException("111846", "CallerContext cannot be null.", "ContainerManager.ContainerBL", String.Empty, "Delete");
            }

            if (container == null)
            {
                throw new MDMOperationException("111867", "Container cannot be null.", "ContainerManager.ContainerBL", String.Empty, "Delete");
            }

            #endregion Validations

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "CallerContext : Application - " + callerContext.Application + " Module - " + callerContext.Module);

            ContainerCollection containerCollection = new ContainerCollection();
            containerCollection.Add(container);

            Process(containerCollection, containerOperationResult, programName, callerContext);
        }

        /// <summary>
        /// Processes the container collection (All CRUD operations goes through same process)
        /// </summary>
        /// <param name="containerCollection">list of containers to be processeds</param>
        /// <param name="command">contains all the db related properties such as connection string etc.</param>
        /// <param name="containerOperationResult">Indicates operation result for container</param>
        /// <param name="programName">Name of the method which called this API</param>
        /// <param name="callerContext">Caller context indicating who called the API</param>
        public void Process(ContainerCollection containerCollection, OperationResult containerOperationResult, String programName, CallerContext callerContext)
        {
            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();

            if (isTracingEnabled)
            {
                diagnosticActivity.Start();
                diagnosticActivity.LogMessageWithData("Starting to process container collection.", containerCollection != null ? containerCollection.ToXml() : "NULL", MessageClassEnum.Information);
            }
            try
            {
                Boolean emptyShortNameError = false;
                Int32 counter = -1;
                ContainerCollection originalContainers = null;

                ContainerCollection containers = new ContainerCollection(containerCollection.ToList());

                //Required for Locales Compare and Merge
                LoadOriginalContainers(containers, true, callerContext, out originalContainers);

                //loop through containers collection to remove containers with invalid data
                foreach (Container container in containers)
                {
                    container.ReferenceId = counter.ToString();
                    counter--;

                    if (container.Action == ObjectAction.Create || container.Action == ObjectAction.Update)
                    {
                        Container originalContainer = null;
                        Boolean propertyValidationPassed = true;

                        if (container.OriginalContainer != null)
                        {
                            originalContainer = container.OriginalContainer;
                        }
                        else
                        {
                            if (!String.IsNullOrEmpty(container.Name))
                            {
                                originalContainer = Get(container.Name, container.OrganizationId, callerContext, false);
                            }
                        }

                        if (container.AutoExtensionEnabled && originalContainers != null && originalContainers.Count > 0)
                        {
                            Container parentContainer = originalContainers.GetContainer(container.ParentContainerId);

                            if (parentContainer != null && !parentContainer.AutoExtensionEnabled)
                            {
                                propertyValidationPassed = false;
                                containerCollection.Remove(container);

                                Object[] param = new Object[] { container.Name, parentContainer.Name };

                                LocaleMessage localeMessage = new LocaleMessageBL().Get(_systemUILocale, "114589", param, false, callerContext); // Unable to process the container '{0}', as the auto-extension flag for this container is set as "true" and the auto-extension flag for its parent container '{1} ' is set as "false".
                                containerOperationResult.AddOperationResult(localeMessage.Code, localeMessage.Message, OperationResultType.Error);
                            }
                        }

                        #region Property Validations

                        // remove container if shortname is empty
                        if (String.IsNullOrEmpty(container.Name))
                        {
                            containerCollection.Remove(container);
                            propertyValidationPassed = false;
                            if (!emptyShortNameError)
                            {
                                LocaleMessage localeMessage = new LocaleMessageBL().Get(_systemUILocale, "112001", false, callerContext);
                                containerOperationResult.AddOperationResult(localeMessage.Code, localeMessage.Message, OperationResultType.Error);
                                emptyShortNameError = true;
                            }
                        }
                        // remove container if longname is empty
                        else if (String.IsNullOrEmpty(container.LongName))
                        {
                            containerCollection.Remove(container);
                            propertyValidationPassed = false;
                            Object[] param = new Object[] { container.Name };
                            LocaleMessage localeMessage = new LocaleMessageBL().Get(_systemUILocale, "112002", param, false, callerContext);
                            containerOperationResult.AddOperationResult(localeMessage.Code, localeMessage.Message, OperationResultType.Error);
                        }

                        // remove container if Org Id is empty
                        else if (container.OrganizationId <= 0)
                        {
                            containerCollection.Remove(container);
                            propertyValidationPassed = false;
                            Object[] param = new Object[] { container.Name };
                            LocaleMessage localeMessage = new LocaleMessageBL().Get(_systemUILocale, "112003", param, false, callerContext);
                            containerOperationResult.AddOperationResult(localeMessage.Code, localeMessage.Message, OperationResultType.Error);
                        }
                        // remove container if HierarchyId is empty
                        else if (container.HierarchyId <= 0)
                        {
                            containerCollection.Remove(container);
                            propertyValidationPassed = false;
                            Object[] param = new Object[] { container.Name };
                            LocaleMessage localeMessage = new LocaleMessageBL().Get(_systemUILocale, "112004", param, false, callerContext);
                            containerOperationResult.AddOperationResult(localeMessage.Code, localeMessage.Message, OperationResultType.Error);
                        }

                        // remove container if a container with same shortname already exists in the given org id
                        else if (container.Action == ObjectAction.Create && originalContainer != null)
                        {
                            Object[] parameters = new Object[] { container.Name };
                            LocaleMessage localeMessage = new LocaleMessageBL().Get(_systemUILocale, "112005", parameters, false, callerContext);
                            containerOperationResult.AddOperationResult(localeMessage.Code, localeMessage.Message, OperationResultType.Error);
                            containerCollection.Remove(container);
                            propertyValidationPassed = false;
                        }
                        else if (container.Action == ObjectAction.Update)
                        {
                            Container containerToUpdate = null;
                            containerToUpdate = this.GetById(container.Id);

                            //if container we are trying to update does not exist remove the container from collection.
                            if (containerToUpdate == null)
                            {
                                containerCollection.Remove(container);
                                propertyValidationPassed = false;
                                Object[] param = new Object[] { container.Id };
                                LocaleMessage localeMessage = new LocaleMessageBL().Get(_systemUILocale, "112008", param, false, callerContext);
                                containerOperationResult.AddOperationResult(localeMessage.Code, localeMessage.Message, OperationResultType.Error);
                            }
                            //if in container to be updated we are trying change the organization of container, we remove that container from container collection.
                            else if (containerToUpdate.OrganizationId != container.OrganizationId)
                            {
                                containerCollection.Remove(container);
                                propertyValidationPassed = false;
                                Object[] param = new Object[] { container.Name };
                                LocaleMessage localeMessage = new LocaleMessageBL().Get(_systemUILocale, "112011", param, false, callerContext);
                                containerOperationResult.AddOperationResult(localeMessage.Code, localeMessage.Message, OperationResultType.Error);
                            }
                            //updating a container, updating shortname to existing shortname in same org.
                            /*
                             Eg: Org- Riverworks
                             *   Catalog : PM
                             *   CatalogToUpdate: SM
                             *   If SM exists and we try to update shortname of SM to PM, it will create 2 PMs in Riverworks. 
                             *   Hence we remove that container from containercollection.
                             */
                            else if (originalContainer != null &&
                                (container.Name == originalContainer.Name
                                && container.Id != originalContainer.Id))
                            {
                                containerCollection.Remove(container);
                                propertyValidationPassed = false;
                                Object[] param = new Object[] { container.Name };
                                LocaleMessage localeMessage = new LocaleMessageBL().Get(_systemUILocale, "112014", param, false, callerContext);
                                containerOperationResult.AddOperationResult(localeMessage.Code, localeMessage.Message, OperationResultType.Error);
                            }
                        }

                        #endregion Property Validations

                        #region Locale Validations

                        if (propertyValidationPassed)
                        {
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
                                    }
                                }

                                if (duplicateLocales.Length > 0)
                                {
                                    containerCollection.Remove(container);

                                    Object[] parameters = new Object[] { container.Name, duplicateLocales.Substring(0, duplicateLocales.Length - 1) };

                                    LocaleMessage localeMessage = new LocaleMessageBL().Get(_systemUILocale, "114268", parameters, false, callerContext);
                                    containerOperationResult.AddOperationResult(localeMessage.Code, localeMessage.Message, OperationResultType.Error);
                                }
                            }
                            else if (container.SupportedLocales != null && container.SupportedLocales.Count == 1 && container.SupportedLocales.First().Locale == _systemDataLocale)
                            {
                                hasSystemDataLocale = true;
                            }

                            //Add SDL if no locale is found or SDL is missing.
                            if (!hasSystemDataLocale)
                            {
                                container.SupportedLocales.Add(new Locale()
                                {
                                    Id = (Int32)_systemDataLocale,
                                    Name = _systemDataLocale.ToString(),
                                    LongName = _systemDataLocale.GetDescription(),
                                    Locale = _systemDataLocale,
                                    Action = ObjectAction.Create
                                });
                            }

                            CompareAndMergeLocales(containers);
                        }

                        #endregion Locale Validations
                    }
                }

                #region Call DA

                if (containerCollection.Count > 0)
                {
                    if (isTracingEnabled)
                    {
                        diagnosticActivity.LogMessageWithData("Completed validation and starting to save the filtered container collection.", containerCollection.ToXml(), MessageClassEnum.Information);
                    }

                    try
                    {
                        using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required))
                        {
                            //Get Data locale 
                            LocaleEnum systemDataLocale = _systemDataLocale;

                            //Get current logon user
                            String loginUser = SecurityPrincipalHelper.GetCurrentSecurityPrincipal().CurrentUserName;

                            DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Update);

                            ContainerDA containerDA = new ContainerDA();
                            containerDA.Process(containerCollection, containerOperationResult, command, systemDataLocale, loginUser, programName);

                            if (containerOperationResult != null && containerOperationResult.OperationResultStatus == OperationResultStatusEnum.Successful)
                            {
                                containerDA.ProcessLocaleMappings(containerCollection, containerOperationResult, command, _systemDataLocale, loginUser, callerContext.ProgramName);
                            }

                            transactionScope.Complete();
                        }
                    }
                    catch (Exception exception)
                    {
                        throw new MDMOperationException(exception.Message, exception);
                    }
                    #region Invalidate Internal cache and notify job engine for cache change
                    if (isTracingEnabled)
                    {
                        diagnosticActivity.LogInformation("Starting to invalidate the containers cache.");
                    }

                    InvalidateCache(containerCollection, callerContext);

                    if (isTracingEnabled)
                    {
                        diagnosticActivity.LogInformation("Completed invalidating containers cache.");
                    }
                    #endregion
                }
                else
                {
                    containerOperationResult.OperationResultStatus = OperationResultStatusEnum.Failed;
                }

                #endregion Call DA

                #region activity log

                if ((containerOperationResult.OperationResultStatus == OperationResultStatusEnum.Successful || containerOperationResult.OperationResultStatus == OperationResultStatusEnum.None) && AppConfigurationHelper.GetAppConfig<Boolean>(Constants.ACTIVITYLOG_DATAMODEL_ENABLED_APPCONFIG_KEY, false))
                {
                    if (callerContext.ProgramName.IsNullOrEmpty())
                    {
                        callerContext.ProgramName = "ContainerBL";
                    }
                    LogDataModelChanges(containers, callerContext);
                }

                #endregion activity log

                //if there are any error codes coming in OR from db, populate error messages for them
                foreach (Error error in containerOperationResult.Errors)
                {
                    if (!String.IsNullOrEmpty(error.ErrorCode) && String.IsNullOrWhiteSpace(error.ErrorMessage))
                    {
                        Object[] param = error.Params != null ? error.Params.ToArray() : null;
                        LocaleMessage localeMessage = new LocaleMessageBL().Get(_systemUILocale, error.ErrorCode, param, false, callerContext);

                        if (localeMessage != null)
                        {
                            error.ErrorMessage = localeMessage.Message;
                        }
                    }
                }
            }
            finally
            {
                #region Step: Final Diagnostics and tracing

                if (isTracingEnabled)
                {
                    diagnosticActivity.LogMessageWithData("Completed the container collection process. See 'View Data' for operation result XML.", containerOperationResult.ToXml(), MessageClassEnum.Information);
                    diagnosticActivity.Stop();
                }

                #endregion
            }
        }

        /// <summary>
        /// Copies the Mappings from source container to the target Container
        /// </summary>
        /// <param name="sourceContainerId">source container Id</param>
        /// <param name="targetContainerId">target Container Id</param>
        /// <param name="containerTempleteCopyContext">Context specifies which all mappings needs to be copied from source to target</param>
        /// <param name="callerContext">context indicates the properties of the caller who called the API like program name,Module</param>
        /// <returns></returns>
        public OperationResult CopyMappings(Int32 sourceContainerId, Int32 targetContainerId, ContainerTemplateCopyContext containerTempleteCopyContext, CallerContext callerContext)
        {
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Container CopyMappings starting..");

            #region ParameterValidation

            if (callerContext == null)
            {
                throw new MDMOperationException("111846", "CallerContext cannot be null.", "ContainerManager.ContainerBL", String.Empty, "CopyMappings");
            }

            if (sourceContainerId < 0)
            {
                throw new MDMOperationException("111821", "ContainerId must be greater than 0.", "ContainerManager.ContainerBL", String.Empty, "CopyMappings");
            }

            if (targetContainerId < 0)
            {
                throw new MDMOperationException("111821", "ContainerId must be greater than 0.", "ContainerManager.ContainerBL", String.Empty, "CopyMappings");
            }

            if (containerTempleteCopyContext == null)
            {
                throw new MDMOperationException("111821", "ContainerTemplateCopyContext cannot be null.", "ContainerManager.ContainerBL", String.Empty, "CopyMappings");
            }

            #endregion ParameterValidation

            OperationResult operationResult = new OperationResult();

            //Get current logon user
            String loginUser = SecurityPrincipalHelper.GetCurrentSecurityPrincipal().CurrentUserName;

            DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Update);

            ContainerDA containerDA = new ContainerDA();
            operationResult = containerDA.CopyMappings(sourceContainerId, targetContainerId, containerTempleteCopyContext, command, loginUser, callerContext.ProgramName);

            //TODO: not sure whether invalidate cache is required. ContainerCache contains only the metadata: To be clarified
            new ContainerBufferManager().RemoveContainers();

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Container CopyMappings completed successfully.");

            return operationResult;
        }

        /// <summary>
        /// Process container locale data model collection
        /// </summary>
        /// <param name="iDataModelObjects">Collection of data model objects to be processed.</param>
        /// <param name="operationResults">Collection of DataModel OperationResult</param>
        /// <param name="iCallerContext">Context of caller making call to this API.</param>
        /// <returns>Result of operation</returns>
        public void ProcessLocaleMappings(IDataModelObjectCollection iDataModelObjects, DataModelOperationResultCollection operationResults, ICallerContext iCallerContext)
        {
            ContainerCollection containers = iDataModelObjects as ContainerCollection;
            CallerContext callerContext = iCallerContext as CallerContext;

            if (containers.Count > 0)
            {
                ContainerDA containerDA = new ContainerDA();

                using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required))
                {
                    //Get current logged in user
                    String loginUser = SecurityPrincipalHelper.GetCurrentSecurityPrincipal().CurrentUserName;
                    DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Update);

                    //Process only locales as container process has already happened.
                    containerDA.ProcessLocaleMappings(containers, operationResults, command, _systemDataLocale, loginUser, callerContext.ProgramName);

                    transactionScope.Complete();
                }

                LocalizeErrors(operationResults, callerContext);
            }

            #region activity log

            if ((operationResults.OperationResultStatus == OperationResultStatusEnum.Successful || operationResults.OperationResultStatus == OperationResultStatusEnum.None) && AppConfigurationHelper.GetAppConfig<Boolean>(Constants.ACTIVITYLOG_DATAMODEL_ENABLED_APPCONFIG_KEY, false))
            {
                if (callerContext.ProgramName.IsNullOrEmpty())
                {
                    callerContext.ProgramName = "ContainerBL";
                }
                LogDataModelChanges(containers, callerContext);
            }

            #endregion activity log
        }

        #endregion CUD Methods

        #region IDataModelManager Methods

        /// <summary>
        /// Generate and OperationResult Schema for given DataModelObjectCollection
        /// </summary>
        /// <param name="iDataModelObjects"></param>
        public void PrepareOperationResultsSchema(IDataModelObjectCollection iDataModelObjects, DataModelOperationResultCollection operationResultCollection)
        {
            ContainerCollection containers = iDataModelObjects as ContainerCollection;

            if (containers != null && containers.Count > 0)
            {
                if (operationResultCollection.Count > 0)
                {
                    operationResultCollection.Clear();
                }

                Int32 containerToBeCreated = -1;

                foreach (Container container in containers)
                {
                    DataModelOperationResult containerOperationResult = new DataModelOperationResult(container.Id, container.LongName, container.ExternalId, container.ReferenceId);

                    if (String.IsNullOrEmpty(containerOperationResult.ExternalId))
                    {
                        containerOperationResult.ExternalId = container.Name;
                    }

                    if (container.Id < 1)
                    {
                        container.Id = containerToBeCreated;
                        containerOperationResult.Id = containerToBeCreated;
                        containerToBeCreated--;
                    }

                    operationResultCollection.Add(containerOperationResult);
                }
            }
        }

        /// <summary>
        /// Validates DataModelObjects and populate OperationResults
        /// </summary>
        /// <param name="iDataModelObjects">Collection of data model objects to be filled up.</param>
        /// <param name="operationResults">Collection of DataModel OperationResult</param>
        /// <param name="iCallerContext">Context of caller making call to this API.</param>
        public void Validate(IDataModelObjectCollection iDataModelObjects, DataModelOperationResultCollection operationResults, ICallerContext iCallerContext)
        {
            ContainerCollection containers = iDataModelObjects as ContainerCollection;
            CallerContext callerContext = iCallerContext as CallerContext;

            ValidateInputParameters(containers, operationResults, callerContext);
            ValidateContainersProperties(containers, operationResults, callerContext);
        }

        /// <summary>
        /// Load original data model objects
        /// </summary>
        /// <param name="iDataModelObjects">Collection of data model objects to load original object.</param>
        /// <param name="operationResults">Collection of DataModel OperationResult</param>
        /// <param name="iCallerContext">Context of caller making call to this API.</param>
        public void LoadOriginal(IDataModelObjectCollection iDataModelObjects, DataModelOperationResultCollection operationResults, ICallerContext iCallerContext)
        {
            ContainerCollection containers = iDataModelObjects as ContainerCollection;

            if (containers != null)
            {
                ContainerCollection originalContainers = null;
                LoadOriginalContainers(containers, false, iCallerContext as CallerContext, out originalContainers);
            }
        }

        /// <summary>
        /// Fills missing information to data model objects.
        /// </summary>
        /// <param name="iDataModelObjects">Collection of data model objects to be filled up.</param>
        /// <param name="iCallerContext">Context of caller making call to this API.</param>
        public void FillDataModel(IDataModelObjectCollection iDataModelObjects, ICallerContext iCallerContext)
        {
            FillContainers(iDataModelObjects as ContainerCollection, iCallerContext as CallerContext);
        }

        /// <summary>
        /// Compare and merge data model object collection with current collection
        /// </summary>
        /// <param name="iDataModelObjects">Collection of data model objects to be compare and merge.</param>
        /// <param name="operationResults">Collection of DataModel OperationResult</param>
        /// <param name="iCallerContext">Context of caller making call to this API.</param>
        /// <returns>Returns merged data model objects.</returns>
        public void CompareAndMerge(IDataModelObjectCollection iDataModelObjects, DataModelOperationResultCollection operationResults, ICallerContext iCallerContext)
        {
            CompareAndMerge(iDataModelObjects as ContainerCollection, operationResults, iCallerContext as CallerContext);
        }

        /// <summary>
        /// Process data model collection
        /// </summary>
        /// <param name="iDataModelObjects">Collection of data model objects to be processed.</param>
        /// <param name="operationResults">Collection of DataModel OperationResult</param>
        /// <param name="iCallerContext">Context of caller making call to this API.</param>
        /// <returns>Result of operation</returns>
        public void Process(IDataModelObjectCollection iDataModelObjects, DataModelOperationResultCollection operationResults, ICallerContext iCallerContext)
        {
            ContainerCollection containers = iDataModelObjects as ContainerCollection;
            CallerContext callerContext = iCallerContext as CallerContext;

            if (containers.Count > 0)
            {
                ContainerDA containerDA = new ContainerDA();

                using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required))
                {
                    //Get current logged in user
                    String loginUser = SecurityPrincipalHelper.GetCurrentSecurityPrincipal().CurrentUserName;
                    DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Update);

                    containerDA.Process(containers, operationResults, command, _systemDataLocale, loginUser, callerContext.ProgramName);

                    ContainerCollection containersTobeProcessedForLocales = new ContainerCollection();

                    foreach (Container container in containers)
                    {
                        IOperationResult operationResult = operationResults.GetByReferenceId(container.ReferenceId);
                        if (operationResult != null && operationResult.OperationResultStatus == OperationResultStatusEnum.Successful)
                        {
                            containersTobeProcessedForLocales.Add(container);
                        }
                    }

                    containerDA.ProcessLocaleMappings(containersTobeProcessedForLocales, operationResults, command, _systemDataLocale, loginUser, callerContext.ProgramName);

                    transactionScope.Complete();
                }

                LocalizeErrors(operationResults, callerContext);
            }

            #region activity log

            if ((operationResults.OperationResultStatus == OperationResultStatusEnum.Successful || operationResults.OperationResultStatus == OperationResultStatusEnum.None) && AppConfigurationHelper.GetAppConfig<Boolean>(Constants.ACTIVITYLOG_DATAMODEL_ENABLED_APPCONFIG_KEY, false))
            {
                if (callerContext.ProgramName.IsNullOrEmpty())
                {
                    callerContext.ProgramName = "ContainerBL";
                }
                LogDataModelChanges(containers, callerContext);
            }

            #endregion activity log

        }

        /// <summary>
        /// Processes the Entity cache statuses for data model objects
        /// </summary>
        /// <param name="iDataModelObjects">Collection of data model objects to be process entity cache load.</param>
        /// <param name="operationResults">Collection of DataModel OperationResult</param>
        /// <param name="iCallerContext">Context of caller making call to this API.</param>
        public void InvalidateCache(IDataModelObjectCollection iDataModelObjects, DataModelOperationResultCollection operationResults, ICallerContext iCallerContext)
        {
            ContainerCollection containers = iDataModelObjects as ContainerCollection;
            InvalidateCache(containers, iCallerContext as CallerContext);
        }

        #endregion

        #endregion Public Methods

        #region Private Methods

        #region Validation Methods

        /// <summary>
        /// Validates user permission for given container based on user action
        /// </summary>
        /// <param name="container">Indicates container for which needs to validate user permisssion</param>
        /// <param name="userAction">Indicates user action</param>
        /// <param name="dataSecurityManager">Indicates data security manager for get permission</param>
        /// <returns>Returns true is user contains permission for container otherwise false</returns>
        private Boolean ValidateUserPermission(Container container, UserAction userAction, Int32 userId, DataSecurityBL dataSecurityManager)
        {
            Permission permission = null;
            Int32 objectTypeId = (Int32)ObjectType.Catalog;

            PermissionContext permissionContext = new PermissionContext(0, container.Id, 0, 0, 0, 0, 0, 0, userId, 0);
            permission = dataSecurityManager.GetMDMObjectPermission(container.Id, objectTypeId, ObjectType.Catalog.ToString(), permissionContext);

            if (permission != null)
                container.PermissionSet = permission.PermissionSet;

            return (permission == null ? false : permission.PermissionSet.Contains(userAction));
        }

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
                AddOperationResults(operationResults, "113590", String.Empty, null, OperationResultType.Error, TraceEventType.Error, callerContext);
            }

            if (callerContext == null)
            {
                AddOperationResults(operationResults, "111846", String.Empty, null, OperationResultType.Error, TraceEventType.Error, callerContext);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="containers"></param>
        /// <param name="operationResults"></param>
        /// <param name="callerContext"></param>
        private void ValidateContainersProperties(ContainerCollection containers, DataModelOperationResultCollection operationResults, CallerContext callerContext)
        {
            Collection<String> uniqueContainerNames = new Collection<String>();

            foreach (Container container in containers)
            {
                IDataModelOperationResult operationResult = operationResults.GetByReferenceId(container.ReferenceId);

                //If container Action is Read/Ignore/Delete then by pass validation process.
                if (container.Action == ObjectAction.Read || container.Action == ObjectAction.Ignore || container.Action == ObjectAction.Delete)
                    continue;

                #region Validate Organization Name

                if (String.IsNullOrWhiteSpace(container.OrganizationShortName))
                {
                    AddOperationResult(operationResult, "111865", String.Empty, null, OperationResultType.Error, TraceEventType.Error, callerContext);
                }

                #endregion

                #region Validate Hierarchy Name

                if (String.IsNullOrWhiteSpace(container.HierarchyShortName))
                {
                    AddOperationResult(operationResult, "113603", String.Empty, new Object[] { container.OrganizationShortName }, OperationResultType.Error, TraceEventType.Error, callerContext);
                }

                #endregion

                #region Validate Short Name

                if (String.IsNullOrWhiteSpace(container.Name))
                {
                    AddOperationResult(operationResult, "111681", String.Empty, null, OperationResultType.Error, TraceEventType.Error, callerContext);
                }
                else
                {
                    if (!String.IsNullOrWhiteSpace(container.OrganizationShortName))
                    {
                        if (uniqueContainerNames.Contains(container.Name))
                        {
                            AddOperationResult(operationResult, "112014", String.Empty, new Object[] { container.Name }, OperationResultType.Error, TraceEventType.Error, callerContext);
                        }
                        else
                        {
                            uniqueContainerNames.Add(container.Name);
                        }
                    }
                }

                #endregion

                #region Validate Long Name

                if (String.IsNullOrWhiteSpace(container.LongName))
                {
                    AddOperationResult(operationResult, "112002", String.Empty, new Object[] { container.Name }, OperationResultType.Error, TraceEventType.Error, callerContext);
                }

                #endregion

                #region Validate Container Type

                if (container.ContainerType == ContainerType.Unknown)
                {
                    AddOperationResult(operationResult, "114467", String.Empty, new Object[] { container.Name }, OperationResultType.Error, TraceEventType.Error, callerContext);
                }

                #endregion Validate Container Type

                #region Validate Workflow Type

                if (container.WorkflowType == WorkflowType.Unknown)
                {
                    AddOperationResult(operationResult, "114466", String.Empty, new Object[] { container.Name }, OperationResultType.Error, TraceEventType.Error, callerContext);
                }

                #endregion Validate Workflow Type

            }
        }

        #endregion

        #region Utility Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        private Collection<Container> Convert(String xml)
        {
            Collection<Container> containers = new Collection<Container>();

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);

            XmlNode root = xmlDoc.FirstChild;

            foreach (XmlNode node in root.ChildNodes)
            {
                if (node.Name.Equals("Catalog"))
                {
                    // get all properties for container, from datatable rows
                    int containerId = -1;
                    if (node.Attributes["PK_Catalog"] != null)
                        Int32.TryParse(node.Attributes["PK_Catalog"].Value, out containerId);

                    if (containerId < 1)
                        continue;

                    string shortName = string.Empty;
                    if (node.Attributes["ShortName"] != null)
                        shortName = node.Attributes["ShortName"].Value;

                    string longName = string.Empty;
                    if (node.Attributes["LongName"] != null)
                        longName = node.Attributes["LongName"].Value;

                    int taxonomyId = -1;
                    if (node.Attributes["FK_Taxonomy"] != null)
                        Int32.TryParse(node.Attributes["FK_Taxonomy"].Value, out taxonomyId);

                    int orgId = -1;
                    if (node.Attributes["FK_Org"] != null)
                        Int32.TryParse(node.Attributes["FK_Org"].Value, out orgId);

                    String orgName = String.Empty;
                    if (node.Attributes["OrgName"] != null)
                        orgName = node.Attributes["OrgName"].Value;

                    String orgLongName = String.Empty;
                    if (node.Attributes["OrgLongName"] != null)
                        orgLongName = node.Attributes["OrgLongName"].Value;

                    var container = new Container(containerId);
                    container.Name = shortName;
                    container.LongName = longName;
                    container.HierarchyId = taxonomyId;
                    container.OrganizationId = orgId;
                    container.OrganizationShortName = orgName;
                    container.OrganizationLongName = orgLongName;

                    containers.Add(container);
                }
            }

            return containers;
        }

        /// <summary>
        /// Returns Message Object based on message code
        /// </summary>
        /// <param name="messageCode">Message Code</param>
        /// <param name="callerContext">caller context indicating who called the API</param>
        /// <returns></returns>
        private LocaleMessage GetSystemLocaleMessage(String messageCode, CallerContext callerContext)
        {
            LocaleMessageBL localeMessageBL = new LocaleMessageBL();

            return localeMessageBL.Get(_systemUILocale, messageCode, false, callerContext);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="containers"></param>
        /// <param name="callerContext"></param>
        private void InvalidateCache(ContainerCollection containers, CallerContext callerContext)
        {
            // Update activity log
            ProcessEntityCacheLoadForContainers(containers, callerContext);

            LocaleBufferManager localeBufferManager = new LocaleBufferManager();
            localeBufferManager.RemoveAvailableLocales(true);

            ContainerBufferManager containerBufferManager = new ContainerBufferManager();
            containerBufferManager.RemoveContainers();
        }

        /// <summary>
        /// if there are any error codes coming in OR from db, populate error messages for them
        /// </summary>
        /// <param name="operationResults">DataModelOperationResultCollection</param>
        /// <param name="callerContext">CallerContext</param>
        private void LocalizeErrors(DataModelOperationResultCollection operationResults, CallerContext callerContext)
        {
            foreach (DataModelOperationResult operationResult in operationResults)
            {
                foreach (Error error in operationResult.Errors)
                {
                    if (!String.IsNullOrEmpty(error.ErrorCode) && String.IsNullOrEmpty(error.ErrorMessage))
                    {
                        _localeMessage = _localeMessageBL.Get(_systemUILocale, error.ErrorCode, false, callerContext);

                        if (_localeMessage != null)
                        {
                            error.ErrorMessage = _localeMessage.Message;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="organizationName"></param>
        /// <returns></returns>
        private Organization GetOrganizationByName(String organizationName, CallerContext callerContext)
        {
            return new OrganizationBL().GetByName(organizationName, new OrganizationContext(), callerContext);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hierarchyName"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        private Hierarchy GetHierarchyByName(String hierarchyName, CallerContext callerContext)
        {
            return new HierarchyBL().GetByName(hierarchyName, callerContext);
        }

        /// <summary>
        /// 
        /// </summary>
        private void AddOperationResults(DataModelOperationResultCollection operationResults, String messageCode, String message, Object[] parameters, OperationResultType operationResultType, TraceEventType traceEventType, CallerContext callerContext)
        {
            if (parameters != null && parameters.Count() > 0)
            {
                _localeMessage = _localeMessageBL.TryGet(_systemUILocale, messageCode, message, parameters, false, callerContext);
            }
            else
            {
                _localeMessage = _localeMessageBL.TryGet(_systemUILocale, messageCode, message, false, callerContext);
            }

            if (_localeMessage != null)
            {
                operationResults.AddOperationResult(_localeMessage.Code, _localeMessage.Message, operationResultType);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void AddOperationResult(IDataModelOperationResult operationResult, String messageCode, String message, Object[] parameters, OperationResultType operationResultType, TraceEventType traceEventType, CallerContext callerContext)
        {
            if (parameters != null && parameters.Count() > 0)
            {
                _localeMessage = _localeMessageBL.TryGet(_systemUILocale, messageCode, message, parameters, false, callerContext);
            }
            else
            {
                _localeMessage = _localeMessageBL.TryGet(_systemUILocale, messageCode, message, false, callerContext);
            }

            if (_localeMessage != null)
            {
                operationResult.AddOperationResult(_localeMessage.Code, _localeMessage.Message, operationResultType);
            }
        }

        #endregion

        #region Get Methods

        /// <summary>
        /// Gets all containers
        /// </summary>
        /// <param name="containerContext"></param>
        /// <param name="callerContext">caller context indicating who called the API</param>
        /// <returns>Container Collection</returns>
        private Dictionary<Int32, Container> GetAllContainers(ContainerContext containerContext, CallerContext callerContext)
        {
            Dictionary<Int32, Container> containers;
            Dictionary<Int32, Container> permittedContainers = new Dictionary<Int32, Container>();

            #region Step : Diagnostic Activity initialization

            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();

            String traceMessage = String.Empty;

            if (isTracingEnabled)
            {
                diagnosticActivity.Start();
                traceMessage = String.Format("Starting get all containers internal execution flow for container context:{0}, caller context:{1} and with sending out containerObejctPermissonSet ", containerContext.ToXml(), callerContext.ToXml());
                diagnosticActivity.LogInformation(traceMessage);
            }

            #endregion

            try
            {

                var containerBufferManager = new ContainerBufferManager();
                containers = !containerContext.LoadAttributes ? containerBufferManager.FindAllContainers() : containerBufferManager.FindAllContainersWithAttributes();

                if (isTracingEnabled)
                {
                    diagnosticActivity.LogDurationInfo(String.Format("Loaded {0} containers from cache.", containers != null ? containers.Count() : 0));
                }

                if (containers == null || containers.Count < 1)
                {
                    //Get command
                    DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Read);

                    var containerDA = new ContainerDA();
                    containers = containerDA.GetAll(containerContext, command);

                    if (isTracingEnabled)
                    {
                        diagnosticActivity.LogDurationInfo(String.Format("Loaded {0} containers from database.", containers != null ? containers.Count() : 0));
                    }

                    if (containerContext.LoadAttributes)
                    {
                        LoadContainerAttributes(containers, callerContext);
                    }

                    if (containers != null && containers.Count > 0)
                    {
                        if (Constants.TRACING_ENABLED)
                            MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Caching {0} containers.", containers.Count), MDMTraceSource.DataModel);

                        containerBufferManager.UpdateContainers(containers, containerContext, 3);

                        if (isTracingEnabled)
                        {
                            diagnosticActivity.LogDurationInfo("Containers cache is updated.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (isTracingEnabled)
                {
                    diagnosticActivity.LogError(ex.Message);
                    diagnosticActivity.Stop();
                }

                throw new MDMOperationException("110817", "Failed to Load Data.", "ContainerManager.ContainerBL", String.Empty, "ContainerBL.GetAllContainers");
            }

            finally
            {
                if (isTracingEnabled)
                {
                    diagnosticActivity.Stop();
                }
            }
            return containers;
        }

        /// <summary>
        /// Apply security for a given container.
        /// </summary>
        /// <param name="container"></param>
        /// <param name="containerContext"></param>
        /// <param name="callerContext"></param>
        /// <param name="containerObjectPermissionSet"></param>
        /// <returns></returns>
        private Container ApplyContainerSecurity(Container container, ContainerContext containerContext, CallerContext callerContext, out Collection<UserAction> containerObjectPermissionSet)
        {
            Dictionary<Int32, Container> containers = new Dictionary<Int32, Container>();

            if (container != null)
            {
                containers.Add(container.Id, container);
            }

            Dictionary<Int32, Container> permittedContainers = ApplyContainerSecurity(containers, containerContext, callerContext, out containerObjectPermissionSet);

            // The returned object has to have only 1
            if (permittedContainers != null && permittedContainers.Count == 1)
            {
                return permittedContainers[container.Id];
            }

            return null;
        }

        /// <summary>
        /// Apply security for a given set of containers.
        /// </summary>
        /// <param name="containers"></param>
        /// <param name="containerContext"></param>
        /// <param name="callerContext"></param>
        /// <param name="containerObjectPermissionSet"></param>
        /// <returns></returns>
        private Dictionary<Int32, Container> ApplyContainerSecurity(Dictionary<Int32, Container> containers, ContainerContext containerContext, CallerContext callerContext, out Collection<UserAction> containerObjectPermissionSet)
        {
            Dictionary<Int32, Container> permittedContainers = new Dictionary<Int32, Container>();

            #region Step : Diagnostic Activity initialization

            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();

            String traceMessage = String.Empty;

            if (isTracingEnabled)
            {
                diagnosticActivity.Start();
                traceMessage = String.Format("Starting apply security for container context:{0}, caller context:{1} and with sending out containerObejctPermissonSet ", containerContext.ToXml(), callerContext.ToXml());
                diagnosticActivity.LogInformation(traceMessage);
            }
            #endregion

            try
            {
                containerObjectPermissionSet = new Collection<UserAction> { UserAction.Add, UserAction.Update, UserAction.Delete, UserAction.View };

                if (containers != null && containers.Count > 0)
                {
                    Int32 loginId = SecurityPrincipalHelper.GetCurrentSecurityPrincipal().CurrentUserId;
                    var dataSecurityManager = new DataSecurityBL();

                    foreach (Container container in containers.Values)
                    {
                        if ((!containerContext.IncludeApproved && container.IsApproved) ||
                            (containerContext.ApplySecurity && !ValidateUserPermission(container, UserAction.View, loginId, dataSecurityManager)))
                        {
                            continue;
                        }

                        permittedContainers.Add(container.Id, container);
                    }

                    if (containerContext.ApplySecurity)
                    {
                        if (isTracingEnabled)
                        {
                            diagnosticActivity.LogDurationInfo(String.Format("Filtered out {0} containers based on user permissions", permittedContainers != null ? permittedContainers.Count() : 0));
                        }

                        if (permittedContainers.Count == 0)
                        {
                            throw new MDMOperationException("112452", "You do not have sufficient permission to view Containers", "ContainerManager.ContainerBL", String.Empty, "ContainerBL.GetAllContainers");
                        }

                        //Set container object level permission at collection level
                        const int objectTypeId = (Int32)ObjectType.Catalog;
                        var permissionContext = new PermissionContext(0, 0, 0, 0, 0, 0, 0, 0, loginId, 0);
                        var permission = dataSecurityManager.GetMDMObjectPermission(0, objectTypeId, ObjectType.Catalog.ToString(), permissionContext);

                        if (permission != null)
                        {
                            containerObjectPermissionSet = permission.PermissionSet;
                        }
                    }
                }

                if (isTracingEnabled)
                {
                    diagnosticActivity.LogInformation("Loaded all containers based on user permissions.");
                }
            }

            catch (Exception ex)
            {
                if (isTracingEnabled)
                {
                    diagnosticActivity.LogError(ex.Message);
                    diagnosticActivity.Stop();
                }

                throw new MDMOperationException("110817", "Failed to Load Data.", "ContainerManager.ContainerBL", String.Empty, "ContainerBL.GetAllContainers");
            }

            finally
            {
                if (isTracingEnabled)
                {
                    diagnosticActivity.Stop();
                }
            }

            return permittedContainers;
        }

        /// <summary>
        /// Get dummy attributes in a container replaced with objects attributes filled with complete details 
        /// </summary>
        /// <param name="containers"></param>
        /// <param name="callerContext"></param>
        private void LoadContainerAttributes(Dictionary<Int32, Container> containers, CallerContext callerContext)
        {
            if (containers != null)
            {
                AttributeModelCollection attributeModelCollection = new AttributeModelCollection();
                Collection<Int32> attributeIds = new Collection<Int32>();

                AttributeModelBL attributeModelManager = new AttributeModelBL();
                LookupBL lookupManager = new LookupBL();

                //Get unique list of container atributes
                foreach (Container container in containers.Values)
                {
                    if (container.Attributes != null && container.Attributes.Count > 0)
                    {
                        foreach (Attribute attribute in container.Attributes)
                        {
                            if (!attributeIds.Contains(attribute.Id))
                            {
                                attributeIds.Add(attribute.Id);
                            }
                        }
                    }
                }

                //get attribute models
                AttributeModelContext attributeModelContext = new AttributeModelContext();
                attributeModelContext.GetCompleteDetailsOfAttribute = true;
                attributeModelContext.Locales.Add(_systemDataLocale);
                attributeModelContext.AttributeModelType = AttributeModelType.System;

                if (attributeIds != null && attributeIds.Count > 0)
                {
                    AttributeModelCollection attributeModels = attributeModelManager.GetByIds(attributeIds, attributeModelContext);

                    foreach (Container container in containers.Values)
                    {
                        if (container.Attributes != null && container.Attributes.Count > 0)
                        {
                            AttributeCollection rawContainerAttributeCollection = new AttributeCollection(container.Attributes.ToXml());
                            Dictionary<Int32, Int64> attributeValueRefIdPair = new Dictionary<int, long>();
                            ApplicationContext applicationContext = new ApplicationContext();
                            PopulateApplicationContext(container, applicationContext, callerContext);

                            container.Attributes.Clear();

                            foreach (Attribute attribute in rawContainerAttributeCollection)
                            {
                                IAttributeModel iContainerAttributeModel = attributeModels.GetAttributeModel(attribute.Id, attribute.Locale);

                                if (iContainerAttributeModel != null)
                                {
                                    Attribute containerAttribute = new Attribute((AttributeModel)iContainerAttributeModel);

                                    containerAttribute.SourceFlag = attribute.SourceFlag;
                                    containerAttribute.AttributeModelType = attribute.AttributeModelType;
                                    containerAttribute.Locale = attribute.Locale;
                                    containerAttribute.Action = ObjectAction.Read;

                                    IValueCollection values = attribute.GetCurrentValuesInvariant();

                                    if (values != null)
                                    {
                                        containerAttribute.SetValueInvariant(values);
                                    }

                                    if (iContainerAttributeModel.IsLookup)
                                    {
                                        PopulateLookupValues(containerAttribute, (AttributeModel)iContainerAttributeModel, applicationContext, callerContext);
                                    }

                                    container.Attributes.Add(containerAttribute);

                                }
                            }

                            #region Get lookpdisplay value in case of lookup attribute

                            //if (attributeValueRefIdPair.Count > 0)
                            //{
                            //    String displayValue = String.Empty;
                            //    Dictionary<Int32, String> attributeDisplayValuePair = new Dictionary<int, string>();
                            //    Value lookupDisplayValue = new Value();
                            //    attributeDisplayValuePair = lookupManager.GetLookupAttributeDisplayValue(attributeValueRefIdPair, callerContext);

                            //    foreach (KeyValuePair<Int32, Int64> pair in attributeValueRefIdPair)
                            //    {
                            //         attributeDisplayValuePair.TryGetValue(pair.Key, out displayValue);
                            //         lookupDisplayValue.AttrVal = displayValue;
                            //         container.Attributes[pair.Key, MDM.Utility.GlobalizationHelper.GetSystemDataLocale()].SetValueInvariant(lookupDisplayValue);
                            //    }
                            //}

                            #endregion get lookpdisplay value in case of lookup attribute

                        }
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attribute"></param>
        /// <param name="attributeModel"></param>
        /// <param name="applicationContext"></param>
        /// <param name="callerContext"></param>
        private void PopulateLookupValues(Attribute attribute, AttributeModel attributeModel, ApplicationContext applicationContext, CallerContext callerContext)
        {
            LookupBL lookupManager = new LookupBL();

            ValueCollection values = (ValueCollection)attribute.GetCurrentValuesInvariant();

            if (values != null && values.Count > 0)
            {
                //Create list of lookup PK list to fetch
                Collection<Int32> lookupValueRefIdList = new Collection<Int32>();
                foreach (Value value in values)
                {
                    if (value.AttrVal != null)
                    {
                        lookupValueRefIdList.Add(ValueTypeHelper.Int32TryParse(value.AttrVal.ToString(), -1));
                    }
                }

                if (lookupValueRefIdList.Count > 0)
                {
                    //Get lookup table with provided list of PK..
                    Lookup lookup = lookupManager.Get(attribute.Id, attribute.Locale, -1, lookupValueRefIdList, applicationContext, callerContext, false);

                    if (lookup != null && lookup.Rows.Count > 0)
                    {
                        foreach (Value value in values)
                        {
                            if (value.AttrVal != null)
                            {
                                value.ValueRefId = ValueTypeHelper.Int32TryParse(value.AttrVal.ToString(), -1);
                            }

                            if (value.ValueRefId < 1)
                                continue;

                            Row lookupRow = (Row)lookup.GetRecordById(value.ValueRefId);

                            if (lookupRow != null)
                            {
                                Object displayValObj = lookupRow.GetValue(Lookup.DisplayFormatColumnName);

                                if (displayValObj != null)
                                {
                                    string displayVal = displayValObj.ToString();

                                    if (!String.IsNullOrWhiteSpace(displayVal))
                                        value.SetDisplayValue(displayVal);
                                }

                                Object exportValObj = lookupRow.GetValue(Lookup.ExportFormatColumnName);

                                if (exportValObj != null)
                                {
                                    string exportVal = exportValObj.ToString();

                                    if (!String.IsNullOrWhiteSpace(exportVal))
                                        value.SetExportValue(exportVal);
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="container"></param>
        /// <param name="applicationContext"></param>
        /// <param name="callerContext">Indicates caller context</param>
        private void PopulateApplicationContext(Container container, ApplicationContext applicationContext, CallerContext callerContext)
        {
            applicationContext.OrganizationId = container.OrganizationId;
            applicationContext.OrganizationName = container.OrganizationShortName;
            applicationContext.ContainerId = container.Id;
            applicationContext.ContainerName = container.Name;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="containers"></param>
        /// <param name="loadByIds"></param>
        /// <param name="callerContext"></param>
        /// <param name="originalContainers"></param>
        private void LoadOriginalContainers(ContainerCollection containers, Boolean loadByIds, CallerContext callerContext, out ContainerCollection originalContainers)
        {
            ContainerCollection allOriginalContainers = GetAll(callerContext);
            originalContainers = allOriginalContainers;

            if (allOriginalContainers != null && allOriginalContainers.Count > 0)
            {
                if (loadByIds)
                {
                    foreach (Container container in containers)
                    {
                        container.OriginalContainer = allOriginalContainers.GetContainer(container.Id);
                    }
                }
                else
                {
                    foreach (Container container in containers)
                    {
                        //As the container name is unique across organization so get the first container with the requested container name.
                        container.OriginalContainer = allOriginalContainers.GetContainer(container.Name, container.OrganizationShortName);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="containers"></param>
        /// <param name="callerContext"></param>
        private void FillContainers(ContainerCollection containers, CallerContext callerContext)
        {
            String hierarchyShortName = String.Empty;
            String parentContainerName = String.Empty;

            foreach (Container container in containers)
            {
                hierarchyShortName = container.HierarchyShortName;
                parentContainerName = container.ParentContainerName;

                if (container.Id < 1)
                {
                    container.Id = (container.OriginalContainer != null) ? container.OriginalContainer.Id : container.Id;
                }

                if (container.OrganizationId < 1)
                {
                    if (container.OriginalContainer != null)
                    {
                        container.OrganizationId = container.OriginalContainer.OrganizationId;
                    }
                    else
                    {
                        Organization organization = GetOrganizationByName(container.OrganizationShortName, callerContext);

                        if (organization != null)
                        {
                            container.OrganizationId = organization.Id;
                            container.OrganizationShortName = organization.Name;
                            container.OrganizationLongName = organization.LongName;
                        }
                    }
                }

                if (container.HierarchyId < 1 && !String.IsNullOrWhiteSpace(hierarchyShortName))
                {
                    if (container.OriginalContainer != null && String.Compare(container.OriginalContainer.HierarchyShortName, hierarchyShortName) == 0)
                    {
                        container.HierarchyId = container.OriginalContainer.HierarchyId;
                    }
                    else
                    {
                        Hierarchy hierarchy = GetHierarchyByName(hierarchyShortName, callerContext);

                        if (hierarchy != null)
                        {
                            container.HierarchyId = hierarchy.Id;
                            container.HierarchyShortName = hierarchy.Name;
                            container.HierarchyLongName = hierarchy.LongName;
                        }
                    }
                }

                if (container.ParentContainerId < 1 && !String.IsNullOrWhiteSpace(parentContainerName))
                {
                    if (container.OriginalContainer != null && String.Compare(container.OriginalContainer.ParentContainerName, parentContainerName) == 0)
                    {
                        container.ParentContainerId = container.OriginalContainer.ParentContainerId;
                    }
                    else
                    {
                        Container parentContainer = Get(parentContainerName, callerContext);

                        if (parentContainer != null)
                        {
                            container.ParentContainerId = parentContainer.Id;
                            container.ParentContainerName = parentContainer.Name;
                        }
                    }
                }
            }
        }

        #endregion

        #region Process Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="containers"></param>
        /// <param name="operationResults"></param>
        /// <param name="callerContext"></param>
        private void CompareAndMerge(ContainerCollection containers, DataModelOperationResultCollection operationResults, CallerContext callerContext)
        {
            ContainerCollection availableContainers = new ContainerBL().GetAll(callerContext);

            foreach (Container deltaContainer in containers)
            {
                IDataModelOperationResult operationResult = operationResults.GetByReferenceId(deltaContainer.ReferenceId);

                //If delta object Action is Read/Ignore then skip Merge Delta Process.
                if (deltaContainer.Action == ObjectAction.Read || deltaContainer.Action == ObjectAction.Ignore)
                    continue;

                Container origContainer = deltaContainer.OriginalContainer;

                if (origContainer != null)
                {
                    //If delta object Action is Delete then skip Merge Delta process.
                    if (deltaContainer.Action != ObjectAction.Delete)
                    {
                        //Locales is through ContainerLocaleMappingBL.cs
                        deltaContainer.SupportedLocales = origContainer.SupportedLocales;

                        origContainer.MergeDelta(deltaContainer, callerContext, false);

                        //If container to be updated we are trying change the organization of container, populate operation result as validation.
                        if (deltaContainer.Action == ObjectAction.Update)
                        {
                            if (deltaContainer.ContainerType != origContainer.ContainerType)
                            {
                                //If we tries to change the container type of existing container, then error message should be thrown.
                                //Message: Unable to process the Container ‘{0}’, as the container type cannot be modified.

                                AddOperationResult(operationResult, "114435", String.Empty, new Object[] { deltaContainer.Name }, OperationResultType.Error, TraceEventType.Error, callerContext);
                            }

                            if (deltaContainer.OrganizationId != origContainer.OrganizationId)
                            {
                                AddOperationResult(operationResult, "112011", String.Empty, new Object[] { deltaContainer.Name }, OperationResultType.Error, TraceEventType.Error, callerContext);
                            }

                            if (deltaContainer.ParentContainerId != origContainer.ParentContainerId)
                            {
                                //If we tries to change the parent container name of existing container, then error message should be thrown.
                                //Message: Unable to process the Container ‘{0}’, as the parent container name cannot be modified.
                                AddOperationResult(operationResult, "114423", String.Empty, new Object[] { deltaContainer.Name }, OperationResultType.Error, TraceEventType.Error, callerContext);
                            }
                        }
                    }
                }
                else
                {
                    if (deltaContainer.Action == ObjectAction.Delete)
                    {
                        AddOperationResult(operationResult, "113599", String.Empty, new Object[] { deltaContainer.Name, deltaContainer.OrganizationShortName }, OperationResultType.Error, TraceEventType.Error, callerContext);
                    }
                    else
                    {

                        if (deltaContainer.OrganizationId < 1)
                        {
                            AddOperationResult(operationResult, "111947", String.Empty, new Object[] { "", "", deltaContainer.OrganizationShortName }, OperationResultType.Error, TraceEventType.Error, callerContext);
                        }
                        if (deltaContainer.HierarchyId < 1)
                        {
                            AddOperationResult(operationResult, "113675", "Hierarchy: {0} is invalid.", new Object[] { deltaContainer.HierarchyShortName }, OperationResultType.Error, TraceEventType.Error, callerContext);
                        }

                        #region Validate Child/Parent Link

                        if (deltaContainer.ParentContainerId > 0)
                        {
                            if (deltaContainer.ContainerType == ContainerType.Upstream && !String.IsNullOrWhiteSpace(deltaContainer.ParentContainerName))
                            {
                                AddOperationResult(operationResult, "114263", String.Empty, new Object[] { deltaContainer.Name }, OperationResultType.Error, TraceEventType.Error, callerContext);
                            }
                            else
                            {
                                Container parentContainer = availableContainers.GetContainer(deltaContainer.ParentContainerId);

                                if (parentContainer != null)
                                {
                                    if (deltaContainer.ContainerType == ContainerType.MasterCollaboration && parentContainer.ContainerType != ContainerType.Upstream)
                                    {
                                        AddOperationResult(operationResult, "114264", String.Empty, new Object[] { deltaContainer.Name }, OperationResultType.Error, TraceEventType.Error, callerContext);
                                    }

                                    if (deltaContainer.ContainerType == ContainerType.ExtensionCollaboration && parentContainer.ContainerType == ContainerType.Upstream)
                                    {
                                        AddOperationResult(operationResult, "114265", String.Empty, new Object[] { deltaContainer.Name, parentContainer.Name }, OperationResultType.Error, TraceEventType.Error, callerContext);
                                    }

                                    if (deltaContainer.AutoExtensionEnabled && !parentContainer.AutoExtensionEnabled)
                                    {
                                        AddOperationResult(operationResult, "114589", String.Empty, new Object[] { deltaContainer.Name, parentContainer.Name }, OperationResultType.Error, TraceEventType.Error, callerContext);//Unable to process the container '{0}', as the auto-extension flag for this container is set as "true" and the auto-extension flag for its parent container '{1} ' is set as "false".
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (deltaContainer.ContainerType == ContainerType.ExtensionCollaboration)
                            {
                                //If we try to create extension collaboration type container with parent container value as empty, then error message should be thrown.
                                //Message: Unable to process the Container ‘{0}’, as the parent container cannot be empty for extension collaboration container.

                                AddOperationResult(operationResult, "114434", String.Empty, new Object[] { deltaContainer.Name }, OperationResultType.Error, TraceEventType.Error, callerContext);
                            }
                        }

                        #endregion validate Child/Parent Link

                        //If original object is not found then set Action as Create always.
                        deltaContainer.Action = ObjectAction.Create;

                        deltaContainer.SupportedLocales.Add(new Locale()
                        {
                            Id = (Int32)_systemDataLocale,
                            Name = _systemDataLocale.ToString(),
                            LongName = _systemDataLocale.GetDescription(),
                            Locale = _systemDataLocale,
                            Action = ObjectAction.Create
                        });
                    }
                    operationResult.PerformedAction = deltaContainer.Action;
                }
            }
        }

        /// <summary>
        /// Log changes to tb_datamodelactivitylog if DA call is sucessfull and appconfig key datamodelactivitylog.enable == true
        /// </summary>
        /// <param name="containerCollection"></param>
        /// <param name="callerContext"></param>
        private void LogDataModelChanges(ContainerCollection containerCollection, CallerContext callerContext)
        {
            #region step: populate datamodelactivitylog object

            DataModelActivityLogBL dataModelActivityLogManager = new DataModelActivityLogBL();
            DataModelActivityLogCollection activityLogCollection = dataModelActivityLogManager.FillDataModelActivityLogCollection(containerCollection);

            #endregion step: populate datamodelactivitylog object

            #region step: make api call

            if (activityLogCollection != null) // null activitylog collection means there was error in mapping
            {
                dataModelActivityLogManager.Process(activityLogCollection, callerContext);
            }

            #endregion step: make api call
        }

        /// <summary>
        /// Compare and Merge Container Locales based on Originial Container
        /// </summary>
        /// <param name="containers">Parameter representing the Container Collection</param>
        private void CompareAndMergeLocales(ContainerCollection containers)
        {
            foreach (Container container in containers)
            {
                MergeLocales(container);
            }
        }

        /// <summary>
        /// Calculate and Merge the locales based on the original containers.
        /// </summary>
        /// <param name="container">Parameter representing the Container object</param>
        private static void MergeLocales(Container container)
        {
            if (container.OriginalContainer != null)
            {
                LocaleCollection originalLocaleCollection = container.OriginalContainer.SupportedLocales;
                var originalLocaleEnums = originalLocaleCollection.Select(a => a.Locale);
                LocaleCollection currentLocaleCollection = container.SupportedLocales;
                var currentLocaleEnums = currentLocaleCollection.Select(a => a.Locale);

                //Calculate Create Actions
                foreach (Locale locale in currentLocaleCollection)
                {
                    if (!originalLocaleEnums.Contains(locale.Locale))
                    {
                        locale.Action = ObjectAction.Create;
                    }
                }

                //Calculate Delete Actions
                foreach (Locale locale in originalLocaleCollection)
                {
                    if (!currentLocaleEnums.Contains(locale.Locale))
                    {
                        Locale removedLocale = new Locale()
                        {
                            Id = (Int32)locale.Id,
                            Name = locale.Name,
                            LongName = locale.LongName,
                            Locale = locale.Locale,
                            Action = ObjectAction.Delete
                        };
                        currentLocaleCollection.Add(removedLocale);
                    }
                }
            }
            else
            {
                //This is create of the container scenario
                foreach (Locale locale in container.SupportedLocales)
                {
                    if (locale.Action == ObjectAction.Read)
                    {
                        locale.Action = ObjectAction.Create;
                    }
                }
            }
        }

        #endregion

        #region EntityCacheLoad For Container

        /// <summary>
        /// Processes the Entity cache statuses for entities in the specified containers.
        /// </summary>
        private void ProcessEntityCacheLoadForContainers(ContainerCollection containerCollection, CallerContext callerContext)
        {
            Collection<Int32> containerIdList = GetContainerIdListForEntityCacheLoad(containerCollection, callerContext);

            if (containerIdList.Count > 0)
            {
                String entityCacheLoadContext = CreateEntityCacheLoadContextForContainer(containerIdList);

                EntityActivityLog entityActivityLog = new EntityActivityLog()
                {
                    PerformedAction = EntityActivityList.EntityCacheLoad,
                    Context = entityCacheLoadContext
                };

                EntityActivityLogCollection entityActivityLogCollection = new EntityActivityLogCollection() { entityActivityLog };

                EntityActivityLogBL entityActivityLogBL = new EntityActivityLogBL();
                entityActivityLogBL.Process(entityActivityLogCollection, callerContext);
            }
        }

        /// <summary>
        /// Returns the Container id list for which locale mapping has been updated.
        /// </summary>
        private Collection<Int32> GetContainerIdListForEntityCacheLoad(ContainerCollection containerCollection, CallerContext callerContext)
        {
            Collection<Int32> containerIdList = new Collection<Int32>();

            Container containerInDB = null;
            foreach (Container container in containerCollection)
            {
                if (container.Action == ObjectAction.Update)
                {
                    containerInDB = GetById(container.Id);
                    if (IsLocaleUpdatedForContainer(container.SupportedLocales, containerInDB.SupportedLocales))
                    {
                        containerIdList.Add(container.Id);
                    }
                }
            }
            return containerIdList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="localeEnumCollection"></param>
        /// <param name="localeEnumCollectionInDB"></param>
        /// <returns></returns>
        private Boolean IsLocaleUpdatedForContainer(LocaleCollection localeCollection, LocaleCollection localeCollectionInDB)
        {
            IEnumerable<LocaleEnum> localeEnumCollection = localeCollection.Select(locale => locale.Locale);
            IEnumerable<LocaleEnum> localeEnumCollectionInDB = localeCollectionInDB.Select(locale => locale.Locale);

            Int32 localesUnion = localeEnumCollection.Union(localeEnumCollectionInDB).Count();
            Int32 localesIntersect = localeEnumCollection.Intersect(localeEnumCollectionInDB).Count();
            return (localesUnion != localesIntersect);
        }

        /// <summary>
        /// Builds and returns the EntityCacheLoadContext in an XML form based on the container. 
        /// </summary>
        private String CreateEntityCacheLoadContextForContainer(Collection<Int32> containerIdList)
        {
            // Create EntityCacheLoadContextItemCollection
            EntityCacheLoadContextItemCollection entityCacheLoadContextItemCollection = new EntityCacheLoadContextItemCollection();

            // Create EntityCacheLoadContextItem for Container
            EntityCacheLoadContextItem entityCacheLoadContextItemForContainer =
                entityCacheLoadContextItemCollection.GetOrCreateEntityCacheLoadContextItem(EntityCacheLoadContextTypeEnum.Container);
            entityCacheLoadContextItemForContainer.AddValues(containerIdList);

            // Create EntityCacheLoadContext with the above parameters
            EntityCacheLoadContext entityCacheLoadContext = new EntityCacheLoadContext();
            entityCacheLoadContext.CacheStatus = (Int32)(EntityCacheComponentEnum.InheritedAttributes | EntityCacheComponentEnum.OverriddenAttributes);
            entityCacheLoadContext.Add(entityCacheLoadContextItemCollection);

            // Generate XML from the object
            String entityCacheLoadContextAsString = entityCacheLoadContext.ToXml();
            return entityCacheLoadContextAsString;
        }

        #endregion

        #endregion Private Methods

        #endregion
    }
}