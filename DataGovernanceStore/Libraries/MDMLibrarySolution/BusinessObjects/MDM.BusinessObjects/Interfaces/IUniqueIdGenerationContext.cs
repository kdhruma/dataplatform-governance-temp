using System;

namespace MDM.Interfaces
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Exposes methods or properties to set or get unique id context, which indicates information to be loaded into the unique id object.
    /// </summary>
    public interface IUniqueIdGenerationContext: IApplicationContext
    {
        #region Properties

        /// <summary>
        /// Property denoting object Type for the unique id context
        /// </summary>
        ObjectType DataModelObjectType { get; set; }

        /// <summary>
        /// Property denoting whether to fill missing names by id or not
        /// </summary>
        Boolean ResolveNameToId { get; set; }        

        /// <summary>
        /// Property denoting how many unique ids are to be generated
        /// </summary>
        Int32 NoOfUIdsToGenerate { get; set; }

        #endregion
    }
}
