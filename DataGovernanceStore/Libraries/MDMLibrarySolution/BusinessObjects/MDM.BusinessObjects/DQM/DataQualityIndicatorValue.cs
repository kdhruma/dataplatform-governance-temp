using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects.DQM
{
    using MDM.Interfaces;
    using MDM.Core;
    using MDM.Core.Extensions;

    /// <summary>
    /// Specifies DataQualityIndicator Value
    /// </summary>
    [DataContract]
    public class DataQualityIndicatorValue : IDataQualityIndicatorValue
    {
        #region Fields

        /// <summary>
        /// Field denoting name of main node in Xml for NormalizationResult
        /// </summary>    
        private const String NodeName = "DataQualityIndicatorValue";

        #endregion

        #region Constructors

        /// <summary>
        /// Parameter less constructor
        /// </summary>
        public DataQualityIndicatorValue() { }

        /// <summary>
        /// Constructor
        /// </summary>
        public DataQualityIndicatorValue(String xml)
        {
            LoadFromXmlWithOuterNode(xml);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public DataQualityIndicatorValue(Int16 dataQualityIndicatorId)
            : this(dataQualityIndicatorId, null, null)
        {
            this.DataQualityIndicatorId = dataQualityIndicatorId;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public DataQualityIndicatorValue(Int16 dataQualityIndicatorId, Boolean? value)
            : this(dataQualityIndicatorId, value, null)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public DataQualityIndicatorValue(Int16 dataQualityIndicatorId, Boolean? value, DataQualityIndicatorFailureInfoCollection failures)
        {
            this.DataQualityIndicatorId = dataQualityIndicatorId;
            this.Value = value;
            this.FailureInfoCollection = failures;
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        public DataQualityIndicatorValue(DataQualityIndicatorValue dataQualityIndicator)
            : this(dataQualityIndicator.DataQualityIndicatorId, dataQualityIndicator.Value, dataQualityIndicator.FailureInfoCollection)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Property denoting DataQualityIndicator Id
        /// </summary>
        [DataMember]
        public Int16 DataQualityIndicatorId { get; set; }

        /// <summary>
        /// Property denoting DataQualityIndicator Value
        /// </summary>
        [DataMember]
        public Boolean? Value { get; set; }

        /// <summary>
        /// Property denoting Failure Info Collection
        /// </summary>
        [DataMember]
        public DataQualityIndicatorFailureInfoCollection FailureInfoCollection { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Determines whether two Object instances are equal
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false</returns>
        public override Boolean Equals(object obj)
        {
            if (obj != null)
            {
                DataQualityIndicatorValue objectToBeCompared = obj as DataQualityIndicatorValue;
                if (objectToBeCompared != null)
                {
                    if (ReferenceEquals(this, objectToBeCompared))
                    {
                        return true;
                    }

                    return
                        this.DataQualityIndicatorId == objectToBeCompared.DataQualityIndicatorId &&
                        this.Value == objectToBeCompared.Value &&
                        (this.FailureInfoCollection != null
                            ? this.FailureInfoCollection.Equals(objectToBeCompared.FailureInfoCollection)
                            : objectToBeCompared.FailureInfoCollection == null);
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
                this.DataQualityIndicatorId.GetHashCode()
                ^ this.Value.GetHashCode()
                ^ (this.FailureInfoCollection != null ? this.FailureInfoCollection.GetHashCode() : 0);
        }

        #region DataQualityIndicator Failures Handling

        /// <summary>
        /// Adds failure to DataQualityIndicatorFailureInfoCollection
        /// </summary>
        /// <param name="failure">DataQualityIndicatorFailureInfo that need to be added to DataQualityIndicatorValue</param>
        public void AddFailure(DataQualityIndicatorFailureInfo failure)
        {
            if (FailureInfoCollection.IsNullOrEmpty())
            {
                FailureInfoCollection = new DataQualityIndicatorFailureInfoCollection();
            }
            FailureInfoCollection.Add(failure);
            Value = false; // If we are adding a failure then DataQualityIndicator has failed
        }

        /// <summary>
        /// Add collection of failures to current DataQualityIndicatorFailureInfoCollection
        /// </summary>
        /// <param name="failures">Collection of DataQualityIndicatorFailureInfo that need to be added to DataQualityIndicatorValue</param>
        public void AddFailures(DataQualityIndicatorFailureInfoCollection failures)
        {
            foreach (DataQualityIndicatorFailureInfo error in failures.DefaultIfEmpty())
            {
                this.AddFailure(error);
            }
        }

        /// <summary>
        /// Add failures to current DataQualityIndicatorValue from the passed one
        /// </summary>
        /// <param name="dataQualityIndicatorValue">Source DataQualityIndicatorValue for FailureInfoCollection</param>
        public void AddFailures(DataQualityIndicatorValue dataQualityIndicatorValue)
        {
            this.AddFailures(dataQualityIndicatorValue.FailureInfoCollection);
        }

        #endregion

        #region Xml Serialization
        

        /// <summary>
        /// Get Xml representation of DataQualityIndicatorValue
        /// </summary>
        /// <returns>Xml representation of DataQualityIndicatorValue object</returns>
        public String ToXml()
        {
            String xml = String.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //Information node start
            xmlWriter.WriteStartElement("DataQualityIndicatorValue");

            xmlWriter.WriteAttributeString("Id", DataQualityIndicatorId.ToString(CultureInfo.InvariantCulture));

            xmlWriter.WriteAttributeString("Value", Value.ToString());

            if (!FailureInfoCollection.IsNullOrEmpty())
            {
                xmlWriter.WriteStartElement("DataQualityIndicatorFailures");
                xmlWriter.WriteRaw(FailureInfoCollection.ToXml(false));
                xmlWriter.WriteEndElement();
            }

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
        /// Loads DataQualityIndicatorValue from XmlNode
        /// </summary>
        public void LoadFromXml(XmlNode node)
        {
            if (node == null)
            {
                return;
            }

            if (node.Attributes != null)
            {
                if (node.Attributes["Id"] != null)
                {
                    DataQualityIndicatorId = ValueTypeHelper.ConvertToNullableInt16(node.Attributes["Id"].Value) ?? DataQualityIndicatorId;
                }
                if (node.Attributes["Value"] != null)
                {
                    Value = ValueTypeHelper.ConvertToNullableBoolean(node.Attributes["Value"].Value);
                }
            }

            foreach (XmlNode child in node.ChildNodes)
            {
                String nodeName = child.Name;
                switch (nodeName)
                {
                    case "DataQualityIndicatorFailures":
                        FailureInfoCollection.LoadFromXml(child);
                        break;
                }
            }
        }

        /// <summary>
        /// Loads DataQualityIndicatorValue from XML with outer node
        /// </summary>
        public void LoadFromXmlWithOuterNode(String xmlWithOuterNode)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlWithOuterNode);
            XmlNode node = doc.SelectSingleNode("DataQualityIndicatorValue");
            if (node != null)
            {
                LoadFromXml(node);
            }
        }

        #endregion
        
        #endregion
    }
}