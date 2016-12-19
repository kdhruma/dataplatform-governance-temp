using System;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.BusinessObjects.Interfaces;

    /// <summary>
    /// Specifies SurvivorshipRuleSourceListItem
    /// </summary>
    [DataContract]
    public class SurvivorshipRuleSourceListItem : ISurvivorshipRuleSourceListItem
    {
        #region Fields
        
        /// <summary>
        /// Field for SourceId
        /// </summary>
        private Int32 _sourceId = 0;

        /// <summary>
        /// Field for SourceAttributeId
        /// </summary>
        private Int64? _sourceAttributeId = null;

        /// <summary>
        /// Field for Priority
        /// </summary>
        private Int32 _priority = 0;

        #endregion

        #region Properties

        /// <summary>
        /// Property denotes SourceId
        /// </summary>
        [DataMember]
        public Int32 SourceId
        {
            get { return _sourceId; }
            set { _sourceId = value; }
        }

        /// <summary>
        /// Property denotes SourceAttributeId
        /// </summary>
        [DataMember]
        public Int64? SourceAttributeId
        {
            get { return _sourceAttributeId; }
            set { _sourceAttributeId = value; }
        }

        /// <summary>
        /// Property denotes Priority
        /// </summary>
        [DataMember]
        public Int32 Priority
        {
            get { return _priority; }
            set { _priority = value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs SurvivorshipRuleSourceListItem
        /// </summary>
        public SurvivorshipRuleSourceListItem()
            : base()
        {
        }

        /// <summary>
        /// Copy constructor. Constructs SurvivorshipRuleSourceListItem using specified instance data
        /// </summary>
        public SurvivorshipRuleSourceListItem(SurvivorshipRuleSourceListItem source)
            : base()
        {
            this.SourceId = source.SourceId;
            this.SourceAttributeId = source.SourceAttributeId;
            this.Priority = source.Priority;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Get Xml representation of MatchingRule
        /// </summary>
        public String ToXml()
        {
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //SurvivorshipRuleSourceListItem node start
            xmlWriter.WriteStartElement("Source");
            
            xmlWriter.WriteAttributeString("SrcId", SourceId.ToString(CultureInfo.InvariantCulture));

            xmlWriter.WriteAttributeString("SrcAttrId", SourceAttributeId == null ? "" : SourceAttributeId.Value.ToString(CultureInfo.InvariantCulture));
            
            xmlWriter.WriteAttributeString("Priority", Priority.ToString(CultureInfo.InvariantCulture));
            
            //SurvivorshipRuleSourceListItem node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //Get the actual XML
            String result = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return result;
        }

        /// <summary>
        /// Loads SurvivorshipRuleSourceListItem from XML node
        /// </summary>
        public void LoadFromXml(XmlNode node)
        {
            SourceId = 0;
            SourceAttributeId = null;
            Priority = 0;
            if (node.Attributes != null)
            {
                if (node.Attributes["SrcId"] != null)
                {
                    Int32 tmp;
                    if (Int32.TryParse(node.Attributes["SrcId"].Value, NumberStyles.Any, CultureInfo.InvariantCulture, out tmp))
                    {
                        SourceId = tmp;
                    }
                }
                if (node.Attributes["SrcAttrId"] != null)
                {
                    Int64 tmp;
                    if (Int64.TryParse(node.Attributes["SrcAttrId"].Value, NumberStyles.Any, CultureInfo.InvariantCulture, out tmp))
                    {
                        SourceAttributeId = tmp;
                    }
                }
                if (node.Attributes["Priority"] != null)
                {
                    Int32 tmp;
                    if (Int32.TryParse(node.Attributes["Priority"].Value, NumberStyles.Any, CultureInfo.InvariantCulture, out tmp))
                    {
                        Priority = tmp;
                    }
                }
            }
        }

        #endregion

        #region ICloneable Members

        /// <summary>
        /// Clone SurvivorshipRuleSourceListItem
        /// </summary>
        /// <returns>Cloned SurvivorshipRuleSourceListItem object</returns>
        public object Clone()
        {
            SurvivorshipRuleSourceListItem cloned = new SurvivorshipRuleSourceListItem(this);
            return cloned;
        }

        #endregion
    }
}
