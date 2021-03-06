﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Security.Principal;
using System.ServiceModel;

namespace MDM.Services
{
    using BusinessObjects;
    using BusinessObjects.Diagnostics;
    using BusinessObjects.Imports;
    using Core;
    using Core.Exceptions;
    using Interfaces;
    using ServiceProxies;
    using Utility;
    using WCFServiceInterfaces;

    /// <summary>
    /// Data Service facilitates to work with MDMCenter entity data. 
    /// This includes methods to search entities and work with the following: Entity Master Information, Entity Attributes, Relationship, Hierarchies (child entities), and Extensions (MDLs).
    /// </summary>
    public class DataService : ServiceClientBase
    {
        #region Fields

        private Boolean _isProtoBufEndPointInitialized = false;

        #endregion

        #region Properties

        #endregion

        #region Constructors

        /// <summary>
        /// Use this default constructor only when calling MDM service within the execution context of an MDM Application. 
        /// Example: Service call from the MDM business rule.
        /// This would try to use current execution security context for service authentication purpose. 
        /// </summary>
        public DataService()
            : base(typeof(DataService))
        {
            MDMTraceHelper.InitializeTraceSource();
        }

        /// <summary>
        /// Use this default constructor with binding configuration name only when calling MDM service within the execution context of an MDM Application. 
        /// Example: Service call from the MDM business rule.
        /// This would try to use current execution security context for service authentication purpose. 
        /// </summary>
        /// <param name="endPointConfigurationName">Indicates the EndPointConfigurationName</param>
        public DataService(String endPointConfigurationName)
            : base(typeof(DataService), endPointConfigurationName)
        {
        }

        /// <summary>
        /// Use this constructor for form based authentication passing username and password
        /// </summary>
        /// <param name="endPointConfigurationName">Indicates the EndPointConfigurationName</param>
        /// <param name="userName">Name of the Login User</param>
        /// <param name="password">Password of the User</param>
        public DataService(String endPointConfigurationName, String userName, String password)
            : base(endPointConfigurationName, userName, password)
        {

        }

        /// <summary>
        /// Use this constructor for window authentication passing current windows identity.
        /// </summary>
        /// <param name="endPointConfigurationName">Indicates the EndPointConfigurationName</param>
        /// <param name="userIdentity">Indicates User's Identity</param>
        public DataService(String endPointConfigurationName, IIdentity userIdentity)
            : base(endPointConfigurationName, userIdentity)
        {
        }

        /// <summary>
        /// Constructor with client configuration.
        /// </summary>
        /// <param name="wcfClientConfiguration">Indicates WCF Client Configuration</param>
        public DataService(IWCFClientConfiguration wcfClientConfiguration)
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
        public DataService(String endPointConfigurationName, AuthenticationType authenticationType, IIdentity userIdentity, String userName, String password)
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
        public DataService(String endPointConfigurationName, EndpointAddress endPointAddress, AuthenticationType authenticationType, IIdentity userIdentity, String userName, String password)
            : base(endPointConfigurationName, endPointAddress, authenticationType, userIdentity, userName, password)
        {
        }

        #endregion

        #region Entity Methods

        /// <summary>
        /// Gets the model of an Entity based on the following: Entity Type ID, Category ID, Parent Entity ID, and Entity Context
        /// </summary>
        /// <example>
        /// <code>
        /// // Assumption: EntityId, AttributeId, ContainerId etc. are pre-assumed
        /// // It may change based on database setup
        ///
        /// // Get new instance of IEntityContext and IEntity using MDMObjectFactory
        ///    IEntityContext entityContext = MDMObjectFactory.GetIEntityContext();
        ///    IEntity entity = MDMObjectFactory.GetIEntity();
        ///
        /// // Get MDM data service
        ///    DataService dataService = GetMDMDataService();
        ///
        ///    entityContext.ContainerId = 5; // Product Master Container
        ///    entityContext.CategoryId = 1; // Category 1 - Apparel
        ///    entityContext.ParentEntityId = 1; // Category - Apparel
        ///    entityContext.EntityTypeId = 16; // Style
        ///    entityContext.Locale = LocaleEnum.en_WW; // Default locale
        ///    entityContext.DataLocales.Add(LocaleEnum.en_WW);
        ///    entityContext.LoadAttributes = true;
        ///    entityContext.AttributeIdList = new System.Collections.ObjectModel.Collection<![CDATA[<Int32>]]> { 4027 }; 
        /// // Load Product Description attribute
        /// // Get the attribute id using the following query: 
        /// // select * from tb_Attribute table where shortname = 'type in the shortname of the attribute in this place'
        /// 
        /// // Below code will make WCF call to get the EntityModel for given context
        ///    entity = dataService.GetEntityModel(entityContext, MDMCenterApplication.MDMCenter, MDMCenterModules.Entity);
        ///
        /// </code>
        /// </example>
        /// <param name="iEntityContext">Indicates the data context for which Entity needs to be fetched</param>
        /// <param name="application">Indicates the name of the application which is performing the action</param>
        /// <param name="module">Indicates the name of the module which is performing the action</param>
        /// <returns>Returns the Entity Interface with the Model loaded</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        public IEntity GetEntityModel(IEntityContext iEntityContext, MDMCenterApplication application, MDMCenterModules module)
        {
            return MakeServiceCall("GetEntityModel",
                                   "GetEntityModel",
                                   service => service.GetEntityModel((EntityContext)iEntityContext, application, module),
                                   MDMTraceSource.EntityGet,
                                   true);
        }

        /// <summary>
        /// Gets the Entity based on the Entity Id
        /// </summary>
        /// <param name="entityId">Indicates the Entity Id</param>
        /// <param name="iEntityContext">Indicates the data context for which Entity needs to be fetched</param>
        /// <param name="application">Indicates the name of the application which invoked the API</param>
        /// <param name="module">Indicates the name of the module which invoked the API</param>
        /// <param name="applyAVS"> Indicates whether AVS should be applied. If the value is True, AVS is applied</param>
        /// <param name="publishEvents"> Indicates whether events should be published. If the value is True, events are published </param>
        /// <returns>Returns Entity in the requested EntityContext</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        /// <example>
        /// <code language="c#" title="Sample 1: Get entity with ShowAtCreation attributes" source="..\MDM.APISamples\EntityManager\Entity\GetEntity.cs" region="Get Entity By Entity Id" />
        /// <code language="c#" title="Sample 2: Get entity with ShowAtCreation attributes without publishing events and apply AVS" source="..\MDM.APISamples\EntityManager\Entity\GetEntity.cs" region="Get Entity By Entity Id And Optional Parameters" />
        /// </example>
        public IEntity GetEntity(Int64 entityId, IEntityContext iEntityContext, MDMCenterApplication application, MDMCenterModules module, Boolean publishEvents = true, Boolean applyAVS = true)
        {
            return MakeServiceCall<IEntity>("GetEntity",
                                            "GetEntity",
                                            service =>
                                            {
                                                if (iEntityContext == null)
                                                {
                                                    throw new ArgumentNullException(
                                                        "EntityContext is null. Requested EntityId:" + entityId);
                                                }
                                                return service.GetEntity(entityId,
                                                                         (EntityContext)iEntityContext,
                                                                         application,
                                                                         module,
                                                                         publishEvents,
                                                                         applyAVS);
                                            },
                                            MDMTraceSource.EntityGet,
                                            true);
        }

        /// <summary>
        /// Gets the Entity based on the Entity Id and EntityGetOptions
        /// </summary>
        /// <param name="entityId">Indicates the Entity Id</param>
        /// <param name="iEntityContext">Indicates the data context for which Entity needs to be fetched</param>
        /// <param name="iEntityGetOptions">Indicates the options available while retrieving entity data</param>
        /// <param name="iCallerContext">Indicates the name of the Application and Module which invoked the API</param>
        /// <returns>Returns Entity in the requested EntityContext</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>        
        /// <exception cref="ArgumentNullException">Thrown when one of the passed context arguments is null</exception>
        /// <example>
        /// <code language="c#" title="Sample 1: Get entity with all attributes" source="..\MDM.APISamples\EntityManager\Entity\GetEntity.cs" region="Get Entity By Entity Id EntityGetOptions And CallerContext" />
        /// <code language="c#" title="Sample 2: Get entity with attributes of given attribute IDs" source="..\MDM.APISamples\EntityManager\Entity\GetEntity.cs" region="Get Entity And Attributes By Entity Id" />
        /// <code language="c#" title="Sample 3: Get entity with attributes of given attribute group IDs" source="..\MDM.APISamples\EntityManager\Entity\GetEntity.cs" region="Get Entity And Attributes By Entity Id And AttributeGroupID" />
        /// <code language="c#" title="Sample 4: Get entity with hierarchy relationships" source="..\MDM.APISamples\EntityManager\Entity\GetEntity.cs" region="Get Entity And HierarchyRelationships" />
        /// <code language="c#" title="Sample 5: Get entity with attributes of given attribute group IDs" source="..\MDM.APISamples\EntityManager\Entity\GetEntity.cs" region="Get Entity And Attributes By Entity Id And AttributeGroupID" />
        /// <code language="c#" title="Sample 6: Get an entity with attributes of given attribute IDs without publishing events" source="..\MDM.APISamples\EntityManager\Entity\GetEntity.cs" region="Get Entity And Attributes By Entity Id" />
        /// </example>
        public IEntity GetEntity(Int64 entityId, IEntityContext iEntityContext, IEntityGetOptions iEntityGetOptions, ICallerContext iCallerContext)
        {
            return MakeServiceCall<IEntity>("GetEntity",
                                            "GetEntityWithGetOptions",
                                            service => service.GetEntity(entityId,
                                                                         iEntityContext as EntityContext,
                                                                         iEntityGetOptions as EntityGetOptions,
                                                                         FillDiagnosticTraces(iCallerContext)),
                                            MDMTraceSource.EntityGet,
                                            true);
        }

