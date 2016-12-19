using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace MDM.BusinessObjects
{
    using MDM.BusinessObjects.Interfaces;
    using MDM.Core;
    using MDM.Interfaces;
    using System.IO;
    using System.Xml;

    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class EntityVariantDefinitionMapping : MDMObject, IEntityVariantDefinitionMapping, IDataModelObject
    {
        #region Fields

        /// <summary>
        /// Field indicates identifier of entity variant definition
        /// </summary>
        Int32 _entityVariantDefinitionId = 0;

        /// <summary>
        /// Field indicates name of entity variant definition
        /// </summary>
        String _entityVariantDefinitionName = String.Empty;

        /// <summary>
        /// Field indicates identifier of container
        /// </summary>
        Int32 _containerId = 0;

        /// <summary>
        /// Field indicates name of container
        /// </summary>
        String _containerName = String.Empty;

        /// <summary>
        /// Field indicates identifier of category
        /// </summary>
        Int64 _categoryId = 0;

        /// <summary>
        /// Field indicates name of category
        /// </summary>
        String _categoryName = String.Empty;

        /// <summary>
        /// Field indicates path of category
        /// </summary>
        String _categoryPath = String.Empty;

        /// <summary>
        /// Field indicates original entity variant definition mapping
        /// </summary>
        EntityVariantDefinitionMapping _originalEntityVariantDefinitionMapping = null;

        #region IDataModelObject Fields

        /// <summary>
        /// Field indicates EntityVariantDefinitionMapping key External Id from external source.
        /// </summary>
        private String _externalId = String.Empty;

        #endregion IDataModelObject Fields

        #endregion Fields

        #region Properties

        /// <summary>
        /// Property denoting identifier of entity variant definition
        /// </summary>
        [DataMember]
        public Int32 EntityVariantDefinitionId
        {
            get { return _entityVariantDefinitionId; }
            set { _entityVariantDefinitionId = value; }
        }

        /// <summary>
        /// Property denoting name of entity variant definition
        /// </summary>
        [DataMember]
        public String EntityVariantDefinitionName
        {
            get { return _entityVariantDefinitionName; }
            set { _entityVariantDefinitionName = value; }
        }

        /// <summary>
        /// Property denoting identifier of container
        /// </summary>
        [DataMember]
        public Int32 ContainerId
        {
            get { return _containerId; }
            set { _containerId = value; }
        }

        /// <summary>
        /// Property denoting name of container
        /// </summary>
        [DataMember]
        public String ContainerName
        {
            get { return _containerName; }
            set { _containerName = value; }
        }

        /// <summary>
        /// Property denoting identifier of category
        /// </summary>
        [DataMember]
        public Int64 CategoryId
        {
            get { return _categoryId; }
            set { _categoryId = value; }
        }

        /// <summary>
        /// Property denoting name of category
        /// </summary>
        [DataMember]
        public String CategoryName
        {
            get { return _categoryName; }
            set { _categoryName = value; }
        }

        /// <summary>
        /// Property denoting path of category
        /// </summary>
        [DataMember]
        public String CategoryPath
        {
            get { return _categoryPath; }
            set { _categoryPath = value; }
        }

        /// <summary>
        /// Property denoting original entity variant definition mapping
        /// </summary>
        [DataMember]
        public EntityVariantDefinitionMapping OriginalEntityVariantDefinitionMapping
        {
            get { return _originalEntityVariantDefinitionMapping; }
            set { _originalEntityVariantDefinitionMapping = value; }
        }

        #region IDataModelObject Properties

        /// <summary>
        /// Property denoting a ObjectType for an dataModelObject object
        /// </summary>
        public ObjectType DataModelObjectType
        {
            get
            {
                return MDM.Core.ObjectType.EntityVariantDefinitionMapping;
            }
        }

        /// <summary>
        /// Property denoting the external id for an dataModelObject object
        /// </summary>
        public String ExternalId
        {
            get { return _externalId; }
            set { _externalId = value; }
        }

        #endregion IDataModelObject Properties

        #endregion Properties

        #region Method

        #region Public Method

        /// <summary>
        /// Clone entity variant definition mapping object
        /// </summary>
        /// <returns>Cloned copy of entity variant definition mapping object.</returns>
        public IEntityVariantDefinitionMapping Clone()
        {
            EntityVariantDefinitionMapping clonedEntityVariantDefinition = new EntityVariantDefinitionMapping();

            clonedEntityVariantDefinition.Id = this.Id;
            clonedEntityVariantDefinition.EntityVariantDefinitionId = this.EntityVariantDefinitionId;
            clonedEntityVariantDefinition.EntityVariantDefinitionName = this.EntityVariantDefinitionName;
            clonedEntityVariantDefinition.ContainerId = this.ContainerId;
            clonedEntityVariantDefinition.ContainerName = this.ContainerName;
            clonedEntityVariantDefinition.CategoryId = this.CategoryId;
            clonedEntityVariantDefinition.CategoryName = this.CategoryName;
            clonedEntityVariantDefinition.CategoryPath = this.CategoryPath;

            return clonedEntityVariantDefinition;
        }

        /// <summary>
        /// Delta Merge of EntityVariantDefinitionMapping
        /// </summary>
        /// <param name="deltaEntityVariantDefinitionMapping">Indicates EntityVariantDefinitionMapping that needs to be merged.</param>
        /// <param name="iCallerContext">Indicates the name of application and the module that are performing the action.</param>
        /// <param name="returnClonedObject">Indicates whether clone merge object or not.</param>
        /// <returns>Returns merged EntityVariantDefinitionMapping instance.</returns>
        public EntityVariantDefinitionMapping MergeDelta(EntityVariantDefinitionMapping deltaEntityVariantDefinitionMapping, ICallerContext iCallerContext, Boolean returnClonedObject = true)
        {
            EntityVariantDefinitionMapping mergedEntityVariantDefinitionMapping = (returnClonedObject == true) ? deltaEntityVariantDefinitionMapping.Clone() as EntityVariantDefinitionMapping : deltaEntityVariantDefinitionMapping;

            mergedEntityVariantDefinitionMapping.Action = (mergedEntityVariantDefinitionMapping.Equals(this)) ? ObjectAction.Read : ObjectAction.Update;

            return mergedEntityVariantDefinitionMapping;
        }

        /// <summary>
        /// Determines whether specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">EntityType object which needs to be compared.</param>
        /// <returns>Result of the comparison in Boolean.</returns>
        public override bool Equals(object obj)
        {
            if (obj is EntityVariantDefinitionMapping)
            {
                EntityVariantDefinitionMapping objectToBeCompared = obj as EntityVariantDefinitionMapping;

                if (!base.Equals(objectToBeCompared))
                {
                    return false;
                }

                if (this.EntityVariantDefinitionId != objectToBeCompared.EntityVariantDefinitionId)
                {
                    return false;
                }

                if (this.EntityVariantDefinitionName != objectToBeCompared.EntityVariantDefinitionName)
                {
                    return false;
                }

                if (this.ContainerId != objectToBeCompared.ContainerId)
                {
                    return false;
                }

                if (this.ContainerName != objectToBeCompared.ContainerName)
                {
                    return false;
                }

                if (this.CategoryId != objectToBeCompared.CategoryId)
                {
                    return false;
                }

                if (this.CategoryName != objectToBeCompared.CategoryName)
                {
                    return false;
                }

                if (this.CategoryPath != objectToBeCompared.CategoryPath)
                {
                    return false;
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Represents entity variant definition mapping in Xml format 
        /// </summary>
        /// <returns>Returns entity variant definition mapping in Xml format as string.</returns>
        public override String ToXml()
        {
            String xml = string.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            #region For Entity Variant Definition Metadata

            xmlWriter.WriteStartElement("EntityVariantDefinitionMapping");
            xmlWriter.WriteAttributeString("Id", this.Id.ToString());
            xmlWriter.WriteAttributeString("EntityVariantDefinitionId", this.EntityVariantDefinitionId.ToString());
            xmlWriter.WriteAttributeString("EntityVariantDefinitionName", this.EntityVariantDefinitionName);
            xmlWriter.WriteAttributeString("ContainerId", this.ContainerId.ToString());
            xmlWriter.WriteAttributeString("ContainerName", this.ContainerName);
            xmlWriter.WriteAttributeString("CategoryId", this.CategoryId.ToString());
            xmlWriter.WriteAttributeString("CategoryName", this.CategoryName);
            xmlWriter.WriteAttributeString("CategoryPath", this.CategoryPath);

            xmlWriter.WriteEndElement(); //For Entity Variant Definition Mapping

            #endregion For Entity Variant Definition Metadata

            xmlWriter.Flush();

            //get the actual XML
            xml = sw.ToString();

            xmlWriter.Close();

            sw.Close();

            return xml;
        }

        /// <summary>
        ///  Serves as a hash function for entity variant definition mapping
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode() ^ this.EntityVariantDefinitionId.GetHashCode() ^ this.EntityVariantDefinitionName.GetHashCode() ^ this.ContainerId.GetHashCode()
                 ^ this.ContainerName.GetHashCode() ^ this.CategoryId.GetHashCode() ^ this.CategoryName.GetHashCode() ^ this.CategoryPath.GetHashCode();
        }

        #region IDataModelObject Methods

        /// <summary>
        /// Get IDataModelObject for current object.
        /// </summary>
        /// <returns>IDataModelObject</returns>
        public IDataModelObject GetDataModelObject()
        {
            return this;
        }

        #endregion IDataModelObject Methods

        #endregion Public Method

        #region Private Method



        #endregion Private Method

        #endregion Method
    }
}
