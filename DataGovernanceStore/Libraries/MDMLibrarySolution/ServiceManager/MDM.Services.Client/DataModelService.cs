using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Security.Principal;
using System.ServiceModel;

namespace MDM.Services
{
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.DynamicTableSchema;
    using MDM.BusinessObjects.Interfaces;
    using MDM.Core;
    using MDM.Core.Exceptions;
    using MDM.Interfaces;
    using MDM.Services.ServiceProxies;
    using MDM.Utility;
    using MDM.WCFServiceInterfaces;

    /// <summary>
    /// DataModel Service facilitates to work with MDMCenter data models. 
    /// This includes fetching and processing data model elements like organizations, hierarchies (taxonomies), containers, 
    /// entity types, categories, attribute models and various mappings.
    /// </summary>   
    public class DataModelService : ServiceClientBase
    {
        #region Fields

        #endregion

        #region Properties

        #endregion

        #region Constructors

        /// <summary>
        /// Use this default constructor only when calling MDM service within the execution context of an MDM Application. 
        /// Example: Service call from the MDM business rule.
        /// This would try to use current execution security context for service authentication purpose. 
        /// </summary>
        public DataModelService()
            : base(typeof(DataModelService))
        {
        }

        /// <summary>
        /// Use this default constructor with binding configuration name only when calling MDM service within the execution context of an MDM Application. 
        /// Example: Service call from the MDM business rule.
        /// This would try to use current execution security context for service authentication purpose. 
        /// </summary>
        /// <param name="endPointConfigurationName">Indicates the EndPointConfigurationName</param>
        public DataModelService(String endPointConfigurationName)
            : base(typeof(DataModelService), endPointConfigurationName)
        {
        }

        /// <summary>
        /// Use this constructor for form based authentication passing username and password
        /// </summary>
        /// <param name="endPointConfigurationName">Indicates the EndPointConfigurationName</param>
        /// <param name="userName">Name of the Login User</param>
        /// <param name="password">Password of the User</param>
        public DataModelService(String endPointConfigurationName, String userName, String password)
            : base(endPointConfigurationName, userName, password)
        {

        }

        /// <summary>
        /// Use this constructor for window authentication passing current windows identity.
        /// </summary>
        /// <param name="endPointConfigurationName">Indicates the EndPointConfigurationName</param>
        /// <param name="userIdentity">Indicates User's Identity</param>
        public DataModelService(String endPointConfigurationName, IIdentity userIdentity)
            : base(endPointConfigurationName, userIdentity)
        {
        }

        /// <summary>
        /// Constructor with client configuration.
        /// </summary>
        /// <param name="wcfClientConfiguration">Indicates WCF Client Configuration</param>
        public DataModelService(IWCFClientConfiguration wcfClientConfiguration)
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
        public DataModelService(String endPointConfigurationName, AuthenticationType authenticationType, IIdentity userIdentity, String userName, String password)
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
        public DataModelService(String endPointConfigurationName, EndpointAddress endPointAddress, AuthenticationType authenticationType, IIdentity userIdentity, String userName, String password)
            : base(endPointConfigurationName, endPointAddress, authenticationType, userIdentity, userName, password)
        {
        }

        #endregion

        #region Attribute Model Methods

        /// <summary>
        /// Gets the attribute model for the requested Attribute model context
        /// </summary>
        /// <example>
        /// <code>
        /// 
        /// // Get new instance of IAttributeModelContext and iAttributeModelCollection using MDMObjectFactory
        ///    IAttributeModelContext iAttributeModelContext = MDMObjectFactory.GetIAttributeModelContext();
        ///    IAttributeModelCollection iAttributeModelCollection = MDMObjectFactory.GetIAttributeModelCollection();
        ///
        /// // Set the Locale in which the lookup data needs to returned
        ///    LocaleEnum locale = LocaleEnum.en_WW;
        ///
        /// // Get MDM dataModel service
        ///    DataModelService mdmDataModelService = GetMDMDataModelService();
        ///
        /// // It is HIGHLY RECOMMENDED to set below properties for better performance of the API system
        ///    iAttributeModelContext.AttributeModelType = AttributeModelType.All;
        ///    iAttributeModelContext.Locales.Add(locale);
        ///    iAttributeModelContext.GetCompleteDetailsOfAttribute = true;
        ///    iAttributeModelContext.GetOnlyShowAtCreationAttributes = true;
        ///    iAttributeModelContext.CategoryId = 1;// Apparel
        ///    iAttributeModelContext.ContainerId = 5;// Product Master
        ///    iAttributeModelContext.EntityTypeId = 16;// Style
        ///
        /// // Make a WCF call to dataModelService.GetAttributeModelsByContext
        ///    iAttributeModelCollection = mdmDataModelService.GetAttributeModelsByContext(iAttributeModelContext);
        ///
        /// </code>
        /// </example>
        /// <param name="iAttributeModelContext">Indicates the data context for which model needs to be fetched</param>
        /// <returns>Returns Attribute Model Object</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        public IAttributeModelCollection GetAttributeModelsByContext(IAttributeModelContext iAttributeModelContext)
        {
            CallerContext callerContext = new CallerContext(MDMCenterApplication.MDMCenter, MDMCenterModules.Entity);
            AttributeModelContext attributeModelContext = (AttributeModelContext)iAttributeModelContext;
            return MakeServiceCall("GetAttributeModelsByContext",
                                   "GetAttributeModelsByContext",
                                   service => service.GetAttributeModelsByContext(
                                       attributeModelContext,
                                       FillDiagnosticTraces(callerContext)));
        }

        /// <summary>
        /// Gets the attribute model for the requested Attribute model context
        /// </summary>
        /// <param name="iAttributeModelContext">Indicates the data context for which model needs to be fetched</param>
        /// <param name="iCallerContext">Indicates the caller context</param>
        /// <returns>Returns Attribute Model Collection Object</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        /// <example>
        /// <code language="c#" title="Get Common AttributeModels By ContainerName And EntityTypeName" source="..\MDM.APISamples\EntityManager\AttributeModel\GetAttributeModel.cs" region="GetCommonAttributeModelsByContainerNameAndEntityTypeName" />
        /// <code language="c#" title="Get Common AttributeModels By ContainerName And EntityTypeName for Locale" source="..\MDM.APISamples\EntityManager\AttributeModel\GetAttributeModel.cs" region="GetCommonAttributeModelsByContainerNameAndEntityTypeNameforLocale" />
        /// <code language="c#" title="Get Required Common AttributeModels By ContainerName And EntityTypeName for Locale" source="..\MDM.APISamples\EntityManager\AttributeModel\GetAttributeModel.cs" region="GetRequiredCommonAttributeModelsByContainerNameAndEntityTypeNameforLocale" />
        /// <code language="c#" title="Get Technical AttributeModels By CategoryName" source="..\MDM.APISamples\EntityManager\AttributeModel\GetAttributeModel.cs" region="GetTechnicalAttributeModelsByCategoryName" />
        /// <code language="c#" title="Get Technical AttributeModels By CategoryName for Locale" source="..\MDM.APISamples\EntityManager\AttributeModel\GetAttributeModel.cs" region="GetTechnicalAttributeModelsByCategoryNameforLocale" />
        /// <code language="c#" title="Get Required Technical AttributeModels By CategoryName for Locale" source="..\MDM.APISamples\EntityManager\AttributeModel\GetAttributeModel.cs" region="GetRequiredTechnicalAttributeModelsByCategoryNameforLocale" />
        /// <code language="c#" title="Get Relationship AttributeModels By ContainerName And RelationshipTypeName" source="..\MDM.APISamples\EntityManager\AttributeModel\GetAttributeModel.cs" region="GetRelationshipAttributeModelsByContainerNameAndRelationshipTypeName" />
        /// <code language="c#" title="Get Relationship AttributeModels By ContainerName And RelationshipTypeName for Locale" source="..\MDM.APISamples\EntityManager\AttributeModel\GetAttributeModel.cs" region="GetRelationshipAttributeModelsByContainerNameAndRelationshipTypeNameforLocale" />
        /// <code language="c#" title="Get Required Relationship AttributeModels By ContainerName And RelationshipTypeName for Locale" source="..\MDM.APISamples\EntityManager\AttributeModel\GetAttributeModel.cs" region="GetRequiredRelationshipAttributeModelsByContainerNameAndRelationshipTypeNameforLocale" />
        /// <code language="c#" title="Get ShowAtCreation AttributeModels By ContainerName And EntityTypeName" source="..\MDM.APISamples\EntityManager\AttributeModel\GetAttributeModel.cs" region="GetShowAtCreationAttributeModelsByContainerNameAndEntityTypeName" />
        /// </example>
        public IAttributeModelCollection GetAttributeModels(IAttributeModelContext iAttributeModelContext, ICallerContext iCallerContext)
        {
            return MakeServiceCall<IAttributeModelCollection>("GetAttributeModels", "GetAttributeModelsByContext",
                    client => client.GetAttributeModelsByContext(iAttributeModelContext as AttributeModelContext, FillDiagnosticTraces(iCallerContext)));
        }

        /// <summary>
        /// Gets all the base attribute models
        /// </summary>
        /// <returns>Attribute Model Collection Object</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        public IAttributeModelCollection GetAllBaseAttributeModels()
        {
            return MakeServiceCall("GetAllBaseAttributeModels",
                                   "GetAttributeModelsByContext",
                                   service => service.GetAllBaseAttributeModels());
        }

        /// <summary>
        /// Get Attribute models by received Ids
        /// </summary>
        /// <example>
        /// <code>
        ///  DataModelService dataModelService = new DataModelService();
        ///
        /// List<![CDATA[<Int32>]]> attrIds = new List<![CDATA[<Int32>]]>() { 4051, 4052 };
        ///
        /// IAttributeModelCollection attributes =
        /// dataModelService.GetBaseAttributeModelsByIds(new Collection<![CDATA[<Int32>]]>(attrIds), new CallerContext());
        /// </code>
        /// </example>
        /// <param name="attributeIds">Indicates which Ids should be used to get Attribute Model Ids</param>
        /// <param name="callerContext">Context indicating the caller of the API</param>
        /// <returns>Attribute Model Collection object</returns>
        public IAttributeModelCollection GetBaseAttributeModelsByIds(ICollection<Int32> attributeIds, ICallerContext callerContext)
        {
            return MakeServiceCall<IAttributeModelCollection>("GetBaseAttributeModelsByIds", "GetBaseAttributeModelsByIds",
                                                  client =>
                                                      client.GetBaseAttributeModelsByIds(attributeIds as Collection<Int32>, FillDiagnosticTraces(callerContext)));
        }

        /// <summary>
        /// Gets the Attribute Model for the requested Attribute ID
        /// </summary>
        /// <example>
        /// <code>
        /// 
        ///    DataModelService dataModelService = GetMDMDataModelService();
        ///    IAttributeModelCollection attributeModels = MDMObjectFactory.GetIAttributeModelCollection();
        ///
        /// // Assumption: Entity is assigned hard coded variables in this sample
        ///    Int32 attributeId = 4027;
        ///
        /// // Get new instance of IAttributeModelContext using MDMObjectFactory
        ///    IAttributeModelContext attributeModelContext = MDMObjectFactory.GetIAttributeModelContext();
        ///
        /// // Set the Locale in which the Lookup data needs to returned
        ///    LocaleEnum locale = LocaleEnum.en_WW;
        ///
        /// // Set caller context for getting Attribute Model
        ///    ICallerContext callerContext = MDMObjectFactory.GetICallerContext();
        ///    callerContext.Application = MDMCenterApplication.PIM;
        ///    callerContext.Module = MDMCenterModules.Entity;
        ///    
        /// // It is HIGHLY RECOMMENDED to set below properties for better performance of the API system
        ///    attributeModelContext.AttributeModelType = AttributeModelType.Common;
        ///    attributeModelContext.Locales.Add(locale);
        ///    attributeModelContext.CategoryId = 1;//Apparel
        ///    attributeModelContext.ContainerId = 5;//Product Master
        ///    attributeModelContext.EntityTypeId = 16;// Style
        ///
        /// // Make a WCF call to dataModelService.GetAttributeModel
        ///    attributeModels = dataModelService.GetAttributeModel(attributeId, attributeModelContext, callerContext);
        ///
        /// </code>
        /// </example>
        /// <param name="attributeId">Indicates ID of the Attribute for which model is required</param>
        /// <param name="iAttributeModelContext">Indicates the data context for which model needs to be fetched</param>
        /// <param name="iCallerContext">Indicates the name of the application and the module that are performing the action</param>
        /// <returns>Returns Attribute Model Collection Object</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        public IAttributeModelCollection GetAttributeModel(Int32 attributeId, IAttributeModelContext iAttributeModelContext, ICallerContext iCallerContext)
        {
            AttributeModelContext attributeModelContext = (AttributeModelContext)iAttributeModelContext;
            return MakeServiceCall(
                "GetAttributeModel",
                "GetAttributeModel",
                service =>
                {
                    Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
                    if (isTracingEnabled)
                    {
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Requested Attribute Model for Attribute Id: {0}", attributeId), MDMTraceSource.AttributeModelGet);
                    }
                    return service.GetAttributeModel(attributeId,
                                                     attributeModelContext,
                                                     FillDiagnosticTraces(iCallerContext));
                });
        }

        /// <summary>
        /// Gets the Attribute models for the requested Attribute IDs
        /// </summary>
        /// <example>
        /// <code>
        /// 
        /// // Get new instance of IAttributeModelContext and iAttributeModelCollection using MDMObjectFactory
        ///    IAttributeModelContext iAttributeModelContext = MDMObjectFactory.GetIAttributeModelContext();
        ///    IAttributeModelCollection iAttributeModelCollection = MDMObjectFactory.GetIAttributeModelCollection();
        ///
        /// // Assumption: Entity is assigned hard coded variables in this sample
        ///    <![CDATA[Collection<Int32>]]> attributeIdList = new <![CDATA[Collection<int>()]]> { 4027, 4036, 4039 };
        ///
        /// // Set the Locale in which the lookup data needs to returned
        ///    LocaleEnum locale = LocaleEnum.en_WW;
        ///
        /// // Get MDM dataModel service
        ///    DataModelService mdmDataModelService = GetMDMDataModelService();
        ///
        /// // It is HIGHLY RECOMMENDED to set below properties for better performance of the API system
        ///    iAttributeModelContext.AttributeModelType = AttributeModelType.Common;
        ///    iAttributeModelContext.Locales.Add(locale);
        ///    iAttributeModelContext.GetCompleteDetailsOfAttribute = true;
        ///    iAttributeModelContext.CategoryId = 1;//Apparel
        ///    iAttributeModelContext.ContainerId = 5;//Product Master
        ///    iAttributeModelContext.EntityTypeId = 16;// Style
        ///
        /// // Make a WCF call to dataModelService.GetAttributeModelsByIds
        ///    iAttributeModelCollection = mdmDataModelService.GetAttributeModelsByIds(attributeIdList, iAttributeModelContext);
        ///
        /// </code>
        /// </example>
        /// <param name="attributeIds">Indicates array of Attribute IDs for which models are required</param>
        /// <param name="iAttributeModelContext">Indicates the data context for which models needs to be fetched</param>
        /// <returns>Returns Collection of Attribute Model Objects</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        public IAttributeModelCollection GetAttributeModelsByIds(Collection<Int32> attributeIds, IAttributeModelContext iAttributeModelContext)
        {
            AttributeModelContext attributeModelContext = (AttributeModelContext)iAttributeModelContext;
            return MakeServiceCall("GetAttributeModelsByIds",
                                   "GetAttributeModelsByIds",
                                   service => service.GetAttributeModelsByIds(attributeIds, attributeModelContext));
        }

        /// <summary>
        /// Gets the attribute models for the requested attribute group ids
        /// </summary>
        /// <param name="attributeGroupIds">Ids of the attribute group for which models are required</param>
        /// <param name="excludeAttributeIds">Ids which needs to be excluded</param>
        /// <param name="iAttributeModelContext">The data context for which model needs to be fetched</param>
        /// <returns>Collection of Attribute Models object</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        public IAttributeModelCollection GetAttributeModelsByGroupIds(Collection<Int32> attributeGroupIds, Collection<Int32> excludeAttributeIds, IAttributeModelContext iAttributeModelContext)
        {
            AttributeModelContext attributeModelContext = (AttributeModelContext)iAttributeModelContext;
            return MakeServiceCall("GetAttributeModelsByGroupIds",
                                   "GetAttributeModelsByGroupIds",
                                   service => service.GetAttributeModelsByGroupIds(attributeGroupIds,
                                                                                   excludeAttributeIds,
                                                                                   attributeModelContext));
        }

        /// <summary>
        /// Gets the attribute models for the requested custom view id
        /// </summary>
        /// <param name="customViewId">Id of the custom view for which models are required</param>
        /// <param name="excludeAttributeIds">Ids which needs to be excluded</param>
        /// <param name="iAttributeModelContext">The data context for which model needs to be fetched</param>
        /// <returns>Collection of Attribute Models object</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        public IAttributeModelCollection GetAttributeModelsByCustomViewId(Int32 customViewId, Collection<Int32> excludeAttributeIds, IAttributeModelContext iAttributeModelContext)
        {
            AttributeModelContext attributeModelContext = (AttributeModelContext)iAttributeModelContext;
            return MakeServiceCall("GetAttributeModelsByCustomViewId",
                                   "GetAttributeModelsByCustomViewId",
                                   service => service.GetAttributeModelsByCustomViewId(customViewId,
                                                                                       excludeAttributeIds,
                                                                                       attributeModelContext));
        }

        /// <summary>
        /// Gets the attribute models for the requested state view id
        /// </summary>
        /// <example>
        /// <code>
        /// // Setting the state view id
        /// Int32 stateViewId = 1;
        /// // Attribute Ids that needs to be excluded
        /// Collection<![CDATA[<Int32>]]> excludeAttributeIds = new Collection<![CDATA[<Int32>]]>() { 4001, 4002, 4003 };
        /// 
        /// // Get new instance of IAttributeModelContext and iAttributeModelCollection using MDMObjectFactory
        /// IAttributeModelContext iAttributeModelContext = MDMObjectFactory.GetIAttributeModelContext();
        /// 
        /// // Set the Locale in which lookup data needs to returned
        /// LocaleEnum locale = LocaleEnum.en_WW;
        /// // Get MDM dataModel service
        /// DataModelService mdmDataModelService = InitializeServiceWithUserNameAndPassword();
        /// iAttributeModelContext.AttributeModelType = AttributeModelType.Common;
        /// iAttributeModelContext.Locales.Add(locale);
        /// iAttributeModelContext.GetCompleteDetailsOfAttribute = true;
        /// iAttributeModelContext.CategoryId = 1; // For example: Apparel
        /// iAttributeModelContext.ContainerId = 5; // For example: Product Master
        /// iAttributeModelContext.EntityTypeId = 16; // For example: Style
        /// // Make a WCF call to dataModelService.GetAttributeModelsByIds
        /// IAttributeModelCollection iAttributeModelCollection = mdmDataModelService.GetAttributeModelsByStateViewId(stateViewId, excludeAttributeIds, iAttributeModelContext);
        /// 
        /// </code>
        /// </example>
        /// <param name="stateViewId">Indicates the state view id for which models are required</param>
        /// <param name="excludeAttributeIds">Indicates the attribute ids that needs to be excluded</param>
        /// <param name="iAttributeModelContext">Indicates the data context for which model needs to be fetched</param>
        /// <returns>Returns the collection of attribute models object</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        public IAttributeModelCollection GetAttributeModelsByStateViewId(Int32 stateViewId, Collection<Int32> excludeAttributeIds, IAttributeModelContext iAttributeModelContext)
        {
            AttributeModelContext attributeModelContext = (AttributeModelContext)iAttributeModelContext;
            return MakeServiceCall("GetAttributeModelsByStateViewId",
                                   "GetAttributeModelsByStateViewId",
                                   service =>
                                   {
                                       Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
                                       if (isTracingEnabled)
                                       {
                                           MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Requested Get Attribute Models By StateViewId for StateViewId: {0}", stateViewId), MDMTraceSource.AttributeModelGet);
                                       }
                                       return service.GetAttributeModelsByStateViewId(stateViewId,
                                                                                      excludeAttributeIds,
                                                                                      attributeModelContext);
                                   });
        }

        /// <summary>
        /// Gets the attribute models for the specified attribute ids, attribute group ids, custom view id, state view id, exclude attribute ids and attribute model context
        /// </summary>
        /// <param name="attributeIds">Indicates IDs of the attributes for which models are required</param>
        /// <param name="attributeGroupIds">Indicates IDs of Attribute groups for which models are required</param>
        /// <param name="customViewId">Indicates Custom View ID of which models are required</param>
        /// <param name="stateViewId">Indicates State View ID of which models are required</param>
        /// <param name="excludeAttributeIds">Indicates Attribute IDs which needs to be excluded</param>
        /// <param name="iAttributeModelContext">Indicates the data context for which models needs to be fetched</param>
        /// <returns>Returns collection of attribute models object</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        public IAttributeModelCollection GetAttributeModels(Collection<Int32> attributeIds, Collection<Int32> attributeGroupIds, Int32 customViewId, Int32 stateViewId, Collection<Int32> excludeAttributeIds, IAttributeModelContext iAttributeModelContext)
        {
            AttributeModelContext attributeModelContext = (AttributeModelContext)iAttributeModelContext;
            return MakeServiceCall("GetAttributeModels",
                                   "GetAttributeModels",
                                   service =>
                                   {
                                       Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
                                       if (isTracingEnabled)
                                       {
                                           MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Requested Attribute Models for CustomViewId: {0}, StateViewId: {1}", customViewId, stateViewId), MDMTraceSource.AttributeModelGet);
                                       }
                                       return service.GetAttributeModels(attributeIds,
                                                                         attributeGroupIds,
                                                                         customViewId,
                                                                         stateViewId,
                                                                         excludeAttributeIds,
                                                                         attributeModelContext);
                                   });
        }

