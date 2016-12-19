using System;
using System.IO;
using System.Xml;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using ProtoBuf;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Core.Extensions;
    using MDM.Interfaces;
    
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    [ProtoContract]
    public class CallDataContext : ObjectBase, ICallDataContext
    {
        #region Fields

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        [ProtoMember(1)]
        private Collection<Int32> _organizationIdList = new Collection<Int32>();

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        [ProtoMember(2)]
        private Collection<Int32> _containerIdList = new Collection<Int32>();

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        [ProtoMember(3)]
        private Collection<Int32> _entityTypeIdList = new Collection<Int32>();

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        [ProtoMember(4)]
        private Collection<Int32> _relationshipTypeIdList = new Collection<Int32>();

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        [ProtoMember(5)]
        private Collection<Int64> _categoryIdList = new Collection<Int64>();

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        [ProtoMember(6)]
        private Collection<Int32> _attributeIdList = new Collection<Int32>();

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        [ProtoMember(7)]
        private Collection<LocaleEnum> _localeList = new Collection<LocaleEnum>();

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        [ProtoMember(8)]
        private Collection<String> _lookupTableNameList = new Collection<String>();

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        [ProtoMember(9)]
        private Collection<Int64> _entityIdList = new Collection<Int64>();

        #endregion

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        public CallDataContext()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valuesAsXml"></param>
        public CallDataContext(String valuesAsXml)
        {
            LoadData(valuesAsXml);
        }

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public Collection<Int32> OrganizationIdList
        {
            get { return _organizationIdList; }
            set { _organizationIdList = value; }
        }


        /// <summary>
        /// 
        /// </summary>
        public Collection<Int32> ContainerIdList
        {
            get { return _containerIdList; }
            set { _containerIdList = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Collection<Int32> EntityTypeIdList
        {
            get { return _entityTypeIdList; }
            set { _entityTypeIdList = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Collection<Int32> RelationshipTypeIdList
        {
            get { return _relationshipTypeIdList; }
            set { _relationshipTypeIdList = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Collection<Int64> CategoryIdList
        {
            get { return _categoryIdList; }
            set { _categoryIdList = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Collection<Int32> AttributeIdList
        {
            get { return _attributeIdList; }
            set { _attributeIdList = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Collection<LocaleEnum> LocaleList
        {
            get { return _localeList; }
            set { _localeList = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Collection<String> LookupTableNameList
        {
            get { return _lookupTableNameList; }
            set { _lookupTableNameList = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Collection<Int64> EntityIdList
        {
            get { return _entityIdList; }
            set { _entityIdList = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public String ToXml()
        {
            String callDataContextXml = String.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //CallDataContext node start
            xmlWriter.WriteStartElement("CallDataContext");

            xmlWriter.WriteStartElement("OrganizationIdList");
            if(this.OrganizationIdList != null && this.OrganizationIdList.Count > 0)
                xmlWriter.WriteRaw(ValueTypeHelper.JoinCollection(this.OrganizationIdList, "|"));
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("ContainerIdList");
            if (this.ContainerIdList != null && this.ContainerIdList.Count > 0)
                xmlWriter.WriteRaw(ValueTypeHelper.JoinCollection(this.ContainerIdList, "|"));
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("EntityTypeIdList");
            if (this.EntityTypeIdList != null && this.EntityTypeIdList.Count > 0)
                xmlWriter.WriteRaw(ValueTypeHelper.JoinCollection(this.EntityTypeIdList, "|"));
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("RelationshipTypeIdList");
            if (this.RelationshipTypeIdList != null && this.RelationshipTypeIdList.Count > 0)
                xmlWriter.WriteRaw(ValueTypeHelper.JoinCollection(this.RelationshipTypeIdList, "|"));
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("CategoryIdList");
            if (this.CategoryIdList != null && this.CategoryIdList.Count > 0)
                xmlWriter.WriteRaw(ValueTypeHelper.JoinCollection(this.CategoryIdList, "|"));
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("AttributeIdList");
            if (this.AttributeIdList != null && this.AttributeIdList.Count > 0)
                xmlWriter.WriteRaw(ValueTypeHelper.JoinCollection(this.AttributeIdList, "|"));
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("LocaleList");
            if (this.LocaleList != null && this.LocaleList.Count > 0)
                xmlWriter.WriteRaw(ValueTypeHelper.JoinCollection(this.LocaleList, "|"));
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("LookupTableNameList");
            if (this.LookupTableNameList != null && this.LookupTableNameList.Count > 0)
                xmlWriter.WriteRaw(ValueTypeHelper.JoinCollection(this.LookupTableNameList, "#@#"));
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("EntityIdList");
            if (this.EntityIdList != null && this.EntityIdList.Count > 0)
                xmlWriter.WriteRaw(ValueTypeHelper.JoinCollection(this.EntityIdList, "|"));
            xmlWriter.WriteEndElement();

            //DataModelContext node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //Get the actual XML
            callDataContextXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return callDataContextXml;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public CallDataContext Clone()
        {
            CallDataContext clonedCallDataContext = new CallDataContext();

            clonedCallDataContext._organizationIdList = _organizationIdList.CopyCollection();
            clonedCallDataContext._containerIdList = _containerIdList.CopyCollection();
            clonedCallDataContext._entityTypeIdList = _entityTypeIdList.CopyCollection();
            clonedCallDataContext._categoryIdList = _categoryIdList.CopyCollection();
            clonedCallDataContext._relationshipTypeIdList = _relationshipTypeIdList.CopyCollection();
            clonedCallDataContext._attributeIdList = _attributeIdList.CopyCollection();
            clonedCallDataContext._localeList = _localeList.CopyCollection();
            clonedCallDataContext._lookupTableNameList = _lookupTableNameList.CloneCollection();
            clonedCallDataContext._entityIdList = _entityIdList.CopyCollection();

            return clonedCallDataContext;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valuesAsXml"></param>
        private void LoadData(String valuesAsXml)
        {
            #region Sample Xml

            #endregion Sample Xml

            XmlTextReader reader = null;
            try
            {
                reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                while (!reader.EOF)
                {
                    String data = String.Empty;

                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "OrganizationIdList")
                    {
                        data = reader.ReadElementContentAsString();
                        if(!String.IsNullOrWhiteSpace(data))
                            this.OrganizationIdList = ValueTypeHelper.SplitStringToIntCollection(data, '|');
                    }
                    else if (reader.NodeType == XmlNodeType.Element && reader.Name == "ContainerIdList")
                    {
                        data = reader.ReadElementContentAsString();
                        if (!String.IsNullOrWhiteSpace(data))
                            this.ContainerIdList = ValueTypeHelper.SplitStringToIntCollection(data, '|');
                    }
                    else if (reader.NodeType == XmlNodeType.Element && reader.Name == "EntityTypeIdList")
                    {
                        data = reader.ReadElementContentAsString();
                        if (!String.IsNullOrWhiteSpace(data))
                            this.EntityTypeIdList = ValueTypeHelper.SplitStringToIntCollection(data, '|');
                    }
                    else if (reader.NodeType == XmlNodeType.Element && reader.Name == "RelationshipTypeIdList")
                    {
                        data = reader.ReadElementContentAsString();
                        if (!String.IsNullOrWhiteSpace(data))
                            this.RelationshipTypeIdList = ValueTypeHelper.SplitStringToIntCollection(data, '|');
                    }
                    else if (reader.NodeType == XmlNodeType.Element && reader.Name == "CategoryIdList")
                    {
                        data = reader.ReadElementContentAsString();
                        if (!String.IsNullOrWhiteSpace(data))
                            this.CategoryIdList = ValueTypeHelper.SplitStringToLongCollection(data, '|');
                    }
                    else if (reader.NodeType == XmlNodeType.Element && reader.Name == "AttributeIdList")
                    {
                        data = reader.ReadElementContentAsString();
                        if (!String.IsNullOrWhiteSpace(data))                        
                            this.AttributeIdList = ValueTypeHelper.SplitStringToIntCollection(data, '|');
                    }
                    else if (reader.NodeType == XmlNodeType.Element && reader.Name == "LocaleList")
                    {
                        data = reader.ReadElementContentAsString();
                        if (!String.IsNullOrWhiteSpace(data))                        
                            this.LocaleList = ValueTypeHelper.SplitStringToEnumCollection<LocaleEnum>(data, '|');
                    }
                    else if (reader.NodeType == XmlNodeType.Element && reader.Name == "LookupTableNameList")
                    {
                        data = reader.ReadElementContentAsString();
                        if (!String.IsNullOrWhiteSpace(data))
                            this.LookupTableNameList = ValueTypeHelper.SplitStringToStringCollection(data, "#@#");
                    }
                    else if (reader.NodeType == XmlNodeType.Element && reader.Name == "EntityIdList")
                    {
                        data = reader.ReadElementContentAsString();
                        if (!String.IsNullOrWhiteSpace(data))
                            this.EntityIdList = ValueTypeHelper.SplitStringToLongCollection(data, '|');
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
    }
}
