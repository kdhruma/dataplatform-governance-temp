using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MDM.BusinessObjects.DQM
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    [DataContract]
    [ProtoContract]
    public class MatchQueryData
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        [ProtoMember(1)]
        public Int64 EntityId;

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        [ProtoMember(2)]
        public Dictionary<string, string> QueryParameterValues;

        #endregion

        #region Methods

        /// <summary>
        /// Determines whether the current object instance is superset of the operation result passed as parameter
        /// </summary>
        /// <param name="queryData">Indicates the subset object to compare with the current object</param>
        /// <param name="compareId">Indicates whether ids to be compared or not</param>
        /// <returns>Returns true if the current object is superset of the subset instances; otherwise false</returns>
        public Boolean IsSuperSetOf(MatchQueryData queryData, Boolean compareId = false)
        {
            if (compareId)
                if (this.EntityId != queryData.EntityId)
                    return false;

            if (this.QueryParameterValues != queryData.QueryParameterValues)
                return false;

            return true;
        }

        #endregion
    }
}