        /// <summary>
        /// Gets Entity Collection for the requested Entity Ids 
        /// </summary>
        /// <param name="entityIdList">Indicates the Entity Ids for which Entity Objects are required</param>
        /// <param name="iEntityContext">Indicates the data context for which Entity needs to be fetched</param>
        /// <param name="application">Indicates the name of the application which invoked the API</param>
        /// <param name="module">Indicates the name of the module which invoked the API</param>
        /// <param name="applyAVS"> Indicates whether AVS should be applied. If the value is True, AVS is applied</param>
        /// <param name="publishEvents"> Indicates whether events should be published. If the value is True, events are published</param>
        /// <returns>Returns collection of entities in the requested context</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        /// <example>
        /// <code language="c#" title="Get Entities By Entity Id Collection" source="..\MDM.APISamples\EntityManager\Entity\GetEntity.cs" region="Get Entities By Entity Id Collection" />
        /// <code language="c#" title="Get Entities By Entity Id Collection And Optional Parameters" source="..\MDM.APISamples\EntityManager\Entity\GetEntity.cs" region="Get Entities By Entity Id Collection And Optional Parameters" />
        /// </example>
        public IEntityCollection GetEntities(Collection<Int64> entityIdList, IEntityContext iEntityContext, MDMCenterApplication application, MDMCenterModules module, Boolean publishEvents = true, Boolean applyAVS = true)
        {
            IEntityGetOptions iEntityGetOption = MDMObjectFactory.GetIEntityGetOptions();
            iEntityGetOption.ApplyAVS = applyAVS;
            iEntityGetOption.PublishEvents = publishEvents;

            ICallerContext iCallerContext = MDMObjectFactory.GetICallerContext(application, module);

            return GetEntities(entityIdList, iEntityContext, iEntityGetOption, iCallerContext);
        }

        /// <summary>
        /// Gets Entity Collection for the requested Entity Ids And Entity Get Options
        /// </summary>
        /// <param name="entityIdList">Indicates the Entity Ids for which Entity Objects are required</param>
        /// <param name="entityContext">Indicates the data context for which Entity needs to be fetched</param>
        /// <param name="entityGetOptions">Indicates the options available while retrieving entity data</param>
        /// <param name="callerContext">Indicates the name of the Application and Module which invoked the API</param>
        /// <returns>Returns collection of entities in the requested context</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        /// <example>
        /// <code language="c#" title="Get Entities By Entity Id Collection And Entity Get Options" source="..\MDM.APISamples\EntityManager\Entity\GetEntity.cs" region="Get Entities By Entity Id Collection And Entity Get Options" />
        /// </example>
        public IEntityCollection GetEntities(Collection<Int64> entityIdList, IEntityContext entityContext, IEntityGetOptions entityGetOptions, ICallerContext callerContext)
        {
            return MakeServiceCall<EntityCollection>("GetEntities",
                                                     "GetEntities",
                                                     client => client.GetEntities(entityIdList,
                                                                            (EntityContext)entityContext,
                                                                            (EntityGetOptions)entityGetOptions,
                                                                            FillDiagnosticTraces(callerContext)),
                                                     MDMTraceSource.EntityGet);
        }

        /// <summary>
        /// Creates an Entity Object after getting the model from GetModelbyId method
        /// </summary>
        /// <param name="iEntity">Indicates the instance of entities to be processed in the MDM system</param>
        /// <param name="programName">Indicates the name of the program which invoked the API</param>
        /// <param name="application">Indicates the name of the application which invoked the API</param>
        /// <param name="module">Indicates the name of the module which invoked the API</param>
        /// <returns>Returns the result of creating the entity</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        /// <example>
        /// <code language="c#" title="Create Entity" source="..\MDM.APISamples\EntityManager\Entity\ProcessEntity.cs" region="Create Entity Without EntityProcessingOptions" />
        /// </example>
        public IEntityOperationResult CreateEntity(IEntity iEntity, String programName, MDMCenterApplication application, MDMCenterModules module)
        {
            return MakeServiceCall("CreateEntity",
                                   "CreateEntity",
                                   service => service.CreateEntity((Entity)iEntity,
                                                                   new EntityProcessingOptions(),
                                                                   programName,
                                                                   application,
                                                                   module));
        }

        /// <summary>
        /// Creates an entity with the entity details and entity processing options provided by the user 
        /// </summary>
        /// <param name="iEntity">Indicates the instance of entity to be created in MDMCenter</param>
        /// <param name="iEntityProcessingOptions">Indicates the instance of Processing Option for the Processing Entities</param>
        /// <param name="programName">Indicates the name of the program which invoked the API</param>
        /// <param name="application">Indicates the name of the application which invoked the API</param>
        /// <param name="module">Indicates the name of the module which invoked the API</param>
        /// <returns>Returns the result of creating the entity</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        /// <example>
        /// <code language="c#" title="Create Entity" source="..\MDM.APISamples\EntityManager\Entity\ProcessEntity.cs" region="Create Entity With EntityProcessingOptions" />
        /// </example>
        public IEntityOperationResult CreateEntity(IEntity iEntity, IEntityProcessingOptions iEntityProcessingOptions, String programName, MDMCenterApplication application, MDMCenterModules module)
        {
            return this.MakeServiceCall<IEntityOperationResult>("CreateEntity", "CreateEntity",
               client => client.CreateEntity(iEntity as Entity, iEntityProcessingOptions as EntityProcessingOptions, programName, application, module));
        }

        /// <summary>
        /// Creates the entities based on the entity details
        /// </summary>
        /// <param name="iEntities">Indicates the instance of entities to be processed in the MDM system</param>
        /// <param name="programName">Indicates the name of the program which invoked the API</param>
        /// <param name="application">Indicates the name of the application which invoked the API</param>
        /// <param name="module">Indicates the name of the module which invoked the API</param>
        /// <returns>Returns the result of creating the entities</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        /// <example>
        /// <code language="c#" title="Create Entities" source="..\MDM.APISamples\EntityManager\Entity\ProcessEntity.cs" region="Create Entities" />
        /// </example>
        public IEntityOperationResultCollection CreateEntities(IEntityCollection iEntities, String programName, MDMCenterApplication application, MDMCenterModules module)
        {
            return MakeServiceCall("CreateEntities",
                                   "CreateEntities",
                                   service => service.CreateEntities((EntityCollection)iEntities,
                                                                     new EntityProcessingOptions(),
                                                                     programName,
                                                                     application,
                                                                     module));
        }

        /// <summary>
        /// Updates an entity based on the entity details
        /// </summary>
        /// <param name="iEntity">Indicates the entity to be updated</param>
        /// <param name="programName">Indicates the name of the program which invoked the API</param>
        /// <param name="application">Indicates the name of the application which invoked the API</param>
        /// <param name="module">Indicates the name of the module which invoked the API</param>
        /// <returns>Returns the result of updating the entity</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        /// <example>
        /// <code language="c#" title="Update Entity" source="..\MDM.APISamples\EntityManager\Entity\ProcessEntity.cs" region="Update Entity Without EntityProcessingOptions" />
        /// </example>
        public IEntityOperationResult UpdateEntity(IEntity iEntity, String programName, MDMCenterApplication application, MDMCenterModules module)
        {
            return MakeServiceCall("UpdateEntity",
                                   "UpdateEntity",
                                   service =>
                                   {
                                       Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
                                       if (isTracingEnabled)
                                       {
                                           MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Requested Update Entity for EntityID: {0}", iEntity.Id), MDMTraceSource.EntityProcess);
                                       }
                                       return service.UpdateEntity((Entity)iEntity,
                                                                   new EntityProcessingOptions(),
                                                                   programName,
                                                                   application,
                                                                   module);
                                   });
        }

        /// <summary>
        /// Updates an entity based on the entity details and entity processing options
        /// </summary>
        /// <param name="entityPassed">Indicates the entity to be updated</param>
        /// <param name="entityProcessingOptions">Indicates the instance of Processing Option for the Processing Entities</param>
        /// <param name="context">Indicates the name of the application and module which invoked the API</param>
        /// <returns>Returns the result of updating the entity</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        /// <example>
        /// <code language="c#" title="Update Entity" source="..\MDM.APISamples\EntityManager\Entity\ProcessEntity.cs" region="Update Entity With EntityProcessingOptions And CallerContext" />
        /// </example>
        public IEntityOperationResult UpdateEntity(IEntity entityPassed, IEntityProcessingOptions entityProcessingOptions, ICallerContext context)
        {
            TraceSettings tcSettings = MDMOperationContextHelper.GetCurrentTraceSettings();

            ValidateCallerContext(context);

            context.TraceSettings = tcSettings;

            return this.MakeServiceCall<IEntityOperationResult>("UpdateEntityByContext", "UpdateEntityByContext",
                        client => client.UpdateEntityByContext(entityPassed as Entity, context as CallerContext, entityProcessingOptions as EntityProcessingOptions));
        }

