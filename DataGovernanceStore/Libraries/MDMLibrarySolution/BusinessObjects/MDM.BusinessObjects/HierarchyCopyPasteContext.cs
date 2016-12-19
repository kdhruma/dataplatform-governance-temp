using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using ProtoBuf;

namespace MDM.BusinessObjects
{
    using MDM.Interfaces;
    using MDM.Core;

    /// <summary>
    /// Specified the Hierarchy Copy\Paste Context
    /// </summary>
    [DataContract]
    [ProtoContract]
    public class HierarchyCopyPasteContext : ObjectBase, IHierarchyCopyPasteContext
    {
        #region Fields

        /// <summary>
        /// Field denoting source hierarchy Id. From which we want to copy categories
        /// </summary>
        private Int32 _sourceHierarchyId;

        /// <summary>
        /// Field denoting target hierarchy Id. To which we want to paste categories
        /// </summary>
        private Int32 _targetHierarchyId;
        
        #endregion Fields

        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public HierarchyCopyPasteContext() : base()
        {
        }

        /// <summary>
        /// Initialize HierarchyCopyPasteContext
        /// </summary>
        /// <param name="sourceHierarchyId">Id of source hierarchy</param>
        /// <param name="targetHierarchyId">Id of target hierarchy</param>
        public HierarchyCopyPasteContext(Int32 sourceHierarchyId, Int32 targetHierarchyId)
        {
            this._sourceHierarchyId = sourceHierarchyId;
            this._targetHierarchyId = targetHierarchyId;
        }

        /// <summary>
        /// Initialize HierarchyCopyPasteContext from Xml
        /// </summary>
        /// <param name="valuesAsXml">
        /// Xml having values
        /// <para> Sample:</para>
        /// <![CDATA[<HierarchyCopyPasteContext SourceHierarchyId="62" TargetHierarchyId="78" Locale="en_WW"/>]]>
        /// </param>
        public HierarchyCopyPasteContext(String valuesAsXml)
        {
            LoadHierarchyCopyPasteContext(valuesAsXml);
        }
        
        #endregion Constructors
        
        #region Properties

        /// <summary>
        /// Property denoting source hierarchy Id. From which we want to copy categories
        /// </summary>
        [ProtoMember(1)]
        [DataMember]
        public Int32 SourceHierarchyId
        {
            get { return _sourceHierarchyId; }
            set { _sourceHierarchyId = value; }
        }

        /// <summary>
        /// Property denoting target hierarchy Id. To which we want to paste categories
        /// </summary>
        [ProtoMember(2)]
        [DataMember]
        public Int32 TargetHierarchyId
        {
            get { return _targetHierarchyId; }
            set { _targetHierarchyId = value; }
        }

        #endregion Properties

        #region Methods

        #region Public Methods

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>[true] if the specified Object is equal to the current Object; otherwise, [false].</returns>
        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
            {
                if (obj is HierarchyCopyPasteContext)
                {
                    HierarchyCopyPasteContext objectToBeCompared = obj as HierarchyCopyPasteContext;
                    
                    if (!this.SourceHierarchyId.Equals(objectToBeCompared.SourceHierarchyId))
                        return false;

                    if (!this.TargetHierarchyId.Equals(objectToBeCompared.TargetHierarchyId))
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
            Int32 hashCode = 0;
            hashCode = base.GetHashCode() ^ this.TargetHierarchyId.GetHashCode() ^ this.SourceHierarchyId.GetHashCode();
            
            return hashCode;
        }

        /// <summary>
        /// Represents HierarchyCopyPasteContext in Xml format
        /// </summary>
        /// <returns>String representation of current HierarchyCopyPasteContext object</returns>
        public String ToXml()
        {
            String xml = String.Empty;
            
            using (StringWriter sw = new StringWriter())
            {
                using (XmlTextWriter xmlWriter = new XmlTextWriter(sw))
                {
                    //RelationshipContext node start
                    xmlWriter.WriteStartElement("HierarchyCopyPasteContext");

                    xmlWriter.WriteAttributeString("SourceHierarchyId", this.SourceHierarchyId.ToString());
                    xmlWriter.WriteAttributeString("TargetHierarchyId", this.TargetHierarchyId.ToString());

                    //RelationshipContext end node
                    xmlWriter.WriteEndElement();
                    xmlWriter.Flush();
                    //get the actual XML
                    xml = sw.ToString();

                    xmlWriter.Close();
                }
            }

            return xml;
        }

        /// <summary>
        /// Create clone of HierarchyCopyPasteContext
        /// </summary>
        /// <returns>Clone of HierarchyCopyPasteContext</returns>
        public IHierarchyCopyPasteContext Clone()
        {
            HierarchyCopyPasteContext clonedHierarchyCopyPasteContext = new HierarchyCopyPasteContext();
            
            clonedHierarchyCopyPasteContext._sourceHierarchyId = this._sourceHierarchyId;
            clonedHierarchyCopyPasteContext._targetHierarchyId = this._targetHierarchyId;

            return clonedHierarchyCopyPasteContext;
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Initialize HierarchyCopyPasteContext from Xml
        /// </summary>
        /// <param name="valuesAsXml">
        /// Xml having values
        /// <para> Sample:</para>
        /// <![CDATA[<HierarchyCopyPasteContext SourceHierarchyId="62" TargetHierarchyId="78" Locale="en_WW"/>]]>
        /// </param>
        private void LoadHierarchyCopyPasteContext(String valuesAsXml)
        {
            #region Sample Xml

            /*
             * <HierarchyCopyPasteContext SourceHierarchyId="62" TargetHierarchyId="78" Locale="en_WW"/>
             */

            #endregion Sample Xml

            XmlTextReader reader = null;

            using (reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null))
            {
                while (!reader.EOF)
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "HierarchyCopyPasteContext")
                    {
                        #region Read EntityContext Properties

                        if (reader.HasAttributes)
                        {
                            if (reader.MoveToAttribute("SourceHierarchyId"))
                            {
                                this.SourceHierarchyId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(),
                                    this._sourceHierarchyId);
                            }

                            if (reader.MoveToAttribute("TargetHierarchyId"))
                            {
                                this.TargetHierarchyId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(),
                                    this._targetHierarchyId);
                            }
                        }

                        #endregion
                    }
                    else
                    {
                        //Keep on reading the XML until we reach expected node.
                        reader.Read();
                    }
                }
            }
        }


        #endregion Private Methods

        #endregion Methods
    }
}
