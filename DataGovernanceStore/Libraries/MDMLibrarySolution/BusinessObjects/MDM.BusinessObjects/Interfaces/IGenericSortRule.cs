using System;

namespace MDM.Interfaces
{
    /// <summary>
    /// Exposes methods or properties to set or get iGeneric Sort Rule.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IGenericSortRule<T>
    {
        /// <summary>
        /// Specifies generic element column representation
        /// </summary>
        T Column { get; set; }

        /// <summary>
        /// Specifies order of sorting
        /// </summary>
        Boolean IsDescendingOrder { get; set; }
    }
}