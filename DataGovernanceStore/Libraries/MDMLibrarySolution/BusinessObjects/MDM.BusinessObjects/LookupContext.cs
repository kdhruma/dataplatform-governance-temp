using System;
using System.IO;
using System.Xml;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Indicates Lookup context. This is used to specify what all information is to be loaded for lookup
    /// </summary>
    [DataContract]
    public class LookupContext : ObjectBase, ILookupContext
    {
        #region Fields

        /// <summary>
        /// Field denoting attribute id list for which lookup attribute values are to be fetched
        /// </summary>
        private Collection<Int32> _attributeIdList = new Collection<Int32>();

        /// <summary>
        /// Field denoting for which locale lookup attribute value is to be fetched
        /// </summary>
        private String _locale = "en_WW";

        /// <summary>
        /// Field denoting name of lookup table
        /// </summary>
        private String _lookupTableName = String.Empty;

        /// <summary>
        /// Field denoting how many lookup values are to be returned. Pass -1 to get all values
        /// </summary>
        private Int32 _maxRecordsToReturn = -1;

        /// <summary>
        /// Field denoting whether to return only lookup model (columns) or to return lookup with data
        /// </summary>
        private Boolean _returnOnlyModel = false;

        /// <summary>
        /// Field denoting whether lookup data is to be fetched for lookup attribute or lookup maintenance
        /// </summary>
        private LookupValueFilterType _valueFilterType = LookupValueFilterType.LookupMaster;

        /// <summary>
        /// Field denoting the criteria for filtering lookup tables
        /// </summary>
        private LookupTableFilterType _tableFilterType = LookupTableFilterType.All;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public LookupContext()
            : base()
        {
        }

        /// <summary>
        /// Populate context from Xml
        /// </summary>
        /// <param name="valuesAsXml">
        /// Xml having information to load in current information.
        /// <para>
        /// Sample Xml:
        /// <![CDATA[
        ///     <LookupContext AttributeIdList="3006,3007,3008,4120,3059" 
        ///         Locale="en_WW" 
        ///         LookupTableName="tblk_Color" 
        ///         MaxRecordsToReturn ="100" 
        ///         ReturnOnlyModel="true" 
        ///         ValueFilterType="LookupTable"/>
        /// ]]>
        /// </para>
        /// </param>
        public LookupContext(String valuesAsXml)
        {
            LoadLookupContext(valuesAsXml);
        }

        #endregion Constructor

        #region Properties

        /// <summary>
        /// Property denoting attribute id list for which lookup attribute values are to be fetched
        /// </summary>
        [DataMember]
        public Collection<Int32> AttributeIdList
        {
            get
            {
                return _attributeIdList;
            }
            set
            {
                _attributeIdList = value;
            }
        }

        /// <summary>
        /// Property denoting for which locale lookup attribute value is to be fetched
        /// </summary>
        [DataMember]
        public String Locale
        {
            get
            {
                return _locale;
            }
            set
            {
                _locale = value;
            }
        }

        /// <summary>
        /// Property denoting name of lookup table
        /// </summary>
        [DataMember]
        public String LookupTableName
        {
            get
            {
                return _lookupTableName;
            }
            set
            {
                _lookupTableName = value;
            }
        }

        /// <summary>
        /// Property denoting how many lookup values are to be returned. Pass -1 to get all values
        /// </summary>
        [DataMember]
        public Int32 MaxRecordsToReturn
        {
            get
            {
                return _maxRecordsToReturn;
            }
            set
            {
                _maxRecordsToReturn = value;
            }
        }

        /// <summary>
        /// Property denoting whether to return only lookup model (columns) or to return lookup with data
        /// </summary>
        [DataMember]
        public Boolean ReturnOnlyModel
        {
            get
            {
                return _returnOnlyModel;
            }
            set
            {
                _returnOnlyModel = value;
            }
        }

        /// <summary>
        /// Property denoting whether lookup data is to be fetched for lookup attribute or lookup maintenance
        /// </summary>
        [DataMember]
        public LookupValueFilterType ValueFilterType
        {
            get
            {
                return _valueFilterType;
            }
            set
            {
                _valueFilterType = value;
            }
        }

        /// <summary>
        /// Property denoting the criteria for filtering lookup tables
        /// </summary>
        [DataMember]
        public LookupTableFilterType TableFilterType
        {
            get { return this._tableFilterType; }
            set { this._tableFilterType = value; }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Represents LookupContext in Xml format
        /// </summary>
        /// <returns>String representing LookupContext in Xml format</returns>
        public String ToXml()
        {
            String xml = String.Empty;
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            xmlWriter.WriteStartElement("LookupContext");

            #region write LookupContext meta data for Full Xml

            xmlWriter.WriteAttributeString("AttributeIdList", ValueTypeHelper.JoinCollection(this.AttributeIdList, ","));
            xmlWriter.WriteAttributeString("Locale", this.Locale);
            xmlWriter.WriteAttributeString("LookupTableName", this.LookupTableName);
            xmlWriter.WriteAttributeString("MaxRecordsToReturn", this.MaxRecordsToReturn.ToString());
            xmlWriter.WriteAttributeString("ReturnOnlyModel", this.ReturnOnlyModel.ToString());
            xmlWriter.WriteAttributeString("ValueFilterType", this.ValueFilterType.ToString());

            #endregion write LookupContext meta data for Full Xml

            //LookupContext node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //get the actual XML
            xml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return xml;
        }

        /// <summary>
        /// Represents LookupContext in Xml format for given Serialization option
        /// </summary>
        /// <param name="objectSerialization">Options specifying which Xml format is to be generated</param>
        /// <returns>String representing LookupContext in Xml format</returns>
        public String ToXml(ObjectSerialization objectSerialization)
        {
            String xml = String.Empty;
            if (objectSerialization == ObjectSerialization.Full)
            {
                return this.ToXml();
            }
            else
            {
                StringWriter sw = new StringWriter();
                XmlTextWriter xmlWriter = new XmlTextWriter(sw);

                xmlWriter.WriteStartElement("LookupContext");

                if (objectSerialization == ObjectSerialization.ProcessingOnly)
                {
                    #region write LookupContext meta data for Processing Xml

                    xmlWriter.WriteAttributeString("AttributeIdList", ValueTypeHelper.JoinCollection(this.AttributeIdList, ","));
                    xmlWriter.WriteAttributeString("Locale", this.Locale);
                    xmlWriter.WriteAttributeString("LookupTableName", this.LookupTableName);
                    xmlWriter.WriteAttributeString("MaxRecordsToReturn", this.MaxRecordsToReturn.ToString());
                    xmlWriter.WriteAttributeString("ReturnOnlyModel", this.ReturnOnlyModel.ToString());
                    xmlWriter.WriteAttributeString("ValueFilterType", this.ValueFilterType.ToString());

                    #endregion write LookupContext meta data for Processing Xml
                }
                else if (objectSerialization == ObjectSerialization.UIRender)
                {
                    #region write LookupContext meta data for UIRendering Xml

                    xmlWriter.WriteAttributeString("AttributeIdList", ValueTypeHelper.JoinCollection(this.AttributeIdList, ","));
                    xmlWriter.WriteAttributeString("Locale", this.Locale);
                    xmlWriter.WriteAttributeString("LookupTableName", this.LookupTableName);
                    xmlWriter.WriteAttributeString("MaxRecordsToReturn", this.MaxRecordsToReturn.ToString());
                    xmlWriter.WriteAttributeString("ReturnOnlyModel", this.ReturnOnlyModel.ToString());
                    xmlWriter.WriteAttributeString("ValueFilterType", this.ValueFilterType.ToString());

                    #endregion write LookupContext meta data for UIRendering Xml
                }
                //LookupContext node end
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
        /// Populate context from Xml
        /// </summary>
        /// <param name="valuesAsXml">
        /// Xml having information to load in current information.
        /// <para>
        /// Sample Xml:
        /// <![CDATA[
        ///     <LookupContext AttributeIdList="3006,3007,3008,4120,3059" 
        ///         Locale="en_WW" 
        ///         LookupTableName="tblk_Color" 
        ///         MaxRecordsToReturn ="100" 
        ///         ReturnOnlyModel="true" 
        ///         ValueFilterType="LookupTable"/>
        /// ]]>
        /// </para>
        /// </param>
        public void LoadLookupContext(String valuesAsXml)
        {
            #region Sample Xml
            /*
             * <LookupContext AttributeIdList="3006,3007,3008,4120,3059" 
             *      Locale="en_WW" 
             *      LookupTableName="tblk_Color" 
             *      MaxRecordsToReturn ="100" 
             *      ReturnOnlyModel="true" 
             *      ValueFilterType="LookupTable"/>
             */
            #endregion Sample Xml

            XmlTextReader reader = null;
            try
            {
                reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                while (!reader.EOF)
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "LookupContext")
                    {
                        #region Read LookupContext Properties

                        if (reader.HasAttributes)
                        {
                            if (reader.MoveToAttribute("AttributeIdList"))
                            {
                                this.AttributeIdList = ValueTypeHelper.SplitStringToIntCollection(reader.ReadContentAsString(), ',');
                            }

                            if (reader.MoveToAttribute("Locale"))
                            {
                                this.Locale = reader.ReadContentAsString();
                            }

                            if (reader.MoveToAttribute("LookupTableName"))
                            {
                                this.LookupTableName = reader.ReadContentAsString();
                            }

                            if (reader.MoveToAttribute("MaxRecordsToReturn"))
                            {
                                this.MaxRecordsToReturn = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), -1);
                            }

                            if (reader.MoveToAttribute("ReturnOnlyModel"))
                            {
                                this.ReturnOnlyModel = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
                            }

                            if (reader.MoveToAttribute("ValueFilterType"))
                            {
                                LookupValueFilterType valueFilterType = LookupValueFilterType.LookupMaster;
                                Enum.TryParse(reader.ReadContentAsString(), true, out valueFilterType);
                                this.ValueFilterType = valueFilterType;
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

        #endregion Methods
    }
}
