using System;

namespace MDM.Interfaces
{
    using MDM.Core;
    using MDM.Core.Exceptions;

    /// <summary>
    /// Exposes methods or properties to set or get attributes.
    /// </summary>
    public interface IAttribute : IMDMObject
    {
        #region Properties

        /// <summary>
        /// Property for AttributeParentID (Attribute group ID)
        /// </summary>
        Int32 AttributeParentId { get; set; }

        /// <summary>
        /// Property for AttributeParentName (Attribute group short name) 
        /// </summary>
        String AttributeParentName { get; set; }

        /// <summary>
        /// Property for AttributeParentLongName (Attribute group long name) 
        /// </summary>
        String AttributeParentLongName{ get; set; }

        /// <summary>
        /// Property to decide if attribute is collection 
        /// </summary>
        Boolean IsCollection { get; set; }

        /// <summary>
        /// Property to decide if attribute is a complex attribute 
        /// </summary>
        Boolean IsComplex { get; set; }

        /// <summary>
        /// Property to decide if attribute is a lookup
        /// </summary>
        Boolean IsLookup { get; set; }

        /// <summary>
        /// Property which indicates whether attribute is read only
        /// </summary>
        Boolean ReadOnly { get; set; }

        /// <summary>
        /// Property which indicates whether attribute is required
        /// </summary>
        Boolean Required { get; set; }

        /// <summary>
        /// Property for attribute data type 
        /// </summary>
        AttributeDataType AttributeDataType{ get; set; }

        /// <summary>
        /// Property for attribute value source ('O' or 'I') 
        /// </summary>
        AttributeValueSource SourceFlag { get; set; }

        /// <summary>
        /// Property denoting the model type of an attribute. It indicates whether an attribute is common attribute or technical attribute.
        /// For possible values, see <see cref="AttributeModelType" />
        /// </summary>
        AttributeModelType AttributeModelType{ get; set; }

        /// <summary>
        /// Property denoting the type of attribute. for possible values, see <see cref="MDM.Core.AttributeTypeEnum"/>
        /// </summary>
        MDM.Core.AttributeTypeEnum AttributeType{ get; set; }

        /// <summary>
        /// Property denoting value reference id.
        /// For complex attributes, ValueRefId represents the PK of respective complex attribute (tbcx_) table
        /// </summary>
        Int32 InstanceRefId { get; set; }

        /// <summary>
        /// Property denoting sequence of current attribute.
        /// This is used in case of complex and complex collection attribute
        /// </summary>
        Decimal Sequence { get; set; }

        /// <summary>
        /// Property denoting the entity id this attribute belongs to
        /// </summary>
        Int64 EntityId { get; }

        /// <summary>
        /// Indicates if inherited value for attribute is invalid based on validations
        /// </summary>
        Boolean HasInvalidInheritedValues { get; set; }

        /// <summary>
        /// Indicates if overridden value for attribute is invalid based on validations
        /// </summary>
        Boolean HasInvalidOverriddenValues { get; set; }

        /// <summary>
        /// Indicates if current value for attribute is invalid based on validations
        /// </summary>
        Boolean HasInvalidValues { get; set; }

        #endregion

        #region Methods

        #region ToXml methods
        
        /// <summary>
        /// Get Xml representation of Attribute object
        /// </summary>
        /// <returns>Xml representation of object</returns>
        String ToXml();

        /// <summary>
        /// Get Xml representation of Attribute object
        /// </summary>
        /// <param name="serialization">serialization option. Based on the value selected, the different xml representation will be there</param>
        /// <returns>Xml representation of object</returns>
        String ToXml( ObjectSerialization serialization);//, Boolean attributeMetadataOnly = false );
        
        #endregion ToXml methods

        #region Get Attribute Value

        #region Get Overridden Value

        /// <summary>
        /// Get overridden values for current attribute
        /// </summary>
        /// <returns>Collection of overridden values for current attribute object</returns>
        IValueCollection GetOverriddenValues();

        /// <summary>
        /// Get overridden values for current attribute
        /// </summary>
        /// <param name="formatLocale">Locale in which value needs to be returned.</param>
        /// <returns>Collection of overridden values for current attribute object</returns>
        IValueCollection GetOverriddenValues(LocaleEnum formatLocale);

        /// <summary>
        /// Get overridden values for current attribute
        /// </summary>
        /// <returns>Collection of overridden values for current attribute object</returns>
        IValueCollection GetOverriddenValuesInvariant();

        /// <summary>
        /// Get AttrVal of overridden value.
        /// </summary>
        /// <returns>AttrVal of current attribute</returns>
        /// <exception cref="NotSupportedException">Value property cannot be accessible when attribute is collection.</exception>
        Object GetOverriddenValue();

        /// <summary>
        /// Get AttrVal of overridden value in given formatLocale
        /// </summary>
        /// <param name="formatLocale">Locale in which value needs to be returned.</param>
        /// <returns>AttrVal of current attribute formatted in given locale</returns>
        /// <exception cref="NotSupportedException">Value property cannot be accessible when attribute is collection.</exception>
        Object GetOverriddenValue(LocaleEnum formatLocale);

