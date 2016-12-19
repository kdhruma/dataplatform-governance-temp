using System;
using System.Collections.Generic;

namespace MDM.Utility
{
    /// <summary>
    /// Represents application configuration constants class (containing enumerations and dictionaries)
    /// </summary>
    public class ApplicationConfigConstants
    {
        #region Public Dictionaries

        /// <summary>
        /// 
        /// </summary>
        public static Dictionary<ApplicationConfigTemplateFieldEnum, String> ApplicationConfigTemplateColumns = new Dictionary<ApplicationConfigTemplateFieldEnum, String>();

        /// <summary>
        /// 
        /// </summary>
        public static Dictionary<ToolBarTemplateFieldEnum, String> ToolBarTemplateColumns = new Dictionary<ToolBarTemplateFieldEnum, String>();

        /// <summary>
        /// 
        /// </summary>
        public static Dictionary<PanelBarTemplateFieldEnum, String> PanelBarTemplateColumns = new Dictionary<PanelBarTemplateFieldEnum, String>();

        /// <summary>
        /// 
        /// </summary>
        public static Dictionary<SearchAttributeTemplateFieldEnum, String> SearchAttributeTemplateColumns = new Dictionary<SearchAttributeTemplateFieldEnum, String>();

        /// <summary>
        /// 
        /// </summary>
        public static Dictionary<GridTemplateFieldEnum, String> GridTemplateColumns = new Dictionary<GridTemplateFieldEnum, String>();

        #endregion

        /// <summary>
        /// 
        /// </summary>
        static ApplicationConfigConstants()
        {
            BuildTemplateColumnDictionaries();
        }

        private static void BuildTemplateColumnDictionaries()
        {
            //Application Config metadata

            ApplicationConfigTemplateColumns.Add(ApplicationConfigTemplateFieldEnum.ConfigType, "Config Type");
            ApplicationConfigTemplateColumns.Add(ApplicationConfigTemplateFieldEnum.ShortName, "Extended Config Name");
            ApplicationConfigTemplateColumns.Add(ApplicationConfigTemplateFieldEnum.ApplicationContextDefinition, "Application Context Definition");
            ApplicationConfigTemplateColumns.Add(ApplicationConfigTemplateFieldEnum.Organization, "Organization Name");
            ApplicationConfigTemplateColumns.Add(ApplicationConfigTemplateFieldEnum.Container, "Container Name");
            ApplicationConfigTemplateColumns.Add(ApplicationConfigTemplateFieldEnum.Taxonomy, "Hierarchy Name");
            ApplicationConfigTemplateColumns.Add(ApplicationConfigTemplateFieldEnum.CategoryPath, "Category Path");
            ApplicationConfigTemplateColumns.Add(ApplicationConfigTemplateFieldEnum.EntityType, "Entity Type Name");
            ApplicationConfigTemplateColumns.Add(ApplicationConfigTemplateFieldEnum.AttributePath, "Attribute Path");
            ApplicationConfigTemplateColumns.Add(ApplicationConfigTemplateFieldEnum.RelationshipType, "Relationship Type Name");
            ApplicationConfigTemplateColumns.Add(ApplicationConfigTemplateFieldEnum.SecurityRole, "Security Role Name");
            ApplicationConfigTemplateColumns.Add(ApplicationConfigTemplateFieldEnum.SecurityUserLogin, "Security User Login");
            ApplicationConfigTemplateColumns.Add(ApplicationConfigTemplateFieldEnum.Locale, "Locale");
            ApplicationConfigTemplateColumns.Add(ApplicationConfigTemplateFieldEnum.IsPopulate, "Execute?");

            //ToolBar metadata
            ToolBarTemplateColumns.Add(ToolBarTemplateFieldEnum.ConfigName, "Extended Config Name");
            ToolBarTemplateColumns.Add(ToolBarTemplateFieldEnum.ToolBarItemType, "ToolBar Item Type");
            ToolBarTemplateColumns.Add(ToolBarTemplateFieldEnum.ToolBarItemKey, "ToolBar Item  Key");
            ToolBarTemplateColumns.Add(ToolBarTemplateFieldEnum.ToolBarItemParentKey, "ToolBar Item Parent Key");
            ToolBarTemplateColumns.Add(ToolBarTemplateFieldEnum.Action, "Action");
            ToolBarTemplateColumns.Add(ToolBarTemplateFieldEnum.Property, "Property");
            ToolBarTemplateColumns.Add(ToolBarTemplateFieldEnum.Value, "Value");

            //Search grid metadata
            GridTemplateColumns.Add(GridTemplateFieldEnum.ConfigName, "Extended Config Name");
            GridTemplateColumns.Add(GridTemplateFieldEnum.ColumnName, "Column Key");
            GridTemplateColumns.Add(GridTemplateFieldEnum.Action, "Action");
            GridTemplateColumns.Add(GridTemplateFieldEnum.Property, "Property");
            GridTemplateColumns.Add(GridTemplateFieldEnum.Value, "Value");

            //PanelBar metadata
            PanelBarTemplateColumns.Add(PanelBarTemplateFieldEnum.ConfigName, "Extended Config Name");
            PanelBarTemplateColumns.Add(PanelBarTemplateFieldEnum.PanelType, "Panel Type");
            PanelBarTemplateColumns.Add(PanelBarTemplateFieldEnum.PanelName, "Panel Name");
            PanelBarTemplateColumns.Add(PanelBarTemplateFieldEnum.ParentPanelName, "Parent Panel Name");
            PanelBarTemplateColumns.Add(PanelBarTemplateFieldEnum.Action, "Action");
            PanelBarTemplateColumns.Add(PanelBarTemplateFieldEnum.Property, "Property");
            PanelBarTemplateColumns.Add(PanelBarTemplateFieldEnum.Value, "Value");

            //Search attribute metadata
            SearchAttributeTemplateColumns.Add(SearchAttributeTemplateFieldEnum.ConfigName, "Extended Config Name");
            SearchAttributeTemplateColumns.Add(SearchAttributeTemplateFieldEnum.AttributeRuleType, "Attribute Rule Type");
            SearchAttributeTemplateColumns.Add(SearchAttributeTemplateFieldEnum.AttributeRule, "Attribute Rule Key");
            SearchAttributeTemplateColumns.Add(SearchAttributeTemplateFieldEnum.Action, "Action");
            SearchAttributeTemplateColumns.Add(SearchAttributeTemplateFieldEnum.AttributePath, "Attribute Path");
            SearchAttributeTemplateColumns.Add(SearchAttributeTemplateFieldEnum.Property, "Property");
            SearchAttributeTemplateColumns.Add(SearchAttributeTemplateFieldEnum.Value, "Value");
        }
    }

