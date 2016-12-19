using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using MDM.Core;

namespace RS.MDM.ConfigurationObjects
{
    using Configuration;
    using Validations;

    /// <summary>
    /// Provides EntityIdentificationStep config
    /// </summary>
    [XmlRoot("EntityIdentificationStep")]
    [Serializable()]
    public sealed class EntityIdentificationStep : Object
    {
        #region Fields

        private Collections.Generic.List<IdentificationField> _identificationFields = new Collections.Generic.List<IdentificationField>();
        private EntityIdentificationBehavior _behaviorOnNoMatch = EntityIdentificationBehavior.Unknown;
        private EntityIdentificationBehavior _behaviorOnDuplicateMatches = EntityIdentificationBehavior.Unknown;
        private EntityIdentificationServiceType _identificationService = EntityIdentificationServiceType.Unknown;
        private String _seq = "0";

        #endregion

        #region Properties

        /// <summary>
        /// Represents IdentificationFields
        /// </summary>
        [Category("IdentificationFields")]
        [Description("Represents IdentificationFields")]
        [TrackChanges()]
        public Collections.Generic.List<IdentificationField> IdentificationFields
        {
            get
            {
                this.SetParent();
                return this._identificationFields;
            }
            set
            {
                this._identificationFields = value;
                this.SetParent();
            }
        }

        /// <summary>
        /// Represents the behavior on no match
        /// </summary>
        [Category("Properties")]
        [XmlAttribute("BehaviorOnNoMatch")]
        [Description("Determines action to perform on no match")]
        [TrackChanges()]
        public EntityIdentificationBehavior BehaviorOnNoMatch
        {
            get
            {
                return this._behaviorOnNoMatch;
            }
            set
            {
                this._behaviorOnNoMatch = value;
            }
        }

        /// <summary>
        /// Represents the behavior on duplicate match
        /// </summary>
        [Category("Properties")]
        [XmlAttribute("BehaviorOnDuplicateMatches")]
        [Description("Determines action to perform on duplicate matches")]
        [TrackChanges()]
        public EntityIdentificationBehavior BehaviorOnDuplicateMatches
        {
            get
            {
                return this._behaviorOnDuplicateMatches;
            }
            set
            {
                this._behaviorOnDuplicateMatches = value;
            }
        }

        /// <summary>
        /// Indicates Sequence of the Entity Identifier
        /// </summary>
        [Category("Properties")]
        [XmlAttribute("Seq")]
        [Description("Indicates Sequence")]
        [TrackChanges()]
        public String Seq
        {
            get
            {
                return this._seq;
            }
            set
            {
                this._seq = value;
            }
        }

        /// <summary>
        /// Represents the identification service
        /// </summary>
        [Category("Properties")]
        [XmlAttribute("IdentificationService")]
        [Description("Determines identification service")]
        [TrackChanges()]
        public EntityIdentificationServiceType IdentificationService
        {
            get
            {
                return this._identificationService;
            }
            set
            {
                this._identificationService = value;
            }
        }
        #endregion

        #region Constructor

        /// <summary>
        /// Parameterized constructor with values as xml representation of object.
        /// </summary>
        public EntityIdentificationStep(String valuesAsXml)
        {

        }


