using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDM.Utility
{
    /// <summary>
    /// Represents the helper methods for entity search value builder
    /// </summary>
    public class EntitySearchValuesBuilder
    {
        private StringBuilder _keyValueBuilder = new StringBuilder();
        private StringBuilder _searchValueBuilder = new StringBuilder();

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public EntitySearchValuesBuilder()
        {
        }

        /// <summary>
        /// Add key Value in key value builder
        /// </summary>
        /// <param name="keyValue">key value which needs to be added</param>
        public void AddKeyValue(string keyValue)
        {
            _keyValueBuilder.Append(keyValue);
            _keyValueBuilder.Append(" ");
        }

        /// <summary>
        /// Add search Value in search value builder
        /// </summary>
        /// <param name="searchValue">search value which needs to be added</param>
        public void AddSearchValue(string searchValue)
        {
            _searchValueBuilder.Append(searchValue);
            _searchValueBuilder.Append(" ");
        }

        /// <summary>
        /// Remove search value builder and key value builder
        /// </summary>
        public void Clear()
        {
            _searchValueBuilder.Clear();
            _keyValueBuilder.Clear();
        }

        /// <summary>
        /// Property defining the key value
        /// </summary>
        public string KeyValue
        {
            get
            {
                return _keyValueBuilder.ToString();
            }
        }

        /// <summary>
        /// Property defining the search value
        /// </summary>
        public string SearchValue
        {
            get
            {
                return _searchValueBuilder.ToString();
            }
        }
    }
}
