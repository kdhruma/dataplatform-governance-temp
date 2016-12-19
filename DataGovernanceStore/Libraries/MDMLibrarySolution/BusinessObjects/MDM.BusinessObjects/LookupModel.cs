using System;
using System.Runtime.Serialization;
using System.IO;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies instance of lookup model.
    /// </summary>
    [DataContract]
    public class LookupModel : ILookupModel
    {
        #region Fields

        /// <summary>
        ///  Indicates id of lookup table.
        /// </summary>
        private Int32 _id = 0;

        /// <summary>
        /// Indicates lookup table name with prefix as "tblk_".
        /// </summary>
        private String _tableName = String.Empty;

        /// <summary>
        /// Indicates display table name for lookup without prefix as "tblk_".
        /// </summary>
        private String _displayTableName = String.Empty;

        /// <summary>
        /// Indicates whether lookup is view based or not.
        /// </summary>
        private Boolean _isViewBasedLookup = false;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public LookupModel()
            : base()
        {
        }

        /// <summary>
        /// Constructor with XML having values of object. Populate current object using XML
        /// </summary>
        /// <param name="valuesAsXml">XML having xml value</param>
        /// <example>
        ///     Sample XML:
        ///     <para>
        ///         &lt;LookupModel Id="2" 
        ///             TableName="tblk_Color" 
        ///             DisplayTableName="Color" 
        ///             IsViewBasedLookup="1" /&gt;
        /// </para>
        /// </example>
        public LookupModel(String valuesAsXml)
        {
            LoadLookupModelDetail(valuesAsXml);
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Specifies id of lookup.
        /// </summary>
        [DataMember]
        public Int32 Id
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
        /// Specifies lookup table name with prefix as "tblk_".
        /// </summary>
        [DataMember]
        public String TableName
        {
            get
            {
                return this._tableName;
            }
            set
            {
                this._tableName = value;
            }
        }

        /// <summary>
        /// Specifies display table name for lookup without prefix as "tblk_".
        /// </summary>
        [DataMember]
        public String DisplayTableName
        {
            get
            {
                return this._tableName;
            }
            set
            {
                this._tableName = value;
            }
        }

        /// <summary>
        /// Specifies whether lookup is view based or not.
        /// </summary>
        [DataMember]
        public Boolean IsViewBasedLookup
        {
            get
            {
                return this._isViewBasedLookup;
            }
            set
            {
                this._isViewBasedLookup = value;
            }
        }

        #endregion Properties   

        #region Methods

        #region Public Methods

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override Boolean Equals(object obj)
        {
            if (obj is LookupModel)
            {
                LookupModel objectToBeCompared = obj as LookupModel;

                if (this.Id != objectToBeCompared.Id)
                {
                    return false;
                }
                if (this.TableName != objectToBeCompared.TableName)
                {
                    return false;
                }
                if (this.DisplayTableName != objectToBeCompared.DisplayTableName)
                {
                    return false;
                }
                if (this.IsViewBasedLookup != objectToBeCompared.IsViewBasedLookup)
                {
                    return false;
                }
                return true;
            }

            return false;
        }

        /// <summary>
        /// Serves as a hash function for type
        /// </summary>
        /// <returns>A hash code for the current Object.</returns>
        public override Int32 GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Get XML representation of lookup model object.
        /// </summary>
        /// <returns>XML representation of lookup model object.</returns>
        public String ToXml()
        {
            String lookupModelXml = String.Empty;

            using (StringWriter sw = new StringWriter())
            {
                using (XmlTextWriter xmlWriter = new XmlTextWriter(sw))
                {
                    xmlWriter.WriteStartElement("LookupModel");

                    #region Write relationship properties

                    xmlWriter.WriteAttributeString("Id", this._id.ToString());
                    xmlWriter.WriteAttributeString("TableName", this._tableName);
                    xmlWriter.WriteAttributeString("DisplayTableName", this._tableName);
                    xmlWriter.WriteAttributeString("IsViewBasedLookup", this._isViewBasedLookup.ToString());

                    #endregion

                    //Relationship Type node end
                    xmlWriter.WriteEndElement();

                    //get the actual XML
                    lookupModelXml = sw.ToString();
                }
            }

            return lookupModelXml;
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Load lookup model object from XML.
        /// </summary>
        /// <param name="valuesAsXml">XML having xml value</param>
        /// <example>
        ///     Sample XML:
        ///     <para>
        ///     <![CDATA[
        ///         <LookupModel
        ///             Id="284" 
        ///             TableName="tblk_Color" 
        ///             DisplayTableName="Color" 
        ///             IsViewBasedLookup="false" 
        ///         </LookupModel>
        ///     ]]>    
        ///     </para>
        /// </example>
        private void LoadLookupModelDetail(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (reader.Read())
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "LookupModel" && reader.IsStartElement())
                        {
                            #region Read Lookup Model Attributes

                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("Id"))
                                {
                                    this.Id = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), this._id);
                                }

                                if (reader.MoveToAttribute("TableName"))
                                {
                                    this.TableName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("DisplayTableName"))
                                {
                                    this.DisplayTableName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("IsViewBasedLookup"))
                                {
                                    this.IsViewBasedLookup = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), this._isViewBasedLookup);
                                }
                            }

                            #endregion
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

        #endregion Private Methods

        #endregion Methods
    }
}