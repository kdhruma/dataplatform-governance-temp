using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects.DQM
{
    using MDM.Core;
    using MDM.Interfaces;
    using MDM.Core.Extensions;

    /// <summary>
    /// Specifies EntityQualityData
    /// </summary>
    [DataContract]
    public class EntityQualityData : IEntityQualityData
    {
        #region Fields

        #endregion

        #region Constructors

        /// <summary>
        /// Parameter less constructor
        /// </summary>
        public EntityQualityData()
        {
            DataQualityIndicatorValues = new DataQualityIndicatorValueCollection();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public EntityQualityData(Int64 entityId, Byte? overallScore, DateTime? measurementDate) : this()
        {
            EntityId = entityId;
            OverallScore = overallScore;
            MeasurementDate = measurementDate;
        }

        /// <summary>
        /// Constructor with XML-format String of an EntityQualityData as input parameter
        /// </summary>
        /// <param name="valueAsXml"></param>
        public EntityQualityData(String valueAsXml)
        {
            LoadEntityQualityDataFromXml(valueAsXml);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Property denoting Entity Id
        /// </summary>
        [DataMember]
        public Int64 EntityId { get; set; }

        /// <summary>
        /// Property denoting Overall Score
        /// </summary>
        [DataMember]
		public Byte? OverallScore { get; set; }

        /// <summary>
        /// Property denoting Measurement Date
        /// </summary>
        [DataMember]
        public DateTime? MeasurementDate { get; set; }

        /// <summary>
        /// Property denoting the list of DataQualityIndicator Values of an entity
        /// </summary>
        [DataMember]
        public DataQualityIndicatorValueCollection DataQualityIndicatorValues { get; set; }

        /// <summary>
        /// Property denoting if Simmarization Required
        /// </summary>
        [DataMember]
        public Boolean SummarizationRequired { get; set; }

        #endregion

        #region Public Methods

        #region Equality Comparison

        /// <summary>
        /// Determines whether two Object instances are equal
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false</returns>
        public override Boolean Equals(Object obj)
        {
            if (obj != null)
            {
                EntityQualityData objectToBeCompared = obj as EntityQualityData;
                if (objectToBeCompared != null)
                {
                    if (ReferenceEquals(this, objectToBeCompared))
                    {
                        return true;
                    }

                    return
                        EntityId == objectToBeCompared.EntityId &&
                        OverallScore == objectToBeCompared.OverallScore &&
                        MeasurementDate == objectToBeCompared.MeasurementDate &&
                        DataQualityIndicatorValues.Equals(objectToBeCompared.DataQualityIndicatorValues) &&
                        SummarizationRequired == objectToBeCompared.SummarizationRequired;
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
            return
                EntityId.GetHashCode()
                ^ OverallScore.GetHashCode()
                ^ MeasurementDate.GetHashCode()
                ^ DataQualityIndicatorValues.GetHashCode()
                ^ SummarizationRequired.GetHashCode();
        }

        #endregion

        #region Private Methods

        private void LoadEntityQualityDataFromXml(String valueAsXml)
        {
            XmlTextReader reader = null;

            try
            {
                reader = new XmlTextReader(valueAsXml, XmlNodeType.Element, null);

                while (!reader.EOF)
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        switch (reader.Name)
                        {
                            case "EntityId":
                                EntityId = ValueTypeHelper.Int64TryParse(reader.ReadElementContentAsString(), 0);
                                break;
                            case "OverallScore":
                                OverallScore = ValueTypeHelper.ConvertToNullableByte(reader.ReadElementContentAsString());
                                break;
                            case "MeasurementDate":
                                MeasurementDate = ValueTypeHelper.ConvertToNullableDateTime(reader.ReadElementContentAsString());
                                break;
                            case "DataQualityIndicatorValues":
                                DataQualityIndicatorValues = ParseDataQualityIndicatorValueCollection(reader);
                                break;
                            case "SummarizationRequired":
                                SummarizationRequired = ValueTypeHelper.BooleanTryParse(reader.ReadElementContentAsString(), false);
                                break;
                            default:
                                reader.Read();
                                break;
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

        private DataQualityIndicatorValueCollection ParseDataQualityIndicatorValueCollection(XmlReader reader)
        {
            if (reader == null)
            {
                return null;
            }

            String attributeXml = reader.ReadOuterXml();
            if (String.IsNullOrEmpty(attributeXml))
            {
                return null;
            }

            DataQualityIndicatorValueCollection dataQualityIndicatorValueCollection = new DataQualityIndicatorValueCollection(attributeXml);
            
            return dataQualityIndicatorValueCollection;
        }

        #endregion
        
        #region Xml Serialization

        /// <summary>
        /// Serializae EntityQualityData to xml
        /// </summary>
        /// <returns></returns>
        public String ToXml()
        {
            String xml = String.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //Information node start
            xmlWriter.WriteStartElement("EntityQualityData");

            xmlWriter.WriteAttributeString("EntityId", EntityId.ToString(CultureInfo.InvariantCulture));

            xmlWriter.WriteAttributeString("OverallScore", OverallScore.ToString());

            xmlWriter.WriteAttributeString("MeasurementDate",
                MeasurementDate.HasValue ? MeasurementDate.Value.ToString("o") : MeasurementDate.ToString());

            xmlWriter.WriteAttributeString("SummarizationRequired",
                SummarizationRequired.ToString(CultureInfo.InvariantCulture));

            xmlWriter.WriteStartElement("DataQualityIndicatorValues");
            xmlWriter.WriteRaw(DataQualityIndicatorValues.ToXml(false));
            xmlWriter.WriteEndElement();

            //Information node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //Get the actual XML
            xml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return xml;
        }

        /// <summary>
        /// Loads EntityQualityData from XmlNode
        /// </summary>
        /// <param name="node">XmlNode for deserialization</param>
        public void LoadFromXml(XmlNode node)
        {
            Clear();
            if (node == null)
            {
                return;
            }

            if (node.Attributes != null)
            {
                if (node.Attributes["EntityId"] != null)
                {
                    EntityId = ValueTypeHelper.ConvertToNullableInt64(node.Attributes["EntityId"].Value) ?? EntityId;
                }
                if (node.Attributes["OverallScore"] != null)
                {
                    OverallScore = ValueTypeHelper.ConvertToNullableByte(node.Attributes["OverallScore"].Value);
                }
                if (node.Attributes["MeasurementDate"] != null)
                {
                    MeasurementDate = ValueTypeHelper.ConvertToNullableDateTime(node.Attributes["MeasurementDate"].Value);
                }
                if (node.Attributes["SummarizationRequired"] != null)
                {
                    SummarizationRequired =
                        ValueTypeHelper.ConvertToNullableBoolean(node.Attributes["SummarizationRequired"].Value) ??
                        SummarizationRequired;
                }
            }

            foreach (XmlNode child in node.ChildNodes)
            {
                String nodeName = child.Name;
                switch (nodeName)
                {
                    case "DataQualityIndicatorValues":
                        DataQualityIndicatorValues.LoadFromXml(child);
                        break;
                }
            }
        }

        /// <summary>
        /// Loads EntityQualityData from XML with outer node
        /// </summary>
        /// <param name="xmlWithOuterNode">Xml for deserialization</param>
        public void LoadFromXmlWithOuterNode(String xmlWithOuterNode)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlWithOuterNode);
            XmlNode node = doc.SelectSingleNode("EntityQualityData");
            if (node != null)
            {
                LoadFromXml(node);
            }
        }

        #endregion
        
        #endregion

        #region Private Methods

        /// <summary>
        /// Resets state of current EntityQualityData
        /// </summary>
        private void Clear()
        {
            EntityId = 0;
            OverallScore = null;
            MeasurementDate = null;
            SummarizationRequired = false;
            DataQualityIndicatorValues.Clear();
        }

        #endregion

    }
}