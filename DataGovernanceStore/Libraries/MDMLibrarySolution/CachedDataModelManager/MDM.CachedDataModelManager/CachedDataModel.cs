using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml;

namespace MDM.CachedDataModelManager
{
    using MDM.AdminManager.Business;
    using MDM.AttributeModelManager.Business;
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Diagnostics;
    using MDM.CacheManager.Business;
    using MDM.ConfigurationManager.Business;
    using MDM.Core;
    using MDM.DataModelManager.Business;
    using MDM.ExceptionManager;
    using MDM.ExceptionManager.Handlers;
    using MDM.Interfaces;
    using MDM.KnowledgeManager.Business;
    using MDM.LookupManager.Business;
    using MDM.ParallelizationManager.Processors;
    using MDM.Services;
    using MDM.Utility;
    using RS.MDM.Configuration;
    using RS.MDM.ConfigurationObjects;

    public sealed class CachedDataModel : ICachedDataModel
    {
        #region Lock Object

        // lock object for the load all..singleton
        private static Object lockObj = new Object();

        // lock object for the load all..singleton
        private Object categoryLockObj = new Object();
        #endregion

        #region Fields

        public static EventLogHandler LogHandler = null;

        private static CachedDataModel _instance = null;

        private ICallerContext _callerContext = new CallerContext(MDMCenterApplication.JobService, MDMCenterModules.Import);

        /// <summary>
        /// Field denotes the cache manager.
        /// </summary>
        private ICache _cacheManager = null;

        /// <summary>
        /// Field denotes an instance of the CacheSynchronizationHelper class.
        /// </summary>
        private CacheSynchronizationHelper _cacheSynchronizationHelper = null;

        private String defaultErrorMessage = " {0} were not loaded. Either they are not available in the database or there were issues loading it. This needs to be corrected before doing any imports.";

        private Collection<LocaleEnum> _localeNames = null;
                
        #endregion

        #region Constructors

        #endregion

        #region Properties

        #endregion

        #region Methods

        public static ICachedDataModel GetSingleton(Boolean lazyLoading = false)
        {
            if (_instance == null)
            {
                lock (lockObj)
                {
                    if (_instance == null)
                    {
                        _instance = new CachedDataModel();
                        LogHandler = new EventLogHandler();

                        if (!lazyLoading)
                        {
                            _instance.LoadAll();
                    }
                }
            }
            }

            return _instance;
        }

        public Boolean LoadUserPermissions()
        {
            Boolean successFlag = false;

            try
            {
                PermissionContext context = new PermissionContext(0, 0, 0, 0, 0, 0, 0, 0, SecurityPrincipalHelper.GetCurrentSecurityPrincipal().CurrentUserId, 0);

                DataSecurityBL dataSecurityBL = new DataSecurityBL();
                dataSecurityBL.LoadPermissions(context);

                successFlag = true;
            }
            catch (Exception ex)
            {
                throw new Exception("CachedDataModel: LoadUserPermissions: Not able to load permissions. Error: " + ex.Message);
            }

            return successFlag;
        }

        /// <summary>
        /// Returns the list of organizations available in the system
        /// </summary>
        /// <returns>List of Organization objects</returns>
        public List<Organization> GetOrganizations()
        {
            String cachedDataModelOrgCacheKey = CacheKeyGenerator.GetAllCachedDataModelOrganizationsCacheKey();
            
            List<Organization> organizations = GetDataModelListFromCacheOrDB<Organization>("Organizations", cachedDataModelOrgCacheKey, () =>
            {
                IOrganizationContext organizationContext = MDMObjectFactory.GetIOrganizationContext();
                DataModelService dataModelService = new DataModelService();
                return dataModelService.GetAllOrganizations(organizationContext, _callerContext);
            });

            return organizations;
        }

        /// <summary>
        /// Returns the list of containers available in the system
        /// </summary>
        /// <returns>List of Container objects</returns>
        public List<Container> GetContainers()
        {
            String cachedDataModelContainerCacheKey = CacheKeyGenerator.GetAllCachedDataModelContainersCacheKey();

            List<Container> containers = GetDataModelListFromCacheOrDB<Container>("Containers", cachedDataModelContainerCacheKey, () =>
                        {
                DataModelService dataModelService = new DataModelService();
                ContainerContext containerContext = new ContainerContext();
                containerContext.IncludeApproved = true;
                return dataModelService.GetAllContainers(containerContext as IContainerContext, _callerContext);
            });

            return containers;
                        }

        /// <summary>
        /// Returns the list of hierarchies available in the system
        /// </summary>
        /// <returns>List of Hierarchy objects</returns>
        public List<Hierarchy> GetHierarchies()
                {
            String cachedDataModelHierarchiesCacheKey = CacheKeyGenerator.GetAllCachedDataModelHierarchiesCacheKey();
                    
            List<Hierarchy> hierarchies = GetDataModelListFromCacheOrDB<Hierarchy>("Hierarchies", cachedDataModelHierarchiesCacheKey, () =>
                {
                DataModelService dataModelService = new DataModelService();
                return dataModelService.GetAllHierarchies(_callerContext);
            });

            return hierarchies;
                }

        /// <summary>
        /// Returns the list of entity types available in the system
        /// </summary>
        /// <returns>List of EntityType objects</returns>
        public List<EntityType> GetEntityTypes()
        {
            String cachedDataModelEntityTypeCacheKey = CacheKeyGenerator.GetAllCachedDataModelEntityTypesCacheKey();

            List<EntityType> entityTypes = GetDataModelListFromCacheOrDB<EntityType>("EntityTypes", cachedDataModelEntityTypeCacheKey, () =>
            {
                DataModelService dataModelService = new DataModelService();
                return dataModelService.GetAllEntityTypes(_callerContext);
            });

            return entityTypes;
        }

        /// <summary>
        /// Returns the list of relationship types available in the system
        /// </summary>
        /// <returns>List of RelationshipType objects</returns>
        public List<RelationshipType> GetRelationshipTypes()
        {
            String relationshipTypeCacheKey = CacheKeyGenerator.GetAllCachedDataModelRelationshipTypesCacheKey();

            List<RelationshipType> relationshipTypes = GetDataModelListFromCacheOrDB<RelationshipType>("RelationshipTypes", relationshipTypeCacheKey, () =>
            {
                DataModelService dataModelService = new DataModelService();
                return dataModelService.GetAllRelationshipTypes(_callerContext);
            });

            return relationshipTypes;
                    }

        #region Category Management

