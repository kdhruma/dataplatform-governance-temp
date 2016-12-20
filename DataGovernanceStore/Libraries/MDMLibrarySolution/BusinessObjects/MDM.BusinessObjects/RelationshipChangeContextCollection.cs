using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Interfaces;
    using MDM.Core.Extensions;

    /// <summary>
    /// Specifies collection of change context of relationship.
    /// </summary>
    [DataContract]
    [ProtoContract]
    public class RelationshipChangeContextCollection : InterfaceContractCollection<IRelationshipChangeContext, RelationshipChangeContext>, IRelationshipChangeContextCollection
    {
        #region Fields
        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the relationship change context Collection
        /// </summary>
        public RelationshipChangeContextCollection()
        {
        }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        /// <param name="valuesAsXml">XML having xml values</param>
        public RelationshipChangeContextCollection(String valuesAsXml)
        {
            LoadRelationshipChangeContextCollection(valuesAsXml);
        }

        /// <summary>
        /// Converts list into current instance
        /// </summary>
        /// <param name="relationshpiChangeContextList">List of attribute change contexts</param>
        public RelationshipChangeContextCollection(IList<RelationshipChangeContext> relationshpiChangeContextList)
        {
            if (relationshpiChangeContextList != null)
            {
                this._items = new Collection<RelationshipChangeContext>(relationshpiChangeContextList);
            }
        }

        #endregion Constructors

        #region Properties
        #endregion Properties

        #region Methods

        #region Public Methods

        /// <summary>
        /// Adds relationship change context collection to existing collection
        /// </summary>
        /// <param name="relationshipChangeContexts">Indicates relationship change contexts to be added</param>
        public void AddRange(RelationshipChangeContextCollection relationshipChangeContexts)
        {
            if (relationshipChangeContexts != null && relationshipChangeContexts.Count > 0)
            {
                foreach (RelationshipChangeContext relationshipChangeContext in relationshipChangeContexts)
                {
                    this.Add(relationshipChangeContext);
                }
            }
        }

        /// <summary>
        /// Get Xml representation of AttributeChangeContext Collection
        /// </summary>
        /// <returns>Xml representation of AttributeChangeContext Collection</returns>
        public String ToXml()
        {
            String returnXml = String.Empty;

            using (StringWriter sw = new StringWriter())
            {
                using (XmlTextWriter xmlWriter = new XmlTextWriter(sw))
                {
                    //RelationshipChangeContextCollection node start
                    xmlWriter.WriteStartElement("RelationshipChangeContexts");

                    #region Write RelationshipChangeContextCollection

                    if (_items != null)
                    {
                        foreach (RelationshipChangeContext relationshipChangeContext in this._items)
                        {
                            xmlWriter.WriteRaw(relationshipChangeContext.ToXml());
                        }
                    }

                    #endregion

                    //RelationshipChangeContextCollection node end
                    xmlWriter.WriteEndElement();
                    xmlWriter.Flush();

                    //Get the actual XML
                    returnXml = sw.ToString();
                }
            }

            return returnXml;
        }

        /// <summary>
        /// Gets RelationshipChangeContext based on relationship id
        /// </summary>
        /// <param name="relationshipId">Indicates relationship id for which relationship change context to be retrieved</param>
        /// <returns>RelationshipChangeContext based on given relationship id</returns>
        public RelationshipChangeContext Get(Int64 relationshipId)
        {
            if (this._items != null && this._items.Count > 0)
            {
                foreach (RelationshipChangeContext relationshipChangeContext in this._items)
                {
                    if (relationshipChangeContext.RelationshipId == relationshipId)
                    {
                        return relationshipChangeContext;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the relationship type name list
        /// </summary>
        /// <returns>Returns relationship type name list</returns>
        public Collection<String> GetRelationshipTypeNameList()
        {
            Collection<String> relationshipTypeNameList = null;

            if (this._items.Count > 0)
            {
                relationshipTypeNameList = new Collection<String>();

                foreach (RelationshipChangeContext relationshipChangeContext in this._items)
                {
                    relationshipTypeNameList.Add(relationshipChangeContext.RelationshipTypeName);
                }
            }

            return relationshipTypeNameList;
        }

        /// <summary>
        /// Gets the relationship attribute id list from relationship change contexts
        /// </summary>
        /// <returns>Returns relationship attribute id list</returns>
        public Collection<Int32> GetRelationshipAttributeIdList()
        {
            Collection<Int32> relationshipAttributeIdList = null;

            if (this._items.Count > 0)
            {
                relationshipAttributeIdList = new Collection<Int32>();

                foreach (RelationshipChangeContext relationshipChangeContext in this._items)
                {
                    Collection<Int32> attributeIdList = relationshipChangeContext.AttributeChangeContexts.GetAttributeIdList();

                    if (attributeIdList != null && attributeIdList.Count > 0)
                    {
                        relationshipAttributeIdList.AddRange<Int32>(attributeIdList);
                    }
                }
            }

            return relationshipAttributeIdList;
        }

        /// <summary>
        /// Gets the relationship id list from relationship change contexts
        /// </summary>
        /// <returns>Returns relationship id list</returns>
        public Collection<Int64> GetRelationshipIdList()
        {
            Collection<Int64> relationshipIdList = null;

            if (this._items.Count > 0)
            {
                relationshipIdList = new Collection<Int64>();

                foreach (RelationshipChangeContext relationshipChangeContext in this._items)
                {
                    relationshipIdList.Add(relationshipChangeContext.RelationshipId);
                }
            }

            return relationshipIdList;
        }

        /// <summary>
        /// Gets the relationship type id list from relationship change contexts
        /// </summary>
        /// <returns>Returns relationship type id list</returns>
        public Collection<Int32> GetRelationshipTypeIdList()
        {
            Collection<Int32> relationshipTypeIdList = null;

            if (this._items.Count > 0)
            {
                relationshipTypeIdList = new Collection<Int32>();

                foreach (RelationshipChangeContext relationshipChangeContext in this._items)
                {
                    relationshipTypeIdList.Add(relationshipChangeContext.RelationshipTypeId);
                }
            }

            return relationshipTypeIdList;
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valuesAsXml"></param>
        private void LoadRelationshipChangeContextCollection(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "RelationshipChangeContext")
                        {
                            String relationshipChangeContextXml = reader.ReadOuterXml();

                            if (!String.IsNullOrEmpty(relationshipChangeContextXml))
                            {
                                RelationshipChangeContext relationshipChangeContext = new RelationshipChangeContext(relationshipChangeContextXml);

                                if (relationshipChangeContext != null)
                                {
                                    this.Add(relationshipChangeContext);
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
        }

        #endregion Private Methods

        #endregion Methods
    }
}