        /// <summary>
        /// Updates an entity based on the entity details and entity processing options
        /// </summary>
        /// <param name="iEntity">Indicates the entity to be updated</param>
        /// <param name="iEntityProcessingOptions">Indicates the instance of Processing Option for the Processing Entities</param>
        /// <param name="programName">Indicates the name of the program which invoked the API</param>
        /// <param name="application">Indicates the name of the application which invoked the API</param>
        /// <param name="module">Indicates the name of the module which invoked the API</param>
        /// <returns>Returns the result of updating the entity</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        /// <example>
        /// <code language="c#" title="Update Entity" source="..\MDM.APISamples\EntityManager\Entity\ProcessEntity.cs" region="Update Entity With EntityProcessingOptions" />
        /// </example>
        public IEntityOperationResult UpdateEntity(IEntity iEntity, IEntityProcessingOptions iEntityProcessingOptions, String programName, MDMCenterApplication application, MDMCenterModules module)
        {
            return MakeServiceCall("UpdateEntity",
                                   "UpdateEntity",
                                   service => service.UpdateEntity((Entity)iEntity,
                                                                   (EntityProcessingOptions)iEntityProcessingOptions,
                                                                   programName,
                                                                   application,
                                                                   module));
        }

        /// <summary>
        /// Updates the entities based on the entity details
        /// </summary>
        /// <param name="iEntities">Indicates instance of entities to be processed in the MDM system</param>
        /// <param name="programName">Indicates the name of the Program which invoked the API</param>
        /// <param name="application">Indicates the name of the application which invoked the API</param>
        /// <param name="module">Indicates the name of the module which invoked the API</param>
        /// <returns>Returns the result of updating the entities</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        /// <example>
        /// <code language="c#" title="Update Entities Without EntityProcessingOptions" source="..\MDM.APISamples\EntityManager\Entity\ProcessEntity.cs" region="Update Entities Without EntityProcessingOptions" />
        /// </example>
        public IEntityOperationResultCollection UpdateEntities(IEntityCollection iEntities, String programName, MDMCenterApplication application, MDMCenterModules module)
        {
            return MakeServiceCall("UpdateEntities",
                                   "UpdateEntities",
                                   service => service.UpdateEntities((EntityCollection)iEntities,
                                                                     new EntityProcessingOptions(),
                                                                     programName,
                                                                     application,
                                                                     module));
        }

        /// <summary>
        /// Updates the entities based on the entity details and entity processing options
        /// </summary>
        /// <param name="iEntities">Indicates the instance of the entities to be processed in the MDM system</param>
        /// <param name="iEntityProcessingOptions">Indicates the instance of Processing Option for the Processing Entities</param>
        /// <param name="programName">Indicates the name of the Program which invoked the API</param>
        /// <param name="application">Indicates the name of the application which invoked the API</param>
        /// <param name="module">Indicates the name of the module which invoked the API</param>
        /// <returns>Returns the result of updating the entities</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        /// <example>
        /// <code language="c#" title="Update Entities With EntityProcessingOptions" source="..\MDM.APISamples\EntityManager\Entity\ProcessEntity.cs" region="Update Entities With EntityProcessingOptions" />
        /// </example>
        public IEntityOperationResultCollection UpdateEntities(IEntityCollection iEntities, IEntityProcessingOptions iEntityProcessingOptions, String programName, MDMCenterApplication application, MDMCenterModules module)
        {
            return this.MakeServiceCall<IEntityOperationResultCollection>("UpdateEntities", "UpdateEntities",
                    client => client.UpdateEntities(iEntities as EntityCollection, iEntityProcessingOptions as EntityProcessingOptions, programName, application, module));
        }

        /// <summary>
        /// Deletes an existing Entity
        /// </summary>
        /// <param name="iEntity">Indicates the Entity Object to be deleted in the MDM system </param>
        /// <param name="programName">Indicates the name of the program that requested for delete action</param>
        /// <param name="application">Indicates the name of the application which is performing the action</param>
        /// <param name="module">Indicates the name of the module which is performing the action</param>
        /// <returns>Returns the operation result for deleting an Entity</returns>
        /// <example>
        /// <code language="c#" title="Delete Entity Without EntityProcessingOptions" source="..\MDM.APISamples\EntityManager\Entity\ProcessEntity.cs" region="Delete Entity Without EntityProcessingOptions" />
        /// </example>
        public IEntityOperationResult DeleteEntity(IEntity iEntity, String programName, MDMCenterApplication application, MDMCenterModules module)
        {
            return MakeServiceCall("DeleteEntity",
                                   "DeleteEntity",
                                   service => service.DeleteEntity((Entity)iEntity, programName, application, module));
        }

        /// <summary>
        /// Deletes the entities based on entity details
        /// </summary>
        /// <param name="iEntities">Indicates the instance of entities to be deleted in the MDM system</param>
        /// <param name="programName">Indicates the name of the Program which invoked the API</param>
        /// <param name="application">Indicates the name of the application which invoked the API</param>
        /// <param name="module">Indicates the name of the module which invoked the API</param>
        /// <returns>Returns the result of deleting the entities</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        /// <example>
        /// <code language="c#" title="Delete Entities Without EntityProcessingOptions" source="..\MDM.APISamples\EntityManager\Entity\ProcessEntity.cs" region="Delete Entities Without EntityProcessingOptions" />
        /// </example>
        public IEntityOperationResultCollection DeleteEntities(IEntityCollection iEntities, String programName, MDMCenterApplication application, MDMCenterModules module)
        {
            return MakeServiceCall("DeleteEntities",
                                   "DeleteEntities",
                                   service => service.DeleteEntities((EntityCollection)iEntities,
                                                                     programName,
                                                                     application,
                                                                     module));
        }

        /// <summary>
        /// Processes a given list of entities based on their actions
        /// </summary>
        /// <param name="iEntities">Indicates the instance of entities to be processed in the MDMCenter system</param>
        /// <param name="programName">Indicates the name of the program which invoked the API</param>
        /// <param name="application">Indicates the name of the application which invoked the API</param>
        /// <param name="module">Indicates the name of the module which invoked the API</param>
        /// <returns>Returns the result of processing the entities</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        /// <example>
        /// <code language="c#" title="Process Entities Without EntityProcessingOptions" source="..\MDM.APISamples\EntityManager\Entity\ProcessEntity.cs" region="Process Entities Without EntityProcessingOptions" />
        /// </example>
        public IEntityOperationResultCollection ProcessEntities(IEntityCollection iEntities, String programName, MDMCenterApplication application, MDMCenterModules module)
        {
            return ProcessEntities(iEntities, MDMObjectFactory.GetIEntityProcessingOptions(), programName, application, module);
        }

        /// <summary>
        /// Processes a given list of entities based on their actions and entity processing options
        /// </summary>
        /// <param name="iEntities">Indicates the instance of entities to be processed in MDMCenter</param>
        /// <param name="iEntityProcessingOptions">Indicates the instance of Processing Option for the Processing Entities</param>
        /// <param name="programName">Indicates the name of the program from where the Process method is being called. This is used for log purpose.</param>
        /// <param name="application">Indicates the name of the application which is performing the action</param>
        /// <param name="module">Indicates the name of the module which is performing the action</param>
        /// <returns>Returns the result of processing the entities</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        /// <example>
        /// <code language="c#" title="Process Entities With EntityProcessingOptions" source="..\MDM.APISamples\EntityManager\Entity\ProcessEntity.cs" region="Process Entities With EntityProcessingOptions" />
        /// </example>
        public IEntityOperationResultCollection ProcessEntities(IEntityCollection iEntities, IEntityProcessingOptions iEntityProcessingOptions, String programName, MDMCenterApplication application, MDMCenterModules module)
        {
            CallerContext callerContext = new CallerContext() { Application = application, Module = module, ProgramName = programName };
            return this.ProcessEntities(iEntities, iEntityProcessingOptions, callerContext);
        }

        /// <summary>
        /// Processes a given list of entities based on their actions
        /// </summary>
        /// <param name="iEntities">Indicates the instance of the entities to be processed in the MDMCenter</param>
        /// <param name="iEntityProcessingOptions">Indicates the instance of Processing Option for the Processing Entities</param>
        /// <param name="iCallerContext">Indicates the name of application and the module that are performing the action</param>
        /// <returns>Returns the result of processing the entities</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        /// <example>
        /// <code language="c#" title="Process Entities With EntityProcessingOptions And CallerContext" source="..\MDM.APISamples\EntityManager\Entity\ProcessEntity.cs" region="Process Entities With EntityProcessingOptions And CallerContext" />
        /// <code language="c#" title="Process Entities Extension Relationships" source="..\MDM.APISamples\EntityManager\Entity\ProcessEntity.cs" region="Process Entities Extension Relationships" />
        /// </example>
        public IEntityOperationResultCollection ProcessEntities(IEntityCollection iEntities, IEntityProcessingOptions iEntityProcessingOptions, ICallerContext iCallerContext)
        {
            return MakeServiceCall("ProcessEntities",
                                   "ProcessEntities",
                                   client => client.ProcessEntities(
                                       (EntityCollection)iEntities,
                                       (EntityProcessingOptions)iEntityProcessingOptions,
                                       FillDiagnosticTraces(iCallerContext)),
                                   MDMTraceSource.EntityProcess);
        }
        
