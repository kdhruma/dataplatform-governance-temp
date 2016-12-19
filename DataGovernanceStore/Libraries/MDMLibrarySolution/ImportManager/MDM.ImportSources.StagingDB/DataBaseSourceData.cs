using System;
using System.Data;

namespace MDM.ImportSources.StagingDB
{
    using MDM.BusinessObjects;
    using MDM.ConfigurationManager.Business;
    using MDM.Core;
    using MDM.Imports.Interfaces;
    using MDM.ImportSources.StagingDB.Data;
    using MDM.BusinessObjects.Jobs;
    using MDM.BusinessObjects.Imports;
    using MDM.Interfaces;
    using MDM.Utility;
    using MDM.ExceptionManager;
    using MDM.BusinessObjects.Diagnostics;

    /// <summary>
    /// This class implements the source data from a staging database.
    /// </summary>
    public class DataBaseSourceData : IEntityImportSourceData
    {
        private IJobResultHandler _jobResultHandler = null;

        Int64 _entitySeed = -1;

        Int64 _entityEndPoint = -1;

        Int64 _entityCount = -1;

        Int32 _batchSize = 100;

        ImportProviderBatchingType _batchingType = ImportProviderBatchingType.Multiple;

        /// <summary>
        /// Is any trace on
        /// </summary>
        private Boolean _isAnyTraceOn = false;

        /// <summary>
        /// Default constructor. Sets the entity information ( seed, endpoint, count) from the staging database.
        /// </summary>
        public DataBaseSourceData(bool isDiagnosticsEnabled = false)
        {
            //Get command
            DBCommandProperties command = DBCommandHelper.Get(MDMCenterApplication.JobService, MDMCenterModules.Staging, MDMCenterModuleAction.Read);

            DataBaseSourceDataDA datasourceDA = new DataBaseSourceDataDA();
            datasourceDA.GetEntityInformation(command, out _entitySeed, out _entityEndPoint, out _entityCount, out _batchSize);

            // Set the global trace flag
            _isAnyTraceOn = isDiagnosticsEnabled | Constants.TRACING_ENABLED | Constants.PERFORMANCE_TRACING_ENABLED;
        }

        /// <summary>
        /// Seed and end point are passed from client.
        /// </summary>
        /// <param name="seed"></param>
        /// <param name="endPoint"></param>
        public DataBaseSourceData(Int64 seed, Int64 endPoint)
        {
            _entitySeed = seed;
            _entityEndPoint = endPoint;
            _entityCount = endPoint - seed + 1;
        }

        #region Public Properties

        public Int64 EntitySeed
        {
            get { return _entitySeed; }
            set { _entitySeed = value; }
        }

        public Int64 EntityEndPoint
        {
            get { return _entityEndPoint; }
            set { _entityEndPoint = value; }
        }

        public Int64 EntityCount
        {
            get { return _entityCount; }
            set { _entityCount = value; }
        }

        public Int32 BatchSize
        {
            get { return _batchSize; }
            set { _batchSize = value; }
        }

        public ImportProviderBatchingType BatchingType
        {
            get { return _batchingType; }
            set { _batchingType = value; }
        }

        #endregion

        #region IEntityImportSourceData Properties
        public IJobResultHandler JobResultHandler
        {
            get
            {
                return _jobResultHandler;
            }
            set
            {
                _jobResultHandler = value;
            }
        }
        #endregion

        #region IImportSource Data Methods

        /// <summary>
        /// Provide an opputunity for the source data to initializes itself with some configuration from the job.
        /// </summary>
        /// <param name="job"></param>
        /// <param name="importProfile"></param>
        /// <returns></returns>
        public Boolean Initialize(Job job, ImportProfile importProfile)
        {
            string message = string.Empty;
            DiagnosticActivity activity = new DiagnosticActivity();
            if (_isAnyTraceOn) activity.Start();

            JobParameter startingRecordJobParameter = job.JobData.JobParameters["StartingStagingDBRecordNumber"];
            JobParameter endingRecordJobParameter = job.JobData.JobParameters["EndingStagingDBRecordNumber"];

            if (startingRecordJobParameter != null && endingRecordJobParameter != null)
            {
                _entitySeed = ValueTypeHelper.Int32TryParse(startingRecordJobParameter.Value, 0);
                _entityEndPoint = ValueTypeHelper.Int32TryParse(endingRecordJobParameter.Value, 0);
                _entityCount = _entityEndPoint - _entitySeed + 1;
            }
            else
            {
                return false;
            }

            if (_isAnyTraceOn) activity.Stop();

            return true;
        }

