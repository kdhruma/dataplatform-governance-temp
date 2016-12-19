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
    /// Specifies DiagnosticActivity Collection
    /// </summary>
    [DataContract]
    [ProtoBuf.ProtoContract]
    public class DiagnosticActivityCollection : InterfaceContractCollection<IDiagnosticActivity, DiagnosticActivity>, IDiagnosticActivityCollection
    {
        #region Constructors
        
        /// <summary>
        /// Initializes a new instance of the DiagnosticActivityCollection class.
        /// </summary>
        public DiagnosticActivityCollection() { }

        /// <summary>
        /// Initializes a new instance of the DiagnosticActivity class.
        /// </summary>
        /// <param name="diagnosticActivityList">List of diagnosticRecord object</param>
        public DiagnosticActivityCollection(List<DiagnosticActivity> diagnosticActivityList)
        {
            if (diagnosticActivityList != null)
            {
                this._items = new Collection<DiagnosticActivity>(diagnosticActivityList);
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Get Xml representation of DiagnosticActivity Collection
        /// </summary>
        /// <returns>Xml representation of DiagnosticActivity Collection</returns>
        public String ToXml()
        {
            String diagnosticActivityCollectionXml = String.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //Parameter node start
            xmlWriter.WriteStartElement("DiagnosticActivities");

            if (this._items != null && this._items.Count > 0)
            {
                foreach (DiagnosticActivity item in this._items)
                {
                    xmlWriter.WriteRaw(item.ToXml());
                }
            }

            //Param node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //Get the actual XML
            diagnosticActivityCollectionXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return diagnosticActivityCollectionXml;
        }

        /// <summary>
        /// GetByReferenceId
        /// </summary>
        /// <param name="index"></param>
        /// <returns>DiagnosticActivity</returns>
        public DiagnosticActivity GetByReferenceId(long index)
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
        /// 
        /// </summary>
        /// <param name="activityId"></param>
        /// <returns></returns>
        public DiagnosticActivity GetByActivityId(Guid activityId)
        {
            DiagnosticActivity diagnosticActivity = null;

            if (activityId != Guid.Empty)
            {
                foreach (var item in this._items)
                {
                    if (item.ActivityId == activityId)
                    {
                        diagnosticActivity = item;
                        break;
                    }
                }
            }

            return diagnosticActivity;
        }

        /// <summary>
        /// Loads Diagnostic Activity Collection from Stream using Xml Reader
        /// </summary>
        /// <param name="reader"></param>
        public void LoadFromStream(XmlReader reader)
        {
            if (reader != null)
            {
                if (reader.IsEmptyElement)
                {
                    return;
                }

                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "DiagnosticActivity")
                    {
                        DiagnosticActivity diagnosticActivity = new DiagnosticActivity();

                        diagnosticActivity.LoadFromStream(reader);

                        Add(diagnosticActivity);
                    }
                    else if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "DiagnosticActivities")
                    {
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Removes element in collection at specified index
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
