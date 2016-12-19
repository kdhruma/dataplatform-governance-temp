using System;
using System.ComponentModel;
using System.Reflection;
using System.Text;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;

namespace MDM.Core
{
    /// <summary>
    /// Specifies ValueTypeHelper which provides functionalities related to object value
    /// </summary>
    public class ValueTypeHelper
    {
        #region Fields

        #endregion

        #region Constructors

        #endregion

        #region Properties

        #endregion

        #region Helper methods for array

        /// <summary>
        /// Joins an array of integers as string with the delimiter given.
        /// </summary>
        /// <param name="array">Array of integer to be joined</param>
        /// <param name="delimiter">Delimiter which will be used to join the array elements.</param>
        /// <returns>String with array elements joined together with the given delimiter</returns>
        public static String JoinArray(Int32[] array, String delimiter)
        {
            String output = String.Empty;

            if (array == null)
                throw new ArgumentNullException("array", "Value array cannot be null or empty");

            if (String.IsNullOrEmpty(delimiter))
                throw new ArgumentNullException("delimiter", "Separator cannot be null or empty");

            StringBuilder stringBuilder = new StringBuilder();
            Int32 itemCount = 0;
            Int32 totalCount = array.Length;

            foreach (Int32 item in array)
            {
                stringBuilder.Append(item.ToString());

                if (itemCount < totalCount - 1)
                    stringBuilder.Append(delimiter);

                itemCount++;
            }

            output = stringBuilder.ToString();

            return output;
        }

        /// <summary>
        /// Joins an array of strings as string with the delimiter given.
        /// </summary>
        /// <param name="array">Array of string to be joined</param>
        /// <param name="delimiter">Delimiter which will be used to join the array elements.</param>
        /// <returns>String with array elements joined together with the given delimiter</returns>
        public static String JoinArray(String[] array, String delimiter)
        {
            String output = String.Empty;

            if (array == null)
                throw new ArgumentNullException("array", "Value array cannot be null or empty");

            if (String.IsNullOrEmpty(delimiter))
                throw new ArgumentNullException("delimiter", "Separator cannot be null or empty");

            StringBuilder stringBuilder = new StringBuilder();
            Int32 itemCount = 0;
            Int32 totalCount = array.Length;

            foreach (String item in array)
            {
                stringBuilder.Append(item);

                if (itemCount < totalCount - 1)
                    stringBuilder.Append(delimiter);

                itemCount++;
            }

            output = stringBuilder.ToString();

            return output;
        }

        /// <summary>
        /// Splits the input string and generates a 32 bit integer array. 
        /// The string equivalent of integer elements has to be seperated by given delimiter.
        /// </summary>
        /// <param name="data">Input string containing integers seperated by delimiter.</param>
        /// <param name="delimiter">Delimiter by which integers are seperated in the input string.</param>
        /// <returns>An 32 bit integer array.</returns>
        public static Int32[] SplitStringToIntArray(String data, Char delimiter)
        {
            Int32[] output = null;
            if (!String.IsNullOrEmpty(data))
            {
                String[] strOutput = data.Split(delimiter);
                output = Array.ConvertAll<String, Int32>(strOutput,
                    delegate(String item)
                    {
                        Int32 result;
                        Int32.TryParse(item, out result);
                        return result;
                    });
            }
            return output;
        }

        /// <summary>
        /// Splits the input string and generates a 64 bit integer array. 
        /// The string equivalent of integer elements has to be seperated by given delimiter.
        /// </summary>
        /// <param name="data">Input string containing integers seperated by delimiter.</param>
        /// <param name="delimiter">Delimiter by which integers are seperated in the input string.</param>
        /// <returns>An 64 bit integer array.</returns>
        public static Int64[] SplitStringToLongArray(String data, Char delimiter)
        {
            Int64[] output = null;
            if (!String.IsNullOrEmpty(data))
            {
                String[] strOutput = data.Split(delimiter);
                output = Array.ConvertAll<String, Int64>(strOutput,
                    delegate(String item)
                    {
                        Int64 result;
                        Int64.TryParse(item, out result);
                        return result;
                    });
            }
            return output;
        }

        /// <summary>
        /// Splits the input string and generates a String array. 
        /// </summary>
        /// <param name="data">Input string containing string separated by delimiter.</param>
        /// <param name="delimiter">Delimiter by which string are separated in the input string.</param>
        /// <returns>String array.</returns>
        public static String[] SplitStringToStringArray(String data, Char delimiter)
        {
            String[] output = null;
            if (!String.IsNullOrEmpty(data) && delimiter > 0)
            {
                output = data.Split(delimiter);
            }
            return output;
        }
        #endregion Helper methods for array

        #region Helper methods for Collection

        /// <summary>
        /// Joins a collection of LocaleEnum as LocaleId as string with the delimiter given.
        /// </summary>
        /// <param name="localeCollection">Collection of LocaleEnum to be joined</param>
        /// <param name="delimiter">Delimiter which will be used to join the array elements.</param>
        /// <returns>String with collection items joined together with the given delimiter</returns>
        public static String JoinCollectionGetLocaleIdList(Collection<LocaleEnum> localeCollection, String delimiter)
        {
            String output = String.Empty;

            #region Parameter validation

            if (localeCollection == null)
                throw new ArgumentNullException("localeCollection", "Value array cannot be null or empty");

            if (String.IsNullOrEmpty(delimiter))
                throw new ArgumentNullException("delimiter", "Delimiter cannot be null or empty");

            #endregion Parameter validation

            StringBuilder stringBuilder = new StringBuilder();

            foreach (LocaleEnum locale in localeCollection)
            {
                if (stringBuilder.Length > 0)
                    stringBuilder.Append(delimiter);

                stringBuilder.Append((Int32)locale);
            }

            output = stringBuilder.ToString();

            return output;
        }

        /// <summary>
        /// Join the Collection item with given delimiter
        /// </summary>
        /// <param name="items">Collection of T items to be joined</param>
        /// <param name="delimiter">Delimiter which will be used to join the array elements.</param>
        /// <returns>String with collection items joined together with the given delimiter</returns>
        public static String JoinCollection<T>(Collection<T> items, String delimiter)
        {
            #region Parameter validation

            if (items == null)
                throw new ArgumentNullException("items", "items array cannot be null or empty");

            if (String.IsNullOrEmpty(delimiter))
                throw new ArgumentNullException("delimiter", "Delimiter cannot be null or empty");

            #endregion Parameter validation

            // Optimized way of Generating csv from Collection
            return String.Join<T>(delimiter, items);
        }

