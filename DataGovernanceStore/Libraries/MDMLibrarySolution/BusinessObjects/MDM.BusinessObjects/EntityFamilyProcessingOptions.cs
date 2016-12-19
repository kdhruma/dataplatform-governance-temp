using System;
using System.Runtime.Serialization;

namespace MDM.BusinessObjects
{
    using MDM.BusinessObjects.Interfaces;
    using MDM.Core;

    /// <summary>
    /// Specifies EntityFamilyProcessingOptions which specifies various flags and indications to entity family processing logic
    /// </summary>
    [DataContract]
    public class EntityFamilyProcessingOptions : ObjectBase, IEntityFamilyProcessingOptions
    {
        #region Fields

        /// <summary>
        /// 
        /// </summary>
        private Boolean _performAutoExtensions = true;

        /// <summary>
        /// 
        /// </summary>
        private Boolean _invokeAsyncBRs = true;

        /// <summary>
        /// 
        /// </summary>
        private Boolean _invokeEntityFamilyEvents = true;

        /// <summary>
        /// 
        /// </summary>
        private Boolean _validateMetadata = true;

        /// <summary>
        /// 
        /// </summary>
        private Boolean _saveEntityFamilyUpdatesToDB = true;

        /// <summary>
        /// 
        /// </summary>
        private Boolean _saveEntityStateUpdatesToDB = true;

        #endregion Fields

        #region Constructors
        #endregion Constructors

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public Boolean PerformAutoExtensions 
        {
            get
            {
                return this._performAutoExtensions;
            }
            set
            {
                this._performAutoExtensions = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public Boolean InvokeAsyncBRs
        {
            get
            {
                return this._invokeAsyncBRs;
            }
            set
            {
                this._invokeAsyncBRs = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public Boolean InvokeEntityFamilyEvents 
        {
            get
            {
                return this._invokeEntityFamilyEvents;
            }
            set
            {
                this._invokeEntityFamilyEvents = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public Boolean ValidateMetadata 
        {
            get
            {
                return this._validateMetadata;
            }
            set
            {
                this._validateMetadata = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public Boolean SaveEntityFamilyUpdatesToDB 
        {
            get 
            {
                return this._saveEntityFamilyUpdatesToDB;
            }
            set
            {
                this._saveEntityFamilyUpdatesToDB = value;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public Boolean SaveEntityStateUpdatesToDB
        {
            get
            {
                return this._saveEntityStateUpdatesToDB;
            }
            set
            {
                this._saveEntityStateUpdatesToDB = value;
            }
        }

        #endregion Properties

        #region Methods

        #region Public Methods
        #endregion Public Methods

        #region Private Methods
        #endregion Private Methods

        #endregion Methods
    }
}
