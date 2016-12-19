using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDM.Interfaces
{
    /// <summary>
    /// Exposes methods or properties to set or get integration data.
    /// </summary>
    public interface IIntegrationData
    {
        #region Properties

        /// <summary>
        /// Specificities id for integration data
        /// </summary>
        String Id { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Get XML representation of integration data
        /// </summary>
        /// <returns>XML representation of integration data</returns>
        String ToXml();

        /// <summary>
        /// Get integration data
        /// </summary>
        /// <returns></returns>
        String GetData();

        /// <summary>
        /// Set integration data
        /// </summary>
        /// <param name="data">Holds value to be set</param>
        void SetData(String data);

        /// <summary>
        /// Get additional data for Integration object 
        /// </summary>
        /// <returns>collection of additional data - key and value pair collection</returns>
        Collection<KeyValuePair<String, String>> GetAdditionalData();

        /// <summary>
        /// Add additional data - key and value pair.
        /// Key must be unique.
        /// </summary>
        /// <param name="key">Key of AdditionalData to be add</param>
        /// <param name="value">Value of AdditionalData to be add</param>
        void AddAdditionalData(String key, String value);

        /// <summary>
        /// Get AdditionalData value based on key
        /// </summary>
        /// <param name="key">Key of AdditionalData to search on</param>
        /// <returns>AdditionlData key-value pair having specified key</returns>
        String GetAdditionalData(String key);

        /// <summary>
        /// Remove AdditionalData based on key
        /// </summary>
        /// <param name="key">Key of AdditionalData to be delete</param>
        /// <returns>true if item is successfully removed; otherwise, false.</returns>
        Boolean RemoveAdditionalData(String key);

        /// <summary>
        /// Check if additional data contains key value pair with given key
        /// </summary>
        /// <param name="key">key to search in additional data collection</param>
        /// <returns> true if key is found in the additional data collection; otherwise,false.</returns>
        Boolean Contains(String key);

        #endregion
    }
}