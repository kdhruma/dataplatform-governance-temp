using System;
using MDM.BusinessObjects.Exports;
using MDM.Core;

namespace MDM.Interfaces.Exports
{
    /// <summary>
    /// Exposes methods or properties to set or get the syndication export profile related information.
    /// </summary>
    public interface IEntityExportSyndicationProfileData : IMDMObject
    {
        #region Properties

        /// <summary>
        /// Property specifying syndication export profile data ruleset id
        /// </summary>
        Int32 RuleSetId { get; set; }

        ///// <summary>
        ///// Property specifying collection of profile settings
        ///// </summary>
        //ProfileSettingCollection ProfileSettings { get; set; }

        ///// <summary>
        ///// Property specifying notification
        ///// </summary>
        //Notification Notification { get; set; }

        ///// <summary>
        ///// Property specifying scopespecification
        ///// </summary>
        //ScopeSpecification ScopeSpecification { get; set; }

        ///// <summary>
        ///// Property specifying outputspecification
        ///// </summary>
        //OutputSpecification OutputSpecification { get; set; }

        ///// <summary>
        ///// Property specifying executionspecification
        ///// </summary>
        //ExecutionSpecification ExecutionSpecification { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Represents syndication ExportProfileData in Xml format
        /// </summary>
        /// <returns>Syndication exportProfileData in Xml format</returns>
        String ToXml();

        /// <summary>
        /// Represents syndication ExportProfileData in Xml format based on requested object serialization
        /// </summary>
        /// <param name="objectSerialization">Type of the serialization option</param>
        /// <returns>Xml representation of syndicationExportProfileData</returns>
        String ToXml(ObjectSerialization objectSerialization);

        #endregion Methods
    }
}
