using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Security.Principal;
using System.ServiceModel;

namespace MDM.Services
{
    using Core;
    using Core.Exceptions;
    using BusinessObjects;
    using BusinessObjects.Diagnostics;
    using BusinessObjects.Interfaces;
    using BusinessObjects.Exports;
    using Interfaces;
    using Interfaces.Exports;
    using InternalServiceProxies;
    using Utility;
    using WCFServiceInterfaces;

    /// <summary>
    /// InternalCommonService provides external program with an ability to work with MDM Entity data and even provides API methods to search entities and work with the following: 
    /// Entity master information, Entity attributes, Relationship, hierarchies (child entities) and extensions (MDLs).
    /// </summary>
    public class InternalCommonService : ServiceClientBase
    {
        #region Fields

        private Boolean _isProtoBufEndPointInitialized = false;

        #endregion

        #region Constructors

        /// <summary>
        /// Use this default constructor only when calling MDM service within the execution context of an MDM Application. 
        /// Example: Service call from the MDM business rule.
        /// This would try to use current execution security context for service authentication purpose. 
        /// </summary>
        public InternalCommonService()
            : base(typeof(InternalCommonService))
        {
        }

        /// <summary>
        /// Use this default constructor with binding configuration name only when calling MDM service within the execution context of an MDM Application. 
        /// Example: Service call from the MDM business rule.
        /// This would try to use current execution security context for service authentication purpose. 
        /// </summary>
        /// <param name="endPointConfigurationName">Indicates the EndPointConfigurationName</param>
        public InternalCommonService(String endPointConfigurationName)
            : base(typeof(InternalCommonService), endPointConfigurationName)
        {
        }

        /// <summary>
        /// Use this constructor for form based authentication passing username and password
        /// </summary>
        /// <param name="endPointConfigurationName">Indicates the EndPointConfigurationName</param>
        /// <param name="userName">Name of the Login User</param>
        /// <param name="password">Password of the User</param>
        public InternalCommonService(String endPointConfigurationName, String userName, String password)
            : base(endPointConfigurationName, userName, password)
        {

        }

        /// <summary>
        /// Use this constructor for window authentication passing current windows identity.
        /// </summary>
        /// <param name="endPointConfigurationName">Indicates the EndPointConfigurationName</param>
        /// <param name="userIdentity">Indicates User's Identity</param>
        public InternalCommonService(String endPointConfigurationName, IIdentity userIdentity)
            : base(endPointConfigurationName, userIdentity)
        {
        }

        /// <summary>
        /// Constructor with client configuration.
        /// </summary>
        /// <param name="wcfClientConfiguration">Indicates WCF Client Configuration</param>
        public InternalCommonService(IWCFClientConfiguration wcfClientConfiguration)
            : base(wcfClientConfiguration)
        {
        }

        /// <summary>
        /// Constructor with endPointConfigurationName, authenticationType, userIdentity, userName and password
        /// </summary>
        /// <param name="endPointConfigurationName">Indicates the EndPointConfigurationName</param>
        /// <param name="authenticationType">Indicates Type of Authentication i.e Windows/Forms</param>
        /// <param name="userIdentity">Indicates Identity of the Login User</param>
        /// <param name="userName">Name of the Login User</param>
        /// <param name="password">Password of the User</param>
        public InternalCommonService(String endPointConfigurationName, AuthenticationType authenticationType, IIdentity userIdentity, String userName, String password)
            : base(endPointConfigurationName, authenticationType, userIdentity, userName, password)
        {
        }

        /// <summary>
        /// Constructor with endPointConfigurationName, endPointAddress, authenticationType, userIdentity, userName and password
        /// </summary>
        /// <param name="endPointConfigurationName">Indicates the EndPointConfigurationName</param>
        /// <param name="endPointAddress"> Provides a unique network address that a client uses to communicate with a service endpoint.</param>
        /// <param name="authenticationType">Indicates Type of Authentication i.e Windows/Forms</param>
        /// <param name="userIdentity">Indicates Identity of the Login User</param>
        /// <param name="userName">Name of the Login User</param>
        /// <param name="password">Password of the User</param>
        public InternalCommonService(String endPointConfigurationName, EndpointAddress endPointAddress, AuthenticationType authenticationType, IIdentity userIdentity, String userName, String password)
            : base(endPointConfigurationName, endPointAddress, authenticationType, userIdentity, userName, password)
        {
        }

        #endregion

        #region Methods

        #region Public Methods

        #region Entity Methods

