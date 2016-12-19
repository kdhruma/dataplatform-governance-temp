using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Globalization;

namespace RS.MDM.ComponentModel
{
    /// <summary>
    /// Provides functionality to expand an item in the property grid
    /// </summary>
    public sealed class StringExpandableObjectConvertor : ExpandableObjectConverter
    {
        #region Methods

        /// <summary>
        /// Returns a string representation of an object in the propertygrid
        /// </summary>
        /// <param name="context">Indicates the context of the propertygrid</param>
        /// <param name="culture">Indicates the culture of the current thread</param>
        /// <param name="value">Indicates the value of an object</param>
        /// <param name="destinationType">Indicates the type of the destination</param>
        /// <returns>A string representation of an object in the propertygrid</returns>
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if ((destinationType.ToString() == "System.String") && (value is string))
            {
                return "Click here to edit";
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }

        #endregion

    }

 

}