        /// <summary>
        /// Joins a collection of Column Names as string with the delimiter given with escaping.
        /// <para>
        /// This method wraps each column into '[]' before joining to solve the DB syntax issues.
        /// </para>
        /// </summary>
        /// <param name="columnNameCollection">Collection of Column Names</param>
        /// <param name="delimiter">Delimiter which will be used to join the array elements.</param>
        /// <returns>String with collection items wrapped in '[]' and joined together with the given delimiter</returns>
        public static String JoinColumnCollectionWithEscaping(Collection<String> columnNameCollection, String delimiter)
        {
            String output = String.Empty;

            #region Parameter validation

            if (columnNameCollection == null)
                throw new ArgumentNullException("columnNameCollection", "Column Names collection cannot be null or empty");

            if (String.IsNullOrEmpty(delimiter))
                throw new ArgumentNullException("delimiter", "Delimiter cannot be null or empty");

            #endregion Parameter validation

            StringBuilder stringBuilder = new StringBuilder();

            foreach (String columnName in columnNameCollection)
            {
                if (stringBuilder.Length > 0)
                    stringBuilder.Append(delimiter);

                stringBuilder.Append("[");
                stringBuilder.Append(columnName);
                stringBuilder.Append("]");
            }

            output = stringBuilder.ToString();

            return output;
        }

        /// <summary>
        /// Converts object collection to object Array
        /// </summary>
        /// <param name="objCollection">Indicates collection of objects to be converted to array.</param>
        /// <returns>Returns array of objects.</returns>
        public static Object[] ConvertObjectCollectionToArray(Collection<Object> objCollection)
        {
            Object[] parameters = new Object[objCollection.Count];
            objCollection.CopyTo(parameters, 0);
            return parameters;
        }

        /// <summary>
        /// Splits the input string and generates a 32 bit integer array. 
        /// The string equivalent of integer elements has to be seperated by given delimiter.
        /// </summary>
        /// <param name="data">Input string containing integers seperated by delimiter.</param>
        /// <param name="delimiter">Delimiter by which integers are seperated in the input string.</param>
        /// <returns>An 32 bit integer array.</returns>
        public static Collection<Int32> SplitStringToIntCollection(String data, Char delimiter)
        {
            Collection<Int32> output = null;
            if (!String.IsNullOrEmpty(data))
            {
                output = new Collection<Int32>();
                String[] strOutput = data.Split(delimiter);
                foreach (String s in strOutput)
                {
                    if (String.IsNullOrWhiteSpace(s))
                        continue;
                    Int32 result;
                    Int32.TryParse(s, out result);
                    output.Add(result);
                }
            }
            return output;
        }

        /// <summary>
        /// Splits the input string and generates a 64 bit integer array. 
        /// The string equivalent of Long elements has to be separated by given delimiter.
        /// </summary>
        /// <param name="data">Input string containing integers separated by delimiter.</param>
        /// <param name="delimiter">Delimiter by which long values are separated in the input string.</param>
        /// <returns>An 64 bit integer array.</returns>
        public static Collection<Int64> SplitStringToLongCollection(String data, Char delimiter)
        {
            Collection<Int64> output = null;
            if (!String.IsNullOrEmpty(data))
            {
                output = new Collection<Int64>();
                String[] strOutput = data.Split(delimiter);
                foreach (String s in strOutput)
                {
                    if (String.IsNullOrWhiteSpace(s))
                        continue;
                    Int64 result;
                    Int64.TryParse(s, out result);
                    output.Add(result);
                }
            }
            return output;
        }

        /// <summary>
        /// Splits the input string and generates a Guid array. 
        /// The string equivalent of Long elements has to be separated by given delimiter.
        /// </summary>
        /// <param name="data">Input string containing Guids separated by delimiter.</param>
        /// <param name="delimiter">Delimiter by which Guid values are separated in the input string.</param>
        /// <returns>An Guid array.</returns>
        public static Collection<Guid> SplitStringToGuidCollection(String data, Char delimiter)
        {
            Collection<Guid> output = null;
            if (!String.IsNullOrEmpty(data))
            {
                output = new Collection<Guid>();
                String[] strOutput = data.Split(delimiter);
                foreach (String s in strOutput)
                {
                    if (String.IsNullOrWhiteSpace(s))
                    {
                        continue;
                    }
                    Guid result;
                    if (Guid.TryParse(s, out result))
                    {
                        output.Add(result);
                    }
                }
            }
            return output;
        }

        /// <summary>
        /// Splits the input string and generates a LocaleEnum collection. 
        /// The string equivalent of Long elements has to be separated by given delimiter.
        /// </summary>
        /// <param name="data">Input string containing LocaleEnum value separated by delimiter.</param>
        /// <param name="delimiter">Delimiter by which LocaleEnum values are separated in the input string.</param>
        /// <returns>An LocaleEnum collection.</returns>
        public static Collection<LocaleEnum> SplitStringToLocaleEnumCollection(String data, Char delimiter)
        {
            Collection<LocaleEnum> output = null;
            if (!String.IsNullOrEmpty(data))
            {
                output = new Collection<LocaleEnum>();
                String[] strOutput = data.Split(delimiter);
                foreach (String s in strOutput)
                {
                    LocaleEnum result = LocaleEnum.UnKnown;
                    Enum.TryParse(s, out result);
                    output.Add(result);
                }
            }
            return output;
        }

        /// <summary>
        /// Splits the input string and generates a MDMTraceSource collection. 
        /// The string equivalent of Long elements has to be separated by given delimiter.
        /// </summary>
        /// <param name="data">Input string containing MDMTraceSource value separated by delimiter.</param>
        /// <param name="delimiter">Delimiter by which MDMTraceSource values are separated in the input string.</param>
        /// <returns>An MDMTraceSource collection.</returns>
        public static Collection<MDMTraceSource> SplitStringToMDMTraceSourcesEnumCollection(String data, Char delimiter)
        {
            Collection<MDMTraceSource> output = null;
            if (!String.IsNullOrEmpty(data))
            {
                output = new Collection<MDMTraceSource>();
                String[] strOutput = data.Split(delimiter);
                foreach (String s in strOutput)
                {
                    MDMTraceSource result = MDMTraceSource.UnKnown;
                    Enum.TryParse(s, out result);
                    output.Add(result);
                }
            }
            return output;
        }

        /// <summary>
        /// Splits the input string and generates a specified collection. 
        /// The string equivalent of Long elements has to be separated by given delimiter.
        /// </summary>
        /// <param name="data">Input string containing Enum value separated by delimiter.</param>
        /// <param name="delimiter">Delimiter by which Enum values are separated in the input string.</param>
        /// <returns>Enum collection.</returns>
        public static Collection<TEnum> SplitStringToEnumCollection<TEnum>(String data, Char delimiter) where TEnum : struct
        {
            if (!typeof(TEnum).IsEnum)
                throw new ArgumentException("TEnum must be an enumerated type");

            Collection<TEnum> output = null;
            if (!String.IsNullOrEmpty(data))
            {
                output = new Collection<TEnum>();
                String[] strOutput = data.Split(delimiter);
                foreach (String s in strOutput)
                {
                    TEnum result = default(TEnum);
                    Enum.TryParse(s, out result);
                    output.Add(result);
                }
            }
            return output;
        }