    #region Enums

    /// <summary>
    /// 
    /// </summary>
    public enum ApplicationConfigTemplateFieldEnum
    {
        /// <summary>
        /// 
        /// </summary>
        IsPopulate,

        /// <summary>
        /// 
        /// </summary>
        ConfigType,

        /// <summary>
        /// 
        /// </summary>
        ApplicationContextDefinition,

        /// <summary>
        /// 
        /// </summary>
        ShortName,

        /// <summary>
        /// 
        /// </summary>
        LongName,

        /// <summary>
        /// 
        /// </summary>
        Organization,

        /// <summary>
        /// 
        /// </summary>
        Container,

        /// <summary>
        /// 
        /// </summary>
        EntityType,

        /// <summary>
        /// 
        /// </summary>
        RelationshipType,

        /// <summary>
        /// 
        /// </summary>
        SecurityRole,

        /// <summary>
        /// 
        /// </summary>
        SecurityUserLogin,

        /// <summary>
        /// 
        /// </summary>
        Taxonomy,

        /// <summary>
        /// 
        /// </summary>
        CategoryPath,
        
        /// <summary>
        /// 
        /// </summary>
        AttributePath,

        /// <summary>
        /// 
        /// </summary>
        Locale
    }

    /// <summary>
    /// 
    /// </summary>
    public enum ToolBarTemplateFieldEnum
    {
        /// <summary>
        /// 
        /// </summary>
        ConfigName,

        /// <summary>
        /// 
        /// </summary>
        ToolBarItemType,

        /// <summary>
        /// 
        /// </summary>
        ToolBarItemKey,

        /// <summary>
        /// 
        /// </summary>
        ToolBarItemParentKey,

        /// <summary>
        /// 
        /// </summary>
        Event,

        /// <summary>
        /// 
        /// </summary>
        Property,

        /// <summary>
        /// 
        /// </summary>
        Value,

        /// <summary>
        /// 
        /// </summary>
        Action
    }

    /// <summary>
    /// 
    /// </summary>
    public enum PanelBarTemplateFieldEnum
    {
        /// <summary>
        /// 
        /// </summary>
        ConfigName,

        /// <summary>
        /// 
        /// </summary>
        PanelType,

        /// <summary>
        /// 
        /// </summary>
        PanelName,

        /// <summary>
        /// 
        /// </summary>
        ParentPanelName,

        /// <summary>
        /// 
        /// </summary>
        Property,

        /// <summary>
        /// 
        /// </summary>
        Value,

        /// <summary>
        /// 
        /// </summary>
        Action
    }

    /// <summary>
    /// 
    /// </summary>
    public enum SearchAttributeTemplateFieldEnum
    {
        /// <summary>
        /// 
        /// </summary>
        ConfigName,

        /// <summary>
        /// 
        /// </summary>
        AttributeRuleType,

        /// <summary>
        /// 
        /// </summary>
        AttributeRule,

        /// <summary>
        /// 
        /// </summary>
        Property,

        /// <summary>
        /// 
        /// </summary>
        Value,

        /// <summary>
        /// 
        /// </summary>
        Action,

        /// <summary>
        /// 
        /// </summary>
        AttributePath
    }

    /// <summary>
    /// 
    /// </summary>
    public enum GridTemplateFieldEnum
    {
        /// <summary>
        /// 
        /// </summary>
        ConfigName,

        /// <summary>
        /// 
        /// </summary>
        ColumnType,

        /// <summary>
        /// 
        /// </summary>
        ColumnName,

        /// <summary>
        /// 
        /// </summary>
        ParentColumnName,

        /// <summary>
        /// 
        /// </summary>
        Property,

        /// <summary>
        /// 
        /// </summary>
        Value,

        /// <summary>
        /// 
        /// </summary>
        Action
    }

    #endregion
}
