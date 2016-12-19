using System;
using MDM.Core;

namespace MDM.BusinessObjects
{   
    /// <summary>
    /// Represents class for BusinessObject Attribute
    /// </summary>
    public class BusinessObjectAttribute
    {
        /// <summary>
        /// Property denoting identifier of BusinessObjectAttribute
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Property denoting IsInherited flag of BusinessObjectAttribute
        /// </summary>
        public bool IsInherited { get; set; }

        /// <summary>
        /// Property denoting Array of Values for BusinessObjectAttribute
        /// </summary>
        public Value[] Values { get; set; }

        /// <summary>
        /// Property denoting Array of Inherited Values for BusinessObjectAttribute
        /// </summary>
        public Value[] InheritedValues { get; set; }

        /// <summary>
        /// Checks if there is any Overriden|Inherited value/s for BusinessObjectAttribute
        /// </summary>
        /// <returns>True : If any value. False : otherwise</returns>
        public Boolean HasAnyValue()
        {
            return HasOverriddenValues() || HasInheritedValues();
        }


        private bool HasInheritedValues()
        {
            return InheritedValues != null && InheritedValues.Length > 0;
        }
        private bool HasOverriddenValues()
        {
            return Values != null && Values.Length > 0;
        }

        /// <summary>
        /// Get Current Values based on source flag
        /// </summary>
        /// <returns></returns>
        public Value[] GetCurrentValues()
        {
            return IsInherited ? InheritedValues : Values;
        }
    }
}
