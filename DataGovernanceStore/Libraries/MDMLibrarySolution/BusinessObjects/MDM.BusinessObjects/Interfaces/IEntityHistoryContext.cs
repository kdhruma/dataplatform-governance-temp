using System;
using System.Collections.ObjectModel;

namespace MDM.Interfaces
{

    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get entity history context, which indicates information to be loaded into the entity history object.
    /// </summary>
    public interface IEntityHistoryContext
    {
        #region Properties

        /// <summary>
        /// Property to denote attribute id list for which item level history is requested
        /// </summary>
        Collection<Int32> AttributeIdList { get; set; }

        /// <summary>
        /// Property denoting container id of an entity for which history is requested
        /// </summary>
        Int32 ContainerId { get; set; }

        /// <summary>
        /// Property to denote starting(from) date time 
        /// </summary>
        DateTime? FromDateTime { get; set; }

        /// <summary>
        /// Property to denote end(to) date time
        /// </summary>
        DateTime? ToDateTime { get; set; }

        /// <summary>
        /// Property to denotes whether to load metadata version details 
        /// </summary>
        Boolean LoadMetadataVersionDetails { get; set; }

        /// <summary>
        /// Property to denotes whether to load attribute version details 
        /// </summary>
        Boolean LoadAttributesVersionDetails { get; set; }

        /// <summary>
        /// Property to denotes whether to load relationship attribute version details 
        /// </summary>
        Boolean LoadRelationshipsVersionDetails { get; set; }

        /// <summary>
        /// Property to denotes whether to load extension relationship version details 
        /// </summary>
        Boolean LoadExtensionRelationshipsVersionDetails { get; set; }

        /// <summary>
        /// Property to denotes whether to load hierarchy relationship version details 
        /// </summary>
        Boolean LoadHierarchyRelationshipsVersionDetails { get; set; }

        /// <summary>
        /// Property to denotes whether to load workflow details or not 
        /// </summary>
        Boolean LoadWorkflowVersionDetails { get; set; }

        /// <summary>
        /// Property denoting whether requested history is for entity or category.
        /// </summary>
        Boolean IsHistoryForCategory { get; set; }

        /// <summary>
        /// Property denoting locale in which model has to be displayed
        /// </summary>
        LocaleEnum CurrentDataLocale { get; set; }

        /// <summary>
        /// Property denoting locale in which data has to be formatted
        /// </summary>
        LocaleEnum CurrentUILocale { get; set; }

        /// <summary>
        /// Property to denotes number of records to return in result
        /// </summary>
        Int32 MaxRecordsToReturn { get; set; }
        #endregion

        #region Methods

        /// <summary>
        /// Represents EntityHistoryContext  in Xml format
        /// </summary>
        /// <returns>
        /// EntityHistoryContext  in Xml format
        /// </returns>
        String ToXml();

        #endregion
    }
}
