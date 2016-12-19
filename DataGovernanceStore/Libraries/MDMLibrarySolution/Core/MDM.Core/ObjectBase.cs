using System;
using System.Runtime.Serialization;

using ProtoBuf;

namespace MDM.Core
{
    //TODO:: Check the impact of applying DataContract attribute.

    /// <summary>
    /// Base of all the BusinessObjects. 
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    [ProtoContract]
    public abstract class ObjectBase
    {
        #region Fields 
        
        /// <summary>
        /// Field defining the type id of the MDM object
        /// </summary>
        [DataMember(Name = "ObjectTypeId")]
        [ProtoMember(1)]
        private Int32 _objectTypeId = 0;

        /// <summary>
        /// Field defining the type of the MDM object
        /// </summary>
        [DataMember(Name = "ObjectType")]
        [ProtoMember(2)]
        private String _objectType = String.Empty;

        #endregion

        #region Constructors 

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public ObjectBase()
        {
        }

        /// <summary>
        /// Initializes a new instance of the Object Base class.
        /// </summary>
        /// <param name="objectType">Type of an Object</param>
        /// <param name="objectTypeId">Type Id of an Object</param>
        public ObjectBase(Int32 objectTypeId, String objectType)
        {
            _objectTypeId = objectTypeId;
            _objectType = objectType;
        }

        #endregion

        #region Properties 

        /// <summary>
        /// Property defining the type id of the MDM object
        /// </summary>
        public virtual Int32 ObjectTypeId
        {
            get
            {
                return _objectTypeId;
            }
        }

        /// <summary>
        /// Property defining the type of the MDM object
        /// </summary>
        public virtual String ObjectType
        {
            get
            {
                return _objectType;
            }
        }

        #endregion 
    }
}
