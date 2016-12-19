using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml;
using System.Collections.ObjectModel;
using System.Text;

namespace MDM.BusinessObjects.DQM
{
    using MDM.Interfaces;

    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class MatchingResult : IMatchingResult, ICloneable
    {
        #region Fields

        MatchQueryData _matchQueryData = new MatchQueryData();

        #endregion

        #region Constructor

        /// <summary>
        /// Parameterless Constructor 
        /// </summary>
        public MatchingResult()
        {
        }

        /// <summary>
        /// Constructor which takes MatchingResult as input paramater
        /// </summary>
        /// <param name="res">MatchingResult</param>
        public MatchingResult(MatchingResult res)
        {
            QueryId = res.QueryId;
            SuspectCount = res.SuspectCount;
            Id = res.Id;
            JobId = res.JobId;
            ProfileId = res.ProfileId;
            SourceEntityId = res.SourceEntityId;
            SuspectCollection = res.SuspectCollection;
            QueryData = res.QueryData;
            Status = res.Status;
            IsHistoryRecord = res.IsHistoryRecord;
        }

        /// <summary>
        /// Constructor with xml as input parameter
        /// </summary>
        /// <param name="valuesAsXml">Indicates the values to load from xml for MatchingResult</param>     
        public MatchingResult(String valuesAsXml)
        {
            LoadFromXml(valuesAsXml);
        }

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public Int64 QueryId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public Int32 SuspectCount { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public Int64 Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public Int32 ProfileId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public Int64 SourceEntityId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public Entity SourceEntity { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public MatchSuspectCollection SuspectCollection { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public MatchQueryData QueryData
        {
            get { return this._matchQueryData; }
            set { this._matchQueryData = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public Int64 JobId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public String Status { get; set; }

        /// <summary>
        /// Indicates if result is not current and its fetched from history tables.
        /// </summary>
        [DataMember]
        public Boolean IsHistoryRecord { get; set; }

        #endregion

        #region Public Method

        /// <summary>
        /// Determines whether the current object instance is superset of the operation result passed as parameter
        /// </summary>
        /// <param name="subsetMatchingResult">Indicates the subset object to compare with the current object</param>
        /// <param name="compareIds">Indicates whether ids to be compared or not</param>
        /// <returns>Returns true if the current object is superset of the subset instances; otherwise false</returns>
        public Boolean IsSuperSetOf(MatchingResult subsetMatchingResult, Boolean compareIds)
        {
            if (compareIds)
            {
                if (this.Id != subsetMatchingResult.Id)
                    return false;

                if (this.QueryId != subsetMatchingResult.QueryId)
                    return false;

                if (this.ProfileId != subsetMatchingResult.ProfileId)
                    return false;

                if (this.SourceEntityId != subsetMatchingResult.SourceEntityId)
                    return false;

                if (this.JobId != subsetMatchingResult.JobId)
                    return false;
            }

            if (this.SuspectCount != subsetMatchingResult.SuspectCount)
                return false;

            if (!this.SuspectCollection.IsSuperSetOf(subsetMatchingResult.SuspectCollection))
                return false;

            if (!this.QueryData.IsSuperSetOf(subsetMatchingResult.QueryData))
                return false;

            if (this.Status != subsetMatchingResult.Status)
                return false;

            if (this.IsHistoryRecord != subsetMatchingResult.IsHistoryRecord)
                return false;

            return true;
        }

        /// <summary>
        /// Gives XML representation of the object.
        /// </summary>
        /// <returns></returns>
        public void LoadFromXml(string valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;

                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "MatchResult")
                        {
                            String suspectCollection = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(suspectCollection))
                            {
                                MatchSuspectCollection matchSuspectCollection = new MatchSuspectCollection();
                                matchSuspectCollection.LoadFromXml(suspectCollection);

                                this.SuspectCollection = matchSuspectCollection;
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
        /// Returns XML representation of the object.
        /// </summary>
        /// <returns></returns>
        public virtual string ToXml()
        {
            var returnBuffer = new StringBuilder();

            returnBuffer.AppendLine("<MatchResult>");
            returnBuffer.AppendFormat("<SuspectCollection QueryId=\"{0}\">", QueryId);

            if (SuspectCollection != null)
            {
                if (SuspectCollection.Suspects != null)
                {
                    foreach (var suspect in SuspectCollection.Suspects)
                    {
                        returnBuffer.AppendFormat("<Suspect Score=\"{0}\" ReferenceId=\"{1}\">", suspect.Score, suspect.TargetEntityId);
                        returnBuffer.AppendLine(Environment.NewLine);
                        returnBuffer.Append("<MatchAttributes>");
                        foreach (var mattr in suspect.SuspectFields)
                        {
                            returnBuffer.AppendFormat("<MatchAttribute Name=\"{0}\" Value=\"{1}\" Score=\"{2}\"/>", mattr.Name, mattr.Value, mattr.Score);

                        }
                        returnBuffer.Append("</MatchAttributes>");
                        returnBuffer.AppendLine(Environment.NewLine);
                        returnBuffer.AppendLine("</Suspect>");
                    }
                }

                returnBuffer.AppendFormat("<NativeResults><![CDATA[{0}]]></NativeResults>", SuspectCollection.NativeResults);
                returnBuffer.AppendLine(Environment.NewLine);
            }

            returnBuffer.AppendLine("</SuspectCollection>");

            returnBuffer.AppendLine("</MatchResult>");

            return returnBuffer.ToString();
        }

        #endregion

        /// <summary>
        /// Implementing ICloneable Interface
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            MatchingResult clonedProfile = new MatchingResult(this);
            return clonedProfile;
        }
    }
}
