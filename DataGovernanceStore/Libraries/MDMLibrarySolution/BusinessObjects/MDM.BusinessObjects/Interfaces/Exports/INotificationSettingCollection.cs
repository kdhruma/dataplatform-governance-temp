using System;
using System.Collections.Generic;
using MDM.BusinessObjects.Exports;
using MDM.Core;

namespace MDM.Interfaces
{
    /// <summary>
    /// Exposes methods or properties to set or get the Notification setting collection.
    /// </summary>
    public interface INotificationSettingCollection : ICollection<NotificationSetting>
    {
        #region Methods

        /// <summary>
        /// Get Xml representation of Notificationsetting collection
        /// </summary>
        /// <returns>Xml representation of Notificationsetting collection</returns>
        String ToXml();

        /// <summary>
        /// Get Xml representation of Notificationsetting collection based on requested object serialization
        /// </summary>
        /// <param name="objectSerialization">Type of the serialization option</param>
        /// <returns>Xml representation of Notificationsetting collection</returns>
        String ToXml(ObjectSerialization objectSerialization);

        #endregion Methods
    }
}
