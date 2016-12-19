using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace RS.MDM.ConfigurationObjects
{
    using Configuration;
    using Validations;

	/// <summary>
    /// Provides EntityIdentificationConfig
    /// </summary>
    [XmlRoot("EntityIdentificationConfig")]
    [Serializable()]
    public sealed class EntityIdentificationConfig : Object
    {
        #region Fields

        private Collections.Generic.List<EntityIdentificationStep> _entityIdentificationSteps = new Collections.Generic.List<EntityIdentificationStep>();

        #endregion

        #region Properties

        /// <summary>
        /// Represents the EntityIdentificationSteps
        /// </summary>
        [Category("EntityIdentificationSteps")]
        [Description("Represents EntityIdentificationSteps")]
        [TrackChanges()]
        public Collections.Generic.List<EntityIdentificationStep> EntityIdentificationSteps
        {
            get
            {
                this.SetParent();
                return this._entityIdentificationSteps;
            }
            set
            {
                this._entityIdentificationSteps = value;
                this.SetParent();
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Parameterized constructor with values as xml representation of object.
        /// </summary>
        public EntityIdentificationConfig(String valuesAsXml)
        {

        }

        public EntityIdentificationConfig()
        {
            this.AddVerb("Add EntityIdentificationConfig");
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Get XML representation of the EntityIdentificationConfig object
        /// </summary>
        /// <returns>XML representation of EntityIdentificationConfig</returns>
        public override String ToXml()
        {
            String parameterXml;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //EntityHistoryExcludeListConfig node start
            xmlWriter.WriteStartElement("EntityIdentificationConfig");

            #region EntityIdentificationSteps Node

            xmlWriter.WriteStartElement("EntityIdentificationSteps");

            foreach (EntityIdentificationStep entityUniqueElement in this.EntityIdentificationSteps)
            {
                xmlWriter.WriteRaw(entityUniqueElement.ToXml());
            }

            xmlWriter.WriteEndElement();

            #endregion EntityIdentificationSteps Node

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
        /// Load the EntityIdentificationStep object from the input xml
        /// </summary>
        /// <param name="valueAsXml">EntityIdentificationStep object representation as xml</param>
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
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "EntityIdentificationStep")
                        {
                            //Read EntityIdentificationStep
                            #region Read EntityIdentificationStep

                            String entityIdentificationStepXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(entityIdentificationStepXml))
                            {
                                EntityIdentificationStep entityIdentifierNode = new EntityIdentificationStep(entityIdentificationStepXml);
	                            EntityIdentificationSteps.Add(entityIdentifierNode);
                            }

                            #endregion Read EntityIdentificationStep
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

            foreach (EntityIdentificationStep entityIdentificationStepElement in this._entityIdentificationSteps)
            {
                if (entityIdentificationStepElement != null)
                {
                    entityIdentificationStepElement.GenerateNewUniqueIdentifier();
                }
            }
        }

        /// <summary>
        /// Finds and returns a list of child objects for a given unique identifier
        /// </summary>
        /// <param name="uniqueIdentifier">Indicates an unique identifier of an object that needs to be found</param>
        /// <param name="includeDeletedItems">Indicates if deleted items to be included in the search</param>
        /// <returns>A list of Child objects matching the given unique identifier</returns>
        public override List<Object> FindChildren(String uniqueIdentifier, Boolean includeDeletedItems)
        {
            List<Object> list = new List<Object>();
            list.AddRange(base.FindChildren(uniqueIdentifier, includeDeletedItems));

            foreach (EntityIdentificationStep entityIdentificationStepElement in this._entityIdentificationSteps)
            {
                if (entityIdentificationStepElement != null)
                {
                    list.AddRange(entityIdentificationStepElement.FindChildren(uniqueIdentifier, includeDeletedItems));
                }
            }

            return list;
        }

        /// <summary>
        /// Sets the parent (container) for all the aggregated children
        /// </summary>
        public override void SetParent()
        {
            if (this._entityIdentificationSteps != null)
            {
                foreach (EntityIdentificationStep entityIdentificationStepElement in this._entityIdentificationSteps)
                {
                    if (entityIdentificationStepElement != null)
                    {
                        entityIdentificationStepElement.Parent = this;
                        entityIdentificationStepElement.InheritedParent = this.InheritedParent;
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

            if (this._entityIdentificationSteps != null && this._entityIdentificationSteps.Count > 0)
            {
                for (Int32 i = _entityIdentificationSteps.Count - 1; i > -1; i--)
                {
                    EntityIdentificationStep entityIdentificationStepElement = _entityIdentificationSteps[i];

                    if (entityIdentificationStepElement != null)
                    {
                        if (entityIdentificationStepElement.PropertyChanges.ObjectStatus == InheritedObjectStatus.Delete)
                        {
                            this._entityIdentificationSteps.Remove(entityIdentificationStepElement);
                        }
                        else
                        {
                            entityIdentificationStepElement.AcceptChanges();
                        }
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
            System.Windows.Forms.TreeNode entityIdentifierElements = new System.Windows.Forms.TreeNode("EntityIdentificationStep");
            entityIdentifierElements.ImageKey = @"EntityIdentificationConfig";
            entityIdentifierElements.SelectedImageKey = entityIdentifierElements.ImageKey;
            entityIdentifierElements.Tag = this.EntityIdentificationSteps;
            treeNode.Nodes.Add(entityIdentifierElements);

            foreach (EntityIdentificationStep entityIdentifier in this._entityIdentificationSteps)
            {
                if (entityIdentifier != null)
                {
                    entityIdentifierElements.Nodes.Add(entityIdentifier.GetTreeNode());
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
        public override void OnDesignerVerbClick(String text, Object configObject, object treeView)
        {
            ConfigurationObject configurationObject;

            base.OnDesignerVerbClick(text, configObject, treeView);

            switch (text)
            {
                case "Add EntityIdentificationStep":
                    this.EntityIdentificationSteps.Add(new EntityIdentificationStep());
                    break;
            }

            if (text != "Find Changes" && text != "Accept Changes" && configObject is ConfigurationObject)
            {
                configurationObject = configObject as ConfigurationObject;
                configurationObject._isConfigDirty = true;
            }

            TypeDescriptor.Refresh(this);
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
                validationErrors = new ValidationErrorCollection();
            }

            if (this.EntityIdentificationSteps.Count == 0)
            {
                validationErrors.Add("The Entity unique identification config does not contain any Element.", ValidationErrorType.Warning, "EntityIdentificationConfig", this);
            }

        }

        #endregion
    }
}