        /// <summary>
        /// Gets Entity by given External ID
        /// </summary>
        /// <param name="externalId">Indicates External ID for which Entity needs to be populated</param>
        /// <param name="iEntityContext">Indicates the data context for which Entity needs to be fetched</param>
        /// <param name="icontext">Indicates the application and the module name by which action is being performed</param>
        /// <returns>Returns Entity by External ID</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        public IEntity GetEntityByExternalId(String externalId, IEntityContext iEntityContext, ICallerContext icontext)
        {
            return MakeServiceCall("GetEntityByExternalId",
                                   "GetEntityByExternalId",
                                   service => service.GetEntityByExternalId(externalId,
                                                                            (EntityContext)iEntityContext,
                                                                            FillDiagnosticTraces(icontext)),
                                   MDMTraceSource.EntityGet,
                                   true);
        }

        /// <summary>
        /// Gets Entity By given External ID
        /// </summary>
        /// <param name="externalId">Indicates External ID for which Entity needs to be populated</param>
        /// <param name="iEntityContext">Indicates the data context for which Entity needs to be fetched</param>
        /// <param name="icontext">Indicates the application and the module name by which action is being performed</param>
        /// <param name="publishEvents">Indicates whether to publish events or not. Default is set to 'true'</param>
        /// <param name="applyAVS">Indicates whether to apply AVS or not. Default is set to 'true'</param>
        /// <returns>Returns Entity by External ID</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        public IEntity GetEntityByExternalId(String externalId, IEntityContext iEntityContext, ICallerContext icontext, Boolean publishEvents = true, Boolean applyAVS = true)
        {
            return MakeServiceCall("GetEntityByExternalId",
                                   "GetEntityByExternalId",
                                   service => service.GetEntityByExternalId(externalId,
                                                                            (EntityContext)iEntityContext,
                                                                            FillDiagnosticTraces(icontext),
                                                                            publishEvents,
                                                                            applyAVS),
                                   MDMTraceSource.EntityGet,
                                   true);
        }

        /// <summary>
        /// Gets an entity collection for the specified entity external identifiers and the entity context
        /// </summary>
        /// <param name="externalIdList">Lists the external id(s) for which the entities needs to be retrieved</param>
        /// <param name="entityContext">Specifies the entity context for which entities needs to be retrieved</param>
        /// <param name="callerContext">Specifies the caller context, which contains the application and module that has invoked the API</param>
        /// <param name="publishEvents">Specifies whether to publish events or not. Default value is set to 'false'</param>
        /// <param name="applyAVS">Specifies whether to apply AVS or not. Default value is set to 'false'</param>
        /// <returns>Returns the collection of entities</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        /// <example>
        /// <code language="c#" source="..\MDM.APISamples\EntityManager\Entity\GetEntity.cs" region="GetEntitiesByExternalIds" />
        /// </example>
        public IEntityCollection GetEntitiesByExternalIds(Collection<String> externalIdList, IEntityContext entityContext, ICallerContext callerContext, Boolean publishEvents = false, Boolean applyAVS = false)
        {
            return MakeServiceCall<EntityCollection>("GetEntitiesByExternalIds", "GetEntitiesByExternalIds",
                client => client.GetEntitiesByExternalIds(externalIdList, (EntityContext)entityContext, FillDiagnosticTraces(callerContext), publishEvents, applyAVS));
        }
        
        /// <summary>
        /// Updates entity attributes in bulk
        /// </summary>
        /// <param name="iTemplateEntities">Indicates collection of entity for which attributes needs to be update</param>
        /// <param name="entityIdsToProcess">Indicates collection of entity identifiers which needs to process</param>
        /// <param name="actionPerformed">Indicates name of the action which being performed</param>
        /// <param name="iCallerContext">Indicates application and module name by which operation is being performed</param>
        /// <returns>Returns collection of entity operation result for ensuring whether bulk entity attribute update is successful or not</returns>
        public IEntityOperationResultCollection BulkUpdateEntityAttributes(IEntityCollection iTemplateEntities, Collection<Int64> entityIdsToProcess, String actionPerformed, ICallerContext iCallerContext)
        {
            return MakeServiceCall("BulkUpdateEntityAttributes",
                                   "BulkUpdateEntityAttributes",
                                   service => service.BulkUpdateEntityAttributes(iTemplateEntities as EntityCollection,
                                                                                 entityIdsToProcess,
                                                                                 actionPerformed,
                                                                                 FillDiagnosticTraces(iCallerContext)));
        }
        
        #endregion

        #region Name Based Entity Methods

        /// <summary>
        /// Gets an entity by providing container name, category name, entity type name and parent entity name in entity context with publish events and apply avs as optional parameter
        /// </summary>
        /// <param name="entityId">Indicates entity id for which entity needs to be populated</param>
        /// <param name="iEntityContext">Indicates the data context for which entity needs to be fetched</param>
        /// <param name="callerContext">Indicates application and module name by which operation is being performed</param>
        /// <param name="publishEvents">Indicates whether to publish events or not. Default is set to 'true'</param>
        /// <param name="applyAVS">Indicates whether to apply AVS or not. Default is set to 'true'</param>
        /// <returns>Returns entity object</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        /// <example>
        /// <code language="c#" title="Get Entity Using ContextNames" source="..\MDM.APISamples\EntityManager\Entity\GetEntity.cs" region="GetEntityUsingContextNames" />
        /// </example>
        public IEntity GetEntityUsingContextNames(Int64 entityId, IEntityContext iEntityContext, ICallerContext callerContext, Boolean publishEvents = true, Boolean applyAVS = true)
        {
            ValidateEntityContext(iEntityContext);

            EntityContext entityContext = (EntityContext)iEntityContext;
            entityContext.ResolveIdsByName = true;
            return (IEntity)this.GetEntity(entityId, entityContext, callerContext.Application, callerContext.Module, publishEvents, applyAVS);
        }

        /// <summary>
        /// Gets an entity collection for specified entity ids by providing container name, category name, entity type name and parent entity name in entity context
        /// </summary>
        /// <param name="entityIdList">Indicates entity ids for which entity needs to be populated</param>
        /// <param name="iEntityContext">Indicates the data context for which entity needs to be fetched</param>
        /// <param name="callerContext">Indicates application and module name by which operation is being performed</param>
        /// <param name="publishEvents">Indicates whether to publish events or not. Default is set to 'false'</param>
        /// <param name="applyAVS">Indicates whether to apply AVS or not. Default is set to 'false'</param>
        /// <returns>Returns entity collection object</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        /// <example>
        /// <code language="c#" title="Get Entities Using ContextNames" source="..\MDM.APISamples\EntityManager\Entity\GetEntity.cs" region="GetEntitiesUsingContextNames" />
        /// </example>
        public IEntityCollection GetEntitiesUsingContextNames(Collection<Int64> entityIdList, IEntityContext iEntityContext, ICallerContext callerContext, Boolean publishEvents = false, Boolean applyAVS = false)
        {
            ValidateEntityContext(iEntityContext);

            EntityContext entityContext = (EntityContext)iEntityContext;
            entityContext.ResolveIdsByName = true;
            return (IEntityCollection)this.GetEntities(entityIdList, entityContext, callerContext.Application, callerContext.Module, publishEvents, applyAVS);
        }

        /// <summary>
        /// Gets an entity collection for the specified entity identifier's using entityGetOptions and providing container name, category name, entity type name, and parent entity name in the entity context
        /// </summary>
        /// <param name="entityIdList">Indicates entity ids for which entity needs to be populated</param>
        /// <param name="iEntityContext">Indicates the data context for which entity needs to be fetched</param>
        /// <param name="entityGetOptions">Indicates the options available while retrieving entity data</param>
        /// <param name="iCallerContext">Indicates application and module name by which operation is being performed</param>
        /// <returns>Returns entity collection object</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        /// <example>
        /// <code language="c#" title="Get Entities Using ContextNames With EntityGetOptions" source="..\MDM.APISamples\EntityManager\Entity\GetEntity.cs" region="GetEntitiesUsingContextNamesWithEntityGetOptions" />
        /// </example>
        public IEntityCollection GetEntitiesUsingContextNames(Collection<Int64> entityIdList, IEntityContext iEntityContext, IEntityGetOptions entityGetOptions, ICallerContext iCallerContext)
        {
            ValidateEntityContext(iEntityContext);

            EntityContext entityContext = (EntityContext)iEntityContext;
            entityContext.ResolveIdsByName = true;
            return (IEntityCollection)this.GetEntities(entityIdList, entityContext, entityGetOptions, iCallerContext);
        }

