using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;

namespace MDM.WCFServices
{
    using Core;
    using Core.Exceptions;
    using BusinessObjects;
    using EntityManager.Business;
    using ExceptionManager;
    using SearchManager.Business;
    using WCFServiceInterfaces;

    [ServiceBehavior(Namespace = "http://wcfservices.riversand.com", InstanceContextMode = InstanceContextMode.PerCall)]
    public class InteropEntityService : IInteropEntityService
    {
        #region Fields

        /// <summary>
        /// 
        /// </summary>
        private CallerContext _callerContext = new CallerContext(MDMCenterApplication.PIM, MDMCenterModules.Entity);

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public String GetEntity(String name)
        {
            EntityContext entityContext = new EntityContext() 
                {
                    LoadEntityProperties = true,
                    LoadAttributes = true,
                    AttributeModelType = AttributeModelType.All,
                    LoadAttributeModels = true,
                    LoadRelationships = true,
                    LoadWorkflowInformation = true,
                    LoadHierarchyRelationships = false,
                    LoadExtensionRelationships = false,
                    DataLocales = new Collection<LocaleEnum> {LocaleEnum.en_WW},
                    Locale = LocaleEnum.en_WW,
                    LoadLookupDisplayValues = true
                };

            if (entityContext.RelationshipContext != null)
            {
                entityContext.RelationshipContext.LoadRelationshipAttributes = true;
            }

            return GetEntityXml(name, entityContext);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public String GetEntityAttributeValues(String name)
        {
            EntityContext entityContext = new EntityContext()
            {
                LoadEntityProperties = true,
                LoadAttributes = true,
                AttributeModelType = AttributeModelType.All,
                LoadAttributeModels = true,
                LoadRelationships = false,
                LoadHierarchyRelationships = false,
                LoadExtensionRelationships = false,
                LoadWorkflowInformation = false,
                DataLocales = new Collection<LocaleEnum> { LocaleEnum.en_WW },
                Locale = LocaleEnum.en_WW,
                LoadLookupDisplayValues = true
            };

            if (entityContext.RelationshipContext != null)
            {
                entityContext.RelationshipContext.LoadRelationshipAttributes = false;
            }

            return GetEntityXml(name, entityContext);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parentEntityName"></param>
        /// <returns></returns>
        public String GetEntityChildren(String parentEntityName)
        {
            String entityXml = String.Empty;

            try
            {
                Collection<Int32> attrids = new Collection<Int32>();

                EntityContext parentEntityContext = new EntityContext()
                {
                    LoadEntityProperties = true,
                    LoadAttributes = false,
                    AttributeModelType = AttributeModelType.All,
                    LoadAttributeModels = false,
                    LoadRelationships = false,
                    LoadHierarchyRelationships = false,
                    LoadExtensionRelationships = false,
                    LoadWorkflowInformation = false,
                    DataLocales = new Collection<LocaleEnum> { LocaleEnum.en_WW },
                    Locale = LocaleEnum.en_WW,
                    LoadLookupDisplayValues = false
                };
                _callerContext.ProgramName = "MDM.WCFServices.InteropEntityService.GetEntityChildren";

                if (parentEntityContext.RelationshipContext != null)
                {
                    parentEntityContext.RelationshipContext.LoadRelationshipAttributes = false;
                }

                Entity parentEntity = GetEntityObject(parentEntityName, parentEntityContext);

                if (parentEntity != null)
                {
                    var entityHierarchyBL = new EntityHierarchyBL();
                    EntityCollection childEntites = entityHierarchyBL.GetChildEntities(parentEntity.Id, 0, LocaleEnum.en_WW, attrids, true, 1000, _callerContext, true);

                    if (childEntites != null)
                    {
                        entityXml = childEntites.ToXml(ObjectSerialization.External);
                    }
                }
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw this.WrapException(ex);
            }

            return entityXml;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public String GetEntityRelationships(String name)
        {
            EntityContext entityContext = new EntityContext()
            {
                LoadEntityProperties = true,
                LoadAttributes = false,
                AttributeModelType = AttributeModelType.All,
                LoadAttributeModels = false,
                LoadRelationships = true,
                LoadHierarchyRelationships = false,
                LoadExtensionRelationships = false,
                LoadWorkflowInformation = false,
                DataLocales = new Collection<LocaleEnum> { LocaleEnum.en_WW },
                Locale = LocaleEnum.en_WW,
                LoadLookupDisplayValues = true
            };

            if (entityContext.RelationshipContext != null)
            {
                entityContext.RelationshipContext.LoadRelationshipAttributes = true;
            }

            return GetEntityXml(name, entityContext);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public String GetEntityExtensions(String parentEntityName)
        {
            String entityXml = String.Empty;

            try
            {
                Collection<Int32> attrids = new Collection<Int32>();

                EntityContext parentEntityContext = new EntityContext()
                {
                    LoadEntityProperties = true,
                    LoadAttributes = false,
                    AttributeModelType = AttributeModelType.All,
                    LoadAttributeModels = false,
                    LoadRelationships = false,
                    LoadHierarchyRelationships = false,
                    LoadExtensionRelationships = true,
                    LoadWorkflowInformation = false,
                    DataLocales = new Collection<LocaleEnum> { LocaleEnum.en_WW },
                    Locale = LocaleEnum.en_WW,
                    LoadLookupDisplayValues = false
                };

                if (parentEntityContext.RelationshipContext != null)
                {
                    parentEntityContext.RelationshipContext.LoadRelationshipAttributes = true;
                }

                EntityContext extendedEntityContext = (EntityContext) parentEntityContext.Clone();


                Entity parentEntity = GetEntityObject(parentEntityName, parentEntityContext);

                EntityCollection extendedEntities = new EntityCollection();

                if (parentEntity != null && parentEntity.ExtensionRelationships != null && parentEntity.ExtensionRelationships.Count > 0)
                {
                    IList<Int64> extendedEntitiyIdList = parentEntity.ExtensionRelationships.Select(e => e.RelatedEntityId).ToList();

                    Collection<Int64> entityIdList = new Collection<Int64>(extendedEntitiyIdList);

                    EntityBL entityManager = new EntityBL();
                    extendedEntities = entityManager.Get(entityIdList, extendedEntityContext, false, _callerContext.Application, _callerContext.Module, false, false, false);
                }

                if (extendedEntities != null)
                {
                    entityXml = extendedEntities.ToXml(ObjectSerialization.External);
                }
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw this.WrapException(ex);
            }

            return entityXml;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public String GetEntityWorkFlowDetails(String name)
        {
            String entityXml = String.Empty;

            EntityContext entityContext = new EntityContext()
            {
                LoadEntityProperties = true,
                LoadAttributes = false,
                AttributeModelType = AttributeModelType.All,
                LoadAttributeModels = false,
                LoadRelationships = false,
                LoadHierarchyRelationships = false,
                LoadExtensionRelationships = false,
                LoadWorkflowInformation = true,
                DataLocales = new Collection<LocaleEnum> { LocaleEnum.en_WW },
                Locale = LocaleEnum.en_WW,
                LoadLookupDisplayValues = true
            };

            if (entityContext.RelationshipContext != null)
            {
                entityContext.RelationshipContext.LoadRelationshipAttributes = true;
            }

            Entity entity = GetEntityObject(name, entityContext);

            if(entity != null)
            {
                entityXml = entity.ToXml(); //This would write WF info too. External xml wont write
            }

            return entityXml;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public String GetEntityHistory(String name)
        {
            String entityHistoryXml = String.Empty;

            EntityContext entityContext = new EntityContext() { LoadEntityProperties = true, DataLocales = new Collection<LocaleEnum> { LocaleEnum.en_WW }, Locale = LocaleEnum.en_WW };
            _callerContext.ProgramName = "MDM.WCFServices.InteropEntityService.GetEntityHistory";

            Entity entity = GetEntityObject(name, entityContext);

            if(entity != null)
            {
                DataService dataService = new DataService();

                EntityHistoryContext entityHistoryContext = new EntityHistoryContext { ContainerId = entity.ContainerId };

                //EntityHistory entityHistory = dataService.GetEntityHistory(entity.Id, entityHistoryContext, _callerContext);

                //if (entityHistory != null)
                //    entityHistoryXml = entityHistory.ToXml();
            }

            return entityHistoryXml;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <param name="loadAttrbutes"></param>
        /// <param name="loadRelationships"></param>
        /// <param name="loadWorkflowInformation"></param>
        /// <param name="loadAllRequiredAttributes"></param>
        /// <param name="attributeIdList"></param>
        /// <param name="dataLocale"></param>
        /// <returns></returns>
        public String GenerateEntityContextXml(Boolean loadAttributes, Boolean loadChildren, Boolean loadRelationships, Boolean loadWorkflowInformation, Boolean loadOnlyRequiredAttributes, String attributeIdList, String dataLocale)
        {
            Collection<Int32> attrIdList = new Collection<Int32>();
            if(!String.IsNullOrWhiteSpace(attributeIdList))
            {
                attrIdList = ValueTypeHelper.SplitStringToIntCollection(attributeIdList, ',');
            }

            LocaleEnum locale = LocaleEnum.en_WW;
            if(!String.IsNullOrWhiteSpace(dataLocale))
            {
                Enum.TryParse<LocaleEnum>(dataLocale, true, out locale);
            }

            EntityContext entityContext = new EntityContext()
            {
                LoadEntityProperties = true,
                LoadAttributes = loadAttributes,
                AttributeModelType = AttributeModelType.All,
                AttributeIdList = attrIdList,
                LoadRequiredAttributes = loadOnlyRequiredAttributes,
                LoadAttributeModels = true,
                LoadRelationships = loadRelationships,
                LoadWorkflowInformation = loadWorkflowInformation,
                LoadHierarchyRelationships = loadChildren,
                LoadExtensionRelationships = false,
                DataLocales = new Collection<LocaleEnum> { locale },
                Locale = locale,
                LoadLookupDisplayValues = true
            };

            if (entityContext.RelationshipContext != null)
            {
                entityContext.RelationshipContext.LoadRelationshipAttributes = true;
            }

            return entityContext.ToXml();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <param name="name"></param>
        /// <param name="entityContext"></param>
        /// <param name="outputFormat"></param>
        /// <returns></returns>
        public String GetEntityAdvanced(String name, String entityContext, String outputFormat)
        {
            String entityData = String.Empty;

            EntityContext entityContextObj = new EntityContext(entityContext);

            if (outputFormat == "json")
            {
                Entity entity = GetEntityObject(name, entityContextObj);
                entityData = entity.ToXml(); //TODO:: cast entity object into json string
            }
            else
            {
                entityData = GetEntityXml(name, entityContextObj);
            }

            return entityData;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <param name="entityId"></param>
        /// <param name="entityContext"></param>
        /// <returns></returns>
        public String GetEntityById(Int64 entityId, String entityContext, String outputFormat)
        {
            String entityData = String.Empty;

            EntityContext entityContextObj = new EntityContext(entityContext);

            if (outputFormat == "json")
            {
                Entity entity = GetEntityObject(entityId, entityContextObj);
                entityData = entity.ToXml(); //TODO:: cast entity object into json string
            }
            else
            {
                Entity entity = GetEntityObject(entityId, entityContextObj);

                if (entity != null)
                    entityData = entity.ToXml(ObjectSerialization.External);
            }
             
            return entityData;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <param name="searchCriteria"></param>
        /// <param name="searchContext"></param>
        /// <returns></returns>
        public String SearchEntities(String searchCriteria, String searchContext)
        {
            String entityXml = String.Empty;

            try
            {
                EntityBL entityManager = new EntityBL();
                SearchCriteria entitySearchCriteria = new SearchCriteria(searchCriteria);
                SearchContext entitySearchContext = new SearchContext(searchContext);
                _callerContext.ProgramName = "MDM.WCFServices.InteropEntityService.SearchEntities";

                EntitySearchBL entitySearchBL = new EntitySearchBL(new EntityBL());
                SearchReadResult searchReadResult = entitySearchBL.SearchEntities(entitySearchCriteria, entitySearchContext, _callerContext);

                EntityCollection entities = searchReadResult.Entities;

                if (entities != null)
                {
                    entityXml = entities.ToXml();
                }
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw this.WrapException(ex);
            }

            return entityXml;
        }

        public String ProcessEntity(String entityXML)
        {
            EntityBL entityBL = new EntityBL();
            Entity entity = new Entity(entityXML);
            EntityCollection entities = new EntityCollection();
            entities.Add(entity);

            EntityOperationResultCollection operationResultCollection = entityBL.Process(entities, "InteropEntityService", MDMCenterApplication.PIM, MDMCenterModules.Integration);

            if ( operationResultCollection != null && operationResultCollection.Count == 1)
            {
                return operationResultCollection.FirstOrDefault().ToXml();
            }
            else
            {
                EntityOperationResult operationResult = new EntityOperationResult(entity.Id, entity.Name, entity.LongName);
                operationResult.AddOperationResult("88888", "Entity process failed. Please retry the operation", OperationResultType.Error);
                return operationResult.ToXml();
            }
        }

        #region Private Methods

        /// <summary>
        /// Wrap the normal exception into a WCF fault
        /// </summary>
        /// <param name="ex">The exception</param>
        /// <returns>The Fault Exception of type WcfException</returns>
        private FaultException<MDMExceptionDetails> WrapException(Exception ex)
        {
            //TODO add the service url in the fault code
            MDMExceptionDetails fault = null;
            FaultReason reason = null;
            FaultException<MDMExceptionDetails> exception = null;

            //Get message code
            String messageCode = String.Empty;
            Object[] messageArguments = null;

            if (ex is MDMOperationException)
            {
                MDMOperationException mdmException = ex as MDMOperationException;

                messageCode = mdmException.MessageCode;
                messageArguments = mdmException.MessageArguments;
            }

            fault = new MDMExceptionDetails(messageCode, ex.Message, ex.StackTrace, ex.Source, ex.TargetSite.ToString(), messageArguments);

            reason = new FaultReason(ex.Message);

            exception = new FaultException<MDMExceptionDetails>(fault, reason);

            return exception;
        }

        /// <summary>
        /// Logs the exception into Event Log
        /// </summary>
        /// <param name="ex">Exception occurred</param>
        private void LogException(Exception ex)
        {
            try
            {
                ExceptionHandler exceptionHandler = new ExceptionHandler(ex);
            }
            catch
            {
                //Do not throw
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="entityContext"></param>
        /// <returns></returns>
        private Entity GetEntityObject(Int64 entityId, EntityContext entityContext)
        {
            Entity entity = null;

            try
            {
                EntityBL entityManager = new EntityBL();
                entity = entityManager.Get(entityId, entityContext, _callerContext.Application, _callerContext.Module, false, false);
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw this.WrapException(ex);
            }

            return entity;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="externalId"></param>
        /// <param name="entityContext"></param>
        /// <returns></returns>
        private Entity GetEntityObject(String externalId, EntityContext entityContext)
        {
            Entity entity = null;
            _callerContext.ProgramName = "MDM.WCFServices.InteropEntityService.GetEntityObject";

            try
            {
                EntityBL entityManager = new EntityBL();
                entity = entityManager.GetByExternalId(externalId, entityContext, _callerContext, false, false);
            }
            catch (Exception ex)
            {
                this.LogException(ex);
                throw this.WrapException(ex);
            }

            return entity;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="externalId"></param>
        /// <param name="entityContext"></param>
        /// <returns></returns>
        private String GetEntityXml(String externalId, EntityContext entityContext)
        {
            String entityXml = String.Empty;

            Entity entityObject = GetEntityObject(externalId, entityContext);

            if (entityObject != null)
            {
                entityXml = entityObject.ToXml(ObjectSerialization.External);
            }

            return entityXml;
        }

        #endregion

        #endregion

    }
}
