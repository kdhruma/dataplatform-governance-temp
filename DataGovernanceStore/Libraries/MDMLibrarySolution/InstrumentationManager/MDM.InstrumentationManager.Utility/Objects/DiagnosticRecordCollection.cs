using System;
using System.Xml;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace MDM.BusinessObjects.Diagnostics
{
    using MDM.Core;
    using MDM.Interfaces;
    using MDM.Interfaces.Diagnostics;

    /// <summary>
    /// Specifies DiagnosticRecord Collection
    /// </summary>
    [DataContract]
    [ProtoBuf.ProtoContract]
    public class DiagnosticRecordCollection : InterfaceContractCollection<IDiagnosticRecord,DiagnosticRecord>, IDiagnosticRecordCollection
    {
        #region Constructors
        
        /// <summary>
        /// Initializes a new instance of the DiagnosticRecordCollection class.
        /// </summary>
        public DiagnosticRecordCollection() { }

        /// <summary>
        /// Initializes a new instance of the DiagnosticRecord class.
        /// </summary>
        /// <param name="diagnosticRecordList">List of diagnosticRecord object</param>
        public DiagnosticRecordCollection(List<DiagnosticRecord> diagnosticRecordList)
        {
            if (diagnosticRecordList != null)
            {
                this._items = new Collection<DiagnosticRecord>(diagnosticRecordList);
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Get Xml representation of DiagnosticRecord Collection
        /// </summary>
        /// <returns>Xml representation of DiagnosticRecord Collection</returns>
        public String ToXml()
        {
            String diagnosticRecordCollectionXml = String.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //Parameter node start
            xmlWriter.WriteStartElement("DiagnosticRecords");

            if (this._items != null && this._items.Count > 0)
            {
                foreach (DiagnosticRecord item in this._items)
                {
                    xmlWriter.WriteRaw(item.ToXml());
                }
            }

            //Param node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //Get the actual XML
            diagnosticRecordCollectionXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return diagnosticRecordCollectionXml;
        }

        /// <summary>
        /// Loads Diagnostic Records Collection from String
        /// </summary>
        /// <param name="valuesAsXml">Diagnostic Record Collection serialized in Xml String</param>
        public void LoadFromXml(String valuesAsXml)
        {
            if (String.IsNullOrEmpty(valuesAsXml))
            {
                return;
            }

            XmlReader reader = null;

            try
            {
                reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                while (!reader.EOF)
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "DiagnosticRecord")
                    {
                        DiagnosticRecord diagnosticRecord = new DiagnosticRecord();

                        String diagnosticRecordXml = reader.ReadOuterXml();

                        diagnosticRecord.LoadFromXml(diagnosticRecordXml);

                        Add(diagnosticRecord);
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

        /// <summary>
        /// GetByReferenceId
        /// </summary>
        /// <param name="index"></param>
        /// <returns>DiagnosticRecord</returns>
        public DiagnosticRecord GetByReferenceId(long index)
        {
            foreach (var item in this._items)
            {
                if (item.ReferenceId == index)
                {
                    return item;
                }
            }

            return null;
        }

        /// <summary>
        /// Removes element from collection at specified index
        /// </summary>
        /// <param name="index"></param>
        public void RemoveAt(Int32 index)
        {
            _items.RemoveAt(index);
        }

        #endregion

        #region Private Methods

        #endregion
    }
}
