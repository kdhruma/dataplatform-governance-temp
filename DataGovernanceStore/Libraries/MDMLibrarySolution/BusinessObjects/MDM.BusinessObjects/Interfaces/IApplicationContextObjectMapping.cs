using MDM.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDM.BusinessObjects.Interfaces
{
    /// <summary>
    /// Exposes methods or properties used for providing application context object mapping information.
    /// </summary>
    public interface IApplicationContextObjectMapping : IMDMObject 
    {
        #region Fields

        /// <summary>
        /// Field indicates identifier of application context
        /// </summary>
        Int32 ApplicationContextId { get; set; }

        /// <summary>
        /// Field indicates type of application context
        /// </summary>
        ApplicationContextType ApplicationContextType { get; set; }

        /// <summary>
        /// Field indicates type of object
        /// </summary>
        Int32 ContextObjectTypeId { get; set; }

        /// <summary>
        /// Field indicates identifier of object
        /// </summary>
        Int64 ObjectId { get; set; }

        #endregion Fields

        #region Methods

        /// <summary>
        /// Represents application context object mapping in Xml format 
        /// </summary>
        /// <returns>Returns application context object mapping in Xml format as string.</returns>
        String ToXml();

        #endregion Methods
    }
}
