using System;
using MDM.Core;

namespace MDM.Interfaces
{
    /// <summary>
    /// Exposes methods or properties to set or get the Notification setting business object.
    /// </summary>
    public interface INotificationSetting
    {
        #region Properties

        /// <summary>
        /// Property specifying Notification setting name
        /// </summary>
        String Name { get; set; }

        /// <summary>
        /// Property specifying Notification setting value
        /// </summary>
        String Value { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Represents Notification setting in Xml format
        /// </summary>
        /// <returns>Notification setting in Xml format</returns>
        String ToXml();

        /// <summary>
        /// Represents Notification setting in Xml format based on requested object serialization
        /// </summary>
        /// <param name="objectSerialization">Type of the serialization option</param>
        /// <returns>Xml representation of Notification setting</returns>
        String ToXml(ObjectSerialization objectSerialization);

        #endregion Methods
    }
}
