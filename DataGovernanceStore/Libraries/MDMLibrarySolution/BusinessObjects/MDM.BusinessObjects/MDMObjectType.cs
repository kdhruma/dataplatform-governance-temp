using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
namespace MDM.BusinessObjects
{
    using MDM.Interfaces;
    using MDM.Core;

    /// <summary>
    /// Represents MDMObjectType. For example, Entity, EntityCollection
    /// </summary>
    [DataContract]
    public class MDMObjectType : ObjectBase, IMDMObjectType 
    {
        #region Fields

        /// <summary>
        /// Indicates Id of MDMObjectType
        /// </summary>
        private Int16 _Id = -1;

        /// <summary>
        /// Indicates name of MDMObjectType. E.g., Entity, Relationship
        /// </summary>
        private String _name = String.Empty;

        /// <summary>
        /// Indicates fully qualified class name of MDMObject
        /// </summary>
        private String _className = String.Empty;

        /// <summary>
        /// Indicates Assembly in which specified object is contained.
        /// </summary>
        private String _assemblyName = String.Empty;

        #endregion Fields

        #region Constructor


        /// <summary>
        /// Parameter less constructor
        /// </summary>
        public MDMObjectType()
        {
        }

        /// <summary>
        /// Constructor which takes xml as input
        /// </summary>
        /// <param name="valuesAsXml">Values in xml format</param>
        public MDMObjectType(String valuesAsXml)
        {
            LoadMDMObjectType(valuesAsXml);
        }

        #endregion Constructor

        #region Properties

        /// <summary>
        /// Indicates Id of MDMObjectType
        /// </summary>
        [DataMember]
        public Int16 Id
        {
            get { return _Id; }
            set { _Id = value; }
        }

        /// <summary>
        /// Indicates name of MDMObjectType. E.g., Entity, Relationship
        /// </summary>
        [DataMember]
        public String Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// Indicates fully qualified class name of MDMObject
        /// </summary>
        [DataMember]
        public String ClassName
        {
            get { return _className; }
            set { _className = value; }
        }
        
        /// <summary>
        /// Indicates Assembly in which specified object is contained.
        /// </summary>
        [DataMember]
        public String AssemblyName
        {
            get { return _assemblyName; }
            set { _assemblyName = value; }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Get xml version of MDMObjectType
        /// </summary>
        /// <returns>Xml representation of <see cref="MDMObjectType"/></returns>
        public String ToXml()
        {
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);
            String xml = String.Empty;

            xmlWriter.WriteStartElement("MDMObjectType");

            xmlWriter.WriteAttributeString("Id", this.Id.ToString());
            xmlWriter.WriteAttributeString("Name", this.Name);
            xmlWriter.WriteAttributeString("ClassName", this.ClassName);
            xmlWriter.WriteAttributeString("AssemblyName", this.AssemblyName);

            //MDMObjectType end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //get the actual XML
            xml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return xml;
        }

        /// <summary>
        /// Create a new instance of MDMObjectType with same value as current one.
        /// </summary>
        /// <returns><see cref="IMDMObjectType"/></returns>
        public IMDMObjectType Clone()
        {
            MDMObjectType type = new MDMObjectType();
            type.Id = this.Id;
            type.Name = this.Name;
            type.AssemblyName = this.AssemblyName;
            type.ClassName = this.ClassName;

            return type;
        }

        #endregion Methods

        #region Private Methods

        private void LoadMDMObjectType(string valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);
                    while (reader.Read())
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "MDMObjectType" && reader.IsStartElement())
                        {
                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("Id"))
                                    this.Id = ValueTypeHelper.Int16TryParse(reader.ReadContentAsString(), -1);
                                if (reader.MoveToAttribute("Name"))
                                    this.Name = reader.ReadContentAsString();
                                if (reader.MoveToAttribute("ClassName"))
                                    this.ClassName = reader.ReadContentAsString();
                                if (reader.MoveToAttribute("AssemblyName"))
                                    this.AssemblyName = reader.ReadContentAsString();
                            }
                        }
                    }
                }
                finally
                {
                    reader.Close();
                }
            }
        }

        #endregion Private Methods
    }
}
