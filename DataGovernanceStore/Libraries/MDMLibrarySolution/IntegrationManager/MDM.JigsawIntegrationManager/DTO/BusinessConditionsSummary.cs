using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MDM.JigsawIntegrationManager.DTO
{
  
    /// <summary>
    /// Represents JigsawBusinessConditionsSummary
    /// </summary>
    internal class BusinessConditionsSummary
    {
        /// <summary>
        /// 
        /// </summary>
        public ICollection<String> PassedBusinessConditions {get; set;}

        /// <summary>
        /// 
        /// </summary>
        public ICollection<String> FailedBusinessConditions {get; set;}

        /// <summary>
        /// 
        /// </summary>
        public ICollection<String> UnknownBusinessConditions { get; set; }

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