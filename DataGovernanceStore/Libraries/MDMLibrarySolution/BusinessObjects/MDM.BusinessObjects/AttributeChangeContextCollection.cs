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
    /// Specifies collection of change context of an attribute.
    /// </summary>
    [DataContract]
    [ProtoContract]
    public class AttributeChangeContextCollection : InterfaceContractCollection<IAttributeChangeContext, AttributeChangeContext>, IAttributeChangeContextCollection
    {
        #region Fields
        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the attribute change context Collection
        /// </summary>
        public AttributeChangeContextCollection()
        {
        }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        /// <param name="valuesAsXml">XML having xml values</param>
        public AttributeChangeContextCollection(String valuesAsXml)
        {
            LoadAttributeChangeContextCollection(valuesAsXml);
        }

        /// <summary>
        /// Converts list into current instance
        /// </summary>
        /// <param name="attributeChangeContextList">List of attribute change contexts</param>
        public AttributeChangeContextCollection(IList<AttributeChangeContext> attributeChangeContextList)
        {
            if (attributeChangeContextList != null)
            {
                this._items = new Collection<AttributeChangeContext>(attributeChangeContextList);
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
                    //AttributeChangeContextCollection node start
                    xmlWriter.WriteStartElement("AttributeChangeContexts");

                    #region Write AttributeChangeContextCollection

                    if (_items != null)
                    {
                        foreach (AttributeChangeContext attributeChangeContext in this._items)
                        {
                            xmlWriter.WriteRaw(attributeChangeContext.ToXml());
                        }
                    }

                    #endregion

                    //AttributeChangeContextCollection node end
                    xmlWriter.WriteEndElement();
                    xmlWriter.Flush();

                    //Get the actual XML
                    returnXml = sw.ToString();
                }
            }

            return returnXml;
        }

        /// <summary>
        /// Adds attributeinfo and object action of an attribute change context as input parameters
        /// </summary>
        /// <param name="attributeInfo">Indicates attributeinfo</param>
        /// <param name="objectAction">Indicates attribute action</param>
        public void Add(AttributeInfo attributeInfo, ObjectAction objectAction)
        {
            AttributeChangeContext filteredAttributeChangeContext = Get(objectAction);

            if (filteredAttributeChangeContext == null)
            {
                filteredAttributeChangeContext = new AttributeChangeContext(attributeInfo, objectAction);
                this.Add(filteredAttributeChangeContext);
            }
            else
            {
                if (!filteredAttributeChangeContext.AttributeInfoCollection.Contains(attributeInfo))
                {
                    filteredAttributeChangeContext.AttributeInfoCollection.Add(attributeInfo);
                }
            }
        }

        /// <summary>
        /// Gets attribute action for given attribute id
        /// </summary>
        /// <param name="attributeId">Indicates attribute id</param>
        /// <returns>Returns object action for given attribute id</returns>
        public ObjectAction Get(Int32 attributeId)
        {
            if (this._items.Count > 0)
            {
                foreach (AttributeChangeContext attributeChangeContext in this._items)
                {
                    if (attributeChangeContext.AttributeInfoCollection.Contains(attributeId))
                    {
                        return attributeChangeContext.Action;
                    }
                }
            }

            return ObjectAction.Unknown;
        }

        /// <summary>
        /// Removes attribute change context for a given attribute id
        /// </summary>
        /// <param name="attributeId">Indicates attribute id to be removed</param>
        /// <returns>'true' if operation is successful;otherwise 'false'.</returns>
        public Boolean Remove(Int32 attributeId)
        {
            Boolean result = false;

            if (this._items.Count > 0)
            {
                Collection<ObjectAction> toBeRemovedObjectAction = new Collection<ObjectAction>();

                foreach (AttributeChangeContext attributeChangeContext in this._items)
                {
                    if (attributeChangeContext.AttributeInfoCollection.Contains(attributeId))
                    {
                        AttributeInfo attributeInfo = attributeChangeContext.AttributeInfoCollection.GetByAttributeId(attributeId);

                        if (attributeInfo != null)
                        {
                            result = attributeChangeContext.AttributeInfoCollection.Remove(attributeInfo);
                        }

                        //No pending attribute list for an attribute change context. Remove them from list.
                        if (attributeChangeContext.AttributeInfoCollection.Count < 1)
                        {
                            toBeRemovedObjectAction.Add(attributeChangeContext.Action);
                        }
                    }
                }

                if (toBeRemovedObjectAction.Count > 0)
                {
                    foreach (ObjectAction objectAction in toBeRemovedObjectAction)
                    {
                        AttributeChangeContext toBeRemovedAttributeChangeContext = Get(objectAction);
                        this.Remove(toBeRemovedAttributeChangeContext);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Gets the attribute id list from attribute change contexts
        /// </summary>
        /// <returns>Returns attribute id list</returns>
        public Collection<Int32> GetAttributeIdList()
        {
            Collection<Int32> attributeIdList = null;

            if (this._items.Count > 0)
            {
                attributeIdList = new Collection<Int32>();

                foreach (AttributeChangeContext attributeChangeContext in this._items)
                {
                    foreach (AttributeInfo attributeInfo in attributeChangeContext.AttributeInfoCollection)
                    {
                        if (!attributeIdList.Contains(attributeInfo.Id))
                        {
                            attributeIdList.Add(attributeInfo.Id);
                        }
                    }
                }
            }

            return attributeIdList;
        }

        /// <summary>
        /// Gets the attribute ids from attribute change contexts based on given object action
        /// </summary>
        /// <param name="action">Indicates object action</param>
        /// <returns>Returns attribute id list</returns>
        public Collection<Int32> GetAttributeIdList(ObjectAction action)
        {
            Collection<Int32> attributeIdList = null;

            if (this._items.Count > 0)
            {
                attributeIdList = new Collection<Int32>();

                foreach (AttributeChangeContext attributeChangeContext in this._items)
                {
                    if (attributeChangeContext.Action == action)
                    {
                        foreach (AttributeInfo attributeInfo in attributeChangeContext.AttributeInfoCollection)
                        {
                            attributeIdList.Add(attributeInfo.Id);
                        }
                    }
                }
            }

            return attributeIdList;
        }

        /// <summary>
        /// Gets the attribute name list from attribute change contexts
        /// </summary>
        /// <returns>Returns attribute name list</returns>
        public Collection<String> GetAttributeNameList()
        {
            Collection<String> attributeNameList = null;

            if (this._items.Count > 0)
            {
                attributeNameList = new Collection<String>();

                foreach (AttributeChangeContext attributeChangeContext in this._items)
                {
                    foreach (AttributeInfo attributeInfo in attributeChangeContext.AttributeInfoCollection)
                    {
                        attributeNameList.Add(attributeInfo.Name);
                    }
                }
            }

            return attributeNameList;
        }

        /// <summary>
        /// Gets the attributegroup name list from attribute change contexts
        /// </summary>
        /// <returns>Returns attributegroup name list</returns>
        public Collection<String> GetAttributeParentNameList()
        {
            Collection<String> attributeGroupNameList = null;

            if (this._items.Count > 0)
            {
                attributeGroupNameList = new Collection<String>();

                foreach (AttributeChangeContext attributeChangeContext in this._items)
                {
                    foreach (AttributeInfo attributeInfo in attributeChangeContext.AttributeInfoCollection)
                    {
                        attributeGroupNameList.Add(attributeInfo.AttributeParentName);
                    }
                }
            }

            return attributeGroupNameList;
        }

        /// <summary>
        /// Checks whether current object has been changed i.e any object having Action as Create, Update or Delete 
        /// </summary>
        /// <returns>Return true if object is changed else false</returns>
        public Boolean HasAttributesChanged()
        {
            foreach (AttributeChangeContext attributeChangeContext in this._items)
            {
                return (attributeChangeContext.AttributeInfoCollection.Count > 0) ? true : false;
            }

            return false;
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valuesAsXml"></param>
        private void LoadAttributeChangeContextCollection(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "AttributeChangeContext")
                        {
                            String attributeChangeContextXml = reader.ReadOuterXml();

                            if (!String.IsNullOrEmpty(attributeChangeContextXml))
                            {
                                AttributeChangeContext attributeChangeContext = new AttributeChangeContext(attributeChangeContextXml);

                                if (attributeChangeContext != null)
                                {
                                    this.Add(attributeChangeContext);
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
        /// Gets attribute change context for a given object action
        /// </summary>
        /// <param name="objectAction">Indicates object action of an attribute</param>
        /// <returns>Returns attribute change context for a given action</returns>
        private AttributeChangeContext Get(ObjectAction objectAction)
        {
            if (this._items.Count > 0)
            {
                foreach (AttributeChangeContext attributeChangeContext in this._items)
                {
                    if (attributeChangeContext.Action == objectAction)
                    {
                        return attributeChangeContext;
                    }
                }
            }

            return null;
        }

        #endregion Private Methods

        #endregion Methods
    }
}