        /// <summary>
        /// Gets Entity Objects for the requested Entity IDs and paging criteria
        /// </summary>
        /// <example>
        /// <code>
        /// // Assumption: EntityId, AttributeId, ContainerId etc., are assumed in this sample
        /// // It may change based on database setup. 
        ///
        /// // Get InternalCommonService
        /// InternalCommonService internalCommonService = new InternalCommonService();
        ///
        /// // Assumption: Entity and Attribute values are hardcoded in this sample
        /// <![CDATA[Collection<Int64>]]> entityIds = new <![CDATA[Collection<Int64>]]> { 32 };
        /// <![CDATA[Collection<Int32>]]> attributeIdList = new <![CDATA[Collection<Int32>]]> { 4027 };
        ///
        /// // Get new instance of IEntityContext using MDMObjectFactory
        /// IEntityContext entityContext = MDMObjectFactory.GetIEntityContext();
        ///
        /// // "entityContext.Locale" locale for Entity (Entity's metadata will be populated in this locale). 
        /// // If entityContext.DataLocales is not populated, attribute value will be returned in this locale. 
        /// entityContext.Locale = LocaleEnum.en_WW;
        ///
        /// // Note: EntityContext properties ContainerId, EntityTypeId, CategoryId and ParentEntityId are optional to set.
        /// // Default value for all these properties is '0'.
        /// // It is HIGHLY RECOMMENDED to set below properties for better performance of API system
        ///
        /// entityContext.ContainerId = 5;
        /// entityContext.EntityTypeId = 16;
        /// entityContext.CategoryId = 1;
        /// entityContext.ParentEntityId = 1;
        ///
        /// // Populate list of locales in which attribute value is required in "entityContext.DataLocales" (locale collection).
        /// // If "entityContext.DataLocales" is not populated then entity locale from "entityContext.Locale" will be considered as attribute value locale.
        /// // If Attribute value is asked in multiple locales ("entityContext.DataLocales" has more than 1 value for locale),
        /// // then there will be multiple attribute instance (1 per locale) in entity.Attributes collection.
        /// // If attribute is not localizable, then attribute.Locale will be SDL (System Data Locale).
        ///
        /// entityContext.DataLocales.Add(LocaleEnum.en_WW);
        /// entityContext.DataLocales.Add(LocaleEnum.de_DE);
        ///
        /// // Set 'entityContext.LoadAttributes' to true to load entity attributes
        /// entityContext.LoadEntityProperties = true;
        ///
        /// // Set 'entityContext.AttributeIdList'
        /// entityContext.AttributeIdList = attributeIdList;
        /// entityContext.LoadAttributes = true;
        ///
        /// // Below code will make WCF call and try to get an entity for given entityIds and entityContext
        /// IEntityPaginationResult result = internalCommonService.GetEntitiesByEntityIdsAndPagination(entityIds, entityContext, pagingCriteria, MDMCenterApplication.PIM, MDMCenterModules.Entity);
        ///
        /// </code>
        /// </example>
        /// <param name="entityIdList">Indicates Entity IDs for which Entity Objects are required</param>
        /// <param name="iEntityContext">Indicates the data context for which Entity needs to be fetched</param>
        /// <param name="iPagingCriteria">Indicates the paging criteria for the result</param>
        /// <param name="iCallerContext">Indicates the context for the consumer/client</param>
        /// <param name="applyAVS"> Indicates whether AVS should be applied. If the value is True, AVS is applied</param>
        /// <param name="publishEvents"> Indicates whether events should be published. If the value is True, events are published</param>
        /// <returns>Returns collection of entities in the requested context</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        public IEntityPaginationResult GetEntitiesByIdsAndPagination(Collection<Int64> entityIdList, IEntityContext iEntityContext, IPagingCriteria iPagingCriteria, ICallerContext iCallerContext, Boolean publishEvents = true, Boolean applyAVS = true)
        {
            EntityContext entityContext = (EntityContext)iEntityContext;
            PagingCriteria pagingCriteria = (PagingCriteria)iPagingCriteria;

            return MakeServiceCall<IEntityPaginationResult>("GetEntitiesByIdsAndPagination", "GetEntitiesByIdsAndPagination", client =>
                client.GetEntitiesByIdsAndPagination(entityIdList, entityContext, pagingCriteria, FillDiagnosticTraces(iCallerContext), publishEvents, applyAVS));
        }

        /// <summary>
        /// Gets Entity Objects for the requested paging criteria
        /// </summary>
        /// <example>
        /// <code>
        /// // Assumption: EntityId, AttributeId, ContainerId etc., are assumed in this sample
        /// // It may change based on database setup. 
        ///
        /// // Get InternalCommonService 
        /// InternalCommonService internalCommonService = new InternalCommonService();
        ///
        /// // Assumption: Entity and Attribute values are hardcoded in this sample
        /// <![CDATA[Collection<Int64>]]> entityIds = new <![CDATA[Collection<Int64>]]> { 32 };
        /// <![CDATA[Collection<Int32>]]> attributeIdList = new <![CDATA[Collection<Int32>]]> { 4027 };
        ///
        /// // Get new instance of IEntityContext using MDMObjectFactory
        /// IEntityContext entityContext = MDMObjectFactory.GetIEntityContext();
        ///
        /// // "entityContext.Locale" locale for Entity (Entity's metadata will be populated in this locale). 
        /// // If entityContext.DataLocales is not populated, attribute value will be returned in this locale. 
        /// entityContext.Locale = LocaleEnum.en_WW;
        ///
        /// // Note: EntityContext properties ContainerId, EntityTypeId, CategoryId and ParentEntityId are optional to set.
        /// // Default value for all these properties is '0'.
        /// // It is HIGHLY RECOMMENDED to set below properties for better performance of API system
        ///
        /// entityContext.ContainerId = 5;
        /// entityContext.EntityTypeId = 16;
        /// entityContext.CategoryId = 1;
        /// entityContext.ParentEntityId = 1;
        ///
        /// // Populate list of locales in which attribute value is required in "entityContext.DataLocales" (locale collection).
        /// // If "entityContext.DataLocales" is not populated then entity locale from "entityContext.Locale" will be considered as attribute value locale.
        /// // If Attribute value is asked in multiple locales ("entityContext.DataLocales" has more than 1 value for locale),
        /// // then there will be multiple attribute instance (1 per locale) in entity.Attributes collection.
        /// // If attribute is not localizable, then attribute.Locale will be SDL (System Data Locale).
        ///
        /// entityContext.DataLocales.Add(LocaleEnum.en_WW);
        /// entityContext.DataLocales.Add(LocaleEnum.de_DE);
        ///
        /// // Set 'entityContext.LoadAttributes' to true to load entity attributes
        /// entityContext.LoadEntityProperties = true;
        ///
        /// // Set 'entityContext.AttributeIdList'
        /// entityContext.AttributeIdList = attributeIdList;
        /// entityContext.LoadAttributes = true;
        ///
        /// // Below code will make WCF call and try to get an entity for given entityIds and entityContext
        /// IEntityPaginationResult result = internalCommonService.GetEntitiesByPagination(entityContext, pagingCriteria, MDMCenterApplication.PIM, MDMCenterModules.Entity);
        ///
        /// </code>
        /// </example>
        /// <param name="iEntityContext">Indicates the data context for which Entity needs to be fetched</param>
        /// <param name="iPagingCriteria">Indicates the paging criteria for the result</param>
        /// <param name="iCallerContext">Indicates the context for the consumer/client</param>
        /// <param name="applyAVS"> Indicates whether AVS should be applied. If the value is True, AVS is applied</param>
        /// <param name="publishEvents"> Indicates whether events should be published. If the value is True, events are published</param>
        /// <returns>Returns collection of entities in the requested context</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        public IEntityPaginationResult GetEntitiesByPagination(IEntityContext iEntityContext, IPagingCriteria iPagingCriteria, ICallerContext iCallerContext, Boolean publishEvents = true, Boolean applyAVS = true)
        {
            EntityContext entityContext = (EntityContext)iEntityContext;
            PagingCriteria pagingCriteria = (PagingCriteria)iPagingCriteria;

            return MakeServiceCall<IEntityPaginationResult>("GetEntitiesByPagination", "GetEntitiesByPagination", client =>
                client.GetEntitiesByPagination(entityContext, pagingCriteria, FillDiagnosticTraces(iCallerContext), publishEvents, applyAVS));
        }

