using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Drawing.Design;

namespace RS.MDM.ConfigurationObjects
{
    using RS.MDM.Validations;
    using RS.MDM.Configuration;

    /// <summary>
    /// Provides configuration for translation
    /// </summary>
    [XmlRoot("TranslationConfig")]
    [Serializable()]
    [XmlInclude(typeof(Translation))]
    public sealed class TranslationConfig : RS.MDM.Object
    {
        #region Field
        /// <summary>
        /// Field indicates collection of translation object happens from one language to another language
        /// </summary>
        private RS.MDM.Collections.Generic.List<Translation> _translations = new RS.MDM.Collections.Generic.List<Translation>();

        #endregion Fields

        #region Properties
        
        /// <summary>
        /// Collection of translation to process
        /// </summary>
        [Description("Indicates translations")]
        [Category("Translations")]
        [TrackChanges()]
        [Editor(typeof(RS.MDM.ComponentModel.Design.CollectionEditor), typeof(UITypeEditor))]
        public RS.MDM.Collections.Generic.List<Translation> Translations
        {
            get
            {
                this.SetParent();
                return this._translations;
            }
            set
            {
                this._translations = value;
                this.SetParent();
            }
        }

        #endregion Properties
       
        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public TranslationConfig()
            : base()
        {
            this.AddVerb("Add New Translation");
        }

        #endregion

        #region Method

        #region Serialization and deserialization
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context">Additional Context used for deserialization</param>
        [System.Runtime.Serialization.OnDeserialized()]
        private void OnDeserialized(System.Runtime.Serialization.StreamingContext context)
        {
            this.SetParent();
        }

        #endregion Serialization and deserialization

