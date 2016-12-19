using RS.MDM.Configuration;
using RS.MDM.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace RS.MDM.ConfigurationObjects
{
    /// <summary>
    /// Provides system default configuration for EntityHistory Exclude list config
    /// </summary>
    [XmlRoot("EntityHistoryExcludeListConfig")]
    [Serializable()]
    public sealed class EntityHistoryExcludeListConfig :  Object
    {
        #region LocaleConfigXml

        //<LocaleConfig xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" Id="-1" 
        //              UniqueIdentifier="292c728e-099e-4680-a63a-9307f5d55228" Name="LocaleConfig" Description="" Tag="" 
        //              ClassName="RS.MDM.Web.UI.Configuration.LocaleConfig" AssemblyName="PCWebControls.dll" 
        //              InheritedParentUId="" SystemDataLocale="en_WW" SystemUILocale="en_WW" AllowableUILocales ="en_WW,de_DE,fr_FR" 
        //              ModelDisplayLocaleType="DataLocale" DataFormattingLocaleType="DataLocale" >
        //<PropertyChanges ObjectStatus="None" />
        //</LocaleConfig>

        #endregion

        #region Fields

        private RS.MDM.Collections.Generic.List<EntityHistoryExcludeElement> _entityHistoryExcludeElements = new RS.MDM.Collections.Generic.List<EntityHistoryExcludeElement>();

        #endregion

        #region Properties

        /// <summary>
        /// Represents the change type
        /// </summary>
        [Category("EntityHistoryExcludeElements")]
        [Description("Represents entity history exclude element")]
        [TrackChanges()]
        public RS.MDM.Collections.Generic.List<EntityHistoryExcludeElement> EntityHistoryExcludeElements
        {
            get
            {
                this.SetParent();
                return this._entityHistoryExcludeElements;
            }
            set
            {
                this._entityHistoryExcludeElements = value;
                this.SetParent();
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Parameterized constructor with values as xml representation of object.
        /// </summary>
        public EntityHistoryExcludeListConfig(String ValuesAsXml)
            : base()
        {

        }

        public EntityHistoryExcludeListConfig()
            : base()
        {
            this.AddVerb("Add EntityHistoryExcludeElement");
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Get XML representation of the EntityHistoryExcludeElement object
        /// </summary>
        /// <returns>XML representation of EntityHistoryExcludeElement</returns>
        public override String ToXml()
        {
            String parameterXml = String.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //EntityHistoryExcludeListConfig node start
            xmlWriter.WriteStartElement("EntityHistoryExcludeListConfig");

            #region EntityHistoryExcludeSubElement Node

            xmlWriter.WriteStartElement("EntityHistoryExcludeElements");

            foreach (EntityHistoryExcludeElement entityHistoryExcludeElement in this.EntityHistoryExcludeElements)
            {
                xmlWriter.WriteRaw(entityHistoryExcludeElement.ToXml());
            }

            xmlWriter.WriteEndElement();

            #endregion EntityHistoryExcludeSubElement Node

            //EntityHistoryExcludeListConfig node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //Get the actual XML
            parameterXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return parameterXml;
        }

        /// <summary>
        /// Load the EntityHistoryExcludeElement object from the input xml
        /// </summary>
        /// <param name="valueAsXml">EntityHistoryExcludeElement object representation as xml</param>
        public void LoadParameter(String valueAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valueAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valueAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "EntityHistoryExcludeElement")
                        {
                            //Read EntityHistoryExcludeElement
                            #region Read EntityHistoryExcludeElement

                            String EntityHistoryExcludeSubElementXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(EntityHistoryExcludeSubElementXml))
                            {
                                EntityHistoryExcludeElement entityHistoryExcludeElement = new EntityHistoryExcludeElement(EntityHistoryExcludeSubElementXml);
                                if (entityHistoryExcludeElement != null)
                                {
                                    this.EntityHistoryExcludeElements.Add(entityHistoryExcludeElement);
                                }
                            }

                            #endregion Read EntityHistoryExcludeElement
                        }
                        else
                        {
                            //Keep on reading the xml until we reach expected node.
                            reader.Read();
                        }
                    }
                }
                finally
                {
                    if (reader != null)
                    {
                        reader.Close();
                    }
                }
            }
        }

        #endregion

        #region Serialization & Deserialization

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
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

            foreach (EntityHistoryExcludeElement entityHistoryExcludeElement in this._entityHistoryExcludeElements)
            {
                if (entityHistoryExcludeElement != null)
                {
                    entityHistoryExcludeElement.GenerateNewUniqueIdentifier();
                }
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

            foreach (EntityHistoryExcludeElement entityHistoryExcludeElement in this._entityHistoryExcludeElements)
            {
                if (entityHistoryExcludeElement != null)
                {
                    list.AddRange(entityHistoryExcludeElement.FindChildren(uniqueIdentifier, includeDeletedItems));
                }
            }

            return list;
        }

        /// <summary>
        /// Sets the parent (container) for all the aggregated children
        /// </summary>
        public override void SetParent()
        {
            if (this._entityHistoryExcludeElements != null)
            {
                foreach (EntityHistoryExcludeElement entityHistoryExcludeElement in this._entityHistoryExcludeElements)
                {
                    if (entityHistoryExcludeElement != null)
                    {
                        entityHistoryExcludeElement.Parent = this;
                        entityHistoryExcludeElement.InheritedParent = this.InheritedParent;
                    }
                }
            }

        }

        /// <summary>
        /// Accepts the changes to the object
        /// </summary>
        public override void AcceptChanges()
        {
            base.AcceptChanges();

            if (this._entityHistoryExcludeElements != null && this._entityHistoryExcludeElements.Count > 0)
            {
                for (int i = _entityHistoryExcludeElements.Count - 1; i > -1; i--)
                {
                    EntityHistoryExcludeElement entityHistoryExcludeElement = _entityHistoryExcludeElements[i];

                    if (entityHistoryExcludeElement != null)
                    {
                        if (entityHistoryExcludeElement.PropertyChanges.ObjectStatus == InheritedObjectStatus.Delete)
                        {
                            this._entityHistoryExcludeElements.Remove(entityHistoryExcludeElement);
                        }
                        else
                        {
                            entityHistoryExcludeElement.AcceptChanges();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Finds the changes of an object with respect to an instance of an inherited parent
        /// </summary>
        public override void FindChanges()
        {
            base.FindChanges();

            if (this._entityHistoryExcludeElements != null)
            {
                foreach (EntityHistoryExcludeElement entityHistoryExcludeElement in this._entityHistoryExcludeElements)
                {
                    if (entityHistoryExcludeElement != null)
                    {
                        entityHistoryExcludeElement.FindChanges();
                    }
                }
            }

            if (this.Parent == null && this.InheritedParent != null)
            {
                this.InheritedParent.FindDeletes(this);
            }
        }

        /// <summary>
        /// Finds deleted children of an inherited child
        /// </summary>
        /// <param name="inheritedChild">Indicates the inherited child</param>
        public override void FindDeletes(Object inheritedChild)
        {
            base.FindDeletes(inheritedChild);

            string previousSibling = string.Empty;

            if (this._entityHistoryExcludeElements != null)
            {
                previousSibling = string.Empty;
                foreach (EntityHistoryExcludeElement entityHistoryExcludeElement in this._entityHistoryExcludeElements)
                {
                    if (entityHistoryExcludeElement != null)
                    {
                        List<RS.MDM.Object> _items = inheritedChild.FindChildren(entityHistoryExcludeElement.UniqueIdentifier, true);

                        if (_items.Count == 0)
                        {
                            EntityHistoryExcludeElement _EntityHistoryExcludeElementClone = RS.MDM.Object.Clone(entityHistoryExcludeElement, false) as EntityHistoryExcludeElement;
                            _EntityHistoryExcludeElementClone.PropertyChanges.Items.Clear();
                            _EntityHistoryExcludeElementClone.PropertyChanges.ObjectStatus = InheritedObjectStatus.Delete;
                            ((EntityHistoryExcludeListConfig)inheritedChild).EntityHistoryExcludeElements.InsertAfter(previousSibling, _EntityHistoryExcludeElementClone);
                        }
                        else
                        {
                            entityHistoryExcludeElement.FindDeletes(_items[0]);
                        }

                        previousSibling = entityHistoryExcludeElement.UniqueIdentifier;
                    }
                }
            }
        }

        /// <summary>
        /// Inherits a parent object (instance)
        /// </summary>
        /// <param name="inheritedParent">Indicates an instance of an object that needs to be inherited</param>
        public override void InheritParent(RS.MDM.Object inheritedParent)
        {
            if (inheritedParent != null)
            {
                base.InheritParent(inheritedParent);

                EntityHistoryExcludeListConfig _inheritedParent = inheritedParent as EntityHistoryExcludeListConfig;

                string _previousSibling = string.Empty;

                // Apply all the changes

                foreach (EntityHistoryExcludeElement entityHistoryExcludeElement in this._entityHistoryExcludeElements)
                {
                    switch (entityHistoryExcludeElement.PropertyChanges.ObjectStatus)
                    {
                        case InheritedObjectStatus.Add:
                            EntityHistoryExcludeElement _entityHistoryExcludeElementClone = RS.MDM.Object.Clone(entityHistoryExcludeElement, false) as EntityHistoryExcludeElement;
                            _inheritedParent.EntityHistoryExcludeElements.InsertAfter(_previousSibling, _entityHistoryExcludeElementClone);
                            break;
                        case InheritedObjectStatus.Change:
                            EntityHistoryExcludeElement _inheritedChild = _inheritedParent.EntityHistoryExcludeElements.GetItem(entityHistoryExcludeElement.UniqueIdentifier);
                            entityHistoryExcludeElement.InheritParent(_inheritedChild);
                            break;
                    }

                    _previousSibling = entityHistoryExcludeElement.UniqueIdentifier;
                }

                // Cleanup the changes

                foreach (EntityHistoryExcludeElement entityHistoryExcludeElement in this._entityHistoryExcludeElements)
                {
                    if (entityHistoryExcludeElement.PropertyChanges.ObjectStatus == InheritedObjectStatus.Delete)
                    {
                        _inheritedParent.EntityHistoryExcludeElements.Remove(entityHistoryExcludeElement.UniqueIdentifier);
                    }
                    else
                    {
                        EntityHistoryExcludeElement _inheritedChild = _inheritedParent.EntityHistoryExcludeElements.GetItem(entityHistoryExcludeElement.UniqueIdentifier);

                        if (_inheritedChild != null)
                            _inheritedChild.PropertyChanges.Reset();
                    }
                }

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

            System.Windows.Forms.TreeNode entityHistoryExcludeElements = new System.Windows.Forms.TreeNode("EntityHistoryExcludeElements");
            entityHistoryExcludeElements.ImageKey = "EntityHistoryExcludeListConfig";
            entityHistoryExcludeElements.SelectedImageKey = entityHistoryExcludeElements.ImageKey;
            entityHistoryExcludeElements.Tag = this.EntityHistoryExcludeElements;
            treeNode.Nodes.Add(entityHistoryExcludeElements);

            foreach (EntityHistoryExcludeElement entityHistoryExcludeElement in this._entityHistoryExcludeElements)
            {
                if (entityHistoryExcludeElement != null)
                {
                    entityHistoryExcludeElements.Nodes.Add(entityHistoryExcludeElement.GetTreeNode());
                }
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
                case "Add EntityHistoryExcludeElement":
                    this.EntityHistoryExcludeElements.Add(new EntityHistoryExcludeElement());
                    break;
            }

            if (text != "Find Changes" && text != "Accept Changes" && configObject != null && configObject is ConfigurationObject)
            {
                configurationObject = configObject as ConfigurationObject;
                configurationObject._isConfigDirty = true;
            }

            System.ComponentModel.TypeDescriptor.Refresh(this);
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

            if (this.EntityHistoryExcludeElements.Count == 0)
            {
                validationErrors.Add("The Entity history exclude list config does not contain any Element.", ValidationErrorType.Warning, "EntityHistoryExcludeListConfig", this);
            }

        }

        #endregion
    }
}
