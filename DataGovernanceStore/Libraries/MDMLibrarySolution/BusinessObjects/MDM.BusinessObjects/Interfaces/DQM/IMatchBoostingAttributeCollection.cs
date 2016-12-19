using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDM.Interfaces.DQM
{
    /// <summary>
    /// Exposes properties and methods to set or get match boosting attribute collection
    /// </summary>
    interface IMatchBoostingAttributeCollection
    {
        #region Properties
        #endregion Properties

        #region Methods

        /// <summary>
        /// Converts match boosting attribute collection into xml string
        /// </summary>
        /// <returns>Returns string xml of match boosting attribute collection</returns>
        String ToXml();

        /// <summary>
        /// Load match boosting attributes from xml
        /// </summary>
        /// <param name="valuesAsXml">Indicates string xml of match boosting attributes</param>
        void LoadMatchBoostingAttributes(String valuesAsXml);

        /// <summary>
        /// Convert match boosting attribute collection into Json format
        /// </summary>
        /// <returns>Returnds JObject of match boosting attribute collection</returns>
        JArray ToJson();

        #endregion Methods
    }
}
