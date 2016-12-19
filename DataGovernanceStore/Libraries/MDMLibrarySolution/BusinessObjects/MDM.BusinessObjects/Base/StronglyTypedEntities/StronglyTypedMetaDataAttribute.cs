using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDM.BusinessObjects
{
    /// <summary>
    /// Represents the attribute to be decorated on C# strong type class properties representing the required meta data of the attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class StronglyTypedMetaDataAttribute : System.Attribute
    {
        Int32 _attributeId = -1;
        String _shortName = String.Empty;
        String _parentName = String.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="StronglyTypedMetaDataAttribute"/> class.
        /// </summary>
        public StronglyTypedMetaDataAttribute()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StronglyTypedMetaDataAttribute" /> class.
        /// </summary>
        /// <param name="attributeId">The attribute identifier.</param>
        /// <param name="shortName">The short name of the attribute.</param>
        /// <param name="parentName">Name of the parent attribute or group.</param>
        public StronglyTypedMetaDataAttribute(Int32 attributeId, String shortName, String parentName)
        {
            _attributeId = attributeId;
            _shortName = shortName;
            _parentName = parentName;
        }

        /// <summary>
        /// Gets or sets the attribute identifier.
        /// </summary>
        /// <value>
        /// The attribute identifier.
        /// </value>
        public Int32 AttributeId
        {
            get
            {
                return _attributeId;
            }
            set
            {
                _attributeId = value;
            }
        }

        /// <summary>
        /// Gets or sets the short name.
        /// </summary>
        /// <value>
        /// The short name.
        /// </value>
        public String ShortName
        {
            get
            {
                return _shortName;
            }
            set
            {
                _shortName = value;
            }
        }

        /// <summary>
        /// Gets or sets the name of the parent.
        /// </summary>
        /// <value>
        /// The name of the parent.
        /// </value>
        public String ParentName
        {
            get
            {
                return _parentName;
            }
            set
            {
                _parentName = value;
            }
        }
    }
}
