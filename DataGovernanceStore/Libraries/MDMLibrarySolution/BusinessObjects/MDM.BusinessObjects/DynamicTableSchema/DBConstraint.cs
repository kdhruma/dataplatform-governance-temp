using System;
using System.Runtime.Serialization;
using System.IO;
using System.Xml;

namespace MDM.BusinessObjects.DynamicTableSchema
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class DBConstraint : ObjectBase, IDBConstraint
    {
        #region Fields

        /// <summary>
        /// 
        /// </summary>
        String _columnName = String.Empty;

        /// <summary>
        /// 
        /// </summary>
        ConstraintType _constraintType = ConstraintType.None;

        /// <summary>
        /// 
        /// </summary>
        String _value = String.Empty;

        /// <summary>
        /// 
        /// </summary>
        ObjectAction _action = ObjectAction.Unknown;

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public String ColumnName
        {
            get
            {
                return _columnName;
            }

            set
            {
                _columnName = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public ConstraintType ConstraintType
        {
            get
            {
                return _constraintType;
            }

            set
            {
                _constraintType = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public String Value
        {
            get
            {
                return _value;
            }

            set
            {
                _value = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public ObjectAction Action
        {
            get
            {
                return _action;
            }
            set
            {
                _action = value;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public DBConstraint()
            : base()
        {
        }

        /// <summary>
        /// DBConstraint will be populated with columnName, constraintType, value and action
        /// </summary>
        /// <param name="columnName">Indicates constraint is for whic column</param>
        /// <param name="constraintType">Indicates the type of constraint</param>
        /// <param name="value">Indicates value of constraint</param>
        /// <param name="action">Indicates if constraint is Added/ Deleted/ Updated</param>
        public DBConstraint(String columnName, ConstraintType constraintType, String value, ObjectAction action)
        {
            _columnName = columnName;
            _constraintType = constraintType;
            _value = value;
            _action = action;
        }

        /// <summary>
        /// Constructor with XML having values of object. Populate current object using XML
        /// </summary>
        /// <param name="valuesAsXml">XML having xml value</param>
        /// <example>
        ///     Sample XML:
        ///     <para>
        ///     <![CDATA[
        ///        <Constraint>
        ///                ColumnName="LookupKey" 
        ///                ConstraintType = "DefaultValue"
        ///                Value = "Default"
        ///                Action = "Create"
        ///         <Constraint>
        ///     ]]>    
        ///     </para>
        /// </example>
        public DBConstraint(String valuesAsXml)
        {
            LoadDBConstraint(valuesAsXml);
        }

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override Boolean Equals(object obj)
        {
            if (base.Equals(obj))
            {
                if (obj is DBConstraint)
                {
                    DBConstraint objectToBeCompared = obj as DBConstraint;

                    if (this.ColumnName != objectToBeCompared.ColumnName)
                        return false;

                    if (this.Value != objectToBeCompared.Value)
                        return false;

                    if (this.ConstraintType != objectToBeCompared.ConstraintType)
                        return false;

                    if (this.Action != objectToBeCompared.Action)
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

            hashCode = base.GetHashCode() ^ this.ColumnName.GetHashCode() ^ this.Value.GetHashCode() ^ this.ConstraintType.GetHashCode() ^ this.Action.GetHashCode();

            return hashCode;
        }

        /// <summary>
        /// Clone DBConstraint object
        /// </summary>
        /// <returns>
        /// Cloned copy of IDBConstraint object.
        /// </returns>
        public IDBConstraint Clone()
        {
            DBConstraint clonedDBConstraint = new DBConstraint();

            clonedDBConstraint.ColumnName = this.ColumnName;
            clonedDBConstraint.ConstraintType = this.ConstraintType;
            clonedDBConstraint.Value = this.Value;
            clonedDBConstraint.Action = this.Action;

            return clonedDBConstraint;
        }
        
        #region ToXml Methods

        /// <summary>
        /// Get Xml representation of DBConstraint
        /// </summary>
        /// <returns>Xml representation of object</returns>
        public String ToXml()
        {
            String dbConstraintXml = String.Empty;
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //DBConstraint node start
            xmlWriter.WriteStartElement("Constraint");

            #region Write DBConstraint Properties

            xmlWriter.WriteAttributeString("ColumnName", this.ColumnName);
            xmlWriter.WriteAttributeString("ConstraintType", this.ConstraintType.ToString());
            xmlWriter.WriteAttributeString("Value", this.Value.ToString());
            xmlWriter.WriteAttributeString("Action", this.Action.ToString());

            #endregion

            //Constraint node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //get the actual XML
            dbConstraintXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return dbConstraintXml;
        }

        /// <summary>
        /// Get Xml representation of DBConstraint
        /// </summary>
        /// <returns>Xml representation of object</returns>
        public String ToXml(ObjectSerialization serialization)
        {
            String DBConstraintXml = String.Empty;

            if (serialization == ObjectSerialization.Full)
            {
                DBConstraintXml = this.ToXml();
            }
            else
            {
                StringWriter sw = new StringWriter();
                XmlTextWriter xmlWriter = new XmlTextWriter(sw);

                //DBConstraint node start
                xmlWriter.WriteStartElement("Constraint");

                if (serialization == ObjectSerialization.ProcessingOnly)
                {
                    #region Write DBConstraint Properties

                    xmlWriter.WriteAttributeString("ColumnName", this.ColumnName);
                    xmlWriter.WriteAttributeString("ConstraintType", this.ConstraintType.ToString());
                    xmlWriter.WriteAttributeString("Value", this.Value.ToString());
                    xmlWriter.WriteAttributeString("Action", this.Action.ToString());

                    #endregion
                }
                else if (serialization == ObjectSerialization.UIRender)
                {
                    #region Write DBConstraint Properties

                    xmlWriter.WriteAttributeString("ColumnName", this.ColumnName);
                    xmlWriter.WriteAttributeString("ConstraintType", this.ConstraintType.ToString());
                    xmlWriter.WriteAttributeString("Value", this.Value.ToString());
                    xmlWriter.WriteAttributeString("Action", this.Action.ToString());

                    #endregion
                }

                //Constraint node end
                xmlWriter.WriteEndElement();

                xmlWriter.Flush();

                //get the actual XML
                DBConstraintXml = sw.ToString();

                xmlWriter.Close();
                sw.Close();
            }
            return DBConstraintXml;
        }

        #endregion ToXml Methods

        #endregion

        #region Private Methods

        /// <summary>
        /// Load DBConstraint object from XML.
        /// </summary>
        /// <param name="valuesAsXml">XML having xml value</param>
        /// <example>
        ///     Sample XML:
        ///     <para>
        ///     <![CDATA[
        ///        <Constraint>
        ///                ColumnName="LookupKey" 
        ///                ConstraintType = "DefaultValue"
        ///                Value = "Default"
        ///                Action = "Create"
        ///         <Constraint>
        ///     ]]>    
        ///     </para>
        /// </example>
        private void LoadDBConstraint(String valuesAsXml)
        {
            #region Sample Xml
            /*
                <Constraint>
                        ColumnName="LookupKey" 
                        ConstraintType = "DefaultValue"
                        Value = "Default"
                        Action = "Create"
                 <Constraint>
            */
            #endregion Sample Xml

            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "Constraint")
                        {
                            #region Read DBConstraint Properties

                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("ColumnName"))
                                {
                                    this.ColumnName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("Value"))
                                {
                                    this.Value = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("ConstraintType"))
                                {
                                    this.ConstraintType = ConstraintType.None;
                                    ConstraintType constraintType = Core.ConstraintType.None;
                                    Enum.TryParse(reader.ReadContentAsString(), out constraintType);
                                    this.ConstraintType = constraintType;
                                }

                                if (reader.MoveToAttribute("Action"))
                                {
                                    ObjectAction action = ObjectAction.Unknown;
                                    Enum.TryParse(reader.ReadContentAsString(), out action);
                                    this.Action = action;
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

        #endregion
    }
}