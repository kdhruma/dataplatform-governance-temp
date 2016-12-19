using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

using ProtoBuf;

namespace MDM.BusinessObjects
{
    using MDM.BusinessObjects.Interfaces;
    using MDM.Core;
    /// <summary>
    /// Specifies Source Info
    /// </summary>
    [DataContract]
    [ProtoContract]
    public class SourceInfo : ISourceInfo
    {
        #region Fields

        /// <summary>
        /// Field for Source Id
        /// </summary>
        private Int32? _sourceId = null;


        /// <summary>
        /// Field for Source Entity Id
        /// </summary>
        private Int64? _sourceEntityId = null;

        #endregion

        #region Constuctors

        /// <summary>
        /// Constructs SourceInfo
        /// </summary>
        public SourceInfo()
        {
        }

        /// <summary>
        /// Constructs SourceInfo
        /// </summary>
        public SourceInfo(Int32? sourceId, Int64? sourceEntityId)
        {
            this.SourceId = sourceId;
            this.SourceEntityId = sourceEntityId;
        }

        /// <summary>
        /// Constructs SourceInfo
        /// </summary>
        public SourceInfo(Int32? sourceId)
        {
            this.SourceId = sourceId;
        }

        /// <summary>
        /// Constructs SourceInfo
        /// </summary>
        public SourceInfo(Int64? sourceEntityId)
        {
            this.SourceEntityId = sourceEntityId;
        }

        /// <summary>
        /// Constructs Source using specified instance data
        /// </summary>
        public SourceInfo(SourceInfo sourceInfo)
        {
            this.SourceId = sourceInfo.SourceId;
            this.SourceEntityId = sourceInfo.SourceEntityId;
        }

        /// <summary>
        /// Counstructor desirialize source from Xml
        /// </summary>
        /// <param name="valueAsXml"></param>
        public SourceInfo(String valueAsXml)
        {
            XmlTextReader reader = null;
            try
            {
                reader = new XmlTextReader(valueAsXml, XmlNodeType.Element, null);

                while (!reader.EOF)
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "SourceInfo")
                    {
                        if (reader.HasAttributes)
                        {
                            if (reader.MoveToAttribute("SourceId"))
                            {
                                this.SourceId = ValueTypeHelper.ConvertToNullableInt32(reader.ReadContentAsString());
                            }

                            if (reader.MoveToAttribute("SourceEntityId"))
                            {
                                this.SourceEntityId = ValueTypeHelper.ConvertToNullableInt64(reader.ReadContentAsString());
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

        #endregion

        #region Properties

        /// <summary>
        /// Property denoting SourceId
        /// </summary>
        [DataMember]
        [ProtoMember(1)]
        public Int32? SourceId
        {
            get { return _sourceId; }
            set { _sourceId = value; }
        }

        /// <summary>
        /// Property denoting SourceEntityId
        /// </summary>
        [DataMember]
        [ProtoMember(2)]
        public Int64? SourceEntityId
        {
            get { return _sourceEntityId; }
            set { _sourceEntityId = value; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns if sources of sourceInfos are equals
        /// </summary>
        /// <param name="sourceInfo"></param>
        /// <returns></returns>
        public Boolean IsSourceEquals(SourceInfo sourceInfo)
        {
            if (sourceInfo != null)
            {
                return this.SourceId == sourceInfo.SourceId;
            }
            return false;
        }

        /// <summary>
        /// Serves as a hash function for type
        /// </summary>
        /// <returns>A hash code for the current Object</returns>
        public new Int32 GetHashCode()
        {
            return
                this.SourceId.GetHashCode()
                ^ this.SourceEntityId.GetHashCode();
        }

        /// <summary>
        /// Determines whether two Object instances are equal
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false</returns>
        public new Boolean Equals(object obj)
        {
            if (obj != null)
            {
                SourceInfo objectToBeCompared = obj as SourceInfo;
                if (objectToBeCompared != null)
                {
                    return
                        this.SourceId == objectToBeCompared.SourceId &&
                        this.SourceEntityId == objectToBeCompared.SourceEntityId;
                }
            }

            return false;
        }

        /// <summary>
        /// Get xml representation of source info object
        /// </summary>
        /// <returns>Returns xml representation of source info object</returns>
        public String ToXml()
        {
            String sourceXml = String.Empty;
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            xmlWriter.WriteStartElement("Source");

            xmlWriter.WriteAttributeString("SourceId", this.SourceId.ToString());
            xmlWriter.WriteAttributeString("SourceEntityId", this.SourceEntityId.ToString());

            //Source node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //get the actual XML
            sourceXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return sourceXml;
        }

        #endregion

        #region ICloneable Members

        /// <summary>
        /// Clone source
        /// </summary>
        /// <returns>Cloned source info object</returns>
        public object Clone()
        {
            SourceInfo clonedSourceInfo = new SourceInfo(this);
            return clonedSourceInfo;
        }

        #endregion
    }
}
