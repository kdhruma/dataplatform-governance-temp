using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    ///<summary>
    ///Specifies the specifications for search 
    ///</summary>
    [DataContract]
    [KnownType(typeof(Attribute))]
    public class SearchContext : MDMObject, ISearchContext
    {
        #region Fields

        /// <summary>
        /// Specifies the maximum number of records to return
        /// </summary>
        private Int32 _maxRecordsToReturn = 0;

        /// <summary>
        /// Represents whether to calculate search scores or not
        /// </summary>
        private Boolean _calculateScore = false;

        /// <summary>
        /// Represents if search in taxonomy is enabled
        /// </summary>
        private Boolean _searchInTaxonomy = false;

        /// <summary>
        /// Indicates search depth
        /// </summary>
        private Int32 _searchDepth = -1;

        /// <summary>
        /// Indicates whether to include category path in result or not
        /// </summary>
        private Boolean _includeCategoryPathInResult = true;

        /// <summary>
        /// Represents the list of attribute ids configured for display in the search results 
        /// </summary>
        private Collection<Attribute> _returnAttributeList = new Collection<Attribute>();

        #endregion

        #region Properties

        /// <summary>
        /// Specifies the maximum number of records to return
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
        /// Represents whether to return search scores or not
        /// </summary>
        [DataMember]
        public Boolean CalculateScore
        {
            get
            {
                return _calculateScore;
            }
            set
            {
                _calculateScore = value;
            }
        }

        /// <summary>
        /// Indicates if SearchInTaxonomy is enabled
        /// </summary>
        [DataMember]
        public Boolean SearchInTaxonomy
        {
            get
            {
                return _searchInTaxonomy;
            }
            set
            {
                _searchInTaxonomy = value;
            }
        }

        /// <summary>
        /// Indicates if SearchInTaxonomy is enabled
        /// </summary>
        [DataMember]
        public Int32 SearchDepth
        {
            get
            {
                return _searchDepth;
            }
            set
            {
                _searchDepth = value;
            }
        }

        /// <summary>
        /// Indicates whether to include category path in result or not
        /// </summary>
        [DataMember]
        public Boolean IncludeCategoryPathInResult
        {
            get
            {
                return _includeCategoryPathInResult;
            }
            set
            {
                _includeCategoryPathInResult = value;
            }
        }

        /// <summary>
        /// Represents the list of attribute ids configured for display in the search results 
        /// </summary>
        [DataMember]
        public Collection<Attribute> ReturnAttributeList
        {
            get
            {
                return _returnAttributeList;
            }
            set
            {
                _returnAttributeList = value;
            }
        }

        /// <summary>
        /// Specifies wheather return atttribute list is configured or not
        /// </summary>
        public Boolean IsRetrunAttributeListConfigured
        {
            get
            {
                return (_returnAttributeList != null && _returnAttributeList.Count > 0) ? true : false;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the Search Specifications class.
        /// </summary>
        public SearchContext()
            : base()
        {

        }

        /// <summary>
        /// Initializes a new instance of the Search Specifications class.
        /// </summary>
        /// <param name="maxRecordsToReturn"></param>
        /// <param name="returnScores"></param>
        /// <param name="returnAttributeList"></param>
        public SearchContext(Int32 maxRecordsToReturn, Boolean returnScores, Collection<Attribute> returnAttributeList)
        {
            this._maxRecordsToReturn = maxRecordsToReturn;
            this._calculateScore = returnScores;
            this._returnAttributeList = returnAttributeList;
        }

        /// <summary>
        /// Constructor with maxRecordsToReturn, returnScores, searchIntaxonomy, searchDepth, and returnAttributeList as input parameters
        /// </summary>
        /// <param name="maxRecordsToReturn">Indicates the maximum number of records to return in search</param>
        /// <param name="returnScores">Indicates whether to return score or not</param>
        /// <param name="searchIntaxonomy">Indicates whether to search in hierarchy or not</param>
        /// <param name="searchDepth">Indicates the search depth</param>
        /// <param name="returnAttributeList">Indication the collection of attributes to return</param>
        public SearchContext(Int32 maxRecordsToReturn, Boolean returnScores, Boolean searchIntaxonomy, Int32 searchDepth, Collection<Attribute> returnAttributeList)
        {
            this._maxRecordsToReturn = maxRecordsToReturn;
            this._calculateScore = returnScores;
            this._searchDepth = searchDepth;
            this._searchInTaxonomy = searchIntaxonomy;
            this._returnAttributeList = returnAttributeList;
        }

        /// <summary>
        /// Xml constructor
        /// </summary>
        /// <param name="valuesAsXml">Xml having value for current SearchContext object</param>
        public SearchContext(String valuesAsXml)
        {
            LoadSearchContext(valuesAsXml);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>True if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override Boolean Equals(Object obj)
        {
            if (base.Equals(obj))
            {
                if (obj is SearchContext)
                {
                    SearchContext objectToBeCompared = obj as SearchContext;

                    if (this.MaxRecordsToReturn != objectToBeCompared.MaxRecordsToReturn)
                        return false;

                    if (this.CalculateScore != objectToBeCompared.CalculateScore)
                        return false;

                    if (this.ReturnAttributeList != objectToBeCompared.ReturnAttributeList)

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
            Int32 hashCode = base.GetHashCode() ^ this.MaxRecordsToReturn.GetHashCode() ^ this.CalculateScore.GetHashCode() ^ this.ReturnAttributeList.GetHashCode() ^ this.SearchInTaxonomy.GetHashCode() ^ this.SearchDepth.GetHashCode();

            return hashCode;
        }

        /// <summary>
        /// Get Xml representation of Search Specifications
        /// </summary>
        /// <returns>Xml representation of Search Specifications</returns>
        public override String ToXml()
        {
            String searchSpecificationsXml = String.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //Search Specifications node start
            xmlWriter.WriteStartElement("SearchSpecifications");

            xmlWriter.WriteAttributeString("MaxRecordsToReturn", this.MaxRecordsToReturn.ToString());
            xmlWriter.WriteAttributeString("CalculateScore", this.CalculateScore.ToString());
            xmlWriter.WriteAttributeString("SearchDepth", this.SearchDepth.ToString());
            xmlWriter.WriteAttributeString("SearchInTaxonomy", this.SearchInTaxonomy.ToString());
            xmlWriter.WriteAttributeString("IncludeCategoryPathInResult", this.IncludeCategoryPathInResult.ToString());
            //xmlWriter.WriteAttributeString("ReturnAttributeIdList", ValueTypeHelper.JoinCollection(this.ReturnAttributeIdList, ","));

            #region Generate ReturnAttributeId

            //Start of ReturnAttibuteList
            xmlWriter.WriteStartElement("ReturnAttibuteList");
            foreach (Attribute attribute in ReturnAttributeList)
            {
                //Start of Attribute
                xmlWriter.WriteStartElement("Attribute");
                xmlWriter.WriteAttributeString("Id", attribute.Id.ToString());
                xmlWriter.WriteAttributeString("Locale", attribute.Locale.ToString());
                xmlWriter.WriteAttributeString("Name", attribute.Name);

                //End of Attribute
                xmlWriter.WriteEndElement();
            }

            //End of ReturnAttibuteList
            xmlWriter.WriteEndElement();

            #endregion Generate ReturnAttributeId
            //Search Specifications node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //Get the actual XML
            searchSpecificationsXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return searchSpecificationsXml;
        }
        
        /// <summary>
        /// Set ReturnAttributeList
        /// </summary>
        /// <param name="iReturnAttributeList"></param>
        public void SetReturnAttributeList(Collection<IAttribute> iReturnAttributeList)
        {
            foreach (IAttribute iAttribute in iReturnAttributeList)
            {
                Attribute attribute = (Attribute)iAttribute;
                this.ReturnAttributeList.Add(attribute);
            }
        }

        /// <summary>
        /// Return ReturnAttributeList
        /// </summary>
        /// <returns></returns>
        public Collection<IAttribute> GetReturnAttributeList()
        {
            Collection<IAttribute> iAttributeColleciton = new Collection<IAttribute>();

            foreach (Attribute attribute in this.ReturnAttributeList)
            {
                IAttribute iAttribute = (IAttribute)attribute;
                iAttributeColleciton.Add(iAttribute);
            }

            return iAttributeColleciton;
        }

        /// <summary>
        /// Gets the return attribute id list 
        /// </summary>
        /// <returns></returns>
        public Collection<Int32> GetReturnAttributeIdList()
        {
            Collection<Int32> attributeIdList = null;

            if (this._returnAttributeList != null && this._returnAttributeList.Count > 0)
            {
                attributeIdList = new Collection<Int32>();

                foreach (Attribute attribute in this.ReturnAttributeList)
                {
                    if (!attributeIdList.Contains(attribute.Id))
                    {
                        attributeIdList.Add(attribute.Id);
                    }
                }
            }

            return attributeIdList;
        }

        /// <summary>
        /// Gets the return attribute locale list
        /// </summary>
        /// <returns></returns>
        public Collection<LocaleEnum> GetReturnAttributeLocaleList()
        {
            Collection<LocaleEnum> attributeLocaleList = null;

            if (this._returnAttributeList != null && this._returnAttributeList.Count > 0)
            {
                attributeLocaleList = new Collection<LocaleEnum>();

                foreach (Attribute attribute in this.ReturnAttributeList)
                {
                    if (!attributeLocaleList.Contains(attribute.Locale))
                    {
                        attributeLocaleList.Add(attribute.Locale);
                    }
                }
            }

            return attributeLocaleList;
        }

        #region Private Methods

        /// <summary>
        /// Initialize current object through Xml
        /// </summary>
        /// <param name="valuesAsXml">Xml having value of SearchContext
        /// <para>
        /// Sample Xml
        /// <![CDATA[
        /// <SearchSpecifications MaxRecordsToReturn="5000" CalculateScore="False" ResturnAttributeIdList="8526,3003,82,84" />
        /// ]]>
        /// </para>
        /// </param>
        private void LoadSearchContext(String valuesAsXml)
        {
            #region Sample Xml
            /*
            * <SearchSpecifications MaxRecordsToReturn="5000" CalculateScore="False" ResturnAttributeIdList="8526,3003,82,84" IncludeCategoryPathInResult="False"/>
            */
            #endregion Sample Xml

            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "SearchSpecifications")
                        {
                            #region Read Search Context

                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("MaxRecordsToReturn"))
                                {
                                    this.MaxRecordsToReturn = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                                }
                                if (reader.MoveToAttribute("CalculateScore"))
                                {
                                    this.CalculateScore = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
                                }
                                if (reader.MoveToAttribute("SearchDepth"))
                                {
                                    this.SearchDepth = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                                }
                                if (reader.MoveToAttribute("SearchIntaxonomy"))
                                {
                                    this.SearchInTaxonomy = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
                                }
                                if (reader.MoveToAttribute("IncludeCategoryPathInResult"))
                                {
                                    this.IncludeCategoryPathInResult = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
                                }
                            }

                            #endregion Read Search Context
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "Attribute")
                        {
                            //Read attributes
                            #region Read attributes

                            if (reader.HasAttributes)
                            {
                                Int32 attributeId = -1;
                                LocaleEnum locale = LocaleEnum.UnKnown;
                                String attributeName = String.Empty;

                                if (reader.MoveToAttribute("Id"))
                                {
                                    attributeId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                                }
                                if (reader.MoveToAttribute("Name"))
                                {
                                    attributeName = reader.ReadContentAsString();
                                }
                                if (reader.MoveToAttribute("Locale"))
                                {
                                    LocaleEnum.TryParse(reader.ReadContentAsString(), out locale);
                                }

                                Attribute attribute = new Attribute();
                                attribute.Id = attributeId;
                                attribute.Name = attributeName;
                                attribute.Locale = locale;

                                if (!this.ReturnAttributeList.Contains(attribute))
                                {
                                    this.ReturnAttributeList.Add(attribute);
                                }
                            }

                            #endregion Read attributes
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

        #endregion Prviate Methods

        #endregion Methods
    }
}