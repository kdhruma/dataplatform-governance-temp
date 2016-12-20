using System;
using System.IO;
using System.Xml;

namespace MDM.BusinessObjects.Workflow
{
    using MDM.BusinessObjects.Interfaces;
    using MDM.BusinessObjects.Interfaces.Workflow;
    using MDM.Core.Exceptions;

    /// <summary>
    /// A class indicating collection of WorkflowInvokableEntityInfo
    /// </summary>
    public class WorkflowInvokableEntityInfoCollection : InterfaceContractCollection<IWorkflowInvokableEntityInfo, WorkflowInvokableEntityInfo>, IWorkflowInvokableEntityInfoCollection
    {
        #region Fields
        #endregion Fields

        #region Constructors
        #endregion Constructor

        #region Properties
        #endregion Properties

        #region Methods
        
        /// <summary>
        /// Gets Xml representation of the WorkflowInvokableEntityInfoCollection object
        /// </summary>
        /// <returns>Xml string representing the WorkflowInvokableEntityInfoCollection</returns>
        public String ToXml()
        {
            String returnXml = String.Empty;

            using (StringWriter sw = new StringWriter())
            {
                using (XmlTextWriter xmlWriter = new XmlTextWriter(sw))
                {
                    //WorkflowInvokableEntityInfoCollection node start
                    xmlWriter.WriteStartElement("WorkflowInvokableEntityInfoCollection");

                    #region Write WorkflowInvokableEntityInfoCollection
                    
                    if (_items != null)
                    {
                        foreach (WorkflowInvokableEntityInfo item in this._items)
                        {
                            xmlWriter.WriteRaw(item.ToXml());
                        }
                    }

                    #endregion Write WorkflowInvokableEntityInfoCollection

                    //WorkflowInvokableEntityInfoCollection node end
                    xmlWriter.WriteEndElement();
              
                    xmlWriter.Flush();

                    //Get the actual XML
                    returnXml = sw.ToString();
                }
            }

            return returnXml;
        }

        /// <summary>
        /// Gets WorkflowInvokableEntityInfo based on given entity id
        /// </summary>
        /// <param name="entityId">Indicates entity id</param>
        /// <returns>Returns matched WorkflowInvokableEntityInfo</returns>
        public WorkflowInvokableEntityInfo GetByEntityId(Int64 entityId)
        {
            WorkflowInvokableEntityInfo matchedInfo = null;

            if (this._items.Count > 0)
            {
                foreach (WorkflowInvokableEntityInfo info in this._items)
                {
                    if (info.EntityId == entityId)
                    {
                        matchedInfo = info;
                        break;
                    }
                }
            }

            if(matchedInfo == null)
            {
                throw new Exception("Provided entity id is invalid as it is not present in the system.");
            }

            return matchedInfo;
        }

        #endregion Methods
    }
}