        /// <summary>
        /// Get AttrVal of overridden value.
        /// </summary>
        /// <returns>AttrVal of current attribute</returns>
        /// <exception cref="NotSupportedException">Value property cannot be accessible when attribute is collection.</exception>
        Object GetOverriddenValueInvariant();

        /// <summary>
        /// Get IValue  object for overridden value.
        /// </summary>
        /// <returns>IValue object for current attribute</returns>
        /// <exception cref="NotSupportedException">Value property cannot be accessible when attribute is collection.</exception>
        IValue GetOverriddenValueInstance();

        /// <summary>
        /// Get IValue  object for overridden value into formatLocale
        /// </summary>
        /// <param name="formatLocale">Locale in which value needs to be returned.</param>
        /// <returns>IValue object for current attribute having formatted value</returns>
        IValue GetOverriddenValueInstance(LocaleEnum formatLocale);

        /// <summary>
        /// Get IValue  object for overridden value.
        /// </summary>
        /// <returns>IValue object for current attribute</returns>
        IValue GetOverriddenValueInstanceInvariant();

        #endregion Get Overridden Value

        #region Get Inherited Value

        /// <summary>
        /// Get AttrVal of inherited value.
        /// </summary>
        /// <returns>AttrVal of current attribute</returns>
        /// <exception cref="NotSupportedException">Value property cannot be accessible when attribute is collection.</exception>
        Object GetInheritedValue();

        /// <summary>
        /// Get AttrVal of inherited value in formatted Locale.
        /// </summary>
        /// <param name="formatLocale">Locale in which value needs to be returned.</param>
        /// <returns>AttrVal of current attribute havinf formatted value</returns>
        Object GetInheritedValue(LocaleEnum formatLocale);

        /// <summary>
        /// Get AttrVal of inherited value.
        /// </summary>
        /// <returns>AttrVal of current attribute</returns>
        Object GetInheritedValueInvariant();

        /// <summary>
        /// Get IValue object for of inherited value.
        /// </summary>
        /// <returns>IValue for current attribute value</returns>
        /// <exception cref="NotSupportedException">Value property cannot be accessible when attribute is collection.</exception>
        IValue GetInheritedValueInstance();

        /// <summary>
        /// Get IValue object for of inherited value in given formate Locale
        /// </summary>
        /// <param name="formatLocale">Locale in which value needs to be returned.</param>
        /// <returns>IValue for current attribute value</returns>
        IValue GetInheritedValueInstance(LocaleEnum formatLocale);

        /// <summary>
        /// Get IValue object for of inherited value.
        /// </summary>
        /// <returns>IValue for current attribute value</returns>
        IValue GetInheritedValueInstanceInvariant();

        /// <summary>
        /// Get inherited values for current attribute
        /// </summary>
        /// <returns>Collection of inherited values for current attribute object</returns>
        IValueCollection GetInheritedValues();

        /// <summary>
        /// Get inherited values for current attribute
        /// </summary>
        /// <param name="formatLocale">Locale in which value needs to be returned.</param>
        /// <returns>Collection of inherited values for current attribute object</returns>
        IValueCollection GetInheritedValues(LocaleEnum formatLocale);

        /// <summary>
        /// Get inherited values for current attribute
        /// </summary>
        /// <returns>Collection of inherited values for current attribute object</returns>
        IValueCollection GetInheritedValuesInvariant();

        #endregion Get Inherited Value

        #region Get Current Value

        /// <summary>
        /// Get current value based on Source
        /// If Source = Overridden then returns OverriddenValues
        /// If Source = Inherited then returns InheritedValues
        /// </summary>
        /// <returns>Collection of values based on source flag</returns>
        IValueCollection GetCurrentValues();

        /// <summary>
        /// Get current value based on Source
        /// If Source = Overridden then returns OverriddenValues
        /// If Source = Inherited then returns InheritedValues
        /// </summary>
        /// <param name="formatLocale">Locale in which value needs to be returned.</param>
        /// <returns>Collection of values based on source flag</returns>
        IValueCollection GetCurrentValues(LocaleEnum formatLocale);

        /// <summary>
        /// Get current value based on Source
        /// If Source = Overridden then returns OverriddenValues
        /// If Source = Inherited then returns InheritedValues
        /// </summary>
        /// <returns>Collection of values based on source flag</returns>
        IValueCollection GetCurrentValuesInvariant();

        /// <summary>
        /// Returns the current (considering source flag) value of attribute as Object
        /// </summary>
        /// <returns>Current value (considering source flag)</returns>
        /// <exception cref="NotSupportedException">Value property cannot be accessible when attribute is collection.</exception>
        Object GetCurrentValue();

        /// <summary>
        /// Returns the current (considering source flag) value of attribute as Object
        /// </summary>
        /// <param name="formatLocale">Locale in which value needs to be returned.</param>
        /// <returns>Current value (considering source flag)</returns>
        Object GetCurrentValue(LocaleEnum formatLocale);

