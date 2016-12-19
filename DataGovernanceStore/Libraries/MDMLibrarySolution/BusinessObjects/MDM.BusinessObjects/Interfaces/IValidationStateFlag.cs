namespace MDM.Interfaces
{
    using MDM.Core;
    using System;

    /// <summary>
    /// Exposes methods or properties used for providing entity validation state flag related information.
    /// </summary>
    public interface IValidationStateFlag
    {
        #region Properties

        /// <summary>
        /// Property denoting  IsSelfValid validation state attribute value 
        /// </summary>    
        ValidationStateValue IsSelfValid { get; set; }

        /// <summary>
        /// Property denoting  IsMetaDataValid validation state attribute value 
        /// </summary>     
        ValidationStateValue IsMetaDataValid { get; set; }

        /// <summary>
        /// Property denoting  IsCommonAttributesValid validation state attribute value 
        /// </summary> 
        ValidationStateValue IsCommonAttributesValid { get; set; }

        /// <summary>
        /// Property denoting  IsCategoryAttributesValid validation state attribute value 
        /// </summary> 
        ValidationStateValue IsCategoryAttributesValid { get; set; }

        /// <summary>
        /// Property denoting  isRelationshipsValid validation state attribute value 
        /// </summary> 
        ValidationStateValue IsRelationshipsValid { get; set; }

        /// <summary>
        /// Property denoting  IsEntityVariantValid validation state attribute value 
        /// </summary> 
        ValidationStateValue IsEntityVariantValid { get; set; }

        /// <summary>
        /// Property denoting  IsExtensionsValid validation state attribute value 
        /// </summary> 
        ValidationStateValue IsExtensionsValid { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Represents EntityValidationState  in Xml format
        /// </summary>
        /// <returns>
        /// EntityValidationState in Xml format
        /// </returns>
        String ToXml();

        #endregion
    }
}
