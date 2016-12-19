using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects.DynamicTableSchema
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies the DBTable of Dynamic Schema
    /// </summary>
    [DataContract]
    public class DBTable : MDMObject, IDBTable, IDataModelObject
    {
        #region Fields

        /// <summary>
        /// Field denoting the populate RST object of an DBTable
        /// </summary>
        private Boolean _populateRSTObject = false;

        /// <summary>
        /// Field denoting the list of DBColumn of an DBTable
        /// </summary>
        private DBColumnCollection _DBColumns = new DBColumnCollection();

        /// <summary>
        /// Field denoting the list of Constraints of an DBTable
        /// </summary>
        private DBConstraintCollection _Constraints = new DBConstraintCollection();

        /// <summary>
        /// Field denoting the list of Relationships of an DBTable
        /// </summary>
        private DBRelationshipCollection _Relationships = new DBRelationshipCollection();

        /// <summary>
        /// Specifies DynamicTableType for table.
        /// </summary>
        private DynamicTableType _dynamicTableType = DynamicTableType.Unknown;

        /// <summary>
        /// Field denoting the Original DBTable
        /// </summary>
        private DBTable _originalDBTable = null;

        /// <summary>
        /// Field denoting attribute id for DBTable
        /// </summary>
        private Int32 _attributeId = -1;

        #region IDataModelObject Fields

        /// <summary>
        /// Field denoting lookup model key External Id from external source.
        /// </summary>
        private String _externalId = String.Empty;

        #endregion

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public DBTable()
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
        ///         &lt;Table
        ///             Id="101" 
        ///             Name="tblk_Color"
        ///             Action="Update"
        ///             PopulateRSTObject="false"/&gt;
        /// </para>
        /// </example>
        public DBTable(String valuesAsXml)
        {
            LoadDBTable(valuesAsXml);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Property denoting the PopulateRSTObject of an DBTable
        /// </summary>
        [DataMember]
        public Boolean PopulateRSTObject
        {
            get
            {
                return _populateRSTObject;
            }
            set
            {
                _populateRSTObject = value;
            }
        }

        /// <summary>
        /// Property denoting the list of DBColumns of an DBTable
        /// </summary>
        [DataMember]
        public DBColumnCollection Columns
        {
            get
            {
                return _DBColumns;
            }
            set
            {
                _DBColumns = value;
            }
        }

        /// <summary>
        /// Property denoting the list of DBColumns of an DBTable
        /// </summary>
        [DataMember]
        public DBConstraintCollection Constraints
        {
            get
            {
                return _Constraints;
            }
            set
            {
                _Constraints = value;
            }
        }

        /// <summary>
        /// Property denoting the list of DBRelationships of an DBTable
        /// </summary>
        [DataMember]
        public DBRelationshipCollection Relationships
        {
            get
            {
                return _Relationships;
            }
            set
            {
                _Relationships = value;
            }
        }

        /// <summary>
        ///  Property denoting the DynamicTableType.
        /// </summary>
        [DataMember]
        public DynamicTableType DynamicTableType
        {
            get
            {
                return _dynamicTableType;
            }
            set
            {
                _dynamicTableType = value;
            }
        }

        /// <summary>
        /// Property denoting the Original DBTable
        /// </summary>
        public DBTable OriginalDBTable
        {
            get
            {
                return _originalDBTable;
            }
            set
            {
                _originalDBTable = value;
            }
        }

        /// <summary>
        /// Property denoting attribute id for DBTable
        /// </summary>
        public Int32 AttributeId
        {
            get { return _attributeId; }
            set { _attributeId = value; }
        }

        #region IDataModelObject Properties

        /// <summary>
        /// Property denoting a ObjectType for an dataModelObject object
        /// </summary>
        public ObjectType DataModelObjectType
        {
            get
            {
                return MDM.Core.ObjectType.LookupModel;
            }
        }

        /// <summary>
        /// Property denoting the external id for an dataModelObject object
        /// </summary>
        public String ExternalId
        {
            get
            {
                return _externalId;
            }
            set
            {
                _externalId = value;
            }
        }

        #endregion

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Determines whether specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">DBTable object which needs to be compared.</param>
        /// <returns>Result of the comparison in Boolean.</returns>
        public override Boolean Equals(object obj)
        {
            if (CompareDBTableMetaDataProperties(obj))
            {
                DBTable objectToBeCompared = obj as DBTable;

                if (!this.Columns.Equals(objectToBeCompared.Columns))
                    return false;

                if (!this.Constraints.Equals(objectToBeCompared.Constraints))
                    return false;

                if (!this.Relationships.Equals(objectToBeCompared.Relationships))
                    return false;

                return true;
            }

            return false;
        }

        /// <summary>
        /// Returns the hash code for this instance
        /// </summary>
        /// <returns>Hash code of the instance</returns>
        public override Int32 GetHashCode()
        {
            return base.GetHashCode() ^ this.PopulateRSTObject.GetHashCode() ^ this.DynamicTableType.GetHashCode() ^ this.Columns.GetHashCode() ^ this.Constraints.GetHashCode() ^ this.Relationships.GetHashCode();
        }

        /// <summary>
        /// Get Xml representation of DBTable
        /// </summary>
        /// <returns>Xml representation of object</returns>
        public override String ToXml()
        {
            String DBTableXml = String.Empty;
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //DBTable node start
            xmlWriter.WriteStartElement("Table");

            #region Write DBTable Properties

            xmlWriter.WriteAttributeString("Id", this.Id.ToString());
            xmlWriter.WriteAttributeString("Name", this.Name);
            xmlWriter.WriteAttributeString("Action", this.Action.ToString());
            xmlWriter.WriteAttributeString("PopulateRSTObject", this.PopulateRSTObject.ToString());
            xmlWriter.WriteAttributeString("DynamicTableType", this.DynamicTableType.ToString());

            #endregion

            #region Write DBColumn Properties

            if (this.Columns != null)
                xmlWriter.WriteRaw(this.Columns.ToXml());

            #endregion

            #region Write DBConstraint Properties

            if (this.Constraints != null)
                xmlWriter.WriteRaw(this.Constraints.ToXml());

            #endregion Write DBConstraint Properties

            #region Write DBRelationship Properties

            if (this.Constraints != null)
                xmlWriter.WriteRaw(this.Relationships.ToXml());

            #endregion Write DBRelationship Properties

            //DBTable node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //get the actual XML
            DBTableXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return DBTableXml;
        }

        /// <summary>
        /// Get Xml representation of DBTable
        /// </summary>
        /// <returns>Xml representation of object</returns>
        public override String ToXml(ObjectSerialization serialization)
        {
            String DBTableXml = String.Empty;

            if (serialization == ObjectSerialization.Full)
            {
                DBTableXml = this.ToXml();
            }
            else
            {
                StringWriter sw = new StringWriter();
                XmlTextWriter xmlWriter = new XmlTextWriter(sw);

                //DBTable node start
                xmlWriter.WriteStartElement("Table");

                if (serialization == ObjectSerialization.ProcessingOnly)
                {
                    #region Write DBTable Properties

                    xmlWriter.WriteAttributeString("Id", this.Id.ToString());
                    xmlWriter.WriteAttributeString("Name", this.Name);
                    xmlWriter.WriteAttributeString("Action", this.Action.ToString());
                    xmlWriter.WriteAttributeString("PopulateRSTObject", this.PopulateRSTObject.ToString());

                    #endregion
                }
                else if (serialization == ObjectSerialization.UIRender)
                {
                    #region Write DBTable Properties

                    xmlWriter.WriteAttributeString("Id", this.Id.ToString());
                    xmlWriter.WriteAttributeString("Name", this.Name);
                    xmlWriter.WriteAttributeString("Action", this.Action.ToString());
                    xmlWriter.WriteAttributeString("PopulateRSTObject", this.PopulateRSTObject.ToString());

                    #endregion
                }

                //DBTable node end
                xmlWriter.WriteEndElement();

                xmlWriter.Flush();

                //get the actual XML
                DBTableXml = sw.ToString();

                xmlWriter.Close();
                sw.Close();
            }
            return DBTableXml;
        }

        /// <summary>
        /// Clone DBTable object
        /// </summary>
        /// <returns>
        /// Cloned copy of IDBTable object.
        /// </returns>
        public IDBTable Clone()
        {
            DBTable clonedDBTable = new DBTable();

            clonedDBTable.Id = this.Id;
            clonedDBTable.Name = this.Name;
            clonedDBTable.LongName = this.LongName;
            clonedDBTable.Locale = this.Locale;
            clonedDBTable.Action = this.Action;
            clonedDBTable.AuditRefId = this.AuditRefId;
            clonedDBTable.ExtendedProperties = this.ExtendedProperties;

            clonedDBTable.PopulateRSTObject = this.PopulateRSTObject;
            clonedDBTable.DynamicTableType = this.DynamicTableType;

            clonedDBTable.Columns = this.Columns.Clone() as DBColumnCollection;
            clonedDBTable.Constraints = this.Constraints.Clone() as DBConstraintCollection;
            clonedDBTable.Relationships = new DBRelationshipCollection(this.Relationships.ToList());

            return clonedDBTable;
        }

        /// <summary>
        /// Delta Merge of DBTable
        /// </summary>
        /// <param name="deltaDBTable">DBTable that needs to be merged.</param>
        /// <param name="iCallerContext">Indicates the name of application and the module that are performing the action</param>
        /// <param name="returnClonedObject">true means return new merged cloned object;otherwise, same object.</param>
        /// <returns>Merged IDBTable instance</returns>
        public IDBTable MergeDelta(IDBTable deltaDBTable, ICallerContext iCallerContext, Boolean returnClonedObject = true)
        {
            DBTable mergedDBTable = (DBTable)((returnClonedObject == true) ? deltaDBTable.Clone() : deltaDBTable);

            mergedDBTable.Action = (CompareDBTableMetaDataProperties(mergedDBTable)) ? ObjectAction.Read : ObjectAction.Update;

            Boolean hasChanged = false;
            Int32 deletedColumnsCount = 0;

            foreach (DBColumn deltaColumn in mergedDBTable.Columns)
            {
                #region Merge and Compare DBColumns and DBConstraints

                DBConstraintCollection deltaDBConstraints = mergedDBTable.Constraints.Get(deltaColumn.Name);
                DBColumn originalDBColumn = this.Columns.Get(deltaColumn.Name);
                DBConstraintCollection originalDBConstraints = this.Constraints.Get(deltaColumn.Name);

                if (deltaColumn.Action == ObjectAction.Read || deltaColumn.Action == ObjectAction.Ignore)
                {
                    deltaDBConstraints.SetAction(ObjectAction.Read);
                    continue;
                }
                else if (deltaColumn.Action == ObjectAction.Delete)
                {
                    if (originalDBColumn != null)
                    {
                        hasChanged = true;
                        deletedColumnsCount++;
                        SetConstraintsAction(originalDBConstraints, deltaDBConstraints, mergedDBTable, ObjectAction.Delete);
                    }

                    continue;
                }

                if (originalDBColumn != null)
                {
                    originalDBColumn.MergeDelta(deltaColumn, iCallerContext, returnClonedObject);

                    if (deltaColumn.Action == ObjectAction.Read)
                    {
                        deltaDBConstraints.SetAction(ObjectAction.Read);
                    }
                    else
                    {
                        deltaDBConstraints.SetAction(ObjectAction.Create);
                        SetConstraintsAction(originalDBConstraints, deltaDBConstraints, mergedDBTable, ObjectAction.Update);
                        hasChanged = true;
                    }
                }
                else
                {
                    //If original object is not found then set Action as Create always.
                    deltaColumn.Action = ObjectAction.Create;
                    deltaDBConstraints.SetAction(ObjectAction.Create);
                    hasChanged = true;
                }

                #endregion
            }

            //If we have any change in columns or constraints then set Table Action also as 'Update'
            if (hasChanged)
            {
                //If original db table and deleted columns count are same ,it means user has deleted all the columns.
                mergedDBTable.Action = (this.Columns.Count == deletedColumnsCount) ? ObjectAction.Delete : ObjectAction.Update;
            }

            return mergedDBTable;
        }

        #region IDataModelObject Methods

        /// <summary>
        /// Get IDataModelObject for current object.
        /// </summary>
        /// <returns>IDataModelObject</returns>
        public IDataModelObject GetDataModelObject()
        {
            return this;
        }

        #endregion

        #endregion

        #region Private Methods

        /// <summary>
        /// Load DBTable object from XML.
        /// </summary>
        /// <param name="valuesAsXml">XML having xml value</param>
        /// <example>
        ///     Sample XML:
        ///     <para>
        ///     <![CDATA[
        ///         <Table
        ///             Id="101" 
        ///             Name="tblk_Color" 
        ///             Action="Update"
        ///             PopulateRSTObject="false"
        ///         </Column>
        ///     ]]>    
        ///     </para>
        /// </example>
        private void LoadDBTable(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "Table")
                        {
                            #region Read DBTable Properties

                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("Id"))
                                {
                                    this.Id = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                                }

                                if (reader.MoveToAttribute("Name"))
                                {
                                    this.Name = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("Action"))
                                {
                                    ObjectAction objectAction = ObjectAction.Unknown;
                                    Enum.TryParse(reader.ReadContentAsString(), out objectAction);
                                    this.Action = objectAction;
                                }

                                if (reader.MoveToAttribute("PopulateRSTObject"))
                                {
                                    this.PopulateRSTObject = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
                                }

                                if (reader.MoveToAttribute("DynamicTableType"))
                                {
                                    DynamicTableType dynamicTableType = DynamicTableType.Unknown;
                                    ValueTypeHelper.EnumTryParse<DynamicTableType>(reader.ReadContentAsString(), true, out dynamicTableType);
                                    this.DynamicTableType = dynamicTableType;
                                }
                            }

                            #endregion
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "Columns")
                        {
                            #region Read Columns Properties

                            String columnsXml = reader.ReadOuterXml();

                            if (!String.IsNullOrEmpty(columnsXml))
                            {
                                DBColumnCollection columns = new DBColumnCollection(columnsXml);
                                this.Columns = columns;
                            }

                            #endregion
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "Constraints")
                        {
                            #region Read Constraints Properties

                            String constraintsXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(constraintsXml))
                            {
                                DBConstraintCollection constraints = new DBConstraintCollection(constraintsXml);
                                this.Constraints = constraints;
                            }

                            #endregion
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "Relationships")
                        {
                            #region Read Relationships Properties

                            String relationshipsXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(relationshipsXml))
                            {
                                DBRelationshipCollection relationships = new DBRelationshipCollection(relationshipsXml);
                                this.Relationships = relationships;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private Boolean CompareDBTableMetaDataProperties(object obj)
        {
            if (base.Equals(obj))
            {
                if (obj is DBTable)
                {
                    DBTable objectToBeCompared = obj as DBTable;

                    if (this.PopulateRSTObject != objectToBeCompared.PopulateRSTObject)
                        return false;

                    if (this.DynamicTableType != objectToBeCompared.DynamicTableType)
                        return false;

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Set the constraints action based on the changes done in dbconstraints
        /// </summary>
        /// <param name="originalDBConstraints">Indicates original db constraints</param>
        /// <param name="deltaDBConstraints">Indicates changes in db constraints</param>
        /// <param name="mergedDBTable">Indicates merged db table</param>
        /// <param name="objectAction">Indicates object action</param>
        private void SetConstraintsAction(DBConstraintCollection originalDBConstraints, DBConstraintCollection deltaDBConstraints,DBTable mergedDBTable, ObjectAction objectAction)
        {
            if (originalDBConstraints != null)
            {
                foreach (DBConstraint originalDBConstraint in originalDBConstraints)
                {
                    DBConstraint deltaDBConstraint = deltaDBConstraints.Get(originalDBConstraint.ColumnName, originalDBConstraint.ConstraintType);

                    //If delta constraint is not found then set Action as Delete.
                    if (deltaDBConstraint == null)
                    {
                        DBConstraint clonedDBConstraint = originalDBConstraint.Clone() as DBConstraint;
                        clonedDBConstraint.Action = ObjectAction.Delete;
                        mergedDBTable.Constraints.Add(clonedDBConstraint);
                    }
                    else
                    {
                        deltaDBConstraint.Action = objectAction;
                    }
                }
            }
        }

        #endregion

        #endregion
    }
}