        /// <summary>
        /// Splits the input string and generates a specified collection. 
        /// The string equivalent of Long elements has to be separated by given delimiter.
        /// </summary>
        /// <param name="data">Input string containing Enum value separated by delimiter.</param>
        /// <returns>Enum collection.</returns>
        public static Collection<TEnum> StringCollectionToEnumCollection<TEnum>(Collection<String> data) where TEnum : struct
        {
            if (!typeof(TEnum).IsEnum)
                throw new ArgumentException("TEnum must be an enumerated type");

            Collection<TEnum> output = null;
            if (data != null && data.Count > 0)
            {
                output = new Collection<TEnum>();
                foreach (String s in data)
                {
                    TEnum result = default(TEnum);
                    Enum.TryParse(s, out result);
                    output.Add(result);
                }
            }
            return output;
        }

        /// <summary>
        /// Splits the input string and generates a LocaleEnum collection. 
        /// The string equivalent of Long elements has to be separated by given delimiter.
        /// </summary>
        /// <param name="data">Input string containing LocaleEnum value separated by delimiter.</param>
        /// <param name="delimiter">Delimiter by which LocaleEnum values are separated in the input string.</param>
        /// <returns>An 64 bit integer array.</returns>
        public static Collection<LocaleEnum> SplitLocaleIdStringToLocaleEnumCollection(String data, Char delimiter)
        {
            Collection<LocaleEnum> output = null;
            if (!String.IsNullOrEmpty(data))
            {
                output = new Collection<LocaleEnum>();
                String[] strOutput = data.Split(delimiter);
                foreach (String s in strOutput)
                {
                    Int32 localeId = ValueTypeHelper.Int32TryParse(s, 0);
                    LocaleEnum result = (LocaleEnum)localeId;
                    output.Add(result);
                }
            }
            return output;
        }

        /// <summary>
        /// Splits the input string and generates a UserAction collection. 
        /// The string equivalent of UserAction elements has to be separated by given delimiter.
        /// </summary>
        /// <param name="data">Input string containing UserAction value separated by delimiter.</param>
        /// <param name="delimiter">Delimiter by which UserAction values are separated in the input string.</param>
        /// <returns>Collection of UserAction</returns>
        public static Collection<UserAction> SplitStringToUserActionCollection(String data, Char delimiter)
        {
            Collection<UserAction> output = null;

            if (!String.IsNullOrWhiteSpace(data))
            {
                output = new Collection<UserAction>();
                String[] strOutput = data.Split(delimiter);
                foreach (String s in strOutput)
                {
                    if (String.Compare(s, "noaccess", StringComparison.InvariantCultureIgnoreCase) == 0)
                    {
                        output.Add(UserAction.None);
                    }
                    else
                    {
                        UserAction result = UserAction.Unknown;
                        Enum.TryParse(s, out result);
                        output.Add(result);
                    }
                }
            }
            return output;
        }

        /// <summary>
        /// Splits the input string and generates a String collection. 
        /// </summary>
        /// <param name="data">Input string containing string separated by delimiter.</param>
        /// <param name="delimiter">Delimiter by which string are separated in the input string.</param>
        /// <returns>String array.</returns>
        public static Collection<String> SplitStringToStringCollection(String data, Char delimiter)
        {
            Collection<String> output = null;

            if (!String.IsNullOrWhiteSpace(data) && delimiter > 0)
            {
                output = new Collection<String>();

                String[] strOutput = data.Split(delimiter);
                foreach (String s in strOutput)
                {
                    output.Add(s);
                }
            }

            return output;
        }

        /// <summary>
        /// Splits the input string and generates a String collection. 
        /// </summary>
        /// <param name="data">Input string containing string separated by delimiter.</param>
        /// <param name="delimiter">Delimiter by which string are separated in the input string.</param>
        /// <param name="splitOption">Type of string splitting options</param>
        /// <returns>String collection.</returns>
        public static Collection<String> SplitStringToStringCollection(String data, String delimiter, StringSplitOptions splitOption = StringSplitOptions.None)
        {
            Collection<String> output = null;

            if (!(String.IsNullOrWhiteSpace(data) || String.IsNullOrWhiteSpace(delimiter)))
            {
                output = new Collection<String>();

                String[] strOutput = data.Split(new String[] { delimiter }, splitOption);
                foreach (String s in strOutput)
                {
                    output.Add(s);
                }
            }

            return output;
        }

        /// <summary>
        /// Splits the string to generic collection.
        /// </summary>
        /// <typeparam name="T">Generic T type</typeparam>
        /// <param name="data">Input string containing string separated by delimiter.</param>
        /// <param name="delimiter">Delimiter by which string are separated in the input string.</param>
        /// <param name="splitOption">Type of string splitting options</param>
        /// <returns>Generic T type of collection </returns>
        public static Collection<T> SplitStringToGenericCollection<T>(String data, String delimiter, StringSplitOptions splitOption = StringSplitOptions.None)
        {
            Collection<T> output = null;

            if (!(String.IsNullOrWhiteSpace(data) || String.IsNullOrWhiteSpace(delimiter)))
            {
                output = new Collection<T>();

                String[] strOutput = data.Split(new String[] { delimiter }, splitOption);
                foreach (String s in strOutput)
                {
                    output.Add((T)Convert.ChangeType(s, typeof(T)));
                }
            }

            return output;
        }


        /// <summary>
        /// Splits the input string and generates a String Array. 
        /// </summary>
        /// <param name="data">Input string containing string separated by delimiter.</param>
        /// <param name="separator">Delimiter by which string are separated in the input string.</param>
        /// <param name="splitOption">Type of string spliting option</param>
        /// <returns>String array.</returns>
        public static String[] SplitStringToStringArray(String data, String separator, StringSplitOptions splitOption = StringSplitOptions.None)
        {
            String[] output = null;

            if (!(String.IsNullOrWhiteSpace(data) || String.IsNullOrWhiteSpace(separator)))
            {
                output = data.Split(new String[] { separator }, splitOption);
            }

            return output;
        }

        /// <summary>
        /// Merge two collections
        /// </summary>
        /// <param name="collection1">1st Collection</param>
        /// <param name="collection2">2nd Collection</param>
        /// <returns>Merged collection</returns>
        public static Collection<Int32> MergeCollections(Collection<Int32> collection1, Collection<Int32> collection2)
        {
            if (collection1 == null)
            {
                throw new ArgumentNullException("collection1");
            }

            if (collection2 == null)
            {
                throw new ArgumentNullException("collection2");
            }

            Collection<Int32> mergedCollection = new Collection<Int32>(collection1);

            foreach (Int32 item in collection2)
            {
                if (!mergedCollection.Contains<Int32>(item))
                {
                    mergedCollection.Add(item);
                }
            }

            return mergedCollection;
        }