        /// <summary>
        /// Returns the current (considering source flag) value of attribute as Object
        /// </summary>
        /// <returns>Current value object (considering source flag)</returns>
        Object GetCurrentValueInvariant();

        /// <summary>
        /// Returns the current (considering source flag) value object of attribute as Object
        /// </summary>
        /// <returns>Current value object (considering source flag)</returns>
        /// <exception cref="NotSupportedException">Value property cannot be accessible when attribute is collection.</exception>
        IValue GetCurrentValueInstance();

        /// <summary>
        /// Returns the current (considering source flag) value of attribute as Object
        /// </summary>
        /// <param name="formatLocale">Locale in which value needs to be returned.</param>
        /// <returns>Current value object (considering source flag)</returns>
        IValue GetCurrentValueInstance(LocaleEnum formatLocale);

        /// <summary>
        /// Returns the current (considering source flag) value of attribute as Object
        /// </summary>
        /// <returns>Current value object (considering source flag)</returns>
        IValue GetCurrentValueInstanceInvariant();

        /// <summary>
        ///  Get the current value for the attribute.
        /// </summary>
        /// <example>
        /// <code language="c#" title="Get Attribute CurrentValue Instance In Specified Type"  source="..\MDM.APISamples\BusinessObjects\AttributeUtilities.cs" region="Get Attribute CurrentValue Instance In Specified Type"/>
        /// <code language="c#" title="Get Entity With Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>
        /// </example>
        /// <typeparam name="T">Indicates the data type of the result</typeparam>
        /// <returns>Returns the current value based on the requested data type if the data type is valid</returns>
        T GetCurrentValue<T>();

        /// <summary>
        ///  Get the current Invariant value for the attribute.
        /// </summary>
        /// <example>
        /// <code language="c#" title="Get Attribute CurrentValue Instance Invariant In Specified Type"  source="..\MDM.APISamples\BusinessObjects\AttributeUtilities.cs" region="Get Attribute CurrentValue Instance Invariant In Specified Type"/>
        /// <code language="c#" title="Get Entity With Attributes"  source="..\MDM.APISamples\EntityManager\Entity\EntityUtility.cs" region="GetEntityWithAttributes"/>
        /// </example>
        /// <typeparam name="T">Indicates the data type of the result</typeparam>
        /// <returns>Returns the current Invariant value based on the requested data type if the data type is valid</returns>
        T GetCurrentValueInvariant<T>();

        #endregion Get Current Value

        #endregion Get Attribute Value

        #region Set Attribute Value

        #region Set/Append Inherited values

        /// <summary>
        /// Sets value as inherited
        /// If current attribute's <see cref="SourceFlag"/> is Inherited then only it will save the value. Otherwise throws exception
        /// </summary>
        /// <param name="value">Value to set</param>
        /// <exception cref="ArgumentNullException">Value cannot be null</exception>
        /// <exception cref="InvalidSourceFlagException">Cannot change value when SourceFlag is Overridden.</exception>
        void SetInheritedValue(IValue value);

        /// <summary>
        /// Sets value as inherited
        /// If attribute's <see cref="SourceFlag"/> is Inherited then only it will save the value. Otherwise throws exception
        /// </summary>
        /// <param name="valueCollection">Values to set</param>
        /// <exception cref="ArgumentNullException">Value cannot be null</exception>
        /// <exception cref="InvalidSourceFlagException">Cannot change value when SourceFlag is Overridden.</exception>
        void SetInheritedValue(IValueCollection valueCollection);

        /// <summary>
        /// Sets value as inherited
        /// If attribute's <see cref="SourceFlag"/> is Inherited then only it will save the value. Otherwise throws exception
        /// </summary>
        /// <param name="value">Value to set</param>
        /// <exception cref="ArgumentNullException">Value cannot be null</exception>
        /// <exception cref="InvalidSourceFlagException">Cannot change value when SourceFlag is Overridden.</exception>
        void SetInheritedValueInvariant(IValue value);

        /// <summary>
        /// Appends new value into the already existing inherited values
        /// </summary>
        /// <param name="value">Value which needs to be added</param>
        /// <exception cref="ArgumentNullException">Thrown when Value parameter is null</exception>
        /// <exception cref="InvalidSourceFlagException">Cannot append value when SourceFlag is Overridden.</exception>
        void AppendInheritedValue(IValue value);

        /// <summary>
        /// Appends new value into the already existing inherited values
        /// </summary>
        /// <param name="value">Value which needs to be added</param>
        /// <exception cref="ArgumentNullException">Value cannot be null</exception>
        /// <exception cref="InvalidSourceFlagException">Cannot change value when SourceFlag is Overridden.</exception>
        void AppendInheritedValueInvariant(IValue value);

        #endregion Set/Append Inherited values

        #region Set/Append Overridden values

