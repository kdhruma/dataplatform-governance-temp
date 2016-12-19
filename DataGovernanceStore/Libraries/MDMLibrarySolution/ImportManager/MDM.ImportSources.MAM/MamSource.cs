using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace MDM.ImportSources.MAM
{
    using MDM.Imports.Interfaces;
    using MDM.Core;
    using MDM.BusinessObjects.Jobs;
    using MDM.BusinessObjects.Imports;
    using MDM.BusinessObjects;
    using MDM.Interfaces;


    /// <summary>
    /// Mam Data source.
    /// </summary>
    public class MamSource : IEntityImportSourceData
    {
        private IJobResultHandler _jobResultHandler = null;

        ImportProviderBatchingType _batchingType = ImportProviderBatchingType.None;

        private String _sourceFile = String.Empty;

        /// <summary>
        /// Default constructor
        /// </summary>
        public MamSource()
        {
        }

        public MamSource(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentNullException("File is not available");
            }

            _sourceFile = filePath;
        }

        #region Public Properties
        public ImportProviderBatchingType BatchingType
        {
            get { return _batchingType; }
            set { _batchingType = value; }
        }

        public String SourceFile
        {
            get { return _sourceFile; }
            set { _sourceFile = value; }
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

        #region IImportSourceData Methods
        /// <summary>
        /// Provide an opputunity for the source data to initializes itself with some configuration from the job.
        /// </summary>
        /// <param name="job"></param>
        /// <param name="importProfile"></param>
        /// <returns></returns>
        public Boolean Initialize(Job job, ImportProfile importProfile)
        {
            JobParameter fileIdParameter = job.JobData.JobParameters["FilePath"];

            if (fileIdParameter == null)
                throw new ArgumentException(String.Format("File path job parameter is missing in job configuration. Please check the file upload status. Job Id: {0}", job.Id));

            // Try to get file path from the job data parameter..
            _sourceFile = fileIdParameter.Value;

            if (string.IsNullOrEmpty(_sourceFile))
            {
                throw new ArgumentNullException("Source file path is not available");
            }
            return true;
        }

        /// <summary>
        /// Total number of entities available for processing.
        /// </summary>
        /// <returns></returns>
        public long GetEntityDataCount()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Provides a seed for the caller to start from.
        /// </summary>
        /// <returns></returns>
        public long GetEntityDataSeed()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// If seed is provided, this indicates the endpoint for the caller to stop processing.
        /// </summary>
        /// <returns></returns>
        public long GetEntityEndPoint()
        {
            throw new NotImplementedException();
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
        public int GetEntityDataBatchSize()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets all the entity data available to process.
        /// </summary>
        /// <returns></returns>
        public MDM.BusinessObjects.EntityCollection GetAllEntityData(MDMCenterApplication application, MDMCenterModules module, IEntityProviderContext entityProviderContext)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the entity data for a given batch.
        /// </summary>
        /// <param name="startPK"></param>
        /// <param name="endPK"></param>
        /// <returns></returns>
        public MDM.BusinessObjects.EntityCollection GetEntityDataBatch(long startPK, long endPK, MDM.Core.MDMCenterApplication application, MDM.Core.MDMCenterModules module, IEntityProviderContext entityProviderContext)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the next available batch of entity data. This method is used when the source is a forward only reader.
        /// </summary>
        /// <param name="startPK"></param>
        /// <param name="endPK"></param>
        /// <returns></returns>
        public MDM.BusinessObjects.EntityCollection GetEntityDataNextBatch(int batchSize, MDM.Core.MDMCenterApplication application, MDM.Core.MDMCenterModules module, IEntityProviderContext entityProviderContext)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the attribute data for a given set of entities and attribute type. If your provider implements attributes separately use this method.
        /// </summary>
        /// <param name="attributeType"></param>
        /// <param name="entityCollection"></param>
        /// <returns></returns>
        public MDM.BusinessObjects.AttributeCollection GetAttributeDataforEntities(MDM.Core.AttributeModelType attributeType, MDM.BusinessObjects.EntityCollection entityCollection, MDM.Core.MDMCenterApplication application, MDM.Core.MDMCenterModules module)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the attribute data for a given set of entities and attribute type.
        /// </summary>
        /// <param name="attributeType"></param>
        /// <param name="entityList"></param>
        /// <param name="entityCollection"></param>
        /// <returns></returns>
        public MDM.BusinessObjects.AttributeCollection GetAttributeDataforEntityList(MDM.Core.AttributeModelType attributeType, string entityList, MDM.BusinessObjects.EntityCollection entityCollection, MDM.Core.MDMCenterApplication application, MDM.Core.MDMCenterModules module)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Updates the source with the status as error with the information passed on from the processing method.
        /// </summary>
        /// <param name="errorEntities"></param>
        /// <returns></returns>
        public bool UpdateErrorEntities(MDM.BusinessObjects.EntityOperationResultCollection errorEntities, MDM.Core.MDMCenterApplication application, MDM.Core.MDMCenterModules module)
        {
            throw new NotImplementedException();
        }

        public bool UpdateErrorRelationships(MDM.BusinessObjects.RelationshipOperationResultCollection errorRelationships, MDMCenterApplication application, MDMCenterModules module)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Update the source with the status as error with the informatin passed on from the processing method
        /// </summary>
        /// <param name="errorAttributes"></param>
        /// <returns></returns>
        public bool UpdateErrorAttributes(MDM.BusinessObjects.AttributeOperationResultCollection errorAttributes, MDM.Core.MDMCenterApplication application, MDM.Core.MDMCenterModules module)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Update the source with the status as error for this attribute type and for all the entities in this batch
        /// </summary>
        /// <param name="attributeType"></param>
        /// <param name="entityList"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public bool UpdateErrorAttributes(MDM.Core.AttributeModelType attributeType, string entityList, string errorMessage, MDM.Core.MDMCenterApplication application, MDM.Core.MDMCenterModules module)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Update the source with the status as success for all the processed entities.
        /// </summary>
        /// <param name="errorAttributes"></param>
        /// <returns></returns>
        public bool UpdateSuccessEntities(MDM.BusinessObjects.EntityCollection entities, MDM.Core.MDMCenterApplication application, MDM.Core.MDMCenterModules module)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Update the source with the status as success for all the processed entities.
        /// </summary>
        /// <param name="errorAttributes"></param>
        /// <returns></returns>
        public bool UpdateSuccessAttributes(MDM.Core.AttributeModelType attributeType, MDM.BusinessObjects.EntityCollection entities, MDM.Core.MDMCenterApplication application, MDM.Core.MDMCenterModules module)
        {
            throw new NotImplementedException();
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

        #region IImportSourceData Methods For Relationship

        public Int64 GetRelationshipDataCount()
        {
            throw new NotImplementedException();
        }

        public Int64 GetRelationshipDataSeed()
        {
            throw new NotImplementedException();
        }

        public Int64 GetRelationshipEndPoint()
        {
            throw new NotImplementedException();
        }

        public Int32 GetRelationshipDataBatchSize()
        {
            throw new NotImplementedException();
        }

        public BusinessObjects.EntityCollection GetAllRelationshipData(MDMCenterApplication application, MDMCenterModules module)
        {
            throw new NotImplementedException();
        }

        public BusinessObjects.EntityCollection GetRelationshipDataBatch(Int64 startPK, Int64 endPK, MDMCenterApplication application, MDMCenterModules module)
        {
            throw new NotImplementedException();
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
            return null;
        }
        #endregion

        #region Mapping Related Interface Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputFieldName"></param>
        /// <returns></returns>
        public Attribute GetAttributeInfoFromInputFieldName(String inputFieldName)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
