using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects.Exports
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies the mdmobject collection object
    /// </summary>
    [DataContract]
    public class MDMObjectCollection : InterfaceContractCollection<IMDMObject, MDMObject> , IMDMObjectCollection
    {
        #region Fields

        #endregion Fields

        #region Constructors

		/// <summary>
		/// Parameterless constructor
		/// </summary>
		public MDMObjectCollection() { }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        /// <param name="valueAsXml">Values Which needs to be added in the collection in XMl format</param>
        /// <param name="objectSerialization">Specifies the object serialization type</param>
        public MDMObjectCollection(String valueAsXml, ObjectSerialization objectSerialization = ObjectSerialization.Full)
        {
            LoadMDMObjectCollection(valueAsXml, objectSerialization);
        }

		/// <summary>
        /// Initialize mdmobject collection from IList
		/// </summary>
        /// <param name="mdmObjectList">IList of mdmobjectcollection</param>
        public MDMObjectCollection(IList<MDMObject> mdmObjectList)
		{
            this._items = new Collection<MDMObject>(mdmObjectList);
		}

        #endregion Constructors

        #region Properties

        #endregion Properties

        #region Public Methods

        /// <summary>
        /// Get Xml representation of mdmobject collection object
        /// </summary>
        /// <returns>Xml string representing the mdmobject collection</returns>
        public String ToXml()
        {
            String mdmObjectsXml = String.Empty;

            mdmObjectsXml = "<MDMObjects>";

            if (this._items != null && this._items.Count > 0)
            {
                foreach (MDMObject mdmObject in this._items)
                {
                    mdmObjectsXml = String.Concat(mdmObjectsXml, mdmObject.ToXml());
                }
            }

            mdmObjectsXml = String.Concat(mdmObjectsXml, "</MDMObjects>");

            return mdmObjectsXml;
        }

        /// <summary>
        /// Get Xml representation of mdmobject collection object
        /// </summary>
        /// <param name="objectSerialization">Specifies the object serialization type</param>
        /// <returns>Xml string representing the mdmobject collection</returns>
        public String ToXml(ObjectSerialization objectSerialization)
        {
            String mdmObjectsXml = String.Empty;

            //TODO:: Write logic as per the different object serialization type whenever needed..
            if (objectSerialization != ObjectSerialization.Full)
            {
                mdmObjectsXml = "<MDMObjects>";

                if (this._items != null && this._items.Count > 0)
                {
                    foreach (MDMObject mdmObject in this._items)
                    {
                        mdmObjectsXml = String.Concat(mdmObjectsXml, mdmObject.ToXml(objectSerialization));
                    }
                }

                mdmObjectsXml = String.Concat(mdmObjectsXml, "</MDMObjects>");
            }
            else
            {
                mdmObjectsXml = this.ToXml();
            }

            return mdmObjectsXml;
        }

        /// <summary>
        /// Add mdm object object in collection
        /// </summary>
        /// <param name="mdmObject">MDMObject to add in collection</param>
        public new void Add(MDMObject mdmObject)
        {
            this._items.Add(mdmObject);
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Loads the mdmobject collection from given XMl and add into this
        /// </summary>
        /// <param name="valuesAsXml">Values Which needs to be added in the collection in XMl format</param>
        /// <param name="objectSerialization">Specifies the object serialization type</param>
        private void LoadMDMObjectCollection(String valuesAsXml, ObjectSerialization objectSerialization = ObjectSerialization.Full)
        {
            #region Sample Xml

            /*
                 <MDMObjects>
					<MDMObject Id="" Locator="" Include="" MappedName="" />
					<MDMObject Id="" Locator="" Include="" MappedName="" />
				</MDMObjects>
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
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "MDMObject")
                        {
                            #region Read MDMObject Collection

                            String mdmObjectsXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(mdmObjectsXml))
                            {
                                MDMObject mdmObject = new MDMObject(mdmObjectsXml, objectSerialization);
                                if (mdmObject != null)
                                {
                                    this.Add(mdmObject);
                                }
                            }

                            #endregion Read MDMObject Collection
                        }
                        else
                        {
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

        #endregion Private Methods
    }
}