        /// <summary>
        /// Gets the attribute models for the requested attribute unique identifier (attribute name and group name) in a locale provided by user
        /// </summary>
        /// <param name="iAttributeUniqueIdentifier">Indicates unique identifier for attribute</param>
        /// <param name="locale">Indicates the locale of attribute model</param>
        /// <param name="iCallerContext">Indicates name of application and module which invoked the API</param>
        /// <returns>Returns attribute model based on attribute unique identifier in a locale provided by the user</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        /// <example>
        /// <code language="c#" title="Get AttributeModel By AttributeName And Attribute ParentName for Locale" source="..\MDM.APISamples\EntityManager\AttributeModel\GetAttributeModel.cs" region="GetAttributeModelByAttributeNameAndParentNameforLocale" />
        /// </example>
        public IAttributeModel GetAttributeModelByUniqueIdentifier(IAttributeUniqueIdentifier iAttributeUniqueIdentifier, LocaleEnum locale, ICallerContext iCallerContext)
        {
            return MakeServiceCall("GetAttributeModelByUniqueIdentifier", "GetAttributeModelByUniqueIdentifierWithLocale",
                client =>
                client.GetAttributeModelByUniqueIdentifier(iAttributeUniqueIdentifier as AttributeUniqueIdentifier, locale, FillDiagnosticTraces(iCallerContext)));
        }

        /// <summary>
        /// Gets the attribute models for the requested attribute unique identifier (attribute name and group name)
        /// </summary>
        /// <param name="iAttributeUniqueIdentifier">Indicates unique identifier for attribute</param>
        /// <param name="iCallerContext">Indicates name of application and module which invoked the API</param>
        /// <returns>Returns attribute model interface based on attribute unique identifier</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        /// <example>
        /// <code language="c#" title="Get AttributeModel By AttributeName And Attribute ParentName" source="..\MDM.APISamples\EntityManager\AttributeModel\GetAttributeModel.cs" region="GetAttributeModelByAttributeNameAndParentName" />
        /// </example>
        public IAttributeModel GetAttributeModelByUniqueIdentifier(IAttributeUniqueIdentifier iAttributeUniqueIdentifier, ICallerContext iCallerContext)
        {
            return MakeServiceCall("GetAttributeModelByUniqueIdentifier", "GetAttributeModelByUniqueIdentifier",
                client =>
                client.GetAttributeModelByUniqueIdentifier(iAttributeUniqueIdentifier as AttributeUniqueIdentifier, FillDiagnosticTraces(iCallerContext)));
        }

        /// <summary>
        /// Gets the attribute models for the requested attribute unique identifiers (attribute name and group name) in a locale provided by user
        /// </summary>
        /// <param name="iAttributeUniqueIdentifiers">Indicates unique identifier for attribute collection</param>
        /// <param name="locale">Indicates locale of attribute model</param>
        /// <param name="iCallerContext">Indicates name of application and module which invoked the API</param>
        /// <returns>Returns collection of attribute model based on attribute unique identifiers in a locale provided by user</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        /// <example>
        /// <code language="c#" title="Get AttributeModels By AttributeNames And Attribute ParentNames for Locale" source="..\MDM.APISamples\EntityManager\AttributeModel\GetAttributeModel.cs" region="Get AttributeModels By AttributeNames And Attribute ParentNames for Locale" />
        /// </example>
        public IAttributeModelCollection GetAttributeModelsByUniqueIdentifiers(IAttributeUniqueIdentifierCollection iAttributeUniqueIdentifiers, LocaleEnum locale, ICallerContext iCallerContext)
        {
            return MakeServiceCall("GetAttributeModelsByUniqueIdentifiers", "GetAttributeModelsByUniqueIdentifiersWithLocale",
                client =>
                client.GetAttributeModelsByUniqueIdentifiers(iAttributeUniqueIdentifiers as AttributeUniqueIdentifierCollection, locale, FillDiagnosticTraces(iCallerContext)));
        }

        /// <summary>
        /// Gets the attribute models for the requested attribute unique identifiers (attribute name and group name)
        /// </summary>
        /// <param name="iAttributeUniqueIdentifiers">Indicates unique identifier for attribute collection</param>
        /// <param name="iCallerContext">Indicates name of application and module which invoked the API</param>
        /// <returns>Returns attribute model based on attribute unique identifiers</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        /// <example>
        /// <code language="c#" title="Get AttributeModels By AttributeNames And Attribute ParentNames" source="..\MDM.APISamples\EntityManager\AttributeModel\GetAttributeModel.cs" region="Get AttributeModels By AttributeNames And Attribute ParentNames" />
        /// </example>
        public IAttributeModelCollection GetAttributeModelsByUniqueIdentifiers(IAttributeUniqueIdentifierCollection iAttributeUniqueIdentifiers, ICallerContext iCallerContext)
        {
            return MakeServiceCall("GetAttributeModelsByUniqueIdentifiers", "GetAttributeModelsByUniqueIdentifiers",
                client =>
                client.GetAttributeModelsByUniqueIdentifiers(iAttributeUniqueIdentifiers as AttributeUniqueIdentifierCollection, FillDiagnosticTraces(iCallerContext)));
        }

        /// <summary>
        /// Processes Attribute Models
        /// </summary>
        /// <param name="iAttributeModelCollection">Indicates Attribute Model  Collection that needs to be processed</param>
        /// <param name="programName">Indicates the name of the Program</param>
        /// <param name="iCallerContext">Indicates the name of application and module that are performing the action</param>
        /// <returns>Returns collection of Attribute Operation Result</returns>
        public IAttributeOperationResultCollection ProcessAttributeModels(IAttributeModelCollection iAttributeModelCollection, String programName, ICallerContext iCallerContext)
        {
            AttributeModelCollection attributeModelCollection = (AttributeModelCollection)iAttributeModelCollection;
            return MakeServiceCall("ProcessAttributeModels",
                                   "ProcessAttributeModels",
                                   service => service.ProcessAttributeModels(attributeModelCollection,
                                                                             programName,
                                                                             FillDiagnosticTraces(iCallerContext)));
        }

        /// <summary>
        /// Get Attribute Models By attribute unique Identifier for requested locales
        /// </summary>
        /// <param name="attributeUniqueIdentifier">Specifies the attribute unique identifier object</param>
        /// <param name="locales">Specifies List of Locales</param>
        /// <param name="iCallerContext">Specifies the Context of the Caller</param>
        /// <returns>Attribute Models</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        /// <example>
        /// <code language="c#" title="Get AttributeModels in Given Locales" source="..\MDM.APISamples\EntityManager\AttributeModel\GetAttributeModel.cs" region="GetAttributeModelsinGivenLocalesUsingDataModelService" />
        /// </example>
        public IAttributeModelCollection GetAttributeModelsByUniqueIdentifier(IAttributeUniqueIdentifier attributeUniqueIdentifier, Collection<LocaleEnum> locales, ICallerContext iCallerContext)
        {
            return MakeServiceCall<IAttributeModelCollection>("GetAttributeModelsByUniqueIdentifier", "GetAttributeModelsByUniqueIdentifier", client =>
                    client.GetAttributeModelsByUniqueIdentifier(attributeUniqueIdentifier as AttributeUniqueIdentifier, locales, FillDiagnosticTraces(iCallerContext)));
        }

        /// <summary>
        /// Gets All Stateview attributes
        /// </summary>
        /// <example>
        /// <code>     
        /// // This sample demonstrates how to get all stateview attribute models
        /// DataModelService dataModelService = new DataModelService(); 
        /// ICallerContext callerContext = MDMObjectFactory.GetICallerContext(MDMCenterApplication.MDMCenter, MDMCenterModules.DQM);
        /// 
        /// // Indicates a collection of locales
        /// Collection<![CDATA[<LocaleEnum>]]> localeEnumCollection = new Collection<![CDATA[<LocaleEnum>]]>();
        /// localeEnumCollection.Add(LocaleEnum.en_WW);
        /// localeEnumCollection.Add(LocaleEnum.de_DE);
        /// 
        /// // Calls service that returns a collection of stateview attributes
        /// IAttributeModelCollection stateviewAttributeModels = dataModelService.GetAllStateviewAttributeModels(localeEnumCollection, callerContext);
        /// </code>
        /// </example>
        /// <param name="locales">Indicates a List of Locales</param>
        /// <param name="iCallerContext">Indicates Application and Module name by which action is being performed</param>
        /// <returns>Returns a collection of Stateview Attribute Models</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDMCenter operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        public IAttributeModelCollection GetAllStateviewAttributeModels(Collection<LocaleEnum> locales, ICallerContext iCallerContext)
        {
            return MakeServiceCall<AttributeModelCollection>("GetAllStateviewAttributeModels", "GetAllStateviewAttributeModels",
                                                  client =>
                                                      client.GetAllStateviewAttributeModels(locales, FillDiagnosticTraces(iCallerContext)));
        }


        #endregion

        #region Entity Hierarchy Definition Methods

        /// <summary>
        /// Processes the hierarchy definition based on the EntityVariantDefinitionCollection details
        /// </summary>
        /// <param name="iEntityVariantDefinitions">Indicates Interface for Entity Variant Definition</param>
        /// <param name="iCallerContext">Indicates the name of the application and the module which invoked the API</param>
        /// <returns>Returns operationresult collection.</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        public IOperationResultCollection ProcessVariantDefinitions(IEntityVariantDefinitionCollection iEntityVariantDefinitions, ICallerContext iCallerContext)
        {
            EntityVariantDefinitionCollection entityHierarchyDefinitions = (EntityVariantDefinitionCollection)iEntityVariantDefinitions;

            return MakeServiceCall<IOperationResultCollection>("ProcessVariantDefinitions",
                            "ProcessVariantDefinitions",
                            service => service.ProcessVariantDefinitions(entityHierarchyDefinitions, (CallerContext)iCallerContext));
        }

        /// <summary>
        /// Gets Hierarchy Definition based on given Container ID, Category ID and Entity Type ID
        /// </summary>
        /// <example>
        /// <code>
        /// // Get new instance of IEntityContext using MDMObjectFactory
        /// IEntityHierarchyDefinition iEntityHierarchyDefinition = MDMObjectFactory.GetIEntityHierarchyDefinition();
        ///
        /// // Assumption: Hard coded variables are considered in this sample
        /// Int64 categoryId = 1; //Apparel as per RiverWorks Model
        /// Int32 containerId = 5; //Product Master as per RiverWorks Model
        /// Int32 entityTypeId = 16; //Style as per RiverWorks Model
        ///
        /// // Get MDM dataModel service
        /// DataModelService mdmDataModelService = GetMDMDataModelService();
        ///
        /// // Make a WCF call to DataModelService.GetHierarchyDefinitionByContext
        /// iEntityHierarchyDefinition = mdmDataModelService.GetHierarchyDefinitionByContext(containerId, categoryId, entityTypeId);
        ///
        /// </code>
        /// </example>
        /// <param name="containerId">Indicates Container ID</param>
        /// <param name="categoryId">Indicates Category ID</param>
        /// <param name="entityTypeId">Indicates Entity Type ID</param>
        /// <param name="iCallerContext">Indicates the name of the application and the module which invoked the API</param>
        /// <returns>Returns Entity Hierarchy Definition Interface Object</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        public IEntityVariantDefinition GetVariantDefinitionByContext(Int32 containerId, Int64 categoryId, Int32 entityTypeId, ICallerContext iCallerContext)
        {
            return MakeServiceCall("GetHierarchyDefinitionByContext",
                                   "GetHierarchyDefinitionByContext",
                                   service =>
                                   {
                                       Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
                                       if (isTracingEnabled)
                                       {
                                           MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Requested Hierarchy Definition By Context for containerId: " + containerId + ",categoryId: " + categoryId + ",entityTypeId: " + entityTypeId, MDMTraceSource.EntityHierarchyGet);
                                       }
                                       return service.GetVariantDefinitionByContext(containerId,
                                                                                      categoryId,
                                                                                      entityTypeId,
                                                                                      (CallerContext)iCallerContext);
                                   });
        }

        ///  <summary>
        ///  Gets hierarchy definition based on given entity variant definition identifier
        ///  </summary>
        ///  <example>
        ///  <code language="c#" title="Get Variant Definition By Id"  source="..\..\Documentation\MDM.APISamples\EntityManager\EntityHierarchy\EntityVariantDefinition.cs" region="GetVariantDefinitionById"/>
        ///  </example>
        ///  <param name="entityVariantDefinitionId">Indicates entity variant definition identifier</param>
        ///  <param name="iCallerContext">Indicates the name of the application and the module which invoked the API</param>
        ///  <returns>Returns hierarchy definition based on given entity variant definition identifier</returns>
        ///  <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        ///  <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        ///  <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        ///  <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        ///  <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        public IEntityVariantDefinition GetVariantDefinitionById(Int32 entityVariantDefinitionId, ICallerContext iCallerContext)
        {
            return MakeServiceCall<IEntityVariantDefinition>("GetVariantDefinitionById",
                                   "GetVariantDefinitionById",
                                   client => client.GetVariantDefinitionById(entityVariantDefinitionId, FillDiagnosticTraces(iCallerContext)));
        }

        /// <summary>
        /// Gets All Entity Variant Definitions
        /// </summary>
        /// <param name="iCallerContext">Indicates the name of the application and the module which invoked the API</param>
        /// <returns>Returns collection of Entity Variant Definition Interface Object</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        public IEntityVariantDefinitionCollection GetAllVariantDefinitions(ICallerContext iCallerContext)
        {
            EntityVariantDefinitionCollection entityHierarchyDefinitions =
                MakeServiceCall("GetAllHierarchyDefinitions",
                                "GetAllHierarchyDefinitions",
                                service => service.GetAllVariantDefinitions((CallerContext)iCallerContext));

            EntityVariantDefinitionCollection returnValue = new EntityVariantDefinitionCollection();
            if (entityHierarchyDefinitions != null)
            {
                foreach (EntityVariantDefinition definition in entityHierarchyDefinitions)
                {
                    returnValue.Add(definition);
                }
            }

            return returnValue;
        }

        #endregion

        #region Dynamic Table Schema Methods

        /// <summary>
        /// Process table
        /// </summary>
        /// <param name="iDbTable">This parameter is specifying instance of table to be processed</param>
        /// <param name="dynamicTableType">This parameter is specifying dynamic table type</param>
        /// <param name="iCallerContext">Name of application and Module which are performing action</param>
        /// <returns>Results of the operation having errors and information if any</returns>
        public IOperationResult Process(IDBTable iDbTable, DynamicTableType dynamicTableType, ICallerContext iCallerContext)
        {
            return MakeServiceCall("Process",
                                   "DynamicTableSchemaProcess",
                                   service => service.DynamicTableSchemaProcess((DBTable)iDbTable,
                                                                                dynamicTableType,
                                                                                FillDiagnosticTraces(iCallerContext)));
        }

        /// <summary>
        /// Process Multiple tables
        /// </summary>
        /// <param name="iDbTables">This parameter is specifying instance of tables to be processed</param>
        /// <param name="dynamicTableType">This parameter is specifying dynamic table type</param>
        /// <param name="iCallerContext">Name of application and Module which are performing action</param>
        /// <returns>Results of the operation having errors and information if any</returns>
        public IOperationResult Process(IDBTableCollection iDbTables, DynamicTableType dynamicTableType, ICallerContext iCallerContext)
        {
            return MakeServiceCall("Process",
                                   "DynamicTableSchemaProcesses",
                                   service => service.DynamicTableSchemaProcesses((DBTableCollection)iDbTables,
                                                                                  dynamicTableType,
                                                                                  FillDiagnosticTraces(iCallerContext)));
        }

        #endregion

        #region Container Methods

        /// <summary>
        /// Creates Container
        /// </summary>
        /// <param name="iContainer">Indicates a Container Object to be created </param>
        /// <param name="programName">Indicates name of the program</param>
        /// <param name="iCallerContext">Indicates the name of application and module that are performing the action</param>
        /// <returns>Returns the state of Container creation </returns>
        public IOperationResult CreateContainer(IContainer iContainer, String programName, ICallerContext iCallerContext)
        {
            return MakeServiceCall("CreateContainer",
                                   "CreateContainer",
                                   service => service.CreateContainer((Container)iContainer,
                                                                      programName,
                                                                      FillDiagnosticTraces(iCallerContext)));
        }

        /// <summary>
        /// Updates Container
        /// </summary>
        /// <param name="iContainer">Indicates a Container Object to be updated</param>
        /// <param name="programName">Indicates name of the program</param>
        /// <param name="iCallerContext">Indicates the name of application and module that are performing the action</param>
        /// <returns>Returns the state of Container updation</returns>
        public IOperationResult UpdateContainer(IContainer iContainer, String programName, ICallerContext iCallerContext)
        {
            return MakeServiceCall("UpdateContainer",
                                   "UpdateContainer",
                                   service => service.UpdateContainer((Container)iContainer,
                                                                      programName,
                                                                      FillDiagnosticTraces(iCallerContext)));
        }

        /// <summary>
        /// Deletes Container
        /// </summary>
        /// <param name="iContainer">Indicates a Container Object to be deleted</param>
        /// <param name="programName">Indicates name of the program</param>
        /// <param name="iCallerContext">Indicates the name of application and module that are performing the action</param>
        /// <returns> Returns the state of Container deletion</returns>
        public IOperationResult DeleteContainer(IContainer iContainer, String programName, ICallerContext iCallerContext)
        {
            return MakeServiceCall("DeleteContainer",
                                   "DeleteContainer",
                                   service => service.DeleteContainer((Container)iContainer,
                                                                      programName,
                                                                      FillDiagnosticTraces(iCallerContext)));
        }

        /// <summary>
        /// Processes Container
        /// </summary>
        /// <param name="iContainerCollection">Indicates a ContainerCollection Object to be processed</param>
        /// <param name="iCallerContext">Indicates the name of application and module that are performing the action</param>
        /// <returns> Returns the state of Container processing</returns>
        public IOperationResult ProcessContainer(IContainerCollection iContainerCollection, ICallerContext iCallerContext)
        {
            return this.MakeServiceCall<IOperationResult>("ProcessContainer", "ProcessContainer",
                client => client.ProcessContainer(iContainerCollection as ContainerCollection, FillDiagnosticTraces(iCallerContext)));
        }

        /// <summary>
        /// Gets container collection based on organization
        /// </summary>
        /// <param name="orgId">Indicates the Organisation id</param>
        /// <returns>Collection of containers based on organization</returns>
        public IContainerCollection GetContainerCollectionByOrganization(Int32 orgId)
        {
            return MakeServiceCall("GetContainerCollectionByOrganization",
                                   "GetContainerCollectionByOrganization",
                                   service => service.GetContainerCollectionByOrganization(orgId));
        }
        #endregion Container CUD

        #region Container Get

        /// <summary>
        /// Gets all Container
        /// </summary>
        /// <param name="iCallerContext">Indicates the name of the application and the module that are performing the action</param>
        /// <returns>Returns a collection of Containers</returns>
        public IContainerCollection GetAllContainers(ICallerContext iCallerContext)
        {
            return this.MakeServiceCall<IContainerCollection>("GetAllContainers", "GetAllContainersWithoutContext",
                 client => client.GetAllContainers(FillDiagnosticTraces(iCallerContext)));
        }

        /// <summary>
        /// Gets all Containers
        /// </summary>
        /// <param name="iContainerContext">Indicates the context containing flag to load attributes or not</param>                
        /// <param name="iCallerContext">Indicates the name of the application and the module that are performing the action</param>
        /// <returns>Returns a collection of Containers</returns>
        public IContainerCollection GetAllContainers(IContainerContext iContainerContext, ICallerContext iCallerContext)
        {

            return this.MakeServiceCall<IContainerCollection>("GetAllContainers", "GetAllContainers",
                client => client.GetAllContainers(iContainerContext as ContainerContext, FillDiagnosticTraces(iCallerContext)));
        }

        /// <summary>
        /// Gets container based on given container id
        /// </summary>
        /// <param name="containerId">Indicates Id of Container</param>
        /// <param name="iCallerContext">Name of application and Module which are performing action</param>
        /// <returns>Returns requested Container</returns>
        public IContainer GetContainerById(Int32 containerId, ICallerContext iCallerContext)
        {
            return this.MakeServiceCall<IContainer>("GetContainerById", "GetContainerById",
                client => client.GetContainerById(containerId, FillDiagnosticTraces(iCallerContext)));
        }

        /// <summary>
        /// Gets container based on given container short name
        /// </summary>
        /// <example>
        /// <code language="c#" title="Get Container By Name"  source="..\..\Documentation\MDM.APISamples\DataModelManager\Container\GetContainerByName.cs" region="Container By Name"/>
        /// </example>
        /// <param name="containerShortName">Indicates Short Name of Container</param>        
        /// <param name="iCallerContext">Name of application and Module which are performing action</param>
        /// <returns>Returns requested Container</returns>
        public IContainer GetContainerByName(String containerShortName, ICallerContext iCallerContext)
        {
            return this.MakeServiceCall<IContainer>("GetContainerByName", "GetContainerByName",
                client => client.GetContainerByName(containerShortName, FillDiagnosticTraces(iCallerContext)));
        }

