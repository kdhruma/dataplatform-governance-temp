using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using MDM.BusinessObjects.DQM;
using MDM.Core;
using MDM.Interfaces;
using System.Text;

namespace MDM.BusinessObjects
{
    /// <summary>
    /// collection of server information
    /// </summary>
    [DataContract]
    public class ServerInfoCollection: InterfaceContractCollection<IServerInfo, ServerInfo>, IServerInfoCollection
    {
        #region Public Methods

        /// <summary>
        /// 
        /// </summary>
        public ServerInfoCollection()
        {
        }

        /// <summary>
        /// Replaces old serverInfo collection and sets serverInfo collection to current server info collection.
        /// </summary>
        /// <param name="serverInfos">Collection of server info</param>
        public void SetServerInfo(Collection<ServerInfo> serverInfos)
        {
            if (serverInfos != null)
            {
                this._items = new Collection<ServerInfo>(serverInfos);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loadXml"></param>
        public ServerInfoCollection(String loadXml)
        {
            var xelement = new XmlDocument();

            xelement.LoadXml(loadXml);

            _items = new Collection<ServerInfo>();

            foreach (XmlNode cnode in xelement.FirstChild.ChildNodes)
            {
                var sinfo = new ServerInfo(cnode.OuterXml);
                _items.Add(sinfo);
            }
        }

        /// <summary>
        /// Get Xml representation of server info collection object
        /// </summary>
        /// <returns>Xml string server info collection</returns>
        public String ToXml()
        {
            var xmlout = new StringBuilder(); //new XmlOutput();

           //var snode =  xmlout.Node("ServersInfo").Within();
            xmlout.Append("<ServersInfo>");

           foreach (var sinfo in _items)
           {
               xmlout.Append(sinfo.ToXml());
           }

           xmlout.Append("</ServersInfo>");
           //snode.EndWithin();

           return xmlout.ToString(); //snode.GetOuterXml();

            /*
            String serverInfoXml = String.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);
            xmlWriter.WriteStartElement("ServersInfo");

            foreach (ServerInfo serverInfo in this._items)
            {
                xmlWriter.WriteRaw(serverInfo.ToXml());
            }

            xmlWriter.WriteEndElement();
            xmlWriter.Flush();

            serverInfoXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return serverInfoXml;
             * */
        }

        /// <summary>
        /// Clone server info collection .
        /// </summary>
        /// <returns>cloned server info collection  object.</returns>
        public IServerInfoCollection Clone()
        {
            ServerInfoCollection clonedServerInfos = new ServerInfoCollection();

            if (this._items != null && this._items.Count > 0)
            {
                foreach (ServerInfo serverInfo in this._items)
                {
                    ServerInfo clonedServerInfo = serverInfo.Clone();
                    clonedServerInfos.Add(clonedServerInfo);
                }
            }
            return clonedServerInfos;
        }

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">ServerInfo collection Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override Boolean Equals(object obj)
        {
            if (obj is ServerInfoCollection)
            {
                ServerInfoCollection objectToBeCompared = obj as ServerInfoCollection;

                if (this._items.Count == objectToBeCompared._items.Count)
                {
                    for (Int32 i = 0; i < this._items.Count; i++)
                    {
                        if (!this._items[i].Equals(objectToBeCompared._items[i]))
                            return false;
                    }

                    return true;
                }
                else
                    return false;
            }

            return false ;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            Int32 hashCode = 0;
            foreach (ServerInfo attr in this._items)
            {
                hashCode += attr.GetHashCode();
            }
            return hashCode;
        }

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="objectToBeCompared">ServerInfo collection Object to compare with the current Object.</param>
        /// <param name="compareIds">Flag to say whether Id based properties has to be considered in comparison or not.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public Boolean IsSuperSerOf(ServerInfoCollection objectToBeCompared, Boolean compareIds = false)
        {
            if (this._items.Count == objectToBeCompared._items.Count)
            {
                for (Int32 i = 0; i < this._items.Count; i++)
                {
                    if (!this._items[i].IsSuperSetOf(objectToBeCompared._items[i], compareIds))
                        return false;
                }
            }
            else
                return false;

            return true;
        }

        /// <summary>
        /// Gets server info records based on server id and server short name.
        /// </summary>
        /// <returns>ServerInfo object from collection</returns>
        public ServerInfo GetServerInfo(Int16 serverId, String shortName)
        {
            ServerInfo serverInfo = null;

            if (_items != null)
            {
                foreach (ServerInfo items in this._items)
                {
                    if (items.Id == serverId && items.Name == shortName)
                    {
                        serverInfo = items;
                        break;
                    }
                }
            }

            return serverInfo;
        }

        /// <summary>
        /// Gets server info records based on server short name.
        /// </summary>
        /// <returns>ServerInfo object from collection</returns>
        public ServerInfo GetServerInfo(String shortName)
        {
            ServerInfo serverInfo = null;

            if (_items != null)
            {
                foreach (ServerInfo items in this._items)
                {
                    if (items.Name == shortName)
                    {
                        serverInfo = items;
                        break;
                    }
                }
            }

            return serverInfo;
        }

        /// <summary>
        /// Get ServerInfo by Id
        /// </summary>
        /// <param name="serverId">Indicates Id of the Server</param>
        /// <returns>Server Info</returns>
        public ServerInfo GetServerInfo(Int16 serverId)
        {
            if (_items != null && _items.Count > 0)
            {
                foreach (ServerInfo item in this._items)
                {
                    if (item.Id == serverId)
                    {
                        return item;
                    }
                }
            }

            return null;
        }

        #endregion

        #region Private Methods

        private void LoadServerInfoCollection(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "ServerInfo")
                        {
                            #region Read server info Properties

                            String serverInfoXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(serverInfoXml))
                            {
                                ServerInfo serverInfo = new ServerInfo(serverInfoXml);
                                if (serverInfo != null)
                                {
                                    this.Add(serverInfo);
                                }
                            }
                            #endregion
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
    }
}
