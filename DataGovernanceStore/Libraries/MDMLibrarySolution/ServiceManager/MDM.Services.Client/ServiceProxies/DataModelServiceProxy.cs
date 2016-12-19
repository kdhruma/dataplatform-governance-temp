using System;
using System.Collections.ObjectModel;

namespace MDM.Services.ServiceProxies
{
    using MDM.BusinessObjects;
    using MDM.Core;
    using MDM.Services.DataModelServiceClient;

    /// <summary>
    /// Represents class for datamodel service proxy
    /// </summary>
    internal class DataModelServiceProxy : DataModelServiceClient, MDM.WCFServiceInterfaces.IDataModelService
    {
        /// <summary>
        /// Gets the lookup schema based on requested lookup names.
        /// </summary>
        /// <param name="lookupNames">Indicates list of lookup names</param>
        /// <param name="callerContext">Indicates application and module name by which operation is being performed</param>
        /// <returns>Returns the list of lookup schema</returns>
        public LookupCollection GetLookupSchema(Collection<String> lookupNames, CallerContext callerContext)
        {
            return this.GetLookupsSchema(lookupNames, callerContext);
        }

        #region Constructors

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public DataModelServiceProxy()
        {

        }

        /// <summary>
        /// Constructor with endpoint configuration name
        /// </summary>
        /// <param name="endpointConfigurationName">Indicates the endpoint configuration name</param>
        public DataModelServiceProxy(String endpointConfigurationName)
            : base(endpointConfigurationName)
        {
        }

        /// <summary>
        /// Constructor with endpoint configuration name and remote address
        /// </summary>
        /// <param name="endpointConfigurationName">Indicates the endpoint configuration name</param>
        /// <param name="remoteAddress">Indicates the remote address</param>
        public DataModelServiceProxy(String endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress)
            : base(endpointConfigurationName, remoteAddress)
        {
        }

        #endregion

        /* Note: Below are methods which has different name in the WCF contract so not coming up as part of Service Reference class
         * We need to explicitly divert call for all the mismatched method names.
         */

        #region IDataModelService Members

        /// <summary>
        /// Gets lookup data for the requested attribute and locale
        /// </summary>
        /// <param name="attributeId">Indicates identifier of the attribute for which lookup data needs to be fetched</param>
        /// <param name="locale">Indicates locale for which data needs to be fetched</param>
        /// <param name="maxRecordsToReturn">Indicates max number of lookup records to return. Setting '-1' returns all record</param>
        /// <param name="applicationContext">Indicates current context of the application</param>
        /// <param name="callerContext">Indicates application and module name by which action is being performed</param>
        /// <returns>Returns lookup data for the requested attribute and locale</returns>
        public Lookup GetAttributeLookupData(int attributeId, LocaleEnum locale, int maxRecordsToReturn, ApplicationContext applicationContext, CallerContext callerContext)
        {
            return this.GetAttributeLookupDataWithApplicationContext(attributeId, locale, maxRecordsToReturn, applicationContext, callerContext);
        }

        /// <summary>
        /// Gets lookup data for the requested attribute and locale
        /// </summary>
        /// <param name="attributeId">Indicates identifier of the attribute for which lookup data needs to be fetched</param>
        /// <param name="locale">Indicates locale for which data needs to be fetched</param>
        /// <param name="lookupValueIdList">Indicates list of lookup ids to be returned along with requested max number of records to return</param>
        /// <param name="callerContext">Indicates application and module name by which action is being performed</param>
        /// <returns>Returns lookup data for the requested attribute and locale</returns>
        public Lookup GetAttributeLookupData(int attributeId, LocaleEnum locale, Collection<int> lookupValueIdList, CallerContext callerContext)
        {
            return this.GetAttributeLookupDataWithLookupValueIdList(attributeId, locale, lookupValueIdList, callerContext);
        }