        /// <summary>
        /// Gets entity objects for the requested entity scopes
        /// </summary>
        /// <param name="entityScopeCollection">Indicates the entity scopes for which the entities are retrieved</param>
        /// <param name="entityGetOptions">Indicates the options available while retrieving entity data</param>
        /// <param name="callerContext">Indicates the caller context, which contains the application and module that has invoked the API</param>
        /// <returns>Returns a collection of entities which were retrieved based on the entity scope and a collection of failed list of results for the entities which were not retrieved</returns>
        /// <exception cref="MDMOperationException">Thrown if entityScopeCollection is null</exception>
        /// <exception cref="MDMOperationException">Thrown if none of the EntityContext's properties like LoadEntityProperties, LoadAttributes or LoadRelationships is set to true</exception>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        public IEntityReadResult GetEntitiesByEnityScopes(IEntityScopeCollection entityScopeCollection, IEntityGetOptions entityGetOptions, ICallerContext callerContext)
        {
            return MakeServiceCall("GetEntities",
                                   "GetEntities",
                                   client => client.GetEntitiesByEntityScopes(
                                       (EntityScopeCollection)entityScopeCollection,
                                       (EntityGetOptions)entityGetOptions,
                                       FillDiagnosticTraces(callerContext)),
                                   MDMTraceSource.EntityGet);
        }

        /// <summary>
        /// Creates Entity that has Image Attributes
        /// </summary>
        /// <param name="iEntity">Indicates the instance of entities to be processed in the MDM system</param>
        /// <param name="iImageDetails">Indicates instance of the Image details to be processed</param>
        /// <param name="programName">Indicates the name of the program from where the Process method is being called. This is used for log purpose.</param>
        /// <param name="application">Indicates the name of the application which is performing the action</param>
        /// <param name="module">Indicates the name of the module which is performing the action</param>
        /// <returns>Returns results of the operation having errors and information, if any</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        public IEntityOperationResult CreateEntityHavingImageAttributes(IEntity iEntity, Dictionary<KeyValuePair<Int32, LocaleEnum>, IFile> iImageDetails, String programName, MDMCenterApplication application, MDMCenterModules module)
        {
            Entity entity = (Entity)iEntity;

            Dictionary<KeyValuePair<int, LocaleEnum>, File> imageDetails = new Dictionary<KeyValuePair<Int32, LocaleEnum>, File>();

            //Loop through the key value pair in dictionary and cast IFile into File object
            foreach (KeyValuePair<KeyValuePair<Int32, LocaleEnum>, IFile> pair in iImageDetails)
            {
               if (pair.Key.Key > 0 && pair.Value != null)
                {
                    //imageDetails.Add(pair.Key, ( File ) pair.Value);
                    imageDetails.Add(new KeyValuePair<int, LocaleEnum>(pair.Key.Key, pair.Key.Value), (File)pair.Value);
                }
            }

            return MakeServiceCall<IEntityOperationResult>("CreateEntityHavingImageAttributes", "CreateEntityHavingImageAttributes", client => client.CreateEntityHavingImageAttributes(entity, imageDetails, programName, application, module));
        }
        
        /// <summary>
        /// Updates an Entity with Image Attributes
        /// </summary>
        /// <param name="iEntity">Indicates the Entity Object with attributes</param>
        /// <param name="iImageDetails">Indicates Image Details: A Dictionary with the Attribute ID as key and the file object as value</param>
        /// <param name="programName">Indicates the name of the Program from where the Process method is being called. This is used for log purpose.</param>
        /// <param name="application">Indicates the name of the application which is performing the action</param>
        /// <param name="module">Indicates the name of the module which is performing the action</param>
        /// <returns>Returns the Operation Result indicating Entity Updated or not</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        public IEntityOperationResult UpdateEntityHavingImageAttributes(IEntity iEntity, Dictionary<KeyValuePair<Int32, LocaleEnum>, IFile> iImageDetails, String programName, MDMCenterApplication application, MDMCenterModules module)
        {
            Entity entity = (Entity)iEntity;

            Dictionary<KeyValuePair<int, LocaleEnum>, File> imageDetails = new Dictionary<KeyValuePair<Int32, LocaleEnum>, File>();

            //Loop through the key value pair in dictionary and cast IFile into File object
            foreach (KeyValuePair<KeyValuePair<Int32, LocaleEnum>, IFile> pair in iImageDetails)
            {
                if (pair.Key.Key > 0 && pair.Value != null)
                {
                    //imageDetails.Add(pair.Key, ( File ) pair.Value);
                    imageDetails.Add(new KeyValuePair<int, LocaleEnum>(pair.Key.Key, pair.Key.Value), (File)pair.Value);
                }
            }

            return MakeServiceCall<IEntityOperationResult>("UpdateEntityHavingImageAttributes", "UpdateEntityHavingImageAttributes", client => client.UpdateEntityHavingImageAttributes(entity, imageDetails, programName, application, module));
        }

