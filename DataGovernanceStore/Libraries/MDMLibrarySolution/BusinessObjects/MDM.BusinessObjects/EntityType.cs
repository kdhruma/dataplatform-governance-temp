using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies the Type of an Entity
    /// </summary>
    [DataContract]
    public class EntityType : MDMObject, IEntityType, IDataModelObject
    {
        #region Fields

        /// <summary>
        /// Field denoting the child entity types of an entity type
        /// </summary>
        private EntityTypeCollection _entityTypes = new EntityTypeCollection();

        /// <summary>
        /// Field denoting the catalog branch level of entity type
        /// </summary>
        private Int32 _catalogBranchLevel = 2;

        /// <summary>
        /// Field Denoting the original entity type
        /// </summary>
        private EntityType _originalEntityType = null;

        #region IDataModelObject Fields

        /// <summary>
        /// Field denoting Organization key External Id from external source.
        /// </summary>
        private String _externalId = String.Empty;

        #endregion

        #endregion

        #region Constructors

        /// <summary>
        /// Parameter-less Constructor
        /// </summary>
        public EntityType()
            : base()
        {
        }

        /// <summary>
        /// Constructor with Id as input parameter
        /// </summary>
        /// <param name="id">Indicates the Identity of an Entity Type</param>
        public EntityType(Int32 id)
            : base(id)
        {
        }

        /// <summary>
        /// Constructor with Id, Name and Description of an Entity Type as input parameters
        /// </summary>
        /// <param name="id">Indicates the Identity of an Entity Type</param>
        /// <param name="name">Indicates the Name of an Entity Type</param>
        /// <param name="longName">Indicates the Description of an Entity Type</param>
        public EntityType(Int32 id, String name, String longName)
            : base(id, name, longName)
        {
        }

        /// <summary>
        /// Constructor with Id, Name, LongName and Locale of an Entity Type as input parameters
        /// </summary>
        /// <param name="id">Indicates the Identity of an Entity Type</param>
        /// <param name="name">Indicates the Name of an Entity Type</param>
        /// <param name="longName">Indicates the LongName of an Entity Type</param>
        /// <param name="locale">Indicates the Locale of an Entity Type</param>
        public EntityType(Int32 id, String name, String longName, LocaleEnum locale)
            : base(id, name, longName, locale)
        {
        }
        
        /// <summary>
        /// Create EntityType object with property values xml as input parameter
        /// </summary>
        /// <param name="valuesAsXml">XML representation for EntityType from which object is to be created</param>
        public EntityType(String valuesAsXml)
        {
            LoadEntityType(valuesAsXml);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Property defining the type of the MDM object
        /// </summary>
        public new String ObjectType
        {
            get
            {
                return "EntityType";
            }
        }

        /// <summary>
        /// Property denoting the child entity types of an entity type
        /// </summary>
        [DataMember]
        public EntityTypeCollection EntityTypes
        {
            get
            {
                return this._entityTypes;
            }
            set
            {
                this._entityTypes = value;
            }
        }

        /// <summary>
        /// Property denoting the catalog branch level of entity type
        /// </summary>
        [DataMember]
        public Int32 CatalogBranchLevel
        {
            get { return _catalogBranchLevel; }
            set { _catalogBranchLevel = value; }
        }

        /// <summary>
        /// Property denoting the original entity type
        /// </summary>
        public EntityType OriginalEntityType
        {
            get
            {
                return _originalEntityType;
            }
            set
            {
                this._originalEntityType = value;
            }
        }

        #region IDataModelObject Properties

        /// <summary>
        /// Property denoting a ObjectType for an dataModelObject object
        /// </summary>
        public ObjectType DataModelObjectType
        {
            get
            {
                return MDM.Core.ObjectType.EntityType;
            }
        }

        /// <summary>
        /// Property denoting the external id for an dataModelObject object
        /// </summary>
        public String ExternalId
        {
            get
            {
                return _externalId;
            }
            set
            {
                _externalId = value;
            }
        }

        #endregion

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Get Xml representation of EntityType object
        /// </summary>
        /// <returns>Xml representation of object</returns>
        public String ToXML()
        {
            String xml = string.Empty;
            StringWriter stringWriter = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(stringWriter);

            xmlWriter.WriteStartElement("EntityType");

            xmlWriter.WriteAttributeString("Id", this.Id.ToString());
            xmlWriter.WriteAttributeString("Name", this.Name);
            xmlWriter.WriteAttributeString("LongName", this.LongName);
            xmlWriter.WriteAttributeString("CatalogBranchLevel", this.CatalogBranchLevel.ToString());
            xmlWriter.WriteAttributeString("Action", this.Action.ToString());

            xmlWriter.WriteEndElement();
            xmlWriter.Flush();

            xml = stringWriter.ToString();

            xmlWriter.Close();
            stringWriter.Close();

            return xml;
        }

        /// <summary>
        /// Clone EntityType object
        /// </summary>
        /// <returns>cloned copy of EntityType object.</returns>
        public IEntityType Clone()
        {
            EntityType clonedEntityType = new EntityType();

            clonedEntityType.Id = this.Id;
            clonedEntityType.Name = this.Name;
            clonedEntityType.LongName = this.LongName;
            clonedEntityType.Locale = this.Locale;
            clonedEntityType.Action = this.Action;
            clonedEntityType.AuditRefId = this.AuditRefId;
            clonedEntityType.ExtendedProperties = this.ExtendedProperties;

            clonedEntityType.EntityTypes = (EntityTypeCollection)this.EntityTypes.Clone();
            clonedEntityType.CatalogBranchLevel = this.CatalogBranchLevel;

            return clonedEntityType;
        }

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="subsetEntityType">The Object to compare with the current Object.</param>
        /// <param name="compareIds">Indicates if Ids to be compared or not.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public Boolean IsSuperSetOf(EntityType subsetEntityType, Boolean compareIds = false)
        {
            if (subsetEntityType != null)
            {
                if (base.IsSuperSetOf(subsetEntityType, compareIds))
                {
                    if (this.CatalogBranchLevel != subsetEntityType.CatalogBranchLevel)
                        return false;

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Delta Merge of entity type
        /// </summary>
        /// <param name="deltaEntityType">Entity Type that needs to be merged.</param>
        /// <param name="iCallerContext">Indicates the name of application and the module that are performing the action</param>
        /// <param name="returnClonedObject">true means return new merged cloned object;otherwise, same object.</param>
        /// <returns>Merged entity type instance</returns>
        public IEntityType MergeDelta(IEntityType deltaEntityType, ICallerContext iCallerContext, Boolean returnClonedObject = true)
        {
            IEntityType mergedEntityType = (returnClonedObject == true) ? deltaEntityType.Clone() : deltaEntityType; 

            mergedEntityType.Action = (mergedEntityType.Equals(this)) ? ObjectAction.Read : ObjectAction.Update;

            return mergedEntityType;
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

        #endregion

        #endregion

        #region Overrides

        /// <summary>
        /// Determines whether specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">EntityType object which needs to be compared.</param>
        /// <returns>Result of the comparison in Boolean.</returns>
        public override bool Equals(object obj)
        {
            if (obj is EntityType)
            {
                EntityType objectToBeCompared = obj as EntityType;

                if (!base.Equals(objectToBeCompared))
                    return false;

                if (this.CatalogBranchLevel != objectToBeCompared.CatalogBranchLevel)
                    return false;

                if (this.EntityTypes.Count != objectToBeCompared.EntityTypes.Count)
                    return false;

                // Compare child entity type collection
                var matchedChildEntityTypes = from p in this.EntityTypes
                                              join q in objectToBeCompared.EntityTypes
                                              on p.GetHashCode() equals q.GetHashCode()
                                              select p;

                if (matchedChildEntityTypes.Count() != this.EntityTypes.Count)
                    return false;

                return true;
            }

            return false;
        }

        /// <summary>
        /// Returns the hash code for this instance
        /// </summary>
        /// <returns>Hash code of the instance</returns>
        public override Int32 GetHashCode()
        {
            return base.GetHashCode() ^ this.CatalogBranchLevel.GetHashCode();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Load EntityType object from Xml
        /// </summary>
        /// <param name="valuesAsXml">Xml having xml value</param>
        private void LoadEntityType(String valuesAsXml)
        {
            XmlTextReader reader = null;
            try
            {
                reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                while (!reader.EOF)
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "EntityType")
                    {
                        #region Read entity type properties

                        if (reader.HasAttributes)
                        {
                            if (reader.MoveToAttribute("Id"))
                            {
                                this.Id = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                            }

                            if (reader.MoveToAttribute("Name"))
                            {
                                this.Name = reader.ReadContentAsString();
                            }

                            if (reader.MoveToAttribute("LongName"))
                            {
                                this.LongName = reader.ReadContentAsString();
                            }

                            if (reader.MoveToAttribute("CatalogBranchLevel"))
                            {
                                this.CatalogBranchLevel = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                            }

                            if (reader.MoveToAttribute("Action"))
                            {
                                this.Action = ValueTypeHelper.GetAction(reader.ReadContentAsString());
                            }
                        }

                        #endregion
                    }
                    else
                    {
                        //Keep on reading the xml until we reach expected node.
                        reader.Read();
                    }
                }
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        #endregion

        #endregion Methods
    }
}