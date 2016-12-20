using System;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using MDM.BusinessObjects.DQM;
using MDM.Core;
using MDM.Interfaces;

namespace MDM.BusinessObjects
{   
    /// <summary>
    /// Represent Business Object for ServiceStatus
    /// </summary>
    [DataContract]
    public class ServiceStatus: IServiceStatus
    {
        #region Fields

        /// <summary>
        /// Field denoting Name of the Server
        /// </summary>
        private String _server = String.Empty;

        /// <summary>
        /// Field denoting Type of service
        /// </summary>
        private MDMServiceType _service = MDMServiceType.UnKnown;

        /// <summary>
        /// Field denoting subType of Service
        /// </summary>
        private MDMServiceSubType _serviceSubType = MDMServiceSubType.UnKnown;

        /// <summary>
        /// Field denoting status of the server in form of XML
        /// </summary>
        private String _serviceStatusXML = String.Empty;

        /// <summary>
        /// Field denoting Config of the server in form of XML
        /// </summary>
        private String _serviceConfigXML = String.Empty;

        /// <summary>
        /// Field denoting Modified Datetime of the Server
        /// </summary>
        private DateTime _modifiedDateTime = DateTime.Now;

        #endregion

        #region Contructors

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public ServiceStatus()
        {
            Server = String.Empty;
            Service = MDMServiceType.UnKnown;
            ServiceSubType = MDMServiceSubType.UnKnown;
            ServiceStatusXML = String.Empty;
            ServiceConfigXML = String.Empty;
            ModifiedDateTime = DateTime.MinValue;

        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="server">Indicates the name of the server</param>
        /// <param name="service">Indicates the name of the service</param>
        /// <param name="serviceSubType">Indicates the type of the service</param>
        /// <param name="serviceStatus">Indicates the status of the service</param>
        /// <param name="serviceConfig">Indicates the config of the service</param>
        /// <param name="modifiedDateTime">Indicates the modified Date and Time</param>
        public ServiceStatus(String server, MDMServiceType service, MDMServiceSubType serviceSubType, String serviceStatus, String serviceConfig, DateTime modifiedDateTime)
        {
            this.Server = server;
            this.Service = service;
            this.ServiceSubType = serviceSubType;
            this.ServiceStatusXML = serviceStatus;
            this.ServiceConfigXML = serviceConfig;
            this.ModifiedDateTime = modifiedDateTime;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valueXml"></param>
        public ServiceStatus(String valueXml)
        {
            Server = String.Empty;
            Service = MDMServiceType.UnKnown;
            ServiceSubType = MDMServiceSubType.UnKnown;
            ServiceStatusXML = String.Empty;
            ServiceConfigXML = String.Empty;
            ModifiedDateTime = DateTime.MinValue;

            LoadFromXml(valueXml);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Property denoting Name of the Server
        /// </summary>
        [DataMember]
        public String Server
        {
            get { return this._server; }
            set { this._server = value; }
        }

        /// <summary>
        /// Property denoting Type of service
        /// </summary>
        [DataMember]
        public MDMServiceType Service
        {
            get { return this._service; }
            set { this._service = value; }
        }

        /// <summary>
        /// Property denoting subType of Service
        /// </summary>
        [DataMember]
        public MDMServiceSubType ServiceSubType
        {
            get { return this._serviceSubType; }
            set { this._serviceSubType = value; }
        }

        /// <summary>
        /// Property denoting status of the server in form of XML
        /// </summary>
        [DataMember]
        public String ServiceStatusXML
        {
            get { return this._serviceStatusXML; }
            set { this._serviceStatusXML = value; }
        }

        /// <summary>
        /// Property denoting status of the server in form of XML
        /// </summary>
        [DataMember]
        public String ServiceConfigXML
        {
            get { return this._serviceConfigXML; }
            set { this._serviceConfigXML = value; }
        }

        /// <summary>
        /// Property denoting Modified Datetime of the Server
        /// </summary>
        [DataMember]
        public DateTime ModifiedDateTime
        {
            get { return this._modifiedDateTime; }
            set { this._modifiedDateTime = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loadXml"></param>
        public void LoadFromXml(String loadXml)
        {
            var xelement = new XmlDocument();

            xelement.LoadXml(loadXml);

            foreach (XmlAttribute attr in xelement.FirstChild.Attributes)
            {
                if (attr.Name == "Server")
                    Server = attr.Value;
                else if (attr.Name == "Service")
                    Enum.TryParse<MDMServiceType>(attr.Value, out _service);
                else if (attr.Name == "ServiceSubType")
                    Enum.TryParse<MDMServiceSubType>(attr.Value, out _serviceSubType);
                else if (attr.Name == "ModifiedDateTime")
                    ModifiedDateTime = ValueTypeHelper.ConvertToDateTime(attr.Value);
            }

            foreach (XmlNode node in xelement.ChildNodes)
            {
                if (node.Name == "ServiceStatusXml")
                {
                    if (node.FirstChild.NodeType == XmlNodeType.CDATA)
                    {
                        ServiceStatusXML = node.FirstChild.Value;
                    }
                }

                if (node.Name == "ServiceConfigXml")
                {
                    if (node.FirstChild.NodeType == XmlNodeType.CDATA)
                    {
                        ServiceConfigXML = node.FirstChild.Value;
                    }
                }
            }

        }

        /// <summary>
        /// XML representation of ServiceStatus
        /// </summary>
        /// <returns>ServiceStatus XML</returns>
        public string ToXml()
        {
            var xmlout = new XmlOutput();

            xmlout.Node("ServiceStatus").Within()
                .Attribute("Server", Server)
                .Attribute("Service", Service.ToString())
                .Attribute("ServiceSubType", ServiceSubType.ToString())
                .Attribute("ModifiedDateTime", ModifiedDateTime.ToString())
                .Node("ServiceStatusXml").InnerText(ServiceStatusXML, true)
                .Node("ServiceConfigXml").InnerText(ServiceConfigXML, true)
                .EndWithin();

            return xmlout.GetOuterXml();

            /*
            String serverStatusXml = String.Empty;
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);
            StringBuilder builder = new StringBuilder();

            //ServiceStatus node start
            xmlWriter.WriteStartElement("ServiceStatus");

            xmlWriter.WriteAttributeString("Server", this.Server);
            xmlWriter.WriteAttributeString("Service", this.Service.ToString());
            xmlWriter.WriteAttributeString("ServiceSubType", this.ServiceSubType.ToString());     
            xmlWriter.WriteAttributeString("ModifiedDateTime", this.ModifiedDateTime.ToString());

            //TODO: Need to write XML in ServiceStatus Node
            //xmlWriter.WriteStartElement("ServiceStatusXML");

            //xmlWriter.WriteString(builder.Append(this.ServiceStatusXML).ToString());

            //xmlWriter.WriteEndElement();

            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //get the actual XML
            serverStatusXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return serverStatusXml;
             * */
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ServiceStatus Clone()
        {
            var ss = new ServiceStatus();

            ss.Server = this.Server;
            ss.Service = this.Service;
            ss.ServiceConfigXML = this.ServiceConfigXML;
            ss.ServiceStatusXML = this.ServiceStatusXML;
            ss.ServiceSubType = this.ServiceSubType;

            return ss;
        }


        #endregion
    }
}
