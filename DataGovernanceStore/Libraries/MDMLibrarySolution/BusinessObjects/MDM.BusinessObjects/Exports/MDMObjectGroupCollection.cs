using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Xml;
using MDM.Interfaces.Exports;

namespace MDM.BusinessObjects.Exports
{
    using MDM.Core;
    using System.Collections;

    /// <summary>
    /// Specifies the mdmobjectgroup collection object
    /// </summary>
    [DataContract]
    public class MDMObjectGroupCollection : InterfaceContractCollection<IMDMObjectGroup, MDMObjectGroup>, IMDMObjectGroupCollection
    {
        #region Fields

        #endregion Fields

        #region Constructors

		/// <summary>
		/// Parameterless constructor
		/// </summary>
		public MDMObjectGroupCollection() { }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        /// <param name="valueAsXml">Values Which needs to be added in the collection in XMl format</param>
        /// <param name="objectSerialization">Specifies the object serialization type</param>
        public MDMObjectGroupCollection(String valueAsXml, ObjectSerialization objectSerialization = ObjectSerialization.Full)
        {
            LoadMDMObjectGroupCollection(valueAsXml, objectSerialization);
        }

		/// <summary>
        /// Initialize mdmobjectgroup collection from IList
		/// </summary>
        /// <param name="mdmObjectGroupsList">IList of mdmobjectgroupcollection</param>
        public MDMObjectGroupCollection(IList<MDMObjectGroup> mdmObjectGroupsList)
		{
            this._items = new Collection<MDMObjectGroup>(mdmObjectGroupsList);
		}

        #endregion Constructors

        #region Properties

        #endregion Properties

        #region Public Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objectType"></param>
        /// <returns></returns>
        public IMDMObjectGroup GetMDMObjectGroup(ObjectType objectType)
        {
            MDMObjectGroup objectGroup = null;

            foreach (MDMObjectGroup group in this._items)
            {
                if (group.ObjectType == objectType)
                {
                    objectGroup = group;
                    break;
                }
            }

            return objectGroup;
        }

        /// <summary>
        /// Get Xml representation of mdmobjectgroup collection object
        /// </summary>
        /// <returns>Xml string representing the mdmobjectgroup collection</returns>
        public String ToXml()
        {
            String mdmObjectGroupsXml = String.Empty;

            mdmObjectGroupsXml = "<MDMObjectGroups>";

            if (this._items != null && this._items.Count > 0)
            {
                foreach (MDMObjectGroup mdmObjectGroup in this._items)
                {
                    mdmObjectGroupsXml = String.Concat(mdmObjectGroupsXml, mdmObjectGroup.ToXml());
                }
            }

            mdmObjectGroupsXml = String.Concat(mdmObjectGroupsXml, "</MDMObjectGroups>");

            return mdmObjectGroupsXml;
        }

        /// <summary>
        /// Get Xml representation of mdmobjectgroup collection object
        /// </summary>
        /// <param name="objectSerialization">Specifies the object serialization type</param>
        /// <returns>Xml string representing the mdmobjectgroup collection</returns>
        public String ToXml(ObjectSerialization objectSerialization)
        {
            String mdmObjectGroupsXml = String.Empty;

            //TODO:: Write logic as per the different object serialization type whenever needed..
            if (objectSerialization != ObjectSerialization.Full)
            {
                mdmObjectGroupsXml = "<MDMObjectGroups>";

                if (this._items != null && this._items.Count > 0)
                {
                    foreach (MDMObjectGroup mdmObjectGroup in this._items)
                    {
                        mdmObjectGroupsXml = String.Concat(mdmObjectGroupsXml, mdmObjectGroup.ToXml(objectSerialization));
                    }
                }

                mdmObjectGroupsXml = String.Concat(mdmObjectGroupsXml, "</MDMObjectGroups>");
            }
            else
            {
                mdmObjectGroupsXml = this.ToXml();
            }

            return mdmObjectGroupsXml;
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Loads the profile setting collection from given XMl and add into this
        /// </summary>
        /// <param name="valuesAsXml">Values Which needs to be added in the collection in XMl format</param>
        /// <param name="objectSerialization">Specifies the object serialization type</param>
        private void LoadMDMObjectGroupCollection(String valuesAsXml, ObjectSerialization objectSerialization = ObjectSerialization.Full)
        {
            #region Sample Xml

            /*
				<MDMObjectGroup ObjectType="CommonAtttribute" IncludeAll="" IncludeEmpty="" StartWith="">
					    <MDMObjects>
						    <MDMObject Id="" Locator="" Include="" MappedName="" />
						    <MDMObject Id="" Locator="" Include="" MappedName="" />
					    </MDMObjects>
				    </MDMObjectGroup>
				    <MDMObjectGroup ObjectType="CategoryAttribute" IncludeAll="" IncludeEmpty="" StartWith="">
					    <MDMObjects />
				    </MDMObjectGroup>
				    <MDMObjectGroup ObjectType="SystemAttribute" IncludeAll="" IncludeEmpty="" StartWith="">
					    <MDMObjects />
				    </MDMObjectGroup>
				    <MDMObjectGroup ObjectType="WorkflowAttribute" IncludeAll="" IncludeEmpty="" StartWith="">
					    <MDMObjects />
				    </MDMObjectGroup>
				    <MDMObjectGroup ObjectType="EntityType" IncludeAll="" IncludeEmpty="" StartWith="">
					    <MDMObjects />
				    </MDMObjectGroup>
				    <MDMObjectGroup ObjectType="RelationshipType" IncludeAll="" IncludeEmpty="" StartWith="">
					    <MDMObjects />
				    </MDMObjectGroup>
				    <MDMObjectGroup ObjectType="RelationshipAttributes" IncludeAll="" IncludeEmpty="" StartWith="">
					    <MDMObjects />
				    </MDMObjectGroup>
				    <MDMObjectGroup ObjectType="Locale" IncludeAll="" IncludeEmpty="" StartWith="">
					    <MDMObjects />
				    </MDMObjectGroup>
			    </MDMObjectGroups>
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
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "MDMObjectGroup")
                        {
                            #region Read mdmObjectGroup Collection

                            String mdmObjectGroupsXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(mdmObjectGroupsXml))
                            {
                                MDMObjectGroup mdmObjectGroup = new MDMObjectGroup(mdmObjectGroupsXml, objectSerialization);
                                if (mdmObjectGroup != null)
                                {
                                    this.Add(mdmObjectGroup);
                                }
                            }

                            #endregion Read mdmObjectGroup Collection
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
