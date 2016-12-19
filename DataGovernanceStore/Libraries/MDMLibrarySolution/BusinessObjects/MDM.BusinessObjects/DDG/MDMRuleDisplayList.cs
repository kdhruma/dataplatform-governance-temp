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
    using MDM.Interfaces;

    /// <summary>
    /// Represents the class that contain MDMRuleDisplayList information
    /// </summary>
    [DataContract]
    [ProtoContract(ImplicitFields = ImplicitFields.AllFields)]
    [KnownType(typeof(AttributeModelCollection))]
    [KnownType(typeof(RelationshipTypeCollection))]
    public class MDMRuleDisplayList : IMDMRuleDisplayList, ICloneable
    {
        #region Fields

        /// <summary>
        /// Field denoting the display type
        /// </summary>
        private DisplayType _displayType = DisplayType.Unknown;

        /// <summary>
        /// Field denoting the attribute models
        /// </summary>
        private AttributeModelCollection _attributeModelList = null;

        /// <summary>
        /// Field denoting the relationship types
        /// </summary>
        private RelationshipTypeCollection _relationshipTypeList = null;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Property denoting the display type
        /// </summary>
        [DataMember]
        public DisplayType DisplayType
        {
            get { return _displayType; }
            set { _displayType = value; }
        }

        /// <summary>
        /// Property denoting the attribute models
        /// </summary>
        [DataMember]
        public AttributeModelCollection AttributeModelList
        {
            get
            {
                if (_attributeModelList == null)
                {
                    _attributeModelList = new AttributeModelCollection();
                }
                return _attributeModelList;
            }
            set { _attributeModelList = value; }
        }

        /// <summary>
        /// Property denoting the relationship types
        /// </summary>
        [DataMember]
        public RelationshipTypeCollection RelationshipTypeList
        {
            get
            {
                if (_relationshipTypeList == null)
                {
                    _relationshipTypeList = new RelationshipTypeCollection();
                }

                return _relationshipTypeList;
            }
            set { _relationshipTypeList = value; }
        }

        #endregion Properties

        #region Constructor

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public MDMRuleDisplayList()
        {

        }

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        public MDMRuleDisplayList(DisplayType displayType)
        {
            this._displayType = displayType;
        }

        /// <summary>
        /// Parameterized Constructor which generates Object From Xml
        /// </summary>
        public MDMRuleDisplayList(String valuesAsXml)
        {
            LoadDisplayListFromXml(valuesAsXml);
        }

        #endregion Constructor

        #region Methods

        #region Public Methods

        /// <summary>
        /// Sets the Display List Based on Ids
        /// </summary>
        /// <param name="objectIds">Indicates the Ids of Objects</param>
        public void SetDisplayListByIds(ICollection<Int32> objectIds)
        {
            if (objectIds != null && objectIds.Count > 0)
            {
                switch (this._displayType)
                {
                    case DisplayType.AttributeList:
                        foreach (Int32 attributeId in objectIds)
                        {
                            this.AttributeModelList.Add(new AttributeModel(attributeId, String.Empty, String.Empty), true);
                        }
                        break;
                    case DisplayType.RelationshipType:
                        foreach (Int32 relationshipTypeId in objectIds)
                        {
                            this.RelationshipTypeList.Add(new RelationshipType(relationshipTypeId, String.Empty, String.Empty));
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// Gets the Ids of Objects which are stored in Display List
        /// </summary>
        /// <returns>Ids of Objects</returns>
        public ICollection<Int32> GetDisplayListIds()
        {
            List<Int32> objectIds = new List<Int32>();

            switch (this._displayType)
            {
                case DisplayType.AttributeList:
                    if (this._attributeModelList != null && this._attributeModelList.Count > 0)
                    {
                        objectIds.AddRange(this._attributeModelList.GetAttributeIdList());
                    }
                    break;
                case DisplayType.RelationshipType:
                    if (this._relationshipTypeList != null && this._relationshipTypeList.Count > 0)
                    {
                        objectIds.AddRange(this._relationshipTypeList.GetRelationshipTypeIds());
                    }
                    break;
            }

            return new Collection<Int32>(objectIds);
        }

        /// <summary>
        /// Sets the Display List Based on Names
        /// </summary>
        /// <param name="objectNames">Indicates the names of Objects</param>
        public void SetDisplayListByNames(ICollection<String> objectNames)
        {
            if (objectNames != null && objectNames.Count > 0)
            {
                switch (this._displayType)
                {
                    case DisplayType.AttributeList:
                        foreach (String attributeName in objectNames)
                        {
                            this.AttributeModelList.Add(new AttributeModel(-1, attributeName, String.Empty), true);
                        }
                        break;
                    case DisplayType.RelationshipType:
                        foreach (String relationshipTypeName in objectNames)
                        {
                            this.RelationshipTypeList.Add(new RelationshipType(-1, relationshipTypeName, String.Empty));
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// Gets the Names of Objects which are stored in Display List
        /// </summary>
        /// <returns>Names of Objects</returns>
        public ICollection<String> GetDisplayListNames()
        {
            List<String> objectNames = new List<String>();

            switch (this._displayType)
            {
                case DisplayType.AttributeList:
                    if (this._attributeModelList != null && this._attributeModelList.Count > 0)
                    {
                        foreach (AttributeModel attributeModel in this._attributeModelList)
                        {
                            objectNames.Add(attributeModel.Name);
                        }
                    }
                    break;
                case DisplayType.RelationshipType:
                    if (this._relationshipTypeList != null && this._relationshipTypeList.Count > 0)
                    {
                        foreach (RelationshipType relationshipType in this._relationshipTypeList)
                        {
                            objectNames.Add(relationshipType.Name);
                        }
                    }
                    break;
            }

            return new Collection<String>(objectNames);
        }

        /// <summary>
        /// Gets a cloned instance of the current MDMRuleDisplayList object
        /// </summary>
        /// <returns>Cloned instance of the current MDMRuleDisplayList object</returns>
        public MDMRuleDisplayList Clone()
        {
            MDMRuleDisplayList clonedDisplayList = new MDMRuleDisplayList();

            clonedDisplayList.DisplayType = this.DisplayType;
            clonedDisplayList.AttributeModelList = this.AttributeModelList;
            clonedDisplayList.RelationshipTypeList = this.RelationshipTypeList;

            return clonedDisplayList;
        }

        /// <summary>
        /// Gets Xml representation of MDMRuleDisplayList Object
        /// </summary>
        /// <returns>MDMRuleDisplayList Object as Xml</returns>
        public String ToXml()
        {
            String outputXml = String.Empty;

            using (StringWriter sw = new StringWriter())
            {
                using (XmlTextWriter xmlWriter = new XmlTextWriter(sw))
                {
                    // DisplayList node start
                    xmlWriter.WriteStartElement("DisplayList");

                    xmlWriter.WriteAttributeString("DisplayType", DisplayType.ToString());

                    if (this._attributeModelList != null)
                    {
                        xmlWriter.WriteRaw(this._attributeModelList.ToXml(ObjectSerialization.ProcessingOnly));
                    }
                    if (this._relationshipTypeList != null)
                    {
                        xmlWriter.WriteRaw(this._relationshipTypeList.ToXml(ObjectSerialization.ProcessingOnly));
                    }

                    // DisplayList node end
                    xmlWriter.WriteEndElement();
                }

                //Get the output XML
                outputXml = sw.ToString();
            }

            return outputXml;
        }

        #endregion Public Methods

        #region Overrides Methods

        /// <summary>
        /// Determines whether specified object is equal to the current object
        /// </summary>
        /// <param name="obj">MDMRuleDisplayList object which needs to be compared</param>
        /// <returns>Result of the comparison in Boolean</returns>
        public override Boolean Equals(Object obj)
        {
            if (obj is MDMRuleDisplayList)
            {
                MDMRuleDisplayList objectToBeCompared = obj as MDMRuleDisplayList;

                if (this.DisplayType != objectToBeCompared.DisplayType)
                {
                    return false;
                }

                if (this._attributeModelList.Count != objectToBeCompared.AttributeModelList.Count)
                {
                    return false;
                }

                foreach (AttributeModel attributeModel in this._attributeModelList)
                {
                    AttributeModel attributeModelToCompare = null;

                    if (attributeModel.Id > 0)
                    {
                        attributeModelToCompare = objectToBeCompared.AttributeModelList.GetAttributeModel(attributeModel.Id, attributeModel.Locale) as AttributeModel;
                    }
                    else
                    {
                        attributeModelToCompare = objectToBeCompared.AttributeModelList.GetAttributeModel(new AttributeUniqueIdentifier(attributeModel.Name, String.Empty), attributeModel.Locale);
                    }

                    if (attributeModelToCompare == null)
                    {
                        return false;
                    }
                }

                if (this.RelationshipTypeList.Count != objectToBeCompared.RelationshipTypeList.Count)
                {
                    return false;
                }

                foreach (RelationshipType relationshipType in this.RelationshipTypeList)
                {
                    RelationshipType relationshipTypeToCompare = null;

                    if (relationshipType.Id > 0)
                    {
                        relationshipTypeToCompare = objectToBeCompared.RelationshipTypeList.Get(relationshipType.Id);
                    }
                    else
                    {
                        relationshipTypeToCompare = objectToBeCompared.RelationshipTypeList.Get(relationshipType.Name);
                    }

                    if (relationshipTypeToCompare == null)
                    {
                        return false;
                    }
                }

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
            int hashCode = 0;

            hashCode = base.GetHashCode() ^ DisplayType.GetHashCode();

            if (this._attributeModelList != null && this._attributeModelList.Count > 0)
            {
                foreach (AttributeModel attributeModel in this._attributeModelList)
                {
                    hashCode = hashCode ^ attributeModel.GetHashCode();
                }
            }

            if (this._relationshipTypeList != null && this._relationshipTypeList.Count > 0)
            {
                foreach (RelationshipType relationshipType in this._relationshipTypeList)
                {
                    hashCode = hashCode ^ relationshipType.GetHashCode();
                }
            }

            return hashCode;
        }

        #endregion Overrides Methods

        #region IClonable Members

        /// <summary>
        /// Gets a cloned instance of the current MDMRuleDisplayList object
        /// </summary>
        /// <returns>Cloned Object</returns>
        Object ICloneable.Clone()
        {
            return this.Clone();
        }

        #endregion IClonable Members

        #region Private Methods

        private void LoadDisplayListFromXml(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                using (XmlTextReader reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null))
                {
                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "DisplayList")
                        {
                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("DisplayType"))
                                {
                                    ValueTypeHelper.EnumTryParse<DisplayType>(reader.ReadContentAsString(), true, out this._displayType);
                                }

                                reader.Read();
                            }
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "AttributeModels")
                        {
                            String attributeModelsList = reader.ReadOuterXml();

                            if (attributeModelsList != null)
                            {
                                this._attributeModelList = new AttributeModelCollection(attributeModelsList);
                            }

                            reader.Read();
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "RelationshipTypes")
                        {
                            String relationshipTypes = reader.ReadOuterXml();

                            if (relationshipTypes != null)
                            {
                                this._relationshipTypeList = new RelationshipTypeCollection(relationshipTypes);
                            }

                            reader.Read();
                        }
                        else
                        {
                            //Keep on reading the xml until we reach expected node.
                            reader.Read();
                        }
                    }
                }
            }
        }

        #endregion Private Methods

        #endregion Methods
    }
}
