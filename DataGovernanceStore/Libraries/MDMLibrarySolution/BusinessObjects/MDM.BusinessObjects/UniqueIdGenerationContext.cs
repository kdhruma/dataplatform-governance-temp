using System;
using System.Runtime.Serialization;

namespace MDM.BusinessObjects
{
    using MDM.Interfaces;
    using MDM.Core;

    /// <summary>
    /// Specifies context for any unique id. This context helps in generating unique ids based on the context parameters.
    /// </summary>
    [DataContract]
    public class UniqueIdGenerationContext : ApplicationContext, IUniqueIdGenerationContext
    {
        #region Fields        

        /// <summary>
        /// Field denoting the object type to indicate the context
        /// </summary>
        private ObjectType _dataModelObjectType;

        /// <summary>
        /// Field denoting whether to fill missing id by name or not
        /// </summary>
        private Boolean _resolveNameToId = true;

        /// <summary>
        /// Field denoting how many unique ids are to be generated
        /// </summary>
        private Int32 _noOfUIdsToGenerate = 1;

        #endregion

        #region Constructor

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public UniqueIdGenerationContext()
        {

        }               

        #endregion

        #region Properties        

        /// <summary>
        /// Property denoting object Type for the unique id context
        /// </summary>
        [DataMember]
        public ObjectType DataModelObjectType
        {
            get { return _dataModelObjectType; }
            set { _dataModelObjectType = value; }
        }

        /// <summary>
        /// Property denoting whether to fill missing names by id or not
        /// </summary>
        [DataMember]
        public Boolean ResolveNameToId
        {
            get { return _resolveNameToId; }
            set { _resolveNameToId = value; }
        }

        /// <summary>
        /// Property denoting how many unique ids are to be generated
        /// </summary>
        [DataMember]
        public Int32 NoOfUIdsToGenerate
        {
            get { return _noOfUIdsToGenerate; }
            set { _noOfUIdsToGenerate = value; }
        }

        #endregion
    }
}
