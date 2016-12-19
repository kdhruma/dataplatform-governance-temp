using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;

namespace MDM.BusinessObjects.DQMNormalization
{
    using MDM.Interfaces;
    using System.Text;

    /// <summary>
    /// Specifies NormalizationRules Collection
    /// </summary>
    [DataContract]
    public class NormalizationRulesCollection : INormalizationRulesCollection, ICloneable
    {
        #region Fields

        [DataMember]
        private Collection<NormalizationRule> _normalizationRules = new Collection<NormalizationRule>();

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public NormalizationRulesCollection()
        {
            this._normalizationRules = new Collection<NormalizationRule>();
        }

        /// <summary>
        /// Initialize NormalizationRulesCollection from IList
        /// </summary>
        /// <param name="normalizationRulesList">IList of NormalizationRules</param>
        public NormalizationRulesCollection(IList<NormalizationRule> normalizationRulesList)
        {
            this._normalizationRules = new Collection<NormalizationRule>(normalizationRulesList);
        }

        #endregion

        #region Properties
        
        /// <summary>
        /// Indexer to getting normalization rules by Id
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public NormalizationRule this[Int32 index]   
        {            
            get { return _normalizationRules [index]; }                        
        }

        #endregion

        #region Methods

        #region Public Method

        /// <summary>
        /// Clone Normalization Rules collection
        /// </summary>
        /// <returns>Cloned Normalization Rules collection object</returns>
        public Object Clone()
        {
            NormalizationRulesCollection clonedRules = new NormalizationRulesCollection();

            if (this._normalizationRules != null && this._normalizationRules.Count > 0)
            {
                foreach (NormalizationRule rule in this._normalizationRules)
                {
                    NormalizationRule clonedRule = rule.Clone() as NormalizationRule;
                    clonedRules.Add(clonedRule);
                }
            }

            return clonedRules;
        }

        /// <summary>
        /// Remove NormalizationRule object from NormalizationRuleCollection
        /// </summary>
        /// <param name="normalizationRuleId">Id of NormalizationRule which information is to be removed from collection</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original collection</returns>
        public Boolean Remove(Int32 normalizationRuleId)
        {
            NormalizationRule normalizationRule = Get(normalizationRuleId);
            if (normalizationRule == null)
            {
                return false;
            }
            return this.Remove(normalizationRule);
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
                NormalizationRulesCollection objectToBeCompared = obj as NormalizationRulesCollection;
                if (objectToBeCompared != null)
                {
                    if (ReferenceEquals(this, objectToBeCompared))
                    {
                        return true;
                    }

                    List<NormalizationRule> normalizationRules = this._normalizationRules.ToList();
                    List<NormalizationRule> rules = objectToBeCompared.ToList();

                    Int32 normalizationRuleUnion = normalizationRules.Union(rules).Count();
                    Int32 normalizationRulesIntersect = normalizationRules.Intersect(rules).Count();

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


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public String ToXml()
        {
            var buffer = new StringBuilder();

            buffer.AppendLine("<NormalizationRulesCollection>");

            if (this._normalizationRules != null && this._normalizationRules.Count > 0)
            {
                foreach (NormalizationRule rule in this._normalizationRules)
                {
                    buffer.AppendLine(rule.ToXml());
                }
            }

            buffer.AppendLine("</NormalizationRulesCollection>");

            return buffer.ToString();
        }

        #endregion

        #region Private Method

        /// <summary>
        /// Get NormalizationRule from current NormalizationRuleCollection based on NormalizationRule Id
        /// </summary>
        /// <param name="normalizationRuleId">Id of NormalizationRule which information is to be searched</param>
        /// <returns>NormalizationRule having given NormalizationRuleId messageCode</returns>
        private NormalizationRule Get(Int32 normalizationRuleId)
        {
            return this._normalizationRules.FirstOrDefault(normalizationRule => normalizationRule.Id == normalizationRuleId);
        }

        #endregion

        #endregion

        #region ICollection<NormalizationRule> Members

        /// <summary>
        /// Add NormalizationRule object in collection
        /// </summary>
        /// <param name="item">NormalizationRule to add in collection</param>
        public void Add(NormalizationRule item)
        {
            this._normalizationRules.Add(item);
        }

        /// <summary>
        /// Add INormalizationRule object in collection
        /// </summary>
        /// <param name="item">INormalizationRule to add in collection</param>
        public void Add(INormalizationRule item)
        {
            this._normalizationRules.Add((NormalizationRule)item);
        }

        /// <summary>
        /// Removes all NormalizationRules from collection
        /// </summary>
        public void Clear()
        {
            this._normalizationRules.Clear();
        }

        /// <summary>
        /// Determines whether the NormalizationRulesCollection contains a specific NormalizationRule
        /// </summary>
        /// <param name="item">The NormalizationRule object to locate in the NormalizationRulesCollection</param>
        /// <returns>
        /// <para>true : If NormalizationRule found in NormalizationRulesCollection</para>
        /// <para>false : If NormalizationRule found not in NormalizationRulesCollection</para>
        /// </returns>
        public Boolean Contains(NormalizationRule item)
        {
            return this._normalizationRules.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the NormalizationRulesCollection to an
        /// System.Array, starting at a particular System.Array index
        /// </summary>
        /// <param name="array"> 
        ///  The one-dimensional System.Array that is the destination of the elements copied from NormalizationRulesCollection. The System.Array must have zero-based indexing
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins</param>
        public void CopyTo(NormalizationRule[] array, Int32 arrayIndex)
        {
            this._normalizationRules.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Get the count of no. of NormalizationRule in NormalizationRulesCollection
        /// </summary>
        public Int32 Count
        {
            get
            {
                return this._normalizationRules.Count;
            }
        }

        /// <summary>
        /// Check if NormalizationRulesCollection is read-only
        /// </summary>
        public Boolean IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the NormalizationRulesCollection
        /// </summary>
        /// <param name="item">The NormalizationRule object to remove from the NormalizationRulesCollection</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original NormalizationRulesCollection</returns>
        public Boolean Remove(NormalizationRule item)
        {
            return this._normalizationRules.Remove(item);
        }

        #endregion

        #region IEnumerable<NormalizationRules> Members

        /// <summary>
        /// Returns an enumerator that iterates through a NormalizationRulesCollection
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection</returns>
        public IEnumerator<NormalizationRule> GetEnumerator()
        {
            return this._normalizationRules.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a NormalizationRulesCollection
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this._normalizationRules.GetEnumerator();
        }

        #endregion
    }
}