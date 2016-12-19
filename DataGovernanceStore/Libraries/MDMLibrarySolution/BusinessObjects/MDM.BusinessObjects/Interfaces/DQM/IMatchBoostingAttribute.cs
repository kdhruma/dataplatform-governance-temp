using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDM.BusinessObjects.Interfaces.DQM
{
    /// <summary>
    /// Exposes methods or properties to get or set the match boosting attribute
    /// </summary>
    public interface IMatchBoostingAttribute
    {
        #region Properties

        /// <summary>
        /// Indicates low match score of attribute
        /// </summary>
        Double Low { get; set; }

        /// <summary>
        /// Indicates high match score of attribute
        /// </summary>
        Double High { get; set; }

        /// <summary>
        /// Indicates jigsaw comparator for match
        /// </summary>
        String Comparator { get; set; }

        #endregion Properties

        #region Method

        /// <summary>
        /// Converts match boosting attribute into xml string
        /// </summary>
        /// <returns>Returns string xml of match boosting attribute.</returns>
        String ToXml();

        /// <summary>
        /// Load match boosting attribute from xml
        /// </summary>
        /// <param name="valuesAsXml">Indicates string xml of match boosting attribute</param>
        void LoadMatchBoostingAttribute(String valuesAsXml);

        /// <summary>
        /// Convert match boosting attribute into Json format
        /// </summary>
        /// <returns>Returns JObject of MatchBoostingAttribute</returns>
        JObject ToJson();

        #endregion Method
    }
}
