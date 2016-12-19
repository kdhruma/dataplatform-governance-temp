using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MDM.BusinessObjects.DQM
{
    using MDM.Core;

    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    [Serializable]
    [ProtoContract]
    public class MatchSuspectCollection 
    {
        #region Properties

        /// <summary>
        /// Id of the query that generated these results
        /// </summary>
        [DataMember]
        [ProtoMember(1)]
        public Int64 QueryId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        [ProtoMember(2)]
        public String MatchType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        [ProtoMember(3)]
        public Collection<MatchSuspect> Suspects { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        [ProtoMember(4)]
        public String NativeResults { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MatchSuspectCollection"/> class.
        /// </summary>
        public MatchSuspectCollection()
        {
            Suspects = new Collection<MatchSuspect>();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Determines whether the current object instance is superset of the operation result passed as parameter
        /// </summary>
        /// <param name="subsetSuspectCollection">Indicates the subset object to compare with the current object</param>
        /// <param name="compareId">Indicates whether ids to be compared or not</param>
        /// <returns>Returns true if the current object is superset of the subset instances; otherwise false</returns>
        public Boolean IsSuperSetOf(MatchSuspectCollection subsetSuspectCollection, Boolean compareId = false)
        {
            if (compareId)
            {
                if (this.QueryId != subsetSuspectCollection.QueryId)
                    return false;

                // Native Result contains matching context ids, Thats why NativeResults comes in this scope.
                if (this.NativeResults != subsetSuspectCollection.NativeResults)
                    return false;
            }

            if (this.MatchType != subsetSuspectCollection.MatchType)
                return false;

            if (((this.Suspects != null && this.Suspects.Count > 0) && (subsetSuspectCollection.Suspects == null || subsetSuspectCollection.Suspects.Count == 0)) ||
                ((this.Suspects == null || this.Suspects.Count == 0) && (subsetSuspectCollection.Suspects != null && subsetSuspectCollection.Suspects.Count > 0)))
            {
                return false;
            }
            else if(this.Suspects != null && subsetSuspectCollection.Suspects != null)
            {
                // Both Suspect collections will not be null
                foreach (MatchSuspect suspect in Suspects)
                {
                    if (!suspect.IsSuperSetOf(subsetSuspectCollection.Suspects))
                        return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Loads Match Suspect Collection from "SuspectCollection" XML node
        /// </summary>
        public void LoadFromXml(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);
                    Suspects = new Collection<MatchSuspect>();

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "SuspectCollection")
                        {
                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("QueryId"))
                                {
                                    this.QueryId = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), 0);
                                }
                                if (reader.MoveToAttribute("MatchType"))
                                {
                                    this.MatchType = reader.ReadContentAsString();
                                }
                            }
                            else
                            {
                                reader.Read();
                            }
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "Suspect")
                        {
                            MatchSuspect suspect = new MatchSuspect();
                            suspect.LoadFromXml(reader.ReadOuterXml());

                            Suspects.Add(suspect);
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "NativeResults")
                        {
                            this.NativeResults = reader.ReadElementContentAsString();
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
