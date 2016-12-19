using System;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties for setting the application configuration item.
    /// </summary>
    public interface IApplicationConfigurationItem : IMDMObject
    {
        /// <summary>
        /// Property indicates ConfigParentId
        /// </summary>
        Int32? ConfigParentId { get; set; }

        /// <summary>
        /// Property indicates ContextDefinitionId
        /// </summary>
        Int32? ContextDefinitionId { get; set; }

        /// <summary>
        /// Property indicates OrganizationId
        /// </summary>
        Int32 OrganizationId { get; set; }

        /// <summary>
        /// Property indicates ContainerId
        /// </summary>
        Int32 ContainerId { get; set; }

        /// <summary>
        /// Property indicates CategoryId
        /// </summary>
        Int64 CategoryId { get; set; }

        /// <summary>
        /// Property indicates EntityId
        /// </summary>
        Int64 EntityId { get; set; }

        /// <summary>
        /// Property indicates AttributeId
        /// </summary>
        Int32 AttributeId { get; set; }

        /// <summary>
        /// Property indicates NodeTypeId
        /// </summary>
        Int32 NodeTypeId { get; set; }

        /// <summary>
        /// Property indicates RelationshipTypeId
        /// </summary>
        Int32 RelationshipTypeId { get; set; }

        /// <summary>
        /// Property indicates SecurityRoleId
        /// </summary>
        Int32 SecurityRoleId { get; set; }

        /// <summary>
        /// Property indicates SecurityUserId
        /// </summary>
        Int32 SecurityUserId { get; set; }

        /// <summary>
        /// Property indicates ConfigXML
        /// </summary>
        String ConfigXml { get; set; }

        /// <summary>
        /// Property indicates Description
        /// </summary>
        String Description { get; set; }

        /// <summary>
        /// Property indicates Precondition
        /// </summary>
        String Precondition { get; set; }

        /// <summary>
        /// Property indicates Postcondition
        /// </summary>
        String Postcondition { get; set; }

        /// <summary>
        /// Property indicates XsdSchema
        /// </summary>
        String XsdSchema { get; set; }

        /// <summary>
        /// Property indicates SampleXml
        /// </summary>
        String SampleXml { get; set; }

        /// <summary>
        /// Property indicates CreateDateTime
        /// </summary>
        DateTime? CreateDateTime { get; set; }

        /// <summary>
        /// Property indicates ModDateTime
        /// </summary>
        DateTime? ModDateTime { get; set; }

        /// <summary>
        /// Property indicates CreateUser
        /// </summary>
        String CreateUser { get; set; }

        /// <summary>
        /// Property indicates ModUser
        /// </summary>
        String ModUser { get; set; }

        /// <summary>
        /// Property indicates CreateProgram
        /// </summary>
        String CreateProgram { get; set; }

        /// <summary>
        /// Property indicates ModProgram
        /// </summary>
        String ModProgram { get; set; }

        /// <summary>
        /// Property indicates SequenceNumber
        /// </summary>
        Int32? SequenceNumber { get; set; }

        /// <summary>
        /// Property indicates Locale
        /// </summary>
        new LocaleEnum? Locale { get; set; }

        /// <summary>
        /// Property indicates Lookup Name
        /// </summary>
        String ObjectName { get; set; }
    }
}

