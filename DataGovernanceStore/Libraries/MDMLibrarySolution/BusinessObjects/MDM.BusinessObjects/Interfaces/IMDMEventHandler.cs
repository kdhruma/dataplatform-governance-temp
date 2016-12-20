using System;
using System.Collections.ObjectModel;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Contains properties and methods to manipulate mdm event handler object
    /// </summary>
    public interface IMDMEventHandler : IMDMObject
    {
        #region Properties

        /// <summary>
        /// Property denoting the event information identifier.
        /// </summary>
        /// <value>
        /// The event information identifier.
        /// </value>
        Int32 EventInfoId { get; set; }

        /// <summary>
        /// Property denoting the name of the assembly.
        /// </summary>
        /// <value>
        /// The name of the assembly.
        /// </value>
        String AssemblyName { get; set; }

        /// <summary>
        /// Property denoting the fully qualified class name.
        /// </summary>
        /// <value>
        /// The fully qualified class name.
        /// </value>
        String FullyQualifiedClassName { get; set; }

        /// <summary>
        /// Property denoting the name of the event handler method.
        /// </summary>
        /// <value>
        /// The name of the event handler method.
        /// </value>
        String EventHandlerMethodName { get; set; }

        /// <summary>
        /// Property denoting the sequence.
        /// </summary>
        /// <value>
        /// The sequence.
        /// </value>
        Int32 Sequence { get; set; }

        /// <summary>
        /// Property denoting a value indicating whether mdm event handler is enabled.
        /// </summary>
        /// <value>
        /// true if enabled; otherwise, false.
        /// </value>
        Boolean Enabled { get; set; }

        /// <summary>
        /// Property denoting the module where this event handler is subscribed from.
        /// </summary>
        /// <value>
        /// The module.
        /// </value>
        MDMCenterExtensionEnum Module { get; set; }

        /// <summary>
        /// Property denoting the service types where this event handler is subscribed on.
        /// </summary>
        /// <value>
        /// The subscribed on service types.
        /// </value>
        Collection<MDMServiceType> SubscribedOnServiceTypes { get; set; }

        /// <summary>
        /// Property denoting a value indicating whether the handler method is static.
        /// </summary>
        /// <value>
        /// true if the handler method is static; otherwise, false.
        /// </value>
        Boolean IsHandlerMethodStatic { get; set; }

        /// <summary>
        /// Property denoting a value indicating whether the event handler method is internal.
        /// </summary>
        /// <value>
        /// true if this handler is internal; otherwise, false.
        /// </value>
        Boolean IsInternal { get; set; }

        /// <summary>
        /// Property denoting the name of the appconfig key
        /// </summary>
        /// <value>
        /// The name of the appconfig key
        /// </value>
        String AppConfigKeyName { get; set; }

        /// <summary>
        /// Property denoting the value of the appconfig key
        /// </summary>
        /// <value>
        /// The value of the appconfig key
        /// </value>
        String AppConfigKeyValue { get; set; }

        /// <summary>
        /// Property denoting the name of the featureconfig key
        /// </summary>
        /// <value>
        /// The name of the featureconfig key
        /// </value>
        String FeatureConfigKeyName { get; set; }

        /// <summary>
        /// Property denoting the value of the featureconfig key
        /// </summary>
        /// <value>
        /// The value of the featureconfig key
        /// </value>
        Boolean FeatureConfigKeyValue { get; set; }

        #endregion Properties

        #region Methods
        /// <summary>
        /// Get XML representation of mdm event handler object
        /// </summary>
        /// <returns>XML representation of mdm event handler object</returns>
        String ToXml();

        /// <summary>
        /// Gets a cloned instance of the current mdm event handler object
        /// </summary>
        /// <returns>Cloned instance of the current mdm event handler object</returns>
        IMDMEventHandler Clone();

        /// <summary>
        /// Gets a MDMEventInfo for the current MDMEvent handler
        /// </summary>
        /// <returns>Returns the MDMEventInfo Object</returns>
        IMDMEventInfo GetMDMEventInfo();

        #endregion Methods
    }
}