        /// <summary>
        /// Merge two collections
        /// </summary>
        /// <param name="collection1">1st Collection</param>
        /// <param name="collection2">2nd Collection</param>
        /// <returns>Merged collection</returns>
        public static Collection<Int64> MergeCollections(Collection<Int64> collection1, Collection<Int64> collection2)
        {
            if (collection1 == null)
            {
                throw new ArgumentNullException("collection1");
            }

            if (collection2 == null)
            {
                throw new ArgumentNullException("collection2");
            }

            Collection<Int64> mergedCollection = new Collection<Int64>(collection1);
            foreach (Int64 item in collection2)
            {
                if (!mergedCollection.Contains<Int64>(item))
                {
                    mergedCollection.Add(item);
                }
            }

            return mergedCollection;
        }

        /// <summary>
        /// Merge two collections
        /// </summary>
        /// <param name="collection1">1st Collection</param>
        /// <param name="collection2">2nd Collection</param>
        /// <param name="stringComparer">String comparion options</param>
        /// <returns>Merged collection</returns>
        public static Collection<String> MergeCollections(Collection<String> collection1, Collection<String> collection2, StringComparer stringComparer = null)
        {
            if (collection1 == null)
            {
                throw new ArgumentNullException("collection1");
            }

            if (collection2 == null)
            {
                throw new ArgumentNullException("collection2");
            }

            if (stringComparer == null)
            {
                stringComparer = StringComparer.InvariantCultureIgnoreCase;
            }

            var mergedCollection = new Collection<String>(collection1);

            foreach (String item in collection2)
            {
                if (!mergedCollection.Contains(item, stringComparer))
                {
                    mergedCollection.Add(item);
                }
            }

            return mergedCollection;
        }

        /// <summary>
        /// Merge two collections
        /// </summary>
        /// <param name="collection1">1st Collection</param>
        /// <param name="collection2">2nd Collection</param>
        /// <returns>Merged collection</returns>
        public static Collection<LocaleEnum> MergeCollections(Collection<LocaleEnum> collection1, Collection<LocaleEnum> collection2)
        {
            if (collection1 == null)
            {
                throw new ArgumentNullException("collection1");
            }

            if (collection2 == null)
            {
                throw new ArgumentNullException("collection2");
            }

            Collection<LocaleEnum> mergedCollection = new Collection<LocaleEnum>(collection1);

            foreach (LocaleEnum item in collection2)
            {
                if (!mergedCollection.Contains<LocaleEnum>(item))
                {
                    mergedCollection.Add(item);
                }
            }

            return mergedCollection;
        }

