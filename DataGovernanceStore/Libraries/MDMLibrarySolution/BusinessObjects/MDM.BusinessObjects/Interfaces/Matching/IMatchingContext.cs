using MDM.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace MDM.Interfaces
{
    /// <summary>
    /// Exposes methods or properties to set or get the context for matching operations.
    /// </summary>
    public interface IMatchingContext
    {
        /// <summary>
        /// Indicates the unique identifer of the query.
        /// </summary>
        Int64 QueryId { get; set; }

        /// <summary>
        /// This property is set to true if batching has to be enabled.
        /// There will be a group Id that will help get all the information to run the 
        /// sources in parallel.
        /// </summary>
        bool IsBatched { get; set; }

        /// <summary>
        /// This Property defines if the Query is scheduled on a certain date or time.
        /// </summary>
        bool IsQueryScheduled { get; set; }

        /// <summary>
        /// This will have the Query that needs to be processed and run. 
        /// The format of the query for now is in XML format but we will move to
        /// JSON soon.
        /// </summary>
        string MatchQuery { get; set; }


        /// <summary>
        /// This pjrope
        /// </summary>
        DateTime ScheduledTime { get; set; }

        /// <summary>
        /// This property holds the type of the Matching Engine
        /// The default RTI matching engine is Tibco Patterns Engine
        /// </summary>
        MatchingEngineType EngineType { get; set; }

        /// <summary>
        /// This property value defines the list of batches that
        /// needs to run in batches. 
        /// </summary>
        Int32 BatchGroupId { get; set; }

        /// <summary>
        /// This property value defines the list entity save options
        /// </summary>
        SaveAttributeValuesType SaveAttributeValues { get; set; }

        /// <summary>
        /// This property is applicable when the query is batched so that only Query Parameter Values
        /// can be swapped.
        /// </summary>
       Dictionary<string, string> QueryParameterValues { get; set; }

        /// <summary>
        /// This property tells the engine that they should
        /// save the results returned by the 3rd party engine with all the
        /// available properties of each result row and field.
        /// </summary>
        bool SaveNativeResults { get; set; }
    }
}
