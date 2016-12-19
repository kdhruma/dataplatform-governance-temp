using System;

namespace MDM.BusinessObjects.DataModel
{
    using MDM.Interfaces;
    using MDM.Core;
    using MDM.Core.DataModel;

    /// <summary>
    /// Represents class for storing data model validation options
    /// </summary>
    public class DataModelValidationOptions : IDataModelValidationOptions
    {
        #region Fields

        private Boolean _validateLookupDependencies = false;

        private Boolean _validateUomDependencies = false;

        #endregion
        
        #region Properties

        /// <summary>
        /// Denotes whether lookup dependencies validation is required
        /// </summary>
        public Boolean ValidateLookupDependencies
        {
            get { return _validateLookupDependencies; }
            set { _validateLookupDependencies = value; }
        }

        /// <summary>
        /// Denotes whether UOM dependencies validation is required
        /// </summary>
        public Boolean ValidateUomDependencies
        {
            get { return _validateUomDependencies; }
            set { _validateUomDependencies = value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Represents empty constructor for data model validation options
        /// </summary>
        public DataModelValidationOptions()
        {
        }

        /// <summary>
        /// Constructs DataModelValidationOptions from given dictionary of configuration items
        /// </summary>
        /// <param name="configurationItems">Represents source dictionary of configuration items</param>
        public DataModelValidationOptions(IDataModelConfigurationItemDictionary configurationItems)
        {
            if (configurationItems.ContainsKey(DataModelConfigurationItem.ValidateLookupDependencies))
            {
                ValidateLookupDependencies = ValueTypeHelper.BooleanTryParse(configurationItems[DataModelConfigurationItem.ValidateLookupDependencies], false);
            }

            if (configurationItems.ContainsKey(DataModelConfigurationItem.ValidateUomDependencies))
            {
                ValidateUomDependencies = ValueTypeHelper.BooleanTryParse(configurationItems[DataModelConfigurationItem.ValidateUomDependencies], false);
            }
        }

        #endregion

    }
}
