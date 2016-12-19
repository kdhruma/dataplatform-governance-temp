using System;
using System.Collections.ObjectModel;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects.Integration;

    /// <summary>
    /// Specifies JigsawIntegrationConfiguration interface
    /// </summary>
    public interface IJigsawIntegrationConfiguration : ICloneable
    {

        /// <summary>
        /// Gets or sets the zookeeper configuration.
        /// </summary>
        Collection<JigsawNode> ZookeeperConfiguration { get; set; }

        /// <summary>
        /// Gets or sets the kafka configuration.
        /// </summary>
        Collection<JigsawNode> KafkaConfiguration { get; set; }

        /// <summary>
        /// Gets or sets the topic configuration.
        /// </summary>
        Collection<JigsawTopic> TopicConfiguration { get; set; }
    }
}