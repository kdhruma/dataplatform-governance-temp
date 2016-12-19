using MDM.BusinessObjects.Interfaces;
using ProtoBuf;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace MDM.BusinessObjects
{
    /// <summary>
    /// Class for paginated entity result
    /// </summary>
    [DataContract]
    [ProtoContract]
    public class EntityPaginationResult : IEntityPaginationResult
    {
        /// <summary>
        /// Property for entity result for particular paging citeria
        /// </summary>
        [DataMember]
        [ProtoMember(1)]
        public EntityCollection EntityList
        {
            get;
            set;
        }

        /// <summary>
        /// Property for EntityId list for full result
        /// </summary>
        [DataMember]
        [ProtoMember(2)]
        public Collection<long> EntityIdList
        {
            get;
            set;
        }
    }
}