        /// <summary>
        /// Total number of entities available for processing.
        /// </summary>
        /// <returns></returns>
        public Int64 GetEntityDataCount()
        {
            return EntityCount;
        }

        /// <summary>
        /// Provides a seed for the caller to start from.
        /// </summary>
        /// <returns></returns>
        public Int64 GetEntityDataSeed()
        {
            return EntitySeed;
        }

        /// <summary>
        /// If seed is provided, this indicates the endpoint for the caller to stop processing.
        /// </summary>
        /// <returns></returns>
        public Int64 GetEntityEndPoint()
        {
            return EntityEndPoint;
        }

        /// <summary>
        /// Indicates the batching mode the proiver supports.
        /// </summary>
        /// <returns></returns>
        public ImportProviderBatchingType GetBatchingType()
        {
            return BatchingType;
        }
        /// <summary>
        /// Indicates the batch size for the worker thread.
        /// </summary>
        /// <returns></returns>
        public Int32 GetEntityDataBatchSize()
        {
            return BatchSize;
        }

        public EntityCollection GetAllEntityData(MDMCenterApplication application, MDMCenterModules module, IEntityProviderContext entityProviderContext)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the entity data for a given batch.
        /// </summary>
        /// <param name="startPK"></param>
        /// <param name="endPK"></param>
        /// <returns></returns>
        public EntityCollection GetEntityDataBatch(Int64 startPK, Int64 endPK, MDMCenterApplication application, MDMCenterModules module, IEntityProviderContext entityProviderContext)
        {
            string message = string.Empty;
            DiagnosticActivity activity = new DiagnosticActivity();
            if (_isAnyTraceOn) activity.Start();

            String containerName = String.Empty;
            String organizationName = String.Empty;
            String entityTypeName = String.Empty;
            //Get command
            DBCommandProperties command = DBCommandHelper.Get(application, module, MDMCenterModuleAction.Read);

            if (entityProviderContext.EntityProviderContextType == EntityProviderContextType.Container)
                containerName = entityProviderContext.ContainerName;

            if (entityProviderContext.EntityProviderContextType == EntityProviderContextType.Organization)
                organizationName = entityProviderContext.OrganizationName;

            if (entityProviderContext.EntityProviderContextType == EntityProviderContextType.EntityType)
                entityTypeName = entityProviderContext.EntityTypeName;

            DataBaseSourceDataDA datasourceDA = new DataBaseSourceDataDA();
            
            if (_isAnyTraceOn) activity.Stop();

            return datasourceDA.GetEntityDataBatch(startPK, endPK, command, containerName, organizationName, entityTypeName);
        }

        /// <summary>
        /// Gets the next available batch of entity data. This method is used when the source is a forward only reader.
        /// </summary>
        /// <param name="startPK"></param>
        /// <param name="endPK"></param>
        /// <param name="application"></param>
        /// <param name="module"></param>
        /// <returns></returns>
        public EntityCollection GetEntityDataNextBatch(Int32 batchSize, MDMCenterApplication application, MDMCenterModules module, IEntityProviderContext entityProviderContext)
        {
            return null;
        }