        /// <summary>
        /// Overrides already existing value with new value.
        /// If current attribute's <see cref="SourceFlag"/> is Overridden then only it will save the value. Otherwise throws exception
        /// </summary>
        /// <param name="value">New value to set in overridden attribute collection.</param>
        /// <exception cref="ArgumentNullException">Value cannot be null</exception>
        /// <exception cref="InvalidSourceFlagException">Cannot change value when SourceFlag is Inherited.</exception>
        void SetValue(IValue value);

        /// <summary>
        /// Set overridden attribute value.
        /// </summary>
        /// <param name="attrVal">Value to set for current attribute</param>
        /// <exception cref="InvalidSourceFlagException">Cannot change value when SourceFlag is Inherited</exception>
        /// <exception cref="NotSupportedException">Value property cannot be accessible when attribute is collection</exception>
        void SetValue(Object attrVal);

        /// <summary>
        /// Overrides already existing value with new value.
        /// If current attribute's <see cref="SourceFlag"/> is Overridden then only it will save the value. Otherwise throws exception
        /// </summary>
        /// <param name="val">Value to set for current attribute</param>
        /// <param name="formatLocale">Locale in which value needs to be set.</param>
        /// <exception cref="InvalidSourceFlagException">Cannot change value when SourceFlag is Inherited</exception>
        /// <exception cref="NotSupportedException">Value property cannot be accessible when attribute is collection</exception>
        void SetValue(IValue val, LocaleEnum formatLocale);

        /// <summary>
        /// Overrides already existing value with new value.
        /// If current attribute's <see cref="SourceFlag"/> is Overridden then only it will save the value. Otherwise throws exception
        /// </summary>
        /// <param name="val">Value to set for current attribute</param>
        /// <exception cref="InvalidSourceFlagException">Cannot change value when SourceFlag is Inherited</exception>
        /// <exception cref="NotSupportedException">Value property cannot be accessible when attribute is collection</exception>
        void SetValueInvariant(IValue val);

        /// <summary>
        /// Overrides already existing value with new values.
        /// If current attribute's <see cref="SourceFlag"/> is Overridden then only it will save the value. Otherwise throws exception
        /// </summary>
        /// <param name="valueCollection">New value to set in overridden attribute collection.</param>
        /// <exception cref="ArgumentNullException">Value cannot be null</exception>
        /// <exception cref="InvalidSourceFlagException">Cannot change value when SourceFlag is Inherited.</exception>
        /// <exception cref="InvalidOperationException">Cannot add multiple values to collection if IsCollection is false</exception>
        void SetValue(IValueCollection valueCollection);

        /// <summary>
        /// Overrides already existing value with new values.
        /// If current attribute's <see cref="SourceFlag"/> is Overridden then only it will save the value. Otherwise throws exception
        /// </summary>
        /// <param name="valueCollection">New value to set in overridden attribute collection.</param>
        /// <param name="formatLocale">Locale in which value needs to be set.</param>
        /// <exception cref="ArgumentNullException">Value cannot be null</exception>
        /// <exception cref="InvalidSourceFlagException">Cannot change value when SourceFlag is Inherited.</exception>
        /// <exception cref="InvalidOperationException">Cannot add multiple values to collection if IsCollection is false</exception>
        void SetValue(IValueCollection valueCollection, LocaleEnum formatLocale);

        /// <summary>
        /// Overrides already existing value with new values.
        /// If current attribute's <see cref="SourceFlag"/> is Overridden then only it will save the value. Otherwise throws exception
        /// </summary>
        /// <param name="values">New value to set in overridden attribute collection.</param>
        /// <exception cref="ArgumentNullException">Value cannot be null</exception>
        /// <exception cref="InvalidSourceFlagException">Cannot change value when SourceFlag is Inherited.</exception>
        /// <exception cref="InvalidOperationException">Cannot add multiple values to collection if IsCollection is false</exception>
        void SetValueInvariant(IValueCollection values);

        /// <summary>
        /// Appends new value in already existing values.
        /// If current attribute's <see cref="SourceFlag"/> is Overridden then only it will save the value. Otherwise throws exception
        /// If more than 1 values are being added and attribute is not collection (IsCollection = false) then throws error 
        /// </summary>
        /// <param name="value">New value to add in overridden attribute collection.</param>
        /// <exception cref="InvalidSourceFlagException">Value cannot be null</exception>
        /// <exception cref="InvalidSourceFlagException">Cannot change value when SourceFlag is Inherited.</exception>
        /// <exception cref="InvalidOperationException">Cannot add multiple values to collection if IsCollection is false</exception>
        void AppendValue(IValue value);

        /// <summary>
        /// Appends new value in already existing values.
        /// If current attribute's <see cref="SourceFlag"/> is Overridden then only it will save the value. Otherwise throws exception
        /// If more than 1 values are being added and attribute is not collection (IsCollection = false) then throws error 
        /// </summary>
        /// <param name="value">New value to add in overridden attribute collection.</param>
        /// <param name="formatLocale">Locale in which value needs to be set.</param>
        /// <exception cref="InvalidSourceFlagException">Value cannot be null</exception>
        /// <exception cref="InvalidSourceFlagException">Cannot change value when SourceFlag is Inherited.</exception>
        /// <exception cref="InvalidOperationException">Cannot add multiple values to collection if IsCollection is false</exception>
        void AppendValue(IValue value, LocaleEnum formatLocale);

