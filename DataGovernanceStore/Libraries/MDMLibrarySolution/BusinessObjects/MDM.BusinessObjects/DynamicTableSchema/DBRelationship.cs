using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects.DynamicTableSchema
{
    using MDM.Core;

    /// <summary>
    /// Specifies the DBRelationship of Dynamic Schema
    /// </summary>
    [DataContract]

    public class DBRelationship: MDMObject
    {
        #region Fields

        /// <summary>
        /// field denoting Old Name of DBRelationship
        /// </summary>
        [DataMember]
        private String _columnName = String.Empty;

        /// <summary>
        /// field denoting Data Type of DBRelationship
        /// </summary>
        [DataMember]
        private String _refTableName = String.Empty;

        /// <summary>
        /// field denoting Length of DBRelationship
        /// </summary>
        [DataMember]
        private String _refColumnName = String.Empty;

        /// <summary>
        /// field denoting Precision of DBRelationship
        /// </summary>
        [DataMember]
        private String _refMask = String.Empty;

        /// <summary>
        /// field denoting default Value of DBRelationship
        /// </summary>
        [DataMember]
        private String _displayColumns = String.Empty;

        /// <summary>
        /// field denoting Sequence of DBRelationship
        /// </summary>
        [DataMember]
        private String _searchColumns = String.Empty;

        /// <summary>
        /// field denoting Nullable of DBRelationship
        /// </summary>
        [DataMember]
        private String _sortColumns = String.Empty;

        /// <summary>
        /// field denoting IsUnique of DBRelationship
        /// </summary>
        [DataMember]
        private ObjectAction _action = ObjectAction.Unknown;

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public DBRelationship()
            : base()
        {
        }

        /// <summary>
        /// Constructor with XML having values of object. Populate current object using XML
        /// </summary>
        /// <param name="valuesAsXml">XML having xml value</param>
        /// <example>
        ///     Sample XML:
        ///     <para>
        ///        &lt;Relationship
        ///        ColumnName = "Value"
        ///        RefTableName = "tblk_UOM" 
        ///        RefColumnName="" 
        ///        RefMask = ""
        ///        DisplayColumns = "Name"
        ///        SearchColumns = "" 
        ///        SortColumns = "" 
        ///        Action="Create"
        ///        &gt;
        /// </para>
        /// </example>
        public DBRelationship(String valuesAsXml)
        {
            LoadDBRelationship(valuesAsXml);
        }

        #endregion

        #region Properties

        /// <summary>
        ///  Property denoting the ColumnName of the DBRelationship
        /// </summary>
        [DataMember]
        public String ColumnName
        {
            get
            {
                return this._columnName;
            }
            set
            {
                this._columnName = value;
            }
        }

        /// <summary>
        ///  Property denoting the RefTableName of the DBRelationship
        /// </summary>
        [DataMember]
        public String RefTableName
        {
            get
            {
                return this._refTableName;
            }
            set
            {
                this._refTableName = value;
            }
        }

        /// <summary>
        ///  Property denoting the RefColumnname of the DBRelationship
        /// </summary>
        [DataMember]
        public String RefColumnname
        {
            get
            {
                return this._refColumnName;
            }
            set
            {
                this._refColumnName = value;
            }
        }

        /// <summary>
        ///  Property denoting the RefMask of the DBRelationship
        /// </summary>
        [DataMember]
        public String RefMask
        {
            get
            {
                return this._refMask;
            }
            set
            {
                this._refMask = value;
            }
        }

        /// <summary>
        ///  Property denoting the DisplayColumns of the DBRelationship
        /// </summary>
        [DataMember]
        public String DisplayColumns
        {
            get
            {
                return this._displayColumns;
            }
            set
            {
                this._displayColumns = value;
            }
        }

        /// <summary>
        ///  Property denoting the SearchColumns of the DBRelationship
        /// </summary>
        [DataMember]
        public String SearchColumns
        {
            get
            {
                return this._searchColumns;
            }
            set
            {
                this._searchColumns = value;
            }
        }

        /// <summary>
        ///  Property denoting the SortColumns of the DBRelationship
        /// </summary>
        [DataMember]
        public String SortColumns
        {
            get
            {
                return this._sortColumns;
            }
            set
            {
                this._sortColumns = value;
            }
        }

        /// <summary>
        ///  Property denoting the Action of the DBRelationship
        /// </summary>
        [DataMember]
        public override ObjectAction Action
        {
            get
            {
                return this._action;
            }
            set
            {
                this._action = value;
            }
        }

        #endregion

        #region Methods

        #region Load Methods

        /// <summary>
        /// Load DBColumn object from XML.
        /// </summary>
        /// <param name="valuesAsXml">XML having xml value</param>
        public void LoadDBRelationship(String valuesAsXml)
        {

            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "Relationship")
                        {
                            #region Read DBColumn Properties

                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("ColumnName"))
                                {
                                    this.ColumnName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("RefTableName"))
                                {
                                    this.RefTableName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("RefColumnName"))
                                {
                                    this.RefColumnname = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("RefMask"))
                                {
                                    this.RefMask = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("DisplayColumns"))
                                {
                                    this.DisplayColumns = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("SearchColumns"))
                                {
                                    this.SearchColumns = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("SortColumns"))
                                {
                                    this.SortColumns = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("Action"))
                                {
                                    ObjectAction objectAction = ObjectAction.Unknown;
                                    Enum.TryParse(reader.ReadContentAsString(), out objectAction);
                                    this.Action = objectAction;
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
        }

        #endregion

        #region ToXml methods

        /// <summary>
        /// Get Xml representation of DBColumn
        /// </summary>
        /// <returns>Xml representation of object</returns>
        public override String ToXml()
        {
            String DBColumnXml = String.Empty;
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //DBColumn node start
            xmlWriter.WriteStartElement("Relationship");

            #region Write DBColumn Properties

            xmlWriter.WriteAttributeString("ColumnName", this.ColumnName.ToString());
            xmlWriter.WriteAttributeString("RefTableName", this.RefTableName);
            xmlWriter.WriteAttributeString("RefColumnName", this.RefColumnname);
            xmlWriter.WriteAttributeString("RefMask", this.RefMask);
            xmlWriter.WriteAttributeString("DisplayColumns", this.DisplayColumns);
            xmlWriter.WriteAttributeString("SearchColumns", this.SearchColumns);
            xmlWriter.WriteAttributeString("SortColumns", this.SortColumns);
            xmlWriter.WriteAttributeString("Action", this.Action.ToString());

            #endregion

            //DBColumn node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //get the actual XML
            DBColumnXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return DBColumnXml;
        }

        /// <summary>
        /// Get Xml representation of DBColumn
        /// </summary>
        /// <returns>Xml representation of object</returns>
        public override String ToXml(ObjectSerialization serialization)
        {
            String DBRelationshipXml = String.Empty;

            if (serialization == ObjectSerialization.Full)
            {
                DBRelationshipXml = this.ToXml();
            }
            else
            {
                StringWriter sw = new StringWriter();
                XmlTextWriter xmlWriter = new XmlTextWriter(sw);

                //DBColumn node start
                xmlWriter.WriteStartElement("Relationship");

                if (serialization == ObjectSerialization.ProcessingOnly)
                {
                    #region Write DBColumn Properties

                    xmlWriter.WriteAttributeString("ColumnName", this.ColumnName);
                    xmlWriter.WriteAttributeString("RefTableName", this.RefTableName);
                    xmlWriter.WriteAttributeString("RefColumnName", this.RefColumnname);
                    xmlWriter.WriteAttributeString("RefMask", this.RefMask);
                    xmlWriter.WriteAttributeString("DisplayColumns", this.DisplayColumns);
                    xmlWriter.WriteAttributeString("SearchColumns", this.SearchColumns);
                    xmlWriter.WriteAttributeString("SortColumns", this.SortColumns);
                    xmlWriter.WriteAttributeString("Action", this.Action.ToString());

                    #endregion
                }
                else if (serialization == ObjectSerialization.UIRender)
                {
                    #region Write DBRelationship Properties

                    xmlWriter.WriteAttributeString("ColumnName", this.ColumnName);
                    xmlWriter.WriteAttributeString("RefTableName", this.RefTableName);
                    xmlWriter.WriteAttributeString("RefColumnName", this.RefColumnname);
                    xmlWriter.WriteAttributeString("RefMask", this.RefMask);
                    xmlWriter.WriteAttributeString("DisplayColumns", this.DisplayColumns);
                    xmlWriter.WriteAttributeString("SearchColumns", this.SearchColumns);
                    xmlWriter.WriteAttributeString("SortColumns", this.SortColumns);
                    xmlWriter.WriteAttributeString("Action", this.Action.ToString());

                    #endregion
                }

                //DBColumn node end
                xmlWriter.WriteEndElement();

                xmlWriter.Flush();

                //get the actual XML
                DBRelationshipXml = sw.ToString();

                xmlWriter.Close();
                sw.Close();
            }
            return DBRelationshipXml;
        }

        #endregion

        #region Public Methods



        /// <summary>
        /// Serves as a hash function for type
        /// </summary>
        /// <returns>A hash code for the current Object.</returns>
        public override Int32 GetHashCode()
        {
            Int32 hashCode = 0;

            hashCode = base.GetHashCode() ^ this.ColumnName.GetHashCode() ^ this.RefTableName.GetHashCode() ^ this.RefColumnname.GetHashCode() ^ this.RefMask.GetHashCode() ^
                       this.DisplayColumns.GetHashCode() ^ this.SearchColumns.GetHashCode() ^ this.SortColumns.GetHashCode();

            return hashCode;
        }

        #endregion

        #region Private Methods

        #endregion

        #endregion
    }
}
