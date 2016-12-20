using System;
using System.Data;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using System.Drawing.Design;
using System.Configuration;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Web.UI.WebControls;
using System.Collections.Generic;

using MDM.Core;

namespace RS.MDM.ConfigurationObjects
{
    using RS.MDM.Validations;
    using RS.MDM.Configuration;

    /// <summary>
    /// Provides configuration for translation
    /// </summary>
    [XmlRoot("Translation")]
    [Serializable()]
    public sealed class Translation : RS.MDM.Object
    {
        #region Field

        /// <summary>
        /// Field indicates source language for translation
        /// </summary>
        private string _sourceLanguage = string.Empty;

        /// <summary>
        /// field indicates target language for translation
        /// </summary>
        private string _targetLanguage = string.Empty;

        /// <summary>
        /// Field indicating default type to consider for Target Locale value for this translation
        /// </summary>
        private TranslationExportTargetValueType _defaultTargetValueType = TranslationExportTargetValueType.SourceValue;

        /// <summary>
        /// Field indicates multistep translation is needed
        /// </summary>
        private bool _isMultiStepTranslation = false;

        /// <summary>
        /// Indicates weightage for translation
        /// </summary>
        private int _weightage = 0;

        /// <summary>
        /// Indicates Number number of translationsteps need to taken a place (Depends on IsMultiStepTransaltion)
        /// </summary>
        private RS.MDM.Collections.Generic.List<TranslationStep> _translationSteps = new RS.MDM.Collections.Generic.List<TranslationStep>();

        #endregion

        #region Properties

        /// <summary>
        /// Specifies source language for translation
        /// </summary>
        [XmlAttribute("SourceLanguage")]
        [Description("Indicates Source Language For Translation")]
        [Category("Translation")]
        [TrackChanges()]
        public string SourceLanguage
        {
            get
            {
                return _sourceLanguage;
            }
            set
            {
                _sourceLanguage = value;
            }
        }

        /// <summary>
        /// Specifies target language for translation
        /// </summary>
        [XmlAttribute("TargetLanguage")]
        [Description("Indicates Target Language For Translation")]
        [Category("Translation")]
        [TrackChanges()]
        public string TargetLanguage
        {
            get
            {
                return _targetLanguage;
            }
            set
            {
                _targetLanguage = value;
            }
        }

        /// <summary>
        /// Specifies default type to consider for Target Locale value for this translation
        /// </summary>
        [XmlAttribute("DefaultTargetValueType")]
        [Description("Indicates default type to consider for Target Locale value for Translation")]
        [Category("Translation")]
        [TrackChanges()]
        public TranslationExportTargetValueType DefaultTargetValueType
        {
            get
            {
                return _defaultTargetValueType;
            }
            set
            {
                _defaultTargetValueType = value;
            }
        }

        /// <summary>
        /// Specifies if translation is multi step process
        /// </summary>
        [XmlAttribute("IsMultiStepTranslation")]
        [Description("Indicates Translation is of multisteps or not")]
        [Category("Translation")]
        [TrackChanges()]
        public bool IsMultiStepTranslation
        {
            get
            {
                return _isMultiStepTranslation;
            }
            set
            {
                _isMultiStepTranslation = value;
            }
        }

        /// <summary>
        /// Specifies weightage for translation
        /// </summary>
        [XmlAttribute("Weightage")]
        [Description("Indicates Weightage For Translation")]
        [Category("Translation")]
        [TrackChanges()]
        public int Weightage
        {
            get
            {
                return _weightage;
            }
            set
            {
                _weightage = value;
            }
        }

