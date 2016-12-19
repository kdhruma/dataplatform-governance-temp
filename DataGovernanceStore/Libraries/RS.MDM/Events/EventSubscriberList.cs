namespace RS.MDM.Events
{
    /// <summary>
    /// Specifies all the available Event Subscribers 
    /// </summary>
    public enum EventSubscriberList
    {
        /// <summary>
        /// ProductCenter Event Subscriber
        /// </summary>
        ProductCenter = 1,
        /// <summary>
        /// WebUI Event Subscriber
        /// </summary>
        WebUI = 2,
        /// <summary>
        /// AdvancedSearchPage Event Subscriber
        /// </summary>
        AdvancedSearchPage = 3,
        /// <summary>
        /// AdvancedSearchPageGrid Event Subscriber
        /// </summary>
        AdvancedSearchPageGrid = 4,
        /// <summary>
        /// AdvancedSearchPageSearchAttributes Event Subscriber
        /// </summary>
        AdvancedSearchPageSearchAttributes = 5,
        /// <summary>
        /// AdvancedSearchPageToolBar Event Subscriber
        /// </summary>
        AdvancedSearchPageToolBar = 6,
        /// <summary>
        /// DQMGuidedSearchPage Event Subscriber
        /// </summary>
        DQMGuidedSearchPage = 7,
        /// <summary>
        /// DQMGuidedSearchPageGrid Event Subscriber
        /// </summary>
        DQMGuidedSearchPageGrid = 8,
        /// <summary>
        /// DQMGuidedSearchPageToolBar Event Subscriber
        /// </summary>
        DQMGuidedSearchPageToolBar = 9,
        /// <summary>
        /// ServiceResultsPage Event Subscriber
        /// </summary>
        ServiceResultsPage = 10,
        /// <summary>
        /// ServiceResultsPageGrid Event Subscriber
        /// </summary>
        ServiceResultsPageGrid = 11,
        /// <summary>
        /// ServiceResultsPageToolBar Event Subscriber
        /// </summary>
        ServiceResultsPageToolBar = 12,
        /// <summary>
        /// ServiceResultsPageDataSource Event Subscriber
        /// </summary>
        ServiceResultsPageDataSource = 13,
        /// <summary>
        /// ServiceSelectorPage Event Subscriber
        /// </summary>
        ServiceSelectorPage = 14,
        /// <summary>
        /// ServiceSelectorPageToolBar Event Subscriber
        /// </summary>
        ServiceSelectorPageToolBar = 15,
        /// <summary>
        /// ServiceSelectorAdvancedSearch Event Subscriber
        /// </summary>
        ServiceSelectorAdvancedSearch = 16,
        /// <summary>
        /// AdvancedSearchPageToolBarRelationship Event Subscriber
        /// </summary>
        AdvancedSearchPageToolBarRelationship = 17,
        /// <summary>
        /// RelationshipPage Event Subscriber
        /// </summary>
        RelationshipPage = 18,
        /// <summary>
        /// RelationshipPageToolBar Event Subscriber
        /// </summary>
        RelationshipPageToolBar = 19,
        /// <summary>
        /// RelationshipPage Event Subscriber
        /// </summary>
        WhereUsedPage = 20,
        /// <summary>
        /// RelationshipPageToolBar Event Subscriber
        /// </summary>
        WhereUsedPageToolBar = 21,
        /// <summary>
        /// RelationshipPageGrid Event Subscriber
        /// </summary>
        WhereUsedPageGrid = 22,
        /// <summary>
        /// RelationshipPage Event Subscriber
        /// </summary>
        HomePage = 23,
        /// <summary>
        /// RelationshipPageToolBar Event Subscriber
        /// </summary>
        HomePagePanelBar = 24,
        /// <summary>
        /// ApplicationPage Event Subscriber
        /// </summary>
        NodeTypeApplicationsPage = 25,
        /// <summary>
        /// NodeTypeApplicationsPageToolbar Event Subscriber
        /// </summary>
        NodeTypeApplicationsPageToolBar = 26,
        /// <summary>
        /// NodeTypeApplicationsPageGrid Event Subscriber
        /// </summary>
        NodeTypeApplicationsPageGrid = 27,
        /// <summary>
        /// NodeTypeApplicationsPageGrid Event Subscriber
        /// </summary>
        NodeTypeApplicationsPageControls = 28,
        /// <summary>
        /// RelationshipPageGrid Event Subscriber
        /// </summary>
        RelationshipPageGrid = 29,
        /// <summary>
        /// DocumentBrowserPage Event Subscriber
        /// </summary>
        DocumentBrowserPage = 30,
        /// <summary>
        /// DocumentBrowserPageDocumentExplorer Event Subscriber
        /// </summary>
        DocumentBrowserPageDocumentExplorer = 31,
        /// <summary>
        /// DocumentBrowserPageGrid Event Subscriber
        /// </summary>
        DocumentBrowserPageGrid = 32,
        /// <summary>
        /// DocumentBrowserPageGrid Event Subscriber
        /// </summary>
        AdvancedSearchPageToolBarDocumentAssociation = 33,
        /// <summary>
        /// ItemEditPageToolBar Event Subscriber
        /// </summary>
        ItemEditPage = 34,
        /// <summary>
        /// ItemEditPageToolBar Event Subscriber
        /// </summary>
        ItemEditPageToolBar = 35,
        /// <summary>
        /// ChildEntityDetailPage Event Subscriber
        /// </summary>
        ChildEntityDetailPage = 36,
        /// <summary>
        /// ChildEntityDetailGrid Event Subscriber
        /// </summary>
        ChildEntityDetailGrid = 37,
        /// <summary>
        /// ChildEntityDetailGrid Event Subscriber
        /// </summary>
        ChildEntityDetailToolBar = 38,
        /// <summary>
        /// EntityDetailPage Event Subscriber
        /// </summary>
        EntityDetailPage = 39,
        /// <summary>
        /// EntityDetailRibbonBar Event Subscriber
        /// </summary>
        EntityDetailPageRibbonBar = 40,
        /// <summary>
        /// EntityDetailToolBar Event Subscriber
        /// </summary>
        EntityDetailPageActionToolBar = 41,
        /// <summary>
        /// EntityDetailStageTransitionToolBar Event Subscriber
        /// </summary>
        EntityDetailPageStageTransitionToolBar = 42,
        /// <summary>
        /// EntityDetailAssignmentToolBar Event Subscriber
        /// </summary>
        EntityDetailPageAssignmentToolBar = 43,
        /// <summary>
        /// EntityExplorerPage Event Subscriber
        /// </summary>
        EntityExplorerPage = 44,
        /// <summary>
        /// EntityExplorerPageGrid Event Subscriber
        /// </summary>
        EntityExplorerPageGrid = 45,
        /// <summary>
        /// EntityExplorerPageSearchAttributes Event Subscriber
        /// </summary>
        EntityExplorerPageSearchAttributes = 46,
        /// <summary>
        /// EntityExplorerPageRibbonBar Event Subscriber
        /// </summary>
        EntityExplorerPageRibbonBar = 47,
        /// <summary>
        /// EntityExplorerPageActionToolBar Event Subscriber
        /// </summary>
        EntityExplorerPageActionToolBar = 48,
        /// <summary>
        /// EntityExplorerPageStageTransitionToolBar Event Subscriber
        /// </summary>
        EntityExplorerPageStageTransitionToolBar = 49,
        /// <summary>
        /// EntityExplorerPageAssignmentToolBar Event Subscriber
        /// </summary>
        EntityExplorerPageAssignmentToolBar = 50,
        /// <summary>
        /// MetaDataPage Event Subscriber
        /// </summary>
        MetaDataPage = 51,
        /// <summary>
        /// MetaDataPageToolBar Event Subscriber
        /// </summary>
        MetaDataPageToolBar = 52,
        /// <summary>
        /// AdvancedSearchPageToolBarTestRuleSet Event Subscriber
        /// </summary>
        AdvancedSearchPageToolBarTestRuleSet = 53,
        /// <summary>
        /// EntityEditorPage Event Subscriber
        /// </summary>
        EntityEditorPage = 54,
        /// <summary>
        /// EntityEditorPageNavigationPanel Event Subscriber
        /// </summary>
        EntityEditorPageNavigationPanel = 55,
        /// <summary>
        /// EntityEditorPageOtherViewsPanel Event Subscriber
        /// </summary>
        EntityEditorPageOtherViewsPanel = 56,
        /// <summary>
        /// EntityEditorStandardViewsTreeView Event Subscriber
        /// </summary>
        EntityEditorStandardViewsTreeView = 57,
        /// <summary>
        /// EntityEditorMyViewsTreeView Event Subscriber
        /// </summary>
        EntityEditorMyViewsTreeView = 59,
        /// <summary>
        /// EntityEditorContainersTreeView Event Subscriber
        /// </summary>
        EntityEditorContainersTreeView = 60,
        /// <summary>
        /// EntityEditorDefaultSettings Event Subscriber
        /// </summary>
        EntityEditorDefaultSettings = 61,
        /// <summary>
        /// EntityEditorPage NavigationPanel StateViews
        /// </summary>
        EntityEditorPageNavigationPanelStateViews = 62,
        /// <summary>
        /// EntityExplorerPage Left Panel Bar Event Subscriber
        /// </summary>
        EntityExplorerPageLeftPanelBar = 63,
        /// <summary>
        /// EntityEditorPage BreadCrumb 
        /// </summary>
        EntityEditorPageBreadCrumb = 64,
        /// <summary>
        /// EntityEditorPage Title
        /// </summary>
        EntityEditorPageTitle = 65,
        /// <summary>
        /// Entity Explorer Page Windows Workflow Action ToolBar
        /// </summary>
        EntityExplorerPageWindowsWorkflowActionToolBar = 66,
        /// <summary>
        /// Entity Explorer Page Windows Workflow Assignment ToolBar
        /// </summary>
        EntityExplorerPageWindowsWorkflowAssignmentToolBar = 67,
        /// <summary>
        /// Entity Detail Page Windows Workflow Action ToolBar
        /// </summary>
        EntityDetailPageWindowsWorkflowActionToolBar = 68,
        /// <summary>
        /// Entity Detail Page Windows Workflow Assignment ToolBar
        /// </summary>
        EntityDetailPageWindowsWorkflowAssignmentToolBar = 69,
        /// <summary>
        /// Entity Editor Other views Panel WorkflowTasks Tree view
        /// </summary>
        EntityEditorPageOtherviewsPanelWorkflowTasksTreeView = 70,
        /// <summary>
        /// Entity Editor Container Panel Data Config
        /// </summary>
        EntityEditorContainerPanelDataConfig = 71,
        /// <summary>
        /// Vendor Portal Event Subscriber
        /// </summary>
        VendorPortal = 72,
        /// <summary>
        /// Vendor Portal Web UI Event Subscriber
        /// </summary>
        VPWebUI = 73,
        /// <summary>
        /// Vendor Portal Entity Explorer Page Event Subscriber
        /// </summary>
        VPEntityExplorerPage = 74,
        /// <summary>
        /// Vendor Portal Entity Explorer Page Menu Event Subscriber
        /// </summary>
        VPEntityExplorerPageMenu = 75,
        /// <summary>
        /// Vendor Portal Entity Explorer Page Search Grid Event Subscriber
        /// </summary>
        VPEntityExplorerPageSearchGrid = 76,
        /// <summary>
        /// Vendor Portal Entity Explorer Page Left Panel TreeView Event Subscriber
        /// </summary>
        VPEntityExplorerPageLeftPanelTreeView = 77,
        /// <summary>
        /// Vendor Portal Entity Explorer Page Basic Actions Toolbar Event Subscriber
        /// </summary>
        VPEntityExplorerPageBasicActionsToolBar = 78,
        /// <summary>
        /// Vendor Portal Entity Explorer Page Stage Transitions Toolbar Event Subscriber
        /// </summary>
        VPEntityExplorerPageStageTransitionToolBar = 79,
        /// <summary>
        /// Vendor Portal Entity Editor Page Event Subscriber
        /// </summary>
        VPEntityEditorPage = 80,
        /// <summary>
        /// Vendor Portal Entity Editor Page Left Panel Tree View Event Subscriber
        /// </summary>
        VPEntityEditorPageLeftPanelTreeView = 81,
        /// <summary>
        /// Vendor Portal Entity Editor Page Basic Actions Toolbar Event Subscriber
        /// </summary>
        VPEntityEditorPageBasicActionsToolBar = 82,
        /// <summary>
        /// Vendor Portal Entity Editor Page Stage Transitions Toolbar Event Subscriber
        /// </summary>
        VPEntityEditorPageStageTransitionToolBar = 83,
        /// <summary>
        /// Vendor Portal Entity Explorer Page Report Grid Event Subscriber
        /// </summary>
        VPEntityExplorerPageReportGrid = 84,
        /// <summary>
        /// Vendor Portal Entity Explorer Page Report Search Panel Event Subscriber
        /// </summary>
        VPEntityExplorerPageSearchPanel = 85,
        /// <summary>
        /// Vendor Portal Search Configuration Event Subscriber
        /// </summary>
        VPSearchConfiguration = 86,
        /// <summary>
        /// Vendor Portal Entity Editor Page Tab Title Event Subscriber
        /// </summary>
        VPEntityEditorPageTabTitle = 87,
        /// <summary>
        /// EntityEditorPage OtherViewsPanel Configuration Event Subscriber
        /// </summary>
        EntityEditorPageOtherViewsPanelConfiguration = 88,
        /// <summary>
        /// MDMCenter Event Subscriber
        /// </summary>
        MDMCenter = 89,
        /// <summary>
        /// Entity Hierarchy Event Subscriber
        /// </summary>
        EntityHierarchy = 90,
        /// <summary>
        /// Entity Hierarchy Matrix ToolBar Event Subscriber
        /// </summary>
        EntityHierarchyMatrixToolBar = 91,
        /// <summary>
        /// Entity Hierarchy Level Detail ToolBar Event Subscriber
        /// </summary>
        EntityHierarchyLevelDetailToolBar = 92,
        /// <summary>
        /// MAM Event Subscriber
        /// </summary>
        MAM = 93,
        /// <summary>
        /// MAM web UI event subscriber
        /// </summary>
        MAMWebUI = 94,
        /// <summary>
        /// MAM entity explorer event subscriber
        /// </summary>
        MAMEntityExplorerPage = 95,
        /// <summary>
        /// MAM entity editor event subscriber
        /// </summary>
        MAMEntityEditorPage = 96,
        /// <summary>
        /// Entity Hierarchy Rule ToolBar Event Subscriber
        /// </summary>
        EntityHierarchyRuleToolBar = 97,
        /// <summary>
        /// Entity Hierarchy Rule Grid Event Subscriber
        /// </summary>
        EntityHierarchyLevelDetailGrid = 98,
        /// <summary>
        /// MDMCenter Search Manager Event Subscriber
        /// </summary>
        SearchManager = 99,
        /// <summary>
        /// MDMCenter Search Criteria Event Subscriber
        /// </summary>
        SearchCriteria = 100,
        /// <summary>
        /// MDMCenter Search Weightage Event Subscriber
        /// </summary>
        SearchWeightage = 101,
        /// <summary>
        /// MDMCenter Search Workflow Event Subscriber
        /// </summary>
        WorkflowSearch = 102,
        /// <summary>
        /// MDMCenter Search Configuration Event Subscriber
        /// </summary>
        MDMCenterSearchConfiguration = 113,
        /// <summary>
        /// MDMCenter Locale Configuration Event Subscriber
        /// </summary>
        LocaleConfiguration = 114,
        /// <summary>
        /// Vendor Portal Bulk Loader Page
        /// </summary>
        VPBulkLoaderPage = 115,
        /// <summary>
        /// Vendor Portal Bulk Loader Page Import Templates List Event Subscriber
        /// </summary>
        VPBulkLoaderPageImportTemplatesList = 116,
        /// <summary>
        /// Vendor Portal Bulk Loader Page Batch Job Grid
        /// </summary>
        VPBulkLoaderPageBatchJobGrid = 117,
        /// <summary>
        /// Vendor Portal Bulk Loader Page Import Profiles List
        /// </summary>
        VPBulkLoaderPageImportProfilesList = 118,
        /// <summary>
        /// Vendor Portal Bulk Loader Page Batch Job ToolBar
        /// </summary>
        VPBulkLoaderPageBatchJobToolBar = 119,
        /// <summary>
        /// Vendor Portal Entity Explorer Page Entity Assignment ToolBar
        /// </summary>
        VPEntityExplorerPageEntityAssignmentToolBar = 120,
        /// <summary>
        /// Vendor Portal Bulk Loader Page Batch Job Details ToolBar
        /// </summary>
        VPBulkLoaderPageBatchJobDetailsToolBar = 121,
        /// <summary>
        /// Vendor Portal Bulk Loader Page Batch Job Details Grid
        /// </summary>
        VPBulkLoaderPageBatchJobDetailsGrid = 122,
        /// <summary>
        /// MetaDataPagePanelBar
        /// </summary>
        MetaDataPagePanelBar = 123,
        /// <summary>
        /// Vendor Portal Entity Editor Page Settings
        /// </summary>
        VPEntityEditorPageSettings = 124,
        /// <summary>
        /// Vendor Portal Entity Editor Page Workflow Stage ToolBar
        /// </summary>
        VPEntityEditorPageWorkflowStageToolBar = 125,
        /// <summary>
        /// Vendor Portal Entity Editor Page Entity Assignment Toolbar
        /// </summary>
        VPEntityEditorPageEntityAssignmentToolBar = 126,
        /// <summary>
        /// Vendor Portal Entity Editor Page Entity Hierarchy Simple Grid
        /// </summary>
        VPEntityEditorPageEntityHierarchySimpleGrid = 127,
        /// <summary>
        /// Vendor Portal Entity Explorer page Export Toolbar
        /// </summary>
        VPEntityExplorerPageExportToolBar = 128,
        /// <summary>
        /// Vendor Portal Bulk Loader page Settings
        /// </summary>
        VPBulkLoaderPageDownloadTemplateSettings = 129,
        /// <summary>
        /// Vendor Portal Done Report page
        /// </summary>
        VPDoneReportPage = 130,
        /// <summary>
        /// Vendor Portal Done Report page Done Report Entities Grid
        /// </summary>
        VPDoneReportPageGrid = 131,
        /// <summary>
        /// Vendor Portal Done Report page Settings
        /// </summary>
        VPDoneReportPageSettings = 132,
        /// <summary>
        /// Vendor Portal Entity Editor page Entity Hierarchy Simple Actions ToolBar
        /// </summary>
        VPEntityEditorPageEntityHierarchySimpleActionsToolBar = 133,
        /// <summary>
        /// Vendor Portal Entity Editor page Entity Comparer
        /// </summary>
        VPEntityEditorPageEntityComparerSettings = 134,
        /// <summary>
        /// Vendor Portal Entity Editor page Hierarchy Simple Batch Job Details ToolBar
        /// </summary>
        VPEntityEditorPageHierarchySimpleBatchJobDetailsToolBar = 135,
        /// <summary>
        /// Vendor Portal Entity Editor page Hierarchy Simple Import Profiles List
        /// </summary>
        VPEntityEditorPageHierarchySimpleImportProfilesList = 136,
        /// <summary>
        /// Vendor Portal Entity Editor page Hierarchy Simple Batch Job Details Grid
        /// </summary>
        VPEntityEditorPageHierarchySimpleBatchJobDetailsGrid = 137,
        /// <summary>
        /// EntityHierarchyConfiguration
        /// </summary>
        EntityHierarchyConfiguration = 138,
        /// <summary>
        /// Vendor Portal Editor page Entity Hierarchy Simple Batch Job Details Download Settings
        /// </summary>
        VPEntityEditorPageHierarchySimpleBatchJobDetailsDownloadSettings = 139,
        /// <summary>
        /// Vendor Portal Bulk Loader Batch Job Details Download Settings
        /// </summary>
        VPBulkLoaderPageBatchJobDetailsDownloadSettings = 140,
        /// <summary>
        /// DataQualityManagement Settings
        /// </summary>
        DataQualityManagement = 141,

        /// <summary>
        /// Data Quality Management Dashboard 
        /// </summary>
        DQMDashboard = 142,

        /// <summary>
        /// DQM Report Page Basic Actions ToolBar
        /// </summary>
        DQMReportPageBasicActionsToolBar = 143,

        /// <summary>
        /// DQM Dashboard page Settings
        /// </summary>
        DQMDashboardPageSettings = 144,

        /// <summary>
        /// DQM Dashboard page Report Grid
        /// </summary>
        DQMReportPageReportGrid = 145,

        /// <summary>
        /// DQM Entity Set page Report Grid
        /// </summary>
        DQMEntitySetReportPageReportGrid = 146,

        /// <summary>
        /// DQM DataQualityIndicator Management page Grid
        /// </summary>
        DQMDataQualityIndicatorManagementPageGrid = 147,

        /// <summary>
        /// DQM DataQualityIndicator Management page Toolbar
        /// </summary>
        DQMDataQualityIndicatorManagementPageToolbar = 148,

        /// <summary>
        /// DQM Validation Profile Management page Toolbar
        /// </summary>
        DQMValidationProfileManagementPageToolbar = 149,

        /// <summary>
        /// DQM Validation Jobs page Toolbar
        /// </summary>
        DQMValidationJobsPageToolbar = 150,

        /// <summary>
        /// MDM Complex Attribute page Event Subscriber
        /// </summary>
        ComplexAttributeEditorPage = 151,

        /// <summary>
        /// MDM Complex Attribute page Tool Bar Event Subscriber
        /// </summary>
        ComplexAttributeEditorPageToolbar = 152,

        /// <summary>
        /// MDM Complex Attribute page grid Event Subscriber
        /// </summary>
        ComplexAttributeEditorPageGrid = 153,

        /// <summary>
        /// VP Complex Attribute page Event Subscriber
        /// </summary>
        VPComplexAttributeEditorPage = 154,

        /// <summary>
        /// VP Complex Attribute page Tool bar Event Subscriber
        /// </summary>
        VPComplexAttributeEditorPageToolbar = 155,

        /// <summary>
        /// MDM Complex Attribute page grid Event Subscriber
        /// </summary>
        VPComplexAttributeEditorPageGrid = 156,

        /// <summary>
        /// MDM Entity History View page Event Subscriber
        /// </summary>
        EntityHistoryViewPage = 157,

        /// <summary>
        /// MDM Entity History View page grid Event Subscriber
        /// </summary>
        EntityHistoryViewPageGrid = 158,

        /// <summary>
        /// VP Entity History View page Event Subscriber
        /// </summary>
        VPEntityHistoryViewPage = 159,

        /// <summary>
        /// MDM Entity History View page grid Event Subscriber
        /// </summary>
        VPEntityHistoryViewPageGrid = 160,

        /// <summary>
        /// VP Catalog View page Event Subscriber
        /// </summary>
        VPCatalogViewPage = 161,

        /// <summary>
        /// VP Catalog View page Action Toolbar Event Subscriber
        /// </summary>
        VPCatalogViewPageActionToolbar = 162,

        /// <summary>
        /// VP catalog View page search grid Event Subscriber
        /// </summary>
        VPCatalogViewPageSearchGrid = 163,

        /// <summary>
        /// VP Catalog View page search panel Event Subscriber
        /// </summary>
        VPCatalogViewPageSearchPanel = 164,

        /// <summary>
        /// MDM entity history exclude list Event Subscriber
        /// </summary>
        EntityHistoryViewPageExcludeList = 165,

        /// <summary>
        /// VP entity history exclude list Event Subscriber
        /// </summary>
        VPEntityHistoryViewPageExcludeList = 166,

        /// <summary>
        /// DQM DataNormalization Event Subscriber
        /// </summary>
        DataNormalization = 167,

        /// <summary>
        /// DQM DataNormalization Profile Management page Basic Action Toolbar Event Subscriber
        /// </summary>
        DataNormalizationProfileManagementPageBasicActionsToolBar = 168,

        /// <summary>
        /// DQM DataNormalization Profile Management page Grid Event Subscriber
        /// </summary>
        DataNormalizationProfileManagementPageGrid = 169,

        /// <summary>
        /// DQM DataNormalization Rule Management page Basic Action Toolbar Event Subscriber
        /// </summary>
        DataNormalizationRuleManagementPageBasicActionsToolBar = 170,

        /// <summary>
        /// DQM DataNormalization Rule Management page Grid Event Subscriber
        /// </summary>
        DataNormalizationRuleManagementPageGrid = 171,

        /// <summary>
        /// catalog view event subscriber
        /// </summary>
        CatalogViewPage = 172,

        /// <summary>
        /// relationship page catalog view action toolbar event subscriber
        /// </summary>
        CatalogViewRelationshipPageActionToolbar = 173,

        /// <summary>
        /// catalog view page search grid event subscriber
        /// </summary>
        CatalogViewPageSearchGrid = 174,

        /// <summary>
        /// catalog view page search panel event subscriber
        /// </summary>
        CatalogViewPageSearchPanel = 175,

        /// <summary>
        /// DQM DataNormalization Report page Basic Action Toolbar Event Subscriber
        /// </summary>
        DataNormalizationReportPageBasicActionsToolBar = 176,

        /// <summary>
        /// DQM DataNormalization Report page Grid Event Subscriber
        /// </summary>
        DataNormalizationReportPageGrid = 177,

        /// <summary>
        /// Entity Editor Variants Panel Data Config
        /// </summary>
        VariantsPanelDataConfig = 178,

        /// <summary>
        /// Entity hierarchy level detail page catalog view action toolbar event subscriber
        /// </summary>
        CatalogViewEHPageActionToolbar = 179,

        /// <summary>
        ///DQM validation Profile management page grid event subscriber
        /// </summary>
        DQMValidationProfileManagementPageGrid = 180,

        /// <summary>
        /// Job page grid of DQM validation event subscriber
        /// </summary>
        DQMValidationJobsPageGrid = 181,

        /// <summary>
        /// DQM data merge event subscriber
        /// </summary>
        DataMerge = 182,

        /// <summary>
        /// DataMerge Planning rule set basic action toolbar event subscriber
        /// </summary>
        DataMergeMergePlanningProfilePageBasicActionsToolBar = 183,

        /// <summary>
        /// DataMerge Planning rule set grid event subscriber
        /// </summary>
        DataMergeMergePlanningProfilePageGrid = 184,

        /// <summary>
        /// DataMerge Source Management page basic action toolbar event subscriber
        /// </summary>
        DataMergeSourceManagementPageBasicActionsToolBar = 185,

        /// <summary>
        /// Data merge source management page grid event subscriber
        /// </summary>
        DataMergeSourceManagementPageGrid = 186,

        /// <summary>
        /// Translation configuration event subscriber
        /// </summary>
        TranslationConfiguration = 187,

        /// <summary>
        /// DataMerge Merge Report page basic action toolbar event subscriber
        /// </summary>
        DataMergeMergeReportPageBasicActionsToolBar = 188,

        /// <summary>
        /// Data merge source management page grid event subscriber
        /// </summary>
        DataMergeMergeReportPageGrid = 189,

        /// <summary>
        /// DQM data validation event subscriber
        /// </summary>
        DataValidation = 190,

        /// <summary>
        /// DataValidation Profile Management page toolbar event subscriber
        /// </summary>
        DataValidationProfileManagementPageBasicActionsToolBar = 191,

        /// <summary>
        /// DataValidation Profile Management page grid event subscriber
        /// </summary>
        DataValidationProfileManagementPageGrid = 192,

        /// <summary>
        /// DataMerge Profile Management page toolbar event subscriber
        /// </summary>
        DataMergeProfileManagementPageBasicActionsToolBar = 193,

        /// <summary>
        /// DataMerge Profile Management page grid event subscriber
        /// </summary>
        DataMergeProfileManagementPageGrid = 194,

        /// <summary>
        /// Entity Type Panel event subscriber
        /// </summary>
        EntityTypePanel = 209,

        /// <summary>
        /// DQM DataMerge SurvivorshipRule Management page BasicActionToolbar event subscriber
        /// </summary>
        DataMergeSurvivorshipRuleManagementPageBasicActionToolbar = 200,

        /// <summary>
        /// DQM DataMerge SurvivorshipRule Management page Grid event subscriber
        /// </summary>
        DataMergeSurvivorshipRuleManagementPageGrid = 201,

        /// <summary>
        /// MDM Workflow Comment history grid event subscriber
        /// </summary>
        WorkflowCommentsViewPageGrid = 213,

        /// <summary>
        /// EntityUniqueIdentification config subscriber
        /// </summary>
        EntityUniqueIdentification = 214,

        /// <summary>
        /// MDM Hierarchical Attribute page Event Subscriber
        /// </summary>
        HierarchicalAttributeEditorPage = 215,

        /// <summary>
        /// MDM Hierarchical Attribute page Tool Bar Event Subscriber
        /// </summary>
        HierarchicalAttributeEditorPageToolbar = 216,
        
        /// <summary>
        /// VP Hierarchical Attribute page Event Subscriber
        /// </summary>
        VPHierarchicalAttributeEditorPage = 217,

        /// <summary>
        /// VP Hierarchical Attribute page Tool bar Event Subscriber
        /// </summary>
        VPHierarchicalAttributeEditorPageToolbar = 218,
    }
}
