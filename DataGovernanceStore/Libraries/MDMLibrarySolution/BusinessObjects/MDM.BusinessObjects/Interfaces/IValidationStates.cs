using System;

namespace MDM.Interfaces
{
    using MDM.Core;    

    /// <summary>
    /// Exposes methods or properties used for providing entity validation state related information.
    /// </summary>
    public interface IValidationStates
    {
        #region Properties

        /// <summary>
        /// Property denoting  IsSelfValid validation state attribute value 
        /// </summary>    
        ValidityStateValue IsSelfValid { get; set; }

        /// <summary>
        /// Property denoting  IsMetaDataValid validation state attribute value 
        /// </summary>     
        ValidityStateValue IsMetaDataValid { get; set; }

        /// <summary>
        /// Property denoting  IsCommonAttributesValid validation state attribute value 
        /// </summary> 
        ValidityStateValue IsCommonAttributesValid { get; set; }

        /// <summary>
        /// Property denoting  IsCategoryAttributesValid validation state attribute value 
        /// </summary> 
        ValidityStateValue IsCategoryAttributesValid { get; set; }

        /// <summary>
        /// Property denoting  isRelationshipsValid validation state attribute value 
        /// </summary> 
        ValidityStateValue IsRelationshipsValid { get; set; }

        /// <summary>
        /// Property denoting  IsEntityVariantValid validation state attribute value 
        /// </summary> 
        ValidityStateValue IsEntityVariantValid { get; set; }

        /// <summary>
        /// Property denoting  IsExtensionsValid validation state attribute value 
        /// </summary> 
        ValidityStateValue IsExtensionsValid { get; set; }

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