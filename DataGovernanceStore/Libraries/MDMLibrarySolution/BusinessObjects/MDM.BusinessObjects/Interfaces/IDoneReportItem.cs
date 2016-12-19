using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MDM.BusinessObjects;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get "Done Report" item.
    /// </summary>
    public interface IDoneReportItem : IMDMObject
    {
        #region Properties

        /// <summary>
        /// Property denoting the Id of an entity
        /// </summary>
        new Int64 Id { get; set; }

        /// <summary>
        /// Property denoting WorkflowFirstActivityTime
        /// </summary>
        DateTime? WorkflowFirstActivityTime { get; set; }

        /// <summary>
        /// Property denoting WorkflowLastActivityTime
        /// </summary>
        DateTime? WorkflowLastActivityTime { get; set; }

        /// <summary>
        /// Property denoting ParentId
        /// </summary>
        Int64 ParentId { get; set; }

        /// <summary>
        /// Property denoting ParentName
        /// </summary>
        String ParentName { get; set; }

        /// <summary>
        /// Property denoting ParentPath
        /// </summary>
        String ParentPath { get; set; }

        /// <summary>
        /// Property denoting CatalogId
        /// </summary>
        Int32 CatalogId { get; set; }

        /// <summary>
        /// Property denoting CatalogName
        /// </summary>
        String CatalogName { get; set; }

        /// <summary>
        /// Property denoting CatalogLongName
        /// </summary>
        String CatalogLongName { get; set; }

        /// <summary>
        /// Property denoting OrganizationId
        /// </summary>
        Int32 OrganizationId { get; set; }

        /// <summary>
        /// Property denoting OrganizationName
        /// </summary>
        String OrganizationName { get; set; }

        /// <summary>
        /// Property denoting OrganizationLongName
        /// </summary>
        String OrganizationLongName { get; set; }

        /// <summary>
        /// Property denoting CategoryId
        /// </summary>
        Int32 CategoryId { get; set; }

        /// <summary>
        /// Property denoting CategoryName
        /// </summary>
        String CategoryName { get; set; }

        /// <summary>
        /// Property denoting CategoryLongName
        /// </summary>
        String CategoryLongName { get; set; }

        /// <summary>
        /// Property denoting NodeTypeId
        /// </summary>
        Int32 NodeTypeId { get; set; }

        /// <summary>
        /// Property denoting NodeTypeDescription
        /// </summary>
        String NodeTypeDescription { get; set; }

        /// <summary>
        /// Property denoting AttributesValues
        /// </summary>
        Dictionary<Int32, ValueCollection> AttributesValues { get; set; }

        /// <summary>
        /// Property denoting WorkflowInfo
        /// </summary>
        Collection<DoneReportItemWorkflowInfo> WorkflowInfo { get; set; }

        #endregion
    }
}