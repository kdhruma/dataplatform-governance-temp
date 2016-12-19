using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace MDM.BusinessObjects.DQM
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class MatchDefinition
    {
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public Collection<MatchRule> Rules { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public Collection<MatchingCondition> Conditions { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public Collection<MatchPredicate> Predicates { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public MatchResultSet ResultSet { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public MatchDefinition()
        {
            Rules = new Collection<MatchRule>();
            Predicates = new Collection<MatchPredicate>();
            Conditions = new Collection<MatchingCondition>();
            ResultSet = new MatchResultSet();
        }
    }
}