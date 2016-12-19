using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Business Object Class for ContainerTempleteCopyContext
    /// </summary>
    public class ContainerTemplateCopyContext : MDMObject, IContainerTemplateCopyContext
    {
        #region Fields

        /// <summary>
        /// Indicates if container entitytype mappings are to be copied
        /// </summary>
        private Boolean _copyContainerEntityTypeMappings = false;

        /// <summary>
        /// Indicates if Container Entitytype Mappings Attribute are to be copied
        /// </summary>
        private Boolean _copyContainerEntityTypeAttributeMappings = false;

        /// <summary>
        /// Indicates if Container RelationshipType Mappings are to be copied
        /// </summary>
        private Boolean _copyContainerRelationshipTypeMappings = false;

        /// <summary>
        /// Indicates if Container RelationshipType Attribute Mappings are to be copied
        /// </summary>
        private Boolean _copyContainerRelationshipTypeAttributeMappings = false;

        /// <summary>
        /// Indicates if Container Inheritance Rule are to be copied
        /// </summary>
        private Boolean _flushAndFillTargetContainer = false;

        /// <summary>
        /// Indicates if Container BranchLevel One is to be copied
        /// </summary>
        private Boolean _copyContainerBranchLevelOne = false;

        #endregion

        #region Constructors

        /// <summary>
        /// Parameter Less Constructor
        /// </summary>
        public ContainerTemplateCopyContext()
            : base()
        {
        }

        /// <summary>
        ///  Constructor with All ContainerTempleteCopyContext properties as parameter.
        /// </summary>
        /// <param name="copyContainerEntityTypeMappings">Indicates if container entitytype mappings are to be copied</param>
        /// <param name="copyContainerEntityTypeAttributeMappings">Indicates if Container Entitytype Mappings Attribute are to be copied</param>
        /// <param name="copyContainerRelationshipTypeMappings">Indicates if Container RelationshipType Mappings are to be copied</param>
        /// <param name="copyContainerRelationshipTypeAttributeMappings">Indicates if Container RelationshipType Attribute Mappings are to be copied</param>
        /// <param name="flushAndFillTargetContainer">Indicates if Container InheritanceRule  are to be copied</param>
        /// <param name="copyContainerBranchLevelOne">Indicates if Container BranchLevelOne are to be copied</param>        
        public ContainerTemplateCopyContext(Boolean copyContainerEntityTypeMappings, 
            Boolean copyContainerEntityTypeAttributeMappings, 
            Boolean copyContainerRelationshipTypeMappings, 
            Boolean copyContainerRelationshipTypeAttributeMappings,
            Boolean flushAndFillTargetContainer,
            Boolean copyContainerBranchLevelOne)
            : base()
        {
            this._copyContainerEntityTypeMappings = copyContainerEntityTypeMappings;
            this._copyContainerEntityTypeAttributeMappings = copyContainerEntityTypeAttributeMappings;
            this._copyContainerRelationshipTypeMappings = copyContainerRelationshipTypeMappings;
            this._copyContainerRelationshipTypeAttributeMappings = copyContainerRelationshipTypeAttributeMappings;
            this._flushAndFillTargetContainer = flushAndFillTargetContainer;
            this._copyContainerBranchLevelOne = copyContainerBranchLevelOne;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Indicates if container entitytype mappings are to be copied
        /// </summary>
        [DataMember]
        public Boolean CopyContainerEntityTypeMappings
        {
            get { return _copyContainerEntityTypeMappings; }
            set { _copyContainerEntityTypeMappings = value; }
        }

        /// <summary>
        /// Indicates if Container Entitytype Mappings Attribute are to be copied
        /// </summary>
        [DataMember]
        public Boolean CopyContainerEntityTypeAttributeMappings
        {
            get { return _copyContainerEntityTypeAttributeMappings; }
            set { _copyContainerEntityTypeAttributeMappings = value; }
        }

        /// <summary>
        /// Indicates if Container RelationshipType Mappings are to be copied
        /// </summary>
        [DataMember]
        public Boolean CopyContainerRelationshipTypeMappings
        {
            get { return _copyContainerRelationshipTypeMappings; }
            set { _copyContainerRelationshipTypeMappings = value; }
        }

        /// <summary>
        /// Indicates if Container RelationshipType Attribute Mappings are to be copied
        /// </summary>
        [DataMember]
        public Boolean CopyContainerRelationshipTypeAttributeMappings
        {
            get { return _copyContainerRelationshipTypeAttributeMappings; }
            set { _copyContainerRelationshipTypeAttributeMappings = value; }
        }

        /// <summary>
        /// Indicates if copying is flush and fill or the incremental
        /// </summary>
        [DataMember]
        public Boolean FlushAndFillTargetContainer
        {
            get { return _flushAndFillTargetContainer; }
            set { _flushAndFillTargetContainer = value; }
        }

        /// <summary>
        /// Indicates if Container BranchLevel One is to be copied
        /// </summary>
        [DataMember]
        public Boolean CopyContainerBranchLevelOne
        {
            get { return _copyContainerBranchLevelOne; }
            set { _copyContainerBranchLevelOne = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override Boolean Equals(Object obj)
        {
            if (base.Equals(obj))
            {
                if (obj != null && obj is ContainerTemplateCopyContext)
                {
                    ContainerTemplateCopyContext objectToBeCompared = obj as ContainerTemplateCopyContext;

                    if (this.CopyContainerEntityTypeMappings != objectToBeCompared.CopyContainerEntityTypeMappings)
                        return false;

                    if (this.CopyContainerEntityTypeAttributeMappings != objectToBeCompared.CopyContainerEntityTypeAttributeMappings)
                        return false;

                    if (this.CopyContainerRelationshipTypeMappings != objectToBeCompared.CopyContainerRelationshipTypeMappings)
                        return false;

                    if (this.CopyContainerRelationshipTypeAttributeMappings != objectToBeCompared.CopyContainerRelationshipTypeAttributeMappings)
                        return false;

                    if (this.FlushAndFillTargetContainer != objectToBeCompared.FlushAndFillTargetContainer)
                        return false;

                    if (this.CopyContainerBranchLevelOne != objectToBeCompared.CopyContainerBranchLevelOne)
                        return false;

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Serves as a hash function for type
        /// </summary>
        /// <returns>A hash code for the current Object.</returns>
        public override Int32 GetHashCode()
        {
            Int32 hashCode = base.GetHashCode() ^ this.CopyContainerEntityTypeMappings.GetHashCode() ^ this.CopyContainerEntityTypeAttributeMappings.GetHashCode() ^ this.CopyContainerRelationshipTypeMappings.GetHashCode() ^ this.CopyContainerRelationshipTypeAttributeMappings.GetHashCode() ^ this.FlushAndFillTargetContainer.GetHashCode() ^ this.CopyContainerBranchLevelOne.GetHashCode();

            return hashCode;
        }

        /// <summary>
        /// Populate current object from incoming XML
        /// </summary>
        /// <example>
        /// <![CDATA[
        /// <ContainerTempleteCopyContext 
        ///     CopyContainerEntityTypeMappings="true" 
        ///     CopyContainerEntityTypeAttributeMappings="true" 
        ///     CopyContainerRelationshipTypeMappings="true" 
        ///     CopyContainerRelationshipTypeAttributeMappings="true">
        /// </ContainerTempleteCopyContext>
        /// ]]>
        /// </example>
        /// <param name="valuesAsXml">XML representation for ContainerTempleteCopyContext from which object is to be created</param>
        public void LoadContainerTempleteCopyContext(String valuesAsXml)
        {
            #region Sample Xml
            /* <ContainerTempleteCopyContext 
                     CopyContainerEntityTypeMappings="true" 
                     CopyContainerEntityTypeAttributeMappings="true" 
                     CopyContainerRelationshipTypeMappings="true" 
                     CopyContainerRelationshipTypeAttributeMappings="true"
                     CopyContainerInheritanceRule = "true"
                     CopyContainerBranchLevelOne = "true">
               </ContainerTempleteCopyContext>
                 */
            #endregion

            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "ContainerTempleteCopyContext")
                        {
                            #region Read ContainerTempleteCopyContext Properties

                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("CopyContainerEntityTypeMappings"))
                                {
                                    this.CopyContainerEntityTypeMappings = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), false);
                                }

                                if (reader.MoveToAttribute("CopyContainerEntityTypeAttributeMappings"))
                                {
                                    this.CopyContainerEntityTypeAttributeMappings = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), false);
                                }

                                if (reader.MoveToAttribute("CopyContainerRelationshipTypeMappings"))
                                {
                                    this.CopyContainerRelationshipTypeMappings =  ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), false);
                                }
                                if (reader.MoveToAttribute("CopyContainerRelationshipTypeAttributeMappings"))
                                {
                                    this.CopyContainerRelationshipTypeAttributeMappings =  ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), false);
                                }
                                if (reader.MoveToAttribute("CopyContainerInheritanceRule"))
                                {
                                    this.FlushAndFillTargetContainer =  ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), false);
                                }
                                if (reader.MoveToAttribute("CopyContainerBranchLevelOne"))
                                {
                                    this.CopyContainerBranchLevelOne = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), false);
                                }
                                if (reader.MoveToAttribute("CopyContainerInheritanceRule"))
                                {
                                    this.CopyContainerBranchLevelOne = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), false);
                                }
                                if (reader.MoveToAttribute("CopyContainerBranchLevelOne"))
                                {
                                    this.CopyContainerBranchLevelOne = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), false);
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
        }

        #endregion

        #region IContainerTempleteCopyContext Methods

        /// <summary>
        /// Get Xml representation of ContainerTempleteCopyContext object
        /// </summary>
        /// <returns>XML String of ContainerTempleteCopyContext Object</returns>
        public override String ToXml()
        {
            String containerTempleteCopyContextXml = String.Empty;
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //ContainerTempleteCopyContext node start
            xmlWriter.WriteStartElement("ContainerTempleteCopyContext");

            xmlWriter.WriteAttributeString("CopyContainerEntityTypeMappings", this.CopyContainerEntityTypeMappings.ToString());
            xmlWriter.WriteAttributeString("CopyContainerEntityTypeAttributeMappings", this.CopyContainerEntityTypeAttributeMappings.ToString());
            xmlWriter.WriteAttributeString("CopyContainerRelationshipTypeMappings", this.CopyContainerRelationshipTypeMappings.ToString());
            xmlWriter.WriteAttributeString("CopyContainerRelationshipTypeAttributeMappings", this.CopyContainerRelationshipTypeAttributeMappings.ToString());
            xmlWriter.WriteAttributeString("CopyContainerInheritanceRule", this.FlushAndFillTargetContainer.ToString());
            xmlWriter.WriteAttributeString("CopyContainerBranchLevelOne", this.CopyContainerBranchLevelOne.ToString());

            //ContainerTempleteCopyContext node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //get the actual XML
            containerTempleteCopyContextXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return containerTempleteCopyContextXml;
        }

        /// <summary>
        /// Get Xml representation of ContainerTempleteCopyContextXml object
        /// </summary>
        /// <param name="objectSerialization">serialization option. Based on the value selected, the different xml representation will be there</param>
        /// <returns>Xml representation of object</returns>
        public override String ToXml(ObjectSerialization objectSerialization)
        {
            return this.ToXml();
           
        }
        #endregion

    }
}
