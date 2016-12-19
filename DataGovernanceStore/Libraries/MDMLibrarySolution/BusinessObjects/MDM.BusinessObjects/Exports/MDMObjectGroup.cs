using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects.Exports
{
    using MDM.Core;
    using MDM.Interfaces.Exports;

    /// <summary>
    /// Specifies the mdmobjectgroup object
    /// </summary>
    [DataContract]
    public class MDMObjectGroup : MDMObject, IMDMObjectGroup
    {
        #region Fields

        /// <summary>
        /// Field specifying object type of mdmObjectGroup
        /// </summary>
        private ObjectType _objectType = 0;

        /// <summary>
        /// Field specifying include all mdmobject or not 
        /// </summary>
        private Boolean _includeAll = false;

        /// <summary>
        /// Field specifying include all mdmobject or not 
        /// </summary>
        private Boolean _includeEmpty = false;

        /// <summary>
        /// Field specifying include all mdmobject or not 
        /// </summary>
        private Boolean _startWith = true;

        /// <summary>
        /// Field specifying collection of mdmObject
        /// </summary>
        private MDMObjectCollection _mdmObjects = new MDMObjectCollection();

        #endregion Fields

        #region Properties

        /// <summary>
        /// Property specifies object type of mdmObjectGroup
        /// </summary>
        [DataMember]
        public new ObjectType ObjectType
        {
            get
            {
                return _objectType;
            }
            set
            {
                _objectType = value;
            }
        }

        /// <summary>
        /// Property specifies include all mdmobject or not
        /// </summary>
        [DataMember]
        public Boolean IncludeAll
        {
            get
            {
                return _includeAll;
            }
            set
            {
                _includeAll = value;
            }
        }

        /// <summary>
        /// Property specifies include empty mdmobject or not
        /// </summary>
        [DataMember]
        public Boolean IncludeEmpty
        {
            get
            {
                return _includeEmpty;
            }
            set
            {
                _includeEmpty = value;
            }
        }

        /// <summary>
        /// Property specifies start with mdmobject or not
        /// </summary>
        [DataMember]
        public Boolean StartWith
        {
            get
            {
                return _startWith;
            }
            set
            {
                _startWith = value;
            }
        }

        /// <summary>
        /// Property specifies mdmObjectCollection object
        /// </summary>
        [DataMember]
        public MDMObjectCollection MDMObjects
        {
            get
            {
                return _mdmObjects;
            }
            set
            {
                _mdmObjects = value;
            }
        }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Initializes mdmobjectgroup object with default parameters
        /// </summary>
        public MDMObjectGroup() : base() { }

        /// <summary>
        /// Initializes object with the values provided as Xml
        /// </summary>
        /// <param name="valuesAsXml">Xml string having values</param>
        /// <param name="objectSerialization">Specifies Enum type of objectSerialization</param>
        public MDMObjectGroup(String valuesAsXml, ObjectSerialization objectSerialization = ObjectSerialization.Full)
        {
            LoadMDMObjectGroup(valuesAsXml, objectSerialization);
        }

        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IList GetMDMObjectNames()
        {
            List<String> items = new List<String>();

            foreach (MDMObject mdmObject in this._mdmObjects)
            {
                items.Add(mdmObject.Name);    
            }

            return items;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IList GetMDMObjectIdList()
        {
            List<Int32> items = new List<Int32>();

            foreach (MDMObject mdmObject in this._mdmObjects)
            {
                items.Add(mdmObject.Id);
            }

            return items;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Collection<Int32> GetMDMObjectIds()
        {
            Collection<Int32> items = new Collection<Int32>();

            foreach (MDMObject mdmObject in this._mdmObjects)
            {
                items.Add(mdmObject.Id);
            }

            return items;
        }

        /// <summary>
        /// Represents mdmobjectgroup in Xml format
        /// </summary>
        /// <returns>String representation of current mdmobjectgroup object</returns>
        public override String ToXml()
        {
            String mdmObjectGroupXml = String.Empty;
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //MDMObjectGroup node start
            xmlWriter.WriteStartElement("MDMObjectGroup");

            #region write MDMObjectGroup properties for full MDMObjectGroup xml

            xmlWriter.WriteAttributeString("ObjectType", this.ObjectType.ToString());
            xmlWriter.WriteAttributeString("IncludeAll", this.IncludeAll.ToString());
            xmlWriter.WriteAttributeString("IncludeEmpty", this.IncludeEmpty.ToString());
            xmlWriter.WriteAttributeString("StartWith", this.StartWith.ToString());

            #endregion  write MDMObjectGroup properties for full MDMObjectGroup xml

            #region write mdmObjects for Full mdmObject Xml

            if (this.MDMObjects != null)
                xmlWriter.WriteRaw(this.MDMObjects.ToXml());

            #endregion write mdmObjectsfor Full mdmObject Xml

            //MDMObjectGroup node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //get the actual XML
            mdmObjectGroupXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return mdmObjectGroupXml;
        }

        /// <summary>
        /// Represents mdmobjectgroup in Xml format
        /// </summary>
        /// <param name="objectSerialization">Specifies the object serialization type</param>
        /// <returns>String representation of current mdmobjectgroup object</returns>
        public override String ToXml(ObjectSerialization objectSerialization)
        {
            String mdmObjectGroupXml = String.Empty;

            //TODO:: Write logic as per the different object serialization type whenever needed..
            if (objectSerialization != ObjectSerialization.Full)
            {
                StringWriter sw = new StringWriter();
                XmlTextWriter xmlWriter = new XmlTextWriter(sw);

                //MDMObjectGroup node start
                xmlWriter.WriteStartElement("MDMObjectGroup");

                #region write MDMObjectGroup properties for full MDMObjectGroup xml

                xmlWriter.WriteAttributeString("ObjectType", this.ObjectType.ToString());
                xmlWriter.WriteAttributeString("IncludeAll", this.IncludeAll.ToString());
                xmlWriter.WriteAttributeString("IncludeEmpty", this.IncludeEmpty.ToString());
                xmlWriter.WriteAttributeString("StartWith", this.StartWith.ToString());

                #endregion  write MDMObjectGroup properties for full MDMObjectGroup xml

                #region write mdmObjects for Full mdmObject Xml

                if (this.MDMObjects != null)
                    xmlWriter.WriteRaw(this.MDMObjects.ToXml(objectSerialization));

                #endregion write mdmObjectsfor Full mdmObject Xml

                //MDMObjectGroup node end
                xmlWriter.WriteEndElement();

                xmlWriter.Flush();

                //get the actual XML
                mdmObjectGroupXml = sw.ToString();

                xmlWriter.Close();
                sw.Close();
            }
            else
            {
                mdmObjectGroupXml = this.ToXml();
            }

            return mdmObjectGroupXml;
        }

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
            {
                if (obj is MDMObjectGroup)
                {
                    MDMObjectGroup objectToBeCompared = obj as MDMObjectGroup;

                    if (!this.MDMObjects.Equals(objectToBeCompared.MDMObjects))
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
            hashCode = base.GetHashCode() ^ this.MDMObjects.GetHashCode();

            return hashCode;
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Loads the mdmobjectgroup with the XML having values of object
        /// </summary>
        /// <param name="valuesAsXml">XML having values of object</param>
        /// <param name="objectSerialization">Specifies the object serialization type</param>
        private void LoadMDMObjectGroup(String valuesAsXml, ObjectSerialization objectSerialization = ObjectSerialization.Full)
        {
            #region Sample Xml

            /*
                <MDMObjectGroup ObjectType="CommonAtttribute" IncludeAll="" IncludeEmpty="" StartWith="">
					<MDMObjects>
						<MDMObject Id="" Locator="" Include="" MappedName="" />
						<MDMObject Id="" Locator="" Include="" MappedName="" />
					</MDMObjects>
				</MDMObjectGroup>
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
                        #region Read MDMObjectGroup

                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "MDMObjectGroup")
                        {
                            #region Read mdmObjectGroup Properties

                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("ObjectType"))
                                {
                                    ObjectType objectType = ObjectType.None;
                                    Enum.TryParse<ObjectType>(reader.ReadContentAsString(), out objectType);
                                    this.ObjectType = objectType;
                                }

                                if (reader.MoveToAttribute("IncludeAll"))
                                {
                                    this.IncludeAll = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
                                }

                                if (reader.MoveToAttribute("IncludeEmpty"))
                                {
                                    this.IncludeEmpty = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
                                }

                                if (reader.MoveToAttribute("StartWith"))
                                {
                                    this.StartWith = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
                                }

                            }

                            #endregion
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "MDMObjects")
                        {
                            #region Read mdmObjects collection

                            String mdmObjectsXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(mdmObjectsXml))
                            {
                                MDMObjectCollection mdmObjectCollection = new MDMObjectCollection(mdmObjectsXml);
                                if (mdmObjectCollection != null)
                                {
                                    foreach (MDMObject mdmObject in mdmObjectCollection)
                                    {
                                        if (!this.MDMObjects.Contains(mdmObject))
                                        {
                                            this.MDMObjects.Add(mdmObject);
                                        }
                                    }
                                }
                            }

                            #endregion Read search attribute rules collection
                        }
                        else
                        {
                            reader.Read();
                        }

                        #endregion Read MDMObjectGroup
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
    }
}
