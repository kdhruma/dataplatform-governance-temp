using System;
namespace MDM.Interfaces
{
    /// <summary>
    /// Exposes methods or properties to set or get properties for connector's run time configuration such as Assembly name, class name implementing IConnector methods.
    /// </summary>
    public interface IRunTimeSpecifications
    {
        #region Properties

        /// <summary>
        /// Indicates name of assembly containing implementation of IConnector for given connector
        /// </summary>
        String AssemblyName { get; set; }

        /// <summary>
        /// Indicates name of class containing implementation of IConnector for given connector
        /// </summary>
        String ClassName { get; set; }

        /// <summary>
        /// Indicates file watcher folder where files will be dropped for this connector
        /// </summary>
        String FileWatcherFolderName { get; set; }

        /// <summary>
        /// Decides whether Orchestration is done by core system or connector implementation team will do it.
        /// True : Core will call the connector methods in sequence.
        /// False : Core will only call ProcessInboundMessage / ProcessOutboundMessage. Calling rest of IConnector methods will be done by external system.
        /// </summary>
        Boolean UseInplaceOrchestration { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Represents RunTimeSpecifications in Xml format
        /// </summary>
        String ToXml();

        /// <summary>
        /// Clone the RunTimeSpecifications object and return new instance with the same values.
        /// </summary>
        /// <returns>New cloned IRunTimeSpecifications</returns>
        IRunTimeSpecifications Clone();

        #endregion Methods
    }
}