        /// <summary>
        /// Gets lookup data for the requested attribute and locale
        /// </summary>
        /// <param name="attributeId">Indicates identifier of the attribute for which lookup data needs to be fetched</param>
        /// <param name="locale">Indicates locale for which data needs to be fetched</param>
        /// <param name="maxRecordsToReturn">Indicates max number of lookup records to return. Setting '-1' returns all record</param>
        /// <param name="lookupValueIdList">Indicates list of lookup ids to be returned along with requested max number of records to return</param>
        /// <param name="applicationContext">Indicates current context of the application</param>
        /// <param name="callerContext">Indicates application and module name by which action is being performed</param>
        /// <returns>Returns lookup data for the requested attribute and locale</returns>
        public Lookup GetAttributeLookupData(int attributeId, LocaleEnum locale, int maxRecordsToReturn, Collection<int> lookupValueIdList, ApplicationContext applicationContext, CallerContext callerContext)
        {
            return this.GetAttributeLookupDataWithApplicationContextAndLookupValueIdList(attributeId, locale, maxRecordsToReturn, lookupValueIdList, applicationContext, callerContext);
        }

        /// <summary>
        /// Gets lookup data for the requested attribute and locale
        /// </summary>
        /// <param name="attributeIds">Indicates list of attribute identifiers for which data needs to be fetched</param>
        /// <param name="locale">Indicates locale for which data needs to be fetched</param>
        /// <param name="maxRecordsToReturn">Indicates max number of lookup records to return. Setting '-1' returns all record</param>
        /// <param name="applicationContext">Indicates current context of the application</param>
        /// <param name="callerContext">Indicates application and module name by which action is being performed</param>
        /// <returns>Returns collection of lookup data for the requested attribute and locale</returns>
        public LookupCollection GetAttributesLookupData(Collection<int> attributeIds, LocaleEnum locale, int maxRecordsToReturn, ApplicationContext applicationContext, CallerContext callerContext)
        {
            return this.GetAttributesLookupDataWithApplicationContext(attributeIds, locale, maxRecordsToReturn, applicationContext, callerContext);
        }

        /// <summary>
        /// Gets lookup data for the requested attribute and locale
        /// </summary>
        /// <param name="attributeId">Indicates identifier of the attribute for which lookup data needs to be fetched</param>
        /// <param name="locale">Indicates locale for which data needs to be fetched</param>
        /// <param name="maxRecordsToReturn">Indicates max number of lookup records to return. Setting '-1' returns all record</param>
        /// <param name="lookupValueIdList">Indicates list of lookup ids to be returned along with requested max number of records to return</param>
        /// <param name="applicationContext">Indicates current context of the application</param>
        /// <param name="callerContext">Indicates application and module name by which action is being performed</param>
        /// <param name="isDependent">Indicates whether the attribute is dependent or not</param>
        /// <param name="dependentAttributes">Indicates dependent attribute collection details</param>
        /// <returns>Returns lookup data for the requested attribute and locale</returns>
        public Lookup GetAttributeLookupData(Int32 attributeId, LocaleEnum locale, Int32 maxRecordsToReturn, Collection<Int32> lookupValueIdList, ApplicationContext applicationContext, CallerContext callerContext, Boolean isDependent, DependentAttributeCollection dependentAttributes)
        {
            return this.GetAttributeLookupDataFilterBasedOnDependency(attributeId, locale, maxRecordsToReturn, lookupValueIdList, applicationContext, callerContext, isDependent, dependentAttributes);
        }