        /// <summary>
        /// Gets the completion status for Entity Views based on criterion defined in the Entity View XML
        /// </summary>
        /// <param name="entityId">Indicates the Entity ID for which the status needs to be determined</param>
        /// <param name="userId">Indicates the User ID for which the status needs to be determined</param>
        /// <param name="entityViewXml">Indicates the Entity Editor left panel Config XML containing Views, and the completion criteria</param>
        /// <param name="iEntityContext">Indicates the data context for which Entity needs to be fetched</param>
        /// <param name="isRecalculationRequired">Indicates whether the completion status needs to be recalculated or fetched from the cache</param>
        /// <param name="application">Indicates the name of the application which is performing the action</param>
        /// <param name="module">Indicates the name of the module which is performing the action</param>
        /// <returns>Collection of Entity Views with the completion status</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        public IEntityViewCollection GetEntityViewCompletionStatus(Int64 entityId, Int32 userId, String entityViewXml, IEntityContext iEntityContext, Boolean isRecalculationRequired, MDMCenterApplication application, MDMCenterModules module)
        {
            EntityContext entityContext = (EntityContext)iEntityContext;

            return MakeServiceCall<IEntityViewCollection>("GetEntityViewCompletionStatus", "GetEntityViewCompletionStatus", client => client.GetEntityViewCompletionStatus(entityId, userId, entityViewXml, entityContext, isRecalculationRequired, application, module));
        }

        /// <summary>
        /// Checks whether the requested entity exists or not
        /// </summary>
        /// <example>
        /// <code>
        /// 
        /// // Get MDM data service
        /// DataService mdmDataService = GetMDMDataService();
        /// 
        /// // Create an entityContext object 
        /// // The Id's are as per River Works Data Model
        /// 
        /// // Get new instance of IEntityContext using MDMObjectFactory
        /// IEntityContext entityContext = <![CDATA[MDMObjectFactory.GetIEntityContext()]]>;
        /// 
        /// // "entityContext.Locale" is the locale for Entity (Entity's metadata will be populated in this locale). 
        /// // If entityContext.DataLocales is not populated, then attribute value will be returned in this locale. 
        /// entityContext.Locale = LocaleEnum.en_WW;
        /// 
        /// // Making a call to GetEntity to return the Entity object and then passing to IsEntityExists to check if it exists or not
        /// Int64 entityId = 43046; //Entity of the type KitCode as per River Works data model
        /// 
        /// // Set 'entityContext.LoadAttributes' to true to load entity attributes
        /// entityContext.LoadEntityProperties = true;
        /// 
        /// // Set 'entityContext.LoadRequiredAttributes' property to get an entity with all attributes marked as 'ShowAtCreation'
        /// entityContext.LoadCreationAttributes = true;
        /// 
        /// // Below will make WCF call and try to get an entity for given entityId and entityContext
        /// IEntity entity = mdmDataService.GetEntity(entityId, entityContext, callerApplication, callerModule);
        /// 
        /// // Now pass this entity object to IsEntityExists method
        /// // Below will make WCF call checks that the requested entity exists or not
        /// Boolean isEntityExists = mdmDataService.IsEntityExists(entity, entity.Id, entity.ContainerId, MDMCenterApplication.PIM, MDMCenterModules.Entity);
        /// </code>
        /// </example>
        /// <param name="iEntity">Indicates the Entity object to be checked</param>
        /// <param name="entityId">Indicates the id of the Entity to be requested</param>
        /// <param name="catalogId">Indicates the container/catalog where the entity is placed </param>
        /// <param name="application">Indicates the name of the application</param>
        /// <param name="module">Indicates the name of the module</param>
        /// <returns>Returns the status whether the entity exists or not</returns>
        public Boolean IsEntityExists(IEntity iEntity, Int64 entityId, Int32 catalogId, MDMCenterApplication application, MDMCenterModules module)
        {
            return MakeServiceCall("IsEntityExists", "IsEntityExists", client => client.IsEntityExists(iEntity as Entity, entityId, catalogId, application, module));
        }

        /// <summary>
        /// Gets entity validation scores
        /// </summary>
        /// <param name="entityIds">Indicates entity ids to get their validation scores</param>
        /// <param name="iCallerContext">Indicates the caller context</param>
        /// <returns></returns>
        public IEntityStateValidationScoreCollection GetEntityScores(Collection<Int64> entityIds, ICallerContext iCallerContext)
        {
            return this.MakeServiceCall<EntityStateValidationScoreCollection>("GetEntityScores", "GetEntityScores",
                            client => client.GetEntityScores(entityIds, iCallerContext as CallerContext), MDMTraceSource.Entity);
        }

        #endregion Entity Methods

        #region DataModel Methods

        /// <summary>
        /// Generates an excel file with all data models present in the system.
        /// </summary>
        /// <param name="iDataModelExportContext">Indicates context for data model export</param>
        /// <param name="iCallerContext">Indicates the context for the consumer/client</param>
        /// <returns>A file containing the data model</returns>
        public IFile ExportDataModelAsExcel(IDataModelExportContext iDataModelExportContext, ICallerContext iCallerContext)
        {
            return MakeServiceCall<IFile>("ExportDataModelAsExcel", "ExportDataModelAsExcel", client => client.ExportDataModelAsExcel((DataModelExportContext)iDataModelExportContext, FillDiagnosticTraces(iCallerContext)));
        }

        #endregion DataModel Methods

        #region Category Methods

        /// <summary>
        /// Search Categories for requested context
        /// </summary>
        /// <param name="containerNames">Collection of container names in which the category belongs</param>
        /// <param name="categoryLongNames">Collection of category long names in which result has to be returned</param>
        /// <param name="dataLocales">DataLocales in which result has to be returned.</param>
        /// <param name="iCallerContext">Context indicating the caller of the API</param>
        /// <returns>Collection of Category</returns>
        public ICategoryCollection SearchCategories(Collection<String> containerNames, Collection<String> categoryLongNames, Collection<LocaleEnum> dataLocales, ICallerContext iCallerContext)
        {
            return MakeServiceCall<ICategoryCollection>("SearchCategories", "SearchCategories", client => client.SearchCategories(containerNames, categoryLongNames, dataLocales, FillDiagnosticTraces(iCallerContext)));
        }

        #endregion Category Methods

        #region Entity Hierarchy Methods

        /// <summary>
        /// Reclassifies entities given in input xml
        /// </summary>
        /// <param name="dataXml">Xml containing list of entity Ids to be reclassified with destination container ids..</param>
        /// <param name="userLogin">UserName who is doing the reclassification</param>
        /// <param name="isCategoryReclassify">True if it is CategoryReclassify otherwise EntityReclassify as false</param>
        /// <returns>Datatable with Reclassified Entity</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        //[Obsolete("This method has been obsoleted. Please use DataService.ProcessEntity method instead of this")]
        public ITable ReclassifyLegacy(String dataXml, String userLogin, Boolean isCategoryReclassify)
        {
            return MakeServiceCall("ReclassifyLegacy", "ReclassifyLegacy", client => client.ReclassifyLegacy(dataXml, userLogin, isCategoryReclassify));
        }

