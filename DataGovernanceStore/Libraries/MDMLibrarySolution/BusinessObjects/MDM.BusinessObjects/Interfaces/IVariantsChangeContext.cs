using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDM.Interfaces
{
    /// <summary>
    /// Exposes methods or properties used for providing variants change context related information.
    /// </summary>
    public interface IVariantsChangeContext
    {
        #region Properties
        #endregion Properties

        #region Methods

        #region ToXml Methods

        /// <summary>
        /// Gets XML representation of relationship change context object
        /// </summary>
        /// <returns>XML representation of relationship change context object</returns>
        String ToXml();

        #endregion ToXml Methods

        #region Entity Change Contexts related methods

        /// <summary>
        /// Gets the entity change contexts
        /// </summary>
        /// <returns>Return entity change context.</returns>
        IEntityChangeContextCollection GetEntityChangeContexts();

        /// <summary>
        /// Sets the entity change context.
        /// </summary>
        /// <param name="iEntityChangeContexts">Indicates the entity change contexts to be set</param>
        void SetEntityChangeContexts(IEntityChangeContextCollection iEntityChangeContexts);

        #endregion

        #endregion
    }
}