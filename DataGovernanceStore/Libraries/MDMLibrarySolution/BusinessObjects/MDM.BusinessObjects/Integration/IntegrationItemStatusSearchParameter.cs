using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects.Integration
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Class for key and its values for IntegrationItemStatus search
    /// </summary>
    [DataContract]
    public class IntegrationItemStatusSearchParameter : ObjectBase, IIntegrationItemStatusSearchParameter
    {
        #region Fields

        /// <summary>
        /// Indicates key for search. Typically Id of MDMObjectType or ExternalObjectType or DimensionType
        /// </summary>
        private Int32 _searchKey = -1;
        
        /// <summary>
        /// Indicates collection of values for given key. Typically Ids of MDMObject or ExternalId or StatusType (Error, info etc).
        /// </summary>
        [DataMember]
        private Collection<String> _searchValues = new Collection<String>();

        /// <summary>
        /// Indicates operator for search.
        /// </summary>
        private SearchOperator _operator = SearchOperator.In;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Parameter less constructor
        /// </summary>
        public IntegrationItemStatusSearchParameter()
            : base()
        {
        }

        /// <summary>
        /// Xml constructor
        /// </summary>
        /// <param name="valuesAsXml">Values in xml format for IntegrationItemStatusSearchParameter</param>
        public IntegrationItemStatusSearchParameter(String valuesAsXml)
        {
            LoadIntegrationIntegrationItemStatusSearchParameter(valuesAsXml);
        }

        #endregion Constructor

        #region Properties

        /// <summary>
        /// Indicates key for search. Typically Id of MDMObjectType or ExternalObjectType or DimensionType
        /// </summary>
        [DataMember]
        public Int32 SearchKey
        {
            get { return _searchKey; }
            set { _searchKey = value; }
        }

        /// <summary>
        /// Indicates collection of values for given key. Typically Ids of MDMObject or ExternalId or StatusType (Error, info etc).
        /// </summary>
        [DataMember]
        public Collection<String> SearchValues
        {
            get { return _searchValues; }
            set { _searchValues = value; }
        }

        /// <summary>
        /// Indicates operator for search.
        /// </summary>
        [DataMember]
        public SearchOperator Operator
        {
            get { return _operator; }
            set { _operator = value; }
        }

        #endregion Properties

        #region Public methods

        /// <summary>
        /// Represents IntegrationItemStatusSearchParameter in xml format
        /// </summary>
        /// <returns>Xml representation of IIntegrationItemStatusSearchParameter</returns>
        public String ToXml()
        {
            CultureInfo culture = CultureInfo.InvariantCulture;
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //IntegrationItemStatusSearchParameter node start
            xmlWriter.WriteStartElement("IntegrationItemStatusSearchParameter");

            xmlWriter.WriteAttributeString("SearchKey", this.SearchKey.ToString(culture));
            xmlWriter.WriteAttributeString("Operator", this.Operator.ToString());

            if (this.SearchValues != null)
            {
                StringBuilder searchValues = new StringBuilder();

                foreach (String searchValue in this.SearchValues)
                {
                    searchValues.Append(searchValue + ",");
                }

                if (this.SearchValues.Any())
                {
                    searchValues.Length = searchValues.Length - 1; // remove last comma
                }

                xmlWriter.WriteAttributeString("SearchValues", searchValues.ToString());
            }

            //IntegrationItemStatusSearchParameter node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();
            //get the actual XML
            String xml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return xml;

        }

        /// <summary>
        /// Return new object with the values same as current one
        /// </summary>
        /// <returns>New object with same values as current one</returns>
        public IIntegrationItemStatusSearchParameter Clone()
        {
            throw new NotImplementedException();
        }

        #endregion Public methods

        #region Private Methods

        private void LoadIntegrationIntegrationItemStatusSearchParameter(String valueAsXml)
        {
            if (!String.IsNullOrEmpty(valueAsXml))
            {
                XmlTextReader reader = null;

                try
                {
                    reader = new XmlTextReader(valueAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "IntegrationItemStatusSearchParameter")
                        {
                            #region Read IntegrationItemStatusSearchParameter properties

                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("SearchKey"))
                                {
                                    this.SearchKey = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                                }

                                if (reader.MoveToAttribute("Operator"))
                                {
                                    this.Operator = (SearchOperator)Enum.Parse(typeof(SearchOperator), reader.ReadContentAsString());
                                }

                                if (reader.MoveToAttribute("SearchValues"))
                                {
                                    foreach (var searchValue in reader.ReadContentAsString().Split(','))
                                    {
                                        this.SearchValues.Add(searchValue);
                                    }
                                }
                            }

                            #endregion Read IntegrationItemStatusSearchParameter properties
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

        #endregion Private Methods
    }
}
