using System;
using System.Runtime.Serialization;
using System.IO;
using System.Xml;

namespace MDM.BusinessObjects.DynamicTableSchema
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies the DBColumn of Dynamic Schema
    /// </summary>
    [DataContract]
    public class DBColumn : MDMObject, IDBColumn
    {
        #region Fields

        /// <summary>
        /// field denoting Old Name of DBColumn
        /// </summary>
        [DataMember]
        private String _oldName = String.Empty;

        /// <summary>
        /// field denoting Data Type of DBColumn
        /// </summary>
        [DataMember]
        private String _dataType = String.Empty;

        /// <summary>
        /// field denoting Length of DBColumn
        /// </summary>
        [DataMember]
        private Int32 _length = 0;

        /// <summary>
        /// field denoting Precision of DBColumn
        /// </summary>
        [DataMember]
        private Int32 _precision = 0;

        /// <summary>
        /// field denoting default Value of DBColumn
        /// </summary>
        [DataMember]
        private String _defaultValue = String.Empty;

        /// <summary>
        /// field denoting Sequence of DBColumn
        /// </summary>
        [DataMember]
        private Int32 _sequence = 0;

        /// <summary>
        /// field denoting Nullable of DBColumn
        /// </summary>
        [DataMember]
        private Boolean _nullable = false;

        /// <summary>
        /// field denoting IsUnique of DBColumn
        /// </summary>
        [DataMember]
        private Boolean _isUnique = false;

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public DBColumn()
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
        ///         &lt;Column
        ///             Id="101" 
        ///             Name="Code" 
        ///             OldName="Value"
        ///             DataType="Integer"
        ///             Length="200"
        ///             Precision="2"
        ///             DefaultValue="1000"
        ///             Sequence="1000"
        ///             Nullable="True"
        ///             IsUnique="True"
        ///             Action="Create"
        /// 
        /// /&gt;
        /// </para>
        /// </example>
        public DBColumn(String valuesAsXml)
        {
            LoadDBColumn(valuesAsXml);
        }

        #endregion

        #region Properties

        /// <summary>
        ///  Property denoting the Old Name of the DBColumn
        /// </summary>
        [DataMember]
        public String OldName
        {
            get
            {
                return this._oldName;
            }
            set
            {
                this._oldName = value;
            }
        }

        /// <summary>
        ///  Property denoting the DataType of the DBColumn
        /// </summary>
        [DataMember]
        public String DataType
        {
            get
            {
                return this._dataType;
            }
            set
            {
                this._dataType = value;
            }
        }

        /// <summary>
        ///  Property denoting the Length of the DBColumn
        /// </summary>
        [DataMember]
        public Int32 Length
        {
            get
            {
                return this._length;
            }
            set
            {
                this._length = value;
            }
        }

        /// <summary>
        ///  Property denoting the Precision of the DBColumn
        /// </summary>
        [DataMember]
        public Int32 Precision
        {
            get
            {
                return this._precision;
            }
            set
            {
                this._precision = value;
            }
        }

        /// <summary>
        ///  Property denoting the Sequence of the DBColumn
        /// </summary>
        [DataMember]
        public Int32 Sequence
        {
            get
            {
                return this._sequence;
            }
            set
            {
                this._sequence = value;
            }
        }

        /// <summary>
        ///  Property denoting the Default Value of the DBColumn
        /// </summary>
        [DataMember]
        public String DefaultValue
        {
            get
            {
                return this._defaultValue;
            }
            set
            {
                this._defaultValue = value;
            }
        }

        /// <summary>
        ///  Property denoting the Nullable of the DBColumn
        /// </summary>
        [DataMember]
        public Boolean Nullable
        {
            get
            {
                return this._nullable;
            }
            set
            {
                this._nullable = value;
            }
        }

        /// <summary>
        ///  Property denoting the IsUnique of the DBColumn
        /// </summary>
        [DataMember]
        public Boolean IsUnique
        {
            get
            {
                return this._isUnique;
            }
            set
            {
                this._isUnique = value;
            }
        }

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
            {
                if (obj is DBColumn)
                {
                    DBColumn objectToBeCompared = obj as DBColumn;

                    if (this.OldName != objectToBeCompared.OldName)
                        return false;

                    if (this.DataType != objectToBeCompared.DataType)
                        return false;

                    if (this.Length != objectToBeCompared.Length)
                        return false;

                    if (this.Precision != objectToBeCompared.Precision)
                        return false;

                    if (this.DefaultValue != objectToBeCompared.DefaultValue)
                        return false;

                    if (this.Sequence != objectToBeCompared.Sequence)
                        return false;

                    if (this.Nullable != objectToBeCompared.Nullable)
                        return false;

                    if (this.IsUnique != objectToBeCompared.IsUnique)
                        return false;

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Serves as a hash function for type
        /// </summary>
        /// <returns>A hash code for the current Object.</returns>
        public override Int32 GetHashCode()
        {
            Int32 hashCode = 0;

            hashCode = base.GetHashCode() ^ this.OldName.GetHashCode() ^ this.DataType.GetHashCode() ^ this.Length.GetHashCode() ^ this.Precision.GetHashCode() ^
                       this.DefaultValue.GetHashCode() ^ this.Sequence.GetHashCode() ^ this.Nullable.GetHashCode() ^ this.IsUnique.GetHashCode();

            return hashCode;
        }

        /// <summary>
        /// Delta Merge of DBcolumn
        /// </summary>
        /// <param name="deltaColumn">DBColumn that needs to be merged.</param>
        /// <param name="iCallerContext">Indicates the name of application and the module that are performing the action</param>
        /// <param name="returnClonedObject">true means return new merged cloned object;otherwise, same object.</param>
        /// <returns>Merged IDBTable instance</returns>
        public IDBColumn MergeDelta(IDBColumn deltaColumn, ICallerContext iCallerContext, bool returnClonedObject = true)
        {
            IDBColumn mergedDBColumn = (returnClonedObject == true) ? deltaColumn.Clone() : deltaColumn;

            mergedDBColumn.Action = (mergedDBColumn.Equals(this)) ? ObjectAction.Read : ObjectAction.Update;

            // If delta object column name and original object column name are not same it means user has renamed the column name.
            // In such scenario pass column action as Rename.
            if (mergedDBColumn.Action != ObjectAction.Read && !mergedDBColumn.Name.Equals(this.Name))
            {
                mergedDBColumn.Action = ObjectAction.Rename;
            }

            return mergedDBColumn;
        }

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
            xmlWriter.WriteStartElement("Column");

            #region Write DBColumn Properties

            xmlWriter.WriteAttributeString("Id", this.Id.ToString());
            xmlWriter.WriteAttributeString("Name", this.Name);
            xmlWriter.WriteAttributeString("OldName", this.OldName);
            xmlWriter.WriteAttributeString("DataType", this.DataType.ToString());
            xmlWriter.WriteAttributeString("Length", this.Length.ToString());
            xmlWriter.WriteAttributeString("Precision", this.Precision.ToString());
            xmlWriter.WriteAttributeString("DefaultValue", this.DefaultValue);
            xmlWriter.WriteAttributeString("Sequence", this.Sequence.ToString());
            xmlWriter.WriteAttributeString("Nullable", this.Nullable.ToString());
            xmlWriter.WriteAttributeString("IsUnique", this.IsUnique.ToString());
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
            String DBColumnXml = String.Empty;

            if (serialization == ObjectSerialization.Full)
            {
                DBColumnXml = this.ToXml();
            }
            else
            {
                StringWriter sw = new StringWriter();
                XmlTextWriter xmlWriter = new XmlTextWriter(sw);

                //DBColumn node start
                xmlWriter.WriteStartElement("Column");

                if (serialization == ObjectSerialization.ProcessingOnly)
                {
                    #region Write DBColumn Properties

                    xmlWriter.WriteAttributeString("Id", this.Id.ToString());
                    xmlWriter.WriteAttributeString("Name", this.Name);
                    xmlWriter.WriteAttributeString("OldName", this.OldName);
                    xmlWriter.WriteAttributeString("DataType", this.DataType.ToString());
                    xmlWriter.WriteAttributeString("Length", this.Length.ToString());
                    xmlWriter.WriteAttributeString("Precision", this.Precision.ToString());
                    xmlWriter.WriteAttributeString("DefaultValue", this.DefaultValue);
                    xmlWriter.WriteAttributeString("Sequence", this.Sequence.ToString());
                    xmlWriter.WriteAttributeString("Nullable", this.Nullable.ToString());
                    xmlWriter.WriteAttributeString("Action", this.Action.ToString());

                    #endregion
                }
                else if (serialization == ObjectSerialization.UIRender)
                {
                    #region Write DBColumn Properties

                    xmlWriter.WriteAttributeString("Id", this.Id.ToString());
                    xmlWriter.WriteAttributeString("Name", this.Name);
                    xmlWriter.WriteAttributeString("OldName", this.OldName);
                    xmlWriter.WriteAttributeString("DataType", this.DataType.ToString());
                    xmlWriter.WriteAttributeString("Length", this.Length.ToString());
                    xmlWriter.WriteAttributeString("Precision", this.Precision.ToString());
                    xmlWriter.WriteAttributeString("DefaultValue", this.DefaultValue);
                    xmlWriter.WriteAttributeString("Sequence", this.Sequence.ToString());
                    xmlWriter.WriteAttributeString("Nullable", this.Nullable.ToString());
                    xmlWriter.WriteAttributeString("Action", this.Action.ToString());

                    #endregion
                }

                //DBColumn node end
                xmlWriter.WriteEndElement();

                xmlWriter.Flush();

                //get the actual XML
                DBColumnXml = sw.ToString();

                xmlWriter.Close();
                sw.Close();
            }
            return DBColumnXml;
        }

        /// <summary>
        /// Clone DBColumn object
        /// </summary>
        /// <returns>
        /// Cloned copy of IDBColumn object.
        /// </returns>
        public IDBColumn Clone()
        {
            DBColumn clonedDBColumn = new DBColumn();

            clonedDBColumn.Id = this.Id;
            clonedDBColumn.Name = this.Name;
            clonedDBColumn.LongName = this.LongName;
            clonedDBColumn.Locale = this.Locale;
            clonedDBColumn.Action = this.Action;
            clonedDBColumn.AuditRefId = this.AuditRefId;
            clonedDBColumn.ExtendedProperties = this.ExtendedProperties;

            clonedDBColumn.OldName = this.OldName;
            clonedDBColumn.DataType = this.DataType;
            clonedDBColumn.Length = this.Length;
            clonedDBColumn.Precision = this.Precision;
            clonedDBColumn.DefaultValue = this.DefaultValue;
            clonedDBColumn.Sequence = this.Sequence;
            clonedDBColumn.Nullable = this.Nullable;
            clonedDBColumn.IsUnique = this.IsUnique;

            return clonedDBColumn;
        }

        #endregion

        #endregion

        #region Private Methods

        /// <summary>
        /// Load DBColumn object from XML.
        /// </summary>
        /// <param name="valuesAsXml">XML having xml value</param>
        /// <example>
        ///     Sample XML:
        ///     <para>
        ///     <![CDATA[
        ///         <Column
        ///             Id="101" 
        ///             Name="Code" 
        ///             OldName="Value"
        ///             DataType="Integer"
        ///             Length="200"
        ///             Precision="0"
        ///             DefaultValue="100"
        ///             Sequence="1"
        ///             Nullable="True"
        ///             IsUnique="True"
        ///             Action="Create"
        ///         </Column>
        ///     ]]>    
        ///     </para>
        /// </example>
        private void LoadDBColumn(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "Column")
                        {
                            #region Read DBColumn Properties

                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("Id"))
                                {
                                    this.Id = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                                }

                                if (reader.MoveToAttribute("Name"))
                                {
                                    this.Name = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("OldName"))
                                {
                                    this.OldName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("DataType"))
                                {
                                    this.DataType = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("Length"))
                                {
                                    this.Length = reader.ReadContentAsInt();
                                }

                                if (reader.MoveToAttribute("Precision"))
                                {
                                    this.Precision = reader.ReadContentAsInt();
                                }

                                if (reader.MoveToAttribute("DefaultValue"))
                                {
                                    this.DefaultValue = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("Sequence"))
                                {
                                    this.Sequence = reader.ReadContentAsInt();
                                }

                                if (reader.MoveToAttribute("Nullable"))
                                {
                                    this.Nullable = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
                                }

                                if (reader.MoveToAttribute("IsUnique"))
                                {
                                    this.IsUnique = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
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

        #endregion
    }
}
