using System;
using System.Xml;
using System.Xml.Schema;
using System.Data;

namespace MDM.ImportSources.Generic12
{
    using MDM.BusinessObjects;
    using MDM.Core;
    using MDM.Imports.Interfaces;
    using MDM.BusinessObjects.Jobs;
    using MDM.BusinessObjects.Imports;
    using System.IO;
    using MDM.Interfaces;

    /// <summary>
    /// This class implements the source data from a RS generic source.
    /// </summary>
    public class Generic12 : IEntityImportSourceData
    {
        private IJobResultHandler _jobResultHandler = null;

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

        #region IEntityImportSourceData Members

        public bool Initialize(Job job, ImportProfile importProfile)
        {
            throw new NotImplementedException();
        }

        public long GetEntityDataCount()
        {
            throw new NotImplementedException();
        }

        public long GetEntityDataSeed()
        {
            throw new NotImplementedException();
        }

        public long GetEntityEndPoint()
        {
            throw new NotImplementedException();
        }

        public ImportProviderBatchingType GetBatchingType()
        {
            throw new NotImplementedException();
        }

        public int GetEntityDataBatchSize()
        {
            throw new NotImplementedException();
        }

        public EntityCollection GetAllEntityData(MDMCenterApplication application, MDMCenterModules module, IEntityProviderContext entityProviderContext)
        {
            throw new NotImplementedException();
        }

        public EntityCollection GetEntityDataBatch(long startPK, long endPK, MDMCenterApplication application, MDMCenterModules module, IEntityProviderContext entityProviderContext)
        {
            throw new NotImplementedException();
        }

        public EntityCollection GetEntityDataNextBatch(int batchSize, MDMCenterApplication application, MDMCenterModules module, IEntityProviderContext entityProviderContext)
        {
            throw new NotImplementedException();
        }

        public AttributeCollection GetAttributeDataforEntities(AttributeModelType attributeType, EntityCollection entityCollection, MDMCenterApplication application, MDMCenterModules module)
        {
            throw new NotImplementedException();
        }

        public AttributeCollection GetAttributeDataforEntityList(AttributeModelType attributeType, string entityList, EntityCollection entityCollection, MDMCenterApplication application, MDMCenterModules module)
        {
            throw new NotImplementedException();
        }

        public bool UpdateErrorEntities(EntityOperationResultCollection errorEntities, MDMCenterApplication application, MDMCenterModules module)
        {
            throw new NotImplementedException();
        }

        public bool UpdateErrorRelationships(RelationshipOperationResultCollection errorRelationships, MDMCenterApplication application, MDMCenterModules module)
        {
            throw new NotImplementedException();
        }

        public bool UpdateErrorAttributes(AttributeOperationResultCollection errorAttributes, MDMCenterApplication application, MDMCenterModules module)
        {
            throw new NotImplementedException();
        }

        public bool UpdateErrorAttributes(AttributeModelType attributeType, string entityList, string errorMessage, MDMCenterApplication application, MDMCenterModules module)
        {
            throw new NotImplementedException();
        }

        public bool UpdateSuccessEntities(EntityCollection entities, MDMCenterApplication application, MDMCenterModules module)
        {
            throw new NotImplementedException();
        }

        public bool UpdateSuccessAttributes(AttributeModelType attributeType, EntityCollection entities, MDMCenterApplication application, MDMCenterModules module)
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
        public DataSet GetInputSpecification(Int32 entityCountToRead , CallerContext callerContext)
        {
            throw new NotImplementedException();
        }

        public long GetRelationshipDataCount()
        {
            throw new NotImplementedException();
        }

        public long GetRelationshipDataSeed()
        {
            throw new NotImplementedException();
        }

        public long GetRelationshipEndPoint()
        {
            throw new NotImplementedException();
        }

        public int GetRelationshipDataBatchSize()
        {
            throw new NotImplementedException();
        }

        public BusinessObjects.EntityCollection GetAllRelationshipData(MDMCenterApplication application, MDMCenterModules module)
        {
            throw new NotImplementedException();
        }

        public EntityCollection GetRelationshipDataBatch(long startPK, long endPK, MDMCenterApplication application, MDMCenterModules module)
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
