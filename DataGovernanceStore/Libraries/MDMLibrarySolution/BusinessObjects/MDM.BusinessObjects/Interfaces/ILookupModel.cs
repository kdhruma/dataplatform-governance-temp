using System;

namespace MDM.Interfaces
{
    /// <summary>
    /// Exposes methods or properties to set or get lookup model information.
    /// </summary>
    public interface ILookupModel
    {
        #region Fields

        /// <summary>
        /// Specifies id of lookup.
        /// </summary>
        Int32 Id { get; }

        /// <summary>
        /// Specifies lookup table name with prefix as "tblk_".
        /// </summary>
        String TableName { get; }

        /// <summary>
        /// Specifies display table name for lookup without prefix as "tblk_".
        /// </summary>
        String DisplayTableName { get; }

        /// <summary>
        /// Specifies whether lookup is view based or not.
        /// </summary>
        Boolean IsViewBasedLookup { get; }

        #endregion Fields

        #region Methods
        #endregion Methods
    }
}