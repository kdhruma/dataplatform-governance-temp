using System;
using System.Collections.ObjectModel;
using MDM.Core;

namespace MDM.Interfaces.Exports
{
    /// <summary>
    /// Exposes methods or properties to set or get the email notification business object.
    /// </summary>
    public interface IEmailNotification : IMDMObject
    {
        #region Properties

        /// <summary>
        /// Property specifying email notification action
        /// </summary>
        new ExportJobStatus Action { get; set; }

        /// <summary>
        /// Property specifying email notification emails
        /// </summary>
        Collection<String> Emails { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Represents email notification in Xml format
        /// </summary>
        /// <returns>ExportProfile in Xml format</returns>
        String ToXml();

        /// <summary>
        /// Represents email notification in Xml format based on requested object serialization
        /// </summary>
        /// <param name="objectSerialization">Type of the serialization option</param>
        /// <returns>Xml representation of email notification</returns>
        String ToXml(ObjectSerialization objectSerialization);

        #endregion Methods
    }
}
