using System;
using MDM.BusinessObjects.Exports;
using MDM.Core;

namespace MDM.Interfaces.Exports
{
    /// <summary>
    /// Exposes methods or properties to set or get the notification related information.
    /// </summary>
    public interface INotification : IMDMObject
    {
        #region Properties

        /// <summary>
        /// Property specifying email notification collection
        /// </summary>
        EmailNotificationCollection EmailNotifications { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Represents notification in Xml format
        /// </summary>
        /// <returns>notification in Xml format</returns>
        String ToXml();

        /// <summary>
        /// Represents notification in Xml format based on requested object serialization
        /// </summary>
        /// <param name="objectSerialization">Type of the serialization option</param>
        /// <returns>Xml representation of notification</returns>
        String ToXml(ObjectSerialization objectSerialization);

        #endregion Methods
    }
}
