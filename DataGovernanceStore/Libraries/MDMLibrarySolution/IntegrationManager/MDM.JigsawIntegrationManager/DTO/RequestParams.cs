using System;
using System.Collections.Generic;

namespace MDM.JigsawIntegrationManager.DTO
{
    using MDM.Core;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Represents class for RequestParams.
    /// </summary>
    public class RequestParams
    {
        /// <summary>
        /// Gets or sets the type of the entity.
        /// </summary>
        public Dictionary<String, Object> Params { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public RequestParams()
        {
            Params = new Dictionary<String, Object>();
        }
    }
}
