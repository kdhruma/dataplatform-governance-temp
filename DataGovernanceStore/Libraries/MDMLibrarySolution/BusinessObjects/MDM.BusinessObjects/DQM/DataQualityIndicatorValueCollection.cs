using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.IO;
using System.Xml;

namespace MDM.BusinessObjects.DQM
{
    using MDM.Interfaces;
    using MDM.Core;
    using MDM.Core.Extensions;

    /// <summary>
    /// Specifies the DataQualityIndicator Value Collection
    /// </summary>
    [DataContract]
    public class DataQualityIndicatorValueCollection : ICollection<DataQualityIndicatorValue>
    {
        #region Fields

        [DataMember]
        private Collection<DataQualityIndicatorValue> _dataQualityIndicatorValues = new Collection<DataQualityIndicatorValue>();

        /// <summary>
        /// Field denoting name of main node in Xml for NormalizationResultsCollection
        /// </summary>    
        private const String NodeName = "DataQualityIndicatorValue";

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public DataQualityIndicatorValueCollection()
        {
            this._dataQualityIndicatorValues = new Collection<DataQualityIndicatorValue>();
        }

        /// <summary>
        /// Initialize DataQualityIndicatorValueCollection from IList
        /// </summary>
        /// <param name="dataQualityIndicatorValuesList">IList of DataQualityIndicatorValues</param>
        public DataQualityIndicatorValueCollection(IList<DataQualityIndicatorValue> dataQualityIndicatorValuesList)
        {
            _dataQualityIndicatorValues = new Collection<DataQualityIndicatorValue>(dataQualityIndicatorValuesList);
        }

        /// <summary>
        /// Initialize DataQualityIndicatorValueCollection from String in Xml formal
        /// </summary>
        /// <param name="valuesAsXml">String in Xml format of DataQualityIndicatorValues</param>
        public DataQualityIndicatorValueCollection(String valuesAsXml)
        {
            LoadDataQualityIndicatorValueCollection(valuesAsXml);
        }

        #endregion

        #region Properties

        #endregion

        #region Methods

        #region Public Method

        /// <summary>
        /// Remove DataQualityIndicatorValue object from DataQualityIndicatorValueCollection
        /// </summary>
        /// <param name="dataQualityIndicatorId">Id of DataQualityIndicator which is to be removed from collection</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original collection</returns>
        public Boolean Remove(Int16 dataQualityIndicatorId)
        {
            DataQualityIndicatorValue dataQualityIndicatorValue = Get(dataQualityIndicatorId);

            if (dataQualityIndicatorValue == null)
            {
                return false;
        }

            return Remove(dataQualityIndicatorValue);
        }