        /// <summary>
        /// Checks wheather the HierarchyDefinition is modified or not
        /// </summary>
        /// <param name="entityId">Id of an Entity</param>
        /// <param name="entityHierarchyDefinitionId">Id of a HierarchyDefinition</param>
        /// <param name="callerContext">Specifies the caller context which contains the application and module that has invoked the API</param>
        /// <returns>True if Hierarchy Definition has been modified</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        public Boolean IsEntityHierarchyMatrixLatest(Int64 entityId, Int32 entityHierarchyDefinitionId, CallerContext callerContext)
        {
            DiagnosticActivity curActivity = new DiagnosticActivity();

            CallerContext clonedCallerContext = null;

            if (callerContext != null)
            {
                clonedCallerContext = callerContext.Clone() as CallerContext;

                if (clonedCallerContext != null)
                {
                    clonedCallerContext.ActivityId = curActivity.ActivityId;
                }
            }

            return MakeServiceCall("IsEntityHierarchyMatrixLatest", "IsEntityHierarchyMatrixLatest", client => client.IsEntityHierarchyMatrixLatest(entityId, entityHierarchyDefinitionId, clonedCallerContext));
        }

        /// <summary>
        /// Gets entity variants level and entity types based on given entity id
        /// </summary>
        /// <param name="entityId">Indicates entity id for which entity variant level and entity type to be fetched</param>
        /// <param name="iCallerContext">Indicates the caller context</param>
        /// <returns>Returns collection of key value pair with key as variant level and value as entity type</returns>
        public Dictionary<Int32, EntityType> GetEntityVariantLevels(Int64 entityId, ICallerContext iCallerContext)
        {
            return this.MakeServiceCall<Dictionary<Int32, EntityType>>("GetEntityVariantLevels", "GetEntityVariantLevels",
                           client => client.GetEntityVariantLevels(entityId, iCallerContext as CallerContext), MDMTraceSource.EntityHierarchyGet);
        }

        #endregion Entity Hierarchy Methods

        #region Attribute Version

        /// <summary>
        /// Gets the version history details of the attribute 
        /// </summary>
        /// <param name="entityId">Indicates the entity identifier</param>
        /// <param name="entityParentId">Indicates the Parent identifier of an entity</param>
        /// <param name="attributeId">Indicates the attribute identifier</param>
        /// <param name="catalogId">Indicates the catalog identifier</param>
        /// <param name="locales">Indicates the locale identifier</param>
        /// <param name="sequence">Indicates the sequence</param>
        /// <param name="iCallerContext">Indicates the name of application and the module that are performing the action</param>
        /// <returns>Returns the attribute version collection details</returns>
        public IAttributeVersionCollection GetComplexAttributeVersions(Int64 entityId, Int64 entityParentId, Int32 attributeId, Int32 catalogId, Collection<LocaleEnum> locales, Int32 sequence, ICallerContext iCallerContext)
        {
            return MakeServiceCall("GetComplexAttributeVersions", "GetComplexAttributeVersions", client => client.GetComplexAttributeVersions(entityId, entityParentId, attributeId, catalogId, locales, sequence, FillDiagnosticTraces(iCallerContext)), MDMTraceSource.EntityGet);
        }

        /// <summary>
        /// Gets the complex data for complex attribute's version history for the requested attribute id and AuditRefId
        /// </summary>
        /// <example>
        /// <code>
        /// DataService dataService = new DataService();
        /// 
        /// // Entity Id for which version history is requested
        /// Int64 entityId = 54716;
        /// 
        /// // ContainerId under entity is created
        /// Int32 containerId = 5;
        /// 
        /// // AuditRefId for which history is requested
        /// Int64 auditRefId = 29598;
        /// 
        /// // Attribute Id for which the version history details is requested
        /// Int32 attributeId = 4032;
        /// 
        /// // Locale for which data is requested
        /// LocaleEnum locale = LocaleEnum.en_WW;
        /// 
        /// iCallerContext icallerContext = new CallerContext() { Application = MDMCenterApplication.MDMCenter, Module = MDMCenterModules.Entity };
        /// 
        /// Attribute attr = (Attribute)dataService.GetComplexDataByAuditRefId(entityId, containerId, attributeId, auditRefId, locale, callerContext);
        /// 
        /// </code>
        /// </example>
        /// <param name="entityId">Indicates the Entity Id for which data is requested</param>
        /// <param name="containerId">Indicates the containerId for which data is requested</param>
        /// <param name="attributeId">Indicates the attribute id for which the data is requested</param>
        /// <param name="auditRefId">AuditRefId for which history is requested</param>
        /// <param name="locale">Indicates the locale details</param>
        /// <param name="iCallerContext">Indicates the name of application and the module that are performing the action</param>
        /// <returns>Returns the attribute object with the complex attribute's data</returns>
        public IAttribute GetComplexDataByAuditRefId(Int64 entityId, Int32 containerId, Int32 attributeId, Int64 auditRefId, LocaleEnum locale, ICallerContext iCallerContext)
        {
            return this.MakeServiceCall<IAttribute>("GetComplexDataByAuditRefId", "GetComplexDataByAuditRefId", client => client.GetComplexDataByAuditRefId(entityId, containerId, attributeId, auditRefId, locale, FillDiagnosticTraces(iCallerContext)), MDMTraceSource.EntityGet);
        }

