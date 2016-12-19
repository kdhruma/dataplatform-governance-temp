using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Interfaces;
    using MDM.Core;

    /// <summary>
    /// Specifies Source
    /// </summary>
    [DataContract]
    public class Source : MDMObject, ISource
    {
        #region Fields

        /// <summary>
        /// Field for Description
        /// </summary>
        private String _description = System.String.Empty;

        /// <summary>
        /// Field denoting whether source attribute is system
        /// </summary>
        private Boolean _isInternal = false;

        #endregion

        #region Constuctors

        /// <summary>
        /// Constructs Source
        /// </summary>
        public Source()
            : base()
        {
        }

        /// <summary>
        /// Constructs Source using specified instance data
        /// </summary>
        public Source(Source source)
            : base(source.Id, source.Name, source.LongName, source.Locale, source.AuditRefId, source.ProgramName)
        {
            this.Description = source.Description;
            this.IsInternal = source.IsInternal;
        }

        /// <summary>
        /// Counstructor desirialize source from Xml
        /// </summary>
        /// <param name="valueAsXml"></param>
        public Source(String valueAsXml)
        {
            XmlTextReader reader = null;
            try
            {
                reader = new XmlTextReader(valueAsXml, XmlNodeType.Element, null);

                while (!reader.EOF)
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "Source")
                    {
                        if (reader.HasAttributes)
                        {
                            if (reader.MoveToAttribute("Id"))
                            {
                                this.Id = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), this.Id);
                            }

                            if (reader.MoveToAttribute("Name"))
                            {
                                this.Name = reader.ReadContentAsString();
                            }

                            if (reader.MoveToAttribute("LongName"))
                            {
                                this.LongName = reader.ReadContentAsString();
                            }

                            if (reader.MoveToAttribute("Description"))
                            {
                                this.Description = reader.ReadContentAsString();
                            }

                            if (reader.MoveToAttribute("IsInternal"))
                            {
                                this.IsInternal = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), false);
                            }
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

        /// <summary>
        /// Initialize only Id property
        /// </summary>
        /// <param name="sourceId">Id of the source</param>
        public Source(Int32 sourceId)
        {
            this.Id = sourceId;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Property denoting Description
        /// </summary>
        [DataMember]
        public String Description
        {
            get { return _description; }
            set { _description = value; }
        }

        /// <summary>
        /// Property denoting IsSystem
        /// </summary>
        [DataMember]
        public Boolean IsInternal
        {
            get { return _isInternal; }
            set { _isInternal = value; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Serves as a hash function for type
        /// </summary>
        /// <returns>A hash code for the current Object</returns>
        public override Int32 GetHashCode()
        {
            return
                base.GetHashCode()
                ^ this.Description.GetHashCode()
                ^ this.IsInternal.GetHashCode();
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
                if (obj is Source)
                {
                    Source objectToBeCompared = obj as Source;
                    return
                        this.Description == objectToBeCompared.Description &&
                        this.IsInternal == objectToBeCompared.IsInternal;

                }
            }
            return false;
        }

        /// <summary>
        /// Get Xml representation of Source object
        /// </summary>
        /// <returns>Xml representation of Source object</returns>
        public override String ToXml()
        {
            String sourceXml = String.Empty;
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            xmlWriter.WriteStartElement("Source");

            xmlWriter.WriteAttributeString("Id", this.Id.ToString());
            xmlWriter.WriteAttributeString("Name", this.Name);
            xmlWriter.WriteAttributeString("LongName", this.LongName);
            xmlWriter.WriteAttributeString("Description", this.Description);
            xmlWriter.WriteAttributeString("IsInternal", this.IsInternal.ToString());

            //Source node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //get the actual XML
            sourceXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return sourceXml;
        }

        /// <summary>
        /// Set source id attribute to target node
        /// </summary>
        /// <param name="xmlWriter">Xml writer responsible for serializing target object</param>
        public void SetSourceIdAttribute(XmlWriter xmlWriter)
        {
            xmlWriter.WriteAttributeString("SourceId", this.Id.ToString());
        }

        #endregion

        #region ICloneable Members

        /// <summary>
        /// Clone source
        /// </summary>
        /// <returns>Cloned source object</returns>
        public object Clone()
        {
            Source clonedSource = new Source(this);
            return clonedSource;
        }

        #endregion
    }
}