        public EntityIdentificationStep()
        {
            AddVerb("Add EntityIdentificationStep");
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Get XML representation of the EntityIdentifier object
        /// </summary>
        /// <returns>XML representation of EntityIdentifier</returns>
        public override String ToXml()
        {
            String parameterXml;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //EntityIdentifier node start
            xmlWriter.WriteStartElement("EntityIdentificationStep");

            xmlWriter.WriteAttributeString("IdentificationService", this.IdentificationService.ToString());
            xmlWriter.WriteAttributeString("Seq", this.Seq);
            xmlWriter.WriteAttributeString("BehaviorOnNoMatch", this.BehaviorOnNoMatch.ToString());
            xmlWriter.WriteAttributeString("BehaviorOnDuplicateMatches", this.BehaviorOnDuplicateMatches.ToString());

            #region Identifier Node

            //xmlWriter.WriteStartElement("Identifier");

            foreach (IdentificationField identifierElement in this.IdentificationFields)
            {
                xmlWriter.WriteRaw(identifierElement.ToXml());
            }

            //xmlWriter.WriteEndElement();

            #endregion Identifier Node

            //EntityIdentifier node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //Get the actual XML
            parameterXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return parameterXml;
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

            foreach (IdentificationField identifierElement in this._identificationFields)
            {
                if (identifierElement != null)
                {
                    identifierElement.GenerateNewUniqueIdentifier();
                }
            }
        }

        /// <summary>
        /// Finds and returns a list of child objects for a given unique identifier
        /// </summary>
        /// <param name="uniqueIdentifier">Indicates an unique identifier of an object that needs to be found</param>
        /// <param name="includeDeletedItems">Indicates if deleted items to be included in the search</param>
        /// <returns>A list of Child objects matching the given unique identifier</returns>
        public override List<Object> FindChildren(String uniqueIdentifier, bool includeDeletedItems)
        {
            List<Object> list = new List<Object>();
            list.AddRange(base.FindChildren(uniqueIdentifier, includeDeletedItems));

            foreach (IdentificationField identifierElement in this._identificationFields)
            {
                if (identifierElement != null)
                {
                    list.AddRange(identifierElement.FindChildren(uniqueIdentifier, includeDeletedItems));
                }
            }

            return list;
        }

        /// <summary>
        /// Sets the parent (container) for all the aggregated children
        /// </summary>
        public override void SetParent()
        {
            if (this._identificationFields != null)
            {
                foreach (IdentificationField identifierElement in this._identificationFields)
                {
                    if (identifierElement != null)
                    {
                        identifierElement.Parent = this;
                        identifierElement.InheritedParent = this.InheritedParent;
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

            if (this._identificationFields != null && this._identificationFields.Count > 0)
            {
                for (int i = _identificationFields.Count - 1; i > -1; i--)
                {
                    IdentificationField identifierElement = _identificationFields[i];

                    if (identifierElement != null)
                    {
                        if (identifierElement.PropertyChanges.ObjectStatus == InheritedObjectStatus.Delete)
                        {
                            this._identificationFields.Remove(identifierElement);
                        }
                        else
                        {
                            identifierElement.AcceptChanges();
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

            if (this._identificationFields != null)
            {
                foreach (IdentificationField identifierElement in this._identificationFields)
                {
                    if (identifierElement != null)
                    {
                        identifierElement.FindChanges();
                    }
                }
            }

            if (this.Parent == null && this.InheritedParent != null)
            {
                this.InheritedParent.FindDeletes(this);
            }
        }
               
        /// <summary>
        /// Get a tree node that represents an object and its aggregated children
        /// </summary>
        /// <returns>A Tree Node that represents an object and its aggregated children</returns>
        public override System.Windows.Forms.TreeNode GetTreeNode()
        {
            System.Windows.Forms.TreeNode treeNode = base.GetTreeNode();

            System.Windows.Forms.TreeNode identificationFieldElements = new System.Windows.Forms.TreeNode("IdentificationField");
            identificationFieldElements.ImageKey = @"IdentificationField";
            identificationFieldElements.SelectedImageKey = identificationFieldElements.ImageKey;
            identificationFieldElements.Tag = this.IdentificationFields;

			treeNode.Nodes.Add(identificationFieldElements);

            foreach (IdentificationField identificationFieldElement in this._identificationFields)
            {
                if (identificationFieldElement != null)
                {
                    identificationFieldElements.Nodes.Add(identificationFieldElement.GetTreeNode());
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
                case "Add IdentificationField":
                    this.IdentificationFields.Add(new IdentificationField());
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

            if (this.IdentificationFields.Count == 0)
            {
                validationErrors.Add("The EntityIdentificationStep does not contain any sub elements.", ValidationErrorType.Warning, "EntityIdentificationStep", this);
            }
        }

        #endregion
    }
}