        /// <summary>
        /// Appends new value in already existing values.
        /// If current attribute's <see cref="SourceFlag"/> is Overridden then only it will save the value. Otherwise throws exception
        /// If more than 1 values are being added and attribute is not collection (IsCollection = false) then throws error 
        /// </summary>
        /// <param name="value">New value to add in overridden attribute collection.</param>
        /// <exception cref="InvalidSourceFlagException">Value cannot be null</exception>
        /// <exception cref="InvalidSourceFlagException">Cannot change value when SourceFlag is Inherited.</exception>
        /// <exception cref="InvalidOperationException">Cannot add multiple values to collection if IsCollection is false</exception>
        void AppendValueInvariant(IValue value);

        /// <summary>
        /// Appends new value in already existing values.
        /// If current attribute's <see cref="SourceFlag"/> is Overridden then only it will save the value. Otherwise throws exception
        /// If more than 1 values are being added and attribute is not collection (IsCollection = false) then throws error 
        /// </summary>
        /// <param name="valueCollection">New value to add in overridden attribute collection.</param>
        /// <exception cref="ArgumentNullException">ValueCollection cannot be null</exception>
        /// <exception cref="InvalidSourceFlagException">Cannot change value when SourceFlag is Inherited.</exception>
        /// <exception cref="InvalidOperationException">Cannot add multiple values to collection if IsCollection is false</exception>
        void AppendValue(IValueCollection valueCollection);

        /// <summary>
        /// Appends new value in already existing values.
        /// If current attribute's <see cref="SourceFlag"/> is Overridden then only it will save the value. Otherwise throws exception
        /// If more than 1 values are being added and attribute is not collection (IsCollection = false) then throws error 
        /// </summary>
        /// <param name="valueCollection">New value to add in overridden attribute collection.</param>
        /// <param name="formatLocale">Locale in which value needs to be set.</param>
        /// <exception cref="InvalidSourceFlagException">Value cannot be null</exception>
        /// <exception cref="InvalidSourceFlagException">Cannot change value when SourceFlag is Inherited.</exception>
        /// <exception cref="InvalidOperationException">Cannot add multiple values to collection if IsCollection is false</exception>
        void AppendValue(IValueCollection valueCollection, LocaleEnum formatLocale);
        
        /// <summary>
        /// Appends new value in already existing values.
        /// If current attribute's <see cref="SourceFlag"/> is Overridden then only it will save the value. Otherwise throws exception
        /// If more than 1 values are being added and attribute is not collection (IsCollection = false) then throws error 
        /// </summary>
        /// <param name="valueCollection">New value to add in overridden attribute collection.</param>
        /// <exception cref="InvalidSourceFlagException">Value cannot be null</exception>
        /// <exception cref="InvalidSourceFlagException">Cannot change value when SourceFlag is Inherited.</exception>
        /// <exception cref="InvalidOperationException">Cannot add multiple values to collection if IsCollection is false</exception>
        void AppendValueInvariant(IValueCollection valueCollection);

        #endregion Set/Append Overridden values

        #endregion Set Attribute Value

        #region Check value

        /// <summary>
        /// Check if attribute has any value (without considering SourceFlag. overridden or inherited). It will ignore actions by default.
        /// </summary>
        /// <returns>True : If value collection (Inherited or Overridden) has any value. False : otherwise</returns>
        Boolean HasAnyValue();

        /// <summary>
        /// Check if attribute has any value (considering current source flag). It will ignore actions by default.
        /// </summary>
        /// <returns>True : Based on current source flag, respective value collection has value. False : otherwise</returns>
        Boolean HasValue();

        /// <summary>
        /// Check if attribute has any value (without considering SourceFlag. overridden or inherited)
        /// <param name="ignoreAction">If set to true, action will not be considered, which means the values getting deleted are also counted as values. If set to false, action will be considered and value which are marked for delete are not counted as value</param>
        /// </summary>
        /// <returns>True : If value collection (Inherited or Overridden) has any value. False : otherwise</returns>
        Boolean HasAnyValue(Boolean ignoreAction);

        /// <summary>
        /// Check if attribute has any value (considering current source flag)
        /// <param name="ignoreAction">If set to true, action will not be considered, which means the values getting deleted are also counted as values. If set to false, action will be considered and value which are marked for delete are not counted as value</param>
        /// </summary>
        /// <returns>True : Based on current source flag, respective value collection has value. False : otherwise</returns>
        Boolean HasValue(Boolean ignoreAction);

        /// <summary>
        /// check if attribute value has been changed
        /// </summary>
        /// <returns>True : id current action is Create OR Update OR Delete. False : otherwise</returns>
        bool CheckHasChanged();

        #endregion Check value