        /// <summary>
        /// Returns the categories available in the system as a hierarchy id bases dictionary.
        /// </summary>
        /// <returns>Hierarchy id based dictionary of category objects</returns>
        public Dictionary<Int32, CategoryCollection> GetCategories()
        {
            String cachedDataModelCategoriesCacheKey = CacheKeyGenerator.GetAllCachedDataModelCategoriesCacheKey();

            Dictionary<Int32, CategoryCollection> categories = GetDataFromCache<Dictionary<Int32, CategoryCollection>>(cachedDataModelCategoriesCacheKey);

            if (categories == null)
            {
                lock (categoryLockObj)
                {
                    // check again to make sure another thread has not loaded it again.
                    categories = GetDataFromCache<Dictionary<Int32, CategoryCollection>>(cachedDataModelCategoriesCacheKey);

                    if (categories == null)
                    {
                        categories = LoadDataModelFromDB<KeyValuePair<Int32, CategoryCollection>>("Categories", () =>
                        {
                            Dictionary<Int32, CategoryCollection> localCategories = new Dictionary<Int32, CategoryCollection>();

                            List<Hierarchy> hierarchies = this.GetHierarchies();

                            Int32 hierarchyId = 0;
                            DataModelService dataModelService = new DataModelService();

                            if (hierarchies != null && hierarchies.Count > 0)
                            {
                                foreach (Hierarchy hierarchy in hierarchies)
                                {
                                    hierarchyId = hierarchy.Id;
                                    if (!localCategories.ContainsKey(hierarchyId))
                                    {
                                        //The old API was passing 1 as locale, Same behavior is achieved using LocaleEnum.en_WW
                                        CategoryCollection categoryCollection = dataModelService.GetAllCategories(hierarchyId, LocaleEnum.en_WW, _callerContext) as CategoryCollection;
                                        localCategories.Add(hierarchyId, categoryCollection);
                                    }
                                }
                            }

                            return localCategories;
                        }) as Dictionary<Int32, CategoryCollection>;

                        if (categories != null && categories.Count > 0)
                        {
                            SetDataInCache(cachedDataModelCategoriesCacheKey, categories);
                        }
                    }
                }
            }

            return categories;
        }

        /// <summary>
        /// Returns the categories available in the system based on the hierarchy id specified.
        /// </summary>
        /// <param name="hierarchyId">Indicates the hierarchy id based on which the categories have to be retrieved</param>
        /// <returns>Collection of category objects</returns>
        public CategoryCollection GetCategories(Int32 hierarchyId)
        {
            CategoryCollection categoryCollection = null;
            Dictionary<Int32, CategoryCollection> categories = this.GetCategories();

            if (categories != null && categories.ContainsKey(hierarchyId))
            {
                categoryCollection = categories[hierarchyId];
            }

            return categoryCollection;
        }

        /// <summary>
        /// Returns the category based on the hierarchy id and category path.
        /// </summary>
        /// <param name="hierarchyId">Indicates the hierarchy id based on which the category has to be retrieved</param>
        /// <param name="categoryPath">Indicates the category path based on which the category has to be retrieved</param>
        /// <returns>A Category objects</returns>
        public Category GetCategory(Int32 hierarchyId, String categoryPath)
        {
            Category category = GetCategory(hierarchyId, categories =>
            {
                var categoryList = categories.Where(cat => (String.Compare(cat.Path, categoryPath, StringComparison.InvariantCultureIgnoreCase) == 0));
                if (!categoryList.Any())
                {
                    //try to find category by long name path..
                    categoryList = categories.Where(cat => (String.Compare(cat.LongNamePath, categoryPath, StringComparison.InvariantCultureIgnoreCase) == 0));
                }
                return categoryList;
            });
            return category;
        }

        /// <summary> 
        /// Get Category based on Category Id and Hierarchy id.
        /// </summary>
        /// <param name="hierarchyId">Indicates the Hierarchy id</param>
        /// <param name="categoryId">Indicates the category id</param>
        /// <returns>Return category if it present.else null. Also if more than one found then will throw error.</returns>
        public Category GetCategory(Int32 hierarchyId, Int64 categoryId)
        {
            Category category = GetCategory(hierarchyId, categories =>
            {
                var categoryList = categories.Where(cat => cat.Id == categoryId);
                return categoryList;
            });
            return category;
        }

        /// <summary> 
        /// Get Category based on Category path and Hierarchy name.
        /// </summary>
        /// <param name="hierarchyId">Indicates the Hierarchy id</param>
        /// <param name="categoryId">Indicates the category path</param>
        /// <returns>Return category if it present.else null. Also if more than one found then will throw error.</returns>
        public Category GetCategory(String hierarchyName, String categoryPath)
        {
            Hierarchy hierarchy = GetHierarchyByName(hierarchyName);
            if (hierarchy == null)
                return null;

            Category category = this.GetCategory(hierarchy.Id, categoryPath);
            return category;
        }

        #endregion

        #region Attribute Model Management

        /// <summary>
        /// Returns the AttributeModels available in the system as an AttributeModelContext based dictionary.
        /// </summary>
        /// <returns>AttributeModelContext based dictionary of AttributeModels</returns>
        public ConcurrentDictionary<AttributeModelContext, AttributeModelCollection> GetAllContextualAttributeModels()
        {
            return LoadAllContextualAttributeModels();
                    }

        /// <summary>
        /// Returns the AttributeModels based on the requested AttributeModelContext.
        /// </summary>
        /// <param name="attributeModelContext">Indicates the context object based on which attribute models needs to be retrieved</param>
        /// <returns>Collection of AttributeModels</returns>
        public AttributeModelCollection GetContextualAttributeModels(AttributeModelContext attributeModelContext)
        {
            AttributeModelCollection returnAttributeModelCollection = new AttributeModelCollection();
            returnAttributeModelCollection = this.GetAttributeModelsUsingBL(attributeModelContext);
            return returnAttributeModelCollection;
        }

        /// <summary>
        /// Returns the AttributeModel based on the AttributeModelContext and the attribute specified.
        /// </summary>
        /// <param name="attributeModelContext">Indicates the context object based on which attribute model needs to be retrieved</param>
        /// <param name="attribute">Indicates the attribute object for which attribute model needs to be retrieved</param>
        /// <returns>AttributeModel object</returns>
        public AttributeModel GetContextualAttributeModel(AttributeModelContext attributeModelContext, Attribute attribute)
        {
            AttributeModel attributeModel = null;
            AttributeModelCollection models = this.GetAttributeModelsUsingBL(attributeModelContext);

            if (models != null && models.Count > 0)
            {
                attributeModel = models.GetAttributeModel(attribute);
            }
            return attributeModel;
        }

        /// <summary>
        /// Returns the AttributeModels based on the requested AttributeModelContext and attribute id.
        /// </summary>
        /// <param name="attributeModelContext">Indicates the context object based on which attribute models needs to be retrieved</param>        
        /// <param name="attributeId">Indicates the attribute id for which attribute model needs to be retrieved</param>
        /// <returns>Collection of AttributeModels</returns>
        public AttributeModelCollection GetContextualAttributeModel(AttributeModelContext attributeModelContext, Int32 attributeId)
        {
            AttributeModelCollection attributeModel = null;

            AttributeModelCollection attributeModels = this.GetAttributeModelsUsingBL(attributeModelContext);

            attributeModel = attributeModels.GetAttributeModel(attributeId) as AttributeModelCollection;

            return attributeModel;
        }

