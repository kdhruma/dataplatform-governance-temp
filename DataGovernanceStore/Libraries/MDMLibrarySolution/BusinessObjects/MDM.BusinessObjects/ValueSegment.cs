using System;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies Value Segment
    /// </summary>
    [DataContract]
    public class ValueSegment : MDMObject, IValueSegment
    {
        #region Constants

        private const String ClassName = "ValueSegment";

        #endregion

        #region Fields

        #endregion

        #region Constructors

        /// <summary>
        /// Parameter less constructor
        /// </summary>
        public ValueSegment()
            :base()
        {
            LeftBoundOperator = SearchOperator.None;
            RightBoundOperator = SearchOperator.None;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Property denoting Left Bound
        /// </summary>
        [DataMember]
        public Decimal? LeftBound { get; set; }

        /// <summary>
        /// Property denoting Left Bound Operator
        /// </summary>
        [DataMember]
        public SearchOperator LeftBoundOperator { get; set; }

        /// <summary>
        /// Property denoting Right Bound
        /// </summary>
        [DataMember]
        public Decimal? RightBound { get; set; }

        /// <summary>
        /// Property denoting Right Bound Operator
        /// </summary>
        [DataMember]
        public SearchOperator RightBoundOperator { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Determines whether two Object instances are equal
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false</returns>
        public override Boolean Equals(object obj)
        {
            if (base.Equals(obj))
            {
                if (obj is ValueSegment)
                {
                    ValueSegment objectToBeCompared = obj as ValueSegment;

                    return
                        this.LeftBound == objectToBeCompared.LeftBound &&
                        this.LeftBoundOperator == objectToBeCompared.LeftBoundOperator &&
                        this.RightBound == objectToBeCompared.RightBound &&
                        this.RightBoundOperator == objectToBeCompared.RightBoundOperator;
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
                base.GetHashCode()
                ^ this.LeftBound.GetHashCode()
                ^ this.LeftBoundOperator.GetHashCode()
                ^ this.RightBound.GetHashCode()
                ^ this.RightBoundOperator.GetHashCode();
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
                xmlWriter.WriteStartElement(ClassName);

                xmlWriter.WriteAttributeString("Id", Id.ToString(CultureInfo.InvariantCulture));
                xmlWriter.WriteAttributeString("Name", Name);
                xmlWriter.WriteAttributeString("LongName", LongName);
                xmlWriter.WriteAttributeString("LeftBound", LeftBound.ToString());
                xmlWriter.WriteAttributeString("LeftBoundOperator", LeftBoundOperator.ToString());
                xmlWriter.WriteAttributeString("RightBound", RightBound.ToString());
                xmlWriter.WriteAttributeString("RightBoundOperator", RightBoundOperator.ToString());

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
        /// <param name="node">XmlNode for deserialization</param>
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
                    Id = ValueTypeHelper.Int32TryParse(node.Attributes["Id"].Value, Id);
                }
                if (node.Attributes["Name"] != null)
                {
                    Name = node.Attributes["Name"].Value;
                }
                if (node.Attributes["LongName"] != null)
                {
                    LongName = node.Attributes["LongName"].Value;
                }
                if (node.Attributes["LeftBound"] != null)
                {
                    LeftBound = ValueTypeHelper.ConvertToNullableDecimal(node.Attributes["LeftBound"].Value);
                }
                if (node.Attributes["LeftBoundOperator"] != null)
                {
                    SearchOperator tempOperator;
                    ValueTypeHelper.EnumTryParse(node.Attributes["LeftBoundOperator"].Value, true, out tempOperator);
                    LeftBoundOperator = tempOperator;
                }
                if (node.Attributes["RightBound"] != null)
                {
                    RightBound = ValueTypeHelper.ConvertToNullableDecimal(node.Attributes["RightBound"].Value);
                }
                if (node.Attributes["RightBoundOperator"] != null)
                {
                    SearchOperator tempOperator;
                    ValueTypeHelper.EnumTryParse(node.Attributes["RightBoundOperator"].Value, true, out tempOperator);
                    RightBoundOperator = tempOperator;
                }
            }
        }

        /// <summary>
        /// Loads current object from provided Xml
        /// </summary>
        /// <param name="xml">Xml string for deserialization</param>
        public void LoadFromXml(String xml)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            XmlNode node = doc.SelectSingleNode(ClassName);
            if (node != null)
            {
                LoadFromXml(node);
            }
        }

        #endregion

        #endregion
    }
}