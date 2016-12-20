using System.Collections.Generic;
using MDM.Core;

namespace MDM.BusinessObjects
{
    /// <summary>
    /// Represents class for business object
    /// </summary>
    public class BusinessObject
    {
        /// <summary>
        /// Property denoting identifier of business object
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Property denoting name of business object
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Property denoting entity type name
        /// </summary>
        public string EntityTypeName { get; set; }

        /// <summary>
        /// Property denoting attributes of business object
        /// </summary>
        public Dictionary<int, BusinessObjectAttribute> Attributes { get; set; }

        /// <summary>
        /// Get business object attribute object based on id and locale
        /// </summary>
        /// <param name="id">Indicates the identifier of attribute</param>
        /// <param name="locale">Indicates the locale based on which object to be returned</param>
        /// <returns>Returns business object attribute object based on id and locale</returns>
        public BusinessObjectAttribute GetAttribute(int id, LocaleEnum locale)
        {
            //todo[mv]: so far there is no locale support
            BusinessObjectAttribute result;
            Attributes.TryGetValue(id, out result);
            return result;
        }
    }
}
