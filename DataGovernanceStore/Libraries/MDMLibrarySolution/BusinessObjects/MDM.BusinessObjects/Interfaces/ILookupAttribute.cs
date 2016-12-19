using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties used for providing lookup attribute related information.
    /// </summary>
    public interface ILookupAttribute
    {
        #region Methods

        #region Value Get

        /// <summary>
        /// Gets the current value of the lookup attribute from the current entity
        /// </summary>
        /// <example>
        /// <code language="c#" title="Get Current Value Instance"  source="..\..\Documentation\MDM.APISamples\LookupManager\LookupAttributeSamples.cs" region="Get Current Value Instance"/>
        /// <code language="c#" title="Get Entity With Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>
        /// </example>
        /// <returns>Returns the current value</returns>
        IValue GetCurrentValueInstance();

        /// <summary>
        /// Gets the inherited value of the lookup attribute from the current entity
        /// </summary>
        /// <example>
        /// <code language="c#" title="Get Inherited Value Instance"  source="..\..\Documentation\MDM.APISamples\LookupManager\LookupAttributeSamples.cs" region="Get Inherited Value Instance"/>
        /// <code language="c#" title="Get Entity With Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>
        /// </example>
        /// <returns>Returns the inherited value</returns>
        IValue GetInheritedValueInstance();

        /// <summary>
        /// Gets the current invariant value of the lookup attribute from the current entity
        /// </summary>
        /// <example>
        /// <code language="c#" title="Get Current Value Instance Invariant"  source="..\..\Documentation\MDM.APISamples\LookupManager\LookupAttributeSamples.cs" region="Get Current Value Instance Invariant"/>
        /// <code language="c#" title="Get Entity With Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>
        /// </example>
        /// <returns>Returns the current invariant value</returns>
        IValue GetCurrentValueInstanceInvariant();

        /// <summary>
        /// Gets the inherited invariant value of the lookup attribute from the current entity
        /// </summary>
        /// <example>
        /// <code language="c#" title="Get Inherited Value Instance Invariant"  source="..\..\Documentation\MDM.APISamples\LookupManager\LookupAttributeSamples.cs" region="Get Inherited Value Instance Invariant"/>
        /// <code language="c#" title="Get Entity With Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>
        /// </example>
        /// <returns>Returns the inherited invariant value</returns>
        IValue GetInheritedValueInstanceInvariant();

        /// <summary>
        /// Gets the current display value of the lookup attribute from the current entity
        /// </summary>
        /// <example>
        /// <code language="c#" title="Get Current Display Value"  source="..\..\Documentation\MDM.APISamples\LookupManager\LookupAttributeSamples.cs" region="Get Current Display Value"/>
        /// <code language="c#" title="Get Entity With Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>
        /// </example>
        /// <returns>String representation of display value</returns>
        String GetCurrentDisplayValue();

        /// <summary>
        /// Gets the export value of the lookup attribute from the current entity
        /// </summary>
        /// <example>
        /// <code language="c#" title="Get Current Export Value"  source="..\..\Documentation\MDM.APISamples\LookupManager\LookupAttributeSamples.cs" region="Get Current Export Value"/>
        /// <code language="c#" title="Get Entity With Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>
        /// </example>
        /// <returns>String representation of export value</returns>
        String GetCurrentExportValue();

        /// <summary>
        /// Gets the inherited display value of the lookup attribute from the current entity
        /// </summary>
        /// <example>
        /// <code language="c#" title="Get Inherited Display Value"  source="..\..\Documentation\MDM.APISamples\LookupManager\LookupAttributeSamples.cs" region="Get Inherited Display Value"/>
        /// <code language="c#" title="Get Entity With Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>
        /// </example>
        /// <returns>String representation of display value</returns>
        String GetInheritedDisplayValue();

        /// <summary>
        /// Gets attribute export value
        /// </summary>
        /// <example>
        /// <code language="c#" title="Get Inherited Export Value"  source="..\..\Documentation\MDM.APISamples\LookupManager\LookupAttributeSamples.cs" region="Get Inherited Export Value"/>
        /// <code language="c#" title="Get Entity With Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>
        /// </example>
        /// <returns>String representation of export value</returns>
        String GetInheritedExportValue();

        /// <summary>
        /// Gets the current lookup row details of the attribute from the current entity
        /// </summary>
        /// <example>
        /// <code language="c#" title="Get Current Lookup Row As Dictionary"  source="..\..\Documentation\MDM.APISamples\LookupManager\LookupAttributeSamples.cs" region="Get Current Lookup Row As Dictionary"/>
        /// <code language="c#" title="Get Entity With Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>
        /// </example>
        /// <returns>Returns a dictionary with row details</returns>
        Dictionary<String, String> GetCurrentLookupRowAsDictionary();

        /// <summary>
        /// Gets the inherited lookup row details of the attribute from the current entity
        /// </summary>
        /// <example>
        /// <code language="c#" title="Get Inherited Lookup Row As Dictionary"  source="..\..\Documentation\MDM.APISamples\LookupManager\LookupAttributeSamples.cs" region="Get Inherited Lookup Row As Dictionary"/>
        /// <code language="c#" title="Get Entity With Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>
        /// </example>
        /// <returns>Returns a dictionary with row details</returns>
        Dictionary<String, String> GetInheritedLookupRowAsDictionary();

        /// <summary>
        /// Retrieves lookup column details for current value of the attribute by given column name
        /// </summary>
        /// <example>
        /// <code language="c#" title="Get Current Lookup Cell Value"  source="..\..\Documentation\MDM.APISamples\LookupManager\LookupAttributeSamples.cs" region="Get Current Lookup Cell Value"/>
        /// <code language="c#" title="Get Entity With Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>
        /// </example>
        /// <param name="columnName">Indicates the lookup column name</param>
        /// <returns>Column Value</returns>
        String GetCurrentLookupCellValue(String columnName);

        /// <summary>
        /// Retrieves lookup column details for inherited value of the attribute by given column name
        /// </summary>
        /// <example>
        /// <code language="c#" title="Get Inherited Lookup Cell Value"  source="..\..\Documentation\MDM.APISamples\LookupManager\LookupAttributeSamples.cs" region="Get Inherited Lookup Cell Value"/>
        /// <code language="c#" title="Get Entity With Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>
        /// </example>
        /// <param name="columnName">Indicates the lookup column name</param>
        /// <returns>Column Value</returns>
        String GetInheritedLookupCellValue(String columnName);

        #endregion

        #region Value Set

        /// <summary>
        /// Overrides the existing value with new value.
        /// </summary>
        /// <example>
        /// <code language="c#" title="Set Lookup Value"  source="..\..\Documentation\MDM.APISamples\LookupManager\LookupAttributeSamples.cs" region="Set Lookup Value"/>
        /// <code language="c#" title="Get Entity With Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>
        /// </example>
        /// <param name="value">Specifies New value to set in overridden attribute</param>
        void SetValue(IValue value);

        /// <summary>
        /// Overrides the existing value with new value in the specified locale.
        /// </summary>
        /// <example>
        /// <code language="c#" title="Set Lookup Value for Locale"  source="..\..\Documentation\MDM.APISamples\LookupManager\LookupAttributeSamples.cs" region="Set Lookup Value For Locale"/>
        /// <code language="c#" title="Get Entity With Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>
        /// </example>
        /// <param name="value">Specifies New value to set in overridden attribute</param>
        /// <param name="formatLocale">Specifies Locale in which value will be set</param>
        void SetValue(IValue value, LocaleEnum formatLocale);

        /// <summary>
        /// Overrides the existing value with new values without applying locale formatting
        /// </summary>
        /// <example>
        /// <code language="c#" title="Set Lookup Value Invariant"  source="..\..\Documentation\MDM.APISamples\LookupManager\LookupAttributeSamples.cs" region="Set Lookup Value Invariant"/>
        /// <code language="c#" title="Get Entity With Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>
        /// </example>
        /// <param name="value">Specifies New value to set in overridden attribute collection</param>
        void SetValueInvariant(IValue value);

        /// <summary>
        /// Overrides the existing value with new values.
        /// </summary>
        /// <example>
        /// <code language="c#" title="Set Lookup Value using Object"  source="..\..\Documentation\MDM.APISamples\LookupManager\LookupAttributeSamples.cs" region="Set Lookup Value From Object"/>
        /// <code language="c#" title="Get Entity With Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>
        /// </example>
        /// <param name="value">Specifies New value to add in overridden attribute</param>
        void SetValue(Object value);

        /// <summary>
        /// Sets the display value of the lookup attribute
        /// </summary>
        /// <example>
        /// <code language="c#" title="Set Lookup Display Value"  source="..\..\Documentation\MDM.APISamples\LookupManager\LookupAttributeSamples.cs" region="Set Lookup Display Value"/>
        /// <code language="c#" title="Get Entity With Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>
        /// </example>
        /// <param name="displayValue">Specifies display value</param>
        void SetDisplayValue(String displayValue);

        /// <summary>
        /// Sets the export value of the lookup attribute
        /// </summary>
        /// <example>
        /// <code language="c#" title="Set Lookup Export Value"  source="..\..\Documentation\MDM.APISamples\LookupManager\LookupAttributeSamples.cs" region="Set Lookup Export Value"/>
        /// <code language="c#" title="Get Entity With Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>
        /// </example>
        /// <param name="exportValue">Specifies export value</param>
        void SetExportValue(String exportValue);

        #endregion

        #endregion
    }
}
