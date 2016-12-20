using System;
using System.Collections.ObjectModel;

namespace MDM.Interfaces.Exports
{
    using MDM.BusinessObjects.Exports;
using MDM.Core;
using MDM.Core.DataModel;

    /// <summary>
    /// Exposes methods or properties to set or get the data model export context business object.
    /// </summary>
    public interface IDataModelExportContext : IMDMObject
    {
        #region Properties

        /// <summary>
        /// Represents collection of organization ids that should be used during export
        /// </summary>
        Collection<Int32> OrganizationIdList { get; set; }

        /// <summary>
        /// Represents collection of container ids that should be used during export
        /// </summary>
        Collection<Int32> ContainerIdList { get; set; }

        /// <summary>
        /// Represents collection of hierarchy ids that should be used during export
        /// </summary>
        Collection<Int32> HierarchyIdList { get; set; }

        /// <summary>
        /// Represents collection of entity type ids that should be used during export
        /// </summary>
        Collection<Int32> EntityTypeIdList { get; set; }

        /// <summary>
        /// Represents collection of relationship type ids that should be used during export
        /// </summary>
        Collection<Int32> RelationshipTypeIdList { get; set; }

        /// <summary>
        /// Represents collection of lookup table names that should be used during export
        /// </summary>
        Collection<String> LookupTableNames { get; set; }

        /// <summary>
        /// Represents collection of sheet names that needs to be export
        /// </summary>
        Collection<DataModelSheet> SheetNames { get; set; }

        /// <summary>
        /// Represents collection of locales in which data model needs to export
        /// </summary>
        Collection<LocaleEnum> Locales { get; set; }

        /// <summary>
        /// Represents whether to exclude non translated model data or not
        /// </summary>
        Boolean ExcludeNonTranslatedModelData { get; set; }

        #endregion Properties
    }
}
