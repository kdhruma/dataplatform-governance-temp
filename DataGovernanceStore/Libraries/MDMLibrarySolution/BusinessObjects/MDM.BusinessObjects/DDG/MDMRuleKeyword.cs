using System;
using System.Runtime.Serialization;
using ProtoBuf;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Represents the class that contain MDM Rule Keyword information
    /// </summary>
    [DataContract]
    [ProtoContract(ImplicitFields = ImplicitFields.AllFields)]
    public class MDMRuleKeyword : MDMObject, IMDMRuleKeyword, ICloneable
    {
        #region Fields
     
        /// <summary>
        /// Field denotes whether the rule is enabled or not
        /// </summary>
        private Boolean _isEnabled = false;

        /// <summary>
        /// Field denotes syntax
        /// </summary>
        private String _syntax;

        /// <summary>
        /// Field denotes keyword sample
        /// </summary>
        private String _sample;

        /// <summary>
        /// Field denotes description (or description's localization message code) for Rule Keyword
        /// </summary>
        private String _description;

        /// <summary>
        /// Field denotes Parent Keyword Group Id
        /// </summary>
        private Int32 _parentKeywordGroupId;

        /// <summary>
        /// Field denotes Parent Keyword Group name
        /// </summary>
        private String _parentKeywordGroupName;

        /// <summary>
        /// Field denotes whether the Parent Keyword Group is enabled or not
        /// </summary>
        private Boolean _isParentKeywordGroupEnabled = false;

        /// <summary>
        /// Field denotes whether the keyword can be display in UI or not
        /// </summary>
        private Boolean _showInUI = true;

        /// <summary>
        /// Field denotes whether the keyword is core keyword or custom keyword
        /// </summary>
        private Boolean _isSystemKeyword = true;

        /// <summary>
        /// Field denotes the DDGKeywordCategory
        /// </summary>
        private DDGKeywordCategory _ddgKeywordCategory = DDGKeywordCategory.Unknown;

        #endregion

        #region Properties

        /// <summary>
        /// Property denotes whether the rule is enabled or not
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

        /// <summary>
        /// Property whether the keyword can be display in UI or not
        /// </summary>
        [DataMember]
        public Boolean ShowInUI
        {
            get
            {
                return _showInUI;
            }
            set
            {
                _showInUI = value;
            }
        }


        /// <summary>
        /// Property denotes whether the keyword is core keyword or custom keyword
        /// </summary>
        [DataMember]
        public Boolean IsSystemKeyword
        {
            get
            {
                return _isSystemKeyword;
            }
            set
            {
                _isSystemKeyword = value;
            }
        }

        /// <summary>
        /// Property denotes syntax
        /// </summary>
        [DataMember]
        public String Syntax
        {
            get
            {
                return _syntax;
            }
            set
            {
                _syntax = value;
            }
        }

        /// <summary>
        /// Property denotes keyword sample
        /// </summary>
        [DataMember]
        public String Sample
        {
            get
            {
                return _sample;
            }
            set
            {
                _sample = value;
            }
        }

        /// <summary>
        /// Property denotes description (or description's localization message code) for Rule Keyword
        /// </summary>
        [DataMember]
        public String Description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
            }
        }

        /// <summary>
        /// Property denotes Parent Keyword Group Id
        /// </summary>
        [DataMember]
        public Int32 ParentKeywordGroupId
        {
            get
            {
                return _parentKeywordGroupId;
            }
            set
            {
                _parentKeywordGroupId = value;
            }
        }

        /// <summary>
        /// Property denotes Parent Keyword Group name
        /// </summary>
        [DataMember]
        public String ParentKeywordGroupName
        {
            get
            {
                return _parentKeywordGroupName;
            }
            set
            {
                _parentKeywordGroupName = value;
            }
        }

        /// <summary>
        /// Property denotes whether the Parent Keyword Group is enabled or not
        /// </summary>
        [DataMember]
        public Boolean IsParentKeywordGroupEnabled
        {
            get
            {
                return _isParentKeywordGroupEnabled;
            }
            set
            {
                _isParentKeywordGroupEnabled = value;
            }
        }

        /// <summary>
        /// Property denotes DDGKeywordCategory
        /// </summary>
        [DataMember]
        public DDGKeywordCategory DdgKeywordCategory
        {
            get
            {
                return _ddgKeywordCategory;
            }
            set
            {
                _ddgKeywordCategory = value;
            }
        }
        #endregion

        #region Pubic Methods

        #region ICloneable Members

        /// <summary>
        /// Gets a cloned instance of the current MDMRuleKeyword object
        /// </summary>
        /// <returns>Cloned instance of the current MDMRuleKeyword object</returns>
        public MDMRuleKeyword Clone()
        {
            MDMRuleKeyword clonedObject = new MDMRuleKeyword();

            clonedObject.Id = this.Id;
            clonedObject.ReferenceId = this.ReferenceId;
            clonedObject.Name = this.Name;
            clonedObject.Action = this.Action;
            clonedObject.IsEnabled = this.IsEnabled;
            clonedObject.Syntax = this.Syntax;
            clonedObject.Sample = this.Sample;
            clonedObject.Description = this.Description;
            clonedObject.ParentKeywordGroupId = this.ParentKeywordGroupId;
            clonedObject.ParentKeywordGroupName = this.ParentKeywordGroupName;
            clonedObject.IsParentKeywordGroupEnabled = this.IsParentKeywordGroupEnabled;
            clonedObject.IsSystemKeyword = this.IsSystemKeyword;
            clonedObject.ShowInUI = this.ShowInUI;
            clonedObject.DdgKeywordCategory = this.DdgKeywordCategory;

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