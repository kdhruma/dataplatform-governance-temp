using ProtoBuf;
using System;
using System.Collections.Generic;
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
    public class MatchSuspectField : IEquatable<MatchSuspectField>
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        [ProtoMember(1)]
        public Int64 SourceAttributeId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        [ProtoMember(2)]
        public string Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        [ProtoMember(3)]
        public string Value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        [ProtoMember(4)]
        public double Score { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        [ProtoMember(5)]
        public Int64 TargetAttributeId { get; set; }

        /// <summary>
        /// Property denoting the high.
        /// </summary>
        [DataMember]
        [ProtoMember(6)]
        public Double High
        {
            get;
            set;
        }

        /// <summary>
        /// Property denoting the low.
        /// </summary>
        [DataMember]
        [ProtoMember(7)]
        public Double Low
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(MatchSuspectField other)
        {
            if (SourceAttributeId == 0)
                return Name.Equals(other.Name);

            return SourceAttributeId.Equals(other.SourceAttributeId);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gives XML representation of the object.
        /// </summary>
        /// <param name="valuesAsXml"></param>
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
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "MatchAttribute")
                        {
                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("Name"))
                                {
                                    this.Name = reader.ReadContentAsString();
                                }
                                if (reader.MoveToAttribute("Value"))
                                {
                                    this.Value = reader.ReadContentAsString();
                                }
                                if (reader.MoveToAttribute("Score"))
                                {
                                    this.Score = ValueTypeHelper.DoubleTryParse(reader.ReadContentAsString(), 0);
                                }
                                if (reader.MoveToAttribute("High"))
                                {
                                    this.High = ValueTypeHelper.DoubleTryParse(reader.ReadContentAsString(), 0);
                                }
                                if (reader.MoveToAttribute("Low"))
                                {
                                    this.Low = ValueTypeHelper.DoubleTryParse(reader.ReadContentAsString(), 0);
                                }
                                if (reader.MoveToAttribute("SourceAttributeId"))
                                {
                                    this.SourceAttributeId = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), 0);
                                }
                                if (reader.MoveToAttribute("TargetAttributeId"))
                                {
                                    this.TargetAttributeId = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), 0);
                                }
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
        /// Determines whether the current object instance is superset of the operation result passed as parameter
        /// </summary>
        /// <param name="collection">Indicates the subset object to compare with the current object</param>
        /// <param name="compareId">Indicates whether ids to be compared or not</param>
        /// <returns>Returns true if the current object is superset of the subset instances; otherwise false</returns>
        public Boolean IsSuperSetOf(System.Collections.ObjectModel.Collection<MatchSuspectField> collection, Boolean compareId = false)
        {
            foreach (MatchSuspectField suspectField in collection)
            {
                if (compareId)
                {
                    if (this.SourceAttributeId != suspectField.SourceAttributeId)
                    {
                        return false;
                    }

                    if (this.TargetAttributeId != suspectField.TargetAttributeId)
                    {
                        return false;
                    }
                }

                if (this.Name != suspectField.Name)
                {
                    return false;
                }

                if (this.Value != suspectField.Value)
                {
                    return false;
                }

                if (this.Score != suspectField.Score)
                {
                    return false;
                }

                if (this.High != suspectField.High)
                {
                    return false;
                }

                if (this.Low != suspectField.Low)
                {
                    return false;
                }

                return true;
            }

            return false;
        }

        #endregion
    }
}
