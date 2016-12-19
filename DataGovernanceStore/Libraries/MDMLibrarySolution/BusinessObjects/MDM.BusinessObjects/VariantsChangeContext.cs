using ProtoBuf;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies the change context of a variants.
    /// </summary>
    [DataContract]
    [ProtoContract]
    public class VariantsChangeContext : ObjectBase, IVariantsChangeContext
    {
        #region Fields

        /// <summary>
        /// Indicates collection of EaaH change contexts for a variants.
        /// </summary>
        private EntityChangeContextCollection _entityChangeContexts = null;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Parameter-less Constructor
        /// </summary>
        public VariantsChangeContext()
            : base()
        {
        }

        /// <summary>
        /// Converts XML into current instance
        /// </summary>
        /// <param name="valuesAsXml">Specifies xml representation of instance</param>
        public VariantsChangeContext(String valuesAsXml)
        {
            LoadVariantsChangeContext(valuesAsXml);
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Specifies collection of locale change context including attribute change contexts.
        /// </summary>
        [DataMember]
        [ProtoMember(1)]
        public EntityChangeContextCollection EntityChangeContexts
        {
            get
            {
                if (this._entityChangeContexts == null)
                {
                    this._entityChangeContexts = new EntityChangeContextCollection();
                }

                return this._entityChangeContexts;
            }
            set
            {
                this._entityChangeContexts = value;
            }
        }

        #endregion Properties

        #region Methods

        #region Public Methods

        /// <summary>
        /// Gives xml representation of an instance
        /// </summary>
        /// <returns>String xml representation</returns>
        public String ToXml()
        {
            String returnXml = String.Empty;

            using (StringWriter sw = new StringWriter())
            {
                using (XmlTextWriter xmlWriter = new XmlTextWriter(sw))
                {
                    //VariantsChangeContext node start
                    xmlWriter.WriteStartElement("VariantsChangeContext");

                    #region write EaaH change context

                    if (this._entityChangeContexts != null)
                    {
                        xmlWriter.WriteRaw(this.EntityChangeContexts.ToXml());
                    }

                    #endregion write EaaH change context

                    //VariantsChangeContext node end
                    xmlWriter.WriteEndElement();
                    xmlWriter.Flush();

                    //get the actual XML
                    returnXml = sw.ToString();
                }
            }

            return returnXml;
        }

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override Boolean Equals(Object obj)
        {
            if (obj is VariantsChangeContext)
            {
                VariantsChangeContext objectToBeCompared = obj as VariantsChangeContext;

                if (!this.EntityChangeContexts.Equals(objectToBeCompared.EntityChangeContexts))
                {
                    return false;
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Serves as a hash function for type
        /// </summary>
        /// <returns>A hash code for the current Object.</returns>
        public override Int32 GetHashCode()
        {
            Int32 hashCode = 0;

            hashCode = this.EntityChangeContexts.GetHashCode();

            return hashCode;
        }

        /// <summary>
        /// Delta Merge of variants change context
        /// </summary>
        /// <param name="deltaVariantsChangeContext">Indicates delta variants change context needs to be merged</param>
        public void Merge(VariantsChangeContext deltaVariantsChangeContext)
        {
            EntityChangeContextCollection deltaEntityChangeContexts = deltaVariantsChangeContext._entityChangeContexts;

            //Merge only if we have anything from delta.
            if (deltaEntityChangeContexts != null && deltaEntityChangeContexts.Count > 0)
            {
                //We don't have any into current collection. Take all data from delta and update to current collection.
                if (this._entityChangeContexts == null || this._entityChangeContexts.Count < 1)
                {
                    this._entityChangeContexts = deltaEntityChangeContexts;
                }
                else
                {
                    this._entityChangeContexts.Merge(deltaEntityChangeContexts);
                }
            }
        }

        #region Entity Change Contexts related methods

        /// <summary>
        /// Gets the entity change contexts
        /// </summary>
        /// <returns>Return entity change context.</returns>
        public IEntityChangeContextCollection GetEntityChangeContexts()
        {
            if (this._entityChangeContexts == null)
            {
                return null;
            }

            return (IEntityChangeContextCollection)this.EntityChangeContexts;
        }

        /// <summary>
        /// Sets the entity change context.
        /// </summary>
        /// <param name="iEntityChangeContexts">Indicates the entity change contexts to be set</param>
        public void SetEntityChangeContexts(IEntityChangeContextCollection iEntityChangeContexts)
        {
            this.EntityChangeContexts = (EntityChangeContextCollection)iEntityChangeContexts;
        }

        #endregion Entity Change Contexts related methods

        #region Helper Methods

        /// <summary>
        /// Gets the attribute id list from variants change context
        /// </summary>
        /// <returns>Returns attribute id list</returns>
        public Collection<Int32> GetAttributeIdList()
        {
            Collection<Int32> attributeIdList = null;

            if (this._entityChangeContexts != null && this._entityChangeContexts.Count > 0)
            {
                attributeIdList = this._entityChangeContexts.GetAttributeIdList();
            }

            return attributeIdList;
        }

        /// <summary>
        /// Gets the attribute ids from variants change context based on given object action
        /// </summary>
        /// <param name="action">Indicates object action</param>
        /// <returns>Returns attribute id list</returns>
        public Collection<Int32> GetAttributeIdList(ObjectAction action)
        {
            Collection<Int32> attributeIdList = null;

            if (this._entityChangeContexts != null && this._entityChangeContexts.Count > 0)
            {
                attributeIdList = this._entityChangeContexts.GetAttributeIdList(action);
            }

            return attributeIdList;
        }

        /// <summary>
        /// Gets the attribute locale list from variants change context
        /// </summary>
        /// <returns>Returns attribute locale list</returns>
        public Collection<LocaleEnum> GetAttributeLocaleList()
        {
            Collection<LocaleEnum> attributeLocaleList = null;

            if (this._entityChangeContexts != null && this._entityChangeContexts.Count > 0)
            {
                attributeLocaleList = this._entityChangeContexts.GetAttributeLocaleList();
            }

            return attributeLocaleList;
        }

        /// <summary>
        /// Gets the attribute name list from variants change context
        /// </summary>
        /// <returns>Returns attribute name list</returns>
        public Collection<String> GetAttributeNameList()
        {
            Collection<String> attributeNameList = null;

            if (this._entityChangeContexts != null && this._entityChangeContexts.Count > 0)
            {
                attributeNameList = this._entityChangeContexts.GetAttributeNameList();
            }

            return attributeNameList;
        }

        /// <summary>
        /// Gets the relationship type id list from variants change contexts
        /// </summary>
        /// <returns>Returns relationship type id list</returns>
        public Collection<Int32> GetRelationshipTypeIdList()
        {
            Collection<Int32> relationshipTypeIdList = null;

            if (this._entityChangeContexts != null && this._entityChangeContexts.Count > 0)
            {
                relationshipTypeIdList = this._entityChangeContexts.GetRelationshipTypeIdList();
            }

            return relationshipTypeIdList;
        }

        /// <summary>
        /// Gets the relationship type name list from entity change contexts
        /// </summary>
        /// <returns>Returns relationship type name list</returns>
        public Collection<String> GetRelationshipTypeNameList()
        {
            Collection<String> relationshipTypeNameList = null;

            if (this._entityChangeContexts != null && this._entityChangeContexts.Count > 0)
            {
                relationshipTypeNameList = this._entityChangeContexts.GetRelationshipTypeNameList();
            }

            return relationshipTypeNameList;
        }

        /// <summary>
        /// Gets the relationship attribute id list from entity change contexts
        /// </summary>
        /// <returns>Returns relationship attribute id list</returns>
        public Collection<Int32> GetRelationshipAttributeIdList()
        {
            Collection<Int32> relationshipAttributeIdList = null;

            if (this._entityChangeContexts != null && this._entityChangeContexts.Count > 0)
            {
                relationshipAttributeIdList = this._entityChangeContexts.GetRelationshipAttributeIdList();
            }

            return relationshipAttributeIdList;
        }

        /// <summary>
        /// Gets the relationship attribute locale list from entity change contexts
        /// </summary>
        /// <returns>Returns relationship attribute locale list</returns>
        public Collection<LocaleEnum> GetRelationshipAttributeLocaleList()
        {
            Collection<LocaleEnum> relationshipAttributeLocaleList = null;

            if (this._entityChangeContexts != null && this._entityChangeContexts.Count > 0)
            {
                relationshipAttributeLocaleList = this._entityChangeContexts.GetRelationshipAttributeLocaleList();
            }

            return relationshipAttributeLocaleList;
        }

        /// <summary>
        /// Gets the relationship attribute name list from entity change contexts
        /// </summary>
        /// <returns>Returns relationship attribute name list</returns>
        public Collection<String> GetRelationshipAttributeNameList()
        {
            Collection<String> relationshipAttributeNameList = null;

            if (this._entityChangeContexts != null && this._entityChangeContexts.Count > 0)
            {
                relationshipAttributeNameList = this._entityChangeContexts.GetRelationshipAttributeNameList();
            }

            return relationshipAttributeNameList;
        }

        /// <summary>
        /// Checks whether relationship object has been changed i.e any object having Action as Create, Update or Delete 
        /// </summary>
        /// <returns>Return true if object is changed else false</returns>
        public Boolean HasRelationshipsChanged()
        {
            Boolean hasRelationshipsChanged = false;

            if (this._entityChangeContexts != null && this._entityChangeContexts.Count > 0)
            {
                hasRelationshipsChanged = this._entityChangeContexts.HasRelationshipsChanged();
            }

            return hasRelationshipsChanged;
        }

        /// <summary>
        /// Checks whether any entity change context action is Create 
        /// </summary>
        /// <returns>Return true if object is having Create action</returns>
        public Boolean HasEntitiesCreated()
        {
            Boolean hasEntitiesCreated = false;

            if (this._entityChangeContexts != null && this._entityChangeContexts.Count > 0)
            {
                hasEntitiesCreated = this._entityChangeContexts.HasEntitiesCreated();
            }

            return hasEntitiesCreated;
        }

        /// <summary>
        /// Gets entity id list based on given object action.
        /// </summary>
        /// <param name="objectActions">Indicates collection of object action</param>
        /// <returns>Returns collection of entity id list for given object action</returns>
        public Collection<Int64> GetEntityIdList(Collection<ObjectAction> objectActions)
        {
            Collection<Int64> entityIdList = null;

            if (this._entityChangeContexts != null && this._entityChangeContexts.Count > 0)
            {
                entityIdList = this._entityChangeContexts.GetEntityIdList(objectActions);
            }

            return entityIdList;
        }

        #endregion Helper Methods

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valuesAsXml"></param>
        private void LoadVariantsChangeContext(String valuesAsXml)
        {
            XmlTextReader reader = null;
            try
            {
                reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                while (!reader.EOF)
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "EntityChangeContexts")
                    {
                        #region Read EntityChangeContexts

                        String entityChangeContextsXml = reader.ReadOuterXml();

                        if (!String.IsNullOrEmpty(entityChangeContextsXml))
                        {
                            EntityChangeContextCollection entityChangeContexts = new EntityChangeContextCollection(entityChangeContextsXml);

                            if (entityChangeContexts != null && entityChangeContexts.Count > 0)
                            {
                                this.EntityChangeContexts = entityChangeContexts;
                            }
                        }

                        #endregion Read EntityChangeContexts
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

        #endregion Private Methods

        #endregion Methods
    }
}