        /// <summary>
        /// Gets an entity for the specified external id by providing container name, category name, entity type name and parent entity name in entity context with publish events and apply avs as optional parameter
        /// </summary>
        /// <param name="externalId">Indicates external id for which entity needs to be populated</param>
        /// <param name="iEntityContext">Indicates the data context for which entity needs to be fetched</param>
        /// <param name="iCallerContext">Indicates application and module name by which operation is being performed</param>
        /// <param name="publishEvents">Indicates whether to publish events or not. Default is set to 'false'</param>
        /// <param name="applyAVS">Indicates whether to apply AVS or not. Default is set to 'false'</param>
        /// <returns>Returns entity object</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        /// <example>
        /// <code language="c#" title="Get Entity By External Id Using ContextNames" source="..\MDM.APISamples\EntityManager\Entity\GetEntity.cs" region="GetEntityByExternalIdUsingContextNamesWithPublishEventsandAVSOptions" />
        /// </example>
        public IEntity GetEntityByExternalIdUsingContextNames(String externalId, IEntityContext iEntityContext, ICallerContext iCallerContext, Boolean publishEvents = false, Boolean applyAVS = false)
        {
            ValidateEntityContext(iEntityContext);

            EntityContext entityContext = (EntityContext)iEntityContext;
            entityContext.ResolveIdsByName = true;
            return (IEntity)this.GetEntityByExternalId(externalId, entityContext, iCallerContext, publishEvents, applyAVS);
        }

        /// <summary>
        /// Creates an entity using the entity processing options
        /// </summary>
        /// <param name="iEntity">Indicates the new entity to be created</param>
        /// <param name="iEntityProcessingOptions">Indicates processing options for creating the new entity</param>
        /// <param name="callerContext">Indicates application and module name by which operation is being performed</param>
        /// <returns>Returns results of the operation having errors and information, if any</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDMCenter operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        /// <example>
        /// <code language="c#" title="Create an Entity Using ContextNames" source="..\MDM.APISamples\EntityManager\Entity\EntityProcessSamples.cs" region="CreateEntityUsingContextNames" />
        /// </example>
        public IEntityOperationResult CreateEntityUsingContextNames(IEntity iEntity, IEntityProcessingOptions iEntityProcessingOptions, ICallerContext callerContext)
        {
            ValidateEntityProcessingOptions(iEntityProcessingOptions);

            EntityProcessingOptions entityProcessingOptions = iEntityProcessingOptions as EntityProcessingOptions;
            entityProcessingOptions.ResolveIdsByNames = true;

            return this.MakeServiceCall<IEntityOperationResult>("CreateEntity", "CreateEntity",
                client => client.CreateEntity(iEntity as Entity, entityProcessingOptions, callerContext.ProgramName, callerContext.Application, callerContext.Module));
        }

        /// <summary>
        /// Updates an existing entity based on the entity processing options.
        /// </summary>
        /// <param name="iEntity">Indicates the entity to be updated </param>
        /// <param name="iEntityProcessingOptions">Indicates the processing options for updating the entity</param>
        /// <param name="callerContext">Indicates application and module name by which operation is being performed</param>
        /// <returns>Returns the results of the Operation having errors and information, if any</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDMCenter operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        /// <example>
        /// <code language="c#" title="Update an Entity Using ContextNames" source="..\MDM.APISamples\EntityManager\Entity\EntityProcessSamples.cs" region="UpdateEntityUsingContextNames" />
        /// <code language="c#" title="Get the entity from EntityUtility" source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntity" />
        /// </example>
        public IEntityOperationResult UpdateEntityUsingContextNames(IEntity iEntity, IEntityProcessingOptions iEntityProcessingOptions, ICallerContext callerContext)
        {
            ValidateEntityProcessingOptions(iEntityProcessingOptions);

            EntityProcessingOptions entityProcessingOptions = iEntityProcessingOptions as EntityProcessingOptions;
            entityProcessingOptions.ResolveIdsByNames = true;

            return this.MakeServiceCall<IEntityOperationResult>("UpdateEntityByContextNames", "UpdateEntityByContextNames",
                client => client.UpdateEntity(iEntity as Entity, entityProcessingOptions, callerContext.ProgramName, callerContext.Application, callerContext.Module));
        }

        /// <summary>
        ///  Updates existing entities based on the entity processing options.
        /// </summary>
        /// <param name="iEntities">Indicates the instance of the entities to be updated</param>
        /// <param name="iEntityProcessingOptions">Indicates the processing options for updating the entities</param>
        /// <param name="callerContext">Indicates application and module name by which operation is being performed</param>
        /// <returns>Returns results of the Operation having errors and information, if any</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDMCenter operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        /// <example>
        /// <code language="c#" title="Update Entities Using ContextNames" source="..\MDM.APISamples\EntityManager\Entity\EntityProcessSamples.cs" region="UpdateEntitiesUsingContextNames" />
        /// <code language="c#" title="Get the entities from EntityUtility" source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntity" />
        /// </example>
        public IEntityOperationResultCollection UpdateEntitiesUsingContextNames(IEntityCollection iEntities, IEntityProcessingOptions iEntityProcessingOptions, ICallerContext callerContext)
        {
            ValidateEntityProcessingOptions(iEntityProcessingOptions);

            EntityProcessingOptions entityProcessingOptions = iEntityProcessingOptions as EntityProcessingOptions;
            entityProcessingOptions.ResolveIdsByNames = true;

            return this.MakeServiceCall<IEntityOperationResultCollection>("UpdateEntitiesByContextNames", "UpdateEntitiesByContextNames",
              client => client.UpdateEntities(iEntities as EntityCollection, entityProcessingOptions, callerContext.ProgramName, callerContext.Application, callerContext.Module));
        }

        /// <summary>
        /// Processes a given list of entities based on their actions.
        /// </summary>
        /// <param name="iEntities">Indicates the instance of the entities to be processed in the MDM system</param>
        /// <param name="iEntityProcessingOptions">Indicates the instance of Processing Option for the Processing Entities</param>
        /// <param name="iCallerContext">Indicates the name of application and the module that are performing the action</param>
        /// <returns>Returns the results of the operation having errors and information, if any</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDMCenter operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        /// <example>
        /// <code language="c#" title="Process Entities Using ContextNames" source="..\MDM.APISamples\EntityManager\Entity\GetEntity.cs" region="ProcessEntitiesUsingContextNamesWithCallerContext" />
        /// </example>
        public IEntityOperationResultCollection ProcessEntitiesUsingContextNames(IEntityCollection iEntities, IEntityProcessingOptions iEntityProcessingOptions, ICallerContext iCallerContext)
        {
            ValidateEntityProcessingOptions(iEntityProcessingOptions);

            EntityProcessingOptions entityProcessingOptions = iEntityProcessingOptions as EntityProcessingOptions;
            entityProcessingOptions.ResolveIdsByNames = true;

            return this.ProcessEntities(iEntities, entityProcessingOptions as IEntityProcessingOptions, iCallerContext);
        }

        #endregion

        #region Entity Locale Methods

        /// <summary>
        /// Process given list of entities based on their actions and data locales
        /// </summary>
        /// <param name="dataXml">XML representing Entity - ID, Locale and action
        /// <example>
        ///  <Entities>
        ///            <Entity Id="190" EntityId = "134" Name="AP" LongName="Apparel" Locale="1" Action="Update" ></Entity>
        ///            <Entity Id="-1" EntityId = "134" Name="AP" LongName="Kleider" Locale="21" Action="Create" ></Entity>
        ///  </Entities>
        /// </example>
        /// </param>
        /// <param name="systemDataLocale">Indicates the system data locale</param>
        /// <param name="returnResult">Whether need to return result or not</param>
        /// <param name="callerContext">Indicates the CallerContext</param>
        /// <returns>Returns IEntityOperationResultCollection</returns>
        public IEntityOperationResultCollection EntityLocaleProcess(String dataXml, String systemDataLocale, Boolean returnResult, CallerContext callerContext)
        {
            return MakeServiceCall("EntityLocaleProcess",
                                   "ProcessEntityLocale",
                                   service =>
                                   {
                                       Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
                                       if (isTracingEnabled)
                                       {
                                           MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Requested Entity Locale Process for SystemDataLocale: {0}, ReturnResult: {1}", systemDataLocale, returnResult));
                                       }
                                       return service.ProcessEntityLocale(dataXml,
                                                                          systemDataLocale,
                                                                          returnResult,
                                                                          FillDiagnosticTraces(callerContext));
                                   });
        }

        /// <summary>
        /// Gets the Entity in given Locale 
        /// </summary>
        /// <param name="entityId">Indicates the Entity ID</param>
        /// <param name="datalocales">Indicates the data Locales</param>
        /// <param name="callerContext">Indicates the name of application and the module that are performing the action</param>
        /// <returns>Returns collection of Entity</returns>
        public IEntityCollection GetEntityLocale(Int64 entityId, Collection<Locale> datalocales, CallerContext callerContext)
        {
            return MakeServiceCall("GetEntityLocale",
                                   "GetEntityLocale",
                                   service =>
                                   {
                                       Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
                                       if (isTracingEnabled)
                                       {
                                           MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Requested Get Entity Locale for EntityID: {0}", entityId));
                                       }
                                       return service.GetEntityLocale(entityId,
                                                                      datalocales,
                                                                      FillDiagnosticTraces(callerContext));
                                   });
        }

