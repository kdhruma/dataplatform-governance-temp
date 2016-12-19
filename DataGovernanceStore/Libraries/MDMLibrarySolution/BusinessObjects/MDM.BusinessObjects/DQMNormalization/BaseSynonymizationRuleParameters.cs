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
    public abstract class BaseSynonymizationRuleParameters : MDMObject, IBaseSynonymizationRuleParameters, ICloneable
    {
        #region Fields

        private String _wordListName = String.Empty;
        
        #endregion

        #region Constructors

        /// <summary>
        /// Specifies constructor for BaseSynonymizationRuleParameters
        /// </summary>
        protected BaseSynonymizationRuleParameters()
        {
        }

        /// <summary>
        /// Specifies copy constructor for BaseSynonymizationRuleParameters
        /// </summary>
        /// <param name="source"></param>
        protected BaseSynonymizationRuleParameters(BaseSynonymizationRuleParameters source)
        {
            _wordListName = source._wordListName;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Property for the WordLшstName
        /// </summary>
        [DataMember]
        public String WordListName
        {
            get { return _wordListName; }
            set { _wordListName = value; }
        }
        
        #endregion

        #region Public Methods

        /// <summary>
        /// Creates a clone deep of synonymization rule parameter object
        /// </summary>
        /// <returns>Returns a deep copy of synonymization rule parameter object</returns>
        public abstract Object Clone();

        /// <summary>
        /// Specifies method for loading rule from xml
        /// </summary>
        /// <param name="xml"></param>
        public virtual void LoadFromXml(String xml)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(new StringReader(xml));

            XmlElement root = doc.DocumentElement;

            XmlNodeList wordListNodes = root.GetElementsByTagName("WordList");

            if (wordListNodes.Count > 0)
            {
                WordListName = wordListNodes.Item(0).Attributes["Name"].Value;
            }
        }

        #endregion
    }
}