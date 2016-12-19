using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RS.MDM.Configuration
{
    /// <summary>
    /// Provides functionality to track certain properties for changes
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class TrackChangesAttribute : Attribute
    {
        #region Fields

        /// <summary>
        /// field for tracking changes flag
        /// </summary>
        private bool _trackChanges = true;

        /// <summary>
        /// Indicates an attribute to skip tracking changes for a property
        /// </summary>
        public static readonly TrackChangesAttribute No = new TrackChangesAttribute(false);

        /// <summary>
        /// Indicates an attribute to track changes for a property
        /// </summary>
        public static readonly TrackChangesAttribute Yes = new TrackChangesAttribute(true);

        /// <summary>
        /// Indicates default value of the trackingchanges attribute
        /// </summary>
        public static readonly TrackChangesAttribute Default = Yes;

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public TrackChangesAttribute()
        {
        }

        /// <summary>
        /// Constructor with trackChanges flag as input
        /// </summary>
        /// <param name="trackChanges">Indicates a boolean value that denotes if changes need to be tracked for a property</param>
        public TrackChangesAttribute(bool trackChanges)
        {
            this._trackChanges = trackChanges;
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Returns a value that indicates whether this instance is equal to a specified object.
        /// </summary>
        /// <param name="obj">An System.Object to compare with this instance or null.</param>
        /// <returns>true if obj equals the type and value of this instance; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (obj == this)
            {
                return true;
            }
            TrackChangesAttribute attribute = obj as TrackChangesAttribute;
            return ((attribute != null) && (attribute.TrackChanges == this._trackChanges));
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>A 32-bit signed integer hash code.</returns>
        public override int GetHashCode()
        {
            return this._trackChanges.GetHashCode();
        }

        /// <summary>
        /// When overridden in a derived class, indicates whether the value of this instance
        ///     is the default value for the derived class.
        /// </summary>
        /// <returns>true if this instance is the default attribute for the class; otherwise, false.</returns>
        public override bool IsDefaultAttribute()
        {
            return this.Equals(Default);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a boolean value that indicates if changes need to be tracked for a property
        /// </summary>
        public bool TrackChanges
        {
            get
            {
                return this._trackChanges;
            }
        }

        #endregion
    }
}
