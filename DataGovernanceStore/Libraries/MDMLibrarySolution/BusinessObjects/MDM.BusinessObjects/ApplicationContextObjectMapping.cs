using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.BusinessObjects.Interfaces;
    using MDM.Core;

    /// <summary>
    /// Specifies application context object mapping
    /// </summary>
    [DataContract]
    public class ApplicationContextObjectMapping : MDMObject, IApplicationContextObjectMapping
    {
        #region Fields

        /// <summary>
        /// Field indicates identifier of application context
        /// </summary>
        private Int32 _applicationContextId = 0;

        /// <summary>
        /// Field indicates type of application context
        /// </summary>
        private ApplicationContextType _applicationContextType = ApplicationContextType.CC;

        /// <summary>
        /// Field indicates type of object
        /// </summary>
        private Int32 _contextObjectTypeId = -1;

        /// <summary>
        /// Field indicates identifier of object
        /// </summary>
        private Int64 _objectId = 0;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Property denoting identifier of application context
        /// </summary>
        [DataMember]
        public Int32 ApplicationContextId
        {
            get { return _applicationContextId; }
            set { _applicationContextId = value; }
        }

        /// <summary>
        /// Property denoting type of application context
        /// </summary>
        [DataMember]
        public ApplicationContextType ApplicationContextType
        {
            get { return _applicationContextType; }
            set { _applicationContextType = value; }
        }

        /// <summary>
        /// Property denoting type of object
        /// </summary>
        [DataMember]
        public Int32 ContextObjectTypeId
        {
            get { return _contextObjectTypeId; }
            set { _contextObjectTypeId = value; }
        }

        /// <summary>
        /// Property denoting identifier of object
        /// </summary>
        [DataMember]
        public Int64 ObjectId
        {
            get { return _objectId; }
            set { _objectId = value; }
        }

        #endregion Properties

        #region Methods

        #region Public Methods

        /// <summary>
        /// Represents application context object mapping in Xml format 
        /// </summary>
        /// <returns>Returns application context object mapping in Xml format as string.</returns>
        public override String ToXml()
        {
            String xml = string.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            #region For Entity Variant Definition Metadata

            xmlWriter.WriteStartElement("ApplicationContectObjectMapping");
            xmlWriter.WriteAttributeString("Id", this.Id.ToString());
            xmlWriter.WriteAttributeString("ApplicationContextId", this.ApplicationContextId.ToString());
            xmlWriter.WriteAttributeString("ApplicationContextType", this.ApplicationContextType.ToString());
            xmlWriter.WriteAttributeString("ObjectTypeId", this.ContextObjectTypeId.ToString());
            xmlWriter.WriteAttributeString("ObjectId", this.ObjectId.ToString());

            xmlWriter.WriteEndElement(); //For Application Context Object Mapping

            #endregion For Entity Variant Definition Metadata

            xmlWriter.Flush();

            //get the actual XML
            xml = sw.ToString();

            xmlWriter.Close();

            sw.Close();

            return xml;
        }

        #endregion Public Methods

        #region Private Methods

        #endregion Private Methods

        #endregion Methods
    }
}
