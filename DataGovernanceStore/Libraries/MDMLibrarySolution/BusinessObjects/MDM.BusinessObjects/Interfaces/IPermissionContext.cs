using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MDM.Core;
using MDM.BusinessObjects;
using System.Collections.ObjectModel;

namespace MDM.Interfaces
{
    /// <summary>
    /// Exposes methods or properties to set or get context for permissions.
    /// </summary>
    public interface IPermissionContext 
    {

        /// <summary>
        /// Property denoting the role id
        /// </summary>
         Int32 RoleId { get; set; }
        

        /// <summary>
        /// Property denoting the user id
        /// </summary>
         Int32 UserId { get; set; }
        

        /// <summary>
        /// Property denoting the Org Id
        /// </summary>
         Int32 OrgId { get; set; }
       
        /// <summary>
        /// Property denoting the Container Id
        /// </summary>
        Int32 ContainerId { get; set; }
       
        /// <summary>
        /// Property denoting the Category Id
        /// </summary>
         Int64 CategoryId { get; set; }

        /// <summary>
        /// Property denoting the Entity Type Id
        /// </summary>
        Int32 EntityTypeId { get; set; }

        /// <summary>
        /// Property denoting the Entity Id
        /// </summary>
        Int64 EntityId { get; set; }
        

        /// <summary>
        /// Property denoting the Attribute Id
        /// </summary>

        Int32 AttributeId { get; set; }
        

        /// <summary>
        /// Property denoting the Relationship Type Id
        /// </summary>
        Int32 RelationshipTypeId { get; set; }
       

        /// <summary>
        /// Property denoting the Locale Id
        /// </summary>
        Int32 LocaleId { get; set; }
        

       
    }
}
