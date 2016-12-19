using System.Runtime.Serialization;

namespace MDM.BusinessObjects.Exports
{
    /// <summary>
    /// Class to represent Export Type
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum ExportType
    {
        /// <summary>
        /// Indicates unknown
        /// </summary>
        [EnumMember]
        Unknown = 0,

        /// <summary>
        /// Indicates all attributes as export type
        /// </summary>
        [EnumMember]
        AllAttributes = 1,

        /// <summary>
        /// Indicates visible attribute as export type
        /// </summary>
        [EnumMember]
        VisibleAttributes = 2,

        /// <summary>
        /// Indicates export template as export type
        /// </summary>
        [EnumMember]
        ExportTemplate = 3,

        /// <summary>
        /// Indicates all attributes and all relationships as export type
        /// </summary>
        [EnumMember]
        AllAttributesAndAllRelationships = 4
    }
}
