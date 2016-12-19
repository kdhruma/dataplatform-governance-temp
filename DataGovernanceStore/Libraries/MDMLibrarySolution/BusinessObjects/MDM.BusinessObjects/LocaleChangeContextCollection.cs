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
    /// Specifies collection of change context of a locale.
    /// </summary>
    [DataContract]
    [ProtoContract]
    public class LocaleChangeContextCollection : InterfaceContractCollection<ILocaleChangeContext, LocaleChangeContext>, ILocaleChangeContextCollection
    {
        #region Fields
        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the attribute change context Collection
        /// </summary>
        public LocaleChangeContextCollection()
        {
        }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        /// <param name="valuesAsXml">XML having xml values</param>
        public LocaleChangeContextCollection(String valuesAsXml)
        {
            LoadLocaleChangeContextCollection(valuesAsXml);
        }

        /// <summary>
        /// Converts list into current instance
        /// </summary>
        /// <param name="localeChangeContextList">List of attribute change contexts</param>
        public LocaleChangeContextCollection(IList<LocaleChangeContext> localeChangeContextList)
        {
            if (localeChangeContextList != null)
            {
                this._items = new Collection<LocaleChangeContext>(localeChangeContextList);
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
                    //LocaleChangeContextCollection node start
                    xmlWriter.WriteStartElement("LocaleChangeContexts");

                    #region Write LocaleChangeContextCollection

                    if (_items != null)
                    {
                        foreach (LocaleChangeContext localeChangeContext in this._items)
                        {
                            xmlWriter.WriteRaw(localeChangeContext.ToXml());
                        }
                    }

                    #endregion

                    //LocaleChangeContextCollection node end
                    xmlWriter.WriteEndElement();
                    xmlWriter.Flush();

                    //Get the actual XML
                    returnXml = sw.ToString();
                }
            }

            return returnXml;
        }

        /// <summary>
        /// Gets LocaleChangeContext based on dataLocale
        /// </summary>
        /// <param name="dataLocale">Indicates data locale for which locale change context to be retrieved</param>
        /// <returns>LocaleChangeContext based on given dataLocale</returns>
        public LocaleChangeContext Get(LocaleEnum dataLocale)
        {
            if (this._items != null && this._items.Count > 0)
            {
                foreach (LocaleChangeContext localeChangeContext in this._items)
                {
                    if (localeChangeContext.DataLocale == dataLocale)
                    {
                        return localeChangeContext;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Delta Merge of locale change contexts
        /// </summary>
        /// <param name="deltaLocaleChangeContexts">Indicates delta locale change contexts needs to be merged</param>
        public void Merge(LocaleChangeContextCollection deltaLocaleChangeContexts)
        {
            //Merge only if we have anything from delta.
            if (deltaLocaleChangeContexts != null && deltaLocaleChangeContexts.Count > 0)
            {
                //We don't have any into current collection. Take all data from delta and update to current collection.
                if (this == null || this.Count < 1)
                {
                    this._items = deltaLocaleChangeContexts._items;
                }
                else
                {
                    foreach (LocaleChangeContext deltaLocaleChangeContext in deltaLocaleChangeContexts)
                    {
                        LocaleEnum dataLocale = deltaLocaleChangeContext.DataLocale;

                        LocaleChangeContext originalLocaleChangeContext = this.Get(dataLocale);

                        //No previous locale change context found over here.. Add directly to current collection
                        if (originalLocaleChangeContext == null)
                        {
                            this._items.Add(deltaLocaleChangeContext);
                        }
                        else
                        {
                            Merge(originalLocaleChangeContext, deltaLocaleChangeContext);
                        }
                    }
                }
            }
        }

        #region Get Methods

        /// <summary>
        /// Gets the attribute id list from locale change contexts
        /// </summary>
        /// <returns>Returns attribute id list</returns>
        public Collection<Int32> GetAttributeIdList()
        {
            Collection<Int32> attributeIdList = null;

            if (this._items.Count > 0)
            {
                attributeIdList = new Collection<Int32>();

                foreach (LocaleChangeContext localeChangeContext in this._items)
                {
                    attributeIdList.AddRange<Int32>(localeChangeContext.AttributeChangeContexts.GetAttributeIdList());
                }
            }

            return attributeIdList;
        }

        /// <summary>
        /// Gets the attribute ids from locale change contexts based on given object action
        /// </summary>
        /// <param name="action">Indicates object action</param>
        /// <returns>Returns attribute id list</returns>
        public Collection<Int32> GetAttributeIdList(ObjectAction action)
        {
            Collection<Int32> attributeIdList = null;

            if (this._items.Count > 0)
            {
                attributeIdList = new Collection<Int32>();

                foreach (LocaleChangeContext localeChangeContext in this._items)
                {
                    attributeIdList.AddRange<Int32>(localeChangeContext.AttributeChangeContexts.GetAttributeIdList(action));
                }
            }

            return attributeIdList;
        }

        /// <summary>
        /// Gets the attribute name list from locale change contexts
        /// </summary>
        /// <returns>Returns attribute name list</returns>
        public Collection<String> GetAttributeNameList()
        {
            Collection<String> attributeNameList = null;

            if (this._items.Count > 0)
            {
                attributeNameList = new Collection<String>();

                foreach (LocaleChangeContext localeChangeContext in this._items)
                {
                    attributeNameList.AddRange<String>(localeChangeContext.AttributeChangeContexts.GetAttributeNameList());
                }
            }

            return attributeNameList;
        }

        /// <summary>
        /// Gets the attribute locale list from locale change contexts
        /// </summary>
        /// <returns>Returns attribute locale list</returns>
        public Collection<LocaleEnum> GetAttributeLocaleList()
        {
            Collection<LocaleEnum> attributeLocaleList = null;

            if (this._items.Count > 0)
            {
                attributeLocaleList = new Collection<LocaleEnum>();

                foreach (LocaleChangeContext localeChangeContext in this._items)
                {
                    attributeLocaleList.Add(localeChangeContext.DataLocale);
                }
            }

            return attributeLocaleList;
        }

        /// <summary>
        /// Gets the relationship id list from locale change contexts
        /// </summary>
        /// <returns>Returns relationship id list</returns>
        public Collection<Int64> GetRelationshipIdList()
        {
            Collection<Int64> relationshipIdList = null;

            if (this._items.Count > 0)
            {
                relationshipIdList = new Collection<Int64>();

                foreach (LocaleChangeContext localeChangeContext in this._items)
                {
                    foreach (RelationshipChangeContext relationshipChangeContext in localeChangeContext.RelationshipChangeContexts)
                    {
                        relationshipIdList.Add(relationshipChangeContext.RelationshipId);
                    }
                }
            }

            return relationshipIdList;
        }

        /// <summary>
        /// Gets the relationship type id list from locale change contexts
        /// </summary>
        /// <returns>Returns relationship type id list</returns>
        public Collection<Int32> GetRelationshipTypeIdList()
        {
            Collection<Int32> relationshipTypeIdList = null;

            if (this._items.Count > 0)
            {
                relationshipTypeIdList = new Collection<Int32>();

                foreach (LocaleChangeContext localeChangeContext in this._items)
                {
                    foreach (RelationshipChangeContext relationshipChangeContext in localeChangeContext.RelationshipChangeContexts)
                    {
                        relationshipTypeIdList.Add(relationshipChangeContext.RelationshipTypeId);
                    }
                }
            }

            return relationshipTypeIdList;
        }

        /// <summary>
        /// Gets the relationship type name list from locale change contexts
        /// </summary>
        /// <returns>Returns relationship type namelist</returns>
        public Collection<String> GetRelationshipTypeNameList()
        {
            Collection<String> relationshipTypeNameList = null;

            if (this._items.Count > 0)
            {
                relationshipTypeNameList = new Collection<String>();

                foreach (LocaleChangeContext localeChangeContext in this._items)
                {
                    foreach (RelationshipChangeContext relationshipChangeContext in localeChangeContext.RelationshipChangeContexts)
                    {
                        relationshipTypeNameList.Add(relationshipChangeContext.RelationshipTypeName);
                    }
                }
            }

            return relationshipTypeNameList;
        }

        /// <summary>
        /// Gets the relationship attribute id list from locale change contexts
        /// </summary>
        /// <returns>Returns relationship attribute id list</returns>
        public Collection<Int32> GetRelationshipAttributeIdList()
        {
            Collection<Int32> relationshipAttributeIdList = null;

            if (this._items.Count > 0)
            {
                relationshipAttributeIdList = new Collection<Int32>();

                foreach (LocaleChangeContext localeChangeContext in this._items)
                {
                    foreach (RelationshipChangeContext relationshipChangeContext in localeChangeContext.RelationshipChangeContexts)
                    {
                        relationshipAttributeIdList.AddRange<Int32>(relationshipChangeContext.AttributeChangeContexts.GetAttributeIdList());
                    }
                }
            }

            return relationshipAttributeIdList;
        }

        /// <summary>
        /// Gets the relationship attribute name list from locale change contexts
        /// </summary>
        /// <returns>Returns relationship attribute name list</returns>
        public Collection<String> GetRelationshipAttributeNameList()
        {
            Collection<String> relationshipAttributeNameList = null;

            if (this._items.Count > 0)
            {
                relationshipAttributeNameList = new Collection<String>();

                foreach (LocaleChangeContext localeChangeContext in this._items)
                {
                    foreach (RelationshipChangeContext relationshipChangeContext in localeChangeContext.RelationshipChangeContexts)
                    {
                        relationshipAttributeNameList.AddRange<String>(relationshipChangeContext.AttributeChangeContexts.GetAttributeNameList());
                    }
                }
            }

            return relationshipAttributeNameList;
        }

        /// <summary>
        /// Gets the relationship attribute locale list from locale change contexts
        /// </summary>
        /// <returns>Returns relationship attribute locale list</returns>
        public Collection<LocaleEnum> GetRelationshipAttributeLocaleList()
        {
            return GetAttributeLocaleList();
        }

        #endregion Get Methods

        #region Has Changed Methods

        /// <summary>
        /// Checks whether atttribute object has been changed i.e any object having Action as Create, Update or Delete 
        /// </summary>
        /// <returns>Return true if object is changed else false</returns>
        public Boolean HasAttributesChanged()
        {
            Boolean hasAttributesChanged = false;

            if (this._items.Count > 0)
            {
                foreach (LocaleChangeContext localeChangeContext in this._items)
                {
                    hasAttributesChanged = localeChangeContext.AttributeChangeContexts.HasAttributesChanged();

                    if (hasAttributesChanged)
                    {
                        break;
                    }
                }
            }

            return hasAttributesChanged;
        }

        /// <summary>
        /// Checks whether relationship attributes object has been changed i.e any object having Action as Create, Update or Delete 
        /// </summary>
        /// <returns>Return true if object is changed else false</returns>
        public Boolean HasRelationshipAttributesChanged()
        {
            Boolean hasRelationshipAttributesChanged = false;

            if (this._items.Count > 0)
            {
                foreach (LocaleChangeContext localeChangeContext in this._items)
                {
                    foreach (RelationshipChangeContext relationshipChangeContext in localeChangeContext.RelationshipChangeContexts)
                    {
                        hasRelationshipAttributesChanged = relationshipChangeContext.AttributeChangeContexts.HasAttributesChanged();

                        if (hasRelationshipAttributesChanged)
                        {
                            break;
                        }
                    }
                }
            }

            return hasRelationshipAttributesChanged;
        }

        /// <summary>
        /// Checks whether relationship object has been changed i.e any object having Action as Create, Update or Delete 
        /// </summary>
        /// <returns>Return true if object is changed else false</returns>
        public Boolean HasRelationshipsChanged()
        {
            Boolean hasRelationshipsChanged = false;

            if (this._items.Count > 0)
            {
                foreach (LocaleChangeContext localeChangeContext in this._items)
                {
                    hasRelationshipsChanged = (localeChangeContext.RelationshipChangeContexts.Count > 0) ? true : false;

                    if (hasRelationshipsChanged)
                    {
                        break;
                    }
                }
            }

            return hasRelationshipsChanged;
        }

        /// <summary>
        /// Checks whether relationship object has been changed i.e any object having Action as Create
        /// </summary>
        /// <returns>Return true if object is changed else false</returns>
        public Boolean IsRelationshipsCreated()
        {
            return HasRelationshipsChangedForGivenAction(ObjectAction.Create);
        }

        /// <summary>
        /// Checks whether relationship object has been changed i.e any object having Action as Update
        /// </summary>
        /// <returns>Return true if object is changed else false</returns>
        public Boolean IsRelationshipsUpdated()
        {
            return HasRelationshipsChangedForGivenAction(ObjectAction.Update);
        }

        /// <summary>
        /// Checks whether relationship object has been changed i.e any object having Action as Delete 
        /// </summary>
        /// <returns>Return true if object is changed else false</returns>
        public Boolean IsRelationshipsDeleted()
        {
            return HasRelationshipsChangedForGivenAction(ObjectAction.Delete);
        }

        #endregion Has Changed Methods

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valuesAsXml"></param>
        private void LoadLocaleChangeContextCollection(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "LocaleChangeContext")
                        {
                            String localeChangeContextXml = reader.ReadOuterXml();

                            if (!String.IsNullOrEmpty(localeChangeContextXml))
                            {
                                LocaleChangeContext localeChangeContext = new LocaleChangeContext(localeChangeContextXml);

                                if (localeChangeContext != null)
                                {
                                    this.Add(localeChangeContext);
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
        /// Delta Merge of locale change context
        /// </summary>
        /// <param name="originalLocaleChangeContext">Indicates original locale change context</param>
        /// <param name="deltaLocaleChangeContext">Indicates delta locale change context needs to be merged</param>
        private void Merge(LocaleChangeContext originalLocaleChangeContext, LocaleChangeContext deltaLocaleChangeContext)
        {
            #region Merge Attribute Change Context

            MergeAttributeChangeContexts(originalLocaleChangeContext.AttributeChangeContexts, deltaLocaleChangeContext.AttributeChangeContexts);

            #endregion Merge Attribute Change Context

            #region Merge Relationship Change Context

            MergeRelationshipChangeContexts(originalLocaleChangeContext.RelationshipChangeContexts, deltaLocaleChangeContext.RelationshipChangeContexts);

            #endregion Merge Relationship Change Context
        }

        /// <summary>
        /// Delta Merge of attribute change contexts
        /// </summary>
        /// <param name="originalAttributeChangeContexts">Indicates original attribute change context</param>
        /// <param name="deltaAttributeChangeContexts">Indicates delta attribute change context needs to be merged</param>
        private void MergeAttributeChangeContexts(AttributeChangeContextCollection originalAttributeChangeContexts, AttributeChangeContextCollection deltaAttributeChangeContexts)
        {
            //Merge only if we have anything from delta.
            if (deltaAttributeChangeContexts != null && deltaAttributeChangeContexts.Count > 0)
            {
                //We don't have any into current collection. Take all data from delta and update to current collection.
                if (originalAttributeChangeContexts == null || originalAttributeChangeContexts.Count < 1)
                {
                    originalAttributeChangeContexts.AddRange(deltaAttributeChangeContexts, false);
                }
                else
                {
                    foreach (AttributeChangeContext deltaAttributeChangeContext in deltaAttributeChangeContexts)
                    {
                        ObjectAction currentAction = deltaAttributeChangeContext.Action;

                        foreach (AttributeInfo attributeInfo in deltaAttributeChangeContext.AttributeInfoCollection)
                        {
                            AttributeInfo originalAttributeInfo = attributeInfo.Clone();
                            ObjectAction originalAction = originalAttributeChangeContexts.Get(attributeInfo.Id);

                            //If original action comes as Unknown..it means delta attribute is not available in original attribute change contexts.
                            if (originalAction == ObjectAction.Unknown)
                            {
                                originalAttributeChangeContexts.Add(originalAttributeInfo, currentAction);
                            }
                            else
                            {
                                ObjectAction mergedAction = EntityFamilyChangeContext.GetMergedAction(originalAction, currentAction);

                                //If meregedAction and previous action are not same then and then update to current collection.
                                //If same attribute is coming for multiple times for update then don't need to change the action.
                                if (mergedAction != originalAction)
                                {
                                    originalAttributeChangeContexts.Remove(attributeInfo.Id);
                                    originalAttributeChangeContexts.Add(originalAttributeInfo, currentAction);
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Delta Merge of relationship change contexts
        /// </summary>
        /// <param name="originalRelationshipChangeContexts">Indicates original relationship change context</param>
        /// <param name="deltaRelationshipChangeContexts">Indicates delta relationship change context needs to be merged</param>
        private void MergeRelationshipChangeContexts(RelationshipChangeContextCollection originalRelationshipChangeContexts, RelationshipChangeContextCollection deltaRelationshipChangeContexts)
        {
            //Merge only if we have anything from delta.
            if (deltaRelationshipChangeContexts != null && deltaRelationshipChangeContexts.Count > 0)
            {
                //We don't have any into current collection. Take all data from delta and update to current collection.
                if (originalRelationshipChangeContexts == null || originalRelationshipChangeContexts.Count < 1)
                {
                    originalRelationshipChangeContexts.AddRange(deltaRelationshipChangeContexts, false);
                }
                else
                {
                    foreach (RelationshipChangeContext deltaRelationshipChangeContext in deltaRelationshipChangeContexts)
                    {
                        Int64 relationshipId = deltaRelationshipChangeContext.RelationshipId;
                        ObjectAction currentAction = deltaRelationshipChangeContext.Action;

                        RelationshipChangeContext originalRelationshipChangeContext = originalRelationshipChangeContexts.Get(relationshipId);

                        //No previous relationship change context found over here.. Add directly to current collection
                        if (originalRelationshipChangeContext == null)
                        {
                            originalRelationshipChangeContexts.Add(deltaRelationshipChangeContext);
                        }
                        else
                        {
                            ObjectAction originalAction = originalRelationshipChangeContext.Action;
                            ObjectAction mergedAction = EntityFamilyChangeContext.GetMergedAction(originalAction, currentAction);

                            //If meregedAction and previous action are not same then and then update to current collection.
                            //If same attribute is coming for multiple times for update then don't need to change the action.
                            if (mergedAction != originalAction)
                            {
                                originalRelationshipChangeContext.Action = mergedAction;

                                MergeAttributeChangeContexts(originalRelationshipChangeContext.AttributeChangeContexts, deltaRelationshipChangeContext.AttributeChangeContexts);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Checks whether relationship object has been changed for given object action
        /// </summary>
        /// <param name="objectAction">Indicates object action</param>
        private Boolean HasRelationshipsChangedForGivenAction(ObjectAction objectAction)
        {
            if (this._items.Count > 0)
            {
                foreach (LocaleChangeContext localeChangeContext in this._items)
                {
                    foreach (RelationshipChangeContext relationshipChangeContext in localeChangeContext.RelationshipChangeContexts)
                    {
                        if (relationshipChangeContext.Action == objectAction)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        #endregion Private Methods

        #endregion Methods
    }
}