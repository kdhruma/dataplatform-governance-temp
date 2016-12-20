using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects;
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get audit information collection.
    /// </summary>
    public interface IAuditInfoCollection : IEnumerable<AuditInfo>
    {
        #region Properties

        #endregion

        #region Methods

        /// <summary>
        /// Get Xml representation of AuditInfoCollection object
        /// </summary>
        /// <returns>Xml string representing the AuditInfoCollection</returns>     
        String ToXml();

        /// <summary>
        /// Get Xml representation of AuditInfo object
        /// </summary>
        /// <param name="objectSerialization">serialization option. Based on the value selected, the different xml representation will be there</param>
        /// <returns>Xml representation of object</returns>
        String ToXml(ObjectSerialization objectSerialization);

        /// <summary>
        /// Get AuditInfoCollection By UserLogin 
        /// </summary>
        /// <param name="userLogin">User Login</param>
        /// <returns>AuditInfoCollection</returns>
        AuditInfoCollection GetByUser(String userLogin);

        /// <summary>
        /// Get AuditInfoCollection By Action
        /// </summary>
        /// <param name="action">Action</param>
        /// <returns>AuditInfoCollection</returns>
        AuditInfoCollection GetByAction(ObjectAction action);

        /// <summary>
        /// Get AuditInfoCollection By Locale
        /// </summary>
        /// <param name="locale">Locale</param>
        /// <returns>AuditInfoCollection</returns>
        AuditInfoCollection GetByLocale(LocaleEnum locale);

        /// <summary>
        /// Get Latest AuditInfo
        /// </summary>
        /// <returns>AuditInfo</returns>
        AuditInfo GetLatest();

        /// <summary>
        /// If modification has been done after given datetime or not
        /// </summary>
        /// <param name="dateTime">DateTime</param>
        /// <returns>True If modification has been done before given datetime</returns>
        Boolean HasModifiedAfter(DateTime dateTime);

        /// <summary>
        /// If modification has been done before given datetime or not
        /// </summary>
        /// <param name="dateTime">DateTime</param>
        /// <returns>True if modification has been done before given datetime</returns>
        Boolean HasModifiedBefore(DateTime dateTime);

        /// <summary>
        /// Returns wheather collection has been modified or not.
        /// </summary>
        /// <returns>True if Modification has been done</returns>
        Boolean HasModification();

        #endregion  
    }
}
