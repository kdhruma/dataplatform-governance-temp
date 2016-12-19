using System;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get type of integration message.
    /// </summary>
    public interface IIntegrationMessageType
    {
        #region Properties

        /// <summary>
        /// Indicates Id of type of integration message
        /// </summary>
        Int16 Id { get; set; }

        /// <summary>
        /// Indicates action to be performed on the object
        /// </summary>
        ObjectAction Action { get; set; }

        /// <summary>
        /// Indicates Short name for the message type
        /// </summary>
        String Name { get; set; }

        /// <summary>
        /// Indicates Long name for the message type. This is typically fully qualified class name
        /// </summary>
        String LongName { get; set; }

        /// <summary>
        /// Indicates Id of connector for which this message will be used
        /// </summary>
        Int16 ConnectorId { get; set; }

        /// <summary>
        /// Indicates name of connector for which this message will be used
        /// </summary>
        String ConnectorLongName { get; set; }

        /// <summary>
        /// Indicates weightage for integration message
        /// </summary>
        Int32 Weightage {get;set;}

        #endregion Properties

        #region Methods

        /// <summary>
        /// Represents IntegrationMessageType in Xml format
        /// </summary>
        String ToXml();

        #endregion Methods
    }
}
