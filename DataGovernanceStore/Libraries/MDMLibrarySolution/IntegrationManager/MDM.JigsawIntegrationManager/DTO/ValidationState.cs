using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace MDM.JigsawIntegrationManager.DTO
{
    
    /// <summary>
    /// Represents JigsawValidationState
    /// </summary>
    internal class ValidationState
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public Boolean Value { get; set; }
    }
}