using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RS.MDM.ComponentModel
{
    /// <summary>
    /// Provides functionality to expand an item in the property grid
    /// </summary>
    public class ExpandableObjectConverter : System.ComponentModel.ExpandableObjectConverter
    {
        /// <summary>
        /// Returns a string representation of an object in the propertygrid
        /// </summary>
        /// <param name="context">Indicates the context of the propertygrid</param>
        /// <param name="culture">Indicates the culture of the current thread</param>
        /// <param name="value">Indicates the value of an object</param>
        /// <param name="destinationType">Indicates the type of the destination</param>
        /// <returns>A string representation of an object in the propertygrid</returns>
        public override object ConvertTo(System.ComponentModel.ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType.ToString() == "System.String")
            {
                if (value != null && typeof(RS.MDM.Object).IsAssignableFrom(value.GetType()))
                {
                    string _name = ((RS.MDM.Object)value).Name;
                    if (!string.IsNullOrEmpty(_name))
                    {
                        return _name;
                    }
                }
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
