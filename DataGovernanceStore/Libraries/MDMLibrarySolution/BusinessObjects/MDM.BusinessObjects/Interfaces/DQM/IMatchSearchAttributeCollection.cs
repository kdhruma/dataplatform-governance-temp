using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDM.Interfaces.DQM
{
    /// <summary>
    /// Exposes properties and methods to set or get match search attribute collection
    /// </summary>
    interface IMatchSearchAttributeCollection
    {
        #region Properties
        #endregion Properties

        #region Methods

        /// <summary>
        /// Converts match search attribute collection into xml string
        /// </summary>
        /// <returns>Returns string xml of match search attribute collection</returns>
        String ToXml();

        /// <summary>
        /// Load match search attributes from xml
        /// </summary>
        /// <param name="valuesAsXml">Indicates string xml of match search attributes</param>
        void LoadMatchSearchAttributes(String valuesAsXml);

        /// <summary>
        /// Convert match search attribute collection into Json format
        /// </summary>
        /// <returns>Returns JObject of match search attibute collection</returns>
        JArray ToJson();

        #endregion Methods
    }
}
