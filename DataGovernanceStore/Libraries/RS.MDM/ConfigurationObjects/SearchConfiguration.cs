using System;
using System.IO;
using System.Xml;
using System.Data;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Collections.Generic;
using core = MDM.Core;

namespace RS.MDM.ConfigurationObjects
{
    using RS.MDM.Validations;
    using RS.MDM.Configuration;

    /// <summary>
    /// Represents SearchConfiguration object
    /// </summary>
    [XmlRoot("SearchConfiguration")]
    [Serializable()]
    public class SearchConfiguration : Object
    {
        #region Fields

        /// <summary>
        /// Represents the parameter list for the SearchConfiguration
        /// </summary>
        private RS.MDM.Collections.Generic.List<Parameter> _parameters = new Collections.Generic.List<Parameter>();

        
        #endregion

        #region Properties
        /// <summary>
        /// Represents the parameter list for the SearchConfiguration
        /// </summary>
        [Category("Parameters")]
        [Description("Represents the parameter list for the SearchConfiguration")]
        [TrackChanges()]
        public RS.MDM.Collections.Generic.List<Parameter> Parameters
        {
            get
            {
                this.SetParent();
                return this._parameters;
            }
            set
            {
                this._parameters = value;
                this.SetParent();
            }
        }

        #endregion

        #region Constructors

         /// <summary>
        /// Initializes a new instance of the SearchConfiguration class.
        /// </summary>
        public SearchConfiguration()
            : base()
        {
            this.AddVerb("Add Parameter");
        }


        #endregion

        #region Serialization & Deserialization

        /// <summary>
        /// Generates an unique identifier for an object and all the aggregated children
        /// </summary>
        public override void GenerateNewUniqueIdentifier()
        {
            base.GenerateNewUniqueIdentifier();

            foreach (Parameter param in this._parameters)
            {
                if (param != null)
                {
                    param.GenerateNewUniqueIdentifier();
                }
            }
        }



        #endregion

        #region Overrides
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

            foreach (Parameter param in this._parameters)
            {
                if (param != null)
                {
                    list.AddRange(param.FindChildren(uniqueIdentifier, includeDeletedItems));
                }
            }

            return list;
        }

        /// <summary>
        /// Sets the parent (container) for all the aggregated children
        /// </summary>
        public override void SetParent()
        {
            if (this._parameters != null)
            {
                foreach (Parameter param in this._parameters)
                {
                    if (param != null)
                    {
                        param.Parent = this;
                        param.InheritedParent = this.InheritedParent;
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

            if (this._parameters != null && this._parameters.Count > 0)
            {
                for (int i = _parameters.Count - 1; i > -1; i--)
                {
                    Parameter param = _parameters[i];

                    if (param != null)
                    {
                        if (param.PropertyChanges.ObjectStatus == InheritedObjectStatus.Delete)
                        {
                            this._parameters.Remove(param);
                        }
                        else
                        {
                            param.AcceptChanges();
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

            if (this._parameters != null)
            {
                foreach (Parameter param in this._parameters)
                {
                    if (param != null)
                    {
                        param.FindChanges();
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

            if (this._parameters != null)
            {
                previousSibling = string.Empty;
                foreach (Parameter param in this._parameters)
                {
                    if (param != null)
                    {
                        List<RS.MDM.Object> _items = inheritedChild.FindChildren(param.UniqueIdentifier, true);

                        if (_items.Count == 0)
                        {
                            Parameter _dataItemClone = RS.MDM.Object.Clone(param, false) as Parameter;
                            _dataItemClone.PropertyChanges.Items.Clear();
                            _dataItemClone.PropertyChanges.ObjectStatus = InheritedObjectStatus.Delete;
                            ((SearchConfiguration)inheritedChild).Parameters.InsertAfter(previousSibling, _dataItemClone);
                        }
                        else
                        {
                            param.FindDeletes(_items[0]);
                        }

                        previousSibling = param.UniqueIdentifier;
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

                SearchConfiguration _inheritedParent = inheritedParent as SearchConfiguration;

                string _previousSibling = string.Empty;

                // Apply all the changes

                foreach (Parameter param in this._parameters)
                {
                    switch (param.PropertyChanges.ObjectStatus)
                    {
                        case InheritedObjectStatus.Add:
                            Parameter _dataItemClone = RS.MDM.Object.Clone(param, false) as Parameter;
                            _inheritedParent.Parameters.InsertAfter(_previousSibling, _dataItemClone);
                            break;
                        case InheritedObjectStatus.Change:
                            Parameter _inheritedChild = _inheritedParent.Parameters.GetItem(param.UniqueIdentifier);
                            param.InheritParent(_inheritedChild);
                            break;
                    }

                    _previousSibling = param.UniqueIdentifier;
                }

                // Cleanup the changes

                foreach (Parameter param in this._parameters)
                {
                    if (param.PropertyChanges.ObjectStatus == InheritedObjectStatus.Delete)
                    {
                        _inheritedParent.Parameters.Remove(param.UniqueIdentifier);
                    }
                    else
                    {
                        Parameter _inheritedChild = _inheritedParent.Parameters.GetItem(param.UniqueIdentifier);

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

            System.Windows.Forms.TreeNode parameters = new System.Windows.Forms.TreeNode("Parameters");
            parameters.ImageKey = "Parameters";
            parameters.SelectedImageKey = parameters.ImageKey;
            parameters.Tag = this.Parameters;
            treeNode.Nodes.Add(parameters);

            foreach (Parameter param in this._parameters)
            {
                if (param != null)
                {
                    parameters.Nodes.Add(param.GetTreeNode());
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
                case "Add Parameter":
                    this.Parameters.Add(new Parameter());
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
        /// <returns>XML representation of Search Configuration </returns>
        public override String ToXml()
        {
            String searchConfigurationXml = String.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            #region SearchConfiguration Node

            //SearchConfiguration node start
            xmlWriter.WriteStartElement("SearchConfiguration");

            #region Parameters Node

            xmlWriter.WriteStartElement("Parameters");

            if (this.Parameters != null)
            {
                foreach (Parameter param in this.Parameters)
                {
                    xmlWriter.WriteRaw(param.ToXml());
                }
            }
            xmlWriter.WriteEndElement();

            #endregion Parameters Node

            //SearchConfiguration node end
            xmlWriter.WriteEndElement();

            #endregion SearchConfiguration Node

            xmlWriter.Flush();

            //Get the actual XML
            searchConfigurationXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return searchConfigurationXml;
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

            if (this.Parameters.Count == 0)
            {
                validationErrors.Add("The SearchConfiguration does not contain any Parameter.", ValidationErrorType.Warning, "Search Configuration", this);
            }
        }

        #endregion    
    }
}
