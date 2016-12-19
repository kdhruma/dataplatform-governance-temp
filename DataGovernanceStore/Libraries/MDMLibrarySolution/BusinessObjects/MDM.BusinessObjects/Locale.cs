using System;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies the Locale
    /// </summary>
    [DataContract]
    [KnownType(typeof(MDMObject))]
    public class Locale : MDMObject, ILocale
    {
        #region Fields

        /// <summary>
        /// Field denoting the region id of the Locale
        /// </summary>
        private Int32 _regionId = 0;

        /// <summary>
        /// Field denoting the name of the region
        /// </summary>
        private String _regionName = string.Empty;

        /// <summary>
        /// Field denoting the language id of the Locale
        /// </summary>
        private Int32 _languageId = 0;

        /// <summary>
        /// Field denoting the name of the language
        /// </summary>
        private String _languageName = string.Empty;

        /// <summary>
        /// Field denoting the Culture id of the Locale
        /// </summary>
        private Int32 _cultureId = 0;

        /// <summary>
        /// Field denoting the name of the culture
        /// </summary>
        private String _cultureName = string.Empty;

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public Locale()
            : base()
        {
        }

        /// <summary>
        /// Constructor with Id as input parameter
        /// </summary>
        /// <param name="id">Indicates the Identity of a Locale</param>
        public Locale(Int32 id)
            : base(id)
        {
        }

        /// <summary>
        /// Constructor with Id, Name and Description of a Locale as input parameters
        /// </summary>
        /// <param name="id">Indicates the Identity of a Locale</param>
        /// <param name="name">Indicates the Name of a Locale</param>
        /// <param name="longName">Indicates the Description of a Locale</param>
        public Locale(Int32 id, String name, String longName)
            : base(id, name, longName)
        {
        }

        /// <summary>
        /// Constructor with object array as input parameter
        /// </summary>
        /// <param name="objectArray">Object array containing value for Locale object</param>
        public Locale(object[] objectArray)
        {
            Int32 intId = -1;
            if (objectArray[0] != null)
                Int32.TryParse(objectArray[0].ToString(), out intId);

            this.Id = intId;

            if (objectArray[1] != null)
            {
                this.Name = objectArray[1].ToString();
                LocaleEnum locale = LocaleEnum.UnKnown;
                Enum.TryParse(this.Name, out locale);
                this.Locale = locale;
            }


            if (objectArray[2] != null)
                this.LongName = objectArray[2].ToString();

            if (objectArray[3] != null)
                Int32.TryParse(objectArray[3].ToString(), out this._regionId);

            if (objectArray[4] != null)
                this._regionName = objectArray[4].ToString();

            if (objectArray[5] != null)
                Int32.TryParse(objectArray[5].ToString(), out this._cultureId);

            if (objectArray[6] != null)
                this._cultureName = objectArray[6].ToString();

            if (objectArray[7] != null)
                Int32.TryParse(objectArray[7].ToString(), out this._languageId);

            if (objectArray[8] != null)
                this._languageName = objectArray[8].ToString();
        }

        /// <summary>
        /// Constructor with XML having values of object. Populate current object using XML
        /// </summary>
        /// <param name="valuesAsXml">XML having xml value</param>
        /// <example>
        ///     Sample XML:
        ///     <para>
        ///         &lt;Locale
        ///             PK_Locale="101" 
        ///             ShortName="tblk_Color"
        ///             LongName="Update"
        ///             FK_Region="236"
        ///             RegionName=""
        ///             FK_Lang="1"
        ///             LanguageName=""
        ///             FK_Culture="50"
        ///             CultureName=""/&gt;
        /// </para>
        /// </example>
        public Locale(String valuesAsXml)
        {
            LoadLocale(valuesAsXml);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Property defining the type of the MDM object
        /// </summary>
        public new String ObjectType
        {
            get
            {
                return "Locale";
            }
        }

        /// <summary>
        /// Property denoting the Region id of a Locale
        /// </summary>
        [DataMember]
        public Int32 RegionId
        {
            get
            {
                return this._regionId;
            }
            set
            {
                this._regionId = value;
            }
        }

        /// <summary>
        /// Property denoting the Region Name of a Locale
        /// </summary>
        [DataMember]
        public String RegionName
        {
            get
            {
                return this._regionName;
            }
            set
            {
                this._regionName = value;
            }
        }

        /// <summary>
        /// Property denoting the Language id of a Locale
        /// </summary>
        [DataMember]
        public Int32 LanguageId
        {
            get
            {
                return this._languageId;
            }
            set
            {
                this._languageId = value;
            }
        }

        /// <summary>
        /// Property denoting the Language Name of a Locale
        /// </summary>
        [DataMember]
        public String LanguageName
        {
            get
            {
                return this._languageName;
            }
            set
            {
                this._languageName = value;
            }
        }

        /// <summary>
        /// Property denoting the Culture id of a Locale
        /// </summary>
        [DataMember]
        public Int32 CultureId
        {
            get
            {
                return this._cultureId;
            }
            set
            {
                this._cultureId = value;
            }
        }

        /// <summary>
        /// Property denoting the Culture Name of a Locale
        /// </summary>
        [DataMember]
        public String CultureName
        {
            get
            {
                return this._cultureName;
            }
            set
            {
                this._cultureName = value;
            }
        }

        #endregion

        #region Methods

        #region Load Methods

        /// <summary>
        /// Load DBTable object from XML.
        /// </summary>
        /// <param name="valuesAsXml">XML having xml value</param>
        /// <example>
        ///     Sample XML:
        ///     <para>
        ///     <![CDATA[
        ///         <Locale
        ///             PK_Locale="101" 
        ///             ShortName="tblk_Color" 
        ///             LongName="Update"
        ///             FK_Region="236"
        ///             RegionName=""
        ///             FK_Lang="1"
        ///             LanguageName=""
        ///             FK_Culture="50"
        ///             CultureName=""
        ///         </Locale>
        ///     ]]>    
        ///     </para>
        /// </example>
        public void LoadLocale(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "Locale")
                        {
                            #region Read DBTable Properties

                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("PK_Locale"))
                                {
                                    this.Id = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                                }

                                if (reader.MoveToAttribute("ShortName"))
                                {
                                    this.Name = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("LongName"))
                                {
                                    this.LongName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("FK_Region"))
                                {
                                    this.RegionId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                                }

                                if (reader.MoveToAttribute("RegionName"))
                                {
                                    this.RegionName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("FK_Lang"))
                                {
                                    this.LanguageId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                                }

                                if (reader.MoveToAttribute("LanguageName"))
                                {
                                    this.LanguageName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("FK_Culture"))
                                {
                                    this.CultureId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                                }

                                if (reader.MoveToAttribute("CultureName"))
                                {
                                    this.CultureName = reader.ReadContentAsString();
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
        /// Get Xml representation of Locale object
        /// </summary>
        /// <returns>Xml representation of object</returns>
        public override String ToXml()
        {
            String xml = string.Empty;
            xml = String.Format("<Locale PK_Locale=\"{0}\" ShortName=\"{1}\" LongName=\"{2}\" FK_Culture=\"{3}\" FK_Region=\"{4}\" FK_Lang=\"{5}\" />", this.Id, this.Name, this.LongName, this.CultureId, this.RegionId, this.LanguageId);

            return xml;

        }

        #endregion

        /// <summary>
        /// Returns a cloned instance of the current object
        /// </summary>
        /// <returns>Cloned Locale Instance</returns>
        public ILocale Clone()
        {
            return (Locale)this.MemberwiseClone();
        }

        #endregion

    }
}
