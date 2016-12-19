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
    /// Provides system default configuration for EntityHistory Exclude list Element config
    /// </summary>
    [XmlRoot("EntityHistoryExcludeElement")]
    [Serializable()]
    public sealed class EntityHistoryExcludeElement : Object
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

        private RS.MDM.Collections.Generic.List<EntityHistoryExcludeSubElement> _entityHistoryExcludeSubElements = new RS.MDM.Collections.Generic.List<EntityHistoryExcludeSubElement>();

        #endregion

        #region Properties

        /// <summary>
        /// Represents the change type
        /// </summary>
        [Category("EntityHistoryExcludeSubElements")]
        [Description("Represents entity history exclude sub element")]
        [TrackChanges()]
        public RS.MDM.Collections.Generic.List<EntityHistoryExcludeSubElement> EntityHistoryExcludeSubElements
        {
            get
            {
                this.SetParent();
                return this._entityHistoryExcludeSubElements;
            }
            set
            {
                this._entityHistoryExcludeSubElements = value;
                this.SetParent();
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Parameterized constructor with values as xml representation of object.
        /// </summary>
        public EntityHistoryExcludeElement(String ValuesAsXml)
            : base()
        {

        }


        public EntityHistoryExcludeElement()
            : base()
        {
            this.AddVerb("Add EntityHistoryExcludeSubElement");
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

            //EntityHistoryExcludeElement node start
            xmlWriter.WriteStartElement("EntityHistoryExcludeElement");

            #region EntityHistoryExcludeSubElement Node

            xmlWriter.WriteStartElement("EntityHistoryExcludeSubElements");

            foreach (EntityHistoryExcludeSubElement entityHistoryExcludeSubElement in this.EntityHistoryExcludeSubElements)
            {
                xmlWriter.WriteRaw(entityHistoryExcludeSubElement.ToXml());
            }

            xmlWriter.WriteEndElement();

            #endregion EntityHistoryExcludeSubElement Node

            //EntityHistoryExcludeElement node end
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
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "EntityHistoryExcludeSubElement")
                        {
                            //Read EntityHistoryExcludeSubElement
                            #region Read EntityHistoryExcludeSubElement

                            String EntityHistoryExcludeSubElementXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(EntityHistoryExcludeSubElementXml))
                            {
                                EntityHistoryExcludeSubElement entityHistoryExcludeSubElement = new EntityHistoryExcludeSubElement(EntityHistoryExcludeSubElementXml);
                                if (entityHistoryExcludeSubElement != null)
                                {
                                    this.EntityHistoryExcludeSubElements.Add(entityHistoryExcludeSubElement);
                                }
                            }

                            #endregion Read EntityHistoryExcludeSubElement
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

            foreach (EntityHistoryExcludeSubElement entityHistoryExcludeSubElement in this._entityHistoryExcludeSubElements)
            {
                if (entityHistoryExcludeSubElement != null)
                {
                    entityHistoryExcludeSubElement.GenerateNewUniqueIdentifier();
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

            foreach (EntityHistoryExcludeSubElement entityHistoryExcludeSubElement in this._entityHistoryExcludeSubElements)
            {
                if (entityHistoryExcludeSubElement != null)
                {
                    list.AddRange(entityHistoryExcludeSubElement.FindChildren(uniqueIdentifier, includeDeletedItems));
                }
            }

            return list;
        }

        /// <summary>
        /// Sets the parent (container) for all the aggregated children
        /// </summary>
        public override void SetParent()
        {
            if (this._entityHistoryExcludeSubElements != null)
            {
                foreach (EntityHistoryExcludeSubElement entityHistoryExcludeSubElement in this._entityHistoryExcludeSubElements)
                {
                    if (entityHistoryExcludeSubElement != null)
                    {
                        entityHistoryExcludeSubElement.Parent = this;
                        entityHistoryExcludeSubElement.InheritedParent = this.InheritedParent;
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

            if (this._entityHistoryExcludeSubElements != null && this._entityHistoryExcludeSubElements.Count > 0)
            {
                for (int i = _entityHistoryExcludeSubElements.Count - 1; i > -1; i--)
                {
                    EntityHistoryExcludeSubElement entityHistoryExcludeSubElement = _entityHistoryExcludeSubElements[i];

                    if (entityHistoryExcludeSubElement != null)
                    {
                        if (entityHistoryExcludeSubElement.PropertyChanges.ObjectStatus == InheritedObjectStatus.Delete)
                        {
                            this._entityHistoryExcludeSubElements.Remove(entityHistoryExcludeSubElement);
                        }
                        else
                        {
                            entityHistoryExcludeSubElement.AcceptChanges();
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

            if (this._entityHistoryExcludeSubElements != null)
            {
                foreach (EntityHistoryExcludeSubElement entityHistoryExcludeSubElement in this._entityHistoryExcludeSubElements)                
                {
                    if (entityHistoryExcludeSubElement != null)
                    {
                        entityHistoryExcludeSubElement.FindChanges();
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

            if (this._entityHistoryExcludeSubElements != null)
            {
                previousSibling = string.Empty;
                foreach (EntityHistoryExcludeSubElement entityHistoryExcludeSubElement in this._entityHistoryExcludeSubElements)                
                {
                    if (entityHistoryExcludeSubElement != null)
                    {
                        List<RS.MDM.Object> _items = inheritedChild.FindChildren(entityHistoryExcludeSubElement.UniqueIdentifier, true);

                        if (_items.Count == 0)
                        {
                            EntityHistoryExcludeSubElement _EntityHistoryExcludeSubElementClone = RS.MDM.Object.Clone(entityHistoryExcludeSubElement, false) as EntityHistoryExcludeSubElement;
                            _EntityHistoryExcludeSubElementClone.PropertyChanges.Items.Clear();
                            _EntityHistoryExcludeSubElementClone.PropertyChanges.ObjectStatus = InheritedObjectStatus.Delete;
                            ((EntityHistoryExcludeElement)inheritedChild).EntityHistoryExcludeSubElements.InsertAfter(previousSibling, _EntityHistoryExcludeSubElementClone);
                        }
                        else
                        {
                            entityHistoryExcludeSubElement.FindDeletes(_items[0]);
                        }

                        previousSibling = entityHistoryExcludeSubElement.UniqueIdentifier;
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

                EntityHistoryExcludeElement _inheritedParent = inheritedParent as EntityHistoryExcludeElement;

                string _previousSibling = string.Empty;

                // Apply all the changes

                foreach (EntityHistoryExcludeSubElement entityHistoryExcludeSubElement in this._entityHistoryExcludeSubElements)
                {
                    switch (entityHistoryExcludeSubElement.PropertyChanges.ObjectStatus)
                    {
                        case InheritedObjectStatus.Add:
                            EntityHistoryExcludeSubElement _EntityHistoryExcludeSubElementClone = RS.MDM.Object.Clone(entityHistoryExcludeSubElement, false) as EntityHistoryExcludeSubElement;
                            _inheritedParent.EntityHistoryExcludeSubElements.InsertAfter(_previousSibling, _EntityHistoryExcludeSubElementClone);
                            break;
                        case InheritedObjectStatus.Change:
                            EntityHistoryExcludeSubElement _inheritedChild = _inheritedParent.EntityHistoryExcludeSubElements.GetItem(entityHistoryExcludeSubElement.UniqueIdentifier);
                            entityHistoryExcludeSubElement.InheritParent(_inheritedChild);
                            break;
                    }

                    _previousSibling = entityHistoryExcludeSubElement.UniqueIdentifier;
                }

                // Cleanup the changes

                foreach (EntityHistoryExcludeSubElement entityHistoryExcludeSubElement in this._entityHistoryExcludeSubElements)
                {
                    if (entityHistoryExcludeSubElement.PropertyChanges.ObjectStatus == InheritedObjectStatus.Delete)
                    {
                        _inheritedParent._entityHistoryExcludeSubElements.Remove(entityHistoryExcludeSubElement.UniqueIdentifier);
                    }
                    else
                    {
                        EntityHistoryExcludeSubElement _inheritedChild = _inheritedParent.EntityHistoryExcludeSubElements.GetItem(entityHistoryExcludeSubElement.UniqueIdentifier);

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

            System.Windows.Forms.TreeNode entityHistoryExcludeSubElements = new System.Windows.Forms.TreeNode("EntityHistoryExcludeSubElements");
            entityHistoryExcludeSubElements.ImageKey = "EntityHistoryExcludeSubElement";
            entityHistoryExcludeSubElements.SelectedImageKey = entityHistoryExcludeSubElements.ImageKey;
            entityHistoryExcludeSubElements.Tag = this.EntityHistoryExcludeSubElements;
            treeNode.Nodes.Add(entityHistoryExcludeSubElements);

            foreach (EntityHistoryExcludeSubElement entityHistoryExcludeSubElement in this._entityHistoryExcludeSubElements)
            {
                if (entityHistoryExcludeSubElement != null)
                {
                    entityHistoryExcludeSubElements.Nodes.Add(entityHistoryExcludeSubElement.GetTreeNode());
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
                case "Add EntityHistoryExcludeSubElement":
                    this.EntityHistoryExcludeSubElements.Add(new EntityHistoryExcludeSubElement());
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

            if (this.EntityHistoryExcludeSubElements.Count == 0)
            {
                validationErrors.Add("The EntityHistory exclude elements does not contain any sub elements.", ValidationErrorType.Warning, "EntityHistoryExcludeElement", this);
            }
        }

        #endregion
    }
}