        /// <summary>
        /// Removes all the items from the collection if the given item present in the collection
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="collection">Collection of given Type</param>
        /// <param name="item">item to be removed </param>
        public static void RemoveAll<T>(Collection<T> collection, T item)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }
            Int32 count = collection.Count;
            for (Int32 i = 0; i < count; ++i)
            {
                if (collection.Contains(item))
                    collection.Remove(item);
            }
        }

        /// <summary>
        /// Clones given Collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static Collection<T> CloneCollection<T>(Collection<T> collection)
        {
            Collection<T> clonedCollection = null;

            if (collection != null)
            {
                clonedCollection = new Collection<T>();

                foreach (var item in collection)
                {
                    clonedCollection.Add(item);
                }
            }

            return clonedCollection;
        }

        #endregion Helper methods for Collection

        #region Helper methods for type conversion

        /// <summary>
        /// Converts a string to double equivalent.
        /// </summary>
        /// <param name="data">input string.</param>
        /// <param name="defaultValue">Default value of the output if parsing is failed.</param>
        /// <returns>Equivalent double if parsing is successful otherwise the default value given.</returns>
        public static Double DoubleTryParse(String data, Double defaultValue)
        {
            Double output = 0;
            //if TryParse fails, it assigns 0 to the out parameter, we don't need 0, we need the defaultValue
            if (Double.TryParse(data, out output))
                return output;
            else
                return defaultValue;
        }

        /// <summary>
        /// Converts a string to decimal equivalent.
        /// </summary>
        /// <param name="data">Specifies the input string.</param>
        /// <param name="defaultValue">Specifies the default value of the output if parsing is failed.</param>
        /// <param name="styles">Specifies the type of number style</param>
        /// <param name="cultureInfo">Specifies culture information</param>
        /// <returns>Equivalent decimal if parsing is successful otherwise the default value given.</returns>
        public static Boolean FractionTryParse(String data, out Decimal defaultValue, NumberStyles styles, CultureInfo cultureInfo)
        {
            Boolean isDecimal = false;
            defaultValue = 0;

            if (!String.IsNullOrWhiteSpace(data))
            {
                data = data.Trim();
                if (Decimal.TryParse(data, NumberStyles.AllowDecimalPoint | (styles & NumberStyles.AllowLeadingSign), cultureInfo, out defaultValue))
                {
                    //Able to parse.
                    //Incoming value is a whole number
                    isDecimal = true;
                    return isDecimal;
                }

                //Incoming value is a fractional number
                //Convert fraction to decimal
                Decimal wholeNumber = default(Decimal);
                Decimal numerator = 0;
                Decimal denominator = 0;

                String stringWholeNumber = String.Empty;
                String stringFractionNumber = data;

                //Check whether it is Mixed fraction
                if (data.Contains(" "))
                {
                    Int32 spaceIndex = data.IndexOf(' ');
                    stringWholeNumber = data.Substring(0, spaceIndex);
                    stringFractionNumber = data.Substring(spaceIndex + 1).Trim();

                    if (!Decimal.TryParse(stringWholeNumber, styles, cultureInfo, out wholeNumber))
                    {
                        return isDecimal;
                    }
                }

                //Get Numerator and denominator
                if (stringFractionNumber.Contains("/"))
                {
                    String[] partsOfFraction = stringFractionNumber.Split('/');
                    if (partsOfFraction != null)
                    {
                        if (partsOfFraction[0] != null && partsOfFraction[1] != null)
                        {
                            if (Decimal.TryParse(partsOfFraction[0], styles, cultureInfo, out numerator) && Decimal.TryParse(partsOfFraction[1], styles, cultureInfo, out denominator))
                                isDecimal = true;
                        }
                    }
                }

                if (denominator > 0)
                {
                    defaultValue = numerator / denominator;
                    defaultValue = wholeNumber + defaultValue;
                }
                else
                {
                    isDecimal = false;
                }
            }
            // trimming the value of decimal to max 4 digits after decimal 
            defaultValue = Math.Round(defaultValue, 4);
            return isDecimal;
        }

        /// <summary>
        /// Converts a string to 64 bit signed integer equivalent.
        /// </summary>
        /// <param name="data">input string.</param>
        /// <param name="defaultValue">Default value of the output if parsing is failed.</param>
        /// <returns>Equivalent 64 bit signed integer if parsing is successful otherwise the default value given.</returns>
        public static Int64 Int64TryParse(String data, Int64 defaultValue)
        {
            Int64 output = defaultValue;
            //if TryParse fails, it assigns 0 to the out parameter, we don't need 0, we need the defaultValue
            if (Int64.TryParse(data, out output))
                return output;
            else
                return defaultValue;
        }

        /// <summary>
        /// Converts a string to 32 bit signed integer equivalent.
        /// </summary>
        /// <param name="data">input string.</param>
        /// <param name="defaultValue">Default value of the output if parsing is failed.</param>
        /// <returns>Equivalent 32 bit signed integer if parsing is successful otherwise the default value given.</returns>
        public static Int32 Int32TryParse(String data, Int32 defaultValue)
        {
            Int32 output = defaultValue;
            //if Int32.TryParse fails, it assigns 0 to the out parameter, we don't need 0, we need the defaultValue
            if (Int32.TryParse(data, out output))
                return output;
            else
                return defaultValue;
        }

        /// <summary>
        /// Converts a string to decimal equivalent.
        /// </summary>
        /// <param name="data">input string.</param>
        /// <param name="defaultValue">Default value of the output if parsing is failed.</param>
        /// <returns>Equivalent decimal if parsing is successful otherwise the default value given.</returns>
        public static Decimal DecimalTryParse(String data, Decimal defaultValue)
        {
            Decimal output = defaultValue;
            //if Decimal.TryParse fails, it assigns 0 to the out parameter, we don't need 0, we need the defaultValue
            if (Decimal.TryParse(data, out output))
                return output;
            else
                return defaultValue;
        }

        /// <summary>
        /// Converts a string to decimal equivalent.
        /// </summary>
        /// <param name="data">input string.</param>
        /// <param name="defaultValue">Default value of the output if parsing is failed.</param>
        /// <param name="locale">locale.</param>
        /// <returns>Equivalent decimal if parsing is successful otherwise the default value given.</returns>
        public static Decimal DecimalTryParse(String data, Decimal defaultValue, LocaleEnum locale)
        {
            if (locale == LocaleEnum.UnKnown)
            {
                return DecimalTryParse(data, defaultValue);
            }
            else
            {
                IFormatProvider culture = CultureInfo.CreateSpecificCulture(locale.GetCultureName());
                Decimal output = defaultValue;

                if (Decimal.TryParse(data, NumberStyles.Any, culture, out output))
                    return output;
                else
                    return defaultValue;
            }
        }

        /// <summary>
        /// Converts a string to DateTime equivalent.
        /// </summary>
        /// <param name="data">input string.</param>
        /// <param name="defaultValue">Default value of the output if parsing is failed.</param>
        /// <param name="format"></param>
        /// <param name="dateTimeStyles"></param>
        /// <returns>Equivalent DateTime if parsing is successful otherwise the default value given.
        ///          If null, empty or whitespace is passed for data, default value will be returned.</returns>
        public static DateTime DateTimeTryParse(String data, DateTime defaultValue, IFormatProvider format = null, DateTimeStyles dateTimeStyles = DateTimeStyles.AssumeLocal)
        {
            DateTime output = defaultValue;

            if (!String.IsNullOrWhiteSpace(data))
            {
                if (DateTime.TryParse(data, format, dateTimeStyles, out output))
                {
                    return output;
                }
            }

            return defaultValue;
        }

        /// <summary>
        /// Converts a string to DateTime equivalent.
        /// </summary>
        /// <param name="data">input string.</param>
        /// <param name="defaultValue">Default value of the output if parsing is failed.</param>
        /// <param name="locale">locale.</param>
        /// <returns>Equivalent DateTime if parsing is successful otherwise the default value given.</returns>
        public static DateTime DateTimeTryParse(String data, DateTime defaultValue, LocaleEnum locale)
        {
            if (locale == LocaleEnum.UnKnown)
            {
                return DateTimeTryParse(data, defaultValue);
            }
            else
            {
                IFormatProvider culture = CultureInfo.CreateSpecificCulture(locale.GetCultureName());
                DateTime output = defaultValue;

                if (DateTime.TryParse(data, culture, DateTimeStyles.None, out output))
                    return output;
                else
                    return defaultValue;
            }
        }

        /// <summary>
        /// Converts a string to Byte equivalent.
        /// </summary>
        /// <param name="data">input string.</param>
        /// <param name="defaultValue">Default value of the output if parsing is failed.</param>
        /// <returns>Equivalent 8 bit unsigned integer if parsing is successful otherwise the default value given.</returns>
        public static Byte ByteTryParse(String data, Byte defaultValue)
        {
            Byte output = defaultValue;
            Byte.TryParse(data, out output);
            return output;
        }

        /// <summary>
        /// Converts a string to 16 bit signed integer equivalent.
        /// </summary>
        /// <param name="data">input string.</param>
        /// <param name="defaultValue">Default value of the output if parsing is failed.</param>
        /// <returns>Equivalent 16 bit signed integer if parsing is successful otherwise the default value given.</returns>
        public static Int16 Int16TryParse(String data, Int16 defaultValue)
        {
            Int16 output = defaultValue;
            Int16.TryParse(data, out output);
            return output;
        }

        /// <summary>
        /// Convert string value into Boolean
        /// </summary>
        /// <param name="stringValue">Value to be converted into Boolean</param>
        /// <param name="defaultValue">Default value of the output if parsing is failed.</param>
        /// <returns>Boolean equivalent of stringValue. If conversion fails, then returns false</returns>
        /// <exception cref="ArgumentException">stringValue is null or empty. Cannot convert to Boolean</exception>
        public static Boolean BooleanTryParse(String stringValue, Boolean defaultValue)
        {
            Boolean returnValue = defaultValue;

            if (!String.IsNullOrWhiteSpace(stringValue))
            {
                if (!Boolean.TryParse(stringValue, out returnValue))
                {
                    //Values provided were not True / False, but Yes / No / Y / N
                    switch (stringValue.ToLowerInvariant())
                    {
                        case "y":
                        case "yes":
                        case "1":
                            {
                                returnValue = true;
                                break;
                            }
                        case "n":
                        case "no":
                        case "0":
                            {
                                returnValue = false;
                                break;
                            }
                    }
                }
            }

            return returnValue;
        }

        /// <summary>
        /// Convert string value into Boolean
        /// </summary>
        /// <param name="stringValue">Value to be converted into Boolean</param>
        /// <returns>Boolean equivalent of stringValue. If conversion fails, then returns false</returns>
        /// <exception cref="ArgumentException">stringValue is null or empty. Cannot convert to Boolean</exception>
        public static Boolean ConvertToBoolean(String stringValue)
        {
            if (String.IsNullOrWhiteSpace(stringValue))
            {
                throw new ArgumentException("stringValue is null or empty. Cannot convert to Boolean");
            }

            Boolean returnValue = false;

            if (!String.IsNullOrWhiteSpace(stringValue))
            {
                Boolean.TryParse(stringValue, out returnValue);
            }

            return returnValue;
        }

        /// <summary>
        /// Convert object value into Decimal
        /// </summary>
        /// <param name="objectValue">Value to be converted into Decimal</param>
        /// <returns>Decimal equivalent of objectValue. If conversion fails, then returns 0</returns>
        /// <exception cref="ArgumentNullException">objectValue is null. Cannot convert to Decimal</exception>
        public static Decimal ConvertToDecimal(Object objectValue)
        {
            if (objectValue == null)
            {
                throw new ArgumentNullException("objectValue", "Cannot convert to Decimal");
            }

            return ValueTypeHelper.ConvertToDecimal(objectValue.ToString());
        }

        /// <summary>
        /// Convert string value into Decimal
        /// </summary>
        /// <param name="stringValue">Value to be converted into Decimal</param>
        /// <returns>Decimal equivalent of stringValue. If conversion fails, then returns 0</returns>
        /// <exception cref="ArgumentNullException">stringValue is null. Cannot convert to Decimal</exception>
        public static Decimal ConvertToDecimal(String stringValue)
        {
            if (String.IsNullOrWhiteSpace(stringValue))
            {
                throw new ArgumentException("stringValue is null or empty. Cannot convert to Decimal");
            }

            Decimal decimalVal = 0;
            Decimal.TryParse(stringValue, out decimalVal);

            return decimalVal;
        }
        /// <summary>
        /// Convert object value into int
        /// </summary>
        /// <param name="objectValue">Value to be converted into Int</param>
        /// <returns>Int32 equivalent of objectValue. If conversion fails, then returns 0</returns>
        /// <exception cref="ArgumentNullException">objectValue is null. Cannot convert to Decimal</exception>
        public static Int32 ConvertToInt32(Object objectValue)
        {
            if (objectValue == null)
            {
                throw new ArgumentNullException("objectValue", "Cannot convert to Decimal");
            }

            return ValueTypeHelper.ConvertToInt32(objectValue.ToString());
        }
        /// <summary>
        /// Convert string value into int
        /// </summary>
        /// <param name="stringValue">Value to be converted into int</param>
        /// <returns>Int32 equivalent of stringValue. If conversion fails, then returns 0</returns>
        /// <exception cref="ArgumentNullException">stringValue is null. Cannot convert to int</exception>
        public static Int32 ConvertToInt32(String stringValue)
        {
            if (String.IsNullOrWhiteSpace(stringValue))
            {
                throw new ArgumentException("stringValue is null or empty. Cannot convert to Int");
            }

            Int32 intVal = 0;
            Int32.TryParse(stringValue, out intVal);

            return intVal;
        }

        /// <summary>
        /// Convert object value into int
        /// </summary>
        /// <param name="objectValue">Value to be converted into Int</param>
        /// <returns>Int64 equivalent of objectValue. If conversion fails, then returns 0</returns>
        /// <exception cref="ArgumentNullException">objectValue is null. Cannot convert to Decimal</exception>
        public static Int64 ConvertToInt64(Object objectValue)
        {
            if (objectValue == null)
            {
                throw new ArgumentNullException("objectValue", "Cannot convert to Decimal");
            }

            return ValueTypeHelper.ConvertToInt64(objectValue.ToString());
        }
        /// <summary>
        /// Convert string value into int64
        /// </summary>
        /// <param name="stringValue">Value to be converted into int</param>
        /// <returns>Int64 equivalent of stringValue. If conversion fails, then returns 0</returns>
        /// <exception cref="ArgumentNullException">stringValue is null. Cannot convert to int</exception>
        public static Int64 ConvertToInt64(String stringValue)
        {
            if (String.IsNullOrWhiteSpace(stringValue))
            {
                throw new ArgumentException("stringValue is null or empty. Cannot convert to Int");
            }

            Int64 intVal = 0;
            Int64.TryParse(stringValue, out intVal);

            return intVal;
        }

        /// <summary>
        /// Convert object value into datetime
        /// </summary>
        /// <param name="objectValue">Value to be converted into datetime</param>
        /// <returns>Datetime equivalent of objectValue. If conversion fails, then returns 0</returns>
        /// <exception cref="ArgumentNullException">objectValue is null. Cannot convert to datetime</exception>
        public static DateTime ConvertToDateTime(Object objectValue)
        {
            if (objectValue == null)
            {
                throw new ArgumentNullException("objectValue", "Cannot convert to DateTime");
            }

            return ValueTypeHelper.ConvertToDateTime(objectValue.ToString());
        }

        /// <summary>
        /// Convert string value into datetime
        /// </summary>
        /// <param name="stringValue">Value to be converted into datetime</param>
        /// <returns>datetime equivalent of stringValue. If conversion fails, then returns DateTime.MinValue</returns>
        /// <exception cref="ArgumentNullException">stringValue is null. Cannot convert to datetime</exception>
        public static DateTime ConvertToDateTime(String stringValue)
        {
            if (String.IsNullOrWhiteSpace(stringValue))
            {
                throw new ArgumentException("stringValue is null or empty. Cannot convert to DateTime");
            }

            DateTime value = DateTime.MinValue;
            DateTime.TryParse(stringValue, out value);

            return value;
        }

        /// <summary>
        /// Convert string value into nullable datetime
        /// </summary>
        /// <param name="stringValue">Value to be converted into datetime</param>
        /// <returns>datetime equivalent of stringValue. If conversion fails, then returns DateTime.MinValue</returns>
        public static DateTime? ConvertToNullableDateTime(String stringValue)
        {
            if (String.IsNullOrWhiteSpace(stringValue))
            {
                return null;
            }

            DateTime value = DateTime.MinValue;
            DateTime.TryParse(stringValue, out value);

            return value;
        }

        /// <summary>
        /// Converts fraction value to decimal. Assumes that fraction value is valid
        /// </summary>
        /// <param name="fractionValue">Fraction value which needs to be converted</param>
        /// <returns>Decimal value of the fraction</returns>
        public static Decimal ConvertFractionToDecimal(String fractionValue)
        {
            Decimal decimalValue = 0;

            if (!String.IsNullOrWhiteSpace(fractionValue))
            {
                fractionValue = fractionValue.Trim();

                if (Decimal.TryParse(fractionValue, out decimalValue))
                {
                    //Able to parse.
                    //Incoming value is a whole number
                    return decimalValue;
                }

                //Incoming value is a fractional number
                //Convert fraction to decimal
                Int32 wholeNumber = 0;
                Decimal numerator = 0;
                Decimal denominator = 0;

                String strWholeNumber = String.Empty;
                String strFractionNumber = fractionValue;

                //Check whether it is Mixed fraction
                if (fractionValue.Contains(" "))
                {
                    Int32 spaceIndex = fractionValue.IndexOf(' ');
                    strWholeNumber = fractionValue.Substring(0, spaceIndex);
                    strFractionNumber = fractionValue.Substring(spaceIndex + 1).Trim();

                    Int32.TryParse(strWholeNumber, out wholeNumber);
                }

                //Get Numerator and denominator
                String[] partsOfFraction = strFractionNumber.Split('/');

                if (partsOfFraction != null && partsOfFraction[0] != null && partsOfFraction[1] != null)
                {
                    Decimal.TryParse(partsOfFraction[0], out numerator);
                    Decimal.TryParse(partsOfFraction[1], out denominator);
                }

                if (denominator > 0)
                {
                    decimalValue = wholeNumber + (numerator / denominator);
                }
                else
                {
                    throw new Exception("Denominator cannot be zero for a fraction.");
                }
            }

            return decimalValue;
        }

        /// <summary>
        /// Convert Boolean value to Int representation (1 or 0)
        /// </summary>
        /// <param name="boolValue">Boolean value to convert</param>
        /// <returns>Int16 equivalent of Boolean</returns>
        public static Int16 ConvertBooleanToInt(Boolean boolValue)
        {
            return
                boolValue ? (Int16)1 : (Int16)0;
        }

        /// <summary>
        /// Convert Boolean value to String representation (Y or N)
        /// </summary>
        /// <param name="boolValue">Boolean value to convert</param>
        /// <returns>String equivalent of Boolean</returns>
        public static String ConvertBooleanToString(Boolean boolValue)
        {
            return
                boolValue ? "Y" : "N";
        }

        /// <summary>
        /// Convert String value representation like 'Y' or 'N' to Boolean 
        /// </summary>
        /// <param name="valueAs_N_OR_Y">Boolean value to convert</param>
        /// <returns>Int16 equivalent of Boolean</returns>
        public static Boolean ConvertToBooleanFromShortString(String valueAs_N_OR_Y)
        {
            return
                valueAs_N_OR_Y.Equals("Y") || valueAs_N_OR_Y.Equals("y");
        }

        /// <summary>
        /// Convert String value representation like 'Y' or 'N' to Boolean 
        /// </summary>
        /// <param name="valueAs_0_OR_1">Boolean value to convert</param>
        /// <returns>Int16 equivalent of Boolean</returns>
        public static Boolean ConvertToBooleanFromInteger(Int32 valueAs_0_OR_1)
        {
            return valueAs_0_OR_1.Equals(1);
        }

        /// <summary>
        /// Convert string value into nullable Boolean
        /// </summary>
        /// <param name="stringValue">Value to be converted into Boolean</param>
        /// <returns>Boolean equivalent of stringValue. If conversion fails, then returns null</returns>
        public static Boolean? ConvertToNullableBoolean(String stringValue)
        {
            if (String.IsNullOrWhiteSpace(stringValue))
            {
                return null;
            }

            Boolean value;
            return Boolean.TryParse(stringValue, out value) ? (Boolean?)value : null;
        }

        /// <summary>
        /// Convert string value into nullable Int16
        /// </summary>
        /// <param name="stringValue">Value to be converted into Int16</param>
        /// <returns>Int16 equivalent of stringValue. If conversion fails, then returns null</returns>
        public static Byte? ConvertToNullableByte(String stringValue)
        {
            if (String.IsNullOrWhiteSpace(stringValue))
            {
                return null;
            }

            Byte value;
            return Byte.TryParse(stringValue, out value) ? (Byte?)value : null;
        }

        /// <summary>
        /// Convert string value into nullable Int16
        /// </summary>
        /// <param name="stringValue">Value to be converted into Int16</param>
        /// <returns>Int16 equivalent of stringValue. If conversion fails, then returns null</returns>
        public static Int16? ConvertToNullableInt16(String stringValue)
        {
            if (String.IsNullOrWhiteSpace(stringValue))
            {
                return null;
            }

            Int16 value;
            return Int16.TryParse(stringValue, out value) ? (Int16?)value : null;
        }

        /// <summary>
        /// Gets the default value if input type if input is null. Else returns the input value
        /// </summary>
        /// <typeparam name="T">Type of the input</typeparam>
        /// <param name="data">value to be casted</param>
        /// <returns>Gets the default value if input type if input is null. Else returns the input value</returns>
        public static T GetValue<T>(Object data)
        {
            T returnVal = default(T);

            if (data != null)
            {
                returnVal = (T)Convert.ChangeType(data, typeof(T));
            }

            return returnVal;
        }

        /// <summary>
        /// Convert string value into nullable Int32
        /// </summary>
        /// <param name="stringValue">Value to be converted into Int32</param>
        /// <returns>Int32 equivalent of stringValue. If conversion fails, then returns null</returns>
        public static Int32? ConvertToNullableInt32(String stringValue)
        {
            if (String.IsNullOrWhiteSpace(stringValue))
            {
                return null;
            }

            Int32 value;
            return Int32.TryParse(stringValue, out value) ? (Int32?)value : null;
        }

        /// <summary>
        /// Convert string value into nullable Int64
        /// </summary>
        /// <param name="stringValue">Value to be converted into Int32</param>
        /// <returns>Int64 equivalent of stringValue. If conversion fails, then returns null</returns>
        public static Int64? ConvertToNullableInt64(String stringValue)
        {
            if (String.IsNullOrWhiteSpace(stringValue))
            {
                return null;
            }

            Int64 value;
            return Int64.TryParse(stringValue, out value) ? (Int64?)value : null;
        }

        /// <summary>
        /// Convert string value into nullable Decimal
        /// </summary>
        /// <param name="stringValue">Value to be converted into Decimal</param>
        /// <returns>Decimal equivalent of stringValue. If conversion fails, then returns null</returns>
        public static Decimal? ConvertToNullableDecimal(String stringValue)
        {
            if (String.IsNullOrWhiteSpace(stringValue))
            {
                return null;
            }

            Decimal value;
            return Decimal.TryParse(stringValue, out value) ? (Decimal?)value : null;
        }

        /// <summary>
        /// Converts object value to nullable decimal.
        /// </summary>
        /// <param name="value">Value to be converted into Decimal.</param>
        /// <returns>Decimal equivalent of object Value. If conversion fails, then returns null</returns>
        public static Decimal? ConvertToNullableDecimal(Object value)
        {
            if (value != null && !String.IsNullOrWhiteSpace(value.ToString()))
            {
                return ValueTypeHelper.ConvertToNullableDecimal(value.ToString());
            }
            return null;
        }

        #endregion Helper methods for type conversion

        #region Helper methods for enums

        /// <summary>
        /// Returns list of values in provided enum type
        /// </summary>
        /// <typeparam name="T">Provide EnumType here</typeparam>
        /// <returns>List of Values</returns>
        public static IEnumerable<T> GetEnumValues<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }

        /// <summary>
        /// Get ObjectAction enum from db actions.
        /// </summary>
        /// <param name="action">db action</param>
        /// <returns>MDM Object Action</returns>
        public static ObjectAction GetAction(String action)
        {
            ObjectAction objectAction = ObjectAction.Read;

            if (!String.IsNullOrWhiteSpace(action))
            {
                switch (action.ToUpperInvariant())
                {
                    case "ADD":
                        objectAction = ObjectAction.Create;
                        break;
                    case "UPDATE":
                        objectAction = ObjectAction.Update;
                        break;
                    case "DELETE":
                        objectAction = ObjectAction.Delete;
                        break;
                    case "NOCHANGE":
                        objectAction = ObjectAction.Read;
                        break;
                    case "MOVE":
                        objectAction = ObjectAction.Reclassify;
                        break;
                    default:
                        Enum.TryParse<ObjectAction>(action, out objectAction);
                        break;
                }
            }
            return objectAction;
        }

        /// <summary>
        /// Get DB Action String from ActionEnum
        /// </summary>
        /// <param name="action">MDM Action</param>
        /// <returns>DB Action as String</returns>
        public static String GetActionString(ObjectAction action)
        {
            String actionString = String.Empty;

            if (action == ObjectAction.Create)
            {
                actionString = "Add";
            }
            else if (action == ObjectAction.Read)
            {
                actionString = "NoChange";
            }
            else if (action == ObjectAction.Reclassify)
            {
                actionString = "Move";
            }
            else
            {
                actionString = action.ToString();
            }

            return actionString;
        }

        /// <summary>
        /// Get Value from given Description
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="description">Description</param>
        /// <returns>List of Enum</returns>
        public static T GetValueFromDescription<T>(String description) where T : struct, IConvertible
        {
            var type = typeof(T);
            if (!type.IsEnum)
            {
                throw new InvalidOperationException();
            }
            foreach (var field in type.GetFields())
            {
                var attribute = Attribute.GetCustomAttribute(field,
                    typeof(DescriptionAttribute)) as DescriptionAttribute;
                if (attribute != null)
                {
                    if (attribute.Description.Equals(description, StringComparison.OrdinalIgnoreCase))
                    {
                        return (T)field.GetValue(null);
                    }
                }
                else
                {
                    if (field.Name == description)
                    {
                        return (T)field.GetValue(null);
                    }
                }
            }
            return default(T);
        }

        /// <summary>
        /// Get All Description of given type
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <returns>List of String having description</returns>
        public static List<String> GetAllDescriptions<T>() where T : struct, IConvertible
        {
            var type = typeof(T);
            if (!type.IsEnum)
            {
                throw new InvalidOperationException();
            }

            var descriptionsList = new List<String>();

            foreach (var field in type.GetFields())
            {
                var attribute =
                    Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
                if (attribute != null)
                {
                    descriptionsList.Add(attribute.Description);
                }
            }
            return descriptionsList;
        }

        /// <summary>
        /// Get All Description of given value
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="value">Value</param>
        /// <returns>List of String having description</returns>
        public static String GetDescriptionFromValue<T>(T value) where T : struct, IConvertible
        {
            var type = typeof(T);
            if (!type.IsEnum)
            {
                throw new InvalidOperationException();
            }
            FieldInfo field = type.GetField(value.ToString());
            DescriptionAttribute attr = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
            return attr != null ? attr.Description : value.ToString();
        }

        /// <summary>
        /// Converts the string representation of the name or numeric value of one or more enumerated constants to an equivalent enumerated object. A parameter
        ///  specifies whether the operation is case-sensitive. The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enumeration type to which to convert value.</typeparam>
        /// <param name="value">The string representation of the enumeration name or underlying value to convert.</param>
        /// <param name="ignoreCase">true to ignore case; false to consider case.</param>
        /// <param name="result">Specifies the result</param>
        /// <returns>true if the value parameter was converted successfully; otherwise, false.</returns>
        public static Boolean EnumTryParse<TEnum>(String value, Boolean ignoreCase, out TEnum result) where TEnum : struct
        {
            result = default(TEnum);
            Boolean returnResult = false;

            if (!String.IsNullOrWhiteSpace(value))
            {
                returnResult = Enum.TryParse(value, ignoreCase, out result);
            }

            return returnResult;
        }

        #endregion

        #region Helper methods for lookup search

        /// <summary>
        /// Get the lookup search operator from a string
        /// </summary>
        /// <param name="lookupSearchOperator">Indicates the string representation of lookup search operator</param>
        /// <returns>Returns the lookup search operator from a string representation of search operator</returns>
        public static LookupSearchOperatorEnum ConvertToLookupSearchOperatorEnum(String lookupSearchOperator)
        {
            LookupSearchOperatorEnum lookupSearchOperatorEnum = LookupSearchOperatorEnum.None;

            if (!String.IsNullOrWhiteSpace(lookupSearchOperator))
            {
                switch (lookupSearchOperator.ToLowerInvariant())
                {
                    case "eq":
                    case "equalto":
                    case "equals": lookupSearchOperatorEnum = LookupSearchOperatorEnum.EqualTo;
                        break;

                    case "contains":
                    case "cn": lookupSearchOperatorEnum = LookupSearchOperatorEnum.Contains;
                        break;

                    case "does not contains":
                    case "not contains":
                    case "notcontains":
                    case "nc": lookupSearchOperatorEnum = LookupSearchOperatorEnum.NotContains;
                        break;
                }
            }

            return lookupSearchOperatorEnum;
        }

        #endregion

        #region Collection methods

        /// <summary>
        /// Compare two collection 
        /// </summary>
        /// <typeparam name="T">Type of collection</typeparam>
        /// <param name="collection1">Collection1</param>
        /// <param name="collection2">Collection2</param>
        /// <returns>True if both are same</returns>
        public static Boolean CollectionEquals<T>(Collection<T> collection1, Collection<T> collection2)
        {
            if (collection1 == null || collection2 == null)
                return false;

            if (collection1.Count != collection2.Count)
                return false;

            foreach (T item in collection1)
            {
                if (!collection2.Contains(item))
                    return false; // the collections are not equal
            }

            foreach (T item in collection2)
            {
                if (!collection1.Contains(item))
                    return false; // the collections are not equal
            }

            return true;
        }


        /// <summary>
        /// Compare two collection with order
        /// </summary>
        /// <typeparam name="T">Type of collection</typeparam>
        /// <param name="collection1">Collection1</param>
        /// <param name="collection2">Collection2</param>
        /// <returns>True if both are same</returns>
        public static Boolean CollectionExactEquals<T>(Collection<T> collection1, Collection<T> collection2)
        {
            if (collection1 == null && collection2 == null)
            {
                return true;
            }

            if (collection1 == null || collection2 == null)
            {
                return false;
            }

            if (collection1.Count != collection2.Count)
            {
                return false;
            }

            for (Int32 index = 0; index < collection1.Count; index++)
            {
                T item1 = collection1[index];
                T item2 = collection2[index];

                if (item1.Equals(item2)==false)
                {
                    return false;
                }
            }

            return true;
        }
        

        /// <summary>
        /// Checks if at least one of the element matches in the source and target.
        /// </summary>
        /// <param name="source">The source collection</param>
        /// <param name="target">The target collection</param>
        /// <returns></returns>
        public static Boolean CheckIfAnyMatches<T>(Collection<T> source, Collection<T> target)
        {
            foreach (T sourceId in source)
            {
                if (target.Contains(sourceId))
                {
                    return true;
                }
            }
            return false;
        }

        #endregion
    }
}
