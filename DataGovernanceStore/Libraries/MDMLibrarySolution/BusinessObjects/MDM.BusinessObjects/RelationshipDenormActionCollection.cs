using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;
    using System.Collections;

    /// <summary>
    /// Specifies the RelationshipDenormAction Collection for the Object
    /// </summary>
    [DataContract]
    public class RelationshipDenormActionCollection : InterfaceContractCollection<IRelationshipDenormAction, RelationshipDenormAction>, IRelationshipDenormActionCollection
    {
        #region Fields

        /// <summary>
        /// Indicates collection of Relationship Denorm Action Collection.
        /// </summary>
        [DataMember]
        private Collection<RelationshipDenormAction> _relationshipDenormActions = new Collection<RelationshipDenormAction>();

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public RelationshipDenormActionCollection() { }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        public RelationshipDenormActionCollection(String valueAsXml, ObjectSerialization objectSerialization = ObjectSerialization.Full)
        {
            LoadRelationshipDenormActionCollection(valueAsXml, objectSerialization);
        }

        /// <summary>
        /// Initialize relationshipDenormActionCollection from IList
        /// </summary>
        /// <param name="relationshipDenormAction">IList of relationshipDenormAction</param>
        public RelationshipDenormActionCollection(IList<RelationshipDenormAction> relationshipDenormAction)
        {
            if (relationshipDenormAction != null)
            {
                this._relationshipDenormActions = new Collection<RelationshipDenormAction>(relationshipDenormAction);
            }
        }

        #endregion Constructors

        #region Properties

        #endregion Properties

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

            foreach (RelationshipDenormAction RelationshipDenormAction in this._relationshipDenormActions)
            {
                hashCode += RelationshipDenormAction.GetHashCode();
            }

            return hashCode;
        }

        #region IRelationshipDenormActionCollection Members

        #region ToXml methods

        /// <summary>
        /// Get Xml representation of RelationshipDenormActionCollection object
        /// </summary>
        /// <returns>Xml string representing the RelationshipDenormActionCollection</returns>
        public string ToXml()
        {
            String relationshipDenormActionsXml = String.Empty;

            relationshipDenormActionsXml = "<RelationshipDenormActions>";

            if (this._relationshipDenormActions != null && this._relationshipDenormActions.Count > 0)
            {
                foreach (RelationshipDenormAction relationshipDenormAction in this._relationshipDenormActions)
                {
                    relationshipDenormActionsXml = String.Concat(relationshipDenormActionsXml, relationshipDenormAction.ToXml());
                }
            }

            relationshipDenormActionsXml = String.Concat(relationshipDenormActionsXml, "</RelationshipDenormActions>");

            return relationshipDenormActionsXml;
        }

        /// <summary>
        /// Get Xml representation of RelationshipDenormActionCollection object
        /// </summary>
        /// <param name="objectSerialization">objectSerialization option. Based on the value selected, the different xml representation will be there</param>
        /// <returns>Xml string representing the RelationshipDenormActionCollection</returns>
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
        /// Load RelationshipDenormActionCollection object from XML.
        /// </summary>
        /// <param name="valuesAsXml">XML having xml value</param>
        /// <param name="objectSerialization">Specifies the object serialization type</param>
        /// <example>
        ///     Sample XML:
        /// </example>
        private void LoadRelationshipDenormActionCollection(String valuesAsXml, ObjectSerialization objectSerialization = ObjectSerialization.Full)
        {
            #region Sample Xml
            /*
             * <RelationshipDenormActions>
             *   <RelationshipDenormAction Action="RelationshipCreate" ExtensionProcessingMode="ASync" HierarchyProcessingMode="ASync" WhereUsedProcessingMode="ASync" RelationshipTreeProcessingMode="ASync" />
             *   <RelationshipDenormAction Action="RelationshipUpdate" ExtensionProcessingMode="ASync" HierarchyProcessingMode="ASync" WhereUsedProcessingMode="ASync" RelationshipTreeProcessingMode="ASync" />
             * </RelationshipDenormActions>

             */
            #endregion Sample Xml

            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "RelationshipDenormAction")
                        {
                            #region Read RelationshipDenormAction Collection

                            String relationshipDenormActionXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(relationshipDenormActionXml))
                            {
                                RelationshipDenormAction relationshipDenormAction = new RelationshipDenormAction(relationshipDenormActionXml, objectSerialization);
                                if (relationshipDenormAction != null)
                                {
                                    this.Add(relationshipDenormAction);
                                }
                            }

                            #endregion Read RelationshipDenormAction Collection
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

    }
}
