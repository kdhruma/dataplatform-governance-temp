using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.ObjectModel;
using MDM.Core;

namespace RS.MDM.ConfigurationObjects
{
    using RS.MDM.Configuration;
    using RS.MDM.Validations;

    /// <summary>
    /// Represents a data item of the entity type panel config
    /// </summary>
    [XmlRoot("EntityTypePanel")]
    [Serializable()]
    public sealed class EntityTypePanel : Object
    {
        #region Fields

        /// <summary>
        /// Represents the entity type item
        /// </summary>
        private EntityTypePanelItem _entityTypePanelItem = new EntityTypePanelItem();

        #endregion

        #region Properties

        /// <summary>
        /// Represents the entity type
        /// </summary>
        [Category("EntityTypePanelItem")]
        [Description("Represents the Entity Type Panel Item")]
        [TrackChanges()]
        public EntityTypePanelItem EntityTypePanelItem
        {
            get
            {
                this.SetParent();
                return this._entityTypePanelItem;
            }
            set
            {
                this._entityTypePanelItem = value;
                this.SetParent();
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the EntityTypePanel class.
        /// </summary>
        public EntityTypePanel() : base()
        {
            this.AddVerb("Add EntityType Panel Item");
        }

        #endregion

        #region Methods

        /// <summary>
        /// It retuns collection of string from comma separated entity type name
        /// </summary>
        /// <param name="entityTypeNames">Comma Separated strinf which contains entity type name</param>
        /// <returns></returns>
        public Collection<String> GetEntityTypeNames(String entityTypeNames)
        {
            Collection<String> entityTypes = new Collection<String>();

            if (!String.IsNullOrEmpty(entityTypeNames))
            {
                entityTypes = ValueTypeHelper.SplitStringToStringCollection(entityTypeNames, ',');
            }

            return entityTypes;
        }

        /// <summary>
        /// It returns title of Entity type panel
        /// </summary>
        /// <returns></returns>
        public String GetEntityTypePanelTitle()
        {
            return  this._entityTypePanelItem.Title;
        }

        /// <summary>
        /// It returns tooltip of Entity type panel
        /// </summary>
        /// <returns></returns>
        public String GetEntityTypePanelToolTip()
        {
            return this._entityTypePanelItem.ToolTip;
        }

        #endregion

        #region Serialization & Deserialization

        [System.Runtime.Serialization.OnDeserialized()]
        private void OnDeserialized(System.Runtime.Serialization.StreamingContext context)
        {
            this.SetParent();
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Generates an unique identifier for an object and all the aggregated children
        /// </summary>
        public override void GenerateNewUniqueIdentifier()
        {
            base.GenerateNewUniqueIdentifier();

            if (this._entityTypePanelItem != null)
            {
                this._entityTypePanelItem.GenerateNewUniqueIdentifier();
            }
        }

        /// <summary>
        /// Finds and returns a list of child objects for a given unique identifier
        /// </summary>
        /// <param name="uniqueIdentifier">Indicates an unique identifier of an object that needs to be found</param>
        /// <param name="includeDeletedItems">Indicates if deleted items to be included in the search</param>
        /// <returns>A list of Child objects matching the given unique identifier</returns>
        public override List<RS.MDM.Object> FindChildren(string uniqueIdentifier, bool includeDeletedItems)
        {
            List<RS.MDM.Object> list = new List<Object>();
            list.AddRange(base.FindChildren(uniqueIdentifier, includeDeletedItems));

            if (this._entityTypePanelItem != null)
            {
                list.AddRange(this._entityTypePanelItem.FindChildren(uniqueIdentifier, includeDeletedItems));
            }

            return list;
        }

        /// <summary>
        /// Sets the parent (container) for all the aggregated children
        /// </summary>
        public override void SetParent()
        {
            if (this._entityTypePanelItem != null)
            {
                this._entityTypePanelItem.Parent = this;
                this._entityTypePanelItem.InheritedParent = this.InheritedParent;
            }
        }

        /// <summary>
        /// Get a tree node that represents an object and its aggregated children
        /// </summary>
        /// <returns>A Tree Node that represents an object and its aggregated children</returns>
        public override System.Windows.Forms.TreeNode GetTreeNode()
        {
            System.Windows.Forms.TreeNode treeNode = base.GetTreeNode();

            if (this.PropertyChanges.ObjectStatus == RS.MDM.Configuration.InheritedObjectStatus.None)
            {
                treeNode.ImageKey = "NavigationPane";
                treeNode.SelectedImageKey = treeNode.ImageKey;
            }

            System.Windows.Forms.TreeNode searchPanelItem = new System.Windows.Forms.TreeNode("EntityTypePanelItem");
            searchPanelItem.ImageKey = "Item";
            searchPanelItem.SelectedImageKey = searchPanelItem.ImageKey;
            searchPanelItem.Tag = this.EntityTypePanelItem;
            treeNode.Nodes.Add(searchPanelItem);

            if (this._entityTypePanelItem != null)
            {
                searchPanelItem.Nodes.Add(this._entityTypePanelItem.GetTreeNode());
            }

            return treeNode;
        }

        /// <summary>
        /// Execute logic related to a given verb
        /// </summary>
        /// <param name="text">Indicate the text that represents a supported verb</param>
        /// <param name="configObject">Indicates configuration object on which action is being performed</param>
        /// <param name="treeView">Indicates tree view of main form</param>
        public override void OnDesignerVerbClick(string text, Object configObject, object treeView)
        {
            ConfigurationObject configurationObject = null;

            base.OnDesignerVerbClick(text, configObject, treeView);

            switch (text)
            {
                case "Add Entity Type Panel Item":
                    this.EntityTypePanelItem = new EntityTypePanelItem();
                    break;
            }

            if (text != "Find Changes" && text != "Accept Changes" && configObject != null && configObject is ConfigurationObject)
            {
                configurationObject = configObject as ConfigurationObject;
                configurationObject._isConfigDirty = true;
            }

            System.ComponentModel.TypeDescriptor.Refresh(this);
        }

        /// <summary>
        /// Get XML representation of the object
        /// </summary>
        /// <returns>XML representation of Entity Type Panel</returns>
        public override String ToXml()
        {
            String entityTypePanelXml = String.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            #region EntityTypePanel Node

            //Parameter node start
            xmlWriter.WriteStartElement("EntityTypePanel");

            #region EntityTypePanelItem Node

            xmlWriter.WriteStartElement("EntityTypePanelItem");

            if (this.EntityTypePanelItem != null)
            {
                xmlWriter.WriteRaw(this.EntityTypePanelItem.ToXml());
            }

            xmlWriter.WriteEndElement();

            #endregion EntityTypePanelItem Node

            //Value node end
            xmlWriter.WriteEndElement();

            #endregion EntityTypePanel Node

            xmlWriter.Flush();

            //Get the actual XML
            entityTypePanelXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return entityTypePanelXml;
        }

        #endregion

        #region Validations

        /// <summary>
        /// Validates an object and aggregates all the validation exceptions
        /// </summary>
        /// <param name="validationErrors">A container to aggregate all the validation exceptions</param>
        public override void Validate(ref ValidationErrorCollection validationErrors)
        {
            this.SetParent();

            if (validationErrors == null)
            {
                validationErrors = new RS.MDM.Validations.ValidationErrorCollection();
            }

            if (this.EntityTypePanelItem == null)
            {
                validationErrors.Add("The EntityTypePanel does not contain any EntityTypePanelItem.", ValidationErrorType.Warning, "EntityTypePanel", this);
            }
        }

        #endregion
    }
}
