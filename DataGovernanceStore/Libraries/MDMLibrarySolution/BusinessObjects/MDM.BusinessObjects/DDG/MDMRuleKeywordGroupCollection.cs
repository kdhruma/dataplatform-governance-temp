using System;
using System.Runtime.Serialization;
using ProtoBuf;

namespace MDM.BusinessObjects
{
    using MDM.Interfaces;

    /// <summary>
    /// Specifies collection of MDMRuleKeywordGroup items
    /// </summary>
    [DataContract]
    [ProtoContract(ImplicitFields = ImplicitFields.AllFields)]
    public class MDMRuleKeywordGroupCollection : InterfaceContractCollection<IMDMRuleKeywordGroup, MDMRuleKeywordGroup>, IMDMRuleKeywordGroupCollection, ICloneable
    {
        #region Fields

        #endregion Fields

        #region Properties

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Parameter less constructor
        /// </summary>
        public MDMRuleKeywordGroupCollection()
            : base()
        {
        }

        #endregion Constructors

        #region Public Methods

        #region ICloneable Members

        /// <summary>
        /// Gets a cloned instance of the current MDMRuleKeywordGroup collection object
        /// </summary>
        /// <returns>Cloned instance of the current MDMRuleKeywordGroup collection object</returns>
        public MDMRuleKeywordGroupCollection Clone()
        {
            MDMRuleKeywordGroupCollection clonedCollection = new MDMRuleKeywordGroupCollection();
            if (this._items != null && this._items.Count > 0)
            {
                foreach (MDMRuleKeywordGroup item in this._items)
                {
                    clonedCollection.Add(item.Clone());
                }
            }
            return clonedCollection;
        }

        /// <summary>
        /// Gets a cloned instance of the current MDMRuleKeywordGroup collection object
        /// </summary>
        /// <returns>Cloned instance of the current MDMRuleKeywordGroup collection object</returns>
        Object ICloneable.Clone()
        {
            return this.Clone();
        }

        #endregion ICloneable Members

        #endregion Public Methods
    }
}