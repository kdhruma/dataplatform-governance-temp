using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using MDM.BusinessObjects.DQM;
using MDM.Core;
using MDM.Interfaces;

namespace MDM.BusinessObjects
{
    /// <summary>
    /// Indicated information related to a server
    /// </summary>
    [DataContract]
    public class ServerInfo : MDMObject, IServerInfo
    {
        private MDMCenterSystem _serverType;
        private String _virtualDirectory;
        private ServiceStatus _serverStatus;


        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public ServerInfo() : base() { }

        /// <summary>
        /// Parameterized constructor 
        /// </summary>
        /// <param name="serverId">id of a server</param>
        /// <param name="serverName">name of the server</param>
        /// <param name="serverLongName">Long name for the server</param>
        public ServerInfo(Int32 serverId, String serverName, String serverLongName) : 
            base()
        {
            this.Id = serverId;
            this.Name = serverName;
            this.LongName = serverLongName;
        }

        /// <summary>
        /// Parameterized constructor with values as xml
        /// </summary>
        /// <param name="valuesAsXml">values as xml for object</param>
        public ServerInfo(String valuesAsXml)
            : base()
        {
            ServerStatus = new ServiceStatus();
            LoadServerInfo(valuesAsXml);
        }

        #endregion

        #region Properties

        ///// <summary>
        ///// Indicates Id of a server
        ///// </summary>
        //public new Int16 Id
        //{
        //    get { return _id; }
        //    set { _id = value; }
        //}


        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public MDMCenterSystem ServerType
        {
            get { return _serverType; }
            set { _serverType = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public String VirtualDirectory
        {
            get { return _virtualDirectory; }
            set { _virtualDirectory = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public ServiceStatus ServerStatus
        {
            get { return _serverStatus; }
            set { _serverStatus = value; }
        }
        
        #endregion
       
        #region Methods

        #region Public Methods

        /// <summary>
        /// Represents Server info  in Xml format
        /// </summary>
        /// <returns>String representation of current ServerInfo object</returns>
        public override String ToXml()
        {
            var xmlout = new XmlOutput();

            xmlout.Node("ServerInfo").Within()
                .Attribute("Id",  Id.ToString())
                .Attribute("Name", (!String.IsNullOrEmpty(Name) ? Name : String.Empty))
                .Attribute("LongName", (!String.IsNullOrEmpty(LongName) ? LongName : String.Empty))
                .Attribute("VirtualDirectory",  (!String.IsNullOrEmpty(VirtualDirectory) ? VirtualDirectory : String.Empty))
                .Attribute("ServerType", ServerType.ToString())
                .Node("ServerStatus").InnerText((ServerStatus != null) ? ServerStatus.ToXml() : String.Empty, true)
                .EndWithin();

            return xmlout.GetOuterXml();

            /*
            String xml = String.Empty;
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            // ServerInfo node start
            xmlWriter.WriteStartElement("ServerInfo");

            xmlWriter.WriteAttributeString("Id", this.Id.ToString());
            xmlWriter.WriteAttributeString("Name", this.Name);
            xmlWriter.WriteAttributeString("LongName", this.LongName);

            // ServerInfo end node
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            // get the actual XML
            xml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return xml;
             * */
        }

        /// <summary>
        /// clones the object to another object
        /// </summary>
        /// <returns></returns>
        public ServerInfo Clone()
        {
            ServerInfo clonedServerInfo = new ServerInfo();

            clonedServerInfo.Id = this.Id;
            clonedServerInfo.Name = this.Name;
            clonedServerInfo.LongName = this.LongName;
            clonedServerInfo.ServerType = this.ServerType;
            clonedServerInfo.VirtualDirectory = this.VirtualDirectory;
            clonedServerInfo.ServerStatus = this.ServerStatus.Clone();

            return clonedServerInfo;
        }

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="objectToBeCompared">ServerInfo Object to compare with the current Object.</param>
        public Boolean Equals(ServerInfo objectToBeCompared)
        {
            if (this.Id != objectToBeCompared.Id)
                return false;

            if (this.Name != objectToBeCompared.Name)
                return false;

            if (this.LongName != objectToBeCompared.LongName)
                return false;

            return true;
        }

        /// <summary>
        /// Determines whether one Object is a superset of another object or not.
        /// </summary>
        /// <param name="objectToBeCompared">ServerInfo Object to compare with the current Object.</param>
        /// <param name="compareIds">Flag to say whether Id based properties has to be considered in comparison or not.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public Boolean IsSuperSetOf(ServerInfo objectToBeCompared, Boolean compareIds = false)
        {
            if (compareIds)
            {
                if (this.Id != objectToBeCompared.Id)
                    return false;
            }

            if (this.Name != objectToBeCompared.Name)
                return false;

            if (this.LongName != objectToBeCompared.LongName)
                return false;

            return true;
        }

        /// <summary>
        /// Serves as a hash function for type
        /// </summary>
        /// <returns>A hash code for the current Object.</returns>
        public override int GetHashCode()
        {
            int hashCode = base.GetHashCode() ^ this.Id.GetHashCode() ^
                        this.Name.GetHashCode() ^
                        this.LongName.GetHashCode();

            return hashCode;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public String GetUrl()
        {
            return Name + "/" + VirtualDirectory;
        }

        #endregion

        #region Private Methods

        private void LoadServerInfo(String valuesAsXml)
        {
            var xelement = new XmlDocument();

            xelement.LoadXml(valuesAsXml);

            foreach (XmlAttribute attr in xelement.FirstChild.Attributes)
            {
                if (attr.Name.Equals("Id", StringComparison.OrdinalIgnoreCase))
                {
                    Id = ValueTypeHelper.Int32TryParse(attr.Value, Id);
                }
                else if (attr.Name.Equals("Name", StringComparison.OrdinalIgnoreCase))
                    Name = attr.Value;
                else if (attr.Name.Equals("LongName", StringComparison.OrdinalIgnoreCase))
                    LongName = attr.Value;
                else if (attr.Name.Equals("VirtualDirectory", StringComparison.OrdinalIgnoreCase))
                    VirtualDirectory = attr.Value;
                else if (attr.Name.Equals("ServerType", StringComparison.OrdinalIgnoreCase))
                    Enum.TryParse<MDMCenterSystem>(attr.Value, out _serverType);
            }

            XmlNode cnode = xelement.FirstChild.FirstChild;

            if (cnode.NodeType == XmlNodeType.CDATA)
            {
                var sxml = cnode.InnerText;
                if (!String.IsNullOrEmpty(sxml))
                    ServerStatus = new ServiceStatus(sxml);
            }


            /*
            XmlTextReader reader = null;
            try
            {
                reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                while (!reader.EOF)
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "ServerInfo")
                    {
                        #region Read server info Properties

                        if (reader.HasAttributes)
                        {
                            if (reader.MoveToAttribute("Id"))
                            {
                                Int16 serverId = -1;
                                Int16.TryParse(reader.ReadContentAsString(), out serverId);
                                this.Id = serverId;
                            }

                            if (reader.MoveToAttribute("Name"))
                            {
                                this.Name = reader.ReadContentAsString();
                            }

                            if (reader.MoveToAttribute("LongName"))
                            {
                                this.LongName = reader.ReadContentAsString();
                            }
                        }

                        #endregion
                    }
                    else
                    {
                        //Keep on reading the xml until we reach expected node.
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
             * */
        }

        #endregion
        
        #endregion
    }
}
