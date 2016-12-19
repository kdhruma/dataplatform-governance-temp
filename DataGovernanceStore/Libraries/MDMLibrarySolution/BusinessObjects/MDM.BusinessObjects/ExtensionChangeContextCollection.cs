using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Core.Extensions;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies collection of change context of extension.
    /// </summary>
    [DataContract]
    [ProtoContract]
    public class ExtensionChangeContextCollection : InterfaceContractCollection<IExtensionChangeContext, ExtensionChangeContext>, IExtensionChangeContextCollection
    {
        #region Fields
        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the relationship change context Collection
        /// </summary>
        public ExtensionChangeContextCollection()
        {
        }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        /// <param name="valuesAsXml">XML having xml values</param>
        public ExtensionChangeContextCollection(String valuesAsXml)
        {
            LoadExtensionChangeContextCollection(valuesAsXml);
        }

        /// <summary>
        /// Converts list into current instance
        /// </summary>
        /// <param name="extensionChangeContextList">List of attribute change contexts</param>
        public ExtensionChangeContextCollection(IList<ExtensionChangeContext> extensionChangeContextList)
        {
            if (extensionChangeContextList != null)
            {
                this._items = new Collection<ExtensionChangeContext>(extensionChangeContextList);
            }
        }

        #endregion Constructors

        #region Properties
        #endregion Properties

        #region Methods

        #region Public Methods

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
                    //ExtensionChangeContextCollection node start
                    xmlWriter.WriteStartElement("ExtensionChangeContexts");

                    #region Write ExtensionChangeContextCollection

                    if (_items != null)
                    {
                        foreach (ExtensionChangeContext extensionChangeContext in this._items)
                        {
                            xmlWriter.WriteRaw(extensionChangeContext.ToXml());
                        }
                    }

                    #endregion

                    //ExtensionChangeContextCollection node end
                    xmlWriter.WriteEndElement();
                    xmlWriter.Flush();

                    //Get the actual XML
                    returnXml = sw.ToString();
                }
            }

            return returnXml;
        }

        /// <summary>
        /// Delta Merge of extension change contexts
        /// </summary>
        /// <param name="deltaExtensionChangeContexts">Indicates extension change contexts needs to be merged</param>
        public void Merge(ExtensionChangeContextCollection deltaExtensionChangeContexts)
        {
            //Merge only if we have anything from delta.
            if (deltaExtensionChangeContexts != null && deltaExtensionChangeContexts.Count > 0)
            {
                ExtensionChangeContextCollection originalExtensionChangeContexts = this;

                //We don't have any into current collection. Take all data from delta and update to current collection.
                if (originalExtensionChangeContexts == null || originalExtensionChangeContexts.Count < 1)
                {
                    originalExtensionChangeContexts = deltaExtensionChangeContexts;
                }
                else
                {
                    foreach (ExtensionChangeContext deltaExtensionChangeContext in deltaExtensionChangeContexts)
                    {
                        Int32 containerId = deltaExtensionChangeContext.ContainerId;

                        ExtensionChangeContext originalExtensionChangeContext = this.Get(containerId);

                        //No previous entity change context found over here.. Add directly to current collection
                        if (originalExtensionChangeContext == null)
                        {
                            this._items.Add(deltaExtensionChangeContext);
                        }
                        else
                        {
                            //If meregedAction and previous action are not same then and then update to current collection.
                            //If same attribute is coming for multiple times for update then don't need to change the action.
                            originalExtensionChangeContext.VariantsChangeContext.Merge(deltaExtensionChangeContext.VariantsChangeContext);
                        }
                    }
                }
            }
        }

        #region Helper Methods

        /// <summary>
        /// Gets the attribute id list from entity extension change contexts
        /// </summary>
        /// <returns>Returns attribute id list</returns>
        public Collection<Int32> GetAttributeIdList()
        {
            Collection<Int32> attributeIdList = null;

            if (this._items!= null && this._items.Count > 0)
            {
                attributeIdList = new Collection<Int32>();

                foreach (ExtensionChangeContext extensionChangeContext in this._items)
                {
                    attributeIdList.AddRange<Int32>(extensionChangeContext.VariantsChangeContext.GetAttributeIdList());
                }
            }

            return attributeIdList;
        }

        /// <summary>
        /// Gets the attribute locale list from entity extension change contexts
        /// </summary>
        /// <returns>Returns attribute locale list</returns>
        public Collection<LocaleEnum> GetAttributeLocaleList()
        {
            Collection<LocaleEnum> attributeLocaleList = null;

            if (this._items != null && this._items.Count > 0)
            {
                attributeLocaleList = new Collection<LocaleEnum>();

                foreach (ExtensionChangeContext extensionChangeContext in this._items)
                {
                    attributeLocaleList.AddRange<LocaleEnum>(extensionChangeContext.VariantsChangeContext.GetAttributeLocaleList());
                }
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

            if (this._items != null && this._items.Count > 0)
            {
                attributeNameList = new Collection<String>();

                foreach (ExtensionChangeContext extensionChangeContext in this._items)
                {
                    attributeNameList.AddRange<String>(extensionChangeContext.VariantsChangeContext.GetAttributeNameList());
                }
            }

            return attributeNameList;
        }
        
        /// <summary>
        /// Gets the relationship type id list from extension change contexts
        /// </summary>
        /// <returns>Returns relationship type id list</returns>
        public Collection<Int32> GetRelationshipTypeIdList()
        {
            Collection<Int32> relationshipTypeIdList = null;

            if (this._items != null && this._items.Count > 0)
            {
                relationshipTypeIdList = new Collection<Int32>();

                foreach (ExtensionChangeContext extensionChangeContext in this._items)
                {
                    relationshipTypeIdList.AddRange<Int32>(extensionChangeContext.VariantsChangeContext.GetRelationshipTypeIdList());
                }
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

            if (this._items != null && this._items.Count > 0)
            {
                relationshipTypeNameList = new Collection<String>();

                foreach (ExtensionChangeContext extensionChangeContext in this._items)
                {
                    relationshipTypeNameList.AddRange<String>(extensionChangeContext.VariantsChangeContext.GetRelationshipTypeNameList());
                }
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

            if (this._items != null && this._items.Count > 0)
            {
                relationshipAttributeIdList = new Collection<Int32>();

                foreach (ExtensionChangeContext extensionChangeContext in this._items)
                {
                    relationshipAttributeIdList.AddRange<Int32>(extensionChangeContext.VariantsChangeContext.GetRelationshipAttributeIdList());
                }
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

            if (this._items != null && this._items.Count > 0)
            {
                relationshipAttributeLocaleList = new Collection<LocaleEnum>();

                foreach (ExtensionChangeContext extensionChangeContext in this._items)
                {
                    relationshipAttributeLocaleList.AddRange<LocaleEnum>(extensionChangeContext.VariantsChangeContext.GetRelationshipAttributeLocaleList());
                }
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

            if (this._items != null && this._items.Count > 0)
            {
                relationshipAttributeNameList = new Collection<String>();

                foreach (ExtensionChangeContext extensionChangeContext in this._items)
                {
                    relationshipAttributeNameList.AddRange<String>(extensionChangeContext.VariantsChangeContext.GetRelationshipAttributeNameList());
                }
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

            if (this._items != null && this._items.Count > 0)
            {
                foreach (ExtensionChangeContext extensionChangeContext in this._items)
                {
                    hasRelationshipsChanged = extensionChangeContext.VariantsChangeContext.HasRelationshipsChanged();

                    if (hasRelationshipsChanged)
                    {
                        break;
                    }
                }
            }

            return hasRelationshipsChanged;
        }

        /// <summary>
        /// Checks whether any entity change context action is Create
        /// </summary>
        /// <returns>Return true if object is having Create</returns>
        public Boolean HasEntitiesCreated()
        {
            Boolean hasEntitiesCreated = false;

            if (this._items != null && this._items.Count > 0)
            {
                foreach (ExtensionChangeContext extensionChangeContext in this._items)
                {
                    hasEntitiesCreated = extensionChangeContext.VariantsChangeContext.HasEntitiesCreated();

                    if (hasEntitiesCreated)
                    {
                        break;
                    }
                }
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

            if (this._items != null && this._items.Count > 0)
            {
                entityIdList = new Collection<Int64>();

                foreach (ExtensionChangeContext extensionChangeContext in this._items)
                {
                    entityIdList.AddRange<Int64>(extensionChangeContext.VariantsChangeContext.GetEntityIdList(objectActions));
                }
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
        private void LoadExtensionChangeContextCollection(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "ExtensionChangeContexts")
                        {
                            String extensionChangeContextXml = reader.ReadOuterXml();

                            if (!String.IsNullOrEmpty(extensionChangeContextXml))
                            {
                                ExtensionChangeContext extensionChangeContext = new ExtensionChangeContext(extensionChangeContextXml);

                                if (extensionChangeContext != null)
                                {
                                    this.Add(extensionChangeContext);
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

        /// <summary>
        /// Gets extension change context for a given container id
        /// </summary>
        /// <param name="containerId">Indicates container id</param>
        /// <returns>Returns extension change context based on container id</returns>
        private ExtensionChangeContext Get(Int32 containerId)
        {
            if (this._items.Count > 0)
            {
                foreach (ExtensionChangeContext extensionChangeContext in this._items)
                {
                    if (extensionChangeContext.ContainerId == containerId)
                    {
                        return extensionChangeContext;
                    }
                }
            }

            return null;
        }

        #endregion Private Methods

        #endregion Methods
    }
}