using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace MDM.JigsawIntegrationManager.DTO
{

    /// <summary>
    /// Represents JigsawValidationStatesSummary
    /// </summary>
    internal class ValidationStatesSummary
    {
        /// <summary>
        /// 
        /// </summary>
        public Double DataQualityScore{ get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ICollection<String> ValidStates { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ICollection<String> InValidStates { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ICollection<String> ErrorCodesForTypeError{get; set;}

        /// <summary>
        /// 
        /// </summary>
        public ICollection<String> ErrorCodesForTypeWarning{get; set;}

        /// <summary>
        /// 
        /// </summary>
        public ICollection<String> AttributesWithErrors{get; set;}

        /// <summary>
        /// 
        /// </summary>
        public ICollection<String> AttributesWithWarnings{get; set;}

        /// <summary>
        /// 
        /// </summary>
        public ICollection<String> ReasonTypes{get; set;}

        /// <summary>
        /// 
        /// </summary>
        public ICollection<String> AttributesErrorCodeList{get; set;}

    }
}