        /// <summary>
        /// Gets the hierarchical attribute for hierarchical version history for the requested attribute id and AuditRefId
        /// </summary>
        /// <example>
        /// <code>
        /// DataService dataService = new DataService();
        /// 
        /// // Entity Id for which version history is requested
        /// Int64 entityId = 54716;
        /// 
        /// // ContainerId under entity is created
        /// Int32 containerId = 5;
        /// 
        /// // AuditRefId for which history is requested
        /// Int64 auditRefId = 29598;
        /// 
        /// // Attribute Id for which the version history details is requested
        /// Int32 attributeId = 4032;
        /// 
        /// // Locale for which data is requested
        /// LocaleEnum locale = LocaleEnum.en_WW;
        /// 
        /// iCallerContext icallerContext = new CallerContext() { Application = MDMCenterApplication.MDMCenter, Module = MDMCenterModules.Entity };
        /// 
        /// Attribute hierarchicalAttribute = (Attribute)dataService.GetHierarchicalAttributeByAuditRefId(entityId, containerId, attributeId, auditRefId, locale, callerContext);
        /// 
        /// </code>
        /// </example>
        /// <param name="entityId">Indicates the Entity Id for which data is requested</param>
        /// <param name="containerId">Indicates the containerId for which data is requested</param>
        /// <param name="attributeId">Indicates the attribute id for which the data is requested</param>
        /// <param name="auditRefId">AuditRefId for which history is requested</param>
        /// <param name="locale">Indicates the locale details</param>
        /// <param name="iCallerContext">Indicates the name of application and the module that are performing the action</param>
        /// <returns>Hierarchical attribute at some specific point of history</returns>
        public IAttribute GetHierarchicalAttributeByAuditRefId(Int64 entityId, Int32 containerId, Int32 attributeId, Int64 auditRefId, LocaleEnum locale, ICallerContext iCallerContext)
        {
            return this.MakeServiceCall<IAttribute>("GetHierarchicalAttributeByAuditRefId", "GetHierarchicalAttributeByAuditRefId", client => client.GetHierarchicalAttributeByAuditRefId(entityId, containerId, attributeId, auditRefId, locale, FillDiagnosticTraces(iCallerContext)), MDMTraceSource.EntityGet);
        }


        #endregion

        #region File Methods

        /// <summary>        
        /// Gets Import templete file
        /// </summary>
        /// <param name="importProfileName">Indicates Name of Import Template</param>    
        /// <param name="callerContext">Specifies Caller context</param>
        /// <returns>File object</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        public IFile GetImportTemplateFileByImportProfileName(String importProfileName, CallerContext callerContext)
        {
            return this.MakeServiceCall("GetImportTemplateFileByImportProfileName", "GetImportTemplateFileByImportProfileName", client => client.GetImportTemplateFileByImportProfileName(importProfileName, callerContext), MDMTraceSource.APIFramework);
        }

        #endregion

        #region Queued Entity

        /// <summary>
        /// Get queued entities by given entityActivityLogId
        /// </summary>
        /// <param name="entityActivity">Type of Activity performed</param>
        /// <param name="entityActivityLogId">PK of entityActivityLog table</param>
        /// <param name="iCallerContext">Context indicating who called the method</param>
        /// <returns>Collection of queued entities.</returns>
        public IQueuedEntityCollection GetQueuedEntities(EntityActivityList entityActivity, Int64 entityActivityLogId, ICallerContext iCallerContext)
        {
            return this.MakeServiceCall("GetQueuedEntities", "GetQueuedEntities", client => client.GetQueuedEntities(entityActivity, entityActivityLogId, FillDiagnosticTraces(iCallerContext)), MDMTraceSource.DenormProcess);
        }

        #endregion Queued Entity

        #region MDMEvents and MDMEvent Handlers Methods

        /// <summary>
        /// Get all the MDMEvents from the system
        /// </summary>
        /// <param name="callerContext">Indicates the name of application and the module that are performing the action</param>
        /// <returns>Returns the list of MDMEvents</returns>
        public IMDMEventInfoCollection GetMDMEventInformation(ICallerContext callerContext)
        {
            return MakeServiceCall<IMDMEventInfoCollection>("GetMDMEventInformation", "GetMDMEventInformation", client => client.GetMDMEventInformation(FillDiagnosticTraces(callerContext)));
        }

        /// <summary>
        /// Get all the MDMEvent Handlers from the system
        /// </summary>
        /// <param name="eventHandlerIdList">Indicates the list of event Handler Ids</param>
        /// <param name="callerContext">Indicates the name of application and the module that are performing the action</param>
        /// <returns>Returns the list of MDMEvent Handlers</returns>
        public IMDMEventHandlerCollection GetMDMEventHandlers(Collection<Int32> eventHandlerIdList, ICallerContext callerContext)
        {
            return MakeServiceCall<IMDMEventHandlerCollection>("GetMDMEventHandlers", "GetMDMEventHandlers", client => client.GetMDMEventHandlers(eventHandlerIdList, FillDiagnosticTraces(callerContext)));
        }

        /// <summary>
        /// Process the requested MDMEvent Handler based on their actions.
        /// </summary>
        /// <param name="mdmEventHandlerCollection">Indicates the list of MDMEvnet Handlers</param>
        /// <param name="callerContext">Indicates the name of application and the module that are performing the action</param>
        /// <returns>Returns the operation results</returns>
        public IOperationResultCollection ProcessMDMEventHandlers(IMDMEventHandlerCollection mdmEventHandlerCollection, ICallerContext callerContext)
        {
            return MakeServiceCall<IOperationResultCollection>("ProcessMDMEventHandlers", "ProcessMDMEventHandlers", client => client.ProcessMDMEventHandlers(mdmEventHandlerCollection as MDMEventHandlerCollection, FillDiagnosticTraces(callerContext)));
        }

        #endregion MDMEvents and MDMEvent Handlers Methods

        #region Relationship type mapping related Interfaces

        /// <summary>
        /// Get container relationship type entity type mapping based on given context
        /// </summary>
        /// <param name="iEntityModelContext">Indicates the entity model context based on which mappings to be returned.</param>
        /// <param name="iCallerContext">Indicates the name of application and the module that are performing the action</param>
        /// <returns>Returns the container relationship type entity type mappings based on entity model context.</returns>
        public IContainerRelationshipTypeEntityTypeMappingCollection GetContainerRelationshipTypeEntityTypeMappings(IEntityModelContext iEntityModelContext, ICallerContext iCallerContext)
        {
            return MakeServiceCall<ContainerRelationshipTypeEntityTypeMappingCollection>
                ("GetContainerRelationshipTypeEntityTypeMappings", "GetContainerRelationshipTypeEntityTypeMappings",
                client => client.GetContainerRelationshipTypeEntityTypeMappings(iEntityModelContext as EntityModelContext, 
                                                                        iCallerContext as CallerContext), MDMTraceSource.DataModel);
        }

