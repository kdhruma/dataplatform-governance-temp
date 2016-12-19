using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects.DQM
{
    using Core.Extensions;
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies Matching Profile
    /// </summary>
    [DataContract]
    public class MatchingProfile : DQMJobProfile, IMatchingProfile
    {
        #region Fields

        /// <summary>
        /// Field indicates name of the profile.
        /// </summary>
        private String _profileName = String.Empty;

        /// <summary>
        /// Field indicates maximum number for Suspects to return.
        /// </summary>
        private Int32 _maxReturnRecords = 0;

        /// <summary>
        /// Field indicates the entity type identifier
        /// </summary>
        private Int32 _entityTypeId = 0;

        /// <summary>
        /// Field indicates the entity type name
        /// </summary>
        private String _entityTypeName = String.Empty;

        /// <summary>
        /// Filed indicates the source identifier
        /// </summary>
        private Int32 _sourceId = 0;

        /// <summary>
        /// Field indicates the source name
        /// </summary>
        private String _sourceName = String.Empty;

        /// <summary>
        /// Field indicates the match review profile identifier
        /// </summary>
        private Int32 _matchReviewProfileId = 0;

        /// <summary>
        /// Field indicates a value indicating whether to apply container filter.
        /// </summary>
        private Boolean _applyContainerFilter = false;

        /// <summary>
        /// Field indicates a value indicating whether to apply category filter.
        /// </summary>
        private Boolean _applyCategoryFilter = false;

        /// <summary>
        /// Field indicates a value indicating whether to include pending match review items.
        /// </summary>
        private Boolean _includePendingMatchReviewItems = false;

        /// <summary>
        /// Field indicates deterministic match rule
        /// </summary>
        private MatchRule _deterministicMatchRule = new MatchRule();

        /// <summary>
        /// Field indicates fuzzy match rule
        /// </summary>
        private MatchRule _fuzzyMatchRule = new MatchRule();

        /// <summary>
        /// Field indicates match event
        /// </summary>
        private MatchEvent _matchEvent = new MatchEvent();


        #endregion Fields

        #region Constuctors

        /// <summary>
        /// Parameterless Constructor 
        /// </summary>
        public MatchingProfile()
            : base(DQMJobType.Matching)
        {
            MaxReturnRecords = 10;
            Weightage = 1;
            Enabled = true;
        }

        /// <summary>
        /// Constructor which takes MatchingProfile as input parameter
        /// </summary>
        public MatchingProfile(MatchingProfile source)
            : base(source)
        {
            ProfileName = source.ProfileName;
            MaxReturnRecords = source.MaxReturnRecords;
            EntityTypeId = source.EntityTypeId;
            EntityTypeName = source.EntityTypeName;
            SourceId = source.SourceId;
            SourceName = source.SourceName;
            MatchReviewProfileId = source.MatchReviewProfileId;
            Locale = source.Locale;
            ApplyContainerFilter = source.ApplyContainerFilter;
            ApplyCategoryFilter = source.ApplyCategoryFilter;
            IncludePendingMatchReviewItems = source.IncludePendingMatchReviewItems;
            DeterministicMatchRule = source.DeterministicMatchRule.Clone();
            FuzzyMatchRule = source.FuzzyMatchRule.Clone();
            MatchEvent = source.MatchEvent.Clone();
        }

        /// <summary>
        /// Base Constructor 
        /// </summary>
        /// <param name="xmlWithOuterNode">Xml formatted string</param>
        public MatchingProfile(String xmlWithOuterNode)
        {
            this.JobType = DQMJobType.Matching;
            LoadFromXml(xmlWithOuterNode);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Name of the profile.
        /// </summary>
        [DataMember]
        public String ProfileName
        {
            get { return _profileName; }
            set { _profileName = value; }
        }
        
        /// <summary>
        /// Maximum number for Suspects to return.
        /// </summary>
        [DataMember]
        public Int32 MaxReturnRecords
        {
            get { return _maxReturnRecords; }
            set { _maxReturnRecords = value; }
        }

        /// <summary>
        /// Property denoting the entity type identifier.
        /// </summary>
        [DataMember]
        public Int32 EntityTypeId
        {
            get { return _entityTypeId; }
            set { _entityTypeId = value; }
        }

        /// <summary>
        /// Property denotes entity type name
        /// </summary>
        [DataMember]
        public String EntityTypeName
        {
            get { return _entityTypeName; }
            set { _entityTypeName = value; }
        }

        /// <summary>
        /// Property denoting the source identifier.
        /// </summary>
        [DataMember]
        public Int32 SourceId
        {
            get { return _sourceId; }
            set { _sourceId = value; }
        }

        /// <summary>
        /// Property denotes the source name
        /// </summary>
        [DataMember]
        public String SourceName
        {
            get { return _sourceName; }
            set { _sourceName = value; }
        }

        /// <summary>
        /// Property denoting the match review profile identifier.
        /// </summary>
        [DataMember]
        public Int32 MatchReviewProfileId
        {
            get { return _matchReviewProfileId; }
            set { _matchReviewProfileId = value; }
        }

        /// <summary>
        /// Property denoting a value indicating whether to apply container filter.
        /// </summary>
        [DataMember]
        public Boolean ApplyContainerFilter
        {
            get { return _applyContainerFilter; }
            set { _applyContainerFilter = value; }
        }

        /// <summary>
        /// Property denoting a value indicating whether to apply category filter.
        /// </summary>
        [DataMember]
        public Boolean ApplyCategoryFilter
        {
            get { return _applyCategoryFilter; }
            set { _applyCategoryFilter = value; }
        }

        /// <summary>
        /// Property denoting a value indicating whether to include pending match review items.
        /// </summary>
        [DataMember]
        public Boolean IncludePendingMatchReviewItems
        {
            get { return _includePendingMatchReviewItems; }
            set { _includePendingMatchReviewItems = value; }
        }

        /// <summary>
        /// Property denotes deterministic match rule
        /// </summary>
        [DataMember]
        public MatchRule DeterministicMatchRule
        {
            get { return _deterministicMatchRule; }
            set { _deterministicMatchRule = value; }
        }

        /// <summary>
        /// Property denotes fuzzy match rule
        /// </summary>
        [DataMember]
        public MatchRule FuzzyMatchRule
        {
            get { return _fuzzyMatchRule; }
            set { _fuzzyMatchRule = value; }
        }

        /// <summary>
        /// Property denotes match event
        /// </summary>
        [DataMember]
        public MatchEvent MatchEvent
        {
            get { return _matchEvent; }
            set { _matchEvent = value; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override String PropertiesOnlyToXmlWithOuterNode()
        {
            String returnXml = String.Empty;

            using (StringWriter sw = new StringWriter())
            {
                using (XmlTextWriter xmlWriter = new XmlTextWriter(sw))
                {
                    xmlWriter.WriteStartElement("MatchingProfile"); //MatchingProfile node start

                    #region write Matching Profile

                    xmlWriter.WriteAttributeString("Name", ProfileName);
                    xmlWriter.WriteAttributeString("EntityTypeId", EntityTypeId.ToString());
                    xmlWriter.WriteAttributeString("EntityTypeName", EntityTypeName);
                    xmlWriter.WriteAttributeString("SourceId", SourceId.ToString());
                    xmlWriter.WriteAttributeString("SourceName", SourceName);
                    xmlWriter.WriteAttributeString("Locale", Locale.ToString());
                    xmlWriter.WriteAttributeString("MatchReviewProfileId", MatchReviewProfileId.ToString());
                    xmlWriter.WriteAttributeString("ApplyContainerFilter", ApplyContainerFilter.ToString());
                    xmlWriter.WriteAttributeString("ApplyCategoryFilter", ApplyCategoryFilter.ToString());
                    xmlWriter.WriteAttributeString("IncludePendingMatchReviewItems", IncludePendingMatchReviewItems.ToString());
                    xmlWriter.WriteAttributeString("MaxRecords", MaxReturnRecords.ToString());

                    xmlWriter.WriteStartElement("DeterministicMatchRule"); // DeterministicMatchRule start node
                    xmlWriter.WriteRaw(this.DeterministicMatchRule.ToXml());
                    xmlWriter.WriteEndElement(); //DeterministicMatchRule end node

                    xmlWriter.WriteStartElement("FuzzyMatchRule"); // FuzzyMatchRule start node
                    xmlWriter.WriteRaw(this.FuzzyMatchRule.ToXml());
                    xmlWriter.WriteEndElement(); // FuzzyMatchRule end node

                    xmlWriter.WriteRaw(this.MatchEvent.ToXml());

                    #endregion write Matching Profile

                    xmlWriter.WriteEndElement(); //MatchingProfile node end
                    xmlWriter.Flush();

                    //get the actual XML
                    returnXml = sw.ToString();
                }
            }

            return returnXml;
        }

        /// <summary>
        /// Loads Matching Profile properties from Xml
        /// </summary>
        /// <param name="matchingProfileXmlNode">Xml Node containing information</param>
        public void LoadFromXml(String matchingProfileXmlNode)
        {
            if (!String.IsNullOrEmpty(matchingProfileXmlNode))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(matchingProfileXmlNode, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "MatchingProfile")
                        {
                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("Name"))
                                {
                                    this.ProfileName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("EntityTypeId"))
                                {
                                    this.EntityTypeId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), -1);
                                }

                                if (reader.MoveToAttribute("EntityTypeName"))
                                {
                                    this.EntityTypeName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("SourceId"))
                                {
                                    this.SourceId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), -1);
                                }

                                if (reader.MoveToAttribute("SourceName"))
                                {
                                    this.SourceName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("MatchReviewProfileId"))
                                {
                                    this.MatchReviewProfileId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), -1);
                                }

                                if (reader.MoveToAttribute("Locale"))
                                {
                                    LocaleEnum locale = LocaleEnum.UnKnown;
                                    Enum.TryParse(reader.ReadContentAsString(), out locale);
                                    this.Locale = locale;
                                }

                                if (reader.MoveToAttribute("ApplyContainerFilter"))
                                {
                                    this.ApplyContainerFilter = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), false);
                                }

                                if (reader.MoveToAttribute("ApplyCategoryFilter"))
                                {
                                    this.ApplyCategoryFilter = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), false);
                                }

                                if (reader.MoveToAttribute("IncludePendingMatchReviewItems"))
                                {
                                    this.IncludePendingMatchReviewItems = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), false);
                                }

                                if (reader.MoveToAttribute("MaxRecords"))
                                {
                                    this.MaxReturnRecords = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), -1);
                                }
                            }
                            else
                            {
                                reader.Read();
                            }
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "DeterministicMatchRule")
                        {
                            this.DeterministicMatchRule = new MatchRule(reader.ReadOuterXml());
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "FuzzyMatchRule")
                        {
                            this.FuzzyMatchRule = new MatchRule(reader.ReadOuterXml());
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "MatchEvent")
                        {
                            this.MatchEvent = new MatchEvent(reader.ReadOuterXml());
                        }
                        else
                        {
                            reader.Read();
                        }
                    }
                }
                finally
                {
                    if (reader != null)
                    {
                        reader.Close();
                    }
                }

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmlWithOuterNode"></param>
        public override void LoadPropertiesOnlyFromXmlWithOuterNode(String xmlWithOuterNode)
        {
            if (!String.IsNullOrWhiteSpace(xmlWithOuterNode))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(xmlWithOuterNode, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "MatchingProfile")
                        {
                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("Name"))
                                {
                                    this.ProfileName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("EntityTypeId"))
                                {
                                    this.EntityTypeId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), -1);
                                }

                                if (reader.MoveToAttribute("EntityTypeName"))
                                {
                                    this.EntityTypeName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("SourceId"))
                                {
                                    this.SourceId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), -1);
                                }

                                if (reader.MoveToAttribute("SourceName"))
                                {
                                    this.SourceName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("MatchReviewProfileId"))
                                {
                                    this.MatchReviewProfileId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), -1);
                                }

                                if (reader.MoveToAttribute("Locale"))
                                {
                                    LocaleEnum locale = LocaleEnum.UnKnown;
                                    Enum.TryParse(reader.ReadContentAsString(), out locale);
                                    this.Locale = locale;
                                }

                                if (reader.MoveToAttribute("ApplyContainerFilter"))
                                {
                                    this.ApplyContainerFilter = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), false);
                                }

                                if (reader.MoveToAttribute("ApplyCategoryFilter"))
                                {
                                    this.ApplyCategoryFilter = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), false);
                                }

                                if (reader.MoveToAttribute("IncludePendingMatchReviewItems"))
                                {
                                    this.IncludePendingMatchReviewItems = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), false);
                                }

                                if (reader.MoveToAttribute("MaxRecords"))
                                {
                                    this.MaxReturnRecords = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), -1);
                                }
                            }
                            else
                            {
                                reader.Read();
                            }
                        }
                        else
                        {
                            reader.Read();
                        }
                    }
                }
                finally
                {
                    if (reader != null)
                    {
                        reader.Close();
                    }
                }
            }
        }

        /// <summary>
        /// Convert matching profile into Json format.
        /// </summary>
        /// <returns>Returns JObject of matching profile</returns>
        public JObject ToJson()
        {
            return new JObject(
                    new JProperty("match",
                        new JObject(
                            new JProperty("mapping",
                                new JObject(
                                    new JProperty("systemAttributePath", "attributesInfo.system"),
                                    new JProperty("sourceAttributePath", "attributesInfo.source"),
                                    new JProperty("jobIdAttributePath", "jsAttributesInfo.jsChangeContext.jobId"),
                                    new JProperty("targetExternalIdPath","attributesInfo.externalId")
                                )
                            ),

                            new JProperty("systems",
                                new JObject(new JProperty("MDMCenter",
                                    new JObject(new JProperty("sources",
                                        new JObject(new JProperty(this.SourceName.ToJsCompliant(),
                                            new JObject(new JProperty("locales",
                                                new JObject(new JProperty(this.Locale.GetCultureName(),
                                                    new JObject(new JProperty("domains",
                                                        new JObject(new JProperty(this.EntityTypeName.ToJsCompliant(),
                                                            new JObject(
                                                                new JProperty("deterministic", this.DeterministicMatchRule.ToJson()),
                                                                new JProperty("fuzzy", this.FuzzyMatchRule.ToJson()),
                                                                new JProperty("matchEvent", this.MatchEvent.ToJson())
                                                            )
                                                        ))
                                                    ))
                                                ))
                                            ))
                                        ))
                                    ))
                                ))
                            ))
                        )
                      );
        }

        #endregion

        #region ICloneable Members

        /// <summary>
        /// Clone Matching profile
        /// </summary>
        /// <returns>Cloned Matching profile object</returns>
        public override Object Clone()
        {
            MatchingProfile clonedProfile = new MatchingProfile(this);
            return clonedProfile;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valuesAsXml"></param>
        private void LoadMatchRule(String valuesAsXml)
        {
            if (!String.IsNullOrEmpty(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "Deterministic")
                        {

                        }
                        else
                        {
                            reader.Read();
                        }
                    }
                }
                finally
                {
                    if (reader != null)
                    {
                        reader.Close();
                    }
                }

            }
        }

        #endregion
    }
}