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
    /// Specifies MergePlanning Rule
    /// </summary>
    [DataContract]
    public class MergeResult : MDMObject, IMergeResult
    {
        #region Fields

        /// <summary>
        /// Field for MergeResult id
        /// </summary>
        private Int64 _id = 0;

        /// <summary>
        /// Field for MergeJob id
        /// </summary>
        private Int64 _mergeJobId = 0;

        /// <summary>
        /// Field for SourceEntity id
        /// </summary>
        private Int64 _sourceEntityId = 0;

        /// <summary>
        /// Field for TargetEntity id
        /// </summary>
        private Int64? _targetEntityId = null;

        /// <summary>
        /// Field for MergeAction
        /// </summary>
        private MergeAction _mergeAction = MergeAction.Unknown;

        /// <summary>
        /// Field for DateTime of Merge
        /// </summary>
        private DateTime? _mergeDateTime = null;

        /// <summary>
        /// Field for merge result status
        /// </summary>
        private MergeResultStatus _mergeResultStatus = MergeResultStatus.Unknown;

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless contsructor
        /// </summary>
        public MergeResult()
        {

        }

        /// <summary>
        /// Construct MergeResult from xml
        /// </summary>
        /// <param name="xml"></param>
        public MergeResult(String xml)
        {
            this.LoadFromXml(xml);
        } 

        #endregion

        #region Properties
        
        /// <summary>
        /// Property denoting MergeResult id
        /// </summary>
        [DataMember]
        public new Int64 Id
        {
            get { return _id; } 
            set { _id = value; }
        }

        /// <summary>
        /// Property denoting MergeJob id
        /// </summary>
        [DataMember]
        public Int64 MergeJobId
        {
            get { return _mergeJobId; }
            set { _mergeJobId = value; }
        }

        /// <summary>
        /// Property denoting SourceEntity id
        /// </summary>
        [DataMember]
        public Int64 SourceEntityId
        {
            get { return _sourceEntityId; }
            set { _sourceEntityId = value; }
        }

        /// <summary>
        /// Property denoting TargetEntity id
        /// </summary>
        [DataMember]
        public Int64? TargetEntityId
        {
            get { return _targetEntityId; }
            set { _targetEntityId = value; }
        }

        /// <summary>
        /// Property denoting Default MergeAction
        /// </summary>
        [DataMember]
        public MergeAction MergeAction
        {
            get { return _mergeAction; }
            set { _mergeAction = value; }
        }

        /// <summary>
        /// Property denoting DateTime of Merge
        /// </summary>
        [DataMember]
        public DateTime? MergeDateTime
        {
            get { return _mergeDateTime; }
            set { _mergeDateTime = value; }
        }

        /// <summary>
        /// Property denoting merge result status
        /// </summary>
        [DataMember]
        public MergeResultStatus MergeResultStatus 
        {
            get { return _mergeResultStatus; } 
            set { _mergeResultStatus = value; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Get Xml representation of MergeResult
        /// </summary>
        public override String ToXml()
        {
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //MergeResults node start
            xmlWriter.WriteStartElement("MergeResult");

            xmlWriter.WriteAttributeString("Id", Id.ToString(CultureInfo.InvariantCulture));

            xmlWriter.WriteStartElement("MergeJobId");
            xmlWriter.WriteValue(MergeJobId.ToString(CultureInfo.InvariantCulture));
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("SourceEntityId");
            xmlWriter.WriteValue(SourceEntityId.ToString(CultureInfo.InvariantCulture));
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("TargetEntityId");
            xmlWriter.WriteValue(TargetEntityId.HasValue ? TargetEntityId.Value.ToString(CultureInfo.InvariantCulture) : String.Empty);
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("MergeAction");
            xmlWriter.WriteValue(MergeAction.ToString());
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("MergeDateTime");
            xmlWriter.WriteValue(MergeDateTime.HasValue ? MergeDateTime.Value.ToString(CultureInfo.InvariantCulture) : String.Empty);
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("MergeResultStatus");
            xmlWriter.WriteValue(MergeResultStatus.ToString());
            xmlWriter.WriteEndElement();

            //MergeResult node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //Get the actual XML
            String result = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return result;
        }

        /// <summary>
        /// Loads MergeResult from XML node
        /// </summary>
        public void LoadFromXml(XmlNode node)
        {
            this.Clear();
            Int64 defaultInt64 = 0;

            if (node != null && node.Attributes != null)
            {
                if (node.Attributes["Id"] != null)
                {
                    Id = ValueTypeHelper.Int64TryParse(node.Attributes["Id"].Value, defaultInt64);
                }
            }

            if (node != null)
            {
                foreach (XmlNode child in node.ChildNodes)
                {
                    if (child.Name == "MergeJobId")
                    {
                        MergeJobId = ValueTypeHelper.Int64TryParse(child.InnerText, defaultInt64);
                    }
                    else if (child.Name == "SourceEntityId")
                    {
                        SourceEntityId = ValueTypeHelper.Int64TryParse(child.InnerText, defaultInt64);
                    }
                    else if (child.Name == "TargetEntityId")
                    {
                        TargetEntityId = ValueTypeHelper.ConvertToNullableInt64(child.InnerText);
                    }
                    else if (child.Name == "MergeAction")
                    {
                        MergeAction tmpMergeAction;
                        if (Enum.TryParse(child.InnerText, true, out tmpMergeAction))
                        {
                            MergeAction = tmpMergeAction;
                        }
                    }
                    else if (child.Name == "MergeDateTime")
                    {
                        MergeDateTime = ValueTypeHelper.ConvertToNullableDateTime(child.InnerText);
                    }
                    else if (child.Name == "MergeResultStatus")
                    {
                        MergeResultStatus tmpMergeResultStatus;
                        if (Enum.TryParse(child.InnerText, true, out tmpMergeResultStatus))
                        {
                            MergeResultStatus = tmpMergeResultStatus;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Loads MergeResult from XML
        /// </summary>
        public void LoadFromXml(String xml)
        {
            if (!String.IsNullOrWhiteSpace(xml))
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xml);

                XmlNode node = doc.SelectSingleNode("MergeResult");
                if (node != null)
                {
                    LoadFromXml(node);
                }
            }
        }

        void Clear()
        {
            this.MergeAction = MergeAction.Unknown;
            this.MergeDateTime = DateTime.Now;
            this.MergeJobId = 0;
            this.MergeResultStatus = MergeResultStatus.Unknown;
            this.SourceEntityId = 0;
            this.TargetEntityId = 0;
        }

        #endregion

        #region ICloneable Members

        /// <summary>
        /// Clone MergeResult
        /// </summary>
        /// <returns>Cloned MergeResult object</returns>
        public object Clone()
        {
            MergeResult cloned = new MergeResult();
            cloned = (MergeResult) this.MemberwiseClone();
            cloned.Id = this.Id;
            cloned.MergeJobId = this.MergeJobId;
            cloned.SourceEntityId = this.SourceEntityId;
            cloned.TargetEntityId = this.TargetEntityId;
            cloned.MergeAction = this.MergeAction;
            cloned.MergeDateTime = this.MergeDateTime;
            cloned.MergeResultStatus = this.MergeResultStatus;

            return cloned;
        }

        #endregion
    }
}