        /// <summary>
        /// Returns the AttributeModels based on the requested AttributeModelContext and attribute ids.
        /// </summary>
        /// <param name="attributeModelContext">Indicates the context object based on which attribute models needs to be retrieved</param>        
        /// <param name="attributeIds">Indicates the attribute id's for which attribute models needs to be retrieved</param>
        /// <returns>Collection of AttributeModels</returns>
        public AttributeModelCollection GetContextualAttributeModels(AttributeModelContext attributeModelContext, Collection<Int32> attributeIds)
        {
            AttributeModelCollection filteredAttributeModels = new AttributeModelCollection();

            AttributeModelCollection contextualAttributeModels = this.GetAttributeModelsUsingBL(attributeModelContext);

            if (contextualAttributeModels != null)
            {
                foreach (Int32 attributeId in attributeIds)
                {
                    AttributeModelCollection attributeModel = new AttributeModelCollection();
                    attributeModel = contextualAttributeModels.GetAttributeModel(attributeId) as AttributeModelCollection;

                    if (attributeModel != null && attributeModel.Count > 0)
                    {
                        filteredAttributeModels.AddRange(attributeModel);
                }
            }
            }

            return filteredAttributeModels;
        }

        /// <summary>
        /// Returns the collection of attribute models available in the system
        /// </summary>
        /// <returns>An AttributeModelCollection object</returns>
        public AttributeModelCollection GetAllBaseAttributeModels()
        {
            String message = String.Empty;
            TraceSettings traceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();

            DiagnosticActivity activity = new DiagnosticActivity();
            if (traceSettings.IsBasicTracingEnabled)
            {
                activity.Start();
            }

            Collection<LocaleEnum> locales = this.GetAvailableLocaleValues();
            String localesAsString = String.Empty;

            if (traceSettings.IsBasicTracingEnabled)
            {
                if (locales != null)
                {
                    localesAsString = ValueTypeHelper.JoinCollection(locales, ",");
                }

                message = String.Format("Loading all attribute master records for Locales: {0}", localesAsString);
                activity.LogInformation(message);
            }

            AttributeModelContext attributeModelContext = new AttributeModelContext()
            {
                AttributeModelType = AttributeModelType.AttributeMaster,
                ApplySecurity = false,
                ApplySorting = false,
                Locales = locales
            };

            AttributeModelCollection attributeModels = this.GetAttributeModelsUsingBL(attributeModelContext);

            if (attributeModels == null)
            {
                message = String.Format(defaultErrorMessage, "Base attribute models");
                activity.LogError(message);

                WriteEventLogMessage(message, MessageClassEnum.Error);
            }
            else if (traceSettings.IsBasicTracingEnabled)
            {
                message = String.Format("{0} attribute master records for Locales: {1}", attributeModels.Count, localesAsString);
                activity.LogInformation(message);
            }

            if (traceSettings.IsBasicTracingEnabled)
            {
                activity.Stop();
            }

            return attributeModels;
        }

        /// <summary>
        /// Returns the AttributeModel object based on the attribute id and locale specified
        /// </summary>
        /// <param name="attributeId">Indicates the attribute id for which attribute models needs to be retrieved</param>
        /// <param name="locale">Indicates the locale for which attribute models needs to be retrieved</param>
        /// <returns>AttributeModel object</returns>
        public AttributeModel GetBaseAttributeModel(Int32 attributeId, LocaleEnum locale)
        {
            AttributeModel attributeModel = null;

            AttributeModelCollection attributeModelCollection = GetAllBaseAttributeModels();

            if (attributeModelCollection != null)
                attributeModel = (AttributeModel)attributeModelCollection.GetAttributeModel(attributeId, locale);

            return attributeModel;
        }

        #endregion

        #region Lookup Management

        public void LoadLookupTableCollection()
        {
            //DO nothing here..all lookup are loaded on demand only
        }

        /// <summary>
        /// Gets the look up table for a given attribute it from the cache.
        /// </summary>
        /// <param name="attributeId"></param>
        /// <param name="locale"></param>
        /// <returns></returns>
        public Lookup GetLookupTable(Int32 attributeId, LocaleEnum locale)
        {
            TraceSettings traceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();

            DiagnosticActivity activity = new DiagnosticActivity();
            if (traceSettings.IsBasicTracingEnabled)
                    {
                activity.Start();
            }

            Lookup lookupTable = null;

            try
                        {
                LookupBL lookupBL = new LookupBL();
                lookupTable = lookupBL.Get(attributeId, locale, -1, null, new MDM.BusinessObjects.ApplicationContext(), _callerContext as CallerContext, true);
                            }
            catch (Exception ex)
            {
                String message = String.Format("CachedDataModel: Not able to load lookup table for attribute Id: {0}, locale: {1}. Error: {2}", attributeId, locale, ex.Message);

                activity.LogError(message);
                throw new Exception(message);
                                }

            if (traceSettings.IsBasicTracingEnabled)
            {
                activity.Stop();
            }

            return lookupTable;
        }

