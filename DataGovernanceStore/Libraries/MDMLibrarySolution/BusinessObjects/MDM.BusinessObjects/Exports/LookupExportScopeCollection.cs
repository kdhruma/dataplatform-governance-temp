using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects.Exports
{
    using MDM.Core;
    using MDM.Interfaces;
    using MDM.Interfaces.Exports;

    /// <summary>
    /// Specifies the Lookup Export profile data scope collection object
    /// </summary>
    [DataContract]
    [KnownType(typeof(LocaleCollection))]
    public class LookupExportScopeCollection : InterfaceContractCollection<ILookupExportScope, LookupExportScope>, ILookupExportScopeCollection
    {
        #region Fields

        /// <summary>
        /// Field denoting whether all lookups to be included in the scope or only the specific lookups.
        /// </summary>
        private Boolean _includeAllLookup = false;

        /// <summary>
        /// Field denoting whether all locales to be included in the scope or only the specific locales.
        /// </summary>
        private Boolean _includeAllLocale = false;

        /// <summary>
        /// Field denoting the collection of lookup export scope object
        /// </summary>
        private Collection<LookupExportScope> _lookupExportScopes = new Collection<LookupExportScope>();

        /// <summary>
        /// Field denoting the collection of locale 
        /// </summary>
        private LocaleCollection _locales = new LocaleCollection();

        /// <summary>
        /// Field denoting the lookup export group by order.
        /// </summary>
        private LookupExportGroupOrder _groupBy = LookupExportGroupOrder.ColumnName;

        /// <summary>
        /// Field denoting the lookup export file format.
        /// </summary>  
        private LookupExportFileFormat _fileFormat = LookupExportFileFormat.Unknown;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LookupExportScopeCollection"/> class. 
        /// Parameter-less Constructor
        /// </summary>
        public LookupExportScopeCollection()
            : base()
        {
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="LookupExportScopeCollection"/> class. 
        /// with value as Constructor
        /// </summary>
        /// <param name="valuesAsXml">XML having values of object</param>
        public LookupExportScopeCollection(String valuesAsXml)
        {
            LoadLookupExportScopes(valuesAsXml);
        }
        #endregion Constructors

        #region Properties

        /// <summary>
        /// Represents whether all lookups to be included in the scope or only the specific lookups.
        /// </summary>
        [DataMember]
        public Boolean IncludeAllLookup
        {
            get
            {
                return _includeAllLookup;
            }
            set
            {
                _includeAllLookup = value;
            }
        }

        /// <summary>
        /// Represents whether all locales to be included in the scope or only the specific locales.
        /// </summary>
        [DataMember]
        public Boolean IncludeAllLocale
        {
            get
            {
                return _includeAllLocale;
            }
            set
            {
                _includeAllLocale = value;
            }
        }

        /// <summary>
        /// Represents the collection of Lookup export scope list.
        /// </summary>
        [DataMember]
        public Collection<LookupExportScope> LookupExportScopes
        {
            get
            {
                return _lookupExportScopes;
            }
            set
            {
                _lookupExportScopes = value;
            }
        }

        /// <summary>
        /// Represents the collection of locale list.
        /// </summary>
        [DataMember]
        public LocaleCollection Locales
        {
            get
            {
                return _locales;
            }
            set
            {
                _locales = value;
            }
        }

        /// <summary>
        /// Represents the lookup export group by order.
        /// </summary>
        [DataMember]
        public LookupExportGroupOrder GroupBy
        {
            get
            {
                return _groupBy;
            }
            set
            {
                _groupBy = value;
            }
        }

        /// <summary>
        /// Represents the lookup export file format.
        /// </summary>  
        [DataMember]
        public LookupExportFileFormat FileFormat
        {
            get
            {
                return _fileFormat;
            }
            set
            {
                _fileFormat = value;
            }
        }

        #endregion Properties

        #region Public Methods

        /// <summary>
        /// Represents Lookup export Scopes in Xml format
        /// </summary>
        /// <returns>String representation of Lookup export scopes object</returns>
        public String ToXml()
        {
            String xml = String.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //Lookup export scp type unique identifier node start
            xmlWriter.WriteStartElement("LookupExportScopes");

            xmlWriter.WriteAttributeString("IncludeAllLookup", this.IncludeAllLookup.ToString());
            xmlWriter.WriteAttributeString("IncludeAllLocale", this.IncludeAllLocale.ToString());
            xmlWriter.WriteAttributeString("GroupBy", this.GroupBy.ToString());
            xmlWriter.WriteAttributeString("FileFormat", this.FileFormat.ToString());

            if (!this.IncludeAllLocale)
            {
                if (this.Locales != null)
                    xmlWriter.WriteRaw(this.Locales.ToXml());
            }

            if (!this.IncludeAllLookup)
            {
                if (this.LookupExportScopes != null)
                {
                    foreach (LookupExportScope scope in this.LookupExportScopes)
                    {
                        xmlWriter.WriteRaw(scope.ToXml());  
                    }
                }
            }

            xmlWriter.WriteEndElement();

            xmlWriter.Flush();
            //get the actual XML
            xml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return xml;
        }

        /// <summary>
        /// Sets the Locale list for the current export scope
        /// </summary>
        /// <param name="iLocaleCollection">Indicates the locale Collection Interface</param>
        public void SetExportScopeLocales(ILocaleCollection iLocaleCollection)
        {
            if (this.Locales != null)
            {
                LocaleCollection localeCollection = (LocaleCollection)iLocaleCollection;
                this.Locales = localeCollection;
            }
        }

        /// <summary>
        /// Gets the locales list for the current export scope
        /// </summary>
        /// <returns>Locale Collection Interface</returns>
        /// <exception cref="NullReferenceException">Locale list for the current export scope is null.</exception>
        public ILocaleCollection GetExportScopeLocales()
        {
            if (this.Locales == null)
            {
                throw new NullReferenceException("Locale list for the current export scope is null.");
            }
            return (ILocaleCollection)this.Locales;
        }

        /// <summary>
        /// Get the export scope lookup table names
        /// </summary>
        /// <returns>Returns the list of lookup export table names</returns>
        public Collection<String> GetLookupExportScopeTables()
        {
            Collection<String> lookupNames = new Collection<String>();

            if (this.LookupExportScopes != null)
            {
                foreach (LookupExportScope scope in this.LookupExportScopes)
                {
                    lookupNames.Add(scope.Name);
                }
            }

            return lookupNames;
        }

        #endregion Public Methods

        #region Private Method

        /// <summary>
        /// Loads the lookup export scopes with the XML having values of object
        /// </summary>
        /// <param name="valuesAsXml">XML having values of object</param>
        private void LoadLookupExportScopes(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;

                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        #region Read syndication export profile data

                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "LookupExportScopes")
                        {
                            #region Read syndication export profile data Properties

                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("IncludeAllLocale"))
                                {
                                    this.IncludeAllLocale = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
                                }

                                if (reader.MoveToAttribute("IncludeAllLookup"))
                                {
                                    this.IncludeAllLookup = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
                                }

                                if (reader.MoveToAttribute("GroupBy"))
                                {
                                    LookupExportGroupOrder groupBy = _groupBy;
                                    Enum.TryParse<LookupExportGroupOrder>(reader.ReadContentAsString(), out groupBy);

                                    this.GroupBy = groupBy;
                                }
                                if (reader.MoveToAttribute("FileFormat"))
                                {
                                    LookupExportFileFormat fileFormat = _fileFormat;
                                    Enum.TryParse<LookupExportFileFormat>(reader.ReadContentAsString(), out fileFormat);

                                    this.FileFormat = fileFormat;
                                }
                            }

                            #endregion
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "Locales")
                        {
                            #region Read Locales collection

                            String localeXml = reader.ReadOuterXml();

                            if (!String.IsNullOrEmpty(localeXml))
                            {
                                LocaleCollection locales = new LocaleCollection(localeXml);

                                if (locales != null)
                                {
                                    foreach (Locale locale in locales)
                                    {
                                        if (!this.Locales.Contains(locale))
                                        {
                                            this.Locales.Add(locale);
                                        }
                                    }
                                }
                            }

                            #endregion Read Locales
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "LookupExportScope")
                        {
                            String scopeXml = reader.ReadOuterXml();

                            if (!String.IsNullOrEmpty(scopeXml))
                            {
                                LookupExportScope scope = new LookupExportScope(scopeXml);

                                this.LookupExportScopes.Add(scope);
                            }

                        }
                        else
                        {
                            reader.Read();
                        }

                        #endregion Read syndication export profile data
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
    }
}
