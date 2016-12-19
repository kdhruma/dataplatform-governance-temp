using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies the EntityCacheInvalidateContext Collection for the Object
    /// </summary>
    [DataContract]
    public class EntityCacheInvalidateContextCollection : InterfaceContractCollection<IEntityCacheInvalidateContext, EntityCacheInvalidateContext>, IEntityCacheInvalidateContextCollection
    {
        #region Fields

        /// <summary>
        /// Indicates collection of EntityCacheInvalidateContext Collection.
        /// </summary>
        [DataMember]
        private Collection<EntityCacheInvalidateContext> _entityCacheInvalidateContexts = new Collection<EntityCacheInvalidateContext>();

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public EntityCacheInvalidateContextCollection() { }

        /// <summary>
        /// Constructor which takes XML as input
        /// </summary>
        public EntityCacheInvalidateContextCollection(String valueAsXml, ObjectSerialization objectSerialization = ObjectSerialization.Full)
        {
            LoadRelationshipDenormActionCollection(valueAsXml, objectSerialization);
        }

        /// <summary>
        /// Initialize entityCacheInvalidateContextCollection from IList
        /// </summary>
        /// <param name="entityCacheInvalidateContextList">IList of entityCacheInvalidateContext</param>
        public EntityCacheInvalidateContextCollection(IList<EntityCacheInvalidateContext> entityCacheInvalidateContextList)
        {
            if (entityCacheInvalidateContextList != null)
            {
                this._entityCacheInvalidateContexts = new Collection<EntityCacheInvalidateContext>(entityCacheInvalidateContextList);
            }
        }

        #endregion Constructors

        #region Properties
        #endregion

        #region Methods
        
        #region Public Methods

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            return false;
        }

        /// <summary>
        /// Serves as a hash function for type
        /// </summary>
        /// <returns>A hash code for the current Object.</returns>
        public override int GetHashCode()
        {
            Int32 hashCode = 0;

            foreach (EntityCacheInvalidateContext entityCacheInvalidateContext in this._entityCacheInvalidateContexts)
            {
                hashCode += entityCacheInvalidateContext.GetHashCode();
            }

            return hashCode;
        }

        #region IRelationshipDenormActionCollection Members

        #region ToXml methods

        /// <summary>
        /// Get XML representation of entityCacheInvalidateContext collection object
        /// </summary>
        /// <returns>XML string representing the entityCacheInvalidateContext collection</returns>
        public string ToXml()
        {
            String entityCacheInvalidateContextXml = String.Empty;

            entityCacheInvalidateContextXml = "<EntityCacheInvalidateContexts>";

            if (this._entityCacheInvalidateContexts != null && this._entityCacheInvalidateContexts.Count > 0)
            {
                foreach (EntityCacheInvalidateContext entityCacheInvalidateContext in this._entityCacheInvalidateContexts)
                {
                    entityCacheInvalidateContextXml = String.Concat(entityCacheInvalidateContextXml, entityCacheInvalidateContext.ToXml());
                }
            }

            entityCacheInvalidateContextXml = String.Concat(entityCacheInvalidateContextXml, "</EntityCacheInvalidateContexts>");

            return entityCacheInvalidateContextXml;
        }

        /// <summary>
        /// Get XML representation of EntityCacheInvalidateContext collection object
        /// </summary>
        /// <param name="objectSerialization">objectSerialization option. Based on the value selected, the different XML representation will be there</param>
        /// <returns>XML string representing the EntityCacheInvalidateContext collection</returns>
        public string ToXml(ObjectSerialization objectSerialization)
        {
            String relationshipDenormActionsXml = String.Empty;

            //TODO:: Write logic as per the different object serialization type whenever needed..
            relationshipDenormActionsXml = this.ToXml();

            return relationshipDenormActionsXml;
        }

        #endregion ToXml methods

        #endregion IRelationshipDenormActionCollection Members

        #endregion Public Methods

        #region Private Methods

        ///<summary>
        /// Load EntityCacheInvalidateContext collection object from XML.
        /// </summary>
        /// <param name="valuesAsXml">XML having XML value</param>
        /// <param name="objectSerialization">Specifies the object serialization type</param>
        /// <example>
        ///     Sample XML:
        /// </example>
        private void LoadRelationshipDenormActionCollection(String valuesAsXml, ObjectSerialization objectSerialization = ObjectSerialization.Full)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;

                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "EntityCacheInvalidateContexts")
                        {
                            #region Read EntityCacheInvalidateContext Collection

                            String entityCacheInvalidateContextXml = reader.ReadOuterXml();

                            if (!String.IsNullOrWhiteSpace(entityCacheInvalidateContextXml))
                            {
                                EntityCacheInvalidateContext entityCacheInvalidateContext = new EntityCacheInvalidateContext(entityCacheInvalidateContextXml, objectSerialization);

                                if (entityCacheInvalidateContext != null)
                                {
                                    this.Add(entityCacheInvalidateContext);
                                }
                            }

                            #endregion
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
        }

        #endregion Private Methods
#endregion
    }
}