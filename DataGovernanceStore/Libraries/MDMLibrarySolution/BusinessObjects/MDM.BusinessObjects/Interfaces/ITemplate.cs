using System;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get a template.
    /// </summary>
    public interface ITemplate : IMDMObject
    {
        #region Properties

        /// <summary>
        /// Property denoting File Type
        /// </summary>
        String FileType { get; set; }

        /// <summary>
        /// Property denoting FileData
        /// </summary>
        Byte[] FileData { get; set; }

        /// <summary>
        /// Property denoting Template Type
        /// </summary>
        TemplateType TemplateType { get; set; }

        #endregion
    }
}