using System;
using System.Runtime.Serialization;

namespace MDM.BusinessObjects.DQM
{
    using MDM.Interfaces;

    /// <summary>
    /// Specifies Normalization Report Item
    /// </summary>
    [DataContract]
    public class NormalizationReportItem : NormalizationResult, INormalizationReportItem, ICloneable
    {
        #region Fields

        /// <summary>
        /// Field denoting entity short name
        /// </summary>
        private String _entityShortName;

        /// <summary>
        /// Field denoting entity long name
        /// </summary>
        private String _entityLongName;

        /// <summary>
        /// Field denoting normalization rule short name
        /// </summary>
        private String _ruleShortName;

        /// <summary>
        /// Field denoting normalization rule long name
        /// </summary>
        private String _ruleLongName;

        /// <summary>
        /// Field denoting attribute short name
        /// </summary>
        private String _attributeShortName;

        /// <summary>
        /// Field denoting attribute long name
        /// </summary>
        private String _attributeLongName;

        /// <summary>
        /// Field denoting organization short name
        /// </summary>
        private String _organizationShortName;

        /// <summary>
        /// Field denoting organization long name
        /// </summary>
        private String _organizationLongName;

        /// <summary>
        /// Field denoting container short name
        /// </summary>
        private String _containerShortName;

        /// <summary>
        /// Field denoting container long name
        /// </summary>
        private String _containerLongName;

        /// <summary>
        /// Field denoting entity type short name
        /// </summary>
        private String _entityTypeShortName;

        /// <summary>
        /// Field denoting entity type long name
        /// </summary>
        private String _entityTypeLongName;

        /// <summary>
        /// Field denoting category short name
        /// </summary>
        private String _categoryShortName;

        /// <summary>
        /// Field denoting category long name
        /// </summary>
        private String _categoryLongName;

        /// <summary>
        /// Field denoting the Organization Id of a Container
        /// </summary>
        private Int32 _organizationId;

        /// <summary>
        /// Field denoting the Container Id of an entity
        /// </summary>
        private Int32 _сontainerId;

        /// <summary>
        /// Field denoting the Category Id of an entity
        /// </summary>
        private Int64 _сategoryId;

        /// <summary>
        /// Field denoting the Type Id of an entity
        /// </summary>
        private Int32 _entityTypeId;

        #endregion

        #region Properties

        /// <summary>
        /// Property denoting entity short name
        /// </summary>
        [DataMember]
        public String EntityShortName
        {
            get { return _entityShortName; }
            set { _entityShortName = value; }
        }

        /// <summary>
        /// Property denoting entity long name
        /// </summary>
        [DataMember]
        public String EntityLongName
        {
            get { return _entityLongName; }
            set { _entityLongName = value; }
        }

        /// <summary>
        /// Property denoting normalization rule short name
        /// </summary>
        [DataMember]
        public String RuleShortName
        {
            get { return _ruleShortName; }
            set { _ruleShortName = value; }
        }

        /// <summary>
        /// Property denoting normalization rule long name
        /// </summary>
        [DataMember]
        public String RuleLongName
        {
            get { return _ruleLongName; }
            set { _ruleLongName = value; }
        }

        /// <summary>
        /// Property denoting attribute short name
        /// </summary>
        [DataMember]
        public String AttributeShortName
        {
            get { return _attributeShortName; }
            set { _attributeShortName = value; }
        }

        /// <summary>
        /// Property denoting attribute long name
        /// </summary>
        [DataMember]
        public String AttributeLongName
        {
            get { return _attributeLongName; }
            set { _attributeLongName = value; }
        }

        /// <summary>
        /// Property denoting organization short name
        /// </summary>
        [DataMember]
        public String OrganizationShortName
        {
            get { return _organizationShortName; }
            set { _organizationShortName = value; }
        }

        /// <summary>
        /// Property denoting organization long name
        /// </summary>
        [DataMember]
        public String OrganizationLongName
        {
            get { return _organizationLongName; }
            set { _organizationLongName = value; }
        }

        /// <summary>
        /// Property denoting container short name
        /// </summary>
        [DataMember]
        public String ContainerShortName
        {
            get { return _containerShortName; }
            set { _containerShortName = value; }
        }

        /// <summary>
        /// Property denoting container long name
        /// </summary>
        [DataMember]
        public String ContainerLongName
        {
            get { return _containerLongName; }
            set { _containerLongName = value; }
        }

        /// <summary>
        /// Property denoting entity type short name
        /// </summary>
        [DataMember]
        public String EntityTypeShortName
        {
            get { return _entityTypeShortName; }
            set { _entityTypeShortName = value; }
        }

        /// <summary>
        /// Property denoting entity type long name
        /// </summary>
        [DataMember]
        public String EntityTypeLongName
        {
            get { return _entityTypeLongName; }
            set { _entityTypeLongName = value; }
        }

        /// <summary>
        /// Property denoting category short name
        /// </summary>
        [DataMember]
        public String CategoryShortName
        {
            get { return _categoryShortName; }
            set { _categoryShortName = value; }
        }

        /// <summary>
        /// Property denoting category long name
        /// </summary>
        [DataMember]
        public String CategoryLongName
        {
            get { return _categoryLongName; }
            set { _categoryLongName = value; }
        }

        /// <summary>
        /// Property denoting the Organization Id of a Container
        /// </summary>
        [DataMember]
        public Int32 OrganizationId
        {
            get { return _organizationId; }
            set { _organizationId = value; }
        }

        /// <summary>
        /// Property denoting the Container Id of an entity
        /// </summary>
        [DataMember]
        public Int32 ContainerId
        {
            get { return _сontainerId; }
            set { _сontainerId = value; }
        }

        /// <summary>
        /// Property denoting the Category Id of an entity
        /// </summary>
        [DataMember]
        public Int64 CategoryId
        {
            get { return _сategoryId; }
            set { _сategoryId = value; }
        }

        /// <summary>
        /// Property denoting the Type Id of an entity
        /// </summary>
        [DataMember]
        public Int32 EntityTypeId
        {
            get { return _entityTypeId; }
            set { _entityTypeId = value; }
        }

        #endregion

        #region ICloneable Members

        /// <summary>
        /// Creates a clone copy of normalization report item
        /// </summary>
        /// <returns>Returns a clone copy of normalization report item</returns>
        public new object Clone()
        {
            NormalizationReportItem result = (NormalizationReportItem) this.MemberwiseClone();
            return result;
        }

        #endregion
    }
}