        /// <summary>
        /// Gets container based on given container short name and organization short name
        /// </summary>
        /// <example>
        /// <code language="c#" title="Get Container by container name and Organization name"  source="..\..\Documentation\MDM.APISamples\DataModelManager\Container\GetContainerByName.cs" region="Get Container by ContainerName and OrganizationName"/>
        /// </example>
        /// <param name="containerName">Indicates Container ShortName</param>
        /// <param name="organizationName">Indicates Organization ShortName</param>
        /// <param name="iCallerContext">Name of application and Module which are performing action</param>
        /// <returns>Gives Container by given ShortName and Organization ShortName</returns>
        public IContainer GetContainerByContainerNameAndOrgName(String containerName, String organizationName, ICallerContext iCallerContext)
        {
            return this.MakeServiceCall<IContainer>("GetContainerByContainerNameAndOrgName", "GetContainerByContainerNameAndOrgName",
                client => client.GetContainerByContainerNameAndOrgName(containerName, organizationName, FillDiagnosticTraces(iCallerContext)));
        }

        /// <summary>
        /// Gets container based on given container id and container context
        /// </summary>
        /// <param name="containerId">Indicates Id of Container</param>
        /// <param name="iContainerContext">Indicates context containing flag to load attributes or not</param>        
        /// <param name="iCallerContext">Name of application and Module which are performing action</param>
        /// <returns>Returns requested Container</returns>
        public IContainer GetContainerById(Int32 containerId, IContainerContext iContainerContext, ICallerContext iCallerContext)
        {

            return this.MakeServiceCall<IContainer>("GetContainerById", "GetContainerByIdBasedOnContext",
                client => client.GetContainerById(containerId, iContainerContext as ContainerContext, FillDiagnosticTraces(iCallerContext)));
        }

        /// <summary>
        /// Gets container based on given container short name and container context
        /// </summary>
        /// <example>
        /// <code language="c#" title="Get container By container context and name"  source="..\..\Documentation\MDM.APISamples\DataModelManager\Container\GetContainerByName.cs" region="Container By Name"/>
        /// </example>
        /// <param name="containerShortName">Indicates Short Name of Container</param>        
        /// <param name="iContainerContext">Indicates context containing flag to load attributes or not</param>                
        /// <param name="iCallerContext">Name of application and Module which are performing action</param>
        /// <returns>Returns requested Container</returns>
        public IContainer GetContainerByName(String containerShortName, IContainerContext iContainerContext, ICallerContext iCallerContext)
        {

            return this.MakeServiceCall<IContainer>("GetContainerByName", "GetContainerByNameBasedOnContext",
                client => client.GetContainerByName(containerShortName, iContainerContext as ContainerContext, FillDiagnosticTraces(iCallerContext)));
        }

        /// <summary>
        /// Gets container based on given container short name, organization short name and container context
        /// </summary>
        /// <example>
        /// <code language="c#" title="Get Container by Container context, Container Name and Organization name"  source="..\..\Documentation\MDM.APISamples\DataModelManager\Container\GetContainerByName.cs" region="Get Container with System level attributes"/>
        /// </example>
        /// <param name="containerName">Indicates Container ShortName</param>
        /// <param name="organizationName">Indicates Organization ShortName</param>
        /// <param name="iContainerContext">Indicates context containing flag to load attributes or not</param>                
        /// <param name="iCallerContext">Name of application and Module which are performing action</param>
        /// <returns>Gives Container by given ShortName and Organization ShortName</returns>
        public IContainer GetContainerByContainerNameAndOrgName(String containerName, String organizationName, IContainerContext iContainerContext, ICallerContext iCallerContext)
        {
            return this.MakeServiceCall<IContainer>("GetContainerByContainerNameAndOrgName", "GetContainerByContainerNameAndOrgNameBasedOnContext",
                client => client.GetContainerByContainerNameAndOrgName(containerName, organizationName, iContainerContext as ContainerContext, FillDiagnosticTraces(iCallerContext)));
        }

        /// <summary>
        /// Get hierarchy of child container based on given container identifier and container context
        /// </summary>
        /// <example>
        /// <code language="c#" title="Get Child Containers By Parent Container Id"  source="..\..\Documentation\MDM.APISamples\DataModelManager\Container\GetContainerHierarchy.cs" region="GetChildContainersByParentContainerId"/>
        /// </example>
        /// <param name="containerId">Indicates container identifier</param>
        /// <param name="iContainerContext">Indicates context of the container specifying properties like load attributes</param>
        /// <param name="iCallerContext">Indicates caller of the API specifying application and module name.</param>
        /// <param name="loadRecursive">Indicates whether load only immediate child containers or complete hierarchy of child containers.</param>
        /// <returns>Returns collection of child container based on given container identifier and container context</returns>
        public IContainerCollection GetChildContainersByParentContainerId(Int32 containerId, IContainerContext iContainerContext, ICallerContext iCallerContext, Boolean loadRecursive)
        {
            return MakeServiceCall<IContainerCollection>("GetAllChildContainersByParentContainerId", "GetAllChildContainersByParentContainerId",
                client => client.GetChildContainersByParentContainerId(containerId, iContainerContext as ContainerContext, iCallerContext as CallerContext, loadRecursive));
        }

        /// <summary>
        /// Get hierarchy of child container with requested container identifier's container itself based on given container identifier and container context
        /// </summary>
        /// <example>
        /// <code language="c#" title="Get Container Hierarchy By Container Id"  source="..\..\Documentation\MDM.APISamples\DataModelManager\Container\GetContainerHierarchy.cs" region="GetContainerHierarchyByContainerId"/>
        /// </example>
        /// <param name="containerId">Indicates container identifier</param>
        /// <param name="iContainerContext">Indicates context of the container specifying properties like load attributes</param>
        /// <param name="iCallerContext">Indicates caller of the API specifying application and module name.</param>
        /// <returns>Returns collection of child containers with requested container identifier's container itself based on given container identifier and container context</returns>
        public IContainerCollection GetContainerHierarchyByContainerId(Int32 containerId, IContainerContext iContainerContext, ICallerContext iCallerContext)
        {
            return MakeServiceCall<IContainerCollection>("GetContainerHierarchyByContainerId", "GetContainerHierarchyByContainerId",
                client => client.GetContainerHierarchyByContainerId(containerId, iContainerContext as ContainerContext, iCallerContext as CallerContext));
        }

        #endregion Container Get

        #region Lookup Methods

        /// <summary>
        /// Loads all the lookup data into cache
        /// </summary>
        /// <param name="iCallerContext">Indicates the name of the Application and Module which invoked the API</param>
        /// <returns>Boolean flag that indicates whether the loading is successfull or not</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        /// <example>
        /// <code language="c#" title="Load Lookup Data" source="..\MDM.APISamples\LookupManager\GetLookupdata.cs" region="Load Lookup Data" />
        /// </example>
        public Boolean LoadLookupData(ICallerContext iCallerContext)
        {
            return MakeServiceCall("LoadLookupData",
                                   "LoadLookupData",
                                   service => service.LoadLookupData(FillDiagnosticTraces(iCallerContext)));
        }

        /// <summary>
        /// Gets the lookup model of the requested lookup table
        /// </summary>
        /// <param name="lookupTableName">Indicates the name of the lookup table</param>
        /// <param name="iCallerContext">Indicates the name of the Application and Module which invoked the API</param>
        /// <returns>Returns the lookup model for the requested lookup table</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        /// <example>
        /// <code language="c#" title="Get Lookup Model" source="..\MDM.APISamples\LookupManager\GetLookupdata.cs" region="Get Lookup Model" />
        /// </example>
        public ILookup GetLookupModel(String lookupTableName, ICallerContext iCallerContext)
        {
            return MakeServiceCall("GetLookupModel", "GetLookupModel",
                                   service =>
                                   {
                                       Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
                                       if (isTracingEnabled)
                                       {
                                           MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Requested Lookup Model for LookupTableName: {0}.", lookupTableName), MDMTraceSource.LookupGet);
                                       }
                                       return service.GetLookupModel(lookupTableName, FillDiagnosticTraces(iCallerContext));
                                   });
        }

        /// <summary>
        /// Gets Lookup data for the requested Lookup table in a locale provided by the user
        /// </summary>
        /// <param name="lookupTableName">Indicates the name of the Lookup table</param>
        /// <param name="locale">Indicates the Locale for which data needs to be fetched</param>
        /// <param name="maxRecordsToReturn">Indicates the maximum number of Lookup records to return. Setting '-1' returns all record</param>
        /// <param name="getLatest">Indicates the Boolean flag which specifies whether to get from Database or Cache. A value True always gets from DB</param>
        /// <param name="iCallerContext">Indicates the name of application and module which invoked this API</param>
        /// <returns>Returns lookup data for the specified lookup table name in a locale requested by the user</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        /// <example>
        /// <code language="c#" title="Get Lookup Data" source="..\MDM.APISamples\LookupManager\GetLookupdata.cs" region="Get Lookup Data" />
        /// <code language="c#" title="Get Lookup Data MaxRecordsOption" source="..\MDM.APISamples\LookupManager\GetLookupdata.cs" region="Get Lookup Data MaxRecordsOption" />
        /// </example>
        public ILookup GetLookupData(String lookupTableName, LocaleEnum locale, Int32 maxRecordsToReturn, Boolean getLatest, ICallerContext iCallerContext)
        {
            return MakeServiceCall("GetLookupData", "GetLookupData",
                                   service =>
                                   {
                                       Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
                                       if (isTracingEnabled)
                                       {
                                           MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Requested Lookup Data for lookupTableName: {0}, Locale: {1}, getLatest: {2}", lookupTableName, locale, getLatest), MDMTraceSource.LookupGet);
                                       }
                                       return service.GetLookupData(lookupTableName, locale, maxRecordsToReturn, getLatest, FillDiagnosticTraces(iCallerContext));
                                   });
        }

        /// <summary>
        /// Gets the collection of lookup row for the specified lookup table and lookup search rule collection in a locale specified by user
        /// </summary>
        /// <param name="lookupTableName">Indicates the name of the lookup table</param>
        /// <param name="locale">Indicates the locale for which data needs to be fetched</param>
        /// <param name="searchRuleCollection">Indicates the search rules for filtering the lookup row collection</param>
        /// <param name="maxRecordsToReturn">Indicates the max number of lookup records to return. Setting '-1' returns all record</param>
        /// <param name="callerContext">Indicates the application name and module name by which action is being performed</param>
        /// <returns>Returns collection of lookup rows</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        /// <example>
        /// <code language="c#" title="Get lookup rows" source="..\MDM.APISamples\LookupManager\GetLookupdata.cs" region="GetLookupRows" />
        /// </example>
        public IRowCollection GetLookupRows(String lookupTableName, LocaleEnum locale, ILookupSearchRuleCollection searchRuleCollection, Int32 maxRecordsToReturn,
            ICallerContext callerContext)
        {
            return MakeServiceCall<RowCollection>("GetLookupRows", "GetLookupRows", client => client.GetLookupRows(lookupTableName, locale,
                (LookupSearchRuleCollection)searchRuleCollection, maxRecordsToReturn, FillDiagnosticTraces(callerContext)));
        }

        /// <summary>
        /// Gets the lookup data for the specified attribute Id in a locale provided by user
        /// </summary>
        /// <param name="attributeId">Indicates the id of the attribute for which data needs to be get</param>
        /// <param name="locale">Indicates the locale for which data needs to be fetched</param>
        /// <param name="maxRecordsToReturn">Indicates the maximum number of lookup records to return. Setting '-1' returns all record</param>
        /// <param name="iCallerContext">Indicates the name of the application and the module which has invoked the API</param>
        /// <returns>Returns lookup data for the specified attribute id in a locale provided by user</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        /// <example>
        /// <code language="c#" title="Get lookup by attribute id and locale" source="..\MDM.APISamples\LookupManager\GetLookupdata.cs" region="Get lookup by attribute id and locale" />
        /// </example>
        public ILookup GetAttributeLookupData(Int32 attributeId, LocaleEnum locale, Int32 maxRecordsToReturn, ICallerContext iCallerContext)
        {
            return MakeServiceCall("GetAttributeLookupData", "GetAttributeLookupData",
                                   service =>
                                   {
                                       Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
                                       if (isTracingEnabled)
                                       {
                                           MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Requested Attribute Lookup Data for attributeId: {0}, locale: {1}, maxRecordsToReturn: {2}", attributeId, locale, maxRecordsToReturn), MDMTraceSource.LookupGet);
                                       }
                                       return service.GetAttributeLookupData(attributeId, locale, maxRecordsToReturn, FillDiagnosticTraces(iCallerContext));
                                   });
        }

        /// <summary>
        /// Gets the lookup data for the specified attribute name, attribute parent name, and application context as optional parameter in a locale specified by user
        /// </summary>
        /// <param name="attributeName">Indicates the short name of the attribute for which lookup data needs to be fetched</param>
        /// <param name="attributeParentName">Indicates the parent name of the attribute for which lookup data needs to be fetched</param>
        /// <param name="locale">Indicates the locale for which data needs to be fetched</param>
        /// <param name="maxRecordsToReturn">Indicates the maximum number of lookup records to return. Setting '-1' returns all record</param>
        /// <param name="callerContext">Indicates the name of the application and the module which has invoked the API</param>
        /// <param name="applicationContext">Indicates the current context of the application</param>
        /// <returns>Returns lookup data for the specified attribute name, attribute parent name, and application context in a locale specified by user</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        /// <example>
        /// <code language="c#" title="Get lookup data by attribute name and attribute parent name" source="..\MDM.APISamples\LookupManager\GetLookupdata.cs" region="GetLookupByAttributeNameAndParentName" />
        /// </example>
        public ILookup GetAttributeLookupData(String attributeName, String attributeParentName, LocaleEnum locale, Int32 maxRecordsToReturn,
            ICallerContext callerContext, IApplicationContext applicationContext = null)
        {
            if (applicationContext == null)
            {
                applicationContext = new ApplicationContext();
            }

            return MakeServiceCall<Lookup>("GetAttributeLookupData", "GetAttributeLookupDataFilterBasedOnDependencyUsingName",
                client => client.GetAttributeLookupData(attributeName, attributeParentName, locale, maxRecordsToReturn, null, (ApplicationContext)applicationContext,
                    FillDiagnosticTraces(callerContext), false, null));
        }

        /// <summary>
        /// Gets the lookup data for the specified attribute id and application context in a locale provided by user
        /// </summary>
        /// <param name="attributeId">Indicates the id of the attribute for which lookup data needs to be fetched</param>
        /// <param name="locale">Indicates the locale for which data needs to be fetched</param>
        /// <param name="maxRecordsToReturn">Indicates the maximum number of lookup records to return. Setting '-1' returns all record</param>
        /// <param name="iApplicationContext">Indicates the current context of the application</param>
        /// <param name="iCallerContext">Indicates the name of the application and the module which has invoked the API</param>
        /// <returns>Returns lookup data for the specified attribute id and application context in a locale provided by user</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        /// <example>
        /// <code language="c#" title="Get lookup by attribute id and application context for locale" source="..\MDM.APISamples\LookupManager\GetLookupdata.cs" region="Get lookup by attribute id and application context for locale" />
        /// <code language="c#" title="Get lookup by attribute id and application context for german locale" source="..\MDM.APISamples\LookupManager\GetLookupdata.cs" region="Get lookup by attribute id and application context for german locale" />
        /// </example>
        public ILookup GetAttributeLookupData(Int32 attributeId, LocaleEnum locale, Int32 maxRecordsToReturn, IApplicationContext iApplicationContext, ICallerContext iCallerContext)
        {
            return MakeServiceCall("GetAttributeLookupData",
                                   "GetAttributeLookupData",
                                   service =>
                                   {
                                       Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
                                       if (isTracingEnabled)
                                       {
                                           MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Requested Attribute Lookup Data for AttributeId: {0}, Locale: {1}, MaxRecordsToReturn: {2}", attributeId, locale, maxRecordsToReturn), MDMTraceSource.LookupGet);
                                       }
                                       return service.GetAttributeLookupData(
                                           attributeId,
                                           locale,
                                           maxRecordsToReturn,
                                           (ApplicationContext)iApplicationContext,
                                           FillDiagnosticTraces(iCallerContext));
                                   });
        }

        /// <summary>
        /// Gets the lookup data for the specified attribute id and lookup value Ids in a locale provided by user
        /// </summary>
        /// <param name="attributeId">Indicates the id of the attribute for which lookup data needs to be fetched</param>
        /// <param name="locale">Indicates the locale for which data needs to be fetched</param>
        /// <param name="lookupValueIdList">Indicates the list of lookup ids to be returned along with requested maximum number of records</param>
        /// <param name="iCallerContext">Indicates the name of the application and the module which has invoked the API</param>
        /// <returns>Returns lookup data for the specified attribute id and lookup values id in a locale provided by user</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        /// <example>
        /// <code language="c#" title="Get the lookup by attribute id and lookup value ids for locale" source="..\MDM.APISamples\LookupManager\GetLookupdata.cs" region="Get the lookup by attribute id and lookup values id for locale" />
        /// </example>
        public ILookup GetAttributeLookupData(Int32 attributeId, LocaleEnum locale, Collection<Int32> lookupValueIdList, ICallerContext iCallerContext)
        {
            return MakeServiceCall("GetAttributeLookupData",
                                   "GetAttributeLookupData",
                                   service =>
                                   {
                                       Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
                                       if (isTracingEnabled)
                                       {
                                           MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Requested Attribute Lookup Data for AttributeId: {0}, Locale: {1}, LookupValueIdList: {2}", attributeId, locale, ValueTypeHelper.JoinCollection(lookupValueIdList, ",")), MDMTraceSource.LookupGet);
                                       }
                                       return service.GetAttributeLookupData(
                                           attributeId,
                                           locale,
                                           lookupValueIdList,
                                           FillDiagnosticTraces(iCallerContext));
                                   });
        }

        /// <summary>
        /// Gets the lookup data for the specified attribute id, lookup value Ids and application context in a locale provided by user
        /// </summary>
        /// <param name="attributeId">Indicates the id of the attribute for which lookup data needs to be fetched</param>
        /// <param name="locale">Indicates the locale for which data needs to be fetched</param>
        /// <param name="maxRecordsToReturn">Indicates the maximum number of lookup records to return. Setting '-1' returns all records</param>
        /// <param name="lookupValueIdList">Indicates a list of lookup ids to be returned along with requested maximum number of records</param>
        /// <param name="iApplicationContext">Indicates the current context of the application</param>
        /// <param name="iCallerContext">Indicates the name of the application and the module which has invoked the API</param>
        /// <returns>Returns lookup data for the specified attribute id, lookup values id, and application context in a locale provided by user</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        /// <example>
        /// <code language="c#" title="Get lookup by attribute id lookup value Ids and application context for locale" source="..\MDM.APISamples\LookupManager\GetLookupdata.cs" region="Get lookup by attribute id lookup values Id and application context for locale" />
        /// <code language="c#" title="Get lookup by attribute id lookup value Ids and application context for german locale" source="..\MDM.APISamples\LookupManager\GetLookupdata.cs" region="Get lookup by attribute id lookup values Id and application context for german locale" />
        /// </example>
        public ILookup GetAttributeLookupData(Int32 attributeId, LocaleEnum locale, Int32 maxRecordsToReturn, Collection<Int32> lookupValueIdList, IApplicationContext iApplicationContext, ICallerContext iCallerContext)
        {
            return MakeServiceCall("GetAttributeLookupData",
                                   "GetAttributeLookupData",
                                   service =>
                                   {
                                       Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
                                       if (isTracingEnabled)
                                       {
                                           MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Requested Attribute Lookup Data for AttributeId: {0}, Locale: {1}, MaxRecords: {2}, LookupValueIdList: {3}", attributeId, locale, maxRecordsToReturn, ValueTypeHelper.JoinCollection(lookupValueIdList, ",")), MDMTraceSource.LookupGet);
                                       }
                                       return service.GetAttributeLookupData(
                                           attributeId,
                                           locale,
                                           maxRecordsToReturn,
                                           lookupValueIdList,
                                           (ApplicationContext)iApplicationContext,
                                           FillDiagnosticTraces(iCallerContext));
                                   });
        }

        /// <summary>
        /// Gets the lookup data for the specified attribute id, lookup value Ids, application context and dependent attributes in a locale provided by user
        /// </summary>
        /// <param name="attributeId">Indicates the id of the attribute for which lookup data needs to be fetched</param>
        /// <param name="locale">Indicates the locale for which data needs to be fetched</param>
        /// <param name="maxRecordsToReturn">Indicates the maximum number of lookup records to return. Setting '-1' returns all records</param>
        /// <param name="lookupValueIdList">Indicates the list of lookup pk ids to be returned along with requested maximum number of records</param>
        /// <param name="iApplicationContext">Indicates the current context of the application</param>
        /// <param name="isDependent">Indicates whether the requested attribute is dependent or not</param>
        /// <param name="dependentAttributes">Indicates the dependency details</param>
        /// <param name="iCallerContext">Indicates the name of the application and the module which has invoked the API</param>
        /// <returns>Returns lookup data for the specified attribute id, lookup values id, application context, and dependent attributes in a locale provided by user</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        /// <example>
        /// <code language="c#" title="Get lookup by attribute id lookup value Ids application context and dependent attribute for locale" source="..\MDM.APISamples\LookupManager\GetLookupdata.cs" region="Get lookup by attribute id lookup values Id application context and dependent attribute for locale" />
        /// <code language="c#" title="Get lookup by attribute id lookup value Ids application context and dependent attribute for german locale" source="..\MDM.APISamples\LookupManager\GetLookupdata.cs" region="Get lookup by attribute id lookup values Id application context and dependent attribute for german locale" />
        /// </example>
        public ILookup GetAttributeLookupData(Int32 attributeId, LocaleEnum locale, Int32 maxRecordsToReturn, Collection<Int32> lookupValueIdList, IApplicationContext iApplicationContext, Boolean isDependent, IDependentAttributeCollection dependentAttributes, ICallerContext iCallerContext)
        {
            return MakeServiceCall("GetAttributeLookupData",
                                   "GetAttributeLookupData",
                                   service =>
                                   {
                                       Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
                                       if (isTracingEnabled)
                                       {
                                           MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Requested Attribute Lookup Data for AttributeId: {0}, Locale: {1}, MaxRecords: {2}, LookupValueIdList: {3}", attributeId, locale, maxRecordsToReturn, ValueTypeHelper.JoinCollection(lookupValueIdList, ",")), MDMTraceSource.LookupGet);
                                       }
                                       return service.GetAttributeLookupData(
                                           attributeId,
                                           locale,
                                           maxRecordsToReturn,
                                           lookupValueIdList,
                                           (ApplicationContext)iApplicationContext,
                                           FillDiagnosticTraces(iCallerContext),
                                           isDependent,
                                           (DependentAttributeCollection)dependentAttributes);
                                   });
        }

