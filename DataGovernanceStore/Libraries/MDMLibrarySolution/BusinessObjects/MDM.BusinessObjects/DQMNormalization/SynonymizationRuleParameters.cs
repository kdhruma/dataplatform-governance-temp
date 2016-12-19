using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects.DQMNormalization
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Represents class for synonymization rule parameters
    /// </summary>
    [DataContract]
    public class SynonymizationRuleParameters : BaseSynonymizationRuleParameters, ISynonymizationRuleParameters
    {
        #region Fields

        private Dictionary<String, String> _synonymDictionary = new Dictionary<String, String>(StringComparer.OrdinalIgnoreCase);

        private Func<String, WordList> _getWordListByNameCallback;

        #endregion

        #region Constructors

        /// <summary>
        /// Specifies constructor for SynonymizationRuleParameters
        /// </summary>
        /// <param name="getWordListByNameCallbackCallback"></param>
        public SynonymizationRuleParameters(Func<String, WordList> getWordListByNameCallbackCallback)
        {
            _getWordListByNameCallback = getWordListByNameCallbackCallback;
        }

        /// <summary>
        /// Specifies copy constructor for SynonymizationRuleParameters
        /// </summary>
        /// <param name="source"></param>
        protected SynonymizationRuleParameters(SynonymizationRuleParameters source)
            : base(source)
        {
            _getWordListByNameCallback = (Func<String, WordList>) source._getWordListByNameCallback.Clone();
            _synonymDictionary = source._synonymDictionary.ToDictionary(entry => entry.Key, entry => entry.Value);
        }

        #endregion

        #region Properties
        
        /// <summary>
        /// Property for the SynonymDictionary
        /// </summary>
        [DataMember]
        public Dictionary<String, String> SynonymDictionary
        {
            get { return _synonymDictionary; }
            set { _synonymDictionary = value; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Creates a deep copy of synonymization rule parameter object
        /// </summary>
        /// <returns>Returns a deep copy of synonymization rule parameter object</returns>
        public override Object Clone()
        {
            return new SynonymizationRuleParameters(this);
        }

        /// <summary>
        /// Specifies method for loading rule from xml
        /// </summary>
        /// <param name="rule"></param>
        public override void LoadFromXml(String rule)
        {
            if (!String.IsNullOrEmpty(rule))
            {
                // Try to parse provided rule as xml
                // If xml is not valid then use provided string as a WordList name
                try
                {
                    #region Xml example

                    // <SynonimizationParameters>
                    //      <WordList Name="SynList1" />
                    // </SynonimizationParameters>

                    #endregion

                    base.LoadFromXml(rule);
                }
                catch
                {
                    this.WordListName = rule;
                }

                if (!String.IsNullOrEmpty(WordListName))
                {
                    WordList list = _getWordListByNameCallback(WordListName);
                    if (list != null && list.Id > 0)
                    {
                        foreach (WordElement wordElement in list.WordElements)
                        {
                            String sub = wordElement.Substitute.ToLower(CultureInfo.InvariantCulture);
                            if (!SynonymDictionary.ContainsKey(sub))
                            {
                                SynonymDictionary.Add(sub, wordElement.Word);
                            }
                        }
                    }
                }
            }
        }

        #endregion
    }
}