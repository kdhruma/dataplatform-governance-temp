using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects.DQMNormalization
{
    using MDM.Interfaces;

    /// <summary>
    /// Class for Normalization Rulesets Collection
    /// </summary>
    [DataContract]
    public class NormalizationRulesetsCollection : INormalizationRulesetsCollection
    {
        #region Fields

        private const String RulesetCollectionNodeName = "RulesetsCollection";
        private const String RulesetsNodeName = "Ruleset";

        [DataMember]
        private Collection<NormalizationRuleset> _normalizationRules = new Collection<NormalizationRuleset>();

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public NormalizationRulesetsCollection()
        {
            this._normalizationRules = new Collection<NormalizationRuleset>();
        }

        /// <summary>
        /// Initialize NormalizationRulesetsCollection from IList
        /// </summary>
        /// <param name="normalizationRulesList">IList of NormalizationRules</param>
        public NormalizationRulesetsCollection(IList<NormalizationRuleset> normalizationRulesList)
        {
            this._normalizationRules = new Collection<NormalizationRuleset>(normalizationRulesList);
        }
        
        /// <summary>
        /// Initialize NormalizationRulesetsCollection from xml string
        /// </summary>
        /// <param name="valuesAsXml">String representation of NormalizationRules</param>
        public NormalizationRulesetsCollection(String valuesAsXml)
        {
            LoadFromXmlWithOuterNode(valuesAsXml);
        }

        private void LoadFromXmlWithOuterNode(string valuesAsXml)
        {
            if (String.IsNullOrEmpty(valuesAsXml))
            {
                return;
            }

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(valuesAsXml);
            XmlNode node = doc.SelectSingleNode(RulesetCollectionNodeName);
            if (node != null)
            {
                LoadFromXml(node);
            }
        }

        private void LoadFromXml(XmlNode node)
        {
            if (node == null)
            {
                return;
            }

            XmlNodeList rulesetNodes = node.SelectNodes(RulesetsNodeName);

            if (rulesetNodes == null || rulesetNodes.Count < 1)
            {
                return;
            }

            List<NormalizationRuleset> rulesets = new List<NormalizationRuleset>();
            foreach (XmlNode rulesetNode in rulesetNodes)
            {
                NormalizationRuleset ruleset = new NormalizationRuleset();

                ruleset.LoadPropertiesOnlyFromXml(rulesetNode.OuterXml);

                rulesets.Add(ruleset);
            }
            _normalizationRules = new Collection<NormalizationRuleset>(rulesets);
        }

        #endregion

        #region Properties
        
        /// <summary>
        /// Indexer to getting rule sets by Id
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public NormalizationRuleset this[Int32 index]   
        {            
            get { return _normalizationRules [index]; }                        
        }

        #endregion

        #region Methods

        #region Public Method

        /// <summary>
        /// Clone Normalization rulesets collection
        /// </summary>
        /// <returns>Cloned Normalization rulesets collection object</returns>
        public INormalizationRulesetsCollection Clone()
        {
            NormalizationRulesetsCollection clonedProfiles = new NormalizationRulesetsCollection();

            if (this._normalizationRules != null && this._normalizationRules.Count > 0)
            {
                foreach (NormalizationRuleset profile in this._normalizationRules)
                {
                    NormalizationRuleset clonedProfile = profile.Clone() as NormalizationRuleset;
                    clonedProfiles.Add(clonedProfile);
                }
            }

            return clonedProfiles;
        }

        /// <summary>
        /// Remove NormalizationRuleset object from NormalizationRuleCollection
        /// </summary>
        /// <param name="normalizationRuleId">Id of NormalizationRuleset which information is to be removed from collection</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original collection</returns>
        public Boolean Remove(Int32 normalizationRuleId)
        {
            NormalizationRuleset normalizationRuleset = Get(normalizationRuleId);
            if (normalizationRuleset == null)
            {
                return false;
            }
            return this.Remove(normalizationRuleset);
        }

        /// <summary>
        /// Determines whether two Object instances are equal
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false</returns>
        public override Boolean Equals(object obj)
        {
            if (obj != null)
            {
                NormalizationRulesetsCollection objectToBeCompared = obj as NormalizationRulesetsCollection;
                if (objectToBeCompared != null)
                {
                    if (ReferenceEquals(this, objectToBeCompared))
                    {
                        return true;
                    }

                    List<NormalizationRuleset> normalizationRulesets = this._normalizationRules.ToList();
                    List<NormalizationRuleset> rulesets = objectToBeCompared.ToList();

                    Int32 normalizationRuleUnion = normalizationRulesets.Union(rulesets).Count();
                    Int32 normalizationRulesIntersect = normalizationRulesets.Intersect(rulesets).Count();

                    return (normalizationRuleUnion == normalizationRulesIntersect);
                }
            }
            return false;
        }

        /// <summary>
        /// Serves as a hash function for type
        /// </summary>
        /// <returns>A hash code for the current Object</returns>
        public override Int32 GetHashCode()
        {
            return this._normalizationRules.Sum(item => item.GetHashCode());
        }

        #endregion

        #region Private Method

        /// <summary>
        /// Get NormalizationRuleset from current NormalizationRuleCollection based on NormalizationRuleset Id
        /// </summary>
        /// <param name="normalizationRuleId">Id of NormalizationRuleset which information is to be searched</param>
        /// <returns>NormalizationRuleset having given NormalizationRuleId messageCode</returns>
        private NormalizationRuleset Get(Int32 normalizationRuleId)
        {
            return this._normalizationRules.FirstOrDefault(normalizationRule => normalizationRule.Id == normalizationRuleId);
        }

        #endregion

        #endregion

        #region ICollection<NormalizationRuleset> Members

        /// <summary>
        /// Add NormalizationRuleset object in collection
        /// </summary>
        /// <param name="item">NormalizationRuleset to add in collection</param>
        public void Add(NormalizationRuleset item)
        {
            this._normalizationRules.Add(item);
        }

        /// <summary>
        /// Add INormalizationRuleset object in collection
        /// </summary>
        /// <param name="item">INormalizationRuleset to add in collection</param>
        public void Add(INormalizationRuleset item)
        {
            this._normalizationRules.Add((NormalizationRuleset)item);
        }

        /// <summary>
        /// Removes all NormalizationRules from collection
        /// </summary>
        public void Clear()
        {
            this._normalizationRules.Clear();
        }

        /// <summary>
        /// Determines whether the NormalizationRulesetsCollection contains a specific NormalizationRuleset
        /// </summary>
        /// <param name="item">The NormalizationRuleset object to locate in the NormalizationRulesetsCollection</param>
        /// <returns>
        /// <para>true : If NormalizationRuleset found in NormalizationRulesetsCollection</para>
        /// <para>false : If NormalizationRuleset found not in NormalizationRulesetsCollection</para>
        /// </returns>
        public Boolean Contains(NormalizationRuleset item)
        {
            return this._normalizationRules.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the NormalizationRulesetsCollection to an
        /// System.Array, starting at a particular System.Array index
        /// </summary>
        /// <param name="array"> 
        ///  The one-dimensional System.Array that is the destination of the elements copied from NormalizationRulesetsCollection. The System.Array must have zero-based indexing
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins</param>
        public void CopyTo(NormalizationRuleset[] array, Int32 arrayIndex)
        {
            this._normalizationRules.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Get the count of no. of NormalizationRuleset in NormalizationRulesetsCollection
        /// </summary>
        public Int32 Count
        {
            get
            {
                return this._normalizationRules.Count;
            }
        }

        /// <summary>
        /// Check if NormalizationRulesetsCollection is read-only
        /// </summary>
        public Boolean IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the NormalizationRulesetsCollection
        /// </summary>
        /// <param name="item">The NormalizationRuleset object to remove from the NormalizationRulesetsCollection</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original NormalizationRulesetsCollection</returns>
        public Boolean Remove(NormalizationRuleset item)
        {
            return this._normalizationRules.Remove(item);
        }

        #endregion

        #region IEnumerable<NormalizationRules> Members

        /// <summary>
        /// Returns an enumerator that iterates through a NormalizationRulesetsCollection
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection</returns>
        public IEnumerator<NormalizationRuleset> GetEnumerator()
        {
            return this._normalizationRules.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a NormalizationRulesetsCollection
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this._normalizationRules.GetEnumerator();
        }

        #endregion
    }
}