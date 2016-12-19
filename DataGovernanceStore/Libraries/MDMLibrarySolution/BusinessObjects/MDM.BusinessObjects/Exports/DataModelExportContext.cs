using System;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace MDM.BusinessObjects.Exports
{
    using MDM.Core;
    using MDM.Core.DataModel;
    using MDM.Interfaces.Exports;

    /// <summary>
    /// Specifies the Export Context
    /// </summary>
    [DataContract]
    [KnownType(typeof(MDMObject))]
    public class DataModelExportContext : MDMObject, IDataModelExportContext
    {
        #region Fields

        /// <summary>
        /// Field denoting the organization id list
        /// </summary>
        private Collection<Int32> _organizationIdList;

        /// <summary>
        /// Field denoting the container id list
        /// </summary>
        private Collection<Int32> _containerIdList;

        /// <summary>
        /// Field denoting the hierarchy id list
        /// </summary>
        private Collection<Int32> _hierarchyIdList;

        /// <summary>
        /// Field denoting the entity type id list
        /// </summary>
        private Collection<Int32> _entityTypeIdList;

        /// <summary>
        /// Field denoting the relationship type id list
        /// </summary>
        private Collection<Int32> _relationshipTypeIdList;

        /// <summary>
        /// Field denoting the lookup table name list
        /// </summary>
        private Collection<String> _lookupTableNames;

        /// <summary>
        /// Field denoting the sheet names
        /// </summary>
        private Collection<DataModelSheet> _sheetNames;

        /// <summary>
        /// Field denoting locale list
        /// </summary>
        private Collection<LocaleEnum> _locales;

        /// <summary>
        /// Field denoting whether to exclude non translated model data or not
        /// </summary>
        private Boolean _excludeNonTranslatedModelData = false;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Represents the parameter less default Constructor
        /// </summary>
        public DataModelExportContext()
            : base()
        {
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Collection of organization ids that should be used during export
        /// </summary>
        [DataMember]
        public Collection<Int32> OrganizationIdList
        {
            get
            {
                return _organizationIdList ?? (_organizationIdList = new Collection<Int32>());
            }
            set
            {
                _organizationIdList = value;
            }
        }

        /// <summary>
        /// Collection of container ids that should be used during export
        /// </summary>
        [DataMember]
        public Collection<Int32> ContainerIdList
        {
            get
            {
                return _containerIdList ?? (_containerIdList = new Collection<Int32>());
            }
            set
            {
                _containerIdList = value;
            }
        }

        /// <summary>
        /// Collection of hierarchy ids that should be used during export
        /// </summary>
        [DataMember]
        public Collection<Int32> HierarchyIdList
        {
            get
            {
                return _hierarchyIdList ?? (_hierarchyIdList = new Collection<Int32>());
            }
            set
            {
                _hierarchyIdList = value;
            }
        }

        /// <summary>
        /// Collection of entity type ids that should be used during export
        /// </summary>
        [DataMember]
        public Collection<Int32> EntityTypeIdList
        {
            get
            {
                return _entityTypeIdList ?? (_entityTypeIdList = new Collection<Int32>());
            }
            set
            {
                _entityTypeIdList = value;
            }
        }

        /// <summary>
        /// Collection of relationship type ids that should be used during export
        /// </summary>
        [DataMember]
        public Collection<Int32> RelationshipTypeIdList
        {
            get
            {
                return _relationshipTypeIdList ?? (_relationshipTypeIdList = new Collection<Int32>());
            }
            set
            {
                _relationshipTypeIdList = value;
            }
        }

        /// <summary>
        /// Collection of lookup table names that should be used during export
        /// </summary>
        [DataMember]
        public Collection<String> LookupTableNames
        {
            get
            {
                return _lookupTableNames ?? (_lookupTableNames = new Collection<String>());
            }
            set
            {
                _lookupTableNames = value;
            }
        }

        /// <summary>
        /// Collection of sheet names that needs to be export
        /// </summary>
        [DataMember]
        public Collection<DataModelSheet> SheetNames
        {
            get
            {
                return _sheetNames ?? (_sheetNames = new Collection<DataModelSheet>());
            }
            set
            {
                _sheetNames = value;
            }
        }

        /// <summary>
        /// Collection of locales in which data model needs to export
        /// </summary>
        [DataMember]
        public Collection<LocaleEnum> Locales
        {
            get
            {
                return _locales ?? (_locales = new Collection<LocaleEnum>());
            }
            set
            {
                _locales = value;
            }
        }

        /// <summary>
        /// Indicates whether to exclude non translated model data or not
        /// </summary>
        [DataMember]
        public Boolean ExcludeNonTranslatedModelData
        {
            get 
            { 
                return _excludeNonTranslatedModelData; 
            }
            set 
            { 
                _excludeNonTranslatedModelData = value; 
            }
        }

        #endregion Properties

        #region Methods

        #endregion Methods
    }
}