        #endregion Relationship type mapping related Interfaces

        /// <summary>
        /// Gets attribute models by container ids
        /// </summary>
        /// <param name="containerIdList">Indicates containers ids based on which needs to get attribute models</param>
        /// <param name="iCallerContext">Indicates context of the caller</param>
        /// <returns>Collection of attribute models</returns>
        public IAttributeModelCollection GetMappedAttributeModelsByContainers(Collection<Int32> containerIdList, ICallerContext iCallerContext)
        {
            return MakeServiceCall("GetMappedAttributeModelsByContainers", "GetMappedAttributeModelsByContainers",
                client => client.GetMappedAttributeModelsByContainers(containerIdList, FillDiagnosticTraces(iCallerContext)));
        }

        #region Promote Methods
        /// <summary>
        /// Enqueues for promote.
        /// </summary>
        /// <param name="entityFamilyQueueCollection">The entity family queue collection.</param>
        /// <param name="callerContext">The caller context.</param>
        /// <returns>OperationResultCollection containing the results of operation</returns>
        public IOperationResultCollection EnqueueForPromote(IEntityFamilyQueueCollection entityFamilyQueueCollection, ICallerContext callerContext)
        {
            return MakeServiceCall<IOperationResultCollection>("EnqueueForPromote", "EnqueueForPromote", client => client.EnqueueForPromote(entityFamilyQueueCollection as EntityFamilyQueueCollection, FillDiagnosticTraces(callerContext)));
        }

        #endregion Promote Methods

        #region Entity State Validation Methods

        /// <summary>
        /// Gets the state of the entity validation for given entity id list and its family members
        /// </summary>
        /// <param name="entityIds">Indicates the entity ids</param>
        /// <param name="needGlobalFamilyErrors">Indicates whether to return states for global family members or not</param>
        /// <param name="needVariantFamilyErrors">Indicates whether to return states for variant family members or not</param>
        /// <param name="iCallerContext">Indicates Application and Module name by which action is being performed</param>
        /// <returns>Returns collection of entity state validation based on provided entity id list</returns>
        public IEntityStateValidationCollection GetEntitiesStateValidation(Collection<Int64> entityIds, Boolean needGlobalFamilyErrors, Boolean needVariantFamilyErrors, ICallerContext iCallerContext)
        {
            return MakeServiceCall<IEntityStateValidationCollection>("GetEntitiesStateValidation", "GetEntitiesStateValidation", client => client.GetEntitiesStateValidation(entityIds, needGlobalFamilyErrors, needVariantFamilyErrors, FillDiagnosticTraces(iCallerContext)));
        }


        /// <summary>
        /// Enqueue for re-validate process for given entity family queue
        /// </summary>
        /// <param name="iEntityFamilyQueue">Indicates entity family queue for which re-validation process is required</param>
        /// <param name="iCallerContext">Indicates application and module name of caller</param>
        /// <returns>Returns operation result of current operation</returns>
        public IOperationResult EnqueueForRevalidate(IEntityFamilyQueue iEntityFamilyQueue, ICallerContext iCallerContext)
        {
            return MakeServiceCall<IOperationResult>("EnqueueForRevalidate", "EnqueueForRevalidate", client => client.EnqueueForRevalidate(iEntityFamilyQueue as EntityFamilyQueue, FillDiagnosticTraces(iCallerContext)));
        }

        /// <summary>
        /// Gets business conditions for the given entity ids
        /// </summary>
        /// <param name="entityIds">Indicates the entity ids</param>
        /// <param name="iCallerContext">Indicates Application and Module name by which action is being performed</param>
        /// <returns>Returns collection of entity business conditions based on provided entity ids</returns>
        public IEntityBusinessConditionCollection GetEntityBusinessConditions(Collection<Int64> entityIds, ICallerContext iCallerContext)
        {
            return MakeServiceCall<IEntityBusinessConditionCollection>("GetEntityBusinessConditions", "GetEntityBusinessConditions", client => client.GetEntityBusinessConditions(entityIds, FillDiagnosticTraces(iCallerContext)));
        }

        #endregion Entity State Validation Methods

        #region Entity Activity Log Methods

