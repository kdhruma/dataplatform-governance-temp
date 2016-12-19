using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using MDM.Core;
using ProtoBuf;

namespace MDM.BusinessObjects
{
    using MDM.Interfaces;

    /// <summary>
    /// Specifies collection of MDMRuleKeyword items
    /// </summary>
    [DataContract]
    [ProtoContract(ImplicitFields = ImplicitFields.AllFields)]
    public class MDMRuleKeywordCollection : InterfaceContractCollection<IMDMRuleKeyword, MDMRuleKeyword>, IMDMRuleKeywordCollection, ICloneable
    {
        #region Fields

        #endregion Fields

        #region Properties

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Parameter less constructor
        /// </summary>
        public MDMRuleKeywordCollection()
            : base()
        {
        }

        #endregion Constructors

        #region Public Methods

        #region ICloneable Members

        /// <summary>
        /// Gets a cloned instance of the current MDMRuleKeyword collection object
        /// </summary>
        /// <returns>Cloned instance of the current MDMRuleKeyword collection object</returns>
        public MDMRuleKeywordCollection Clone()
        {
            MDMRuleKeywordCollection clonedCollection = new MDMRuleKeywordCollection();
            if (this._items != null && this._items.Count > 0)
            {
                foreach (MDMRuleKeyword item in this._items)
                {
                    clonedCollection.Add(item.Clone());
                }
            }
            return clonedCollection;
        }

        /// <summary>
        /// Gets a cloned instance of the current MDMRuleKeyword collection object
        /// </summary>
        /// <returns>Cloned instance of the current MDMRuleKeyword collection object</returns>
        Object ICloneable.Clone()
        {
            return this.Clone();
        }

        #endregion ICloneable Members

        #region Helper Methods

        /// <summary>
        /// Gets list of keywords based on category.
        /// </summary>
        /// <param name="keywordCategory">Indicates the keyword category</param>
        /// <returns>Returns list of keywords based on category</returns>
        public String[] GetKeywordsByCategory(DDGKeywordCategory keywordCategory)
        {
            Collection<String> result = new Collection<String>();

            if (this._items != null && this._items.Count > 0)
            {
                foreach (MDMRuleKeyword keyword in this._items)
                {
                    if (keyword.DdgKeywordCategory == keywordCategory)
                    {
                        if (result.Contains(keyword.NameInLowerCase) == false)
                        {
                            result.Add(keyword.NameInLowerCase);
                        }
                    }
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Gets list of extension / custamized keywords.
        /// </summary>
        /// <returns>Returns list of keywords based on category</returns>
        public String[] GetExtensionCoreKeywords()
        {
            Collection<String> result = new Collection<String>();

            if (this._items != null && this._items.Count > 0)
            {
                foreach (MDMRuleKeyword keyword in this._items)
                {
                    if (keyword.IsSystemKeyword == false)
                    {
                        result.Add(keyword.NameInLowerCase);
                    }
                }
            }

            return result.ToArray();
        }

        #endregion Helper Methods

        #endregion Public Methods
    }
}