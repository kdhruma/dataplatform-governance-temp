using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDM.Interfaces
{
    interface IMatchingService
    {
        /// <summary>
        /// This is main the method that should be called to run the Match.
        /// The query returns immediately with the queryId. Set this QueryId in the
        /// Context. 
        /// </summary>
        /// <param name="context"></param>
        /// <returns>QueryId</returns>
        Int64 Match(IMatchingContext context);

        /// <summary>
        /// This will return the status of the Query Job. If the query is started from the UI,
        /// it can wait on a separate thread to get the Status. 
        /// </summary>
        /// <param name="context"></param>
        /// <returns>MatchStatus</returns>
        MatchStatus GetStatus(IMatchingContext context);

        /// <summary>
        /// This method return Errors for a Match Job.
        /// If the QueryId has to be present to get the Error. 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        string GetError(IMatchingContext context);
    }


    /// <summary>
    /// Represents the different state of the matching operation. 
    /// </summary>
    public enum MatchStatus
    {
        /// <summary>
        /// Match operation is completed.
        /// </summary>
        MatchCompleted,
        /// <summary>
        /// Match operation is still executing. 
        /// </summary>
        MatchRunning,
        /// <summary>
        /// Match operation completed but it produced some errors. 
        /// </summary>
        MatchCompletedWithErrors,
        /// <summary>
        /// Match operation is started. 
        /// </summary>
        MatchStarted
    }
}
