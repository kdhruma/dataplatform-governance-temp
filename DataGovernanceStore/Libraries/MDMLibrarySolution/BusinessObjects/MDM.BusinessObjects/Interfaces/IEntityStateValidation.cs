using System;
using System.Collections.ObjectModel;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties used for providing entity state validation related information.
    /// </summary>
    public interface IEntityStateValidation : IMDMObject
    {
        #region Properties

        /// <summary>
        /// Indicates unique identifier for System validation state attribute  
        /// </summary>
        SystemAttributes SystemValidationStateAttribute { get; set; }

        /// <summary>
        ///  Indicates reason type
        /// </summary> 
        ReasonType ReasonType { get; set; }

        /// <summary>
        /// Indicates unique identifier for Entity  
        /// </summary> 
        Int64 EntityId { get; set; }

        /// <summary>
        /// Indicates name for Entity  
        /// </summary>
        String EntityName { get; set; }

        /// <summary>
        /// Indicates long name for Entity  
        /// </summary>
        String EntityLongName { get; set; }

        /// <summary>
        /// Indicates entity type long name of an entity
        /// </summary>
        String EntityTypeLongName { get; set; }

        /// <summary>
        /// Indicates category long name of an entity
        /// </summary>
        String CategoryLongName { get; set; }

        /// <summary>
        /// Indicates unique identifier for container  
        /// </summary> 
        Int32 ContainerId { get; set; }

        /// <summary>
        /// Indicates container name for Entity  
        /// </summary>
        String ContainerName { get; set; }

        /// <summary>
        /// Indicates container long name for Entity  
        /// </summary>
        String ContainerLongName { get; set; }

        /// <summary>
        /// Indicates unique identifier for relationship  
        /// </summary> 
        Int64 RelationshipId { get; set; }

        /// <summary>
        /// Indicates relationship type long name for Entity 
        /// </summary>
        String RelationshipTypeLongName { get; set; }

        /// <summary>
        /// Indicates unique identifier for related entity   
        /// </summary>
        Int64 RelatedEntityId { get; set; }

        /// <summary>
        /// Indicates name of related entity
        /// </summary>
        String RelatedEntityName { get; set; }

        /// <summary>
        /// Indicates long name of related entity  
        /// </summary>
        String RelatedEntityLongName { get; set; }

        /// <summary>
        /// Indicates attribute model type  
        /// </summary>{ get; set; }
        AttributeModelType AttributeModelType { get; set; }

        /// <summary>
        /// Indicates unique identifier for attribute   
        /// </summary>
        Int32 AttributeId { get; set; }

        /// <summary>
        /// Indicates name of an attribute
        /// </summary>
        String AttributeName { get; set; }

        /// <summary>
        /// Indicates long name of an attribute
        /// </summary>
        String AttributeLongName { get; set; }

        /// <summary>
        /// Indicates message code for error/ warning / information 
        /// </summary> 
        String MessageCode { get; set; }

        /// <summary>
        /// Indicates message parameter with #@# string separator 
        /// </summary> 
        Collection<Object> MessageParameters { get; set; }

        /// <summary>
        /// Indicates unique identifier for rule map context
        /// </summary> 
        Int32 RuleMapContextId { get; set; }

        /// <summary>
        ///Indicates rule map context name
        /// </summary>
        String RuleMapContextName { get; set; }

        /// <summary>
        /// Indicates unique identifier for rule 
        /// </summary> 
        Int32 RuleId { get; set; }

        /// <summary>
        ///Indicates rule name
        /// </summary>
        String RuleName { get; set; }

        /// <summary>
        /// Indicates operation result type 
        /// </summary> 
        OperationResultType OperationResultType { get; set; }

        /// <summary>
        /// Indicates job id
        /// </summary>
        Int32 JobId { get; set; }

        /// <summary>
        /// Indicates time stamp indicating when error was recorded.
        /// </summary>
        DateTime AuditTimeStamp { get; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Represents EntityStateValidation  in Xml format
        /// </summary>
        /// <returns>
        /// EntityStateValidation in Xml format
        /// </returns>
        String ToXml();

        #endregion Methods
    }
}