        public BusinessObjects.AttributeCollection GetAttributeDataforEntities(AttributeModelType attributeType, EntityCollection entityCollection, MDMCenterApplication application, MDMCenterModules module)
        {
            //Get command
            DBCommandProperties command = DBCommandHelper.Get(application, module, MDMCenterModuleAction.Read);

            // Get the list in the format required by the database...
            string entityList = string.Empty;

            foreach (Entity item in entityCollection)
            {
                // Get entity;
                if (string.IsNullOrEmpty(entityList))
                    entityList = string.Format("'{0}'", item.ReferenceId);
                else
                    entityList = string.Format("{0}, '{1}'", entityList, item.ReferenceId);
            }

            DataBaseSourceDataDA datasourceDA = new DataBaseSourceDataDA();
            return datasourceDA.GetAttributeDataforEntities(attributeType, entityList, entityCollection, command);
        }

        public BusinessObjects.AttributeCollection GetAttributeDataforEntityList(AttributeModelType attributeType, string entityList, EntityCollection entityCollection, MDMCenterApplication application, MDMCenterModules module)
        {
            //Get command
            DBCommandProperties command = DBCommandHelper.Get(application, module, MDMCenterModuleAction.Read);

            DataBaseSourceDataDA datasourceDA = new DataBaseSourceDataDA();
            return datasourceDA.GetAttributeDataforEntities(attributeType, entityList, entityCollection, command);
        }

        /// <summary>
        /// Updates the source with the status as error with the information passed on from the processing method.
        /// </summary>
        /// <param name="errorEntities"></param>
        /// <returns></returns>
        public bool UpdateErrorEntities(EntityOperationResultCollection errorEntities, MDMCenterApplication application, MDMCenterModules module)
        {
            string message = string.Empty;
            DiagnosticActivity activity = new DiagnosticActivity();
            if (_isAnyTraceOn) activity.Start();

            Boolean result = true;

            try
            {
                DBCommandProperties command = DBCommandHelper.Get(application, module, MDMCenterModuleAction.Read);

                DataBaseSourceDataDA datasourceDA = new DataBaseSourceDataDA();
                datasourceDA.UpdateErrorEntities(errorEntities, command);
            }
            catch (Exception ex)
            {
                result = false;
                ExceptionHandler exceptionHandler = new ExceptionHandler(ex);

                activity.LogError(string.Format("Error in UpdateErrorEntities: {0}", ex.Message));
            }
            finally
            {
                if (_isAnyTraceOn) activity.Stop();
            }

            return result;

        }

        /// <summary>
        /// UpdateErrorRelationships
        /// </summary>
        /// <param name="errorRelationships"></param>
        /// <param name="application"></param>
        /// <param name="module"></param>
        /// <returns></returns>
        public bool UpdateErrorRelationships(BusinessObjects.RelationshipOperationResultCollection errorRelationships, MDMCenterApplication application, MDMCenterModules module)
        {
            string message = string.Empty;
            DiagnosticActivity activity = new DiagnosticActivity();
            if (_isAnyTraceOn) activity.Start();

            Boolean result = true;

            try
            {
                DBCommandProperties command = DBCommandHelper.Get(application, module, MDMCenterModuleAction.Read);

                DataBaseSourceDataDA datasourceDA = new DataBaseSourceDataDA();
                datasourceDA.UpdateErrorRelationships(errorRelationships, command);
            }
            catch (Exception ex)
            {
                result = false;
                ExceptionHandler exceptionHandler = new ExceptionHandler(ex);

                activity.LogError(string.Format("Error in UpdateErrorRelationships: {0}", ex.Message));
            }
            finally
            {
                if (_isAnyTraceOn) activity.Stop();
            }

            return result;
        }

        /// <summary>
        /// Update the source with the status as error with the informatin passed on from the processing method
        /// </summary>
        /// <param name="errorAttributes"></param>
        /// <returns></returns>
        public bool UpdateErrorAttributes(AttributeOperationResultCollection errorAttributes, MDMCenterApplication application, MDMCenterModules module)
        {
            string message = string.Empty;
            DiagnosticActivity activity = new DiagnosticActivity();
            if (_isAnyTraceOn) activity.Start();

            Boolean result = true;

            try
            {
                DBCommandProperties command = DBCommandHelper.Get(application, module, MDMCenterModuleAction.Read);

                DataBaseSourceDataDA datasourceDA = new DataBaseSourceDataDA();
                datasourceDA.UpdateErrorAttributes(errorAttributes, command);
            }
            catch (Exception ex)
            {
                result = false;
                ExceptionHandler exceptionHandler = new ExceptionHandler(ex);

                activity.LogError(string.Format("Error in UpdateErrorAttributes: {0}", ex.Message));                
            }
            finally
            {
                if (_isAnyTraceOn) activity.Stop();
            }

            return result;
        }

