using System;

namespace MDM.Interfaces
{
    using MDM.Core;
    using MDM.BusinessObjects;

    /// <summary>
    /// Exposes methods or properties to set or get word list.
    /// </summary>
    public interface IWordList : IMDMObject, ICloneable
    {
        /// <summary>
        /// Denotes word elements
        /// </summary>
        WordElementsCollection WordElements { get; set; }

        /// <summary>
        /// Denotes word list type id
        /// </summary>
        Int32? WordListTypeId { get; set; }

        /// <summary>
        /// Denotes word breaker set id
        /// </summary>
        Int32? WordBreakerSetId { get; set; }

        /// <summary>
        /// Denotes create date time
        /// </summary>
        DateTime? CreateDateTime { get; set; }

        /// <summary>
        /// Denotes last modification date time
        /// </summary>
        DateTime? ModDateTime { get; set; }

        /// <summary>
        /// Denotes user that created current word list
        /// </summary>
        String CreateUser { get; set; }

        /// <summary>
        /// Denotes the last user that modified current word list
        /// </summary>
        String ModUser { get; set; }

        /// <summary>
        /// Denotes create program
        /// </summary>
        String CreateProgram { get; set; }

        /// <summary>
        /// Denotes the last program that modified current word list
        /// </summary>
        String ModProgram { get; set; }
    }
}