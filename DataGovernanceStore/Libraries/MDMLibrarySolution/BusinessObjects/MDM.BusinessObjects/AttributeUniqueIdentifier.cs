using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

using ProtoBuf;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies properties of using which an attribute can be uniquely identified in the system
    /// </summary>
    [DataContract]
    [ProtoContract]
    public class AttributeUniqueIdentifier : ObjectBase, IAttributeUniqueIdentifier
    {
        #region Fields 
        
        /// <summary>
        /// Field denoting attribute name
        /// </summary>
        private String _attributeName = String.Empty;

        /// <summary>
        /// Field denoting attribute group name
        /// </summary>
        private String _attributeGroupName = String.Empty;

        /// <summary>
        /// Field denoting instance ref Id for attribute.
        /// This is used for identifying 1 complex child attribute when attribute is complex collection
        /// </summary>
        private Int32 _instanceRefId = -1;

        #endregion

        #region Constructors 

        /// <summary>
        /// ParameterLess Constructor
        /// </summary>
        public AttributeUniqueIdentifier()
        {

        }

        /// <summary>
        /// Constructor which takes attribute name and attribute group name as input parameters
        /// </summary>
        /// <param name="attributeName">Indicates the name of attribute</param>
        /// <param name="attributeGroupName">Indicates the group name of attribute</param>
        public AttributeUniqueIdentifier( String attributeName, String attributeGroupName )
        {
            this._attributeName = attributeName;
            this._attributeGroupName = attributeGroupName;
            this._instanceRefId = -1;
        }

        /// <summary>
        /// Constructor having attribute name and attribute group name as parameter
        /// </summary>
        /// <param name="attributeName">Attribute name which is used to identify attribute uniquely</param>
        /// <param name="attributeGroupName">Attribute group name which is used to identify attribute uniquely</param>
        /// <param name="instanceRefId">Indicates instance ref if for child record in case attribute is complex collection attribute</param>
        public AttributeUniqueIdentifier(String attributeName, String attributeGroupName, Int32 instanceRefId) 
            : base() 
        {
            this._attributeName = attributeName;
            this._attributeGroupName = attributeGroupName;
            this._instanceRefId = instanceRefId;
        }

        /// <summary>
        /// Initialize AttributeUniqueIdentifier object from Xml
        /// </summary>
        /// <param name="valuesAsXml">Xml having value for AttributeUniqueIdentifier object</param>
        public AttributeUniqueIdentifier( String valuesAsXml )
        {
            LoadAttributeUniqueIdentifier(valuesAsXml);
        }

        
        #endregion

        #region Properties 

        /// <summary>
        /// Property denoting attribute name
        /// </summary>
        [DataMember]
        [ProtoMember(1)]
        public String AttributeName
        {
            get { return _attributeName; }
            set { _attributeName = value; }
        }

        /// <summary>
        /// Property denoting attribute group name
        /// </summary>
        [DataMember]
        [ProtoMember(2)]
        public String AttributeGroupName
        {
            get { return _attributeGroupName; }
            set { _attributeGroupName = value; }
        }

        /// <summary>
        /// Property denoting instance ref Id for attribute.
        /// This is used for identifying 1 complex child attribute when attribute is complex collection
        /// </summary>
        [DataMember]
        [ProtoMember(3)]
        public Int32 InstanceRefId
        {
            get
            {
                return this._instanceRefId;
            }
            set
            {
                this._instanceRefId = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Initialize AttributeUniqueIdentifier object from Xml
        /// </summary>
        /// <param name="valuesAsXml">Xml having value for AttributeUniqueIdentifier object
        /// <para>Sample Xml</para>
        /// <![CDATA[
        /// <AttributeUniqueIdentifier AttributeName="Product Id" AttribtueGroupName = "CoreAttributes" InstanceRefId = "-1"/>
        /// ]]>
        /// </param>
        public void LoadAttributeUniqueIdentifier( string valuesAsXml )
        {
            #region Sample Xml

            /*
            <AttributeUniqueIdentifier AttributeName="Product Id" AttribtueGroupName = "CoreAttributes" InstanceRefId = "-1"/>
             * */
            #endregion Sample Xml

            XmlTextReader reader = null;
            try
            {
                reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                while ( !reader.EOF )
                {
                    if ( reader.NodeType == XmlNodeType.Element && reader.Name == "AttributeUniqueIdentifier" )
                    {
                        #region Read Attribute unique identifier Properties

                        if ( reader.HasAttributes )
                        {
                            if ( reader.MoveToAttribute("AttributeName") )
                            {
                                this.AttributeName = reader.ReadContentAsString();
                            }

                            if ( reader.MoveToAttribute("AttributeGroupName") )
                            {
                                this.AttributeGroupName = reader.ReadContentAsString();
                            }

                            if ( reader.MoveToAttribute("InstanceRefId") )
                            {
                                Int32 refId = -1;
                                Int32.TryParse(reader.ReadContentAsString(), out refId);
                                this.InstanceRefId = refId;
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
                if ( reader != null )
                {
                    reader.Close();
                }
            }
        }

        /// <summary>
        /// Represents AttributeUniqueIdentifier  in Xml format
        /// </summary>
        /// <returns>String representation of current AttributeUniqueIdentifier object</returns>
        public String ToXml()
        {
            String xml = String.Empty;

            String attributeIds = String.Empty;
            String attributeGroupIds = String.Empty;
            String relationshipTypeIds = String.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);
            //xmlWriter.WriteStartDocument();

            //Attribute node start
            xmlWriter.WriteStartElement("AttributeUniqueIdentifier");

            xmlWriter.WriteAttributeString("AttributeName", this.AttributeName);
            xmlWriter.WriteAttributeString("AttributeGroupName", this.AttributeGroupName);
            xmlWriter.WriteAttributeString("InstanceRefId", this.InstanceRefId.ToString());

            //EntityContext end node
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();
            //get the actual XML
            xml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return xml;
        }

        #endregion Methods

    }
}
