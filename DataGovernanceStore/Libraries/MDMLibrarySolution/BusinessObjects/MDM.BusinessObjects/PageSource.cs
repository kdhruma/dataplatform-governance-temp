using System.Runtime.Serialization;

namespace MDM.BusinessObjects
{
    /// <summary>
    /// Represent class to indicate the page source
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum PageSource
    {
        /// <summary>
        /// Indicates unknown
        /// </summary>
        [EnumMember]
        Unknown = 0,

        /// <summary>
        /// Indicates entity explorer as page source
        /// </summary>
        [EnumMember]
        EntityExplorer = 1,

        /// <summary>
        /// Indicates entity hierarchy as page source
        /// </summary>
        [EnumMember]
        EntityHierarchy = 2,

        /// <summary>
        /// Indicates relationships as page source
        /// </summary>
        [EnumMember]
        Relationships = 3
    }
}