        #endregion
        
        #region Entity Hierarchy Methods

        /// <summary>
        /// Gets the entity object along with its hierarchy based on the entity identifier and the entity context specified.
        /// </summary>
        /// <param name="entityUniqueIdentifier">Specifies the entity unique identifier for which entity hierarchy needs to be retrieved</param>
        /// <param name="entityDataContext">Indicates the entity context collection for which entities needs to be retrieved</param>
        /// <param name="entityGetOptions">Indicates the options available while retrieving entities in entity hierarchy</param>
        /// <param name="callerContext">Indicates the caller context, which contains the application and module that has invoked the API</param>
        /// <returns>Returns entity wth the hierarchy entities loaded based on the entity ID and requested context</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        /// <example>
        /// <code language="c#" title="Sample 1: Get all entities using parent entity ID" source="..\MDM.APISamples\EntityManager\EntityHierarchy\GetEntityHierarchy.cs" region="GetEntityHierarchySample1" />
        /// <code language="c#" title="Sample 2: Get all entities with attributes using parent entity ID and attribute IDs" source="..\MDM.APISamples\EntityManager\EntityHierarchy\GetEntityHierarchy.cs" region="GetEntityHierarchySample2" />
        /// <code language="c#" title="Sample 3: Get all entities with attributes using parent entity ID and attribute names" source="..\MDM.APISamples\EntityManager\EntityHierarchy\GetEntityHierarchy.cs" region="GetEntityHierarchySample3" />
        /// <code language="c#" title="Sample 4: Get all entities with different attributes using parent entity ID" source="..\MDM.APISamples\EntityManager\EntityHierarchy\GetEntityHierarchy.cs" region="GetEntityHierarchySample4" />
        /// <code language="c#" title="Sample 5: Get all entities with different attributes using parent entity name" source="..\MDM.APISamples\EntityManager\EntityHierarchy\GetEntityHierarchy.cs" region="GetEntityHierarchySample5" />
        /// <code language="c#" title="Sample 6: Get all entities with relationships using parent entity name" source="..\MDM.APISamples\EntityManager\EntityHierarchy\GetEntityHierarchy.cs" region="GetEntityHierarchySample6" />
        /// <code language="c#" title="Sample 7: Get all entities with different relationships using parent entity ID" source="..\MDM.APISamples\EntityManager\EntityHierarchy\GetEntityHierarchy.cs" region="GetEntityHierarchySample7" />
        /// <code language="c#" title="Sample 8: Get all entities with different attributes and relationships using parent entity ID" source="..\MDM.APISamples\EntityManager\EntityHierarchy\GetEntityHierarchy.cs" region="GetEntityHierarchySample8" />
        /// <code language="c#" title="Sample 9: Get all entities with different attributes and relationships using parent entity name" source="..\MDM.APISamples\EntityManager\EntityHierarchy\GetEntityHierarchy.cs" region="GetEntityHierarchySample9" />
        /// <code language="c#" title="Sample 10: Get all entities with attributes using child entity ID" source="..\MDM.APISamples\EntityManager\EntityHierarchy\GetEntityHierarchy.cs" region="GetEntityHierarchySample10" />
        /// <code language="c#" title="Sample 11: Get all entities with attributes using child entity ID" source="..\MDM.APISamples\EntityManager\EntityHierarchy\GetEntityHierarchy.cs" region="GetEntityHierarchySample11" />
        /// </example>
        public IEntity GetEntityHierarchy(IEntityUniqueIdentifier entityUniqueIdentifier, IEntityDataContext entityDataContext, IEntityGetOptions entityGetOptions, ICallerContext callerContext)
        {
            return this.MakeServiceCall<IEntity>("GetEntityHierarchy", "GetEntityHierarchy",
                   client => client.GetEntityHierarchy(entityUniqueIdentifier as EntityUniqueIdentifier, entityDataContext as EntityContext, entityGetOptions as EntityGetOptions, callerContext as CallerContext), useProtoBufSerialization: true);
        }
        
        /// <summary>
        /// Gets the IDs of Child entities by Entity Type
        /// </summary>
        /// <param name="entityIds">Indicates Entity IDs </param>
        /// <param name="entityTypeIds">Indicates Entity Type IDs </param>
        /// <returns>Returns collection of IDs under Entity Type</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        //[Obsolete("This method has been obsoleted. Please use DataService.GetEntityWithHierarchy method instead of this")]
        public Collection<Int64> GetChildEntitiesIdsByEntityType(Collection<Int64> entityIds, Collection<Int32> entityTypeIds)
        {
            return this.MakeServiceCall("GetChildEntitiesIdsByEntityType", "GetChildEntitiesIdsByEntityType", client => client.GetChildEntitiesIdsByEntityType(entityIds, entityTypeIds));
        }
        
        #endregion Entity Hierarchy Contracts

        #region File Methods

        /// <summary>        
        /// Gets file for the Entity
        /// </summary>
        /// <example>
        /// <code>
        /// Int32 fileId = 1; // Id in Tb_File
        /// IFile iFile = null;
        ///
        /// // Get MDM DataService
        /// DataService dataService = GetMDMDataService();
        ///
        /// // Below code will make WCF call to GetFile(Int32 FileId, Boolean getFileDetails)
        /// iFile = dataService.GetFile(fileId, false);
        ///
        /// </code>
        /// </example>
        /// <param name="fileId">Indicates file ID</param>    
        /// <param name="getFileDetails">Specifies whether to get File details or not</param>
        /// <returns>Returns file Object</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        public IFile GetFile(Int32 fileId, Boolean getFileDetails)
        {
            return MakeServiceCall("GetFile",
                                   "GetFile",
                                   service =>
                                   {
                                       Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
                                       if (isTracingEnabled)
                                       {
                                           MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Requested GetFile for FileID: {0}, getFileDetails: {1}", fileId, getFileDetails));
                                       }
                                       return service.GetFile(fileId, getFileDetails);
                                   });
        }

        /// <summary>        
        /// Gets collection of files using specified filter <paramref name="fileIdsFilter"/>.
        /// </summary>
        /// <example>
        /// <code>
        /// Collection&lt;Int32&gt; fileIds = new Collection&lt;Int32&gt;{1, 2, 3}; // IDs in tb_File
        ///
        /// // Get MDM DataService
        /// DataService dataService = GetMDMDataService();
        ///
        /// // Below code will make WCF call to GetFiles(Collection&lt;Int32&gt; fileIdsFilter, Boolean getOnlyFileDetails)
        /// FileCollection files = dataService.GetFiles(fileIds, false);
        /// </code>
        /// </example>
        /// <param name="fileIdsFilter">Indicates ids of files. All files will be returned if filter is empty.</param>
        /// <param name="getOnlyFileDetails">Indicates file content requesting status. Please set to True if you want only files metadata information except file content.</param>
        /// <returns>Returns collection of files</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        public IFileCollection GetFiles(Collection<Int32> fileIdsFilter, Boolean getOnlyFileDetails)
        {
            return MakeServiceCall("GetFiles", "GetFiles", service => service.GetFiles(fileIdsFilter, getOnlyFileDetails));
        }

        /// <summary>        
        /// Processing the file
        /// </summary>
        /// <param name="iFile">Indicates the  File details</param>
        /// <returns>The process status</returns>
        public Int32 ProcessFile(IFile iFile)
        {
            return MakeServiceCall("ProcessFile", "ProcessFile", client => client.ProcessFile(iFile as File));
        }

        #endregion File Methods

        #region EntityMap Contracts

        /// <summary>
        /// Gets the entity mapping for the requested external ID
        /// </summary>
        /// <param name="systemId">Indicates the External System ID</param>
        /// <param name="externalId">Indicates the Entity External ID</param>
        /// <param name="containerId">Indicates the Container ID of the Entity</param>
        /// <param name="entityTypeId">Indicates the Entity Type ID of the Entity</param>
        /// <param name="categoryId">Indicates the Category ID of the Entity</param>
        /// <param name="iEntityIdentificationMap">Indicates the Entity Identification Mappings</param>
        /// <param name="application">Indicates the name of the application which invoked this API</param>
        /// <param name="module">Indicates the name of the module which invoked this API</param>
        /// <returns>Returns Entity Map Object having base properties for requested externalId which defines its uniqueness</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        /// <example>
        /// <code language="c#" title="Get EntityMap" source="..\MDM.APISamples\EntityManager\Entity\GetEntity.cs" region="Get EntityMap" />
        /// </example>
        public IEntityMap GetEntityMap(String systemId, String externalId, Int32 containerId, Int32 entityTypeId, Int64 categoryId, IEntityIdentificationMap iEntityIdentificationMap, MDMCenterApplication application, MDMCenterModules module)
        {
            return MakeServiceCall("GetEntityMap",
                                   "GetEntityMap",
                                   service =>
                                   {
                                       Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
                                       if (isTracingEnabled)
                                       {
                                           MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Requested Get Entity Map for SystemId : {0}, externalID: {1}, ContainerId : {2}, EntityTypeId: {3}, CategoryId: {4}", systemId, externalId, containerId, entityTypeId, categoryId));
                                       }
                                       return service.GetEntityMap(
                                           systemId,
                                           externalId,
                                           containerId,
                                           entityTypeId,
                                           categoryId,
                                           (EntityIdentificationMap)iEntityIdentificationMap,
                                           application,
                                           module);
                                   });
        }

