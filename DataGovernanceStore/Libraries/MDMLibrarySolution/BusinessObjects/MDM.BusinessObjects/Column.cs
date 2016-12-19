using System;
using System.IO;
using System.Xml;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;
using System.Linq;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Represents columns in table. Columns defines the structure of table
    /// </summary>
    [DataContract]
    public class Column : MDMObject, IColumn
    {
        #region Fields

        /// <summary>
        ///  Field for default value
        /// </summary>
        private Object _defaultValue = null;

        /// <summary>
        /// Field for data type of column
        /// </summary>
        private String _dataType = String.Empty;

        /// <summary>
        /// Field for data length
        /// </summary>
        private String _dataLength = String.Empty;

        /// <summary>
        /// Field denoting Nullable property
        /// </summary>
        private Boolean _nullable = false;

        /// <summary>
        /// Field for column is unique or not.
        /// </summary>
        private Boolean _isUnique = false;

        /// <summary>
        /// Field denoting the column sort order.
        /// </summary>
        private SortOrder _sortOder = SortOrder.None;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public Column()
            : base()
        {
        }

        /// <summary>
        /// Constructor with Id as input parameter
        /// <param name="id">Indicates the Identity of Column Instance (RSTObjectId)</param>
        /// </summary>
        public Column( Int32 id )
            : base(id)
        {
        }

        /// <summary>
        /// Constructor with all properties of Column object
        /// </summary>
        /// <param name="id">Indicates the Identity of a Column (RSTObjectId)</param>
        /// <param name="name">Indicates the Name of a Column </param>
        /// <param name="longName">Indicates the LongName of a Column </param>
        /// <param name="defaultValue">Indicates the Default Value for the Column </param>
        public Column( Int32 id , String name, String longName, Object defaultValue)
            : base(id, name,longName)
        {
            this.DefaultValue = defaultValue;
        }

        /// <summary>
        /// Create column object from xml
        /// </summary>
        /// <param name="valuesAsXml">
        /// Xml having values which we want to populate in current object
        /// <para>
        /// Sample Xml:
        /// <![CDATA[
        /// <Column Id="" Name="LookupKey" DisplayName="LookupKey" DataType="String" DataLength="255" Nullable="False" IsUnique="False"/>
        /// ]]>
        /// </para>
        /// </param>
        public Column( String valuesAsXml )
        {
            LoadColumnFromXml(valuesAsXml);
        }

        #endregion Constructor

        #region Properties

        /// <summary>
        /// Indicates the DefaultValue of column
        /// </summary>
        [DataMember]
        public Object DefaultValue
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
        /// Indicates the DataType of column
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
        /// Indicates the DataLength of column
        /// </summary>
        [DataMember]
        public String DataLength
        {
            get
            {
                return this._dataLength;
            }
            set
            {
                this._dataLength = value;
            }
        }

        /// <summary>
        ///  Property denoting the Nullable property
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
        /// Indicates the Column IsUnique or not.
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

        /// <summary>
        /// Indicates the Column Sort order.
        /// </summary>
        [DataMember]
        public SortOrder SortOrder
        {
            get
            {
                return this._sortOder;
            }
            set
            {
                this._sortOder = value;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Represents Column in Xml format
        /// </summary>
        /// <returns>String representing Column in Xml format</returns>
        public override String ToXml()
        {
            String xml = String.Empty;
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            xmlWriter.WriteStartElement("Column");

            #region write Column meta data for Full Xml

            xmlWriter.WriteAttributeString("Id", this.Id.ToString());
            xmlWriter.WriteAttributeString("Name", this.Name);
            xmlWriter.WriteAttributeString("LongName", this.LongName);
            if (this.DefaultValue == null)
            {
                xmlWriter.WriteAttributeString("DefaultValue", String.Empty);
            }
            else
            {
                xmlWriter.WriteAttributeString("DefaultValue", this.DefaultValue.ToString());
            }
            xmlWriter.WriteAttributeString("DataType", this.DataType);
            xmlWriter.WriteAttributeString("DataLength", this.DataLength);
            xmlWriter.WriteAttributeString("Nullable", this.Nullable.ToString());
            xmlWriter.WriteAttributeString("IsUnique", this.IsUnique.ToString());
            xmlWriter.WriteAttributeString("SortOrder", this.SortOrder.ToString());

            #endregion write Column meta data for Full Xml

            //Column node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //get the actual XML
            xml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return xml;
        }

        /// <summary>
        /// Represents Column in Xml format for given Serialization option
        /// </summary>
        /// <param name="objectSerialization">Options specifying which Xml format is to be generated</param>
        /// <returns>String representing Column in Xml format</returns>
        public override String ToXml( ObjectSerialization objectSerialization )
        {
            String xml = String.Empty;
            if ( objectSerialization == ObjectSerialization.Full )
            {
                return this.ToXml();
            }
            else
            {
                StringWriter sw = new StringWriter();
                XmlTextWriter xmlWriter = new XmlTextWriter(sw);

                xmlWriter.WriteStartElement("Column");

                if ( objectSerialization == ObjectSerialization.ProcessingOnly )
                {
                    #region write Column meta data for Processing Xml

                    xmlWriter.WriteAttributeString("Id", this.Id.ToString());
                    xmlWriter.WriteAttributeString("Name", this.Name);
                    xmlWriter.WriteAttributeString("LongName", this.LongName);
                    if (this.DefaultValue == null)
                    {
                        xmlWriter.WriteAttributeString("DefaultValue", String.Empty);
                    }
                    else
                    {
                        xmlWriter.WriteAttributeString("DefaultValue", this.DefaultValue.ToString());
                    }
                    xmlWriter.WriteAttributeString("DataType", this.DataType);
                    xmlWriter.WriteAttributeString("DataLength", this.DataLength);
                    xmlWriter.WriteAttributeString("Nullable", this.Nullable.ToString());
                    xmlWriter.WriteAttributeString("IsUnique", this.IsUnique.ToString());

                    #endregion write Column meta data for Processing Xml

                }
                else if ( objectSerialization == ObjectSerialization.UIRender )
                {
                    #region write Column meta data for UIRendering Xml

                    xmlWriter.WriteAttributeString("Id", this.Id.ToString());
                    xmlWriter.WriteAttributeString("Name", this.Name);
                    xmlWriter.WriteAttributeString("LongName", this.LongName);
                    if (this.DefaultValue == null)
                    {
                        xmlWriter.WriteAttributeString("DefaultValue", String.Empty);
                    }
                    else
                    {
                        xmlWriter.WriteAttributeString("DefaultValue", this.DefaultValue.ToString());
                    }
                    xmlWriter.WriteAttributeString("DataType", this.DataType);
                    xmlWriter.WriteAttributeString("DataLength", this.DataLength);
                    xmlWriter.WriteAttributeString("Nullable", this.Nullable.ToString());
                    xmlWriter.WriteAttributeString("IsUnique", this.IsUnique.ToString());

                    #endregion write Column meta data for UIRendering Xml
                }

                //Column node end
                xmlWriter.WriteEndElement();

                xmlWriter.Flush();

                //get the actual XML
                xml = sw.ToString();

                xmlWriter.Close();
                sw.Close();
            }
            return xml;
        }

        /// <summary>
        /// Create column object from xml
        /// </summary>
        /// <param name="valuesAsXml">
        /// Xml having values which we want to populate in current object
        /// <para>
        /// Sample Xml:
        /// <![CDATA[
        /// <Column Id="" Name="LookupKey" DisplayName="LookupKey" DefaultValue ="" DataType="String" DataLength="255" Nullable="False" IsUnique="False"/>
        /// ]]>
        /// </para>
        /// </param>
        public void LoadColumnFromXml( String valuesAsXml )
        {
            #region Sample Xml
            /*
             * <Column Id="" Name="LookupKey" LongName="LookupKey" DefaultValue ="" DataType="String" DataLength="255" Nullable="False" IsUnique="False"/>
             */
            #endregion Sample Xml

            if ( !String.IsNullOrWhiteSpace(valuesAsXml) )
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while ( !reader.EOF )
                    {
                        if ( reader.NodeType == XmlNodeType.Element && reader.Name == "Column" )
                        {
                            #region Read column Metadata

                            if ( reader.HasAttributes )
                            {
                                if ( reader.MoveToAttribute("Id") )
                                {
                                    this.Id = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(),0);
                                }

                                if ( reader.MoveToAttribute("Name") )
                                {
                                    this.Name = reader.ReadContentAsString();
                                }

                                if ( reader.MoveToAttribute("LongName") )
                                {
                                    this.LongName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("DefaultValue"))
                                {
                                    this.DefaultValue = reader.ReadContentAsObject();
                                }

                                if (reader.MoveToAttribute("DataLength"))
                                {
                                    this.DataLength = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("DataType"))
                                {
                                    String[] values = reader.ReadContentAsString().Split('(');

                                    if (values != null && values.Count() > 0)
                                    {
                                        this.DataType = values[0];

                                        if (values.Count() >= 2)
                                        {
                                            this.DataLength = values[1].Substring(0, values[1].Length - 1);
                                        }
                                    }
                                }

                                if (reader.MoveToAttribute("Nullable"))
                                {
                                    this.Nullable = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
                                }

                                if (reader.MoveToAttribute("IsUnique"))
                                {
                                    this.IsUnique = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
                                }

                                if (reader.MoveToAttribute("SortOrder"))
                                {
                                    SortOrder sortOrder = SortOrder.None;
                                    Enum.TryParse<SortOrder>(reader.ReadContentAsString(), out  sortOrder);
                                    this.SortOrder = sortOrder;
                                }
                            }

                            #endregion Read column Metadata
                        }
                        else
                        {
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
        }

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="subsetColumn">The Object to compare with the current Object.</param>
        /// <param name="compareIds">Flag to determine whether id based comparission is true or not</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public Boolean IsSuperSetOf(Column subsetColumn, Boolean compareIds = false)
        {
            if (subsetColumn != null)
            {
                if (base.IsSuperSetOf(subsetColumn, compareIds))
                {
                    if (this.DefaultValue != subsetColumn.DefaultValue)
                        return false;

                    if (this.DataType != subsetColumn.DataType)
                        return false;

                    if (this.DataLength != subsetColumn.DataLength)
                        return false;

                    if (this.Nullable != subsetColumn.Nullable)
                        return false;

                    if (this.IsUnique != subsetColumn.IsUnique)
                        return false;

                    if (this.SortOrder != subsetColumn.SortOrder)
                        return false;

                }
            }
            return true;
        }

        /// <summary>
        /// Clones column object
        /// </summary>
        /// <returns></returns>
        public IColumn Clone()
        {
            var clonedColumn = new Column {
                    Id = Id, 
                    Name = Name, 
                    LongName = LongName, 
                    Locale = Locale, 
                    ExtendedProperties = ExtendedProperties,
                    _dataLength = _dataLength, 
                    _dataType = _dataType, 
                    _defaultValue = _defaultValue, 
                    _nullable = _nullable,
                    _isUnique = _isUnique, 
                    _sortOder = _sortOder
            };

            return clonedColumn;
        }

        #endregion Methods
    }
}
