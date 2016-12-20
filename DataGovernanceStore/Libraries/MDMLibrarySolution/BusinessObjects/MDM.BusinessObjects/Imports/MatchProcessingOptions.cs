using System;
using System.IO;
using System.Xml;
using System.Runtime.Serialization;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    
    /// <summary>
    /// Specifies MatchProcessingOptions which specifies various flags and indications to entity processing logic
    /// </summary>
    [DataContract]
    public class MatchProcessingOptions : ObjectBase
    {
        #region Fields

        /// <summary>
        /// Indicates the number of entity threads/tasks that will be used for the import processing.
        /// </summary>
        private Int32 _matchProfileGroupId = -1;

        /// <summary>
        /// Indicates the MatchProfileGroupName.
        /// </summary>
        private String _matchProfileGroupName;

        /// <summary>
        /// Indicates if the match needs to be executed as a job.
        /// </summary>
        private Boolean _runMatchasJob = false;

        /// <summary>
        /// Indicates if the match needs to be executed only for new entities.
        /// </summary>
        private Boolean _matchOnlyNewEntities = false;

        /// <summary>
        /// Property denoting the matching job id
        /// </summary>
        private Int64 _matchJobId = -1;
        
        #endregion Fields

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public MatchProcessingOptions() { }

        /// <summary>
        /// Constructor which takes valuesAsXml as input parameter
        /// </summary>
        /// <param name="valuesAsXml">Xml formatted values with which object will be initalized.</param>
        public MatchProcessingOptions(String valuesAsXml)
        {
            LoadMatchProcessingOptions(valuesAsXml);
        }

        #endregion Constructor

        #region Properties

        /// <summary>
        /// Indicates the match profile group id.
        /// </summary>
        [DataMember]
        public Int32 MatchProfileGroupId
        {
            get { return _matchProfileGroupId; }
            set { _matchProfileGroupId = value; }
        }

        /// <summary>
        /// Indicates the match profile group Name.
        /// </summary>
        [DataMember]
        public String MatchProfileGroupName
        {
            get { return _matchProfileGroupName; }
            set { _matchProfileGroupName = value; }
        }

        /// <summary>
        /// Runs the matching as a job
        /// </summary>
        [DataMember]
        public Boolean RunMatchAsJob
        {
            get { return _runMatchasJob; }
            set { _runMatchasJob = value; }
        }

        /// <summary>
        /// Match only when the entity is being created.
        /// </summary>
        [DataMember]
        public Boolean MatchOnlyNewEntities
        {
            get { return _matchOnlyNewEntities; }
            set { _matchOnlyNewEntities = value; }
        }

        /// <summary>
        /// Indicates the match Job id.
        /// </summary>
        [DataMember]
        public Int64 MatchJobId
        {
            get { return _matchJobId; }
            set { _matchJobId = value; }
        }
        #endregion Properties

        #region Methods
         
        #region Public methods

        /// <summary>
        /// Represents MatchProcessingOptions in Xml format
        /// </summary>
        /// <returns>String representation of current MatchProcessingOptions object instance</returns>
        public String ToXml()
        {
            String xml = String.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //Attribute node start
            xmlWriter.WriteStartElement("MatchProcessingOptions");

            xmlWriter.WriteAttributeString("MatchProfileGroupId", this.MatchProfileGroupId.ToString());
            xmlWriter.WriteAttributeString("MatchProfileGroupName", MatchProfileGroupName);
            xmlWriter.WriteAttributeString("RunMatchAsJob", this.RunMatchAsJob.ToString());
            xmlWriter.WriteAttributeString("MatchOnlyNewEntities", this.MatchOnlyNewEntities.ToString());
            xmlWriter.WriteAttributeString("MatchJobId", this.MatchJobId.ToString());

            //MatchProcessingOptions end node
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();
            //get the actual XML
            xml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return xml;
        }

        /// <summary>
        /// Represents MatchProcessingOptions in Xml format
        /// </summary>
        /// <returns>String representation of current MatchProcessingOptions object instance</returns>
        public String ToXml(ObjectSerialization serialization)
        {
            String xml = String.Empty;

            if (serialization == ObjectSerialization.Full)
            {
                xml = this.ToXml();
            }
            else
            {
                StringWriter sw = new StringWriter();
                XmlTextWriter xmlWriter = new XmlTextWriter(sw);

                //Attribute node start
                xmlWriter.WriteStartElement("MatchProcessingOptions");

                if (serialization == ObjectSerialization.ProcessingOnly)
                {
                    xmlWriter.WriteAttributeString("MatchProfileGroupId", this.MatchProfileGroupId.ToString());
                    xmlWriter.WriteAttributeString("RunMatchAsJob", this.RunMatchAsJob.ToString());
                    xmlWriter.WriteAttributeString("MatchOnlyNewEntities", this.MatchOnlyNewEntities.ToString());
                    xmlWriter.WriteAttributeString("MatchJobId", this.MatchJobId.ToString());
                }

                if (serialization == ObjectSerialization.UIRender)
                {
                    xmlWriter.WriteAttributeString("MatchProfileGroupId", this.MatchProfileGroupId.ToString());
                    xmlWriter.WriteAttributeString("MatchProfileGroupName", MatchProfileGroupName);
                    xmlWriter.WriteAttributeString("RunMatchAsJob", this.RunMatchAsJob.ToString());
                    xmlWriter.WriteAttributeString("MatchOnlyNewEntities", this.MatchOnlyNewEntities.ToString());
                    xmlWriter.WriteAttributeString("MatchJobId", this.MatchJobId.ToString());
                }

                //MatchProcessingOptions end node
                xmlWriter.WriteEndElement();

                xmlWriter.Flush();
                //get the actual XML
                xml = sw.ToString();

                xmlWriter.Close();
                sw.Close();
            }
            return xml;
        }

        #endregion Public methods

        #region Private methods

        private void LoadMatchProcessingOptions(String valuesAsXml)
        { 
            #region Sample Xml
            //<MatchProcessingOptions ValidateEntities="true" PublishEvents="true" ProcessOnlyEntities="false" ProcessDefaultValues="true" CollectionProcessingType="Replace" AttributeValidationLevel="Warn" RelationshipTypeValidationLevel="Warn" RelationshipAttributeValidationLevel="Warn" PopulateLookupRefIdForInitialLoad = "true"/>
            #endregion Sample Xml

            XmlTextReader reader = null;
            try
            {
                reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                while (!reader.EOF)
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "MatchProcessingOptions")
                    {
                        #region Read MatchProcessingOptions Properties

                        if (reader.HasAttributes)
                        {
                            if (reader.MoveToAttribute("MatchProfileGroupId"))
                            {
                                this.MatchProfileGroupId = Convert.ToInt32(reader.ReadContentAsString());
                            }

                            if (reader.MoveToAttribute("MatchProfileGroupName"))
                            {
                                this.MatchProfileGroupName = reader.ReadContentAsString();
                            }

                            if (reader.MoveToAttribute("RunMatchAsJob"))
                            {
                                this.RunMatchAsJob = Convert.ToBoolean(reader.ReadContentAsString());
                            }

                            if (reader.MoveToAttribute("MatchOnlyNewEntities"))
                            {
                                this.MatchOnlyNewEntities = Convert.ToBoolean(reader.ReadContentAsString());
                            }

                            if (reader.MoveToAttribute("MatchJobId"))
                            {
                                this.MatchJobId = Convert.ToInt64(reader.ReadContentAsString());
                            }
                        }

                        #endregion
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

        #endregion Private methods

        #endregion Methods

    }
}
