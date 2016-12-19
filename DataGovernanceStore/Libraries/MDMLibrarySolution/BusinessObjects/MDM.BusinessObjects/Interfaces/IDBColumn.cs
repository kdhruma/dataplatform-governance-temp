using System;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    ///  Exposes methods and properties used for defining database column.
    /// </summary>
    public interface IDBColumn : IMDMObject
    {
        #region Properties

        /// <summary>
        /// Field denoting Old Name of DBColumn
        /// </summary>
        String OldName { get; set; }

        /// <summary>
        /// Field denoting DataType of DBColumn
        /// </summary>
        String DataType { get; set; }

        /// <summary>
        /// Field denoting Length of DBColumn
        /// </summary>
        Int32 Length { get; set; }

        /// <summary>
        /// Field denoting Precision of DBColumn
        /// </summary>
        Int32 Precision { get; set; }
        
        /// <summary>
        /// Field denoting Sequence of DBColumn
        /// </summary>
        Int32 Sequence { get; set; }

        /// <summary>
        /// Field denoting DefaultValue of DBColumn
        /// </summary>
        String DefaultValue { get; set; }

        /// <summary>
        /// Field denoting Nullable of DBColumn
        /// </summary>
        Boolean Nullable { get; set; }

        /// <summary>
        /// Field denoting IsUnique of DBColumn
        /// </summary>
        Boolean IsUnique { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Get Xml representation of DBColumn object
        /// </summary>
        /// <returns>Xml representation of object</returns>
        String ToXml();

        /// <summary>
        /// Get Xml representation of DBColumn object
        /// </summary>
        /// <param name="serialization">serialization option. Based on the value selected, the different xml representation will be there</param>
        /// <returns>Xml representation of object</returns>
        String ToXml(ObjectSerialization serialization);

        /// <summary>
        /// Clone DBColumn object
        /// </summary>
        /// <returns>cloned copy of DBcolumn object.</returns>
        IDBColumn Clone();

        #endregion
    }
}