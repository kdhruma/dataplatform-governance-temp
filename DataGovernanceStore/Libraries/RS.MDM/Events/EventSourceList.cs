namespace RS.MDM.Events
{
    /// <summary>
    /// Specifies all the available Event Sources
    /// </summary>
    public enum EventSourceList
    {
        /// <summary>
        /// ProductCenter Event Source
        /// </summary>
        ProductCenter = 1,
        /// <summary>
        /// AdvacedSearch Event Source
        /// </summary>
        AdvacedSearch = 2,
        /// <summary>
        /// DQMGuidedSearch Event Source
        /// </summary>
        DQMGuidedSearch = 3,
        /// <summary>
        /// PNMatching Event Source
        /// </summary>
        PNMatching = 4,
        /// <summary>
        /// DescriptionMatching Event Source
        /// </summary>
        DescriptionMatching = 5,
        /// <summary>
        /// AutoClassification Event Source
        /// </summary>
        AutoClassification = 6,
        /// <summary>
        /// PromoteDemote Event Source
        /// </summary>
        PromoteDemote = 7,
        /// <summary>
        /// Relationship Event Source
        /// </summary>
        Relationship = 8,
        /// <summary>
        /// Home Event Source
        /// </summary>
        Home = 9,
        /// <summary>
        /// Normalization Event Source
        /// </summary>
        Normalization = 10,
        /// <summary>
        /// AttributeExtraction Event Source
        /// </summary>
        AttributeExtraction = 11,
        /// <summary>
        /// Validation Event Source
        /// </summary>
        Validation = 12,
        /// <summary>
        /// Application Event Source
        /// </summary>
        NodeTypeApplications = 13,
        /// <summary>
        /// DocumentExplorer Event Source
        /// </summary>
        DocumentExplorer = 14,
        /// <summary>
        /// ItemEdit Event Source
        /// </summary>
        ItemEdit = 15,
        /// <summary>
        /// ChildEntity Event Source
        /// </summary>
        ChildEntityDetail = 16,
        /// <summary>
        /// StageTransitionService Event Source
        /// </summary>
        StageTransitionService = 17,
        /// <summary>
        /// Default Attribute Value Event Source
        /// </summary>
        DefaultValue = 18,
        /// <summary>
        /// EntityDetail Event Source
        /// </summary>
        EntityDetail = 19,
        /// <summary>
        /// EntityExplorer Event Source
        /// </summary>
        EntityExplorer = 20,
        /// <summary>
        /// Meta Data Event Source
        /// </summary>
        MetaData = 21,
        ///<summary>
        /// Entity Editor Event Source
        ///</summary>
        EntityEditor = 22,
        ///<summary>
        /// Maintain View Event Source
        ///</summary>
        ConfigurableAttributeGroups = 23,
        /// <summary>
        /// 
        /// </summary>
        RuleBasedAttributeGroups = 24,
        /// <summary>
        /// 
        /// </summary>
        Entity = 25,
        /// <summary>
        /// Source for Entity Updating event
        /// </summary>
        EntityUpdating = 26,
        /// <summary>
        /// Source for Entity Updated event
        /// </summary>
        EntityUpdated = 27,
        /// <summary>
        /// Source for Entity Common Attributes Updating event
        /// </summary>
        EntityCommonAttributesUpdating = 28,
        /// <summary>
        /// Source for Entity Category Attributes Updating event
        /// </summary>
        EntityCategoryAttributesUpdating = 29,
        /// <summary>
        /// Source for Entity System Attributes Updating event
        /// </summary>
        EntitySystemAttributesUpdating = 30,
        /// <summary>
        /// Source for Entity Common Attributes Updated event
        /// </summary>
        EntityCommonAttributesUpdated = 31,
        /// <summary>
        /// Source for Entity Category Attributes Updated event
        /// </summary>
        EntityCategoryAttributesUpdated = 32,
        /// <summary>
        /// Source for Entity System Attributes Updated event
        /// </summary>
        EntitySystemAttributesUpdated = 33,
        /// <summary>
        /// Source for Relationship Updating event
        /// </summary>
        RelationShipUpdating = 34,
        /// <summary>
        /// Source for Relationship Updated event
        /// </summary>
        RelationShipUpdated = 35,
        /// <summary>
        /// 
        /// </summary>
        EntityAttributePublish = 36,
        /// <summary>
        /// Source for Relationship Creating event
        /// </summary>
        RelationShipCreating = 37,
        /// <summary>
        /// Source for Relationship Created event
        /// </summary>
        RelationShipCreated = 38,
        /// <summary>
        /// Source for Entity Creating event
        /// </summary>
        EntityCreating = 39,
        /// <summary>
        /// Source for Entity Created event
        /// </summary>
        EntityCreated = 40,
        /// <summary>
        /// Vendor Portal Event Source
        /// </summary>
        VendorPortal = 41,
        /// <summary>
        /// Vendor Portal Entity Explorer event source
        /// </summary>
        VPEntityExplorer = 42,
        /// <summary>
        /// Vendor Portal Entity Editor event source
        /// </summary>
        VPEntityEditor = 43,
        /// <summary>
        /// Vendor Portal Stage Transition Pre Process event source
        /// </summary>
        VPStageTransitionPreProcess = 44,
        /// <summary>
        /// Vendor Portal Stage Transition Post Process event source
        /// </summary>
        VPStageTransitionPostProcess = 45,
        /// <summary>
        /// Vendor Portal State Transition Pre Process event source
        /// </summary>
        VPStateTransitionPreProcess = 46,
        /// <summary>
        /// Vendor Portal State Transition Post Process event source
        /// </summary>
        VPStateTransitionPostProcess = 47,
        /// <summary>
        /// Source for Entity processing event
        /// </summary>
        EntityProcessing = 48,
        /// <summary>
        /// Source for Entity processed event
        /// </summary>
        EntityProcessed = 49,
        /// <summary>
        /// Source for Entity Hierarchy Relationship processing event
        /// </summary>
        HierarchyRelationshipProcessing = 50,
        /// <summary>
        /// Source for Entity Hierarchy Relationship processed event
        /// </summary>
        HierarchyRelationshipProcessed = 51,
        /// <summary>
        /// Source for Entity Validation event
        /// </summary>
        EntityValidate = 52,
        /// <summary>
        /// MDMCenter event source
        /// </summary>
        MDMCenter = 53,
        /// <summary>
        /// Entity Hierarchy event source
        /// </summary>
        EntityHierarchy = 54,
        /// <summary>
        /// MAM event source
        /// </summary>
        MAM = 55,
        /// <summary>
        /// MAM Entity Explorer event source
        /// </summary>
        MAMEntityExplorer = 56,
        /// <summary>
        /// MAM Entity Editor event source
        /// </summary>
        MAMEntityEditor = 57,
        /// <summary>
        /// EntityLoaded event
        /// </summary>
        EntityLoaded = 58,
        /// <summary>
        /// EntityCreatePostProcessStarting event
        /// </summary>
        EntityCreatePostProcessStarting = 88,
        /// <summary>
        /// EntityUpdatePostProcessStarting event
        /// </summary>
        EntityUpdatePostProcessStarting = 89,
        /// <summary>
        /// EntityCreatePostProcessCompleted event
        /// </summary>
        EntityCreatePostProcessCompleted = 90,
        /// <summary>
        /// EntityUpdatePostProcessCompleted event
        /// </summary>
        EntityUpdatePostProcessCompleted = 91,
        /// <summary>
        /// EntityDeletePostProcessStarting event
        /// </summary>
        EntityDeletePostProcessStarting = 92,
        /// <summary>
        /// EntityDeletePostProcessCompleted event
        /// </summary>
        EntityDeletePostProcessCompleted = 93,
        /// <summary>
        /// EntityDataQualityValidation event
        /// </summary>
        EntityDataQualityValidation = 94,
        /// <summary>
        /// DataQualityManagement WebApp Event Source
        /// </summary>
        DataQualityManagement = 95,
        /// <summary>
        /// Data Quality Management Dashboard Event Source
        /// </summary>
        DQMDashboard = 96,
        /// <summary>
        /// MDM Complex Attribute Event Source
        /// </summary>
        ComplexAttributeEditor = 97,
        /// <summary>
        /// VP Complex Attribute Event Source
        /// </summary>
        VPComplexAttributeEditor = 98,
        /// <summary>
        /// MDM Entity History View Event Source
        /// </summary>
        EntityHistoryView = 99,
        /// <summary>
        /// VP Entity History View Event Source
        /// </summary>
        VPEntityHistoryView = 100,

        /// <summary>
        /// VP Catalog View Event Source
        /// </summary>
        VPCatalogView = 101,

        /// <summary>
        /// Catalog View Event Source
        /// </summary>
        CatalogView = 103,

        /// <summary>
        /// Event Source representing entity loading.
        /// </summary>
        EntityLoading = 104,

        /// <summary>
        /// EntityNormalization event
        /// </summary>
        EntityNormalization = 105,

        /// <summary>
        /// EntityValidation event
        /// </summary>
        EntityValidation = 106
    }
}
