using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using MDM.Core;

namespace MDM.BusinessObjects.DQM
{
    /// <summary>
    /// This class handles the condition part of the Tibco Patterns Rules. 
    /// </summary>
    [DataContract]
    public class MatchingCondition : ICloneable
    {
        private double _weight;
        private List<String> _ruleNames;
        private List<double> _ruleWeights;
        private MatchScoreCombinerType _combinerType;
        private List<MatchingCondition> _conditions;

        /// <summary>
        /// Paramterless Constructor 
        /// </summary>
        public MatchingCondition()
        {
            _ruleNames = new List<string>();
            _ruleWeights = new List<double>();
            _conditions = new List<MatchingCondition>();
            _combinerType = MatchScoreCombinerType.AND;
            _weight = 0.0;
        }

        /// <summary>
        /// Constructor which takes MatchingCondition source as its input paramter
        /// </summary>
        /// <param name="source">Indicates MatchingCondition source</param>
        public MatchingCondition(MatchingCondition source)
        {
            _ruleNames = new List<string>(source._ruleNames);
            _ruleWeights = new List<double>(source._ruleWeights);
            _combinerType = source._combinerType;
            _weight = source._weight;
            _conditions = new List<MatchingCondition>();

            foreach (var condi in source._conditions)
            {
                var ncondi = new MatchingCondition(condi);
                _conditions.Add(ncondi);
            }
        }

        /// <summary>
        /// List of Rule Names that are configured in the Matching Condition
        /// </summary>
        [DataMember]
        public List<String> RuleNames
        {
            get { return _ruleNames; }
            set { _ruleNames = value; }
        }

        /// <summary>
        /// List of Rule Weights corresponding to RuleNames
        /// </summary>
        [DataMember]
        public List<double> RuleWeights
        {
            get { return _ruleWeights; }
            set { _ruleWeights = value; }
        }

        /// <summary>
        /// Matching Condition combiner.
        /// </summary>
        [DataMember]
        public MatchScoreCombinerType ScoreCombiner
        {
            get { return _combinerType;  }
            set { _combinerType = value; }
        }

        /// <summary>
        /// Matching Condition Weight.
        /// </summary>
        [DataMember]
        public double Weight
        {
            get { return _weight; }
            set { _weight = value; }
        }

        /// <summary>
        /// List of Matching Conditions( nested )
        /// </summary>
        [DataMember]
        public List<MatchingCondition> Conditions
        {
            get { return _conditions; }
            set { _conditions = value; }
        }

        #region ICloneable Members

        /// <summary>
        /// Implementation of IConeable interface.
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return new MatchingCondition(this);
        }

        #endregion
    }

}