        /// <summary>
        /// Determines whether two Object instances are equal
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false</returns>
        public override Boolean Equals(Object obj)
        {
            if (obj != null)
            {
                DataQualityIndicatorValueCollection objectToBeCompared = obj as DataQualityIndicatorValueCollection;
                if (objectToBeCompared != null)
                {
                    if (ReferenceEquals(this, objectToBeCompared))
                    {
                        return true;
                    }

                Int32 dataQualityIndicatorValuesUnion = _dataQualityIndicatorValues.ToList().Union(objectToBeCompared.ToList()).Count();
                Int32 dataQualityIndicatorValuesIntersect = _dataQualityIndicatorValues.ToList().Intersect(objectToBeCompared.ToList()).Count();

                    return (dataQualityIndicatorValuesUnion == dataQualityIndicatorValuesIntersect);
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
            Int32 hashCode = 0;
            foreach (DataQualityIndicatorValue item in _dataQualityIndicatorValues)
            {
                hashCode += item.GetHashCode();
            }
            return hashCode;
        }

        #endregion

        #region Private Method

        /// <summary>
        /// Get DataQualityIndicatorValue from current DataQualityIndicatorValue collection based on DataQualityIndicator Id
        /// </summary>
        /// <param name="dataQualityIndicatorId">Id of DataQualityIndicator which is to be searched</param>
        /// <returns>DataQualityIndicatorValue having given DataQualityIndicatorValueId messageCode</returns>
        private DataQualityIndicatorValue Get(Int16 dataQualityIndicatorId)
        {
            return _dataQualityIndicatorValues.FirstOrDefault(a => a.DataQualityIndicatorId == dataQualityIndicatorId);
        }

        private void LoadDataQualityIndicatorValueCollection(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name.Equals(NodeName))
                        {
                            String dataQualityIndicatorValueXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(dataQualityIndicatorValueXml))
                            {
                                DataQualityIndicatorValue normalizationResult = new DataQualityIndicatorValue(dataQualityIndicatorValueXml);
                                Add(normalizationResult);
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

        #endregion

        #endregion

        #region ICollection<DataQualityIndicatorValue> Members

        /// <summary>
        /// Add DataQualityIndicatorValue object in collection
        /// </summary>
        /// <param name="item">DataQualityIndicatorValue to add in collection</param>
        public void Add(DataQualityIndicatorValue item)
        {
            _dataQualityIndicatorValues.Add(item);
        }

        /// <summary>
        /// Add IDataQualityIndicatorValue object in collection
        /// </summary>
        /// <param name="item">IDataQualityIndicatorValue to add in collection</param>
        public void Add(IDataQualityIndicatorValue item)
        {
            _dataQualityIndicatorValues.Add((DataQualityIndicatorValue)item);
        }

        /// <summary>
        /// Removes all DataQualityIndicatorValues from collection
        /// </summary>
        public void Clear()
        {
            _dataQualityIndicatorValues.Clear();
        }

        /// <summary>
        /// Determines whether the DataQualityIndicatorValueCollection contains a specific DataQualityIndicatorValue
        /// </summary>
        /// <param name="item">The DataQualityIndicatorValue object to locate in the DataQualityIndicatorValueCollection</param>
        /// <returns>
        /// <para>true : If DataQualityIndicatorValue found in DataQualityIndicatorValueCollection</para>
        /// <para>false : If DataQualityIndicatorValue found not in DataQualityIndicatorValueCollection</para>
        /// </returns>
        public Boolean Contains(DataQualityIndicatorValue item)
        {
            return _dataQualityIndicatorValues.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the DataQualityIndicatorValueCollection to an
        /// System.Array, starting at a particular System.Array index
        /// </summary>
        /// <param name="array"> 
        ///  The one-dimensional System.Array that is the destination of the elements copied from DataQualityIndicatorValueCollection. The System.Array must have zero-based indexing
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins</param>
        public void CopyTo(DataQualityIndicatorValue[] array, Int32 arrayIndex)
        {
            _dataQualityIndicatorValues.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Get the count of no. of DataQualityIndicatorValues in DataQualityIndicatorValueCollection
        /// </summary>
        public Int32 Count
        {
            get
            {
                return _dataQualityIndicatorValues.Count;
            }
        }

        /// <summary>
        /// Check if DataQualityIndicatorValueCollection is read-only
        /// </summary>
        public Boolean IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the DataQualityIndicatorValueCollection
        /// </summary>
        /// <param name="item">The DataQualityIndicatorValue object to remove from the DataQualityIndicatorValueCollection</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original DataQualityIndicatorValueCollection</returns>
        public Boolean Remove(DataQualityIndicatorValue item)
        {
            return _dataQualityIndicatorValues.Remove(item);
        }

        #endregion

        #region IEnumerable<DataQualityIndicatorValue> Members

        /// <summary>
        /// Returns an enumerator that iterates through a DataQualityIndicatorValueCollection
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection</returns>
        public IEnumerator<DataQualityIndicatorValue> GetEnumerator()
        {
            return _dataQualityIndicatorValues.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a DataQualityIndicatorValueCollection
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _dataQualityIndicatorValues.GetEnumerator();
        }

        #endregion

        #region Xml Serialization Members

        /// <summary>
        /// Denotes method for xml serialization
        /// </summary>
        public String ToXml(Boolean withOuterNode = true)
        {
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            if (withOuterNode)
            {
                xmlWriter.WriteStartElement("DataQualityIndicatorValueCollection");
            }

            foreach (DataQualityIndicatorValue dataQualityIndicatorValue in this)
            {
                xmlWriter.WriteRaw(dataQualityIndicatorValue.ToXml());
            }

            if (withOuterNode)
            {
                xmlWriter.WriteEndElement();
            }

            xmlWriter.Flush();

            String result = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return result;
        }

        /// <summary>
        /// Denotes method for xml deserialization
        /// </summary>
        public void LoadFromXmlWithOuterNode(String xml)
        {
            if (!String.IsNullOrWhiteSpace(xml))
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xml);

                XmlNode node = doc.SelectSingleNode("DataQualityIndicatorValueCollection");
                if (node != null)
                {
                    LoadFromXml(node);
                }
            }
        }

        /// <summary>
        /// Denotes method for xml deserialization
        /// </summary>
        public void LoadFromXml(XmlNode node)
        {
            Clear();
            foreach (XmlNode child in node.ChildNodes)
            {
                if (child.Name == "DataQualityIndicatorValue")
                {
                    DataQualityIndicatorValue item = new DataQualityIndicatorValue();
                    item.LoadFromXml(child);
                    Add(item);
                }
            }
        }

        #region Database Storage Conversion

        #region Xml Example

        //<FAILED>
        //    <DataQualityIndicator ID="1">
        //        <INFO AID="123" RID="215" MSGC="122330">
        //            <PARAMS>
        //                <PARAM>Hello, World!></PARAM>
        //                <PARAM>123></PARAM>
        //                <PARAM><![CDATA[First argument must be >= Second]]></PARAM>
        //            </PARAMS>
        //        </INFO>
        //    </DataQualityIndicator>
        //    <DataQualityIndicator ID="2">
        //        <INFO AID="125" RID="311">
        //            <MSG><![CDATA[123 <= 152]]></MSG>
        //        </INFO>
        //    </DataQualityIndicator>
        //</FAILED>

        #endregion

        /// <summary>
        /// Converts DataQualityIndicators Failure Info to xml for storing in DB
        /// </summary>
        /// <returns>Xml for DB storage</returns>
        public String DataQualityIndicatorFailureInfoToDatabaseStorageForm()
        {
            String result = String.Empty;

            if (_dataQualityIndicatorValues == null)
            {
                return result;
            }

            IList<DataQualityIndicatorValue> failedDataQualityIndicators = _dataQualityIndicatorValues.Where(dataQualityIndicator => dataQualityIndicator.Value == false && !dataQualityIndicator.FailureInfoCollection.IsNullOrEmpty()).ToList();

            if (!failedDataQualityIndicators.IsNullOrEmpty())
            {
                StringWriter sw = new StringWriter();
                XmlTextWriter xmlWriter = new XmlTextWriter(sw);
                xmlWriter.WriteStartElement("FAILED");

                foreach (DataQualityIndicatorValue dataQualityIndicator in failedDataQualityIndicators)
                {
                    xmlWriter.WriteStartElement("DataQualityIndicator");
                    xmlWriter.WriteAttributeString("ID", dataQualityIndicator.DataQualityIndicatorId.ToString(CultureInfo.InvariantCulture));

                    foreach (DataQualityIndicatorFailureInfo failure in dataQualityIndicator.FailureInfoCollection)
                    {
                        xmlWriter.WriteStartElement("INFO");

                        if (failure.AttributeId.HasValue)
                        {
                            xmlWriter.WriteAttributeString("AID", failure.AttributeId.Value.ToString(CultureInfo.InvariantCulture));
                        }
                        if (failure.RelationshipId.HasValue)
                        {
                            xmlWriter.WriteAttributeString("RID", failure.RelationshipId.Value.ToString(CultureInfo.InvariantCulture));
                        }
                        if (failure.LocaleId.HasValue)
                        {
                            xmlWriter.WriteAttributeString("LID", failure.LocaleId.Value.ToString(CultureInfo.InvariantCulture));
                        }

                        // If message code exists then write it as an attributes and write it's parameters
                        // otherwise write failure message CNode
                        if (!String.IsNullOrEmpty(failure.FailureMessageCode))
                        {
                            xmlWriter.WriteAttributeString("MSGC", failure.FailureMessageCode);
                            if (!failure.Params.IsNullOrEmpty())
                            {
                                xmlWriter.WriteStartElement("PARAMS");
                                foreach (Object param in failure.Params)
                                {
                                    xmlWriter.WriteStartElement("PARAM");
                                    xmlWriter.WriteRaw(param.ToString().WrapWithCDataBlock(true));
                                    xmlWriter.WriteEndElement(); // </PARAM>
                                }
                                xmlWriter.WriteEndElement(); // </PARAMS>
                            }
                        }
                        else
                        {
                            if (!String.IsNullOrEmpty(failure.FailureMessage))
                            {
                                xmlWriter.WriteStartElement("MSG");
                                xmlWriter.WriteRaw(failure.FailureMessage.WrapWithCDataBlock(true));
                                xmlWriter.WriteEndElement();
                            }
                        }
                        
                        if (!String.IsNullOrEmpty(failure.AttributeValue))
                        {
                            xmlWriter.WriteStartElement("ATTRVAL");
                            xmlWriter.WriteRaw(failure.AttributeValue.WrapWithCDataBlock(true));
                            xmlWriter.WriteEndElement();
                        }

                        xmlWriter.WriteEndElement(); // </INFO>
                    }
                    xmlWriter.WriteEndElement(); // </DataQualityIndicator>
                }

                xmlWriter.WriteEndElement(); // </FAILED>

                xmlWriter.Flush();

                //Get the actual XML
                result = sw.ToString();

                xmlWriter.Close();
                sw.Close();
            }

            return result;
        }

        /// <summary>
        /// Loads DataQualityIndicators Failure Info from xml form DB
        /// </summary>
        /// <param name="xml">Xml from DB</param>
        public void LoadDataQualityIndicatorFailureInfoFromDatabaseStorageForm(String xml)
        {
            if (String.IsNullOrWhiteSpace(xml) || _dataQualityIndicatorValues.IsNullOrEmpty())
            {
                return;
            }

            // FailureInfoCollection information clearing
            foreach (DataQualityIndicatorValue dataQualityIndicatorValue in _dataQualityIndicatorValues)
            {
                dataQualityIndicatorValue.FailureInfoCollection = null;
            }

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            XmlNodeList dataQualityIndicatorNodes = doc.SelectNodes("/FAILED/DataQualityIndicator");
            if (dataQualityIndicatorNodes == null || dataQualityIndicatorNodes.Count == 0)
            {
                return;
            }

            foreach (XmlNode dataQualityIndicatorNode in dataQualityIndicatorNodes)
            {
                Int16? dataQualityIndicatorId = null;
                if (dataQualityIndicatorNode.Attributes != null)
                {
                    if (dataQualityIndicatorNode.Attributes["ID"] != null)
                    {
                        dataQualityIndicatorId = ValueTypeHelper.ConvertToNullableInt16(dataQualityIndicatorNode.Attributes["ID"].Value);
                    }
                }
                if (!dataQualityIndicatorId.HasValue)
                {
                    continue;
                }
                DataQualityIndicatorValue dataQualityIndicatorValue = _dataQualityIndicatorValues.FirstOrDefault(x => x.DataQualityIndicatorId == dataQualityIndicatorId.Value);
                if (dataQualityIndicatorValue == null)
                {
                    continue;
                }

                XmlNodeList infoNodes = dataQualityIndicatorNode.SelectNodes("INFO");
                if (infoNodes != null && infoNodes.Count > 0)
                {
                    DataQualityIndicatorFailureInfoCollection failuresInfo = new DataQualityIndicatorFailureInfoCollection();

                    foreach (XmlNode infoNode in infoNodes)
                    {
                        DataQualityIndicatorFailureInfo failureInfo = new DataQualityIndicatorFailureInfo();

                        if (infoNode.Attributes != null)
                        {
                            if (infoNode.Attributes["AID"] != null)
                            {
                                failureInfo.AttributeId =
                                    ValueTypeHelper.ConvertToNullableInt32(infoNode.Attributes["AID"].Value);
                            }
                            if (infoNode.Attributes["RID"] != null)
                            {
                                failureInfo.RelationshipId =
                                    ValueTypeHelper.ConvertToNullableInt64(infoNode.Attributes["RID"].Value);
                            }
                            if (infoNode.Attributes["LID"] != null)
                            {
                                failureInfo.LocaleId =
                                    ValueTypeHelper.ConvertToNullableInt16(infoNode.Attributes["LID"].Value);
                            }
                            if (infoNode.Attributes["MSGC"] != null)
                            {
                                failureInfo.FailureMessageCode = infoNode.Attributes["MSGC"].Value;
                            }
                        }

                        if (!String.IsNullOrWhiteSpace(failureInfo.FailureMessageCode))
                        {
                            XmlNodeList paramNodes = infoNode.SelectNodes("PARAMS/PARAM");
                            if (paramNodes != null)
                            {
                                foreach (XmlNode paramNode in paramNodes)
                                {
                                    Object param = paramNode.InnerText;
                                    failureInfo.Params.Add(param);
                                }
                            }
                        }
                        else
                        {
                            XmlNode msgNode = infoNode.SelectSingleNode("MSG");
                            if (msgNode != null)
                            {
                                failureInfo.FailureMessage = msgNode.InnerText;
                            }
                        }

                        XmlNode attrValNode = infoNode.SelectSingleNode("ATTRVAL");
                        if (attrValNode != null)
                        {
                            failureInfo.AttributeValue = attrValNode.InnerText;
                        }

                        failuresInfo.Add(failureInfo);
                    }

                    if (failuresInfo.Any())
                    {
                        dataQualityIndicatorValue.FailureInfoCollection = failuresInfo;
                    }
                }
            }
        }

        #endregion

        #endregion
    }
}