        /// <summary>
        /// Gets the lookup data for the specified attribute name, attribute parent name, lookup values id, application context, and dependent attributes in a locale specified by user
        /// </summary>
        /// <param name="attributeName">Indicates the short name of the attribute for which lookup data needs to be fetched</param>
        /// <param name="attributeParentName">Indicates the parent name of the attribute for which lookup data needs to be fetched</param>
        /// <param name="locale">Indicates the locale for which data needs to be fetched</param>
        /// <param name="maxRecordsToReturn">Indicates the maximum number of lookup records to return. Setting '-1' returns all record</param>
        /// <param name="lookupValueIdList">Indicates the list of lookup ids to be returned along with requested max number of records to return</param>
        /// <param name="applicationContext">Indicates the current context of the application</param>
        /// <param name="callerContext">Indicates the name of the application and the module which has invoked the API</param>
        /// <param name="isDependent">Indicates whether the attribute is dependent or not</param>
        /// <param name="dependentAttributeCollection">Indicates the dependent attribute collection details</param>
        /// <returns>Returns lookup data for the specified attribute name, attribute parent name, lookup values id, application context, and dependent attributes in a locale specified by user</returns>
        /// <example>
        /// <code language="c#" title="Get lookup by attribute name and attribute parent name with dependent attribute" source="..\MDM.APISamples\LookupManager\GetLookupdata.cs" region="GetLookupByAttributeNameAndParentNameWithDependentAttribute" />
        /// </example>
        public ILookup GetAttributeLookupData(String attributeName, String attributeParentName, LocaleEnum locale, Int32 maxRecordsToReturn, Collection<Int32> lookupValueIdList,
            IApplicationContext applicationContext, ICallerContext callerContext, Boolean isDependent, IDependentAttributeCollection dependentAttributeCollection)
        {
            return MakeServiceCall<Lookup>("GetAttributeLookupData", "GetAttributeLookupDataFilterBasedOnDependencyUsingName",
                client => client.GetAttributeLookupData(attributeName, attributeParentName, locale, maxRecordsToReturn, lookupValueIdList, (ApplicationContext)applicationContext,
                    FillDiagnosticTraces(callerContext), isDependent, (DependentAttributeCollection)dependentAttributeCollection));
        }

        /// <summary>
        /// Gets all the lookup records for the search value provided by user from the lookup table
        /// </summary>
        /// <param name="attributeId">Indicates the Id of the Attribute for which Lookup data needs to be fetched</param>
        /// <param name="locale">Indicates the Locale for which data needs to be fetched</param>
        /// <param name="maxRecordsToReturn">Indicates a maximum number of Lookup records to return. Setting '-1' returns all records</param>
        /// <param name="searchValue">Indicates the value to be searched for the Attribute Lookup</param>
        /// <param name="iApplicationContext">Indicates the current context of the application</param>
        /// <param name="iCallerContext">Indicates the name of the application and the module which invoked the API</param>
        /// <returns>Returns Lookup data for the requested attribute with search value in a locale provided by the user</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        /// <example>
        /// <code language="c#" title="Search lookup by Attribute id and Application Context in English Locale" source="..\MDM.APISamples\LookupManager\SearchLookupdata.cs" region="Search lookup by Attribute id and Application Context in English Locale" />
        /// <code language="c#" title="Search lookup by Attribute id and Application Context in German Locale" source="..\MDM.APISamples\LookupManager\SearchLookupdata.cs" region="Search lookup by Attribute id and Application Context in German Locale" />
        /// </example>
        public ILookup SearchAttributeLookupData(Int32 attributeId, LocaleEnum locale, Int32 maxRecordsToReturn, String searchValue, IApplicationContext iApplicationContext, ICallerContext iCallerContext)
        {
            return MakeServiceCall("SearchAttributeLookupData",
                                   "SearchAttributeLookupData",
                                   service =>
                                   {
                                       Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
                                       if (isTracingEnabled)
                                       {
                                           MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Requested Attribute Lookup Data for AttributeId: {0}, Locale: {1}, MaxRecords: {2}, SearchValue: {3}", attributeId, locale, maxRecordsToReturn, searchValue), MDMTraceSource.LookupGet);
                                       }
                                       return service.SearchAttributeLookupData(
                                           attributeId,
                                           locale,
                                           maxRecordsToReturn,
                                           searchValue,
                                           (ApplicationContext)iApplicationContext,
                                           FillDiagnosticTraces(iCallerContext));
                                   });
        }

        /// <summary>
        /// Gets all the lookup records for the search value and dependent attribute collection provided by user from the lookup table
        /// </summary>
        /// <param name="attributeId">Indicates Id of the Attribute for which Lookup data needs to be fetched</param>
        /// <param name="locale">Indicates the Locale for which data needs to be fetched</param>
        /// <param name="maxRecordsToReturn">Indicates a maximum number of Lookup records to return. Setting '-1' returns all records</param>
        /// <param name="searchValue">Indicates the value to be searched for the Attribute Lookup</param>
        /// <param name="iApplicationContext">Indicates the current context of the application</param>
        /// <param name="isDependent">Indicates whether the requested Attribute is Dependent Attribute or not</param>
        /// <param name="dependentAttributes">Indicates the Dependent Attribute Collection details</param>
        /// <param name="iCallerContext">Indicates the name of the application and the module which invoked the API</param>
        /// <returns>Returns Lookup data for the requested attribute with search value in a locale provided by the user</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        /// <example>
        /// <code language="c#" title="Search lookup by Attribute Id Application Context and DependentAttributeCollection in English Locale" source="..\MDM.APISamples\LookupManager\SearchLookupdata.cs" region="Search lookup by Attribute id Application Context and DependentAttributeCollection in English Locale" />
        /// <code language="c#" title="Search lookup by Attribute Id Application Context and DependentAttributeCollection in German Locale" source="..\MDM.APISamples\LookupManager\SearchLookupdata.cs" region="Search lookup by Attribute id Application Context and DependentAttributeCollection in German Locale" />
        /// </example>
        public ILookup SearchAttributeLookupData(Int32 attributeId, LocaleEnum locale, Int32 maxRecordsToReturn, String searchValue, IApplicationContext iApplicationContext, Boolean isDependent, IDependentAttributeCollection dependentAttributes, ICallerContext iCallerContext)
        {
            return MakeServiceCall("SearchAttributeLookupData",
                                   "SearchAttributeLookupData",
                                   service =>
                                   {
                                       Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
                                       if (isTracingEnabled)
                                       {
                                           MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Requested Attribute Lookup Data for AttributeId: {0}, Locale: {1}, MaxRecords: {2}, SearchValue: {3}", attributeId, locale, maxRecordsToReturn, searchValue), MDMTraceSource.LookupGet);
                                       }
                                       return service.SearchAttributeLookupData(
                                           attributeId,
                                           locale,
                                           maxRecordsToReturn,
                                           searchValue,
                                           (ApplicationContext)iApplicationContext,
                                           isDependent,
                                           (DependentAttributeCollection)dependentAttributes,
                                           FillDiagnosticTraces(iCallerContext));
                                   });
        }

        /// <summary>
        /// Gets Lookup Attribute Collection for requested Attribute Ids in a locale provided by user
        /// </summary>
        /// <param name="attributeIds">Indicates list of Attribute Ids for which data needs to be fetched</param>
        /// <param name="locale">Indicates the Locale for which data needs to be fetched</param>
        /// <param name="maxRecordsToReturn">Indicates the maximum number of Lookup records to return. Setting '-1' returns all record</param>
        /// <param name="iCallerContext">Indicates the name of application and module which invoked the API</param>
        /// <returns>Returns Lookup Attribute Collection for the requested Attribute Ids in a locale provided by the user</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        /// <example>
        /// <code language="c#" title="Get Lookup Attribute Collection Based on Attribute Ids" source="..\MDM.APISamples\LookupManager\GetLookupdata.cs" region="Get Lookup Attribute Collection Based on Attribute Ids" />
        /// </example>
        public ILookupCollection GetAttributesLookupData(Collection<Int32> attributeIds, LocaleEnum locale, Int32 maxRecordsToReturn, ICallerContext iCallerContext)
        {
            return MakeServiceCall("GetAttributesLookupData",
                                   "GetAttributesLookupData",
                                   service =>
                                   {
                                       Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
                                       if (isTracingEnabled)
                                       {
                                           MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Requested Attributes Lookup Data for Locale: {0}. maxRecordsToReturn: {1}", locale, maxRecordsToReturn), MDMTraceSource.LookupGet);
                                       }
                                       return service.GetAttributesLookupData(
                                           attributeIds,
                                           locale,
                                           maxRecordsToReturn,
                                           FillDiagnosticTraces(iCallerContext));
                                   });
        }

        /// <summary>
        /// Gets Lookup Attribute Collection for requested Attribute Ids under the given Application Context in a Locale provided by user
        /// </summary>
        /// <param name="attributeIds">Indicates the list of Attribute Ids for which data needs to be fetched</param>
        /// <param name="locale">Indicates the Locale for which data needs to be fetched</param>
        /// <param name="maxRecordsToReturn">Indicates the maximum number of Lookup records to return. Setting '-1' returns all record</param>
        /// <param name="iApplicationContext">Indicates the current context of the application</param>
        /// <param name="iCallerContext">Indicates the name of application and module which invoked the API</param>
        /// <returns>Returns Lookup Attribute Collection for the requested Attribute Ids under the given Application Context in a locale provided by the user</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        /// <example>
        /// <code language="c#" title="Get Lookup Attribute Collection Based on Attribute Ids And Application Context in English Locale" source="..\MDM.APISamples\LookupManager\GetLookupdata.cs" region="Get Lookup Attribute Collection Based on Attribute Ids And Application Context in English Locale" />
        /// <code language="c#" title="Get Lookup Attribute Collection Based on Attribute Ids And Application Context in German Locale" source="..\MDM.APISamples\LookupManager\GetLookupdata.cs" region="Get Lookup Attribute Collection Based on Attribute Ids And Application Context in German Locale" />
        /// </example>
        public ILookupCollection GetAttributesLookupData(Collection<Int32> attributeIds, LocaleEnum locale, Int32 maxRecordsToReturn, IApplicationContext iApplicationContext, ICallerContext iCallerContext)
        {
            return MakeServiceCall("GetAttributesLookupData",
                                   "GetAttributesLookupData",
                                   service =>
                                   {
                                       Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
                                       if (isTracingEnabled)
                                       {
                                           MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Requested Attributes Lookup Data for Locale: {0}. maxRecordsToReturn: {1}", locale, maxRecordsToReturn), MDMTraceSource.LookupGet);
                                       }
                                       return service.GetAttributesLookupData(
                                           attributeIds,
                                           locale,
                                           maxRecordsToReturn,
                                           (ApplicationContext)iApplicationContext,
                                           FillDiagnosticTraces(iCallerContext));
                                   });
        }

        /// <summary>
        /// Gets the lookup display values for the specified lookup attribute and lookup value Ids
        /// </summary>
        /// <param name="attributeValueRefIdPair">Indicates the collection of lookup attributes and the corresponding lookup value Ids</param>
        /// <param name="locale">Indicates the Locale for which data needs to be fetched</param>
        /// <param name="maxRecordsToReturn">Indicates the max number of each lookup items to return. Setting '-1' returns all record</param>
        /// <param name="iApplicationContext">Indicates the Current context of the application</param>
        /// <param name="iCallerContext">Indicates the name of the Application and Module which invoked the API</param>
        /// <returns>Returns the lookup display values for the specified lookup attribute and lookup value Ids</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        /// <example>
        /// <code language="c#" title="Get LookupAttribute DisplayValue" source="..\MDM.APISamples\LookupManager\GetLookupdata.cs" region="Get LookupAttribute DisplayValue" />
        /// </example>
        public Dictionary<Int32, Dictionary<Int32, String>> GetLookupAttributeDisplayValue(Dictionary<Int32, Collection<Int32>> attributeValueRefIdPair, LocaleEnum locale, Int32 maxRecordsToReturn, IApplicationContext iApplicationContext, ICallerContext iCallerContext)
        {
            return MakeServiceCall("GetLookupAttributeDisplayValue",
                                   "GetLookupAttributeDisplayValue",
                                   service =>
                                   {
                                       Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
                                       if (isTracingEnabled)
                                       {
                                           MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Requested Attributes Display Value Data for Locale: {0}. maxRecordsToReturn: {1}", locale, maxRecordsToReturn), MDMTraceSource.LookupGet);
                                       }
                                       return service.GetLookupAttributeDisplayValue(
                                           attributeValueRefIdPair,
                                           locale,
                                           maxRecordsToReturn,
                                           (ApplicationContext)iApplicationContext,
                                           FillDiagnosticTraces(iCallerContext));
                                   });
        }

        /// <summary>
        /// Processes the lookup data
        /// </summary>
        /// <param name="lookup">Indicates Lookup data which needs to be processed</param>
        /// <param name="iCallerContext">Indicates the caller context, which contains the application and module that has invoked the API</param>
        /// <param name="invalidateCache">Indicates whether to invalidate the lookup cache or not</param>
        /// <returns>Return the result of the processing the lookup data</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        /// <example>
        /// <code language="c#" title="Add Lookup Record" source="..\MDM.APISamples\LookupManager\ProcessLookup.cs" region="Add Lookup Record" />
        /// </example>
        public IOperationResult ProcessLookupData(ILookup lookup, ICallerContext iCallerContext, Boolean invalidateCache = true)
        {
            return MakeServiceCall("ProcessLookupData",
                "ProcessLookupData", service => service.ProcessLookupData(lookup as Lookup,
                                                                          iCallerContext.ProgramName,
                                                                          FillDiagnosticTraces(iCallerContext),
                                                                          invalidateCache));
        }

        /// <summary>
        /// Processes the lookup data collection
        /// </summary>
        /// <param name="lookups">Indicates a collection of lookup data data that needs to be processed</param>
        /// <param name="iCallerContext">Indicates the caller context, which contains the application and module that has invoked the API</param>
        /// <returns>Returns the result of the processing the lookup data collection</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        /// <example>
        /// <code language="c#" title="Add Lookup Records" source="..\MDM.APISamples\LookupManager\ProcessLookup.cs" region="Add Lookup Records" />
        /// </example>
        public IOperationResult ProcessLookupData(ILookupCollection lookups, ICallerContext iCallerContext)
        {
            return MakeServiceCall("ProcessLookupData",
                                   "ProcessLookupData",
                                   service => service.ProcessLookups(lookups as LookupCollection,
                                                                     iCallerContext.ProgramName,
                                                                     FillDiagnosticTraces(iCallerContext)));
        }

        /// <summary>
        /// Gets all related lookup table names for the requested lookup
        /// </summary>
        /// <param name="lookup">Indicates name of the current lookup table</param>
        /// <param name="iCallerContext">Indicates the caller context, which contains the application and module that has invoked the API</param>
        /// <returns>Returns collection of referrer lookups</returns>
        /// <example>
        /// <code language="c#" title="Get Related Lookups" source="..\MDM.APISamples\LookupManager\RelationshipLookup.cs" region="Get Related Lookups" />
        /// </example>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        public ILookupCollection GetRelatedLookups(ILookup lookup, ICallerContext iCallerContext)
        {
            return MakeServiceCall<LookupCollection>("GetRelatedLookups", "GetRelatedLookups",
                                    client =>
                                        client.GetRelatedLookups(lookup as Lookup, FillDiagnosticTraces(iCallerContext)));
        }

        /// <summary>
        /// Gets the lookup schema based on the requested lookup table names
        /// </summary>
        /// <param name="lookupNames">Indicates a list of lookup table names</param>
        /// <param name="callerContext">Indicates the caller context, which contains the application and module that has invoked the API</param>
        /// <returns>Returns the list of lookup schemas</returns>
        /// <example>
        /// <code language="c#" title="Get Lookup Schema" source="..\MDM.APISamples\LookupManager\LookupSchema.cs" region="GetLookupsSchema" />
        /// </example>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        public ILookupCollection GetLookupSchema(Collection<String> lookupNames, ICallerContext callerContext)
        {
            return MakeServiceCall<LookupCollection>("GetLookupSchema", "GetLookupsSchema",
                                               client =>
                                                   client.GetLookupSchema(lookupNames, FillDiagnosticTraces(callerContext)));
        }

        /// <summary>
        /// Gets the lookup schema based on the requested lookup table name
        /// </summary>
        /// <param name="lookupName">Indicates the lookup name</param>
        /// <param name="callerContext">Indicates the caller context, which contains the application and module that has invoked the API</param>
        /// <returns>Returns the lookup schema</returns>
        /// <example>
        /// <code language="c#" title="Get Lookup Schema" source="..\MDM.APISamples\LookupManager\LookupSchema.cs" region="GetLookupsSchema" />
        /// </example>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        public ILookup GetLookupSchema(String lookupName, ICallerContext callerContext)
        {
            return MakeServiceCall<Lookup>("GetLookupSchema", "GetLookupSchema",
                                             client =>
                                                 client.GetLookupSchema(lookupName, FillDiagnosticTraces(callerContext)));
        }

        #endregion

        #region Copy Container Mappings

        /// <summary>
        /// Copies the Mappings from the Source Container to the Target Container
        /// </summary>
        /// <param name="sourceContainerId">Indicates the Source Container ID</param>
        /// <param name="targetContainerId">Indicates the Target Container ID</param>
        /// <param name="iContainerTemplateCopyContext">Indicates all mappings that needs to be copied from Source to Target</param>
        /// <param name="iCallerContext">Indicates the name of application and module that are performing the action</param>
        /// <returns>Returns a value that indicates success or failure state </returns>
        public IOperationResult CopyContainerMappings(Int32 sourceContainerId, Int32 targetContainerId, IContainerTemplateCopyContext iContainerTemplateCopyContext, ICallerContext iCallerContext)
        {
            return MakeServiceCall("CopyContainerMappings",
                                   "CopyContainerMappings",
                                   service => service.CopyContainerMappings(
                                       sourceContainerId,
                                       targetContainerId,
                                       (ContainerTemplateCopyContext)iContainerTemplateCopyContext,
                                       FillDiagnosticTraces(iCallerContext)));
        }

        #endregion Copy Container Mappings

        #region EntityType Get

        /// <summary>
        /// Gets all Entity Types in the System
        /// </summary>
        /// <param name="iCallerContext">Indicates the name of the application and the module that are performing the action</param>
        /// <param name="getLatest">Indicates a Boolean flag which says whether to get from DB or cache. A Value True always gets from DB</param>
        /// <returns>Returns a collection of Entity Types</returns>
        public IEntityTypeCollection GetAllEntityTypes(ICallerContext iCallerContext, Boolean getLatest = false)
        {
            return MakeServiceCall("GetEntityTypes",
                                   "GetAllEntityTypes",
                                   service => service.GetAllEntityTypes(FillDiagnosticTraces(iCallerContext), getLatest));
        }

        /// <summary>
        /// Get all entity types by list of ids
        /// </summary>
        /// <param name="entityTypeIds">Collection of EntityType Ids to search in the system</param>
        /// <returns>Collection of EntityTypes with specified Ids in the Id list</returns>
        public IEntityTypeCollection GetEntityTypesByIds(Collection<Int32> entityTypeIds)
        {
            return MakeServiceCall("GetEntityTypesByIds",
                                   "GetEntityTypesByIds",
                                   service => service.GetEntityTypesByIds(entityTypeIds));
        }

        /// <summary>
        /// Gets Entity Type based on ID
        /// </summary>
        /// <param name="entityTypeId">Indicates Entity Type ID for which data is to be fetched</param>
        /// <param name="iCallerContext">Indicates the name of application and module that are performing the action</param>
        /// <returns>Returns Entity Type for given ID</returns>
        public IEntityType GetEntityTypeById(Int32 entityTypeId, ICallerContext iCallerContext)
        {
            return MakeServiceCall<IEntityType>("GetEntityTypeById", "GetEntityTypeById", client => client.GetEntityTypeById(entityTypeId, FillDiagnosticTraces(iCallerContext)));
        }

        /// <summary>
        /// Gets entity types based on unique entity type short names provided        
        /// </summary>
        /// <param name="entityTypeShortNames">Indicates collection of entity type short names</param>
        /// <param name="iCallerContext">Indicates the name of application and the module that are performing the action</param>
        /// <returns>Returns entity type collection for given entity type short names</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        public IEntityTypeCollection GetEntityTypesByShortNames(Collection<String> entityTypeShortNames, ICallerContext iCallerContext)
        {
            return MakeServiceCall("GetEntityTypesByShortNames", "GetEntityTypesByShortNames",
                client =>
                client.GetEntityTypesByShortNames(entityTypeShortNames, FillDiagnosticTraces(iCallerContext)));
        }

