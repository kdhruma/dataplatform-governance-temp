using System;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Contains properties and methods to manipulate event info object
    /// </summary>
    public interface IMDMEventInfo : IMDMObject
    {
        #region Properties

        /// <summary>
        /// Indicates the name of the event manager class.
        /// </summary>
        /// <value>
        /// The name of the event manager class.
        /// </value>
        String EventManagerClassName { get; set; }

        /// <summary>
        /// Indicates the description of the event.
        /// </summary>
        /// <value>
        /// The description of the event.
        /// </value>
        String Description { get; set; }

        /// <summary>
        /// Indicates a value indicating whether this event info is obsolete.
        /// </summary>
        /// <value>
        /// True if this event info is obsolete; otherwise, False.
        /// </value>
        Boolean IsObsolete { get; set; }

        /// <summary>
        /// Indicates the alternate event information identifier.
        /// </summary>
        /// <value>
        /// The alternate event information identifier.
        /// </value>
        Int32 AlternateEventInfoId { get; set; }

        /// <summary>
        /// Indicates whether this event info has business rule support.
        /// </summary>
        /// <value>
        /// True if this event info has business rule support; otherwise, False.
        /// </value>
        Boolean HasBusinessRuleSupport { get; set; }

        /// <summary>
        /// Property denoting a value indicating whether this event info is internal.
        /// </summary>
        /// <value>
        /// true if this instance is internal; otherwise, false.
        /// </value>
        Boolean IsInternal { get; set; }

        /// <summary>
        /// Property denoting the name of the assembly.
        /// </summary>
        /// <value>
        /// The name of the assembly.
        /// </value>
        String AssemblyName { get; set; }

         /// <summary>
        /// Property denoting the name of the Event.
        /// </summary>
        /// <value>
        /// The name of the Event.
        /// </value>
        String EventName { get; set; }
                
        #endregion Properties

        #region Methods

        /// <summary>
        /// Get XML representation of event info object
        /// </summary>
        /// <returns>XML representation of event info object</returns>
        String ToXml();
        
        /// <summary>
        /// Gets a cloned instance of the current event info object
        /// </summary>
        /// <returns>Cloned instance of the current event info object</returns>
        IMDMEventInfo Clone();

        #endregion Methods
    }
}
