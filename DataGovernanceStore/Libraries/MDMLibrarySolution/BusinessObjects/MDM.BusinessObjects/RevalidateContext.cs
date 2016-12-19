using ProtoBuf;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using System.Collections.ObjectModel;
using System.Linq;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;
    using MDM.Core.Extensions;

    /// <summary>
    /// Specifies class for revalidate context for dynamic data governance.
    /// </summary>
    [DataContract]
    [ProtoContract(ImplicitFields = ImplicitFields.AllFields)]
    [KnownType(typeof(MDMObject))]
    public class RevalidateContext : MDMObject, IRevalidateContext
    {
        #region Fields

        /// <summary>
        /// Indicates search criteria to specify the context for dynamic data governance. 
        /// </summary>
        private SearchCriteria _searchCriteria;

        /// <summary>
        ///  Indicates collection of entity identifiers on which rule to be applied
        /// </summary>
        private Collection<Int64> _entityIds;

        /// <summary>
        /// Indicates collection of rule map context identifiers which needs to be applied
        /// </summary>
        private Collection<Int32> _ruleMapContextIds;

        /// <summary>
        /// Indicates collection of rule map context names which needs to be applied
        /// </summary>
        private Collection<String> _ruleMapContextNames;

        /// <summary>
        /// Indicates the revalidate mode for triggering the rule
        /// </summary>
        private RevalidateMode _revalidateMode = RevalidateMode.Unknown;

        /// <summary>
        /// Indicates whether revalidate context record for master collaboration or not
        /// </summary>
        private Boolean _isMasterCollaborationRecord = false;

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public RevalidateContext()
            : base()
        {
        }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        /// <param name="valuesAsXml">XML having xml value</param>
        public RevalidateContext(String valuesAsXml)
        {
            this.LoadRevalidateContext(valuesAsXml);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Indicates search criteria to specify the context for dynamic data governance. 
        /// </summary>
        [DataMember]
        public SearchCriteria SearchCriteria
        {
            get
            {
                return this._searchCriteria;
            }
            set
            {
                this._searchCriteria = value;
            }
        }

        /// <summary>
        ///  Indicates collection of entity identifiers on which rule to be applied
        /// </summary>
        [DataMember]
        public Collection<Int64> EntityIds
        {
            get
            {
                return this._entityIds;
            }
            set
            {
                this._entityIds= value;
            }
        }

        /// <summary>
        /// Indicates collection of rule map context identifiers which needs to be applied
        /// </summary>
        [DataMember]
        public Collection<Int32> RuleMapContextIds
        {
            get
            {
                return this._ruleMapContextIds;
            }
            set
            {
                this._ruleMapContextIds = value;
            }
        }

        /// <summary>
        /// Indicates collection of rule map context names which needs to be applied
        /// </summary>
        [DataMember]
        public Collection<String> RuleMapContextNames
        {
            get
            {
                return this._ruleMapContextNames;
            }
            set
            {
                this._ruleMapContextNames = value;
            }
        }

        /// <summary>
        /// Indicates the revalidate mode for triggering the rule
        /// </summary>
        [DataMember]
        public RevalidateMode RevalidateMode
        {
            get
            {
                return this._revalidateMode;
            }
            set
            {
                this._revalidateMode = value;
            }
        }

        /// <summary>
        /// Indicates whether revalidate context record for master collaboration or not
        /// </summary>
        [DataMember]
        public Boolean IsMasterCollaborationRecord
        {
            get
            {
                return this._isMasterCollaborationRecord;
            }
            set
            {
                this._isMasterCollaborationRecord = value;
            }
        }

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Gives xml representation of an instance
        /// </summary>
        /// <returns>String xml representation</returns>
        public override String ToXml()
        {
            String returnXml = String.Empty;

            using (StringWriter sw = new StringWriter())
            {
                using (XmlTextWriter xmlWriter = new XmlTextWriter(sw))
                {
                    String strEntityIds = String.Empty;

                    if (this.EntityIds != null && this.EntityIds.Count > 0)
                    {
                        strEntityIds = ValueTypeHelper.JoinCollection<Int64>(this.EntityIds, ",");
                    }

                    String ruleMapContextIds = String.Empty;

                    if (this.RuleMapContextIds != null && this.RuleMapContextIds.Count > 0)
                    {
                        ruleMapContextIds = ValueTypeHelper.JoinCollection<Int32>(this.RuleMapContextIds, ",");
                    }

                    String ruleMapContextNames= String.Empty;

                    if (this.RuleMapContextNames != null && this.RuleMapContextNames.Count > 0)
                    {
                        ruleMapContextNames = ValueTypeHelper.JoinCollection<String>(this.RuleMapContextNames, ",");
                    }

                    //EntityStateValidation node start
                    xmlWriter.WriteStartElement("RevalidateContext");

                    #region write RevalidateContext
                    xmlWriter.WriteAttributeString("EntityIds", strEntityIds);
                    xmlWriter.WriteAttributeString("RuleMapContextIds", ruleMapContextIds);
                    xmlWriter.WriteAttributeString("RuleMapContextNames", ruleMapContextNames);
                    xmlWriter.WriteAttributeString("RevalidateMode", this.RevalidateMode.ToString());

                    if (this.SearchCriteria != null)
                    {
                        xmlWriter.WriteRaw(this.SearchCriteria.ToXml());
                    }

                    #endregion write RevalidateContext

                    //EntityStateValidation node end
                    xmlWriter.WriteEndElement();
                    xmlWriter.Flush();

                    //get the return XML
                    returnXml = sw.ToString();
                }
            }
                
            return returnXml;
        }

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override Boolean Equals(Object obj)
        {
            if (obj is RevalidateContext)
            {
                RevalidateContext objectToBeCompared = obj as RevalidateContext;

                if (!this.SearchCriteria.Equals(objectToBeCompared.SearchCriteria))
                {
                    return false;
                }

                Int32 entityIdsUnion = this.EntityIds.ToList().Union(objectToBeCompared.EntityIds.ToList()).Count();
                Int32 entityIdsIntersect = this.EntityIds.ToList().Intersect(objectToBeCompared.EntityIds.ToList()).Count();

                if (entityIdsUnion != entityIdsIntersect)
                {
                    return false;
                }

                Int32 ruleMapContextIdsUnion = this.RuleMapContextIds.ToList().Union(objectToBeCompared.RuleMapContextIds.ToList()).Count();
                Int32 ruleMapContextIdsIntersect = this.RuleMapContextIds.ToList().Intersect(objectToBeCompared.RuleMapContextIds.ToList()).Count();

                if (ruleMapContextIdsUnion != ruleMapContextIdsIntersect)
                {
                    return false;
                }

                Int32 ruleMapContextNamesUnion = this.RuleMapContextNames.ToList().Union(objectToBeCompared.RuleMapContextNames.ToList()).Count();
                Int32 ruleMapContextNamesIntersect = this.RuleMapContextNames.ToList().Intersect(objectToBeCompared.RuleMapContextNames.ToList()).Count();

                if (ruleMapContextNamesUnion != ruleMapContextNamesIntersect)
                {
                    return false;
                }

                if (this.RevalidateMode != objectToBeCompared.RevalidateMode)
                {
                    return false;
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Serves as a hash function for type
        /// </summary>
        /// <returns>A hash code for the current Object.</returns>
        public override Int32 GetHashCode()
        {
            Int32 hashCode = 0;

            hashCode = this.EntityIds.GetHashCode() ^ this.RuleMapContextIds.GetHashCode() ^ this.RuleMapContextNames.GetHashCode() ^ this.RevalidateMode.GetHashCode() ^
                       this.SearchCriteria.GetHashCode();

            return hashCode;
        }

        /// <summary>
        /// Delta Merge of revalidate context
        /// </summary>
        /// <param name="deltaRevalidateContext">Indicates delta revalidate context needs to be merged</param>
        public void Merge(RevalidateContext deltaRevalidateContext)
        {
            //Merge only if we have anything from delta.
            if (deltaRevalidateContext != null)
            {
                this.RuleMapContextIds.AddRange<Int32>(deltaRevalidateContext.RuleMapContextIds, true);
                this.RuleMapContextNames.AddRange<String>(deltaRevalidateContext.RuleMapContextNames, true);
            }
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Load revalidate context from xml 
        /// </summary>
        /// <param name="valuesAsXml"></param>
        private void LoadRevalidateContext(String valuesAsXml)
        {
            if (string.IsNullOrWhiteSpace(valuesAsXml))
            {
                return;
            }

            XmlTextReader reader = null;
            try
            {
                reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                while (!reader.EOF)
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "RevalidateContext")
                    {
                        #region Read RevalidateContext

                        if (reader.HasAttributes)
                        {
                            if (reader.MoveToAttribute("EntityIds"))
                            {
                                this.EntityIds = ValueTypeHelper.SplitStringToLongCollection(reader.ReadContentAsString(), ',');
                            }
                            if (reader.MoveToAttribute("RuleMapContextIds"))
                            {
                                this.RuleMapContextIds = ValueTypeHelper.SplitStringToIntCollection(reader.ReadContentAsString(), ',');
                            }
                            if (reader.MoveToAttribute("RuleMapContextNames"))
                            {
                                this.RuleMapContextNames = ValueTypeHelper.SplitStringToStringCollection(reader.ReadContentAsString(), ',');
                            }
                            if (reader.MoveToAttribute("RevalidateMode"))
                            {
                                ValueTypeHelper.EnumTryParse(reader.ReadContentAsString(), true, out this._revalidateMode);
                                this.RevalidateMode = this._revalidateMode;
                            }
                        }
                        #endregion Read Revalidate Context
                    }
                    else if (reader.NodeType == XmlNodeType.Element && reader.Name == "SearchCriteria")
                    {
                        #region Read SearchCriteria

                        String SearchCriteriaXml = reader.ReadOuterXml();
                        this.SearchCriteria = new SearchCriteria(SearchCriteriaXml);

                        #endregion
                    }
                    else
                    {
                        //Keep on reading the xml until we reach expected node.
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
        #endregion Private Methods

        #endregion Methods
    }
}