        /// <summary>
        /// Specifies steps for translation
        /// </summary>
        [Description("Indicates Number of Translation Steps For Translation")]
        [Category("Translation")]
        [TrackChanges()]
        [Editor(typeof(RS.MDM.ComponentModel.Design.CollectionEditor), typeof(UITypeEditor))]
        public RS.MDM.Collections.Generic.List<TranslationStep> TranslationSteps
        {
            get
            {
                this.SetParent();
                return this._translationSteps;
            }
            set
            {
                this._translationSteps = value;
                this.SetParent();
            }
        }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public Translation()
            : base()
        {
            this.AddVerb("Add Translation Step");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="targetLanguage"></param>
        /// <param name="visibility"></param>
        /// <param name="toolTipFormat"></param>
        public Translation(int id, string sourceLanguage, string targetLanguage, bool isMultiStepTranslation, int weightage)
            : base()
        {
            this.AddVerb("Add Translation Step");

            this.Id = id;
            this.SourceLanguage = sourceLanguage;
            this.TargetLanguage = targetLanguage;
            this.IsMultiStepTranslation = isMultiStepTranslation;
            this.Weightage = weightage;
        }

        #endregion

        #region Methods

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

            foreach (TranslationStep _translationStep in this._translationSteps)
            {
                if (_translationStep != null)
                {
                    _translationStep.GenerateNewUniqueIdentifier();
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

            foreach (TranslationStep _translationStep in this._translationSteps)
            {
                if (_translationStep != null)
                {
                    _list.AddRange(_translationStep.FindChildren(uniqueIdentifier, includeDeletedItems));
                }
            }

            return _list;
        }

        /// <summary>
        /// Sets the parent (container) for all the aggregated children
        /// </summary>
        public override void SetParent()
        {
            if (this._translationSteps != null)
            {
                foreach (TranslationStep _translationStep in this._translationSteps)
                {
                    if (_translationStep != null)
                    {
                        _translationStep.Parent = this;
                        _translationStep.InheritedParent = this.InheritedParent;
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

            if (this._translationSteps != null && this._translationSteps.Count > 0)
            {
                for (int i = _translationSteps.Count - 1; i > -1; i--)
                {
                    TranslationStep _stepItem = _translationSteps[i];

                    if (_stepItem != null)
                    {
                        if (_stepItem.PropertyChanges.ObjectStatus == InheritedObjectStatus.Delete)
                        {
                            this._translationSteps.Remove(_stepItem);
                        }
                        else
                        {
                            _stepItem.AcceptChanges();
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

            if (this._translationSteps != null)
            {
                foreach (TranslationStep _translationStepItem in this._translationSteps)
                {
                    if (_translationStepItem != null)
                    {
                        _translationStepItem.FindChanges();
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

            if (this._translationSteps != null)
            {
                _previousSibling = string.Empty;
                foreach (TranslationStep _translationStepItem in this._translationSteps)
                {
                    if (_translationStepItem != null)
                    {
                        List<RS.MDM.Object> _items = inheritedChild.FindChildren(_translationStepItem.UniqueIdentifier, true);

                        if (_items.Count == 0)
                        {
                            TranslationStep _translationStepClone = RS.MDM.Object.Clone(_translationStepItem, false) as TranslationStep;
                            _translationStepClone.PropertyChanges.Items.Clear();
                            _translationStepClone.PropertyChanges.ObjectStatus = InheritedObjectStatus.Delete;
                            ((Translation)inheritedChild).TranslationSteps.InsertAfter(_previousSibling, _translationStepClone);
                        }
                        else
                        {
                            _translationStepItem.FindDeletes(_items[0]);
                        }

                        _previousSibling = _translationStepItem.UniqueIdentifier;
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

                Translation _inheritedParent = inheritedParent as Translation;

                string _previousSibling = string.Empty;

                foreach (TranslationStep _translationStepItem in this._translationSteps)
                {
                    switch (_translationStepItem.PropertyChanges.ObjectStatus)
                    {
                        case InheritedObjectStatus.Add:
                            TranslationStep _translationStepClone = RS.MDM.Object.Clone(_translationStepItem, false) as TranslationStep;
                            _inheritedParent.TranslationSteps.InsertAfter(_previousSibling, _translationStepClone);
                            break;
                        case InheritedObjectStatus.Change:
                            TranslationStep _inheritedChild = _inheritedParent.TranslationSteps.GetItem(_translationStepItem.UniqueIdentifier);
                            _translationStepItem.InheritParent(_inheritedChild);
                            break;
                    }

                    _previousSibling = _translationStepItem.UniqueIdentifier;
                }

                // Cleanup the changes
                foreach (TranslationStep _translationStepItem in this._translationSteps)
                {
                    if (_translationStepItem.PropertyChanges.ObjectStatus == InheritedObjectStatus.Delete)
                    {
                        _inheritedParent.TranslationSteps.Remove(_translationStepItem.UniqueIdentifier);
                    }
                    else
                    {
                        TranslationStep _inheritedChild = _inheritedParent.TranslationSteps.GetItem(_translationStepItem.UniqueIdentifier);

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

            System.Windows.Forms.TreeNode translationStepNode = new System.Windows.Forms.TreeNode("TranslationSteps");
            translationStepNode.ImageKey = "Items";
            translationStepNode.SelectedImageKey = translationStepNode.ImageKey;
            translationStepNode.Tag = this.TranslationSteps;
            _treeNode.Nodes.Add(translationStepNode);

            foreach (TranslationStep _translationStepItem in this._translationSteps)
            {
                if (_translationStepItem != null)
                {
                    translationStepNode.Nodes.Add(_translationStepItem.GetTreeNode());
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
                case "Add Translation Step":
                    this.TranslationSteps.Add(new TranslationStep());
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

            if (this.TranslationSteps.Count == 0)
            {
                validationErrors.Add("The Translation does not contain any Translation Steps", ValidationErrorType.Warning, "Translation", this);
            }
        }

        #endregion
    }
}
