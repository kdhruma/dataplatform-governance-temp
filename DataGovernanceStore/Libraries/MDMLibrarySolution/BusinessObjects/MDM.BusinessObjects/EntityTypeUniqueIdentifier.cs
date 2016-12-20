using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using System.IO;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies properties which an entity type can be uniquely identified in the system
    /// </summary>
    [DataContract]
    [System.Obsolete("EntityTypeUniqueIdentifier is no longer needed as EntityType short name is enough to identify entityType uniquely.")]
    public class EntityTypeUniqueIdentifier : ObjectBase, IEntityTypeUniqueIdentifier
    {
        #region Fields

        /// <summary>
        /// Field denoting entity type name
        /// </summary>
        private String _entityTypeName = String.Empty;

        /// <summary>
        /// Field denoting parent entity type name
        /// </summary>
        private String _parentEntityTypeName = String.Empty;

        #endregion

        #region Constructors 

        /// <summary>
        /// Constructor having entity type name and parent entity type name as parameter
        /// </summary>
        /// <param name="entityTypeName">Entity type name which is used to identify entity type uniquely</param>
        /// <param name="parentEntityTypeName">Parent entity type name which is used to identify entity type uniquely</param>
        public EntityTypeUniqueIdentifier(String entityTypeName, String parentEntityTypeName )
        {
            this._entityTypeName = entityTypeName;
            this._parentEntityTypeName = parentEntityTypeName;
        }

        /// <summary>
        /// Initialize entityTypeUniqueIdentifier object from Xml
        /// </summary>
        /// <param name="valuesAsXml">Xml having value for entityTypeUniqueIdentifier object</param>
        public EntityTypeUniqueIdentifier(String valuesAsXml)
        {
            LoadEntityTypeUniqueIdentifier(valuesAsXml);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Property denoting entity type name
        /// </summary>
        [DataMember]
        public String EntityTypeName
        {
            get { return _entityTypeName; }
            set { _entityTypeName = value; }
        }

        /// <summary>
        /// Property denoting parent entity type name
        /// </summary>
        [DataMember]
        public String ParentEntityTypeName
        {
            get { return _parentEntityTypeName; }
            set { _parentEntityTypeName = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Initialize entityTypeUniqueIdentifier object from Xml
        /// </summary>
        /// <param name="valuesAsXml">Xml having value for entityTypeUniqueIdentifier object
        /// <para>Sample Xml</para>
        /// <![CDATA[
        /// <EntityTypeUniqueIdentifier EntityTypeName = "color" ParentEntityTypeName = "style"/>
        /// ]]>
        /// </param>
        public void LoadEntityTypeUniqueIdentifier(string valuesAsXml)
        {
            XmlTextReader reader = null;
            try
            {
                reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                while (!reader.EOF)
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "EntityTypeUniqueIdentifier")
                    {
                        #region Read entityType unique identifier Properties

                        if (reader.HasAttributes)
                        {
                            if (reader.MoveToAttribute("EntityTypeName"))
                            {
                                this.EntityTypeName = reader.ReadContentAsString();
                            }

                            if (reader.MoveToAttribute("ParentEntityTypeName"))
                            {
                                this.ParentEntityTypeName = reader.ReadContentAsString();
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

        /// <summary>
        /// Represents entityTypeUniqueIdentifier in Xml format
        /// </summary>
        /// <returns>String representation of current entityTypeUniqueIdentifier object</returns>
        public String ToXml()
        {
            String xml = String.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //Entity type unique identifier node start
            xmlWriter.WriteStartElement("EntityTypeUniqueIdentifier");

            xmlWriter.WriteAttributeString("EntityTypeName", this.EntityTypeName);
            xmlWriter.WriteAttributeString("ParentEntityTypeName", this.ParentEntityTypeName);

            //Entity type unique identifier end node
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
