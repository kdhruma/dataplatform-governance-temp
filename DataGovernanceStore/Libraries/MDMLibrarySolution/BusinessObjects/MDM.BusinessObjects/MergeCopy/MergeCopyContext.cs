using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace MDM.BusinessObjects.MergeCopy
{
    using MDM.Core;
    using MDM.Interfaces;
    using MDM.Core.Extensions;

    /// <summary>
    /// Represents merge copy context used for all constant settings to control merge copy operation and the handler interface
    /// </summary>
    [DataContract]
    public class MergeCopyContext
    {
        #region Enums and Class for Caller Context

        #region MergeCopySource

        /// <summary>
        /// Indicates the source action for MergeCopy. The program name etc are in EntityContext.
        /// </summary>
        [DataContract(Name = "MergeCopySource")]
        public enum MergeCopySource
        {
            /// <summary>
            /// Indicates promote source
            /// </summary>
            [EnumMember]
            Promote = 1,

            /// <summary>
            /// Indicates checkout source
            /// </summary>
            [EnumMember]
            Checkout,

            /// <summary>
            /// Indicates import source
            /// </summary>
            [EnumMember]
            Import,

            /// <summary>
            /// Indicates MDL source
            /// </summary>
            [EnumMember]
            MDL,

            /// <summary>
            /// Indicates unknown
            /// </summary>
            [EnumMember]
            Unknown
        }

        #endregion

        #region Target object type

        /// <summary>
        /// Indicates the target object type
        /// </summary>
        [DataContract(Name = "TargetObjectType")]
        public enum TargetObjectType
        {
            /// <summary>
            /// Indicates unknown
            /// </summary>
            [EnumMember]
            Unknown,

            /// <summary>
            /// Indicates entity as target object type
            /// </summary>
            [EnumMember]
            Entity,
            
            /// <summary>
            /// Indicates attribute as target object type
            /// </summary>
            [EnumMember]
            Attribute,

            /// <summary>
            /// Indicates relationship as target object type
            /// </summary>
            [EnumMember]
            Relationship
        }

        #endregion 
    
        #region MergeCopyOption

        /// <summary>
        ///  Indicates merge copy option which represents the flags that can be added to a list and passed to the handler and actual merge copy implementation
        /// </summary>
        [DataContract(Name = "MergeCopyOption")]
        public enum MergeCopyOption
        {
            /// <summary>
            /// Indicates that if destination entity exists, delete and fill with clone of duplicate; else merge 
            /// </summary>
            [EnumMember]
            FlushAndFillEntityIfDestinationExists,
            
            /// <summary>
            /// Indicates that if destination attribute exists, delete and fill with clone of duplicate; else merge
            /// </summary>
            [EnumMember]
            FlushAndFillAttributeIfDestinationExists,

            /// <summary>
            /// Indicates that during overwrite, do not write destination attribute if destination time stamp is new
            /// </summary>
            [EnumMember]
            MergeByTimestamp,

            /// <summary>
            /// Indicates that create delta XML
            /// </summary>
            [EnumMember]
            GenerateDeltaOnly,

            /// <summary>
            /// Indicates that if destination attribute exists and flag is not set for flush and fill then stop processing
            /// </summary>
            [EnumMember]
            StopAtFirstConflict,

            /// <summary>
            /// Indicates to copy relationship attributes
            /// </summary>
            [EnumMember]
            CopyRelationshipAttributes,

            /// <summary>
            /// Indicates to copy relationships
            /// </summary>
            [EnumMember]
            CopyRelationships,

            /// <summary>
            /// Indicates to copy system attributes
            /// </summary>
            [EnumMember]
            CopySystemAttributes,

            /// <summary>
            /// Indicates to copy technical attributes
            /// </summary>
            [EnumMember]
            CopyTechnicalAttributes,

            /// <summary>
            /// Indicates to copy common attributes
            /// </summary>
            [EnumMember]
            CopyCommonAttributes,

            /// <summary>
            /// Indicates to copy only required attributes
            /// </summary>
            [EnumMember]
            CopyRequiredAttributesOnly,

            /// <summary>
            /// Indicates to copy only complex attributes
            /// </summary>
            [EnumMember]
            CopyComplexAttributes,

            /// <summary>
            /// Indicates to compresses and base64s the json before sending out
            /// </summary>
            [EnumMember]
            CompressJsonBuffer,

            /// <summary>
            /// Indicates unknown
            /// </summary>
            [EnumMember]
            Unknown,

            /// <summary>
            /// Indicates if need process empty values of source
            /// </summary>
            [EnumMember]
            ProcessEmptySourceAttributes,

            /// <summary>
            /// Indicates whether I(source) to O(target) merge allowed
            /// </summary>
            [EnumMember]
            AllowInheritedToOverriddenMerging
        }

        #endregion

        #region MergeCopyHandlerConflictMode

        /// <summary>
        /// The handler gets this flag to signify the fine grained context where it was called. 
        /// Typically it will also find data pre-populated to execute custom logic, e.g. source attribute, destination attribute
        /// </summary>
        [DataContract(Name = "MergeCopyHandlerConflictMode")]
        public enum MergeCopyHandlerConflictMode
        {
            /// <summary>
            /// Indicates merge copy handler conflict mode as before entity operation
            /// </summary>
            [EnumMember]
            BeforeEntityOperation,

            /// <summary>
            /// Indicates merge copy handler conflict mode as after entity operation mode
            /// </summary>
            [EnumMember]
            AfterEntityOperation,

            /// <summary>
            /// Indicates merge copy handler conflict mode as before attribute operation mode
            /// </summary>
            [EnumMember]
            BeforeAttributeOperation,

            /// <summary>
            /// Indicates merge copy handler conflict mode as after attribute operation mode
            /// </summary>
            [EnumMember]
            AfterAttributeOperation,

            /// <summary>
            /// Indicates the conflict mode when entity conflict happens between source and target and the source is newer than the target
            /// </summary>
            [EnumMember]
            EntityConflictSourceNewer,

            /// <summary>
            /// Indicates the conflict mode when entity conflict happens between source and target and the source is older than the target
            /// </summary>
            [EnumMember]
            EntityConflictSourceOlder,

            /// <summary>
            /// Indicates the conflict mode when entity conflict happens between source and target and the source does not exist
            /// </summary>
            [EnumMember]
            EntityConflictSourceNonexistent,

            /// <summary>
            /// Indicates the conflict mode when entity conflict happens between source and target and the target does not exist
            /// </summary>
            [EnumMember]
            EntityConflictDestinationNonexistent,

            /// <summary>
            /// Indicates the conflict mode when attribute conflict happens between source and target and the source is newer than the target
            /// </summary>
            [EnumMember]
            AttributeConflictSourceNewer,

            /// <summary>
            /// Indicates the conflict mode when attribute conflict happens between source and target and the source is older than the target
            /// </summary>
            [EnumMember]
            AttributeConflictSourceOlder,

            /// <summary>
            /// Indicates the conflict mode when attribute conflict happens between source and target and the source does not exist
            /// </summary>
            [EnumMember]
            AttributeConflictSourceNonexistent,

            /// <summary>
            /// Indicates the conflict mode when attribute conflict happens between source and target and the target does not exist
            /// </summary>
            [EnumMember]
            AttributeConflictDestinationNonexistent,

            /// <summary>
            /// Indicates the conflict mode when relationship conflict happens between source and target and the source is new
            /// </summary>
            [EnumMember]
            RelationshipConflictSourceNewer,

            /// <summary>
            /// Indicates the conflict mode when relationship conflict happens between source and target and the source is old
            /// </summary>
            [EnumMember]
            RelationshipConflictSourceOlder,

            /// <summary>
            /// Indicates the conflict mode when relationship conflict happens between source and target and the source does not exist
            /// </summary>
            [EnumMember]
            RelationshipConflictSourceNonexistent,

            /// <summary>
            /// Indicates the conflict mode when relationship conflict happens between source and target and the target does not exist
            /// </summary>
            [EnumMember]
            RelationshipConflictDestinationNonexistent,

            /// <summary>
            /// Indicates the conflict mode when relationship attribute conflict happens between source and target and the source is new
            /// </summary>
            [EnumMember]
            RelationshipAttributeConflictSourceNewer,

            /// <summary>
            /// Indicates the conflict mode when relationship attribute conflict happens between source and target and the source is old
            /// </summary>
            [EnumMember]
            RelationshipAttributeConflictSourceOlder,

            /// <summary>
            /// Indicates the conflict mode when relationship attribute conflict happens between source and target and the source does not exist
            /// </summary>
            [EnumMember]
            RelationshipAttributeConflictSourceNonexistent,

            /// <summary>
            /// Indicates the conflict mode when relationship attribute conflict happens between source and target and the target does not exist
            /// </summary>
            [EnumMember]
            RelationshipAttributeConflictDestinationNonexistent,

            /// <summary>
            /// Indicates the conflict mode when entity conflict happens between source and target and the source is more preferable than target
            /// </summary>
            [EnumMember]
            EntityConflictSourceLineagePreferrable,

            /// <summary>
            /// Indicates the conflict mode when entity conflict happens between source and target and the target is more preferable than source
            /// </summary>
            [EnumMember]
            EntityConflictTargetLineagePreferrable,

            /// <summary>
            /// Indicates the conflict mode when entity conflict happens between source and target and the external source is preferred
            /// </summary>
            [EnumMember]
            EntityConflictSourceExternalPreferrable,

            /// <summary>
            /// Indicates the conflict mode when entity conflict happens between source and target and the external target is preferred
            /// </summary>
            [EnumMember]
            EntityConflictTargetExternalPreferrable,

            /// <summary>
            /// Indicates the conflict mode when attribute conflict happens between source and target and the source is more preferable than the target
            /// </summary>
            [EnumMember]
            AttributeConflictSourceLineagePreferrable,

            /// <summary>
            /// Indicates the conflict mode when entity conflict happens between source and target and the target is more preferable than the source
            /// </summary>
            [EnumMember]
            AttributeConflictTargetLineagePreferrable,

            /// <summary>
            /// Indicates the conflict mode when attribute conflict happens between source and target and the external source is preferred
            /// </summary>
            [EnumMember]
            AttributeConflictSourceExternalPreferrable,

            /// <summary>
            /// Indicates the conflict mode when attribute conflict happens between source and target and the external target is preferred
            /// </summary>
            [EnumMember]
            AttributeConflictTargetExternalPreferrable,

            /// <summary>
            /// Indicates the conflict mode when attribute conflict happens between source and target and the union operation to be performed
            /// </summary>
            [EnumMember]
            AttributeConflictUnion,

            /// <summary>
            /// Indicates the conflict mode when relationship conflict happens between source and target and the source is preferable than the target
            /// </summary>
            [EnumMember]
            RelationshipConflictSourceLineagePreferrable,

            /// <summary>
            /// Indicates the conflict mode when relationship conflict happens between source and target and the target is preferable than the source
            /// </summary>
            [EnumMember]
            RelationshipConflictTargetLineagePreferrable,

            /// <summary>
            /// Indicates the conflict mode when relationship conflict happens between source and target and the custom source is preferred
            /// </summary>
            [EnumMember]
            RelationshipConflictSourceCustomPreferrable,

            /// <summary>
            /// Indicates the conflict mode when relationship conflict happens between source and target and the custom target is preferred
            /// </summary>
            [EnumMember]
            RelationshipConflictTargetCustomPreferrable,

            /// <summary>
            /// Indicates the conflict mode when relationship attribute conflict happens between source and target and the source is preferable than the target
            /// </summary>
            [EnumMember]
            RelationshipAttributeConflictSourceLineagePreferrable,

            /// <summary>
            /// Indicates the conflict mode when relationship attribute conflict happens between source and target and the target is preferable than the source
            /// </summary>
            [EnumMember]
            RelationshipAttributeConflictTargetLineagePreferrable,

            /// <summary>
            /// Indicates the conflict mode when relationship attribute conflict happens between source and target and the custom source is preferred
            /// </summary>
            [EnumMember]
            RelationshipAttributeConflictSourceCustomPreferrable,

            /// <summary>
            /// Indicates the conflict mode when relationship attribute conflict happens between source and target and the custom target is preferred
            /// </summary>
            [EnumMember]
            RelationshipAttributeConflictTargetCustomPreferrable,

            /// <summary>
            /// Indicates mode as all
            /// </summary>
            [EnumMember]
            ALL,

            /// <summary>
            /// Indicates unknown
            /// </summary>
            [EnumMember]
            Unknown
        }

        #endregion

        #region Json marshaller state machine enums

        /// <summary>
        /// Indicates the class that holds json marshaller state for MergeCopy handler call
        /// </summary>
        [DataContract]
        public class MergeCopyJsonMarshallerState
        {
            /// <summary>
            /// Indicates whether the current merge copy json marshaller state is in preamble
            /// </summary>
            [DataMember]
            public bool isInPreamble = false;

            /// <summary>
            /// Indicates whether the current merge copy json marshaller state is in entity conflict array
            /// </summary>
            [DataMember]
            public bool isInEntityConflictArray = false;

            /// <summary>
            /// Indicates whether the current merge copy json marshaller state is in entity conflict element
            /// </summary>
            [DataMember]
            public bool isInEntityConflictElement = false;

            /// <summary>
            /// Indicates whether the current merge copy json marshaller state is in attribute conflict array
            /// </summary>
            [DataMember]
            public bool isInAttributeConflictsArray = false;

            /// <summary>
            /// Indicates whether the current merge copy json marshaller state is in relationship conflict array
            /// </summary>
            [DataMember]
            public bool isInRelationshipConflictsArray = false;

            /// <summary>
            /// Indicates whether the current merge copy json marshaller state is in relationship conflict element
            /// </summary>
            [DataMember]
            public bool isInRelationshipConflictsElement = false;

            /// <summary>
            /// Indicates the complex attribute conflict buffer information
            /// </summary>
            [DataMember]
            public StringBuilder complexAttributeConflictBuffer = new StringBuilder();
        }

        #endregion

        /// <summary>
        /// Indicates the class that holds all filter information for MergeCopy handler call
        /// </summary>
        [DataContract]
        [KnownType(typeof(MergeCopyContext.MergeCopySource))]
        [KnownType(typeof(MergeCopyContext.MergeCopyOutputStatus))]
        [KnownType(typeof(MergeCopyContext.MergeCopyOption))]
        [KnownType(typeof(MergeCopyContext.MergeCopyJsonMarshallerState))]
        [KnownType(typeof(MergeCopyContext.MergeCopyInternalHandler))]
        [KnownType(typeof(List<MergeCopyContext.MergeCopyHandlerConflictMode>))]
        [KnownType(typeof(Dictionary<String, Boolean>))]
        [KnownType(typeof(List<MergeCopyOption>))]
        [KnownType(typeof(Object[]))]
        public class MergeCopyCallerContext
        {
            private const string BEFORE_AFTER_FILTER = "BeforeAfter";

            #region Internal data structures

            /// <summary>
            /// Indicates mapping for subscribed entity type filter
            /// </summary>
            [DataMember]
            Dictionary<String, Boolean> subscribedEntityTypeFilterMap;

            /// <summary>
            /// Indicates mapping for subscribed attribute type filter
            /// </summary>
            [DataMember]
            Dictionary<String, Boolean> subscribedAttributeFilterMap;

            /// <summary>
            /// Indicates mapping for subscribed relationship type filter
            /// </summary>
            [DataMember]
            Dictionary<String, Boolean> subscribedRelationshipTypeFilterMap;

            /// <summary>
            /// Indicates mapping for subscribed relationship attribute filter
            /// </summary>
            [DataMember]
            Dictionary<String, Boolean> subscribedRelationshipAttributeFilterMap;

            /// <summary>
            /// Indicates mapping for subscribed before and after filter
            /// </summary>
            [DataMember]
            Dictionary<String, Boolean> subscribedBeforeAfterFilterMap;

            /// <summary>
            /// Indicates list of merge copy handler subscribed conflict modes
            /// </summary>
            [DataMember]
            List<MergeCopyContext.MergeCopyHandlerConflictMode> subscribedConflictModes;

            /// <summary>
            /// Indicates list of merge copy subscribed options
            /// </summary>
            [DataMember]
            List<MergeCopyOption> subscribedOptions;

            /// <summary>
            /// Indicates list of merge copy handler which will be called at specific points in the processing as per handler call mode 
            /// </summary>
            [DataMember]
            List<IMergeCopyHandler> handlers;

            /// <summary>
            /// Indicates the information about the extension provider for merge copy for implementation of master-staging 1-N extension
            /// </summary>
            [DataMember]
            IMergeCopyEntityExtensionProvider extensionProvider;
 
            /// <summary>
            /// Indicates the information about the caller which is generic data store managed by caller
            /// </summary>
            [DataMember]
            Object[] callerManagedInformation; 

            /// <summary>
            /// Indicates the login name of user account that initiated the merge copy
            /// </summary>
            [DataMember]
            String userName; 

            /// <summary>
            /// Indicates the original action triggering the merge copy, e.g. CheckOut
            /// </summary>
            [DataMember]
            MergeCopySource mergeCopySource;

            /// <summary>
            /// Indicates the conflict mode that the handler can check to find out conflict type
            /// </summary>
            [DataMember]
            MergeCopyHandlerConflictMode currentConflictMode;

            /// <summary>
            /// Indicates the flag which when set, the engine will only process the root entity in entity hierarchy
            /// </summary>
            [DataMember]
            Boolean processOnlyFirstLevelFlag;

            /// <summary>
            /// Indicates the flag which when set, the engine will merge relationships and related entities for all entities in the hierarchy
            /// </summary>
            [DataMember]
            Boolean processRelationshipsFlag;

            /// <summary>
            /// Indicates the flag which when set, the engine will merge relationships and related entities only at first level.
            /// If the processRelationships is also set, processRelationships will be ignored.
            /// If processOnlyFirstLevel is set then it will take precedence.
            /// </summary>
            [DataMember]
            Boolean processRelationshipsOnlyAtFirstLevelFlag;

            /// <summary>
            /// Indicates the flag which when set specifies the engine will use native extension relationships
            /// </summary>
            [DataMember]
            Boolean processExtensionsFlag;

            /// <summary>
            /// Indicates the flag which when set specifies the engine will ignore unmapped attributes and will try to merge
            /// </summary>
            [DataMember]
            Boolean ignoreUnmappedAttributes;

            /// <summary>
            /// Indicates the flag which when set specifies the engine will ignore entities and attributes which are same while creating json 
            /// </summary>
            [DataMember]
            Boolean ignoreIdenticalItemsInJson;

            /// <summary>
            /// Indicates the count that the engine will return specifying number of entities worth of data while creating json 
            /// </summary>
            [DataMember]
            int jsonEntitiesToReturn;  

            /// <summary>
            /// Indicates the count that the engine will return specifying number of entities after offset N (works with jsonEntitiesToReturn) when creating json
            /// Offset starts from 1
            /// </summary>
            [DataMember]
            int jsonEntitiesOffset;

            /// <summary>
            /// Indicates a boolean flag which when set specifies that the engine does not emit json for a node if the target entity does not exist 
            /// </summary>
            [DataMember]
            bool doNotCreateJsonIfTargetEntityOfTreeNodeIsNonExsistent;

            /// <summary>
            /// Indicates a exclusion list of attribute for which the engine will emit json or copy during merge copy
            /// Overrides : copy flags set to true
            /// </summary>
            [DataMember]
            List<String> attributeExclusionList;

            /// <summary>
            /// Indicates a inclusion list of mandatory attribute for which the engine will emit json or copy during merge copy
            /// Overrides : attributeExclusionList, ignoreIdenticalItemsInJson for attributes, copy flags set to false
            /// </summary>
            [DataMember]
            List<String> attributeMandatoryInclusionList;

            #endregion

            #region constructor

            /// <summary>
            /// Constructor which takes user name and merge copy mode as input parameters
            /// </summary>
            /// <param name="userName">Indicates the login name of the user who called merge copy operation</param>
            /// <param name="mode">Indicates the original action triggering the merge copy, e.g. CheckOut</param>
            public MergeCopyCallerContext(String userName, MergeCopySource mode)
            {
                this.subscribedEntityTypeFilterMap = new Dictionary<String, Boolean>();
                this.subscribedAttributeFilterMap = new Dictionary<String, Boolean>();
                this.subscribedRelationshipTypeFilterMap = new Dictionary<String, Boolean>();
                this.subscribedRelationshipAttributeFilterMap = new Dictionary<String, Boolean>();
                this.subscribedBeforeAfterFilterMap = new Dictionary<String, Boolean>();
                this.subscribedConflictModes = new List<MergeCopyContext.MergeCopyHandlerConflictMode>();
                this.subscribedOptions = new List<MergeCopyOption>();
                this.handlers = new List<IMergeCopyHandler>();
                this.userName = userName;
                this.mergeCopySource = mode;
                this.currentConflictMode = MergeCopyHandlerConflictMode.Unknown;
                this.attributeExclusionList = new List<String>();
                this.attributeMandatoryInclusionList = new List<String>();
            }

            #endregion

            /// <summary>
            /// Get the known type for merge copy operation
            /// </summary>
            /// <returns></returns>
            public static Type[] GetKnownTypes()
            {
                List<Type> result = new List<Type>();
                result.Add(typeof(Dictionary<String, Boolean>));
                result.Add(typeof(Dictionary<String, Boolean>).Assembly.GetType("System.Collections.ListDictionaryInternal"));
                return result.ToArray();
            }

            #region Methods used by the caller before MergeCopy

            /// <summary>
            /// Set the entity extension provider to the merge copy context
            /// </summary>
            /// <param name="extensionProvider">Indicates the entity extension provider which to be set to the merge copy context</param>
            public void SetExtensionProvider(IMergeCopyEntityExtensionProvider extensionProvider)
            {
                this.extensionProvider = extensionProvider;
            }

            /// <summary>
            /// Get the entity extension provider for the current merge copy context
            /// </summary>
            /// <returns>Returns the entity extension provider for the current merge copy context</returns>
            public IMergeCopyEntityExtensionProvider GetExtensionProvider()
            {
                return this.extensionProvider;
            }

            /// <summary>
            /// Get the caller managed information for the current merge copy context
            /// </summary>
            /// <returns>Returns the entity extension provider for the current merge copy context</returns>
            public Object[] GetCallerManagedInformation()
            {
                return this.callerManagedInformation;
            }

            /// <summary>
            /// Set the caller managed information to the current merge copy context
            /// </summary>
            /// <param name="callerManagedInformation">Indicates the caller managed information to be set to the current merge copy context</param>
            public void SetCallerManagedInformation(Object[] callerManagedInformation)
            {
                this.callerManagedInformation = callerManagedInformation;
            }

            #region Methods to add a new filter subscription : Called by the caller while setting up MergeCopyContext and before initiating MergeCopy

            /// <summary>
            /// Set the process extensions flag to the current merge copy context
            /// </summary>
            /// <param name="processExtensionsFlag">Indicates the process extensions flag which to be set to the current merge copy context</param>
            public void SetProcessExtensionsFlag(bool processExtensionsFlag)
            {
                this.processExtensionsFlag = processExtensionsFlag;
            }

            /// <summary>
            /// Get the process extension flag from the current merge copy context
            /// </summary>
            /// <returns>Returns the process extension flag from the current merge copy context</returns>
            public bool GetProcessExtensionsFlag()
            {
                return this.processExtensionsFlag;
            }

            /// <summary>
            /// Set the process relationships only at first level flag to the current merge copy context 
            /// </summary>
            /// <param name="processRelationshipsOnlyAtFirstLevelFlag">
            /// Indicates the process relationships only at first level flag to be set to the current merge copy context
            /// </param>
            public void SetProcessRelationshipsOnlyAtFirstLevelFlag(bool processRelationshipsOnlyAtFirstLevelFlag)
            {
                this.processRelationshipsOnlyAtFirstLevelFlag = processRelationshipsOnlyAtFirstLevelFlag;
            }

            /// <summary>
            /// Get the process relationships only at first level flag from the current merge copy context
            /// </summary>
            /// <returns>Returns the process relationships only at first level flag from the current merge copy context</returns>
            public bool GetProcessRelationshipsOnlyAtFirstLevelFlag()
            {
                return this.processRelationshipsOnlyAtFirstLevelFlag;
            }

            /// <summary>
            /// Set the process relationships flag to the current merge copy context
            /// </summary>
            /// <param name="processRelationshipsFlag">Indicates the process relationships value to be set to the current merge copy context </param>
            public void SetProcessRelationshipsFlag(bool processRelationshipsFlag)
            {
                this.processRelationshipsFlag = processRelationshipsFlag;
            }

            /// <summary>
            /// Get the process relationships flag from the current merge copy context
            /// </summary>
            /// <returns>Returns the process relationships flag from the current merge copy context</returns>
            public bool GetProcessRelationshipsFlag()
            {
                return this.processRelationshipsFlag;
            }

            /// <summary>
            /// Set process only root level flag to the current merge copy context
            /// </summary>
            /// <param name="processOnlyRootLevelFlag">Indicates the process only root level flag value to be set to the current merge copy context</param>
            public void SetProcessOnlyRootLevel(bool processOnlyRootLevelFlag)
            {
                this.processOnlyFirstLevelFlag = processOnlyRootLevelFlag;
            }

            /// <summary>
            /// Set process only root level flag to the current nerge copy context
            /// </summary>
            /// <param name="processOnlyRootLevelFlag">Indicates the process only root level flag to be set to the current merge copy context</param>
            public void SetProcessOnlyRootLevelFlag(bool processOnlyRootLevelFlag)
            {
                this.processOnlyFirstLevelFlag = processOnlyRootLevelFlag;
            }

            /// <summary>
            /// Get the process only root level flag from the current merge copy context
            /// </summary>
            /// <returns>Returns the process only root level flag from the current merge copy context</returns>
            public bool GetProcessOnlyRootLevelFlag()
            {
                return this.processOnlyFirstLevelFlag;
            }

            /// <summary>
            /// Add the specified entity type name to the subscribed entity type filter map
            /// </summary>
            /// <param name="entityTypeName">Indicates the entity type name to be added to the subscribed entity type filter map</param>
            public void addEntityTypeFilter(String entityTypeName)
            {
                subscribedEntityTypeFilterMap.Add(entityTypeName.Trim().ToUpper(), true);
            }

            /// <summary>
            /// Add the concatenation of attribute name and group name to the subscribed attribute filter map
            /// </summary>
            /// <param name="name">Indicates the attribute name to be added to the subscribed attribute filter map</param>
            /// <param name="groupName">Indicates the attribute group name to be added to the subscribed attribute filter map</param>
            public void addAttributeFilter(String name, String groupName)
            {
                subscribedAttributeFilterMap.Add((name + groupName).Trim().ToUpper(), true);
            }

            /// <summary>
            /// Add the relationship type name to the subscribed relationship type filter map
            /// </summary>
            /// <param name="relationshipTypeName">Indicates the relationship type name to be added to the subscribed relationship type filter map</param>
            public void addRelationshipTypeFilter(String relationshipTypeName)
            {
                subscribedRelationshipTypeFilterMap.Add(relationshipTypeName.Trim().ToUpper(), true);
            }

            /// <summary>
            /// Add the concatenation of relationship attribute name and its group name to the subscribed relationship attribute filter map
            /// </summary>
            /// <param name="name">Indicates the relationship attribute name to be added to the subscribed relationship attribute filter map</param>
            /// <param name="groupName">Indicates the relationship attribute group name to be added to the subscribed relationship attribute filter map</param>
            public void addRelationshipAttributeFilter(String name, String groupName)
            {
                subscribedRelationshipAttributeFilterMap.Add((name + groupName).Trim().ToUpper(), true);
            }

            /// <summary>
            /// Add before-after event name to the subscribed before after filter map
            /// </summary>
            /// <param name="beforeAfterEventName">Indicates the before-after event name to be added to the subscribed before-after filter map</param>
            public void addBeforeAfterFilter(String beforeAfterEventName)
            {
                subscribedBeforeAfterFilterMap.Add(BEFORE_AFTER_FILTER + beforeAfterEventName, true);
            }
            #endregion

            #region Methods to subscribe to a new conflict or merge copy option called by the caller after setting up CallerContext and before initiating MergeCopy

            /// <summary>
            /// Add the merge copy handler conflict mode to the subscribed merge copy conflict mode
            /// </summary>
            /// <param name="callBackMode">Indicates the merge copy handler conflict mode to be added to the subscribed conflict mode</param>
            /// <returns>Returns a list of the subscribed merge copy conflict mode</returns>
            public List<MergeCopyContext.MergeCopyHandlerConflictMode> AddConflictSubscription(MergeCopyContext.MergeCopyHandlerConflictMode callBackMode)
            {
                this.subscribedConflictModes.Add(callBackMode);
                return this.subscribedConflictModes;
            }

            /// <summary>
            /// Add the merge copy option to the subscribed merge copy option
            /// </summary>
            /// <param name="callOption">Indicates the merge copy option to be added to the subscribed merge copy option</param>
            /// <returns>Returns a list of the subscribed merge copy option</returns>
            public List<MergeCopyContext.MergeCopyOption> AddCallOptionSubscription(MergeCopyContext.MergeCopyOption callOption)
            {
                this.subscribedOptions.Add(callOption);
                return this.subscribedOptions;
            }

            #endregion

            #region Method to add a new handler set before initiating MergeCopy

            /// <summary>
            /// Add the specified merge copy handler to the list of the current merge copy handler
            /// </summary>
            /// <param name="handler">Indicates the merge copy handler to be added to the list of current merge copy handler</param>
            /// <returns></returns>
            public List<IMergeCopyHandler> AddHandler(IMergeCopyHandler handler)
            {
                this.handlers.Add(handler);
                return this.handlers;
            }

            #endregion

            #endregion

            #region Methods used by the caller during MergeCopy

            /// <summary>
            /// Get the current merge copy handler conflict mode
            /// </summary>
            /// <returns>Returns the current merge copy handler conflict mode</returns>
            public MergeCopyContext.MergeCopyHandlerConflictMode GetCurrentConflictMode()
            {
                return this.currentConflictMode;
            }

            #endregion

            #region The caller should not need the following methods. These are used by the engine

            #region Accessor methods called by the enclosing context during MergeCopy

            /// <summary>
            /// Get the merge copy handler subscribed conflict mode from the current merge copy context
            /// </summary>
            /// <returns>Returns the merge copy handler subscribed conflict mode from the current merge copy context</returns>
            public List<MergeCopyContext.MergeCopyHandlerConflictMode> GetHandlerSubscribedConflictModes()
            {
                return this.subscribedConflictModes;
            }

            /// <summary>
            /// Get the merge copy option from the merge copy context
            /// </summary>
            /// <returns>Returns the merge copy option from the merge copy context</returns>
            public List<MergeCopyContext.MergeCopyOption> GetHandlerSubscribedOptions()
            {
                return this.subscribedOptions;
            }

            /// <summary>
            /// Get a list of current merge copy handlers
            /// </summary>
            /// <returns>Returns a list of current merge copy handlers</returns>
            public List<IMergeCopyHandler> GetHandlers()
            {
                return this.handlers;
            }

            /// <summary>
            /// Get the user login name who initiated the merge copy call
            /// </summary>
            /// <returns>Returns the user login name who initiated the merge copy call</returns>
            public String GetUserName()
            {
                return this.userName;
            }

            /// <summary>
            /// Get the merge copy source of the current merge copy context
            /// </summary>
            /// <returns>Returns the merge copy source value of the current merge copy context</returns>
            public MergeCopySource GetSource()
            {
                return this.mergeCopySource;
            }

            #endregion

            #region Accessor methods to check for a filter subscription called by MergeCopyEngine to determine whether to call handler or not

            /// <summary>
            /// Get the value of current subscribed entity type filter map based on the specified entity type name
            /// <param name="entityTypeName">Indicates the entity type name whose value to be returned</param>
            /// <param name="applyLineageEnhancements">Indicates whether to apply lineage enhancement or not</param>
            /// <returns>Returns the current subscribed entity type filter map value based on the specified entity type name.</returns>
            /// </summary>
            public bool isEntityTypeEnabled(String entityTypeName, Boolean applyLineageEnhancements = false)
            {
                if (subscribedEntityTypeFilterMap.Count == 0 && applyLineageEnhancements)
                    return true;

                bool ret;
                subscribedEntityTypeFilterMap.TryGetValue(entityTypeName.Trim().ToUpper(), out ret);
                return ret;
            }

            /// <summary>
            /// Get the value of subscribed attribute filter map based on the specified attribute name and group name 
            /// </summary>
            /// <param name="name">Indicates the attribute name</param>
            /// <param name="groupName">Indicates the attribute group name</param>
            /// <param name="applyLineageEnhancements">Indicates whether to apply lineage enhancement or not</param>
            /// <returns>Returns the value of subscribed attribute filter map based on the specified attribute name and group name </returns>
            public bool isAttributeEnabled(String name, String groupName, Boolean applyLineageEnhancements = false)
            {
                if (subscribedAttributeFilterMap.Count == 0 && applyLineageEnhancements)
                    return true;

                bool ret;
                subscribedAttributeFilterMap.TryGetValue((name + groupName).Trim().ToUpper(), out ret);
                return ret;
            }

            /// <summary>
            /// Get the value of subscribed relationship type filter map based on the relationship type name
            /// </summary>
            /// <param name="relationshipTypeName">indicates the relationship type name</param>
            /// <returns>Returns the value of subscribed relationship type filter map based on the relationship type name</returns>
            public bool isReltionshipTypeEnabled(String relationshipTypeName)
            {
                bool ret;
                subscribedRelationshipTypeFilterMap.TryGetValue(relationshipTypeName.Trim().ToUpper(), out ret);
                return ret;
            }

            /// <summary>
            /// Get the value of subscribed relationship attribute filter map based on the relationship attribute name and group name
            /// </summary>
            /// <param name="name">Indicates the relationship attribute name</param>
            /// <param name="groupName">Indicates the relationship attribute group name</param>
            /// <returns>Returns the value of subscribed relationship attribute filter map based on the relationship attribute name and group name</returns>
            public bool isRelationshipAttributeEnabled(String name, String groupName)
            {
                bool ret;
                subscribedRelationshipAttributeFilterMap.TryGetValue((name + groupName).Trim().ToUpper(), out ret);
                return ret;
            }

            /// <summary>
            /// Get the value of subscribed before and after filter map based on the merge copy current conflict mode
            /// </summary>
            /// <returns>Returns the value of subscribed before and after filter map based on the merge copy current conflict mode</returns>
            public bool isBeforeAfterOptionFilterEnabled()
            {
                bool ret;
                subscribedBeforeAfterFilterMap.TryGetValue(BEFORE_AFTER_FILTER + this.GetCurrentConflictMode(), out ret);
                return ret;
            }

            /// <summary>
            /// Get the merge copy subscribed attribute filter map value
            /// </summary>
            /// <returns>Returns the merge copy subscribed attribute filter map value</returns>
            public Dictionary<String, bool> GetAttributeFilterMap()
            {
                return subscribedAttributeFilterMap;
            }

            /// <summary>
            /// Get the merge copy subscribed entity type filter map value
            /// </summary>
            /// <returns>Returns the merge copy subscribed entity type filter map value</returns>
            public Dictionary<String, bool> GetEntityTypeFilterMap()
            {
                return subscribedEntityTypeFilterMap;
            }            
            
            #endregion

            #region Methods called by the merge copy engine before handler is called

            /// <summary>
            /// Set the merge copy conflict mode to the current merge copy context
            /// </summary>
            /// <param name="conflictMode">Indicates the merge copy handler conflict mode</param>
            public void SetCurrentConflictMode(MergeCopyContext.MergeCopyHandlerConflictMode conflictMode)
            {
                this.currentConflictMode = conflictMode;
            }

            /// <summary>
            /// Set the ignored unmapped attributes flag to the current merge copy context
            /// </summary>
            /// <param name="ignoreUnmappedAttributes">Indicates the ignore unmapped attributes flag value related to merge copy</param>
            public void SetIgnoreUnmappedAttributes(bool ignoreUnmappedAttributes)
            {
                this.ignoreUnmappedAttributes = ignoreUnmappedAttributes;
            }

            /// <summary>
            /// Get the ignore unmapped attributes flag value from the current merge copy context
            /// </summary>
            /// <returns>Returns the ignore unmapped attributes flag value from the current merge copy context</returns>
            public bool GetIgnoreUnmappedAttributes()
            {
                return this.ignoreUnmappedAttributes;
            }

            /// <summary>
            /// Set the value of ignore identification items in json to the current merge copy context
            /// </summary>
            /// <param name="ignoreIdenticalItemsInJson">Indicates the ignore identification items in json value</param>
            public void SetIgnoreIdenticalItemsInJson(bool ignoreIdenticalItemsInJson)
            {
                this.ignoreIdenticalItemsInJson = ignoreIdenticalItemsInJson;
            }

            /// <summary>
            /// Get the ignore identification items in json value from the current merge copy context
            /// </summary>
            /// <returns>Returns the ignore identification items in json value from the current merge copy context</returns>
            public bool GetIgnoreIdenticalItemsInJson()
            {
                return this.ignoreIdenticalItemsInJson;
            }

            /// <summary>
            /// Set the json entities offset value to the current merge copy context
            /// </summary>
            /// <param name="jsonEntitiesOffset">Indicates the merge copy json entities offset value</param>
            public void SetJsonEntitiesOffset(int jsonEntitiesOffset)
            {
                this.jsonEntitiesOffset = jsonEntitiesOffset;
            }

            /// <summary>
            /// Get the json entities offset value from the current merge copy context
            /// </summary>
            /// <returns>Returns the json entities offset value from the current merge copy context</returns>
            public int GetJsonEntitiesOffset()
            {
                return this.jsonEntitiesOffset;
            }

            /// <summary>
            /// Set the merge copy json entities to return value to the current merge copy context
            /// </summary>
            /// <param name="jsonEntitiesToReturn">Indicates the merge copy jason entities to return value</param>
            public void SetJsonEntitiesToReturn(int jsonEntitiesToReturn)
            {
                this.jsonEntitiesToReturn = jsonEntitiesToReturn;
            }

            /// <summary>
            /// Get the merge copy json entities to return value from the current merge copy context
            /// </summary>
            /// <returns>Returns the merge copy json entities to return value from the current merge copy context</returns>
            public int GetJsonEntitiesToReturn()
            {
                return this.jsonEntitiesToReturn;
            }

            /// <summary>
            /// Set the value indicating whether to create json if target entity of tree node exist or not to the current merge copy context
            /// </summary>
            /// <param name="doNotCreateJsonIfTargetEntityOfTreeNodeIsNonExistent">Indicates the value indicating whether to create json if target entity of tree node exists or not</param>
            public void SetDoNotCreateJsonIfTargetEntityOfTreeNodeIsNonExsistentr(bool doNotCreateJsonIfTargetEntityOfTreeNodeIsNonExistent)
            {
                this.doNotCreateJsonIfTargetEntityOfTreeNodeIsNonExsistent = doNotCreateJsonIfTargetEntityOfTreeNodeIsNonExistent;
            }

            /// <summary>
            /// Get the value indicating whether to create json if target entity of tree node exist or not 
            /// </summary>
            /// <returns>Returns the value indicating whether to create json if target entity of tree node exist or not</returns>
            public bool GetDoNotCreateJsonIfTargetEntityOfTreeNodeIsNonExsistent()
            {
                return this.doNotCreateJsonIfTargetEntityOfTreeNodeIsNonExsistent;
            }
            
            /// <summary>
            /// Set the attribute exclusion list to the current merge copy context
            /// </summary>
            /// <param name="attributeExclusionList">Indicates the list of attributes to be excluded for merge copy</param>
            public void SetAttributeExclusionList(List<String> attributeExclusionList)
            {
                this.attributeExclusionList = attributeExclusionList;
            }

            /// <summary>
            /// Get the list of attribute to be excluded for merge copy
            /// </summary>
            /// <returns>Returns the list of attribute to be exclided for merge copy</returns>
            public List<String> GetAttributeExclusionList()
            {
                return this.attributeExclusionList;
            }

            /// <summary>
            /// Set the mandatory attribute in the inclusion list of the merge copy 
            /// </summary>
            /// <param name="attributeMandatoryInclusionList">Indicates a list of mandatory attributes to be included</param>
            public void SetAttributeMandatoryInclusionList(List<String> attributeMandatoryInclusionList)
            {
                this.attributeMandatoryInclusionList = attributeMandatoryInclusionList;
            }

            /// <summary>
            /// Get a list of mandatory attribute to be included from the current merge copy
            /// </summary>
            /// <returns>Returns a list of mandatory attribute to be included from the current merge copy</returns>
            public List<String> GetAttributeMandatoryInclusionList()
            {
                return this.attributeMandatoryInclusionList;
            }

            #endregion

            #endregion
        }

        #endregion

        #region Enums and Class for Context

        #region MergeCopyOutputStatus set by the engine into the context

        /// <summary>
        /// Indicates the output status from MergeCopy operation
        /// </summary>
        [DataContract(Name = "MergeCopyOutputStatus")]
        public enum MergeCopyOutputStatus
        {
            /// <summary>
            /// Indicates merge copy output status to be fine
            /// </summary>
            [EnumMember]
            OK = 1,

            /// <summary>
            /// Indicates error as the merge copy output 
            /// </summary>
            [EnumMember]
            Error,

            /// <summary>
            /// Indicates unknown
            /// </summary>
            [EnumMember]
            Unknown
        }
        #endregion

        /// <summary>
        /// The class that holds all contextual information for MergeCopy, and in turn the Handler callback
        /// </summary>
        [DataContract]
        [KnownType(typeof(MergeCopyContext.MergeCopyHandlerConflictMode))]
        [KnownType(typeof(MergeCopyContext.MergeCopySource))]
        [KnownType(typeof(MergeCopyContext.MergeCopyOutputStatus))]
        [KnownType(typeof(MergeCopyContext.MergeCopyOption))]
        [KnownType(typeof(MergeCopyContext.MergeCopyJsonMarshallerState))]
        [KnownType(typeof(MergeCopyContext.MergeCopyInternalHandler))]
        [KnownType(typeof(List<MergeCopyContext.MergeCopyHandlerConflictMode>))]
        [KnownType(typeof(Dictionary<String, Boolean>))]
        [KnownType(typeof(List<MergeCopyOption>))]
        [KnownType(typeof(MergeCopyCallerContext))]
        [KnownType(typeof(MergeCopyConflictContext))]
        [KnownType(typeof(MergeCopyJsonMarshallerState))]
        [KnownType(typeof(SurvivorshipRuleStrategyPriorityCollection))]
        [KnownType(typeof(StringBuilder))]
        public class Context
        {
            #region Context data

            /// <summary>
            /// Indicates the merge copy output status from the merge copy context
            /// </summary>
            [DataMember]
            MergeCopyOutputStatus outputStatus;                 // Set by MergeCopy

            /// <summary>
            /// Indicates XML with delta output.This is populated only if the corresponding GenerateDeltaXMLOnly flag enum was set
            /// </summary>
            [DataMember]
            StringBuilder outputDelta;

            /// <summary>
            /// Indicates the merge copy caller context specifying the name of application and the module that are performing the action
            /// </summary>
            [DataMember]
            MergeCopyCallerContext callerContext;

            /// <summary>
            /// Indicates the merge copy conflict context
            /// </summary>
            [DataMember]
            MergeCopyConflictContext conflictContext;

            /// <summary>
            /// Indicates the merge copy audit handler holding audit information cache for a single merge copy call initiated by merge copy facade
            /// </summary>
            [DataMember]
            Object auditHandler; 

            /// <summary>
            /// Indicates merge copy json marshaller state
            /// </summary>
            [DataMember]
            MergeCopyJsonMarshallerState marshallerState;

            /// <summary>
            /// Indicates a boolean flag specifying whether to load the internal auditRefId array
            /// </summary>
            [DataMember]
            public bool captureAuditRefIds;

            /// <summary>
            /// Indicates a boolean flag specifying whether to override native implementation
            /// </summary>
            [DataMember]
            public bool overrideNativeImplementation;

            /// <summary>
            /// Indicates a boolean flag specifying whether to call custom handler or not
            /// </summary>
            [DataMember]
            public bool neverCallCustomHandler;

            /// <summary>
            /// Indicates a boolean flag specifying whether to stop core event generation or not
            /// </summary>
            [DataMember]
            public bool stopCoreEventGeneration;

            /// <summary>
            /// Indicates a boolean flag specifying whether to stop core entity load time event generation or not
            /// </summary>
            [DataMember]
            public bool stopCoreEntityLoadTimeEventGeneration;

            /// <summary>
            /// Indicates a boolean flag specifying whether to process attributes or not
            /// </summary>
            [DataMember]
            public bool doNotProcessAttributesFlag;            

            /// <summary>
            /// Indicates a boolean flag specifying behavior when destination exist i.e. whether to flush and fill the destination or not
            /// </summary>
            [DataMember]
            public bool flushAndFillEntityIfDestinationExists;

            /// <summary>
            /// Indicates a boolean flag specifying behavior when destination attribute exist i.e, whether to flush and fill destination attribute or not
            /// </summary>
            [DataMember]
            public bool flushAndFillAttributeIfDestinationExists;

            /// <summary>
            /// Indicates a boolean flag specifying behavior when source attribute does not have values or has empty values
            /// </summary>
            [DataMember]
            public bool processEmptySourceAttributes;

            /// <summary>
            /// Indicates a boolean flag whether to merge by timestamp or not
            /// </summary>
            [DataMember]
            public bool mergeByTimestamp;

            /// <summary>
            /// Indicates a boolean flag indicating whether to generate delta change only between source and target
            /// </summary>
            [DataMember]
            public bool generateDeltaOnly;

            /// <summary>
            /// Indicates a boolean flag indicating whether to stop at first conflict between source and target or not
            /// </summary>
            [DataMember]
            public bool stopAtFirstConflict;

            /// <summary>
            /// Indicates a boolean flag specifying whether to copy relationship attributes from source to target or not
            /// </summary>
            [DataMember]
            public bool copyRelationshipAttributes;

            /// <summary>
            /// Indicates a boolean flag specifying whether to copy relationships from source to target or not
            /// </summary>
            [DataMember]
            public bool copyRelationships;

            /// <summary>
            /// Indicates a boolean flag specifying whether to copy system attributes from source to target or not
            /// </summary>
            [DataMember]
            public bool copySystemAttributes;

            /// <summary>
            /// Indicates a boolean flag specifying whether to copy technical attributes from source to target or not
            /// </summary>
            [DataMember]
            public bool copyTechnicalAttributes;

            /// <summary>
            /// Indicates a boolean flag specifying whether to copy common attributes from source to target or not
            /// </summary>
            [DataMember]
            public bool copyCommonAttributes;

            /// <summary>
            /// Indicates a boolean flag specifying whether to copy only required attribute
            /// </summary>
            [DataMember]
            public bool copyRequiredAttributesOnly;

            /// <summary>
            /// Indicates a boolean flag specifying whether to copy complex attributes from source to target or not
            /// </summary>
            [DataMember]
            public bool copyComplexAttributes;

            /// <summary>
            /// Indicates a boolean flag specifying whether to compress json buffer or not
            /// </summary>
            [DataMember]
            public bool compressJsonBuffer;

            /// <summary>
            /// Indicates a boolean flag specifying whether to apply lineage enhancement or not
            /// </summary>
            [DataMember]
            public bool applyLineageEnhancements;

            /// <summary>
            /// Indicates a boolean flag specifying whether to skip saving or not
            /// </summary>
            [DataMember] 
            public bool skipSaving;

            /// <summary>
            /// Indicates a collection of survivorship rule strategy with priority
            /// </summary>
            [DataMember] 
            public SurvivorshipRuleStrategyPriorityCollection strategyPriority;

            /// <summary>
            /// Indicates a collection of merge copy strategy
            /// </summary>
            [DataMember] 
            public CollectionStrategy collectionStrategy;

            /// <summary>
            /// Indicates whether to skip target entity loading or not
            /// </summary>
            [DataMember]
            public Boolean skipTargetEntityLoading;

            /// <summary>
            /// Indicates locales
            /// </summary>
            [DataMember]
            public Collection<LocaleEnum> locales;

            /// <summary>
            /// Indicates a boolean flag specifying whether to stop core event generation or not for source entity extension parent update
            /// </summary>
            [DataMember]
            public Boolean stopCoreEventGenerationOnSourceEntityExtensionParentUpdate;
            
            /// <summary>
            /// Indicates per engine call cache which is internal to the engine
            /// </summary>
            private Object entityCache;

            /// <summary>
            /// Indicates a boolean flag specifying whether I(source) to O(target) merge allowed or not
            /// </summary>
            [DataMember]
            public Boolean allowInheritedToOverriddenMerging;

            #endregion

            /// <summary>
            /// Indicates context constructor with merge copy caller context as its input parameter
            /// </summary>
            /// <param name="callerContext">Indicates the name of application and the module that are performing the action</param>
            public Context(MergeCopyCallerContext callerContext)
            {
                this.outputDelta = new StringBuilder();
                this.outputStatus = MergeCopyOutputStatus.Unknown;
                this.callerContext = callerContext;
                this.marshallerState = new MergeCopyJsonMarshallerState();
                this.ProcessOptions();
                this.SetDefaultStrategyPriority();
            }

            #region Getter/Setter Methods to call during engine run

            /// <summary>
            /// Get entity cache value of the current merge copy context
            /// </summary>
            /// <returns></returns>
            public Object GetEntityCache()
            {
                return this.entityCache;
            }

            /// <summary>
            /// Set entity cache to the current merge copy context
            /// </summary>
            /// <param name="entityCache">Indicates entity cache to set to the current merge copy context</param>
            public void SetEntityCache(Object entityCache)
            {
                this.entityCache = entityCache;
            }

            /// <summary>
            /// Get conflict context of the current merge copy context
            /// </summary>
            /// <returns>Returns conflict context of the current merge copy context</returns>
            public MergeCopyConflictContext GetConflictContext()
            {
                return this.conflictContext;
            }

            /// <summary>
            /// Set merge copy conflict context to the current merge copy context
            /// </summary>
            /// <param name="conflictContext">Indicates the merge copy conflict context to be set to the curent merge copy context</param>
            public void SetConflictContext(MergeCopyConflictContext conflictContext)
            {
                this.conflictContext = conflictContext;
            }

            /// <summary>
            /// Get the caller context of the current merge copy context
            /// </summary>
            /// <returns>Returns the caller context of the current merge copy context</returns>
            public MergeCopyCallerContext GetCallerContext()
            {
                return this.callerContext;
            }

            /// <summary>
            /// Get StopCoreEntityLoadTimeEventGeneration value from the current merge copy context
            /// </summary>
            /// <returns></returns>
            public bool GetStopCoreEntityLoadTimeEventGeneration()
            {
                return this.stopCoreEntityLoadTimeEventGeneration;
            }

            /// <summary>
            /// Set StopCoreEntityLoadTimeEventGeneration value to the current merge copy context
            /// </summary>
            /// <param name="stopCoreEntityLoadTimeEventGeneration"></param>
            public void SetStopCoreEntityLoadTimeEventGeneration(bool stopCoreEntityLoadTimeEventGeneration)
            {
                this.stopCoreEntityLoadTimeEventGeneration = stopCoreEntityLoadTimeEventGeneration;
            }

            /// <summary>
            /// Get StopCoreEventGeneration value from the current merge copy context
            /// </summary>
            /// <returns>Returns StopCoreEventGeneration value from the current merge copy context</returns>
            public bool GetStopCoreEventGeneration()
            {
                return this.stopCoreEventGeneration;
            }

            /// <summary>
            /// Set StopCoreEventGeneration to the current merge copy context
            /// </summary>
            /// <param name="stopCoreEventGeneration">Indicates the StopCoreEventGeneration to be set to the current merge copy context</param>
            public void SetStopCoreEventGeneration(bool stopCoreEventGeneration)
            {
                this.stopCoreEventGeneration = stopCoreEventGeneration;
            }

            /// <summary>
            /// Get DoNotProcessAttributes flag from the current merge copy context 
            /// </summary>
            /// <returns>Returns DoNotProcessAttributes flag from the current merge copy context</returns>
            public bool GetDoNotProcessAttributesFlag()
            {
                return this.doNotProcessAttributesFlag;
            }

            /// <summary>
            /// Set DoNotProcessAttributes flag to the current merge copy context
            /// </summary>
            /// <param name="doNotProcessAttributesFlag">Indicates DoNotProcessAttributes flag to be set to the current merge copy context</param>
            public void SetDoNotProcessAttributesFlag(bool doNotProcessAttributesFlag)
            {
                this.doNotProcessAttributesFlag = doNotProcessAttributesFlag;
            }

            /// <summary>
            /// Get audit handler from the current merge copy context
            /// </summary>
            /// <returns>Returns audit handler from the current merge copy context</returns>
            public Object GetAuditHandler()
            {
                return this.auditHandler;
            }

            /// <summary>
            /// Set audit handler to the current merge copy context
            /// </summary>
            /// <param name="handler">Indicates the audit handler to be set to the current merge copy context</param>
            public void SetAuditHandler(Object handler)
            {
                this.auditHandler = handler;
            }

            /// <summary>
            /// Get lineage enhancement flag from the current merge copy context
            /// </summary>
            /// <returns>Returns lineage enhancement flag from the current merge copy context</returns>
            public bool GetLineageEnhancementsFlag()
            {
                return this.applyLineageEnhancements;
            }

            /// <summary>
            /// Set lineage enhancement flag to the current merge copy context
            /// </summary>
            /// <param name="applyLineageEnhancements">Indicates apply lineage enhancement flag which is to be set to the current merge copy context</param>
            public void SetLineageEnhancementsFlag(bool applyLineageEnhancements)
            {
                this.applyLineageEnhancements = applyLineageEnhancements;
            }

            /// <summary>
            /// Get skip saving flag value from the current merge copy context
            /// </summary>
            /// <returns>Returns skip saving flag value from the current merge copy context</returns>
            public bool GetSkipSavingFlag()
            {
                return this.skipSaving;
            }

            /// <summary>
            /// Set skip saving flag to the current merge copy context
            /// </summary>
            /// <param name="skipSaving">Indicates the skip saving flag to be set to the current merge copy context</param>
            public void SetSkipSavingFlag(bool skipSaving)
            {
                this.skipSaving = skipSaving;
            }

            /// <summary>
            /// Get strategy priority from the current merge copy context
            /// </summary>
            /// <returns>Returns strategy priority from the current merge copy context</returns>
            public SurvivorshipRuleStrategyPriorityCollection GetStrategyPriority()
            {
                return this.strategyPriority;
            }

            /// <summary>
            /// Set strategy priority to the current merge copy context
            /// </summary>
            /// <param name="strategyPriority">Indicates the strategy priority to be set to the current merge copy context</param>
            public void SetStrategyPriority(SurvivorshipRuleStrategyPriorityCollection strategyPriority)
            {
                this.strategyPriority = strategyPriority;
            }

            /// <summary>
            /// Get collection strategy from the current merge copy context
            /// </summary>
            /// <returns>Returns collection strategy from the current merge copy context</returns>
            public CollectionStrategy GetCollectionStrategy() 
            {
                return this.collectionStrategy;
            }

            /// <summary>
            /// Set collection strategy to the current merge copy context
            /// </summary>
            /// <param name="collectionStrategy">Indicates the collection strategy to be set the merge copy context</param>
            public void SetCollectionStrategy(CollectionStrategy collectionStrategy)
            {
                this.collectionStrategy = collectionStrategy;
            }

            /// <summary>
            /// Get SkipTargetEntityLoading flag from the current merge copy context
            /// </summary>
            /// <returns>Returns SkipTargetEntityLoading flag from the current merge copy context</returns>
            public Boolean GetSkipTargetEntityLoadingFlag()
            {
                return this.skipTargetEntityLoading;
            }

            /// <summary>
            /// Set SkipTargetEntityLoading flag to the current merge copy context
            /// </summary>
            /// <param name="skipTargetEntityLoading">Indicates SkipTargetEntityLoading flag to the current merge copy context</param>
            public void SetSkipTargetEntityLoadingFlag(Boolean skipTargetEntityLoading)
            {
                this.skipTargetEntityLoading = skipTargetEntityLoading;
            }

            /// <summary>
            /// Get locale value from the current merge copy context
            /// </summary>
            /// <returns>Returns locale value from the current merge copy context</returns>
            public LocaleEnum GetLocale()
            {
                if (!this.locales.IsNullOrEmpty())
                {
                    return this.locales.FirstOrDefault();
                }

                return LocaleEnum.UnKnown;
            }

            /// <summary>
            /// Set locale value to the current merge copy context
            /// </summary>
            /// <param name="locale">Indicates the locale value to be set to the current merge copy context</param>
            public void SetLocale(LocaleEnum locale)
            {
                this.locales = new Collection<LocaleEnum>(){locale};
            }

            /// <summary>
            /// Get locale values from the current merge copy context
            /// </summary>
            /// <returns>Returns locale values from the current merge copy context</returns>
            public Collection<LocaleEnum> GetLocales()
            {
                return this.locales ?? new Collection<LocaleEnum>(){LocaleEnum.UnKnown};
            }

            /// <summary>
            /// Set locale value to the current merge copy context
            /// </summary>
            /// <param name="locales">Indicates the locale value to be set to the current merge copy context</param>
            public void SetLocales(Collection<LocaleEnum> locales)
            {
                this.locales = locales;
            }

            private void ProcessOptions()
            {
                foreach (MergeCopyOption option in callerContext.GetHandlerSubscribedOptions())
                {

                    if (option.Equals(MergeCopyOption.FlushAndFillEntityIfDestinationExists))
                    {
                        flushAndFillEntityIfDestinationExists = true;
                        continue;
                    }
                    if (option.Equals(MergeCopyOption.FlushAndFillAttributeIfDestinationExists))
                    {
                        flushAndFillAttributeIfDestinationExists = true;
                        continue;
                    }
                    if (option.Equals(MergeCopyOption.ProcessEmptySourceAttributes))
                    {
                        processEmptySourceAttributes = true;
                        continue;
                    }
                    if (option.Equals(MergeCopyOption.MergeByTimestamp))
                    {
                        mergeByTimestamp = true;
                        continue;
                    }
                    if (option.Equals(MergeCopyOption.GenerateDeltaOnly))
                    {
                        generateDeltaOnly = true;
                        continue;
                    }
                    if (option.Equals(MergeCopyOption.StopAtFirstConflict))
                    {
                        stopAtFirstConflict = true;
                        continue;
                    }
                    if (option.Equals(MergeCopyOption.CopyRelationshipAttributes))
                    {
                        copyRelationshipAttributes = true;
                        continue;
                    }
                    if (option.Equals(MergeCopyOption.CopyRelationships))
                    {
                        copyRelationships = true;
                        continue;
                    }
                    if (option.Equals(MergeCopyOption.CopySystemAttributes))
                    {
                        copySystemAttributes = true;
                        continue;
                    }
                    if (option.Equals(MergeCopyOption.CopyTechnicalAttributes))
                    {
                        copyTechnicalAttributes = true;
                        continue;
                    }
                    if (option.Equals(MergeCopyOption.CopyCommonAttributes))
                    {
                        copyCommonAttributes = true;
                        continue;
                    }
                    if (option.Equals(MergeCopyOption.CopyRequiredAttributesOnly))
                    {
                        copyRequiredAttributesOnly = true;
                        continue;
                    }
                    if (option.Equals(MergeCopyOption.CopyComplexAttributes))
                    {
                        copyComplexAttributes = true;
                        continue;
                    }
                    if (option.Equals(MergeCopyOption.CompressJsonBuffer))
                    {
                        compressJsonBuffer = true;
                        continue;
                    }
                    if (option.Equals(MergeCopyOption.AllowInheritedToOverriddenMerging))
                    {
                        allowInheritedToOverriddenMerging = true;
                        continue;
                    }
                }
            }

            private void SetDefaultStrategyPriority()
            {
                this.SetStrategyPriority(new SurvivorshipRuleStrategyPriorityCollection()
                {
                    new SurvivorshipRuleStrategyPriority()
                    {
                        Priority = 1,
                        Strategy = RulesetStrategy.Source
                    },
                    new SurvivorshipRuleStrategyPriority()
                    {
                        Priority = 2,
                        Strategy = RulesetStrategy.Recent
                }
                });
            }

            /// <summary>
            /// Set merge copy output status to the current merge copy context
            /// </summary>
            /// <param name="status">Indicates the status to be set to the current merge copy context</param>
            public void SetStatus(MergeCopyOutputStatus status)
            {
                this.outputStatus = status;
            }

            /// <summary>
            /// Get merge copy output status from the current merge copy context
            /// </summary>
            /// <returns>Returns merge copy output status from the current merge copy context</returns>
            public MergeCopyOutputStatus GetStatus()
            {
                return this.outputStatus;
            }

            /// <summary>
            /// Reset output delta change to the current merge copy context
            /// </summary>
            public void ResetDelta()
            {
                this.outputDelta = new StringBuilder();
            }

            /// <summary>
            /// Get output delta change from the current merge copy context
            /// </summary>
            /// <returns>Returns output delta change from the current merge copy context</returns>
            public StringBuilder GetDelta()
            {
                return this.outputDelta;
            }

            /// <summary>
            /// Set output delta change to the current merge copy context
            /// </summary>
            /// <param name="delta">Indicates the output delta change to be set to the current merge copy context</param>
            public void SetDelta(StringBuilder delta)
            {
                this.outputDelta = delta;
            }

            /// <summary>
            /// Append the specified string to the output delta of the current merge copy context
            /// </summary>
            /// <param name="str">Indicates the string to append to the output delta of the current merge copy context</param>
            /// <returns>Returns the output delta string after appending the specified string</returns>
            public StringBuilder AppendToDelta(String str)
            {
                return this.outputDelta.Append(str);
            }

            /// <summary>
            /// Get the merge copy handler conflict mode from the caller context
            /// </summary>
            /// <returns>Returns the merge copy handler conflict mode from the caller context</returns>
            public List<MergeCopyContext.MergeCopyHandlerConflictMode> GetHandlerSubscribedConflictModes()
            {
                return callerContext.GetHandlerSubscribedConflictModes();
            }

            /// <summary>
            /// Get the merge copy option from the current merge copy caller context
            /// </summary>
            /// <returns>REeturns the merge copy option from the current merge copy caller context</returns>
            public List<MergeCopyContext.MergeCopyOption> GetHandlerSubscribedOptions()
            {
                return callerContext.GetHandlerSubscribedOptions();
            }

            /// <summary>
            /// Get merge copy handlers from the current merge copy caller context
            /// </summary>
            /// <returns>Retuns merge copy handlers from the current merge copy caller context</returns>
            public List<IMergeCopyHandler> GetHandlers()
            {
                return callerContext.GetHandlers();
            }

            /// <summary>
            /// Set the merge copy handler conflict mode to the current merge copy context
            /// </summary>
            /// <param name="conflictMode">Indicates the merge copy handler conflict mode to be set to the current merge copy context</param>
            public void SetCurrentConflictMode(MergeCopyContext.MergeCopyHandlerConflictMode conflictMode)
            {
                callerContext.SetCurrentConflictMode(conflictMode);
            }

            /// <summary>
            /// Get the merge copy handler conflict mode from the current merge copy caller context
            /// </summary>
            /// <returns>Returns the merge copy handler conflict mode from the current merge copy caller context</returns>
            public MergeCopyContext.MergeCopyHandlerConflictMode GetCurrentConflictMode()
            {
                return callerContext.GetCurrentConflictMode();
            }

            /// <summary>
            /// Get StopCoreEventGenerationOnSourceEntityExtensionParentUpdate value from the current merge copy context
            /// </summary>
            /// <returns>Returns StopCoreEventGenerationOnSourceEntityExtensionParentUpdate value from the current merge copy context</returns>
            public Boolean GetStopCoreEventGenerationOnSourceEntityExtensionParentUpdate()
            {
                return this.stopCoreEventGenerationOnSourceEntityExtensionParentUpdate;
            }

            /// <summary>
            /// Set StopCoreEventGenerationOnSourceEntityExtensionParentUpdate to the current merge copy context
            /// </summary>
            /// <param name="stopCoreEventGenerationOnSourceEntityExtensionParentUpdate">
            /// Indicates the StopCoreEventGenerationOnSourceEntityExtensionParentUpdate to be set to the current merge copy context
            /// </param>
            public void SetStopCoreEventGenerationOnSourceEntityExtensionParentUpdate(Boolean stopCoreEventGenerationOnSourceEntityExtensionParentUpdate)
            {
                this.stopCoreEventGenerationOnSourceEntityExtensionParentUpdate = stopCoreEventGenerationOnSourceEntityExtensionParentUpdate;
            }

            #region MergeCopy JSON Streaming State Getter/Setters

            /// <summary>
            /// Get the complex attribute conflict buffer information from the current merge copy marshaller state
            /// </summary>
            /// <returns></returns>
            public StringBuilder GetComplexAttributeConflictBuffer()
            {
                return this.marshallerState.complexAttributeConflictBuffer;
            }

            /// <summary>
            /// Set the complex attribute conflict buffer.
            /// </summary>
            /// <param name="complexAttributeConflictBuffer">Indicates the complex attribute conflict buffer value of merge copy marshaller state</param>
            public void SetComplexAttributeConflictBuffer(StringBuilder complexAttributeConflictBuffer)
            {
                this.marshallerState.complexAttributeConflictBuffer = complexAttributeConflictBuffer;
            }

            /// <summary>
            /// Get whether the marshaller state of merge copy is in preamble or not
            /// </summary>
            /// <returns>Returns the value indicating whether the marshaller state of merge copy is in preamble or not</returns>
            public bool GetJsonStreamerStateInPreamble()
            {
                return this.marshallerState.isInPreamble;
            }

            /// <summary>
            /// Get whether the marshaller state is in entity conflict array or not
            /// </summary>
            /// <returns>Returns boolean value indicating whether the marshaller state is in entity conflict array or not</returns>
            public bool GetJsonStreamerStateInEntityConflictArray()
            {
                return this.marshallerState.isInEntityConflictArray;
            }

            /// <summary>
            /// Get whether the marshaller state of merge copy is in attribute conflict array or not
            /// </summary>
            /// <returns>Returns boolean value indicating whether the marshaller state of merge copy is in attribute conflict array or not</returns>
            public bool GetJsonStreamerStateInAttributeConflictsArray()
            {
                return this.marshallerState.isInAttributeConflictsArray;
            }

            /// <summary>
            /// Get whether the marshaller state of merge copy is in relationship conflict array or not
            /// </summary>
            /// <returns>Returns boolean value indicating whether the marshaller state of merge copy is in relationship conflict array or not</returns>
            public bool GetJsonStreamerStateInRelationshipConflictsArray()
            {
                return this.marshallerState.isInRelationshipConflictsArray;
            }

            /// <summary>
            /// Get whether the marshaller state of merge copy is in entity conflict element or not
            /// </summary>
            /// <returns>Returns boolean value indicating whether the marshaller state of merge copy is in entity conflict element or not</returns>
            public bool GetJsonStreamerStateInEntityConflictElement()
            {
                return this.marshallerState.isInEntityConflictElement;
            }

            /// <summary>
            /// Get whether the marshaller state of merge copy is in relationship conflict element or not
            /// </summary>
            /// <returns>Returns boolean value indicating whether the marshaller state of merge copy is in relationship conflict element or not</returns>
            public bool GetJsonStreamerStateInRelationshipConflictsElement()
            {
                return this.marshallerState.isInRelationshipConflictsElement;
            }

            /// <summary>
            /// Set the merge copy marshaller state is in preamble value to the current merge copy context
            /// </summary>
            /// <param name="isInPreamble">Indicates whether the marshaller state is in preamble</param>
            public void SetJsonStreamerStateInPreamble(bool isInPreamble)
            {
                this.marshallerState.isInPreamble = isInPreamble;
            }

            /// <summary>
            /// Set the merge copy marshaller state entity conflict array value to the current merge copy context
            /// </summary>
            /// <param name="isInEntityConflictArray">Indicates whether the current merge copy marshaller state is in entity conflict array</param>
            public void SetJsonStreamerStateInEntityConflictArray(bool isInEntityConflictArray)
            {
                this.marshallerState.isInEntityConflictArray = isInEntityConflictArray;
            }

            /// <summary>
            /// Set the attribute conflict array boolean value to the current merge copy context
            /// </summary>
            /// <param name="isInAttributeConflictsArray">Indicates whether the current merge copy marshaller state is in attribute conflict array</param>
            public void SetJsonStreamerStateInAttributeConflictsArray(bool isInAttributeConflictsArray)
            {
                this.marshallerState.isInAttributeConflictsArray = isInAttributeConflictsArray;
            }

            /// <summary>
            /// Set the relationship conflict array boolean value to the current merge copy context
            /// </summary>
            /// <param name="isInRelationshipConflictsArray">Indicates whether the current merge copy marshaller state is in relationship conflict array</param>
            public void SetJsonStreamerStateInRelationshipConflictsArray(bool isInRelationshipConflictsArray)
            {
                this.marshallerState.isInRelationshipConflictsArray = isInRelationshipConflictsArray;
            }

            /// <summary>
            /// Set the entity conflict element boolean value to the current merge copy context
            /// </summary>
            /// <param name="isInEntityConflictElement">Indicates whether the current merge copy marshaller state is in entity conflict element</param>
            public void SetJsonStreamerStateInEntityConflictElement(bool isInEntityConflictElement)
            {
                this.marshallerState.isInEntityConflictElement = isInEntityConflictElement;
            }
           
            /// <summary>
            /// Set the relationship conflict element boolean value to the current merge copy context
            /// </summary>
            /// <param name="isInRelationshipConflictsElement">Indicates whether the current merge copy marshaller state is in relationship conflict element</param>
            public void SetJsonStreamerStateInRelationshipConflictsElement(bool isInRelationshipConflictsElement)
            {
                this.marshallerState.isInRelationshipConflictsElement = isInRelationshipConflictsElement;
            }

            #endregion

            #endregion
        }

        #endregion

        #region Class for conflict instance

        /// <summary>
        /// Class storing conflict information during merge copy
        /// </summary>
        [DataContract]
        [KnownType(typeof(Entity))]
        [KnownType(typeof(Attribute))]
        [KnownType(typeof(RelationshipType))]
        public class MergeCopyConflictContext
        {
            /// <summary>
            /// Indicates a list of user managed node information
            /// </summary>
            [DataMember]
            public List<Object> userManagedNodeInformation;

            /// <summary>
            /// Indicates a source entity as a conflict context
            /// </summary>
            [DataMember]
            public IEntity sourceEntity = null;

            /// <summary>
            /// Indicates a target entity as a conflict context
            /// </summary>
            [DataMember]
            public IEntity targetEntity = null;

            /// <summary>
            /// Indicates a source attribute as a conflict context
            /// </summary>
            [DataMember]
            public IAttribute sourceAttribute = null;

            /// <summary>
            /// Indicates a target attribute as a conflict context
            /// </summary>
            [DataMember]
            public IAttribute targetAttribute = null;

            /// <summary>
            /// Indicates a attribute model as a conflict context
            /// </summary>
            [DataMember]
            public IAttributeModel attributeModel = null;

            /// <summary>
            /// Indicates a source relationship as a conflict context
            /// </summary>
            [DataMember]
            public IRelationship sourceRelationship = null;

            /// <summary>
            /// Indicates a target relationship as a conflict context
            /// </summary>
            [DataMember]
            public IRelationship targetRelationship = null;

            /// <summary>
            /// Indicates whether to skip engine processing for current node or not
            /// </summary>
            [DataMember]
            public bool skipEngineProcessingForCurrentNode = false;

            /// <summary>
            /// Indicates whether the two attribute values are different or not
            /// </summary>
            [DataMember]
            public bool areAttributeValuesDifferent = false;

            /// <summary>
            /// Indicates a collection strategy
            /// </summary>
            [DataMember]
            public CollectionStrategy collectionStrategy = CollectionStrategy.Unknown;

            /// <summary>
            /// Indicates a target object type
            /// </summary>
            [DataMember]
            public TargetObjectType targetObjectType = TargetObjectType.Unknown;

            /// <summary>
            /// Constructor with source and target entity, source and target attribute, attribute model, 
            /// source and target relationship and parent merge context as input paramaters
            /// Some members could set to be null by the caller depending on nature of conflict
            /// </summary>
            /// <param name="sourceEntity">Indicates source entity</param>
            /// <param name="targetEntity">Indicates target entity</param>
            /// <param name="sourceAttribute">Indicates source attribute</param>
            /// <param name="targetAttribute">Indicates target attribute</param>
            /// <param name="attributeModel">Indicates attribute model instance</param>
            /// <param name="sourceRelationship">Indicates source relationship</param>
            /// <param name="targetRelationship">Indicates target relationship</param>
            /// <param name="parentMergeContext">Indicates merge context of parent entity</param>
            public MergeCopyConflictContext(
                            IEntity sourceEntity,
                            IEntity targetEntity,
                            IAttribute sourceAttribute,
                            IAttribute targetAttribute,
                            IAttributeModel attributeModel,
                            IRelationship sourceRelationship,
                            IRelationship targetRelationship,
                            MergeCopyContext.Context parentMergeContext)
            {
                this.sourceEntity = sourceEntity;
                this.targetEntity = targetEntity;
                this.sourceAttribute = sourceAttribute;
                this.targetAttribute = targetAttribute;
                this.attributeModel = attributeModel;
                this.sourceRelationship = sourceRelationship;
                this.targetRelationship = targetRelationship;
                this.userManagedNodeInformation = new List<Object>();
            }

            private MergeCopyConflictContext(IEntity sourceEntity, IEntity targetEntity)
            {
                this.sourceEntity = sourceEntity;
                this.targetEntity = targetEntity;
            }

            private MergeCopyConflictContext(
                IEntity sourceEntity,
                IEntity targetEntity,
                IAttribute sourceAttribute,
                IAttribute targetAttribute,
                IAttributeModel attributeModel)
            {
                this.sourceEntity = sourceEntity;
                this.targetEntity = targetEntity;
                this.sourceAttribute = sourceAttribute;
                this.targetAttribute = targetAttribute;
                this.attributeModel = attributeModel;
            }

            private MergeCopyConflictContext(
                IEntity sourceEntity,
                IEntity targetEntity,
                IRelationship sourceRelationship,
                IRelationship targetRelationship)
            {
                this.sourceEntity = sourceEntity;
                this.targetEntity = targetEntity;
                this.sourceRelationship = sourceRelationship;
                this.targetRelationship = targetRelationship;
            }

            /// <summary>
            /// Defines whether current conflict type related to Entity
            /// </summary>
            public static Boolean IsEntityConflict(MergeCopyHandlerConflictMode conflictMode)
            {
                return conflictMode == MergeCopyHandlerConflictMode.EntityConflictDestinationNonexistent ||
                       conflictMode == MergeCopyHandlerConflictMode.EntityConflictSourceNewer ||
                       conflictMode == MergeCopyHandlerConflictMode.EntityConflictSourceNonexistent ||
                       conflictMode == MergeCopyHandlerConflictMode.EntityConflictSourceOlder ||
                       conflictMode == MergeCopyHandlerConflictMode.EntityConflictSourceLineagePreferrable ||
                       conflictMode == MergeCopyHandlerConflictMode.EntityConflictTargetLineagePreferrable ||
                       conflictMode == MergeCopyHandlerConflictMode.EntityConflictSourceExternalPreferrable ||
                       conflictMode == MergeCopyHandlerConflictMode.EntityConflictTargetExternalPreferrable;
            }

            /// <summary>
            /// Defines whether current conflict type related to Attribute
            /// </summary>
            public static Boolean IsAttributeConflict(MergeCopyHandlerConflictMode conflictMode)
            {
                return conflictMode == MergeCopyHandlerConflictMode.AttributeConflictDestinationNonexistent ||
                       conflictMode == MergeCopyHandlerConflictMode.AttributeConflictSourceNewer ||
                       conflictMode == MergeCopyHandlerConflictMode.AttributeConflictSourceNonexistent ||
                       conflictMode == MergeCopyHandlerConflictMode.AttributeConflictSourceOlder ||
                       conflictMode == MergeCopyHandlerConflictMode.AttributeConflictSourceLineagePreferrable ||
                       conflictMode == MergeCopyHandlerConflictMode.AttributeConflictTargetLineagePreferrable ||
                       conflictMode == MergeCopyHandlerConflictMode.AttributeConflictSourceExternalPreferrable ||
                       conflictMode == MergeCopyHandlerConflictMode.AttributeConflictTargetExternalPreferrable ||
                       conflictMode == MergeCopyHandlerConflictMode.AttributeConflictUnion;
            }

            /// <summary>
            /// Defines whether current conflict type related to Relationship
            /// </summary>
            public static Boolean IsRelationshipConflict(MergeCopyHandlerConflictMode conflictMode)
            {
                return conflictMode == MergeCopyHandlerConflictMode.RelationshipConflictDestinationNonexistent ||
                       conflictMode == MergeCopyHandlerConflictMode.RelationshipConflictSourceNewer ||
                       conflictMode == MergeCopyHandlerConflictMode.RelationshipConflictSourceNonexistent ||
                       conflictMode == MergeCopyHandlerConflictMode.RelationshipConflictSourceOlder ||
                       conflictMode == MergeCopyHandlerConflictMode.RelationshipConflictSourceLineagePreferrable ||
                       conflictMode == MergeCopyHandlerConflictMode.RelationshipConflictTargetLineagePreferrable ||
                       conflictMode == MergeCopyHandlerConflictMode.RelationshipConflictSourceCustomPreferrable ||
                       conflictMode == MergeCopyHandlerConflictMode.RelationshipConflictTargetCustomPreferrable;
            }
            
            /// <summary>
            /// Get user managed node information from the current merge copy context
            /// </summary>
            /// <returns>Returns user managed node information from the current merge copy context</returns>
            public List<Object> GetUserManagedNodeInformation()
            {
                return this.userManagedNodeInformation;
            }

            /// <summary>
            /// Set the user managed node information to the current merge copy context
            /// </summary>
            /// <param name="userManagedNodeInformation">Indicates the user managed node information in merge copy</param>
            public void SetUserManagedNodeInformation(List<Object> userManagedNodeInformation)
            {
                this.userManagedNodeInformation = userManagedNodeInformation;
            }

            /// <summary>
            /// Determine whether the two attribute values are different based on AreAttributeValuesDifferent flag
            /// </summary>
            /// <returns>Returns whether the two attribute values are different in merge copy</returns>
            public bool GetAreAttributeValuesDifferent()
            {
                return this.areAttributeValuesDifferent;
            }

            /// <summary>
            /// Set the value of areAttributeValuesDifferent to the current merge copy context
            /// </summary>
            /// <param name="areAttributeValuesDifferent">Indicates the boolean flag value of areAttributeValueDifferent to set to the current merge copy context</param>
            public void SetAreAttributeValuesDifferent(bool areAttributeValuesDifferent)
            {
                this.areAttributeValuesDifferent = areAttributeValuesDifferent;
            }

            /// <summary>
            /// Get collection strategy values from the current merge copy context
            /// </summary>
            /// <returns>Returns the collection strategy values from the current merge copy context</returns>
            public CollectionStrategy GetCollectionStrategy()
            {
                return this.collectionStrategy;
            }

            /// <summary>
            /// Set the collection strategy value to the current merge copy context
            /// </summary>
            /// <param name="collectionStrategy">Indicates the collection strategy value to set to the current merge copy context</param>
            public void SetCollectionStrategy(CollectionStrategy collectionStrategy)
            {
                this.collectionStrategy = collectionStrategy;
            }

            /// <summary>
            /// Get skip engine processing for current node value from the current merge copy context
            /// </summary>
            /// <returns>Returns skip engine processing for current node value from the current merge copy context</returns>
            public bool GetSkipEngineProcessingForCurrentNode()
            {
                return this.skipEngineProcessingForCurrentNode;
            }

            /// <summary>
            /// Set skip engine processing for current node value as true to the current merge copy context
            /// </summary>
            public void SetSkipEngineProcessingForCurrentNode()
            {
                this.skipEngineProcessingForCurrentNode = true;
            }

            /// <summary>
            /// Set skip engine processing for current node value as false to the current merge copy context
            /// </summary>
            public void UnsetSkipEngineProcessingForCurrentNode()
            {
                this.skipEngineProcessingForCurrentNode = false;
            }

            /// <summary>
            /// Sets target object type (Entity, Attribute, Relationship) value to the current merge copy context for handling
            /// </summary>
            public void SetTargetObjectType(TargetObjectType targetObjectType)
            {
                this.targetObjectType = targetObjectType;
            }

            /// <summary>
            /// Gets target object type (Entity, Attribute, Relationship) from the current merge copy context
            /// </summary>
            public TargetObjectType GetTargetObjectType()
            {
                return this.targetObjectType;
            }
        }

        #endregion

        #region Internal callback : placeholder

        /// <summary>
        /// This will be used to later internally hook up core engine functionality. This is just a placeholder for now.
        /// </summary>
        public class MergeCopyInternalHandler : MDM.BusinessObjects.MergeCopy.IMergeCopyHandler
        {
            /// <summary>
            /// Internal Callback
            /// </summary>
            /// <param name="conflictContext">Indicates the conflict context information</param>
            /// <param name="callerContext">Indicates the information about the application and module which initiated the merge copy call</param>
            /// <param name="entityOperationResult">Indicates the result of entity operation</param>
            /// <returns>Returns the result of merge operation (it is not implemented currently and hence will return false.)</returns>
            public bool MergeUsingMergeContext(MergeCopyContext.MergeCopyConflictContext conflictContext, MergeCopyContext.MergeCopyCallerContext callerContext, IEntityOperationResult entityOperationResult)
            { 
                return false; 
            }
        }

        #endregion
    }

}