        /// <summary>
        /// Gets entity type based on unique entity type short name provided
        /// </summary>
        /// <param name="entityTypeShortName">Indicates entity type short name</param>
        /// <param name="iCallerContext">Indicates the name of application and the module that are performing the action</param>
        /// <returns>Returns entity type for given entity type short name</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        public IEntityType GetEntityTypeByShortName(String entityTypeShortName, ICallerContext iCallerContext)
        {
            return MakeServiceCall("GetEntityTypeByShortName", "GetEntityTypeByShortName",
                client =>
                client.GetEntityTypeByShortName(entityTypeShortName, FillDiagnosticTraces(iCallerContext)));
        }
        #endregion EntityType Get

        #region RelationshipType CUD

        /// <summary>
        /// Creates Relationship Type
        /// </summary>
        /// <param name="iRelationshipType">Indicates a Relationship Object to be created</param>
        /// <param name="iCallerContext">Indicates the name of application and module that are performing the action</param>
        /// <returns>Returns a Success or Failure state</returns>
        /// <exception cref="ArgumentNullException">Thrown when RelationshipType Object is Null</exception>
        /// <exception cref="ArgumentNullException">Thrown when RelationshipType ShortName is Null or has empty String</exception>
        /// <exception cref="ArgumentNullException">Thrown when RelationshipType LongName is Null or has empty String</exception>
        public IOperationResult CreateRelationshipType(IRelationshipType iRelationshipType, ICallerContext iCallerContext)
        {
            return MakeServiceCall("CreateRelationshipType", "CreateRelationshipType",
               client =>
               client.CreateRelationshipType(iRelationshipType as RelationshipType, FillDiagnosticTraces(iCallerContext)));
        }

        /// <summary>
        /// Updates Relationship Type
        /// </summary>
        /// <param name="iRelationshipType">Indicates a Relationship Type Object to be updated</param>
        /// <param name="iCallerContext">Indicates the name of application and module that are performing the action</param>
        /// <returns>Returns a Success or Failure state</returns>
        /// <exception cref="ArgumentNullException">Thrown when RelationshipType Object is Null</exception>
        /// <exception cref="ArgumentNullException">Thrown when RelationshipType ShortName is Null or has empty String</exception>
        /// <exception cref="ArgumentNullException">Thrown when RelationshipType LongName is Null or has empty String</exception>
        public IOperationResult UpdateRelationshipType(IRelationshipType iRelationshipType, ICallerContext iCallerContext)
        {
            return MakeServiceCall("UpdateRelationshipType", "UpdateRelationshipType",
               client =>
               client.UpdateRelationshipType(iRelationshipType as RelationshipType, FillDiagnosticTraces(iCallerContext)));
        }

        /// <summary>
        /// Deletes RelationshipType
        /// </summary>
        /// <param name="iRelationshipType">Indicates a Relationship Object to be deleted</param>
        /// <param name="iCallerContext">Indicates the name of application and module that are performing the action</param>
        /// <returns>Returns a Success or Failure state</returns>
        /// <exception cref="ArgumentNullException">Thrown when RelationshipType Object is Null</exception>
        public IOperationResult DeleteRelationshipType(IRelationshipType iRelationshipType, ICallerContext iCallerContext)
        {
            return MakeServiceCall("DeleteRelationshipType", "DeleteRelationshipType",
               client =>
               client.DeleteRelationshipType(iRelationshipType as RelationshipType, FillDiagnosticTraces(iCallerContext)));
        }

        /// <summary>
        /// Processes Relationship Types
        /// </summary>
        /// <param name="iRelationshipTypes">Indicates a Relationship Object to be processed</param>
        /// <param name="iCallerContext">Indicates the name of application and module that are performing the action</param>
        /// <returns>Returns a Success or Failure state</returns>
        /// <exception cref="ArgumentNullException">Thrown when RelationshipType Object is Null</exception>
        public IOperationResultCollection ProcessRelationshipTypes(IRelationshipTypeCollection iRelationshipTypes, ICallerContext iCallerContext)
        {

            return MakeServiceCall("ProcessRelationshipTypes", "ProcessRelationshipTypes",
               client =>
               client.ProcessRelationshipTypes(iRelationshipTypes as RelationshipTypeCollection, FillDiagnosticTraces(iCallerContext)));
        }

        #endregion RelationshipType CUD

        #region RelationshipType Get

        /// <summary>
        /// Gets all Relationship Types
        /// </summary>
        /// <param name="iCallerContext">Indicates the name of the application and the module that are performing the action</param>
        /// <returns>Returns a collection of Relationship Types</returns>
        public IRelationshipTypeCollection GetAllRelationshipTypes(ICallerContext iCallerContext)
        {
            return MakeServiceCall("GetAllRelationshipTypes", "GetAllRelationshipTypes",
               client =>
               client.GetAllRelationshipTypes(FillDiagnosticTraces(iCallerContext)));
        }

        /// <summary>
        /// Gets Relationship Type for given ID
        /// </summary>
        /// <param name="relationshipTypeId">Indicates ID of Relationship Type for which values are to be fetched</param>
        /// <param name="iCallerContext">Indicates the name of application and module that are performing the action</param>
        /// <returns>Returns Relationship Type for given ID</returns>
        public IRelationshipType GetRelationshipTypeById(Int32 relationshipTypeId, ICallerContext iCallerContext)
        {
            return MakeServiceCall("GetRelationshipTypeById", "GetRelationshipTypeById",
               client =>
               client.GetRelationshipTypeById(relationshipTypeId, FillDiagnosticTraces(iCallerContext)));
        }

        /// <summary>
        /// Gets relationship type based on given name
        /// </summary>
        /// <param name="relationshipTypeName">Indicates name of the relationship type</param>
        /// <param name="iCallerContext">Indicates the name of the application and the module that are performing the action</param>
        /// <returns>Returns relationship type based on name</returns>
        public IRelationshipType GetRelationshipTypeByName(String relationshipTypeName, ICallerContext iCallerContext)
        {
            return MakeServiceCall("GetRelationshipTypeByName", "GetRelationshipTypeByName",
                client =>
                client.GetRelationshipTypeByName(relationshipTypeName, FillDiagnosticTraces(iCallerContext)));
        }

        /// <summary>
        /// Gets all relationship types based on container id and entity type id
        /// </summary>
        /// <example>
        /// <code>     
        /// //Create an instance of datamodel service
        /// DataModelService mdmDataModelService = GetMDMDataModelService();
        /// 
        /// // Key Note: CallerContext has properties Application and Module which are mandatory to be set
        /// // Indicates name of application and module
        /// ICallerContext callerContext = <![CDATA[MDMObjectFactory.GetICallerContext()]]>;
        /// callerContext.Application = MDMCenterApplication.MDMCenter;
        /// callerContext.Module = MDMCenterModules.Entity;
        /// 
        /// //Values are as per River Works Data Model
        /// //Product Master catalog
        /// Int32 containerId = 5;
        /// 
        /// //Kitcode Entity Type
        /// Int32 entityTypeId = 31;
        /// 
        /// // Below will make WCF call which gives relationship types based on entity type,container id and callercontext
        /// IRelationshipTypeCollection relationshipCollection = mdmDataModelService.GetRelationshipTypes(containerId, entityTypeId, callerContext);
        /// </code>
        /// </example>
        /// <param name="containerId">Specifies container id.</param>
        /// <param name="entityTypeId">Specifies entity type id.</param>
        /// <param name="iCallerContext">Indicates the name of the application and the module that are performing the action</param>
        /// <returns>collection of relationship types</returns>
        public IRelationshipTypeCollection GetRelationshipTypes(Int32 containerId, Int32 entityTypeId, ICallerContext iCallerContext)
        {
            return MakeServiceCall("GetRelationshipTypes", "GetRelationshipTypes",
               client =>
               client.GetRelationshipTypes(containerId, entityTypeId, FillDiagnosticTraces(iCallerContext)));
        }

        #endregion RelationshipType Get

        #region EntityType Process

        /// <summary>
        /// Processes Entity Type collection
        /// </summary>
        /// <param name="iEntityTypeCollection">Indicates Entity Type collection to process</param>
        /// <param name="iCallerContext">Indicates the name of application and module that are performing the action</param>
        /// <returns>Returns collection of Operation Result</returns>
        public IOperationResultCollection ProcessEntityTypes(IEntityTypeCollection iEntityTypeCollection, ICallerContext iCallerContext)
        {
            return MakeServiceCall("ProcessEntityTypes", "ProcessEntityTypes",
                client =>
                client.ProcessEntityTypes(iEntityTypeCollection as EntityTypeCollection, FillDiagnosticTraces(iCallerContext)));
        }

        #endregion EntityType Process

        #region Organization Methods

        #region Get Methods

        /// <summary>
        /// Gets all Organizations in the System by Caller Context
        /// </summary>
        /// <param name="organizationId">Indicates the ID of the Organization</param>
        /// <param name="iOrganizationContext">Indicates the context containing flag to load attributes or not</param>
        /// <param name="iCallerContext">Indicates the name of the application and the module that are performing the action</param>
        /// <returns>Returns a collection of Organizations</returns>
        public IOrganization GetOrganizationById(Int32 organizationId, IOrganizationContext iOrganizationContext, ICallerContext iCallerContext)
        {
            return MakeServiceCall("GetOrganizationById", "GetOrganizationById",
                client =>
                client.GetOrganizationById(organizationId, iOrganizationContext as OrganizationContext, FillDiagnosticTraces(iCallerContext)));
        }

        /// <summary>
        /// Gets all Organizations in the System by the Caller Context
        /// </summary>
        /// <param name="iOrganizationContext">Indicates the context containing flag to load attributes or not</param>
        /// <param name="iCallerContext">Indicates the name of the application and the module that are performing the action</param>
        /// <returns>Returns a collection of Organization</returns>
        public IOrganizationCollection GetAllOrganizations(IOrganizationContext iOrganizationContext, ICallerContext iCallerContext)
        {
            return MakeServiceCall("GetAllOrganizations", "GetAllOrganizations",
                client =>
                client.GetAllOrganizations(iOrganizationContext as OrganizationContext, FillDiagnosticTraces(iCallerContext)));
        }

        /// <summary>
        /// Gets Organization's childs in the System by ID
        /// </summary>
        /// <param name="parentOrganizationId">Indicates ID of the Organization</param>
        /// <param name="iCallerContext">Indicates the name of the application and the module that are performing the action</param>
        /// <returns>Returns a collection of child Objects</returns>
        public IMDMObjectInfoCollection GetAllOrganizationDependencies(Int32 parentOrganizationId, ICallerContext iCallerContext)
        {
            return MakeServiceCall("GetAllOrganizationDependencies", "GetAllOrganizationDependencies",
                client =>
                client.GetAllOrganizationDependencies(parentOrganizationId, FillDiagnosticTraces(iCallerContext)));
        }

        /// <summary>
        /// Get organization based on given organization short name
        /// </summary>
        /// <example>
        /// <code language="c#" title="Get Organization By Name"  source="..\..\Documentation\MDM.APISamples\DataModelManager\Organization\GetOrganizationByName.cs" region="Organization By Name"/>
        /// </example>
        /// <param name="organizationShortName">Indicates short name of the organization</param>
        /// <param name="iOrganizationContext">Indicates context of organization which tells whether to load organization attributes or not</param>
        /// <param name="iCallerContext">Indicates the name of the application and the module that are performing the action</param>
        /// <returns>Returns organization based on organization short name</returns>
        public IOrganization GetOrganizationByName(String organizationShortName, IOrganizationContext iOrganizationContext, ICallerContext iCallerContext)
        {
            return MakeServiceCall("GetAllOrganizations", "GetAllOrganizations",
                client =>
                client.GetOrganizationByName(organizationShortName, iOrganizationContext as OrganizationContext, FillDiagnosticTraces(iCallerContext)));

        }
        #endregion

        #region CUD Methods

        /// <summary>
        /// Creates Organization
        /// </summary>
        /// <param name="iOrganization">Indicates an Organization Object to be created</param>
        /// <param name="iCallerContext">Indicates the name of application and module that are performing the action</param>
        /// <returns>Returns the state of Organization creation</returns>
        public IOperationResult CreateOrganization(IOrganization iOrganization, ICallerContext iCallerContext)
        {
            return MakeServiceCall("CreateOrganization", "CreateOrganization",
               client =>
               client.CreateOrganization(iOrganization as Organization, FillDiagnosticTraces(iCallerContext)));
        }

        /// <summary>
        /// Updates Organization
        /// </summary>
        /// <param name="iOrganization">Indicates an Organization Object to be updated</param>
        /// <param name="iCallerContext">Indicates the name of application and module that are performing the action</param>
        /// <returns>Returns the state of Organization updation</returns>
        public IOperationResult UpdateOrganization(IOrganization iOrganization, ICallerContext iCallerContext)
        {
            return MakeServiceCall("UpdateOrganization", "UpdateOrganization",
               client =>
               client.UpdateOrganization(iOrganization as Organization, FillDiagnosticTraces(iCallerContext)));
        }

        /// <summary>
        /// Deletes Organization
        /// </summary>
        /// <param name="iOrganization">Indicates an Organization Object to be deleted</param>
        /// <param name="iCallerContext">Indicates the name of application and module that are performing the action</param>
        /// <returns>Returns the state of Organization Deletion</returns>
        public IOperationResult DeleteOrganization(IOrganization iOrganization, ICallerContext iCallerContext)
        {
            return MakeServiceCall("DeleteOrganization", "DeleteOrganization",
               client =>
               client.DeleteOrganization(iOrganization as Organization, FillDiagnosticTraces(iCallerContext)));
        }

        #endregion

        #region Process Methods

        /// <summary>
        /// Processes operations with <see cref="OrganizationCollection"/>
        /// </summary>
        /// <param name="iOrganizationCollection">Indicates collection of Organizations</param>
        /// <param name="iCallerContext">Indicates the name of application and module that are performing the action</param>
        /// <returns>Returns instance of the <see cref="OperationResult"/></returns>
        public IOperationResultCollection ProcessOrganizations(IOrganizationCollection iOrganizationCollection, ICallerContext iCallerContext)
        {
            return MakeServiceCall("ProcessOrganizations", "ProcessOrganizations",
                                         client =>
                                             client.ProcessOrganizations(iOrganizationCollection as OrganizationCollection, FillDiagnosticTraces(iCallerContext)));
        }

        #endregion

        #endregion

        #region Hierarchy Get

        /// <summary>
        /// Gets all Hierarchies in the System by the Caller Context
        /// </summary>
        /// <param name="iCallerContext">Indicates the name of the application and the module that are performing the action</param>
        /// <returns>Returns a collection of Hierarchy</returns>
        public HierarchyCollection GetAllHierarchies(ICallerContext iCallerContext)
        {
            return MakeServiceCall("GetAllHierarchies", "GetAllHierarchies",
                client =>
                client.GetAllHierarchies(FillDiagnosticTraces(iCallerContext)));
        }

        /// <summary>
        /// Get hierarchy based on provided name.
        /// </summary>
        /// <param name="hierarchyShortName">Indicates short name - i.e. unique name of hierarchy</param>
        /// <param name="iCallerContext">Indicates the name of the application and the module that are performing the action</param>
        /// <returns>Returns hierarchy object based on provided name</returns>
        public Hierarchy GetHierarchyByName(String hierarchyShortName, ICallerContext iCallerContext)
        {
            return MakeServiceCall("GetHierarchyByName", "GetHierarchyByName",
                 client =>
                 client.GetHierarchyByName(hierarchyShortName, FillDiagnosticTraces(iCallerContext)));

        }

        #endregion hierarchy Get

        #region Hierarchy Process

        /// <summary>
        /// Processes <see cref="HierarchyCollection"/>
        /// </summary>
        /// <param name="iHierarchyCollection">Indicates a collection of Hierarchy</param>
        /// <param name="iCallerContext">Indicates the name of the application and the module that are performing the action</param>
        /// <returns>Returns an instance of the <see cref="OperationResult"/></returns>
        public OperationResultCollection ProcessHierarchies(IHierarchyCollection iHierarchyCollection, ICallerContext iCallerContext)
        {
            return MakeServiceCall("ProcessHierarchies", "ProcessHierarchies",
                                         client =>
                                             client.ProcessHierarchies(iHierarchyCollection as HierarchyCollection, FillDiagnosticTraces(iCallerContext)));
        }

        #endregion Hierarchy Process

        #region Hierarchy CUD

        /// <summary>
        /// Creates <see cref="Hierarchy"/>
        /// </summary>
        /// <param name="iHierarchy">Indicates a Hierarchy Object to be created</param>
        /// <param name="iCallerContext">Indicates the name of application and module that are performing the action</param>
        /// <returns>Returns the state of <see cref="OperationResult"/> creation </returns>
        public OperationResult CreateHierarchy(IHierarchy iHierarchy, ICallerContext iCallerContext)
        {
            return MakeServiceCall("CreateHierarchy", "CreateHierarchy",
                                         client =>
                                             client.CreateHierarchy(iHierarchy as Hierarchy, FillDiagnosticTraces(iCallerContext)));
        }

        /// <summary>
        /// Updates <see cref="Hierarchy"/>
        /// </summary>
        /// <param name="iHierarchy">Indicates a Hierarchy Object to be updated</param>
        /// <param name="iCallerContext">Indicates the name of application and module that are performing the action</param>
        /// <returns>Returns the state of  <see cref="OperationResult"/> updation</returns>
        public OperationResult UpdateHierarchy(IHierarchy iHierarchy, ICallerContext iCallerContext)
        {
            return MakeServiceCall("UpdateHierarchy", "UpdateHierarchy",
                                         client =>
                                             client.UpdateHierarchy(iHierarchy as Hierarchy, FillDiagnosticTraces(iCallerContext)));
        }

        /// <summary>
        /// Deletes <see cref="Hierarchy"/>
        /// </summary>
        /// <param name="iHierarchy">Indicates a Hierarchy Object to be deleted</param>
        /// <param name="iCallerContext">Indicates the name of application and module that are performing the action</param>
        /// <returns>Returns the state of  <see cref="OperationResult"/> deletion</returns>
        public OperationResult DeleteHierarchy(IHierarchy iHierarchy, ICallerContext iCallerContext)
        {
            return MakeServiceCall("DeleteHierarchy", "DeleteHierarchy",
                                         client =>
                                             client.DeleteHierarchy(iHierarchy as Hierarchy, FillDiagnosticTraces(iCallerContext)));
        }

        #endregion Hierarchy CUD

        #region Category - Attribute Mapping Methods

        #region Category - Attribute Mapping Get

        /// <summary>
        /// Gets category for attribute mappings
        /// </summary>
        /// <param name="categoryId">Identifies the Category</param>
        /// <param name="iCallerContext">Identifies the caller context, which contains information about who the caller is</param>
        /// <returns>Returns the collection of (Category to attribute) mapping based on the unique identifier of a Category</returns>
        public ICategoryAttributeMappingCollection GetCategoryAttributeMappingsByCategoryId(Int64 categoryId,
                                                                                    ICallerContext iCallerContext)
        {
            return MakeServiceCall("GetCategoryAttributeMappingsByCategoryId", "GetCategoryAttributeMappingsByCategoryId",
                client =>
                client.GetCategoryAttributeMappingsByCategoryId(categoryId, FillDiagnosticTraces(iCallerContext)));
        }

        /// <summary>
        /// Gets category for attribute mappings
        /// </summary>
        /// <param name="catalogId">Identifies the Catalog </param>
        /// <param name="iCallerContext">Identifies the caller context, which contains information about who the caller is</param>
        /// <returns>Returns the collection of (Category to attribute) mapping based on the unique identifier of a Catalog</returns>
        public ICategoryAttributeMappingCollection GetCategoryAttributeMappingsByCatalogId(Int32 catalogId,
                                                                                          ICallerContext iCallerContext)
        {
            return MakeServiceCall("GetCategoryAttributeMappingsByCatalogId", "GetCategoryAttributeMappingsByCatalogId",
                client =>
                client.GetCategoryAttributeMappingsByCatalogId(catalogId, FillDiagnosticTraces(iCallerContext)));
        }


        /// <summary>
        /// Gets category for attribute mappings
        /// </summary>
        /// <param name="hierarchyId">Identifies the Hierarchy</param>
        /// <param name="iCallerContext">Identifies the caller context, which contains information about who the caller is</param>
        /// <returns>Returns the collection of (Category to attribute) mapping based on the unique identifier of a Hierarchy</returns>
        public ICategoryAttributeMappingCollection GetCategoryAttributeMappingsByHierarchyId(Int32 hierarchyId,
                                                                                            ICallerContext iCallerContext)
        {
            return MakeServiceCall("GetCategoryAttributeMappingsByHierarchyId", "GetCategoryAttributeMappingsByHierarchyId",
                client =>
                client.GetCategoryAttributeMappingsByHierarchyId(hierarchyId, FillDiagnosticTraces(iCallerContext)));
        }

        #endregion Category - Attribute Mapping Get

        #region Category - Attribute Mapping Process

        /// <summary>
        /// Process (CRUD) operations with entity of type <see cref="CategoryAttributeMappingCollection"/>
        /// </summary>
        /// <param name="iCategoryAttributeMappingCollection">Collection of Category - Attribute Mappings</param>
        /// <param name="iCallerContext">Context of the caller app</param>
        /// <returns>Instance of the <see cref="OperationResult"/></returns>
        public IOperationResultCollection ProcessCategoryAttributeMappings(ICategoryAttributeMappingCollection iCategoryAttributeMappingCollection, ICallerContext iCallerContext)
        {
            return MakeServiceCall("ProcessCategoryAttributeMappings", "ProcessCategoryAttributeMappings",
                                         client =>
                                             client.ProcessCategoryAttributeMappings(iCategoryAttributeMappingCollection as CategoryAttributeMappingCollection, FillDiagnosticTraces(iCallerContext)));
        }

