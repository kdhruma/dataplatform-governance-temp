using MDM.BusinessObjects.DQM;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDM.Interfaces.DQM
{
    /// <summary>
    /// Exposes properties or methods to get or set the match rule
    /// </summary>
    public interface IMatchRule
    {
        #region Fields

        /// <summary>
        /// Property denotes ---
        /// </summary>
        Boolean UseAndTemplate { get; set; }

        /// <summary>
        /// Property denotes ---
        /// </summary>
        Double Threshold { get; set; }

        /// <summary>
        /// Property denotes ---
        /// </summary>
        Double MayBeThreshold { get; set; }

        /// <summary>
        /// Property denotes collection of match search attribute
        /// </summary>
        MatchSearchAttributeCollection MatchSearchAttributes { get; set; }

        /// <summary>
        /// Property denotes collection of match boosting attribute
        /// </summary>
        MatchBoostingAttributeCollection MatchBoostingAttributes { get; set; }

        #endregion Fields

        #region Methods

        /// <summary>
        /// Convert match rule into Json
        /// </summary>
        /// <returns></returns>
        JObject ToJson();

        #endregion Methods
    }
}
