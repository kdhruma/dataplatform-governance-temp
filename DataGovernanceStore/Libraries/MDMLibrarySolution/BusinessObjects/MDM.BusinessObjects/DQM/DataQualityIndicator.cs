using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects.DQM
{
    using MDM.Core;
    using MDM.Core.Extensions;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies DataQualityIndicator
    /// </summary>
    [DataContract]
    public class DataQualityIndicator : MDMObject, IDataQualityIndicator
    {
        #region Fields

        /// <summary>
        /// Field for DataQualityIndicator Id
        /// </summary>        
        private Int16 _id = -1;

        /// <summary>
        /// Field for TableColumnName of DataQualityIndicatorValues Table
        /// </summary>
        private String _dataQualityIndicatorValuesTableColumnName = null;

        /// <summary>
        /// Field for StateViewAttributeId
        /// </summary>
        private Int32? _stateViewAttributeId = null;

        /// <summary>
        /// Field for Weight
        /// </summary>
        private Byte? _weight = null;

        /// <summary>
        /// Field for IsCritical
        /// </summary>
        private Boolean? _isCritical = null;

        /// <summary>
        /// Field for validation container ids
        /// </summary>
        private Collection<Int32> _containerIds = new Collection<Int32>();

        /// <summary>
        /// Field for validation category ids
        /// </summary>
        private Collection<Int64> _categoryIds = new Collection<Int64>();

        /// <summary>
        /// Field for validation entity type ids
        /// </summary>
        private Collection<Int32> _entityTypeIds = new Collection<Int32>();

        /// <summary>
        /// Field for DataQualityIndicator Description
        /// </summary>
        private String _description = String.Empty;

        #endregion

        #region Properties

        /// <summary>
        /// Property denoting DataQualityIndicator Id
        /// </summary>        
        [DataMember]
        public new Int16 Id
        {
            get { return _id; }
            set { _id = value; }
        }
        
        /// <summary>
        /// Property denoting TableColumnName of DataQualityIndicatorValues Table
        /// </summary>
        [DataMember]
        public String DataQualityIndicatorValuesTableColumnName
        {
            get { return _dataQualityIndicatorValuesTableColumnName; }
            set { _dataQualityIndicatorValuesTableColumnName = value; }
        }

        /// <summary>
        /// Property denoting StateViewAttributeId
        /// </summary>
        [DataMember]
        public Int32? StateViewAttributeId
        {
            get { return _stateViewAttributeId; }
            set { _stateViewAttributeId = value; }
        }

        /// <summary>
        /// Property denoting Weight
        /// </summary>
        [DataMember]
        public Byte? Weight
        {
            get { return _weight; }
            set { _weight = value; }
        }

        /// <summary>
        /// Property denoting IsCritical
        /// </summary>
        [DataMember]
        public Boolean? IsCritical
        {
            get { return _isCritical; }
            set { _isCritical = value; }
        }

        /// <summary>
        /// Selected for validation container ids
        /// </summary>
        [DataMember]
        public Collection<Int32> ContainerIds
        {
            get { return _containerIds; }
            set { _containerIds = value; }
        }

        /// <summary>
        /// Selected for validation category ids
        /// </summary>
        [DataMember]
        public Collection<Int64> CategoryIds
        {
            get { return _categoryIds; }
            set { _categoryIds = value; }
        }

        /// <summary>
        /// Selected for validation entity type ids
        /// </summary>
        [DataMember]
        public Collection<Int32> EntityTypeIds
        {
            get { return _entityTypeIds; }
            set { _entityTypeIds = value; }
        }

        /// <summary>
        /// Specifies DataQualityIndicator description
        /// </summary>
        [DataMember]
        public String Description
        {
            get { return _description; }
            set { _description = value; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Makes Deep copy of DataQualityIndicator
        /// </summary>        
        public object Clone()
        {
            DataQualityIndicator dataQualityIndicator = (DataQualityIndicator)this.MemberwiseClone();

            dataQualityIndicator.CategoryIds = CategoryIds;
            dataQualityIndicator.ContainerIds = ContainerIds;
            dataQualityIndicator.EntityTypeIds = EntityTypeIds;            

            return dataQualityIndicator;
        }

        /// <summary>
        /// Determines whether two Object instances are equal
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false</returns>
        public override Boolean Equals(object obj)
        {
            if (base.Equals(obj))
            {
                if (obj is DataQualityIndicator)
                {
                    DataQualityIndicator objectToBeCompared = obj as DataQualityIndicator;

                    Boolean result =
                        this.Id == objectToBeCompared.Id &&
                        this.DataQualityIndicatorValuesTableColumnName == objectToBeCompared.DataQualityIndicatorValuesTableColumnName &&
                        this.StateViewAttributeId == objectToBeCompared.StateViewAttributeId &&
                        this.Weight == objectToBeCompared.Weight &&
                        this.IsCritical == objectToBeCompared.IsCritical &&
                        this.Description == objectToBeCompared.Description;

                    if (result)
                    {
                        if (
                            !CompareCollections(this.ContainerIds, objectToBeCompared.ContainerIds) ||
                            !CompareCollections(this.CategoryIds, objectToBeCompared.CategoryIds) ||
                            !CompareCollections(this.EntityTypeIds, objectToBeCompared.EntityTypeIds)
                            )
                        {
                            return false;
                        }
                    }

                    return result;
                }
            }
            return false;
        }

        /// <summary>
        /// Serves as a hash function for type
        /// </summary>
        /// <returns>A hash code for the current Object</returns>
        public override Int32 GetHashCode()
        {
            Int32 containersHashCode = 0;
            if (this.ContainerIds != null)
            {
                foreach (Int32 item in this.ContainerIds)
                {
                    containersHashCode += item.GetHashCode();
                }
            }
            Int32 categoriesHashCode = 0;
            if (this.CategoryIds != null)
            {
                foreach (Int64 item in this.CategoryIds)
                {
                    categoriesHashCode += item.GetHashCode();
                }
            }
            Int32 entityTypesHashCode = 0;
            if (this.EntityTypeIds != null)
            {
                foreach (Int32 item in this.EntityTypeIds)
                {
                    entityTypesHashCode += item.GetHashCode();
                }
            }

            return
                base.GetHashCode()
                ^ this.Id.GetHashCode()
                ^ ((this.DataQualityIndicatorValuesTableColumnName != null) ? this.DataQualityIndicatorValuesTableColumnName.GetHashCode() : 0)
                ^ this.StateViewAttributeId.GetHashCode()
                ^ this.Weight.GetHashCode()
                ^ this.IsCritical.GetHashCode()
                ^ this.Description.GetHashCode()
                ^ containersHashCode
                ^ categoriesHashCode
                ^ entityTypesHashCode;
        }

        
        #region Xml Serialization

        /// <summary>
        /// Gets Xml representation of current object
        /// </summary>
        /// <returns>Returns Xml representation of current object as string</returns>
        public override String ToXml()
        {
            String result = String.Empty;

            using (StringWriter sw = new StringWriter())
            using (XmlTextWriter xmlWriter = new XmlTextWriter(sw))
            {
                xmlWriter.WriteStartElement("DataQualityIndicator");

                xmlWriter.WriteAttributeString("Id", Id.ToString(CultureInfo.InvariantCulture));
                xmlWriter.WriteAttributeString("Name", Name);
                xmlWriter.WriteAttributeString("LongName", LongName);
                xmlWriter.WriteAttributeString("DataQualityIndicatorValuesTableColumnName", DataQualityIndicatorValuesTableColumnName);
                xmlWriter.WriteAttributeString("StateViewAttributeId", StateViewAttributeId.ToString());
                xmlWriter.WriteAttributeString("Weight", Weight.ToString());
                xmlWriter.WriteAttributeString("IsCritical", IsCritical.ToString());
                xmlWriter.WriteAttributeString("Description", Description);
                
                #region Write Containers Ids

                xmlWriter.WriteStartElement("Containers");

                if (!ContainerIds.IsNullOrEmpty())
                {
                    foreach (Int32 containerId in ContainerIds)
                    {
                        xmlWriter.WriteStartElement("Container");

                        xmlWriter.WriteAttributeString("Id", containerId.ToString(CultureInfo.InvariantCulture));

                        xmlWriter.WriteEndElement();
                    }
                }

                xmlWriter.WriteEndElement();

                #endregion Write Containers Ids

                #region Write Categories Ids

                xmlWriter.WriteStartElement("Categories");

                if (CategoryIds != null)
                {
                    foreach (Int64 categoryId in CategoryIds)
                    {
                        xmlWriter.WriteStartElement("Category");

                        xmlWriter.WriteAttributeString("Id", categoryId.ToString(CultureInfo.InvariantCulture));

                        xmlWriter.WriteEndElement();
                    }
                }

                xmlWriter.WriteEndElement();

                #endregion Write Categories Ids

                #region Write Entity Types Ids

                xmlWriter.WriteStartElement("EntityTypes");

                if (EntityTypeIds != null)
                {
                    foreach (Int32 entityTypeId in EntityTypeIds)
                    {
                        xmlWriter.WriteStartElement("EntityType");

                        xmlWriter.WriteAttributeString("Id", entityTypeId.ToString(CultureInfo.InvariantCulture));

                        xmlWriter.WriteEndElement();
                    }
                }

                xmlWriter.WriteEndElement();

                #endregion Write Entity Types Ids

                xmlWriter.WriteEndElement();
                xmlWriter.Flush();

                // Get the actual XML
                result = sw.ToString();
            }

            return result;
        }

        /// <summary>
        /// Loads current object from provided XmlNode
        /// </summary>
        /// <param name="xmlNode">XmlNode for deserialization</param>
        public void LoadFromXml(XmlNode xmlNode)
        {
            if (xmlNode == null)
            {
                return;
            }
            if (xmlNode.Attributes != null)
            {
                if (xmlNode.Attributes["Id"] != null)
                {
                    Id = ValueTypeHelper.Int16TryParse(xmlNode.Attributes["Id"].Value, Id);
                }
                if (xmlNode.Attributes["Name"] != null)
                {
                    Name = xmlNode.Attributes["Name"].Value;
                }
                if (xmlNode.Attributes["LongName"] != null)
                {
                    LongName = xmlNode.Attributes["LongName"].Value;
                }
                if (xmlNode.Attributes["DataQualityIndicatorValuesTableColumnName"] != null)
                {
                    DataQualityIndicatorValuesTableColumnName = xmlNode.Attributes["DataQualityIndicatorValuesTableColumnName"].Value;
                }
                if (xmlNode.Attributes["StateViewAttributeId"] != null)
                {
                    StateViewAttributeId = ValueTypeHelper.ConvertToNullableInt16(xmlNode.Attributes["StateViewAttributeId"].Value);
                }
                if (xmlNode.Attributes["Weight"] != null)
                {
                    Weight = ValueTypeHelper.ConvertToNullableByte(xmlNode.Attributes["Weight"].Value);
                }
                if (xmlNode.Attributes["IsCritical"] != null)
                {
                    IsCritical = ValueTypeHelper.ConvertToNullableBoolean(xmlNode.Attributes["IsCritical"].Value);
                }
                if (xmlNode.Attributes["Description"] != null)
                {
                    Description = xmlNode.Attributes["Description"].Value;
                }
            }
            
            #region Read Containers Ids

            XmlNodeList nodes = xmlNode.SelectNodes(@"Containers/Container");
            ContainerIds = null;
            if (nodes != null)
            {
                foreach (XmlNode node in nodes)
                {
                    Int32 value;
                    if (node.Attributes != null && node.Attributes["Id"] != null && Int32.TryParse(node.Attributes["Id"].Value, out value))
                    {
                        if (ContainerIds == null)
                        {
                            ContainerIds = new Collection<Int32>();
                        }
                        if (!ContainerIds.Contains(value))
                        {
                            ContainerIds.Add(value);
                        }
                    }
                }
            }

            #endregion Read Containers Ids

            #region Read Categories Ids

            nodes = xmlNode.SelectNodes(@"Categories/Category");
            CategoryIds = null;
            if (nodes != null)
            {
                foreach (XmlNode node in nodes)
                {
                    Int64 value;
                    if (node.Attributes != null && node.Attributes["Id"] != null && Int64.TryParse(node.Attributes["Id"].Value, out value))
                    {
                        if (CategoryIds == null)
                        {
                            CategoryIds = new Collection<Int64>();
                        }
                        if (!CategoryIds.Contains(value))
                        {
                            CategoryIds.Add(value);
                        }
                    }
                }
            }

            #endregion Read Categories Ids

            #region Read Entity Types Ids

            nodes = xmlNode.SelectNodes(@"EntityTypes/EntityType");
            EntityTypeIds = null;
            if (nodes != null)
            {
                foreach (XmlNode node in nodes)
                {
                    Int32 value;
                    if (node.Attributes != null && node.Attributes["Id"] != null && Int32.TryParse(node.Attributes["Id"].Value, out value))
                    {
                        if (EntityTypeIds == null)
                        {
                            EntityTypeIds = new Collection<Int32>();
                        }
                        if (!EntityTypeIds.Contains(value))
                        {
                            EntityTypeIds.Add(value);
                        }
                    }
                }
            }

            #endregion Read Entity Typed Ids
        }

        /// <summary>
        /// Loads current object from provided Xml
        /// </summary>
        /// <param name="xml">Xml string for deserialization</param>
        public void LoadFromXml(String xml)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            XmlNode node = doc.SelectSingleNode("DataQualityIndicator");
            if (node != null)
            {
                LoadFromXml(node);
            }
        }


        /// <summary>
        /// Get Xml representation of Additional Settings only properties (ContainerIds, CategoryIds and EntityTypeIds)
        /// </summary>
        public String AdditionalSettingsOnlyToXml()
        {
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //Validation Profile node start
            xmlWriter.WriteStartElement("AdditionalSettings");

            #region Write Containers Ids

            xmlWriter.WriteStartElement("Containers");

            if (!ContainerIds.IsNullOrEmpty())
            {
                foreach (Int32 containerId in ContainerIds)
                {
                    xmlWriter.WriteStartElement("Container");

                    xmlWriter.WriteAttributeString("Id", containerId.ToString(CultureInfo.InvariantCulture));

                    xmlWriter.WriteEndElement();
                }
            }

            xmlWriter.WriteEndElement();

            #endregion Write Containers Ids

            #region Write Categories Ids

            xmlWriter.WriteStartElement("Categories");

            if (CategoryIds != null)
            {
                foreach (Int64 categoryId in CategoryIds)
                {
                    xmlWriter.WriteStartElement("Category");

                    xmlWriter.WriteAttributeString("Id", categoryId.ToString(CultureInfo.InvariantCulture));

                    xmlWriter.WriteEndElement();
                }
            }

            xmlWriter.WriteEndElement();

            #endregion Write Categories Ids

            #region Write Entity Types Ids

            xmlWriter.WriteStartElement("EntityTypes");

            if (EntityTypeIds != null)
            {
                foreach (Int32 entityTypeId in EntityTypeIds)
                {
                    xmlWriter.WriteStartElement("EntityType");

                    xmlWriter.WriteAttributeString("Id", entityTypeId.ToString(CultureInfo.InvariantCulture));

                    xmlWriter.WriteEndElement();
                }
            }

            xmlWriter.WriteEndElement();

            #endregion Write Entity Types Ids

            //Validation Profile node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //Get the actual XML
            String operationResultXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return operationResultXml;
        }

        /// <summary>
        /// Loads Additional Settings only properties (ContainerIds, CategoryIds and EntityTypeIds) from XML
        /// </summary>
        public void LoadAdditionalSettingsFromXml(String xmlData)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml(xmlData);

            #region Read Containers Ids

            XmlNodeList nodes = xDoc.SelectNodes(@"/AdditionalSettings/Containers/Container");
            ContainerIds = null;
            if (nodes != null)
            {
                foreach (XmlNode node in nodes)
                {
                    Int32 value;
                    if (node.Attributes != null && node.Attributes["Id"] != null && Int32.TryParse(node.Attributes["Id"].Value, out value))
                    {
                        if (ContainerIds == null)
                        {
                            ContainerIds = new Collection<Int32>();
                        }
                        if (!ContainerIds.Contains(value))
                        {
                            ContainerIds.Add(value);
                        }
                    }
                }
            }

            #endregion Read Containers Ids

            #region Read Categories Ids

            nodes = xDoc.SelectNodes(@"/AdditionalSettings/Categories/Category");
            CategoryIds = null;
            if (nodes != null)
            {
                foreach (XmlNode node in nodes)
                {
                    Int64 value;
                    if (node.Attributes != null && node.Attributes["Id"] != null && Int64.TryParse(node.Attributes["Id"].Value, out value))
                    {
                        if (CategoryIds == null)
                        {
                            CategoryIds = new Collection<Int64>();
                        }
                        if (!CategoryIds.Contains(value))
                        {
                            CategoryIds.Add(value);
                        }
                    }
                }
            }

            #endregion Read Categories Ids

            #region Read Entity Types Ids

            nodes = xDoc.SelectNodes(@"/AdditionalSettings/EntityTypes/EntityType");
            EntityTypeIds = null;
            if (nodes != null)
            {
                foreach (XmlNode node in nodes)
                {
                    Int32 value;
                    if (node.Attributes != null && node.Attributes["Id"] != null && Int32.TryParse(node.Attributes["Id"].Value, out value))
                    {
                        if (EntityTypeIds == null)
                        {
                            EntityTypeIds = new Collection<Int32>();
                        }
                        if (!EntityTypeIds.Contains(value))
                        {
                            EntityTypeIds.Add(value);
                        }
                    }
                }
            }

            #endregion Read Entity Typed Ids
        }

        #endregion

        #endregion

        #region Private Methods

        private Boolean CompareCollections<T>(Collection<T> a, Collection<T> b)
        {
            if ((a == null) != (b == null))
            {
                return false;
            }
            if (a == null)
            {
                return true;
            }
            if (a.Count != b.Count)
            {
                return false;
            }
            if (a.Count == 0)
            {
                return true;
            }
            List<T> aSorted = a.OrderBy(x => x).ToList();
            List<T> bSorted = b.OrderBy(x => x).ToList();
            for (Int32 i = 0; i < aSorted.Count; i++)
            {
                if (!aSorted[i].Equals(bSorted[i]))
                {
                    return false;
                }
            }
            return true;
        }

        #endregion
    }
}