        /// <summary>
        /// Gets Entity Map Collection with basic properties
        /// </summary>
        /// <param name="iEntityMapCollection">Indicates the collection of entity maps which needs to be loaded</param>
        /// <param name="iEntityIdentificationMap">Indicates the Entity Identification Mappings</param>
        /// <param name="application">Indicates the name of the application which invoked this API</param>
        /// <param name="module">Indicates the name of the module which invoked this API</param>
        /// <returns>Result of the operation saying whether it is successful or not</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        /// <example>
        /// <code language="c#" title="Load InternalDetails" source="..\MDM.APISamples\EntityManager\Entity\GetEntity.cs" region="Load InternalDetails" />
        /// </example>
        public Boolean LoadInternalDetails(IEntityMapCollection iEntityMapCollection, IEntityIdentificationMap iEntityIdentificationMap, MDMCenterApplication application, MDMCenterModules module)
        {
            Boolean loadSuccessful = false;

            EntityMapCollection entityMapCollection =
                MakeServiceCall("LoadInternalDetails",
                                "LoadInternalDetails",
                                service => service.LoadInternalDetails((EntityMapCollection)iEntityMapCollection,
                                                                       (EntityIdentificationMap)iEntityIdentificationMap,
                                                                       application,
                                                                       module));

            if (entityMapCollection != null && entityMapCollection.Count > 0)
            {
                foreach (EntityMap entityMap in entityMapCollection)
                {
                    IEntityMap iEntityMap = iEntityMapCollection.FirstOrDefault(e => e.Id == entityMap.Id);

                    if (iEntityMap != null)
                    {
                        iEntityMap.Id = entityMap.Id;
                        iEntityMap.ObjectTypeId = entityMap.ObjectTypeId;
                        iEntityMap.ObjectType = entityMap.ObjectType;
                        iEntityMap.SystemId = entityMap.SystemId;
                        iEntityMap.ExternalId = entityMap.ExternalId;
                        iEntityMap.InternalId = entityMap.InternalId;
                        iEntityMap.ContainerId = entityMap.ContainerId;
                        iEntityMap.CategoryId = entityMap.CategoryId;
                        iEntityMap.EntityTypeId = entityMap.EntityTypeId;
                    }
                }

                loadSuccessful = true;
            }

            return loadSuccessful;
        }

        #endregion EntityMap Contracts

        #region Cache Management Methods

        /// <summary>
        /// Removes entire entity from cache, including all Attributes and relationships.
        /// </summary>
        /// <param name="iEntityCacheInvalidateContexts">EntityCacheInvalidateContextCollection to be removed from cache</param>
        /// <param name="iCallerContext">Indicates Application and Module name by which action is being performed</param>
        /// <returns>Flag for the status of the operation</returns>
        public Boolean RemoveEntityCache(IEntityCacheInvalidateContextCollection iEntityCacheInvalidateContexts, ICallerContext iCallerContext)
        {
            return this.MakeServiceCall<Boolean>("RemoveEntityCache", "RemoveEntityCache",
                client => client.RemoveEntityCache(iEntityCacheInvalidateContexts as EntityCacheInvalidateContextCollection, FillDiagnosticTraces(iCallerContext)));
        }

        #endregion
        
        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private IDataService GetClient(Boolean useProtoBufSerialization = false, MDMTraceSource traceSource = MDMTraceSource.General)
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            IDataService serviceClient = null;

            if (WCFServiceInstanceLoader.IsLocalInstancesEnabled())
            {
                serviceClient = WCFServiceInstanceLoader.GetServiceInstance<IDataService>();
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
                else if (_isProtoBufEndPointInitialized)
                {
                    this.EndPointConfigurationName = this.EndPointConfigurationName.Replace("_UsingProtoBuf", "");
                    _isProtoBufEndPointInitialized = false;

                    if (isTracingEnabled)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "ProtoBuf serialization disabled", traceSource);
                }

                DataServiceProxy dataServiceProxy = null;

                if (String.IsNullOrEmpty(this.EndPointConfigurationName) && this.EndpointAddress == null)
                    dataServiceProxy = new DataServiceProxy();
                else if (!String.IsNullOrEmpty(this.EndPointConfigurationName) && this.EndpointAddress == null)
                    dataServiceProxy = new DataServiceProxy(this.EndPointConfigurationName);
                else if (!String.IsNullOrEmpty(this.EndPointConfigurationName) && this.EndpointAddress != null)
                    dataServiceProxy = new DataServiceProxy(this.EndPointConfigurationName, this.EndpointAddress);

                if (this.AuthenticationType == Core.AuthenticationType.Forms)
                {
                    dataServiceProxy.ClientCredentials.UserName.UserName = this.UserName;
                    dataServiceProxy.ClientCredentials.UserName.Password = this.Password;
                    dataServiceProxy.ClientCredentials.ServiceCertificate.Authentication.CertificateValidationMode = System.ServiceModel.Security.X509CertificateValidationMode.None;
                }

                serviceClient = dataServiceProxy;
            }

