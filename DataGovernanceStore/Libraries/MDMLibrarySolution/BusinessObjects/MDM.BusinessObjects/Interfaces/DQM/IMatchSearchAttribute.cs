using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDM.Interfaces.DQM
{
    /// <summary>
    /// Exposes methods or properties to set or get the match search attribute
    /// </summary>
    public interface IMatchSearchAttribute
    {
        #region Properties

        /// <summary>
        /// Indicates extension of match
        /// </summary>
        String Extension { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Converts match search attribute into xml string
        /// </summary>
        /// <returns>Returns string xml of match search attribute.</returns>
        String ToXml();

        /// <summary>
        /// Load match search attribute from xml
        /// </summary>
        /// <param name="valuesAsXml">Indicates string xml of match search attribute</param>
        void LoadMatchSearchAttribute(String valuesAsXml);

        /// <summary>
        /// Converts MatchSearchAttribute into Json format
        /// </summary>
        /// <returns>Returns JObject of match search attribute</returns>
        JObject ToJson();

        #endregion Methods

    }
}