        #region Clear values

        /// <summary>
        /// Clear values.
        /// If current attribute's <see cref="SourceFlag"/> is Overridden then only it will clear value. Otherwise throws exception
        /// </summary>
        /// <exception cref="InvalidSourceFlagException">Cannot change value when SourceFlag is Inherited.</exception>
        /// <exception cref="ArgumentNullException">OverriddenValues is null. No values to clear</exception>
        void ClearValue();

        /// <summary>
        /// Removes the first occurrence of a specific value from the current attribute value collection.
        /// </summary>
        /// <param name="value">The value object to remove from the current attribute value collection.</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original Value collection</returns>
        /// <exception cref="ArgumentNullException">Value cannot be null</exception>
        /// <exception cref="InvalidSourceFlagException">Cannot change value when SourceFlag is Inherited</exception>
        /// <exception cref="ArgumentNullException">OverriddenValues is null. No values to clear</exception>
        /// <exception cref="InvalidOperationException">Removing value is not supported for Non collection attribute</exception>
        Boolean RemoveValue( IValue value );

        #endregion Clear values

        /// <summary>
        /// Get child attributes for current attribute
        /// </summary>
        /// <returns>Attribute collection interface representing child attributes</returns>
        /// <exception cref="NullReferenceException">Thrown if child attributes is null</exception>
        IAttributeCollection GetChildAttributes();

        /// <summary>
        /// Get child attributes for current attribute
        /// </summary>
        /// <returns>Attribute collection interface representing child attributes</returns>
        /// <exception cref="NullReferenceException">Thrown if child attributes is null</exception>
        IAttributeCollection GetChildAttributes(AttributeValueSource sourceFlag);

        #region complex attribute helper method

        /// <summary>
        /// Get complex child attribute collection from complex attribute.
        /// <para>
        /// If attribute is complex collection then returns collection of child attributes at 1st position
        /// </para>
        /// </summary>
        /// <returns>Collection interface of complex child attributes</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when attribute is not complex attribute.
        /// <para>
        /// Also raised when attribute is not complex attribute. If current attribute is either complex attribute instance or complex child attribute then this error is raised.
        /// </para>
        /// </exception>
        IAttributeCollection GetComplexChildAttributes();

        /// <summary>
        /// Get immediate child attribute collection of hierarchy attribute at given level.
        /// <para>
        /// If attribute is hierarchy collection then returns collection of child attributes at 1st position
        /// </para>
        /// </summary>
        /// <returns>Collection interface of immediate child attributes</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when attribute is not hierarchy attribute.
        /// <para>
        /// Also raised when attribute is not immediate attribute. If current attribute is either immediate attribute instance or immediate child attribute then this error is raised.
        /// </para>
        /// </exception>
        IAttributeCollection GetHierarchicalChildAttributes();

        /// <summary>
        /// Get complex attribute instance based on InstanceRefId
        /// </summary>
        /// <param name="instanceRefId">InstanceRefId of complex attribute instance which is to be fetched</param>
        /// <returns>Attribute interface having given InstanceRefId.</returns>
        /// <exception cref="InvalidOperationException">
        ///     Thrown when attribute is not complex attribute.
        ///     <para>
        ///         Also raised when attribute is not complex attribute. If current attribute is either complex attribute instance or complex child attribute then this error is raised.
        ///     </para>
        /// </exception>
        /// <exception cref="DuplicateObjectException">Thrown when multiple complex attributes having same InstanceRefId is found.</exception>
        IAttribute GetComplexAttributeInstanceByInstanceRefId( Int32 instanceRefId );

        /// <summary>
        /// Get complex attribute instance at given sequence.
        /// </summary>
        /// <param name="sequence">Sequence of attribute instance which is to be searched within complex attribute instances</param>
        /// <returns>Attribute interface having at given sequence</returns>
        /// <exception cref="InvalidOperationException">
        ///     Thrown when attribute is not complex attribute.
        ///     <para>
        ///         Also raised when attribute is not complex attribute. If current attribute is either complex attribute instance or complex child attribute then this error is raised.
        ///     </para>
        /// </exception>
        /// <exception cref="DuplicateObjectException">Thrown when there are values having same sequence.</exception>
        IAttribute GetComplexAttributeInstanceBySequence( Decimal sequence );

        /// <summary>
        /// Get complex child attributes for complex attribute instance having given InstanceRefId
        /// </summary>
        /// <param name="instanceRefId">InstanceRefId to be searched</param>
        /// <returns>Collection of attribute interfaces having given InstanceRefId</returns>
        /// <exception cref="InvalidOperationException">
        ///     Thrown when attribute is not complex attribute.
        ///     <para>
        ///         Also raised when attribute is not complex attribute. If current attribute is either complex attribute instance or complex child attribute then this error is raised.
        ///     </para>
        /// </exception>
        /// <exception cref="DuplicateObjectException">Thrown when multiple complex attribute instance having same InstanceRefId is found.</exception>
        IAttributeCollection GetComplexChildAttributesByInstanceRefId( Int32 instanceRefId );

