using System;
using System.Text;

namespace MDM.BusinessObjects.DQMNormalization
{
    using MDM.Interfaces;

    /// <summary>
    /// This class contains helper methods for Normalization
    /// </summary>
    public class NormalizationHelper
    {
        /// <summary>
        /// Formats Attribute Value for later usage in NormalizationResult structure
        /// </summary>
        /// <param name="attribute">Attribute</param>
        /// <returns></returns>
        public static  String FormatAttributeValueToNormalizationResultFormat(Attribute attribute)
        {
            if (!attribute.IsCollection)
            {
                var tmp = attribute.GetCurrentValueInvariant();
                if (tmp == null)
                {
                    return String.Empty;
                }
                return tmp.ToString();
            }
            else
            {
                StringBuilder result = new StringBuilder();
                IValueCollection values = attribute.GetCurrentValues();
                if (values != null && values.Count > 0)
                {
                    foreach (Value currentValue in values)
                    {
                        if (result.Length > 0)
                        {
                            result.Append(',');
                        }
                        result.Append(currentValue.AttrVal.ToString());
                    }
                }
                return result.ToString();
            }
        }
    }
}