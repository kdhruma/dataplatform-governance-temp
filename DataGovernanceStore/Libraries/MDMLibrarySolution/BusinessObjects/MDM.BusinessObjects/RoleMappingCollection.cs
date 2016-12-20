using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies collection of role mapping
    /// </summary>
    [DataContract]
    public class RoleMappingCollection : InterfaceContractCollection<IRoleMapping, RoleMapping>, IRoleMappingCollection
    {
        #region Constructors

		/// <summary>
		/// Parameterless constructor
		/// </summary>
		public RoleMappingCollection() { }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        /// <param name="valueAsXml">Xml representation of RoleMappingCollection object</param>        
        public RoleMappingCollection(String valueAsXml)
        {
            LoadRoleMappingCollection(valueAsXml);
        }

		/// <summary>
        /// Initialize RoleMappingCollection from IList
		/// </summary>
        /// <param name="roleMappingList">IList of roleMapping</param>
        public RoleMappingCollection(IList<RoleMapping> roleMappingList)
		{
            if (roleMappingList != null)
            {
                this._items = new Collection<RoleMapping>(roleMappingList);
            }
		}

        #endregion Constructors

        #region Properties

        #endregion Properties

        #region Public Methods

        /// <summary>
        /// Get Xml representation of RoleMappingCollection object
        /// </summary>
        /// <returns>Xml string representing the RoleMappingCollection</returns>
        public string ToXml()
        {
            String roleMappingsXml = String.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //Parameter node start
            xmlWriter.WriteStartElement("RoleMappings");

            if (this._items != null && this._items.Count > 0)
            {
                foreach (RoleMapping item in this._items)
                {
                    xmlWriter.WriteRaw(item.ToXml());
                }
            }

            //Param node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //Get the actual XML
            roleMappingsXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return roleMappingsXml;
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Loads the RoleMappingCollection from given XMl 
        /// </summary>
        /// <param name="valuesAsXml">Xml representation of RoleMapping Collection</param>        
        private void LoadRoleMappingCollection(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "ExternalRole")
                        {
                            #region Read RoleMapping Collection

                            String roleMappingXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(roleMappingXml))
                            {
                                RoleMapping roleMapping = new RoleMapping(roleMappingXml);
                                if (roleMapping != null)
                                {
                                    this.Add(roleMapping);
                                }
                            }

                            #endregion Read RoleMapping Collection
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
        }

        #endregion Private Methods
    }
}