        /// <summary>
        /// Get complex child attributes for complex attribute instance having given sequence
        /// </summary>
        /// <param name="sequence">Sequence to be searched</param>
        /// <returns>Collection of attribute interfaces having given InstanceRefId</returns>
        /// <exception cref="InvalidOperationException">
        ///     Thrown when attribute is not complex attribute.
        ///     <para>
        ///         Also raised when attribute is not complex attribute. If current attribute is either complex attribute instance or complex child attribute then this error is raised.
        ///     </para>
        /// </exception>
        /// <exception cref="DuplicateObjectException">Thrown when multiple complex attribute instance having same Sequence is found.</exception>
        IAttributeCollection GetComplexChildAttributesBySequence( Decimal sequence );

        /// <summary>
        /// Get new model for complex child record.
        /// </summary>
        /// <returns>Attribute collection interface of complex child attribute</returns>
        /// <exception cref="InvalidOperationException">
        ///     Thrown when attribute is not complex attribute.
        ///     <para>
        ///         Also raised when attribute is not complex attribute. If current attribute is either complex attribute instance or complex child attribute then this error is raised.
        ///     </para>
        /// </exception>
        /// <exception cref="Exception">Thrown in case there is no child attribute model loaded for complex attribute.
        /// Currently there is no mechanism available through which we can make a DB call and get child attribute model.</exception>
        IAttributeCollection NewComplexChildRecord();

        /// <summary>
        /// Add new complex attribute instance in complex attribute.
        /// </summary>
        /// <param name="childAttributes">Complex child attributes to add in complex attribute.</param>
        /// <exception cref="InvalidOperationException">
        /// Raised when 
        /// <para>
        ///  - Attribute is not complex attribute.
        /// </para>
        /// <para>
        ///  - Attribute is not level 1 complex attribute. If current attribute is either complex attribute instance or complex child attribute then this error is raised.
        /// </para>
        /// <para>
        /// - Attribute is not collection and more than 1 values are being added.
        /// </para>
        /// </exception>
        /// <exception cref="InvalidSourceFlagException">
        /// Raised when 
        /// <para>
        /// - Source flag is not overridden AttributeValueSource.Overridden and user is trying to add value
        /// </para>
        /// <para>
        /// - Any of childAttributes from argument is not there in complex child attribute model.
        /// </para>
        /// </exception>
        IAttribute AddComplexChildRecord(IAttributeCollection childAttributes);

        /// <summary>
        /// Add new complex attribute instance in complex attribute at given sequence.
        /// </summary>
        /// <param name="childAttributes">Complex child attributes to add in complex attribute.</param>
        /// <param name="sequence">The zero-based Sequence at which complex child attribute should be inserted.</param>
        /// <exception cref="InvalidOperationException">
        ///     Raised when 
        ///     <para>
        ///      - Attribute is not complex attribute.
        ///     </para>
        ///     <para>
        ///      - Attribute is not level 1 complex attribute. If current attribute is either complex attribute instance or complex child attribute then this error is raised.
        ///     </para>
        ///     <para>
        ///     - Attribute is not collection and more than 1 values are being added.
        ///     </para>
        /// </exception>
        /// <exception cref="InvalidSourceFlagException">
        ///     Raised when 
        ///     <para>
        ///     - Source flag is not overridden AttributeValueSource.Overridden and user is trying to add value
        ///     </para>
        ///     <para>
        ///     - Any of childAttributes from argument is not there in complex child attribute model.
        ///     </para>
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///     Raised when ChildAttributes to add is null
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     If sequence is less than 0
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Raised if sequence is greater than no. of values.
        /// </exception>
        IAttribute AddComplexChildRecordAt(Decimal sequence, IAttributeCollection childAttributes);

        /// <summary>
        /// Remove value from Non collection complex attribute.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Raised when 
        /// <para>
        ///  - Attribute is not complex attribute.
        /// </para>
        /// <para>
        ///  - Attribute is not level 1 complex attribute. If current attribute is either complex attribute instance or complex child attribute then this error is raised.
        /// </para>
        /// <para>
        ///  - Attribute is collection
        /// </para>
        /// </exception>
        /// <exception cref="InvalidSourceFlagException">
        /// Raised when 
        /// <para>
        /// - Source flag is not overridden AttributeValueSource.Overridden and user is trying to add value
        /// </para>
        /// </exception>
        void RemoveComplexChildRecord();

        /// <summary>
        /// Remove complex child attribute based on Sequence.
        /// </summary>
        /// <param name="sequence">Sequence to remove</param>
        /// <exception cref="InvalidOperationException">
        /// Raised when 
        /// <para>
        ///  - Attribute is not complex attribute.
        /// </para>
        /// <para>
        ///  - Attribute is not level 1 complex attribute. If current attribute is either complex attribute instance or complex child attribute then this error is raised.
        /// </para>
        /// </exception>
        /// <exception cref="InvalidSourceFlagException">
        /// Raised when 
        /// <para>
        /// - Source flag is not overridden AttributeValueSource.Overridden and user is trying to add value
        /// </para>
        /// </exception>
        /// <exception cref="ArgumentException">Raised when sequence is less than 0</exception>
        /// <exception cref="ArgumentOutOfRangeException">Raised when sequence value is more than no. of values in attribute</exception>
        void RemoveComplexChildRecordBySequence( Decimal sequence );

