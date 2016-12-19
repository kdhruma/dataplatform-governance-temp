using System;
using System.IO;
using System.Xml;

namespace MDM.BusinessObjects.Workflow
{
    using MDM.BusinessObjects.Interfaces.Workflow;

    /// <summary>
    /// A class containing information about invokable workflow entity
    /// </summary>
    public class WorkflowInvokableEntityInfo : IWorkflowInvokableEntityInfo
    {
        #region Fields

        /// <summary>
        /// A field denoting current entity id
        /// </summary>
        private Int64 _entityId = 0;

        /// <summary>
        /// A field denoting workflow invokaleble entity id
        /// </summary>
        private Int64 _workflowInvokableEntityId = 0;
        
        /// <summary>
        /// A field denoting if current entity is in workflow or not
        /// </summary>
        private Boolean _isEntityInWorkflow = false;

        #endregion Fields

        #region Constructors
        #endregion Constructors

        #region Properties

        /// <summary>
        /// Indicates current entity id
        /// </summary>
        public Int64 EntityId
        {
            get 
            {
                return _entityId; 
            }
            set 
            {
                _entityId = value; 
            }
        }

        /// <summary>
        /// Indicates workflow invokable entity id
        /// </summary>
        public Int64 WorkflowInvokableEntityId
        {
            get 
            { 
                return _workflowInvokableEntityId; 
            }
            set 
            {
                _workflowInvokableEntityId = value;
            }
        }

        /// <summary>
        /// Indicates if current entity is in workflow or not
        /// </summary>
        public Boolean IsEntityInWorkflow
        {
            get 
            { 
                return _isEntityInWorkflow; 
            }
            set 
            { 
                _isEntityInWorkflow = value; 
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Gets Xml representation of the WorkflowInvokableEntityInfo object
        /// </summary>
        /// <returns>Xml string representing the WorkflowInvokableEntityInfo</returns>
        public String ToXml()
        {
            String returnXml = String.Empty;

            using (StringWriter sw = new StringWriter())
            {
                using (XmlTextWriter xmlWriter = new XmlTextWriter(sw))
                {
                    //WorkflowInvokableEntityInfo node start
                    xmlWriter.WriteStartElement("WorkflowInvokableEntityInfo");

                    #region write WorkflowInvokableEntityInfo

                    xmlWriter.WriteAttributeString("EntityId", this.EntityId.ToString());
                    xmlWriter.WriteAttributeString("WorkflowInvokableEntityId", this.WorkflowInvokableEntityId.ToString());
                    xmlWriter.WriteAttributeString("IsEntityInWorkflow", this.IsEntityInWorkflow.ToString());

                    #endregion write WorkflowInvokableEntityInfo

                    //WorkflowInvokableEntityInfo node end
                    xmlWriter.WriteEndElement();
                    xmlWriter.Flush();

                    //get the actual XML
                    returnXml = sw.ToString();
                }
            }

            return returnXml;
        }

        #endregion Methods
    }
}
