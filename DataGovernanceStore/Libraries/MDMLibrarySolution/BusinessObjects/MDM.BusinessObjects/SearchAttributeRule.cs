using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
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
    public class SearchAttributeRule : MDMObject, ISearchAttributeRule, ICloneable
    {
        #region Fields

        /// <summary>
        /// Specifies the attribute for which rule is defined
        /// </summary>
        private Attribute _attribute = new Attribute();

        /// <summary>
        /// Represents search operator for rule
        /// </summary>
        private SearchOperator _operator = SearchOperator.None;

        /// <summary>
        /// Represents search value for rule
        /// </summary>
        private String _displaySearchValue = String.Empty;

        ///// <summary>
        ///// Represents version of workflow
        ///// </summary>
        //private Int32 _workflowVersion = 0;

        #endregion

        #region Properties

        /// <summary>
        /// Specifies the attribute for which rule is defined
        /// </summary>
        [DataMember]
        public Attribute Attribute
        {
            get
            {
                return _attribute;
            }
            set
            {
                _attribute = value;
            }
        }

        /// <summary>
        /// Represents rule operator for search
        /// </summary>
        [DataMember]
        public SearchOperator Operator
        {
            get
            {
                return _operator;
            }
            set
            {
                _operator = value;
            }
        }

        /// <summary>
        /// Represents search value for rule
        /// </summary>
        [DataMember]
        public String DisplaySearchValue
        {
            set
            {
                this._displaySearchValue = value;
            }
            get
            {
                return this._displaySearchValue;
            }
        }

        ///// <summary>
        ///// Represents version of workflow
        ///// </summary>
        //[DataMember]
        //public Int32 WorkflowVersion
        //{
        //    get
        //    {
        //        return _workflowVersion;
        //    }
        //    set
        //    {
        //        _workflowVersion = value;
        //    }
        //}

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the Search Attribute Rule class.
        /// </summary>
        public SearchAttributeRule()
            : base()
        {

        }

        /// <summary>
        /// Initializes a new instance of the Search Attribute Rule class.
        /// </summary>
        /// <param name="attribute"></param>
        /// <param name="searchOperator"></param>
        public SearchAttributeRule(Attribute attribute, SearchOperator searchOperator)
        {
            this.Attribute = attribute;
            this.Operator = searchOperator;
        }

        /// <summary>
        /// Initializes a new instance of the Search Attribute Rule class.
        /// </summary>
        /// <param name="attributeId">Id of the search attribute</param>
        /// <param name="attributeModelType">Attribute model type</param>
        /// <param name="searchValue">Search value</param>
        /// <param name="searchOperator">Search operator</param>
        public SearchAttributeRule(Int32 attributeId, AttributeModelType attributeModelType, Object searchValue, SearchOperator searchOperator)
        {
            this.Attribute = new Attribute(attributeId, "", "", attributeModelType, searchValue);
            this.Operator = searchOperator;
        }

        /// <summary>
        /// Xml Constructor
        /// </summary>
        /// <param name="valuesAsXml">Xml containing value of current SearchAttributeRule object</param>
        public SearchAttributeRule(String valuesAsXml)
        {
            LoadSearchAttributeRule(valuesAsXml);
        }

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>True if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
            {
                if (obj is SearchAttributeRule)
                {
                    SearchAttributeRule objectToBeCompared = obj as SearchAttributeRule;

                    if (!this.Attribute.Equals(objectToBeCompared.Attribute))
                        return false;

                    if (this.Operator != objectToBeCompared.Operator)
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
        public override int GetHashCode()
        {
            Int32 hashCode = base.GetHashCode() ^ this.Attribute.GetHashCode() ^ this.Operator.GetHashCode();

            return hashCode;
        }

        /// <summary>
        /// Clones SearchAttributeRule
        /// </summary>
        /// <returns>A cloned SearchAttributeRule</returns>
        public object Clone()
        {
            SearchAttributeRule clonedSearchAttributeRule = new SearchAttributeRule();
            clonedSearchAttributeRule.Attribute = this.Attribute.Clone();
            clonedSearchAttributeRule.Operator = this.Operator;
            clonedSearchAttributeRule.DisplaySearchValue = this.DisplaySearchValue;

            return clonedSearchAttributeRule;
        }

        /// <summary>
        /// Get Xml representation of Search Attribute Rule
        /// </summary>
        /// <returns>Xml representation of Search Attribute Rule</returns>
        /// <exception cref="ArgumentNullException">Thrown when Attribute object passed is null</exception>
        public override String ToXml()
        {
            if (this.Attribute == null)
                throw new ArgumentNullException("Attribute Object");

            //Get the value object
            ValueCollection values = this.Attribute.CurrentValues;
            StringBuilder sbValue = new StringBuilder();

            if (values != null && values.Count > 0)
            {
                foreach(Value value in values)
                {
                    sbValue.Append(String.Format("{0}{1}",value.GetStringValue(), ","));
                }
            }

            String searchAttributeRuleXml = String.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            // TODO - Remove with N hibernate cleanup

            //Common or category attribute
            //Format - '<Attribute AttributeID="4141" Value="ss" Operator="in" />'
            xmlWriter.WriteStartElement("Attribute");

            xmlWriter.WriteAttributeString("AttributeID", this.Attribute.Id.ToString());
            xmlWriter.WriteAttributeString("Value", sbValue.ToString().Trim(',')); //Assuming that values requested for search are passed as string
            xmlWriter.WriteAttributeString("Locale", this.Attribute.Locale.ToString());
            xmlWriter.WriteAttributeString("DisplaySearchValue", this.DisplaySearchValue);

            if (this.Operator != SearchOperator.None)
            {
                xmlWriter.WriteAttributeString("Operator", Utility.GetOperatorStringUnescaped(this.Operator));
            }

            xmlWriter.WriteEndElement();


            xmlWriter.Flush();

            //Get the actual XML
            searchAttributeRuleXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return searchAttributeRuleXml;
        }

        /// <summary>
        /// Load current SearchAttributeRule from Xml
        /// </summary>
        /// <param name="valuesAsXml">Xml having value of SearchAttributeRule
        /// <para>
        /// Sample Xml
        /// <![CDATA[
        /// System Attribute : <SystemAttribute WhereClause="##84##='s*'" />
        /// Common / Technical Attribute : <Attribute AttributeId="24" Value="aa" Operator="like"/>
        /// ]]>
        /// </para>
        /// </param>
        public void LoadSearchAttributeRule(String valuesAsXml)
        {
            #region Sample Xml
            /*
            *   System Attribute : <SystemAttribute WhereClause="##84##='s*'" />
                Common / Technical Attribute : <Attribute AttributeId="24" Value="aa" Operator="like"/>
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
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "Attribute")
                        {
                            #region Search attribute rule (Common/Technical attribute)

                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("AttributeID"))
                                {
                                    this.Attribute.Id = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                                }
                                if (reader.MoveToAttribute("Value"))
                                {
                                    this.Attribute.SourceFlag = AttributeValueSource.Overridden;
                                    this.Attribute.SetValue(reader.ReadContentAsObject());
                                }
                                if (reader.MoveToAttribute("Locale"))
                                {
                                    String localeName = reader.ReadContentAsString();
                                    LocaleEnum locale = LocaleEnum.UnKnown;
                                    LocaleEnum.TryParse(localeName, out locale);
                                    this.Attribute.Locale = locale;
                                }
                                if (reader.MoveToAttribute("Operator"))
                                {
                                    this.Operator = Utility.GetOperatorEnum(reader.ReadContentAsString());
                                }
                                if (reader.MoveToAttribute("DisplaySearchValue"))
                                {
                                    this.DisplaySearchValue = reader.ReadContentAsString();
                                }
                            }
                            else 
                            {
                                //While old add rule xml come, Attribute node will not have any attributes hence continue reading....
                                reader.Read();
                            }

                            #endregion Search attribute rule (Common/Technical attribute)
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "SystemAttribute")
                        {
                            #region Search attribute rule (Common/Technical attribute)

                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("AttributeID"))
                                {
                                    this.Attribute.Id = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                                }
                                if (reader.MoveToAttribute("Value"))
                                {
                                    this.Attribute.SourceFlag = AttributeValueSource.Overridden;
                                    this.Attribute.SetValue(reader.ReadContentAsObject());
                                }
                                if (reader.MoveToAttribute("Locale"))
                                {
                                    String localeName = reader.ReadContentAsString();
                                    LocaleEnum locale = LocaleEnum.UnKnown;
                                    LocaleEnum.TryParse(localeName, out locale);
                                    this.Attribute.Locale = locale;
                                }
                                if (reader.MoveToAttribute("Operator"))
                                {
                                    this.Operator = Utility.GetOperatorEnum(reader.ReadContentAsString());
                                }
                                if (reader.MoveToAttribute("DisplaySearchValue"))
                                {
                                    this.DisplaySearchValue = reader.ReadContentAsString();
                                }
                            }

                            #endregion Search attribute rule (Common/Technical attribute)
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

        #region Private Methods

        #endregion

        #region ISearchAttributeRule methods

        /// <summary>
        /// Get Attribute for current search rule
        /// </summary>
        /// <returns>Attribute interface</returns>
        /// <exception cref="NullReferenceException">Thrown when Attribute is null</exception>
        public IAttribute GetAttribute()
        {
            if (this.Attribute == null)
            {
                throw new NullReferenceException("Attribute is null");
            }
            return (IAttribute)this.Attribute;
        }

        /// <summary>
        /// Set attribute for current search rule
        /// </summary>
        /// <param name="attribute">Attribute to set</param>
        /// <exception cref="ArgumentNullException">Thrown if attribute is null</exception>
        public void SetAttribute(IAttribute attribute)
        {
            if (attribute == null)
            {
                throw new ArgumentNullException("Attribute");
            }

            this.Attribute = (Attribute)attribute;
        }

        #endregion ISearchAttributeRule methods

        #endregion
    }
}
