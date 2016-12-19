using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Transactions;
using SM = System.ServiceModel;

namespace MDM.AttributeModelManager.Business
{
    using MDM.ActivityLogManager.Business;
    using MDM.AdminManager.Business;
    using MDM.AttributeDependencyManager.Business;
    using MDM.AttributeModelManager.Data;
    using MDM.BufferManager;
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.DataModel;
    using MDM.BusinessObjects.Diagnostics;
    using MDM.ConfigurationManager.Business;
    using MDM.Core;
    using MDM.Core.Exceptions;
    using MDM.Core.Extensions;
    using MDM.DataModelManager.Business;
    using MDM.ExcelUtility;
    using MDM.Interfaces;
    using MDM.KnowledgeManager.Business;
    using MDM.MessageManager.Business;
    using MDM.Utility;

    /// <summary>
    /// Specifies the business operations for attribute model
    /// </summary>
    public class AttributeModelBL : BusinessLogicBase, IAttributeModelManager, IDataModelManager
    {
        #region Fields

        /// <summary>
        /// Specifies Attribute Key Separator
        /// </summary>
        private const Char AttributeKeySeparator = '_';

        /// <summary>
        /// Specifies security principal for user.
        /// </summary>
        private SecurityPrincipal _securityPrincipal = null;

        /// <summary>
        /// 
        /// </summary>
        AttributeModelBufferManager _attributeModelBufferManager = null;

        /// <summary>
        /// 
        /// </summary>
        SM.OperationContext _operationContext;

        /// <summary>
        /// 
        /// </summary>
        AttributeModelMappingPropertiesBL _mappingPropertiesManager = null;

        /// <summary>
        /// Field denoting locale Message
        /// </summary>
        private LocaleMessage _localeMessage = null;

        /// <summary>
        /// Field denoting localeMessageBL
        /// </summary>
        private LocaleMessageBL _localeMessageBL = null;

        /// <summary>
        /// Specifies Attribute Validation Options
        /// </summary>
        private IDataModelValidationOptions _dataModelValidationOptions = null;

        /// <summary>
        /// Specifies Lookup Manager
        /// </summary>
        private ILookupManager _lookupManager = null;

        /// <summary>
        /// Specifies Lookup Metadata
        /// </summary>
        private Lookup _lookupMetadata = null;

        /// <summary>
        /// 
        /// </summary>
        private LocaleEnum _systemDataLocale;

        /// <summary>
        /// Specifies lock object used while creating attribute model dependency dictionary in cache
        /// </summary>
        private static Object _lockObj = new Object();

        /// <summary>
        /// 
        /// </summary>
        private LocaleEnum _systemUILocale;

        /// <summary>
        /// Indicates current trace settings
        /// </summary>
        private TraceSettings _currentTraceSettings = new TraceSettings();

        #endregion

        #region Constructors

        /// <summary>
        /// Default Constructor that loads the security principal from Cache if present
        /// </summary>
        public AttributeModelBL()
        {
            GetSecurityPrincipal();
            _systemDataLocale = GlobalizationHelper.GetSystemDataLocale();
            _systemUILocale = GlobalizationHelper.GetSystemUILocale();
            _currentTraceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
        }

        /// <summary>
        /// Constructor which takes the security principal if its already present in the system 
        /// </summary>
        /// <param name="securityPrincipalInstance">The user specific security principal of this instance</param>
        public AttributeModelBL(SecurityPrincipal securityPrincipalInstance)
        {
            _securityPrincipal = securityPrincipalInstance;
            SecurityPrincipalHelper.ValidateSecurityPrincipal(_securityPrincipal);
            _systemDataLocale = GlobalizationHelper.GetSystemDataLocale();
            _systemUILocale = GlobalizationHelper.GetSystemUILocale();
            _currentTraceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
        }

        /// <summary>
        /// Specifies a constructor which injects managers required for handling AttributeModels
        /// </summary>
        /// <param name="dataModelValidationOptions">Specifies attribute validation options</param>
        /// <param name="lookupManager">Specifies lookup manager</param>
        public AttributeModelBL(IDataModelValidationOptions dataModelValidationOptions, ILookupManager lookupManager)
            : this()
        {
            _dataModelValidationOptions = dataModelValidationOptions;
            _lookupManager = lookupManager;
        }

        #endregion

        #region Properties

        private AttributeModelBufferManager AttributeModelBufferManager
        {
            get
            {
                if (this._attributeModelBufferManager == null)
                {
                    this._attributeModelBufferManager = new AttributeModelBufferManager();
                }

                return this._attributeModelBufferManager;
            }
        }

        private AttributeModelMappingPropertiesBL MappingPropertiesManager
        {
            get
            {
                if (this._mappingPropertiesManager == null)
                {
                    this._mappingPropertiesManager = new AttributeModelMappingPropertiesBL();
                }

                return this._mappingPropertiesManager;
            }
        }

        private LocaleMessageBL LocaleMessageManager
        {
            get
            {
                if (this._localeMessageBL == null)
                {
                    this._localeMessageBL = new LocaleMessageBL();
                }

                return this._localeMessageBL;
            }
        }

        #endregion

        #region Methods

        #region Public Methods

        #region Get Methods

        /// <summary>
        /// Gets the Id of Attribute coming under requested Attribute Group and having Attribute Name
        /// </summary>
        /// <param name="attributeName">Attribute short name</param>
        /// <param name="attributeParentName">Attribute parent name</param>
        /// <returns>Id of Attribute</returns>
        public Int32 GetAttributeId(String attributeName, String attributeParentName)
        {
            Int32 attributeId = 0;
            AttributeUniqueIdentifier attributeUniqueIdentifier = new AttributeUniqueIdentifier(attributeName, attributeParentName);

            Collection<Int32> attributeIds = GetAttributeIdList(new AttributeUniqueIdentifierCollection() { attributeUniqueIdentifier });

            if (attributeIds != null && attributeIds.Count > 0)
            {
                attributeId = attributeIds.SingleOrDefault();
            }

            return attributeId;
        }

        /// <summary>
        /// Gets Attribute id list coming under requested Attribute unique identifier - i.e. Attribute Name and group name.
        /// </summary>
        /// <param name="attributeUniqueIdentifiers">Unique identifier for attribute model containing attribute name and parent name</param>
        /// <returns>List of attribute ids</returns>
        public Collection<Int32> GetAttributeIdList(AttributeUniqueIdentifierCollection attributeUniqueIdentifiers)
        {
            #region Parameter validation

            ValidateAttributeUniqueIdentifierCollection(attributeUniqueIdentifiers, "GetAttributeIdList");

            #endregion

            var attributeIdList = new Collection<Int32>();

            if (attributeUniqueIdentifiers != null && attributeUniqueIdentifiers.Count > 0)
            {
                Dictionary<String, Int32> allAttributeModels = GetAttributeUniqueIdentifierToAttributeIdMaps();
                Dictionary<String, Int32> attributeNameToIdMaps = null;

                if (allAttributeModels != null && allAttributeModels.Count > 0)
                {
                    foreach (AttributeUniqueIdentifier attributeUniqueIdentifier in attributeUniqueIdentifiers)
                    {
                        ValidateAttributeUniqueIdentifier(attributeUniqueIdentifier, "GetAttributeIdList");

                        Int32 attributeId = 0;
                        String key = String.Empty;

                        if (String.IsNullOrWhiteSpace(attributeUniqueIdentifier.AttributeGroupName))
                        {
                            if (attributeNameToIdMaps == null)
                            {
                                attributeNameToIdMaps = GetAttributeNameToAttributeIdMaps();
                            }

                            attributeNameToIdMaps.TryGetValue(attributeUniqueIdentifier.AttributeName.ToLower(), out attributeId);
                        }
                        else
                        {
                            key = GetKey(attributeUniqueIdentifier.AttributeName, attributeUniqueIdentifier.AttributeGroupName);
                            allAttributeModels.TryGetValue(key, out attributeId);
                        }

                        if (attributeId > 0)
                        {
                            attributeIdList.Add(attributeId);
                        }
                    }
                }
            }

            return attributeIdList;
        }

        /// <summary>
        /// Gets Attribute id list coming under requested Attribute names. Here assumption is that requested attribute name is unique across the system
        /// </summary>
        /// <param name="attributeNames">list of attribute names</param>
        /// <returns>List of attribute ids</returns>
        public Collection<Int32> GetAttributeIdList(Collection<String> attributeNames)
        {
            var attributeIdList = new Collection<Int32>();

            if (attributeNames != null && attributeNames.Count > 0)
            {

                Dictionary<String, Int32> attributeNameToAttributeIdMaps = GetAttributeNameToAttributeIdMaps();

                if (attributeNameToAttributeIdMaps != null && attributeNameToAttributeIdMaps.Count > 0)
                {
                    foreach (String attributeName in attributeNames)
                    {
                        String key = GetKey(attributeName);

                        if (attributeNameToAttributeIdMaps.ContainsKey(key))
                        {
                            Int32 attributeId;

                            attributeNameToAttributeIdMaps.TryGetValue(key, out attributeId);

                            if (attributeId > 0)
                            {
                                attributeIdList.Add(attributeId);
                            }
                        }
                        else
                        {
                            throw new MDMOperationException(String.Format("Requested attribute with name:'{0}' is not found in base attribute models", attributeName));
                        }
                    }
                }
            }

            return attributeIdList;
        }

        /// <summary>
        /// Gets Attribute model coming under requested Attribute unique identifier - i.e. Attribute Name and group name.
        /// </summary>
        /// <param name="attributeUniqueIdentifier">Unique identifier for attribute model containing attribute name and parent name</param>
        /// <param name="callerContext">Context indicating the caller of the API</param>
        /// <returns>Attribute model</returns>
        public AttributeModel GetByUniqueIdentifier(AttributeUniqueIdentifier attributeUniqueIdentifier, CallerContext callerContext)
        {
            return GetByUniqueIdentifier(attributeUniqueIdentifier, _systemDataLocale, callerContext);
        }

        /// <summary>
        /// Gets Attribute model coming under requested Attribute unique identifier - i.e. Attribute Name and group name.
        /// </summary>
        /// <param name="attributeUniqueIdentifier">Unique identifier for attribute model containing attribute name and parent name</param>
        /// <param name="locale">Locale of attribute model</param>
        /// <param name="callerContext">Context indicating the caller of the API</param>
        /// <returns>Attribute model</returns>
        public AttributeModel GetByUniqueIdentifier(AttributeUniqueIdentifier attributeUniqueIdentifier, LocaleEnum locale, CallerContext callerContext)
        {
            AttributeModel attributeModel = null;
            AttributeUniqueIdentifierCollection attributeUniqueIdentifiers = null;

            if (attributeUniqueIdentifier != null)
            {
                attributeUniqueIdentifiers = new AttributeUniqueIdentifierCollection() { attributeUniqueIdentifier };
            }

            AttributeModelCollection attributeModels = GetByUniqueIdentifiers(attributeUniqueIdentifiers, locale, callerContext);

            if (attributeModels != null && attributeModels.Count == 1)
            {
                attributeModel = attributeModels.SingleOrDefault();
            }

            return attributeModel;
        }

        /// <summary>
        /// Get Attribute Models By AttributeUniqueIdentifier (AttributeName, AttributeGroupName) for requested Locales
        /// </summary>
        /// <param name="attributeUniqueIdentifier">Indicates AttributeName and AttributeGroupName</param>
        /// <param name="locales">Indicates List of Locales</param>
        /// <param name="callerContext">Indicates Context of the Caller</param>
        /// <returns>AttributeModelCollection</returns>
        public AttributeModelCollection GetByUniqueIdentifier(AttributeUniqueIdentifier attributeUniqueIdentifier, Collection<LocaleEnum> locales, CallerContext callerContext)
        {
            AttributeModelContext attributeModelContext = new AttributeModelContext();
            attributeModelContext.AttributeModelType = AttributeModelType.AttributeMaster;
            attributeModelContext.Locales = locales;

            AttributeUniqueIdentifierCollection attributeUniqueIdentifiers = new AttributeUniqueIdentifierCollection() { attributeUniqueIdentifier };

            return GetAttributeModelsByUniqueIdentifiers(attributeUniqueIdentifiers, attributeModelContext, callerContext);
        }

        /// <summary>
        /// Gets Attribute models coming under requested Attribute unique identifier - i.e. Attribute Name and group name.
        /// </summary>
        /// <param name="attributeUniqueIdentifiers">Unique identifier for attribute model containing attribute name and parent name</param>
        /// <param name="callerContext">Context indicating the caller of the API</param>
        /// <returns>Attribute model collection</returns>
        public AttributeModelCollection GetByUniqueIdentifiers(AttributeUniqueIdentifierCollection attributeUniqueIdentifiers, CallerContext callerContext)
        {
            return GetByUniqueIdentifiers(attributeUniqueIdentifiers, _systemDataLocale, callerContext);
        }

        /// <summary>
        /// Gets Attribute models coming under requested Attribute unique identifier - i.e. Attribute Name and group name.
        /// </summary>
        /// <param name="attributeUniqueIdentifiers">Unique identifier for attribute model containing attribute name and parent name</param>
        /// <param name="locale">Lcale for attribute models</param>
        /// <param name="callerContext">Context indicating the caller of the API</param>
        /// <returns>Attribute model collection</returns>
        public AttributeModelCollection GetByUniqueIdentifiers(AttributeUniqueIdentifierCollection attributeUniqueIdentifiers, LocaleEnum locale, CallerContext callerContext)
        {
            AttributeModelContext attributeModelContext = new AttributeModelContext();
            attributeModelContext.AttributeModelType = AttributeModelType.AttributeMaster;
            attributeModelContext.Locales = new Collection<LocaleEnum> { locale };

            return GetAttributeModelsByUniqueIdentifiers(attributeUniqueIdentifiers, attributeModelContext, callerContext);
        }

        /// <summary>
        /// Gets the attribute models for the requested ContainerId, CategoryId, EntityTypeId, AttributeModelType.
        /// It will load all attribute models.
        /// </summary>
        /// <param name="containerId">Indicates container Id for which attribute models are to be loaded</param>
        /// <param name="categoryId">Indicates category Id for which attribute models are to be loaded</param>
        /// <param name="entityTypeId">Indicates entity type Id for which attribute models are to be loaded</param>
        /// <param name="locales">List of locales in which AttributeModels are to be loaded.</param>
        /// <param name="attributeModelType">Indicates which type of attributes are to be loaded. If no value is given then AttributeModelType.All will be taken</param>
        /// <returns>Collection of Attribute Models object qualified for given context</returns>
        /// <exception cref="ArgumentException">Thrown if either of containerId , categoryId or entityTypeId value is less than 0</exception>
        public AttributeModelCollection Get(Int32 containerId, Int64 categoryId, Int32 entityTypeId, Collection<LocaleEnum> locales, AttributeModelType attributeModelType = AttributeModelType.All)
        {
            AttributeModelContext attributeModelContext = new AttributeModelContext();
            attributeModelContext.CategoryId = categoryId;
            attributeModelContext.ContainerId = containerId;
            attributeModelContext.EntityTypeId = entityTypeId;
            attributeModelContext.AttributeModelType = attributeModelType;
            attributeModelContext.Locales = locales;
            attributeModelContext.GetCompleteDetailsOfAttribute = true;

            AttributeModelCollection attributeModels = Get(attributeModelContext);

            return attributeModels;
        }

        /// <summary>
        /// Gets the attribute model for the requested attribute id
        /// </summary>
        /// <param name="attributeId">Id of the attribute for which model is required</param>
        /// <param name="attributeModelContext">The data context for which model needs to be fetched</param>
        /// <returns>Attribute Model Collection object</returns>
        /// <exception cref="ArgumentNullException">Will be raised when the parameter 'attributeModelContext' is null</exception>
        public AttributeModelCollection GetById(Int32 attributeId, AttributeModelContext attributeModelContext)
        {
            //Prepare attribute Ids array
            Collection<Int32> attributeIds = new Collection<Int32> { attributeId };

            //Get attribute model
            AttributeModelCollection attributeModelCollection = GetByIds(attributeIds, attributeModelContext);

            //return
            return attributeModelCollection;
        }

        /// <summary>
        /// Gets the attribute models context
        /// </summary>
        /// <param name="attributeModelContext">The data context for which models needs to be fetched</param>
        /// <returns>Collection of Attribute Models object</returns>
        /// <exception cref="ArgumentNullException">Will be raised when the parameter 'attributeModelContext' is null</exception>
        /// <exception cref="ArgumentException">Will be raised when attributeModelContext parameters are not provided</exception>
        public AttributeModelCollection Get(AttributeModelContext attributeModelContext)
        {
            //Validate AttributeModelContext
            ValidateAttributeModelContext(attributeModelContext);

            var attributeModels = GetAttributeModels(null, null, null, attributeModelContext);

            return attributeModels;
        }

        /// <summary>
        /// Gets the attribute models context
        /// </summary>
        /// <param name="attributeModelContext">The data context for which models needs to be fetched</param>
        /// <param name="iEntityModelManager">Specifies the defination for an entity model manager</param>
        /// <param name="callerContext">Specifies caller context</param>
        /// <returns>Collection of Attribute Models object</returns>
        /// <exception cref="ArgumentNullException">Will be raised when the parameter 'attributeModelContext' is null</exception>
        /// <exception cref="ArgumentException">Will be raised when attributeModelContext parameters are not provided</exception>
        public AttributeModelCollection Get(AttributeModelContext attributeModelContext, IEntityModelManager iEntityModelManager, CallerContext callerContext)
        {
            //Populate Ids by Name
            PopulateIdsByName(attributeModelContext, iEntityModelManager, callerContext);

            return Get(attributeModelContext);
        }

        /// <summary>
        /// Gets the attribute models for the requested attribute ids, attribute group ids, custom view id and state view id based on attribute model context
        /// </summary>
        /// <param name="attributeIds">Ids of the attributes for which models are required</param>
        /// <param name="attributeGroupIds">Ids of attribute groups for which models are required</param>
        /// <param name="customViewId">Custom view id of which models are required</param>
        /// <param name="stateViewId">State view id of which models are required</param>
        /// <param name="excludeAttributeIds">Ids which needs to be excluded</param>
        /// <param name="attributeModelContext">The data context for which models needs to be fetched</param>
        /// <returns>Collection of Attribute Models object</returns>
        /// <exception cref="ArgumentNullException">Will be raised when the parameter 'attributeModelContext' is null</exception>
        /// <exception cref="ArgumentException">Will be raised when 'attributeIds' and 'attributeGroupIds' and 'customiewId' and 'stateViewId' are not provided</exception>
        public AttributeModelCollection Get(Collection<Int32> attributeIds, Collection<Int32> attributeGroupIds, Int32 customViewId, Int32 stateViewId, Collection<Int32> excludeAttributeIds, AttributeModelContext attributeModelContext)
        {
            AttributeModelCollection attributeModels = null;
            try
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StartTraceActivity("AttributeModelBL.Get", MDMTraceSource.AttributeModelGet, false);

                //Validate AttributeModelContext
                ValidateAttributeModelContext(attributeModelContext);

                if (customViewId > 0 || stateViewId > 0)
                {
                    Collection<Int32> attrIds = GetAttributeIdList(null, customViewId, stateViewId, attributeModelContext);

                    if (attrIds != null && attrIds.Count > 0)
                    {
                        var enumerator = attrIds.GetEnumerator();
                        if (attributeIds == null) attributeIds = new Collection<Int32>();

                        while (enumerator.MoveNext())
                        {
                            attributeIds.Add(enumerator.Current);
                        }
                    }
                }

                if ((attributeIds == null || attributeIds.Count == 0) && (attributeGroupIds == null || attributeGroupIds.Count == 0))
                {
                    _localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), "111964", false, new CallerContext(MDMCenterApplication.PIM, MDMCenterModules.Entity));
                    throw new MDMOperationException("111964", _localeMessage.Message, "AttributeModelManager", String.Empty, "Get"); //Attribute models cannot be fetched. Please provide attribute details for which models are required.
                }

