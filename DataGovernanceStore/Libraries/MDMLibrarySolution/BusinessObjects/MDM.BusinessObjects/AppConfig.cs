using System;
using System.IO;
using System.Xml;
using System.Runtime.Serialization;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Class that represents Application configuration setting
    /// </summary>
    [DataContract]
    public class AppConfig : MDMObject, IAppConfig
    {
        #region Fields

        /// <summary>
        /// field denoting Value
        /// </summary>
        private string _value;

        /// <summary>
        /// field denoting Description
        /// </summary>
        private string _description;

        /// <summary>
        /// field denoting Domain
        /// </summary>
        private string _domain;

        /// <summary>
        /// field denoting Client
        /// </summary>
        private string _client;

        /// <summary>
        /// field denoting RowSourceType
        /// </summary>
        private string _rowSourceType;

        /// <summary>
        /// field denoting RowSource
        /// </summary>
        private string _rowSource;

        /// <summary>
        /// field denoting UserConfigurable
        /// </summary>
        private bool _userConfigurable;

        /// <summary>
        /// field denoting LongDescription
        /// </summary>
        private string _longDescription;

        /// <summary>
        /// field denoting ValidationRule
        /// </summary>
        private string _validationRule;

        /// <summary>
        /// field denoting ValidationMethod
        /// </summary>
        private string _validationMethod;

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public AppConfig()
        {
            InitProperties();
        }

        /// <summary>
        /// Constructor with Id as input parameter
        /// </summary>
        /// <param name="id">Indicates the Identity of an AppConfig Instance</param>
        public AppConfig(Int32 id) : base(id)
        {
            InitProperties();
        }

        /// <summary>
        /// Constructor with Object array. 
        /// </summary>
        /// <param name="objectArray">Object array containing values for AppConfig. </param>
        public AppConfig(object[] objectArray)
        {
            int intId = -1;
            if (objectArray[0] != null)
                Int32.TryParse(objectArray[0].ToString(), out intId);

            Id = intId;

            if (objectArray[1] != null)
                Name = objectArray[1].ToString().Trim();

            if (objectArray[2] != null)
                Value = objectArray[2].ToString();

            if (objectArray[3] != null)
                Description = objectArray[3].ToString();

            if (objectArray[4] != null)
                Domain = objectArray[4].ToString();

            if (objectArray[5] != null)
                Client = objectArray[5].ToString();

            if (objectArray[6] != null)
                RowSourceType = objectArray[6].ToString();

            if (objectArray[7] != null)
                RowSource = objectArray[7].ToString();

            if (objectArray[8] != null)
            {
                bool configurable;
                if (Boolean.TryParse(objectArray[8].ToString(), out configurable))
                {
                    UserConfigurable = configurable;
                }
            }

            if (objectArray[15] != null)
                LongDescription = objectArray[15].ToString();

            if (objectArray[16] != null)
                ValidationRule = objectArray[16].ToString();

            if (objectArray[17] != null)
                ValidationMethod = objectArray[17].ToString();
        }
        
        /// <summary>
        /// Constructor with XML as input parameter
        /// </summary>
        /// <param name="valuesAsXml">XML having value for AppConfig object</param>
        /// <example>
        /// Sample XML
        /// <para>
        ///     &lt;AppConfig 
        ///         ShortName="Option1" 
        ///         PK_App_Config="4" 
        ///         LongName="Option1" 
        ///         Action="Create" 
        ///         Description="Some descr" 
        ///         Client="c" 
        ///         Domain="d" 
        ///         RowSourceType="r1" 
        ///         RowSource="r1" 
        ///         LongDescription="long descr" 
        ///         ValidationRule="rule" 
        ///         ValidationMethod="method" 
        ///         UserConfigurable="True" /&gt;
        /// </para>
        /// </example>
        public AppConfig(String valuesAsXml)
        {
            /*
             * Sample:
             * <AppConfig ShortName="Option1" PK_App_Config="4" LongName="Option1" Action="Create" Description="Some descr" Client="c" Domain="d" 
             *     RowSourceType="r1" RowSource="r1" LongDescription="long descr" ValidationRule="rule" ValidationMethod="method" UserConfigurable="True" />
             */

            if (!String.IsNullOrEmpty(valuesAsXml))
            {
                XmlTextReader reader = null;

                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (reader.Read())
                    {
                        if (reader.Name == "AppConfig")
                        {
                            if (reader.IsStartElement())
                            {
                                #region Read role metadata

                                if (reader.HasAttributes)
                                {
                                    if (reader.MoveToAttribute("PK_App_Config"))
                                    {
                                        Id = reader.ReadContentAsInt();
                                    }
                                    else if (reader.MoveToAttribute("Id"))
                                    {
                                        Id = reader.ReadContentAsInt();
                                    }

                                    if (reader.MoveToAttribute("ShortName"))
                                    {
                                        Name = reader.ReadContentAsString();
                                    }
                                    else if (reader.MoveToAttribute("Name"))
                                    {
                                        Name = reader.ReadContentAsString();
                                    }

                                    if (reader.MoveToAttribute("LongName"))
                                    {
                                        LongName = reader.ReadContentAsString();
                                    }

                                    if (reader.MoveToAttribute("Action"))
                                    {
                                        ObjectAction act;
                                        if (Enum.TryParse(reader.ReadContentAsString(), out act))
                                        {
                                            Action = act;
                                        }
                                    }

                                    if (reader.MoveToAttribute("Value"))
                                    {
                                        Value = reader.ReadContentAsString();
                                    }

                                    if (reader.MoveToAttribute("Description"))
                                    {
                                        Description = reader.ReadContentAsString();
                                    }

                                    if (reader.MoveToAttribute("Client"))
                                    {
                                        Client = reader.ReadContentAsString();
                                    }

                                    if (reader.MoveToAttribute("Domain"))
                                    {
                                        Domain = reader.ReadContentAsString();
                                    }

                                    if (reader.MoveToAttribute("RowSourceType"))
                                    {
                                        RowSourceType = reader.ReadContentAsString();
                                    }

                                    if (reader.MoveToAttribute("RowSource"))
                                    {
                                        RowSource = reader.ReadContentAsString();
                                    }

                                    if (reader.MoveToAttribute("LongDescription"))
                                    {
                                        LongDescription = reader.ReadContentAsString();
                                    }

                                    if (reader.MoveToAttribute("ValidationRule"))
                                    {
                                        ValidationRule = reader.ReadContentAsString();
                                    }
                                    if (reader.MoveToAttribute("ValidationMethod"))
                                    {
                                        ValidationMethod = reader.ReadContentAsString();
                                    }

                                    if (reader.MoveToAttribute("UserConfigurable"))
                                    {
                                        Boolean userConfigurable;
                                        if (Boolean.TryParse(reader.ReadContentAsString().ToLowerInvariant(), out userConfigurable))
                                        {
                                            UserConfigurable = userConfigurable;
                                        }
                                    }
                                }

                                #endregion Read role metadata
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
        }

        #endregion

        #region Property

        /// <summary>
        /// Property for Value of AppConfig
        /// </summary>
        [DataMember]
        public string Value
        {
            get { return _value; }
            set { this._value = value; }
        }

        /// <summary>
        /// Property for Description of AppConfig
        /// </summary>
        [DataMember]
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        /// <summary>
        /// Property for Domain of AppConfig
        /// </summary>
        [DataMember]
        public string Domain
        {
            get { return _domain; }
            set { _domain = value; }
        }

        /// <summary>
        /// Property for Client of AppConfig
        /// </summary>
        [DataMember]
        public string Client
        {
            get { return _client; }
            set { _client = value; }
        }

        /// <summary>
        /// Property for Row Source Type of AppConfig
        /// </summary>
        [DataMember]
        public string RowSourceType
        {
            get { return _rowSourceType; }
            set { _rowSourceType = value; }
        }

        /// <summary>
        /// Property for Row Source of AppConfig
        /// </summary>
        [DataMember]
        public string RowSource
        {
            get { return _rowSource; }
            set { _rowSource = value; }
        }

        /// <summary>
        /// Property for User Configurable option of AppConfig
        /// </summary>
        [DataMember]
        public bool UserConfigurable
        {
            get { return _userConfigurable; }
            set { _userConfigurable = value; }
        }

        /// <summary>
        /// Property for Long Description of AppConfig
        /// </summary>
        [DataMember]
        public string LongDescription
        {
            get { return _longDescription; }
            set { _longDescription = value; }
        }

        /// <summary>
        /// Property for Validation Rule of AppConfig
        /// </summary>
        [DataMember]
        public string ValidationRule
        {
            get { return _validationRule; }
            set { _validationRule = value; }
        }

        /// <summary>
        /// Property for Validation Method of AppConfig
        /// </summary>
        [DataMember]
        public string ValidationMethod
        {
            get { return _validationMethod; }
            set { _validationMethod = value; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Xml representation of AppConfig object
        /// </summary>
        /// <returns>Xml format of AppConfig</returns>
        public override String ToXml()
        {
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //AppConfig node start
            xmlWriter.WriteStartElement("AppConfig");

            xmlWriter.WriteAttributeString("Id", Id.ToString());
            xmlWriter.WriteAttributeString("Name", Name);
            xmlWriter.WriteAttributeString("LongName", LongName);
            xmlWriter.WriteAttributeString("Action", Action.ToString());

            xmlWriter.WriteAttributeString("Value", Value);
            xmlWriter.WriteAttributeString("Description", Description);
            xmlWriter.WriteAttributeString("Client", Client);
            xmlWriter.WriteAttributeString("Domain", Domain);
            xmlWriter.WriteAttributeString("RowSourceType", RowSourceType);
            xmlWriter.WriteAttributeString("RowSource", RowSource);
            xmlWriter.WriteAttributeString("UserConfigurable", UserConfigurable.ToString());
            xmlWriter.WriteAttributeString("LongDescription", LongDescription);
            xmlWriter.WriteAttributeString("ValidationRule", ValidationRule);
            xmlWriter.WriteAttributeString("ValidationMethod", ValidationMethod);

            //AppConfig node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //get the actual XML
            String xml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return xml;
        }

        /// <summary>
        /// Xml representation of AppConfig object
        /// </summary>
        /// <param name="serialization">Type of serialization</param>
        /// <returns>Xml format of AppConfig</returns>
        public override String ToXml(ObjectSerialization serialization)
        {
            return ToXml();
        }

        #endregion Public Methods

        #region Private Methods

        private void InitProperties()
        {
            Value = string.Empty;
            Description = string.Empty;
            Domain = string.Empty;
            Client = string.Empty;
            RowSourceType = string.Empty;
            RowSource = string.Empty;
            LongDescription = string.Empty;
            ValidationRule = string.Empty;
            ValidationMethod = string.Empty;
        }

        #endregion Private Methods
    }
}
