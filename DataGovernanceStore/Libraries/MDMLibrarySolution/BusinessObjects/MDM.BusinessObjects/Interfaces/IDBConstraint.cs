using System;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get the database constraint related information.
    /// </summary>
    public interface IDBConstraint
    {
        #region Properties

        /// <summary>
        /// Field denoting ColumnName of DBConstraint
        /// </summary>
        String ColumnName { get; set; }

        /// <summary>
        /// Field denoting ConstraintType of DBConstraint
        /// </summary>
        ConstraintType ConstraintType { get; set; }

        /// <summary>
        /// Field denoting Value of DBConstraint
        /// </summary>
        String Value { get; set; }

        /// <summary>
        /// Field denoting Action of DBConstraint
        /// </summary>
        ObjectAction Action { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Get Xml representation of DBConstraint object
        /// </summary>
        /// <returns>Xml representation of object</returns>
        String ToXml();

        /// <summary>
        /// Get Xml representation of DBConstraint object
        /// </summary>
        /// <param name="serialization">serialization option. Based on the value selected, the different xml representation will be there</param>
        /// <returns>Xml representation of object</returns>
        String ToXml(ObjectSerialization serialization);

        /// <summary>
        /// Clone DBContraint object
        /// </summary>
        /// <returns>cloned copy of DBContraint object.</returns>
        IDBConstraint Clone();

        #endregion
    }
}