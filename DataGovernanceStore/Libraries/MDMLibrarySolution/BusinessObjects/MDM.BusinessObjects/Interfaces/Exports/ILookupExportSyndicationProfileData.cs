using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDM.Interfaces.Exports
{
    using MDM.Core;
    using MDM.BusinessObjects;

    /// <summary>
    /// Exposes methods or properties to set or get the lookup export syndication profile related information.
    /// </summary>
    public interface ILookupExportSyndicationProfileData : IMDMObject
    {
        #region Properties

        #endregion

        #region Methods
        /// <summary>
        /// Represents lookup export syndication profiledata in Xml format
        /// </summary>
        /// <returns>String representation of current syndication exportprofiledata object</returns>
        String ToXml();

        /// <summary>
        /// Represents Lookup export syndication profiledata in Xml format
        /// </summary>
        /// <param name="objectSerialization">Specifies the object serialization type</param>
        /// <returns>String representation of current Lookup export syndication profiledata object</returns>
        String ToXml(ObjectSerialization objectSerialization);

        /// <summary>
        /// Set the execution specification for the current lookup export profile.
        /// </summary>
        /// <param name="executionSpecification">Indicates the execution specification object</param>
        void SetExecutionSpecification(IExecutionSpecification executionSpecification);

        /// <summary>
        /// Get the execution specification for the current lookup export profile.
        /// </summary>
        /// <returns>Returns Interface of execution specification object.</returns>
        IExecutionSpecification GetExecutionSpecification();

        /// <summary>
        /// Set the output specification for the current lookup export profile.
        /// </summary>
        /// <param name="outputSpecification">Indicates the output specification object</param>
        void SetOutputSpecification(IOutputSpecification outputSpecification);

        /// <summary>
        /// Get the output specification for the current lookup export profile.
        /// </summary>
        /// <returns>Returns Interface of output specification object.</returns>
        IOutputSpecification GetOutputSpecification();

        /// <summary>
        /// Set the notification details for the current lookup export profile.
        /// </summary>
        /// <param name="iNotification">Indicates the notification object</param>
        void SetNotification(INotification iNotification);

        /// <summary>
        /// Get the Notification details for the current lookup export profile.
        /// </summary>
        /// <returns>Returns Interface of Notification object.</returns>
        INotification GetNotification();

        /// <summary>
        /// Set the Profile details for the current lookup export profile.
        /// </summary>
        /// <param name="iProfileSettingCollection">Indicates the ProfileSettingCollection object</param>
        void SetProfileSettings(IProfileSettingCollection iProfileSettingCollection);

        /// <summary>
        /// Get the Profile details for the current lookup export profile.
        /// </summary>
        /// <returns>Returns Interface of ProfileSettings object.</returns>
        IProfileSettingCollection GetProfileSettings();

        /// <summary>
        /// Set the lookup export scopes for the current lookup export profile.
        /// </summary>
        /// <param name="iLookupExportScopes">Indicates the LookupExportScopeCollection object</param>
        void SetLookupExportScopes(ILookupExportScopeCollection iLookupExportScopes);

        /// <summary>
        /// Get the lookup export scopes for the current lookup export profile.
        /// </summary>
        /// <returns>Returns Interface of LookupExportScopeCollection object.</returns>
        ILookupExportScopeCollection GetLookupExportScopes();
        #endregion
    }
}
