using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.ComponentModel;
using RS.MDM.Configuration;
using System.Drawing.Design;
using RS.MDM.Validations;

namespace RS.MDM.ConfigurationObjects
{
    /// <summary>
    /// Provides configuration for AttributePanel
    /// </summary>
    [XmlRoot("EntityHierarchyPanelConfig")]
    [Serializable()]
    [XmlInclude(typeof(EntityHierarchyPanelConfigItem))]
    public class EntityHierarchyPanelConfig: Object
    {
        #region Fields

        /// <summary>
        /// field for EntityHierarchyPanelConfigItems
        /// </summary>
        private RS.MDM.Collections.Generic.List<EntityHierarchyPanelConfigItem> _entityHierarchyConfigurationItems = new RS.MDM.Collections.Generic.List<EntityHierarchyPanelConfigItem>();

        #endregion

        #region Properties

        /// <summary>
        /// Indicates EntityHierarchyPanelConfigItems
        /// </summary>
        [Category("EntityHierarchyPanelConfigItem")]
        [TrackChanges()]
        [Editor(typeof(RS.MDM.ComponentModel.Design.CollectionEditor), typeof(UITypeEditor))]
        public RS.MDM.Collections.Generic.List<EntityHierarchyPanelConfigItem> EntityHierarchyConfigurationItems
        {
            get
            {
                this.SetParent();
                return this._entityHierarchyConfigurationItems;
            }
            set
            {
                this._entityHierarchyConfigurationItems = value;
                this.SetParent();
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Parameter-less constructor
        /// </summary>
        public EntityHierarchyPanelConfig()
            : base()
        {
            
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

            foreach (EntityHierarchyPanelConfigItem _item in this._entityHierarchyConfigurationItems)
            {
                if (_item != null)
                {
                    _item.GenerateNewUniqueIdentifier();
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

            foreach (EntityHierarchyPanelConfigItem _item in this._entityHierarchyConfigurationItems)
            {
                _list.AddRange(_item.FindChildren(uniqueIdentifier, includeDeletedItems));
            }

            return _list;
        }

        /// <summary>
        /// Sets the parent (container) for all the aggregated children
        /// </summary>
        public override void SetParent()
        {
            if (this._entityHierarchyConfigurationItems != null)
            {
                foreach (EntityHierarchyPanelConfigItem _item in this._entityHierarchyConfigurationItems)
                {
                    if (_item != null)
                    {
                        _item.Parent = this;
                        _item.InheritedParent = this.InheritedParent;
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

            if (this._entityHierarchyConfigurationItems != null && this._entityHierarchyConfigurationItems.Count > 0)
            {
                for (int i = _entityHierarchyConfigurationItems.Count - 1; i > -1; i--)
                {
                    EntityHierarchyPanelConfigItem _item = _entityHierarchyConfigurationItems[i];

                    if (_item != null)
                    {
                        if (_item.PropertyChanges.ObjectStatus == InheritedObjectStatus.Delete)
                        {
                            this._entityHierarchyConfigurationItems.Remove(_item);
                        }
                        else
                        {
                            _item.AcceptChanges();
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

            if (this._entityHierarchyConfigurationItems != null)
            {
                foreach (EntityHierarchyPanelConfigItem _item in this._entityHierarchyConfigurationItems)
                {
                    if (_item != null)
                    {
                        _item.FindChanges();
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

            if (this._entityHierarchyConfigurationItems != null)
            {
                _previousSibling = string.Empty;
                foreach (EntityHierarchyPanelConfigItem _item in this._entityHierarchyConfigurationItems)
                {
                    if (_item != null)
                    {
                        List<RS.MDM.Object> _items = inheritedChild.FindChildren(_item.UniqueIdentifier, true);
                        if (_items.Count == 0)
                        {
                            Panel _panelClone = RS.MDM.Object.Clone(_item, false) as Panel;
                            _panelClone.PropertyChanges.Items.Clear();
                            _panelClone.PropertyChanges.ObjectStatus = InheritedObjectStatus.Delete;
                            ((PanelBar)inheritedChild).Panels.InsertAfter(_previousSibling, _panelClone);
                        }
                        else
                        {
                            _item.FindDeletes(_items[0]);
                        }
                        _previousSibling = _item.UniqueIdentifier;
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
                PanelBar _inheritedParent = inheritedParent as PanelBar;
                string _previousSibling = string.Empty;

                // Apply all the changes
                foreach (EntityHierarchyPanelConfigItem _item in this._entityHierarchyConfigurationItems)
                {
                    switch (_item.PropertyChanges.ObjectStatus)
                    {
                        case InheritedObjectStatus.Add:
                            Panel _panelClone = RS.MDM.Object.Clone(_item, false) as Panel;
                            _inheritedParent.Panels.InsertAfter(_previousSibling, _panelClone);
                            break;
                        case InheritedObjectStatus.Change:
                            Panel _inheritedChild = _inheritedParent.Panels.GetItem(_item.UniqueIdentifier);
                            _item.InheritParent(_inheritedChild);
                            break;
                    }

                    _previousSibling = _item.UniqueIdentifier;
                }

                // Cleanup the changes
                foreach (EntityHierarchyPanelConfigItem _item in this._entityHierarchyConfigurationItems)
                {
                    if (_item.PropertyChanges.ObjectStatus == InheritedObjectStatus.Delete)
                    {
                        _inheritedParent.Panels.Remove(_item.UniqueIdentifier);
                    }
                    else
                    {
                        Panel _inheritedChild = _inheritedParent.Panels.GetItem(_item.UniqueIdentifier);

                        if (_inheritedChild != null)
                            _inheritedChild.PropertyChanges.Reset();
                    }
                }
            }
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

            if (this._entityHierarchyConfigurationItems.Count == 0)
            {
                validationErrors.Add("The entityHierarchyConfiguration does not contain any items", ValidationErrorType.Warning, "EntityHierarchyPanelConfigItems", this);
            }
        }

        #endregion
    }
}
