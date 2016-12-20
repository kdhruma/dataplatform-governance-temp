using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDM.BusinessObjects.Interfaces
{
    /// <summary>
    /// Exposes methods or properties used for providing application context object mapping collection information.
    /// </summary>
    public interface IApplicationContextObjectMappingCollection : IEnumerable<ApplicationContextObjectMapping>
    {
        #region Fields
        #endregion Fields

        #region Methods

        /// <summary>
        /// Represents application context object mapping collection in Xml format 
        /// </summary>
        /// <returns>Returns application context object mapping collection in Xml format as string.</returns>
        String ToXml();

        #endregion Methhods

    }
}
