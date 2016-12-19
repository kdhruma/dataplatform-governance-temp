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
    public class SearchMDMRuleRule : MDMObject, ISearchMDMRuleRule
    {
        #region Fields

        /// <summary>
        /// Specifies the MDMRule for which rule is defined
        /// </summary>
        private MDMRule _MDMRule = new MDMRule();

        /// <summary>
        /// Represents search operator for rule
        /// </summary>
        private SearchOperator _operator = SearchOperator.None;

        /// <summary>
        /// Stores the condition value of the business condition rule
        /// </summary>
        private Collection<String>  _conditions = new Collection<String>();
        
        #endregion

        #region Properties

        /// <summary>
        /// Specifies the MDMRule for which rule is defined
        /// </summary>
        [DataMember]
        public MDMRule MDMRule
        {
            get
            {
                return _MDMRule;
            }
            set
            {
                _MDMRule = value;
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
        /// Presents the value of the businessCondition
        /// </summary>
        [DataMember]
        public Collection<String> Conditions
        {
            get
            {
                return _conditions;
            }
            set
            {
                _conditions = value;
            }
        }


        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the Search MDMRule Rule class.
        /// </summary>
        public SearchMDMRuleRule()
            : base()
        {

        }

        /// <summary>
        /// Initializes a new instance of the Search Validation States Rule class.
        /// </summary>
        /// <param name="MDMRule"></param>
        /// <param name="searchOperator"></param>
        public SearchMDMRuleRule(MDMRule MDMRule, SearchOperator searchOperator)
        {
            this.MDMRule = MDMRule;
            this.Operator = searchOperator;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="MDMRule"></param>
        /// <param name="searchOperator"></param>
        /// <param name="conditions"></param>
        public SearchMDMRuleRule(MDMRule MDMRule, SearchOperator searchOperator,Collection<String> conditions)
        {
            this.MDMRule = MDMRule;
            this.Operator = searchOperator;
            this.Conditions = conditions;
        }

        /// <summary>
        /// Xml Constructor
        /// </summary>
        /// <param name="valuesAsXml">Xml containing value of current SearchMDMRuleRule object</param>
        public SearchMDMRuleRule(String valuesAsXml)
        {
            LoadSearchMDMRuleRule(valuesAsXml);
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
                if (obj is SearchMDMRuleRule)
                {
                    SearchMDMRuleRule objectToBeCompared = obj as SearchMDMRuleRule;

                    if (!this.MDMRule.Equals(objectToBeCompared.MDMRule))
                    {
                        return false;
                    }

                    if (this.Operator != objectToBeCompared.Operator)
                    {
                        return false;
                    }

                    if (this.Conditions != objectToBeCompared.Conditions)
                    {
                        return false;
                    }

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
            Int32 hashCode = base.GetHashCode() ^ this.MDMRule.GetHashCode() ^ this.Operator.GetHashCode() ^ this.Conditions.GetHashCode();

            return hashCode;
        }
        
        /// <summary>
        /// Get Xml representation of Search Attribute Rule
        /// </summary>
        /// <returns>Xml representation of Search Attribute Rule</returns>
        /// <exception cref="ArgumentNullException">Thrown when Attribute object passed is null</exception>
        public override String ToXml()
        {
            if (this.MDMRule == null)
            {
                throw new ArgumentNullException("MDMRule Object");
            }
            
            String searchMDMRuleRuleXml = String.Empty;

            using (StringWriter sw = new StringWriter())
            {
                using (XmlTextWriter xmlWriter = new XmlTextWriter(sw))
                {


                    xmlWriter.WriteStartElement("SearchMDMRuleRule");

                    if (this.Operator != SearchOperator.None)
                    {
                        xmlWriter.WriteAttributeString("Operator", Utility.GetOperatorStringUnescaped(this.Operator));
                    }

                    if (this.MDMRule != null)
                    {
                        xmlWriter.WriteAttributeString("RuleId", MDMRule.Id.ToString());
                        xmlWriter.WriteAttributeString("RuleName", MDMRule.Name);
                        xmlWriter.WriteAttributeString("RuleType", MDMRule.RuleType.ToString());

                        String conditions = String.Empty;

                        if ((Conditions != null) && Conditions.Any())
                        {
                            conditions = String.Join(",", Conditions);
                        }
                        xmlWriter.WriteAttributeString("Conditions", conditions);
                    }

                    xmlWriter.WriteEndElement();

                    xmlWriter.Flush();

                    //Get the actual XML
                    searchMDMRuleRuleXml = sw.ToString();
                }
            }

            return searchMDMRuleRuleXml;
        }

        /// <summary>
        /// Load current SearchAttributeRule from Xml
        /// </summary>
        /// <param name="valuesAsXml">Xml having value of SearchAttributeRule
        /// <para>
        /// Sample Xml
        /// <![CDATA[
        ///  <SearchMDMRuleRule RuleID="" RuleName="Promote"  Operator="And" Condition="Success">
        ///  </SearchMDMRuleRule>
        /// ]]>
        /// </para>
        /// </param>
        public void LoadSearchMDMRuleRule(String valuesAsXml)
        {
            #region Sample Xml
            /*
            *   <SearchMDMRuleRule RuleID="" RuleName="Promote"  Operator="And" Condition="Success" >            *      
            *   </SearchMDMRuleRule>
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
                        #region Search MDMRule rule 
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "SearchMDMRuleRule")
                        {
                           if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("RuleId"))
                                {
                                    this.MDMRule.Id = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                                }

                                if (reader.MoveToAttribute("RuleName"))
                                {
                                    this.MDMRule.Name = reader.ReadContentAsString();
                                }
                                 
                                if (reader.MoveToAttribute("RuleType"))
                                {
                                    MDMRuleType value = MDMRuleType.BusinessCondition;
                                    if (ValueTypeHelper.EnumTryParse(reader.ReadContentAsString(), true, out value))
                                    {
                                        this.MDMRule.RuleType = value;
                                    }
                                } 
                                
                                if (reader.MoveToAttribute("Operator"))
                                {
                                    this.Operator = Utility.GetOperatorEnum(reader.ReadContentAsString());
                                }

                                if (reader.MoveToAttribute("Conditions"))
                                {
                                    String conditions = reader.ReadContentAsString();

                                    if (!String.IsNullOrEmpty(conditions))
                                    {
                                        this.Conditions = new Collection<string>(conditions.Split(','));
                                    }
                                } 
                            }
                            else
                            {
                                reader.Read();
                            }

                        }
                        else
                        {
                            reader.Read();
                        }

                        #endregion Search validaiton states rule
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
