using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace RS.MDM.ComponentModel
{
    /// <summary>
    /// Dynamically control the read/write of an object's property
    /// </summary>
    public sealed class DynamicPropertyDescriptor : PropertyDescriptor
    {
        #region Fields

        /// <summary>
        /// field for property descriptor
        /// </summary>
        private PropertyDescriptor _propertyDescriptor;

        /// <summary>
        /// field for readonly flag
        /// </summary>
        private bool _readOnly;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor with PropertyDescriptor and readonly flag as input parameters
        /// </summary>
        /// <param name="propertyDescriptor">Indicates the propertydescriptor that needs to be made readonly</param>
        /// <param name="readOnly">Indicates the readonly flag</param>
        public DynamicPropertyDescriptor(PropertyDescriptor propertyDescriptor, bool readOnly)
            : base(propertyDescriptor)
        {
            this._propertyDescriptor = propertyDescriptor;
            this._readOnly = readOnly;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the type of the component
        /// </summary>
        public override Type ComponentType
        {
            get
            {
                return this._propertyDescriptor.ComponentType;
            }
        }

        /// <summary>
        /// Gets a flag that denotes if the Property is readonly
        /// </summary>
        public override bool IsReadOnly
        {
            get
            {
                return this._readOnly;
            }
        }

        /// <summary>
        /// Gets the type of the property
        /// </summary>
        public override Type PropertyType
        {
            get
            {
                return this._propertyDescriptor.PropertyType;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returns a boolean value that indicates if the property can be reset
        /// </summary>
        /// <param name="component">The queries object</param>
        /// <returns>A boolean value that indicates if the property can be reset</returns>
        public override bool CanResetValue(object component)
        {
            return this._propertyDescriptor.CanResetValue(component);
        }

        /// <summary>
        /// Gets the value of a component
        /// </summary>
        /// <param name="component">Indicates a component for which the value needs to be extracted</param>
        /// <returns>A value of type object</returns>
        public override object GetValue(object component)
        {
            return this._propertyDescriptor.GetValue(component);
        }

        /// <summary>
        /// Resets the value of a component
        /// </summary>
        /// <param name="component">Indicates a component for which the value needs to be reset</param>
        public override void ResetValue(object component)
        {
            this._propertyDescriptor.ResetValue(component);
        }

        /// <summary>
        /// Sets the value for a component
        /// </summary>
        /// <param name="component">Indicates a component for which the value needs to be set</param>
        /// <param name="value">Indicates the value that needs to be set on the component</param>
        public override void SetValue(object component, object value)
        {
            this._propertyDescriptor.SetValue(component, value);
        }

        /// <summary>
        /// Return a boolean value that indicates if the value needs to be serialized
        /// </summary>
        /// <param name="component">The input component</param>
        /// <returns>A boolean value that indicates if the value needs to be serialized</returns>
        public override bool ShouldSerializeValue(object component)
        {
            return this._propertyDescriptor.ShouldSerializeValue(component);
        }

        #endregion
    }
}
