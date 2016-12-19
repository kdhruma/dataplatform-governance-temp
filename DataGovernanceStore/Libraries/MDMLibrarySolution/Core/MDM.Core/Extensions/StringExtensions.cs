using System;
using System.Globalization;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace MDM.Core.Extensions
{
    /// <summary>
    /// Class providing extensions for string
    /// </summary>
    public static class StringExtensions
    {
        #region Constants

        //ExecutableStringPattern for finding javascript tag
        const String ExecutableStringPattern = "<script(?:(?!script>)[\\s\\S])*script>";

        #endregion Constants

        /// <summary>
        /// Checks string has value
        /// </summary>
        /// <param name="s">String to check</param>
        /// <returns>True if String has value</returns>
        public static bool HasValue(this string s)
        {
            return !string.IsNullOrEmpty(s);
        }

        /// <summary>
        /// Checks string has no value
        /// </summary>
        /// <param name="s">String to check</param>
        /// <returns>true if not have value</returns>
        public static bool HasNoValue(this string s)
        {
            return string.IsNullOrEmpty(s);
        }

        /// <summary>
        /// Get Boolean value
        /// </summary>
        /// <param name="val">String Value</param>
        /// <returns>Boolean</returns>
        public static bool GetBool(this string val)
        {
            return GetBool(val, false);
        }

        /// <summary>
        /// Convert String value into Boolean
        /// </summary>
        /// <param name="val">Value to be convert</param>
        /// <param name="defaultValue">default boolean value </param>
        /// <returns>Boolean</returns>
        public static bool GetBool(this string val, bool defaultValue)
        {
            bool result;

            return String.IsNullOrEmpty(val) ||
                   !bool.TryParse(val, out result)
                       ? defaultValue
                       : result;
        }

        /// <summary>
        /// Convert String value into integer
        /// </summary>
        /// <param name="val">Value to convert</param>
        /// <returns>Converted Integer value</returns>
        public static int GetInt(this string val)
        {
            return GetInt(val, 0);
        }

        /// <summary>
        /// Convert String value into integer
        /// </summary>
        /// <param name="val">Value to convert</param>
        /// <param name="defaultValue">Default value </param>
        /// <returns>converted integer value</returns>
        public static int GetInt(this string val, int defaultValue)
        {
            int result;
            return String.IsNullOrEmpty(val) ||
                   !int.TryParse(val, out result)
                       ? defaultValue
                       : result;
        }

        /// <summary>
        /// Convert to Long (Int64)
        /// </summary>
        /// <param name="val">Value to convert</param>
        /// <returns>Long</returns>
        public static long GetLong(this string val)
        {
            return GetLong(val, 0L);
        }

        /// <summary>
        /// Convert to Long
        /// </summary>
        /// <param name="val">Value to convert</param>
        /// <param name="defaultValue">Default value</param>
        /// <returns>Long</returns>
        public static long GetLong(this string val, long defaultValue)
        {
            long result;

            return String.IsNullOrEmpty(val) ||
                   !long.TryParse(val, out result)
                       ? defaultValue
                       : result;
        }

        /// <summary>
        /// Convert to ULong
        /// </summary>
        /// <param name="val">Value to convert</param>
        /// <returns>ULong</returns>
        public static ulong GetULong(this string val)
        {
            return GetULong(val, 0ul);
        }

        /// <summary>
        /// Convert to ULong
        /// </summary>
        /// <param name="val">Value to convert</param>
        /// <param name="defaultValue">Default value</param>
        /// <returns>ULong</returns>
        public static ulong GetULong(this string val, ulong defaultValue)
        {
            ulong result;

            return String.IsNullOrEmpty(val) ||
                   !ulong.TryParse(val, out result)
                       ? defaultValue
                       : result;
        }

        /// <summary>
        /// Convert to Double
        /// </summary>
        /// <param name="val">Value to convert</param>
        /// <returns>Double</returns>
        public static double GetDouble(this string val)
        {
            return GetDouble(val, 0d);
        }

        /// <summary>
        /// Convert to Double
        /// </summary>
        /// <param name="val">Value to convert</param>
        /// <param name="defaultValue">Default value</param>
        /// <returns>Double</returns>
        public static double GetDouble(this string val, double defaultValue)
        {
            double result;

            return String.IsNullOrEmpty(val) ||
                   !double.TryParse(val, NumberStyles.Any, CultureInfo.InvariantCulture, out result)
                       ? defaultValue
                       : result;
        }

        /// <summary>
        /// Convert to Decimal
        /// </summary>
        /// <param name="val">Value to convert</param>
        /// <returns>Decimal</returns>
        public static decimal GetDecimal(this string val)
        {
            return GetDecimal(val, 0);
        }

        /// <summary>
        /// Convert to Decimal
        /// </summary>
        /// <param name="val">Value to convert</param>
        /// <param name="defaultValue">Default value</param>
        /// <returns>Decimal</returns>
        public static decimal GetDecimal(this string val, decimal defaultValue)
        {
            decimal result;

            return String.IsNullOrEmpty(val) ||
                   !decimal.TryParse(val, NumberStyles.Any, CultureInfo.InvariantCulture, out result)
                       ? defaultValue
                       : result;
        }

        /// <summary>
        /// Convert to Date
        /// </summary>
        /// <param name="date">Value to convert</param>
        /// <returns>DateTime</returns>
        public static DateTime GetDate(this string date)
        {
            return GetDate(date, DateTime.MinValue);
        }

        /// <summary>
        /// Get DateFormates
        /// </summary>
        public static readonly string[] DateFormats =
            {
                "ddd MMM dd HH:mm:ss %zzzz yyyy",
                "yyyy-MM-dd\\THH:mm:ss\\Z",
                "yyyy-MM-dd HH:mm:ss",
                "yyyy-MM-dd HH:mm"
            };

        /// <summary>
        /// Convert to date
        /// </summary>
        /// <param name="date">Value to convert</param>
        /// <param name="defaultValue">Default value</param>
        /// <returns>DateTime</returns>
        public static DateTime GetDate(this string date, DateTime defaultValue)
        {
            DateTime result;

            return String.IsNullOrEmpty(date) ||
                   !DateTime.TryParseExact(date,
                                           DateFormats,
                                           CultureInfo.InvariantCulture,
                                           DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal, out result)
                       ? defaultValue
                       : result;
        }

        /// <summary>
        /// Fix the Json
        /// </summary>
        /// <param name="value">value</param>
        /// <returns>Fixed JSon</returns>
        public static string FixJson(this string value)
        {
            return value != null ? value.Replace("&amp;", "&") : null;
        }

        /// <summary>
        /// HTML character encoding according with http://www.w3.org/International/articles/definitions-characters/
        /// see also http://www.w3.org/International/questions/qa-escapes
        /// </summary>
        /// <param name="text">Value to encode</param>
        /// <returns>Encoded HTML</returns>
        public static String HtmlEncode(this string text)
        {
            if (text.HasNoValue())
            {
                return text;
            }

            // Start it off with a buffer of 10% on top of the passed string length, 
            // which should be more than enough for typical usage
            Int32 capacity = text.Length + (int)(text.Length * 0.1);
            StringBuilder result = new StringBuilder(capacity);
            // Characters are below the 127 value threshold: namely, &, <, >, and ". 
            // The call to the HttpUtility method covers these, and a few others.
            Char[] chars = WebUtility.HtmlDecode(text).ToCharArray();
            foreach (Char c in chars)
            {
                Int32 value = Convert.ToInt32(c);
                if (value > 127)
                {
                    result.AppendFormat("&#{0};", value);
                }
                else
                {
                    result.Append(c);
                }
            }

            return result.ToString();
        }

        /// <summary>
        /// Encode provided string with CData block (more then one if it contains unsafe input)
        /// </summary>
        /// <param name="source">Source string that needs to be wrapped</param>
        /// <param name="wrapOnlyIfNecessary">Indicates whether it is required to wrap a source string without special symbols</param>
        /// <returns>Source string wrapped with CData block</returns>
        public static String WrapWithCDataBlock(this String source, Boolean wrapOnlyIfNecessary = false)
        {
            if (String.IsNullOrEmpty(source))
            {
                return String.Empty;
            }
            if (wrapOnlyIfNecessary)
            {
                if (source.IndexOfAny(new char[] { '"', '&', '\'', '<', '>' }) == -1)
                {
                    return source;
                }
            }
            return "<![CDATA[" + source.Replace("]]>", "]]]]><![CDATA[>") + "]]>";
        }

        /// <summary>
        /// Returns a new string in which all occurrences of a specified string in the current instance are replaced with another specified string.
        /// </summary>
        /// <param name="orginalValue">Value</param>
        /// <param name="oldValue">The string to be replaced.</param>
        /// <param name="newValue">The string to replace all occurrences of oldValue.</param>
        /// <returns>Returns a new string in which all occurrences of a specified string in the current instance are replaced with another specified string.</returns>
        public static String ReplaceExt(this String orginalValue, String oldValue, String newValue)
        {
            if (!String.IsNullOrEmpty(oldValue))
            {
                orginalValue = orginalValue.Replace(oldValue, newValue);
            }

            return orginalValue;
        }

        /// <summary>
        /// Check is string contains executable javascript code
        /// </summary>
        /// <param name="orginalValue">Value for checking</param>
        /// <returns>Returns a new string in which all occurrences of a specified string in the current instance are replaced with another specified string.</returns>
        public static Boolean ContainsExecutableCode(this String orginalValue)
        {
            Regex regex = new Regex(ExecutableStringPattern);

            return regex.IsMatch(orginalValue.ToLower());
        }

        /// <summary>
        /// Removes the executable javascript code from string.
        /// </summary>
        /// <param name="originalValue">The original value.</param>
        /// <returns>String with all executable code removed</returns>
        public static String RemoveExecutableCode(this String originalValue)
        {
            Regex regex = new Regex(ExecutableStringPattern);

            return regex.Replace(originalValue, "");
            
        }

        /// <summary>
        /// To the name of the js compliant property.
        /// </summary>
        /// <param name="value">The name.</param>
        /// <param name="replacement"></param>
        /// <returns></returns>
        public static String ToJsCompliant(this String value, String replacement = "")
        {
            Regex pattern = new Regex("[ ().]");
            StringBuilder replaced = new StringBuilder(pattern.Replace(value, replacement));

            if (replaced.Length > 0)
            {
                replaced[0] = Char.ToLower(replaced[0]);
            }

            return replaced.ToString();
        }
    }
}
