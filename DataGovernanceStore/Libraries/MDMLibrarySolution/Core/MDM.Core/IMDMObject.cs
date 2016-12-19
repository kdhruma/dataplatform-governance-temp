using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDM.Core
{
    /// <summary>
    /// Interface for MDMObject 
    /// </summary>
    public interface IMDMObject
    {
        #region Properties

        /// <summary>
        /// Indicates the Id of an object
        /// </summary>
        Int32 Id { get; set; }

        /// <summary>
        /// Indicates the Name of an object. Name often refers to the ShortName
        /// </summary>
        String Name { get; set; }

        /// <summary>
        /// Indicates the Long Name of an object
        /// </summary>
        String LongName { get; set; }

        /// <summary>
        /// Indicates the Locale of an object
        /// </summary>
        LocaleEnum Locale { get; set; }

        /// <summary>
        /// Indicates the Action of an object
        /// </summary>
        ObjectAction Action { get; set; }

        /// <summary>
        /// Indicates the auditRefId of an object
        /// </summary>
        Int64 AuditRefId { get; set; }

        /// <summary>
        /// Indicates the ProgramName of an object
        /// </summary>
        String ProgramName { get; set; }

        /// <summary>
        /// Indicates the UserName for an object who changed it
        /// </summary>
        String UserName { get; set; }

        /// <summary>
        /// Indicates the reference Id
        /// </summary>
        String ReferenceId { get; set; }

        #endregion 

        #region Methods

        #endregion
    }
}