        /// <summary>
        /// Get the lookup based on requested lookup name and locale
        /// </summary>
        /// <param name="lookupName">Indicates the lookup Name</param>
        /// <param name="locale">Indicates the locale</param>
        /// <returns>Returns the lookup object</returns>
        public Lookup GetLookup(String lookupName, LocaleEnum locale)
        {
            TraceSettings traceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();

            DiagnosticActivity activity = new DiagnosticActivity();
            if (traceSettings.IsBasicTracingEnabled)
            {
                activity.Start();
            }

            Lookup lookup = null;
            //Do we required to put thread lock here
            try
            {
                LookupBL lookupBL = new LookupBL();
                lookup = lookupBL.Get(lookupName, locale, -1, false, _callerContext as CallerContext);

                Boolean exportLookupAuditInfo = AppConfigurationHelper.GetAppConfig<Boolean>("MDMCenter.LookupExportManager.ExportLookupAuditInfo.Enabled", false);

                if (exportLookupAuditInfo)
                {
                    LookupAuditInfoBL lookupAuditInfoBL = new LookupAuditInfoBL();
                    lookupAuditInfoBL.PopulateLookupAuditInfo(lookup, _callerContext as CallerContext);
                }
            }
            catch (Exception ex)
            {
                String message = String.Format("CachedDataModel: Not able to load lookup {0}. Error: {1}", lookupName, ex.Message);

                activity.LogError(message);
                throw new Exception(message);
            }

            if (traceSettings.IsBasicTracingEnabled)
            {
                activity.Stop();
            }

            return lookup;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lookupNames"></param>
        /// <param name="locales"></param>
        /// <returns></returns>
        public LookupCollection GetLookups(Collection<String> lookupNames, Collection<LocaleEnum> locales)
        {
            LookupCollection result = null;

            if (lookupNames == null || lookupNames.Count <= 0 || locales == null || locales.Count <= 0)
            {
                return result;
            }

            result = new LookupCollection();

            //Create parallel get here with some batch size. Assuming a default of 3 tasks
            Int32 numberOfLookupsThreads = AppConfigurationHelper.GetAppConfig("MDMCenter.CachedDataModel.ParallelLookupGet.ThreadPoolSize", 3);

            // do we have enough lookups?
            if (lookupNames.Count < numberOfLookupsThreads)
                numberOfLookupsThreads = 1;

            this._localeNames = locales;
            var lookupCollection = new ParallelTaskProcessor().RunInParallel<String, LookupCollection>(lookupNames, GetLookups, null, numberOfLookupsThreads);

            foreach (LookupCollection lookups in lookupCollection)
            {
                result.AddLookups(lookups);
            }

            return result;
        }

        #endregion

        #region UOM Management

        /// <summary>
        /// Returns the list of UOM's available in the system
        /// </summary>
        /// <returns>Collection of UOM objects</returns>
        public UOMCollection LoadUOMCollection()
        {
            TraceSettings traceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();

            DiagnosticActivity activity = new DiagnosticActivity();
            if (traceSettings.IsBasicTracingEnabled)
            {
                activity.Start();
            }

            String cacheKey = CacheKeyGenerator.GetAllCachedDataModelUOMCacheKey();

            UOMCollection uomCollection = GetDataFromCache<UOMCollection>(cacheKey);
            if (uomCollection == null)
            {
                uomCollection = (UOMCollection)LoadDataModelFromDB<UOM>("UomCollection", () =>
                {
                    //BL call here, because UOM is not cached in BL and it makes direct DA call.
                            UomBL uomManager = new UomBL();
                            String uomCollectionXml = uomManager.GetAll();
                    return new UOMCollection(uomCollectionXml);
                });

                if (uomCollection != null && uomCollection.Count > 0)
                {
                    SetDataInCache<UOMCollection>(cacheKey, uomCollection);
                        }
                    }
            else if (traceSettings.IsBasicTracingEnabled)
                    {
                activity.LogInformation(String.Format("{0} UomCollection loaded from cache", uomCollection.Count));
                    }

            if (traceSettings.IsBasicTracingEnabled)
            {
                activity.Stop();
                    }

            return uomCollection;
        }

        #endregion

        #region ToXML

        /// <summary>
        /// Dump out the contents of the cache data model object.
        /// </summary>
        /// <returns></returns>
        public String ToXml()
        {
            String cacheDataModelXml = String.Empty;
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            xmlWriter.WriteStartElement("CachedDataModel");

            #region write entity meta data for Full Xml

            #endregion

            #region Write entity type Xml

            xmlWriter.WriteStartElement("EntityTypes");
            
            List<EntityType> entityTypes = GetEntityTypes();

            if (entityTypes != null && entityTypes.Count > 0)
            {
                foreach (EntityType entityType in entityTypes)
                {
                    xmlWriter.WriteStartElement("EntityType");
                    xmlWriter.WriteAttributeString("Id", entityType.Id.ToString());
                    xmlWriter.WriteAttributeString("Name", entityType.Name);
                    xmlWriter.WriteAttributeString("LongName", entityType.LongName);
                    xmlWriter.WriteEndElement();
                    //xmlWriter.WriteRaw(entityType.ToXML());                    
                }
            }
            else
            {
                xmlWriter.WriteAttributeString("EntityType", "No Entity Types available in job service cache.");
            }

            xmlWriter.WriteEndElement();

            #endregion write entity type Xml

            #region Write relationship type Xml

            xmlWriter.WriteStartElement("RelationshipTypes");

            List<RelationshipType> relationshipTypes = GetRelationshipTypes();

            if (relationshipTypes != null && relationshipTypes.Count > 0)
            {
                foreach (RelationshipType relationshipType in relationshipTypes)
                {
                    xmlWriter.WriteStartElement("RelationshipType");
                    xmlWriter.WriteAttributeString("Id", relationshipType.Id.ToString());
                    xmlWriter.WriteAttributeString("Name", relationshipType.Name);
                    xmlWriter.WriteAttributeString("LongName", relationshipType.LongName);
                    xmlWriter.WriteEndElement();
                    //xmlWriter.WriteRaw(relationshipType.ToXml());
                }
            }
            else
            {
                xmlWriter.WriteAttributeString("RelationshipType", "No relationship types available in job service cache.");
            }

            xmlWriter.WriteEndElement();

            #endregion write relationship type Xml

            #region Write Organization Xml

            xmlWriter.WriteStartElement("Organizations");

            List<Organization> organizations = GetOrganizations();

            if (organizations != null && organizations.Count > 0)
            {
                foreach (Organization organization in organizations)
                {
                    xmlWriter.WriteStartElement("Organization");
                    xmlWriter.WriteAttributeString("Id", organization.Id.ToString());
                    xmlWriter.WriteAttributeString("Name", organization.Name);
                    xmlWriter.WriteAttributeString("LongName", organization.LongName);
                    xmlWriter.WriteEndElement();
                }
            }
            else
            {
                xmlWriter.WriteAttributeString("Organization", "No organization available in job service cache.");
            }

            xmlWriter.WriteEndElement();

            #endregion write Organization Xml

            #region Write Containers Xml

            xmlWriter.WriteStartElement("Containers");

            List<Container> containers = GetContainers();

            if (containers != null && containers.Count > 0)
            {
                foreach (Container container in containers)
                {
                    xmlWriter.WriteStartElement("Container");
                    xmlWriter.WriteAttributeString("Id", container.Id.ToString());
                    xmlWriter.WriteAttributeString("Name", container.Name);
                    xmlWriter.WriteAttributeString("LongName", container.LongName);
                    xmlWriter.WriteEndElement();
                    //xmlWriter.WriteRaw(container.ToXml());
                }
            }
            else
            {
                xmlWriter.WriteAttributeString("Containers", "No containers available in job service cache.");
            }

            xmlWriter.WriteEndElement();

            #endregion write Containers Xml

            #region Write Hierarchies Xml

            xmlWriter.WriteStartElement("Hierarchies");

            List<Hierarchy> hierarchies = GetHierarchies();

            if (hierarchies != null && hierarchies.Count > 0)
            {
                foreach (Hierarchy hierarchy in hierarchies)
                {
                    xmlWriter.WriteStartElement("Hierarchy");
                    xmlWriter.WriteAttributeString("Id", hierarchy.Id.ToString());
                    xmlWriter.WriteAttributeString("Name", hierarchy.Name);
                    xmlWriter.WriteAttributeString("LongName", hierarchy.LongName);
                    xmlWriter.WriteEndElement();
                }
            }
            else
            {
                xmlWriter.WriteAttributeString("Hierachies", "No hierarchies available in job service cache.");
            }

            xmlWriter.WriteEndElement();

            #endregion write Hierarchies Xml

            #region Write Categories Xml

            xmlWriter.WriteStartElement("Categories");

            Dictionary<Int32, CategoryCollection> categories = GetCategories();

            if (categories != null && categories.Count > 0)
            {
                foreach (KeyValuePair<Int32, CategoryCollection> keyValuePair in categories)
                {
                    if (keyValuePair.Value != null)
                    {
                        foreach (Category category in keyValuePair.Value)
                        {
                            xmlWriter.WriteStartElement("Category");
                            xmlWriter.WriteAttributeString("Id", category.Id.ToString());
                            xmlWriter.WriteAttributeString("Name", category.Name);
                            xmlWriter.WriteAttributeString("LongName", category.LongName);
                            xmlWriter.WriteEndElement();
                        }
                    }
                }
            }
            else
            {
                xmlWriter.WriteAttributeString("Categories", "No categories available in job service cache.");
            }

            xmlWriter.WriteEndElement();

            #endregion write Categories Xml

            #region Write Base Attribute model Xml

            xmlWriter.WriteStartElement("BaseAttributeModels");

            AttributeModelCollection baseAttributeModels = GetAllBaseAttributeModels();

            if (baseAttributeModels != null && baseAttributeModels.Count > 0)
            {
                foreach (AttributeModel attributeModel in baseAttributeModels)
                {
                    xmlWriter.WriteRaw(attributeModel.ToXml());
                }
            }
            else
            {
                xmlWriter.WriteAttributeString("Base Attribute Model", "No base attribute models are available in job service cache.");
            }

            xmlWriter.WriteEndElement();

            #endregion write Base Attribute model  Xml

            #region Write Contextual Attribute model Xml

            xmlWriter.WriteStartElement("ContextualAttributeModels");

            //TODO:: Not implemented
            xmlWriter.WriteAttributeString("Contextual Attribute Model", "No contextual attribute models are available in job service cache.");

            //if (this._contextualAttributeModels != null && this._contextualAttributeModels.Count > 0)
            //{
            //    foreach (KeyValuePair<AttributeModelContext, AttributeModelCollection> pair in this._contextualAttributeModels)
            //    {
            //        xmlWriter.WriteStartElement("ContextualAttributeModel");

            //        xmlWriter.WriteStartElement("AttributeModelConext");
            //        AttributeModelContext attributeModelContext = pair.Key;
            //        AttributeModelCollection attributeModelCollection = pair.Value;
            //        xmlWriter.WriteRaw(attributeModelContext.ToXml());
            //        xmlWriter.WriteEndElement();

            //        xmlWriter.WriteStartElement("AttributeModels");
            //        foreach (AttributeModel attributeModel in attributeModelCollection)
            //        {
            //            xmlWriter.WriteRaw(attributeModel.ToXml());
            //        }
            //        xmlWriter.WriteEndElement();
            //        xmlWriter.WriteEndElement();
            //    }
            //}
            //else
            //{
            //    xmlWriter.WriteAttributeString("Contextual Attribute Model", "No contextual attribute models are available in job service cache.");
            //}

                    xmlWriter.WriteEndElement();

            #endregion write Contextual  Attribute model  Xml

            #region Write lookup collection Xml

            xmlWriter.WriteStartElement("Lookuptables");

            //TODO:: Not implemented
                xmlWriter.WriteAttributeString("Lookuptables", "No look up tables are available in job service cache.");

            //if (this._lookupTableCollection != null && this._lookupTableCollection.Count > 0)
            //{
            //    foreach (KeyValuePair<String, Lookup> pair in this._lookupTableCollection)
            //    {
            //        xmlWriter.WriteStartElement("LookupCollection");

            //        xmlWriter.WriteStartElement("LookupKey");
            //        xmlWriter.WriteRaw(pair.Key);
            //        xmlWriter.WriteEndElement(); // for the lookup key
            //        xmlWriter.WriteStartElement("LookupTable");
            //        Lookup lookupTable = pair.Value;
            //        xmlWriter.WriteRaw(lookupTable.ToXml());
            //        xmlWriter.WriteEndElement(); // for the lookup table
            //        xmlWriter.WriteEndElement(); // for the collection
            //    }
            //}
            //else
            //{
            //    xmlWriter.WriteAttributeString("Lookuptables", "No look up tables are available in job service cache.");
            //}

            xmlWriter.WriteEndElement(); // for the lookup tables..

            #endregion write lookup collection Xml

            xmlWriter.WriteEndElement();
            xmlWriter.Flush();

            //get the actual XML
            String cacheModeXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return cacheModeXml;
        }

        #endregion

        #region Locale

        /// <summary>
        /// Load all the locales available in the system
        /// </summary>
        /// <returns>Return the list of locales</returns>
        public Collection<LocaleEnum> GetAvailableLocaleValues()
        {
            TraceSettings traceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();

            DiagnosticActivity activity = new DiagnosticActivity();
            if (traceSettings.IsBasicTracingEnabled)
            {
                activity.Start();
            }

            String availableLocaleEnumsCacheKey = CacheKeyGenerator.GetAvailableLocaleEnumsCacheKey();

            Collection<LocaleEnum> locales = GetDataFromCache<Collection<LocaleEnum>>(availableLocaleEnumsCacheKey);
            if (locales == null)
            {
                IEnumerable<Locale> availableLocales = LoadDataModelFromDB<Locale>("Locales", () =>
                {
                    KnowledgeBaseService knowledgeBaseService = new KnowledgeBaseService();
                    return knowledgeBaseService.GetAvailableLocales();
                });

                if (availableLocales != null && availableLocales.Any())
                {
                    locales = new Collection<LocaleEnum>();
                    foreach (Locale locale in availableLocales)
                    {
                        locales.Add(locale.Locale);
                    }

                    SetDataInCache(availableLocaleEnumsCacheKey, locales);
                }
            }
            else if (traceSettings.IsBasicTracingEnabled)
            {
                activity.LogInformation(String.Format("{0} Locales loaded from cache", locales.Count));
            }

            if (traceSettings.IsBasicTracingEnabled)
            {
                activity.Stop();
            }

            return locales;
        }

        #endregion 

        #endregion

        #region Private Methods

        private void LoadAll()
                {
            string message = string.Empty;
            var traceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();

            DiagnosticActivity activity = new DiagnosticActivity();
            if (traceSettings.IsBasicTracingEnabled) activity.Start();

            WriteEventLogMessage("LoadAll for CachedDataModel starting...", MessageClassEnum.Information);

                _cacheManager = CacheFactory.GetCache();
                _cacheSynchronizationHelper = new CacheSynchronizationHelper();

            Int32 totalSteps = 10;
            Int32 stepNumber = 1;

            //String userName = "system";

            // Always start with loading security principal of master user to do boot strap..
            //String message = String.Format("Step {0} of {1}. Loading Security Principal...", stepNumber++, totalSteps);
            //WriteEventLogMessage(message, MessageClassEnum.Information);
            //this.LoadSecurityPrincipal(userName);

            message = String.Format("Step {0} of {1}. Loading System Locale Details...", stepNumber++, totalSteps);
            if (traceSettings.IsBasicTracingEnabled) activity.LogInformation(message);
            WriteEventLogMessage(message, MessageClassEnum.Information);
            this.LoadSystemLocaleDetails();

            // Loads user permissions of master user..
            message = String.Format("Step {0} of {1}. Loading User Permissions...", stepNumber++, totalSteps);
            if (traceSettings.IsBasicTracingEnabled) activity.LogInformation(message);
            WriteEventLogMessage(message, MessageClassEnum.Information);
            this.LoadUserPermissions();

            //Here, calling get methods will end up loading all objects if they dont exist into this static instance..
            message = String.Format("Step {0} of {1}. Loading Hierarchies...", stepNumber++, totalSteps);
            if (traceSettings.IsBasicTracingEnabled) activity.LogInformation(message);
            WriteEventLogMessage(message, MessageClassEnum.Information);
            this.GetHierarchies();

            message = String.Format("Step {0} of {1}. Loading Organizations...", stepNumber++, totalSteps);
            if (traceSettings.IsBasicTracingEnabled) activity.LogInformation(message);
            WriteEventLogMessage(message, MessageClassEnum.Information);
            this.GetOrganizations();

            message = String.Format("Step {0} of {1}. Loading Containers...", stepNumber++, totalSteps);
            if (traceSettings.IsBasicTracingEnabled) activity.LogInformation(message);
            WriteEventLogMessage(message, MessageClassEnum.Information);
            this.GetContainers();

            message = String.Format("Step {0} of {1}. Loading Categories...", stepNumber++, totalSteps);
            if (traceSettings.IsBasicTracingEnabled) activity.LogInformation(message);
            WriteEventLogMessage(message, MessageClassEnum.Information);
            this.GetCategories();

            message = String.Format("Step {0} of {1}. Loading Entity Types...", stepNumber++, totalSteps);
            if (traceSettings.IsBasicTracingEnabled) activity.LogInformation(message);
            WriteEventLogMessage(message, MessageClassEnum.Information);
            this.GetEntityTypes();

            message = String.Format("Step {0} of {1}. Loading Relationship Types...", stepNumber++, totalSteps);
            if (traceSettings.IsBasicTracingEnabled) activity.LogInformation(message);
            WriteEventLogMessage(message, MessageClassEnum.Information);
            this.GetRelationshipTypes();

            message = String.Format("Step {0} of {1}. Loading Base Attribute Models...", stepNumber++, totalSteps);
            if (traceSettings.IsBasicTracingEnabled) activity.LogInformation(message);
            WriteEventLogMessage(message, MessageClassEnum.Information);
            AttributeModelCollection baseAttributeModelCollection = this.GetAllBaseAttributeModels();

            if (baseAttributeModelCollection != null && baseAttributeModelCollection.Count > 0)
            {
                WriteEventLogMessage(String.Format("{0} base attribute models loaded.", baseAttributeModelCollection.Count), MessageClassEnum.Information);
            }

            message = String.Format("Step {0} of {1}. Loading UOM Collections ...", stepNumber++, totalSteps);
            if (traceSettings.IsBasicTracingEnabled) activity.LogInformation(message);
            WriteEventLogMessage(message, MessageClassEnum.Information);
            this.LoadUOMCollection();

            message = "LoadAll for CachedDataModel completed.";
            if (traceSettings.IsBasicTracingEnabled) activity.LogInformation(message);
            WriteEventLogMessage(message, MessageClassEnum.Information);
        }

        private void LoadSystemLocaleDetails()
        {
            ApplicationConfigurationBL applicationConfigurationBL = new ApplicationConfigurationBL();
            var localeConfiguration = applicationConfigurationBL.GetLocaleApplicationConfigurations(new ApplicationConfiguration((Int32)EventSource.MDMCenter, (Int32)EventSubscriber.LocaleConfiguration, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0));

            //Default system locale is en_WW
            LocaleEnum systemDataLocale = LocaleEnum.en_WW;
            LocaleEnum systemUILocale = LocaleEnum.en_WW;
            LocaleType modelDisplayLocaleType = LocaleType.DataLocale;
            LocaleType dataFormattingLocaleType = LocaleType.DataLocale;

            if (localeConfiguration != null && localeConfiguration.Count > 0 && localeConfiguration.Keys.Contains("LocaleConfiguration"))
            {
                if (localeConfiguration["LocaleConfiguration"] is LocaleConfig)
                {
                    ILocaleConfig localeConfig = localeConfiguration["LocaleConfiguration"];

                    systemDataLocale = localeConfig.SystemDataLocale;
                    systemUILocale = localeConfig.SystemUILocale;
                    modelDisplayLocaleType = localeConfig.ModelDisplayLocaleType;
                    dataFormattingLocaleType = localeConfig.DataFormattingLocaleType;
                }
            }

            //Locale Config List: 0th position is system data locale; 1st position is system UI locale; 2nd position is model display locale type and 3rd position is data formatting locale type;
            String[] locales = new String[] { systemDataLocale.ToString(), systemUILocale.ToString(), modelDisplayLocaleType.ToString(), dataFormattingLocaleType.ToString() }; 

            String cacheKey = "RS_SDL";
            ICache cache = CacheFactory.GetCache();

            cache.Set(cacheKey, locales, DateTime.Now.AddHours(1));

            MDM.Utility.GlobalizationHelper.LoadSystemLocales();
        }

        #region DataModel Load Methods

        #region Attribute Model Methods

        private ConcurrentDictionary<AttributeModelContext, AttributeModelCollection> LoadAllContextualAttributeModels()
        {
            string message = string.Empty;
            var traceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();

            DiagnosticActivity activity = new DiagnosticActivity();
            if (traceSettings.IsBasicTracingEnabled) activity.Start();

            ConcurrentDictionary<AttributeModelContext, AttributeModelCollection> localAttributeModels = new ConcurrentDictionary<AttributeModelContext, AttributeModelCollection>();

            try
            {
                ContainerEntityTypeMappingBL containerEntityTypeMappingManager = new ContainerEntityTypeMappingBL();
                AttributeModelBL attributeModelBL = new AttributeModelBL();

                Collection<Int32> containerIdList = new Collection<Int32>();
                Collection<Int32> entityTypeIdList = new Collection<Int32>();

                #region Get Container List

                //Get AppConfig value for the list of container id
                String containerFromConfig = AppConfigurationHelper.GetAppConfig<String>("MDMCenter.CachedDataModel.AttributeModelCache.PreLoad.ContainerIdList");
                String[] strContainerIdList = containerFromConfig.Split('|');

                if (String.IsNullOrEmpty(containerFromConfig) || strContainerIdList == null || strContainerIdList.Count() == 0)
                {
                    // no container is specified in the app config. load for all containers.
                    if (traceSettings.IsBasicTracingEnabled)
                    {
                        message = "No container information is available in app config for pre loading. Contextual attribute model will load common attributes for all containers.";
                        activity.LogWarning(message);
                    }

                    // When the app config is not available, load all containers from the database.
                    List<Container> containers = this.GetContainers();

                    foreach (Container container in containers)
                    {
                        containerIdList.Add(container.Id);
                    }
                }
                else
                {
                    foreach (String strContainerId in strContainerIdList)
                    {
                        Int32 containerId = ValueTypeHelper.Int32TryParse(strContainerId, 0);

                        if (containerId > 0)
                            containerIdList.Add(containerId);
                    }
                }

                if (traceSettings.IsBasicTracingEnabled)
                {
                    message = String.Format("The container list that will be preloaded from the database is {0}", containerIdList);
                    activity.LogInformation(message);
                }

                #endregion

                #region Get EntityType List

                //Get AppConfig value for the list of entity type
                String entityTypeFromConfig = AppConfigurationHelper.GetAppConfig<String>("MDMCenter.CachedDataModel.AttributeModelCache.PreLoad.EntityTypeIdList");
                String[] strEntityTypeIdList = entityTypeFromConfig.Split('|');

                if (String.IsNullOrEmpty(entityTypeFromConfig) || strEntityTypeIdList == null || strEntityTypeIdList.Count() <= 0)
                {
                    // no entity type is specified in the app config. load for all entity types.
                    if (traceSettings.IsBasicTracingEnabled)
                        activity.LogWarning("No entity type information is available in app config for pre loading. Contextual attribute model load for common attributes for all entity types.");

                    // When the app config is not available, load all entity types from the database.
                    List<EntityType> entityTypes = this.GetEntityTypes();

                    foreach (EntityType entityType in entityTypes)
                    {
                        entityTypeIdList.Add(entityType.Id);
                    }
                }
                else
                {
                    foreach (String strEntityTypeId in strEntityTypeIdList)
                    {
                        Int32 entityTypeId = ValueTypeHelper.Int32TryParse(strEntityTypeId, 0);

                        if (entityTypeId > 0)
                            entityTypeIdList.Add(entityTypeId);
                    }
                }

                if (traceSettings.IsBasicTracingEnabled)
                {
                    message = String.Format("The entity type list that will be preloaded from the database is {0}", entityTypeIdList);
                    activity.LogInformation(message);
                }

                #endregion

                #region Get System Data Locale

                Collection<LocaleEnum> locales = new Collection<LocaleEnum>();

                locales.Add(MDM.Utility.GlobalizationHelper.GetSystemDataLocale());

                #endregion

                #region Get All Relationship Types

                List<RelationshipType> relationshipTypes = this.GetRelationshipTypes();

                #endregion

                #region Load All Contextual Common Attributes

                //Load common attributes per container, entity type and locale..
                if (containerIdList.Count > 0 && entityTypeIdList.Count > 0 && locales.Count > 0)
                {
                    foreach (Int32 containerId in containerIdList)
                    {
                        foreach (Int32 entityTypeId in entityTypeIdList)
                        {
                            foreach (LocaleEnum locale in locales)
                            {
                                AttributeModelContext context = new AttributeModelContext(containerId, entityTypeId, 0, 0, new Collection<LocaleEnum>() { locale }, 0, AttributeModelType.Common, false, false, true);

                                AttributeModelCollection attributeModelCollection = attributeModelBL.Get(context) as AttributeModelCollection;
                                if (attributeModelCollection != null)
                                {
                                    if (traceSettings.IsBasicTracingEnabled)
                                    {
                                        message = attributeModelCollection.Count + " common attributes loaded for ContainerId:" + containerId + " EntityTypeId:" + entityTypeId + " Locale:" + locale.ToString();
                                        activity.LogInformation(message);
                                    }

                                    localAttributeModels.TryAdd(context, attributeModelCollection);
                                }
                            }
                        }
                    }
                }

                #endregion

                #region Load All Contextual Tech Attributes

                String strLoadTechAttributes = AppConfigurationHelper.GetAppConfig<String>("MDMCenter.CachedDataModel.AttributeModelCache.LoadTechnicalAttributes.Enabled");

                Boolean loadTechAttributes = false;

                if (!String.IsNullOrWhiteSpace(strLoadTechAttributes))
                    loadTechAttributes = ValueTypeHelper.BooleanTryParse(strLoadTechAttributes, loadTechAttributes);

                if (traceSettings.IsBasicTracingEnabled)
                {
                    message = "Technical attributes load flag (LoadTechnicalAttributes) is set to " + loadTechAttributes.ToString();
                    activity.LogInformation(message);
                }

                if (loadTechAttributes)
                {
                    #region Get All Categories

                    Dictionary<Int32, CategoryCollection> categories = this.GetCategories();

                    #endregion

                    // Load technical attributes
                    if (categories != null && categories.Count > 0 && locales.Count > 0)
                    {
                        foreach (KeyValuePair<Int32, CategoryCollection> keyValuePair in categories)
                        {
                            if (keyValuePair.Value != null)
                            {
                                foreach (Category category in keyValuePair.Value)
                                {
                                    foreach (LocaleEnum locale in locales)
                                    {
                                        AttributeModelContext context = new AttributeModelContext(0, 0, 0, category.Id, new Collection<LocaleEnum>() { locale }, 0, AttributeModelType.Category, false, false, true);

                                        AttributeModelCollection attributeModelCollection = attributeModelBL.Get(context) as AttributeModelCollection;
                                        if (attributeModelCollection != null)
                                        {
                                            if (traceSettings.IsBasicTracingEnabled)
                                            {
                                                message = string.Format("{0} technical attributes loaded for CategoryId: {1}, Locale: {2}", attributeModelCollection.Count, category.Id, locale.ToString());
                                                activity.LogInformation(message);
                                            }

                                            localAttributeModels.TryAdd(context, attributeModelCollection);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (traceSettings.IsBasicTracingEnabled)
                    {
                        message = "Skipping technical attributes load as load flag is set to false";
                        activity.LogInformation(message);
                    }
                }

                #endregion

                #region Load All Contextual Relationship Attributes

                //Load relationship attributes per container, relationship type and locale..
                if (containerIdList.Count > 0 && relationshipTypes != null && relationshipTypes.Count > 0 && locales.Count > 0)
                {
                    foreach (Int32 containerId in containerIdList)
                    {
                        foreach (RelationshipType relationshipType in relationshipTypes)
                        {
                            foreach (LocaleEnum locale in locales)
                            {
                                AttributeModelContext context = new AttributeModelContext(containerId, 0, relationshipType.Id, 0, new Collection<LocaleEnum>() { locale }, 0, AttributeModelType.Relationship, false, false, true);

                                AttributeModelCollection attributeModelCollection = attributeModelBL.Get(context);
                                if (attributeModelCollection != null)
                                {
                                    if (traceSettings.IsBasicTracingEnabled)
                                    {
                                        message = string.Format("{0} relationship attributes loaded for ContainerId: {1}, RelationshipTypeId: {2}, Locale: {3}", attributeModelCollection.Count, containerId, relationshipType.Id, locale.ToString());
                                        activity.LogInformation(message);
                                    }

                                    localAttributeModels.TryAdd(context, attributeModelCollection);
                                }
                            }
                        }
                    }
                }

                #endregion

            }
            catch (Exception ex)
            {
                message = "CachedDataModel: Not able to load attribute models. Error: " + ex.Message;

                activity.LogError(message);
                throw new Exception(message, ex);
            }

            if (traceSettings.IsBasicTracingEnabled) activity.Stop();

            return localAttributeModels;
        }

        private AttributeModelCollection GetAttributeModelsUsingBL(AttributeModelContext attributeModelContext)
        {
            AttributeModelBL attributeModelBL = new AttributeModelBL();
            AttributeModelCollection models = attributeModelBL.Get(attributeModelContext);
            return models;
            }

        #endregion

        #region Lookup Methods

        private LookupCollection GetLookups(String lookupName)
        {
            // This will method will be get called as parallel based on the thread poll size
            LookupCollection result = new LookupCollection();

            if (_localeNames != null)
                {
                foreach (LocaleEnum locale in _localeNames)
                    {
                    result.Add(this.GetLookup(lookupName, locale));
            }
        }

            return result;
        }

        #endregion Lookup Methods

        #region Category Methods

        private Category GetCategory(Int32 hierarchyId, Func<CategoryCollection, IEnumerable<Category>> comparisonMethod)
        {
            String message = String.Empty;
            TraceSettings traceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();

            DiagnosticActivity activity = new DiagnosticActivity();
            if (traceSettings.IsBasicTracingEnabled)
            {
                activity.Start();
            }

            Category category = null;
            CategoryCollection categories = this.GetCategories(hierarchyId);

            if (categories != null && categories.Count > 0)
            {
                //If more than one category present then will throw error. This will avoid the duplicates. But it is not possible in real scenario.
                var categoryList = comparisonMethod.Invoke(categories);
                if (categoryList.Any())
        {
                    category = categoryList.FirstOrDefault();

                    if (traceSettings.IsBasicTracingEnabled && categoryList.Count() > 1)
            {
                        message = String.Format("Multiple categories found, proceeding with first category ({0}) - {1}",
                            category.Id, String.Join(", ", categoryList.Select(x => String.Format("{0}({1})", x.Name, x.Id)).ToList()));

                        activity.LogWarning(message);
                }
            }
            }

            if (traceSettings.IsBasicTracingEnabled)
            {
                activity.Stop();
            }

            return category;
            }

        private Hierarchy GetHierarchyByName(String hierarchyName)
            {
            List<Hierarchy> allHierarchies = this.GetHierarchies();

            Hierarchy hierarchy = allHierarchies.FirstOrDefault(hr => String.Compare(hr.Name, hierarchyName, StringComparison.InvariantCultureIgnoreCase) == 0);
            if (hierarchy == null)
            {
                //try to find Hierarchy by long name..
                hierarchy = allHierarchies.FirstOrDefault(hr => String.Compare(hr.LongName, hierarchyName, StringComparison.InvariantCultureIgnoreCase) == 0);
            }

            return hierarchy;
        }

        #endregion Category Methods

        #region Generic DataModel Get Methods

        /// <summary>
        /// Gets a list of data model object if available in cache, else the data is retrieved using the method delegate and returned
        /// </summary>
        /// <returns>List of data model objects</returns>
        private List<TDataModel> GetDataModelListFromCacheOrDB<TDataModel>(String dataModelType, String cacheKey, Func<IEnumerable<TDataModel>> method)
        {
            TraceSettings traceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();

            DiagnosticActivity activity = new DiagnosticActivity();
            if (traceSettings.IsBasicTracingEnabled)
            {
                activity.Start();
            }

            List<TDataModel> modelData = GetDataFromCache<List<TDataModel>>(cacheKey);
            if (modelData == null)
            {
                IEnumerable<TDataModel> callResult = LoadDataModelFromDB<TDataModel>(dataModelType, method);
                if (callResult != null && callResult.Any())
                {
                    modelData = callResult.ToList();
                    SetDataInCache<List<TDataModel>>(cacheKey, modelData);
                }
            }
            else if (traceSettings.IsBasicTracingEnabled)
            {
                activity.LogInformation(String.Format("{0} {1} loaded from cache", modelData.Count, dataModelType));
            }

            if (traceSettings.IsBasicTracingEnabled)
            {
                activity.Stop();
            }

            return modelData;
        }

        /// <summary>
        /// Loads the requested data model using the method delegate
        /// </summary>
        /// <returns></returns>
        private IEnumerable<TDataModel> LoadDataModelFromDB<TDataModel>(String dataModelType, Func<IEnumerable<TDataModel>> method)
        {
            String message = String.Empty;
            TraceSettings traceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();

            var activity = new DiagnosticActivity();
            if (traceSettings.IsBasicTracingEnabled)
            {
                activity.Start();
            }

            IEnumerable<TDataModel> dataModelCollection = null;

            try
            {
                dataModelCollection = method.Invoke();
            }
            catch (Exception ex)
            {
                message = String.Format("CachedDataModel: Not able to load {0}. Error: {1}", dataModelType, ex.Message);

                activity.LogError(message);
                throw new Exception(message);
            }

            if (dataModelCollection == null || !dataModelCollection.Any())
            {
                message = String.Format(defaultErrorMessage, dataModelType);

                activity.LogError(message);
                WriteEventLogMessage(message, MessageClassEnum.Error);
            }
            else
            {
                message = String.Format("{0} {1} loaded.", dataModelCollection.Count(), dataModelType);

                if (traceSettings.IsBasicTracingEnabled)
                        {
                    activity.LogInformation(message);
            }
                WriteEventLogMessage(message, MessageClassEnum.Information);
        }

            if (traceSettings.IsBasicTracingEnabled)
        {
                activity.Stop();
        }

            return dataModelCollection;
            }

        #endregion Generic DataModel Get Methods

        #endregion DataModel Load Methods

        #region Cache Helper Methods

        /// <summary>
        /// Returns the data from cache for the specified cache key.
        /// </summary>
        private T GetDataFromCache<T>(String cacheKey)
        {
            T data = default(T);
            try
            {
                data = _cacheManager.Get<T>(cacheKey);
            }
            catch (Exception ex)
            {
                String message = String.Format("Error occurred while retrieving data for key {0}. Internal error : {1}", cacheKey, ex.Message);

                DiagnosticActivity activity = new DiagnosticActivity();
                activity.LogError(message);

                ExceptionHandler exceptionHandler = new ExceptionHandler(ex);
            }
            return data;
        }

        /// <summary>
        /// Inserts the data to cache for the specified cache key.
        /// </summary>
        private void SetDataInCache<T>(String cacheKey, T data)
        {
            String message = String.Empty;
            TraceSettings traceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();

            DiagnosticActivity activity = new DiagnosticActivity();
            if (traceSettings.IsBasicTracingEnabled)
            {
                activity.Start();
                activity.OperationId = Constants.ProfileTracingOperationId;
            }

            if (data != null)
            {
                try
                {
                    DateTime expirationTime = DateTime.Now.AddDays(5);

                    _cacheSynchronizationHelper.NotifyLocalCacheInsert(cacheKey, expirationTime, false);
                    _cacheManager.Set<T>(cacheKey, data, expirationTime);
                }
                catch (Exception ex)
                {
                    message = String.Format("Error occurred while inserting {0} into cache. Internal error : {1}", data.GetType(), ex.Message);

                    activity.LogError(message);
                    ExceptionHandler exceptionHandler = new ExceptionHandler(ex);
                }
        }

            if (traceSettings.IsBasicTracingEnabled)
            {
                activity.Stop();
            }
        }

        #endregion 

        #region Logger Helper Method

        private void WriteEventLogMessage(String message, MessageClassEnum messageClass)
                    {
            Console.WriteLine(message);
            switch (messageClass)
            {
                case MessageClassEnum.Error:
                    LogHandler.WriteErrorLog(message, 100);
                    break;
                case MessageClassEnum.Warning:
                    LogHandler.WriteWarningLog(message, 50);
                    break;
                case MessageClassEnum.Information:
                    LogHandler.WriteInformationLog(message, 10);
                    break;
                default:
                    break;
            }
        }

        #endregion  Logger Helper Method

        #endregion Private Methods
    }
}