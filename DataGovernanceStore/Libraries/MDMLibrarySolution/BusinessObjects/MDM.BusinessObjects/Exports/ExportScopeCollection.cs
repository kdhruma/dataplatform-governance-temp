using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Xml;
using MDM.Interfaces.Exports;

namespace MDM.BusinessObjects.Exports
{
    using MDM.Core;

    /// <summary>
    /// Specifies the export scope collection object
    /// </summary>
    [DataContract]
    [KnownType(typeof(ExportScope))]
    public class ExportScopeCollection : InterfaceContractCollection<IExportScope, ExportScope>, IExportScopeCollection
    {
        #region Fields

        #endregion Fields

        #region Constructors

		/// <summary>
		/// Parameterless constructor
		/// </summary>
		public ExportScopeCollection() { }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        /// <param name="valueAsXml">Values Which needs to be added in the collection in XMl format</param>
        /// <param name="objectSerialization">Specifies the object serialization type</param>
        public ExportScopeCollection(String valueAsXml, ObjectSerialization objectSerialization = ObjectSerialization.Full)
        {
            LoadExportScopeCollection(valueAsXml, objectSerialization);
        }

		/// <summary>
        /// Initialize exportScopeCollection from IList
		/// </summary>
        /// <param name="exportScopeList">IList of exportScopeCollection</param>
        public ExportScopeCollection(IList<ExportScope> exportScopeList)
		{
            this._items = new Collection<ExportScope>(exportScopeList);
		}

        #endregion Constructors

        #region Properties

        #endregion Properties

        #region Methods

        #region Public Methods

        /// <summary>
        /// Get export scope from given object type
        /// </summary>
        /// <param name="objectType">Indicates object type to search on export scope collection</param>
        /// <returns>Returns export scope having given object type</returns>
        public List<ExportScope> GetExportScope(ObjectType objectType)
        {
            List<ExportScope> exportScopes = new List<ExportScope>();

            foreach (ExportScope exportScope in this._items)
            {
                if (exportScope.ObjectType == objectType)
                    exportScopes.Add(exportScope);
            }

            return exportScopes;
        }

        /// <summary>
        /// Get export scope given for a object type and object id
        /// </summary>
        /// <param name="objectType"></param>
        /// <param name="objectId"></param>
        /// <returns></returns>
        public ExportScope GetExportScope(ObjectType objectType,Int32 objectId)
        {

            foreach (ExportScope exportScope in this._items)
            {
                if ((exportScope.ObjectType == objectType) && (exportScope.ObjectId == objectId))
                {
                    return exportScope;
                }
            }

            return null;
        }

        /// <summary>
        /// Get Xml representation of export scope collection object
        /// </summary>
        /// <returns>Xml string representing the export scope collection</returns>
        public String ToXml()
        {
            String exportScopesXml = String.Empty;

            exportScopesXml = "<ExportScopes>";

            if (this._items != null && this._items.Count > 0)
            {
                foreach (ExportScope exportScope in this._items)
                {
                    exportScopesXml = String.Concat(exportScopesXml, exportScope.ToXml());
                }
            }

            exportScopesXml = String.Concat(exportScopesXml, "</ExportScopes>");

            return exportScopesXml;
        }

        /// <summary>
        /// Get Xml representation of export scope collection object
        /// </summary>
        /// <param name="objectSerialization">Specifies the object serialization type</param>
        /// <returns>Xml string representing the export scope collection</returns>
        public String ToXml(ObjectSerialization objectSerialization)
        {
            String exportScopesXml = String.Empty;

            exportScopesXml = "<ExportScopes>";

            if (this._items != null && this._items.Count > 0)
            {
                foreach (ExportScope exportScope in this._items)
                {
                    exportScopesXml = String.Concat(exportScopesXml, exportScope.ToXml(objectSerialization));
                }
            }

            exportScopesXml = String.Concat(exportScopesXml, "</ExportScopes>");

            return exportScopesXml;
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Loads the export scope collection from given XMl and add into this
        /// </summary>
        /// <param name="valuesAsXml">Values Which needs to be added in the collection in XMl format</param>
        /// <param name="objectSerialization">Specifies the object serialization type</param>
        private void LoadExportScopeCollection(String valuesAsXml, ObjectSerialization objectSerialization = ObjectSerialization.Full)
        {
            #region Sample Xml

            /*
                 <ExportScopes>
			        <ExportScope ObjectType="Container" ObjectId="" ObjectUniqueIdentifier="Product Master" Include="" IsRecursive="false">
				        <SearchAttributeRules>
					        <SearchAttributeRule AttributeId="" Operator="" Value="" />
					        <SearchAttributeRule AttributeId="" Operator="" Value="" />
				        </SearchAttributeRules>
				        <ExportScopes>
					        <ExportScope ObjectType="Category" ObjectId="" ObjectUniqueIdentifier="Apparel" Include="" IsRecursive="false">
						        <SearchAttributeRules />
					        </ExportScope>
					        <ExportScope ObjectType="Entity" ObjectId="" ObjectUniqueIdentifier="P1121" Include="" IsRecursive="false">
						        <SearchAttributeRules />
					        </ExportScope>
				        </ExportScopes>
			        </ExportScope>
		        </ExportScopes>
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
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "ExportScope")
                        {
                            #region Read ExportScopes Collection

                            String exportScopesXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(exportScopesXml))
                            {
                                ExportScope exportScope = new ExportScope(exportScopesXml, objectSerialization);
                                if (exportScope != null)
                                {
                                    this.Add(exportScope);
                                }
                            }

                            #endregion Read ExportScopes Collection
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

        #endregion
    }
}
