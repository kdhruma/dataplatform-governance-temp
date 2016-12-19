using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MDM.JigsawIntegrationManager
{
    using MDM.Core.Extensions;
    using Attribute = MDM.BusinessObjects.Attribute;
    using MDM.JigsawIntegrationManager.JsonSerializers;
    using Core;

    /// <summary>
    /// Represents JigsawExtensionMethods
    /// </summary>
    public static class JigsawExtensionMethods
    {
        /// <summary>
        /// Converts the object using standard deserializer for simple types and delegating complex deserialization logic to LoadFromJToken method
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        public static T ConvertToObject<T>(this JToken obj)
        {
            T result = obj.ToObject<T>(JigsawJsonSerializer.JsonSerializer);

            if (result != null && result is IJigsawJsonDeserializable)
            {
                (result as IJigsawJsonDeserializable).LoadFromJToken(obj);
            }

            return result;
        }

        /// <summary>
        /// Converts the property.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The object.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        public static T ConvertProperty<T>(this JObject obj, String propertyName)
        {
            JToken property = obj.GetProperty(propertyName);
            return property != null ? property.ToObject<T>(JigsawJsonSerializer.JsonSerializer) : default(T);
        }

        /// <summary>
        /// Converts the deserializable property.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The object.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        public static T ConvertDeserializableProperty<T>(this JObject obj, String propertyName) where T: IJigsawJsonDeserializable, new()
        {
            T result = new T();
            JToken property = obj.GetProperty(propertyName);

            if (property != null)
            {
                result = property.ConvertToObject<T>();
            }

            return result;
        }

        /// <summary>
        /// Converts the deserializable array.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">The array.</param>
        /// <param name="targetCollection">The target collection.</param>
        /// <returns></returns>
        public static void ConvertToCollection<T>(this JArray array, ICollection<T> targetCollection)
        {
            targetCollection.Clear();

            foreach (JToken token in array)
            {
                targetCollection.Add(token.ConvertToObject<T>());
            }
        }

        /// <summary>
        /// Converts the deserializable array.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">The array.</param>
        /// <param name="targetCollection">The target collection.</param>
        /// <returns></returns>
        public static void ConvertToCollection<T>(this JObject obj, String propertyName, ICollection<T> targetCollection)
        {
            JArray array = obj.GetProperty(propertyName) as JArray;

            if (array != null)
            {
                array.ConvertToCollection(targetCollection);
            }
        }

        /// <summary>
        /// To the j array.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">The collection.</param>
        /// <returns></returns>
        public static JArray ToJArray<T>(this ICollection<T> collection)
        {
            JArray jArray = new JArray();

            if (collection.IsNullOrEmpty())
            {
                return jArray;
            }

            foreach (T item in collection)
            {
                if (item is IJigsawJsonSerializable)
                {
                    jArray.Add((item as IJigsawJsonSerializable).ToJToken());
                }
                else
                {
                    jArray.Add(JToken.FromObject(item));
                }
            }

            return jArray;
        }

        /// <summary>
        /// To the jigsaw compliant string.
        /// </summary>
        public static String ToJigsawString(this object obj)
        {
            return obj.ToJigsawValue().ToString();
        }

        /// <summary>
        /// To the jigsaw compliant string.
        /// </summary>
        public static JToken ToJigsawValue(this object obj)
        {
            return JToken.FromObject(obj, JigsawJsonSerializer.JsonSerializer);
        }

        /// <summary>
        /// Determines whether [is validation state attribute].
        /// </summary>
        /// <param name="attribute">The attribute.</param>
        /// <returns></returns>
        public static Boolean IsValidationStateAttribute(this Attribute attribute)
        {
            if (attribute.AttributeParentName == "EntityState")
            {
                if (String.Compare(attribute.Name, SystemAttributes.LifecycleStatus.ToString(), true) == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }

            return false;
        }
        
        /// <summary>
        /// Determines whether the string is a Json String.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <returns>True if it is a Json String; Else false</returns>
        public static Boolean IsJsonString(this String input)
        {
            input = input.Trim();
            return input.StartsWith("{") && input.EndsWith("}")
                   || input.StartsWith("[") && input.EndsWith("]");
        }

        /// <summary>
        /// Returns a JObject if the string is a Json String else null.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <returns>Returns a JObject if the string is a Json String else null</returns>
        public static JObject ToJObject(this String input)
        {
            input = input.Trim();

            if (input.IsJsonString())
            {
                return JObject.Parse(input);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the property.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The object.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        private static JToken GetProperty(this JObject obj, String propertyName)
        {
            return obj.Property(propertyName) != null ? obj.Property(propertyName).Value : null;
        }
    }
}