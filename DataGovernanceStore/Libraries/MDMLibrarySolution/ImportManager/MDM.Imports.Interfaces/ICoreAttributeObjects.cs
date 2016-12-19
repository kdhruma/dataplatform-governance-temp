using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDM.Imports.Interfaces
{
    /// <summary>
    /// This interface is used by the Import processor to bulk copy the attributes in to the core database server.
    /// </summary>
    public interface ICoreAttributeObjects
    {
        /// <summary>
        /// Gets the common attribute object.
        /// </summary>
        /// <returns></returns>
        IBulkInsert GetCommonAttribueObject();

        /// <summary>
        /// Gets the technical attribute object
        /// </summary>
        /// <returns></returns>
        IBulkInsert GetTechnicalAttributeObject();
        
        /// <summary>
        /// Gets the DN Search object
        /// </summary>
        /// <returns></returns>
        IDNSearch GetDNSearchObject();

        /// <summary>
        /// Gets the Relationship attribute object
        /// </summary>
        /// <returns></returns>
        IBulkInsert GetRelationshipAttribueObject();
    }
}
