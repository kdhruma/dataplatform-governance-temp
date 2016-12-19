using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDM.Interfaces.DQM
{
    /// <summary>
    /// Exposes properties or methods to get or set match event
    /// </summary>
    public interface IMatchEvent
    {
        #region Properties

        /// <summary>
        /// Property denotes external identifier fields
        /// </summary>
        Collection<String> ExternalIdFields { get; set; }

        /// <summary>
        /// Property denotes extra relation fields
        /// </summary>
        Collection<String> ExtraRelationFields { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Converts match event into xml string
        /// </summary>
        /// <returns>Returns string xml of match event.</returns>
        String ToXml();

        /// <summary>
        /// Load match event from xml
        /// </summary>
        /// <param name="valuesAsXml">Indicates string xml of match event</param>
        void LoadMatchEvent(String valuesAsXml);

        /// <summary>
        /// Convert match event into Json format
        /// </summary>
        /// <returns>Returns JObject of match event</returns>
        JObject ToJson();

        #endregion Methods
    }
}
