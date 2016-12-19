using System;
using System.Collections.Generic;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects;
    using MDM.Core;
    using MDM.Core.Exceptions;

    /// <summary>
    /// Exposes methods or properties to set or get the collection of values.
    /// </summary>
    public interface IValueCollection : IEnumerable<Value>
    {
        #region Properties

        /// <summary>
        /// Get the count of no. of Values in ValueCollection
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Check if ValueCollection is read-only.
        /// </summary>
        bool IsReadOnly { get; }

        /// <summary>
        /// Indicates any of the values in current collection has invalid values.
        /// </summary>
        Boolean HasInvalidValues { get; set; }

        #endregion Properties

        #region Methods

        #region ICollection<IValue> Members

        /// <summary>
        /// Add Value object in collection
        /// </summary>
        /// <param name="item">Value to add in collection</param>
        void Add(IValue item);

        /// <summary>
        /// Determines whether the ValueCollection contains a specific Value.
        /// </summary>
        /// <param name="item">The Value object to locate in the ValueCollection.</param>
        /// <returns>
        /// <para>true : If Value found in ValueCollection</para>
        /// <para>false : If Value found not in ValueCollection</para>
        /// </returns>
        bool Contains(IValue item);

        /// <summary>
        /// Removes the first occurrence of a specific object from the ValueCollection.
        /// </summary>
        /// <param name="item">The Value object to remove from the ValueCollection.</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original ValueCollection</returns>
        bool Remove(IValue item);

        #endregion

        #region ToXml methods

        /// <summary>
        /// Get Xml representation of Value object
        /// </summary>
        /// <returns>Xml representation of object</returns>
        String ToXml();

        /// <summary>
        /// Get Xml representation of Value object
        /// </summary>
        /// <param name="serialization">serialization option. Based on the value selected, the different xml representation will be there</param>
        /// <param name="attributeDataType">Attribute Data Type</param>
        /// <returns>Xml representation of object</returns>
        String ToXml(ObjectSerialization serialization, AttributeDataType attributeDataType);

        #endregion ToXml methods

        #region Get value

        /// <summary>
        /// Get value from value collection based on ValueRefId
        /// </summary>
        /// <param name="valueRefId">ValueRefId to search</param>
        /// <returns>Value interface having given ValueRefId</returns>
        /// <exception cref="DuplicateObjectException">Raised if multiple values having same ValueRefId is found</exception>
        IValue GetByValueRefId( Int32 valueRefId );

        /// <summary>
        /// Get value from value collection based on Sequence
        /// </summary>
        /// <param name="sequence">Sequence to search</param>
        /// <returns>Value interface having given sequence</returns>
        /// <exception cref="DuplicateObjectException">Raised if multiple values having same Sequence is found</exception>
        IValue GetBySequence( Decimal sequence );

        /// <summary>
        /// Get values having given locale name
        /// </summary>
        /// <param name="locale">Locale to search</param>
        /// <returns>Value collection interface having given locale name</returns>
        /// <exception cref="ArgumentNullException">Raised if locale is empty</exception>
        IValueCollection GetByLocale( Core.LocaleEnum locale );

        #endregion Get value

        /// <summary>
        /// Removes all Values from collection
        /// </summary>
        void Clear();

        /// <summary>
        /// Clear each value's properties from current ValueCollection
        /// </summary>
        void ClearValues();

        /// <summary>
        /// Get next sequence no. Calculated from existing sequence value + 1
        /// </summary>
        /// <returns>Next sequence value (Last sequence value + 1)</returns>
        /// <exception cref="Exception">Raised when current _value is not initialized</exception>
        Decimal GetNextSequenceValue();

        /// <summary>
        /// Set provided object action to each instance of value in the collection
        /// </summary>
        /// <param name="action">Action to bet set</param>
        void SetAction(ObjectAction action);
        
        #endregion
    }
}
