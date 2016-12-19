using System;

namespace MDM.BusinessObjects.Interfaces
{
    /// <summary>
    /// Exposes methods or properties to set or get entity family processing options, which specifies various flags and indications to entity family processing logic.
    /// </summary>
    public interface IEntityFamilyProcessingOptions
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        Boolean PerformAutoExtensions { get; set; }

        /// <summary>
        /// 
        /// </summary>
        Boolean InvokeAsyncBRs { get; set; }

        /// <summary>
        /// 
        /// </summary>
        Boolean InvokeEntityFamilyEvents { get; set; }

        /// <summary>
        /// 
        /// </summary>
        Boolean ValidateMetadata { get; set; }

        /// <summary>
        /// 
        /// </summary>
        Boolean SaveEntityFamilyUpdatesToDB { get; set; }

        /// <summary>
        /// 
        /// </summary>
        Boolean SaveEntityStateUpdatesToDB { get; set; }

        #endregion Properties

        #region Methods
        #endregion Methods
    }
}
