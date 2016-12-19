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
    public class MatchSuspect
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        [ProtoMember(1)]
        public double Score { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        [ProtoMember(2)]
        public Int64 TargetEntityId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        [ProtoMember(3)]
        public Collection<MatchSuspectField> SuspectFields { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        [ProtoMember(4)]
        public String TargetEntityLongName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        [ProtoMember(5)]
        public double NativeScore { get; set; }

        /// <summary>
        /// Property denoting the type of the match.
        /// </summary>
        [DataMember]
        [ProtoMember(6)]
        public String MatchType
        {
            get;
            set;
        }

        /// <summary>
        /// Property denoting the exactly matched attributes.
        /// </summary>
        [DataMember]
        [ProtoMember(7)]
        public Collection<String> ExactlyMatchedAttributes
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool ScoreEquals(MatchSuspect other)
        {
            return Score.Equals(other.Score);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gives XML representation of the object.
        /// </summary>
        public void LoadFromXml(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "Suspect")
                        {
                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("Score"))
                                {
                                    this.Score = ValueTypeHelper.DoubleTryParse(reader.ReadContentAsString(), 0);
                                }
                                if (reader.MoveToAttribute("NativeScore"))
                                {
                                    this.NativeScore = ValueTypeHelper.DoubleTryParse(reader.ReadContentAsString(), 0);
                                }
                                if (reader.MoveToAttribute("TargetEntityId"))
                                {
                                    this.TargetEntityId = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), 0);
                                }
                                if (reader.MoveToAttribute("TargetEntityLongName"))
                                {
                                    this.TargetEntityLongName = reader.ReadContentAsString();
                                }
                                if (reader.MoveToAttribute("MatchType"))
                                {
                                    this.MatchType = reader.ReadContentAsString();
                                }
                                if (reader.MoveToAttribute("ExactlyMatchedAttributes"))
                                {
                                    this.ExactlyMatchedAttributes = ValueTypeHelper.SplitStringToStringCollection(reader.ReadContentAsString(), ',');
                                }
                            }
                            else
                            {
                                reader.Read();
                            }
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "MatchAttributes")
                        {
                            MatchSuspectField matchAttributes = new MatchSuspectField();
                            matchAttributes.LoadFromXml(reader.ReadOuterXml());

                            SuspectFields = new Collection<MatchSuspectField>();
                            SuspectFields.Add(matchAttributes);
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
        /// Determines whether the current object instance is superset of the operation result passed as parameter
        /// </summary>
        /// <param name="suspects">Indicates the subset object to compare with the current object</param>
        /// <param name="compareId">Indicates whether ids to be compared or not</param>
        /// <returns>Returns true if the current object is superset of the subset instances; otherwise false</returns>
        public Boolean IsSuperSetOf(Collection<MatchSuspect> suspects, Boolean compareId = false)
        {
            foreach (MatchSuspect suspect in suspects)
            {
                if (compareId)
                {
                    if (this.TargetEntityId != suspect.TargetEntityId)
                    {
                        return false;
                    }
                }

                if (this.Score != suspect.Score)
                {
                    return false;
                }

                if (this.TargetEntityLongName != suspect.TargetEntityLongName)
                {
                    return false;
                }

                if (this.NativeScore != suspect.NativeScore)
                {
                    return false;
                }

                if (this.MatchType != suspect.MatchType)
                {
                    return false;
                }

                foreach (MatchSuspectField suspectField in SuspectFields)
                {
                    if (!suspectField.IsSuperSetOf(suspect.SuspectFields))
                    {
                        return false;
                    }
                }
                
                return true;
            }

            return false;
        }

        #endregion
    }
}