        #endregion Category - Attribute Mapping Process

        #region Category - Attribute Mapping Inherit

        /// <summary>
        /// Inherits entity of type <see cref="CategoryAttributeMapping"/>
        /// </summary>
        /// <param name="iCategoryAttributeMapping">The Category - Attribute Mapping</param>
        /// <param name="iCallerContext">Context of the caller app</param>
        /// <returns>Instance of the <see cref="OperationResult"/></returns>
        public IOperationResult InheritCategoryAttributeMapping(ICategoryAttributeMapping iCategoryAttributeMapping, ICallerContext iCallerContext)
        {
            return MakeServiceCall("InheritCategoryAttributeMapping", "InheritCategoryAttributeMapping",
                                         client =>
                                             client.InheritCategoryAttributeMapping(iCategoryAttributeMapping as CategoryAttributeMapping, FillDiagnosticTraces(iCallerContext)));
        }

        #endregion Category - Attribute Mapping Process

        #region Category - Attribute Mapping CUD

        /// <summary>
        /// Creates entity of type <see cref="CategoryAttributeMapping"/>
        /// </summary>
        /// <param name="iCategoryAttributeMapping">The Category - Attribute Mapping</param>
        /// <param name="iCallerContext">Context of the caller app</param>
        /// <returns>Instance of the <see cref="OperationResult"/></returns>
        public IOperationResult CreateCategoryAttributeMapping(ICategoryAttributeMapping iCategoryAttributeMapping, ICallerContext iCallerContext)
        {
            return MakeServiceCall("CreateCategoryAttributeMapping", "CreateCategoryAttributeMapping",
                                         client =>
                                             client.CreateCategoryAttributeMapping(iCategoryAttributeMapping as CategoryAttributeMapping, FillDiagnosticTraces(iCallerContext)));
        }

        /// <summary>
        /// Updates entity of type <see cref="CategoryAttributeMapping"/>
        /// </summary>
        /// <param name="iCategoryAttributeMapping">The Category - Attribute Mapping</param>
        /// <param name="iCallerContext">Context of the caller app</param>
        /// <returns>Instance of the <see cref="OperationResult"/></returns>
        public IOperationResult UpdateCategoryAttributeMapping(ICategoryAttributeMapping iCategoryAttributeMapping, ICallerContext iCallerContext)
        {
            return MakeServiceCall("UpdateCategoryAttributeMapping", "UpdateCategoryAttributeMapping",
                                         client =>
                                             client.UpdateCategoryAttributeMapping(iCategoryAttributeMapping as CategoryAttributeMapping, FillDiagnosticTraces(iCallerContext)));
        }

        /// <summary>
        /// Deletes entity of type <see cref="CategoryAttributeMapping"/>
        /// </summary>
        /// <param name="iCategoryAttributeMapping">The Category - Attribute Mapping</param>
        /// <param name="iCallerContext">Context of the caller app</param>
        /// <returns>Instance of the <see cref="OperationResult"/></returns>
        public IOperationResult DeleteCategoryAttributeMapping(ICategoryAttributeMapping iCategoryAttributeMapping, ICallerContext iCallerContext)
        {
            return MakeServiceCall("DeleteCategoryAttributeMapping", "DeleteCategoryAttributeMapping",
                                         client =>
                                             client.DeleteCategoryAttributeMapping(iCategoryAttributeMapping as CategoryAttributeMapping, FillDiagnosticTraces(iCallerContext)));
        }

        #endregion Category - Attribute Mapping CUD

        #endregion Category - Attribute Mapping Methods

        #region Dependency Attribute Methods

        /// <summary>
        /// Gets the dependency mapping details for the requested Attribute
        /// This method will return the link table details.
        /// For example, if the the Attribute is a Lookup, then will it return the WSID of the Lookup table which is mapped to the requested Attribute
        /// If the Attribute is a non-lookup Attribute, then it will return the Dependent values based on the mapping link.
        /// </summary>
        /// <param name="attributeId">Indicates ID of the Attribute for which dependency details needs to be fetched</param>
        /// <param name="iApplicationContext">Indicates current context of the application</param>
        /// <param name="IdependentAttributeCollection">Indicates Dependency Attribute mapping details for the Attribute</param>
        /// <param name="iCallerContext">Indicates the name of application and module that are performing the action</param>
        /// <returns>Returns collection of String values</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        public Collection<String> GetDependencyMappings(Int32 attributeId, IApplicationContext iApplicationContext, IDependentAttributeCollection IdependentAttributeCollection, ICallerContext iCallerContext)
        {
            return MakeServiceCall("GetDependencyMappings",
                                   "GetDependencyMappings",
                                   service => service.GetDependencyMappings(
                                       attributeId,
                                       (ApplicationContext)iApplicationContext,
                                       (DependentAttributeCollection)IdependentAttributeCollection,
                                       FillDiagnosticTraces(iCallerContext)));
        }

        /// <summary>
        /// Gets the Dependency Details for the requested Attribute
        /// </summary>
        /// <param name="attributeId">Indicates ID of the attribute for which dependency details needs to be fetched</param>
        /// <param name="iApplicationContext">Indicates current context of the application </param>
        /// <param name="iCallerContext">Indicates the name of application and module that are performing the action</param>
        /// <param name="includeChildDependency"> Indicates to include Child Dependency. A True value will include Child dependency</param>
        /// <returns>Returns collection of Dependent Attribute </returns>
        public DependentAttributeCollection GetDependencyDetails(Int32 attributeId, IApplicationContext iApplicationContext, Boolean includeChildDependency, ICallerContext iCallerContext)
        {
            return MakeServiceCall("GetDependencyDetails",
                                   "GetDependencyDetails",
                                   service => service.GetDependencyDetails(attributeId,
                                                                           (ApplicationContext)iApplicationContext,
                                                                           includeChildDependency,
                                                                           FillDiagnosticTraces(iCallerContext)));
        }

        /// <summary>
        /// Get all dependencies for given attribute.
        /// </summary>
        /// <param name="attributeId">Attribute Id for which dependencies are to be selected</param>
        /// <param name="callerContext">Context indicating application making this API call.</param>
        /// <returns>Attribute Dependencies having collection of parent attribute and context for given attribute id</returns>
        public IDependentAttributeCollection GetAttributeDependencies(Int32 attributeId, ICallerContext callerContext)
        {
            return this.MakeServiceCall<IDependentAttributeCollection>("GetAttributeDependencies", "GetAttributeDependencies", client => client.GetAttributeDependencies(attributeId, FillDiagnosticTraces(callerContext)));
        }

        /// <summary>
        /// Get the dependent attribute data for the requested link tableD
        /// </summary>
        /// <param name="linkTableName">Indicates the link table name</param>
        /// <param name="locale">Indicates the locale</param>
        /// <param name="callerContext">Indicates the caller context information regarding application and module.</param>
        /// <returns>Returns the dependent attribute data mapping collection for the respective link table.</returns>
        public DependentAttributeDataMapCollection GetDependentData(String linkTableName, LocaleEnum locale, ICallerContext callerContext)
        {
            return this.MakeServiceCall<DependentAttributeDataMapCollection>("GetDependentData", "GetDependentData", client => client.GetDependentData(linkTableName, locale, FillDiagnosticTraces(callerContext)));
        }

        /// <summary>
        /// Get all child dependent attribute models
        /// </summary>
        /// <param name="modelContext">Attribute model context which indicates what all attribute models to load.</param>
        /// <param name="callerContext">Context indicating the caller of the API</param>
        /// <returns>Dependent child attribute model</returns>
        public IAttributeModelCollection GetAllDependentChildAttributeModels(IAttributeModelContext modelContext, ICallerContext callerContext)
        {
            return this.MakeServiceCall<IAttributeModelCollection>("GetAllDependentChildAttributeModels", "GetAllDependentChildAttributeModels", client => client.GetAllDependentChildAttributeModels(modelContext as AttributeModelContext, FillDiagnosticTraces(callerContext)), MDMTraceSource.AttributeModelGet);
        }

        #endregion

        #region Dependency Attribute CUD

        /// <summary>
        /// Create attribute dependency
        /// </summary>
        /// <param name="attributeId">AttributeId for which dependency is to be created</param>
        /// <param name="dependentAttribute">Attribute dependency to create</param>
        /// <param name="callerContext">Context which called the application</param>
        /// <returns>Result of the operation</returns>
        public IOperationResult CreateAttributeDependency(Int32 attributeId, IDependentAttribute dependentAttribute, ICallerContext callerContext)
        {
            return this.MakeServiceCall<IOperationResult>("CreateAttributeDependency", "CreateAttributeDependency", client => client.CreateAttributeDependency(attributeId, dependentAttribute as DependentAttribute, FillDiagnosticTraces(callerContext)));
        }

        /// <summary>
        /// Update attribute dependency
        /// </summary>
        /// <param name="attributeId">AttributeId for which dependency is to be created</param>
        /// <param name="dependentAttribute">Attribute dependency to create</param>
        /// <param name="callerContext">Context which called the application</param>
        /// <returns>Result of the operation</returns>
        public IOperationResult UpdateAttributeDependency(Int32 attributeId, IDependentAttribute dependentAttribute, ICallerContext callerContext)
        {
            return this.MakeServiceCall<IOperationResult>("UpdateAttributeDependency", "UpdateAttributeDependency", client => client.UpdateAttributeDependency(attributeId, dependentAttribute as DependentAttribute, FillDiagnosticTraces(callerContext)));
        }


        /// <summary>
        /// Delete attribute dependency
        /// </summary>
        /// <param name="attributeId">AttributeId for which dependency is to be created</param>
        /// <param name="dependentAttribute">Attribute dependency to create</param>
        /// <param name="callerContext">Context which called the application</param>
        /// <returns>Result of the operation</returns>
        public IOperationResult DeleteAttributeDependency(Int32 attributeId, IDependentAttribute dependentAttribute, ICallerContext callerContext)
        {
            return this.MakeServiceCall<IOperationResult>("DeleteAttributeDependency", "DeleteAttributeDependency", client => client.DeleteAttributeDependency(attributeId, dependentAttribute as DependentAttribute, FillDiagnosticTraces(callerContext)));
        }


        /// <summary>
        /// Create - Update or Delete given attribute dependency
        /// </summary>
        /// <param name="attributeId">AttributeId for which dependency is to be processed</param>
        /// <param name="dependentAttributes">DependentAttribute collection to process</param>
        /// <param name="callerContext">Context which called the application</param>
        /// <returns>Result of the operation</returns>
        public IOperationResultCollection ProcessAttributeDependencies(Int32 attributeId, IDependentAttributeCollection dependentAttributes, ICallerContext callerContext)
        {
            return this.MakeServiceCall<IOperationResultCollection>("ProcessAttributeDependencies", "ProcessAttributeDependencies", client => client.ProcessAttributeDependencies(attributeId, dependentAttributes as DependentAttributeCollection, FillDiagnosticTraces(callerContext)));
        }

        /// <summary>
        /// Create - Update or Delete attribute dependent data
        /// </summary>
        /// <param name="dependentAttributeDataMaps">DependentAttribute Data map collection to process</param>
        /// <param name="callerContext">Context which called the application</param>
        /// <returns>Result of the operation</returns>
        public IOperationResultCollection ProcessDependentData(IDependentAttributeDataMapCollection dependentAttributeDataMaps, ICallerContext callerContext)
        {
            return this.MakeServiceCall<IOperationResultCollection>("ProcessDependentData", "ProcessDependentData", client => client.ProcessDependentData(dependentAttributeDataMaps as DependentAttributeDataMapCollection, FillDiagnosticTraces(callerContext)));
        }

        /// <summary>
        /// Create attribute dependent data
        /// </summary>
        /// <param name="dependentAttributeDataMap">Indicates dependent attribute data map to create</param>
        /// <param name="callerContext">Indicates context which called the application</param>
        /// <returns>Returns result of the operation</returns>
        public IOperationResult CreateDependentData(IDependentAttributeDataMap dependentAttributeDataMap, ICallerContext callerContext)
        {
            return this.MakeServiceCall<IOperationResult>("CreateDependentData", "CreateDependentData", client => client.CreateDependentData(dependentAttributeDataMap as DependentAttributeDataMap, FillDiagnosticTraces(callerContext)));
        }

        /// <summary>
        /// Update attribute dependent data
        /// </summary>
        /// <param name="dependentAttributeDataMap">DependentAttribute Data map to update</param>
        /// <param name="callerContext">Context which called the application</param>
        /// <returns>Result of the operation</returns>
        public IOperationResult UpdateDependentData(IDependentAttributeDataMap dependentAttributeDataMap, ICallerContext callerContext)
        {
            return this.MakeServiceCall<IOperationResult>("UpdateDependentData", "UpdateDependentData", client => client.UpdateDependentData(dependentAttributeDataMap as DependentAttributeDataMap, FillDiagnosticTraces(callerContext)));
        }

        /// <summary>
        /// Delete attribute dependent data
        /// </summary>
        /// <param name="dependentAttributeDataMap">DependentAttribute Data map to delete</param>
        /// <param name="callerContext">Context which called the application</param>
        /// <returns>Result of the operation</returns>
        public IOperationResult DeleteDependentData(IDependentAttributeDataMap dependentAttributeDataMap, ICallerContext callerContext)
        {
            return this.MakeServiceCall<IOperationResult>("DeleteDependentData", "DeleteDependentData", client => client.DeleteDependentData(dependentAttributeDataMap as DependentAttributeDataMap, FillDiagnosticTraces(callerContext)));
        }

        /// <summary>
        /// Process attribute dependent data for 1 dependency
        /// </summary>
        /// <param name="dependentAttributeDataMap">DependentAttribute Data map to process</param>
        /// <param name="callerContext">Context which called the application</param>
        /// <returns>Result of the operation</returns>
        public IOperationResult ProcessDependentData(IDependentAttributeDataMap dependentAttributeDataMap, ICallerContext callerContext)
        {
            return this.MakeServiceCall<IOperationResult>("ProcessDependentData", "ProcessDependentData", client => client.ProcessDependentData(dependentAttributeDataMap as DependentAttributeDataMap, FillDiagnosticTraces(callerContext)));
        }
        #endregion Dependency Attribute CUD

        #region Attribute Group

        /// <summary>
        /// Get attribute group name collection based on provided name and context.
        /// </summary>
        /// <param name="attributeGroupShortName">Short Name of the attribute group</param>
        /// <param name="iAttributeModelContext">Attribute model context defines context from which we need to get attribute group</param>
        /// <param name="iCallerContext">Caller context to denote application and module information</param>
        /// <returns>Returns collection of attribute group with all details.</returns>
        public Collection<AttributeGroup> GetAttributeGroupByName(String attributeGroupShortName, IAttributeModelContext iAttributeModelContext, ICallerContext iCallerContext)
        {
            return MakeServiceCall("GetAttributeGroupByName", "GetAttributeGroupByName",
                  client =>
                  client.GetAttributeGroupsByName(attributeGroupShortName, iAttributeModelContext as AttributeModelContext, FillDiagnosticTraces(iCallerContext)));
        }

        #endregion

        #region Entity Type Attribute Mapping

        #region Get

        /// <summary>
        /// Gets all EntityType Attribute Mappings from the system
        /// </summary>
        /// <param name="iCallerContext">Context which called the application</param>
        /// <returns>All EntityType Attribute Mappings</returns>
        public IEntityTypeAttributeMappingCollection GetAllEntityTypeAttributeMapping(ICallerContext iCallerContext)
        {
            return MakeServiceCall<IEntityTypeAttributeMappingCollection>("GetAllEntityTypeAttributeMapping", "GetAllEntityTypeAttributeMapping",
                                         client =>
                                             client.GetAllEntityTypeAttributeMapping(FillDiagnosticTraces(iCallerContext)));
        }

        /// <summary>
        /// Gets EntityType Attribute Mappings from the system based on given EntityType Id
        /// </summary>
        /// <param name="entityTypeId">Indicates the EntityType Id for which attribute mappings needs to be fetched</param>
        /// <param name="iCallerContext">Context which called the application</param>
        /// <returns>EntityType Attribute Mappings for a specified EntityTypeId</returns>
        public IEntityTypeAttributeMappingCollection GetEntityTypeAttributeMappingsByEntityTypeId(Int32 entityTypeId, ICallerContext iCallerContext)
        {
            return MakeServiceCall<IEntityTypeAttributeMappingCollection>("GetEntityTypeAttributeMappingsByEntityTypeId", "GetEntityTypeAttributeMappingsByEntityTypeId",
                                         client =>
                                             client.GetEntityTypeAttributeMappingsByEntityTypeId(entityTypeId, FillDiagnosticTraces(iCallerContext)));
        }

        /// <summary>
        /// Gets EntityType Attribute Mappings from the system based on given Attribute Id
        /// </summary>
        /// <param name="attributeId">Indicates the Attribute Id for which attribute mappings needs to be fetched</param>
        /// <param name="iCallerContext">Context which called the application</param>
        /// <returns>EntityType Attribute Mappings for a specified AttributeId</returns>
        public IEntityTypeAttributeMappingCollection GetEntityTypeAttributeMappingsByAttributeId(Int32 attributeId, ICallerContext iCallerContext)
        {
            return MakeServiceCall<IEntityTypeAttributeMappingCollection>("GetEntityTypeAttributeMappingsByAttributeId", "GetEntityTypeAttributeMappingsByAttributeId",
                                         client =>
                                                  client.GetEntityTypeAttributeMappingsByAttributeId(attributeId, FillDiagnosticTraces(iCallerContext)));
        }

        /// <summary>
        /// Gets EntityType Attribute Mappings from the system based on EntityTypeId and AttributeGroupId
        /// </summary>
        /// <param name="entityTypeId">Indicates the EntityType Id for which attribute mappings needs to be fetched</param>
        /// <param name="attributeGroupId">Indicates the AttributeGroup Id for which attribute mappings needs to be fetched</param>
        /// <param name="iCallerContext">Context which called the application</param>
        /// <returns>EntityType Attribute Mappings for a specified EntityTypeId and AttributeGroupId</returns>
        public IEntityTypeAttributeMappingCollection GetEntityTypeAttributeMappings(Int32 entityTypeId, Int32 attributeGroupId, ICallerContext iCallerContext)
        {
            return MakeServiceCall<IEntityTypeAttributeMappingCollection>("GetEntityTypeAttributeMappings", "GetEntityTypeAttributeMappings",
                                         client =>
                                                      client.GetEntityTypeAttributeMappings(entityTypeId, attributeGroupId, FillDiagnosticTraces(iCallerContext)));
        }

        #endregion Get

        /// <summary>
        /// Create, Update or Delete EntityType Attribute Mapping 
        /// </summary>
        /// <param name="iEntityTypeAttributeMappings">EntityTypeAttributeMappings to process</param>
        /// <param name="iCallerContext">Context which called the application</param>
        /// <returns>Result of the operation</returns>
        public IOperationResult ProcessEntityTypeAttributeMappings(IEntityTypeAttributeMappingCollection iEntityTypeAttributeMappings, ICallerContext iCallerContext)
        {
            return MakeServiceCall<IOperationResult>("ProcessEntityTypeAttributeMapping", "ProcessEntityTypeAttributeMapping",
                                         client =>
                                                      client.ProcessEntityTypeAttributeMapping(iEntityTypeAttributeMappings as EntityTypeAttributeMappingCollection, FillDiagnosticTraces(iCallerContext)));
        }

        #endregion Entity Type Attribute Mapping

        #region Container Entity Type Attribute Mapping

        /// <summary>
        /// Gets Container Entity Type Attribute Mappings from the system based on container Id, entityType Id , attributeGroup Id and attribute Id
        /// </summary>
        /// <param name="containerId">Indicates the container Id for which attribute mappings needs to be fetched</param>
        /// <param name="entityTypeId">Indicates the EntityType Id for which attribute mappings needs to be fetched</param>
        /// <param name="attributeGroupId">Indicates the container Id for which attribute mappings needs to be fetched</param>
        /// <param name="attributeId">Indicates the attribute Id for which attribute mappings needs to be fetched</param>
        /// <param name="iCallerContext">Context which called the application</param>
        /// <returns>Container EntityType Attribute Mappings for a specified ContainerId, EntityTypeId, AttributeId and AttributeGroupId</returns>
        public IContainerEntityTypeAttributeMappingCollection GetContainerEntityTypeAttributeMappings(Int32 containerId, Int32 entityTypeId, Int32 attributeGroupId, Int32 attributeId, ICallerContext iCallerContext)
        {
            return MakeServiceCall<IContainerEntityTypeAttributeMappingCollection>("GetContainerEntityTypeAttributeMappings", "GetContainerEntityTypeAttributeMappings",
                                         client =>
                                                      client.GetContainerEntityTypeAttributeMappings(containerId, entityTypeId, attributeGroupId, attributeId, FillDiagnosticTraces(iCallerContext)));
        }

        /// <summary>
        /// Create, Update or delete Container EntityType Attribute Mappings
        /// </summary>
        /// <param name="iContainerEntityTypeAttributeMappings">ContainerEntityTypeAttributeMappings to process</param>
        /// <param name="iCallerContext">Context which called the application</param>
        /// <returns>Result of the operation</returns>
        public IOperationResult ProcessContainerEntityTypeAttributeMappings(IContainerEntityTypeAttributeMappingCollection iContainerEntityTypeAttributeMappings, ICallerContext iCallerContext)
        {
            return MakeServiceCall<IOperationResult>("ProcessContainerEntityTypeAttributeMappings", "ProcessContainerEntityTypeAttributeMappings",
                                         client =>
                                                      client.ProcessContainerEntityTypeAttributeMappings(iContainerEntityTypeAttributeMappings as ContainerEntityTypeAttributeMappingCollection, FillDiagnosticTraces(iCallerContext)));
        }

