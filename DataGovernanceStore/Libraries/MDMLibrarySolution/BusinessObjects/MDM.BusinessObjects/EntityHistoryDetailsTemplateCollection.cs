using System;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies class for Entity History Details Template Collection
    /// </summary>
    [DataContract]
    public class EntityHistoryDetailsTemplateCollection : InterfaceContractCollection<IEntityHistoryDetailsTemplate, EntityHistoryDetailsTemplate>, IEntityHistoryDetailsTemplateCollection
    {
        #region Fields

        #endregion

        #region Constructor

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public EntityHistoryDetailsTemplateCollection()
            : base()
        {
        }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        public EntityHistoryDetailsTemplateCollection(String valueAsXml)
        {
            LoadEntityHistoryDetailsTemplateCollection(valueAsXml);
        }

        #endregion

        #region Properties

        #endregion

        #region Methods

        #region Public Methods

        #region Load Methods

        /// <summary>
        /// Load Entity History Details Template Collection from Xml
        /// <param name="valuesAsXml">xml Having data to load object</param>
        /// </summary>
        public void LoadEntityHistoryDetailsTemplateCollection(String valuesAsXml)
        {

            #region SampleXml
            //      <EntityHistoryDetailsTemplateCollection>
            //         <EntityHistoryDetailsTemplate ChangeType="CommonAttribute" TemplateCode="456009" TemplateText="##AttributeLongName##(##AttributeParentLongName##) value ##AttributeValue## added" Description="">
            //         </EntityHistoryDetailsTemplate>
            //      </EntityHistoryDetailsTemplateCollection>
            #endregion

            XmlTextReader reader = null;
            try
            {
                reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                while (!reader.EOF)
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "EntityHistoryDetailsTemplate")
                    {
                        String entityHistoryDetailsTemplateXml = reader.ReadOuterXml();
                        if (!String.IsNullOrEmpty(entityHistoryDetailsTemplateXml))
                        {
                            EntityHistoryDetailsTemplate entityHistoryDetailsTemplate = new EntityHistoryDetailsTemplate(entityHistoryDetailsTemplateXml);
                            if (entityHistoryDetailsTemplate != null)
                            {
                                this.Add(entityHistoryDetailsTemplate);
                            }
                        }
                    }
                    else
                    {
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

        #region Override Methods

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (obj is EntityHistoryDetailsTemplateCollection)
            {
                EntityHistoryDetailsTemplateCollection objectToBeCompared = obj as EntityHistoryDetailsTemplateCollection;
                Int32 entityHistoryDetailsTemplatesUnion = this._items.ToList().Union(objectToBeCompared.ToList()).Count();
                Int32 entityHistoryDetailsTemplatesIntersect = this._items.ToList().Intersect(objectToBeCompared.ToList()).Count();

                if (entityHistoryDetailsTemplatesUnion != entityHistoryDetailsTemplatesIntersect)
                    return false;

                return true;
            }

            return false;
        }

        /// <summary>
        /// Serves as a hash function for type
        /// </summary>
        /// <returns>A hash code for the current Object.</returns>
        public override int GetHashCode()
        {
            Int32 hashCode = 0;

            foreach (EntityHistoryDetailsTemplate entityHistoryDetailsTemplate in this._items)
            {
                hashCode += entityHistoryDetailsTemplate.GetHashCode();
            }

            return hashCode;
        }

        /// <summary>
        /// Determines whether the actual collection is superset of the subset collection or not.
        /// </summary>
        /// <param name="subsetEntityHistoryTemplateCollection">Indicates the subset collection of entity history template.</param>
        /// <returns>Returns true it is superset of subset collection, otherwise returns false.</returns>
        public Boolean IsSuperSetOf(EntityHistoryDetailsTemplateCollection subsetEntityHistoryTemplateCollection)
        {
            foreach (EntityHistoryDetailsTemplate entityHistoryTemplate in subsetEntityHistoryTemplateCollection)
            {
                EntityHistoryDetailsTemplate subsetEntityHistoryTemplate = this.Where(e => e.ChangeType == entityHistoryTemplate.ChangeType && e.TemplateCode == entityHistoryTemplate.TemplateCode).ToList<EntityHistoryDetailsTemplate>().FirstOrDefault();

                if (subsetEntityHistoryTemplate == null)
                {
                    return false;
                }

                if (!entityHistoryTemplate.IsSuperSetOf(subsetEntityHistoryTemplate))
                {
                    return false;
                }
            }

            return true;
        }

        #endregion

        #region IEntityHistoryDetailsTemplateCollection Members

        #region ToXml methods

        /// <summary>
        /// Get Xml representation of Entity History Details Template Collection object
        /// </summary>
        /// <returns>Xml string representing the Entity History Details Template Collection</returns>
        public String ToXml()
        {
            String returnXml = String.Empty;

            returnXml = "<EntityHistoryDetailsTemplateCollection>";

            if (this._items != null && this._items.Count > 0)
            {
                foreach (EntityHistoryDetailsTemplate entityHistoryDetailsTemplate in this._items)
                {
                    returnXml = String.Concat(returnXml, entityHistoryDetailsTemplate.ToXml());
                }
            }

            returnXml = String.Concat(returnXml, "</EntityHistoryDetailsTemplateCollection>");

            return returnXml;
        }

        /// <summary>
        /// Get Xml representation of Entity History Details Template Collection based on requested object serialization
        /// </summary>
        /// <param name="objectSerialization"></param>
        /// <returns></returns>
        public String ToXml(ObjectSerialization objectSerialization)
        {
            String returnXml = String.Empty;

            returnXml = "<EntityHistoryDetailsTemplateCollection>";

            if (this._items != null && this._items.Count > 0)
            {
                foreach (EntityHistoryDetailsTemplate entityHistoryDetailsTemplate in this._items)
                {
                    returnXml = String.Concat(returnXml, entityHistoryDetailsTemplate.ToXml(objectSerialization));
                }
            }

            returnXml = String.Concat(returnXml, "</EntityHistoryDetailsTemplateCollection>");

            return returnXml;
        }

        #endregion ToXml methods

        #endregion IEntityHistoryDetailsTemplateCollection Members

        /// <summary>
        /// Gets EntityHistoryDetailsTemplate based on change type and action
        /// </summary>
        /// <param name="changeType">Type of change is being done in entity</param>
        /// <param name="action">Action which is being performed on entity</param>
        /// <returns>EntityHistoryDetailsTemplate</returns>
        public EntityHistoryDetailsTemplate GetEntityHistoryDetailsTemplate(EntityChangeType changeType, ObjectAction action)
        {
            EntityHistoryDetailsTemplate entityHistoryDetailsTemplate = null;

            if (_items != null)
            {
                foreach (EntityHistoryDetailsTemplate currentEntityHistoryDetailsTemplate in _items)
                {
                    if (currentEntityHistoryDetailsTemplate.ChangeType == changeType && currentEntityHistoryDetailsTemplate.Action == action)
                    {
                        entityHistoryDetailsTemplate = currentEntityHistoryDetailsTemplate;
                        break;
                    }
                }
            }

            return entityHistoryDetailsTemplate;
        }

        #endregion Public Methods

        #region Private Methods

        #endregion

        #endregion
    }
}
