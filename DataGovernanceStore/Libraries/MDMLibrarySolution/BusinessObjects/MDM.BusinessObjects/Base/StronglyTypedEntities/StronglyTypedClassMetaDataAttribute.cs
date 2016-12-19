using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDM.BusinessObjects
{
    /// <summary>
    /// Represents the attribute to be decorated on C# strong type class representing the required meta data of the class
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class StronglyTypedClassMetaDataAttribute : System.Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StronglyTypedClassMetaDataAttribute"/> class.
        /// </summary>
        public StronglyTypedClassMetaDataAttribute()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StronglyTypedClassMetaDataAttribute"/> class.
        /// </summary>
        /// <param name="entityTypeId">Entity Type Id</param>
        public StronglyTypedClassMetaDataAttribute(Int32 entityTypeId)
        {
            this.EntityTypeId = entityTypeId;
        }


        /// <summary>
        /// Gets or sets the entity type id.
        /// </summary>
        /// <value>
        /// The entity type id
        /// </value>
        public Int32 EntityTypeId { get; set; }
    }
}