        /// <summary>
        /// Gets all the impacted entity logs based on the processing status
        /// * LogType.Current -> get all the records from tb_EntityActivityLog with IsLoaded = true, IsProcessed = 0
        /// * LogType.Past -> get all the records from tb_EntityActivityLog_HS
        /// * LogType.Pending -> get all the records from tb_EntityActivityLog with IsLoaded = false, IsProcessed = 0
        /// </summary>
        /// <param name="entityActivityList">Indicates the entity activity list type</param>
        /// <param name="processingStatus">Indicates the processing status type like ProcessingStatus.Current,ProcessingStatus.Past,ProcessingStatus.Pending</param>
        /// <param name="fromRecordNumber">Indicates the starting index of record to be fetched</param>
        /// <param name="toRecordNumber">Indicates the end index of record to be fetched</param>
        /// <param name="iCallerContext">Indicates the caller context specifying the caller application and module details.</param>
        /// <returns>Returns collection of entity activity log based on the given context</returns>
        public IEntityActivityLogCollection GetEntityActivityLogs(EntityActivityList entityActivityList, ProcessingStatus processingStatus, Int64 fromRecordNumber, Int64 toRecordNumber, ICallerContext iCallerContext)
        {
            return MakeServiceCall("GetEntityActivityLogs",
                                   "GetEntityActivityLogs",
                                   service => service.GetEntityActivityLogs(entityActivityList, processingStatus,
                                                                            fromRecordNumber,
                                                                            toRecordNumber,
                                                                            FillDiagnosticTraces(iCallerContext)));
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityIdList"></param>
        /// <param name="activityLongName"></param>
        /// <param name="workflowAction"></param>
        /// <param name="comments"></param>
        /// <param name="operationType"></param>
        /// <param name="newlyAssignedUser"></param>
        /// <param name="iCallerContext"></param>
        /// <returns></returns>
        public IEntityOperationResultCollection PerformBulkWorkflowOperation(Collection<Int64> entityIdList, String activityLongName, String workflowAction, String comments, String operationType, ISecurityUser newlyAssignedUser, ICallerContext iCallerContext)
        {
            return MakeServiceCall("PerformBulkWorkflowOperation", "PerformBulkWorkflowOperation",
                    client => client.PerformBulkWorkflowOperation(entityIdList, activityLongName, workflowAction, comments, operationType, newlyAssignedUser as SecurityUser, iCallerContext as CallerContext));
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private IInternalCommonService GetClient(Boolean useProtoBufSerialization = false, MDMTraceSource traceSource = MDMTraceSource.General)
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            IInternalCommonService serviceClient = null;

            if (WCFServiceInstanceLoader.IsLocalInstancesEnabled())
            {
                serviceClient = WCFServiceInstanceLoader.GetServiceInstance<IInternalCommonService>();
            }

            if (serviceClient == null) //Means the given type is not implemented for local load..
            {
                if (useProtoBufSerialization)
                {
                    Boolean isProtoBufSerializationEnabled = AppConfigurationHelper.GetAppConfig("MDMCenter.WCFService.ProtoBufSerialization.Enabled", true);
                    if (isProtoBufSerializationEnabled && !_isProtoBufEndPointInitialized && !String.IsNullOrEmpty(this.EndPointConfigurationName))
                    {
                        this.EndPointConfigurationName = String.Format("{0}{1}", this.EndPointConfigurationName, "_UsingProtoBuf");

                        //If service is called multiple times, EndPointConfigurationName is appended _UsingProtoBuf more than once
                        _isProtoBufEndPointInitialized = true;

                        if (isTracingEnabled)
                            MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "ProtoBuf serialization enabled", traceSource);
                    }
                }

                InternalCommonServiceProxy internalCommonServiceProxy = null;

                if (String.IsNullOrEmpty(this.EndPointConfigurationName) && this.EndpointAddress == null)
                {
                    internalCommonServiceProxy = new InternalCommonServiceProxy();
                }
                else if (!String.IsNullOrEmpty(this.EndPointConfigurationName) && this.EndpointAddress == null)
                {
                    internalCommonServiceProxy = new InternalCommonServiceProxy(this.EndPointConfigurationName);
                }
                else if (!String.IsNullOrEmpty(this.EndPointConfigurationName) && this.EndpointAddress != null)
                {
                    internalCommonServiceProxy = new InternalCommonServiceProxy(this.EndPointConfigurationName, this.EndpointAddress);
                }

                if (this.AuthenticationType == Core.AuthenticationType.Forms)
                {
                    internalCommonServiceProxy.ClientCredentials.UserName.UserName = this.UserName;
                    internalCommonServiceProxy.ClientCredentials.UserName.Password = this.Password;
                    internalCommonServiceProxy.ClientCredentials.ServiceCertificate.Authentication.CertificateValidationMode = System.ServiceModel.Security.X509CertificateValidationMode.None;
                }

                serviceClient = internalCommonServiceProxy;
            }

            return serviceClient;
        }

        /// <summary>
        /// Makes the InternalCommonServiceClient call. Creates the client instance, executes call delegate against it in
        /// impersonated context (if necessary) and then disposes the client.
        /// Also emits traces when necessary.
        /// </summary>
        /// <typeparam name="TResult">Indicates type of the result of service call.</typeparam>
        /// <param name="clientMethodName">Indicates name of the client method to include in traces.</param>
        /// <param name="serverMethodName">Indicates name of the server method to include in traces.</param>
        /// <param name="call">Indicates call delegate to be executed against a client instance.</param>
        /// <param name="traceSource">Indicates name of the trace source</param>
        /// <returns>Returns the value returned by service, or default.</returns>
        private TResult MakeServiceCall<TResult>(String clientMethodName, String serverMethodName, Func<IInternalCommonService, TResult> call, MDMTraceSource traceSource = MDMTraceSource.APIFramework)
        {
            #region Diagnostics & Tracing

            ExecutionContext executionContext = new ExecutionContext(traceSource);
            DiagnosticActivity activity = new DiagnosticActivity(executionContext, String.Format("InternalCommonService.{0}", clientMethodName));
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;

            //Start trace activity
            if (isTracingEnabled)
            {
                activity.Start();
            }

            #endregion Diagnostics & Tracing

            TResult result = default(TResult);
            IInternalCommonService internalCommonServiceClient = null;

            try
            {
                internalCommonServiceClient = GetClient();

                ValidateContext();

                if (isTracingEnabled)
                {
                    activity.LogVerbose(String.Format("InternalCommonService sends '{0}' request message.", serverMethodName));
                }

                result = Impersonate(() => call(internalCommonServiceClient));

                if (isTracingEnabled)
                {
                    activity.LogVerbose(String.Format("InternalCommonService receives '{0}' response message.", serverMethodName));
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            finally
            {
                //Close the client
                DisposeClient(internalCommonServiceClient);

                if (isTracingEnabled)
                {
                    activity.Stop();
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        private void DisposeClient(IInternalCommonService client)
        {
            if (client == null)
                return;

            if (client.GetType().Equals(typeof(InternalCommonServiceProxy)))
            {
                InternalCommonServiceProxy serviceClient = (InternalCommonServiceProxy)client;
                if (serviceClient.State == CommunicationState.Created || serviceClient.State == CommunicationState.Opened || serviceClient.State == CommunicationState.Opening)
                {
                    serviceClient.Close();
                }
                else if (serviceClient.State == CommunicationState.Faulted)
                {
                    serviceClient.Abort();
                }
            }
            else
            {
                //Do nothing...
            }
        }

        /// <summary>
        /// Throws argument null exception if caller context object is null.
        /// </summary>
        /// <param name="iCallerContext">Indicates caller context, which contains the application and module that has invoked the API</param>
        private void ValidateCallerContext(ICallerContext iCallerContext)
        {
            if (iCallerContext == null)
            {
                throw new ArgumentNullException(paramName: "iCallerContext");
            }
        }

        #endregion Private Methods

        #endregion
    }
}
