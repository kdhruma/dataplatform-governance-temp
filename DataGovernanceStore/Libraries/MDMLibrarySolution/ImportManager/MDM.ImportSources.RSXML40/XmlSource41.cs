using System;
using System.Xml;
using System.Xml.Schema;

namespace MDM.ImportSources.RSXml
{
    using MDM.BusinessObjects;
    using MDM.Core;
    using MDM.Imports.Interfaces;
    using MDM.BusinessObjects.Jobs;
    using MDM.BusinessObjects.Imports;
    using System.IO;
    using MDM.Interfaces;
    using System.Data;

    /// <summary>
    /// This is the wrapper class for a 4.1 specific profile. Any functionality specific to this version can be overridden here.
    /// </summary>
    public class XmlSource41 : BaseXmlSource, IEntityImportSourceData
    {

        private IJobResultHandler _jobResultHandler = null;

        public XmlSource41(string filePath)
            : base(filePath)
        {

        }

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
        #region IImportSourceData Methods For Entity

        /// <summary>
        /// Provide an opputunity for the source data to initializes itself with some configuration from the job.
        /// </summary>
        /// <param name="job"></param>
        /// <param name="importProfile"></param>
        /// <returns></returns>
        Boolean IImportSourceData.Initialize(Job job, ImportProfile importProfile)
        {
            return base.Initialize(job, importProfile);
        }

        Int64 IEntityImportSourceData.GetEntityDataCount()
        {
            return base.GetEntityDataCount();
        }

        Int64 IEntityImportSourceData.GetEntityDataSeed()
        {
            return base.GetEntityDataSeed();
        }

        Int64 IEntityImportSourceData.GetEntityEndPoint()
        {
            return base.GetEntityEndPoint();
        }

        /// <summary>
        /// Indicates the batching mode the proiver supports.
        /// </summary>
        /// <returns></returns>
        ImportProviderBatchingType IEntityImportSourceData.GetBatchingType()
        {
            return base.GetBatchingType();
        }

        Int32 IEntityImportSourceData.GetEntityDataBatchSize()
        {
            return base.GetEntityDataBatchSize();
        }

        BusinessObjects.EntityCollection IEntityImportSourceData.GetAllEntityData(MDMCenterApplication application, MDMCenterModules module, IEntityProviderContext entityProviderContext)
        {
            return base.GetAllEntityData(application, module, entityProviderContext);
        }

        BusinessObjects.EntityCollection IEntityImportSourceData.GetEntityDataBatch(Int64 startPK, Int64 endPK, MDMCenterApplication application, MDMCenterModules module, IEntityProviderContext entityProviderContext)
        {
            return base.GetEntityDataBatch(startPK, endPK, application, module, entityProviderContext);
        }

        /// <summary>
        /// Gets the next available batch of data for processing.
        /// </summary>
        /// <param name="batchSize"></param>
        /// <param name="application"></param>
        /// <param name="module"></param>
        /// <returns></returns>
        BusinessObjects.EntityCollection IEntityImportSourceData.GetEntityDataNextBatch(Int32 batchSize, MDMCenterApplication application, MDMCenterModules module, IEntityProviderContext entityProviderContext)
        {
            return base.GetEntityDataNextBatch(batchSize, application, module, entityProviderContext);
        }

        BusinessObjects.AttributeCollection IEntityImportSourceData.GetAttributeDataforEntities(Core.AttributeModelType attributeType, BusinessObjects.EntityCollection entityCollection, MDMCenterApplication application, MDMCenterModules module)
        {
            return base.GetAttributeDataforEntities(attributeType, entityCollection, application, module);
        }

        BusinessObjects.AttributeCollection IEntityImportSourceData.GetAttributeDataforEntityList(Core.AttributeModelType attributeType, string entityList, BusinessObjects.EntityCollection entityCollection, MDMCenterApplication application, MDMCenterModules module)
        {
            return base.GetAttributeDataforEntityList(attributeType, entityList, entityCollection, application, module);
        }

        bool IEntityImportSourceData.UpdateErrorEntities(BusinessObjects.EntityOperationResultCollection errorEntities, MDMCenterApplication application, MDMCenterModules module)
        {
            return true;
        }

        bool IEntityImportSourceData.UpdateErrorRelationships(BusinessObjects.RelationshipOperationResultCollection errorRelationships, MDMCenterApplication application, MDMCenterModules module)
        {
            return true;
        }

        bool IEntityImportSourceData.UpdateErrorAttributes(BusinessObjects.AttributeOperationResultCollection errorAttributes, MDMCenterApplication application, MDMCenterModules module)
        {
            return true;
        }

        bool IEntityImportSourceData.UpdateErrorAttributes(Core.AttributeModelType attributeType, string entityList, string errorMessage, MDMCenterApplication application, MDMCenterModules module)
        {
            return true;
        }

        bool IEntityImportSourceData.UpdateSuccessEntities(BusinessObjects.EntityCollection entities, MDMCenterApplication application, MDMCenterModules module)
        {
            return true;
        }

        bool IEntityImportSourceData.UpdateSuccessAttributes(Core.AttributeModelType attributeType, BusinessObjects.EntityCollection entities, MDMCenterApplication application, MDMCenterModules module)
        {
            return true;
        }

        /// <summary>
        /// Get input specification.
        /// This is mostly used on UI for profile management. It will get list of entity metadata and attributes, which can be used later for changing attribute mapping.
        /// </summary>
        /// <param name="entityCountToRead">No. of entities to read from sample.</param>
        /// <param name="callerContext">Indicates who called this method</param>
        /// <returns>DataSet representing schema available in excel.</returns>
        DataSet IEntityImportSourceData.GetInputSpecification(Int32 entityCountToRead, CallerContext callerContext)
        {
            return base.GetInputSpecification(entityCountToRead, callerContext);
        }

        #endregion

        #region IImportSourceData Methods For Relationship

        Int64 IEntityImportSourceData.GetRelationshipDataCount()
        {
            return 0;
        }

        Int64 IEntityImportSourceData.GetRelationshipDataSeed()
        {
            return 0;
        }

        Int64 IEntityImportSourceData.GetRelationshipEndPoint()
        {
            return 0;
        }

        Int32 IEntityImportSourceData.GetRelationshipDataBatchSize()
        {
            return base.GetEntityDataBatchSize();
        }

        BusinessObjects.EntityCollection IEntityImportSourceData.GetAllRelationshipData(MDMCenterApplication application, MDMCenterModules module)
        {
            throw new NotImplementedException();
        }

        BusinessObjects.EntityCollection IEntityImportSourceData.GetRelationshipDataBatch(Int64 startPK, Int64 endPK, MDMCenterApplication application, MDMCenterModules module)
        {
            throw new NotImplementedException();
        }

        BusinessObjects.EntityCollection IEntityImportSourceData.GetRelationshipDataNextBatch(Int32 batchSize, MDMCenterApplication application, MDMCenterModules module)
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
        ExtensionRelationship IEntityImportSourceData.GetParentExtensionRelationShip(Entity entity, MDMCenterApplication application, MDMCenterModules module)
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
        Attribute IEntityImportSourceData.GetAttributeInfoFromInputFieldName(String inputFieldName)
        {
            return base.GetAttributeInfoFromInputFieldName(inputFieldName);
        }

        #endregion
    }
}