        /// <summary>
        /// Update the source with the status as error for this attribute type and for all the entities in this batch
        /// </summary>
        /// <param name="attributeType"></param>
        /// <param name="entityList"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public bool UpdateErrorAttributes(AttributeModelType attributeType, string entityList, string errorMessage, MDMCenterApplication application, MDMCenterModules module)
        {
            string message = string.Empty;
            DiagnosticActivity activity = new DiagnosticActivity();
            if (_isAnyTraceOn) activity.Start();

            Boolean result = true;

            try
            {
                DBCommandProperties command = DBCommandHelper.Get(application, module, MDMCenterModuleAction.Read);

                DataBaseSourceDataDA datasourceDA = new DataBaseSourceDataDA();

                datasourceDA.UpdateErrorAttributes(attributeType, entityList, errorMessage, command);
            }
            catch (Exception ex)
            {
                result = false;
                ExceptionHandler exceptionHandler = new ExceptionHandler(ex);

                message = "DataBaseSourceData.UpdateErrorAttributes failed for EntityId List:" + entityList;
                message += "and AttributeModelType: " + attributeType.ToString();
                message += ". Exception:" + ex.Message;

                activity.LogError(message);
            }
            finally
            {
                if (_isAnyTraceOn) activity.Stop();
            }

            return result;
        }

        /// <summary>
        /// Update the source with the status as success for all the processed entities.
        /// </summary>
        /// <param name="errorAttributes"></param>
        /// <returns></returns>
        public bool UpdateSuccessEntities(EntityCollection entities, MDMCenterApplication application, MDMCenterModules module)
        {
            string message = string.Empty;
            DiagnosticActivity activity = new DiagnosticActivity();
            if (_isAnyTraceOn) activity.Start();

            Boolean result = true;            

            try
            {
                DBCommandProperties command = DBCommandHelper.Get(application, module, MDMCenterModuleAction.Read);

                DataBaseSourceDataDA datasourceDA = new DataBaseSourceDataDA();
                datasourceDA.UpdateSuccessEntities(entities, command);

            }
            catch (Exception ex)
            {
                result = false;

                ExceptionHandler exceptionHandler = new ExceptionHandler(ex);
                String errorMessage = "DataBaseSourceData.UpdateSuccessEntities failed for EntityId List:";

                if (entities != null && entities.Count > 0)
                {
                    errorMessage += ValueTypeHelper.JoinCollection(entities.GetEntityIdList(), ",");
                }
                errorMessage += ".Exception: " + ex.Message;

                activity.LogError(errorMessage);
            }
            finally
            {
                if (_isAnyTraceOn) activity.Stop();
            }

            return result;
        }

        /// <summary>
        /// Update the source with the status as success for all the processed entities.
        /// </summary>
        /// <param name="errorAttributes"></param>
        /// <returns></returns>
        public bool UpdateSuccessAttributes(AttributeModelType attributeType, EntityCollection entities, MDMCenterApplication application, MDMCenterModules module)
        {
            string message = string.Empty;
            DiagnosticActivity activity = new DiagnosticActivity();
            if (_isAnyTraceOn) activity.Start();

            Boolean result = true;

            try
            {
                //Get command
                DBCommandProperties command = DBCommandHelper.Get(application, module, MDMCenterModuleAction.Read);

                DataBaseSourceDataDA datasourceDA = new DataBaseSourceDataDA();
                datasourceDA.UpdateSuccessAttributes(attributeType, entities, command);
            }
            catch (Exception ex)
            {
                result = false;

                String errorMessage = "UpdateSuccessAttributes.UpdateSuccessAttributes failed for entityIds:";

                if (entities != null && entities.Count > 0)
                {
                    errorMessage += ValueTypeHelper.JoinCollection(entities.GetEntityIdList(), ",");
                }

                errorMessage += " and AttributeModelType: " + attributeType.ToString();
                errorMessage += ". Exception:" + ex.Message;

                ExceptionHandler exceptionHandler = new ExceptionHandler(ex);

                activity.LogError(errorMessage);
            }
            finally
            {
                if (_isAnyTraceOn) activity.Stop();
            }

            return result;
        }

