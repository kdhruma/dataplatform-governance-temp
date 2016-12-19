using System;
using System.Xml;
using System.Linq;
using System.Diagnostics;
using System.Transactions;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MDM.AdminManager.Business
{
    using MDM.Core;
    using MDM.Interfaces;
    using MDM.Utility;
    using MDM.BusinessObjects;
    using MDM.AdminManager.Data;
    using MDM.MessageManager.Business;
    using MDM.ConfigurationManager.Business;
    using MDM.BusinessObjects.DataModel; 
    using MDM.KnowledgeManager.Business;

   
    public class UserPreferencesBL : BusinessLogicBase, IDataModelManager
    {
        #region Fields

        /// <summary>
        /// Field denoting locale Message
        /// </summary>
        private LocaleMessage _localeMessage;

        /// <summary>
        /// Field denoting localeMessageBL
        /// </summary>
        private LocaleMessageBL _localeMessageBL = new LocaleMessageBL();

        /// <summary>
        /// Field denoting reference of OrganizationBL.
        /// </summary>
        private IOrganizationManager _iOrganizationManager = null;

        /// <summary>
        /// Field denoting reference of ContainerBL.
        /// </summary>
        private IContainerManager _iContainerManager = null;

        /// <summary>
        /// Field denoting reference of HierarchyBL.
        /// </summary>
        private IHierarchyManager _iHierarchyManager = null;

        /// <summary>
        /// Filed denoting DataLocale
        /// </summary>
        private LocaleEnum _systemUILocale = LocaleEnum.UnKnown;

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public UserPreferencesBL()
        {
            this._systemUILocale = GlobalizationHelper.GetSystemUILocale();
        }

        /// <summary>
        /// Initializes a new instance of the userPreference BL.
        /// </summary>
        /// <param name="iOrganizationManager">reference of OrganizationBL.</param>
        public UserPreferencesBL(IOrganizationManager iOrganizationManager, IContainerManager iContainerManager, IHierarchyManager iHierarchyManager)
        {
            this._iOrganizationManager= iOrganizationManager;
            this._iContainerManager = iContainerManager;
            this._iHierarchyManager = iHierarchyManager;
            this._systemUILocale = GlobalizationHelper.GetSystemUILocale();
        }

        #endregion

        #region Properties

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userLoginName"></param>
        /// <returns></returns>
        public UserPreferences GetUserPreferences(String userLoginName)
        {
            UserPreferences userPreferences = null;

            try
            {
                UserPreferencesDA userPreferencesDA = new UserPreferencesDA();
                userPreferences = userPreferencesDA.GetUserPreferences(userLoginName, userLoginName);
            }
            catch (Exception ex)
            {
                ApplicationException appException = new ApplicationException("Failed to load user preferences for requested user: " + userLoginName + ". Internal error is: " + ex.Message, ex);
                ExceptionManager.ExceptionHandler exceptionHandler = new ExceptionManager.ExceptionHandler(appException);
            }

            return userPreferences;
        }

        /// <summary>
        /// Process UserPreferences
        /// </summary>
        /// <param name="userLoginName">Name of Login User</param>
        /// <param name="userPreferences">UserPreferences to be processed</param>
        /// <returns>True if process is successful.</returns>
        public Boolean ProcessUserPreferences(String userLogin, UserPreferences userPreferences, CallerContext context)
        {
            #region Parameter Validation

            if (userPreferences == null)
                throw new ArgumentNullException("UserPreferences cannot be null");

            #endregion

            //TODO: Because Circular Reference, not able to call DBCommandHelper of MDM.ConfigurationManager.
            DBCommandProperties command = null;
            UserPreferencesDA userPreferencesDA = new UserPreferencesDA();

            if (userPreferences.Id < 1)
                userPreferences.Action = ObjectAction.Create;

            return userPreferencesDA.ProcessUserPreferences(userLogin, userPreferences, command);
        }

        #region IDataModelManager Methods

        /// <summary>
        /// Generate and OperationResult Schema for given DataModelObjectCollection
        /// </summary>
        /// <param name="iDataModelObjects"></param>
        public void PrepareOperationResultsSchema(IDataModelObjectCollection iDataModelObjects, DataModelOperationResultCollection operationResultCollection)
        {
            UserPreferencesCollection userPreferencesCollection = iDataModelObjects as UserPreferencesCollection;

            if (userPreferencesCollection != null && userPreferencesCollection.Count > 0)
            {
                if (operationResultCollection.Count > 0)
                {
                    operationResultCollection.Clear();
                }

                Int32 userPreferencesToBeCreated = -1;

                foreach (UserPreferences userPreferences in userPreferencesCollection)
                {
                    DataModelOperationResult userPreferencesOperationResult = new DataModelOperationResult(userPreferences.Id, userPreferences.LongName, userPreferences.ExternalId, userPreferences.ReferenceId);

                    if (String.IsNullOrEmpty(userPreferencesOperationResult.ExternalId))
                    {
                        userPreferencesOperationResult.ExternalId = userPreferences.Name;
                    }

                    if (userPreferences.Id < 1)
                    {
                        userPreferences.Id = userPreferencesToBeCreated;
                        userPreferencesOperationResult.Id = userPreferencesToBeCreated;
                        userPreferencesToBeCreated--;
                    }

                    operationResultCollection.Add(userPreferencesOperationResult);
                }
            }
        }

        /// <summary>
        /// Validates DataModelObjects and populate OperationResults
        /// </summary>
        /// <param name="iDataModelObjects">Collection of data model objects to be filled up.</param>
        /// <param name="operationResult">Collection of DataModel OperationResult</param>
        /// <param name="iCallerContext">Context of caller making call to this API.</param>
        public void Validate(IDataModelObjectCollection iDataModelObjects, DataModelOperationResultCollection operationResults, ICallerContext iCallerContext)
        {
            ValidateInputParameters(iDataModelObjects as UserPreferencesCollection, operationResults, iCallerContext as CallerContext);
        }

        /// <summary>
        /// Load original data model objects
        /// </summary>
        /// <param name="iDataModelObjects">Collection of data model objects to load original object.</param>
        /// <param name="operationResult">Collection of DataModel OperationResult</param>
        /// <param name="iCallerContext">Context of caller making call to this API.</param>
        public void LoadOriginal(IDataModelObjectCollection iDataModelObjects, DataModelOperationResultCollection operationResult, ICallerContext iCallerContext)
        {
           UserPreferencesCollection userPreferencesCollection = iDataModelObjects as UserPreferencesCollection;

           if (userPreferencesCollection != null)
            {
                LoadOriginalUserPreferences(userPreferencesCollection, iCallerContext as CallerContext);
            }
        }

        /// <summary>
        /// Fills missing information to data model objects.
        /// </summary>
        /// <param name="iDataModelObjects">Collection of data model objects to be filled up.</param>
        /// <param name="iCallerContext">Context of caller making call to this API.</param>
        public void FillDataModel(IDataModelObjectCollection iDataModelObjects, ICallerContext iCallerContext)
        {
            FillUserPreferences(iDataModelObjects as UserPreferencesCollection, iCallerContext as CallerContext);
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
            CompareAndMerge(iDataModelObjects as UserPreferencesCollection, operationResults, iCallerContext as CallerContext);
        }

        /// <summary>
        /// Process data model collection
        /// </summary>
        /// <param name="iDataModelObjects">Collection of data model objects to be processed.</param>
        /// <param name="iCallerContext">Context of caller making call to this API.</param>
        /// <returns>Result of operation</returns>
        public void Process(IDataModelObjectCollection iDataModelObjects, DataModelOperationResultCollection operationResults, ICallerContext iCallerContext)
        {
            UserPreferencesCollection userPreferencesCollection = iDataModelObjects as UserPreferencesCollection;
            CallerContext callerContext = (CallerContext)iCallerContext;

            if (userPreferencesCollection.Count > 0)
            {
                DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Execute);

                #region Perform user preferences updates in database

                using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
                {
                    new UserPreferencesDA().ProcessUserPreferences(userPreferencesCollection, operationResults, command);
                    transactionScope.Complete();
                }

                LocalizeErrors(operationResults, callerContext);

                #endregion
            }
        }

        /// <summary>
        /// Processes the User preferences statuses for data model objects
        /// </summary>
        /// <param name="iDataModelObjects">Collection of data model objects to be process user preferences cache load.</param>
        /// <param name="iCallerContext">Context of caller making call to this API.</param>
        public void InvalidateCache(IDataModelObjectCollection iDataModelObjects, DataModelOperationResultCollection operationResult, ICallerContext iCallerContext)
        {
            //Currently User Preferences does not support caching
        }

        #endregion

        #endregion

        #region Private Methods

        private String GetNodeElementValueAsString(XmlElement element, String attributeName)
        {
            String output = String.Empty;
            if (element != null)
            {
                output = element.GetAttribute(attributeName);
            }

            return output;
        }

        private Int32 GetNodeElementValueAsInteger(XmlElement element, String attributeName)
        {
            Int32 output = 0;

            String value = GetNodeElementValueAsString(element, attributeName);
            Int32.TryParse(value, out output);
            
            return output;
        }
        
        private void LoadOriginalUserPreferences(UserPreferencesCollection userPreferencesCollection, CallerContext callerContext)
        {
            foreach (UserPreferences userPreferences in userPreferencesCollection)
            {
                UserPreferences originalUserPreferences = GetUserPreferences(userPreferences.LoginName);
               
               if(originalUserPreferences != null && originalUserPreferences.Id > 0)
               {
                    userPreferences.OriginalUserPreferences = originalUserPreferences;
               }
            }
        }

        private void ValidateInputParameters(UserPreferencesCollection userPreferencesCollection, DataModelOperationResultCollection operationResults, CallerContext callerContext)
        {
            if (userPreferencesCollection == null || userPreferencesCollection.Count < 1)
            {
                AddOperationResults(operationResults, "113588", "User Preferences are not available or empty", null, OperationResultType.Error, TraceEventType.Error, callerContext);
            }

            if (callerContext == null)
            {
                AddOperationResults(operationResults, "111846", "CallerContext cannot be null.", null, OperationResultType.Error, TraceEventType.Error, callerContext);
            }
        }

        private void FillUserPreferences(UserPreferencesCollection userPreferencesCollection, CallerContext callerContext)
        {
            #region Local Variables and Object Initialization

            Dictionary<String, Organization> organizationDictionary = new Dictionary<String, Organization>();
            Dictionary<String, Container> containerDictionary = new Dictionary<String, Container>();
            Dictionary<String, Hierarchy> hierarchyDictionary = new Dictionary<String, Hierarchy>();
            Dictionary<String, SecurityRole> securityRoleDictionary = new Dictionary<String, SecurityRole>();
            Dictionary<String, SecurityUser> securityUserDictionary = new Dictionary<String, SecurityUser>();
            SecurityRoleBL securityRoleBL = new SecurityRoleBL();
            SecurityUserBL securityUserBL = new SecurityUserBL();
            TimeZoneBL timeZoneBL = new TimeZoneBL();
                        
            #endregion

            #region Fill missing details

            Collection<TimeZone> timeZones = timeZoneBL.GetAll();
            SecurityRoleCollection securityRoles = securityRoleBL.GetAll(callerContext);
                            
            foreach (UserPreferences userPreferences in userPreferencesCollection)
            {
                #region Fill user preference Id

                if (userPreferences.Id < 1)
                {
                    userPreferences.Id = (userPreferences.OriginalUserPreferences != null) ? userPreferences.OriginalUserPreferences.Id : userPreferences.Id;
                }

                #endregion

                #region Fill organization details

                if (userPreferences.DefaultOrgId < 1)
                {
                    if (userPreferences.OriginalUserPreferences != null && userPreferences.OriginalUserPreferences.DefaultOrgName.Equals(userPreferences.DefaultOrgName, StringComparison.OrdinalIgnoreCase))
                    {
                        userPreferences.DefaultOrgId = userPreferences.OriginalUserPreferences.DefaultOrgId;
                    }
                    else
                    {
                        Organization organization = null;
                        if (organizationDictionary.ContainsKey(userPreferences.DefaultOrgName))
                        {
                            organization = organizationDictionary[userPreferences.DefaultOrgName];
                        }

                        if (organization != null)
                        {
                            userPreferences.DefaultOrgId = organization.Id;
                            userPreferences.DefaultOrgName = organization.Name;
                            userPreferences.DefaultOrgLongName = organization.LongName;
                        }
                        else
                        {

                            organization = this._iOrganizationManager.GetByName(userPreferences.DefaultOrgName, new OrganizationContext(), callerContext);

                            if (organization != null)
                            {
                                userPreferences.DefaultOrgId = organization.Id;
                                userPreferences.DefaultOrgLongName = organization.LongName;

                                organizationDictionary.Add(userPreferences.DefaultOrgName, organization);
                            }

                        }
                    }
                }

                #endregion

                #region Fill container details

                if (userPreferences.DefaultContainerId < 1)
                {
                    if (userPreferences.OriginalUserPreferences != null && userPreferences.OriginalUserPreferences.DefaultContainerName.Equals(userPreferences.DefaultContainerName, StringComparison.OrdinalIgnoreCase))
                    {
                        userPreferences.DefaultContainerId = userPreferences.OriginalUserPreferences.DefaultContainerId;
                    }
                    else if (!String.IsNullOrWhiteSpace(userPreferences.DefaultContainerName))
                    {
                        Container container = null;

                        if (containerDictionary.ContainsKey(userPreferences.DefaultContainerName))
                        {
                            container = containerDictionary[userPreferences.DefaultContainerName];
                        }

                        if (container != null)
                        {
                            userPreferences.DefaultContainerId = container.Id;
                            userPreferences.DefaultContainerName = container.Name;
                            userPreferences.DefaultContainerLongName = container.LongName;
                        }
                        else
                        {
                            container = this._iContainerManager.Get(userPreferences.DefaultContainerName, new ContainerContext(), callerContext);

                            if (container != null)
                            {
                                userPreferences.DefaultContainerId = container.Id;
                                userPreferences.DefaultContainerLongName = container.LongName;

                                containerDictionary.Add(userPreferences.DefaultContainerName, container);
                            }
                        }
                    }
                }

                if (userPreferences.DefaultHierarchyId < 1)
                {
                    if (userPreferences.OriginalUserPreferences != null && userPreferences.OriginalUserPreferences.DefaultHierarchyName.Equals(userPreferences.DefaultHierarchyName, StringComparison.OrdinalIgnoreCase))
                    {
                        userPreferences.DefaultHierarchyId = userPreferences.OriginalUserPreferences.DefaultHierarchyId;
                    }
                    else if (!String.IsNullOrWhiteSpace(userPreferences.DefaultHierarchyName))
                    {
                        Hierarchy hierarchy = null;

                        if (hierarchyDictionary.ContainsKey(userPreferences.DefaultHierarchyName))
                        {
                            hierarchy = hierarchyDictionary[userPreferences.DefaultHierarchyName];
                        }

                        if (hierarchy != null)
                        {
                            userPreferences.DefaultHierarchyId = hierarchy.Id;
                            userPreferences.DefaultHierarchyName= hierarchy.Name;
                            userPreferences.DefaultHierarchyLongName = hierarchy.LongName;
                        }
                        else
                        {
                            hierarchy = this._iHierarchyManager.GetByName(userPreferences.DefaultHierarchyName, callerContext);

                            if (hierarchy != null)
                            {
                                userPreferences.DefaultHierarchyId = hierarchy.Id;
                                userPreferences.DefaultHierarchyLongName = hierarchy.LongName;

                                hierarchyDictionary.Add(userPreferences.DefaultHierarchyName, hierarchy);
                            }
                        }
                    }
                }

                #endregion

                #region Fill Security role details

                if (userPreferences.DefaultRoleId < 1)
                {
                    if (userPreferences.OriginalUserPreferences != null && userPreferences.OriginalUserPreferences.DefaultRoleName.Equals(userPreferences.DefaultRoleName, StringComparison.OrdinalIgnoreCase))
                    {
                        userPreferences.DefaultRoleId = userPreferences.OriginalUserPreferences.DefaultRoleId;
                    }
                    else
                    {
                        SecurityRole securityRole = null;
                        
                        if (securityRoleDictionary.ContainsKey(userPreferences.DefaultRoleName))
                        {
                            securityRole = securityRoleDictionary[userPreferences.DefaultRoleName];
                        }

                        if (securityRole != null)
                        {
                            userPreferences.DefaultRoleId = securityRole.Id;
                            userPreferences.DefaultRoleName = securityRole.Name;
                        }
                        else
                        {
                           
                            if(securityRoles != null && securityRoles.Count > 0)
                            {
                                foreach(SecurityRole role in securityRoles)
                                {
                                    if(role.Name.ToLowerInvariant().Equals(userPreferences.DefaultRoleName.ToLowerInvariant()))
                                    {
                                        securityRoleDictionary.Add(userPreferences.DefaultRoleName, role);
                                        userPreferences.DefaultRoleId = role.Id;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }

                #endregion

                #region Fill Security user details

                if (userPreferences.LoginId< 1)
                {
                    if (userPreferences.OriginalUserPreferences != null && userPreferences.OriginalUserPreferences.LoginName.Equals(userPreferences.LoginName, StringComparison.OrdinalIgnoreCase))
                    {
                        userPreferences.LoginId = userPreferences.OriginalUserPreferences.LoginId;
                    }
                    else
                    {
                        SecurityUser securityUser = null;
                        if (securityUserDictionary.ContainsKey(userPreferences.LoginName))
                        {
                            securityUser = securityUserDictionary[userPreferences.LoginName];
                        }

                        if (securityUser != null)
                        {
                            userPreferences.LoginName = securityUser.Name;
                        }
                        else
                        {
                            securityUser = securityUserBL.GetUser(userPreferences.LoginName, callerContext);

                            if (securityUser != null)
                            {
                                userPreferences.LoginId = securityUser.Id;

                                securityUserDictionary.Add(userPreferences.LoginName, securityUser);
                            }
                        }
                    }
                }

                #endregion

                #region Fill Time Zone details

                if (userPreferences.DefaultTimeZoneId < 1)
                {
                    if (userPreferences.OriginalUserPreferences != null)
                    {
                        userPreferences.DefaultTimeZoneId= userPreferences.OriginalUserPreferences.DefaultTimeZoneId;
                    }
                    else
                    {
                        if(timeZones != null)
                        {
                            foreach(TimeZone currentTimeZone in timeZones)
                            {
                                if(currentTimeZone.Name.ToLowerInvariant().Equals(userPreferences.DefaultTimeZoneShortName.ToLowerInvariant()))
                                {
                                    userPreferences.DefaultTimeZoneId = currentTimeZone.Id;
                                    break;
                                }
                            }
                        }
                    }
                }

                #endregion
            }

            #endregion
        }

        private void CompareAndMerge(UserPreferencesCollection userPreferencesCollection, DataModelOperationResultCollection operationResults, CallerContext callerContext)
        {

            Dictionary<Int32, LocaleCollection> containerLocaleMap = new Dictionary<int, LocaleCollection>();
            LocaleBL localeManager = new LocaleBL();

            foreach (UserPreferences deltaUserPreferences in userPreferencesCollection)
            {
                IDataModelOperationResult operationResult = operationResults.GetByReferenceId(deltaUserPreferences.ReferenceId);

                //If delta object Action is Read/Ignore then skip Merge Delta Process.
                if (deltaUserPreferences.Action == ObjectAction.Read || deltaUserPreferences.Action == ObjectAction.Ignore)
                    continue;

                IUserPreferences origUserPreferences= deltaUserPreferences.OriginalUserPreferences;

                //If the requested user login does not exist, User Preferences get procedure returns the object with Id as -1.
                if (origUserPreferences != null)
                {
                    //If delta object Action is Delete then skip Merge Delta process.
                    if (deltaUserPreferences.Action != ObjectAction.Delete)
                    {
                        origUserPreferences.MergeDelta(deltaUserPreferences, callerContext, false);
                    }
                }
                else
                {
                    if (deltaUserPreferences.Action == ObjectAction.Delete)
                    {
                        AddOperationResult(operationResult, "113614", String.Empty, new Object[] { deltaUserPreferences.Name, deltaUserPreferences.LoginName }, OperationResultType.Error, TraceEventType.Error, callerContext);
                    }
                    else
                    {
                        //If original object is not found then set Action as Create always.
                        deltaUserPreferences.Action = ObjectAction.Create;
                    }
                }
                Int32 containerId = deltaUserPreferences.DefaultContainerId;
                if (containerId > 0)
                {
                    LocaleCollection containerLocales;
                    if(!containerLocaleMap.TryGetValue(containerId, out containerLocales))
                    {
                        Container container = this._iContainerManager.Get(containerId, callerContext, false);
                        containerLocales = container.SupportedLocales;
                        containerLocaleMap.Add(containerId, containerLocales);
                    }

                    if (!containerLocales.Contains(deltaUserPreferences.DataLocale))
                    {
                        AddOperationResult(operationResult, "114271", String.Empty, new Object[] { deltaUserPreferences.DataLocale.ToString(), deltaUserPreferences.LoginName, deltaUserPreferences.DefaultContainerName }, OperationResultType.Error, TraceEventType.Error, callerContext);
                    }
                }
                

                operationResult.PerformedAction = deltaUserPreferences.Action;
            }
        }

        #region Utility Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="callerContext"></param>
        /// <param name="entityTypeProcessOperationResult"></param>
        private void LocalizeErrors(CallerContext callerContext, OperationResult entityTypeProcessOperationResult)
        {
            foreach (Error error in entityTypeProcessOperationResult.Errors)
            {
                if (!String.IsNullOrEmpty(error.ErrorCode))
                {
                    LocaleMessage localeMessage = new LocaleMessageBL().Get(GlobalizationHelper.GetSystemUILocale(), error.ErrorCode, false, callerContext);

                    if (localeMessage != null)
                    {
                        error.ErrorMessage = localeMessage.Message;
                    }
                }
            }
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
                        _localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), error.ErrorCode, false, callerContext);

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
        /// <param name="messageCode"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        private LocaleMessage GetSystemLocaleMessage(String messageCode, CallerContext callerContext)
        {
            LocaleMessageBL localeMessageBL = new LocaleMessageBL();
            return localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), messageCode, false, callerContext);
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

        #endregion

        #endregion
    }
}