        #region Override
        /// <summary>
        /// Generates an unique identifier for an object and all the aggregated children
        /// </summary>
        public override void GenerateNewUniqueIdentifier()
        {
            base.GenerateNewUniqueIdentifier();

            foreach (Translation _translationItem in this._translations)
            {
                if (_translationItem != null)
                {
                    _translationItem.GenerateNewUniqueIdentifier();
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
            List<RS.MDM.Object> _list = new List<Object>();
            _list.AddRange(base.FindChildren(uniqueIdentifier, includeDeletedItems));

            foreach (Translation _translationItem in this._translations)
            {
                if (_translationItem != null)
                {
                    _list.AddRange(_translationItem.FindChildren(uniqueIdentifier, includeDeletedItems));
                }
            }

            return _list;
        }

        /// <summary>
        /// Sets the parent (container) for all the aggregated children
        /// </summary>
        public override void SetParent()
        {
            if (this._translations != null)
            {
                foreach (Translation _translationItem in this._translations)
                {
                    if (_translationItem != null)
                    {
                        _translationItem.Parent = this;
                        _translationItem.InheritedParent = this.InheritedParent;
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

            if (this._translations != null && this._translations.Count > 0)
            {
                for (int i = _translations.Count - 1; i > -1; i--)
                {
                    Translation _translationItem = _translations[i];

                    if (_translationItem != null)
                    {
                        if (_translationItem.PropertyChanges.ObjectStatus == InheritedObjectStatus.Delete)
                        {
                            this._translations.Remove(_translationItem);
                        }
                        else
                        {
                            _translationItem.AcceptChanges();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Finds the changes of an object wrt an instance of an inherited parent
        /// </summary>
        public override void FindChanges()
        {
            base.FindChanges();

            if (this._translations != null)
            {
                foreach (Translation _translationItem in this._translations)
                {
                    if (_translationItem != null)
                    {
                        _translationItem.FindChanges();
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

            string _previousSibling = string.Empty;

            if (this._translations != null)
            {
                _previousSibling = string.Empty;
                foreach (Translation _translationItem in this._translations)
                {
                    if (_translationItem != null)
                    {
                        List<RS.MDM.Object> _items = inheritedChild.FindChildren(_translationItem.UniqueIdentifier, true);

                        if (_items.Count == 0)
                        {
                            Translation _translationItemClone = RS.MDM.Object.Clone(_translationItem, false) as Translation;
                            _translationItemClone.PropertyChanges.Items.Clear();
                            _translationItemClone.PropertyChanges.ObjectStatus = InheritedObjectStatus.Delete;
                            ((TranslationConfig)inheritedChild).Translations.InsertAfter(_previousSibling, _translationItemClone);
                        }
                        else
                        {
                            _translationItem.FindDeletes(_items[0]);
                        }

                        _previousSibling = _translationItem.UniqueIdentifier;
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

                TranslationConfig _inheritedParent = inheritedParent as TranslationConfig;

                string _previousSibling = string.Empty;

                foreach (Translation _translationItem in this._translations)
                {
                    switch (_translationItem.PropertyChanges.ObjectStatus)
                    {
                        case InheritedObjectStatus.Add:
                            Translation _translationItemClone = RS.MDM.Object.Clone(_translationItem, false) as Translation;
                            _inheritedParent.Translations.InsertAfter(_previousSibling, _translationItemClone);
                            break;
                        case InheritedObjectStatus.Change:
                            Translation _inheritedChild = _inheritedParent.Translations.GetItem(_translationItem.UniqueIdentifier);
                            _translationItem.InheritParent(_inheritedChild);
                            break;
                    }

                    _previousSibling = _translationItem.UniqueIdentifier;
                }

                // Cleanup the changes
                foreach (Translation _translationItem in this._translations)
                {
                    if (_translationItem.PropertyChanges.ObjectStatus == InheritedObjectStatus.Delete)
                    {
                        _inheritedParent.Translations.Remove(_translationItem.UniqueIdentifier);
                    }
                    else
                    {
                        Translation _inheritedChild = _inheritedParent.Translations.GetItem(_translationItem.UniqueIdentifier);

                        if (_inheritedChild != null)
                            _inheritedChild.PropertyChanges.Reset();
                    }
                }
            }
        }

        /// <summary>
        /// Get a tree node that reprents an object and its aggregated children
        /// </summary>
        /// <returns>A Tree Node that represents an object and its aggregated children</returns>
        public override System.Windows.Forms.TreeNode GetTreeNode()
        {
            System.Windows.Forms.TreeNode _treeNode = base.GetTreeNode();

            if (this.PropertyChanges.ObjectStatus == RS.MDM.Configuration.InheritedObjectStatus.None)
            {
                _treeNode.ImageKey = "NavigationPane";
                _treeNode.SelectedImageKey = _treeNode.ImageKey;
            }

            System.Windows.Forms.TreeNode translationNodes = new System.Windows.Forms.TreeNode("Translations");
            translationNodes.ImageKey = "Items";
            translationNodes.SelectedImageKey = translationNodes.ImageKey;
            translationNodes.Tag = this.Translations;
            _treeNode.Nodes.Add(translationNodes);

            foreach (Translation _translationItem in this._translations)
            {
                if (_translationItem != null)
                {
                    translationNodes.Nodes.Add(_translationItem.GetTreeNode());
                }
            }

            return _treeNode;
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
                case "Add New Translation":
                    this.Translations.Add(new Translation());
                    break;
            }

            if (text != "Find Changes" && text != "Accept Changes" && configObject != null && configObject is ConfigurationObject)
            {
                configurationObject = configObject as ConfigurationObject;
                configurationObject._isConfigDirty = true;
            }

            System.ComponentModel.TypeDescriptor.Refresh(this);
        }

        #endregion Override

        #region Validation

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

            if (this.Translations.Count == 0)
            {
                validationErrors.Add("The Translation Config does not contain any translations.", ValidationErrorType.Warning, "TranslationConfig", this);
            }
        }

        #endregion Validation

        #endregion Method
    }
}
