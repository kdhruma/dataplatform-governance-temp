using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using MDM.Core;
using MDM.Interfaces;

namespace MDM.BusinessObjects
{
    /// <summary>
    /// Specifies an DoneReportItem
    /// </summary>
    [DataContract]
    public class DoneReportItem : MDMObject, IDoneReportItem
    {
        #region Fields

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public DoneReportItem()
            :base()
        {
            AttributesValues = new Dictionary<Int32, ValueCollection>();
            WorkflowInfo = new Collection<DoneReportItemWorkflowInfo>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Property for the id of an Entity
        /// </summary>
        [DataMember]
        public new Int64 Id { get; set; }

        /// <summary>
        /// Property denoting WorkflowFirstActivityTime
        /// </summary>
        [DataMember]
        public DateTime? WorkflowFirstActivityTime { get; set; }

        /// <summary>
        /// Property denoting WorkflowLastActivityTime
        /// </summary>
        [DataMember]
        public DateTime? WorkflowLastActivityTime { get; set; }

        /// <summary>
        /// Property denoting ParentId
        /// </summary>
        [DataMember]
        public Int64 ParentId { get; set; }

        /// <summary>
        /// Property denoting ParentName
        /// </summary>
        [DataMember]
        public String ParentName { get; set; }

        /// <summary>
        /// Property denoting ParentPath
        /// </summary>
        [DataMember]
        public String ParentPath { get; set; }

        /// <summary>
        /// Property denoting CatalogId
        /// </summary>
        [DataMember]
        public Int32 CatalogId { get; set; }

        /// <summary>
        /// Property denoting CatalogName
        /// </summary>
        [DataMember]
        public String CatalogName { get; set; }

        /// <summary>
        /// Property denoting CatalogLongName
        /// </summary>
        [DataMember]
        public String CatalogLongName { get; set; }

        /// <summary>
        /// Property denoting OrganizationId
        /// </summary>
        [DataMember]
        public Int32 OrganizationId { get; set; }

        /// <summary>
        /// Property denoting OrganizationName
        /// </summary>
        [DataMember]
        public String OrganizationName { get; set; }

        /// <summary>
        /// Property denoting OrganizationLongName
        /// </summary>
        [DataMember]
        public String OrganizationLongName { get; set; }

        /// <summary>
        /// Property denoting CategoryId
        /// </summary>
        [DataMember]
        public Int32 CategoryId { get; set; }

        /// <summary>
        /// Property denoting CategoryName
        /// </summary>
        [DataMember]
        public String CategoryName { get; set; }

        /// <summary>
        /// Property denoting CategoryLongName
        /// </summary>
        [DataMember]
        public String CategoryLongName { get; set; }

        /// <summary>
        /// Property denoting NodeTypeId
        /// </summary>
        [DataMember]
        public Int32 NodeTypeId { get; set; }

        /// <summary>
        /// Property denoting NodeTypeDescription
        /// </summary>
        [DataMember]
        public String NodeTypeDescription { get; set; }

        /// <summary>
        /// Property denoting AttributesValues
        /// </summary>
        [DataMember]
        public Dictionary<Int32, ValueCollection> AttributesValues { get; set; }

        /// <summary>
        /// Property denoting WorkflowInfo
        /// </summary>
        [DataMember]
        public Collection<DoneReportItemWorkflowInfo> WorkflowInfo { get; set; }

        #endregion
    }
}