        /// <summary>
        /// Get input specification.
        /// This is mostly used on UI for profile management. It will get list of entity metadata and attributes, which can be used later for changing attribute mapping.
        /// </summary>
        /// <param name="entityCountToRead">No. of entities to read from sample.</param>
        /// <param name="callerContext">Indicates who called this method</param>
        /// <returns>DataSet representing schema available in excel.</returns>
        public DataSet GetInputSpecification(Int32 entityCountToRead, CallerContext callerContext)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Relationship methods implementations

        /// <summary>
        /// Total number of entities available for processing.
        /// </summary>
        /// <returns></returns>
        public Int64 GetRelationshipDataCount()
        {
            return EntityCount;
        }

        /// <summary>
        /// Provides a seed for the caller to start from.
        /// </summary>
        /// <returns></returns>
        public Int64 GetRelationshipDataSeed()
        {
            return EntitySeed;
        }

        /// <summary>
        /// If seed is provided, this indicates the endpoint for the caller to stop processing.
        /// </summary>
        /// <returns></returns>
        public Int64 GetRelationshipEndPoint()
        {
            return EntityEndPoint;
        }

        /// <summary>
        /// Indicates the batch size for the worker thread.
        /// </summary>
        /// <returns></returns>
        public Int32 GetRelationshipDataBatchSize()
        {
            return BatchSize;
        }

        public BusinessObjects.EntityCollection GetAllRelationshipData(MDMCenterApplication application, MDMCenterModules module)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the relationship data for a given batch.
        /// </summary>
        /// <param name="startPK"></param>
        /// <param name="endPK"></param>
        /// <returns></returns>
        public EntityCollection GetRelationshipDataBatch(Int64 startPK, Int64 endPK, MDMCenterApplication application, MDMCenterModules module)
        {
            string message = string.Empty;
            DiagnosticActivity activity = new DiagnosticActivity();
            if (_isAnyTraceOn) activity.Start();

            try
            {
                //Get command
                DBCommandProperties command = DBCommandHelper.Get(application, module, MDMCenterModuleAction.Read);

                DataBaseSourceDataDA datasourceDA = new DataBaseSourceDataDA();
                return datasourceDA.GetRelationshipDataBatch(startPK, endPK, command);
            }
            finally
            {
                if (_isAnyTraceOn) activity.Stop();
            }
        }

        public BusinessObjects.EntityCollection GetRelationshipDataNextBatch(Int32 batchSize, MDMCenterApplication application, MDMCenterModules module)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region IImportSourceData methods for Extension Relationships
        /// <summary>
        /// Gets the parent extension relationship for a given entity
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="application"></param>
        /// <param name="module"></param>
        /// <returns></returns>
        public ExtensionRelationship GetParentExtensionRelationShip(Entity entity, MDMCenterApplication application, MDMCenterModules module)
        {
            string message = string.Empty;
            DiagnosticActivity activity = new DiagnosticActivity();
            if (_isAnyTraceOn) activity.Start();

            try
            {
                //Get command
                DBCommandProperties command = DBCommandHelper.Get(application, module, MDMCenterModuleAction.Read);

                DataBaseSourceDataDA datasourceDA = new DataBaseSourceDataDA();
                return datasourceDA.GetParentExtensionRelationShip(entity, command);
            }
            finally
            {
                if (_isAnyTraceOn) activity.Stop();
            }
        }
        #endregion

        #region Mapping Related Interface Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputFieldNames"></param>
        /// <returns></returns>
        public Attribute GetAttributeInfoFromInputFieldName(String inputFieldName)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
