using System;
using System.Runtime.Serialization;
using ProtoBuf;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Represents the class that contain MDM Rule Keyword Group information
    /// </summary>
    [DataContract]
    [ProtoContract(ImplicitFields = ImplicitFields.AllFields)]
    public class MDMRuleKeywordGroup : MDMObject, IMDMRuleKeywordGroup, ICloneable
    {
        #region Fields
     
        /// <summary>
        /// Field denotes whether the rule group is enabled or not
        /// </summary>
        private Boolean _isEnabled = false;

        #endregion

        #region Properties

        /// <summary>
        /// Property denotes whether the rule group is enabled or not
        /// </summary>
        [DataMember]
        public Boolean IsEnabled
        {
            get
            {
                return _isEnabled;
            }
            set
            {
                _isEnabled = value;
            }
        }

        #endregion

        #region Pubic Methods

        #region ICloneable Members

        /// <summary>
        /// Gets a cloned instance of the current MDMRuleKeywordGroup object
        /// </summary>
        /// <returns>Cloned instance of the current MDMRuleKeywordGroup object</returns>
        public MDMRuleKeywordGroup Clone()
        {
            MDMRuleKeywordGroup clonedObject = new MDMRuleKeywordGroup();

            clonedObject.Id = this.Id;
            clonedObject.ReferenceId = this.ReferenceId;
            clonedObject.Name = this.Name;
            clonedObject.Action = this.Action;
            clonedObject.IsEnabled = this.IsEnabled;

            return clonedObject;
        }

        /// <summary>
        /// Gets a cloned instance of the current MDMRuleMap object
        /// </summary>
        /// <returns>Cloned instance of the current MDMRuleMap object</returns>
        Object ICloneable.Clone()
        {
            return this.Clone();
        }

        #endregion ICloneable Members

        #endregion Public Methods
    }
}