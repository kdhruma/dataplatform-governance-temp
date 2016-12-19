using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties used for providing entity variant definition mapping information.
    /// </summary>
    public interface IEntityVariantDefinitionMapping : IMDMObject
    {
        #region Fields

        /// <summary>
        /// Field indicates identifier of entity variant definition
        /// </summary>
        Int32 EntityVariantDefinitionId { get; set; }

        /// <summary>
        /// Field indicates name of entity variant definition
        /// </summary>
        String EntityVariantDefinitionName { get; set; }

        /// <summary>
        /// Field indicates identifier of container
        /// </summary>
        Int32 ContainerId { get; set; }

        /// <summary>
        /// Field indicates name of container
        /// </summary>
        String ContainerName { get; set; }

        /// <summary>
        /// Field indicates identifier of category
        /// </summary>
        Int64 CategoryId { get; set; }

        /// <summary>
        /// Field indicates name of category
        /// </summary>
        String CategoryName { get; set; }

        /// <summary>
        /// Field indicates path of category
        /// </summary>
        String CategoryPath { get; set; }

        #endregion Fields

        #region Methods

        /// <summary>
        /// Represents entity variant definition mapping in Xml format 
        /// </summary>
        /// <returns>Returns entity variant definition mapping in Xml format as string.</returns>
        String ToXml();

        #endregion Methods

    }
}
