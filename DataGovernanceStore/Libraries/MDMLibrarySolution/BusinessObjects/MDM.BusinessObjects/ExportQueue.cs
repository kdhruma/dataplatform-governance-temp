using System;
using System.IO;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Provides the properties and methods relating to export queue
    /// </summary>
    public class ExportQueue : MDMObject, IExportQueue
    {
        #region Fields

        private Int64 _id = -1;
        private Int64 _entityFamilyId = -1;
        private Int64 _entityGlobalFamilyId = -1;
        private Int64 _entityId = -1;
        private Int16 _hierarchyLevel = -1;
        private Int32 _containerId = -1;
        private Int32 _exportProfileId = -1;
        private Int32 _entityTypeId = -1;
        private Boolean _isExported = false;
        private Boolean _isExportInProgress = false;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Parameterless constructor of ExportQueue class.
        /// </summary>
        public ExportQueue()
        {

        }

        /// <summary>
        /// Constructor accepting the xml representation of the export queue object as parameter
        /// </summary>
        /// <param name="valuesAsXml">The values as XML.</param>
        public ExportQueue(String valuesAsXml)
        {

        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Indicates unique id of export queue
        /// </summary>
        public new Int64 Id
        {
            get
            {
                return this._id;
            }
            set
            {
                this._id = value;
            }
        }

        /// <summary>
        /// Property denoting the entity family identifier.
        /// </summary>
        public Int64 EntityFamilyId
        {
            get
            {
                return _entityFamilyId;
            }
            set
            {
                _entityFamilyId = value;
            }
        }

        /// <summary>
        /// Property denoting the entity global family identifier.
        /// </summary>
        public Int64 EntityGlobalFamilyId
        {
            get
            {
                return _entityGlobalFamilyId;
            }
            set
            {
                _entityGlobalFamilyId = value;
            }
        }

        /// <summary>
        /// Property denoting the entity identifier.
        /// </summary>
        public Int64 EntityId
        {
            get
            {
                return _entityId;
            }
            set
            {
                _entityId = value;
            }
        }

        /// <summary>
        /// Property denoting the hierarchy level
        /// </summary>
        public Int16 HierarchyLevel
        {
            get 
            {
                return _hierarchyLevel;
            }
            set 
            {
                _hierarchyLevel = value;
            }
        }

        /// <summary>
        /// Property denoting the container identifier
        /// </summary>
        public Int32 ContainerId
        {
            get
            {
                return _containerId;
            }
            set
            {
                _containerId = value;
            }
        }

        /// <summary>
        /// Property denoting the export profile identifier.
        /// </summary>
        public Int32 ExportProfileId
        {
            get
            {
                return _exportProfileId;
            }
            set
            {
                _exportProfileId = value;
            }
        }

        /// <summary>
        /// Property denoting the entity type identifier
        /// </summary>
        public Int32 EntityTypeId
        {
            get
            {
                return _entityTypeId;
            }
            set
            {
                _entityTypeId = value;
            }
        }

        /// <summary>
        /// Property denoting a value indicating whether this instance is exported.
        /// </summary>
        public Boolean IsExported
        {
            get
            {
                return _isExported;
            }
            set
            {
                _isExported = value;
            }
        }

        /// <summary>
        /// Property denoting a value indicating whether export of the this instance is in progress.
        /// </summary>
        public Boolean IsExportInProgress
        {
            get
            {
                return _isExportInProgress;
            }
            set
            {
                _isExportInProgress = value;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Represents ExportQueue in Xml format
        /// </summary>
        /// <returns>
        /// String representation of current ExportQueue
        /// </returns>
        public override String ToXml()
        {
            String returnXml = String.Empty;

            using (StringWriter sw = new StringWriter())
            {
                using (XmlTextWriter xmlWriter = new XmlTextWriter(sw))
                {
                    //ExportQueue node start
                    xmlWriter.WriteStartElement("ExportQueue");

                    #region write EntityFamilyQueue

                    xmlWriter.WriteAttributeString("Id", this.Id.ToString());
                    xmlWriter.WriteAttributeString("EntityFamilyId", this.EntityFamilyId.ToString());
                    xmlWriter.WriteAttributeString("EntityGlobalFamilyId", this.EntityGlobalFamilyId.ToString());
                    xmlWriter.WriteAttributeString("EntityId", this.EntityId.ToString());
                    xmlWriter.WriteAttributeString("HierarchyLevel", this.HierarchyLevel.ToString());
                    xmlWriter.WriteAttributeString("ContainerId", this.ContainerId.ToString());
                    xmlWriter.WriteAttributeString("ExportProfileId", this.ExportProfileId.ToString());
                    xmlWriter.WriteAttributeString("EntityTypeId", this._entityTypeId.ToString());
                    xmlWriter.WriteAttributeString("IsExported", this.IsExported.ToString());
                    xmlWriter.WriteAttributeString("IsExportInProgress", this.IsExportInProgress.ToString());

                    #endregion

                    //ExportQueue node end
                    xmlWriter.WriteEndElement();
                    xmlWriter.Flush();

                    //get the actual XML
                    returnXml = sw.ToString();
                }
            }

            return returnXml;
        }

        /// <summary>
        /// Loads the export queue from Xml.
        /// </summary>
        /// <param name="valuesAsXml">The values as XML.</param>
        private void LoadExportQueue(String valuesAsXml)
        {
            XmlTextReader reader = null;
            try
            {
                reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                while (!reader.EOF)
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "ExportQueue")
                    {
                        #region Read ExportQueue

                        if (reader.HasAttributes)
                        {
                            if (reader.MoveToAttribute("Id"))
                            {
                                this._id = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), this._id);
                            }
                            if (reader.MoveToAttribute("EntityFamilyId"))
                            {
                                this._entityFamilyId = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), this._entityFamilyId);
                            }
                            if (reader.MoveToAttribute("EntityGlobalFamilyId"))
                            {
                                this._entityGlobalFamilyId = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), this._entityGlobalFamilyId);
                            }
                            if (reader.MoveToAttribute("EntityId"))
                            {
                                this._entityId = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), this._entityId);
                            }
                            if (reader.MoveToAttribute("HierarchyLevel"))
                            {
                                this._hierarchyLevel = ValueTypeHelper.Int16TryParse(reader.ReadContentAsString(), this._hierarchyLevel);
                            }
                            if (reader.MoveToAttribute("ContainerId"))
                            {
                                this._containerId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), this._containerId);
                            }
                            if (reader.MoveToAttribute("ExportProfileId"))
                            {
                                this._exportProfileId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), this._exportProfileId);
                            }
                            if(reader.MoveToAttribute("EntityTypeId"))
                            {
                                this._entityTypeId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), this._entityTypeId);
                            }
                            if (reader.MoveToAttribute("IsExported"))
                            {
                                this._isExported = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), this._isExported);
                            }
                            if (reader.MoveToAttribute("IsExportInProgress"))
                            {
                                this._isExportInProgress = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), this._isExportInProgress);
                            }
                            if (reader.MoveToAttribute("Action"))
                            {
                                this.Action = ValueTypeHelper.GetAction(reader.ReadContentAsString());
                            }
                        }

                        #endregion
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

        #endregion Methods
    }
}