        #endregion Container Entity Type Attribute Mapping

        #region Container EntityType Mapping

        /// <summary>
        /// Gets all Container EntityType mappings from the system
        /// </summary>
        /// <param name="iCallerContext">Context which called the application</param>
        /// <returns>All Container Entity type mappings</returns>
        public IContainerEntityTypeMappingCollection GetAllContainerEntityTypeMappings(ICallerContext iCallerContext)
        {
            return MakeServiceCall<IContainerEntityTypeMappingCollection>("GetAllContainerEntityTypeMappings", "GetAllContainerEntityTypeMappings",
                                         client =>
                                                      client.GetAllContainerEntityTypeMappings(FillDiagnosticTraces(iCallerContext)));
        }

        /// <summary>
        /// Gets Container EntityType mappings from the system based on containerId
        /// </summary>
        /// <param name="containerId">Indicates the container Id for which Container EntityType mappings needs to be fetched</param>
        /// <param name="iCallerContext">Context which called the application</param>
        /// <returns>Container EntityType mappings for a specified container Id</returns>
        public IContainerEntityTypeMappingCollection GetContainerEntityTypeMappingsByContainerId(Int32 containerId, ICallerContext iCallerContext)
        {
            return MakeServiceCall<IContainerEntityTypeMappingCollection>("GetContainerEntityTypeMappingsByContainerId", "GetContainerEntityTypeMappingsByContainerId",
                                         client =>
                                                      client.GetContainerEntityTypeMappingsByContainerId(containerId, FillDiagnosticTraces(iCallerContext)));
        }

        /// <summary>
        /// Gets Container EntityType mappings from the system based on EntityTypeId
        /// </summary>
        /// <param name="entityTypeId">Indicates the EntityType Id for which Container EntityType mappings needs to be fetched</param>
        /// <param name="iCallerContext">Context which called the application</param>
        /// <returns>Container EntityType mappings for a specified EntityType Id</returns>
        public IContainerEntityTypeMappingCollection GetContainerEntityTypeMappingsByEntityTypeId(Int32 entityTypeId, ICallerContext iCallerContext)
        {
            return MakeServiceCall<IContainerEntityTypeMappingCollection>("GetContainerEntityTypeMappingsByEntityTypeId", "GetContainerEntityTypeMappingsByEntityTypeId",
                                         client =>
                                                      client.GetContainerEntityTypeMappingsByEntityTypeId(entityTypeId, FillDiagnosticTraces(iCallerContext)));
        }

        /// <summary>
        /// Gets mapped EntityTypes from the system based on containerId
        /// </summary>
        /// <param name="containerId">Indicates the container Id for which mapped EntityTypes needs to be fetched</param>
        /// <param name="iCallerContext">Context which called the application</param>
        /// <returns>Mapped EntityTypes for a specified container Id</returns>
        public IEntityTypeCollection GetMappedEntityTypesWithContainer(Int32 containerId, ICallerContext iCallerContext)
        {
            return MakeServiceCall<IEntityTypeCollection>("GetMappedEntityTypesWithContainer", "GetMappedEntityTypesWithContainer",
                                         client =>
                                                      client.GetMappedEntityTypesWithContainer(containerId, FillDiagnosticTraces(iCallerContext)));
        }

        /// <summary>
        /// Create, Update or Delete Container EntityType Mappings
        /// </summary>
        /// <param name="iContainerEntityTypeMappings">ContainerEntityTypeMappings to process</param>
        /// <param name="iCallerContext">Context which called the application</param>
        /// <returns>Result of the operation</returns>
        public IOperationResult ProcessContainerEntityTypeMappings(IContainerEntityTypeMappingCollection iContainerEntityTypeMappings, ICallerContext iCallerContext)
        {
            return MakeServiceCall<IOperationResult>("ProcessContainerEntityTypeMappings", "ProcessContainerEntityTypeMappings",
                                         client =>
                                                      client.ProcessContainerEntityTypeMappings(iContainerEntityTypeMappings as ContainerEntityTypeMappingCollection, FillDiagnosticTraces(iCallerContext)));
        }

        #endregion Container EntityType Mapping

        #region Container RelationshipType Attribute Mapping

        /// <summary>
        /// Gets Container RelationshipType Attribute mappings from the system based on containerId, relationshipId and attributeGroupId
        /// </summary>
        /// <param name="containerId">Indicates the container Id for which mappings needs to be fetched</param>
        /// <param name="relationshipTypeId">Indicates the RelationshipType Id for which mappings needs to be fetched</param>
        /// <param name="attributeGroupId">Indicates the AttributeGroup Id for which mappings needs to be fetched</param>
        /// <param name="iCallerContext">Context which called the application</param>
        /// <returns>Container RelationshipType Attribute mappings for specified containerId, relationshipTypeId and attributeGroupId</returns>
        public IContainerRelationshipTypeAttributeMappingCollection GetContainerRelationshipTypeAttributeMappings(Int32 containerId, Int32 relationshipTypeId, Int32 attributeGroupId, ICallerContext iCallerContext)
        {

            return MakeServiceCall<IContainerRelationshipTypeAttributeMappingCollection>("GetContainerRelationshipTypeAttributeMappings", "GetContainerRelationshipTypeAttributeMappings",
                                         client =>
                                                      client.GetContainerRelationshipTypeAttributeMappings(containerId, relationshipTypeId, attributeGroupId, FillDiagnosticTraces(iCallerContext)));
        }

        /// <summary>
        /// Create, Update or Delete Container RelationshipType Attribute Mappings
        /// </summary>
        /// <param name="iContainerRelationshipTypeAttributeMappings">Container RelationshipType Attribute Mappings to process</param>
        /// <param name="iCallerContext">Context which called the application</param>
        /// <returns>Result of the operation</returns>
        public IOperationResult ProcessContainerRelationshipTypeAttributeMappings(IContainerRelationshipTypeAttributeMappingCollection iContainerRelationshipTypeAttributeMappings, ICallerContext iCallerContext)
        {

            return MakeServiceCall<IOperationResult>("ProcessContainerRelationshipTypeAttributeMappings", "ProcessContainerRelationshipTypeAttributeMappings",
                                         client =>
                                                      client.ProcessContainerRelationshipTypeAttributeMappings(iContainerRelationshipTypeAttributeMappings as ContainerRelationshipTypeAttributeMappingCollection, FillDiagnosticTraces(iCallerContext)));
        }

        #endregion Container RelationshipType Attribute Mapping

        #region Container RelationshipType EntityType Mapping

        /// <summary>
        /// Gets all container RelationshipType EntityType mappings from the system
        /// </summary>
        /// <param name="iCallerContext">Context which called the application</param>
        /// <returns>All container RelationshipType EntityType mappings</returns>
        public IContainerRelationshipTypeEntityTypeMappingCollection GetAllContainerRelationshipTypeEntityTypeMappings(ICallerContext iCallerContext)
        {
            return MakeServiceCall<IContainerRelationshipTypeEntityTypeMappingCollection>("GetAllContainerRelationshipTypeEntityTypeMappings", "GetAllContainerRelationshipTypeEntityTypeMappings",
                                         client =>
                                                      client.GetAllContainerRelationshipTypeEntityTypeMappings(FillDiagnosticTraces(iCallerContext)));
        }


        /// <summary>
        /// Gets container RelationshipType EntityType mappings based on containerId and relationshipId
        /// </summary>
        /// <param name="containerId">Indicates the container Id for which mappings needs to be fetched</param>
        /// <param name="relationshipTypeId">Indicates the relationship type Id for which mappings needs to be fetched</param>
        /// <param name="iCallerContext">Context which called the application</param>
        /// <returns>Container RelationshipType EntityType mappings  for specified containerId and RelationshipTypeId</returns>
        public IContainerRelationshipTypeEntityTypeMappingCollection GetContainerRelationshipTypeEntityTypeMappings(Int32 containerId, Int32 relationshipTypeId, ICallerContext iCallerContext)
        {
            return MakeServiceCall<IContainerRelationshipTypeEntityTypeMappingCollection>("GetContainerRelationshipTypeEntityTypeMappings", "GetContainerRelationshipTypeEntityTypeMappings",
                                         client =>
                                                      client.GetContainerRelationshipTypeEntityTypeMappings(containerId, relationshipTypeId, FillDiagnosticTraces(iCallerContext)));
        }

        /// <summary>
        /// Gets mapped EntityTypes based on ContainerId and RelationshipTypeId
        /// </summary>
        /// <param name="containerId">Indicates the container Id for which mapped EntityTypes needs to be fetched</param>
        /// <param name="relationshipTypeId">Indicates the relationshipType Id for which mapped EntityTypes needs to be fetched</param>
        /// <param name="iCallerContext">Context which called the application</param>
        /// <returns>Mapped EntityTypes for specified containerId and relationshipTypeId</returns>
        public IEntityTypeCollection GetMappedEntityTypesWithContainerAndRelationshipType(Int32 containerId, Int32 relationshipTypeId, ICallerContext iCallerContext)
        {
            return MakeServiceCall<IEntityTypeCollection>("GetMappedEntityTypesWithContainerAndRelationshipType", "GetMappedEntityTypesWithContainerAndRelationshipType",
                                         client =>
                                                      client.GetMappedEntityTypesWithContainerAndRelationshipType(containerId, relationshipTypeId, FillDiagnosticTraces(iCallerContext)));
        }

        /// <summary>
        /// Create, Update or Delete Container RelationshipType EntityType Mappings
        /// </summary>
        /// <param name="iContainerRelationshipTypeEntityTypeMappingCollection">Container RelationshipType EntityType Mappings to process</param>
        /// <param name="iCallerContext">Context which called the application</param>
        /// <returns>Result of the operation</returns>
        public IOperationResult ProcessContainerRelationshipTypeEntityTypeMappings(IContainerRelationshipTypeEntityTypeMappingCollection iContainerRelationshipTypeEntityTypeMappingCollection, ICallerContext iCallerContext)
        {
            return MakeServiceCall<IOperationResult>("ProcessContainerRelationshipTypeEntityTypeMappings", "ProcessContainerRelationshipTypeEntityTypeMappings",
                                         client =>
                                                      client.ProcessContainerRelationshipTypeEntityTypeMappings(iContainerRelationshipTypeEntityTypeMappingCollection as ContainerRelationshipTypeEntityTypeMappingCollection, FillDiagnosticTraces(iCallerContext)));
        }

        #endregion  Container RelationshipType EntityType Mapping

        #region RelationshipType Attribute Mapping

        /// <summary>
        /// Gets RelationshipType attribute mappings from the system based on relationshpiypeId and attributeGroupId
        /// </summary>
        /// <param name="relationshipTypeId">Indicates the RelationshipType Id for which mappings needs to be fetched</param>
        /// <param name="attributeGroupId">Indicates the AttributeGroup Id for which mappings needs to be fetched</param>
        /// <param name="iCallerContext">Context which called the application</param>
        /// <returns>RelationshipType attribute mappings for specified relationshipType Id and attributeGroup Id</returns>
        public IRelationshipTypeAttributeMappingCollection GetRelationshipTypeAttributeMappings(Int32 relationshipTypeId, Int32 attributeGroupId, ICallerContext iCallerContext)
        {
            return MakeServiceCall<IRelationshipTypeAttributeMappingCollection>("GetRelationshipTypeAttributeMappings", "GetRelationshipTypeAttributeMappings",
                             client =>
                                          client.GetRelationshipTypeAttributeMappings(relationshipTypeId, attributeGroupId, FillDiagnosticTraces(iCallerContext)));
        }

        /// <summary>
        /// Create, Update or Delete RelationshipType attribute mappings
        /// </summary>
        /// <param name="iRelationshipTypeAttributeMappings">RelationshipType attribute mappings to process</param>
        /// <param name="iCallerContext">Context which called the application</param>
        /// <returns>Result of the operation</returns>
        public IOperationResult ProcessRelationshipTypeAttributeMappings(IRelationshipTypeAttributeMappingCollection iRelationshipTypeAttributeMappings, ICallerContext iCallerContext)
        {
            return MakeServiceCall<IOperationResult>("ProcessRelationshipTypeAttributeMappings", "ProcessRelationshipTypeAttributeMappings",
                             client =>
                                          client.ProcessRelationshipTypeAttributeMappings(iRelationshipTypeAttributeMappings as RelationshipTypeAttributeMappingCollection, FillDiagnosticTraces(iCallerContext)));
        }

        #endregion RelationshipType Attribute Mapping

        #region RelationshipType EntityType Mapping

        /// <summary>
        /// Gets all RelationshipType EntityType mappings from the system
        /// </summary>
        /// <param name="iCallerContext">Context which called the application</param>
        /// <returns>All RelationshipType EntityType mappings</returns>
        public IRelationshipTypeEntityTypeMappingCollection GetAllRelationshipTypeEntityTypeMappings(ICallerContext iCallerContext)
        {
            return MakeServiceCall<IRelationshipTypeEntityTypeMappingCollection>("GetAllRelationshipTypeEntityTypeMappings", "GetAllRelationshipTypeEntityTypeMappings",
                             client =>
                                          client.GetAllRelationshipTypeEntityTypeMappings(FillDiagnosticTraces(iCallerContext)));
        }

        /// <summary>
        /// Gets RelationshipType EntityType mappings from the system based on RelationshipType Id
        /// </summary>
        /// <param name="relationshipTypeId">Indicates the RelationshipType Id for which mappings needs to be fetched</param>
        /// <param name="iCallerContext">Context which called the application</param>
        /// <returns>RelationshipType EntityType mappings for specified RelationshipType Id</returns>
        public IRelationshipTypeEntityTypeMappingCollection GetRelationshipTypeEntityTypeMappingsByRelationshipTypeId(Int32 relationshipTypeId, ICallerContext iCallerContext)
        {
            return MakeServiceCall<IRelationshipTypeEntityTypeMappingCollection>("GetRelationshipTypeEntityTypeMappingsByRelationshipTypeId", "GetRelationshipTypeEntityTypeMappingsByRelationshipTypeId",
                               client =>
                                            client.GetRelationshipTypeEntityTypeMappingsByRelationshipTypeId(relationshipTypeId, FillDiagnosticTraces(iCallerContext)));
        }

        /// <summary>
        /// Gets RelationshipType EntityType mappings from the system from the system based on EntityType Id
        /// </summary>
        /// <param name="entityTypeId">Indicates the EntityType Id for which mappings needs to be fetched</param>
        /// <param name="iCallerContext">Context which called the application</param>
        /// <returns>RelationshipType EntityType mappings for specified EntityType Id</returns>
        public IRelationshipTypeEntityTypeMappingCollection GetRelationshipTypeEntityTypeMappingsByEntityTypeId(Int32 entityTypeId, ICallerContext iCallerContext)
        {
            return MakeServiceCall<IRelationshipTypeEntityTypeMappingCollection>("GetRelationshipTypeEntityTypeMappingsByEntityTypeId", "GetRelationshipTypeEntityTypeMappingsByEntityTypeId",
                                client =>
                                             client.GetRelationshipTypeEntityTypeMappingsByEntityTypeId(entityTypeId, FillDiagnosticTraces(iCallerContext)));
        }

        /// <summary>
        /// Gets mapped EntityTypes from the system based on RelationshipType Id
        /// </summary>
        /// <param name="relationshipTypeId">Indicates the RelationshipType Id for which mapped EntityTypes needs to be fetched</param>
        /// <param name="iCallerContext">Context which called the application</param>
        /// <returns>Mapped EntityTypes for specified RelationshipType Id</returns>
        public IEntityTypeCollection GetMappedEntityTypesWithRelationshipType(Int32 relationshipTypeId, ICallerContext iCallerContext)
        {
            return MakeServiceCall<IEntityTypeCollection>("GetMappedEntityTypesWithRelationshipType", "GetMappedEntityTypesWithRelationshipType",
                                client =>
                                             client.GetMappedEntityTypesWithRelationshipType(relationshipTypeId, FillDiagnosticTraces(iCallerContext)));
        }

        /// <summary>
        /// Create, Update or Delete RelationshipType EntityType mappings
        /// </summary>
        /// <param name="iRelationshipTypeEntityTypeMappings">RelationshipType EntityType mappings to process</param>
        /// <param name="iCallerContext">Context which called the application</param>
        /// <returns>Result of the operation</returns>
        public IOperationResult ProcessRelationshipTypeEntityTypeMappings(IRelationshipTypeEntityTypeMappingCollection iRelationshipTypeEntityTypeMappings, ICallerContext iCallerContext)
        {
            return MakeServiceCall("ProcessRelationshipTypeEntityTypeMappings", "ProcessRelationshipTypeEntityTypeMappings",
                                client =>
                                             client.ProcessRelationshipTypeEntityTypeMappings(iRelationshipTypeEntityTypeMappings as RelationshipTypeEntityTypeMappingCollection, FillDiagnosticTraces(iCallerContext)));
        }

        #endregion RelationshipType EntityType Mapping

        #region Container RelationshipType EntityType Mapping Cardinalities

        /// <summary>
        /// Get Container RelationshipType EntityType MappingCardinalities
        /// </summary>
        /// <param name="containerId">Indicates the container id</param>
        /// <param name="fromEntityTypeId">Indicates the from entitytype id</param>
        /// <param name="relationshipTypeId">Indicates the from relationshiptype id</param>
        /// <param name="callerContext">Context which called the application</param>
        /// <returns>Collection of ContainerEntityTypeRelationshipTypeCardinalities</returns>
        public IContainerRelationshipTypeEntityTypeMappingCardinalityCollection GetContainerRelationshipTypeEntityTypeMappingCardinalities(Int32 containerId, Int32 fromEntityTypeId, Int32 relationshipTypeId, ICallerContext callerContext)
        {
            return MakeServiceCall("GetContainerRelationshipTypeEntityTypeMappingCardinalities", "GetContainerRelationshipTypeEntityTypeMappingCardinalities",
                               client => client.GetContainerRelationshipTypeEntityTypeMappingCardinalities(containerId, fromEntityTypeId, relationshipTypeId, FillDiagnosticTraces(callerContext)));
        }

        /// <summary>
        /// Create, Update or Delete Container RelationshipType EntityType MappingCardinalities
        /// </summary>
        /// <param name="containerRelationshipTypeEntityTypeMappingCardinalities">Container RelationshipType EntityType MappingCardinalities to process</param>
        /// <param name="callerContext">Context which called the application</param>
        /// <returns>Result of the operation</returns>
        public IOperationResultCollection ProcessContainerRelationshipTypeEntityTypeMappingCardinalities(IContainerRelationshipTypeEntityTypeMappingCardinalityCollection containerRelationshipTypeEntityTypeMappingCardinalities, ICallerContext callerContext)
        {
            return MakeServiceCall("ProcessContainerRelationshipTypeEntityTypeMappingCardinalities", "ProcessContainerRelationshipTypeEntityTypeMappingCardinalities",
                               client => client.ProcessContainerRelationshipTypeEntityTypeMappingCardinalities(containerRelationshipTypeEntityTypeMappingCardinalities as ContainerRelationshipTypeEntityTypeMappingCardinalityCollection, FillDiagnosticTraces(callerContext)));
        }

        #endregion Container RelationshipType EntityType Mapping Cardinalities

        #region Category Get/Search

        /// <summary>
        /// Gets the collection of categories for the specified hierarchy Id in a locale provided by user
        /// </summary>       
        /// <param name="hierarchyId">Indicates the hierarchy Id in which categories are requested</param>
        /// <param name="locale">Indicates the locale in which category is needed</param>
        /// <param name="iCallerContext">Indicates the caller context, which contains the application and module that has invoked the API</param>
        /// <returns>Returns collection of category</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        /// <example>
        /// <code language="c#" title="Sample: Get categories by hierarchy Id" source="..\MDM.APISamples\DataModelManager\Category\GetCategoryById.cs" region="GetAllCategories" />
        /// </example>
        public ICategoryCollection GetAllCategories(Int32 hierarchyId, LocaleEnum locale, ICallerContext iCallerContext)
        {
            return MakeServiceCall<ICategoryCollection>("GetAllCategories", "GetAllCategories",
                                                  client =>
                                                      client.GetAllCategories(hierarchyId, locale, FillDiagnosticTraces(iCallerContext)));

        }

        /// <summary>
        /// Gets the collection of categories for the specified container name in a locale provided by the user
        /// </summary>
        /// <param name="containerName">Indicates the container name in which categories are requested</param>
        /// <param name="locale">Indicates the locale in which category is needed</param>
        /// <param name="iCallerContext">Indicates the caller context, which contains the application and module that has invoked the API</param>
        /// <returns>Returns a collection of categories</returns>  
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        /// <example>
        /// <code language="c#" title="Get categories by container name" source="..\MDM.APISamples\DataModelManager\Category\GetCategoryByName.cs" region="GetAllCategories" />
        /// </example>

        public ICategoryCollection GetAllCategories(String containerName, LocaleEnum locale, ICallerContext iCallerContext)
        {
            return MakeServiceCall<ICategoryCollection>("GetAllCategories", "GetAllCategoriesUsingContainerName",
                                                   client =>
                                                       client.GetAllCategoriesUsingContainerName(containerName, locale, FillDiagnosticTraces(iCallerContext)));
        }

