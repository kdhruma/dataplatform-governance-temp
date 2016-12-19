using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;
    using MDM.BusinessObjects.Interfaces;

    /// <summary>
    /// Specifies Sources Collection
    /// </summary>
    [DataContract]
    public class SourceCollection : InterfaceContractCollection<ISource, Source>, ISourceCollection
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the Source Collection
        /// </summary>
        public SourceCollection()
        { }

        /// <summary>
        /// Initialize Sources collection from IList
        /// </summary>
        /// <param name="sourcesList">Source items</param>
        public SourceCollection(IList<Source> sourcesList)
        {
            this._items = new Collection<Source>(sourcesList);
        }
        /// <summary>
        /// Initializes a new instance of the Source Collection from Xml
        /// </summary>
        /// <param name="valuesAsXml">Serialized source collection</param>
        public SourceCollection(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "Source")
                        {
                            String sourceXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(sourceXml))
                            {
                                Source source = new Source(sourceXml);
                                this.Add(source);
                            }
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

        #endregion

        #region Public Members

        /// <summary>
        /// Get source object from collection by Id
        /// </summary>
        /// <param name="sourceId">Id of the source</param>
        /// <returns>Source object</returns>
        public Source Get(Int32 sourceId)
        {
            return this._items.FirstOrDefault(source => source.Id == sourceId);
        }

        /// <summary>
        /// Get source object form collection by source object with only Id property populated
        /// </summary>
        /// <param name="source">Source object with only Id property populated</param>
        /// <returns>Source object</returns>
        public Source Get(Source source)
        {
            return source == null ? source : this._items.FirstOrDefault(s => s.Id == source.Id);
        }

        /// <summary>
        /// Serialization to Xml
        /// </summary>
        /// <returns>Xml representation of Source Collection</returns>
        public string ToXml()
        {
            String sourceXml = String.Empty;
            StringBuilder builder = new StringBuilder();

            foreach (Source source in this._items)
            {
                builder.Append(source.ToXml());
            }

            sourceXml = String.Format("<Sources>{0}</Sources>", builder);
            return sourceXml;
        }

        #endregion Public Members

        #region ICloneable Members

        /// <summary>
        /// Clone Sources collection
        /// </summary>
        /// <returns>Cloned Sources collection object</returns>
        public object Clone()
        {
            SourceCollection clonedSources = new SourceCollection();

            if (this._items != null && this._items.Count > 0)
            {
                foreach (Source source in this._items)
                {
                    Source clonedSource = source.Clone() as Source;
                    clonedSources.Add(clonedSource);
                }
            }
            return clonedSources;
        }

        #endregion
    }
}