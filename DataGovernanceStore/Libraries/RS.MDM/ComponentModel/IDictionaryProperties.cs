using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RS.MDM.ComponentModel
{
    /// <summary>
    /// Provides functionality to an object to supply a set of possible key-value pairs for each property
    /// </summary>
    public interface IDictionaryProperties
    {
        /// <summary>
        /// Gets the string value for a given property
        /// </summary>
        /// <param name="propName">Indicates the name of the property</param>
        /// <returns>A string value that denotes the property value</returns>
        string GetValue(string propName);

        /// <summary>
        /// Gets the value for a given property and given key
        /// </summary>
        /// <param name="propName">Indicates the name of the property</param>
        /// <param name="key">Indicates the key for the property</param>
        /// <returns>A string value that denotes the property value for a given key</returns>
        string GetValue(string propName, int key);

        /// <summary>
        /// Gets the key for a given property
        /// </summary>
        /// <param name="propName">Indicates the name of the property</param>
        /// <returns>An integer value that indicates the key for a given property</returns>
        int GetKey(string propName);

        /// <summary>
        /// Gets the key for a given property and value
        /// </summary>
        /// <param name="propName">Indicates the name of the property</param>
        /// <param name="value">Indicates the value of the property</param>
        /// <returns>An integer value that indicates the key for a given property and value</returns>
        int GetKey(string propName, string value);

        /// <summary>
        /// Set the value of a property
        /// </summary>
        /// <param name="propName">Indicates the name of the property</param>
        /// <param name="value">Indicates the value of the property</param>
        void SetValue(string propName, string value);

        /// <summary>
        /// Get a dictionary for a given property name
        /// </summary>
        /// <param name="propName">Indicates the name of the property</param>
        /// <returns>A dictionary that contains possible key-value pairs for a given property name</returns>
        Dictionary<int, string> GetDictionary(string propName);
    }
}