                attributeModels = GetAttributeModels(attributeIds, attributeGroupIds, excludeAttributeIds, attributeModelContext);
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("AttributeModelBL.Get", MDMTraceSource.AttributeModelGet);
            }

            return attributeModels;
        }

        /// <summary>
        /// Gets the attribute models for the requested attribute ids and attribute group ids based on attribute model context
        /// </summary>
        /// <param name="attributeIds">Ids of the attributes for which models are required</param>
        /// <param name="attributeGroupIds">Ids of attribute groups for which models are required</param>
        /// <param name="excludeAttributeIds">Ids which needs to be excluded</param>
        /// <param name="attributeModelContext">The data context for which models needs to be fetched</param>
        /// <returns>Collection of Attribute Models object</returns>
        /// <exception cref="ArgumentNullException">Will be raised when the parameter 'attributeModelContext' is null</exception>
        /// <exception cref="ArgumentException">Will be raised when either 'attributeIds' or 'attributeGroupIds' are not provided</exception>
        public AttributeModelCollection Get(Collection<Int32> attributeIds, Collection<Int32> attributeGroupIds, Collection<Int32> excludeAttributeIds, AttributeModelContext attributeModelContext)
        {
            //Validate AttributeModelContext
            ValidateAttributeModelContext(attributeModelContext);

            AttributeModelCollection attributeModels = null;

            attributeModels = GetAttributeModels(attributeIds, attributeGroupIds, excludeAttributeIds, attributeModelContext);

            //return
            return attributeModels;
        }

        /// <summary>
        /// Gets the attribute models from the requested entity context
        /// </summary>
        /// <param name="entityContext">entity context</param>
        /// <returns>attribute model collection for given entity context</returns>
        public AttributeModelCollection Get(EntityContext entityContext)
        {
            #region Validation

            if (entityContext == null)
            {
                throw new MDMOperationException("111842", "EntityContext cannot be null.", "AttributeModelManager.AttributeModelBL", String.Empty, "Get");
            }

            #endregion Validation

            AttributeModelCollection attributeModels = new AttributeModelCollection();

            AttributeModelContext attributeModelContext = new AttributeModelContext(entityContext.ContainerId, entityContext.EntityTypeId, 0/*Relationship attributes load will be a separate call*/,
                entityContext.CategoryId, entityContext.DataLocales, 0, entityContext.AttributeModelType, entityContext.LoadCreationAttributes, entityContext.LoadRequiredAttributes, false);

            if (entityContext.AttributeIdList != null && entityContext.AttributeIdList.Count > 0)
                attributeModels = Get(entityContext.AttributeIdList, entityContext.AttributeGroupIdList, null, attributeModelContext);
            else // get all the mapped attributes for the given entity..
                attributeModels = Get(attributeModelContext);

            return attributeModels;
        }

        /// <summary>
        /// Gets the attribute models for the requested attribute ids
        /// </summary>
        /// <param name="attributeIds">Array of attribute ids for which models are required</param>
        /// <param name="attributeModelContext">The data context for which models needs to be fetched</param>
        /// <returns>Collection of Attribute Model objects</returns>
        /// <exception cref="ArgumentNullException">Will be raised when the parameter 'attributeModelContext' is null</exception>
        /// <exception cref="ArgumentException">Will be raised when 'attributeIds' not provided</exception>
        public AttributeModelCollection GetByIds(Collection<Int32> attributeIds, AttributeModelContext attributeModelContext)
        {
            AttributeModelCollection attributeModels = null;

            try
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StartTraceActivity("AttributeModelBL.GetByIds", MDMTraceSource.AttributeModelGet, false);

                if (attributeIds == null || attributeIds.Count < 1)
                {
                    throw new ArgumentException("Attribute Models cannot be fetched. Provide Attribute Ids for which Attribute Models are required.");
                }

                //Validate AttributeModelContext
                ValidateAttributeModelContext(attributeModelContext);

                attributeModels = GetAttributeModels(attributeIds, null, null, attributeModelContext);
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("AttributeModelBL.GetByIds", MDMTraceSource.AttributeModelGet);
            }

            return attributeModels;
        }

        /// <summary>
        /// Gets the attribute models for the requested attribute group id
        /// </summary>
        /// <param name="attributeGroupId">Id of the attribute group for which model is required</param>
        /// <param name="excludeAttributeIds">Ids which needs to be excluded</param>
        /// <param name="attributeModelContext">The data context for which model needs to be fetched</param>
        /// <returns>Collection of Attribute Model objects</returns>
        /// <exception cref="ArgumentNullException">Will be raised when the parameter 'attributeModelContext' is null</exception>
        public AttributeModelCollection GetByGroupId(Int32 attributeGroupId, Collection<Int32> excludeAttributeIds, AttributeModelContext attributeModelContext)
        {
            return GetByGroupIds(new Collection<Int32> { attributeGroupId }, excludeAttributeIds, attributeModelContext);
        }

        /// <summary>
        /// Gets the attribute models for the requested attribute group ids
        /// </summary>
        /// <param name="attributeGroupIds">Ids of the attribute group for which models are required</param>
        /// <param name="excludeAttributeIds">Ids which needs to be excluded</param>
        /// <param name="attributeModelContext">The data context for which model needs to be fetched</param>
        /// <returns>Collection of Attribute Models object</returns>
        /// <exception cref="ArgumentNullException">Will be raised when the parameter 'attributeModelContext' is null</exception>
        /// <exception cref="ArgumentException">Will be raised when 'attributeGroupIds' not provided</exception>
        public AttributeModelCollection GetByGroupIds(Collection<Int32> attributeGroupIds, Collection<Int32> excludeAttributeIds, AttributeModelContext attributeModelContext)
        {
            AttributeModelCollection attributeModels = null;

            try
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StartTraceActivity("AttributeModelBL.GetByGroupIds", MDMTraceSource.AttributeModelGet, false);

                //Check whether attribute group ids are provided.. if yes get the attribute ids for these group ids
                if (attributeGroupIds == null || attributeGroupIds.Count < 1)
                {
                    throw new ArgumentException("Attribute Models cannot be fetched. Provide Attribute Group Ids for which Attribute Models are required.");
                }

                //Validate AttributeModelContext
                ValidateAttributeModelContext(attributeModelContext);

                attributeModels = GetAttributeModels(null, attributeGroupIds, excludeAttributeIds, attributeModelContext);

            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("AttributeModelBL.GetByGroupIds", MDMTraceSource.AttributeModelGet);
            }

            return attributeModels;
        }

        /// <summary>
        /// Gets the attribute models for the requested custom view id
        /// </summary>
        /// <param name="customViewId">Id of the custom view for which models are required</param>
        /// <param name="excludeAttributeIds">Ids which needs to be excluded</param>
        /// <param name="attributeModelContext">The data context for which model needs to be fetched</param>
        /// <returns>Collection of Attribute Models object</returns>
        /// <exception cref="ArgumentNullException">Will be raised when the parameter 'attributeModelContext' is null</exception>
        /// <exception cref="ArgumentException">Will be raised when 'customViewId' not provided</exception>
        public AttributeModelCollection GetByCustomViewId(Int32 customViewId, Collection<Int32> excludeAttributeIds, AttributeModelContext attributeModelContext)
        {
            AttributeModelCollection attributeModels = null;

            try
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StartTraceActivity("AttributeModelBL.GetByCustomViewId", MDMTraceSource.AttributeModelGet, false);

                if (customViewId < 1)
                {
                    throw new ArgumentException("Attribute Models cannot be fetched. Provide Custom View Id for which Attribute Models are required.");
                }

                ValidateAttributeModelContext(attributeModelContext);

                Collection<Int32> attributeIdList = this.GetAttributeIdList(null, customViewId, 0, attributeModelContext);

                attributeModels = GetAttributeModels(attributeIdList, null, excludeAttributeIds, attributeModelContext);
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("AttributeModelBL.GetByCustomViewId", MDMTraceSource.AttributeModelGet);
            }

            return attributeModels;
        }

        /// <summary>
        /// Gets the attribute models for the requested state view id
        /// </summary>
        /// <param name="stateViewId">Id of the state view for which models are required</param>
        /// <param name="excludeAttributeIds">Ids which needs to be excluded</param>
        /// <param name="attributeModelContext">The data context for which model needs to be fetched</param>
        /// <returns>Collection of Attribute Models object</returns>
        /// <exception cref="ArgumentNullException">Will be raised when the parameter 'attributeModelContext' is null</exception>
        /// <exception cref="ArgumentException">Will be raised when 'stateViewId' not provided</exception>
        public AttributeModelCollection GetByStateViewId(Int32 stateViewId, Collection<Int32> excludeAttributeIds, AttributeModelContext attributeModelContext)
        {
            AttributeModelCollection attributeModels = null;

            try
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StartTraceActivity("AttributeModelBL.GetByStateViewId", MDMTraceSource.AttributeModelGet, false);

                if (stateViewId < 1)
                {
                    throw new ArgumentException("Attribute Models cannot be fetched. Provide Custom View Id for which Attribute Models are required.");
                }

                ValidateAttributeModelContext(attributeModelContext);

                Collection<Int32> attributeIdList = this.GetAttributeIdList(null, 0, stateViewId, attributeModelContext);

                attributeModels = GetAttributeModels(attributeIdList, null, excludeAttributeIds, attributeModelContext);
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("AttributeModelBL.GetByStateViewId", MDMTraceSource.AttributeModelGet);

            }

            return attributeModels;
        }

        /// <summary>
        /// Gets the ids of attributes coming under requested attribute groups, custom views or state views 
        /// This method try to loads attribute ids directly from database. It would be recommended to use other overload method to gain performance.
        /// </summary>
        /// <param name="attributeGroupIds"> Comma separated list of attribute group ids</param>
        /// <param name="customViewId">Custom view id</param>
        /// <param name="stateViewId">State view id</param>
        /// <param name="attributeModelContext">The data context for which ids needs to be fetched</param>
        /// <returns>collection of attribute ids.</returns>
        /// <exception cref="ArgumentNullException">Will be raised when the parameter 'attributeModelContext' is null</exception>
        public Collection<Int32> GetAttributeIdList(Collection<Int32> attributeGroupIds, Int32 customViewId, Int32 stateViewId, AttributeModelContext attributeModelContext)
        {
            Collection<Int32> attributeIds = null;

            try
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StartTraceActivity("AttributeModelBL.GetAttributeIdList", MDMTraceSource.AttributeModelGet, false);

                AttributeModelMappingPropertiesCollection attributeModelMappingProperties = GetMappedAttributesForContext(attributeGroupIds, customViewId, stateViewId, attributeModelContext);

                if (attributeModelMappingProperties != null && attributeModelMappingProperties.Count > 0)
                {
                    attributeIds = attributeModelMappingProperties.GetAttributeIds();
                }
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("AttributeModelBL.GetAttributeIdList", MDMTraceSource.AttributeModelGet);
            }

            return attributeIds;
        }

        /// <summary>
        /// Gets the ids of attributes coming under requested attribute groups or based on attribute model context
        /// This method try to loads attribute ids from cache if its available.
        /// </summary>
        /// <param name="attributeGroupIds">collection of attribute group ids.</param>
        /// <param name="attributeModelContext">The data context for which model needs to be fetch.</param>
        /// <returns>collection of attribute ids.</returns>
        public Collection<Int32> GetAttributeIdList(Collection<Int32> attributeGroupIds, AttributeModelContext attributeModelContext)
        {
            Collection<Int32> attributeIds = null;

            try
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StartTraceActivity("AttributeModelBL.GetAttributeIdList", MDMTraceSource.AttributeModelGet, false);

                CallerContext callerContext = new CallerContext(MDMCenterApplication.PIM, MDMCenterModules.Entity);

                AttributeModelMappingPropertiesCollection attributeModelMappingProperties = GetAttributeModelMappingProperties(null, attributeGroupIds, attributeModelContext, false, callerContext);

                if (attributeModelMappingProperties != null && attributeModelMappingProperties.Count > 0)
                {
                    attributeIds = attributeModelMappingProperties.GetAttributeIds();
                }
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("AttributeModelBL.GetAttributeIdList", MDMTraceSource.AttributeModelGet);
            }

            return attributeIds;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public AttributeModelCollection GetBaseAttributeModelsByIds(Collection<Int32> ids)
        {
            AttributeModelCollection baseAttributeModels = new AttributeModelCollection();

            try
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StartTraceActivity("AttributeModelBL.GetAttributeModelsByIds", MDMTraceSource.AttributeModelGet, false);

                Dictionary<Int32, AttributeModelBaseProperties> baseAttributeModelBaseProperties = GetBaseAttributeModels();

                if (baseAttributeModelBaseProperties != null && baseAttributeModelBaseProperties.Count > 0)
                {
                    baseAttributeModels = new AttributeModelCollection(baseAttributeModelBaseProperties.Where(am => ids.Contains(am.Key)).Select(baseAttributeModelBaseProperty => new AttributeModel(baseAttributeModelBaseProperty.Value, null, null)).ToList());
                }

            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("AttributeModelBL.GetAttributeModelsByIds", MDMTraceSource.AttributeModelGet);
            }

            return baseAttributeModels;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public AttributeModelCollection GetAllBaseAttributeModels()
        {
            AttributeModelCollection baseAttributeModels = new AttributeModelCollection();

            try
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StartTraceActivity("AttributeModelBL.GetAllBaseAttributeModels", MDMTraceSource.AttributeModelGet, false);

                Dictionary<Int32, AttributeModelBaseProperties> baseAttributeModelBaseProperties = GetBaseAttributeModels();

                if (baseAttributeModelBaseProperties != null && baseAttributeModelBaseProperties.Count > 0)
                {
                    var enumerator = baseAttributeModelBaseProperties.GetEnumerator();

                    while (enumerator.MoveNext())
                    {
                        AttributeModel attributeModel = new AttributeModel(enumerator.Current.Value, null, null);
                        baseAttributeModels.Add(attributeModel, true);
                    }
                }
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("AttributeModelBL.GetAllBaseAttributeModels", MDMTraceSource.AttributeModelGet);
            }

            return baseAttributeModels;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public AttributeModelCollection GetAllBaseRelationshipAttributeModels()
        {
            AttributeModelCollection baseAttributeModels = new AttributeModelCollection();

            try
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StartTraceActivity("AttributeModelBL.GetAllBaseRelationshipAttributeModels", MDMTraceSource.AttributeModelGet, false);

                Dictionary<Int32, AttributeModelBaseProperties> baseAttributeModelBaseProperties = GetBaseAttributeModels();

                if (baseAttributeModelBaseProperties != null && baseAttributeModelBaseProperties.Count > 0)
                {
                    var enumerator = baseAttributeModelBaseProperties.GetEnumerator();

                    while (enumerator.MoveNext())
                    {
                        AttributeModelBaseProperties attrModelBaseProperties = enumerator.Current.Value;

                        if (attrModelBaseProperties.AttributeModelType == AttributeModelType.Relationship)
                        {
                            AttributeModel attributeModel = new AttributeModel(enumerator.Current.Value, null, null);
                            baseAttributeModels.Add(attributeModel, true);
                        }
                    }
                }
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("AttributeModelBL.GetAllBaseRelationshipAttributeModels", MDMTraceSource.AttributeModelGet);
            }

            return baseAttributeModels;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeIdList"></param>
        /// <returns></returns>
        public Collection<KeyValuePair<Int32, Collection<AttributeModelContext>>> GetWhereUsed(Collection<Int32> attributeIdList)
        {
            Collection<KeyValuePair<Int32, Collection<AttributeModelContext>>> result = null;

            if (attributeIdList != null && attributeIdList.Count > 0)
            {
                result = new AttributeModelDA().GetWhereUsed(attributeIdList);
            }

            return result;
        }

        /// <summary>
        /// Sort Attribute Models based on sort order recursively.If sort order value is 0 then it will be always last.
        /// Sort AttributeModels applies for child attribute model also if it has any.
        /// </summary>
        /// <param name="attributeModels">This parameter is specifying attribute models</param>
        /// <returns>sorted attribute model based on sort order</returns>
        public AttributeModelCollection SortAttributeModels(AttributeModelCollection attributeModels)
        {
            AttributeModelCollection sortedAttributeModels = new AttributeModelCollection();

            if (attributeModels != null && attributeModels.Count > 0)
            {
                //if sort order value is 0 then sort attribute models by long name.
                List<AttributeModel> negativeAndZeroethOrderAttributeModels = attributeModels.Where(a => a.SortOrder <= 0).OrderBy(a => a.LongName).ToList();

                //sort all attribute models by sort order then by attribute model long name.
                List<AttributeModel> positiveOrderAttributeModels = attributeModels.Where(a => a.SortOrder > 0).OrderBy(a => a.SortOrder).ThenBy(a => a.LongName).ToList();

                if (positiveOrderAttributeModels != null && positiveOrderAttributeModels.Count > 0)
                {
                    sortedAttributeModels = new AttributeModelCollection(positiveOrderAttributeModels);
                }

                if (negativeAndZeroethOrderAttributeModels != null && negativeAndZeroethOrderAttributeModels.Count > 0)
                {
                    foreach (AttributeModel attrModel in negativeAndZeroethOrderAttributeModels)
                    {
                        sortedAttributeModels.Add(attrModel, true);
                    }
                }

                foreach (AttributeModel attributeModel in attributeModels)
                {
                    //Apply sorting for child Attribute Models recursively.
                    AttributeModelCollection childAttribueModels = attributeModel.AttributeModels;

                    if (childAttribueModels != null && childAttribueModels.Count > 0)
                    {
                        attributeModel.AttributeModels = SortAttributeModels(childAttribueModels);
                    }
                }
            }

            return sortedAttributeModels;
        }

        /// <summary>
        /// Get all child dependent attribute models
        /// </summary>
        /// <param name="modelContext">Attribute model context which indicates what all attribute models to load.</param>
        /// <param name="callerContext">Context indicating the caller of the API</param>
        /// <returns>Dependent child attribute model</returns>
        public AttributeModelCollection GetAllDependentChildAttributeModels(AttributeModelContext modelContext, CallerContext callerContext)
        {
            AttributeModelCollection dependentChildAttributeModels = new AttributeModelCollection();
            ValidateAttributeModelContext(modelContext);

            //1. Get all attribute models
            AttributeModelCollection allModels = this.Get(modelContext);

            if (allModels != null && allModels.Count > 0)
            {
                //2. Get attribute models which are marked as IsDependent = true
                dependentChildAttributeModels = allModels.GetDependentChildAttributeModels();
            }

            return dependentChildAttributeModels;
        }

        /// <summary>
        /// Returns mapped attribute ids based on given container ids
        /// </summary>
        /// <param name="containerIdList">Indicates container id list to fetch attribute models</param>
        /// <param name="callerContext">Indicates caller context of the API</param>
        /// <returns></returns>
        public Collection<Int32> GetMappedAttributesIdsForContainers(Collection<Int32> containerIdList, CallerContext callerContext)
        {
            var diagnosticActivity = new DiagnosticActivity();
            Collection<Int32> attributeIdList = new Collection<Int32>();

            try
            {
                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.Start();
                }

                AttributeModelDA _attributeModelDA = new AttributeModelDA();

                attributeIdList = _attributeModelDA.GetMappedAttributesIdsForContainers(containerIdList);
            }
            finally
            {
                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.Stop();
                }
            }

            return attributeIdList;
        }

        /// <summary>
        /// Returns mapped attributes based on given container ids
        /// Note: Method returns base attribute model properties and will not be having property changes done at Container Mapping level.
        /// </summary>
        /// <param name="containerIdList">Indicates container id list to fetch attribute models</param>
        /// <returns></returns>
        public AttributeModelCollection GetMappedAttributeModelsForContainers(Collection<Int32> containerIdList, CallerContext callerContext)
        {
            var diagnosticActivity = new DiagnosticActivity();
            AttributeModelCollection attributeModels = new AttributeModelCollection();

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
                    throw new MDMOperationException("113960", _localeMessage.Message, "AttributeModelBL.GetMappedAttributeModelsByContainers", String.Empty, "Get");
                }

                #region Get attribute id list based on container from Database

                Collection<Int32> attributeIdList = GetMappedAttributesIdsForContainers(containerIdList, callerContext);

                #endregion Get attribute id list based on container from Database

                Dictionary<Int32, AttributeModelBaseProperties> baseAttributeModelBaseProperties = GetBaseAttributeModels();

                if (baseAttributeModelBaseProperties != null && baseAttributeModelBaseProperties.Count > 0)
                {
                    var enumerator = baseAttributeModelBaseProperties.GetEnumerator();

                    while (enumerator.MoveNext())
                    {
                        if (attributeIdList.Contains(enumerator.Current.Key))
                        {
                            AttributeModel attributeModel = new AttributeModel(enumerator.Current.Value, null, null);
                            attributeModels.Add(attributeModel, true);
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

            return attributeModels;
        }

        #endregion

        #region Process Methods

        /// <summary>
        /// Process attribute models
        /// </summary>
        /// <param name="attributeModelCollection"></param>
        /// <param name="attributeOperationResults"></param>
        /// <param name="programName"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        public AttributeOperationResultCollection ProcessAttributeModels(AttributeModelCollection attributeModelCollection, AttributeOperationResultCollection attributeOperationResults, String programName, CallerContext callerContext)
        {
            const String methodName = "process";

            try
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StartTraceActivity("AttributeModelManager.AttributeModel.ProcessAttributeModels", false);

                DBCommandProperties command = DBCommandHelper.Get(callerContext.Application, callerContext.Module, MDMCenterModuleAction.Update);

                #region Step : Prepare attribute model Operation Result Schema

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Prepare AttributeOperationResultCollection starting...", MDMTraceSource.AttributeModelGet);

                attributeOperationResults = PrepareAttributeOperationResultsSchema(attributeModelCollection);

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Prepare AttributeOperationResultCollection completed.", MDMTraceSource.AttributeModelGet);

                #endregion

                #region Step : Validate for uniqueness

                if (attributeModelCollection != null && attributeModelCollection.Count > 0)
                {
                    var attributeModelsWithOperationResults = new Collection<Tuple<AttributeModel, OperationResult>>();

                    foreach (var attributeModel in attributeModelCollection)
                    {
                        var operationResult = attributeOperationResults.GetAttributeOperationResult(attributeModel.Id, attributeModel.Locale);

                        attributeModelsWithOperationResults.Add(new Tuple<AttributeModel, OperationResult>(attributeModel, operationResult));

                        if (attributeModel.IsCollection)
                        {
                            ValidateComplexChildAttributes(attributeModelCollection, attributeModel, operationResult, callerContext);
                        }

                        ValidateExportMask(attributeModel, operationResult, callerContext);
                    }

                    ValidateForUniqueness(attributeModelsWithOperationResults, callerContext, methodName);

                    attributeOperationResults.RefreshOperationResultStatus();
                }

                #endregion

                if (!(attributeOperationResults.OperationResultStatus == OperationResultStatusEnum.CompletedWithErrors
                    || attributeOperationResults.OperationResultStatus == OperationResultStatusEnum.Failed))
                {
                    #region Step: Process attribute models

                    if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Sending AttributeModel DA Process request...", MDMTraceSource.AttributeModelGet);

                    String loginUser = _securityPrincipal.CurrentUserName;

                    new AttributeModelDA().ProcessAttributeModels(attributeModelCollection, (Int32)_systemDataLocale, loginUser, attributeOperationResults, programName, command);

                    #region activitylog

                    Dictionary<Int32, AttributeModelBaseProperties> originalBaseAttributeModels = GetBaseAttributeModels();

                    if (AppConfigurationHelper.GetAppConfig<Boolean>(Constants.ACTIVITYLOG_DATAMODEL_ENABLED_APPCONFIG_KEY, false))
                    {
                        if (callerContext.ProgramName.IsNullOrEmpty())
                        {
                            callerContext.ProgramName = "AttributeModelBL";
                        }
                        LogDataModelChanges(attributeModelCollection, callerContext, originalBaseAttributeModels);
                    }

                    #endregion activity log

                    if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Receiving AttributeModel DA Process response.", MDMTraceSource.AttributeModelGet);

                    #endregion

                    #region Cache Invalidation

                    Collection<Int32> attributeIds = new Collection<Int32>();
                    Collection<LocaleEnum> locales = new Collection<LocaleEnum>();
                    Collection<Int32> attrIdsForLookupInvalidation = new Collection<Int32>();
                    Boolean isMappingCacheDirty = false;
                    Boolean isSystemAttributesCacheDirty = false;

                    Collection<Int32> entityCacheReloadAttributeIds = new Collection<Int32>();

                    locales.Add(_securityPrincipal.UserPreferences.DataLocale);

                    foreach (AttributeModel attributeModel in attributeModelCollection)
                    {
                        Int32 attributeId = attributeModel.Id;

                        if (!locales.Contains(attributeModel.Locale))
                        {
                            locales.Add(attributeModel.Locale);
                        }

                        if (attributeModel.Action != ObjectAction.Create && attributeModel.Action != ObjectAction.Read)
                        {
                            if (!attributeIds.Contains(attributeId))
                            {
                                attributeIds.Add(attributeId);
                            }

                            if (attributeModel.IsLookup && !attrIdsForLookupInvalidation.Contains(attributeId))
                            {
                                attrIdsForLookupInvalidation.Add(attributeId);
                            }
                        }

                        if (attributeModel.AttributeModelType == AttributeModelType.System || attributeModel.AttributeModelType == AttributeModelType.MetaDataAttribute)
                        {
                            isSystemAttributesCacheDirty = true;
                        }

                        if (originalBaseAttributeModels != null && originalBaseAttributeModels.Count > 0 && !isMappingCacheDirty)
                        {
                            AttributeModelBaseProperties originalBaseAttributeModel = AttributeModelUtility.GetBaseAttributeModelByKey(originalBaseAttributeModels, attributeId);

                            if (originalBaseAttributeModel != null)
                            {
                                if (!originalBaseAttributeModel.Name.Equals(attributeModel.Name))
                                    isMappingCacheDirty = true;
                                else if (!originalBaseAttributeModel.LongName.Equals(attributeModel.LongName))
                                    isMappingCacheDirty = true;
                                else if (!originalBaseAttributeModel.AttributeParentName.Equals(attributeModel.AttributeParentName))
                                    isMappingCacheDirty = true;
                                else if (!originalBaseAttributeModel.AttributeParentLongName.Equals(attributeModel.AttributeParentLongName))
                                    isMappingCacheDirty = true;

                                if (!originalBaseAttributeModel.Inheritable.Equals(attributeModel.Inheritable) &&
                                    attributeModel.Action == ObjectAction.Update)
                                {
                                    entityCacheReloadAttributeIds.Add(attributeModel.Id);
                                }
                            }
                        }
                    }

                    if (entityCacheReloadAttributeIds != null && entityCacheReloadAttributeIds.Count > 0)
                    {
                        ProcessEntityCacheLoadContextForAttributeModelBasePropertyChange(entityCacheReloadAttributeIds, callerContext);
                    }

                    if (isSystemAttributesCacheDirty)
                    {
                        MappingBufferManager mappingBufferManager = new MappingBufferManager();
                        mappingBufferManager.RemoveSystemAndMetaDataAttributeModelMappings();
                    }

                    if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Invalidate Cache for AttributeModel(s) started...", MDMTraceSource.AttributeModelGet);

                    //Remove attribute master dictionary too..
                    AttributeModelBufferManager.RemoveBaseAttributeModels();

                    if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Invalidate Cache for AttributeModel(s) completed.", MDMTraceSource.AttributeModelGet);

                    if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Invalidate Cache for InvalidateImpactedAttributeModels AttributeModel(s) started...", MDMTraceSource.AttributeModelGet);

                    Task.Factory.StartNew(() => { InvalidateImpactedAttributesAsync(attributeIds, locales, attrIdsForLookupInvalidation, isMappingCacheDirty); });

                    if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Invalidate Cache for InvalidateImpactedAttributeModels AttributeModel(s) completed.", MDMTraceSource.AttributeModelGet);

                    #endregion
                }
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("AttributeModelManager.AttributeModel.ProcessAttributeModels", MDMTraceSource.AttributeModelGet);
            }

            return attributeOperationResults;
        }

        #endregion

        #region IDataModelManager Methods

        /// <summary>
        /// Generate and OperationResult Schema for given DataModelObjectCollection
        /// </summary>
        /// <param name="iDataModelObjects"></param>
        /// <param name="operationResults"></param>
        public void PrepareOperationResultsSchema(IDataModelObjectCollection iDataModelObjects, DataModelOperationResultCollection operationResults)
        {
            AttributeModelCollection attributeModels = iDataModelObjects as AttributeModelCollection;

            if (attributeModels != null && attributeModels.Count > 0)
            {
                if (operationResults.Count > 0)
                {
                    operationResults.Clear();
                }

                Int32 attributeModelToBeCreated = -1;

                foreach (AttributeModel attributeModel in attributeModels)
                {
                    DataModelOperationResult attributeModelOperationResult = new DataModelOperationResult(attributeModel.Id, attributeModel.LongName, attributeModel.ExternalId, attributeModel.ReferenceId);

                    if (String.IsNullOrEmpty(attributeModelOperationResult.ExternalId))
                    {
                        attributeModelOperationResult.ExternalId = attributeModel.Name;
                    }

                    if (attributeModel.Id < 1)
                    {
                        attributeModel.Id = attributeModelToBeCreated;
                        attributeModelOperationResult.Id = attributeModelToBeCreated;
                        attributeModelToBeCreated--;
                    }

                    operationResults.Add(attributeModelOperationResult);

                    if (attributeModel.IsComplex)
                    {
                        AttributeModelCollection childAttributeModels = (AttributeModelCollection)attributeModel.GetChildAttributeModels();

                        if (childAttributeModels != null && childAttributeModels.Count > 0)
                        {
                            PrepareOperationResultsSchema(childAttributeModels, operationResults);
                        }
                    }
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
            ValidateInputParameters(iDataModelObjects as AttributeModelCollection, operationResults, iCallerContext as CallerContext);
        }

        /// <summary>
        /// Load original data model objects
        /// </summary>
        /// <param name="iDataModelObjects">Collection of data model objects to load original object.</param>
        /// <param name="operationResult">Collection of DataModel OperationResult</param>
        /// <param name="iCallerContext">Context of caller making call to this API.</param>
        public void LoadOriginal(IDataModelObjectCollection iDataModelObjects, DataModelOperationResultCollection operationResult, ICallerContext iCallerContext)
        {
            AttributeModelCollection attributeModels = iDataModelObjects as AttributeModelCollection;

            if (attributeModels != null)
            {
                LoadOriginalAttributeModels(attributeModels, iCallerContext as CallerContext);
            }
        }

        /// <summary>
        /// Fills missing information to data model objects.
        /// </summary>
        /// <param name="iDataModelObjects">Collection of data model objects to be filled up.</param>
        /// <param name="iCallerContext">Context of caller making call to this API.</param>
        public void FillDataModel(IDataModelObjectCollection iDataModelObjects, ICallerContext iCallerContext)
        {
            FillAttributeModels(iDataModelObjects as AttributeModelCollection, iCallerContext as CallerContext);
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
            CompareAndMerge(iDataModelObjects as AttributeModelCollection, operationResults, iCallerContext as CallerContext);
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
            AttributeModelCollection attributeModels = iDataModelObjects as AttributeModelCollection;
            CallerContext callerContext = (CallerContext)iCallerContext;

            if (attributeModels != null && attributeModels.Count > 0)
            {
                String userName = _securityPrincipal.CurrentUserName;
                DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Execute);

                #region Perform attribute model updates in database

                using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = Constants.DEFAULT_ISOLATION_LEVEL }))
                {
                    new AttributeModelDA().Process(attributeModels, (Int32)_systemDataLocale, userName, operationResults, callerContext.ProgramName, command);
                    transactionScope.Complete();

                    #region activity log

                    if (AppConfigurationHelper.GetAppConfig<Boolean>(Constants.ACTIVITYLOG_DATAMODEL_ENABLED_APPCONFIG_KEY, false))
                    {
                        if (callerContext.ProgramName.IsNullOrEmpty())
                        {
                            callerContext.ProgramName = "AttributeModelBL";
                        }
                        LogDataModelChanges(attributeModels, callerContext);
                    }

                    #endregion activity log
                }

                GenerateComplexAttributeSchema(attributeModels, operationResults, callerContext);

                LocalizeErrors(operationResults, callerContext);

                #endregion
            }
        }

        /// <summary>
        /// Processes the Entity cache statuses for data model objects
        /// </summary>
        /// <param name="iDataModelObjects">Collection of data model objects to be process entity cache load.</param>
        /// <param name="operationResults">Collection of DataModel OperationResult</param>
        /// <param name="iCallerContext">Context of caller making call to this API.</param>
        public void InvalidateCache(IDataModelObjectCollection iDataModelObjects, DataModelOperationResultCollection operationResults, ICallerContext iCallerContext)
        {
            #region Cache Invalidation

            AttributeModelCollection attributeModels = iDataModelObjects as AttributeModelCollection;
            Collection<Int32> attributeIds = new Collection<Int32>();
            Collection<LocaleEnum> locales = new Collection<LocaleEnum>();
            Collection<Int32> attrIdsForLookupInvalidation = new Collection<Int32>();
            Boolean isMappingCacheDirty = false;

            locales.Add(_securityPrincipal.UserPreferences.DataLocale);

            if (attributeModels == null)
            {
                return;
            }

            foreach (AttributeModel attributeModel in attributeModels)
            {
                Int32 attributeId = attributeModel.Id;

                if (!locales.Contains(attributeModel.Locale))
                {
                    locales.Add(attributeModel.Locale);
                }

                if (attributeModel.Action != ObjectAction.Create && attributeModel.Action != ObjectAction.Read && attributeModel.Action != ObjectAction.Ignore)
                {
                    if (!attributeIds.Contains(attributeId))
                    {
                        attributeIds.Add(attributeId);
                    }

                    if (attributeModel.IsLookup && !attrIdsForLookupInvalidation.Contains(attributeId))
                    {
                        attrIdsForLookupInvalidation.Add(attributeId);
                    }
                }

                if (!isMappingCacheDirty)
                {
                    if (attributeModel.OriginalAttributeModel != null)
                    {
                        if (!attributeModel.OriginalAttributeModel.Name.Equals(attributeModel.Name))
                            isMappingCacheDirty = true;
                        else if (!attributeModel.OriginalAttributeModel.LongName.Equals(attributeModel.LongName))
                            isMappingCacheDirty = true;
                        else if (!attributeModel.OriginalAttributeModel.AttributeParentName.Equals(attributeModel.AttributeParentName))
                            isMappingCacheDirty = true;
                        else if (!attributeModel.OriginalAttributeModel.AttributeParentLongName.Equals(attributeModel.AttributeParentLongName))
                            isMappingCacheDirty = true;
                    }
                }
            }

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Invalidate Cache for AttributeModel(s) started...", MDMTraceSource.AttributeModelGet);

            //Remove attribute master dictionary too..
            AttributeModelBufferManager.RemoveBaseAttributeModels();

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Invalidate Cache for AttributeModel(s) completed.", MDMTraceSource.AttributeModelGet);

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Invalidate Cache for InvalidateImpactedAttributeModels AttributeModel(s) started...", MDMTraceSource.AttributeModelGet);

            _operationContext = SM.OperationContext.Current;

            Task.Factory.StartNew(() => { InvalidateImpactedAttributesAsync(attributeIds, locales, attrIdsForLookupInvalidation, isMappingCacheDirty); });

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Invalidate Cache for InvalidateImpactedAttributeModels AttributeModel(s) completed.", MDMTraceSource.AttributeModelGet);

            #endregion
        }

        #endregion

        #region Utility Methods

        /// <summary>
        /// Populates the attribute ids for given attributes based on their names.
        /// </summary>
        /// <param name="attributes">The attributes.</param>
        public void PopulateAttributeIdsByNames(ICollection<Attribute> attributes)
        {
            if (attributes != null && attributes.Count > 0)
            {
                Dictionary<String, Int32> attributeNameToAttributeIdMaps = GetAttributeNameToAttributeIdMaps();

                if (attributeNameToAttributeIdMaps != null && attributeNameToAttributeIdMaps.Count > 0)
                {
                    foreach (Attribute attribute in attributes.Where(attr => attr.Id < 1))
                    {
                        String key = GetKey(attribute.Name);

                        if (attributeNameToAttributeIdMaps.ContainsKey(key))
                        {
                            Int32 attributeId;

                            attributeNameToAttributeIdMaps.TryGetValue(key, out attributeId);

                            if (attributeId > 0)
                            {
                                attribute.Id = attributeId;
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #endregion

        #region Private Methods

        #region Attribute Dependency Methods

        /// <summary>
        /// This will load the dependent attribute details for requested attribute model. 
        /// </summary>
        /// <param name="attributeModel">Indicates the Attribute model</param>
        /// <param name="applicationContext">Indicates the Application Context</param>
        /// <param name="callerContext">Indicates the caller context</param>
        /// <param name="depedentAttributeModelsDictionary"></param>
        /// <param name="isCollectionAttributeDependencyEnabled">Indicates the collection attribute dependency is enabled.</param>
        private void LoadDependentAttributeMappings(AttributeModel attributeModel, ApplicationContext applicationContext, CallerContext callerContext, Dictionary<Int32, AttributeModel> depedentAttributeModelsDictionary, Boolean isCollectionAttributeDependencyEnabled = false)
        {
            if (attributeModel.IsCollection && !isCollectionAttributeDependencyEnabled)
            {
                return;
            }

            if (attributeModel != null && (attributeModel.IsDependentAttribute || attributeModel.HasDependentAttribute))
            {
                Int32 attributeId = attributeModel.Id;
                AttributeModel attrModel = null;

                if (depedentAttributeModelsDictionary != null && depedentAttributeModelsDictionary.ContainsKey(attributeId))
                {
                    attrModel = depedentAttributeModelsDictionary[attributeId];
                }

                if (attrModel == null)
                {
                    applicationContext.Locale = attributeModel.Locale.ToString();
                    applicationContext.AttributeId = attributeId;

                    DependentAttributeCollection dependentAttributes = new DependentAttributeCollection();
                    AttributeDependencyBL attributeDependencyBL = new AttributeDependencyBL();

                    dependentAttributes = attributeDependencyBL.GetDependencyDetails(attributeModel.Id, applicationContext, true, callerContext);

                    if (dependentAttributes != null && dependentAttributes.Count > 0)
                    {
                        attributeModel.DependentChildAttributes = dependentAttributes.FilterDependentAttributes(DependencyType.Child);
                        attributeModel.DependentParentAttributes = dependentAttributes.FilterDependentAttributes(DependencyType.Parent);

                        if (depedentAttributeModelsDictionary != null && !depedentAttributeModelsDictionary.ContainsKey(attributeId))
                        {
                            depedentAttributeModelsDictionary.Add(attributeId, attributeModel);
                        }
                    }
                }
                else
                {
                    attributeModel.DependentChildAttributes = attrModel.DependentChildAttributes;
                    attributeModel.DependentParentAttributes = attrModel.DependentParentAttributes;
                }
            }
        }

        #endregion

        #region Attribute Dependency Caching

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeModel"></param>
        /// <param name="applicationContext"></param>
        /// <param name="callerContext"></param>
        /// <param name="isCollectionAttributeDependencyEnabled"></param>
        private void LoadDependentAttributeMappingsUsingCache(AttributeModel attributeModel, ApplicationContext applicationContext, CallerContext callerContext, Boolean isCollectionAttributeDependencyEnabled = false)
        {
            if (attributeModel != null && attributeModel.IsCollection && !isCollectionAttributeDependencyEnabled)
            {
                return;
            }

            if (attributeModel != null && (attributeModel.IsDependentAttribute || attributeModel.HasDependentAttribute))
            {
                Int32 attributeId = attributeModel.Id;
                String attributeModelKey = CacheKeyGenerator.GetAttributeModelDependencyCacheKey(applicationContext, attributeId);

                AttributeModel attrModel = null;
                Dictionary<String, AttributeModel> attributeDependencies = AttributeModelBufferManager.FindAttributeModelDependencies();
                if (attributeDependencies != null)
                {
                    attributeDependencies.TryGetValue(attributeModelKey, out attrModel);
                }

                if (attrModel == null)
                {
                    applicationContext.Locale = attributeModel.Locale.ToString();
                    applicationContext.AttributeId = attributeId;

                    DependentAttributeCollection dependentAttributes = new DependentAttributeCollection();
                    AttributeDependencyBL attributeDependencyBL = new AttributeDependencyBL();

                    dependentAttributes = attributeDependencyBL.GetDependencyDetails(attributeModel.Id, applicationContext, true, callerContext);

                    if (dependentAttributes != null && dependentAttributes.Count > 0)
                    {
                        attributeModel.DependentChildAttributes = dependentAttributes.FilterDependentAttributes(DependencyType.Child);
                        attributeModel.DependentParentAttributes = dependentAttributes.FilterDependentAttributes(DependencyType.Parent);

                        UpdateAttributeModelDependencyInCache(attributeDependencies, attributeModel, attributeModelKey);
                    }
                }
                else
                {
                    attributeModel.DependentChildAttributes = attrModel.DependentChildAttributes;
                    attributeModel.DependentParentAttributes = attrModel.DependentParentAttributes;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeModelDependencies"></param>
        /// <param name="attributeModel"></param>
        /// <param name="attributeModelKey"></param>
        private void UpdateAttributeModelDependencyInCache(Dictionary<String, AttributeModel> attributeModelDependencies, AttributeModel attributeModel, String attributeModelKey)
        {
            lock (_lockObj)
            {
                if (attributeModelDependencies == null)
                {
                    // Check in cache again, if some other thread created the instance
                    attributeModelDependencies = AttributeModelBufferManager.FindAttributeModelDependencies();
                    if (attributeModelDependencies == null)
                    {
                        // If instance is still not available, create it and add to cache
                        attributeModelDependencies = new Dictionary<String, AttributeModel>();
                        attributeModelDependencies.Add(attributeModelKey, attributeModel);
                        AttributeModelBufferManager.UpdateAttributeModelDependencies(attributeModelDependencies, 3, false);
                    }
                    else if (!attributeModelDependencies.ContainsKey(attributeModelKey))
                    {
                        // If instance is available, update the dictionary if attribute is not present
                        attributeModelDependencies.Add(attributeModelKey, attributeModel);
                    }
                }
                else if (!attributeModelDependencies.ContainsKey(attributeModelKey))
                {
                    // If instance is available, update the dictionary if attribute is not present
                    attributeModelDependencies.Add(attributeModelKey, attributeModel);
                }
            }
        }

        #endregion Attribute Dependency Caching

        #region Helper Methods

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
        /// Returns Message Object based on message code
        /// </summary>
        /// <param name="messageCode">Message Code</param>
        /// <param name="callerContext">caller context indicating who called the API</param>
        /// <returns></returns>
        private LocaleMessage GetSystemLocaleMessage(String messageCode, CallerContext callerContext)
        {
            LocaleMessageBL localeMessageBL = new LocaleMessageBL();

            return localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), messageCode, false, callerContext);
        }

        /// <summary>
        /// Prepares an attribute operation result collection
        /// </summary>
        /// <param name="attributeModels">collection of attribute models</param>
        /// <returns></returns>
        private AttributeOperationResultCollection PrepareAttributeOperationResultsSchema(AttributeModelCollection attributeModels)
        {
            AttributeOperationResultCollection attributeOperationResults = new AttributeOperationResultCollection();
            Int32 attributeIdToBeCreated = -1;

            foreach (AttributeModel attributeModel in attributeModels)
            {
                AttributeOperationResult attributeOperationResult = new AttributeOperationResult();

                if (attributeModel.Id < 1)
                {
                    attributeModel.Id = attributeIdToBeCreated;
                    attributeOperationResult.AttributeId = attributeIdToBeCreated;
                    attributeIdToBeCreated--;
                }
                else
                {
                    attributeOperationResult.AttributeId = attributeModel.Id;
                }

                attributeOperationResult.AttributeLongName = attributeModel.LongName;
                attributeOperationResult.Locale = attributeModel.Locale;

                attributeOperationResults.Add(attributeOperationResult);

                if (attributeModel.IsComplex == true)
                {
                    if (attributeModel.GetChildAttributeModels() != null)
                    {
                        AttributeModelCollection ComplexChildren = (AttributeModelCollection)attributeModel.GetChildAttributeModels();// check with return

                        foreach (AttributeModel complexChild in ComplexChildren)
                        {
                            AttributeOperationResult childAttributeOperationResult = new AttributeOperationResult();

                            if (complexChild.Id < 1)
                            {
                                complexChild.Id = attributeIdToBeCreated;
                                childAttributeOperationResult.AttributeId = attributeIdToBeCreated;
                                attributeIdToBeCreated--;
                            }
                            else
                            {
                                childAttributeOperationResult.AttributeId = complexChild.Id;
                            }

                            childAttributeOperationResult.AttributeLongName = complexChild.LongName;

                            attributeOperationResults.Add(childAttributeOperationResult);
                        }
                    }
                }
            }

            return attributeOperationResults;
        }

        /// <summary>
        /// Async method to call _attributeModelBufferManager.InvalidateImpactedAttributeModels
        /// </summary>
        /// <param name="attributeIds"></param>
        /// <param name="locales"></param>
        /// <param name="attrIdsForLookupInvalidation"></param>
        /// <param name="isMappingCacheDirty"></param>
        private void InvalidateImpactedAttributesAsync(Collection<Int32> attributeIds, Collection<LocaleEnum> locales, Collection<Int32> attrIdsForLookupInvalidation, Boolean isMappingCacheDirty)
        {
            try
            {
                SM.OperationContext.Current = _operationContext;

                AttributeModelBufferManager.RemoveAttributeModelLocaleProperties(locales);

                //Remove impacted contextual attribute models..(models for common mappings, tech mappings, etc)
                AttributeModelBufferManager.InvalidateImpactedAttributeModels(attributeIds, new AttributeModelBL(), isMappingCacheDirty, true);

                #region Invalidate lookup cache if it is a lookup attribute

                if (attrIdsForLookupInvalidation != null && attrIdsForLookupInvalidation.Count > 0)
                {
                    LookupBufferManager lookupBufferManager = new LookupBufferManager();
                    LocaleBL localeManager = new LocaleBL();

                    LocaleCollection allowableDataLocales = localeManager.GetAvailableLocales();

                    foreach (Int32 attributeId in attrIdsForLookupInvalidation)
                    {
                        foreach (Locale locale in allowableDataLocales)
                        {
                            try
                            {
                                lookupBufferManager.RemoveLookup(attributeId, locale.Locale, true, true);
                            }
                            catch (Exception ex)
                            {
                                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, ex.ToString(), MDMTraceSource.AttributeModelGet);
                            }
                        }
                    }
                }

                #endregion
            }
            catch (Exception ex)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, "Error Invalidating impacted attributes. : AttributeModelManager.AttributeModel.InvalidateImpactedAttributesAsync : Ex.ToString() : " + ex.ToString(), MDMTraceSource.AttributeModelGet);
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
                        _localeMessage = LocaleMessageManager.Get(_systemUILocale, error.ErrorCode, false, callerContext);

                        if (_localeMessage != null)
                        {
                            error.ErrorMessage = _localeMessage.Message;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Populate ids by name.
        /// </summary>
        /// <param name="attributeModelContext">The data context for which models needs to be fetched</param>
        /// <param name="iEntityModelManager">Specifies the defination for an entity model manager</param>
        /// <param name="callerContext">Specifies caller context</param>
        private void PopulateIdsByName(AttributeModelContext attributeModelContext, IEntityModelManager iEntityModelManager, CallerContext callerContext)
        {
            EntityModelContext entityModelContext = new EntityModelContext();

            if (attributeModelContext.ContainerId < 1 && !String.IsNullOrEmpty(attributeModelContext.ContainerName))
            {
                entityModelContext.ContainerName = attributeModelContext.ContainerName;
            }

            if (attributeModelContext.EntityTypeId < 1 && !String.IsNullOrEmpty(attributeModelContext.EntityTypeName))
            {
                entityModelContext.EntityTypeName = attributeModelContext.EntityTypeName;
            }

            if (attributeModelContext.CategoryId < 1 && !String.IsNullOrEmpty(attributeModelContext.CategoryPath))
            {
                entityModelContext.CategoryPath = attributeModelContext.CategoryPath;
            }

            if (attributeModelContext.RelationshipTypeId < 1 && !String.IsNullOrEmpty(attributeModelContext.RelationshipTypeName))
            {
                Collection<String> relationShipTypeNames = new Collection<String>();
                relationShipTypeNames.Add(attributeModelContext.RelationshipTypeName);

                entityModelContext.RelationshipTypeNames = relationShipTypeNames;
            }

            if (!String.IsNullOrEmpty(entityModelContext.ContainerName) || !String.IsNullOrEmpty(entityModelContext.EntityTypeName) || !String.IsNullOrEmpty(entityModelContext.CategoryPath) || entityModelContext.RelationshipTypeNames != null)
            {
                iEntityModelManager.FillEntityModelContextByName(ref entityModelContext, callerContext);

                if (entityModelContext.ContainerId > 0)
                    attributeModelContext.ContainerId = entityModelContext.ContainerId;
                if (entityModelContext.EntityTypeId > 0)
                    attributeModelContext.EntityTypeId = entityModelContext.EntityTypeId;
                if (entityModelContext.CategoryId > 0)
                    attributeModelContext.CategoryId = entityModelContext.CategoryId;
                if (entityModelContext.RelationshipTypeIds != null)
                    attributeModelContext.RelationshipTypeId = entityModelContext.RelationshipTypeIds.FirstOrDefault();
            }
        }

        #endregion

        #region Get Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeUniqueIdentifiers"></param>
        /// <param name="attributeModelContext"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        private AttributeModelCollection GetAttributeModelsByUniqueIdentifiers(AttributeUniqueIdentifierCollection attributeUniqueIdentifiers, AttributeModelContext attributeModelContext, CallerContext callerContext)
        {
            #region Parameter Validation

            ValidateCallerContext(callerContext, "GetAttributeModelsByUniqueIdentifiers");
            ValidateAttributeUniqueIdentifierCollection(attributeUniqueIdentifiers, "GetAttributeModelsByUniqueIdentifiers", callerContext);
            ValidateAttributeModelContext(attributeModelContext);

            #endregion

            AttributeModelCollection attributeModels = null;

            try
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StartTraceActivity("AttributeModelBL.GetByUniqueIdentifiers", MDMTraceSource.AttributeModelGet, false);

                if (attributeModelContext != null)
                {
                    Collection<Int32> attributeIds = GetAttributeIdList(attributeUniqueIdentifiers);

                    if (attributeIds != null && attributeIds.Count > 0)
                    {
                        attributeModels = GetAttributeModels(attributeIds, null, null, attributeModelContext, callerContext.Application, callerContext.Module);
                    }
                }
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("AttributeModelBL.GetByUniqueIdentifiers", MDMTraceSource.AttributeModelGet);
            }

            return attributeModels;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeIds"></param>
        /// <param name="attributeGroupIds"></param>
        /// <param name="excludeAttributeIdCollection"></param>
        /// <param name="attributeModelContext"></param>
        /// <param name="application"></param>
        /// <param name="module"></param>
        /// <returns></returns>
        private AttributeModelCollection GetAttributeModels(Collection<Int32> attributeIds, Collection<Int32> attributeGroupIds, Collection<Int32> excludeAttributeIdCollection, AttributeModelContext attributeModelContext, MDMCenterApplication application = MDMCenterApplication.PIM, MDMCenterModules module = MDMCenterModules.UIProcess)
        {
            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Get Attribute Models started...", MDMTraceSource.AttributeModelGet);
                Int32 numberOfAttributeIds = (attributeIds != null) ? attributeIds.Count : 0;
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0} attribute(s) requested for Attribute Model get. ", numberOfAttributeIds), MDMTraceSource.AttributeModelGet);
            }

            AttributeModelCollection attributeModels = new AttributeModelCollection();

            #region Validation

            var attributeContextLocales = attributeModelContext.Locales;

            if (attributeContextLocales == null || attributeContextLocales.Count < 1)
            {
                return attributeModels;
            }

            #endregion

            #region Initial Setup

            DurationHelper durationHelper = new DurationHelper(DateTime.Now);
            DurationHelper completeAttributeModelDurationHelper = new DurationHelper(DateTime.Now);

            CallerContext callerContext = new CallerContext(application, module);
            AttributeModelMappingPropertiesCollection attributeModelMappingProperties = new AttributeModelMappingPropertiesCollection();

            //If excludeAttributeIds is null, prepare a dummy object for the further logic to work..
            if (excludeAttributeIdCollection == null)
                excludeAttributeIdCollection = new Collection<Int32>();

            Boolean isAttributeDependencyEnabled = AppConfigurationHelper.GetAppConfig<Boolean>("MDMCenter.AttributeDependency.Enabled", false);
            Boolean isCollectionAttributeDependencyEnabled = false;
            Boolean isAttributeDependencyCacheEnabled = false;
            ApplicationContext applicationContext = new ApplicationContext(0, attributeModelContext.ContainerId, attributeModelContext.EntityTypeId, attributeModelContext.RelationshipTypeId, attributeModelContext.CategoryId, attributeModelContext.EntityId, -1, String.Empty, _securityPrincipal.CurrentUserId, -1);

            if (attributeModelContext.ApplyAttributeDependency)
            {
                isAttributeDependencyEnabled = AppConfigurationHelper.GetAppConfig<Boolean>("MDMCenter.AttributeDependency.Enabled", false);
                isCollectionAttributeDependencyEnabled = AppConfigurationHelper.GetAppConfig<Boolean>("MDMCenter.AttributeDependency.CollectionAttribute.Enabled", false);

                if (isAttributeDependencyEnabled)
                {
                    isAttributeDependencyCacheEnabled = AppConfigurationHelper.GetAppConfig<Boolean>("MDMCenter.AttributeModelManager.AttributeDependencyCache.Enabled", false);
                }
            }

            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("MDMCenter.AttributeDependency.Enabled value set as {0}", isAttributeDependencyEnabled), MDMTraceSource.AttributeModelGet);
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("MDMCenter.AttributeDependency.CollectionAttribute.Enabled value set as {0}", isAttributeDependencyEnabled), MDMTraceSource.AttributeModelGet);
            }

            if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - taken for initial setup and config reads.", durationHelper.GetDurationInMilliseconds(DateTime.Now)), MDMTraceSource.AttributeModelGet);

            #endregion

            #region Get Mapping based Properties

            attributeModelMappingProperties = GetAttributeModelMappingProperties(attributeIds, attributeGroupIds, attributeModelContext, false, callerContext);

            #endregion

            if (attributeModelMappingProperties != null && attributeModelMappingProperties.Count > 0)
            {
                #region Initialization of Data Security

                DataSecurityBL dataSecurityBL = null;
                PermissionContext permissionContext = null;
                Boolean applySecurity = attributeModelContext.ApplySecurity;
                Boolean loadAttributePermission = attributeModelContext.LoadPermissions;

                if (applySecurity || loadAttributePermission)
                {
                    //Get the permissions for each attribute model and add to the collection only if attribute is having permissions..
                    //Prepare permission context.. Keep RoleId as '0' and pass the current user Id.. The Load logic determines the permissions by considering all roles of the current user.
                    permissionContext = new PermissionContext(0, attributeModelContext.ContainerId, attributeModelContext.CategoryId, attributeModelContext.EntityTypeId, attributeModelContext.RelationshipTypeId, 0, 0, 0, _securityPrincipal.CurrentUserId, 0);
                    dataSecurityBL = new DataSecurityBL(_securityPrincipal, permissionContext);

                    if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - taken for initialization of permission context and data security.", durationHelper.GetDurationInMilliseconds(DateTime.Now)), MDMTraceSource.AttributeModelGet);
                }

                #endregion

                #region Get Base Properties

                Dictionary<Int32, AttributeModelBaseProperties> baseAttributeModels = GetBaseAttributeModels();

                if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - taken to load base attribute models.Total base attribute models loaded are: {1}", durationHelper.GetDurationInMilliseconds(DateTime.Now), baseAttributeModels.Count), MDMTraceSource.AttributeModelGet);

                #endregion

                #region Get AttributeModel Locale Properties

                Dictionary<String, AttributeModelLocaleProperties> attributeModelLocaleProperties = GetAttributeModelLocaleProperties(attributeContextLocales);

                if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - taken to load base attribute models. Total locale based attribute models loaded are: {1}", durationHelper.GetDurationInMilliseconds(DateTime.Now), attributeModelLocaleProperties.Count), MDMTraceSource.AttributeModelGet);

                #endregion

                #region Prepare AttributeModels

                AttributeModel attributeModel = null;
                AttributeModelBaseProperties baseAttributeModel = null;
                AttributeModelLocaleProperties attrModelLocaleProperties = null;
                Permission permission = null;
                Boolean isAttributeHavingPermissions = false;
                Dictionary<Int32, AttributeModel> depedentAttributeModelsDictionary = null;

                foreach (AttributeModelMappingProperties attrModelMappingProperties in attributeModelMappingProperties)
                {
                    if (!excludeAttributeIdCollection.Contains(attrModelMappingProperties.Id))
                    {
                        isAttributeHavingPermissions = false;
                        depedentAttributeModelsDictionary = new Dictionary<Int32, AttributeModel>();

                        baseAttributeModel = AttributeModelUtility.GetBaseAttributeModelByKey(baseAttributeModels, attrModelMappingProperties.Id);

                        if (baseAttributeModel != null)
                        {
                            #region Determine Data Security

                            if (applySecurity || loadAttributePermission)
                            {
                                AttributeModelType attributeModelType = baseAttributeModel.AttributeModelType;

                                //Skipping Permission checks in case of Relationship attributes,Attribute Group and System attributes 
                                //as there is no permission at relationship attribute,Attribute group and System attributes level.
                                if (attributeModelType != AttributeModelType.Relationship
                                    && attributeModelType != AttributeModelType.AttributeGroup
                                    && attributeModelType != AttributeModelType.MetaDataAttribute
                                    && attributeModelType != AttributeModelType.System)
                                {
                                    Int32 attributeTypeId = 0;

                                    if (attributeModelType == AttributeModelType.Common)
                                        attributeTypeId = (Int32)ObjectType.CommercialAttribute;
                                    else if (attributeModelType == AttributeModelType.Category)
                                        attributeTypeId = (Int32)ObjectType.TechnicalAttribute;
                                    else if (attributeModelType == AttributeModelType.Relationship)
                                        attributeTypeId = (Int32)ObjectType.RelationshipAttribute;

                                    permission = dataSecurityBL.GetAttributePermission(attrModelMappingProperties.Id, attributeTypeId, permissionContext);

                                    if (permission != null && permission.PermissionSet != null)
                                    {
                                        isAttributeHavingPermissions = true;
                                    }
                                }
                                else
                                {
                                    permission = new Permission();
                                    permission.PermissionSet.Add(UserAction.View);

                                    //If attributeModelType is Relationship then add "Update" permission to allow user to updates attributes.
                                    if (attributeModelType == AttributeModelType.Relationship || attributeModelType == AttributeModelType.System || attributeModelType == AttributeModelType.MetaDataAttribute)
                                    {
                                        permission.PermissionSet.Add(UserAction.Update);
                                    }

                                    isAttributeHavingPermissions = true;
                                }
                            }

                            #endregion

                            #region Construct Attribute Model

                            if (isAttributeHavingPermissions || !applySecurity)
                            {
                                foreach (LocaleEnum locale in attributeModelContext.GetLocales())
                                {
                                    attrModelLocaleProperties = GetAttributeModelLocalePropertiesByKey(attributeModelLocaleProperties, GetKey(attrModelMappingProperties.Id, locale));

                                    // If attribute model does not have localized  value then check parent has localized value or not.
                                    if (attrModelLocaleProperties == null)
                                    {
                                        AttributeModelLocaleProperties parentAttributeModelLocaleProperties = GetAttributeModelLocalePropertiesByKey(attributeModelLocaleProperties, GetKey(baseAttributeModel.AttributeParentId, locale));

                                        //Yes, parent attribute model has localized values then construct object and attach to attribute model.
                                        if (parentAttributeModelLocaleProperties != null)
                                        {
                                            attrModelLocaleProperties = ConstructAttributeModelLocaleProperties(baseAttributeModel, parentAttributeModelLocaleProperties);
                                        }
                                    }

                                    attributeModel = new AttributeModel(baseAttributeModel, attrModelMappingProperties, attrModelLocaleProperties, locale);
                                    attributeModel.Locale = locale;

                                    if ((applySecurity || loadAttributePermission) && permission != null)
                                    {
                                        attributeModel.PermissionSet = permission.PermissionSet;

                                        if (applySecurity && !permission.PermissionSet.Contains(UserAction.Update))
                                            attributeModel.ReadOnly = true;
                                    }

                                    #region Load Dependent Attributes

                                    if (isAttributeDependencyEnabled)
                                    {
                                        if (Constants.TRACING_ENABLED)
                                            MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Loading dependent attribute mappings started...", MDMTraceSource.AttributeModelGet);

                                        if (!isAttributeDependencyCacheEnabled)
                                        {
                                            LoadDependentAttributeMappings(attributeModel, applicationContext, callerContext, depedentAttributeModelsDictionary, isCollectionAttributeDependencyEnabled);
                                        }
                                        else
                                        {
                                            LoadDependentAttributeMappingsUsingCache(attributeModel, applicationContext, callerContext, isCollectionAttributeDependencyEnabled);
                                        }

                                        //If the attribute model is complex then load the dependency mapping for children as well.
                                        if (attributeModel.IsComplex)
                                        {
                                            foreach (AttributeModel childAttributeModel in attributeModel.GetChildAttributeModels())
                                            {
                                                if (!isAttributeDependencyCacheEnabled)
                                                {
                                                    LoadDependentAttributeMappings(childAttributeModel, applicationContext, callerContext, depedentAttributeModelsDictionary, isCollectionAttributeDependencyEnabled);
                                                }
                                                else
                                                {
                                                    LoadDependentAttributeMappingsUsingCache(childAttributeModel, applicationContext, callerContext, isCollectionAttributeDependencyEnabled);
                                                }
                                            }
                                        }

                                        if (Constants.TRACING_ENABLED)
                                            MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Loading dependent attribute mappings completed.", MDMTraceSource.AttributeModelGet);

                                        if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                                        {
                                            MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - taken to load dependent attribute mappings.", durationHelper.GetDurationInMilliseconds(DateTime.Now)), MDMTraceSource.AttributeModelGet);
                                        }
                                    }

                                    #endregion

                                    attributeModels.Add(attributeModel, true);
                                }
                            }

                            #endregion
                        }
                    }
                }

                #endregion

                if (attributeModels == null || attributeModels.Count < 1)
                {
                    PermissionsNotFoundException exception = new PermissionsNotFoundException("View/Edit permissions are not available to any attributes in this product view");
                    exception.Source = "Permissions";
                    throw exception;
                }

                #region Apply Sorting

                if (attributeModelContext.ApplySorting)
                {
                    if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Sorting of attribute models based on sort order started...", MDMTraceSource.AttributeModelGet);

                    attributeModels = SortAttributeModels(attributeModels);

                    if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Sorting of attribute models based on sort order completed.", MDMTraceSource.AttributeModelGet);
                }

                #endregion

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Get Attribute Models completed.", MDMTraceSource.AttributeModelGet);

                if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - taken for overall load.Total attribute models loaded are : {1}", completeAttributeModelDurationHelper.GetDurationInMilliseconds(DateTime.Now), attributeModels.Count), MDMTraceSource.AttributeModelGet);
                }
            }

            return attributeModels;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private Dictionary<String, AttributeModelLocaleProperties> GetAttributeModelLocaleProperties(Collection<LocaleEnum> locales)
        {
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Get Attribute Models for locale started...", MDMTraceSource.AttributeModelGet);

            Dictionary<String, AttributeModelLocaleProperties> attributeModelLocalePropertiesCollection = null;

            if (locales != null && locales.Count > 0)
            {
                attributeModelLocalePropertiesCollection = new Dictionary<String, AttributeModelLocaleProperties>();

                foreach (LocaleEnum locale in locales)
                {
                    if (locale == _systemDataLocale)
                        continue;

                    #region Get attribute models locale properties from Cache if available

                    if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Trying to get attribute models locale properties from the cache.", MDMTraceSource.AttributeModelGet);

                    Dictionary<String, AttributeModelLocaleProperties> attributeModelLocaleProperties = AttributeModelBufferManager.FindAttributeModelLocaleProperties(locale);

                    #endregion

                    #region Get locale based attribute models from Database

                    //Not found in cache, load from the DB
                    if (attributeModelLocaleProperties == null)
                    {
                        if (Constants.TRACING_ENABLED)
                            MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Trying to get attribute models locale properties from DB as not found in the cache.", MDMTraceSource.AttributeModelGet);

                        attributeModelLocaleProperties = new AttributeModelDA().GetAttributeModelLocaleProperties(locale);

                        #region Cache base Attribute Models

                        if (attributeModelLocaleProperties != null)
                        {
                            AttributeModelBufferManager.UpdateAttributeModelLocaleProperties(attributeModelLocaleProperties, locale, 3);
                        }

                        #endregion
                    }
                    else
                    {
                        if (Constants.TRACING_ENABLED)
                            MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0} base attribute models locale properties  found in cache for the cache.", attributeModelLocaleProperties.Count), MDMTraceSource.AttributeModelGet);
                    }

                    #endregion

                    #region Merge Attribute Model Locale Properties for all locale

                    if (attributeModelLocaleProperties != null && attributeModelLocaleProperties.Count > 0)
                    {
                        if (locales.Count > 1)
                        {
                            var enumerator = attributeModelLocaleProperties.GetEnumerator();

                            while (enumerator.MoveNext())
                            {
                                String key = enumerator.Current.Key;

                                if (!attributeModelLocalePropertiesCollection.ContainsKey(key))
                                {
                                    attributeModelLocalePropertiesCollection.Add(key, enumerator.Current.Value);
                                }
                            }
                        }
                        else
                        {
                            attributeModelLocalePropertiesCollection = attributeModelLocaleProperties;
                        }
                    }

                    #endregion
                }
            }

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Get Attribute Models for locale completed.", MDMTraceSource.AttributeModelGet);

            return attributeModelLocalePropertiesCollection;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private Dictionary<Int32, AttributeModelBaseProperties> GetBaseAttributeModels()
        {
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Base attribute model load started...", MDMTraceSource.AttributeModelGet);

            Dictionary<String, Int32> attributeUniqueIdentifierToAttributeIdMaps = null;
            Dictionary<String, Int32> attributeNameToAttributeIdMaps = null;

            #region Get base attribute models from Cache if available

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Trying to get base attribute models from the cache.", MDMTraceSource.AttributeModelGet);

            Dictionary<Int32, AttributeModelBaseProperties> baseAttributeModels = AttributeModelBufferManager.FindBaseAttributeModels();

            #endregion

            #region Get base attribute models from Database

            if (baseAttributeModels == null || baseAttributeModels.Count < 1)
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Trying to load base attribute models from DB as not found in cache for the cache.", MDMTraceSource.AttributeModelGet);

                baseAttributeModels = LoadBaseAttributeModelsFromDB(out attributeUniqueIdentifierToAttributeIdMaps, out attributeNameToAttributeIdMaps);
            }
            else
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0} base attribute model(s) found in the cache.", baseAttributeModels.Count), MDMTraceSource.AttributeModelGet);
            }

            #endregion

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Base attribute model load completed.", MDMTraceSource.AttributeModelGet);

            return baseAttributeModels;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private Dictionary<String, Int32> GetAttributeUniqueIdentifierToAttributeIdMaps()
        {
            var attributeUniqueIdentifierToAttributeIdMaps = AttributeModelBufferManager.FindAttributeUniqueIdentifierToAttributeIdMaps();

            if (attributeUniqueIdentifierToAttributeIdMaps == null || attributeUniqueIdentifierToAttributeIdMaps.Count < 1)
            {
                var attributeNameToAttributeIdMaps = new Dictionary<String, Int32>(); // just blank dictionary to pass into DB get call
                LoadBaseAttributeModelsFromDB(out attributeUniqueIdentifierToAttributeIdMaps, out attributeNameToAttributeIdMaps);
            }

            return attributeUniqueIdentifierToAttributeIdMaps;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private Dictionary<String, Int32> GetAttributeNameToAttributeIdMaps()
        {
            var attributeNameToAttributeIdMaps = AttributeModelBufferManager.FindAttributeNameToAttributeIdMaps();

            if (attributeNameToAttributeIdMaps == null || attributeNameToAttributeIdMaps.Count < 1)
            {
                Dictionary<string, int> attributeUniqueIdentifierToAttributeIdMaps; // just blank dictionary to pass into DB get call
                LoadBaseAttributeModelsFromDB(out attributeUniqueIdentifierToAttributeIdMaps, out attributeNameToAttributeIdMaps);
            }

            return attributeNameToAttributeIdMaps;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeUniqueIdentifierToAttributeIdMaps"></param>
        /// <param name="attributeNameToAttributeIdMaps"></param>
        /// <returns></returns>
        private Dictionary<Int32, AttributeModelBaseProperties> LoadBaseAttributeModelsFromDB(out Dictionary<String, Int32> attributeUniqueIdentifierToAttributeIdMaps, out Dictionary<String, Int32> attributeNameToAttributeIdMaps)
        {
            var attributeModelDA = new AttributeModelDA();

            #region Get base attribute models from Database

            Dictionary<Int32, AttributeModelBaseProperties> baseAttributeModels = attributeModelDA.GetAllBaseAttributeModels(out attributeUniqueIdentifierToAttributeIdMaps, out attributeNameToAttributeIdMaps);

            #region Cache base Attribute Models

            if (baseAttributeModels != null && baseAttributeModels.Count > 0)
            {
                AttributeModelBufferManager.UpdateBaseAttributeModels(baseAttributeModels, 3);
                AttributeModelBufferManager.UpdateBaseAttributeIdMaps(attributeUniqueIdentifierToAttributeIdMaps, attributeNameToAttributeIdMaps, 3);
            }

            #endregion

            #endregion

            return baseAttributeModels;
        }

        /// <summary>
        /// Gets the ids of attributes coming under requested attribute groups, custom views or state views 
        /// </summary>
        /// <param name="attributeGroupIds"> Comma separated list of attribute group ids</param>
        /// <param name="customViewId">Custom view id</param>
        /// <param name="stateViewId">State view id</param>
        /// <param name="attributeModelContext">The data context for which ids needs to be fetched</param>
        /// <returns>Comma separated list of attribute ids</returns>
        /// <exception cref="ArgumentNullException">Will be raised when the parameter 'attributeModelContext' is null</exception>
        private AttributeModelMappingPropertiesCollection GetMappedAttributesForContext(Collection<Int32> attributeGroupIds, Int32 customViewId, Int32 stateViewId, AttributeModelContext attributeModelContext)
        {
            if (attributeModelContext == null)
                throw new ArgumentNullException("attributeModelContext");

            String attributeGroupIdListString = String.Empty;

            //Convert attribute group Ids array into comma separated list
            if (attributeGroupIds != null && attributeGroupIds.Count > 0)
            {
                attributeGroupIdListString = ValueTypeHelper.JoinCollection(attributeGroupIds, ",");
            }

            Boolean ignoreMapping = false;

            AttributeModelType attributeModelType = attributeModelContext.AttributeModelType;
            if (attributeModelType == AttributeModelType.AttributeMaster)
            {
                ignoreMapping = true;
            }

            AttributeModelDA attributeModelDA = new AttributeModelDA();
            AttributeModelMappingPropertiesCollection attributeModelMappingProperties = attributeModelDA.GetMappedAttributesForContext(attributeGroupIdListString, customViewId, stateViewId, attributeModelContext.ContainerId, attributeModelContext.EntityTypeId, attributeModelContext.RelationshipTypeId, attributeModelContext.CategoryId, attributeModelContext.GetLocales(), attributeModelContext.EntityId, attributeModelContext.GetOnlyShowAtCreationAttributes, attributeModelContext.GetOnlyRequiredAttributes, ignoreMapping, attributeModelType);

            return attributeModelMappingProperties;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseAttributeModel"></param>
        /// <param name="parentAttributeModelLocaleProperties"></param>
        /// <returns></returns>
        private AttributeModelLocaleProperties ConstructAttributeModelLocaleProperties(AttributeModelBaseProperties baseAttributeModel, AttributeModelLocaleProperties parentAttributeModelLocaleProperties)
        {
            AttributeModelLocaleProperties attrModelLocaleProperties = new AttributeModelLocaleProperties()
            {
                Id = baseAttributeModel.Id,
                AttributeParentLongName = parentAttributeModelLocaleProperties.LongName
            };

            return attrModelLocaleProperties;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeModels"></param>
        /// <param name="callerContext"></param>
        private void LoadOriginalAttributeModels(AttributeModelCollection attributeModels, CallerContext callerContext)
        {
            AttributeModelCollection originalAttributeModels = GetAllBaseAttributeModels();

            if (originalAttributeModels != null && originalAttributeModels.Count > 0)
            {
                foreach (AttributeModel attributeModel in attributeModels)
                {
                    AttributeUniqueIdentifier attributeUniqueIdentifier = new AttributeUniqueIdentifier(attributeModel.Name, attributeModel.AttributeParentName);

                    attributeModel.OriginalAttributeModel = originalAttributeModels.GetAttributeModel(attributeUniqueIdentifier, _systemDataLocale, true);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeModels"></param>
        /// <param name="callerContext"></param>
        private void FillAttributeModels(AttributeModelCollection attributeModels, CallerContext callerContext)
        {
            Dictionary<String, Int32> attributeUniqueIdentifierToAttributeIdMaps = null;
            Dictionary<Int32, AttributeModelBaseProperties> baseAttributeModels = null;

            foreach (AttributeModel attributeModel in attributeModels)
            {
                if (attributeModel.OriginalAttributeModel != null)
                {
                    attributeModel.Id = attributeModel.OriginalAttributeModel.Id;
                    attributeModel.AttributeParentId = attributeModel.OriginalAttributeModel.AttributeParentId;
                    attributeModel.AttributeParentLongName = attributeModel.OriginalAttributeModel.AttributeParentLongName;
                    attributeModel.Path = attributeModel.OriginalAttributeModel.Path;
                    attributeModel.IsComplexChild = attributeModel.OriginalAttributeModel.IsComplexChild;
                }
                else
                {
                    if (attributeUniqueIdentifierToAttributeIdMaps == null)
                        attributeUniqueIdentifierToAttributeIdMaps = GetAttributeUniqueIdentifierToAttributeIdMaps();

                    if (attributeUniqueIdentifierToAttributeIdMaps != null && attributeUniqueIdentifierToAttributeIdMaps.Count > 0)
                    {
                        String key = GetAttributeGroupKeyBasedOnModelType(attributeModel);

                        if (!String.IsNullOrWhiteSpace(key))
                        {
                            Int32 attributeParentId = 0;

                            //Try to find out attribute parent/group id.
                            attributeUniqueIdentifierToAttributeIdMaps.TryGetValue(key, out attributeParentId);

                            attributeModel.AttributeParentId = attributeParentId;

                            // If attribute group id is not available, it means we have complex child attribute
                            // or user imports attribute with incorrect attribute type or incorrect attribute group.
                            // Now try to find with attributeParentName only.
                            if (attributeModel.AttributeParentId < 1)
                            {
                                foreach (String keyValue in attributeUniqueIdentifierToAttributeIdMaps.Keys)
                                {
                                    String complexAttributeName = String.Empty;
                                    String[] values = keyValue.Split(new String[] { Constants.STRING_PATH_SEPARATOR }, StringSplitOptions.None);

                                    if (values != null && values.Count() > 0)
                                    {
                                        complexAttributeName = values[0];
                                    }

                                    if (String.Compare(complexAttributeName, attributeModel.AttributeParentName, StringComparison.InvariantCultureIgnoreCase) == 0)
                                    {
                                        attributeUniqueIdentifierToAttributeIdMaps.TryGetValue(keyValue, out attributeParentId);

                                        if (baseAttributeModels == null)
                                        {
                                            baseAttributeModels = GetBaseAttributeModels();
                                        }

                                        AttributeModelBaseProperties parentAttributeModel = AttributeModelUtility.GetBaseAttributeModelByKey(baseAttributeModels, attributeParentId);

                                        if (parentAttributeModel != null && parentAttributeModel.AttributeModelType == attributeModel.AttributeModelType &&
                                            (parentAttributeModel.IsComplex || parentAttributeModel.AttributeDataTypeName.ToLower().Equals("hierarchical")))
                                        {
                                            attributeModel.AttributeParentId = attributeParentId;
                                            attributeModel.IsComplexChild = true;

                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region Get Attribute Model Mapping Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeIds"></param>
        /// <param name="attributeGroupIds"></param>
        /// <param name="attributeModelContext"></param>
        /// <param name="getLatest"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        private AttributeModelMappingPropertiesCollection GetAttributeModelMappingProperties(Collection<Int32> attributeIds, Collection<Int32> attributeGroupIds, AttributeModelContext attributeModelContext, Boolean getLatest, CallerContext callerContext)
        {
            #region Initial Setup

            AttributeModelMappingPropertiesCollection attributeModelMappingProperties = new AttributeModelMappingPropertiesCollection();
            Int32 containerId = attributeModelContext.ContainerId;
            Int32 entityTypeId = attributeModelContext.EntityTypeId;
            Int64 categoryId = attributeModelContext.CategoryId;
            Int32 relationshipTypeId = attributeModelContext.RelationshipTypeId;
            AttributeModelMappingPropertiesCollection systemAttributeModelMappingProperties = null;
            DurationHelper durationHelper = new DurationHelper(DateTime.Now);

            #endregion

            #region Get Common Attributes Mappings

            if (attributeModelContext.AttributeModelType == AttributeModelType.Common
                || attributeModelContext.AttributeModelType == AttributeModelType.All)
            {
                if (containerId > 0 && entityTypeId > 0)
                {
                    AttributeModelMappingPropertiesCollection commonAttributeModelMappingProperties = MappingPropertiesManager.GetCommonAttributeModelMappingProperties(attributeIds, attributeGroupIds, containerId, entityTypeId, getLatest, callerContext);
                    attributeModelMappingProperties.AddRange(commonAttributeModelMappingProperties);

                    if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - taken for getting common attributes mappings.", durationHelper.GetDurationInMilliseconds(DateTime.Now)), MDMTraceSource.AttributeModelGet);
                }
            }

            #endregion

            #region Get Category Attributes Mappings

            if (attributeModelContext.AttributeModelType == AttributeModelType.Category
               || attributeModelContext.AttributeModelType == AttributeModelType.All)
            {
                if (categoryId > 0)
                {
                    AttributeModelMappingPropertiesCollection categoryAttributeModelMappingProperties = MappingPropertiesManager.GetCategoryAttributeModelMappingProperties(attributeIds, attributeGroupIds, categoryId, getLatest, callerContext);
                    attributeModelMappingProperties.AddRange(categoryAttributeModelMappingProperties);

                    if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - taken for getting category attributes mappings.", durationHelper.GetDurationInMilliseconds(DateTime.Now)), MDMTraceSource.AttributeModelGet);
                }
            }

            #endregion

            #region Get Relationship Attributes Mappings

            if (attributeModelContext.AttributeModelType == AttributeModelType.Relationship
                || attributeModelContext.AttributeModelType == AttributeModelType.All)
            {
                if (relationshipTypeId > 0)
                {
                    AttributeModelMappingPropertiesCollection relationshipAttributeModelMappingProperties = MappingPropertiesManager.GetRelationshipAttirbuteModelMappingProperties(attributeIds, attributeGroupIds, containerId, relationshipTypeId, getLatest, callerContext);
                    attributeModelMappingProperties.AddRange(relationshipAttributeModelMappingProperties);

                    if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - taken for getting relationship attributes mappings.", durationHelper.GetDurationInMilliseconds(DateTime.Now)), MDMTraceSource.AttributeModelGet);
                }
            }

            #endregion

            #region Get System Attributes Mappings

            if (attributeModelContext.AttributeModelType == AttributeModelType.System
                || attributeModelContext.AttributeModelType == AttributeModelType.MetaDataAttribute
                || attributeModelContext.AttributeModelType == AttributeModelType.All)
            {
                systemAttributeModelMappingProperties = GetSystemAttributeModelMappingProperties(attributeIds, attributeGroupIds, attributeModelContext.GetLocales(), attributeModelContext.GetOnlyShowAtCreationAttributes, attributeModelContext.GetOnlyRequiredAttributes, false, callerContext);

                attributeModelMappingProperties.AddRange(systemAttributeModelMappingProperties);

                if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - taken for getting system attributes mappings.", durationHelper.GetDurationInMilliseconds(DateTime.Now)), MDMTraceSource.AttributeModelGet);
            }

            #endregion

            #region Get Attribute Master Mappings

            if (attributeModelContext.AttributeModelType == AttributeModelType.AttributeMaster)
            {
                attributeModelMappingProperties = GetAttributeMasterMappingProperties(attributeIds, attributeGroupIds);
            }

            #endregion

            #region Load Validation States Attributes

            if (attributeModelContext.LoadStateValidationAttributes)
            {
                Collection<Int32> stateValidationAttributeIdList = EntityOperationsHelper.GetStateValidationAttributes();

                AttributeModelMappingPropertiesCollection stateValidationAttributeModelMappingProperties = GetSystemAttributeModelMappingProperties(stateValidationAttributeIdList, null, attributeModelContext.GetLocales(), attributeModelContext.GetOnlyShowAtCreationAttributes, attributeModelContext.GetOnlyRequiredAttributes, false, callerContext);

                if (systemAttributeModelMappingProperties == null || systemAttributeModelMappingProperties.Count < 1)
                {
                    attributeModelMappingProperties.AddRange(stateValidationAttributeModelMappingProperties);
                }
                else
                {
                    foreach (AttributeModelMappingProperties stateValidateAttrModelMappingProperties in stateValidationAttributeModelMappingProperties)
                    {
                        if (!systemAttributeModelMappingProperties.Contains(stateValidateAttrModelMappingProperties.Id, stateValidateAttrModelMappingProperties.AttributeParentId))
                        {
                            attributeModelMappingProperties.Add(stateValidateAttrModelMappingProperties);
                        }
                    }
                }

                if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - taken for getting system attributes mappings.", durationHelper.GetDurationInMilliseconds(DateTime.Now)), MDMTraceSource.AttributeModelGet);
            }

            #endregion Load  Validation States Attributes

            if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - taken for getting overall mapping based properties.", durationHelper.GetDurationInMilliseconds(DateTime.Now)), MDMTraceSource.AttributeModelGet);

            #region Filter Attribute Mappings

            if (attributeModelContext.GetOnlyShowAtCreationAttributes || attributeModelContext.GetOnlyRequiredAttributes)
            {
                attributeModelMappingProperties = FilterShowAtCreationAndRequiredAttributeMappings(attributeModelMappingProperties, attributeModelContext);

                if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - taken for filtering attribute model mappings based context.", durationHelper.GetDurationInMilliseconds(DateTime.Now)), MDMTraceSource.AttributeModelGet);
            }

            #endregion

            return attributeModelMappingProperties;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeIds"></param>
        /// <param name="attributeGroupIds"></param>
        /// <param name="locales"></param>
        /// <param name="getOnlyShowAtCreationAttributes"></param>
        /// <param name="getOnlyRequiredAttributes"></param>
        /// <param name="getLastest"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        private AttributeModelMappingPropertiesCollection GetSystemAttributeModelMappingProperties(Collection<Int32> attributeIds, Collection<Int32> attributeGroupIds, Collection<LocaleEnum> locales, Boolean getOnlyShowAtCreationAttributes, Boolean getOnlyRequiredAttributes, Boolean getLastest, CallerContext callerContext)
        {
            #region Initial Setup

            AttributeModelMappingPropertiesCollection attributeModelMappingProperties = null;
            AttributeModelMappingPropertiesCollection filteredAttributeModelMappingProperties = null;
            MappingBufferManager mappingBufferManager = new MappingBufferManager();

            #endregion

            #region Get System Attribute Mapping Properties from Cache if available

            attributeModelMappingProperties = mappingBufferManager.FindSystemAndMetaDataAttributeModelMappingsPartialDetails();

            #endregion

            #region Get System Attribute Mapping Properties from Database

            if (attributeModelMappingProperties == null || attributeModelMappingProperties.Count < 1)
            {
                AttributeModelContext attributeModelContext = new AttributeModelContext(0, 0, 0, 0, locales, 0, AttributeModelType.System, getOnlyShowAtCreationAttributes, getOnlyRequiredAttributes, false);

                attributeModelMappingProperties = GetMappedAttributesForContext(null, 0, 0, attributeModelContext);

                #region Cache Mappings data

                if (attributeModelMappingProperties != null && attributeModelMappingProperties.Count > 0)
                {
                    mappingBufferManager.UpdateSystemAndMetaDataAttributeMappings(attributeModelMappingProperties, 3);
                }

                #endregion
            }

            #endregion

            #region Filter System Attribute Mapping Properties

            if (attributeModelMappingProperties != null && attributeModelMappingProperties.Count > 0)
            {
                filteredAttributeModelMappingProperties = attributeModelMappingProperties.FilterByAttributeIdAndGroupId(attributeIds, attributeGroupIds);
            }

            #endregion

            return filteredAttributeModelMappingProperties;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeIds"></param>
        /// <param name="attributeGroupIds"></param>
        /// <returns></returns>
        private AttributeModelMappingPropertiesCollection GetAttributeMasterMappingProperties(Collection<Int32> attributeIds, Collection<Int32> attributeGroupIds)
        {
            AttributeModelMappingPropertiesCollection attributeModelMappingProperties = null;
            Dictionary<Int32, AttributeModelBaseProperties> baseAttributeModels = GetBaseAttributeModels();

            if (baseAttributeModels != null && baseAttributeModels.Count > 0)
            {
                attributeModelMappingProperties = new AttributeModelMappingPropertiesCollection();

                if (attributeIds != null && attributeIds.Count > 0)
                {
                    foreach (Int32 attributeId in attributeIds)
                    {
                        if (attributeId > 0)
                        {
                            AttributeModelBaseProperties baseAttributeModel = AttributeModelUtility.GetBaseAttributeModelByKey(baseAttributeModels, attributeId);

                            if (baseAttributeModel != null)
                            {
                                Int32 attributeParentId = baseAttributeModel.AttributeParentId;
                                AttributeModelType attributeModelType = baseAttributeModel.AttributeModelType;
                                Boolean showAtCreation = baseAttributeModel.ShowAtCreation;
                                Boolean required = baseAttributeModel.Required;

                                AttributeModelMappingProperties attrModelMappingProperties = new AttributeModelMappingProperties(attributeId, attributeParentId, required, null, showAtCreation, null, null, null, attributeModelType, null);
                                attributeModelMappingProperties.Add(attrModelMappingProperties);
                            }
                        }
                    }
                }
                else
                {
                    var enumerator = baseAttributeModels.GetEnumerator();

                    while (enumerator.MoveNext())
                    {
                        AttributeModelBaseProperties baseAttributeModel = enumerator.Current.Value;

                        Int32 attributeId = baseAttributeModel.Id;
                        Int32 attributeParentId = baseAttributeModel.AttributeParentId;
                        AttributeModelType attributeModelType = baseAttributeModel.AttributeModelType;
                        Boolean showAtCreation = baseAttributeModel.ShowAtCreation;
                        Boolean required = baseAttributeModel.Required;

                        AttributeModelMappingProperties attrModelMappingProperties = new AttributeModelMappingProperties(attributeId, attributeParentId, required, null, showAtCreation, null, null, null, attributeModelType, null);

                        if (attributeGroupIds != null && attributeGroupIds.Count > 0)
                        {
                            if (attributeGroupIds.Contains(attributeParentId))
                                attributeModelMappingProperties.Add(attrModelMappingProperties);
                        }
                        else
                        {
                            attributeModelMappingProperties.Add(attrModelMappingProperties);
                        }
                    }
                }
            }

            return attributeModelMappingProperties;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeModelMappingProperties"></param>
        /// <param name="attributeModelContext"></param>
        /// <returns></returns>
        private AttributeModelMappingPropertiesCollection FilterShowAtCreationAndRequiredAttributeMappings(AttributeModelMappingPropertiesCollection attributeModelMappingProperties, AttributeModelContext attributeModelContext)
        {
            AttributeModelMappingPropertiesCollection filteredAttributeModelMappingPropertiesCollection = null;

            if (attributeModelMappingProperties != null && attributeModelMappingProperties.Count > 0)
            {
                filteredAttributeModelMappingPropertiesCollection = new AttributeModelMappingPropertiesCollection();

                foreach (AttributeModelMappingProperties attrModelMappingProperties in attributeModelMappingProperties)
                {
                    if (attributeModelContext.AttributeModelType != AttributeModelType.System
                   || attributeModelContext.AttributeModelType != AttributeModelType.MetaDataAttribute
                   || attributeModelContext.AttributeModelType != AttributeModelType.AttributeMaster)
                    {
                        if ((attributeModelContext.GetOnlyShowAtCreationAttributes && attrModelMappingProperties.ShowAtCreation == true)
                            || (attributeModelContext.GetOnlyRequiredAttributes && attrModelMappingProperties.Required == true))
                        {
                            filteredAttributeModelMappingPropertiesCollection.Add(attrModelMappingProperties);
                        }
                    }
                    else
                    {
                        filteredAttributeModelMappingPropertiesCollection.Add(attrModelMappingProperties);
                    }
                }
            }

            return filteredAttributeModelMappingPropertiesCollection;
        }

        #endregion

        #region Process Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeModels"></param>
        /// <param name="operationResults"></param>
        /// <param name="callerContext"></param>
        private void CompareAndMerge(AttributeModelCollection attributeModels, DataModelOperationResultCollection operationResults, CallerContext callerContext)
        {
            const String methodName = "import";

            var attributeModelsWithOperationResults = new Collection<Tuple<AttributeModel, OperationResult>>();

            foreach (AttributeModel deltaAttributeModel in attributeModels)
            {
                IDataModelOperationResult operationResult = operationResults.GetByReferenceId(deltaAttributeModel.ReferenceId);

                //If delta object Action is Read/Ignore then skip Merge Delta Process.
                if (deltaAttributeModel.Action == ObjectAction.Read || deltaAttributeModel.Action == ObjectAction.Ignore)
                    continue;

                IAttributeModel origAttributeModel = deltaAttributeModel.OriginalAttributeModel;

                if (origAttributeModel != null)
                {
                    //If delta object Action is Delete then skip Merge Delta process.
                    if (deltaAttributeModel.Action != ObjectAction.Delete)
                    {
                        origAttributeModel.MergeDelta(deltaAttributeModel, callerContext, false);

                        if (deltaAttributeModel.Action == ObjectAction.Read)
                        {
                            AddOperationResult(operationResult, "113986", String.Format("Attribute: {0} already exists in the system. Object was not modified.", deltaAttributeModel.Name), new Object[] { deltaAttributeModel.Name }, OperationResultType.Information, TraceEventType.Information, callerContext);
                        }
                    }
                }
                else
                {
                    if (deltaAttributeModel.Action == ObjectAction.Delete)
                    {
                        AddOperationResult(operationResult, "113604", String.Empty, new Object[] { deltaAttributeModel.Name, deltaAttributeModel.AttributeParentName }, OperationResultType.Error, TraceEventType.Error, callerContext);
                    }
                    else
                    {
                        if (deltaAttributeModel.AttributeParentId < 1)
                        {
                            AddOperationResult(operationResult, "114132", "Attribute Parent: {0} is invalid or is not a child of Attribute Type: {1}.", new Object[] { deltaAttributeModel.AttributeParentName, deltaAttributeModel.AttributeModelType }, OperationResultType.Error, TraceEventType.Error, callerContext);
                        }

                        if (deltaAttributeModel.AttributeModelType == AttributeModelType.Unknown)
                        {
                            AddOperationResult(operationResult, "113698", "Attribute type cannot be null or unknown", null, OperationResultType.Error, TraceEventType.Error, callerContext);
                        }

                        if (deltaAttributeModel.AttributeModelType != AttributeModelType.AttributeGroup && deltaAttributeModel.AttributeModelType != AttributeModelType.CommonAttributeGroup &&
                            deltaAttributeModel.AttributeModelType != AttributeModelType.CategoryAttributeGroup && deltaAttributeModel.AttributeModelType != AttributeModelType.RelationshipAttributeGroup)
                        {
                            if (String.IsNullOrWhiteSpace(deltaAttributeModel.AttributeDataTypeName) || deltaAttributeModel.AttributeDataTypeId < 1)
                            {
                                AddOperationResult(operationResult, "113677", "Attribute data type is empty or not specified.", null, OperationResultType.Error, TraceEventType.Error, callerContext);
                            }
                            if (String.IsNullOrWhiteSpace(deltaAttributeModel.AttributeDisplayTypeName) || deltaAttributeModel.AttributeDisplayTypeId < 1)
                            {
                                AddOperationResult(operationResult, "113678", "Attribute DisplayType is empty or not specified.", null, OperationResultType.Error, TraceEventType.Error, callerContext);
                            }
                        }

                        //If original object is not found then set Action as Create always.
                        deltaAttributeModel.Action = ObjectAction.Create;
                    }
                }

                operationResult.PerformedAction = deltaAttributeModel.Action;

                if (deltaAttributeModel.IsCollection)   //For further validations
                {
                    ValidateComplexChildAttributes(attributeModels, deltaAttributeModel, (OperationResult)operationResult, callerContext);
                }

                attributeModelsWithOperationResults.Add(new Tuple<AttributeModel, OperationResult>(deltaAttributeModel, (OperationResult)operationResult));
            }

            ValidateForUniqueness(attributeModelsWithOperationResults, callerContext, methodName);
        }

        #endregion

        #region Utility Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeName"></param>
        /// <returns></returns>
        private String GetKey(String attributeName)
        {
            return attributeName != null ? attributeName.Trim().ToLowerInvariant() : String.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeName"></param>
        /// <param name="attributeParentName"></param>
        /// <returns></returns>
        private String GetKey(String attributeName, String attributeParentName)
        {
            return String.Concat(attributeName.Trim(), Constants.STRING_PATH_SEPARATOR, attributeParentName.Trim());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeId"></param>
        /// <param name="locale"></param>
        /// <returns></returns>
        private String GetKey(Int32 attributeId, LocaleEnum locale)
        {
            return String.Concat(attributeId, AttributeKeySeparator, (Int32)locale);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeModelLocaleProperties"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private AttributeModelLocaleProperties GetAttributeModelLocalePropertiesByKey(Dictionary<String, AttributeModelLocaleProperties> attributeModelLocaleProperties, String key)
        {
            AttributeModelLocaleProperties attrModelLocaleProperties = null;

            if (attributeModelLocaleProperties != null && attributeModelLocaleProperties.ContainsKey(key))
            {
                attrModelLocaleProperties = attributeModelLocaleProperties[key];
            }

            return attrModelLocaleProperties;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentAttributeModel"></param>
        /// <returns></returns>
        private String GetAttributeGroupKeyBasedOnModelType(AttributeModel currentAttributeModel)
        {
            String key = null;

            if (currentAttributeModel != null)
            {
                if (currentAttributeModel.AttributeModelType == AttributeModelType.Common)
                {
                    key = GetKey(currentAttributeModel.AttributeParentName, "Common Attributes");
                }
                else if (currentAttributeModel.AttributeModelType == AttributeModelType.Category)
                {
                    key = GetKey(currentAttributeModel.AttributeParentName, "Category Specific");
                }
                else if (currentAttributeModel.AttributeModelType == AttributeModelType.Relationship)
                {
                    key = GetKey(currentAttributeModel.AttributeParentName, "Relationship Attributes");
                }
            }

            return key;
        }

        /// <summary>
        /// 
        /// </summary>
        private void AddOperationResults(DataModelOperationResultCollection operationResults, String messageCode, String message, Object[] parameters, OperationResultType operationResultType, TraceEventType traceEventType, CallerContext callerContext)
        {
            if (parameters != null && parameters.Any())
            {
                _localeMessage = LocaleMessageManager.TryGet(_systemUILocale, messageCode, message, parameters, false, callerContext);
            }
            else
            {
                _localeMessage = LocaleMessageManager.TryGet(_systemUILocale, messageCode, message, false, callerContext);
            }

            if (_localeMessage != null)
            {
                operationResults.AddOperationResult(_localeMessage.Code, _localeMessage.Message, operationResultType);
            }
        }

        /// <summary>
        /// Updates Operation result using input parameters
        /// </summary>
        private void AddOperationResult(IOperationResult operationResult, String messageCode, String message, Object[] parameters, OperationResultType operationResultType, TraceEventType traceEventType, CallerContext callerContext)
        {
            if (parameters != null && parameters.Any())
            {
                _localeMessage = LocaleMessageManager.TryGet(_systemUILocale, messageCode, message, parameters, false, callerContext);
            }
            else
            {
                _localeMessage = LocaleMessageManager.TryGet(_systemUILocale, messageCode, message, false, callerContext);
            }

            if (_localeMessage != null)
            {
                Collection<Object> paramsCollection = parameters != null ? new Collection<Object>(parameters) : null;

                operationResult.AddOperationResult(_localeMessage.Code, _localeMessage.Message, paramsCollection, operationResultType);
            }
        }

        #endregion

        #region Validation methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeModelContext"></param>
        private void ValidateAttributeModelContext(AttributeModelContext attributeModelContext)
        {
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Validate Attribute Model Context started...", MDMTraceSource.AttributeModelGet);

            if (attributeModelContext == null)
                throw new ArgumentNullException("attributeModelContext");

            switch (attributeModelContext.AttributeModelType)
            {
                case AttributeModelType.All:
                    if ((attributeModelContext.ContainerId < 1 || attributeModelContext.EntityTypeId < 1))
                    {
                        if (attributeModelContext.CategoryId < 1)
                        {
                            if (attributeModelContext.RelationshipTypeId < 1)
                                throw new ArgumentException("Attribute Models cannot be fetched. Provide either Container Id or Entity Type Id or Category Id or Relationship Type Id for which Attribute Models are required.");
                        }
                    }
                    break;
                case AttributeModelType.Common:
                    if (attributeModelContext.ContainerId < 1 || attributeModelContext.EntityTypeId < 1)
                        throw new ArgumentException("Attribute Models cannot be fetched. Provide either Container Id or Entity Type Id for which Attribute Models are required.");
                    break;
                case AttributeModelType.Category:
                    if (attributeModelContext.CategoryId < 1)
                        throw new ArgumentException("Attribute Models cannot be fetched. Provide Category Id for which Attribute Models are required.");
                    break;
                case AttributeModelType.Relationship:
                    if (attributeModelContext.RelationshipTypeId < 1)
                        throw new ArgumentException("Attribute Models cannot be fetched. Provide Relationship Type Id for which Attribute Models are required.");
                    break;
            }

            ValidateAttributeModelContextForLocales(attributeModelContext);

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Validate Attribute Model Context completed.", MDMTraceSource.AttributeModelGet);
        }

        /// <summary>
        /// Check AttributeModelContext, if no locale value is provided then add locale as System default data locale
        /// </summary>
        /// <param name="attributeModelContext">AttributeModelContext to check</param>
        private void ValidateAttributeModelContextForLocales(AttributeModelContext attributeModelContext)
        {
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Validate Attribute Model Context for locales started...", MDMTraceSource.AttributeModelGet);

            var attributeModelContextLocales = attributeModelContext.Locales;

            if (attributeModelContextLocales.Contains(LocaleEnum.UnKnown) || attributeModelContextLocales.Contains(LocaleEnum.Neutral) || attributeModelContextLocales.Count < 1)
            {
                attributeModelContextLocales.Add(_systemDataLocale);
            }

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Validate Attribute Model Context for locales completed", MDMTraceSource.AttributeModelGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeUniqueIdentifierCollection"></param>
        /// <param name="methodName"></param>
        /// <param name="callerContext"></param>
        private void ValidateAttributeUniqueIdentifierCollection(AttributeUniqueIdentifierCollection attributeUniqueIdentifierCollection, String methodName, CallerContext callerContext = null)
        {
            if (callerContext == null)
            {
                callerContext = new CallerContext();
            }

            if (attributeUniqueIdentifierCollection == null)
            {
                String errorMessage = this.GetSystemLocaleMessage("112643", callerContext).Message;
                throw new MDMOperationException("112643", errorMessage, "AttributeModelManager.AttributeModelBL", String.Empty, methodName, MDMTraceSource.DataModel);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="callerContext"></param>
        /// <param name="methodName"></param>
        private void ValidateCallerContext(CallerContext callerContext, String methodName)
        {
            if (callerContext == null)
            {
                String errorMessage = "CallerContext cannot be null.";
                throw new MDMOperationException("111846", errorMessage, "DataModelManager.EntityTypeBL", String.Empty, methodName, MDMTraceSource.DataModel);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeUniqueIdentifier"></param>
        /// <param name="methodName"></param>
        /// <param name="callerContext"></param>
        private void ValidateAttributeUniqueIdentifier(AttributeUniqueIdentifier attributeUniqueIdentifier, String methodName, CallerContext callerContext = null)
        {
            if (callerContext == null)
            {
                callerContext = new CallerContext();
            }

            if (attributeUniqueIdentifier == null)
            {
                String errorMessage = this.GetSystemLocaleMessage("112643", callerContext).Message;
                throw new MDMOperationException("112643", errorMessage, "DataModelManager.EntityTypeBL", String.Empty, methodName, MDMTraceSource.DataModel);
            }

            if (String.IsNullOrWhiteSpace(attributeUniqueIdentifier.AttributeName))
            {
                String errorMessage = this.GetSystemLocaleMessage("112646", callerContext).Message;
                throw new MDMOperationException("112646", errorMessage, "DataModelManager.EntityTypeBL", String.Empty, methodName, MDMTraceSource.DataModel);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeModels"></param>
        /// <param name="operationResults"></param>
        /// <param name="callerContext"></param>
        private void ValidateInputParameters(AttributeModelCollection attributeModels, DataModelOperationResultCollection operationResults, CallerContext callerContext)
        {
            var allAttributeUniqueNames = new Collection<String>();

            Boolean validateUomDependencies = _dataModelValidationOptions != null && _dataModelValidationOptions.ValidateUomDependencies;

            if (callerContext == null)
            {
                AddOperationResults(operationResults, "111846", String.Empty, null, OperationResultType.Error, TraceEventType.Error, new CallerContext(MDMCenterApplication.MDMCenter, MDMCenterModules.AttributeImport));
            }

            if (attributeModels == null || attributeModels.Count < 1)
            {
                AddOperationResults(operationResults, "112815", String.Empty, null, OperationResultType.Error, TraceEventType.Error, callerContext);
            }
            else
            {
                foreach (AttributeModel attributeModel in attributeModels)
                {
                    IDataModelOperationResult operationResult = operationResults.GetByReferenceId(attributeModel.ReferenceId);

                    if (String.IsNullOrWhiteSpace(attributeModel.Name))
                    {
                        AddOperationResult(operationResult, "112646", String.Empty, null, OperationResultType.Error, TraceEventType.Error, callerContext);
                    }
                    else
                    {
                        ValidateLocalization(attributeModel, operationResult, callerContext);

                        ValidateSortOrder(attributeModel, operationResult, callerContext);

                        if (attributeModel.IsLookup)
                        {
                            _lookupMetadata = null;
                            LoadLookupMetadata(attributeModel, callerContext);
                        }

                        if (String.IsNullOrWhiteSpace(attributeModel.AttributeParentName))
                        {
                            AddOperationResult(operationResult, "112647", String.Empty, new Object[] { attributeModel.AttributeParentName }, OperationResultType.Error, TraceEventType.Error, callerContext);
                        }
                        if ((String.Compare(attributeModel.AttributeDataTypeName, AttributeDataType.Decimal.ToString(), StringComparison.InvariantCultureIgnoreCase)) == 0)
                        {
                            ValidatePrecision(attributeModel, operationResult, callerContext);
                        }
                        if ((String.Compare(attributeModel.AttributeDisplayTypeName, AttributeDisplayType.LookupTable.ToString(), StringComparison.InvariantCultureIgnoreCase)) == 0)
                        {
                            ValidateLookupColumns(attributeModel, operationResult, callerContext);
                        }
                        if (attributeModel.AttributeModelType == AttributeModelType.System
                            || attributeModel.AttributeModelType == AttributeModelType.MetaDataAttribute)
                        {
                            AddOperationResult(operationResult, "113706", String.Format("'{0}' attribute under '{1}' is an internal attribute of type '{2}'. Hence will not be processed further.", attributeModel.Name, attributeModel.AttributeParentName, attributeModel.AttributeModelType), new object[] { attributeModel.Name, attributeModel.AttributeParentName, attributeModel.AttributeModelType }, OperationResultType.Error, TraceEventType.Warning, callerContext);
                        }
                        if (!String.IsNullOrWhiteSpace(attributeModel.DefaultValue) || !String.IsNullOrWhiteSpace(attributeModel.MinInclusive) ||
                            !String.IsNullOrWhiteSpace(attributeModel.MinExclusive) || !String.IsNullOrWhiteSpace(attributeModel.MaxInclusive) ||
                            !String.IsNullOrWhiteSpace(attributeModel.MaxExclusive) || attributeModel.MinLength != 0 || attributeModel.MaxLength != 0)
                        {
                            ValidateRangeAndDefaultValues(attributeModel, operationResult, callerContext);
                        }
                        if (validateUomDependencies && !String.IsNullOrWhiteSpace(attributeModel.UomType))
                        {
                            ValidateUom(attributeModel, operationResult, callerContext);
                        }
                        if (!String.IsNullOrWhiteSpace(attributeModel.AllowableValues))
                        {
                            if (attributeModel.AllowableValues.Length > 4000)
                            {
                                AddOperationResult(operationResult, "113955", String.Format("{0} length should be less than {1} characters", "Allowable Values", "4000"), new Object[] { "Allowable Values", "4000" }, OperationResultType.Error, TraceEventType.Warning, callerContext);
                            }
                        }
                        if (attributeModel.Name.Length > 150)
                        {
                            AddOperationResult(operationResult, "110640", "Short Name length should be less than 150 characters", null, OperationResultType.Error, TraceEventType.Warning, callerContext);
                        }

                        if (!String.IsNullOrEmpty(attributeModel.LongName) && attributeModel.LongName.Length > 300)
                        {
                            AddOperationResult(operationResult, "110639", "Long Name length should be less than 300 characters", null, OperationResultType.Error, TraceEventType.Warning, callerContext);
                        }

                        if (attributeModel.Name.Contains("<") || attributeModel.Name.Contains(">"))
                        {
                            AddOperationResult(operationResult, "113949", String.Format("Invalid '{0}' value provided for attribute '{1}' and attribute parent '{2}'. Value cannot contain '<' or '>' characters.", "Short Name", attributeModel.Name, attributeModel.AttributeParentName),
                                new Object[] { "Short Name", attributeModel.Name, attributeModel.AttributeParentName }, OperationResultType.Error, TraceEventType.Warning, callerContext);
                        }

                        if (String.IsNullOrWhiteSpace(attributeModel.LongName))
                        {
                            AddOperationResult(operationResult, "113676", "Attribute long name is empty or not specified.", null, OperationResultType.Error, TraceEventType.Error, callerContext);
                        }
                        else if (attributeModel.LongName.Contains("<") || attributeModel.LongName.Contains(">"))
                        {
                            AddOperationResult(operationResult, "113949", String.Format("Invalid '{0}' value provided for attribute '{1}' and attribute parent '{2}'. Value cannot contain '<' or '>' characters.", "Long Name", attributeModel.Name, attributeModel.AttributeParentName),
                                new Object[] { "Long Name", attributeModel.Name, attributeModel.AttributeParentName }, OperationResultType.Error, TraceEventType.Warning, callerContext);
                        }

                        String attributeNameKey = String.Format("{0}_{1}", attributeModel.Name, attributeModel.Locale);

                        if (allAttributeUniqueNames.Contains(attributeNameKey))
                        {
                            AddUniqueCheckFailedOperationResult(attributeModel, (OperationResult)operationResult, callerContext);
                        }
                        else
                        {
                            allAttributeUniqueNames.Add(attributeNameKey);
                        }

                        if (!String.IsNullOrWhiteSpace(attributeModel.UomType) && !String.IsNullOrWhiteSpace(attributeModel.AllowableUOM) && String.IsNullOrWhiteSpace(attributeModel.DefaultUOM))
                        {
                            AddOperationResult(operationResult, "114279", "Default UOM is empty or not specified.", null, OperationResultType.Error, TraceEventType.Error, callerContext);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeModelsWithOperationResults"></param>
        /// <param name="callerContext"></param>
        /// <param name="methodName"></param>
        private void ValidateForUniqueness(Collection<Tuple<AttributeModel, OperationResult>> attributeModelsWithOperationResults, CallerContext callerContext, String methodName)
        {
            Dictionary<String, Int32> attributeUniqueListDictionary = null;

            attributeUniqueListDictionary = GetAttributeNameToAttributeIdMaps();

            foreach (var attributeModelWithOperationResult in attributeModelsWithOperationResults)
            {
                var attributeModel = attributeModelWithOperationResult.Item1;
                var operationResult = attributeModelWithOperationResult.Item2;

                String key = GetKey(attributeModel.Name);

                if ((attributeModel.Action == ObjectAction.Create || attributeModel.Action == ObjectAction.Update)
                        && attributeUniqueListDictionary.ContainsKey(key))
                {
                    Int32 attributeId = attributeUniqueListDictionary[key];

                    if (attributeId > 0 && attributeId != attributeModel.Id) // check if existing attribute is same as current attribute model
                    {
                        AddUniqueCheckFailedOperationResult(attributeModel, operationResult, callerContext);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="useAttributeShortNameOnlyForUniqueness"></param>
        /// <param name="attributeModel"></param>
        /// <param name="operationResult"></param>
        /// <param name="callerContext"></param>
        private void AddUniqueCheckFailedOperationResult(AttributeModel attributeModel, OperationResult operationResult, CallerContext callerContext)
        {
            AddOperationResult(operationResult, "113991", // correct message code here
                String.Format("Failed to process the attribute model. Attribute with the short name '{0}' already exists.", attributeModel.Name),
                new Object[] { attributeModel.Name }, OperationResultType.Error, TraceEventType.Error, callerContext);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeModel"></param>
        /// <param name="operationResult"></param>
        /// <param name="callerContext"></param>
        private void ValidateAttributeDefaultValueWithAllowableValues(AttributeModel attributeModel, IDataModelOperationResult operationResult, CallerContext callerContext)
        {
            Boolean isValueValid = false;

            String[] allowedValuesList = attributeModel.AllowableValues.Split(new String[] { "||" }, StringSplitOptions.RemoveEmptyEntries);

            if (allowedValuesList.Contains(attributeModel.DefaultValue))
            {
                isValueValid = true;
            }

            if (!isValueValid)
            {
                AddOperationResult(operationResult, "113952", String.Format("Invalid default value provided for attribute '{0}' and attribute parent '{1}'. Default value must exist in Allowable Value(s) list.", attributeModel.Name, attributeModel.AttributeParentName),
                    new Object[] { attributeModel.Name, attributeModel.AttributeParentName }, OperationResultType.Error, TraceEventType.Error, callerContext);
            }
        }

        private void ValidateComplexChildAttributes(AttributeModelCollection attributeModels, AttributeModel sourceAttributeModel, OperationResult operationResult, CallerContext callerContext)
        {
            if (attributeModels != null && sourceAttributeModel != null && sourceAttributeModel.IsCollection && sourceAttributeModel.IsComplexChild)
            {
                IAttributeModel parentAttrModel = attributeModels.GetAttributeModel(sourceAttributeModel.AttributeParentId, sourceAttributeModel.Locale);

                if (parentAttrModel != null && parentAttrModel.IsComplex && parentAttrModel.AttributeDataTypeName.ToLower().Equals("hierarchical"))
                {
                    AddOperationResult(operationResult, "114036", String.Format("Invalid IsCollection value. The ‘IsCollection’ is not supported for the child attribute: {0} of the complex attribute: {1}.", sourceAttributeModel.Name, sourceAttributeModel.AttributeParentName),
                        new Object[] { sourceAttributeModel.Name, sourceAttributeModel.AttributeParentName }, OperationResultType.Error, TraceEventType.Error, callerContext);
                }
            }
        }

        private void ValidateExportMask(AttributeModel attributeModel, OperationResult operationResult, CallerContext callerContext)
        {
            if (attributeModel.IsLookup && !String.IsNullOrEmpty(attributeModel.ExportMask) && LookupUtility.IsExportMaskInvalid(attributeModel.ExportMask))
            {
                AddOperationResult(operationResult, "110254", "Invalid format of Export Mask", new Object[0], OperationResultType.Error, TraceEventType.Error, callerContext);
            }
        }

        #endregion

        #region Generate Complex Attribute Schema

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeModels"></param>
        /// <param name="operationResults"></param>
        /// <param name="callerContext"></param>
        private void GenerateComplexAttributeSchema(AttributeModelCollection attributeModels, DataModelOperationResultCollection operationResults, CallerContext callerContext)
        {
            if (attributeModels != null && attributeModels.Count > 0)
            {
                HashSet<Int32> complexAttributeIds = new HashSet<Int32>();

                foreach (AttributeModel attributeModel in attributeModels)
                {
                    if (!attributeModel.IsComplexChild)
                        continue;

                    DataModelOperationResult operationResult = operationResults.GetByReferenceId(attributeModel.ReferenceId) as DataModelOperationResult;

                    if (operationResult.OperationResultStatus == OperationResultStatusEnum.None || operationResult.OperationResultStatus == OperationResultStatusEnum.Successful)
                    {
                        complexAttributeIds.Add(attributeModel.AttributeParentId);
                    }
                }

                if (complexAttributeIds.Count > 0)
                {
                    DynamicTableSchemaBL dynamicTableSchemaBL = new DynamicTableSchemaBL();

                    foreach (Int32 complexAttributeId in complexAttributeIds)
                    {
                        dynamicTableSchemaBL.Get(complexAttributeId, DynamicTableType.Complex, callerContext);
                    }
                }
            }
        }

        #endregion

        #region Validate Range Values

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeModel"></param>
        /// <param name="operationResult"></param>
        /// <param name="callerContext"></param>
        private void ValidatePrecision(AttributeModel attributeModel, IDataModelOperationResult operationResult, CallerContext callerContext)
        {
            if (attributeModel.Precision < 1)
            {
                AddOperationResult(operationResult, "113919", String.Format("Precision cannot be zero or blank for decimal attributes. Please enter a valid precision for attribute {0} under group {1} and re-upload the data model", attributeModel.Name, attributeModel.AttributeParentName), new Object[] { attributeModel.Name, attributeModel.AttributeParentName }, OperationResultType.Error, TraceEventType.Error, callerContext);
            }
            else if (!String.IsNullOrWhiteSpace(attributeModel.DefaultValue))
            {
                CultureInfo cultureInfo = new CultureInfo(attributeModel.Locale.GetCultureName());
                Regex regEx = new Regex(String.Format(@"/^-?\\d+\{0}?\d{{0,{1}}}?$", cultureInfo.NumberFormat.NumberDecimalSeparator, attributeModel.Precision));

                if (!regEx.IsMatch(attributeModel.DefaultValue))
                {
                    AddOperationResult(operationResult, "113946", String.Format("Invalid default value provided for attribute '{0}' and attribute parent '{1}'. Enter decimal value with specified precision.", attributeModel.Name, attributeModel.AttributeParentName), new Object[] { attributeModel.Name, attributeModel.AttributeParentName }, OperationResultType.Error, TraceEventType.Error, callerContext);
                }
            }
        }

        /// <summary>
        /// Validates uom
        /// </summary>
        /// <param name="attributeModel">Indicates attribute model for which needs to perform validation</param>
        /// <param name="operationResult">Indicates operation result for attribute model</param>
        /// <param name="callerContext">Indicates context indicating the caller of the API</param>
        private void ValidateUom(AttributeModel attributeModel, IDataModelOperationResult operationResult, CallerContext callerContext)
        {
            UomManager.Business.UomBL uomManager = new UomManager.Business.UomBL();
            UOMCollection targetUoms = uomManager.GetAllUomsWithType(new UomContext(), callerContext, attributeModel.UomType);

            #region Validate UOM Type

            if (!targetUoms.Any())
            {
                Object[] stringParams = new Object[] { attributeModel.Name, attributeModel.AttributeParentName, attributeModel.UomType };

                AddOperationResult(operationResult, "114031",
                    String.Format("Attribute '{0}' under '{1}' group has invalid UOM Type '{2}'.", stringParams),
                    stringParams, OperationResultType.Error, TraceEventType.Error, callerContext);

                return;
            }

            #endregion

            #region Validate allowable and default UOMs

            if (!String.IsNullOrWhiteSpace(attributeModel.AllowableUOM))
            {
                IList<String> allowableUoms = attributeModel.AllowableUOM.Split(RSExcelConstants.UomSeparator.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                IList<String> invalidUoms = new List<String>();
                foreach (String allowableUom in allowableUoms)
                {
                    if (!targetUoms.Any(uom => uom.Key.Equals(allowableUom, StringComparison.InvariantCultureIgnoreCase)))
                    {
                        invalidUoms.Add(allowableUom);
                    }
                }

                if (invalidUoms.Any())
                {
                    Object[] stringParams = new Object[] { attributeModel.Name, attributeModel.AttributeParentName, String.Join(RSExcelConstants.UomSeparator, invalidUoms) };

                    AddOperationResult(operationResult, "114033",
                        String.Format("Attribute '{0}' under '{1}' group has invalid Allowed UOMs '{2}'.", stringParams),
                        stringParams, OperationResultType.Error, TraceEventType.Error, callerContext);
                }

                #region Validate default UOM

                if (!String.IsNullOrWhiteSpace(attributeModel.DefaultUOM) && !allowableUoms.Contains(attributeModel.DefaultUOM))
                {
                    Object[] stringParams = new Object[] { attributeModel.Name, attributeModel.AttributeParentName, attributeModel.DefaultUOM };

                    AddOperationResult(operationResult, "114034",
                        String.Format("Attribute '{0}' under '{1}' group has invalid Default UOM '{2}'.", stringParams),
                        stringParams, OperationResultType.Error, TraceEventType.Error, callerContext);
                }

                #endregion

            }

            #endregion
        }

        /// <summary>
        /// Validates lookup default value
        /// </summary>
        /// <param name="attributeModel">Indicates attribute model for which needs to perform validation</param>
        /// <param name="operationResult">Indicates operation result for attribute model</param>
        /// <param name="callerContext">Indicates context indicating the caller of the API</param>
        private void ValidateLookupDefaultValue(AttributeModel attributeModel, IDataModelOperationResult operationResult, CallerContext callerContext)
        {
            String defaultValue = attributeModel.DefaultValue;
            if (String.IsNullOrWhiteSpace(defaultValue) || String.IsNullOrWhiteSpace(attributeModel.LookUpTableName) || _lookupManager == null)
            {
                return;
            }

            if (_lookupMetadata == null)
            {
                Object[] stringParams = new Object[] { attributeModel.Name, attributeModel.AttributeParentName, attributeModel.LookUpTableName };

                AddOperationResult(operationResult, "114037",
                    String.Format("Attribute '{0}' under '{1}' group has invalid lookup table name '{2}'.", stringParams),
                    stringParams, OperationResultType.Error, TraceEventType.Error, callerContext);
            }

            Lookup lookup = LookupUtility.ConstructAttributeLookupData(attributeModel, _lookupMetadata, true);

            // Default value is invalid if it cannot be found in display and export format values, if these formats are configured
            if ((!lookup.IsDisplayFormatConfigured() || lookup.GetRecordByDisplayFormat(defaultValue) == null) &&
                (!lookup.IsExportFormatConfigured() || lookup.GetRecordByExportFormat(defaultValue) == null))
            {
                Object[] stringParams = new Object[] { attributeModel.Name, attributeModel.AttributeParentName, defaultValue, lookup.Name };

                AddOperationResult(operationResult, "114032",
                    String.Format("Attribute '{0}' under '{1}' group has invalid default value '{2}' for lookup '{3}'.", stringParams),
                    stringParams, OperationResultType.Error, TraceEventType.Error, callerContext);
            }
        }

        /// <summary>
        /// Validates lookup columns like search columns, display format, display columns, lookup sort order for lookup attribute
        /// </summary>
        /// <param name="attributeModel">Indicates attribute model for which needs to perform validation for lookup columns</param>
        /// <param name="operationResult">Indicates operation result for attribute model</param>
        /// <param name="callerContext">Indicates context indicating the caller of the API</param>
        private void ValidateLookupColumns(AttributeModel attributeModel, IDataModelOperationResult operationResult, CallerContext callerContext)
        {
            #region Validate lookup table name for lookup attribute

            if (String.IsNullOrWhiteSpace(attributeModel.LookUpTableName))
            {
                AddOperationResult(operationResult, "113976", String.Format("Attribute '{0}' under '{1}' group is of the type lookup, but lookup table name is not specified.", attributeModel.Name, attributeModel.AttributeParentName),
                new Object[] { attributeModel.Name, attributeModel.AttributeParentName }, OperationResultType.Error, TraceEventType.Error, callerContext);
            }

            #endregion Validate lookup table name for lookup attribute

            #region Validate display format for lookup attribute

            if (!String.IsNullOrEmpty(attributeModel.LkDisplayFormat))
            {
                ValidateLookupColumns(attributeModel, attributeModel.LkDisplayFormat, "Display Format", false, operationResult, callerContext);
            }

            #endregion Validate display format for lookup attribute

            #region Validate display columns for lookup attribute

            if (!String.IsNullOrEmpty(attributeModel.LkDisplayColumns))
            {
                ValidateLookupColumns(attributeModel, attributeModel.LkDisplayColumns, "Display Columns", true, operationResult, callerContext);
            }
            else
            {
                AddOperationResult(operationResult, "113947", String.Format("'{0}' is empty or not specified for attribute '{1}' and attribute parent '{2}'. Specify the value using open and close brackets. Example: '[Value],[Code]'", "Display Columns", attributeModel.Name, attributeModel.AttributeParentName),
                    new Object[] { "Display Columns", attributeModel.Name, attributeModel.AttributeParentName }, OperationResultType.Error, TraceEventType.Error, callerContext);
            }

            #endregion Validate display format for lookup attribute

            #region Validate sort order for lookup attribute

            if (!String.IsNullOrEmpty(attributeModel.LkSortOrder))
            {
                ValidateLookupColumns(attributeModel, attributeModel.LkSortOrder, "Sort Order", true, operationResult, callerContext);
            }
            else
            {
                AddOperationResult(operationResult, "113947", String.Format("'{0}' is empty or not specified for attribute '{1}' and attribute parent '{2}'. Specify the value using open and close brackets. Example: '[Value],[Code]'", "Sort Order", attributeModel.Name, attributeModel.AttributeParentName),
                   new Object[] { "Sort Order", attributeModel.Name, attributeModel.AttributeParentName }, OperationResultType.Error, TraceEventType.Error, callerContext);
            }

            #endregion Validate sort order for lookup attribute

            #region Validate search columns for lookup attribute

            if (!String.IsNullOrEmpty(attributeModel.LkSearchColumns))
            {
                ValidateLookupColumns(attributeModel, attributeModel.LkSearchColumns, "Search Columns", true, operationResult, callerContext);
            }
            else
            {
                AddOperationResult(operationResult, "113947", String.Format("'{0}' is empty or not specified for attribute '{1}' and attribute parent '{2}'. Specify the value using open and close brackets. Example: '[Value],[Code]'", "Search Columns", attributeModel.Name, attributeModel.AttributeParentName),
                   new Object[] { "Search Columns", attributeModel.Name, attributeModel.AttributeParentName }, OperationResultType.Error, TraceEventType.Error, callerContext);
            }

            #endregion Validate search columns for lookup attribute

            #region Validate Mapped Lookup for Lookup Attribute

            if (_dataModelValidationOptions != null && _dataModelValidationOptions.ValidateLookupDependencies)
            {
                if (_lookupMetadata == null)
                {
                    String message = "Lookup table '{0}' mapped to the attribute '{1}' and attribute parent '{2}' is not available in the system.";
                    AddOperationResult(operationResult, "114049", message, new Object[] { attributeModel.LookUpTableName, attributeModel.Name, attributeModel.AttributeParentName }, OperationResultType.Error, TraceEventType.Error, callerContext);
                }
            }

            #endregion Validate Mapped Lookup for Lookup Attribute

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeModel"></param>
        /// <param name="callerContext"></param>
        private void LoadLookupMetadata(AttributeModel attributeModel, CallerContext callerContext)
        {
            if (_lookupMetadata == null)
                _lookupMetadata = _lookupManager.GetModel(attributeModel.LookUpTableName, callerContext);
        }

        /// <summary>
        /// Validates lookup columns like search columns, display format, display columns, lookup sort order for given lookup column
        /// </summary>
        /// <param name="attributeModel">Indicates attribute model for which needs to validate lookup column</param>
        /// <param name="value">Indicates values which needs to validate</param>
        /// <param name="parameterName">Indicates name of the parameter</param>
        /// <param name="operationResult">Indicates operation result for attribute model</param>
        /// <param name="callerContext">Indicates context indicating the caller of the API</param>
        private void ValidateLookupColumns(AttributeModel attributeModel, String value, String parameterName, Boolean validateDelimiter, IDataModelOperationResult operationResult, CallerContext callerContext)
        {
            String bracketOpen = "[";
            String bracketClosed = "]";
            String delimiter = ",";
            Int32 positionBracketOpen = 0, positionBracketClosed = 0;
            Collection<String> columnsArray = new Collection<String>();

            if (value.IndexOf("'") >= 0)
            {
                AddOperationResult(operationResult, "113941", String.Format("Invalid '{0}' value provided for attribute '{1}' and attribute parent '{2}'. Value cannot contain single quote (') character.", parameterName, attributeModel.Name, attributeModel.AttributeParentName),
                    new Object[] { parameterName, attributeModel.Name, attributeModel.AttributeParentName }, OperationResultType.Error, TraceEventType.Error, callerContext);
            }
            else
            {
                #region Validation of lookup columns

                while ((positionBracketClosed < (value.Length - 1)))
                {
                    positionBracketOpen = value.IndexOf(bracketOpen, positionBracketClosed);
                    positionBracketClosed = value.IndexOf(bracketClosed, positionBracketOpen + 1);

                    if (positionBracketOpen != -1 && positionBracketClosed != -1)
                    {
                        Int32 length = (positionBracketClosed - positionBracketOpen) + 1;
                        columnsArray.Add(value.Substring(positionBracketOpen, length));
                    }
                    else
                    {
                        AddOperationResult(operationResult, "113950", String.Format("Invalid '{0}' value provided for attribute '{1}' and attribute parent '{2}'. Value should be in a correct format using open and close brackets. Example: '[Value],[Code]'", parameterName, attributeModel.Name, attributeModel.AttributeParentName),
                            new Object[] { parameterName, attributeModel.Name, attributeModel.AttributeParentName }, OperationResultType.Error, TraceEventType.Error, callerContext);
                        break;
                    }
                }

                #endregion Validation of lookup columns
            }

            #region Validation of lookup column for delimiter
            if (validateDelimiter && columnsArray.Count > 0)
            {
                String properString = String.Empty;
                for (Int32 columnArrayAddedByUser = 0; columnArrayAddedByUser < columnsArray.Count; columnArrayAddedByUser++)
                {
                    properString = String.Format("{0}{1}{2}", properString, columnsArray[columnArrayAddedByUser], delimiter);
                }
                properString = properString.Substring(0, properString.Length - delimiter.Length);
                if (String.Compare(value, properString, StringComparison.InvariantCultureIgnoreCase) != 0)
                {
                    AddOperationResult(operationResult, "113953", String.Format("Invalid value in '{0}' provided for attribute '{1}' and attribute parent '{2}'. Specify the values using delimiter '{3}'.", parameterName, attributeModel.Name, attributeModel.AttributeParentName, delimiter),
                        new Object[] { parameterName, attributeModel.Name, attributeModel.AttributeParentName, delimiter }, OperationResultType.Error, TraceEventType.Error, callerContext);
                }
            }

            #endregion Validation of lookup column for delimiter
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeModel"></param>
        /// <param name="operationResult"></param>
        /// <param name="callerContext"></param>
        private void ValidateSortOrder(AttributeModel attributeModel, IDataModelOperationResult operationResult, CallerContext callerContext)
        {
            //Here this one added because user can add string value also while data model import
            Int32 sortOrder = ValueTypeHelper.Int32TryParse(attributeModel.SortOrder.ToString(), -1);

            if (sortOrder < 0)
            {
                AddOperationResult(operationResult, "113939", String.Format("Invalid '{0}' provided for attribute '{1}' and attribute parent '{2}'. Value should be non-negative integer.", "Sort Order", attributeModel.Name, attributeModel.AttributeParentName),
                                    new Object[] { "Sort Order", attributeModel.Name, attributeModel.AttributeParentName }, OperationResultType.Error, TraceEventType.Error, callerContext);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeModel"></param>
        /// <param name="operationResult"></param>
        /// <param name="callerContext"></param>
        private void ValidateRangeAndDefaultValues(AttributeModel attributeModel, IDataModelOperationResult operationResult, CallerContext callerContext)
        {
            String attributeDataTypeName = attributeModel.AttributeDataTypeName;
            Boolean checkFrom = false;
            Boolean checkTo = false;

            AttributeDataType attributeDataType;
            if (ValueTypeHelper.EnumTryParse(attributeDataTypeName, true, out attributeDataType))
            {
                switch (attributeDataType)
                {
                    case AttributeDataType.Decimal:
                    case AttributeDataType.Integer:
                    case AttributeDataType.Fraction:
                        ValidateRangeAndDefaultValues<Decimal>(-1, -1, attributeModel, operationResult, callerContext);
                        break;
                    case AttributeDataType.Date:
                    case AttributeDataType.DateTime:
                        #region Date and date time validation for range from and range to

                        DateTime dtRangeFrom = DateTime.MinValue;
                        DateTime dtRangeTo = DateTime.MaxValue;

                        ValidateRangeAndDefaultValues<DateTime>(dtRangeFrom, dtRangeTo, attributeModel, operationResult, callerContext);

                        #endregion Date and date time validation for range from and range to
                        break;
                    case AttributeDataType.String:
                        if ((String.Compare(attributeModel.AttributeDisplayTypeName, AttributeDisplayType.DropDown.ToString(), StringComparison.InvariantCultureIgnoreCase)) == 0)
                        {
                            if (String.IsNullOrWhiteSpace(attributeModel.AllowableValues))
                            {
                                AddOperationResult(operationResult, "114017", String.Format("Allowable values are empty or not specified for attribute '{0}' and attribute parent '{1}'.", attributeModel.Name, attributeModel.AttributeParentName), new Object[] { attributeModel.Name, attributeModel.AttributeParentName }, OperationResultType.Error, TraceEventType.Error, callerContext);
                            }
                            else if (!String.IsNullOrWhiteSpace(attributeModel.DefaultValue))
                            {
                                ValidateAttributeDefaultValueWithAllowableValues(attributeModel, operationResult, callerContext);
                            }
                        }

                        if ((String.Compare(attributeModel.AttributeDisplayTypeName, AttributeDisplayType.LookupTable.ToString(), StringComparison.InvariantCultureIgnoreCase)) != 0)
                        {
                            #region String validation for min length and max length

                            Int32 minLength = ValueTypeHelper.Int32TryParse(attributeModel.MinLength.ToString(), -1);
                            Int32 maxLength = ValueTypeHelper.Int32TryParse(attributeModel.MaxLength.ToString(), -1);

                            #region Min length validation

                            if (minLength < 0)
                            {
                                AddOperationResult(operationResult, "113939", String.Format("Invalid '{0}' provided for attribute '{1}' and attribute parent '{2}'. Value should be non-negative integer.", "Min Length", attributeModel.Name, attributeModel.AttributeParentName),
                                    new Object[] { "Min Length", attributeModel.Name, attributeModel.AttributeParentName }, OperationResultType.Error, TraceEventType.Error, callerContext);
                            }
                            else if (minLength > 0)
                            {
                                checkFrom = true;
                            }

                            #endregion Min length validation

                            #region Max length validation

                            if (maxLength < 0)
                            {
                                AddOperationResult(operationResult, "113939", String.Format("Invalid '{0}' provided for attribute '{1}' and attribute parent '{2}'. Value should be non-negative integer.", "Max Length", attributeModel.Name, attributeModel.AttributeParentName),
                                    new Object[] { "Max Length", attributeModel.Name, attributeModel.AttributeParentName }, OperationResultType.Error, TraceEventType.Error, callerContext);
                            }
                            else if (maxLength > 0)
                            {
                                checkTo = true;
                            }

                            #endregion Max length validation

                            if ((checkFrom && checkTo) && minLength > maxLength)
                            {
                                AddOperationResult(operationResult, "113954", String.Format("'{0}' value cannot be greater than '{1}' value for attribute '{2}' and attribute parent '{3}'", "Min Length", "Max Length", attributeModel.Name, attributeModel.AttributeParentName),
                                   new Object[] { "Min Length", "Max Length", attributeModel.Name, attributeModel.AttributeParentName }, OperationResultType.Error, TraceEventType.Error, callerContext);
                            }
                            else if (!String.IsNullOrWhiteSpace(attributeModel.DefaultValue))
                            {
                                if ((checkFrom && attributeModel.DefaultValue.Length < minLength) || (checkTo && attributeModel.DefaultValue.Length > maxLength))
                                {
                                    AddOperationResultForInvalidValueOutOfRange(attributeModel, operationResult, callerContext, attributeModel.MinLength.ToString(), attributeModel.MaxLength.ToString());
                                }
                            }

                            #endregion String validation for min length and max length
                        }
                        else if (_dataModelValidationOptions != null && _dataModelValidationOptions.ValidateLookupDependencies)
                        {
                            ValidateLookupDefaultValue(attributeModel, operationResult, callerContext);
                        }

                        break;
                    case AttributeDataType.Boolean:
                        if (!String.IsNullOrWhiteSpace(attributeModel.DefaultValue))
                        {
                            if (!ValidateAttributeModelValues(attributeModel, attributeModel.DefaultValue))
                            {
                                AddOperationResult(operationResult, "113943", String.Format("Invalid '{0}' value provided for attribute '{1}' and attribute parent '{2}'. Value should contain any of the following: {3}", attributeModel.AttributeDataTypeName, attributeModel.Name, attributeModel.AttributeParentName, "0 or 1, true or false"),
                                    new Object[] { attributeModel.AttributeDataTypeName, attributeModel.Name, attributeModel.AttributeParentName, "0 or 1, true or false" }, OperationResultType.Error, TraceEventType.Error, callerContext);
                            }
                        }
                        break;
                    case AttributeDataType.URL:
                        if (!String.IsNullOrWhiteSpace(attributeModel.DefaultValue))
                        {
                            if (!ValidateAttributeModelValues(attributeModel, attributeModel.DefaultValue))
                            {
                                AddOperationResult(operationResult, "113943", String.Format("Invalid '{0}' value provided for attribute '{1}' and attribute parent '{2}'. Value should contain any of the following: {3}", attributeModel.AttributeDataTypeName, attributeModel.Name, attributeModel.AttributeParentName, "http:|https:|ftp://www.(url)"),
                                    new Object[] { attributeModel.AttributeDataTypeName, attributeModel.Name, attributeModel.AttributeParentName, "http:|https:|ftp://www.(url)" }, OperationResultType.Error, TraceEventType.Error, callerContext);
                            }
                        }
                        break;
                    case AttributeDataType.Image:
                    case AttributeDataType.ImageURL:
                        if (!String.IsNullOrWhiteSpace(attributeModel.DefaultValue))
                        {
                            if (!ValidateAttributeModelValues(attributeModel, attributeModel.DefaultValue))
                            {
                                AddOperationResult(operationResult, "113940", String.Format("Invalid image file provided for attribute '{0}' and attribute parent '{1}'. File should contain any of the following extensions: {2}", attributeModel.Name, attributeModel.AttributeParentName, "jpg,jpeg,gif,bmp,png,tiff,exif"),
                                    new Object[] { attributeModel.Name, attributeModel.AttributeParentName, "jpg,jpeg,gif,bmp,png,tiff,exif" }, OperationResultType.Error, TraceEventType.Error, callerContext);
                            }
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeModel"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private Boolean ValidateAttributeModelValues(AttributeModel attributeModel, String value)
        {
            String attributeDataTypeName = attributeModel.AttributeDataTypeName;
            Boolean hasValidValues = true;

            AttributeDataType attributeDataType;
            if (ValueTypeHelper.EnumTryParse(attributeDataTypeName, true, out attributeDataType))
            {
                try
                {
                    //Get Culture Info
                    CultureInfo cultureInfo = new CultureInfo(attributeModel.Locale.GetCultureName());

                    switch (attributeDataType)
                    {
                        case AttributeDataType.Date:
                            if (!value.Trim().ToLowerInvariant().Equals("[system date]"))
                            {
                                Regex dateRegex = new Regex(Constants.DATE_VALIDATION_EXPRESSION);

                                if (!dateRegex.IsMatch(value))
                                {
                                    hasValidValues = false;
                                }
                                else
                                {
                                    DateTime.Parse(value, cultureInfo.DateTimeFormat);
                                }
                            }
                            break;
                        case AttributeDataType.DateTime:
                            if (!value.Trim().ToLowerInvariant().Equals("[system date]"))
                            {
                                DateTime.Parse(value, cultureInfo.DateTimeFormat);
                            }
                            break;
                        case AttributeDataType.Integer:
                            Int32.Parse(value);
                            break;
                        case AttributeDataType.Decimal:
                            Decimal.Parse(value, NumberStyles.AllowDecimalPoint, cultureInfo);

                            // Regex below doesn't allow integers and does't validate decimal max size. 
                            // Left here for reference
                            //                            String decimalSeparator = cultureInfo.NumberFormat.NumberDecimalSeparator;
                            //                            Regex decimalRegEx = new Regex(@"/^[0-9-]*([\" + decimalSeparator + "][0-9]+)?$");

                            break;
                        case AttributeDataType.Boolean:
                            String booleanValue = value.ToString().ToLowerInvariant();
                            if (!(booleanValue == "0" || booleanValue == "1" || booleanValue == "true" || booleanValue == "false"))
                            {
                                hasValidValues = false;
                            }
                            break;
                        case AttributeDataType.Image:
                            Regex imageRegEx = new Regex(Constants.IMAGE_VALIDATION_EXPRESSION);
                            if (!imageRegEx.IsMatch(value))
                            {
                                hasValidValues = false;
                            }
                            break;
                        case AttributeDataType.ImageURL:
                            Regex imageURLRegEx = new Regex(Constants.IMAGEURL_VALIDATION_EXPRESSION);
                            if (!imageURLRegEx.IsMatch(value))
                            {
                                hasValidValues = false;
                            }
                            break;
                        case AttributeDataType.URL:
                            Regex urlRegEx = new Regex(Constants.URL_VALIDATION_EXPRESSION);

                            if (!urlRegEx.IsMatch(value))
                            {
                                hasValidValues = false;
                            }
                            break;
                        case AttributeDataType.Fraction:
                            hasValidValues = ValidateFraction(value);

                            break;
                    }
                }
                catch
                {
                    hasValidValues = false;
                }
            }

            return hasValidValues;
        }

        /// <summary>
        /// Validates given value is valid fraction or not
        /// It value is not valid then it throws exception
        /// </summary>
        /// <param name="value">Indicates value which needs to validate</param>
        private Boolean ValidateFraction(String value)
        {
            Boolean hasValidValues = true;

            try
            {
                //Validate for fraction 
                Regex fractionRegEx = new Regex(Constants.MIXED_FRACTION_VALIDATION_EXPRESSION);

                if (!fractionRegEx.IsMatch(value))
                {
                    //It is not the mixed fraction. Check for valid whole number or fraction
                    fractionRegEx = new Regex(Constants.WHOLENUMBER_FRACTION_VALIDATION_EXPRESSION);

                    if (!fractionRegEx.IsMatch(value))
                    {
                        hasValidValues = false;
                    }

                    //For scenerio when denominator is zero.
                    String[] partsOfFraction = value.Trim().Split('/');
                    if (partsOfFraction != null && partsOfFraction.Length > 1)
                    {
                        if (partsOfFraction[0] != null)
                        {
                            Int64 numerator = 0;
                            Int64.TryParse(partsOfFraction[0], out numerator);

                            //if numerator is zero, then also it is invalid fraction so exception should be thrown.
                            if (numerator == 0)
                            {
                                hasValidValues = false;
                            }
                        }

                        if (partsOfFraction[1] != null)
                        {
                            Int64 denominator = 0;
                            Int64.TryParse(partsOfFraction[1], out denominator);

                            //if denominator is zero, then also it is invalid fraction so exception should be thrown.
                            if (denominator <= 0)
                            {
                                hasValidValues = false;
                            }
                        }

                    }
                }
                else
                {
                    //It is a mixed fraction. Check the fractional part is a proper fraction
                    String fractionalPart = value.Substring(value.IndexOf(' ') + 1);

                    String[] partsOfFraction = fractionalPart.Trim().Split('/');

                    Int64 numerator = 0;
                    Int64 denominator = 0;

                    if (partsOfFraction != null && partsOfFraction[0] != null && partsOfFraction[1] != null)
                    {
                        Int64.TryParse(partsOfFraction[0], out numerator);
                        Int64.TryParse(partsOfFraction[1], out denominator);

                        if (numerator >= denominator || numerator < 1)
                        {
                            hasValidValues = false;
                        }
                    }
                }
            }
            catch
            {
                hasValidValues = false;
            }

            return hasValidValues;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeModel"></param>
        /// <param name="operationResult"></param>
        /// <param name="callerContext"></param>
        /// <param name="parameterName"></param>
        /// <param name="dataTypeName"></param>
        private void AddOperationResultForInvalidValue(AttributeModel attributeModel, IOperationResult operationResult, CallerContext callerContext, String parameterName, String dataTypeName)
        {
            AddOperationResult(operationResult, "113951", String.Format("Invalid '{0}' provided for attribute '{1}' and attribute parent '{2}'. Value should be valid '{3}'.", parameterName, attributeModel.Name, attributeModel.AttributeParentName, dataTypeName),
                new Object[] { parameterName, attributeModel.Name, attributeModel.AttributeParentName, dataTypeName }, OperationResultType.Error, TraceEventType.Error, callerContext);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeModel"></param>
        /// <param name="operationResult"></param>
        /// <param name="callerContext"></param>
        /// <param name="fromRange"></param>
        /// <param name="toRange"></param>
        private void AddOperationResultForInvalidValueOutOfRange(AttributeModel attributeModel, IOperationResult operationResult, CallerContext callerContext, String fromRange, String toRange)
        {
            AddOperationResult(operationResult, "113942", String.Format("Invalid '{0}' provided for attribute '{1}' and attribute parent '{2}'. Value should be in the range of {3}-{4}.", "Default Value", attributeModel.Name, attributeModel.AttributeParentName, fromRange, toRange),
                new Object[] { "Default Value", attributeModel.Name, attributeModel.AttributeParentName, fromRange, toRange }, OperationResultType.Error, TraceEventType.Error, callerContext);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TDataType"></typeparam>
        /// <param name="rangeFrom"></param>
        /// <param name="rangeTo"></param>
        /// <param name="attributeModel"></param>
        /// <param name="operationResult"></param>
        /// <param name="callerContext"></param>
        private void ValidateRangeAndDefaultValues<TDataType>(TDataType rangeFrom, TDataType rangeTo, AttributeModel attributeModel, IOperationResult operationResult, CallerContext callerContext) where TDataType : IComparable
        {
            TDataType settingRangeFrom = rangeFrom;
            TDataType settingRangeTo = rangeTo;
            Boolean checkFrom = false, checkTo = false;

            // either Inclusive or Exclusive will be initialized at the time, not both
            String lowerBoundary = !String.IsNullOrWhiteSpace(attributeModel.MinInclusive)
                ? attributeModel.MinInclusive
                : attributeModel.MinExclusive;

            String upperBoundary = !String.IsNullOrWhiteSpace(attributeModel.MaxInclusive)
                ? attributeModel.MaxInclusive
                : attributeModel.MaxExclusive;

            if (!String.IsNullOrWhiteSpace(lowerBoundary))
            {
                if (!ValidateAttributeModelValues(attributeModel, lowerBoundary.Trim()))
                {
                    AddOperationResultForInvalidValue(attributeModel, operationResult, callerContext, "Range From", attributeModel.AttributeDataTypeName);
                }
                else
                {
                    checkFrom = true;
                    settingRangeFrom = (TDataType)Convert.ChangeType(attributeModel.RangeFrom, typeof(TDataType));
                }
            }

            if (!String.IsNullOrWhiteSpace(upperBoundary))
            {
                if (!ValidateAttributeModelValues(attributeModel, upperBoundary.Trim()))
                {
                    AddOperationResultForInvalidValue(attributeModel, operationResult, callerContext, "Range To", attributeModel.AttributeDataTypeName);
                }
                else
                {
                    checkTo = true;
                    settingRangeTo = (TDataType)Convert.ChangeType(attributeModel.RangeTo, typeof(TDataType));
                }
            }
            if ((checkFrom && checkTo) && settingRangeFrom.CompareTo(settingRangeTo) > 0)
            {
                AddOperationResult(operationResult, "113954", String.Format("'{0}' value cannot be greater than '{1}' value for attribute '{2}' and attribute parent '{3}'", "Range From", "Range To", attributeModel.Name, attributeModel.AttributeParentName),
                   new Object[] { "Range From", "Range To", attributeModel.Name, attributeModel.AttributeParentName }, OperationResultType.Error, TraceEventType.Error, callerContext);
            }
            else if (!String.IsNullOrWhiteSpace(attributeModel.DefaultValue))
            {
                if (!ValidateAttributeModelValues(attributeModel, attributeModel.DefaultValue))
                {
                    if (attributeModel.AttributeDataTypeName == AttributeDataType.Fraction.ToString())
                    {
                        AddOperationResult(operationResult, "113945", String.Format("Invalid default value provided for attribute '{0}' and attribute parent '{1}'. Value should be a valid fraction. Example: 1/2, 3/2, 3 1/2", attributeModel.Name, attributeModel.AttributeParentName),
                            new Object[] { attributeModel.Name, attributeModel.AttributeParentName }, OperationResultType.Error, TraceEventType.Error, callerContext);
                    }
                    else
                    {
                        AddOperationResult(operationResult, "113948", String.Format("Invalid default value for data type '{0}' provided for attribute '{1}' and attribute parent '{2}'", attributeModel.AttributeDataTypeName, attributeModel.Name, attributeModel.AttributeParentName),
                            new Object[] { attributeModel.AttributeDataTypeName, attributeModel.Name, attributeModel.AttributeParentName }, OperationResultType.Error, TraceEventType.Error, callerContext);
                    }
                }
                else
                {
                    TDataType defaultValue;

                    if (attributeModel.AttributeDataTypeName == AttributeDataType.Fraction.ToString())
                    {
                        defaultValue = (TDataType)Convert.ChangeType(ValueTypeHelper.ConvertFractionToDecimal(attributeModel.DefaultValue), typeof(TDataType));
                    }
                    else
                    {
                        if (attributeModel.DefaultValue.Trim().ToLowerInvariant().Equals("[system date]")
                            && (attributeModel.AttributeDataTypeName.Equals(AttributeDataType.Date.ToString()) || attributeModel.AttributeDataTypeName.Equals(AttributeDataType.DateTime.ToString())))
                        {
                            defaultValue = attributeModel.AttributeDataTypeName.Equals(AttributeDataType.Date.ToString()) ?
                                (TDataType)Convert.ChangeType(DateTime.Now.ToShortDateString(), typeof(TDataType)) : (TDataType)Convert.ChangeType(DateTime.Now.ToString(Constants.GENERAL_LONG_DATE_FORMAT), typeof(TDataType));
                        }
                        else
                        {
                            defaultValue = (TDataType)Convert.ChangeType(attributeModel.DefaultValue, typeof(TDataType));
                        }
                    }
                    if (((checkFrom && defaultValue.CompareTo(settingRangeFrom) <= 0) || (checkTo && defaultValue.CompareTo(settingRangeTo) >= 0)))
                    {
                        AddOperationResultForInvalidValueOutOfRange(attributeModel, operationResult, callerContext, attributeModel.RangeFrom, attributeModel.RangeTo);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeModel"></param>
        /// <param name="operationResult"></param>
        /// <param name="callerContext"></param>
        private void ValidateLocalization(AttributeModel attributeModel, IDataModelOperationResult operationResult, CallerContext callerContext)
        {
            Boolean isInvalidLocalization = false;

            AttributeDataType attributeDataType = (AttributeDataType)attributeModel.AttributeDataTypeId;

            if (attributeDataType == AttributeDataType.String)
            {
                AttributeDisplayType attributeDisplayType = (AttributeDisplayType)attributeModel.AttributeDisplayTypeId;

                if (attributeDisplayType == AttributeDisplayType.LookupTable || attributeDisplayType == AttributeDisplayType.DropDown)
                {
                    if (attributeModel.IsLocalizable)
                    {
                        isInvalidLocalization = true;
                    }
                }
            }

            if (attributeDataType == AttributeDataType.Boolean || attributeDataType == AttributeDataType.URL ||
                attributeDataType == AttributeDataType.Image || attributeDataType == AttributeDataType.File)
            {
                if (attributeModel.IsLocalizable)
                {
                    isInvalidLocalization = true;
                }
            }

            if (isInvalidLocalization)
            {
                AddOperationResult(operationResult, "114027", String.Format("Localization is not supported for the attribute '{0}' under '{1}' with the data type: '{2}' and display type: '{3}'.", attributeModel.Name, attributeModel.AttributeParentName, attributeModel.AttributeDataTypeName, attributeModel.AttributeDisplayTypeName), new Object[] { attributeModel.Name, attributeModel.AttributeParentName, attributeModel.AttributeDataTypeName, attributeModel.AttributeDisplayTypeName }, OperationResultType.Error, TraceEventType.Error, callerContext);
            }
        }

        #endregion

        #region EntityCacheLoadContext For AttributeModel base properties

        /// <summary>
        /// Processes the EntityCacheLoadContext for attribute model base properties change.
        /// </summary>
        private void ProcessEntityCacheLoadContextForAttributeModelBasePropertyChange(Collection<Int32> attributeModelList, CallerContext callerContext)
        {
            // Create EntityCacheLoadContext entityCacheContextRequest
            String entityCacheLoadContext = CreateEntityCacheLoadContextForAttributeModel(attributeModelList);

            EntityActivityLog entityActivityLog = new EntityActivityLog()
            {
                PerformedAction = EntityActivityList.EntityCacheLoad,
                Context = entityCacheLoadContext
            };

            EntityActivityLogCollection entityActivityLogCollection = new EntityActivityLogCollection() { entityActivityLog };

            EntityActivityLogBL entityActivityLogBL = new EntityActivityLogBL();
            entityActivityLogBL.Process(entityActivityLogCollection, callerContext);
        }

        /// <summary>
        /// Builds and returns the EntityCacheLoadContext in an XML form based on the attribute model. 
        /// </summary>
        private String CreateEntityCacheLoadContextForAttributeModel(Collection<Int32> attributeModelList)
        {
            // Create EntityCacheLoadContextItemCollection
            EntityCacheLoadContextItemCollection entityCacheLoadContextItemCollection = new EntityCacheLoadContextItemCollection();

            // Create EntityCacheLoadContextItem for attribute model
            EntityCacheLoadContextItem entityCacheLoadContextItemForAttributeModel =
                entityCacheLoadContextItemCollection.GetOrCreateEntityCacheLoadContextItem(EntityCacheLoadContextTypeEnum.Attribute);
            entityCacheLoadContextItemForAttributeModel.AddValues(attributeModelList);

            // Create EntityCacheLoadContext with the above parameters
            EntityCacheLoadContext entityCacheLoadContext = new EntityCacheLoadContext();
            entityCacheLoadContext.CacheStatus = (Int32)(EntityCacheComponentEnum.InheritedAttributes | EntityCacheComponentEnum.OverriddenAttributes);
            entityCacheLoadContext.Add(entityCacheLoadContextItemCollection);

            // Generate XML from the object
            String entityCacheLoadContextAsString = entityCacheLoadContext.ToXml();
            return entityCacheLoadContextAsString;
        }

        /// <summary>
        /// Log changes to tb_datamodelactivitylog if DA call is successful and appconfig key datamodelactivitylog.enable == true
        /// </summary>
        /// <param name="attributeModelCollection">Indicates attribute model collection to be logged</param>
        /// <param name="callerContext">Indicates caller context</param>
        /// <param name="baseAttributeModels">Indicates original attribute models</param>
        private void LogDataModelChanges(AttributeModelCollection attributeModelCollection, CallerContext callerContext, Dictionary<Int32, AttributeModelBaseProperties> baseAttributeModels = null)
        {
            #region Step: Populate datamodelactivitylog object

            DataModelActivityLogBL dataModelActivityLogManager = new DataModelActivityLogBL();
            DataModelActivityLogCollection activityLogCollection = dataModelActivityLogManager.FillDataModelActivityLogCollection(attributeModelCollection, attributeModelCollection.DataModelObjectType, callerContext, baseAttributeModels);

            #endregion Step: Populate datamodelactivitylog object

            #region Step: Make api call

            if (activityLogCollection != null && activityLogCollection.Count > 0) // null activity log collection means there was error in mapping
            {
                dataModelActivityLogManager.Process(activityLogCollection, callerContext);
            }

            #endregion Step: Make api call
        }

        #endregion

        #endregion

        #endregion
    }
}