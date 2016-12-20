using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;
using System.Collections.ObjectModel;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Represents the class that contains collection of MDMRuleMapRules
    /// </summary>
    [DataContract]
    public class MDMRuleMapRuleCollection : InterfaceContractCollection<IMDMRuleMapRule, MDMRuleMapRule>, IMDMRuleMapRuleCollection, IBusinessRuleObjectCollection
    {
        #region Fields
        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mdmRuleMapName"></param>
        /// <returns></returns>
        public MDMRuleMapRuleCollection this[String mdmRuleMapName]
        {
            get
            {
                return FilterMDMRuleMapRulesByRuleMapName(mdmRuleMapName) as MDMRuleMapRuleCollection;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mdmRuleMapId"></param>
        /// <param name="mdmRuleId"></param>
        /// <returns></returns>
        public MDMRuleMapRule this[Int32 mdmRuleMapId, Int32 mdmRuleId]
        {
            get
            {
                return GetMDMRuleMapRuleByRuleMapIdAndRuleId(mdmRuleMapId, mdmRuleId) as MDMRuleMapRule;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mdmRuleMapName"></param>
        /// <param name="mdmRuleName"></param>
        /// <param name="mdmRuleStatus"></param>
        /// <returns></returns>
        public MDMRuleMapRule this[String mdmRuleMapName, String mdmRuleName, RuleStatus mdmRuleStatus]
        {
            get
            {
                return GetMDMRuleMapRuleByRuleMapNameAndRuleName(mdmRuleMapName, mdmRuleName, mdmRuleStatus) as MDMRuleMapRule;
            }
        }

        #endregion Properties

        #region Constructor

        /// <summary>
        /// Default Constructor
        /// </summary>
        public MDMRuleMapRuleCollection()
        {
        }

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        public MDMRuleMapRuleCollection(String valuesAsXml)
        {
            LoadMDMRuleMapRuleCollection(valuesAsXml);
        }

        #endregion Constructor

        #region Methods

        #region Public Methods

        #region Override Methods

        /// <summary>
        /// Determines whether specified object is equal to the current object
        /// </summary>
        /// <param name="obj">MDMRuleMapRuleCollection object which needs to be compared</param>
        /// <returns>Result of the comparison in Boolean</returns>
        public override Boolean Equals(Object obj)
        {
            if (obj is MDMRuleMapRuleCollection)
            {
                MDMRuleMapRuleCollection objectToBeCompared = obj as MDMRuleMapRuleCollection;
                Int32 mdmRuleMapRulesUnion = this._items.ToList().Union(objectToBeCompared.ToList()).Count();
                Int32 mdmRuleMapRulesIntersect = this._items.ToList().Intersect(objectToBeCompared.ToList()).Count();

                if (mdmRuleMapRulesUnion != mdmRuleMapRulesIntersect)
                    return false;

                return true;
            }

            return false;
        }

        /// <summary>
        /// Returns the hash code for this instance
        /// </summary>
        /// <returns>Hash code of the instance</returns>
        public override Int32 GetHashCode()
        {
            int hashCode = 0;

            hashCode = base.GetHashCode();

            foreach (MDMRuleMapRule mdmRuleMapRule in this._items)
            {
                hashCode += mdmRuleMapRule.GetHashCode();
            }

            return hashCode;
        }

        /// <summary>
        /// Gets Xml representation for MDMRuleMapRule
        /// </summary>
        public String ToXml()
        {
            String outputXml = String.Empty;

            using (StringWriter sw = new StringWriter())
            {
                using (XmlTextWriter xmlWriter = new XmlTextWriter(sw))
                {
                    // MDMRuleMapRules node start
                    xmlWriter.WriteStartElement("MDMRuleMapRules");

                    foreach (MDMRuleMapRule mdmRuleMapRule in this._items)
                    {
                        xmlWriter.WriteRaw(mdmRuleMapRule.ToXml());
                    }

                    // MDMRuleMapRules node end
                    xmlWriter.WriteEndElement();
                }

                //Get the actual XML
                outputXml = sw.ToString();
            }

            return outputXml;
        }

        #endregion Override Methods

        #region IBusinessRuleObjectCollection Members

        /// <summary>
        /// Remove mdmrulemaprule from collection by reference id
        /// </summary>
        /// <param name="referenceId">Indicates the reference id</param>
        /// <returns>Returns true if mdmrulemaprule is successfully removed from the collection</returns>
        public Boolean RemoveByReferenceId(Int64 referenceId)
        {
            return true;
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An enumerator object that can be used to iterate through the collection.</returns>
        public new IEnumerator GetEnumerator()
        {
            return this._items.GetEnumerator();
        }

        #endregion IBusinessRuleObjectCollection Members

        #region IMDMRuleMapRuleCollection

        /// <summary>
        /// Filter MDMRuleMapRules by RuleMap Name
        /// </summary>
        /// <param name="ruleMapName">Indicates the rulemap name</param>
        /// <returns>Returns MDMRuleMapRules which contains the rulemap name provided by the user</returns>
        public IMDMRuleMapRuleCollection FilterMDMRuleMapRulesByRuleMapName(String ruleMapName)
        {
            MDMRuleMapRuleCollection mdmRuleMapRulesToReturn = new MDMRuleMapRuleCollection();

            if (!String.IsNullOrWhiteSpace(ruleMapName))
            {
                foreach (MDMRuleMapRule mdmRuleMapRule in this._items)
                {
                    if (String.Compare(mdmRuleMapRule.RuleMapName, ruleMapName, true) == 0 && !mdmRuleMapRulesToReturn.Contains(mdmRuleMapRule))
                    {
                        mdmRuleMapRulesToReturn.Add(mdmRuleMapRule);
                    }
                }
            }
            else
            {
                throw new ArgumentNullException("ruleMapName", "Parameter ruleMapName should not be null or empty.");
            }

            return mdmRuleMapRulesToReturn;
        }

        /// <summary>
        /// Get MDMRuleMapRule by RuleMap Id and RuleId
        /// </summary>
        /// <param name="ruleMapId">Indicates the rulemap id</param>
        /// <param name="ruleId">Indicates the rule id</param>
        /// <returns>MDMRuleMapRule</returns>
        public IMDMRuleMapRule GetMDMRuleMapRuleByRuleMapIdAndRuleId(Int32 ruleMapId, Int32 ruleId)
        {
            foreach (MDMRuleMapRule mdmRuleMapRule in this._items)
            {
                if (mdmRuleMapRule.RuleMapId == ruleMapId && mdmRuleMapRule.RuleId == ruleId)
                {
                    return mdmRuleMapRule;
                }
            }

            return null;

        }

        /// <summary>
        /// Get MDMRuleMapRule by the provided context
        /// </summary>
        /// <param name="ruleMapId">Indicates the rule map identifier</param>
        /// <param name="ruleId">Indicates the rule identifier</param>
        /// <param name="ruleStatus">Indicates the status of rule</param>
        /// <returns>Returns the requested MDM Rule Map Rule information</returns>
        public IMDMRuleMapRule GetMDMRuleMapRuleByRuleMapIdAndRuleId(Int32 ruleMapId, Int32 ruleId, RuleStatus ruleStatus)
        {
            //If the ruleStatus is Unknown, we are not considering it currently for adding in collection for further operation. Need to check it when required.
            MDMRuleMapRule mdmRuleMapRuleToReturn = null;

            if (this._items != null && this._items.Count > 0)
            {
                foreach (MDMRuleMapRule mdmRuleMapRule in this._items)
                {
                    if (mdmRuleMapRule.RuleMapId == ruleMapId && mdmRuleMapRule.RuleId == ruleId && mdmRuleMapRule.RuleStatus == ruleStatus)
                    {
                        mdmRuleMapRuleToReturn = mdmRuleMapRule;
                        break;
                    }
                }
            }
            return mdmRuleMapRuleToReturn;
        }

        /// <summary>
        /// Get MDMRuleMapRule by RuleMap Name, RuleName and RuleStatus
        /// </summary>
        /// <param name="ruleMapName">Indicates the rulemap name</param>
        /// <param name="ruleName">Indicates the rule name</param>
        /// <param name="ruleStatus">Indicates the rule status</param>
        /// <returns>MDMRuleMapRule</returns>
        public IMDMRuleMapRule GetMDMRuleMapRuleByRuleMapNameAndRuleName(String ruleMapName, String ruleName, RuleStatus ruleStatus)
        {
            MDMRuleMapRule mdmRuleMapRuleToReturn = new MDMRuleMapRule();

            if (!String.IsNullOrWhiteSpace(ruleMapName) && !String.IsNullOrWhiteSpace(ruleName))
            {
                foreach (MDMRuleMapRule mdmRuleMapRule in this._items)
                {
                    if (String.Compare(mdmRuleMapRule.RuleMapName, ruleMapName) == 0 && String.Compare(mdmRuleMapRule.RuleName, ruleName) == 0 && mdmRuleMapRule.RuleStatus == ruleStatus)
                    {
                        mdmRuleMapRuleToReturn = mdmRuleMapRule;
                        break;
                    }
                }
            }
            else
            {
                throw new ArgumentNullException();
            }

            return mdmRuleMapRuleToReturn;
        }

        #endregion IMDMRuleMapRuleCollection

        #endregion Public Methods

        #region Private Methods

        private void LoadMDMRuleMapRuleCollection(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                using (XmlTextReader reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null))
                {
                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "MDMRuleMapRule")
                        {
                            String mdmRuleMapRulesXml = reader.ReadOuterXml();

                            if (!String.IsNullOrWhiteSpace(mdmRuleMapRulesXml))
                            {
                                MDMRuleMapRule mdmRuleMapRule = new MDMRuleMapRule(mdmRuleMapRulesXml);

                                if (mdmRuleMapRule != null)
                                {
                                    this.Add(mdmRuleMapRule);
                                }
                            }
                        }
                        else
                        {
                            //Keep on reading the xml until we reach expected node.
                            reader.Read();
                        }
                    }
                }
            }
        }

        #endregion Private Methods

        #endregion Methods
    }
}