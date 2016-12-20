using System;
using System.Runtime.Serialization;

namespace MDM.BusinessObjects
{
    /// <summary>
    /// Specifies relationship processing context.
    /// </summary>
    [DataContract]
    public class RelationshipProcessingContext
    {
        #region Fields

        /// <summary>
        /// Field for the entity id of relationship processing context.
        /// </summary>
        private Int64 _entityId = -1;

        /// <summary>
        /// Field for the category id of relationship processing context.
        /// </summary>
        private Int64 _categoryId = 0;

        /// <summary>
        /// Field for the entity type id of relationship processing context.
        /// </summary>
        private Int32 _entityTypeId = 0;

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public RelationshipProcessingContext()
            : base()
        {
        }

        /// <summary>
        /// Constructor with entityId, categoryId and entityTypeId of an Entity as input parameters
        /// </summary>
        /// <param name="entityId">Specifies entity id of an entity</param>
        /// <param name="categoryId">Specifies category id of an entity</param>
        /// <param name="entityTypeId">Specifies entity type id of an entity</param>
        public RelationshipProcessingContext(Int64 entityId, Int64 categoryId, Int32 entityTypeId)
            : base()
        {
            this._entityId = entityId;
            this._categoryId = categoryId;
            this._entityTypeId = entityTypeId;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Specifies entity id for relationship processing context.
        /// </summary>
        [DataMember]
        public Int64 EntityId
        {
            get { return _entityId; }
            set { _entityId = value; }
        }

        /// <summary>
        /// Specifies category id for relationship processing context.
        /// </summary>
        [DataMember]
        public Int64 CategoryId
        {
            get
            {
                return this._categoryId;
            }
            set
            {
                this._categoryId = value;
            }
        }

        /// <summary>
        /// Specifies entity type id for relationship processing context.
        /// </summary>
        [DataMember]
        public Int32 EntityTypeId
        {
            get
            {
                return this._entityTypeId;
            }
            set
            {
                this._entityTypeId = value;
            }
        }

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
            {
                if (obj is Entity)
                {
                    RelationshipProcessingContext objectToBeCompared = obj as RelationshipProcessingContext;

                    if (this.EntityId != objectToBeCompared.EntityId)
                        return false;

                    if (this.CategoryId != objectToBeCompared.CategoryId)
                        return false;

                    if (this.EntityTypeId != objectToBeCompared.EntityTypeId)
                        return false;

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Serves as a hash function for type
        /// </summary>
        /// <returns>A hash code for the current Object.</returns>
        public override Int32 GetHashCode()
        {
            return this.EntityId.GetHashCode() ^ this.CategoryId.GetHashCode() ^ this.EntityTypeId.GetHashCode();
        }

        #endregion

        #region Private Methods
        #endregion

        #endregion
    }
}