        /// <summary>
        /// Gets lookup data for the requested attribute and locale
        /// </summary>
        /// <param name="attributeName">Specifies the short name of the attribute for which lookup data needs to be fetched</param>
        /// <param name="attributeParentName">Specifies the parent name of the attribute for which lookup data needs to be fetched</param>
        /// <param name="locale">Indicates locale for which data needs to be fetched</param>
        /// <param name="maxRecordsToReturn">Indicates max number of lookup records to return. Setting '-1' returns all record</param>
        /// <param name="lookupValueIdList">Indicates list of lookup ids to be returned along with requested max number of records to return</param>
        /// <param name="applicationContext">Indicates current context of the application</param>
        /// <param name="callerContext">Indicates application and module name by which action is being performed</param>
        /// <param name="isDependent">Indicates whether the attribute is dependent or not</param>
        /// <param name="dependentAttributeCollection">Indicates dependent attribute collection details</param>
        /// <returns>Returns lookup data for the requested attribute and locale</returns>        
        public Lookup GetAttributeLookupData(String attributeName, String attributeParentName, LocaleEnum locale, Int32 maxRecordsToReturn, Collection<Int32> lookupValueIdList, ApplicationContext applicationContext, CallerContext callerContext, Boolean isDependent, DependentAttributeCollection dependentAttributeCollection)
        {
            return base.GetAttributeLookupDataFilterBasedOnDependencyUsingName(attributeName, attributeParentName, locale, maxRecordsToReturn, lookupValueIdList,
                applicationContext, callerContext, isDependent, dependentAttributeCollection);
        }

        /// <summary>
        /// Gets all containers
        /// </summary>
        /// <param name="callerContext">Indicates application and module name by which action is being performed</param>
        /// <returns>Returns collection of container</returns>
        public ContainerCollection GetAllContainers(CallerContext callerContext)
        {
            return this.GetAllContainersWithoutContext(callerContext);
        }

        /// <summary>
        /// Gets container by given short name
        /// </summary>
        /// <param name="containerShortName">Indicates short name of the requested container</param>
        /// <param name="containerContext">Indicates flag to decide whether to load attributes or not</param>
        /// <param name="callerContext">Indicates application and module name by which action is being performed</param>
        /// <returns>Returns requested container by given short name</returns>
        public Container GetContainerByName(String containerShortName, ContainerContext containerContext, CallerContext callerContext)
        {
            return this.GetContainerByNameBasedOnContext(containerShortName, containerContext, callerContext);
        }

        /// <summary>
        ///  Gets container by given container short name and organization short name
        /// </summary>
        /// <param name="containerShortName">Indicates short name of the requested container</param>
        /// <param name="organizationName">Indicates short name of the organization</param>
        /// <param name="containerContext">Indicates flag to decide whether to load attributes or not</param>
        /// <param name="callerContext">Indicates application and module name by which action is being performed</param>
        /// <returns>Returns requested container by given short name and organization short name</returns>
        public Container GetContainerByContainerNameAndOrgName(String containerShortName, String organizationName, ContainerContext containerContext, CallerContext callerContext)
        {
            return this.GetContainerByContainerNameAndOrgNameBasedOnContext(containerShortName, organizationName, containerContext, callerContext);
        }

        /// <summary>
        /// Gets container by container id
        /// </summary>
        /// <param name="containerId">Indicates identifier of the requested Container</param>
        /// <param name="containerContext">Indicates flag to decide whether to load attributes or not</param>
        /// <param name="callerContext">Indicates application and module name by which action is being performed</param>
        /// <returns>Returns requested container by container id</returns>
        public Container GetContainerById(Int32 containerId, ContainerContext containerContext, CallerContext callerContext)
        {
            return this.GetContainerByIdBasedOnContext(containerId, containerContext, callerContext);
        }

        /// <summary>
        /// Gets lookup data for the requested attribute and locale
        /// </summary>
        /// <param name="attributeId">Indicates identifier of the attribute for which lookup data needs to be fetched</param>
        /// <param name="locale">Indicates locale for which data needs to be get</param>
        /// <param name="maxRecordsToReturn">Indicates max number of lookup records to return. Setting '-1' returns all record</param>
        /// <param name="searchValue">Indicates value to be searched for the attribute lookup.</param>
        /// <param name="applicationContext">Indicates current context of the application</param>
        /// <param name="isDependent">Indicates whether the attribute is dependent or not</param>
        /// <param name="dependentAttributes">Indicates dependent attribute collection details</param>
        /// <param name="callerContext">Indicates application and module name by which action is being performed</param>
        /// <returns>Returns lookup data for the requested attribute and locale</returns>
        public Lookup SearchAttributeLookupData(Int32 attributeId, LocaleEnum locale, Int32 maxRecordsToReturn, String searchValue, ApplicationContext applicationContext, Boolean isDependent, DependentAttributeCollection dependentAttributes, CallerContext callerContext)
        {
            return this.DependentSearchAttributeLookupData(attributeId, locale, maxRecordsToReturn, searchValue, applicationContext, isDependent, dependentAttributes, callerContext);
        }

