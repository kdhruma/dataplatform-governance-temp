using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Threading;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    ///<summary>
    ///Specifies the specifications for search 
    ///</summary>
    [DataContract]
    public class SearchValidationStatesRule : MDMObject, ISearchValidationStatesRule
    {
        #region Fields

        /// <summary>
        /// Stores the value thats needs to checked for validation state view attributes
        /// </summary>
        private ValidityStateValue _value = ValidityStateValue.NotChecked;

        /// <summary>
        /// Specifies the attribute for which rule is defined
        /// </summary>
        private SystemAttributes _attributeId = SystemAttributes.EntityCommonAttributesValid;

        /// <summary>
        /// Represents search operator for rule
        /// </summary>
        private SearchOperator _operator = SearchOperator.None;
        
        #endregion

        #region Properties

        /// <summary>
        /// Specifies the attribute for which rule is defined
        /// </summary>
        [DataMember]
        public SystemAttributes AttributeId
        {
            get
            {
                return _attributeId;
            }
            set
            {
                _attributeId = value;
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
        /// value of the validation state
        /// </summary>
        [DataMember]
        public ValidityStateValue Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the Search ValidationStates Rule class.
        /// </summary>
        public SearchValidationStatesRule()
            : base()
        {

        }
         
        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeId"></param>
        /// <param name="searchOperator"></param>
        public SearchValidationStatesRule(SystemAttributes attributeId, SearchOperator searchOperator)
        {
            this.AttributeId = attributeId;
            this.Operator = searchOperator;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeId"></param> 
        /// <param name="searchOperator"></param>
        /// <param name="value"></param>
        public SearchValidationStatesRule(SystemAttributes attributeId, SearchOperator searchOperator, ValidityStateValue value)
        {
            this.AttributeId = attributeId;
            this.Operator = searchOperator;
            this.Value = value;
        }

         
        /// <summary>
        /// Xml Constructor
        /// </summary>
        /// <param name="valuesAsXml">Xml containing value of current SearchValidationStatesRule object</param>
        public SearchValidationStatesRule(String valuesAsXml)
        {
            LoadSearchValidationStatesRule(valuesAsXml);
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
                if (obj is SearchValidationStatesRule)
                {
                    SearchValidationStatesRule objectToBeCompared = obj as SearchValidationStatesRule;

                    if (!this.AttributeId.Equals(objectToBeCompared.AttributeId))
                        return false;

                    if (!this.Value.Equals(objectToBeCompared.Value))
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
            Int32 hashCode = base.GetHashCode() ^ this.AttributeId.GetHashCode() ^ this.Value.GetHashCode() ^ this.Operator.GetHashCode();

            return hashCode;
        }
        
        /// <summary>
        /// Get Xml representation of Search Attribute Rule
        /// </summary>
        /// <returns>Xml representation of Search Attribute Rule</returns>
        /// <exception cref="ArgumentNullException">Thrown when Attribute object passed is null</exception>
        public override String ToXml()
        {
            String searchValidationStatesRuleXml = String.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);
             
            xmlWriter.WriteStartElement("SearchValidationStatesRule");

            xmlWriter.WriteAttributeString("AttributeID", this.AttributeId.ToString());
            if (this.Operator != SearchOperator.None)
            {
                xmlWriter.WriteAttributeString("Operator", Utility.GetOperatorStringUnescaped(this.Operator));
            }
             
            xmlWriter.WriteAttributeString("Value", this.Value.ToString());

            xmlWriter.WriteEndElement();


            xmlWriter.Flush();

            //Get the actual XML
            searchValidationStatesRuleXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return searchValidationStatesRuleXml;
        }

        /// <summary>
        /// Load current SearchAttributeRule from Xml
        /// </summary>
        /// <param name="valuesAsXml">Xml having value of SearchAttributeRule
        /// <para>
        /// Sample Xml
        /// <![CDATA[
        /// <SearchValidationStatesRule IsCategoryAttributesValid="" IsCommonAttributesValid=""  Operator="And"/>
        /// ]]>
        /// </para>
        /// </param>
        public void LoadSearchValidationStatesRule(String valuesAsXml)
        {
            #region Sample Xml
            /*
            *   <SearchValidationStatesRule IsCategoryAttributesValid="" IsCommonAttributesValid=""  Operator="And"/>
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
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "SearchValidationStatesRule")
                        {
                            #region Search validationStates rule 

                            if (reader.HasAttributes)
                            {

                                if (reader.MoveToAttribute("AttributeID"))
                                {
                                    SystemAttributes localAttributeId = SystemAttributes.EntityCommonAttributesValid;
                                    if (ValueTypeHelper.EnumTryParse(reader.ReadContentAsString(), true, out localAttributeId))
                                    {
                                        this.AttributeId = localAttributeId;
                                    } 
                                }
                                if (reader.MoveToAttribute("Operator"))
                                {
                                    this.Operator = Utility.GetOperatorEnum(reader.ReadContentAsString());
                                }

                                if (reader.MoveToAttribute("Value"))
                                {
                                    String validationStateValue = reader.ReadContentAsString();

                                    if (!String.IsNullOrEmpty(validationStateValue))
                                    {
                                        ValidityStateValue stateValue;

                                        this.Value = Enum.TryParse(validationStateValue, out stateValue) ? stateValue : ValidityStateValue.NotChecked;
                                    }
                                } 
                                 
                            }
                            else 
                            {
                                //While old add rule xml come, Attribute node will not have any attributes hence continue reading....
                                reader.Read();
                            }

                            #endregion Search validaiton states rule
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

        #endregion
    }
}
