using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using ProtoBuf;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Represents the class that contain MdmRule attribute context details
    /// </summary>
    [DataContract]
    [ProtoContract(ImplicitFields = ImplicitFields.AllFields)]
    [KnownType(typeof(AttributeModelContext))]
    [KnownType(typeof(MDMRuleContextFilter))]
    public class MDMRuleAttributeModelContext : IMDMRuleAttributeModelContext
    {
        #region Fields

        /// <summary>
        /// Indicates the container Id list
        /// </summary>
        [DataMember]
        private Collection<Int32> _containerIds = null;

        /// <summary>
        /// Indicates the attribute model context
        /// </summary>
        [DataMember]
        private AttributeModelContext _attributeModelContext = null;

        /// <summary>
        /// Indicates the mdmrule context filter.
        /// </summary>
        [DataMember]
        private MDMRuleContextFilter _mdmRuleContextFilter = null;

        /// <summary>
        /// Indicates whether to load full entity family or only for requested Entity
        /// </summary>
        [DataMember]
        private Boolean _loadEntityFamily = true;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Property denotes the Attribute model context
        /// </summary>
        public AttributeModelContext AttributeModelContext
        {
            get
            {
                if (this._attributeModelContext == null)
                {
                    this._attributeModelContext = new AttributeModelContext();
                }
                return this._attributeModelContext;
            }
            set
            {
                this._attributeModelContext = value;
            }
        }

        /// <summary>
        /// Property denotes the MDMRule Context Filter
        /// </summary>
        public MDMRuleContextFilter MDMRuleContextFilter
        {
            get
            {
                if (this._mdmRuleContextFilter == null)
                {
                    this._mdmRuleContextFilter = new MDMRuleContextFilter();
                }

                return this._mdmRuleContextFilter;
            }
            set
            {
                this._mdmRuleContextFilter = value;
            }
        }

        /// <summary>
        /// Property denotes the container Id list
        /// </summary>
        public Collection<Int32> ContainerIdList
        {
            get
            {
                if (this._containerIds == null)
                {
                    this._containerIds = new Collection<Int32>();
                }
                return this._containerIds;
            }
            set
            {
                this._containerIds = value;
            }
        }

        /// <summary>
        /// Property denotes whether to load full entity family or only for requested Entity
        /// </summary>
        [DataMember]
        public Boolean LoadEntityFamily
        {
            get
            {
                return this._loadEntityFamily;
            }
            set
            {
                this._loadEntityFamily = value;
            }
        }

        #endregion Properties

        #region Constructor

        /// <summary>
        /// Constructor with parameter less constructor
        /// </summary>
        public MDMRuleAttributeModelContext()
        { 
        
        }

        /// <summary>
        /// Constructor with MDMRuleAttributeModelContext object as xml format
        /// </summary>
        /// <param name="valueAsXml"></param>
        public MDMRuleAttributeModelContext(String valueAsXml)
        {
            LoadMDMRuleAttributeModelContextFromXml(valueAsXml);
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Gets Xml representation of MDMRuleAttributeModelContext Object
        /// </summary>
        public String ToXml()
        {
            String outputXml = String.Empty;

            using (StringWriter sw = new StringWriter())
            {
                using (XmlTextWriter xmlWriter = new XmlTextWriter(sw))
                {
                    xmlWriter.WriteStartElement("MDMRuleAttributeModelContext");

                    xmlWriter.WriteAttributeString("ContainerIds", ValueTypeHelper.JoinCollection<Int32>(this.ContainerIdList, ","));
                    xmlWriter.WriteAttributeString("LoadEntityFamily", this.LoadEntityFamily.ToString());

                    if (this.MDMRuleContextFilter != null)
                    {
                        xmlWriter.WriteRaw(this.MDMRuleContextFilter.ToXml());
                    }

                    if (this.AttributeModelContext != null)
                    {
                        xmlWriter.WriteRaw(this.AttributeModelContext.ToXml());
                    }

                    xmlWriter.WriteEndElement();
                }

                //Get the output XML
                outputXml = sw.ToString();
            }

            return outputXml;
        }

        /// <summary>
        /// Initialize current object with Xml
        /// </summary>
        /// <param name="valuesAsXml">Xml having values for current MDMRuleAttributeModelContext
        /// </param>
        public void LoadMDMRuleAttributeModelContextFromXml(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                using (XmlTextReader reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null))
                {
                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "MDMRuleAttributeModelContext")
                        {
                            if (reader.HasAttributes)
                            {

                                if (reader.MoveToAttribute("ContainerIds"))
                                {
                                    this.ContainerIdList = ValueTypeHelper.SplitStringToIntCollection(reader.ReadContentAsString(), ',');
                                }

                                if (reader.MoveToAttribute("LoadEntityFamily"))
                                {
                                    this.LoadEntityFamily = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), this._loadEntityFamily);
                                }

                                reader.Read();
                            }
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "AttributeModelContext")
                        {
                            String attributeModelContextXml = reader.ReadOuterXml();

                            if (!String.IsNullOrEmpty(attributeModelContextXml))
                            {
                                this.AttributeModelContext = new AttributeModelContext(attributeModelContextXml);
                            }
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "MDMRuleContextFilter")
                        {
                            String mdmRuleContextFilterXml = reader.ReadOuterXml();

                            if (!String.IsNullOrEmpty(mdmRuleContextFilterXml))
                            {
                                this.MDMRuleContextFilter = new MDMRuleContextFilter(mdmRuleContextFilterXml);
                            }
                        }
                        else
                        {
                            reader.Read();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Set the AttributeModel Context 
        /// </summary>
        /// <param name="modelContext">Indicates the IAttributeModelContext</param>
        public void SetAttributeModelContext(IAttributeModelContext modelContext)
        {
            if (modelContext != null)
            {
                this.AttributeModelContext = (AttributeModelContext)modelContext;
            }
        }

        /// <summary>
        /// Set the MDMRuleContextFilter
        /// </summary>
        /// <param name="mdmRuleContextFilter">Indicates the MDMRuleContextFilter</param>
        public void SetMDMRuleContextFilter(IMDMRuleContextFilter mdmRuleContextFilter)
        {
            if (mdmRuleContextFilter != null)
            {
                this.MDMRuleContextFilter = (MDMRuleContextFilter)mdmRuleContextFilter;
            }
        }

        #endregion Methods

    }
}
