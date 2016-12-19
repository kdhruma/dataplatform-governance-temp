using System;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get iGeneric Search Rule.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IGenericSearchRule<T>
    {

        /// <summary>
        /// Specifies generic element column representation
        /// </summary>
        T Column { get; set; }

        /// <summary>
        /// Specifies search operator
        /// </summary>
        SearchOperator Operator { get; set; }

        /// <summary>
        /// Specifies search term
        /// </summary>
        String SearchTerm { get; set; }
    }
}