        /// <summary>
        /// Process attribute data dependency 
        /// </summary>
        /// <param name="dependentAttributeDataMap">Indicates dependent attribute data map to delete</param>
        /// <param name="callerContext">Indicates application and module name by which action is being performed</param>
        /// <returns>Returns operation result after processing dependent attributes</returns>
        public OperationResult ProcessDependentData(DependentAttributeDataMap dependentAttributeDataMap, CallerContext callerContext)
        {
            return ProcessDependentDatum(dependentAttributeDataMap, callerContext);
        }

        /// <summary>
        /// Gets the attribute model for the requested attribute unique identifier (attribute name, and group name)
        /// </summary>
        /// <param name="attributeUniqueIdentifier">Indicates unique identifier for attribute</param>
        /// <param name="locale">Indicates locale of attribute model</param>
        /// <param name="callerContext">Indicates application and module name by which action is being performed</param>
        /// <returns>Returns attribute model based on requested attribute unique identifier</returns>
        public AttributeModel GetAttributeModelByUniqueIdentifier(AttributeUniqueIdentifier attributeUniqueIdentifier, LocaleEnum locale, CallerContext callerContext)
        {
            return GetAttributeModelByUniqueIdentifierWithLocale(attributeUniqueIdentifier, locale, callerContext);
        }

        /// <summary>
        /// Gets the attribute models for the requested attribute unique identifier (attribute name, and group name)
        /// </summary>
        /// <param name="attributeUniqueIdentifier">Indicates unique identifier for attribute collection</param>
        /// <param name="locales">Indicates locale of attribute models</param>
        /// <param name="callerContext">Indicates application and module name by which action is being performed</param>
        /// <returns>Returns collection of attribute model based on requested attribute unique identifier</returns>
        public AttributeModelCollection GetAttributeModelsByUniqueIdentifier(AttributeUniqueIdentifier attributeUniqueIdentifier, Collection<LocaleEnum> locales, CallerContext callerContext)
        {
            return GetAttributeModelsByUniqueIdentifierWithLocales(attributeUniqueIdentifier, locales, callerContext);
        }

        /// <summary>
        /// Gets the attribute models for the requested attribute unique identifier (attribute name, and group name)
        /// </summary>
        /// <param name="attributeUniqueIdentifiers">Indicates unique identifier for attribute collection</param>
        /// <param name="locale">Indicates locale of attribute models</param>
        /// <param name="callerContext">Indicates application and module name by which action is being performed</param>
        /// <returns>Returns collection of attribute model based on requested attribute unique identifier</returns>
        public AttributeModelCollection GetAttributeModelsByUniqueIdentifiers(AttributeUniqueIdentifierCollection attributeUniqueIdentifiers, LocaleEnum locale, CallerContext callerContext)
        {
            return GetAttributeModelsByUniqueIdentifiersWithLocale(attributeUniqueIdentifiers, locale, callerContext);
        }

        /// <summary>
        /// Search categories for given search criteria and return list of categories with specified context. 
        /// </summary>
        /// <param name="searchCriteria">Indicates search criteria.</param>
        /// <param name="searchContext">Indicates search context. Example: SearchContext.MaxRecordCount indicates max records to be fetched while searching</param>
        /// <param name="searchOperationResult">Indicates search operation result</param>
        /// <param name="callerContext">Indicates application an module name by which action is being performed</param>
        /// <returns>Returns collection of categories based on search criteria and search context</returns>
        public EntityCollection SearchCategories(SearchCriteria searchCriteria, SearchContext searchContext, OperationResult searchOperationResult, CallerContext callerContext)
        {
            return SearchCategoriesByCategoryAttributeValues(searchCriteria, searchContext, searchOperationResult, callerContext);
        }

        #endregion

    }
}
