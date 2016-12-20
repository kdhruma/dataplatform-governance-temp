using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.ComponentModel;

namespace RS.MDM.ConfigurationObjects
{
    [XmlRoot("WorkflowSearchConfiguration")]
    [Serializable()]
    public class WorkflowSearchConfiguration : Object
    {
        private String _workflowName = String.Empty;
        private Boolean _returnWorkflowResult = false;

        [XmlAttribute("WorkflowName")]
        [Category("WorkflowSearchConfiguration")]
        [Description("Property denoting workflow name configured for search")]
        public String WorkflowName
        {
            get
            {
                return _workflowName;
            }
            set
            {
                _workflowName = value;
            }
        }

        [XmlAttribute("ReturnWorkflowResult")]
        [Category("WorkflowSearchConfiguration")]
        [Description("Property denoting enable/disable workflow result display on grid")]
        public Boolean ReturnWorkflowResult
        {
            get
            {
                return _returnWorkflowResult;
            }
            set
            {
                _returnWorkflowResult = value;
            }
        }

           /// <summary>
        /// Initializes a new instance of WorkflowSearchConfiguration class.
        /// </summary>
        public WorkflowSearchConfiguration()
            : base()
        {

        }
        /// <summary>
        /// Initializes a new instance of WorkflowSearchConfiguration class.
        /// </summary>
        public WorkflowSearchConfiguration(String workflowName, Boolean returnWorkflowResult)
            
        {
            _workflowName = workflowName;
            _returnWorkflowResult = returnWorkflowResult;
          
        }

        #region Serialization & Deserialization

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        [System.Runtime.Serialization.OnDeserialized()]
        private void OnDeserialized(System.Runtime.Serialization.StreamingContext context)
        {
            this.SetParent();
        }

        #endregion  

        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
            {
                if (obj is WorkflowSearchConfiguration)
                {
                    WorkflowSearchConfiguration objectToBeCompared = obj as WorkflowSearchConfiguration;

                    if (!this.WorkflowName.Equals(objectToBeCompared.WorkflowName))
                        return false;

                    if (this.ReturnWorkflowResult != objectToBeCompared.ReturnWorkflowResult)
                        return false;

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Serves as a hash function for type
        /// </summary>
        /// <returns>A hash code for the current Object.</returns>
        public override int GetHashCode()
        {
            Int32 hashCode = base.GetHashCode() ^ this.WorkflowName.GetHashCode() ^ this.ReturnWorkflowResult.GetHashCode();

            return hashCode;
        }
    }
}
