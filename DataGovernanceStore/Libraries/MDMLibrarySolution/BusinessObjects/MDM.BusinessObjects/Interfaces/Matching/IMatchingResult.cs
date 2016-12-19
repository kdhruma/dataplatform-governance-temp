using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using MDM.BusinessObjects.DQM;
using MDM.BusinessObjects;

namespace MDM.Interfaces
{
    /// <summary>
    /// Exposes methods or properties to set or get the matching result object that will be created by the matching engine. 
    /// </summary>
    public interface IMatchingResult
    {
        #region Properties

        /// <summary>
        /// Indicates number of suspect records.
        /// </summary>
        Int32 SuspectCount { get; set; }

        /// <summary>
        /// Indicates Identifier of the result.
        /// </summary>
        Int64 Id { get; set; }

        /// <summary>
        /// Indicates Profile Id is used for the matching operation.
        /// </summary>
        Int32 ProfileId { get; set; }

        /// <summary>
        /// Indicates Identifer of the source entity object that was used for matching.
        /// </summary>
        Int64 SourceEntityId { get; set; }

        /// <summary>
        /// Indicates Instance of the Entity Object that was used for the matching operation.
        /// </summary>
        Entity SourceEntity { get; set; }

        /// <summary>
        ///  Indicates Collection of the suspect records that is returned by the matching engine. 
        /// </summary>
        MatchSuspectCollection SuspectCollection { get; set; }

        /// <summary>
        /// Indicates Query values that was used to perform the matching operation.
        /// </summary>
        MatchQueryData QueryData { get; set; }

        /// <summary>
        /// Indicates Job Identifer if the matching operation is done asynchronously using a job operation. 
        /// </summary>
        Int64 JobId { get; set; }

        /// <summary>
        /// Indicates Status of the matching operation. The status will start with the string, "Error" if there is any error otherwise it will be empty.
        /// </summary>
        String Status { get; set; }

        #endregion

        //#region Methods

        ///<summary>
        ///Returns the resutls in XML format.
        ///</summary>
        ///<returns></returns>
        string ToXml();

        ///// <summary>
        ///// Returns the results in JSON format.
        ///// </summary>
        ///// <returns></returns>
        //string ToJson();

        //#endregion
    }
  }
