using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDM.BusinessObjects.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Search Weightage Interface
    /// </summary>
    public interface ISearchWeightageAttribute :IMDMObject
    {
        /// <summary>
        /// Get Xml representation of Search Weightage object
        /// </summary>
        /// <returns>Xml representation of Search Weightage object</returns>
        String ToXml();
  
    }
}
