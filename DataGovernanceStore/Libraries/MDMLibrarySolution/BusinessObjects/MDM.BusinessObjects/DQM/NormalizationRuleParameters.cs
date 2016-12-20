using System;
using System.Runtime.Serialization;
using ProtoBuf;

namespace MDM.BusinessObjects.DQM
{
    /// <summary>
    /// Specify parameters for normalization rule
    /// </summary>
    [ProtoContract]
    [DataContract]
    public class NormalizationRuleParameters
    {
        /// <summary>
        /// Property denoting Rule Name
        /// </summary>
        [DataMember]
        public String RuleName { get; set; }

        /// <summary>
        /// Property denoting Rule Id
        /// </summary>
        [DataMember]
        public Int32 RuleId { get; set; }

        /// <summary>
        /// Property denoting parameter which will be used
        /// </summary>
        [DataMember]
        public String Parameter { get; set; }

        /// <summary>
        /// Property denoting Attribute Id
        /// </summary>
        [DataMember]
        public Int32? AttributeId { get; set; }
    }
}