        /// <summary>
        /// Remove complex child attribute based on ValueRefId.
        /// </summary>
        /// <param name="instanceRefId">Indicates the instance reference identifier to remove</param>
        /// <exception cref="InvalidOperationException">
        /// Raised when 
        /// <para>
        ///  - Attribute is not complex attribute.
        /// </para>
        /// <para>
        ///  - Attribute is not level 1 complex attribute. If current attribute is either complex attribute instance or complex child attribute then this error is raised.
        /// </para>
        /// </exception>
        /// <exception cref="InvalidSourceFlagException">
        /// Raised when 
        /// <para>
        /// - Source flag is not overridden AttributeValueSource.Overridden and user is trying to add value
        /// </para>
        /// </exception>
        /// <exception cref="ArgumentException">Raised when ValueRefId is less than 0</exception>
        /// <exception cref="ArgumentOutOfRangeException">Raised when sequence value is more than no. of values in attribute</exception>
        /// <exception cref="Exception">Raised when no Value is found with given ValueRefId</exception>
        void RemoveComplexChildRecordByInstanceRefId( Int32 instanceRefId );

        #endregion complex attribute helper method

        /// <summary>
        /// Delta Merge of Attribute Values
        /// </summary>
        /// <param name="deltaAttribute">Attribute that needs to be merged.</param>
        /// <param name="flushExistingValues">True if existing values needs to be flushed else false</param>
        /// <param name="iCallerContext">Indicates the name of application and the module that are performing the action</param>
        /// <returns>Merged attribute instance</returns>
        IAttribute MergeDelta(IAttribute deltaAttribute, Boolean flushExistingValues, ICallerContext iCallerContext);

        /// <summary>
        /// Delta Merge of Attribute Values
        /// </summary>
        /// <param name="deltaAttribute">Attribute that needs to be merged.</param>
        /// <param name="flushExistingValues">True if existing values needs to be flushed else false</param>
        /// <param name="stringComparison">Comparison options to be used while finding changes</param>
        /// <param name="iCallerContext">Indicates the name of application and the module that are performing the action</param>
        /// <returns>Merged attribute instance</returns>
        IAttribute MergeDelta(IAttribute deltaAttribute, Boolean flushExistingValues, StringComparison stringComparison, ICallerContext iCallerContext);

        /// <summary>
        /// Delta Merge of Attribute Values based on the defined merge behavior
        /// </summary>
        /// <param name="deltaAttribute">Attribute that needs to be merged</param>
        /// <param name="flushExistingValues">True if existing values needs to be flushed else false</param>
        /// <param name="stringComparision">Comparison options to be used while finding changes</param>
        /// <param name="attributeCompareAndMergeBehavior">merge behavior of the attribute values</param>
        /// <param name="callerContext">Indicates the name of application and the module that are performing the action</param>
        /// <returns>Merged attribute instance</returns>
        IAttribute MergeDelta(IAttribute deltaAttribute, Boolean flushExistingValues, StringComparison stringComparision, AttributeCompareAndMergeBehavior attributeCompareAndMergeBehavior, ICallerContext callerContext);

        /// <summary>
        /// Delta Merge of Attribute Values used by lineage-based merge
        /// </summary>
        /// <param name="source">Attribute that needs to be merged.</param>
        /// <param name="collectionStrategy">Strategy of collection merging</param>
        /// <param name="allowInhiritantToOverridenMerging">Flag indicates whether I(source) to O(target) merge allowed</param>
        /// <param name="isSourceProcessingEnabled">Indicates if needed process source info</param>
        /// <returns>Merged attribute instance</returns>
        IAttribute MergeDelta(IAttribute source, CollectionStrategy collectionStrategy, Boolean allowInhiritantToOverridenMerging, Boolean isSourceProcessingEnabled = false);

        /// <summary>
        /// Delta Merge of Attribute Values used by lineage-based merge
        /// </summary>
        /// <param name="source">Attribute that needs to be merged.</param>
        /// <param name="collectionStrategy">Strategy of collection merging</param>
        /// <param name="allowInhiritantToOverridenMerging">Flag indicates whether I(source) to O(target) merge allowed</param>
        /// <param name="stringComparison">Comparision options to be used while finding changes</param>
        /// <param name="isSourceProcessingEnabled">Indicates if need process source info</param>
        /// <returns>Merged attribute instance</returns>
        IAttribute MergeDelta(IAttribute source, CollectionStrategy collectionStrategy, Boolean allowInhiritantToOverridenMerging, StringComparison stringComparison, Boolean isSourceProcessingEnabled = false);

        #endregion

    }
}