        /// <summary>
        /// Gets the category details based on requested hierarchy Id and category Id
        /// </summary>
        /// <example>
        /// <code>
        /// 
        /// //Create an instance of data model service
        /// DataModelService mdmDataModelService = GetMDMDataModelService();
        /// 
        /// // Key Note: CallerContext has properties Application and Module which are mandatory to be set
        /// // Indicates name of application and module
        /// ICallerContext callerContext = MDMObjectFactory.GetICallerContext();
        /// callerContext.Application = MDMCenterApplication.PIM;
        /// callerContext.Module = MDMCenterModules.Entity;
        ///    
        /// //Get new instance of ICategory using MDMObjectFactory
        /// ICategory iCategory = MDMObjectFactory.GetICategory();
        /// 
        /// // Assumed hard coded variables for API Sample purpose..
        /// Int32 hierarchyId = 1; //ProductHierarchy as per River works Model
        /// Int64 categoryId = 2392; //Toys as per RiverWorks Model
        ///
        /// //Make a WCF call to DataModelService.GetCategoryById, It will return category details for requested Id
        /// iCategory = mdmDataModelService.GetCategoryById(hierarchyId, categoryId, callerContext);
        /// 
        /// </code>
        /// </example>
        /// <param name="hierarchyId">Indicates the hierarchy Id in which category is requested</param>
        /// <param name="categoryId">Indicates the Id of the category</param>
        /// <param name="iCallerContext">Indicates the caller context, which contains the application and module that has invoked the API</param>
        /// <returns>Returns the category details based on requested hierachy Id and Category Id</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        public ICategory GetCategoryById(Int32 hierarchyId, Int64 categoryId, ICallerContext iCallerContext)
        {
            return MakeServiceCall<ICategory>("GetCategoryById", "GetCategoryById",
                                                  client =>
                                                      client.GetCategoryById(hierarchyId, categoryId, FillDiagnosticTraces(iCallerContext)));
        }

        /// <summary>
        /// Gets the collection of categories for the specified hierarchy and category mapping details
        /// </summary>
        /// <param name="mappingCollection">Indicates a mapping between category and  hierarchy identifiers</param>
        /// <param name="callerContext">Indicates the caller context, which contains the application and module that has invoked the API</param>
        /// <returns>Returns the collection of categories based on hierachy category mapping details</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDMCenter operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        /// <example>
        /// <code language="c#" title="Get Categories By Ids"  source="..\..\Documentation\MDM.APISamples\DataModelManager\Category\GetCategoryById.cs" region="Get Categories By Ids"/>
        /// </example>
        public ICategoryCollection GetCategoriesByIds(IHierachyCategoryMappingCollection mappingCollection, ICallerContext callerContext)
        {
            return MakeServiceCall<ICategoryCollection>("GetCategoriesByIds", "GetCategoriesByIds",
                                                  client =>
                                                      client.GetCategoriesByIds(mappingCollection as HierachyCategoryMappingCollection, FillDiagnosticTraces(callerContext)));
        }

        /// <summary>
        /// Gets the category for the specified hierarchy Id and category name
        /// </summary>
        /// <example>
        /// <code language="c#" title="Get Category By Name"  source="..\..\Documentation\MDM.APISamples\DataModelManager\Category\GetCategoryByName.cs" region="Get Category By Name"/>
        /// </example>
        /// <param name="hierarchyId">Indicates the hierarchy Id in which category is requested</param>
        /// <param name="categoryName">Indicates the name of the category</param>
        /// <param name="iCallerContext">Indicates the caller context, which contains the application and module that has invoked the API</param>
        /// <returns>Returns category information based on the specified hierarchy Id and category name</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        public ICategory GetCategoryByName(Int32 hierarchyId, String categoryName, ICallerContext iCallerContext)
        {
            return MakeServiceCall<ICategory>("GetCategoryByName", "GetCategoryByName",
                                                  client =>
                                                      client.GetCategoryByName(hierarchyId, categoryName, FillDiagnosticTraces(iCallerContext)));
        }

        /// <summary>
        /// Gets the category for the specified container name and category name
        /// </summary>
        /// <param name="containerName">Indicates the container name in which category is requested</param>
        /// <param name="categoryName">Indicates the name of the category</param>
        /// <param name="iCallerContext">Indicates the caller context, which contains the application and module that has invoked the API</param>
        /// <returns>Returns category based on the container name and category name</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        /// <example>
        /// <code language="c#" title="Get category by category name" source="..\MDM.APISamples\DataModelManager\Category\GetCategoryByName.cs" region="GetCategoryByNames" />
        /// </example>

        public ICategory GetCategoryByName(String containerName, String categoryName, ICallerContext iCallerContext)
        {
            return MakeServiceCall<ICategory>("GetCategoryByName", "GetCategoryByNameUsingContainerName",
                                                  client =>
                                                      client.GetCategoryByNameUsingContainerName(containerName, categoryName, FillDiagnosticTraces(iCallerContext)));
        }

        /// <summary>
        /// Gets the category for the specified hierarchy Id and category path
        /// </summary>
        /// <example>
        /// <code>
        /// 
        /// //Create an instance of data model service
        /// DataModelService mdmDataModelService = GetMDMDataModelService();
        /// 
        /// // Key Note: CallerContext has properties Application and Module which are mandatory to be set
        /// // Indicates name of application and module
        /// ICallerContext callerContext = MDMObjectFactory.GetICallerContext();
        /// callerContext.Application = MDMCenterApplication.PIM;
        /// callerContext.Module = MDMCenterModules.Entity;
        /// 
        /// //Get new instance of ICategory using MDMObjectFactory
        /// ICategory iCategory = MDMObjectFactory.GetICategory();
        ///
        /// // Assumed hard coded variables for API Sample purpose..
        /// Int32 hierarchyId = 1; //ProductHierarchy as per River works Model
        /// 
        /// //Path Separator should be as per the AppConfig "Catalog.Category.PathSeparator"
        ///  //Default Separator is " >> ".
        /// String categoryPath = "<![CDATA[toys >> vehicles >> battery Powered]]>"; //battery Powered category as per RiverWorks Model
        ///
        /// //Make a WCF call to DataModelService.GetCategoryByPath, It will return category details for requested Path
        /// iCategory = mdmDataModelService.GetCategoryByPath(hierarchyId, categoryPath, callerContext);
        /// 
        /// </code>
        /// </example>
        /// <param name="hierarchyId">Indicates the hierarchy Id in which category is requested</param>
        /// <param name="categoryPath">Indicates the path of the category</param>
        /// <param name="iCallerContext">Indicates the caller context, which contains the application and module that has invoked the API</param>
        /// <returns>Returns the category based on the hierarchy Id and the category path</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        public ICategory GetCategoryByPath(Int32 hierarchyId, String categoryPath, ICallerContext iCallerContext)
        {
            return MakeServiceCall<ICategory>("GetCategoryByPath", "GetCategoryByPath",
                                                  client =>
                                                      client.GetCategoryByPath(hierarchyId, categoryPath, FillDiagnosticTraces(iCallerContext)));
        }

        /// <summary>
        /// Gets the category for the specified container name and category path
        /// </summary>
        /// <param name="containerName">Indicates the container name in which category is requested</param>
        /// <param name="categoryPath">Indicates the path of the category</param>
        /// <param name="iCallerContext">Indicates the caller context, which contains the application and module that has invoked the API</param>
        /// <returns>Returns category by specified container name and category path</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        /// <example>
        /// <code language="c#" title="Get category by category path" source="..\MDM.APISamples\DataModelManager\Category\GetCategoryByName.cs" region="GetCategoryByPathUsingContainerName" />
        /// </example>

        public ICategory GetCategoryByPath(String containerName, String categoryPath, ICallerContext iCallerContext)
        {
            return MakeServiceCall<ICategory>("GetCategoryByPath", "GetCategoryByPathUsingContainerName",
                                                  client =>
                                                      client.GetCategoryByPathUsingContainerName(containerName, categoryPath, FillDiagnosticTraces(iCallerContext)));
        }

        /// <summary>
        /// Searches the categories based on the category context details
        /// </summary>
        /// <param name="iCategoryContext">Indicates the category context, which contains the hierarchy id, locale, and other criteria</param>
        /// <param name="iCallerContext">Indicates the caller context, which contains the application and module that has invoked the API</param>
        /// <returns>Returns a collection of categories</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        ///<example>
        ///<code language="c#" title="Search Categories" source="..\MDM.APISamples\DataModelManager\Category\SearchCategories.cs" region="Search Categories" />
        ///<code language="c#" title="Search Categories For GetCategoriesForLookup" source="..\MDM.APISamples\DataModelManager\Category\SearchCategories.cs" region="Search Categories For GetCategoriesForLookup" />
        ///<code language="c#" title="Search Categories For GetCategoryTreeNodeItems" source="..\MDM.APISamples\DataModelManager\Category\SearchCategories.cs" region="Search Categories For GetCategoryTreeNodeItems" />
        ///</example>
        public ICategoryCollection SearchCategories(ICategoryContext iCategoryContext, ICallerContext iCallerContext)
        {
            return MakeServiceCall<ICategoryCollection>("SearchCategories", "SearchCategories",
                                                  client =>
                                                      client.SearchCategories(iCategoryContext as CategoryContext, FillDiagnosticTraces(iCallerContext)));
        }

        #endregion

        #region UOM Get

        /// <summary>
        /// Get UOM  based on UOM context.
        /// </summary>
        /// <param name="iUomContext">Specifies UOM context containing short name , UOM type</param>
        /// <param name="iCallerContext">Context indicating the caller of the API</param>
        /// <returns>UOM object based on context.</returns>
        public IUOM GetUom(IUomContext iUomContext, ICallerContext iCallerContext)
        {
            return MakeServiceCall<IUOM>("GetUom", "GetUom", client => client.GetUom(iUomContext as UomContext, FillDiagnosticTraces(iCallerContext)));
        }

        /// <summary>
        /// Get UOM Conversion rates XML based on UOM context.
        /// </summary>
        /// <param name="iCallerContext">Context indicating the caller of the API</param>
        /// <returns>Conversion rates XML as string.</returns>
        public string GetUomConversionsAsXml(ICallerContext iCallerContext)
        {
            return MakeServiceCall<string>("GetUomConversionsAsXml", "GetUomConversionsAsXml", client => client.GetUomConversionsAsXml(FillDiagnosticTraces(iCallerContext)));
        }

        #endregion

        #region Search Category Methods

        /// <summary>
        /// Searches categories for a given search criteria and returns a list of entities with a specified context
        /// </summary>
        /// <example>
        /// <code>
        /// // Get MDM data service
        ///    DataModelService mdmDataService = GetMDMDataModelService();
        ///    
        ///    SearchCriteria searchCriteria = new SearchCriteria();
        ///    searchCriteria.CategoryIds = new <![CDATA[Collection<Int64>() ]]> { 11 }; // Search Category Fiction
        ///    searchCriteria.EntityTypeIds = new <![CDATA[Collection<Int32> ]]> { 16 }; // Search Entity Type Style
        ///    searchCriteria.ContainerIds = new <![CDATA[Collection<Int32>()]]> { 4 }; // Search Container Staging master
        ///    searchCriteria.Locales = new <![CDATA[Collection<LocaleEnum> ]]>{ LocaleEnum.en_WW }; 
        ///    searchCriteria.Locale = LocaleEnum.en_WW;
        ///
        ///    SearchContext searchContext = new SearchContext();
        ///    SearchContext.MaxRecordsToReturn = 200; // Defines the maximum search records to return
        ///    SearchContext.Locale = LocaleEnum.en_WW; // Set the Locale in which Search data needs to returned
        /// 
        /// // Get new instance of IOperationResult and ICallerContext using MDMObjectFactory
        ///    IOperationResult searchOperationResult = <![CDATA[MDMObjectFactory.GetIOperationResult()]]>;
        ///    ICallerContext callerContext = <![CDATA[MDMObjectFactory.GetICallerContext()]]>;
        ///
        ///    callerContext.Application = MDMCenterApplication.MDMCenter;
        ///    callerContext.Module = MDMCenterModules.Entity;
        ///
        ///    IEntityCollection iSearchedEntityCollection = mdmDataService.SearchEntities(iSearchCriteria, iSearchContext, searchOperationResult, callerContext);
        ///
        /// </code>
        /// </example>
        /// <param name="iSearchCriteria">Indicates the search criteria</param>
        /// <param name="iSearchContext">Indicates the search context. Example: SearchContext.MaxRecordCount indicates maximum records to be fetched while searching and AttributeIdList indicates the list of attributes to load in returned entities</param>
        ///  <param name="iSearchOperationResult">Indicates the search operation result</param>
        /// <param name="iCallerContext">Indicates  the caller context, which contains the application and module that has invoked the API</param>
        /// <returns>Returns a list of entities with a specified context</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        public IEntityCollection SearchCategories(ISearchCriteria iSearchCriteria, ISearchContext iSearchContext, IOperationResult iSearchOperationResult, ICallerContext iCallerContext)
        {
            return MakeServiceCall<IEntityCollection>("SearchCategories", "SearchCategories",
                                                  client =>
                                                      client.SearchCategories(iSearchCriteria as SearchCriteria, iSearchContext as SearchContext, iSearchOperationResult as OperationResult, FillDiagnosticTraces(iCallerContext)));
        }

        #endregion

        #region Unique Id Methods

        /// <summary>
        /// Generates a unique id(s) based on the ids specified in context
        /// </summary>
        /// <param name="iCallerContext">Represents caller context such as application and module</param>
        /// <param name="iUniqueIdGenerationContext">Represents the unique id context to generate the unique id</param>
        /// <returns>Returns a collection of string having auto ids generated from database</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        /// <example>
        /// <code language="c#" title="Generates unique id by context ids" source="..\MDM.APISamples\EntityManager\EntityHierarchy\GenerateUniqueId.cs" region="GenerateUniqueIdByContextIds" />
        /// </example>
        public Collection<String> GenerateUniqueId(ICallerContext iCallerContext, IUniqueIdGenerationContext iUniqueIdGenerationContext)
        {
            UniqueIdGenerationContext uniqueIdGenerateContext = (UniqueIdGenerationContext)iUniqueIdGenerationContext;

            return MakeServiceCall<Collection<String>>("GenerateUniqueId", "GenerateUniqueId",
                                    client => client.GenerateUniqueId(FillDiagnosticTraces(iCallerContext), uniqueIdGenerateContext));
        }

        #endregion

        #region Entity Model Method

        /// <summary>
        /// Gets the entity model details by providing container name, category name, entity type name and parent entity name in entity context
        /// </summary>
        /// <param name="iEntityContext">Indicates the data context for which entity needs to be fetched</param>
        /// <param name="iCallerContext">Indicates application and module name by which operation is being performed</param>
        /// <returns>Returns entity object</returns>
        /// <exception cref="EndpointNotFoundException">Thrown when a remote endpoint could not be found or reached</exception>
        /// <exception cref="InvalidOperationException">Thrown when a method call is invalid</exception>
        /// <exception cref="TimeoutException">Thrown when the time allotted for an operation has expired</exception>
        /// <exception cref="MDMOperationException">Thrown when the MDM operation fails</exception>
        /// <exception cref="CommunicationException">Thrown when there is a communication error in either the service or client application</exception>
        /// <example>
        /// <code language="c#" title="Get EntityModel Using ContextNames" source="..\MDM.APISamples\EntityManager\Entity\GetEntity.cs" region="GetEntityModelUsingContextNames" />
        /// </example>
        public IEntity GetEntityModel(IEntityContext iEntityContext, ICallerContext iCallerContext)
        {
            ValidateEntityContext(iEntityContext);

            EntityContext entityContext = (EntityContext)iEntityContext;
            entityContext.ResolveIdsByName = true;
            return MakeServiceCall<IEntity>("GetEntityModel", "GetEntityModel", client => client.GetEntityModel(entityContext, FillDiagnosticTraces(iCallerContext)));
        }

        #endregion

        #region Entity Variant Defintion Mapping

        /// <summary>
        /// Get all entity variant definition mappings
        /// </summary>
        /// <example>
        /// <code language="c#" title="Get All Entity Variant Definition Mappings"  source="..\..\Documentation\MDM.APISamples\EntityManager\EntityHierarchy\EntityVariantDefinition.cs" region="GetAllEntityVariantDefinitionMappings"/>
        /// </example>
        /// <param name="iCallerContext">Indicates the caller context specifying the caller application and module.</param>
        /// <returns>Returns collection of entity variant definition mappings</returns>
        public IEntityVariantDefinitionMappingCollection GetAllEntityVariantDefinitionMappings(ICallerContext iCallerContext)
        {
            return MakeServiceCall<IEntityVariantDefinitionMappingCollection>("GetAllEntityVariantDefinitionMappings", "GetAllEntityVariantDefinitionMappings", client =>
                client.GetAllEntityVariantDefinitionMappings(iCallerContext as CallerContext));
        }

        /// <summary>
        /// Create, Update and Delete entity variant definition mapping 
        /// </summary>
        /// <example>
        /// <code language="c#" title="Create Entity Variant Definition Mappings"  source="..\..\Documentation\MDM.APISamples\EntityManager\EntityHierarchy\EntityVariantDefinition.cs" region="CreateEntityVariantDefinitionMappings"/>
        /// <code language="c#" title="Update Entity Variant Definition Mappings"  source="..\..\Documentation\MDM.APISamples\EntityManager\EntityHierarchy\EntityVariantDefinition.cs" region="UpdateEntityVariantDefinitionMappings"/>
        /// <code language="c#" title="Delete Entity Variant Definition Mappings"  source="..\..\Documentation\MDM.APISamples\EntityManager\EntityHierarchy\EntityVariantDefinition.cs" region="DeleteEntityVariantDefinitionMappings"/>
        /// </example>
        /// <param name="iEntityVariantDefinitionMappings">Indicates collection of entity variant definition mappings to be processed.</param>
        /// <param name="iCallerContext">Indicates the caller context specifying the caller application and module.</param>
        /// <returns>Returns operation result.</returns>
        public IOperationResultCollection ProcessEntityVariantDefinitionMappings(IEntityVariantDefinitionMappingCollection iEntityVariantDefinitionMappings, ICallerContext iCallerContext)
        {
            return MakeServiceCall<IOperationResultCollection>("ProcessEntityVariantDefinitionMappings", "ProcessEntityVariantDefinitionMappings", client =>
                client.ProcessEntityVariantDefinitionMappings(iEntityVariantDefinitionMappings as EntityVariantDefinitionMappingCollection, iCallerContext as CallerContext));
        }

        #endregion Entity Variant Defintion Mapping

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private IDataModelService GetClient()
        {
            IDataModelService serviceClient = null;

            if (WCFServiceInstanceLoader.IsLocalInstancesEnabled())
            {
                serviceClient = WCFServiceInstanceLoader.GetServiceInstance<IDataModelService>();
            }

            if (serviceClient == null) //Means the given type is not implemented for local load..
            {
                DataModelServiceProxy dataModelServiceProxy = null;

                if (String.IsNullOrEmpty(this.EndPointConfigurationName) && this.EndpointAddress == null)
                    dataModelServiceProxy = new DataModelServiceProxy();
                else if (!String.IsNullOrEmpty(this.EndPointConfigurationName) && this.EndpointAddress == null)
                    dataModelServiceProxy = new DataModelServiceProxy(this.EndPointConfigurationName);
                else if (!String.IsNullOrEmpty(this.EndPointConfigurationName) && this.EndpointAddress != null)
                    dataModelServiceProxy = new DataModelServiceProxy(this.EndPointConfigurationName, this.EndpointAddress);

                if (this.AuthenticationType == Core.AuthenticationType.Forms)
                {
                    dataModelServiceProxy.ClientCredentials.UserName.UserName = this.UserName;
                    dataModelServiceProxy.ClientCredentials.UserName.Password = this.Password;
                    dataModelServiceProxy.ClientCredentials.ServiceCertificate.Authentication.CertificateValidationMode = System.ServiceModel.Security.X509CertificateValidationMode.None;
                }

                serviceClient = dataModelServiceProxy;
            }

            return serviceClient;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        private void DisposeClient(IDataModelService client)
        {
            if (client == null)
                return;

            if (client.GetType().Equals(typeof(DataModelServiceProxy)))
            {
                DataModelServiceProxy serviceClient = (DataModelServiceProxy)client;
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
        /// Makes the DataModelServiceClient call. Creates the client instance, executes call delegate against it in
        /// impersonated context (if necessary) and then disposes the client.
        /// Also emits traces when necessary.
        /// </summary>
        /// <typeparam name="TResult">The type of the result of service call.</typeparam>
        /// <param name="clientMethodName">Name of the client method to include in traces.</param>
        /// <param name="serverMethodName">Name of the server method to include in traces.</param>
        /// <param name="call">The call delegate to be executed against a client instance.</param>
        /// <param name="traceSource">The type operation source for tracing</param>
        /// <returns>The value returned by service, or default.</returns>
        private TResult MakeServiceCall<TResult>(string clientMethodName, string serverMethodName, Func<IDataModelService, TResult> call, MDMTraceSource traceSource = MDMTraceSource.APIFramework)
        {
            Boolean isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            //Start trace activity
            if (isTracingEnabled)
                MDMTraceHelper.StartTraceActivity("DataModelServiceClient." + clientMethodName, traceSource, false);

            TResult result = default(TResult);
            IDataModelService dataModelServiceClient = null;

            try
            {
                dataModelServiceClient = GetClient();

                ValidateContext();

                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "DataModelServiceClient sends '" + serverMethodName + "' request message.", traceSource);

                result = Impersonate(() => call(dataModelServiceClient));

                if (isTracingEnabled)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Verbose, "DataModelServiceClient receives '" + serverMethodName + "' response message.", traceSource);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            finally
            {
                //Close the client
                DisposeClient(dataModelServiceClient);

                if (isTracingEnabled)
                    MDMTraceHelper.StopTraceActivity("DataModelServiceClient." + clientMethodName, traceSource);
            }

            return result;
        }

        private void ValidateEntityContext(IEntityContext iEntityContext)
        {
            if (iEntityContext == null)
                throw new ArgumentNullException("iEntityContext");
        }

        #endregion
    }
}