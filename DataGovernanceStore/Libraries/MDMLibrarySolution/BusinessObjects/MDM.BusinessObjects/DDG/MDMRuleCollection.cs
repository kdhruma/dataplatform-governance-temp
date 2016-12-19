using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Represents the class that contains collection of MDMRules
    /// </summary>
    [DataContract]
    public class MDMRuleCollection : IMDMRuleCollection, ICloneable, IBusinessRuleObjectCollection
    {
        #region Fields

        /// <summary>
        /// Field for MDMRule collection
        /// </summary>
        [DataMember]
        private Collection<MDMRule> _mdmRuleCollection = new Collection<MDMRule>();

        #endregion Fields

        #region Properties

        #region IEnumerable Members

        /// <summary>
        /// Gets the number of elements contained in MDMRuleCollection
        /// </summary>
        public Int32 Count
        {
            get { return this._mdmRuleCollection.Count; }
        }

        /// <summary>
        /// Check if MDMRuleCollection is read-only
        /// </summary>
        public Boolean IsReadOnly
        {
            get { return false; }
        }

        #endregion IEnumerable Members

        #region IMDMRuleCollection Members

        /// <summary>
        /// Get all published rules which are available in the current collection
        /// </summary>
        /// <returns>Returns the published MDMRules</returns>
        public MDMRuleCollection PublishedBusinessRules
        {
            get
            {
                return GetAllBusinessRules(RuleStatus.Published) as MDMRuleCollection;
            }
        }

        /// <summary>
        /// Get all published business conditions which are available in the current collection
        /// </summary>
        /// <returns>Returns the published Business Conditions</returns>
        public MDMRuleCollection PublishedBusinessConditions
        {
            get
            {
                return GetAllBusinessConditions(RuleStatus.Published) as MDMRuleCollection;
            }
        }

        /// <summary>
        /// Get all draft rules which are available in the current collection
        /// </summary>
        /// <returns>Returns the published MDMRules</returns>
        public MDMRuleCollection DraftBusinessRules
        {
            get
            {
                return GetAllBusinessRules(RuleStatus.Draft) as MDMRuleCollection;
            }
        }

        /// <summary>
        /// Get all draft business conditions which are available in the current collection
        /// </summary>
        /// <returns>Returns the published Business Conditions</returns>
        public MDMRuleCollection DraftBusinessConditions
        {
            get
            {
                return GetAllBusinessConditions(RuleStatus.Draft) as MDMRuleCollection;
            }
        }

        /// <summary>
        /// Get all business rules which are available in the current collection
        /// </summary>
        /// <returns>Returns the business rules</returns>
        public MDMRuleCollection BusinessRules
        {
            get
            {
                return GetAllBusinessRules() as MDMRuleCollection;
            }
        }

        /// <summary>
        /// Get all business conditions which are available in the current collection
        /// </summary>
        /// <returns>Returns the business conditions</returns>
        public MDMRuleCollection BusinessConditions
        {
            get
            {
                return GetAllBusinessConditions() as MDMRuleCollection;
            }
        }

        /// <summary>
        /// Get all published (business rules + business conditions) which are available in the current collection
        /// </summary>
        /// <returns>Returns all published (business rules + business conditions)</returns>
        public MDMRuleCollection AllPublishedRules
        {
            get
            {
                return GetMDMRulesByStatus(RuleStatus.Published) as MDMRuleCollection;
            }
        }

        /// <summary>
        /// Get all draft (business rules + business conditions) which are available in the current collection
        /// </summary>
        /// <returns>Returns all draft (business rules + business conditions)</returns>
        public MDMRuleCollection AllDraftRules
        {
            get
            {
                return GetMDMRulesByStatus(RuleStatus.Draft) as MDMRuleCollection;
            }
        }

        #endregion IMDMRuleCollection Members

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public MDMRuleCollection()
        { }

        /// <summary>
        /// Initialize MDMRuleCollection from MDMRuleList
        /// </summary>
        /// <param name="mdmRuleList">Source items</param>
        public MDMRuleCollection(Collection<MDMRule> mdmRuleList)
        {
            if (mdmRuleList != null)
            {
                this._mdmRuleCollection = mdmRuleList;
            }
        }

        /// <summary>
        /// Parameterised constructor having object values in xml format
        /// Populates current object using XML
        /// </summary>
        /// <param name="valueAsXml">Indicates xml representation of MDMRuleCollection</param>
        public MDMRuleCollection(String valueAsXml)
        {
            LoadMDMRuleCollectionFromXml(valueAsXml);
        }

        #endregion Constructors

        #region Methods

        #region Public Methods

        #region Get Methods

        /// <summary>
        /// Returns MDMRule based on the ruleid from MDMRuleCollection
        /// </summary>
        /// <param name="mdmRuleId">Id of rule</param>
        /// <returns>Returns MDMRule if exists, else null</returns>
        public MDMRule GetMDMRuleById(Int32 mdmRuleId)
        {
            MDMRule result = null;

            if (mdmRuleId > 0)
            {
                foreach (MDMRule mdmRule in _mdmRuleCollection)
                {
                    if (mdmRule.Id == mdmRuleId)
                    {
                        result = mdmRule;
                        break;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Returns MDMRule based on the referenceId from MDMRuleCollection
        /// </summary>
        /// <param name="referenceId">Indicates the referenceId of rule</param>
        /// <returns>Returns MDMRule if exists, else null</returns>
        public MDMRule GetMDMRuleByReferenceId(Int32 referenceId)
        {
            MDMRule result = null;

            foreach (MDMRule mdmRule in _mdmRuleCollection)
            {
                if (mdmRule.ReferenceId == referenceId)
                {
                    result = mdmRule;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Returns MDMRule based on the rule name from MDMRuleCollection
        /// </summary>
        /// <param name="mdmRuleName">Name of rule</param>
        /// <returns>Returns MDMRule if exists, else null</returns>
        public MDMRule GetMDMRuleByName(String mdmRuleName)
        {
            MDMRule result = null;

            if (!String.IsNullOrWhiteSpace(mdmRuleName))
            {
                foreach (MDMRule mdmRule in _mdmRuleCollection)
                {
                    if (String.Compare(mdmRule.Name, mdmRuleName) == 0)
                    {
                        result = mdmRule;
                        break;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Returns MDMRule based on the rule name from MDMRuleCollection
        /// </summary>
        /// <param name="mdmRuleName">Name of rule</param>
        /// <param name="ruleStatus">Indicates whether to returns Draft copy or Publish version</param>
        /// <returns>Returns MDMRule if exists, else null</returns>
        public MDMRule GetMDMRuleByNameAndStatus(String mdmRuleName, RuleStatus ruleStatus = RuleStatus.Draft)
        {
            MDMRule result = null;

            if (!String.IsNullOrWhiteSpace(mdmRuleName))
            {
                String mdmRuleNameTrimmed = mdmRuleName.Trim();
                foreach (MDMRule mdmRule in _mdmRuleCollection)
                {
                    if (String.Compare(mdmRule.Name.Trim(), mdmRuleNameTrimmed, true) == 0 && mdmRule.RuleStatus == ruleStatus)
                    {
                        result = mdmRule;
                        break;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Gets the MDMRules based on requested MDMRuleType
        /// </summary>
        /// <param name="ruleType">Indicates the rule type</param>
        /// <returns>Returns the MDMRuleCollection object</returns>
        public MDMRuleCollection GetMDMRulesByType(MDMRuleType ruleType)
        {
            MDMRuleCollection filteredRules = null;

            if (this._mdmRuleCollection != null && this.Count > 0)
            {
                filteredRules = new MDMRuleCollection();

                foreach (MDMRule rule in this._mdmRuleCollection)
                {
                    if (rule.RuleType == ruleType)
                    {
                        filteredRules.Add(rule);
                    }
                }
            }

            return filteredRules;
        }

        #endregion Get Methods

        #region Get Multiple Methods

        /// <summary>
        /// Get all published rules which are available in the current collection
        /// </summary>
        /// <returns>Returns the published MDMRules</returns>
        public IMDMRuleCollection GetMDMRulesByStatus(RuleStatus ruleStatus = RuleStatus.Draft)
        {
            MDMRuleCollection filteredRules = null;

            if (this._mdmRuleCollection != null && this.Count > 0)
            {
                filteredRules = new MDMRuleCollection();

                foreach (MDMRule rule in this._mdmRuleCollection)
                {
                    if (rule.RuleStatus == ruleStatus)
                    {
                        filteredRules.Add(rule);
                    }
                }
            }
            return filteredRules;
        }

        /// <summary>
        /// Gets all the BusinessConditions based on Rule name
        /// </summary>
        /// <param name="mdmRuleName">Indicates the MDMRule name</param>
        /// <returns>Returns the Business condition Rules as MDMRuleCollection object</returns>
        public MDMRuleCollection GetBusinessConditionRulesByRuleName(String mdmRuleName)
        {
            MDMRuleCollection filteredRules = null;

            if (!String.IsNullOrWhiteSpace(mdmRuleName) && this._mdmRuleCollection != null && this.Count > 0)
            {
                filteredRules = new MDMRuleCollection();

                foreach (MDMRule rule in this._mdmRuleCollection)
                {
                    if (rule.RuleType == MDMRuleType.BusinessCondition && rule.BusinessConditionRules.Contains(mdmRuleName))
                    {
                        filteredRules.Add(rule);
                    }
                }
            }

            return filteredRules;
        }

        /// <summary>
        /// Returns id of all mdmrules
        /// </summary>
        /// <param name="ruleStatus">Parameter to filter mdmrule id's based on rulestatus</param>
        /// <returns>Collection of mdmrule id's</returns>
        public ICollection<Int32> GetMDMRuleIds(RuleStatus ruleStatus = RuleStatus.Unknown)
        {
            Collection<Int32> mdmRuleIds = new Collection<Int32>();

            if (this._mdmRuleCollection != null && this.Count > 0)
            {
                foreach (MDMRule rule in this._mdmRuleCollection)
                {
                    if (!mdmRuleIds.Contains(rule.Id))
                    {
                        if (rule.RuleStatus == RuleStatus.Published)
                        {
                            mdmRuleIds.Add(rule.Id);
                        }
                        else if (rule.RuleStatus == RuleStatus.Published)
                        {
                            mdmRuleIds.Add(rule.Id);
                        }
                        else
                        {
                            mdmRuleIds.Add(rule.Id);
                        }
                    }
                }
            }

            return mdmRuleIds;
        }

        /// <summary>
        /// Returns name of all mdmrules
        /// </summary>
        /// <param name="ruleStatus">Parameter to filter mdmrule names based on rulestatus</param>
        /// <returns>Collection of mdmrule names</returns>
        public ICollection<String> GetMDMRuleNames(RuleStatus ruleStatus = RuleStatus.Unknown)
        {
            Collection<String> mdmRuleNames = new Collection<String>();

            if (this._mdmRuleCollection != null && this.Count > 0)
            {
                foreach (MDMRule rule in this._mdmRuleCollection)
                {
                    if (!mdmRuleNames.Contains(rule.Name))
                    {
                        if (rule.RuleStatus == RuleStatus.Published)
                        {
                            mdmRuleNames.Add(rule.Name);
                        }
                        else if (rule.RuleStatus == RuleStatus.Published)
                        {
                            mdmRuleNames.Add(rule.Name);
                        }
                        else
                        {
                            mdmRuleNames.Add(rule.Name);
                        }
                    }
                }
            }

            return mdmRuleNames;
        }

        /// <summary>
        /// Gets all Non-BusinessConditions Rules
        /// </summary>
        /// <returns>Returns all Non-BusinessCondition Rules</returns>
        public IMDMRuleCollection GetAllBusinessRules(RuleStatus ruleStatus = RuleStatus.Unknown)
        {
            MDMRuleCollection filteredRules = null;

            if (this._mdmRuleCollection != null && this.Count > 0)
            {
                filteredRules = new MDMRuleCollection();

                foreach (MDMRule rule in this._mdmRuleCollection)
                {
                    if (rule.RuleType != MDMRuleType.BusinessCondition)
                    {
                        if (ruleStatus == RuleStatus.Unknown)
                        {
                            filteredRules.Add(rule);
                        }
                        else
                        {
                            if (rule.RuleStatus == ruleStatus)
                            {
                                filteredRules.Add(rule);
                            }
                        }
                    }
                }
            }

            return filteredRules;
        }

        /// <summary>
        /// Gets all BusinessConditions
        /// </summary>
        /// <param name="ruleStatus"></param>
        /// <returns>Returns all Non-BusinessCondition Rules</returns>
        public IMDMRuleCollection GetAllBusinessConditions(RuleStatus ruleStatus = RuleStatus.Unknown)
        {
            MDMRuleCollection filteredRules = null;

            if (this._mdmRuleCollection != null && this.Count > 0)
            {
                filteredRules = new MDMRuleCollection();

                foreach (MDMRule rule in this._mdmRuleCollection)
                {
                    if (rule.RuleType == MDMRuleType.BusinessCondition)
                    {
                        if (ruleStatus == RuleStatus.Unknown)
                        {
                            filteredRules.Add(rule);
                        }
                        else
                        {
                            if (rule.RuleStatus == ruleStatus)
                            {
                                filteredRules.Add(rule);
                            }
                        }
                    }
                }
            }

            return filteredRules;
        }

        /// <summary>
        /// Returns mdmrules based on the rule id from mdmrulecollection
        /// </summary>
        /// <param name="mdmRuleIds">Indicates list of mdmrule ids</param>
        /// <returns>Returns mdmrules if exists, else null</returns>
        public MDMRuleCollection GetMDMRulesByRuleIds(Collection<Int32> mdmRuleIds)
        {
            MDMRuleCollection filteredRules = null;

            if (mdmRuleIds != null && mdmRuleIds.Count > 0 && this._mdmRuleCollection != null)
            {
                filteredRules = new MDMRuleCollection();

                foreach (Int32 ruleId in mdmRuleIds)
                {
                    MDMRule rule = this.GetMDMRuleById(ruleId);

                    if (rule != null)
                    {
                        filteredRules.Add(rule);
                    }
                }
            }

            return filteredRules;
        }

        /// <summary>
        /// Gets all the business condition's validation rule Ids.
        /// This will returns only the Unique Rule Ids.
        /// </summary>
        public Collection<Int32> GetBusinessConditionValidationRuleIds()
        {
            Collection<Int32> filteredRuleIds = null;

            if (this._mdmRuleCollection != null)
            {
                filteredRuleIds = new Collection<Int32>();

                foreach (MDMRule rule in this._mdmRuleCollection)
                {
                    if (rule.RuleType == MDMRuleType.BusinessCondition && rule.BusinessConditionRules != null && rule.BusinessConditionRules.Count > 0)
                    {
                        foreach (Int32 ruleId in rule.BusinessConditionRuleIds)
                        {
                            if (filteredRuleIds.Contains(ruleId) == false)
                            {
                                filteredRuleIds.Add(ruleId);
                            }
                        }
                    }
                }
            }

            return filteredRuleIds;
        }

        #endregion Get Multiple Methods

        #region Overridden Methods

        /// <summary>
        /// Gets a cloned instance of the current MDMRuleCollection object
        /// </summary>
        /// <returns>Cloned instance of the current MDMRuleCollection object</returns>
        public MDMRuleCollection Clone()
        {
            MDMRuleCollection clonedMdmRuleCollection = new MDMRuleCollection();

            if (this._mdmRuleCollection != null && this._mdmRuleCollection.Count > 0)
            {
                foreach (MDMRule mdmRule in this._mdmRuleCollection)
                {
                    MDMRule clonedMdmRule = mdmRule.Clone();
                    clonedMdmRuleCollection.Add(clonedMdmRule);
                }
            }

            return clonedMdmRuleCollection;
        }

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override Boolean Equals(object obj)
        {
            if (obj is MDMRuleCollection)
            {
                MDMRuleCollection objectToBeCompared = obj as MDMRuleCollection;

                Int32 mdmRulesUnion = this._mdmRuleCollection.ToList().Union(objectToBeCompared.ToList()).Count();
                Int32 mdmRulesIntersection = this._mdmRuleCollection.ToList().Intersect(objectToBeCompared.ToList()).Count();

                if (mdmRulesUnion != mdmRulesIntersection)
                    return false;

                return true;
            }

            return false;
        }

        /// <summary>
        /// Serves as a hash function for type
        /// </summary>
        /// <returns>A hash code for the current Object.</returns>
        public override Int32 GetHashCode()
        {
            Int32 hashCode = 0;

            foreach (MDMRule mdmRule in this._mdmRuleCollection)
            {
                hashCode += mdmRule.GetHashCode();
            }

            return hashCode;
        }

        /// <summary>
        /// Get Xml representation of MDMRule Collection
        /// </summary>
        /// <returns>Returns xml representation of MDMRuleCollection</returns>
        public String ToXml()
        {
            #region Sample Xml

            // Todo..

            #endregion

            String outputXml = String.Empty;

            using (StringWriter sw = new StringWriter())
            {
                using (XmlTextWriter xmlWriter = new XmlTextWriter(sw))
                {
                    //MDMRules node start
                    xmlWriter.WriteStartElement("MDMRules");

                    foreach (MDMRule mdmRule in _mdmRuleCollection)
                    {
                        xmlWriter.WriteRaw(mdmRule.ToXml());
                    }

                    //MDMRules node end
                    xmlWriter.WriteEndElement();
                }

                //Get the actual XML
                outputXml = sw.ToString();
            }

            return outputXml;
        }

        /// <summary>
        /// Compares MDMRuleCollection with current collection
        /// </summary>
        /// <param name="subSetMDMRuleCollection">Indicates MDMRuleCollection to be compare with the current collection</param>
        /// <returns>Returns True : If both are same. False : otherwise</returns>
        public Boolean IsSuperSetOf(MDMRuleCollection subSetMDMRuleCollection)
        {
            if (subSetMDMRuleCollection != null)
            {
                foreach (MDMRule subSetMDMRule in subSetMDMRuleCollection)
                {
                    MDMRule sourceMDMRule = this.GetMDMRuleByName(subSetMDMRule.Name);

                    //If it doesn't return, that means super set doesn't contain that mdmrule.
                    //So return false, else do further comparison
                    if (sourceMDMRule != null)
                    {
                        if (!sourceMDMRule.IsSuperSetOf(subSetMDMRule))
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        #endregion Overridden Methods

        #endregion Public Methods

        #region ICollection Members

        /// <summary>
        /// Adds the specified mdmRule
        /// </summary>
        /// <param name="mdmRule">mdmrule object</param>
        public void Add(MDMRule mdmRule)
        {
            if (mdmRule != null)
            {
                this._mdmRuleCollection.Add(mdmRule);
            }
        }

        /// <summary>
        /// Adds the requested MDMRules into existing collection
        /// </summary>
        /// <param name="mdmRules">Indicates the MDMRules</param>
        public void AddRange(MDMRuleCollection mdmRules)
        {
            if (mdmRules != null && mdmRules.Count > 0)
            {
                foreach (MDMRule rule in mdmRules)
                {
                    if (this._mdmRuleCollection.Contains(rule) == false)
                    {
                        this._mdmRuleCollection.Add(rule);
                    }
                }
            }
        }

        /// <summary>
        /// Removes all items from the current MDMRuleCollection
        /// </summary>
        public void Clear()
        {
            this._mdmRuleCollection.Clear();
        }

        /// <summary>
        /// Determines whether the current MDMRuleCollection contains the specified mdmRule.
        /// </summary>
        /// <param name="mdmRule">Rule to be verified</param>
        /// <returns>True if found in collection else false</returns>
        public Boolean Contains(MDMRule mdmRule)
        {
            return this._mdmRuleCollection.Contains(mdmRule);
        }

        /// <summary>
        /// Determines whether the current MDMRuleCollection contains the specified mdmrule name.
        /// </summary>
        /// <param name="ruleName">RuleName to be verified</param>
        /// <returns>True if rulename found in collection else false</returns>
        public Boolean Contains(String ruleName)
        {
            Boolean ruleNameFound = false;

            foreach (MDMRule mdmRule in this._mdmRuleCollection)
            {
                if (String.Compare(mdmRule.Name, ruleName) == 0)
                {
                    ruleNameFound = true;
                    break;
                }
            }

            return ruleNameFound;
        }

        /// <summary>
        /// Returns the result as true if the requested rule name is present in requested rule type else result as false.
        /// </summary>
        /// <param name="mdmRuleName">Name of rule</param>
        /// <param name="ruleType">Indicates the rule type</param>
        /// <returns>Returns the result as true if the requested rule name is present in requested rule type else result as false..</returns>
        public Boolean Contains(String mdmRuleName, MDMRuleType ruleType)
        {
            Boolean result = false;

            if (!String.IsNullOrWhiteSpace(mdmRuleName))
            {
                foreach (MDMRule mdmRule in _mdmRuleCollection)
                {
                    if (mdmRule.RuleType == ruleType && String.Compare(mdmRule.Name, mdmRuleName) == 0)
                    {
                        result = true;
                        break;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Returns the result as true if the requested rule name is present in other that Business condition rule type else result as false.
        /// </summary>
        /// <param name="mdmRuleName">Name of rule</param>
        /// <returns>Returns the result as true if the requested rule name is present in other that Business condition rule type else result as false.</returns>
        public Boolean ContainsNonBusinessConditionRule(String mdmRuleName)
        {
            Boolean result = false;

            if (!String.IsNullOrWhiteSpace(mdmRuleName))
            {
                foreach (MDMRule mdmRule in _mdmRuleCollection)
                {
                    if (mdmRule.RuleType != MDMRuleType.BusinessCondition && String.Compare(mdmRule.Name, mdmRuleName) == 0)
                    {
                        result = true;
                        break;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Copies the elements of the MDMRuleCollection to an System.Array, starting at a particular System.Array index
        /// </summary>
        /// <param name="array">
        /// The one-dimensional System.Array that is the destination of the elements copied from mdmrule collection. The System.Array must have zero-based indexing
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins</param>
        public void CopyTo(MDMRule[] array, Int32 arrayIndex)
        {
            this._mdmRuleCollection.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the MDMRuleCollection.
        /// </summary>
        /// <param name="mdmRule">Rule to be removed from the MDMRuleCollection</param>
        /// <returns>True if mdmrule is successfully removed; otherwise, false.</returns>
        public Boolean Remove(MDMRule mdmRule)
        {
            return this._mdmRuleCollection.Remove(mdmRule);
        }

        /// <summary>
        /// Removes the MDMRule from MDMRuleCollection by reference id
        /// </summary>
        /// <param name="referenceId">Indicates the reference id of MDMRule</param>
        /// <returns>True - If MDMRule is removed successfully, False - If MDMRule is not removed successfully</returns>
        public Boolean RemoveByReferenceId(Int64 referenceId)
        {
            Boolean result = false;

            foreach (MDMRule rule in this._mdmRuleCollection)
            {
                if (rule.ReferenceId == referenceId)
                {
                    result = this.Remove(rule);
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection
        /// </summary>
        /// <returns>A enumerator that can be used to iterate through the collection</returns>
        public IEnumerator<MDMRule> GetEnumerator()
        {
            return this._mdmRuleCollection.GetEnumerator();
        }

        #endregion ICollection Members

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An enumerator object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this._mdmRuleCollection.GetEnumerator();
        }

        #endregion IEnumerable Members

        #region IClonable Members

        /// <summary>
        /// Gets a cloned instance of the current MDMRuleCollection object
        /// </summary>
        /// <returns>Cloned instance of the current MDMRuleCollection object</returns>
        Object ICloneable.Clone()
        {
            return this.Clone();
        }

        #endregion IClonable Members

        #region Private Methods

        /// <summary>
        /// Loads MDMRule Collection
        /// </summary>
        /// <param name="valuesAsXml">Indicates xml representation of MDMRuleCollection</param>
        private void LoadMDMRuleCollectionFromXml(String valuesAsXml)
        {
            #region Sample Xml

            // Todo..

            #endregion

            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                using (XmlTextReader reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null))
                {
                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "MDMRule")
                        {
                            String mdmRuleXml = reader.ReadOuterXml();

                            if (!String.IsNullOrWhiteSpace(mdmRuleXml))
                            {
                                MDMRule mdmRule = new MDMRule(mdmRuleXml);

                                if (mdmRuleXml != null)
                                {
                                    this.Add(mdmRule);
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