using MDM.BusinessObjects.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MDM.BusinessObjects
{
    /// <summary>
    /// Specifies application context object mapping collection
    /// </summary>
    [DataContract]
    public class ApplicationContextObjectMappingCollection : InterfaceContractCollection<IApplicationContextObjectMapping, ApplicationContextObjectMapping>, IApplicationContextObjectMappingCollection
    {
        #region Fields

        #endregion Fields

        #region Methods

        #region Public Methods

        /// <summary>
        /// Get application context object mapping by given Id
        /// </summary>
        /// <param name="applicationContextObjectMappingId">Indicates identifier of application context object mapping.</param>
        /// <returns>Returns application context object mapping by given Id.</returns>
        public IApplicationContextObjectMapping GetById(Int32 applicationContextObjectMappingId)
        {
            if (applicationContextObjectMappingId > 0 && this._items != null && this._items.Count > 0)
            {
                foreach (var item in this._items)
                {
                    if (item.Id == applicationContextObjectMappingId)
                    {
                        return item;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Get application context object mapping by given reference Id
        /// </summary>
        /// <param name="objectMappingReferenceId">Indicates reference identifier of application context object mapping.</param>
        /// <returns>Returns application context object mapping by given Id.</returns>
        public IApplicationContextObjectMapping GetByReferenceId(String objectMappingReferenceId)
        {
            if (!String.IsNullOrWhiteSpace(objectMappingReferenceId) && this._items != null && this._items.Count > 0)
            {
                String referenceId = objectMappingReferenceId.ToString();
                foreach (var item in this._items)
                {
                    if (String.Compare(item.ReferenceId, referenceId) == 0)
                    {
                        return item;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Gets application context object mapping based on provided application context id
        /// </summary>
        /// <param name="applicationContextId">Indictes application context id</param>
        /// <returns>Returns IApplicationContextObjectMappingCollection</returns>
        public IApplicationContextObjectMappingCollection GetByApplicationContextId(Int32 applicationContextId)
        {
            ApplicationContextObjectMappingCollection objectMappings = null;
            
            if (applicationContextId > 0 && this._items != null && this._items.Count > 0)
            {
                objectMappings = new ApplicationContextObjectMappingCollection();

                foreach (ApplicationContextObjectMapping item in this._items)
                {
                    if (item.ApplicationContextId == applicationContextId)
                    {
                        objectMappings.Add(item);
                    }
                }
            }

            return objectMappings;
        }

        /// <summary>
        /// Represents application context object mapping collection in Xml format 
        /// </summary>
        /// <returns>Returns application context object mapping collection in Xml format as string.</returns>
        public String ToXml()
        {
            String xml = String.Empty;
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            xmlWriter.WriteStartElement("ApplicationContextObjectMappings");

            if (this._items != null)
            {
                foreach (ApplicationContextObjectMapping applicationContextObjectMapping in this._items)
                {
                    xmlWriter.WriteRaw(applicationContextObjectMapping.ToXml());
                }
            }

            //ApplicationContextObjectMappings node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //get the actual XML
            xml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return xml;
        }

        #endregion Public Methods

        #endregion Methods
    }
}
