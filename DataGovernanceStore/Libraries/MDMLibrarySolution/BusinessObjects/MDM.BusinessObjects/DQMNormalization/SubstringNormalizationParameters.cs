using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects.DQMNormalization
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies SubstringNormalizationParameters
    /// </summary>
    public class SubstringNormalizationParameters : BaseSynonymizationRuleParameters, ISubstringNormalizationParameters
    {

        #region Fields

        private Boolean _isCaseSensitive = false;

        private String _wordBreakers = String.Empty;

        #endregion

        #region Constructors

        /// <summary>
        /// Specifies constructor for SubstringNormalizationParameters
        /// </summary>
        public SubstringNormalizationParameters()
        {
        }

        /// <summary>
        /// Specifies copy constructor for SubstringNormalizationParameters
        /// </summary>
        /// <param name="source"></param>
        protected SubstringNormalizationParameters(SubstringNormalizationParameters source)
            : base(source)
        {
            _isCaseSensitive = source._isCaseSensitive;
            _wordBreakers = source._wordBreakers;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Specifies if Rule IsCaseSensitive
        /// </summary>
        [DataMember]
        public Boolean IsCaseSensitive
        {
            get { return _isCaseSensitive; }
            set { _isCaseSensitive = value; }
        }

        /// <summary>
        /// Specifies Word Breakers for synonymization
        /// </summary>
        [DataMember]
        public String WordBreakers
        {
            get { return _wordBreakers; }
            set { _wordBreakers = value; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Creates a clone copy of SubstringNormalizationParameters obejct
        /// </summary>
        /// <returns>Returns a clone copy of SubstringNormalizationParameters object</returns>
        public override Object Clone()
        {
            return new SubstringNormalizationParameters(this);
        }

        /// <summary>
        /// Specifies method for loading SubstringNormalizationParameters from xml
        /// </summary>
        /// <param name="xml"></param>
        /// <example>
        /// 
        /// </example>
        public override void LoadFromXml(String xml)
        {
             // XML Example
             // <SubstringNormalizationParameters IsCaseSensitive="false">
             //      <WordList Name="SynList1" />
             //      <WordBreakers><![CDATA[,.!\n ]]></WordBreakers>
             // </SubstringNormalizationParameters>

            if (!String.IsNullOrEmpty(xml))
            {
                base.LoadFromXml(xml);

                XmlDocument doc = new XmlDocument();
                doc.Load(new StringReader(xml));

                XmlElement root = doc.DocumentElement;
                if (root.Attributes.Count > 0 && !String.IsNullOrEmpty(root.Attributes["IsCaseSensitive"].Value))
                {
                    IsCaseSensitive = ValueTypeHelper.ConvertToNullableBoolean(root.Attributes["IsCaseSensitive"].Value) ?? false;
                }

                XmlNode wordBreakersNode = root.SelectSingleNode("WordBreakers");
                if (wordBreakersNode != null && !String.IsNullOrEmpty(wordBreakersNode.InnerText))
                {
                    _wordBreakers = wordBreakersNode.InnerText;
                }
            }

        }

        #endregion
    }
}