            return serviceClient;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        private void DisposeClient(IDataService client)
        {
            if (client == null)
                return;

            if (client.GetType() == typeof(DataServiceProxy))
            {
                DataServiceProxy serviceClient = (DataServiceProxy)client;
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
        /// 
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="attributeIds"></param>
        /// <param name="attributeUniqueIdentifierCollection"></param>
        /// <param name="findAttributeModels"></param>
        /// <returns></returns>
        private EntityCollection FindEntitiesWithMissingAttributes(EntityCollection entities, Collection<Int32> attributeIds, Collection<IAttributeUniqueIdentifier> attributeUniqueIdentifierCollection, Boolean findAttributeModels)
        {
            EntityCollection entitiesWithMissingAttributes = new EntityCollection();

            foreach (Entity entity in entities)
            {
                Boolean anyAttributeMissing = false;
                AttributeCollection entityAttributes = entity.Attributes;
                AttributeModelCollection entityAttrModels = entity.AttributeModels;

                if ((entityAttributes == null || entityAttributes.Count < 1) || (findAttributeModels && (entityAttrModels == null || entityAttrModels.Count < 1)))
                {
                    entitiesWithMissingAttributes.Add(entity);
                    continue;
                }

                if (attributeIds != null && attributeIds.Count > 0)
                {
                    Collection<Int32> attributeInternalUniqueKeyList = entityAttributes.GetAttributeInternalUniqueKeyList();
                    Collection<Int32> attributeModelInternalUniqueKeyList = entityAttrModels.GetAttributeInternalUniqueKeyList();

                    foreach (Int32 attrId in attributeIds)
                    {
                        if (entity.EntityContext != null && entity.EntityContext.DataLocales != null && entity.EntityContext.DataLocales.Count > 0)
                        {
                            foreach (LocaleEnum locale in entity.EntityContext.DataLocales)
                            {
                                Int32 uniqueKey = Attribute.GetInternalUniqueKey(attrId, locale);

                                if (!attributeInternalUniqueKeyList.Contains(uniqueKey) ||
                                   (findAttributeModels && !attributeModelInternalUniqueKeyList.Contains(uniqueKey)))
                                {
                                    anyAttributeMissing = true;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            Int32 uniqueKey = Attribute.GetInternalUniqueKey(attrId, LocaleEnum.UnKnown);

                            if (!attributeInternalUniqueKeyList.Contains(uniqueKey) ||
                               (findAttributeModels && !attributeModelInternalUniqueKeyList.Contains(uniqueKey)))
                            {
                                anyAttributeMissing = true;
                                break;
                            }
                        }

                        if (anyAttributeMissing)
                        {
                            entitiesWithMissingAttributes.Add(entity);
                            break;
                        }
                    }
                }
                else if (attributeUniqueIdentifierCollection != null && attributeUniqueIdentifierCollection.Count > 0)
                {
                    Dictionary<String, KeyValuePair<Int32, LocaleEnum>> attributeIdLocaleDictionary = entityAttributes.GetAttributeIdLocaleListDictionary();
                    Dictionary<String, KeyValuePair<Int32, LocaleEnum>> attributeModelIdLocaleDictionary = entityAttrModels.GetAttributeIdLocaleListDictionary();

                    foreach (AttributeUniqueIdentifier attributeUniqueIdentifier in attributeUniqueIdentifierCollection)
                    {
                        if (entity.EntityContext != null && entity.EntityContext.DataLocales != null && entity.EntityContext.DataLocales.Count > 0)
                        {
                            // get list of attribute + locale combination which are not available in current entity object
                            foreach (LocaleEnum locale in entity.EntityContext.DataLocales)
                            {
                                String key = AttributeModelUtility.GetAttributeUniqueIdentifierKey(attributeUniqueIdentifier, locale);

                                if (!attributeIdLocaleDictionary.ContainsKey(key) ||
                                   (findAttributeModels && !attributeModelIdLocaleDictionary.ContainsKey(key)))
                                {
                                    anyAttributeMissing = true;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            String key = AttributeModelUtility.GetAttributeUniqueIdentifierKey(attributeUniqueIdentifier, LocaleEnum.UnKnown);

                            if (!attributeIdLocaleDictionary.ContainsKey(key) ||
                               (findAttributeModels && !attributeModelIdLocaleDictionary.ContainsKey(key)))
                            {
                                anyAttributeMissing = true;
                                break;
                            }
                        }

                        if (anyAttributeMissing)
                        {
                            entitiesWithMissingAttributes.Add(entity);
                            break;
                        }
                    }
                }
            }

            return entitiesWithMissingAttributes;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceEntities"></param>
        /// <param name="targetEntities"></param>
        /// <param name="attributeIds"></param>
        /// <param name="iAttributeUniqueIdentifiers"></param>
        /// <param name="fillAttributeModels"></param>
        /// <returns></returns>
        private Boolean FillEntitiesWithMissingAttributes(EntityCollection sourceEntities, EntityCollection targetEntities, Collection<Int32> attributeIds, Collection<IAttributeUniqueIdentifier> iAttributeUniqueIdentifiers, Boolean fillAttributeModels)
        {
            Boolean isEnsureSuccessful = false;
            Boolean isAddSuccessful = false;

            if (sourceEntities != null && targetEntities != null)
            {
                foreach (Entity sourceEntity in sourceEntities)
                {
                    Entity targetEntity = (Entity)targetEntities.GetEntity(sourceEntity.Id);

                    if (targetEntity != null)
                    {
                        if (targetEntity.Attributes == null)
                        {
                            targetEntity.Attributes = new AttributeCollection();
                        }

                        if (targetEntity.AttributeModels == null && fillAttributeModels)
                        {
                            targetEntity.AttributeModels = new AttributeModelCollection();
                        }

                        if (attributeIds != null && attributeIds.Count > 0)
                        {
                            foreach (Int32 attrId in attributeIds)
                            {
                                if (sourceEntity.EntityContext != null && sourceEntity.EntityContext.DataLocales != null && sourceEntity.EntityContext.DataLocales.Count > 0)
                                {
                                    foreach (LocaleEnum locale in sourceEntity.EntityContext.DataLocales)
                                    {
                                        isAddSuccessful = AddAttribute(sourceEntity, targetEntity, attrId, null, locale, fillAttributeModels);

                                        if (isAddSuccessful)
                                        {
                                            isEnsureSuccessful = true;
                                        }
                                    }
                                }
                                else
                                {
                                    isAddSuccessful = AddAttribute(sourceEntity, targetEntity, attrId, null, LocaleEnum.UnKnown, fillAttributeModels);

                                    if (isAddSuccessful)
                                    {
                                        isEnsureSuccessful = true;
                                    }
                                }
                            }
                        }
                        else if (iAttributeUniqueIdentifiers != null && iAttributeUniqueIdentifiers.Count > 0)
                        {
                            foreach (IAttributeUniqueIdentifier iAttrUID in iAttributeUniqueIdentifiers)
                            {
                                if (sourceEntity.EntityContext != null && sourceEntity.EntityContext.DataLocales != null && sourceEntity.EntityContext.DataLocales.Count > 0)
                                {
                                    foreach (LocaleEnum locale in sourceEntity.EntityContext.DataLocales)
                                    {
                                        isAddSuccessful = AddAttribute(sourceEntity, targetEntity, 0, iAttrUID, locale, fillAttributeModels);

                                        if (isAddSuccessful)
                                        {
                                            isEnsureSuccessful = true;
                                        }
                                    }
                                }
                                else
                                {
                                    isAddSuccessful = AddAttribute(sourceEntity, targetEntity, 0, iAttrUID, LocaleEnum.UnKnown, fillAttributeModels);

                                    if (isAddSuccessful)
                                    {
                                        isEnsureSuccessful = true;
                                    }
                                }
                            }
                        }
                        else
                        {
                            foreach (Attribute attribute in sourceEntity.GetAttributes())
                            {
                                isAddSuccessful = AddAttribute(sourceEntity, targetEntity, attribute.Id, null, attribute.Locale, fillAttributeModels);

                                if (isAddSuccessful)
                                {
                                    isEnsureSuccessful = true;
                                }
                            }
                        }
                    }
                }
            }

            return isEnsureSuccessful;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceEntity"></param>
        /// <param name="targetEntity"></param>
        /// <param name="attrId"></param>
        /// <param name="iAttrUniqueIdentifier"></param>
        /// <param name="locale"></param>
        /// <param name="addAttributeModel"></param>
        /// <returns></returns>
        private Boolean AddAttribute(Entity sourceEntity, Entity targetEntity, Int32 attrId, IAttributeUniqueIdentifier iAttrUniqueIdentifier, LocaleEnum locale, Boolean addAttributeModel)
        {
            Boolean isAddSuccessful = false;
            IAttribute sourceAttribute = null;
            IAttributeModel sourceAttrModel = null;

            Entity originalEntity = targetEntity.OriginalEntity;

            if (attrId > 0)
            {
                sourceAttribute = sourceEntity.GetAttribute(attrId, locale);

                if (addAttributeModel)
                {
                    sourceAttrModel = sourceEntity.AttributeModels.GetAttributeModel(attrId, locale);
                }
            }
            else if (iAttrUniqueIdentifier != null)
            {
                sourceAttribute = sourceEntity.GetAttribute(iAttrUniqueIdentifier, locale);

                if (addAttributeModel)
                {
                    sourceAttrModel = sourceEntity.AttributeModels.GetAttributeModel(iAttrUniqueIdentifier, locale);
                }
            }

            if (sourceAttribute != null && !targetEntity.Attributes.Contains(sourceAttribute))
            {
                targetEntity.Attributes.Add(sourceAttribute);
            }

            if (sourceAttrModel != null && !targetEntity.AttributeModels.Contains(sourceAttrModel))
            {
                targetEntity.AttributeModels.Add(sourceAttrModel);
            }

            if (sourceAttribute != null && targetEntity.Attributes.Contains(sourceAttribute.Id, sourceAttribute.Locale)) //TODO:: WHat abt AM?
                isAddSuccessful = true;

            return isAddSuccessful;
        }

        /// <summary>
        /// Makes the DataServiceClient call. Creates the client instance, executes call delegate against it in
        /// impersonated context (if necessary) and then disposes the client.
        /// Also emits traces when necessary.
        /// </summary>
        /// <typeparam name="TResult">Indicates type of the result of service call.</typeparam>
        /// <param name="clientMethodName">Indicates name of the client method to include in traces.</param>
        /// <param name="serverMethodName">Indicates name of the server method to include in traces.</param>
        /// <param name="call">Indicates call delegate to be executed against a client instance.</param>
        /// <param name="traceSource">Indicates name of the trace source</param>
        /// <param name="useProtoBufSerialization">Indicates protobuf serialization should be on or off</param>
        /// <returns>Returns value returned by service, or default.</returns>
        private TResult MakeServiceCall<TResult>(String clientMethodName, String serverMethodName, Func<IDataService, TResult> call, MDMTraceSource traceSource = MDMTraceSource.APIFramework, Boolean useProtoBufSerialization = false)
        {
            #region Diagnostics & Tracing

            ExecutionContext executionContext = new ExecutionContext(traceSource);
            DiagnosticActivity activity = new DiagnosticActivity(executionContext, "DataServiceClient." + clientMethodName);
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;

            //Start trace activity
            if (isTracingEnabled)
            {
                activity.Start();
            }

            #endregion


            TResult result = default(TResult);
            IDataService dataServiceClient = null;

            try
            {
                dataServiceClient = GetClient(useProtoBufSerialization, traceSource);

                ValidateContext();

                if (isTracingEnabled)
                {
                    activity.LogVerbose("DataServiceClient sends '" + serverMethodName + "' request message.");
                }

                result = Impersonate(() => call(dataServiceClient));

                if (isTracingEnabled)
                {
                    activity.LogVerbose("DataServiceClient receives '" + serverMethodName + "' request message.");
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            finally
            {
                //Close the client
                DisposeClient(dataServiceClient);

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
        /// <param name="iEntityContext"></param>
        private void ValidateEntityContext(IEntityContext iEntityContext)
        {
            if (iEntityContext == null)
            {
                throw new ArgumentNullException("iEntityContext");
            }
        }

        /// <summary>
        /// Throws argument null exception if caller context object is null.
        /// </summary>
        /// <param name="iCallerContext">Represents caller context</param>
        private void ValidateCallerContext(ICallerContext iCallerContext)
        {
            if (iCallerContext == null)
            {
                throw new ArgumentNullException("iCallerContext");
            }
        }

        /// <summary>
        /// Throws argument null exception if EntityProcessingOptions object is null.
        /// </summary>
        /// <param name="iEntityProcessingOptions">Represents entity processing option</param>
        private void ValidateEntityProcessingOptions(IEntityProcessingOptions iEntityProcessingOptions)
        {
            if (iEntityProcessingOptions == null)
            {
                throw new ArgumentNullException("iEntityProcessingOptions");
            }
        }

        #endregion
    }
}